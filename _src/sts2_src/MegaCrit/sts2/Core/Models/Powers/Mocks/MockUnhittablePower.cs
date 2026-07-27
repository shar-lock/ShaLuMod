// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Powers.Mocks.MockUnhittablePower
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Powers.Mocks;

public sealed class MockUnhittablePower : PowerModel
{
  public override bool IsMock => true;

  public override PowerType Type => PowerType.Buff;

  public override PowerStackType StackType => PowerStackType.Single;

  public override bool ShouldAllowHitting(Creature creature) => creature != this.Owner;
}
