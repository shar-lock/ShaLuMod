// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Vfx.NPotionFlashVfx
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.Potions;
using MegaCrit.Sts2.Core.TestSupport;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Vfx;

[ScriptPath("res://src/Core/Nodes/Vfx/NPotionFlashVfx.cs")]
public class NPotionFlashVfx : Node2D
{
  private Control _flash;

  public static string ScenePath => SceneHelper.GetScenePath("vfx/vfx_potion_flash");

  public static NPotionFlashVfx? Create(NPotion originPotion)
  {
    if (TestMode.IsOn)
      return (NPotionFlashVfx) null;
    NPotionFlashVfx npotionFlashVfx = PreloadManager.Cache.GetScene(NPotionFlashVfx.ScenePath).Instantiate<NPotionFlashVfx>((PackedScene.GenEditState) 0L);
    npotionFlashVfx._flash = (Control) ((Node) originPotion).Duplicate(15);
    ulong instanceId = ((GodotObject) npotionFlashVfx._flash).GetInstanceId();
    ((GodotObject) npotionFlashVfx._flash).SetScript(new Variant());
    npotionFlashVfx._flash = (Control) GodotObject.InstanceFromId(instanceId);
    return npotionFlashVfx;
  }

  public override void _Ready()
  {
    ((Node) ((Node) this).GetNode<SubViewport>(NodePath.op_Implicit("SubViewport"))).AddChildSafely((Node) this._flash);
    this._flash.Position = Vector2.Zero;
    TaskHelper.RunSafely(this.FlashAndFree());
  }

  private async Task FlashAndFree()
  {
    CpuParticles2D node = ((Node) this).GetNode<CpuParticles2D>(NodePath.op_Implicit("%Flash"));
    node.Emitting = true;
    await ((GodotObject) node).AwaitSignal(CpuParticles2D.SignalName.Finished, (Node) this);
    ((Node) this).QueueFreeSafely();
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(2)
    {
      new MethodInfo(NPotionFlashVfx.MethodName.Create, new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Node2D"), false), (MethodFlags) 33L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("originPotion"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false)
      }, (List<Variant>) null),
      new MethodInfo(NPotionFlashVfx.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NPotionFlashVfx.MethodName.Create) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      NPotionFlashVfx npotionFlashVfx = NPotionFlashVfx.Create(VariantUtils.ConvertTo<NPotion>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = VariantUtils.CreateFrom<NPotionFlashVfx>(ref npotionFlashVfx);
      return true;
    }
    if (!StringName.op_Equality(ref method, NPotionFlashVfx.MethodName._Ready) || ((NativeVariantPtrArgs) ref args).Count != 0)
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
    if (StringName.op_Equality(ref method, NPotionFlashVfx.MethodName.Create) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      NPotionFlashVfx npotionFlashVfx = NPotionFlashVfx.Create(VariantUtils.ConvertTo<NPotion>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = VariantUtils.CreateFrom<NPotionFlashVfx>(ref npotionFlashVfx);
      return true;
    }
    ret = new godot_variant();
    return false;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NPotionFlashVfx.MethodName.Create) || StringName.op_Equality(ref method, NPotionFlashVfx.MethodName._Ready) || base.HasGodotClassMethod(ref method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (!StringName.op_Equality(ref name, NPotionFlashVfx.PropertyName._flash))
      return ((GodotObject) this).SetGodotClassPropertyValue(ref name, ref value);
    this._flash = VariantUtils.ConvertTo<Control>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (!StringName.op_Equality(ref name, NPotionFlashVfx.PropertyName._flash))
      return ((GodotObject) this).GetGodotClassPropertyValue(ref name, ref value);
    value = VariantUtils.CreateFrom<Control>(ref this._flash);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NPotionFlashVfx.PropertyName._flash, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).SaveGodotObjectData(info);
    info.AddProperty(NPotionFlashVfx.PropertyName._flash, Variant.From<Control>(ref this._flash));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).RestoreGodotObjectData(info);
    Variant variant;
    if (!info.TryGetProperty(NPotionFlashVfx.PropertyName._flash, ref variant))
      return;
    this._flash = ((Variant) ref variant).As<Control>();
  }

  public class MethodName : Node2D.MethodName
  {
    public static readonly StringName Create = StringName.op_Implicit(nameof (Create));
    public static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
  }

  public class PropertyName : Node2D.PropertyName
  {
    public static readonly StringName _flash = StringName.op_Implicit(nameof (_flash));
  }

  public class SignalName : Node2D.SignalName
  {
  }
}
