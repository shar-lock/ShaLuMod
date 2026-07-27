// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Monsters.KinPriest
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
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.MonsterMoves.Intents;
using MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine;
using MegaCrit.Sts2.Core.Nodes.Audio;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Monsters;

public sealed class KinPriest : MonsterModel
{
  private const string _theKinCustomTrackName = "the_kin_progress";
  private static readonly LocString _ritualApplyLine = MonsterModel.L10NMonsterLookup("KIN_PRIEST.moves.RITUAL.speakLine1");
  private static readonly LocString _followersDeathLine = MonsterModel.L10NMonsterLookup("KIN_PRIEST.followersDeathLine");
  private const string _grenadeTrigger = "AttackGrenade";
  private const string _laserTrigger = "AttackLaser";
  private const string _rallyTrigger = "Rally";
  private const string _attackGrenadeAnimId = "attack_grenade";
  private const string _soulBeamSfx = "event:/sfx/enemy/enemy_attacks/the_kin_priest/the_kin_priest_soul_beam";
  private const string _soulGrenadeSfx = "event:/sfx/enemy/enemy_attacks/the_kin_priest/the_kin_priest_soul_grenade";
  private const string _rallySfx = "event:/sfx/enemy/enemy_attacks/the_kin_priest/the_kin_priest_rally";
  private const int _beamRepeat = 3;
  private bool _speechUsed;

  protected override string CastSfx
  {
    get => "event:/sfx/enemy/enemy_attacks/the_kin_priest/the_kin_priest_cast";
  }

  public override string HurtSfx
  {
    get => "event:/sfx/enemy/enemy_attacks/the_kin_priest/the_kin_priest_hurt";
  }

  public override string DeathSfx
  {
    get => "event:/sfx/enemy/enemy_attacks/the_kin_priest/the_kin_priest_die";
  }

  public override int MinInitialHp
  {
    get => AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 199, 190);
  }

  public override int MaxInitialHp => this.MinInitialHp;

  private int OrbOfFrailtyDamage
  {
    get => AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 9, 8);
  }

  private int OrbOfWeaknessDamage
  {
    get => AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 9, 8);
  }

  private int BeamDamage => AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 3, 3);

  private int RitualStrength
  {
    get => AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 3, 2);
  }

  public override DamageSfxType TakeDamageSfxType => DamageSfxType.Fur;

  private bool SpeechUsed
  {
    get => this._speechUsed;
    set
    {
      this.AssertMutable();
      this._speechUsed = value;
    }
  }

  public override Task AfterDeath(
    PlayerChoiceContext choiceContext,
    Creature creature,
    bool wasRemovalPrevented,
    float deathAnimLength)
  {
    if (creature.Monster is KinFollower)
    {
      if (this.Creature.IsDead)
        return Task.CompletedTask;
      NRunMusicController.Instance?.UpdateMusicParameter("the_kin_progress", 1f);
      IReadOnlyList<Creature> teammatesOf = this.CombatState.GetTeammatesOf(this.Creature);
      if (!teammatesOf.Any<Creature>((Func<Creature, bool>) (c => c != null && c.Monster is KinFollower && c.IsAlive)))
      {
        Creature creature1 = teammatesOf.FirstOrDefault<Creature>((Func<Creature, bool>) (c => c != null && c.Monster is KinPriest && c.IsAlive));
        if (creature1 != null && creature1.Monster is KinPriest monster)
          monster.AllFollowerDeathResponse();
      }
    }
    else if (creature == this.Creature)
      NRunMusicController.Instance?.UpdateMusicParameter("the_kin_progress", 5f);
    return Task.CompletedTask;
  }

  protected override MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine.MonsterMoveStateMachine GenerateMoveStateMachine()
  {
    List<MonsterState> states = new List<MonsterState>();
    MoveState initialState = new MoveState("ORB_OF_FRAILTY_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.OrbOfFrailtyMove), new AbstractIntent[2]
    {
      (AbstractIntent) new SingleAttackIntent(this.OrbOfFrailtyDamage),
      (AbstractIntent) new DebuffIntent()
    });
    MoveState moveState1 = new MoveState("ORB_OF_WEAKNESS_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.OrbOfWeaknessMove), new AbstractIntent[2]
    {
      (AbstractIntent) new SingleAttackIntent(this.OrbOfWeaknessDamage),
      (AbstractIntent) new DebuffIntent()
    });
    MoveState moveState2 = new MoveState("BEAM_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.BeamMove), new AbstractIntent[1]
    {
      (AbstractIntent) new MultiAttackIntent(this.BeamDamage, 3)
    });
    MoveState moveState3 = new MoveState("RITUAL_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.RitualMove), new AbstractIntent[1]
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

  private async Task OrbOfFrailtyMove(IReadOnlyList<Creature> targets)
  {
    AttackCommand attackCommand = await DamageCmd.Attack((Decimal) this.OrbOfFrailtyDamage).FromMonster((MonsterModel) this).WithAttackerAnim("AttackGrenade", 0.0f).WithAttackerFx(sfx: "event:/sfx/enemy/enemy_attacks/the_kin_priest/the_kin_priest_soul_grenade").WithWaitBeforeHit(1f, 1f).WithHitVfxNode((Func<Creature, Node2D>) (t => (Node2D) NKinPriestGrenadeVfx.Create(t))).Execute((PlayerChoiceContext) null);
    IReadOnlyList<FrailPower> frailPowerList = await PowerCmd.Apply<FrailPower>((PlayerChoiceContext) new ThrowingPlayerChoiceContext(), (IEnumerable<Creature>) targets, 1M, this.Creature, (CardModel) null);
  }

  private async Task OrbOfWeaknessMove(IReadOnlyList<Creature> targets)
  {
    AttackCommand attackCommand = await DamageCmd.Attack((Decimal) this.OrbOfWeaknessDamage).FromMonster((MonsterModel) this).WithAttackerAnim("AttackGrenade", 0.0f).WithAttackerFx(sfx: "event:/sfx/enemy/enemy_attacks/the_kin_priest/the_kin_priest_soul_grenade").WithWaitBeforeHit(1f, 1f).WithHitVfxNode((Func<Creature, Node2D>) (t => (Node2D) NKinPriestGrenadeVfx.Create(t))).Execute((PlayerChoiceContext) null);
    IReadOnlyList<WeakPower> weakPowerList = await PowerCmd.Apply<WeakPower>((PlayerChoiceContext) new ThrowingPlayerChoiceContext(), (IEnumerable<Creature>) targets, 1M, this.Creature, (CardModel) null);
  }

  private async Task BeamMove(IReadOnlyList<Creature> targets)
  {
    AttackCommand attackCommand = await DamageCmd.Attack((Decimal) this.BeamDamage).WithHitCount(3).FromMonster((MonsterModel) this).WithAttackerAnim("AttackLaser", 0.4f).AfterAttackerAnim((Func<Task>) (() =>
    {
      NCombatRoom.Instance?.GetCreatureNode(this.Creature)?.GetSpecialNode<NKinPriestBeamVfx>("Visuals/Beam")?.Fire();
      SfxCmd.Play("event:/sfx/enemy/enemy_attacks/the_kin_priest/the_kin_priest_soul_beam");
      return Task.CompletedTask;
    })).WithHitFx("vfx/vfx_attack_blunt", tmpSfx: "blunt_attack.mp3").OnlyPlayAnimOnce().Execute((PlayerChoiceContext) null);
  }

  private async Task RitualMove(IReadOnlyList<Creature> targets)
  {
    if (!this.SpeechUsed)
    {
      this.SpeechUsed = true;
      TalkCmd.Play(KinPriest._ritualApplyLine, this.Creature, VfxColor.Purple, VfxDuration.Standard);
    }
    SfxCmd.Play("event:/sfx/enemy/enemy_attacks/the_kin_priest/the_kin_priest_rally");
    await CreatureCmd.TriggerAnim(this.Creature, "Rally", 1f);
    StrengthPower strengthPower = await PowerCmd.Apply<StrengthPower>((PlayerChoiceContext) new ThrowingPlayerChoiceContext(), this.Creature, (Decimal) this.RitualStrength, this.Creature, (CardModel) null);
  }

  public override CreatureAnimator GenerateAnimator(MegaSprite controller)
  {
    AnimState initialState = new AnimState("idle_loop", true);
    AnimState state1 = new AnimState("rally");
    AnimState state2 = new AnimState("attack_grenade");
    AnimState state3 = new AnimState("attack_laser");
    AnimState state4 = new AnimState("hurt");
    AnimState state5 = new AnimState("die");
    state1.NextState = initialState;
    state2.NextState = initialState;
    state3.NextState = initialState;
    state4.NextState = initialState;
    CreatureAnimator animator = new CreatureAnimator(initialState, controller);
    animator.AddAnyState("Rally", state1);
    animator.AddAnyState("AttackGrenade", state2);
    animator.AddAnyState("AttackLaser", state3);
    animator.AddAnyState("Dead", state5);
    animator.AddAnyState("Hit", state4);
    return animator;
  }

  private void AllFollowerDeathResponse()
  {
    TalkCmd.Play(KinPriest._followersDeathLine, this.Creature, VfxColor.Purple, VfxDuration.Standard);
  }

  public override List<BestiaryMonsterMove> GenerateBestiaryMoveList(
    NCreatureVisuals? creatureVisuals)
  {
    List<BestiaryMonsterMove> bestiaryMoveList = base.GenerateBestiaryMoveList(creatureVisuals);
    bestiaryMoveList.RemoveAll((Predicate<BestiaryMonsterMove>) (m => m.stateId == "ORB_OF_WEAKNESS_MOVE"));
    return bestiaryMoveList;
  }
}
