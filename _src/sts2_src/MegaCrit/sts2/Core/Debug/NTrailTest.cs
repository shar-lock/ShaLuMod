// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Debug.NTrailTest
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Debug;

[ScriptPath("res://src/Core/Debug/NTrailTest.cs")]
public class NTrailTest : Control
{
  public override void _Ready() => this.DelaySpawn();

  private async Task DelaySpawn()
  {
    await Task.Delay(100);
    NCardTrailVfx child1 = NCardTrailVfx.Create(((Node) this).GetNode<Control>(NodePath.op_Implicit("Ironclad")), SceneHelper.GetScenePath("vfx/card_trail_ironclad"));
    ((Node) this).GetParent().AddChildSafely((Node) child1);
    NCardTrailVfx child2 = NCardTrailVfx.Create(((Node) this).GetNode<Control>(NodePath.op_Implicit("Silent")), SceneHelper.GetScenePath("vfx/card_trail_silent"));
    ((Node) this).GetParent().AddChildSafely((Node) child2);
    NCardTrailVfx child3 = NCardTrailVfx.Create(((Node) this).GetNode<Control>(NodePath.op_Implicit("Defect")), SceneHelper.GetScenePath("vfx/card_trail_defect"));
    ((Node) this).GetParent().AddChildSafely((Node) child3);
    NCardTrailVfx child4 = NCardTrailVfx.Create(((Node) this).GetNode<Control>(NodePath.op_Implicit("Regent")), SceneHelper.GetScenePath("vfx/card_trail_regent"));
    ((Node) this).GetParent().AddChildSafely((Node) child4);
    NCardTrailVfx child5 = NCardTrailVfx.Create(((Node) this).GetNode<Control>(NodePath.op_Implicit("Binder")), SceneHelper.GetScenePath("vfx/card_trail_necrobinder"));
    ((Node) this).GetParent().AddChildSafely((Node) child5);
  }

  public override void _Process(double delta)
  {
    this.GlobalPosition = ((Node) this).GetViewport().GetMousePosition();
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(2)
    {
      new MethodInfo(NTrailTest.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NTrailTest.MethodName._Process, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
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
    if (StringName.op_Equality(ref method, NTrailTest.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NTrailTest.MethodName._Process) || ((NativeVariantPtrArgs) ref args).Count != 1)
      return base.InvokeGodotClassMethod(ref method, args, ref ret);
    ((Node) this)._Process(VariantUtils.ConvertTo<double>(ref ((NativeVariantPtrArgs) ref args)[0]));
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NTrailTest.MethodName._Ready) || StringName.op_Equality(ref method, NTrailTest.MethodName._Process) || base.HasGodotClassMethod(ref method);
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
    public static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName _Process = StringName.op_Implicit(nameof (_Process));
  }

  public class PropertyName : Control.PropertyName
  {
  }

  public class SignalName : Control.SignalName
  {
  }
}
