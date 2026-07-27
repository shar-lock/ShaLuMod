// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Vfx.NMechaKnightVfx
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
[ScriptPath("res://src/Core/Nodes/Vfx/NMechaKnightVfx.cs")]
public class NMechaKnightVfx : Node
{
  private GpuParticles2D _flameThrowerParticlesDark;
  private GpuParticles2D _flameThrowerParticlesLight;
  private GpuParticles2D _cinderParticles;
  private GpuParticles2D _glowParticles;
  private GpuParticles2D _engineParticles;
  private GpuParticles2D _engineParticlesDark;
  private Node2D _parent;
  private MegaSprite _animController;

  public override void _Ready()
  {
    this._parent = this.GetParent<Node2D>();
    this._animController = new MegaSprite(Variant.op_Implicit((GodotObject) this._parent));
    this._animController.ConnectAnimationEvent(Callable.From<GodotObject, GodotObject, GodotObject, GodotObject>(new Action<GodotObject, GodotObject, GodotObject, GodotObject>(this.OnAnimationEvent)));
    this._animController.ConnectAnimationStarted(Callable.From<GodotObject, GodotObject, GodotObject>(new Action<GodotObject, GodotObject, GodotObject>(this.OnAnimationStart)));
    this._flameThrowerParticlesDark = ((Node) this._parent).GetNode<GpuParticles2D>(NodePath.op_Implicit("FlameParticlesBone/FlameParticlesDark"));
    this._flameThrowerParticlesLight = ((Node) this._parent).GetNode<GpuParticles2D>(NodePath.op_Implicit("FlameParticlesBone/FlameParticlesLight"));
    this._cinderParticles = ((Node) this._parent).GetNode<GpuParticles2D>(NodePath.op_Implicit("FlameParticlesBone/CinderParticles"));
    this._glowParticles = ((Node) this._parent).GetNode<GpuParticles2D>(NodePath.op_Implicit("FlameParticlesBone/GlowParticles"));
    this._engineParticles = ((Node) this._parent).GetNode<GpuParticles2D>(NodePath.op_Implicit("EngineSlot/EngineBone/EngineParticles"));
    this._engineParticlesDark = ((Node) this._parent).GetNode<GpuParticles2D>(NodePath.op_Implicit("EngineSlot/EngineBone/EngineParticlesDark"));
    this.TurnOffFlameThrower();
    this.TurnOffEngine();
  }

  private void OnAnimationEvent(
    GodotObject _,
    GodotObject __,
    GodotObject ___,
    GodotObject spineEvent)
  {
    switch (new MegaEvent(Variant.op_Implicit(spineEvent)).GetData().GetEventName())
    {
      case "flame_start":
        this.TurnOnFlameThrower();
        break;
      case "flame_end":
        this.TurnOffFlameThrower();
        break;
      case "engine_start":
        this.TurnOnEngine();
        break;
      case "engine_stop":
        this.TurnOffEngine();
        break;
    }
  }

  private void OnAnimationStart(
    GodotObject spineSprite,
    GodotObject animationState,
    GodotObject trackEntry)
  {
    string currentAnimationName = new MegaAnimationState(Variant.op_Implicit(animationState)).GetCurrentAnimationName();
    if (currentAnimationName != "attack")
      this.TurnOffFlameThrower();
    bool flag = currentAnimationName == "hurt";
    ((CanvasItem) this._flameThrowerParticlesDark).Visible = !flag;
    ((CanvasItem) this._flameThrowerParticlesLight).Visible = !flag;
    ((CanvasItem) this._cinderParticles).Visible = !flag;
    ((CanvasItem) this._glowParticles).Visible = !flag;
  }

  private void TurnOnFlameThrower()
  {
    this._flameThrowerParticlesDark.Restart();
    this._flameThrowerParticlesLight.Restart();
    this._cinderParticles.Restart();
    this._glowParticles.Restart();
  }

  private void TurnOffFlameThrower()
  {
    this._flameThrowerParticlesDark.Emitting = false;
    this._flameThrowerParticlesLight.Emitting = false;
    this._cinderParticles.Emitting = false;
    this._glowParticles.Emitting = false;
  }

  private void TurnOnEngine()
  {
    this._engineParticles.Restart();
    this._engineParticlesDark.Restart();
  }

  private void TurnOffEngine()
  {
    this._engineParticles.Emitting = false;
    this._engineParticlesDark.Emitting = false;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(7)
    {
      new MethodInfo(NMechaKnightVfx.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NMechaKnightVfx.MethodName.OnAnimationEvent, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("_"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Object"), false),
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("__"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Object"), false),
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("___"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Object"), false),
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("spineEvent"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Object"), false)
      }, (List<Variant>) null),
      new MethodInfo(NMechaKnightVfx.MethodName.OnAnimationStart, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("spineSprite"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Object"), false),
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("animationState"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Object"), false),
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("trackEntry"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Object"), false)
      }, (List<Variant>) null),
      new MethodInfo(NMechaKnightVfx.MethodName.TurnOnFlameThrower, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NMechaKnightVfx.MethodName.TurnOffFlameThrower, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NMechaKnightVfx.MethodName.TurnOnEngine, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NMechaKnightVfx.MethodName.TurnOffEngine, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NMechaKnightVfx.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      base._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NMechaKnightVfx.MethodName.OnAnimationEvent) && ((NativeVariantPtrArgs) ref args).Count == 4)
    {
      this.OnAnimationEvent(VariantUtils.ConvertTo<GodotObject>(ref ((NativeVariantPtrArgs) ref args)[0]), VariantUtils.ConvertTo<GodotObject>(ref ((NativeVariantPtrArgs) ref args)[1]), VariantUtils.ConvertTo<GodotObject>(ref ((NativeVariantPtrArgs) ref args)[2]), VariantUtils.ConvertTo<GodotObject>(ref ((NativeVariantPtrArgs) ref args)[3]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NMechaKnightVfx.MethodName.OnAnimationStart) && ((NativeVariantPtrArgs) ref args).Count == 3)
    {
      this.OnAnimationStart(VariantUtils.ConvertTo<GodotObject>(ref ((NativeVariantPtrArgs) ref args)[0]), VariantUtils.ConvertTo<GodotObject>(ref ((NativeVariantPtrArgs) ref args)[1]), VariantUtils.ConvertTo<GodotObject>(ref ((NativeVariantPtrArgs) ref args)[2]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NMechaKnightVfx.MethodName.TurnOnFlameThrower) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.TurnOnFlameThrower();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NMechaKnightVfx.MethodName.TurnOffFlameThrower) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.TurnOffFlameThrower();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NMechaKnightVfx.MethodName.TurnOnEngine) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.TurnOnEngine();
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NMechaKnightVfx.MethodName.TurnOffEngine) || ((NativeVariantPtrArgs) ref args).Count != 0)
      return base.InvokeGodotClassMethod(ref method, args, ref ret);
    this.TurnOffEngine();
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NMechaKnightVfx.MethodName._Ready) || StringName.op_Equality(ref method, NMechaKnightVfx.MethodName.OnAnimationEvent) || StringName.op_Equality(ref method, NMechaKnightVfx.MethodName.OnAnimationStart) || StringName.op_Equality(ref method, NMechaKnightVfx.MethodName.TurnOnFlameThrower) || StringName.op_Equality(ref method, NMechaKnightVfx.MethodName.TurnOffFlameThrower) || StringName.op_Equality(ref method, NMechaKnightVfx.MethodName.TurnOnEngine) || StringName.op_Equality(ref method, NMechaKnightVfx.MethodName.TurnOffEngine) || base.HasGodotClassMethod(ref method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NMechaKnightVfx.PropertyName._flameThrowerParticlesDark))
    {
      this._flameThrowerParticlesDark = VariantUtils.ConvertTo<GpuParticles2D>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NMechaKnightVfx.PropertyName._flameThrowerParticlesLight))
    {
      this._flameThrowerParticlesLight = VariantUtils.ConvertTo<GpuParticles2D>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NMechaKnightVfx.PropertyName._cinderParticles))
    {
      this._cinderParticles = VariantUtils.ConvertTo<GpuParticles2D>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NMechaKnightVfx.PropertyName._glowParticles))
    {
      this._glowParticles = VariantUtils.ConvertTo<GpuParticles2D>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NMechaKnightVfx.PropertyName._engineParticles))
    {
      this._engineParticles = VariantUtils.ConvertTo<GpuParticles2D>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NMechaKnightVfx.PropertyName._engineParticlesDark))
    {
      this._engineParticlesDark = VariantUtils.ConvertTo<GpuParticles2D>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NMechaKnightVfx.PropertyName._parent))
      return ((GodotObject) this).SetGodotClassPropertyValue(ref name, ref value);
    this._parent = VariantUtils.ConvertTo<Node2D>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NMechaKnightVfx.PropertyName._flameThrowerParticlesDark))
    {
      value = VariantUtils.CreateFrom<GpuParticles2D>(ref this._flameThrowerParticlesDark);
      return true;
    }
    if (StringName.op_Equality(ref name, NMechaKnightVfx.PropertyName._flameThrowerParticlesLight))
    {
      value = VariantUtils.CreateFrom<GpuParticles2D>(ref this._flameThrowerParticlesLight);
      return true;
    }
    if (StringName.op_Equality(ref name, NMechaKnightVfx.PropertyName._cinderParticles))
    {
      value = VariantUtils.CreateFrom<GpuParticles2D>(ref this._cinderParticles);
      return true;
    }
    if (StringName.op_Equality(ref name, NMechaKnightVfx.PropertyName._glowParticles))
    {
      value = VariantUtils.CreateFrom<GpuParticles2D>(ref this._glowParticles);
      return true;
    }
    if (StringName.op_Equality(ref name, NMechaKnightVfx.PropertyName._engineParticles))
    {
      value = VariantUtils.CreateFrom<GpuParticles2D>(ref this._engineParticles);
      return true;
    }
    if (StringName.op_Equality(ref name, NMechaKnightVfx.PropertyName._engineParticlesDark))
    {
      value = VariantUtils.CreateFrom<GpuParticles2D>(ref this._engineParticlesDark);
      return true;
    }
    if (!StringName.op_Equality(ref name, NMechaKnightVfx.PropertyName._parent))
      return ((GodotObject) this).GetGodotClassPropertyValue(ref name, ref value);
    value = VariantUtils.CreateFrom<Node2D>(ref this._parent);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NMechaKnightVfx.PropertyName._flameThrowerParticlesDark, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NMechaKnightVfx.PropertyName._flameThrowerParticlesLight, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NMechaKnightVfx.PropertyName._cinderParticles, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NMechaKnightVfx.PropertyName._glowParticles, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NMechaKnightVfx.PropertyName._engineParticles, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NMechaKnightVfx.PropertyName._engineParticlesDark, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NMechaKnightVfx.PropertyName._parent, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).SaveGodotObjectData(info);
    info.AddProperty(NMechaKnightVfx.PropertyName._flameThrowerParticlesDark, Variant.From<GpuParticles2D>(ref this._flameThrowerParticlesDark));
    info.AddProperty(NMechaKnightVfx.PropertyName._flameThrowerParticlesLight, Variant.From<GpuParticles2D>(ref this._flameThrowerParticlesLight));
    info.AddProperty(NMechaKnightVfx.PropertyName._cinderParticles, Variant.From<GpuParticles2D>(ref this._cinderParticles));
    info.AddProperty(NMechaKnightVfx.PropertyName._glowParticles, Variant.From<GpuParticles2D>(ref this._glowParticles));
    info.AddProperty(NMechaKnightVfx.PropertyName._engineParticles, Variant.From<GpuParticles2D>(ref this._engineParticles));
    info.AddProperty(NMechaKnightVfx.PropertyName._engineParticlesDark, Variant.From<GpuParticles2D>(ref this._engineParticlesDark));
    info.AddProperty(NMechaKnightVfx.PropertyName._parent, Variant.From<Node2D>(ref this._parent));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NMechaKnightVfx.PropertyName._flameThrowerParticlesDark, ref variant1))
      this._flameThrowerParticlesDark = ((Variant) ref variant1).As<GpuParticles2D>();
    Variant variant2;
    if (info.TryGetProperty(NMechaKnightVfx.PropertyName._flameThrowerParticlesLight, ref variant2))
      this._flameThrowerParticlesLight = ((Variant) ref variant2).As<GpuParticles2D>();
    Variant variant3;
    if (info.TryGetProperty(NMechaKnightVfx.PropertyName._cinderParticles, ref variant3))
      this._cinderParticles = ((Variant) ref variant3).As<GpuParticles2D>();
    Variant variant4;
    if (info.TryGetProperty(NMechaKnightVfx.PropertyName._glowParticles, ref variant4))
      this._glowParticles = ((Variant) ref variant4).As<GpuParticles2D>();
    Variant variant5;
    if (info.TryGetProperty(NMechaKnightVfx.PropertyName._engineParticles, ref variant5))
      this._engineParticles = ((Variant) ref variant5).As<GpuParticles2D>();
    Variant variant6;
    if (info.TryGetProperty(NMechaKnightVfx.PropertyName._engineParticlesDark, ref variant6))
      this._engineParticlesDark = ((Variant) ref variant6).As<GpuParticles2D>();
    Variant variant7;
    if (!info.TryGetProperty(NMechaKnightVfx.PropertyName._parent, ref variant7))
      return;
    this._parent = ((Variant) ref variant7).As<Node2D>();
  }

  public class MethodName : Node.MethodName
  {
    public static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName OnAnimationEvent = StringName.op_Implicit(nameof (OnAnimationEvent));
    public static readonly StringName OnAnimationStart = StringName.op_Implicit(nameof (OnAnimationStart));
    public static readonly StringName TurnOnFlameThrower = StringName.op_Implicit(nameof (TurnOnFlameThrower));
    public static readonly StringName TurnOffFlameThrower = StringName.op_Implicit(nameof (TurnOffFlameThrower));
    public static readonly StringName TurnOnEngine = StringName.op_Implicit(nameof (TurnOnEngine));
    public static readonly StringName TurnOffEngine = StringName.op_Implicit(nameof (TurnOffEngine));
  }

  public class PropertyName : Node.PropertyName
  {
    public static readonly StringName _flameThrowerParticlesDark = StringName.op_Implicit(nameof (_flameThrowerParticlesDark));
    public static readonly StringName _flameThrowerParticlesLight = StringName.op_Implicit(nameof (_flameThrowerParticlesLight));
    public static readonly StringName _cinderParticles = StringName.op_Implicit(nameof (_cinderParticles));
    public static readonly StringName _glowParticles = StringName.op_Implicit(nameof (_glowParticles));
    public static readonly StringName _engineParticles = StringName.op_Implicit(nameof (_engineParticles));
    public static readonly StringName _engineParticlesDark = StringName.op_Implicit(nameof (_engineParticlesDark));
    public static readonly StringName _parent = StringName.op_Implicit(nameof (_parent));
  }

  public class SignalName : Node.SignalName
  {
  }
}
