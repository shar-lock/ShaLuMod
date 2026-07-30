using BaseLib.Utils;                                // CommonActions
using MegaCrit.Sts2.Core.GameActions.Multiplayer;   // PlayerChoiceContext
using MegaCrit.Sts2.Core.Entities.Cards;            // CardPlay / CardType / CardRarity / TargetType
using MegaCrit.Sts2.Core.Localization.DynamicVars;  // DamageVar / IntVar
using MegaCrit.Sts2.Core.ValueProps;                // ValueProp
using WandiMod.WandiModCode.Character;              // WandiModCard
using WandiMod.WandiModCode.Extensions;             // WithUpgradeTo
using WandiMod.WandiModCode.Powers;                 // StrifePower

namespace WandiMod.WandiModCode.Cards;

/// <summary>
/// 裂伤 / Lacerate（普通 · 攻击）
/// 造成 5 点伤害，获得 2 点【纷争】。升级：8 伤，3 纷争。
/// —— 攻防一体：低价攻击顺手产纷争，攻守兼顾。
/// </summary>
public class Lacerate : WandiModCard
{
    public Lacerate() : base(1, CardType.Attack, CardRarity.Common, TargetType.AnyEnemy) { }

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new DamageVar(5, ValueProp.Move).WithUpgradeTo(8),
        new IntVar("Strife", 2).WithUpgradeTo(3),
    ];

    public override IEnumerable<CardKeyword> CanonicalKeywords => [WandiModKeywords.Strife];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CommonActions.CardAttack(this, cardPlay).Execute(choiceContext);
        await StrifePower.Grant(choiceContext, Owner.Creature, DynamicVars["Strife"].IntValue, Owner.Creature, this);
    }
}
