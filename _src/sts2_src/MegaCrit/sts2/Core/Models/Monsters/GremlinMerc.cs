// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Monsters.GremlinMerc
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
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.MonsterMoves.Intents;
using MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Monsters;

public sealed class GremlinMerc : MonsterModel
{
  private const string _attackBuffSfx = "event:/sfx/enemy/enemy_attacks/gremlin_merc/gremlin_merc_attack_buff";
  private bool _hasSpoken;
  private const string _attackDoubleTrigger = "AttackDouble";

  public override int MinInitialHp
  {
    get => AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 51, 47);
  }

  public override int MaxInitialHp
  {
    get => AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 53, 49);
  }

  public override bool HasDeathSfx => false;

  public override DamageSfxType TakeDamageSfxType => DamageSfxType.Fur;

  private int GimmeDamage => AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 8, 7);

  private int GimmeRepeat => 2;

  private int DoubleSmashDamage
  {
    get => AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 7, 6);
  }

  private int DoubleSmashRepeat => 2;

  private int HeheDamage => AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 9, 8);

  public override async Task AfterAddedToRoom()
  {
    await base.AfterAddedToRoom();
    SurprisePower surprisePower = await PowerCmd.Apply<SurprisePower>((PlayerChoiceContext) new ThrowingPlayerChoiceContext(), this.Creature, 1M, this.Creature, (CardModel) null);
    foreach (Player player in (IEnumerable<Player>) this.CombatState.Players)
    {
      ThieveryPower mutable = (ThieveryPower) ModelDb.Power<ThieveryPower>().ToMutable();
      mutable.Target = player.Creature;
      await PowerCmd.Apply((PlayerChoiceContext) new ThrowingPlayerChoiceContext(), (PowerModel) mutable, this.Creature, 20M, this.Creature, (CardModel) null);
    }
  }

  protected override MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine.MonsterMoveStateMachine GenerateMoveStateMachine()
  {
    List<MonsterState> states = new List<MonsterState>();
    MoveState initialState = new MoveState("GIMME_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.GimmeMove), new AbstractIntent[1]
    {
      (AbstractIntent) new MultiAttackIntent(this.GimmeDamage, this.GimmeRepeat)
    });
    MoveState moveState1 = new MoveState("DOUBLE_SMASH_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.DoubleSmashMove), new AbstractIntent[2]
    {
      (AbstractIntent) new MultiAttackIntent(this.DoubleSmashDamage, this.DoubleSmashRepeat),
      (AbstractIntent) new DebuffIntent()
    });
    MoveState moveState2 = new MoveState("HEHE_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.HeheMove), new AbstractIntent[2]
    {
      (AbstractIntent) new SingleAttackIntent(this.HeheDamage),
      (AbstractIntent) new BuffIntent()
    });
    initialState.FollowUpState = (MonsterState) moveState1;
    moveState1.FollowUpState = (MonsterState) moveState2;
    moveState2.FollowUpState = (MonsterState) initialState;
    states.Add((MonsterState) initialState);
    states.Add((MonsterState) moveState1);
    states.Add((MonsterState) moveState2);
    return new MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine.MonsterMoveStateMachine((IEnumerable<MonsterState>) states, (MonsterState) initialState);
  }

  private async Task GimmeMove(IReadOnlyList<Creature> targets)
  {
    if (!this._hasSpoken)
    {
      this._hasSpoken = true;
      TalkCmd.Play(MonsterModel.L10NMonsterLookup("GREMLIN_MERC.moves.GIMME.banter"), this.Creature, VfxColor.Purple, VfxDuration.VeryShort);
      await Cmd.CustomScaledWait(0.25f, 0.5f);
    }
    VfxCmd.PlayOnCreatureCenters((IEnumerable<Creature>) targets, "vfx/vfx_coin_explosion_regular");
    AttackCommand attackCommand = await DamageCmd.Attack((Decimal) this.GimmeDamage).WithHitCount(this.GimmeRepeat).FromMonster((MonsterModel) this).WithAttackerAnim("AttackDouble", 0.15f).OnlyPlayAnimOnce().WithAttackerFx(sfx: this.AttackSfx).WithHitFx("vfx/vfx_attack_slash").Execute((PlayerChoiceContext) null);
    foreach (ThieveryPower powerInstance in this.Creature.GetPowerInstances<ThieveryPower>())
      await powerInstance.Steal();
  }

  private async Task DoubleSmashMove(IReadOnlyList<Creature> targets)
  {
    SfxCmd.Play(this.AttackSfx);
    VfxCmd.PlayOnCreatureCenters((IEnumerable<Creature>) targets, "vfx/vfx_coin_explosion_regular");
    AttackCommand attackCommand = await DamageCmd.Attack((Decimal) this.DoubleSmashDamage).WithHitCount(this.DoubleSmashRepeat).FromMonster((MonsterModel) this).WithAttackerAnim("AttackDouble", 0.15f).OnlyPlayAnimOnce().WithAttackerFx(sfx: this.AttackSfx).WithHitFx("vfx/vfx_attack_slash").Execute((PlayerChoiceContext) null);
    foreach (ThieveryPower powerInstance in this.Creature.GetPowerInstances<ThieveryPower>())
      await powerInstance.Steal();
    IReadOnlyList<WeakPower> weakPowerList = await PowerCmd.Apply<WeakPower>((PlayerChoiceContext) new ThrowingPlayerChoiceContext(), (IEnumerable<Creature>) targets, 2M, this.Creature, (CardModel) null);
  }

  private async Task HeheMove(IReadOnlyList<Creature> targets)
  {
    AttackCommand attackCommand = await DamageCmd.Attack((Decimal) this.HeheDamage).FromMonster((MonsterModel) this).WithAttackerAnim("Attack", 0.15f).WithAttackerFx(sfx: "event:/sfx/enemy/enemy_attacks/gremlin_merc/gremlin_merc_attack_buff").WithHitFx("vfx/vfx_attack_slash").Execute((PlayerChoiceContext) null);
    foreach (ThieveryPower powerInstance in this.Creature.GetPowerInstances<ThieveryPower>())
      await powerInstance.Steal();
    StrengthPower strengthPower = await PowerCmd.Apply<StrengthPower>((PlayerChoiceContext) new ThrowingPlayerChoiceContext(), this.Creature, 2M, this.Creature, (CardModel) null);
  }

  public override CreatureAnimator GenerateAnimator(MegaSprite controller)
  {
    AnimState animState = new AnimState("idle_loop", true);
    AnimState state1 = new AnimState("attack_single");
    AnimState state2 = new AnimState("attack_double");
    AnimState state3 = new AnimState("hurt");
    AnimState state4 = new AnimState("die");
    state1.NextState = animState;
    state2.NextState = animState;
    state3.NextState = animState;
    CreatureAnimator animator = new CreatureAnimator(animState, controller);
    animator.AddAnyState("Idle", animState);
    animator.AddAnyState("Attack", state1);
    animator.AddAnyState("AttackDouble", state2);
    animator.AddAnyState("Dead", state4);
    animator.AddAnyState("Hit", state3);
    return animator;
  }
}
