// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Monsters.TerrorEel
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

public sealed class TerrorEel : MonsterModel
{
  private const string _stunMove = "STUN_MOVE";
  private const string _debuffSfx = "event:/sfx/enemy/enemy_attacks/terror_eel/terror_eel_debuff";
  private const string _attackMultiSfx = "event:/sfx/enemy/enemy_attacks/terror_eel/terror_eel_attack_multi";
  private const string _thrashMoveId = "THRASH_MOVE";
  private const int _hpNormal = 140;
  private const int _hpTough = 150;
  private MoveState _terrorState;
  private const string _attackTripleTrigger = "AttackTripleTrigger";

  public override int MinInitialHp
  {
    get => AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 150, 140);
  }

  public override int MaxInitialHp => this.MinInitialHp;

  private int ShriekAmount
  {
    get => AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 75, 70);
  }

  private int CrashDamage
  {
    get => AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 18, 16 /*0x10*/);
  }

  private int ThrashDamage
  {
    get => AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 4, 3);
  }

  private int ThrashRepeat => 3;

  public MoveState TerrorState
  {
    get => this._terrorState;
    private set
    {
      this.AssertMutable();
      this._terrorState = value;
    }
  }

  public override DamageSfxType TakeDamageSfxType => DamageSfxType.Slime;

  public override async Task AfterAddedToRoom()
  {
    await base.AfterAddedToRoom();
    ShriekPower shriekPower = await PowerCmd.Apply<ShriekPower>((PlayerChoiceContext) new ThrowingPlayerChoiceContext(), this.Creature, (Decimal) this.ShriekAmount, this.Creature, (CardModel) null);
  }

  protected override MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine.MonsterMoveStateMachine GenerateMoveStateMachine()
  {
    List<MonsterState> states = new List<MonsterState>();
    MoveState initialState = new MoveState("CRASH_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.CrashMove), new AbstractIntent[1]
    {
      (AbstractIntent) new SingleAttackIntent(this.CrashDamage)
    });
    MoveState moveState1 = new MoveState("THRASH_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.ThrashMove), new AbstractIntent[2]
    {
      (AbstractIntent) new MultiAttackIntent(this.ThrashDamage, this.ThrashRepeat),
      (AbstractIntent) new BuffIntent()
    });
    MoveState moveState2 = new MoveState("STUN_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.StunMove), new AbstractIntent[1]
    {
      (AbstractIntent) new StunIntent()
    });
    this.TerrorState = new MoveState("TERROR_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.TerrorMove), new AbstractIntent[1]
    {
      (AbstractIntent) new DebuffIntent()
    });
    initialState.FollowUpState = (MonsterState) moveState1;
    moveState1.FollowUpState = (MonsterState) initialState;
    moveState2.FollowUpState = (MonsterState) this.TerrorState;
    this.TerrorState.FollowUpState = (MonsterState) initialState;
    states.Add((MonsterState) initialState);
    states.Add((MonsterState) moveState1);
    states.Add((MonsterState) moveState2);
    states.Add((MonsterState) this.TerrorState);
    return new MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine.MonsterMoveStateMachine((IEnumerable<MonsterState>) states, (MonsterState) initialState);
  }

  private async Task CrashMove(IReadOnlyList<Creature> targets)
  {
    AttackCommand attackCommand = await DamageCmd.Attack((Decimal) this.CrashDamage).FromMonster((MonsterModel) this).WithAttackerAnim("Attack", 0.2f).WithAttackerFx(sfx: this.AttackSfx).WithHitFx("vfx/vfx_attack_slash").Execute((PlayerChoiceContext) null);
  }

  private async Task ThrashMove(IReadOnlyList<Creature> targets)
  {
    AttackCommand attackCommand = await DamageCmd.Attack((Decimal) this.ThrashDamage).WithHitCount(this.ThrashRepeat).FromMonster((MonsterModel) this).WithAttackerAnim("AttackTripleTrigger", 0.25f).OnlyPlayAnimOnce().WithAttackerFx(sfx: "event:/sfx/enemy/enemy_attacks/terror_eel/terror_eel_attack_multi").WithHitFx("vfx/vfx_attack_slash").Execute((PlayerChoiceContext) null);
    VigorPower vigorPower = await PowerCmd.Apply<VigorPower>((PlayerChoiceContext) new ThrowingPlayerChoiceContext(), this.Creature, 6M, this.Creature, (CardModel) null);
  }

  private Task StunMove(IReadOnlyList<Creature> targets) => Task.CompletedTask;

  private async Task TerrorMove(IReadOnlyList<Creature> targets)
  {
    SfxCmd.Play("event:/sfx/enemy/enemy_attacks/terror_eel/terror_eel_debuff");
    await CreatureCmd.TriggerAnim(this.Creature, "Cast", 0.0f);
    await Cmd.Wait(0.7f);
    VfxCmd.PlayOnCreatureCenter(this.Creature, "vfx/vfx_scream");
    await Cmd.CustomScaledWait(0.1f, 0.3f);
    IReadOnlyList<VulnerablePower> vulnerablePowerList = await PowerCmd.Apply<VulnerablePower>((PlayerChoiceContext) new ThrowingPlayerChoiceContext(), (IEnumerable<Creature>) targets, 99M, this.Creature, (CardModel) null);
  }

  public override CreatureAnimator GenerateAnimator(MegaSprite controller)
  {
    AnimState initialState = new AnimState("idle_loop", true);
    AnimState state1 = new AnimState("cast");
    AnimState state2 = new AnimState("attack");
    AnimState state3 = new AnimState("attack_triple");
    AnimState state4 = new AnimState("hurt");
    AnimState state5 = new AnimState("die");
    state1.NextState = initialState;
    state2.NextState = initialState;
    state4.NextState = initialState;
    state3.NextState = initialState;
    CreatureAnimator animator = new CreatureAnimator(initialState, controller);
    animator.AddAnyState("Cast", state1);
    animator.AddAnyState("Attack", state2);
    animator.AddAnyState("AttackTripleTrigger", state3);
    animator.AddAnyState("Dead", state5);
    animator.AddAnyState("Hit", state4);
    return animator;
  }

  protected override bool ShouldShowMoveInBestiary(string moveStateId)
  {
    return moveStateId != "STUN_MOVE";
  }
}
