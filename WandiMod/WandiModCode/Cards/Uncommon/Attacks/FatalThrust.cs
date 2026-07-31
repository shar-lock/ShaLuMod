using BaseLib.Extensions;                           // WithUpgrade
using BaseLib.Utils;                                // CommonActions（CardAttack）
using MegaCrit.Sts2.Core.Commands;                  // PowerCmd
using MegaCrit.Sts2.Core.GameActions.Multiplayer;   // PlayerChoiceContext
using MegaCrit.Sts2.Core.Entities.Cards;            // CardPlay / CardType / CardRarity / TargetType / CardKeyword
using MegaCrit.Sts2.Core.Localization.DynamicVars;  // DamageVar / IntVar
using MegaCrit.Sts2.Core.ValueProps;                // ValueProp
using WandiMod.WandiModCode.Character;              // WandiModCard
using WandiMod.WandiModCode.Powers;                 // FatalThrustPower（临时降力量）

using WandiMod.WandiModCode.Extensions;  // WithUpgradeTo（升级目标值语义）

namespace WandiMod.WandiModCode.Cards;

/// <summary>
/// 致命突刺 / Fatal Thrust（罕见 · 攻击 · 消耗 · 保留）
/// 造成 9 伤害；降低目标 6 点力量（1 回合）。升级：降 8 点力量。
/// —— 保留（Retain）让本牌可攒在手，等敌方爆发回合再打出破甲；消耗保证一次性（不可反复刷新）。
/// 临时降力量走 FatalThrustPower : TemporaryStrengthPower（参考原生 PiercingWail/尖啸）。
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
        new DamageVar(9, ValueProp.Move),                 // 基础伤害 9（升级不变）
        new IntVar("StrengthLoss", 6).WithUpgradeTo(8),     // 降低力量 6→8
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

        // ② 临时降低目标力量（1 回合）—— FatalThrustPower : TemporaryStrengthPower
        //    传正数 loss；TemporaryStrengthPower 内部用 Sign=-1 转为负 StrengthPower，回合结束自动恢复
        int loss = DynamicVars["StrengthLoss"].IntValue;
        await PowerCmd.Apply<FatalThrustPower>(choiceContext, cardPlay.Target, loss, Owner.Creature, this);

        MainFile.Logger.Info($"[致命突刺] 打出 {DynamicVars.Damage.BaseValue} 伤，临时降低目标 {loss} 力量（1 回合）");
    }
}
