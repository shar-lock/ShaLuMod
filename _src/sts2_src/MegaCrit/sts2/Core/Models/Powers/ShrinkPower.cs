// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Powers.ShrinkPower
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Monsters;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.ValueProps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Powers;

public sealed class ShrinkPower : PowerModel
{
  public const Decimal damageDecrease = 30M;
  private const string _damageDecreaseKey = "DamageDecrease";
  private const string _applierNameKey = "ApplierName";

  public override PowerType Type => PowerType.Debuff;

  public override PowerStackType StackType
  {
    get => !this.IsInfinite ? PowerStackType.Counter : PowerStackType.Single;
  }

  public override bool AllowNegative => true;

  private bool IsInfinite => this.Amount < 0;

  protected override IEnumerable<DynamicVar> CanonicalVars
  {
    get
    {
      return (IEnumerable<DynamicVar>) new \u003C\u003Ez__ReadOnlyArray<DynamicVar>(new DynamicVar[2]
      {
        new DynamicVar("DamageDecrease", 30M),
        (DynamicVar) new StringVar("ApplierName")
      });
    }
  }

  public override Task AfterApplied(Creature? applier, CardModel? cardSource)
  {
    if (this.Owner.Monster is Vantom monster)
      monster.ScaleTo(0.5f, 0.75f);
    else
      NCombatRoom.Instance?.GetCreatureNode(this.Owner)?.ScaleTo(0.5f, 0.75);
    Creature applier1 = this.Applier;
    if (applier1 != null && applier1.IsMonster)
      ((StringVar) this.DynamicVars["ApplierName"]).StringValue = this.Applier.Monster.Title.GetFormattedText();
    return Task.CompletedTask;
  }

  public override Task AfterRemoved(Creature oldOwner)
  {
    if (oldOwner.Monster is Vantom monster)
      monster.ScaleTo(1f, 0.75f);
    else
      NCombatRoom.Instance?.GetCreatureNode(oldOwner)?.ScaleTo(1f, 0.75);
    return Task.CompletedTask;
  }

  public override async Task AfterSideTurnEnd(
    PlayerChoiceContext choiceContext,
    CombatSide side,
    IEnumerable<Creature> participants)
  {
    if (this.IsInfinite || !participants.Contains<Creature>(this.Owner))
      return;
    await PowerCmd.Decrement((PowerModel) this);
  }

  public override async Task AfterDeath(
    PlayerChoiceContext choiceContext,
    Creature creature,
    bool wasRemovalPrevented,
    float deathAnimLength)
  {
    if (wasRemovalPrevented || creature != this.Applier)
      return;
    await PowerCmd.Remove((PowerModel) this);
  }

  public override Decimal ModifyDamageMultiplicative(
    Creature? target,
    Decimal amount,
    ValueProp props,
    Creature? dealer,
    CardModel? cardSource,
    CardPlay? cardPlay)
  {
    return this.Owner != dealer || !props.IsPoweredAttack() ? 1M : (100M - this.DynamicVars["DamageDecrease"].BaseValue) / 100M;
  }
}
