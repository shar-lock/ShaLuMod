// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Monsters.ScrollOfBiting
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
using MegaCrit.Sts2.Core.MonsterMoves;
using MegaCrit.Sts2.Core.MonsterMoves.Intents;
using MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine;
using MegaCrit.Sts2.Core.Random;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Monsters;

public sealed class ScrollOfBiting : MonsterModel
{
  private static readonly string[] _skinOptions = new string[2]
  {
    "skin1",
    "skin2"
  };
  private const int _chewRepeat = 2;
  private const int _buffAmt = 2;
  private int _starterMoveIdx;
  private const string _attackDoubleTrigger = "ATTACK_DOUBLE";
  public const string biteSfx = "event:/sfx/enemy/enemy_attacks/scroll_of_biting/scroll_of_biting_bite";
  public const string biteDoubleSfx = "event:/sfx/enemy/enemy_attacks/scroll_of_biting/scroll_of_biting_bite_double";
  public const string buffSfx = "event:/sfx/enemy/enemy_attacks/scroll_of_biting/scroll_of_biting_buff";

  public override int MinInitialHp
  {
    get => AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 33, 30);
  }

  public override int MaxInitialHp
  {
    get => AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 39, 37);
  }

  private int ChompDamage
  {
    get => AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 16 /*0x10*/, 14);
  }

  private int ChewDamage => AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 6, 5);

  public override DamageSfxType TakeDamageSfxType => DamageSfxType.Magic;

  public override bool HasDeathSfx => true;

  public override string DeathSfx
  {
    get => "event:/sfx/enemy/enemy_attacks/scroll_of_biting/scroll_of_biting_die";
  }

  public int StarterMoveIdx
  {
    get => this._starterMoveIdx;
    set
    {
      this.AssertMutable();
      this._starterMoveIdx = value;
    }
  }

  public override void SetupSkins(MegaSprite spine, MegaSkeleton skeleton)
  {
    MegaSkin skin = spine.NewSkin("custom-skin");
    MegaSkeletonDataResource data = skeleton.GetData();
    skin.AddSkin(data.FindSkin(Rng.Chaotic.NextItem<string>((IEnumerable<string>) ScrollOfBiting._skinOptions)));
    skeleton.SetSkin(skin);
    skeleton.SetSlotsToSetupPose();
  }

  public override async Task AfterAddedToRoom()
  {
    await base.AfterAddedToRoom();
    PaperCutsPower paperCutsPower = await PowerCmd.Apply<PaperCutsPower>((PlayerChoiceContext) new ThrowingPlayerChoiceContext(), this.Creature, 2M, this.Creature, (CardModel) null);
  }

  protected override MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine.MonsterMoveStateMachine GenerateMoveStateMachine()
  {
    List<MonsterState> states = new List<MonsterState>();
    MoveState moveState1 = new MoveState("CHOMP", new Func<IReadOnlyList<Creature>, Task>(this.ChompMove), new AbstractIntent[1]
    {
      (AbstractIntent) new SingleAttackIntent(this.ChompDamage)
    });
    MoveState moveState2 = new MoveState("CHEW", new Func<IReadOnlyList<Creature>, Task>(this.ChewState), new AbstractIntent[1]
    {
      (AbstractIntent) new MultiAttackIntent(this.ChewDamage, 2)
    });
    MoveState initialState = new MoveState("MORE_TEETH", new Func<IReadOnlyList<Creature>, Task>(this.MoreTeethMove), new AbstractIntent[1]
    {
      (AbstractIntent) new BuffIntent()
    });
    RandomBranchState randomBranchState = new RandomBranchState("rand");
    moveState1.FollowUpState = (MonsterState) initialState;
    moveState2.FollowUpState = (MonsterState) randomBranchState;
    initialState.FollowUpState = (MonsterState) moveState2;
    randomBranchState.AddBranch((MonsterState) moveState1, MoveRepeatType.CannotRepeat);
    randomBranchState.AddBranch((MonsterState) moveState2, 2);
    states.Add((MonsterState) moveState1);
    states.Add((MonsterState) moveState2);
    states.Add((MonsterState) initialState);
    states.Add((MonsterState) randomBranchState);
    switch (this.StarterMoveIdx % 3)
    {
      case 0:
        return new MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine.MonsterMoveStateMachine((IEnumerable<MonsterState>) states, (MonsterState) moveState1);
      case 1:
        return new MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine.MonsterMoveStateMachine((IEnumerable<MonsterState>) states, (MonsterState) moveState2);
      default:
        return new MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine.MonsterMoveStateMachine((IEnumerable<MonsterState>) states, (MonsterState) initialState);
    }
  }

  private async Task ChompMove(IReadOnlyList<Creature> targets)
  {
    AttackCommand attackCommand = await DamageCmd.Attack((Decimal) this.ChompDamage).FromMonster((MonsterModel) this).WithAttackerAnim("Attack", 0.2f).WithAttackerFx(sfx: "event:/sfx/enemy/enemy_attacks/scroll_of_biting/scroll_of_biting_bite").WithHitFx("vfx/vfx_bite").Execute((PlayerChoiceContext) null);
  }

  private async Task ChewState(IReadOnlyList<Creature> targets)
  {
    AttackCommand attackCommand = await DamageCmd.Attack((Decimal) this.ChewDamage).WithHitCount(2).FromMonster((MonsterModel) this).WithAttackerAnim("ATTACK_DOUBLE", 0.2f).OnlyPlayAnimOnce().WithAttackerFx(sfx: "event:/sfx/enemy/enemy_attacks/scroll_of_biting/scroll_of_biting_bite_double").WithHitFx("vfx/vfx_bite").Execute((PlayerChoiceContext) null);
  }

  private async Task MoreTeethMove(IReadOnlyList<Creature> targets)
  {
    SfxCmd.Play("event:/sfx/enemy/enemy_attacks/scroll_of_biting/scroll_of_biting_buff");
    await CreatureCmd.TriggerAnim(this.Creature, "Cast", 0.8f);
    StrengthPower strengthPower = await PowerCmd.Apply<StrengthPower>((PlayerChoiceContext) new ThrowingPlayerChoiceContext(), this.Creature, 2M, this.Creature, (CardModel) null);
  }

  public override CreatureAnimator GenerateAnimator(MegaSprite controller)
  {
    AnimState initialState = new AnimState("idle_loop", true);
    AnimState state1 = new AnimState("buff");
    AnimState state2 = new AnimState("attack");
    AnimState state3 = new AnimState("attack_double");
    AnimState state4 = new AnimState("hurt");
    AnimState state5 = new AnimState("die");
    state1.NextState = initialState;
    state2.NextState = initialState;
    state4.NextState = initialState;
    state3.NextState = initialState;
    CreatureAnimator animator = new CreatureAnimator(initialState, controller);
    animator.AddAnyState("Cast", state1);
    animator.AddAnyState("Attack", state2);
    animator.AddAnyState("Dead", state5);
    animator.AddAnyState("Hit", state4);
    animator.AddAnyState("ATTACK_DOUBLE", state3);
    return animator;
  }
}
