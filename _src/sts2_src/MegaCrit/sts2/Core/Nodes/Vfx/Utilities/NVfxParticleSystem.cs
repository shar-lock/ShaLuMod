// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Vfx.Utilities.NVfxParticleSystem
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Vfx.Utilities;

[ScriptPath("res://src/Core/Nodes/Vfx/Utilities/NVfxParticleSystem.cs")]
public class NVfxParticleSystem : Node2D
{
  [Export]
  private float _lifetime = 1f;

  public override void _Ready()
  {
    this.TryPlayParticles((Node) this);
    ((GodotObject) ((Node) this).GetTree().CreateTimer((double) this._lifetime, true, false, false)).Connect(SceneTreeTimer.SignalName.Timeout, Callable.From(new Action(this.AfterExpired)), 0U);
  }

  private void TryPlayParticles(Node node)
  {
    switch (node)
    {
      case CpuParticles2D cpuParticles2D:
        cpuParticles2D.Emitting = true;
        break;
      case GpuParticles2D gpuParticles2D:
        gpuParticles2D.Emitting = true;
        break;
    }
    foreach (Node child in node.GetChildren(false))
      this.TryPlayParticles(child);
  }

  private void AfterExpired()
  {
    if (!GodotObject.IsInstanceValid((GodotObject) this))
      return;
    ((Node) this).QueueFreeSafely();
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(3)
    {
      new MethodInfo(NVfxParticleSystem.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NVfxParticleSystem.MethodName.TryPlayParticles, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("node"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Node"), false)
      }, (List<Variant>) null),
      new MethodInfo(NVfxParticleSystem.MethodName.AfterExpired, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NVfxParticleSystem.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NVfxParticleSystem.MethodName.TryPlayParticles) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.TryPlayParticles(VariantUtils.ConvertTo<Node>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NVfxParticleSystem.MethodName.AfterExpired) || ((NativeVariantPtrArgs) ref args).Count != 0)
      return base.InvokeGodotClassMethod(ref method, args, ref ret);
    this.AfterExpired();
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NVfxParticleSystem.MethodName._Ready) || StringName.op_Equality(ref method, NVfxParticleSystem.MethodName.TryPlayParticles) || StringName.op_Equality(ref method, NVfxParticleSystem.MethodName.AfterExpired) || base.HasGodotClassMethod(ref method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (!StringName.op_Equality(ref name, NVfxParticleSystem.PropertyName._lifetime))
      return ((GodotObject) this).SetGodotClassPropertyValue(ref name, ref value);
    this._lifetime = VariantUtils.ConvertTo<float>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (!StringName.op_Equality(ref name, NVfxParticleSystem.PropertyName._lifetime))
      return ((GodotObject) this).GetGodotClassPropertyValue(ref name, ref value);
    value = VariantUtils.CreateFrom<float>(ref this._lifetime);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 3L, NVfxParticleSystem.PropertyName._lifetime, (PropertyHint) 0L, "", (PropertyUsageFlags) 4102L, true)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).SaveGodotObjectData(info);
    info.AddProperty(NVfxParticleSystem.PropertyName._lifetime, Variant.From<float>(ref this._lifetime));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).RestoreGodotObjectData(info);
    Variant variant;
    if (!info.TryGetProperty(NVfxParticleSystem.PropertyName._lifetime, ref variant))
      return;
    this._lifetime = ((Variant) ref variant).As<float>();
  }

  public class MethodName : Node2D.MethodName
  {
    public static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName TryPlayParticles = StringName.op_Implicit(nameof (TryPlayParticles));
    public static readonly StringName AfterExpired = StringName.op_Implicit(nameof (AfterExpired));
  }

  public class PropertyName : Node2D.PropertyName
  {
    public static readonly StringName _lifetime = StringName.op_Implicit(nameof (_lifetime));
  }

  public class SignalName : Node2D.SignalName
  {
  }
}
