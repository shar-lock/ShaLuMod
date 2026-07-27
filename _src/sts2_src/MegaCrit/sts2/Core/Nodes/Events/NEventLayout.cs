// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Events.NEventLayout
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.Debug;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Events;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.Saves;
using MegaCrit.Sts2.Core.Settings;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Events;

[ScriptPath("res://src/Core/Nodes/Events/NEventLayout.cs")]
public class NEventLayout : Control
{
  public const string defaultScenePath = "res://scenes/events/default_event_layout.tscn";
  protected Tween? _descriptionTween;
  protected VBoxContainer _optionsContainer;
  private TextureRect? _portrait;
  private Texture2D? _currentPortraitTex;
  private Texture2D? _currentPhobiaPortraitTex;
  private MegaLabel? _title;
  protected EventModel _event;
  protected MegaLabel? _sharedEventLabel;
  private static readonly LocString _sharedEventLoc = new LocString("events", "SHARED_EVENT_INFO");
  protected MegaRichTextLabel? _description;
  private static bool _isDebugUiVisible;

  public Control? VfxContainer { get; private set; }

  public IEnumerable<NEventOptionButton> OptionButtons
  {
    get
    {
      return ((IEnumerable) ((Node) this._optionsContainer).GetChildren(false)).OfType<NEventOptionButton>();
    }
  }

  public override void _Ready()
  {
    this._portrait = ((Node) this).GetNodeOrNull<TextureRect>(NodePath.op_Implicit("%Portrait"));
    this._title = ((Node) this).GetNodeOrNull<MegaLabel>(NodePath.op_Implicit("%Title"));
    this._description = ((Node) this).GetNodeOrNull<MegaRichTextLabel>(NodePath.op_Implicit("%EventDescription"));
    this.VfxContainer = ((Node) this).GetNodeOrNull<Control>(NodePath.op_Implicit("%VfxContainer"));
    this._sharedEventLabel = ((Node) this).GetNodeOrNull<MegaLabel>(NodePath.op_Implicit("%SharedEventLabel"));
    this._sharedEventLabel?.SetTextAutoSize(NEventLayout._sharedEventLoc.GetFormattedText());
    this._optionsContainer = ((Node) this).GetNode<VBoxContainer>(NodePath.op_Implicit("%OptionsContainer"));
    this._description?.SetText(string.Empty);
    this.ApplyDebugUiVisibility();
  }

  public override void _EnterTree()
  {
    RunManager.Instance.EventSynchronizer.PlayerVoteChanged += new Action<Player>(this.OnPlayerVoteChanged);
    ((GodotObject) NGame.Instance)?.Connect(NGame.SignalName.PhobiaModeToggled, Callable.From(new Action(this.UpdatePhobiaMode)), 0U);
  }

  public override void _ExitTree()
  {
    RunManager.Instance.EventSynchronizer.PlayerVoteChanged -= new Action<Player>(this.OnPlayerVoteChanged);
    ((GodotObject) NGame.Instance)?.Disconnect(NGame.SignalName.PhobiaModeToggled, Callable.From(new Action(this.UpdatePhobiaMode)));
  }

  public virtual void SetEvent(EventModel eventModel)
  {
    this._event = eventModel;
    this.InitializeVisuals();
    this._event.OnRoomEnter();
  }

  protected virtual void InitializeVisuals()
  {
    if (this._event.HasPhobiaModePortrait)
      this.SetPortrait(this._event.CreateInitialPortrait(), this._event.CreateInitialPhobiaModePortrait());
    else
      this.SetPortrait(this._event.CreateInitialPortrait());
    if (!this._event.HasVfx)
      return;
    Node2D vfx = this._event.CreateVfx();
    NEventRoom.Instance.Layout.AddVfxAnchoredToPortrait((Node) vfx);
    vfx.Position = EventModel.VfxOffset;
  }

  private void UpdatePhobiaMode()
  {
    if (this._currentPhobiaPortraitTex == null)
      return;
    if (this._portrait == null)
      throw new InvalidOperationException("Trying to set a portrait in an event layout that doesn't have one.");
    if (SaveManager.Instance.PrefsSave.PhobiaMode)
      this._portrait.Texture = this._currentPhobiaPortraitTex;
    else
      this._portrait.Texture = this._currentPortraitTex;
  }

  public void SetPortrait(Texture2D portrait, Texture2D? phobiaModePortrait = null)
  {
    if (this._portrait == null)
      throw new InvalidOperationException("Trying to set a portrait in an event layout that doesn't have one.");
    this._currentPortraitTex = portrait;
    this._currentPhobiaPortraitTex = phobiaModePortrait;
    this._portrait.Texture = !SaveManager.Instance.PrefsSave.PhobiaMode || this._currentPhobiaPortraitTex == null ? this._currentPortraitTex : this._currentPhobiaPortraitTex;
  }

  public void AddVfxAnchoredToPortrait(Node? vfx) => ((Node) this._portrait).AddChildSafely(vfx);

  public void RemoveNodesOnPortrait()
  {
    foreach (Node child in ((Node) this._portrait).GetChildren(false))
      ((Node) this._portrait).RemoveChildSafely(child);
  }

  public void SetTitle(string title)
  {
    if (this._title == null)
      return;
    this._title.Text = title;
  }

  public void SetDescription(string description)
  {
    if (this._description == null)
      return;
    this._description.SetTextAutoSize(description);
    this.AnimateIn();
  }

  protected virtual void AnimateIn()
  {
    if (this._sharedEventLabel != null)
      ((CanvasItem) this._sharedEventLabel).Modulate = StsColors.transparentWhite;
    if (this._description == null)
      return;
    ((CanvasItem) this._description).Modulate = StsColors.transparentWhite;
    bool flag = SaveManager.Instance.PrefsSave.FastMode == FastModeType.Fast;
    this._descriptionTween?.Kill();
    this._descriptionTween = ((Node) this).CreateTween().SetParallel(true);
    this._descriptionTween.TweenInterval(flag ? 0.2 : 0.5);
    this._descriptionTween.Chain();
    if (this._title != null)
      this._descriptionTween.TweenProperty((GodotObject) this._title, NodePath.op_Implicit("modulate"), Variant.op_Implicit(Colors.White), flag ? 0.25 : 0.5);
    this._descriptionTween.TweenProperty((GodotObject) this._description, NodePath.op_Implicit("modulate"), Variant.op_Implicit(Colors.White), flag ? 0.5 : 1.0).SetDelay(0.25);
    this._descriptionTween.TweenProperty((GodotObject) this._description, NodePath.op_Implicit("visible_ratio"), Variant.op_Implicit(1f), flag ? 0.5 : 1.0).SetDelay(0.25).From(Variant.op_Implicit(0.0f)).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 1L);
    if (this._sharedEventLabel == null)
      return;
    this._descriptionTween.TweenProperty((GodotObject) this._sharedEventLabel, NodePath.op_Implicit("modulate"), Variant.op_Implicit(Colors.White), flag ? 0.25 : 0.5).SetDelay(0.25);
  }

  public void ClearOptions()
  {
    foreach (Node node in ((IEnumerable<Node>) ((Node) this._optionsContainer).GetChildren(false)).ToList<Node>())
    {
      ((Node) this._optionsContainer).RemoveChildSafely(node);
      node.QueueFreeSafely();
    }
  }

  public void AddOptions(IEnumerable<EventOption> options)
  {
    if (this._sharedEventLabel != null)
    {
      MegaLabel sharedEventLabel = this._sharedEventLabel;
      EventModel eventModel = this._event;
      int num = eventModel == null || !eventModel.IsShared || eventModel.IsFinished ? 0 : (this._event.Owner.RunState.Players.Count > 1 ? 1 : 0);
      ((CanvasItem) sharedEventLabel).Visible = num != 0;
    }
    foreach (EventOption option in options)
    {
      NEventOptionButton child = NEventOptionButton.Create(this._event, option, ((Node) this._optionsContainer).GetChildCount(false));
      ((Node) this._optionsContainer).AddChildSafely((Node) child);
      child.RefreshVotes();
    }
    int childCount = ((Node) this._optionsContainer).GetChildCount(false);
    if (childCount == 0)
      return;
    NodePath path1 = ((Node) ((Node) this._optionsContainer).GetChild<Control>(0, false)).GetPath();
    NodePath path2 = ((Node) ((Node) this._optionsContainer).GetChild<Control>(childCount - 1, false)).GetPath();
    for (int index = 0; index < childCount; ++index)
    {
      Control child = ((Node) this._optionsContainer).GetChild<Control>(index, false);
      NodePath path3 = ((Node) child).GetPath();
      child.FocusNeighborLeft = path3;
      child.FocusNeighborRight = path3;
      child.FocusNeighborTop = index > 0 ? ((Node) ((Node) this._optionsContainer).GetChild<Control>(index - 1, false)).GetPath() : path2;
      child.FocusNeighborBottom = index < childCount - 1 ? ((Node) ((Node) this._optionsContainer).GetChild<Control>(index + 1, false)).GetPath() : path1;
    }
    this.AnimateButtonsIn();
  }

  public virtual void OnSetupComplete()
  {
  }

  protected virtual void AnimateButtonsIn()
  {
    foreach (NEventOptionButton optionButton in this.OptionButtons)
    {
      NEventOptionButton button = optionButton;
      Callable callable = Callable.From((Action) (() => button.AnimateIn()));
      ((Callable) ref callable).CallDeferred(Array.Empty<Variant>());
    }
  }

  public async Task BeforeSharedOptionChosen(EventOption option)
  {
    NEventOptionButton chosenButton = (NEventOptionButton) null;
    foreach (NEventOptionButton optionButton in this.OptionButtons)
    {
      optionButton.Disable();
      if (optionButton.Option == option)
        chosenButton = optionButton;
    }
    if (chosenButton == null)
    {
      chosenButton = (NEventOptionButton) null;
    }
    else
    {
      await new EventSplitVoteAnimation(this, this._event.Owner.RunState).TryPlay(chosenButton);
      foreach (NEventOptionButton optionButton in this.OptionButtons)
      {
        if (optionButton.Option != option)
          optionButton.GrayOut();
      }
      await chosenButton.FlashConfirmation();
      chosenButton = (NEventOptionButton) null;
    }
  }

  private void OnPlayerVoteChanged(Player player)
  {
    foreach (NEventOptionButton optionButton in this.OptionButtons)
      optionButton.RefreshVotes();
  }

  public void DisableEventOptions()
  {
    foreach (NClickableControl optionButton in this.OptionButtons)
      optionButton.Disable();
  }

  public override void _Input(InputEvent inputEvent)
  {
    if (!inputEvent.IsActionReleased(DebugHotkey.hideEventUi, false))
      return;
    NEventLayout._isDebugUiVisible = !NEventLayout._isDebugUiVisible;
    this.ApplyDebugUiVisibility();
    ((Node) NGame.Instance).AddChildSafely((Node) NFullscreenTextVfx.Create(NEventLayout._isDebugUiVisible ? "Hide Event UI" : "Show Event UI"));
  }

  private void ApplyDebugUiVisibility()
  {
    if (NEventLayout._isDebugUiVisible)
    {
      ((CanvasItem) this._optionsContainer).Visible = false;
      if (this._title != null)
        ((CanvasItem) this._title).Modulate = Colors.Transparent;
      if (this._description == null)
        return;
      ((CanvasItem) this._description).Visible = false;
    }
    else
    {
      ((CanvasItem) this._optionsContainer).Visible = true;
      if (this._description == null)
        return;
      ((CanvasItem) this._description).Visible = true;
    }
  }

  public virtual Control? DefaultFocusedControl
  {
    get => (Control) this.OptionButtons.FirstOrDefault<NEventOptionButton>();
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(17)
    {
      new MethodInfo(NEventLayout.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NEventLayout.MethodName._EnterTree, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NEventLayout.MethodName._ExitTree, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NEventLayout.MethodName.InitializeVisuals, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NEventLayout.MethodName.UpdatePhobiaMode, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NEventLayout.MethodName.SetPortrait, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("portrait"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Texture2D"), false),
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("phobiaModePortrait"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Texture2D"), false)
      }, (List<Variant>) null),
      new MethodInfo(NEventLayout.MethodName.AddVfxAnchoredToPortrait, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("vfx"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Node"), false)
      }, (List<Variant>) null),
      new MethodInfo(NEventLayout.MethodName.RemoveNodesOnPortrait, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NEventLayout.MethodName.SetTitle, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 4L, StringName.op_Implicit("title"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NEventLayout.MethodName.SetDescription, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 4L, StringName.op_Implicit("description"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NEventLayout.MethodName.AnimateIn, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NEventLayout.MethodName.ClearOptions, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NEventLayout.MethodName.OnSetupComplete, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NEventLayout.MethodName.AnimateButtonsIn, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NEventLayout.MethodName.DisableEventOptions, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NEventLayout.MethodName._Input, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("inputEvent"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("InputEvent"), false)
      }, (List<Variant>) null),
      new MethodInfo(NEventLayout.MethodName.ApplyDebugUiVisibility, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NEventLayout.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NEventLayout.MethodName._EnterTree) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._EnterTree();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NEventLayout.MethodName._ExitTree) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._ExitTree();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NEventLayout.MethodName.InitializeVisuals) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.InitializeVisuals();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NEventLayout.MethodName.UpdatePhobiaMode) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.UpdatePhobiaMode();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NEventLayout.MethodName.SetPortrait) && ((NativeVariantPtrArgs) ref args).Count == 2)
    {
      this.SetPortrait(VariantUtils.ConvertTo<Texture2D>(ref ((NativeVariantPtrArgs) ref args)[0]), VariantUtils.ConvertTo<Texture2D>(ref ((NativeVariantPtrArgs) ref args)[1]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NEventLayout.MethodName.AddVfxAnchoredToPortrait) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.AddVfxAnchoredToPortrait(VariantUtils.ConvertTo<Node>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NEventLayout.MethodName.RemoveNodesOnPortrait) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.RemoveNodesOnPortrait();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NEventLayout.MethodName.SetTitle) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.SetTitle(VariantUtils.ConvertTo<string>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NEventLayout.MethodName.SetDescription) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.SetDescription(VariantUtils.ConvertTo<string>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NEventLayout.MethodName.AnimateIn) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.AnimateIn();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NEventLayout.MethodName.ClearOptions) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.ClearOptions();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NEventLayout.MethodName.OnSetupComplete) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnSetupComplete();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NEventLayout.MethodName.AnimateButtonsIn) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.AnimateButtonsIn();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NEventLayout.MethodName.DisableEventOptions) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.DisableEventOptions();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NEventLayout.MethodName._Input) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      ((Node) this)._Input(VariantUtils.ConvertTo<InputEvent>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NEventLayout.MethodName.ApplyDebugUiVisibility) || ((NativeVariantPtrArgs) ref args).Count != 0)
      return base.InvokeGodotClassMethod(ref method, args, ref ret);
    this.ApplyDebugUiVisibility();
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NEventLayout.MethodName._Ready) || StringName.op_Equality(ref method, NEventLayout.MethodName._EnterTree) || StringName.op_Equality(ref method, NEventLayout.MethodName._ExitTree) || StringName.op_Equality(ref method, NEventLayout.MethodName.InitializeVisuals) || StringName.op_Equality(ref method, NEventLayout.MethodName.UpdatePhobiaMode) || StringName.op_Equality(ref method, NEventLayout.MethodName.SetPortrait) || StringName.op_Equality(ref method, NEventLayout.MethodName.AddVfxAnchoredToPortrait) || StringName.op_Equality(ref method, NEventLayout.MethodName.RemoveNodesOnPortrait) || StringName.op_Equality(ref method, NEventLayout.MethodName.SetTitle) || StringName.op_Equality(ref method, NEventLayout.MethodName.SetDescription) || StringName.op_Equality(ref method, NEventLayout.MethodName.AnimateIn) || StringName.op_Equality(ref method, NEventLayout.MethodName.ClearOptions) || StringName.op_Equality(ref method, NEventLayout.MethodName.OnSetupComplete) || StringName.op_Equality(ref method, NEventLayout.MethodName.AnimateButtonsIn) || StringName.op_Equality(ref method, NEventLayout.MethodName.DisableEventOptions) || StringName.op_Equality(ref method, NEventLayout.MethodName._Input) || StringName.op_Equality(ref method, NEventLayout.MethodName.ApplyDebugUiVisibility) || base.HasGodotClassMethod(ref method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NEventLayout.PropertyName.VfxContainer))
    {
      this.VfxContainer = VariantUtils.ConvertTo<Control>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NEventLayout.PropertyName._descriptionTween))
    {
      this._descriptionTween = VariantUtils.ConvertTo<Tween>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NEventLayout.PropertyName._optionsContainer))
    {
      this._optionsContainer = VariantUtils.ConvertTo<VBoxContainer>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NEventLayout.PropertyName._portrait))
    {
      this._portrait = VariantUtils.ConvertTo<TextureRect>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NEventLayout.PropertyName._currentPortraitTex))
    {
      this._currentPortraitTex = VariantUtils.ConvertTo<Texture2D>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NEventLayout.PropertyName._currentPhobiaPortraitTex))
    {
      this._currentPhobiaPortraitTex = VariantUtils.ConvertTo<Texture2D>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NEventLayout.PropertyName._title))
    {
      this._title = VariantUtils.ConvertTo<MegaLabel>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NEventLayout.PropertyName._sharedEventLabel))
    {
      this._sharedEventLabel = VariantUtils.ConvertTo<MegaLabel>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NEventLayout.PropertyName._description))
      return ((GodotObject) this).SetGodotClassPropertyValue(ref name, ref value);
    this._description = VariantUtils.ConvertTo<MegaRichTextLabel>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NEventLayout.PropertyName.VfxContainer))
    {
      ref godot_variant local = ref value;
      Control vfxContainer = this.VfxContainer;
      godot_variant from = VariantUtils.CreateFrom<Control>(ref vfxContainer);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NEventLayout.PropertyName.DefaultFocusedControl))
    {
      ref godot_variant local = ref value;
      Control defaultFocusedControl = this.DefaultFocusedControl;
      godot_variant from = VariantUtils.CreateFrom<Control>(ref defaultFocusedControl);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NEventLayout.PropertyName._descriptionTween))
    {
      value = VariantUtils.CreateFrom<Tween>(ref this._descriptionTween);
      return true;
    }
    if (StringName.op_Equality(ref name, NEventLayout.PropertyName._optionsContainer))
    {
      value = VariantUtils.CreateFrom<VBoxContainer>(ref this._optionsContainer);
      return true;
    }
    if (StringName.op_Equality(ref name, NEventLayout.PropertyName._portrait))
    {
      value = VariantUtils.CreateFrom<TextureRect>(ref this._portrait);
      return true;
    }
    if (StringName.op_Equality(ref name, NEventLayout.PropertyName._currentPortraitTex))
    {
      value = VariantUtils.CreateFrom<Texture2D>(ref this._currentPortraitTex);
      return true;
    }
    if (StringName.op_Equality(ref name, NEventLayout.PropertyName._currentPhobiaPortraitTex))
    {
      value = VariantUtils.CreateFrom<Texture2D>(ref this._currentPhobiaPortraitTex);
      return true;
    }
    if (StringName.op_Equality(ref name, NEventLayout.PropertyName._title))
    {
      value = VariantUtils.CreateFrom<MegaLabel>(ref this._title);
      return true;
    }
    if (StringName.op_Equality(ref name, NEventLayout.PropertyName._sharedEventLabel))
    {
      value = VariantUtils.CreateFrom<MegaLabel>(ref this._sharedEventLabel);
      return true;
    }
    if (!StringName.op_Equality(ref name, NEventLayout.PropertyName._description))
      return ((GodotObject) this).GetGodotClassPropertyValue(ref name, ref value);
    value = VariantUtils.CreateFrom<MegaRichTextLabel>(ref this._description);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NEventLayout.PropertyName.VfxContainer, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NEventLayout.PropertyName._descriptionTween, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NEventLayout.PropertyName._optionsContainer, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NEventLayout.PropertyName._portrait, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NEventLayout.PropertyName._currentPortraitTex, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NEventLayout.PropertyName._currentPhobiaPortraitTex, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NEventLayout.PropertyName._title, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NEventLayout.PropertyName._sharedEventLabel, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NEventLayout.PropertyName._description, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NEventLayout.PropertyName.DefaultFocusedControl, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).SaveGodotObjectData(info);
    GodotSerializationInfo serializationInfo = info;
    StringName vfxContainer1 = NEventLayout.PropertyName.VfxContainer;
    Control vfxContainer2 = this.VfxContainer;
    Variant variant = Variant.From<Control>(ref vfxContainer2);
    serializationInfo.AddProperty(vfxContainer1, variant);
    info.AddProperty(NEventLayout.PropertyName._descriptionTween, Variant.From<Tween>(ref this._descriptionTween));
    info.AddProperty(NEventLayout.PropertyName._optionsContainer, Variant.From<VBoxContainer>(ref this._optionsContainer));
    info.AddProperty(NEventLayout.PropertyName._portrait, Variant.From<TextureRect>(ref this._portrait));
    info.AddProperty(NEventLayout.PropertyName._currentPortraitTex, Variant.From<Texture2D>(ref this._currentPortraitTex));
    info.AddProperty(NEventLayout.PropertyName._currentPhobiaPortraitTex, Variant.From<Texture2D>(ref this._currentPhobiaPortraitTex));
    info.AddProperty(NEventLayout.PropertyName._title, Variant.From<MegaLabel>(ref this._title));
    info.AddProperty(NEventLayout.PropertyName._sharedEventLabel, Variant.From<MegaLabel>(ref this._sharedEventLabel));
    info.AddProperty(NEventLayout.PropertyName._description, Variant.From<MegaRichTextLabel>(ref this._description));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NEventLayout.PropertyName.VfxContainer, ref variant1))
      this.VfxContainer = ((Variant) ref variant1).As<Control>();
    Variant variant2;
    if (info.TryGetProperty(NEventLayout.PropertyName._descriptionTween, ref variant2))
      this._descriptionTween = ((Variant) ref variant2).As<Tween>();
    Variant variant3;
    if (info.TryGetProperty(NEventLayout.PropertyName._optionsContainer, ref variant3))
      this._optionsContainer = ((Variant) ref variant3).As<VBoxContainer>();
    Variant variant4;
    if (info.TryGetProperty(NEventLayout.PropertyName._portrait, ref variant4))
      this._portrait = ((Variant) ref variant4).As<TextureRect>();
    Variant variant5;
    if (info.TryGetProperty(NEventLayout.PropertyName._currentPortraitTex, ref variant5))
      this._currentPortraitTex = ((Variant) ref variant5).As<Texture2D>();
    Variant variant6;
    if (info.TryGetProperty(NEventLayout.PropertyName._currentPhobiaPortraitTex, ref variant6))
      this._currentPhobiaPortraitTex = ((Variant) ref variant6).As<Texture2D>();
    Variant variant7;
    if (info.TryGetProperty(NEventLayout.PropertyName._title, ref variant7))
      this._title = ((Variant) ref variant7).As<MegaLabel>();
    Variant variant8;
    if (info.TryGetProperty(NEventLayout.PropertyName._sharedEventLabel, ref variant8))
      this._sharedEventLabel = ((Variant) ref variant8).As<MegaLabel>();
    Variant variant9;
    if (!info.TryGetProperty(NEventLayout.PropertyName._description, ref variant9))
      return;
    this._description = ((Variant) ref variant9).As<MegaRichTextLabel>();
  }

  public class MethodName : Control.MethodName
  {
    public static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName _EnterTree = StringName.op_Implicit(nameof (_EnterTree));
    public static readonly StringName _ExitTree = StringName.op_Implicit(nameof (_ExitTree));
    public static readonly StringName InitializeVisuals = StringName.op_Implicit(nameof (InitializeVisuals));
    public static readonly StringName UpdatePhobiaMode = StringName.op_Implicit(nameof (UpdatePhobiaMode));
    public static readonly StringName SetPortrait = StringName.op_Implicit(nameof (SetPortrait));
    public static readonly StringName AddVfxAnchoredToPortrait = StringName.op_Implicit(nameof (AddVfxAnchoredToPortrait));
    public static readonly StringName RemoveNodesOnPortrait = StringName.op_Implicit(nameof (RemoveNodesOnPortrait));
    public static readonly StringName SetTitle = StringName.op_Implicit(nameof (SetTitle));
    public static readonly StringName SetDescription = StringName.op_Implicit(nameof (SetDescription));
    public static readonly StringName AnimateIn = StringName.op_Implicit(nameof (AnimateIn));
    public static readonly StringName ClearOptions = StringName.op_Implicit(nameof (ClearOptions));
    public static readonly StringName OnSetupComplete = StringName.op_Implicit(nameof (OnSetupComplete));
    public static readonly StringName AnimateButtonsIn = StringName.op_Implicit(nameof (AnimateButtonsIn));
    public static readonly StringName DisableEventOptions = StringName.op_Implicit(nameof (DisableEventOptions));
    public static readonly StringName _Input = StringName.op_Implicit(nameof (_Input));
    public static readonly StringName ApplyDebugUiVisibility = StringName.op_Implicit(nameof (ApplyDebugUiVisibility));
  }

  public class PropertyName : Control.PropertyName
  {
    public static readonly StringName VfxContainer = StringName.op_Implicit(nameof (VfxContainer));
    public static readonly StringName DefaultFocusedControl = StringName.op_Implicit(nameof (DefaultFocusedControl));
    public static readonly StringName _descriptionTween = StringName.op_Implicit(nameof (_descriptionTween));
    public static readonly StringName _optionsContainer = StringName.op_Implicit(nameof (_optionsContainer));
    public static readonly StringName _portrait = StringName.op_Implicit(nameof (_portrait));
    public static readonly StringName _currentPortraitTex = StringName.op_Implicit(nameof (_currentPortraitTex));
    public static readonly StringName _currentPhobiaPortraitTex = StringName.op_Implicit(nameof (_currentPhobiaPortraitTex));
    public static readonly StringName _title = StringName.op_Implicit(nameof (_title));
    public static readonly StringName _sharedEventLabel = StringName.op_Implicit(nameof (_sharedEventLabel));
    public static readonly StringName _description = StringName.op_Implicit(nameof (_description));
  }

  public class SignalName : Control.SignalName
  {
  }
}
