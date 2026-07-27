// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Events.RelicTrader
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Events;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Runs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Events;

public sealed class RelicTrader : EventModel
{
  private const string _topRelicOwnedKey = "TopRelicOwned";
  private const string _topRelicNewKey = "TopRelicNew";
  private const string _middleRelicOwnedKey = "MiddleRelicOwned";
  private const string _middleRelicNewKey = "MiddleRelicNew";
  private const string _bottomRelicOwnedKey = "BottomRelicOwned";
  private const string _bottomRelicNewKey = "BottomRelicNew";
  private IReadOnlyList<RelicModel>? _ownedRelics;
  private IReadOnlyList<RelicModel>? _newRelics;

  protected override IReadOnlyList<EventOption> GenerateInitialOptions()
  {
    List<EventOption> initialOptions = new List<EventOption>();
    if (this.OwnedRelics.Count >= 1)
      initialOptions.Add(new EventOption((EventModel) this, new Func<Task>(this.Top), "RELIC_TRADER.pages.INITIAL.options.TOP", this.GetRelicHoverTips(0)));
    if (this.OwnedRelics.Count >= 2)
      initialOptions.Add(new EventOption((EventModel) this, new Func<Task>(this.Middle), "RELIC_TRADER.pages.INITIAL.options.MIDDLE", this.GetRelicHoverTips(1)));
    if (this.OwnedRelics.Count >= 3)
      initialOptions.Add(new EventOption((EventModel) this, new Func<Task>(this.Bottom), "RELIC_TRADER.pages.INITIAL.options.BOTTOM", this.GetRelicHoverTips(2)));
    if (initialOptions.Count == 0)
      initialOptions.Add(new EventOption((EventModel) this, new Func<Task>(this.Done), "PROCEED", Array.Empty<IHoverTip>()));
    return (IReadOnlyList<EventOption>) initialOptions;
  }

  protected override IEnumerable<DynamicVar> CanonicalVars
  {
    get
    {
      return (IEnumerable<DynamicVar>) new \u003C\u003Ez__ReadOnlyArray<DynamicVar>(new DynamicVar[6]
      {
        (DynamicVar) new StringVar("TopRelicOwned"),
        (DynamicVar) new StringVar("TopRelicNew"),
        (DynamicVar) new StringVar("MiddleRelicOwned"),
        (DynamicVar) new StringVar("MiddleRelicNew"),
        (DynamicVar) new StringVar("BottomRelicOwned"),
        (DynamicVar) new StringVar("BottomRelicNew")
      });
    }
  }

  public override bool IsAllowed(IRunState runState)
  {
    return runState.CurrentActIndex != 0 && runState.Players.All<Player>((Func<Player, bool>) (p => this.GetValidRelics(p).Count<RelicModel>() >= 5));
  }

  private IEnumerable<RelicModel> GetValidRelics(Player player)
  {
    return player.Relics.Where<RelicModel>((Func<RelicModel, bool>) (r => r.IsTradable));
  }

  private IReadOnlyList<RelicModel> OwnedRelics
  {
    get
    {
      this.AssertMutable();
      if (this._ownedRelics == null)
        this._ownedRelics = (IReadOnlyList<RelicModel>) this.GetValidRelics(this.Owner).ToList<RelicModel>().StableShuffle<RelicModel>(this.Rng).Take<RelicModel>(3).ToList<RelicModel>();
      return this._ownedRelics;
    }
  }

  private IReadOnlyList<RelicModel> NewRelics
  {
    get
    {
      this.AssertMutable();
      if (this._newRelics == null)
      {
        RelicModel[] relicModelArray = new RelicModel[3];
        for (int index = 0; index < relicModelArray.Length; ++index)
          relicModelArray[index] = RelicFactory.PullNextRelicFromFront(this.Owner);
        this._newRelics = (IReadOnlyList<RelicModel>) relicModelArray;
      }
      return this._newRelics;
    }
  }

  public override void CalculateVars()
  {
    if (this.OwnedRelics.Count > 0 && this.NewRelics.Count > 0)
    {
      ((StringVar) this.DynamicVars["TopRelicOwned"]).StringValue = this.OwnedRelics[0].Title.GetFormattedText();
      ((StringVar) this.DynamicVars["TopRelicNew"]).StringValue = this.NewRelics[0].Title.GetFormattedText();
    }
    if (this.OwnedRelics.Count > 1 && this.NewRelics.Count > 1)
    {
      ((StringVar) this.DynamicVars["MiddleRelicOwned"]).StringValue = this.OwnedRelics[1].Title.GetFormattedText();
      ((StringVar) this.DynamicVars["MiddleRelicNew"]).StringValue = this.NewRelics[1].Title.GetFormattedText();
    }
    if (this.OwnedRelics.Count <= 2 || this.NewRelics.Count <= 2)
      return;
    ((StringVar) this.DynamicVars["BottomRelicOwned"]).StringValue = this.OwnedRelics[2].Title.GetFormattedText();
    ((StringVar) this.DynamicVars["BottomRelicNew"]).StringValue = this.NewRelics[2].Title.GetFormattedText();
  }

  private async Task Top() => await this.Trade(0);

  private async Task Middle() => await this.Trade(1);

  private async Task Bottom() => await this.Trade(2);

  private async Task Trade(int index)
  {
    await RelicCmd.Remove(this.OwnedRelics[index]);
    RelicModel relicModel = await RelicCmd.Obtain(this.NewRelics[index].ToMutable(), this.Owner);
    await this.Done();
  }

  private Task Done()
  {
    this.SetEventFinished(this.L10NLookup("RELIC_TRADER.pages.DONE.description"));
    return Task.CompletedTask;
  }

  private IEnumerable<IHoverTip> GetRelicHoverTips(int index)
  {
    if (this.OwnedRelics.Count <= index || this.NewRelics.Count <= index)
      return (IEnumerable<IHoverTip>) Array.Empty<IHoverTip>();
    List<IHoverTip> items = new List<IHoverTip>();
    items.AddRange(this.OwnedRelics.ElementAt<RelicModel>(index).HoverTips);
    items.AddRange(this.NewRelics.ElementAt<RelicModel>(index).HoverTips);
    // ISSUE: object of a compiler-generated type is created
    return (IEnumerable<IHoverTip>) new \u003C\u003Ez__ReadOnlyList<IHoverTip>(items);
  }
}
