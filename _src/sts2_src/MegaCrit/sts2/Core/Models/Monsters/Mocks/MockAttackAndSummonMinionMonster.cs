// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Monsters.Mocks.MockAttackAndSummonMinionMonster
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Commands.Builders;
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
namespace MegaCrit.Sts2.Core.Models.Monsters.Mocks;

public sealed class MockAttackAndSummonMinionMonster : MonsterModel
{
  public override bool IsMock => true;

  protected override string VisualsPath => SceneHelper.GetScenePath("creature_visuals/defect");

  public override int MinInitialHp => 10;

  public override int MaxInitialHp => 10;

  protected override MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine.MonsterMoveStateMachine GenerateMoveStateMachine()
  {
    List<MonsterState> states = new List<MonsterState>();
    MoveState initialState = new MoveState("ATTACK_AND_SUMMON_MINION", new Func<IReadOnlyList<Creature>, Task>(this.AttackAndSummonMinionMove), new AbstractIntent[1]
    {
      (AbstractIntent) new SingleAttackIntent(1)
    });
    initialState.FollowUpState = (MonsterState) initialState;
    states.Add((MonsterState) initialState);
    return new MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine.MonsterMoveStateMachine((IEnumerable<MonsterState>) states, (MonsterState) initialState);
  }

  private async Task AttackAndSummonMinionMove(IReadOnlyList<Creature> targets)
  {
    AttackCommand attackCommand = await DamageCmd.Attack(1M).FromMonster((MonsterModel) this).Execute((PlayerChoiceContext) null);
    MinionPower minionPower = await PowerCmd.Apply<MinionPower>((PlayerChoiceContext) new ThrowingPlayerChoiceContext(), await CreatureCmd.Add<BigDummy>(this.CombatState), 1M, this.Creature, (CardModel) null);
  }
}
