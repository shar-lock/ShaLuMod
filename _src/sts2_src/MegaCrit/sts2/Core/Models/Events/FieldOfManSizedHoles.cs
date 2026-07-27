// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Events.FieldOfManSizedHoles
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
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.Runs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Events;

public sealed class FieldOfManSizedHoles : EventModel
{
  public override bool IsAllowed(IRunState runState)
  {
    return runState.Players.All<Player>((Func<Player, bool>) (p => CardPile.Get(PileType.Deck, p).Cards.Any<CardModel>(new Func<CardModel, bool>(((EnchantmentModel) ModelDb.Enchantment<PerfectFit>()).CanEnchant))));
  }

  protected override IEnumerable<DynamicVar> CanonicalVars
  {
    get
    {
      return (IEnumerable<DynamicVar>) new \u003C\u003Ez__ReadOnlyArray<DynamicVar>(new DynamicVar[4]
      {
        (DynamicVar) new GoldVar(75),
        (DynamicVar) new CardsVar(2),
        (DynamicVar) new StringVar("ResistCurse", ModelDb.Card<Normality>().Title),
        (DynamicVar) new StringVar("Enchantment", ModelDb.Enchantment<PerfectFit>().Title.GetFormattedText())
      });
    }
  }

  protected override IReadOnlyList<EventOption> GenerateInitialOptions()
  {
    // ISSUE: object of a compiler-generated type is created
    return (IReadOnlyList<EventOption>) new \u003C\u003Ez__ReadOnlyArray<EventOption>(new EventOption[2]
    {
      new EventOption((EventModel) this, new Func<Task>(this.Resist), "FIELD_OF_MAN_SIZED_HOLES.pages.INITIAL.options.RESIST", HoverTipFactory.FromCardWithCardHoverTips<Normality>()),
      new EventOption((EventModel) this, new Func<Task>(this.EnterYourHole), "FIELD_OF_MAN_SIZED_HOLES.pages.INITIAL.options.ENTER_YOUR_HOLE", HoverTipFactory.FromEnchantment<PerfectFit>())
    });
  }

  private async Task Resist()
  {
    await CardPileCmd.RemoveFromDeck((IReadOnlyList<CardModel>) (await CardSelectCmd.FromDeckForRemoval(this.Owner, new CardSelectorPrefs(CardSelectorPrefs.RemoveSelectionPrompt, this.DynamicVars.Cards.IntValue))).ToList<CardModel>());
    // ISSUE: object of a compiler-generated type is created
    IEnumerable<CardPileAddResult> deck = await CardPileCmd.AddCursesToDeck((IEnumerable<CardModel>) new \u003C\u003Ez__ReadOnlySingleElementList<CardModel>((CardModel) ModelDb.Card<Normality>()), this.Owner);
    this.SetEventFinished(this.L10NLookup("FIELD_OF_MAN_SIZED_HOLES.pages.RESIST.description"));
  }

  private async Task EnterYourHole()
  {
    CardModel card = (await CardSelectCmd.FromDeckForEnchantment(this.Owner, (EnchantmentModel) ModelDb.Enchantment<PerfectFit>(), 1, new CardSelectorPrefs(CardSelectorPrefs.EnchantSelectionPrompt, 1))).FirstOrDefault<CardModel>();
    if (card != null)
    {
      CardCmd.Enchant<PerfectFit>(card, 1M);
      NCardEnchantVfx child = NCardEnchantVfx.Create(card);
      if (child != null)
      {
        NRun instance = NRun.Instance;
        if (instance != null)
          ((Godot.Node) instance.GlobalUi.CardPreviewContainer).AddChildSafely((Godot.Node) child);
      }
    }
    this.SetEventFinished(this.L10NLookup("FIELD_OF_MAN_SIZED_HOLES.pages.ENTER_YOUR_HOLE.description"));
  }
}
