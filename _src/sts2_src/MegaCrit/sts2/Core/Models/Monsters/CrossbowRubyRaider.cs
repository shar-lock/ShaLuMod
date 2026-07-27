// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Monsters.CrossbowRubyRaider
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
using MegaCrit.Sts2.Core.MonsterMoves.Intents;
using MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine;
using MegaCrit.Sts2.Core.ValueProps;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Monsters;

public sealed class CrossbowRubyRaider : MonsterModel
{
  private const string _reloadTrigger = "Reload";
  private const string _reloadSfx = "event:/sfx/enemy/enemy_attacks/crossbow_ruby_raider/crossbow_ruby_raider_reload";
  private bool _isCrossbowReloaded;

  public override int MinInitialHp
  {
    get => AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 19, 18);
  }

  public override int MaxInitialHp
  {
    get => AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 22, 21);
  }

  private int FireDamage
  {
    get => AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 16 /*0x10*/, 14);
  }

  public override DamageSfxType TakeDamageSfxType => DamageSfxType.Armor;

  private bool IsCrossbowReloaded
  {
    get => this._isCrossbowReloaded;
    set
    {
      this.AssertMutable();
      this._isCrossbowReloaded = value;
    }
  }

  protected override MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine.MonsterMoveStateMachine GenerateMoveStateMachine()
  {
    List<MonsterState> states = new List<MonsterState>();
    MoveState moveState = new MoveState("FIRE_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.FireMove), new AbstractIntent[1]
    {
      (AbstractIntent) new SingleAttackIntent(this.FireDamage)
    });
    MoveState initialState = new MoveState("RELOAD_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.ReloadMove), new AbstractIntent[1]
    {
      (AbstractIntent) new DefendIntent()
    });
    moveState.FollowUpState = (MonsterState) initialState;
    initialState.FollowUpState = (MonsterState) moveState;
    states.Add((MonsterState) initialState);
    states.Add((MonsterState) moveState);
    return new MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine.MonsterMoveStateMachine((IEnumerable<MonsterState>) states, (MonsterState) initialState);
  }

  private async Task FireMove(IReadOnlyList<Creature> targets)
  {
    AttackCommand attackCommand = await DamageCmd.Attack((Decimal) this.FireDamage).FromMonster((MonsterModel) this).WithAttackerAnim("Attack", 0.25f).WithAttackerFx(sfx: this.AttackSfx).WithHitFx("vfx/vfx_attack_slash").Execute((PlayerChoiceContext) null);
    this.IsCrossbowReloaded = false;
  }

  private async Task ReloadMove(IReadOnlyList<Creature> targets)
  {
    SfxCmd.Play("event:/sfx/enemy/enemy_attacks/crossbow_ruby_raider/crossbow_ruby_raider_reload");
    await CreatureCmd.TriggerAnim(this.Creature, "Reload", 0.25f);
    Decimal num = await CreatureCmd.GainBlock(this.Creature, 3M, ValueProp.Move, (CardPlay) null);
    this.IsCrossbowReloaded = true;
  }

  public override CreatureAnimator GenerateAnimator(MegaSprite controller)
  {
    AnimState animState1 = new AnimState("idle_loop", true);
    AnimState state1 = new AnimState("hurt");
    AnimState state2 = new AnimState("die");
    AnimState state3 = new AnimState("attack");
    AnimState animState2 = new AnimState("hurt_empty");
    AnimState initialState = new AnimState("idle_loop_empty", true);
    AnimState state4 = new AnimState("hurt_empty");
    AnimState state5 = new AnimState("die_empty");
    AnimState state6 = new AnimState("reload");
    state1.NextState = animState1;
    state3.NextState = initialState;
    animState2.NextState = initialState;
    state4.NextState = initialState;
    state6.NextState = animState1;
    CreatureAnimator animator = new CreatureAnimator(initialState, controller);
    animator.AddAnyState("Attack", state3);
    animator.AddAnyState("Reload", state6);
    animator.AddAnyState("Hit", state4, (Func<bool>) (() => !this.IsCrossbowReloaded));
    animator.AddAnyState("Hit", state1, (Func<bool>) (() => this.IsCrossbowReloaded));
    animator.AddAnyState("Dead", state5, (Func<bool>) (() => !this.IsCrossbowReloaded));
    animator.AddAnyState("Dead", state2, (Func<bool>) (() => this.IsCrossbowReloaded));
    return animator;
  }
}
