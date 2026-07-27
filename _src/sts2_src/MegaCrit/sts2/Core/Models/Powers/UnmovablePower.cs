// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Powers.UnmovablePower
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Combat.History.Entries;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.ValueProps;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Powers;

public sealed class UnmovablePower : PowerModel
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

  public override Decimal ModifyBlockMultiplicative(
    Creature target,
    Decimal block,
    ValueProp props,
    CardModel? cardSource,
    CardPlay? cardPlay)
  {
    return target.IsMonster || !props.IsCardOrMonsterMove() || cardSource != null && cardSource.Owner.Creature != this.Owner || CombatManager.Instance.History.Entries.OfType<BlockGainedEntry>().Count<BlockGainedEntry>((Func<BlockGainedEntry, bool>) (e => e.HappenedThisTurn(this.CombatState) && e.CardPlay != null && e.CardPlay.Player.Creature == this.Owner && e.Props.IsCardOrMonsterMove() && e.CardPlay != cardPlay)) >= this.Amount ? 1M : 2M;
  }
}
