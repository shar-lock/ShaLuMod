// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.MonsterModel
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using MegaCrit.Sts2.Core.Animation;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Audio;
using MegaCrit.Sts2.Core.Bindings.MegaSpine;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Debug;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Hooks;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.MonsterMoves.Intents;
using MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Random;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.TestSupport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models;

public abstract class MonsterModel : AbstractModel
{
  private static readonly string _fallbackVisualsPath = SceneHelper.GetScenePath("creature_visuals/fallback");
  public static readonly Vector2 defaultDeathVfxPadding = Vector2.op_Multiply(1.2f, Vector2.One);
  public const string stunnedMoveId = "STUNNED";
  protected const string _locTableName = "monsters";
  private Rng? _rng;
  private RunRngSet? _runRng;
  private bool _isPerformingMove;
  private Creature? _creature;
  private MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine.MonsterMoveStateMachine? _moveStateMachine;
  private bool _spawnedThisTurn;
  private MonsterModel _canonicalInstance;

  public override bool ShouldReceiveCombatHooks => true;

  public virtual LocString Title => MonsterModel.L10NMonsterLookup(this.Id.Entry + ".name");

  public abstract int MinInitialHp { get; }

  public abstract int MaxInitialHp { get; }

  public virtual bool IsHealthBarVisible => true;

  public virtual Vector2 ExtraDeathVfxPadding => MonsterModel.defaultDeathVfxPadding;

  public virtual float HpBarSizeReduction => 0.0f;

  protected virtual string VisualsPath
  {
    get => SceneHelper.GetScenePath("creature_visuals/" + this.Id.Entry.ToLowerInvariant());
  }

  public virtual IEnumerable<string> AssetPaths
  {
    get
    {
      int capacity = 1;
      List<string> stringList = new List<string>(capacity);
      CollectionsMarshal.SetCount<string>(stringList, capacity);
      CollectionsMarshal.AsSpan<string>(stringList)[0] = this.VisualsPath;
      List<string> assetPaths = stringList;
      foreach (AbstractIntent intent in this.GetIntents())
        assetPaths.AddRange(intent.AssetPaths);
      return (IEnumerable<string>) assetPaths;
    }
  }

  public Rng Rng
  {
    get => !this.IsMutable ? Rng.Chaotic : this._rng;
    set
    {
      this.AssertMutable();
      this._rng = value;
    }
  }

  public RunRngSet RunRng
  {
    get => this._runRng;
    set
    {
      this.AssertMutable();
      this._runRng = this._runRng == null ? value : throw new InvalidOperationException("RunRng has already been set!");
    }
  }

  public bool IsPerformingMove
  {
    get => this._isPerformingMove;
    private set
    {
      this.AssertMutable();
      this._isPerformingMove = value;
    }
  }

  public NCreatureVisuals CreateVisuals()
  {
    try
    {
      return PreloadManager.Cache.GetScene(this.VisualsPath).Instantiate<NCreatureVisuals>((PackedScene.GenEditState) 0L);
    }
    catch (Exception ex)
    {
      Log.Error($"Encountered exception loading the creature visuals for {this._creature?.Name}. Falling back to error scene. Exception: {ex}");
      SentryService.CaptureException(ex);
      return this.CreateFallbackVisuals();
    }
  }

  private NCreatureVisuals CreateFallbackVisuals()
  {
    return PreloadManager.Cache.GetScene(MonsterModel._fallbackVisualsPath).Instantiate<NCreatureVisuals>((PackedScene.GenEditState) 0L);
  }

  protected virtual string AttackSfx
  {
    get
    {
      return $"event:/sfx/enemy/enemy_attacks/{this.Id.Entry.ToLowerInvariant()}/{this.Id.Entry.ToLowerInvariant()}_attack";
    }
  }

  protected virtual string CastSfx
  {
    get
    {
      return $"event:/sfx/enemy/enemy_attacks/{this.Id.Entry.ToLowerInvariant()}/{this.Id.Entry.ToLowerInvariant()}_cast";
    }
  }

  public virtual string DeathSfx
  {
    get
    {
      return $"event:/sfx/enemy/enemy_attacks/{this.Id.Entry.ToLowerInvariant()}/{this.Id.Entry.ToLowerInvariant()}_die";
    }
  }

  public virtual bool HasDeathSfx => true;

  public virtual string? HurtSfx => (string) null;

  public virtual bool HasHurtSfx => this.HurtSfx != null;

  protected virtual bool HasPhobiaSpineSkin => false;

  public virtual bool ShouldFadeAfterDeath => true;

  public virtual bool ShouldDisappearFromDoom => true;

  public virtual float HurtAnimationTrackOffsetForDoom => 0.1f;

  public virtual bool ShouldShowInCompendium => true;

  public virtual float DeathAnimLengthOverride => 0.0f;

  public bool HasDeathAnimLengthOverride => (double) this.DeathAnimLengthOverride > 0.0;

  public virtual bool CanChangeScale => true;

  public virtual DamageSfxType TakeDamageSfxType => DamageSfxType.Armor;

  public virtual string TakeDamageSfx
  {
    get
    {
      return "event:/sfx/enemy/enemy_impact_enemy_size/enemy_impact_" + StringHelper.Slugify(this.TakeDamageSfxType.ToString()).ToLowerInvariant();
    }
  }

  public Creature Creature
  {
    get
    {
      return this._creature ?? throw new InvalidOperationException("Creature was accessed before it was set.");
    }
    set
    {
      this.AssertMutable();
      this._creature = this._creature == null ? value : throw new InvalidOperationException($"Monster {this.Id.Entry} already has a creature.");
    }
  }

  public ICombatState CombatState
  {
    get => this.Creature.CombatState ?? (ICombatState) NullCombatState.Instance;
  }

  private List<AbstractIntent> GetIntents()
  {
    List<AbstractIntent> intents = new List<AbstractIntent>();
    foreach (MonsterState monsterState in this.GenerateMoveStateMachine().States.Values)
    {
      if (monsterState.IsMove && monsterState is MoveState moveState)
        intents.AddRange((IEnumerable<AbstractIntent>) moveState.Intents);
    }
    return intents;
  }

  public MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine.MonsterMoveStateMachine? MoveStateMachine
  {
    get => this._moveStateMachine;
    private set
    {
      this.AssertMutable();
      if (this.MoveStateMachine != null)
        throw new InvalidOperationException(this.Id.Entry + "'s move state machine has already been set");
      this._moveStateMachine = value;
    }
  }

  public MoveState NextMove { get; private set; } = new MoveState();

  public bool IntendsToAttack
  {
    get
    {
      return this.NextMove.Intents.Any<AbstractIntent>((Func<AbstractIntent, bool>) (intent =>
      {
        bool intendsToAttack;
        switch (intent.IntentType)
        {
          case IntentType.Attack:
          case IntentType.DeathBlow:
            intendsToAttack = true;
            break;
          default:
            intendsToAttack = false;
            break;
        }
        return intendsToAttack;
      }));
    }
  }

  public bool SpawnedThisTurn
  {
    get => this._spawnedThisTurn;
    private set
    {
      this.AssertMutable();
      this._spawnedThisTurn = value;
    }
  }

  public MonsterModel CanonicalInstance
  {
    get => !this.IsMutable ? this : this._canonicalInstance;
    private set
    {
      this.AssertMutable();
      this._canonicalInstance = value;
    }
  }

  public virtual Task AfterAddedToRoom() => Task.CompletedTask;

  public virtual void BeforeRemovedFromRoom()
  {
  }

  public virtual List<BestiaryMonsterMove> GenerateBestiaryMoveList(NCreatureVisuals? creatureVisuals)
  {
    List<BestiaryMonsterMove> bestiaryMoveList = new List<BestiaryMonsterMove>();
    foreach (string allMove in this.GetAllMoves(this.MoveStateMachine))
    {
      if (this.ShouldShowMoveInBestiary(allMove))
      {
        string moveId = allMove;
        if (moveId.EndsWith("_MOVE"))
        {
          string str = moveId;
          moveId = str.Substring(0, str.Length - 5);
        }
        LocString bestiaryMoveName = this.GetBestiaryMoveName(moveId);
        if (!bestiaryMoveName.Exists())
        {
          if (!moveId.EndsWith('2') && !moveId.EndsWith('3') && !moveId.EndsWith('4'))
          {
            Log.Warn($"No loc for move {allMove} in monster {this.Title.GetFormattedText()}");
            bestiaryMoveList.Add(BestiaryMonsterMove.FromState(allMove));
          }
        }
        else
          bestiaryMoveList.Add(BestiaryMonsterMove.FromState(bestiaryMoveName, allMove));
      }
    }
    MegaSkeletonDataResource data = creatureVisuals?.SpineBody?.GetSkeleton()?.GetData();
    if (data != null && data.HasAnimation("revive"))
      bestiaryMoveList.Add(BestiaryMonsterMove.FromAnim("revive", (string) null));
    if (data != null && data.HasAnimation("hurt"))
      bestiaryMoveList.Add(BestiaryMonsterMove.FromAnim("hurt", this.TakeDamageSfx).StopOtherSfx());
    if (data != null && data.HasAnimation("die"))
      bestiaryMoveList.Add(BestiaryMonsterMove.FromAnim("die", this.DeathSfx).StopOtherSfx());
    return bestiaryMoveList;
  }

  protected virtual bool ShouldShowMoveInBestiary(string moveStateId) => true;

  private IEnumerable<string> GetAllMoves(MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine.MonsterMoveStateMachine machine)
  {
    foreach (KeyValuePair<string, MonsterState> state in machine.States)
    {
      if (state.Value is MoveState)
        yield return state.Key;
    }
  }

  public void ResetStateMachine() => this._moveStateMachine = (MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine.MonsterMoveStateMachine) null;

  public static LocString L10NMonsterLookup(string entryName)
  {
    return new LocString("monsters", entryName);
  }

  public MonsterModel ToMutable()
  {
    this.AssertCanonical();
    MonsterModel mutable = (MonsterModel) this.MutableClone();
    mutable.CanonicalInstance = this;
    return mutable;
  }

  protected abstract MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine.MonsterMoveStateMachine GenerateMoveStateMachine();

  public void SetUpForCombat()
  {
    this.MoveStateMachine = this.GenerateMoveStateMachine();
    this.SpawnedThisTurn = true;
  }

  public void RollMove(IEnumerable<Creature> targets)
  {
    this.NextMove = this.MoveStateMachine.RollMove(targets, this.Creature, this.RunRng.MonsterAi);
  }

  public void SetMoveImmediate(MoveState state, bool forceTransition = false)
  {
    if (!this.NextMove.CanTransitionAway && !forceTransition)
      return;
    this.NextMove = state;
    this.MoveStateMachine.ForceCurrentState((MonsterState) state);
    NCreature creatureNode = this.Creature.GetCreatureNode();
    if (creatureNode == null || !this.CombatState.IsLiveCombat())
      return;
    TaskHelper.RunSafely(creatureNode.RefreshIntents());
  }

  public async Task PerformMove()
  {
    ICombatState combatState;
    MoveState move;
    IReadOnlyList<Creature> targets;
    if (this.CombatState == null)
    {
      combatState = (ICombatState) null;
      move = (MoveState) null;
      targets = (IReadOnlyList<Creature>) null;
    }
    else
    {
      combatState = this.CombatState;
      await Cmd.CustomScaledWait(0.1f, 0.2f);
      this.IsPerformingMove = true;
      move = this.NextMove;
      targets = combatState.PlayerCreatures;
      if (TestMode.IsOff)
        Log.Info($"Monster {this.Id.Entry} performing move {move.Id}");
      await move.PerformMove((IEnumerable<Creature>) targets);
      this.MoveStateMachine?.OnMovePerformed(move);
      CombatManager.Instance.History.MonsterPerformedMove(combatState, this, move, (IEnumerable<Creature>) targets);
      this.IsPerformingMove = false;
      if (this.Creature.IsDead && Hook.ShouldCreatureBeRemovedFromCombatAfterDeath(combatState, this.Creature))
        combatState.RemoveCreature(this.Creature);
      await Cmd.CustomScaledWait(0.1f, 0.4f);
      combatState = (ICombatState) null;
      move = (MoveState) null;
      targets = (IReadOnlyList<Creature>) null;
    }
  }

  public virtual void SetupSkins(MegaSprite spine, MegaSkeleton skeleton)
  {
  }

  public virtual CreatureAnimator GenerateAnimator(MegaSprite controller)
  {
    AnimState animState = new AnimState("idle_loop", true);
    AnimState state1 = new AnimState("cast");
    AnimState state2 = new AnimState("attack");
    AnimState state3 = new AnimState("hurt");
    AnimState state4 = new AnimState("die");
    state1.NextState = animState;
    state2.NextState = animState;
    state3.NextState = animState;
    CreatureAnimator animator = new CreatureAnimator(animState, controller);
    animator.AddAnyState("Idle", animState);
    animator.AddAnyState("Cast", state1);
    animator.AddAnyState("Attack", state2);
    animator.AddAnyState("Dead", state4);
    animator.AddAnyState("Hit", state3);
    return animator;
  }

  public void OnSideSwitch()
  {
    this.AssertMutable();
    this.SpawnedThisTurn = false;
  }

  public virtual void OnDieToDoom()
  {
  }

  protected LocString GetBestiaryMoveName(string moveId)
  {
    return new LocString("monsters", $"{this.Id.Entry}.moves.{moveId}.title");
  }

  public void OnPhobiaModeToggled(bool isOn, MegaSprite spine, MegaSkeleton skeleton)
  {
    if (!this.HasPhobiaSpineSkin)
      return;
    MegaSkin skin = spine.NewSkin("custom-skin");
    MegaSkeletonDataResource data = skeleton.GetData();
    skin.AddSkin(data.FindSkin(isOn ? "phobia" : "normal"));
    skeleton.SetSkin(skin);
    skeleton.SetSlotsToSetupPose();
  }
}
