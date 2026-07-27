// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Monsters.Fabricator
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Animation;
using MegaCrit.Sts2.Core.Bindings.MegaSpine;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.Ascension;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.MonsterMoves;
using MegaCrit.Sts2.Core.MonsterMoves.Intents;
using MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Monsters;

public sealed class Fabricator : MonsterModel
{
  private const string _fabricateTrigger = "fabricate";
  private const string _fabricatingStrikeMove = "FABRICATING_STRIKE_MOVE";
  public static readonly HashSet<MonsterModel> aggroSpawns = new HashSet<MonsterModel>()
  {
    (MonsterModel) ModelDb.Monster<Zapbot>(),
    (MonsterModel) ModelDb.Monster<Stabbot>()
  };
  public static readonly HashSet<MonsterModel> defenseSpawns = new HashSet<MonsterModel>()
  {
    (MonsterModel) ModelDb.Monster<Guardbot>(),
    (MonsterModel) ModelDb.Monster<Noisebot>()
  };
  private MonsterModel? _lastSpawned;

  public override string HurtSfx => "event:/sfx/enemy/enemy_attacks/fabricator/fabricator_hurt";

  public override int MinInitialHp
  {
    get => AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 155, 150);
  }

  public override int MaxInitialHp => this.MinInitialHp;

  private int FabricatingStrikeDamage
  {
    get => AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 21, 18);
  }

  private int DisintegrateDamage
  {
    get => AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 13, 11);
  }

  public override bool ShouldFadeAfterDeath => false;

  protected override MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine.MonsterMoveStateMachine GenerateMoveStateMachine()
  {
    List<MonsterState> states = new List<MonsterState>();
    MoveState state1 = new MoveState("FABRICATE_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.FabricateMove), new AbstractIntent[1]
    {
      (AbstractIntent) new SummonIntent()
    });
    MoveState state2 = new MoveState("FABRICATING_STRIKE_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.FabricatingStrikeMove), new AbstractIntent[2]
    {
      (AbstractIntent) new SingleAttackIntent(this.FabricatingStrikeDamage),
      (AbstractIntent) new SummonIntent()
    });
    MoveState move1 = new MoveState("DISINTEGRATE_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.DisintegrateMove), new AbstractIntent[1]
    {
      (AbstractIntent) new SingleAttackIntent(this.DisintegrateDamage)
    });
    RandomBranchState move2 = new RandomBranchState("RAND");
    move2.AddBranch((MonsterState) state1, MoveRepeatType.CanRepeatForever, (Func<float>) (() => 1f));
    move2.AddBranch((MonsterState) state2, MoveRepeatType.CanRepeatForever, (Func<float>) (() => 1f));
    ConditionalBranchState initialState = new ConditionalBranchState("fabricateBranch");
    initialState.AddState((MonsterState) move2, (Func<bool>) (() => this.CanFabricate));
    initialState.AddState((MonsterState) move1, (Func<bool>) (() => !this.CanFabricate));
    state1.FollowUpState = (MonsterState) initialState;
    move1.FollowUpState = (MonsterState) initialState;
    state2.FollowUpState = (MonsterState) initialState;
    states.Add((MonsterState) state1);
    states.Add((MonsterState) state2);
    states.Add((MonsterState) move1);
    states.Add((MonsterState) initialState);
    states.Add((MonsterState) move2);
    return new MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine.MonsterMoveStateMachine((IEnumerable<MonsterState>) states, (MonsterState) initialState);
  }

  private bool CanFabricate
  {
    get
    {
      return this.CombatState.GetTeammatesOf(this.Creature).Count<Creature>((Func<Creature, bool>) (c => c.IsAlive)) < 4;
    }
  }

  private async Task FabricateMove(IReadOnlyList<Creature> targets)
  {
    await CreatureCmd.TriggerAnim(this.Creature, "fabricate", 0.0f);
    await this.SpawnDefensiveBot();
    await this.SpawnAggroBot();
  }

  private async Task FabricatingStrikeMove(IReadOnlyList<Creature> targets)
  {
    AttackCommand attackCommand = await DamageCmd.Attack((Decimal) this.FabricatingStrikeDamage).FromMonster((MonsterModel) this).WithAttackerAnim("Attack", 0.6f).WithAttackerFx(sfx: this.AttackSfx).WithHitFx("vfx/vfx_attack_slash").Execute((PlayerChoiceContext) null);
    await this.SpawnAggroBot();
  }

  private async Task DisintegrateMove(IReadOnlyList<Creature> targets)
  {
    AttackCommand attackCommand = await DamageCmd.Attack((Decimal) this.DisintegrateDamage).FromMonster((MonsterModel) this).WithAttackerAnim("Attack", 0.6f).WithAttackerFx(sfx: this.AttackSfx).WithHitFx("vfx/vfx_attack_slash").Execute((PlayerChoiceContext) null);
  }

  private async Task SpawnDefensiveBot()
  {
    await this.SpawnBot((IEnumerable<MonsterModel>) Fabricator.defenseSpawns);
  }

  private async Task SpawnAggroBot()
  {
    await this.SpawnBot((IEnumerable<MonsterModel>) Fabricator.aggroSpawns);
  }

  private async Task SpawnBot(IEnumerable<MonsterModel> options)
  {
    if (!this.CombatState.IsLiveCombat())
      return;
    MonsterModel monsterModel = this.RunRng.MonsterAi.NextItem<MonsterModel>((IEnumerable<MonsterModel>) options.Where<MonsterModel>((Func<MonsterModel, bool>) (m => m != this._lastSpawned)).ToList<MonsterModel>());
    this._lastSpawned = monsterModel;
    MinionPower minionPower = await PowerCmd.Apply<MinionPower>((PlayerChoiceContext) new ThrowingPlayerChoiceContext(), await CreatureCmd.Add(monsterModel.ToMutable(), this.CombatState, slotName: this.CombatState.Encounter.GetNextSlot(this.CombatState)), 1M, this.Creature, (CardModel) null);
  }

  public override CreatureAnimator GenerateAnimator(MegaSprite controller)
  {
    AnimState initialState = new AnimState("idle_loop", true);
    AnimState state1 = new AnimState("cast");
    AnimState state2 = new AnimState("attack");
    AnimState state3 = new AnimState("hurt");
    AnimState state4 = new AnimState("die");
    AnimState state5 = new AnimState("fabricate");
    state1.NextState = initialState;
    state2.NextState = initialState;
    state3.NextState = initialState;
    state5.NextState = initialState;
    CreatureAnimator animator = new CreatureAnimator(initialState, controller);
    animator.AddAnyState("Cast", state1);
    animator.AddAnyState("Attack", state2);
    animator.AddAnyState("Dead", state4);
    initialState.AddBranch("Hit", state3);
    state1.AddBranch("Hit", state3);
    state3.AddBranch("Hit", state3);
    animator.AddAnyState("fabricate", state5);
    return animator;
  }

  protected override bool ShouldShowMoveInBestiary(string moveStateId)
  {
    return moveStateId != "FABRICATING_STRIKE_MOVE";
  }
}
