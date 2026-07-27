// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Monsters.Seapunk
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Animation;
using MegaCrit.Sts2.Core.Audio;
using MegaCrit.Sts2.Core.Bindings.MegaSpine;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.Ascension;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.MonsterMoves.Intents;
using MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine;
using MegaCrit.Sts2.Core.ValueProps;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Monsters;

public sealed class Seapunk : MonsterModel
{
  private const string _multiAttackTrigger = "MultiAttack";
  private const string _buffSfx = "event:/sfx/enemy/enemy_attacks/seapunk/seapunk_buff";
  private const string _kickSfx = "event:/sfx/enemy/enemy_attacks/seapunk/seapunk_kick";
  private const string _kickMultiSfx = "event:/sfx/enemy/enemy_attacks/seapunk/seapunk_kick_multi";

  public override int MinInitialHp
  {
    get => AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 47, 44);
  }

  public override int MaxInitialHp
  {
    get => AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 49, 46);
  }

  private int SeaKickDamage
  {
    get => AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 13, 11);
  }

  private int SpinningKickDamage => 2;

  private int SpinningKickRepeat => 4;

  private int BubbleBlock => AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 8, 7);

  private int BubbleStr => AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 2, 1);

  public override DamageSfxType TakeDamageSfxType => DamageSfxType.Fur;

  public override string HurtSfx => "event:/sfx/enemy/enemy_attacks/seapunk/seapunk_hurt";

  public override async Task AfterAddedToRoom() => await base.AfterAddedToRoom();

  protected override MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine.MonsterMoveStateMachine GenerateMoveStateMachine()
  {
    List<MonsterState> states = new List<MonsterState>();
    MoveState initialState = new MoveState("SEA_KICK_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.SeaKickMove), new AbstractIntent[1]
    {
      (AbstractIntent) new SingleAttackIntent(this.SeaKickDamage)
    });
    MoveState moveState1 = new MoveState("SPINNING_KICK_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.SpinningKickMove), new AbstractIntent[1]
    {
      (AbstractIntent) new MultiAttackIntent(this.SpinningKickDamage, this.SpinningKickRepeat)
    });
    MoveState moveState2 = new MoveState("BUBBLE_BURP_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.BubbleBurpMove), new AbstractIntent[2]
    {
      (AbstractIntent) new BuffIntent(),
      (AbstractIntent) new DefendIntent()
    });
    initialState.FollowUpState = (MonsterState) moveState1;
    moveState1.FollowUpState = (MonsterState) moveState2;
    moveState2.FollowUpState = (MonsterState) initialState;
    states.Add((MonsterState) initialState);
    states.Add((MonsterState) moveState1);
    states.Add((MonsterState) moveState2);
    return new MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine.MonsterMoveStateMachine((IEnumerable<MonsterState>) states, (MonsterState) initialState);
  }

  private async Task SeaKickMove(IReadOnlyList<Creature> targets)
  {
    AttackCommand attackCommand = await DamageCmd.Attack((Decimal) this.SeaKickDamage).FromMonster((MonsterModel) this).WithAttackerAnim("Attack", 0.15f).WithAttackerFx(sfx: "event:/sfx/enemy/enemy_attacks/seapunk/seapunk_kick").WithHitFx("vfx/vfx_attack_blunt").Execute((PlayerChoiceContext) null);
  }

  private async Task SpinningKickMove(IReadOnlyList<Creature> targets)
  {
    AttackCommand attackCommand = await DamageCmd.Attack((Decimal) this.SpinningKickDamage).WithHitCount(this.SpinningKickRepeat).FromMonster((MonsterModel) this).WithAttackerAnim("MultiAttack", 0.15f).OnlyPlayAnimOnce().WithAttackerFx(sfx: "event:/sfx/enemy/enemy_attacks/seapunk/seapunk_kick_multi").WithHitFx("vfx/vfx_attack_blunt").Execute((PlayerChoiceContext) null);
  }

  private async Task BubbleBurpMove(IReadOnlyList<Creature> targets)
  {
    SfxCmd.Play("event:/sfx/enemy/enemy_attacks/seapunk/seapunk_buff");
    await CreatureCmd.TriggerAnim(this.Creature, "Cast", 0.75f);
    Decimal num = await CreatureCmd.GainBlock(this.Creature, (Decimal) this.BubbleBlock, ValueProp.Move, (CardPlay) null);
    StrengthPower strengthPower = await PowerCmd.Apply<StrengthPower>((PlayerChoiceContext) new ThrowingPlayerChoiceContext(), this.Creature, (Decimal) this.BubbleStr, this.Creature, (CardModel) null);
  }

  public override CreatureAnimator GenerateAnimator(MegaSprite controller)
  {
    AnimState initialState = new AnimState("idle_loop", true);
    AnimState state1 = new AnimState("attack_multi");
    AnimState state2 = new AnimState("cast");
    AnimState state3 = new AnimState("attack");
    AnimState state4 = new AnimState("hurt");
    AnimState state5 = new AnimState("die");
    state2.NextState = initialState;
    state3.NextState = initialState;
    state4.NextState = initialState;
    state1.NextState = initialState;
    CreatureAnimator animator = new CreatureAnimator(initialState, controller);
    animator.AddAnyState("Cast", state2);
    animator.AddAnyState("Attack", state3);
    animator.AddAnyState("Dead", state5);
    animator.AddAnyState("Hit", state4);
    animator.AddAnyState("MultiAttack", state1);
    return animator;
  }
}
