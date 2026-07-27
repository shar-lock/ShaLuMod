// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Vfx.NLineBurstVfx
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.TestSupport;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Vfx;

[ScriptPath("res://src/Core/Nodes/Vfx/NLineBurstVfx.cs")]
public class NLineBurstVfx : GpuParticles2D
{
  public static IEnumerable<string> AssetPaths
  {
    get
    {
      return (IEnumerable<string>) new \u003C\u003Ez__ReadOnlySingleElementList<string>(NLineBurstVfx.ScenePath);
    }
  }

  private static string ScenePath => SceneHelper.GetScenePath("vfx/vfx_line_burst");

  public static NLineBurstVfx? Create(Creature target)
  {
    if (TestMode.IsOn)
      return (NLineBurstVfx) null;
    NLineBurstVfx nlineBurstVfx = PreloadManager.Cache.GetScene(NLineBurstVfx.ScenePath).Instantiate<NLineBurstVfx>((PackedScene.GenEditState) 0L);
    ((Node2D) nlineBurstVfx).GlobalPosition = NCombatRoom.Instance.GetCreatureNode(target).VfxSpawnPosition;
    return nlineBurstVfx;
  }

  public static NLineBurstVfx? Create(Vector2 position)
  {
    if (TestMode.IsOn)
      return (NLineBurstVfx) null;
    NLineBurstVfx nlineBurstVfx = PreloadManager.Cache.GetScene(NLineBurstVfx.ScenePath).Instantiate<NLineBurstVfx>((PackedScene.GenEditState) 0L);
    ((Node2D) nlineBurstVfx).GlobalPosition = position;
    return nlineBurstVfx;
  }

  public override void _Ready()
  {
    this.Emitting = true;
    TaskHelper.RunSafely(this.DeleteAfterComplete());
  }

  private async Task DeleteAfterComplete()
  {
    await ((GodotObject) this).AwaitSignal(GpuParticles2D.SignalName.Finished, (Node) this);
    ((Node) this).QueueFreeSafely();
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(2)
    {
      new MethodInfo(NLineBurstVfx.MethodName.Create, new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("GPUParticles2D"), false), (MethodFlags) 33L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 5L, StringName.op_Implicit("position"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NLineBurstVfx.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NLineBurstVfx.MethodName.Create) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      NLineBurstVfx nlineBurstVfx = NLineBurstVfx.Create(VariantUtils.ConvertTo<Vector2>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = VariantUtils.CreateFrom<NLineBurstVfx>(ref nlineBurstVfx);
      return true;
    }
    if (!StringName.op_Equality(ref method, NLineBurstVfx.MethodName._Ready) || ((NativeVariantPtrArgs) ref args).Count != 0)
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
    if (StringName.op_Equality(ref method, NLineBurstVfx.MethodName.Create) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      NLineBurstVfx nlineBurstVfx = NLineBurstVfx.Create(VariantUtils.ConvertTo<Vector2>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = VariantUtils.CreateFrom<NLineBurstVfx>(ref nlineBurstVfx);
      return true;
    }
    ret = new godot_variant();
    return false;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NLineBurstVfx.MethodName.Create) || StringName.op_Equality(ref method, NLineBurstVfx.MethodName._Ready) || base.HasGodotClassMethod(ref method);
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

  public class MethodName : GpuParticles2D.MethodName
  {
    public static readonly StringName Create = StringName.op_Implicit(nameof (Create));
    public static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
  }

  public class PropertyName : GpuParticles2D.PropertyName
  {
  }

  public class SignalName : GpuParticles2D.SignalName
  {
  }
}
