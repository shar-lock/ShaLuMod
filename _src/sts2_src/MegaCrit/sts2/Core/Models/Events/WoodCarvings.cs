// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Events.WoodCarvings
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
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.Enchantments;
using MegaCrit.Sts2.Core.Nodes;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.Runs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Events;

public sealed class WoodCarvings : EventModel
{
  private const string _birdCardKey = "BirdCard";
  private const string _snakeEnchantmentKey = "SnakeEnchantment";
  private const string _toricCardKey = "ToricCard";

  public override bool IsAllowed(IRunState runState)
  {
    return runState.Players.All<Player>((Func<Player, bool>) (p => CardPile.Get(PileType.Deck, p).Cards.Any<CardModel>((Func<CardModel, bool>) (c => c != null && c.Rarity == CardRarity.Basic && c.IsRemovable))));
  }

  protected override IReadOnlyList<EventOption> GenerateInitialOptions()
  {
    EventOption eventOption = !PileType.Deck.GetPile(this.Owner).Cards.Any<CardModel>((Func<CardModel, bool>) (c => ModelDb.Enchantment<Slither>().CanEnchant(c))) ? new EventOption((EventModel) this, (Func<Task>) null, "WOOD_CARVINGS.pages.INITIAL.options.SNAKE_LOCKED", Array.Empty<IHoverTip>()) : new EventOption((EventModel) this, new Func<Task>(this.Snake), "WOOD_CARVINGS.pages.INITIAL.options.SNAKE", HoverTipFactory.FromEnchantment<Slither>());
    // ISSUE: object of a compiler-generated type is created
    return (IReadOnlyList<EventOption>) new \u003C\u003Ez__ReadOnlyArray<EventOption>(new EventOption[3]
    {
      new EventOption((EventModel) this, new Func<Task>(this.Bird), "WOOD_CARVINGS.pages.INITIAL.options.BIRD", HoverTipFactory.FromCardWithCardHoverTips<Peck>()),
      eventOption,
      new EventOption((EventModel) this, new Func<Task>(this.Torus), "WOOD_CARVINGS.pages.INITIAL.options.TORUS", HoverTipFactory.FromCardWithCardHoverTips<ToricToughness>())
    });
  }

  protected override IEnumerable<DynamicVar> CanonicalVars
  {
    get
    {
      return (IEnumerable<DynamicVar>) new \u003C\u003Ez__ReadOnlyArray<DynamicVar>(new DynamicVar[3]
      {
        (DynamicVar) new StringVar("BirdCard", ModelDb.Card<Peck>().Title),
        (DynamicVar) new StringVar("SnakeEnchantment", ModelDb.Enchantment<Slither>().Title.GetFormattedText()),
        (DynamicVar) new StringVar("ToricCard", ModelDb.Card<ToricToughness>().Title)
      });
    }
  }

  private async Task Bird()
  {
    CardModel original = (await CardSelectCmd.FromDeckGeneric(this.Owner, new CardSelectorPrefs(CardSelectorPrefs.TransformSelectionPrompt, 1), (Func<CardModel, bool>) (c => c.IsTransformable && c.Rarity == CardRarity.Basic))).FirstOrDefault<CardModel>();
    if (original != null)
    {
      CardPileAddResult? nullable = await CardCmd.TransformTo<Peck>(original, CardPreviewStyle.EventLayout);
    }
    this.SetEventFinished(this.L10NLookup("WOOD_CARVINGS.pages.BIRD.description"));
  }

  private async Task Snake()
  {
    CardModel card = (await CardSelectCmd.FromDeckForEnchantment(this.Owner, (EnchantmentModel) ModelDb.Enchantment<Slither>(), 1, new CardSelectorPrefs(CardSelectorPrefs.EnchantSelectionPrompt, 1))).FirstOrDefault<CardModel>();
    if (card != null)
    {
      CardCmd.Enchant<Slither>(card, 1M);
      NCardEnchantVfx child = NCardEnchantVfx.Create(card);
      if (child != null)
      {
        NRun instance = NRun.Instance;
        if (instance != null)
          ((Godot.Node) instance.GlobalUi.CardPreviewContainer).AddChildSafely((Godot.Node) child);
      }
    }
    this.SetEventFinished(this.L10NLookup("WOOD_CARVINGS.pages.SNAKE.description"));
  }

  private async Task Torus()
  {
    CardModel original = (await CardSelectCmd.FromDeckGeneric(this.Owner, new CardSelectorPrefs(CardSelectorPrefs.TransformSelectionPrompt, 1), (Func<CardModel, bool>) (c => c != null && c.IsTransformable && c.Rarity == CardRarity.Basic))).FirstOrDefault<CardModel>();
    if (original != null)
    {
      CardPileAddResult? nullable = await CardCmd.TransformTo<ToricToughness>(original, CardPreviewStyle.EventLayout);
    }
    this.SetEventFinished(this.L10NLookup("WOOD_CARVINGS.pages.TORUS.description"));
  }
}
