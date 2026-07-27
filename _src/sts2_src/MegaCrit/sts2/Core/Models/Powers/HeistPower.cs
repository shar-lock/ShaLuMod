// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Powers.HeistPower
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Rewards;
using MegaCrit.Sts2.Core.Rooms;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Powers;

public sealed class HeistPower : PowerModel
{
  public override PowerType Type => PowerType.Buff;

  public override PowerStackType StackType => PowerStackType.Counter;

  public override PowerInstanceType InstanceType => PowerInstanceType.Instanced;

  public override Task BeforeDeath(Creature target)
  {
    if (this.Owner != target)
      return Task.CompletedTask;
    if (this.CombatState.RunState.CurrentRoom is CombatRoom currentRoom)
      currentRoom.AddExtraReward(this.Target.Player, (Reward) new GoldReward(this.Amount, this.Target.Player, true));
    this.CombatState.RunState.CurrentMapPointHistoryEntry?.GetEntry(this.Target.Player.NetId).MarkLootReturned(this.Amount);
    return Task.CompletedTask;
  }
}
