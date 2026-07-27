// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Relics.JossPaper
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Saves.Runs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Relics;

public sealed class JossPaper : RelicModel
{
  private const string _exhaustAmountKey = "ExhaustAmount";
  private bool _isActivating;
  private int _cardsExhausted;
  private int _etherealCount;

  public override string FlashSfx => "event:/sfx/ui/relic_activate_draw";

  public override RelicRarity Rarity => RelicRarity.Uncommon;

  public override bool ShowCounter => true;

  public override int DisplayAmount
  {
    get => !this.IsActivating ? this.CardsExhausted : this.DynamicVars["ExhaustAmount"].IntValue;
  }

  private bool IsActivating
  {
    get => this._isActivating;
    set
    {
      this.AssertMutable();
      this._isActivating = value;
      this.InvokeDisplayAmountChanged();
    }
  }

  [SavedProperty(SerializationCondition.SaveIfNotTypeDefault)]
  public int CardsExhausted
  {
    get => this._cardsExhausted;
    set
    {
      this.AssertMutable();
      this._cardsExhausted = value;
      this.Status = (Decimal) this._cardsExhausted == this.DynamicVars["ExhaustAmount"].BaseValue - 1M ? RelicStatus.Active : RelicStatus.Normal;
      this.InvokeDisplayAmountChanged();
    }
  }

  private int EtherealCount
  {
    get => this._etherealCount;
    set
    {
      this.AssertMutable();
      this._etherealCount = value;
    }
  }

  protected override IEnumerable<IHoverTip> ExtraHoverTips
  {
    get
    {
      return (IEnumerable<IHoverTip>) new \u003C\u003Ez__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromKeyword(CardKeyword.Exhaust));
    }
  }

  protected override IEnumerable<DynamicVar> CanonicalVars
  {
    get
    {
      return (IEnumerable<DynamicVar>) new \u003C\u003Ez__ReadOnlyArray<DynamicVar>(new DynamicVar[2]
      {
        new DynamicVar("ExhaustAmount", 5M),
        (DynamicVar) new CardsVar(1)
      });
    }
  }

  public override async Task AfterCardExhausted(
    PlayerChoiceContext choiceContext,
    CardModel card,
    bool causedByEthereal)
  {
    if (card.Owner != this.Owner)
      return;
    if (causedByEthereal)
    {
      this.EtherealCount++;
    }
    else
    {
      this.CardsExhausted++;
      await this.DrawIfThresholdMet(choiceContext);
    }
  }

  public override async Task AfterSideTurnEnd(
    PlayerChoiceContext choiceContext,
    CombatSide side,
    IEnumerable<Creature> participants)
  {
    if (!participants.Contains<Creature>(this.Owner.Creature))
      return;
    this.CardsExhausted += this.EtherealCount;
    this.EtherealCount = 0;
    await this.DrawIfThresholdMet(choiceContext);
  }

  private async Task DrawIfThresholdMet(PlayerChoiceContext choiceContext)
  {
    if ((Decimal) this.CardsExhausted < this.DynamicVars["ExhaustAmount"].BaseValue)
      return;
    TaskHelper.RunSafely(this.DoActivateVisuals());
    IEnumerable<CardModel> cardModels = await CardPileCmd.Draw(choiceContext, (Decimal) (int) ((Decimal) this.CardsExhausted / this.DynamicVars["ExhaustAmount"].BaseValue), this.Owner);
    this.CardsExhausted %= this.DynamicVars["ExhaustAmount"].IntValue;
  }

  private async Task DoActivateVisuals()
  {
    this.IsActivating = true;
    this.Flash();
    await Cmd.Wait(1f);
    this.IsActivating = false;
  }

  public override Task AfterCombatEnd(CombatRoom room)
  {
    this.EtherealCount = 0;
    return Task.CompletedTask;
  }
}
