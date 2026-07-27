// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Relics.RedSkull
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Rooms;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Relics;

public sealed class RedSkull : RelicModel
{
  private const string _hpThresholdKey = "HpThreshold";
  private bool _strengthApplied;

  public override RelicRarity Rarity => RelicRarity.Common;

  protected override IEnumerable<DynamicVar> CanonicalVars
  {
    get
    {
      return (IEnumerable<DynamicVar>) new \u003C\u003Ez__ReadOnlyArray<DynamicVar>(new DynamicVar[2]
      {
        new DynamicVar("HpThreshold", 50M),
        (DynamicVar) new PowerVar<StrengthPower>(3M)
      });
    }
  }

  protected override IEnumerable<IHoverTip> ExtraHoverTips
  {
    get
    {
      return (IEnumerable<IHoverTip>) new \u003C\u003Ez__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromPower<StrengthPower>());
    }
  }

  private bool StrengthApplied
  {
    get => this._strengthApplied;
    set
    {
      this.AssertMutable();
      this._strengthApplied = value;
    }
  }

  public override async Task AfterRoomEntered(AbstractRoom room)
  {
    if (!(room is CombatRoom))
      return;
    await this.ModifyStrengthIfNecessary();
  }

  public override Task AfterCombatEnd(CombatRoom _)
  {
    this.StrengthApplied = false;
    this.Status = RelicStatus.Normal;
    return Task.CompletedTask;
  }

  public override async Task AfterCurrentHpChanged(Creature creature, Decimal _)
  {
    if (!CombatManager.Instance.IsInProgress)
      return;
    await this.ModifyStrengthIfNecessary();
  }

  private async Task ModifyStrengthIfNecessary()
  {
    Creature creature = this.Owner.Creature;
    bool flag = (Decimal) creature.CurrentHp > (Decimal) creature.MaxHp * (this.DynamicVars["HpThreshold"].BaseValue / 100M);
    this.Status = flag ? RelicStatus.Normal : RelicStatus.Active;
    Decimal baseValue = this.DynamicVars.Strength.BaseValue;
    if (flag && this.StrengthApplied)
    {
      this.Flash();
      StrengthPower strengthPower = await PowerCmd.Apply<StrengthPower>((PlayerChoiceContext) new ThrowingPlayerChoiceContext(), creature, -baseValue, creature, (CardModel) null);
      this.StrengthApplied = false;
    }
    else
    {
      if (flag || this.StrengthApplied)
        return;
      this.Flash();
      StrengthPower strengthPower = await PowerCmd.Apply<StrengthPower>((PlayerChoiceContext) new ThrowingPlayerChoiceContext(), creature, baseValue, creature, (CardModel) null);
      this.StrengthApplied = true;
    }
  }
}
