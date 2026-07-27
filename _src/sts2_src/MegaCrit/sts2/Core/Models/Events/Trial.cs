// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Events.Trial
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Events;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Screens.MainMenu;
using MegaCrit.Sts2.Core.Random;
using MegaCrit.Sts2.Core.Rewards;
using MegaCrit.Sts2.Core.Runs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Events;

public sealed class Trial : EventModel
{
  private static readonly string _trialMerchantVfx = SceneHelper.GetScenePath("vfx/events/trial_merchant_vfx");
  private static readonly string _trialNobleVfx = SceneHelper.GetScenePath("vfx/events/trial_noble_vfx");
  private static readonly string _trialNondescriptVfx = SceneHelper.GetScenePath("vfx/events/trial_nondescript_vfx");
  private const string _entrantNumberKey = "EntrantNumber";
  private const string _trialResultKey = "TrialResult";
  private const string _trialStoryKey = "TrialStory";

  protected override IReadOnlyList<EventOption> GenerateInitialOptions()
  {
    // ISSUE: object of a compiler-generated type is created
    return (IReadOnlyList<EventOption>) new \u003C\u003Ez__ReadOnlyArray<EventOption>(new EventOption[2]
    {
      new EventOption((EventModel) this, new Func<Task>(this.Accept), "TRIAL.pages.INITIAL.options.ACCEPT", Array.Empty<IHoverTip>()),
      new EventOption((EventModel) this, new Func<Task>(this.Reject), "TRIAL.pages.INITIAL.options.REJECT", Array.Empty<IHoverTip>())
    });
  }

  protected override IEnumerable<DynamicVar> CanonicalVars
  {
    get
    {
      return (IEnumerable<DynamicVar>) new \u003C\u003Ez__ReadOnlySingleElementList<DynamicVar>(new DynamicVar("EntrantNumber", -1M));
    }
  }

  private static string TrialStartedPath => ImageHelper.GetImagePath("events/trial_started.png");

  public override IEnumerable<string> GetAssetPaths(IRunState runState)
  {
    List<string> items = new List<string>();
    items.AddRange(base.GetAssetPaths(runState));
    items.Add(Trial.TrialStartedPath);
    items.Add(Trial._trialMerchantVfx);
    items.Add(Trial._trialNobleVfx);
    items.Add(Trial._trialNondescriptVfx);
    // ISSUE: object of a compiler-generated type is created
    return (IEnumerable<string>) new \u003C\u003Ez__ReadOnlyList<string>(items);
  }

  private Task Accept()
  {
    if (LocalContext.IsMe(this.Owner))
      NEventRoom.Instance.Layout.RemoveNodesOnPortrait();
    string portraitPath;
    string entryName;
    EventOption[] eventOptionArray;
    switch (this.Rng.NextInt(3))
    {
      case 0:
        portraitPath = Trial._trialMerchantVfx;
        entryName = "TRIAL.pages.MERCHANT.description";
        eventOptionArray = new EventOption[2]
        {
          new EventOption((EventModel) this, new Func<Task>(this.MerchantGuilty), "TRIAL.pages.MERCHANT.options.GUILTY", HoverTipFactory.FromCardWithCardHoverTips<Regret>()),
          new EventOption((EventModel) this, new Func<Task>(this.MerchantInnocent), "TRIAL.pages.MERCHANT.options.INNOCENT", HoverTipFactory.FromCardWithCardHoverTips<Shame>())
        };
        break;
      case 1:
        portraitPath = Trial._trialNobleVfx;
        entryName = "TRIAL.pages.NOBLE.description";
        eventOptionArray = new EventOption[2]
        {
          new EventOption((EventModel) this, new Func<Task>(this.NobleGuilty), "TRIAL.pages.NOBLE.options.GUILTY", Array.Empty<IHoverTip>()),
          new EventOption((EventModel) this, new Func<Task>(this.NobleInnocent), "TRIAL.pages.NOBLE.options.INNOCENT", HoverTipFactory.FromCardWithCardHoverTips<Regret>())
        };
        break;
      case 2:
        portraitPath = Trial._trialNondescriptVfx;
        entryName = "TRIAL.pages.NONDESCRIPT.description";
        // ISSUE: object of a compiler-generated type is created
        eventOptionArray = new EventOption[2]
        {
          new EventOption((EventModel) this, new Func<Task>(this.NondescriptGuilty), "TRIAL.pages.NONDESCRIPT.options.GUILTY", HoverTipFactory.FromCardWithCardHoverTips<Doubt>()),
          new EventOption((EventModel) this, new Func<Task>(this.NondescriptInnocent), "TRIAL.pages.NONDESCRIPT.options.INNOCENT", HoverTipFactory.FromCardWithCardHoverTips<Doubt>().Concat<IHoverTip>((IEnumerable<IHoverTip>) new \u003C\u003Ez__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.Static(StaticHoverTip.Transform))))
        };
        break;
      default:
        throw new ArgumentOutOfRangeException();
    }
    this.AddVfxAnchoredToPortrait(portraitPath);
    if (LocalContext.IsMe(this.Owner))
      NEventRoom.Instance.SetPortrait(PreloadManager.Cache.GetTexture2D(Trial.TrialStartedPath));
    LocString description = this.L10NLookup("TRIAL.trialFormat");
    description.Add((DynamicVar) new StringVar("TrialStory", this.L10NLookup(entryName).GetRawText()));
    this.SetEventState(description, (IEnumerable<EventOption>) eventOptionArray);
    return Task.CompletedTask;
  }

  private Task Reject()
  {
    EventOption[] eventOptionArray = new EventOption[2]
    {
      new EventOption((EventModel) this, new Func<Task>(this.Accept), "TRIAL.pages.REJECT.options.ACCEPT", Array.Empty<IHoverTip>()),
      new EventOption((EventModel) this, new Func<Task>(this.DoubleDown), "TRIAL.pages.REJECT.options.DOUBLE_DOWN", false, true, Array.Empty<IHoverTip>()).ThatWillKillPlayerIf((Func<Player, bool>) (_ => true))
    };
    this.SetEventState(this.L10NLookup("TRIAL.pages.REJECT.description"), (IEnumerable<EventOption>) eventOptionArray);
    return Task.CompletedTask;
  }

  private Task DoubleDown()
  {
    NModalContainer.Instance.Add((Godot.Node) NAbandonRunConfirmPopup.Create((NMainMenu) null));
    return Task.CompletedTask;
  }

  private void AddVfxAnchoredToPortrait(string portraitPath)
  {
    if (!LocalContext.IsMe(this.Owner))
      return;
    Node2D vfx = PreloadManager.Cache.GetScene(portraitPath).Instantiate<Node2D>((PackedScene.GenEditState) 0L);
    vfx.Position = new Vector2(292f, 68f);
    NEventRoom.Instance.Layout.AddVfxAnchoredToPortrait((Godot.Node) vfx);
  }

  private async Task MerchantGuilty()
  {
    CardModel deck = await CardPileCmd.AddCurseToDeck<Regret>(this.Owner);
    for (int i = 0; i < 2; ++i)
    {
      RelicModel relicModel = await RelicCmd.Obtain(RelicFactory.PullNextRelicFromFront(this.Owner).ToMutable(), this.Owner);
    }
    this.SetTrialFinished("TRIAL.pages.MERCHANT_GUILTY.description");
  }

  private async Task MerchantInnocent()
  {
    CardModel deck = await CardPileCmd.AddCurseToDeck<Shame>(this.Owner);
    foreach (CardModel card in await CardSelectCmd.FromDeckForUpgrade(this.Owner, new CardSelectorPrefs(CardSelectorPrefs.UpgradeSelectionPrompt, 2)))
      CardCmd.Upgrade(card);
    this.SetTrialFinished("TRIAL.pages.MERCHANT_INNOCENT.description");
  }

  private async Task NobleGuilty()
  {
    await CreatureCmd.Heal(this.Owner.Creature, 10M);
    this.SetTrialFinished("TRIAL.pages.NOBLE_GUILTY.description");
  }

  private async Task NobleInnocent()
  {
    CardModel deck = await CardPileCmd.AddCurseToDeck<Regret>(this.Owner);
    await PlayerCmd.GainGold(300M, this.Owner);
    this.SetTrialFinished("TRIAL.pages.NOBLE_INNOCENT.description");
  }

  private async Task NondescriptGuilty()
  {
    CardModel deck = await CardPileCmd.AddCurseToDeck<Doubt>(this.Owner);
    List<Reward> rewards = new List<Reward>();
    for (int index = 0; index < 2; ++index)
    {
      // ISSUE: object of a compiler-generated type is created
      rewards.Add((Reward) new CardReward(CardCreationOptions.ForNonCombatWithDefaultOdds((IEnumerable<CardPoolModel>) new \u003C\u003Ez__ReadOnlySingleElementList<CardPoolModel>(this.Owner.Character.CardPool)), 3, this.Owner));
    }
    await RewardsCmd.OfferCustom(this.Owner, rewards);
    this.SetTrialFinished("TRIAL.pages.NONDESCRIPT_GUILTY.description");
  }

  private async Task NondescriptInnocent()
  {
    CardModel deck = await CardPileCmd.AddCurseToDeck<Doubt>(this.Owner);
    foreach (CardModel original in (await CardSelectCmd.FromDeckForTransformation(this.Owner, new CardSelectorPrefs(CardSelectorPrefs.TransformSelectionPrompt, 2))).ToList<CardModel>())
    {
      CardPileAddResult random = await CardCmd.TransformToRandom(original, this.Rng, CardPreviewStyle.EventLayout);
    }
    this.SetTrialFinished("TRIAL.pages.NONDESCRIPT_INNOCENT.description");
  }

  private void SetTrialFinished(string trialResultLoc)
  {
    LocString description = this.L10NLookup("TRIAL.trialResult");
    description.Add((DynamicVar) new StringVar("TrialResult", this.L10NLookup(trialResultLoc).GetRawText()));
    this.SetEventFinished(description);
  }

  public override void CalculateVars()
  {
    if (!(this.DynamicVars["EntrantNumber"].BaseValue == -1M))
      return;
    this.DynamicVars["EntrantNumber"].BaseValue = (Decimal) Rng.Chaotic.NextInt(101, 999);
  }
}
