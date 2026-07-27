// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Monsters.PhrogParasite
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using MegaCrit.Sts2.Core.Audio;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.Ascension;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.MonsterMoves;
using MegaCrit.Sts2.Core.MonsterMoves.Intents;
using MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Monsters;

public sealed class PhrogParasite : MonsterModel
{
  private const int _lashRepeat = 4;
  private const int _infestAmt = 3;

  public override int MinInitialHp
  {
    get => AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 66, 61);
  }

  public override int MaxInitialHp
  {
    get => AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 68, 64 /*0x40*/);
  }

  private int LashDamage => AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 5, 4);

  public override DamageSfxType TakeDamageSfxType => DamageSfxType.Plant;

  public override async Task AfterAddedToRoom()
  {
    await base.AfterAddedToRoom();
    InfestedPower infestedPower = await PowerCmd.Apply<InfestedPower>((PlayerChoiceContext) new ThrowingPlayerChoiceContext(), this.Creature, 4M, this.Creature, (CardModel) null);
  }

  protected override MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine.MonsterMoveStateMachine GenerateMoveStateMachine()
  {
    List<MonsterState> states = new List<MonsterState>();
    MoveState moveState = new MoveState("INFECT_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.InfectMove), new AbstractIntent[1]
    {
      (AbstractIntent) new StatusIntent(3)
    });
    MoveState state = new MoveState("LASH_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.LashMove), new AbstractIntent[1]
    {
      (AbstractIntent) new MultiAttackIntent(this.LashDamage, 4)
    });
    RandomBranchState randomBranchState = new RandomBranchState("RAND");
    moveState.FollowUpState = (MonsterState) state;
    state.FollowUpState = (MonsterState) moveState;
    randomBranchState.AddBranch((MonsterState) moveState, MoveRepeatType.CannotRepeat);
    randomBranchState.AddBranch((MonsterState) state, MoveRepeatType.CannotRepeat);
    states.Add((MonsterState) moveState);
    states.Add((MonsterState) state);
    states.Add((MonsterState) randomBranchState);
    return new MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine.MonsterMoveStateMachine((IEnumerable<MonsterState>) states, (MonsterState) moveState);
  }

  private async Task LashMove(IReadOnlyList<Creature> targets)
  {
    // ISSUE: reference to a compiler-generated field
    // ISSUE: reference to a compiler-generated field
    AttackCommand attackCommand = await DamageCmd.Attack((Decimal) this.LashDamage).WithHitCount(4).FromMonster((MonsterModel) this).WithAttackerAnim("Attack", 0.55f).OnlyPlayAnimOnce().WithAttackerFx(sfx: this.AttackSfx).WithHitVfxNode(PhrogParasite.\u003C\u003EO.\u003C0\u003E__Create ?? (PhrogParasite.\u003C\u003EO.\u003C0\u003E__Create = new Func<Creature, Node2D>(NWormyImpactVfx.Create))).Execute((PlayerChoiceContext) null);
  }

  private async Task InfectMove(IReadOnlyList<Creature> targets)
  {
    SfxCmd.Play(this.CastSfx);
    await CreatureCmd.TriggerAnim(this.Creature, "Cast", 0.75f);
    foreach (Creature target in (IEnumerable<Creature>) targets)
    {
      NWormyImpactVfx child = NWormyImpactVfx.Create(target);
      if (child != null)
      {
        Control vfxContainer = target.GetVfxContainer();
        if (vfxContainer != null)
          ((Node) vfxContainer).AddChildSafely((Node) child);
      }
    }
    await CardPileCmd.AddToCombatAndPreview<Infection>((IEnumerable<Creature>) targets, PileType.Discard, 3, (Player) null);
  }
}
