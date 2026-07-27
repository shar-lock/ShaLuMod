// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Monsters.TestSubject
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
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.MonsterMoves.Intents;
using MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine;
using MegaCrit.Sts2.Core.Nodes.Audio;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.Saves;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Monsters;

public sealed class TestSubject : MonsterModel
{
  private const string _biteMove = "BITE_MOVE";
  private const string _skullBashMove = "SKULL_BASH_MOVE";
  private const string _lacerateMove = "PHASE3_LACERATE_MOVE";
  private const string _testSubjectCustomTrackName = "test_subject_progress";
  private const int _baseTestSubjectNum = 8;
  private const int _phase3LacerateRepeat = 3;
  private const string _growthSpurtTrigger = "GrowthSpurtTrigger";
  private const string _bigAttackTrigger = "BiteTrigger";
  private const string _multiAttackTrigger = "MultiAttackTrigger";
  private const string _deadTrigger = "DeadTrigger";
  private const string _respawnTrigger = "RespawnTrigger";
  private const string _burnTrigger = "BurnTrigger";
  private const string _biteSfx = "event:/sfx/enemy/enemy_attacks/test_subject/test_subject_bite";
  private const string _slashSfx = "event:/sfx/enemy/enemy_attacks/test_subject/test_subject_slash";
  private const string _knockOutSfx = "event:/sfx/enemy/enemy_attacks/test_subject/test_subject_knock_out";
  private const string _reviveTwoHeadsSfx = "event:/sfx/enemy/enemy_attacks/test_subject/test_subject_revive_two_heads";
  private const string _reviveThreeHeadsSfx = "event:/sfx/enemy/enemy_attacks/test_subject/test_subject_revive_three_heads";
  private MoveState _deadState;
  private int _respawns;
  private int _extraMultiClawCount;

  public override LocString Title
  {
    get
    {
      LocString title = base.Title;
      title.Add("Count", (Decimal) (SaveManager.Instance.Progress.TestSubjectKills + 8));
      return title;
    }
  }

  public override int MinInitialHp => this.FirstFormHp;

  public override int MaxInitialHp => this.MinInitialHp;

  public int FirstFormHp
  {
    get => AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 111, 100);
  }

  public int SecondFormHp
  {
    get => AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 212, 200);
  }

  public int ThirdFormHp
  {
    get => AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 313, 300);
  }

  private int EnrageAmount
  {
    get => AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 3, 2);
  }

  private int BiteDamage
  {
    get => AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 22, 20);
  }

  private int SkullBashDamage
  {
    get => AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 16 /*0x10*/, 14);
  }

  private int MultiClawDamage
  {
    get => AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 11, 10);
  }

  private int BaseMultiClawCount => 3;

  private int Phase3LacerateDamage
  {
    get => AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 11, 10);
  }

  private int BigPounceDamage => 45;

  private int BurningGrowlBurnCount
  {
    get => AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 5, 3);
  }

  private int BurningGrowlStrengthGain
  {
    get => AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 3, 2);
  }

  public override string DeathSfx
  {
    get
    {
      if (!this.IsMutable)
        return base.DeathSfx;
      return !this.Creature.HasPower<AdaptablePower>() ? base.DeathSfx : "event:/sfx/enemy/enemy_attacks/test_subject/test_subject_knock_out";
    }
  }

  public override DamageSfxType TakeDamageSfxType => DamageSfxType.Armor;

  private MoveState DeadState
  {
    get => this._deadState;
    set
    {
      this.AssertMutable();
      this._deadState = value;
    }
  }

  private int Respawns
  {
    get => this._respawns;
    set
    {
      this.AssertMutable();
      this._respawns = value;
    }
  }

  private int ExtraMultiClawCount
  {
    get => this._extraMultiClawCount;
    set
    {
      this.AssertMutable();
      this._extraMultiClawCount = value;
    }
  }

  private int MultiClawTotalCount => this.BaseMultiClawCount + this.ExtraMultiClawCount;

  public override bool ShouldDisappearFromDoom => this.Respawns >= 2;

  public async Task TriggerDeadState()
  {
    NRunMusicController.Instance?.UpdateMusicParameter("test_subject_progress", 1f);
    ++this.CombatState.RunState.ExtraFields.TestSubjectKills;
    await CreatureCmd.TriggerAnim(this.Creature, "DeadTrigger", 0.0f);
    this.SetMoveImmediate(this.DeadState, true);
  }

  public override async Task AfterAddedToRoom()
  {
    await base.AfterAddedToRoom();
    AdaptablePower adaptablePower = await PowerCmd.Apply<AdaptablePower>((PlayerChoiceContext) new ThrowingPlayerChoiceContext(), this.Creature, 1M, this.Creature, (CardModel) null);
    EnragePower enragePower = await PowerCmd.Apply<EnragePower>((PlayerChoiceContext) new ThrowingPlayerChoiceContext(), this.Creature, (Decimal) this.EnrageAmount, this.Creature, (CardModel) null);
    this.Creature.PowerApplied += new Action<PowerModel>(this.AfterPowerApplied);
    this.Creature.PowerRemoved += new Action<PowerModel>(this.AfterPowerRemoved);
  }

  public override void BeforeRemovedFromRoom()
  {
    this.Creature.PowerApplied -= new Action<PowerModel>(this.AfterPowerApplied);
    this.Creature.PowerRemoved -= new Action<PowerModel>(this.AfterPowerRemoved);
  }

  protected override MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine.MonsterMoveStateMachine GenerateMoveStateMachine()
  {
    List<MonsterState> states = new List<MonsterState>();
    this.DeadState = new MoveState("RESPAWN_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.RespawnMove), new AbstractIntent[2]
    {
      (AbstractIntent) new HealIntent(),
      (AbstractIntent) new BuffIntent()
    })
    {
      MustPerformOnceBeforeTransitioning = true
    };
    MoveState initialState = new MoveState("BITE_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.BiteMove), new AbstractIntent[1]
    {
      (AbstractIntent) new SingleAttackIntent(this.BiteDamage)
    });
    MoveState moveState1 = new MoveState("SKULL_BASH_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.SkullBashMove), new AbstractIntent[2]
    {
      (AbstractIntent) new SingleAttackIntent(this.SkullBashDamage),
      (AbstractIntent) new DebuffIntent()
    });
    MoveState move1 = new MoveState("MULTI_CLAW_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.MultiClawMove), new AbstractIntent[1]
    {
      (AbstractIntent) new MultiAttackIntent(this.MultiClawDamage, (Func<int>) (() => this.MultiClawTotalCount))
    });
    MoveState move2 = new MoveState("PHASE3_LACERATE_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.Phase3LacerateMove), new AbstractIntent[1]
    {
      (AbstractIntent) new MultiAttackIntent(this.Phase3LacerateDamage, 3)
    });
    MoveState moveState2 = new MoveState("BIG_POUNCE", new Func<IReadOnlyList<Creature>, Task>(this.BigPounceMove), new AbstractIntent[1]
    {
      (AbstractIntent) new SingleAttackIntent(this.BigPounceDamage)
    });
    MoveState moveState3 = new MoveState("BURNING_GROWL_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.BurningGrowlMove), new AbstractIntent[2]
    {
      (AbstractIntent) new StatusIntent(this.BurningGrowlBurnCount),
      (AbstractIntent) new BuffIntent()
    });
    ConditionalBranchState conditionalBranchState = new ConditionalBranchState("REVIVE_BRANCH");
    initialState.FollowUpState = (MonsterState) moveState1;
    moveState1.FollowUpState = (MonsterState) initialState;
    move1.FollowUpState = (MonsterState) move1;
    move2.FollowUpState = (MonsterState) moveState2;
    moveState2.FollowUpState = (MonsterState) moveState3;
    moveState3.FollowUpState = (MonsterState) move2;
    this.DeadState.FollowUpState = (MonsterState) conditionalBranchState;
    conditionalBranchState.AddState((MonsterState) move1, (Func<bool>) (() => this.Respawns < 2));
    conditionalBranchState.AddState((MonsterState) move2, (Func<bool>) (() => this.Respawns >= 2));
    states.Add((MonsterState) this.DeadState);
    states.Add((MonsterState) initialState);
    states.Add((MonsterState) moveState1);
    states.Add((MonsterState) move1);
    states.Add((MonsterState) move2);
    states.Add((MonsterState) moveState2);
    states.Add((MonsterState) moveState3);
    states.Add((MonsterState) conditionalBranchState);
    return new MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine.MonsterMoveStateMachine((IEnumerable<MonsterState>) states, (MonsterState) initialState);
  }

  public override Task AfterDeath(
    PlayerChoiceContext choiceContext,
    Creature creature,
    bool wasRemovalPrevented,
    float deathAnimLength)
  {
    if (creature != this.Creature || this.Creature.HasPower<AdaptablePower>())
      return Task.CompletedTask;
    this.SetColor(Colors.White);
    NRunMusicController.Instance?.UpdateMusicParameter("test_subject_progress", 5f);
    return Task.CompletedTask;
  }

  private void AfterPowerApplied(PowerModel power)
  {
    if (!(power is IntangiblePower))
      return;
    this.SetColor(StsColors.halfTransparentWhite);
  }

  private void AfterPowerRemoved(PowerModel power)
  {
    if (!(power is IntangiblePower))
      return;
    this.SetColor(Colors.White);
  }

  private async Task RespawnMove(IReadOnlyList<Creature> targets)
  {
    this.Respawns++;
    NRunMusicController.Instance?.UpdateMusicParameter("test_subject_progress", 2f);
    SfxCmd.Play(this.Respawns == 1 ? "event:/sfx/enemy/enemy_attacks/test_subject/test_subject_revive_two_heads" : "event:/sfx/enemy/enemy_attacks/test_subject/test_subject_revive_three_heads");
    await CreatureCmd.TriggerAnim(this.Creature, "RespawnTrigger", 0.0f);
    await Cmd.Wait(0.8f);
    this.Creature.GetCreatureNode()?.SetDefaultScaleTo((float) (1.0 + (double) this.Respawns * 0.10000000149011612), 0.1f);
    await Cmd.Wait(1.15f);
    this.Creature.GetPower<AdaptablePower>()?.DoRevive();
    switch (this.Respawns)
    {
      case 1:
        await this.Revive(this.SecondFormHp);
        PainfulStabsPower painfulStabsPower = await PowerCmd.Apply<PainfulStabsPower>((PlayerChoiceContext) new ThrowingPlayerChoiceContext(), this.Creature, 1M, this.Creature, (CardModel) null);
        break;
      case 2:
        await this.Revive(this.ThirdFormHp);
        NemesisPower nemesisPower = await PowerCmd.Apply<NemesisPower>((PlayerChoiceContext) new ThrowingPlayerChoiceContext(), this.Creature, 1M, this.Creature, (CardModel) null);
        await PowerCmd.Remove<AdaptablePower>(this.Creature);
        await PowerCmd.Remove<PainfulStabsPower>(this.Creature);
        break;
    }
  }

  private async Task BiteMove(IReadOnlyList<Creature> targets)
  {
    AttackCommand attackCommand = await DamageCmd.Attack((Decimal) this.BiteDamage).FromMonster((MonsterModel) this).WithAttackerAnim("BiteTrigger", 0.25f).WithAttackerFx(sfx: "event:/sfx/enemy/enemy_attacks/test_subject/test_subject_bite").WithHitFx("vfx/vfx_attack_blunt").Execute((PlayerChoiceContext) null);
  }

  private async Task SkullBashMove(IReadOnlyList<Creature> targets)
  {
    AttackCommand attackCommand = await DamageCmd.Attack((Decimal) this.SkullBashDamage).FromMonster((MonsterModel) this).WithAttackerAnim("BiteTrigger", 0.25f).WithAttackerFx(sfx: "event:/sfx/enemy/enemy_attacks/test_subject/test_subject_bite").WithHitFx("vfx/vfx_attack_blunt").Execute((PlayerChoiceContext) null);
    IReadOnlyList<VulnerablePower> vulnerablePowerList = await PowerCmd.Apply<VulnerablePower>((PlayerChoiceContext) new ThrowingPlayerChoiceContext(), (IEnumerable<Creature>) targets, 1M, this.Creature, (CardModel) null);
  }

  private async Task MultiClawMove(IReadOnlyList<Creature> targets)
  {
    AttackCommand attackCommand = await DamageCmd.Attack((Decimal) this.MultiClawDamage).WithHitCount(this.MultiClawTotalCount).FromMonster((MonsterModel) this).OnlyPlayAnimOnce().WithAttackerAnim("MultiAttackTrigger", 0.2f).WithAttackerFx(sfx: "event:/sfx/enemy/enemy_attacks/test_subject/test_subject_slash").Execute((PlayerChoiceContext) null);
    this.ExtraMultiClawCount++;
  }

  private async Task Phase3LacerateMove(IReadOnlyList<Creature> targets)
  {
    AttackCommand attackCommand = await DamageCmd.Attack((Decimal) this.Phase3LacerateDamage).WithHitCount(3).FromMonster((MonsterModel) this).OnlyPlayAnimOnce().WithAttackerAnim("MultiAttackTrigger", 0.2f).WithAttackerFx(sfx: "event:/sfx/enemy/enemy_attacks/test_subject/test_subject_slash").Execute((PlayerChoiceContext) null);
  }

  private async Task BigPounceMove(IReadOnlyList<Creature> targets)
  {
    AttackCommand attackCommand = await DamageCmd.Attack((Decimal) this.BigPounceDamage).FromMonster((MonsterModel) this).WithAttackerAnim("BiteTrigger", 0.25f).WithAttackerFx(sfx: "event:/sfx/enemy/enemy_attacks/test_subject/test_subject_bite").WithHitFx("vfx/vfx_attack_blunt").Execute((PlayerChoiceContext) null);
  }

  private async Task BurningGrowlMove(IReadOnlyList<Creature> targets)
  {
    this.SetColor(Colors.White);
    Control backVfxContainer = this.Creature.GetBackVfxContainer();
    if (backVfxContainer != null)
      ((Node) backVfxContainer).AddChildSafely((Node) NTestSubjectBurnVfx.Create());
    await CreatureCmd.TriggerAnim(this.Creature, "BurnTrigger", 1.25f);
    await CardPileCmd.AddToCombatAndPreview<Burn>((IEnumerable<Creature>) targets, PileType.Discard, this.BurningGrowlBurnCount, (Player) null);
    StrengthPower strengthPower = await PowerCmd.Apply<StrengthPower>((PlayerChoiceContext) new ThrowingPlayerChoiceContext(), this.Creature, (Decimal) this.BurningGrowlStrengthGain, this.Creature, (CardModel) null);
  }

  private async Task Revive(int baseRespawnHp)
  {
    this.AssertMutable();
    Decimal scaledHp = Creature.ScaleHpForMultiplayer((Decimal) baseRespawnHp, this.CombatState.Encounter, this.CombatState.Players.Count, this.CombatState.RunState.CurrentActIndex);
    Decimal num = await CreatureCmd.SetMaxHp(this.Creature, scaledHp);
    await CreatureCmd.Heal(this.Creature, scaledHp);
  }

  private void SetColor(Color color)
  {
    ((CanvasItem) NCombatRoom.Instance?.GetCreatureNode(this.Creature)?.GetSpecialNode<CanvasGroup>("%CanvasGroup"))?.SetSelfModulate(color);
  }

  public override CreatureAnimator GenerateAnimator(MegaSprite controller)
  {
    AnimState initialState = new AnimState("idle_loop1", true);
    AnimState state1 = new AnimState("hurt1");
    AnimState state2 = new AnimState("attack_double1");
    AnimState state3 = new AnimState("attack_big1");
    AnimState state4 = new AnimState("heal1");
    AnimState state5 = new AnimState("knockout1");
    AnimState animState1 = new AnimState("knocked_out_loop1", true);
    AnimState state6 = new AnimState("regenerate1")
    {
      BoundsContainer = "RespawnBounds1"
    };
    AnimState animState2 = new AnimState("idle_loop2", true);
    AnimState state7 = new AnimState("hurt2");
    AnimState state8 = new AnimState("attack_double2");
    AnimState state9 = new AnimState("attack_big2");
    AnimState state10 = new AnimState("heal2");
    AnimState state11 = new AnimState("knockout2");
    AnimState animState3 = new AnimState("knocked_out_loop2", true);
    AnimState state12 = new AnimState("regenerate2")
    {
      BoundsContainer = "RespawnBounds2"
    };
    AnimState animState4 = new AnimState("idle_loop3", true);
    AnimState state13 = new AnimState("hurt3");
    AnimState state14 = new AnimState("attack_double3");
    AnimState state15 = new AnimState("attack_big3");
    AnimState state16 = new AnimState("burn");
    AnimState state17 = new AnimState("heal3");
    AnimState state18 = new AnimState("die");
    state4.NextState = initialState;
    state3.NextState = initialState;
    state2.NextState = initialState;
    state1.NextState = initialState;
    state5.NextState = animState1;
    state6.NextState = animState2;
    state10.NextState = animState2;
    state9.NextState = animState2;
    state8.NextState = animState2;
    state7.NextState = animState2;
    state11.NextState = animState3;
    state12.NextState = animState4;
    state17.NextState = animState4;
    state15.NextState = animState4;
    state14.NextState = animState4;
    state16.NextState = animState4;
    state13.NextState = animState4;
    CreatureAnimator animator = new CreatureAnimator(initialState, controller);
    animator.AddAnyState("Hit", state1, (Func<bool>) (() => this.Respawns == 0));
    animator.AddAnyState("BiteTrigger", state3, (Func<bool>) (() => this.Respawns == 0));
    animator.AddAnyState("MultiAttackTrigger", state2, (Func<bool>) (() => this.Respawns == 0));
    animator.AddAnyState("GrowthSpurtTrigger", state4, (Func<bool>) (() => this.Respawns == 0));
    animator.AddAnyState("DeadTrigger", state5, (Func<bool>) (() => this.Respawns == 0));
    animator.AddAnyState("RespawnTrigger", state6, (Func<bool>) (() => this.Respawns == 1));
    animator.AddAnyState("Hit", state7, (Func<bool>) (() => this.Respawns == 1));
    animator.AddAnyState("BiteTrigger", state9, (Func<bool>) (() => this.Respawns == 1));
    animator.AddAnyState("MultiAttackTrigger", state8, (Func<bool>) (() => this.Respawns == 1));
    animator.AddAnyState("GrowthSpurtTrigger", state10, (Func<bool>) (() => this.Respawns == 1));
    animator.AddAnyState("DeadTrigger", state11, (Func<bool>) (() => this.Respawns == 1));
    animator.AddAnyState("RespawnTrigger", state12, (Func<bool>) (() => this.Respawns >= 2));
    animator.AddAnyState("Hit", state13, (Func<bool>) (() => this.Respawns >= 2));
    animator.AddAnyState("BiteTrigger", state15, (Func<bool>) (() => this.Respawns >= 2));
    animator.AddAnyState("MultiAttackTrigger", state14, (Func<bool>) (() => this.Respawns >= 2));
    animator.AddAnyState("GrowthSpurtTrigger", state17, (Func<bool>) (() => this.Respawns >= 2));
    animator.AddAnyState("Dead", state18);
    animator.AddAnyState("BurnTrigger", state16);
    return animator;
  }

  protected override bool ShouldShowMoveInBestiary(string moveStateId)
  {
    return moveStateId != "BITE_MOVE" && moveStateId != "SKULL_BASH_MOVE" && moveStateId != "PHASE3_LACERATE_MOVE";
  }
}
