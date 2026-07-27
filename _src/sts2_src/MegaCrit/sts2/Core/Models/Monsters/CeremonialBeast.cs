// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Monsters.CeremonialBeast
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
using MegaCrit.Sts2.Core.Nodes;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.Nodes.Vfx.Cards;
using MegaCrit.Sts2.Core.Nodes.Vfx.Utilities;
using MegaCrit.Sts2.Core.Random;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Monsters;

public sealed class CeremonialBeast : MonsterModel
{
  private const string _stunMove = "STUN_MOVE";
  private const string _plowTrigger = "Plow";
  private const string _plowEndTrigger = "EndPlow";
  private const string _stunTrigger = "Stun";
  private const string _unStunTrigger = "Unstun";
  private const string _plowHitTrigger = "PlowHit";
  private const string _plowSfx = "event:/sfx/enemy/enemy_attacks/ceremonial_beast/ceremonial_beast_plow";
  private const string _plowEndSfx = "event:/sfx/enemy/enemy_attacks/ceremonial_beast/ceremonial_beast_plow_end";
  private const string _shrillSfx = "event:/sfx/enemy/enemy_attacks/ceremonial_beast/ceremonial_beast_shrill";
  private const string _stunSfx = "event:/sfx/enemy/enemy_attacks/ceremonial_beast/ceremonial_beast_stun";
  private bool _isStunnedByPlowRemoval;
  private bool _isInSecondPhase;
  private bool _inMidCharge;
  private MoveState _beastCryState;

  public override int MinInitialHp
  {
    get => AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 262, 252);
  }

  public override int MaxInitialHp => this.MinInitialHp;

  private int PlowAmount
  {
    get => AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 160 /*0xA0*/, 150);
  }

  private int PlowDamage
  {
    get => AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 20, 18);
  }

  private int PlowStrength => 2;

  private int StompDamage
  {
    get => AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 17, 15);
  }

  private int CrushDamage
  {
    get => AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 19, 17);
  }

  private int CrushStrength
  {
    get => AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 4, 3);
  }

  public override bool ShouldDisappearFromDoom => false;

  public override bool ShouldFadeAfterDeath => false;

  public override string DeathSfx
  {
    get => "event:/sfx/enemy/enemy_attacks/ceremonial_beast/ceremonial_beast_die";
  }

  public override DamageSfxType TakeDamageSfxType => DamageSfxType.Fur;

  private bool IsStunnedByPlowRemoval
  {
    get => this._isStunnedByPlowRemoval;
    set
    {
      this.AssertMutable();
      this._isStunnedByPlowRemoval = value;
    }
  }

  public bool IsInSecondPhase
  {
    get => this._isInSecondPhase;
    private set
    {
      this.AssertMutable();
      this._isInSecondPhase = value;
    }
  }

  private bool ShouldPlayRegularHurtAnim => !this.IsStunnedByPlowRemoval && !this.InMidCharge;

  private bool InMidCharge
  {
    get => this._inMidCharge;
    set
    {
      this.AssertMutable();
      this._inMidCharge = value;
    }
  }

  public MoveState BeastCryState
  {
    get => this._beastCryState;
    set
    {
      this.AssertMutable();
      this._beastCryState = value;
    }
  }

  protected override MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine.MonsterMoveStateMachine GenerateMoveStateMachine()
  {
    List<MonsterState> states = new List<MonsterState>();
    MoveState initialState = new MoveState("STAMP_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.StampMove), new AbstractIntent[1]
    {
      (AbstractIntent) new BuffIntent()
    });
    MoveState moveState1 = new MoveState("PLOW_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.PlowMove), new AbstractIntent[2]
    {
      (AbstractIntent) new SingleAttackIntent(this.PlowDamage),
      (AbstractIntent) new BuffIntent()
    });
    MoveState moveState2 = new MoveState("STUN_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.StunnedMove), new AbstractIntent[1]
    {
      (AbstractIntent) new StunIntent()
    })
    {
      MustPerformOnceBeforeTransitioning = true
    };
    this.BeastCryState = new MoveState("BEAST_CRY_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.BeastCryMove), new AbstractIntent[1]
    {
      (AbstractIntent) new DebuffIntent()
    });
    MoveState moveState3 = new MoveState("STOMP_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.StompMove), new AbstractIntent[1]
    {
      (AbstractIntent) new SingleAttackIntent(this.StompDamage)
    });
    MoveState moveState4 = new MoveState("CRUSH_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.CrushMove), new AbstractIntent[2]
    {
      (AbstractIntent) new SingleAttackIntent(this.CrushDamage),
      (AbstractIntent) new BuffIntent()
    });
    initialState.FollowUpState = (MonsterState) moveState1;
    moveState1.FollowUpState = (MonsterState) moveState1;
    moveState2.FollowUpState = (MonsterState) this.BeastCryState;
    this.BeastCryState.FollowUpState = (MonsterState) moveState3;
    moveState3.FollowUpState = (MonsterState) moveState4;
    moveState4.FollowUpState = (MonsterState) this.BeastCryState;
    states.Add((MonsterState) moveState1);
    states.Add((MonsterState) initialState);
    states.Add((MonsterState) moveState2);
    states.Add((MonsterState) this.BeastCryState);
    states.Add((MonsterState) moveState3);
    states.Add((MonsterState) moveState4);
    return new MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine.MonsterMoveStateMachine((IEnumerable<MonsterState>) states, (MonsterState) initialState);
  }

  private async Task StampMove(IReadOnlyList<Creature> targets)
  {
    SfxCmd.Play(this.AttackSfx);
    await CreatureCmd.TriggerAnim(this.Creature, "Attack", 0.6f);
    await Cmd.CustomScaledWait(0.0f, 0.4f);
    PlowPower plowPower = await PowerCmd.Apply<PlowPower>((PlayerChoiceContext) new ThrowingPlayerChoiceContext(), this.Creature, (Decimal) this.PlowAmount, this.Creature, (CardModel) null);
  }

  private async Task PlowMove(IReadOnlyList<Creature> targets)
  {
    SfxCmd.Play("event:/sfx/enemy/enemy_attacks/ceremonial_beast/ceremonial_beast_plow");
    this.InMidCharge = true;
    await CreatureCmd.TriggerAnim(this.Creature, "Plow", 0.0f);
    await Cmd.Wait(0.5f);
    Control vfxContainer1 = this.Creature.GetVfxContainer();
    if (vfxContainer1 != null)
      ((Node) vfxContainer1).AddChildSafely((Node) NHorizontalLinesVfx.Create(new Color("BFFFC880"), 1.2, false));
    await Cmd.Wait(0.5f);
    NCombatRoom.Instance?.RadialBlur(VfxPosition.Left);
    VfxCmd.PlayOnCreatureCenters((IEnumerable<Creature>) targets, "vfx/vfx_attack_blunt");
    using (IEnumerator<Creature> enumerator = targets.GetEnumerator())
    {
      if (enumerator.MoveNext())
      {
        Creature current = enumerator.Current;
        Control vfxContainer2 = current.GetVfxContainer();
        if (vfxContainer2 != null)
          ((Node) vfxContainer2).AddChildSafely((Node) NLineBurstVfx.Create(current));
      }
    }
    NGame.Instance?.ScreenShake(ShakeStrength.Strong, ShakeDuration.Normal, 180f + Rng.Chaotic.NextFloat(-10f, 10f));
    AttackCommand attackCommand = await DamageCmd.Attack((Decimal) this.PlowDamage).FromMonster((MonsterModel) this).WithNoAttackerAnim().Execute((PlayerChoiceContext) null);
    NGame.Instance?.DoHitStop(ShakeStrength.Strong, ShakeDuration.Normal);
    this.InMidCharge = false;
    await Cmd.Wait(0.2f);
    SfxCmd.Play("event:/sfx/enemy/enemy_attacks/ceremonial_beast/ceremonial_beast_plow_end");
    await CreatureCmd.TriggerAnim(this.Creature, "EndPlow", 0.0f);
    await Cmd.Wait(0.5f);
    StrengthPower strengthPower = await PowerCmd.Apply<StrengthPower>((PlayerChoiceContext) new ThrowingPlayerChoiceContext(), this.Creature, (Decimal) this.PlowStrength, this.Creature, (CardModel) null);
  }

  public async Task SetStunned()
  {
    this.IsStunnedByPlowRemoval = true;
    this.IsInSecondPhase = true;
    SfxCmd.Play("event:/sfx/enemy/enemy_attacks/ceremonial_beast/ceremonial_beast_stun");
    await CreatureCmd.TriggerAnim(this.Creature, "Stun", 0.6f);
  }

  public async Task StunnedMove(IReadOnlyList<Creature> targets)
  {
    this.IsStunnedByPlowRemoval = false;
    await CreatureCmd.TriggerAnim(this.Creature, "Unstun", 0.6f);
  }

  private async Task BeastCryMove(IReadOnlyList<Creature> targets)
  {
    SfxCmd.Play("event:/sfx/enemy/enemy_attacks/ceremonial_beast/ceremonial_beast_shrill");
    await CreatureCmd.TriggerAnim(this.Creature, "Cast", 0.0f);
    await Cmd.Wait(0.3f);
    VfxCmd.PlayOnCreatureCenter(this.Creature, "vfx/vfx_scream");
    await Cmd.Wait(0.75f);
    IReadOnlyList<RingingPower> ringingPowerList = await PowerCmd.Apply<RingingPower>((PlayerChoiceContext) new ThrowingPlayerChoiceContext(), (IEnumerable<Creature>) targets, 1M, this.Creature, (CardModel) null);
  }

  private async Task StompMove(IReadOnlyList<Creature> targets)
  {
    AttackCommand attackCommand = await DamageCmd.Attack((Decimal) this.StompDamage).FromMonster((MonsterModel) this).WithAttackerAnim("Attack", 1f).WithAttackerFx(sfx: this.AttackSfx).AfterAttackerAnim((Func<Task>) (() =>
    {
      NGame.Instance?.ScreenShake(ShakeStrength.Strong, ShakeDuration.Normal, 180f + Rng.Chaotic.NextFloat(-10f, 10f));
      return Task.CompletedTask;
    })).WithHitFx("vfx/vfx_attack_slash").WithHitVfxNode((Func<Creature, Node2D>) (_ => (Node2D) NSpikeSplashVfx.Create(this.Creature, VfxColor.Cyan))).Execute((PlayerChoiceContext) null);
  }

  private async Task CrushMove(IReadOnlyList<Creature> targets)
  {
    SfxCmd.Play(this.AttackSfx);
    AttackCommand attackCommand = await DamageCmd.Attack((Decimal) this.CrushDamage).FromMonster((MonsterModel) this).WithAttackerAnim("Attack", 1f).WithAttackerFx(sfx: this.AttackSfx).AfterAttackerAnim((Func<Task>) (() =>
    {
      NGame.Instance?.ScreenShake(ShakeStrength.Strong, ShakeDuration.Normal, 180f + Rng.Chaotic.NextFloat(-10f, 10f));
      return Task.CompletedTask;
    })).WithHitFx("vfx/vfx_attack_slash").WithHitVfxNode((Func<Creature, Node2D>) (_ => (Node2D) NSpikeSplashVfx.Create(this.Creature, VfxColor.Cyan))).Execute((PlayerChoiceContext) null);
    StrengthPower strengthPower = await PowerCmd.Apply<StrengthPower>((PlayerChoiceContext) new ThrowingPlayerChoiceContext(), this.Creature, (Decimal) this.CrushStrength, this.Creature, (CardModel) null);
  }

  public override CreatureAnimator GenerateAnimator(MegaSprite controller)
  {
    AnimState initialState = new AnimState("idle_loop", true);
    AnimState state1 = new AnimState("shrill");
    AnimState state2 = new AnimState("attack");
    AnimState state3 = new AnimState("plow");
    AnimState state4 = new AnimState("plow_end");
    AnimState state5 = new AnimState("plow_end_die");
    AnimState state6 = new AnimState("stun");
    AnimState animState = new AnimState("stun_loop", true);
    AnimState state7 = new AnimState("wake_up");
    AnimState state8 = new AnimState("hurt");
    AnimState state9 = new AnimState("die");
    initialState.AddBranch("Plow", state3);
    state1.NextState = initialState;
    state2.NextState = initialState;
    state8.NextState = initialState;
    state3.AddBranch("EndPlow", state4);
    state4.NextState = initialState;
    state6.NextState = animState;
    state7.NextState = initialState;
    CreatureAnimator animator = new CreatureAnimator(initialState, controller);
    animator.AddAnyState("Unstun", state7);
    animator.AddAnyState("Dead", state9, (Func<bool>) (() => !this.InMidCharge));
    animator.AddAnyState("Dead", state5, (Func<bool>) (() => this.InMidCharge));
    initialState.AddBranch("Hit", state8, (Func<bool>) (() => this.ShouldPlayRegularHurtAnim));
    state1.AddBranch("Hit", state8, (Func<bool>) (() => this.ShouldPlayRegularHurtAnim));
    state8.AddBranch("Hit", state8, (Func<bool>) (() => this.ShouldPlayRegularHurtAnim));
    state2.AddBranch("Hit", state8, (Func<bool>) (() => this.ShouldPlayRegularHurtAnim));
    state6.AddBranch("Hit", state8, (Func<bool>) (() => this.ShouldPlayRegularHurtAnim));
    animState.AddBranch("Hit", state8, (Func<bool>) (() => this.ShouldPlayRegularHurtAnim));
    state7.AddBranch("Hit", state8, (Func<bool>) (() => this.ShouldPlayRegularHurtAnim));
    initialState.AddBranch("Hit", state6, (Func<bool>) (() => this.IsStunnedByPlowRemoval));
    state1.AddBranch("Hit", state6, (Func<bool>) (() => this.IsStunnedByPlowRemoval));
    state8.AddBranch("Hit", state6, (Func<bool>) (() => this.IsStunnedByPlowRemoval));
    state2.AddBranch("Hit", state6, (Func<bool>) (() => this.IsStunnedByPlowRemoval));
    state6.AddBranch("Hit", state6, (Func<bool>) (() => this.IsStunnedByPlowRemoval));
    animState.AddBranch("Hit", state6, (Func<bool>) (() => this.IsStunnedByPlowRemoval));
    state7.AddBranch("Hit", state6, (Func<bool>) (() => this.IsStunnedByPlowRemoval));
    animator.AddAnyState("Stun", state6);
    animator.AddAnyState("Plow", state3);
    animator.AddAnyState("Attack", state2);
    animator.AddAnyState("Cast", state1);
    animator.AddAnyState("PlowHit", state8);
    return animator;
  }

  protected override bool ShouldShowMoveInBestiary(string moveStateId)
  {
    return moveStateId != "STUN_MOVE";
  }

  public override List<BestiaryMonsterMove> GenerateBestiaryMoveList(
    NCreatureVisuals? creatureVisuals)
  {
    List<BestiaryMonsterMove> bestiaryMoveList = base.GenerateBestiaryMoveList(creatureVisuals);
    bestiaryMoveList.RemoveAll((Predicate<BestiaryMonsterMove>) (m =>
    {
      string stateId = m.stateId;
      return stateId == "STAMP_MOVE" || stateId == "CRUSH_MOVE";
    }));
    bestiaryMoveList.Insert(2, BestiaryMonsterMove.FromStun(new Func<Task>(this.SetStunned)));
    return bestiaryMoveList;
  }
}
