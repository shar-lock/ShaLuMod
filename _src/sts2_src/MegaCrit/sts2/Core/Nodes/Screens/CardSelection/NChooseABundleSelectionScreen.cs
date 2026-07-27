// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Screens.CardSelection.NChooseABundleSelectionScreen
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Cards;
using MegaCrit.Sts2.Core.Nodes.Cards.Holders;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.Screens.Overlays;
using MegaCrit.Sts2.Core.Nodes.Screens.ScreenContext;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Screens.CardSelection;

[ScriptPath("res://src/Core/Nodes/Screens/CardSelection/NChooseABundleSelectionScreen.cs")]
public class NChooseABundleSelectionScreen : Control, IOverlayScreen, IScreenContext
{
  private Control _bundleRow;
  private IReadOnlyList<IReadOnlyList<CardModel>> _bundles;
  private Control _bundlePreviewContainer;
  private Control _bundlePreviewCards;
  private NBackButton _previewCancelButton;
  private NConfirmButton _previewConfirmButton;
  private NCardBundle? _selectedBundle;
  private NCommonBanner _banner;
  private readonly TaskCompletionSource<IEnumerable<IReadOnlyList<CardModel>>> _completionSource = new TaskCompletionSource<IEnumerable<IReadOnlyList<CardModel>>>();
  private NPeekButton _peekButton;
  private Tween? _fadeTween;
  private const float _cardXSpacing = 400f;
  private Tween? _cardTween;

  private static string ScenePath
  {
    get => SceneHelper.GetScenePath("/screens/card_selection/choose_a_bundle_selection_screen");
  }

  public static IEnumerable<string> AssetPaths
  {
    get
    {
      return (IEnumerable<string>) new \u003C\u003Ez__ReadOnlySingleElementList<string>(NChooseABundleSelectionScreen.ScenePath);
    }
  }

  public NetScreenType ScreenType => NetScreenType.CardSelection;

  public override void _Ready()
  {
    this._bundleRow = ((Node) this).GetNode<Control>(NodePath.op_Implicit("%BundleRow"));
    this._bundlePreviewContainer = ((Node) this).GetNode<Control>(NodePath.op_Implicit("%BundlePreviewContainer"));
    this._bundlePreviewCards = ((Node) this).GetNode<Control>(NodePath.op_Implicit("%Cards"));
    this._previewCancelButton = ((Node) this).GetNode<NBackButton>(NodePath.op_Implicit("%Cancel"));
    this._previewConfirmButton = ((Node) this).GetNode<NConfirmButton>(NodePath.op_Implicit("%Confirm"));
    this._banner = ((Node) this).GetNode<NCommonBanner>(NodePath.op_Implicit("Banner"));
    this._banner.label.SetTextAutoSize(new LocString("gameplay_ui", "CHOOSE_A_PACK").GetRawText());
    this._banner.AnimateIn();
    ((GodotObject) this._previewCancelButton).Connect(NClickableControl.SignalName.Released, Callable.From<NButton>(new Action<NButton>(this.CancelSelection)), 0U);
    ((GodotObject) this._previewConfirmButton).Connect(NClickableControl.SignalName.Released, Callable.From<NButton>(new Action<NButton>(this.ConfirmSelection)), 0U);
    this._previewCancelButton.Disable();
    this._previewConfirmButton.Disable();
    Vector2 vector2 = Vector2.op_Multiply(Vector2.op_Multiply(Vector2.op_Multiply(Vector2.Left, (float) (this._bundles.Count - 1)), 400f), 0.5f);
    for (int index = 0; index < this._bundles.Count; ++index)
    {
      NCardBundle child = NCardBundle.Create(this._bundles[index]);
      ((Node) this._bundleRow).AddChildSafely((Node) child);
      ((GodotObject) child).Connect(NCardBundle.SignalName.Clicked, Callable.From<NCardBundle>(new Action<NCardBundle>(this.OnBundleClicked)), 0U);
      child.Scale = child.smallScale;
      NCardBundle ncardBundle = child;
      ncardBundle.Position = Vector2.op_Addition(ncardBundle.Position, Vector2.op_Addition(vector2, Vector2.op_Multiply(Vector2.op_Multiply(Vector2.Right, 400f), (float) index)));
    }
    for (int index = 0; index < ((Node) this._bundleRow).GetChildCount(false); ++index)
    {
      NCardBundle child = ((Node) this._bundleRow).GetChild<NCardBundle>(index, false);
      child.Hitbox.FocusNeighborLeft = index > 0 ? ((Node) ((Node) this._bundleRow).GetChild<NCardBundle>(index - 1, false).Hitbox).GetPath() : ((Node) ((Node) this._bundleRow).GetChild<NCardBundle>(((Node) this._bundleRow).GetChildCount(false) - 1, false).Hitbox).GetPath();
      child.Hitbox.FocusNeighborRight = index < ((Node) this._bundleRow).GetChildCount(false) - 1 ? ((Node) ((Node) this._bundleRow).GetChild<NCardBundle>(index + 1, false).Hitbox).GetPath() : ((Node) ((Node) this._bundleRow).GetChild<NCardBundle>(0, false).Hitbox).GetPath();
      child.Hitbox.FocusNeighborTop = ((Node) child.Hitbox).GetPath();
      child.Hitbox.FocusNeighborBottom = ((Node) child.Hitbox).GetPath();
    }
    ((CanvasItem) this._bundlePreviewContainer).Visible = false;
    this._bundlePreviewContainer.MouseFilter = (Control.MouseFilterEnum) 2L;
    this._peekButton = ((Node) this).GetNode<NPeekButton>(NodePath.op_Implicit("%PeekButton"));
    this._peekButton.AddTargets((Control) this._banner, this._bundleRow, this._bundlePreviewContainer);
  }

  public override void _ExitTree()
  {
    ((Node) this)._ExitTree();
    if (((Task) this._completionSource.Task).IsCompleted)
      return;
    this._completionSource.SetCanceled();
  }

  public static NChooseABundleSelectionScreen ShowScreen(
    IReadOnlyList<IReadOnlyList<CardModel>> bundles)
  {
    NChooseABundleSelectionScreen screen = PreloadManager.Cache.GetScene(NChooseABundleSelectionScreen.ScenePath).Instantiate<NChooseABundleSelectionScreen>((PackedScene.GenEditState) 0L);
    ((Node) screen).Name = StringName.op_Implicit(nameof (NChooseABundleSelectionScreen));
    screen._bundles = bundles;
    NOverlayStack.Instance?.Push((IOverlayScreen) screen);
    return screen;
  }

  private void OnBundleClicked(NCardBundle bundleNode)
  {
    this._banner.AnimateOut();
    this._selectedBundle = bundleNode;
    ((CanvasItem) this._bundlePreviewContainer).Visible = true;
    this._bundlePreviewContainer.MouseFilter = (Control.MouseFilterEnum) 0L;
    ((CanvasItem) this._bundleRow).Visible = false;
    this._previewCancelButton.Enable();
    this._previewConfirmButton.Enable();
    Vector2 vector2 = Vector2.op_Multiply(Vector2.op_Multiply(Vector2.op_Multiply(Vector2.Right, (float) (bundleNode.Bundle.Count - 1)), 400f), 0.5f);
    IReadOnlyList<NCard> ncardList = bundleNode.RemoveCardNodes();
    this._cardTween?.Kill();
    this._cardTween = ((Node) this).CreateTween().SetParallel(true);
    for (int index = 0; index < ncardList.Count; ++index)
    {
      Vector2 globalPosition = ncardList[index].GlobalPosition;
      NPreviewCardHolder child = NPreviewCardHolder.Create(ncardList[index], true, true);
      ((Node) this._bundlePreviewCards).AddChildSafely((Node) child);
      child.GlobalPosition = globalPosition;
      ((GodotObject) child).Connect(NCardHolder.SignalName.Pressed, Callable.From<NCardHolder>(new Action<NCardHolder>(this.OpenPreviewScreen)), 0U);
      ncardList[index].UpdateVisuals(PileType.None, CardPreviewMode.Normal);
      this._cardTween.TweenProperty((GodotObject) child, NodePath.op_Implicit("position"), Variant.op_Implicit(Vector2.op_Addition(vector2, Vector2.op_Multiply(Vector2.op_Multiply(Vector2.Left, 400f), (float) index))), 0.5).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 5L);
    }
    for (int index = 0; index < ((Node) this._bundlePreviewCards).GetChildCount(false); ++index)
    {
      NPreviewCardHolder child = ((Node) this._bundlePreviewCards).GetChild<NPreviewCardHolder>(index, false);
      child.FocusNeighborLeft = index < ((Node) this._bundlePreviewCards).GetChildCount(false) - 1 ? ((Node) this._bundlePreviewCards).GetChild(index + 1, false).GetPath() : ((Node) this._bundlePreviewCards).GetChild(0, false).GetPath();
      child.FocusNeighborRight = index > 0 ? ((Node) this._bundlePreviewCards).GetChild(index - 1, false).GetPath() : ((Node) this._bundlePreviewCards).GetChild(((Node) this._bundlePreviewCards).GetChildCount(false) - 1, false).GetPath();
      child.FocusNeighborTop = ((Node) child.Hitbox).GetPath();
      child.FocusNeighborBottom = ((Node) child.Hitbox).GetPath();
    }
    ((Node) this._bundlePreviewCards).GetChild<Control>(((Node) this._bundlePreviewCards).GetChildCount(false) - 1, false).TryGrabFocus();
  }

  private void OpenPreviewScreen(NCardHolder cardHolder)
  {
    NInspectCardScreen inspectCardScreen = NGame.Instance.GetInspectCardScreen();
    int capacity = 1;
    List<CardModel> cards = new List<CardModel>(capacity);
    CollectionsMarshal.SetCount<CardModel>(cards, capacity);
    CollectionsMarshal.AsSpan<CardModel>(cards)[0] = cardHolder.CardNode.Model;
    inspectCardScreen.Open(cards, 0);
  }

  private void CancelSelection(NButton _)
  {
    this._banner.AnimateIn();
    ((CanvasItem) this._bundlePreviewContainer).Visible = false;
    this._bundlePreviewContainer.MouseFilter = (Control.MouseFilterEnum) 2L;
    this._cardTween?.Kill();
    this._selectedBundle?.ReAddCardNodes();
    NCardBundle selectedBundle = this._selectedBundle;
    if (selectedBundle != null)
      selectedBundle.Hitbox.TryGrabFocus();
    this._previewCancelButton.Disable();
    this._previewConfirmButton.Disable();
    this._selectedBundle = (NCardBundle) null;
    ((CanvasItem) this._bundleRow).Visible = true;
  }

  private void ConfirmSelection(NButton _)
  {
    foreach (NCard cardNode in (IEnumerable<NCard>) this._selectedBundle.CardNodes)
    {
      NRun.Instance.GlobalUi.ReparentCard(cardNode);
      NRun.Instance.GlobalUi.TopBar.TrailContainer.AddChildSafely((Node) NCardFlyVfx.Create(cardNode, PileType.Deck, true, cardNode.Model.Owner.Character.TrailPath));
    }
    // ISSUE: object of a compiler-generated type is created
    this._completionSource.SetResult((IEnumerable<IReadOnlyList<CardModel>>) new \u003C\u003Ez__ReadOnlySingleElementList<IReadOnlyList<CardModel>>(this._selectedBundle.Bundle));
  }

  public async Task<IEnumerable<IReadOnlyList<CardModel>>> CardsSelected()
  {
    IEnumerable<IReadOnlyList<CardModel>> task = await this._completionSource.Task;
    NOverlayStack.Instance.Remove((IOverlayScreen) this);
    return task;
  }

  public void AfterOverlayOpened()
  {
    ((CanvasItem) this).Modulate = Colors.Transparent;
    this._fadeTween?.Kill();
    this._fadeTween = ((Node) this).CreateTween();
    this._fadeTween.TweenProperty((GodotObject) this, NodePath.op_Implicit("modulate:a"), Variant.op_Implicit(1f), 0.4);
  }

  public void AfterOverlayClosed()
  {
    this._fadeTween?.Kill();
    ((Node) this).QueueFreeSafely();
  }

  public void AfterOverlayShown()
  {
    ((CanvasItem) this).Visible = true;
    if (!((CanvasItem) this._bundlePreviewContainer).Visible)
      return;
    this._previewCancelButton.Enable();
    this._previewConfirmButton.Enable();
  }

  public void AfterOverlayHidden()
  {
    ((CanvasItem) this).Visible = false;
    this._previewCancelButton.Disable();
    this._previewConfirmButton.Disable();
  }

  public bool UseSharedBackstop => true;

  public Control DefaultFocusedControl
  {
    get
    {
      return !((CanvasItem) this._bundlePreviewContainer).Visible ? (Control) ((Node) this._bundleRow).GetChild<NCardBundle>(0, false).Hitbox : ((Node) this._bundlePreviewCards).GetChild<Control>(((Node) this._bundlePreviewCards).GetChildCount(false) - 1, false);
    }
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(10)
    {
      new MethodInfo(NChooseABundleSelectionScreen.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NChooseABundleSelectionScreen.MethodName._ExitTree, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NChooseABundleSelectionScreen.MethodName.OnBundleClicked, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("bundleNode"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false)
      }, (List<Variant>) null),
      new MethodInfo(NChooseABundleSelectionScreen.MethodName.OpenPreviewScreen, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("cardHolder"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false)
      }, (List<Variant>) null),
      new MethodInfo(NChooseABundleSelectionScreen.MethodName.CancelSelection, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("_"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false)
      }, (List<Variant>) null),
      new MethodInfo(NChooseABundleSelectionScreen.MethodName.ConfirmSelection, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("_"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false)
      }, (List<Variant>) null),
      new MethodInfo(NChooseABundleSelectionScreen.MethodName.AfterOverlayOpened, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NChooseABundleSelectionScreen.MethodName.AfterOverlayClosed, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NChooseABundleSelectionScreen.MethodName.AfterOverlayShown, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NChooseABundleSelectionScreen.MethodName.AfterOverlayHidden, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NChooseABundleSelectionScreen.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NChooseABundleSelectionScreen.MethodName._ExitTree) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._ExitTree();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NChooseABundleSelectionScreen.MethodName.OnBundleClicked) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.OnBundleClicked(VariantUtils.ConvertTo<NCardBundle>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NChooseABundleSelectionScreen.MethodName.OpenPreviewScreen) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.OpenPreviewScreen(VariantUtils.ConvertTo<NCardHolder>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NChooseABundleSelectionScreen.MethodName.CancelSelection) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.CancelSelection(VariantUtils.ConvertTo<NButton>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NChooseABundleSelectionScreen.MethodName.ConfirmSelection) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.ConfirmSelection(VariantUtils.ConvertTo<NButton>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NChooseABundleSelectionScreen.MethodName.AfterOverlayOpened) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.AfterOverlayOpened();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NChooseABundleSelectionScreen.MethodName.AfterOverlayClosed) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.AfterOverlayClosed();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NChooseABundleSelectionScreen.MethodName.AfterOverlayShown) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.AfterOverlayShown();
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NChooseABundleSelectionScreen.MethodName.AfterOverlayHidden) || ((NativeVariantPtrArgs) ref args).Count != 0)
      return base.InvokeGodotClassMethod(ref method, args, ref ret);
    this.AfterOverlayHidden();
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NChooseABundleSelectionScreen.MethodName._Ready) || StringName.op_Equality(ref method, NChooseABundleSelectionScreen.MethodName._ExitTree) || StringName.op_Equality(ref method, NChooseABundleSelectionScreen.MethodName.OnBundleClicked) || StringName.op_Equality(ref method, NChooseABundleSelectionScreen.MethodName.OpenPreviewScreen) || StringName.op_Equality(ref method, NChooseABundleSelectionScreen.MethodName.CancelSelection) || StringName.op_Equality(ref method, NChooseABundleSelectionScreen.MethodName.ConfirmSelection) || StringName.op_Equality(ref method, NChooseABundleSelectionScreen.MethodName.AfterOverlayOpened) || StringName.op_Equality(ref method, NChooseABundleSelectionScreen.MethodName.AfterOverlayClosed) || StringName.op_Equality(ref method, NChooseABundleSelectionScreen.MethodName.AfterOverlayShown) || StringName.op_Equality(ref method, NChooseABundleSelectionScreen.MethodName.AfterOverlayHidden) || base.HasGodotClassMethod(ref method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NChooseABundleSelectionScreen.PropertyName._bundleRow))
    {
      this._bundleRow = VariantUtils.ConvertTo<Control>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NChooseABundleSelectionScreen.PropertyName._bundlePreviewContainer))
    {
      this._bundlePreviewContainer = VariantUtils.ConvertTo<Control>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NChooseABundleSelectionScreen.PropertyName._bundlePreviewCards))
    {
      this._bundlePreviewCards = VariantUtils.ConvertTo<Control>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NChooseABundleSelectionScreen.PropertyName._previewCancelButton))
    {
      this._previewCancelButton = VariantUtils.ConvertTo<NBackButton>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NChooseABundleSelectionScreen.PropertyName._previewConfirmButton))
    {
      this._previewConfirmButton = VariantUtils.ConvertTo<NConfirmButton>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NChooseABundleSelectionScreen.PropertyName._selectedBundle))
    {
      this._selectedBundle = VariantUtils.ConvertTo<NCardBundle>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NChooseABundleSelectionScreen.PropertyName._banner))
    {
      this._banner = VariantUtils.ConvertTo<NCommonBanner>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NChooseABundleSelectionScreen.PropertyName._peekButton))
    {
      this._peekButton = VariantUtils.ConvertTo<NPeekButton>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NChooseABundleSelectionScreen.PropertyName._fadeTween))
    {
      this._fadeTween = VariantUtils.ConvertTo<Tween>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NChooseABundleSelectionScreen.PropertyName._cardTween))
      return ((GodotObject) this).SetGodotClassPropertyValue(ref name, ref value);
    this._cardTween = VariantUtils.ConvertTo<Tween>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NChooseABundleSelectionScreen.PropertyName.ScreenType))
    {
      ref godot_variant local = ref value;
      NetScreenType screenType = this.ScreenType;
      godot_variant from = VariantUtils.CreateFrom<NetScreenType>(ref screenType);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NChooseABundleSelectionScreen.PropertyName.UseSharedBackstop))
    {
      ref godot_variant local = ref value;
      bool useSharedBackstop = this.UseSharedBackstop;
      godot_variant from = VariantUtils.CreateFrom<bool>(ref useSharedBackstop);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NChooseABundleSelectionScreen.PropertyName.DefaultFocusedControl))
    {
      ref godot_variant local = ref value;
      Control defaultFocusedControl = this.DefaultFocusedControl;
      godot_variant from = VariantUtils.CreateFrom<Control>(ref defaultFocusedControl);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NChooseABundleSelectionScreen.PropertyName._bundleRow))
    {
      value = VariantUtils.CreateFrom<Control>(ref this._bundleRow);
      return true;
    }
    if (StringName.op_Equality(ref name, NChooseABundleSelectionScreen.PropertyName._bundlePreviewContainer))
    {
      value = VariantUtils.CreateFrom<Control>(ref this._bundlePreviewContainer);
      return true;
    }
    if (StringName.op_Equality(ref name, NChooseABundleSelectionScreen.PropertyName._bundlePreviewCards))
    {
      value = VariantUtils.CreateFrom<Control>(ref this._bundlePreviewCards);
      return true;
    }
    if (StringName.op_Equality(ref name, NChooseABundleSelectionScreen.PropertyName._previewCancelButton))
    {
      value = VariantUtils.CreateFrom<NBackButton>(ref this._previewCancelButton);
      return true;
    }
    if (StringName.op_Equality(ref name, NChooseABundleSelectionScreen.PropertyName._previewConfirmButton))
    {
      value = VariantUtils.CreateFrom<NConfirmButton>(ref this._previewConfirmButton);
      return true;
    }
    if (StringName.op_Equality(ref name, NChooseABundleSelectionScreen.PropertyName._selectedBundle))
    {
      value = VariantUtils.CreateFrom<NCardBundle>(ref this._selectedBundle);
      return true;
    }
    if (StringName.op_Equality(ref name, NChooseABundleSelectionScreen.PropertyName._banner))
    {
      value = VariantUtils.CreateFrom<NCommonBanner>(ref this._banner);
      return true;
    }
    if (StringName.op_Equality(ref name, NChooseABundleSelectionScreen.PropertyName._peekButton))
    {
      value = VariantUtils.CreateFrom<NPeekButton>(ref this._peekButton);
      return true;
    }
    if (StringName.op_Equality(ref name, NChooseABundleSelectionScreen.PropertyName._fadeTween))
    {
      value = VariantUtils.CreateFrom<Tween>(ref this._fadeTween);
      return true;
    }
    if (!StringName.op_Equality(ref name, NChooseABundleSelectionScreen.PropertyName._cardTween))
      return ((GodotObject) this).GetGodotClassPropertyValue(ref name, ref value);
    value = VariantUtils.CreateFrom<Tween>(ref this._cardTween);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NChooseABundleSelectionScreen.PropertyName._bundleRow, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NChooseABundleSelectionScreen.PropertyName._bundlePreviewContainer, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NChooseABundleSelectionScreen.PropertyName._bundlePreviewCards, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NChooseABundleSelectionScreen.PropertyName._previewCancelButton, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NChooseABundleSelectionScreen.PropertyName._previewConfirmButton, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NChooseABundleSelectionScreen.PropertyName._selectedBundle, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NChooseABundleSelectionScreen.PropertyName._banner, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NChooseABundleSelectionScreen.PropertyName._peekButton, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NChooseABundleSelectionScreen.PropertyName._fadeTween, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NChooseABundleSelectionScreen.PropertyName._cardTween, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 2L, NChooseABundleSelectionScreen.PropertyName.ScreenType, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 1L, NChooseABundleSelectionScreen.PropertyName.UseSharedBackstop, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NChooseABundleSelectionScreen.PropertyName.DefaultFocusedControl, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).SaveGodotObjectData(info);
    info.AddProperty(NChooseABundleSelectionScreen.PropertyName._bundleRow, Variant.From<Control>(ref this._bundleRow));
    info.AddProperty(NChooseABundleSelectionScreen.PropertyName._bundlePreviewContainer, Variant.From<Control>(ref this._bundlePreviewContainer));
    info.AddProperty(NChooseABundleSelectionScreen.PropertyName._bundlePreviewCards, Variant.From<Control>(ref this._bundlePreviewCards));
    info.AddProperty(NChooseABundleSelectionScreen.PropertyName._previewCancelButton, Variant.From<NBackButton>(ref this._previewCancelButton));
    info.AddProperty(NChooseABundleSelectionScreen.PropertyName._previewConfirmButton, Variant.From<NConfirmButton>(ref this._previewConfirmButton));
    info.AddProperty(NChooseABundleSelectionScreen.PropertyName._selectedBundle, Variant.From<NCardBundle>(ref this._selectedBundle));
    info.AddProperty(NChooseABundleSelectionScreen.PropertyName._banner, Variant.From<NCommonBanner>(ref this._banner));
    info.AddProperty(NChooseABundleSelectionScreen.PropertyName._peekButton, Variant.From<NPeekButton>(ref this._peekButton));
    info.AddProperty(NChooseABundleSelectionScreen.PropertyName._fadeTween, Variant.From<Tween>(ref this._fadeTween));
    info.AddProperty(NChooseABundleSelectionScreen.PropertyName._cardTween, Variant.From<Tween>(ref this._cardTween));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NChooseABundleSelectionScreen.PropertyName._bundleRow, ref variant1))
      this._bundleRow = ((Variant) ref variant1).As<Control>();
    Variant variant2;
    if (info.TryGetProperty(NChooseABundleSelectionScreen.PropertyName._bundlePreviewContainer, ref variant2))
      this._bundlePreviewContainer = ((Variant) ref variant2).As<Control>();
    Variant variant3;
    if (info.TryGetProperty(NChooseABundleSelectionScreen.PropertyName._bundlePreviewCards, ref variant3))
      this._bundlePreviewCards = ((Variant) ref variant3).As<Control>();
    Variant variant4;
    if (info.TryGetProperty(NChooseABundleSelectionScreen.PropertyName._previewCancelButton, ref variant4))
      this._previewCancelButton = ((Variant) ref variant4).As<NBackButton>();
    Variant variant5;
    if (info.TryGetProperty(NChooseABundleSelectionScreen.PropertyName._previewConfirmButton, ref variant5))
      this._previewConfirmButton = ((Variant) ref variant5).As<NConfirmButton>();
    Variant variant6;
    if (info.TryGetProperty(NChooseABundleSelectionScreen.PropertyName._selectedBundle, ref variant6))
      this._selectedBundle = ((Variant) ref variant6).As<NCardBundle>();
    Variant variant7;
    if (info.TryGetProperty(NChooseABundleSelectionScreen.PropertyName._banner, ref variant7))
      this._banner = ((Variant) ref variant7).As<NCommonBanner>();
    Variant variant8;
    if (info.TryGetProperty(NChooseABundleSelectionScreen.PropertyName._peekButton, ref variant8))
      this._peekButton = ((Variant) ref variant8).As<NPeekButton>();
    Variant variant9;
    if (info.TryGetProperty(NChooseABundleSelectionScreen.PropertyName._fadeTween, ref variant9))
      this._fadeTween = ((Variant) ref variant9).As<Tween>();
    Variant variant10;
    if (!info.TryGetProperty(NChooseABundleSelectionScreen.PropertyName._cardTween, ref variant10))
      return;
    this._cardTween = ((Variant) ref variant10).As<Tween>();
  }

  public class MethodName : Control.MethodName
  {
    public static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName _ExitTree = StringName.op_Implicit(nameof (_ExitTree));
    public static readonly StringName OnBundleClicked = StringName.op_Implicit(nameof (OnBundleClicked));
    public static readonly StringName OpenPreviewScreen = StringName.op_Implicit(nameof (OpenPreviewScreen));
    public static readonly StringName CancelSelection = StringName.op_Implicit(nameof (CancelSelection));
    public static readonly StringName ConfirmSelection = StringName.op_Implicit(nameof (ConfirmSelection));
    public static readonly StringName AfterOverlayOpened = StringName.op_Implicit(nameof (AfterOverlayOpened));
    public static readonly StringName AfterOverlayClosed = StringName.op_Implicit(nameof (AfterOverlayClosed));
    public static readonly StringName AfterOverlayShown = StringName.op_Implicit(nameof (AfterOverlayShown));
    public static readonly StringName AfterOverlayHidden = StringName.op_Implicit(nameof (AfterOverlayHidden));
  }

  public class PropertyName : Control.PropertyName
  {
    public static readonly StringName ScreenType = StringName.op_Implicit(nameof (ScreenType));
    public static readonly StringName UseSharedBackstop = StringName.op_Implicit(nameof (UseSharedBackstop));
    public static readonly StringName DefaultFocusedControl = StringName.op_Implicit(nameof (DefaultFocusedControl));
    public static readonly StringName _bundleRow = StringName.op_Implicit(nameof (_bundleRow));
    public static readonly StringName _bundlePreviewContainer = StringName.op_Implicit(nameof (_bundlePreviewContainer));
    public static readonly StringName _bundlePreviewCards = StringName.op_Implicit(nameof (_bundlePreviewCards));
    public static readonly StringName _previewCancelButton = StringName.op_Implicit(nameof (_previewCancelButton));
    public static readonly StringName _previewConfirmButton = StringName.op_Implicit(nameof (_previewConfirmButton));
    public static readonly StringName _selectedBundle = StringName.op_Implicit(nameof (_selectedBundle));
    public static readonly StringName _banner = StringName.op_Implicit(nameof (_banner));
    public static readonly StringName _peekButton = StringName.op_Implicit(nameof (_peekButton));
    public static readonly StringName _fadeTween = StringName.op_Implicit(nameof (_fadeTween));
    public static readonly StringName _cardTween = StringName.op_Implicit(nameof (_cardTween));
  }

  public class SignalName : Control.SignalName
  {
  }
}
