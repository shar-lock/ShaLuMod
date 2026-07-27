// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Localization.DynamicVars.SummonVar
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Hooks;
using MegaCrit.Sts2.Core.Models;
using System;

#nullable enable
namespace MegaCrit.Sts2.Core.Localization.DynamicVars;

public class SummonVar : DynamicVar
{
  public const string defaultName = "Summon";

  public SummonVar(Decimal summonAmount)
    : base("Summon", summonAmount)
  {
  }

  public SummonVar(string name, Decimal summonAmount)
    : base(name, summonAmount)
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
    this.PreviewValue = Hook.ModifySummonAmount(card.CombatState, card.Owner, this.BaseValue, (AbstractModel) card);
  }
}
