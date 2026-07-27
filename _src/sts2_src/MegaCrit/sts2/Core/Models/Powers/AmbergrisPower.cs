// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Powers.AmbergrisPower
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Powers;

public sealed class AmbergrisPower : PowerModel
{
  public override PowerType Type => PowerType.Buff;

  public override PowerStackType StackType => PowerStackType.Counter;

  protected override bool IsVisibleInternal => false;

  public override bool ShouldTakeExtraTurn(Player player)
  {
    return this.Amount > 0 && player == this.Owner.Player;
  }

  public override async Task AfterTakingExtraTurn(Player player)
  {
    if (player != this.Owner.Player)
      return;
    await PowerCmd.Decrement((PowerModel) this);
  }
}
