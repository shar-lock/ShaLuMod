// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Vfx.Ui.NLowHpBorderVfx
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

[ScriptPath("res://src/Core/Nodes/Vfx/Ui/NLowHpBorderVfx.cs")]
public class NLowHpBorderVfx : ColorRect
{
  public static readonly string scenePath = SceneHelper.GetScenePath("vfx/ui/vfx_low_hp_border");
  [Export]
  private float _duration = 1f;
  [Export]
  private Curve? _alphaMultiplierCurve;
  [Export]
  private Curve? _noiseOffsetCurve;
  [Export]
  private Gradient? _gradient;
  private Material? _originalMaterial;
  private ShaderMaterial? _materialCopy;
  private bool _isPlaying;
  private double _currentTimer;
  private static readonly StringName _alphaMultiplierString = new StringName("alpha_multiplier");
  private static readonly StringName _noiseInitialOffsetString = new StringName("noise_initial_offset");
  private static readonly StringName _mainColorString = new StringName("main_color");

  public static NLowHpBorderVfx? Create()
  {
    return TestMode.IsOn ? (NLowHpBorderVfx) null : PreloadManager.Cache.GetScene(NLowHpBorderVfx.scenePath).Instantiate<NLowHpBorderVfx>((PackedScene.GenEditState) 0L);
  }

  public override void _Ready()
  {
    this._isPlaying = false;
    this._originalMaterial = ((CanvasItem) this).Material;
    this._materialCopy = (ShaderMaterial) ((Resource) this._originalMaterial).Duplicate(true);
    ((CanvasItem) this).Material = (Material) this._materialCopy;
    this.SetProperties(1f);
  }

  private void SetProperties(float interpolation)
  {
    float num = this._alphaMultiplierCurve.Sample(interpolation);
    Color color = this._gradient.Sample(interpolation);
    this._materialCopy.SetShaderParameter(NLowHpBorderVfx._alphaMultiplierString, Variant.op_Implicit(num));
    this._materialCopy.SetShaderParameter(NLowHpBorderVfx._mainColorString, Variant.op_Implicit(color));
  }

  private void RandomizeInitialOffset()
  {
    this._materialCopy.SetShaderParameter(NLowHpBorderVfx._noiseInitialOffsetString, Variant.op_Implicit(new Vector2(GD.Randf(), GD.Randf())));
  }

  public void Play()
  {
    if (this._isPlaying)
      this._currentTimer = 0.0;
    else
      TaskHelper.RunSafely(this.PlaySequence());
  }

  private async Task PlaySequence()
  {
    this._isPlaying = true;
    this._currentTimer = 0.0;
    this.RandomizeInitialOffset();
    this.SetProperties(0.0f);
    while (this._currentTimer < (double) this._duration)
    {
      this.SetProperties((float) this._currentTimer / this._duration);
      this._currentTimer += ((Node) this).GetProcessDeltaTime();
      double num = (double) await ((Node) this).AwaitProcessFrame();
    }
    this.SetProperties(1f);
    this._isPlaying = false;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(5)
    {
      new MethodInfo(NLowHpBorderVfx.MethodName.Create, new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("ColorRect"), false), (MethodFlags) 33L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NLowHpBorderVfx.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NLowHpBorderVfx.MethodName.SetProperties, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 3L, StringName.op_Implicit("interpolation"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NLowHpBorderVfx.MethodName.RandomizeInitialOffset, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NLowHpBorderVfx.MethodName.Play, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NLowHpBorderVfx.MethodName.Create) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      NLowHpBorderVfx nlowHpBorderVfx = NLowHpBorderVfx.Create();
      ret = VariantUtils.CreateFrom<NLowHpBorderVfx>(ref nlowHpBorderVfx);
      return true;
    }
    if (StringName.op_Equality(ref method, NLowHpBorderVfx.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NLowHpBorderVfx.MethodName.SetProperties) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.SetProperties(VariantUtils.ConvertTo<float>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NLowHpBorderVfx.MethodName.RandomizeInitialOffset) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.RandomizeInitialOffset();
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NLowHpBorderVfx.MethodName.Play) || ((NativeVariantPtrArgs) ref args).Count != 0)
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
    if (StringName.op_Equality(ref method, NLowHpBorderVfx.MethodName.Create) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      NLowHpBorderVfx nlowHpBorderVfx = NLowHpBorderVfx.Create();
      ret = VariantUtils.CreateFrom<NLowHpBorderVfx>(ref nlowHpBorderVfx);
      return true;
    }
    ret = new godot_variant();
    return false;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NLowHpBorderVfx.MethodName.Create) || StringName.op_Equality(ref method, NLowHpBorderVfx.MethodName._Ready) || StringName.op_Equality(ref method, NLowHpBorderVfx.MethodName.SetProperties) || StringName.op_Equality(ref method, NLowHpBorderVfx.MethodName.RandomizeInitialOffset) || StringName.op_Equality(ref method, NLowHpBorderVfx.MethodName.Play) || base.HasGodotClassMethod(ref method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NLowHpBorderVfx.PropertyName._duration))
    {
      this._duration = VariantUtils.ConvertTo<float>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NLowHpBorderVfx.PropertyName._alphaMultiplierCurve))
    {
      this._alphaMultiplierCurve = VariantUtils.ConvertTo<Curve>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NLowHpBorderVfx.PropertyName._noiseOffsetCurve))
    {
      this._noiseOffsetCurve = VariantUtils.ConvertTo<Curve>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NLowHpBorderVfx.PropertyName._gradient))
    {
      this._gradient = VariantUtils.ConvertTo<Gradient>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NLowHpBorderVfx.PropertyName._originalMaterial))
    {
      this._originalMaterial = VariantUtils.ConvertTo<Material>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NLowHpBorderVfx.PropertyName._materialCopy))
    {
      this._materialCopy = VariantUtils.ConvertTo<ShaderMaterial>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NLowHpBorderVfx.PropertyName._isPlaying))
    {
      this._isPlaying = VariantUtils.ConvertTo<bool>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NLowHpBorderVfx.PropertyName._currentTimer))
      return ((GodotObject) this).SetGodotClassPropertyValue(ref name, ref value);
    this._currentTimer = VariantUtils.ConvertTo<double>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NLowHpBorderVfx.PropertyName._duration))
    {
      value = VariantUtils.CreateFrom<float>(ref this._duration);
      return true;
    }
    if (StringName.op_Equality(ref name, NLowHpBorderVfx.PropertyName._alphaMultiplierCurve))
    {
      value = VariantUtils.CreateFrom<Curve>(ref this._alphaMultiplierCurve);
      return true;
    }
    if (StringName.op_Equality(ref name, NLowHpBorderVfx.PropertyName._noiseOffsetCurve))
    {
      value = VariantUtils.CreateFrom<Curve>(ref this._noiseOffsetCurve);
      return true;
    }
    if (StringName.op_Equality(ref name, NLowHpBorderVfx.PropertyName._gradient))
    {
      value = VariantUtils.CreateFrom<Gradient>(ref this._gradient);
      return true;
    }
    if (StringName.op_Equality(ref name, NLowHpBorderVfx.PropertyName._originalMaterial))
    {
      value = VariantUtils.CreateFrom<Material>(ref this._originalMaterial);
      return true;
    }
    if (StringName.op_Equality(ref name, NLowHpBorderVfx.PropertyName._materialCopy))
    {
      value = VariantUtils.CreateFrom<ShaderMaterial>(ref this._materialCopy);
      return true;
    }
    if (StringName.op_Equality(ref name, NLowHpBorderVfx.PropertyName._isPlaying))
    {
      value = VariantUtils.CreateFrom<bool>(ref this._isPlaying);
      return true;
    }
    if (!StringName.op_Equality(ref name, NLowHpBorderVfx.PropertyName._currentTimer))
      return ((GodotObject) this).GetGodotClassPropertyValue(ref name, ref value);
    value = VariantUtils.CreateFrom<double>(ref this._currentTimer);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 3L, NLowHpBorderVfx.PropertyName._duration, (PropertyHint) 0L, "", (PropertyUsageFlags) 4102L, true),
      new PropertyInfo((Variant.Type) 24L, NLowHpBorderVfx.PropertyName._alphaMultiplierCurve, (PropertyHint) 17L, "Curve", (PropertyUsageFlags) 4102L, true),
      new PropertyInfo((Variant.Type) 24L, NLowHpBorderVfx.PropertyName._noiseOffsetCurve, (PropertyHint) 17L, "Curve", (PropertyUsageFlags) 4102L, true),
      new PropertyInfo((Variant.Type) 24L, NLowHpBorderVfx.PropertyName._gradient, (PropertyHint) 17L, "Gradient", (PropertyUsageFlags) 4102L, true),
      new PropertyInfo((Variant.Type) 24L, NLowHpBorderVfx.PropertyName._originalMaterial, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NLowHpBorderVfx.PropertyName._materialCopy, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 1L, NLowHpBorderVfx.PropertyName._isPlaying, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 3L, NLowHpBorderVfx.PropertyName._currentTimer, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).SaveGodotObjectData(info);
    info.AddProperty(NLowHpBorderVfx.PropertyName._duration, Variant.From<float>(ref this._duration));
    info.AddProperty(NLowHpBorderVfx.PropertyName._alphaMultiplierCurve, Variant.From<Curve>(ref this._alphaMultiplierCurve));
    info.AddProperty(NLowHpBorderVfx.PropertyName._noiseOffsetCurve, Variant.From<Curve>(ref this._noiseOffsetCurve));
    info.AddProperty(NLowHpBorderVfx.PropertyName._gradient, Variant.From<Gradient>(ref this._gradient));
    info.AddProperty(NLowHpBorderVfx.PropertyName._originalMaterial, Variant.From<Material>(ref this._originalMaterial));
    info.AddProperty(NLowHpBorderVfx.PropertyName._materialCopy, Variant.From<ShaderMaterial>(ref this._materialCopy));
    info.AddProperty(NLowHpBorderVfx.PropertyName._isPlaying, Variant.From<bool>(ref this._isPlaying));
    info.AddProperty(NLowHpBorderVfx.PropertyName._currentTimer, Variant.From<double>(ref this._currentTimer));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NLowHpBorderVfx.PropertyName._duration, ref variant1))
      this._duration = ((Variant) ref variant1).As<float>();
    Variant variant2;
    if (info.TryGetProperty(NLowHpBorderVfx.PropertyName._alphaMultiplierCurve, ref variant2))
      this._alphaMultiplierCurve = ((Variant) ref variant2).As<Curve>();
    Variant variant3;
    if (info.TryGetProperty(NLowHpBorderVfx.PropertyName._noiseOffsetCurve, ref variant3))
      this._noiseOffsetCurve = ((Variant) ref variant3).As<Curve>();
    Variant variant4;
    if (info.TryGetProperty(NLowHpBorderVfx.PropertyName._gradient, ref variant4))
      this._gradient = ((Variant) ref variant4).As<Gradient>();
    Variant variant5;
    if (info.TryGetProperty(NLowHpBorderVfx.PropertyName._originalMaterial, ref variant5))
      this._originalMaterial = ((Variant) ref variant5).As<Material>();
    Variant variant6;
    if (info.TryGetProperty(NLowHpBorderVfx.PropertyName._materialCopy, ref variant6))
      this._materialCopy = ((Variant) ref variant6).As<ShaderMaterial>();
    Variant variant7;
    if (info.TryGetProperty(NLowHpBorderVfx.PropertyName._isPlaying, ref variant7))
      this._isPlaying = ((Variant) ref variant7).As<bool>();
    Variant variant8;
    if (!info.TryGetProperty(NLowHpBorderVfx.PropertyName._currentTimer, ref variant8))
      return;
    this._currentTimer = ((Variant) ref variant8).As<double>();
  }

  public class MethodName : ColorRect.MethodName
  {
    public static readonly StringName Create = StringName.op_Implicit(nameof (Create));
    public static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName SetProperties = StringName.op_Implicit(nameof (SetProperties));
    public static readonly StringName RandomizeInitialOffset = StringName.op_Implicit(nameof (RandomizeInitialOffset));
    public static readonly StringName Play = StringName.op_Implicit(nameof (Play));
  }

  public class PropertyName : ColorRect.PropertyName
  {
    public static readonly StringName _duration = StringName.op_Implicit(nameof (_duration));
    public static readonly StringName _alphaMultiplierCurve = StringName.op_Implicit(nameof (_alphaMultiplierCurve));
    public static readonly StringName _noiseOffsetCurve = StringName.op_Implicit(nameof (_noiseOffsetCurve));
    public static readonly StringName _gradient = StringName.op_Implicit(nameof (_gradient));
    public static readonly StringName _originalMaterial = StringName.op_Implicit(nameof (_originalMaterial));
    public static readonly StringName _materialCopy = StringName.op_Implicit(nameof (_materialCopy));
    public static readonly StringName _isPlaying = StringName.op_Implicit(nameof (_isPlaying));
    public static readonly StringName _currentTimer = StringName.op_Implicit(nameof (_currentTimer));
  }

  public class SignalName : ColorRect.SignalName
  {
  }
}
