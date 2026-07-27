// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Monsters.Wriggler
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
using MegaCrit.Sts2.Core.MonsterMoves.Intents;
using MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Monsters;

public sealed class Wriggler : MonsterModel
{
  private const string _nastyBiteMoveId = "NASTY_BITE_MOVE";
  private const string _spawnedMoveId = "SPAWNED_MOVE";
  private const string _initMoveId = "INIT_MOVE";
  private const int _wriggleStrength = 2;
  private bool _startStunned;

  public override int MinInitialHp
  {
    get => AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 18, 17);
  }

  public override int MaxInitialHp
  {
    get => AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 22, 21);
  }

  private int BiteDamage => AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 7, 6);

  public override DamageSfxType TakeDamageSfxType => DamageSfxType.Plant;

  public bool StartStunned
  {
    get => this._startStunned;
    set
    {
      this.AssertMutable();
      this._startStunned = value;
    }
  }

  protected override MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine.MonsterMoveStateMachine GenerateMoveStateMachine()
  {
    List<MonsterState> states = new List<MonsterState>();
    MoveState move1 = new MoveState("NASTY_BITE_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.BiteMove), new AbstractIntent[1]
    {
      (AbstractIntent) new SingleAttackIntent(this.BiteDamage)
    });
    MoveState move2 = new MoveState("WRIGGLE_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.WriggleMove), new AbstractIntent[2]
    {
      (AbstractIntent) new BuffIntent(),
      (AbstractIntent) new StatusIntent(1)
    });
    MoveState moveState = new MoveState("SPAWNED_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.SpawnedMove), new AbstractIntent[1]
    {
      (AbstractIntent) new StunIntent()
    });
    ConditionalBranchState conditionalBranchState = new ConditionalBranchState("INIT_MOVE");
    conditionalBranchState.AddState((MonsterState) move1, (Func<bool>) (() => this.Creature.SlotName == "wriggler1"));
    conditionalBranchState.AddState((MonsterState) move2, (Func<bool>) (() => this.Creature.SlotName == "wriggler2"));
    conditionalBranchState.AddState((MonsterState) move1, (Func<bool>) (() => this.Creature.SlotName == "wriggler3"));
    conditionalBranchState.AddState((MonsterState) move2, (Func<bool>) (() => this.Creature.SlotName == "wriggler4"));
    moveState.FollowUpState = (MonsterState) conditionalBranchState;
    move1.FollowUpState = (MonsterState) move2;
    move2.FollowUpState = (MonsterState) move1;
    states.Add((MonsterState) moveState);
    states.Add((MonsterState) move1);
    states.Add((MonsterState) move2);
    states.Add((MonsterState) conditionalBranchState);
    MonsterState initialState = this.StartStunned ? (MonsterState) moveState : (MonsterState) conditionalBranchState;
    return new MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine.MonsterMoveStateMachine((IEnumerable<MonsterState>) states, initialState);
  }

  private Task SpawnedMove(IReadOnlyList<Creature> targets) => Task.CompletedTask;

  private async Task BiteMove(IReadOnlyList<Creature> targets)
  {
    // ISSUE: reference to a compiler-generated field
    // ISSUE: reference to a compiler-generated field
    AttackCommand attackCommand = await DamageCmd.Attack((Decimal) this.BiteDamage).FromMonster((MonsterModel) this).WithAttackerAnim("Attack", 0.15f).WithAttackerFx(sfx: this.AttackSfx).WithHitFx("vfx/vfx_bite").WithHitVfxNode(Wriggler.\u003C\u003EO.\u003C0\u003E__Create ?? (Wriggler.\u003C\u003EO.\u003C0\u003E__Create = new Func<Creature, Node2D>(NWormyImpactVfx.Create))).Execute((PlayerChoiceContext) null);
  }

  private async Task WriggleMove(IReadOnlyList<Creature> targets)
  {
    SfxCmd.Play(this.AttackSfx);
    await CreatureCmd.TriggerAnim(this.Creature, "Attack", 0.15f);
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
    await CardPileCmd.AddToCombatAndPreview<Infection>((IEnumerable<Creature>) targets, PileType.Discard, 1, (Player) null);
    StrengthPower strengthPower = await PowerCmd.Apply<StrengthPower>((PlayerChoiceContext) new ThrowingPlayerChoiceContext(), this.Creature, 2M, this.Creature, (CardModel) null);
  }

  protected override bool ShouldShowMoveInBestiary(string moveStateId)
  {
    return moveStateId != "NASTY_BITE_MOVE" && moveStateId != "SPAWNED_MOVE";
  }
}
