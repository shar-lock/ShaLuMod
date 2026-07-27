// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Vfx.NTheInsatiableVfx
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
[ScriptPath("res://src/Core/Nodes/Vfx/NTheInsatiableVfx.cs")]
public class NTheInsatiableVfx : Node
{
  [Export]
  private CpuParticles2D[] _continuousParticles;
  private CpuParticles2D _salivaFountainParticles;
  private CpuParticles2D _salivaDroolParticles;
  private CpuParticles2D _salivaCloudParticles;
  private GpuParticles2D _baseBlastParticles;
  private Node2D _parent;
  private MegaSprite _animController;

  public override void _Ready()
  {
    this._parent = this.GetParent<Node2D>();
    this._animController = new MegaSprite(Variant.op_Implicit((GodotObject) this._parent));
    this._animController.ConnectAnimationEvent(Callable.From<GodotObject, GodotObject, GodotObject, GodotObject>(new Action<GodotObject, GodotObject, GodotObject, GodotObject>(this.OnAnimationEvent)));
    this._animController.ConnectAnimationStarted(Callable.From<GodotObject, GodotObject, GodotObject>(new Action<GodotObject, GodotObject, GodotObject>(this.OnAnimationStart)));
    this._salivaFountainParticles = ((Node) this._parent).GetNode<CpuParticles2D>(NodePath.op_Implicit("SalivaSlotNode/SalivaFountainParticles"));
    this._salivaDroolParticles = ((Node) this._parent).GetNode<CpuParticles2D>(NodePath.op_Implicit("SalivaSlotNode/SalivaDroolParticles"));
    this._salivaCloudParticles = ((Node) this._parent).GetNode<CpuParticles2D>(NodePath.op_Implicit("SalivaSlotNode/SalivaCloudParticles"));
    this._baseBlastParticles = ((Node) this._parent).GetNode<GpuParticles2D>(NodePath.op_Implicit("BaseBlastSlot/BaseBlastParticles"));
    this._salivaFountainParticles.Emitting = false;
    this._salivaDroolParticles.Emitting = false;
    this._salivaCloudParticles.Emitting = false;
    this._baseBlastParticles.Emitting = false;
  }

  private void OnAnimationEvent(
    GodotObject _,
    GodotObject __,
    GodotObject ___,
    GodotObject spineEvent)
  {
    string eventName = new MegaEvent(Variant.op_Implicit(spineEvent)).GetData().GetEventName();
    if (eventName == null)
      return;
    switch (eventName.Length)
    {
      case 9:
        switch (eventName[1])
        {
          case 'e':
            if (!(eventName == "death_end"))
              return;
            this.TurnOffContinuousParticles();
            return;
          case 'r':
            if (!(eventName == "drool_end"))
              return;
            this.TurnOffDrool();
            return;
          default:
            return;
        }
      case 10:
        if (!(eventName == "saliva_end"))
          break;
        this.TurnOffSaliva();
        break;
      case 11:
        if (!(eventName == "drool_start"))
          break;
        this.TurnOnDrool();
        break;
      case 12:
        if (!(eventName == "saliva_start"))
          break;
        this.TurnOnSaliva();
        break;
      case 14:
        if (!(eventName == "base_blast_end"))
          break;
        this.TurnOffBaseBlast();
        break;
      case 16 /*0x10*/:
        if (!(eventName == "base_blast_start"))
          break;
        this.TurnOnBaseBlast();
        break;
    }
  }

  private void TurnOnSaliva()
  {
    this._salivaFountainParticles.Restart();
    this._salivaCloudParticles.Restart();
  }

  private void TurnOffSaliva()
  {
    this._salivaFountainParticles.Emitting = false;
    this._salivaCloudParticles.Emitting = false;
  }

  private void TurnOnDrool() => this._salivaDroolParticles.Restart();

  private void TurnOffDrool() => this._salivaDroolParticles.Emitting = false;

  private void TurnOnBaseBlast() => this._baseBlastParticles.Emitting = true;

  private void TurnOffBaseBlast() => this._baseBlastParticles.Emitting = false;

  private void TurnOffContinuousParticles()
  {
    foreach (CpuParticles2D continuousParticle in this._continuousParticles)
      continuousParticle.Emitting = false;
  }

  private void OnAnimationStart(
    GodotObject spineSprite,
    GodotObject animationState,
    GodotObject trackEntry)
  {
    string currentAnimationName = new MegaAnimationState(Variant.op_Implicit(animationState)).GetCurrentAnimationName();
    if (currentAnimationName != "attack_thrash")
      this.TurnOffBaseBlast();
    if (!(currentAnimationName != "salivate"))
      return;
    this.TurnOffSaliva();
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(10)
    {
      new MethodInfo(NTheInsatiableVfx.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NTheInsatiableVfx.MethodName.OnAnimationEvent, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("_"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Object"), false),
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("__"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Object"), false),
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("___"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Object"), false),
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("spineEvent"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Object"), false)
      }, (List<Variant>) null),
      new MethodInfo(NTheInsatiableVfx.MethodName.TurnOnSaliva, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NTheInsatiableVfx.MethodName.TurnOffSaliva, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NTheInsatiableVfx.MethodName.TurnOnDrool, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NTheInsatiableVfx.MethodName.TurnOffDrool, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NTheInsatiableVfx.MethodName.TurnOnBaseBlast, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NTheInsatiableVfx.MethodName.TurnOffBaseBlast, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NTheInsatiableVfx.MethodName.TurnOffContinuousParticles, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NTheInsatiableVfx.MethodName.OnAnimationStart, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("spineSprite"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Object"), false),
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("animationState"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Object"), false),
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("trackEntry"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Object"), false)
      }, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NTheInsatiableVfx.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      base._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NTheInsatiableVfx.MethodName.OnAnimationEvent) && ((NativeVariantPtrArgs) ref args).Count == 4)
    {
      this.OnAnimationEvent(VariantUtils.ConvertTo<GodotObject>(ref ((NativeVariantPtrArgs) ref args)[0]), VariantUtils.ConvertTo<GodotObject>(ref ((NativeVariantPtrArgs) ref args)[1]), VariantUtils.ConvertTo<GodotObject>(ref ((NativeVariantPtrArgs) ref args)[2]), VariantUtils.ConvertTo<GodotObject>(ref ((NativeVariantPtrArgs) ref args)[3]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NTheInsatiableVfx.MethodName.TurnOnSaliva) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.TurnOnSaliva();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NTheInsatiableVfx.MethodName.TurnOffSaliva) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.TurnOffSaliva();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NTheInsatiableVfx.MethodName.TurnOnDrool) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.TurnOnDrool();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NTheInsatiableVfx.MethodName.TurnOffDrool) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.TurnOffDrool();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NTheInsatiableVfx.MethodName.TurnOnBaseBlast) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.TurnOnBaseBlast();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NTheInsatiableVfx.MethodName.TurnOffBaseBlast) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.TurnOffBaseBlast();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NTheInsatiableVfx.MethodName.TurnOffContinuousParticles) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.TurnOffContinuousParticles();
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NTheInsatiableVfx.MethodName.OnAnimationStart) || ((NativeVariantPtrArgs) ref args).Count != 3)
      return base.InvokeGodotClassMethod(ref method, args, ref ret);
    this.OnAnimationStart(VariantUtils.ConvertTo<GodotObject>(ref ((NativeVariantPtrArgs) ref args)[0]), VariantUtils.ConvertTo<GodotObject>(ref ((NativeVariantPtrArgs) ref args)[1]), VariantUtils.ConvertTo<GodotObject>(ref ((NativeVariantPtrArgs) ref args)[2]));
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NTheInsatiableVfx.MethodName._Ready) || StringName.op_Equality(ref method, NTheInsatiableVfx.MethodName.OnAnimationEvent) || StringName.op_Equality(ref method, NTheInsatiableVfx.MethodName.TurnOnSaliva) || StringName.op_Equality(ref method, NTheInsatiableVfx.MethodName.TurnOffSaliva) || StringName.op_Equality(ref method, NTheInsatiableVfx.MethodName.TurnOnDrool) || StringName.op_Equality(ref method, NTheInsatiableVfx.MethodName.TurnOffDrool) || StringName.op_Equality(ref method, NTheInsatiableVfx.MethodName.TurnOnBaseBlast) || StringName.op_Equality(ref method, NTheInsatiableVfx.MethodName.TurnOffBaseBlast) || StringName.op_Equality(ref method, NTheInsatiableVfx.MethodName.TurnOffContinuousParticles) || StringName.op_Equality(ref method, NTheInsatiableVfx.MethodName.OnAnimationStart) || base.HasGodotClassMethod(ref method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NTheInsatiableVfx.PropertyName._continuousParticles))
    {
      this._continuousParticles = VariantUtils.ConvertToSystemArrayOfGodotObject<CpuParticles2D>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NTheInsatiableVfx.PropertyName._salivaFountainParticles))
    {
      this._salivaFountainParticles = VariantUtils.ConvertTo<CpuParticles2D>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NTheInsatiableVfx.PropertyName._salivaDroolParticles))
    {
      this._salivaDroolParticles = VariantUtils.ConvertTo<CpuParticles2D>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NTheInsatiableVfx.PropertyName._salivaCloudParticles))
    {
      this._salivaCloudParticles = VariantUtils.ConvertTo<CpuParticles2D>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NTheInsatiableVfx.PropertyName._baseBlastParticles))
    {
      this._baseBlastParticles = VariantUtils.ConvertTo<GpuParticles2D>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NTheInsatiableVfx.PropertyName._parent))
      return ((GodotObject) this).SetGodotClassPropertyValue(ref name, ref value);
    this._parent = VariantUtils.ConvertTo<Node2D>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NTheInsatiableVfx.PropertyName._continuousParticles))
    {
      value = VariantUtils.CreateFromSystemArrayOfGodotObject((GodotObject[]) this._continuousParticles);
      return true;
    }
    if (StringName.op_Equality(ref name, NTheInsatiableVfx.PropertyName._salivaFountainParticles))
    {
      value = VariantUtils.CreateFrom<CpuParticles2D>(ref this._salivaFountainParticles);
      return true;
    }
    if (StringName.op_Equality(ref name, NTheInsatiableVfx.PropertyName._salivaDroolParticles))
    {
      value = VariantUtils.CreateFrom<CpuParticles2D>(ref this._salivaDroolParticles);
      return true;
    }
    if (StringName.op_Equality(ref name, NTheInsatiableVfx.PropertyName._salivaCloudParticles))
    {
      value = VariantUtils.CreateFrom<CpuParticles2D>(ref this._salivaCloudParticles);
      return true;
    }
    if (StringName.op_Equality(ref name, NTheInsatiableVfx.PropertyName._baseBlastParticles))
    {
      value = VariantUtils.CreateFrom<GpuParticles2D>(ref this._baseBlastParticles);
      return true;
    }
    if (!StringName.op_Equality(ref name, NTheInsatiableVfx.PropertyName._parent))
      return ((GodotObject) this).GetGodotClassPropertyValue(ref name, ref value);
    value = VariantUtils.CreateFrom<Node2D>(ref this._parent);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 28L, NTheInsatiableVfx.PropertyName._continuousParticles, (PropertyHint) 23L, "24/34:CPUParticles2D", (PropertyUsageFlags) 4102L, true),
      new PropertyInfo((Variant.Type) 24L, NTheInsatiableVfx.PropertyName._salivaFountainParticles, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NTheInsatiableVfx.PropertyName._salivaDroolParticles, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NTheInsatiableVfx.PropertyName._salivaCloudParticles, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NTheInsatiableVfx.PropertyName._baseBlastParticles, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NTheInsatiableVfx.PropertyName._parent, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).SaveGodotObjectData(info);
    info.AddProperty(NTheInsatiableVfx.PropertyName._continuousParticles, Variant.CreateFrom((GodotObject[]) this._continuousParticles));
    info.AddProperty(NTheInsatiableVfx.PropertyName._salivaFountainParticles, Variant.From<CpuParticles2D>(ref this._salivaFountainParticles));
    info.AddProperty(NTheInsatiableVfx.PropertyName._salivaDroolParticles, Variant.From<CpuParticles2D>(ref this._salivaDroolParticles));
    info.AddProperty(NTheInsatiableVfx.PropertyName._salivaCloudParticles, Variant.From<CpuParticles2D>(ref this._salivaCloudParticles));
    info.AddProperty(NTheInsatiableVfx.PropertyName._baseBlastParticles, Variant.From<GpuParticles2D>(ref this._baseBlastParticles));
    info.AddProperty(NTheInsatiableVfx.PropertyName._parent, Variant.From<Node2D>(ref this._parent));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NTheInsatiableVfx.PropertyName._continuousParticles, ref variant1))
      this._continuousParticles = ((Variant) ref variant1).AsGodotObjectArray<CpuParticles2D>();
    Variant variant2;
    if (info.TryGetProperty(NTheInsatiableVfx.PropertyName._salivaFountainParticles, ref variant2))
      this._salivaFountainParticles = ((Variant) ref variant2).As<CpuParticles2D>();
    Variant variant3;
    if (info.TryGetProperty(NTheInsatiableVfx.PropertyName._salivaDroolParticles, ref variant3))
      this._salivaDroolParticles = ((Variant) ref variant3).As<CpuParticles2D>();
    Variant variant4;
    if (info.TryGetProperty(NTheInsatiableVfx.PropertyName._salivaCloudParticles, ref variant4))
      this._salivaCloudParticles = ((Variant) ref variant4).As<CpuParticles2D>();
    Variant variant5;
    if (info.TryGetProperty(NTheInsatiableVfx.PropertyName._baseBlastParticles, ref variant5))
      this._baseBlastParticles = ((Variant) ref variant5).As<GpuParticles2D>();
    Variant variant6;
    if (!info.TryGetProperty(NTheInsatiableVfx.PropertyName._parent, ref variant6))
      return;
    this._parent = ((Variant) ref variant6).As<Node2D>();
  }

  public class MethodName : Node.MethodName
  {
    public static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName OnAnimationEvent = StringName.op_Implicit(nameof (OnAnimationEvent));
    public static readonly StringName TurnOnSaliva = StringName.op_Implicit(nameof (TurnOnSaliva));
    public static readonly StringName TurnOffSaliva = StringName.op_Implicit(nameof (TurnOffSaliva));
    public static readonly StringName TurnOnDrool = StringName.op_Implicit(nameof (TurnOnDrool));
    public static readonly StringName TurnOffDrool = StringName.op_Implicit(nameof (TurnOffDrool));
    public static readonly StringName TurnOnBaseBlast = StringName.op_Implicit(nameof (TurnOnBaseBlast));
    public static readonly StringName TurnOffBaseBlast = StringName.op_Implicit(nameof (TurnOffBaseBlast));
    public static readonly StringName TurnOffContinuousParticles = StringName.op_Implicit(nameof (TurnOffContinuousParticles));
    public static readonly StringName OnAnimationStart = StringName.op_Implicit(nameof (OnAnimationStart));
  }

  public class PropertyName : Node.PropertyName
  {
    public static readonly StringName _continuousParticles = StringName.op_Implicit(nameof (_continuousParticles));
    public static readonly StringName _salivaFountainParticles = StringName.op_Implicit(nameof (_salivaFountainParticles));
    public static readonly StringName _salivaDroolParticles = StringName.op_Implicit(nameof (_salivaDroolParticles));
    public static readonly StringName _salivaCloudParticles = StringName.op_Implicit(nameof (_salivaCloudParticles));
    public static readonly StringName _baseBlastParticles = StringName.op_Implicit(nameof (_baseBlastParticles));
    public static readonly StringName _parent = StringName.op_Implicit(nameof (_parent));
  }

  public class SignalName : Node.SignalName
  {
  }
}
