// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Relics.ThrowingAxe
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Rooms;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Relics;

public sealed class ThrowingAxe : RelicModel
{
  private bool _usedThisCombat;

  public override RelicRarity Rarity => RelicRarity.Ancient;

  private bool UsedThisCombat
  {
    get => this._usedThisCombat;
    set
    {
      this.AssertMutable();
      this._usedThisCombat = value;
    }
  }

  public override Task AfterRoomEntered(AbstractRoom room)
  {
    if (!(room is CombatRoom))
      return Task.CompletedTask;
    this.UsedThisCombat = false;
    this.Status = RelicStatus.Active;
    return Task.CompletedTask;
  }

  public override int ModifyCardPlayCount(CardModel card, Creature? target, int playCount)
  {
    return this.UsedThisCombat || card.Owner != this.Owner ? playCount : playCount + 1;
  }

  public override Task AfterModifyingCardPlayCount(CardModel card)
  {
    this.UsedThisCombat = true;
    this.Flash();
    this.Status = RelicStatus.Normal;
    return Task.CompletedTask;
  }

  public override Task AfterCombatEnd(CombatRoom _)
  {
    this.UsedThisCombat = false;
    this.Status = RelicStatus.Normal;
    return Task.CompletedTask;
  }
}
