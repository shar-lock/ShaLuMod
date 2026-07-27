// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Vfx.NDesaturateTransitionVfx
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

[ScriptPath("res://src/Core/Nodes/Vfx/NDesaturateTransitionVfx.cs")]
public class NDesaturateTransitionVfx : Node
{
  private Tween? _tween;

  public static IEnumerable<string> AssetPaths
  {
    get
    {
      return (IEnumerable<string>) new \u003C\u003Ez__ReadOnlySingleElementList<string>(NDesaturateTransitionVfx.ScenePath);
    }
  }

  private static string ScenePath => SceneHelper.GetScenePath("vfx/desaturate_transition_vfx");

  public static NDesaturateTransitionVfx? Create()
  {
    return TestMode.IsOn ? (NDesaturateTransitionVfx) null : PreloadManager.Cache.GetScene(NDesaturateTransitionVfx.ScenePath).Instantiate<NDesaturateTransitionVfx>((PackedScene.GenEditState) 0L);
  }

  public override void _Ready() => this.Animate();

  public override void _ExitTree()
  {
    if (this._tween == null || !this._tween.IsRunning())
      return;
    this._tween.Kill();
    NGame.Instance.DeactivateWorldEnvironment();
  }

  private void Animate()
  {
    WorldEnvironment worldEnvironment = NGame.Instance.ActivateWorldEnvironment();
    this._tween = this.CreateTween().SetParallel(true);
    this._tween.TweenProperty((GodotObject) worldEnvironment, NodePath.op_Implicit("environment:tonemap_exposure"), Variant.op_Implicit(1f), 1.0);
    this._tween.TweenProperty((GodotObject) worldEnvironment, NodePath.op_Implicit("environment:adjustment_brightness"), Variant.op_Implicit(0.1f), 1.0);
    this._tween.TweenProperty((GodotObject) worldEnvironment, NodePath.op_Implicit("environment:adjustment_contrast"), Variant.op_Implicit(0.7f), 1.0);
    this._tween.TweenProperty((GodotObject) worldEnvironment, NodePath.op_Implicit("environment:adjustment_saturation"), Variant.op_Implicit(0.25f), 1.0);
    this._tween.Chain();
    this._tween.TweenProperty((GodotObject) worldEnvironment, NodePath.op_Implicit("environment:adjustment_contrast"), Variant.op_Implicit(0.8f), 1.0);
    this._tween.Chain();
    this._tween.TweenProperty((GodotObject) worldEnvironment, NodePath.op_Implicit("environment:adjustment_brightness"), Variant.op_Implicit(1f), 1.0);
    this._tween.TweenProperty((GodotObject) worldEnvironment, NodePath.op_Implicit("environment:adjustment_contrast"), Variant.op_Implicit(1f), 1.0);
    this._tween.TweenProperty((GodotObject) worldEnvironment, NodePath.op_Implicit("environment:adjustment_saturation"), Variant.op_Implicit(1f), 1.0);
    this._tween.Chain().TweenCallback(Callable.From((Action) (() =>
    {
      NGame.Instance.DeactivateWorldEnvironment();
      this.QueueFreeSafely();
    })));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(4)
    {
      new MethodInfo(NDesaturateTransitionVfx.MethodName.Create, new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Node"), false), (MethodFlags) 33L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NDesaturateTransitionVfx.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NDesaturateTransitionVfx.MethodName._ExitTree, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NDesaturateTransitionVfx.MethodName.Animate, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NDesaturateTransitionVfx.MethodName.Create) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      NDesaturateTransitionVfx ndesaturateTransitionVfx = NDesaturateTransitionVfx.Create();
      ret = VariantUtils.CreateFrom<NDesaturateTransitionVfx>(ref ndesaturateTransitionVfx);
      return true;
    }
    if (StringName.op_Equality(ref method, NDesaturateTransitionVfx.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      base._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NDesaturateTransitionVfx.MethodName._ExitTree) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      base._ExitTree();
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NDesaturateTransitionVfx.MethodName.Animate) || ((NativeVariantPtrArgs) ref args).Count != 0)
      return base.InvokeGodotClassMethod(ref method, args, ref ret);
    this.Animate();
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static bool InvokeGodotClassStaticMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NDesaturateTransitionVfx.MethodName.Create) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      NDesaturateTransitionVfx ndesaturateTransitionVfx = NDesaturateTransitionVfx.Create();
      ret = VariantUtils.CreateFrom<NDesaturateTransitionVfx>(ref ndesaturateTransitionVfx);
      return true;
    }
    ret = new godot_variant();
    return false;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NDesaturateTransitionVfx.MethodName.Create) || StringName.op_Equality(ref method, NDesaturateTransitionVfx.MethodName._Ready) || StringName.op_Equality(ref method, NDesaturateTransitionVfx.MethodName._ExitTree) || StringName.op_Equality(ref method, NDesaturateTransitionVfx.MethodName.Animate) || base.HasGodotClassMethod(ref method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (!StringName.op_Equality(ref name, NDesaturateTransitionVfx.PropertyName._tween))
      return ((GodotObject) this).SetGodotClassPropertyValue(ref name, ref value);
    this._tween = VariantUtils.ConvertTo<Tween>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (!StringName.op_Equality(ref name, NDesaturateTransitionVfx.PropertyName._tween))
      return ((GodotObject) this).GetGodotClassPropertyValue(ref name, ref value);
    value = VariantUtils.CreateFrom<Tween>(ref this._tween);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NDesaturateTransitionVfx.PropertyName._tween, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).SaveGodotObjectData(info);
    info.AddProperty(NDesaturateTransitionVfx.PropertyName._tween, Variant.From<Tween>(ref this._tween));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).RestoreGodotObjectData(info);
    Variant variant;
    if (!info.TryGetProperty(NDesaturateTransitionVfx.PropertyName._tween, ref variant))
      return;
    this._tween = ((Variant) ref variant).As<Tween>();
  }

  public class MethodName : Node.MethodName
  {
    public static readonly StringName Create = StringName.op_Implicit(nameof (Create));
    public static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName _ExitTree = StringName.op_Implicit(nameof (_ExitTree));
    public static readonly StringName Animate = StringName.op_Implicit(nameof (Animate));
  }

  public class PropertyName : Node.PropertyName
  {
    public static readonly StringName _tween = StringName.op_Implicit(nameof (_tween));
  }

  public class SignalName : Node.SignalName
  {
  }
}
