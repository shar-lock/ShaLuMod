using BaseLib.Extensions;                           // WithUpgrade 扩展方法
using BaseLib.Utils;                                // CommonActions
using MegaCrit.Sts2.Core.Commands;                 // CreatureCmd
using MegaCrit.Sts2.Core.GameActions.Multiplayer;   // PlayerChoiceContext
using MegaCrit.Sts2.Core.Entities.Cards;            // CardPlay / CardType / CardRarity / TargetType
using MegaCrit.Sts2.Core.Localization.DynamicVars;  // HpLossVar / DamageVar
using MegaCrit.Sts2.Core.ValueProps;                // ValueProp

namespace WandiMod.WandiModCode.Cards;

/// <summary>
/// 残影突袭 / Afterimage Strike（普通 · 攻击）
/// 失去 2 点生命，造成 7 点伤害。升级：10 伤害。
/// —— 0 费卖血快攻：自伤经血仇钩子自动 +1 血仇并放大本张伤害（同 BloodRite 顺序）。
/// </summary>
public class AfterimageStrike : WandiModCard
{
    public AfterimageStrike() : base(
        cost: 0,
        type: CardType.Attack,
        rarity: CardRarity.Common,
        target: TargetType.AnyEnemy)
    {
    }

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new HpLossVar(2),
        new DamageVar(7, ValueProp.Move).WithUpgrade(10),
    ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        // 先自伤（触发血仇 +1）再攻击（吃放大），顺序同 Hemokinesis / BloodRite
        await CreatureCmd.Damage(choiceContext, Owner.Creature, DynamicVars.HpLoss.BaseValue,
            ValueProp.Unblockable | ValueProp.Unpowered | ValueProp.Move, this, cardPlay);
        await CommonActions.CardAttack(this, cardPlay).Execute(choiceContext);
    }
}
