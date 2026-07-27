// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Monsters.CubexConstruct
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
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Random;
using MegaCrit.Sts2.Core.ValueProps;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Monsters;

public sealed class CubexConstruct : MonsterModel
{
  private static readonly string[] _eyeOptions = new string[3]
  {
    "diamondeye",
    "circleeye",
    "squareeye"
  };
  private static readonly string[] _mossOptions = new string[3]
  {
    "moss1",
    "moss2",
    "moss3"
  };
  private const string _chargeTrigger = "Charge";
  private const string _attackEndTrigger = "AttackEnd";
  private const string _chargeStartAnimId = "charge_start";
  private const int _expelRepeats = 2;
  private const string _burrowSfx = "event:/sfx/enemy/enemy_attacks/cubex_construct/cubex_construct_burrow";
  private const string _chargedLoopSfx = "event:/sfx/enemy/enemy_attacks/cubex_construct/cubex_construct_charge_attack";
  private bool _isBurrowed;
  private bool _isCharging;

  public override int MinInitialHp
  {
    get => AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 70, 65);
  }

  public override int MaxInitialHp => this.MinInitialHp;

  private int BlastDamage
  {
    get => AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 8, 7);
  }

  private int ExpelDamage
  {
    get => AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 6, 5);
  }

  public override DamageSfxType TakeDamageSfxType => DamageSfxType.Stone;

  private bool IsBurrowed
  {
    get => this._isBurrowed;
    set
    {
      this.AssertMutable();
      this._isBurrowed = value;
    }
  }

  private bool IsCharging
  {
    get => this._isCharging;
    set
    {
      this.AssertMutable();
      this._isCharging = value;
    }
  }

  public override void SetupSkins(MegaSprite spine, MegaSkeleton skeleton)
  {
    MegaSkin skin = spine.NewSkin("custom-skin");
    MegaSkeletonDataResource data = skeleton.GetData();
    skin.AddSkin(data.FindSkin(Rng.Chaotic.NextItem<string>((IEnumerable<string>) CubexConstruct._eyeOptions)));
    skin.AddSkin(data.FindSkin(Rng.Chaotic.NextItem<string>((IEnumerable<string>) CubexConstruct._mossOptions)));
    skeleton.SetSkin(skin);
    skeleton.SetSlotsToSetupPose();
  }

  public override async Task AfterAddedToRoom()
  {
    await base.AfterAddedToRoom();
    Decimal num = await CreatureCmd.GainBlock(this.Creature, 13M, ValueProp.Move, (CardPlay) null);
    ArtifactPower artifactPower = await PowerCmd.Apply<ArtifactPower>((PlayerChoiceContext) new ThrowingPlayerChoiceContext(), this.Creature, 1M, this.Creature, (CardModel) null);
    this.Creature.CurrentHpChanged += new Action<int, int>(this.OnHpChanged);
    this.IsBurrowed = true;
  }

  public override void BeforeRemovedFromRoom()
  {
    SfxCmd.StopLoop(this.Creature, "event:/sfx/enemy/enemy_attacks/cubex_construct/cubex_construct_charge_attack");
    this.Creature.CurrentHpChanged -= new Action<int, int>(this.OnHpChanged);
  }

  private void OnHpChanged(int oldHp, int newHp)
  {
    if (newHp >= oldHp)
      return;
    SfxCmd.SetParam("event:/sfx/enemy/enemy_attacks/cubex_construct/cubex_construct_charge_attack", "enemy_hurt", 1f);
  }

  protected override MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine.MonsterMoveStateMachine GenerateMoveStateMachine()
  {
    List<MonsterState> states = new List<MonsterState>();
    MoveState initialState = new MoveState("CHARGE_UP_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.ChargeUpMove), new AbstractIntent[1]
    {
      (AbstractIntent) new BuffIntent()
    });
    MoveState moveState1 = new MoveState("REPEATER_BLAST_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.RepeaterBlastMove), new AbstractIntent[2]
    {
      (AbstractIntent) new SingleAttackIntent(this.BlastDamage),
      (AbstractIntent) new BuffIntent()
    });
    MoveState moveState2 = new MoveState("REPEATER_BLAST_MOVE_2", new Func<IReadOnlyList<Creature>, Task>(this.RepeaterBlastMove), new AbstractIntent[2]
    {
      (AbstractIntent) new SingleAttackIntent(this.BlastDamage),
      (AbstractIntent) new BuffIntent()
    });
    MoveState moveState3 = new MoveState("EXPEL_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.ExpelBlastMove), new AbstractIntent[1]
    {
      (AbstractIntent) new MultiAttackIntent(this.ExpelDamage, 2)
    });
    initialState.FollowUpState = (MonsterState) moveState1;
    moveState1.FollowUpState = (MonsterState) moveState2;
    moveState2.FollowUpState = (MonsterState) moveState3;
    moveState3.FollowUpState = (MonsterState) moveState1;
    states.Add((MonsterState) initialState);
    states.Add((MonsterState) moveState1);
    states.Add((MonsterState) moveState2);
    states.Add((MonsterState) moveState3);
    return new MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine.MonsterMoveStateMachine((IEnumerable<MonsterState>) states, (MonsterState) initialState);
  }

  private async Task ChargeUpMove(IReadOnlyList<Creature> targets)
  {
    this.IsBurrowed = false;
    this.IsCharging = true;
    SfxCmd.PlayLoop(this.Creature, "event:/sfx/enemy/enemy_attacks/cubex_construct/cubex_construct_charge_attack", "loop", 2f);
    await CreatureCmd.TriggerAnim(this.Creature, "Charge", 0.0f);
    await Cmd.Wait(0.75f);
    StrengthPower strengthPower = await PowerCmd.Apply<StrengthPower>((PlayerChoiceContext) new ThrowingPlayerChoiceContext(), this.Creature, 2M, this.Creature, (CardModel) null);
  }

  private async Task RepeaterBlastMove(IReadOnlyList<Creature> targets)
  {
    SfxCmd.SetParam("event:/sfx/enemy/enemy_attacks/cubex_construct/cubex_construct_charge_attack", "loop", 1f);
    await Cmd.Wait(0.4f);
    AttackCommand attackCommand = await DamageCmd.Attack((Decimal) this.BlastDamage).FromMonster((MonsterModel) this).WithAttackerAnim("Attack", 0.0f).WithHitFx("vfx/vfx_attack_blunt", tmpSfx: "blunt_attack.mp3").Execute((PlayerChoiceContext) null);
    SfxCmd.SetParam("event:/sfx/enemy/enemy_attacks/cubex_construct/cubex_construct_charge_attack", "loop", 0.0f);
    await Cmd.Wait(0.2f);
    StrengthPower strengthPower = await PowerCmd.Apply<StrengthPower>((PlayerChoiceContext) new ThrowingPlayerChoiceContext(), this.Creature, 2M, this.Creature, (CardModel) null);
    await CreatureCmd.TriggerAnim(this.Creature, "AttackEnd", 0.0f);
  }

  private async Task ExpelBlastMove(IReadOnlyList<Creature> targets)
  {
    SfxCmd.SetParam("event:/sfx/enemy/enemy_attacks/cubex_construct/cubex_construct_charge_attack", "loop", 1f);
    await Cmd.Wait(0.4f);
    AttackCommand attackCommand = await DamageCmd.Attack((Decimal) this.ExpelDamage).WithHitCount(2).FromMonster((MonsterModel) this).WithAttackerAnim("Attack", 0.0f).WithHitFx("vfx/vfx_attack_blunt", tmpSfx: "blunt_attack.mp3").Execute((PlayerChoiceContext) null);
    await Cmd.Wait(0.2f);
    await CreatureCmd.TriggerAnim(this.Creature, "AttackEnd", 0.0f);
  }

  public override CreatureAnimator GenerateAnimator(MegaSprite controller)
  {
    AnimState animState1 = new AnimState("burrowed_loop", true)
    {
      BoundsContainer = "BurrowedBounds"
    };
    AnimState initialState = new AnimState("burrow");
    AnimState state1 = new AnimState("unburrow");
    AnimState animState2 = new AnimState("idle_loop", true)
    {
      BoundsContainer = "IdleBounds"
    };
    AnimState state2 = new AnimState("hurt");
    AnimState state3 = new AnimState("die");
    AnimState state4 = new AnimState("hurt");
    AnimState animState3 = new AnimState("charge_start")
    {
      BoundsContainer = "ChargingBounds"
    };
    AnimState animState4 = new AnimState("charge_loop", true);
    AnimState state5 = new AnimState("attack_loop", true);
    AnimState state6 = new AnimState("attack_finish");
    initialState.NextState = animState1;
    state1.NextState = animState3;
    state6.NextState = animState3;
    state2.NextState = animState2;
    animState3.NextState = animState4;
    state4.NextState = animState4;
    CreatureAnimator animator = new CreatureAnimator(initialState, controller);
    animator.AddAnyState("Charge", state1);
    animator.AddAnyState("Attack", state5);
    animator.AddAnyState("Dead", state3);
    animator.AddAnyState("AttackEnd", state6);
    animator.AddAnyState("Hit", state2, (Func<bool>) (() => !this.IsBurrowed && !this.IsCharging));
    animator.AddAnyState("Hit", state4, (Func<bool>) (() => !this.IsBurrowed && this.IsCharging));
    return animator;
  }

  public override List<BestiaryMonsterMove> GenerateBestiaryMoveList(
    NCreatureVisuals? creatureVisuals)
  {
    List<BestiaryMonsterMove> bestiaryMoveList = base.GenerateBestiaryMoveList(creatureVisuals);
    bestiaryMoveList.RemoveAll((Predicate<BestiaryMonsterMove>) (m => m.stateId == "REPEATER_BLAST_MOVE"));
    return bestiaryMoveList;
  }
}
