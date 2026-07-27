// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Powers.CorruptionPower
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.HoverTips;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Powers;

public sealed class CorruptionPower : PowerModel
{
  public override PowerType Type => PowerType.Buff;

  public override PowerStackType StackType => PowerStackType.Single;

  protected override IEnumerable<IHoverTip> ExtraHoverTips
  {
    get
    {
      return (IEnumerable<IHoverTip>) new \u003C\u003Ez__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromKeyword(CardKeyword.Exhaust));
    }
  }

  public override bool TryModifyEnergyCostInCombatLate(
    CardModel card,
    Decimal originalCost,
    out Decimal modifiedCost)
  {
    if (card.Owner.Creature != this.Owner || card.Type != CardType.Skill)
    {
      modifiedCost = originalCost;
      return false;
    }
    modifiedCost = 0M;
    return true;
  }

  public override CardLocation ModifyCardPlayResultLocation(
    CardModel card,
    bool isAutoPlay,
    ResourceInfo resources,
    CardLocation location)
  {
    if (card.Owner.Creature != this.Owner || card.Type != CardType.Skill)
      return location;
    location.pileType = PileType.Exhaust;
    return location;
  }

  public override Task AfterModifyingCardPlayResultLocation(
    CardModel card,
    CardLocation cardLocation)
  {
    this.Flash();
    return Task.CompletedTask;
  }
}
