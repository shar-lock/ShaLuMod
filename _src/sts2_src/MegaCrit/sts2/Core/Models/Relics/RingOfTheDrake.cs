// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Relics.RingOfTheDrake
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using System;
using System.Collections.Generic;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Relics;

public sealed class RingOfTheDrake : RelicModel
{
  private const string _turnsKey = "Turns";

  public override RelicRarity Rarity => RelicRarity.Starter;

  protected override IEnumerable<DynamicVar> CanonicalVars
  {
    get
    {
      return (IEnumerable<DynamicVar>) new \u003C\u003Ez__ReadOnlyArray<DynamicVar>(new DynamicVar[2]
      {
        (DynamicVar) new CardsVar(2),
        new DynamicVar("Turns", 3M)
      });
    }
  }

  public override Decimal ModifyHandDraw(Player player, Decimal count)
  {
    return player != this.Owner || (Decimal) this.Owner.PlayerCombatState.TurnNumber > this.DynamicVars["Turns"].BaseValue ? count : count + this.DynamicVars.Cards.BaseValue;
  }
}
