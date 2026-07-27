// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.PotionPools.DefectPotionPool
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Timeline.Epochs;
using MegaCrit.Sts2.Core.Unlocks;
using System;
using System.Collections.Generic;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.PotionPools;

public sealed class DefectPotionPool : PotionPoolModel
{
  public override string EnergyColorName => "defect";

  public override Color LabOutlineColor => StsColors.blue;

  protected override IEnumerable<PotionModel> GenerateAllPotions()
  {
    return (IEnumerable<PotionModel>) Defect4Epoch.Potions;
  }

  public override IEnumerable<PotionModel> GetUnlockedPotions(UnlockState unlockState)
  {
    return !unlockState.IsEpochRevealed<Defect4Epoch>() ? (IEnumerable<PotionModel>) Array.Empty<PotionModel>() : this.GenerateAllPotions();
  }
}
