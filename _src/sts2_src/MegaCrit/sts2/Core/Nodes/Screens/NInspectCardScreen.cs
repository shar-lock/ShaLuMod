// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Screens.NInspectCardScreen
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.ControllerInput;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Cards;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.HoverTips;
using MegaCrit.Sts2.Core.Nodes.Screens.ScreenContext;
using MegaCrit.Sts2.Core.TestSupport;
using System;
using System.Collections.Generic;
using System.ComponentModel;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Screens;

[ScriptPath("res://src/Core/Nodes/Screens/NInspectCardScreen.cs")]
public class NInspectCardScreen : Control, IScreenContext
{
  private static readonly string _scenePath = SceneHelper.GetScenePath("screens/inspect_card_screen");
  private NCard _card;
  private NButton _backstop;
  private NTickbox _upgradeTickbox;
  private NButton _leftButton;
  private NButton _rightButton;
  private Control _hoverTipRect;
  private List<CardModel>? _cards;
  private int _index;
  private Tween? _openTween;
  private Tween? _cardTween;
  private Vector2 _cardPosition;
  private float _leftButtonX;
  private float _rightButtonX;
  private const double _arrowButtonDelay = 0.1;
  private bool _viewAllUpgraded;

  public static string[] AssetPaths
  {
    get => new string[1]{ NInspectCardScreen._scenePath };
  }

  private bool IsShowingUpgradedCard => this._upgradeTickbox.IsTicked;

  public static NInspectCardScreen? Create()
  {
    return TestMode.IsOn ? (NInspectCardScreen) null : PreloadManager.Cache.GetScene(NInspectCardScreen._scenePath).Instantiate<NInspectCardScreen>((PackedScene.GenEditState) 0L);
  }

  public override void _Ready()
  {
    this._card = ((Node) this).GetNode<NCard>(NodePath.op_Implicit("Card"));
    this._cardPosition = this._card.Position;
    this._hoverTipRect = ((Node) this).GetNode<Control>(NodePath.op_Implicit("HoverTipRect"));
    this._backstop = ((Node) this).GetNode<NButton>(NodePath.op_Implicit("Backstop"));
    ((GodotObject) this._backstop).Connect(NClickableControl.SignalName.Released, Callable.From<NButton>(new Action<NButton>(this.OnBackstopPressed)), 0U);
    this._leftButton = ((Node) this).GetNode<NButton>(NodePath.op_Implicit("LeftArrow"));
    ((GodotObject) this._leftButton).Connect(NClickableControl.SignalName.Released, Callable.From<NButton>((Action<NButton>) (_ => this.OnLeftButtonReleased())), 0U);
    this._rightButton = ((Node) this).GetNode<NButton>(NodePath.op_Implicit("RightArrow"));
    ((GodotObject) this._rightButton).Connect(NClickableControl.SignalName.Released, Callable.From<NButton>((Action<NButton>) (_ => this.OnRightButtonReleased())), 0U);
    this._leftButtonX = this._leftButton.Position.X;
    this._rightButtonX = this._rightButton.Position.X;
    this._upgradeTickbox = ((Node) this).GetNode<NTickbox>(NodePath.op_Implicit("%Upgrade"));
    this._upgradeTickbox.IsTicked = false;
    ((GodotObject) this._upgradeTickbox).Connect(NTickbox.SignalName.Toggled, Callable.From<NTickbox>(new Action<NTickbox>(this.ToggleShowUpgrade)), 0U);
    ((Node) this).GetNode<MegaLabel>(NodePath.op_Implicit("%ShowUpgradeLabel")).SetTextAutoSize(new LocString("card_selection", "VIEW_UPGRADES").GetFormattedText());
    this._rightButton.Disable();
    this._leftButton.Disable();
    this._upgradeTickbox.Disable();
    this.Close();
  }

  public void Open(List<CardModel> cards, int index, bool viewAllUpgraded = false)
  {
    this._cards = cards;
    ((CanvasItem) this).Visible = true;
    this.MouseFilter = (Control.MouseFilterEnum) 0L;
    this._viewAllUpgraded = viewAllUpgraded;
    this.SetCard(index);
    this._card.Scale = Vector2.op_Multiply(Vector2.One, 1.75f);
    ((CanvasItem) this._card).Modulate = StsColors.transparentBlack;
    ((CanvasItem) this._leftButton).Modulate = StsColors.transparentBlack;
    ((CanvasItem) this._rightButton).Modulate = StsColors.transparentBlack;
    this._openTween?.Kill();
    this._openTween = ((Node) this).CreateTween().SetParallel(true);
    this._openTween.TweenProperty((GodotObject) this._backstop, NodePath.op_Implicit("modulate:a"), Variant.op_Implicit(0.9f), 0.25);
    this._openTween.TweenProperty((GodotObject) this, NodePath.op_Implicit("modulate:a"), Variant.op_Implicit(1f), 0.25).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 5L).From(Variant.op_Implicit(0.0f));
    this._openTween.TweenProperty((GodotObject) this._leftButton, NodePath.op_Implicit("position:x"), Variant.op_Implicit(this._leftButtonX), 0.25).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 10L).From(Variant.op_Implicit(this._leftButtonX + 100f)).SetDelay(0.1);
    this._openTween.TweenProperty((GodotObject) this._leftButton, NodePath.op_Implicit("modulate"), Variant.op_Implicit(Colors.White), 0.25).SetDelay(0.1);
    this._openTween.TweenProperty((GodotObject) this._rightButton, NodePath.op_Implicit("position:x"), Variant.op_Implicit(this._rightButtonX), 0.25).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 10L).From(Variant.op_Implicit(this._rightButtonX - 100f)).SetDelay(0.1);
    this._openTween.TweenProperty((GodotObject) this._rightButton, NodePath.op_Implicit("modulate"), Variant.op_Implicit(Colors.White), 0.25).SetDelay(0.1);
    this._cardTween?.Kill();
    this._cardTween = ((Node) this).CreateTween().SetParallel(true);
    this._cardTween.TweenProperty((GodotObject) this._card, NodePath.op_Implicit("modulate"), Variant.op_Implicit(Colors.White), 0.25);
    this._cardTween.TweenProperty((GodotObject) this._card, NodePath.op_Implicit("scale"), Variant.op_Implicit(Vector2.op_Multiply(Vector2.One, 2f)), 0.15).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 11L).SetDelay(0.1);
    ActiveScreenContext.Instance.Update();
    NHotkeyManager.Instance.AddBlockingScreen((Node) this);
    this._rightButton.Enable();
    this._leftButton.Enable();
    this._upgradeTickbox.Enable();
    NHotkeyManager.Instance.PushHotkeyPressedBinding(StringName.op_Implicit(MegaInput.cancel), new Action(this.Close));
    NHotkeyManager.Instance.PushHotkeyPressedBinding(StringName.op_Implicit(MegaInput.pauseAndBack), new Action(this.Close));
    NHotkeyManager.Instance.PushHotkeyPressedBinding(StringName.op_Implicit(MegaInput.left), new Action(this.OnLeftButtonReleased));
    NHotkeyManager.Instance.PushHotkeyPressedBinding(StringName.op_Implicit(MegaInput.right), new Action(this.OnRightButtonReleased));
  }

  public void Close()
  {
    if (!((CanvasItem) this).Visible)
      return;
    this.MouseFilter = (Control.MouseFilterEnum) 2L;
    this._leftButton.MouseFilter = (Control.MouseFilterEnum) 2L;
    this._rightButton.MouseFilter = (Control.MouseFilterEnum) 2L;
    this._rightButton.Disable();
    this._leftButton.Disable();
    this._upgradeTickbox.Disable();
    NHoverTipSet.Clear();
    ((Node) this).SetProcessUnhandledInput(false);
    this._openTween?.Kill();
    this._openTween = ((Node) this).CreateTween().SetParallel(true);
    this._openTween.TweenProperty((GodotObject) this._backstop, NodePath.op_Implicit("modulate:a"), Variant.op_Implicit(0.0f), 0.25);
    this._openTween.TweenProperty((GodotObject) this._leftButton, NodePath.op_Implicit("modulate:a"), Variant.op_Implicit(0.0f), 0.1);
    this._openTween.TweenProperty((GodotObject) this._rightButton, NodePath.op_Implicit("modulate:a"), Variant.op_Implicit(0.0f), 0.1);
    this._openTween.TweenProperty((GodotObject) this._card, NodePath.op_Implicit("modulate"), Variant.op_Implicit(StsColors.transparentWhite), 0.1);
    this._openTween.Chain().TweenCallback(Callable.From((Action) (() =>
    {
      ((CanvasItem) this).Visible = false;
      ActiveScreenContext.Instance.Update();
    })));
    NHotkeyManager.Instance.RemoveHotkeyPressedBinding(StringName.op_Implicit(MegaInput.cancel), new Action(this.Close));
    NHotkeyManager.Instance.RemoveHotkeyPressedBinding(StringName.op_Implicit(MegaInput.pauseAndBack), new Action(this.Close));
    NHotkeyManager.Instance.RemoveHotkeyPressedBinding(StringName.op_Implicit(MegaInput.left), new Action(this.OnLeftButtonReleased));
    NHotkeyManager.Instance.RemoveHotkeyPressedBinding(StringName.op_Implicit(MegaInput.right), new Action(this.OnRightButtonReleased));
    NHotkeyManager.Instance.RemoveBlockingScreen((Node) this);
  }

  private void OnRightButtonReleased()
  {
    if (!((CanvasItem) this._rightButton).Visible)
      return;
    this.SetCard(this._index + 1);
    ((CanvasItem) this._card).Modulate = Colors.White;
    this._openTween?.Kill();
    this._openTween = ((Node) this).CreateTween().SetParallel(true);
    this._openTween.TweenProperty((GodotObject) this._card, NodePath.op_Implicit("position"), Variant.op_Implicit(this._cardPosition), 0.25).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 5L).From(Variant.op_Implicit(Vector2.op_Addition(this._cardPosition, new Vector2(100f, 0.0f))));
  }

  private void OnLeftButtonReleased()
  {
    if (!((CanvasItem) this._leftButton).Visible)
      return;
    this.SetCard(this._index - 1);
    ((CanvasItem) this._card).Modulate = Colors.White;
    this._openTween?.Kill();
    this._openTween = ((Node) this).CreateTween().SetParallel(true);
    this._openTween.TweenProperty((GodotObject) this._card, NodePath.op_Implicit("position"), Variant.op_Implicit(this._cardPosition), 0.25).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 5L).From(Variant.op_Implicit(Vector2.op_Addition(this._cardPosition, new Vector2(-100f, 0.0f))));
  }

  private void ToggleShowUpgrade(NTickbox _)
  {
    this._viewAllUpgraded = false;
    this.UpdateCardDisplay();
  }

  private void UpdateCardDisplay()
  {
    CardModel card1 = this._cards[this._index];
    CardModel card2 = (CardModel) this._cards[this._index].MutableClone();
    if (this.IsShowingUpgradedCard)
    {
      if (!card1.IsUpgraded && card1.IsUpgradable)
      {
        card2.UpgradePreviewType = CardUpgradePreviewType.Deck;
        card2.UpgradeInternal();
      }
      this._card.Model = card2;
      this._card.ShowUpgradePreview();
    }
    else
    {
      if (card2.IsUpgraded)
        CardCmd.Downgrade(card2);
      this._card.Model = card2;
      this._card.UpdateVisuals(PileType.None, CardPreviewMode.Normal);
    }
    NHoverTipSet.Clear();
    NHoverTipSet.CreateAndShow((Control) this, card2.HoverTips)?.SetAlignment(this._hoverTipRect, HoverTip.GetHoverTipAlignment((Control) this));
  }

  private void SetCard(int index)
  {
    this._index = Math.Clamp(index, 0, this._cards.Count - 1);
    ((CanvasItem) this._leftButton).Visible = this._index > 0;
    this._leftButton.MouseFilter = ((CanvasItem) this._leftButton).Visible ? (Control.MouseFilterEnum) 0L : (Control.MouseFilterEnum) 2L;
    ((CanvasItem) this._rightButton).Visible = this._index < this._cards.Count - 1;
    this._rightButton.MouseFilter = ((CanvasItem) this._rightButton).Visible ? (Control.MouseFilterEnum) 0L : (Control.MouseFilterEnum) 2L;
    ((CanvasItem) this._upgradeTickbox).Visible = this._cards[this._index].MaxUpgradeLevel > 0;
    this._upgradeTickbox.MouseFilter = this._cards[this._index].MaxUpgradeLevel > 0 ? (Control.MouseFilterEnum) 0L : (Control.MouseFilterEnum) 2L;
    this._upgradeTickbox.IsTicked = this._cards[this._index].IsUpgraded || this._viewAllUpgraded;
    this.UpdateCardDisplay();
  }

  private void OnBackstopPressed(NButton _) => this.Close();

  public Control? DefaultFocusedControl => (Control) null;

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(9)
    {
      new MethodInfo(NInspectCardScreen.MethodName.Create, new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false), (MethodFlags) 33L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NInspectCardScreen.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NInspectCardScreen.MethodName.Close, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NInspectCardScreen.MethodName.OnRightButtonReleased, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NInspectCardScreen.MethodName.OnLeftButtonReleased, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NInspectCardScreen.MethodName.ToggleShowUpgrade, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("_"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false)
      }, (List<Variant>) null),
      new MethodInfo(NInspectCardScreen.MethodName.UpdateCardDisplay, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NInspectCardScreen.MethodName.SetCard, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 2L, StringName.op_Implicit("index"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NInspectCardScreen.MethodName.OnBackstopPressed, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("_"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false)
      }, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NInspectCardScreen.MethodName.Create) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      NInspectCardScreen ninspectCardScreen = NInspectCardScreen.Create();
      ret = VariantUtils.CreateFrom<NInspectCardScreen>(ref ninspectCardScreen);
      return true;
    }
    if (StringName.op_Equality(ref method, NInspectCardScreen.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NInspectCardScreen.MethodName.Close) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.Close();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NInspectCardScreen.MethodName.OnRightButtonReleased) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnRightButtonReleased();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NInspectCardScreen.MethodName.OnLeftButtonReleased) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnLeftButtonReleased();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NInspectCardScreen.MethodName.ToggleShowUpgrade) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.ToggleShowUpgrade(VariantUtils.ConvertTo<NTickbox>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NInspectCardScreen.MethodName.UpdateCardDisplay) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.UpdateCardDisplay();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NInspectCardScreen.MethodName.SetCard) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.SetCard(VariantUtils.ConvertTo<int>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NInspectCardScreen.MethodName.OnBackstopPressed) || ((NativeVariantPtrArgs) ref args).Count != 1)
      return base.InvokeGodotClassMethod(ref method, args, ref ret);
    this.OnBackstopPressed(VariantUtils.ConvertTo<NButton>(ref ((NativeVariantPtrArgs) ref args)[0]));
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static bool InvokeGodotClassStaticMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NInspectCardScreen.MethodName.Create) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      NInspectCardScreen ninspectCardScreen = NInspectCardScreen.Create();
      ret = VariantUtils.CreateFrom<NInspectCardScreen>(ref ninspectCardScreen);
      return true;
    }
    ret = new godot_variant();
    return false;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NInspectCardScreen.MethodName.Create) || StringName.op_Equality(ref method, NInspectCardScreen.MethodName._Ready) || StringName.op_Equality(ref method, NInspectCardScreen.MethodName.Close) || StringName.op_Equality(ref method, NInspectCardScreen.MethodName.OnRightButtonReleased) || StringName.op_Equality(ref method, NInspectCardScreen.MethodName.OnLeftButtonReleased) || StringName.op_Equality(ref method, NInspectCardScreen.MethodName.ToggleShowUpgrade) || StringName.op_Equality(ref method, NInspectCardScreen.MethodName.UpdateCardDisplay) || StringName.op_Equality(ref method, NInspectCardScreen.MethodName.SetCard) || StringName.op_Equality(ref method, NInspectCardScreen.MethodName.OnBackstopPressed) || base.HasGodotClassMethod(ref method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NInspectCardScreen.PropertyName._card))
    {
      this._card = VariantUtils.ConvertTo<NCard>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NInspectCardScreen.PropertyName._backstop))
    {
      this._backstop = VariantUtils.ConvertTo<NButton>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NInspectCardScreen.PropertyName._upgradeTickbox))
    {
      this._upgradeTickbox = VariantUtils.ConvertTo<NTickbox>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NInspectCardScreen.PropertyName._leftButton))
    {
      this._leftButton = VariantUtils.ConvertTo<NButton>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NInspectCardScreen.PropertyName._rightButton))
    {
      this._rightButton = VariantUtils.ConvertTo<NButton>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NInspectCardScreen.PropertyName._hoverTipRect))
    {
      this._hoverTipRect = VariantUtils.ConvertTo<Control>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NInspectCardScreen.PropertyName._index))
    {
      this._index = VariantUtils.ConvertTo<int>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NInspectCardScreen.PropertyName._openTween))
    {
      this._openTween = VariantUtils.ConvertTo<Tween>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NInspectCardScreen.PropertyName._cardTween))
    {
      this._cardTween = VariantUtils.ConvertTo<Tween>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NInspectCardScreen.PropertyName._cardPosition))
    {
      this._cardPosition = VariantUtils.ConvertTo<Vector2>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NInspectCardScreen.PropertyName._leftButtonX))
    {
      this._leftButtonX = VariantUtils.ConvertTo<float>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NInspectCardScreen.PropertyName._rightButtonX))
    {
      this._rightButtonX = VariantUtils.ConvertTo<float>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NInspectCardScreen.PropertyName._viewAllUpgraded))
      return ((GodotObject) this).SetGodotClassPropertyValue(ref name, ref value);
    this._viewAllUpgraded = VariantUtils.ConvertTo<bool>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NInspectCardScreen.PropertyName.IsShowingUpgradedCard))
    {
      ref godot_variant local = ref value;
      bool showingUpgradedCard = this.IsShowingUpgradedCard;
      godot_variant from = VariantUtils.CreateFrom<bool>(ref showingUpgradedCard);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NInspectCardScreen.PropertyName.DefaultFocusedControl))
    {
      ref godot_variant local = ref value;
      Control defaultFocusedControl = this.DefaultFocusedControl;
      godot_variant from = VariantUtils.CreateFrom<Control>(ref defaultFocusedControl);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NInspectCardScreen.PropertyName._card))
    {
      value = VariantUtils.CreateFrom<NCard>(ref this._card);
      return true;
    }
    if (StringName.op_Equality(ref name, NInspectCardScreen.PropertyName._backstop))
    {
      value = VariantUtils.CreateFrom<NButton>(ref this._backstop);
      return true;
    }
    if (StringName.op_Equality(ref name, NInspectCardScreen.PropertyName._upgradeTickbox))
    {
      value = VariantUtils.CreateFrom<NTickbox>(ref this._upgradeTickbox);
      return true;
    }
    if (StringName.op_Equality(ref name, NInspectCardScreen.PropertyName._leftButton))
    {
      value = VariantUtils.CreateFrom<NButton>(ref this._leftButton);
      return true;
    }
    if (StringName.op_Equality(ref name, NInspectCardScreen.PropertyName._rightButton))
    {
      value = VariantUtils.CreateFrom<NButton>(ref this._rightButton);
      return true;
    }
    if (StringName.op_Equality(ref name, NInspectCardScreen.PropertyName._hoverTipRect))
    {
      value = VariantUtils.CreateFrom<Control>(ref this._hoverTipRect);
      return true;
    }
    if (StringName.op_Equality(ref name, NInspectCardScreen.PropertyName._index))
    {
      value = VariantUtils.CreateFrom<int>(ref this._index);
      return true;
    }
    if (StringName.op_Equality(ref name, NInspectCardScreen.PropertyName._openTween))
    {
      value = VariantUtils.CreateFrom<Tween>(ref this._openTween);
      return true;
    }
    if (StringName.op_Equality(ref name, NInspectCardScreen.PropertyName._cardTween))
    {
      value = VariantUtils.CreateFrom<Tween>(ref this._cardTween);
      return true;
    }
    if (StringName.op_Equality(ref name, NInspectCardScreen.PropertyName._cardPosition))
    {
      value = VariantUtils.CreateFrom<Vector2>(ref this._cardPosition);
      return true;
    }
    if (StringName.op_Equality(ref name, NInspectCardScreen.PropertyName._leftButtonX))
    {
      value = VariantUtils.CreateFrom<float>(ref this._leftButtonX);
      return true;
    }
    if (StringName.op_Equality(ref name, NInspectCardScreen.PropertyName._rightButtonX))
    {
      value = VariantUtils.CreateFrom<float>(ref this._rightButtonX);
      return true;
    }
    if (!StringName.op_Equality(ref name, NInspectCardScreen.PropertyName._viewAllUpgraded))
      return ((GodotObject) this).GetGodotClassPropertyValue(ref name, ref value);
    value = VariantUtils.CreateFrom<bool>(ref this._viewAllUpgraded);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NInspectCardScreen.PropertyName._card, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NInspectCardScreen.PropertyName._backstop, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NInspectCardScreen.PropertyName._upgradeTickbox, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NInspectCardScreen.PropertyName._leftButton, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NInspectCardScreen.PropertyName._rightButton, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NInspectCardScreen.PropertyName._hoverTipRect, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 2L, NInspectCardScreen.PropertyName._index, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NInspectCardScreen.PropertyName._openTween, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NInspectCardScreen.PropertyName._cardTween, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 5L, NInspectCardScreen.PropertyName._cardPosition, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 3L, NInspectCardScreen.PropertyName._leftButtonX, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 3L, NInspectCardScreen.PropertyName._rightButtonX, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 1L, NInspectCardScreen.PropertyName._viewAllUpgraded, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 1L, NInspectCardScreen.PropertyName.IsShowingUpgradedCard, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NInspectCardScreen.PropertyName.DefaultFocusedControl, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).SaveGodotObjectData(info);
    info.AddProperty(NInspectCardScreen.PropertyName._card, Variant.From<NCard>(ref this._card));
    info.AddProperty(NInspectCardScreen.PropertyName._backstop, Variant.From<NButton>(ref this._backstop));
    info.AddProperty(NInspectCardScreen.PropertyName._upgradeTickbox, Variant.From<NTickbox>(ref this._upgradeTickbox));
    info.AddProperty(NInspectCardScreen.PropertyName._leftButton, Variant.From<NButton>(ref this._leftButton));
    info.AddProperty(NInspectCardScreen.PropertyName._rightButton, Variant.From<NButton>(ref this._rightButton));
    info.AddProperty(NInspectCardScreen.PropertyName._hoverTipRect, Variant.From<Control>(ref this._hoverTipRect));
    info.AddProperty(NInspectCardScreen.PropertyName._index, Variant.From<int>(ref this._index));
    info.AddProperty(NInspectCardScreen.PropertyName._openTween, Variant.From<Tween>(ref this._openTween));
    info.AddProperty(NInspectCardScreen.PropertyName._cardTween, Variant.From<Tween>(ref this._cardTween));
    info.AddProperty(NInspectCardScreen.PropertyName._cardPosition, Variant.From<Vector2>(ref this._cardPosition));
    info.AddProperty(NInspectCardScreen.PropertyName._leftButtonX, Variant.From<float>(ref this._leftButtonX));
    info.AddProperty(NInspectCardScreen.PropertyName._rightButtonX, Variant.From<float>(ref this._rightButtonX));
    info.AddProperty(NInspectCardScreen.PropertyName._viewAllUpgraded, Variant.From<bool>(ref this._viewAllUpgraded));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NInspectCardScreen.PropertyName._card, ref variant1))
      this._card = ((Variant) ref variant1).As<NCard>();
    Variant variant2;
    if (info.TryGetProperty(NInspectCardScreen.PropertyName._backstop, ref variant2))
      this._backstop = ((Variant) ref variant2).As<NButton>();
    Variant variant3;
    if (info.TryGetProperty(NInspectCardScreen.PropertyName._upgradeTickbox, ref variant3))
      this._upgradeTickbox = ((Variant) ref variant3).As<NTickbox>();
    Variant variant4;
    if (info.TryGetProperty(NInspectCardScreen.PropertyName._leftButton, ref variant4))
      this._leftButton = ((Variant) ref variant4).As<NButton>();
    Variant variant5;
    if (info.TryGetProperty(NInspectCardScreen.PropertyName._rightButton, ref variant5))
      this._rightButton = ((Variant) ref variant5).As<NButton>();
    Variant variant6;
    if (info.TryGetProperty(NInspectCardScreen.PropertyName._hoverTipRect, ref variant6))
      this._hoverTipRect = ((Variant) ref variant6).As<Control>();
    Variant variant7;
    if (info.TryGetProperty(NInspectCardScreen.PropertyName._index, ref variant7))
      this._index = ((Variant) ref variant7).As<int>();
    Variant variant8;
    if (info.TryGetProperty(NInspectCardScreen.PropertyName._openTween, ref variant8))
      this._openTween = ((Variant) ref variant8).As<Tween>();
    Variant variant9;
    if (info.TryGetProperty(NInspectCardScreen.PropertyName._cardTween, ref variant9))
      this._cardTween = ((Variant) ref variant9).As<Tween>();
    Variant variant10;
    if (info.TryGetProperty(NInspectCardScreen.PropertyName._cardPosition, ref variant10))
      this._cardPosition = ((Variant) ref variant10).As<Vector2>();
    Variant variant11;
    if (info.TryGetProperty(NInspectCardScreen.PropertyName._leftButtonX, ref variant11))
      this._leftButtonX = ((Variant) ref variant11).As<float>();
    Variant variant12;
    if (info.TryGetProperty(NInspectCardScreen.PropertyName._rightButtonX, ref variant12))
      this._rightButtonX = ((Variant) ref variant12).As<float>();
    Variant variant13;
    if (!info.TryGetProperty(NInspectCardScreen.PropertyName._viewAllUpgraded, ref variant13))
      return;
    this._viewAllUpgraded = ((Variant) ref variant13).As<bool>();
  }

  public class MethodName : Control.MethodName
  {
    public static readonly StringName Create = StringName.op_Implicit(nameof (Create));
    public static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName Close = StringName.op_Implicit(nameof (Close));
    public static readonly StringName OnRightButtonReleased = StringName.op_Implicit(nameof (OnRightButtonReleased));
    public static readonly StringName OnLeftButtonReleased = StringName.op_Implicit(nameof (OnLeftButtonReleased));
    public static readonly StringName ToggleShowUpgrade = StringName.op_Implicit(nameof (ToggleShowUpgrade));
    public static readonly StringName UpdateCardDisplay = StringName.op_Implicit(nameof (UpdateCardDisplay));
    public static readonly StringName SetCard = StringName.op_Implicit(nameof (SetCard));
    public static readonly StringName OnBackstopPressed = StringName.op_Implicit(nameof (OnBackstopPressed));
  }

  public class PropertyName : Control.PropertyName
  {
    public static readonly StringName IsShowingUpgradedCard = StringName.op_Implicit(nameof (IsShowingUpgradedCard));
    public static readonly StringName DefaultFocusedControl = StringName.op_Implicit(nameof (DefaultFocusedControl));
    public static readonly StringName _card = StringName.op_Implicit(nameof (_card));
    public static readonly StringName _backstop = StringName.op_Implicit(nameof (_backstop));
    public static readonly StringName _upgradeTickbox = StringName.op_Implicit(nameof (_upgradeTickbox));
    public static readonly StringName _leftButton = StringName.op_Implicit(nameof (_leftButton));
    public static readonly StringName _rightButton = StringName.op_Implicit(nameof (_rightButton));
    public static readonly StringName _hoverTipRect = StringName.op_Implicit(nameof (_hoverTipRect));
    public static readonly StringName _index = StringName.op_Implicit(nameof (_index));
    public static readonly StringName _openTween = StringName.op_Implicit(nameof (_openTween));
    public static readonly StringName _cardTween = StringName.op_Implicit(nameof (_cardTween));
    public static readonly StringName _cardPosition = StringName.op_Implicit(nameof (_cardPosition));
    public static readonly StringName _leftButtonX = StringName.op_Implicit(nameof (_leftButtonX));
    public static readonly StringName _rightButtonX = StringName.op_Implicit(nameof (_rightButtonX));
    public static readonly StringName _viewAllUpgraded = StringName.op_Implicit(nameof (_viewAllUpgraded));
  }

  public class SignalName : Control.SignalName
  {
  }
}
