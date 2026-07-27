// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Vfx.Examples.NNoiseScroller
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using System.Collections.Generic;
using System.ComponentModel;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Vfx.Examples;

[ScriptPath("res://src/Core/Nodes/Vfx/Examples/NNoiseScroller.cs")]
public class NNoiseScroller : TextureRect
{
  [Export]
  private Vector3 _offsetDelta;
  private FastNoiseLite _noise;

  public override void _Ready()
  {
    this._noise = (FastNoiseLite) ((NoiseTexture2D) this.Texture).Noise;
  }

  public override void _Process(double delta)
  {
    FastNoiseLite noise = this._noise;
    noise.Offset = Vector3.op_Addition(noise.Offset, Vector3.op_Multiply(this._offsetDelta, (float) delta));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(2)
    {
      new MethodInfo(NNoiseScroller.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NNoiseScroller.MethodName._Process, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
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
    if (StringName.op_Equality(ref method, NNoiseScroller.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NNoiseScroller.MethodName._Process) || ((NativeVariantPtrArgs) ref args).Count != 1)
      return base.InvokeGodotClassMethod(ref method, args, ref ret);
    ((Node) this)._Process(VariantUtils.ConvertTo<double>(ref ((NativeVariantPtrArgs) ref args)[0]));
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NNoiseScroller.MethodName._Ready) || StringName.op_Equality(ref method, NNoiseScroller.MethodName._Process) || base.HasGodotClassMethod(ref method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NNoiseScroller.PropertyName._offsetDelta))
    {
      this._offsetDelta = VariantUtils.ConvertTo<Vector3>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NNoiseScroller.PropertyName._noise))
      return ((GodotObject) this).SetGodotClassPropertyValue(ref name, ref value);
    this._noise = VariantUtils.ConvertTo<FastNoiseLite>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NNoiseScroller.PropertyName._offsetDelta))
    {
      value = VariantUtils.CreateFrom<Vector3>(ref this._offsetDelta);
      return true;
    }
    if (!StringName.op_Equality(ref name, NNoiseScroller.PropertyName._noise))
      return ((GodotObject) this).GetGodotClassPropertyValue(ref name, ref value);
    value = VariantUtils.CreateFrom<FastNoiseLite>(ref this._noise);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 9L, NNoiseScroller.PropertyName._offsetDelta, (PropertyHint) 0L, "", (PropertyUsageFlags) 4102L, true),
      new PropertyInfo((Variant.Type) 24L, NNoiseScroller.PropertyName._noise, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).SaveGodotObjectData(info);
    info.AddProperty(NNoiseScroller.PropertyName._offsetDelta, Variant.From<Vector3>(ref this._offsetDelta));
    info.AddProperty(NNoiseScroller.PropertyName._noise, Variant.From<FastNoiseLite>(ref this._noise));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NNoiseScroller.PropertyName._offsetDelta, ref variant1))
      this._offsetDelta = ((Variant) ref variant1).As<Vector3>();
    Variant variant2;
    if (!info.TryGetProperty(NNoiseScroller.PropertyName._noise, ref variant2))
      return;
    this._noise = ((Variant) ref variant2).As<FastNoiseLite>();
  }

  public class MethodName : TextureRect.MethodName
  {
    public static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName _Process = StringName.op_Implicit(nameof (_Process));
  }

  public class PropertyName : TextureRect.PropertyName
  {
    public static readonly StringName _offsetDelta = StringName.op_Implicit(nameof (_offsetDelta));
    public static readonly StringName _noise = StringName.op_Implicit(nameof (_noise));
  }

  public class SignalName : TextureRect.SignalName
  {
  }
}
