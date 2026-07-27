// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Monsters.WaterfallGiant
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
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.MonsterMoves.Intents;
using MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine;
using MegaCrit.Sts2.Core.Nodes.Audio;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.TestSupport;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Monsters;

public sealed class WaterfallGiant : MonsterModel
{
  private const string _siphonMove = "SIPHON_MOVE";
  private const string _waterfallGiantTrackName = "waterfall_giant_progress";
  private const int _endCombatBgmFlag = 5;
  private const int _maxIntensityBgmFlag = 2;
  private const int _increaseIntensityBgmFlag = 1;
  private const int _increaseIntensityAmbienceFlag = 1;
  private const int _maxIntensityAmbienceFlag = 3;
  private const int _endAmbienceFlag = 2;
  private int _currentPressureGunDamage;
  private int _steamEruptionDamage;
  private MoveState _aboutToBlowState;
  private bool _isAboutToBlow;
  private int _pressureBuildupIdx;
  private const int _maxPressureBuildup = 6;
  private const string _attackBuffTrigger = "AttackBuff";
  private const string _attackDebuffTrigger = "AttackDebuff";
  private const string _healTrigger = "Heal";
  private const string _eruptTrigger = "Erupt";
  private const string _attackKickSfx = "event:/sfx/enemy/enemy_attacks/waterfall_giant/waterfall_giant_attack_kick";
  private const string _attackStompSfx = "event:/sfx/enemy/enemy_attacks/waterfall_giant/waterfall_giant_attack_stomp";
  private const string _eruptionSfx = "event:/sfx/enemy/enemy_attacks/waterfall_giant/waterfall_giant_eruption";
  private const string _knockoutSfx = "event:/sfx/enemy/enemy_attacks/waterfall_giant/waterfall_giant_knockout";
  private const string _ambientSfx = "event:/sfx/enemy/enemy_attacks/waterfall_giant/waterfall_giant_ambient";

  public override int MinInitialHp
  {
    get => AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 250, 240 /*0xF0*/);
  }

  public override int MaxInitialHp => this.MinInitialHp;

  public int SiphonHeal => AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 15, 10);

  private int PressurizeAmount
  {
    get => AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 20, 15);
  }

  private int StompDamage
  {
    get => AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 16 /*0x10*/, 15);
  }

  private int RamDamage
  {
    get => AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 11, 10);
  }

  private int PressureUpDamage
  {
    get => AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 14, 13);
  }

  private int BasePressureGunDamage
  {
    get => AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 23, 20);
  }

  public override bool ShouldDisappearFromDoom => !this.Creature.HasPower<SteamEruptionPower>();

  private int PressureGunIncrease => 5;

  private int CurrentPressureGunDamage
  {
    get => this._currentPressureGunDamage;
    set
    {
      this.AssertMutable();
      this._currentPressureGunDamage = value;
    }
  }

  private int SteamEruptionDamage
  {
    get => this._steamEruptionDamage;
    set
    {
      this.AssertMutable();
      this._steamEruptionDamage = value;
    }
  }

  private MoveState AboutToBlowState
  {
    get => this._aboutToBlowState;
    set
    {
      this.AssertMutable();
      this._aboutToBlowState = value;
    }
  }

  private bool IsAboutToBlow
  {
    get => this._isAboutToBlow;
    set
    {
      this.AssertMutable();
      this._isAboutToBlow = value;
    }
  }

  private int PressureBuildupIdx
  {
    get => this._pressureBuildupIdx;
    set
    {
      this.AssertMutable();
      this._pressureBuildupIdx = value;
    }
  }

  public override DamageSfxType TakeDamageSfxType => DamageSfxType.Magic;

  public override bool HasDeathSfx => false;

  public override bool ShouldFadeAfterDeath => this.PressureBuildupIdx == 0;

  public override async Task AfterAddedToRoom()
  {
    await base.AfterAddedToRoom();
    this.CurrentPressureGunDamage = this.BasePressureGunDamage;
    SfxCmd.PlayLoop(this.Creature, "event:/sfx/enemy/enemy_attacks/waterfall_giant/waterfall_giant_ambient", "waterfall_giant_sfx", 2f);
  }

  public override Task AfterDeath(
    PlayerChoiceContext choiceContext,
    Creature creature,
    bool wasRemovalPrevented,
    float deathAnimLength)
  {
    if (creature != this.Creature || this.Creature.HasPower<SteamEruptionPower>())
      return Task.CompletedTask;
    NRunMusicController.Instance?.UpdateMusicParameter("waterfall_giant_progress", 5f);
    this.StopAmbientSfx();
    return Task.CompletedTask;
  }

  public override void BeforeRemovedFromRoom() => this.StopAmbientSfx();

  private void StopAmbientSfx()
  {
    SfxCmd.StopLoop(this.Creature, "event:/sfx/enemy/enemy_attacks/waterfall_giant/waterfall_giant_ambient");
  }

  protected override MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine.MonsterMoveStateMachine GenerateMoveStateMachine()
  {
    List<MonsterState> states = new List<MonsterState>();
    MoveState initialState = new MoveState("PRESSURIZE_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.PressurizeMove), new AbstractIntent[1]
    {
      (AbstractIntent) new BuffIntent()
    });
    MoveState moveState1 = new MoveState("STOMP_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.StompMove), new AbstractIntent[3]
    {
      (AbstractIntent) new SingleAttackIntent(this.StompDamage),
      (AbstractIntent) new DebuffIntent(),
      (AbstractIntent) new BuffIntent()
    });
    MoveState moveState2 = new MoveState("RAM_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.RamMove), new AbstractIntent[2]
    {
      (AbstractIntent) new SingleAttackIntent(this.RamDamage),
      (AbstractIntent) new BuffIntent()
    });
    MoveState moveState3 = new MoveState("SIPHON_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.SiphonMove), new AbstractIntent[2]
    {
      (AbstractIntent) new HealIntent(),
      (AbstractIntent) new BuffIntent()
    });
    MoveState moveState4 = new MoveState("PRESSURE_GUN_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.PressureGunMove), new AbstractIntent[2]
    {
      (AbstractIntent) new SingleAttackIntent((Func<Decimal>) (() => (Decimal) this.CurrentPressureGunDamage)),
      (AbstractIntent) new BuffIntent()
    });
    MoveState moveState5 = new MoveState("PRESSURE_UP_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.PressureUpMove), new AbstractIntent[2]
    {
      (AbstractIntent) new SingleAttackIntent(this.PressureUpDamage),
      (AbstractIntent) new BuffIntent()
    });
    this.AboutToBlowState = new MoveState("ABOUT_TO_BLOW_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.AboutToBlowMove), new AbstractIntent[1]
    {
      (AbstractIntent) new StunIntent()
    })
    {
      MustPerformOnceBeforeTransitioning = true
    };
    MoveState moveState6 = new MoveState("EXPLODE_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.ExplodeMove), new AbstractIntent[1]
    {
      (AbstractIntent) new DeathBlowIntent((Func<Decimal>) (() => (Decimal) this.SteamEruptionDamage))
    });
    initialState.FollowUpState = (MonsterState) moveState1;
    moveState1.FollowUpState = (MonsterState) moveState2;
    moveState2.FollowUpState = (MonsterState) moveState3;
    moveState3.FollowUpState = (MonsterState) moveState4;
    moveState4.FollowUpState = (MonsterState) moveState5;
    moveState5.FollowUpState = (MonsterState) moveState1;
    this.AboutToBlowState.FollowUpState = (MonsterState) moveState6;
    moveState6.FollowUpState = (MonsterState) moveState6;
    states.Add((MonsterState) initialState);
    states.Add((MonsterState) moveState1);
    states.Add((MonsterState) moveState2);
    states.Add((MonsterState) moveState3);
    states.Add((MonsterState) moveState4);
    states.Add((MonsterState) moveState5);
    states.Add((MonsterState) moveState6);
    states.Add((MonsterState) this.AboutToBlowState);
    return new MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine.MonsterMoveStateMachine((IEnumerable<MonsterState>) states, (MonsterState) initialState);
  }

  private async Task PressurizeMove(IReadOnlyList<Creature> targets)
  {
    SfxCmd.Play("event:/sfx/enemy/enemy_attacks/waterfall_giant/waterfall_giant_eruption");
    await CreatureCmd.TriggerAnim(this.Creature, "Heal", 0.8f);
    SteamEruptionPower steamEruptionPower = await PowerCmd.Apply<SteamEruptionPower>((PlayerChoiceContext) new ThrowingPlayerChoiceContext(), this.Creature, (Decimal) this.PressurizeAmount, this.Creature, (CardModel) null);
    this.IncrementBuildUpAnimationTrack();
  }

  private async Task PressureUpMove(IReadOnlyList<Creature> targets)
  {
    AttackCommand attackCommand = await DamageCmd.Attack((Decimal) this.PressureUpDamage).FromMonster((MonsterModel) this).WithAttackerAnim("AttackBuff", 0.15f).WithAttackerFx(sfx: "event:/sfx/enemy/enemy_attacks/waterfall_giant/waterfall_giant_attack_stomp").WithHitFx("vfx/vfx_attack_blunt").Execute((PlayerChoiceContext) null);
    SteamEruptionPower steamEruptionPower = await PowerCmd.Apply<SteamEruptionPower>((PlayerChoiceContext) new ThrowingPlayerChoiceContext(), this.Creature, 3M, this.Creature, (CardModel) null);
    this.IncrementBuildUpAnimationTrack();
  }

  private async Task StompMove(IReadOnlyList<Creature> targets)
  {
    AttackCommand attackCommand = await DamageCmd.Attack((Decimal) this.StompDamage).FromMonster((MonsterModel) this).WithAttackerAnim("AttackDebuff", 0.3f).WithAttackerFx(sfx: "event:/sfx/enemy/enemy_attacks/waterfall_giant/waterfall_giant_attack_stomp").WithHitFx("vfx/vfx_attack_blunt").Execute((PlayerChoiceContext) null);
    IReadOnlyList<WeakPower> weakPowerList = await PowerCmd.Apply<WeakPower>((PlayerChoiceContext) new ThrowingPlayerChoiceContext(), (IEnumerable<Creature>) targets, 1M, this.Creature, (CardModel) null);
    SteamEruptionPower steamEruptionPower = await PowerCmd.Apply<SteamEruptionPower>((PlayerChoiceContext) new ThrowingPlayerChoiceContext(), this.Creature, 3M, this.Creature, (CardModel) null);
    this.IncrementBuildUpAnimationTrack();
  }

  private async Task RamMove(IReadOnlyList<Creature> targets)
  {
    AttackCommand attackCommand = await DamageCmd.Attack((Decimal) this.RamDamage).FromMonster((MonsterModel) this).WithAttackerAnim("Attack", 0.3f).WithAttackerFx(sfx: "event:/sfx/enemy/enemy_attacks/waterfall_giant/waterfall_giant_attack_kick").WithHitFx("vfx/vfx_attack_blunt").Execute((PlayerChoiceContext) null);
    SteamEruptionPower steamEruptionPower = await PowerCmd.Apply<SteamEruptionPower>((PlayerChoiceContext) new ThrowingPlayerChoiceContext(), this.Creature, 3M, this.Creature, (CardModel) null);
    this.IncrementBuildUpAnimationTrack();
  }

  private async Task SiphonMove(IReadOnlyList<Creature> targets)
  {
    SfxCmd.Play("event:/sfx/enemy/enemy_attacks/waterfall_giant/waterfall_giant_eruption");
    await CreatureCmd.TriggerAnim(this.Creature, "Heal", 0.8f);
    await CreatureCmd.Heal(this.Creature, (Decimal) (this.SiphonHeal * this.CombatState.Players.Count));
    SteamEruptionPower steamEruptionPower = await PowerCmd.Apply<SteamEruptionPower>((PlayerChoiceContext) new ThrowingPlayerChoiceContext(), this.Creature, 3M, this.Creature, (CardModel) null);
    this.IncrementBuildUpAnimationTrack();
  }

  private async Task PressureGunMove(IReadOnlyList<Creature> targets)
  {
    AttackCommand attackCommand = await DamageCmd.Attack((Decimal) this.CurrentPressureGunDamage).FromMonster((MonsterModel) this).WithAttackerAnim("Attack", 0.3f).WithAttackerFx(sfx: "event:/sfx/enemy/enemy_attacks/waterfall_giant/waterfall_giant_attack_kick").WithHitFx("vfx/vfx_attack_blunt").Execute((PlayerChoiceContext) null);
    this.CurrentPressureGunDamage += this.PressureGunIncrease;
    SteamEruptionPower steamEruptionPower = await PowerCmd.Apply<SteamEruptionPower>((PlayerChoiceContext) new ThrowingPlayerChoiceContext(), this.Creature, 3M, this.Creature, (CardModel) null);
    this.IncrementBuildUpAnimationTrack();
  }

  private async Task AboutToBlowMove(IReadOnlyList<Creature> targets)
  {
    this.SteamEruptionDamage = this.Creature.GetPowerAmount<SteamEruptionPower>();
    await PowerCmd.Remove<SteamEruptionPower>(this.Creature);
    this.PressureBuildupIdx = 6;
    this.IncrementBuildUpAnimationTrack();
  }

  private async Task ExplodeMove(IReadOnlyList<Creature> targets)
  {
    this.StopAmbientSfx();
    AttackCommand attackCommand = await DamageCmd.Attack((Decimal) this.SteamEruptionDamage).FromMonster((MonsterModel) this).WithAttackerAnim("Erupt", 0.1f).WithAttackerFx(sfx: this.DeathSfx).Execute((PlayerChoiceContext) null);
    await CreatureCmd.Kill(this.Creature);
    NRunMusicController.Instance?.UpdateMusicParameter("waterfall_giant_progress", 5f);
  }

  public async Task TriggerAboutToBlowState()
  {
    this.IsAboutToBlow = true;
    SfxCmd.Play("event:/sfx/enemy/enemy_attacks/waterfall_giant/waterfall_giant_knockout");
    await CreatureCmd.SetMaxAndCurrentHp(this.Creature, 999999999M);
    this.Creature.HpDisplay = HpDisplay.InfiniteWithoutNumbers;
    this.SetMoveImmediate(this.AboutToBlowState, true);
    if (!this.CombatState.IsLiveCombat())
      return;
    NRunMusicController.Instance?.UpdateMusicParameter("waterfall_giant_progress", 2f);
    SfxCmd.SetParam("event:/sfx/enemy/enemy_attacks/waterfall_giant/waterfall_giant_ambient", "waterfall_giant_sfx", 3f);
  }

  private void IncrementBuildUpAnimationTrack()
  {
    if (!TestMode.IsOff)
      return;
    if (this.CombatState.IsLiveCombat())
    {
      NRunMusicController.Instance?.UpdateMusicParameter("waterfall_giant_progress", 1f);
      SfxCmd.SetParam("event:/sfx/enemy/enemy_attacks/waterfall_giant/waterfall_giant_ambient", "waterfall_giant_sfx", 1f);
    }
    ++this.PressureBuildupIdx;
    int num = Mathf.Clamp(Mathf.FloorToInt((float) this.PressureBuildupIdx * 0.5f), 1, 3);
    NCreature creatureNode = this.Creature.GetCreatureNode();
    if (creatureNode == null)
      return;
    SpineAnimationAccess spineAnimation = creatureNode.SpineAnimation;
    // ISSUE: explicit reference operation
    (^ref spineAnimation).SetAnimation($"_tracks/buildup{num}", track: 1);
  }

  public override CreatureAnimator GenerateAnimator(MegaSprite controller)
  {
    AnimState initialState = new AnimState("idle_loop", true);
    AnimState state1 = new AnimState("cast");
    AnimState state2 = new AnimState("attack");
    AnimState state3 = new AnimState("attack_buff");
    AnimState state4 = new AnimState("attack_debuff");
    AnimState state5 = new AnimState("heal");
    AnimState state6 = new AnimState("hurt");
    AnimState state7 = new AnimState("die");
    AnimState animState = new AnimState("die_loop", true);
    AnimState state8 = new AnimState("erupt");
    state1.NextState = initialState;
    state2.NextState = initialState;
    state3.NextState = initialState;
    state4.NextState = initialState;
    state5.NextState = initialState;
    state6.NextState = initialState;
    state7.NextState = animState;
    CreatureAnimator animator = new CreatureAnimator(initialState, controller);
    animator.AddAnyState("Cast", state1);
    animator.AddAnyState("Attack", state2);
    animator.AddAnyState("Dead", state7, (Func<bool>) (() => !this.IsAboutToBlow));
    animator.AddAnyState("Hit", state6, (Func<bool>) (() => !this.IsAboutToBlow));
    animator.AddAnyState("AttackBuff", state3);
    animator.AddAnyState("AttackDebuff", state4);
    animator.AddAnyState("Heal", state5);
    animator.AddAnyState("Erupt", state8);
    return animator;
  }

  protected override bool ShouldShowMoveInBestiary(string moveStateId)
  {
    return moveStateId != "SIPHON_MOVE";
  }

  public override List<BestiaryMonsterMove> GenerateBestiaryMoveList(
    NCreatureVisuals? creatureVisuals)
  {
    List<BestiaryMonsterMove> bestiaryMoveList = base.GenerateBestiaryMoveList(creatureVisuals);
    bestiaryMoveList.RemoveAll((Predicate<BestiaryMonsterMove>) (m =>
    {
      string stateId = m.stateId;
      return stateId == "PRESSURE_UP_MOVE" || stateId == "PRESSURE_GUN_MOVE";
    }));
    return bestiaryMoveList;
  }
}
