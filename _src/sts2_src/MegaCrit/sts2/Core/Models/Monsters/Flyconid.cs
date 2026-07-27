// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Monsters.Flyconid
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
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
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.TestSupport;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Monsters;

public sealed class Flyconid : MonsterModel
{
  public override int MinInitialHp
  {
    get => AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 51, 47);
  }

  public override int MaxInitialHp
  {
    get => AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 53, 49);
  }

  private int SmashDamage
  {
    get => AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 12, 11);
  }

  private int SporeDamage
  {
    get => AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 9, 8);
  }

  protected override MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine.MonsterMoveStateMachine GenerateMoveStateMachine()
  {
    List<MonsterState> states = new List<MonsterState>();
    MoveState state1 = new MoveState("VULNERABLE_SPORES_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.VulnerableSporesMove), new AbstractIntent[1]
    {
      (AbstractIntent) new DebuffIntent()
    });
    MoveState state2 = new MoveState("FRAIL_SPORES_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.FrailSporesMove), new AbstractIntent[2]
    {
      (AbstractIntent) new SingleAttackIntent(this.SporeDamage),
      (AbstractIntent) new DebuffIntent()
    });
    MoveState state3 = new MoveState("SMASH_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.SmashMove), new AbstractIntent[1]
    {
      (AbstractIntent) new SingleAttackIntent(this.SmashDamage)
    });
    RandomBranchState randomBranchState = new RandomBranchState("RAND");
    RandomBranchState initialState = new RandomBranchState("INITIAL");
    state1.FollowUpState = (MonsterState) randomBranchState;
    state2.FollowUpState = (MonsterState) randomBranchState;
    state3.FollowUpState = (MonsterState) randomBranchState;
    randomBranchState.AddBranch((MonsterState) state1, 3, MoveRepeatType.CannotRepeat);
    randomBranchState.AddBranch((MonsterState) state2, 2, MoveRepeatType.CannotRepeat);
    randomBranchState.AddBranch((MonsterState) state3, MoveRepeatType.CannotRepeat);
    initialState.AddBranch((MonsterState) state2, 2, MoveRepeatType.CannotRepeat);
    initialState.AddBranch((MonsterState) state3, MoveRepeatType.CannotRepeat);
    states.Add((MonsterState) state1);
    states.Add((MonsterState) state2);
    states.Add((MonsterState) state3);
    states.Add((MonsterState) randomBranchState);
    states.Add((MonsterState) initialState);
    return new MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine.MonsterMoveStateMachine((IEnumerable<MonsterState>) states, (MonsterState) initialState);
  }

  private async Task VulnerableSporesMove(IReadOnlyList<Creature> targets)
  {
    if (TestMode.IsOff)
      ((Node) this.Creature.GetCreatureNode()?.Visuals.GetCurrentBody()).GetNode<NFlyconidSporesVfx>(NodePath.op_Implicit("%VfxController"))?.SetSporeTypeIsVulnerable(true);
    SfxCmd.Play(this.CastSfx);
    await CreatureCmd.TriggerAnim(this.Creature, "Cast", 0.5f);
    IReadOnlyList<VulnerablePower> vulnerablePowerList = await PowerCmd.Apply<VulnerablePower>((PlayerChoiceContext) new ThrowingPlayerChoiceContext(), (IEnumerable<Creature>) targets, 2M, this.Creature, (CardModel) null);
  }

  private async Task FrailSporesMove(IReadOnlyList<Creature> targets)
  {
    if (TestMode.IsOff)
      ((Node) this.Creature.GetCreatureNode()?.Visuals.GetCurrentBody()).GetNode<NFlyconidSporesVfx>(NodePath.op_Implicit("%VfxController"))?.SetSporeTypeIsVulnerable(false);
    AttackCommand attackCommand = await DamageCmd.Attack((Decimal) this.SporeDamage).FromMonster((MonsterModel) this).WithAttackerAnim("Cast", 0.5f).WithAttackerFx(sfx: this.CastSfx).WithWaitBeforeHit(0.0f, 0.6f).WithHitVfxNode((Func<Creature, Node2D>) (t => (Node2D) NSporeImpactVfx.Create(t, new Color("8aad7d")))).Execute((PlayerChoiceContext) null);
    IReadOnlyList<FrailPower> frailPowerList = await PowerCmd.Apply<FrailPower>((PlayerChoiceContext) new ThrowingPlayerChoiceContext(), (IEnumerable<Creature>) targets, 2M, this.Creature, (CardModel) null);
  }

  private async Task SmashMove(IReadOnlyList<Creature> targets)
  {
    AttackCommand attackCommand = await DamageCmd.Attack((Decimal) this.SmashDamage).FromMonster((MonsterModel) this).WithAttackerAnim("Attack", 0.3f).WithAttackerFx(sfx: this.AttackSfx).WithHitFx("vfx/vfx_attack_blunt").Execute((PlayerChoiceContext) null);
  }
}
