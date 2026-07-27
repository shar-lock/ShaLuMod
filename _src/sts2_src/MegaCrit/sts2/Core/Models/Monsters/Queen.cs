// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Monsters.Queen
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Audio;
using MegaCrit.Sts2.Core.Bindings.MegaSpine;
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
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.ValueProps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Monsters;

public sealed class Queen : MonsterModel
{
  private const string _puppetStringsMove = "PUPPET_STRINGS_MOVE";
  private const string _burnBrightForMeMove = "BURN_BRIGHT_FOR_ME_MOVE";
  private const string _executionMove = "EXECUTION_MOVE";
  private const string _enrageMove = "ENRAGE_MOVE";
  private const string _queenTrackName = "queen_progress";
  private const string _castSfx = "event:/sfx/enemy/enemy_attacks/queen/queen_cast";
  private const int _offWithYourHeadRepeat = 5;
  private bool _hasAmalgamDied;
  private Creature? _amalgam;
  private MoveState _burnBrightForMeState;
  private MoveState _enragedState;

  protected override string AttackSfx => "event:/sfx/enemy/enemy_attacks/queen/queen_arms_attack";

  public override IEnumerable<string> AssetPaths
  {
    get => base.AssetPaths.Concat<string>(ModelDb.Monster<TorchHeadAmalgam>().AssetPaths);
  }

  public override int MinInitialHp
  {
    get => AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 419, 400);
  }

  public override int MaxInitialHp => this.MinInitialHp;

  public override DamageSfxType TakeDamageSfxType => DamageSfxType.Armor;

  private int OffWithYourHeadDamage
  {
    get => AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 4, 3);
  }

  private int ExecutionDamage
  {
    get => AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 18, 15);
  }

  private bool HasAmalgamDied
  {
    get => this._hasAmalgamDied;
    set
    {
      this.AssertMutable();
      this._hasAmalgamDied = value;
    }
  }

  private Creature? Amalgam
  {
    get => this._amalgam;
    set
    {
      this.AssertMutable();
      this._amalgam = value;
    }
  }

  private MoveState BurnBrightForMeState
  {
    get => this._burnBrightForMeState;
    set
    {
      this.AssertMutable();
      this._burnBrightForMeState = value;
    }
  }

  private MoveState EnragedState
  {
    get => this._enragedState;
    set
    {
      this.AssertMutable();
      this._enragedState = value;
    }
  }

  public override void SetupSkins(MegaSprite spine, MegaSkeleton skeleton)
  {
    spine.GetAnimationState().SetAnimation("tracks/writhe", trackId: 1);
  }

  public override void BeforeRemovedFromRoom()
  {
    if (this.CombatState.RunState.IsGameOver)
      return;
    NCombatRoom.Instance?.GetCreatureNode(this.Creature)?.SpineAnimation.SetAnimation("tracks/empty", track: 1);
  }

  public override async Task AfterAddedToRoom()
  {
    await base.AfterAddedToRoom();
    this.Amalgam = this.CombatState.Enemies.First<Creature>((Func<Creature, bool>) (c => c.Monster is TorchHeadAmalgam));
    NRunMusicController.Instance?.UpdateMusicParameter("queen_progress", 1f);
  }

  protected override MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine.MonsterMoveStateMachine GenerateMoveStateMachine()
  {
    List<MonsterState> states = new List<MonsterState>();
    MoveState initialState = new MoveState("PUPPET_STRINGS_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.PuppetStringsMove), new AbstractIntent[1]
    {
      (AbstractIntent) new CardDebuffIntent()
    });
    MoveState moveState1 = new MoveState("YOU_ARE_MINE_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.YoureMineMove), new AbstractIntent[1]
    {
      (AbstractIntent) new DebuffIntent()
    });
    ConditionalBranchState conditionalBranchState1 = new ConditionalBranchState("YOURE_MINE_NOW_BRANCH");
    this.BurnBrightForMeState = new MoveState("BURN_BRIGHT_FOR_ME_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.BurnBrightForMeMove), new AbstractIntent[2]
    {
      (AbstractIntent) new BuffIntent(),
      (AbstractIntent) new DefendIntent()
    });
    ConditionalBranchState conditionalBranchState2 = new ConditionalBranchState("BURN_BRIGHT_FOR_ME_BRANCH");
    MoveState move = new MoveState("OFF_WITH_YOUR_HEAD_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.OffWithYourHeadMove), new AbstractIntent[1]
    {
      (AbstractIntent) new MultiAttackIntent(this.OffWithYourHeadDamage, 5)
    });
    MoveState moveState2 = new MoveState("EXECUTION_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.ExecutionMove), new AbstractIntent[1]
    {
      (AbstractIntent) new SingleAttackIntent(this.ExecutionDamage)
    });
    this.EnragedState = new MoveState("ENRAGE_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.EnrageMove), new AbstractIntent[1]
    {
      (AbstractIntent) new BuffIntent()
    });
    initialState.FollowUpState = (MonsterState) moveState1;
    moveState1.FollowUpState = (MonsterState) conditionalBranchState1;
    conditionalBranchState1.AddState((MonsterState) this.BurnBrightForMeState, (Func<bool>) (() => !this.HasAmalgamDied));
    conditionalBranchState1.AddState((MonsterState) move, (Func<bool>) (() => this.HasAmalgamDied));
    this.BurnBrightForMeState.FollowUpState = (MonsterState) conditionalBranchState2;
    conditionalBranchState2.AddState((MonsterState) this.BurnBrightForMeState, (Func<bool>) (() => !this.HasAmalgamDied));
    conditionalBranchState2.AddState((MonsterState) move, (Func<bool>) (() => this.HasAmalgamDied));
    move.FollowUpState = (MonsterState) moveState2;
    moveState2.FollowUpState = (MonsterState) this.EnragedState;
    this.EnragedState.FollowUpState = (MonsterState) move;
    states.Add((MonsterState) initialState);
    states.Add((MonsterState) moveState1);
    states.Add((MonsterState) this.BurnBrightForMeState);
    states.Add((MonsterState) conditionalBranchState2);
    states.Add((MonsterState) conditionalBranchState1);
    states.Add((MonsterState) move);
    states.Add((MonsterState) moveState2);
    states.Add((MonsterState) this.EnragedState);
    return new MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine.MonsterMoveStateMachine((IEnumerable<MonsterState>) states, (MonsterState) initialState);
  }

  private async Task PuppetStringsMove(IReadOnlyList<Creature> targets)
  {
    SfxCmd.Play("event:/sfx/enemy/enemy_attacks/queen/queen_cast");
    await CreatureCmd.TriggerAnim(this.Creature, "Cast", 0.5f);
    IReadOnlyList<ChainsOfBindingPower> chainsOfBindingPowerList = await PowerCmd.Apply<ChainsOfBindingPower>((PlayerChoiceContext) new ThrowingPlayerChoiceContext(), (IEnumerable<Creature>) targets, 3M, this.Creature, (CardModel) null);
  }

  private async Task YoureMineMove(IReadOnlyList<Creature> targets)
  {
    TalkCmd.Play(MonsterModel.L10NMonsterLookup("QUEEN.banter"), this.Creature, VfxColor.Purple);
    SfxCmd.Play("event:/sfx/enemy/enemy_attacks/queen/queen_cast");
    await CreatureCmd.TriggerAnim(this.Creature, "Cast", 0.5f);
    IReadOnlyList<FrailPower> frailPowerList = await PowerCmd.Apply<FrailPower>((PlayerChoiceContext) new ThrowingPlayerChoiceContext(), (IEnumerable<Creature>) targets, 99M, this.Creature, (CardModel) null);
    IReadOnlyList<WeakPower> weakPowerList = await PowerCmd.Apply<WeakPower>((PlayerChoiceContext) new ThrowingPlayerChoiceContext(), (IEnumerable<Creature>) targets, 99M, this.Creature, (CardModel) null);
    IReadOnlyList<VulnerablePower> vulnerablePowerList = await PowerCmd.Apply<VulnerablePower>((PlayerChoiceContext) new ThrowingPlayerChoiceContext(), (IEnumerable<Creature>) targets, 99M, this.Creature, (CardModel) null);
  }

  private async Task BurnBrightForMeMove(IReadOnlyList<Creature> targets)
  {
    SfxCmd.Play("event:/sfx/enemy/enemy_attacks/queen/queen_cast");
    await CreatureCmd.TriggerAnim(this.Creature, "Cast", 0.8f);
    int strengthAmount = AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 1, 1);
    foreach (Creature target in this.CombatState.GetTeammatesOf(this.Creature).ToList<Creature>().Where<Creature>((Func<Creature, bool>) (teammate => teammate != this.Creature)))
    {
      StrengthPower strengthPower = await PowerCmd.Apply<StrengthPower>((PlayerChoiceContext) new ThrowingPlayerChoiceContext(), target, (Decimal) strengthAmount, this.Creature, (CardModel) null);
    }
    Decimal num = await CreatureCmd.GainBlock(this.Creature, 20M, ValueProp.Move, (CardPlay) null);
  }

  private async Task OffWithYourHeadMove(IReadOnlyList<Creature> targets)
  {
    AttackCommand attackCommand = await DamageCmd.Attack((Decimal) this.OffWithYourHeadDamage).WithHitCount(5).FromMonster((MonsterModel) this).WithAttackerAnim("Attack", 0.6f).OnlyPlayAnimOnce().WithAttackerFx(sfx: this.AttackSfx).WithHitFx("vfx/vfx_attack_blunt").Execute((PlayerChoiceContext) null);
  }

  private async Task ExecutionMove(IReadOnlyList<Creature> targets)
  {
    AttackCommand attackCommand = await DamageCmd.Attack((Decimal) this.ExecutionDamage).FromMonster((MonsterModel) this).WithAttackerAnim("Attack", 0.6f).OnlyPlayAnimOnce().WithAttackerFx(sfx: this.AttackSfx).WithHitFx("vfx/vfx_attack_blunt").Execute((PlayerChoiceContext) null);
  }

  private async Task EnrageMove(IReadOnlyList<Creature> targets)
  {
    SfxCmd.Play("event:/sfx/enemy/enemy_attacks/queen/queen_cast");
    await CreatureCmd.TriggerAnim(this.Creature, "Cast", 0.5f);
    StrengthPower strengthPower = await PowerCmd.Apply<StrengthPower>((PlayerChoiceContext) new ThrowingPlayerChoiceContext(), this.Creature, 2M, this.Creature, (CardModel) null);
  }

  public override Task AfterDeath(
    PlayerChoiceContext choiceContext,
    Creature creature,
    bool wasRemovalPrevented,
    float deathAnimLength)
  {
    if (creature.Monster is TorchHeadAmalgam && this.Creature.IsAlive)
    {
      NRunMusicController.Instance?.UpdateMusicParameter("queen_progress", 2f);
      this.HasAmalgamDied = true;
      this.Amalgam = (Creature) null;
      TalkCmd.Play(MonsterModel.L10NMonsterLookup("QUEEN.amalgamDeathSpeakLine"), this.Creature, VfxColor.Purple);
      if (this.NextMove == this.BurnBrightForMeState)
        this.SetMoveImmediate(this.EnragedState);
    }
    if (creature == this.Creature)
    {
      NRunMusicController.Instance?.UpdateMusicParameter("queen_progress", 5f);
      this.Creature.GetCreatureNode()?.SpineAnimation.SetAnimation("tracks/empty", track: 1);
    }
    return Task.CompletedTask;
  }

  protected override bool ShouldShowMoveInBestiary(string moveStateId)
  {
    return moveStateId != "PUPPET_STRINGS_MOVE" && moveStateId != "BURN_BRIGHT_FOR_ME_MOVE" && moveStateId != "ENRAGE_MOVE" && moveStateId != "EXECUTION_MOVE";
  }
}
