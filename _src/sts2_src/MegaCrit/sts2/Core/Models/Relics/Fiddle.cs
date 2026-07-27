// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Relics.Fiddle
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Relics;

public sealed class Fiddle : RelicModel
{
  public override RelicRarity Rarity => RelicRarity.Ancient;

  protected override IEnumerable<DynamicVar> CanonicalVars
  {
    get
    {
      return (IEnumerable<DynamicVar>) new \u003C\u003Ez__ReadOnlySingleElementList<DynamicVar>((DynamicVar) new CardsVar(3));
    }
  }

  public override Decimal ModifyHandDrawLate(Player player, Decimal count)
  {
    return player != this.Owner ? count : count + (Decimal) this.DynamicVars.Cards.IntValue;
  }

  public override bool ShouldDraw(Player player, bool fromHandDraw)
  {
    return fromHandDraw || player != this.Owner || player.Creature.Side != player.Creature.CombatState.CurrentSide;
  }

  public override Task AfterPreventingDraw()
  {
    this.Flash();
    return Task.CompletedTask;
  }
}
