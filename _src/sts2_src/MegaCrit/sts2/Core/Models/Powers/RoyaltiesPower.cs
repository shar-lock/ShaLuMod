// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Powers.RoyaltiesPower
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Rewards;
using MegaCrit.Sts2.Core.Rooms;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Powers;

public sealed class RoyaltiesPower : PowerModel
{
  public override PowerType Type => PowerType.Buff;

  public override PowerStackType StackType => PowerStackType.Counter;

  public override Task AfterCombatEnd(CombatRoom room)
  {
    room.AddExtraReward(this.Owner.Player, (Reward) new GoldReward(this.Amount, this.Owner.Player));
    return Task.CompletedTask;
  }
}
