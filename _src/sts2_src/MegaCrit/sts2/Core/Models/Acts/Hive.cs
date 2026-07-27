// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Acts.Hive
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

public sealed class Hive : ActModel
{
  public override IEnumerable<EncounterModel> GenerateAllEncounters()
  {
    // ISSUE: object of a compiler-generated type is created
    return (IEnumerable<EncounterModel>) new \u003C\u003Ez__ReadOnlyArray<EncounterModel>(new EncounterModel[20]
    {
      (EncounterModel) ModelDb.Encounter<BowlbugsNormal>(),
      (EncounterModel) ModelDb.Encounter<BowlbugsWeak>(),
      (EncounterModel) ModelDb.Encounter<ChompersNormal>(),
      (EncounterModel) ModelDb.Encounter<DecimillipedeElite>(),
      (EncounterModel) ModelDb.Encounter<EntomancerElite>(),
      (EncounterModel) ModelDb.Encounter<ExoskeletonsNormal>(),
      (EncounterModel) ModelDb.Encounter<ExoskeletonsWeak>(),
      (EncounterModel) ModelDb.Encounter<HunterKillerNormal>(),
      (EncounterModel) ModelDb.Encounter<KaiserCrabBoss>(),
      (EncounterModel) ModelDb.Encounter<InfestedPrismsElite>(),
      (EncounterModel) ModelDb.Encounter<KnowledgeDemonBoss>(),
      (EncounterModel) ModelDb.Encounter<LouseProgenitorNormal>(),
      (EncounterModel) ModelDb.Encounter<MytesNormal>(),
      (EncounterModel) ModelDb.Encounter<OvicopterNormal>(),
      (EncounterModel) ModelDb.Encounter<SlumberingBeetleNormal>(),
      (EncounterModel) ModelDb.Encounter<SpinyToadNormal>(),
      (EncounterModel) ModelDb.Encounter<TheInsatiableBoss>(),
      (EncounterModel) ModelDb.Encounter<TheObscuraNormal>(),
      (EncounterModel) ModelDb.Encounter<ThievingHopperWeak>(),
      (EncounterModel) ModelDb.Encounter<TunnelerWeak>()
    });
  }

  public override string ChestOpenSfx => "event:/sfx/ui/treasure/treasure_act2";

  public override IEnumerable<EncounterModel> BossDiscoveryOrder
  {
    get
    {
      return (IEnumerable<EncounterModel>) new \u003C\u003Ez__ReadOnlyArray<EncounterModel>(new EncounterModel[3]
      {
        (EncounterModel) ModelDb.Encounter<TheInsatiableBoss>(),
        (EncounterModel) ModelDb.Encounter<KnowledgeDemonBoss>(),
        (EncounterModel) ModelDb.Encounter<KaiserCrabBoss>()
      });
    }
  }

  public override IEnumerable<AncientEventModel> AllAncients
  {
    get
    {
      return (IEnumerable<AncientEventModel>) new \u003C\u003Ez__ReadOnlyArray<AncientEventModel>(new AncientEventModel[3]
      {
        (AncientEventModel) ModelDb.AncientEvent<Orobas>(),
        (AncientEventModel) ModelDb.AncientEvent<Pael>(),
        (AncientEventModel) ModelDb.AncientEvent<Tezcatara>()
      });
    }
  }

  public override IEnumerable<AncientEventModel> GetUnlockedAncients(UnlockState unlockState)
  {
    List<AncientEventModel> list = this.AllAncients.ToList<AncientEventModel>();
    if (!unlockState.IsEpochRevealed<OrobasEpoch>())
      list.Remove((AncientEventModel) ModelDb.AncientEvent<Orobas>());
    return (IEnumerable<AncientEventModel>) list;
  }

  public override IEnumerable<EventModel> AllEvents
  {
    get
    {
      return (IEnumerable<EventModel>) new \u003C\u003Ez__ReadOnlyArray<EventModel>(new EventModel[10]
      {
        (EventModel) ModelDb.Event<Amalgamator>(),
        (EventModel) ModelDb.Event<Bugslayer>(),
        (EventModel) ModelDb.Event<ColorfulPhilosophers>(),
        (EventModel) ModelDb.Event<ColossalFlower>(),
        (EventModel) ModelDb.Event<FieldOfManSizedHoles>(),
        (EventModel) ModelDb.Event<InfestedAutomaton>(),
        (EventModel) ModelDb.Event<LostWisp>(),
        (EventModel) ModelDb.Event<SpiritGrafter>(),
        (EventModel) ModelDb.Event<TheLanternKey>(),
        (EventModel) ModelDb.Event<ZenWeaver>()
      });
    }
  }

  protected override int NumberOfWeakEncounters => 2;

  protected override int BaseNumberOfRooms => 14;

  public override int Index => 1;

  public override bool IsDefault => true;

  public override string[] BgMusicOptions
  {
    get
    {
      return new string[2]
      {
        "event:/music/act2_a1_v2",
        "event:/music/act2_a2_v2"
      };
    }
  }

  public override string[] MusicBankPaths
  {
    get
    {
      return new string[2]
      {
        "res://banks/desktop/act2_a1.bank",
        "res://banks/desktop/act2_a2.bank"
      };
    }
  }

  public override string AmbientSfx => "event:/sfx/ambience/act2_ambience";

  public override string ChestSpineResourcePath
  {
    get => "res://animations/backgrounds/treasure_room/chest_room_act_2_skel_data.tres";
  }

  public override string ChestSpineSkinNameNormal => "act2";

  public override string ChestSpineSkinNameStroke => "act2_stroke";

  protected override void ApplyActDiscoveryOrderModifications(UnlockState unlockState)
  {
  }

  public override bool IsUnlocked(UnlockState unlockState) => true;

  public override Color MapTraveledColor => new Color("27221C");

  public override Color MapUntraveledColor => new Color("6E7750");

  public override Color MapBgColor => new Color("9B9562");

  public override MapPointTypeCounts GetMapPointTypes(Rng mapRng)
  {
    int restCount = mapRng.NextGaussianInt(6, 1, 6, 7);
    return new MapPointTypeCounts(MapPointTypeCounts.StandardRandomUnknownCount(mapRng) - 1, restCount);
  }
}
