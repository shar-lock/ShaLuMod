// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Monsters.Vantom
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using MegaCrit.Sts2.Core.Animation;
using MegaCrit.Sts2.Core.Audio;
using MegaCrit.Sts2.Core.Bindings.MegaSpine;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.Ascension;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.MonsterMoves.Intents;
using MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine;
using MegaCrit.Sts2.Core.Nodes;
using MegaCrit.Sts2.Core.Nodes.Audio;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.Nodes.Vfx.Utilities;
using MegaCrit.Sts2.Core.Random;
using MegaCrit.Sts2.Core.TestSupport;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Monsters;

public sealed class Vantom : MonsterModel
{
  private const string _vantomCustomTrackName = "vantom_progress";
  private const int _inkyLanceRepeat = 2;
  private const int _dismemberWounds = 3;
  private const int _prepareStrength = 2;
  private const string _chargeUpTrigger = "CHARGE_UP";
  private const string _buffTrigger = "BUFF";
  private const string _debuffTrigger = "DEBUFF";
  private const string _attackDoubleTrigger = "ATTACK_DOUBLE";
  private const string _heavyAttackTrigger = "ATTACK_HEAVY";
  private const string _buffSfx = "event:/sfx/enemy/enemy_attacks/vantom/vantom_buff";
  private const string _dismemberSfx = "event:/sfx/enemy/enemy_attacks/vantom/vantom_dismember";
  private const string _extend1Sfx = "event:/sfx/enemy/enemy_attacks/vantom/vantom_extend_1";
  private const string _extend2Sfx = "event:/sfx/enemy/enemy_attacks/vantom/vantom_extend_2";
  private const string _extend3Sfx = "event:/sfx/enemy/enemy_attacks/vantom/vantom_extend_2";
  private const string _inkyLanceSfx = "event:/sfx/enemy/enemy_attacks/vantom/vantom_inky_lance";
  private Tween? _scaleTween;

  public override int MinInitialHp
  {
    get => AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 183, 173);
  }

  public int SlipperyAmt => AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 9, 8);

  public override int MaxInitialHp => this.MinInitialHp;

  private int InkBlotDamage
  {
    get => AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 8, 7);
  }

  private int InkyLanceDamage
  {
    get => AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 7, 6);
  }

  private int DismemberDamage
  {
    get => AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 30, 26);
  }

  public override DamageSfxType TakeDamageSfxType => DamageSfxType.Magic;

  public override bool ShouldDisappearFromDoom => false;

  private Tween? ScaleTween
  {
    get => this._scaleTween;
    set
    {
      this.AssertMutable();
      this._scaleTween = value;
    }
  }

  public override async Task AfterAddedToRoom()
  {
    await base.AfterAddedToRoom();
    SlipperyPower slipperyPower = await PowerCmd.Apply<SlipperyPower>((PlayerChoiceContext) new ThrowingPlayerChoiceContext(), this.Creature, (Decimal) this.SlipperyAmt, this.Creature, (CardModel) null);
  }

  public override Task AfterDeath(
    PlayerChoiceContext choiceContext,
    Creature creature,
    bool wasRemovalPrevented,
    float deathAnimLength)
  {
    if (creature != this.Creature)
      return Task.CompletedTask;
    NRunMusicController.Instance?.UpdateMusicParameter("vantom_progress", 5f);
    return Task.CompletedTask;
  }

  public override void SetupSkins(MegaSprite spine, MegaSkeleton skeleton)
  {
    MegaAnimationState animationState = spine.GetAnimationState();
    animationState.SetAnimation("_tracks/charge_up_1", false, 1);
    animationState.AddAnimation("_tracks/charged_1", trackId: 1);
  }

  protected override MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine.MonsterMoveStateMachine GenerateMoveStateMachine()
  {
    List<MonsterState> states = new List<MonsterState>();
    MoveState initialState = new MoveState("INK_BLOT_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.InkBlotMove), new AbstractIntent[1]
    {
      (AbstractIntent) new SingleAttackIntent(this.InkBlotDamage)
    });
    MoveState moveState1 = new MoveState("INKY_LANCE_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.InkyLanceMove), new AbstractIntent[1]
    {
      (AbstractIntent) new MultiAttackIntent(this.InkyLanceDamage, 2)
    });
    MoveState moveState2 = new MoveState("DISMEMBER_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.DismemberMove), new AbstractIntent[2]
    {
      (AbstractIntent) new SingleAttackIntent(this.DismemberDamage),
      (AbstractIntent) new StatusIntent(3)
    });
    MoveState moveState3 = new MoveState("PREPARE_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.PrepareMove), new AbstractIntent[1]
    {
      (AbstractIntent) new BuffIntent()
    });
    initialState.FollowUpState = (MonsterState) moveState1;
    moveState1.FollowUpState = (MonsterState) moveState2;
    moveState2.FollowUpState = (MonsterState) moveState3;
    moveState3.FollowUpState = (MonsterState) initialState;
    states.Add((MonsterState) initialState);
    states.Add((MonsterState) moveState1);
    states.Add((MonsterState) moveState2);
    states.Add((MonsterState) moveState3);
    return new MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine.MonsterMoveStateMachine((IEnumerable<MonsterState>) states, (MonsterState) initialState);
  }

  private async Task InkBlotMove(IReadOnlyList<Creature> targets)
  {
    AttackCommand attackCommand = await DamageCmd.Attack((Decimal) this.InkBlotDamage).FromMonster((MonsterModel) this).WithAttackerAnim("Attack", 0.35f).WithAttackerFx(sfx: "event:/sfx/enemy/enemy_attacks/vantom/vantom_inky_lance").WithHitFx("vfx/vfx_attack_blunt").Execute((PlayerChoiceContext) null);
    if (!TestMode.IsOff || !this.Creature.IsAlive)
      return;
    await Cmd.CustomScaledWait(1f, 1f);
    NRunMusicController.Instance?.UpdateMusicParameter("vantom_progress", 1f);
    SfxCmd.Play("event:/sfx/enemy/enemy_attacks/vantom/vantom_extend_2");
    await CreatureCmd.TriggerAnim(this.Creature, "CHARGE_UP", 0.15f);
    MegaAnimationState animationState = NCombatRoom.Instance?.GetCreatureNode(this.Creature)?.SpineAnimation.GetAnimationState();
    animationState?.SetAnimation("_tracks/charge_up_2", false, 1);
    animationState?.AddAnimation("_tracks/charged_2", trackId: 1);
  }

  private async Task InkyLanceMove(IReadOnlyList<Creature> targets)
  {
    AttackCommand attackCommand = await DamageCmd.Attack((Decimal) this.InkyLanceDamage).WithHitCount(2).FromMonster((MonsterModel) this).WithAttackerAnim("ATTACK_DOUBLE", 0.4f).OnlyPlayAnimOnce().WithAttackerFx(sfx: "event:/sfx/enemy/enemy_attacks/vantom/vantom_inky_lance").WithHitFx("vfx/vfx_attack_blunt").Execute((PlayerChoiceContext) null);
    if (!TestMode.IsOff || !this.Creature.IsAlive)
      return;
    NRunMusicController.Instance?.UpdateMusicParameter("vantom_progress", 2f);
    await Cmd.CustomScaledWait(1f, 1f);
    SfxCmd.Play("event:/sfx/enemy/enemy_attacks/vantom/vantom_extend_2");
    MegaAnimationState animationState = NCombatRoom.Instance?.GetCreatureNode(this.Creature)?.SpineAnimation.GetAnimationState();
    animationState?.SetAnimation("_tracks/charge_up_3", false, 1);
    animationState?.AddAnimation("_tracks/charged_3", trackId: 1);
    await CreatureCmd.TriggerAnim(this.Creature, "CHARGE_UP", 0.15f);
  }

  private async Task DismemberMove(IReadOnlyList<Creature> targets)
  {
    if (TestMode.IsOff && this.Creature.IsAlive)
    {
      MegaAnimationState animationState = NCombatRoom.Instance?.GetCreatureNode(this.Creature)?.SpineAnimation.GetAnimationState();
      animationState?.SetAnimation("_tracks/attack_heavy", false, 1);
      animationState?.AddAnimation("_tracks/charged_0", trackId: 1);
    }
    NRunMusicController.Instance?.UpdateMusicParameter("vantom_progress", 3f);
    await CreatureCmd.TriggerAnim(this.Creature, "ATTACK_HEAVY", 0.0f);
    await Cmd.Wait(0.25f);
    NCombatRoom.Instance?.RadialBlur(VfxPosition.Left);
    NGame.Instance?.ScreenShake(ShakeStrength.Strong, ShakeDuration.Normal, 180f + Rng.Chaotic.NextFloat(-10f, 10f));
    AttackCommand attackCommand = await DamageCmd.Attack((Decimal) this.DismemberDamage).FromMonster((MonsterModel) this).WithNoAttackerAnim().WithHitFx("vfx/vfx_giant_horizontal_slash", "event:/sfx/enemy/enemy_attacks/vantom/vantom_dismember").Execute((PlayerChoiceContext) null);
    NGame.Instance?.DoHitStop(ShakeStrength.Weak, ShakeDuration.Short);
    await Cmd.Wait(0.5f);
    await CardPileCmd.AddToCombatAndPreview<Wound>((IEnumerable<Creature>) targets, PileType.Discard, 3, (Player) null);
  }

  private async Task PrepareMove(IReadOnlyList<Creature> targets)
  {
    SfxCmd.Play("event:/sfx/enemy/enemy_attacks/vantom/vantom_buff");
    await CreatureCmd.TriggerAnim(this.Creature, "BUFF", 0.6f);
    StrengthPower strengthPower = await PowerCmd.Apply<StrengthPower>((PlayerChoiceContext) new ThrowingPlayerChoiceContext(), this.Creature, 2M, this.Creature, (CardModel) null);
    if (!TestMode.IsOff || !this.Creature.IsAlive)
      return;
    await Cmd.CustomScaledWait(1f, 1f);
    SfxCmd.Play("event:/sfx/enemy/enemy_attacks/vantom/vantom_extend_1");
    MegaAnimationState animationState = NCombatRoom.Instance?.GetCreatureNode(this.Creature)?.SpineAnimation.GetAnimationState();
    animationState?.SetAnimation("_tracks/charge_up_1", false, 1);
    animationState?.AddAnimation("_tracks/charged_1", trackId: 1);
    await CreatureCmd.TriggerAnim(this.Creature, "CHARGE_UP", 0.25f);
    NRunMusicController.Instance?.UpdateMusicParameter("vantom_progress", 1f);
  }

  public void ScaleTo(float scale, float duration)
  {
    Node2D specialNode = this.Creature.GetCreatureNode()?.GetSpecialNode<Node2D>("Visuals/ScalingBone");
    if (specialNode == null)
      return;
    Tween scaleTween = this.ScaleTween;
    if (scaleTween != null)
      scaleTween.FastForwardToCompletion();
    this.ScaleTween = ((Node) specialNode).CreateTween();
    this.ScaleTween.TweenProperty((GodotObject) specialNode, NodePath.op_Implicit(nameof (scale)), Variant.op_Implicit(Vector2.op_Multiply(Vector2.One, scale)), (double) duration).SetEase((Tween.EaseType) 2L).SetTrans((Tween.TransitionType) 1L);
  }

  public override CreatureAnimator GenerateAnimator(MegaSprite controller)
  {
    AnimState initialState = new AnimState("idle_loop", true);
    AnimState state1 = new AnimState("buff");
    AnimState state2 = new AnimState("debuff");
    AnimState state3 = new AnimState("attack_double");
    AnimState state4 = new AnimState("attack");
    AnimState state5 = new AnimState("hurt");
    AnimState state6 = new AnimState("die");
    AnimState state7 = new AnimState("charge_up");
    AnimState state8 = new AnimState("attack_heavy");
    state1.NextState = initialState;
    state2.NextState = initialState;
    state4.NextState = initialState;
    state5.NextState = initialState;
    state7.NextState = initialState;
    state8.NextState = initialState;
    state3.NextState = initialState;
    CreatureAnimator animator = new CreatureAnimator(initialState, controller);
    animator.AddAnyState("CHARGE_UP", state7);
    animator.AddAnyState("ATTACK_HEAVY", state8);
    animator.AddAnyState("BUFF", state1);
    animator.AddAnyState("Attack", state4);
    animator.AddAnyState("Dead", state6);
    animator.AddAnyState("Hit", state5);
    animator.AddAnyState("DEBUFF", state2);
    animator.AddAnyState("ATTACK_DOUBLE", state3);
    return animator;
  }
}
