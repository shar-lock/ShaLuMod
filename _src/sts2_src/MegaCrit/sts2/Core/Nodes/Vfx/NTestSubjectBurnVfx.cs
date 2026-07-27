// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Vfx.NTestSubjectBurnVfx
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

[ScriptPath("res://src/Core/Nodes/Vfx/NTestSubjectBurnVfx.cs")]
public class NTestSubjectBurnVfx : ColorRect
{
  private static readonly string _scenePath = SceneHelper.GetScenePath("vfx/monsters/test_subject_burn_vfx");

  public static NTestSubjectBurnVfx? Create()
  {
    return TestMode.IsOn ? (NTestSubjectBurnVfx) null : PreloadManager.Cache.GetScene(NTestSubjectBurnVfx._scenePath).Instantiate<NTestSubjectBurnVfx>((PackedScene.GenEditState) 0L);
  }

  public override void _Ready()
  {
    ((Node) this)._Ready();
    ((CanvasItem) this).Modulate = Colors.Transparent;
    Tween tween = ((Node) this).CreateTween();
    tween.Chain().TweenInterval(0.25);
    tween.Chain().TweenProperty((GodotObject) this, NodePath.op_Implicit("modulate"), Variant.op_Implicit(Colors.White), 0.30000001192092896);
    tween.Chain().TweenInterval(0.10000000149011612);
    tween.Chain().TweenProperty((GodotObject) this, NodePath.op_Implicit("modulate"), Variant.op_Implicit(Color.op_Multiply(Colors.White, 0.5f)), 0.15000000596046448);
    tween.Chain().TweenProperty((GodotObject) this, NodePath.op_Implicit("modulate"), Variant.op_Implicit(Colors.White), 0.25);
    tween.Chain().TweenInterval(0.34999999403953552);
    tween.Chain().TweenProperty((GodotObject) this, NodePath.op_Implicit("modulate"), Variant.op_Implicit(Colors.Transparent), 0.30000001192092896);
    tween.Chain().TweenCallback(Callable.From(new Action(((GodotTreeExtensions) this).QueueFreeSafely)));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(2)
    {
      new MethodInfo(NTestSubjectBurnVfx.MethodName.Create, new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("ColorRect"), false), (MethodFlags) 33L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NTestSubjectBurnVfx.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NTestSubjectBurnVfx.MethodName.Create) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      NTestSubjectBurnVfx ntestSubjectBurnVfx = NTestSubjectBurnVfx.Create();
      ret = VariantUtils.CreateFrom<NTestSubjectBurnVfx>(ref ntestSubjectBurnVfx);
      return true;
    }
    if (!StringName.op_Equality(ref method, NTestSubjectBurnVfx.MethodName._Ready) || ((NativeVariantPtrArgs) ref args).Count != 0)
      return base.InvokeGodotClassMethod(ref method, args, ref ret);
    ((Node) this)._Ready();
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static bool InvokeGodotClassStaticMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NTestSubjectBurnVfx.MethodName.Create) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      NTestSubjectBurnVfx ntestSubjectBurnVfx = NTestSubjectBurnVfx.Create();
      ret = VariantUtils.CreateFrom<NTestSubjectBurnVfx>(ref ntestSubjectBurnVfx);
      return true;
    }
    ret = new godot_variant();
    return false;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NTestSubjectBurnVfx.MethodName.Create) || StringName.op_Equality(ref method, NTestSubjectBurnVfx.MethodName._Ready) || base.HasGodotClassMethod(ref method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).SaveGodotObjectData(info);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).RestoreGodotObjectData(info);
  }

  public class MethodName : ColorRect.MethodName
  {
    public static readonly StringName Create = StringName.op_Implicit(nameof (Create));
    public static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
  }

  public class PropertyName : ColorRect.PropertyName
  {
  }

  public class SignalName : ColorRect.SignalName
  {
  }
}
