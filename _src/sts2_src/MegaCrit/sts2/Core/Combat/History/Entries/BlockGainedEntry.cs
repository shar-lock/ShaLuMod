// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Combat.History.Entries.BlockGainedEntry
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.ValueProps;
using System.Collections.Generic;

#nullable enable
namespace MegaCrit.Sts2.Core.Combat.History.Entries;

public class BlockGainedEntry : CombatHistoryEntry
{
  public int Amount { get; }

  public Creature Receiver => this.Actor;

  public ValueProp Props { get; }

  public CardPlay? CardPlay { get; }

  public override string Description
  {
    get => $"{BlockGainedEntry.GetId(this.Receiver)} gained {this.Amount} block";
  }

  public BlockGainedEntry(
    int amount,
    ValueProp props,
    CardPlay? cardPlay,
    Creature receiver,
    int roundNumber,
    CombatSide currentSide,
    CombatHistory history,
    IEnumerable<Player> players)
    : base(receiver, roundNumber, currentSide, history, players)
  {
    this.Amount = amount;
    this.Props = props;
    this.CardPlay = cardPlay;
  }

  private static string GetId(Creature creature)
  {
    return !creature.IsPlayer ? creature.Monster.Id.Entry : creature.Player.Character.Id.Entry;
  }
}
