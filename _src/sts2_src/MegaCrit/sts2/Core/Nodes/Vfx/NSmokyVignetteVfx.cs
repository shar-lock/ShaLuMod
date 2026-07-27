// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Vfx.NSmokyVignetteVfx
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

[ScriptPath("res://src/Core/Nodes/Vfx/NSmokyVignetteVfx.cs")]
public class NSmokyVignetteVfx : Control
{
  private const string _path = "res://scenes/vfx/whole_screen/vfx_smoky_vignette.tscn";
  private Control _highlights;
  private Color _targetColor;
  private Color _highlightColor;
  private Tween? _tween;

  public static IEnumerable<string> AssetPaths
  {
    get
    {
      return (IEnumerable<string>) new \u003C\u003Ez__ReadOnlySingleElementList<string>("res://scenes/vfx/whole_screen/vfx_smoky_vignette.tscn");
    }
  }

  public static NSmokyVignetteVfx? Create(Color tint, Color highlightColor)
  {
    if (TestMode.IsOn)
      return (NSmokyVignetteVfx) null;
    NSmokyVignetteVfx nsmokyVignetteVfx = PreloadManager.Cache.GetScene("res://scenes/vfx/whole_screen/vfx_smoky_vignette.tscn").Instantiate<NSmokyVignetteVfx>((PackedScene.GenEditState) 0L);
    ((CanvasItem) nsmokyVignetteVfx).Modulate = tint;
    nsmokyVignetteVfx._targetColor = tint;
    nsmokyVignetteVfx._highlightColor = highlightColor;
    return nsmokyVignetteVfx;
  }

  public override void _Ready()
  {
    this._highlights = ((Node) this).GetNode<Control>(NodePath.op_Implicit("Highlights"));
    ((CanvasItem) this._highlights).Modulate = this._highlightColor;
    TaskHelper.RunSafely(this.Animate(true));
  }

  public void Reset(Color tint, Color highlightColor)
  {
    this._targetColor = tint;
    this._highlightColor = highlightColor;
    this._tween?.Kill();
    TaskHelper.RunSafely(this.Animate(false));
  }

  private async Task Animate(bool fadeIn)
  {
    this._tween = ((Node) this).CreateTween().SetParallel(true);
    if (fadeIn)
    {
      this._tween.TweenProperty((GodotObject) this, NodePath.op_Implicit("modulate:a"), Variant.op_Implicit(this._targetColor.A), 0.1).From(Variant.op_Implicit(0.0f));
      this._tween.TweenProperty((GodotObject) this._highlights, NodePath.op_Implicit("modulate:a"), Variant.op_Implicit(this._highlightColor.A), 0.1).From(Variant.op_Implicit(0.0f));
    }
    this._tween.Chain();
    this._tween.TweenProperty((GodotObject) this, NodePath.op_Implicit("modulate:a"), Variant.op_Implicit(0.0f), 1.0).SetEase((Tween.EaseType) 0L).SetTrans((Tween.TransitionType) 7L);
    this._tween.TweenProperty((GodotObject) this._highlights, NodePath.op_Implicit("modulate:a"), Variant.op_Implicit(0.0f), 1.0);
    while (this._tween.IsValid() && this._tween.IsRunning())
    {
      double num = (double) await ((Node) this).AwaitProcessFrame();
    }
    if (!this._tween.IsValid())
      return;
    ((Node) this).QueueFreeSafely();
  }

  public override void _ExitTree() => this._tween?.Kill();

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(4)
    {
      new MethodInfo(NSmokyVignetteVfx.MethodName.Create, new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false), (MethodFlags) 33L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 20L, StringName.op_Implicit("tint"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false),
        new PropertyInfo((Variant.Type) 20L, StringName.op_Implicit("highlightColor"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NSmokyVignetteVfx.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NSmokyVignetteVfx.MethodName.Reset, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 20L, StringName.op_Implicit("tint"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false),
        new PropertyInfo((Variant.Type) 20L, StringName.op_Implicit("highlightColor"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NSmokyVignetteVfx.MethodName._ExitTree, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NSmokyVignetteVfx.MethodName.Create) && ((NativeVariantPtrArgs) ref args).Count == 2)
    {
      NSmokyVignetteVfx nsmokyVignetteVfx = NSmokyVignetteVfx.Create(VariantUtils.ConvertTo<Color>(ref ((NativeVariantPtrArgs) ref args)[0]), VariantUtils.ConvertTo<Color>(ref ((NativeVariantPtrArgs) ref args)[1]));
      ret = VariantUtils.CreateFrom<NSmokyVignetteVfx>(ref nsmokyVignetteVfx);
      return true;
    }
    if (StringName.op_Equality(ref method, NSmokyVignetteVfx.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NSmokyVignetteVfx.MethodName.Reset) && ((NativeVariantPtrArgs) ref args).Count == 2)
    {
      this.Reset(VariantUtils.ConvertTo<Color>(ref ((NativeVariantPtrArgs) ref args)[0]), VariantUtils.ConvertTo<Color>(ref ((NativeVariantPtrArgs) ref args)[1]));
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NSmokyVignetteVfx.MethodName._ExitTree) || ((NativeVariantPtrArgs) ref args).Count != 0)
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
    if (StringName.op_Equality(ref method, NSmokyVignetteVfx.MethodName.Create) && ((NativeVariantPtrArgs) ref args).Count == 2)
    {
      NSmokyVignetteVfx nsmokyVignetteVfx = NSmokyVignetteVfx.Create(VariantUtils.ConvertTo<Color>(ref ((NativeVariantPtrArgs) ref args)[0]), VariantUtils.ConvertTo<Color>(ref ((NativeVariantPtrArgs) ref args)[1]));
      ret = VariantUtils.CreateFrom<NSmokyVignetteVfx>(ref nsmokyVignetteVfx);
      return true;
    }
    ret = new godot_variant();
    return false;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NSmokyVignetteVfx.MethodName.Create) || StringName.op_Equality(ref method, NSmokyVignetteVfx.MethodName._Ready) || StringName.op_Equality(ref method, NSmokyVignetteVfx.MethodName.Reset) || StringName.op_Equality(ref method, NSmokyVignetteVfx.MethodName._ExitTree) || base.HasGodotClassMethod(ref method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NSmokyVignetteVfx.PropertyName._highlights))
    {
      this._highlights = VariantUtils.ConvertTo<Control>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NSmokyVignetteVfx.PropertyName._targetColor))
    {
      this._targetColor = VariantUtils.ConvertTo<Color>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NSmokyVignetteVfx.PropertyName._highlightColor))
    {
      this._highlightColor = VariantUtils.ConvertTo<Color>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NSmokyVignetteVfx.PropertyName._tween))
      return ((GodotObject) this).SetGodotClassPropertyValue(ref name, ref value);
    this._tween = VariantUtils.ConvertTo<Tween>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NSmokyVignetteVfx.PropertyName._highlights))
    {
      value = VariantUtils.CreateFrom<Control>(ref this._highlights);
      return true;
    }
    if (StringName.op_Equality(ref name, NSmokyVignetteVfx.PropertyName._targetColor))
    {
      value = VariantUtils.CreateFrom<Color>(ref this._targetColor);
      return true;
    }
    if (StringName.op_Equality(ref name, NSmokyVignetteVfx.PropertyName._highlightColor))
    {
      value = VariantUtils.CreateFrom<Color>(ref this._highlightColor);
      return true;
    }
    if (!StringName.op_Equality(ref name, NSmokyVignetteVfx.PropertyName._tween))
      return ((GodotObject) this).GetGodotClassPropertyValue(ref name, ref value);
    value = VariantUtils.CreateFrom<Tween>(ref this._tween);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NSmokyVignetteVfx.PropertyName._highlights, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 20L, NSmokyVignetteVfx.PropertyName._targetColor, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 20L, NSmokyVignetteVfx.PropertyName._highlightColor, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NSmokyVignetteVfx.PropertyName._tween, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).SaveGodotObjectData(info);
    info.AddProperty(NSmokyVignetteVfx.PropertyName._highlights, Variant.From<Control>(ref this._highlights));
    info.AddProperty(NSmokyVignetteVfx.PropertyName._targetColor, Variant.From<Color>(ref this._targetColor));
    info.AddProperty(NSmokyVignetteVfx.PropertyName._highlightColor, Variant.From<Color>(ref this._highlightColor));
    info.AddProperty(NSmokyVignetteVfx.PropertyName._tween, Variant.From<Tween>(ref this._tween));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NSmokyVignetteVfx.PropertyName._highlights, ref variant1))
      this._highlights = ((Variant) ref variant1).As<Control>();
    Variant variant2;
    if (info.TryGetProperty(NSmokyVignetteVfx.PropertyName._targetColor, ref variant2))
      this._targetColor = ((Variant) ref variant2).As<Color>();
    Variant variant3;
    if (info.TryGetProperty(NSmokyVignetteVfx.PropertyName._highlightColor, ref variant3))
      this._highlightColor = ((Variant) ref variant3).As<Color>();
    Variant variant4;
    if (!info.TryGetProperty(NSmokyVignetteVfx.PropertyName._tween, ref variant4))
      return;
    this._tween = ((Variant) ref variant4).As<Tween>();
  }

  public class MethodName : Control.MethodName
  {
    public static readonly StringName Create = StringName.op_Implicit(nameof (Create));
    public static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName Reset = StringName.op_Implicit(nameof (Reset));
    public static readonly StringName _ExitTree = StringName.op_Implicit(nameof (_ExitTree));
  }

  public class PropertyName : Control.PropertyName
  {
    public static readonly StringName _highlights = StringName.op_Implicit(nameof (_highlights));
    public static readonly StringName _targetColor = StringName.op_Implicit(nameof (_targetColor));
    public static readonly StringName _highlightColor = StringName.op_Implicit(nameof (_highlightColor));
    public static readonly StringName _tween = StringName.op_Implicit(nameof (_tween));
  }

  public class SignalName : Control.SignalName
  {
  }
}
