// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Powers.FastenPower
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.ValueProps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Powers;

public class FastenPower : PowerModel
{
  public override PowerType Type => PowerType.Buff;

  public override PowerStackType StackType => PowerStackType.Counter;

  protected override IEnumerable<IHoverTip> ExtraHoverTips
  {
    get
    {
      return (IEnumerable<IHoverTip>) new \u003C\u003Ez__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.Static(StaticHoverTip.Block));
    }
  }

  public override Decimal ModifyBlockAdditive(
    Creature target,
    Decimal block,
    ValueProp props,
    CardModel? cardSource,
    CardPlay? cardPlay)
  {
    return this.Owner != target || !props.IsPoweredCardOrMonsterMoveBlock() || cardSource != null && !cardSource.Tags.Contains<CardTag>(CardTag.Defend) ? 0M : (Decimal) this.Amount;
  }

  public override Task AfterModifyingBlockAmount(
    Decimal modifiedBlock,
    CardModel? cardSource,
    CardPlay? cardPlay)
  {
    this.Flash();
    return Task.CompletedTask;
  }
}
