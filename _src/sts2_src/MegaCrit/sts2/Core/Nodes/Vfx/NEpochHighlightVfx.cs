// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Vfx.NEpochHighlightVfx
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Helpers;
using System.Collections.Generic;
using System.ComponentModel;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Vfx;

[ScriptPath("res://src/Core/Nodes/Vfx/NEpochHighlightVfx.cs")]
public class NEpochHighlightVfx : Control
{
  public static readonly string scenePath = SceneHelper.GetScenePath("vfx/epoch_highlight_vfx");

  public static NEpochHighlightVfx Create()
  {
    return PreloadManager.Cache.GetScene(NEpochHighlightVfx.scenePath).Instantiate<NEpochHighlightVfx>((PackedScene.GenEditState) 0L);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(1)
    {
      new MethodInfo(NEpochHighlightVfx.MethodName.Create, new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false), (MethodFlags) 33L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (!StringName.op_Equality(ref method, NEpochHighlightVfx.MethodName.Create) || ((NativeVariantPtrArgs) ref args).Count != 0)
      return base.InvokeGodotClassMethod(ref method, args, ref ret);
    NEpochHighlightVfx nepochHighlightVfx = NEpochHighlightVfx.Create();
    ret = VariantUtils.CreateFrom<NEpochHighlightVfx>(ref nepochHighlightVfx);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static bool InvokeGodotClassStaticMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NEpochHighlightVfx.MethodName.Create) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      NEpochHighlightVfx nepochHighlightVfx = NEpochHighlightVfx.Create();
      ret = VariantUtils.CreateFrom<NEpochHighlightVfx>(ref nepochHighlightVfx);
      return true;
    }
    ret = new godot_variant();
    return false;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NEpochHighlightVfx.MethodName.Create) || base.HasGodotClassMethod(ref method);
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

  public class MethodName : Control.MethodName
  {
    public static readonly StringName Create = StringName.op_Implicit(nameof (Create));
  }

  public class PropertyName : Control.PropertyName
  {
  }

  public class SignalName : Control.SignalName
  {
  }
}
