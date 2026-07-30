using BaseLib.Extensions;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using WandiMod.WandiModCode.Character;
using WandiMod.WandiModCode.Extensions;

namespace WandiMod.WandiModCode.Cards;

/// <summary>灾厄之矛 / Calamity Spear（稀有 · 攻击 · 消耗）。造成「已损失生命」70%/80% 的伤害。</summary>
public class CalamitySpear : WandiModCard
{
    public CalamitySpear() : base(3, CardType.Attack, CardRarity.Rare, TargetType.AnyEnemy) { }
    protected override IEnumerable<DynamicVar> CanonicalVars => [new IntVar("MissingHpPct", 70).WithUpgradeTo(80)];
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (cardPlay.Target == null) return;
        var c = Owner.Creature;
        decimal missingHp = c.MaxHp - c.CurrentHp;
        decimal dmg = missingHp * DynamicVars["MissingHpPct"].IntValue / 100m;
        await DamageCmd.Attack(dmg).FromCard(this, cardPlay).Targeting(cardPlay.Target).WithValueProp(ValueProp.Move).Execute(choiceContext);
        MainFile.Logger.Info($"[灾厄之矛] MissingHp={missingHp} → {dmg} 伤");
    }
}
