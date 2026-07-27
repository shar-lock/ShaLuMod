// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Relics.IronClub
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Saves.Runs;
using System.Collections.Generic;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Relics;

public sealed class IronClub : RelicModel
{
  private bool _isActivating;
  private int _cardsPlayed;

  public override RelicRarity Rarity => RelicRarity.Ancient;

  public override string FlashSfx => "event:/sfx/ui/relic_activate_draw";

  public override bool ShowCounter => true;

  public override int DisplayAmount
  {
    get
    {
      return !this.IsActivating ? this.CardsPlayed % this.DynamicVars.Cards.IntValue : this.DynamicVars.Cards.IntValue;
    }
  }

  protected override IEnumerable<DynamicVar> CanonicalVars
  {
    get
    {
      return (IEnumerable<DynamicVar>) new \u003C\u003Ez__ReadOnlySingleElementList<DynamicVar>((DynamicVar) new CardsVar(4));
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
  public int CardsPlayed
  {
    get => this._cardsPlayed;
    set
    {
      this.AssertMutable();
      this._cardsPlayed = value;
      this.UpdateDisplay();
    }
  }

  private void UpdateDisplay()
  {
    if (this.IsActivating)
    {
      this.Status = RelicStatus.Normal;
    }
    else
    {
      int intValue = this.DynamicVars.Cards.IntValue;
      this.Status = this.CardsPlayed % intValue == intValue - 1 ? RelicStatus.Active : RelicStatus.Normal;
    }
    this.InvokeDisplayAmountChanged();
  }

  public override async Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
  {
    if (cardPlay.Card.Owner != this.Owner)
      return;
    this.CardsPlayed++;
    int intValue = this.DynamicVars.Cards.IntValue;
    if (!CombatManager.Instance.IsInProgress || this.CardsPlayed % intValue != 0)
      return;
    TaskHelper.RunSafely(this.DoActivateVisuals());
    IEnumerable<CardModel> cardModels = await CardPileCmd.Draw(choiceContext, 1M, this.Owner);
  }

  private async Task DoActivateVisuals()
  {
    this.IsActivating = true;
    this.Flash();
    await Cmd.Wait(1f);
    this.IsActivating = false;
  }
}
