// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.CommonUi.NTopBar
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.Debug;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.Potions;
using MegaCrit.Sts2.Core.Nodes.Relics;
using MegaCrit.Sts2.Core.Nodes.Screens.Capstones;
using MegaCrit.Sts2.Core.Nodes.Screens.ScreenContext;
using MegaCrit.sts2.Core.Nodes.TopBar;
using MegaCrit.Sts2.Core.Nodes.TopBar;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.Runs;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.CommonUi;

[ScriptPath("res://src/Core/Nodes/CommonUi/NTopBar.cs")]
public class NTopBar : Control
{
  private NCapstoneContainer _capstoneContainer;
  private static readonly StringName _fontOutlineTheme = StringName.op_Implicit("font_outline_color");
  private static readonly StringName _h = new StringName("h");
  private static readonly StringName _v = new StringName("v");
  private static readonly Color _redLabelOutline = new Color("593400");
  private static readonly Color _blueLabelOutline = new Color("004759");
  private Control _modifiersContainer;
  private Control _achievementLock;
  private Control _ascensionIcon;
  private MegaLabel _ascensionLabel;
  private ShaderMaterial _ascensionHsv;
  private Tween? _hideTween;
  private bool _isDebugHidden;
  private Player? _player;

  public NTopBarMapButton Map { get; private set; }

  public NTopBarDeckButton Deck { get; private set; }

  public NTopBarPauseButton Pause { get; private set; }

  public NPotionContainer PotionContainer { get; private set; }

  public NTopBarRoomIcon RoomIcon { get; private set; }

  public NTopBarFloorIcon FloorIcon { get; private set; }

  public NTopBarBossIcon BossIcon { get; private set; }

  public NTopBarGold Gold { get; private set; }

  public NTopBarHp Hp { get; private set; }

  public NTopBarPortrait Portrait { get; private set; }

  public NTopBarPortraitTip PortraitTip { get; private set; }

  public NRunTimer Timer { get; private set; }

  public Node TrailContainer { get; private set; }

  public Control ActiveScreenProxy { get; private set; }

  public override void _Ready()
  {
    this.TrailContainer = ((Node) this).GetNode<Node>(NodePath.op_Implicit("%TrailContainer"));
    this.Map = ((Node) this).GetNode<NTopBarMapButton>(NodePath.op_Implicit("%Map"));
    this.Deck = ((Node) this).GetNode<NTopBarDeckButton>(NodePath.op_Implicit("%Deck"));
    this.Pause = ((Node) this).GetNode<NTopBarPauseButton>(NodePath.op_Implicit("%PauseButton"));
    this.PotionContainer = ((Node) this).GetNode<NPotionContainer>(NodePath.op_Implicit("%PotionContainer"));
    this.RoomIcon = ((Node) this).GetNode<NTopBarRoomIcon>(NodePath.op_Implicit("%RoomIcon"));
    this.FloorIcon = ((Node) this).GetNode<NTopBarFloorIcon>(NodePath.op_Implicit("%FloorIcon"));
    this.BossIcon = ((Node) this).GetNode<NTopBarBossIcon>(NodePath.op_Implicit("%BossIcon"));
    this.Gold = ((Node) this).GetNode<NTopBarGold>(NodePath.op_Implicit("%TopBarGold"));
    this.Hp = ((Node) this).GetNode<NTopBarHp>(NodePath.op_Implicit("%TopBarHp"));
    this.Portrait = ((Node) this).GetNode<NTopBarPortrait>(NodePath.op_Implicit("%TopBarPortrait"));
    this.PortraitTip = ((Node) this).GetNode<NTopBarPortraitTip>(NodePath.op_Implicit("%TopBarPortraitTip"));
    this.Timer = ((Node) this).GetNode<NRunTimer>(NodePath.op_Implicit("%TimerContainer"));
    this.ActiveScreenProxy = ((Node) this).GetNode<Control>(NodePath.op_Implicit("%ActiveScreenProxy"));
    this._achievementLock = ((Node) this).GetNode<Control>(NodePath.op_Implicit("%AchievementLock"));
    this._ascensionIcon = ((Node) this).GetNode<Control>(NodePath.op_Implicit("%AscensionIcon"));
    this._ascensionLabel = ((Node) this).GetNode<MegaLabel>(NodePath.op_Implicit("%AscensionLabel"));
    this._ascensionHsv = (ShaderMaterial) ((CanvasItem) this._ascensionIcon).Material;
    this._modifiersContainer = ((Node) this).GetNode<Control>(NodePath.op_Implicit("%Modifiers"));
    ((GodotObject) this.ActiveScreenProxy).Connect(Control.SignalName.FocusEntered, Callable.From((Action) (() =>
    {
      IScreenContext currentScreen = ActiveScreenContext.Instance.GetCurrentScreen();
      if (currentScreen == null)
        return;
      Control controlFromTopBar = currentScreen.FocusedControlFromTopBar;
      if (controlFromTopBar == null)
        return;
      controlFromTopBar.TryGrabFocus();
    })), 0U);
    this._capstoneContainer = ((Node) this).GetParent().GetNode<NCapstoneContainer>(NodePath.op_Implicit("%CapstoneScreenContainer"));
    ((GodotObject) this._capstoneContainer).Connect(Node.SignalName.ChildEnteredTree, Callable.From<Node>(new Action<Node>(this.ToggleAnimState)), 0U);
    ((GodotObject) this._capstoneContainer).Connect(Node.SignalName.ChildExitingTree, Callable.From<Node>(new Action<Node>(this.ToggleAnimState)), 0U);
  }

  public void Initialize(IRunState runState)
  {
    if (runState.AscensionLevel > 0)
    {
      if (runState.Players.Count > 1)
      {
        this._ascensionHsv.SetShaderParameter(NTopBar._h, Variant.op_Implicit(0.52f));
        this._ascensionHsv.SetShaderParameter(NTopBar._v, Variant.op_Implicit(1.2f));
        ((Control) this._ascensionLabel).AddThemeColorOverride(NTopBar._fontOutlineTheme, NTopBar._blueLabelOutline);
      }
      else
      {
        this._ascensionHsv.SetShaderParameter(NTopBar._h, Variant.op_Implicit(1f));
        this._ascensionHsv.SetShaderParameter(NTopBar._v, Variant.op_Implicit(1f));
        ((Control) this._ascensionLabel).AddThemeColorOverride(NTopBar._fontOutlineTheme, NTopBar._redLabelOutline);
      }
      ((CanvasItem) this._ascensionIcon).Visible = true;
      this._ascensionLabel.SetTextAutoSize(runState.AscensionLevel.ToString());
    }
    ((CanvasItem) this._achievementLock).Visible = runState.GameMode.AreAchievementsAndEpochsLocked();
    ((CanvasItem) this._modifiersContainer).Visible = runState.Modifiers.Count > 0;
    foreach (ModifierModel modifier in (IEnumerable<ModifierModel>) runState.Modifiers)
      ((Node) this._modifiersContainer).AddChildSafely((Node) NTopBarModifier.Create(modifier));
    this._player = LocalContext.GetMe((IPlayerCollection) runState);
    this.Deck.Initialize(this._player);
    this.RoomIcon.Initialize(runState);
    this.FloorIcon.Initialize(runState);
    this.BossIcon.Initialize(runState);
    this.Gold.Initialize(this._player);
    this.Hp.Initialize(this._player);
    this.Pause.Initialize(runState);
    this.Portrait.Initialize(this._player);
    this.PortraitTip.Initialize(runState);
    this.PotionContainer.Initialize(runState);
    this._player.RelicObtained += new Action<RelicModel>(this.OnRelicsUpdated);
    this._player.RelicRemoved += new Action<RelicModel>(this.OnRelicsUpdated);
    this._player.MaxPotionCountChanged += new Action<int>(this.MaxPotionsChanged);
    Callable callable = Callable.From(new Action(this.UpdateNavigation));
    ((Callable) ref callable).CallDeferred(Array.Empty<Variant>());
  }

  public override void _EnterTree()
  {
    ((Node) this)._EnterTree();
    ActiveScreenContext.Instance.Updated += new Action(this.UpdateNavigation);
  }

  public override void _ExitTree()
  {
    ActiveScreenContext.Instance.Updated -= new Action(this.UpdateNavigation);
    if (this._player == null)
      return;
    this._player.RelicObtained -= new Action<RelicModel>(this.OnRelicsUpdated);
    this._player.RelicRemoved -= new Action<RelicModel>(this.OnRelicsUpdated);
    this._player.MaxPotionCountChanged -= new Action<int>(this.MaxPotionsChanged);
  }

  private void ToggleAnimState(Node _)
  {
    this.Pause.ToggleAnimState();
    this.Deck.ToggleAnimState();
  }

  public override void _Input(InputEvent inputEvent)
  {
    if (!inputEvent.IsActionReleased(DebugHotkey.hideTopBar, false))
      return;
    this.DebugHideTopBar();
  }

  private void DebugHideTopBar()
  {
    if (!this._isDebugHidden)
    {
      ((Node) NGame.Instance).AddChildSafely((Node) NFullscreenTextVfx.Create("Hide Top Bar"));
      this.AnimHide();
    }
    else
    {
      ((Node) NGame.Instance).AddChildSafely((Node) NFullscreenTextVfx.Create("Show Top Bar"));
      this.AnimShow();
    }
    this._isDebugHidden = !this._isDebugHidden;
  }

  public void AnimHide()
  {
    this.FocusBehaviorRecursive = (Control.FocusBehaviorRecursiveEnum) 1L;
    this._hideTween?.Kill();
    this._hideTween = ((Node) this).CreateTween();
    this._hideTween.TweenProperty((GodotObject) this, NodePath.op_Implicit("position:y"), Variant.op_Implicit(-100f), 0.25).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 7L);
  }

  public void AnimShow()
  {
    this.FocusBehaviorRecursive = (Control.FocusBehaviorRecursiveEnum) 2L;
    this._hideTween?.Kill();
    this._hideTween = ((Node) this).CreateTween();
    this._hideTween.TweenProperty((GodotObject) this, NodePath.op_Implicit("position:y"), Variant.op_Implicit(0.0f), 0.25).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 7L);
  }

  private void OnRelicsUpdated(RelicModel _)
  {
    Callable callable = Callable.From(new Action(this.UpdateNavigation));
    ((Callable) ref callable).CallDeferred(Array.Empty<Variant>());
  }

  private void MaxPotionsChanged(int _)
  {
    Callable callable = Callable.From(new Action(this.UpdateNavigation));
    ((Callable) ref callable).CallDeferred(Array.Empty<Variant>());
  }

  private void UpdateNavigation()
  {
    Control control1 = (Control) NRun.Instance.GlobalUi.RelicInventory.RelicNodes.FirstOrDefault<NRelicInventoryHolder>();
    if (control1 == null)
      return;
    this.Gold.FocusNeighborBottom = ((Node) control1).GetPath();
    this.Hp.FocusNeighborBottom = ((Node) control1).GetPath();
    this.FloorIcon.FocusNeighborBottom = ((Node) control1).GetPath();
    this.RoomIcon.FocusNeighborBottom = ((Node) control1).GetPath();
    this.BossIcon.FocusNeighborBottom = ((Node) control1).GetPath();
    this.Gold.FocusNeighborTop = ((Node) this.Gold).GetPath();
    this.Hp.FocusNeighborTop = ((Node) this.Hp).GetPath();
    this.FloorIcon.FocusNeighborTop = ((Node) this.FloorIcon).GetPath();
    this.RoomIcon.FocusNeighborTop = ((Node) this.RoomIcon).GetPath();
    this.BossIcon.FocusNeighborTop = ((Node) this.BossIcon).GetPath();
    this.Hp.FocusNeighborLeft = ((Node) this.Hp).GetPath();
    this.Hp.FocusNeighborRight = ((Node) this.Gold).GetPath();
    this.Gold.FocusNeighborLeft = ((Node) this.Hp).GetPath();
    this.Gold.FocusNeighborRight = ((Node) this.PotionContainer.FirstPotionControl)?.GetPath();
    this.RoomIcon.FocusNeighborLeft = ((Node) this.PotionContainer.LastPotionControl)?.GetPath();
    this.RoomIcon.FocusNeighborRight = ((Node) this.FloorIcon).GetPath();
    this.FloorIcon.FocusNeighborLeft = ((Node) this.RoomIcon).GetPath();
    this.FloorIcon.FocusNeighborRight = ((Node) this.BossIcon).GetPath();
    this.BossIcon.FocusNeighborLeft = ((Node) this.FloorIcon).GetPath();
    this.BossIcon.FocusNeighborRight = ((Node) this.BossIcon).GetPath();
    if (this.PortraitTip.ShowTip)
    {
      this.PortraitTip.FocusNeighborRight = ((Node) this.Hp).GetPath();
      this.PortraitTip.FocusNeighborLeft = ((Node) this.PortraitTip).GetPath();
      this.PortraitTip.FocusNeighborTop = ((Node) this.PortraitTip).GetPath();
      this.PortraitTip.FocusNeighborBottom = ((Node) this.PortraitTip).GetPath();
      this.Hp.FocusNeighborLeft = ((Node) this.PortraitTip).GetPath();
    }
    Viewport viewport = ((Node) this).GetViewport();
    if (viewport != null && viewport.GuiGetFocusOwner() == this.ActiveScreenProxy)
      ActiveScreenContext.Instance.FocusOnDefaultControl();
    if (!((CanvasItem) this._modifiersContainer).Visible)
      return;
    Control[] array = ((IEnumerable) ((Node) this._modifiersContainer).GetChildren(false)).OfType<Control>().ToArray<Control>();
    this.BossIcon.FocusNeighborRight = ((Node) ((IEnumerable<Control>) array).First<Control>()).GetPath();
    if (!((CanvasItem) this.BossIcon).IsVisible())
      this.FloorIcon.FocusNeighborRight = ((Node) ((IEnumerable<Control>) array).First<Control>()).GetPath();
    for (int index = 0; index < array.Length; ++index)
    {
      Control control2 = array[index];
      control2.FocusNeighborTop = ((Node) control2).GetPath();
      control2.FocusNeighborBottom = ((Node) control1).GetPath();
      control2.FocusNeighborRight = index < array.Length - 1 ? ((Node) array[index + 1]).GetPath() : ((Node) control2).GetPath();
      control2.FocusNeighborLeft = index <= 0 ? (((CanvasItem) this.BossIcon).IsVisible() ? ((Node) this.BossIcon).GetPath() : ((Node) this.FloorIcon).GetPath()) : ((Node) array[index - 1]).GetPath();
    }
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(10)
    {
      new MethodInfo(NTopBar.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NTopBar.MethodName._EnterTree, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NTopBar.MethodName._ExitTree, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NTopBar.MethodName.ToggleAnimState, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("_"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Node"), false)
      }, (List<Variant>) null),
      new MethodInfo(NTopBar.MethodName._Input, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("inputEvent"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("InputEvent"), false)
      }, (List<Variant>) null),
      new MethodInfo(NTopBar.MethodName.DebugHideTopBar, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NTopBar.MethodName.AnimHide, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NTopBar.MethodName.AnimShow, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NTopBar.MethodName.MaxPotionsChanged, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 2L, StringName.op_Implicit("_"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NTopBar.MethodName.UpdateNavigation, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NTopBar.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NTopBar.MethodName._EnterTree) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._EnterTree();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NTopBar.MethodName._ExitTree) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._ExitTree();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NTopBar.MethodName.ToggleAnimState) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.ToggleAnimState(VariantUtils.ConvertTo<Node>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NTopBar.MethodName._Input) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      ((Node) this)._Input(VariantUtils.ConvertTo<InputEvent>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NTopBar.MethodName.DebugHideTopBar) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.DebugHideTopBar();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NTopBar.MethodName.AnimHide) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.AnimHide();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NTopBar.MethodName.AnimShow) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.AnimShow();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NTopBar.MethodName.MaxPotionsChanged) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.MaxPotionsChanged(VariantUtils.ConvertTo<int>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NTopBar.MethodName.UpdateNavigation) || ((NativeVariantPtrArgs) ref args).Count != 0)
      return base.InvokeGodotClassMethod(ref method, args, ref ret);
    this.UpdateNavigation();
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NTopBar.MethodName._Ready) || StringName.op_Equality(ref method, NTopBar.MethodName._EnterTree) || StringName.op_Equality(ref method, NTopBar.MethodName._ExitTree) || StringName.op_Equality(ref method, NTopBar.MethodName.ToggleAnimState) || StringName.op_Equality(ref method, NTopBar.MethodName._Input) || StringName.op_Equality(ref method, NTopBar.MethodName.DebugHideTopBar) || StringName.op_Equality(ref method, NTopBar.MethodName.AnimHide) || StringName.op_Equality(ref method, NTopBar.MethodName.AnimShow) || StringName.op_Equality(ref method, NTopBar.MethodName.MaxPotionsChanged) || StringName.op_Equality(ref method, NTopBar.MethodName.UpdateNavigation) || base.HasGodotClassMethod(ref method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NTopBar.PropertyName.Map))
    {
      this.Map = VariantUtils.ConvertTo<NTopBarMapButton>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NTopBar.PropertyName.Deck))
    {
      this.Deck = VariantUtils.ConvertTo<NTopBarDeckButton>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NTopBar.PropertyName.Pause))
    {
      this.Pause = VariantUtils.ConvertTo<NTopBarPauseButton>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NTopBar.PropertyName.PotionContainer))
    {
      this.PotionContainer = VariantUtils.ConvertTo<NPotionContainer>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NTopBar.PropertyName.RoomIcon))
    {
      this.RoomIcon = VariantUtils.ConvertTo<NTopBarRoomIcon>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NTopBar.PropertyName.FloorIcon))
    {
      this.FloorIcon = VariantUtils.ConvertTo<NTopBarFloorIcon>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NTopBar.PropertyName.BossIcon))
    {
      this.BossIcon = VariantUtils.ConvertTo<NTopBarBossIcon>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NTopBar.PropertyName.Gold))
    {
      this.Gold = VariantUtils.ConvertTo<NTopBarGold>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NTopBar.PropertyName.Hp))
    {
      this.Hp = VariantUtils.ConvertTo<NTopBarHp>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NTopBar.PropertyName.Portrait))
    {
      this.Portrait = VariantUtils.ConvertTo<NTopBarPortrait>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NTopBar.PropertyName.PortraitTip))
    {
      this.PortraitTip = VariantUtils.ConvertTo<NTopBarPortraitTip>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NTopBar.PropertyName.Timer))
    {
      this.Timer = VariantUtils.ConvertTo<NRunTimer>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NTopBar.PropertyName.TrailContainer))
    {
      this.TrailContainer = VariantUtils.ConvertTo<Node>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NTopBar.PropertyName.ActiveScreenProxy))
    {
      this.ActiveScreenProxy = VariantUtils.ConvertTo<Control>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NTopBar.PropertyName._capstoneContainer))
    {
      this._capstoneContainer = VariantUtils.ConvertTo<NCapstoneContainer>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NTopBar.PropertyName._modifiersContainer))
    {
      this._modifiersContainer = VariantUtils.ConvertTo<Control>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NTopBar.PropertyName._achievementLock))
    {
      this._achievementLock = VariantUtils.ConvertTo<Control>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NTopBar.PropertyName._ascensionIcon))
    {
      this._ascensionIcon = VariantUtils.ConvertTo<Control>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NTopBar.PropertyName._ascensionLabel))
    {
      this._ascensionLabel = VariantUtils.ConvertTo<MegaLabel>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NTopBar.PropertyName._ascensionHsv))
    {
      this._ascensionHsv = VariantUtils.ConvertTo<ShaderMaterial>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NTopBar.PropertyName._hideTween))
    {
      this._hideTween = VariantUtils.ConvertTo<Tween>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NTopBar.PropertyName._isDebugHidden))
      return ((GodotObject) this).SetGodotClassPropertyValue(ref name, ref value);
    this._isDebugHidden = VariantUtils.ConvertTo<bool>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NTopBar.PropertyName.Map))
    {
      ref godot_variant local = ref value;
      NTopBarMapButton map = this.Map;
      godot_variant from = VariantUtils.CreateFrom<NTopBarMapButton>(ref map);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NTopBar.PropertyName.Deck))
    {
      ref godot_variant local = ref value;
      NTopBarDeckButton deck = this.Deck;
      godot_variant from = VariantUtils.CreateFrom<NTopBarDeckButton>(ref deck);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NTopBar.PropertyName.Pause))
    {
      ref godot_variant local = ref value;
      NTopBarPauseButton pause = this.Pause;
      godot_variant from = VariantUtils.CreateFrom<NTopBarPauseButton>(ref pause);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NTopBar.PropertyName.PotionContainer))
    {
      ref godot_variant local = ref value;
      NPotionContainer potionContainer = this.PotionContainer;
      godot_variant from = VariantUtils.CreateFrom<NPotionContainer>(ref potionContainer);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NTopBar.PropertyName.RoomIcon))
    {
      ref godot_variant local = ref value;
      NTopBarRoomIcon roomIcon = this.RoomIcon;
      godot_variant from = VariantUtils.CreateFrom<NTopBarRoomIcon>(ref roomIcon);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NTopBar.PropertyName.FloorIcon))
    {
      ref godot_variant local = ref value;
      NTopBarFloorIcon floorIcon = this.FloorIcon;
      godot_variant from = VariantUtils.CreateFrom<NTopBarFloorIcon>(ref floorIcon);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NTopBar.PropertyName.BossIcon))
    {
      ref godot_variant local = ref value;
      NTopBarBossIcon bossIcon = this.BossIcon;
      godot_variant from = VariantUtils.CreateFrom<NTopBarBossIcon>(ref bossIcon);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NTopBar.PropertyName.Gold))
    {
      ref godot_variant local = ref value;
      NTopBarGold gold = this.Gold;
      godot_variant from = VariantUtils.CreateFrom<NTopBarGold>(ref gold);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NTopBar.PropertyName.Hp))
    {
      ref godot_variant local = ref value;
      NTopBarHp hp = this.Hp;
      godot_variant from = VariantUtils.CreateFrom<NTopBarHp>(ref hp);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NTopBar.PropertyName.Portrait))
    {
      ref godot_variant local = ref value;
      NTopBarPortrait portrait = this.Portrait;
      godot_variant from = VariantUtils.CreateFrom<NTopBarPortrait>(ref portrait);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NTopBar.PropertyName.PortraitTip))
    {
      ref godot_variant local = ref value;
      NTopBarPortraitTip portraitTip = this.PortraitTip;
      godot_variant from = VariantUtils.CreateFrom<NTopBarPortraitTip>(ref portraitTip);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NTopBar.PropertyName.Timer))
    {
      ref godot_variant local = ref value;
      NRunTimer timer = this.Timer;
      godot_variant from = VariantUtils.CreateFrom<NRunTimer>(ref timer);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NTopBar.PropertyName.TrailContainer))
    {
      ref godot_variant local = ref value;
      Node trailContainer = this.TrailContainer;
      godot_variant from = VariantUtils.CreateFrom<Node>(ref trailContainer);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NTopBar.PropertyName.ActiveScreenProxy))
    {
      ref godot_variant local = ref value;
      Control activeScreenProxy = this.ActiveScreenProxy;
      godot_variant from = VariantUtils.CreateFrom<Control>(ref activeScreenProxy);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NTopBar.PropertyName._capstoneContainer))
    {
      value = VariantUtils.CreateFrom<NCapstoneContainer>(ref this._capstoneContainer);
      return true;
    }
    if (StringName.op_Equality(ref name, NTopBar.PropertyName._modifiersContainer))
    {
      value = VariantUtils.CreateFrom<Control>(ref this._modifiersContainer);
      return true;
    }
    if (StringName.op_Equality(ref name, NTopBar.PropertyName._achievementLock))
    {
      value = VariantUtils.CreateFrom<Control>(ref this._achievementLock);
      return true;
    }
    if (StringName.op_Equality(ref name, NTopBar.PropertyName._ascensionIcon))
    {
      value = VariantUtils.CreateFrom<Control>(ref this._ascensionIcon);
      return true;
    }
    if (StringName.op_Equality(ref name, NTopBar.PropertyName._ascensionLabel))
    {
      value = VariantUtils.CreateFrom<MegaLabel>(ref this._ascensionLabel);
      return true;
    }
    if (StringName.op_Equality(ref name, NTopBar.PropertyName._ascensionHsv))
    {
      value = VariantUtils.CreateFrom<ShaderMaterial>(ref this._ascensionHsv);
      return true;
    }
    if (StringName.op_Equality(ref name, NTopBar.PropertyName._hideTween))
    {
      value = VariantUtils.CreateFrom<Tween>(ref this._hideTween);
      return true;
    }
    if (!StringName.op_Equality(ref name, NTopBar.PropertyName._isDebugHidden))
      return ((GodotObject) this).GetGodotClassPropertyValue(ref name, ref value);
    value = VariantUtils.CreateFrom<bool>(ref this._isDebugHidden);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NTopBar.PropertyName._capstoneContainer, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NTopBar.PropertyName.Map, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NTopBar.PropertyName.Deck, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NTopBar.PropertyName.Pause, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NTopBar.PropertyName.PotionContainer, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NTopBar.PropertyName.RoomIcon, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NTopBar.PropertyName.FloorIcon, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NTopBar.PropertyName.BossIcon, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NTopBar.PropertyName.Gold, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NTopBar.PropertyName.Hp, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NTopBar.PropertyName.Portrait, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NTopBar.PropertyName.PortraitTip, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NTopBar.PropertyName.Timer, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NTopBar.PropertyName.TrailContainer, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NTopBar.PropertyName.ActiveScreenProxy, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NTopBar.PropertyName._modifiersContainer, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NTopBar.PropertyName._achievementLock, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NTopBar.PropertyName._ascensionIcon, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NTopBar.PropertyName._ascensionLabel, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NTopBar.PropertyName._ascensionHsv, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NTopBar.PropertyName._hideTween, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 1L, NTopBar.PropertyName._isDebugHidden, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).SaveGodotObjectData(info);
    GodotSerializationInfo serializationInfo1 = info;
    StringName map1 = NTopBar.PropertyName.Map;
    NTopBarMapButton map2 = this.Map;
    Variant variant1 = Variant.From<NTopBarMapButton>(ref map2);
    serializationInfo1.AddProperty(map1, variant1);
    GodotSerializationInfo serializationInfo2 = info;
    StringName deck1 = NTopBar.PropertyName.Deck;
    NTopBarDeckButton deck2 = this.Deck;
    Variant variant2 = Variant.From<NTopBarDeckButton>(ref deck2);
    serializationInfo2.AddProperty(deck1, variant2);
    GodotSerializationInfo serializationInfo3 = info;
    StringName pause1 = NTopBar.PropertyName.Pause;
    NTopBarPauseButton pause2 = this.Pause;
    Variant variant3 = Variant.From<NTopBarPauseButton>(ref pause2);
    serializationInfo3.AddProperty(pause1, variant3);
    GodotSerializationInfo serializationInfo4 = info;
    StringName potionContainer1 = NTopBar.PropertyName.PotionContainer;
    NPotionContainer potionContainer2 = this.PotionContainer;
    Variant variant4 = Variant.From<NPotionContainer>(ref potionContainer2);
    serializationInfo4.AddProperty(potionContainer1, variant4);
    GodotSerializationInfo serializationInfo5 = info;
    StringName roomIcon1 = NTopBar.PropertyName.RoomIcon;
    NTopBarRoomIcon roomIcon2 = this.RoomIcon;
    Variant variant5 = Variant.From<NTopBarRoomIcon>(ref roomIcon2);
    serializationInfo5.AddProperty(roomIcon1, variant5);
    GodotSerializationInfo serializationInfo6 = info;
    StringName floorIcon1 = NTopBar.PropertyName.FloorIcon;
    NTopBarFloorIcon floorIcon2 = this.FloorIcon;
    Variant variant6 = Variant.From<NTopBarFloorIcon>(ref floorIcon2);
    serializationInfo6.AddProperty(floorIcon1, variant6);
    GodotSerializationInfo serializationInfo7 = info;
    StringName bossIcon1 = NTopBar.PropertyName.BossIcon;
    NTopBarBossIcon bossIcon2 = this.BossIcon;
    Variant variant7 = Variant.From<NTopBarBossIcon>(ref bossIcon2);
    serializationInfo7.AddProperty(bossIcon1, variant7);
    GodotSerializationInfo serializationInfo8 = info;
    StringName gold1 = NTopBar.PropertyName.Gold;
    NTopBarGold gold2 = this.Gold;
    Variant variant8 = Variant.From<NTopBarGold>(ref gold2);
    serializationInfo8.AddProperty(gold1, variant8);
    GodotSerializationInfo serializationInfo9 = info;
    StringName hp1 = NTopBar.PropertyName.Hp;
    NTopBarHp hp2 = this.Hp;
    Variant variant9 = Variant.From<NTopBarHp>(ref hp2);
    serializationInfo9.AddProperty(hp1, variant9);
    GodotSerializationInfo serializationInfo10 = info;
    StringName portrait1 = NTopBar.PropertyName.Portrait;
    NTopBarPortrait portrait2 = this.Portrait;
    Variant variant10 = Variant.From<NTopBarPortrait>(ref portrait2);
    serializationInfo10.AddProperty(portrait1, variant10);
    GodotSerializationInfo serializationInfo11 = info;
    StringName portraitTip1 = NTopBar.PropertyName.PortraitTip;
    NTopBarPortraitTip portraitTip2 = this.PortraitTip;
    Variant variant11 = Variant.From<NTopBarPortraitTip>(ref portraitTip2);
    serializationInfo11.AddProperty(portraitTip1, variant11);
    GodotSerializationInfo serializationInfo12 = info;
    StringName timer1 = NTopBar.PropertyName.Timer;
    NRunTimer timer2 = this.Timer;
    Variant variant12 = Variant.From<NRunTimer>(ref timer2);
    serializationInfo12.AddProperty(timer1, variant12);
    GodotSerializationInfo serializationInfo13 = info;
    StringName trailContainer1 = NTopBar.PropertyName.TrailContainer;
    Node trailContainer2 = this.TrailContainer;
    Variant variant13 = Variant.From<Node>(ref trailContainer2);
    serializationInfo13.AddProperty(trailContainer1, variant13);
    GodotSerializationInfo serializationInfo14 = info;
    StringName activeScreenProxy1 = NTopBar.PropertyName.ActiveScreenProxy;
    Control activeScreenProxy2 = this.ActiveScreenProxy;
    Variant variant14 = Variant.From<Control>(ref activeScreenProxy2);
    serializationInfo14.AddProperty(activeScreenProxy1, variant14);
    info.AddProperty(NTopBar.PropertyName._capstoneContainer, Variant.From<NCapstoneContainer>(ref this._capstoneContainer));
    info.AddProperty(NTopBar.PropertyName._modifiersContainer, Variant.From<Control>(ref this._modifiersContainer));
    info.AddProperty(NTopBar.PropertyName._achievementLock, Variant.From<Control>(ref this._achievementLock));
    info.AddProperty(NTopBar.PropertyName._ascensionIcon, Variant.From<Control>(ref this._ascensionIcon));
    info.AddProperty(NTopBar.PropertyName._ascensionLabel, Variant.From<MegaLabel>(ref this._ascensionLabel));
    info.AddProperty(NTopBar.PropertyName._ascensionHsv, Variant.From<ShaderMaterial>(ref this._ascensionHsv));
    info.AddProperty(NTopBar.PropertyName._hideTween, Variant.From<Tween>(ref this._hideTween));
    info.AddProperty(NTopBar.PropertyName._isDebugHidden, Variant.From<bool>(ref this._isDebugHidden));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NTopBar.PropertyName.Map, ref variant1))
      this.Map = ((Variant) ref variant1).As<NTopBarMapButton>();
    Variant variant2;
    if (info.TryGetProperty(NTopBar.PropertyName.Deck, ref variant2))
      this.Deck = ((Variant) ref variant2).As<NTopBarDeckButton>();
    Variant variant3;
    if (info.TryGetProperty(NTopBar.PropertyName.Pause, ref variant3))
      this.Pause = ((Variant) ref variant3).As<NTopBarPauseButton>();
    Variant variant4;
    if (info.TryGetProperty(NTopBar.PropertyName.PotionContainer, ref variant4))
      this.PotionContainer = ((Variant) ref variant4).As<NPotionContainer>();
    Variant variant5;
    if (info.TryGetProperty(NTopBar.PropertyName.RoomIcon, ref variant5))
      this.RoomIcon = ((Variant) ref variant5).As<NTopBarRoomIcon>();
    Variant variant6;
    if (info.TryGetProperty(NTopBar.PropertyName.FloorIcon, ref variant6))
      this.FloorIcon = ((Variant) ref variant6).As<NTopBarFloorIcon>();
    Variant variant7;
    if (info.TryGetProperty(NTopBar.PropertyName.BossIcon, ref variant7))
      this.BossIcon = ((Variant) ref variant7).As<NTopBarBossIcon>();
    Variant variant8;
    if (info.TryGetProperty(NTopBar.PropertyName.Gold, ref variant8))
      this.Gold = ((Variant) ref variant8).As<NTopBarGold>();
    Variant variant9;
    if (info.TryGetProperty(NTopBar.PropertyName.Hp, ref variant9))
      this.Hp = ((Variant) ref variant9).As<NTopBarHp>();
    Variant variant10;
    if (info.TryGetProperty(NTopBar.PropertyName.Portrait, ref variant10))
      this.Portrait = ((Variant) ref variant10).As<NTopBarPortrait>();
    Variant variant11;
    if (info.TryGetProperty(NTopBar.PropertyName.PortraitTip, ref variant11))
      this.PortraitTip = ((Variant) ref variant11).As<NTopBarPortraitTip>();
    Variant variant12;
    if (info.TryGetProperty(NTopBar.PropertyName.Timer, ref variant12))
      this.Timer = ((Variant) ref variant12).As<NRunTimer>();
    Variant variant13;
    if (info.TryGetProperty(NTopBar.PropertyName.TrailContainer, ref variant13))
      this.TrailContainer = ((Variant) ref variant13).As<Node>();
    Variant variant14;
    if (info.TryGetProperty(NTopBar.PropertyName.ActiveScreenProxy, ref variant14))
      this.ActiveScreenProxy = ((Variant) ref variant14).As<Control>();
    Variant variant15;
    if (info.TryGetProperty(NTopBar.PropertyName._capstoneContainer, ref variant15))
      this._capstoneContainer = ((Variant) ref variant15).As<NCapstoneContainer>();
    Variant variant16;
    if (info.TryGetProperty(NTopBar.PropertyName._modifiersContainer, ref variant16))
      this._modifiersContainer = ((Variant) ref variant16).As<Control>();
    Variant variant17;
    if (info.TryGetProperty(NTopBar.PropertyName._achievementLock, ref variant17))
      this._achievementLock = ((Variant) ref variant17).As<Control>();
    Variant variant18;
    if (info.TryGetProperty(NTopBar.PropertyName._ascensionIcon, ref variant18))
      this._ascensionIcon = ((Variant) ref variant18).As<Control>();
    Variant variant19;
    if (info.TryGetProperty(NTopBar.PropertyName._ascensionLabel, ref variant19))
      this._ascensionLabel = ((Variant) ref variant19).As<MegaLabel>();
    Variant variant20;
    if (info.TryGetProperty(NTopBar.PropertyName._ascensionHsv, ref variant20))
      this._ascensionHsv = ((Variant) ref variant20).As<ShaderMaterial>();
    Variant variant21;
    if (info.TryGetProperty(NTopBar.PropertyName._hideTween, ref variant21))
      this._hideTween = ((Variant) ref variant21).As<Tween>();
    Variant variant22;
    if (!info.TryGetProperty(NTopBar.PropertyName._isDebugHidden, ref variant22))
      return;
    this._isDebugHidden = ((Variant) ref variant22).As<bool>();
  }

  public class MethodName : Control.MethodName
  {
    public static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName _EnterTree = StringName.op_Implicit(nameof (_EnterTree));
    public static readonly StringName _ExitTree = StringName.op_Implicit(nameof (_ExitTree));
    public static readonly StringName ToggleAnimState = StringName.op_Implicit(nameof (ToggleAnimState));
    public static readonly StringName _Input = StringName.op_Implicit(nameof (_Input));
    public static readonly StringName DebugHideTopBar = StringName.op_Implicit(nameof (DebugHideTopBar));
    public static readonly StringName AnimHide = StringName.op_Implicit(nameof (AnimHide));
    public static readonly StringName AnimShow = StringName.op_Implicit(nameof (AnimShow));
    public static readonly StringName MaxPotionsChanged = StringName.op_Implicit(nameof (MaxPotionsChanged));
    public static readonly StringName UpdateNavigation = StringName.op_Implicit(nameof (UpdateNavigation));
  }

  public class PropertyName : Control.PropertyName
  {
    public static readonly StringName Map = StringName.op_Implicit(nameof (Map));
    public static readonly StringName Deck = StringName.op_Implicit(nameof (Deck));
    public static readonly StringName Pause = StringName.op_Implicit(nameof (Pause));
    public static readonly StringName PotionContainer = StringName.op_Implicit(nameof (PotionContainer));
    public static readonly StringName RoomIcon = StringName.op_Implicit(nameof (RoomIcon));
    public static readonly StringName FloorIcon = StringName.op_Implicit(nameof (FloorIcon));
    public static readonly StringName BossIcon = StringName.op_Implicit(nameof (BossIcon));
    public static readonly StringName Gold = StringName.op_Implicit(nameof (Gold));
    public static readonly StringName Hp = StringName.op_Implicit(nameof (Hp));
    public static readonly StringName Portrait = StringName.op_Implicit(nameof (Portrait));
    public static readonly StringName PortraitTip = StringName.op_Implicit(nameof (PortraitTip));
    public static readonly StringName Timer = StringName.op_Implicit(nameof (Timer));
    public static readonly StringName TrailContainer = StringName.op_Implicit(nameof (TrailContainer));
    public static readonly StringName ActiveScreenProxy = StringName.op_Implicit(nameof (ActiveScreenProxy));
    public static readonly StringName _capstoneContainer = StringName.op_Implicit(nameof (_capstoneContainer));
    public static readonly StringName _modifiersContainer = StringName.op_Implicit(nameof (_modifiersContainer));
    public static readonly StringName _achievementLock = StringName.op_Implicit(nameof (_achievementLock));
    public static readonly StringName _ascensionIcon = StringName.op_Implicit(nameof (_ascensionIcon));
    public static readonly StringName _ascensionLabel = StringName.op_Implicit(nameof (_ascensionLabel));
    public static readonly StringName _ascensionHsv = StringName.op_Implicit(nameof (_ascensionHsv));
    public static readonly StringName _hideTween = StringName.op_Implicit(nameof (_hideTween));
    public static readonly StringName _isDebugHidden = StringName.op_Implicit(nameof (_isDebugHidden));
  }

  public class SignalName : Control.SignalName
  {
  }
}
