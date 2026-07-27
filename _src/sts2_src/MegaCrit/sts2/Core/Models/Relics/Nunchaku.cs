// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Relics.Nunchaku
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Saves.Runs;
using System.Collections.Generic;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Relics;

public sealed class Nunchaku : RelicModel
{
  private bool _isActivating;
  private int _attacksPlayed;

  public override RelicRarity Rarity => RelicRarity.Uncommon;

  public override bool ShowCounter => true;

  public override int DisplayAmount
  {
    get
    {
      return !this.IsActivating ? this.AttacksPlayed % this.DynamicVars.Cards.IntValue : this.DynamicVars.Cards.IntValue;
    }
  }

  protected override IEnumerable<DynamicVar> CanonicalVars
  {
    get
    {
      return (IEnumerable<DynamicVar>) new \u003C\u003Ez__ReadOnlyArray<DynamicVar>(new DynamicVar[2]
      {
        (DynamicVar) new CardsVar(10),
        (DynamicVar) new EnergyVar(1)
      });
    }
  }

  protected override IEnumerable<IHoverTip> ExtraHoverTips
  {
    get
    {
      return (IEnumerable<IHoverTip>) new \u003C\u003Ez__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.ForEnergy((RelicModel) this));
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
  public int AttacksPlayed
  {
    get => this._attacksPlayed;
    set
    {
      this.AssertMutable();
      this._attacksPlayed = value;
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
      this.Status = this.AttacksPlayed % intValue == intValue - 1 ? RelicStatus.Active : RelicStatus.Normal;
    }
    this.InvokeDisplayAmountChanged();
  }

  public override async Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
  {
    if (cardPlay.Card.Owner != this.Owner || cardPlay.Card.Type != CardType.Attack)
      return;
    this.AttacksPlayed++;
    int intValue = this.DynamicVars.Cards.IntValue;
    if (!CombatManager.Instance.IsInProgress || this.AttacksPlayed % intValue != 0)
      return;
    TaskHelper.RunSafely(this.DoActivateVisuals());
    await PlayerCmd.GainEnergy(this.DynamicVars.Energy.BaseValue, this.Owner);
  }

  private async Task DoActivateVisuals()
  {
    this.IsActivating = true;
    this.Flash();
    await Cmd.Wait(1f);
    this.IsActivating = false;
  }
}
