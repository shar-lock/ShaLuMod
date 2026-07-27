// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Vfx.Ui.NGaseousScreenVfx
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.TestSupport;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Vfx.Ui;

[ScriptPath("res://src/Core/Nodes/Vfx/Ui/NGaseousScreenVfx.cs")]
public class NGaseousScreenVfx : AspectRatioContainer
{
  private static readonly string _scenePath = SceneHelper.GetScenePath("vfx/ui/vfx_gaseous_screen");
  [Export]
  private ColorRect _gfx;
  [Export]
  private float _duration = 1f;
  [Export]
  private Curve? _alphaMultiplierCurve;
  [Export]
  private Curve? _minBaseAlphaCurve;
  [Export]
  private Curve? _erosionCurve;
  [Export]
  private Curve? _noiseAOffsetCurve;
  [Export]
  private Curve? _noiseBOffsetCurve;
  private Material? _originalMaterial;
  private ShaderMaterial? _materialCopy;
  private float _noiseAOffsetY;
  private float _noiseBOffsetY;
  private static readonly StringName _alphaMultiplierString = new StringName("alpha_multiplier");
  private static readonly StringName _minBaseAlphaString = new StringName("min_base_alpha");
  private static readonly StringName _noiseAOffsetString = new StringName("noise_a_static_offset");
  private static readonly StringName _noiseBOffsetString = new StringName("noise_b_static_offset");
  private static readonly StringName _erosionString = new StringName("erosion_base");

  public static NGaseousScreenVfx? Create()
  {
    return TestMode.IsOn ? (NGaseousScreenVfx) null : PreloadManager.Cache.GetScene(NGaseousScreenVfx._scenePath).Instantiate<NGaseousScreenVfx>((PackedScene.GenEditState) 0L);
  }

  public override void _Ready()
  {
    this._originalMaterial = ((CanvasItem) this._gfx).Material;
    this._materialCopy = (ShaderMaterial) ((Resource) this._originalMaterial).Duplicate(true);
    ((CanvasItem) this._gfx).Material = (Material) this._materialCopy;
    this.SetProperties(1f);
    this.Play();
  }

  private void SetProperties(float interpolation)
  {
    float num1 = this._alphaMultiplierCurve.Sample(interpolation);
    float num2 = this._minBaseAlphaCurve.Sample(interpolation);
    float num3 = this._noiseAOffsetCurve.Sample(interpolation);
    float num4 = this._noiseBOffsetCurve.Sample(interpolation);
    float num5 = this._erosionCurve.Sample(interpolation);
    this._materialCopy.SetShaderParameter(NGaseousScreenVfx._alphaMultiplierString, Variant.op_Implicit(num1));
    this._materialCopy.SetShaderParameter(NGaseousScreenVfx._minBaseAlphaString, Variant.op_Implicit(num2));
    this._materialCopy.SetShaderParameter(NGaseousScreenVfx._noiseAOffsetString, Variant.op_Implicit(new Vector2(num3, this._noiseAOffsetY)));
    this._materialCopy.SetShaderParameter(NGaseousScreenVfx._noiseBOffsetString, Variant.op_Implicit(new Vector2(num4, this._noiseBOffsetY)));
    this._materialCopy.SetShaderParameter(NGaseousScreenVfx._erosionString, Variant.op_Implicit(num5));
  }

  private void Play() => TaskHelper.RunSafely(this.PlaySequence());

  private async Task PlaySequence()
  {
    this._noiseAOffsetY = GD.Randf();
    this._noiseBOffsetY = GD.Randf();
    double timer = 0.0;
    this.SetProperties(0.0f);
    while (timer < (double) this._duration)
    {
      this.SetProperties((float) timer / this._duration);
      timer += ((Node) this).GetProcessDeltaTime();
      double num = (double) await ((Node) this).AwaitProcessFrame();
    }
    this.SetProperties(1f);
    ((Node) this).QueueFreeSafely();
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(4)
    {
      new MethodInfo(NGaseousScreenVfx.MethodName.Create, new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("AspectRatioContainer"), false), (MethodFlags) 33L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NGaseousScreenVfx.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NGaseousScreenVfx.MethodName.SetProperties, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 3L, StringName.op_Implicit("interpolation"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NGaseousScreenVfx.MethodName.Play, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NGaseousScreenVfx.MethodName.Create) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      NGaseousScreenVfx ngaseousScreenVfx = NGaseousScreenVfx.Create();
      ret = VariantUtils.CreateFrom<NGaseousScreenVfx>(ref ngaseousScreenVfx);
      return true;
    }
    if (StringName.op_Equality(ref method, NGaseousScreenVfx.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NGaseousScreenVfx.MethodName.SetProperties) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.SetProperties(VariantUtils.ConvertTo<float>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NGaseousScreenVfx.MethodName.Play) || ((NativeVariantPtrArgs) ref args).Count != 0)
      return base.InvokeGodotClassMethod(ref method, args, ref ret);
    this.Play();
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static bool InvokeGodotClassStaticMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NGaseousScreenVfx.MethodName.Create) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      NGaseousScreenVfx ngaseousScreenVfx = NGaseousScreenVfx.Create();
      ret = VariantUtils.CreateFrom<NGaseousScreenVfx>(ref ngaseousScreenVfx);
      return true;
    }
    ret = new godot_variant();
    return false;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NGaseousScreenVfx.MethodName.Create) || StringName.op_Equality(ref method, NGaseousScreenVfx.MethodName._Ready) || StringName.op_Equality(ref method, NGaseousScreenVfx.MethodName.SetProperties) || StringName.op_Equality(ref method, NGaseousScreenVfx.MethodName.Play) || base.HasGodotClassMethod(ref method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NGaseousScreenVfx.PropertyName._gfx))
    {
      this._gfx = VariantUtils.ConvertTo<ColorRect>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NGaseousScreenVfx.PropertyName._duration))
    {
      this._duration = VariantUtils.ConvertTo<float>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NGaseousScreenVfx.PropertyName._alphaMultiplierCurve))
    {
      this._alphaMultiplierCurve = VariantUtils.ConvertTo<Curve>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NGaseousScreenVfx.PropertyName._minBaseAlphaCurve))
    {
      this._minBaseAlphaCurve = VariantUtils.ConvertTo<Curve>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NGaseousScreenVfx.PropertyName._erosionCurve))
    {
      this._erosionCurve = VariantUtils.ConvertTo<Curve>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NGaseousScreenVfx.PropertyName._noiseAOffsetCurve))
    {
      this._noiseAOffsetCurve = VariantUtils.ConvertTo<Curve>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NGaseousScreenVfx.PropertyName._noiseBOffsetCurve))
    {
      this._noiseBOffsetCurve = VariantUtils.ConvertTo<Curve>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NGaseousScreenVfx.PropertyName._originalMaterial))
    {
      this._originalMaterial = VariantUtils.ConvertTo<Material>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NGaseousScreenVfx.PropertyName._materialCopy))
    {
      this._materialCopy = VariantUtils.ConvertTo<ShaderMaterial>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NGaseousScreenVfx.PropertyName._noiseAOffsetY))
    {
      this._noiseAOffsetY = VariantUtils.ConvertTo<float>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NGaseousScreenVfx.PropertyName._noiseBOffsetY))
      return ((GodotObject) this).SetGodotClassPropertyValue(ref name, ref value);
    this._noiseBOffsetY = VariantUtils.ConvertTo<float>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NGaseousScreenVfx.PropertyName._gfx))
    {
      value = VariantUtils.CreateFrom<ColorRect>(ref this._gfx);
      return true;
    }
    if (StringName.op_Equality(ref name, NGaseousScreenVfx.PropertyName._duration))
    {
      value = VariantUtils.CreateFrom<float>(ref this._duration);
      return true;
    }
    if (StringName.op_Equality(ref name, NGaseousScreenVfx.PropertyName._alphaMultiplierCurve))
    {
      value = VariantUtils.CreateFrom<Curve>(ref this._alphaMultiplierCurve);
      return true;
    }
    if (StringName.op_Equality(ref name, NGaseousScreenVfx.PropertyName._minBaseAlphaCurve))
    {
      value = VariantUtils.CreateFrom<Curve>(ref this._minBaseAlphaCurve);
      return true;
    }
    if (StringName.op_Equality(ref name, NGaseousScreenVfx.PropertyName._erosionCurve))
    {
      value = VariantUtils.CreateFrom<Curve>(ref this._erosionCurve);
      return true;
    }
    if (StringName.op_Equality(ref name, NGaseousScreenVfx.PropertyName._noiseAOffsetCurve))
    {
      value = VariantUtils.CreateFrom<Curve>(ref this._noiseAOffsetCurve);
      return true;
    }
    if (StringName.op_Equality(ref name, NGaseousScreenVfx.PropertyName._noiseBOffsetCurve))
    {
      value = VariantUtils.CreateFrom<Curve>(ref this._noiseBOffsetCurve);
      return true;
    }
    if (StringName.op_Equality(ref name, NGaseousScreenVfx.PropertyName._originalMaterial))
    {
      value = VariantUtils.CreateFrom<Material>(ref this._originalMaterial);
      return true;
    }
    if (StringName.op_Equality(ref name, NGaseousScreenVfx.PropertyName._materialCopy))
    {
      value = VariantUtils.CreateFrom<ShaderMaterial>(ref this._materialCopy);
      return true;
    }
    if (StringName.op_Equality(ref name, NGaseousScreenVfx.PropertyName._noiseAOffsetY))
    {
      value = VariantUtils.CreateFrom<float>(ref this._noiseAOffsetY);
      return true;
    }
    if (!StringName.op_Equality(ref name, NGaseousScreenVfx.PropertyName._noiseBOffsetY))
      return ((GodotObject) this).GetGodotClassPropertyValue(ref name, ref value);
    value = VariantUtils.CreateFrom<float>(ref this._noiseBOffsetY);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NGaseousScreenVfx.PropertyName._gfx, (PropertyHint) 34L, "ColorRect", (PropertyUsageFlags) 4102L, true),
      new PropertyInfo((Variant.Type) 3L, NGaseousScreenVfx.PropertyName._duration, (PropertyHint) 0L, "", (PropertyUsageFlags) 4102L, true),
      new PropertyInfo((Variant.Type) 24L, NGaseousScreenVfx.PropertyName._alphaMultiplierCurve, (PropertyHint) 17L, "Curve", (PropertyUsageFlags) 4102L, true),
      new PropertyInfo((Variant.Type) 24L, NGaseousScreenVfx.PropertyName._minBaseAlphaCurve, (PropertyHint) 17L, "Curve", (PropertyUsageFlags) 4102L, true),
      new PropertyInfo((Variant.Type) 24L, NGaseousScreenVfx.PropertyName._erosionCurve, (PropertyHint) 17L, "Curve", (PropertyUsageFlags) 4102L, true),
      new PropertyInfo((Variant.Type) 24L, NGaseousScreenVfx.PropertyName._noiseAOffsetCurve, (PropertyHint) 17L, "Curve", (PropertyUsageFlags) 4102L, true),
      new PropertyInfo((Variant.Type) 24L, NGaseousScreenVfx.PropertyName._noiseBOffsetCurve, (PropertyHint) 17L, "Curve", (PropertyUsageFlags) 4102L, true),
      new PropertyInfo((Variant.Type) 24L, NGaseousScreenVfx.PropertyName._originalMaterial, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NGaseousScreenVfx.PropertyName._materialCopy, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 3L, NGaseousScreenVfx.PropertyName._noiseAOffsetY, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 3L, NGaseousScreenVfx.PropertyName._noiseBOffsetY, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).SaveGodotObjectData(info);
    info.AddProperty(NGaseousScreenVfx.PropertyName._gfx, Variant.From<ColorRect>(ref this._gfx));
    info.AddProperty(NGaseousScreenVfx.PropertyName._duration, Variant.From<float>(ref this._duration));
    info.AddProperty(NGaseousScreenVfx.PropertyName._alphaMultiplierCurve, Variant.From<Curve>(ref this._alphaMultiplierCurve));
    info.AddProperty(NGaseousScreenVfx.PropertyName._minBaseAlphaCurve, Variant.From<Curve>(ref this._minBaseAlphaCurve));
    info.AddProperty(NGaseousScreenVfx.PropertyName._erosionCurve, Variant.From<Curve>(ref this._erosionCurve));
    info.AddProperty(NGaseousScreenVfx.PropertyName._noiseAOffsetCurve, Variant.From<Curve>(ref this._noiseAOffsetCurve));
    info.AddProperty(NGaseousScreenVfx.PropertyName._noiseBOffsetCurve, Variant.From<Curve>(ref this._noiseBOffsetCurve));
    info.AddProperty(NGaseousScreenVfx.PropertyName._originalMaterial, Variant.From<Material>(ref this._originalMaterial));
    info.AddProperty(NGaseousScreenVfx.PropertyName._materialCopy, Variant.From<ShaderMaterial>(ref this._materialCopy));
    info.AddProperty(NGaseousScreenVfx.PropertyName._noiseAOffsetY, Variant.From<float>(ref this._noiseAOffsetY));
    info.AddProperty(NGaseousScreenVfx.PropertyName._noiseBOffsetY, Variant.From<float>(ref this._noiseBOffsetY));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NGaseousScreenVfx.PropertyName._gfx, ref variant1))
      this._gfx = ((Variant) ref variant1).As<ColorRect>();
    Variant variant2;
    if (info.TryGetProperty(NGaseousScreenVfx.PropertyName._duration, ref variant2))
      this._duration = ((Variant) ref variant2).As<float>();
    Variant variant3;
    if (info.TryGetProperty(NGaseousScreenVfx.PropertyName._alphaMultiplierCurve, ref variant3))
      this._alphaMultiplierCurve = ((Variant) ref variant3).As<Curve>();
    Variant variant4;
    if (info.TryGetProperty(NGaseousScreenVfx.PropertyName._minBaseAlphaCurve, ref variant4))
      this._minBaseAlphaCurve = ((Variant) ref variant4).As<Curve>();
    Variant variant5;
    if (info.TryGetProperty(NGaseousScreenVfx.PropertyName._erosionCurve, ref variant5))
      this._erosionCurve = ((Variant) ref variant5).As<Curve>();
    Variant variant6;
    if (info.TryGetProperty(NGaseousScreenVfx.PropertyName._noiseAOffsetCurve, ref variant6))
      this._noiseAOffsetCurve = ((Variant) ref variant6).As<Curve>();
    Variant variant7;
    if (info.TryGetProperty(NGaseousScreenVfx.PropertyName._noiseBOffsetCurve, ref variant7))
      this._noiseBOffsetCurve = ((Variant) ref variant7).As<Curve>();
    Variant variant8;
    if (info.TryGetProperty(NGaseousScreenVfx.PropertyName._originalMaterial, ref variant8))
      this._originalMaterial = ((Variant) ref variant8).As<Material>();
    Variant variant9;
    if (info.TryGetProperty(NGaseousScreenVfx.PropertyName._materialCopy, ref variant9))
      this._materialCopy = ((Variant) ref variant9).As<ShaderMaterial>();
    Variant variant10;
    if (info.TryGetProperty(NGaseousScreenVfx.PropertyName._noiseAOffsetY, ref variant10))
      this._noiseAOffsetY = ((Variant) ref variant10).As<float>();
    Variant variant11;
    if (!info.TryGetProperty(NGaseousScreenVfx.PropertyName._noiseBOffsetY, ref variant11))
      return;
    this._noiseBOffsetY = ((Variant) ref variant11).As<float>();
  }

  public class MethodName : AspectRatioContainer.MethodName
  {
    public static readonly StringName Create = StringName.op_Implicit(nameof (Create));
    public static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName SetProperties = StringName.op_Implicit(nameof (SetProperties));
    public static readonly StringName Play = StringName.op_Implicit(nameof (Play));
  }

  public class PropertyName : AspectRatioContainer.PropertyName
  {
    public static readonly StringName _gfx = StringName.op_Implicit(nameof (_gfx));
    public static readonly StringName _duration = StringName.op_Implicit(nameof (_duration));
    public static readonly StringName _alphaMultiplierCurve = StringName.op_Implicit(nameof (_alphaMultiplierCurve));
    public static readonly StringName _minBaseAlphaCurve = StringName.op_Implicit(nameof (_minBaseAlphaCurve));
    public static readonly StringName _erosionCurve = StringName.op_Implicit(nameof (_erosionCurve));
    public static readonly StringName _noiseAOffsetCurve = StringName.op_Implicit(nameof (_noiseAOffsetCurve));
    public static readonly StringName _noiseBOffsetCurve = StringName.op_Implicit(nameof (_noiseBOffsetCurve));
    public static readonly StringName _originalMaterial = StringName.op_Implicit(nameof (_originalMaterial));
    public static readonly StringName _materialCopy = StringName.op_Implicit(nameof (_materialCopy));
    public static readonly StringName _noiseAOffsetY = StringName.op_Implicit(nameof (_noiseAOffsetY));
    public static readonly StringName _noiseBOffsetY = StringName.op_Implicit(nameof (_noiseBOffsetY));
  }

  public class SignalName : AspectRatioContainer.SignalName
  {
  }
}
