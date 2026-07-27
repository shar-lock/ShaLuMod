// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Commands.CreatureCmd
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Audio.Debug;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Hooks;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Monsters;
using MegaCrit.Sts2.Core.Nodes;
using MegaCrit.Sts2.Core.Nodes.Audio;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.RestSite;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.Nodes.Vfx.Utilities;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.Runs.History;
using MegaCrit.Sts2.Core.TestSupport;
using MegaCrit.Sts2.Core.ValueProps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Commands;

public static class CreatureCmd
{
  public static async Task<Creature> Add<T>(ICombatState combatState, string? slotName = null) where T : MonsterModel
  {
    Creature creature = combatState.CreateCreature(ModelDb.Monster<T>().ToMutable(), CombatSide.Enemy, slotName);
    await CreatureCmd.Add(creature);
    Creature creature1 = creature;
    creature = (Creature) null;
    return creature1;
  }

  public static async Task<Creature> Add(
    MonsterModel monster,
    ICombatState combatState,
    CombatSide side = CombatSide.Enemy,
    string? slotName = null)
  {
    monster.AssertMutable();
    Creature creature = combatState.CreateCreature(monster, side, slotName);
    await CreatureCmd.Add(creature);
    Creature creature1 = creature;
    creature = (Creature) null;
    return creature1;
  }

  public static async Task Add(Creature creature)
  {
    if (!CombatManager.Instance.IsInProgress)
      throw new InvalidOperationException("Attempted to add a creature outside of combat.");
    ICombatState combatState = creature.CombatState;
    if (combatState == null)
      throw new InvalidOperationException("Attempted to add a creature with no combat state.");
    if (!combatState.IsLiveCombat())
    {
      combatState = (ICombatState) null;
    }
    else
    {
      combatState.AddCreature(creature);
      CombatManager.Instance.AddCreature(creature);
      NCombatRoom.Instance?.AddCreature(creature);
      await CombatManager.Instance.AfterCreatureAdded(creature);
      if (combatState.CurrentSide != CombatSide.Enemy && creature.IsMonster)
        creature.PrepareForNextTurn(combatState.Players.Select<Player, Creature>((Func<Player, Creature>) (p => p.Creature)), false);
      MapPointHistoryEntry pointHistoryEntry = combatState.RunState.CurrentMapPointHistoryEntry;
      MapPointRoomHistoryEntry roomHistoryEntry = pointHistoryEntry != null ? pointHistoryEntry.Rooms.Last<MapPointRoomHistoryEntry>() : (MapPointRoomHistoryEntry) null;
      if (roomHistoryEntry != null && creature != null && creature.Monster != null && creature.Side == CombatSide.Enemy && !roomHistoryEntry.MonsterIds.Contains(creature.Monster.Id))
        roomHistoryEntry.MonsterIds.Add(creature.Monster.Id);
      await Hook.AfterCreatureAddedToCombat(creature.CombatState, creature);
      combatState = (ICombatState) null;
    }
  }

  public static async Task<IEnumerable<DamageResult>> Damage(
    PlayerChoiceContext choiceContext,
    Creature target,
    DamageVar damageVar,
    CardModel cardSource,
    CardPlay? cardPlay)
  {
    return await CreatureCmd.Damage(choiceContext, target, damageVar.BaseValue, damageVar.Props, cardSource, cardPlay);
  }

  public static async Task<IEnumerable<DamageResult>> Damage(
    PlayerChoiceContext choiceContext,
    Creature target,
    Decimal amount,
    ValueProp props,
    CardModel? cardSource,
    CardPlay? cardPlay)
  {
    PlayerChoiceContext choiceContext1 = choiceContext;
    List<Creature> targets = new List<Creature>();
    targets.Add(target);
    Decimal amount1 = amount;
    int props1 = (int) props;
    Creature creature = cardSource?.Owner.Creature;
    CardModel cardSource1 = cardSource;
    CardPlay cardPlay1 = cardPlay;
    return await CreatureCmd.Damage(choiceContext1, (IEnumerable<Creature>) targets, amount1, (ValueProp) props1, creature, cardSource1, cardPlay1);
  }

  public static async Task<IEnumerable<DamageResult>> Damage(
    PlayerChoiceContext choiceContext,
    IEnumerable<Creature> targets,
    DamageVar damageVar,
    Creature dealer)
  {
    return await CreatureCmd.Damage(choiceContext, targets, damageVar.BaseValue, damageVar.Props, dealer);
  }

  public static async Task<IEnumerable<DamageResult>> Damage(
    PlayerChoiceContext choiceContext,
    IEnumerable<Creature> targets,
    Decimal amount,
    ValueProp props,
    Creature dealer)
  {
    return await CreatureCmd.Damage(choiceContext, targets, amount, props, dealer, (CardModel) null, (CardPlay) null);
  }

  public static async Task<IEnumerable<DamageResult>> Damage(
    PlayerChoiceContext choiceContext,
    Creature target,
    DamageVar damageVar,
    Creature dealer)
  {
    return await CreatureCmd.Damage(choiceContext, target, damageVar.BaseValue, damageVar.Props, dealer);
  }

  public static async Task<IEnumerable<DamageResult>> Damage(
    PlayerChoiceContext choiceContext,
    Creature target,
    Decimal amount,
    ValueProp props,
    Creature dealer)
  {
    // ISSUE: object of a compiler-generated type is created
    return await CreatureCmd.Damage(choiceContext, (IEnumerable<Creature>) new \u003C\u003Ez__ReadOnlySingleElementList<Creature>(target), amount, props, dealer, (CardModel) null, (CardPlay) null);
  }

  public static async Task<IEnumerable<DamageResult>> Damage(
    PlayerChoiceContext choiceContext,
    Creature target,
    DamageVar damageVar,
    Creature? dealer,
    CardModel? cardSource,
    CardPlay? cardPlay)
  {
    // ISSUE: object of a compiler-generated type is created
    return await CreatureCmd.Damage(choiceContext, (IEnumerable<Creature>) new \u003C\u003Ez__ReadOnlySingleElementList<Creature>(target), damageVar.BaseValue, damageVar.Props, dealer, cardSource, cardPlay);
  }

  public static async Task<IEnumerable<DamageResult>> Damage(
    PlayerChoiceContext choiceContext,
    Creature target,
    Decimal amount,
    ValueProp props,
    Creature? dealer,
    CardModel? cardSource,
    CardPlay? cardPlay)
  {
    // ISSUE: object of a compiler-generated type is created
    return await CreatureCmd.Damage(choiceContext, (IEnumerable<Creature>) new \u003C\u003Ez__ReadOnlySingleElementList<Creature>(target), amount, props, dealer, cardSource, cardPlay);
  }

  public static async Task<IEnumerable<DamageResult>> Damage(
    PlayerChoiceContext choiceContext,
    IEnumerable<Creature>? targets,
    DamageVar damageVar,
    Creature? dealer,
    CardModel? cardSource,
    CardPlay? cardPlay)
  {
    return await CreatureCmd.Damage(choiceContext, targets, damageVar.BaseValue, damageVar.Props, dealer, cardSource, cardPlay);
  }

  public static async Task<IEnumerable<DamageResult>> Damage(
    PlayerChoiceContext choiceContext,
    IEnumerable<Creature>? targets,
    Decimal amount,
    ValueProp props,
    Creature? dealer,
    CardModel? cardSource,
    CardPlay? cardPlay)
  {
    if (targets == null)
      return (IEnumerable<DamageResult>) Array.Empty<DamageResult>();
    if (dealer != null && dealer.IsDead)
      return (IEnumerable<DamageResult>) targets.Select<Creature, DamageResult>((Func<Creature, DamageResult>) (t => new DamageResult(t, props))).ToList<DamageResult>();
    List<DamageResult> results = new List<DamageResult>();
    List<Creature> targetList = targets.ToList<Creature>();
    if (targetList.Count == 0)
      return (IEnumerable<DamageResult>) results;
    ICombatState combatState = targetList[0].CombatState;
    IRunState runState = IRunState.GetFrom(targetList.Append<Creature>(dealer).OfType<Creature>());
    Creature originalTarget;
    DamageResult unblockedDamageResult;
    foreach (Creature creature in targetList)
    {
      originalTarget = creature;
      if (!originalTarget.IsDead)
      {
        IEnumerable<AbstractModel> modifiers;
        Decimal modifiedAmount = Hook.ModifyDamage(runState, combatState, originalTarget, dealer, amount, props, cardSource, cardPlay, ModifyDamageHookType.All, CardPreviewMode.None, out modifiers);
        await Hook.AfterModifyingDamageAmount(runState, combatState, cardSource, modifiers);
        await Hook.BeforeDamageReceived(choiceContext, runState, combatState, originalTarget, modifiedAmount, props, dealer, cardSource);
        Decimal blockedDamage = (originalTarget.PetOwner?.Creature ?? originalTarget).DamageBlockInternal(modifiedAmount, props);
        Decimal unblockedDamage = Hook.ModifyHpLost(runState, combatState, originalTarget, Math.Max(modifiedAmount - blockedDamage, 0M), props, dealer, cardSource, HpLossHookPhase.BeforeOsty, out modifiers);
        await Hook.AfterModifyingHpLostBeforeOsty(runState, combatState, modifiers);
        Creature unblockedDamageTarget = combatState == null ? originalTarget : Hook.ModifyUnblockedDamageTarget(combatState, originalTarget, unblockedDamage, props, dealer);
        unblockedDamage = Hook.ModifyHpLost(runState, combatState, unblockedDamageTarget, unblockedDamage, props, dealer, cardSource, HpLossHookPhase.AfterOsty, out modifiers);
        await Hook.AfterModifyingHpLostAfterOsty(runState, combatState, modifiers);
        unblockedDamageResult = unblockedDamageTarget.LoseHpInternal(unblockedDamage, props);
        List<DamageResult> damageResults = new List<DamageResult>(1)
        {
          unblockedDamageResult
        };
        bool wasBlockBroken = originalTarget.Block <= 0 && blockedDamage > 0M;
        bool wasFullyBlocked = !props.HasFlag((Enum) ValueProp.Unblockable) && (blockedDamage > 0M || originalTarget.Block > 0) && (int) unblockedDamage == 0;
        if (originalTarget == unblockedDamageTarget)
        {
          unblockedDamageResult.BlockedDamage = (int) blockedDamage;
          unblockedDamageResult.WasBlockBroken = wasBlockBroken;
          unblockedDamageResult.WasFullyBlocked = wasFullyBlocked;
        }
        else
        {
          Decimal originalTargetDamage = Hook.ModifyHpLost(runState, combatState, originalTarget, (Decimal) unblockedDamageResult.OverkillDamage, props, dealer, cardSource, HpLossHookPhase.AfterOsty, out modifiers);
          await Hook.AfterModifyingHpLostAfterOsty(runState, combatState, modifiers);
          DamageResult damageResult = !(originalTargetDamage > 0M) ? new DamageResult(originalTarget, props) : originalTarget.LoseHpInternal(originalTargetDamage, props);
          damageResult.BlockedDamage = (int) blockedDamage;
          damageResult.WasBlockBroken = wasBlockBroken;
          damageResult.WasFullyBlocked = wasFullyBlocked;
          damageResults.Add(damageResult);
        }
        List<Task> hitTriggers = new List<Task>();
        foreach (DamageResult result in damageResults)
        {
          int damage = result.UnblockedDamage + result.OverkillDamage;
          Creature receiver = result.Receiver;
          if (CombatManager.Instance.IsInProgress && !CombatManager.Instance.IsEnding)
            CombatManager.Instance.History.DamageReceived(combatState, receiver, dealer, result, cardSource);
          if (!result.WasFullyBlocked)
          {
            Node vfxContainer = (Node) receiver.GetVfxContainer();
            if (damage > 0 || modifiedAmount == 0M && result.Receiver == unblockedDamageTarget)
            {
              NDamageNumVfx child = NDamageNumVfx.Create(receiver, result);
              if (child != null)
              {
                if (vfxContainer != null)
                  vfxContainer.AddChildSafely((Node) child);
                else
                  ((Node) NRun.Instance.GlobalUi).AddChildSafely((Node) child);
              }
            }
            if (damage > 0)
            {
              if (vfxContainer != null)
                vfxContainer.AddChildSafely((Node) NHitSparkVfx.Create(receiver));
              if (receiver != dealer && !props.HasFlag((Enum) ValueProp.SkipHurtAnim))
              {
                hitTriggers.Add(CreatureCmd.TriggerAnim(receiver, "Hit", 0.0f));
                if (receiver.IsMonster && receiver.Monster.HasHurtSfx)
                  SfxCmd.Play(receiver.Monster.HurtSfx);
              }
              MapPointHistoryEntry pointHistoryEntry = receiver.Player?.RunState.CurrentMapPointHistoryEntry;
              if (pointHistoryEntry != null)
                pointHistoryEntry.GetEntry(receiver.Player.NetId).DamageTaken += result.UnblockedDamage;
            }
            await Task.WhenAll((IEnumerable<Task>) hitTriggers);
            if (damage > 0)
            {
              if (damageResults.Any<DamageResult>((Func<DamageResult, bool>) (r => r.WasBlockBroken)))
                SfxCmd.Play("event:/sfx/block_break");
              if (LocalContext.IsMe(originalTarget) && (!CombatManager.Instance.IsInProgress || originalTarget.GetHpPercentRemaining() <= 0.25))
                PlayerHurtVignetteHelper.Play();
              if (originalTarget.Side == CombatSide.Enemy)
                SfxCmd.PlayDamage(originalTarget.Monster, unblockedDamageResult.UnblockedDamage);
              if (CombatManager.Instance.IsInProgress || LocalContext.ContainsMe((IEnumerable<Creature>) targetList))
              {
                if (unblockedDamageResult.UnblockedDamage < 6)
                  NGame.Instance?.ScreenShake(ShakeStrength.Weak, ShakeDuration.Short);
                else if (unblockedDamageResult.UnblockedDamage < 11)
                  NGame.Instance?.ScreenShake(ShakeStrength.Medium, ShakeDuration.Short);
                else
                  NGame.Instance?.ScreenShake(ShakeStrength.Strong, ShakeDuration.Short);
              }
            }
          }
        }
        results.AddRange((IEnumerable<DamageResult>) damageResults);
        unblockedDamageTarget = (Creature) null;
        unblockedDamageResult = (DamageResult) null;
        damageResults = (List<DamageResult>) null;
        hitTriggers = (List<Task>) null;
        originalTarget = (Creature) null;
      }
    }
    List<Creature> killedCreatures = new List<Creature>();
    foreach (DamageResult damageResult in results)
    {
      unblockedDamageResult = damageResult;
      originalTarget = unblockedDamageResult.Receiver;
      if (unblockedDamageResult.WasBlockBroken)
        await Hook.AfterBlockBroken(originalTarget.CombatState, choiceContext, originalTarget, dealer);
      if (unblockedDamageResult.UnblockedDamage > 0)
        await Hook.AfterCurrentHpChanged(runState, combatState, originalTarget, (Decimal) -unblockedDamageResult.UnblockedDamage);
      if (dealer != null && dealer.Player != null && originalTarget.Player == null)
        dealer.Player.ExtraFields.DamageDealt += unblockedDamageResult.UnblockedDamage;
      if (combatState != null)
        await Hook.AfterDamageGiven(choiceContext, combatState, dealer, unblockedDamageResult, props, originalTarget, cardSource);
      if (unblockedDamageResult.WasTargetKilled && originalTarget.IsDead)
        killedCreatures.Add(originalTarget);
      else
        await Hook.AfterDamageReceived(choiceContext, runState, combatState, originalTarget, unblockedDamageResult, props, dealer, cardSource);
      if (unblockedDamageResult.WasFullyBlocked && CombatManager.Instance.IsInProgress)
      {
        SfxCmd.Play("event:/sfx/block_hit");
        Node vfxContainer = (Node) unblockedDamageResult.Receiver.GetVfxContainer();
        if (vfxContainer != null)
          vfxContainer.AddChildSafely((Node) NBlockSparkVfx.Create(unblockedDamageResult.Receiver));
        if (vfxContainer != null)
          vfxContainer.AddChildSafely((Node) NDamageBlockedVfx.Create(unblockedDamageResult.Receiver));
        NGame.Instance?.ScreenShake(ShakeStrength.Weak, ShakeDuration.Short);
      }
      originalTarget = (Creature) null;
      unblockedDamageResult = (DamageResult) null;
    }
    await CreatureCmd.Kill((IReadOnlyCollection<Creature>) killedCreatures);
    await Cmd.CustomScaledWait(0.1f, 0.2f);
    return (IEnumerable<DamageResult>) results;
  }

  public static async Task Kill(Creature creature, bool force = false)
  {
    // ISSUE: object of a compiler-generated type is created
    await CreatureCmd.Kill((IReadOnlyCollection<Creature>) new \u003C\u003Ez__ReadOnlySingleElementList<Creature>(creature), force);
  }

  public static async Task Kill(IReadOnlyCollection<Creature> creatures, bool force = false)
  {
    IRunState runState;
    if (creatures.Count == 0)
    {
      runState = (IRunState) null;
    }
    else
    {
      runState = creatures.FirstOrDefault<Creature>((Func<Creature, bool>) (c => c.IsPlayer))?.Player?.RunState;
      foreach (Creature creature in creatures.ToList<Creature>())
        await CreatureCmd.KillWithoutCheckingWinCondition(creature, force);
      if (!RunManager.Instance.IsInProgress)
        runState = (IRunState) null;
      else if (RunManager.Instance.IsCleaningUp)
        runState = (IRunState) null;
      else if (runState != null && runState.Players.All<Player>((Func<Player, bool>) (p => p.Creature.IsDead)))
      {
        if (CombatManager.Instance.IsInProgress)
          CombatManager.Instance.LoseCombat();
        if (!TestMode.IsOff)
        {
          runState = (IRunState) null;
        }
        else
        {
          NRun.Instance.RunMusicController.StopMusic();
          NDebugAudioManager.Instance?.StopAll();
          NAudioManager.Instance.PlayMusic("event:/temp/sfx/game_over");
          NRun.Instance.ShowGameOverScreen(RunManager.Instance.OnEnded(false));
          runState = (IRunState) null;
        }
      }
      else if (!CombatManager.Instance.IsInProgress)
      {
        runState = (IRunState) null;
      }
      else
      {
        using (List<Creature>.Enumerator enumerator = creatures.ToList<Creature>().GetEnumerator())
        {
          while (enumerator.MoveNext())
          {
            Creature current = enumerator.Current;
            if (current != null)
            {
              ICombatState combatState = current.CombatState;
              if (combatState != null && combatState.CurrentSide == CombatSide.Player && current.IsDead && current.IsPlayer)
                PlayerCmd.EndTurn(current.Player, false);
            }
          }
          runState = (IRunState) null;
        }
      }
    }
  }

  private static async Task KillWithoutCheckingWinCondition(
    Creature creature,
    bool force,
    int recursion = 0)
  {
    ICombatState combatState;
    IRunState runState;
    AbstractModel preventer;
    if (creature.CombatState == null && !creature.IsPlayer)
    {
      combatState = (ICombatState) null;
      runState = (IRunState) null;
      preventer = (AbstractModel) null;
    }
    else if (creature.CombatState != null && !creature.CombatState.IsLiveCombat())
    {
      combatState = (ICombatState) null;
      runState = (IRunState) null;
      preventer = (AbstractModel) null;
    }
    else
    {
      if (!CombatManager.Instance.IsInProgress && creature.IsPlayer && creature.Player.RunState.Players.Count > 1)
      {
        AbstractRoom currentRoom = creature.Player.RunState.CurrentRoom;
        if ((currentRoom != null ? (currentRoom.IsVictoryRoom ? 1 : 0) : 0) == 0)
        {
          Log.Error($"Player {creature.Player.NetId} has been killed outside of combat in multiplayer! This should not occur");
          await CreatureCmd.Heal(creature, 1M);
          combatState = (ICombatState) null;
          runState = (IRunState) null;
          preventer = (AbstractModel) null;
          return;
        }
      }
      combatState = creature.CombatState;
      // ISSUE: object of a compiler-generated type is created
      runState = IRunState.GetFrom((IEnumerable<Creature>) new \u003C\u003Ez__ReadOnlySingleElementList<Creature>(creature));
      int currentHp = creature.CurrentHp;
      if (currentHp > 0)
      {
        creature.LoseHpInternal((Decimal) currentHp, ValueProp.Unblockable | ValueProp.Unpowered);
        await Hook.AfterCurrentHpChanged(runState, creature.CombatState, creature, (Decimal) -currentHp);
      }
      await Hook.BeforeDeath(runState, combatState, creature);
      preventer = (AbstractModel) null;
      if (force || creature.MaxHp <= 0 || Hook.ShouldDie(runState, combatState, creature, out preventer))
      {
        creature.InvokeDiedEvent();
        bool shouldRemoveFromCombat = combatState != null && Hook.ShouldCreatureBeRemovedFromCombatAfterDeath(combatState, creature);
        float deathAnimLength = 0.0f;
        NCreature creatureNode = NCombatRoom.Instance?.GetCreatureNode(creature);
        if (creatureNode != null)
        {
          deathAnimLength = creatureNode.StartDeathAnim(shouldRemoveFromCombat && creature.IsMonster);
          if (shouldRemoveFromCombat && creature.IsMonster)
            NCombatRoom.Instance?.RemoveCreatureNode(creatureNode);
        }
        await Hook.AfterDeath(runState, combatState, creature, false, deathAnimLength);
        ICombatState combatState1 = combatState;
        List<Creature> teammates = (combatState1 != null ? combatState1.GetTeammatesOf(creature).Where<Creature>((Func<Creature, bool>) (t => t.IsAlive)).ToList<Creature>() : (List<Creature>) null) ?? new List<Creature>();
        if (shouldRemoveFromCombat && creature.Side == CombatSide.Enemy)
        {
          ICombatState combatState2 = combatState;
          if ((combatState2 != null ? (combatState2.Enemies.Contains<Creature>(creature) ? 1 : 0) : 0) != 0)
          {
            CombatManager.Instance.RemoveCreature(creature);
            MonsterModel monster = creature.Monster;
            if (monster != null && !monster.IsPerformingMove)
              combatState.RemoveCreature(creature);
          }
        }
        bool isPrimaryEnemy = creature.IsPrimaryEnemy;
        foreach (PowerModel powerModel in creature.RemoveAllPowersAfterDeath())
          await powerModel.AfterRemoved(creature);
        if (creature.Side == CombatSide.Enemy)
        {
          if (isPrimaryEnemy && teammates.Count != 0 && teammates.All<Creature>((Func<Creature, bool>) (t => t.IsSecondaryEnemy)))
            await CreatureCmd.Kill((IReadOnlyCollection<Creature>) teammates);
        }
        else if (creature.IsPlayer)
        {
          Player player = creature.Player;
          player.PlayerCombatState?.OrbQueue.Clear();
          if (player.IsOstyAlive)
            await CreatureCmd.Kill(player.Osty, force);
          player.DeactivateHooks();
          if (combatState != null && !combatState.Players.All<Player>((Func<Player, bool>) (p => p.Creature.IsDead)))
            await CombatManager.Instance.HandlePlayerDeath(player);
          player = (Player) null;
        }
        teammates = (List<Creature>) null;
        combatState = (ICombatState) null;
        runState = (IRunState) null;
        preventer = (AbstractModel) null;
      }
      else
      {
        if (recursion >= 10)
          throw new InvalidOperationException("Combat is ending, but something is continually preventing the last creature from being killed!");
        await Hook.AfterDeath(runState, combatState, creature, true, 0.0f);
        await Hook.AfterPreventingDeath(runState, combatState, preventer, creature);
        if (!creature.IsDead)
        {
          combatState = (ICombatState) null;
          runState = (IRunState) null;
          preventer = (AbstractModel) null;
        }
        else
        {
          await CreatureCmd.KillWithoutCheckingWinCondition(creature, force, recursion + 1);
          combatState = (ICombatState) null;
          runState = (IRunState) null;
          preventer = (AbstractModel) null;
        }
      }
    }
  }

  public static Task Escape(Creature creature, bool removeCreatureNode = true)
  {
    if (creature.IsDead || creature.CombatState == null || !creature.CombatState.IsLiveCombat())
      return Task.CompletedTask;
    creature.RemoveAllPowersInternalExcept();
    if (removeCreatureNode)
    {
      NCreature creatureNode = NCombatRoom.Instance?.GetCreatureNode(creature);
      if (creatureNode != null)
      {
        NCombatRoom.Instance.RemoveCreatureNode(creatureNode);
        creatureNode.ToggleIsInteractable(false);
        ((CanvasItem) creatureNode).Visible = false;
      }
    }
    CombatManager.Instance.RemoveCreature(creature);
    creature.CombatState?.CreatureEscaped(creature);
    return Task.CompletedTask;
  }

  public static async Task<Decimal> GainBlock(
    Creature creature,
    BlockVar blockVar,
    CardPlay? cardPlay,
    bool fast = false)
  {
    return await CreatureCmd.GainBlock(creature, blockVar.BaseValue, blockVar.Props, cardPlay, fast);
  }

  public static async Task<Decimal> GainBlock(
    Creature creature,
    Decimal amount,
    ValueProp props,
    CardPlay? cardPlay,
    bool fast = false)
  {
    if (CombatManager.Instance.IsOverOrEnding)
      return 0M;
    if (creature.IsDead)
      return 0M;
    ICombatState combatState = creature.CombatState;
    await Hook.BeforeBlockGained(combatState, creature, amount, props, cardPlay?.Card);
    Decimal modifiedAmount = amount;
    IEnumerable<AbstractModel> modifiers;
    modifiedAmount = Hook.ModifyBlock(combatState, creature, modifiedAmount, props, cardPlay?.Card, cardPlay, out modifiers);
    modifiedAmount = Math.Max(modifiedAmount, 0M);
    await Hook.AfterModifyingBlockAmount(combatState, modifiedAmount, cardPlay?.Card, cardPlay, modifiers);
    if (modifiedAmount > 0M)
    {
      SfxCmd.Play("event:/sfx/block_gain");
      VfxCmd.PlayOnCreatureCenter(creature, "vfx/vfx_block");
      creature.GainBlockInternal(modifiedAmount);
      CombatManager.Instance.History.BlockGained(combatState, creature, (int) modifiedAmount, props, cardPlay);
      if (fast)
        await Cmd.CustomScaledWait(0.0f, 0.03f);
      else
        await Cmd.CustomScaledWait(0.1f, 0.25f);
    }
    await Hook.AfterBlockGained(combatState, creature, modifiedAmount, props, cardPlay?.Card);
    return modifiedAmount;
  }

  public static async Task LoseBlock(
    PlayerChoiceContext choiceContext,
    Creature target,
    Decimal amount,
    Creature? remover)
  {
    if (CombatManager.Instance.IsOverOrEnding || target.IsDead || amount <= 0M)
      return;
    int block = target.Block;
    target.LoseBlockInternal(amount);
    if (block <= 0 || target.Block > 0)
      return;
    SfxCmd.Play("event:/sfx/block_break");
    await Hook.AfterBlockBroken(target.CombatState, choiceContext, target, remover);
  }

  public static async Task Heal(Creature creature, Decimal amount, bool playAnim = true)
  {
    if (CombatManager.Instance.IsEnding && !creature.IsPlayer)
      ;
    else
    {
      bool isDead = creature.IsDead;
      Decimal num = Math.Min(amount, (Decimal) (creature.MaxHp - creature.CurrentHp));
      if (creature == null || !(creature.Monster is Osty))
        SfxCmd.Play("event:/sfx/heal");
      creature.HealInternal(amount);
      if (playAnim)
      {
        if (CombatManager.Instance.IsInProgress || creature.Player?.RunState.CurrentRoom is CombatRoom)
        {
          if (creature != null && creature.Monster is Osty)
            VfxCmd.PlayOnCreatureCenter(creature, "vfx/vfx_heal_osty");
          else
            VfxCmd.PlayOnCreatureCenter(creature, "vfx/vfx_cross_heal");
          Node vfxContainer = (Node) creature.GetVfxContainer();
          if (vfxContainer != null)
            vfxContainer.AddChildSafely((Node) NHealNumVfx.Create(creature, amount));
          if (isDead)
            NCombatRoom.Instance?.GetCreatureNode(creature)?.StartReviveAnim();
        }
        else if (LocalContext.IsMe(creature))
        {
          if (creature.Player.RunState.CurrentRoom is EventRoom)
            PlayerFullscreenHealVfx.Play(creature.Player, amount, NEventRoom.Instance?.VfxContainer);
          else if (creature.Player.RunState.CurrentRoom is MerchantRoom)
            PlayerFullscreenHealVfx.Play(creature.Player, amount, (Control) NMerchantRoom.Instance);
        }
        else if (creature.Player?.RunState.CurrentRoom is RestSiteRoom && TestMode.IsOff)
        {
          NRestSiteRoom instance = NRestSiteRoom.Instance;
          NRestSiteCharacter parent = instance != null ? instance.Characters.First<NRestSiteCharacter>((Func<NRestSiteCharacter, bool>) (c => c.Player == creature.Player)) : (NRestSiteCharacter) null;
          Node2D child = PreloadManager.Cache.GetScene(SceneHelper.GetScenePath("vfx/vfx_cross_heal")).Instantiate<Node2D>((PackedScene.GenEditState) 0L);
          if (parent != null)
            ((Node) parent).AddChildSafely((Node) child);
          child.Position = Vector2.Zero;
        }
      }
      MapPointHistoryEntry pointHistoryEntry = creature.Player?.RunState.CurrentMapPointHistoryEntry;
      if (pointHistoryEntry != null && num > 0M)
        pointHistoryEntry.GetEntry(creature.Player.NetId).HpHealed += (int) num;
      if (CombatManager.Instance.IsInProgress)
        await Cmd.CustomScaledWait(0.1f, 0.25f);
      if (!(amount > 0M))
        ;
      else if (creature.CombatState == null)
        ;
      else
        await Hook.AfterCurrentHpChanged(creature.Player?.RunState ?? creature.CombatState.RunState, creature.CombatState, creature, amount);
    }
  }

  public static async Task SetCurrentHp(Creature creature, Decimal amount)
  {
    bool flag = creature.IsDead && amount > 0M;
    Decimal currentHp = (Decimal) creature.CurrentHp;
    creature.SetCurrentHpInternal(amount);
    if (amount != currentHp)
    {
      if (CombatManager.Instance.IsInProgress && flag)
        NCombatRoom.Instance?.GetCreatureNode(creature)?.StartReviveAnim();
      await Hook.AfterCurrentHpChanged(creature.Player?.RunState ?? creature.CombatState?.RunState ?? (IRunState) NullRunState.Instance, creature.CombatState, creature, amount - currentHp);
    }
    if (!creature.IsDead)
      return;
    await CreatureCmd.Kill(creature);
  }

  public static async Task GainMaxHp(Creature creature, Decimal amount)
  {
    if (amount < 0M)
      throw new ArgumentException("amount must be non-negative. Use LoseMaxHp for max HP loss.");
    Decimal amount1 = await CreatureCmd.SetMaxHp(creature, (Decimal) creature.MaxHp + amount);
    MapPointHistoryEntry pointHistoryEntry = creature.Player?.RunState.CurrentMapPointHistoryEntry;
    if (pointHistoryEntry != null)
      pointHistoryEntry.GetEntry(creature.Player.NetId).MaxHpGained += (int) amount1;
    await CreatureCmd.Heal(creature, amount1);
  }

  public static async Task LoseMaxHp(
    PlayerChoiceContext choiceContext,
    Creature creature,
    Decimal amount,
    bool isFromCard)
  {
    if (amount < 0M)
      throw new ArgumentException("amount must be non-negative. Use GainMaxHp for max HP gain.");
    Decimal newMaxHp = (Decimal) creature.MaxHp - amount;
    MapPointHistoryEntry pointHistoryEntry = creature.Player?.RunState.CurrentMapPointHistoryEntry;
    if (pointHistoryEntry != null)
      pointHistoryEntry.GetEntry(creature.Player.NetId).MaxHpLost += (int) amount;
    if (newMaxHp < (Decimal) creature.CurrentHp)
    {
      IEnumerable<DamageResult> damageResults = await CreatureCmd.Damage(choiceContext, creature, (Decimal) creature.CurrentHp - newMaxHp, isFromCard ? ValueProp.Unblockable | ValueProp.Unpowered | ValueProp.Move : ValueProp.Unblockable | ValueProp.Unpowered, (CardModel) null, (CardPlay) null);
    }
    Decimal num = await CreatureCmd.SetMaxHp(creature, Math.Max(1.0M, newMaxHp));
  }

  public static async Task<Decimal> SetMaxHp(Creature creature, Decimal amount)
  {
    int oldMaxHp = creature.MaxHp;
    creature.SetMaxHpInternal(Math.Max(0M, amount));
    int newMaxHp = creature.MaxHp;
    if (creature.MaxHp <= 0)
      await CreatureCmd.Kill(creature);
    return (Decimal) (newMaxHp - oldMaxHp);
  }

  public static async Task SetMaxAndCurrentHp(Creature creature, Decimal amount)
  {
    Decimal num = await CreatureCmd.SetMaxHp(creature, amount);
    await CreatureCmd.SetCurrentHp(creature, amount);
  }

  public static async Task Stun(Creature creature, string? nextMoveId = null)
  {
    await CreatureCmd.Stun(creature, (Func<IReadOnlyList<Creature>, Task>) (_ => Task.CompletedTask), nextMoveId);
  }

  public static Task Stun(
    Creature creature,
    Func<IReadOnlyList<Creature>, Task> stunMove,
    string? nextMoveId = null)
  {
    creature.StunInternal(new Func<IReadOnlyList<Creature>, Task>(Wrapper), nextMoveId);
    return Task.CompletedTask;

    async Task Wrapper(IReadOnlyList<Creature> c)
    {
      NStunnedVfx vfx = NStunnedVfx.Create(creature);
      if (vfx != null)
      {
        Node vfxContainer = (Node) creature.GetVfxContainer();
        if (vfxContainer != null)
        {
          Callable callable = Callable.From((Action) (() => vfxContainer.AddChildSafely((Node) vfx)));
          ((Callable) ref callable).CallDeferred(Array.Empty<Variant>());
        }
      }
      await stunMove(c);
    }
  }

  public static async Task TriggerAnim(Creature creature, string triggerName, float waitTime)
  {
    NCreature creatureNode = creature.GetCreatureNode();
    if (creatureNode == null)
    {
      if (TestMode.IsOn || !CombatManager.Instance.IsInProgress)
        return;
      Log.Error($"Attempted to play animation on creature {creature} but its creature node doesn't exist!");
    }
    else
    {
      if (creature.IsPlayer)
      {
        CharacterModel character = creature.Player.Character;
        if (creature.IsDead)
          return;
        switch (triggerName)
        {
          case "Attack":
            SfxCmd.Play(character.AttackSfx);
            break;
          case "Cast":
            SfxCmd.Play(character.CastSfx);
            break;
          case "PowerUp":
            SfxCmd.Play(character.PowerUpSfx);
            break;
        }
      }
      creatureNode.SetAnimationTrigger(triggerName);
      await Cmd.CustomScaledWait(Mathf.Min(waitTime * 0.5f, 0.25f), waitTime);
    }
  }
}
