// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Vfx.NKaiserCrabBossExplosionVfx
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Bindings.MegaSpine;
using System;
using System.Collections.Generic;
using System.ComponentModel;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Vfx;

[ScriptPath("res://src/Core/Nodes/Vfx/NKaiserCrabBossExplosionVfx.cs")]
public class NKaiserCrabBossExplosionVfx : Node
{
  private MegaSprite _megaSprite;
  private Node2D _parent;
  private GpuParticles2D _explosionParticles;

  public override void _Ready()
  {
    this._parent = this.GetParent<Node2D>();
    this._explosionParticles = ((Node) this._parent).GetNode<GpuParticles2D>(NodePath.op_Implicit("ExplosionSlot/ExplosionParticles"));
    this._megaSprite = new MegaSprite(Variant.op_Implicit((GodotObject) this.GetParent<Node2D>()));
    this._megaSprite.ConnectAnimationEvent(Callable.From<GodotObject, GodotObject, GodotObject, GodotObject>(new Action<GodotObject, GodotObject, GodotObject, GodotObject>(this.OnAnimationEvent)));
    this._explosionParticles.Emitting = false;
    this._explosionParticles.OneShot = true;
  }

  private void OnAnimationEvent(
    GodotObject _,
    GodotObject __,
    GodotObject ___,
    GodotObject spineEvent)
  {
    if (!(new MegaEvent(Variant.op_Implicit(spineEvent)).GetData().GetEventName() == "left_embers_start"))
      return;
    this.OnLeftEmbersStart();
  }

  private void OnLeftEmbersStart() => this._explosionParticles.Restart();

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(3)
    {
      new MethodInfo(NKaiserCrabBossExplosionVfx.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NKaiserCrabBossExplosionVfx.MethodName.OnAnimationEvent, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("_"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Object"), false),
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("__"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Object"), false),
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("___"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Object"), false),
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("spineEvent"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Object"), false)
      }, (List<Variant>) null),
      new MethodInfo(NKaiserCrabBossExplosionVfx.MethodName.OnLeftEmbersStart, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NKaiserCrabBossExplosionVfx.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      base._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NKaiserCrabBossExplosionVfx.MethodName.OnAnimationEvent) && ((NativeVariantPtrArgs) ref args).Count == 4)
    {
      this.OnAnimationEvent(VariantUtils.ConvertTo<GodotObject>(ref ((NativeVariantPtrArgs) ref args)[0]), VariantUtils.ConvertTo<GodotObject>(ref ((NativeVariantPtrArgs) ref args)[1]), VariantUtils.ConvertTo<GodotObject>(ref ((NativeVariantPtrArgs) ref args)[2]), VariantUtils.ConvertTo<GodotObject>(ref ((NativeVariantPtrArgs) ref args)[3]));
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NKaiserCrabBossExplosionVfx.MethodName.OnLeftEmbersStart) || ((NativeVariantPtrArgs) ref args).Count != 0)
      return base.InvokeGodotClassMethod(ref method, args, ref ret);
    this.OnLeftEmbersStart();
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NKaiserCrabBossExplosionVfx.MethodName._Ready) || StringName.op_Equality(ref method, NKaiserCrabBossExplosionVfx.MethodName.OnAnimationEvent) || StringName.op_Equality(ref method, NKaiserCrabBossExplosionVfx.MethodName.OnLeftEmbersStart) || base.HasGodotClassMethod(ref method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NKaiserCrabBossExplosionVfx.PropertyName._parent))
    {
      this._parent = VariantUtils.ConvertTo<Node2D>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NKaiserCrabBossExplosionVfx.PropertyName._explosionParticles))
      return ((GodotObject) this).SetGodotClassPropertyValue(ref name, ref value);
    this._explosionParticles = VariantUtils.ConvertTo<GpuParticles2D>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NKaiserCrabBossExplosionVfx.PropertyName._parent))
    {
      value = VariantUtils.CreateFrom<Node2D>(ref this._parent);
      return true;
    }
    if (!StringName.op_Equality(ref name, NKaiserCrabBossExplosionVfx.PropertyName._explosionParticles))
      return ((GodotObject) this).GetGodotClassPropertyValue(ref name, ref value);
    value = VariantUtils.CreateFrom<GpuParticles2D>(ref this._explosionParticles);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NKaiserCrabBossExplosionVfx.PropertyName._parent, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NKaiserCrabBossExplosionVfx.PropertyName._explosionParticles, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).SaveGodotObjectData(info);
    info.AddProperty(NKaiserCrabBossExplosionVfx.PropertyName._parent, Variant.From<Node2D>(ref this._parent));
    info.AddProperty(NKaiserCrabBossExplosionVfx.PropertyName._explosionParticles, Variant.From<GpuParticles2D>(ref this._explosionParticles));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NKaiserCrabBossExplosionVfx.PropertyName._parent, ref variant1))
      this._parent = ((Variant) ref variant1).As<Node2D>();
    Variant variant2;
    if (!info.TryGetProperty(NKaiserCrabBossExplosionVfx.PropertyName._explosionParticles, ref variant2))
      return;
    this._explosionParticles = ((Variant) ref variant2).As<GpuParticles2D>();
  }

  public class MethodName : Node.MethodName
  {
    public static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName OnAnimationEvent = StringName.op_Implicit(nameof (OnAnimationEvent));
    public static readonly StringName OnLeftEmbersStart = StringName.op_Implicit(nameof (OnLeftEmbersStart));
  }

  public class PropertyName : Node.PropertyName
  {
    public static readonly StringName _parent = StringName.op_Implicit(nameof (_parent));
    public static readonly StringName _explosionParticles = StringName.op_Implicit(nameof (_explosionParticles));
  }

  public class SignalName : Node.SignalName
  {
  }
}
