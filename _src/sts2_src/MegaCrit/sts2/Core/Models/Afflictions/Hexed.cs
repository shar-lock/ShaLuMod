// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Afflictions.Hexed
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models.Powers;
using System.Collections.Generic;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Afflictions;

public sealed class Hexed : AfflictionModel
{
  protected override IEnumerable<IHoverTip> ExtraHoverTips
  {
    get
    {
      return (IEnumerable<IHoverTip>) new \u003C\u003Ez__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromKeyword(CardKeyword.Ethereal));
    }
  }

  public override Task AfterCardEnteredCombat(CardModel card)
  {
    if (card != this.Card || card.Owner.Creature.HasPower<HexPower>())
      return Task.CompletedTask;
    CardCmd.ClearAffliction(this.Card);
    return Task.CompletedTask;
  }
}
