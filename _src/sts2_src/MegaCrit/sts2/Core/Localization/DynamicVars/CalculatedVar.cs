// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Localization.DynamicVars.CalculatedVar
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Models;
using System;

#nullable enable
namespace MegaCrit.Sts2.Core.Localization.DynamicVars;

public class CalculatedVar(string name) : DynamicVar(name, 0M)
{
  private Func<CardModel, Creature?, Decimal>? _multiplierCalc;

  public override void SetOwner(AbstractModel owner)
  {
    base.SetOwner(owner);
    this.UpdateValues();
  }

  public CalculatedVar WithMultiplier(Func<CardModel, Creature?, Decimal> multiplierCalc)
  {
    if (this._multiplierCalc != null)
      throw new InvalidOperationException($"Tried to set extra multiplier calc on {this} twice!");
    this._multiplierCalc = !(multiplierCalc.Target is AbstractModel) ? multiplierCalc : throw new InvalidOperationException("Multiplier calc must be static!");
    return this;
  }

  public Decimal Calculate(Creature? target)
  {
    if (this._multiplierCalc == null)
      throw new InvalidOperationException("Extra multiplier calc must be specified!");
    CardModel owner = (CardModel) this._owner;
    Decimal num = !CombatManager.Instance.IsInProgress || owner.CombatState == null ? 0M : this._multiplierCalc(owner, target);
    return this.GetBaseVar().BaseValue + this.GetExtraVar().BaseValue * num;
  }

  public void RecalculateForUpgradeOrEnchant()
  {
    Decimal baseValue = this.GetBaseVar().BaseValue;
    if (baseValue != this.BaseValue)
      this.WasJustUpgraded = true;
    this.BaseValue = baseValue;
  }

  public override void UpdateCardPreview(
    CardModel card,
    CardPreviewMode previewMode,
    Creature? target,
    bool runGlobalHooks)
  {
    this.PreviewValue = this.Calculate(target);
  }

  protected virtual DynamicVar GetBaseVar()
  {
    return (DynamicVar) ((CardModel) this._owner).DynamicVars.CalculationBase;
  }

  protected virtual DynamicVar GetExtraVar()
  {
    return (DynamicVar) ((CardModel) this._owner).DynamicVars.CalculationExtra;
  }

  protected override Decimal GetBaseValueForIConvertible() => this.Calculate((Creature) null);

  public override string ToString() => this.Calculate((Creature) null).ToString();

  private void UpdateValues()
  {
    if (this._owner == null)
      return;
    this.BaseValue = this.GetBaseVar().BaseValue;
  }
}
