// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Monsters.Byrdpip
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Bindings.MegaSpine;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.MonsterMoves.Intents;
using MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Monsters;

public sealed class Byrdpip : MonsterModel
{
  public override int MinInitialHp => 9999;

  public override int MaxInitialHp => 9999;

  public override bool IsHealthBarVisible => false;

  public override void SetupSkins(MegaSprite spine, MegaSkeleton skeleton)
  {
    string skinName = !this.IsMutable ? MegaCrit.Sts2.Core.Models.Relics.Byrdpip.SkinOptions[0] : this.Creature.PetOwner.GetRelic<MegaCrit.Sts2.Core.Models.Relics.Byrdpip>().Skin;
    MegaSkeletonDataResource data = skeleton.GetData();
    skeleton.SetSkin(data.FindSkin(skinName));
    skeleton.SetSlotsToSetupPose();
  }

  protected override MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine.MonsterMoveStateMachine GenerateMoveStateMachine()
  {
    MoveState initialState = new MoveState("NOTHING_MOVE", (Func<IReadOnlyList<Creature>, Task>) (_ => Task.CompletedTask), Array.Empty<AbstractIntent>());
    initialState.FollowUpState = (MonsterState) initialState;
    // ISSUE: object of a compiler-generated type is created
    return new MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine.MonsterMoveStateMachine((IEnumerable<MonsterState>) new \u003C\u003Ez__ReadOnlySingleElementList<MonsterState>((MonsterState) initialState), (MonsterState) initialState);
  }
}
