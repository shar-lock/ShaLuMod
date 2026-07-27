// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Monsters.Osty
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using MegaCrit.Sts2.Core.Animation;
using MegaCrit.Sts2.Core.Bindings.MegaSpine;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.MonsterMoves.Intents;
using MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Monsters;

public sealed class Osty : MonsterModel
{
  public const float attackerAnimDelay = 0.3f;
  public const string pokeAnim = "attack_poke";
  public const string ostyAttackSfx = "event:/sfx/characters/osty/osty_attack";

  public static Vector2 MinOffset => new Vector2(150f, -75f);

  public static Vector2 MaxOffset => new Vector2(250f, -75f);

  public static Vector2 ScaleRange => new Vector2(1f, 2f);

  public override int MinInitialHp => 1;

  public override int MaxInitialHp => 1;

  public override string DeathSfx => "event:/sfx/characters/osty/osty_die";

  public override bool HasDeathSfx => true;

  public override bool IsHealthBarVisible => this.Creature.IsAlive;

  protected override MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine.MonsterMoveStateMachine GenerateMoveStateMachine()
  {
    MoveState initialState = new MoveState("NOTHING_MOVE", (Func<IReadOnlyList<Creature>, Task>) (_ => Task.CompletedTask), Array.Empty<AbstractIntent>());
    initialState.FollowUpState = (MonsterState) initialState;
    // ISSUE: object of a compiler-generated type is created
    return new MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine.MonsterMoveStateMachine((IEnumerable<MonsterState>) new \u003C\u003Ez__ReadOnlySingleElementList<MonsterState>((MonsterState) initialState), (MonsterState) initialState);
  }

  public override CreatureAnimator GenerateAnimator(MegaSprite controller)
  {
    AnimState initialState = new AnimState("idle_loop", true);
    AnimState state1 = new AnimState("cast");
    AnimState state2 = new AnimState("attack");
    AnimState state3 = new AnimState("attack_poke");
    AnimState state4 = new AnimState("hurt");
    AnimState state5 = new AnimState("die");
    AnimState animState = new AnimState("dead_loop", true);
    AnimState state6 = new AnimState("revive");
    initialState.AddBranch("Hit", state4);
    state1.NextState = initialState;
    state1.AddBranch("Hit", state4);
    state2.NextState = initialState;
    state2.AddBranch("Hit", state4);
    state3.NextState = initialState;
    state3.AddBranch("Hit", state4);
    state4.NextState = initialState;
    state4.AddBranch("Hit", state4);
    state5.NextState = animState;
    state6.NextState = initialState;
    CreatureAnimator animator = new CreatureAnimator(initialState, controller);
    animator.AddAnyState("Attack", state2);
    animator.AddAnyState("Cast", state1);
    animator.AddAnyState("Dead", state5);
    animator.AddAnyState("attack_poke", state3);
    animator.AddAnyState("Revive", state6);
    return animator;
  }

  public static bool CheckMissingWithAnim(Player owner)
  {
    NCombatRoom.Instance?.ShakeOstyIfDead(owner);
    return owner.IsOstyMissing;
  }
}
