// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Powers.MindRotPower
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using System;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Powers;

public sealed class MindRotPower : PowerModel
{
  public override PowerType Type => PowerType.Debuff;

  public override PowerStackType StackType => PowerStackType.Counter;

  public override Decimal ModifyHandDraw(Player player, Decimal count)
  {
    return player != this.Owner.Player ? count : Math.Max(0M, count - (Decimal) this.Amount);
  }

  public override Task AfterModifyingHandDraw()
  {
    this.Flash();
    return Task.CompletedTask;
  }
}
