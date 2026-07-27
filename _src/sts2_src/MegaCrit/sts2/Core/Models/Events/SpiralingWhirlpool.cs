// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Events.SpiralingWhirlpool
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Events;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
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

public sealed class SpiralingWhirlpool : EventModel
{
  protected override IEnumerable<DynamicVar> CanonicalVars
  {
    get
    {
      return (IEnumerable<DynamicVar>) new \u003C\u003Ez__ReadOnlySingleElementList<DynamicVar>((DynamicVar) new HealVar(0M));
    }
  }

  public override void CalculateVars()
  {
    this.DynamicVars.Heal.BaseValue = this.Owner != null ? (Decimal) this.Owner.Creature.MaxHp * 0.33M : 0M;
  }

  public override bool IsAllowed(IRunState runState)
  {
    return runState.Players.All<Player>((Func<Player, bool>) (p => p.Deck.Cards.Any<CardModel>(new Func<CardModel, bool>(((EnchantmentModel) ModelDb.Enchantment<Spiral>()).CanEnchant))));
  }

  protected override IReadOnlyList<EventOption> GenerateInitialOptions()
  {
    // ISSUE: object of a compiler-generated type is created
    return (IReadOnlyList<EventOption>) new \u003C\u003Ez__ReadOnlyArray<EventOption>(new EventOption[2]
    {
      new EventOption((EventModel) this, new Func<Task>(this.ObserveTheSpiral), "SPIRALING_WHIRLPOOL.pages.INITIAL.options.OBSERVE", HoverTipFactory.FromEnchantment<Spiral>()),
      new EventOption((EventModel) this, new Func<Task>(this.Drink), "SPIRALING_WHIRLPOOL.pages.INITIAL.options.DRINK", Array.Empty<IHoverTip>())
    });
  }

  private async Task ObserveTheSpiral()
  {
    CardModel card = (await CardSelectCmd.FromDeckForEnchantment(this.Owner, (EnchantmentModel) ModelDb.Enchantment<Spiral>(), 1, (Func<CardModel, bool>) (c => ModelDb.Enchantment<Spiral>().CanEnchant(c)), new CardSelectorPrefs(CardSelectorPrefs.EnchantSelectionPrompt, 1))).FirstOrDefault<CardModel>();
    if (card != null)
    {
      CardCmd.Enchant<Spiral>(card, 1M);
      NCardEnchantVfx child = NCardEnchantVfx.Create(card);
      if (child != null)
      {
        NRun instance = NRun.Instance;
        if (instance != null)
          ((Godot.Node) instance.GlobalUi.CardPreviewContainer).AddChildSafely((Godot.Node) child);
      }
    }
    this.SetEventFinished(this.L10NLookup("SPIRALING_WHIRLPOOL.pages.OBSERVE.description"));
  }

  private async Task Drink()
  {
    await CreatureCmd.Heal(this.Owner.Creature, (Decimal) this.DynamicVars.Heal.IntValue);
    this.SetEventFinished(this.L10NLookup("SPIRALING_WHIRLPOOL.pages.DRINK.description"));
  }
}
