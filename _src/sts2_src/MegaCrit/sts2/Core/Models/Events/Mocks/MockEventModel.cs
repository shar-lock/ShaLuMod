// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Events.Mocks.MockEventModel
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Events;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Rewards;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Events.Mocks;

public class MockEventModel : EventModel
{
  public bool isShared;
  public int? optionChosen;
  public Func<EventModel, IReadOnlyList<EventOption>>? generateInitialOptions;

  public override bool IsMock => true;

  public override bool IsShared => this.isShared;

  public string OptionKey => this.Id.Entry + ".pages.INITIAL.options.TEST";

  private List<EventOption> DefaultInitialOptions
  {
    get
    {
      int capacity = 2;
      List<EventOption> defaultInitialOptions = new List<EventOption>(capacity);
      CollectionsMarshal.SetCount<EventOption>(defaultInitialOptions, capacity);
      Span<EventOption> span = CollectionsMarshal.AsSpan<EventOption>(defaultInitialOptions);
      int num1 = 0;
      span[num1] = new EventOption((EventModel) this, (Func<Task>) (() =>
      {
        this.optionChosen = new int?(0);
        return Task.CompletedTask;
      }), this.OptionKey, Array.Empty<IHoverTip>());
      int num2 = num1 + 1;
      span[num2] = new EventOption((EventModel) this, (Func<Task>) (() =>
      {
        this.optionChosen = new int?(1);
        return Task.CompletedTask;
      }), this.OptionKey, Array.Empty<IHoverTip>());
      return defaultInitialOptions;
    }
  }

  protected override IReadOnlyList<EventOption> GenerateInitialOptions()
  {
    List<EventOption> initialOptions = new List<EventOption>();
    Func<EventModel, IReadOnlyList<EventOption>> generateInitialOptions = this.generateInitialOptions;
    foreach (EventOption eventOption in (IEnumerable<EventOption>) ((generateInitialOptions != null ? generateInitialOptions((EventModel) this) : (IReadOnlyList<EventOption>) null) ?? (IReadOnlyList<EventOption>) this.DefaultInitialOptions))
      initialOptions.Add(new EventOption(eventOption));
    return (IReadOnlyList<EventOption>) initialOptions;
  }

  public new void EnterCombatWithoutExitingEvent<T>(
    IReadOnlyList<Reward> extraRewards,
    bool shouldResumeAfterCombat)
    where T : EncounterModel
  {
    base.EnterCombatWithoutExitingEvent<T>(extraRewards, shouldResumeAfterCombat);
  }

  public void SetEventState(IEnumerable<EventOption> options)
  {
    this.SetEventState(this.L10NLookup(""), options);
  }

  public void SetEventFinished() => this.SetEventFinished(this.L10NLookup(""));
}
