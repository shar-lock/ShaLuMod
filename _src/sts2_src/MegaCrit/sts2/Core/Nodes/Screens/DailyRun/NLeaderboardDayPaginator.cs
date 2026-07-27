// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Screens.DailyRun.NLeaderboardDayPaginator
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.ControllerInput;
using MegaCrit.Sts2.Core.Daily;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using System;
using System.Collections.Generic;
using System.ComponentModel;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Screens.DailyRun;

[ScriptPath("res://src/Core/Nodes/Screens/DailyRun/NLeaderboardDayPaginator.cs")]
public class NLeaderboardDayPaginator : Control
{
  protected MegaLabel _label;
  private MegaLabel _vfxLabel;
  private NLeaderboardPageArrow _leftArrow;
  private NLeaderboardPageArrow _rightArrow;
  private NSelectionReticle _selectionReticle;
  private Tween? _tween;
  private const double _animDuration = 0.25;
  private const float _animDistance = 90f;
  private DateTimeOffset _currentDay;
  private NDailyRunLeaderboard? _leaderboard;

  public override void _Ready()
  {
    this._label = ((Node) this).GetNode<MegaLabel>(NodePath.op_Implicit("LabelContainer/Mask/Label"));
    this._vfxLabel = ((Node) this).GetNode<MegaLabel>(NodePath.op_Implicit("LabelContainer/Mask/VfxLabel"));
    this._selectionReticle = ((Node) this).GetNode<NSelectionReticle>(NodePath.op_Implicit("SelectionReticle"));
    this._leftArrow = ((Node) this).GetNode<NLeaderboardPageArrow>(NodePath.op_Implicit("LeftArrow"));
    this._rightArrow = ((Node) this).GetNode<NLeaderboardPageArrow>(NodePath.op_Implicit("RightArrow"));
    ((GodotObject) this).Connect(Control.SignalName.FocusEntered, Callable.From(new Action(this.OnFocus)), 0U);
    ((GodotObject) this).Connect(Control.SignalName.FocusExited, Callable.From(new Action(this.OnUnfocus)), 0U);
    this._leftArrow.Connect(new Action(this.PageLeft));
    this._rightArrow.Connect(new Action(this.PageRight));
  }

  public void Initialize(
    NDailyRunLeaderboard leaderboard,
    DateTimeOffset dateTime,
    bool showArrows)
  {
    this._currentDay = dateTime;
    this._leaderboard = leaderboard;
    this.OnDayChanged(false);
    ((CanvasItem) this._leftArrow).Visible = showArrows;
    ((CanvasItem) this._rightArrow).Visible = showArrows;
  }

  public override void _GuiInput(InputEvent input)
  {
    base._GuiInput(input);
    if (input.IsActionPressed(MegaInput.left, false, false))
      this.PageLeft();
    if (!input.IsActionPressed(MegaInput.right, false, false))
      return;
    this.PageRight();
  }

  private void PageLeft()
  {
    DateTimeOffset? nullable = DailyRunUtility.AddLeaderboardDays(this._currentDay, -1);
    if (!nullable.HasValue)
      return;
    this._currentDay = nullable.GetValueOrDefault();
    this.DayChangeHelper(true);
  }

  private void PageRight()
  {
    DateTimeOffset? nullable = DailyRunUtility.AddLeaderboardDays(this._currentDay, 1);
    if (!nullable.HasValue)
      return;
    this._currentDay = nullable.GetValueOrDefault();
    this.DayChangeHelper(false);
  }

  private void DayChangeHelper(bool pagedLeft)
  {
    this._vfxLabel.SetTextAutoSize(this._label.Text);
    ((CanvasItem) this._vfxLabel).Modulate = ((CanvasItem) this._label).Modulate;
    this.OnDayChanged(true);
    this._tween?.Kill();
    this._tween = ((Node) this).CreateTween().SetParallel(true);
    this._tween.TweenProperty((GodotObject) this._label, NodePath.op_Implicit("position:x"), Variant.op_Implicit(0.0f), 0.25).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 7L).From(Variant.op_Implicit(pagedLeft ? -90f : 90f));
    this._tween.TweenProperty((GodotObject) this._label, NodePath.op_Implicit("modulate:a"), Variant.op_Implicit(1f), 0.25).From(Variant.op_Implicit(0.75f));
    this._tween.TweenProperty((GodotObject) this._vfxLabel, NodePath.op_Implicit("position:x"), Variant.op_Implicit(pagedLeft ? 90f : -90f), 0.25).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 7L).From(Variant.op_Implicit(0.0f));
    this._tween.TweenProperty((GodotObject) this._vfxLabel, NodePath.op_Implicit("modulate"), Variant.op_Implicit(StsColors.transparentBlack), 0.25);
  }

  private void OnDayChanged(bool changeLeaderboardDay)
  {
    this._label.SetTextAutoSize(this._currentDay.ToString(NDailyRunScreen.dateFormat));
    if (!changeLeaderboardDay)
      return;
    this._leaderboard.SetDay(this._currentDay);
  }

  public void Disable()
  {
    this._leftArrow.Disable();
    this._rightArrow.Disable();
  }

  public void Enable(bool leftArrowEnabled, bool rightArrowEnabled)
  {
    if (leftArrowEnabled)
      this._leftArrow.Enable();
    if (!rightArrowEnabled)
      return;
    this._rightArrow.Enable();
  }

  private void OnFocus()
  {
    if (!NControllerManager.Instance.IsUsingController)
      return;
    this._selectionReticle.OnSelect();
  }

  private void OnUnfocus() => this._selectionReticle.OnDeselect();

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(10)
    {
      new MethodInfo(NLeaderboardDayPaginator.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NLeaderboardDayPaginator.MethodName._GuiInput, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("input"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("InputEvent"), false)
      }, (List<Variant>) null),
      new MethodInfo(NLeaderboardDayPaginator.MethodName.PageLeft, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NLeaderboardDayPaginator.MethodName.PageRight, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NLeaderboardDayPaginator.MethodName.DayChangeHelper, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 1L, StringName.op_Implicit("pagedLeft"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NLeaderboardDayPaginator.MethodName.OnDayChanged, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 1L, StringName.op_Implicit("changeLeaderboardDay"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NLeaderboardDayPaginator.MethodName.Disable, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NLeaderboardDayPaginator.MethodName.Enable, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 1L, StringName.op_Implicit("leftArrowEnabled"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false),
        new PropertyInfo((Variant.Type) 1L, StringName.op_Implicit("rightArrowEnabled"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NLeaderboardDayPaginator.MethodName.OnFocus, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NLeaderboardDayPaginator.MethodName.OnUnfocus, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NLeaderboardDayPaginator.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NLeaderboardDayPaginator.MethodName._GuiInput) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      base._GuiInput(VariantUtils.ConvertTo<InputEvent>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NLeaderboardDayPaginator.MethodName.PageLeft) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.PageLeft();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NLeaderboardDayPaginator.MethodName.PageRight) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.PageRight();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NLeaderboardDayPaginator.MethodName.DayChangeHelper) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.DayChangeHelper(VariantUtils.ConvertTo<bool>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NLeaderboardDayPaginator.MethodName.OnDayChanged) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.OnDayChanged(VariantUtils.ConvertTo<bool>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NLeaderboardDayPaginator.MethodName.Disable) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.Disable();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NLeaderboardDayPaginator.MethodName.Enable) && ((NativeVariantPtrArgs) ref args).Count == 2)
    {
      this.Enable(VariantUtils.ConvertTo<bool>(ref ((NativeVariantPtrArgs) ref args)[0]), VariantUtils.ConvertTo<bool>(ref ((NativeVariantPtrArgs) ref args)[1]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NLeaderboardDayPaginator.MethodName.OnFocus) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnFocus();
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NLeaderboardDayPaginator.MethodName.OnUnfocus) || ((NativeVariantPtrArgs) ref args).Count != 0)
      return base.InvokeGodotClassMethod(ref method, args, ref ret);
    this.OnUnfocus();
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NLeaderboardDayPaginator.MethodName._Ready) || StringName.op_Equality(ref method, NLeaderboardDayPaginator.MethodName._GuiInput) || StringName.op_Equality(ref method, NLeaderboardDayPaginator.MethodName.PageLeft) || StringName.op_Equality(ref method, NLeaderboardDayPaginator.MethodName.PageRight) || StringName.op_Equality(ref method, NLeaderboardDayPaginator.MethodName.DayChangeHelper) || StringName.op_Equality(ref method, NLeaderboardDayPaginator.MethodName.OnDayChanged) || StringName.op_Equality(ref method, NLeaderboardDayPaginator.MethodName.Disable) || StringName.op_Equality(ref method, NLeaderboardDayPaginator.MethodName.Enable) || StringName.op_Equality(ref method, NLeaderboardDayPaginator.MethodName.OnFocus) || StringName.op_Equality(ref method, NLeaderboardDayPaginator.MethodName.OnUnfocus) || base.HasGodotClassMethod(ref method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NLeaderboardDayPaginator.PropertyName._label))
    {
      this._label = VariantUtils.ConvertTo<MegaLabel>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NLeaderboardDayPaginator.PropertyName._vfxLabel))
    {
      this._vfxLabel = VariantUtils.ConvertTo<MegaLabel>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NLeaderboardDayPaginator.PropertyName._leftArrow))
    {
      this._leftArrow = VariantUtils.ConvertTo<NLeaderboardPageArrow>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NLeaderboardDayPaginator.PropertyName._rightArrow))
    {
      this._rightArrow = VariantUtils.ConvertTo<NLeaderboardPageArrow>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NLeaderboardDayPaginator.PropertyName._selectionReticle))
    {
      this._selectionReticle = VariantUtils.ConvertTo<NSelectionReticle>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NLeaderboardDayPaginator.PropertyName._tween))
    {
      this._tween = VariantUtils.ConvertTo<Tween>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NLeaderboardDayPaginator.PropertyName._leaderboard))
      return ((GodotObject) this).SetGodotClassPropertyValue(ref name, ref value);
    this._leaderboard = VariantUtils.ConvertTo<NDailyRunLeaderboard>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NLeaderboardDayPaginator.PropertyName._label))
    {
      value = VariantUtils.CreateFrom<MegaLabel>(ref this._label);
      return true;
    }
    if (StringName.op_Equality(ref name, NLeaderboardDayPaginator.PropertyName._vfxLabel))
    {
      value = VariantUtils.CreateFrom<MegaLabel>(ref this._vfxLabel);
      return true;
    }
    if (StringName.op_Equality(ref name, NLeaderboardDayPaginator.PropertyName._leftArrow))
    {
      value = VariantUtils.CreateFrom<NLeaderboardPageArrow>(ref this._leftArrow);
      return true;
    }
    if (StringName.op_Equality(ref name, NLeaderboardDayPaginator.PropertyName._rightArrow))
    {
      value = VariantUtils.CreateFrom<NLeaderboardPageArrow>(ref this._rightArrow);
      return true;
    }
    if (StringName.op_Equality(ref name, NLeaderboardDayPaginator.PropertyName._selectionReticle))
    {
      value = VariantUtils.CreateFrom<NSelectionReticle>(ref this._selectionReticle);
      return true;
    }
    if (StringName.op_Equality(ref name, NLeaderboardDayPaginator.PropertyName._tween))
    {
      value = VariantUtils.CreateFrom<Tween>(ref this._tween);
      return true;
    }
    if (!StringName.op_Equality(ref name, NLeaderboardDayPaginator.PropertyName._leaderboard))
      return ((GodotObject) this).GetGodotClassPropertyValue(ref name, ref value);
    value = VariantUtils.CreateFrom<NDailyRunLeaderboard>(ref this._leaderboard);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NLeaderboardDayPaginator.PropertyName._label, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NLeaderboardDayPaginator.PropertyName._vfxLabel, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NLeaderboardDayPaginator.PropertyName._leftArrow, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NLeaderboardDayPaginator.PropertyName._rightArrow, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NLeaderboardDayPaginator.PropertyName._selectionReticle, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NLeaderboardDayPaginator.PropertyName._tween, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NLeaderboardDayPaginator.PropertyName._leaderboard, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).SaveGodotObjectData(info);
    info.AddProperty(NLeaderboardDayPaginator.PropertyName._label, Variant.From<MegaLabel>(ref this._label));
    info.AddProperty(NLeaderboardDayPaginator.PropertyName._vfxLabel, Variant.From<MegaLabel>(ref this._vfxLabel));
    info.AddProperty(NLeaderboardDayPaginator.PropertyName._leftArrow, Variant.From<NLeaderboardPageArrow>(ref this._leftArrow));
    info.AddProperty(NLeaderboardDayPaginator.PropertyName._rightArrow, Variant.From<NLeaderboardPageArrow>(ref this._rightArrow));
    info.AddProperty(NLeaderboardDayPaginator.PropertyName._selectionReticle, Variant.From<NSelectionReticle>(ref this._selectionReticle));
    info.AddProperty(NLeaderboardDayPaginator.PropertyName._tween, Variant.From<Tween>(ref this._tween));
    info.AddProperty(NLeaderboardDayPaginator.PropertyName._leaderboard, Variant.From<NDailyRunLeaderboard>(ref this._leaderboard));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NLeaderboardDayPaginator.PropertyName._label, ref variant1))
      this._label = ((Variant) ref variant1).As<MegaLabel>();
    Variant variant2;
    if (info.TryGetProperty(NLeaderboardDayPaginator.PropertyName._vfxLabel, ref variant2))
      this._vfxLabel = ((Variant) ref variant2).As<MegaLabel>();
    Variant variant3;
    if (info.TryGetProperty(NLeaderboardDayPaginator.PropertyName._leftArrow, ref variant3))
      this._leftArrow = ((Variant) ref variant3).As<NLeaderboardPageArrow>();
    Variant variant4;
    if (info.TryGetProperty(NLeaderboardDayPaginator.PropertyName._rightArrow, ref variant4))
      this._rightArrow = ((Variant) ref variant4).As<NLeaderboardPageArrow>();
    Variant variant5;
    if (info.TryGetProperty(NLeaderboardDayPaginator.PropertyName._selectionReticle, ref variant5))
      this._selectionReticle = ((Variant) ref variant5).As<NSelectionReticle>();
    Variant variant6;
    if (info.TryGetProperty(NLeaderboardDayPaginator.PropertyName._tween, ref variant6))
      this._tween = ((Variant) ref variant6).As<Tween>();
    Variant variant7;
    if (!info.TryGetProperty(NLeaderboardDayPaginator.PropertyName._leaderboard, ref variant7))
      return;
    this._leaderboard = ((Variant) ref variant7).As<NDailyRunLeaderboard>();
  }

  public class MethodName : Control.MethodName
  {
    public static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName _GuiInput = StringName.op_Implicit(nameof (_GuiInput));
    public static readonly StringName PageLeft = StringName.op_Implicit(nameof (PageLeft));
    public static readonly StringName PageRight = StringName.op_Implicit(nameof (PageRight));
    public static readonly StringName DayChangeHelper = StringName.op_Implicit(nameof (DayChangeHelper));
    public static readonly StringName OnDayChanged = StringName.op_Implicit(nameof (OnDayChanged));
    public static readonly StringName Disable = StringName.op_Implicit(nameof (Disable));
    public static readonly StringName Enable = StringName.op_Implicit(nameof (Enable));
    public static readonly StringName OnFocus = StringName.op_Implicit(nameof (OnFocus));
    public static readonly StringName OnUnfocus = StringName.op_Implicit(nameof (OnUnfocus));
  }

  public class PropertyName : Control.PropertyName
  {
    public static readonly StringName _label = StringName.op_Implicit(nameof (_label));
    public static readonly StringName _vfxLabel = StringName.op_Implicit(nameof (_vfxLabel));
    public static readonly StringName _leftArrow = StringName.op_Implicit(nameof (_leftArrow));
    public static readonly StringName _rightArrow = StringName.op_Implicit(nameof (_rightArrow));
    public static readonly StringName _selectionReticle = StringName.op_Implicit(nameof (_selectionReticle));
    public static readonly StringName _tween = StringName.op_Implicit(nameof (_tween));
    public static readonly StringName _leaderboard = StringName.op_Implicit(nameof (_leaderboard));
  }

  public class SignalName : Control.SignalName
  {
  }
}
