// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.RelicPools.SilentRelicPool
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Models.Relics;
using MegaCrit.Sts2.Core.Timeline.Epochs;
using MegaCrit.Sts2.Core.Unlocks;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.RelicPools;

public sealed class SilentRelicPool : RelicPoolModel
{
  public override string EnergyColorName => "silent";

  public override Color LabOutlineColor => StsColors.green;

  protected override IEnumerable<RelicModel> GenerateAllRelics()
  {
    // ISSUE: object of a compiler-generated type is created
    return (IEnumerable<RelicModel>) new \u003C\u003Ez__ReadOnlyArray<RelicModel>(new RelicModel[8]
    {
      (RelicModel) ModelDb.Relic<HelicalDart>(),
      (RelicModel) ModelDb.Relic<NinjaScroll>(),
      (RelicModel) ModelDb.Relic<PaperKrane>(),
      (RelicModel) ModelDb.Relic<RingOfTheSnake>(),
      (RelicModel) ModelDb.Relic<SneckoSkull>(),
      (RelicModel) ModelDb.Relic<Tingsha>(),
      (RelicModel) ModelDb.Relic<ToughBandages>(),
      (RelicModel) ModelDb.Relic<TwistedFunnel>()
    });
  }

  public override IEnumerable<RelicModel> GetUnlockedRelics(UnlockState unlockState)
  {
    List<RelicModel> list = this.AllRelics.ToList<RelicModel>();
    if (!unlockState.IsEpochRevealed<Silent3Epoch>())
      list.RemoveAll((Predicate<RelicModel>) (r => Silent3Epoch.Relics.Any<RelicModel>((Func<RelicModel, bool>) (relic => relic.Id == r.Id))));
    if (!unlockState.IsEpochRevealed<Silent6Epoch>())
      list.RemoveAll((Predicate<RelicModel>) (r => Silent6Epoch.Relics.Any<RelicModel>((Func<RelicModel, bool>) (relic => relic.Id == r.Id))));
    return (IEnumerable<RelicModel>) list;
  }
}
