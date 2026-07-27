// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Powers.IllusionPower
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.Monsters;
using MegaCrit.Sts2.Core.MonsterMoves.Intents;
using MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine;
using MegaCrit.Sts2.Core.Rooms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Powers;

public sealed class IllusionPower : PowerModel
{
  public const string stunTrigger = "StunTrigger";
  public const string wakeUpTrigger = "WakeUpTrigger";
  private string? _followUpStateId;

  public override PowerType Type => PowerType.Buff;

  public override PowerStackType StackType => PowerStackType.Single;

  public override bool ShouldPlayVfx => false;

  public string? FollowUpStateId
  {
    get => this._followUpStateId;
    set
    {
      this.AssertMutable();
      this._followUpStateId = value;
    }
  }

  protected override object InitInternalData() => (object) new IllusionPower.Data();

  public bool IsReviving => this.GetInternalData<IllusionPower.Data>().isReviving;

  public override bool ShouldPowerBeRemovedOnDeath(PowerModel power)
  {
    return power.Type == PowerType.Debuff && !(power is ITemporaryPower);
  }

  public override Task AfterApplied(Creature? applier, CardModel? cardSource)
  {
    return this.Owner.HasPower<MinionPower>() ? Task.CompletedTask : (Task) PowerCmd.Apply<MinionPower>((PlayerChoiceContext) new ThrowingPlayerChoiceContext(), this.Owner, 1M, (Creature) null, (CardModel) null);
  }

  public override async Task AfterDeath(
    PlayerChoiceContext choiceContext,
    Creature creature,
    bool wasRemovalPrevented,
    float deathAnimLength)
  {
    if (wasRemovalPrevented || creature != this.Owner)
      return;
    await CreatureCmd.TriggerAnim(this.Owner, "StunTrigger", 0.0f);
    this.GetInternalData<IllusionPower.Data>().isReviving = true;
    this.Owner.Monster.SetMoveImmediate(new MoveState("REVIVE_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.ReviveMove), new AbstractIntent[1]
    {
      (AbstractIntent) new HealIntent()
    })
    {
      FollowUpStateId = this.FollowUpStateId ?? this.Owner.Monster.MoveStateMachine.StateLog.Last<MonsterState>().Id,
      MustPerformOnceBeforeTransitioning = true
    });
  }

  public override bool ShouldAllowHitting(Creature creature)
  {
    return creature != this.Owner || !this.IsReviving;
  }

  public override bool ShouldCreatureBeRemovedFromCombatAfterDeath(Creature creature)
  {
    return creature != this.Owner;
  }

  public override async Task AfterCombatEnd(CombatRoom room)
  {
    if (this.Owner.IsAlive)
      return;
    await CreatureCmd.TriggerAnim(this.Owner, "Dead", 0.1f);
  }

  private async Task ReviveMove(IReadOnlyList<Creature> targets)
  {
    await CreatureCmd.TriggerAnim(this.Owner, "WakeUpTrigger", 0.0f);
    this.GetInternalData<IllusionPower.Data>().isReviving = false;
    await CreatureCmd.Heal(this.Owner, (Decimal) (this.Owner.MaxHp - this.Owner.CurrentHp));
    if (!(this.Owner.Monster is Parafright))
      return;
    SfxCmd.Play("event:/sfx/enemy/enemy_attacks/obscura/obscura_hologram_heal");
  }

  private class Data
  {
    public bool isReviving;
  }
}
