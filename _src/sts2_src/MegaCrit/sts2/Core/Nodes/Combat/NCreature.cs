// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Combat.NCreature
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Animation;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Bindings.MegaSpine;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Entities.UI;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Monsters;
using MegaCrit.Sts2.Core.MonsterMoves.Intents;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.HoverTips;
using MegaCrit.Sts2.Core.Nodes.Multiplayer;
using MegaCrit.Sts2.Core.Nodes.Orbs;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Screens.ScreenContext;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.Nodes.Vfx.Ui;
using MegaCrit.Sts2.Core.Random;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.Saves;
using MegaCrit.Sts2.Core.Settings;
using MegaCrit.Sts2.Core.TestSupport;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Combat;

[ScriptPath("res://src/Core/Nodes/Combat/NCreature.cs")]
public class NCreature : Control
{
  private static readonly string _scenePath = SceneHelper.GetScenePath("combat/creature");
  private NCreatureStateDisplay _stateDisplay;
  private Tween? _intentFadeTween;
  private Tween? _shakeTween;
  private CreatureAnimator? _spineAnimator;
  private bool _isRemotePlayerOrPet;
  private bool _isInBestiary;
  private float _tempScale = 1f;
  private Tween? _scaleTween;
  private bool _isInMultiselect;
  private NSelectionReticle _selectionReticle;
  private readonly Dictionary<string, (string, float)> _sfxLoops = new Dictionary<string, (string, float)>();

  public static IEnumerable<string> AssetPaths
  {
    get
    {
      return (IEnumerable<string>) new \u003C\u003Ez__ReadOnlySingleElementList<string>(NCreature._scenePath);
    }
  }

  public static Vector2 PowerAppliedVfxPositionOffset => new Vector2(0.0f, -200f);

  public Task? DeathAnimationTask { get; set; }

  public CancellationTokenSource DeathAnimCancelToken { get; } = new CancellationTokenSource();

  public Control Hitbox { get; private set; }

  public NOrbManager? OrbManager { get; private set; }

  public bool IsInteractable { get; private set; } = true;

  public Creature Entity { get; private set; }

  public Vector2 VfxSpawnPosition => ((Node2D) this.Visuals.VfxSpawnPosition).GlobalPosition;

  public Vector2 PowerAppliedVfxSpawnPosition
  {
    get => Vector2.op_Addition(this.VfxSpawnPosition, NCreature.PowerAppliedVfxPositionOffset);
  }

  public NCreatureVisuals Visuals { get; private set; }

  public Node2D Body => this.Visuals.GetCurrentBody();

  public Control IntentContainer { get; private set; }

  public bool IsPlayingDeathAnimation => this.DeathAnimationTask != null;

  public bool HasSpineAnimation => this.Visuals.HasSpineAnimation;

  public SpineAnimationAccess SpineAnimation => this.Visuals.SpineAnimation;

  public bool IsFocused { get; private set; }

  public NMultiplayerPlayerIntentHandler? PlayerIntentHandler { get; private set; }

  public T? GetSpecialNode<T>(string name) where T : Node
  {
    return ((Node) this.Visuals).GetNodeOrNull<T>(NodePath.op_Implicit(name));
  }

  public static NCreature? Create(Creature entity)
  {
    if (TestMode.IsOn)
      return (NCreature) null;
    NCreature ncreature = PreloadManager.Cache.GetScene(NCreature._scenePath).Instantiate<NCreature>((PackedScene.GenEditState) 0L);
    ncreature.Entity = entity;
    ncreature.Visuals = entity.CreateVisuals();
    return ncreature;
  }

  public override void _Ready()
  {
    this._stateDisplay = ((Node) this).GetNode<NCreatureStateDisplay>(NodePath.op_Implicit("%HealthBar"));
    this.IntentContainer = ((Node) this).GetNode<Control>(NodePath.op_Implicit("%Intents"));
    this.Hitbox = ((Node) this).GetNode<Control>(NodePath.op_Implicit("%Hitbox"));
    this._selectionReticle = ((Node) this).GetNode<NSelectionReticle>(NodePath.op_Implicit("%SelectionReticle"));
    ((GodotObject) this.Hitbox).Connect(Control.SignalName.FocusEntered, Callable.From(new Action(this.OnFocus)), 0U);
    ((GodotObject) this.Hitbox).Connect(Control.SignalName.FocusExited, Callable.From(new Action(this.OnUnfocus)), 0U);
    ((GodotObject) this.Hitbox).Connect(Control.SignalName.MouseEntered, Callable.From(new Action(this.OnFocus)), 0U);
    ((GodotObject) this.Hitbox).Connect(Control.SignalName.MouseExited, Callable.From(new Action(this.OnUnfocus)), 0U);
    if (this.Entity.IsPlayer)
    {
      this.OrbManager = NOrbManager.Create(this, LocalContext.IsMe(this.Entity));
      ((Node) this).AddChildSafely((Node) this.OrbManager);
      this.UpdateNavigation();
    }
    int num;
    if (this.Entity.IsPlayer)
    {
      ICombatState combatState = this.Entity.CombatState;
      num = combatState != null ? (combatState.RunState.Players.Count > 1 ? 1 : 0) : 0;
    }
    else
      num = 0;
    if (num != 0)
    {
      this.PlayerIntentHandler = NMultiplayerPlayerIntentHandler.Create(this.Entity.Player);
      if (this.PlayerIntentHandler != null)
      {
        ((Node) this.IntentContainer).AddChildSafely((Node) this.PlayerIntentHandler);
        ((CanvasItem) this.IntentContainer).Modulate = Colors.White;
      }
    }
    ((Node) this).AddChildSafely((Node) this.Visuals);
    ((Node) this).MoveChildSafely((Node) this.Visuals, 0);
    this.Visuals.Position = Vector2.Zero;
    this._stateDisplay.SetCreature(this.Entity);
    bool flag = this.Entity.PetOwner != null && !LocalContext.IsMe(this.Entity.PetOwner);
    this._isRemotePlayerOrPet = (this.Entity.IsPlayer && !LocalContext.IsMe(this.Entity)) | flag;
    if (this._isRemotePlayerOrPet)
      this._stateDisplay.HideImmediately();
    else
      this._stateDisplay.AnimateIn(NCombatRoom.Instance != null && Time.GetTicksMsec() - NCombatRoom.Instance.CreatedMsec < 1000UL ? HealthBarAnimMode.SpawnedAtCombatStart : HealthBarAnimMode.SpawnedDuringCombat);
    if (this.HasSpineAnimation)
    {
      if (this.Entity.Player != null)
      {
        this._spineAnimator = this.Entity.Player.Character.GenerateAnimator(this.Visuals.SpineBody);
      }
      else
      {
        this._spineAnimator = this.Entity.Monster.GenerateAnimator(this.Visuals.SpineBody);
        this.Visuals.SetUpSkin(this.Entity.Monster);
      }
      this.ConnectSpineAnimatorSignals();
      if (this.Entity.IsDead)
      {
        this.SetAnimationTrigger("Dead");
        using (MegaTrackEntry currentTrack = this.SpineAnimation.GetCurrentTrack())
          currentTrack?.SetTrackTime(currentTrack.GetAnimationEnd());
      }
    }
    this.SetOrbManagerPosition();
    if (this.Entity.Monster != null)
      this.ToggleIsInteractable(this.Entity.Monster.IsHealthBarVisible);
    this.UpdateBounds((Node) this.Visuals);
    this.UpdatePhobiaMode();
  }

  public override void _EnterTree()
  {
    ((Node) this)._EnterTree();
    ((GodotObject) NGame.Instance)?.Connect(NGame.SignalName.PhobiaModeToggled, Callable.From(new Action(this.UpdatePhobiaMode)), 0U);
    CombatManager.Instance.CombatEnded += new Action<CombatRoom>(this.OnCombatEnded);
    this.Entity.PowerApplied += new Action<PowerModel>(this.OnPowerApplied);
    this.Entity.PowerRemoved += new Action<PowerModel>(this.OnPowerRemoved);
    this.Entity.PowerIncreased += new Action<PowerModel, int, bool>(this.OnPowerIncreased);
    foreach (PowerModel power in (IEnumerable<PowerModel>) this.Entity.Powers)
      this.SubscribeToPower(power);
    this.ConnectSpineAnimatorSignals();
  }

  public override void _ExitTree()
  {
    ((Node) this)._ExitTree();
    this.StopAllSfxLoops();
    this.DeathAnimCancelToken.Cancel();
    ((GodotObject) NGame.Instance)?.Disconnect(NGame.SignalName.PhobiaModeToggled, Callable.From(new Action(this.UpdatePhobiaMode)));
    CombatManager.Instance.CombatEnded -= new Action<CombatRoom>(this.OnCombatEnded);
    this.Entity.PowerApplied -= new Action<PowerModel>(this.OnPowerApplied);
    this.Entity.PowerRemoved -= new Action<PowerModel>(this.OnPowerRemoved);
    this.Entity.PowerIncreased -= new Action<PowerModel, int, bool>(this.OnPowerIncreased);
    foreach (PowerModel power in (IEnumerable<PowerModel>) this.Entity.Powers)
      this.UnsubscribeFromPower(power);
    if (this._spineAnimator != null)
      this._spineAnimator.BoundsUpdated -= new Action<string>(this.UpdateBounds);
    CombatManager.Instance.StateTracker.CombatStateChanged -= new Action<CombatState>(this.ShowCreatureHoverTips);
  }

  private void ConnectSpineAnimatorSignals()
  {
    if (this._spineAnimator == null)
      return;
    this._spineAnimator.BoundsUpdated -= new Action<string>(this.UpdateBounds);
    this._spineAnimator.BoundsUpdated += new Action<string>(this.UpdateBounds);
  }

  private void UpdateBounds(string boundsNodeName)
  {
    this.UpdateBounds((Node) ((Node) this.Visuals).GetNode<Control>(NodePath.op_Implicit(boundsNodeName)));
  }

  private void UpdatePhobiaMode() => this.Visuals.UpdatePhobiaMode(this.Entity.Monster);

  private void UpdateBounds(Node boundsContainer)
  {
    Control node = boundsContainer.GetNode<Control>(NodePath.op_Implicit("%Bounds"));
    Vector2 vector2_1 = Vector2.op_Division(Vector2.op_Multiply(node.Size, this.Visuals.Scale), this._tempScale);
    Vector2 vector2_2 = Vector2.op_Division(Vector2.op_Subtraction(node.GlobalPosition, this.GlobalPosition), this._tempScale);
    this.Hitbox.Size = vector2_1;
    this.Hitbox.GlobalPosition = Vector2.op_Addition(this.GlobalPosition, vector2_2);
    this._selectionReticle.Size = vector2_1;
    this._selectionReticle.GlobalPosition = Vector2.op_Addition(this.GlobalPosition, vector2_2);
    this._selectionReticle.PivotOffset = Vector2.op_Multiply(this._selectionReticle.Size, 0.5f);
    this.IntentContainer.Position = Vector2.op_Subtraction(((Node2D) boundsContainer.GetNode<Marker2D>(NodePath.op_Implicit("IntentPos"))).Position, Vector2.op_Multiply(this.IntentContainer.Size, 0.5f));
    this.IntentContainer.Position = new Vector2(this.IntentContainer.Position.X, this.IntentContainer.Position.Y * this.Visuals.Scale.X);
    this._stateDisplay.SetCreatureBounds(this.Hitbox);
  }

  public void UpdateNavigation()
  {
    if (this.OrbManager == null)
      return;
    this.Hitbox.FocusNeighborTop = ((Node) this.OrbManager.DefaultFocusOwner).GetPath();
  }

  public Task UpdateIntent(IEnumerable<Creature> targets)
  {
    if (this.Entity.Monster == null)
      throw new InvalidOperationException("Only valid on monsters.");
    IReadOnlyList<AbstractIntent> intents = this.Entity.Monster.NextMove.Intents;
    int index;
    for (index = 0; index < intents.Count && index < ((Node) this.IntentContainer).GetChildCount(false); ++index)
    {
      NIntent child = ((Node) this.IntentContainer).GetChild<NIntent>(index, false);
      child.SetFrozen(false);
      child.UpdateIntent(intents[index], targets, this.Entity);
    }
    float num = (float) ((object) this).GetHashCode() * 0.01f;
    for (; index < intents.Count; ++index)
    {
      NIntent child = NIntent.Create(num + (float) index * 0.3f);
      ((Node) this.IntentContainer).AddChildSafely((Node) child);
      child.UpdateIntent(intents[index], targets, this.Entity);
    }
    foreach (Node node in Enumerable.TakeLast<Node>((IEnumerable<Node>) ((Node) this.IntentContainer).GetChildren(false), ((Node) this.IntentContainer).GetChildCount(false) - index).ToList<Node>())
    {
      ((Node) this.IntentContainer).RemoveChildSafely(node);
      node.QueueFreeSafely();
    }
    return Task.CompletedTask;
  }

  public async Task PerformIntent()
  {
    foreach (NIntent nintent in ((IEnumerable) ((Node) this.IntentContainer).GetChildren(false)).OfType<NIntent>())
    {
      nintent.PlayPerform();
      nintent.SetFrozen(true);
    }
    if (SaveManager.Instance.PrefsSave.FastMode == FastModeType.Instant)
    {
      ((CanvasItem) this.IntentContainer).Modulate = new Color(((CanvasItem) this.IntentContainer).Modulate.R, ((CanvasItem) this.IntentContainer).Modulate.G, ((CanvasItem) this.IntentContainer).Modulate.B, 0.0f);
    }
    else
    {
      this.AnimHideIntent(0.4);
      await Cmd.CustomScaledWait(0.25f, 0.4f);
    }
  }

  public async Task RefreshIntents()
  {
    await this.UpdateIntent(this.Entity.CombatState.Players.Select<Player, Creature>((Func<Player, Creature>) (p => p.Creature)));
    await this.RevealIntents();
  }

  private Task RevealIntents()
  {
    ((CanvasItem) this.IntentContainer).Modulate = Colors.Transparent;
    this._intentFadeTween?.Kill();
    this._intentFadeTween = ((Node) this).CreateTween().SetParallel(true);
    this._intentFadeTween.TweenProperty((GodotObject) this.IntentContainer, NodePath.op_Implicit("modulate:a"), Variant.op_Implicit(1f), 1.0).SetDelay((double) Rng.Chaotic.NextFloat(0.0f, 0.3f));
    return Task.CompletedTask;
  }

  private void OnFocus()
  {
    if (this.IsFocused || this._isInBestiary)
      return;
    this.IsFocused = true;
    if (this._isRemotePlayerOrPet)
    {
      this._stateDisplay.AnimateIn(HealthBarAnimMode.FromHidden);
      ((CanvasItem) this._stateDisplay).ZIndex = 1;
      NCombatRoom.Instance?.GetCreatureNode(LocalContext.GetMe(this.Entity.CombatState)?.Creature)?.SetRemotePlayerFocused(true);
    }
    else
      this._stateDisplay.ShowNameplate();
    NRun.Instance.GlobalUi.MultiplayerPlayerContainer.HighlightPlayer(this.Entity.Player);
    if (NTargetManager.Instance.IsInSelection)
    {
      NTargetManager.Instance.OnNodeHovered((Node) this);
    }
    else
    {
      if (NControllerManager.Instance.IsUsingController)
        this.ShowSingleSelectReticle();
      this.ShowHoverTips(this.Entity.HoverTips);
      CombatManager.Instance.StateTracker.CombatStateChanged += new Action<CombatState>(this.ShowCreatureHoverTips);
    }
  }

  private void OnUnfocus()
  {
    if (this._isInBestiary)
      return;
    this.IsFocused = false;
    this.HideSingleSelectReticle();
    if (this._isRemotePlayerOrPet)
    {
      this._stateDisplay.AnimateOut();
      NCombatRoom.Instance?.GetCreatureNode(LocalContext.GetMe(this.Entity.CombatState)?.Creature)?.SetRemotePlayerFocused(false);
    }
    else
      this._stateDisplay.HideNameplate();
    NRun.Instance.GlobalUi.MultiplayerPlayerContainer.UnhighlightPlayer(this.Entity.Player);
    NTargetManager.Instance.OnNodeUnhovered((Node) this);
    CombatManager.Instance.StateTracker.CombatStateChanged -= new Action<CombatState>(this.ShowCreatureHoverTips);
    this.HideHoverTips();
  }

  public void OnTargetingStarted()
  {
    if (!this.IsFocused)
      return;
    NTargetManager.Instance.OnNodeHovered((Node) this);
    CombatManager.Instance.StateTracker.CombatStateChanged -= new Action<CombatState>(this.ShowCreatureHoverTips);
    this.HideHoverTips();
  }

  private void ShowCreatureHoverTips(CombatState _)
  {
    if (this.Entity.CombatState == null)
      return;
    this.ShowHoverTips(this.Entity.HoverTips);
  }

  public void ShowHoverTips(IEnumerable<IHoverTip> hoverTips)
  {
    if (NCombatRoom.Instance.Ui.Hand.InCardPlay)
      return;
    this.HideHoverTips();
    NHoverTipSet.CreateAndShow(this.Hitbox, hoverTips, HoverTip.GetHoverTipAlignment((Control) this, 0.5f));
  }

  public void SetRemotePlayerFocused(bool remotePlayerFocused)
  {
    if (!LocalContext.IsMe(this.Entity))
      throw new InvalidOperationException("This should only be called on the local player's creature node!");
    if (remotePlayerFocused)
    {
      this._stateDisplay.AnimateOut();
    }
    else
    {
      if (!this.Entity.IsAlive)
        return;
      this._stateDisplay.AnimateIn(HealthBarAnimMode.FromHidden);
    }
  }

  public void HideHoverTips() => NHoverTipSet.Remove(this.Hitbox);

  private void SubscribeToPower(PowerModel power)
  {
    power.Flashed += new Action<PowerModel>(this.OnPowerFlashed);
  }

  private void UnsubscribeFromPower(PowerModel power)
  {
    power.Flashed -= new Action<PowerModel>(this.OnPowerFlashed);
  }

  private void OnPowerApplied(PowerModel power) => this.SubscribeToPower(power);

  private void OnPowerIncreased(PowerModel power, int amount, bool silent)
  {
    if (silent || !CombatManager.Instance.IsInProgress)
      return;
    bool isBuff = power.GetTypeForAmount((Decimal) power.Amount) == PowerType.Buff;
    NPowerAppliedVfx vfx = NPowerAppliedVfx.Create(power, amount, isBuff);
    if (vfx != null)
    {
      if (isBuff)
      {
        NPowerAppliedBuffVfx buffVfx = NPowerAppliedBuffVfx.Create(this.PowerAppliedVfxSpawnPosition);
        Callable callable = Callable.From((Action) (() =>
        {
          NCombatRoom instance = NCombatRoom.Instance;
          if (instance == null)
            return;
          ((Node) instance.CombatVfxContainer).AddChildSafely((Node) buffVfx);
        }));
        ((Callable) ref callable).CallDeferred(Array.Empty<Variant>());
      }
      else
      {
        NPowerAppliedDebuffVfx debuffVfx = NPowerAppliedDebuffVfx.Create(this.PowerAppliedVfxSpawnPosition);
        Callable callable = Callable.From((Action) (() =>
        {
          NCombatRoom instance = NCombatRoom.Instance;
          if (instance == null)
            return;
          ((Node) instance.CombatVfxContainer).AddChildSafely((Node) debuffVfx);
        }));
        ((Callable) ref callable).CallDeferred(Array.Empty<Variant>());
      }
      Callable callable1 = Callable.From((Action) (() =>
      {
        NCombatRoom instance = NCombatRoom.Instance;
        if (instance == null)
          return;
        ((Node) instance.CombatVfxContainer).AddChildSafely((Node) vfx);
      }));
      ((Callable) ref callable1).CallDeferred(Array.Empty<Variant>());
    }
    if (power.ShouldPlayVfx)
      SfxCmd.Play(isBuff ? "event:/sfx/buff" : "event:/sfx/debuff");
    if (isBuff)
      return;
    this.AnimShake();
  }

  private void OnPowerRemoved(PowerModel power)
  {
    NPowerRemovedVfx vfx = NPowerRemovedVfx.Create(power);
    if (vfx != null)
    {
      Callable callable = Callable.From((Action) (() =>
      {
        NCombatRoom instance = NCombatRoom.Instance;
        if (instance == null)
          return;
        ((Node) instance.CombatVfxContainer).AddChildSafely((Node) vfx);
      }));
      ((Callable) ref callable).CallDeferred(Array.Empty<Variant>());
    }
    this.UnsubscribeFromPower(power);
  }

  private void OnPowerFlashed(PowerModel power)
  {
    NPowerFlashVfx vfx = NPowerFlashVfx.Create(power);
    if (vfx == null)
      return;
    Callable callable = Callable.From((Action) (() =>
    {
      NCombatRoom instance = NCombatRoom.Instance;
      if (instance == null)
        return;
      ((Node) instance.CombatVfxContainer).AddChildSafely((Node) vfx);
    }));
    ((Callable) ref callable).CallDeferred(Array.Empty<Variant>());
  }

  private void OnCombatEnded(CombatRoom _)
  {
    this.AnimHideIntent();
    this.OrbManager?.ClearOrbs();
  }

  public void SetAnimationTrigger(string trigger) => this._spineAnimator?.SetTrigger(trigger);

  public float GetCurrentAnimationLength()
  {
    return this.SpineAnimation.GetCurrentAnimationDuration().GetValueOrDefault();
  }

  public float GetCurrentAnimationTimeRemaining()
  {
    using (MegaTrackEntry currentTrack = this.SpineAnimation.GetCurrentTrack())
      return currentTrack == null ? 0.0f : currentTrack.GetTrackComplete() - currentTrack.GetTrackTime();
  }

  public void ToggleIsInteractable(bool on)
  {
    this.IsInteractable = on;
    ((CanvasItem) this._stateDisplay).Visible = !NCombatUi.IsDebugHidingHpBar & on;
    this.Hitbox.MouseFilter = on ? (Control.MouseFilterEnum) 0L : (Control.MouseFilterEnum) 2L;
    this.Hitbox.FocusMode = on ? (Control.FocusModeEnum) 2L : (Control.FocusModeEnum) 0L;
  }

  public void DisableInteractionForDeath()
  {
    if (this.Hitbox.HasFocus())
      ActiveScreenContext.Instance.FocusOnDefaultControl();
    this.Hitbox.FocusMode = (Control.FocusModeEnum) 0L;
    this.Hitbox.MouseFilter = (Control.MouseFilterEnum) 2L;
  }

  public Tween AnimDisableUi()
  {
    Tween tween = ((Node) this).CreateTween();
    if (!((Node) this).IsNodeReady())
    {
      tween.TweenInterval(0.0);
      return tween;
    }
    tween.TweenProperty((GodotObject) this._stateDisplay, NodePath.op_Implicit("modulate:a"), Variant.op_Implicit(0.0f), 0.5).SetDelay(0.5).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 5L);
    return tween;
  }

  public Tween AnimEnableUi()
  {
    Tween tween = ((Node) this).CreateTween();
    tween.TweenProperty((GodotObject) this._stateDisplay, NodePath.op_Implicit("modulate:a"), Variant.op_Implicit(1f), 0.5).SetDelay(0.5).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 5L);
    return tween;
  }

  public float StartDeathAnim(bool shouldRemove)
  {
    this.DisableInteractionForDeath();
    foreach (NIntent nintent in ((IEnumerable) ((Node) this.IntentContainer).GetChildren(false)).OfType<NIntent>())
      nintent.SetFrozen(true);
    Task deathAnimationTask = this.DeathAnimationTask;
    if (deathAnimationTask != null && !deathAnimationTask.IsCompleted)
      return 0.0f;
    float num = 0.0f;
    if (this._spineAnimator != null)
    {
      MonsterModel monster = this.Entity.Monster;
      if ((monster != null ? (monster.HasDeathSfx ? 1 : 0) : 0) != 0)
        SfxCmd.PlayDeath(this.Entity.Monster);
      if (this.Entity.Player != null)
        SfxCmd.PlayDeath(this.Entity.Player);
      this.SetAnimationTrigger("Dead");
      num = this.GetCurrentAnimationLength();
    }
    this.DeathAnimationTask = this.AnimDie(shouldRemove, this.DeathAnimCancelToken.Token);
    TaskHelper.RunSafely(this.DeathAnimationTask);
    MonsterModel monster1 = this.Entity.Monster;
    return monster1 != null && monster1.HasDeathAnimLengthOverride ? this.Entity.Monster.DeathAnimLengthOverride : Mathf.Min(num, 30f);
  }

  public void StartReviveAnim()
  {
    CreatureAnimator spineAnimator = this._spineAnimator;
    if ((spineAnimator != null ? (spineAnimator.HasTrigger("Revive") ? 1 : 0) : 0) != 0)
      this.SetAnimationTrigger("Revive");
    else if (this.Entity.IsPlayer)
      this.AnimTempRevive();
    if (!this._isRemotePlayerOrPet)
      this.AnimEnableUi();
    this.Hitbox.MouseFilter = (Control.MouseFilterEnum) 0L;
  }

  private void AnimTempRevive()
  {
    Tween tween = ((Node) this).CreateTween();
    tween.TweenProperty((GodotObject) this.Visuals, NodePath.op_Implicit("modulate:a"), Variant.op_Implicit(0.0f), 0.2);
    tween.TweenCallback(Callable.From(new Action(this.ImmediatelySetIdle)));
    tween.TweenProperty((GodotObject) this.Visuals, NodePath.op_Implicit("modulate:a"), Variant.op_Implicit(1f), 0.2);
  }

  private void ImmediatelySetIdle()
  {
    this._spineAnimator?.SetTrigger("Idle");
    using (MegaTrackEntry currentTrack = this.SpineAnimation.GetCurrentTrack())
    {
      if (currentTrack == null)
        return;
      currentTrack.SetMixDuration(0.0f);
      currentTrack.SetTrackTime(currentTrack.GetAnimationEnd());
    }
  }

  private async Task AnimDie(bool shouldRemove, CancellationToken cancelToken)
  {
    Tween disableUiTween = this.AnimDisableUi();
    if (!RunManager.Instance.IsSingleplayerOrFakeMultiplayer)
      this.OrbManager?.ClearOrbs();
    if (shouldRemove)
      this.AnimHideIntent();
    if (this._spineAnimator != null)
    {
      await Cmd.Wait(Math.Min(this.GetCurrentAnimationTimeRemaining() + 0.5f, 20f), cancelToken, true);
    }
    else
    {
      MonsterModel monster = this.Entity.Monster;
      if (monster != null && monster.HasDeathAnimLengthOverride)
        await Cmd.Wait(this.Entity.Monster.DeathAnimLengthOverride, cancelToken, true);
    }
    if (cancelToken.IsCancellationRequested)
      disableUiTween = (Tween) null;
    else if (shouldRemove)
    {
      Task fadeVfx = (Task) null;
      MonsterModel monster = this.Entity.Monster;
      if (monster != null && monster.ShouldFadeAfterDeath && ((CanvasItem) this.Body).IsVisibleInTree())
      {
        NMonsterDeathVfx child = NMonsterDeathVfx.Create(this, cancelToken);
        Node parent = ((Node) this).GetParent();
        parent.AddChildSafely((Node) child);
        if (child != null)
          parent.MoveChildSafely((Node) child, ((Node) this).GetIndex(false));
        fadeVfx = child?.PlayVfx();
      }
      if (SaveManager.Instance.PrefsSave.FastMode != FastModeType.Instant)
      {
        if (disableUiTween.IsValid() && disableUiTween.IsRunning())
        {
          if (!await disableUiTween.AwaitFinished((Node) this))
          {
            disableUiTween = (Tween) null;
            return;
          }
        }
        foreach (IDeathDelayer deathDelayer in ((Node) this).GetChildrenRecursive<IDeathDelayer>())
          await deathDelayer.GetDelayTask();
      }
      if (fadeVfx != null)
        await fadeVfx;
      ((Node) this).QueueFreeSafely();
      disableUiTween = (Tween) null;
    }
    else if (!(this.Entity.Monster is Osty))
    {
      disableUiTween = (Tween) null;
    }
    else
    {
      this.OstyScaleToSize(0.0f, 0.75);
      disableUiTween = (Tween) null;
    }
  }

  public void AnimHideIntent(double delay = 0.0)
  {
    this._intentFadeTween?.Kill();
    this._intentFadeTween = ((Node) this).CreateTween().SetParallel(true);
    PropertyTweener propertyTweener = this._intentFadeTween.TweenProperty((GodotObject) this.IntentContainer, NodePath.op_Implicit("modulate:a"), Variant.op_Implicit(0.0f), 0.5);
    if (delay <= 0.0)
      return;
    propertyTweener.SetDelay(delay);
  }

  public void SetScaleAndHue(float scale, float hue)
  {
    this.Visuals.SetScaleAndHue(scale, hue);
    this.UpdateBounds((Node) this.Visuals);
  }

  public void ScaleTo(float size, double duration)
  {
    if (this.Entity.IsMonster && !this.Entity.Monster.CanChangeScale)
      return;
    this._tempScale = size;
    this._scaleTween?.Kill();
    this._scaleTween = ((Node) this).CreateTween();
    this._scaleTween.TweenMethod(Callable.From<Vector2>(new Action<Vector2>(this.DoScaleTween)), Variant.op_Implicit(this.Visuals.Scale), Variant.op_Implicit(Vector2.op_Multiply(Vector2.op_Multiply(Vector2.One, this._tempScale), this.Visuals.DefaultScale)), duration).SetEase((Tween.EaseType) 2L).SetTrans((Tween.TransitionType) 1L);
  }

  public void SetDefaultScaleTo(float size, float duration)
  {
    if (this.Entity.IsMonster && !this.Entity.Monster.CanChangeScale)
      return;
    this.Visuals.DefaultScale = size;
    this.ScaleTo(this._tempScale, (double) duration);
  }

  public void OstyScaleToSize(float ostyHealth, double duration)
  {
    float num = Mathf.Lerp(Osty.ScaleRange.X, Osty.ScaleRange.Y, Mathf.Clamp(ostyHealth / 150f, 0.0f, 1f));
    NCreature creatureNode = NCombatRoom.Instance?.GetCreatureNode(this.Entity.PetOwner.Creature);
    this._scaleTween = ((Node) this).CreateTween();
    Vector2 vector2 = Vector2.op_Multiply(Vector2.op_Multiply(Vector2.One, num), this.Visuals.DefaultScale);
    if (NCombatRoom.Instance != null && !((Node) NCombatRoom.Instance.SceneContainer).IsAncestorOf((Node) this.Visuals))
    {
      ICombatState combatState = this.Entity.CombatState;
      if (combatState != null && combatState.Encounter != null)
        vector2 = Vector2.op_Multiply(vector2, this.Entity.CombatState.Encounter.GetCameraScaling());
    }
    this._scaleTween.TweenProperty((GodotObject) this.Visuals, NodePath.op_Implicit("scale"), Variant.op_Implicit(vector2), duration).SetEase((Tween.EaseType) 2L).SetTrans((Tween.TransitionType) 1L);
    if (LocalContext.IsMe(this.Entity.PetOwner))
      this._scaleTween.Parallel().TweenProperty((GodotObject) this, NodePath.op_Implicit("position"), Variant.op_Implicit(Vector2.op_Addition(creatureNode.Position, NCreature.GetOstyOffsetFromPlayer(this.Entity))), duration);
    this._scaleTween.TweenCallback(Callable.From((Action) (() => this.UpdateBounds((Node) this.Visuals))));
  }

  public static Vector2 GetOstyOffsetFromPlayer(Creature osty)
  {
    Vector2 vector2_1 = Vector2.op_Multiply(Vector2.op_Multiply(Vector2.Right, NCombatRoom.Instance?.GetCreatureNode(osty.PetOwner.Creature).Hitbox.Size.X), 0.5f);
    Vector2 minOffset = Osty.MinOffset;
    Vector2 vector2_2 = ((Vector2) ref minOffset).Lerp(Osty.MaxOffset, Mathf.Clamp((float) osty.MaxHp / 150f, 0.0f, 1f));
    return Vector2.op_Addition(vector2_1, vector2_2);
  }

  public void AnimShake()
  {
    if (!((Node) this).IsInsideTree() || this._shakeTween != null && this._shakeTween.IsRunning() || this.Visuals.IsPlayingHurtAnimation())
      return;
    this.Visuals.Position = Vector2.Zero;
    this._shakeTween = ((Node) this).CreateTween();
    this._shakeTween.TweenMethod(Callable.From<float>((Action<float>) (t => this.Visuals.Position = Vector2.op_Multiply(Vector2.op_Multiply(Vector2.op_Multiply(Vector2.Right, 10f), Mathf.Sin(t * 4f)), Mathf.Sin(t * 0.5f)))), Variant.op_Implicit(0.0f), Variant.op_Implicit(6.28318548f), 1.0).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 7L);
  }

  private void DoScaleTween(Vector2 scale)
  {
    this.Visuals.Scale = scale;
    this.SetOrbManagerPosition();
  }

  private void SetOrbManagerPosition()
  {
    if (this.OrbManager == null)
      return;
    NOrbManager orbManager1 = this.OrbManager;
    Vector2 vector2;
    if ((double) this.Visuals.Scale.X <= 1.0)
    {
      Vector2 scale = this.Visuals.Scale;
      vector2 = ((Vector2) ref scale).Lerp(Vector2.One, 0.5f);
    }
    else
      vector2 = Vector2.One;
    orbManager1.Scale = vector2;
    this.OrbManager.Position = Vector2.op_Multiply(((Node2D) this.Visuals.OrbPosition).Position, Mathf.Min(this.Visuals.Scale.X, 1.25f));
    if (this.OrbManager.IsLocal)
      return;
    NOrbManager orbManager2 = this.OrbManager;
    orbManager2.Position = Vector2.op_Addition(orbManager2.Position, Vector2.op_Multiply(Vector2.Up, 50f));
  }

  public Vector2 GetTopOfHitbox()
  {
    return Vector2.op_Addition(this.Hitbox.GlobalPosition, new Vector2(this.Hitbox.Size.X * 0.5f, 0.0f));
  }

  public Vector2 GetBottomOfHitbox()
  {
    return Vector2.op_Addition(this.Hitbox.GlobalPosition, new Vector2(this.Hitbox.Size.X * 0.5f, this.Hitbox.Size.Y));
  }

  public void TrackBlockStatus(Creature creature) => this._stateDisplay.TrackBlockStatus(creature);

  public void ShowMultiselectReticle()
  {
    this._isInMultiselect = true;
    this.ShowSingleSelectReticle();
  }

  public void HideMultiselectReticle()
  {
    this._isInMultiselect = false;
    this.HideSingleSelectReticle();
  }

  public void ShowSingleSelectReticle() => this._selectionReticle.OnSelect();

  public void HideSingleSelectReticle()
  {
    if (this._isInMultiselect)
      return;
    this._selectionReticle.OnDeselect();
  }

  public void SetupForBestiary()
  {
    ((CanvasItem) this._stateDisplay).Visible = false;
    ((CanvasItem) this.IntentContainer).Visible = false;
    this._isInBestiary = true;
  }

  public void StartSfxLoop(string sfxName) => this.StartSfxLoop(sfxName, "loop", 1f);

  public void StartSfxLoop(string sfxName, string loopParam, float loopStopValue)
  {
    if (this._sfxLoops.ContainsKey(sfxName))
      return;
    this._sfxLoops.Add(sfxName, (loopParam, loopStopValue));
    SfxCmd.PlayLoop(sfxName, false);
  }

  public void StopSfxLoop(string sfxName)
  {
    (string, float) tuple;
    if (!this._sfxLoops.TryGetValue(sfxName, out tuple))
      return;
    SfxCmd.SetParam(sfxName, tuple.Item1, tuple.Item2);
    SfxCmd.StopLoop(sfxName);
    this._sfxLoops.Remove(sfxName);
  }

  public void StopAllSfxLoops()
  {
    foreach (string sfxName in this._sfxLoops.Keys.ToList<string>())
      this.StopSfxLoop(sfxName);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(42)
    {
      new MethodInfo(NCreature.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCreature.MethodName._EnterTree, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCreature.MethodName._ExitTree, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCreature.MethodName.ConnectSpineAnimatorSignals, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCreature.MethodName.UpdateBounds, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 4L, StringName.op_Implicit("boundsNodeName"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NCreature.MethodName.UpdatePhobiaMode, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCreature.MethodName.UpdateNavigation, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCreature.MethodName.OnFocus, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCreature.MethodName.OnUnfocus, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCreature.MethodName.OnTargetingStarted, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCreature.MethodName.SetRemotePlayerFocused, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 1L, StringName.op_Implicit("remotePlayerFocused"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NCreature.MethodName.HideHoverTips, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCreature.MethodName.SetAnimationTrigger, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 4L, StringName.op_Implicit("trigger"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NCreature.MethodName.GetCurrentAnimationLength, new PropertyInfo((Variant.Type) 3L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCreature.MethodName.GetCurrentAnimationTimeRemaining, new PropertyInfo((Variant.Type) 3L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCreature.MethodName.ToggleIsInteractable, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 1L, StringName.op_Implicit("on"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NCreature.MethodName.DisableInteractionForDeath, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCreature.MethodName.AnimDisableUi, new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Tween"), false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCreature.MethodName.AnimEnableUi, new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Tween"), false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCreature.MethodName.StartDeathAnim, new PropertyInfo((Variant.Type) 3L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 1L, StringName.op_Implicit("shouldRemove"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NCreature.MethodName.StartReviveAnim, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCreature.MethodName.AnimTempRevive, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCreature.MethodName.ImmediatelySetIdle, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCreature.MethodName.AnimHideIntent, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 3L, StringName.op_Implicit("delay"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NCreature.MethodName.SetScaleAndHue, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 3L, StringName.op_Implicit("scale"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false),
        new PropertyInfo((Variant.Type) 3L, StringName.op_Implicit("hue"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NCreature.MethodName.ScaleTo, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 3L, StringName.op_Implicit("size"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false),
        new PropertyInfo((Variant.Type) 3L, StringName.op_Implicit("duration"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NCreature.MethodName.SetDefaultScaleTo, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 3L, StringName.op_Implicit("size"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false),
        new PropertyInfo((Variant.Type) 3L, StringName.op_Implicit("duration"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NCreature.MethodName.OstyScaleToSize, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 3L, StringName.op_Implicit("ostyHealth"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false),
        new PropertyInfo((Variant.Type) 3L, StringName.op_Implicit("duration"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NCreature.MethodName.AnimShake, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCreature.MethodName.DoScaleTween, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 5L, StringName.op_Implicit("scale"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NCreature.MethodName.SetOrbManagerPosition, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCreature.MethodName.GetTopOfHitbox, new PropertyInfo((Variant.Type) 5L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCreature.MethodName.GetBottomOfHitbox, new PropertyInfo((Variant.Type) 5L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCreature.MethodName.ShowMultiselectReticle, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCreature.MethodName.HideMultiselectReticle, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCreature.MethodName.ShowSingleSelectReticle, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCreature.MethodName.HideSingleSelectReticle, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCreature.MethodName.SetupForBestiary, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCreature.MethodName.StartSfxLoop, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 4L, StringName.op_Implicit("sfxName"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NCreature.MethodName.StartSfxLoop, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 4L, StringName.op_Implicit("sfxName"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false),
        new PropertyInfo((Variant.Type) 4L, StringName.op_Implicit("loopParam"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false),
        new PropertyInfo((Variant.Type) 3L, StringName.op_Implicit("loopStopValue"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NCreature.MethodName.StopSfxLoop, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 4L, StringName.op_Implicit("sfxName"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NCreature.MethodName.StopAllSfxLoops, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NCreature.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCreature.MethodName._EnterTree) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._EnterTree();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCreature.MethodName._ExitTree) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._ExitTree();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCreature.MethodName.ConnectSpineAnimatorSignals) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.ConnectSpineAnimatorSignals();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCreature.MethodName.UpdateBounds) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.UpdateBounds(VariantUtils.ConvertTo<string>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCreature.MethodName.UpdatePhobiaMode) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.UpdatePhobiaMode();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCreature.MethodName.UpdateNavigation) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.UpdateNavigation();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCreature.MethodName.OnFocus) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnFocus();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCreature.MethodName.OnUnfocus) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnUnfocus();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCreature.MethodName.OnTargetingStarted) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnTargetingStarted();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCreature.MethodName.SetRemotePlayerFocused) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.SetRemotePlayerFocused(VariantUtils.ConvertTo<bool>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCreature.MethodName.HideHoverTips) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.HideHoverTips();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCreature.MethodName.SetAnimationTrigger) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.SetAnimationTrigger(VariantUtils.ConvertTo<string>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCreature.MethodName.GetCurrentAnimationLength) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      float currentAnimationLength = this.GetCurrentAnimationLength();
      ret = VariantUtils.CreateFrom<float>(ref currentAnimationLength);
      return true;
    }
    if (StringName.op_Equality(ref method, NCreature.MethodName.GetCurrentAnimationTimeRemaining) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      float animationTimeRemaining = this.GetCurrentAnimationTimeRemaining();
      ret = VariantUtils.CreateFrom<float>(ref animationTimeRemaining);
      return true;
    }
    if (StringName.op_Equality(ref method, NCreature.MethodName.ToggleIsInteractable) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.ToggleIsInteractable(VariantUtils.ConvertTo<bool>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCreature.MethodName.DisableInteractionForDeath) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.DisableInteractionForDeath();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCreature.MethodName.AnimDisableUi) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      Tween tween = this.AnimDisableUi();
      ret = VariantUtils.CreateFrom<Tween>(ref tween);
      return true;
    }
    if (StringName.op_Equality(ref method, NCreature.MethodName.AnimEnableUi) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      Tween tween = this.AnimEnableUi();
      ret = VariantUtils.CreateFrom<Tween>(ref tween);
      return true;
    }
    if (StringName.op_Equality(ref method, NCreature.MethodName.StartDeathAnim) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      float num = this.StartDeathAnim(VariantUtils.ConvertTo<bool>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = VariantUtils.CreateFrom<float>(ref num);
      return true;
    }
    if (StringName.op_Equality(ref method, NCreature.MethodName.StartReviveAnim) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.StartReviveAnim();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCreature.MethodName.AnimTempRevive) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.AnimTempRevive();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCreature.MethodName.ImmediatelySetIdle) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.ImmediatelySetIdle();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCreature.MethodName.AnimHideIntent) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.AnimHideIntent(VariantUtils.ConvertTo<double>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCreature.MethodName.SetScaleAndHue) && ((NativeVariantPtrArgs) ref args).Count == 2)
    {
      this.SetScaleAndHue(VariantUtils.ConvertTo<float>(ref ((NativeVariantPtrArgs) ref args)[0]), VariantUtils.ConvertTo<float>(ref ((NativeVariantPtrArgs) ref args)[1]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCreature.MethodName.ScaleTo) && ((NativeVariantPtrArgs) ref args).Count == 2)
    {
      this.ScaleTo(VariantUtils.ConvertTo<float>(ref ((NativeVariantPtrArgs) ref args)[0]), VariantUtils.ConvertTo<double>(ref ((NativeVariantPtrArgs) ref args)[1]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCreature.MethodName.SetDefaultScaleTo) && ((NativeVariantPtrArgs) ref args).Count == 2)
    {
      this.SetDefaultScaleTo(VariantUtils.ConvertTo<float>(ref ((NativeVariantPtrArgs) ref args)[0]), VariantUtils.ConvertTo<float>(ref ((NativeVariantPtrArgs) ref args)[1]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCreature.MethodName.OstyScaleToSize) && ((NativeVariantPtrArgs) ref args).Count == 2)
    {
      this.OstyScaleToSize(VariantUtils.ConvertTo<float>(ref ((NativeVariantPtrArgs) ref args)[0]), VariantUtils.ConvertTo<double>(ref ((NativeVariantPtrArgs) ref args)[1]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCreature.MethodName.AnimShake) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.AnimShake();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCreature.MethodName.DoScaleTween) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.DoScaleTween(VariantUtils.ConvertTo<Vector2>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCreature.MethodName.SetOrbManagerPosition) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.SetOrbManagerPosition();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCreature.MethodName.GetTopOfHitbox) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      Vector2 topOfHitbox = this.GetTopOfHitbox();
      ret = VariantUtils.CreateFrom<Vector2>(ref topOfHitbox);
      return true;
    }
    if (StringName.op_Equality(ref method, NCreature.MethodName.GetBottomOfHitbox) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      Vector2 bottomOfHitbox = this.GetBottomOfHitbox();
      ret = VariantUtils.CreateFrom<Vector2>(ref bottomOfHitbox);
      return true;
    }
    if (StringName.op_Equality(ref method, NCreature.MethodName.ShowMultiselectReticle) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.ShowMultiselectReticle();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCreature.MethodName.HideMultiselectReticle) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.HideMultiselectReticle();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCreature.MethodName.ShowSingleSelectReticle) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.ShowSingleSelectReticle();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCreature.MethodName.HideSingleSelectReticle) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.HideSingleSelectReticle();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCreature.MethodName.SetupForBestiary) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.SetupForBestiary();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCreature.MethodName.StartSfxLoop) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.StartSfxLoop(VariantUtils.ConvertTo<string>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCreature.MethodName.StartSfxLoop) && ((NativeVariantPtrArgs) ref args).Count == 3)
    {
      this.StartSfxLoop(VariantUtils.ConvertTo<string>(ref ((NativeVariantPtrArgs) ref args)[0]), VariantUtils.ConvertTo<string>(ref ((NativeVariantPtrArgs) ref args)[1]), VariantUtils.ConvertTo<float>(ref ((NativeVariantPtrArgs) ref args)[2]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCreature.MethodName.StopSfxLoop) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.StopSfxLoop(VariantUtils.ConvertTo<string>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NCreature.MethodName.StopAllSfxLoops) || ((NativeVariantPtrArgs) ref args).Count != 0)
      return base.InvokeGodotClassMethod(ref method, args, ref ret);
    this.StopAllSfxLoops();
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NCreature.MethodName._Ready) || StringName.op_Equality(ref method, NCreature.MethodName._EnterTree) || StringName.op_Equality(ref method, NCreature.MethodName._ExitTree) || StringName.op_Equality(ref method, NCreature.MethodName.ConnectSpineAnimatorSignals) || StringName.op_Equality(ref method, NCreature.MethodName.UpdateBounds) || StringName.op_Equality(ref method, NCreature.MethodName.UpdatePhobiaMode) || StringName.op_Equality(ref method, NCreature.MethodName.UpdateNavigation) || StringName.op_Equality(ref method, NCreature.MethodName.OnFocus) || StringName.op_Equality(ref method, NCreature.MethodName.OnUnfocus) || StringName.op_Equality(ref method, NCreature.MethodName.OnTargetingStarted) || StringName.op_Equality(ref method, NCreature.MethodName.SetRemotePlayerFocused) || StringName.op_Equality(ref method, NCreature.MethodName.HideHoverTips) || StringName.op_Equality(ref method, NCreature.MethodName.SetAnimationTrigger) || StringName.op_Equality(ref method, NCreature.MethodName.GetCurrentAnimationLength) || StringName.op_Equality(ref method, NCreature.MethodName.GetCurrentAnimationTimeRemaining) || StringName.op_Equality(ref method, NCreature.MethodName.ToggleIsInteractable) || StringName.op_Equality(ref method, NCreature.MethodName.DisableInteractionForDeath) || StringName.op_Equality(ref method, NCreature.MethodName.AnimDisableUi) || StringName.op_Equality(ref method, NCreature.MethodName.AnimEnableUi) || StringName.op_Equality(ref method, NCreature.MethodName.StartDeathAnim) || StringName.op_Equality(ref method, NCreature.MethodName.StartReviveAnim) || StringName.op_Equality(ref method, NCreature.MethodName.AnimTempRevive) || StringName.op_Equality(ref method, NCreature.MethodName.ImmediatelySetIdle) || StringName.op_Equality(ref method, NCreature.MethodName.AnimHideIntent) || StringName.op_Equality(ref method, NCreature.MethodName.SetScaleAndHue) || StringName.op_Equality(ref method, NCreature.MethodName.ScaleTo) || StringName.op_Equality(ref method, NCreature.MethodName.SetDefaultScaleTo) || StringName.op_Equality(ref method, NCreature.MethodName.OstyScaleToSize) || StringName.op_Equality(ref method, NCreature.MethodName.AnimShake) || StringName.op_Equality(ref method, NCreature.MethodName.DoScaleTween) || StringName.op_Equality(ref method, NCreature.MethodName.SetOrbManagerPosition) || StringName.op_Equality(ref method, NCreature.MethodName.GetTopOfHitbox) || StringName.op_Equality(ref method, NCreature.MethodName.GetBottomOfHitbox) || StringName.op_Equality(ref method, NCreature.MethodName.ShowMultiselectReticle) || StringName.op_Equality(ref method, NCreature.MethodName.HideMultiselectReticle) || StringName.op_Equality(ref method, NCreature.MethodName.ShowSingleSelectReticle) || StringName.op_Equality(ref method, NCreature.MethodName.HideSingleSelectReticle) || StringName.op_Equality(ref method, NCreature.MethodName.SetupForBestiary) || StringName.op_Equality(ref method, NCreature.MethodName.StartSfxLoop) || StringName.op_Equality(ref method, NCreature.MethodName.StopSfxLoop) || StringName.op_Equality(ref method, NCreature.MethodName.StopAllSfxLoops) || base.HasGodotClassMethod(ref method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NCreature.PropertyName.Hitbox))
    {
      this.Hitbox = VariantUtils.ConvertTo<Control>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCreature.PropertyName.OrbManager))
    {
      this.OrbManager = VariantUtils.ConvertTo<NOrbManager>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCreature.PropertyName.IsInteractable))
    {
      this.IsInteractable = VariantUtils.ConvertTo<bool>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCreature.PropertyName.Visuals))
    {
      this.Visuals = VariantUtils.ConvertTo<NCreatureVisuals>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCreature.PropertyName.IntentContainer))
    {
      this.IntentContainer = VariantUtils.ConvertTo<Control>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCreature.PropertyName.IsFocused))
    {
      this.IsFocused = VariantUtils.ConvertTo<bool>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCreature.PropertyName.PlayerIntentHandler))
    {
      this.PlayerIntentHandler = VariantUtils.ConvertTo<NMultiplayerPlayerIntentHandler>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCreature.PropertyName._stateDisplay))
    {
      this._stateDisplay = VariantUtils.ConvertTo<NCreatureStateDisplay>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCreature.PropertyName._intentFadeTween))
    {
      this._intentFadeTween = VariantUtils.ConvertTo<Tween>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCreature.PropertyName._shakeTween))
    {
      this._shakeTween = VariantUtils.ConvertTo<Tween>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCreature.PropertyName._isRemotePlayerOrPet))
    {
      this._isRemotePlayerOrPet = VariantUtils.ConvertTo<bool>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCreature.PropertyName._isInBestiary))
    {
      this._isInBestiary = VariantUtils.ConvertTo<bool>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCreature.PropertyName._tempScale))
    {
      this._tempScale = VariantUtils.ConvertTo<float>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCreature.PropertyName._scaleTween))
    {
      this._scaleTween = VariantUtils.ConvertTo<Tween>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCreature.PropertyName._isInMultiselect))
    {
      this._isInMultiselect = VariantUtils.ConvertTo<bool>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NCreature.PropertyName._selectionReticle))
      return ((GodotObject) this).SetGodotClassPropertyValue(ref name, ref value);
    this._selectionReticle = VariantUtils.ConvertTo<NSelectionReticle>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NCreature.PropertyName.Hitbox))
    {
      ref godot_variant local = ref value;
      Control hitbox = this.Hitbox;
      godot_variant from = VariantUtils.CreateFrom<Control>(ref hitbox);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NCreature.PropertyName.OrbManager))
    {
      ref godot_variant local = ref value;
      NOrbManager orbManager = this.OrbManager;
      godot_variant from = VariantUtils.CreateFrom<NOrbManager>(ref orbManager);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NCreature.PropertyName.IsInteractable))
    {
      ref godot_variant local = ref value;
      bool isInteractable = this.IsInteractable;
      godot_variant from = VariantUtils.CreateFrom<bool>(ref isInteractable);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NCreature.PropertyName.VfxSpawnPosition))
    {
      ref godot_variant local = ref value;
      Vector2 vfxSpawnPosition = this.VfxSpawnPosition;
      godot_variant from = VariantUtils.CreateFrom<Vector2>(ref vfxSpawnPosition);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NCreature.PropertyName.PowerAppliedVfxSpawnPosition))
    {
      ref godot_variant local = ref value;
      Vector2 vfxSpawnPosition = this.PowerAppliedVfxSpawnPosition;
      godot_variant from = VariantUtils.CreateFrom<Vector2>(ref vfxSpawnPosition);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NCreature.PropertyName.Visuals))
    {
      ref godot_variant local = ref value;
      NCreatureVisuals visuals = this.Visuals;
      godot_variant from = VariantUtils.CreateFrom<NCreatureVisuals>(ref visuals);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NCreature.PropertyName.Body))
    {
      ref godot_variant local = ref value;
      Node2D body = this.Body;
      godot_variant from = VariantUtils.CreateFrom<Node2D>(ref body);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NCreature.PropertyName.IntentContainer))
    {
      ref godot_variant local = ref value;
      Control intentContainer = this.IntentContainer;
      godot_variant from = VariantUtils.CreateFrom<Control>(ref intentContainer);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NCreature.PropertyName.IsPlayingDeathAnimation))
    {
      ref godot_variant local = ref value;
      bool playingDeathAnimation = this.IsPlayingDeathAnimation;
      godot_variant from = VariantUtils.CreateFrom<bool>(ref playingDeathAnimation);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NCreature.PropertyName.HasSpineAnimation))
    {
      ref godot_variant local = ref value;
      bool hasSpineAnimation = this.HasSpineAnimation;
      godot_variant from = VariantUtils.CreateFrom<bool>(ref hasSpineAnimation);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NCreature.PropertyName.IsFocused))
    {
      ref godot_variant local = ref value;
      bool isFocused = this.IsFocused;
      godot_variant from = VariantUtils.CreateFrom<bool>(ref isFocused);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NCreature.PropertyName.PlayerIntentHandler))
    {
      ref godot_variant local = ref value;
      NMultiplayerPlayerIntentHandler playerIntentHandler = this.PlayerIntentHandler;
      godot_variant from = VariantUtils.CreateFrom<NMultiplayerPlayerIntentHandler>(ref playerIntentHandler);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NCreature.PropertyName._stateDisplay))
    {
      value = VariantUtils.CreateFrom<NCreatureStateDisplay>(ref this._stateDisplay);
      return true;
    }
    if (StringName.op_Equality(ref name, NCreature.PropertyName._intentFadeTween))
    {
      value = VariantUtils.CreateFrom<Tween>(ref this._intentFadeTween);
      return true;
    }
    if (StringName.op_Equality(ref name, NCreature.PropertyName._shakeTween))
    {
      value = VariantUtils.CreateFrom<Tween>(ref this._shakeTween);
      return true;
    }
    if (StringName.op_Equality(ref name, NCreature.PropertyName._isRemotePlayerOrPet))
    {
      value = VariantUtils.CreateFrom<bool>(ref this._isRemotePlayerOrPet);
      return true;
    }
    if (StringName.op_Equality(ref name, NCreature.PropertyName._isInBestiary))
    {
      value = VariantUtils.CreateFrom<bool>(ref this._isInBestiary);
      return true;
    }
    if (StringName.op_Equality(ref name, NCreature.PropertyName._tempScale))
    {
      value = VariantUtils.CreateFrom<float>(ref this._tempScale);
      return true;
    }
    if (StringName.op_Equality(ref name, NCreature.PropertyName._scaleTween))
    {
      value = VariantUtils.CreateFrom<Tween>(ref this._scaleTween);
      return true;
    }
    if (StringName.op_Equality(ref name, NCreature.PropertyName._isInMultiselect))
    {
      value = VariantUtils.CreateFrom<bool>(ref this._isInMultiselect);
      return true;
    }
    if (!StringName.op_Equality(ref name, NCreature.PropertyName._selectionReticle))
      return ((GodotObject) this).GetGodotClassPropertyValue(ref name, ref value);
    value = VariantUtils.CreateFrom<NSelectionReticle>(ref this._selectionReticle);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NCreature.PropertyName._stateDisplay, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NCreature.PropertyName._intentFadeTween, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NCreature.PropertyName._shakeTween, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 1L, NCreature.PropertyName._isRemotePlayerOrPet, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 1L, NCreature.PropertyName._isInBestiary, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 3L, NCreature.PropertyName._tempScale, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NCreature.PropertyName._scaleTween, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NCreature.PropertyName.Hitbox, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NCreature.PropertyName.OrbManager, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 1L, NCreature.PropertyName._isInMultiselect, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NCreature.PropertyName._selectionReticle, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 1L, NCreature.PropertyName.IsInteractable, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 5L, NCreature.PropertyName.VfxSpawnPosition, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 5L, NCreature.PropertyName.PowerAppliedVfxSpawnPosition, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NCreature.PropertyName.Visuals, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NCreature.PropertyName.Body, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NCreature.PropertyName.IntentContainer, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 1L, NCreature.PropertyName.IsPlayingDeathAnimation, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 1L, NCreature.PropertyName.HasSpineAnimation, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 1L, NCreature.PropertyName.IsFocused, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NCreature.PropertyName.PlayerIntentHandler, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).SaveGodotObjectData(info);
    GodotSerializationInfo serializationInfo1 = info;
    StringName hitbox1 = NCreature.PropertyName.Hitbox;
    Control hitbox2 = this.Hitbox;
    Variant variant1 = Variant.From<Control>(ref hitbox2);
    serializationInfo1.AddProperty(hitbox1, variant1);
    GodotSerializationInfo serializationInfo2 = info;
    StringName orbManager1 = NCreature.PropertyName.OrbManager;
    NOrbManager orbManager2 = this.OrbManager;
    Variant variant2 = Variant.From<NOrbManager>(ref orbManager2);
    serializationInfo2.AddProperty(orbManager1, variant2);
    GodotSerializationInfo serializationInfo3 = info;
    StringName isInteractable1 = NCreature.PropertyName.IsInteractable;
    bool isInteractable2 = this.IsInteractable;
    Variant variant3 = Variant.From<bool>(ref isInteractable2);
    serializationInfo3.AddProperty(isInteractable1, variant3);
    GodotSerializationInfo serializationInfo4 = info;
    StringName visuals1 = NCreature.PropertyName.Visuals;
    NCreatureVisuals visuals2 = this.Visuals;
    Variant variant4 = Variant.From<NCreatureVisuals>(ref visuals2);
    serializationInfo4.AddProperty(visuals1, variant4);
    GodotSerializationInfo serializationInfo5 = info;
    StringName intentContainer1 = NCreature.PropertyName.IntentContainer;
    Control intentContainer2 = this.IntentContainer;
    Variant variant5 = Variant.From<Control>(ref intentContainer2);
    serializationInfo5.AddProperty(intentContainer1, variant5);
    GodotSerializationInfo serializationInfo6 = info;
    StringName isFocused1 = NCreature.PropertyName.IsFocused;
    bool isFocused2 = this.IsFocused;
    Variant variant6 = Variant.From<bool>(ref isFocused2);
    serializationInfo6.AddProperty(isFocused1, variant6);
    GodotSerializationInfo serializationInfo7 = info;
    StringName playerIntentHandler1 = NCreature.PropertyName.PlayerIntentHandler;
    NMultiplayerPlayerIntentHandler playerIntentHandler2 = this.PlayerIntentHandler;
    Variant variant7 = Variant.From<NMultiplayerPlayerIntentHandler>(ref playerIntentHandler2);
    serializationInfo7.AddProperty(playerIntentHandler1, variant7);
    info.AddProperty(NCreature.PropertyName._stateDisplay, Variant.From<NCreatureStateDisplay>(ref this._stateDisplay));
    info.AddProperty(NCreature.PropertyName._intentFadeTween, Variant.From<Tween>(ref this._intentFadeTween));
    info.AddProperty(NCreature.PropertyName._shakeTween, Variant.From<Tween>(ref this._shakeTween));
    info.AddProperty(NCreature.PropertyName._isRemotePlayerOrPet, Variant.From<bool>(ref this._isRemotePlayerOrPet));
    info.AddProperty(NCreature.PropertyName._isInBestiary, Variant.From<bool>(ref this._isInBestiary));
    info.AddProperty(NCreature.PropertyName._tempScale, Variant.From<float>(ref this._tempScale));
    info.AddProperty(NCreature.PropertyName._scaleTween, Variant.From<Tween>(ref this._scaleTween));
    info.AddProperty(NCreature.PropertyName._isInMultiselect, Variant.From<bool>(ref this._isInMultiselect));
    info.AddProperty(NCreature.PropertyName._selectionReticle, Variant.From<NSelectionReticle>(ref this._selectionReticle));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NCreature.PropertyName.Hitbox, ref variant1))
      this.Hitbox = ((Variant) ref variant1).As<Control>();
    Variant variant2;
    if (info.TryGetProperty(NCreature.PropertyName.OrbManager, ref variant2))
      this.OrbManager = ((Variant) ref variant2).As<NOrbManager>();
    Variant variant3;
    if (info.TryGetProperty(NCreature.PropertyName.IsInteractable, ref variant3))
      this.IsInteractable = ((Variant) ref variant3).As<bool>();
    Variant variant4;
    if (info.TryGetProperty(NCreature.PropertyName.Visuals, ref variant4))
      this.Visuals = ((Variant) ref variant4).As<NCreatureVisuals>();
    Variant variant5;
    if (info.TryGetProperty(NCreature.PropertyName.IntentContainer, ref variant5))
      this.IntentContainer = ((Variant) ref variant5).As<Control>();
    Variant variant6;
    if (info.TryGetProperty(NCreature.PropertyName.IsFocused, ref variant6))
      this.IsFocused = ((Variant) ref variant6).As<bool>();
    Variant variant7;
    if (info.TryGetProperty(NCreature.PropertyName.PlayerIntentHandler, ref variant7))
      this.PlayerIntentHandler = ((Variant) ref variant7).As<NMultiplayerPlayerIntentHandler>();
    Variant variant8;
    if (info.TryGetProperty(NCreature.PropertyName._stateDisplay, ref variant8))
      this._stateDisplay = ((Variant) ref variant8).As<NCreatureStateDisplay>();
    Variant variant9;
    if (info.TryGetProperty(NCreature.PropertyName._intentFadeTween, ref variant9))
      this._intentFadeTween = ((Variant) ref variant9).As<Tween>();
    Variant variant10;
    if (info.TryGetProperty(NCreature.PropertyName._shakeTween, ref variant10))
      this._shakeTween = ((Variant) ref variant10).As<Tween>();
    Variant variant11;
    if (info.TryGetProperty(NCreature.PropertyName._isRemotePlayerOrPet, ref variant11))
      this._isRemotePlayerOrPet = ((Variant) ref variant11).As<bool>();
    Variant variant12;
    if (info.TryGetProperty(NCreature.PropertyName._isInBestiary, ref variant12))
      this._isInBestiary = ((Variant) ref variant12).As<bool>();
    Variant variant13;
    if (info.TryGetProperty(NCreature.PropertyName._tempScale, ref variant13))
      this._tempScale = ((Variant) ref variant13).As<float>();
    Variant variant14;
    if (info.TryGetProperty(NCreature.PropertyName._scaleTween, ref variant14))
      this._scaleTween = ((Variant) ref variant14).As<Tween>();
    Variant variant15;
    if (info.TryGetProperty(NCreature.PropertyName._isInMultiselect, ref variant15))
      this._isInMultiselect = ((Variant) ref variant15).As<bool>();
    Variant variant16;
    if (!info.TryGetProperty(NCreature.PropertyName._selectionReticle, ref variant16))
      return;
    this._selectionReticle = ((Variant) ref variant16).As<NSelectionReticle>();
  }

  public class MethodName : Control.MethodName
  {
    public static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName _EnterTree = StringName.op_Implicit(nameof (_EnterTree));
    public static readonly StringName _ExitTree = StringName.op_Implicit(nameof (_ExitTree));
    public static readonly StringName ConnectSpineAnimatorSignals = StringName.op_Implicit(nameof (ConnectSpineAnimatorSignals));
    public static readonly StringName UpdateBounds = StringName.op_Implicit(nameof (UpdateBounds));
    public static readonly StringName UpdatePhobiaMode = StringName.op_Implicit(nameof (UpdatePhobiaMode));
    public static readonly StringName UpdateNavigation = StringName.op_Implicit(nameof (UpdateNavigation));
    public static readonly StringName OnFocus = StringName.op_Implicit(nameof (OnFocus));
    public static readonly StringName OnUnfocus = StringName.op_Implicit(nameof (OnUnfocus));
    public static readonly StringName OnTargetingStarted = StringName.op_Implicit(nameof (OnTargetingStarted));
    public static readonly StringName SetRemotePlayerFocused = StringName.op_Implicit(nameof (SetRemotePlayerFocused));
    public static readonly StringName HideHoverTips = StringName.op_Implicit(nameof (HideHoverTips));
    public static readonly StringName SetAnimationTrigger = StringName.op_Implicit(nameof (SetAnimationTrigger));
    public static readonly StringName GetCurrentAnimationLength = StringName.op_Implicit(nameof (GetCurrentAnimationLength));
    public static readonly StringName GetCurrentAnimationTimeRemaining = StringName.op_Implicit(nameof (GetCurrentAnimationTimeRemaining));
    public static readonly StringName ToggleIsInteractable = StringName.op_Implicit(nameof (ToggleIsInteractable));
    public static readonly StringName DisableInteractionForDeath = StringName.op_Implicit(nameof (DisableInteractionForDeath));
    public static readonly StringName AnimDisableUi = StringName.op_Implicit(nameof (AnimDisableUi));
    public static readonly StringName AnimEnableUi = StringName.op_Implicit(nameof (AnimEnableUi));
    public static readonly StringName StartDeathAnim = StringName.op_Implicit(nameof (StartDeathAnim));
    public static readonly StringName StartReviveAnim = StringName.op_Implicit(nameof (StartReviveAnim));
    public static readonly StringName AnimTempRevive = StringName.op_Implicit(nameof (AnimTempRevive));
    public static readonly StringName ImmediatelySetIdle = StringName.op_Implicit(nameof (ImmediatelySetIdle));
    public static readonly StringName AnimHideIntent = StringName.op_Implicit(nameof (AnimHideIntent));
    public static readonly StringName SetScaleAndHue = StringName.op_Implicit(nameof (SetScaleAndHue));
    public static readonly StringName ScaleTo = StringName.op_Implicit(nameof (ScaleTo));
    public static readonly StringName SetDefaultScaleTo = StringName.op_Implicit(nameof (SetDefaultScaleTo));
    public static readonly StringName OstyScaleToSize = StringName.op_Implicit(nameof (OstyScaleToSize));
    public static readonly StringName AnimShake = StringName.op_Implicit(nameof (AnimShake));
    public static readonly StringName DoScaleTween = StringName.op_Implicit(nameof (DoScaleTween));
    public static readonly StringName SetOrbManagerPosition = StringName.op_Implicit(nameof (SetOrbManagerPosition));
    public static readonly StringName GetTopOfHitbox = StringName.op_Implicit(nameof (GetTopOfHitbox));
    public static readonly StringName GetBottomOfHitbox = StringName.op_Implicit(nameof (GetBottomOfHitbox));
    public static readonly StringName ShowMultiselectReticle = StringName.op_Implicit(nameof (ShowMultiselectReticle));
    public static readonly StringName HideMultiselectReticle = StringName.op_Implicit(nameof (HideMultiselectReticle));
    public static readonly StringName ShowSingleSelectReticle = StringName.op_Implicit(nameof (ShowSingleSelectReticle));
    public static readonly StringName HideSingleSelectReticle = StringName.op_Implicit(nameof (HideSingleSelectReticle));
    public static readonly StringName SetupForBestiary = StringName.op_Implicit(nameof (SetupForBestiary));
    public static readonly StringName StartSfxLoop = StringName.op_Implicit(nameof (StartSfxLoop));
    public static readonly StringName StopSfxLoop = StringName.op_Implicit(nameof (StopSfxLoop));
    public static readonly StringName StopAllSfxLoops = StringName.op_Implicit(nameof (StopAllSfxLoops));
  }

  public class PropertyName : Control.PropertyName
  {
    public static readonly StringName Hitbox = StringName.op_Implicit(nameof (Hitbox));
    public static readonly StringName OrbManager = StringName.op_Implicit(nameof (OrbManager));
    public static readonly StringName IsInteractable = StringName.op_Implicit(nameof (IsInteractable));
    public static readonly StringName VfxSpawnPosition = StringName.op_Implicit(nameof (VfxSpawnPosition));
    public static readonly StringName PowerAppliedVfxSpawnPosition = StringName.op_Implicit(nameof (PowerAppliedVfxSpawnPosition));
    public static readonly StringName Visuals = StringName.op_Implicit(nameof (Visuals));
    public static readonly StringName Body = StringName.op_Implicit(nameof (Body));
    public static readonly StringName IntentContainer = StringName.op_Implicit(nameof (IntentContainer));
    public static readonly StringName IsPlayingDeathAnimation = StringName.op_Implicit(nameof (IsPlayingDeathAnimation));
    public static readonly StringName HasSpineAnimation = StringName.op_Implicit(nameof (HasSpineAnimation));
    public static readonly StringName IsFocused = StringName.op_Implicit(nameof (IsFocused));
    public static readonly StringName PlayerIntentHandler = StringName.op_Implicit(nameof (PlayerIntentHandler));
    public static readonly StringName _stateDisplay = StringName.op_Implicit(nameof (_stateDisplay));
    public static readonly StringName _intentFadeTween = StringName.op_Implicit(nameof (_intentFadeTween));
    public static readonly StringName _shakeTween = StringName.op_Implicit(nameof (_shakeTween));
    public static readonly StringName _isRemotePlayerOrPet = StringName.op_Implicit(nameof (_isRemotePlayerOrPet));
    public static readonly StringName _isInBestiary = StringName.op_Implicit(nameof (_isInBestiary));
    public static readonly StringName _tempScale = StringName.op_Implicit(nameof (_tempScale));
    public static readonly StringName _scaleTween = StringName.op_Implicit(nameof (_scaleTween));
    public static readonly StringName _isInMultiselect = StringName.op_Implicit(nameof (_isInMultiselect));
    public static readonly StringName _selectionReticle = StringName.op_Implicit(nameof (_selectionReticle));
  }

  public class SignalName : Control.SignalName
  {
  }
}
