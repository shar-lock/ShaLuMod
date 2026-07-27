// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Enchantments.Inky
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Enchantments;

public sealed class Inky : EnchantmentModel
{
  public override bool HasExtraCardText => true;

  public override bool ShowAmount => false;

  protected override IEnumerable<DynamicVar> CanonicalVars
  {
    get
    {
      return (IEnumerable<DynamicVar>) new \u003C\u003Ez__ReadOnlyArray<DynamicVar>(new DynamicVar[2]
      {
        (DynamicVar) new DamageVar(1M, ValueProp.Move),
        (DynamicVar) new PowerVar<WeakPower>(1M)
      });
    }
  }

  protected override IEnumerable<IHoverTip> ExtraHoverTips
  {
    get
    {
      return (IEnumerable<IHoverTip>) new \u003C\u003Ez__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromPower<WeakPower>());
    }
  }

  public override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay? cardPlay)
  {
    // ISSUE: object of a compiler-generated type is created
    IReadOnlyList<WeakPower> weakPowerList = await PowerCmd.Apply<WeakPower>(choiceContext, this.Card.TargetType == TargetType.AllEnemies ? (IEnumerable<Creature>) this.Card.CombatState.HittableEnemies : (IEnumerable<Creature>) new \u003C\u003Ez__ReadOnlySingleElementList<Creature>(cardPlay.Target), this.DynamicVars.Weak.BaseValue, this.Card.Owner.Creature, this.Card);
  }

  public override Decimal EnchantDamageAdditive(Decimal originalDamage, ValueProp props)
  {
    return !props.IsPoweredAttack() ? 0M : this.DynamicVars.Damage.BaseValue;
  }
}
