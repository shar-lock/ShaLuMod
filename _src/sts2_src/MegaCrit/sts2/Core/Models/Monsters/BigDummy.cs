// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Monsters.BigDummy
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.MonsterMoves.Intents;
using MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Monsters;

public sealed class BigDummy : MonsterModel
{
  public override LocString Title => MonsterModel.L10NMonsterLookup("BIG_DUMMY.name");

  protected override string VisualsPath => SceneHelper.GetScenePath("creature_visuals/defect");

  public override int MinInitialHp => 9999;

  public override int MaxInitialHp => 9999;

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

  private Task NothingMove(IReadOnlyList<Creature> targets) => Task.CompletedTask;
}
