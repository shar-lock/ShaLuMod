// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Powers.Mocks.MockDoNotScaleInMultiplayerPower
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Entities.Powers;

#nullable disable
namespace MegaCrit.Sts2.Core.Models.Powers.Mocks;

public class MockDoNotScaleInMultiplayerPower : PowerModel
{
  public override bool IsMock => true;

  public override PowerType Type => PowerType.Debuff;

  public override PowerStackType StackType => PowerStackType.Counter;

  public override bool AllowNegative => true;

  public override bool ShouldScaleInMultiplayer => false;
}
