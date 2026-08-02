using HarmonyLib;                                   // Traverse / HarmonyPatch（显示补丁用）
using MegaCrit.Sts2.Core.Commands;                 // PowerCmd（失血时给自己叠层）
using MegaCrit.Sts2.Core.Entities.Cards;            // CardModel / CardPlay
using MegaCrit.Sts2.Core.Entities.Creatures;        // Creature
using MegaCrit.Sts2.Core.Entities.Powers;           // PowerType / PowerStackType
using MegaCrit.Sts2.Core.GameActions.Multiplayer;   // PlayerChoiceContext / DamageResult
using MegaCrit.Sts2.Core.HoverTips;                 // IHoverTip / HoverTipFactory（悬停预览荡平万邦）
using MegaCrit.Sts2.Core.Models;                    // PowerModel（M2.3 钩子参数用）
using MegaCrit.Sts2.Core.Nodes.Combat;              // NPower（显示补丁目标）
using MegaCrit.Sts2.Core.ValueProps;                // ValueProp
using MegaCrit.Sts2.addons.mega_text;               // MegaLabel（清空数字标签）
using WandiMod.WandiModCode.Cards;                  // ConquerAllLands（8 层触发生成荡平万邦）
using WandiMod.WandiModCode.Relics;                 // UndyingRoyalBlood（觉醒判定）

namespace WandiMod.WandiModCode.Powers;

/// <summary>
/// 血仇（Vengeance）—— 万敌的核心计数型 Power（Buff + Counter）。
/// 机制：
///   1. 失血叠层：每当万敌失去生命（自伤或受击）→ +1 层（钩子 AfterDamageReceived，参考原生 RupturePower）。
///   2. 伤害放大：每层【有效】血仇使万敌的攻击牌伤害 +2%（钩子 ModifyDamageMultiplicative，参考原生 WeakPower）。
///      「有效层数 = 真实层数 - 1」——保底那 1 层是结构性储备（维持 Power 存活），不提供任何增伤。
///      由起手遗物「弑亲血脉」在战斗开始时赋予（BeforeCombatStart，初始 1 层 = 0 有效层）。
///   3. 成长型触发：血仇累积到「上次基准 + 7」时消耗 4 层、生成「荡平万邦」到手牌（钩子 AfterPowerAmountChanged）。
///      不再扣除 5% 当前生命。消耗 4 &lt; 累积 7 → 净留 +3，触发基准逐次抬高（触发点 8→11→14...）。
///      进度由独立 Power「荡平进度 / ConquerProgressPower」展示（0～7）。
/// </summary>
public class VengeancePower : WandiModPower
{
    /// <summary>每次触发需新累积的层数（基准线 +7 才触发）。</summary>
    private const int GainPerTrigger = 7;
    /// <summary>每次触发固定消耗的层数（不清空；消耗 4 &lt; 累积 7 → 净留 +3，血仇逐轮成长）。</summary>
    private const int ConsumeOnTrigger = 4;

    /// <summary>
    /// 成长型触发基准线：上次触发「消耗后」的血仇值。下次触发条件 = 基准 + GainPerTrigger。
    /// 初始 1（= 开局血仇）→ 首次触发在 8 层；每次触发后基准抬高 → 触发点 8→11→14...，血仇净成长 +3/轮。
    /// </summary>
    private int _lastTriggerBase = 1;

    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;

    /// <summary>
    /// 状态栏显示数字 = 真实层数 - 1。保底 1 层（真实）对应显示值 0（= 不展示数字，见 VengeancePowerDisplayPatch）。
    /// </summary>
    public override int DisplayAmount => Math.Max(0, Amount - 1);

    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
    [
        HoverTipFactory.FromCard<ConquerAllLands>(),
    ];

    /// <summary>授予血仇（卡牌调用的统一入口）：叠 Counter 层数。</summary>
    public static async Task Grant(PlayerChoiceContext choiceContext, Creature? target, decimal amount, CardModel? cardSource = null)
    {
        if (target == null)
        {
            MainFile.Logger.Error($"[血仇] Grant 收到空 target（amount={amount}, cardSource={cardSource?.Id.Entry ?? "null"}），已忽略");
            return;
        }
        if (amount <= 0)
        {
            MainFile.Logger.Warn($"[血仇] Grant 收到非正层数 {amount}（cardSource={cardSource?.Id.Entry ?? "null"}），已忽略");
            return;
        }
        await PowerCmd.Apply<VengeancePower>(choiceContext, target, amount, target, cardSource);
    }

    /// <summary>
    /// 失血叠层：万敌每次实际掉血（未格挡伤害 > 0）时，给自己 +1 血仇。
    /// 参考 RupturePower.AfterDamageReceived。
    /// </summary>
    public override async Task AfterDamageReceived(
        PlayerChoiceContext choiceContext,
        Creature target,
        DamageResult result,
        ValueProp props,
        Creature? dealer,
        CardModel? cardSource)
    {
        if (target != Owner || result.UnblockedDamage <= 0)
            return;

        await PowerCmd.Apply<VengeancePower>(choiceContext, Owner, 1, Owner, null);
    }

    /// <summary>
    /// 伤害放大器：每层血仇使万敌打出的攻击牌伤害 +2%（乘法）。
    /// 有效层数（真实-1）× 2%。
    /// </summary>
    public override decimal ModifyDamageMultiplicative(
        Creature? target,
        decimal amount,
        ValueProp props,
        Creature? dealer,
        CardModel? cardSource,
        CardPlay? cardPlay)
    {
        if (dealer != Owner || !props.IsPoweredAttack())
            return 1m;

        int effective = Math.Max(0, Amount - 1);
        return 1m + 0.02m * effective;
    }

    /// <summary>
    /// 成长型触发荡平万邦：血仇累积到「上次基准 + 7」时消耗 4 层（不清空）、生成「荡平万邦」到手牌。
    /// 不再扣 5% 当前血。进度同步到 ConquerProgressPower。
    /// </summary>
    public override async Task AfterPowerAmountChanged(
        PlayerChoiceContext choiceContext,
        PowerModel power,
        decimal amount,
        Creature? applier,
        CardModel? cardSource)
    {
        if (power != this)
            return;

        // 正向变化且达到阈值 → 触发（负向变化只同步进度，避免「消耗→触发」递归）
        if (amount > 0 && Amount >= _lastTriggerBase + GainPerTrigger)
        {
            // 目标消耗 4 层；若不足则只减到保底真实 1 层（玩家可见 0），绝不归零移除 Power
            int consume = Math.Min(ConsumeOnTrigger, Math.Max(0, Amount - 1));
            if (consume > 0)
                await PowerCmd.Apply<VengeancePower>(choiceContext, Owner, -consume, Owner, null);
            _lastTriggerBase = Amount;

            if (CombatState == null || Owner.Player == null)
            {
                MainFile.Logger.Error("[血仇] 成长触发时 CombatState/Owner.Player 为空，荡平万邦未生成（层数已消耗）");
                SyncConquerProgress();
                return;
            }

            CardModel card = CombatState.CreateCard<ConquerAllLands>(Owner.Player);
            bool awakened = Owner.Player.GetRelic<UndyingRoyalBlood>() != null;
            if (awakened)
                CardCmd.Upgrade(card);
            await CardPileCmd.AddGeneratedCardToCombat(card, PileType.Hand, Owner.Player);

            MainFile.Logger.Info($"[血仇] 成长触发荡平万邦：消耗 {consume} 层（目标{ConsumeOnTrigger}，保底真实1层），当前 {Amount} 层（显示{DisplayAmount}），下次触发基准 {_lastTriggerBase}（需累积到 {_lastTriggerBase + GainPerTrigger}），觉醒={awakened}");
        }

        SyncConquerProgress();
    }

    /// <summary>把「距下次生成」进度同步到独立计数器 Power。</summary>
    private void SyncConquerProgress()
    {
        Owner.GetPower<ConquerProgressPower>()?.SyncProgress(Amount, _lastTriggerBase);
    }
}

/// <summary>
/// Harmony 补丁：血仇 Power 显示值 ≤ 0（即真实层数 = 1 保底）时，隐藏状态栏左下角的数字。
/// </summary>
[HarmonyPatch(typeof(NPower), "RefreshAmount")]
static class VengeancePowerDisplayPatch
{
    [HarmonyPostfix]
    static void Postfix(NPower __instance)
    {
        var model = __instance.Model;
        if (model is not VengeancePower || model.DisplayAmount > 0)
            return;

        var label = Traverse.Create(__instance).Field("_amountLabel").GetValue<MegaLabel>();
        label?.SetTextAutoSize("");
    }
}
