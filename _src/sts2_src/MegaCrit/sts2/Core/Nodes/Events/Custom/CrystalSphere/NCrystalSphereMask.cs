// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Events.Custom.CrystalSphere.NCrystalSphereMask
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Events.Custom.CrystalSphereEvent;
using System.Collections.Generic;
using System.ComponentModel;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Events.Custom.CrystalSphere;

[ScriptPath("res://src/Core/Nodes/Events/Custom/CrystalSphere/NCrystalSphereMask.cs")]
public class NCrystalSphereMask : Control
{
  private static readonly StringName _gridFadeParams = new StringName("gridFadeParams");
  private static readonly StringName _timeStr = new StringName("time");
  private ShaderMaterial _material;
  private Array<Vector3> _values = new Array<Vector3>();
  private float _time;

  public override void _Ready()
  {
    this._material = (ShaderMaterial) ((CanvasItem) this).Material;
    for (int index = 0; index < 121; ++index)
      this._values.Add(Vector3.Zero);
    this._values[3] = new Vector3(1f, 0.0f, 0.0f);
  }

  public override void _Process(double delta)
  {
    this._time += (float) delta;
    this._material.SetShaderParameter(NCrystalSphereMask._timeStr, Variant.op_Implicit(this._time));
  }

  public void UpdateMat(CrystalSphereCell cell)
  {
    int num1 = cell.Y * 11 + cell.X;
    float num2 = this._values[num1].Y;
    if ((double) this._values[num1].Z == 0.0)
      num2 = 1f;
    this._values[num1] = new Vector3(num2, (float) (cell.IsHidden ? 1 : 0), this._time);
    this._material.SetShaderParameter(NCrystalSphereMask._gridFadeParams, Array<Vector3>.op_Implicit(this._values));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(2)
    {
      new MethodInfo(NCrystalSphereMask.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCrystalSphereMask.MethodName._Process, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
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
    if (StringName.op_Equality(ref method, NCrystalSphereMask.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NCrystalSphereMask.MethodName._Process) || ((NativeVariantPtrArgs) ref args).Count != 1)
      return base.InvokeGodotClassMethod(ref method, args, ref ret);
    ((Node) this)._Process(VariantUtils.ConvertTo<double>(ref ((NativeVariantPtrArgs) ref args)[0]));
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NCrystalSphereMask.MethodName._Ready) || StringName.op_Equality(ref method, NCrystalSphereMask.MethodName._Process) || base.HasGodotClassMethod(ref method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NCrystalSphereMask.PropertyName._material))
    {
      this._material = VariantUtils.ConvertTo<ShaderMaterial>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCrystalSphereMask.PropertyName._values))
    {
      this._values = VariantUtils.ConvertToArray<Vector3>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NCrystalSphereMask.PropertyName._time))
      return ((GodotObject) this).SetGodotClassPropertyValue(ref name, ref value);
    this._time = VariantUtils.ConvertTo<float>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NCrystalSphereMask.PropertyName._material))
    {
      value = VariantUtils.CreateFrom<ShaderMaterial>(ref this._material);
      return true;
    }
    if (StringName.op_Equality(ref name, NCrystalSphereMask.PropertyName._values))
    {
      value = VariantUtils.CreateFromArray<Vector3>(this._values);
      return true;
    }
    if (!StringName.op_Equality(ref name, NCrystalSphereMask.PropertyName._time))
      return ((GodotObject) this).GetGodotClassPropertyValue(ref name, ref value);
    value = VariantUtils.CreateFrom<float>(ref this._time);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NCrystalSphereMask.PropertyName._material, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 28L, NCrystalSphereMask.PropertyName._values, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 3L, NCrystalSphereMask.PropertyName._time, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).SaveGodotObjectData(info);
    info.AddProperty(NCrystalSphereMask.PropertyName._material, Variant.From<ShaderMaterial>(ref this._material));
    info.AddProperty(NCrystalSphereMask.PropertyName._values, Variant.CreateFrom<Vector3>(this._values));
    info.AddProperty(NCrystalSphereMask.PropertyName._time, Variant.From<float>(ref this._time));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NCrystalSphereMask.PropertyName._material, ref variant1))
      this._material = ((Variant) ref variant1).As<ShaderMaterial>();
    Variant variant2;
    if (info.TryGetProperty(NCrystalSphereMask.PropertyName._values, ref variant2))
      this._values = ((Variant) ref variant2).AsGodotArray<Vector3>();
    Variant variant3;
    if (!info.TryGetProperty(NCrystalSphereMask.PropertyName._time, ref variant3))
      return;
    this._time = ((Variant) ref variant3).As<float>();
  }

  public class MethodName : Control.MethodName
  {
    public static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName _Process = StringName.op_Implicit(nameof (_Process));
  }

  public class PropertyName : Control.PropertyName
  {
    public static readonly StringName _material = StringName.op_Implicit(nameof (_material));
    public static readonly StringName _values = StringName.op_Implicit(nameof (_values));
    public static readonly StringName _time = StringName.op_Implicit(nameof (_time));
  }

  public class SignalName : Control.SignalName
  {
  }
}
