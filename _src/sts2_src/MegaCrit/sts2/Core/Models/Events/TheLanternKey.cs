// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Events.TheLanternKey
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Events;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.Encounters;
using MegaCrit.Sts2.Core.Rewards;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Events;

public sealed class TheLanternKey : EventModel
{
  public override EventLayoutType LayoutType => EventLayoutType.Combat;

  public override EncounterModel CanonicalEncounter
  {
    get => (EncounterModel) ModelDb.Encounter<MysteriousKnightEventEncounter>();
  }

  public override bool IsShared => true;

  protected override IEnumerable<DynamicVar> CanonicalVars
  {
    get
    {
      return (IEnumerable<DynamicVar>) new \u003C\u003Ez__ReadOnlySingleElementList<DynamicVar>((DynamicVar) new GoldVar(100));
    }
  }

  protected override IReadOnlyList<EventOption> GenerateInitialOptions()
  {
    // ISSUE: object of a compiler-generated type is created
    return (IReadOnlyList<EventOption>) new \u003C\u003Ez__ReadOnlyArray<EventOption>(new EventOption[2]
    {
      new EventOption((EventModel) this, new Func<Task>(this.ReturnTheKey), "THE_LANTERN_KEY.pages.INITIAL.options.RETURN_THE_KEY", Array.Empty<IHoverTip>()),
      new EventOption((EventModel) this, new Func<Task>(this.KeepTheKey), "THE_LANTERN_KEY.pages.INITIAL.options.KEEP_THE_KEY", Array.Empty<IHoverTip>())
    });
  }

  private async Task ReturnTheKey()
  {
    await PlayerCmd.GainGold(this.DynamicVars.Gold.BaseValue, this.Owner);
    this.SetEventFinished(this.L10NLookup("THE_LANTERN_KEY.pages.DONE.options.RETURN_THE_KEY.description"));
  }

  private Task KeepTheKey()
  {
    // ISSUE: object of a compiler-generated type is created
    this.SetEventState(this.L10NLookup("THE_LANTERN_KEY.pages.KEEP_THE_KEY.description"), (IEnumerable<EventOption>) new \u003C\u003Ez__ReadOnlySingleElementList<EventOption>(new EventOption((EventModel) this, new Func<Task>(this.Fight), "THE_LANTERN_KEY.pages.KEEP_THE_KEY.options.FIGHT", Array.Empty<IHoverTip>())));
    return Task.CompletedTask;
  }

  private Task Fight()
  {
    int capacity = 1;
    List<Reward> extraRewards = new List<Reward>(capacity);
    CollectionsMarshal.SetCount<Reward>(extraRewards, capacity);
    CollectionsMarshal.AsSpan<Reward>(extraRewards)[0] = (Reward) new SpecialCardReward((CardModel) this.Owner.RunState.CreateCard<LanternKey>(this.Owner), this.Owner);
    this.EnterCombatWithoutExitingEvent<MysteriousKnightEventEncounter>((IReadOnlyList<Reward>) extraRewards, false);
    return Task.CompletedTask;
  }
}
