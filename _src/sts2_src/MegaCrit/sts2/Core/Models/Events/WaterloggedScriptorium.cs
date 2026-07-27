// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Events.WaterloggedScriptorium
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Gold;
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
using System.Runtime.InteropServices;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Events;

public sealed class WaterloggedScriptorium : EventModel
{
  private const int _spawnGoldRequirement = 55;
  private const string _pricklySpongeGoldKey = "PricklySpongeGold";

  public override bool IsAllowed(IRunState runState)
  {
    return runState.Players.All<Player>((Func<Player, bool>) (p => p.Gold >= 55));
  }

  protected override IEnumerable<DynamicVar> CanonicalVars
  {
    get
    {
      return (IEnumerable<DynamicVar>) new \u003C\u003Ez__ReadOnlyArray<DynamicVar>(new DynamicVar[4]
      {
        (DynamicVar) new MaxHpVar(6M),
        (DynamicVar) new GoldVar(55),
        (DynamicVar) new GoldVar("PricklySpongeGold", 99),
        (DynamicVar) new CardsVar(2)
      });
    }
  }

  protected override IReadOnlyList<EventOption> GenerateInitialOptions()
  {
    int capacity = 1;
    List<EventOption> eventOptionList = new List<EventOption>(capacity);
    CollectionsMarshal.SetCount<EventOption>(eventOptionList, capacity);
    CollectionsMarshal.AsSpan<EventOption>(eventOptionList)[0] = new EventOption((EventModel) this, new Func<Task>(this.BloodyInk), "WATERLOGGED_SCRIPTORIUM.pages.INITIAL.options.BLOODY_INK", Array.Empty<IHoverTip>());
    List<EventOption> initialOptions = eventOptionList;
    if (this.Owner.Gold >= this.DynamicVars.Gold.IntValue)
      initialOptions.Add(new EventOption((EventModel) this, new Func<Task>(this.TentacleQuill), "WATERLOGGED_SCRIPTORIUM.pages.INITIAL.options.TENTACLE_QUILL", HoverTipFactory.FromEnchantment<Steady>()));
    else
      initialOptions.Add(new EventOption((EventModel) this, (Func<Task>) null, "WATERLOGGED_SCRIPTORIUM.pages.INITIAL.options.TENTACLE_QUILL_LOCKED", Array.Empty<IHoverTip>()));
    if (this.Owner.Gold >= this.DynamicVars["PricklySpongeGold"].IntValue)
      initialOptions.Add(new EventOption((EventModel) this, new Func<Task>(this.PricklySponge), "WATERLOGGED_SCRIPTORIUM.pages.INITIAL.options.PRICKLY_SPONGE", HoverTipFactory.FromEnchantment<Steady>()));
    else
      initialOptions.Add(new EventOption((EventModel) this, (Func<Task>) null, "WATERLOGGED_SCRIPTORIUM.pages.INITIAL.options.PRICKLY_SPONGE_LOCKED", Array.Empty<IHoverTip>()));
    return (IReadOnlyList<EventOption>) initialOptions;
  }

  private async Task PricklySponge()
  {
    await PlayerCmd.LoseGold(this.DynamicVars["PricklySpongeGold"].BaseValue, this.Owner, GoldLossType.Spent);
    CardSelectorPrefs prefs = new CardSelectorPrefs(CardSelectorPrefs.EnchantSelectionPrompt, this.DynamicVars.Cards.IntValue);
    foreach (CardModel card in await CardSelectCmd.FromDeckForEnchantment(this.Owner, (EnchantmentModel) ModelDb.Enchantment<Steady>(), 1, prefs))
    {
      CardCmd.Enchant<Steady>(card, 1M);
      NCardEnchantVfx child = NCardEnchantVfx.Create(card);
      if (child != null)
      {
        NRun instance = NRun.Instance;
        if (instance != null)
          ((Godot.Node) instance.GlobalUi.CardPreviewContainer).AddChildSafely((Godot.Node) child);
      }
    }
    this.SetEventFinished(this.L10NLookup("WATERLOGGED_SCRIPTORIUM.pages.PRICKLY_SPONGE.description"));
  }

  private async Task TentacleQuill()
  {
    await PlayerCmd.LoseGold(this.DynamicVars.Gold.BaseValue, this.Owner, GoldLossType.Spent);
    CardSelectorPrefs prefs = new CardSelectorPrefs(CardSelectorPrefs.EnchantSelectionPrompt, 1);
    CardModel card = (await CardSelectCmd.FromDeckForEnchantment(this.Owner, (EnchantmentModel) ModelDb.Enchantment<Steady>(), 1, prefs)).FirstOrDefault<CardModel>();
    if (card != null)
    {
      CardCmd.Enchant<Steady>(card, 1M);
      NCardEnchantVfx child = NCardEnchantVfx.Create(card);
      if (child != null)
      {
        NRun instance = NRun.Instance;
        if (instance != null)
          ((Godot.Node) instance.GlobalUi.CardPreviewContainer).AddChildSafely((Godot.Node) child);
      }
    }
    this.SetEventFinished(this.L10NLookup("WATERLOGGED_SCRIPTORIUM.pages.TENTACLE_QUILL.description"));
  }

  private async Task BloodyInk()
  {
    await CreatureCmd.GainMaxHp(this.Owner.Creature, this.DynamicVars.MaxHp.BaseValue);
    this.SetEventFinished(this.L10NLookup("WATERLOGGED_SCRIPTORIUM.pages.BLOODY_INK.description"));
  }
}
