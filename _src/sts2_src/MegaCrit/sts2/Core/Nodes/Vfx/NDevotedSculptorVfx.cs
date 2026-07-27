// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Vfx.NDevotedSculptorVfx
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

[GlobalClass]
[ScriptPath("res://src/Core/Nodes/Vfx/NDevotedSculptorVfx.cs")]
public class NDevotedSculptorVfx : Node
{
  private GpuParticles2D _voiceParticles;
  private GpuParticles2D _attackParticles;
  private Node2D _parent;
  private MegaSprite _animController;

  public override void _Ready()
  {
    this._parent = this.GetParent<Node2D>();
    this._animController = new MegaSprite(Variant.op_Implicit((GodotObject) this._parent));
    this._animController.ConnectAnimationEvent(Callable.From<GodotObject, GodotObject, GodotObject, GodotObject>(new Action<GodotObject, GodotObject, GodotObject, GodotObject>(this.OnAnimationEvent)));
    this._voiceParticles = ((Node) this._parent).GetNode<GpuParticles2D>(NodePath.op_Implicit("VoiceBoneNode/VoiceParticles"));
    this._voiceParticles.Emitting = false;
    this._voiceParticles.OneShot = true;
    this._attackParticles = ((Node) this._parent).GetNode<GpuParticles2D>(NodePath.op_Implicit("AttackParticles"));
    this._attackParticles.Emitting = false;
    this._attackParticles.OneShot = true;
  }

  private void OnAnimationEvent(
    GodotObject _,
    GodotObject __,
    GodotObject ___,
    GodotObject spineEvent)
  {
    switch (new MegaEvent(Variant.op_Implicit(spineEvent)).GetData().GetEventName())
    {
      case "caw":
        this.StartVoice();
        break;
      case "attack":
        this.StartAttack();
        break;
    }
  }

  private void StartVoice() => this._voiceParticles.Restart();

  private void StartAttack() => this._attackParticles.Restart();

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(4)
    {
      new MethodInfo(NDevotedSculptorVfx.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NDevotedSculptorVfx.MethodName.OnAnimationEvent, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("_"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Object"), false),
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("__"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Object"), false),
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("___"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Object"), false),
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("spineEvent"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Object"), false)
      }, (List<Variant>) null),
      new MethodInfo(NDevotedSculptorVfx.MethodName.StartVoice, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NDevotedSculptorVfx.MethodName.StartAttack, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NDevotedSculptorVfx.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      base._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NDevotedSculptorVfx.MethodName.OnAnimationEvent) && ((NativeVariantPtrArgs) ref args).Count == 4)
    {
      this.OnAnimationEvent(VariantUtils.ConvertTo<GodotObject>(ref ((NativeVariantPtrArgs) ref args)[0]), VariantUtils.ConvertTo<GodotObject>(ref ((NativeVariantPtrArgs) ref args)[1]), VariantUtils.ConvertTo<GodotObject>(ref ((NativeVariantPtrArgs) ref args)[2]), VariantUtils.ConvertTo<GodotObject>(ref ((NativeVariantPtrArgs) ref args)[3]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NDevotedSculptorVfx.MethodName.StartVoice) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.StartVoice();
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NDevotedSculptorVfx.MethodName.StartAttack) || ((NativeVariantPtrArgs) ref args).Count != 0)
      return base.InvokeGodotClassMethod(ref method, args, ref ret);
    this.StartAttack();
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NDevotedSculptorVfx.MethodName._Ready) || StringName.op_Equality(ref method, NDevotedSculptorVfx.MethodName.OnAnimationEvent) || StringName.op_Equality(ref method, NDevotedSculptorVfx.MethodName.StartVoice) || StringName.op_Equality(ref method, NDevotedSculptorVfx.MethodName.StartAttack) || base.HasGodotClassMethod(ref method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NDevotedSculptorVfx.PropertyName._voiceParticles))
    {
      this._voiceParticles = VariantUtils.ConvertTo<GpuParticles2D>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NDevotedSculptorVfx.PropertyName._attackParticles))
    {
      this._attackParticles = VariantUtils.ConvertTo<GpuParticles2D>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NDevotedSculptorVfx.PropertyName._parent))
      return ((GodotObject) this).SetGodotClassPropertyValue(ref name, ref value);
    this._parent = VariantUtils.ConvertTo<Node2D>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NDevotedSculptorVfx.PropertyName._voiceParticles))
    {
      value = VariantUtils.CreateFrom<GpuParticles2D>(ref this._voiceParticles);
      return true;
    }
    if (StringName.op_Equality(ref name, NDevotedSculptorVfx.PropertyName._attackParticles))
    {
      value = VariantUtils.CreateFrom<GpuParticles2D>(ref this._attackParticles);
      return true;
    }
    if (!StringName.op_Equality(ref name, NDevotedSculptorVfx.PropertyName._parent))
      return ((GodotObject) this).GetGodotClassPropertyValue(ref name, ref value);
    value = VariantUtils.CreateFrom<Node2D>(ref this._parent);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NDevotedSculptorVfx.PropertyName._voiceParticles, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NDevotedSculptorVfx.PropertyName._attackParticles, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NDevotedSculptorVfx.PropertyName._parent, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).SaveGodotObjectData(info);
    info.AddProperty(NDevotedSculptorVfx.PropertyName._voiceParticles, Variant.From<GpuParticles2D>(ref this._voiceParticles));
    info.AddProperty(NDevotedSculptorVfx.PropertyName._attackParticles, Variant.From<GpuParticles2D>(ref this._attackParticles));
    info.AddProperty(NDevotedSculptorVfx.PropertyName._parent, Variant.From<Node2D>(ref this._parent));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NDevotedSculptorVfx.PropertyName._voiceParticles, ref variant1))
      this._voiceParticles = ((Variant) ref variant1).As<GpuParticles2D>();
    Variant variant2;
    if (info.TryGetProperty(NDevotedSculptorVfx.PropertyName._attackParticles, ref variant2))
      this._attackParticles = ((Variant) ref variant2).As<GpuParticles2D>();
    Variant variant3;
    if (!info.TryGetProperty(NDevotedSculptorVfx.PropertyName._parent, ref variant3))
      return;
    this._parent = ((Variant) ref variant3).As<Node2D>();
  }

  public class MethodName : Node.MethodName
  {
    public static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName OnAnimationEvent = StringName.op_Implicit(nameof (OnAnimationEvent));
    public static readonly StringName StartVoice = StringName.op_Implicit(nameof (StartVoice));
    public static readonly StringName StartAttack = StringName.op_Implicit(nameof (StartAttack));
  }

  public class PropertyName : Node.PropertyName
  {
    public static readonly StringName _voiceParticles = StringName.op_Implicit(nameof (_voiceParticles));
    public static readonly StringName _attackParticles = StringName.op_Implicit(nameof (_attackParticles));
    public static readonly StringName _parent = StringName.op_Implicit(nameof (_parent));
  }

  public class SignalName : Node.SignalName
  {
  }
}
