// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Monsters.BowlbugRock
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
using MegaCrit.Sts2.Core.Nodes.Combat;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Monsters;

public sealed class BowlbugRock : MonsterModel
{
  private const string _dizzyMove = "DIZZY_MOVE";
  private const string _stunTrigger = "Stun";
  private const string _wakeUpTrigger = "Unstun";
  private bool _isOffBalance;
  private const string _stunSfx = "event:/sfx/enemy/enemy_attacks/workbug_rock/workbug_rock_stun";
  private const string _spineSkin = "rock";

  public override int MinInitialHp
  {
    get => AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 46, 45);
  }

  public override int MaxInitialHp
  {
    get => AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 49, 48 /*0x30*/);
  }

  public static int HeadbuttDamage
  {
    get => AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 16 /*0x10*/, 15);
  }

  public bool IsOffBalance
  {
    get => this._isOffBalance;
    set
    {
      this.AssertMutable();
      this._isOffBalance = value;
    }
  }

  public override string DeathSfx => "event:/sfx/enemy/enemy_attacks/workbug_rock/workbug_rock_die";

  protected override string AttackSfx
  {
    get => "event:/sfx/enemy/enemy_attacks/workbug_rock/workbug_rock_attack";
  }

  public override DamageSfxType TakeDamageSfxType => DamageSfxType.Insect;

  public override void SetupSkins(MegaSprite spine, MegaSkeleton skeleton)
  {
    skeleton.SetSkin(skeleton.GetData().FindSkin("rock"));
    skeleton.SetSlotsToSetupPose();
  }

  public override async Task AfterAddedToRoom()
  {
    await base.AfterAddedToRoom();
    ImbalancedPower imbalancedPower = await PowerCmd.Apply<ImbalancedPower>((PlayerChoiceContext) new ThrowingPlayerChoiceContext(), this.Creature, 1M, this.Creature, (CardModel) null);
  }

  protected override MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine.MonsterMoveStateMachine GenerateMoveStateMachine()
  {
    List<MonsterState> states = new List<MonsterState>();
    MoveState moveState = new MoveState("HEADBUTT_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.HeadbuttMove), new AbstractIntent[1]
    {
      (AbstractIntent) new SingleAttackIntent(BowlbugRock.HeadbuttDamage)
    });
    MoveState move = new MoveState("DIZZY_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.DizzyMove), new AbstractIntent[1]
    {
      (AbstractIntent) new StunIntent()
    });
    ConditionalBranchState conditionalBranchState = new ConditionalBranchState("POST_HEADBUTT");
    moveState.FollowUpState = (MonsterState) conditionalBranchState;
    move.FollowUpState = (MonsterState) moveState;
    conditionalBranchState.AddState((MonsterState) move, (Func<bool>) (() => this.IsOffBalance));
    conditionalBranchState.AddState((MonsterState) moveState, (Func<bool>) (() => !this.IsOffBalance));
    states.Add((MonsterState) move);
    states.Add((MonsterState) conditionalBranchState);
    states.Add((MonsterState) moveState);
    return new MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine.MonsterMoveStateMachine((IEnumerable<MonsterState>) states, (MonsterState) moveState);
  }

  private async Task HeadbuttMove(IReadOnlyList<Creature> targets)
  {
    AttackCommand attackCommand = await DamageCmd.Attack((Decimal) BowlbugRock.HeadbuttDamage).FromMonster((MonsterModel) this).WithAttackerAnim("Attack", 0.3f).WithAttackerFx(sfx: this.AttackSfx).WithHitFx("vfx/vfx_attack_blunt").Execute((PlayerChoiceContext) null);
    if (!this.IsOffBalance)
      return;
    await this.Stun();
  }

  private async Task Stun()
  {
    SfxCmd.Play("event:/sfx/enemy/enemy_attacks/workbug_rock/workbug_rock_stun");
    await CreatureCmd.TriggerAnim(this.Creature, nameof (Stun), 0.6f);
    await CreatureCmd.Stun(this.Creature, new Func<IReadOnlyList<Creature>, Task>(this.DizzyMove));
  }

  private async Task DizzyMove(IReadOnlyList<Creature> targets)
  {
    this.IsOffBalance = false;
    await CreatureCmd.TriggerAnim(this.Creature, "Unstun", 0.6f);
  }

  public override CreatureAnimator GenerateAnimator(MegaSprite controller)
  {
    AnimState initialState = new AnimState("idle_loop", true);
    AnimState state1 = new AnimState("buff");
    AnimState state2 = new AnimState("headbutt");
    AnimState state3 = new AnimState("hurt");
    AnimState state4 = new AnimState("hurt_stunned");
    AnimState state5 = new AnimState("wake_up");
    AnimState state6 = new AnimState("die");
    AnimState state7 = new AnimState("stun");
    AnimState animState = new AnimState("stunned_loop", true);
    state1.NextState = initialState;
    state3.NextState = initialState;
    state2.NextState = initialState;
    state7.NextState = animState;
    state4.NextState = animState;
    state5.NextState = initialState;
    CreatureAnimator animator = new CreatureAnimator(initialState, controller);
    animator.AddAnyState("Dead", state6);
    animator.AddAnyState("Cast", state1);
    animator.AddAnyState("Attack", state2);
    animator.AddAnyState("Hit", state3, (Func<bool>) (() => !this.IsOffBalance));
    animator.AddAnyState("Hit", state4, (Func<bool>) (() => this.IsOffBalance));
    animator.AddAnyState("Stun", state7);
    animator.AddAnyState("Unstun", state5);
    return animator;
  }

  public override List<BestiaryMonsterMove> GenerateBestiaryMoveList(
    NCreatureVisuals? creatureVisuals)
  {
    List<BestiaryMonsterMove> bestiaryMoveList = base.GenerateBestiaryMoveList(creatureVisuals);
    bestiaryMoveList.Insert(1, BestiaryMonsterMove.FromStun(new Func<Task>(this.Stun)));
    return bestiaryMoveList;
  }
}
