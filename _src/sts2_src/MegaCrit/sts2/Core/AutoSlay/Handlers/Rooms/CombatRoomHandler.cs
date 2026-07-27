// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.AutoSlay.Handlers.Rooms.CombatRoomHandler
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.AutoSlay.Helpers;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Random;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Runs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.AutoSlay.Handlers.Rooms;

public class CombatRoomHandler : IRoomHandler, IHandler
{
  public RoomType[] HandledTypes
  {
    get
    {
      return new RoomType[3]
      {
        RoomType.Monster,
        RoomType.Elite,
        RoomType.Boss
      };
    }
  }

  public TimeSpan Timeout => TimeSpan.FromMinutes(5L);

  public async Task HandleAsync(Rng random, CancellationToken ct)
  {
    AutoSlayLog.Action("Waiting for combat to start");
    await WaitHelper.Until((Func<bool>) (() => CombatManager.Instance.IsInProgress), ct, new TimeSpan?(AutoSlayConfig.nodeWaitTimeout), "Combat not started");
    AutoSlayLog.Action("Combat started, applying defensive buffs");
    Player player = LocalContext.GetMe((IPlayerCollection) RunManager.Instance.DebugOnlyGetState());
    Creature playerCreature = player.Creature;
    PlatingPower platingPower = await PowerCmd.Apply<PlatingPower>((PlayerChoiceContext) new ThrowingPlayerChoiceContext(), playerCreature, 999M, playerCreature, (CardModel) null);
    RegenPower regenPower = await PowerCmd.Apply<RegenPower>((PlayerChoiceContext) new ThrowingPlayerChoiceContext(), playerCreature, 999M, playerCreature, (CardModel) null);
    int turnCount = 0;
    while (CombatManager.Instance.IsInProgress && turnCount < 100)
    {
      ct.ThrowIfCancellationRequested();
      ++turnCount;
      await WaitHelper.Until((Func<bool>) (() =>
      {
        PlayerCombatState playerCombatState = player.PlayerCombatState;
        return (playerCombatState != null ? (playerCombatState.Phase == PlayerTurnPhase.Play ? 1 : 0) : 0) != 0 || !CombatManager.Instance.IsInProgress;
      }), ct, new TimeSpan?(TimeSpan.FromSeconds(30L)), "Play phase not started");
      if (CombatManager.Instance.IsInProgress)
      {
        Watchdog currentWatchdog1 = AutoSlayer.CurrentWatchdog;
        if (currentWatchdog1 != null)
          currentWatchdog1.Reset($"Combat turn {turnCount}");
        if (turnCount >= 3)
        {
          StrengthPower strengthPower = await PowerCmd.Apply<StrengthPower>((PlayerChoiceContext) new ThrowingPlayerChoiceContext(), playerCreature, 200M, playerCreature, (CardModel) null);
        }
        AutoSlayLog.Action($"Turn {turnCount}: playing cards");
        await CombatRoomHandler.UseAllPotions(player, random, ct);
        int cardsPlayed = 0;
        HashSet<CardModel> attemptedCards = new HashSet<CardModel>();
        while (cardsPlayed < 50)
        {
          PlayerCombatState playerCombatState = player.PlayerCombatState;
          if ((playerCombatState != null ? (playerCombatState.Phase == PlayerTurnPhase.Play ? 1 : 0) : 0) != 0)
          {
            ct.ThrowIfCancellationRequested();
            if (cardsPlayed > 0 && cardsPlayed % 10 == 0)
            {
              Watchdog currentWatchdog2 = AutoSlayer.CurrentWatchdog;
              if (currentWatchdog2 != null)
                currentWatchdog2.Reset($"Combat turn {turnCount}, played {cardsPlayed} cards");
            }
            List<CardModel> list = PileType.Hand.GetPile(player).Cards.Where<CardModel>((Func<CardModel, bool>) (c => c.CanPlay(out UnplayableReason _, out AbstractModel _) && !attemptedCards.Contains(c))).ToList<CardModel>();
            if (list.Count == 0)
            {
              AutoSlayLog.Action("No more playable cards, ending turn");
              break;
            }
            CardModel card = random.NextItem<CardModel>((IEnumerable<CardModel>) list);
            Creature randomTarget = CombatRoomHandler.GetRandomTarget(card, random);
            attemptedCards.Add(card);
            AutoSlayLog.Info("Playing " + card.Id.Entry);
            await CardCmd.AutoPlay((PlayerChoiceContext) new BlockingPlayerChoiceContext(), card, randomTarget);
            ++cardsPlayed;
            await Task.Delay(100, ct);
          }
          else
            break;
        }
        PlayerCombatState playerCombatState1 = player.PlayerCombatState;
        if ((playerCombatState1 != null ? (playerCombatState1.Phase == PlayerTurnPhase.Play ? 1 : 0) : 0) != 0 && CombatManager.Instance.IsInProgress)
          PlayerCmd.EndTurn(player, false);
      }
      else
        break;
    }
    await WaitHelper.Until((Func<bool>) (() => !CombatManager.Instance.IsInProgress), ct, new TimeSpan?(TimeSpan.FromSeconds(30L)), "Combat did not end");
    AutoSlayLog.Action("Combat finished");
    playerCreature = (Creature) null;
  }

  private static Creature? GetRandomTarget(CardModel card, Rng random)
  {
    if (card.TargetType != TargetType.AnyEnemy)
      return (Creature) null;
    ICombatState combatState = card.CombatState;
    if (combatState == null)
      return (Creature) null;
    List<Creature> list = combatState.HittableEnemies.ToList<Creature>();
    return list.Count == 0 ? (Creature) null : random.NextItem<Creature>((IEnumerable<Creature>) list);
  }

  private static async Task UseAllPotions(Player player, Rng random, CancellationToken ct)
  {
    List<PotionModel> list = player.Potions.ToList<PotionModel>();
    ICombatState combatState;
    if (list.Count == 0)
    {
      combatState = (ICombatState) null;
    }
    else
    {
      AutoSlayLog.Action($"Using {list.Count} potion(s)");
      combatState = player.Creature.CombatState;
      foreach (PotionModel potion in list)
      {
        ct.ThrowIfCancellationRequested();
        PlayerCombatState playerCombatState = player.PlayerCombatState;
        if ((playerCombatState != null ? (playerCombatState.Phase != PlayerTurnPhase.Play ? 1 : 0) : 1) == 0)
        {
          if (CombatManager.Instance.IsInProgress)
          {
            Creature potionTarget = CombatRoomHandler.GetPotionTarget(potion, combatState, random);
            if (potionTarget == null && potion.TargetType.IsSingleTarget())
            {
              AutoSlayLog.Warn($"Skipping potion {potion.Id.Entry}: no valid target");
            }
            else
            {
              AutoSlayLog.Info("Using potion: " + potion.Id.Entry);
              potion.EnqueueManualUse(potionTarget);
              await Task.Delay(300, ct);
            }
          }
          else
            break;
        }
        else
          break;
      }
      combatState = (ICombatState) null;
    }
  }

  private static Creature? GetPotionTarget(
    PotionModel potion,
    ICombatState? combatState,
    Rng random)
  {
    if (combatState == null)
      return (Creature) null;
    Creature potionTarget;
    switch (potion.TargetType)
    {
      case TargetType.Self:
        potionTarget = combatState.PlayerCreatures.FirstOrDefault<Creature>((Func<Creature, bool>) (c => c.IsAlive));
        break;
      case TargetType.AnyEnemy:
        potionTarget = random.NextItem<Creature>((IEnumerable<Creature>) combatState.HittableEnemies.ToList<Creature>());
        break;
      case TargetType.AnyPlayer:
        potionTarget = combatState.PlayerCreatures.FirstOrDefault<Creature>((Func<Creature, bool>) (c => c.IsAlive));
        break;
      case TargetType.AnyAlly:
        potionTarget = combatState.PlayerCreatures.FirstOrDefault<Creature>((Func<Creature, bool>) (c => c.IsAlive));
        break;
      default:
        potionTarget = (Creature) null;
        break;
    }
    return potionTarget;
  }
}
