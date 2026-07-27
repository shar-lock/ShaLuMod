// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Relics.PollinousCore
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Saves.Runs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Relics;

public sealed class PollinousCore : RelicModel
{
  private const string _turnsKey = "Turns";
  private bool _isActivating;
  private int _turnsSeen;

  public override RelicRarity Rarity => RelicRarity.Event;

  public override bool ShowCounter => true;

  public override int DisplayAmount
  {
    get => !this.IsActivating ? this.TurnsSeen : this.DynamicVars["Turns"].IntValue;
  }

  protected override IEnumerable<DynamicVar> CanonicalVars
  {
    get
    {
      return (IEnumerable<DynamicVar>) new \u003C\u003Ez__ReadOnlyArray<DynamicVar>(new DynamicVar[2]
      {
        (DynamicVar) new CardsVar(2),
        new DynamicVar("Turns", 4M)
      });
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

  [SavedProperty]
  public int TurnsSeen
  {
    get => this._turnsSeen;
    set
    {
      this.AssertMutable();
      this._turnsSeen = value;
      this.UpdateDisplay();
    }
  }

  private void UpdateDisplay()
  {
    if (this.IsActivating)
      this.Status = RelicStatus.Normal;
    else
      this.Status = this.TurnsSeen == this.DynamicVars["Turns"].IntValue - 1 ? RelicStatus.Active : RelicStatus.Normal;
    this.InvokeDisplayAmountChanged();
  }

  public override Task BeforeHandDraw(
    Player player,
    PlayerChoiceContext choiceContext,
    ICombatState combatState)
  {
    if (player != this.Owner)
      return Task.CompletedTask;
    ++this.TurnsSeen;
    return Task.CompletedTask;
  }

  public override Task AfterCombatEnd(CombatRoom _)
  {
    this.Status = RelicStatus.Normal;
    return Task.CompletedTask;
  }

  public override Decimal ModifyHandDraw(Player player, Decimal count)
  {
    return player != this.Owner || this.TurnsSeen < this.DynamicVars["Turns"].IntValue ? count : count + this.DynamicVars.Cards.BaseValue;
  }

  public override Task AfterModifyingHandDraw()
  {
    this.TurnsSeen = 0;
    TaskHelper.RunSafely(this.DoActivateVisuals());
    return Task.CompletedTask;
  }

  private async Task DoActivateVisuals()
  {
    this.IsActivating = true;
    this.Flash();
    await Cmd.Wait(1f);
    this.IsActivating = false;
  }
}
