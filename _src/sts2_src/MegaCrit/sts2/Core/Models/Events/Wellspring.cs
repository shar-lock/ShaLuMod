// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Events.Wellspring
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Events;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.PotionPools;
using MegaCrit.Sts2.Core.Rewards;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Events;

public sealed class Wellspring : EventModel
{
  private const string _batheKey = "BatheCurses";

  protected override IEnumerable<DynamicVar> CanonicalVars
  {
    get
    {
      return (IEnumerable<DynamicVar>) new \u003C\u003Ez__ReadOnlySingleElementList<DynamicVar>(new DynamicVar("BatheCurses", 1M));
    }
  }

  protected override IReadOnlyList<EventOption> GenerateInitialOptions()
  {
    // ISSUE: object of a compiler-generated type is created
    return (IReadOnlyList<EventOption>) new \u003C\u003Ez__ReadOnlyArray<EventOption>(new EventOption[2]
    {
      new EventOption((EventModel) this, new Func<Task>(this.Bottle), "WELLSPRING.pages.INITIAL.options.BOTTLE", Array.Empty<IHoverTip>()),
      new EventOption((EventModel) this, new Func<Task>(this.Bathe), "WELLSPRING.pages.INITIAL.options.BATHE", HoverTipFactory.FromCardWithCardHoverTips<Guilty>())
    });
  }

  private async Task Bottle()
  {
    PotionModel potionModel = this.Owner.PlayerRng.Rewards.NextItem<PotionModel>(this.Owner.Character.PotionPool.GetUnlockedPotions(this.Owner.UnlockState).Concat<PotionModel>(ModelDb.PotionPool<SharedPotionPool>().GetUnlockedPotions(this.Owner.UnlockState)));
    if (potionModel != null)
      await RewardsCmd.OfferCustom(this.Owner, new List<Reward>(1)
      {
        (Reward) new PotionReward(potionModel.ToMutable(), this.Owner)
      });
    this.SetEventFinished(this.L10NLookup("WELLSPRING.pages.BOTTLE.description"));
  }

  private async Task Bathe()
  {
    await CardPileCmd.RemoveFromDeck((IReadOnlyList<CardModel>) (await CardSelectCmd.FromDeckForRemoval(this.Owner, new CardSelectorPrefs(CardSelectorPrefs.RemoveSelectionPrompt, 1))).ToList<CardModel>());
    await this.AddGuilty(this.DynamicVars["BatheCurses"].IntValue);
    this.SetEventFinished(this.L10NLookup("WELLSPRING.pages.BATHE.description"));
  }

  private async Task AddGuilty(int amount)
  {
    IEnumerable<CardPileAddResult> deck = await CardPileCmd.AddCursesToDeck((IEnumerable<CardModel>) Enumerable.Repeat<Guilty>(ModelDb.Card<Guilty>(), amount), this.Owner);
  }
}
