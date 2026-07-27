// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Monsters.PhantasmalGardener
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
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
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Monsters;

public sealed class PhantasmalGardener : MonsterModel
{
  private int _enlargeTriggers;
  private const string _attackMultiTrigger = "AttackMulti";
  public const string blockStartTrigger = "BlockStart";
  public const string blockEndTrigger = "BlockEnd";
  private const string _biteSfx = "event:/sfx/enemy/enemy_attacks/phantasmal_gardeners/phantasmal_gardeners_attack_bite";
  private const string _lickSfx = "event:/sfx/enemy/enemy_attacks/phantasmal_gardeners/phantasmal_gardeners_attack_lick";
  private const string _buffSfx = "event:/sfx/enemy/enemy_attacks/phantasmal_gardeners/phantasmal_gardeners_buff";

  public override int MinInitialHp
  {
    get => AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 27, 26);
  }

  public override int MaxInitialHp
  {
    get
    {
      return AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 32 /*0x20*/, 31 /*0x1F*/);
    }
  }

  private int BiteDamage => AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 5, 5);

  private int LashDamage => AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 7, 7);

  private int FlailDamage => 1;

  private int FlailRepeat
  {
    get => AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 3, 3);
  }

  private int EnlargeStr => AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 3, 2);

  private int SkittishAmount
  {
    get => AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 7, 6);
  }

  public int EnlargeTriggers
  {
    get => this._enlargeTriggers;
    set
    {
      this.AssertMutable();
      this._enlargeTriggers = value;
    }
  }

  public override bool ShouldFadeAfterDeath => false;

  public float CurrentScale { get; private set; } = 1f;

  public override DamageSfxType TakeDamageSfxType => DamageSfxType.Magic;

  public override string DeathSfx
  {
    get => "event:/sfx/enemy/enemy_attacks/phantasmal_gardeners/phantasmal_gardeners_die";
  }

  public override void SetupSkins(MegaSprite spine, MegaSkeleton skeleton)
  {
    MegaSkin skin = spine.NewSkin("custom-skin");
    MegaSkeletonDataResource data = skeleton.GetData();
    string slotName = this.Creature.SlotName;
    if (slotName == "first" || slotName == "third")
      skin.AddSkin(data.FindSkin("tall"));
    else
      skin.AddSkin(data.FindSkin("short"));
    skeleton.SetSkin(skin);
    skeleton.SetSlotsToSetupPose();
  }

  public override async Task AfterAddedToRoom()
  {
    await base.AfterAddedToRoom();
    SkittishPower skittishPower = await PowerCmd.Apply<SkittishPower>((PlayerChoiceContext) new ThrowingPlayerChoiceContext(), this.Creature, (Decimal) this.SkittishAmount, this.Creature, (CardModel) null);
  }

  protected override MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine.MonsterMoveStateMachine GenerateMoveStateMachine()
  {
    List<MonsterState> states = new List<MonsterState>();
    MoveState move1 = new MoveState("BITE_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.BiteMove), new AbstractIntent[1]
    {
      (AbstractIntent) new SingleAttackIntent(this.BiteDamage)
    });
    MoveState move2 = new MoveState("LASH_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.LashMove), new AbstractIntent[1]
    {
      (AbstractIntent) new SingleAttackIntent(this.LashDamage)
    });
    MoveState move3 = new MoveState("FLAIL_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.FlailMove), new AbstractIntent[1]
    {
      (AbstractIntent) new MultiAttackIntent(this.FlailDamage, this.FlailRepeat)
    });
    MoveState move4 = new MoveState("ENLARGE_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.EnlargeMove), new AbstractIntent[1]
    {
      (AbstractIntent) new BuffIntent()
    });
    ConditionalBranchState initialState = new ConditionalBranchState("INIT_MOVE");
    initialState.AddState((MonsterState) move3, (Func<bool>) (() => this.Creature.SlotName == "first"));
    initialState.AddState((MonsterState) move1, (Func<bool>) (() => this.Creature.SlotName == "second"));
    initialState.AddState((MonsterState) move2, (Func<bool>) (() => this.Creature.SlotName == "third"));
    initialState.AddState((MonsterState) move4, (Func<bool>) (() => this.Creature.SlotName == "fourth"));
    move1.FollowUpState = (MonsterState) move2;
    move2.FollowUpState = (MonsterState) move3;
    move3.FollowUpState = (MonsterState) move4;
    move4.FollowUpState = (MonsterState) move1;
    states.Add((MonsterState) move1);
    states.Add((MonsterState) move2);
    states.Add((MonsterState) move4);
    states.Add((MonsterState) move3);
    return new MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine.MonsterMoveStateMachine((IEnumerable<MonsterState>) states, (MonsterState) initialState);
  }

  private async Task BiteMove(IReadOnlyList<Creature> targets)
  {
    NCombatRoom.Instance?.GetCreatureNode(this.Creature)?.SetDefaultScaleTo(this.CurrentScale, 0.75f);
    AttackCommand attackCommand = await DamageCmd.Attack((Decimal) this.BiteDamage).FromMonster((MonsterModel) this).WithAttackerAnim("Attack", 0.3f).WithAttackerFx(sfx: "event:/sfx/enemy/enemy_attacks/phantasmal_gardeners/phantasmal_gardeners_attack_bite").WithHitFx("vfx/vfx_bite").Execute((PlayerChoiceContext) null);
  }

  private async Task LashMove(IReadOnlyList<Creature> targets)
  {
    NCombatRoom.Instance?.GetCreatureNode(this.Creature)?.SetDefaultScaleTo(this.CurrentScale, 0.75f);
    AttackCommand attackCommand = await DamageCmd.Attack((Decimal) this.LashDamage).FromMonster((MonsterModel) this).WithAttackerAnim("Attack", 0.3f).WithAttackerFx(sfx: "event:/sfx/enemy/enemy_attacks/phantasmal_gardeners/phantasmal_gardeners_attack_bite").WithHitFx("vfx/vfx_bite").Execute((PlayerChoiceContext) null);
  }

  private async Task FlailMove(IReadOnlyList<Creature> targets)
  {
    NCombatRoom.Instance?.GetCreatureNode(this.Creature)?.SetDefaultScaleTo(this.CurrentScale, 0.35f);
    AttackCommand attackCommand = await DamageCmd.Attack((Decimal) this.FlailDamage).WithHitCount(this.FlailRepeat).OnlyPlayAnimOnce().FromMonster((MonsterModel) this).WithAttackerAnim("AttackMulti", 0.3f).WithAttackerFx(sfx: "event:/sfx/enemy/enemy_attacks/phantasmal_gardeners/phantasmal_gardeners_attack_lick").WithHitFx("vfx/vfx_attack_blunt").Execute((PlayerChoiceContext) null);
  }

  private async Task EnlargeMove(IReadOnlyList<Creature> targets)
  {
    SfxCmd.Play("event:/sfx/enemy/enemy_attacks/phantasmal_gardeners/phantasmal_gardeners_buff");
    await CreatureCmd.TriggerAnim(this.Creature, "Cast", 1.5f);
    StrengthPower strengthPower = await PowerCmd.Apply<StrengthPower>((PlayerChoiceContext) new ThrowingPlayerChoiceContext(), this.Creature, (Decimal) this.EnlargeStr, this.Creature, (CardModel) null);
    this.EnlargeTriggers++;
    this.CurrentScale = (float) (1.0 + 0.10000000149011612 * (double) Mathf.Log((float) this.EnlargeTriggers + 1f));
    NCombatRoom.Instance?.GetCreatureNode(this.Creature)?.SetDefaultScaleTo(this.CurrentScale, 0.75f);
  }

  public override CreatureAnimator GenerateAnimator(MegaSprite controller)
  {
    AnimState animState1 = new AnimState("idle_loop", true);
    AnimState state1 = new AnimState("buff");
    AnimState state2 = new AnimState("attack");
    AnimState state3 = new AnimState("attack_multi");
    AnimState state4 = new AnimState("hurt_extended");
    AnimState state5 = new AnimState("hurt");
    AnimState state6 = new AnimState("die");
    AnimState animState2 = new AnimState("block_loop", true);
    AnimState state7 = new AnimState("block_start");
    AnimState state8 = new AnimState("block_end");
    state1.NextState = animState1;
    state2.NextState = animState1;
    state3.NextState = animState1;
    state4.NextState = animState1;
    state5.NextState = animState2;
    state7.NextState = animState2;
    state8.NextState = animState1;
    CreatureAnimator animator = new CreatureAnimator(animState1, controller);
    animator.AddAnyState("Idle", animState1);
    animator.AddAnyState("Cast", state1);
    animator.AddAnyState("Attack", state2);
    animator.AddAnyState("AttackMulti", state3);
    animator.AddAnyState("Dead", state6);
    animator.AddAnyState("Hit", state4, (Func<bool>) (() => !this.Creature.GetPower<SkittishPower>().HasGainedBlockThisTurn));
    animator.AddAnyState("Hit", state5, (Func<bool>) (() => this.Creature.GetPower<SkittishPower>().HasGainedBlockThisTurn));
    animator.AddAnyState("BlockStart", state7);
    animator.AddAnyState("BlockEnd", state8);
    return animator;
  }

  public override List<BestiaryMonsterMove> GenerateBestiaryMoveList(
    NCreatureVisuals? creatureVisuals)
  {
    List<BestiaryMonsterMove> bestiaryMoveList = base.GenerateBestiaryMoveList(creatureVisuals);
    bestiaryMoveList.RemoveAll((Predicate<BestiaryMonsterMove>) (m => m.stateId == "LASH_MOVE"));
    return bestiaryMoveList;
  }
}
