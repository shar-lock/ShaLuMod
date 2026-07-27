// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Relics.OrnamentalFan
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
using MegaCrit.Sts2.Core.ValueProps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Relics;

public sealed class OrnamentalFan : RelicModel
{
  private bool _isActivating;
  private int _attacksPlayedThisTurn;

  public override RelicRarity Rarity => RelicRarity.Uncommon;

  public override bool ShowCounter => CombatManager.Instance.IsInProgress;

  public override int DisplayAmount
  {
    get
    {
      return !this.IsActivating ? this.AttacksPlayedThisTurn % this.DynamicVars.Cards.IntValue : this.DynamicVars.Cards.IntValue;
    }
  }

  protected override IEnumerable<DynamicVar> CanonicalVars
  {
    get
    {
      return (IEnumerable<DynamicVar>) new \u003C\u003Ez__ReadOnlyArray<DynamicVar>(new DynamicVar[2]
      {
        (DynamicVar) new CardsVar(3),
        (DynamicVar) new BlockVar(4M, ValueProp.Unpowered)
      });
    }
  }

  protected override IEnumerable<IHoverTip> ExtraHoverTips
  {
    get
    {
      return (IEnumerable<IHoverTip>) new \u003C\u003Ez__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.Static(StaticHoverTip.Block));
    }
  }

  private bool IsActivating
  {
    get => this._isActivating;
    set
    {
      this.AssertMutable();
      this._isActivating = value;
      this.UpdateDisplay();
    }
  }

  private int AttacksPlayedThisTurn
  {
    get => this._attacksPlayedThisTurn;
    set
    {
      this.AssertMutable();
      this._attacksPlayedThisTurn = value;
      this.UpdateDisplay();
    }
  }

  private void UpdateDisplay()
  {
    if (this.IsActivating || !CombatManager.Instance.IsInProgress)
    {
      this.Status = RelicStatus.Normal;
    }
    else
    {
      int intValue = this.DynamicVars.Cards.IntValue;
      this.Status = this.AttacksPlayedThisTurn % intValue == intValue - 1 ? RelicStatus.Active : RelicStatus.Normal;
    }
    this.InvokeDisplayAmountChanged();
  }

  public override Task BeforeSideTurnStart(
    PlayerChoiceContext choiceContext,
    CombatSide side,
    IReadOnlyList<Creature> participants,
    ICombatState combatState)
  {
    if (!participants.Contains<Creature>(this.Owner.Creature))
      return Task.CompletedTask;
    this.AttacksPlayedThisTurn = 0;
    this.Status = RelicStatus.Normal;
    return Task.CompletedTask;
  }

  public override async Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
  {
    if (cardPlay.Card.Owner != this.Owner || !CombatManager.Instance.IsInProgress || cardPlay.Card.Type != CardType.Attack)
      return;
    this.AttacksPlayedThisTurn++;
    if (this.AttacksPlayedThisTurn % this.DynamicVars.Cards.IntValue != 0)
      return;
    TaskHelper.RunSafely(this.DoActivateVisuals());
    Decimal num = await CreatureCmd.GainBlock(this.Owner.Creature, this.DynamicVars.Block, (CardPlay) null);
  }

  private async Task DoActivateVisuals()
  {
    this.IsActivating = true;
    this.Flash();
    await Cmd.Wait(1f);
    this.IsActivating = false;
  }

  public override Task AfterCombatEnd(CombatRoom _)
  {
    this.IsActivating = false;
    return Task.CompletedTask;
  }
}
