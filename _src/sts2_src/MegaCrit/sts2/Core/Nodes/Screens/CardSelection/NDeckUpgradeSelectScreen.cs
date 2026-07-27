// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Screens.CardSelection.NDeckUpgradeSelectScreen
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
using MegaCrit.Sts2.Core.Runs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Screens.CardSelection;

[ScriptPath("res://src/Core/Nodes/Screens/CardSelection/NDeckUpgradeSelectScreen.cs")]
public sealed class NDeckUpgradeSelectScreen : NCardGridSelectionScreen
{
  private readonly HashSet<CardModel> _selectedCards = new HashSet<CardModel>();
  private CardSelectorPrefs _prefs;
  private IRunState _runState;
  private Control _upgradeSinglePreviewContainer;
  private NUpgradePreview _singlePreview;
  private NBackButton _singlePreviewCancelButton;
  private NConfirmButton _singlePreviewConfirmButton;
  private NTickbox _viewUpgrades;
  private Control _bottomTextContainer;
  private MegaRichTextLabel _infoLabel;
  private Control _upgradeMultiPreviewContainer;
  private Control _multiPreview;
  private NBackButton _multiPreviewCancelButton;
  private NConfirmButton _multiPreviewConfirmButton;
  private NBackButton _closeButton;

  private static string ScenePath
  {
    get => SceneHelper.GetScenePath("screens/card_selection/deck_upgrade_select_screen");
  }

  public static IEnumerable<string> AssetPaths
  {
    get
    {
      return (IEnumerable<string>) new \u003C\u003Ez__ReadOnlySingleElementList<string>(NDeckUpgradeSelectScreen.ScenePath);
    }
  }

  private bool UseSingleSelection => this._prefs.MaxSelect == 1;

  public override void _Ready()
  {
    this.ConnectSignalsAndInitGrid();
    this._upgradeSinglePreviewContainer = ((Node) this).GetNode<Control>(NodePath.op_Implicit("%UpgradeSinglePreviewContainer"));
    this._singlePreview = ((Node) this._upgradeSinglePreviewContainer).GetNode<NUpgradePreview>(NodePath.op_Implicit("UpgradePreview"));
    this._singlePreviewCancelButton = ((Node) this._upgradeSinglePreviewContainer).GetNode<NBackButton>(NodePath.op_Implicit("Cancel"));
    this._singlePreviewConfirmButton = ((Node) this._upgradeSinglePreviewContainer).GetNode<NConfirmButton>(NodePath.op_Implicit("Confirm"));
    this._upgradeMultiPreviewContainer = ((Node) this).GetNode<Control>(NodePath.op_Implicit("%UpgradeMultiPreviewContainer"));
    this._multiPreview = ((Node) this._upgradeMultiPreviewContainer).GetNode<Control>(NodePath.op_Implicit("Cards"));
    this._multiPreviewCancelButton = ((Node) this._upgradeMultiPreviewContainer).GetNode<NBackButton>(NodePath.op_Implicit("Cancel"));
    this._multiPreviewConfirmButton = ((Node) this._upgradeMultiPreviewContainer).GetNode<NConfirmButton>(NodePath.op_Implicit("Confirm"));
    this._closeButton = ((Node) this).GetNode<NBackButton>(NodePath.op_Implicit("%Close"));
    this._bottomTextContainer = ((Node) this).GetNode<Control>(NodePath.op_Implicit("%BottomText"));
    this._infoLabel = ((Node) this._bottomTextContainer).GetNode<MegaRichTextLabel>(NodePath.op_Implicit("%BottomLabel"));
    this._infoLabel.Text = this._prefs.Prompt.GetFormattedText();
    ((GodotObject) this._singlePreviewCancelButton).Connect(NClickableControl.SignalName.Released, Callable.From<NButton>(new Action<NButton>(this.CancelSelection)), 0U);
    ((GodotObject) this._singlePreviewConfirmButton).Connect(NClickableControl.SignalName.Released, Callable.From<NButton>(new Action<NButton>(this.ConfirmSelection)), 0U);
    ((GodotObject) this._multiPreviewCancelButton).Connect(NClickableControl.SignalName.Released, Callable.From<NButton>(new Action<NButton>(this.CancelSelection)), 0U);
    ((GodotObject) this._multiPreviewConfirmButton).Connect(NClickableControl.SignalName.Released, Callable.From<NButton>(new Action<NButton>(this.ConfirmSelection)), 0U);
    ((GodotObject) this._closeButton).Connect(NClickableControl.SignalName.Released, Callable.From<NButton>(new Action<NButton>(this.CloseSelection)), 0U);
    if (this._prefs.Cancelable)
      this._closeButton.Enable();
    else
      this._closeButton.Disable();
    ((CanvasItem) this._upgradeSinglePreviewContainer).Visible = false;
    this._upgradeSinglePreviewContainer.MouseFilter = (Control.MouseFilterEnum) 2L;
    ((CanvasItem) this._upgradeMultiPreviewContainer).Visible = false;
    this._upgradeMultiPreviewContainer.MouseFilter = (Control.MouseFilterEnum) 2L;
    this._singlePreviewCancelButton.Disable();
    this._singlePreviewConfirmButton.Disable();
    this._multiPreviewCancelButton.Disable();
    this._multiPreviewConfirmButton.Disable();
    this._viewUpgrades = ((Node) this).GetNode<NTickbox>(NodePath.op_Implicit("%Upgrades"));
    this._viewUpgrades.IsTicked = false;
    ((GodotObject) this._viewUpgrades).Connect(NTickbox.SignalName.Toggled, Callable.From<NTickbox>(new Action<NTickbox>(this.ToggleShowUpgrades)), 0U);
    this.OnControllerStateUpdated();
    ((GodotObject) NControllerManager.Instance).Connect(NControllerManager.SignalName.MouseDetected, Callable.From(new Action(this.OnControllerStateUpdated)), 0U);
    ((GodotObject) NControllerManager.Instance).Connect(NControllerManager.SignalName.ControllerDetected, Callable.From(new Action(this.OnControllerStateUpdated)), 0U);
    ((GodotObject) NInputManager.Instance).Connect(NInputManager.SignalName.InputRebound, Callable.From(new Action(this.OnControllerStateUpdated)), 0U);
    ((Node) this).GetNode<MegaLabel>(NodePath.op_Implicit("%ViewUpgradesLabel")).SetTextAutoSize(new LocString("card_selection", "VIEW_UPGRADES").GetFormattedText());
  }

  public static NDeckUpgradeSelectScreen ShowScreen(
    IReadOnlyList<CardModel> cards,
    CardSelectorPrefs prefs,
    IRunState runState)
  {
    NDeckUpgradeSelectScreen screen = PreloadManager.Cache.GetScene(NDeckUpgradeSelectScreen.ScenePath).Instantiate<NDeckUpgradeSelectScreen>((PackedScene.GenEditState) 0L);
    ((Node) screen).Name = StringName.op_Implicit(nameof (NDeckUpgradeSelectScreen));
    screen._cards = cards;
    screen._prefs = prefs;
    screen._runState = runState;
    NOverlayStack.Instance.Push((IOverlayScreen) screen);
    return screen;
  }

  protected override IEnumerable<Control> PeekButtonTargets
  {
    get
    {
      return (IEnumerable<Control>) new \u003C\u003Ez__ReadOnlyArray<Control>(new Control[3]
      {
        this._upgradeSinglePreviewContainer,
        this._upgradeMultiPreviewContainer,
        (Control) this._closeButton
      });
    }
  }

  protected override void OnCardClicked(CardModel card)
  {
    if (this._selectedCards.Add(card))
    {
      this._grid.HighlightCard(card);
      if (this.UseSingleSelection)
      {
        ((Node) this).GetViewport().GuiReleaseFocus();
        this._grid.FocusBehaviorRecursive = (Control.FocusBehaviorRecursiveEnum) 1L;
        ((CanvasItem) this._upgradeSinglePreviewContainer).Visible = true;
        this._upgradeSinglePreviewContainer.MouseFilter = (Control.MouseFilterEnum) 0L;
        this._singlePreview.Card = card;
        this._singlePreviewCancelButton.Enable();
        this._singlePreviewConfirmButton.Enable();
        this._grid.SetCanScroll(false);
        this._closeButton.Disable();
      }
      else
      {
        if (this._prefs.MaxSelect != this._selectedCards.Count)
          return;
        ((Node) this).GetViewport().GuiReleaseFocus();
        this._grid.FocusBehaviorRecursive = (Control.FocusBehaviorRecursiveEnum) 1L;
        ((CanvasItem) this._upgradeMultiPreviewContainer).Visible = true;
        this._upgradeMultiPreviewContainer.MouseFilter = (Control.MouseFilterEnum) 0L;
        this._multiPreviewCancelButton.Enable();
        this._multiPreviewConfirmButton.Enable();
        foreach (CardModel selectedCard in this._selectedCards)
        {
          this._grid.UnhighlightCard(selectedCard);
          CardModel card1 = this._runState.CloneCard(selectedCard);
          card1.UpgradeInternal();
          card1.UpgradePreviewType = CardUpgradePreviewType.Deck;
          NCard card2 = NCard.Create(card1);
          ((Node) this._multiPreview).AddChildSafely((Node) NPreviewCardHolder.Create(card2, true, false));
          card2.ShowUpgradePreview();
          this._grid.SetCanScroll(false);
          this._closeButton.Disable();
        }
      }
    }
    else
    {
      this._selectedCards.Remove(card);
      this._grid.UnhighlightCard(card);
    }
  }

  private void CloseSelection(NButton _)
  {
    this._completionSource.SetResult((IEnumerable<CardModel>) Array.Empty<CardModel>());
    this._singlePreviewCancelButton.Disable();
    this._singlePreviewConfirmButton.Disable();
    this._multiPreviewCancelButton.Disable();
    this._multiPreviewConfirmButton.Disable();
    NOverlayStack.Instance.Remove((IOverlayScreen) this);
  }

  private void CancelSelection(NButton _)
  {
    if (this.UseSingleSelection)
    {
      ((CanvasItem) this._upgradeSinglePreviewContainer).Visible = false;
      this._upgradeSinglePreviewContainer.MouseFilter = (Control.MouseFilterEnum) 2L;
      this._singlePreviewCancelButton.Disable();
      this._singlePreviewConfirmButton.Disable();
    }
    else
    {
      ((CanvasItem) this._upgradeMultiPreviewContainer).Visible = false;
      this._upgradeMultiPreviewContainer.MouseFilter = (Control.MouseFilterEnum) 2L;
      for (int index = 0; index < ((Node) this._multiPreview).GetChildCount(false); ++index)
        ((Node) this._multiPreview).GetChild(index, false).QueueFreeSafely();
      this._multiPreviewCancelButton.Disable();
      this._multiPreviewConfirmButton.Disable();
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

  private void ConfirmSelection(NButton _)
  {
    if (this._selectedCards.Count == 0)
      return;
    this.CheckIfSelectionComplete();
  }

  private void CheckIfSelectionComplete()
  {
    this._singlePreviewCancelButton.Enable();
    this._singlePreviewConfirmButton.Enable();
    if (this._selectedCards.Count < this._prefs.MaxSelect)
      return;
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
      return ((CanvasItem) this._upgradeSinglePreviewContainer).Visible || ((CanvasItem) this._upgradeMultiPreviewContainer).Visible ? (Control) null : this._grid.DefaultFocusedControl;
    }
  }

  public override Control? FocusedControlFromTopBar
  {
    get
    {
      return ((CanvasItem) this._upgradeSinglePreviewContainer).Visible || ((CanvasItem) this._upgradeMultiPreviewContainer).Visible ? (Control) null : this._grid.FocusedControlFromTopBar;
    }
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(7)
    {
      new MethodInfo(NDeckUpgradeSelectScreen.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NDeckUpgradeSelectScreen.MethodName.CloseSelection, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("_"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false)
      }, (List<Variant>) null),
      new MethodInfo(NDeckUpgradeSelectScreen.MethodName.CancelSelection, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("_"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false)
      }, (List<Variant>) null),
      new MethodInfo(NDeckUpgradeSelectScreen.MethodName.ConfirmSelection, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("_"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false)
      }, (List<Variant>) null),
      new MethodInfo(NDeckUpgradeSelectScreen.MethodName.CheckIfSelectionComplete, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NDeckUpgradeSelectScreen.MethodName.ToggleShowUpgrades, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("tickbox"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false)
      }, (List<Variant>) null),
      new MethodInfo(NDeckUpgradeSelectScreen.MethodName.OnControllerStateUpdated, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NDeckUpgradeSelectScreen.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NDeckUpgradeSelectScreen.MethodName.CloseSelection) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.CloseSelection(VariantUtils.ConvertTo<NButton>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NDeckUpgradeSelectScreen.MethodName.CancelSelection) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.CancelSelection(VariantUtils.ConvertTo<NButton>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NDeckUpgradeSelectScreen.MethodName.ConfirmSelection) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.ConfirmSelection(VariantUtils.ConvertTo<NButton>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NDeckUpgradeSelectScreen.MethodName.CheckIfSelectionComplete) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.CheckIfSelectionComplete();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NDeckUpgradeSelectScreen.MethodName.ToggleShowUpgrades) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.ToggleShowUpgrades(VariantUtils.ConvertTo<NTickbox>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NDeckUpgradeSelectScreen.MethodName.OnControllerStateUpdated) || ((NativeVariantPtrArgs) ref args).Count != 0)
      return base.InvokeGodotClassMethod(in method, args, out ret);
    this.OnControllerStateUpdated();
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NDeckUpgradeSelectScreen.MethodName._Ready) || StringName.op_Equality(ref method, NDeckUpgradeSelectScreen.MethodName.CloseSelection) || StringName.op_Equality(ref method, NDeckUpgradeSelectScreen.MethodName.CancelSelection) || StringName.op_Equality(ref method, NDeckUpgradeSelectScreen.MethodName.ConfirmSelection) || StringName.op_Equality(ref method, NDeckUpgradeSelectScreen.MethodName.CheckIfSelectionComplete) || StringName.op_Equality(ref method, NDeckUpgradeSelectScreen.MethodName.ToggleShowUpgrades) || StringName.op_Equality(ref method, NDeckUpgradeSelectScreen.MethodName.OnControllerStateUpdated) || base.HasGodotClassMethod(in method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NDeckUpgradeSelectScreen.PropertyName._upgradeSinglePreviewContainer))
    {
      this._upgradeSinglePreviewContainer = VariantUtils.ConvertTo<Control>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NDeckUpgradeSelectScreen.PropertyName._singlePreview))
    {
      this._singlePreview = VariantUtils.ConvertTo<NUpgradePreview>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NDeckUpgradeSelectScreen.PropertyName._singlePreviewCancelButton))
    {
      this._singlePreviewCancelButton = VariantUtils.ConvertTo<NBackButton>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NDeckUpgradeSelectScreen.PropertyName._singlePreviewConfirmButton))
    {
      this._singlePreviewConfirmButton = VariantUtils.ConvertTo<NConfirmButton>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NDeckUpgradeSelectScreen.PropertyName._viewUpgrades))
    {
      this._viewUpgrades = VariantUtils.ConvertTo<NTickbox>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NDeckUpgradeSelectScreen.PropertyName._bottomTextContainer))
    {
      this._bottomTextContainer = VariantUtils.ConvertTo<Control>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NDeckUpgradeSelectScreen.PropertyName._infoLabel))
    {
      this._infoLabel = VariantUtils.ConvertTo<MegaRichTextLabel>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NDeckUpgradeSelectScreen.PropertyName._upgradeMultiPreviewContainer))
    {
      this._upgradeMultiPreviewContainer = VariantUtils.ConvertTo<Control>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NDeckUpgradeSelectScreen.PropertyName._multiPreview))
    {
      this._multiPreview = VariantUtils.ConvertTo<Control>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NDeckUpgradeSelectScreen.PropertyName._multiPreviewCancelButton))
    {
      this._multiPreviewCancelButton = VariantUtils.ConvertTo<NBackButton>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NDeckUpgradeSelectScreen.PropertyName._multiPreviewConfirmButton))
    {
      this._multiPreviewConfirmButton = VariantUtils.ConvertTo<NConfirmButton>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NDeckUpgradeSelectScreen.PropertyName._closeButton))
      return base.SetGodotClassPropertyValue(in name, in value);
    this._closeButton = VariantUtils.ConvertTo<NBackButton>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NDeckUpgradeSelectScreen.PropertyName.UseSingleSelection))
    {
      ref godot_variant local = ref value;
      bool useSingleSelection = this.UseSingleSelection;
      godot_variant from = VariantUtils.CreateFrom<bool>(ref useSingleSelection);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NDeckUpgradeSelectScreen.PropertyName.DefaultFocusedControl))
    {
      ref godot_variant local = ref value;
      Control defaultFocusedControl = this.DefaultFocusedControl;
      godot_variant from = VariantUtils.CreateFrom<Control>(ref defaultFocusedControl);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NDeckUpgradeSelectScreen.PropertyName.FocusedControlFromTopBar))
    {
      ref godot_variant local = ref value;
      Control controlFromTopBar = this.FocusedControlFromTopBar;
      godot_variant from = VariantUtils.CreateFrom<Control>(ref controlFromTopBar);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NDeckUpgradeSelectScreen.PropertyName._upgradeSinglePreviewContainer))
    {
      value = VariantUtils.CreateFrom<Control>(ref this._upgradeSinglePreviewContainer);
      return true;
    }
    if (StringName.op_Equality(ref name, NDeckUpgradeSelectScreen.PropertyName._singlePreview))
    {
      value = VariantUtils.CreateFrom<NUpgradePreview>(ref this._singlePreview);
      return true;
    }
    if (StringName.op_Equality(ref name, NDeckUpgradeSelectScreen.PropertyName._singlePreviewCancelButton))
    {
      value = VariantUtils.CreateFrom<NBackButton>(ref this._singlePreviewCancelButton);
      return true;
    }
    if (StringName.op_Equality(ref name, NDeckUpgradeSelectScreen.PropertyName._singlePreviewConfirmButton))
    {
      value = VariantUtils.CreateFrom<NConfirmButton>(ref this._singlePreviewConfirmButton);
      return true;
    }
    if (StringName.op_Equality(ref name, NDeckUpgradeSelectScreen.PropertyName._viewUpgrades))
    {
      value = VariantUtils.CreateFrom<NTickbox>(ref this._viewUpgrades);
      return true;
    }
    if (StringName.op_Equality(ref name, NDeckUpgradeSelectScreen.PropertyName._bottomTextContainer))
    {
      value = VariantUtils.CreateFrom<Control>(ref this._bottomTextContainer);
      return true;
    }
    if (StringName.op_Equality(ref name, NDeckUpgradeSelectScreen.PropertyName._infoLabel))
    {
      value = VariantUtils.CreateFrom<MegaRichTextLabel>(ref this._infoLabel);
      return true;
    }
    if (StringName.op_Equality(ref name, NDeckUpgradeSelectScreen.PropertyName._upgradeMultiPreviewContainer))
    {
      value = VariantUtils.CreateFrom<Control>(ref this._upgradeMultiPreviewContainer);
      return true;
    }
    if (StringName.op_Equality(ref name, NDeckUpgradeSelectScreen.PropertyName._multiPreview))
    {
      value = VariantUtils.CreateFrom<Control>(ref this._multiPreview);
      return true;
    }
    if (StringName.op_Equality(ref name, NDeckUpgradeSelectScreen.PropertyName._multiPreviewCancelButton))
    {
      value = VariantUtils.CreateFrom<NBackButton>(ref this._multiPreviewCancelButton);
      return true;
    }
    if (StringName.op_Equality(ref name, NDeckUpgradeSelectScreen.PropertyName._multiPreviewConfirmButton))
    {
      value = VariantUtils.CreateFrom<NConfirmButton>(ref this._multiPreviewConfirmButton);
      return true;
    }
    if (!StringName.op_Equality(ref name, NDeckUpgradeSelectScreen.PropertyName._closeButton))
      return base.GetGodotClassPropertyValue(in name, out value);
    value = VariantUtils.CreateFrom<NBackButton>(ref this._closeButton);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 1L, NDeckUpgradeSelectScreen.PropertyName.UseSingleSelection, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NDeckUpgradeSelectScreen.PropertyName._upgradeSinglePreviewContainer, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NDeckUpgradeSelectScreen.PropertyName._singlePreview, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NDeckUpgradeSelectScreen.PropertyName._singlePreviewCancelButton, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NDeckUpgradeSelectScreen.PropertyName._singlePreviewConfirmButton, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NDeckUpgradeSelectScreen.PropertyName._viewUpgrades, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NDeckUpgradeSelectScreen.PropertyName._bottomTextContainer, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NDeckUpgradeSelectScreen.PropertyName._infoLabel, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NDeckUpgradeSelectScreen.PropertyName._upgradeMultiPreviewContainer, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NDeckUpgradeSelectScreen.PropertyName._multiPreview, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NDeckUpgradeSelectScreen.PropertyName._multiPreviewCancelButton, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NDeckUpgradeSelectScreen.PropertyName._multiPreviewConfirmButton, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NDeckUpgradeSelectScreen.PropertyName._closeButton, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NDeckUpgradeSelectScreen.PropertyName.DefaultFocusedControl, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NDeckUpgradeSelectScreen.PropertyName.FocusedControlFromTopBar, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    base.SaveGodotObjectData(info);
    info.AddProperty(NDeckUpgradeSelectScreen.PropertyName._upgradeSinglePreviewContainer, Variant.From<Control>(ref this._upgradeSinglePreviewContainer));
    info.AddProperty(NDeckUpgradeSelectScreen.PropertyName._singlePreview, Variant.From<NUpgradePreview>(ref this._singlePreview));
    info.AddProperty(NDeckUpgradeSelectScreen.PropertyName._singlePreviewCancelButton, Variant.From<NBackButton>(ref this._singlePreviewCancelButton));
    info.AddProperty(NDeckUpgradeSelectScreen.PropertyName._singlePreviewConfirmButton, Variant.From<NConfirmButton>(ref this._singlePreviewConfirmButton));
    info.AddProperty(NDeckUpgradeSelectScreen.PropertyName._viewUpgrades, Variant.From<NTickbox>(ref this._viewUpgrades));
    info.AddProperty(NDeckUpgradeSelectScreen.PropertyName._bottomTextContainer, Variant.From<Control>(ref this._bottomTextContainer));
    info.AddProperty(NDeckUpgradeSelectScreen.PropertyName._infoLabel, Variant.From<MegaRichTextLabel>(ref this._infoLabel));
    info.AddProperty(NDeckUpgradeSelectScreen.PropertyName._upgradeMultiPreviewContainer, Variant.From<Control>(ref this._upgradeMultiPreviewContainer));
    info.AddProperty(NDeckUpgradeSelectScreen.PropertyName._multiPreview, Variant.From<Control>(ref this._multiPreview));
    info.AddProperty(NDeckUpgradeSelectScreen.PropertyName._multiPreviewCancelButton, Variant.From<NBackButton>(ref this._multiPreviewCancelButton));
    info.AddProperty(NDeckUpgradeSelectScreen.PropertyName._multiPreviewConfirmButton, Variant.From<NConfirmButton>(ref this._multiPreviewConfirmButton));
    info.AddProperty(NDeckUpgradeSelectScreen.PropertyName._closeButton, Variant.From<NBackButton>(ref this._closeButton));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    base.RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NDeckUpgradeSelectScreen.PropertyName._upgradeSinglePreviewContainer, ref variant1))
      this._upgradeSinglePreviewContainer = ((Variant) ref variant1).As<Control>();
    Variant variant2;
    if (info.TryGetProperty(NDeckUpgradeSelectScreen.PropertyName._singlePreview, ref variant2))
      this._singlePreview = ((Variant) ref variant2).As<NUpgradePreview>();
    Variant variant3;
    if (info.TryGetProperty(NDeckUpgradeSelectScreen.PropertyName._singlePreviewCancelButton, ref variant3))
      this._singlePreviewCancelButton = ((Variant) ref variant3).As<NBackButton>();
    Variant variant4;
    if (info.TryGetProperty(NDeckUpgradeSelectScreen.PropertyName._singlePreviewConfirmButton, ref variant4))
      this._singlePreviewConfirmButton = ((Variant) ref variant4).As<NConfirmButton>();
    Variant variant5;
    if (info.TryGetProperty(NDeckUpgradeSelectScreen.PropertyName._viewUpgrades, ref variant5))
      this._viewUpgrades = ((Variant) ref variant5).As<NTickbox>();
    Variant variant6;
    if (info.TryGetProperty(NDeckUpgradeSelectScreen.PropertyName._bottomTextContainer, ref variant6))
      this._bottomTextContainer = ((Variant) ref variant6).As<Control>();
    Variant variant7;
    if (info.TryGetProperty(NDeckUpgradeSelectScreen.PropertyName._infoLabel, ref variant7))
      this._infoLabel = ((Variant) ref variant7).As<MegaRichTextLabel>();
    Variant variant8;
    if (info.TryGetProperty(NDeckUpgradeSelectScreen.PropertyName._upgradeMultiPreviewContainer, ref variant8))
      this._upgradeMultiPreviewContainer = ((Variant) ref variant8).As<Control>();
    Variant variant9;
    if (info.TryGetProperty(NDeckUpgradeSelectScreen.PropertyName._multiPreview, ref variant9))
      this._multiPreview = ((Variant) ref variant9).As<Control>();
    Variant variant10;
    if (info.TryGetProperty(NDeckUpgradeSelectScreen.PropertyName._multiPreviewCancelButton, ref variant10))
      this._multiPreviewCancelButton = ((Variant) ref variant10).As<NBackButton>();
    Variant variant11;
    if (info.TryGetProperty(NDeckUpgradeSelectScreen.PropertyName._multiPreviewConfirmButton, ref variant11))
      this._multiPreviewConfirmButton = ((Variant) ref variant11).As<NConfirmButton>();
    Variant variant12;
    if (!info.TryGetProperty(NDeckUpgradeSelectScreen.PropertyName._closeButton, ref variant12))
      return;
    this._closeButton = ((Variant) ref variant12).As<NBackButton>();
  }

  public new class MethodName : NCardGridSelectionScreen.MethodName
  {
    public new static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName CloseSelection = StringName.op_Implicit(nameof (CloseSelection));
    public static readonly StringName CancelSelection = StringName.op_Implicit(nameof (CancelSelection));
    public static readonly StringName ConfirmSelection = StringName.op_Implicit(nameof (ConfirmSelection));
    public static readonly StringName CheckIfSelectionComplete = StringName.op_Implicit(nameof (CheckIfSelectionComplete));
    public static readonly StringName ToggleShowUpgrades = StringName.op_Implicit(nameof (ToggleShowUpgrades));
    public static readonly StringName OnControllerStateUpdated = StringName.op_Implicit(nameof (OnControllerStateUpdated));
  }

  public new class PropertyName : NCardGridSelectionScreen.PropertyName
  {
    public static readonly StringName UseSingleSelection = StringName.op_Implicit(nameof (UseSingleSelection));
    public new static readonly StringName DefaultFocusedControl = StringName.op_Implicit(nameof (DefaultFocusedControl));
    public new static readonly StringName FocusedControlFromTopBar = StringName.op_Implicit(nameof (FocusedControlFromTopBar));
    public static readonly StringName _upgradeSinglePreviewContainer = StringName.op_Implicit(nameof (_upgradeSinglePreviewContainer));
    public static readonly StringName _singlePreview = StringName.op_Implicit(nameof (_singlePreview));
    public static readonly StringName _singlePreviewCancelButton = StringName.op_Implicit(nameof (_singlePreviewCancelButton));
    public static readonly StringName _singlePreviewConfirmButton = StringName.op_Implicit(nameof (_singlePreviewConfirmButton));
    public static readonly StringName _viewUpgrades = StringName.op_Implicit(nameof (_viewUpgrades));
    public static readonly StringName _bottomTextContainer = StringName.op_Implicit(nameof (_bottomTextContainer));
    public static readonly StringName _infoLabel = StringName.op_Implicit(nameof (_infoLabel));
    public static readonly StringName _upgradeMultiPreviewContainer = StringName.op_Implicit(nameof (_upgradeMultiPreviewContainer));
    public static readonly StringName _multiPreview = StringName.op_Implicit(nameof (_multiPreview));
    public static readonly StringName _multiPreviewCancelButton = StringName.op_Implicit(nameof (_multiPreviewCancelButton));
    public static readonly StringName _multiPreviewConfirmButton = StringName.op_Implicit(nameof (_multiPreviewConfirmButton));
    public static readonly StringName _closeButton = StringName.op_Implicit(nameof (_closeButton));
  }

  public new class SignalName : NCardGridSelectionScreen.SignalName
  {
  }
}
