// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Monsters.Crusher
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.Ascension;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.MonsterMoves.Intents;
using MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine;
using MegaCrit.Sts2.Core.Nodes.Audio;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Screens.Bestiary;
using MegaCrit.Sts2.Core.Nodes.Vfx.Backgrounds;
using MegaCrit.Sts2.Core.ValueProps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Monsters;

public sealed class Crusher : MonsterModel
{
  public const string deathVfxPath = "vfx/monsters/kaiser_crab_boss_explosion";
  private const string _attackSlamSfx = "event:/sfx/enemy/enemy_attacks/kaiser_crab/kaiser_crab_left_attack_slam";
  private const string _attackScoopSfx = "event:/sfx/enemy/enemy_attacks/kaiser_crab/kaiser_crab_left_attack_scoop";
  private const string _attackScissorSfx = "event:/sfx/enemy/enemy_attacks/kaiser_crab/kaiser_crab_left_attack_scissor";
  private const string _buffSfx = "event:/sfx/enemy/enemy_attacks/kaiser_crab/kaiser_crab_left_buff";
  private const string _slamSfx = "event:/sfx/enemy/enemy_attacks/kaiser_crab/kaiser_crab_attack_slam";
  private NKaiserCrabBossBackground? _background;

  public override string DeathSfx
  {
    get => "event:/sfx/enemy/enemy_attacks/kaiser_crab/kaiser_crab_left_die";
  }

  public override IEnumerable<string> AssetPaths
  {
    get
    {
      int capacity = 1;
      List<string> first = new List<string>(capacity);
      CollectionsMarshal.SetCount<string>(first, capacity);
      CollectionsMarshal.AsSpan<string>(first)[0] = SceneHelper.GetScenePath("vfx/monsters/kaiser_crab_boss_explosion");
      return first.Concat<string>(base.AssetPaths);
    }
  }

  public override bool ShouldFadeAfterDeath => false;

  public override bool ShouldDisappearFromDoom => false;

  public override float DeathAnimLengthOverride => 2.5f;

  private NKaiserCrabBossBackground? Background
  {
    get
    {
      this.AssertMutable();
      if (this._background == null)
      {
        NCombatRoom instance = NCombatRoom.Instance;
        this._background = ((Node) ((instance != null ? (Control) instance.Background : (Control) null) ?? NBestiary.Instance?.Layout))?.GetNode<NKaiserCrabBossBackground>(NodePath.op_Implicit("%KaiserCrab"));
      }
      return this._background;
    }
  }

  public override int MinInitialHp
  {
    get => AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 219, 209);
  }

  public override int MaxInitialHp => this.MinInitialHp;

  private int ThrashDamage
  {
    get => AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 14, 12);
  }

  private int EnlargingStrikeDamage
  {
    get => AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 4, 4);
  }

  private int BugStingDamage
  {
    get => AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 7, 6);
  }

  private int BugStingTimes => 2;

  private int AdaptStrengthGain
  {
    get => AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 3, 2);
  }

  private int GuardedStrikeDamage
  {
    get => AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 14, 12);
  }

  protected override MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine.MonsterMoveStateMachine GenerateMoveStateMachine()
  {
    List<MonsterState> states = new List<MonsterState>();
    MoveState initialState = new MoveState("THRASH_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.ThrashMove), new AbstractIntent[1]
    {
      (AbstractIntent) new SingleAttackIntent(this.ThrashDamage)
    });
    MoveState moveState1 = new MoveState("ENLARGING_STRIKE_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.EnlargingStrikeMove), new AbstractIntent[1]
    {
      (AbstractIntent) new SingleAttackIntent(this.EnlargingStrikeDamage)
    });
    MoveState moveState2 = new MoveState("BUG_STING_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.BugStingMove), new AbstractIntent[2]
    {
      (AbstractIntent) new MultiAttackIntent(this.BugStingDamage, this.BugStingTimes),
      (AbstractIntent) new DebuffIntent()
    });
    MoveState moveState3 = new MoveState("ADAPT_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.AdaptMove), new AbstractIntent[1]
    {
      (AbstractIntent) new BuffIntent()
    });
    MoveState moveState4 = new MoveState("GUARDED_STRIKE_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.GuardedStrikeMove), new AbstractIntent[2]
    {
      (AbstractIntent) new SingleAttackIntent(this.GuardedStrikeDamage),
      (AbstractIntent) new DefendIntent()
    });
    initialState.FollowUpState = (MonsterState) moveState1;
    moveState1.FollowUpState = (MonsterState) moveState2;
    moveState2.FollowUpState = (MonsterState) moveState3;
    moveState3.FollowUpState = (MonsterState) moveState4;
    moveState4.FollowUpState = (MonsterState) initialState;
    states.Add((MonsterState) initialState);
    states.Add((MonsterState) moveState1);
    states.Add((MonsterState) moveState2);
    states.Add((MonsterState) moveState3);
    states.Add((MonsterState) moveState4);
    return new MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine.MonsterMoveStateMachine((IEnumerable<MonsterState>) states, (MonsterState) initialState);
  }

  public override async Task AfterAddedToRoom()
  {
    await base.AfterAddedToRoom();
    BackAttackLeftPower backAttackLeftPower = await PowerCmd.Apply<BackAttackLeftPower>((PlayerChoiceContext) new ThrowingPlayerChoiceContext(), this.Creature, 1M, this.Creature, (CardModel) null);
    CrabRagePower crabRagePower = await PowerCmd.Apply<CrabRagePower>((PlayerChoiceContext) new ThrowingPlayerChoiceContext(), this.Creature, 1M, this.Creature, (CardModel) null);
    ((CanvasItem) this.Background)?.SetVisible(true);
  }

  public override Task AfterCurrentHpChanged(Creature creature, Decimal delta)
  {
    if (creature != this.Creature || delta >= 0M)
      return Task.CompletedTask;
    this.Background?.PlayHurtAnim(NKaiserCrabBossBackground.ArmSide.Left);
    return Task.CompletedTask;
  }

  public override Task BeforeDeath(Creature creature)
  {
    if (creature != this.Creature)
      return Task.CompletedTask;
    NAudioManager.Instance.PlayOneShot(this.DeathSfx);
    this.Background?.PlayArmDeathAnim(NKaiserCrabBossBackground.ArmSide.Left);
    if (CombatManager.Instance.IsOverOrEnding)
    {
      this.Background?.PlayBodyDeathAnim();
      NRunMusicController.Instance?.UpdateMusicParameter("kaiser_crab_progress", 5f);
    }
    else
      NRunMusicController.Instance?.UpdateMusicParameter("kaiser_crab_progress", 2f);
    return Task.CompletedTask;
  }

  private async Task ThrashMove(IReadOnlyList<Creature> targets)
  {
    SfxCmd.Play("event:/sfx/enemy/enemy_attacks/kaiser_crab/kaiser_crab_left_attack_scoop");
    await (this.Background?.PlayAttackAnim(NKaiserCrabBossBackground.ArmSide.Left, "attack_heavy", 1f) ?? Task.CompletedTask);
    AttackCommand attackCommand = await DamageCmd.Attack((Decimal) this.ThrashDamage).FromMonster((MonsterModel) this).WithHitFx("vfx/vfx_attack_blunt", "event:/sfx/enemy/enemy_attacks/kaiser_crab/kaiser_crab_attack_slam").Execute((PlayerChoiceContext) null);
  }

  private async Task BugStingMove(IReadOnlyList<Creature> targets)
  {
    SfxCmd.Play("event:/sfx/enemy/enemy_attacks/kaiser_crab/kaiser_crab_left_attack_scissor");
    await (this.Background?.PlayAttackAnim(NKaiserCrabBossBackground.ArmSide.Left, "attack_double", 0.5f) ?? Task.CompletedTask);
    AttackCommand attackCommand = await DamageCmd.Attack((Decimal) this.BugStingDamage).WithHitCount(this.BugStingTimes).FromMonster((MonsterModel) this).WithHitFx("vfx/vfx_attack_slash").Execute((PlayerChoiceContext) null);
    IReadOnlyList<WeakPower> weakPowerList = await PowerCmd.Apply<WeakPower>((PlayerChoiceContext) new ThrowingPlayerChoiceContext(), (IEnumerable<Creature>) targets, 2M, this.Creature, (CardModel) null);
    IReadOnlyList<FrailPower> frailPowerList = await PowerCmd.Apply<FrailPower>((PlayerChoiceContext) new ThrowingPlayerChoiceContext(), (IEnumerable<Creature>) targets, 2M, this.Creature, (CardModel) null);
  }

  private async Task AdaptMove(IReadOnlyList<Creature> targets)
  {
    SfxCmd.Play("event:/sfx/enemy/enemy_attacks/kaiser_crab/kaiser_crab_left_buff");
    await (this.Background?.PlayAttackAnim(NKaiserCrabBossBackground.ArmSide.Left, "buff", 0.8f) ?? Task.CompletedTask);
    StrengthPower strengthPower = await PowerCmd.Apply<StrengthPower>((PlayerChoiceContext) new ThrowingPlayerChoiceContext(), this.Creature, (Decimal) this.AdaptStrengthGain, this.Creature, (CardModel) null);
  }

  private async Task EnlargingStrikeMove(IReadOnlyList<Creature> targets)
  {
    SfxCmd.Play("event:/sfx/enemy/enemy_attacks/kaiser_crab/kaiser_crab_left_attack_slam");
    await (this.Background?.PlayAttackAnim(NKaiserCrabBossBackground.ArmSide.Left, "attack_med", 0.65f) ?? Task.CompletedTask);
    AttackCommand attackCommand = await DamageCmd.Attack((Decimal) this.EnlargingStrikeDamage).FromMonster((MonsterModel) this).WithHitFx("vfx/vfx_heavy_blunt", tmpSfx: "heavy_attack.mp3").WithHitVfxSpawnedAtBase().Execute((PlayerChoiceContext) null);
  }

  private async Task GuardedStrikeMove(IReadOnlyList<Creature> targets)
  {
    SfxCmd.Play("event:/sfx/enemy/enemy_attacks/kaiser_crab/kaiser_crab_left_attack_slam");
    await (this.Background?.PlayAttackAnim(NKaiserCrabBossBackground.ArmSide.Left, "attack_med", 0.65f) ?? Task.CompletedTask);
    AttackCommand attackCommand = await DamageCmd.Attack((Decimal) this.GuardedStrikeDamage).FromMonster((MonsterModel) this).WithHitFx("vfx/vfx_heavy_blunt", tmpSfx: "heavy_attack.mp3").WithHitVfxSpawnedAtBase().Execute((PlayerChoiceContext) null);
    Decimal num = await CreatureCmd.GainBlock(this.Creature, 18M, ValueProp.Move, (CardPlay) null);
  }
}
