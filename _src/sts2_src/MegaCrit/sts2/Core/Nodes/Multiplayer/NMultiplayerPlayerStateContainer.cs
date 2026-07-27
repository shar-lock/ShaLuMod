// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Multiplayer.NMultiplayerPlayerStateContainer
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
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.Relics;
using MegaCrit.Sts2.Core.Nodes.Screens.ScreenContext;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.Runs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Multiplayer;

[ScriptPath("res://src/Core/Nodes/Multiplayer/NMultiplayerPlayerStateContainer.cs")]
public class NMultiplayerPlayerStateContainer : Control
{
  private IRunState _runState;
  private readonly List<NMultiplayerPlayerState> _nodes = new List<NMultiplayerPlayerState>();
  private Tween? _tween;
  private bool _hidden;

  public NMultiplayerPlayerState? FirstPlayerState
  {
    get => ((Node) this).GetChild<NMultiplayerPlayerState>(0, false);
  }

  public override void _EnterTree()
  {
    ActiveScreenContext.Instance.Updated += new Action(this.UpdateNavigation);
  }

  public override void _ExitTree()
  {
    ActiveScreenContext.Instance.Updated -= new Action(this.UpdateNavigation);
  }

  public override void _Input(InputEvent inputEvent)
  {
    if (!inputEvent.IsActionReleased(DebugHotkey.hideMpHealthBars, false))
      return;
    ((CanvasItem) this).Visible = !((CanvasItem) this).Visible;
    ((Node) NGame.Instance).AddChildSafely((Node) NFullscreenTextVfx.Create(!((CanvasItem) this).Visible ? "Hide MP Health bars" : "Show MP Health bars"));
  }

  public void Initialize(RunState runState)
  {
    this._runState = (IRunState) runState;
    if (this._runState.Players.Count <= 1)
      return;
    Player me = LocalContext.GetMe((IPlayerCollection) this._runState);
    NMultiplayerPlayerState child1 = NMultiplayerPlayerState.Create(me);
    ((Node) this).AddChildSafely((Node) child1);
    this._nodes.Add(child1);
    // ISSUE: object of a compiler-generated type is created
    foreach (Player player in this._runState.Players.Except<Player>((IEnumerable<Player>) new \u003C\u003Ez__ReadOnlySingleElementList<Player>(me)))
    {
      NMultiplayerPlayerState child2 = NMultiplayerPlayerState.Create(player);
      ((Node) this).AddChildSafely((Node) child2);
      this._nodes.Add(child2);
    }
    this.UpdatePosition();
    ((GodotObject) NRun.Instance.GlobalUi.RelicInventory).Connect(NRelicInventory.SignalName.RelicsChanged, Callable.From(new Action(this.UpdatePositionAfterOneFrame)), 0U);
    ((GodotObject) ((Node) this).GetViewport()).Connect(Viewport.SignalName.SizeChanged, Callable.From(new Action(this.UpdatePositionAfterOneFrame)), 0U);
    for (int index = 0; index < ((Node) this).GetChildCount(false); ++index)
    {
      Control hitbox = (Control) ((Node) this).GetChild<NMultiplayerPlayerState>(index, false).Hitbox;
      hitbox.FocusNeighborLeft = ((Node) hitbox).GetPath();
      hitbox.FocusNeighborTop = index > 0 ? ((Node) ((Node) this).GetChild<NMultiplayerPlayerState>(index - 1, false).Hitbox).GetPath() : (NodePath) null;
      hitbox.FocusNeighborBottom = index < ((Node) this).GetChildCount(false) - 1 ? ((Node) ((Node) this).GetChild<NMultiplayerPlayerState>(index + 1, false).Hitbox).GetPath() : (NodePath) null;
    }
  }

  private void UpdateNavigation()
  {
    Control activeScreenProxy = NRun.Instance.GlobalUi.TopBar.ActiveScreenProxy;
    NodePath path = ((Node) activeScreenProxy).IsValid() ? ((Node) activeScreenProxy).GetPath() : (NodePath) null;
    for (int index = 0; index < ((Node) this).GetChildCount(false); ++index)
    {
      Control hitbox = (Control) ((Node) this).GetChild<NMultiplayerPlayerState>(index, false).Hitbox;
      hitbox.FocusNeighborTop = index > 0 ? ((Node) ((Node) this).GetChild<NMultiplayerPlayerState>(index - 1, false).Hitbox).GetPath() : (NodePath) null;
      hitbox.FocusNeighborBottom = index < ((Node) this).GetChildCount(false) - 1 ? ((Node) ((Node) this).GetChild<NMultiplayerPlayerState>(index + 1, false).Hitbox).GetPath() : path;
    }
  }

  public void LockNavigation()
  {
    for (int index = 0; index < ((Node) this).GetChildCount(false); ++index)
    {
      Control hitbox = (Control) ((Node) this).GetChild<NMultiplayerPlayerState>(index, false).Hitbox;
      hitbox.FocusNeighborTop = index > 0 ? ((Node) ((Node) this).GetChild<NMultiplayerPlayerState>(index - 1, false).Hitbox).GetPath() : ((Node) hitbox).GetPath();
      hitbox.FocusNeighborBottom = index < ((Node) this).GetChildCount(false) - 1 ? ((Node) ((Node) this).GetChild<NMultiplayerPlayerState>(index + 1, false).Hitbox).GetPath() : ((Node) hitbox).GetPath();
      hitbox.FocusNeighborLeft = ((Node) hitbox).GetPath();
      hitbox.FocusNeighborRight = ((Node) hitbox).GetPath();
    }
  }

  public void UnlockNavigation() => this.UpdateNavigation();

  private void UpdatePositionAfterOneFrame()
  {
    TaskHelper.RunSafely(this.UpdatePositionAfterOneFrameAsync());
  }

  private async Task UpdatePositionAfterOneFrameAsync()
  {
    double num = (double) await ((Node) this).AwaitProcessFrame();
    this.UpdatePosition();
  }

  private void UpdatePosition() => this.Position = this.GetTargetPosition();

  private Vector2 GetTargetPosition()
  {
    NRelicInventory relicInventory = NRun.Instance.GlobalUi.RelicInventory;
    int lineCount = relicInventory.GetLineCount();
    Vector2 targetPosition;
    if (lineCount == 0 || ((Node) relicInventory).GetChildCount(false) == 0)
    {
      targetPosition = relicInventory.GetDefaultPosition();
    }
    else
    {
      float y = ((Node) relicInventory).GetChild<Control>(0, false).Size.Y;
      float themeConstant = (float) ((Control) relicInventory).GetThemeConstant(ThemeConstants.FlowContainer.VSeparation, StringName.op_Implicit("FlowContainer"));
      targetPosition = Vector2.op_Addition(relicInventory.GetDefaultPosition(), Vector2.op_Multiply((float) lineCount * (y + themeConstant), Vector2.Down));
    }
    if (this._hidden)
      targetPosition.X = -this.Size.X;
    return targetPosition;
  }

  public void HighlightPlayer(Player player)
  {
    this._nodes.FirstOrDefault<NMultiplayerPlayerState>((Func<NMultiplayerPlayerState, bool>) (n => n.Player == player))?.OnCreatureHovered();
  }

  public void UnhighlightPlayer(Player player)
  {
    this._nodes.FirstOrDefault<NMultiplayerPlayerState>((Func<NMultiplayerPlayerState, bool>) (n => n.Player == player))?.OnCreatureUnhovered();
  }

  public void FlashPlayerReady(Player player)
  {
    this._nodes.FirstOrDefault<NMultiplayerPlayerState>((Func<NMultiplayerPlayerState, bool>) (n => n.Player == player))?.FlashPlayerReady();
  }

  public void AnimHide()
  {
    this._hidden = true;
    this._tween?.Kill();
    this._tween = ((Node) this).CreateTween();
    this._tween.TweenProperty((GodotObject) this, NodePath.op_Implicit("position"), Variant.op_Implicit(this.GetTargetPosition()), 0.20000000298023224).SetTrans((Tween.TransitionType) 4L).SetEase((Tween.EaseType) 2L);
    this._tween.TweenProperty((GodotObject) this, NodePath.op_Implicit("modulate:a"), Variant.op_Implicit(0.0f), 0.20000000298023224).SetTrans((Tween.TransitionType) 4L).SetEase((Tween.EaseType) 2L);
  }

  public void AnimShow()
  {
    this._hidden = false;
    this._tween?.Kill();
    this._tween = ((Node) this).CreateTween();
    this._tween.TweenProperty((GodotObject) this, NodePath.op_Implicit("position"), Variant.op_Implicit(this.GetTargetPosition()), 0.25).SetTrans((Tween.TransitionType) 4L).SetEase((Tween.EaseType) 1L);
    this._tween.TweenProperty((GodotObject) this, NodePath.op_Implicit("modulate:a"), Variant.op_Implicit(1f), 0.15000000596046448).SetTrans((Tween.TransitionType) 4L).SetEase((Tween.EaseType) 0L);
  }

  public void ShowImmediately()
  {
    this._tween?.Kill();
    this._hidden = false;
    this.Position = this.GetTargetPosition();
    Color modulate = ((CanvasItem) this).Modulate;
    modulate.A = 1f;
    ((CanvasItem) this).Modulate = modulate;
  }

  public void HideImmediately()
  {
    this._tween?.Kill();
    this._hidden = true;
    this.Position = this.GetTargetPosition();
    Color modulate = ((CanvasItem) this).Modulate;
    modulate.A = 0.0f;
    ((CanvasItem) this).Modulate = modulate;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(13)
    {
      new MethodInfo(NMultiplayerPlayerStateContainer.MethodName._EnterTree, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NMultiplayerPlayerStateContainer.MethodName._ExitTree, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NMultiplayerPlayerStateContainer.MethodName._Input, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("inputEvent"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("InputEvent"), false)
      }, (List<Variant>) null),
      new MethodInfo(NMultiplayerPlayerStateContainer.MethodName.UpdateNavigation, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NMultiplayerPlayerStateContainer.MethodName.LockNavigation, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NMultiplayerPlayerStateContainer.MethodName.UnlockNavigation, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NMultiplayerPlayerStateContainer.MethodName.UpdatePositionAfterOneFrame, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NMultiplayerPlayerStateContainer.MethodName.UpdatePosition, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NMultiplayerPlayerStateContainer.MethodName.GetTargetPosition, new PropertyInfo((Variant.Type) 5L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NMultiplayerPlayerStateContainer.MethodName.AnimHide, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NMultiplayerPlayerStateContainer.MethodName.AnimShow, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NMultiplayerPlayerStateContainer.MethodName.ShowImmediately, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NMultiplayerPlayerStateContainer.MethodName.HideImmediately, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NMultiplayerPlayerStateContainer.MethodName._EnterTree) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._EnterTree();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NMultiplayerPlayerStateContainer.MethodName._ExitTree) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._ExitTree();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NMultiplayerPlayerStateContainer.MethodName._Input) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      ((Node) this)._Input(VariantUtils.ConvertTo<InputEvent>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NMultiplayerPlayerStateContainer.MethodName.UpdateNavigation) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.UpdateNavigation();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NMultiplayerPlayerStateContainer.MethodName.LockNavigation) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.LockNavigation();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NMultiplayerPlayerStateContainer.MethodName.UnlockNavigation) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.UnlockNavigation();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NMultiplayerPlayerStateContainer.MethodName.UpdatePositionAfterOneFrame) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.UpdatePositionAfterOneFrame();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NMultiplayerPlayerStateContainer.MethodName.UpdatePosition) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.UpdatePosition();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NMultiplayerPlayerStateContainer.MethodName.GetTargetPosition) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      Vector2 targetPosition = this.GetTargetPosition();
      ret = VariantUtils.CreateFrom<Vector2>(ref targetPosition);
      return true;
    }
    if (StringName.op_Equality(ref method, NMultiplayerPlayerStateContainer.MethodName.AnimHide) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.AnimHide();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NMultiplayerPlayerStateContainer.MethodName.AnimShow) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.AnimShow();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NMultiplayerPlayerStateContainer.MethodName.ShowImmediately) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.ShowImmediately();
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NMultiplayerPlayerStateContainer.MethodName.HideImmediately) || ((NativeVariantPtrArgs) ref args).Count != 0)
      return base.InvokeGodotClassMethod(ref method, args, ref ret);
    this.HideImmediately();
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NMultiplayerPlayerStateContainer.MethodName._EnterTree) || StringName.op_Equality(ref method, NMultiplayerPlayerStateContainer.MethodName._ExitTree) || StringName.op_Equality(ref method, NMultiplayerPlayerStateContainer.MethodName._Input) || StringName.op_Equality(ref method, NMultiplayerPlayerStateContainer.MethodName.UpdateNavigation) || StringName.op_Equality(ref method, NMultiplayerPlayerStateContainer.MethodName.LockNavigation) || StringName.op_Equality(ref method, NMultiplayerPlayerStateContainer.MethodName.UnlockNavigation) || StringName.op_Equality(ref method, NMultiplayerPlayerStateContainer.MethodName.UpdatePositionAfterOneFrame) || StringName.op_Equality(ref method, NMultiplayerPlayerStateContainer.MethodName.UpdatePosition) || StringName.op_Equality(ref method, NMultiplayerPlayerStateContainer.MethodName.GetTargetPosition) || StringName.op_Equality(ref method, NMultiplayerPlayerStateContainer.MethodName.AnimHide) || StringName.op_Equality(ref method, NMultiplayerPlayerStateContainer.MethodName.AnimShow) || StringName.op_Equality(ref method, NMultiplayerPlayerStateContainer.MethodName.ShowImmediately) || StringName.op_Equality(ref method, NMultiplayerPlayerStateContainer.MethodName.HideImmediately) || base.HasGodotClassMethod(ref method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NMultiplayerPlayerStateContainer.PropertyName._tween))
    {
      this._tween = VariantUtils.ConvertTo<Tween>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NMultiplayerPlayerStateContainer.PropertyName._hidden))
      return ((GodotObject) this).SetGodotClassPropertyValue(ref name, ref value);
    this._hidden = VariantUtils.ConvertTo<bool>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NMultiplayerPlayerStateContainer.PropertyName.FirstPlayerState))
    {
      ref godot_variant local = ref value;
      NMultiplayerPlayerState firstPlayerState = this.FirstPlayerState;
      godot_variant from = VariantUtils.CreateFrom<NMultiplayerPlayerState>(ref firstPlayerState);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NMultiplayerPlayerStateContainer.PropertyName._tween))
    {
      value = VariantUtils.CreateFrom<Tween>(ref this._tween);
      return true;
    }
    if (!StringName.op_Equality(ref name, NMultiplayerPlayerStateContainer.PropertyName._hidden))
      return ((GodotObject) this).GetGodotClassPropertyValue(ref name, ref value);
    value = VariantUtils.CreateFrom<bool>(ref this._hidden);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NMultiplayerPlayerStateContainer.PropertyName.FirstPlayerState, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NMultiplayerPlayerStateContainer.PropertyName._tween, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 1L, NMultiplayerPlayerStateContainer.PropertyName._hidden, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).SaveGodotObjectData(info);
    info.AddProperty(NMultiplayerPlayerStateContainer.PropertyName._tween, Variant.From<Tween>(ref this._tween));
    info.AddProperty(NMultiplayerPlayerStateContainer.PropertyName._hidden, Variant.From<bool>(ref this._hidden));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NMultiplayerPlayerStateContainer.PropertyName._tween, ref variant1))
      this._tween = ((Variant) ref variant1).As<Tween>();
    Variant variant2;
    if (!info.TryGetProperty(NMultiplayerPlayerStateContainer.PropertyName._hidden, ref variant2))
      return;
    this._hidden = ((Variant) ref variant2).As<bool>();
  }

  public class MethodName : Control.MethodName
  {
    public static readonly StringName _EnterTree = StringName.op_Implicit(nameof (_EnterTree));
    public static readonly StringName _ExitTree = StringName.op_Implicit(nameof (_ExitTree));
    public static readonly StringName _Input = StringName.op_Implicit(nameof (_Input));
    public static readonly StringName UpdateNavigation = StringName.op_Implicit(nameof (UpdateNavigation));
    public static readonly StringName LockNavigation = StringName.op_Implicit(nameof (LockNavigation));
    public static readonly StringName UnlockNavigation = StringName.op_Implicit(nameof (UnlockNavigation));
    public static readonly StringName UpdatePositionAfterOneFrame = StringName.op_Implicit(nameof (UpdatePositionAfterOneFrame));
    public static readonly StringName UpdatePosition = StringName.op_Implicit(nameof (UpdatePosition));
    public static readonly StringName GetTargetPosition = StringName.op_Implicit(nameof (GetTargetPosition));
    public static readonly StringName AnimHide = StringName.op_Implicit(nameof (AnimHide));
    public static readonly StringName AnimShow = StringName.op_Implicit(nameof (AnimShow));
    public static readonly StringName ShowImmediately = StringName.op_Implicit(nameof (ShowImmediately));
    public static readonly StringName HideImmediately = StringName.op_Implicit(nameof (HideImmediately));
  }

  public class PropertyName : Control.PropertyName
  {
    public static readonly StringName FirstPlayerState = StringName.op_Implicit(nameof (FirstPlayerState));
    public static readonly StringName _tween = StringName.op_Implicit(nameof (_tween));
    public static readonly StringName _hidden = StringName.op_Implicit(nameof (_hidden));
  }

  public class SignalName : Control.SignalName
  {
  }
}
