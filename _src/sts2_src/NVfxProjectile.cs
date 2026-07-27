// Decompiled with JetBrains decompiler
// Type: NVfxProjectile
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using System.Collections.Generic;
using System.ComponentModel;

#nullable enable
[ScriptPath("res://src/Core/Nodes/Vfx/Utilities/NVfxProjectile.cs")]
public class NVfxProjectile : Node2D
{
  [Export]
  private Node2D? _projectileHead;
  [Export]
  private GpuParticles2D[] _particles;
  [Export]
  private bool _alignToVelocity;

  public bool AlignToVelocity => this._alignToVelocity;

  public void SetEmitting(bool emitting)
  {
    if (this._projectileHead != null)
      ((CanvasItem) this._projectileHead).Visible = emitting;
    for (int index = 0; index < this._particles.Length; ++index)
      this._particles[index].Emitting = emitting;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(1)
    {
      new MethodInfo(NVfxProjectile.MethodName.SetEmitting, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 1L, StringName.op_Implicit("emitting"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (!StringName.op_Equality(ref method, NVfxProjectile.MethodName.SetEmitting) || ((NativeVariantPtrArgs) ref args).Count != 1)
      return base.InvokeGodotClassMethod(ref method, args, ref ret);
    this.SetEmitting(VariantUtils.ConvertTo<bool>(ref ((NativeVariantPtrArgs) ref args)[0]));
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NVfxProjectile.MethodName.SetEmitting) || base.HasGodotClassMethod(ref method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NVfxProjectile.PropertyName._projectileHead))
    {
      this._projectileHead = VariantUtils.ConvertTo<Node2D>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NVfxProjectile.PropertyName._particles))
    {
      this._particles = VariantUtils.ConvertToSystemArrayOfGodotObject<GpuParticles2D>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NVfxProjectile.PropertyName._alignToVelocity))
      return ((GodotObject) this).SetGodotClassPropertyValue(ref name, ref value);
    this._alignToVelocity = VariantUtils.ConvertTo<bool>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NVfxProjectile.PropertyName.AlignToVelocity))
    {
      ref godot_variant local = ref value;
      bool alignToVelocity = this.AlignToVelocity;
      godot_variant from = VariantUtils.CreateFrom<bool>(ref alignToVelocity);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NVfxProjectile.PropertyName._projectileHead))
    {
      value = VariantUtils.CreateFrom<Node2D>(ref this._projectileHead);
      return true;
    }
    if (StringName.op_Equality(ref name, NVfxProjectile.PropertyName._particles))
    {
      value = VariantUtils.CreateFromSystemArrayOfGodotObject((GodotObject[]) this._particles);
      return true;
    }
    if (!StringName.op_Equality(ref name, NVfxProjectile.PropertyName._alignToVelocity))
      return ((GodotObject) this).GetGodotClassPropertyValue(ref name, ref value);
    value = VariantUtils.CreateFrom<bool>(ref this._alignToVelocity);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NVfxProjectile.PropertyName._projectileHead, (PropertyHint) 34L, "Node2D", (PropertyUsageFlags) 4102L, true),
      new PropertyInfo((Variant.Type) 28L, NVfxProjectile.PropertyName._particles, (PropertyHint) 23L, "24/34:GPUParticles2D", (PropertyUsageFlags) 4102L, true),
      new PropertyInfo((Variant.Type) 1L, NVfxProjectile.PropertyName._alignToVelocity, (PropertyHint) 0L, "", (PropertyUsageFlags) 4102L, true),
      new PropertyInfo((Variant.Type) 1L, NVfxProjectile.PropertyName.AlignToVelocity, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).SaveGodotObjectData(info);
    info.AddProperty(NVfxProjectile.PropertyName._projectileHead, Variant.From<Node2D>(ref this._projectileHead));
    info.AddProperty(NVfxProjectile.PropertyName._particles, Variant.CreateFrom((GodotObject[]) this._particles));
    info.AddProperty(NVfxProjectile.PropertyName._alignToVelocity, Variant.From<bool>(ref this._alignToVelocity));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NVfxProjectile.PropertyName._projectileHead, ref variant1))
      this._projectileHead = ((Variant) ref variant1).As<Node2D>();
    Variant variant2;
    if (info.TryGetProperty(NVfxProjectile.PropertyName._particles, ref variant2))
      this._particles = ((Variant) ref variant2).AsGodotObjectArray<GpuParticles2D>();
    Variant variant3;
    if (!info.TryGetProperty(NVfxProjectile.PropertyName._alignToVelocity, ref variant3))
      return;
    this._alignToVelocity = ((Variant) ref variant3).As<bool>();
  }

  public class MethodName : Node2D.MethodName
  {
    public static readonly StringName SetEmitting = StringName.op_Implicit(nameof (SetEmitting));
  }

  public class PropertyName : Node2D.PropertyName
  {
    public static readonly StringName AlignToVelocity = StringName.op_Implicit(nameof (AlignToVelocity));
    public static readonly StringName _projectileHead = StringName.op_Implicit(nameof (_projectileHead));
    public static readonly StringName _particles = StringName.op_Implicit(nameof (_particles));
    public static readonly StringName _alignToVelocity = StringName.op_Implicit(nameof (_alignToVelocity));
  }

  public class SignalName : Node2D.SignalName
  {
  }
}
