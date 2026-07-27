// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Events.SlipperyBridge
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Events;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.ValueProps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Events;

public sealed class SlipperyBridge : EventModel
{
  private const string _randomCardKey = "RandomCard";
  private const string _hpLossKey = "HpLoss";
  private const string _overcomeLocKey = "SLIPPERY_BRIDGE.pages.INITIAL.options.OVERCOME";
  private const int _initialHpLoss = 3;
  private int _numberOfHoldOns;
  private CardModel? _randomCardToLose;
  private HashSet<CardModel>? _skippedRemovals;

  private int NumberOfHoldOns
  {
    get => this._numberOfHoldOns;
    set
    {
      this.AssertMutable();
      this._numberOfHoldOns = value;
    }
  }

  private CardModel? RandomCardToLose
  {
    get => this._randomCardToLose;
    set
    {
      this.AssertMutable();
      this._randomCardToLose = value;
    }
  }

  private HashSet<CardModel>? SkippedRemovals
  {
    get => this._skippedRemovals;
    set
    {
      this.AssertMutable();
      this._skippedRemovals = value;
    }
  }

  private int CurrentHpLoss => 3 + this.NumberOfHoldOns;

  protected override IEnumerable<DynamicVar> CanonicalVars
  {
    get
    {
      return (IEnumerable<DynamicVar>) new \u003C\u003Ez__ReadOnlyArray<DynamicVar>(new DynamicVar[2]
      {
        (DynamicVar) new StringVar("RandomCard"),
        new DynamicVar("HpLoss", (Decimal) this.CurrentHpLoss)
      });
    }
  }

  public override bool IsAllowed(IRunState runState)
  {
    return runState.TotalFloor > 6 && runState.Players.All<Player>((Func<Player, bool>) (p => p.Deck.Cards.Any<CardModel>((Func<CardModel, bool>) (c => c.IsRemovable))));
  }

  protected override IReadOnlyList<EventOption> GenerateInitialOptions()
  {
    this.GetNewRandomCard();
    // ISSUE: object of a compiler-generated type is created
    return (IReadOnlyList<EventOption>) new \u003C\u003Ez__ReadOnlyArray<EventOption>(new EventOption[2]
    {
      new EventOption((EventModel) this, new Func<Task>(this.Overcome), "SLIPPERY_BRIDGE.pages.INITIAL.options.OVERCOME", new IHoverTip[1]
      {
        HoverTipFactory.FromCard(this.RandomCardToLose)
      }),
      new EventOption((EventModel) this, new Func<Task>(this.HoldOn), "SLIPPERY_BRIDGE.pages.INITIAL.options.HOLD_ON_0", Array.Empty<IHoverTip>()).ThatDoesDamage((Decimal) this.CurrentHpLoss)
    });
  }

  public override void OnRoomEnter()
  {
    NEventRoom instance = NEventRoom.Instance;
    if (instance == null)
      return;
    Control vfxContainer = instance.VfxContainer;
    if (vfxContainer == null)
      return;
    ((Godot.Node) vfxContainer).AddChildSafely((Godot.Node) NRainVfx.Create());
  }

  private void GetNewRandomCard()
  {
    List<CardModel> list;
    if (this.RandomCardToLose == null)
    {
      list = this.Owner.Deck.Cards.Where<CardModel>((Func<CardModel, bool>) (c => c.Rarity != CardRarity.Basic)).ToList<CardModel>();
    }
    else
    {
      if (this.SkippedRemovals == null)
      {
        HashSet<CardModel> cardModelSet;
        this.SkippedRemovals = cardModelSet = new HashSet<CardModel>();
      }
      this.SkippedRemovals.Add(this.RandomCardToLose);
      list = this.Owner.Deck.Cards.Where<CardModel>((Func<CardModel, bool>) (c => c.GetType() != this.RandomCardToLose.GetType())).ToList<CardModel>();
    }
    list.RemoveAll((Predicate<CardModel>) (c =>
    {
      if (!c.IsRemovable)
        return true;
      HashSet<CardModel> skippedRemovals = this.SkippedRemovals;
      // ISSUE: explicit non-virtual call
      return skippedRemovals != null && __nonvirtual (skippedRemovals.Contains(c));
    }));
    if (list.Count == 0)
      list = this.Owner.Deck.Cards.Where<CardModel>((Func<CardModel, bool>) (c => c.IsRemovable)).ToList<CardModel>();
    this.RandomCardToLose = this.Rng.NextItem<CardModel>((IEnumerable<CardModel>) list);
    ((StringVar) this.DynamicVars["RandomCard"]).StringValue = this.RandomCardToLose.Title;
  }

  private async Task Overcome()
  {
    await CardPileCmd.RemoveFromDeck(this.RandomCardToLose);
    this.SetEventFinished(this.L10NLookup("SLIPPERY_BRIDGE.pages.OVERCOME.description"));
  }

  private async Task HoldOn()
  {
    IEnumerable<DamageResult> damageResults = await CreatureCmd.Damage((PlayerChoiceContext) new ThrowingPlayerChoiceContext(), this.Owner.Creature, (Decimal) this.CurrentHpLoss, ValueProp.Unblockable | ValueProp.Unpowered, (CardModel) null, (CardPlay) null);
    this.NumberOfHoldOns++;
    this.DynamicVars["HpLoss"].BaseValue = (Decimal) this.CurrentHpLoss;
    this.GetNewRandomCard();
    string holdOnSuffix1 = this.GetHoldOnSuffix(this.NumberOfHoldOns - 1);
    string holdOnSuffix2 = this.GetHoldOnSuffix(this.NumberOfHoldOns);
    string textKey = $"SLIPPERY_BRIDGE.pages.HOLD_ON_{holdOnSuffix1}.options.HOLD_ON_{holdOnSuffix2}";
    // ISSUE: object of a compiler-generated type is created
    this.SetEventState(this.L10NLookup($"SLIPPERY_BRIDGE.pages.HOLD_ON_{holdOnSuffix1}.description"), (IEnumerable<EventOption>) new \u003C\u003Ez__ReadOnlyArray<EventOption>(new EventOption[2]
    {
      new EventOption((EventModel) this, new Func<Task>(this.Overcome), "SLIPPERY_BRIDGE.pages.INITIAL.options.OVERCOME", new IHoverTip[1]
      {
        HoverTipFactory.FromCard(this.RandomCardToLose)
      }),
      new EventOption((EventModel) this, new Func<Task>(this.HoldOn), textKey, Array.Empty<IHoverTip>()).ThatDoesDamage((Decimal) this.CurrentHpLoss)
    }));
  }

  private string GetHoldOnSuffix(int holdOnNumber)
  {
    return holdOnNumber >= 7 ? "LOOP" : holdOnNumber.ToString();
  }
}
