// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Relics.SilverCrucible
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.Saves.Runs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Relics;

public sealed class SilverCrucible : RelicModel
{
  private int _timesUsed;
  private int _treasureRoomsEntered;

  public override RelicRarity Rarity => RelicRarity.Ancient;

  public override bool IsUsedUp
  {
    get => this.TimesUsed >= this.DynamicVars.Cards.IntValue && this.TreasureRoomsEntered > 0;
  }

  public override bool ShowCounter
  {
    get => this.DynamicVars.Cards.IntValue > 0 && this.TimesUsed < this.DynamicVars.Cards.IntValue;
  }

  public override int DisplayAmount => this.DynamicVars.Cards.IntValue - this.TimesUsed;

  protected override IEnumerable<DynamicVar> CanonicalVars
  {
    get
    {
      return (IEnumerable<DynamicVar>) new \u003C\u003Ez__ReadOnlySingleElementList<DynamicVar>((DynamicVar) new CardsVar(3));
    }
  }

  [SavedProperty]
  public int TimesUsed
  {
    get => this._timesUsed;
    set
    {
      this.AssertMutable();
      this._timesUsed = value;
      this.InvokeDisplayAmountChanged();
      this.CheckIfUsedUp();
    }
  }

  [SavedProperty]
  public int TreasureRoomsEntered
  {
    get => this._treasureRoomsEntered;
    set
    {
      this.AssertMutable();
      this._treasureRoomsEntered = value;
      this.CheckIfUsedUp();
    }
  }

  public override bool IsAllowed(IRunState runState) => runState.Players.Count == 1;

  private void CheckIfUsedUp()
  {
    if (!this.IsUsedUp)
      return;
    this.Status = RelicStatus.Disabled;
  }

  public override bool TryModifyCardRewardOptionsLate(
    Player player,
    List<CardCreationResult> cardRewards,
    CardCreationOptions options)
  {
    if (player != this.Owner || this.TimesUsed >= this.DynamicVars.Cards.IntValue || !options.Flags.HasFlag((Enum) CardCreationFlags.IsCardReward))
      return false;
    foreach (CardCreationResult cardReward in cardRewards)
    {
      CardModel card1 = cardReward.Card;
      if (card1.IsUpgradable)
      {
        CardModel card2 = this.Owner.RunState.CloneCard(card1);
        CardCmd.Upgrade(card2);
        cardReward.ModifyCard(card2, (RelicModel) this);
      }
    }
    return true;
  }

  public override Task AfterModifyingCardRewardOptions()
  {
    if (this.TimesUsed >= this.DynamicVars.Cards.IntValue)
      return Task.CompletedTask;
    ++this.TimesUsed;
    return Task.CompletedTask;
  }

  public override Task AfterRoomEntered(AbstractRoom room)
  {
    if (room is TreasureRoom)
      ++this.TreasureRoomsEntered;
    return Task.CompletedTask;
  }

  public override bool ShouldGenerateTreasure(Player player)
  {
    return player != this.Owner || this.TreasureRoomsEntered > 1;
  }
}
