// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Vfx.NAdditiveOverlayVfx
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.TestSupport;
using System;
using System.Collections.Generic;
using System.ComponentModel;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Vfx;

[ScriptPath("res://src/Core/Nodes/Vfx/NAdditiveOverlayVfx.cs")]
public class NAdditiveOverlayVfx : ColorRect
{
  private static readonly string _scenePath = SceneHelper.GetScenePath("vfx/additive_overlay_vfx");
  private VfxColor _vfxColor;
  private Tween? _tween;

  public static NAdditiveOverlayVfx? Create(VfxColor vfxColor = VfxColor.Red)
  {
    if (TestMode.IsOn)
      return (NAdditiveOverlayVfx) null;
    NAdditiveOverlayVfx nadditiveOverlayVfx = PreloadManager.Cache.GetScene(NAdditiveOverlayVfx._scenePath).Instantiate<NAdditiveOverlayVfx>((PackedScene.GenEditState) 0L);
    nadditiveOverlayVfx._vfxColor = vfxColor;
    return nadditiveOverlayVfx;
  }

  public override void _Ready()
  {
    this.SetVfxColor();
    this._tween = ((Node) this).CreateTween();
    this._tween.TweenProperty((GodotObject) this, NodePath.op_Implicit("modulate:a"), Variant.op_Implicit(0.1f), 0.5).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 5L);
    this._tween.TweenInterval(0.5);
    this._tween.TweenProperty((GodotObject) this, NodePath.op_Implicit("modulate:a"), Variant.op_Implicit(0.0f), 0.5);
    this._tween.Finished += new Action(this.OnTweenFinished);
  }

  private void SetVfxColor()
  {
    switch (this._vfxColor)
    {
      case VfxColor.Red:
        break;
      case VfxColor.Green:
        ((CanvasItem) this).Modulate = new Color("00ff1500");
        break;
      case VfxColor.Blue:
        ((CanvasItem) this).Modulate = new Color("001aff00");
        break;
      case VfxColor.Purple:
        ((CanvasItem) this).Modulate = new Color("b300ff00");
        break;
      case VfxColor.Black:
        break;
      case VfxColor.White:
        ((CanvasItem) this).Modulate = new Color("ffffff00");
        break;
      case VfxColor.Cyan:
        ((CanvasItem) this).Modulate = new Color("00fffb00");
        break;
      case VfxColor.Gold:
        ((CanvasItem) this).Modulate = new Color("b17e0000");
        break;
      default:
        throw new ArgumentOutOfRangeException();
    }
  }

  private void OnTweenFinished() => ((Node) this).QueueFreeSafely();

  public override void _ExitTree()
  {
    this._tween?.Kill();
    if (this._tween == null)
      return;
    this._tween.Finished -= new Action(this.OnTweenFinished);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(5)
    {
      new MethodInfo(NAdditiveOverlayVfx.MethodName.Create, new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("ColorRect"), false), (MethodFlags) 33L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 2L, StringName.op_Implicit("vfxColor"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NAdditiveOverlayVfx.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NAdditiveOverlayVfx.MethodName.SetVfxColor, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NAdditiveOverlayVfx.MethodName.OnTweenFinished, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NAdditiveOverlayVfx.MethodName._ExitTree, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NAdditiveOverlayVfx.MethodName.Create) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      NAdditiveOverlayVfx nadditiveOverlayVfx = NAdditiveOverlayVfx.Create(VariantUtils.ConvertTo<VfxColor>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = VariantUtils.CreateFrom<NAdditiveOverlayVfx>(ref nadditiveOverlayVfx);
      return true;
    }
    if (StringName.op_Equality(ref method, NAdditiveOverlayVfx.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NAdditiveOverlayVfx.MethodName.SetVfxColor) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.SetVfxColor();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NAdditiveOverlayVfx.MethodName.OnTweenFinished) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnTweenFinished();
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NAdditiveOverlayVfx.MethodName._ExitTree) || ((NativeVariantPtrArgs) ref args).Count != 0)
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
    if (StringName.op_Equality(ref method, NAdditiveOverlayVfx.MethodName.Create) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      NAdditiveOverlayVfx nadditiveOverlayVfx = NAdditiveOverlayVfx.Create(VariantUtils.ConvertTo<VfxColor>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = VariantUtils.CreateFrom<NAdditiveOverlayVfx>(ref nadditiveOverlayVfx);
      return true;
    }
    ret = new godot_variant();
    return false;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NAdditiveOverlayVfx.MethodName.Create) || StringName.op_Equality(ref method, NAdditiveOverlayVfx.MethodName._Ready) || StringName.op_Equality(ref method, NAdditiveOverlayVfx.MethodName.SetVfxColor) || StringName.op_Equality(ref method, NAdditiveOverlayVfx.MethodName.OnTweenFinished) || StringName.op_Equality(ref method, NAdditiveOverlayVfx.MethodName._ExitTree) || base.HasGodotClassMethod(ref method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NAdditiveOverlayVfx.PropertyName._vfxColor))
    {
      this._vfxColor = VariantUtils.ConvertTo<VfxColor>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NAdditiveOverlayVfx.PropertyName._tween))
      return ((GodotObject) this).SetGodotClassPropertyValue(ref name, ref value);
    this._tween = VariantUtils.ConvertTo<Tween>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NAdditiveOverlayVfx.PropertyName._vfxColor))
    {
      value = VariantUtils.CreateFrom<VfxColor>(ref this._vfxColor);
      return true;
    }
    if (!StringName.op_Equality(ref name, NAdditiveOverlayVfx.PropertyName._tween))
      return ((GodotObject) this).GetGodotClassPropertyValue(ref name, ref value);
    value = VariantUtils.CreateFrom<Tween>(ref this._tween);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 2L, NAdditiveOverlayVfx.PropertyName._vfxColor, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NAdditiveOverlayVfx.PropertyName._tween, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).SaveGodotObjectData(info);
    info.AddProperty(NAdditiveOverlayVfx.PropertyName._vfxColor, Variant.From<VfxColor>(ref this._vfxColor));
    info.AddProperty(NAdditiveOverlayVfx.PropertyName._tween, Variant.From<Tween>(ref this._tween));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NAdditiveOverlayVfx.PropertyName._vfxColor, ref variant1))
      this._vfxColor = ((Variant) ref variant1).As<VfxColor>();
    Variant variant2;
    if (!info.TryGetProperty(NAdditiveOverlayVfx.PropertyName._tween, ref variant2))
      return;
    this._tween = ((Variant) ref variant2).As<Tween>();
  }

  public class MethodName : ColorRect.MethodName
  {
    public static readonly StringName Create = StringName.op_Implicit(nameof (Create));
    public static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName SetVfxColor = StringName.op_Implicit(nameof (SetVfxColor));
    public static readonly StringName OnTweenFinished = StringName.op_Implicit(nameof (OnTweenFinished));
    public static readonly StringName _ExitTree = StringName.op_Implicit(nameof (_ExitTree));
  }

  public class PropertyName : ColorRect.PropertyName
  {
    public static readonly StringName _vfxColor = StringName.op_Implicit(nameof (_vfxColor));
    public static readonly StringName _tween = StringName.op_Implicit(nameof (_tween));
  }

  public class SignalName : ColorRect.SignalName
  {
  }
}
