// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.RelicPools.NecrobinderRelicPool
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

public sealed class NecrobinderRelicPool : RelicPoolModel
{
  public override string EnergyColorName => "necrobinder";

  public override Color LabOutlineColor => StsColors.pink;

  protected override IEnumerable<RelicModel> GenerateAllRelics()
  {
    // ISSUE: object of a compiler-generated type is created
    return (IEnumerable<RelicModel>) new \u003C\u003Ez__ReadOnlyArray<RelicModel>(new RelicModel[8]
    {
      (RelicModel) ModelDb.Relic<BigHat>(),
      (RelicModel) ModelDb.Relic<BoneFlute>(),
      (RelicModel) ModelDb.Relic<BookRepairKnife>(),
      (RelicModel) ModelDb.Relic<Bookmark>(),
      (RelicModel) ModelDb.Relic<BoundPhylactery>(),
      (RelicModel) ModelDb.Relic<FuneraryMask>(),
      (RelicModel) ModelDb.Relic<IvoryTile>(),
      (RelicModel) ModelDb.Relic<UndyingSigil>()
    });
  }

  public override IEnumerable<RelicModel> GetUnlockedRelics(UnlockState unlockState)
  {
    List<RelicModel> list = this.AllRelics.ToList<RelicModel>();
    if (!unlockState.IsEpochRevealed<Necrobinder3Epoch>())
      list.RemoveAll((Predicate<RelicModel>) (r => Necrobinder3Epoch.Relics.Any<RelicModel>((Func<RelicModel, bool>) (relic => relic.Id == r.Id))));
    if (!unlockState.IsEpochRevealed<Necrobinder6Epoch>())
      list.RemoveAll((Predicate<RelicModel>) (r => Necrobinder6Epoch.Relics.Any<RelicModel>((Func<RelicModel, bool>) (relic => relic.Id == r.Id))));
    return (IEnumerable<RelicModel>) list;
  }
}
