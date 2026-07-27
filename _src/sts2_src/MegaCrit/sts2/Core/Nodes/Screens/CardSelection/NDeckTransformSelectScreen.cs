// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Screens.CardSelection.NDeckTransformSelectScreen
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Cards;
using MegaCrit.Sts2.Core.Nodes.Cards.Holders;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.Screens.Overlays;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Screens.CardSelection;

[ScriptPath("res://src/Core/Nodes/Screens/CardSelection/NDeckTransformSelectScreen.cs")]
public sealed class NDeckTransformSelectScreen : NCardGridSelectionScreen
{
  private readonly HashSet<CardModel> _selectedCards = new HashSet<CardModel>();
  private Func<CardModel, CardTransformation> _cardToTransformation;
  private CardSelectorPrefs _prefs;
  private Control _previewContainer;
  private NTransformPreview _transformPreview;
  private NConfirmButton _confirmButton;
  private NBackButton _previewCancelButton;
  private NConfirmButton _previewConfirmButton;
  private Control _bottomTextContainer;
  private MegaRichTextLabel _infoLabel;
  private NTickbox _viewUpgrades;
  private NBackButton _closeButton;

  private static string ScenePath
  {
    get => SceneHelper.GetScenePath("screens/card_selection/deck_transform_select_screen");
  }

  public static IEnumerable<string> AssetPaths
  {
    get
    {
      return (IEnumerable<string>) new \u003C\u003Ez__ReadOnlySingleElementList<string>(NDeckTransformSelectScreen.ScenePath);
    }
  }

  public override void _Ready()
  {
    this.ConnectSignalsAndInitGrid();
    this._confirmButton = ((Node) this).GetNode<NConfirmButton>(NodePath.op_Implicit("Confirm"));
    this._previewContainer = ((Node) this).GetNode<Control>(NodePath.op_Implicit("%PreviewContainer"));
    this._transformPreview = ((Node) this._previewContainer).GetNode<NTransformPreview>(NodePath.op_Implicit("TransformPreview"));
    this._previewCancelButton = ((Node) this._previewContainer).GetNode<NBackButton>(NodePath.op_Implicit("Cancel"));
    this._previewConfirmButton = ((Node) this._previewContainer).GetNode<NConfirmButton>(NodePath.op_Implicit("Confirm"));
    this._closeButton = ((Node) this).GetNode<NBackButton>(NodePath.op_Implicit("%Close"));
    ((GodotObject) this._previewCancelButton).Connect(NClickableControl.SignalName.Released, Callable.From<NButton>(new Action<NButton>(this.CancelSelection)), 0U);
    ((GodotObject) this._previewConfirmButton).Connect(NClickableControl.SignalName.Released, Callable.From<NButton>(new Action<NButton>(this.CompleteSelection)), 0U);
    ((GodotObject) this._closeButton).Connect(NClickableControl.SignalName.Released, Callable.From<NButton>(new Action<NButton>(this.CloseSelection)), 0U);
    ((GodotObject) this._confirmButton).Connect(NClickableControl.SignalName.Released, Callable.From<NButton>(new Action<NButton>(this.ConfirmSelection)), 0U);
    if (this._prefs.Cancelable)
      this._closeButton.Enable();
    else
      this._closeButton.Disable();
    this.RefreshConfirmButtonVisibility();
    this._previewCancelButton.Disable();
    this._previewConfirmButton.Disable();
    this._bottomTextContainer = ((Node) this).GetNode<Control>(NodePath.op_Implicit("%BottomText"));
    this._infoLabel = ((Node) this._bottomTextContainer).GetNode<MegaRichTextLabel>(NodePath.op_Implicit("%BottomLabel"));
    this._infoLabel.Text = this._prefs.Prompt.GetFormattedText();
    this._viewUpgrades = ((Node) this).GetNode<NTickbox>(NodePath.op_Implicit("%Upgrades"));
    this._viewUpgrades.IsTicked = false;
    ((GodotObject) this._viewUpgrades).Connect(NTickbox.SignalName.Toggled, Callable.From<NTickbox>(new Action<NTickbox>(this.ToggleShowUpgrades)), 0U);
    this.OnControllerStateUpdated();
    ((GodotObject) NControllerManager.Instance).Connect(NControllerManager.SignalName.MouseDetected, Callable.From(new Action(this.OnControllerStateUpdated)), 0U);
    ((GodotObject) NControllerManager.Instance).Connect(NControllerManager.SignalName.ControllerDetected, Callable.From(new Action(this.OnControllerStateUpdated)), 0U);
    ((GodotObject) NInputManager.Instance).Connect(NInputManager.SignalName.InputRebound, Callable.From(new Action(this.OnControllerStateUpdated)), 0U);
    ((Node) this).GetNode<MegaLabel>(NodePath.op_Implicit("%ViewUpgradesLabel")).SetTextAutoSize(new LocString("card_selection", "VIEW_UPGRADES").GetFormattedText());
  }

  public static NDeckTransformSelectScreen ShowScreen(
    IReadOnlyList<CardModel> cards,
    Func<CardModel, CardTransformation> cardToTransformation,
    CardSelectorPrefs prefs)
  {
    NDeckTransformSelectScreen screen = PreloadManager.Cache.GetScene(NDeckTransformSelectScreen.ScenePath).Instantiate<NDeckTransformSelectScreen>((PackedScene.GenEditState) 0L);
    ((Node) screen).Name = StringName.op_Implicit(nameof (NDeckTransformSelectScreen));
    screen._cards = cards;
    screen._cardToTransformation = cardToTransformation;
    screen._prefs = prefs;
    NOverlayStack.Instance.Push((IOverlayScreen) screen);
    return screen;
  }

  protected override IEnumerable<Control> PeekButtonTargets
  {
    get
    {
      return (IEnumerable<Control>) new \u003C\u003Ez__ReadOnlyArray<Control>(new Control[3]
      {
        this._previewContainer,
        (Control) this._closeButton,
        this._bottomTextContainer
      });
    }
  }

  private void RefreshConfirmButtonVisibility()
  {
    if (this._prefs.MinSelect != this._prefs.MaxSelect && this._selectedCards.Count >= this._prefs.MinSelect)
      this._confirmButton.Enable();
    else
      this._confirmButton.Disable();
  }

  protected override void OnCardClicked(CardModel card)
  {
    if (this._selectedCards.Add(card))
    {
      this._grid.HighlightCard(card);
      if (this._prefs.MaxSelect == this._selectedCards.Count)
        this.OpenPreviewScreen();
    }
    else
    {
      this._selectedCards.Remove(card);
      this._grid.UnhighlightCard(card);
    }
    this.RefreshConfirmButtonVisibility();
  }

  private void CloseSelection(NButton _)
  {
    this._completionSource.SetResult((IEnumerable<CardModel>) Array.Empty<CardModel>());
    this._previewCancelButton.Disable();
    this._previewConfirmButton.Disable();
    NOverlayStack.Instance.Remove((IOverlayScreen) this);
  }

  private void CancelSelection(NButton _)
  {
    ((CanvasItem) this._previewContainer).Visible = false;
    this._grid.FocusBehaviorRecursive = (Control.FocusBehaviorRecursiveEnum) 0L;
    this._previewContainer.MouseFilter = (Control.MouseFilterEnum) 2L;
    this._transformPreview.Uninitialize();
    this._previewCancelButton.Disable();
    this._previewConfirmButton.Disable();
    this._grid.SetCanScroll(true);
    if (this._prefs.Cancelable)
      this._closeButton.Enable();
    foreach (CardModel selectedCard in this._selectedCards)
      this._grid.UnhighlightCard(selectedCard);
    NGridCardHolder cardHolder = this._grid.GetCardHolder(this._selectedCards.Last<CardModel>());
    if (cardHolder != null)
      cardHolder.TryGrabFocus();
    this._selectedCards.Clear();
  }

  private void ConfirmSelection(NButton _)
  {
    if (this._selectedCards.Count < this._prefs.MinSelect)
      return;
    if (this._prefs.RequireManualConfirmation)
      this.OpenPreviewScreen();
    else
      this.CompleteSelection(_);
  }

  private void OpenPreviewScreen()
  {
    ((Node) this).GetViewport().GuiReleaseFocus();
    this._grid.FocusBehaviorRecursive = (Control.FocusBehaviorRecursiveEnum) 1L;
    ((CanvasItem) this._previewContainer).Visible = true;
    this._previewContainer.MouseFilter = (Control.MouseFilterEnum) 0L;
    this._previewCancelButton.Enable();
    this._previewConfirmButton.Enable();
    foreach (CardModel selectedCard in this._selectedCards)
      this._grid.UnhighlightCard(selectedCard);
    this._transformPreview.Initialize(this._selectedCards.Select<CardModel, CardTransformation>(this._cardToTransformation));
    this._closeButton.Disable();
  }

  private void CompleteSelection(NButton _)
  {
    this._completionSource.SetResult((IEnumerable<CardModel>) this._selectedCards);
    NOverlayStack.Instance.Remove((IOverlayScreen) this);
  }

  private void ToggleShowUpgrades(NTickbox tickbox)
  {
    this._grid.IsShowingUpgrades = tickbox.IsTicked;
  }

  private void OnControllerStateUpdated()
  {
    ((CanvasItem) this._viewUpgrades).Visible = !NControllerManager.Instance.IsUsingController;
    if (!NControllerManager.Instance.IsUsingController)
      return;
    this._viewUpgrades.IsTicked = false;
    this.ToggleShowUpgrades(this._viewUpgrades);
  }

  public override Control? DefaultFocusedControl
  {
    get
    {
      return ((CanvasItem) this._previewContainer).Visible ? (Control) null : this._grid.DefaultFocusedControl;
    }
  }

  public override Control? FocusedControlFromTopBar
  {
    get
    {
      return ((CanvasItem) this._previewContainer).Visible ? (Control) null : this._grid.FocusedControlFromTopBar;
    }
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(9)
    {
      new MethodInfo(NDeckTransformSelectScreen.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NDeckTransformSelectScreen.MethodName.RefreshConfirmButtonVisibility, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NDeckTransformSelectScreen.MethodName.CloseSelection, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("_"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false)
      }, (List<Variant>) null),
      new MethodInfo(NDeckTransformSelectScreen.MethodName.CancelSelection, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("_"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false)
      }, (List<Variant>) null),
      new MethodInfo(NDeckTransformSelectScreen.MethodName.ConfirmSelection, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("_"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false)
      }, (List<Variant>) null),
      new MethodInfo(NDeckTransformSelectScreen.MethodName.OpenPreviewScreen, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NDeckTransformSelectScreen.MethodName.CompleteSelection, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("_"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false)
      }, (List<Variant>) null),
      new MethodInfo(NDeckTransformSelectScreen.MethodName.ToggleShowUpgrades, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("tickbox"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false)
      }, (List<Variant>) null),
      new MethodInfo(NDeckTransformSelectScreen.MethodName.OnControllerStateUpdated, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NDeckTransformSelectScreen.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NDeckTransformSelectScreen.MethodName.RefreshConfirmButtonVisibility) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.RefreshConfirmButtonVisibility();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NDeckTransformSelectScreen.MethodName.CloseSelection) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.CloseSelection(VariantUtils.ConvertTo<NButton>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NDeckTransformSelectScreen.MethodName.CancelSelection) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.CancelSelection(VariantUtils.ConvertTo<NButton>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NDeckTransformSelectScreen.MethodName.ConfirmSelection) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.ConfirmSelection(VariantUtils.ConvertTo<NButton>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NDeckTransformSelectScreen.MethodName.OpenPreviewScreen) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OpenPreviewScreen();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NDeckTransformSelectScreen.MethodName.CompleteSelection) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.CompleteSelection(VariantUtils.ConvertTo<NButton>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NDeckTransformSelectScreen.MethodName.ToggleShowUpgrades) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.ToggleShowUpgrades(VariantUtils.ConvertTo<NTickbox>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NDeckTransformSelectScreen.MethodName.OnControllerStateUpdated) || ((NativeVariantPtrArgs) ref args).Count != 0)
      return base.InvokeGodotClassMethod(in method, args, out ret);
    this.OnControllerStateUpdated();
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NDeckTransformSelectScreen.MethodName._Ready) || StringName.op_Equality(ref method, NDeckTransformSelectScreen.MethodName.RefreshConfirmButtonVisibility) || StringName.op_Equality(ref method, NDeckTransformSelectScreen.MethodName.CloseSelection) || StringName.op_Equality(ref method, NDeckTransformSelectScreen.MethodName.CancelSelection) || StringName.op_Equality(ref method, NDeckTransformSelectScreen.MethodName.ConfirmSelection) || StringName.op_Equality(ref method, NDeckTransformSelectScreen.MethodName.OpenPreviewScreen) || StringName.op_Equality(ref method, NDeckTransformSelectScreen.MethodName.CompleteSelection) || StringName.op_Equality(ref method, NDeckTransformSelectScreen.MethodName.ToggleShowUpgrades) || StringName.op_Equality(ref method, NDeckTransformSelectScreen.MethodName.OnControllerStateUpdated) || base.HasGodotClassMethod(in method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NDeckTransformSelectScreen.PropertyName._previewContainer))
    {
      this._previewContainer = VariantUtils.ConvertTo<Control>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NDeckTransformSelectScreen.PropertyName._transformPreview))
    {
      this._transformPreview = VariantUtils.ConvertTo<NTransformPreview>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NDeckTransformSelectScreen.PropertyName._confirmButton))
    {
      this._confirmButton = VariantUtils.ConvertTo<NConfirmButton>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NDeckTransformSelectScreen.PropertyName._previewCancelButton))
    {
      this._previewCancelButton = VariantUtils.ConvertTo<NBackButton>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NDeckTransformSelectScreen.PropertyName._previewConfirmButton))
    {
      this._previewConfirmButton = VariantUtils.ConvertTo<NConfirmButton>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NDeckTransformSelectScreen.PropertyName._bottomTextContainer))
    {
      this._bottomTextContainer = VariantUtils.ConvertTo<Control>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NDeckTransformSelectScreen.PropertyName._infoLabel))
    {
      this._infoLabel = VariantUtils.ConvertTo<MegaRichTextLabel>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NDeckTransformSelectScreen.PropertyName._viewUpgrades))
    {
      this._viewUpgrades = VariantUtils.ConvertTo<NTickbox>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NDeckTransformSelectScreen.PropertyName._closeButton))
      return base.SetGodotClassPropertyValue(in name, in value);
    this._closeButton = VariantUtils.ConvertTo<NBackButton>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NDeckTransformSelectScreen.PropertyName.DefaultFocusedControl))
    {
      ref godot_variant local = ref value;
      Control defaultFocusedControl = this.DefaultFocusedControl;
      godot_variant from = VariantUtils.CreateFrom<Control>(ref defaultFocusedControl);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NDeckTransformSelectScreen.PropertyName.FocusedControlFromTopBar))
    {
      ref godot_variant local = ref value;
      Control controlFromTopBar = this.FocusedControlFromTopBar;
      godot_variant from = VariantUtils.CreateFrom<Control>(ref controlFromTopBar);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NDeckTransformSelectScreen.PropertyName._previewContainer))
    {
      value = VariantUtils.CreateFrom<Control>(ref this._previewContainer);
      return true;
    }
    if (StringName.op_Equality(ref name, NDeckTransformSelectScreen.PropertyName._transformPreview))
    {
      value = VariantUtils.CreateFrom<NTransformPreview>(ref this._transformPreview);
      return true;
    }
    if (StringName.op_Equality(ref name, NDeckTransformSelectScreen.PropertyName._confirmButton))
    {
      value = VariantUtils.CreateFrom<NConfirmButton>(ref this._confirmButton);
      return true;
    }
    if (StringName.op_Equality(ref name, NDeckTransformSelectScreen.PropertyName._previewCancelButton))
    {
      value = VariantUtils.CreateFrom<NBackButton>(ref this._previewCancelButton);
      return true;
    }
    if (StringName.op_Equality(ref name, NDeckTransformSelectScreen.PropertyName._previewConfirmButton))
    {
      value = VariantUtils.CreateFrom<NConfirmButton>(ref this._previewConfirmButton);
      return true;
    }
    if (StringName.op_Equality(ref name, NDeckTransformSelectScreen.PropertyName._bottomTextContainer))
    {
      value = VariantUtils.CreateFrom<Control>(ref this._bottomTextContainer);
      return true;
    }
    if (StringName.op_Equality(ref name, NDeckTransformSelectScreen.PropertyName._infoLabel))
    {
      value = VariantUtils.CreateFrom<MegaRichTextLabel>(ref this._infoLabel);
      return true;
    }
    if (StringName.op_Equality(ref name, NDeckTransformSelectScreen.PropertyName._viewUpgrades))
    {
      value = VariantUtils.CreateFrom<NTickbox>(ref this._viewUpgrades);
      return true;
    }
    if (!StringName.op_Equality(ref name, NDeckTransformSelectScreen.PropertyName._closeButton))
      return base.GetGodotClassPropertyValue(in name, out value);
    value = VariantUtils.CreateFrom<NBackButton>(ref this._closeButton);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NDeckTransformSelectScreen.PropertyName._previewContainer, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NDeckTransformSelectScreen.PropertyName._transformPreview, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NDeckTransformSelectScreen.PropertyName._confirmButton, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NDeckTransformSelectScreen.PropertyName._previewCancelButton, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NDeckTransformSelectScreen.PropertyName._previewConfirmButton, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NDeckTransformSelectScreen.PropertyName._bottomTextContainer, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NDeckTransformSelectScreen.PropertyName._infoLabel, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NDeckTransformSelectScreen.PropertyName._viewUpgrades, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NDeckTransformSelectScreen.PropertyName._closeButton, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NDeckTransformSelectScreen.PropertyName.DefaultFocusedControl, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NDeckTransformSelectScreen.PropertyName.FocusedControlFromTopBar, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    base.SaveGodotObjectData(info);
    info.AddProperty(NDeckTransformSelectScreen.PropertyName._previewContainer, Variant.From<Control>(ref this._previewContainer));
    info.AddProperty(NDeckTransformSelectScreen.PropertyName._transformPreview, Variant.From<NTransformPreview>(ref this._transformPreview));
    info.AddProperty(NDeckTransformSelectScreen.PropertyName._confirmButton, Variant.From<NConfirmButton>(ref this._confirmButton));
    info.AddProperty(NDeckTransformSelectScreen.PropertyName._previewCancelButton, Variant.From<NBackButton>(ref this._previewCancelButton));
    info.AddProperty(NDeckTransformSelectScreen.PropertyName._previewConfirmButton, Variant.From<NConfirmButton>(ref this._previewConfirmButton));
    info.AddProperty(NDeckTransformSelectScreen.PropertyName._bottomTextContainer, Variant.From<Control>(ref this._bottomTextContainer));
    info.AddProperty(NDeckTransformSelectScreen.PropertyName._infoLabel, Variant.From<MegaRichTextLabel>(ref this._infoLabel));
    info.AddProperty(NDeckTransformSelectScreen.PropertyName._viewUpgrades, Variant.From<NTickbox>(ref this._viewUpgrades));
    info.AddProperty(NDeckTransformSelectScreen.PropertyName._closeButton, Variant.From<NBackButton>(ref this._closeButton));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    base.RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NDeckTransformSelectScreen.PropertyName._previewContainer, ref variant1))
      this._previewContainer = ((Variant) ref variant1).As<Control>();
    Variant variant2;
    if (info.TryGetProperty(NDeckTransformSelectScreen.PropertyName._transformPreview, ref variant2))
      this._transformPreview = ((Variant) ref variant2).As<NTransformPreview>();
    Variant variant3;
    if (info.TryGetProperty(NDeckTransformSelectScreen.PropertyName._confirmButton, ref variant3))
      this._confirmButton = ((Variant) ref variant3).As<NConfirmButton>();
    Variant variant4;
    if (info.TryGetProperty(NDeckTransformSelectScreen.PropertyName._previewCancelButton, ref variant4))
      this._previewCancelButton = ((Variant) ref variant4).As<NBackButton>();
    Variant variant5;
    if (info.TryGetProperty(NDeckTransformSelectScreen.PropertyName._previewConfirmButton, ref variant5))
      this._previewConfirmButton = ((Variant) ref variant5).As<NConfirmButton>();
    Variant variant6;
    if (info.TryGetProperty(NDeckTransformSelectScreen.PropertyName._bottomTextContainer, ref variant6))
      this._bottomTextContainer = ((Variant) ref variant6).As<Control>();
    Variant variant7;
    if (info.TryGetProperty(NDeckTransformSelectScreen.PropertyName._infoLabel, ref variant7))
      this._infoLabel = ((Variant) ref variant7).As<MegaRichTextLabel>();
    Variant variant8;
    if (info.TryGetProperty(NDeckTransformSelectScreen.PropertyName._viewUpgrades, ref variant8))
      this._viewUpgrades = ((Variant) ref variant8).As<NTickbox>();
    Variant variant9;
    if (!info.TryGetProperty(NDeckTransformSelectScreen.PropertyName._closeButton, ref variant9))
      return;
    this._closeButton = ((Variant) ref variant9).As<NBackButton>();
  }

  public new class MethodName : NCardGridSelectionScreen.MethodName
  {
    public new static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName RefreshConfirmButtonVisibility = StringName.op_Implicit(nameof (RefreshConfirmButtonVisibility));
    public static readonly StringName CloseSelection = StringName.op_Implicit(nameof (CloseSelection));
    public static readonly StringName CancelSelection = StringName.op_Implicit(nameof (CancelSelection));
    public static readonly StringName ConfirmSelection = StringName.op_Implicit(nameof (ConfirmSelection));
    public static readonly StringName OpenPreviewScreen = StringName.op_Implicit(nameof (OpenPreviewScreen));
    public static readonly StringName CompleteSelection = StringName.op_Implicit(nameof (CompleteSelection));
    public static readonly StringName ToggleShowUpgrades = StringName.op_Implicit(nameof (ToggleShowUpgrades));
    public static readonly StringName OnControllerStateUpdated = StringName.op_Implicit(nameof (OnControllerStateUpdated));
  }

  public new class PropertyName : NCardGridSelectionScreen.PropertyName
  {
    public new static readonly StringName DefaultFocusedControl = StringName.op_Implicit(nameof (DefaultFocusedControl));
    public new static readonly StringName FocusedControlFromTopBar = StringName.op_Implicit(nameof (FocusedControlFromTopBar));
    public static readonly StringName _previewContainer = StringName.op_Implicit(nameof (_previewContainer));
    public static readonly StringName _transformPreview = StringName.op_Implicit(nameof (_transformPreview));
    public static readonly StringName _confirmButton = StringName.op_Implicit(nameof (_confirmButton));
    public static readonly StringName _previewCancelButton = StringName.op_Implicit(nameof (_previewCancelButton));
    public static readonly StringName _previewConfirmButton = StringName.op_Implicit(nameof (_previewConfirmButton));
    public static readonly StringName _bottomTextContainer = StringName.op_Implicit(nameof (_bottomTextContainer));
    public static readonly StringName _infoLabel = StringName.op_Implicit(nameof (_infoLabel));
    public static readonly StringName _viewUpgrades = StringName.op_Implicit(nameof (_viewUpgrades));
    public static readonly StringName _closeButton = StringName.op_Implicit(nameof (_closeButton));
  }

  public new class SignalName : NCardGridSelectionScreen.SignalName
  {
  }
}
