// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Entities.Multiplayer.NetFullCombatState
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Entities.Rngs;
using MegaCrit.Sts2.Core.GameActions;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Multiplayer.Serialization;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.Saves;
using MegaCrit.Sts2.Core.Saves.Runs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

#nullable enable
namespace MegaCrit.Sts2.Core.Entities.Multiplayer;

public class NetFullCombatState : IPacketSerializable
{
  public List<uint> nextChoiceIds;
  public List<int> nextRewardIds;
  public uint? lastExecutedHookId;
  public uint? lastExecutedActionId;

  public List<NetFullCombatState.CreatureState> Creatures { get; private set; } = new List<NetFullCombatState.CreatureState>();

  public List<NetFullCombatState.PlayerState> Players { get; private set; } = new List<NetFullCombatState.PlayerState>();

  public SerializableRunRngSet Rng { get; private set; }

  public static NetFullCombatState FromRun(IRunState runState, GameAction? justFinishedAction)
  {
    NetFullCombatState netFullCombatState = new NetFullCombatState()
    {
      nextChoiceIds = new List<uint>(),
      nextRewardIds = new List<int>(),
      Creatures = new List<NetFullCombatState.CreatureState>(),
      Players = new List<NetFullCombatState.PlayerState>(),
      Rng = runState.Rng.ToSerializable()
    };
    netFullCombatState.nextChoiceIds.AddRange((IEnumerable<uint>) RunManager.Instance.PlayerChoiceSynchronizer.ChoiceIds);
    netFullCombatState.nextRewardIds.AddRange(RunManager.Instance.RewardsSetSynchronizer.GetNextRewardIds());
    netFullCombatState.lastExecutedHookId = justFinishedAction is GenericHookGameAction genericHookGameAction ? new uint?(genericHookGameAction.HookId) : new uint?();
    netFullCombatState.lastExecutedActionId = (uint?) justFinishedAction?.Id;
    ICombatState combatState = runState.Players[0].Creature.CombatState;
    foreach (Creature creature in (IEnumerable<Creature>) ((combatState != null ? (object) combatState.Creatures : (object) null) ?? (object) Array.Empty<Creature>()))
    {
      NetFullCombatState.CreatureState creatureState = new NetFullCombatState.CreatureState()
      {
        monsterId = creature.Monster?.Id,
        playerId = creature.Player?.NetId,
        currentHp = creature.CurrentHp,
        maxHp = creature.MaxHp,
        block = creature.Block,
        powers = new List<NetFullCombatState.PowerState>()
      };
      foreach (PowerModel power in (IEnumerable<PowerModel>) creature.Powers)
        creatureState.powers.Add(new NetFullCombatState.PowerState()
        {
          id = power.Id,
          amount = power.Amount
        });
      netFullCombatState.Creatures.Add(creatureState);
    }
    foreach (Player player in (IEnumerable<Player>) runState.Players)
    {
      PlayerCombatState playerCombatState = player.PlayerCombatState;
      NetFullCombatState.PlayerState playerState = new NetFullCombatState.PlayerState()
      {
        playerId = player.NetId,
        characterId = player.Character.Id,
        turnNumber = playerCombatState != null ? playerCombatState.TurnNumber : 0,
        phase = playerCombatState != null ? playerCombatState.Phase : PlayerTurnPhase.None,
        energy = playerCombatState != null ? playerCombatState.Energy : 0,
        stars = playerCombatState != null ? playerCombatState.Stars : 0,
        maxPotionCount = player.MaxPotionCount,
        gold = player.Gold,
        piles = new List<NetFullCombatState.CombatPileState>(),
        potions = new List<NetFullCombatState.PotionState>(),
        relics = new List<NetFullCombatState.RelicState>(),
        orbs = new List<NetFullCombatState.OrbState>(),
        rngSet = player.PlayerRng.ToSerializable(),
        relicGrabBag = player.RelicGrabBag.ToSerializable()
      };
      playerState.rngSet.Rngs.Remove(PlayerRngType.Rewards);
      playerState.rngSet.Rngs.Remove(PlayerRngType.Shops);
      if (playerCombatState != null && CombatManager.Instance.IsInProgress)
      {
        playerState.piles.Add(NetFullCombatState.CombatPileState.From(playerCombatState.Hand));
        playerState.piles.Add(NetFullCombatState.CombatPileState.From(playerCombatState.DrawPile));
        playerState.piles.Add(NetFullCombatState.CombatPileState.From(playerCombatState.DiscardPile));
        playerState.piles.Add(NetFullCombatState.CombatPileState.From(playerCombatState.ExhaustPile));
        playerState.piles.Add(NetFullCombatState.CombatPileState.From(playerCombatState.PlayPile));
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        playerState.orbs.AddRange(playerCombatState.OrbQueue.Orbs.Select<OrbModel, NetFullCombatState.OrbState>(NetFullCombatState.\u003C\u003EO.\u003C0\u003E__From ?? (NetFullCombatState.\u003C\u003EO.\u003C0\u003E__From = new Func<OrbModel, NetFullCombatState.OrbState>(NetFullCombatState.OrbState.From))));
      }
      foreach (PotionModel potion in player.Potions)
        playerState.potions.Add(new NetFullCombatState.PotionState()
        {
          id = potion.Id
        });
      foreach (RelicModel relic in (IEnumerable<RelicModel>) player.Relics)
        playerState.relics.Add(new NetFullCombatState.RelicState()
        {
          relic = relic.ToSerializable()
        });
      netFullCombatState.Players.Add(playerState);
    }
    return netFullCombatState;
  }

  public void Serialize(PacketWriter writer)
  {
    writer.WriteList<NetFullCombatState.CreatureState>((IReadOnlyList<NetFullCombatState.CreatureState>) this.Creatures);
    writer.WriteList<NetFullCombatState.PlayerState>((IReadOnlyList<NetFullCombatState.PlayerState>) this.Players);
    writer.Write<SerializableRunRngSet>(this.Rng);
    writer.WriteInt(this.nextChoiceIds.Count);
    foreach (uint nextChoiceId in this.nextChoiceIds)
      writer.WriteUInt(nextChoiceId);
    writer.WriteInt(this.nextRewardIds.Count);
    foreach (int nextRewardId in this.nextRewardIds)
      writer.WriteInt(nextRewardId);
    writer.WriteBool(this.lastExecutedActionId.HasValue);
    if (this.lastExecutedActionId.HasValue)
      writer.WriteUInt(this.lastExecutedActionId.Value);
    writer.WriteBool(this.lastExecutedHookId.HasValue);
    if (!this.lastExecutedHookId.HasValue)
      return;
    writer.WriteUInt(this.lastExecutedHookId.Value);
  }

  public void Deserialize(PacketReader reader)
  {
    this.Creatures = reader.ReadList<NetFullCombatState.CreatureState>();
    this.Players = reader.ReadList<NetFullCombatState.PlayerState>();
    this.Rng = reader.Read<SerializableRunRngSet>();
    int num1 = reader.ReadInt();
    this.nextChoiceIds = new List<uint>();
    for (int index = 0; index < num1; ++index)
      this.nextChoiceIds.Add(reader.ReadUInt());
    int num2 = reader.ReadInt();
    this.nextRewardIds = new List<int>();
    for (int index = 0; index < num2; ++index)
      this.nextRewardIds.Add(reader.ReadInt());
    if (reader.ReadBool())
      this.lastExecutedActionId = new uint?(reader.ReadUInt());
    if (!reader.ReadBool())
      return;
    this.lastExecutedHookId = new uint?(reader.ReadUInt());
  }

  public NetFullCombatState Anonymized()
  {
    return new NetFullCombatState()
    {
      Creatures = this.Creatures.Select<NetFullCombatState.CreatureState, NetFullCombatState.CreatureState>((Func<NetFullCombatState.CreatureState, NetFullCombatState.CreatureState>) (c => c.Anonymized())).ToList<NetFullCombatState.CreatureState>(),
      Players = this.Players.Select<NetFullCombatState.PlayerState, NetFullCombatState.PlayerState>((Func<NetFullCombatState.PlayerState, NetFullCombatState.PlayerState>) (p => p.Anonymized())).ToList<NetFullCombatState.PlayerState>(),
      Rng = this.Rng,
      nextChoiceIds = this.nextChoiceIds,
      nextRewardIds = this.nextRewardIds,
      lastExecutedHookId = this.lastExecutedHookId,
      lastExecutedActionId = this.lastExecutedActionId
    };
  }

  public override string ToString()
  {
    StringBuilder stringBuilder1 = new StringBuilder();
    StringBuilder stringBuilder2 = stringBuilder1;
    StringBuilder stringBuilder3 = stringBuilder2;
    StringBuilder.AppendInterpolatedStringHandler interpolatedStringHandler;
    // ISSUE: explicit constructor call
    ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).\u002Ector(12, 1, stringBuilder2);
    ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendLiteral("Choice IDs: ");
    ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendFormatted(string.Join<uint>(",", (IEnumerable<uint>) this.nextChoiceIds));
    ref StringBuilder.AppendInterpolatedStringHandler local1 = ref interpolatedStringHandler;
    stringBuilder3.AppendLine(ref local1);
    StringBuilder stringBuilder4 = stringBuilder1;
    StringBuilder stringBuilder5 = stringBuilder4;
    // ISSUE: explicit constructor call
    ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).\u002Ector(12, 1, stringBuilder4);
    ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendLiteral("Reward IDs: ");
    ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendFormatted(string.Join<int>(",", (IEnumerable<int>) this.nextRewardIds));
    ref StringBuilder.AppendInterpolatedStringHandler local2 = ref interpolatedStringHandler;
    stringBuilder5.AppendLine(ref local2);
    StringBuilder stringBuilder6 = stringBuilder1;
    StringBuilder stringBuilder7 = stringBuilder6;
    // ISSUE: explicit constructor call
    ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).\u002Ector(25, 1, stringBuilder6);
    ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendLiteral("Last executed action ID: ");
    ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendFormatted<uint?>(this.lastExecutedActionId);
    ref StringBuilder.AppendInterpolatedStringHandler local3 = ref interpolatedStringHandler;
    stringBuilder7.AppendLine(ref local3);
    StringBuilder stringBuilder8 = stringBuilder1;
    StringBuilder stringBuilder9 = stringBuilder8;
    // ISSUE: explicit constructor call
    ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).\u002Ector(23, 1, stringBuilder8);
    ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendLiteral("Last executed hook ID: ");
    ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendFormatted<uint?>(this.lastExecutedHookId);
    ref StringBuilder.AppendInterpolatedStringHandler local4 = ref interpolatedStringHandler;
    stringBuilder9.AppendLine(ref local4);
    foreach (NetFullCombatState.CreatureState creature in this.Creatures)
    {
      StringBuilder stringBuilder10 = stringBuilder1;
      StringBuilder stringBuilder11 = stringBuilder10;
      interpolatedStringHandler = new StringBuilder.AppendInterpolatedStringHandler(38, 2, stringBuilder10);
      ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendLiteral("Creature with monster ID: ");
      ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendFormatted<ModelId>(creature.monsterId);
      ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendLiteral(" player ID: ");
      ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendFormatted<ulong?>(creature.playerId);
      ref StringBuilder.AppendInterpolatedStringHandler local5 = ref interpolatedStringHandler;
      stringBuilder11.AppendLine(ref local5);
      StringBuilder stringBuilder12 = stringBuilder1;
      StringBuilder stringBuilder13 = stringBuilder12;
      interpolatedStringHandler = new StringBuilder.AppendInterpolatedStringHandler(44, 4, stringBuilder12);
      ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendLiteral("\tCurrent HP: ");
      ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendFormatted<int>(creature.currentHp);
      ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendLiteral(" Max HP: ");
      ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendFormatted<int>(creature.maxHp);
      ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendLiteral(" Block: ");
      ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendFormatted<int>(creature.block);
      ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendLiteral(" Power count: ");
      ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendFormatted<int>(creature.powers.Count);
      ref StringBuilder.AppendInterpolatedStringHandler local6 = ref interpolatedStringHandler;
      stringBuilder13.AppendLine(ref local6);
      foreach (NetFullCombatState.PowerState power in creature.powers)
      {
        StringBuilder stringBuilder14 = stringBuilder1;
        StringBuilder stringBuilder15 = stringBuilder14;
        interpolatedStringHandler = new StringBuilder.AppendInterpolatedStringHandler(16 /*0x10*/, 2, stringBuilder14);
        ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendLiteral("\tPower ");
        ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendFormatted<ModelId>(power.id);
        ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendLiteral(" Amount: ");
        ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendFormatted<int>(power.amount);
        ref StringBuilder.AppendInterpolatedStringHandler local7 = ref interpolatedStringHandler;
        stringBuilder15.AppendLine(ref local7);
      }
    }
    foreach (NetFullCombatState.PlayerState player in this.Players)
    {
      StringBuilder stringBuilder16 = stringBuilder1;
      StringBuilder stringBuilder17 = stringBuilder16;
      interpolatedStringHandler = new StringBuilder.AppendInterpolatedStringHandler(28, 2, stringBuilder16);
      ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendLiteral("Player with ID: ");
      ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendFormatted<ulong>(player.playerId);
      ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendLiteral(" Character: ");
      ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendFormatted<ModelId>(player.characterId);
      ref StringBuilder.AppendInterpolatedStringHandler local8 = ref interpolatedStringHandler;
      stringBuilder17.AppendLine(ref local8);
      StringBuilder stringBuilder18 = stringBuilder1;
      StringBuilder stringBuilder19 = stringBuilder18;
      interpolatedStringHandler = new StringBuilder.AppendInterpolatedStringHandler(15, 2, stringBuilder18);
      ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendLiteral("\tTurn: ");
      ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendFormatted<int>(player.turnNumber);
      ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendLiteral(" Phase: ");
      ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendFormatted<PlayerTurnPhase>(player.phase);
      ref StringBuilder.AppendInterpolatedStringHandler local9 = ref interpolatedStringHandler;
      stringBuilder19.AppendLine(ref local9);
      StringBuilder stringBuilder20 = stringBuilder1;
      StringBuilder stringBuilder21 = stringBuilder20;
      interpolatedStringHandler = new StringBuilder.AppendInterpolatedStringHandler(38, 4, stringBuilder20);
      ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendLiteral("\tEnergy: ");
      ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendFormatted<int>(player.energy);
      ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendLiteral(" Stars: ");
      ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendFormatted<int>(player.stars);
      ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendLiteral(" Max Potions: ");
      ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendFormatted<int>(player.maxPotionCount);
      ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendLiteral(" Gold: ");
      ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendFormatted<int>(player.gold);
      ref StringBuilder.AppendInterpolatedStringHandler local10 = ref interpolatedStringHandler;
      stringBuilder21.AppendLine(ref local10);
      StringBuilder stringBuilder22 = stringBuilder1;
      StringBuilder stringBuilder23 = stringBuilder22;
      interpolatedStringHandler = new StringBuilder.AppendInterpolatedStringHandler(54, 4, stringBuilder22);
      ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendLiteral("\tPile Count: ");
      ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendFormatted<int>(player.piles.Count);
      ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendLiteral(" Potion Count: ");
      ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendFormatted<int>(player.potions.Count);
      ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendLiteral(" Relic Count: ");
      ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendFormatted<int>(player.relics.Count);
      ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendLiteral(" Orb Count: ");
      ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendFormatted<int>(player.orbs.Count);
      ref StringBuilder.AppendInterpolatedStringHandler local11 = ref interpolatedStringHandler;
      stringBuilder23.AppendLine(ref local11);
      foreach (NetFullCombatState.CombatPileState pile in player.piles)
      {
        StringBuilder stringBuilder24 = stringBuilder1;
        StringBuilder stringBuilder25 = stringBuilder24;
        interpolatedStringHandler = new StringBuilder.AppendInterpolatedStringHandler(6, 1, stringBuilder24);
        ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendLiteral("\tPile ");
        ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendFormatted<PileType>(pile.pileType);
        ref StringBuilder.AppendInterpolatedStringHandler local12 = ref interpolatedStringHandler;
        stringBuilder25.AppendLine(ref local12);
        foreach (NetFullCombatState.CardState card in pile.cards)
        {
          StringBuilder stringBuilder26 = stringBuilder1;
          StringBuilder stringBuilder27 = stringBuilder26;
          interpolatedStringHandler = new StringBuilder.AppendInterpolatedStringHandler(19, 1, stringBuilder26);
          ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendLiteral("\t\tSerialized card: ");
          ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendFormatted<SerializableCard>(card.card);
          ref StringBuilder.AppendInterpolatedStringHandler local13 = ref interpolatedStringHandler;
          stringBuilder27.AppendLine(ref local13);
          if (card.affliction != (ModelId) null)
          {
            StringBuilder stringBuilder28 = stringBuilder1;
            StringBuilder stringBuilder29 = stringBuilder28;
            interpolatedStringHandler = new StringBuilder.AppendInterpolatedStringHandler(34, 2, stringBuilder28);
            ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendLiteral("\t\t\tAffliction: ");
            ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendFormatted<ModelId>(card.affliction);
            ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendLiteral(" Affliction Count: ");
            ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendFormatted<int>(card.afflictionCount);
            ref StringBuilder.AppendInterpolatedStringHandler local14 = ref interpolatedStringHandler;
            stringBuilder29.AppendLine(ref local14);
          }
          if (card.keywords != null)
          {
            StringBuilder stringBuilder30 = stringBuilder1;
            StringBuilder stringBuilder31 = stringBuilder30;
            interpolatedStringHandler = new StringBuilder.AppendInterpolatedStringHandler(13, 1, stringBuilder30);
            ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendLiteral("\t\t\tKeywords: ");
            ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendFormatted(string.Join<CardKeyword>(",", (IEnumerable<CardKeyword>) card.keywords));
            ref StringBuilder.AppendInterpolatedStringHandler local15 = ref interpolatedStringHandler;
            stringBuilder31.AppendLine(ref local15);
          }
          if (card.energyCost.HasValue)
          {
            StringBuilder stringBuilder32 = stringBuilder1;
            StringBuilder stringBuilder33 = stringBuilder32;
            interpolatedStringHandler = new StringBuilder.AppendInterpolatedStringHandler(16 /*0x10*/, 1, stringBuilder32);
            ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendLiteral("\t\t\tEnergy Cost: ");
            ref StringBuilder.AppendInterpolatedStringHandler local16 = ref interpolatedStringHandler;
            object energyCost = (object) card.energyCost;
            string str = string.Join(",", new ReadOnlySpan<object>(ref energyCost));
            ((StringBuilder.AppendInterpolatedStringHandler) ref local16).AppendFormatted(str);
            ref StringBuilder.AppendInterpolatedStringHandler local17 = ref interpolatedStringHandler;
            stringBuilder33.AppendLine(ref local17);
          }
        }
      }
      foreach (NetFullCombatState.PotionState potion in player.potions)
      {
        StringBuilder stringBuilder34 = stringBuilder1;
        StringBuilder stringBuilder35 = stringBuilder34;
        interpolatedStringHandler = new StringBuilder.AppendInterpolatedStringHandler(8, 1, stringBuilder34);
        ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendLiteral("\tPotion ");
        ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendFormatted<ModelId>(potion.id);
        ref StringBuilder.AppendInterpolatedStringHandler local18 = ref interpolatedStringHandler;
        stringBuilder35.AppendLine(ref local18);
      }
      foreach (NetFullCombatState.RelicState relic in player.relics)
      {
        StringBuilder stringBuilder36 = stringBuilder1;
        StringBuilder stringBuilder37 = stringBuilder36;
        interpolatedStringHandler = new StringBuilder.AppendInterpolatedStringHandler(29, 3, stringBuilder36);
        ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendLiteral("\tRelic ");
        ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendFormatted<ModelId>(relic.relic.Id);
        ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendLiteral(" Props: ");
        ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendFormatted<SavedProperties>(relic.relic.Props);
        ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendLiteral(" Floor added: ");
        ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendFormatted<int?>(relic.relic.FloorAddedToDeck);
        ref StringBuilder.AppendInterpolatedStringHandler local19 = ref interpolatedStringHandler;
        stringBuilder37.AppendLine(ref local19);
      }
      foreach (NetFullCombatState.OrbState orb in player.orbs)
      {
        StringBuilder stringBuilder38 = stringBuilder1;
        StringBuilder stringBuilder39 = stringBuilder38;
        interpolatedStringHandler = new StringBuilder.AppendInterpolatedStringHandler(24, 3, stringBuilder38);
        ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendLiteral("\tOrb ");
        ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendFormatted<ModelId>(orb.id);
        ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendLiteral(", passive: ");
        ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendFormatted<int>(orb.passive);
        ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendLiteral(" evoke: ");
        ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendFormatted<int>(orb.evoke);
        ref StringBuilder.AppendInterpolatedStringHandler local20 = ref interpolatedStringHandler;
        stringBuilder39.AppendLine(ref local20);
      }
      StringBuilder stringBuilder40 = stringBuilder1;
      StringBuilder stringBuilder41 = stringBuilder40;
      interpolatedStringHandler = new StringBuilder.AppendInterpolatedStringHandler(17, 1, stringBuilder40);
      ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendLiteral("Player RNG seed: ");
      ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendFormatted<ulong>(player.rngSet.Seed);
      ref StringBuilder.AppendInterpolatedStringHandler local21 = ref interpolatedStringHandler;
      stringBuilder41.AppendLine(ref local21);
      foreach (PlayerRngType key in Enum.GetValues<PlayerRngType>())
      {
        SerializableRng serializableRng;
        if (player.rngSet.Rngs.TryGetValue(key, out serializableRng))
        {
          StringBuilder stringBuilder42 = stringBuilder1;
          StringBuilder stringBuilder43 = stringBuilder42;
          interpolatedStringHandler = new StringBuilder.AppendInterpolatedStringHandler(15, 2, stringBuilder42);
          ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendLiteral("\tRNG counter ");
          ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendFormatted<PlayerRngType>(key);
          ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendLiteral(": ");
          ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendFormatted<SerializableRng>(serializableRng);
          ref StringBuilder.AppendInterpolatedStringHandler local22 = ref interpolatedStringHandler;
          stringBuilder43.AppendLine(ref local22);
        }
      }
      stringBuilder1.AppendLine("Player relic grab bag:");
      foreach (RelicRarity key in Enum.GetValues<RelicRarity>())
      {
        List<ModelId> source;
        if (player.relicGrabBag.RelicIdLists.TryGetValue(key, out source))
        {
          StringBuilder stringBuilder44 = stringBuilder1;
          StringBuilder stringBuilder45 = stringBuilder44;
          interpolatedStringHandler = new StringBuilder.AppendInterpolatedStringHandler(10, 2, stringBuilder44);
          ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendLiteral("\tRarity ");
          ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendFormatted<RelicRarity>(key);
          ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendLiteral(": ");
          ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendFormatted(string.Join(",", source.Select<ModelId, string>((Func<ModelId, string>) (m => m.Entry))));
          ref StringBuilder.AppendInterpolatedStringHandler local23 = ref interpolatedStringHandler;
          stringBuilder45.AppendLine(ref local23);
        }
      }
    }
    StringBuilder stringBuilder46 = stringBuilder1;
    StringBuilder stringBuilder47 = stringBuilder46;
    // ISSUE: explicit constructor call
    ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).\u002Ector(17, 1, stringBuilder46);
    ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendLiteral("RNG global seed: ");
    ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendFormatted(this.Rng.Seed);
    ref StringBuilder.AppendInterpolatedStringHandler local24 = ref interpolatedStringHandler;
    stringBuilder47.AppendLine(ref local24);
    foreach (RunRngType key in Enum.GetValues<RunRngType>())
    {
      SerializableRng serializableRng;
      if (this.Rng.Rngs.TryGetValue(key, out serializableRng))
      {
        StringBuilder stringBuilder48 = stringBuilder1;
        StringBuilder stringBuilder49 = stringBuilder48;
        // ISSUE: explicit constructor call
        ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).\u002Ector(15, 2, stringBuilder48);
        ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendLiteral("\tRNG counter ");
        ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendFormatted<RunRngType>(key);
        ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendLiteral(": ");
        ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendFormatted<SerializableRng>(serializableRng);
        ref StringBuilder.AppendInterpolatedStringHandler local25 = ref interpolatedStringHandler;
        stringBuilder49.AppendLine(ref local25);
      }
    }
    return stringBuilder1.ToString();
  }

  public struct CreatureState : IPacketSerializable
  {
    public ModelId? monsterId;
    public ulong? playerId;
    public int currentHp;
    public int maxHp;
    public int block;
    public List<NetFullCombatState.PowerState> powers;

    public void Serialize(PacketWriter writer)
    {
      writer.WriteBool(this.monsterId != (ModelId) null);
      if (this.monsterId != (ModelId) null)
        writer.WriteModelEntry(this.monsterId);
      writer.WriteBool(this.playerId.HasValue);
      if (this.playerId.HasValue)
        writer.WriteULong(this.playerId.Value);
      writer.WriteInt(this.currentHp);
      writer.WriteInt(this.maxHp);
      writer.WriteInt(this.block);
      writer.WriteList<NetFullCombatState.PowerState>((IReadOnlyList<NetFullCombatState.PowerState>) this.powers);
    }

    public void Deserialize(PacketReader reader)
    {
      if (reader.ReadBool())
        this.monsterId = reader.ReadModelIdAssumingType<MonsterModel>();
      if (reader.ReadBool())
        this.playerId = new ulong?(reader.ReadULong());
      this.currentHp = reader.ReadInt();
      this.maxHp = reader.ReadInt();
      this.block = reader.ReadInt();
      this.powers = reader.ReadList<NetFullCombatState.PowerState>();
    }

    public NetFullCombatState.CreatureState Anonymized()
    {
      return this with
      {
        playerId = this.playerId.HasValue ? new ulong?(IdAnonymizer.Anonymize(this.playerId.Value)) : new ulong?()
      };
    }
  }

  public struct PowerState : IPacketSerializable
  {
    public ModelId id;
    public int amount;

    public void Serialize(PacketWriter writer)
    {
      writer.WriteModelEntry(this.id);
      writer.WriteInt(this.amount);
    }

    public void Deserialize(PacketReader reader)
    {
      this.id = reader.ReadModelIdAssumingType<PowerModel>();
      this.amount = reader.ReadInt();
    }
  }

  public struct OrbState : IPacketSerializable
  {
    public ModelId id;
    public int passive;
    public int evoke;

    public void Serialize(PacketWriter writer)
    {
      writer.WriteModelEntry(this.id);
      writer.WriteInt(this.passive, 16 /*0x10*/);
      writer.WriteInt(this.evoke, 16 /*0x10*/);
    }

    public void Deserialize(PacketReader reader)
    {
      this.id = reader.ReadModelIdAssumingType<OrbModel>();
      this.passive = reader.ReadInt(16 /*0x10*/);
      this.evoke = reader.ReadInt(16 /*0x10*/);
    }

    public static NetFullCombatState.OrbState From(OrbModel orb)
    {
      return new NetFullCombatState.OrbState()
      {
        id = orb.Id,
        passive = (int) orb.PassiveVal,
        evoke = (int) orb.EvokeVal
      };
    }
  }

  public struct PlayerState : IPacketSerializable
  {
    public ulong playerId;
    public ModelId characterId;
    public int turnNumber;
    public PlayerTurnPhase phase;
    public int energy;
    public int stars;
    public int maxPotionCount;
    public int gold;
    public List<NetFullCombatState.CombatPileState> piles;
    public List<NetFullCombatState.PotionState> potions;
    public List<NetFullCombatState.RelicState> relics;
    public List<NetFullCombatState.OrbState> orbs;
    public SerializablePlayerRngSet rngSet;
    public SerializableRelicGrabBag relicGrabBag;

    public void Serialize(PacketWriter writer)
    {
      writer.WriteULong(this.playerId);
      writer.WriteModelEntry(this.characterId);
      writer.WriteInt(this.turnNumber);
      writer.WriteEnum<PlayerTurnPhase>(this.phase);
      writer.WriteInt(this.energy);
      writer.WriteInt(this.stars);
      writer.WriteInt(this.maxPotionCount);
      writer.WriteInt(this.gold);
      writer.WriteList<NetFullCombatState.CombatPileState>((IReadOnlyList<NetFullCombatState.CombatPileState>) this.piles);
      writer.WriteList<NetFullCombatState.PotionState>((IReadOnlyList<NetFullCombatState.PotionState>) this.potions);
      writer.WriteList<NetFullCombatState.RelicState>((IReadOnlyList<NetFullCombatState.RelicState>) this.relics);
      writer.WriteList<NetFullCombatState.OrbState>((IReadOnlyList<NetFullCombatState.OrbState>) this.orbs);
      writer.Write<SerializablePlayerRngSet>(this.rngSet);
      writer.Write<SerializableRelicGrabBag>(this.relicGrabBag);
    }

    public void Deserialize(PacketReader reader)
    {
      this.playerId = reader.ReadULong();
      this.characterId = reader.ReadModelIdAssumingType<CharacterModel>();
      this.turnNumber = reader.ReadInt();
      this.phase = reader.ReadEnum<PlayerTurnPhase>();
      this.energy = reader.ReadInt();
      this.stars = reader.ReadInt();
      this.maxPotionCount = reader.ReadInt();
      this.gold = reader.ReadInt();
      this.piles = reader.ReadList<NetFullCombatState.CombatPileState>();
      this.potions = reader.ReadList<NetFullCombatState.PotionState>();
      this.relics = reader.ReadList<NetFullCombatState.RelicState>();
      this.orbs = reader.ReadList<NetFullCombatState.OrbState>();
      this.rngSet = reader.Read<SerializablePlayerRngSet>();
      this.relicGrabBag = reader.Read<SerializableRelicGrabBag>();
    }

    public NetFullCombatState.PlayerState Anonymized()
    {
      return this with
      {
        playerId = IdAnonymizer.Anonymize(this.playerId)
      };
    }
  }

  public struct CombatPileState : IPacketSerializable
  {
    public PileType pileType;
    public List<NetFullCombatState.CardState> cards;

    public void Serialize(PacketWriter writer)
    {
      writer.WriteInt((int) this.pileType);
      writer.WriteList<NetFullCombatState.CardState>((IReadOnlyList<NetFullCombatState.CardState>) this.cards);
    }

    public void Deserialize(PacketReader reader)
    {
      this.pileType = (PileType) reader.ReadInt();
      this.cards = reader.ReadList<NetFullCombatState.CardState>();
    }

    public static NetFullCombatState.CombatPileState From(CardPile pile)
    {
      NetFullCombatState.CombatPileState combatPileState = new NetFullCombatState.CombatPileState();
      combatPileState.pileType = pile.Type;
      combatPileState.cards = new List<NetFullCombatState.CardState>();
      foreach (CardModel card in (IEnumerable<CardModel>) pile.Cards)
        combatPileState.cards.Add(NetFullCombatState.CardState.From(card));
      return combatPileState;
    }
  }

  public struct CardState : IPacketSerializable
  {
    public SerializableCard card;
    public ModelId? affliction;
    public int afflictionCount;
    public int? energyCost;
    public List<CardKeyword>? keywords;

    public void Serialize(PacketWriter writer)
    {
      writer.Write<SerializableCard>(this.card);
      writer.WriteBool(this.affliction != (ModelId) null);
      if (this.affliction != (ModelId) null)
      {
        writer.WriteModelEntry(this.affliction);
        writer.WriteInt(this.afflictionCount);
      }
      writer.WriteBool(this.energyCost.HasValue);
      if (this.energyCost.HasValue)
        writer.WriteInt(this.energyCost.Value);
      writer.WriteBool(this.keywords != null);
      if (this.keywords == null)
        return;
      writer.WriteInt(this.keywords.Count, 3);
      foreach (CardKeyword keyword in this.keywords)
        writer.WriteEnum<CardKeyword>(keyword);
    }

    public void Deserialize(PacketReader reader)
    {
      this.card = reader.Read<SerializableCard>();
      if (reader.ReadBool())
      {
        this.affliction = reader.ReadModelIdAssumingType<AfflictionModel>();
        this.afflictionCount = reader.ReadInt();
      }
      if (reader.ReadBool())
        this.energyCost = new int?(reader.ReadInt());
      if (!reader.ReadBool())
        return;
      this.keywords = new List<CardKeyword>();
      int num = reader.ReadInt(3);
      for (int index = 0; index < num; ++index)
        this.keywords.Add(reader.ReadEnum<CardKeyword>());
    }

    public static NetFullCombatState.CardState From(CardModel card)
    {
      NetFullCombatState.CardState cardState = new NetFullCombatState.CardState();
      cardState.card = card.ToSerializable();
      cardState.affliction = card.Affliction?.Id;
      ref NetFullCombatState.CardState local = ref cardState;
      AfflictionModel affliction = card.Affliction;
      int amount = affliction != null ? affliction.Amount : 0;
      local.afflictionCount = amount;
      IReadOnlySet<CardKeyword> keywords = card.Keywords;
      if (((IReadOnlyCollection<CardKeyword>) keywords).Count > 0)
      {
        cardState.keywords = new List<CardKeyword>();
        foreach (CardKeyword cardKeyword in Enum.GetValues<CardKeyword>())
        {
          if (keywords.Contains(cardKeyword))
            cardState.keywords.Add(cardKeyword);
        }
      }
      else
        cardState.keywords = (List<CardKeyword>) null;
      int withModifiers = card.EnergyCost.GetWithModifiers(CostModifiers.All);
      if (withModifiers != card.EnergyCost.Canonical)
        cardState.energyCost = new int?(withModifiers);
      return cardState;
    }
  }

  public struct PotionState : IPacketSerializable
  {
    public ModelId id;

    public void Serialize(PacketWriter writer) => writer.WriteModelEntry(this.id);

    public void Deserialize(PacketReader reader)
    {
      this.id = reader.ReadModelIdAssumingType<PotionModel>();
    }
  }

  public struct RelicState : IPacketSerializable
  {
    public SerializableRelic relic;

    public void Serialize(PacketWriter writer) => writer.Write<SerializableRelic>(this.relic);

    public void Deserialize(PacketReader reader) => this.relic = reader.Read<SerializableRelic>();
  }
}
