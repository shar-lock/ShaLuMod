// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Relics.Pendulum
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Saves.Runs;
using System.Collections.Generic;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Relics;

public sealed class Pendulum : RelicModel
{
  private const string _turnsKey = "Turns";
  private bool _isActivating;
  private int _turnsSeen;

  public override string FlashSfx => "event:/sfx/ui/relic_activate_draw";

  public override RelicRarity Rarity => RelicRarity.Common;

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
        (DynamicVar) new CardsVar(1),
        new DynamicVar("Turns", 3M)
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
      this.InvokeDisplayAmountChanged();
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
      this.InvokeDisplayAmountChanged();
    }
  }

  public override async Task AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player)
  {
    if (player != this.Owner)
      return;
    this.TurnsSeen = (this.TurnsSeen + 1) % this.DynamicVars["Turns"].IntValue;
    this.Status = this.TurnsSeen == this.DynamicVars["Turns"].IntValue - 1 ? RelicStatus.Active : RelicStatus.Normal;
    if (this.TurnsSeen != 0)
      return;
    TaskHelper.RunSafely(this.DoActivateVisuals());
    IEnumerable<CardModel> cardModels = await CardPileCmd.Draw(choiceContext, this.DynamicVars.Cards.BaseValue, this.Owner);
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
    this.Status = RelicStatus.Normal;
    return Task.CompletedTask;
  }
}
