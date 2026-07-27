// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Monsters.Mocks.MockReattachMonster
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.MonsterMoves.Intents;
using MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Monsters.Mocks;

public sealed class MockReattachMonster : MonsterModel
{
  private MoveState _deadState;

  public override bool IsMock => true;

  public override LocString Title => MonsterModel.L10NMonsterLookup("BIG_DUMMY.name");

  public override int MinInitialHp => 1;

  public override int MaxInitialHp => 1;

  public MoveState DeadState
  {
    get => this._deadState;
    private set
    {
      this.AssertMutable();
      this._deadState = value;
    }
  }

  public override async Task AfterAddedToRoom()
  {
    await base.AfterAddedToRoom();
    ReattachPower reattachPower = await PowerCmd.Apply<ReattachPower>((PlayerChoiceContext) new ThrowingPlayerChoiceContext(), this.Creature, 1M, this.Creature, (CardModel) null);
  }

  protected override MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine.MonsterMoveStateMachine GenerateMoveStateMachine()
  {
    List<MonsterState> states = new List<MonsterState>();
    MoveState initialState = new MoveState("NOTHING", new Func<IReadOnlyList<Creature>, Task>(this.NothingMove), new AbstractIntent[1]
    {
      (AbstractIntent) new HiddenIntent()
    });
    initialState.FollowUpState = (MonsterState) initialState;
    MoveState moveState = new MoveState("REATTACH_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.ReattachMove), new AbstractIntent[1]
    {
      (AbstractIntent) new HealIntent()
    })
    {
      MustPerformOnceBeforeTransitioning = true,
      FollowUpState = (MonsterState) initialState
    };
    this.DeadState = new MoveState("DEAD_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.NothingMove), Array.Empty<AbstractIntent>())
    {
      FollowUpState = (MonsterState) moveState
    };
    states.Add((MonsterState) initialState);
    states.Add((MonsterState) moveState);
    states.Add((MonsterState) this.DeadState);
    return new MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine.MonsterMoveStateMachine((IEnumerable<MonsterState>) states, (MonsterState) initialState);
  }

  private Task NothingMove(IReadOnlyList<Creature> targets) => Task.CompletedTask;

  private async Task ReattachMove(IReadOnlyList<Creature> targets)
  {
    await this.Creature.GetPower<ReattachPower>().DoReattach();
  }
}
