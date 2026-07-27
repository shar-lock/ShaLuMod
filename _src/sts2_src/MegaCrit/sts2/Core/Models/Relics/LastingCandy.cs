// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Relics.LastingCandy
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Models.Characters;
using MegaCrit.Sts2.Core.Rewards;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.Saves.Runs;
using MegaCrit.Sts2.Core.Unlocks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Relics;

public sealed class LastingCandy : RelicModel
{
  private bool _isActivating;
  private int _combatRewardsSeen;

  public override RelicRarity Rarity => RelicRarity.Uncommon;

  public override bool IsAllowed(IRunState runState)
  {
    return !runState.Players.Any<Player>((Func<Player, bool>) (p =>
    {
      if (p != null && p.Character is Ironclad)
      {
        UnlockState unlockState = p.UnlockState;
        if (unlockState != null)
          return unlockState.NumberOfRuns == 0;
      }
      return false;
    })) && RelicModel.IsBeforeAct3TreasureChest(runState);
  }

  public override bool ShowCounter => true;

  public override int DisplayAmount => !this.IsActivating ? this.CombatRewardsSeen % 2 : 2;

  private bool IsActivating
  {
    get => this._isActivating;
    set
    {
      this.AssertMutable();
      this._isActivating = value;
      this.InvokeDisplayAmountChanged();
    }
  }

  [SavedProperty]
  public int CombatRewardsSeen
  {
    get => this._combatRewardsSeen;
    set
    {
      this.AssertMutable();
      this._combatRewardsSeen = value;
    }
  }

  public override bool TryModifyCardRewardOptions(
    Player player,
    List<CardCreationResult> rewardOptions,
    CardCreationOptions creationOptions)
  {
    if (this.Owner != player || creationOptions.Source != CardCreationSource.Encounter || !this.IsInTriggeringCombat || !creationOptions.Flags.HasFlag((Enum) CardCreationFlags.IsCardReward) || !creationOptions.Flags.HasFlag((Enum) CardCreationFlags.IsFromCombat))
      return false;
    bool allowDupes = false;
    List<CardModel> list = creationOptions.GetPossibleCards(player).ToList<CardModel>();
    IEnumerable<CardModel> source = list.Where<CardModel>((Func<CardModel, bool>) (c => LastingCandy.CardPoolFilter(c, rewardOptions, false)));
    if (!source.Any<CardModel>())
    {
      allowDupes = true;
      source = list.Where<CardModel>((Func<CardModel, bool>) (c => LastingCandy.CardPoolFilter(c, rewardOptions, true)));
    }
    if (!source.Any<CardModel>())
      return false;
    CardModel card = CardFactory.CreateForReward(this.Owner, 1, new CardCreationOptions((IEnumerable<CardPoolModel>) creationOptions.CardPools, CardCreationSource.Other, creationOptions.RarityOdds, (Func<CardModel, bool>) (c =>
    {
      Func<CardModel, bool> cardPoolFilter = creationOptions.CardPoolFilter;
      return (cardPoolFilter != null ? (cardPoolFilter(c) ? 1 : 0) : 1) != 0 && LastingCandy.CardPoolFilter(c, rewardOptions, allowDupes);
    })).WithFlags(CardCreationFlags.NoModifyHooks | CardCreationFlags.NoCardPoolModifications)).FirstOrDefault<CardCreationResult>()?.Card;
    if (card != null)
    {
      CardCreationResult cardCreationResult = new CardCreationResult(card);
      cardCreationResult.ModifyCard(card, (RelicModel) this);
      rewardOptions.Add(cardCreationResult);
    }
    return card != null;
  }

  public override Task BeforeCombatRewardOffered(RewardsSet rewards, CombatRoom room)
  {
    if (rewards.Player != this.Owner || rewards.Rewards.All<Reward>((Func<Reward, bool>) (r => !(r is CardReward))))
      return Task.CompletedTask;
    if (this.IsInTriggeringCombat)
      TaskHelper.RunSafely(this.DoActivateVisuals());
    ++this.CombatRewardsSeen;
    this.InvokeDisplayAmountChanged();
    return Task.CompletedTask;
  }

  private async Task DoActivateVisuals()
  {
    this.IsActivating = true;
    this.Flash();
    await Cmd.Wait(1f);
    this.IsActivating = false;
  }

  private bool IsInTriggeringCombat
  {
    get => this.CombatRewardsSeen > 0 && this.CombatRewardsSeen % 2 == 1;
  }

  private static bool CardPoolFilter(
    CardModel card,
    List<CardCreationResult> rewardOptions,
    bool allowDupes)
  {
    if (card.Type != CardType.Power)
      return false;
    return allowDupes || rewardOptions.TrueForAll((Predicate<CardCreationResult>) (o => o.originalCard.Id != card.Id));
  }
}
