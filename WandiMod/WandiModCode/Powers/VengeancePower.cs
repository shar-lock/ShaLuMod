using HarmonyLib;                                   // Traverse / HarmonyPatch（显示补丁用）
using MegaCrit.Sts2.Core.Commands;                 // PowerCmd（失血时给自己叠层）
using MegaCrit.Sts2.Core.Entities.Cards;            // CardModel / CardPlay
using MegaCrit.Sts2.Core.Entities.Creatures;        // Creature
using MegaCrit.Sts2.Core.Entities.Powers;           // PowerType / PowerStackType
using MegaCrit.Sts2.Core.GameActions.Multiplayer;   // PlayerChoiceContext / DamageResult
using MegaCrit.Sts2.Core.HoverTips;                 // IHoverTip / HoverTipFactory
using MegaCrit.Sts2.Core.Models;                    // PowerModel
using MegaCrit.Sts2.Core.Nodes.Combat;              // NPower（显示补丁目标）
using MegaCrit.Sts2.Core.ValueProps;                // ValueProp
using MegaCrit.Sts2.addons.mega_text;               // MegaLabel（清空数字标签）
using WandiMod.WandiModCode.Cards;                  // ConquerAllLands（悬停预览）

namespace WandiMod.WandiModCode.Powers;

/// <summary>
/// 血仇（Vengeance）—— 万敌的核心计数型 Power（Buff + Counter）。
/// 机制：
///   1. 失血叠层：每当万敌失去生命 → +1 层（AfterDamageReceived）。
///   2. 伤害放大：有效层（真实−1）每层攻击 +2%（ModifyDamageMultiplicative）。
///   3. 成长消耗：累积到「上次基准 + 7」时消耗 4 层（不清空，净留 +3）；不生成荡平万邦。
///      「荡平万邦」由独立的 ConquerProgressPower 按获得量计数生成（与本 Power 消耗脱钩）。
/// </summary>
public class VengeancePower : WandiModPower
{
    private const int GainPerTrigger = 7;
    private const int ConsumeOnTrigger = 4;

    /// <summary>上次成长消耗后的血仇真实层数；下次触发 = 基准 + 7。</summary>
    private int _lastTriggerBase = 1;

    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;

    public override int DisplayAmount => Math.Max(0, Amount - 1);

    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
    [
        HoverTipFactory.FromCard<ConquerAllLands>(),
    ];

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
    /// 成长消耗：满基准+7 时扣最多 4 层。不生成卡牌、不改荡平进度（进度只跟获得量走）。
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

        // 仅正向变化触发成长消耗；负向（打牌消耗 / 本段递归消耗）忽略
        if (amount <= 0 || Amount < _lastTriggerBase + GainPerTrigger)
            return;

        int consume = Math.Min(ConsumeOnTrigger, Math.Max(0, Amount - 1));
        if (consume > 0)
            await PowerCmd.Apply<VengeancePower>(choiceContext, Owner, -consume, Owner, null);
        _lastTriggerBase = Amount;

        MainFile.Logger.Info($"[血仇] 成长消耗 {consume} 层（目标{ConsumeOnTrigger}），当前 {Amount}（显示{DisplayAmount}），下次基准 {_lastTriggerBase}+{GainPerTrigger}");
    }
}

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
