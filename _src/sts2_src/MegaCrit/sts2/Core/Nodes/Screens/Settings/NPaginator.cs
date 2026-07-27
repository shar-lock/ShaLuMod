// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Screens.Settings.NPaginator
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.ControllerInput;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using System;
using System.Collections.Generic;
using System.ComponentModel;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Screens.Settings;

[ScriptPath("res://src/Core/Nodes/Screens/Settings/NPaginator.cs")]
public class NPaginator : Control
{
  protected MegaLabel _label;
  private MegaLabel _vfxLabel;
  private NSelectionReticle _selectionReticle;
  protected readonly List<string> _options = new List<string>();
  protected int _currentIndex;
  private Tween? _tween;
  private const double _animDuration = 0.25;
  private const float _animDistance = 90f;

  public override void _Ready()
  {
    if (((object) this).GetType() != typeof (NPaginator))
      throw new InvalidOperationException("Don't call base._Ready(). Use ConnectSignals() instead");
    this.ConnectSignals();
  }

  protected void ConnectSignals()
  {
    this._label = ((Node) this).GetNode<MegaLabel>(NodePath.op_Implicit("%Label"));
    this._vfxLabel = ((Node) this).GetNode<MegaLabel>(NodePath.op_Implicit("%VfxLabel"));
    this._selectionReticle = ((Node) this).GetNode<NSelectionReticle>(NodePath.op_Implicit("SelectionReticle"));
    ((GodotObject) this).Connect(Control.SignalName.FocusEntered, Callable.From(new Action(this.OnFocus)), 0U);
    ((GodotObject) this).Connect(Control.SignalName.FocusExited, Callable.From(new Action(this.OnUnfocus)), 0U);
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

  protected virtual void OnIndexChanged(int index)
  {
  }

  public void SetIndex(int index)
  {
    if (this._currentIndex == index)
      return;
    this._currentIndex = Mathf.Clamp(index, 0, this._options.Count - 1);
    this.OnIndexChanged(this._currentIndex);
  }

  public void PageLeft()
  {
    --this._currentIndex;
    if (this._currentIndex < 0)
      this._currentIndex = this._options.Count - 1;
    this.IndexChangeHelper(true);
  }

  public void PageRight()
  {
    ++this._currentIndex;
    if (this._currentIndex > this._options.Count - 1)
      this._currentIndex = 0;
    this.IndexChangeHelper(false);
  }

  private void IndexChangeHelper(bool pagedLeft)
  {
    this._vfxLabel.SetTextAutoSize(this._label.Text);
    ((CanvasItem) this._vfxLabel).Modulate = ((CanvasItem) this._label).Modulate;
    this.OnIndexChanged(this._currentIndex);
    this._tween?.Kill();
    this._tween = ((Node) this).CreateTween().SetParallel(true);
    this._tween.TweenProperty((GodotObject) this._label, NodePath.op_Implicit("position:x"), Variant.op_Implicit(0.0f), 0.25).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 7L).From(Variant.op_Implicit(pagedLeft ? -90f : 90f));
    this._tween.TweenProperty((GodotObject) this._label, NodePath.op_Implicit("modulate:a"), Variant.op_Implicit(1f), 0.25).From(Variant.op_Implicit(0.75f));
    this._tween.TweenProperty((GodotObject) this._vfxLabel, NodePath.op_Implicit("position:x"), Variant.op_Implicit(pagedLeft ? 90f : -90f), 0.25).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 7L).From(Variant.op_Implicit(0.0f));
    this._tween.TweenProperty((GodotObject) this._vfxLabel, NodePath.op_Implicit("modulate"), Variant.op_Implicit(StsColors.transparentBlack), 0.25);
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
      new MethodInfo(NPaginator.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NPaginator.MethodName.ConnectSignals, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NPaginator.MethodName._GuiInput, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("input"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("InputEvent"), false)
      }, (List<Variant>) null),
      new MethodInfo(NPaginator.MethodName.OnIndexChanged, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 2L, StringName.op_Implicit("index"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NPaginator.MethodName.SetIndex, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 2L, StringName.op_Implicit("index"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NPaginator.MethodName.PageLeft, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NPaginator.MethodName.PageRight, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NPaginator.MethodName.IndexChangeHelper, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 1L, StringName.op_Implicit("pagedLeft"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NPaginator.MethodName.OnFocus, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NPaginator.MethodName.OnUnfocus, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NPaginator.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NPaginator.MethodName.ConnectSignals) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.ConnectSignals();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NPaginator.MethodName._GuiInput) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      base._GuiInput(VariantUtils.ConvertTo<InputEvent>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NPaginator.MethodName.OnIndexChanged) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.OnIndexChanged(VariantUtils.ConvertTo<int>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NPaginator.MethodName.SetIndex) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.SetIndex(VariantUtils.ConvertTo<int>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NPaginator.MethodName.PageLeft) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.PageLeft();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NPaginator.MethodName.PageRight) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.PageRight();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NPaginator.MethodName.IndexChangeHelper) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.IndexChangeHelper(VariantUtils.ConvertTo<bool>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NPaginator.MethodName.OnFocus) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnFocus();
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NPaginator.MethodName.OnUnfocus) || ((NativeVariantPtrArgs) ref args).Count != 0)
      return base.InvokeGodotClassMethod(ref method, args, ref ret);
    this.OnUnfocus();
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NPaginator.MethodName._Ready) || StringName.op_Equality(ref method, NPaginator.MethodName.ConnectSignals) || StringName.op_Equality(ref method, NPaginator.MethodName._GuiInput) || StringName.op_Equality(ref method, NPaginator.MethodName.OnIndexChanged) || StringName.op_Equality(ref method, NPaginator.MethodName.SetIndex) || StringName.op_Equality(ref method, NPaginator.MethodName.PageLeft) || StringName.op_Equality(ref method, NPaginator.MethodName.PageRight) || StringName.op_Equality(ref method, NPaginator.MethodName.IndexChangeHelper) || StringName.op_Equality(ref method, NPaginator.MethodName.OnFocus) || StringName.op_Equality(ref method, NPaginator.MethodName.OnUnfocus) || base.HasGodotClassMethod(ref method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NPaginator.PropertyName._label))
    {
      this._label = VariantUtils.ConvertTo<MegaLabel>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NPaginator.PropertyName._vfxLabel))
    {
      this._vfxLabel = VariantUtils.ConvertTo<MegaLabel>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NPaginator.PropertyName._selectionReticle))
    {
      this._selectionReticle = VariantUtils.ConvertTo<NSelectionReticle>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NPaginator.PropertyName._currentIndex))
    {
      this._currentIndex = VariantUtils.ConvertTo<int>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NPaginator.PropertyName._tween))
      return ((GodotObject) this).SetGodotClassPropertyValue(ref name, ref value);
    this._tween = VariantUtils.ConvertTo<Tween>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NPaginator.PropertyName._label))
    {
      value = VariantUtils.CreateFrom<MegaLabel>(ref this._label);
      return true;
    }
    if (StringName.op_Equality(ref name, NPaginator.PropertyName._vfxLabel))
    {
      value = VariantUtils.CreateFrom<MegaLabel>(ref this._vfxLabel);
      return true;
    }
    if (StringName.op_Equality(ref name, NPaginator.PropertyName._selectionReticle))
    {
      value = VariantUtils.CreateFrom<NSelectionReticle>(ref this._selectionReticle);
      return true;
    }
    if (StringName.op_Equality(ref name, NPaginator.PropertyName._currentIndex))
    {
      value = VariantUtils.CreateFrom<int>(ref this._currentIndex);
      return true;
    }
    if (!StringName.op_Equality(ref name, NPaginator.PropertyName._tween))
      return ((GodotObject) this).GetGodotClassPropertyValue(ref name, ref value);
    value = VariantUtils.CreateFrom<Tween>(ref this._tween);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NPaginator.PropertyName._label, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NPaginator.PropertyName._vfxLabel, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NPaginator.PropertyName._selectionReticle, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 2L, NPaginator.PropertyName._currentIndex, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NPaginator.PropertyName._tween, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).SaveGodotObjectData(info);
    info.AddProperty(NPaginator.PropertyName._label, Variant.From<MegaLabel>(ref this._label));
    info.AddProperty(NPaginator.PropertyName._vfxLabel, Variant.From<MegaLabel>(ref this._vfxLabel));
    info.AddProperty(NPaginator.PropertyName._selectionReticle, Variant.From<NSelectionReticle>(ref this._selectionReticle));
    info.AddProperty(NPaginator.PropertyName._currentIndex, Variant.From<int>(ref this._currentIndex));
    info.AddProperty(NPaginator.PropertyName._tween, Variant.From<Tween>(ref this._tween));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NPaginator.PropertyName._label, ref variant1))
      this._label = ((Variant) ref variant1).As<MegaLabel>();
    Variant variant2;
    if (info.TryGetProperty(NPaginator.PropertyName._vfxLabel, ref variant2))
      this._vfxLabel = ((Variant) ref variant2).As<MegaLabel>();
    Variant variant3;
    if (info.TryGetProperty(NPaginator.PropertyName._selectionReticle, ref variant3))
      this._selectionReticle = ((Variant) ref variant3).As<NSelectionReticle>();
    Variant variant4;
    if (info.TryGetProperty(NPaginator.PropertyName._currentIndex, ref variant4))
      this._currentIndex = ((Variant) ref variant4).As<int>();
    Variant variant5;
    if (!info.TryGetProperty(NPaginator.PropertyName._tween, ref variant5))
      return;
    this._tween = ((Variant) ref variant5).As<Tween>();
  }

  public class MethodName : Control.MethodName
  {
    public static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName ConnectSignals = StringName.op_Implicit(nameof (ConnectSignals));
    public static readonly StringName _GuiInput = StringName.op_Implicit(nameof (_GuiInput));
    public static readonly StringName OnIndexChanged = StringName.op_Implicit(nameof (OnIndexChanged));
    public static readonly StringName SetIndex = StringName.op_Implicit(nameof (SetIndex));
    public static readonly StringName PageLeft = StringName.op_Implicit(nameof (PageLeft));
    public static readonly StringName PageRight = StringName.op_Implicit(nameof (PageRight));
    public static readonly StringName IndexChangeHelper = StringName.op_Implicit(nameof (IndexChangeHelper));
    public static readonly StringName OnFocus = StringName.op_Implicit(nameof (OnFocus));
    public static readonly StringName OnUnfocus = StringName.op_Implicit(nameof (OnUnfocus));
  }

  public class PropertyName : Control.PropertyName
  {
    public static readonly StringName _label = StringName.op_Implicit(nameof (_label));
    public static readonly StringName _vfxLabel = StringName.op_Implicit(nameof (_vfxLabel));
    public static readonly StringName _selectionReticle = StringName.op_Implicit(nameof (_selectionReticle));
    public static readonly StringName _currentIndex = StringName.op_Implicit(nameof (_currentIndex));
    public static readonly StringName _tween = StringName.op_Implicit(nameof (_tween));
  }

  public class SignalName : Control.SignalName
  {
  }
}
