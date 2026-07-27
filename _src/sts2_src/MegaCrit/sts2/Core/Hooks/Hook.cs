// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Hooks.Hook
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.Entities.CardRewardAlternatives;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Merchant;
using MegaCrit.Sts2.Core.Entities.Multiplayer;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.RestSite;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Map;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Rewards;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.ValueProps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Hooks;

public static class Hook
{
  private static IEnumerable<AbstractModel> IterateCombatHookListeners(ICombatState combatState)
  {
    if (!CombatManager.Instance.IsOverOrEnding || CombatManager.Instance.IsStarting)
    {
      foreach (AbstractModel iterateHookListener in combatState.IterateHookListeners())
        yield return iterateHookListener;
    }
  }

  public static async Task AfterActEntered(IRunState runState)
  {
    foreach (AbstractModel model in runState.IterateHookListeners((ICombatState) null))
    {
      await model.AfterActEntered();
      model.InvokeExecutionFinished();
    }
  }

  public static async Task BeforeAttack(ICombatState combatState, AttackCommand command)
  {
    foreach (AbstractModel model in Hook.IterateCombatHookListeners(combatState))
    {
      await model.BeforeAttack(command);
      model.InvokeExecutionFinished();
    }
  }

  public static async Task AfterAttack(
    ICombatState combatState,
    PlayerChoiceContext choiceContext,
    AttackCommand command)
  {
    foreach (AbstractModel model in Hook.IterateCombatHookListeners(combatState))
    {
      await model.AfterAttack(choiceContext, command);
      model.InvokeExecutionFinished();
    }
  }

  public static async Task AfterBlockBroken(
    ICombatState combatState,
    PlayerChoiceContext choiceContext,
    Creature target,
    Creature? breaker)
  {
    foreach (AbstractModel model in combatState.IterateHookListeners())
    {
      await model.AfterBlockBroken(choiceContext, target, breaker);
      model.InvokeExecutionFinished();
    }
  }

  public static async Task AfterBlockCleared(ICombatState combatState, Creature creature)
  {
    foreach (AbstractModel model in Hook.IterateCombatHookListeners(combatState))
    {
      await model.AfterBlockCleared(creature);
      model.InvokeExecutionFinished();
    }
  }

  public static async Task BeforeBlockGained(
    ICombatState combatState,
    Creature creature,
    Decimal amount,
    ValueProp props,
    CardModel? cardSource)
  {
    foreach (AbstractModel model in Hook.IterateCombatHookListeners(combatState))
    {
      await model.BeforeBlockGained(creature, amount, props, cardSource);
      model.InvokeExecutionFinished();
    }
  }

  public static async Task AfterBlockGained(
    ICombatState combatState,
    Creature creature,
    Decimal amount,
    ValueProp props,
    CardModel? cardSource)
  {
    foreach (AbstractModel model in Hook.IterateCombatHookListeners(combatState))
    {
      await model.AfterBlockGained(creature, amount, props, cardSource);
      model.InvokeExecutionFinished();
    }
  }

  public static async Task BeforeCardAutoPlayed(
    ICombatState combatState,
    CardModel card,
    Creature? target,
    AutoPlayType type)
  {
    foreach (AbstractModel model in Hook.IterateCombatHookListeners(combatState))
    {
      await model.BeforeCardAutoPlayed(card, target, type);
      model.InvokeExecutionFinished();
    }
  }

  public static async Task AfterCardChangedPiles(
    IRunState runState,
    ICombatState? combatState,
    CardModel card,
    PileType oldPile,
    AbstractModel? clonedBy)
  {
    AbstractModel model;
    foreach (AbstractModel iterateHookListener in runState.IterateHookListeners(combatState))
    {
      model = iterateHookListener;
      await model.AfterCardChangedPiles(card, oldPile, clonedBy);
      model.InvokeExecutionFinished();
      model = (AbstractModel) null;
    }
    foreach (AbstractModel iterateHookListener in runState.IterateHookListeners(combatState))
    {
      model = iterateHookListener;
      await model.AfterCardChangedPilesLate(card, oldPile, clonedBy);
      model.InvokeExecutionFinished();
      model = (AbstractModel) null;
    }
  }

  public static async Task AfterCardDiscarded(
    ICombatState combatState,
    PlayerChoiceContext choiceContext,
    CardModel card)
  {
    foreach (AbstractModel model in Hook.IterateCombatHookListeners(combatState))
    {
      choiceContext.PushModel(model);
      await model.AfterCardDiscarded(choiceContext, card);
      model.InvokeExecutionFinished();
      choiceContext.PopModel(model);
    }
  }

  public static async Task AfterCardDrawn(
    ICombatState combatState,
    PlayerChoiceContext choiceContext,
    CardModel card,
    bool fromHandDraw)
  {
    AbstractModel model;
    foreach (AbstractModel combatHookListener in Hook.IterateCombatHookListeners(combatState))
    {
      model = combatHookListener;
      choiceContext.PushModel(model);
      await model.AfterCardDrawnEarly(choiceContext, card, fromHandDraw);
      model.InvokeExecutionFinished();
      choiceContext.PopModel(model);
      model = (AbstractModel) null;
    }
    foreach (AbstractModel combatHookListener in Hook.IterateCombatHookListeners(combatState))
    {
      model = combatHookListener;
      choiceContext.PushModel(model);
      await model.AfterCardDrawn(choiceContext, card, fromHandDraw);
      model.InvokeExecutionFinished();
      choiceContext.PopModel(model);
      model = (AbstractModel) null;
    }
  }

  public static async Task AfterCardEnteredCombat(ICombatState combatState, CardModel card)
  {
    foreach (AbstractModel model in Hook.IterateCombatHookListeners(combatState))
    {
      await model.AfterCardEnteredCombat(card);
      model.InvokeExecutionFinished();
    }
  }

  public static async Task AfterCardExhausted(
    ICombatState combatState,
    PlayerChoiceContext choiceContext,
    CardModel card,
    bool causedByEthereal)
  {
    foreach (AbstractModel model in Hook.IterateCombatHookListeners(combatState))
    {
      choiceContext.PushModel(model);
      await model.AfterCardExhausted(choiceContext, card, causedByEthereal);
      model.InvokeExecutionFinished();
      choiceContext.PopModel(model);
    }
  }

  public static async Task AfterCardGeneratedForCombat(
    ICombatState combatState,
    CardModel card,
    Player? creator)
  {
    foreach (AbstractModel model in Hook.IterateCombatHookListeners(combatState))
    {
      await model.AfterCardGeneratedForCombat(card, creator);
      model.InvokeExecutionFinished();
    }
  }

  public static async Task BeforeCardPlayed(ICombatState combatState, CardPlay cardPlay)
  {
    foreach (AbstractModel model in Hook.IterateCombatHookListeners(combatState))
    {
      await model.BeforeCardPlayed(cardPlay);
      model.InvokeExecutionFinished();
    }
  }

  public static async Task AfterCardPlayed(
    ICombatState combatState,
    PlayerChoiceContext choiceContext,
    CardPlay cardPlay)
  {
    AbstractModel model;
    foreach (AbstractModel iterateHookListener in combatState.IterateHookListeners())
    {
      model = iterateHookListener;
      choiceContext.PushModel(model);
      await model.AfterCardPlayed(choiceContext, cardPlay);
      model.InvokeExecutionFinished();
      choiceContext.PopModel(model);
      model = (AbstractModel) null;
    }
    foreach (AbstractModel iterateHookListener in combatState.IterateHookListeners())
    {
      model = iterateHookListener;
      choiceContext.PushModel(model);
      await model.AfterCardPlayedLate(choiceContext, cardPlay);
      model.InvokeExecutionFinished();
      choiceContext.PopModel(model);
      model = (AbstractModel) null;
    }
  }

  public static async Task BeforeCardRemoved(IRunState runState, CardModel card)
  {
    foreach (AbstractModel model in runState.IterateHookListeners((ICombatState) null))
    {
      await model.BeforeCardRemoved(card);
      model.InvokeExecutionFinished();
    }
  }

  public static async Task BeforeCombatStart(IRunState runState, ICombatState? combatState)
  {
    AbstractModel model;
    foreach (AbstractModel iterateHookListener in runState.IterateHookListeners(combatState))
    {
      model = iterateHookListener;
      await model.BeforeCombatStart();
      model.InvokeExecutionFinished();
      model = (AbstractModel) null;
    }
    foreach (AbstractModel iterateHookListener in runState.IterateHookListeners(combatState))
    {
      model = iterateHookListener;
      await model.BeforeCombatStartLate();
      model.InvokeExecutionFinished();
      model = (AbstractModel) null;
    }
  }

  public static async Task AfterCombatEnd(
    IRunState runState,
    ICombatState? combatState,
    CombatRoom room)
  {
    foreach (AbstractModel model in runState.IterateHookListeners(combatState))
    {
      await model.AfterCombatEnd(room);
      model.InvokeExecutionFinished();
    }
  }

  public static async Task BeforeCombatRewardOffered(
    RewardsSet rewards,
    IRunState runState,
    CombatRoom room)
  {
    foreach (AbstractModel model in runState.IterateHookListeners((ICombatState) null))
    {
      await model.BeforeCombatRewardOffered(rewards, room);
      model.InvokeExecutionFinished();
    }
  }

  public static async Task AfterCombatVictory(
    IRunState runState,
    ICombatState? combatState,
    CombatRoom room)
  {
    AbstractModel model;
    foreach (AbstractModel iterateHookListener in runState.IterateHookListeners(combatState))
    {
      model = iterateHookListener;
      await model.AfterCombatVictoryEarly(room);
      model.InvokeExecutionFinished();
      model = (AbstractModel) null;
    }
    foreach (AbstractModel iterateHookListener in runState.IterateHookListeners(combatState))
    {
      model = iterateHookListener;
      await model.AfterCombatVictory(room);
      model.InvokeExecutionFinished();
      model = (AbstractModel) null;
    }
  }

  public static async Task AfterCreatureAddedToCombat(ICombatState combatState, Creature creature)
  {
    foreach (AbstractModel model in combatState.IterateHookListeners())
    {
      await model.AfterCreatureAddedToCombat(creature);
      model.InvokeExecutionFinished();
    }
  }

  public static async Task AfterCurrentHpChanged(
    IRunState runState,
    ICombatState? combatState,
    Creature creature,
    Decimal delta)
  {
    foreach (AbstractModel model in runState.IterateHookListeners(combatState))
    {
      await model.AfterCurrentHpChanged(creature, delta);
      model.InvokeExecutionFinished();
    }
  }

  public static async Task AfterDamageGiven(
    PlayerChoiceContext choiceContext,
    ICombatState combatState,
    Creature? dealer,
    DamageResult results,
    ValueProp props,
    Creature target,
    CardModel? cardSource)
  {
    foreach (AbstractModel model in combatState.IterateHookListeners())
    {
      choiceContext.PushModel(model);
      await model.AfterDamageGiven(choiceContext, dealer, results, props, target, cardSource);
      choiceContext.PopModel(model);
      model.InvokeExecutionFinished();
    }
  }

  public static async Task BeforeDamageReceived(
    PlayerChoiceContext choiceContext,
    IRunState runState,
    ICombatState? combatState,
    Creature target,
    Decimal amount,
    ValueProp props,
    Creature? dealer,
    CardModel? cardSource)
  {
    foreach (AbstractModel model in runState.IterateHookListeners(combatState))
    {
      choiceContext.PushModel(model);
      await model.BeforeDamageReceived(choiceContext, target, amount, props, dealer, cardSource);
      choiceContext.PopModel(model);
      model.InvokeExecutionFinished();
    }
  }

  public static async Task AfterDamageReceived(
    PlayerChoiceContext choiceContext,
    IRunState runState,
    ICombatState? combatState,
    Creature target,
    DamageResult result,
    ValueProp props,
    Creature? dealer,
    CardModel? cardSource)
  {
    AbstractModel model;
    foreach (AbstractModel iterateHookListener in runState.IterateHookListeners(combatState))
    {
      model = iterateHookListener;
      choiceContext.PushModel(model);
      await model.AfterDamageReceived(choiceContext, target, result, props, dealer, cardSource);
      choiceContext.PopModel(model);
      model.InvokeExecutionFinished();
      model = (AbstractModel) null;
    }
    foreach (AbstractModel iterateHookListener in runState.IterateHookListeners(combatState))
    {
      model = iterateHookListener;
      choiceContext.PushModel(model);
      await model.AfterDamageReceivedLate(choiceContext, target, result, props, dealer, cardSource);
      choiceContext.PopModel(model);
      model.InvokeExecutionFinished();
      model = (AbstractModel) null;
    }
  }

  public static async Task BeforeDeath(
    IRunState runState,
    ICombatState? combatState,
    Creature creature)
  {
    foreach (AbstractModel model in runState.IterateHookListeners(combatState))
    {
      await model.BeforeDeath(creature);
      model.InvokeExecutionFinished();
    }
  }

  public static async Task AfterDeath(
    IRunState runState,
    ICombatState? combatState,
    Creature creature,
    bool wasRemovalPrevented,
    float deathAnimLength)
  {
    ulong? netId = LocalContext.NetId;
    if (!netId.HasValue)
      return;
    foreach (AbstractModel model in runState.IterateHookListeners(combatState))
    {
      HookPlayerChoiceContext choiceContext = new HookPlayerChoiceContext(model, netId.Value, combatState, combatState == null ? GameActionType.NonCombat : GameActionType.Combat);
      Task task = model.AfterDeath((PlayerChoiceContext) choiceContext, creature, wasRemovalPrevented, deathAnimLength);
      int num = await choiceContext.AssignTaskAndWaitForPauseOrCompletion(task) ? 1 : 0;
      model.InvokeExecutionFinished();
    }
  }

  public static async Task AfterGoldGained(IRunState runState, Player player)
  {
    foreach (AbstractModel model in runState.IterateHookListeners((ICombatState) null))
    {
      await model.AfterGoldGained(player);
      model.InvokeExecutionFinished();
    }
  }

  public static async Task AfterDiedToDoom(
    ICombatState combatState,
    IReadOnlyList<Creature> creatures)
  {
    ulong? netId = LocalContext.NetId;
    if (!netId.HasValue)
      return;
    foreach (AbstractModel model in combatState.IterateHookListeners())
    {
      HookPlayerChoiceContext choiceContext = new HookPlayerChoiceContext(model, netId.Value, combatState, GameActionType.Combat);
      Task doom = model.AfterDiedToDoom((PlayerChoiceContext) choiceContext, creatures);
      int num = await choiceContext.AssignTaskAndWaitForPauseOrCompletion(doom) ? 1 : 0;
      model.InvokeExecutionFinished();
    }
  }

  public static async Task AfterEnergyReset(ICombatState combatState, Player player)
  {
    AbstractModel model;
    foreach (AbstractModel combatHookListener in Hook.IterateCombatHookListeners(combatState))
    {
      model = combatHookListener;
      await model.AfterEnergyReset(player);
      model.InvokeExecutionFinished();
      model = (AbstractModel) null;
    }
    foreach (AbstractModel combatHookListener in Hook.IterateCombatHookListeners(combatState))
    {
      model = combatHookListener;
      await model.AfterEnergyResetLate(player);
      model.InvokeExecutionFinished();
      model = (AbstractModel) null;
    }
  }

  public static async Task AfterEnergySpent(ICombatState combatState, CardModel card, int amount)
  {
    foreach (AbstractModel model in Hook.IterateCombatHookListeners(combatState))
    {
      await model.AfterEnergySpent(card, amount);
      model.InvokeExecutionFinished();
    }
  }

  public static async Task BeforeFlush(ICombatState combatState, Player player)
  {
    ulong? netId = LocalContext.NetId;
    List<Task> tasksToAwait;
    if (!netId.HasValue)
    {
      tasksToAwait = (List<Task>) null;
    }
    else
    {
      tasksToAwait = new List<Task>();
      HookPlayerChoiceContext playerChoiceContext;
      foreach (AbstractModel combatHookListener in Hook.IterateCombatHookListeners(combatState))
      {
        playerChoiceContext = new HookPlayerChoiceContext(combatHookListener, netId.Value, combatState, GameActionType.Combat);
        int num = await playerChoiceContext.AssignTaskAndWaitForPauseOrCompletion(combatHookListener.BeforeFlush((PlayerChoiceContext) playerChoiceContext, player)) ? 1 : 0;
        tasksToAwait.Add(playerChoiceContext.WaitForCompletion());
        playerChoiceContext = (HookPlayerChoiceContext) null;
      }
      foreach (AbstractModel combatHookListener in Hook.IterateCombatHookListeners(combatState))
      {
        playerChoiceContext = new HookPlayerChoiceContext(combatHookListener, netId.Value, combatState, GameActionType.Combat);
        int num = await playerChoiceContext.AssignTaskAndWaitForPauseOrCompletion(combatHookListener.BeforeFlushLate((PlayerChoiceContext) playerChoiceContext, player)) ? 1 : 0;
        tasksToAwait.Add(playerChoiceContext.WaitForCompletion());
        playerChoiceContext = (HookPlayerChoiceContext) null;
      }
      await Task.WhenAll((IEnumerable<Task>) tasksToAwait);
      tasksToAwait = (List<Task>) null;
    }
  }

  public static async Task AfterFlush(
    ICombatState combatState,
    Player player,
    PlayerChoiceContext playerChoiceContext,
    IReadOnlyCollection<CardModel> flushedCards,
    IReadOnlyCollection<CardModel> retainedCards)
  {
    foreach (AbstractModel model in Hook.IterateCombatHookListeners(combatState))
    {
      playerChoiceContext.PushModel(model);
      await model.AfterFlush(playerChoiceContext, player, flushedCards, retainedCards);
      model.InvokeExecutionFinished();
      playerChoiceContext.PopModel(model);
    }
  }

  public static async Task AfterForge(
    ICombatState combatState,
    Decimal amount,
    Player forger,
    AbstractModel? source)
  {
    foreach (AbstractModel model in Hook.IterateCombatHookListeners(combatState))
    {
      await model.AfterForge(amount, forger, source);
      model.InvokeExecutionFinished();
    }
  }

  public static async Task BeforeHandDraw(
    ICombatState combatState,
    Player player,
    PlayerChoiceContext playerChoiceContext)
  {
    AbstractModel model;
    foreach (AbstractModel combatHookListener in Hook.IterateCombatHookListeners(combatState))
    {
      model = combatHookListener;
      playerChoiceContext.PushModel(model);
      await model.BeforeHandDraw(player, playerChoiceContext, combatState);
      model.InvokeExecutionFinished();
      playerChoiceContext.PopModel(model);
      model = (AbstractModel) null;
    }
    foreach (AbstractModel combatHookListener in Hook.IterateCombatHookListeners(combatState))
    {
      model = combatHookListener;
      playerChoiceContext.PushModel(model);
      await model.BeforeHandDrawLate(player, playerChoiceContext, combatState);
      model.InvokeExecutionFinished();
      playerChoiceContext.PopModel(model);
      model = (AbstractModel) null;
    }
  }

  public static async Task AfterHandEmptied(
    ICombatState combatState,
    PlayerChoiceContext choiceContext,
    Player player)
  {
    foreach (AbstractModel model in Hook.IterateCombatHookListeners(combatState))
    {
      choiceContext.PushModel(model);
      await model.AfterHandEmptied(choiceContext, player);
      model.InvokeExecutionFinished();
      choiceContext.PopModel(model);
    }
  }

  public static async Task AfterItemPurchased(
    IRunState runState,
    Player player,
    MerchantEntry itemPurchased,
    int goldSpent)
  {
    foreach (AbstractModel model in runState.IterateHookListeners((ICombatState) null))
    {
      await model.AfterItemPurchased(player, itemPurchased, goldSpent);
      model.InvokeExecutionFinished();
    }
  }

  public static async Task AfterMapGenerated(IRunState runState, ActMap map, int actIndex)
  {
    foreach (AbstractModel model in runState.IterateHookListeners((ICombatState) null))
    {
      await model.AfterMapGenerated(map, actIndex);
      model.InvokeExecutionFinished();
    }
  }

  public static async Task AfterModifyingBlockAmount(
    ICombatState combatState,
    Decimal modifiedBlock,
    CardModel? cardSource,
    CardPlay? cardPlay,
    IEnumerable<AbstractModel> modifiers)
  {
    foreach (AbstractModel combatHookListener in Hook.IterateCombatHookListeners(combatState))
    {
      AbstractModel modifier = combatHookListener;
      if (modifiers.Contains<AbstractModel>(modifier))
      {
        await modifier.AfterModifyingBlockAmount(modifiedBlock, cardSource, cardPlay);
        modifier.InvokeExecutionFinished();
        modifier = (AbstractModel) null;
      }
    }
  }

  public static async Task AfterModifyingCardPlayCount(
    ICombatState combatState,
    CardModel card,
    IEnumerable<AbstractModel> modifiers)
  {
    foreach (AbstractModel combatHookListener in Hook.IterateCombatHookListeners(combatState))
    {
      AbstractModel modifier = combatHookListener;
      if (modifiers.Contains<AbstractModel>(modifier))
      {
        await modifier.AfterModifyingCardPlayCount(card);
        modifier.InvokeExecutionFinished();
        modifier = (AbstractModel) null;
      }
    }
  }

  public static async Task AfterModifyingCardRewardOptions(
    IRunState runState,
    IEnumerable<AbstractModel> modifiers)
  {
    foreach (AbstractModel iterateHookListener in runState.IterateHookListeners((ICombatState) null))
    {
      AbstractModel modifier = iterateHookListener;
      if (modifiers.Contains<AbstractModel>(modifier))
      {
        await modifier.AfterModifyingCardRewardOptions();
        modifier.InvokeExecutionFinished();
        modifier = (AbstractModel) null;
      }
    }
  }

  public static async Task AfterModifyingDamageAmount(
    IRunState runState,
    ICombatState? combatState,
    CardModel? cardSource,
    IEnumerable<AbstractModel> modifiers)
  {
    foreach (AbstractModel iterateHookListener in runState.IterateHookListeners(combatState))
    {
      AbstractModel modifier = iterateHookListener;
      if (modifiers.Contains<AbstractModel>(modifier))
      {
        await modifier.AfterModifyingDamageAmount(cardSource);
        modifier.InvokeExecutionFinished();
        modifier = (AbstractModel) null;
      }
    }
  }

  public static async Task AfterModifyingEnergyGain(
    ICombatState combatState,
    IEnumerable<AbstractModel> modifiers)
  {
    foreach (AbstractModel combatHookListener in Hook.IterateCombatHookListeners(combatState))
    {
      AbstractModel modifier = combatHookListener;
      if (modifiers.Contains<AbstractModel>(modifier))
      {
        await modifier.AfterModifyingEnergyGain();
        modifier.InvokeExecutionFinished();
        modifier = (AbstractModel) null;
      }
    }
  }

  public static async Task AfterModifyingGoldGained(
    IRunState runState,
    ICombatState? combatState,
    IEnumerable<AbstractModel> modifiers,
    Player player,
    Decimal amount)
  {
    foreach (AbstractModel iterateHookListener in runState.IterateHookListeners(combatState))
    {
      AbstractModel modifier = iterateHookListener;
      if (modifiers.Contains<AbstractModel>(modifier))
      {
        await modifier.AfterModifyingGoldGained(player, amount);
        modifier.InvokeExecutionFinished();
        modifier = (AbstractModel) null;
      }
    }
  }

  public static async Task AfterModifyingHandDraw(
    ICombatState combatState,
    IEnumerable<AbstractModel> modifiers)
  {
    foreach (AbstractModel combatHookListener in Hook.IterateCombatHookListeners(combatState))
    {
      AbstractModel modifier = combatHookListener;
      if (modifiers.Contains<AbstractModel>(modifier))
      {
        await modifier.AfterModifyingHandDraw();
        modifier.InvokeExecutionFinished();
        modifier = (AbstractModel) null;
      }
    }
  }

  public static async Task AfterModifyingHpLostBeforeOsty(
    IRunState runState,
    ICombatState? combatState,
    IEnumerable<AbstractModel> modifiers)
  {
    foreach (AbstractModel iterateHookListener in runState.IterateHookListeners(combatState))
    {
      AbstractModel modifier = iterateHookListener;
      if (modifiers.Contains<AbstractModel>(modifier))
      {
        await modifier.AfterModifyingHpLostBeforeOsty();
        modifier.InvokeExecutionFinished();
        modifier = (AbstractModel) null;
      }
    }
  }

  public static async Task AfterModifyingHpLostAfterOsty(
    IRunState runState,
    ICombatState? combatState,
    IEnumerable<AbstractModel> modifiers)
  {
    foreach (AbstractModel iterateHookListener in runState.IterateHookListeners(combatState))
    {
      AbstractModel modifier = iterateHookListener;
      if (modifiers.Contains<AbstractModel>(modifier))
      {
        await modifier.AfterModifyingHpLostAfterOsty();
        modifier.InvokeExecutionFinished();
        modifier = (AbstractModel) null;
      }
    }
  }

  public static async Task AfterModifyingOrbPassiveTriggerCount(
    ICombatState combatState,
    OrbModel orb,
    IEnumerable<AbstractModel> modifiers)
  {
    foreach (AbstractModel combatHookListener in Hook.IterateCombatHookListeners(combatState))
    {
      AbstractModel modifier = combatHookListener;
      if (modifiers.Contains<AbstractModel>(modifier))
      {
        await modifier.AfterModifyingOrbPassiveTriggerCount(orb);
        modifier.InvokeExecutionFinished();
        modifier = (AbstractModel) null;
      }
    }
  }

  public static async Task AfterModifyingPowerAmountGiven(
    ICombatState combatState,
    IEnumerable<AbstractModel> modifiers,
    PowerModel modifiedPower)
  {
    foreach (AbstractModel combatHookListener in Hook.IterateCombatHookListeners(combatState))
    {
      AbstractModel modifier = combatHookListener;
      if (modifiers.Contains<AbstractModel>(modifier))
      {
        await modifier.AfterModifyingPowerAmountGiven(modifiedPower);
        modifier.InvokeExecutionFinished();
        modifier = (AbstractModel) null;
      }
    }
  }

  public static async Task AfterModifyingPowerAmountReceived(
    ICombatState combatState,
    IEnumerable<AbstractModel> modifiers,
    PowerModel modifiedPower)
  {
    foreach (AbstractModel combatHookListener in Hook.IterateCombatHookListeners(combatState))
    {
      AbstractModel modifier = combatHookListener;
      if (modifiers.Contains<AbstractModel>(modifier))
      {
        await modifier.AfterModifyingPowerAmountReceived(modifiedPower);
        modifier.InvokeExecutionFinished();
        modifier = (AbstractModel) null;
      }
    }
  }

  public static async Task AfterModifyingRewards(
    IRunState runState,
    IEnumerable<AbstractModel> modifiers)
  {
    foreach (AbstractModel iterateHookListener in runState.IterateHookListeners((ICombatState) null))
    {
      AbstractModel modifier = iterateHookListener;
      if (modifiers.Contains<AbstractModel>(modifier))
      {
        await modifier.AfterModifyingRewards();
        modifier.InvokeExecutionFinished();
        modifier = (AbstractModel) null;
      }
    }
  }

  public static async Task AfterOrbChanneled(
    ICombatState combatState,
    PlayerChoiceContext choiceContext,
    Player player,
    OrbModel orb)
  {
    foreach (AbstractModel model in Hook.IterateCombatHookListeners(combatState))
    {
      choiceContext.PushModel(model);
      await model.AfterOrbChanneled(choiceContext, player, orb);
      model.InvokeExecutionFinished();
      choiceContext.PopModel(model);
    }
  }

  public static async Task AfterOrbEvoked(
    PlayerChoiceContext choiceContext,
    ICombatState combatState,
    OrbModel orb,
    IEnumerable<Creature> targets)
  {
    foreach (AbstractModel model in Hook.IterateCombatHookListeners(combatState))
    {
      await model.AfterOrbEvoked(choiceContext, orb, targets);
      model.InvokeExecutionFinished();
    }
  }

  public static async Task AfterOstyRevived(ICombatState combatState, Creature osty)
  {
    foreach (AbstractModel model in Hook.IterateCombatHookListeners(combatState))
    {
      await model.AfterOstyRevived(osty);
      model.InvokeExecutionFinished();
    }
  }

  public static async Task AfterPlayerTurnStart(
    ICombatState combatState,
    PlayerChoiceContext choiceContext,
    Player player)
  {
    AbstractModel model;
    foreach (AbstractModel combatHookListener in Hook.IterateCombatHookListeners(combatState))
    {
      model = combatHookListener;
      choiceContext.PushModel(model);
      await model.AfterPlayerTurnStartEarly(choiceContext, player);
      model.InvokeExecutionFinished();
      choiceContext.PopModel(model);
      model = (AbstractModel) null;
    }
    foreach (AbstractModel combatHookListener in Hook.IterateCombatHookListeners(combatState))
    {
      model = combatHookListener;
      choiceContext.PushModel(model);
      await model.AfterPlayerTurnStart(choiceContext, player);
      model.InvokeExecutionFinished();
      choiceContext.PopModel(model);
      model = (AbstractModel) null;
    }
    foreach (AbstractModel combatHookListener in Hook.IterateCombatHookListeners(combatState))
    {
      model = combatHookListener;
      choiceContext.PushModel(model);
      await model.AfterPlayerTurnStartLate(choiceContext, player);
      model.InvokeExecutionFinished();
      choiceContext.PopModel(model);
      model = (AbstractModel) null;
    }
  }

  public static async Task AfterAutoPostPlayPhaseEntered(
    HookPlayerChoiceContext playerChoiceContext,
    ICombatState combatState,
    Player player)
  {
    if (!LocalContext.NetId.HasValue)
      return;
    foreach (AbstractModel model in Hook.IterateCombatHookListeners(combatState))
    {
      playerChoiceContext.PushModel(model);
      await model.AfterAutoPostPlayPhaseEntered((PlayerChoiceContext) playerChoiceContext, player);
      model.InvokeExecutionFinished();
      playerChoiceContext.PopModel(model);
    }
  }

  public static async Task AfterAutoPrePlayPhaseEntered(
    HookPlayerChoiceContext playerChoiceContext,
    ICombatState combatState,
    Player player)
  {
    if (!LocalContext.NetId.HasValue)
      return;
    AbstractModel model;
    foreach (AbstractModel combatHookListener in Hook.IterateCombatHookListeners(combatState))
    {
      model = combatHookListener;
      playerChoiceContext.PushModel(model);
      await model.AfterAutoPrePlayPhaseEnteredEarly((PlayerChoiceContext) playerChoiceContext, player);
      model.InvokeExecutionFinished();
      playerChoiceContext.PopModel(model);
      model = (AbstractModel) null;
    }
    foreach (AbstractModel combatHookListener in Hook.IterateCombatHookListeners(combatState))
    {
      model = combatHookListener;
      playerChoiceContext.PushModel(model);
      await model.AfterAutoPrePlayPhaseEntered((PlayerChoiceContext) playerChoiceContext, player);
      model.InvokeExecutionFinished();
      playerChoiceContext.PopModel(model);
      model = (AbstractModel) null;
    }
    foreach (AbstractModel combatHookListener in Hook.IterateCombatHookListeners(combatState))
    {
      model = combatHookListener;
      playerChoiceContext.PushModel(model);
      await model.AfterAutoPrePlayPhaseEnteredLate((PlayerChoiceContext) playerChoiceContext, player);
      model.InvokeExecutionFinished();
      playerChoiceContext.PopModel(model);
      model = (AbstractModel) null;
    }
  }

  public static async Task AfterPotionDiscarded(
    IRunState runState,
    ICombatState? combatState,
    PotionModel potion)
  {
    foreach (AbstractModel model in runState.IterateHookListeners(combatState))
    {
      await model.AfterPotionDiscarded(potion);
      model.InvokeExecutionFinished();
    }
  }

  public static async Task AfterPotionProcured(
    IRunState runState,
    ICombatState? combatState,
    PotionModel potion)
  {
    foreach (AbstractModel model in runState.IterateHookListeners(combatState))
    {
      await model.AfterPotionProcured(potion);
      model.InvokeExecutionFinished();
    }
  }

  public static async Task BeforePotionUsed(
    IRunState runState,
    ICombatState? combatState,
    PotionModel potion,
    Creature? target)
  {
    foreach (AbstractModel model in runState.IterateHookListeners(combatState))
    {
      await model.BeforePotionUsed(potion, target);
      model.InvokeExecutionFinished();
    }
  }

  public static async Task AfterPotionUsed(
    IRunState runState,
    ICombatState? combatState,
    PotionModel potion,
    Creature? target)
  {
    foreach (AbstractModel model in runState.IterateHookListeners(combatState))
    {
      await model.AfterPotionUsed(potion, target);
      model.InvokeExecutionFinished();
    }
  }

  public static async Task BeforePowerAmountChanged(
    ICombatState combatState,
    PowerModel power,
    Decimal amount,
    Creature target,
    Creature? applier,
    CardModel? cardSource)
  {
    foreach (AbstractModel modifier in Hook.IterateCombatHookListeners(combatState))
    {
      await modifier.BeforePowerAmountChanged(power, amount, target, applier, cardSource);
      modifier.InvokeExecutionFinished();
    }
  }

  public static async Task AfterPowerAmountChanged(
    ICombatState combatState,
    PlayerChoiceContext choiceContext,
    PowerModel power,
    Decimal amount,
    Creature? applier,
    CardModel? cardSource)
  {
    foreach (AbstractModel model in Hook.IterateCombatHookListeners(combatState))
    {
      await model.AfterPowerAmountChanged(choiceContext, power, amount, applier, cardSource);
      model.InvokeExecutionFinished();
    }
  }

  public static async Task AfterPreventingBlockClear(
    ICombatState combatState,
    AbstractModel preventer,
    Creature creature)
  {
    if (!Hook.IterateCombatHookListeners(combatState).Contains<AbstractModel>(preventer))
      return;
    await preventer.AfterPreventingBlockClear(preventer, creature);
    preventer.InvokeExecutionFinished();
  }

  public static async Task AfterPreventingDeath(
    IRunState runState,
    ICombatState? combatState,
    AbstractModel preventer,
    Creature creature)
  {
    if (!runState.IterateHookListeners(combatState).Contains<AbstractModel>(preventer))
      return;
    await preventer.AfterPreventingDeath(creature);
    preventer.InvokeExecutionFinished();
  }

  public static async Task AfterPreventingDraw(ICombatState combatState, AbstractModel modifier)
  {
    if (!Hook.IterateCombatHookListeners(combatState).Contains<AbstractModel>(modifier))
      return;
    await modifier.AfterPreventingDraw();
    modifier.InvokeExecutionFinished();
  }

  public static async Task AfterRestSiteHeal(IRunState runState, Player player, bool isMimicked)
  {
    foreach (AbstractModel model in runState.IterateHookListeners((ICombatState) null))
    {
      await model.AfterRestSiteHeal(player, isMimicked);
      model.InvokeExecutionFinished();
    }
  }

  public static async Task AfterRestSiteSmith(IRunState runState, Player player)
  {
    foreach (AbstractModel model in runState.IterateHookListeners((ICombatState) null))
    {
      await model.AfterRestSiteSmith(player);
      model.InvokeExecutionFinished();
    }
  }

  public static async Task AfterRewardTaken(IRunState runState, Player player, Reward reward)
  {
    foreach (AbstractModel model in runState.IterateHookListeners((ICombatState) null))
    {
      await model.AfterRewardTaken(player, reward);
      model.InvokeExecutionFinished();
    }
  }

  public static async Task BeforeRoomEntered(IRunState runState, AbstractRoom room)
  {
    foreach (AbstractModel model in runState.IterateHookListeners((ICombatState) null))
    {
      await model.BeforeRoomEntered(room);
      model.InvokeExecutionFinished();
    }
  }

  public static async Task AfterRoomEntered(IRunState runState, AbstractRoom room)
  {
    foreach (AbstractModel model in runState.IterateHookListeners((ICombatState) null))
    {
      await model.AfterRoomEntered(room);
      model.InvokeExecutionFinished();
    }
  }

  public static async Task AfterShuffle(
    ICombatState combatState,
    PlayerChoiceContext choiceContext,
    Player shuffler)
  {
    foreach (AbstractModel model in Hook.IterateCombatHookListeners(combatState))
    {
      choiceContext.PushModel(model);
      await model.AfterShuffle(choiceContext, shuffler);
      model.InvokeExecutionFinished();
      choiceContext.PopModel(model);
    }
  }

  public static async Task BeforeSideTurnStart(
    ICombatState combatState,
    CombatSide side,
    IReadOnlyList<Creature> participants)
  {
    ulong? netId = LocalContext.NetId;
    if (!netId.HasValue)
      return;
    foreach (AbstractModel model in Hook.IterateCombatHookListeners(combatState))
    {
      HookPlayerChoiceContext choiceContext = new HookPlayerChoiceContext(model, netId.Value, combatState, GameActionType.Combat);
      Task task = model.BeforeSideTurnStart((PlayerChoiceContext) choiceContext, side, participants, combatState);
      int num = await choiceContext.AssignTaskAndWaitForPauseOrCompletion(task) ? 1 : 0;
      model.InvokeExecutionFinished();
    }
  }

  public static async Task AfterSideTurnStart(
    ICombatState combatState,
    CombatSide side,
    IReadOnlyList<Creature> participants)
  {
    AbstractModel model;
    foreach (AbstractModel combatHookListener in Hook.IterateCombatHookListeners(combatState))
    {
      model = combatHookListener;
      await model.AfterSideTurnStart(side, participants, combatState);
      model.InvokeExecutionFinished();
      model = (AbstractModel) null;
    }
    foreach (AbstractModel combatHookListener in Hook.IterateCombatHookListeners(combatState))
    {
      model = combatHookListener;
      await model.AfterSideTurnStartLate(side, participants, combatState);
      model.InvokeExecutionFinished();
      model = (AbstractModel) null;
    }
  }

  public static async Task AfterStarsGained(ICombatState combatState, int amount, Player gainer)
  {
    foreach (AbstractModel model in Hook.IterateCombatHookListeners(combatState))
    {
      await model.AfterStarsGained(amount, gainer);
      model.InvokeExecutionFinished();
    }
  }

  public static async Task AfterStarsSpent(ICombatState combatState, int amount, Player spender)
  {
    foreach (AbstractModel model in Hook.IterateCombatHookListeners(combatState))
    {
      await model.AfterStarsSpent(amount, spender);
      model.InvokeExecutionFinished();
    }
  }

  public static async Task AfterSummon(
    ICombatState combatState,
    PlayerChoiceContext choiceContext,
    Player summoner,
    Decimal amount)
  {
    foreach (AbstractModel model in Hook.IterateCombatHookListeners(combatState))
    {
      choiceContext.PushModel(model);
      await model.AfterSummon(choiceContext, summoner, amount);
      model.InvokeExecutionFinished();
      choiceContext.PopModel(model);
    }
  }

  public static async Task AfterTakingExtraTurn(ICombatState combatState, Player player)
  {
    foreach (AbstractModel model in Hook.IterateCombatHookListeners(combatState))
    {
      await model.AfterTakingExtraTurn(player);
      model.InvokeExecutionFinished();
    }
  }

  public static async Task BeforeSideTurnEnd(
    ICombatState combatState,
    CombatSide side,
    IEnumerable<Creature> participants)
  {
    ulong? netId = LocalContext.NetId;
    List<Task> tasksToAwait;
    if (!netId.HasValue)
    {
      tasksToAwait = (List<Task>) null;
    }
    else
    {
      tasksToAwait = new List<Task>();
      HookPlayerChoiceContext playerChoiceContext;
      foreach (AbstractModel combatHookListener in Hook.IterateCombatHookListeners(combatState))
      {
        playerChoiceContext = new HookPlayerChoiceContext(combatHookListener, netId.Value, combatState, GameActionType.Combat);
        int num = await playerChoiceContext.AssignTaskAndWaitForPauseOrCompletion(combatHookListener.BeforeSideTurnEndVeryEarly((PlayerChoiceContext) playerChoiceContext, side, participants)) ? 1 : 0;
        tasksToAwait.Add(playerChoiceContext.WaitForCompletion());
        playerChoiceContext = (HookPlayerChoiceContext) null;
      }
      foreach (AbstractModel combatHookListener in Hook.IterateCombatHookListeners(combatState))
      {
        playerChoiceContext = new HookPlayerChoiceContext(combatHookListener, netId.Value, combatState, GameActionType.Combat);
        int num = await playerChoiceContext.AssignTaskAndWaitForPauseOrCompletion(combatHookListener.BeforeSideTurnEndEarly((PlayerChoiceContext) playerChoiceContext, side, participants)) ? 1 : 0;
        tasksToAwait.Add(playerChoiceContext.WaitForCompletion());
        playerChoiceContext = (HookPlayerChoiceContext) null;
      }
      foreach (AbstractModel combatHookListener in Hook.IterateCombatHookListeners(combatState))
      {
        playerChoiceContext = new HookPlayerChoiceContext(combatHookListener, netId.Value, combatState, GameActionType.Combat);
        int num = await playerChoiceContext.AssignTaskAndWaitForPauseOrCompletion(combatHookListener.BeforeSideTurnEnd((PlayerChoiceContext) playerChoiceContext, side, participants)) ? 1 : 0;
        tasksToAwait.Add(playerChoiceContext.WaitForCompletion());
        playerChoiceContext = (HookPlayerChoiceContext) null;
      }
      await Task.WhenAll((IEnumerable<Task>) tasksToAwait);
      tasksToAwait = (List<Task>) null;
    }
  }

  public static async Task AfterSideTurnEnd(
    ICombatState combatState,
    CombatSide side,
    IEnumerable<Creature> participants)
  {
    ulong? netId = LocalContext.NetId;
    List<Task> tasksToAwait;
    if (!netId.HasValue)
    {
      tasksToAwait = (List<Task>) null;
    }
    else
    {
      tasksToAwait = new List<Task>();
      HookPlayerChoiceContext playerChoiceContext;
      foreach (AbstractModel combatHookListener in Hook.IterateCombatHookListeners(combatState))
      {
        playerChoiceContext = new HookPlayerChoiceContext(combatHookListener, netId.Value, combatState, GameActionType.Combat);
        int num = await playerChoiceContext.AssignTaskAndWaitForPauseOrCompletion(combatHookListener.AfterSideTurnEnd((PlayerChoiceContext) playerChoiceContext, side, participants)) ? 1 : 0;
        tasksToAwait.Add(playerChoiceContext.WaitForCompletion());
        playerChoiceContext = (HookPlayerChoiceContext) null;
      }
      await Task.WhenAll((IEnumerable<Task>) tasksToAwait);
      tasksToAwait.Clear();
      foreach (AbstractModel combatHookListener in Hook.IterateCombatHookListeners(combatState))
      {
        playerChoiceContext = new HookPlayerChoiceContext(combatHookListener, netId.Value, combatState, GameActionType.Combat);
        int num = await playerChoiceContext.AssignTaskAndWaitForPauseOrCompletion(combatHookListener.AfterSideTurnEndLate((PlayerChoiceContext) playerChoiceContext, side, participants)) ? 1 : 0;
        tasksToAwait.Add(playerChoiceContext.WaitForCompletion());
        playerChoiceContext = (HookPlayerChoiceContext) null;
      }
      await Task.WhenAll((IEnumerable<Task>) tasksToAwait);
      tasksToAwait = (List<Task>) null;
    }
  }

  public static Decimal ModifyAttackHitCount(
    ICombatState combatState,
    AttackCommand attackCommand,
    int originalHitCount)
  {
    int hitCount = originalHitCount;
    foreach (AbstractModel combatHookListener in Hook.IterateCombatHookListeners(combatState))
      hitCount = combatHookListener.ModifyAttackHitCount(attackCommand, hitCount);
    return (Decimal) hitCount;
  }

  public static Decimal ModifyBlock(
    ICombatState combatState,
    Creature target,
    Decimal block,
    ValueProp props,
    CardModel? cardSource,
    CardPlay? cardPlay,
    out IEnumerable<AbstractModel> modifiers)
  {
    List<AbstractModel> abstractModelList = new List<AbstractModel>();
    Decimal num1 = block;
    if (cardSource != null && cardSource.Enchantment != null)
    {
      EnchantmentModel enchantment = cardSource.Enchantment;
      Decimal originalBlock = num1 + enchantment.EnchantBlockAdditive(num1);
      num1 = originalBlock * enchantment.EnchantBlockMultiplicative(originalBlock);
    }
    foreach (AbstractModel combatHookListener in Hook.IterateCombatHookListeners(combatState))
    {
      Decimal num2 = combatHookListener.ModifyBlockAdditive(target, num1, props, cardSource, cardPlay);
      num1 += num2;
      if (num2 != 0M)
        abstractModelList.Add(combatHookListener);
    }
    foreach (AbstractModel combatHookListener in Hook.IterateCombatHookListeners(combatState))
    {
      Decimal num3 = combatHookListener.ModifyBlockMultiplicative(target, num1, props, cardSource, cardPlay);
      num1 *= num3;
      if (num3 != 1M)
        abstractModelList.Add(combatHookListener);
    }
    modifiers = (IEnumerable<AbstractModel>) abstractModelList;
    return Math.Max(0M, num1);
  }

  public static CardModel ModifyCardBeingAddedToDeck(
    IRunState runState,
    CardModel card,
    out List<AbstractModel> modifyingModels)
  {
    modifyingModels = new List<AbstractModel>();
    foreach (AbstractModel iterateHookListener in runState.IterateHookListeners((ICombatState) null))
    {
      CardModel newCard;
      if (iterateHookListener.TryModifyCardBeingAddedToDeck(card, out newCard) && newCard != null)
      {
        modifyingModels.Add(iterateHookListener);
        card = newCard;
      }
      iterateHookListener.InvokeExecutionFinished();
    }
    foreach (AbstractModel iterateHookListener in runState.IterateHookListeners((ICombatState) null))
    {
      CardModel newCard;
      if (iterateHookListener.TryModifyCardBeingAddedToDeckLate(card, out newCard) && newCard != null)
      {
        modifyingModels.Add(iterateHookListener);
        card = newCard;
      }
      iterateHookListener.InvokeExecutionFinished();
    }
    return card;
  }

  public static int ModifyCardPlayCount(
    ICombatState combatState,
    CardModel card,
    int playCount,
    Creature? target,
    out List<AbstractModel> modifyingModels)
  {
    modifyingModels = new List<AbstractModel>();
    int playCount1 = playCount;
    foreach (AbstractModel combatHookListener in Hook.IterateCombatHookListeners(combatState))
    {
      int num = playCount1;
      playCount1 = combatHookListener.ModifyCardPlayCount(card, target, playCount1);
      if (playCount1 != num)
        modifyingModels.Add(combatHookListener);
    }
    return playCount1;
  }

  public static CardLocation ModifyCardPlayResultLocation(
    ICombatState combatState,
    CardModel card,
    bool isAutoPlay,
    ResourceInfo resources,
    CardLocation cardLocation,
    out IEnumerable<AbstractModel> modifiers)
  {
    List<AbstractModel> abstractModelList = new List<AbstractModel>();
    foreach (AbstractModel combatHookListener in Hook.IterateCombatHookListeners(combatState))
    {
      CardLocation cardLocation1 = cardLocation;
      cardLocation = combatHookListener.ModifyCardPlayResultLocation(card, isAutoPlay, resources, cardLocation);
      if (cardLocation1 != cardLocation)
        abstractModelList.Add(combatHookListener);
    }
    modifiers = (IEnumerable<AbstractModel>) abstractModelList;
    return cardLocation;
  }

  public static IEnumerable<AbstractModel> ModifyCardRewardAlternatives(
    IRunState runState,
    Player player,
    CardReward cardReward,
    List<CardRewardAlternative> alternatives)
  {
    List<AbstractModel> abstractModelList = new List<AbstractModel>();
    foreach (AbstractModel iterateHookListener in runState.IterateHookListeners((ICombatState) null))
    {
      if (iterateHookListener.TryModifyCardRewardAlternatives(player, cardReward, alternatives))
        abstractModelList.Add(iterateHookListener);
    }
    return (IEnumerable<AbstractModel>) abstractModelList;
  }

  public static CardCreationOptions ModifyCardRewardCreationOptions(
    IRunState runState,
    Player player,
    CardCreationOptions options)
  {
    foreach (AbstractModel iterateHookListener in runState.IterateHookListeners((ICombatState) null))
      options = iterateHookListener.ModifyCardRewardCreationOptions(player, options);
    foreach (AbstractModel iterateHookListener in runState.IterateHookListeners((ICombatState) null))
      options = iterateHookListener.ModifyCardRewardCreationOptionsLate(player, options);
    return options;
  }

  public static bool TryModifyCardRewardOptions(
    IRunState runState,
    Player player,
    List<CardCreationResult> cardRewardOptions,
    CardCreationOptions creationOptions,
    out List<AbstractModel> modifiers)
  {
    bool flag1 = false;
    modifiers = new List<AbstractModel>();
    foreach (AbstractModel iterateHookListener in runState.IterateHookListeners((ICombatState) null))
    {
      bool flag2 = iterateHookListener.TryModifyCardRewardOptions(player, cardRewardOptions, creationOptions);
      flag1 |= flag2;
      if (flag2)
        modifiers.Add(iterateHookListener);
    }
    foreach (AbstractModel iterateHookListener in runState.IterateHookListeners((ICombatState) null))
    {
      bool flag3 = iterateHookListener.TryModifyCardRewardOptionsLate(player, cardRewardOptions, creationOptions);
      flag1 |= flag3;
      if (flag3)
        modifiers.Add(iterateHookListener);
    }
    return flag1;
  }

  public static Decimal ModifyCardRewardUpgradeOdds(
    IRunState runState,
    Player player,
    CardModel card,
    Decimal originalOdds)
  {
    Decimal odds = originalOdds;
    foreach (AbstractModel iterateHookListener in runState.IterateHookListeners((ICombatState) null))
      odds = iterateHookListener.ModifyCardRewardUpgradeOdds(player, card, odds);
    return odds;
  }

  public static Decimal ModifyDamage(
    IRunState runState,
    ICombatState? combatState,
    Creature? target,
    Creature? dealer,
    Decimal damage,
    ValueProp props,
    CardModel? cardSource,
    CardPlay? cardPlay,
    ModifyDamageHookType modifyDamageHookType,
    CardPreviewMode previewMode,
    out IEnumerable<AbstractModel> modifiers)
  {
    List<AbstractModel> modifiers1 = new List<AbstractModel>();
    Decimal num1 = damage;
    if (cardSource != null && cardSource.Enchantment != null)
    {
      if (modifyDamageHookType.HasFlag((Enum) ModifyDamageHookType.Additive))
        num1 += cardSource.Enchantment.EnchantDamageAdditive(num1, props);
      if (modifyDamageHookType.HasFlag((Enum) ModifyDamageHookType.Multiplicative))
        num1 *= cardSource.Enchantment.EnchantDamageMultiplicative(num1, props);
    }
    bool flag1 = target == null && previewMode == CardPreviewMode.MultiCreatureTargeting;
    if (flag1)
    {
      bool flag2;
      if (cardSource != null)
      {
        switch (cardSource.TargetType)
        {
          case TargetType.AllEnemies:
          case TargetType.RandomEnemy:
            CardPile pile = cardSource.Pile;
            if (pile != null)
            {
              switch (pile.Type)
              {
                case PileType.Hand:
                case PileType.Play:
                  flag2 = true;
                  goto label_12;
              }
            }
            else
              break;
            break;
        }
      }
      flag2 = false;
label_12:
      flag1 = flag2;
    }
    bool flag3 = flag1;
    bool flag4 = false;
    if (flag3)
    {
      bool flag5 = true;
      Decimal? nullable = new Decimal?();
      foreach (Creature target1 in (IEnumerable<Creature>) ((combatState != null ? (object) combatState.HittableEnemies : (object) null) ?? (object) Array.Empty<Creature>()))
      {
        List<AbstractModel> modifiers2;
        Decimal num2 = Hook.ModifyDamageInternal(runState, combatState, target1, dealer, num1, props, cardSource, cardPlay, modifyDamageHookType, out modifiers2);
        if (!nullable.HasValue)
          nullable = new Decimal?(num2);
        else if ((int) num2 != (int) nullable.Value)
        {
          flag5 = false;
          break;
        }
        modifiers1.AddRange((IEnumerable<AbstractModel>) modifiers2);
      }
      if (nullable.HasValue & flag5)
      {
        flag4 = true;
        num1 = nullable.Value;
        modifiers1 = modifiers1.Distinct<AbstractModel>().ToList<AbstractModel>();
      }
      else
        modifiers1.Clear();
    }
    if (!flag3 || !flag4)
      num1 = Hook.ModifyDamageInternal(runState, combatState, target, dealer, num1, props, cardSource, cardPlay, modifyDamageHookType, out modifiers1);
    modifiers = (IEnumerable<AbstractModel>) modifiers1;
    return Math.Max(0M, num1);
  }

  public static Decimal ModifyEnergyCostInCombat(
    ICombatState combatState,
    CardModel card,
    Decimal originalCost)
  {
    if (originalCost < 0M)
      return originalCost;
    Decimal modifiedCost = originalCost;
    foreach (AbstractModel combatHookListener in Hook.IterateCombatHookListeners(combatState))
      combatHookListener.TryModifyEnergyCostInCombat(card, modifiedCost, out modifiedCost);
    foreach (AbstractModel combatHookListener in Hook.IterateCombatHookListeners(combatState))
      combatHookListener.TryModifyEnergyCostInCombatLate(card, modifiedCost, out modifiedCost);
    return modifiedCost;
  }

  public static void ModifyKeywordsInCombat(
    ICombatState combatState,
    CardModel card,
    ISet<CardKeyword> keywords)
  {
    foreach (AbstractModel iterateHookListener in combatState.IterateHookListeners())
      iterateHookListener.TryModifyKeywordsInCombat(card, keywords);
  }

  public static Decimal ModifyEnergyGain(
    ICombatState combatState,
    Player player,
    Decimal originalAmount,
    out IEnumerable<AbstractModel> modifiers)
  {
    Decimal amount = originalAmount;
    List<AbstractModel> abstractModelList = new List<AbstractModel>();
    foreach (AbstractModel combatHookListener in Hook.IterateCombatHookListeners(combatState))
    {
      Decimal num = amount;
      amount = combatHookListener.ModifyEnergyGain(player, amount);
      if ((int) num != (int) amount)
        abstractModelList.Add(combatHookListener);
    }
    modifiers = (IEnumerable<AbstractModel>) abstractModelList;
    return amount;
  }

  public static Decimal ModifyGoldGained(
    IRunState runState,
    ICombatState? combatState,
    Decimal amount,
    Player player,
    out IEnumerable<AbstractModel> modifiers)
  {
    Decimal amount1 = amount;
    List<AbstractModel> abstractModelList = new List<AbstractModel>();
    foreach (AbstractModel iterateHookListener in runState.IterateHookListeners(combatState))
    {
      Decimal num = amount1;
      amount1 = iterateHookListener.ModifyGoldGained(player, amount1);
      if ((int) num != (int) amount1)
        abstractModelList.Add(iterateHookListener);
    }
    modifiers = (IEnumerable<AbstractModel>) abstractModelList;
    return amount1;
  }

  public static IReadOnlyList<LocString> ModifyExtraRestSiteHealText(
    IRunState runState,
    Player player,
    IReadOnlyList<LocString> extraText)
  {
    foreach (AbstractModel iterateHookListener in runState.IterateHookListeners((ICombatState) null))
      extraText = iterateHookListener.ModifyExtraRestSiteHealText(player, extraText);
    return extraText;
  }

  public static ActMap ModifyGeneratedMap(IRunState runState, ActMap map, int actIndex)
  {
    foreach (AbstractModel iterateHookListener in runState.IterateHookListeners((ICombatState) null))
    {
      map = iterateHookListener.ModifyGeneratedMap(runState, map, actIndex);
      iterateHookListener.InvokeExecutionFinished();
    }
    return Hook.ModifyGeneratedMapLate(runState, map, actIndex);
  }

  public static ActMap ModifyGeneratedMapLate(IRunState runState, ActMap map, int actIndex)
  {
    foreach (AbstractModel iterateHookListener in runState.IterateHookListeners((ICombatState) null))
    {
      map = iterateHookListener.ModifyGeneratedMapLate(runState, map, actIndex);
      iterateHookListener.InvokeExecutionFinished();
    }
    return map;
  }

  public static Decimal ModifyHandDraw(
    ICombatState combatState,
    Player player,
    Decimal originalCardCount,
    out IEnumerable<AbstractModel> modifiers)
  {
    Decimal count = originalCardCount;
    List<AbstractModel> abstractModelList = new List<AbstractModel>();
    foreach (AbstractModel combatHookListener in Hook.IterateCombatHookListeners(combatState))
    {
      Decimal num = count;
      count = combatHookListener.ModifyHandDraw(player, count);
      if ((int) num != (int) count)
        abstractModelList.Add(combatHookListener);
    }
    foreach (AbstractModel combatHookListener in Hook.IterateCombatHookListeners(combatState))
    {
      Decimal num = count;
      count = combatHookListener.ModifyHandDrawLate(player, count);
      if ((int) num != (int) count)
        abstractModelList.Add(combatHookListener);
    }
    modifiers = (IEnumerable<AbstractModel>) abstractModelList;
    return count;
  }

  public static Decimal ModifyHpLost(
    IRunState runState,
    ICombatState? combatState,
    Creature target,
    Decimal amount,
    ValueProp props,
    Creature? dealer,
    CardModel? cardSource,
    HpLossHookPhase phases,
    out IEnumerable<AbstractModel> modifiers)
  {
    Decimal num = amount;
    List<AbstractModel> abstractModelList = new List<AbstractModel>();
    if (phases.HasFlag((Enum) HpLossHookPhase.BeforeOsty))
    {
      foreach (AbstractModel iterateHookListener in runState.IterateHookListeners(combatState))
      {
        Decimal d = num;
        num = iterateHookListener.ModifyHpLostBeforeOsty(target, num, props, dealer, cardSource);
        if (Decimal.Truncate(d) != Decimal.Truncate(num))
          abstractModelList.Add(iterateHookListener);
      }
      foreach (AbstractModel iterateHookListener in runState.IterateHookListeners(combatState))
      {
        Decimal d = num;
        num = iterateHookListener.ModifyHpLostBeforeOstyLate(target, num, props, dealer, cardSource);
        if (Decimal.Truncate(d) != Decimal.Truncate(num))
          abstractModelList.Add(iterateHookListener);
      }
    }
    if (phases.HasFlag((Enum) HpLossHookPhase.AfterOsty))
    {
      foreach (AbstractModel iterateHookListener in runState.IterateHookListeners(combatState))
      {
        Decimal d = num;
        num = iterateHookListener.ModifyHpLostAfterOsty(target, num, props, dealer, cardSource);
        if (Decimal.Truncate(d) != Decimal.Truncate(num))
          abstractModelList.Add(iterateHookListener);
      }
      foreach (AbstractModel iterateHookListener in runState.IterateHookListeners(combatState))
      {
        Decimal d = num;
        num = iterateHookListener.ModifyHpLostAfterOstyLate(target, num, props, dealer, cardSource);
        if (Decimal.Truncate(d) != Decimal.Truncate(num))
          abstractModelList.Add(iterateHookListener);
      }
    }
    modifiers = (IEnumerable<AbstractModel>) abstractModelList;
    return num;
  }

  public static Decimal ModifyMaxEnergy(ICombatState combatState, Player player, Decimal amount)
  {
    Decimal amount1 = amount;
    foreach (AbstractModel combatHookListener in Hook.IterateCombatHookListeners(combatState))
      amount1 = combatHookListener.ModifyMaxEnergy(player, amount1);
    return amount1;
  }

  public static void ModifyMerchantCardCreationResults(
    IRunState runState,
    Player player,
    List<CardCreationResult> cards)
  {
    foreach (AbstractModel iterateHookListener in runState.IterateHookListeners((ICombatState) null))
      iterateHookListener.ModifyMerchantCardCreationResults(player, cards);
  }

  public static IEnumerable<CardModel> ModifyMerchantCardPool(
    IRunState runState,
    Player player,
    IEnumerable<CardModel> options)
  {
    foreach (AbstractModel iterateHookListener in runState.IterateHookListeners((ICombatState) null))
      options = iterateHookListener.ModifyMerchantCardPool(player, options);
    return options;
  }

  public static CardRarity ModifyMerchantCardRarity(
    IRunState runState,
    Player player,
    CardRarity rarity)
  {
    foreach (AbstractModel iterateHookListener in runState.IterateHookListeners((ICombatState) null))
      rarity = iterateHookListener.ModifyMerchantCardRarity(player, rarity);
    return rarity;
  }

  public static Decimal ModifyMerchantPrice(
    IRunState runState,
    Player player,
    MerchantEntry entry,
    Decimal result)
  {
    foreach (AbstractModel iterateHookListener in runState.IterateHookListeners((ICombatState) null))
      result = iterateHookListener.ModifyMerchantPrice(player, entry, result);
    return result;
  }

  public static EventModel ModifyNextEvent(IRunState runState, EventModel currentEvent)
  {
    EventModel currentEvent1 = currentEvent;
    foreach (AbstractModel iterateHookListener in runState.IterateHookListeners((ICombatState) null))
      currentEvent1 = iterateHookListener.ModifyNextEvent(currentEvent1);
    return currentEvent1;
  }

  public static float ModifyOddsIncreaseForUnrolledRoomType(
    IRunState runState,
    RoomType roomType,
    float oddsIncrease)
  {
    foreach (AbstractModel iterateHookListener in runState.IterateHookListeners((ICombatState) null))
      oddsIncrease = iterateHookListener.ModifyOddsIncreaseForUnrolledRoomType(roomType, oddsIncrease);
    return oddsIncrease;
  }

  public static int ModifyOrbPassiveTriggerCount(
    ICombatState combatState,
    OrbModel orb,
    int triggerCount,
    out List<AbstractModel> modifyingModels)
  {
    modifyingModels = new List<AbstractModel>();
    int triggerCount1 = triggerCount;
    foreach (AbstractModel combatHookListener in Hook.IterateCombatHookListeners(combatState))
    {
      int num = triggerCount1;
      triggerCount1 = combatHookListener.ModifyOrbPassiveTriggerCounts(orb, triggerCount1);
      if (triggerCount1 != num)
        modifyingModels.Add(combatHookListener);
    }
    return triggerCount1;
  }

  public static Decimal ModifyOrbValue(ICombatState combatState, OrbModel orb, Decimal amount)
  {
    Decimal num = amount;
    foreach (AbstractModel combatHookListener in Hook.IterateCombatHookListeners(combatState))
      num = combatHookListener.ModifyOrbValue(orb, num);
    return num;
  }

  public static Decimal ModifyPowerAmountGiven(
    ICombatState combatState,
    PowerModel power,
    Creature giver,
    Decimal amount,
    Creature? target,
    CardModel? cardSource,
    out IEnumerable<AbstractModel> modifiers)
  {
    Decimal amount1 = amount;
    List<AbstractModel> abstractModelList = new List<AbstractModel>();
    foreach (AbstractModel combatHookListener in Hook.IterateCombatHookListeners(combatState))
    {
      Decimal num = combatHookListener.ModifyPowerAmountGivenAdditive(power, giver, amount1, target, cardSource);
      amount1 += num;
      if (num != 0M)
        abstractModelList.Add(combatHookListener);
    }
    foreach (AbstractModel combatHookListener in Hook.IterateCombatHookListeners(combatState))
    {
      Decimal num = combatHookListener.ModifyPowerAmountGivenMultiplicative(power, giver, amount1, target, cardSource);
      amount1 *= num;
      if (num != 1M)
        abstractModelList.Add(combatHookListener);
    }
    modifiers = (IEnumerable<AbstractModel>) abstractModelList;
    return amount1;
  }

  public static Decimal ModifyPowerAmountReceived(
    ICombatState combatState,
    PowerModel canonicalPower,
    Creature target,
    Decimal amount,
    Creature? giver,
    out IEnumerable<AbstractModel> modifiers)
  {
    Decimal amount1 = amount;
    List<AbstractModel> abstractModelList = new List<AbstractModel>();
    foreach (AbstractModel combatHookListener in Hook.IterateCombatHookListeners(combatState))
    {
      Decimal modifiedAmount;
      if (combatHookListener.TryModifyPowerAmountReceived(canonicalPower, target, amount1, giver, out modifiedAmount))
      {
        amount1 = modifiedAmount;
        abstractModelList.Add(combatHookListener);
      }
    }
    modifiers = (IEnumerable<AbstractModel>) abstractModelList;
    return amount1;
  }

  public static Decimal ModifyRestSiteHealAmount(
    IRunState runState,
    Creature creature,
    Decimal amount)
  {
    Decimal amount1 = amount;
    foreach (AbstractModel iterateHookListener in runState.IterateHookListeners((ICombatState) null))
      amount1 = iterateHookListener.ModifyRestSiteHealAmount(creature, amount1);
    return amount1;
  }

  public static IEnumerable<AbstractModel> ModifyRestSiteOptions(
    IRunState runState,
    Player player,
    ICollection<RestSiteOption> options)
  {
    List<AbstractModel> abstractModelList = new List<AbstractModel>();
    foreach (AbstractModel iterateHookListener in runState.IterateHookListeners((ICombatState) null))
    {
      if (iterateHookListener.TryModifyRestSiteOptions(player, options))
        abstractModelList.Add(iterateHookListener);
    }
    return (IEnumerable<AbstractModel>) abstractModelList;
  }

  public static IEnumerable<AbstractModel> ModifyRestSiteHealRewards(
    IRunState runState,
    Player player,
    List<Reward> rewards,
    bool isMimicked)
  {
    List<AbstractModel> abstractModelList = new List<AbstractModel>();
    foreach (AbstractModel iterateHookListener in runState.IterateHookListeners((ICombatState) null))
    {
      if (iterateHookListener.TryModifyRestSiteHealRewards(player, rewards, isMimicked))
        abstractModelList.Add(iterateHookListener);
    }
    return (IEnumerable<AbstractModel>) abstractModelList;
  }

  public static IEnumerable<AbstractModel> ModifyRewards(
    IRunState runState,
    Player player,
    List<Reward> rewards,
    AbstractRoom? room)
  {
    List<AbstractModel> abstractModelList = new List<AbstractModel>();
    foreach (AbstractModel iterateHookListener in runState.IterateHookListeners((ICombatState) null))
    {
      if (iterateHookListener.TryModifyRewards(player, rewards, room))
        abstractModelList.Add(iterateHookListener);
    }
    foreach (AbstractModel iterateHookListener in runState.IterateHookListeners((ICombatState) null))
    {
      if (iterateHookListener.TryModifyRewardsLate(player, rewards, room))
        abstractModelList.Add(iterateHookListener);
    }
    return (IEnumerable<AbstractModel>) abstractModelList;
  }

  public static void ModifyShuffleOrder(
    ICombatState combatState,
    Player player,
    List<CardModel> cards,
    bool isInitialShuffle)
  {
    foreach (AbstractModel combatHookListener in Hook.IterateCombatHookListeners(combatState))
      combatHookListener.ModifyShuffleOrder(player, cards, isInitialShuffle);
  }

  public static Decimal ModifyStarCost(
    ICombatState combatState,
    CardModel card,
    Decimal originalCost)
  {
    if (originalCost < 0M)
      return originalCost;
    Decimal modifiedCost = originalCost;
    foreach (AbstractModel combatHookListener in Hook.IterateCombatHookListeners(combatState))
      combatHookListener.TryModifyStarCost(card, modifiedCost, out modifiedCost);
    return modifiedCost;
  }

  public static Decimal ModifySummonAmount(
    ICombatState combatState,
    Player summoner,
    Decimal amount,
    AbstractModel? source)
  {
    Decimal amount1 = amount;
    foreach (AbstractModel combatHookListener in Hook.IterateCombatHookListeners(combatState))
      amount1 = combatHookListener.ModifySummonAmount(summoner, amount1, source);
    return amount1;
  }

  public static Creature ModifyUnblockedDamageTarget(
    ICombatState combatState,
    Creature originalTarget,
    Decimal amount,
    ValueProp props,
    Creature? dealer)
  {
    Creature target = originalTarget;
    foreach (AbstractModel iterateHookListener in combatState.IterateHookListeners())
      target = iterateHookListener.ModifyUnblockedDamageTarget(target, amount, props, dealer);
    return target;
  }

  public static IReadOnlySet<RoomType> ModifyUnknownMapPointRoomTypes(
    IRunState runState,
    IReadOnlySet<RoomType> roomTypes)
  {
    IReadOnlySet<RoomType> roomTypes1 = (IReadOnlySet<RoomType>) new HashSet<RoomType>((IEnumerable<RoomType>) roomTypes);
    foreach (AbstractModel iterateHookListener in runState.IterateHookListeners((ICombatState) null))
      roomTypes1 = iterateHookListener.ModifyUnknownMapPointRoomTypes(roomTypes1);
    return roomTypes1;
  }

  public static int ModifyXValue(ICombatState combatState, CardModel card, int originalValue)
  {
    int originalValue1 = originalValue;
    foreach (AbstractModel combatHookListener in Hook.IterateCombatHookListeners(combatState))
      originalValue1 = combatHookListener.ModifyXValue(card, originalValue1);
    return originalValue1;
  }

  public static bool ShouldAddToDeck(
    IRunState runState,
    CardModel card,
    out AbstractModel? preventer)
  {
    foreach (AbstractModel iterateHookListener in runState.IterateHookListeners((ICombatState) null))
    {
      if (!iterateHookListener.ShouldAddToDeck(card))
      {
        preventer = iterateHookListener;
        return false;
      }
    }
    preventer = (AbstractModel) null;
    return true;
  }

  public static bool ShouldAfflict(
    ICombatState combatState,
    CardModel card,
    AfflictionModel affliction)
  {
    foreach (AbstractModel combatHookListener in Hook.IterateCombatHookListeners(combatState))
    {
      if (!combatHookListener.ShouldAfflict(card, affliction))
        return false;
    }
    return true;
  }

  public static bool ShouldAllowAncient(
    IRunState runState,
    Player player,
    AncientEventModel ancient)
  {
    foreach (AbstractModel iterateHookListener in runState.IterateHookListeners((ICombatState) null))
    {
      if (!iterateHookListener.ShouldAllowAncient(player, ancient))
        return false;
    }
    return true;
  }

  public static bool ShouldAllowHitting(ICombatState combatState, Creature creature)
  {
    foreach (AbstractModel combatHookListener in Hook.IterateCombatHookListeners(combatState))
    {
      if (!combatHookListener.ShouldAllowHitting(creature))
        return false;
    }
    return true;
  }

  public static bool ShouldAllowMerchantCardRemoval(IRunState runState, Player player)
  {
    foreach (AbstractModel iterateHookListener in runState.IterateHookListeners((ICombatState) null))
    {
      if (!iterateHookListener.ShouldAllowMerchantCardRemoval(player))
        return false;
    }
    return true;
  }

  public static bool ShouldAllowSelectingMoreCardRewards(
    IRunState runState,
    Player player,
    CardReward reward)
  {
    foreach (AbstractModel iterateHookListener in runState.IterateHookListeners((ICombatState) null))
    {
      if (iterateHookListener.ShouldAllowSelectingMoreCardRewards(player, reward))
        return true;
    }
    return false;
  }

  public static bool ShouldAllowTargeting(
    ICombatState combatState,
    Creature target,
    out AbstractModel? preventer)
  {
    foreach (AbstractModel combatHookListener in Hook.IterateCombatHookListeners(combatState))
    {
      if (!combatHookListener.ShouldAllowTargeting(target))
      {
        preventer = combatHookListener;
        return false;
      }
    }
    preventer = (AbstractModel) null;
    return true;
  }

  public static bool ShouldClearBlock(
    ICombatState combatState,
    Creature creature,
    out AbstractModel? preventer)
  {
    foreach (AbstractModel combatHookListener in Hook.IterateCombatHookListeners(combatState))
    {
      if (!combatHookListener.ShouldClearBlock(creature))
      {
        preventer = combatHookListener;
        return false;
      }
    }
    preventer = (AbstractModel) null;
    return true;
  }

  public static bool ShouldCreatureBeRemovedFromCombatAfterDeath(
    ICombatState combatState,
    Creature creature)
  {
    foreach (AbstractModel iterateHookListener in combatState.IterateHookListeners())
    {
      if (!iterateHookListener.ShouldCreatureBeRemovedFromCombatAfterDeath(creature))
        return false;
    }
    return true;
  }

  public static bool ShouldDie(
    IRunState runState,
    ICombatState? combatState,
    Creature creature,
    out AbstractModel? preventer)
  {
    foreach (AbstractModel iterateHookListener in runState.IterateHookListeners(combatState))
    {
      if (!iterateHookListener.ShouldDie(creature))
      {
        preventer = iterateHookListener;
        return false;
      }
    }
    foreach (AbstractModel iterateHookListener in runState.IterateHookListeners(combatState))
    {
      if (!iterateHookListener.ShouldDieLate(creature))
      {
        preventer = iterateHookListener;
        return false;
      }
    }
    preventer = (AbstractModel) null;
    return true;
  }

  public static bool ShouldDisableRemainingRestSiteOptions(IRunState runState, Player player)
  {
    foreach (AbstractModel iterateHookListener in runState.IterateHookListeners((ICombatState) null))
    {
      if (!iterateHookListener.ShouldDisableRemainingRestSiteOptions(player))
        return false;
    }
    return true;
  }

  public static bool ShouldDraw(
    ICombatState combatState,
    Player player,
    bool fromHandDraw,
    out AbstractModel? modifier)
  {
    foreach (AbstractModel combatHookListener in Hook.IterateCombatHookListeners(combatState))
    {
      if (!combatHookListener.ShouldDraw(player, fromHandDraw))
      {
        modifier = combatHookListener;
        return false;
      }
    }
    modifier = (AbstractModel) null;
    return true;
  }

  public static bool ShouldEtherealTrigger(ICombatState combatState, CardModel card)
  {
    foreach (AbstractModel combatHookListener in Hook.IterateCombatHookListeners(combatState))
    {
      if (!combatHookListener.ShouldEtherealTrigger(card))
        return false;
    }
    return true;
  }

  public static bool ShouldFlush(ICombatState combatState, Player player)
  {
    foreach (AbstractModel combatHookListener in Hook.IterateCombatHookListeners(combatState))
    {
      if (!combatHookListener.ShouldFlush(player))
        return false;
    }
    return true;
  }

  public static bool ShouldGenerateTreasure(IRunState runState, Player player)
  {
    foreach (AbstractModel iterateHookListener in runState.IterateHookListeners((ICombatState) null))
    {
      if (!iterateHookListener.ShouldGenerateTreasure(player))
        return false;
    }
    return true;
  }

  public static bool ShouldGainStars(ICombatState combatState, Decimal amount, Player player)
  {
    foreach (AbstractModel combatHookListener in Hook.IterateCombatHookListeners(combatState))
    {
      if (!combatHookListener.ShouldGainStars(amount, player))
        return false;
    }
    return true;
  }

  public static bool ShouldPayExcessEnergyCostWithStars(ICombatState combatState, Player player)
  {
    foreach (AbstractModel combatHookListener in Hook.IterateCombatHookListeners(combatState))
    {
      if (combatHookListener.ShouldPayExcessEnergyCostWithStars(player))
        return true;
    }
    return false;
  }

  public static bool ShouldPlay(
    ICombatState combatState,
    CardModel card,
    out AbstractModel? preventer,
    AutoPlayType autoPlayType)
  {
    foreach (AbstractModel combatHookListener in Hook.IterateCombatHookListeners(combatState))
    {
      if (!combatHookListener.ShouldPlay(card, autoPlayType))
      {
        preventer = combatHookListener;
        return false;
      }
    }
    preventer = (AbstractModel) null;
    return true;
  }

  public static bool ShouldPlayerResetEnergy(ICombatState combatState, Player player)
  {
    foreach (AbstractModel combatHookListener in Hook.IterateCombatHookListeners(combatState))
    {
      if (!combatHookListener.ShouldPlayerResetEnergy(player))
        return false;
    }
    return true;
  }

  public static bool ShouldProceedToNextMapPoint(IRunState runState)
  {
    foreach (AbstractModel iterateHookListener in runState.IterateHookListeners((ICombatState) null))
    {
      if (!iterateHookListener.ShouldProceedToNextMapPoint())
        return false;
    }
    return true;
  }

  public static bool ShouldProcurePotion(
    IRunState runState,
    ICombatState? combatState,
    PotionModel potion,
    Player player)
  {
    foreach (AbstractModel iterateHookListener in runState.IterateHookListeners(combatState))
    {
      if (!iterateHookListener.ShouldProcurePotion(potion, player))
        return false;
    }
    return true;
  }

  public static bool ShouldRefillMerchantEntry(
    IRunState runState,
    MerchantEntry entry,
    Player player)
  {
    foreach (AbstractModel iterateHookListener in runState.IterateHookListeners((ICombatState) null))
    {
      if (iterateHookListener.ShouldRefillMerchantEntry(entry, player))
        return true;
    }
    return false;
  }

  public static bool ShouldStopCombatFromEnding(ICombatState combatState)
  {
    foreach (AbstractModel iterateHookListener in combatState.IterateHookListeners())
    {
      if (iterateHookListener.ShouldStopCombatFromEnding())
        return true;
    }
    return false;
  }

  public static bool ShouldTakeExtraTurn(ICombatState combatState, Player player)
  {
    foreach (AbstractModel combatHookListener in Hook.IterateCombatHookListeners(combatState))
    {
      if (combatHookListener.ShouldTakeExtraTurn(player))
        return true;
    }
    return false;
  }

  public static bool ShouldForcePotionReward(IRunState runState, Player player, RoomType roomType)
  {
    bool flag = false;
    foreach (AbstractModel iterateHookListener in runState.IterateHookListeners((ICombatState) null))
      flag = flag || iterateHookListener.ShouldForcePotionReward(player, roomType);
    return flag;
  }

  public static bool ShouldAllowFreeTravel(IRunState runState)
  {
    bool flag = false;
    foreach (AbstractModel iterateHookListener in runState.IterateHookListeners((ICombatState) null))
      flag = flag || iterateHookListener.ShouldAllowFreeTravel();
    return flag;
  }

  public static bool ShouldPowerBeRemovedOnDeath(PowerModel power)
  {
    if (power.Owner.CombatState == null)
      return true;
    foreach (AbstractModel iterateHookListener in power.CombatState.IterateHookListeners())
    {
      if (!iterateHookListener.ShouldPowerBeRemovedOnDeath(power))
        return false;
    }
    return true;
  }

  private static Decimal ModifyDamageInternal(
    IRunState runState,
    ICombatState? combatState,
    Creature? target,
    Creature? dealer,
    Decimal damage,
    ValueProp props,
    CardModel? cardSource,
    CardPlay? cardPlay,
    ModifyDamageHookType modifyDamageHookType,
    out List<AbstractModel> modifiers)
  {
    Decimal amount = damage;
    List<AbstractModel> abstractModelList = new List<AbstractModel>();
    if (modifyDamageHookType.HasFlag((Enum) ModifyDamageHookType.Additive))
    {
      foreach (AbstractModel iterateHookListener in runState.IterateHookListeners(combatState))
      {
        Decimal num = iterateHookListener.ModifyDamageAdditive(target, amount, props, dealer, cardSource, cardPlay);
        amount += num;
        if (num != 0M)
          abstractModelList.Add(iterateHookListener);
      }
    }
    if (modifyDamageHookType.HasFlag((Enum) ModifyDamageHookType.Multiplicative))
    {
      foreach (AbstractModel iterateHookListener in runState.IterateHookListeners(combatState))
      {
        Decimal num = iterateHookListener.ModifyDamageMultiplicative(target, amount, props, dealer, cardSource, cardPlay);
        amount *= num;
        if (num != 1M)
          abstractModelList.Add(iterateHookListener);
      }
    }
    if (modifyDamageHookType.HasFlag((Enum) ModifyDamageHookType.Cap))
    {
      Decimal num1 = Decimal.MaxValue;
      foreach (AbstractModel iterateHookListener in runState.IterateHookListeners(combatState))
      {
        Decimal num2 = iterateHookListener.ModifyDamageCap(target, props, dealer, cardSource, cardPlay);
        if (num2 < num1)
        {
          num1 = num2;
          if (amount > num2)
          {
            amount = num2;
            abstractModelList.Add(iterateHookListener);
          }
        }
      }
    }
    modifiers = abstractModelList;
    return amount;
  }
}
