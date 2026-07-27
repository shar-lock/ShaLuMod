// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Combat.NPingButton
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.ControllerInput;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.HoverTips;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Screens.ScreenContext;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Runs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Combat;

[ScriptPath("res://src/Core/Nodes/Combat/NPingButton.cs")]
public class NPingButton : NButton
{
  private static readonly StringName _v = new StringName("v");
  private const double _flyInOutDuration = 0.5;
  private NPingButton.State _state = NPingButton.State.Hidden;
  private Control _visuals;
  private TextureRect _image;
  private MegaLabel _label;
  private Viewport _viewport;
  private ShaderMaterial _hsv;
  private CancellationTokenSource? _showCancelTokenSource;
  private static readonly Vector2 _showPosRatio = Vector2.op_Division(new Vector2(1536f, 932f), NGame.devResolution);
  private static readonly Vector2 _hidePosRatio = Vector2.op_Addition(NPingButton._showPosRatio, Vector2.op_Division(new Vector2(0.0f, 250f), NGame.devResolution));
  private Tween? _positionTween;
  private Tween? _hoverTween;

  private Vector2 ShowPos
  {
    get
    {
      Vector2 showPosRatio = NPingButton._showPosRatio;
      Rect2 visibleRect = this._viewport.GetVisibleRect();
      Vector2 size = ((Rect2) ref visibleRect).Size;
      return Vector2.op_Multiply(showPosRatio, size);
    }
  }

  private Vector2 HidePos
  {
    get
    {
      Vector2 hidePosRatio = NPingButton._hidePosRatio;
      Rect2 visibleRect = this._viewport.GetVisibleRect();
      Vector2 size = ((Rect2) ref visibleRect).Size;
      return Vector2.op_Multiply(hidePosRatio, size);
    }
  }

  protected override string[] Hotkeys
  {
    get
    {
      return new string[1]
      {
        StringName.op_Implicit(MegaInput.select)
      };
    }
  }

  public override void _Ready()
  {
    this.ConnectSignals();
    this._visuals = ((Node) this).GetNode<Control>(NodePath.op_Implicit("Visuals"));
    this._image = ((Node) this).GetNode<TextureRect>(NodePath.op_Implicit("Visuals/Image"));
    this._label = ((Node) this).GetNode<MegaLabel>(NodePath.op_Implicit("Visuals/Label"));
    this._viewport = ((Node) this).GetViewport();
    this._hsv = (ShaderMaterial) ((CanvasItem) this._image).Material;
    this._label.SetTextAutoSize(new LocString("gameplay_ui", "PING_BUTTON").GetFormattedText());
    this.Position = this.HidePos;
    this.Disable();
  }

  public override void _EnterTree()
  {
    base._EnterTree();
    CombatManager.Instance.AboutToSwitchToEnemyTurn += new Action<CombatState>(this.OnAboutToSwitchToEnemyTurn);
    CombatManager.Instance.PlayerEndedTurn += new Action<Player, bool>(this.AfterPlayerEndedTurn);
    CombatManager.Instance.PlayerUnendedTurn += new Action<Player>(this.AfterPlayerUnendedTurn);
  }

  public override void _ExitTree()
  {
    base._ExitTree();
    this._showCancelTokenSource?.Cancel();
    this._showCancelTokenSource = (CancellationTokenSource) null;
    CombatManager.Instance.AboutToSwitchToEnemyTurn -= new Action<CombatState>(this.OnAboutToSwitchToEnemyTurn);
    CombatManager.Instance.PlayerEndedTurn -= new Action<Player, bool>(this.AfterPlayerEndedTurn);
    CombatManager.Instance.PlayerUnendedTurn -= new Action<Player>(this.AfterPlayerUnendedTurn);
  }

  private void AfterPlayerEndedTurn(Player player, bool _)
  {
    if (CombatManager.Instance.AllPlayersReadyToEndTurn())
    {
      this.SetState(NPingButton.State.Disabled);
    }
    else
    {
      if (!LocalContext.IsMe(player))
        return;
      this._showCancelTokenSource = new CancellationTokenSource();
      TaskHelper.RunSafely(this.AnimInAfterDelay());
    }
  }

  private void AfterPlayerUnendedTurn(Player player)
  {
    if (!LocalContext.IsMe(player))
      return;
    this._showCancelTokenSource?.Cancel();
    this.SetState(NPingButton.State.Hidden);
  }

  private async Task AnimInAfterDelay()
  {
    await Task.Delay(500, this._showCancelTokenSource.Token);
    if (this._showCancelTokenSource.IsCancellationRequested)
      return;
    this.SetState(NPingButton.State.Enabled);
  }

  private void OnAboutToSwitchToEnemyTurn(CombatState _) => this.SetState(NPingButton.State.Hidden);

  protected override void OnRelease()
  {
    if (this._state != NPingButton.State.Enabled)
      return;
    RunManager.Instance.FlavorSynchronizer.SendEndTurnPing();
    this._hoverTween?.Kill();
    this._hoverTween = ((Node) this).CreateTween().SetParallel(true);
    this._hoverTween.TweenMethod(Callable.From<float>(new Action<float>(this.UpdateShaderV)), this._hsv.GetShaderParameter(NPingButton._v), Variant.op_Implicit(this.IsFocused ? 1.5f : 1f), 0.5).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 5L);
    this._hoverTween.TweenProperty((GodotObject) this._visuals, NodePath.op_Implicit("position"), Variant.op_Implicit(Vector2.Zero), 0.5).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 5L);
    this._hoverTween.TweenProperty((GodotObject) this._label, NodePath.op_Implicit("modulate"), Variant.op_Implicit(this.IsEnabled ? StsColors.cream : StsColors.gray), 0.5).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 5L);
  }

  protected override void OnEnable()
  {
    ((CanvasItem) this._image).Modulate = Colors.White;
    ((CanvasItem) this._label).Modulate = StsColors.cream;
  }

  protected override void OnDisable()
  {
    NHoverTipSet.Remove((Control) this);
    ((CanvasItem) this._image).Modulate = StsColors.gray;
    ((CanvasItem) this._label).Modulate = StsColors.gray;
  }

  private void AnimOut()
  {
    this._showCancelTokenSource?.Cancel();
    this._hoverTween?.Kill();
    this._positionTween?.Kill();
    this._positionTween = ((Node) this).CreateTween();
    this._positionTween.TweenProperty((GodotObject) this, NodePath.op_Implicit("position"), Variant.op_Implicit(this.HidePos), 0.5).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 5L);
  }

  private void AnimIn()
  {
    this._positionTween?.Kill();
    this._positionTween = ((Node) this).CreateTween();
    this._positionTween.TweenProperty((GodotObject) this, NodePath.op_Implicit("position"), Variant.op_Implicit(this.ShowPos), 0.5).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 5L);
  }

  public void OnCombatEnded() => this.SetState(NPingButton.State.Hidden);

  protected override void OnFocus()
  {
    base.OnFocus();
    this._hoverTween?.Kill();
    this._hsv.SetShaderParameter(NPingButton._v, Variant.op_Implicit(1.5));
    this._visuals.Position = new Vector2(0.0f, -2f);
  }

  protected override void OnUnfocus()
  {
    NHoverTipSet.Remove((Control) this);
    this._hoverTween?.Kill();
    this._hoverTween = ((Node) this).CreateTween().SetParallel(true);
    this._hoverTween.TweenMethod(Callable.From<float>(new Action<float>(this.UpdateShaderV)), this._hsv.GetShaderParameter(NPingButton._v), Variant.op_Implicit(1f), 0.5).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 5L);
    this._hoverTween.TweenProperty((GodotObject) this._visuals, NodePath.op_Implicit("position"), Variant.op_Implicit(Vector2.Zero), 0.5).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 5L);
    this._hoverTween.TweenProperty((GodotObject) this._label, NodePath.op_Implicit("modulate"), Variant.op_Implicit(this.IsEnabled ? StsColors.cream : StsColors.gray), 0.5).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 5L);
  }

  protected override void OnPress()
  {
    this._hoverTween?.Kill();
    this._hoverTween = ((Node) this).CreateTween().SetParallel(true);
    this._hoverTween.TweenMethod(Callable.From<float>(new Action<float>(this.UpdateShaderV)), this._hsv.GetShaderParameter(NPingButton._v), Variant.op_Implicit(1f), 0.5).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 5L);
    this._hoverTween.TweenProperty((GodotObject) this._visuals, NodePath.op_Implicit("position"), Variant.op_Implicit(new Vector2(0.0f, 4f)), 0.5).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 7L);
    this._hoverTween.TweenProperty((GodotObject) this._label, NodePath.op_Implicit("modulate"), Variant.op_Implicit(Colors.DarkGray), 0.5).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 5L);
  }

  private void UpdateShaderV(float value)
  {
    this._hsv.SetShaderParameter(NPingButton._v, Variant.op_Implicit(value));
  }

  private void SetState(NPingButton.State newState)
  {
    if (this._state == newState)
      return;
    if (newState == NPingButton.State.Hidden)
      this.AnimOut();
    if (newState == NPingButton.State.Enabled && this._state == NPingButton.State.Hidden)
      this.AnimIn();
    this._state = newState;
    this.RefreshEnabled();
  }

  public void RefreshEnabled()
  {
    bool flag = NCombatRoom.Instance == null || NCombatRoom.Instance.Mode != CombatRoomMode.ActiveCombat || !ActiveScreenContext.Instance.IsCurrent((IScreenContext) NCombatRoom.Instance) || NCombatRoom.Instance.Ui.Hand.IsInCardSelection;
    if (this._state == NPingButton.State.Enabled && !flag)
      this.Enable();
    else
      this.Disable();
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(15)
    {
      new MethodInfo(NPingButton.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NPingButton.MethodName._EnterTree, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NPingButton.MethodName._ExitTree, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NPingButton.MethodName.OnRelease, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NPingButton.MethodName.OnEnable, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NPingButton.MethodName.OnDisable, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NPingButton.MethodName.AnimOut, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NPingButton.MethodName.AnimIn, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NPingButton.MethodName.OnCombatEnded, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NPingButton.MethodName.OnFocus, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NPingButton.MethodName.OnUnfocus, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NPingButton.MethodName.OnPress, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NPingButton.MethodName.UpdateShaderV, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 3L, StringName.op_Implicit("value"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NPingButton.MethodName.SetState, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 2L, StringName.op_Implicit("newState"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NPingButton.MethodName.RefreshEnabled, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NPingButton.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NPingButton.MethodName._EnterTree) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._EnterTree();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NPingButton.MethodName._ExitTree) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._ExitTree();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NPingButton.MethodName.OnRelease) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnRelease();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NPingButton.MethodName.OnEnable) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnEnable();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NPingButton.MethodName.OnDisable) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnDisable();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NPingButton.MethodName.AnimOut) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.AnimOut();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NPingButton.MethodName.AnimIn) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.AnimIn();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NPingButton.MethodName.OnCombatEnded) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnCombatEnded();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NPingButton.MethodName.OnFocus) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnFocus();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NPingButton.MethodName.OnUnfocus) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnUnfocus();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NPingButton.MethodName.OnPress) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnPress();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NPingButton.MethodName.UpdateShaderV) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.UpdateShaderV(VariantUtils.ConvertTo<float>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NPingButton.MethodName.SetState) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.SetState(VariantUtils.ConvertTo<NPingButton.State>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NPingButton.MethodName.RefreshEnabled) || ((NativeVariantPtrArgs) ref args).Count != 0)
      return base.InvokeGodotClassMethod(in method, args, out ret);
    this.RefreshEnabled();
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NPingButton.MethodName._Ready) || StringName.op_Equality(ref method, NPingButton.MethodName._EnterTree) || StringName.op_Equality(ref method, NPingButton.MethodName._ExitTree) || StringName.op_Equality(ref method, NPingButton.MethodName.OnRelease) || StringName.op_Equality(ref method, NPingButton.MethodName.OnEnable) || StringName.op_Equality(ref method, NPingButton.MethodName.OnDisable) || StringName.op_Equality(ref method, NPingButton.MethodName.AnimOut) || StringName.op_Equality(ref method, NPingButton.MethodName.AnimIn) || StringName.op_Equality(ref method, NPingButton.MethodName.OnCombatEnded) || StringName.op_Equality(ref method, NPingButton.MethodName.OnFocus) || StringName.op_Equality(ref method, NPingButton.MethodName.OnUnfocus) || StringName.op_Equality(ref method, NPingButton.MethodName.OnPress) || StringName.op_Equality(ref method, NPingButton.MethodName.UpdateShaderV) || StringName.op_Equality(ref method, NPingButton.MethodName.SetState) || StringName.op_Equality(ref method, NPingButton.MethodName.RefreshEnabled) || base.HasGodotClassMethod(in method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NPingButton.PropertyName._state))
    {
      this._state = VariantUtils.ConvertTo<NPingButton.State>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NPingButton.PropertyName._visuals))
    {
      this._visuals = VariantUtils.ConvertTo<Control>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NPingButton.PropertyName._image))
    {
      this._image = VariantUtils.ConvertTo<TextureRect>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NPingButton.PropertyName._label))
    {
      this._label = VariantUtils.ConvertTo<MegaLabel>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NPingButton.PropertyName._viewport))
    {
      this._viewport = VariantUtils.ConvertTo<Viewport>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NPingButton.PropertyName._hsv))
    {
      this._hsv = VariantUtils.ConvertTo<ShaderMaterial>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NPingButton.PropertyName._positionTween))
    {
      this._positionTween = VariantUtils.ConvertTo<Tween>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NPingButton.PropertyName._hoverTween))
      return base.SetGodotClassPropertyValue(in name, in value);
    this._hoverTween = VariantUtils.ConvertTo<Tween>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NPingButton.PropertyName.ShowPos))
    {
      ref godot_variant local = ref value;
      Vector2 showPos = this.ShowPos;
      godot_variant from = VariantUtils.CreateFrom<Vector2>(ref showPos);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NPingButton.PropertyName.HidePos))
    {
      ref godot_variant local = ref value;
      Vector2 hidePos = this.HidePos;
      godot_variant from = VariantUtils.CreateFrom<Vector2>(ref hidePos);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NPingButton.PropertyName.Hotkeys))
    {
      ref godot_variant local = ref value;
      string[] hotkeys = this.Hotkeys;
      godot_variant from = VariantUtils.CreateFrom<string[]>(ref hotkeys);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NPingButton.PropertyName._state))
    {
      value = VariantUtils.CreateFrom<NPingButton.State>(ref this._state);
      return true;
    }
    if (StringName.op_Equality(ref name, NPingButton.PropertyName._visuals))
    {
      value = VariantUtils.CreateFrom<Control>(ref this._visuals);
      return true;
    }
    if (StringName.op_Equality(ref name, NPingButton.PropertyName._image))
    {
      value = VariantUtils.CreateFrom<TextureRect>(ref this._image);
      return true;
    }
    if (StringName.op_Equality(ref name, NPingButton.PropertyName._label))
    {
      value = VariantUtils.CreateFrom<MegaLabel>(ref this._label);
      return true;
    }
    if (StringName.op_Equality(ref name, NPingButton.PropertyName._viewport))
    {
      value = VariantUtils.CreateFrom<Viewport>(ref this._viewport);
      return true;
    }
    if (StringName.op_Equality(ref name, NPingButton.PropertyName._hsv))
    {
      value = VariantUtils.CreateFrom<ShaderMaterial>(ref this._hsv);
      return true;
    }
    if (StringName.op_Equality(ref name, NPingButton.PropertyName._positionTween))
    {
      value = VariantUtils.CreateFrom<Tween>(ref this._positionTween);
      return true;
    }
    if (!StringName.op_Equality(ref name, NPingButton.PropertyName._hoverTween))
      return base.GetGodotClassPropertyValue(in name, out value);
    value = VariantUtils.CreateFrom<Tween>(ref this._hoverTween);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 2L, NPingButton.PropertyName._state, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NPingButton.PropertyName._visuals, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NPingButton.PropertyName._image, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NPingButton.PropertyName._label, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NPingButton.PropertyName._viewport, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NPingButton.PropertyName._hsv, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 5L, NPingButton.PropertyName.ShowPos, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 5L, NPingButton.PropertyName.HidePos, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NPingButton.PropertyName._positionTween, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NPingButton.PropertyName._hoverTween, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 34L, NPingButton.PropertyName.Hotkeys, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    base.SaveGodotObjectData(info);
    info.AddProperty(NPingButton.PropertyName._state, Variant.From<NPingButton.State>(ref this._state));
    info.AddProperty(NPingButton.PropertyName._visuals, Variant.From<Control>(ref this._visuals));
    info.AddProperty(NPingButton.PropertyName._image, Variant.From<TextureRect>(ref this._image));
    info.AddProperty(NPingButton.PropertyName._label, Variant.From<MegaLabel>(ref this._label));
    info.AddProperty(NPingButton.PropertyName._viewport, Variant.From<Viewport>(ref this._viewport));
    info.AddProperty(NPingButton.PropertyName._hsv, Variant.From<ShaderMaterial>(ref this._hsv));
    info.AddProperty(NPingButton.PropertyName._positionTween, Variant.From<Tween>(ref this._positionTween));
    info.AddProperty(NPingButton.PropertyName._hoverTween, Variant.From<Tween>(ref this._hoverTween));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    base.RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NPingButton.PropertyName._state, ref variant1))
      this._state = ((Variant) ref variant1).As<NPingButton.State>();
    Variant variant2;
    if (info.TryGetProperty(NPingButton.PropertyName._visuals, ref variant2))
      this._visuals = ((Variant) ref variant2).As<Control>();
    Variant variant3;
    if (info.TryGetProperty(NPingButton.PropertyName._image, ref variant3))
      this._image = ((Variant) ref variant3).As<TextureRect>();
    Variant variant4;
    if (info.TryGetProperty(NPingButton.PropertyName._label, ref variant4))
      this._label = ((Variant) ref variant4).As<MegaLabel>();
    Variant variant5;
    if (info.TryGetProperty(NPingButton.PropertyName._viewport, ref variant5))
      this._viewport = ((Variant) ref variant5).As<Viewport>();
    Variant variant6;
    if (info.TryGetProperty(NPingButton.PropertyName._hsv, ref variant6))
      this._hsv = ((Variant) ref variant6).As<ShaderMaterial>();
    Variant variant7;
    if (info.TryGetProperty(NPingButton.PropertyName._positionTween, ref variant7))
      this._positionTween = ((Variant) ref variant7).As<Tween>();
    Variant variant8;
    if (!info.TryGetProperty(NPingButton.PropertyName._hoverTween, ref variant8))
      return;
    this._hoverTween = ((Variant) ref variant8).As<Tween>();
  }

  private enum State
  {
    Enabled,
    Disabled,
    Hidden,
  }

  public new class MethodName : NButton.MethodName
  {
    public new static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public new static readonly StringName _EnterTree = StringName.op_Implicit(nameof (_EnterTree));
    public new static readonly StringName _ExitTree = StringName.op_Implicit(nameof (_ExitTree));
    public new static readonly StringName OnRelease = StringName.op_Implicit(nameof (OnRelease));
    public new static readonly StringName OnEnable = StringName.op_Implicit(nameof (OnEnable));
    public new static readonly StringName OnDisable = StringName.op_Implicit(nameof (OnDisable));
    public static readonly StringName AnimOut = StringName.op_Implicit(nameof (AnimOut));
    public static readonly StringName AnimIn = StringName.op_Implicit(nameof (AnimIn));
    public static readonly StringName OnCombatEnded = StringName.op_Implicit(nameof (OnCombatEnded));
    public new static readonly StringName OnFocus = StringName.op_Implicit(nameof (OnFocus));
    public new static readonly StringName OnUnfocus = StringName.op_Implicit(nameof (OnUnfocus));
    public new static readonly StringName OnPress = StringName.op_Implicit(nameof (OnPress));
    public static readonly StringName UpdateShaderV = StringName.op_Implicit(nameof (UpdateShaderV));
    public static readonly StringName SetState = StringName.op_Implicit(nameof (SetState));
    public static readonly StringName RefreshEnabled = StringName.op_Implicit(nameof (RefreshEnabled));
  }

  public new class PropertyName : NButton.PropertyName
  {
    public static readonly StringName ShowPos = StringName.op_Implicit(nameof (ShowPos));
    public static readonly StringName HidePos = StringName.op_Implicit(nameof (HidePos));
    public new static readonly StringName Hotkeys = StringName.op_Implicit(nameof (Hotkeys));
    public static readonly StringName _state = StringName.op_Implicit(nameof (_state));
    public static readonly StringName _visuals = StringName.op_Implicit(nameof (_visuals));
    public static readonly StringName _image = StringName.op_Implicit(nameof (_image));
    public static readonly StringName _label = StringName.op_Implicit(nameof (_label));
    public static readonly StringName _viewport = StringName.op_Implicit(nameof (_viewport));
    public static readonly StringName _hsv = StringName.op_Implicit(nameof (_hsv));
    public static readonly StringName _positionTween = StringName.op_Implicit(nameof (_positionTween));
    public static readonly StringName _hoverTween = StringName.op_Implicit(nameof (_hoverTween));
  }

  public new class SignalName : NButton.SignalName
  {
  }
}
