// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Events.DollRoom
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Audio.Debug;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Events;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Relics;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.TestSupport;
using MegaCrit.Sts2.Core.ValueProps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Events;

public sealed class DollRoom : EventModel
{
  private const string _takeTimeHpLossKey = "TakeTimeHpLoss";
  private const string _examineHpLossKey = "ExamineHpLoss";
  private int? _ambienceHandle;
  private static readonly DollRoom.DollChoice[] _dolls = new DollRoom.DollChoice[3]
  {
    new DollRoom.DollChoice()
    {
      relic = (RelicModel) ModelDb.Relic<DaughterOfTheWind>(),
      descriptionKey = "DOLL_ROOM.pages.DAUGHTER_OF_WIND.description"
    },
    new DollRoom.DollChoice()
    {
      relic = (RelicModel) ModelDb.Relic<MrStruggles>(),
      descriptionKey = "DOLL_ROOM.pages.MR_STRUGGLES.description"
    },
    new DollRoom.DollChoice()
    {
      relic = (RelicModel) ModelDb.Relic<BingBong>(),
      descriptionKey = "DOLL_ROOM.pages.FABLE.description"
    }
  };

  private int? AmbienceHandle
  {
    get => this._ambienceHandle;
    set
    {
      this.AssertMutable();
      this._ambienceHandle = value;
    }
  }

  public override bool IsAllowed(IRunState runState) => runState.CurrentActIndex == 1;

  protected override IEnumerable<DynamicVar> CanonicalVars
  {
    get
    {
      return (IEnumerable<DynamicVar>) new \u003C\u003Ez__ReadOnlyArray<DynamicVar>(new DynamicVar[2]
      {
        (DynamicVar) new DamageVar("TakeTimeHpLoss", 5M, ValueProp.Unblockable | ValueProp.Unpowered),
        (DynamicVar) new DamageVar("ExamineHpLoss", 15M, ValueProp.Unblockable | ValueProp.Unpowered)
      });
    }
  }

  protected override IReadOnlyList<EventOption> GenerateInitialOptions()
  {
    // ISSUE: object of a compiler-generated type is created
    return (IReadOnlyList<EventOption>) new \u003C\u003Ez__ReadOnlyArray<EventOption>(new EventOption[3]
    {
      new EventOption((EventModel) this, new Func<Task>(this.ChooseRandom), "DOLL_ROOM.pages.INITIAL.options.RANDOM", Array.Empty<IHoverTip>()),
      new EventOption((EventModel) this, new Func<Task>(this.TakeSomeTime), "DOLL_ROOM.pages.INITIAL.options.TAKE_SOME_TIME", Array.Empty<IHoverTip>()).ThatDoesDamage(this.DynamicVars["TakeTimeHpLoss"].BaseValue),
      new EventOption((EventModel) this, new Func<Task>(this.Examine), "DOLL_ROOM.pages.INITIAL.options.EXAMINE", Array.Empty<IHoverTip>()).ThatDoesDamage(this.DynamicVars["ExamineHpLoss"].BaseValue)
    });
  }

  protected override Task BeforeEventStarted(bool isPreFinished)
  {
    if (LocalContext.IsMe(this.Owner) && TestMode.IsOff)
      this.AmbienceHandle = new int?(NDebugAudioManager.Instance.Play("doll_room_amb.mp3"));
    return Task.CompletedTask;
  }

  private async Task ChooseRandom()
  {
    await this.ChooseDollAndShowDescription(this.Rng.NextItem<DollRoom.DollChoice>((IEnumerable<DollRoom.DollChoice>) DollRoom._dolls));
  }

  private async Task TakeSomeTime()
  {
    IEnumerable<DamageResult> damageResults = await CreatureCmd.Damage((PlayerChoiceContext) new ThrowingPlayerChoiceContext(), this.Owner.Creature, (DamageVar) this.DynamicVars["TakeTimeHpLoss"], (Creature) null, (CardModel) null, (CardPlay) null);
    IEnumerable<DollRoom.DollChoice> dollChoices = ((IEnumerable<DollRoom.DollChoice>) DollRoom._dolls).ToList<DollRoom.DollChoice>().StableShuffle<DollRoom.DollChoice>(this.Rng).Take<DollRoom.DollChoice>(2);
    List<EventOption> eventOptionList = new List<EventOption>();
    foreach (DollRoom.DollChoice choice in dollChoices)
      eventOptionList.Add(this.OptionFromChoice(choice));
    this.SetEventState(this.L10NLookup("DOLL_ROOM.pages.TAKE_SOME_TIME.description"), (IEnumerable<EventOption>) eventOptionList);
  }

  private async Task Examine()
  {
    IEnumerable<DamageResult> damageResults = await CreatureCmd.Damage((PlayerChoiceContext) new ThrowingPlayerChoiceContext(), this.Owner.Creature, (DamageVar) this.DynamicVars["ExamineHpLoss"], (Creature) null, (CardModel) null, (CardPlay) null);
    IEnumerable<DollRoom.DollChoice> dollChoices = (IEnumerable<DollRoom.DollChoice>) ((IEnumerable<DollRoom.DollChoice>) DollRoom._dolls).ToList<DollRoom.DollChoice>().StableShuffle<DollRoom.DollChoice>(this.Rng);
    List<EventOption> eventOptionList = new List<EventOption>();
    foreach (DollRoom.DollChoice choice in dollChoices)
      eventOptionList.Add(this.OptionFromChoice(choice));
    this.SetEventState(this.L10NLookup("DOLL_ROOM.pages.EXAMINE.description"), (IEnumerable<EventOption>) eventOptionList);
  }

  private EventOption OptionFromChoice(DollRoom.DollChoice choice)
  {
    LocString title = choice.relic.Title;
    LocString description = this.L10NLookup("DOLL_ROOM.pages.TAKE.options.TAKE.description");
    description.Add("RelicName", choice.relic.Title);
    return new EventOption((EventModel) this, new Func<Task>(Func), title, description, choice.relic.Title.GetRawText(), HoverTipFactory.FromRelic(choice.relic)).WithOverridenHistoryName(choice.relic.Title);

    Task Func() => this.ChooseDollAndShowDescription(choice);
  }

  private async Task ChooseDollAndShowDescription(DollRoom.DollChoice choice)
  {
    this.StopAudio();
    RelicModel relicModel = await RelicCmd.Obtain(choice.relic.ToMutable(), this.Owner);
    this.SetEventFinished(this.L10NLookup(choice.descriptionKey));
  }

  protected override void OnEventFinished() => this.StopAudio();

  private void StopAudio()
  {
    if (!this.AmbienceHandle.HasValue)
      return;
    NDebugAudioManager.Instance.Stop(this.AmbienceHandle.Value);
    this.AmbienceHandle = new int?();
  }

  private struct DollChoice : IComparable<DollRoom.DollChoice>
  {
    public RelicModel relic;
    public string descriptionKey;

    public int CompareTo(DollRoom.DollChoice other)
    {
      return this.relic.CompareTo((AbstractModel) other.relic);
    }
  }
}
