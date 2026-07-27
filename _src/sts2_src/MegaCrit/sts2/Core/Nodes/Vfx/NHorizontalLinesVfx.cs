// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Vfx.NHorizontalLinesVfx
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
namespace MegaCrit.Sts2.Core.Nodes.Vfx;

[ScriptPath("res://src/Core/Nodes/Vfx/NHorizontalLinesVfx.cs")]
public class NHorizontalLinesVfx : GpuParticles2D
{
  private Tween? _tween;
  private static readonly string _scenePath = SceneHelper.GetScenePath("vfx/whole_screen/horizontal_lines_vfx");
  private double _duration;
  private ParticleProcessMaterial _mat;
  private bool _isMovingRight;

  public static IEnumerable<string> AssetPaths
  {
    get
    {
      return (IEnumerable<string>) new \u003C\u003Ez__ReadOnlySingleElementList<string>(NHorizontalLinesVfx._scenePath);
    }
  }

  public static NHorizontalLinesVfx? Create(Color color, double duration = 2.0, bool movingRightwards = true)
  {
    if (TestMode.IsOn)
      return (NHorizontalLinesVfx) null;
    NHorizontalLinesVfx nhorizontalLinesVfx = PreloadManager.Cache.GetScene(NHorizontalLinesVfx._scenePath).Instantiate<NHorizontalLinesVfx>((PackedScene.GenEditState) 0L);
    nhorizontalLinesVfx._duration = Mathf.Max(1.0, duration);
    nhorizontalLinesVfx._mat = (ParticleProcessMaterial) nhorizontalLinesVfx.ProcessMaterial;
    nhorizontalLinesVfx._mat.Color = color;
    nhorizontalLinesVfx._isMovingRight = movingRightwards;
    return nhorizontalLinesVfx;
  }

  public override void _Ready()
  {
    Control parent = ((Node) this).GetParent<Control>();
    this._mat.EmissionShapeOffset = new Vector3(-500f, parent.Size.Y * 0.5f, 0.0f);
    this._mat.EmissionShapeScale = new Vector3(200f, parent.Size.Y * 0.5f, 1f);
    if (!this._isMovingRight)
    {
      ((Node2D) this).RotationDegrees = 180f;
      ((Node2D) this).Position = new Vector2(parent.Size.X, parent.Size.Y);
    }
    TaskHelper.RunSafely(this.PlayAnim());
  }

  public override void _ExitTree() => this._tween?.Kill();

  private async Task PlayAnim()
  {
    ((CanvasItem) this).Modulate = StsColors.transparentWhite;
    this._tween = ((Node) this).CreateTween().SetParallel(true);
    this._tween.TweenProperty((GodotObject) this, NodePath.op_Implicit("modulate:a"), Variant.op_Implicit(1f), 0.45);
    this._tween.Chain();
    this._tween.TweenInterval(this._duration - 0.9);
    this._tween.Chain();
    this._tween.TweenProperty((GodotObject) this, NodePath.op_Implicit("modulate:a"), Variant.op_Implicit(0.0f), 0.45);
    bool flag = await this._tween.AwaitFinished((Node) this);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(3)
    {
      new MethodInfo(NHorizontalLinesVfx.MethodName.Create, new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("GPUParticles2D"), false), (MethodFlags) 33L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 20L, StringName.op_Implicit("color"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false),
        new PropertyInfo((Variant.Type) 3L, StringName.op_Implicit("duration"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false),
        new PropertyInfo((Variant.Type) 1L, StringName.op_Implicit("movingRightwards"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NHorizontalLinesVfx.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NHorizontalLinesVfx.MethodName._ExitTree, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NHorizontalLinesVfx.MethodName.Create) && ((NativeVariantPtrArgs) ref args).Count == 3)
    {
      NHorizontalLinesVfx nhorizontalLinesVfx = NHorizontalLinesVfx.Create(VariantUtils.ConvertTo<Color>(ref ((NativeVariantPtrArgs) ref args)[0]), VariantUtils.ConvertTo<double>(ref ((NativeVariantPtrArgs) ref args)[1]), VariantUtils.ConvertTo<bool>(ref ((NativeVariantPtrArgs) ref args)[2]));
      ret = VariantUtils.CreateFrom<NHorizontalLinesVfx>(ref nhorizontalLinesVfx);
      return true;
    }
    if (StringName.op_Equality(ref method, NHorizontalLinesVfx.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NHorizontalLinesVfx.MethodName._ExitTree) || ((NativeVariantPtrArgs) ref args).Count != 0)
      return base.InvokeGodotClassMethod(ref method, args, ref ret);
    ((Node) this)._ExitTree();
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static bool InvokeGodotClassStaticMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NHorizontalLinesVfx.MethodName.Create) && ((NativeVariantPtrArgs) ref args).Count == 3)
    {
      NHorizontalLinesVfx nhorizontalLinesVfx = NHorizontalLinesVfx.Create(VariantUtils.ConvertTo<Color>(ref ((NativeVariantPtrArgs) ref args)[0]), VariantUtils.ConvertTo<double>(ref ((NativeVariantPtrArgs) ref args)[1]), VariantUtils.ConvertTo<bool>(ref ((NativeVariantPtrArgs) ref args)[2]));
      ret = VariantUtils.CreateFrom<NHorizontalLinesVfx>(ref nhorizontalLinesVfx);
      return true;
    }
    ret = new godot_variant();
    return false;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NHorizontalLinesVfx.MethodName.Create) || StringName.op_Equality(ref method, NHorizontalLinesVfx.MethodName._Ready) || StringName.op_Equality(ref method, NHorizontalLinesVfx.MethodName._ExitTree) || base.HasGodotClassMethod(ref method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NHorizontalLinesVfx.PropertyName._tween))
    {
      this._tween = VariantUtils.ConvertTo<Tween>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NHorizontalLinesVfx.PropertyName._duration))
    {
      this._duration = VariantUtils.ConvertTo<double>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NHorizontalLinesVfx.PropertyName._mat))
    {
      this._mat = VariantUtils.ConvertTo<ParticleProcessMaterial>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NHorizontalLinesVfx.PropertyName._isMovingRight))
      return ((GodotObject) this).SetGodotClassPropertyValue(ref name, ref value);
    this._isMovingRight = VariantUtils.ConvertTo<bool>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NHorizontalLinesVfx.PropertyName._tween))
    {
      value = VariantUtils.CreateFrom<Tween>(ref this._tween);
      return true;
    }
    if (StringName.op_Equality(ref name, NHorizontalLinesVfx.PropertyName._duration))
    {
      value = VariantUtils.CreateFrom<double>(ref this._duration);
      return true;
    }
    if (StringName.op_Equality(ref name, NHorizontalLinesVfx.PropertyName._mat))
    {
      value = VariantUtils.CreateFrom<ParticleProcessMaterial>(ref this._mat);
      return true;
    }
    if (!StringName.op_Equality(ref name, NHorizontalLinesVfx.PropertyName._isMovingRight))
      return ((GodotObject) this).GetGodotClassPropertyValue(ref name, ref value);
    value = VariantUtils.CreateFrom<bool>(ref this._isMovingRight);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NHorizontalLinesVfx.PropertyName._tween, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 3L, NHorizontalLinesVfx.PropertyName._duration, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NHorizontalLinesVfx.PropertyName._mat, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 1L, NHorizontalLinesVfx.PropertyName._isMovingRight, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).SaveGodotObjectData(info);
    info.AddProperty(NHorizontalLinesVfx.PropertyName._tween, Variant.From<Tween>(ref this._tween));
    info.AddProperty(NHorizontalLinesVfx.PropertyName._duration, Variant.From<double>(ref this._duration));
    info.AddProperty(NHorizontalLinesVfx.PropertyName._mat, Variant.From<ParticleProcessMaterial>(ref this._mat));
    info.AddProperty(NHorizontalLinesVfx.PropertyName._isMovingRight, Variant.From<bool>(ref this._isMovingRight));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NHorizontalLinesVfx.PropertyName._tween, ref variant1))
      this._tween = ((Variant) ref variant1).As<Tween>();
    Variant variant2;
    if (info.TryGetProperty(NHorizontalLinesVfx.PropertyName._duration, ref variant2))
      this._duration = ((Variant) ref variant2).As<double>();
    Variant variant3;
    if (info.TryGetProperty(NHorizontalLinesVfx.PropertyName._mat, ref variant3))
      this._mat = ((Variant) ref variant3).As<ParticleProcessMaterial>();
    Variant variant4;
    if (!info.TryGetProperty(NHorizontalLinesVfx.PropertyName._isMovingRight, ref variant4))
      return;
    this._isMovingRight = ((Variant) ref variant4).As<bool>();
  }

  public class MethodName : GpuParticles2D.MethodName
  {
    public static readonly StringName Create = StringName.op_Implicit(nameof (Create));
    public static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName _ExitTree = StringName.op_Implicit(nameof (_ExitTree));
  }

  public class PropertyName : GpuParticles2D.PropertyName
  {
    public static readonly StringName _tween = StringName.op_Implicit(nameof (_tween));
    public static readonly StringName _duration = StringName.op_Implicit(nameof (_duration));
    public static readonly StringName _mat = StringName.op_Implicit(nameof (_mat));
    public static readonly StringName _isMovingRight = StringName.op_Implicit(nameof (_isMovingRight));
  }

  public class SignalName : GpuParticles2D.SignalName
  {
  }
}
