// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Commands.OrbCmd
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Orbs;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Hooks;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Commands;

public static class OrbCmd
{
  public static Task AddSlots(Player player, int amount)
  {
    if (CombatManager.Instance.IsOverOrEnding)
      return Task.CompletedTask;
    amount = Math.Min(10 - player.PlayerCombatState.OrbQueue.Capacity, amount);
    player.PlayerCombatState.OrbQueue.AddCapacity(amount);
    NCombatRoom.Instance?.GetCreatureNode(player.Creature).OrbManager?.AddSlotAnim(amount);
    return Task.CompletedTask;
  }

  public static void RemoveSlots(Player player, int amount)
  {
    if (CombatManager.Instance.IsOverOrEnding)
      return;
    amount = Math.Min(player.PlayerCombatState.OrbQueue.Capacity, amount);
    player.PlayerCombatState.OrbQueue.RemoveCapacity(amount);
    NCombatRoom.Instance?.GetCreatureNode(player.Creature).OrbManager?.RemoveSlotAnim(amount);
  }

  public static async Task Channel<T>(PlayerChoiceContext choiceContext, Player player) where T : OrbModel
  {
    await OrbCmd.Channel(choiceContext, ModelDb.Orb<T>().ToMutable(0), player);
  }

  public static async Task Channel(PlayerChoiceContext choiceContext, OrbModel orb, Player player)
  {
    ICombatState combatState;
    OrbQueue orbQueue;
    if (CombatManager.Instance.IsOverOrEnding)
    {
      combatState = (ICombatState) null;
      orbQueue = (OrbQueue) null;
    }
    else
    {
      combatState = player.Creature.CombatState;
      orbQueue = player.PlayerCombatState.OrbQueue;
      if (player.Character.BaseOrbSlotCount == 0 && orbQueue.Capacity == 0)
        await OrbCmd.AddSlots(player, 1);
      orb.AssertMutable();
      orb.Owner = player;
      if (orbQueue.Orbs.Count >= orbQueue.Capacity)
        await OrbCmd.EvokeNext(choiceContext, player);
      if (!await player.PlayerCombatState.OrbQueue.TryEnqueue(orb))
      {
        combatState = (ICombatState) null;
        orbQueue = (OrbQueue) null;
      }
      else
      {
        CombatManager.Instance.History.OrbChanneled(combatState, orb);
        orb.PlayChannelSfx();
        NCombatRoom.Instance?.GetCreatureNode(player.Creature)?.OrbManager?.AddOrbAnim();
        await Hook.AfterOrbChanneled(combatState, choiceContext, player, orb);
        combatState = (ICombatState) null;
        orbQueue = (OrbQueue) null;
      }
    }
  }

  public static async Task EvokeNext(
    PlayerChoiceContext choiceContext,
    Player player,
    bool dequeue = true)
  {
    OrbQueue orbQueue = player.PlayerCombatState.OrbQueue;
    OrbModel orb;
    if (orbQueue.Orbs.Count <= 0)
    {
      orb = (OrbModel) null;
    }
    else
    {
      orb = orbQueue.Orbs.First<OrbModel>();
      choiceContext.PushModel((AbstractModel) orb);
      await OrbCmd.Evoke(choiceContext, player, orb, dequeue);
      choiceContext.PopModel((AbstractModel) orb);
      orb = (OrbModel) null;
    }
  }

  public static async Task EvokeLast(
    PlayerChoiceContext choiceContext,
    Player player,
    bool dequeue = true)
  {
    OrbQueue orbQueue = player.PlayerCombatState.OrbQueue;
    OrbModel orb;
    if (orbQueue.Orbs.Count <= 0)
    {
      orb = (OrbModel) null;
    }
    else
    {
      orb = orbQueue.Orbs.Last<OrbModel>();
      choiceContext.PushModel((AbstractModel) orb);
      await OrbCmd.Evoke(choiceContext, player, orb, dequeue);
      choiceContext.PopModel((AbstractModel) orb);
      orb = (OrbModel) null;
    }
  }

  private static async Task Evoke(
    PlayerChoiceContext choiceContext,
    Player player,
    OrbModel evokedOrb,
    bool dequeue = true)
  {
    if (CombatManager.Instance.IsOverOrEnding)
      return;
    OrbQueue orbQueue = player.PlayerCombatState.OrbQueue;
    if (orbQueue.Orbs.Count <= 0)
      return;
    bool removed = false;
    if (dequeue)
    {
      removed = orbQueue.Remove(evokedOrb);
      NCombatRoom.Instance?.GetCreatureNode(player.Creature)?.OrbManager?.EvokeOrbAnim(evokedOrb);
    }
    choiceContext.PushModel((AbstractModel) evokedOrb);
    IEnumerable<Creature> targets = await evokedOrb.Evoke(choiceContext);
    choiceContext.PopModel((AbstractModel) evokedOrb);
    if (player.Creature.CombatState == null)
      return;
    await Hook.AfterOrbEvoked(choiceContext, player.Creature.CombatState, evokedOrb, targets);
    if (!removed)
      return;
    evokedOrb.RemoveInternal();
  }

  public static async Task Passive(
    PlayerChoiceContext choiceContext,
    OrbModel orb,
    Creature? target,
    bool countAffectedByHooks = false)
  {
    if (CombatManager.Instance.IsOverOrEnding)
      return;
    choiceContext.PushModel((AbstractModel) orb);
    if (countAffectedByHooks)
      await orb.TriggerPassive(choiceContext, target);
    else
      await orb.Passive(choiceContext, target);
    choiceContext.PopModel((AbstractModel) orb);
  }
}
