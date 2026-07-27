// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Commands.PowerCmd
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Hooks;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Commands;

public static class PowerCmd
{
  public static async Task<IReadOnlyList<T>> Apply<T>(
    PlayerChoiceContext choiceContext,
    IEnumerable<Creature>? targets,
    Decimal amount,
    Creature? applier,
    CardModel? cardSource,
    bool silent = false)
    where T : PowerModel
  {
    List<T> powers = new List<T>();
    if (targets == null)
      return (IReadOnlyList<T>) powers;
    foreach (Creature target in targets)
    {
      T obj = await PowerCmd.Apply<T>(choiceContext, target, amount, applier, cardSource, silent);
      if ((object) obj != null)
        powers.Add(obj);
    }
    return (IReadOnlyList<T>) powers;
  }

  public static async Task<T?> Apply<T>(
    PlayerChoiceContext choiceContext,
    Creature target,
    Decimal amount,
    Creature? applier,
    CardModel? cardSource,
    bool silent = false)
    where T : PowerModel
  {
    if (CombatManager.Instance.IsEnding)
      return default (T);
    if (!target.CanReceivePowers)
      return default (T);
    PowerModel basePower = (PowerModel) ModelDb.Power<T>();
    PowerModel power = PowerCmd.FindExistingInstanceForStacking(basePower, target, applier);
    if (power == null)
    {
      power = basePower.ToMutable();
      await PowerCmd.Apply(choiceContext, power, target, amount, applier, cardSource, silent);
    }
    else if (await PowerCmd.ModifyAmount(choiceContext, power, amount, applier, cardSource, silent) == 0)
      power = (PowerModel) null;
    return power as T;
  }

  public static async Task Apply(
    PlayerChoiceContext choiceContext,
    PowerModel power,
    Creature target,
    Decimal amount,
    Creature? applier,
    CardModel? cardSource,
    bool silent = false)
  {
    ICombatState combatState;
    if (CombatManager.Instance.IsEnding)
      combatState = (ICombatState) null;
    else if (amount == 0M)
      combatState = (ICombatState) null;
    else if (!target.CanReceivePowers)
    {
      combatState = (ICombatState) null;
    }
    else
    {
      combatState = target.CombatState;
      if (combatState == null)
      {
        combatState = (ICombatState) null;
      }
      else
      {
        PowerModel instanceForStacking = PowerCmd.FindExistingInstanceForStacking(power, target, applier);
        if (instanceForStacking != null)
        {
          int num = await PowerCmd.ModifyAmount(choiceContext, instanceForStacking, amount, applier, cardSource);
          combatState = (ICombatState) null;
        }
        else
        {
          power.AssertMutable();
          power.Applier = applier;
          await Hook.BeforePowerAmountChanged(combatState, power, amount, target, applier, cardSource);
          Decimal modifiedAmount = amount;
          IEnumerable<AbstractModel> givenModifiers = (IEnumerable<AbstractModel>) null;
          if (applier != null && combatState.ContainsCreature(applier))
            modifiedAmount = Hook.ModifyPowerAmountGiven(combatState, power, applier, modifiedAmount, target, cardSource, out givenModifiers);
          IEnumerable<AbstractModel> receivedModifiers;
          modifiedAmount = Hook.ModifyPowerAmountReceived(combatState, power, target, modifiedAmount, applier, out receivedModifiers);
          if (combatState.Players.Count > 1 && (target.IsPrimaryEnemy || target.IsSecondaryEnemy) && power.ShouldScaleInMultiplayer)
            modifiedAmount = power.GetScaledAmountForMultiplayer(combatState, applier, modifiedAmount, target, cardSource);
          await power.BeforeApplied(target, modifiedAmount, applier, cardSource);
          if (!target.CanReceivePowers)
          {
            combatState = (ICombatState) null;
          }
          else
          {
            power.ApplyInternal(target, modifiedAmount, silent);
            if (modifiedAmount != 0M)
              CombatManager.Instance.History.PowerReceived(combatState, power, modifiedAmount, applier);
            if (power.IsVisible && CombatManager.Instance.IsInProgress)
              await Cmd.CustomScaledWait(0.1f, 0.25f);
            if (target.Side == CombatSide.Player && power.Type == PowerType.Debuff)
              power.SkipNextDurationTick = true;
            if (givenModifiers != null)
              await Hook.AfterModifyingPowerAmountGiven(combatState, givenModifiers, power);
            await Hook.AfterModifyingPowerAmountReceived(combatState, receivedModifiers, power);
            if (modifiedAmount != 0M)
            {
              await power.AfterApplied(applier, cardSource);
              await Hook.AfterPowerAmountChanged(combatState, choiceContext, power, modifiedAmount, applier, cardSource);
            }
            givenModifiers = (IEnumerable<AbstractModel>) null;
            receivedModifiers = (IEnumerable<AbstractModel>) null;
            combatState = (ICombatState) null;
          }
        }
      }
    }
  }

  public static PowerModel? FindExistingInstanceForStacking(
    PowerModel basePower,
    Creature target,
    Creature? applier)
  {
    switch (basePower.InstanceType)
    {
      case PowerInstanceType.None:
        return target.GetPower(basePower.Id);
      case PowerInstanceType.Instanced:
        return (PowerModel) null;
      case PowerInstanceType.InstancedPerApplier:
        return target.GetPowerInstances(basePower.Id).FirstOrDefault<PowerModel>((Func<PowerModel, bool>) (p => p.Applier == applier));
      default:
        throw new ArgumentOutOfRangeException("InstanceType");
    }
  }

  public static async Task Decrement(PowerModel power)
  {
    int num = await PowerCmd.ModifyAmount((PlayerChoiceContext) new ThrowingPlayerChoiceContext(), power, -1M, (Creature) null, (CardModel) null);
  }

  public static async Task TickDownDuration(PowerModel power)
  {
    if (power.SkipNextDurationTick)
      power.SkipNextDurationTick = false;
    else
      await PowerCmd.Decrement(power);
  }

  public static async Task<int> ModifyAmount(
    PlayerChoiceContext choiceContext,
    PowerModel power,
    Decimal offset,
    Creature? applier,
    CardModel? cardSource,
    bool silent = false)
  {
    if (CombatManager.Instance.IsEnding)
      return 0;
    Creature owner = power.Owner;
    ICombatState combatState = owner.CombatState;
    if (combatState == null)
      return 0;
    await Hook.BeforePowerAmountChanged(combatState, power, offset, owner, applier, cardSource);
    Decimal modifiedOffset = offset;
    IEnumerable<AbstractModel> modifiers = (IEnumerable<AbstractModel>) null;
    if (applier != null && combatState.ContainsCreature(applier))
      modifiedOffset = Hook.ModifyPowerAmountGiven(combatState, power, applier, modifiedOffset, owner, cardSource, out modifiers);
    IEnumerable<AbstractModel> receivedModifiers;
    modifiedOffset = Hook.ModifyPowerAmountReceived(combatState, power, owner, modifiedOffset, applier, out receivedModifiers);
    CombatManager.Instance.History.PowerReceived(combatState, power, modifiedOffset, applier);
    int newAmount = power.Amount + (int) modifiedOffset;
    power.SetAmount(newAmount, silent);
    if (modifiers != null)
      await Hook.AfterModifyingPowerAmountGiven(combatState, modifiers, power);
    await Hook.AfterModifyingPowerAmountReceived(combatState, receivedModifiers, power);
    if ((int) modifiedOffset != 0)
      await Hook.AfterPowerAmountChanged(combatState, choiceContext, power, modifiedOffset, applier, cardSource);
    if (power.ShouldRemoveDueToAmount())
      await PowerCmd.Remove(power);
    if (CombatManager.Instance.IsInProgress && owner != null && owner.IsMonster && owner.IsAlive)
    {
      NCreature creatureNode = NCombatRoom.Instance?.GetCreatureNode(owner);
      if (creatureNode != null)
      {
        try
        {
          await creatureNode.UpdateIntent((IEnumerable<Creature>) combatState.Allies);
        }
        catch (ObjectDisposedException ex)
        {
          Log.Error(ex.ToString());
        }
      }
    }
    if (power.IsVisible && CombatManager.Instance.IsInProgress)
      await Cmd.CustomScaledWait(0.1f, 0.25f);
    return newAmount;
  }

  public static async Task Remove<T>(Creature creature) where T : PowerModel
  {
    await PowerCmd.Remove((PowerModel) creature.GetPower<T>());
  }

  public static async Task Remove(PowerModel? power)
  {
    if (power == null)
      return;
    power.RemoveInternal();
    await Cmd.CustomScaledWait(0.2f, 0.4f);
    await power.AfterRemoved(power.Owner);
  }
}
