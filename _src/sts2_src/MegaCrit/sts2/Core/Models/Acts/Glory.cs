// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Acts.Glory
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using MegaCrit.Sts2.Core.Map;
using MegaCrit.Sts2.Core.Models.Encounters;
using MegaCrit.Sts2.Core.Models.Events;
using MegaCrit.Sts2.Core.Random;
using MegaCrit.Sts2.Core.Unlocks;
using System.Collections.Generic;
using System.Linq;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Acts;

public sealed class Glory : ActModel
{
  public override IEnumerable<EncounterModel> GenerateAllEncounters()
  {
    // ISSUE: object of a compiler-generated type is created
    return (IEnumerable<EncounterModel>) new \u003C\u003Ez__ReadOnlyArray<EncounterModel>(new EncounterModel[18]
    {
      (EncounterModel) ModelDb.Encounter<AxebotsNormal>(),
      (EncounterModel) ModelDb.Encounter<ConstructMenagerieNormal>(),
      (EncounterModel) ModelDb.Encounter<DevotedSculptorWeak>(),
      (EncounterModel) ModelDb.Encounter<AeonglassBoss>(),
      (EncounterModel) ModelDb.Encounter<FabricatorNormal>(),
      (EncounterModel) ModelDb.Encounter<FrogKnightNormal>(),
      (EncounterModel) ModelDb.Encounter<GlobeHeadNormal>(),
      (EncounterModel) ModelDb.Encounter<KnightsElite>(),
      (EncounterModel) ModelDb.Encounter<MechaKnightElite>(),
      (EncounterModel) ModelDb.Encounter<OwlMagistrateNormal>(),
      (EncounterModel) ModelDb.Encounter<QueenBoss>(),
      (EncounterModel) ModelDb.Encounter<ScrollsOfBitingNormal>(),
      (EncounterModel) ModelDb.Encounter<ScrollsOfBitingWeak>(),
      (EncounterModel) ModelDb.Encounter<SlimedBerserkerNormal>(),
      (EncounterModel) ModelDb.Encounter<SoulNexusElite>(),
      (EncounterModel) ModelDb.Encounter<TestSubjectBoss>(),
      (EncounterModel) ModelDb.Encounter<TheLostAndForgottenNormal>(),
      (EncounterModel) ModelDb.Encounter<TurretOperatorWeak>()
    });
  }

  public override string ChestOpenSfx => "event:/sfx/ui/treasure/treasure_act3";

  public override IEnumerable<EncounterModel> BossDiscoveryOrder
  {
    get
    {
      return (IEnumerable<EncounterModel>) new \u003C\u003Ez__ReadOnlyArray<EncounterModel>(new EncounterModel[3]
      {
        (EncounterModel) ModelDb.Encounter<QueenBoss>(),
        (EncounterModel) ModelDb.Encounter<TestSubjectBoss>(),
        (EncounterModel) ModelDb.Encounter<AeonglassBoss>()
      });
    }
  }

  public override IEnumerable<AncientEventModel> AllAncients
  {
    get
    {
      return (IEnumerable<AncientEventModel>) new \u003C\u003Ez__ReadOnlyArray<AncientEventModel>(new AncientEventModel[3]
      {
        (AncientEventModel) ModelDb.AncientEvent<Nonupeipe>(),
        (AncientEventModel) ModelDb.AncientEvent<Tanx>(),
        (AncientEventModel) ModelDb.AncientEvent<Vakuu>()
      });
    }
  }

  public override IEnumerable<AncientEventModel> GetUnlockedAncients(UnlockState unlockState)
  {
    return (IEnumerable<AncientEventModel>) this.AllAncients.ToList<AncientEventModel>();
  }

  public override IEnumerable<EventModel> AllEvents
  {
    get
    {
      return (IEnumerable<EventModel>) new \u003C\u003Ez__ReadOnlyArray<EventModel>(new EventModel[7]
      {
        (EventModel) ModelDb.Event<BattlewornDummy>(),
        (EventModel) ModelDb.Event<GraveOfTheForgotten>(),
        (EventModel) ModelDb.Event<HungryForMushrooms>(),
        (EventModel) ModelDb.Event<Reflections>(),
        (EventModel) ModelDb.Event<RoundTeaParty>(),
        (EventModel) ModelDb.Event<Trial>(),
        (EventModel) ModelDb.Event<TinkerTime>()
      });
    }
  }

  protected override int NumberOfWeakEncounters => 2;

  protected override int BaseNumberOfRooms => 13;

  public override int Index => 2;

  public override bool IsDefault => true;

  public override string[] BgMusicOptions
  {
    get
    {
      return new string[2]
      {
        "event:/music/act3_a1_v1",
        "event:/music/act3_a2_v1"
      };
    }
  }

  public override string[] MusicBankPaths
  {
    get
    {
      return new string[2]
      {
        "res://banks/desktop/act3_a1.bank",
        "res://banks/desktop/act3_a2.bank"
      };
    }
  }

  public override string AmbientSfx => "event:/sfx/ambience/act3_ambience";

  public override string ChestSpineResourcePath
  {
    get => "res://animations/backgrounds/treasure_room/chest_room_act_3_skel_data.tres";
  }

  public override string ChestSpineSkinNameNormal => "act3";

  public override string ChestSpineSkinNameStroke => "act3_stroke";

  protected override void ApplyActDiscoveryOrderModifications(UnlockState unlockState)
  {
  }

  public override bool IsUnlocked(UnlockState unlockState) => true;

  public override Color MapTraveledColor => new Color("1D1E2F");

  public override Color MapUntraveledColor => new Color("60717C");

  public override Color MapBgColor => new Color("819A97");

  public override MapPointTypeCounts GetMapPointTypes(Rng mapRng)
  {
    int restCount = mapRng.NextInt(5, 7);
    return new MapPointTypeCounts(MapPointTypeCounts.StandardRandomUnknownCount(mapRng) - 1, restCount);
  }
}
