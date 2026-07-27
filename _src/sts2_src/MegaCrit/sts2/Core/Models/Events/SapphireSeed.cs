// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Events.SapphireSeed
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
using MegaCrit.Sts2.Core.Nodes.Vfx;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Events;

public sealed class SapphireSeed : EventModel
{
  private const string _enchantmentKey = "Enchantment";

  protected override IReadOnlyList<EventOption> GenerateInitialOptions()
  {
    // ISSUE: object of a compiler-generated type is created
    return (IReadOnlyList<EventOption>) new \u003C\u003Ez__ReadOnlyArray<EventOption>(new EventOption[2]
    {
      new EventOption((EventModel) this, new Func<Task>(this.Eat), "SAPPHIRE_SEED.pages.INITIAL.options.EAT", Array.Empty<IHoverTip>()),
      new EventOption((EventModel) this, new Func<Task>(this.Plant), "SAPPHIRE_SEED.pages.INITIAL.options.PLANT", HoverTipFactory.FromEnchantment<Sown>())
    });
  }

  protected override IEnumerable<DynamicVar> CanonicalVars
  {
    get
    {
      return (IEnumerable<DynamicVar>) new \u003C\u003Ez__ReadOnlyArray<DynamicVar>(new DynamicVar[2]
      {
        (DynamicVar) new StringVar("Enchantment", ModelDb.Enchantment<Sown>().Title.GetFormattedText()),
        (DynamicVar) new HealVar(9M)
      });
    }
  }

  private async Task Eat()
  {
    await CreatureCmd.Heal(this.Owner.Creature, (Decimal) this.DynamicVars.Heal.IntValue);
    CardModel card = (await CardSelectCmd.FromDeckForUpgrade(this.Owner, new CardSelectorPrefs(CardSelectorPrefs.UpgradeSelectionPrompt, 1))).FirstOrDefault<CardModel>();
    if (card != null)
      CardCmd.Upgrade(card);
    this.SetEventFinished(this.L10NLookup("SAPPHIRE_SEED.pages.EAT.description"));
  }

  private async Task Plant()
  {
    EnchantmentModel sown = (EnchantmentModel) ModelDb.Enchantment<Sown>();
    List<CardModel> list = PileType.Deck.GetPile(this.Owner).Cards.Where<CardModel>((Func<CardModel, bool>) (c => sown.CanEnchant(c))).ToList<CardModel>();
    CardSelectorPrefs prefs = new CardSelectorPrefs(CardSelectorPrefs.EnchantSelectionPrompt, 1);
    CardModel card = (await CardSelectCmd.FromDeckForEnchantment((IReadOnlyList<CardModel>) list, sown, 1, prefs)).FirstOrDefault<CardModel>();
    if (card != null)
    {
      CardCmd.Enchant<Sown>(card, 1M);
      NCardEnchantVfx child = NCardEnchantVfx.Create(card);
      if (child != null)
      {
        NRun instance = NRun.Instance;
        if (instance != null)
          ((Godot.Node) instance.GlobalUi.CardPreviewContainer).AddChildSafely((Godot.Node) child);
      }
    }
    this.SetEventFinished(this.L10NLookup("SAPPHIRE_SEED.pages.PLANT.description"));
  }
}
