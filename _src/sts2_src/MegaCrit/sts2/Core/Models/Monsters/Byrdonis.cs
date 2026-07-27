// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Monsters.Byrdonis
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Animation;
using MegaCrit.Sts2.Core.Bindings.MegaSpine;
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

public sealed class Byrdonis : MonsterModel
{
  private const string _angryTrigger = "Angry";

  public override int MinInitialHp
  {
    get => AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 90, 81);
  }

  public override int MaxInitialHp
  {
    get => AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 90, 84);
  }

  private static int PeckDamage
  {
    get => AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 4, 3);
  }

  private static int PeckRepeat
  {
    get => AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 3, 3);
  }

  private static int SwoopDamage
  {
    get => AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 19, 17);
  }

  public override string DeathSfx => "event:/sfx/enemy/enemy_attacks/byrdonis/byrdonis_die";

  public override string TakeDamageSfx => "event:/sfx/enemy/enemy_attacks/byrdonis/byrdonis_hurt";

  public override async Task AfterAddedToRoom()
  {
    await base.AfterAddedToRoom();
    TerritorialPower territorialPower = await PowerCmd.Apply<TerritorialPower>((PlayerChoiceContext) new ThrowingPlayerChoiceContext(), this.Creature, 1M, this.Creature, (CardModel) null);
  }

  protected override MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine.MonsterMoveStateMachine GenerateMoveStateMachine()
  {
    List<MonsterState> states = new List<MonsterState>();
    MoveState moveState = new MoveState("PECK_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.PeckMove), new AbstractIntent[1]
    {
      (AbstractIntent) new MultiAttackIntent(Byrdonis.PeckDamage, Byrdonis.PeckRepeat)
    });
    MoveState initialState = new MoveState("SWOOP_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.SwoopMove), new AbstractIntent[1]
    {
      (AbstractIntent) new SingleAttackIntent(Byrdonis.SwoopDamage)
    });
    initialState.FollowUpState = (MonsterState) moveState;
    moveState.FollowUpState = (MonsterState) initialState;
    states.Add((MonsterState) initialState);
    states.Add((MonsterState) moveState);
    return new MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine.MonsterMoveStateMachine((IEnumerable<MonsterState>) states, (MonsterState) initialState);
  }

  private async Task PeckMove(IReadOnlyList<Creature> targets)
  {
    AttackCommand attackCommand = await DamageCmd.Attack((Decimal) Byrdonis.PeckDamage).WithHitCount(Byrdonis.PeckRepeat).FromMonster((MonsterModel) this).WithAttackerAnim("Attack", 0.4f).WithAttackerFx(sfx: this.AttackSfx).WithHitFx("vfx/vfx_attack_slash").Execute((PlayerChoiceContext) null);
  }

  private async Task SwoopMove(IReadOnlyList<Creature> targets)
  {
    AttackCommand attackCommand = await DamageCmd.Attack((Decimal) Byrdonis.SwoopDamage).FromMonster((MonsterModel) this).WithAttackerAnim("Attack", 0.4f).WithAttackerFx(sfx: this.AttackSfx).WithHitFx("vfx/vfx_attack_slash").Execute((PlayerChoiceContext) null);
  }

  public override CreatureAnimator GenerateAnimator(MegaSprite controller)
  {
    AnimState initialState = new AnimState("idle_loop", true);
    AnimState state1 = new AnimState("hurt");
    AnimState state2 = new AnimState("attack");
    AnimState state3 = new AnimState("die");
    AnimState state4 = new AnimState("get_angry");
    state1.NextState = initialState;
    state2.NextState = initialState;
    state4.NextState = initialState;
    CreatureAnimator animator = new CreatureAnimator(initialState, controller);
    animator.AddAnyState("Angry", state4);
    animator.AddAnyState("Dead", state3);
    animator.AddAnyState("Hit", state1);
    animator.AddAnyState("Attack", state2);
    return animator;
  }
}
