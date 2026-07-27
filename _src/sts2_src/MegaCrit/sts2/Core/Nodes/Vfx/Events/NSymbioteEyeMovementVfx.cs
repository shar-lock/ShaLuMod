// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Vfx.Events.NSymbioteEyeMovementVfx
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using System.Collections.Generic;
using System.ComponentModel;

#nullable disable
namespace MegaCrit.Sts2.Core.Nodes.Vfx.Events;

[ScriptPath("res://src/Core/Nodes/Vfx/Events/NSymbioteEyeMovementVfx.cs")]
public class NSymbioteEyeMovementVfx : PathFollow2D
{
  private const float _speed = 0.1f;

  public override void _Process(double delta) => this.ProgressRatio += (float) delta * 0.1f;

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(1)
    {
      new MethodInfo(NSymbioteEyeMovementVfx.MethodName._Process, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 3L, StringName.op_Implicit("delta"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (!StringName.op_Equality(ref method, NSymbioteEyeMovementVfx.MethodName._Process) || ((NativeVariantPtrArgs) ref args).Count != 1)
      return base.InvokeGodotClassMethod(ref method, args, ref ret);
    ((Node) this)._Process(VariantUtils.ConvertTo<double>(ref ((NativeVariantPtrArgs) ref args)[0]));
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NSymbioteEyeMovementVfx.MethodName._Process) || base.HasGodotClassMethod(ref method);
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

  public class MethodName : PathFollow2D.MethodName
  {
    public static readonly StringName _Process = StringName.op_Implicit(nameof (_Process));
  }

  public class PropertyName : PathFollow2D.PropertyName
  {
  }

  public class SignalName : PathFollow2D.SignalName
  {
  }
}
