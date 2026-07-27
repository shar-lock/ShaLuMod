// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Relics.GalacticDust
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Saves.Runs;
using MegaCrit.Sts2.Core.ValueProps;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Relics;

public sealed class GalacticDust : RelicModel
{
  private bool _isActivating;
  private int _starsSpent;

  public override bool ShowCounter => true;

  public override int DisplayAmount
  {
    get
    {
      return !this.IsActivating ? this.StarsSpent % this.DynamicVars.Stars.IntValue : this.DynamicVars.Stars.IntValue;
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
  public int StarsSpent
  {
    get => this._starsSpent;
    set
    {
      this.AssertMutable();
      this._starsSpent = value;
      this.UpdateDisplay();
    }
  }

  private void UpdateDisplay()
  {
    if (this.IsActivating)
      this.Status = RelicStatus.Normal;
    else
      this.Status = this.StarsSpent == this.DynamicVars.Stars.IntValue - 1 ? RelicStatus.Active : RelicStatus.Normal;
    this.InvokeDisplayAmountChanged();
  }

  protected override IEnumerable<IHoverTip> ExtraHoverTips
  {
    get
    {
      return (IEnumerable<IHoverTip>) new \u003C\u003Ez__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.Static(StaticHoverTip.Block));
    }
  }

  public override RelicRarity Rarity => RelicRarity.Uncommon;

  protected override IEnumerable<DynamicVar> CanonicalVars
  {
    get
    {
      return (IEnumerable<DynamicVar>) new \u003C\u003Ez__ReadOnlyArray<DynamicVar>(new DynamicVar[2]
      {
        (DynamicVar) new StarsVar(10),
        (DynamicVar) new BlockVar(10M, ValueProp.Unpowered)
      });
    }
  }

  public override async Task AfterStarsSpent(int amount, Player spender)
  {
    if (spender != this.Owner)
      return;
    this.StarsSpent += amount;
    if (this.StarsSpent >= this.DynamicVars.Stars.IntValue)
    {
      TaskHelper.RunSafely(this.DoActivateVisuals());
      Decimal num = await CreatureCmd.GainBlock(this.Owner.Creature, (Decimal) (Mathf.FloorToInt((float) this.StarsSpent / (float) this.DynamicVars.Stars.IntValue) * this.DynamicVars.Block.IntValue), ValueProp.Unpowered, (CardPlay) null);
      this.StarsSpent %= this.DynamicVars.Stars.IntValue;
    }
    this.InvokeDisplayAmountChanged();
  }

  private async Task DoActivateVisuals()
  {
    this.IsActivating = true;
    this.Flash();
    await Cmd.Wait(1f);
    this.IsActivating = false;
  }
}
