// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Monsters.Myte
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Animation;
using MegaCrit.Sts2.Core.Bindings.MegaSpine;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Context;
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
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.TestSupport;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Monsters;

public sealed class Myte : MonsterModel
{
  private const int _toxicCount = 2;
  private const string _suckTrigger = "Suck";
  private const string _attackSfx = "event:/sfx/enemy/enemy_attacks/mite/mite_attack";
  private const string _castSfx = "event:/sfx/enemy/enemy_attacks/mite/mite_cast";
  private const string _suckSfx = "event:/sfx/enemy/enemy_attacks/mite/mite_suck";

  public override int MinInitialHp
  {
    get => AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 64 /*0x40*/, 61);
  }

  public override int MaxInitialHp
  {
    get => AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 69, 67);
  }

  private int BiteDamage
  {
    get => AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 15, 13);
  }

  private int SuckDamage => AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 6, 4);

  private int SuckStrength
  {
    get => AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 3, 2);
  }

  public override string DeathSfx => "event:/sfx/enemy/enemy_attacks/mite/mite_die";

  protected override MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine.MonsterMoveStateMachine GenerateMoveStateMachine()
  {
    List<MonsterState> states = new List<MonsterState>();
    MoveState move1 = new MoveState("TOXIC_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.ToxicMove), new AbstractIntent[1]
    {
      (AbstractIntent) new StatusIntent(2)
    });
    MoveState moveState = new MoveState("BITE_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.BiteMove), new AbstractIntent[1]
    {
      (AbstractIntent) new SingleAttackIntent(this.BiteDamage)
    });
    MoveState move2 = new MoveState("SUCK_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.SuckMove), new AbstractIntent[2]
    {
      (AbstractIntent) new SingleAttackIntent(this.SuckDamage),
      (AbstractIntent) new BuffIntent()
    });
    ConditionalBranchState initialState = new ConditionalBranchState("INIT_MOVE");
    initialState.AddState((MonsterState) move1, (Func<bool>) (() => this.Creature.SlotName == "first"));
    initialState.AddState((MonsterState) move2, (Func<bool>) (() => this.Creature.SlotName == "second"));
    move1.FollowUpState = (MonsterState) moveState;
    moveState.FollowUpState = (MonsterState) move2;
    move2.FollowUpState = (MonsterState) move1;
    states.Add((MonsterState) move1);
    states.Add((MonsterState) moveState);
    states.Add((MonsterState) move2);
    return new MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine.MonsterMoveStateMachine((IEnumerable<MonsterState>) states, (MonsterState) initialState);
  }

  private async Task ToxicMove(IReadOnlyList<Creature> targets)
  {
    if (TestMode.IsOff)
    {
      NCreature creatureNode = this.Creature.GetCreatureNode();
      if (creatureNode != null)
      {
        Creature creature = LocalContext.GetMe(this.CombatState)?.Creature;
        creatureNode.GetSpecialNode<NMyteVfx>("%NMyteVfx")?.SetTarget(creature);
      }
    }
    SfxCmd.Play("event:/sfx/enemy/enemy_attacks/mite/mite_cast");
    await CreatureCmd.TriggerAnim(this.Creature, "Cast", 0.6f);
    await CardPileCmd.AddToCombatAndPreview<Toxic>((IEnumerable<Creature>) targets, PileType.Hand, 2, (Player) null);
  }

  private async Task BiteMove(IReadOnlyList<Creature> targets)
  {
    AttackCommand attackCommand = await DamageCmd.Attack((Decimal) this.BiteDamage).FromMonster((MonsterModel) this).WithAttackerAnim("Attack", 0.5f).WithAttackerFx(sfx: "event:/sfx/enemy/enemy_attacks/mite/mite_attack").WithHitFx("vfx/vfx_bite").Execute((PlayerChoiceContext) null);
  }

  private async Task SuckMove(IReadOnlyList<Creature> targets)
  {
    AttackCommand attackCommand = await DamageCmd.Attack((Decimal) this.SuckDamage).FromMonster((MonsterModel) this).WithAttackerAnim("Suck", 0.4f).WithAttackerFx(sfx: "event:/sfx/enemy/enemy_attacks/mite/mite_suck").WithHitFx("vfx/vfx_bite").Execute((PlayerChoiceContext) null);
    StrengthPower strengthPower = await PowerCmd.Apply<StrengthPower>((PlayerChoiceContext) new ThrowingPlayerChoiceContext(), this.Creature, (Decimal) this.SuckStrength, this.Creature, (CardModel) null);
  }

  public override CreatureAnimator GenerateAnimator(MegaSprite controller)
  {
    AnimState initialState = new AnimState("idle_loop", true);
    AnimState state1 = new AnimState("cast");
    AnimState state2 = new AnimState("attack");
    AnimState state3 = new AnimState("hurt");
    AnimState state4 = new AnimState("die");
    AnimState state5 = new AnimState("suck");
    state1.NextState = initialState;
    state2.NextState = initialState;
    state3.NextState = initialState;
    state5.NextState = initialState;
    CreatureAnimator animator = new CreatureAnimator(initialState, controller);
    animator.AddAnyState("Cast", state1);
    animator.AddAnyState("Attack", state2);
    animator.AddAnyState("Suck", state5);
    animator.AddAnyState("Dead", state4);
    animator.AddAnyState("Hit", state3);
    return animator;
  }
}
