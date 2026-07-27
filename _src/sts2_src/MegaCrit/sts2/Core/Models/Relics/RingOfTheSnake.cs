// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Relics.RingOfTheSnake
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

public sealed class RingOfTheSnake : RelicModel
{
  public override RelicRarity Rarity => RelicRarity.Starter;

  protected override IEnumerable<DynamicVar> CanonicalVars
  {
    get
    {
      return (IEnumerable<DynamicVar>) new \u003C\u003Ez__ReadOnlySingleElementList<DynamicVar>((DynamicVar) new CardsVar(2));
    }
  }

  public override Decimal ModifyHandDraw(Player player, Decimal count)
  {
    return player != this.Owner || this.Owner.PlayerCombatState.TurnNumber > 1 ? count : count + this.DynamicVars.Cards.BaseValue;
  }
}
