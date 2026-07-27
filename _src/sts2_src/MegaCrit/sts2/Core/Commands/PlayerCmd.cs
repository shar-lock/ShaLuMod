// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Commands.PlayerCmd
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Gold;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.RestSite;
using MegaCrit.Sts2.Core.Hooks;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Runs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Commands;

public static class PlayerCmd
{
  public const string goldSmallSfx = "event:/sfx/ui/gold/gold_1";
  public const string goldMediumSfx = "event:/sfx/ui/gold/gold_2";
  public const string goldLargeSfx = "event:/sfx/ui/gold/gold_3";

  public static async Task GainEnergy(Decimal amount, Player player)
  {
    if (amount <= 0M || CombatManager.Instance.IsEnding)
      return;
    ICombatState combatState = player.Creature.CombatState;
    IEnumerable<AbstractModel> modifiers;
    Decimal finalAmount = Hook.ModifyEnergyGain(combatState, player, amount, out modifiers);
    await Hook.AfterModifyingEnergyGain(combatState, modifiers);
    if (!(finalAmount > 0M))
      return;
    SfxCmd.Play("event:/sfx/ui/gain_energy");
    player.PlayerCombatState.GainEnergy(finalAmount);
  }

  public static Task LoseEnergy(Decimal amount, Player player)
  {
    if (amount <= 0M || CombatManager.Instance.IsEnding)
      return Task.CompletedTask;
    player.PlayerCombatState.LoseEnergy(amount);
    return Task.CompletedTask;
  }

  public static async Task SetEnergy(Decimal amount, Player player)
  {
    if (CombatManager.Instance.IsEnding)
      return;
    int energy = player.PlayerCombatState.Energy;
    if ((Decimal) energy < amount)
    {
      await PlayerCmd.GainEnergy(amount - (Decimal) energy, player);
    }
    else
    {
      if (!((Decimal) energy > amount))
        return;
      await PlayerCmd.LoseEnergy((Decimal) energy - amount, player);
    }
  }

  public static async Task GainStars(Decimal amount, Player player)
  {
    if (CombatManager.Instance.IsEnding || !Hook.ShouldGainStars(player.Creature.CombatState, amount, player))
      return;
    player.PlayerCombatState.GainStars(amount);
    await Hook.AfterStarsGained(player.Creature.CombatState, (int) amount, player);
  }

  public static Task LoseStars(Decimal amount, Player player)
  {
    if (CombatManager.Instance.IsEnding)
      return Task.CompletedTask;
    player.PlayerCombatState.LoseStars(amount);
    return Task.CompletedTask;
  }

  public static async Task SetStars(Decimal amount, Player player)
  {
    if (CombatManager.Instance.IsEnding)
      return;
    int stars = player.PlayerCombatState.Stars;
    if ((Decimal) stars < amount)
    {
      await PlayerCmd.GainStars(amount - (Decimal) stars, player);
    }
    else
    {
      if (!((Decimal) stars > amount))
        return;
      await PlayerCmd.LoseStars((Decimal) stars - amount, player);
    }
  }

  public static async Task GainGold(Decimal amount, Player player, bool wasStolenBack = false)
  {
    IRunState runState = player.RunState;
    IEnumerable<AbstractModel> modifiers;
    amount = Hook.ModifyGoldGained(runState, player.Creature.CombatState, amount, player, out modifiers);
    await Hook.AfterModifyingGoldGained(runState, player.Creature.CombatState, modifiers, player, amount);
    if (!(amount > 0M))
    {
      runState = (IRunState) null;
    }
    else
    {
      if (player == LocalContext.GetMe((IPlayerCollection) runState))
        SfxCmd.Play(amount >= 100M ? "event:/sfx/ui/gold/gold_3" : (amount > 30M ? "event:/sfx/ui/gold/gold_2" : "event:/sfx/ui/gold/gold_1"));
      PlayerMapPointHistoryEntry entry = runState.CurrentMapPointHistoryEntry?.GetEntry(player.NetId);
      if (entry != null)
      {
        if (wasStolenBack)
          entry.GoldStolen -= (int) amount;
        else
          entry.GoldGained += (int) amount;
      }
      player.Gold += (int) amount;
      await Hook.AfterGoldGained(runState, player);
      runState = (IRunState) null;
    }
  }

  public static Task LoseGold(Decimal amount, Player player, GoldLossType goldLossType = GoldLossType.Lost)
  {
    SfxCmd.Play("event:/sfx/ui/gold/gold_1");
    PlayerMapPointHistoryEntry entry = player.RunState.CurrentMapPointHistoryEntry?.GetEntry(player.NetId);
    if (entry != null)
    {
      switch (goldLossType)
      {
        case GoldLossType.Spent:
          entry.GoldSpent += (int) amount;
          break;
        case GoldLossType.Lost:
          entry.GoldLost += (int) amount;
          break;
        case GoldLossType.Stolen:
          entry.GoldStolen += (int) amount;
          entry.MarkLootStolen((int) amount);
          break;
      }
    }
    player.Gold = int.Max(0, player.Gold - (int) amount);
    return Task.CompletedTask;
  }

  public static async Task SetGold(Decimal amount, Player player)
  {
    int gold = player.Gold;
    if ((Decimal) gold < amount)
    {
      await PlayerCmd.GainGold(amount - (Decimal) gold, player);
    }
    else
    {
      if (!((Decimal) gold > amount))
        return;
      await PlayerCmd.LoseGold((Decimal) gold - amount, player);
    }
  }

  public static Task GainMaxPotionCount(int amount, Player player)
  {
    player.AddToMaxPotionCount(amount);
    return Task.CompletedTask;
  }

  public static Task LoseMaxPotionCount(int amount, Player player)
  {
    player.SubtractFromMaxPotionCount(amount);
    return Task.CompletedTask;
  }

  public static async Task<Creature> AddPet<T>(Player player) where T : MonsterModel
  {
    Creature pet = player.Creature.CombatState.CreateCreature(ModelDb.Monster<T>().ToMutable(), player.Creature.Side, (string) null);
    await PlayerCmd.AddPet(pet, player);
    Creature creature = pet;
    pet = (Creature) null;
    return creature;
  }

  public static async Task AddPet(Creature pet, Player player)
  {
    if (pet.CombatState == null)
      throw new InvalidOperationException("Pet must already be added to a combat state.");
    player.PlayerCombatState.AddPetInternal(pet);
    await CreatureCmd.Add(pet);
  }

  public static async Task MimicRestSiteHeal(Player player, bool playSfx = true)
  {
    if (playSfx)
      HealRestSiteOption.PlayRestSiteHealSfx();
    await HealRestSiteOption.ExecuteRestSiteHeal(player, true);
  }

  public static void EndTurn(Player player, bool canBackOut, Func<Task>? actionDuringEnemyTurn = null)
  {
    if (CombatManager.Instance.IsPlayerReadyToEndTurn(player))
      return;
    if (LocalContext.IsMe(player))
      CombatManager.Instance.OnEndedTurnLocally();
    CombatManager.Instance.SetReadyToEndTurn(player, canBackOut, actionDuringEnemyTurn);
  }

  public static void CompleteQuest(CardModel questCard)
  {
    questCard.Owner.RunState.CurrentMapPointHistoryEntry?.GetEntry(questCard.Owner.NetId).CompletedQuests.Add(questCard.Id);
  }
}
