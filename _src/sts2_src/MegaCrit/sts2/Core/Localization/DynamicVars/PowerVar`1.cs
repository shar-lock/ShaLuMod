// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Localization.DynamicVars.PowerVar`1
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Hooks;
using MegaCrit.Sts2.Core.Models;
using System;
using System.Collections.Generic;

#nullable enable
namespace MegaCrit.Sts2.Core.Localization.DynamicVars;

public class PowerVar<T> : DynamicVar where T : PowerModel
{
  public PowerVar(Decimal powerAmount)
    : base(typeof (T).Name, powerAmount)
  {
  }

  public PowerVar(string name, Decimal powerAmount)
    : base(name, powerAmount)
  {
  }

  public override void UpdateCardPreview(
    CardModel card,
    CardPreviewMode previewMode,
    Creature? target,
    bool runGlobalHooks)
  {
    if (!runGlobalHooks)
      return;
    this.PreviewValue = Hook.ModifyPowerAmountGiven(card.CombatState, (PowerModel) ModelDb.Power<T>(), card.Owner.Creature, this.BaseValue, target, card, out IEnumerable<AbstractModel> _);
  }
}
