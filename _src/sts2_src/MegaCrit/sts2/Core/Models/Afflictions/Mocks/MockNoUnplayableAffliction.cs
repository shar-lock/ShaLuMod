// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Afflictions.Mocks.MockNoUnplayableAffliction
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

#nullable disable
namespace MegaCrit.Sts2.Core.Models.Afflictions.Mocks;

public sealed class MockNoUnplayableAffliction : AfflictionModel
{
  public override bool IsMock => true;

  public override bool CanAfflictUnplayableCards => false;
}
