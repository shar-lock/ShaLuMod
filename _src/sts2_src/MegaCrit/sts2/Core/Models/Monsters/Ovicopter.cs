// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Monsters.Ovicopter
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Animation;
using MegaCrit.Sts2.Core.Audio;
using MegaCrit.Sts2.Core.Bindings.MegaSpine;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.Ascension;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.MonsterMoves.Intents;
using MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Monsters;

public sealed class Ovicopter : MonsterModel
{
  private const string _tenderizerMove = "TENDERIZER_MOVE";
  private const string _layTrigger = "layTrigger";
  private const string _buffTrigger = "buffTrigger";
  private const string _idleLoop = "event:/sfx/enemy/enemy_attacks/egg_layer/egg_layer_idle_loop";
  private const string _laySfx = "event:/sfx/enemy/enemy_attacks/egg_layer/egg_layer_lay";

  public override int MinInitialHp
  {
    get => AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 126, 124);
  }

  public override int MaxInitialHp
  {
    get => AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 132, 130);
  }

  private int SmashDamage
  {
    get => AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 17, 16 /*0x10*/);
  }

  private int TenderizerDamage
  {
    get => AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 8, 7);
  }

  private int NutritionalPasteStrengthAmount
  {
    get => AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 4, 3);
  }

  public override string DeathSfx => "event:/sfx/enemy/enemy_attacks/egg_layer/egg_layer_die";

  protected override string AttackSfx
  {
    get => "event:/sfx/enemy/enemy_attacks/egg_layer/egg_layer_attack";
  }

  public override DamageSfxType TakeDamageSfxType => DamageSfxType.Slime;

  public override async Task AfterAddedToRoom()
  {
    await base.AfterAddedToRoom();
    SfxCmd.PlayLoop("event:/sfx/enemy/enemy_attacks/egg_layer/egg_layer_idle_loop");
  }

  public override void BeforeRemovedFromRoom()
  {
    SfxCmd.StopLoop("event:/sfx/enemy/enemy_attacks/egg_layer/egg_layer_idle_loop");
  }

  private bool CanLay
  {
    get
    {
      return this.CombatState.GetTeammatesOf(this.Creature).Count<Creature>((Func<Creature, bool>) (c => c.IsAlive)) <= 3;
    }
  }

  protected override MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine.MonsterMoveStateMachine GenerateMoveStateMachine()
  {
    List<MonsterState> states = new List<MonsterState>();
    MoveState moveState1 = new MoveState("LAY_EGGS_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.LayEggsMove), new AbstractIntent[1]
    {
      (AbstractIntent) new SummonIntent()
    });
    MoveState moveState2 = new MoveState("SMASH_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.SmashMove), new AbstractIntent[1]
    {
      (AbstractIntent) new SingleAttackIntent(this.SmashDamage)
    });
    MoveState moveState3 = new MoveState("TENDERIZER_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.TenderizerMove), new AbstractIntent[2]
    {
      (AbstractIntent) new SingleAttackIntent(this.TenderizerDamage),
      (AbstractIntent) new DebuffIntent()
    });
    MoveState move = new MoveState("NUTRITIONAL_PASTE_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.NutritionalPasteMove), new AbstractIntent[1]
    {
      (AbstractIntent) new BuffIntent()
    });
    ConditionalBranchState conditionalBranchState = new ConditionalBranchState("SUMMON_BRANCH_STATE");
    moveState1.FollowUpState = (MonsterState) moveState2;
    move.FollowUpState = (MonsterState) moveState2;
    moveState2.FollowUpState = (MonsterState) moveState3;
    moveState3.FollowUpState = (MonsterState) conditionalBranchState;
    conditionalBranchState.AddState((MonsterState) moveState1, (Func<bool>) (() => this.CanLay));
    conditionalBranchState.AddState((MonsterState) move, (Func<bool>) (() => !this.CanLay));
    states.Add((MonsterState) move);
    states.Add((MonsterState) moveState1);
    states.Add((MonsterState) moveState2);
    states.Add((MonsterState) moveState3);
    states.Add((MonsterState) conditionalBranchState);
    return new MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine.MonsterMoveStateMachine((IEnumerable<MonsterState>) states, (MonsterState) moveState1);
  }

  private async Task LayEggsMove(IReadOnlyList<Creature> targets)
  {
    SfxCmd.Play("event:/sfx/enemy/enemy_attacks/egg_layer/egg_layer_lay");
    await CreatureCmd.TriggerAnim(this.Creature, "layTrigger", 1f);
    for (int i = 0; i < 3; ++i)
    {
      EncounterModel encounter = this.CombatState.Encounter;
      string slotName = encounter != null ? encounter.Slots.LastOrDefault<string>((Func<string, bool>) (s => this.CombatState.Enemies.All<Creature>((Func<Creature, bool>) (c => c.SlotName != s)))) : (string) null;
      if (slotName != null)
      {
        MinionPower minionPower = await PowerCmd.Apply<MinionPower>((PlayerChoiceContext) new ThrowingPlayerChoiceContext(), await CreatureCmd.Add<ToughEgg>(this.CombatState, slotName), 1M, this.Creature, (CardModel) null);
      }
    }
  }

  private async Task NutritionalPasteMove(IReadOnlyList<Creature> targets)
  {
    SfxCmd.Play("event:/sfx/enemy/enemy_attacks/egg_layer/egg_layer_lay");
    await CreatureCmd.TriggerAnim(this.Creature, "buffTrigger", 1f);
    StrengthPower strengthPower = await PowerCmd.Apply<StrengthPower>((PlayerChoiceContext) new ThrowingPlayerChoiceContext(), this.Creature, (Decimal) this.NutritionalPasteStrengthAmount, this.Creature, (CardModel) null);
  }

  private async Task SmashMove(IReadOnlyList<Creature> targets)
  {
    AttackCommand attackCommand = await DamageCmd.Attack((Decimal) this.SmashDamage).FromMonster((MonsterModel) this).WithAttackerAnim("Attack", 0.3f).WithAttackerFx(sfx: this.AttackSfx).WithHitFx("vfx/vfx_attack_slash").Execute((PlayerChoiceContext) null);
  }

  private async Task TenderizerMove(IReadOnlyList<Creature> targets)
  {
    AttackCommand attackCommand = await DamageCmd.Attack((Decimal) this.TenderizerDamage).FromMonster((MonsterModel) this).WithAttackerAnim("Attack", 0.3f).WithAttackerFx(sfx: this.AttackSfx).WithHitFx("vfx/vfx_attack_slash").Execute((PlayerChoiceContext) null);
    IReadOnlyList<VulnerablePower> vulnerablePowerList = await PowerCmd.Apply<VulnerablePower>((PlayerChoiceContext) new ThrowingPlayerChoiceContext(), (IEnumerable<Creature>) targets, 2M, this.Creature, (CardModel) null);
  }

  public override CreatureAnimator GenerateAnimator(MegaSprite controller)
  {
    AnimState initialState = new AnimState("idle_loop", true);
    AnimState state1 = new AnimState("cast");
    AnimState state2 = new AnimState("buff");
    AnimState state3 = new AnimState("attack");
    AnimState state4 = new AnimState("hurt");
    AnimState state5 = new AnimState("die");
    AnimState state6 = new AnimState("lay");
    state2.NextState = initialState;
    state1.NextState = initialState;
    state3.NextState = initialState;
    state6.NextState = initialState;
    state4.NextState = initialState;
    CreatureAnimator animator = new CreatureAnimator(initialState, controller);
    animator.AddAnyState("Dead", state5);
    animator.AddAnyState("Hit", state4);
    animator.AddAnyState("Cast", state1);
    animator.AddAnyState("Attack", state3);
    animator.AddAnyState("layTrigger", state6);
    animator.AddAnyState("buffTrigger", state2);
    return animator;
  }

  protected override bool ShouldShowMoveInBestiary(string moveStateId)
  {
    return moveStateId != "TENDERIZER_MOVE";
  }
}
