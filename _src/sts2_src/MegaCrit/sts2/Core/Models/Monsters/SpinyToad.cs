// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Monsters.SpinyToad
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Animation;
using MegaCrit.Sts2.Core.Audio;
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

public sealed class SpinyToad : MonsterModel
{
  private const string _spikeTrigger = "Spiked";
  private const string _unSpikeTrigger = "Unspiked";
  private const string _attackHeavySfx = "event:/sfx/enemy/enemy_attacks/spiny_toad/spiny_toad_explode";
  private const string _buffSfx = "event:/sfx/enemy/enemy_attacks/spiny_toad/spiny_toad_protrude";
  private bool _isSpiny;

  public override int MinInitialHp
  {
    get => AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 121, 116);
  }

  public override int MaxInitialHp
  {
    get => AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 124, 119);
  }

  private int LashDamage
  {
    get => AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 19, 17);
  }

  private int ExplosionDamage
  {
    get => AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 25, 23);
  }

  protected override string AttackSfx
  {
    get => "event:/sfx/enemy/enemy_attacks/spiny_toad/spiny_toad_lick";
  }

  public bool IsSpiny
  {
    get => this._isSpiny;
    set
    {
      this.AssertMutable();
      this._isSpiny = value;
    }
  }

  public override string DeathSfx => "event:/sfx/enemy/enemy_attacks/spiny_toad/spiny_toad_die";

  public override DamageSfxType TakeDamageSfxType => DamageSfxType.Armor;

  protected override MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine.MonsterMoveStateMachine GenerateMoveStateMachine()
  {
    List<MonsterState> states = new List<MonsterState>();
    MoveState initialState = new MoveState("PROTRUDING_SPIKES_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.SpikesMove), new AbstractIntent[1]
    {
      (AbstractIntent) new BuffIntent()
    });
    MoveState moveState1 = new MoveState("SPIKE_EXPLOSION_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.ExplosionMove), new AbstractIntent[1]
    {
      (AbstractIntent) new SingleAttackIntent(this.ExplosionDamage)
    });
    MoveState moveState2 = new MoveState("TONGUE_LASH_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.LashMove), new AbstractIntent[1]
    {
      (AbstractIntent) new SingleAttackIntent(this.LashDamage)
    });
    initialState.FollowUpState = (MonsterState) moveState1;
    moveState1.FollowUpState = (MonsterState) moveState2;
    moveState2.FollowUpState = (MonsterState) initialState;
    states.Add((MonsterState) initialState);
    states.Add((MonsterState) moveState1);
    states.Add((MonsterState) moveState2);
    return new MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine.MonsterMoveStateMachine((IEnumerable<MonsterState>) states, (MonsterState) initialState);
  }

  private async Task SpikesMove(IReadOnlyList<Creature> targets)
  {
    SfxCmd.Play("event:/sfx/enemy/enemy_attacks/spiny_toad/spiny_toad_protrude");
    await CreatureCmd.TriggerAnim(this.Creature, "Spiked", 0.5f);
    this.IsSpiny = true;
    ThornsPower thornsPower = await PowerCmd.Apply<ThornsPower>((PlayerChoiceContext) new ThrowingPlayerChoiceContext(), this.Creature, 5M, this.Creature, (CardModel) null);
  }

  private async Task ExplosionMove(IReadOnlyList<Creature> targets)
  {
    this.IsSpiny = false;
    AttackCommand attackCommand = await DamageCmd.Attack((Decimal) this.ExplosionDamage).FromMonster((MonsterModel) this).WithAttackerAnim("Unspiked", 0.7f).WithAttackerFx(sfx: "event:/sfx/enemy/enemy_attacks/spiny_toad/spiny_toad_explode").WithHitFx("vfx/vfx_attack_slash").Execute((PlayerChoiceContext) null);
    ThornsPower thornsPower = await PowerCmd.Apply<ThornsPower>((PlayerChoiceContext) new ThrowingPlayerChoiceContext(), this.Creature, -5M, this.Creature, (CardModel) null);
    await Cmd.Wait(1f);
  }

  private async Task LashMove(IReadOnlyList<Creature> targets)
  {
    AttackCommand attackCommand = await DamageCmd.Attack((Decimal) this.LashDamage).FromMonster((MonsterModel) this).WithAttackerAnim("Attack", 0.3f).WithAttackerFx(sfx: this.AttackSfx).WithHitFx("vfx/vfx_attack_slash").Execute((PlayerChoiceContext) null);
  }

  public override CreatureAnimator GenerateAnimator(MegaSprite controller)
  {
    AnimState animState = new AnimState("idle_loop", true);
    AnimState state1 = new AnimState("hurt");
    AnimState state2 = new AnimState("die");
    AnimState state3 = new AnimState("protrude");
    AnimState state4 = new AnimState("lick");
    AnimState state5 = new AnimState("explode");
    AnimState initialState = new AnimState("idle_naked_loop", true);
    AnimState state6 = new AnimState("hurt_naked");
    AnimState state7 = new AnimState("die_naked");
    initialState.AddBranch("Spiked", state3);
    animState.AddBranch("Unspiked", state5);
    state3.NextState = animState;
    state6.NextState = initialState;
    state6.AddBranch("Spiked", state3);
    state1.NextState = animState;
    state1.AddBranch("Unspiked", state5);
    state4.NextState = initialState;
    state5.NextState = initialState;
    CreatureAnimator animator = new CreatureAnimator(initialState, controller);
    animator.AddAnyState("Attack", state4);
    animator.AddAnyState("Hit", state6, (Func<bool>) (() => !this.IsSpiny));
    animator.AddAnyState("Hit", state1, (Func<bool>) (() => this.IsSpiny));
    animator.AddAnyState("Dead", state7, (Func<bool>) (() => !this.IsSpiny));
    animator.AddAnyState("Dead", state2, (Func<bool>) (() => this.IsSpiny));
    return animator;
  }
}
