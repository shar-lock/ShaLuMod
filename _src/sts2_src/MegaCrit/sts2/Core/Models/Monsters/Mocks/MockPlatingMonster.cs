// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Monsters.Mocks.MockPlatingMonster
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.MonsterMoves.Intents;
using MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Monsters.Mocks;

public sealed class MockPlatingMonster : MonsterModel
{
  private int _platingAmount;

  public override bool IsMock => true;

  public override LocString Title => MonsterModel.L10NMonsterLookup("BIG_DUMMY.name");

  protected override string VisualsPath => SceneHelper.GetScenePath("creature_visuals/defect");

  public override int MinInitialHp => 9999;

  public override int MaxInitialHp => 9999;

  public int PlatingAmount
  {
    get => this._platingAmount;
    set
    {
      this.AssertMutable();
      this._platingAmount = value;
    }
  }

  protected override MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine.MonsterMoveStateMachine GenerateMoveStateMachine()
  {
    List<MonsterState> states = new List<MonsterState>();
    MoveState initialState = new MoveState("NOTHING", new Func<IReadOnlyList<Creature>, Task>(this.NothingMove), new AbstractIntent[1]
    {
      (AbstractIntent) new HiddenIntent()
    });
    initialState.FollowUpState = (MonsterState) initialState;
    states.Add((MonsterState) initialState);
    return new MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine.MonsterMoveStateMachine((IEnumerable<MonsterState>) states, (MonsterState) initialState);
  }

  public override async Task AfterAddedToRoom()
  {
    await base.AfterAddedToRoom();
    PlatingPower platingPower = await PowerCmd.Apply<PlatingPower>((PlayerChoiceContext) new ThrowingPlayerChoiceContext(), this.Creature, (Decimal) this.PlatingAmount, this.Creature, (CardModel) null);
  }

  private Task NothingMove(IReadOnlyList<Creature> targets) => Task.CompletedTask;
}
