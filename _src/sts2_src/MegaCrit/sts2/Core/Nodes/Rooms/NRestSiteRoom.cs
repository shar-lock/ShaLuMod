// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Rooms.NRestSiteRoom
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.Debug;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.RestSite;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Nodes.Audio;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Nodes.Ftue;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.RestSite;
using MegaCrit.Sts2.Core.Nodes.Screens.Map;
using MegaCrit.Sts2.Core.Nodes.Screens.ScreenContext;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.Saves;
using MegaCrit.Sts2.Core.TestSupport;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Rooms;

[ScriptPath("res://src/Core/Nodes/Rooms/NRestSiteRoom.cs")]
public class NRestSiteRoom : Control, IScreenContext, IRoomWithProceedButton
{
  public readonly List<NRestSiteCharacter> characterAnims = new List<NRestSiteCharacter>();
  private const float _lowDescriptionYPos = 885f;
  private const string _scenePath = "res://scenes/rooms/rest_site_room.tscn";
  private static bool _isDebugUiVisible;
  private RestSiteRoom _room;
  private IRunState _runState;
  private Control _choicesContainer;
  private Control _choicesScreen;
  private NProceedButton _proceedButton;
  private Control _restSiteLighting;
  private readonly List<Control> _characterContainers = new List<Control>();
  private readonly CancellationTokenSource _cts = new CancellationTokenSource();
  private Tween? _descriptionTween;
  private Tween? _descriptionPositionTween;
  private Tween? _choicesTween;
  private float _originalDescriptionYPos;
  private bool _roomExiting;
  private Control? _lastFocused;

  public static NRestSiteRoom? Instance => NRun.Instance?.RestSiteRoom;

  public static IEnumerable<string> AssetPaths
  {
    get
    {
      return (IEnumerable<string>) new \u003C\u003Ez__ReadOnlySingleElementList<string>("res://scenes/rooms/rest_site_room.tscn");
    }
  }

  public NProceedButton ProceedButton => this._proceedButton;

  private MegaLabel Header { get; set; }

  private MegaRichTextLabel Description { get; set; }

  private Control BgContainer { get; set; }

  public IReadOnlyList<RestSiteOption> Options => this._room.Options;

  public List<NRestSiteCharacter> Characters { get; } = new List<NRestSiteCharacter>();

  public static NRestSiteRoom? Create(RestSiteRoom room, IRunState runState)
  {
    if (TestMode.IsOn)
      return (NRestSiteRoom) null;
    NRestSiteRoom nrestSiteRoom = PreloadManager.Cache.GetScene("res://scenes/rooms/rest_site_room.tscn").Instantiate<NRestSiteRoom>((PackedScene.GenEditState) 0L);
    nrestSiteRoom._room = room;
    nrestSiteRoom._runState = runState;
    return nrestSiteRoom;
  }

  public override void _Ready()
  {
    this.Header = ((Node) this).GetNode<MegaLabel>(NodePath.op_Implicit("%Header"));
    this.Description = ((Node) this).GetNode<MegaRichTextLabel>(NodePath.op_Implicit("%Description"));
    ((CanvasItem) this.Description).Modulate = Colors.Transparent;
    this._choicesContainer = ((Node) this).GetNode<Control>(NodePath.op_Implicit("%ChoicesContainer"));
    this._choicesScreen = ((Node) this).GetNode<Control>(NodePath.op_Implicit("%ChoicesScreen"));
    this._proceedButton = ((Node) this).GetNode<NProceedButton>(NodePath.op_Implicit("%ProceedButton"));
    this.BgContainer = ((Node) this).GetNode<Control>(NodePath.op_Implicit("BgContainer"));
    this._characterContainers.Add(((Node) this).GetNode<Control>(NodePath.op_Implicit("BgContainer/Character_1")));
    this._characterContainers.Add(((Node) this).GetNode<Control>(NodePath.op_Implicit("BgContainer/Character_2")));
    this._characterContainers.Add(((Node) this).GetNode<Control>(NodePath.op_Implicit("BgContainer/Character_3")));
    this._characterContainers.Add(((Node) this).GetNode<Control>(NodePath.op_Implicit("BgContainer/Character_4")));
    Control restSiteBackground = this._runState.Act.CreateRestSiteBackground();
    ((Node) this.BgContainer).AddChildSafely((Node) restSiteBackground);
    ((Node) this.BgContainer).MoveChildSafely((Node) restSiteBackground, 0);
    this._restSiteLighting = ((Node) restSiteBackground).GetNode<Control>(NodePath.op_Implicit("%RestSiteLighting"));
    this.Header.SetTextAutoSize(new LocString("rest_site_ui", "PROMPT").GetFormattedText());
    ((GodotObject) this._proceedButton).Connect(NClickableControl.SignalName.Released, Callable.From<NButton>(new Action<NButton>(this.OnProceedButtonReleased)), 0U);
    this._proceedButton.UpdateText(NProceedButton.ProceedLoc);
    if (NRestSiteRoom._isDebugUiVisible)
      ((CanvasItem) this._choicesScreen).Modulate = Colors.Transparent;
    NGame.Instance.SetScreenShakeTarget((Control) this);
    this._proceedButton.Disable();
    for (int index = 0; index < this._runState.Players.Count; ++index)
    {
      NRestSiteCharacter child = NRestSiteCharacter.Create(this._runState.Players[index], index);
      this.characterAnims.Add(child);
      ((Node) this._characterContainers[index]).AddChildSafely((Node) child);
      child.Position = Vector2.Zero;
      if (index % 2 == 1)
        child.FlipX();
      this.Characters.Add(child);
    }
    this._originalDescriptionYPos = ((Control) this.Description).Position.Y;
    this.UpdateRestSiteOptions();
    TaskHelper.RunSafely(this.ShowFtueIfNeeded());
    RunManager.Instance.RestSiteSynchronizer.PlayerHoverChanged += new Action<ulong>(this.OnPlayerChangedHoveredRestSiteOption);
    RunManager.Instance.RestSiteSynchronizer.BeforePlayerOptionChosen += new Action<RestSiteOption, ulong>(this.OnBeforePlayerSelectedRestSiteOption);
    RunManager.Instance.RestSiteSynchronizer.AfterPlayerOptionChosen += new Action<RestSiteOption, bool, ulong>(this.OnAfterPlayerSelectedRestSiteOption);
  }

  public override void _EnterTree()
  {
    ActiveScreenContext.Instance.Updated += new Action(this.OnActiveScreenUpdated);
  }

  public override void _ExitTree()
  {
    this._cts.Cancel();
    this._descriptionTween?.Kill();
    this._descriptionPositionTween?.Kill();
    this._choicesTween?.Kill();
    RunManager.Instance.RestSiteSynchronizer.PlayerHoverChanged -= new Action<ulong>(this.OnPlayerChangedHoveredRestSiteOption);
    RunManager.Instance.RestSiteSynchronizer.BeforePlayerOptionChosen -= new Action<RestSiteOption, ulong>(this.OnBeforePlayerSelectedRestSiteOption);
    RunManager.Instance.RestSiteSynchronizer.AfterPlayerOptionChosen -= new Action<RestSiteOption, bool, ulong>(this.OnAfterPlayerSelectedRestSiteOption);
    ActiveScreenContext.Instance.Updated -= new Action(this.OnActiveScreenUpdated);
  }

  public void AfterSelectingOption(RestSiteOption option)
  {
    TaskHelper.RunSafely(this.AfterSelectingOptionAsync(option));
  }

  private async Task ShowFtueIfNeeded()
  {
    if (SaveManager.Instance.SeenFtue("rest_site_ftue"))
      return;
    ((CanvasItem) this._choicesContainer).Visible = false;
    ((CanvasItem) this.Header).Visible = false;
    await Cmd.Wait(0.5f, this._cts.Token);
    ((CanvasItem) this._choicesContainer).Visible = true;
    ((CanvasItem) this.Header).Visible = true;
    Control choicesContainer = this._choicesContainer;
    Color modulate = ((CanvasItem) this._choicesContainer).Modulate;
    modulate.A = 0.0f;
    Color color1 = modulate;
    ((CanvasItem) choicesContainer).Modulate = color1;
    MegaLabel header = this.Header;
    modulate = ((CanvasItem) this.Header).Modulate;
    modulate.A = 0.0f;
    Color color2 = modulate;
    ((CanvasItem) header).Modulate = color2;
    Tween tween = ((Node) this).CreateTween();
    tween.TweenProperty((GodotObject) this._choicesContainer, NodePath.op_Implicit("modulate:a"), Variant.op_Implicit(1f), 0.5);
    tween.TweenProperty((GodotObject) this.Header, NodePath.op_Implicit("modulate:a"), Variant.op_Implicit(1f), 0.5);
    NModalContainer.Instance.Add((Node) NRestSiteFtue.Create(this._choicesContainer));
    SaveManager.Instance.MarkFtueAsComplete("rest_site_ftue");
  }

  public void BeforeExitingRoom()
  {
    this._roomExiting = true;
    this.DisableOptions();
  }

  public void DisableOptions()
  {
    foreach (NClickableControl nclickableControl in ((IEnumerable) ((Node) this._choicesContainer).GetChildren(false)).OfType<NRestSiteButton>())
      nclickableControl.Disable();
  }

  public void EnableOptions()
  {
    if (this._roomExiting)
      return;
    foreach (NClickableControl nclickableControl in ((IEnumerable) ((Node) this._choicesContainer).GetChildren(false)).OfType<NRestSiteButton>())
      nclickableControl.Enable();
    Control defaultFocusedControl = this.DefaultFocusedControl;
    if (defaultFocusedControl == null)
      return;
    defaultFocusedControl.TryGrabFocus();
  }

  public void AnimateDescriptionDown()
  {
    this._descriptionPositionTween?.Kill();
    this._descriptionPositionTween = ((Node) this).CreateTween();
    this._descriptionPositionTween.TweenProperty((GodotObject) this.Description, NodePath.op_Implicit("position:y"), Variant.op_Implicit(885f), 0.800000011920929).SetTrans((Tween.TransitionType) 5L).SetEase((Tween.EaseType) 1L);
  }

  public void AnimateDescriptionUp()
  {
    this._descriptionPositionTween?.Kill();
    this._descriptionPositionTween = ((Node) this).CreateTween();
    this._descriptionPositionTween.TweenProperty((GodotObject) this.Description, NodePath.op_Implicit("position:y"), Variant.op_Implicit(this._originalDescriptionYPos), 0.800000011920929).SetTrans((Tween.TransitionType) 5L).SetEase((Tween.EaseType) 1L);
  }

  private void UpdateRestSiteOptions()
  {
    if (!((Node) this).IsValid() || !((Node) this).IsInsideTree())
      return;
    foreach (Node child in ((Node) this._choicesContainer).GetChildren(false))
      child.QueueFreeSafely();
    List<NRestSiteButton> nrestSiteButtonList1 = new List<NRestSiteButton>();
    foreach (RestSiteOption option in (IEnumerable<RestSiteOption>) this.Options)
    {
      NRestSiteButton child = NRestSiteButton.Create(option);
      ((Node) this._choicesContainer).AddChildSafely((Node) child);
      ((GodotObject) child).Connect(NClickableControl.SignalName.Focused, Callable.From<NRestSiteButton>(new Action<NRestSiteButton>(this.RestSiteButtonHovered)), 0U);
      ((GodotObject) child).Connect(NClickableControl.SignalName.Unfocused, Callable.From<NRestSiteButton>(new Action<NRestSiteButton>(this.RestSiteButtonUnhovered)), 0U);
      nrestSiteButtonList1.Add(child);
    }
    for (int index = 0; index < nrestSiteButtonList1.Count; ++index)
    {
      NRestSiteButton nrestSiteButton1 = nrestSiteButtonList1[index];
      NRestSiteButton nrestSiteButton2 = nrestSiteButton1;
      NodePath path;
      if (index <= 0)
      {
        List<NRestSiteButton> nrestSiteButtonList2 = nrestSiteButtonList1;
        path = ((Node) nrestSiteButtonList2[nrestSiteButtonList2.Count - 1]).GetPath();
      }
      else
        path = ((Node) nrestSiteButtonList1[index - 1]).GetPath();
      nrestSiteButton2.FocusNeighborLeft = path;
      nrestSiteButton1.FocusNeighborRight = index < nrestSiteButtonList1.Count - 1 ? ((Node) nrestSiteButtonList1[index + 1]).GetPath() : ((Node) nrestSiteButtonList1[0]).GetPath();
      nrestSiteButton1.FocusNeighborTop = ((Node) nrestSiteButton1).GetPath();
      nrestSiteButton1.FocusNeighborBottom = ((Node) nrestSiteButton1).GetPath();
    }
  }

  private void RestSiteButtonHovered(NRestSiteButton button)
  {
    RunManager.Instance.RestSiteSynchronizer.LocalOptionHovered(button.Option);
    this._lastFocused = (Control) button;
  }

  private void RestSiteButtonUnhovered(NRestSiteButton button)
  {
    RunManager.Instance.RestSiteSynchronizer.LocalOptionHovered((RestSiteOption) null);
  }

  private void OnPlayerChangedHoveredRestSiteOption(ulong playerId)
  {
    if (this._runState.Players.Count <= 1)
      return;
    NRestSiteCharacter nrestSiteCharacter = this.Characters.First<NRestSiteCharacter>((Func<NRestSiteCharacter, bool>) (c => (long) c.Player.NetId == (long) playerId));
    int? hoveredOptionIndex = RunManager.Instance.RestSiteSynchronizer.GetHoveredOptionIndex(playerId);
    RestSiteOption option = !hoveredOptionIndex.HasValue ? (RestSiteOption) null : RunManager.Instance.RestSiteSynchronizer.GetOptionsForPlayer(playerId)[hoveredOptionIndex.Value];
    nrestSiteCharacter.ShowHoveredRestSiteOption(option);
  }

  private void OnBeforePlayerSelectedRestSiteOption(RestSiteOption option, ulong playerId)
  {
    if (this._runState.Players.Count <= 1)
      return;
    this.Characters.First<NRestSiteCharacter>((Func<NRestSiteCharacter, bool>) (c => (long) c.Player.NetId == (long) playerId)).SetSelectingRestSiteOption(option);
  }

  private void OnAfterPlayerSelectedRestSiteOption(
    RestSiteOption option,
    bool success,
    ulong playerId)
  {
    if (this._runState.Players.Count <= 1)
      return;
    NRestSiteCharacter nrestSiteCharacter = this.Characters.First<NRestSiteCharacter>((Func<NRestSiteCharacter, bool>) (c => (long) c.Player.NetId == (long) playerId));
    nrestSiteCharacter.SetSelectingRestSiteOption((RestSiteOption) null);
    if (!success)
      return;
    nrestSiteCharacter.ShowSelectedRestSiteOption(option);
    if (LocalContext.IsMe(nrestSiteCharacter.Player))
      return;
    TaskHelper.RunSafely(option.DoRemotePostSelectVfx());
  }

  public NRestSiteButton? GetButtonForOption(RestSiteOption option)
  {
    foreach (NRestSiteButton buttonForOption in ((IEnumerable) ((Node) this._choicesContainer).GetChildren(false)).OfType<NRestSiteButton>())
    {
      if (buttonForOption.Option == option)
        return buttonForOption;
    }
    return (NRestSiteButton) null;
  }

  public NRestSiteCharacter? GetCharacterForPlayer(Player player)
  {
    foreach (NRestSiteCharacter characterAnim in this.characterAnims)
    {
      if (characterAnim.Player == player)
        return characterAnim;
    }
    return (NRestSiteCharacter) null;
  }

  private async Task AfterSelectingOptionAsync(RestSiteOption option)
  {
    Task task1 = this.HideChoices(this._cts.Token);
    Task task2 = option.DoLocalPostSelectVfx(this._cts.Token);
    this.ExtinguishFireIfAble();
    \u003C\u003Ey__InlineArray2<Task> buffer = new \u003C\u003Ey__InlineArray2<Task>();
    // ISSUE: reference to a compiler-generated method
    \u003CPrivateImplementationDetails\u003E.InlineArrayElementRef<\u003C\u003Ey__InlineArray2<Task>, Task>(ref buffer, 0) = task1;
    // ISSUE: reference to a compiler-generated method
    \u003CPrivateImplementationDetails\u003E.InlineArrayElementRef<\u003C\u003Ey__InlineArray2<Task>, Task>(ref buffer, 1) = task2;
    // ISSUE: reference to a compiler-generated method
    await Task.WhenAll(\u003CPrivateImplementationDetails\u003E.InlineArrayAsReadOnlySpan<\u003C\u003Ey__InlineArray2<Task>, Task>(in buffer, 2));
    this.UpdateRestSiteOptions();
    this.ShowProceedButton();
    if (this.Options.Count <= 0)
      return;
    await this.ShowChoices(this._cts.Token);
    Control defaultFocusedControl = this.DefaultFocusedControl;
    if (defaultFocusedControl == null)
      return;
    defaultFocusedControl.TryGrabFocus();
  }

  private void ShowProceedButton()
  {
    if (this._proceedButton.IsEnabled)
      return;
    this._proceedButton.Enable();
    NMapScreen.Instance.SetTravelEnabled(true);
  }

  private void OnProceedButtonReleased(NButton _) => NMapScreen.Instance.Open();

  public void SetText(string formattedText)
  {
    this._descriptionTween?.Kill();
    ((CanvasItem) this.Description).Modulate = Colors.White;
    this.Description.Text = formattedText;
  }

  public void FadeOutOptionDescription()
  {
    this._descriptionTween?.Kill();
    this._descriptionTween = ((Node) this).CreateTween();
    this._descriptionTween.TweenProperty((GodotObject) this.Description, NodePath.op_Implicit("modulate:a"), Variant.op_Implicit(0.0f), 1.0).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 5L).From(Variant.op_Implicit(1f));
  }

  public override void _Input(InputEvent inputEvent)
  {
    if (!inputEvent.IsActionReleased(DebugHotkey.hideRestSite, false))
      return;
    NRestSiteRoom._isDebugUiVisible = !NRestSiteRoom._isDebugUiVisible;
    ((CanvasItem) this._choicesScreen).Modulate = NRestSiteRoom._isDebugUiVisible ? Colors.Transparent : Colors.White;
    ((Node) NGame.Instance).AddChildSafely((Node) NFullscreenTextVfx.Create(NRestSiteRoom._isDebugUiVisible ? "Hide RestSite UI" : "Show RestSite UI"));
  }

  private void ExtinguishFireIfAble()
  {
    if (RunManager.Instance.RestSiteSynchronizer.GetLocalOptions().Count > 0 || !((CanvasItem) this._restSiteLighting).Visible)
      return;
    foreach (NRestSiteCharacter characterAnim in this.characterAnims)
      characterAnim.HideFlameGlow();
    foreach (CanvasItem characterContainer in this._characterContainers)
      characterContainer.Modulate = Colors.DarkGray;
    ((CanvasItem) this._restSiteLighting).Visible = false;
    if (this._runState.IsGameOver)
      return;
    NRunMusicController.Instance?.TriggerCampfireGoingOut();
  }

  private async Task ShowChoices(CancellationToken ct)
  {
    this._choicesTween?.Kill();
    this._choicesTween = ((Node) this).CreateTween();
    this._choicesTween.TweenProperty((GodotObject) this._choicesScreen, NodePath.op_Implicit("modulate:a"), Variant.op_Implicit(1f), 0.5).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 7L);
    await this._choicesTween.AwaitFinished(ct);
  }

  private async Task HideChoices(CancellationToken ct)
  {
    foreach (NClickableControl nclickableControl in ((IEnumerable) ((Node) this._choicesContainer).GetChildren(false)).OfType<NButton>())
      nclickableControl.Disable();
    this._choicesTween?.Kill();
    this._choicesTween = ((Node) this).CreateTween();
    this._choicesTween.TweenProperty((GodotObject) this._choicesScreen, NodePath.op_Implicit("modulate:a"), Variant.op_Implicit(0.0f), 0.5);
    this._lastFocused = (Control) null;
    await this._choicesTween.AwaitFinished(ct);
  }

  public Control? DefaultFocusedControl
  {
    get
    {
      if (this._lastFocused != null)
        return this._lastFocused;
      return ((Node) this._choicesContainer).GetChildCount(false) <= 0 ? (Control) null : (Control) ((Node) this._choicesContainer).GetChild<NRestSiteButton>(0, false);
    }
  }

  private void OnActiveScreenUpdated()
  {
    this.UpdateControllerNavEnabled<NRestSiteRoom>();
    if (ActiveScreenContext.Instance.IsCurrent((IScreenContext) this) && this.Options.Count == 0)
    {
      this.ShowProceedButton();
    }
    else
    {
      if (this._proceedButton.IsEnabled)
        return;
      this._proceedButton.Disable();
    }
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(19)
    {
      new MethodInfo(NRestSiteRoom.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NRestSiteRoom.MethodName._EnterTree, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NRestSiteRoom.MethodName._ExitTree, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NRestSiteRoom.MethodName.BeforeExitingRoom, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NRestSiteRoom.MethodName.DisableOptions, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NRestSiteRoom.MethodName.EnableOptions, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NRestSiteRoom.MethodName.AnimateDescriptionDown, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NRestSiteRoom.MethodName.AnimateDescriptionUp, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NRestSiteRoom.MethodName.UpdateRestSiteOptions, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NRestSiteRoom.MethodName.RestSiteButtonHovered, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("button"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false)
      }, (List<Variant>) null),
      new MethodInfo(NRestSiteRoom.MethodName.RestSiteButtonUnhovered, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("button"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false)
      }, (List<Variant>) null),
      new MethodInfo(NRestSiteRoom.MethodName.OnPlayerChangedHoveredRestSiteOption, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 2L, StringName.op_Implicit("playerId"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NRestSiteRoom.MethodName.ShowProceedButton, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NRestSiteRoom.MethodName.OnProceedButtonReleased, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("_"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false)
      }, (List<Variant>) null),
      new MethodInfo(NRestSiteRoom.MethodName.SetText, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 4L, StringName.op_Implicit("formattedText"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NRestSiteRoom.MethodName.FadeOutOptionDescription, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NRestSiteRoom.MethodName._Input, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("inputEvent"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("InputEvent"), false)
      }, (List<Variant>) null),
      new MethodInfo(NRestSiteRoom.MethodName.ExtinguishFireIfAble, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NRestSiteRoom.MethodName.OnActiveScreenUpdated, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NRestSiteRoom.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NRestSiteRoom.MethodName._EnterTree) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._EnterTree();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NRestSiteRoom.MethodName._ExitTree) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._ExitTree();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NRestSiteRoom.MethodName.BeforeExitingRoom) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.BeforeExitingRoom();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NRestSiteRoom.MethodName.DisableOptions) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.DisableOptions();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NRestSiteRoom.MethodName.EnableOptions) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.EnableOptions();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NRestSiteRoom.MethodName.AnimateDescriptionDown) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.AnimateDescriptionDown();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NRestSiteRoom.MethodName.AnimateDescriptionUp) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.AnimateDescriptionUp();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NRestSiteRoom.MethodName.UpdateRestSiteOptions) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.UpdateRestSiteOptions();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NRestSiteRoom.MethodName.RestSiteButtonHovered) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.RestSiteButtonHovered(VariantUtils.ConvertTo<NRestSiteButton>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NRestSiteRoom.MethodName.RestSiteButtonUnhovered) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.RestSiteButtonUnhovered(VariantUtils.ConvertTo<NRestSiteButton>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NRestSiteRoom.MethodName.OnPlayerChangedHoveredRestSiteOption) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.OnPlayerChangedHoveredRestSiteOption(VariantUtils.ConvertTo<ulong>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NRestSiteRoom.MethodName.ShowProceedButton) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.ShowProceedButton();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NRestSiteRoom.MethodName.OnProceedButtonReleased) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.OnProceedButtonReleased(VariantUtils.ConvertTo<NButton>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NRestSiteRoom.MethodName.SetText) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.SetText(VariantUtils.ConvertTo<string>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NRestSiteRoom.MethodName.FadeOutOptionDescription) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.FadeOutOptionDescription();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NRestSiteRoom.MethodName._Input) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      ((Node) this)._Input(VariantUtils.ConvertTo<InputEvent>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NRestSiteRoom.MethodName.ExtinguishFireIfAble) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.ExtinguishFireIfAble();
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NRestSiteRoom.MethodName.OnActiveScreenUpdated) || ((NativeVariantPtrArgs) ref args).Count != 0)
      return base.InvokeGodotClassMethod(ref method, args, ref ret);
    this.OnActiveScreenUpdated();
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NRestSiteRoom.MethodName._Ready) || StringName.op_Equality(ref method, NRestSiteRoom.MethodName._EnterTree) || StringName.op_Equality(ref method, NRestSiteRoom.MethodName._ExitTree) || StringName.op_Equality(ref method, NRestSiteRoom.MethodName.BeforeExitingRoom) || StringName.op_Equality(ref method, NRestSiteRoom.MethodName.DisableOptions) || StringName.op_Equality(ref method, NRestSiteRoom.MethodName.EnableOptions) || StringName.op_Equality(ref method, NRestSiteRoom.MethodName.AnimateDescriptionDown) || StringName.op_Equality(ref method, NRestSiteRoom.MethodName.AnimateDescriptionUp) || StringName.op_Equality(ref method, NRestSiteRoom.MethodName.UpdateRestSiteOptions) || StringName.op_Equality(ref method, NRestSiteRoom.MethodName.RestSiteButtonHovered) || StringName.op_Equality(ref method, NRestSiteRoom.MethodName.RestSiteButtonUnhovered) || StringName.op_Equality(ref method, NRestSiteRoom.MethodName.OnPlayerChangedHoveredRestSiteOption) || StringName.op_Equality(ref method, NRestSiteRoom.MethodName.ShowProceedButton) || StringName.op_Equality(ref method, NRestSiteRoom.MethodName.OnProceedButtonReleased) || StringName.op_Equality(ref method, NRestSiteRoom.MethodName.SetText) || StringName.op_Equality(ref method, NRestSiteRoom.MethodName.FadeOutOptionDescription) || StringName.op_Equality(ref method, NRestSiteRoom.MethodName._Input) || StringName.op_Equality(ref method, NRestSiteRoom.MethodName.ExtinguishFireIfAble) || StringName.op_Equality(ref method, NRestSiteRoom.MethodName.OnActiveScreenUpdated) || base.HasGodotClassMethod(ref method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NRestSiteRoom.PropertyName.Header))
    {
      this.Header = VariantUtils.ConvertTo<MegaLabel>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NRestSiteRoom.PropertyName.Description))
    {
      this.Description = VariantUtils.ConvertTo<MegaRichTextLabel>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NRestSiteRoom.PropertyName.BgContainer))
    {
      this.BgContainer = VariantUtils.ConvertTo<Control>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NRestSiteRoom.PropertyName._choicesContainer))
    {
      this._choicesContainer = VariantUtils.ConvertTo<Control>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NRestSiteRoom.PropertyName._choicesScreen))
    {
      this._choicesScreen = VariantUtils.ConvertTo<Control>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NRestSiteRoom.PropertyName._proceedButton))
    {
      this._proceedButton = VariantUtils.ConvertTo<NProceedButton>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NRestSiteRoom.PropertyName._restSiteLighting))
    {
      this._restSiteLighting = VariantUtils.ConvertTo<Control>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NRestSiteRoom.PropertyName._descriptionTween))
    {
      this._descriptionTween = VariantUtils.ConvertTo<Tween>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NRestSiteRoom.PropertyName._descriptionPositionTween))
    {
      this._descriptionPositionTween = VariantUtils.ConvertTo<Tween>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NRestSiteRoom.PropertyName._choicesTween))
    {
      this._choicesTween = VariantUtils.ConvertTo<Tween>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NRestSiteRoom.PropertyName._originalDescriptionYPos))
    {
      this._originalDescriptionYPos = VariantUtils.ConvertTo<float>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NRestSiteRoom.PropertyName._roomExiting))
    {
      this._roomExiting = VariantUtils.ConvertTo<bool>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NRestSiteRoom.PropertyName._lastFocused))
      return ((GodotObject) this).SetGodotClassPropertyValue(ref name, ref value);
    this._lastFocused = VariantUtils.ConvertTo<Control>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NRestSiteRoom.PropertyName.ProceedButton))
    {
      ref godot_variant local = ref value;
      NProceedButton proceedButton = this.ProceedButton;
      godot_variant from = VariantUtils.CreateFrom<NProceedButton>(ref proceedButton);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NRestSiteRoom.PropertyName.Header))
    {
      ref godot_variant local = ref value;
      MegaLabel header = this.Header;
      godot_variant from = VariantUtils.CreateFrom<MegaLabel>(ref header);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NRestSiteRoom.PropertyName.Description))
    {
      ref godot_variant local = ref value;
      MegaRichTextLabel description = this.Description;
      godot_variant from = VariantUtils.CreateFrom<MegaRichTextLabel>(ref description);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NRestSiteRoom.PropertyName.BgContainer))
    {
      ref godot_variant local = ref value;
      Control bgContainer = this.BgContainer;
      godot_variant from = VariantUtils.CreateFrom<Control>(ref bgContainer);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NRestSiteRoom.PropertyName.DefaultFocusedControl))
    {
      ref godot_variant local = ref value;
      Control defaultFocusedControl = this.DefaultFocusedControl;
      godot_variant from = VariantUtils.CreateFrom<Control>(ref defaultFocusedControl);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NRestSiteRoom.PropertyName._choicesContainer))
    {
      value = VariantUtils.CreateFrom<Control>(ref this._choicesContainer);
      return true;
    }
    if (StringName.op_Equality(ref name, NRestSiteRoom.PropertyName._choicesScreen))
    {
      value = VariantUtils.CreateFrom<Control>(ref this._choicesScreen);
      return true;
    }
    if (StringName.op_Equality(ref name, NRestSiteRoom.PropertyName._proceedButton))
    {
      value = VariantUtils.CreateFrom<NProceedButton>(ref this._proceedButton);
      return true;
    }
    if (StringName.op_Equality(ref name, NRestSiteRoom.PropertyName._restSiteLighting))
    {
      value = VariantUtils.CreateFrom<Control>(ref this._restSiteLighting);
      return true;
    }
    if (StringName.op_Equality(ref name, NRestSiteRoom.PropertyName._descriptionTween))
    {
      value = VariantUtils.CreateFrom<Tween>(ref this._descriptionTween);
      return true;
    }
    if (StringName.op_Equality(ref name, NRestSiteRoom.PropertyName._descriptionPositionTween))
    {
      value = VariantUtils.CreateFrom<Tween>(ref this._descriptionPositionTween);
      return true;
    }
    if (StringName.op_Equality(ref name, NRestSiteRoom.PropertyName._choicesTween))
    {
      value = VariantUtils.CreateFrom<Tween>(ref this._choicesTween);
      return true;
    }
    if (StringName.op_Equality(ref name, NRestSiteRoom.PropertyName._originalDescriptionYPos))
    {
      value = VariantUtils.CreateFrom<float>(ref this._originalDescriptionYPos);
      return true;
    }
    if (StringName.op_Equality(ref name, NRestSiteRoom.PropertyName._roomExiting))
    {
      value = VariantUtils.CreateFrom<bool>(ref this._roomExiting);
      return true;
    }
    if (!StringName.op_Equality(ref name, NRestSiteRoom.PropertyName._lastFocused))
      return ((GodotObject) this).GetGodotClassPropertyValue(ref name, ref value);
    value = VariantUtils.CreateFrom<Control>(ref this._lastFocused);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NRestSiteRoom.PropertyName._choicesContainer, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NRestSiteRoom.PropertyName._choicesScreen, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NRestSiteRoom.PropertyName._proceedButton, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NRestSiteRoom.PropertyName.ProceedButton, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NRestSiteRoom.PropertyName._restSiteLighting, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NRestSiteRoom.PropertyName._descriptionTween, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NRestSiteRoom.PropertyName._descriptionPositionTween, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NRestSiteRoom.PropertyName._choicesTween, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 3L, NRestSiteRoom.PropertyName._originalDescriptionYPos, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 1L, NRestSiteRoom.PropertyName._roomExiting, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NRestSiteRoom.PropertyName._lastFocused, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NRestSiteRoom.PropertyName.Header, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NRestSiteRoom.PropertyName.Description, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NRestSiteRoom.PropertyName.BgContainer, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NRestSiteRoom.PropertyName.DefaultFocusedControl, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).SaveGodotObjectData(info);
    GodotSerializationInfo serializationInfo1 = info;
    StringName header1 = NRestSiteRoom.PropertyName.Header;
    MegaLabel header2 = this.Header;
    Variant variant1 = Variant.From<MegaLabel>(ref header2);
    serializationInfo1.AddProperty(header1, variant1);
    GodotSerializationInfo serializationInfo2 = info;
    StringName description1 = NRestSiteRoom.PropertyName.Description;
    MegaRichTextLabel description2 = this.Description;
    Variant variant2 = Variant.From<MegaRichTextLabel>(ref description2);
    serializationInfo2.AddProperty(description1, variant2);
    GodotSerializationInfo serializationInfo3 = info;
    StringName bgContainer1 = NRestSiteRoom.PropertyName.BgContainer;
    Control bgContainer2 = this.BgContainer;
    Variant variant3 = Variant.From<Control>(ref bgContainer2);
    serializationInfo3.AddProperty(bgContainer1, variant3);
    info.AddProperty(NRestSiteRoom.PropertyName._choicesContainer, Variant.From<Control>(ref this._choicesContainer));
    info.AddProperty(NRestSiteRoom.PropertyName._choicesScreen, Variant.From<Control>(ref this._choicesScreen));
    info.AddProperty(NRestSiteRoom.PropertyName._proceedButton, Variant.From<NProceedButton>(ref this._proceedButton));
    info.AddProperty(NRestSiteRoom.PropertyName._restSiteLighting, Variant.From<Control>(ref this._restSiteLighting));
    info.AddProperty(NRestSiteRoom.PropertyName._descriptionTween, Variant.From<Tween>(ref this._descriptionTween));
    info.AddProperty(NRestSiteRoom.PropertyName._descriptionPositionTween, Variant.From<Tween>(ref this._descriptionPositionTween));
    info.AddProperty(NRestSiteRoom.PropertyName._choicesTween, Variant.From<Tween>(ref this._choicesTween));
    info.AddProperty(NRestSiteRoom.PropertyName._originalDescriptionYPos, Variant.From<float>(ref this._originalDescriptionYPos));
    info.AddProperty(NRestSiteRoom.PropertyName._roomExiting, Variant.From<bool>(ref this._roomExiting));
    info.AddProperty(NRestSiteRoom.PropertyName._lastFocused, Variant.From<Control>(ref this._lastFocused));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NRestSiteRoom.PropertyName.Header, ref variant1))
      this.Header = ((Variant) ref variant1).As<MegaLabel>();
    Variant variant2;
    if (info.TryGetProperty(NRestSiteRoom.PropertyName.Description, ref variant2))
      this.Description = ((Variant) ref variant2).As<MegaRichTextLabel>();
    Variant variant3;
    if (info.TryGetProperty(NRestSiteRoom.PropertyName.BgContainer, ref variant3))
      this.BgContainer = ((Variant) ref variant3).As<Control>();
    Variant variant4;
    if (info.TryGetProperty(NRestSiteRoom.PropertyName._choicesContainer, ref variant4))
      this._choicesContainer = ((Variant) ref variant4).As<Control>();
    Variant variant5;
    if (info.TryGetProperty(NRestSiteRoom.PropertyName._choicesScreen, ref variant5))
      this._choicesScreen = ((Variant) ref variant5).As<Control>();
    Variant variant6;
    if (info.TryGetProperty(NRestSiteRoom.PropertyName._proceedButton, ref variant6))
      this._proceedButton = ((Variant) ref variant6).As<NProceedButton>();
    Variant variant7;
    if (info.TryGetProperty(NRestSiteRoom.PropertyName._restSiteLighting, ref variant7))
      this._restSiteLighting = ((Variant) ref variant7).As<Control>();
    Variant variant8;
    if (info.TryGetProperty(NRestSiteRoom.PropertyName._descriptionTween, ref variant8))
      this._descriptionTween = ((Variant) ref variant8).As<Tween>();
    Variant variant9;
    if (info.TryGetProperty(NRestSiteRoom.PropertyName._descriptionPositionTween, ref variant9))
      this._descriptionPositionTween = ((Variant) ref variant9).As<Tween>();
    Variant variant10;
    if (info.TryGetProperty(NRestSiteRoom.PropertyName._choicesTween, ref variant10))
      this._choicesTween = ((Variant) ref variant10).As<Tween>();
    Variant variant11;
    if (info.TryGetProperty(NRestSiteRoom.PropertyName._originalDescriptionYPos, ref variant11))
      this._originalDescriptionYPos = ((Variant) ref variant11).As<float>();
    Variant variant12;
    if (info.TryGetProperty(NRestSiteRoom.PropertyName._roomExiting, ref variant12))
      this._roomExiting = ((Variant) ref variant12).As<bool>();
    Variant variant13;
    if (!info.TryGetProperty(NRestSiteRoom.PropertyName._lastFocused, ref variant13))
      return;
    this._lastFocused = ((Variant) ref variant13).As<Control>();
  }

  public class MethodName : Control.MethodName
  {
    public static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName _EnterTree = StringName.op_Implicit(nameof (_EnterTree));
    public static readonly StringName _ExitTree = StringName.op_Implicit(nameof (_ExitTree));
    public static readonly StringName BeforeExitingRoom = StringName.op_Implicit(nameof (BeforeExitingRoom));
    public static readonly StringName DisableOptions = StringName.op_Implicit(nameof (DisableOptions));
    public static readonly StringName EnableOptions = StringName.op_Implicit(nameof (EnableOptions));
    public static readonly StringName AnimateDescriptionDown = StringName.op_Implicit(nameof (AnimateDescriptionDown));
    public static readonly StringName AnimateDescriptionUp = StringName.op_Implicit(nameof (AnimateDescriptionUp));
    public static readonly StringName UpdateRestSiteOptions = StringName.op_Implicit(nameof (UpdateRestSiteOptions));
    public static readonly StringName RestSiteButtonHovered = StringName.op_Implicit(nameof (RestSiteButtonHovered));
    public static readonly StringName RestSiteButtonUnhovered = StringName.op_Implicit(nameof (RestSiteButtonUnhovered));
    public static readonly StringName OnPlayerChangedHoveredRestSiteOption = StringName.op_Implicit(nameof (OnPlayerChangedHoveredRestSiteOption));
    public static readonly StringName ShowProceedButton = StringName.op_Implicit(nameof (ShowProceedButton));
    public static readonly StringName OnProceedButtonReleased = StringName.op_Implicit(nameof (OnProceedButtonReleased));
    public static readonly StringName SetText = StringName.op_Implicit(nameof (SetText));
    public static readonly StringName FadeOutOptionDescription = StringName.op_Implicit(nameof (FadeOutOptionDescription));
    public static readonly StringName _Input = StringName.op_Implicit(nameof (_Input));
    public static readonly StringName ExtinguishFireIfAble = StringName.op_Implicit(nameof (ExtinguishFireIfAble));
    public static readonly StringName OnActiveScreenUpdated = StringName.op_Implicit(nameof (OnActiveScreenUpdated));
  }

  public class PropertyName : Control.PropertyName
  {
    public static readonly StringName ProceedButton = StringName.op_Implicit(nameof (ProceedButton));
    public static readonly StringName Header = StringName.op_Implicit(nameof (Header));
    public static readonly StringName Description = StringName.op_Implicit(nameof (Description));
    public static readonly StringName BgContainer = StringName.op_Implicit(nameof (BgContainer));
    public static readonly StringName DefaultFocusedControl = StringName.op_Implicit(nameof (DefaultFocusedControl));
    public static readonly StringName _choicesContainer = StringName.op_Implicit(nameof (_choicesContainer));
    public static readonly StringName _choicesScreen = StringName.op_Implicit(nameof (_choicesScreen));
    public static readonly StringName _proceedButton = StringName.op_Implicit(nameof (_proceedButton));
    public static readonly StringName _restSiteLighting = StringName.op_Implicit(nameof (_restSiteLighting));
    public static readonly StringName _descriptionTween = StringName.op_Implicit(nameof (_descriptionTween));
    public static readonly StringName _descriptionPositionTween = StringName.op_Implicit(nameof (_descriptionPositionTween));
    public static readonly StringName _choicesTween = StringName.op_Implicit(nameof (_choicesTween));
    public static readonly StringName _originalDescriptionYPos = StringName.op_Implicit(nameof (_originalDescriptionYPos));
    public static readonly StringName _roomExiting = StringName.op_Implicit(nameof (_roomExiting));
    public static readonly StringName _lastFocused = StringName.op_Implicit(nameof (_lastFocused));
  }

  public class SignalName : Control.SignalName
  {
  }
}
