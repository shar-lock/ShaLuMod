// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Monsters.LivingShield
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Audio;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.Ascension;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.MonsterMoves.Intents;
using MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Monsters;

public sealed class LivingShield : MonsterModel
{
  public override int MinInitialHp
  {
    get => AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 65, 55);
  }

  public override int MaxInitialHp => this.MinInitialHp;

  public override DamageSfxType TakeDamageSfxType => DamageSfxType.Armor;

  public override bool HasDeathSfx => false;

  private int ShieldSlamDamage => 6;

  private int SmashDamage
  {
    get => AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 18, 16 /*0x10*/);
  }

  private int EnrageStr => 3;

  public override async Task AfterAddedToRoom()
  {
    await base.AfterAddedToRoom();
    RampartPower rampartPower = await PowerCmd.Apply<RampartPower>((PlayerChoiceContext) new ThrowingPlayerChoiceContext(), this.Creature, 25M, this.Creature, (CardModel) null);
  }

  protected override MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine.MonsterMoveStateMachine GenerateMoveStateMachine()
  {
    List<MonsterState> states = new List<MonsterState>();
    MoveState moveState = new MoveState("SHIELD_SLAM_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.ShieldSlamMove), new AbstractIntent[1]
    {
      (AbstractIntent) new SingleAttackIntent(this.ShieldSlamDamage)
    });
    ConditionalBranchState conditionalBranchState = new ConditionalBranchState("SHIELD_SLAM_BRANCH");
    MoveState move = new MoveState("SMASH_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.SmashMove), new AbstractIntent[2]
    {
      (AbstractIntent) new SingleAttackIntent(this.SmashDamage),
      (AbstractIntent) new BuffIntent()
    });
    moveState.FollowUpState = (MonsterState) conditionalBranchState;
    conditionalBranchState.AddState((MonsterState) moveState, (Func<bool>) (() => this.GetAllyCount() > 0));
    conditionalBranchState.AddState((MonsterState) move, (Func<bool>) (() => this.GetAllyCount() == 0));
    move.FollowUpState = (MonsterState) move;
    states.Add((MonsterState) moveState);
    states.Add((MonsterState) move);
    states.Add((MonsterState) conditionalBranchState);
    return new MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine.MonsterMoveStateMachine((IEnumerable<MonsterState>) states, (MonsterState) moveState);
  }

  private async Task ShieldSlamMove(IReadOnlyList<Creature> targets)
  {
    AttackCommand attackCommand = await DamageCmd.Attack((Decimal) this.ShieldSlamDamage).FromMonster((MonsterModel) this).WithAttackerAnim("Attack", 0.3f).WithAttackerFx(sfx: this.AttackSfx).WithHitFx("vfx/vfx_attack_slash").Execute((PlayerChoiceContext) null);
  }

  private async Task SmashMove(IReadOnlyList<Creature> targets)
  {
    AttackCommand attackCommand = await DamageCmd.Attack((Decimal) this.SmashDamage).FromMonster((MonsterModel) this).WithAttackerAnim("Attack", 0.3f).WithAttackerFx(sfx: this.AttackSfx).WithHitFx("vfx/vfx_attack_slash").Execute((PlayerChoiceContext) null);
    StrengthPower strengthPower = await PowerCmd.Apply<StrengthPower>((PlayerChoiceContext) new ThrowingPlayerChoiceContext(), this.Creature, (Decimal) this.EnrageStr, this.Creature, (CardModel) null);
  }

  private int GetAllyCount()
  {
    return this.CombatState.GetTeammatesOf(this.Creature).Count<Creature>((Func<Creature, bool>) (c => c.IsAlive && c != this.Creature));
  }
}
