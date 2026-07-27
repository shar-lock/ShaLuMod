// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Vfx.NBlockBrokenVfx
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

[ScriptPath("res://src/Core/Nodes/Vfx/NBlockBrokenVfx.cs")]
public class NBlockBrokenVfx : Sprite2D
{
  private const string _scenePath = "res://scenes/vfx/vfx_block_broken.tscn";

  public static IEnumerable<string> AssetPaths
  {
    get
    {
      return (IEnumerable<string>) new \u003C\u003Ez__ReadOnlySingleElementList<string>("res://scenes/vfx/vfx_block_broken.tscn");
    }
  }

  public override void _Ready()
  {
    ((GodotObject) ((Node) this).GetNode<AnimationPlayer>(NodePath.op_Implicit("AnimationPlayer"))).Connect(AnimationMixer.SignalName.AnimationFinished, Callable.From<StringName>(new Action<StringName>(this.OnAnimationFinished)), 0U);
  }

  private void OnAnimationFinished(StringName _) => ((Node) this).QueueFreeSafely();

  public static NBlockBrokenVfx? Create()
  {
    return TestMode.IsOn ? (NBlockBrokenVfx) null : PreloadManager.Cache.GetScene("res://scenes/vfx/vfx_block_broken.tscn").Instantiate<NBlockBrokenVfx>((PackedScene.GenEditState) 0L);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(3)
    {
      new MethodInfo(NBlockBrokenVfx.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NBlockBrokenVfx.MethodName.OnAnimationFinished, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 21L, StringName.op_Implicit("_"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NBlockBrokenVfx.MethodName.Create, new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Sprite2D"), false), (MethodFlags) 33L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NBlockBrokenVfx.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NBlockBrokenVfx.MethodName.OnAnimationFinished) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.OnAnimationFinished(VariantUtils.ConvertTo<StringName>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NBlockBrokenVfx.MethodName.Create) || ((NativeVariantPtrArgs) ref args).Count != 0)
      return base.InvokeGodotClassMethod(ref method, args, ref ret);
    NBlockBrokenVfx nblockBrokenVfx = NBlockBrokenVfx.Create();
    ret = VariantUtils.CreateFrom<NBlockBrokenVfx>(ref nblockBrokenVfx);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static bool InvokeGodotClassStaticMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NBlockBrokenVfx.MethodName.Create) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      NBlockBrokenVfx nblockBrokenVfx = NBlockBrokenVfx.Create();
      ret = VariantUtils.CreateFrom<NBlockBrokenVfx>(ref nblockBrokenVfx);
      return true;
    }
    ret = new godot_variant();
    return false;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NBlockBrokenVfx.MethodName._Ready) || StringName.op_Equality(ref method, NBlockBrokenVfx.MethodName.OnAnimationFinished) || StringName.op_Equality(ref method, NBlockBrokenVfx.MethodName.Create) || base.HasGodotClassMethod(ref method);
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

  public class MethodName : Sprite2D.MethodName
  {
    public static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName OnAnimationFinished = StringName.op_Implicit(nameof (OnAnimationFinished));
    public static readonly StringName Create = StringName.op_Implicit(nameof (Create));
  }

  public class PropertyName : Sprite2D.PropertyName
  {
  }

  public class SignalName : Sprite2D.SignalName
  {
  }
}
