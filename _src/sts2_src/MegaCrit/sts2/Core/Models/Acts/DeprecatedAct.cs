// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Acts.DeprecatedAct
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using MegaCrit.Sts2.Core.Map;
using MegaCrit.Sts2.Core.Random;
using MegaCrit.Sts2.Core.Unlocks;
using System;
using System.Collections.Generic;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Acts;

public sealed class DeprecatedAct : ActModel
{
  public override IEnumerable<EncounterModel> GenerateAllEncounters()
  {
    return (IEnumerable<EncounterModel>) Array.Empty<EncounterModel>();
  }

  public override bool IsUnlocked(UnlockState unlockState) => true;

  public override string ChestOpenSfx => "";

  public override IEnumerable<EncounterModel> BossDiscoveryOrder
  {
    get => (IEnumerable<EncounterModel>) Array.Empty<EncounterModel>();
  }

  public override IEnumerable<AncientEventModel> AllAncients
  {
    get => (IEnumerable<AncientEventModel>) Array.Empty<AncientEventModel>();
  }

  public override IEnumerable<AncientEventModel> GetUnlockedAncients(UnlockState unlockState)
  {
    return (IEnumerable<AncientEventModel>) Array.Empty<AncientEventModel>();
  }

  public override IEnumerable<EventModel> AllEvents
  {
    get => (IEnumerable<EventModel>) Array.Empty<EventModel>();
  }

  protected override int NumberOfWeakEncounters => 0;

  protected override int BaseNumberOfRooms => 0;

  public override string[] BgMusicOptions => Array.Empty<string>();

  public override string[] MusicBankPaths => Array.Empty<string>();

  public override string AmbientSfx => "";

  public override string ChestSpineResourcePath => "";

  public override string ChestSpineSkinNameNormal => "";

  public override string ChestSpineSkinNameStroke => "";

  protected override void ApplyActDiscoveryOrderModifications(UnlockState unlockState)
  {
  }

  public override int Index => -1;

  public override bool IsDefault => false;

  public override Color MapTraveledColor => new Color("27221C");

  public override Color MapUntraveledColor => new Color("6E7750");

  public override Color MapBgColor => new Color("9B9562");

  public override MapPointTypeCounts GetMapPointTypes(Rng mapRng) => new MapPointTypeCounts(0, 0);
}
