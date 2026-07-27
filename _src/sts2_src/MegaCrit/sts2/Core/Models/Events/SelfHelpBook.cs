// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Events.SelfHelpBook
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Events;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Enchantments;
using MegaCrit.Sts2.Core.Nodes;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Events;

public sealed class SelfHelpBook : EventModel
{
  private const string _readTheBackDescriptionKey = "SELF_HELP_BOOK.pages.READ_THE_BACK.description";
  private const string _readPassageDescriptionKey = "SELF_HELP_BOOK.pages.READ_PASSAGE.description";
  private const string _readEntireBookDescriptionKey = "SELF_HELP_BOOK.pages.READ_ENTIRE_BOOK.description";
  private const int _sharpAmount = 2;
  private const int _nimbleAmount = 2;
  private const int _swiftAmount = 2;

  protected override IEnumerable<DynamicVar> CanonicalVars
  {
    get
    {
      return (IEnumerable<DynamicVar>) new \u003C\u003Ez__ReadOnlyArray<DynamicVar>(new DynamicVar[6]
      {
        (DynamicVar) new StringVar("Enchantment1", ModelDb.Enchantment<Sharp>().Title.GetFormattedText()),
        (DynamicVar) new StringVar("Enchantment2", ModelDb.Enchantment<Nimble>().Title.GetFormattedText()),
        (DynamicVar) new StringVar("Enchantment3", ModelDb.Enchantment<Swift>().Title.GetFormattedText()),
        (DynamicVar) new IntVar("Enchantment1Amount", 2M),
        (DynamicVar) new IntVar("Enchantment2Amount", 2M),
        (DynamicVar) new IntVar("Enchantment3Amount", 2M)
      });
    }
  }

  protected override IReadOnlyList<EventOption> GenerateInitialOptions()
  {
    List<EventOption> initialOptions = new List<EventOption>();
    bool flag1 = this.PlayerHasCardsAvailable<Sharp>(this.Owner, CardType.Attack);
    bool flag2 = this.PlayerHasCardsAvailable<Nimble>(this.Owner, CardType.Skill);
    bool flag3 = this.PlayerHasCardsAvailable<Swift>(this.Owner, CardType.Power);
    if (flag1 | flag2 | flag3)
    {
      if (flag1)
        initialOptions.Add(new EventOption((EventModel) this, new Func<Task>(this.ReadTheBack), "SELF_HELP_BOOK.pages.INITIAL.options.READ_THE_BACK", HoverTipFactory.FromEnchantment<Sharp>(2)));
      else
        initialOptions.Add(new EventOption((EventModel) this, (Func<Task>) null, "SELF_HELP_BOOK.pages.INITIAL.options.READ_THE_BACK_LOCKED", Array.Empty<IHoverTip>()));
      if (flag2)
        initialOptions.Add(new EventOption((EventModel) this, new Func<Task>(this.ReadPassage), "SELF_HELP_BOOK.pages.INITIAL.options.READ_PASSAGE", HoverTipFactory.FromEnchantment<Nimble>(2)));
      else
        initialOptions.Add(new EventOption((EventModel) this, (Func<Task>) null, "SELF_HELP_BOOK.pages.INITIAL.options.READ_PASSAGE_LOCKED", Array.Empty<IHoverTip>()));
      if (flag3)
        initialOptions.Add(new EventOption((EventModel) this, new Func<Task>(this.ReadEntireBook), "SELF_HELP_BOOK.pages.INITIAL.options.READ_ENTIRE_BOOK", HoverTipFactory.FromEnchantment<Swift>(2)));
      else
        initialOptions.Add(new EventOption((EventModel) this, (Func<Task>) null, "SELF_HELP_BOOK.pages.INITIAL.options.READ_ENTIRE_BOOK_LOCKED", Array.Empty<IHoverTip>()));
    }
    else
      initialOptions.Add(new EventOption((EventModel) this, new Func<Task>(this.SkipBook), "SELF_HELP_BOOK.pages.INITIAL.options.NO_OPTIONS", Array.Empty<IHoverTip>()));
    return (IReadOnlyList<EventOption>) initialOptions;
  }

  private async Task ReadTheBack()
  {
    await this.SelectAndEnchant<Sharp>(2, CardType.Attack, this.L10NLookup("SELF_HELP_BOOK.pages.READ_THE_BACK.description"));
  }

  private async Task ReadPassage()
  {
    await this.SelectAndEnchant<Nimble>(2, CardType.Skill, this.L10NLookup("SELF_HELP_BOOK.pages.READ_PASSAGE.description"));
  }

  private async Task ReadEntireBook()
  {
    await this.SelectAndEnchant<Swift>(2, CardType.Power, this.L10NLookup("SELF_HELP_BOOK.pages.READ_ENTIRE_BOOK.description"));
  }

  private bool PlayerHasCardsAvailable<T>(Player player, CardType typeRestriction) where T : EnchantmentModel
  {
    EnchantmentModel enchantment = (EnchantmentModel) ModelDb.Enchantment<T>();
    return PileType.Deck.GetPile(player).Cards.FirstOrDefault<CardModel>((Func<CardModel, bool>) (c => this.DeckFilter(c, enchantment, typeRestriction))) != null;
  }

  private async Task SelectAndEnchant<T>(
    int amount,
    CardType typeRestriction,
    LocString finalDescription)
    where T : EnchantmentModel
  {
    CardSelectorPrefs prefs = new CardSelectorPrefs(CardSelectorPrefs.EnchantSelectionPrompt, 1);
    CardModel card = (await CardSelectCmd.FromDeckForEnchantment(this.Owner, (EnchantmentModel) ModelDb.Enchantment<T>(), amount, (Func<CardModel, bool>) (c => c.Type == typeRestriction), prefs)).FirstOrDefault<CardModel>();
    if (card != null)
      await this.ApplyEnchantment<T>(card, amount);
    this.SetEventFinished(finalDescription);
  }

  private bool DeckFilter(CardModel card, EnchantmentModel enchantment, CardType type)
  {
    return card.Pile.Type == PileType.Deck && card.Type == type && enchantment.CanEnchant(card);
  }

  private Task ApplyEnchantment<T>(CardModel card, int amount) where T : EnchantmentModel
  {
    CardCmd.Enchant<T>(card, (Decimal) amount);
    NCardEnchantVfx child = NCardEnchantVfx.Create(card);
    if (child != null)
    {
      NRun instance = NRun.Instance;
      if (instance != null)
        ((Godot.Node) instance.GlobalUi.CardPreviewContainer).AddChildSafely((Godot.Node) child);
    }
    return Task.CompletedTask;
  }

  private Task SkipBook()
  {
    this.SetEventFinished(this.L10NLookup("SELF_HELP_BOOK.pages.NO_OPTIONS.description"));
    return Task.CompletedTask;
  }
}
