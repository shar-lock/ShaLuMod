// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.PotionPools.MockPotionPool
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Models.Potions.Mocks;
using System.Collections.Generic;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.PotionPools;

public sealed class MockPotionPool : PotionPoolModel
{
  public override bool IsMock => true;

  public override string EnergyColorName => "colorless";

  protected override IEnumerable<PotionModel> GenerateAllPotions()
  {
    // ISSUE: object of a compiler-generated type is created
    return (IEnumerable<PotionModel>) new \u003C\u003Ez__ReadOnlySingleElementList<PotionModel>((PotionModel) ModelDb.Potion<MockDiscardAndAddShivsPotion>());
  }
}
