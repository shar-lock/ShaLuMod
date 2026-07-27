// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Vfx.NRadialBlurVfx
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.TestSupport;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Vfx;

[ScriptPath("res://src/Core/Nodes/Vfx/NRadialBlurVfx.cs")]
public class NRadialBlurVfx : BackBufferCopy
{
  private static readonly StringName _blurCenterx = new StringName("blur_center:x");
  private ShaderMaterial _blurShader;
  private VfxPosition _vfxPosition;
  private Control _rect;
  private Tween? _tween;

  public override void _Ready()
  {
    this._rect = ((Node) this).GetNode<Control>(NodePath.op_Implicit("Rect"));
    this._blurShader = (ShaderMaterial) ((CanvasItem) this._rect).GetMaterial();
  }

  public void Activate(VfxPosition vfxPosition = VfxPosition.Center)
  {
    if (TestMode.IsOn || ((CanvasItem) this).Visible)
      return;
    ((CanvasItem) this).Visible = true;
    Control rect = this._rect;
    StringName size = Control.PropertyName.Size;
    Rect2 viewportRect = ((CanvasItem) this).GetViewportRect();
    Variant variant = Variant.op_Implicit(((Rect2) ref viewportRect).Size);
    ((GodotObject) rect).SetDeferred(size, variant);
    switch (vfxPosition)
    {
      case VfxPosition.Left:
        this._blurShader.SetShaderParameter(NRadialBlurVfx._blurCenterx, Variant.op_Implicit(0.3f));
        break;
      case VfxPosition.Right:
        this._blurShader.SetShaderParameter(NRadialBlurVfx._blurCenterx, Variant.op_Implicit(0.6f));
        break;
      default:
        this._blurShader.SetShaderParameter(NRadialBlurVfx._blurCenterx, Variant.op_Implicit(0.45f));
        break;
    }
    TaskHelper.RunSafely(this.Animate());
  }

  public override void _ExitTree() => this._tween?.Kill();

  private async Task Animate()
  {
    this._tween = ((Node) this).CreateTween().SetParallel(true);
    this._tween.TweenProperty((GodotObject) this._blurShader, NodePath.op_Implicit("shader_parameter/blur_power"), Variant.op_Implicit(0.005f), 0.10000000149011612);
    this._tween.Chain();
    this._tween.TweenProperty((GodotObject) this._blurShader, NodePath.op_Implicit("shader_parameter/blur_power"), Variant.op_Implicit(0.0f), 0.89999997615814209).SetEase((Tween.EaseType) 0L).SetTrans((Tween.TransitionType) 7L);
    if (!await this._tween.AwaitFinished((Node) this))
      return;
    ((CanvasItem) this).Visible = false;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(3)
    {
      new MethodInfo(NRadialBlurVfx.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NRadialBlurVfx.MethodName.Activate, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 2L, StringName.op_Implicit("vfxPosition"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NRadialBlurVfx.MethodName._ExitTree, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NRadialBlurVfx.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NRadialBlurVfx.MethodName.Activate) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.Activate(VariantUtils.ConvertTo<VfxPosition>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NRadialBlurVfx.MethodName._ExitTree) || ((NativeVariantPtrArgs) ref args).Count != 0)
      return base.InvokeGodotClassMethod(ref method, args, ref ret);
    ((Node) this)._ExitTree();
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NRadialBlurVfx.MethodName._Ready) || StringName.op_Equality(ref method, NRadialBlurVfx.MethodName.Activate) || StringName.op_Equality(ref method, NRadialBlurVfx.MethodName._ExitTree) || base.HasGodotClassMethod(ref method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NRadialBlurVfx.PropertyName._blurShader))
    {
      this._blurShader = VariantUtils.ConvertTo<ShaderMaterial>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NRadialBlurVfx.PropertyName._vfxPosition))
    {
      this._vfxPosition = VariantUtils.ConvertTo<VfxPosition>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NRadialBlurVfx.PropertyName._rect))
    {
      this._rect = VariantUtils.ConvertTo<Control>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NRadialBlurVfx.PropertyName._tween))
      return ((GodotObject) this).SetGodotClassPropertyValue(ref name, ref value);
    this._tween = VariantUtils.ConvertTo<Tween>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NRadialBlurVfx.PropertyName._blurShader))
    {
      value = VariantUtils.CreateFrom<ShaderMaterial>(ref this._blurShader);
      return true;
    }
    if (StringName.op_Equality(ref name, NRadialBlurVfx.PropertyName._vfxPosition))
    {
      value = VariantUtils.CreateFrom<VfxPosition>(ref this._vfxPosition);
      return true;
    }
    if (StringName.op_Equality(ref name, NRadialBlurVfx.PropertyName._rect))
    {
      value = VariantUtils.CreateFrom<Control>(ref this._rect);
      return true;
    }
    if (!StringName.op_Equality(ref name, NRadialBlurVfx.PropertyName._tween))
      return ((GodotObject) this).GetGodotClassPropertyValue(ref name, ref value);
    value = VariantUtils.CreateFrom<Tween>(ref this._tween);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NRadialBlurVfx.PropertyName._blurShader, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 2L, NRadialBlurVfx.PropertyName._vfxPosition, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NRadialBlurVfx.PropertyName._rect, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NRadialBlurVfx.PropertyName._tween, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).SaveGodotObjectData(info);
    info.AddProperty(NRadialBlurVfx.PropertyName._blurShader, Variant.From<ShaderMaterial>(ref this._blurShader));
    info.AddProperty(NRadialBlurVfx.PropertyName._vfxPosition, Variant.From<VfxPosition>(ref this._vfxPosition));
    info.AddProperty(NRadialBlurVfx.PropertyName._rect, Variant.From<Control>(ref this._rect));
    info.AddProperty(NRadialBlurVfx.PropertyName._tween, Variant.From<Tween>(ref this._tween));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NRadialBlurVfx.PropertyName._blurShader, ref variant1))
      this._blurShader = ((Variant) ref variant1).As<ShaderMaterial>();
    Variant variant2;
    if (info.TryGetProperty(NRadialBlurVfx.PropertyName._vfxPosition, ref variant2))
      this._vfxPosition = ((Variant) ref variant2).As<VfxPosition>();
    Variant variant3;
    if (info.TryGetProperty(NRadialBlurVfx.PropertyName._rect, ref variant3))
      this._rect = ((Variant) ref variant3).As<Control>();
    Variant variant4;
    if (!info.TryGetProperty(NRadialBlurVfx.PropertyName._tween, ref variant4))
      return;
    this._tween = ((Variant) ref variant4).As<Tween>();
  }

  public class MethodName : BackBufferCopy.MethodName
  {
    public static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName Activate = StringName.op_Implicit(nameof (Activate));
    public static readonly StringName _ExitTree = StringName.op_Implicit(nameof (_ExitTree));
  }

  public class PropertyName : BackBufferCopy.PropertyName
  {
    public static readonly StringName _blurShader = StringName.op_Implicit(nameof (_blurShader));
    public static readonly StringName _vfxPosition = StringName.op_Implicit(nameof (_vfxPosition));
    public static readonly StringName _rect = StringName.op_Implicit(nameof (_rect));
    public static readonly StringName _tween = StringName.op_Implicit(nameof (_tween));
  }

  public class SignalName : BackBufferCopy.SignalName
  {
  }
}
