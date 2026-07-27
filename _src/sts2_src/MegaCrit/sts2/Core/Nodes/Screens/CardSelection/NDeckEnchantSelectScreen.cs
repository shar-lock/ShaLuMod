// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Screens.CardSelection.NDeckEnchantSelectScreen
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

[ScriptPath("res://src/Core/Nodes/Screens/CardSelection/NDeckEnchantSelectScreen.cs")]
public sealed class NDeckEnchantSelectScreen : NCardGridSelectionScreen
{
  private readonly HashSet<CardModel> _selectedCards = new HashSet<CardModel>();
  private CardSelectorPrefs _prefs;
  private EnchantmentModel _enchantment;
  private int _enchantmentAmount;
  private Control _enchantSinglePreviewContainer;
  private NEnchantPreview _singlePreview;
  private NBackButton _singlePreviewCancelButton;
  private NConfirmButton _singlePreviewConfirmButton;
  private NConfirmButton _confirmButton;
  private Control _enchantMultiPreviewContainer;
  private Control _multiPreview;
  private NBackButton _multiPreviewCancelButton;
  private NConfirmButton _multiPreviewConfirmButton;
  private Control _enchantmentDescriptionContainer;
  private MegaLabel _enchantmentTitle;
  private MegaRichTextLabel _enchantmentDescription;
  private TextureRect _enchantmentIcon;
  private Control _bottomTextContainer;
  private MegaRichTextLabel _infoLabel;
  private NBackButton _closeButton;

  private static string ScenePath
  {
    get => SceneHelper.GetScenePath("screens/card_selection/deck_enchant_select_screen");
  }

  public static IEnumerable<string> AssetPaths
  {
    get
    {
      return (IEnumerable<string>) new \u003C\u003Ez__ReadOnlySingleElementList<string>(NDeckEnchantSelectScreen.ScenePath);
    }
  }

  private bool UseSingleSelection => this._prefs.MaxSelect == 1;

  public override void _Ready()
  {
    this.ConnectSignalsAndInitGrid();
    this._confirmButton = ((Node) this).GetNode<NConfirmButton>(NodePath.op_Implicit("Confirm"));
    this._enchantSinglePreviewContainer = ((Node) this).GetNode<Control>(NodePath.op_Implicit("%EnchantSinglePreviewContainer"));
    this._singlePreview = ((Node) this._enchantSinglePreviewContainer).GetNode<NEnchantPreview>(NodePath.op_Implicit("EnchantPreview"));
    this._singlePreviewCancelButton = ((Node) this._enchantSinglePreviewContainer).GetNode<NBackButton>(NodePath.op_Implicit("Cancel"));
    this._singlePreviewConfirmButton = ((Node) this._enchantSinglePreviewContainer).GetNode<NConfirmButton>(NodePath.op_Implicit("Confirm"));
    this._enchantMultiPreviewContainer = ((Node) this).GetNode<Control>(NodePath.op_Implicit("%EnchantMultiPreviewContainer"));
    this._multiPreview = ((Node) this._enchantMultiPreviewContainer).GetNode<Control>(NodePath.op_Implicit("Cards"));
    this._multiPreviewCancelButton = ((Node) this._enchantMultiPreviewContainer).GetNode<NBackButton>(NodePath.op_Implicit("Cancel"));
    this._multiPreviewConfirmButton = ((Node) this._enchantMultiPreviewContainer).GetNode<NConfirmButton>(NodePath.op_Implicit("Confirm"));
    this._enchantmentDescriptionContainer = ((Node) this).GetNode<Control>(NodePath.op_Implicit("%EnchantmentDescriptionContainer"));
    this._enchantmentIcon = ((Node) this._enchantmentDescriptionContainer).GetNode<TextureRect>(NodePath.op_Implicit("%EnchantmentIcon"));
    this._enchantmentTitle = ((Node) this._enchantmentDescriptionContainer).GetNode<MegaLabel>(NodePath.op_Implicit("%EnchantmentTitle"));
    this._enchantmentDescription = ((Node) this._enchantmentDescriptionContainer).GetNode<MegaRichTextLabel>(NodePath.op_Implicit("%EnchantmentDescription"));
    this._closeButton = ((Node) this).GetNode<NBackButton>(NodePath.op_Implicit("%Close"));
    ((GodotObject) this._singlePreviewCancelButton).Connect(NClickableControl.SignalName.Released, Callable.From<NButton>(new Action<NButton>(this.CancelSelection)), 0U);
    ((GodotObject) this._singlePreviewConfirmButton).Connect(NClickableControl.SignalName.Released, Callable.From<NButton>(new Action<NButton>(this.ConfirmSelection)), 0U);
    ((GodotObject) this._multiPreviewCancelButton).Connect(NClickableControl.SignalName.Released, Callable.From<NButton>(new Action<NButton>(this.CancelSelection)), 0U);
    ((GodotObject) this._multiPreviewConfirmButton).Connect(NClickableControl.SignalName.Released, Callable.From<NButton>(new Action<NButton>(this.ConfirmSelection)), 0U);
    ((GodotObject) this._closeButton).Connect(NClickableControl.SignalName.Released, Callable.From<NButton>(new Action<NButton>(this.CloseSelection)), 0U);
    ((GodotObject) this._confirmButton).Connect(NClickableControl.SignalName.Released, Callable.From<NButton>(new Action<NButton>(this.PreviewSelection)), 0U);
    if (this._prefs.Cancelable)
      this._closeButton.Enable();
    else
      this._closeButton.Disable();
    EnchantmentModel mutable = this._enchantment.ToMutable();
    mutable.Amount = this._enchantmentAmount;
    mutable.RecalculateValues();
    this._enchantmentTitle.SetTextAutoSize(mutable.Title.GetFormattedText());
    this._enchantmentDescription.Text = mutable.DynamicDescription.GetFormattedText();
    this._enchantmentIcon.Texture = (Texture2D) mutable.Icon;
    ((CanvasItem) this._enchantSinglePreviewContainer).Visible = false;
    this._enchantSinglePreviewContainer.MouseFilter = (Control.MouseFilterEnum) 2L;
    ((CanvasItem) this._enchantMultiPreviewContainer).Visible = false;
    this._enchantMultiPreviewContainer.MouseFilter = (Control.MouseFilterEnum) 2L;
    this.RefreshConfirmButtonVisibility();
    this._bottomTextContainer = ((Node) this).GetNode<Control>(NodePath.op_Implicit("%BottomText"));
    this._infoLabel = ((Node) this._bottomTextContainer).GetNode<MegaRichTextLabel>(NodePath.op_Implicit("%BottomLabel"));
    this._infoLabel.Text = $"[center]{this._prefs.Prompt.GetFormattedText()}[/center]";
  }

  public static NDeckEnchantSelectScreen ShowScreen(
    IReadOnlyList<CardModel> cards,
    EnchantmentModel enchantment,
    int amount,
    CardSelectorPrefs prefs)
  {
    NDeckEnchantSelectScreen screen = PreloadManager.Cache.GetScene(NDeckEnchantSelectScreen.ScenePath).Instantiate<NDeckEnchantSelectScreen>((PackedScene.GenEditState) 0L);
    ((Node) screen).Name = StringName.op_Implicit(nameof (NDeckEnchantSelectScreen));
    screen._cards = cards;
    screen._prefs = prefs;
    screen._enchantment = enchantment;
    screen._enchantmentAmount = amount;
    NOverlayStack.Instance.Push((IOverlayScreen) screen);
    return screen;
  }

  protected override IEnumerable<Control> PeekButtonTargets
  {
    get
    {
      return (IEnumerable<Control>) new \u003C\u003Ez__ReadOnlyArray<Control>(new Control[5]
      {
        this._enchantSinglePreviewContainer,
        this._enchantMultiPreviewContainer,
        this._enchantmentDescriptionContainer,
        (Control) this._closeButton,
        this._bottomTextContainer
      });
    }
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

  private void RefreshConfirmButtonVisibility()
  {
    if (this._prefs.MinSelect != this._prefs.MaxSelect && this._selectedCards.Count >= this._prefs.MinSelect)
      this._confirmButton.Enable();
    else
      this._confirmButton.Disable();
  }

  private void CloseSelection(NButton _)
  {
    this._completionSource.SetResult((IEnumerable<CardModel>) Array.Empty<CardModel>());
    NOverlayStack.Instance.Remove((IOverlayScreen) this);
  }

  private void CancelSelection(NButton _)
  {
    if (this.UseSingleSelection)
    {
      this._singlePreviewCancelButton.Disable();
      this._singlePreviewConfirmButton.Disable();
      ((CanvasItem) this._enchantSinglePreviewContainer).Visible = false;
      this._enchantSinglePreviewContainer.MouseFilter = (Control.MouseFilterEnum) 2L;
    }
    else
    {
      this._multiPreviewCancelButton.Disable();
      this._multiPreviewConfirmButton.Disable();
      ((CanvasItem) this._enchantMultiPreviewContainer).Visible = false;
      this._enchantMultiPreviewContainer.MouseFilter = (Control.MouseFilterEnum) 2L;
      for (int index = 0; index < ((Node) this._multiPreview).GetChildCount(false); ++index)
        ((Node) this._multiPreview).GetChild(index, false).QueueFreeSafely();
    }
    this._grid.FocusBehaviorRecursive = (Control.FocusBehaviorRecursiveEnum) 0L;
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

  private void PreviewSelection(NButton _) => this.PreviewSelection();

  private void PreviewSelection()
  {
    this._grid.FocusBehaviorRecursive = (Control.FocusBehaviorRecursiveEnum) 1L;
    this._grid.SetCanScroll(false);
    this._closeButton.Disable();
    ((Node) this).GetViewport().GuiReleaseFocus();
    if (this.UseSingleSelection)
    {
      ((CanvasItem) this._enchantSinglePreviewContainer).Visible = true;
      this._enchantSinglePreviewContainer.MouseFilter = (Control.MouseFilterEnum) 0L;
      this._singlePreview.Init(this._selectedCards.First<CardModel>(), this._enchantment, this._enchantmentAmount);
      this._singlePreviewCancelButton.Enable();
      this._singlePreviewConfirmButton.Enable();
    }
    else
    {
      ((CanvasItem) this._enchantMultiPreviewContainer).Visible = true;
      this._enchantMultiPreviewContainer.MouseFilter = (Control.MouseFilterEnum) 0L;
      this._multiPreviewCancelButton.Enable();
      this._multiPreviewConfirmButton.Enable();
      foreach (CardModel selectedCard in this._selectedCards)
      {
        NCard card = NCard.Create(selectedCard);
        ((Node) this._multiPreview).AddChildSafely((Node) NPreviewCardHolder.Create(card, true, false));
        card.UpdateVisuals(selectedCard.Pile.Type, CardPreviewMode.Normal);
      }
    }
  }

  private void ConfirmSelection(NButton inputEvent)
  {
    if (this._selectedCards.Count == 0)
      return;
    this.CheckIfSelectionComplete();
  }

  private void CheckIfSelectionComplete()
  {
    this._singlePreviewCancelButton.Enable();
    this._singlePreviewConfirmButton.Enable();
    if (this._selectedCards.Count < this._prefs.MinSelect || this._selectedCards.Count > this._prefs.MaxSelect)
      return;
    this._completionSource.SetResult((IEnumerable<CardModel>) this._selectedCards);
    NOverlayStack.Instance.Remove((IOverlayScreen) this);
  }

  public override Control? DefaultFocusedControl
  {
    get
    {
      return ((CanvasItem) this._enchantSinglePreviewContainer).Visible || ((CanvasItem) this._enchantMultiPreviewContainer).Visible ? (Control) null : this._grid.DefaultFocusedControl;
    }
  }

  public override Control? FocusedControlFromTopBar
  {
    get
    {
      return ((CanvasItem) this._enchantSinglePreviewContainer).Visible || ((CanvasItem) this._enchantMultiPreviewContainer).Visible ? (Control) null : this._grid.FocusedControlFromTopBar;
    }
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(8)
    {
      new MethodInfo(NDeckEnchantSelectScreen.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NDeckEnchantSelectScreen.MethodName.RefreshConfirmButtonVisibility, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NDeckEnchantSelectScreen.MethodName.CloseSelection, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("_"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false)
      }, (List<Variant>) null),
      new MethodInfo(NDeckEnchantSelectScreen.MethodName.CancelSelection, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("_"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false)
      }, (List<Variant>) null),
      new MethodInfo(NDeckEnchantSelectScreen.MethodName.PreviewSelection, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("_"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false)
      }, (List<Variant>) null),
      new MethodInfo(NDeckEnchantSelectScreen.MethodName.PreviewSelection, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NDeckEnchantSelectScreen.MethodName.ConfirmSelection, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("inputEvent"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false)
      }, (List<Variant>) null),
      new MethodInfo(NDeckEnchantSelectScreen.MethodName.CheckIfSelectionComplete, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NDeckEnchantSelectScreen.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NDeckEnchantSelectScreen.MethodName.RefreshConfirmButtonVisibility) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.RefreshConfirmButtonVisibility();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NDeckEnchantSelectScreen.MethodName.CloseSelection) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.CloseSelection(VariantUtils.ConvertTo<NButton>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NDeckEnchantSelectScreen.MethodName.CancelSelection) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.CancelSelection(VariantUtils.ConvertTo<NButton>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NDeckEnchantSelectScreen.MethodName.PreviewSelection) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.PreviewSelection(VariantUtils.ConvertTo<NButton>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NDeckEnchantSelectScreen.MethodName.PreviewSelection) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.PreviewSelection();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NDeckEnchantSelectScreen.MethodName.ConfirmSelection) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.ConfirmSelection(VariantUtils.ConvertTo<NButton>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NDeckEnchantSelectScreen.MethodName.CheckIfSelectionComplete) || ((NativeVariantPtrArgs) ref args).Count != 0)
      return base.InvokeGodotClassMethod(in method, args, out ret);
    this.CheckIfSelectionComplete();
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NDeckEnchantSelectScreen.MethodName._Ready) || StringName.op_Equality(ref method, NDeckEnchantSelectScreen.MethodName.RefreshConfirmButtonVisibility) || StringName.op_Equality(ref method, NDeckEnchantSelectScreen.MethodName.CloseSelection) || StringName.op_Equality(ref method, NDeckEnchantSelectScreen.MethodName.CancelSelection) || StringName.op_Equality(ref method, NDeckEnchantSelectScreen.MethodName.PreviewSelection) || StringName.op_Equality(ref method, NDeckEnchantSelectScreen.MethodName.ConfirmSelection) || StringName.op_Equality(ref method, NDeckEnchantSelectScreen.MethodName.CheckIfSelectionComplete) || base.HasGodotClassMethod(in method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NDeckEnchantSelectScreen.PropertyName._enchantmentAmount))
    {
      this._enchantmentAmount = VariantUtils.ConvertTo<int>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NDeckEnchantSelectScreen.PropertyName._enchantSinglePreviewContainer))
    {
      this._enchantSinglePreviewContainer = VariantUtils.ConvertTo<Control>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NDeckEnchantSelectScreen.PropertyName._singlePreview))
    {
      this._singlePreview = VariantUtils.ConvertTo<NEnchantPreview>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NDeckEnchantSelectScreen.PropertyName._singlePreviewCancelButton))
    {
      this._singlePreviewCancelButton = VariantUtils.ConvertTo<NBackButton>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NDeckEnchantSelectScreen.PropertyName._singlePreviewConfirmButton))
    {
      this._singlePreviewConfirmButton = VariantUtils.ConvertTo<NConfirmButton>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NDeckEnchantSelectScreen.PropertyName._confirmButton))
    {
      this._confirmButton = VariantUtils.ConvertTo<NConfirmButton>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NDeckEnchantSelectScreen.PropertyName._enchantMultiPreviewContainer))
    {
      this._enchantMultiPreviewContainer = VariantUtils.ConvertTo<Control>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NDeckEnchantSelectScreen.PropertyName._multiPreview))
    {
      this._multiPreview = VariantUtils.ConvertTo<Control>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NDeckEnchantSelectScreen.PropertyName._multiPreviewCancelButton))
    {
      this._multiPreviewCancelButton = VariantUtils.ConvertTo<NBackButton>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NDeckEnchantSelectScreen.PropertyName._multiPreviewConfirmButton))
    {
      this._multiPreviewConfirmButton = VariantUtils.ConvertTo<NConfirmButton>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NDeckEnchantSelectScreen.PropertyName._enchantmentDescriptionContainer))
    {
      this._enchantmentDescriptionContainer = VariantUtils.ConvertTo<Control>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NDeckEnchantSelectScreen.PropertyName._enchantmentTitle))
    {
      this._enchantmentTitle = VariantUtils.ConvertTo<MegaLabel>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NDeckEnchantSelectScreen.PropertyName._enchantmentDescription))
    {
      this._enchantmentDescription = VariantUtils.ConvertTo<MegaRichTextLabel>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NDeckEnchantSelectScreen.PropertyName._enchantmentIcon))
    {
      this._enchantmentIcon = VariantUtils.ConvertTo<TextureRect>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NDeckEnchantSelectScreen.PropertyName._bottomTextContainer))
    {
      this._bottomTextContainer = VariantUtils.ConvertTo<Control>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NDeckEnchantSelectScreen.PropertyName._infoLabel))
    {
      this._infoLabel = VariantUtils.ConvertTo<MegaRichTextLabel>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NDeckEnchantSelectScreen.PropertyName._closeButton))
      return base.SetGodotClassPropertyValue(in name, in value);
    this._closeButton = VariantUtils.ConvertTo<NBackButton>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NDeckEnchantSelectScreen.PropertyName.UseSingleSelection))
    {
      ref godot_variant local = ref value;
      bool useSingleSelection = this.UseSingleSelection;
      godot_variant from = VariantUtils.CreateFrom<bool>(ref useSingleSelection);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NDeckEnchantSelectScreen.PropertyName.DefaultFocusedControl))
    {
      ref godot_variant local = ref value;
      Control defaultFocusedControl = this.DefaultFocusedControl;
      godot_variant from = VariantUtils.CreateFrom<Control>(ref defaultFocusedControl);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NDeckEnchantSelectScreen.PropertyName.FocusedControlFromTopBar))
    {
      ref godot_variant local = ref value;
      Control controlFromTopBar = this.FocusedControlFromTopBar;
      godot_variant from = VariantUtils.CreateFrom<Control>(ref controlFromTopBar);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NDeckEnchantSelectScreen.PropertyName._enchantmentAmount))
    {
      value = VariantUtils.CreateFrom<int>(ref this._enchantmentAmount);
      return true;
    }
    if (StringName.op_Equality(ref name, NDeckEnchantSelectScreen.PropertyName._enchantSinglePreviewContainer))
    {
      value = VariantUtils.CreateFrom<Control>(ref this._enchantSinglePreviewContainer);
      return true;
    }
    if (StringName.op_Equality(ref name, NDeckEnchantSelectScreen.PropertyName._singlePreview))
    {
      value = VariantUtils.CreateFrom<NEnchantPreview>(ref this._singlePreview);
      return true;
    }
    if (StringName.op_Equality(ref name, NDeckEnchantSelectScreen.PropertyName._singlePreviewCancelButton))
    {
      value = VariantUtils.CreateFrom<NBackButton>(ref this._singlePreviewCancelButton);
      return true;
    }
    if (StringName.op_Equality(ref name, NDeckEnchantSelectScreen.PropertyName._singlePreviewConfirmButton))
    {
      value = VariantUtils.CreateFrom<NConfirmButton>(ref this._singlePreviewConfirmButton);
      return true;
    }
    if (StringName.op_Equality(ref name, NDeckEnchantSelectScreen.PropertyName._confirmButton))
    {
      value = VariantUtils.CreateFrom<NConfirmButton>(ref this._confirmButton);
      return true;
    }
    if (StringName.op_Equality(ref name, NDeckEnchantSelectScreen.PropertyName._enchantMultiPreviewContainer))
    {
      value = VariantUtils.CreateFrom<Control>(ref this._enchantMultiPreviewContainer);
      return true;
    }
    if (StringName.op_Equality(ref name, NDeckEnchantSelectScreen.PropertyName._multiPreview))
    {
      value = VariantUtils.CreateFrom<Control>(ref this._multiPreview);
      return true;
    }
    if (StringName.op_Equality(ref name, NDeckEnchantSelectScreen.PropertyName._multiPreviewCancelButton))
    {
      value = VariantUtils.CreateFrom<NBackButton>(ref this._multiPreviewCancelButton);
      return true;
    }
    if (StringName.op_Equality(ref name, NDeckEnchantSelectScreen.PropertyName._multiPreviewConfirmButton))
    {
      value = VariantUtils.CreateFrom<NConfirmButton>(ref this._multiPreviewConfirmButton);
      return true;
    }
    if (StringName.op_Equality(ref name, NDeckEnchantSelectScreen.PropertyName._enchantmentDescriptionContainer))
    {
      value = VariantUtils.CreateFrom<Control>(ref this._enchantmentDescriptionContainer);
      return true;
    }
    if (StringName.op_Equality(ref name, NDeckEnchantSelectScreen.PropertyName._enchantmentTitle))
    {
      value = VariantUtils.CreateFrom<MegaLabel>(ref this._enchantmentTitle);
      return true;
    }
    if (StringName.op_Equality(ref name, NDeckEnchantSelectScreen.PropertyName._enchantmentDescription))
    {
      value = VariantUtils.CreateFrom<MegaRichTextLabel>(ref this._enchantmentDescription);
      return true;
    }
    if (StringName.op_Equality(ref name, NDeckEnchantSelectScreen.PropertyName._enchantmentIcon))
    {
      value = VariantUtils.CreateFrom<TextureRect>(ref this._enchantmentIcon);
      return true;
    }
    if (StringName.op_Equality(ref name, NDeckEnchantSelectScreen.PropertyName._bottomTextContainer))
    {
      value = VariantUtils.CreateFrom<Control>(ref this._bottomTextContainer);
      return true;
    }
    if (StringName.op_Equality(ref name, NDeckEnchantSelectScreen.PropertyName._infoLabel))
    {
      value = VariantUtils.CreateFrom<MegaRichTextLabel>(ref this._infoLabel);
      return true;
    }
    if (!StringName.op_Equality(ref name, NDeckEnchantSelectScreen.PropertyName._closeButton))
      return base.GetGodotClassPropertyValue(in name, out value);
    value = VariantUtils.CreateFrom<NBackButton>(ref this._closeButton);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 1L, NDeckEnchantSelectScreen.PropertyName.UseSingleSelection, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 2L, NDeckEnchantSelectScreen.PropertyName._enchantmentAmount, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NDeckEnchantSelectScreen.PropertyName._enchantSinglePreviewContainer, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NDeckEnchantSelectScreen.PropertyName._singlePreview, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NDeckEnchantSelectScreen.PropertyName._singlePreviewCancelButton, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NDeckEnchantSelectScreen.PropertyName._singlePreviewConfirmButton, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NDeckEnchantSelectScreen.PropertyName._confirmButton, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NDeckEnchantSelectScreen.PropertyName._enchantMultiPreviewContainer, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NDeckEnchantSelectScreen.PropertyName._multiPreview, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NDeckEnchantSelectScreen.PropertyName._multiPreviewCancelButton, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NDeckEnchantSelectScreen.PropertyName._multiPreviewConfirmButton, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NDeckEnchantSelectScreen.PropertyName._enchantmentDescriptionContainer, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NDeckEnchantSelectScreen.PropertyName._enchantmentTitle, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NDeckEnchantSelectScreen.PropertyName._enchantmentDescription, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NDeckEnchantSelectScreen.PropertyName._enchantmentIcon, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NDeckEnchantSelectScreen.PropertyName._bottomTextContainer, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NDeckEnchantSelectScreen.PropertyName._infoLabel, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NDeckEnchantSelectScreen.PropertyName._closeButton, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NDeckEnchantSelectScreen.PropertyName.DefaultFocusedControl, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NDeckEnchantSelectScreen.PropertyName.FocusedControlFromTopBar, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    base.SaveGodotObjectData(info);
    info.AddProperty(NDeckEnchantSelectScreen.PropertyName._enchantmentAmount, Variant.From<int>(ref this._enchantmentAmount));
    info.AddProperty(NDeckEnchantSelectScreen.PropertyName._enchantSinglePreviewContainer, Variant.From<Control>(ref this._enchantSinglePreviewContainer));
    info.AddProperty(NDeckEnchantSelectScreen.PropertyName._singlePreview, Variant.From<NEnchantPreview>(ref this._singlePreview));
    info.AddProperty(NDeckEnchantSelectScreen.PropertyName._singlePreviewCancelButton, Variant.From<NBackButton>(ref this._singlePreviewCancelButton));
    info.AddProperty(NDeckEnchantSelectScreen.PropertyName._singlePreviewConfirmButton, Variant.From<NConfirmButton>(ref this._singlePreviewConfirmButton));
    info.AddProperty(NDeckEnchantSelectScreen.PropertyName._confirmButton, Variant.From<NConfirmButton>(ref this._confirmButton));
    info.AddProperty(NDeckEnchantSelectScreen.PropertyName._enchantMultiPreviewContainer, Variant.From<Control>(ref this._enchantMultiPreviewContainer));
    info.AddProperty(NDeckEnchantSelectScreen.PropertyName._multiPreview, Variant.From<Control>(ref this._multiPreview));
    info.AddProperty(NDeckEnchantSelectScreen.PropertyName._multiPreviewCancelButton, Variant.From<NBackButton>(ref this._multiPreviewCancelButton));
    info.AddProperty(NDeckEnchantSelectScreen.PropertyName._multiPreviewConfirmButton, Variant.From<NConfirmButton>(ref this._multiPreviewConfirmButton));
    info.AddProperty(NDeckEnchantSelectScreen.PropertyName._enchantmentDescriptionContainer, Variant.From<Control>(ref this._enchantmentDescriptionContainer));
    info.AddProperty(NDeckEnchantSelectScreen.PropertyName._enchantmentTitle, Variant.From<MegaLabel>(ref this._enchantmentTitle));
    info.AddProperty(NDeckEnchantSelectScreen.PropertyName._enchantmentDescription, Variant.From<MegaRichTextLabel>(ref this._enchantmentDescription));
    info.AddProperty(NDeckEnchantSelectScreen.PropertyName._enchantmentIcon, Variant.From<TextureRect>(ref this._enchantmentIcon));
    info.AddProperty(NDeckEnchantSelectScreen.PropertyName._bottomTextContainer, Variant.From<Control>(ref this._bottomTextContainer));
    info.AddProperty(NDeckEnchantSelectScreen.PropertyName._infoLabel, Variant.From<MegaRichTextLabel>(ref this._infoLabel));
    info.AddProperty(NDeckEnchantSelectScreen.PropertyName._closeButton, Variant.From<NBackButton>(ref this._closeButton));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    base.RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NDeckEnchantSelectScreen.PropertyName._enchantmentAmount, ref variant1))
      this._enchantmentAmount = ((Variant) ref variant1).As<int>();
    Variant variant2;
    if (info.TryGetProperty(NDeckEnchantSelectScreen.PropertyName._enchantSinglePreviewContainer, ref variant2))
      this._enchantSinglePreviewContainer = ((Variant) ref variant2).As<Control>();
    Variant variant3;
    if (info.TryGetProperty(NDeckEnchantSelectScreen.PropertyName._singlePreview, ref variant3))
      this._singlePreview = ((Variant) ref variant3).As<NEnchantPreview>();
    Variant variant4;
    if (info.TryGetProperty(NDeckEnchantSelectScreen.PropertyName._singlePreviewCancelButton, ref variant4))
      this._singlePreviewCancelButton = ((Variant) ref variant4).As<NBackButton>();
    Variant variant5;
    if (info.TryGetProperty(NDeckEnchantSelectScreen.PropertyName._singlePreviewConfirmButton, ref variant5))
      this._singlePreviewConfirmButton = ((Variant) ref variant5).As<NConfirmButton>();
    Variant variant6;
    if (info.TryGetProperty(NDeckEnchantSelectScreen.PropertyName._confirmButton, ref variant6))
      this._confirmButton = ((Variant) ref variant6).As<NConfirmButton>();
    Variant variant7;
    if (info.TryGetProperty(NDeckEnchantSelectScreen.PropertyName._enchantMultiPreviewContainer, ref variant7))
      this._enchantMultiPreviewContainer = ((Variant) ref variant7).As<Control>();
    Variant variant8;
    if (info.TryGetProperty(NDeckEnchantSelectScreen.PropertyName._multiPreview, ref variant8))
      this._multiPreview = ((Variant) ref variant8).As<Control>();
    Variant variant9;
    if (info.TryGetProperty(NDeckEnchantSelectScreen.PropertyName._multiPreviewCancelButton, ref variant9))
      this._multiPreviewCancelButton = ((Variant) ref variant9).As<NBackButton>();
    Variant variant10;
    if (info.TryGetProperty(NDeckEnchantSelectScreen.PropertyName._multiPreviewConfirmButton, ref variant10))
      this._multiPreviewConfirmButton = ((Variant) ref variant10).As<NConfirmButton>();
    Variant variant11;
    if (info.TryGetProperty(NDeckEnchantSelectScreen.PropertyName._enchantmentDescriptionContainer, ref variant11))
      this._enchantmentDescriptionContainer = ((Variant) ref variant11).As<Control>();
    Variant variant12;
    if (info.TryGetProperty(NDeckEnchantSelectScreen.PropertyName._enchantmentTitle, ref variant12))
      this._enchantmentTitle = ((Variant) ref variant12).As<MegaLabel>();
    Variant variant13;
    if (info.TryGetProperty(NDeckEnchantSelectScreen.PropertyName._enchantmentDescription, ref variant13))
      this._enchantmentDescription = ((Variant) ref variant13).As<MegaRichTextLabel>();
    Variant variant14;
    if (info.TryGetProperty(NDeckEnchantSelectScreen.PropertyName._enchantmentIcon, ref variant14))
      this._enchantmentIcon = ((Variant) ref variant14).As<TextureRect>();
    Variant variant15;
    if (info.TryGetProperty(NDeckEnchantSelectScreen.PropertyName._bottomTextContainer, ref variant15))
      this._bottomTextContainer = ((Variant) ref variant15).As<Control>();
    Variant variant16;
    if (info.TryGetProperty(NDeckEnchantSelectScreen.PropertyName._infoLabel, ref variant16))
      this._infoLabel = ((Variant) ref variant16).As<MegaRichTextLabel>();
    Variant variant17;
    if (!info.TryGetProperty(NDeckEnchantSelectScreen.PropertyName._closeButton, ref variant17))
      return;
    this._closeButton = ((Variant) ref variant17).As<NBackButton>();
  }

  public new class MethodName : NCardGridSelectionScreen.MethodName
  {
    public new static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName RefreshConfirmButtonVisibility = StringName.op_Implicit(nameof (RefreshConfirmButtonVisibility));
    public static readonly StringName CloseSelection = StringName.op_Implicit(nameof (CloseSelection));
    public static readonly StringName CancelSelection = StringName.op_Implicit(nameof (CancelSelection));
    public static readonly StringName PreviewSelection = StringName.op_Implicit(nameof (PreviewSelection));
    public static readonly StringName ConfirmSelection = StringName.op_Implicit(nameof (ConfirmSelection));
    public static readonly StringName CheckIfSelectionComplete = StringName.op_Implicit(nameof (CheckIfSelectionComplete));
  }

  public new class PropertyName : NCardGridSelectionScreen.PropertyName
  {
    public static readonly StringName UseSingleSelection = StringName.op_Implicit(nameof (UseSingleSelection));
    public new static readonly StringName DefaultFocusedControl = StringName.op_Implicit(nameof (DefaultFocusedControl));
    public new static readonly StringName FocusedControlFromTopBar = StringName.op_Implicit(nameof (FocusedControlFromTopBar));
    public static readonly StringName _enchantmentAmount = StringName.op_Implicit(nameof (_enchantmentAmount));
    public static readonly StringName _enchantSinglePreviewContainer = StringName.op_Implicit(nameof (_enchantSinglePreviewContainer));
    public static readonly StringName _singlePreview = StringName.op_Implicit(nameof (_singlePreview));
    public static readonly StringName _singlePreviewCancelButton = StringName.op_Implicit(nameof (_singlePreviewCancelButton));
    public static readonly StringName _singlePreviewConfirmButton = StringName.op_Implicit(nameof (_singlePreviewConfirmButton));
    public static readonly StringName _confirmButton = StringName.op_Implicit(nameof (_confirmButton));
    public static readonly StringName _enchantMultiPreviewContainer = StringName.op_Implicit(nameof (_enchantMultiPreviewContainer));
    public static readonly StringName _multiPreview = StringName.op_Implicit(nameof (_multiPreview));
    public static readonly StringName _multiPreviewCancelButton = StringName.op_Implicit(nameof (_multiPreviewCancelButton));
    public static readonly StringName _multiPreviewConfirmButton = StringName.op_Implicit(nameof (_multiPreviewConfirmButton));
    public static readonly StringName _enchantmentDescriptionContainer = StringName.op_Implicit(nameof (_enchantmentDescriptionContainer));
    public static readonly StringName _enchantmentTitle = StringName.op_Implicit(nameof (_enchantmentTitle));
    public static readonly StringName _enchantmentDescription = StringName.op_Implicit(nameof (_enchantmentDescription));
    public static readonly StringName _enchantmentIcon = StringName.op_Implicit(nameof (_enchantmentIcon));
    public static readonly StringName _bottomTextContainer = StringName.op_Implicit(nameof (_bottomTextContainer));
    public static readonly StringName _infoLabel = StringName.op_Implicit(nameof (_infoLabel));
    public static readonly StringName _closeButton = StringName.op_Implicit(nameof (_closeButton));
  }

  public new class SignalName : NCardGridSelectionScreen.SignalName
  {
  }
}
