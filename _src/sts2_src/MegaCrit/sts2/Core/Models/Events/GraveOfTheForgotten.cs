// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Events.GraveOfTheForgotten
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
using MegaCrit.Sts2.Core.Models.Relics;
using MegaCrit.Sts2.Core.Nodes;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.Runs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Events;

public sealed class GraveOfTheForgotten : EventModel
{
  private const string _enchantmentKey = "Enchantment";
  private const string _relicKey = "Relic";
  private const string _curseKey = "Curse";

  public override bool IsAllowed(IRunState runState)
  {
    return runState.Players.All<Player>(new Func<Player, bool>(this.HasEnchantableCards));
  }

  protected override IReadOnlyList<EventOption> GenerateInitialOptions()
  {
    // ISSUE: object of a compiler-generated type is created
    return (IReadOnlyList<EventOption>) new \u003C\u003Ez__ReadOnlyArray<EventOption>(new EventOption[2]
    {
      !this.HasEnchantableCards(this.Owner) ? new EventOption((EventModel) this, (Func<Task>) null, "GRAVE_OF_THE_FORGOTTEN.pages.INITIAL.options.CONFRONT_LOCKED", Array.Empty<IHoverTip>()) : new EventOption((EventModel) this, new Func<Task>(this.Confront), "GRAVE_OF_THE_FORGOTTEN.pages.INITIAL.options.CONFRONT", HoverTipFactory.FromEnchantment<SoulsPower>().Concat<IHoverTip>(HoverTipFactory.FromCardWithCardHoverTips<Decay>())),
      new EventOption((EventModel) this, new Func<Task>(this.Accept), "GRAVE_OF_THE_FORGOTTEN.pages.INITIAL.options.ACCEPT", HoverTipFactory.FromRelic<ForgottenSoul>())
    });
  }

  protected override IEnumerable<DynamicVar> CanonicalVars
  {
    get
    {
      return (IEnumerable<DynamicVar>) new \u003C\u003Ez__ReadOnlyArray<DynamicVar>(new DynamicVar[3]
      {
        (DynamicVar) new StringVar("Relic", ModelDb.Relic<ForgottenSoul>().Title.GetFormattedText()),
        (DynamicVar) new StringVar("Enchantment", ModelDb.Enchantment<SoulsPower>().Title.GetFormattedText()),
        (DynamicVar) new StringVar("Curse", ModelDb.Card<Decay>().Title)
      });
    }
  }

  private async Task Confront()
  {
    CardModel deck = await CardPileCmd.AddCurseToDeck<Decay>(this.Owner);
    CardModel card = (await CardSelectCmd.FromDeckForEnchantment(this.Owner, (EnchantmentModel) ModelDb.Enchantment<SoulsPower>(), 1, new CardSelectorPrefs(CardSelectorPrefs.EnchantSelectionPrompt, 1))).FirstOrDefault<CardModel>();
    if (card != null)
    {
      CardCmd.Enchant<SoulsPower>(card, 1M);
      NCardEnchantVfx child = NCardEnchantVfx.Create(card);
      if (child != null)
      {
        NRun instance = NRun.Instance;
        if (instance != null)
          ((Godot.Node) instance.GlobalUi.CardPreviewContainer).AddChildSafely((Godot.Node) child);
      }
    }
    this.SetEventFinished(this.L10NLookup("GRAVE_OF_THE_FORGOTTEN.pages.CONFRONT.description"));
  }

  private async Task Accept()
  {
    ForgottenSoul forgottenSoul = await RelicCmd.Obtain<ForgottenSoul>(this.Owner);
    this.SetEventFinished(this.L10NLookup("GRAVE_OF_THE_FORGOTTEN.pages.ACCEPT.description"));
  }

  private bool HasEnchantableCards(Player player)
  {
    return PileType.Deck.GetPile(player).Cards.Any<CardModel>((Func<CardModel, bool>) (c => ModelDb.Enchantment<SoulsPower>().CanEnchant(c)));
  }
}
