// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Vfx.NMirrorVfx
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.TestSupport;
using System.Collections.Generic;
using System.ComponentModel;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Vfx;

[ScriptPath("res://src/Core/Nodes/Vfx/NMirrorVfx.cs")]
public class NMirrorVfx : Control
{
  private Sprite2D _mask1;
  private Control _reflection1;
  private Sprite2D _mask2;
  private Control _reflection2;
  private Sprite2D _mask3;
  private Control _reflection3;
  private FastNoiseLite _noise = new FastNoiseLite();
  private float _totalTime;
  private const float _noiseSpeed = 2f;

  private static string ScenePath => SceneHelper.GetScenePath("vfx/whole_screen/mirror_vfx");

  public static NMirrorVfx? Create()
  {
    return TestMode.IsOn ? (NMirrorVfx) null : PreloadManager.Cache.GetScene(NMirrorVfx.ScenePath).Instantiate<NMirrorVfx>((PackedScene.GenEditState) 0L);
  }

  public override void _Ready()
  {
    this._mask1 = ((Node) this).GetNode<Sprite2D>(NodePath.op_Implicit("Mask1"));
    this._reflection1 = ((Node) this).GetNode<Control>(NodePath.op_Implicit("Mask1/Reflection"));
    this._mask2 = ((Node) this).GetNode<Sprite2D>(NodePath.op_Implicit("Mask2"));
    this._reflection2 = ((Node) this).GetNode<Control>(NodePath.op_Implicit("Mask2/Reflection"));
    this._mask3 = ((Node) this).GetNode<Sprite2D>(NodePath.op_Implicit("Mask3"));
    this._reflection3 = ((Node) this).GetNode<Control>(NodePath.op_Implicit("Mask3/Reflection"));
  }

  public override void _Process(double delta)
  {
    this._totalTime += (float) delta * 2f;
    this._noise.Seed = 0;
    float num1 = 1.05f + ((Noise) this._noise).GetNoise1D(this._totalTime);
    ((Node2D) this._mask1).Scale = new Vector2(num1, num1);
    this._reflection1.Scale = ((Node2D) this._mask1).Scale;
    this._noise.Seed = 1;
    ((Node2D) this._mask1).RotationDegrees = Mathf.Abs(((Noise) this._noise).GetNoise1D(this._totalTime)) * 10f;
    this._noise.Seed = 2;
    float num2 = 1.05f + ((Noise) this._noise).GetNoise1D(this._totalTime);
    ((Node2D) this._mask2).Scale = new Vector2(num2, num2);
    this._reflection2.Scale = new Vector2(num2, num2);
    this._noise.Seed = 3;
    ((Node2D) this._mask2).RotationDegrees = Mathf.Abs(((Noise) this._noise).GetNoise1D(this._totalTime)) * 20f;
    this._noise.Seed = 4;
    float num3 = 1.05f + ((Noise) this._noise).GetNoise1D(this._totalTime);
    ((Node2D) this._mask3).Scale = new Vector2(num3, num3);
    this._reflection3.Scale = new Vector2(num3, num3);
    this._noise.Seed = 5;
    ((Node2D) this._mask3).RotationDegrees = Mathf.Abs(((Noise) this._noise).GetNoise1D(this._totalTime)) * 30f;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(3)
    {
      new MethodInfo(NMirrorVfx.MethodName.Create, new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false), (MethodFlags) 33L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NMirrorVfx.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NMirrorVfx.MethodName._Process, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
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
    if (StringName.op_Equality(ref method, NMirrorVfx.MethodName.Create) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      NMirrorVfx nmirrorVfx = NMirrorVfx.Create();
      ret = VariantUtils.CreateFrom<NMirrorVfx>(ref nmirrorVfx);
      return true;
    }
    if (StringName.op_Equality(ref method, NMirrorVfx.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NMirrorVfx.MethodName._Process) || ((NativeVariantPtrArgs) ref args).Count != 1)
      return base.InvokeGodotClassMethod(ref method, args, ref ret);
    ((Node) this)._Process(VariantUtils.ConvertTo<double>(ref ((NativeVariantPtrArgs) ref args)[0]));
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static bool InvokeGodotClassStaticMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NMirrorVfx.MethodName.Create) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      NMirrorVfx nmirrorVfx = NMirrorVfx.Create();
      ret = VariantUtils.CreateFrom<NMirrorVfx>(ref nmirrorVfx);
      return true;
    }
    ret = new godot_variant();
    return false;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NMirrorVfx.MethodName.Create) || StringName.op_Equality(ref method, NMirrorVfx.MethodName._Ready) || StringName.op_Equality(ref method, NMirrorVfx.MethodName._Process) || base.HasGodotClassMethod(ref method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NMirrorVfx.PropertyName._mask1))
    {
      this._mask1 = VariantUtils.ConvertTo<Sprite2D>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NMirrorVfx.PropertyName._reflection1))
    {
      this._reflection1 = VariantUtils.ConvertTo<Control>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NMirrorVfx.PropertyName._mask2))
    {
      this._mask2 = VariantUtils.ConvertTo<Sprite2D>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NMirrorVfx.PropertyName._reflection2))
    {
      this._reflection2 = VariantUtils.ConvertTo<Control>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NMirrorVfx.PropertyName._mask3))
    {
      this._mask3 = VariantUtils.ConvertTo<Sprite2D>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NMirrorVfx.PropertyName._reflection3))
    {
      this._reflection3 = VariantUtils.ConvertTo<Control>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NMirrorVfx.PropertyName._noise))
    {
      this._noise = VariantUtils.ConvertTo<FastNoiseLite>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NMirrorVfx.PropertyName._totalTime))
      return ((GodotObject) this).SetGodotClassPropertyValue(ref name, ref value);
    this._totalTime = VariantUtils.ConvertTo<float>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NMirrorVfx.PropertyName._mask1))
    {
      value = VariantUtils.CreateFrom<Sprite2D>(ref this._mask1);
      return true;
    }
    if (StringName.op_Equality(ref name, NMirrorVfx.PropertyName._reflection1))
    {
      value = VariantUtils.CreateFrom<Control>(ref this._reflection1);
      return true;
    }
    if (StringName.op_Equality(ref name, NMirrorVfx.PropertyName._mask2))
    {
      value = VariantUtils.CreateFrom<Sprite2D>(ref this._mask2);
      return true;
    }
    if (StringName.op_Equality(ref name, NMirrorVfx.PropertyName._reflection2))
    {
      value = VariantUtils.CreateFrom<Control>(ref this._reflection2);
      return true;
    }
    if (StringName.op_Equality(ref name, NMirrorVfx.PropertyName._mask3))
    {
      value = VariantUtils.CreateFrom<Sprite2D>(ref this._mask3);
      return true;
    }
    if (StringName.op_Equality(ref name, NMirrorVfx.PropertyName._reflection3))
    {
      value = VariantUtils.CreateFrom<Control>(ref this._reflection3);
      return true;
    }
    if (StringName.op_Equality(ref name, NMirrorVfx.PropertyName._noise))
    {
      value = VariantUtils.CreateFrom<FastNoiseLite>(ref this._noise);
      return true;
    }
    if (!StringName.op_Equality(ref name, NMirrorVfx.PropertyName._totalTime))
      return ((GodotObject) this).GetGodotClassPropertyValue(ref name, ref value);
    value = VariantUtils.CreateFrom<float>(ref this._totalTime);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NMirrorVfx.PropertyName._mask1, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NMirrorVfx.PropertyName._reflection1, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NMirrorVfx.PropertyName._mask2, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NMirrorVfx.PropertyName._reflection2, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NMirrorVfx.PropertyName._mask3, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NMirrorVfx.PropertyName._reflection3, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NMirrorVfx.PropertyName._noise, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 3L, NMirrorVfx.PropertyName._totalTime, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).SaveGodotObjectData(info);
    info.AddProperty(NMirrorVfx.PropertyName._mask1, Variant.From<Sprite2D>(ref this._mask1));
    info.AddProperty(NMirrorVfx.PropertyName._reflection1, Variant.From<Control>(ref this._reflection1));
    info.AddProperty(NMirrorVfx.PropertyName._mask2, Variant.From<Sprite2D>(ref this._mask2));
    info.AddProperty(NMirrorVfx.PropertyName._reflection2, Variant.From<Control>(ref this._reflection2));
    info.AddProperty(NMirrorVfx.PropertyName._mask3, Variant.From<Sprite2D>(ref this._mask3));
    info.AddProperty(NMirrorVfx.PropertyName._reflection3, Variant.From<Control>(ref this._reflection3));
    info.AddProperty(NMirrorVfx.PropertyName._noise, Variant.From<FastNoiseLite>(ref this._noise));
    info.AddProperty(NMirrorVfx.PropertyName._totalTime, Variant.From<float>(ref this._totalTime));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NMirrorVfx.PropertyName._mask1, ref variant1))
      this._mask1 = ((Variant) ref variant1).As<Sprite2D>();
    Variant variant2;
    if (info.TryGetProperty(NMirrorVfx.PropertyName._reflection1, ref variant2))
      this._reflection1 = ((Variant) ref variant2).As<Control>();
    Variant variant3;
    if (info.TryGetProperty(NMirrorVfx.PropertyName._mask2, ref variant3))
      this._mask2 = ((Variant) ref variant3).As<Sprite2D>();
    Variant variant4;
    if (info.TryGetProperty(NMirrorVfx.PropertyName._reflection2, ref variant4))
      this._reflection2 = ((Variant) ref variant4).As<Control>();
    Variant variant5;
    if (info.TryGetProperty(NMirrorVfx.PropertyName._mask3, ref variant5))
      this._mask3 = ((Variant) ref variant5).As<Sprite2D>();
    Variant variant6;
    if (info.TryGetProperty(NMirrorVfx.PropertyName._reflection3, ref variant6))
      this._reflection3 = ((Variant) ref variant6).As<Control>();
    Variant variant7;
    if (info.TryGetProperty(NMirrorVfx.PropertyName._noise, ref variant7))
      this._noise = ((Variant) ref variant7).As<FastNoiseLite>();
    Variant variant8;
    if (!info.TryGetProperty(NMirrorVfx.PropertyName._totalTime, ref variant8))
      return;
    this._totalTime = ((Variant) ref variant8).As<float>();
  }

  public class MethodName : Control.MethodName
  {
    public static readonly StringName Create = StringName.op_Implicit(nameof (Create));
    public static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName _Process = StringName.op_Implicit(nameof (_Process));
  }

  public class PropertyName : Control.PropertyName
  {
    public static readonly StringName _mask1 = StringName.op_Implicit(nameof (_mask1));
    public static readonly StringName _reflection1 = StringName.op_Implicit(nameof (_reflection1));
    public static readonly StringName _mask2 = StringName.op_Implicit(nameof (_mask2));
    public static readonly StringName _reflection2 = StringName.op_Implicit(nameof (_reflection2));
    public static readonly StringName _mask3 = StringName.op_Implicit(nameof (_mask3));
    public static readonly StringName _reflection3 = StringName.op_Implicit(nameof (_reflection3));
    public static readonly StringName _noise = StringName.op_Implicit(nameof (_noise));
    public static readonly StringName _totalTime = StringName.op_Implicit(nameof (_totalTime));
  }

  public class SignalName : Control.SignalName
  {
  }
}
