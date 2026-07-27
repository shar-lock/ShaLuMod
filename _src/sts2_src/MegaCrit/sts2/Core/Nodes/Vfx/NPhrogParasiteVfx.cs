// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Vfx.NPhrogParasiteVfx
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Bindings.MegaSpine;
using MegaCrit.Sts2.Core.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Vfx;

[GlobalClass]
[ScriptPath("res://src/Core/Nodes/Vfx/NPhrogParasiteVfx.cs")]
public class NPhrogParasiteVfx : Node
{
  private GpuParticles2D _bubbleParticlesA;
  private GpuParticles2D _bubbleParticlesB;
  private GpuParticles2D _bubbleParticlesC;
  private GpuParticles2D _gooParticlesDeath;
  private GpuParticles2D _wormParticlesDeath;
  private Node2D _parent;
  private MegaSprite _animController;

  public override void _Ready()
  {
    this._parent = this.GetParent<Node2D>();
    this._animController = new MegaSprite(Variant.op_Implicit((GodotObject) this._parent));
    this._animController.ConnectAnimationEvent(Callable.From<GodotObject, GodotObject, GodotObject, GodotObject>(new Action<GodotObject, GodotObject, GodotObject, GodotObject>(this.OnAnimationEvent)));
    this._bubbleParticlesA = ((Node) this._parent).GetNode<GpuParticles2D>(NodePath.op_Implicit("BubbleABoneNode/WormParticlesA"));
    this._bubbleParticlesB = ((Node) this._parent).GetNode<GpuParticles2D>(NodePath.op_Implicit("BubbleBSlotNode/WormParticlesB"));
    this._bubbleParticlesC = ((Node) this._parent).GetNode<GpuParticles2D>(NodePath.op_Implicit("BubbleCBoneNode/WormParticlesC"));
    this._gooParticlesDeath = ((Node) this._parent).GetNode<GpuParticles2D>(NodePath.op_Implicit("DeathParticles"));
    this._wormParticlesDeath = ((Node) this._parent).GetNode<GpuParticles2D>(NodePath.op_Implicit("DeathWormParticles"));
    this._bubbleParticlesA.Emitting = false;
    this._bubbleParticlesB.Emitting = false;
    this._bubbleParticlesC.Emitting = false;
    this._gooParticlesDeath.Emitting = false;
    this._wormParticlesDeath.Emitting = false;
    this._gooParticlesDeath.OneShot = true;
    this._wormParticlesDeath.OneShot = true;
    this.RunWhenSpineReady(this._animController, (Action<MegaAnimationState>) (animState => animState.SetAnimation("die")));
    this._animController.ConnectAnimationStarted(Callable.From<GodotObject, GodotObject, GodotObject>(new Action<GodotObject, GodotObject, GodotObject>(this.OnAnimationStart)));
  }

  private void OnAnimationEvent(
    GodotObject _,
    GodotObject __,
    GodotObject ___,
    GodotObject spineEvent)
  {
    switch (new MegaEvent(Variant.op_Implicit(spineEvent)).GetData().GetEventName())
    {
      case "infect":
        this.TurnOnInfect();
        break;
      case "stop_infect":
        this.TurnOffInfect();
        break;
      case "explode":
        this.StartExplode();
        break;
    }
  }

  private void OnAnimationStart(
    GodotObject spineSprite,
    GodotObject animationState,
    GodotObject trackEntry)
  {
    this.TurnOffInfect();
  }

  private void TurnOnInfect()
  {
    this._bubbleParticlesA.Emitting = true;
    this._bubbleParticlesB.Emitting = true;
    this._bubbleParticlesC.Emitting = true;
  }

  private void TurnOffInfect()
  {
    this._bubbleParticlesA.Emitting = false;
    this._bubbleParticlesB.Emitting = false;
    this._bubbleParticlesC.Emitting = false;
  }

  private void StartExplode()
  {
    this._gooParticlesDeath.Restart();
    this._wormParticlesDeath.Restart();
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(6)
    {
      new MethodInfo(NPhrogParasiteVfx.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NPhrogParasiteVfx.MethodName.OnAnimationEvent, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("_"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Object"), false),
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("__"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Object"), false),
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("___"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Object"), false),
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("spineEvent"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Object"), false)
      }, (List<Variant>) null),
      new MethodInfo(NPhrogParasiteVfx.MethodName.OnAnimationStart, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("spineSprite"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Object"), false),
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("animationState"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Object"), false),
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("trackEntry"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Object"), false)
      }, (List<Variant>) null),
      new MethodInfo(NPhrogParasiteVfx.MethodName.TurnOnInfect, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NPhrogParasiteVfx.MethodName.TurnOffInfect, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NPhrogParasiteVfx.MethodName.StartExplode, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NPhrogParasiteVfx.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      base._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NPhrogParasiteVfx.MethodName.OnAnimationEvent) && ((NativeVariantPtrArgs) ref args).Count == 4)
    {
      this.OnAnimationEvent(VariantUtils.ConvertTo<GodotObject>(ref ((NativeVariantPtrArgs) ref args)[0]), VariantUtils.ConvertTo<GodotObject>(ref ((NativeVariantPtrArgs) ref args)[1]), VariantUtils.ConvertTo<GodotObject>(ref ((NativeVariantPtrArgs) ref args)[2]), VariantUtils.ConvertTo<GodotObject>(ref ((NativeVariantPtrArgs) ref args)[3]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NPhrogParasiteVfx.MethodName.OnAnimationStart) && ((NativeVariantPtrArgs) ref args).Count == 3)
    {
      this.OnAnimationStart(VariantUtils.ConvertTo<GodotObject>(ref ((NativeVariantPtrArgs) ref args)[0]), VariantUtils.ConvertTo<GodotObject>(ref ((NativeVariantPtrArgs) ref args)[1]), VariantUtils.ConvertTo<GodotObject>(ref ((NativeVariantPtrArgs) ref args)[2]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NPhrogParasiteVfx.MethodName.TurnOnInfect) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.TurnOnInfect();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NPhrogParasiteVfx.MethodName.TurnOffInfect) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.TurnOffInfect();
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NPhrogParasiteVfx.MethodName.StartExplode) || ((NativeVariantPtrArgs) ref args).Count != 0)
      return base.InvokeGodotClassMethod(ref method, args, ref ret);
    this.StartExplode();
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NPhrogParasiteVfx.MethodName._Ready) || StringName.op_Equality(ref method, NPhrogParasiteVfx.MethodName.OnAnimationEvent) || StringName.op_Equality(ref method, NPhrogParasiteVfx.MethodName.OnAnimationStart) || StringName.op_Equality(ref method, NPhrogParasiteVfx.MethodName.TurnOnInfect) || StringName.op_Equality(ref method, NPhrogParasiteVfx.MethodName.TurnOffInfect) || StringName.op_Equality(ref method, NPhrogParasiteVfx.MethodName.StartExplode) || base.HasGodotClassMethod(ref method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NPhrogParasiteVfx.PropertyName._bubbleParticlesA))
    {
      this._bubbleParticlesA = VariantUtils.ConvertTo<GpuParticles2D>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NPhrogParasiteVfx.PropertyName._bubbleParticlesB))
    {
      this._bubbleParticlesB = VariantUtils.ConvertTo<GpuParticles2D>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NPhrogParasiteVfx.PropertyName._bubbleParticlesC))
    {
      this._bubbleParticlesC = VariantUtils.ConvertTo<GpuParticles2D>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NPhrogParasiteVfx.PropertyName._gooParticlesDeath))
    {
      this._gooParticlesDeath = VariantUtils.ConvertTo<GpuParticles2D>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NPhrogParasiteVfx.PropertyName._wormParticlesDeath))
    {
      this._wormParticlesDeath = VariantUtils.ConvertTo<GpuParticles2D>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NPhrogParasiteVfx.PropertyName._parent))
      return ((GodotObject) this).SetGodotClassPropertyValue(ref name, ref value);
    this._parent = VariantUtils.ConvertTo<Node2D>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NPhrogParasiteVfx.PropertyName._bubbleParticlesA))
    {
      value = VariantUtils.CreateFrom<GpuParticles2D>(ref this._bubbleParticlesA);
      return true;
    }
    if (StringName.op_Equality(ref name, NPhrogParasiteVfx.PropertyName._bubbleParticlesB))
    {
      value = VariantUtils.CreateFrom<GpuParticles2D>(ref this._bubbleParticlesB);
      return true;
    }
    if (StringName.op_Equality(ref name, NPhrogParasiteVfx.PropertyName._bubbleParticlesC))
    {
      value = VariantUtils.CreateFrom<GpuParticles2D>(ref this._bubbleParticlesC);
      return true;
    }
    if (StringName.op_Equality(ref name, NPhrogParasiteVfx.PropertyName._gooParticlesDeath))
    {
      value = VariantUtils.CreateFrom<GpuParticles2D>(ref this._gooParticlesDeath);
      return true;
    }
    if (StringName.op_Equality(ref name, NPhrogParasiteVfx.PropertyName._wormParticlesDeath))
    {
      value = VariantUtils.CreateFrom<GpuParticles2D>(ref this._wormParticlesDeath);
      return true;
    }
    if (!StringName.op_Equality(ref name, NPhrogParasiteVfx.PropertyName._parent))
      return ((GodotObject) this).GetGodotClassPropertyValue(ref name, ref value);
    value = VariantUtils.CreateFrom<Node2D>(ref this._parent);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NPhrogParasiteVfx.PropertyName._bubbleParticlesA, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NPhrogParasiteVfx.PropertyName._bubbleParticlesB, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NPhrogParasiteVfx.PropertyName._bubbleParticlesC, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NPhrogParasiteVfx.PropertyName._gooParticlesDeath, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NPhrogParasiteVfx.PropertyName._wormParticlesDeath, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NPhrogParasiteVfx.PropertyName._parent, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).SaveGodotObjectData(info);
    info.AddProperty(NPhrogParasiteVfx.PropertyName._bubbleParticlesA, Variant.From<GpuParticles2D>(ref this._bubbleParticlesA));
    info.AddProperty(NPhrogParasiteVfx.PropertyName._bubbleParticlesB, Variant.From<GpuParticles2D>(ref this._bubbleParticlesB));
    info.AddProperty(NPhrogParasiteVfx.PropertyName._bubbleParticlesC, Variant.From<GpuParticles2D>(ref this._bubbleParticlesC));
    info.AddProperty(NPhrogParasiteVfx.PropertyName._gooParticlesDeath, Variant.From<GpuParticles2D>(ref this._gooParticlesDeath));
    info.AddProperty(NPhrogParasiteVfx.PropertyName._wormParticlesDeath, Variant.From<GpuParticles2D>(ref this._wormParticlesDeath));
    info.AddProperty(NPhrogParasiteVfx.PropertyName._parent, Variant.From<Node2D>(ref this._parent));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NPhrogParasiteVfx.PropertyName._bubbleParticlesA, ref variant1))
      this._bubbleParticlesA = ((Variant) ref variant1).As<GpuParticles2D>();
    Variant variant2;
    if (info.TryGetProperty(NPhrogParasiteVfx.PropertyName._bubbleParticlesB, ref variant2))
      this._bubbleParticlesB = ((Variant) ref variant2).As<GpuParticles2D>();
    Variant variant3;
    if (info.TryGetProperty(NPhrogParasiteVfx.PropertyName._bubbleParticlesC, ref variant3))
      this._bubbleParticlesC = ((Variant) ref variant3).As<GpuParticles2D>();
    Variant variant4;
    if (info.TryGetProperty(NPhrogParasiteVfx.PropertyName._gooParticlesDeath, ref variant4))
      this._gooParticlesDeath = ((Variant) ref variant4).As<GpuParticles2D>();
    Variant variant5;
    if (info.TryGetProperty(NPhrogParasiteVfx.PropertyName._wormParticlesDeath, ref variant5))
      this._wormParticlesDeath = ((Variant) ref variant5).As<GpuParticles2D>();
    Variant variant6;
    if (!info.TryGetProperty(NPhrogParasiteVfx.PropertyName._parent, ref variant6))
      return;
    this._parent = ((Variant) ref variant6).As<Node2D>();
  }

  public class MethodName : Node.MethodName
  {
    public static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName OnAnimationEvent = StringName.op_Implicit(nameof (OnAnimationEvent));
    public static readonly StringName OnAnimationStart = StringName.op_Implicit(nameof (OnAnimationStart));
    public static readonly StringName TurnOnInfect = StringName.op_Implicit(nameof (TurnOnInfect));
    public static readonly StringName TurnOffInfect = StringName.op_Implicit(nameof (TurnOffInfect));
    public static readonly StringName StartExplode = StringName.op_Implicit(nameof (StartExplode));
  }

  public class PropertyName : Node.PropertyName
  {
    public static readonly StringName _bubbleParticlesA = StringName.op_Implicit(nameof (_bubbleParticlesA));
    public static readonly StringName _bubbleParticlesB = StringName.op_Implicit(nameof (_bubbleParticlesB));
    public static readonly StringName _bubbleParticlesC = StringName.op_Implicit(nameof (_bubbleParticlesC));
    public static readonly StringName _gooParticlesDeath = StringName.op_Implicit(nameof (_gooParticlesDeath));
    public static readonly StringName _wormParticlesDeath = StringName.op_Implicit(nameof (_wormParticlesDeath));
    public static readonly StringName _parent = StringName.op_Implicit(nameof (_parent));
  }

  public class SignalName : Node.SignalName
  {
  }
}
