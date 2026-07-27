// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Relics.BigMushroom
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Rooms;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Relics;

public sealed class BigMushroom : RelicModel
{
  public override RelicRarity Rarity => RelicRarity.Event;

  public override bool HasUponPickupEffect => true;

  protected override IEnumerable<DynamicVar> CanonicalVars
  {
    get
    {
      return (IEnumerable<DynamicVar>) new \u003C\u003Ez__ReadOnlyArray<DynamicVar>(new DynamicVar[2]
      {
        (DynamicVar) new MaxHpVar(20M),
        (DynamicVar) new CardsVar(2)
      });
    }
  }

  public override async Task AfterObtained()
  {
    await CreatureCmd.GainMaxHp(this.Owner.Creature, this.DynamicVars.MaxHp.BaseValue);
    this.Grow();
  }

  public override Task AfterRoomEntered(AbstractRoom _)
  {
    this.Grow();
    return Task.CompletedTask;
  }

  public override Decimal ModifyHandDraw(Player player, Decimal cardsToDraw)
  {
    return player != this.Owner || this.Owner.PlayerCombatState.TurnNumber != 1 ? cardsToDraw : cardsToDraw - (Decimal) this.DynamicVars.Cards.IntValue;
  }

  private void Grow()
  {
    NCombatRoom.Instance?.GetCreatureNode(this.Owner.Creature)?.ScaleTo(1.5f, 0.0);
  }
}
