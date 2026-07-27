// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Acts.Overgrowth
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Map;
using MegaCrit.Sts2.Core.Models.Encounters;
using MegaCrit.Sts2.Core.Models.Events;
using MegaCrit.Sts2.Core.Random;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Timeline.Epochs;
using MegaCrit.Sts2.Core.Unlocks;
using System.Collections.Generic;
using System.Linq;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Acts;

public sealed class Overgrowth : ActModel
{
  public override IEnumerable<EncounterModel> GenerateAllEncounters()
  {
    // ISSUE: object of a compiler-generated type is created
    return (IEnumerable<EncounterModel>) new \u003C\u003Ez__ReadOnlyArray<EncounterModel>(new EncounterModel[22]
    {
      (EncounterModel) ModelDb.Encounter<BygoneEffigyElite>(),
      (EncounterModel) ModelDb.Encounter<ByrdonisElite>(),
      (EncounterModel) ModelDb.Encounter<CeremonialBeastBoss>(),
      (EncounterModel) ModelDb.Encounter<CubexConstructNormal>(),
      (EncounterModel) ModelDb.Encounter<FlyconidNormal>(),
      (EncounterModel) ModelDb.Encounter<FogmogNormal>(),
      (EncounterModel) ModelDb.Encounter<FuzzyWurmCrawlerWeak>(),
      (EncounterModel) ModelDb.Encounter<InkletsNormal>(),
      (EncounterModel) ModelDb.Encounter<MawlerNormal>(),
      (EncounterModel) ModelDb.Encounter<NibbitsNormal>(),
      (EncounterModel) ModelDb.Encounter<NibbitsWeak>(),
      (EncounterModel) ModelDb.Encounter<OvergrowthCrawlers>(),
      (EncounterModel) ModelDb.Encounter<PhrogParasiteElite>(),
      (EncounterModel) ModelDb.Encounter<RubyRaidersNormal>(),
      (EncounterModel) ModelDb.Encounter<ShrinkerBeetleWeak>(),
      (EncounterModel) ModelDb.Encounter<SlimesNormal>(),
      (EncounterModel) ModelDb.Encounter<SlimesWeak>(),
      (EncounterModel) ModelDb.Encounter<SlitheringStranglerNormal>(),
      (EncounterModel) ModelDb.Encounter<SnappingJaxfruitNormal>(),
      (EncounterModel) ModelDb.Encounter<TheKinBoss>(),
      (EncounterModel) ModelDb.Encounter<VantomBoss>(),
      (EncounterModel) ModelDb.Encounter<VineShamblerNormal>()
    });
  }

  public override string ChestOpenSfx => "event:/sfx/ui/treasure/treasure_act1";

  public override IEnumerable<EncounterModel> BossDiscoveryOrder
  {
    get
    {
      return (IEnumerable<EncounterModel>) new \u003C\u003Ez__ReadOnlyArray<EncounterModel>(new EncounterModel[3]
      {
        (EncounterModel) ModelDb.Encounter<VantomBoss>(),
        (EncounterModel) ModelDb.Encounter<CeremonialBeastBoss>(),
        (EncounterModel) ModelDb.Encounter<TheKinBoss>()
      });
    }
  }

  public override IEnumerable<AncientEventModel> AllAncients
  {
    get
    {
      return (IEnumerable<AncientEventModel>) new \u003C\u003Ez__ReadOnlySingleElementList<AncientEventModel>((AncientEventModel) ModelDb.AncientEvent<Neow>());
    }
  }

  public override IEnumerable<AncientEventModel> GetUnlockedAncients(UnlockState unlockState)
  {
    List<AncientEventModel> list = this.AllAncients.ToList<AncientEventModel>();
    if (!unlockState.IsEpochRevealed<NeowEpoch>())
      list.Remove((AncientEventModel) ModelDb.AncientEvent<Neow>());
    return (IEnumerable<AncientEventModel>) list;
  }

  public override IEnumerable<EventModel> AllEvents
  {
    get
    {
      return (IEnumerable<EventModel>) new \u003C\u003Ez__ReadOnlyArray<EventModel>(new EventModel[13]
      {
        (EventModel) ModelDb.Event<AromaOfChaos>(),
        (EventModel) ModelDb.Event<ByrdonisNest>(),
        (EventModel) ModelDb.Event<DenseVegetation>(),
        (EventModel) ModelDb.Event<JungleMazeAdventure>(),
        (EventModel) ModelDb.Event<LuminousChoir>(),
        (EventModel) ModelDb.Event<MorphicGrove>(),
        (EventModel) ModelDb.Event<SapphireSeed>(),
        (EventModel) ModelDb.Event<SunkenStatue>(),
        (EventModel) ModelDb.Event<TabletOfTruth>(),
        (EventModel) ModelDb.Event<UnrestSite>(),
        (EventModel) ModelDb.Event<Wellspring>(),
        (EventModel) ModelDb.Event<WhisperingHollow>(),
        (EventModel) ModelDb.Event<WoodCarvings>()
      });
    }
  }

  protected override void ApplyActDiscoveryOrderModifications(UnlockState unlockState)
  {
    if (unlockState.NumberOfRuns != 0)
      return;
    Log.Info("First run ever. Presenting rooms in a set order.");
    RoomSet.SwapToOrCreateAtIndex<EncounterModel, NibbitsWeak>(this._rooms.normalEncounters, 0);
    RoomSet.SwapToOrCreateAtIndex<EncounterModel, SlimesWeak>(this._rooms.normalEncounters, 1);
    RoomSet.SwapToOrCreateAtIndex<EncounterModel, ShrinkerBeetleWeak>(this._rooms.normalEncounters, 2);
    RoomSet.SwapToOrCreateAtIndex<EncounterModel, InkletsNormal>(this._rooms.normalEncounters, 3);
    RoomSet.SwapToOrCreateAtIndex<EncounterModel, MawlerNormal>(this._rooms.normalEncounters, 4);
    RoomSet.SwapToOrCreateAtIndex<EncounterModel, RubyRaidersNormal>(this._rooms.normalEncounters, 5);
    RoomSet.SwapToOrCreateAtIndex<EncounterModel, NibbitsNormal>(this._rooms.normalEncounters, 6);
    RoomSet.SwapToOrCreateAtIndex<EventModel, ByrdonisNest>(this._rooms.events, 0);
    RoomSet.SwapToOrCreateAtIndex<EventModel, SapphireSeed>(this._rooms.events, 1);
    RoomSet.SwapToOrCreateAtIndex<EncounterModel, ByrdonisElite>(this._rooms.eliteEncounters, 0);
    RoomSet.SwapToOrCreateAtIndex<EncounterModel, PhrogParasiteElite>(this._rooms.eliteEncounters, 1);
  }

  public override bool IsUnlocked(UnlockState unlockState) => true;

  protected override int NumberOfWeakEncounters => 3;

  protected override int BaseNumberOfRooms => 15;

  public override int Index => 0;

  public override bool IsDefault => true;

  public override string[] BgMusicOptions
  {
    get
    {
      return new string[2]
      {
        "event:/music/act1_a1_v1",
        "event:/music/act1_a2_v2"
      };
    }
  }

  public override string[] MusicBankPaths
  {
    get
    {
      return new string[2]
      {
        "res://banks/desktop/act1_a1.bank",
        "res://banks/desktop/act1_a2.bank"
      };
    }
  }

  public override string AmbientSfx => "event:/sfx/ambience/act1_ambience";

  public override string ChestSpineResourcePath
  {
    get => "res://animations/backgrounds/treasure_room/chest_room_act_1_skel_data.tres";
  }

  public override string ChestSpineSkinNameNormal => "act1";

  public override string ChestSpineSkinNameStroke => "act1_stroke";

  public override Color MapTraveledColor => new Color("28231D");

  public override Color MapUntraveledColor => new Color("877256");

  public override Color MapBgColor => new Color("A78A67");

  public override MapPointTypeCounts GetMapPointTypes(Rng mapRng)
  {
    int restCount = mapRng.NextGaussianInt(7, 1, 6, 7);
    return new MapPointTypeCounts(MapPointTypeCounts.StandardRandomUnknownCount(mapRng), restCount);
  }
}
