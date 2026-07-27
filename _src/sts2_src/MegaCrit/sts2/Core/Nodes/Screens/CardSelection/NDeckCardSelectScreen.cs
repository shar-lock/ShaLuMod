// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Screens.CardSelection.NDeckCardSelectScreen
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

[ScriptPath("res://src/Core/Nodes/Screens/CardSelection/NDeckCardSelectScreen.cs")]
public sealed class NDeckCardSelectScreen : NCardGridSelectionScreen
{
  private readonly HashSet<CardModel> _selectedCards = new HashSet<CardModel>();
  private CardSelectorPrefs _prefs;
  private Control _previewContainer;
  private Control _previewCards;
  private NBackButton _previewCancelButton;
  private NConfirmButton _previewConfirmButton;
  private NBackButton _closeButton;
  private NConfirmButton _confirmButton;
  private MegaRichTextLabel _infoLabel;

  private static string ScenePath
  {
    get => SceneHelper.GetScenePath("screens/card_selection/deck_card_select_screen");
  }

  public static IEnumerable<string> AssetPaths
  {
    get
    {
      return (IEnumerable<string>) new \u003C\u003Ez__ReadOnlySingleElementList<string>(NDeckCardSelectScreen.ScenePath);
    }
  }

  protected override IEnumerable<Control> PeekButtonTargets
  {
    get
    {
      return (IEnumerable<Control>) new \u003C\u003Ez__ReadOnlyArray<Control>(new Control[3]
      {
        this._previewContainer,
        (Control) this._closeButton,
        (Control) this._confirmButton
      });
    }
  }

  public override void _Ready()
  {
    this.ConnectSignalsAndInitGrid();
    this._previewContainer = ((Node) this).GetNode<Control>(NodePath.op_Implicit("%PreviewContainer"));
    this._previewCards = ((Node) this._previewContainer).GetNode<Control>(NodePath.op_Implicit("%Cards"));
    this._previewCancelButton = ((Node) this._previewContainer).GetNode<NBackButton>(NodePath.op_Implicit("%PreviewCancel"));
    this._previewConfirmButton = ((Node) this._previewContainer).GetNode<NConfirmButton>(NodePath.op_Implicit("%PreviewConfirm"));
    this._closeButton = ((Node) this).GetNode<NBackButton>(NodePath.op_Implicit("%Close"));
    this._confirmButton = ((Node) this).GetNode<NConfirmButton>(NodePath.op_Implicit("%Confirm"));
    this._infoLabel = ((Node) this).GetNode<MegaRichTextLabel>(NodePath.op_Implicit("%BottomLabel"));
    ((GodotObject) this._previewCancelButton).Connect(NClickableControl.SignalName.Released, Callable.From<NButton>(new Action<NButton>(this.CancelSelection)), 0U);
    ((GodotObject) this._previewConfirmButton).Connect(NClickableControl.SignalName.Released, Callable.From<NButton>(new Action<NButton>(this.ConfirmSelection)), 0U);
    ((GodotObject) this._closeButton).Connect(NClickableControl.SignalName.Released, Callable.From<NButton>(new Action<NButton>(this.CloseSelection)), 0U);
    ((GodotObject) this._confirmButton).Connect(NClickableControl.SignalName.Released, Callable.From<NButton>(new Action<NButton>(this.PreviewSelection)), 0U);
    if (this._prefs.Cancelable)
      this._closeButton.Enable();
    else
      this._closeButton.Disable();
    this.RefreshConfirmButtonVisibility();
    ((CanvasItem) this._previewContainer).Visible = false;
    this._previewContainer.MouseFilter = (Control.MouseFilterEnum) 2L;
    this._previewCancelButton.Disable();
    this._previewConfirmButton.Disable();
    this._infoLabel.Text = this._prefs.Prompt.GetFormattedText();
  }

  public static NDeckCardSelectScreen Create(
    IReadOnlyList<CardModel> cards,
    CardSelectorPrefs prefs)
  {
    NDeckCardSelectScreen cardSelectScreen = PreloadManager.Cache.GetScene(NDeckCardSelectScreen.ScenePath).Instantiate<NDeckCardSelectScreen>((PackedScene.GenEditState) 0L);
    ((Node) cardSelectScreen).Name = StringName.op_Implicit(nameof (NDeckCardSelectScreen));
    cardSelectScreen._cards = cards;
    cardSelectScreen._prefs = prefs;
    return cardSelectScreen;
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
        this.PreviewSelection();
    }
    else
    {
      this._selectedCards.Remove(card);
      this._grid.UnhighlightCard(card);
    }
    this.RefreshConfirmButtonVisibility();
  }

  private void PreviewSelection(NButton _) => this.PreviewSelection();

  private void PreviewSelection()
  {
    this._grid.FocusBehaviorRecursive = (Control.FocusBehaviorRecursiveEnum) 1L;
    ((Node) this).GetViewport().GuiReleaseFocus();
    ((CanvasItem) this._previewContainer).Visible = true;
    this._previewContainer.MouseFilter = (Control.MouseFilterEnum) 0L;
    this._closeButton.Disable();
    this._grid.SetCanScroll(false);
    this._previewCancelButton.Enable();
    this._previewConfirmButton.Enable();
    foreach (CardModel selectedCard in this._selectedCards)
    {
      this._grid.UnhighlightCard(selectedCard);
      NCard card = NCard.Create(selectedCard);
      ((Node) this._previewCards).AddChildSafely((Node) NPreviewCardHolder.Create(card, true, false));
      card.UpdateVisuals(selectedCard.Pile.Type, CardPreviewMode.Normal);
    }
    Callable callable = Callable.From((Action) (() =>
    {
      this._previewCards.PivotOffset = Vector2.op_Division(this._previewCards.Size, 2f);
      float num = 1f;
      if (this._selectedCards.Count > 6)
        num = 0.55f;
      else if (this._selectedCards.Count > 3)
        num = 0.8f;
      this._previewCards.Scale = Vector2.op_Multiply(Vector2.One, num);
    }));
    ((Callable) ref callable).CallDeferred(Array.Empty<Variant>());
  }

  private void CloseSelection(NButton _)
  {
    this._completionSource.SetResult((IEnumerable<CardModel>) Array.Empty<CardModel>());
    NOverlayStack.Instance.Remove((IOverlayScreen) this);
  }

  private void CancelSelection(NButton _)
  {
    ((CanvasItem) this._previewContainer).Visible = false;
    this._previewCancelButton.Disable();
    this._previewConfirmButton.Disable();
    this._grid.FocusBehaviorRecursive = (Control.FocusBehaviorRecursiveEnum) 0L;
    this._grid.SetCanScroll(true);
    this._previewContainer.MouseFilter = (Control.MouseFilterEnum) 2L;
    for (int index = 0; index < ((Node) this._previewCards).GetChildCount(false); ++index)
      ((Node) this._previewCards).GetChild(index, false).QueueFreeSafely();
    NGridCardHolder cardHolder = this._grid.GetCardHolder(this._selectedCards.Last<CardModel>());
    if (cardHolder != null)
      cardHolder.TryGrabFocus();
    this._selectedCards.Clear();
    if (!this._prefs.Cancelable)
      return;
    this._closeButton.Enable();
  }

  private void ConfirmSelection(NButton _) => this.CheckIfSelectionComplete();

  private void CheckIfSelectionComplete()
  {
    if (this._selectedCards.Count < this._prefs.MinSelect)
      return;
    this._completionSource.SetResult((IEnumerable<CardModel>) this._selectedCards);
    NOverlayStack.Instance.Remove((IOverlayScreen) this);
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

  public override void AfterOverlayShown()
  {
    if (!this._prefs.Cancelable)
      return;
    this._closeButton.Enable();
  }

  public override void AfterOverlayHidden() => this._closeButton.Disable();

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(10)
    {
      new MethodInfo(NDeckCardSelectScreen.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NDeckCardSelectScreen.MethodName.RefreshConfirmButtonVisibility, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NDeckCardSelectScreen.MethodName.PreviewSelection, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("_"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false)
      }, (List<Variant>) null),
      new MethodInfo(NDeckCardSelectScreen.MethodName.PreviewSelection, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NDeckCardSelectScreen.MethodName.CloseSelection, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("_"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false)
      }, (List<Variant>) null),
      new MethodInfo(NDeckCardSelectScreen.MethodName.CancelSelection, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("_"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false)
      }, (List<Variant>) null),
      new MethodInfo(NDeckCardSelectScreen.MethodName.ConfirmSelection, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("_"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false)
      }, (List<Variant>) null),
      new MethodInfo(NDeckCardSelectScreen.MethodName.CheckIfSelectionComplete, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NDeckCardSelectScreen.MethodName.AfterOverlayShown, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NDeckCardSelectScreen.MethodName.AfterOverlayHidden, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NDeckCardSelectScreen.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NDeckCardSelectScreen.MethodName.RefreshConfirmButtonVisibility) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.RefreshConfirmButtonVisibility();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NDeckCardSelectScreen.MethodName.PreviewSelection) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.PreviewSelection(VariantUtils.ConvertTo<NButton>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NDeckCardSelectScreen.MethodName.PreviewSelection) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.PreviewSelection();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NDeckCardSelectScreen.MethodName.CloseSelection) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.CloseSelection(VariantUtils.ConvertTo<NButton>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NDeckCardSelectScreen.MethodName.CancelSelection) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.CancelSelection(VariantUtils.ConvertTo<NButton>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NDeckCardSelectScreen.MethodName.ConfirmSelection) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.ConfirmSelection(VariantUtils.ConvertTo<NButton>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NDeckCardSelectScreen.MethodName.CheckIfSelectionComplete) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.CheckIfSelectionComplete();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NDeckCardSelectScreen.MethodName.AfterOverlayShown) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.AfterOverlayShown();
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NDeckCardSelectScreen.MethodName.AfterOverlayHidden) || ((NativeVariantPtrArgs) ref args).Count != 0)
      return base.InvokeGodotClassMethod(in method, args, out ret);
    this.AfterOverlayHidden();
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NDeckCardSelectScreen.MethodName._Ready) || StringName.op_Equality(ref method, NDeckCardSelectScreen.MethodName.RefreshConfirmButtonVisibility) || StringName.op_Equality(ref method, NDeckCardSelectScreen.MethodName.PreviewSelection) || StringName.op_Equality(ref method, NDeckCardSelectScreen.MethodName.CloseSelection) || StringName.op_Equality(ref method, NDeckCardSelectScreen.MethodName.CancelSelection) || StringName.op_Equality(ref method, NDeckCardSelectScreen.MethodName.ConfirmSelection) || StringName.op_Equality(ref method, NDeckCardSelectScreen.MethodName.CheckIfSelectionComplete) || StringName.op_Equality(ref method, NDeckCardSelectScreen.MethodName.AfterOverlayShown) || StringName.op_Equality(ref method, NDeckCardSelectScreen.MethodName.AfterOverlayHidden) || base.HasGodotClassMethod(in method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NDeckCardSelectScreen.PropertyName._previewContainer))
    {
      this._previewContainer = VariantUtils.ConvertTo<Control>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NDeckCardSelectScreen.PropertyName._previewCards))
    {
      this._previewCards = VariantUtils.ConvertTo<Control>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NDeckCardSelectScreen.PropertyName._previewCancelButton))
    {
      this._previewCancelButton = VariantUtils.ConvertTo<NBackButton>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NDeckCardSelectScreen.PropertyName._previewConfirmButton))
    {
      this._previewConfirmButton = VariantUtils.ConvertTo<NConfirmButton>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NDeckCardSelectScreen.PropertyName._closeButton))
    {
      this._closeButton = VariantUtils.ConvertTo<NBackButton>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NDeckCardSelectScreen.PropertyName._confirmButton))
    {
      this._confirmButton = VariantUtils.ConvertTo<NConfirmButton>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NDeckCardSelectScreen.PropertyName._infoLabel))
      return base.SetGodotClassPropertyValue(in name, in value);
    this._infoLabel = VariantUtils.ConvertTo<MegaRichTextLabel>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NDeckCardSelectScreen.PropertyName.DefaultFocusedControl))
    {
      ref godot_variant local = ref value;
      Control defaultFocusedControl = this.DefaultFocusedControl;
      godot_variant from = VariantUtils.CreateFrom<Control>(ref defaultFocusedControl);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NDeckCardSelectScreen.PropertyName.FocusedControlFromTopBar))
    {
      ref godot_variant local = ref value;
      Control controlFromTopBar = this.FocusedControlFromTopBar;
      godot_variant from = VariantUtils.CreateFrom<Control>(ref controlFromTopBar);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NDeckCardSelectScreen.PropertyName._previewContainer))
    {
      value = VariantUtils.CreateFrom<Control>(ref this._previewContainer);
      return true;
    }
    if (StringName.op_Equality(ref name, NDeckCardSelectScreen.PropertyName._previewCards))
    {
      value = VariantUtils.CreateFrom<Control>(ref this._previewCards);
      return true;
    }
    if (StringName.op_Equality(ref name, NDeckCardSelectScreen.PropertyName._previewCancelButton))
    {
      value = VariantUtils.CreateFrom<NBackButton>(ref this._previewCancelButton);
      return true;
    }
    if (StringName.op_Equality(ref name, NDeckCardSelectScreen.PropertyName._previewConfirmButton))
    {
      value = VariantUtils.CreateFrom<NConfirmButton>(ref this._previewConfirmButton);
      return true;
    }
    if (StringName.op_Equality(ref name, NDeckCardSelectScreen.PropertyName._closeButton))
    {
      value = VariantUtils.CreateFrom<NBackButton>(ref this._closeButton);
      return true;
    }
    if (StringName.op_Equality(ref name, NDeckCardSelectScreen.PropertyName._confirmButton))
    {
      value = VariantUtils.CreateFrom<NConfirmButton>(ref this._confirmButton);
      return true;
    }
    if (!StringName.op_Equality(ref name, NDeckCardSelectScreen.PropertyName._infoLabel))
      return base.GetGodotClassPropertyValue(in name, out value);
    value = VariantUtils.CreateFrom<MegaRichTextLabel>(ref this._infoLabel);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NDeckCardSelectScreen.PropertyName._previewContainer, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NDeckCardSelectScreen.PropertyName._previewCards, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NDeckCardSelectScreen.PropertyName._previewCancelButton, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NDeckCardSelectScreen.PropertyName._previewConfirmButton, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NDeckCardSelectScreen.PropertyName._closeButton, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NDeckCardSelectScreen.PropertyName._confirmButton, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NDeckCardSelectScreen.PropertyName._infoLabel, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NDeckCardSelectScreen.PropertyName.DefaultFocusedControl, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NDeckCardSelectScreen.PropertyName.FocusedControlFromTopBar, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    base.SaveGodotObjectData(info);
    info.AddProperty(NDeckCardSelectScreen.PropertyName._previewContainer, Variant.From<Control>(ref this._previewContainer));
    info.AddProperty(NDeckCardSelectScreen.PropertyName._previewCards, Variant.From<Control>(ref this._previewCards));
    info.AddProperty(NDeckCardSelectScreen.PropertyName._previewCancelButton, Variant.From<NBackButton>(ref this._previewCancelButton));
    info.AddProperty(NDeckCardSelectScreen.PropertyName._previewConfirmButton, Variant.From<NConfirmButton>(ref this._previewConfirmButton));
    info.AddProperty(NDeckCardSelectScreen.PropertyName._closeButton, Variant.From<NBackButton>(ref this._closeButton));
    info.AddProperty(NDeckCardSelectScreen.PropertyName._confirmButton, Variant.From<NConfirmButton>(ref this._confirmButton));
    info.AddProperty(NDeckCardSelectScreen.PropertyName._infoLabel, Variant.From<MegaRichTextLabel>(ref this._infoLabel));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    base.RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NDeckCardSelectScreen.PropertyName._previewContainer, ref variant1))
      this._previewContainer = ((Variant) ref variant1).As<Control>();
    Variant variant2;
    if (info.TryGetProperty(NDeckCardSelectScreen.PropertyName._previewCards, ref variant2))
      this._previewCards = ((Variant) ref variant2).As<Control>();
    Variant variant3;
    if (info.TryGetProperty(NDeckCardSelectScreen.PropertyName._previewCancelButton, ref variant3))
      this._previewCancelButton = ((Variant) ref variant3).As<NBackButton>();
    Variant variant4;
    if (info.TryGetProperty(NDeckCardSelectScreen.PropertyName._previewConfirmButton, ref variant4))
      this._previewConfirmButton = ((Variant) ref variant4).As<NConfirmButton>();
    Variant variant5;
    if (info.TryGetProperty(NDeckCardSelectScreen.PropertyName._closeButton, ref variant5))
      this._closeButton = ((Variant) ref variant5).As<NBackButton>();
    Variant variant6;
    if (info.TryGetProperty(NDeckCardSelectScreen.PropertyName._confirmButton, ref variant6))
      this._confirmButton = ((Variant) ref variant6).As<NConfirmButton>();
    Variant variant7;
    if (!info.TryGetProperty(NDeckCardSelectScreen.PropertyName._infoLabel, ref variant7))
      return;
    this._infoLabel = ((Variant) ref variant7).As<MegaRichTextLabel>();
  }

  public new class MethodName : NCardGridSelectionScreen.MethodName
  {
    public new static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName RefreshConfirmButtonVisibility = StringName.op_Implicit(nameof (RefreshConfirmButtonVisibility));
    public static readonly StringName PreviewSelection = StringName.op_Implicit(nameof (PreviewSelection));
    public static readonly StringName CloseSelection = StringName.op_Implicit(nameof (CloseSelection));
    public static readonly StringName CancelSelection = StringName.op_Implicit(nameof (CancelSelection));
    public static readonly StringName ConfirmSelection = StringName.op_Implicit(nameof (ConfirmSelection));
    public static readonly StringName CheckIfSelectionComplete = StringName.op_Implicit(nameof (CheckIfSelectionComplete));
    public new static readonly StringName AfterOverlayShown = StringName.op_Implicit(nameof (AfterOverlayShown));
    public new static readonly StringName AfterOverlayHidden = StringName.op_Implicit(nameof (AfterOverlayHidden));
  }

  public new class PropertyName : NCardGridSelectionScreen.PropertyName
  {
    public new static readonly StringName DefaultFocusedControl = StringName.op_Implicit(nameof (DefaultFocusedControl));
    public new static readonly StringName FocusedControlFromTopBar = StringName.op_Implicit(nameof (FocusedControlFromTopBar));
    public static readonly StringName _previewContainer = StringName.op_Implicit(nameof (_previewContainer));
    public static readonly StringName _previewCards = StringName.op_Implicit(nameof (_previewCards));
    public static readonly StringName _previewCancelButton = StringName.op_Implicit(nameof (_previewCancelButton));
    public static readonly StringName _previewConfirmButton = StringName.op_Implicit(nameof (_previewConfirmButton));
    public static readonly StringName _closeButton = StringName.op_Implicit(nameof (_closeButton));
    public static readonly StringName _confirmButton = StringName.op_Implicit(nameof (_confirmButton));
    public static readonly StringName _infoLabel = StringName.op_Implicit(nameof (_infoLabel));
  }

  public new class SignalName : NCardGridSelectionScreen.SignalName
  {
  }
}
