// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Powers.CrimsonMantlePower
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Powers;

public sealed class CrimsonMantlePower : PowerModel
{
  private const string _selfDamageKey = "SelfDamage";

  public override PowerType Type => PowerType.Buff;

  public override PowerStackType StackType => PowerStackType.Counter;

  protected override IEnumerable<IHoverTip> ExtraHoverTips
  {
    get
    {
      return (IEnumerable<IHoverTip>) new \u003C\u003Ez__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.Static(StaticHoverTip.Block));
    }
  }

  protected override IEnumerable<DynamicVar> CanonicalVars
  {
    get
    {
      return (IEnumerable<DynamicVar>) new \u003C\u003Ez__ReadOnlySingleElementList<DynamicVar>((DynamicVar) new DamageVar("SelfDamage", 0M, ValueProp.Unblockable | ValueProp.Unpowered));
    }
  }

  public override async Task AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player)
  {
    if (player != this.Owner.Player)
      return;
    this.Flash();
    DamageVar dynamicVar = (DamageVar) this.DynamicVars["SelfDamage"];
    IEnumerable<DamageResult> damageResults = await CreatureCmd.Damage(choiceContext, this.Owner, dynamicVar.BaseValue, dynamicVar.Props, this.Owner);
    Decimal num = await CreatureCmd.GainBlock(this.Owner, (Decimal) this.Amount, ValueProp.Unpowered, (CardPlay) null);
  }

  public void IncrementSelfDamage()
  {
    this.AssertMutable();
    ++this.DynamicVars["SelfDamage"].BaseValue;
  }
}
