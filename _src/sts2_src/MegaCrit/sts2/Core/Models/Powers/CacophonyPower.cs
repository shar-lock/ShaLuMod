// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Powers.CacophonyPower
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Powers;

public sealed class CacophonyPower : PowerModel
{
  public override PowerType Type => PowerType.Buff;

  public override PowerStackType StackType => PowerStackType.Counter;

  public override int DisplayAmount => this.DynamicVars.Cards.IntValue;

  public override PowerInstanceType InstanceType => PowerInstanceType.Instanced;

  protected override IEnumerable<DynamicVar> CanonicalVars
  {
    get
    {
      return (IEnumerable<DynamicVar>) new \u003C\u003Ez__ReadOnlySingleElementList<DynamicVar>((DynamicVar) new CardsVar(33));
    }
  }

  public override async Task AfterCardDrawn(
    PlayerChoiceContext choiceContext,
    CardModel card,
    bool fromHandDraw)
  {
    CardsVar cards = this.DynamicVars.Cards;
    cards.BaseValue = Decimal.op_Decrement(cards.BaseValue);
    this.InvokeDisplayAmountChanged();
    if (this.DynamicVars.Cards.IntValue > 0)
      return;
    await Cmd.Wait(0.5f);
    Creature target = this.Owner.Player.RunState.Rng.CombatTargets.NextItem<Creature>((IEnumerable<Creature>) this.CombatState.HittableEnemies);
    if (target != null)
    {
      IEnumerable<DamageResult> damageResults = await CreatureCmd.Damage(choiceContext, target, (Decimal) this.Amount, ValueProp.Unpowered, this.Owner);
    }
    this.DynamicVars.Cards.BaseValue = 33M;
    this.InvokeDisplayAmountChanged();
  }
}
