// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Powers.EchoFormPower
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Combat.History.Entries;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using System;
using System.Linq;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Powers;

public sealed class EchoFormPower : PowerModel
{
  public override PowerType Type => PowerType.Buff;

  public override PowerStackType StackType => PowerStackType.Counter;

  public override int ModifyCardPlayCount(CardModel card, Creature? target, int playCount)
  {
    return card.Owner.Creature != this.Owner || CombatManager.Instance.History.CardPlaysStarted.Count<CardPlayStartedEntry>((Func<CardPlayStartedEntry, bool>) (e => e.Actor == this.Owner && e.CardPlay.IsFirstInSeries && e.HappenedThisTurn(this.CombatState))) >= this.Amount ? playCount : playCount + 1;
  }

  public override Task AfterModifyingCardPlayCount(CardModel card)
  {
    this.Flash();
    return Task.CompletedTask;
  }
}
