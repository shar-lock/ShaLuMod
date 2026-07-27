// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.RelicPools.SharedRelicPool
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Models.Relics;
using MegaCrit.Sts2.Core.Timeline.Epochs;
using MegaCrit.Sts2.Core.Unlocks;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.RelicPools;

public sealed class SharedRelicPool : RelicPoolModel
{
  public override string EnergyColorName => "colorless";

  protected override IEnumerable<RelicModel> GenerateAllRelics()
  {
    // ISSUE: object of a compiler-generated type is created
    return (IEnumerable<RelicModel>) new \u003C\u003Ez__ReadOnlyArray<RelicModel>(new RelicModel[118]
    {
      (RelicModel) ModelDb.Relic<Akabeko>(),
      (RelicModel) ModelDb.Relic<AmethystAubergine>(),
      (RelicModel) ModelDb.Relic<Anchor>(),
      (RelicModel) ModelDb.Relic<ArtOfWar>(),
      (RelicModel) ModelDb.Relic<BagOfMarbles>(),
      (RelicModel) ModelDb.Relic<BagOfPreparation>(),
      (RelicModel) ModelDb.Relic<BeatingRemnant>(),
      (RelicModel) ModelDb.Relic<Bellows>(),
      (RelicModel) ModelDb.Relic<BeltBuckle>(),
      (RelicModel) ModelDb.Relic<BloodVial>(),
      (RelicModel) ModelDb.Relic<BookOfFiveRings>(),
      (RelicModel) ModelDb.Relic<BowlerHat>(),
      (RelicModel) ModelDb.Relic<Bread>(),
      (RelicModel) ModelDb.Relic<BronzeScales>(),
      (RelicModel) ModelDb.Relic<BurningSticks>(),
      (RelicModel) ModelDb.Relic<Candelabra>(),
      (RelicModel) ModelDb.Relic<CaptainsWheel>(),
      (RelicModel) ModelDb.Relic<Cauldron>(),
      (RelicModel) ModelDb.Relic<CentennialPuzzle>(),
      (RelicModel) ModelDb.Relic<Chandelier>(),
      (RelicModel) ModelDb.Relic<ChemicalX>(),
      (RelicModel) ModelDb.Relic<CloakClasp>(),
      (RelicModel) ModelDb.Relic<DingyRug>(),
      (RelicModel) ModelDb.Relic<DollysMirror>(),
      (RelicModel) ModelDb.Relic<DragonFruit>(),
      (RelicModel) ModelDb.Relic<EternalFeather>(),
      (RelicModel) ModelDb.Relic<FestivePopper>(),
      (RelicModel) ModelDb.Relic<FresnelLens>(),
      (RelicModel) ModelDb.Relic<FrozenEgg>(),
      (RelicModel) ModelDb.Relic<GamblingChip>(),
      (RelicModel) ModelDb.Relic<GamePiece>(),
      (RelicModel) ModelDb.Relic<GhostSeed>(),
      (RelicModel) ModelDb.Relic<Girya>(),
      (RelicModel) ModelDb.Relic<GnarledHammer>(),
      (RelicModel) ModelDb.Relic<Gorget>(),
      (RelicModel) ModelDb.Relic<GremlinHorn>(),
      (RelicModel) ModelDb.Relic<HappyFlower>(),
      (RelicModel) ModelDb.Relic<HornCleat>(),
      (RelicModel) ModelDb.Relic<IceCream>(),
      (RelicModel) ModelDb.Relic<IntimidatingHelmet>(),
      (RelicModel) ModelDb.Relic<JossPaper>(),
      (RelicModel) ModelDb.Relic<JuzuBracelet>(),
      (RelicModel) ModelDb.Relic<Kifuda>(),
      (RelicModel) ModelDb.Relic<Kunai>(),
      (RelicModel) ModelDb.Relic<Kusarigama>(),
      (RelicModel) ModelDb.Relic<Lantern>(),
      (RelicModel) ModelDb.Relic<LastingCandy>(),
      (RelicModel) ModelDb.Relic<LavaLamp>(),
      (RelicModel) ModelDb.Relic<LeesWaffle>(),
      (RelicModel) ModelDb.Relic<LetterOpener>(),
      (RelicModel) ModelDb.Relic<LizardTail>(),
      (RelicModel) ModelDb.Relic<LoomingFruit>(),
      (RelicModel) ModelDb.Relic<LuckyFysh>(),
      (RelicModel) ModelDb.Relic<Mango>(),
      (RelicModel) ModelDb.Relic<MealTicket>(),
      (RelicModel) ModelDb.Relic<MeatOnTheBone>(),
      (RelicModel) ModelDb.Relic<MembershipCard>(),
      (RelicModel) ModelDb.Relic<MercuryHourglass>(),
      (RelicModel) ModelDb.Relic<MiniatureCannon>(),
      (RelicModel) ModelDb.Relic<MiniatureTent>(),
      (RelicModel) ModelDb.Relic<MoltenEgg>(),
      (RelicModel) ModelDb.Relic<MummifiedHand>(),
      (RelicModel) ModelDb.Relic<MysticLighter>(),
      (RelicModel) ModelDb.Relic<Nunchaku>(),
      (RelicModel) ModelDb.Relic<OddlySmoothStone>(),
      (RelicModel) ModelDb.Relic<OldCoin>(),
      (RelicModel) ModelDb.Relic<Orichalcum>(),
      (RelicModel) ModelDb.Relic<OrnamentalFan>(),
      (RelicModel) ModelDb.Relic<Orrery>(),
      (RelicModel) ModelDb.Relic<Pantograph>(),
      (RelicModel) ModelDb.Relic<ParryingShield>(),
      (RelicModel) ModelDb.Relic<Pear>(),
      (RelicModel) ModelDb.Relic<PenNib>(),
      (RelicModel) ModelDb.Relic<Pendulum>(),
      (RelicModel) ModelDb.Relic<Permafrost>(),
      (RelicModel) ModelDb.Relic<PetrifiedToad>(),
      (RelicModel) ModelDb.Relic<Planisphere>(),
      (RelicModel) ModelDb.Relic<Pocketwatch>(),
      (RelicModel) ModelDb.Relic<PotionBelt>(),
      (RelicModel) ModelDb.Relic<PrayerWheel>(),
      (RelicModel) ModelDb.Relic<PunchDagger>(),
      (RelicModel) ModelDb.Relic<RainbowRing>(),
      (RelicModel) ModelDb.Relic<RazorTooth>(),
      (RelicModel) ModelDb.Relic<RedMask>(),
      (RelicModel) ModelDb.Relic<RegalPillow>(),
      (RelicModel) ModelDb.Relic<ReptileTrinket>(),
      (RelicModel) ModelDb.Relic<RingingTriangle>(),
      (RelicModel) ModelDb.Relic<RippleBasin>(),
      (RelicModel) ModelDb.Relic<RoyalStamp>(),
      (RelicModel) ModelDb.Relic<ScreamingFlagon>(),
      (RelicModel) ModelDb.Relic<Shovel>(),
      (RelicModel) ModelDb.Relic<Shuriken>(),
      (RelicModel) ModelDb.Relic<SlingOfCourage>(),
      (RelicModel) ModelDb.Relic<SparklingRouge>(),
      (RelicModel) ModelDb.Relic<StoneCalendar>(),
      (RelicModel) ModelDb.Relic<StoneCracker>(),
      (RelicModel) ModelDb.Relic<Strawberry>(),
      (RelicModel) ModelDb.Relic<StrikeDummy>(),
      (RelicModel) ModelDb.Relic<SturdyClamp>(),
      (RelicModel) ModelDb.Relic<TheAbacus>(),
      (RelicModel) ModelDb.Relic<TheCourier>(),
      (RelicModel) ModelDb.Relic<TinyMailbox>(),
      (RelicModel) ModelDb.Relic<Toolbox>(),
      (RelicModel) ModelDb.Relic<ToxicEgg>(),
      (RelicModel) ModelDb.Relic<TungstenRod>(),
      (RelicModel) ModelDb.Relic<TuningFork>(),
      (RelicModel) ModelDb.Relic<UnceasingTop>(),
      (RelicModel) ModelDb.Relic<UnsettlingLamp>(),
      (RelicModel) ModelDb.Relic<Vajra>(),
      (RelicModel) ModelDb.Relic<Vambrace>(),
      (RelicModel) ModelDb.Relic<VenerableTeaSet>(),
      (RelicModel) ModelDb.Relic<VeryHotCocoa>(),
      (RelicModel) ModelDb.Relic<VexingPuzzlebox>(),
      (RelicModel) ModelDb.Relic<WarPaint>(),
      (RelicModel) ModelDb.Relic<Whetstone>(),
      (RelicModel) ModelDb.Relic<WhiteBeastStatue>(),
      (RelicModel) ModelDb.Relic<WhiteStar>(),
      (RelicModel) ModelDb.Relic<WingCharm>()
    });
  }

  public override IEnumerable<RelicModel> GetUnlockedRelics(UnlockState unlockState)
  {
    List<RelicModel> list = this.AllRelics.ToList<RelicModel>();
    if (!unlockState.IsEpochRevealed<Relic1Epoch>())
      list.RemoveAll((Predicate<RelicModel>) (r => Relic1Epoch.Relics.Any<RelicModel>((Func<RelicModel, bool>) (relic => relic.Id == r.Id))));
    if (!unlockState.IsEpochRevealed<Relic2Epoch>())
      list.RemoveAll((Predicate<RelicModel>) (r => Relic2Epoch.Relics.Any<RelicModel>((Func<RelicModel, bool>) (relic => relic.Id == r.Id))));
    if (!unlockState.IsEpochRevealed<Relic3Epoch>())
      list.RemoveAll((Predicate<RelicModel>) (r => Relic3Epoch.Relics.Any<RelicModel>((Func<RelicModel, bool>) (relic => relic.Id == r.Id))));
    if (!unlockState.IsEpochRevealed<Relic4Epoch>())
      list.RemoveAll((Predicate<RelicModel>) (r => Relic4Epoch.Relics.Any<RelicModel>((Func<RelicModel, bool>) (relic => relic.Id == r.Id))));
    if (!unlockState.IsEpochRevealed<Relic5Epoch>())
      list.RemoveAll((Predicate<RelicModel>) (r => Relic5Epoch.Relics.Any<RelicModel>((Func<RelicModel, bool>) (relic => relic.Id == r.Id))));
    return (IEnumerable<RelicModel>) list;
  }
}
