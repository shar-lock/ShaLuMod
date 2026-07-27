// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Monsters.EyeWithTeeth
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Animation;
using MegaCrit.Sts2.Core.Audio;
using MegaCrit.Sts2.Core.Bindings.MegaSpine;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.MonsterMoves.Intents;
using MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Monsters;

public sealed class EyeWithTeeth : MonsterModel
{
  private const int _distractAmount = 3;

  public override int MinInitialHp => 6;

  public override int MaxInitialHp => this.MinInitialHp;

  public override DamageSfxType TakeDamageSfxType => DamageSfxType.Magic;

  public override bool ShouldDisappearFromDoom => false;

  public override async Task AfterAddedToRoom()
  {
    await base.AfterAddedToRoom();
    IllusionPower illusionPower = await PowerCmd.Apply<IllusionPower>((PlayerChoiceContext) new ThrowingPlayerChoiceContext(), this.Creature, 1M, this.Creature, (CardModel) null);
  }

  protected override MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine.MonsterMoveStateMachine GenerateMoveStateMachine()
  {
    List<MonsterState> states = new List<MonsterState>();
    MoveState initialState = new MoveState("DISTRACT_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.DistractMove), new AbstractIntent[1]
    {
      (AbstractIntent) new StatusIntent(3)
    });
    initialState.FollowUpState = (MonsterState) initialState;
    states.Add((MonsterState) initialState);
    return new MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine.MonsterMoveStateMachine((IEnumerable<MonsterState>) states, (MonsterState) initialState);
  }

  private async Task DistractMove(IReadOnlyList<Creature> targets)
  {
    SfxCmd.Play(this.AttackSfx);
    await CreatureCmd.TriggerAnim(this.Creature, "Attack", 0.7f);
    VfxCmd.PlayOnCreatureCenters((IEnumerable<Creature>) targets, "vfx/vfx_attack_slash");
    await CardPileCmd.AddToCombatAndPreview<Dazed>((IEnumerable<Creature>) targets, PileType.Discard, 3, (Player) null);
  }

  public override CreatureAnimator GenerateAnimator(MegaSprite controller)
  {
    AnimState initialState = new AnimState("idle_loop", true);
    AnimState state1 = new AnimState("attack");
    AnimState state2 = new AnimState("die");
    state1.NextState = initialState;
    CreatureAnimator animator = new CreatureAnimator(initialState, controller);
    animator.AddAnyState("Attack", state1);
    animator.AddAnyState("Dead", state2, (Func<bool>) (() => !this.CombatState.GetTeammatesOf(this.Creature).Any<Creature>((Func<Creature, bool>) (t => t != null && t.IsPrimaryEnemy && t.IsAlive))));
    return animator;
  }
}
