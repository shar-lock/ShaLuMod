// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Events.EndlessConveyor
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Gold;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Events;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.PotionPools;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Rewards;
using MegaCrit.Sts2.Core.Runs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Events;

public sealed class EndlessConveyor : EventModel
{
  private const string _currentDishTitleKey = "CurrentDishTitle";
  private const string _lastDishTitleKey = "LastDishTitle";
  private const string _goldenFyshGoldKey = "GoldenFyshGold";
  private const string _clamRollHealKey = "ClamRollHeal";
  private const string _caviarMaxHpKey = "CaviarMaxHp";
  private string _lastDishId = "";
  private int _numOfGrabs;
  private EndlessConveyor.Dish _currentDish;

  public override bool IsAllowed(IRunState runState)
  {
    return runState.Players.All<Player>((Func<Player, bool>) (p => p.Gold >= 120));
  }

  private int NumOfGrabs
  {
    get => this._numOfGrabs;
    set
    {
      this.AssertMutable();
      this._numOfGrabs = value;
    }
  }

  private EndlessConveyor.Dish CurrentDish
  {
    get => this._currentDish;
    set
    {
      this.AssertMutable();
      this._currentDish = value;
    }
  }

  protected override IEnumerable<DynamicVar> CanonicalVars
  {
    get
    {
      return (IEnumerable<DynamicVar>) new \u003C\u003Ez__ReadOnlyArray<DynamicVar>(new DynamicVar[6]
      {
        (DynamicVar) new GoldVar(40),
        (DynamicVar) new GoldVar("GoldenFyshGold", 75),
        (DynamicVar) new HealVar("ClamRollHeal", 10M),
        (DynamicVar) new MaxHpVar("CaviarMaxHp", 4M),
        (DynamicVar) new StringVar("CurrentDishTitle"),
        (DynamicVar) new StringVar("LastDishTitle")
      });
    }
  }

  public override void CalculateVars()
  {
    this.RollDish();
    ((StringVar) this.DynamicVars["CurrentDishTitle"]).StringValue = this.CurrentDish.title.GetFormattedText();
  }

  protected override IReadOnlyList<EventOption> GenerateInitialOptions()
  {
    // ISSUE: object of a compiler-generated type is created
    return (IReadOnlyList<EventOption>) new \u003C\u003Ez__ReadOnlyArray<EventOption>(new EventOption[2]
    {
      this.GenerateGrabSomethingOffTheBeltOption(),
      new EventOption((EventModel) this, new Func<Task>(this.ObserveChef), "ENDLESS_CONVEYOR.pages.INITIAL.options.OBSERVE_CHEF", Array.Empty<IHoverTip>())
    });
  }

  private async Task GrabSomethingOffTheBelt()
  {
    if (this._currentDish.id != "GOLDEN_FYSH")
      await PlayerCmd.LoseGold((Decimal) this.DynamicVars.Gold.IntValue, this.Owner, GoldLossType.Spent);
    await this.CurrentDish.action();
    this.RollDish();
    ((StringVar) this.DynamicVars["LastDishTitle"]).StringValue = ((StringVar) this.DynamicVars["CurrentDishTitle"]).StringValue;
    ((StringVar) this.DynamicVars["CurrentDishTitle"]).StringValue = this.CurrentDish.title.GetFormattedText();
    // ISSUE: object of a compiler-generated type is created
    this.SetEventState(this.L10NLookup("ENDLESS_CONVEYOR.pages.GRAB_SOMETHING_OFF_THE_BELT.description"), (IEnumerable<EventOption>) new \u003C\u003Ez__ReadOnlyArray<EventOption>(new EventOption[2]
    {
      this.GenerateGrabSomethingOffTheBeltOption(),
      new EventOption((EventModel) this, new Func<Task>(this.Leave), "ENDLESS_CONVEYOR.pages.GRAB_SOMETHING_OFF_THE_BELT.options.LEAVE", Array.Empty<IHoverTip>())
    }));
  }

  private EventOption GenerateGrabSomethingOffTheBeltOption()
  {
    return this.Owner.Gold >= this.DynamicVars.Gold.IntValue ? new EventOption((EventModel) this, new Func<Task>(this.GrabSomethingOffTheBelt), this._currentDish.optionKey, this._currentDish.hoverTips) : new EventOption((EventModel) this, (Func<Task>) null, "ENDLESS_CONVEYOR.pages.ALL.options.LOCKED", Array.Empty<IHoverTip>());
  }

  private async Task ClamRoll()
  {
    await CreatureCmd.Heal(this.Owner.Creature, (Decimal) this.DynamicVars["ClamRollHeal"].IntValue);
  }

  private async Task Caviar()
  {
    await CreatureCmd.GainMaxHp(this.Owner.Creature, (Decimal) this.DynamicVars["CaviarMaxHp"].IntValue);
  }

  private async Task SuspiciousCondiment()
  {
    PotionModel potionModel = this.Owner.PlayerRng.Rewards.NextItem<PotionModel>(this.Owner.Character.PotionPool.GetUnlockedPotions(this.Owner.UnlockState).Concat<PotionModel>(ModelDb.PotionPool<SharedPotionPool>().GetUnlockedPotions(this.Owner.UnlockState)));
    if (potionModel == null)
      return;
    await RewardsCmd.OfferCustom(this.Owner, new List<Reward>(1)
    {
      (Reward) new PotionReward(potionModel.ToMutable(), this.Owner)
    });
  }

  private async Task JellyLiver()
  {
    CardModel original = (await CardSelectCmd.FromDeckForTransformation(this.Owner, new CardSelectorPrefs(CardSelectorPrefs.TransformSelectionPrompt, 1))).FirstOrDefault<CardModel>();
    if (original == null)
      return;
    CardPileAddResult random = await CardCmd.TransformToRandom(original, this.Rng, CardPreviewStyle.EventLayout);
  }

  private async Task SeapunkSalad()
  {
    CardCmd.PreviewCardPileAdd(await CardPileCmd.Add((CardModel) this.Owner.RunState.CreateCard<FeedingFrenzy>(this.Owner), PileType.Deck), style: CardPreviewStyle.EventLayout);
  }

  private async Task FriedEel()
  {
    // ISSUE: object of a compiler-generated type is created
    CardCmd.PreviewCardPileAdd(await CardPileCmd.Add(CardFactory.CreateForReward(this.Owner, 1, CardCreationOptions.ForNonCombatWithDefaultOdds((IEnumerable<CardPoolModel>) new \u003C\u003Ez__ReadOnlySingleElementList<CardPoolModel>((CardPoolModel) ModelDb.CardPool<ColorlessCardPool>()))).First<CardCreationResult>().Card, PileType.Deck), style: CardPreviewStyle.EventLayout);
  }

  private async Task GoldenFysh()
  {
    await PlayerCmd.GainGold(this.DynamicVars["GoldenFyshGold"].BaseValue, this.Owner);
  }

  private Task SpicySnappy()
  {
    List<CardModel> list = PileType.Deck.GetPile(this.Owner).Cards.Where<CardModel>((Func<CardModel, bool>) (c => c.IsUpgradable)).ToList<CardModel>();
    if (list.Count != 0)
      CardCmd.Upgrade(this.Rng.NextItem<CardModel>((IEnumerable<CardModel>) list));
    return Task.CompletedTask;
  }

  private void RollDish()
  {
    ++this.NumOfGrabs;
    if (this.NumOfGrabs % 5 == 0)
    {
      this._lastDishId = "SEAPUNK_SALAD";
      this._currentDish = new EndlessConveyor.Dish("SEAPUNK_SALAD", new Func<Task>(this.SeapunkSalad), HoverTipFactory.FromCardWithCardHoverTips<FeedingFrenzy>(), 0.0f);
    }
    else
    {
      int capacity = 4;
      List<EndlessConveyor.Dish> dishList1 = new List<EndlessConveyor.Dish>(capacity);
      CollectionsMarshal.SetCount<EndlessConveyor.Dish>(dishList1, capacity);
      Span<EndlessConveyor.Dish> span = CollectionsMarshal.AsSpan<EndlessConveyor.Dish>(dishList1);
      int num1 = 0;
      span[num1] = new EndlessConveyor.Dish("CAVIAR", new Func<Task>(this.Caviar), (IEnumerable<IHoverTip>) Array.Empty<IHoverTip>(), 6f);
      int num2 = num1 + 1;
      span[num2] = new EndlessConveyor.Dish("SPICY_SNAPPY", new Func<Task>(this.SpicySnappy), (IEnumerable<IHoverTip>) Array.Empty<IHoverTip>(), 3f);
      int num3 = num2 + 1;
      // ISSUE: object of a compiler-generated type is created
      span[num3] = new EndlessConveyor.Dish("JELLY_LIVER", new Func<Task>(this.JellyLiver), (IEnumerable<IHoverTip>) new \u003C\u003Ez__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.Static(StaticHoverTip.Transform)), 3f);
      int num4 = num3 + 1;
      span[num4] = new EndlessConveyor.Dish("FRIED_EEL", new Func<Task>(this.FriedEel), (IEnumerable<IHoverTip>) Array.Empty<IHoverTip>(), 3f);
      List<EndlessConveyor.Dish> dishList2 = dishList1;
      if (this.Owner.HasOpenPotionSlots)
        dishList2.Add(new EndlessConveyor.Dish("SUSPICIOUS_CONDIMENT", new Func<Task>(this.SuspiciousCondiment), (IEnumerable<IHoverTip>) Array.Empty<IHoverTip>(), 3f));
      if (this.Owner.Creature.CurrentHp != this.Owner.Creature.MaxHp)
        dishList2.Add(new EndlessConveyor.Dish("CLAM_ROLL", new Func<Task>(this.ClamRoll), (IEnumerable<IHoverTip>) Array.Empty<IHoverTip>(), 6f));
      if (this.NumOfGrabs > 1)
        dishList2.Add(new EndlessConveyor.Dish("GOLDEN_FYSH", new Func<Task>(this.GoldenFysh), (IEnumerable<IHoverTip>) Array.Empty<IHoverTip>(), 1f));
      dishList2.RemoveAll((Predicate<EndlessConveyor.Dish>) (d => d.id == this._lastDishId));
      float num5 = 0.0f;
      foreach (EndlessConveyor.Dish dish in dishList2)
        num5 += dish.weight;
      float num6 = this.Rng.NextFloat() * num5;
      float num7 = 0.0f;
      foreach (EndlessConveyor.Dish dish in dishList2)
      {
        num7 += dish.weight;
        if ((double) num6 < (double) num7)
        {
          this._lastDishId = dish.id;
          this._currentDish = dish;
          break;
        }
      }
    }
  }

  private Task ObserveChef()
  {
    IEnumerable<CardModel> source = this.Owner.Deck.Cards.Where<CardModel>((Func<CardModel, bool>) (c => c.IsUpgradable));
    if (!(source is CardModel[] cardModelArray))
      cardModelArray = source.ToArray<CardModel>();
    IEnumerable<CardModel> cardModels = (IEnumerable<CardModel>) cardModelArray;
    if (cardModels.Any<CardModel>())
      CardCmd.Upgrade(this.Rng.NextItem<CardModel>(cardModels));
    this.SetEventFinished(this.L10NLookup("ENDLESS_CONVEYOR.pages.OBSERVE_CHEF.description"));
    return Task.CompletedTask;
  }

  private Task Leave()
  {
    this.SetEventFinished(this.L10NLookup("ENDLESS_CONVEYOR.pages.LEAVE.description"));
    return Task.CompletedTask;
  }

  private record struct Dish(
    string id,
    Func<Task> action,
    IEnumerable<IHoverTip> hoverTips,
    float weight)
  {
    public readonly string id = id;
    public readonly LocString title = new LocString("events", $"ENDLESS_CONVEYOR.DISHES.{id}.title");
    public readonly string optionKey = "ENDLESS_CONVEYOR.pages.ALL.options." + id;
    public readonly IEnumerable<IHoverTip> hoverTips = hoverTips;
    public readonly float weight = weight;
    public readonly Func<Task> action = action;

    [CompilerGenerated]
    public override readonly int GetHashCode()
    {
      return ((((EqualityComparer<string>.Default.GetHashCode(this.id) * -1521134295 + EqualityComparer<LocString>.Default.GetHashCode(this.title)) * -1521134295 + EqualityComparer<string>.Default.GetHashCode(this.optionKey)) * -1521134295 + EqualityComparer<IEnumerable<IHoverTip>>.Default.GetHashCode(this.hoverTips)) * -1521134295 + EqualityComparer<float>.Default.GetHashCode(this.weight)) * -1521134295 + EqualityComparer<Func<Task>>.Default.GetHashCode(this.action);
    }

    [CompilerGenerated]
    public readonly bool Equals(EndlessConveyor.Dish other)
    {
      return EqualityComparer<string>.Default.Equals(this.id, other.id) && EqualityComparer<LocString>.Default.Equals(this.title, other.title) && EqualityComparer<string>.Default.Equals(this.optionKey, other.optionKey) && EqualityComparer<IEnumerable<IHoverTip>>.Default.Equals(this.hoverTips, other.hoverTips) && EqualityComparer<float>.Default.Equals(this.weight, other.weight) && EqualityComparer<Func<Task>>.Default.Equals(this.action, other.action);
    }
  }
}
