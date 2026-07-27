// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Powers.CuriousPower
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Powers;
using System;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Powers;

public sealed class CuriousPower : PowerModel
{
  public override PowerType Type => PowerType.Buff;

  public override PowerStackType StackType => PowerStackType.Counter;

  public override bool TryModifyEnergyCostInCombat(
    CardModel card,
    Decimal originalCost,
    out Decimal modifiedCost)
  {
    modifiedCost = originalCost;
    if (card.Owner.Creature != this.Owner || card.Type != CardType.Power || originalCost <= 0M)
      return false;
    modifiedCost = originalCost - (Decimal) this.Amount;
    if (modifiedCost < 0M)
      modifiedCost = 0M;
    return true;
  }
}
