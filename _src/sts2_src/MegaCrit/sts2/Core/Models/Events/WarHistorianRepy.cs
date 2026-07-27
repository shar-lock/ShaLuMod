// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Events.WarHistorianRepy
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Events;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.Relics;
using MegaCrit.Sts2.Core.Rewards;
using MegaCrit.Sts2.Core.Runs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Events;

public sealed class WarHistorianRepy : EventModel
{
  public override bool IsAllowed(IRunState runState) => false;

  public override bool IsShared => true;

  protected override IReadOnlyList<EventOption> GenerateInitialOptions()
  {
    // ISSUE: object of a compiler-generated type is created
    return (IReadOnlyList<EventOption>) new \u003C\u003Ez__ReadOnlyArray<EventOption>(new EventOption[2]
    {
      new EventOption((EventModel) this, new Func<Task>(this.InitialUnlockCage), "WAR_HISTORIAN_REPY.pages.INITIAL.options.UNLOCK_CAGE", HoverTipFactory.FromRelic<HistoryCourse>().Concat<IHoverTip>(HoverTipFactory.FromCardWithCardHoverTips<LanternKey>())),
      new EventOption((EventModel) this, new Func<Task>(this.InitialUnlockChest), "WAR_HISTORIAN_REPY.pages.INITIAL.options.UNLOCK_CHEST", HoverTipFactory.FromCardWithCardHoverTips<LanternKey>())
    });
  }

  private bool ShouldGetSecondReward
  {
    get
    {
      return this.Owner.Deck.Cards.Any<CardModel>((Func<CardModel, bool>) (c => c is LanternKey)) && this.Owner.RunState.Players.Count <= 1;
    }
  }

  private async Task InitialUnlockCage()
  {
    await this.RemoveLanternKeysForInitialChoice();
    await this.UnlockCage();
    if (this.ShouldGetSecondReward)
    {
      // ISSUE: object of a compiler-generated type is created
      this.SetEventState(this.L10NLookup("WAR_HISTORIAN_REPY.pages.UNLOCK_CAGE.description"), (IEnumerable<EventOption>) new \u003C\u003Ez__ReadOnlySingleElementList<EventOption>(new EventOption((EventModel) this, new Func<Task>(this.SecondUnlockChest), "WAR_HISTORIAN_REPY.pages.INITIAL.options.UNLOCK_CHEST", HoverTipFactory.FromCardWithCardHoverTips<LanternKey>())));
    }
    else
      this.SetEventFinished(this.L10NLookup("WAR_HISTORIAN_REPY.pages.UNLOCK_CAGE.description"));
  }

  private async Task InitialUnlockChest()
  {
    await this.RemoveLanternKeysForInitialChoice();
    await this.UnlockChest();
    if (this.ShouldGetSecondReward)
    {
      // ISSUE: object of a compiler-generated type is created
      this.SetEventState(this.L10NLookup("WAR_HISTORIAN_REPY.pages.UNLOCK_CHEST.description"), (IEnumerable<EventOption>) new \u003C\u003Ez__ReadOnlySingleElementList<EventOption>(new EventOption((EventModel) this, new Func<Task>(this.SecondUnlockCage), "WAR_HISTORIAN_REPY.pages.INITIAL.options.UNLOCK_CAGE", HoverTipFactory.FromRelic<HistoryCourse>().Concat<IHoverTip>(HoverTipFactory.FromCardWithCardHoverTips<LanternKey>()))));
    }
    else
      this.SetEventFinished(this.L10NLookup("WAR_HISTORIAN_REPY.pages.UNLOCK_CHEST.description"));
  }

  private async Task SecondUnlockCage()
  {
    this.SetEventFinished(this.L10NLookup("WAR_HISTORIAN_REPY.pages.EXTRA_UNLOCK_CAGE.description"));
    await this.RemoveLanternKeysForSecondChoice();
    await this.UnlockCage();
  }

  private async Task SecondUnlockChest()
  {
    this.SetEventFinished(this.L10NLookup("WAR_HISTORIAN_REPY.pages.EXTRA_UNLOCK_CHEST.description"));
    await this.RemoveLanternKeysForSecondChoice();
    await this.UnlockChest();
  }

  private async Task UnlockChest()
  {
    await RewardsCmd.OfferCustom(this.Owner, new List<Reward>()
    {
      (Reward) new PotionReward(this.Owner),
      (Reward) new PotionReward(this.Owner),
      (Reward) new RelicReward(this.Owner),
      (Reward) new RelicReward(this.Owner)
    });
  }

  private async Task UnlockCage()
  {
    this.Owner.RunState.ExtraFields.FreedRepy = true;
    HistoryCourse historyCourse = await RelicCmd.Obtain<HistoryCourse>(this.Owner);
  }

  private async Task RemoveLanternKeysForInitialChoice()
  {
    if (this.Owner.RunState.Players.Count > 1)
      await this.RemoveLanternKeysForSecondChoice();
    else
      await this.RemoveFirstLanternKey();
  }

  private async Task RemoveFirstLanternKey()
  {
    CardModel cardModel = this.Owner.Deck.Cards.FirstOrDefault<CardModel>((Func<CardModel, bool>) (c => c is LanternKey));
    if (cardModel == null)
      return;
    PlayerCmd.CompleteQuest(cardModel);
    await CardPileCmd.RemoveFromDeck(cardModel);
  }

  private async Task RemoveLanternKeysForSecondChoice()
  {
    foreach (CardModel cardModel in this.Owner.Deck.Cards.Where<CardModel>((Func<CardModel, bool>) (c => c is LanternKey)).ToList<CardModel>())
    {
      PlayerCmd.CompleteQuest(cardModel);
      await CardPileCmd.RemoveFromDeck(cardModel);
    }
  }
}
