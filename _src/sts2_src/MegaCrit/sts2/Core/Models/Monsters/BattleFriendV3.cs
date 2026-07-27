// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Monsters.BattleFriendV3
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Animation;
using MegaCrit.Sts2.Core.Bindings.MegaSpine;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.MonsterMoves.Intents;
using MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Monsters;

public sealed class BattleFriendV3 : MonsterModel
{
  public override int MinInitialHp => 300;

  public override int MaxInitialHp => 300;

  public override void SetupSkins(MegaSprite spine, MegaSkeleton skeleton)
  {
    MegaSkeletonDataResource data = skeleton.GetData();
    skeleton.SetSkin(data.FindSkin("v3"));
    skeleton.SetSlotsToSetupPose();
  }

  protected override MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine.MonsterMoveStateMachine GenerateMoveStateMachine()
  {
    MoveState initialState = new MoveState("NOTHING_MOVE", (Func<IReadOnlyList<Creature>, Task>) (_ => Task.CompletedTask), Array.Empty<AbstractIntent>());
    initialState.FollowUpState = (MonsterState) initialState;
    // ISSUE: object of a compiler-generated type is created
    return new MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine.MonsterMoveStateMachine((IEnumerable<MonsterState>) new \u003C\u003Ez__ReadOnlySingleElementList<MonsterState>((MonsterState) initialState), (MonsterState) initialState);
  }

  public override async Task AfterAddedToRoom()
  {
    BattlewornDummyTimeLimitPower dummyTimeLimitPower = await PowerCmd.Apply<BattlewornDummyTimeLimitPower>((PlayerChoiceContext) new ThrowingPlayerChoiceContext(), this.Creature, 3M, (Creature) null, (CardModel) null);
  }

  public override CreatureAnimator GenerateAnimator(MegaSprite controller)
  {
    AnimState animState1 = new AnimState("idle_loop", true);
    AnimState state1 = new AnimState("hurt");
    AnimState state2 = new AnimState("die");
    AnimState animState2 = new AnimState("die_loop", true);
    state1.NextState = animState1;
    state2.NextState = animState2;
    CreatureAnimator animator = new CreatureAnimator(animState1, controller);
    animator.AddAnyState("Idle", animState1);
    animator.AddAnyState("Dead", state2);
    animator.AddAnyState("Hit", state1);
    return animator;
  }
}
