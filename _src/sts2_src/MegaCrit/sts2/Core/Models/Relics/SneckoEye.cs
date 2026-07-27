// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Relics.SneckoEye
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.TestSupport;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Relics;

public sealed class SneckoEye : RelicModel
{
  private int _testEnergyCostOverride = -1;

  public override RelicRarity Rarity => RelicRarity.Ancient;

  protected override IEnumerable<DynamicVar> CanonicalVars
  {
    get
    {
      return (IEnumerable<DynamicVar>) new \u003C\u003Ez__ReadOnlySingleElementList<DynamicVar>((DynamicVar) new CardsVar(2));
    }
  }

  protected override IEnumerable<IHoverTip> ExtraHoverTips
  {
    get
    {
      return (IEnumerable<IHoverTip>) new \u003C\u003Ez__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromPower<ConfusedPower>());
    }
  }

  public override async Task AfterObtained()
  {
    if (!CombatManager.Instance.IsInProgress)
      return;
    await this.ApplyPower();
  }

  public override async Task BeforeCombatStart() => await this.ApplyPower();

  public override Decimal ModifyHandDraw(Player player, Decimal count)
  {
    return player != this.Owner ? count : count + this.DynamicVars.Cards.BaseValue;
  }

  private async Task ApplyPower()
  {
    ConfusedPower confusedPower = await PowerCmd.Apply<ConfusedPower>((PlayerChoiceContext) new ThrowingPlayerChoiceContext(), this.Owner.Creature, 1M, this.Owner.Creature, (CardModel) null);
    this.ApplyTestEnergyCostOverrideToPower();
  }

  public void SetTestEnergyCostOverride(int value)
  {
    TestMode.AssertOn();
    this.AssertMutable();
    this._testEnergyCostOverride = value;
    this.ApplyTestEnergyCostOverrideToPower();
  }

  private void ApplyTestEnergyCostOverrideToPower()
  {
    if (this._testEnergyCostOverride < 0)
      return;
    ConfusedPower power = this.Owner.Creature.GetPower<ConfusedPower>();
    if (power == null)
      return;
    power.TestEnergyCostOverride = this._testEnergyCostOverride;
  }
}
