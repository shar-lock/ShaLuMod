// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Events.WelcomeToWongos
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Gold;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Events;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Relics;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.Saves;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Events;

public sealed class WelcomeToWongos : EventModel
{
  private const int _wongoPointsForBadge = 2000;
  private const string _bargainBinCostKey = "BargainBinCost";
  private const string _featuredItemCostKey = "FeaturedItemCost";
  private const string _mysteryBoxCostKey = "MysteryBoxCost";
  private const string _mysteryBoxRelicCountKey = "MysteryBoxRelicCount";
  private const string _mysteryBoxCombatCountKey = "MysteryBoxCombatCount";
  private const string _wongoPointAmountKey = "WongoPointAmount";
  private const string _remainingWongoPointAmountKey = "RemainingWongoPointAmount";
  private const string _totalWongoBadgeAmountKey = "TotalWongoBadgeAmount";
  private const string _randomRelicKey = "RandomRelic";
  private RelicModel? _featuredItem;

  private RelicModel? FeaturedItem
  {
    get => this._featuredItem;
    set
    {
      this.AssertMutable();
      this._featuredItem = value;
    }
  }

  public override bool IsAllowed(IRunState runState)
  {
    return runState.CurrentActIndex == 1 && runState.Players.All<Player>((Func<Player, bool>) (p => p.Gold >= 100));
  }

  protected override IEnumerable<DynamicVar> CanonicalVars
  {
    get
    {
      return (IEnumerable<DynamicVar>) new \u003C\u003Ez__ReadOnlyArray<DynamicVar>(new DynamicVar[9]
      {
        new DynamicVar("BargainBinCost", 100M),
        new DynamicVar("MysteryBoxCost", 300M),
        new DynamicVar("FeaturedItemCost", 200M),
        new DynamicVar("MysteryBoxRelicCount", 3M),
        new DynamicVar("MysteryBoxCombatCount", 5M),
        new DynamicVar("WongoPointAmount", 0M),
        new DynamicVar("RemainingWongoPointAmount", 0M),
        new DynamicVar("TotalWongoBadgeAmount", 0M),
        (DynamicVar) new StringVar("RandomRelic")
      });
    }
  }

  protected override IReadOnlyList<EventOption> GenerateInitialOptions()
  {
    Player owner = this.Owner;
    this.FeaturedItem = RelicFactory.PullNextRelicFromFront(owner, RelicRarity.Rare, (Func<RelicModel, bool>) (r => r.IsAllowedInShops));
    ((StringVar) this.DynamicVars["RandomRelic"]).StringValue = this.FeaturedItem.Title.GetFormattedText();
    List<EventOption> initialOptions = new List<EventOption>();
    if ((Decimal) owner.Gold >= this.DynamicVars["BargainBinCost"].BaseValue)
      initialOptions.Add(new EventOption((EventModel) this, new Func<Task>(this.BuyBargainBin), "WELCOME_TO_WONGOS.pages.INITIAL.options.BARGAIN_BIN", Array.Empty<IHoverTip>()));
    else
      initialOptions.Add(new EventOption((EventModel) this, (Func<Task>) null, "WELCOME_TO_WONGOS.pages.INITIAL.options.BARGAIN_BIN_LOCKED", Array.Empty<IHoverTip>()));
    if ((Decimal) owner.Gold >= this.DynamicVars["FeaturedItemCost"].BaseValue)
      initialOptions.Add(new EventOption((EventModel) this, new Func<Task>(this.BuyFeaturedItem), "WELCOME_TO_WONGOS.pages.INITIAL.options.FEATURED_ITEM", this.FeaturedItem.HoverTips));
    else
      initialOptions.Add(new EventOption((EventModel) this, (Func<Task>) null, "WELCOME_TO_WONGOS.pages.INITIAL.options.FEATURED_ITEM_LOCKED", Array.Empty<IHoverTip>()));
    if ((Decimal) owner.Gold >= this.DynamicVars["MysteryBoxCost"].BaseValue)
      initialOptions.Add(new EventOption((EventModel) this, new Func<Task>(this.BuyMysteryBox), "WELCOME_TO_WONGOS.pages.INITIAL.options.MYSTERY_BOX", Array.Empty<IHoverTip>()));
    else
      initialOptions.Add(new EventOption((EventModel) this, (Func<Task>) null, "WELCOME_TO_WONGOS.pages.INITIAL.options.MYSTERY_BOX_LOCKED", Array.Empty<IHoverTip>()));
    initialOptions.Add(new EventOption((EventModel) this, new Func<Task>(this.Leave), "WELCOME_TO_WONGOS.pages.INITIAL.options.LEAVE", Array.Empty<IHoverTip>()));
    return (IReadOnlyList<EventOption>) initialOptions;
  }

  private async Task<LocString> CheckObtainWongoBadge(int pointsEarned)
  {
    int wongoPoints = SaveManager.Instance.Progress.WongoPoints;
    int num1 = wongoPoints % 2000 + pointsEarned;
    int num2 = wongoPoints + pointsEarned;
    this.DynamicVars["WongoPointAmount"].BaseValue = (Decimal) num1;
    this.DynamicVars["RemainingWongoPointAmount"].BaseValue = (Decimal) (2000 - num1);
    this.DynamicVars["TotalWongoBadgeAmount"].BaseValue = (Decimal) (num2 / 2000);
    this.Owner.ExtraFields.WongoPoints = pointsEarned;
    if (num1 < 2000)
      return !(this.DynamicVars["TotalWongoBadgeAmount"].BaseValue > 0M) ? this.L10NLookup("WELCOME_TO_WONGOS.pages.AFTER_BUY.description") : this.L10NLookup("WELCOME_TO_WONGOS.pages.AFTER_BUY_BADGE_COUNTER.description");
    WongoCustomerAppreciationBadge appreciationBadge = await RelicCmd.Obtain<WongoCustomerAppreciationBadge>(this.Owner);
    return this.L10NLookup("WELCOME_TO_WONGOS.pages.AFTER_BUY_RECEIVE_BADGE.description");
  }

  private async Task BuyBargainBin()
  {
    await PlayerCmd.LoseGold(this.DynamicVars["BargainBinCost"].BaseValue, this.Owner, GoldLossType.Spent);
    RelicModel relicModel = await RelicCmd.Obtain(RelicFactory.PullNextRelicFromFront(this.Owner, RelicRarity.Common, (Func<RelicModel, bool>) (r => r.IsAllowedInShops)).ToMutable(), this.Owner);
    this.SetEventFinished(await this.CheckObtainWongoBadge(32 /*0x20*/));
  }

  private async Task BuyMysteryBox()
  {
    await PlayerCmd.LoseGold(this.DynamicVars["MysteryBoxCost"].BaseValue, this.Owner, GoldLossType.Spent);
    WongosMysteryTicket wongosMysteryTicket = await RelicCmd.Obtain<WongosMysteryTicket>(this.Owner);
    this.SetEventFinished(await this.CheckObtainWongoBadge(8));
  }

  private async Task BuyFeaturedItem()
  {
    await PlayerCmd.LoseGold(this.DynamicVars["FeaturedItemCost"].BaseValue, this.Owner, GoldLossType.Spent);
    RelicModel relicModel = await RelicCmd.Obtain(this.FeaturedItem.ToMutable(), this.Owner);
    this.SetEventFinished(await this.CheckObtainWongoBadge(16 /*0x10*/));
  }

  private async Task Leave()
  {
    CardModel card = this.Rng.NextItem<CardModel>(this.Owner.Deck.Cards.Where<CardModel>((Func<CardModel, bool>) (c => c.IsUpgraded)));
    if (card != null)
    {
      CardCmd.Downgrade(card);
      CardCmd.Preview(card);
      await Cmd.CustomScaledWait(0.5f, 1.2f);
    }
    this.SetEventFinished(this.L10NLookup("WELCOME_TO_WONGOS.pages.LEAVE.description"));
  }
}
