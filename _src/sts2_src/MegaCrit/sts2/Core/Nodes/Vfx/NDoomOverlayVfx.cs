// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Vfx.NDoomOverlayVfx
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

[ScriptPath("res://src/Core/Nodes/Vfx/NDoomOverlayVfx.cs")]
public class NDoomOverlayVfx : BackBufferCopy
{
  private static readonly string _scenePath = SceneHelper.GetScenePath("vfx/doom_overlay_vfx");
  private static NDoomOverlayVfx? _instance;
  private Tween? _tween;

  public static NDoomOverlayVfx? GetOrCreate()
  {
    if (TestMode.IsOn)
      return (NDoomOverlayVfx) null;
    if (NDoomOverlayVfx._instance != null)
      NDoomOverlayVfx._instance.PlayVfx();
    else
      NDoomOverlayVfx._instance = PreloadManager.Cache.GetScene(NDoomOverlayVfx._scenePath).Instantiate<NDoomOverlayVfx>((PackedScene.GenEditState) 0L);
    return !GodotObject.IsInstanceValid((GodotObject) NDoomOverlayVfx._instance) ? (NDoomOverlayVfx) null : NDoomOverlayVfx._instance;
  }

  public override void _Ready()
  {
    ((CanvasItem) this).Modulate = Colors.Transparent;
    Control node = ((Node) this).GetNode<Control>(NodePath.op_Implicit("%Rect"));
    StringName size = Control.PropertyName.Size;
    Rect2 viewportRect = ((CanvasItem) this).GetViewportRect();
    Variant variant = Variant.op_Implicit(((Rect2) ref viewportRect).Size);
    ((GodotObject) node).SetDeferred(size, variant);
    this.PlayVfx();
  }

  private void PlayVfx()
  {
    if (!GodotObject.IsInstanceValid((GodotObject) this))
      return;
    this._tween?.Kill();
    this._tween = ((Node) this).CreateTween();
    this._tween.TweenProperty((GodotObject) this, NodePath.op_Implicit("modulate:a"), Variant.op_Implicit(1f), 1.0).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 5L);
    this._tween.TweenInterval(1.2000000476837158);
    this._tween.TweenProperty((GodotObject) this, NodePath.op_Implicit("modulate:a"), Variant.op_Implicit(0.0f), 0.5);
    this._tween.TweenCallback(Callable.From((Action) (() => NDoomOverlayVfx._instance = (NDoomOverlayVfx) null)));
    this._tween.Finished += new Action(this.OnTweenFinished);
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
      new MethodInfo(NDoomOverlayVfx.MethodName.GetOrCreate, new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("BackBufferCopy"), false), (MethodFlags) 33L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NDoomOverlayVfx.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NDoomOverlayVfx.MethodName.PlayVfx, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NDoomOverlayVfx.MethodName.OnTweenFinished, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NDoomOverlayVfx.MethodName._ExitTree, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NDoomOverlayVfx.MethodName.GetOrCreate) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      NDoomOverlayVfx ndoomOverlayVfx = NDoomOverlayVfx.GetOrCreate();
      ret = VariantUtils.CreateFrom<NDoomOverlayVfx>(ref ndoomOverlayVfx);
      return true;
    }
    if (StringName.op_Equality(ref method, NDoomOverlayVfx.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NDoomOverlayVfx.MethodName.PlayVfx) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.PlayVfx();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NDoomOverlayVfx.MethodName.OnTweenFinished) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnTweenFinished();
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NDoomOverlayVfx.MethodName._ExitTree) || ((NativeVariantPtrArgs) ref args).Count != 0)
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
    if (StringName.op_Equality(ref method, NDoomOverlayVfx.MethodName.GetOrCreate) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      NDoomOverlayVfx ndoomOverlayVfx = NDoomOverlayVfx.GetOrCreate();
      ret = VariantUtils.CreateFrom<NDoomOverlayVfx>(ref ndoomOverlayVfx);
      return true;
    }
    ret = new godot_variant();
    return false;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NDoomOverlayVfx.MethodName.GetOrCreate) || StringName.op_Equality(ref method, NDoomOverlayVfx.MethodName._Ready) || StringName.op_Equality(ref method, NDoomOverlayVfx.MethodName.PlayVfx) || StringName.op_Equality(ref method, NDoomOverlayVfx.MethodName.OnTweenFinished) || StringName.op_Equality(ref method, NDoomOverlayVfx.MethodName._ExitTree) || base.HasGodotClassMethod(ref method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (!StringName.op_Equality(ref name, NDoomOverlayVfx.PropertyName._tween))
      return ((GodotObject) this).SetGodotClassPropertyValue(ref name, ref value);
    this._tween = VariantUtils.ConvertTo<Tween>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (!StringName.op_Equality(ref name, NDoomOverlayVfx.PropertyName._tween))
      return ((GodotObject) this).GetGodotClassPropertyValue(ref name, ref value);
    value = VariantUtils.CreateFrom<Tween>(ref this._tween);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NDoomOverlayVfx.PropertyName._tween, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).SaveGodotObjectData(info);
    info.AddProperty(NDoomOverlayVfx.PropertyName._tween, Variant.From<Tween>(ref this._tween));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).RestoreGodotObjectData(info);
    Variant variant;
    if (!info.TryGetProperty(NDoomOverlayVfx.PropertyName._tween, ref variant))
      return;
    this._tween = ((Variant) ref variant).As<Tween>();
  }

  public class MethodName : BackBufferCopy.MethodName
  {
    public static readonly StringName GetOrCreate = StringName.op_Implicit(nameof (GetOrCreate));
    public static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName PlayVfx = StringName.op_Implicit(nameof (PlayVfx));
    public static readonly StringName OnTweenFinished = StringName.op_Implicit(nameof (OnTweenFinished));
    public static readonly StringName _ExitTree = StringName.op_Implicit(nameof (_ExitTree));
  }

  public class PropertyName : BackBufferCopy.PropertyName
  {
    public static readonly StringName _tween = StringName.op_Implicit(nameof (_tween));
  }

  public class SignalName : BackBufferCopy.SignalName
  {
  }
}
