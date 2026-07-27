// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Events.StoneOfAllTime
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Events;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Enchantments;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.ValueProps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Events;

public sealed class StoneOfAllTime : EventModel
{
  private const string _drinkRandomPotionKey = "DrinkRandomPotion";
  private const string _drinkMaxHpGain = "DrinkMaxHpGain";
  private const string _pushHpLoss = "PushHpLoss";
  private const string _pushVigorousAmountKey = "PushVigorousAmount";
  private PotionModel? _drinkAndLiftPotion;

  private PotionModel? DrinkAndLiftPotion
  {
    get => this._drinkAndLiftPotion;
    set
    {
      this.AssertMutable();
      this._drinkAndLiftPotion = value;
    }
  }

  protected override IEnumerable<DynamicVar> CanonicalVars
  {
    get
    {
      return (IEnumerable<DynamicVar>) new \u003C\u003Ez__ReadOnlyArray<DynamicVar>(new DynamicVar[4]
      {
        (DynamicVar) new StringVar("DrinkRandomPotion"),
        new DynamicVar("DrinkMaxHpGain", 10M),
        new DynamicVar("PushHpLoss", 6M),
        new DynamicVar("PushVigorousAmount", 8M)
      });
    }
  }

  public override bool IsAllowed(IRunState runState)
  {
    return runState.CurrentActIndex == 1 && runState.Players.All<Player>((Func<Player, bool>) (player => player.Potions.Any<PotionModel>()));
  }

  protected override Task BeforeEventStarted(bool isPreFinished)
  {
    this.Owner.CanUseOrRemovePotions = false;
    return Task.CompletedTask;
  }

  protected override void OnEventFinished() => this.Owner.CanUseOrRemovePotions = true;

  protected override IReadOnlyList<EventOption> GenerateInitialOptions()
  {
    this.DrinkAndLiftPotion = this.Rng.NextItem<PotionModel>(this.Owner.Potions);
    EventOption eventOption1;
    if (this.DrinkAndLiftPotion != null)
    {
      ((StringVar) this.DynamicVars["DrinkRandomPotion"]).StringValue = this.DrinkAndLiftPotion.Title.GetFormattedText();
      eventOption1 = new EventOption((EventModel) this, new Func<Task>(this.Lift), "STONE_OF_ALL_TIME.pages.INITIAL.options.LIFT", new IHoverTip[1]
      {
        HoverTipFactory.FromPotion(this.DrinkAndLiftPotion)
      });
    }
    else
      eventOption1 = new EventOption((EventModel) this, (Func<Task>) null, "STONE_OF_ALL_TIME.pages.INITIAL.options.LIFT_LOCKED", Array.Empty<IHoverTip>());
    EventOption eventOption2 = CardPile.Get(PileType.Deck, this.Owner).Cards.Count<CardModel>((Func<CardModel, bool>) (c => ModelDb.Enchantment<Vigorous>().CanEnchant(c))) < 1 ? new EventOption((EventModel) this, (Func<Task>) null, "STONE_OF_ALL_TIME.pages.INITIAL.options.PUSH_LOCKED", Array.Empty<IHoverTip>()) : new EventOption((EventModel) this, new Func<Task>(this.Push), "STONE_OF_ALL_TIME.pages.INITIAL.options.PUSH", HoverTipFactory.FromEnchantment<Vigorous>(this.DynamicVars["PushVigorousAmount"].IntValue)).ThatDoesDamage(this.DynamicVars["PushHpLoss"].BaseValue);
    // ISSUE: object of a compiler-generated type is created
    return (IReadOnlyList<EventOption>) new \u003C\u003Ez__ReadOnlyArray<EventOption>(new EventOption[2]
    {
      eventOption1,
      eventOption2
    });
  }

  private async Task Lift()
  {
    await PotionCmd.Discard(this.DrinkAndLiftPotion);
    await CreatureCmd.GainMaxHp(this.Owner.Creature, this.DynamicVars["DrinkMaxHpGain"].BaseValue);
    this.Rng.NextInt(100);
    LocString description = this.L10NLookup("STONE_OF_ALL_TIME.pages.LIFT.description");
    description.Add(this.DynamicVars["DrinkRandomPotion"]);
    this.SetEventFinished(description);
  }

  private async Task Push()
  {
    IEnumerable<DamageResult> damageResults = await CreatureCmd.Damage((PlayerChoiceContext) new ThrowingPlayerChoiceContext(), this.Owner.Creature, this.DynamicVars["PushHpLoss"].BaseValue, ValueProp.Unblockable | ValueProp.Unpowered, (CardModel) null, (CardPlay) null);
    CardSelectorPrefs prefs = new CardSelectorPrefs(CardSelectorPrefs.EnchantSelectionPrompt, 1);
    Vigorous vigorous = ModelDb.Enchantment<Vigorous>();
    foreach (CardModel card in await CardSelectCmd.FromDeckForEnchantment(this.Owner, (EnchantmentModel) vigorous, this.DynamicVars["PushVigorousAmount"].IntValue, prefs))
    {
      CardCmd.Enchant(vigorous.ToMutable(), card, this.DynamicVars["PushVigorousAmount"].BaseValue);
      CardCmd.Preview(card);
    }
    this.Rng.NextInt(100);
    this.SetEventFinished(this.L10NLookup("STONE_OF_ALL_TIME.pages.PUSH.description"));
    vigorous = (Vigorous) null;
  }
}
