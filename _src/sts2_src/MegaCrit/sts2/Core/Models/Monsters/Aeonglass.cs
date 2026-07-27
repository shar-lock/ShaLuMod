// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Monsters.Aeonglass
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
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.MonsterMoves.Intents;
using MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine;
using MegaCrit.Sts2.Core.Nodes.Audio;
using MegaCrit.Sts2.Core.ValueProps;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Monsters;

public sealed class Aeonglass : MonsterModel
{
  private const string _attackHeavyTrigger = "AttackHeavy";
  private const string _attackDoubleTrigger = "AttackDouble";
  private const string _aeonglassTrackName = "queen_progress";
  private int _additionalStrength;
  private int _witherUpgradeCount;

  public override int MinInitialHp
  {
    get => AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 535, 512 /*0x0200*/);
  }

  public override int MaxInitialHp => this.MinInitialHp;

  public override DamageSfxType TakeDamageSfxType => DamageSfxType.Stone;

  private int EbbDamage
  {
    get => AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 26, 22);
  }

  private int EbbBlock => 33;

  private int EyeLasersDamage
  {
    get => AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 12, 11);
  }

  private int EyeLasersRepeat => 2;

  private int IncreasingIntensityBaseStrength
  {
    get => AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 4, 3);
  }

  private int WitherAmount
  {
    get => AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 2, 1);
  }

  private int AdditionalStrength
  {
    get => this._additionalStrength;
    set
    {
      this.AssertMutable();
      this._additionalStrength = value;
    }
  }

  public int WitherUpgradeCount
  {
    get => this._witherUpgradeCount;
    set
    {
      this.AssertMutable();
      this._witherUpgradeCount = value;
    }
  }

  private int IncreasingIntensityTotalStrength
  {
    get => this.IncreasingIntensityBaseStrength + this.AdditionalStrength;
  }

  public override async Task AfterAddedToRoom()
  {
    await base.AfterAddedToRoom();
    NRunMusicController.Instance?.UpdateMusicParameter("queen_progress", 1f);
    foreach (Creature creature in (IEnumerable<Creature>) this.CombatState.GetOpponentsOf(this.Creature))
    {
      WitheringPresencePower mutable = (WitheringPresencePower) ModelDb.Power<WitheringPresencePower>().ToMutable();
      mutable.Target = creature;
      await PowerCmd.Apply((PlayerChoiceContext) new ThrowingPlayerChoiceContext(), (PowerModel) mutable, this.Creature, 6M, this.Creature, (CardModel) null);
    }
    ArtifactPower artifactPower = await PowerCmd.Apply<ArtifactPower>((PlayerChoiceContext) new ThrowingPlayerChoiceContext(), this.Creature, 3M, this.Creature, (CardModel) null);
  }

  public override Task AfterDeath(
    PlayerChoiceContext choiceContext,
    Creature creature,
    bool wasRemovalPrevented,
    float deathAnimLength)
  {
    if (creature != this.Creature)
      return Task.CompletedTask;
    NRunMusicController.Instance?.UpdateMusicParameter("queen_progress", 5f);
    return Task.CompletedTask;
  }

  protected override MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine.MonsterMoveStateMachine GenerateMoveStateMachine()
  {
    List<MonsterState> states = new List<MonsterState>();
    MoveState initialState = new MoveState("EBB_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.EbbMove), new AbstractIntent[2]
    {
      (AbstractIntent) new SingleAttackIntent(this.EbbDamage),
      (AbstractIntent) new DefendIntent()
    });
    MoveState moveState1 = new MoveState("EYE_LASERS_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.EyeLasersMove), new AbstractIntent[1]
    {
      (AbstractIntent) new MultiAttackIntent(this.EyeLasersDamage, this.EyeLasersRepeat)
    });
    MoveState moveState2 = new MoveState("INCREASING_INTENSITY_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.IncreasingIntensityMove), new AbstractIntent[2]
    {
      (AbstractIntent) new StatusIntent(this.WitherAmount),
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

  private async Task EbbMove(IReadOnlyList<Creature> targets)
  {
    AttackCommand attackCommand = await DamageCmd.Attack((Decimal) this.EbbDamage).FromMonster((MonsterModel) this).WithAttackerAnim("AttackHeavy", 0.3f).WithHitFx("vfx/vfx_attack_blunt").Execute((PlayerChoiceContext) null);
    Decimal num = await CreatureCmd.GainBlock(this.Creature, (Decimal) this.EbbBlock, ValueProp.Move, (CardPlay) null);
  }

  private async Task EyeLasersMove(IReadOnlyList<Creature> targets)
  {
    AttackCommand attackCommand = await DamageCmd.Attack((Decimal) this.EyeLasersDamage).FromMonster((MonsterModel) this).WithHitCount(this.EyeLasersRepeat).WithAttackerAnim("AttackDouble", 0.4f).OnlyPlayAnimOnce().WithHitFx("vfx/vfx_attack_blunt").Execute((PlayerChoiceContext) null);
  }

  private async Task IncreasingIntensityMove(IReadOnlyList<Creature> targets)
  {
    await CreatureCmd.TriggerAnim(this.Creature, "Cast", 0.4f);
    foreach (Creature target in (IEnumerable<Creature>) targets)
    {
      if (target.Player?.PlayerCombatState != null)
      {
        foreach (CardModel allCard in target.Player.PlayerCombatState.AllCards)
        {
          if (allCard is Wither wither)
            wither.FakeUpgrade();
        }
      }
    }
    this.WitherUpgradeCount++;
    await CardPileCmd.AddToCombatAndPreview<Wither>((IEnumerable<Creature>) targets, PileType.Discard, this.WitherAmount, (Player) null);
    StrengthPower strengthPower = await PowerCmd.Apply<StrengthPower>((PlayerChoiceContext) new ThrowingPlayerChoiceContext(), this.Creature, (Decimal) this.IncreasingIntensityTotalStrength, this.Creature, (CardModel) null);
    this.AdditionalStrength++;
  }

  public override Task AfterCardGeneratedForCombat(CardModel card, Player? creator)
  {
    if (!(card is Wither wither))
      return Task.CompletedTask;
    this.MatchWitherToUpgradeCount(wither);
    return Task.CompletedTask;
  }

  public void MatchWitherToUpgradeCount(Wither wither)
  {
    for (int index = 0; index < this.WitherUpgradeCount; ++index)
      wither.FakeUpgrade();
  }

  public override CreatureAnimator GenerateAnimator(MegaSprite controller)
  {
    AnimState animState = new AnimState("idle_loop", true);
    AnimState state1 = new AnimState("wither");
    AnimState state2 = new AnimState("attack_heavy");
    AnimState state3 = new AnimState("attack_double");
    AnimState state4 = new AnimState("hurt");
    AnimState state5 = new AnimState("die");
    state1.NextState = animState;
    state2.NextState = animState;
    state3.NextState = animState;
    state4.NextState = animState;
    CreatureAnimator animator = new CreatureAnimator(animState, controller);
    animator.AddAnyState("Idle", animState);
    animator.AddAnyState("Cast", state1);
    animator.AddAnyState("AttackHeavy", state2);
    animator.AddAnyState("AttackDouble", state3);
    animator.AddAnyState("Dead", state5);
    animator.AddAnyState("Hit", state4);
    return animator;
  }
}
