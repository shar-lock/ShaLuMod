using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using WandiMod.WandiModCode.Character;
using WandiMod.WandiModCode.Extensions;
using WandiMod.WandiModCode.Powers; // DeathDenialPower

namespace WandiMod.WandiModCode.Cards;

/// <summary>死亡拒绝 / Death Denial（稀有 · 技能 · 消耗）。本回合若致死，免死并回 50% 生命 / 75%。</summary>
public class DeathDenial : WandiModCard
{
    public DeathDenial() : base(1, CardType.Skill, CardRarity.Rare, TargetType.Self) { }
    protected override IEnumerable<DynamicVar> CanonicalVars => [new IntVar("HealPct", 50).WithUpgradeTo(75)];
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await PowerCmd.Apply<DeathDenialPower>(choiceContext, Owner.Creature, DynamicVars["HealPct"].IntValue, Owner.Creature, this);
        MainFile.Logger.Info($"[死亡拒绝] 本回合免死（回 {DynamicVars["HealPct"].IntValue}% 生命）");
    }
}
