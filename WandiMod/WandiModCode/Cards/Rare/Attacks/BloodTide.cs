using BaseLib.Extensions;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using WandiMod.WandiModCode.Character;
using WandiMod.WandiModCode.Extensions;

namespace WandiMod.WandiModCode.Cards;

/// <summary>血海狂涛 / Blood Tide（稀有 · 攻击 · 随机）。4伤×4 随机敌人 / 6×4。</summary>
public class BloodTide : WandiModCard
{
    public BloodTide() : base(1, CardType.Attack, CardRarity.Rare, TargetType.RandomEnemy) { }
    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new DamageVar(4, ValueProp.Move).WithUpgradeTo(5),
        new RepeatVar(4).WithUpgradeTo(5),
    ];
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        => await CommonActions.CardAttack(this, cardPlay).Execute(choiceContext);
}
