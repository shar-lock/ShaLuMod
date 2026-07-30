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

/// <summary>弑神枪·无尽 / Godslayer Endless（稀有 · 攻击 · X费 · 保留）。X段，每段=血仇层数+bonus / +2。</summary>
public class GodslayerEndless : WandiModCard
{
    public GodslayerEndless() : base(-2, CardType.Attack, CardRarity.Rare, TargetType.AnyEnemy) { }
    protected override IEnumerable<DynamicVar> CanonicalVars => [new IntVar("BonusPerHit", 0).WithUpgradeTo(2)];
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Retain, WandiModKeywords.Vengeance];
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (cardPlay.Target == null) return;
        var c = Owner.Creature;
        int blood = c.GetPower<VengeancePower>()?.Amount ?? 0;
        int bonus = DynamicVars["BonusPerHit"].IntValue;
        decimal perHit = blood + bonus;
        int hits = DynamicVars.Energy.IntValue;
        for (int i = 0; i < hits; i++)
            await DamageCmd.Attack(perHit).FromCard(this, cardPlay).Targeting(cardPlay.Target).WithValueProp(ValueProp.Move).Execute(choiceContext);
        MainFile.Logger.Info($"[弑神枪·无尽] {hits} 段 × {perHit}（血仇 {blood}+{bonus}）");
    }
}
