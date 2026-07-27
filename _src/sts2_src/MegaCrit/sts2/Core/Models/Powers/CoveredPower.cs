// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Powers.CoveredPower
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
using MegaCrit.Sts2.Core.Platform;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.ValueProps;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Powers;

public sealed class CoveredPower : PowerModel
{
  private const string _applierTag = "Applier";

  public override PowerType Type => PowerType.Buff;

  public override PowerStackType StackType => PowerStackType.Single;

  public override PowerInstanceType InstanceType => PowerInstanceType.Instanced;

  protected override IEnumerable<DynamicVar> CanonicalVars
  {
    get
    {
      return (IEnumerable<DynamicVar>) new \u003C\u003Ez__ReadOnlySingleElementList<DynamicVar>((DynamicVar) new StringVar("Applier"));
    }
  }

  public override async Task AfterApplied(Creature? applier, CardModel? cardSource)
  {
    ((StringVar) this.DynamicVars["Applier"]).StringValue = PlatformUtil.GetPlayerName(RunManager.Instance.NetService.Platform, this.Applier.Player.NetId);
    InterceptPower interceptPower = this.Applier.GetPower<InterceptPower>();
    if (interceptPower == null)
      interceptPower = await PowerCmd.Apply<InterceptPower>((PlayerChoiceContext) new ThrowingPlayerChoiceContext(), this.Applier, 1M, this.Owner, (CardModel) null);
    interceptPower.AddCoveredCreature(this.Owner);
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
    return target != this.Owner || !props.IsPoweredAttack() ? 1M : 0M;
  }

  public override async Task AfterSideTurnEnd(
    PlayerChoiceContext choiceContext,
    CombatSide side,
    IEnumerable<Creature> participants)
  {
    if (side != CombatSide.Enemy)
      return;
    await PowerCmd.Remove((PowerModel) this);
  }
}
