// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Combat.NSelectionReticle
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Combat;

[ScriptPath("res://src/Core/Nodes/Combat/NSelectionReticle.cs")]
public class NSelectionReticle : Control
{
  private Tween? _currentTween;
  private readonly CancellationTokenSource _cancelToken = new CancellationTokenSource();

  public bool IsSelected { get; private set; }

  public override void _Ready()
  {
    ((CanvasItem) this).Modulate = Colors.Transparent;
    this.PivotOffset = Vector2.op_Multiply(this.Size, 0.5f);
  }

  public override void _ExitTree()
  {
    ((Node) this)._ExitTree();
    this._cancelToken.Cancel();
  }

  public void OnSelect()
  {
    if (NCombatUi.IsDebugHideTargetingUi)
      return;
    this._currentTween?.Kill();
    this._currentTween = ((Node) this).CreateTween().SetParallel(true);
    this._currentTween.TweenProperty((GodotObject) this, NodePath.op_Implicit("modulate:a"), Variant.op_Implicit(1f), 0.20000000298023224);
    this._currentTween.TweenProperty((GodotObject) this, NodePath.op_Implicit("scale"), Variant.op_Implicit(Vector2.One), 0.5).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 5L).From(Variant.op_Implicit(Vector2.op_Multiply(Vector2.One, 0.9f)));
    ((CanvasItem) this).Modulate = Colors.White;
    this.Scale = Vector2.One;
    this.IsSelected = true;
  }

  public void OnDeselect()
  {
    if (this._cancelToken.IsCancellationRequested)
      return;
    this._currentTween?.Kill();
    if (!((Node) this).IsValid() || !((Node) this).IsInsideTree())
      return;
    this._currentTween = ((Node) this).CreateTween()?.SetParallel(true);
    this._currentTween?.TweenProperty((GodotObject) this, NodePath.op_Implicit("modulate:a"), Variant.op_Implicit(0.0f), 0.20000000298023224).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 1L);
    this._currentTween?.TweenProperty((GodotObject) this, NodePath.op_Implicit("scale"), Variant.op_Implicit(Vector2.op_Multiply(Vector2.One, 1.05f)), 0.20000000298023224).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 1L);
    this.IsSelected = false;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(4)
    {
      new MethodInfo(NSelectionReticle.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NSelectionReticle.MethodName._ExitTree, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NSelectionReticle.MethodName.OnSelect, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NSelectionReticle.MethodName.OnDeselect, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NSelectionReticle.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NSelectionReticle.MethodName._ExitTree) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._ExitTree();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NSelectionReticle.MethodName.OnSelect) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnSelect();
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NSelectionReticle.MethodName.OnDeselect) || ((NativeVariantPtrArgs) ref args).Count != 0)
      return base.InvokeGodotClassMethod(ref method, args, ref ret);
    this.OnDeselect();
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NSelectionReticle.MethodName._Ready) || StringName.op_Equality(ref method, NSelectionReticle.MethodName._ExitTree) || StringName.op_Equality(ref method, NSelectionReticle.MethodName.OnSelect) || StringName.op_Equality(ref method, NSelectionReticle.MethodName.OnDeselect) || base.HasGodotClassMethod(ref method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NSelectionReticle.PropertyName.IsSelected))
    {
      this.IsSelected = VariantUtils.ConvertTo<bool>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NSelectionReticle.PropertyName._currentTween))
      return ((GodotObject) this).SetGodotClassPropertyValue(ref name, ref value);
    this._currentTween = VariantUtils.ConvertTo<Tween>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NSelectionReticle.PropertyName.IsSelected))
    {
      ref godot_variant local = ref value;
      bool isSelected = this.IsSelected;
      godot_variant from = VariantUtils.CreateFrom<bool>(ref isSelected);
      local = from;
      return true;
    }
    if (!StringName.op_Equality(ref name, NSelectionReticle.PropertyName._currentTween))
      return ((GodotObject) this).GetGodotClassPropertyValue(ref name, ref value);
    value = VariantUtils.CreateFrom<Tween>(ref this._currentTween);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NSelectionReticle.PropertyName._currentTween, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 1L, NSelectionReticle.PropertyName.IsSelected, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).SaveGodotObjectData(info);
    GodotSerializationInfo serializationInfo = info;
    StringName isSelected1 = NSelectionReticle.PropertyName.IsSelected;
    bool isSelected2 = this.IsSelected;
    Variant variant = Variant.From<bool>(ref isSelected2);
    serializationInfo.AddProperty(isSelected1, variant);
    info.AddProperty(NSelectionReticle.PropertyName._currentTween, Variant.From<Tween>(ref this._currentTween));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NSelectionReticle.PropertyName.IsSelected, ref variant1))
      this.IsSelected = ((Variant) ref variant1).As<bool>();
    Variant variant2;
    if (!info.TryGetProperty(NSelectionReticle.PropertyName._currentTween, ref variant2))
      return;
    this._currentTween = ((Variant) ref variant2).As<Tween>();
  }

  public class MethodName : Control.MethodName
  {
    public static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName _ExitTree = StringName.op_Implicit(nameof (_ExitTree));
    public static readonly StringName OnSelect = StringName.op_Implicit(nameof (OnSelect));
    public static readonly StringName OnDeselect = StringName.op_Implicit(nameof (OnDeselect));
  }

  public class PropertyName : Control.PropertyName
  {
    public static readonly StringName IsSelected = StringName.op_Implicit(nameof (IsSelected));
    public static readonly StringName _currentTween = StringName.op_Implicit(nameof (_currentTween));
  }

  public class SignalName : Control.SignalName
  {
  }
}
