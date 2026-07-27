// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Vfx.NBasicTrail
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using System.Collections.Generic;
using System.ComponentModel;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Vfx;

[ScriptPath("res://src/Core/Nodes/Vfx/NBasicTrail.cs")]
public class NBasicTrail : Line2D
{
  private Node2D _target;
  [Export]
  private int _maxSegments = 10;

  public override void _Ready() => this._target = ((Node) this).GetParent<Node2D>();

  public override void _Process(double delta)
  {
    ((Node2D) this).GlobalPosition = Vector2.Zero;
    ((Node2D) this).GlobalRotation = 0.0f;
    ((Node2D) this).GlobalScale = Vector2.One;
    this.AddPoint(this._target.GlobalPosition, -1);
    if (this.Points.Length <= this._maxSegments)
      return;
    this.RemovePoint(0);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(2)
    {
      new MethodInfo(NBasicTrail.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NBasicTrail.MethodName._Process, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 3L, StringName.op_Implicit("delta"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NBasicTrail.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NBasicTrail.MethodName._Process) || ((NativeVariantPtrArgs) ref args).Count != 1)
      return base.InvokeGodotClassMethod(ref method, args, ref ret);
    ((Node) this)._Process(VariantUtils.ConvertTo<double>(ref ((NativeVariantPtrArgs) ref args)[0]));
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NBasicTrail.MethodName._Ready) || StringName.op_Equality(ref method, NBasicTrail.MethodName._Process) || base.HasGodotClassMethod(ref method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NBasicTrail.PropertyName._target))
    {
      this._target = VariantUtils.ConvertTo<Node2D>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NBasicTrail.PropertyName._maxSegments))
      return ((GodotObject) this).SetGodotClassPropertyValue(ref name, ref value);
    this._maxSegments = VariantUtils.ConvertTo<int>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NBasicTrail.PropertyName._target))
    {
      value = VariantUtils.CreateFrom<Node2D>(ref this._target);
      return true;
    }
    if (!StringName.op_Equality(ref name, NBasicTrail.PropertyName._maxSegments))
      return ((GodotObject) this).GetGodotClassPropertyValue(ref name, ref value);
    value = VariantUtils.CreateFrom<int>(ref this._maxSegments);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NBasicTrail.PropertyName._target, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 2L, NBasicTrail.PropertyName._maxSegments, (PropertyHint) 0L, "", (PropertyUsageFlags) 4102L, true)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).SaveGodotObjectData(info);
    info.AddProperty(NBasicTrail.PropertyName._target, Variant.From<Node2D>(ref this._target));
    info.AddProperty(NBasicTrail.PropertyName._maxSegments, Variant.From<int>(ref this._maxSegments));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NBasicTrail.PropertyName._target, ref variant1))
      this._target = ((Variant) ref variant1).As<Node2D>();
    Variant variant2;
    if (!info.TryGetProperty(NBasicTrail.PropertyName._maxSegments, ref variant2))
      return;
    this._maxSegments = ((Variant) ref variant2).As<int>();
  }

  public class MethodName : Line2D.MethodName
  {
    public static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName _Process = StringName.op_Implicit(nameof (_Process));
  }

  public class PropertyName : Line2D.PropertyName
  {
    public static readonly StringName _target = StringName.op_Implicit(nameof (_target));
    public static readonly StringName _maxSegments = StringName.op_Implicit(nameof (_maxSegments));
  }

  public class SignalName : Line2D.SignalName
  {
  }
}
