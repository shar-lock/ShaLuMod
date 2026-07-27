// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Context.LocalContext
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.Saves;
using MegaCrit.Sts2.Core.Saves.Runs;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable enable
namespace MegaCrit.Sts2.Core.Context;

public static class LocalContext
{
  public static ulong? NetId { get; set; }

  public static Player? GetMe(IPlayerCollection? playerCollection)
  {
    if (!LocalContext.NetId.HasValue || playerCollection == null)
      return (Player) null;
    return playerCollection.GetPlayer(LocalContext.NetId.Value) ?? throw new InvalidOperationException("Local player not found in player collection.");
  }

  public static SerializablePlayer? GetMe(SerializableRun? run)
  {
    if (!LocalContext.NetId.HasValue || run == null)
      return (SerializablePlayer) null;
    return run.Players.FirstOrDefault<SerializablePlayer>((Func<SerializablePlayer, bool>) (p => (long) p.NetId == (long) LocalContext.NetId.Value)) ?? throw new InvalidOperationException("Local player not found in serializable run.");
  }

  public static Player? GetMe(ICombatState? combatState)
  {
    if (!LocalContext.NetId.HasValue || combatState == null)
      return (Player) null;
    return combatState.GetPlayer(LocalContext.NetId.Value) ?? throw new InvalidOperationException("Local player not found in combat.");
  }

  public static Player? GetMe(IEnumerable<Player> players)
  {
    return !LocalContext.NetId.HasValue ? (Player) null : players.FirstOrDefault<Player>((Func<Player, bool>) (player =>
    {
      long netId1 = (long) player.NetId;
      ulong? netId2 = LocalContext.NetId;
      long valueOrDefault = (long) netId2.GetValueOrDefault();
      return netId1 == valueOrDefault & netId2.HasValue;
    }));
  }

  public static Creature? GetMe(IEnumerable<Creature> creatures)
  {
    return !LocalContext.NetId.HasValue ? (Creature) null : creatures.FirstOrDefault<Creature>((Func<Creature, bool>) (creature =>
    {
      ulong? netId1 = creature.Player?.NetId;
      ulong? netId2 = LocalContext.NetId;
      return (long) netId1.GetValueOrDefault() == (long) netId2.GetValueOrDefault() & netId1.HasValue == netId2.HasValue;
    }));
  }

  public static bool IsMe(Player? player)
  {
    if (player == null || !LocalContext.NetId.HasValue)
      return false;
    long netId1 = (long) player.NetId;
    ulong? netId2 = LocalContext.NetId;
    long valueOrDefault = (long) netId2.GetValueOrDefault();
    return netId1 == valueOrDefault & netId2.HasValue;
  }

  public static bool IsMe(Creature? creature) => LocalContext.IsMe(creature?.Player);

  public static bool ContainsMe(IEnumerable<Player> players)
  {
    // ISSUE: reference to a compiler-generated field
    // ISSUE: reference to a compiler-generated field
    return players.Any<Player>(LocalContext.\u003C\u003EO.\u003C0\u003E__IsMe ?? (LocalContext.\u003C\u003EO.\u003C0\u003E__IsMe = new Func<Player, bool>(LocalContext.IsMe)));
  }

  public static bool ContainsMe(IEnumerable<Creature> creatures)
  {
    // ISSUE: reference to a compiler-generated field
    // ISSUE: reference to a compiler-generated field
    return creatures.Any<Creature>(LocalContext.\u003C\u003EO.\u003C1\u003E__IsMe ?? (LocalContext.\u003C\u003EO.\u003C1\u003E__IsMe = new Func<Creature, bool>(LocalContext.IsMe)));
  }

  public static bool IsMine(CardModel? card)
  {
    return card != null && card.IsMutable && LocalContext.IsMe(card.Owner);
  }

  public static bool IsMine(PotionModel? potion)
  {
    return potion != null && potion.IsMutable && LocalContext.IsMe(potion.Owner);
  }

  public static bool IsMine(RelicModel? relic)
  {
    return relic != null && relic.IsMutable && LocalContext.IsMe(relic.Owner);
  }

  public static bool IsMine(EventModel? eventModel)
  {
    return eventModel != null && eventModel.IsMutable && LocalContext.IsMe(eventModel.Owner);
  }
}
