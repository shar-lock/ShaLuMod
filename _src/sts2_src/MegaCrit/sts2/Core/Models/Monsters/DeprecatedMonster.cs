// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Monsters.DeprecatedMonster
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.MonsterMoves.Intents;
using MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Monsters;

public sealed class DeprecatedMonster : MonsterModel
{
  public override int MinInitialHp => 0;

  public override int MaxInitialHp => 0;

  public override bool HasDeathSfx => false;

  public override IEnumerable<string> AssetPaths => (IEnumerable<string>) Array.Empty<string>();

  protected override MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine.MonsterMoveStateMachine GenerateMoveStateMachine()
  {
    MoveState initialState = new MoveState("STUB", (Func<IReadOnlyList<Creature>, Task>) (_ => Task.CompletedTask), new AbstractIntent[1]
    {
      (AbstractIntent) new HiddenIntent()
    });
    initialState.FollowUpState = (MonsterState) initialState;
    // ISSUE: object of a compiler-generated type is created
    return new MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine.MonsterMoveStateMachine((IEnumerable<MonsterState>) new \u003C\u003Ez__ReadOnlySingleElementList<MonsterState>((MonsterState) initialState), (MonsterState) initialState);
  }
}
