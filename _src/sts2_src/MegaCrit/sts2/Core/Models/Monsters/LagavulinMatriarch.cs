// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Monsters.LagavulinMatriarch
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
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.MonsterMoves.Intents;
using MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.ValueProps;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Monsters;

public sealed class LagavulinMatriarch : MonsterModel
{
  private const string _sleepMoveId = "SLEEP_MOVE";
  public const string slashMoveId = "SLASH_MOVE";
  private const string _sleepTrigger = "Sleep";
  public const string wakeTrigger = "Wake";
  private const string _attackHeavyTrigger = "AttackHeavy";
  private const string _attackDoubleTrigger = "AttackDouble";
  private const string _slamSfx = "event:/sfx/enemy/enemy_attacks/lagavulin_matriarch/lagavulin_matriarch_slam";
  private const string _castSfx = "event:/sfx/enemy/enemy_attacks/lagavulin_matriarch/lagavulin_matriarch_cast";
  public const string awakenSfx = "event:/sfx/enemy/enemy_attacks/lagavulin_matriarch/lagavulin_matriarch_awaken";
  private const string _attackStabSfx = "event:/sfx/enemy/enemy_attacks/lagavulin_matriarch/lagavulin_matriarch_attack_stab";
  private bool _isAwake;
  private bool _isShellAwake;
  private NSleepingVfx? _sleepingVfx;

  public override int MinInitialHp
  {
    get => AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 233, 222);
  }

  public override int MaxInitialHp => this.MinInitialHp;

  private int SlashDamage
  {
    get => AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 21, 19);
  }

  private int Slash2Damage
  {
    get => AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 14, 12);
  }

  private int Slash2Block
  {
    get => AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 14, 12);
  }

  private int DisembowelDamage
  {
    get => AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 10, 9);
  }

  private int DisembowelRepeat => 2;

  public override DamageSfxType TakeDamageSfxType => DamageSfxType.ArmorBig;

  public bool IsAwake
  {
    get => this._isAwake;
    set
    {
      this.AssertMutable();
      this._isAwake = value;
    }
  }

  public bool IsShellAwake
  {
    get => this._isShellAwake;
    set
    {
      this.AssertMutable();
      this._isShellAwake = value;
    }
  }

  private NSleepingVfx? SleepingVfx
  {
    get => this._sleepingVfx;
    set
    {
      this.AssertMutable();
      this._sleepingVfx = value;
    }
  }

  public override void SetupSkins(MegaSprite spine, MegaSkeleton skeleton)
  {
    spine.GetAnimationState().SetAnimation("_tracks/eyes_closed_loop", trackId: 1);
  }

  public override async Task AfterAddedToRoom()
  {
    await base.AfterAddedToRoom();
    await this.Sleep();
  }

  private async Task Sleep()
  {
    this.IsAwake = false;
    await CreatureCmd.TriggerAnim(this.Creature, nameof (Sleep), 0.0f);
    PlatingPower platingPower = await PowerCmd.Apply<PlatingPower>((PlayerChoiceContext) new ThrowingPlayerChoiceContext(), this.Creature, 12M, this.Creature, (CardModel) null);
    AsleepPower asleepPower = await PowerCmd.Apply<AsleepPower>((PlayerChoiceContext) new ThrowingPlayerChoiceContext(), this.Creature, 3M, this.Creature, (CardModel) null);
    Marker2D specialNode = this.Creature.GetCreatureNode()?.GetSpecialNode<Marker2D>("%SleepVfxPos");
    if (specialNode == null)
      return;
    this.SleepingVfx = NSleepingVfx.Create(((Node2D) specialNode).GlobalPosition);
    ((Node) specialNode).AddChildSafely((Node) this.SleepingVfx);
    this.SleepingVfx.Position = Vector2.Zero;
  }

  public override Task AfterDamageReceived(
    PlayerChoiceContext choiceContext,
    Creature target,
    DamageResult result,
    ValueProp props,
    Creature? dealer,
    CardModel? cardSource)
  {
    if (target != this.Creature)
      return Task.CompletedTask;
    this.SleepingVfx?.Stop();
    this.SleepingVfx = (NSleepingVfx) null;
    if (this.Creature.CurrentHp <= this.Creature.MaxHp / 2 && !this.IsShellAwake)
    {
      NCreature creatureNode = this.Creature.GetCreatureNode();
      SpineAnimationAccess spineAnimation;
      if (creatureNode != null)
      {
        spineAnimation = creatureNode.SpineAnimation;
        spineAnimation.SetAnimation("_tracks/eyes_open", false, 1);
      }
      if (creatureNode != null)
      {
        spineAnimation = creatureNode.SpineAnimation;
        spineAnimation.AddAnimation("_tracks/eyes_open_loop", track: 1);
      }
      this.IsShellAwake = true;
    }
    return Task.CompletedTask;
  }

  public override Task AfterDeath(
    PlayerChoiceContext choiceContext,
    Creature creature,
    bool wasRemovalPrevented,
    float deathAnimLength)
  {
    if (creature != this.Creature)
      return Task.CompletedTask;
    this.Creature.GetCreatureNode()?.SpineAnimation.SetAnimation("_tracks/eyes_dead", false, 1);
    this.SleepingVfx?.Stop();
    this.SleepingVfx = (NSleepingVfx) null;
    return Task.CompletedTask;
  }

  protected override MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine.MonsterMoveStateMachine GenerateMoveStateMachine()
  {
    List<MonsterState> states = new List<MonsterState>();
    MoveState moveState1 = new MoveState("SLEEP_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.SleepMove), new AbstractIntent[1]
    {
      (AbstractIntent) new SleepIntent()
    });
    MoveState move = new MoveState("SLASH_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.SlashMove), new AbstractIntent[1]
    {
      (AbstractIntent) new SingleAttackIntent(this.SlashDamage)
    });
    MoveState moveState2 = new MoveState("SLASH2_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.Slash2Move), new AbstractIntent[2]
    {
      (AbstractIntent) new SingleAttackIntent(this.Slash2Damage),
      (AbstractIntent) new DefendIntent()
    });
    MoveState moveState3 = new MoveState("DISEMBOWEL_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.DisembowelMove), new AbstractIntent[1]
    {
      (AbstractIntent) new MultiAttackIntent(this.DisembowelDamage, this.DisembowelRepeat)
    });
    MoveState moveState4 = new MoveState("SOUL_SIPHON_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.SoulSiphonMove), new AbstractIntent[2]
    {
      (AbstractIntent) new DebuffIntent(),
      (AbstractIntent) new BuffIntent()
    });
    ConditionalBranchState conditionalBranchState = new ConditionalBranchState("SLEEP_BRANCH");
    moveState1.FollowUpState = (MonsterState) conditionalBranchState;
    move.FollowUpState = (MonsterState) moveState3;
    moveState3.FollowUpState = (MonsterState) moveState2;
    moveState2.FollowUpState = (MonsterState) moveState4;
    moveState4.FollowUpState = (MonsterState) move;
    conditionalBranchState.AddState((MonsterState) moveState1, (Func<bool>) (() => this.Creature.HasPower<AsleepPower>()));
    conditionalBranchState.AddState((MonsterState) move, (Func<bool>) (() => !this.Creature.HasPower<AsleepPower>()));
    states.Add((MonsterState) conditionalBranchState);
    states.Add((MonsterState) moveState1);
    states.Add((MonsterState) move);
    states.Add((MonsterState) moveState2);
    states.Add((MonsterState) moveState4);
    states.Add((MonsterState) moveState3);
    return new MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine.MonsterMoveStateMachine((IEnumerable<MonsterState>) states, (MonsterState) moveState1);
  }

  private Task SleepMove(IReadOnlyList<Creature> targets) => Task.CompletedTask;

  public async Task WakeUpMove(IReadOnlyList<Creature> _)
  {
    if (this._isAwake)
      return;
    SfxCmd.Play("event:/sfx/enemy/enemy_attacks/lagavulin_matriarch/lagavulin_matriarch_awaken");
    await CreatureCmd.TriggerAnim(this.Creature, "Wake", 0.6f);
    this.SleepingVfx?.Stop();
    this.SleepingVfx = (NSleepingVfx) null;
    this.IsAwake = true;
  }

  private async Task SlashMove(IReadOnlyList<Creature> targets)
  {
    this.SleepingVfx?.Stop();
    this.SleepingVfx = (NSleepingVfx) null;
    AttackCommand attackCommand = await DamageCmd.Attack((Decimal) this.SlashDamage).FromMonster((MonsterModel) this).WithAttackerAnim("AttackHeavy", 0.3f).WithAttackerFx(sfx: "event:/sfx/enemy/enemy_attacks/lagavulin_matriarch/lagavulin_matriarch_slam").WithHitFx("vfx/vfx_attack_slash").Execute((PlayerChoiceContext) null);
  }

  private async Task Slash2Move(IReadOnlyList<Creature> targets)
  {
    this.SleepingVfx?.Stop();
    this.SleepingVfx = (NSleepingVfx) null;
    AttackCommand attackCommand = await DamageCmd.Attack((Decimal) this.Slash2Damage).FromMonster((MonsterModel) this).WithAttackerAnim("AttackHeavy", 0.2f).WithAttackerFx(sfx: "event:/sfx/enemy/enemy_attacks/lagavulin_matriarch/lagavulin_matriarch_slam").WithHitFx("vfx/vfx_attack_slash").Execute((PlayerChoiceContext) null);
    Decimal num = await CreatureCmd.GainBlock(this.Creature, (Decimal) this.Slash2Block, ValueProp.Move, (CardPlay) null);
  }

  private async Task DisembowelMove(IReadOnlyList<Creature> targets)
  {
    this.SleepingVfx?.Stop();
    this.SleepingVfx = (NSleepingVfx) null;
    AttackCommand attackCommand = await DamageCmd.Attack((Decimal) this.DisembowelDamage).WithHitCount(this.DisembowelRepeat).FromMonster((MonsterModel) this).WithAttackerAnim("AttackDouble", 0.15f).OnlyPlayAnimOnce().WithAttackerFx(sfx: "event:/sfx/enemy/enemy_attacks/lagavulin_matriarch/lagavulin_matriarch_attack_stab").WithHitFx("vfx/vfx_attack_slash").Execute((PlayerChoiceContext) null);
  }

  private async Task SoulSiphonMove(IReadOnlyList<Creature> targets)
  {
    this.SleepingVfx?.Stop();
    this.SleepingVfx = (NSleepingVfx) null;
    SfxCmd.Play("event:/sfx/enemy/enemy_attacks/lagavulin_matriarch/lagavulin_matriarch_cast");
    await CreatureCmd.TriggerAnim(this.Creature, "Cast", 0.6f);
    IReadOnlyList<StrengthPower> strengthPowerList = await PowerCmd.Apply<StrengthPower>((PlayerChoiceContext) new ThrowingPlayerChoiceContext(), (IEnumerable<Creature>) targets, -2M, this.Creature, (CardModel) null);
    IReadOnlyList<DexterityPower> dexterityPowerList = await PowerCmd.Apply<DexterityPower>((PlayerChoiceContext) new ThrowingPlayerChoiceContext(), (IEnumerable<Creature>) targets, -2M, this.Creature, (CardModel) null);
    StrengthPower strengthPower = await PowerCmd.Apply<StrengthPower>((PlayerChoiceContext) new ThrowingPlayerChoiceContext(), this.Creature, 2M, this.Creature, (CardModel) null);
  }

  public override CreatureAnimator GenerateAnimator(MegaSprite controller)
  {
    AnimState state1 = new AnimState("sleep_loop", true);
    AnimState state2 = new AnimState("hurt_sleeping");
    AnimState state3 = new AnimState("wake_up");
    AnimState initialState = new AnimState("idle_loop", true);
    AnimState state4 = new AnimState("cast");
    AnimState state5 = new AnimState("attack_heavy");
    AnimState state6 = new AnimState("attack_double");
    AnimState state7 = new AnimState("hurt");
    AnimState state8 = new AnimState("die");
    state2.NextState = state3;
    state3.NextState = initialState;
    state4.NextState = initialState;
    state5.NextState = initialState;
    state6.NextState = initialState;
    state7.NextState = initialState;
    CreatureAnimator animator = new CreatureAnimator(initialState, controller);
    animator.AddAnyState("Sleep", state1);
    animator.AddAnyState("Wake", state3, (Func<bool>) (() => !this.IsAwake));
    animator.AddAnyState("Cast", state4);
    animator.AddAnyState("AttackHeavy", state5);
    animator.AddAnyState("AttackDouble", state6);
    animator.AddAnyState("Dead", state8);
    animator.AddAnyState("Hit", state7, (Func<bool>) (() => this.IsAwake));
    animator.AddAnyState("Hit", state2, (Func<bool>) (() => !this.IsAwake));
    return animator;
  }

  protected override bool ShouldShowMoveInBestiary(string moveStateId)
  {
    return moveStateId != "SLEEP_MOVE";
  }

  public override List<BestiaryMonsterMove> GenerateBestiaryMoveList(
    NCreatureVisuals? creatureVisuals)
  {
    List<BestiaryMonsterMove> bestiaryMoveList = base.GenerateBestiaryMoveList(creatureVisuals);
    bestiaryMoveList.Insert(0, BestiaryMonsterMove.FromAction(this.GetBestiaryMoveName("SLEEP"), new Func<Task>(this.Sleep)));
    bestiaryMoveList.Insert(1, BestiaryMonsterMove.FromNonStateMove(this.GetBestiaryMoveName("WAKE_UP"), new Func<IReadOnlyList<Creature>, Task>(this.WakeUpMove)));
    return bestiaryMoveList;
  }
}
