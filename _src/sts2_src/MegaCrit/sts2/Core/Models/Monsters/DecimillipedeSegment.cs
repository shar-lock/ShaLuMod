// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Monsters.DecimillipedeSegment
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using MegaCrit.Sts2.Core.Animation;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Bindings.MegaSpine;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.Ascension;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.MonsterMoves;
using MegaCrit.Sts2.Core.MonsterMoves.Intents;
using MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine;
using MegaCrit.Sts2.Core.Nodes;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.TestSupport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Monsters;

public abstract class DecimillipedeSegment : MonsterModel
{
  private int _starterMoveIdx;
  private const int _writheRepeat = 2;
  private MoveState _deadState;
  public static readonly string rocksVfxPath = SceneHelper.GetScenePath("vfx/vfx_decimillipede_rocks");
  private const string _healSfx = "event:/sfx/enemy/enemy_attacks/decimillipede/decimillipede_heal";
  private const string _attackTriple = "event:/sfx/enemy/enemy_attacks/decimillipede/decimillipede_attack_triple";
  private const string _attackBuff = "event:/sfx/enemy/enemy_attacks/decimillipede/decimillipede_attack_buff";
  private const string _attackWeaken = "event:/sfx/enemy/enemy_attacks/decimillipede/decimillipede_attack_weaken";

  private string PhobiaModeAliveTexture
  {
    get
    {
      return ImageHelper.GetImagePath($"monsters/phobia_mode/{this.Id.Entry.ToLowerInvariant()}_phobia.png");
    }
  }

  private string PhobiaModeDeadTexture
  {
    get
    {
      return ImageHelper.GetImagePath($"monsters/phobia_mode/{this.Id.Entry.ToLowerInvariant()}_shriveled_phobia.png");
    }
  }

  public override LocString Title => MonsterModel.L10NMonsterLookup("DECIMILLIPEDE_SEGMENT.name");

  public int StarterMoveIdx
  {
    get => this._starterMoveIdx;
    set
    {
      this.AssertMutable();
      this._starterMoveIdx = value;
    }
  }

  public override int MinInitialHp
  {
    get => AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 46, 40);
  }

  public override int MaxInitialHp
  {
    get => AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 52, 46);
  }

  private int WritheDamage
  {
    get => AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 6, 5);
  }

  private int ConstrictDamage
  {
    get => AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 9, 8);
  }

  private int BulkDamage => AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 7, 6);

  private int BulkStrength => 2;

  public override float HpBarSizeReduction => 35f;

  public MoveState DeadState
  {
    get => this._deadState;
    private set
    {
      this.AssertMutable();
      this._deadState = value;
    }
  }

  public override bool ShouldFadeAfterDeath => false;

  public override bool ShouldDisappearFromDoom => false;

  public override bool ShouldShowInCompendium => false;

  public override bool CanChangeScale => false;

  public override string DeathSfx
  {
    get => "event:/sfx/enemy/enemy_attacks/decimillipede/decimillipede_die";
  }

  public override IEnumerable<string> AssetPaths
  {
    get
    {
      int capacity = 3;
      List<string> first = new List<string>(capacity);
      CollectionsMarshal.SetCount<string>(first, capacity);
      Span<string> span = CollectionsMarshal.AsSpan<string>(first);
      int num1 = 0;
      span[num1] = DecimillipedeSegment.rocksVfxPath;
      int num2 = num1 + 1;
      span[num2] = this.PhobiaModeAliveTexture;
      int num3 = num2 + 1;
      span[num3] = this.PhobiaModeDeadTexture;
      return first.Concat<string>(base.AssetPaths);
    }
  }

  public override async Task AfterAddedToRoom()
  {
    await base.AfterAddedToRoom();
    Decimal maxHp = (Decimal) this.Creature.MaxHp;
    if (maxHp % 2M == 1M)
      ++maxHp;
    int count = this.CombatState.Players.Count;
    int currentActIndex = this.CombatState.RunState.CurrentActIndex;
    List<Creature> list = this.CombatState.GetTeammatesOf(this.Creature).Where<Creature>((Func<Creature, bool>) (c => c != this.Creature)).ToList<Creature>();
    while (list.Any<Creature>((Func<Creature, bool>) (c => (Decimal) c.MaxHp == maxHp)))
    {
      maxHp += 2M;
      if (maxHp > Creature.ScaleHpForMultiplayer((Decimal) this.MaxInitialHp, this.CombatState.Encounter, count, currentActIndex))
        maxHp = Creature.ScaleHpForMultiplayer((Decimal) this.MinInitialHp, this.CombatState.Encounter, count, currentActIndex);
    }
    await CreatureCmd.SetMaxAndCurrentHp(this.Creature, maxHp);
    ReattachPower reattachPower = await PowerCmd.Apply<ReattachPower>((PlayerChoiceContext) new ThrowingPlayerChoiceContext(), this.Creature, 25M, this.Creature, (CardModel) null);
  }

  protected override MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine.MonsterMoveStateMachine GenerateMoveStateMachine()
  {
    List<MonsterState> states = new List<MonsterState>();
    MoveState state1 = new MoveState("WRITHE_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.WritheMove), new AbstractIntent[1]
    {
      (AbstractIntent) new MultiAttackIntent(this.WritheDamage, 2)
    });
    MoveState state2 = new MoveState("BULK_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.BulkMove), new AbstractIntent[2]
    {
      (AbstractIntent) new SingleAttackIntent(this.BulkDamage),
      (AbstractIntent) new BuffIntent()
    });
    MoveState state3 = new MoveState("CONSTRICT_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.ConstrictMove), new AbstractIntent[2]
    {
      (AbstractIntent) new SingleAttackIntent(this.ConstrictDamage),
      (AbstractIntent) new DebuffIntent()
    });
    this.DeadState = new MoveState("DEAD_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.DeadMove), Array.Empty<AbstractIntent>());
    MoveState moveState1 = new MoveState("REATTACH_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.ReattachMove), new AbstractIntent[1]
    {
      (AbstractIntent) new HealIntent()
    })
    {
      MustPerformOnceBeforeTransitioning = true
    };
    state3.FollowUpState = (MonsterState) state2;
    state2.FollowUpState = (MonsterState) state1;
    state1.FollowUpState = (MonsterState) state3;
    RandomBranchState randomBranchState = new RandomBranchState("RAND");
    this.DeadState.FollowUpState = (MonsterState) moveState1;
    moveState1.FollowUpState = (MonsterState) randomBranchState;
    randomBranchState.AddBranch((MonsterState) state1, MoveRepeatType.CannotRepeat);
    randomBranchState.AddBranch((MonsterState) state2, MoveRepeatType.CannotRepeat);
    randomBranchState.AddBranch((MonsterState) state3, MoveRepeatType.CannotRepeat);
    states.Add((MonsterState) state1);
    states.Add((MonsterState) state2);
    states.Add((MonsterState) state3);
    states.Add((MonsterState) this.DeadState);
    states.Add((MonsterState) moveState1);
    states.Add((MonsterState) randomBranchState);
    MoveState moveState2;
    switch (this.StarterMoveIdx % 3)
    {
      case 0:
        moveState2 = state1;
        break;
      case 1:
        moveState2 = state2;
        break;
      default:
        moveState2 = state3;
        break;
    }
    MoveState initialState = moveState2;
    return new MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine.MonsterMoveStateMachine((IEnumerable<MonsterState>) states, (MonsterState) initialState);
  }

  private async Task WritheMove(IReadOnlyList<Creature> targets)
  {
    SfxCmd.Play("event:/sfx/enemy/enemy_attacks/decimillipede/decimillipede_attack_triple");
    await this.AnimSegmentsAttack();
    AttackCommand attackCommand = await DamageCmd.Attack((Decimal) this.WritheDamage).WithHitCount(2).FromMonster((MonsterModel) this).WithHitFx("vfx/vfx_attack_blunt").Execute((PlayerChoiceContext) null);
  }

  private async Task BulkMove(IReadOnlyList<Creature> targets)
  {
    SfxCmd.Play("event:/sfx/enemy/enemy_attacks/decimillipede/decimillipede_attack_buff");
    await this.AnimSegmentsAttack();
    AttackCommand attackCommand = await DamageCmd.Attack((Decimal) this.BulkDamage).FromMonster((MonsterModel) this).WithHitFx("vfx/vfx_attack_blunt").Execute((PlayerChoiceContext) null);
    StrengthPower strengthPower = await PowerCmd.Apply<StrengthPower>((PlayerChoiceContext) new ThrowingPlayerChoiceContext(), this.Creature, (Decimal) this.BulkStrength, this.Creature, (CardModel) null);
  }

  private async Task ConstrictMove(IReadOnlyList<Creature> targets)
  {
    SfxCmd.Play("event:/sfx/enemy/enemy_attacks/decimillipede/decimillipede_attack_weaken");
    await this.AnimSegmentsAttack();
    AttackCommand attackCommand = await DamageCmd.Attack((Decimal) this.ConstrictDamage).FromMonster((MonsterModel) this).WithHitFx("vfx/vfx_attack_blunt").Execute((PlayerChoiceContext) null);
    IReadOnlyList<WeakPower> weakPowerList = await PowerCmd.Apply<WeakPower>((PlayerChoiceContext) new ThrowingPlayerChoiceContext(), (IEnumerable<Creature>) targets, 1M, this.Creature, (CardModel) null);
  }

  private Task DeadMove(IReadOnlyList<Creature> targets) => Task.CompletedTask;

  private async Task ReattachMove(IReadOnlyList<Creature> targets)
  {
    SfxCmd.Play("event:/sfx/enemy/enemy_attacks/decimillipede/decimillipede_heal");
    if (this.CombatState.IsLiveCombat())
      await this.Creature.GetPower<ReattachPower>().DoReattach();
    this.ChangePhobiaModeTexture(ResourceLoader.Load<Texture2D>(this.PhobiaModeAliveTexture, (string) null, (ResourceLoader.CacheMode) 1L));
  }

  private async Task AnimSegmentsAttack()
  {
    if (TestMode.IsOn)
      return;
    foreach (Creature creature in this.CombatState.GetTeammatesOf(this.Creature).Where<Creature>((Func<Creature, bool>) (c => c.Monster is DecimillipedeSegment)))
      ((DecimillipedeSegment) creature.Monster).SegmentAttack();
    Node2D child = PreloadManager.Cache.GetScene(DecimillipedeSegment.rocksVfxPath).Instantiate<Node2D>((PackedScene.GenEditState) 0L);
    Control vfxContainer = this.Creature.GetVfxContainer();
    if (vfxContainer != null)
      ((Node) vfxContainer).AddChildSafely((Node) child);
    Node2D node2D = child;
    Rect2 viewportRect = ((CanvasItem) NGame.Instance).GetViewportRect();
    Vector2 vector2 = Vector2.op_Multiply(((Rect2) ref viewportRect).Size, 0.5f);
    node2D.GlobalPosition = vector2;
    await Cmd.Wait(0.5f);
  }

  public override Task AfterDeath(
    PlayerChoiceContext choiceContext,
    Creature creature,
    bool wasRemovalPrevented,
    float deathAnimLength)
  {
    if (creature != this.Creature || TestMode.IsOn)
      return Task.CompletedTask;
    this.ChangePhobiaModeTexture(ResourceLoader.Load<Texture2D>(this.PhobiaModeDeadTexture, (string) null, (ResourceLoader.CacheMode) 1L));
    return Task.CompletedTask;
  }

  private void ChangePhobiaModeTexture(Texture2D tex)
  {
    NCreature creatureNode = this.Creature.GetCreatureNode();
    if (creatureNode == null || !creatureNode.Visuals.IsUsingPhobiaModeBody)
      return;
    ((Sprite2D) creatureNode.Body).Texture = tex;
  }

  public override CreatureAnimator GenerateAnimator(MegaSprite controller)
  {
    AnimState animState1 = new AnimState("idle_loop", true);
    AnimState animState2 = new AnimState("hurt");
    AnimState animState3 = new AnimState("dead_loop", true);
    AnimState state1 = new AnimState("wither");
    AnimState state2 = new AnimState("regenerate");
    animState2.NextState = animState1;
    state1.NextState = animState3;
    state2.NextState = animState1;
    CreatureAnimator animator = new CreatureAnimator(animState1, controller);
    animator.AddAnyState("Revive", state2);
    animator.AddAnyState("Hit", animState1);
    animator.AddAnyState("Dead", state1);
    return animator;
  }

  public abstract void SegmentAttack();
}
