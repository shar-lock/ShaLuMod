// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Monsters.FatGremlin
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Animation;
using MegaCrit.Sts2.Core.Audio;
using MegaCrit.Sts2.Core.Bindings.MegaSpine;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Ascension;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.MonsterMoves.Intents;
using MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Monsters;

public sealed class FatGremlin : MonsterModel
{
  private const string _fleeTrigger = "FleeTrigger";
  private const string _wakeUpTrigger = "WakeUpTrigger";
  private const string _escapeSfx = "event:/sfx/enemy/enemy_attacks/gremlin_merc/fat_gremlin_escape";
  private bool _isAwake;

  public override int MinInitialHp
  {
    get => AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 14, 13);
  }

  public override int MaxInitialHp
  {
    get => AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 18, 17);
  }

  public override DamageSfxType TakeDamageSfxType => DamageSfxType.Insect;

  public override string DeathSfx => "event:/sfx/enemy/enemy_attacks/gremlin_merc/fat_gremlin_die";

  private bool IsAwake
  {
    get => this._isAwake;
    set
    {
      this.AssertMutable();
      this._isAwake = value;
    }
  }

  protected override MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine.MonsterMoveStateMachine GenerateMoveStateMachine()
  {
    List<MonsterState> states = new List<MonsterState>();
    MoveState initialState = new MoveState("SPAWNED_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.SpawnedMove), new AbstractIntent[1]
    {
      (AbstractIntent) new StunIntent()
    });
    MoveState moveState = new MoveState("FLEE_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.FleeMove), new AbstractIntent[1]
    {
      (AbstractIntent) new EscapeIntent()
    });
    initialState.FollowUpState = (MonsterState) moveState;
    moveState.FollowUpState = (MonsterState) moveState;
    states.Add((MonsterState) initialState);
    states.Add((MonsterState) moveState);
    return new MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine.MonsterMoveStateMachine((IEnumerable<MonsterState>) states, (MonsterState) initialState);
  }

  private async Task SpawnedMove(IReadOnlyList<Creature> targets)
  {
    await CreatureCmd.TriggerAnim(this.Creature, "WakeUpTrigger", 0.8f);
    this.IsAwake = true;
  }

  private async Task FleeMove(IReadOnlyList<Creature> targets)
  {
    TalkCmd.Play(MonsterModel.L10NMonsterLookup("FAT_GREMLIN.moves.FLEE.banter"), this.Creature, VfxColor.Swamp, VfxDuration.Standard);
    await Cmd.CustomScaledWait(0.75f, 1.25f);
    NCombatRoom.Instance?.GetCreatureNode(this.Creature)?.ToggleIsInteractable(false);
    SfxCmd.Play("event:/sfx/enemy/enemy_attacks/gremlin_merc/fat_gremlin_escape");
    await CreatureCmd.TriggerAnim(this.Creature, "FleeTrigger", 0.0f);
    await Cmd.Wait(1.25f);
    await CreatureCmd.Escape(this.Creature);
  }

  public override CreatureAnimator GenerateAnimator(MegaSprite controller)
  {
    string str = this.Creature.HasPower<HeistPower>() || !this.CombatState.IsLiveCombat() ? string.Empty : "_no_bag/";
    AnimState state1 = new AnimState(str + "awake_loop", true);
    AnimState initialState = new AnimState(str + "spawn");
    AnimState state2 = new AnimState(str + "flee");
    AnimState animState = new AnimState(str + "stunned_loop", true);
    AnimState state3 = new AnimState(str + "wake_up");
    AnimState state4 = new AnimState(str + "hurt_stunned");
    AnimState state5 = new AnimState(str + "hurt_awake");
    AnimState state6 = new AnimState(str + "die");
    initialState.NextState = animState;
    state4.NextState = animState;
    state5.NextState = state1;
    state3.NextState = state1;
    CreatureAnimator animator = new CreatureAnimator(initialState, controller);
    animator.AddAnyState("Idle", state1);
    animator.AddAnyState("FleeTrigger", state2);
    animator.AddAnyState("WakeUpTrigger", state3);
    animator.AddAnyState("Dead", state6);
    animator.AddAnyState("Hit", state5, (Func<bool>) (() => this.IsAwake));
    animator.AddAnyState("Hit", state4, (Func<bool>) (() => !this.IsAwake));
    return animator;
  }
}
