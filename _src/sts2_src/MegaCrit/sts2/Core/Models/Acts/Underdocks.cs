// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Acts.Underdocks
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using MegaCrit.Sts2.Core.Map;
using MegaCrit.Sts2.Core.Models.Encounters;
using MegaCrit.Sts2.Core.Models.Events;
using MegaCrit.Sts2.Core.Random;
using MegaCrit.Sts2.Core.Timeline.Epochs;
using MegaCrit.Sts2.Core.Unlocks;
using System.Collections.Generic;
using System.Linq;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Acts;

public sealed class Underdocks : ActModel
{
  public override IEnumerable<EncounterModel> GenerateAllEncounters()
  {
    // ISSUE: object of a compiler-generated type is created
    return (IEnumerable<EncounterModel>) new \u003C\u003Ez__ReadOnlyArray<EncounterModel>(new EncounterModel[20]
    {
      (EncounterModel) ModelDb.Encounter<CorpseSlugsNormal>(),
      (EncounterModel) ModelDb.Encounter<CorpseSlugsWeak>(),
      (EncounterModel) ModelDb.Encounter<CultistsNormal>(),
      (EncounterModel) ModelDb.Encounter<FossilStalkerNormal>(),
      (EncounterModel) ModelDb.Encounter<GremlinMercNormal>(),
      (EncounterModel) ModelDb.Encounter<HauntedShipNormal>(),
      (EncounterModel) ModelDb.Encounter<LagavulinMatriarchBoss>(),
      (EncounterModel) ModelDb.Encounter<LivingFogNormal>(),
      (EncounterModel) ModelDb.Encounter<PhantasmalGardenersElite>(),
      (EncounterModel) ModelDb.Encounter<PunchConstructNormal>(),
      (EncounterModel) ModelDb.Encounter<SeapunkNormal>(),
      (EncounterModel) ModelDb.Encounter<SeapunkWeak>(),
      (EncounterModel) ModelDb.Encounter<SewerClamNormal>(),
      (EncounterModel) ModelDb.Encounter<SkulkingColonyElite>(),
      (EncounterModel) ModelDb.Encounter<SludgeSpinnerWeak>(),
      (EncounterModel) ModelDb.Encounter<SoulFyshBoss>(),
      (EncounterModel) ModelDb.Encounter<TerrorEelElite>(),
      (EncounterModel) ModelDb.Encounter<ToadpolesWeak>(),
      (EncounterModel) ModelDb.Encounter<TwoTailedRatsNormal>(),
      (EncounterModel) ModelDb.Encounter<WaterfallGiantBoss>()
    });
  }

  public override IEnumerable<EncounterModel> BossDiscoveryOrder
  {
    get
    {
      return (IEnumerable<EncounterModel>) new \u003C\u003Ez__ReadOnlyArray<EncounterModel>(new EncounterModel[3]
      {
        (EncounterModel) ModelDb.Encounter<WaterfallGiantBoss>(),
        (EncounterModel) ModelDb.Encounter<SoulFyshBoss>(),
        (EncounterModel) ModelDb.Encounter<LagavulinMatriarchBoss>()
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

  public override string ChestOpenSfx => "event:/sfx/ui/treasure/treasure_act1";

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
      return (IEnumerable<EventModel>) new \u003C\u003Ez__ReadOnlyArray<EventModel>(new EventModel[10]
      {
        (EventModel) ModelDb.Event<AbyssalBaths>(),
        (EventModel) ModelDb.Event<DrowningBeacon>(),
        (EventModel) ModelDb.Event<EndlessConveyor>(),
        (EventModel) ModelDb.Event<PunchOff>(),
        (EventModel) ModelDb.Event<SpiralingWhirlpool>(),
        (EventModel) ModelDb.Event<SunkenStatue>(),
        (EventModel) ModelDb.Event<SunkenTreasury>(),
        (EventModel) ModelDb.Event<DoorsOfLightAndDark>(),
        (EventModel) ModelDb.Event<TrashHeap>(),
        (EventModel) ModelDb.Event<WaterloggedScriptorium>()
      });
    }
  }

  protected override void ApplyActDiscoveryOrderModifications(UnlockState unlockState)
  {
  }

  public override bool IsUnlocked(UnlockState unlockState)
  {
    return unlockState.IsEpochRevealed<UnderdocksEpoch>();
  }

  protected override int NumberOfWeakEncounters => 3;

  protected override int BaseNumberOfRooms => 15;

  public override int Index => 0;

  public override bool IsDefault => false;

  public override string[] BgMusicOptions
  {
    get => new string[1]{ "event:/music/act1_b1_v1" };
  }

  public override string[] MusicBankPaths
  {
    get
    {
      return new string[1]
      {
        "res://banks/desktop/act1_b1.bank"
      };
    }
  }

  public override string AmbientSfx => "event:/sfx/ambience/act3_ambience";

  public override string ChestSpineResourcePath
  {
    get => "res://animations/backgrounds/treasure_room/chest_room_act_1_skel_data.tres";
  }

  public override string ChestSpineSkinNameNormal => "act1";

  public override string ChestSpineSkinNameStroke => "act1_stroke";

  public override Color MapTraveledColor => new Color("180F24");

  public override Color MapUntraveledColor => new Color("534A62");

  public override Color MapBgColor => new Color("9F95A5");

  public override MapPointTypeCounts GetMapPointTypes(Rng mapRng)
  {
    int restCount = mapRng.NextGaussianInt(7, 1, 6, 7);
    return new MapPointTypeCounts(MapPointTypeCounts.StandardRandomUnknownCount(mapRng), restCount);
  }
}
