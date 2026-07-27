// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Events.Symbiote
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Events;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Enchantments;
using MegaCrit.Sts2.Core.Nodes;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.Runs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Events;

public sealed class Symbiote : EventModel
{
  private const string _enchantmentKey = "Enchantment";

  public override bool IsAllowed(IRunState runState) => runState.CurrentActIndex > 0;

  protected override IReadOnlyList<EventOption> GenerateInitialOptions()
  {
    // ISSUE: reference to a compiler-generated field
    // ISSUE: reference to a compiler-generated field
    EventOption eventOption1 = !PileType.Deck.GetPile(this.Owner).Cards.Any<CardModel>(Symbiote.\u003C\u003EO.\u003C0\u003E__CanEnchant ?? (Symbiote.\u003C\u003EO.\u003C0\u003E__CanEnchant = new Func<CardModel, bool>(Symbiote.CanEnchant))) ? new EventOption((EventModel) this, (Func<Task>) null, "SYMBIOTE.pages.INITIAL.options.APPROACH_LOCKED", Array.Empty<IHoverTip>()) : new EventOption((EventModel) this, new Func<Task>(this.Approach), "SYMBIOTE.pages.INITIAL.options.APPROACH", HoverTipFactory.FromEnchantment<Corrupted>());
    EventOption eventOption2 = new EventOption((EventModel) this, new Func<Task>(this.KillWithFire), "SYMBIOTE.pages.INITIAL.options.KILL_WITH_FIRE", new IHoverTip[1]
    {
      HoverTipFactory.Static(StaticHoverTip.Transform)
    });
    int capacity = 2;
    List<EventOption> initialOptions = new List<EventOption>(capacity);
    CollectionsMarshal.SetCount<EventOption>(initialOptions, capacity);
    Span<EventOption> span = CollectionsMarshal.AsSpan<EventOption>(initialOptions);
    int num1 = 0;
    span[num1] = eventOption1;
    int num2 = num1 + 1;
    span[num2] = eventOption2;
    return (IReadOnlyList<EventOption>) initialOptions;
  }

  protected override IEnumerable<DynamicVar> CanonicalVars
  {
    get
    {
      return (IEnumerable<DynamicVar>) new \u003C\u003Ez__ReadOnlyArray<DynamicVar>(new DynamicVar[2]
      {
        (DynamicVar) new StringVar("Enchantment", ModelDb.Enchantment<Corrupted>().Title.GetFormattedText()),
        (DynamicVar) new CardsVar(1)
      });
    }
  }

  private async Task Approach()
  {
    CardSelectorPrefs prefs = new CardSelectorPrefs(CardSelectorPrefs.EnchantSelectionPrompt, 1);
    CardModel card = (await CardSelectCmd.FromDeckForEnchantment(this.Owner, (EnchantmentModel) ModelDb.Enchantment<Corrupted>(), 1, prefs)).FirstOrDefault<CardModel>();
    if (card != null)
    {
      CardCmd.Enchant<Corrupted>(card, 1M);
      NCardEnchantVfx child = NCardEnchantVfx.Create(card);
      if (child != null)
      {
        NRun instance = NRun.Instance;
        if (instance != null)
          ((Godot.Node) instance.GlobalUi.CardPreviewContainer).AddChildSafely((Godot.Node) child);
      }
    }
    this.SetEventFinished(this.L10NLookup("SYMBIOTE.pages.APPROACH.description"));
  }

  private async Task KillWithFire()
  {
    foreach (CardModel original in (await CardSelectCmd.FromDeckForTransformation(this.Owner, new CardSelectorPrefs(CardSelectorPrefs.TransformSelectionPrompt, this.DynamicVars.Cards.IntValue))).ToList<CardModel>())
    {
      CardPileAddResult random = await CardCmd.TransformToRandom(original, this.Rng, CardPreviewStyle.EventLayout);
    }
    this.SetEventFinished(this.L10NLookup("SYMBIOTE.pages.KILL_WITH_FIRE.description"));
  }

  private static bool CanEnchant(CardModel card)
  {
    return ModelDb.Enchantment<Corrupted>().CanEnchant(card);
  }
}
