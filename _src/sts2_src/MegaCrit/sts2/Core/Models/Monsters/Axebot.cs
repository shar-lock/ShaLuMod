// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Monsters.Axebot
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

public sealed class Axebot : MonsterModel
{
  private const int _oneTwoRepeat = 2;
  private const string _hammerUppercutTrigger = "uppercut";
  private const string _sharpenTrigger = "sharpen";
  public const string respawnTrigger = "respawn";
  private const string _buffSfx = "event:/sfx/enemy/enemy_attacks/axebot/axebot_buff";
  private const string _spinSfx = "event:/sfx/enemy/enemy_attacks/axebot/axebot_attack_spin";
  private int? _stockOverrideAmount;
  private bool _shouldPlaySpawnAnimation;

  private int BootUpBlock
  {
    get => AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 15, 10);
  }

  private int OneTwoDamage
  {
    get => AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 10, 9);
  }

  private int BootUpStrGain
  {
    get => AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 4, 3);
  }

  private int HammerUppercutDamage
  {
    get => AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 14, 12);
  }

  public override int MinInitialHp
  {
    get => AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 76, 70);
  }

  public override int MaxInitialHp
  {
    get => AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 86, 78);
  }

  public override DamageSfxType TakeDamageSfxType => DamageSfxType.Armor;

  public int StockAmount
  {
    get => this._stockOverrideAmount ?? 2;
    set
    {
      this.AssertMutable();
      this._stockOverrideAmount = new int?(value);
    }
  }

  public bool ShouldPlaySpawnAnimation
  {
    get => this._shouldPlaySpawnAnimation;
    set
    {
      this.AssertMutable();
      this._shouldPlaySpawnAnimation = value;
    }
  }

  public override async Task AfterAddedToRoom()
  {
    if (this.StockAmount <= 0)
      return;
    StockPower stockPower = await PowerCmd.Apply<StockPower>((PlayerChoiceContext) new ThrowingPlayerChoiceContext(), this.Creature, (Decimal) this.StockAmount, (Creature) null, (CardModel) null);
  }

  protected override MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine.MonsterMoveStateMachine GenerateMoveStateMachine()
  {
    List<MonsterState> states = new List<MonsterState>();
    MoveState initialState1 = new MoveState("BOOT_UP_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.BootUpMove), new AbstractIntent[2]
    {
      (AbstractIntent) new DefendIntent(),
      (AbstractIntent) new BuffIntent()
    });
    MoveState moveState = new MoveState("ONE_TWO_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.OneTwoMove), new AbstractIntent[1]
    {
      (AbstractIntent) new MultiAttackIntent(this.OneTwoDamage, 2)
    });
    MoveState initialState2 = new MoveState("HAMMER_UPPERCUT_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.HammerUppercutMove), new AbstractIntent[2]
    {
      (AbstractIntent) new SingleAttackIntent(this.HammerUppercutDamage),
      (AbstractIntent) new DebuffIntent()
    });
    initialState1.FollowUpState = (MonsterState) initialState2;
    initialState2.FollowUpState = (MonsterState) moveState;
    moveState.FollowUpState = (MonsterState) initialState2;
    states.Add((MonsterState) initialState1);
    states.Add((MonsterState) moveState);
    states.Add((MonsterState) initialState2);
    return this._stockOverrideAmount.HasValue ? new MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine.MonsterMoveStateMachine((IEnumerable<MonsterState>) states, (MonsterState) initialState1) : new MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine.MonsterMoveStateMachine((IEnumerable<MonsterState>) states, (MonsterState) initialState2);
  }

  private async Task BootUpMove(IReadOnlyList<Creature> targets)
  {
    SfxCmd.Play("event:/sfx/enemy/enemy_attacks/axebot/axebot_buff");
    await CreatureCmd.TriggerAnim(this.Creature, "sharpen", 0.3f);
    await Cmd.Wait(0.25f);
    Decimal num = await CreatureCmd.GainBlock(this.Creature, (Decimal) this.BootUpBlock, ValueProp.Move, (CardPlay) null);
    StrengthPower strengthPower = await PowerCmd.Apply<StrengthPower>((PlayerChoiceContext) new ThrowingPlayerChoiceContext(), this.Creature, (Decimal) (this.BootUpStrGain * (2 - this.StockAmount)), this.Creature, (CardModel) null);
  }

  private async Task OneTwoMove(IReadOnlyList<Creature> targets)
  {
    AttackCommand attackCommand = await DamageCmd.Attack((Decimal) this.OneTwoDamage).WithHitCount(2).FromMonster((MonsterModel) this).WithAttackerAnim("Attack", 0.35f).WithAttackerFx(sfx: this.AttackSfx).WithHitFx("vfx/vfx_attack_slash").OnlyPlayAnimOnce().Execute((PlayerChoiceContext) null);
  }

  private async Task HammerUppercutMove(IReadOnlyList<Creature> targets)
  {
    AttackCommand attackCommand = await DamageCmd.Attack((Decimal) this.HammerUppercutDamage).FromMonster((MonsterModel) this).WithAttackerAnim("uppercut", 0.8f).WithAttackerFx(sfx: "event:/sfx/enemy/enemy_attacks/axebot/axebot_attack_spin").WithHitFx("vfx/vfx_attack_blunt").Execute((PlayerChoiceContext) null);
    IReadOnlyList<WeakPower> weakPowerList = await PowerCmd.Apply<WeakPower>((PlayerChoiceContext) new ThrowingPlayerChoiceContext(), (IEnumerable<Creature>) targets, 2M, this.Creature, (CardModel) null);
    IReadOnlyList<FrailPower> frailPowerList = await PowerCmd.Apply<FrailPower>((PlayerChoiceContext) new ThrowingPlayerChoiceContext(), (IEnumerable<Creature>) targets, 2M, this.Creature, (CardModel) null);
  }

  public override CreatureAnimator GenerateAnimator(MegaSprite controller)
  {
    AnimState animState = new AnimState("idle_loop", true);
    AnimState state1 = new AnimState("attack");
    AnimState state2 = new AnimState("special");
    AnimState state3 = new AnimState("sharpen");
    AnimState state4 = new AnimState("respawn");
    AnimState state5 = new AnimState("hurt");
    AnimState state6 = new AnimState("die");
    state1.NextState = animState;
    state2.NextState = animState;
    state3.NextState = animState;
    state5.NextState = animState;
    state4.NextState = animState;
    CreatureAnimator animator = new CreatureAnimator(this._shouldPlaySpawnAnimation ? state4 : animState, controller);
    animator.AddAnyState("Dead", state6);
    animator.AddAnyState("Hit", state5);
    animator.AddAnyState("Attack", state1);
    animator.AddAnyState("uppercut", state2);
    animator.AddAnyState("sharpen", state3);
    animator.AddAnyState("respawn", state4);
    return animator;
  }
}
