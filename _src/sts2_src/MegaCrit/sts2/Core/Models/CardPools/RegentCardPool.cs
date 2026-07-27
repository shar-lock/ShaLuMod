// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.CardPools.RegentCardPool
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Timeline.Epochs;
using MegaCrit.Sts2.Core.Unlocks;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.CardPools;

public sealed class RegentCardPool : CardPoolModel
{
  public override string Title => "regent";

  public override string EnergyColorName => "regent";

  public override string CardFrameMaterialPath => "card_frame_orange";

  public override Color DeckEntryCardColor => new Color("E36600");

  public override Color EnergyOutlineColor => new Color("803D0E");

  public override bool IsColorless => false;

  protected override CardModel[] GenerateAllCards()
  {
    return new CardModel[91]
    {
      (CardModel) ModelDb.Card<Alignment>(),
      (CardModel) ModelDb.Card<Arsenal>(),
      (CardModel) ModelDb.Card<AstralPulse>(),
      (CardModel) ModelDb.Card<BeatIntoShape>(),
      (CardModel) ModelDb.Card<Begone>(),
      (CardModel) ModelDb.Card<BigBang>(),
      (CardModel) ModelDb.Card<BlackHole>(),
      (CardModel) ModelDb.Card<Bombardment>(),
      (CardModel) ModelDb.Card<Bulwark>(),
      (CardModel) ModelDb.Card<BundleOfJoy>(),
      (CardModel) ModelDb.Card<CelestialMight>(),
      (CardModel) ModelDb.Card<Charge>(),
      (CardModel) ModelDb.Card<ChildOfTheStars>(),
      (CardModel) ModelDb.Card<CloakOfStars>(),
      (CardModel) ModelDb.Card<CollisionCourse>(),
      (CardModel) ModelDb.Card<Comet>(),
      (CardModel) ModelDb.Card<Conqueror>(),
      (CardModel) ModelDb.Card<Constellation>(),
      (CardModel) ModelDb.Card<Convergence>(),
      (CardModel) ModelDb.Card<CosmicIndifference>(),
      (CardModel) ModelDb.Card<CrashLanding>(),
      (CardModel) ModelDb.Card<CrescentSpear>(),
      (CardModel) ModelDb.Card<CrushUnder>(),
      (CardModel) ModelDb.Card<DecisionsDecisions>(),
      (CardModel) ModelDb.Card<DefendRegent>(),
      (CardModel) ModelDb.Card<Devastate>(),
      (CardModel) ModelDb.Card<DyingStar>(),
      (CardModel) ModelDb.Card<FallingStar>(),
      (CardModel) ModelDb.Card<ForegoneConclusion>(),
      (CardModel) ModelDb.Card<Furnace>(),
      (CardModel) ModelDb.Card<GammaBlast>(),
      (CardModel) ModelDb.Card<GatherLight>(),
      (CardModel) ModelDb.Card<Genesis>(),
      (CardModel) ModelDb.Card<Glimmer>(),
      (CardModel) ModelDb.Card<Glitterstream>(),
      (CardModel) ModelDb.Card<Glow>(),
      (CardModel) ModelDb.Card<Guards>(),
      (CardModel) ModelDb.Card<GuidingStar>(),
      (CardModel) ModelDb.Card<HammerTime>(),
      (CardModel) ModelDb.Card<HeavenlyDrill>(),
      (CardModel) ModelDb.Card<Hegemony>(),
      (CardModel) ModelDb.Card<HeirloomHammer>(),
      (CardModel) ModelDb.Card<HiddenCache>(),
      (CardModel) ModelDb.Card<IAmInvincible>(),
      (CardModel) ModelDb.Card<KinglyKick>(),
      (CardModel) ModelDb.Card<KinglyPunch>(),
      (CardModel) ModelDb.Card<KnockoutBlow>(),
      (CardModel) ModelDb.Card<KnowThyPlace>(),
      (CardModel) ModelDb.Card<Largesse>(),
      (CardModel) ModelDb.Card<LunarBlast>(),
      (CardModel) ModelDb.Card<MakeItSo>(),
      (CardModel) ModelDb.Card<ManifestAuthority>(),
      (CardModel) ModelDb.Card<MeteorShower>(),
      (CardModel) ModelDb.Card<MonarchsGaze>(),
      (CardModel) ModelDb.Card<Monologue>(),
      (CardModel) ModelDb.Card<NeutronAegis>(),
      (CardModel) ModelDb.Card<Orbit>(),
      (CardModel) ModelDb.Card<PaleBlueDot>(),
      (CardModel) ModelDb.Card<Parry>(),
      (CardModel) ModelDb.Card<ParticleWall>(),
      (CardModel) ModelDb.Card<Patter>(),
      (CardModel) ModelDb.Card<PhotonCut>(),
      (CardModel) ModelDb.Card<Plot>(),
      (CardModel) ModelDb.Card<PillarOfCreation>(),
      (CardModel) ModelDb.Card<Prophesize>(),
      (CardModel) ModelDb.Card<Quasar>(),
      (CardModel) ModelDb.Card<Radiate>(),
      (CardModel) ModelDb.Card<RefineBlade>(),
      (CardModel) ModelDb.Card<Reflect>(),
      (CardModel) ModelDb.Card<Resonance>(),
      (CardModel) ModelDb.Card<RoyalGamble>(),
      (CardModel) ModelDb.Card<Royalties>(),
      (CardModel) ModelDb.Card<SeekingEdge>(),
      (CardModel) ModelDb.Card<SevenStars>(),
      (CardModel) ModelDb.Card<ShiningStrike>(),
      (CardModel) ModelDb.Card<SolarStrike>(),
      (CardModel) ModelDb.Card<SpectrumShift>(),
      (CardModel) ModelDb.Card<SpoilsOfBattle>(),
      (CardModel) ModelDb.Card<Stardust>(),
      (CardModel) ModelDb.Card<StrikeRegent>(),
      (CardModel) ModelDb.Card<SummonForth>(),
      (CardModel) ModelDb.Card<Supermassive>(),
      (CardModel) ModelDb.Card<SwordSage>(),
      (CardModel) ModelDb.Card<Terraforming>(),
      (CardModel) ModelDb.Card<TheSealedThrone>(),
      (CardModel) ModelDb.Card<TheSmith>(),
      (CardModel) ModelDb.Card<Tutor>(),
      (CardModel) ModelDb.Card<Tyranny>(),
      (CardModel) ModelDb.Card<Venerate>(),
      (CardModel) ModelDb.Card<VoidForm>(),
      (CardModel) ModelDb.Card<WroughtInWar>()
    };
  }

  protected override IEnumerable<CardModel> FilterThroughEpochs(
    UnlockState unlockState,
    IEnumerable<CardModel> cards)
  {
    List<CardModel> list = cards.ToList<CardModel>();
    if (!unlockState.IsEpochRevealed<Regent2Epoch>())
      list.RemoveAll((Predicate<CardModel>) (c => Regent2Epoch.Cards.Any<CardModel>((Func<CardModel, bool>) (card => card.Id == c.Id))));
    if (!unlockState.IsEpochRevealed<Regent5Epoch>())
      list.RemoveAll((Predicate<CardModel>) (c => Regent5Epoch.Cards.Any<CardModel>((Func<CardModel, bool>) (card => card.Id == c.Id))));
    if (!unlockState.IsEpochRevealed<Regent7Epoch>())
      list.RemoveAll((Predicate<CardModel>) (c => Regent7Epoch.Cards.Any<CardModel>((Func<CardModel, bool>) (card => card.Id == c.Id))));
    return (IEnumerable<CardModel>) list;
  }
}
