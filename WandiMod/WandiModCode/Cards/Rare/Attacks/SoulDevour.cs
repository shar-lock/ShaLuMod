using BaseLib.Extensions;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using WandiMod.WandiModCode.Character;
using WandiMod.WandiModCode.Extensions;
using WandiMod.WandiModCode.Powers;

namespace WandiMod.WandiModCode.Cards;

/// <summary>噬魂 / Soul Devour（稀有 · 攻击 · 消耗）。18伤；每有1血仇+2 / 24、+3。</summary>
public class SoulDevour : WandiModCard
{
    public SoulDevour() : base(2, CardType.Attack, CardRarity.Rare, TargetType.AnyEnemy) { }
    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new DamageVar(18, ValueProp.Move).WithUpgradeTo(24),
        new IntVar("BonusPerBlood", 2).WithUpgradeTo(3),
    ];
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust, WandiModKeywords.Vengeance];
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (cardPlay.Target == null) return;
        var c = Owner.Creature;
        int blood = c.GetPower<VengeancePower>()?.Amount ?? 0;
        decimal dmg = DynamicVars.Damage.BaseValue + blood * DynamicVars["BonusPerBlood"].IntValue;
        await DamageCmd.Attack(dmg).FromCard(this, cardPlay).Targeting(cardPlay.Target).WithValueProp(ValueProp.Move).Execute(choiceContext);
        MainFile.Logger.Info($"[噬魂] 基础 {DynamicVars.Damage.BaseValue} + 血仇 {blood}×{DynamicVars["BonusPerBlood"].IntValue} = {dmg}");
    }
}
