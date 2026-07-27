// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Relics.Pantograph
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Rooms;
using System.Collections.Generic;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Relics;

public sealed class Pantograph : RelicModel
{
  public override RelicRarity Rarity => RelicRarity.Uncommon;

  protected override IEnumerable<DynamicVar> CanonicalVars
  {
    get
    {
      return (IEnumerable<DynamicVar>) new \u003C\u003Ez__ReadOnlySingleElementList<DynamicVar>((DynamicVar) new HealVar(25M));
    }
  }

  public override Task AfterRoomEntered(AbstractRoom room)
  {
    if (this.Owner.Creature.IsDead)
      return Task.CompletedTask;
    this.Status = this.Owner.RunState.Map.BossMapPoint.parents.Contains(this.Owner.RunState.CurrentMapPoint) ? RelicStatus.Active : RelicStatus.Normal;
    return Task.CompletedTask;
  }

  public override async Task BeforeCombatStart()
  {
    if (this.Owner.Creature.IsDead || this.Owner.RunState.CurrentRoom.RoomType != RoomType.Boss)
      return;
    this.Flash();
    await CreatureCmd.Heal(this.Owner.Creature, this.DynamicVars.Heal.BaseValue);
  }
}
