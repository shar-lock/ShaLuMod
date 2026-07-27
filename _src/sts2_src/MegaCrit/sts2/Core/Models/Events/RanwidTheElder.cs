// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Events.RanwidTheElder
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Gold;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Events;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Runs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Events;

public sealed class RanwidTheElder : EventModel
{
  private const string _potionChoiceKey = "RANWID_THE_ELDER.pages.INITIAL.options.POTION";
  private const string _potionKey = "Potion";
  private const string _relicChoiceKey = "RANWID_THE_ELDER.pages.INITIAL.options.RELIC";
  private const string _relicKey = "Relic";

  private LocString PotionChoiceTitle
  {
    get => new LocString("events", "RANWID_THE_ELDER.pages.INITIAL.options.POTION.title");
  }

  private LocString PotionChoiceDescription
  {
    get => new LocString("events", "RANWID_THE_ELDER.pages.INITIAL.options.POTION.description");
  }

  private LocString RelicChoiceTitle
  {
    get => new LocString("events", "RANWID_THE_ELDER.pages.INITIAL.options.RELIC.title");
  }

  private LocString RelicChoiceDescription
  {
    get => new LocString("events", "RANWID_THE_ELDER.pages.INITIAL.options.RELIC.description");
  }

  protected override Task BeforeEventStarted(bool isPreFinished)
  {
    this.Owner.CanUseOrRemovePotions = false;
    return Task.CompletedTask;
  }

  protected override void OnEventFinished() => this.Owner.CanUseOrRemovePotions = true;

  public override bool IsAllowed(IRunState runState)
  {
    return runState.CurrentActIndex != 0 && !runState.Players.Any<Player>((Func<Player, bool>) (p => !this.GetValidRelics(p).Any<RelicModel>())) && !runState.Players.Any<Player>((Func<Player, bool>) (p => p.Gold < 100)) && !runState.Players.Any<Player>((Func<Player, bool>) (p => !p.Potions.Any<PotionModel>()));
  }

  private IEnumerable<RelicModel> GetValidRelics(Player player)
  {
    return player.Relics.Where<RelicModel>((Func<RelicModel, bool>) (r => r.IsTradable));
  }

  protected override IEnumerable<DynamicVar> CanonicalVars
  {
    get
    {
      return (IEnumerable<DynamicVar>) new \u003C\u003Ez__ReadOnlyArray<DynamicVar>(new DynamicVar[3]
      {
        (DynamicVar) new GoldVar(100),
        (DynamicVar) new StringVar("Potion", "Potion"),
        (DynamicVar) new StringVar("Relic", "Relic")
      });
    }
  }

  protected override IReadOnlyList<EventOption> GenerateInitialOptions()
  {
    List<EventOption> initialOptions = new List<EventOption>();
    PotionModel potion = this.Rng.NextItem<PotionModel>(this.Owner.Potions);
    if (potion != null)
    {
      ((StringVar) this.DynamicVars["Potion"]).StringValue = potion.Title.GetFormattedText();
      initialOptions.Add(new EventOption((EventModel) this, (Func<Task>) (async () => await this.GivePotion(potion)), this.PotionChoiceTitle, this.PotionChoiceDescription, "RANWID_THE_ELDER.pages.INITIAL.options.POTION", potion.HoverTips).ThatHasDynamicTitle());
    }
    else
      initialOptions.Add(new EventOption((EventModel) this, (Func<Task>) null, "RANWID_THE_ELDER.pages.INITIAL.options.POTION_LOCKED", Array.Empty<IHoverTip>()));
    initialOptions.Add(new EventOption((EventModel) this, new Func<Task>(this.GiveGold), "RANWID_THE_ELDER.pages.INITIAL.options.GOLD", Array.Empty<IHoverTip>()));
    RelicModel relic = this.Rng.NextItem<RelicModel>(this.Owner.Relics.Where<RelicModel>((Func<RelicModel, bool>) (r => r.IsTradable)));
    if (relic != null)
    {
      ((StringVar) this.DynamicVars["Relic"]).StringValue = relic.Title.GetFormattedText();
      initialOptions.Add(new EventOption((EventModel) this, (Func<Task>) (async () => await this.GiveRelic(relic)), this.RelicChoiceTitle, this.RelicChoiceDescription, "RANWID_THE_ELDER.pages.INITIAL.options.RELIC", relic.HoverTips).ThatHasDynamicTitle());
    }
    else
      initialOptions.Add(new EventOption((EventModel) this, (Func<Task>) null, "RANWID_THE_ELDER.pages.INITIAL.options.RELIC_LOCKED", Array.Empty<IHoverTip>()));
    return (IReadOnlyList<EventOption>) initialOptions;
  }

  private async Task GivePotion(PotionModel potion)
  {
    await PotionCmd.Discard(potion);
    RelicModel relicModel = await RelicCmd.Obtain(RelicFactory.PullNextRelicFromFront(this.Owner).ToMutable(), this.Owner);
    this.SetEventFinished(this.L10NLookup("RANWID_THE_ELDER.pages.POTION.description"));
  }

  private async Task GiveGold()
  {
    await PlayerCmd.LoseGold((Decimal) this.DynamicVars.Gold.IntValue, this.Owner, GoldLossType.Spent);
    RelicModel relicModel = await RelicCmd.Obtain(RelicFactory.PullNextRelicFromFront(this.Owner).ToMutable(), this.Owner);
    this.SetEventFinished(this.L10NLookup("RANWID_THE_ELDER.pages.GOLD.description"));
  }

  private async Task GiveRelic(RelicModel relic)
  {
    await RelicCmd.Remove(relic);
    for (int i = 0; i < 2; ++i)
    {
      RelicModel relicModel = await RelicCmd.Obtain(RelicFactory.PullNextRelicFromFront(this.Owner).ToMutable(), this.Owner);
    }
    this.SetEventFinished(this.L10NLookup("RANWID_THE_ELDER.pages.RELIC.description"));
  }
}
