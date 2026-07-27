// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Monsters.FakeMerchantMonster
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
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.MonsterMoves;
using MegaCrit.Sts2.Core.MonsterMoves.Intents;
using MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.Random;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Monsters;

public sealed class FakeMerchantMonster : MonsterModel
{
  private const string _spewCoinsTrigger = "spew";
  private const string _throwRelicTrigger = "throw";
  private const int _spewCoinsDamage = 2;
  private const int _spewCoinsRepeat = 8;
  private const string _attackMultiTrigger = "attack_multi";

  public override int MinInitialHp
  {
    get => AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 175, 165);
  }

  public override int MaxInitialHp
  {
    get => AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 175, 165);
  }

  private int SwipeDamage
  {
    get => AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 15, 13);
  }

  private int ThrowRelicDamage
  {
    get => AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 10, 9);
  }

  public override DamageSfxType TakeDamageSfxType => DamageSfxType.Slime;

  public override string DeathSfx => "event:/sfx/npcs/reverse_merchant/reverse_merchant_die";

  public override string HurtSfx => "event:/sfx/npcs/reverse_merchant/reverse_merchant_hurt";

  protected override MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine.MonsterMoveStateMachine GenerateMoveStateMachine()
  {
    List<MonsterState> states = new List<MonsterState>();
    MoveState moveState = new MoveState("SWIPE_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.SwipeMove), new AbstractIntent[1]
    {
      (AbstractIntent) new SingleAttackIntent(this.SwipeDamage)
    });
    MoveState state1 = new MoveState("SPEW_COINS_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.SpewCoinsMove), new AbstractIntent[1]
    {
      (AbstractIntent) new MultiAttackIntent(2, 8)
    });
    MoveState state2 = new MoveState("THROW_RELIC_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.ThrowRelicMove), new AbstractIntent[2]
    {
      (AbstractIntent) new SingleAttackIntent(this.ThrowRelicDamage),
      (AbstractIntent) new DebuffIntent()
    });
    MoveState state3 = new MoveState("ENRAGE_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.EnrageMove), new AbstractIntent[1]
    {
      (AbstractIntent) new BuffIntent()
    });
    RandomBranchState randomBranchState1 = new RandomBranchState("RAND_MOVE");
    randomBranchState1.AddBranch((MonsterState) moveState, MoveRepeatType.CannotRepeat);
    randomBranchState1.AddBranch((MonsterState) state1, MoveRepeatType.CannotRepeat);
    randomBranchState1.AddBranch((MonsterState) state2, MoveRepeatType.CannotRepeat);
    randomBranchState1.AddBranch((MonsterState) state3, 3, MoveRepeatType.CannotRepeat);
    moveState.FollowUpState = (MonsterState) randomBranchState1;
    state1.FollowUpState = (MonsterState) randomBranchState1;
    state3.FollowUpState = (MonsterState) randomBranchState1;
    RandomBranchState randomBranchState2 = new RandomBranchState("RAND_ATTACK_MOVE");
    randomBranchState2.AddBranch((MonsterState) moveState, MoveRepeatType.CannotRepeat);
    randomBranchState2.AddBranch((MonsterState) state1, MoveRepeatType.CannotRepeat);
    randomBranchState2.AddBranch((MonsterState) state2, MoveRepeatType.CannotRepeat);
    state2.FollowUpState = (MonsterState) randomBranchState2;
    states.Add((MonsterState) moveState);
    states.Add((MonsterState) state1);
    states.Add((MonsterState) state2);
    states.Add((MonsterState) state3);
    states.Add((MonsterState) randomBranchState1);
    states.Add((MonsterState) randomBranchState2);
    return new MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine.MonsterMoveStateMachine((IEnumerable<MonsterState>) states, (MonsterState) moveState);
  }

  private async Task SwipeMove(IReadOnlyList<Creature> targets)
  {
    await this.ShowDialogueForMove("SWIPE");
    AttackCommand attackCommand = await DamageCmd.Attack((Decimal) this.SwipeDamage).FromMonster((MonsterModel) this).WithAttackerAnim("Attack", 0.15f).WithAttackerFx(sfx: this.AttackSfx).WithHitFx("vfx/vfx_attack_slash").Execute((PlayerChoiceContext) null);
  }

  private async Task SpewCoinsMove(IReadOnlyList<Creature> targets)
  {
    await this.ShowDialogueForMove("SPEW_COINS");
    AttackCommand attackCommand = await DamageCmd.Attack(2M).FromMonster((MonsterModel) this).WithHitCount(8).OnlyPlayAnimOnce().WithAttackerAnim("spew", 0.15f).WithAttackerFx(sfx: this.AttackSfx).WithHitFx("vfx/vfx_attack_slash").Execute((PlayerChoiceContext) null);
  }

  private async Task ThrowRelicMove(IReadOnlyList<Creature> targets)
  {
    await this.ShowDialogueForMove("THROW_RELIC");
    AttackCommand attackCommand = await DamageCmd.Attack((Decimal) this.ThrowRelicDamage).FromMonster((MonsterModel) this).WithAttackerAnim("throw", 0.15f).WithAttackerFx(sfx: this.AttackSfx).WithHitFx("vfx/vfx_attack_slash").Execute((PlayerChoiceContext) null);
    IReadOnlyList<FrailPower> frailPowerList = await PowerCmd.Apply<FrailPower>((PlayerChoiceContext) new ThrowingPlayerChoiceContext(), (IEnumerable<Creature>) targets, 1M, this.Creature, (CardModel) null);
  }

  private async Task EnrageMove(IReadOnlyList<Creature> targets)
  {
    await this.ShowDialogueForMove("ENRAGE");
    await CreatureCmd.TriggerAnim(this.Creature, "Cast", 0.5f);
    StrengthPower strengthPower = await PowerCmd.Apply<StrengthPower>((PlayerChoiceContext) new ThrowingPlayerChoiceContext(), this.Creature, 2M, this.Creature, (CardModel) null);
  }

  public override CreatureAnimator GenerateAnimator(MegaSprite controller)
  {
    AnimState initialState = new AnimState("combat_idle_loop", true);
    AnimState state1 = new AnimState("attack");
    AnimState state2 = new AnimState("attack_multi");
    AnimState state3 = new AnimState("attack_throw");
    AnimState state4 = new AnimState("buff");
    AnimState state5 = new AnimState("hurt");
    AnimState state6 = new AnimState("die");
    state1.NextState = initialState;
    state2.NextState = initialState;
    state3.NextState = initialState;
    state4.NextState = initialState;
    state5.NextState = initialState;
    CreatureAnimator animator = new CreatureAnimator(initialState, controller);
    animator.AddAnyState("Dead", state6);
    animator.AddAnyState("Hit", state5);
    animator.AddAnyState("Attack", state1);
    animator.AddAnyState("spew", state2);
    animator.AddAnyState("throw", state3);
    animator.AddAnyState("Cast", state4);
    return animator;
  }

  private async Task ShowDialogueForMove(string moveId)
  {
    LocString line = Rng.Chaotic.NextItem<LocString>(this.GetLinesForMove(moveId));
    if (line == null)
      return;
    TalkCmd.Play(line, this.Creature, VfxColor.Blue);
    await Cmd.Wait(0.5f);
  }

  private IEnumerable<LocString> GetLinesForMove(string moveId)
  {
    return (IEnumerable<LocString>) LocManager.Instance.GetTable("monsters").GetLocStringsWithPrefix($"{this.Id.Entry}.moves.{moveId}.speakLine");
  }
}
