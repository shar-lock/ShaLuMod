// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Combat.History.Entries.DamageReceivedEntry
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Models;
using System.Collections.Generic;

#nullable enable
namespace MegaCrit.Sts2.Core.Combat.History.Entries;

public class DamageReceivedEntry : CombatHistoryEntry
{
  public DamageResult Result { get; }

  public Creature? Dealer { get; }

  public CardModel? CardSource { get; }

  public Creature Receiver => this.Actor;

  public override string Description
  {
    get
    {
      string id = DamageReceivedEntry.GetId(this.Receiver);
      if (this.Dealer != null)
        return $"{DamageReceivedEntry.GetId(this.Dealer)} dealt {this.Result.UnblockedDamage} damage to {id}";
      return $"{id} took {this.Result.UnblockedDamage} damage";
    }
  }

  public DamageReceivedEntry(
    DamageResult result,
    Creature receiver,
    Creature? dealer,
    CardModel? cardSource,
    int roundNumber,
    CombatSide currentSide,
    CombatHistory history,
    IEnumerable<Player> players)
    : base(receiver, roundNumber, currentSide, history, players)
  {
    this.Result = result;
    this.Dealer = dealer;
    this.CardSource = cardSource;
  }

  private static string GetId(Creature creature)
  {
    return !creature.IsPlayer ? creature.Monster.Id.Entry : creature.Player.Character.Id.Entry;
  }
}
