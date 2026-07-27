// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Monsters.SlimedBerserker
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
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Monsters;

public sealed class SlimedBerserker : MonsterModel
{
  private const string _smotherMove = "SMOTHER_MOVE";
  private const int _pummelingRepeat = 4;
  private const int _leechingDrain = 3;
  private const int _vomitSlimeInDiscard = 10;
  private const string _hugTrigger = "Hug";
  private const string _vomitTrigger = "Vomit";

  public override int MinInitialHp
  {
    get => AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 281, 261);
  }

  public override int MaxInitialHp => this.MinInitialHp;

  public override DamageSfxType TakeDamageSfxType => DamageSfxType.Slime;

  public override bool HasDeathSfx => true;

  protected override string CastSfx
  {
    get => "event:/sfx/enemy/enemy_attacks/slimed_berserker/slimed_berserker_buff";
  }

  private string SlimeSfx
  {
    get => "event:/sfx/enemy/enemy_attacks/slimed_berserker/slimed_berserker_slime";
  }

  private int PummelingDamage
  {
    get => AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 5, 4);
  }

  private int SmotherDamage
  {
    get => AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 33, 30);
  }

  protected override MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine.MonsterMoveStateMachine GenerateMoveStateMachine()
  {
    List<MonsterState> states = new List<MonsterState>();
    MoveState initialState = new MoveState("VOMIT_ICHOR_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.VomitIchorMove), new AbstractIntent[1]
    {
      (AbstractIntent) new StatusIntent(10)
    });
    MoveState moveState1 = new MoveState("LEECHING_HUG_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.LeechingHugMove), new AbstractIntent[2]
    {
      (AbstractIntent) new DebuffIntent(),
      (AbstractIntent) new BuffIntent()
    });
    MoveState moveState2 = new MoveState("SMOTHER_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.SmotherMove), new AbstractIntent[1]
    {
      (AbstractIntent) new SingleAttackIntent(this.SmotherDamage)
    });
    MoveState moveState3 = new MoveState("FURIOUS_PUMMELING_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.FuriousPummelingMove), new AbstractIntent[1]
    {
      (AbstractIntent) new MultiAttackIntent(this.PummelingDamage, 4)
    });
    initialState.FollowUpState = (MonsterState) moveState3;
    moveState3.FollowUpState = (MonsterState) moveState1;
    moveState1.FollowUpState = (MonsterState) moveState2;
    moveState2.FollowUpState = (MonsterState) initialState;
    states.Add((MonsterState) initialState);
    states.Add((MonsterState) moveState2);
    states.Add((MonsterState) moveState1);
    states.Add((MonsterState) moveState3);
    return new MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine.MonsterMoveStateMachine((IEnumerable<MonsterState>) states, (MonsterState) initialState);
  }

  private async Task VomitIchorMove(IReadOnlyList<Creature> targets)
  {
    SfxCmd.Play(this.SlimeSfx);
    await CreatureCmd.TriggerAnim(this.Creature, "Vomit", 0.7f);
    await CardPileCmd.AddToCombatAndPreview<Slimed>((IEnumerable<Creature>) targets, PileType.Discard, 10, (Player) null);
  }

  private async Task LeechingHugMove(IReadOnlyList<Creature> targets)
  {
    SfxCmd.Play(this.CastSfx);
    await CreatureCmd.TriggerAnim(this.Creature, "Hug", 0.65f);
    IReadOnlyList<WeakPower> weakPowerList = await PowerCmd.Apply<WeakPower>((PlayerChoiceContext) new ThrowingPlayerChoiceContext(), (IEnumerable<Creature>) targets, 3M, (Creature) null, (CardModel) null);
    StrengthPower strengthPower = await PowerCmd.Apply<StrengthPower>((PlayerChoiceContext) new ThrowingPlayerChoiceContext(), this.Creature, 3M, this.Creature, (CardModel) null);
  }

  private async Task FuriousPummelingMove(IReadOnlyList<Creature> targets)
  {
    AttackCommand attackCommand = await DamageCmd.Attack((Decimal) this.PummelingDamage).WithHitCount(4).OnlyPlayAnimOnce().FromMonster((MonsterModel) this).WithAttackerAnim("Attack", 0.2f).WithAttackerFx(sfx: this.AttackSfx).Execute((PlayerChoiceContext) null);
  }

  private async Task SmotherMove(IReadOnlyList<Creature> targets)
  {
    AttackCommand attackCommand = await DamageCmd.Attack((Decimal) this.SmotherDamage).FromMonster((MonsterModel) this).WithAttackerAnim("Attack", 0.2f).WithAttackerFx(sfx: this.AttackSfx).Execute((PlayerChoiceContext) null);
  }

  public override CreatureAnimator GenerateAnimator(MegaSprite controller)
  {
    AnimState initialState = new AnimState("idle_loop", true);
    AnimState state1 = new AnimState("hug");
    AnimState state2 = new AnimState("vomit");
    AnimState state3 = new AnimState("attack");
    AnimState state4 = new AnimState("hurt");
    AnimState state5 = new AnimState("die");
    state1.NextState = initialState;
    state2.NextState = initialState;
    state3.NextState = initialState;
    state4.NextState = initialState;
    CreatureAnimator animator = new CreatureAnimator(initialState, controller);
    animator.AddAnyState("Hug", state1);
    animator.AddAnyState("Vomit", state2);
    animator.AddAnyState("Attack", state3);
    animator.AddAnyState("Dead", state5);
    animator.AddAnyState("Hit", state4);
    return animator;
  }

  protected override bool ShouldShowMoveInBestiary(string moveStateId)
  {
    return moveStateId != "SMOTHER_MOVE";
  }
}
