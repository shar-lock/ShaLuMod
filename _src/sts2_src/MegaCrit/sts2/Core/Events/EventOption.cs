// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Events.EventOption
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Events;

public class EventOption
{
  public string TextKey { get; private set; }

  public LocString Title { get; private set; }

  public LocString Description { get; private set; }

  private Func<Task>? OnChosen { get; }

  public IEnumerable<IHoverTip> HoverTips { get; set; }

  public bool IsLocked { get; }

  public bool IsProceed { get; private set; }

  public bool WasChosen { get; private set; }

  public RelicModel? Relic { get; private set; }

  private bool DisableOnChosen { get; }

  public Func<Player, bool>? WillKillPlayer { get; private set; }

  public bool ShouldSaveChoiceToHistory { get; private set; } = true;

  public LocString HistoryName { get; private set; }

  public bool ShouldSaveVariablesToHistory { get; private set; } = true;

  public event Func<EventOption, Task>? BeforeChosen;

  public EventOption(
    EventModel eventModel,
    Func<Task>? onChosen,
    LocString title,
    LocString description,
    string textKey,
    IEnumerable<IHoverTip> hoverTips)
  {
    this.TextKey = textKey;
    this.OnChosen = onChosen;
    this.Title = title;
    this.Description = description;
    this.HoverTips = hoverTips;
    this.IsLocked = this.OnChosen == null;
    this.DisableOnChosen = true;
    this.HistoryName = title;
    this.AddLocVars(eventModel);
  }

  public EventOption(
    EventModel eventModel,
    Func<Task>? onChosen,
    string textKey,
    IEnumerable<IHoverTip> hoverTips)
  {
    this.TextKey = textKey;
    this.OnChosen = onChosen;
    this.Title = eventModel.GetOptionTitle(textKey);
    this.Description = eventModel.GetOptionDescription(textKey);
    this.HoverTips = hoverTips;
    this.IsLocked = this.OnChosen == null;
    this.DisableOnChosen = true;
    this.HistoryName = this.Title;
    this.AddLocVars(eventModel);
  }

  public EventOption(
    EventModel eventModel,
    Func<Task>? onChosen,
    string textKey,
    params IHoverTip[] hoverTips)
    : this(eventModel, onChosen, textKey, (IEnumerable<IHoverTip>) ((IEnumerable<IHoverTip>) hoverTips).ToList<IHoverTip>())
  {
  }

  public EventOption(
    EventModel eventModel,
    Func<Task>? onChosen,
    string textKey,
    bool disableOnChosen = true,
    bool isProceed = false,
    params IHoverTip[] hoverTips)
    : this(eventModel, onChosen, textKey, (IEnumerable<IHoverTip>) ((IEnumerable<IHoverTip>) hoverTips).ToList<IHoverTip>())
  {
    this.IsProceed = isProceed;
    this.DisableOnChosen = disableOnChosen;
  }

  public EventOption(EventOption eventOption)
  {
    this.TextKey = eventOption.TextKey;
    this.OnChosen = eventOption.OnChosen;
    this.Title = eventOption.Title;
    this.Description = eventOption.Description;
    this.HoverTips = eventOption.HoverTips;
    this.IsLocked = eventOption.IsLocked;
    this.DisableOnChosen = eventOption.DisableOnChosen;
    this.HistoryName = eventOption.HistoryName;
    this.IsProceed = eventOption.IsProceed;
    this.DisableOnChosen = eventOption.DisableOnChosen;
  }

  public static EventOption FromRelic(
    RelicModel relic,
    EventModel eventModel,
    Func<Task>? onChosen,
    string textKey)
  {
    LocString title = eventModel.GetOptionTitle(textKey) ?? relic.Title;
    LocString description = eventModel.GetOptionDescription(textKey) ?? relic.DynamicEventDescription;
    return new EventOption(eventModel, onChosen, title, description, textKey, relic.HoverTipsExcludingRelic).WithRelic(relic);
  }

  public EventOption WithRelic<T>(Player? owner) where T : RelicModel
  {
    RelicModel mutable = ModelDb.Relic<T>().ToMutable();
    if (owner != null)
      mutable.Owner = owner;
    return this.WithRelic(mutable);
  }

  public EventOption WithRelic(RelicModel relic)
  {
    relic.AssertMutable();
    this.Relic = relic;
    return this;
  }

  public async Task Chosen()
  {
    if (this.OnChosen == null || this.DisableOnChosen && this.WasChosen)
      return;
    this.WasChosen = true;
    if (this.BeforeChosen != null)
      await this.BeforeChosen(this);
    await this.OnChosen();
  }

  public EventOption WithOverridenHistoryName(LocString historyName)
  {
    this.HistoryName = historyName;
    return this;
  }

  public EventOption ThatDoesDamage(Decimal damage)
  {
    return this.ThatWillKillPlayerIf((Func<Player, bool>) (p => (Decimal) p.Creature.CurrentHp <= damage));
  }

  public EventOption ThatDecreasesMaxHp(Decimal value)
  {
    return this.ThatWillKillPlayerIf((Func<Player, bool>) (p => (Decimal) p.Creature.MaxHp <= value));
  }

  public EventOption ThatWillKillPlayerIf(Func<Player, bool> willKillPlayer)
  {
    this.WillKillPlayer = willKillPlayer;
    return this;
  }

  public EventOption ThatHasDynamicTitle()
  {
    this.ShouldSaveVariablesToHistory = true;
    return this;
  }

  public EventOption ThatWontSaveToChoiceHistory()
  {
    this.ShouldSaveChoiceToHistory = false;
    return this;
  }

  private void AddLocVars(EventModel eventModel)
  {
    eventModel.Owner?.Character.AddDetailsTo(this.Description);
    LocString description = this.Description;
    Player owner = eventModel.Owner;
    int num = owner != null ? (owner.RunState.Players.Count > 1 ? 1 : 0) : 0;
    description.Add("IsMultiplayer", num != 0);
  }

  public override string ToString()
  {
    return $"{nameof (EventOption)} title: {this.Title.GetRawText()} description: {this.Description.GetRawText()} textKey: {this.TextKey}";
  }
}
