// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.PotionPools.EventPotionPool
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Models.Potions;
using System.Collections.Generic;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.PotionPools;

public sealed class EventPotionPool : PotionPoolModel
{
  public override string EnergyColorName => "colorless";

  protected override IEnumerable<PotionModel> GenerateAllPotions()
  {
    // ISSUE: object of a compiler-generated type is created
    return (IEnumerable<PotionModel>) new \u003C\u003Ez__ReadOnlyArray<PotionModel>(new PotionModel[3]
    {
      (PotionModel) ModelDb.Potion<Ambergris>(),
      (PotionModel) ModelDb.Potion<FoulPotion>(),
      (PotionModel) ModelDb.Potion<GlowwaterPotion>()
    });
  }
}
