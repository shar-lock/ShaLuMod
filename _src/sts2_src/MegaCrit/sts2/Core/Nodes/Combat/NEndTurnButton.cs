// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Combat.NEndTurnButton
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.ControllerInput;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Nodes.Ftue;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.HoverTips;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Screens.ScreenContext;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.Saves;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Combat;

[ScriptPath("res://src/Core/Nodes/Combat/NEndTurnButton.cs")]
public class NEndTurnButton : NButton
{
  private static readonly StringName _v = new StringName("v");
  private const float _flyInOutDuration = 0.5f;
  private static readonly LocString _endTurnLoc = new LocString("gameplay_ui", "END_TURN_BUTTON");
  private CombatState? _combatState;
  private CardPile? _playerHand;
  private NEndTurnButton.State _state = NEndTurnButton.State.Hidden;
  private bool _isShiny;
  private Control _visuals;
  private Texture2D _glowTexture;
  private Texture2D _normalTexture;
  private TextureRect _image;
  private ShaderMaterial _hsv;
  private Control _glow;
  private Control _glowVfx;
  private MegaLabel _label;
  private NCombatUi _combatUi;
  private Viewport _viewport;
  private NMultiplayerVoteContainer _playerIconContainer;
  private NEndTurnLongPressBar _longPressBar;
  private float _pulseTimer = 1f;
  private static readonly Vector2 _hoverTipOffset = new Vector2(-76f, -302f);
  private static readonly Vector2 _showPosRatio = Vector2.op_Division(new Vector2(1604f, 846f), NGame.devResolution);
  private static readonly Vector2 _hidePosRatio = Vector2.op_Addition(NEndTurnButton._showPosRatio, Vector2.op_Division(new Vector2(0.0f, 250f), NGame.devResolution));
  private Tween? _positionTween;
  private Tween? _hoverTween;
  private Tween? _glowVfxTween;
  private Tween? _glowEnableTween;
  private const int _ftueDisableEndTurnCount = 3;
  private int _endTurnWithNoPlayableCardsCount;

  private static string EndTurnButtonPath => "res://images/packed/combat_ui/end_turn_button.png";

  private static string EndTurnButtonGlowPath
  {
    get => "res://images/packed/combat_ui/end_turn_button_glow.png";
  }

  public static IEnumerable<string> AssetPaths
  {
    get
    {
      List<string> items = new List<string>();
      items.Add(NEndTurnButton.EndTurnButtonPath);
      items.Add(NEndTurnButton.EndTurnButtonGlowPath);
      items.AddRange(NMultiplayerVoteContainer.AssetPaths);
      return (IEnumerable<string>) new \u003C\u003Ez__ReadOnlyList<string>(items);
    }
  }

  private bool CanTurnBeEnded
  {
    get
    {
      return !NCombatRoom.Instance.Ui.Hand.InCardPlay && NCombatRoom.Instance.Ui.Hand.CurrentMode == NPlayerHand.Mode.Play;
    }
  }

  private Vector2 ShowPos
  {
    get
    {
      Vector2 showPosRatio = NEndTurnButton._showPosRatio;
      Rect2 visibleRect = this._viewport.GetVisibleRect();
      Vector2 size = ((Rect2) ref visibleRect).Size;
      return Vector2.op_Multiply(showPosRatio, size);
    }
  }

  private Vector2 HidePos
  {
    get
    {
      Vector2 hidePosRatio = NEndTurnButton._hidePosRatio;
      Rect2 visibleRect = this._viewport.GetVisibleRect();
      Vector2 size = ((Rect2) ref visibleRect).Size;
      return Vector2.op_Multiply(hidePosRatio, size);
    }
  }

  protected override string[] Hotkeys
  {
    get
    {
      return new string[1]
      {
        StringName.op_Implicit(MegaInput.accept)
      };
    }
  }

  public override void _Ready()
  {
    this.ConnectSignals();
    this._visuals = ((Node) this).GetNode<Control>(NodePath.op_Implicit("Visuals"));
    this._image = ((Node) this).GetNode<TextureRect>(NodePath.op_Implicit("Visuals/Image"));
    this._hsv = (ShaderMaterial) ((CanvasItem) this._image).Material;
    this._glow = (Control) ((Node) this).GetNode<TextureRect>(NodePath.op_Implicit("Visuals/Glow"));
    this._glowVfx = (Control) ((Node) this).GetNode<TextureRect>(NodePath.op_Implicit("Visuals/GlowVfx"));
    this._label = ((Node) this).GetNode<MegaLabel>(NodePath.op_Implicit("Visuals/Label"));
    this._playerIconContainer = ((Node) this).GetNode<NMultiplayerVoteContainer>(NodePath.op_Implicit("PlayerIconContainer"));
    this._longPressBar = ((Node) this).GetNode<NEndTurnLongPressBar>(NodePath.op_Implicit("%Bar"));
    this._longPressBar.Init(this);
    this.Disable();
    this._combatUi = ((Node) this).GetParent<NCombatUi>();
    this._viewport = ((Node) this).GetViewport();
    this._glowTexture = (Texture2D) PreloadManager.Cache.GetCompressedTexture2D(NEndTurnButton.EndTurnButtonGlowPath);
    this._normalTexture = (Texture2D) PreloadManager.Cache.GetCompressedTexture2D(NEndTurnButton.EndTurnButtonPath);
  }

  public override void _EnterTree()
  {
    base._EnterTree();
    CombatManager.Instance.TurnStarted += new Action<CombatState>(this.OnTurnStarted);
    CombatManager.Instance.AboutToSwitchToEnemyTurn += new Action<CombatState>(this.OnAboutToSwitchToEnemyTurn);
    CombatManager.Instance.PlayerEndedTurn += new Action<Player, bool>(this.AfterPlayerEndedTurn);
    CombatManager.Instance.PlayerUnendedTurn += new Action<Player>(this.AfterPlayerUnendedTurn);
    CombatManager.Instance.StateTracker.CombatStateChanged += new Action<CombatState>(this.OnCombatStateChanged);
  }

  public override void _ExitTree()
  {
    base._ExitTree();
    this._positionTween?.Kill();
    this._hoverTween?.Kill();
    CombatManager.Instance.TurnStarted -= new Action<CombatState>(this.OnTurnStarted);
    CombatManager.Instance.AboutToSwitchToEnemyTurn -= new Action<CombatState>(this.OnAboutToSwitchToEnemyTurn);
    CombatManager.Instance.PlayerEndedTurn -= new Action<Player, bool>(this.AfterPlayerEndedTurn);
    CombatManager.Instance.PlayerUnendedTurn -= new Action<Player>(this.AfterPlayerUnendedTurn);
    CombatManager.Instance.StateTracker.CombatStateChanged -= new Action<CombatState>(this.OnCombatStateChanged);
  }

  public void Initialize(CombatState state)
  {
    this._combatState = state;
    this._playerHand = PileType.Hand.GetPile(LocalContext.GetMe((ICombatState) this._combatState));
    this._playerIconContainer.Initialize(new NMultiplayerVoteContainer.PlayerVotedDelegate(this.ShouldDisplayPlayerIcon), this._combatState.Players);
  }

  private bool ShouldDisplayPlayerIcon(Player player)
  {
    return CombatManager.Instance.IsPlayerReadyToEndTurn(player);
  }

  private bool PlayerCanTakeAction(Player player)
  {
    if (!player.Creature.IsAlive)
      return false;
    return CombatManager.Instance.PlayersTakingExtraTurn.Count == 0 || CombatManager.Instance.PlayersTakingExtraTurn.Contains<Player>(player);
  }

  private void AfterPlayerEndedTurn(Player player, bool canBackOut)
  {
    this._playerIconContainer.RefreshPlayerVotes();
    if (LocalContext.IsMe(player))
    {
      this.StartOrStopPulseVfx();
      Player me = LocalContext.GetMe(player.Creature.CombatState);
      if (((CombatManager.Instance.AllPlayersReadyToEndTurn() ? 0 : (this.PlayerCanTakeAction(me) ? 1 : 0)) & (canBackOut ? 1 : 0)) != 0)
      {
        this.SetState(NEndTurnButton.State.Enabled);
        this._label.SetTextAutoSize(new LocString("gameplay_ui", "UNDO_END_TURN_BUTTON").GetFormattedText());
      }
      else
        this.SetState(NEndTurnButton.State.Disabled);
    }
    if (!CombatManager.Instance.AllPlayersReadyToEndTurn())
      return;
    this.SetState(NEndTurnButton.State.Disabled);
  }

  private void AfterPlayerUnendedTurn(Player player)
  {
    this._playerIconContainer.RefreshPlayerVotes();
    if (!LocalContext.IsMe(player) || !this.PlayerCanTakeAction(player))
      return;
    this.SetState(NEndTurnButton.State.Enabled);
    NEndTurnButton._endTurnLoc.Add("turnNumber", (Decimal) player.PlayerCombatState.TurnNumber);
    this._label.SetTextAutoSize(NEndTurnButton._endTurnLoc.GetFormattedText());
    this.StartOrStopPulseVfx();
  }

  private void OnAboutToSwitchToEnemyTurn(CombatState _)
  {
    this.SetState(NEndTurnButton.State.Hidden);
  }

  private void OnTurnStarted(CombatState state)
  {
    if (state.CurrentSide != CombatSide.Player || !CombatManager.Instance.IsInProgress)
      return;
    this._playerIconContainer.RefreshPlayerVotes(false);
    Player me = LocalContext.GetMe((ICombatState) state);
    NEndTurnButton._endTurnLoc.Add("turnNumber", (Decimal) me.PlayerCombatState.TurnNumber);
    this._label.SetTextAutoSize(NEndTurnButton._endTurnLoc.GetFormattedText());
    if (this.PlayerCanTakeAction(me))
    {
      this.SetState(NEndTurnButton.State.Enabled);
    }
    else
    {
      this.AnimIn();
      this.SetState(NEndTurnButton.State.Disabled);
    }
  }

  private void OnCombatStateChanged(CombatState combatState) => this.StartOrStopPulseVfx();

  private void StartOrStopPulseVfx()
  {
    bool flag = !this.HasPlayableCard() && !CombatManager.Instance.IsPlayerReadyToEndTurn(LocalContext.GetMe((ICombatState) this._combatState)) && this._state == NEndTurnButton.State.Enabled;
    if (this._isShiny)
    {
      if (flag)
        return;
      this._isShiny = false;
      this._glowEnableTween?.Kill();
      this._glowEnableTween = ((Node) this).CreateTween().SetParallel(true);
      this._glowEnableTween.TweenProperty((GodotObject) this._glow, NodePath.op_Implicit("modulate:a"), Variant.op_Implicit(0.0f), 0.5).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 5L);
      this._glowVfxTween?.Kill();
      this._glowVfxTween = ((Node) this).CreateTween();
      this._glowVfxTween.TweenProperty((GodotObject) this._glowVfx, NodePath.op_Implicit("modulate:a"), Variant.op_Implicit(0.0f), 0.5).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 5L);
    }
    else
    {
      if (!flag)
        return;
      this._isShiny = true;
      this._glowVfxTween?.Kill();
      this.GlowPulse();
      this._glowEnableTween?.Kill();
      this._glowEnableTween = ((Node) this).CreateTween().SetParallel(true);
      this._glowEnableTween.TweenProperty((GodotObject) this._glow, NodePath.op_Implicit("modulate:a"), Variant.op_Implicit(0.75f), 0.8).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 10L);
      this._glowEnableTween.TweenProperty((GodotObject) this._glow, NodePath.op_Implicit("scale"), Variant.op_Implicit(Vector2.op_Multiply(Vector2.One, 0.5f)), 0.8).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 10L).From(Variant.op_Implicit(Vector2.op_Multiply(Vector2.One, 0.45f)));
    }
  }

  private void GlowPulse()
  {
    this._glowVfxTween = ((Node) this).CreateTween().SetParallel(true).SetLoops(0);
    this._glowVfxTween.TweenProperty((GodotObject) this._glowVfx, NodePath.op_Implicit("scale"), Variant.op_Implicit(Vector2.op_Multiply(Vector2.One, 0.7f)), 1.5).From(Variant.op_Implicit(Vector2.op_Multiply(Vector2.One, 0.5f))).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 3L);
    this._glowVfxTween.TweenProperty((GodotObject) this._glowVfx, NodePath.op_Implicit("modulate:a"), Variant.op_Implicit(0.0f), 1.5).From(Variant.op_Implicit(0.4f));
  }

  protected override void OnRelease()
  {
    if (this.ShouldShowPlayableCardsFtue())
      return;
    if (SaveManager.Instance.PrefsSave.IsLongPressEnabled)
      this._longPressBar.CancelPress();
    else
      this.CallReleaseLogic();
  }

  public void CallReleaseLogic()
  {
    if (!this.CanTurnBeEnded)
      return;
    this._glowEnableTween?.Kill();
    this._glowEnableTween = ((Node) this).CreateTween().SetParallel(true);
    this._glowEnableTween.TweenProperty((GodotObject) this._glow, NodePath.op_Implicit("modulate:a"), Variant.op_Implicit(0.0f), 0.5).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 5L);
    Player me = LocalContext.GetMe((ICombatState) this._combatState);
    int turnNumber = me.PlayerCombatState.TurnNumber;
    if (!CombatManager.Instance.IsPlayerReadyToEndTurn(me))
    {
      this.SetState(NEndTurnButton.State.Disabled);
      RunManager.Instance.ActionQueueSynchronizer.RequestEnqueue((GameAction) new EndPlayerTurnAction(me, turnNumber));
    }
    else
    {
      this.SetState(NEndTurnButton.State.Disabled);
      RunManager.Instance.ActionQueueSynchronizer.RequestEnqueue((GameAction) new UndoEndPlayerTurnAction(me, turnNumber));
    }
  }

  public void SecretEndTurnLogicViaFtue()
  {
    this._glowEnableTween?.Kill();
    this._glowEnableTween = ((Node) this).CreateTween().SetParallel(true);
    this._glowEnableTween.TweenProperty((GodotObject) this._glow, NodePath.op_Implicit("modulate:a"), Variant.op_Implicit(0.0f), 0.5).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 5L);
    Player me = LocalContext.GetMe((ICombatState) this._combatState);
    RunManager.Instance.ActionQueueSynchronizer.RequestEnqueue((GameAction) new EndPlayerTurnAction(me, me.PlayerCombatState.TurnNumber));
  }

  private bool ShouldShowPlayableCardsFtue()
  {
    if (SaveManager.Instance.SeenFtue("can_play_cards_ftue"))
      return false;
    bool play = LocalContext.GetMe((ICombatState) this._combatState).PlayerCombatState.HasCardsToPlay();
    if (play)
    {
      NModalContainer.Instance.Add((Node) NCanPlayCardsFtue.Create());
      SaveManager.Instance.MarkFtueAsComplete("can_play_cards_ftue");
    }
    else
    {
      ++this._endTurnWithNoPlayableCardsCount;
      if (this._endTurnWithNoPlayableCardsCount == 3)
      {
        Log.Info($"Ended {3} turns without cards left to play. Good job! Disabling can_play_cards ftue.");
        SaveManager.Instance.MarkFtueAsComplete("can_play_cards_ftue");
      }
    }
    return play;
  }

  protected override void OnEnable()
  {
    base.OnEnable();
    Tween hoverTween = this._hoverTween;
    if (hoverTween != null)
      hoverTween.FastForwardToCompletion();
    this._image.Texture = this._normalTexture;
    ((CanvasItem) this._image).Modulate = Colors.White;
    ((CanvasItem) this._label).Modulate = StsColors.cream;
  }

  protected override void OnDisable()
  {
    base.OnDisable();
    NHoverTipSet.Remove((Control) this);
    Tween hoverTween = this._hoverTween;
    if (hoverTween != null)
      hoverTween.FastForwardToCompletion();
    ((CanvasItem) this._image).Modulate = StsColors.gray;
    ((CanvasItem) this._label).Modulate = StsColors.gray;
    this.StartOrStopPulseVfx();
  }

  private void AnimOut()
  {
    this._hoverTween?.Kill();
    this._positionTween?.Kill();
    this._positionTween = ((Node) this).CreateTween();
    this._positionTween.TweenProperty((GodotObject) this, NodePath.op_Implicit("position"), Variant.op_Implicit(this.HidePos), 0.5).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 5L);
  }

  private void AnimIn()
  {
    this._positionTween?.Kill();
    this._positionTween = ((Node) this).CreateTween();
    this._positionTween.TweenProperty((GodotObject) this, NodePath.op_Implicit("position"), Variant.op_Implicit(this.ShowPos), 0.5).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 10L);
  }

  public void OnCombatEnded() => this.SetState(NEndTurnButton.State.Hidden);

  protected override void OnFocus()
  {
    base.OnFocus();
    this._hoverTween?.Kill();
    this._hsv.SetShaderParameter(NEndTurnButton._v, Variant.op_Implicit(1.5));
    this._visuals.Position = new Vector2(0.0f, -2f);
    Player me = LocalContext.GetMe((ICombatState) this._combatState);
    if (!CombatManager.Instance.IsPlayerReadyToEndTurn(me))
    {
      ((CanvasItem) this._label).Modulate = me.PlayerCombatState.HasCardsToPlay() ? StsColors.red : Colors.Cyan;
      this._combatUi.Hand.FlashPlayableHolders();
      LocString title = new LocString("static_hover_tips", "END_TURN.title");
      title.Add("Hotkey", NInputManager.Instance.GetShortcutKey(MegaInput.accept).ToString());
      NHoverTipSet.CreateAndShow((Control) this, (IHoverTip) new HoverTip(title, new LocString("static_hover_tips", "END_TURN.description")))?.SetGlobalPosition(Vector2.op_Addition(this.GlobalPosition, NEndTurnButton._hoverTipOffset), false);
    }
    else
      ((CanvasItem) this._label).Modulate = StsColors.cream;
  }

  private bool HasPlayableCard()
  {
    if (this._playerHand == null)
      return false;
    foreach (CardModel card in (IEnumerable<CardModel>) this._playerHand.Cards)
    {
      if (card.CanPlay())
        return true;
    }
    return false;
  }

  protected override void OnUnfocus()
  {
    if (SaveManager.Instance.PrefsSave.IsLongPressEnabled)
      this._longPressBar.CancelPress();
    NHoverTipSet.Remove((Control) this);
    this._hoverTween?.Kill();
    this._hoverTween = ((Node) this).CreateTween().SetParallel(true);
    this._hoverTween.TweenMethod(Callable.From<float>(new Action<float>(this.UpdateShaderV)), this._hsv.GetShaderParameter(NEndTurnButton._v), Variant.op_Implicit(1f), 0.5).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 5L);
    this._hoverTween.TweenProperty((GodotObject) this._visuals, NodePath.op_Implicit("position"), Variant.op_Implicit(Vector2.Zero), 0.5).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 5L);
    this._hoverTween.TweenProperty((GodotObject) this._label, NodePath.op_Implicit("modulate"), Variant.op_Implicit(this.IsEnabled ? StsColors.cream : StsColors.gray), 0.5).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 5L);
  }

  protected override void OnPress()
  {
    if (!this.CanTurnBeEnded)
      return;
    if (SaveManager.Instance.PrefsSave.IsLongPressEnabled)
      this._longPressBar.StartPress();
    this._hoverTween?.Kill();
    this._hoverTween = ((Node) this).CreateTween().SetParallel(true);
    this._hoverTween.TweenMethod(Callable.From<float>(new Action<float>(this.UpdateShaderV)), this._hsv.GetShaderParameter(NEndTurnButton._v), Variant.op_Implicit(1f), 0.5).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 5L);
    this._hoverTween.TweenProperty((GodotObject) this._visuals, NodePath.op_Implicit("position"), Variant.op_Implicit(new Vector2(0.0f, 8f)), 0.5).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 7L);
    this._hoverTween.TweenProperty((GodotObject) this._label, NodePath.op_Implicit("modulate"), Variant.op_Implicit(Colors.DarkGray), 0.5).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 5L);
  }

  private void UpdateShaderV(float value)
  {
    this._hsv.SetShaderParameter(NEndTurnButton._v, Variant.op_Implicit(value));
  }

  private void SetState(NEndTurnButton.State newState)
  {
    if (this._state == newState)
      return;
    if (newState == NEndTurnButton.State.Hidden)
      this.AnimOut();
    if (newState == NEndTurnButton.State.Enabled && this._state == NEndTurnButton.State.Hidden)
      this.AnimIn();
    this._state = newState;
    this.RefreshEnabled();
  }

  public void RefreshEnabled()
  {
    bool flag = NCombatRoom.Instance == null || NCombatRoom.Instance.Mode != CombatRoomMode.ActiveCombat || !ActiveScreenContext.Instance.IsCurrent((IScreenContext) NCombatRoom.Instance) || NCombatRoom.Instance.Ui.Hand.IsInCardSelection;
    if (this._state == NEndTurnButton.State.Enabled && !flag)
      this.Enable();
    else
      this.Disable();
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(21)
    {
      new MethodInfo(NEndTurnButton.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NEndTurnButton.MethodName._EnterTree, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NEndTurnButton.MethodName._ExitTree, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NEndTurnButton.MethodName.StartOrStopPulseVfx, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NEndTurnButton.MethodName.GlowPulse, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NEndTurnButton.MethodName.OnRelease, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NEndTurnButton.MethodName.CallReleaseLogic, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NEndTurnButton.MethodName.SecretEndTurnLogicViaFtue, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NEndTurnButton.MethodName.ShouldShowPlayableCardsFtue, new PropertyInfo((Variant.Type) 1L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NEndTurnButton.MethodName.OnEnable, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NEndTurnButton.MethodName.OnDisable, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NEndTurnButton.MethodName.AnimOut, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NEndTurnButton.MethodName.AnimIn, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NEndTurnButton.MethodName.OnCombatEnded, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NEndTurnButton.MethodName.OnFocus, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NEndTurnButton.MethodName.HasPlayableCard, new PropertyInfo((Variant.Type) 1L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NEndTurnButton.MethodName.OnUnfocus, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NEndTurnButton.MethodName.OnPress, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NEndTurnButton.MethodName.UpdateShaderV, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 3L, StringName.op_Implicit("value"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NEndTurnButton.MethodName.SetState, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 2L, StringName.op_Implicit("newState"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NEndTurnButton.MethodName.RefreshEnabled, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NEndTurnButton.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NEndTurnButton.MethodName._EnterTree) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._EnterTree();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NEndTurnButton.MethodName._ExitTree) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._ExitTree();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NEndTurnButton.MethodName.StartOrStopPulseVfx) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.StartOrStopPulseVfx();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NEndTurnButton.MethodName.GlowPulse) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.GlowPulse();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NEndTurnButton.MethodName.OnRelease) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnRelease();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NEndTurnButton.MethodName.CallReleaseLogic) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.CallReleaseLogic();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NEndTurnButton.MethodName.SecretEndTurnLogicViaFtue) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.SecretEndTurnLogicViaFtue();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NEndTurnButton.MethodName.ShouldShowPlayableCardsFtue) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      bool flag = this.ShouldShowPlayableCardsFtue();
      ret = VariantUtils.CreateFrom<bool>(ref flag);
      return true;
    }
    if (StringName.op_Equality(ref method, NEndTurnButton.MethodName.OnEnable) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnEnable();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NEndTurnButton.MethodName.OnDisable) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnDisable();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NEndTurnButton.MethodName.AnimOut) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.AnimOut();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NEndTurnButton.MethodName.AnimIn) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.AnimIn();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NEndTurnButton.MethodName.OnCombatEnded) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnCombatEnded();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NEndTurnButton.MethodName.OnFocus) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnFocus();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NEndTurnButton.MethodName.HasPlayableCard) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      bool flag = this.HasPlayableCard();
      ret = VariantUtils.CreateFrom<bool>(ref flag);
      return true;
    }
    if (StringName.op_Equality(ref method, NEndTurnButton.MethodName.OnUnfocus) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnUnfocus();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NEndTurnButton.MethodName.OnPress) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnPress();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NEndTurnButton.MethodName.UpdateShaderV) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.UpdateShaderV(VariantUtils.ConvertTo<float>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NEndTurnButton.MethodName.SetState) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.SetState(VariantUtils.ConvertTo<NEndTurnButton.State>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NEndTurnButton.MethodName.RefreshEnabled) || ((NativeVariantPtrArgs) ref args).Count != 0)
      return base.InvokeGodotClassMethod(in method, args, out ret);
    this.RefreshEnabled();
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NEndTurnButton.MethodName._Ready) || StringName.op_Equality(ref method, NEndTurnButton.MethodName._EnterTree) || StringName.op_Equality(ref method, NEndTurnButton.MethodName._ExitTree) || StringName.op_Equality(ref method, NEndTurnButton.MethodName.StartOrStopPulseVfx) || StringName.op_Equality(ref method, NEndTurnButton.MethodName.GlowPulse) || StringName.op_Equality(ref method, NEndTurnButton.MethodName.OnRelease) || StringName.op_Equality(ref method, NEndTurnButton.MethodName.CallReleaseLogic) || StringName.op_Equality(ref method, NEndTurnButton.MethodName.SecretEndTurnLogicViaFtue) || StringName.op_Equality(ref method, NEndTurnButton.MethodName.ShouldShowPlayableCardsFtue) || StringName.op_Equality(ref method, NEndTurnButton.MethodName.OnEnable) || StringName.op_Equality(ref method, NEndTurnButton.MethodName.OnDisable) || StringName.op_Equality(ref method, NEndTurnButton.MethodName.AnimOut) || StringName.op_Equality(ref method, NEndTurnButton.MethodName.AnimIn) || StringName.op_Equality(ref method, NEndTurnButton.MethodName.OnCombatEnded) || StringName.op_Equality(ref method, NEndTurnButton.MethodName.OnFocus) || StringName.op_Equality(ref method, NEndTurnButton.MethodName.HasPlayableCard) || StringName.op_Equality(ref method, NEndTurnButton.MethodName.OnUnfocus) || StringName.op_Equality(ref method, NEndTurnButton.MethodName.OnPress) || StringName.op_Equality(ref method, NEndTurnButton.MethodName.UpdateShaderV) || StringName.op_Equality(ref method, NEndTurnButton.MethodName.SetState) || StringName.op_Equality(ref method, NEndTurnButton.MethodName.RefreshEnabled) || base.HasGodotClassMethod(in method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NEndTurnButton.PropertyName._state))
    {
      this._state = VariantUtils.ConvertTo<NEndTurnButton.State>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NEndTurnButton.PropertyName._isShiny))
    {
      this._isShiny = VariantUtils.ConvertTo<bool>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NEndTurnButton.PropertyName._visuals))
    {
      this._visuals = VariantUtils.ConvertTo<Control>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NEndTurnButton.PropertyName._glowTexture))
    {
      this._glowTexture = VariantUtils.ConvertTo<Texture2D>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NEndTurnButton.PropertyName._normalTexture))
    {
      this._normalTexture = VariantUtils.ConvertTo<Texture2D>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NEndTurnButton.PropertyName._image))
    {
      this._image = VariantUtils.ConvertTo<TextureRect>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NEndTurnButton.PropertyName._hsv))
    {
      this._hsv = VariantUtils.ConvertTo<ShaderMaterial>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NEndTurnButton.PropertyName._glow))
    {
      this._glow = VariantUtils.ConvertTo<Control>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NEndTurnButton.PropertyName._glowVfx))
    {
      this._glowVfx = VariantUtils.ConvertTo<Control>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NEndTurnButton.PropertyName._label))
    {
      this._label = VariantUtils.ConvertTo<MegaLabel>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NEndTurnButton.PropertyName._combatUi))
    {
      this._combatUi = VariantUtils.ConvertTo<NCombatUi>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NEndTurnButton.PropertyName._viewport))
    {
      this._viewport = VariantUtils.ConvertTo<Viewport>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NEndTurnButton.PropertyName._playerIconContainer))
    {
      this._playerIconContainer = VariantUtils.ConvertTo<NMultiplayerVoteContainer>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NEndTurnButton.PropertyName._longPressBar))
    {
      this._longPressBar = VariantUtils.ConvertTo<NEndTurnLongPressBar>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NEndTurnButton.PropertyName._pulseTimer))
    {
      this._pulseTimer = VariantUtils.ConvertTo<float>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NEndTurnButton.PropertyName._positionTween))
    {
      this._positionTween = VariantUtils.ConvertTo<Tween>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NEndTurnButton.PropertyName._hoverTween))
    {
      this._hoverTween = VariantUtils.ConvertTo<Tween>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NEndTurnButton.PropertyName._glowVfxTween))
    {
      this._glowVfxTween = VariantUtils.ConvertTo<Tween>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NEndTurnButton.PropertyName._glowEnableTween))
    {
      this._glowEnableTween = VariantUtils.ConvertTo<Tween>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NEndTurnButton.PropertyName._endTurnWithNoPlayableCardsCount))
      return base.SetGodotClassPropertyValue(in name, in value);
    this._endTurnWithNoPlayableCardsCount = VariantUtils.ConvertTo<int>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NEndTurnButton.PropertyName.CanTurnBeEnded))
    {
      ref godot_variant local = ref value;
      bool canTurnBeEnded = this.CanTurnBeEnded;
      godot_variant from = VariantUtils.CreateFrom<bool>(ref canTurnBeEnded);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NEndTurnButton.PropertyName.ShowPos))
    {
      ref godot_variant local = ref value;
      Vector2 showPos = this.ShowPos;
      godot_variant from = VariantUtils.CreateFrom<Vector2>(ref showPos);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NEndTurnButton.PropertyName.HidePos))
    {
      ref godot_variant local = ref value;
      Vector2 hidePos = this.HidePos;
      godot_variant from = VariantUtils.CreateFrom<Vector2>(ref hidePos);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NEndTurnButton.PropertyName.Hotkeys))
    {
      ref godot_variant local = ref value;
      string[] hotkeys = this.Hotkeys;
      godot_variant from = VariantUtils.CreateFrom<string[]>(ref hotkeys);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NEndTurnButton.PropertyName._state))
    {
      value = VariantUtils.CreateFrom<NEndTurnButton.State>(ref this._state);
      return true;
    }
    if (StringName.op_Equality(ref name, NEndTurnButton.PropertyName._isShiny))
    {
      value = VariantUtils.CreateFrom<bool>(ref this._isShiny);
      return true;
    }
    if (StringName.op_Equality(ref name, NEndTurnButton.PropertyName._visuals))
    {
      value = VariantUtils.CreateFrom<Control>(ref this._visuals);
      return true;
    }
    if (StringName.op_Equality(ref name, NEndTurnButton.PropertyName._glowTexture))
    {
      value = VariantUtils.CreateFrom<Texture2D>(ref this._glowTexture);
      return true;
    }
    if (StringName.op_Equality(ref name, NEndTurnButton.PropertyName._normalTexture))
    {
      value = VariantUtils.CreateFrom<Texture2D>(ref this._normalTexture);
      return true;
    }
    if (StringName.op_Equality(ref name, NEndTurnButton.PropertyName._image))
    {
      value = VariantUtils.CreateFrom<TextureRect>(ref this._image);
      return true;
    }
    if (StringName.op_Equality(ref name, NEndTurnButton.PropertyName._hsv))
    {
      value = VariantUtils.CreateFrom<ShaderMaterial>(ref this._hsv);
      return true;
    }
    if (StringName.op_Equality(ref name, NEndTurnButton.PropertyName._glow))
    {
      value = VariantUtils.CreateFrom<Control>(ref this._glow);
      return true;
    }
    if (StringName.op_Equality(ref name, NEndTurnButton.PropertyName._glowVfx))
    {
      value = VariantUtils.CreateFrom<Control>(ref this._glowVfx);
      return true;
    }
    if (StringName.op_Equality(ref name, NEndTurnButton.PropertyName._label))
    {
      value = VariantUtils.CreateFrom<MegaLabel>(ref this._label);
      return true;
    }
    if (StringName.op_Equality(ref name, NEndTurnButton.PropertyName._combatUi))
    {
      value = VariantUtils.CreateFrom<NCombatUi>(ref this._combatUi);
      return true;
    }
    if (StringName.op_Equality(ref name, NEndTurnButton.PropertyName._viewport))
    {
      value = VariantUtils.CreateFrom<Viewport>(ref this._viewport);
      return true;
    }
    if (StringName.op_Equality(ref name, NEndTurnButton.PropertyName._playerIconContainer))
    {
      value = VariantUtils.CreateFrom<NMultiplayerVoteContainer>(ref this._playerIconContainer);
      return true;
    }
    if (StringName.op_Equality(ref name, NEndTurnButton.PropertyName._longPressBar))
    {
      value = VariantUtils.CreateFrom<NEndTurnLongPressBar>(ref this._longPressBar);
      return true;
    }
    if (StringName.op_Equality(ref name, NEndTurnButton.PropertyName._pulseTimer))
    {
      value = VariantUtils.CreateFrom<float>(ref this._pulseTimer);
      return true;
    }
    if (StringName.op_Equality(ref name, NEndTurnButton.PropertyName._positionTween))
    {
      value = VariantUtils.CreateFrom<Tween>(ref this._positionTween);
      return true;
    }
    if (StringName.op_Equality(ref name, NEndTurnButton.PropertyName._hoverTween))
    {
      value = VariantUtils.CreateFrom<Tween>(ref this._hoverTween);
      return true;
    }
    if (StringName.op_Equality(ref name, NEndTurnButton.PropertyName._glowVfxTween))
    {
      value = VariantUtils.CreateFrom<Tween>(ref this._glowVfxTween);
      return true;
    }
    if (StringName.op_Equality(ref name, NEndTurnButton.PropertyName._glowEnableTween))
    {
      value = VariantUtils.CreateFrom<Tween>(ref this._glowEnableTween);
      return true;
    }
    if (!StringName.op_Equality(ref name, NEndTurnButton.PropertyName._endTurnWithNoPlayableCardsCount))
      return base.GetGodotClassPropertyValue(in name, out value);
    value = VariantUtils.CreateFrom<int>(ref this._endTurnWithNoPlayableCardsCount);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 1L, NEndTurnButton.PropertyName.CanTurnBeEnded, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 2L, NEndTurnButton.PropertyName._state, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 1L, NEndTurnButton.PropertyName._isShiny, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NEndTurnButton.PropertyName._visuals, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NEndTurnButton.PropertyName._glowTexture, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NEndTurnButton.PropertyName._normalTexture, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NEndTurnButton.PropertyName._image, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NEndTurnButton.PropertyName._hsv, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NEndTurnButton.PropertyName._glow, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NEndTurnButton.PropertyName._glowVfx, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NEndTurnButton.PropertyName._label, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NEndTurnButton.PropertyName._combatUi, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NEndTurnButton.PropertyName._viewport, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NEndTurnButton.PropertyName._playerIconContainer, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NEndTurnButton.PropertyName._longPressBar, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 3L, NEndTurnButton.PropertyName._pulseTimer, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 5L, NEndTurnButton.PropertyName.ShowPos, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 5L, NEndTurnButton.PropertyName.HidePos, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NEndTurnButton.PropertyName._positionTween, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NEndTurnButton.PropertyName._hoverTween, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NEndTurnButton.PropertyName._glowVfxTween, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NEndTurnButton.PropertyName._glowEnableTween, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 2L, NEndTurnButton.PropertyName._endTurnWithNoPlayableCardsCount, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 34L, NEndTurnButton.PropertyName.Hotkeys, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    base.SaveGodotObjectData(info);
    info.AddProperty(NEndTurnButton.PropertyName._state, Variant.From<NEndTurnButton.State>(ref this._state));
    info.AddProperty(NEndTurnButton.PropertyName._isShiny, Variant.From<bool>(ref this._isShiny));
    info.AddProperty(NEndTurnButton.PropertyName._visuals, Variant.From<Control>(ref this._visuals));
    info.AddProperty(NEndTurnButton.PropertyName._glowTexture, Variant.From<Texture2D>(ref this._glowTexture));
    info.AddProperty(NEndTurnButton.PropertyName._normalTexture, Variant.From<Texture2D>(ref this._normalTexture));
    info.AddProperty(NEndTurnButton.PropertyName._image, Variant.From<TextureRect>(ref this._image));
    info.AddProperty(NEndTurnButton.PropertyName._hsv, Variant.From<ShaderMaterial>(ref this._hsv));
    info.AddProperty(NEndTurnButton.PropertyName._glow, Variant.From<Control>(ref this._glow));
    info.AddProperty(NEndTurnButton.PropertyName._glowVfx, Variant.From<Control>(ref this._glowVfx));
    info.AddProperty(NEndTurnButton.PropertyName._label, Variant.From<MegaLabel>(ref this._label));
    info.AddProperty(NEndTurnButton.PropertyName._combatUi, Variant.From<NCombatUi>(ref this._combatUi));
    info.AddProperty(NEndTurnButton.PropertyName._viewport, Variant.From<Viewport>(ref this._viewport));
    info.AddProperty(NEndTurnButton.PropertyName._playerIconContainer, Variant.From<NMultiplayerVoteContainer>(ref this._playerIconContainer));
    info.AddProperty(NEndTurnButton.PropertyName._longPressBar, Variant.From<NEndTurnLongPressBar>(ref this._longPressBar));
    info.AddProperty(NEndTurnButton.PropertyName._pulseTimer, Variant.From<float>(ref this._pulseTimer));
    info.AddProperty(NEndTurnButton.PropertyName._positionTween, Variant.From<Tween>(ref this._positionTween));
    info.AddProperty(NEndTurnButton.PropertyName._hoverTween, Variant.From<Tween>(ref this._hoverTween));
    info.AddProperty(NEndTurnButton.PropertyName._glowVfxTween, Variant.From<Tween>(ref this._glowVfxTween));
    info.AddProperty(NEndTurnButton.PropertyName._glowEnableTween, Variant.From<Tween>(ref this._glowEnableTween));
    info.AddProperty(NEndTurnButton.PropertyName._endTurnWithNoPlayableCardsCount, Variant.From<int>(ref this._endTurnWithNoPlayableCardsCount));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    base.RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NEndTurnButton.PropertyName._state, ref variant1))
      this._state = ((Variant) ref variant1).As<NEndTurnButton.State>();
    Variant variant2;
    if (info.TryGetProperty(NEndTurnButton.PropertyName._isShiny, ref variant2))
      this._isShiny = ((Variant) ref variant2).As<bool>();
    Variant variant3;
    if (info.TryGetProperty(NEndTurnButton.PropertyName._visuals, ref variant3))
      this._visuals = ((Variant) ref variant3).As<Control>();
    Variant variant4;
    if (info.TryGetProperty(NEndTurnButton.PropertyName._glowTexture, ref variant4))
      this._glowTexture = ((Variant) ref variant4).As<Texture2D>();
    Variant variant5;
    if (info.TryGetProperty(NEndTurnButton.PropertyName._normalTexture, ref variant5))
      this._normalTexture = ((Variant) ref variant5).As<Texture2D>();
    Variant variant6;
    if (info.TryGetProperty(NEndTurnButton.PropertyName._image, ref variant6))
      this._image = ((Variant) ref variant6).As<TextureRect>();
    Variant variant7;
    if (info.TryGetProperty(NEndTurnButton.PropertyName._hsv, ref variant7))
      this._hsv = ((Variant) ref variant7).As<ShaderMaterial>();
    Variant variant8;
    if (info.TryGetProperty(NEndTurnButton.PropertyName._glow, ref variant8))
      this._glow = ((Variant) ref variant8).As<Control>();
    Variant variant9;
    if (info.TryGetProperty(NEndTurnButton.PropertyName._glowVfx, ref variant9))
      this._glowVfx = ((Variant) ref variant9).As<Control>();
    Variant variant10;
    if (info.TryGetProperty(NEndTurnButton.PropertyName._label, ref variant10))
      this._label = ((Variant) ref variant10).As<MegaLabel>();
    Variant variant11;
    if (info.TryGetProperty(NEndTurnButton.PropertyName._combatUi, ref variant11))
      this._combatUi = ((Variant) ref variant11).As<NCombatUi>();
    Variant variant12;
    if (info.TryGetProperty(NEndTurnButton.PropertyName._viewport, ref variant12))
      this._viewport = ((Variant) ref variant12).As<Viewport>();
    Variant variant13;
    if (info.TryGetProperty(NEndTurnButton.PropertyName._playerIconContainer, ref variant13))
      this._playerIconContainer = ((Variant) ref variant13).As<NMultiplayerVoteContainer>();
    Variant variant14;
    if (info.TryGetProperty(NEndTurnButton.PropertyName._longPressBar, ref variant14))
      this._longPressBar = ((Variant) ref variant14).As<NEndTurnLongPressBar>();
    Variant variant15;
    if (info.TryGetProperty(NEndTurnButton.PropertyName._pulseTimer, ref variant15))
      this._pulseTimer = ((Variant) ref variant15).As<float>();
    Variant variant16;
    if (info.TryGetProperty(NEndTurnButton.PropertyName._positionTween, ref variant16))
      this._positionTween = ((Variant) ref variant16).As<Tween>();
    Variant variant17;
    if (info.TryGetProperty(NEndTurnButton.PropertyName._hoverTween, ref variant17))
      this._hoverTween = ((Variant) ref variant17).As<Tween>();
    Variant variant18;
    if (info.TryGetProperty(NEndTurnButton.PropertyName._glowVfxTween, ref variant18))
      this._glowVfxTween = ((Variant) ref variant18).As<Tween>();
    Variant variant19;
    if (info.TryGetProperty(NEndTurnButton.PropertyName._glowEnableTween, ref variant19))
      this._glowEnableTween = ((Variant) ref variant19).As<Tween>();
    Variant variant20;
    if (!info.TryGetProperty(NEndTurnButton.PropertyName._endTurnWithNoPlayableCardsCount, ref variant20))
      return;
    this._endTurnWithNoPlayableCardsCount = ((Variant) ref variant20).As<int>();
  }

  private enum State
  {
    Enabled,
    Disabled,
    Hidden,
  }

  public new class MethodName : NButton.MethodName
  {
    public new static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public new static readonly StringName _EnterTree = StringName.op_Implicit(nameof (_EnterTree));
    public new static readonly StringName _ExitTree = StringName.op_Implicit(nameof (_ExitTree));
    public static readonly StringName StartOrStopPulseVfx = StringName.op_Implicit(nameof (StartOrStopPulseVfx));
    public static readonly StringName GlowPulse = StringName.op_Implicit(nameof (GlowPulse));
    public new static readonly StringName OnRelease = StringName.op_Implicit(nameof (OnRelease));
    public static readonly StringName CallReleaseLogic = StringName.op_Implicit(nameof (CallReleaseLogic));
    public static readonly StringName SecretEndTurnLogicViaFtue = StringName.op_Implicit(nameof (SecretEndTurnLogicViaFtue));
    public static readonly StringName ShouldShowPlayableCardsFtue = StringName.op_Implicit(nameof (ShouldShowPlayableCardsFtue));
    public new static readonly StringName OnEnable = StringName.op_Implicit(nameof (OnEnable));
    public new static readonly StringName OnDisable = StringName.op_Implicit(nameof (OnDisable));
    public static readonly StringName AnimOut = StringName.op_Implicit(nameof (AnimOut));
    public static readonly StringName AnimIn = StringName.op_Implicit(nameof (AnimIn));
    public static readonly StringName OnCombatEnded = StringName.op_Implicit(nameof (OnCombatEnded));
    public new static readonly StringName OnFocus = StringName.op_Implicit(nameof (OnFocus));
    public static readonly StringName HasPlayableCard = StringName.op_Implicit(nameof (HasPlayableCard));
    public new static readonly StringName OnUnfocus = StringName.op_Implicit(nameof (OnUnfocus));
    public new static readonly StringName OnPress = StringName.op_Implicit(nameof (OnPress));
    public static readonly StringName UpdateShaderV = StringName.op_Implicit(nameof (UpdateShaderV));
    public static readonly StringName SetState = StringName.op_Implicit(nameof (SetState));
    public static readonly StringName RefreshEnabled = StringName.op_Implicit(nameof (RefreshEnabled));
  }

  public new class PropertyName : NButton.PropertyName
  {
    public static readonly StringName CanTurnBeEnded = StringName.op_Implicit(nameof (CanTurnBeEnded));
    public static readonly StringName ShowPos = StringName.op_Implicit(nameof (ShowPos));
    public static readonly StringName HidePos = StringName.op_Implicit(nameof (HidePos));
    public new static readonly StringName Hotkeys = StringName.op_Implicit(nameof (Hotkeys));
    public static readonly StringName _state = StringName.op_Implicit(nameof (_state));
    public static readonly StringName _isShiny = StringName.op_Implicit(nameof (_isShiny));
    public static readonly StringName _visuals = StringName.op_Implicit(nameof (_visuals));
    public static readonly StringName _glowTexture = StringName.op_Implicit(nameof (_glowTexture));
    public static readonly StringName _normalTexture = StringName.op_Implicit(nameof (_normalTexture));
    public static readonly StringName _image = StringName.op_Implicit(nameof (_image));
    public static readonly StringName _hsv = StringName.op_Implicit(nameof (_hsv));
    public static readonly StringName _glow = StringName.op_Implicit(nameof (_glow));
    public static readonly StringName _glowVfx = StringName.op_Implicit(nameof (_glowVfx));
    public static readonly StringName _label = StringName.op_Implicit(nameof (_label));
    public static readonly StringName _combatUi = StringName.op_Implicit(nameof (_combatUi));
    public static readonly StringName _viewport = StringName.op_Implicit(nameof (_viewport));
    public static readonly StringName _playerIconContainer = StringName.op_Implicit(nameof (_playerIconContainer));
    public static readonly StringName _longPressBar = StringName.op_Implicit(nameof (_longPressBar));
    public static readonly StringName _pulseTimer = StringName.op_Implicit(nameof (_pulseTimer));
    public static readonly StringName _positionTween = StringName.op_Implicit(nameof (_positionTween));
    public static readonly StringName _hoverTween = StringName.op_Implicit(nameof (_hoverTween));
    public static readonly StringName _glowVfxTween = StringName.op_Implicit(nameof (_glowVfxTween));
    public static readonly StringName _glowEnableTween = StringName.op_Implicit(nameof (_glowEnableTween));
    public static readonly StringName _endTurnWithNoPlayableCardsCount = StringName.op_Implicit(nameof (_endTurnWithNoPlayableCardsCount));
  }

  public new class SignalName : NButton.SignalName
  {
  }
}
