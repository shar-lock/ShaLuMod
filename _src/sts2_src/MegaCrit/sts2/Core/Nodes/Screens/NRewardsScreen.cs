// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Screens.NRewardsScreen
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Audio.Debug;
using MegaCrit.Sts2.Core.Entities.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Hooks;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Map;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Nodes.Ftue;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.Rewards;
using MegaCrit.Sts2.Core.Nodes.Screens.Overlays;
using MegaCrit.Sts2.Core.Nodes.Screens.ScreenContext;
using MegaCrit.Sts2.Core.Rewards;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.Runs.History;
using MegaCrit.Sts2.Core.Saves;
using MegaCrit.Sts2.Core.TestSupport;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Screens;

[ScriptPath("res://src/Core/Nodes/Screens/NRewardsScreen.cs")]
public class NRewardsScreen : Control, IOverlayScreen, IScreenContext
{
  private const float _scrollLimitTop = 35f;
  private const int _scrollbarThreshold = 400;
  private IRunState _runState;
  private NProceedButton _proceedButton;
  private Control _rewardsContainer;
  private NScrollbar _scrollbar;
  private MegaLabel _headerLabel;
  private Control _rewardContainerMask;
  private Control _waitingForOtherPlayersOverlay;
  private Control _rewardsWindow;
  private Vector2 _targetDragPos;
  private bool _scrollbarPressed;
  private Tween? _fadeTween;
  private readonly List<Control> _rewardButtons = new List<Control>();
  private readonly List<Control> _skippedRewardButtons = new List<Control>();
  private Control? _lastRewardFocused;
  private RewardsSet _rewardsSet;
  private bool _disableProceedForever;
  private bool _isTerminal;
  private bool _skipDisallowed;
  private static readonly LocString _waitingLoc = new LocString("gameplay_ui", "MULTIPLAYER_WAITING");
  private 
  #nullable disable
  NRewardsScreen.CompletedEventHandler backing_Completed;

  private bool CanScroll => (double) this._rewardsContainer.Size.Y >= 400.0;

  private float ScrollLimitBottom
  {
    get => (float) (35.0 - (double) this._rewardsContainer.Size.Y + 400.0);
  }

  private static 
  #nullable enable
  string ScenePath => SceneHelper.GetScenePath("screens/rewards_screen");

  public static IEnumerable<string> AssetPaths
  {
    get
    {
      return (IEnumerable<string>) new \u003C\u003Ez__ReadOnlySingleElementList<string>(NRewardsScreen.ScenePath);
    }
  }

  public bool IsComplete { get; private set; }

  public NetScreenType ScreenType => NetScreenType.Rewards;

  public static NRewardsScreen ShowScreen(RewardsSet set, bool isTerminal, IRunState runState)
  {
    NRewardsScreen screen = PreloadManager.Cache.GetScene(NRewardsScreen.ScenePath).Instantiate<NRewardsScreen>((PackedScene.GenEditState) 0L);
    screen._rewardsSet = set;
    screen._isTerminal = isTerminal;
    screen._runState = runState;
    NOverlayStack.Instance.Push((IOverlayScreen) screen);
    return screen;
  }

  public override void _Ready()
  {
    this._proceedButton = ((Node) this).GetNode<NProceedButton>(NodePath.op_Implicit("ProceedButton"));
    this._rewardContainerMask = ((Node) this).GetNode<Control>(NodePath.op_Implicit("%RewardContainerMask"));
    this._rewardsContainer = ((Node) this).GetNode<Control>(NodePath.op_Implicit("%RewardsContainer"));
    this._scrollbar = ((Node) this).GetNode<NScrollbar>(NodePath.op_Implicit("%Scrollbar"));
    this._headerLabel = ((Node) this).GetNode<MegaLabel>(NodePath.op_Implicit("%HeaderLabel"));
    this._waitingForOtherPlayersOverlay = ((Node) this).GetNode<Control>(NodePath.op_Implicit("%WaitingForOtherPlayers"));
    ((Node) this._waitingForOtherPlayersOverlay).GetNode<MegaLabel>(NodePath.op_Implicit("Label")).SetTextAutoSize(NRewardsScreen._waitingLoc.GetRawText());
    this._rewardsWindow = ((Node) this).GetNode<Control>(NodePath.op_Implicit("Rewards"));
    ((CanvasItem) this._rewardsWindow).Modulate = StsColors.transparentBlack;
    ((GodotObject) this._proceedButton).Connect(NClickableControl.SignalName.Released, Callable.From<NButton>(new Action<NButton>(this.OnProceedButtonPressed)), 0U);
    this._proceedButton.SetPulseState(false);
    this.TryEnableProceedButton();
    this._proceedButton.UpdateText(NProceedButton.SkipLoc);
    NDebugAudioManager.Instance?.Play("victory.mp3");
    ((GodotObject) this._scrollbar).Connect(NScrollbar.SignalName.MousePressed, Callable.From<InputEvent>((Action<InputEvent>) (_ => this._scrollbarPressed = true)), 0U);
    ((GodotObject) this._scrollbar).Connect(NScrollbar.SignalName.MouseReleased, Callable.From<InputEvent>((Action<InputEvent>) (_ => this._scrollbarPressed = false)), 0U);
    this._targetDragPos = new Vector2(this._rewardsContainer.Position.X, 35f);
    int num;
    if (this._runState.CurrentRoom is CombatRoom)
    {
      MapPointHistoryEntry pointHistoryEntry = this._runState.CurrentMapPointHistoryEntry;
      num = pointHistoryEntry != null ? (pointHistoryEntry.GetEntry(this._rewardsSet.Player.NetId).WasMugged ? 1 : 0) : 0;
    }
    else
      num = 0;
    this._headerLabel.SetTextAutoSize(new LocString("gameplay_ui", num != 0 ? "COMBAT_REWARD_HEADER_MUGGED" : "COMBAT_REWARD_HEADER_LOOT").GetFormattedText());
    ((GodotObject) ((Node) this).GetViewport()).Connect(Viewport.SignalName.GuiFocusChanged, Callable.From<Control>(new Action<Control>(this.ProcessGuiFocus)), 0U);
    ((GodotObject) this._rewardsContainer).Connect(Control.SignalName.FocusEntered, Callable.From((Action) (() => this.DefaultFocusedControl.TryGrabFocus())), 0U);
    foreach (Control rewardButton in this._rewardButtons)
      this.RemoveButton(rewardButton);
    List<Reward> list = this._rewardsSet.Rewards.ToList<Reward>();
    this._rewardButtons.Clear();
    foreach (Reward reward in list)
    {
      Control option;
      if (reward is LinkedRewardSet linkedReward)
      {
        option = (Control) NLinkedRewardSet.Create(linkedReward, this);
        ((GodotObject) option).Connect(NLinkedRewardSet.SignalName.RewardClaimed, Callable.From<NLinkedRewardSet>(new Action<NLinkedRewardSet>(this.RewardCollectedFrom)), 0U);
      }
      else
      {
        option = (Control) NRewardButton.Create(reward, this);
        ((GodotObject) option).Connect(NRewardButton.SignalName.RewardClaimed, Callable.From<NRewardButton>(new Action<NRewardButton>(this.RewardCollectedFrom)), 0U);
        ((GodotObject) option).Connect(NRewardButton.SignalName.RewardSkipped, Callable.From<NRewardButton>(new Action<NRewardButton>(this.RewardSkippedFrom)), 0U);
        ((GodotObject) option).Connect(Control.SignalName.FocusEntered, Callable.From<Control>((Func<Control>) (() => this._lastRewardFocused = option)), 0U);
      }
      reward.MarkContentAsSeen();
      this._rewardButtons.Add(option);
      ((Node) this._rewardsContainer).AddChildSafely((Node) option);
    }
    if (this._rewardsSet.DisallowSkipping)
    {
      this._skipDisallowed = true;
      if (this._proceedButton.IsSkip)
        this._proceedButton.Disable();
    }
    if (this._rewardsContainer.HasFocus())
      this.DefaultFocusedControl.TryGrabFocus();
    TaskHelper.RunSafely(this.RelicFtueCheck());
    Callable callable = Callable.From(new Action(this.UpdateScreenState));
    ((Callable) ref callable).CallDeferred(Array.Empty<Variant>());
  }

  private async Task RelicFtueCheck()
  {
    if (SaveManager.Instance.SeenFtue("obtain_relic_ftue"))
      return;
    double num1 = (double) await ((Node) this).AwaitProcessFrame();
    double num2 = (double) await ((Node) this).AwaitProcessFrame();
    foreach (NRewardButton relicReward in this._rewardButtons.OfType<NRewardButton>())
    {
      if (relicReward.Reward is RelicReward)
      {
        NModalContainer.Instance.Add((Node) NRelicRewardFtue.Create((Control) relicReward));
        SaveManager.Instance.MarkFtueAsComplete("obtain_relic_ftue");
        break;
      }
    }
  }

  public void RewardCollectedFrom(Control button)
  {
    int num = this._rewardButtons.IndexOf(button);
    this.RemoveButton(button);
    this._lastRewardFocused = this._rewardButtons.Count > 0 ? this._rewardButtons[Mathf.Min(num, this._rewardButtons.Count - 1)] : (Control) null;
    this.UpdateScreenState();
    if (this._rewardButtons.Count <= 0 && !this._isTerminal)
      return;
    this.TryEnableProceedButton();
    if (!this._proceedButton.IsEnabled || this._rewardButtons.Except<Control>((IEnumerable<Control>) this._skippedRewardButtons).Any<Control>())
      return;
    this._proceedButton.SetPulseState(true);
  }

  public void RewardSkippedFrom(Control button)
  {
    this._skippedRewardButtons.Add(button);
    if (this._rewardButtons.Except<Control>((IEnumerable<Control>) this._skippedRewardButtons).Any<Control>())
      return;
    this._proceedButton.SetPulseState(true);
  }

  private void UpdateScreenState()
  {
    if (this._rewardButtons.Count == 0)
    {
      if (!RunManager.Instance.RewardsSetSynchronizer.IsRewardsSetCompleted(this._rewardsSet))
        Log.Error("All rewards have been taken, but the rewards set is not complete on the backend!");
      if (this._isTerminal)
      {
        this._fadeTween?.Kill();
        this._fadeTween = ((Node) this).CreateTween().SetParallel(true);
        this._fadeTween.TweenProperty((GodotObject) ((Node) this).GetNode<Control>(NodePath.op_Implicit("Rewards")), NodePath.op_Implicit("modulate:a"), Variant.op_Implicit(0.0f), 0.25);
        NOverlayStack.Instance.HideBackstop();
        this._proceedButton.UpdateText(NProceedButton.ProceedLoc);
        this.TryEnableProceedButton();
        this._proceedButton.SetPulseState(true);
        this._rewardsContainer.FocusMode = (Control.FocusModeEnum) 0L;
        this.IsComplete = true;
        ((GodotObject) this).EmitSignal(NRewardsScreen.SignalName.Completed, Array.Empty<Variant>());
      }
      else
        NOverlayStack.Instance.Remove((IOverlayScreen) this);
    }
    this._rewardsContainer.ResetSize();
    ((CanvasItem) this._scrollbar).Visible = this.CanScroll;
    ((Control) this._scrollbar).MouseFilter = this.CanScroll ? (Control.MouseFilterEnum) 0L : (Control.MouseFilterEnum) 2L;
    if (!this.CanScroll)
      this._targetDragPos.Y = 35f;
    for (int index = 0; index < this._rewardButtons.Count; ++index)
    {
      this._rewardButtons[index].FocusNeighborLeft = ((Node) this._rewardButtons[index]).GetPath();
      this._rewardButtons[index].FocusNeighborRight = ((Node) this._rewardButtons[index]).GetPath();
      this._rewardButtons[index].FocusNeighborTop = index > 0 ? ((Node) this._rewardButtons[index - 1]).GetPath() : ((Node) this._rewardButtons[index]).GetPath();
      this._rewardButtons[index].FocusNeighborBottom = index < this._rewardButtons.Count - 1 ? ((Node) this._rewardButtons[index + 1]).GetPath() : ((Node) this._rewardButtons[index]).GetPath();
    }
  }

  private void RemoveButton(Control button)
  {
    ((Node) button).GetParent().RemoveChildSafely((Node) button);
    ((Node) button).QueueFreeSafely();
    int num = this._rewardButtons.IndexOf(button);
    this._rewardButtons.Remove(button);
    if (this._rewardButtons.Count <= 0)
      return;
    this._rewardButtons[Mathf.Min(num, this._rewardButtons.Count - 1)].TryGrabFocus();
  }

  private void OnProceedButtonPressed(NButton _)
  {
    if (RunManager.Instance.debugAfterCombatRewardsOverride != null && this._isTerminal)
    {
      Action combatRewardsOverride = RunManager.Instance.debugAfterCombatRewardsOverride;
      if (combatRewardsOverride == null)
        return;
      combatRewardsOverride();
    }
    else if (this._isTerminal && (this._runState.CurrentRoom.RoomType == RoomType.Boss || this._runState.CurrentRoom.IsVictoryRoom))
    {
      int num;
      if (this._runState.Map.SecondBossMapPoint != null)
      {
        MapCoord? currentMapCoord = this._runState.CurrentMapCoord;
        MapCoord coord = this._runState.Map.BossMapPoint.coord;
        num = currentMapCoord.HasValue ? (currentMapCoord.GetValueOrDefault() == coord ? 1 : 0) : 0;
      }
      else
        num = 0;
      if (num != 0)
      {
        TaskHelper.RunSafely(RunManager.Instance.ProceedFromTerminalRewardsScreen());
      }
      else
      {
        this._proceedButton.Disable();
        this._disableProceedForever = true;
        if (RunManager.Instance.ActChangeSynchronizer.IsWaitingForOtherPlayers())
          ((CanvasItem) this._waitingForOtherPlayersOverlay).Visible = true;
        RunManager.Instance.ActChangeSynchronizer.SetLocalPlayerReady();
      }
    }
    else if (this._isTerminal)
    {
      if (this._proceedButton.IsSkip)
      {
        if (TestMode.IsOn || SaveManager.Instance.SeenFtue("combat_reward_ftue"))
          TaskHelper.RunSafely(RunManager.Instance.ProceedFromTerminalRewardsScreen());
        else
          TaskHelper.RunSafely(this.RewardFtueCheck());
      }
      else
      {
        if (this._runState.ActFloor > 4)
          SaveManager.Instance.MarkFtueAsComplete("combat_reward_ftue");
        TaskHelper.RunSafely(RunManager.Instance.ProceedFromTerminalRewardsScreen());
      }
    }
    else
    {
      RunManager.Instance.RewardsSetSynchronizer.SkipLocalRewardsSet();
      NOverlayStack.Instance.Remove((IOverlayScreen) this);
    }
  }

  private void BeforeRoomExit()
  {
    this._rewardButtons.Clear();
    this.UpdateScreenState();
    if (!((Node) this).IsValid())
      return;
    this._proceedButton.Disable();
  }

  public void AfterOverlayOpened()
  {
    RunManager.Instance.RewardsSetSynchronizer.RewardsSkippedDuringRoomExit += new Action(this.BeforeRoomExit);
  }

  public void AfterOverlayClosed()
  {
    this._proceedButton.Disable();
    RunManager.Instance.RewardsSetSynchronizer.RewardsSkippedDuringRoomExit -= new Action(this.BeforeRoomExit);
    ((Node) this).QueueFreeSafely();
  }

  private void TryEnableProceedButton()
  {
    if (this._disableProceedForever || this._skipDisallowed && this._proceedButton.IsSkip || !Hook.ShouldProceedToNextMapPoint(this._runState) || this._proceedButton.IsEnabled)
      return;
    if (this._isTerminal && this._rewardButtons.Count == 0)
      NOverlayStack.Instance.HideBackstop();
    this._proceedButton.Enable();
  }

  public void AfterOverlayShown()
  {
    this.TryEnableProceedButton();
    this.UpdateScreenState();
    if (this.IsComplete)
      return;
    Tween fadeTween = this._fadeTween;
    if (fadeTween != null)
      fadeTween.FastForwardToCompletion();
    this._fadeTween = ((Node) this).CreateTween().SetParallel(true);
    this._fadeTween.TweenProperty((GodotObject) this._rewardsWindow, NodePath.op_Implicit("modulate"), Variant.op_Implicit(Colors.White), 0.5);
    this._fadeTween.TweenProperty((GodotObject) this._rewardsWindow, NodePath.op_Implicit("position:y"), Variant.op_Implicit(this._rewardsWindow.Position.Y), 0.5).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 10L).From(Variant.op_Implicit(this._rewardsWindow.Position.Y + 100f));
  }

  public void AfterOverlayHidden()
  {
    this._proceedButton.Disable();
    if (this.IsComplete)
      return;
    Tween fadeTween = this._fadeTween;
    if (fadeTween != null)
      fadeTween.FastForwardToCompletion();
    this._fadeTween = ((Node) this).CreateTween();
    this._fadeTween.TweenProperty((GodotObject) this._rewardsWindow, NodePath.op_Implicit("modulate:a"), Variant.op_Implicit(0), 0.25);
  }

  public bool UseSharedBackstop => true;

  public override void _GuiInput(InputEvent inputEvent)
  {
    if (!((CanvasItem) this).IsVisibleInTree() || !this.CanScroll)
      return;
    this.ProcessScrollEvent(inputEvent);
  }

  private void ProcessScrollEvent(InputEvent inputEvent)
  {
    this._targetDragPos = Vector2.op_Addition(this._targetDragPos, new Vector2(0.0f, ScrollHelper.GetDragForScrollEvent(inputEvent)));
  }

  private void ProcessGuiFocus(Control focusedControl)
  {
    if (!((CanvasItem) this).IsVisibleInTree() || !this.CanScroll || !NControllerManager.Instance.IsUsingController || !this._rewardButtons.Contains(focusedControl))
      return;
    this._targetDragPos = new Vector2(this._targetDragPos.X, Mathf.Clamp((float) (-(double) focusedControl.Position.Y + (double) this._rewardContainerMask.Size.Y * 0.5), this.ScrollLimitBottom, 35f));
  }

  public override void _Process(double delta)
  {
    if (!((CanvasItem) this).IsVisibleInTree())
      return;
    this.UpdateScrollPosition(delta);
  }

  private void UpdateScrollPosition(double delta)
  {
    Vector2 position1 = this._rewardsContainer.Position;
    if (!((Vector2) ref position1).IsEqualApprox(this._targetDragPos))
    {
      float num1 = (float) Mathf.Sign(this._rewardsContainer.Position.Y - this._targetDragPos.Y);
      Control rewardsContainer = this._rewardsContainer;
      Vector2 position2 = this._rewardsContainer.Position;
      Vector2 vector2 = ((Vector2) ref position2).Lerp(this._targetDragPos, Mathf.Clamp((float) delta * 15f, 0.0f, 1f));
      rewardsContainer.Position = vector2;
      float num2 = (float) Mathf.Sign(this._rewardsContainer.Position.Y - this._targetDragPos.Y);
      if ((double) Math.Abs(this._rewardsContainer.Position.Y - this._targetDragPos.Y) < 0.5 || !Mathf.IsEqualApprox(num1, num2))
        this._rewardsContainer.Position = this._targetDragPos;
      if (!this._scrollbarPressed && this.CanScroll)
        this._scrollbar.SetValueWithoutAnimation((double) Mathf.Clamp(this._rewardsContainer.Position.Y / this.ScrollLimitBottom, 0.0f, 1f) * 100.0);
    }
    if (this._scrollbarPressed)
      this._targetDragPos.Y = Mathf.Lerp(35f, this.ScrollLimitBottom, (float) this._scrollbar.Value * 0.01f);
    if ((double) this._targetDragPos.Y < (double) Mathf.Min(this.ScrollLimitBottom, 0.0f))
    {
      this._targetDragPos.Y = Mathf.Lerp(this._targetDragPos.Y, this.ScrollLimitBottom, (float) delta * 12f);
    }
    else
    {
      if ((double) this._targetDragPos.Y <= (double) Mathf.Max(this.ScrollLimitBottom, 0.0f))
        return;
      this._targetDragPos.Y = Mathf.Lerp(this._targetDragPos.Y, 35f, (float) delta * 12f);
    }
  }

  private async Task RewardFtueCheck()
  {
    ((CanvasItem) this._proceedButton).Hide();
    NCombatRewardFtue modalToCreate = NCombatRewardFtue.Create(this._rewardsContainer);
    NModalContainer.Instance.Add((Node) modalToCreate);
    SaveManager.Instance.MarkFtueAsComplete("combat_reward_ftue");
    await modalToCreate.WaitForPlayerToConfirm();
    ((CanvasItem) this._proceedButton).Show();
  }

  public Control DefaultFocusedControl
  {
    get
    {
      return this._rewardButtons.Count == 0 ? this._rewardsContainer : this._lastRewardFocused ?? this._rewardButtons[0];
    }
  }

  public Control FocusedControlFromTopBar
  {
    get => this._rewardButtons.Count <= 0 ? this._rewardsContainer : this._rewardButtons[0];
  }

  public void HideWaitingForPlayersScreen()
  {
    ((CanvasItem) this._waitingForOtherPlayersOverlay).Visible = false;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(18)
    {
      new MethodInfo(NRewardsScreen.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NRewardsScreen.MethodName.RewardCollectedFrom, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("button"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false)
      }, (List<Variant>) null),
      new MethodInfo(NRewardsScreen.MethodName.RewardSkippedFrom, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("button"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false)
      }, (List<Variant>) null),
      new MethodInfo(NRewardsScreen.MethodName.UpdateScreenState, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NRewardsScreen.MethodName.RemoveButton, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("button"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false)
      }, (List<Variant>) null),
      new MethodInfo(NRewardsScreen.MethodName.OnProceedButtonPressed, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("_"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false)
      }, (List<Variant>) null),
      new MethodInfo(NRewardsScreen.MethodName.BeforeRoomExit, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NRewardsScreen.MethodName.AfterOverlayOpened, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NRewardsScreen.MethodName.AfterOverlayClosed, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NRewardsScreen.MethodName.TryEnableProceedButton, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NRewardsScreen.MethodName.AfterOverlayShown, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NRewardsScreen.MethodName.AfterOverlayHidden, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NRewardsScreen.MethodName._GuiInput, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("inputEvent"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("InputEvent"), false)
      }, (List<Variant>) null),
      new MethodInfo(NRewardsScreen.MethodName.ProcessScrollEvent, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("inputEvent"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("InputEvent"), false)
      }, (List<Variant>) null),
      new MethodInfo(NRewardsScreen.MethodName.ProcessGuiFocus, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("focusedControl"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false)
      }, (List<Variant>) null),
      new MethodInfo(NRewardsScreen.MethodName._Process, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 3L, StringName.op_Implicit("delta"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NRewardsScreen.MethodName.UpdateScrollPosition, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 3L, StringName.op_Implicit("delta"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NRewardsScreen.MethodName.HideWaitingForPlayersScreen, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NRewardsScreen.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NRewardsScreen.MethodName.RewardCollectedFrom) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.RewardCollectedFrom(VariantUtils.ConvertTo<Control>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NRewardsScreen.MethodName.RewardSkippedFrom) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.RewardSkippedFrom(VariantUtils.ConvertTo<Control>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NRewardsScreen.MethodName.UpdateScreenState) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.UpdateScreenState();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NRewardsScreen.MethodName.RemoveButton) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.RemoveButton(VariantUtils.ConvertTo<Control>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NRewardsScreen.MethodName.OnProceedButtonPressed) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.OnProceedButtonPressed(VariantUtils.ConvertTo<NButton>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NRewardsScreen.MethodName.BeforeRoomExit) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.BeforeRoomExit();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NRewardsScreen.MethodName.AfterOverlayOpened) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.AfterOverlayOpened();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NRewardsScreen.MethodName.AfterOverlayClosed) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.AfterOverlayClosed();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NRewardsScreen.MethodName.TryEnableProceedButton) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.TryEnableProceedButton();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NRewardsScreen.MethodName.AfterOverlayShown) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.AfterOverlayShown();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NRewardsScreen.MethodName.AfterOverlayHidden) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.AfterOverlayHidden();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NRewardsScreen.MethodName._GuiInput) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      base._GuiInput(VariantUtils.ConvertTo<InputEvent>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NRewardsScreen.MethodName.ProcessScrollEvent) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.ProcessScrollEvent(VariantUtils.ConvertTo<InputEvent>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NRewardsScreen.MethodName.ProcessGuiFocus) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.ProcessGuiFocus(VariantUtils.ConvertTo<Control>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NRewardsScreen.MethodName._Process) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      ((Node) this)._Process(VariantUtils.ConvertTo<double>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NRewardsScreen.MethodName.UpdateScrollPosition) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.UpdateScrollPosition(VariantUtils.ConvertTo<double>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NRewardsScreen.MethodName.HideWaitingForPlayersScreen) || ((NativeVariantPtrArgs) ref args).Count != 0)
      return base.InvokeGodotClassMethod(ref method, args, ref ret);
    this.HideWaitingForPlayersScreen();
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NRewardsScreen.MethodName._Ready) || StringName.op_Equality(ref method, NRewardsScreen.MethodName.RewardCollectedFrom) || StringName.op_Equality(ref method, NRewardsScreen.MethodName.RewardSkippedFrom) || StringName.op_Equality(ref method, NRewardsScreen.MethodName.UpdateScreenState) || StringName.op_Equality(ref method, NRewardsScreen.MethodName.RemoveButton) || StringName.op_Equality(ref method, NRewardsScreen.MethodName.OnProceedButtonPressed) || StringName.op_Equality(ref method, NRewardsScreen.MethodName.BeforeRoomExit) || StringName.op_Equality(ref method, NRewardsScreen.MethodName.AfterOverlayOpened) || StringName.op_Equality(ref method, NRewardsScreen.MethodName.AfterOverlayClosed) || StringName.op_Equality(ref method, NRewardsScreen.MethodName.TryEnableProceedButton) || StringName.op_Equality(ref method, NRewardsScreen.MethodName.AfterOverlayShown) || StringName.op_Equality(ref method, NRewardsScreen.MethodName.AfterOverlayHidden) || StringName.op_Equality(ref method, NRewardsScreen.MethodName._GuiInput) || StringName.op_Equality(ref method, NRewardsScreen.MethodName.ProcessScrollEvent) || StringName.op_Equality(ref method, NRewardsScreen.MethodName.ProcessGuiFocus) || StringName.op_Equality(ref method, NRewardsScreen.MethodName._Process) || StringName.op_Equality(ref method, NRewardsScreen.MethodName.UpdateScrollPosition) || StringName.op_Equality(ref method, NRewardsScreen.MethodName.HideWaitingForPlayersScreen) || base.HasGodotClassMethod(ref method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NRewardsScreen.PropertyName.IsComplete))
    {
      this.IsComplete = VariantUtils.ConvertTo<bool>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NRewardsScreen.PropertyName._proceedButton))
    {
      this._proceedButton = VariantUtils.ConvertTo<NProceedButton>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NRewardsScreen.PropertyName._rewardsContainer))
    {
      this._rewardsContainer = VariantUtils.ConvertTo<Control>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NRewardsScreen.PropertyName._scrollbar))
    {
      this._scrollbar = VariantUtils.ConvertTo<NScrollbar>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NRewardsScreen.PropertyName._headerLabel))
    {
      this._headerLabel = VariantUtils.ConvertTo<MegaLabel>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NRewardsScreen.PropertyName._rewardContainerMask))
    {
      this._rewardContainerMask = VariantUtils.ConvertTo<Control>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NRewardsScreen.PropertyName._waitingForOtherPlayersOverlay))
    {
      this._waitingForOtherPlayersOverlay = VariantUtils.ConvertTo<Control>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NRewardsScreen.PropertyName._rewardsWindow))
    {
      this._rewardsWindow = VariantUtils.ConvertTo<Control>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NRewardsScreen.PropertyName._targetDragPos))
    {
      this._targetDragPos = VariantUtils.ConvertTo<Vector2>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NRewardsScreen.PropertyName._scrollbarPressed))
    {
      this._scrollbarPressed = VariantUtils.ConvertTo<bool>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NRewardsScreen.PropertyName._fadeTween))
    {
      this._fadeTween = VariantUtils.ConvertTo<Tween>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NRewardsScreen.PropertyName._lastRewardFocused))
    {
      this._lastRewardFocused = VariantUtils.ConvertTo<Control>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NRewardsScreen.PropertyName._disableProceedForever))
    {
      this._disableProceedForever = VariantUtils.ConvertTo<bool>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NRewardsScreen.PropertyName._isTerminal))
    {
      this._isTerminal = VariantUtils.ConvertTo<bool>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NRewardsScreen.PropertyName._skipDisallowed))
      return ((GodotObject) this).SetGodotClassPropertyValue(ref name, ref value);
    this._skipDisallowed = VariantUtils.ConvertTo<bool>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NRewardsScreen.PropertyName.CanScroll))
    {
      ref godot_variant local = ref value;
      bool canScroll = this.CanScroll;
      godot_variant from = VariantUtils.CreateFrom<bool>(ref canScroll);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NRewardsScreen.PropertyName.ScrollLimitBottom))
    {
      ref godot_variant local = ref value;
      float scrollLimitBottom = this.ScrollLimitBottom;
      godot_variant from = VariantUtils.CreateFrom<float>(ref scrollLimitBottom);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NRewardsScreen.PropertyName.IsComplete))
    {
      ref godot_variant local = ref value;
      bool isComplete = this.IsComplete;
      godot_variant from = VariantUtils.CreateFrom<bool>(ref isComplete);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NRewardsScreen.PropertyName.ScreenType))
    {
      ref godot_variant local = ref value;
      NetScreenType screenType = this.ScreenType;
      godot_variant from = VariantUtils.CreateFrom<NetScreenType>(ref screenType);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NRewardsScreen.PropertyName.UseSharedBackstop))
    {
      ref godot_variant local = ref value;
      bool useSharedBackstop = this.UseSharedBackstop;
      godot_variant from = VariantUtils.CreateFrom<bool>(ref useSharedBackstop);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NRewardsScreen.PropertyName.DefaultFocusedControl))
    {
      ref godot_variant local = ref value;
      Control defaultFocusedControl = this.DefaultFocusedControl;
      godot_variant from = VariantUtils.CreateFrom<Control>(ref defaultFocusedControl);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NRewardsScreen.PropertyName.FocusedControlFromTopBar))
    {
      ref godot_variant local = ref value;
      Control controlFromTopBar = this.FocusedControlFromTopBar;
      godot_variant from = VariantUtils.CreateFrom<Control>(ref controlFromTopBar);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NRewardsScreen.PropertyName._proceedButton))
    {
      value = VariantUtils.CreateFrom<NProceedButton>(ref this._proceedButton);
      return true;
    }
    if (StringName.op_Equality(ref name, NRewardsScreen.PropertyName._rewardsContainer))
    {
      value = VariantUtils.CreateFrom<Control>(ref this._rewardsContainer);
      return true;
    }
    if (StringName.op_Equality(ref name, NRewardsScreen.PropertyName._scrollbar))
    {
      value = VariantUtils.CreateFrom<NScrollbar>(ref this._scrollbar);
      return true;
    }
    if (StringName.op_Equality(ref name, NRewardsScreen.PropertyName._headerLabel))
    {
      value = VariantUtils.CreateFrom<MegaLabel>(ref this._headerLabel);
      return true;
    }
    if (StringName.op_Equality(ref name, NRewardsScreen.PropertyName._rewardContainerMask))
    {
      value = VariantUtils.CreateFrom<Control>(ref this._rewardContainerMask);
      return true;
    }
    if (StringName.op_Equality(ref name, NRewardsScreen.PropertyName._waitingForOtherPlayersOverlay))
    {
      value = VariantUtils.CreateFrom<Control>(ref this._waitingForOtherPlayersOverlay);
      return true;
    }
    if (StringName.op_Equality(ref name, NRewardsScreen.PropertyName._rewardsWindow))
    {
      value = VariantUtils.CreateFrom<Control>(ref this._rewardsWindow);
      return true;
    }
    if (StringName.op_Equality(ref name, NRewardsScreen.PropertyName._targetDragPos))
    {
      value = VariantUtils.CreateFrom<Vector2>(ref this._targetDragPos);
      return true;
    }
    if (StringName.op_Equality(ref name, NRewardsScreen.PropertyName._scrollbarPressed))
    {
      value = VariantUtils.CreateFrom<bool>(ref this._scrollbarPressed);
      return true;
    }
    if (StringName.op_Equality(ref name, NRewardsScreen.PropertyName._fadeTween))
    {
      value = VariantUtils.CreateFrom<Tween>(ref this._fadeTween);
      return true;
    }
    if (StringName.op_Equality(ref name, NRewardsScreen.PropertyName._lastRewardFocused))
    {
      value = VariantUtils.CreateFrom<Control>(ref this._lastRewardFocused);
      return true;
    }
    if (StringName.op_Equality(ref name, NRewardsScreen.PropertyName._disableProceedForever))
    {
      value = VariantUtils.CreateFrom<bool>(ref this._disableProceedForever);
      return true;
    }
    if (StringName.op_Equality(ref name, NRewardsScreen.PropertyName._isTerminal))
    {
      value = VariantUtils.CreateFrom<bool>(ref this._isTerminal);
      return true;
    }
    if (!StringName.op_Equality(ref name, NRewardsScreen.PropertyName._skipDisallowed))
      return ((GodotObject) this).GetGodotClassPropertyValue(ref name, ref value);
    value = VariantUtils.CreateFrom<bool>(ref this._skipDisallowed);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NRewardsScreen.PropertyName._proceedButton, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NRewardsScreen.PropertyName._rewardsContainer, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NRewardsScreen.PropertyName._scrollbar, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NRewardsScreen.PropertyName._headerLabel, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NRewardsScreen.PropertyName._rewardContainerMask, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NRewardsScreen.PropertyName._waitingForOtherPlayersOverlay, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NRewardsScreen.PropertyName._rewardsWindow, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 5L, NRewardsScreen.PropertyName._targetDragPos, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 1L, NRewardsScreen.PropertyName.CanScroll, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 1L, NRewardsScreen.PropertyName._scrollbarPressed, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 3L, NRewardsScreen.PropertyName.ScrollLimitBottom, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NRewardsScreen.PropertyName._fadeTween, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NRewardsScreen.PropertyName._lastRewardFocused, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 1L, NRewardsScreen.PropertyName._disableProceedForever, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 1L, NRewardsScreen.PropertyName._isTerminal, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 1L, NRewardsScreen.PropertyName._skipDisallowed, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 1L, NRewardsScreen.PropertyName.IsComplete, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 2L, NRewardsScreen.PropertyName.ScreenType, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 1L, NRewardsScreen.PropertyName.UseSharedBackstop, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NRewardsScreen.PropertyName.DefaultFocusedControl, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NRewardsScreen.PropertyName.FocusedControlFromTopBar, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).SaveGodotObjectData(info);
    GodotSerializationInfo serializationInfo = info;
    StringName isComplete1 = NRewardsScreen.PropertyName.IsComplete;
    bool isComplete2 = this.IsComplete;
    Variant variant = Variant.From<bool>(ref isComplete2);
    serializationInfo.AddProperty(isComplete1, variant);
    info.AddProperty(NRewardsScreen.PropertyName._proceedButton, Variant.From<NProceedButton>(ref this._proceedButton));
    info.AddProperty(NRewardsScreen.PropertyName._rewardsContainer, Variant.From<Control>(ref this._rewardsContainer));
    info.AddProperty(NRewardsScreen.PropertyName._scrollbar, Variant.From<NScrollbar>(ref this._scrollbar));
    info.AddProperty(NRewardsScreen.PropertyName._headerLabel, Variant.From<MegaLabel>(ref this._headerLabel));
    info.AddProperty(NRewardsScreen.PropertyName._rewardContainerMask, Variant.From<Control>(ref this._rewardContainerMask));
    info.AddProperty(NRewardsScreen.PropertyName._waitingForOtherPlayersOverlay, Variant.From<Control>(ref this._waitingForOtherPlayersOverlay));
    info.AddProperty(NRewardsScreen.PropertyName._rewardsWindow, Variant.From<Control>(ref this._rewardsWindow));
    info.AddProperty(NRewardsScreen.PropertyName._targetDragPos, Variant.From<Vector2>(ref this._targetDragPos));
    info.AddProperty(NRewardsScreen.PropertyName._scrollbarPressed, Variant.From<bool>(ref this._scrollbarPressed));
    info.AddProperty(NRewardsScreen.PropertyName._fadeTween, Variant.From<Tween>(ref this._fadeTween));
    info.AddProperty(NRewardsScreen.PropertyName._lastRewardFocused, Variant.From<Control>(ref this._lastRewardFocused));
    info.AddProperty(NRewardsScreen.PropertyName._disableProceedForever, Variant.From<bool>(ref this._disableProceedForever));
    info.AddProperty(NRewardsScreen.PropertyName._isTerminal, Variant.From<bool>(ref this._isTerminal));
    info.AddProperty(NRewardsScreen.PropertyName._skipDisallowed, Variant.From<bool>(ref this._skipDisallowed));
    info.AddSignalEventDelegate(NRewardsScreen.SignalName.Completed, (Delegate) this.backing_Completed);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NRewardsScreen.PropertyName.IsComplete, ref variant1))
      this.IsComplete = ((Variant) ref variant1).As<bool>();
    Variant variant2;
    if (info.TryGetProperty(NRewardsScreen.PropertyName._proceedButton, ref variant2))
      this._proceedButton = ((Variant) ref variant2).As<NProceedButton>();
    Variant variant3;
    if (info.TryGetProperty(NRewardsScreen.PropertyName._rewardsContainer, ref variant3))
      this._rewardsContainer = ((Variant) ref variant3).As<Control>();
    Variant variant4;
    if (info.TryGetProperty(NRewardsScreen.PropertyName._scrollbar, ref variant4))
      this._scrollbar = ((Variant) ref variant4).As<NScrollbar>();
    Variant variant5;
    if (info.TryGetProperty(NRewardsScreen.PropertyName._headerLabel, ref variant5))
      this._headerLabel = ((Variant) ref variant5).As<MegaLabel>();
    Variant variant6;
    if (info.TryGetProperty(NRewardsScreen.PropertyName._rewardContainerMask, ref variant6))
      this._rewardContainerMask = ((Variant) ref variant6).As<Control>();
    Variant variant7;
    if (info.TryGetProperty(NRewardsScreen.PropertyName._waitingForOtherPlayersOverlay, ref variant7))
      this._waitingForOtherPlayersOverlay = ((Variant) ref variant7).As<Control>();
    Variant variant8;
    if (info.TryGetProperty(NRewardsScreen.PropertyName._rewardsWindow, ref variant8))
      this._rewardsWindow = ((Variant) ref variant8).As<Control>();
    Variant variant9;
    if (info.TryGetProperty(NRewardsScreen.PropertyName._targetDragPos, ref variant9))
      this._targetDragPos = ((Variant) ref variant9).As<Vector2>();
    Variant variant10;
    if (info.TryGetProperty(NRewardsScreen.PropertyName._scrollbarPressed, ref variant10))
      this._scrollbarPressed = ((Variant) ref variant10).As<bool>();
    Variant variant11;
    if (info.TryGetProperty(NRewardsScreen.PropertyName._fadeTween, ref variant11))
      this._fadeTween = ((Variant) ref variant11).As<Tween>();
    Variant variant12;
    if (info.TryGetProperty(NRewardsScreen.PropertyName._lastRewardFocused, ref variant12))
      this._lastRewardFocused = ((Variant) ref variant12).As<Control>();
    Variant variant13;
    if (info.TryGetProperty(NRewardsScreen.PropertyName._disableProceedForever, ref variant13))
      this._disableProceedForever = ((Variant) ref variant13).As<bool>();
    Variant variant14;
    if (info.TryGetProperty(NRewardsScreen.PropertyName._isTerminal, ref variant14))
      this._isTerminal = ((Variant) ref variant14).As<bool>();
    Variant variant15;
    if (info.TryGetProperty(NRewardsScreen.PropertyName._skipDisallowed, ref variant15))
      this._skipDisallowed = ((Variant) ref variant15).As<bool>();
    NRewardsScreen.CompletedEventHandler completedEventHandler;
    if (!info.TryGetSignalEventDelegate<NRewardsScreen.CompletedEventHandler>(NRewardsScreen.SignalName.Completed, ref completedEventHandler))
      return;
    this.backing_Completed = completedEventHandler;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<MethodInfo> GetGodotSignalList()
  {
    return new List<MethodInfo>(1)
    {
      new MethodInfo(NRewardsScreen.SignalName.Completed, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  public event NRewardsScreen.CompletedEventHandler Completed
  {
    add => this.backing_Completed += value;
    remove => this.backing_Completed -= value;
  }

  protected void EmitSignalCompleted()
  {
    ((GodotObject) this).EmitSignal(NRewardsScreen.SignalName.Completed, Array.Empty<Variant>());
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RaiseGodotClassSignalCallbacks(
    in godot_string_name signal,
    NativeVariantPtrArgs args)
  {
    if (StringName.op_Equality(ref signal, NRewardsScreen.SignalName.Completed) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      NRewardsScreen.CompletedEventHandler backingCompleted = this.backing_Completed;
      if (backingCompleted == null)
        return;
      backingCompleted();
    }
    else
      ((GodotObject) this).RaiseGodotClassSignalCallbacks(ref signal, args);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassSignal(in godot_string_name signal)
  {
    return StringName.op_Equality(ref signal, NRewardsScreen.SignalName.Completed) || base.HasGodotClassSignal(ref signal);
  }

  [Signal]
  public delegate void CompletedEventHandler();

  public class MethodName : Control.MethodName
  {
    public static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName RewardCollectedFrom = StringName.op_Implicit(nameof (RewardCollectedFrom));
    public static readonly StringName RewardSkippedFrom = StringName.op_Implicit(nameof (RewardSkippedFrom));
    public static readonly StringName UpdateScreenState = StringName.op_Implicit(nameof (UpdateScreenState));
    public static readonly StringName RemoveButton = StringName.op_Implicit(nameof (RemoveButton));
    public static readonly StringName OnProceedButtonPressed = StringName.op_Implicit(nameof (OnProceedButtonPressed));
    public static readonly StringName BeforeRoomExit = StringName.op_Implicit(nameof (BeforeRoomExit));
    public static readonly StringName AfterOverlayOpened = StringName.op_Implicit(nameof (AfterOverlayOpened));
    public static readonly StringName AfterOverlayClosed = StringName.op_Implicit(nameof (AfterOverlayClosed));
    public static readonly StringName TryEnableProceedButton = StringName.op_Implicit(nameof (TryEnableProceedButton));
    public static readonly StringName AfterOverlayShown = StringName.op_Implicit(nameof (AfterOverlayShown));
    public static readonly StringName AfterOverlayHidden = StringName.op_Implicit(nameof (AfterOverlayHidden));
    public static readonly StringName _GuiInput = StringName.op_Implicit(nameof (_GuiInput));
    public static readonly StringName ProcessScrollEvent = StringName.op_Implicit(nameof (ProcessScrollEvent));
    public static readonly StringName ProcessGuiFocus = StringName.op_Implicit(nameof (ProcessGuiFocus));
    public static readonly StringName _Process = StringName.op_Implicit(nameof (_Process));
    public static readonly StringName UpdateScrollPosition = StringName.op_Implicit(nameof (UpdateScrollPosition));
    public static readonly StringName HideWaitingForPlayersScreen = StringName.op_Implicit(nameof (HideWaitingForPlayersScreen));
  }

  public class PropertyName : Control.PropertyName
  {
    public static readonly StringName CanScroll = StringName.op_Implicit(nameof (CanScroll));
    public static readonly StringName ScrollLimitBottom = StringName.op_Implicit(nameof (ScrollLimitBottom));
    public static readonly StringName IsComplete = StringName.op_Implicit(nameof (IsComplete));
    public static readonly StringName ScreenType = StringName.op_Implicit(nameof (ScreenType));
    public static readonly StringName UseSharedBackstop = StringName.op_Implicit(nameof (UseSharedBackstop));
    public static readonly StringName DefaultFocusedControl = StringName.op_Implicit(nameof (DefaultFocusedControl));
    public static readonly StringName FocusedControlFromTopBar = StringName.op_Implicit(nameof (FocusedControlFromTopBar));
    public static readonly StringName _proceedButton = StringName.op_Implicit(nameof (_proceedButton));
    public static readonly StringName _rewardsContainer = StringName.op_Implicit(nameof (_rewardsContainer));
    public static readonly StringName _scrollbar = StringName.op_Implicit(nameof (_scrollbar));
    public static readonly StringName _headerLabel = StringName.op_Implicit(nameof (_headerLabel));
    public static readonly StringName _rewardContainerMask = StringName.op_Implicit(nameof (_rewardContainerMask));
    public static readonly StringName _waitingForOtherPlayersOverlay = StringName.op_Implicit(nameof (_waitingForOtherPlayersOverlay));
    public static readonly StringName _rewardsWindow = StringName.op_Implicit(nameof (_rewardsWindow));
    public static readonly StringName _targetDragPos = StringName.op_Implicit(nameof (_targetDragPos));
    public static readonly StringName _scrollbarPressed = StringName.op_Implicit(nameof (_scrollbarPressed));
    public static readonly StringName _fadeTween = StringName.op_Implicit(nameof (_fadeTween));
    public static readonly StringName _lastRewardFocused = StringName.op_Implicit(nameof (_lastRewardFocused));
    public static readonly StringName _disableProceedForever = StringName.op_Implicit(nameof (_disableProceedForever));
    public static readonly StringName _isTerminal = StringName.op_Implicit(nameof (_isTerminal));
    public static readonly StringName _skipDisallowed = StringName.op_Implicit(nameof (_skipDisallowed));
  }

  public class SignalName : Control.SignalName
  {
    public static readonly StringName Completed = StringName.op_Implicit(nameof (Completed));
  }
}
