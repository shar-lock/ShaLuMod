// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.RelicPools.IroncladRelicPool
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

public sealed class IroncladRelicPool : RelicPoolModel
{
  public override string EnergyColorName => "ironclad";

  public override Color LabOutlineColor => StsColors.red;

  protected override IEnumerable<RelicModel> GenerateAllRelics()
  {
    // ISSUE: object of a compiler-generated type is created
    return (IEnumerable<RelicModel>) new \u003C\u003Ez__ReadOnlyArray<RelicModel>(new RelicModel[8]
    {
      (RelicModel) ModelDb.Relic<Brimstone>(),
      (RelicModel) ModelDb.Relic<BurningBlood>(),
      (RelicModel) ModelDb.Relic<CharonsAshes>(),
      (RelicModel) ModelDb.Relic<DemonTongue>(),
      (RelicModel) ModelDb.Relic<PaperPhrog>(),
      (RelicModel) ModelDb.Relic<RedSkull>(),
      (RelicModel) ModelDb.Relic<RuinedHelmet>(),
      (RelicModel) ModelDb.Relic<SelfFormingClay>()
    });
  }

  public override IEnumerable<RelicModel> GetUnlockedRelics(UnlockState unlockState)
  {
    List<RelicModel> list = this.AllRelics.ToList<RelicModel>();
    if (!unlockState.IsEpochRevealed<Ironclad3Epoch>())
      list.RemoveAll((Predicate<RelicModel>) (r => Ironclad3Epoch.Relics.Any<RelicModel>((Func<RelicModel, bool>) (relic => relic.Id == r.Id))));
    if (!unlockState.IsEpochRevealed<Ironclad6Epoch>())
      list.RemoveAll((Predicate<RelicModel>) (r => Ironclad6Epoch.Relics.Any<RelicModel>((Func<RelicModel, bool>) (relic => relic.Id == r.Id))));
    return (IEnumerable<RelicModel>) list;
  }
}
