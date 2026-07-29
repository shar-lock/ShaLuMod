using BaseLib.Extensions;                           // WithUpgrade
using BaseLib.Utils;                                // CommonActions（CardAttack）
using MegaCrit.Sts2.Core.Commands;                  // PowerCmd
using MegaCrit.Sts2.Core.GameActions.Multiplayer;   // PlayerChoiceContext
using MegaCrit.Sts2.Core.Entities.Cards;            // CardPlay / CardType / CardRarity / TargetType / CardKeyword
using MegaCrit.Sts2.Core.Localization.DynamicVars;  // DamageVar / IntVar
using MegaCrit.Sts2.Core.Models.Powers;             // StrengthPower（原生力量）
using MegaCrit.Sts2.Core.ValueProps;                // ValueProp
using WandiMod.WandiModCode.Character;              // WandiModCard

namespace WandiMod.WandiModCode.Cards;

/// <summary>
/// 致命突刺 / Fatal Thrust（罕见 · 攻击 · 消耗 · 保留）
/// 造成 14 伤害；降低目标 6 点力量（1 回合）。升级：降 8 点力量。
/// —— 保留（Retain）让本牌可攒在手，等敌方爆发回合再打出破甲；消耗保证一次性（不可反复刷新）。
///
/// TODO: 「1 回合」临时降低力量机制运行时确认——目前用 PowerCmd.Apply 负层数 StrengthPower 实现。
///   原生 StrengthPower 是 Buff 且层数持久（参考 Bloodthirst 的正向施加），这里的 -6 力量会持续到
///   战斗结束而非 1 回合。需确认游戏是否有 LoseStrengthPower / 临时 duration 钩子（原生 StS 的
///   LoseStrengthPower/ShrinkPower-power 等），确认后改为临时降低版本。
/// </summary>
public class FatalThrust : WandiModCard
{
    public FatalThrust() : base(
        cost: 1,
        type: CardType.Attack,
        rarity: CardRarity.Uncommon,
        target: TargetType.AnyEnemy)
    {
    }

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new DamageVar(14, ValueProp.Move),                // 基础伤害 14（升级不变）
        new IntVar("StrengthLoss", 6).WithUpgrade(8),     // 降低力量 6→8
    ];

    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust, CardKeyword.Retain];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (cardPlay.Target == null)
        {
            MainFile.Logger.Error("[致命突刺] OnPlay 时目标为空，效果未触发");
            return;
        }

        // ① 标准攻击（DamageVar 走 CommonActions.CardAttack，自动吃力量/血仇放大）
        await CommonActions.CardAttack(this, cardPlay).Execute(choiceContext);

        // ② 降低目标力量（负层数 StrengthPower = 减力量；TODO: 永久 vs 1 回合待确认）
        int loss = DynamicVars["StrengthLoss"].IntValue;
        await PowerCmd.Apply<StrengthPower>(choiceContext, cardPlay.Target, -loss, Owner.Creature, this);

        MainFile.Logger.Info($"[致命突刺] 打出 {DynamicVars.Damage.BaseValue} 伤，降低目标 {loss} 力量（临时降低机制 TODO）");
    }
}
