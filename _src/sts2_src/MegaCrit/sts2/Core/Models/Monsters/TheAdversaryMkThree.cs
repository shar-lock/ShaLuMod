// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Monsters.TheAdversaryMkThree
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
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Monsters;

public sealed class TheAdversaryMkThree : MonsterModel
{
  public override int MinInitialHp
  {
    get => AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 300, 300);
  }

  public override int MaxInitialHp => this.MinInitialHp;

  private int CrashDamage => 15;

  private int FlameBeamDamage => 18;

  private int BarrageDamage => 10;

  private int BarrageRepeat => 2;

  public override DamageSfxType TakeDamageSfxType => DamageSfxType.Armor;

  public override async Task AfterAddedToRoom()
  {
    await base.AfterAddedToRoom();
    ArtifactPower artifactPower = await PowerCmd.Apply<ArtifactPower>((PlayerChoiceContext) new ThrowingPlayerChoiceContext(), this.Creature, 2M, this.Creature, (CardModel) null);
  }

  protected override MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine.MonsterMoveStateMachine GenerateMoveStateMachine()
  {
    List<MonsterState> states = new List<MonsterState>();
    MoveState initialState = new MoveState("CRASH_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.CrashMove), new AbstractIntent[1]
    {
      (AbstractIntent) new SingleAttackIntent(this.CrashDamage)
    });
    MoveState moveState1 = new MoveState("FLAME_BEAM_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.FlameBeamMove), new AbstractIntent[1]
    {
      (AbstractIntent) new SingleAttackIntent(this.FlameBeamDamage)
    });
    MoveState moveState2 = new MoveState("BARRAGE_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.BarrageMove), new AbstractIntent[2]
    {
      (AbstractIntent) new MultiAttackIntent(this.BarrageDamage, this.BarrageRepeat),
      (AbstractIntent) new BuffIntent()
    });
    initialState.FollowUpState = (MonsterState) moveState1;
    moveState1.FollowUpState = (MonsterState) moveState2;
    moveState2.FollowUpState = (MonsterState) initialState;
    states.Add((MonsterState) initialState);
    states.Add((MonsterState) moveState1);
    states.Add((MonsterState) moveState2);
    return new MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine.MonsterMoveStateMachine((IEnumerable<MonsterState>) states, (MonsterState) initialState);
  }

  private async Task CrashMove(IReadOnlyList<Creature> targets)
  {
    AttackCommand attackCommand = await DamageCmd.Attack((Decimal) this.CrashDamage).FromMonster((MonsterModel) this).WithAttackerAnim("Attack", 0.15f).WithAttackerFx(sfx: this.AttackSfx).WithHitFx("vfx/vfx_attack_slash").Execute((PlayerChoiceContext) null);
  }

  private async Task FlameBeamMove(IReadOnlyList<Creature> targets)
  {
    AttackCommand attackCommand = await DamageCmd.Attack((Decimal) this.FlameBeamDamage).FromMonster((MonsterModel) this).WithAttackerAnim("Attack", 0.15f).WithAttackerFx(sfx: this.AttackSfx).WithHitFx("vfx/vfx_attack_slash").Execute((PlayerChoiceContext) null);
  }

  private async Task BarrageMove(IReadOnlyList<Creature> targets)
  {
    AttackCommand attackCommand = await DamageCmd.Attack((Decimal) this.BarrageDamage).WithHitCount(this.BarrageRepeat).FromMonster((MonsterModel) this).WithAttackerAnim("Attack", 0.15f).WithAttackerFx(sfx: this.AttackSfx).WithHitFx("vfx/vfx_attack_slash").Execute((PlayerChoiceContext) null);
    StrengthPower strengthPower = await PowerCmd.Apply<StrengthPower>((PlayerChoiceContext) new ThrowingPlayerChoiceContext(), this.Creature, 4M, this.Creature, (CardModel) null);
  }
}
