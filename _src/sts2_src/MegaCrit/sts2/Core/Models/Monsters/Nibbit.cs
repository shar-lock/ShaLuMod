// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Monsters.Nibbit
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

public sealed class Nibbit : MonsterModel
{
  private const string _sliceMove = "SLICE_MOVE";
  private bool _isFront;
  private bool _isAlone;

  public override int MinInitialHp
  {
    get => AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 44, 42);
  }

  public override int MaxInitialHp
  {
    get => AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 48 /*0x30*/, 46);
  }

  private int ButtDamage
  {
    get => AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 13, 12);
  }

  private int SliceBlock => AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 6, 5);

  private int SliceDamage
  {
    get => AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 7, 6);
  }

  private int HissStrengthGain
  {
    get => AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 3, 2);
  }

  public override string DeathSfx => "event:/sfx/enemy/enemy_attacks/nibbit/nibbit_die";

  public override DamageSfxType TakeDamageSfxType => DamageSfxType.Slime;

  public bool IsFront
  {
    get => this._isFront;
    set
    {
      this.AssertMutable();
      this._isFront = value;
    }
  }

  public bool IsAlone
  {
    get => this._isAlone;
    set
    {
      this.AssertMutable();
      this._isAlone = value;
    }
  }

  protected override MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine.MonsterMoveStateMachine GenerateMoveStateMachine()
  {
    List<MonsterState> states = new List<MonsterState>();
    MoveState move1 = new MoveState("BUTT_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.ButtMove), new AbstractIntent[1]
    {
      (AbstractIntent) new SingleAttackIntent(this.ButtDamage)
    });
    MoveState move2 = new MoveState("SLICE_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.SliceMove), new AbstractIntent[2]
    {
      (AbstractIntent) new SingleAttackIntent(this.SliceDamage),
      (AbstractIntent) new DefendIntent()
    });
    MoveState move3 = new MoveState("HISS_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.HissMove), new AbstractIntent[1]
    {
      (AbstractIntent) new BuffIntent()
    });
    ConditionalBranchState initialState = new ConditionalBranchState("INIT_MOVE");
    if (this._isAlone)
    {
      initialState.AddState((MonsterState) move1, (Func<bool>) (() => ((Nibbit) this.Creature.Monster).IsAlone));
    }
    else
    {
      initialState.AddState((MonsterState) move3, (Func<bool>) (() => !((Nibbit) this.Creature.Monster).IsFront));
      initialState.AddState((MonsterState) move2, (Func<bool>) (() => ((Nibbit) this.Creature.Monster).IsFront));
    }
    move2.FollowUpState = (MonsterState) move3;
    move1.FollowUpState = (MonsterState) move2;
    move3.FollowUpState = (MonsterState) move1;
    states.Add((MonsterState) initialState);
    states.Add((MonsterState) move1);
    states.Add((MonsterState) move2);
    states.Add((MonsterState) move3);
    return new MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine.MonsterMoveStateMachine((IEnumerable<MonsterState>) states, (MonsterState) initialState);
  }

  private async Task ButtMove(IReadOnlyList<Creature> targets)
  {
    AttackCommand attackCommand = await DamageCmd.Attack((Decimal) this.ButtDamage).FromMonster((MonsterModel) this).WithAttackerAnim("Attack", 0.15f).WithAttackerFx(sfx: this.AttackSfx).WithHitFx("vfx/vfx_attack_slash").Execute((PlayerChoiceContext) null);
  }

  private async Task SliceMove(IReadOnlyList<Creature> targets)
  {
    AttackCommand attackCommand = await DamageCmd.Attack((Decimal) this.SliceDamage).FromMonster((MonsterModel) this).WithAttackerAnim("Attack", 0.15f).WithAttackerFx(sfx: this.AttackSfx).WithHitFx("vfx/vfx_attack_slash").Execute((PlayerChoiceContext) null);
    Decimal num = await CreatureCmd.GainBlock(this.Creature, (Decimal) this.SliceBlock, ValueProp.Move, (CardPlay) null);
  }

  private async Task HissMove(IReadOnlyList<Creature> targets)
  {
    await CreatureCmd.TriggerAnim(this.Creature, "Cast", 0.6f);
    StrengthPower strengthPower = await PowerCmd.Apply<StrengthPower>((PlayerChoiceContext) new ThrowingPlayerChoiceContext(), this.Creature, (Decimal) this.HissStrengthGain, this.Creature, (CardModel) null);
  }

  public override CreatureAnimator GenerateAnimator(MegaSprite controller)
  {
    AnimState initialState = new AnimState("idle_loop", true);
    AnimState state1 = new AnimState("hiss");
    AnimState state2 = new AnimState("attack");
    AnimState state3 = new AnimState("hurt");
    AnimState state4 = new AnimState("die");
    state1.NextState = initialState;
    state2.NextState = initialState;
    state3.NextState = initialState;
    CreatureAnimator animator = new CreatureAnimator(initialState, controller);
    animator.AddAnyState("Cast", state1);
    animator.AddAnyState("Attack", state2);
    animator.AddAnyState("Dead", state4);
    animator.AddAnyState("Hit", state3);
    return animator;
  }

  protected override bool ShouldShowMoveInBestiary(string moveStateId)
  {
    return moveStateId != "SLICE_MOVE";
  }
}
