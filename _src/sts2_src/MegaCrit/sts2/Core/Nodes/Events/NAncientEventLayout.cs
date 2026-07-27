// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Events.NAncientEventLayout
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Ancients;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Screens;
using MegaCrit.Sts2.Core.Nodes.Screens.ScreenContext;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.Saves;
using MegaCrit.Sts2.Core.Settings;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Events;

[ScriptPath("res://src/Core/Nodes/Events/NAncientEventLayout.cs")]
public class NAncientEventLayout : NEventLayout
{
  public const string ancientScenePath = "res://scenes/events/ancient_event_layout.tscn";
  private const double _contentTweenDuration = 1.0;
  private AncientEventModel _ancientEvent;
  private readonly List<AncientDialogueLine> _dialogue = new List<AncientDialogueLine>();
  private int _currentDialogueLine;
  private NAncientBgContainer _ancientBgContainer;
  private Control? _ancientNameBanner;
  private Tween? _bannerTween;
  private Control _contentContainer;
  private float _originalContentContainerHeight;
  private VBoxContainer _content;
  private VBoxContainer _dialogueContainer;
  private NAncientDialogueHitbox _dialogueHitbox;
  private Control _fakeNextButtonContainer;
  private Control _fakeNextButton;
  private TextureRect _fakeNextButtonControllerIcon;
  private MegaLabel _fakeNextButtonLabel;
  private Tween? _contentTween;
  private CancellationTokenSource _cts = new CancellationTokenSource();

  public override void _Ready()
  {
    base._Ready();
    this._ancientBgContainer = ((Node) this).GetNode<NAncientBgContainer>(NodePath.op_Implicit("%AncientBgContainer"));
    this._contentContainer = ((Node) this).GetNode<Control>(NodePath.op_Implicit("%ContentContainer"));
    this._content = ((Node) this).GetNode<VBoxContainer>(NodePath.op_Implicit("%Content"));
    this._dialogueContainer = ((Node) this).GetNode<VBoxContainer>(NodePath.op_Implicit("%DialogueContainer"));
    this._dialogueHitbox = ((Node) this).GetNode<NAncientDialogueHitbox>(NodePath.op_Implicit("%DialogueHitbox"));
    ((GodotObject) this._dialogueHitbox).Connect(NClickableControl.SignalName.Released, Callable.From<NClickableControl>(new Action<NClickableControl>(this.OnDialogueHitboxClicked)), 0U);
    ((CanvasItem) this._dialogueHitbox).Visible = false;
    this._dialogueHitbox.Disable();
    this._fakeNextButtonContainer = ((Node) this).GetNode<Control>(NodePath.op_Implicit("%FakeNextButtonContainer"));
    this._fakeNextButton = ((Node) this._fakeNextButtonContainer).GetNode<Control>(NodePath.op_Implicit("FakeNextButton"));
    this._fakeNextButtonLabel = ((Node) this._fakeNextButton).GetNode<MegaLabel>(NodePath.op_Implicit("Label"));
    this._fakeNextButtonControllerIcon = ((Node) this._fakeNextButton).GetNode<TextureRect>(NodePath.op_Implicit("ControllerIcon"));
    this._originalContentContainerHeight = this._contentContainer.Size.Y;
    this._contentContainer.Size = new Vector2(this._contentContainer.Size.X, this._fakeNextButtonContainer.GlobalPosition.Y - this._contentContainer.GlobalPosition.Y);
    this.UpdateHotkeyDisplay();
    ((GodotObject) NControllerManager.Instance).Connect(NControllerManager.SignalName.MouseDetected, Callable.From(new Action(this.UpdateHotkeyDisplay)), 0U);
    ((GodotObject) NControllerManager.Instance).Connect(NControllerManager.SignalName.ControllerDetected, Callable.From(new Action(this.UpdateHotkeyDisplay)), 0U);
    ((GodotObject) NInputManager.Instance).Connect(NInputManager.SignalName.InputRebound, Callable.From(new Action(this.UpdateHotkeyDisplay)), 0U);
  }

  public override void _EnterTree()
  {
    base._EnterTree();
    this._cts = new CancellationTokenSource();
    ActiveScreenContext.Instance.Updated += new Action(this.UpdateBannerVisibility);
  }

  public override void _ExitTree()
  {
    base._ExitTree();
    this._cts.Cancel();
    if (this._ancientEvent.HasAmbientBgm)
      SfxCmd.StopLoop(this._ancientEvent.AmbientBgm);
    ActiveScreenContext.Instance.Updated -= new Action(this.UpdateBannerVisibility);
    ((GodotObject) NControllerManager.Instance).Disconnect(NControllerManager.SignalName.MouseDetected, Callable.From(new Action(this.UpdateHotkeyDisplay)));
    ((GodotObject) NControllerManager.Instance).Disconnect(NControllerManager.SignalName.ControllerDetected, Callable.From(new Action(this.UpdateHotkeyDisplay)));
    ((GodotObject) NInputManager.Instance).Disconnect(NInputManager.SignalName.InputRebound, Callable.From(new Action(this.UpdateHotkeyDisplay)));
  }

  protected override void InitializeVisuals()
  {
    this._ancientEvent = (AncientEventModel) this._event;
    this._ancientNameBanner = (Control) NAncientNameBanner.Create(this._ancientEvent);
    ((Node) this).AddChildSafely((Node) this._ancientNameBanner);
    this.UpdateBannerVisibility();
    AncientEventModel ancientEvent = this._ancientEvent;
    if (ancientEvent != null && ancientEvent.Owner != null && ancientEvent.HealedAmount > 0)
      TaskHelper.RunSafely(this.PlayHealVfxAfterFadeIn(this._ancientEvent.Owner, (Decimal) this._ancientEvent.HealedAmount));
    foreach (Node child in ((Node) this._ancientBgContainer).GetChildren(false))
      ((Node) this._ancientBgContainer).RemoveChildSafely(child);
    ((Node) this._ancientBgContainer).AddChildSafely((Node) this._ancientEvent.CreateBackgroundScene().Instantiate<Control>((PackedScene.GenEditState) 0L));
    if (!this._ancientEvent.HasAmbientBgm)
      return;
    SfxCmd.PlayLoop(this._ancientEvent.AmbientBgm);
  }

  protected override void AnimateIn()
  {
    if (this._description == null)
      return;
    ((CanvasItem) this._description).Modulate = Colors.Transparent;
    this._descriptionTween?.Kill();
    this._descriptionTween = ((Node) this).CreateTween().SetParallel(true);
    if (SaveManager.Instance.PrefsSave.FastMode == FastModeType.Fast)
      this._descriptionTween.TweenInterval(0.2);
    else
      this._descriptionTween.TweenInterval(0.5);
    this._descriptionTween.Chain();
    this._descriptionTween.TweenProperty((GodotObject) this._description, NodePath.op_Implicit("modulate"), Variant.op_Implicit(Colors.White), 1.0);
  }

  public void SetDialogue(IReadOnlyList<AncientDialogueLine> lines)
  {
    this._dialogue.Clear();
    this._dialogue.AddRange((IEnumerable<AncientDialogueLine>) lines);
    this._currentDialogueLine = 0;
    foreach (AncientDialogueLine line in (IEnumerable<AncientDialogueLine>) lines)
    {
      NAncientDialogueLine child = NAncientDialogueLine.Create(line, this._ancientEvent, this._ancientEvent.Owner.Character);
      ((GodotObject) child).Connect(NClickableControl.SignalName.Focused, Callable.From<NClickableControl>(new Action<NClickableControl>(this.OnDialogueLineFocused)), 0U);
      ((GodotObject) child).Connect(NClickableControl.SignalName.Unfocused, Callable.From<NClickableControl>(new Action<NClickableControl>(this.OnDialogueLineUnfocused)), 0U);
      ((Node) this._dialogueContainer).AddChildSafely((Node) child);
    }
  }

  private void OnDialogueLineFocused(NClickableControl dialogueLine)
  {
    if (((Node) this._dialogueContainer).GetChild<NAncientDialogueLine>(this._currentDialogueLine, false) == dialogueLine)
      return;
    for (int index = 0; index < this._currentDialogueLine; ++index)
      ((Node) this._dialogueContainer).GetChild<NAncientDialogueLine>(index, false).FadeInStaleDialogue();
  }

  private void OnDialogueLineUnfocused(NClickableControl dialogueLine)
  {
    if (((object) ((Node) this._dialogueContainer).GetChild<NAncientDialogueLine>(this._currentDialogueLine, false)).Equals((object) dialogueLine))
      return;
    for (int index = 0; index < this._currentDialogueLine; ++index)
      ((Node) this._dialogueContainer).GetChild<NAncientDialogueLine>(index, false).FadeOutStaleDialogue();
  }

  public void ClearDialogue()
  {
    this._dialogue.Clear();
    this._currentDialogueLine = 0;
    foreach (Node node in ((IEnumerable<Node>) ((Node) this._dialogueContainer).GetChildren(false)).ToList<Node>())
    {
      ((Node) this._dialogueContainer).RemoveChildSafely(node);
      node.QueueFreeSafely();
    }
  }

  public override void OnSetupComplete()
  {
    ((Control) this._dialogueContainer).ResetSize();
    ((Control) this._optionsContainer).ResetSize();
    ((Control) this._content).ResetSize();
    ((Control) this._content).Position = new Vector2(((Control) this._content).Position.X, this._contentContainer.Size.Y);
    this.SetDialogueLineAndAnimate(0);
  }

  protected override void AnimateButtonsIn()
  {
    foreach (NEventOptionButton optionButton in this.OptionButtons)
    {
      ((CanvasItem) optionButton).Modulate = Colors.White;
      optionButton.EnableButton();
    }
  }

  private async Task PlayHealVfxAfterFadeIn(Player player, Decimal healAmount)
  {
    await Cmd.Wait(0.2f, this._cts.Token);
    PlayerFullscreenHealVfx.Play(player, healAmount, this.VfxContainer);
  }

  private void OnDialogueHitboxClicked(NClickableControl _)
  {
    this.SetDialogueLineAndAnimate(this._currentDialogueLine + 1);
  }

  private void SetDialogueLineAndAnimate(int lineIndex)
  {
    this._currentDialogueLine = lineIndex;
    if (this._contentTween != null)
    {
      this._contentTween.Pause();
      this._contentTween.CustomStep(1.0);
      this._contentTween.Kill();
      this._contentTween = (Tween) null;
    }
    this.UpdateFakeNextButton();
    NAncientDialogueLine childOrNull = ((Node) this._dialogueContainer).GetChildOrNull<NAncientDialogueLine>(this._currentDialogueLine, false);
    float num = 0.0f;
    if (childOrNull != null)
    {
      childOrNull.OnAnimInSetVisible();
      childOrNull.PlaySfx();
      num = childOrNull.Position.Y + childOrNull.Size.Y;
    }
    if (this.IsDialogueOnLastLine)
    {
      ((CanvasItem) this._fakeNextButtonContainer).Visible = false;
      this._contentContainer.Size = new Vector2(this._contentContainer.Size.X, this._originalContentContainerHeight);
      ((CanvasItem) this._dialogueHitbox).Visible = false;
      this._dialogueHitbox.Disable();
      foreach (NEventOptionButton optionButton in this.OptionButtons)
        optionButton.EnableButton();
      num += ((Control) this._optionsContainer).Size.Y + 10f;
    }
    else
    {
      ((CanvasItem) this._fakeNextButtonContainer).Visible = true;
      ((CanvasItem) this._dialogueHitbox).Visible = true;
      this._dialogueHitbox.Enable();
    }
    if (((Node) this._dialogueContainer).GetChildCount(false) > this._currentDialogueLine)
      ((Node) this._dialogueContainer).GetChild<NAncientDialogueLine>(this._currentDialogueLine, false).SetSpeakerIconVisible();
    foreach (Control optionButton in this.OptionButtons)
      optionButton.FocusMode = (Control.FocusModeEnum) 0L;
    this._contentTween = ((Node) this).CreateTween();
    this._contentTween.TweenProperty((GodotObject) this._content, NodePath.op_Implicit("position"), Variant.op_Implicit(new Vector2(((Control) this._content).Position.X, this._contentContainer.Size.Y - num)), 1.0).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 5L);
    if (this.IsDialogueOnLastLine)
      this._contentTween.Parallel().TweenCallback(Callable.From((Action) (() =>
      {
        foreach (Control optionButton in this.OptionButtons)
          optionButton.FocusMode = (Control.FocusModeEnum) 2L;
        Control defaultFocusedControl = this.DefaultFocusedControl;
        if (defaultFocusedControl == null)
          return;
        defaultFocusedControl.TryGrabFocus();
      }))).SetDelay(0.8);
    for (int index = 0; index < this._currentDialogueLine; ++index)
      ((Node) this._dialogueContainer).GetChild<NAncientDialogueLine>(index, false).SetTransparency(index != this._currentDialogueLine ? 0.25f : 1f);
  }

  private void UpdateFakeNextButton()
  {
    LocString nextButtonText = this._dialogue.Count > this._currentDialogueLine ? this._dialogue[this._currentDialogueLine].NextButtonText : (LocString) null;
    if (nextButtonText != null)
      this._fakeNextButtonLabel.SetTextAutoSize(nextButtonText.GetFormattedText() ?? "");
    else
      ((CanvasItem) this._fakeNextButtonLabel).Visible = false;
  }

  private bool IsDialogueOnLastLine => this._currentDialogueLine >= this._dialogue.Count - 1;

  private void HideNameBanner()
  {
    if (this._ancientNameBanner == null)
      return;
    this._bannerTween?.Kill();
    this._bannerTween = ((Node) this).CreateTween();
    this._bannerTween.TweenProperty((GodotObject) this._ancientNameBanner, NodePath.op_Implicit("modulate:a"), Variant.op_Implicit(0.0f), 0.25);
  }

  private void ShowNameBanner()
  {
    if (this._ancientNameBanner == null)
      return;
    this._bannerTween?.Kill();
    this._bannerTween = ((Node) this).CreateTween();
    this._bannerTween.TweenProperty((GodotObject) this._ancientNameBanner, NodePath.op_Implicit("modulate:a"), Variant.op_Implicit(1f), 0.5);
  }

  private void UpdateBannerVisibility()
  {
    if (NEventRoom.Instance == null)
      return;
    if (ActiveScreenContext.Instance.IsCurrent((IScreenContext) NEventRoom.Instance))
      this.ShowNameBanner();
    else
      this.HideNameBanner();
  }

  public override Control? DefaultFocusedControl
  {
    get
    {
      return !this.IsDialogueOnLastLine ? (Control) null : (Control) this.OptionButtons.FirstOrDefault<NEventOptionButton>();
    }
  }

  private void UpdateHotkeyDisplay()
  {
    ((CanvasItem) this._fakeNextButtonControllerIcon).Visible = NControllerManager.Instance.IsUsingController;
    string hotkey = this._dialogueHitbox.GetHotkey();
    if (hotkey == null)
      return;
    this._fakeNextButtonControllerIcon.Texture = NInputManager.Instance.GetHotkeyIcon(hotkey);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(17)
    {
      new MethodInfo(NAncientEventLayout.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NAncientEventLayout.MethodName._EnterTree, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NAncientEventLayout.MethodName._ExitTree, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NAncientEventLayout.MethodName.InitializeVisuals, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NAncientEventLayout.MethodName.AnimateIn, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NAncientEventLayout.MethodName.OnDialogueLineFocused, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("dialogueLine"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false)
      }, (List<Variant>) null),
      new MethodInfo(NAncientEventLayout.MethodName.OnDialogueLineUnfocused, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("dialogueLine"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false)
      }, (List<Variant>) null),
      new MethodInfo(NAncientEventLayout.MethodName.ClearDialogue, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NAncientEventLayout.MethodName.OnSetupComplete, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NAncientEventLayout.MethodName.AnimateButtonsIn, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NAncientEventLayout.MethodName.OnDialogueHitboxClicked, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("_"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false)
      }, (List<Variant>) null),
      new MethodInfo(NAncientEventLayout.MethodName.SetDialogueLineAndAnimate, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 2L, StringName.op_Implicit("lineIndex"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NAncientEventLayout.MethodName.UpdateFakeNextButton, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NAncientEventLayout.MethodName.HideNameBanner, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NAncientEventLayout.MethodName.ShowNameBanner, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NAncientEventLayout.MethodName.UpdateBannerVisibility, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NAncientEventLayout.MethodName.UpdateHotkeyDisplay, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NAncientEventLayout.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NAncientEventLayout.MethodName._EnterTree) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._EnterTree();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NAncientEventLayout.MethodName._ExitTree) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._ExitTree();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NAncientEventLayout.MethodName.InitializeVisuals) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.InitializeVisuals();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NAncientEventLayout.MethodName.AnimateIn) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.AnimateIn();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NAncientEventLayout.MethodName.OnDialogueLineFocused) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.OnDialogueLineFocused(VariantUtils.ConvertTo<NClickableControl>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NAncientEventLayout.MethodName.OnDialogueLineUnfocused) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.OnDialogueLineUnfocused(VariantUtils.ConvertTo<NClickableControl>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NAncientEventLayout.MethodName.ClearDialogue) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.ClearDialogue();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NAncientEventLayout.MethodName.OnSetupComplete) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnSetupComplete();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NAncientEventLayout.MethodName.AnimateButtonsIn) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.AnimateButtonsIn();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NAncientEventLayout.MethodName.OnDialogueHitboxClicked) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.OnDialogueHitboxClicked(VariantUtils.ConvertTo<NClickableControl>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NAncientEventLayout.MethodName.SetDialogueLineAndAnimate) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.SetDialogueLineAndAnimate(VariantUtils.ConvertTo<int>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NAncientEventLayout.MethodName.UpdateFakeNextButton) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.UpdateFakeNextButton();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NAncientEventLayout.MethodName.HideNameBanner) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.HideNameBanner();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NAncientEventLayout.MethodName.ShowNameBanner) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.ShowNameBanner();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NAncientEventLayout.MethodName.UpdateBannerVisibility) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.UpdateBannerVisibility();
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NAncientEventLayout.MethodName.UpdateHotkeyDisplay) || ((NativeVariantPtrArgs) ref args).Count != 0)
      return base.InvokeGodotClassMethod(in method, args, out ret);
    this.UpdateHotkeyDisplay();
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NAncientEventLayout.MethodName._Ready) || StringName.op_Equality(ref method, NAncientEventLayout.MethodName._EnterTree) || StringName.op_Equality(ref method, NAncientEventLayout.MethodName._ExitTree) || StringName.op_Equality(ref method, NAncientEventLayout.MethodName.InitializeVisuals) || StringName.op_Equality(ref method, NAncientEventLayout.MethodName.AnimateIn) || StringName.op_Equality(ref method, NAncientEventLayout.MethodName.OnDialogueLineFocused) || StringName.op_Equality(ref method, NAncientEventLayout.MethodName.OnDialogueLineUnfocused) || StringName.op_Equality(ref method, NAncientEventLayout.MethodName.ClearDialogue) || StringName.op_Equality(ref method, NAncientEventLayout.MethodName.OnSetupComplete) || StringName.op_Equality(ref method, NAncientEventLayout.MethodName.AnimateButtonsIn) || StringName.op_Equality(ref method, NAncientEventLayout.MethodName.OnDialogueHitboxClicked) || StringName.op_Equality(ref method, NAncientEventLayout.MethodName.SetDialogueLineAndAnimate) || StringName.op_Equality(ref method, NAncientEventLayout.MethodName.UpdateFakeNextButton) || StringName.op_Equality(ref method, NAncientEventLayout.MethodName.HideNameBanner) || StringName.op_Equality(ref method, NAncientEventLayout.MethodName.ShowNameBanner) || StringName.op_Equality(ref method, NAncientEventLayout.MethodName.UpdateBannerVisibility) || StringName.op_Equality(ref method, NAncientEventLayout.MethodName.UpdateHotkeyDisplay) || base.HasGodotClassMethod(in method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NAncientEventLayout.PropertyName._currentDialogueLine))
    {
      this._currentDialogueLine = VariantUtils.ConvertTo<int>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NAncientEventLayout.PropertyName._ancientBgContainer))
    {
      this._ancientBgContainer = VariantUtils.ConvertTo<NAncientBgContainer>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NAncientEventLayout.PropertyName._ancientNameBanner))
    {
      this._ancientNameBanner = VariantUtils.ConvertTo<Control>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NAncientEventLayout.PropertyName._bannerTween))
    {
      this._bannerTween = VariantUtils.ConvertTo<Tween>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NAncientEventLayout.PropertyName._contentContainer))
    {
      this._contentContainer = VariantUtils.ConvertTo<Control>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NAncientEventLayout.PropertyName._originalContentContainerHeight))
    {
      this._originalContentContainerHeight = VariantUtils.ConvertTo<float>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NAncientEventLayout.PropertyName._content))
    {
      this._content = VariantUtils.ConvertTo<VBoxContainer>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NAncientEventLayout.PropertyName._dialogueContainer))
    {
      this._dialogueContainer = VariantUtils.ConvertTo<VBoxContainer>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NAncientEventLayout.PropertyName._dialogueHitbox))
    {
      this._dialogueHitbox = VariantUtils.ConvertTo<NAncientDialogueHitbox>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NAncientEventLayout.PropertyName._fakeNextButtonContainer))
    {
      this._fakeNextButtonContainer = VariantUtils.ConvertTo<Control>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NAncientEventLayout.PropertyName._fakeNextButton))
    {
      this._fakeNextButton = VariantUtils.ConvertTo<Control>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NAncientEventLayout.PropertyName._fakeNextButtonControllerIcon))
    {
      this._fakeNextButtonControllerIcon = VariantUtils.ConvertTo<TextureRect>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NAncientEventLayout.PropertyName._fakeNextButtonLabel))
    {
      this._fakeNextButtonLabel = VariantUtils.ConvertTo<MegaLabel>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NAncientEventLayout.PropertyName._contentTween))
      return base.SetGodotClassPropertyValue(in name, in value);
    this._contentTween = VariantUtils.ConvertTo<Tween>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NAncientEventLayout.PropertyName.IsDialogueOnLastLine))
    {
      ref godot_variant local = ref value;
      bool dialogueOnLastLine = this.IsDialogueOnLastLine;
      godot_variant from = VariantUtils.CreateFrom<bool>(ref dialogueOnLastLine);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NAncientEventLayout.PropertyName.DefaultFocusedControl))
    {
      ref godot_variant local = ref value;
      Control defaultFocusedControl = this.DefaultFocusedControl;
      godot_variant from = VariantUtils.CreateFrom<Control>(ref defaultFocusedControl);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NAncientEventLayout.PropertyName._currentDialogueLine))
    {
      value = VariantUtils.CreateFrom<int>(ref this._currentDialogueLine);
      return true;
    }
    if (StringName.op_Equality(ref name, NAncientEventLayout.PropertyName._ancientBgContainer))
    {
      value = VariantUtils.CreateFrom<NAncientBgContainer>(ref this._ancientBgContainer);
      return true;
    }
    if (StringName.op_Equality(ref name, NAncientEventLayout.PropertyName._ancientNameBanner))
    {
      value = VariantUtils.CreateFrom<Control>(ref this._ancientNameBanner);
      return true;
    }
    if (StringName.op_Equality(ref name, NAncientEventLayout.PropertyName._bannerTween))
    {
      value = VariantUtils.CreateFrom<Tween>(ref this._bannerTween);
      return true;
    }
    if (StringName.op_Equality(ref name, NAncientEventLayout.PropertyName._contentContainer))
    {
      value = VariantUtils.CreateFrom<Control>(ref this._contentContainer);
      return true;
    }
    if (StringName.op_Equality(ref name, NAncientEventLayout.PropertyName._originalContentContainerHeight))
    {
      value = VariantUtils.CreateFrom<float>(ref this._originalContentContainerHeight);
      return true;
    }
    if (StringName.op_Equality(ref name, NAncientEventLayout.PropertyName._content))
    {
      value = VariantUtils.CreateFrom<VBoxContainer>(ref this._content);
      return true;
    }
    if (StringName.op_Equality(ref name, NAncientEventLayout.PropertyName._dialogueContainer))
    {
      value = VariantUtils.CreateFrom<VBoxContainer>(ref this._dialogueContainer);
      return true;
    }
    if (StringName.op_Equality(ref name, NAncientEventLayout.PropertyName._dialogueHitbox))
    {
      value = VariantUtils.CreateFrom<NAncientDialogueHitbox>(ref this._dialogueHitbox);
      return true;
    }
    if (StringName.op_Equality(ref name, NAncientEventLayout.PropertyName._fakeNextButtonContainer))
    {
      value = VariantUtils.CreateFrom<Control>(ref this._fakeNextButtonContainer);
      return true;
    }
    if (StringName.op_Equality(ref name, NAncientEventLayout.PropertyName._fakeNextButton))
    {
      value = VariantUtils.CreateFrom<Control>(ref this._fakeNextButton);
      return true;
    }
    if (StringName.op_Equality(ref name, NAncientEventLayout.PropertyName._fakeNextButtonControllerIcon))
    {
      value = VariantUtils.CreateFrom<TextureRect>(ref this._fakeNextButtonControllerIcon);
      return true;
    }
    if (StringName.op_Equality(ref name, NAncientEventLayout.PropertyName._fakeNextButtonLabel))
    {
      value = VariantUtils.CreateFrom<MegaLabel>(ref this._fakeNextButtonLabel);
      return true;
    }
    if (!StringName.op_Equality(ref name, NAncientEventLayout.PropertyName._contentTween))
      return base.GetGodotClassPropertyValue(in name, out value);
    value = VariantUtils.CreateFrom<Tween>(ref this._contentTween);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 2L, NAncientEventLayout.PropertyName._currentDialogueLine, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NAncientEventLayout.PropertyName._ancientBgContainer, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NAncientEventLayout.PropertyName._ancientNameBanner, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NAncientEventLayout.PropertyName._bannerTween, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NAncientEventLayout.PropertyName._contentContainer, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 3L, NAncientEventLayout.PropertyName._originalContentContainerHeight, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NAncientEventLayout.PropertyName._content, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NAncientEventLayout.PropertyName._dialogueContainer, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NAncientEventLayout.PropertyName._dialogueHitbox, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NAncientEventLayout.PropertyName._fakeNextButtonContainer, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NAncientEventLayout.PropertyName._fakeNextButton, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NAncientEventLayout.PropertyName._fakeNextButtonControllerIcon, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NAncientEventLayout.PropertyName._fakeNextButtonLabel, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NAncientEventLayout.PropertyName._contentTween, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 1L, NAncientEventLayout.PropertyName.IsDialogueOnLastLine, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NAncientEventLayout.PropertyName.DefaultFocusedControl, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    base.SaveGodotObjectData(info);
    info.AddProperty(NAncientEventLayout.PropertyName._currentDialogueLine, Variant.From<int>(ref this._currentDialogueLine));
    info.AddProperty(NAncientEventLayout.PropertyName._ancientBgContainer, Variant.From<NAncientBgContainer>(ref this._ancientBgContainer));
    info.AddProperty(NAncientEventLayout.PropertyName._ancientNameBanner, Variant.From<Control>(ref this._ancientNameBanner));
    info.AddProperty(NAncientEventLayout.PropertyName._bannerTween, Variant.From<Tween>(ref this._bannerTween));
    info.AddProperty(NAncientEventLayout.PropertyName._contentContainer, Variant.From<Control>(ref this._contentContainer));
    info.AddProperty(NAncientEventLayout.PropertyName._originalContentContainerHeight, Variant.From<float>(ref this._originalContentContainerHeight));
    info.AddProperty(NAncientEventLayout.PropertyName._content, Variant.From<VBoxContainer>(ref this._content));
    info.AddProperty(NAncientEventLayout.PropertyName._dialogueContainer, Variant.From<VBoxContainer>(ref this._dialogueContainer));
    info.AddProperty(NAncientEventLayout.PropertyName._dialogueHitbox, Variant.From<NAncientDialogueHitbox>(ref this._dialogueHitbox));
    info.AddProperty(NAncientEventLayout.PropertyName._fakeNextButtonContainer, Variant.From<Control>(ref this._fakeNextButtonContainer));
    info.AddProperty(NAncientEventLayout.PropertyName._fakeNextButton, Variant.From<Control>(ref this._fakeNextButton));
    info.AddProperty(NAncientEventLayout.PropertyName._fakeNextButtonControllerIcon, Variant.From<TextureRect>(ref this._fakeNextButtonControllerIcon));
    info.AddProperty(NAncientEventLayout.PropertyName._fakeNextButtonLabel, Variant.From<MegaLabel>(ref this._fakeNextButtonLabel));
    info.AddProperty(NAncientEventLayout.PropertyName._contentTween, Variant.From<Tween>(ref this._contentTween));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    base.RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NAncientEventLayout.PropertyName._currentDialogueLine, ref variant1))
      this._currentDialogueLine = ((Variant) ref variant1).As<int>();
    Variant variant2;
    if (info.TryGetProperty(NAncientEventLayout.PropertyName._ancientBgContainer, ref variant2))
      this._ancientBgContainer = ((Variant) ref variant2).As<NAncientBgContainer>();
    Variant variant3;
    if (info.TryGetProperty(NAncientEventLayout.PropertyName._ancientNameBanner, ref variant3))
      this._ancientNameBanner = ((Variant) ref variant3).As<Control>();
    Variant variant4;
    if (info.TryGetProperty(NAncientEventLayout.PropertyName._bannerTween, ref variant4))
      this._bannerTween = ((Variant) ref variant4).As<Tween>();
    Variant variant5;
    if (info.TryGetProperty(NAncientEventLayout.PropertyName._contentContainer, ref variant5))
      this._contentContainer = ((Variant) ref variant5).As<Control>();
    Variant variant6;
    if (info.TryGetProperty(NAncientEventLayout.PropertyName._originalContentContainerHeight, ref variant6))
      this._originalContentContainerHeight = ((Variant) ref variant6).As<float>();
    Variant variant7;
    if (info.TryGetProperty(NAncientEventLayout.PropertyName._content, ref variant7))
      this._content = ((Variant) ref variant7).As<VBoxContainer>();
    Variant variant8;
    if (info.TryGetProperty(NAncientEventLayout.PropertyName._dialogueContainer, ref variant8))
      this._dialogueContainer = ((Variant) ref variant8).As<VBoxContainer>();
    Variant variant9;
    if (info.TryGetProperty(NAncientEventLayout.PropertyName._dialogueHitbox, ref variant9))
      this._dialogueHitbox = ((Variant) ref variant9).As<NAncientDialogueHitbox>();
    Variant variant10;
    if (info.TryGetProperty(NAncientEventLayout.PropertyName._fakeNextButtonContainer, ref variant10))
      this._fakeNextButtonContainer = ((Variant) ref variant10).As<Control>();
    Variant variant11;
    if (info.TryGetProperty(NAncientEventLayout.PropertyName._fakeNextButton, ref variant11))
      this._fakeNextButton = ((Variant) ref variant11).As<Control>();
    Variant variant12;
    if (info.TryGetProperty(NAncientEventLayout.PropertyName._fakeNextButtonControllerIcon, ref variant12))
      this._fakeNextButtonControllerIcon = ((Variant) ref variant12).As<TextureRect>();
    Variant variant13;
    if (info.TryGetProperty(NAncientEventLayout.PropertyName._fakeNextButtonLabel, ref variant13))
      this._fakeNextButtonLabel = ((Variant) ref variant13).As<MegaLabel>();
    Variant variant14;
    if (!info.TryGetProperty(NAncientEventLayout.PropertyName._contentTween, ref variant14))
      return;
    this._contentTween = ((Variant) ref variant14).As<Tween>();
  }

  public new class MethodName : NEventLayout.MethodName
  {
    public new static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public new static readonly StringName _EnterTree = StringName.op_Implicit(nameof (_EnterTree));
    public new static readonly StringName _ExitTree = StringName.op_Implicit(nameof (_ExitTree));
    public new static readonly StringName InitializeVisuals = StringName.op_Implicit(nameof (InitializeVisuals));
    public new static readonly StringName AnimateIn = StringName.op_Implicit(nameof (AnimateIn));
    public static readonly StringName OnDialogueLineFocused = StringName.op_Implicit(nameof (OnDialogueLineFocused));
    public static readonly StringName OnDialogueLineUnfocused = StringName.op_Implicit(nameof (OnDialogueLineUnfocused));
    public static readonly StringName ClearDialogue = StringName.op_Implicit(nameof (ClearDialogue));
    public new static readonly StringName OnSetupComplete = StringName.op_Implicit(nameof (OnSetupComplete));
    public new static readonly StringName AnimateButtonsIn = StringName.op_Implicit(nameof (AnimateButtonsIn));
    public static readonly StringName OnDialogueHitboxClicked = StringName.op_Implicit(nameof (OnDialogueHitboxClicked));
    public static readonly StringName SetDialogueLineAndAnimate = StringName.op_Implicit(nameof (SetDialogueLineAndAnimate));
    public static readonly StringName UpdateFakeNextButton = StringName.op_Implicit(nameof (UpdateFakeNextButton));
    public static readonly StringName HideNameBanner = StringName.op_Implicit(nameof (HideNameBanner));
    public static readonly StringName ShowNameBanner = StringName.op_Implicit(nameof (ShowNameBanner));
    public static readonly StringName UpdateBannerVisibility = StringName.op_Implicit(nameof (UpdateBannerVisibility));
    public static readonly StringName UpdateHotkeyDisplay = StringName.op_Implicit(nameof (UpdateHotkeyDisplay));
  }

  public new class PropertyName : NEventLayout.PropertyName
  {
    public static readonly StringName IsDialogueOnLastLine = StringName.op_Implicit(nameof (IsDialogueOnLastLine));
    public new static readonly StringName DefaultFocusedControl = StringName.op_Implicit(nameof (DefaultFocusedControl));
    public static readonly StringName _currentDialogueLine = StringName.op_Implicit(nameof (_currentDialogueLine));
    public static readonly StringName _ancientBgContainer = StringName.op_Implicit(nameof (_ancientBgContainer));
    public static readonly StringName _ancientNameBanner = StringName.op_Implicit(nameof (_ancientNameBanner));
    public static readonly StringName _bannerTween = StringName.op_Implicit(nameof (_bannerTween));
    public static readonly StringName _contentContainer = StringName.op_Implicit(nameof (_contentContainer));
    public static readonly StringName _originalContentContainerHeight = StringName.op_Implicit(nameof (_originalContentContainerHeight));
    public static readonly StringName _content = StringName.op_Implicit(nameof (_content));
    public static readonly StringName _dialogueContainer = StringName.op_Implicit(nameof (_dialogueContainer));
    public static readonly StringName _dialogueHitbox = StringName.op_Implicit(nameof (_dialogueHitbox));
    public static readonly StringName _fakeNextButtonContainer = StringName.op_Implicit(nameof (_fakeNextButtonContainer));
    public static readonly StringName _fakeNextButton = StringName.op_Implicit(nameof (_fakeNextButton));
    public static readonly StringName _fakeNextButtonControllerIcon = StringName.op_Implicit(nameof (_fakeNextButtonControllerIcon));
    public static readonly StringName _fakeNextButtonLabel = StringName.op_Implicit(nameof (_fakeNextButtonLabel));
    public static readonly StringName _contentTween = StringName.op_Implicit(nameof (_contentTween));
  }

  public new class SignalName : NEventLayout.SignalName
  {
  }
}
