// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Vfx.NCeremonialBeastVfx
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Bindings.MegaSpine;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Vfx;

[GlobalClass]
[ScriptPath("res://src/Core/Nodes/Vfx/NCeremonialBeastVfx.cs")]
public class NCeremonialBeastVfx : Node, IDeathDelayer
{
  [Export]
  private GpuParticles2D _deathParticles;
  [Export]
  private CpuParticles2D _energyParticlesFront;
  [Export]
  private CpuParticles2D _energyParticlesBack;
  [Export]
  private Node2D _plowStartTarget;
  [Export]
  private Node2D _plowEndTarget;
  private Node2D _parent;
  private MegaSprite _animController;
  private Vector2 _globalPlowTarget;
  private Vector2 _globalPlowEndTarget;
  private readonly TaskCompletionSource _deathTask = new TaskCompletionSource();

  public override void _Ready()
  {
    this._parent = this.GetParent<Node2D>();
    this._animController = new MegaSprite(Variant.op_Implicit((GodotObject) this._parent));
    this._animController.ConnectAnimationEvent(Callable.From<GodotObject, GodotObject, GodotObject, GodotObject>(new Action<GodotObject, GodotObject, GodotObject, GodotObject>(this.OnAnimationEvent)));
    this._deathParticles.OneShot = true;
    this._deathParticles.Emitting = false;
    this._energyParticlesBack.Emitting = true;
    this._energyParticlesFront.Emitting = true;
    this._globalPlowTarget = this._plowStartTarget.GlobalPosition;
    this._globalPlowEndTarget = this._plowEndTarget.GlobalPosition;
  }

  private void OnAnimationEvent(
    GodotObject _,
    GodotObject __,
    GodotObject ___,
    GodotObject spineEvent)
  {
    switch (new MegaEvent(Variant.op_Implicit(spineEvent)).GetData().GetEventName())
    {
      case "turnOffEnergy":
        this.TurnOffEnergyParticles();
        break;
      case "turnOnEnergy":
        this.TurnOnEnergyParticles();
        break;
      case "deathParticles":
        this.TurnOnDeathParticles();
        break;
      case "plowStart":
        this.OnPlowStart();
        break;
      case "plowEnd":
        this.OnPlowEnd();
        break;
    }
  }

  public Task GetDelayTask() => this._deathTask.Task;

  private void TurnOnDeathParticles()
  {
    this._deathParticles.Restart();
    TaskHelper.RunSafely(this.FinishTaskWhenDeathParticlesFinished());
  }

  private async Task FinishTaskWhenDeathParticlesFinished()
  {
    await ((GodotObject) this._deathParticles).AwaitSignal(CpuParticles2D.SignalName.Finished, (Node) this);
    this._deathTask.SetResult();
  }

  private void TurnOnEnergyParticles()
  {
    this._energyParticlesFront.Emitting = true;
    this._energyParticlesBack.Emitting = true;
  }

  private void TurnOffEnergyParticles()
  {
    this._energyParticlesFront.Emitting = false;
    this._energyParticlesBack.Emitting = false;
  }

  private void OnPlowStart() => this._plowStartTarget.GlobalPosition = this._globalPlowTarget;

  private void OnPlowEnd() => this._plowEndTarget.GlobalPosition = this._globalPlowEndTarget;

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(7)
    {
      new MethodInfo(NCeremonialBeastVfx.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCeremonialBeastVfx.MethodName.OnAnimationEvent, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("_"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Object"), false),
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("__"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Object"), false),
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("___"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Object"), false),
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("spineEvent"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Object"), false)
      }, (List<Variant>) null),
      new MethodInfo(NCeremonialBeastVfx.MethodName.TurnOnDeathParticles, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCeremonialBeastVfx.MethodName.TurnOnEnergyParticles, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCeremonialBeastVfx.MethodName.TurnOffEnergyParticles, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCeremonialBeastVfx.MethodName.OnPlowStart, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCeremonialBeastVfx.MethodName.OnPlowEnd, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NCeremonialBeastVfx.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      base._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCeremonialBeastVfx.MethodName.OnAnimationEvent) && ((NativeVariantPtrArgs) ref args).Count == 4)
    {
      this.OnAnimationEvent(VariantUtils.ConvertTo<GodotObject>(ref ((NativeVariantPtrArgs) ref args)[0]), VariantUtils.ConvertTo<GodotObject>(ref ((NativeVariantPtrArgs) ref args)[1]), VariantUtils.ConvertTo<GodotObject>(ref ((NativeVariantPtrArgs) ref args)[2]), VariantUtils.ConvertTo<GodotObject>(ref ((NativeVariantPtrArgs) ref args)[3]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCeremonialBeastVfx.MethodName.TurnOnDeathParticles) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.TurnOnDeathParticles();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCeremonialBeastVfx.MethodName.TurnOnEnergyParticles) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.TurnOnEnergyParticles();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCeremonialBeastVfx.MethodName.TurnOffEnergyParticles) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.TurnOffEnergyParticles();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCeremonialBeastVfx.MethodName.OnPlowStart) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnPlowStart();
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NCeremonialBeastVfx.MethodName.OnPlowEnd) || ((NativeVariantPtrArgs) ref args).Count != 0)
      return base.InvokeGodotClassMethod(ref method, args, ref ret);
    this.OnPlowEnd();
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NCeremonialBeastVfx.MethodName._Ready) || StringName.op_Equality(ref method, NCeremonialBeastVfx.MethodName.OnAnimationEvent) || StringName.op_Equality(ref method, NCeremonialBeastVfx.MethodName.TurnOnDeathParticles) || StringName.op_Equality(ref method, NCeremonialBeastVfx.MethodName.TurnOnEnergyParticles) || StringName.op_Equality(ref method, NCeremonialBeastVfx.MethodName.TurnOffEnergyParticles) || StringName.op_Equality(ref method, NCeremonialBeastVfx.MethodName.OnPlowStart) || StringName.op_Equality(ref method, NCeremonialBeastVfx.MethodName.OnPlowEnd) || base.HasGodotClassMethod(ref method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NCeremonialBeastVfx.PropertyName._deathParticles))
    {
      this._deathParticles = VariantUtils.ConvertTo<GpuParticles2D>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCeremonialBeastVfx.PropertyName._energyParticlesFront))
    {
      this._energyParticlesFront = VariantUtils.ConvertTo<CpuParticles2D>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCeremonialBeastVfx.PropertyName._energyParticlesBack))
    {
      this._energyParticlesBack = VariantUtils.ConvertTo<CpuParticles2D>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCeremonialBeastVfx.PropertyName._plowStartTarget))
    {
      this._plowStartTarget = VariantUtils.ConvertTo<Node2D>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCeremonialBeastVfx.PropertyName._plowEndTarget))
    {
      this._plowEndTarget = VariantUtils.ConvertTo<Node2D>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCeremonialBeastVfx.PropertyName._parent))
    {
      this._parent = VariantUtils.ConvertTo<Node2D>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCeremonialBeastVfx.PropertyName._globalPlowTarget))
    {
      this._globalPlowTarget = VariantUtils.ConvertTo<Vector2>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NCeremonialBeastVfx.PropertyName._globalPlowEndTarget))
      return ((GodotObject) this).SetGodotClassPropertyValue(ref name, ref value);
    this._globalPlowEndTarget = VariantUtils.ConvertTo<Vector2>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NCeremonialBeastVfx.PropertyName._deathParticles))
    {
      value = VariantUtils.CreateFrom<GpuParticles2D>(ref this._deathParticles);
      return true;
    }
    if (StringName.op_Equality(ref name, NCeremonialBeastVfx.PropertyName._energyParticlesFront))
    {
      value = VariantUtils.CreateFrom<CpuParticles2D>(ref this._energyParticlesFront);
      return true;
    }
    if (StringName.op_Equality(ref name, NCeremonialBeastVfx.PropertyName._energyParticlesBack))
    {
      value = VariantUtils.CreateFrom<CpuParticles2D>(ref this._energyParticlesBack);
      return true;
    }
    if (StringName.op_Equality(ref name, NCeremonialBeastVfx.PropertyName._plowStartTarget))
    {
      value = VariantUtils.CreateFrom<Node2D>(ref this._plowStartTarget);
      return true;
    }
    if (StringName.op_Equality(ref name, NCeremonialBeastVfx.PropertyName._plowEndTarget))
    {
      value = VariantUtils.CreateFrom<Node2D>(ref this._plowEndTarget);
      return true;
    }
    if (StringName.op_Equality(ref name, NCeremonialBeastVfx.PropertyName._parent))
    {
      value = VariantUtils.CreateFrom<Node2D>(ref this._parent);
      return true;
    }
    if (StringName.op_Equality(ref name, NCeremonialBeastVfx.PropertyName._globalPlowTarget))
    {
      value = VariantUtils.CreateFrom<Vector2>(ref this._globalPlowTarget);
      return true;
    }
    if (!StringName.op_Equality(ref name, NCeremonialBeastVfx.PropertyName._globalPlowEndTarget))
      return ((GodotObject) this).GetGodotClassPropertyValue(ref name, ref value);
    value = VariantUtils.CreateFrom<Vector2>(ref this._globalPlowEndTarget);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NCeremonialBeastVfx.PropertyName._deathParticles, (PropertyHint) 34L, "GPUParticles2D", (PropertyUsageFlags) 4102L, true),
      new PropertyInfo((Variant.Type) 24L, NCeremonialBeastVfx.PropertyName._energyParticlesFront, (PropertyHint) 34L, "CPUParticles2D", (PropertyUsageFlags) 4102L, true),
      new PropertyInfo((Variant.Type) 24L, NCeremonialBeastVfx.PropertyName._energyParticlesBack, (PropertyHint) 34L, "CPUParticles2D", (PropertyUsageFlags) 4102L, true),
      new PropertyInfo((Variant.Type) 24L, NCeremonialBeastVfx.PropertyName._plowStartTarget, (PropertyHint) 34L, "Node2D", (PropertyUsageFlags) 4102L, true),
      new PropertyInfo((Variant.Type) 24L, NCeremonialBeastVfx.PropertyName._plowEndTarget, (PropertyHint) 34L, "Node2D", (PropertyUsageFlags) 4102L, true),
      new PropertyInfo((Variant.Type) 24L, NCeremonialBeastVfx.PropertyName._parent, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 5L, NCeremonialBeastVfx.PropertyName._globalPlowTarget, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 5L, NCeremonialBeastVfx.PropertyName._globalPlowEndTarget, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).SaveGodotObjectData(info);
    info.AddProperty(NCeremonialBeastVfx.PropertyName._deathParticles, Variant.From<GpuParticles2D>(ref this._deathParticles));
    info.AddProperty(NCeremonialBeastVfx.PropertyName._energyParticlesFront, Variant.From<CpuParticles2D>(ref this._energyParticlesFront));
    info.AddProperty(NCeremonialBeastVfx.PropertyName._energyParticlesBack, Variant.From<CpuParticles2D>(ref this._energyParticlesBack));
    info.AddProperty(NCeremonialBeastVfx.PropertyName._plowStartTarget, Variant.From<Node2D>(ref this._plowStartTarget));
    info.AddProperty(NCeremonialBeastVfx.PropertyName._plowEndTarget, Variant.From<Node2D>(ref this._plowEndTarget));
    info.AddProperty(NCeremonialBeastVfx.PropertyName._parent, Variant.From<Node2D>(ref this._parent));
    info.AddProperty(NCeremonialBeastVfx.PropertyName._globalPlowTarget, Variant.From<Vector2>(ref this._globalPlowTarget));
    info.AddProperty(NCeremonialBeastVfx.PropertyName._globalPlowEndTarget, Variant.From<Vector2>(ref this._globalPlowEndTarget));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NCeremonialBeastVfx.PropertyName._deathParticles, ref variant1))
      this._deathParticles = ((Variant) ref variant1).As<GpuParticles2D>();
    Variant variant2;
    if (info.TryGetProperty(NCeremonialBeastVfx.PropertyName._energyParticlesFront, ref variant2))
      this._energyParticlesFront = ((Variant) ref variant2).As<CpuParticles2D>();
    Variant variant3;
    if (info.TryGetProperty(NCeremonialBeastVfx.PropertyName._energyParticlesBack, ref variant3))
      this._energyParticlesBack = ((Variant) ref variant3).As<CpuParticles2D>();
    Variant variant4;
    if (info.TryGetProperty(NCeremonialBeastVfx.PropertyName._plowStartTarget, ref variant4))
      this._plowStartTarget = ((Variant) ref variant4).As<Node2D>();
    Variant variant5;
    if (info.TryGetProperty(NCeremonialBeastVfx.PropertyName._plowEndTarget, ref variant5))
      this._plowEndTarget = ((Variant) ref variant5).As<Node2D>();
    Variant variant6;
    if (info.TryGetProperty(NCeremonialBeastVfx.PropertyName._parent, ref variant6))
      this._parent = ((Variant) ref variant6).As<Node2D>();
    Variant variant7;
    if (info.TryGetProperty(NCeremonialBeastVfx.PropertyName._globalPlowTarget, ref variant7))
      this._globalPlowTarget = ((Variant) ref variant7).As<Vector2>();
    Variant variant8;
    if (!info.TryGetProperty(NCeremonialBeastVfx.PropertyName._globalPlowEndTarget, ref variant8))
      return;
    this._globalPlowEndTarget = ((Variant) ref variant8).As<Vector2>();
  }

  public class MethodName : Node.MethodName
  {
    public static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName OnAnimationEvent = StringName.op_Implicit(nameof (OnAnimationEvent));
    public static readonly StringName TurnOnDeathParticles = StringName.op_Implicit(nameof (TurnOnDeathParticles));
    public static readonly StringName TurnOnEnergyParticles = StringName.op_Implicit(nameof (TurnOnEnergyParticles));
    public static readonly StringName TurnOffEnergyParticles = StringName.op_Implicit(nameof (TurnOffEnergyParticles));
    public static readonly StringName OnPlowStart = StringName.op_Implicit(nameof (OnPlowStart));
    public static readonly StringName OnPlowEnd = StringName.op_Implicit(nameof (OnPlowEnd));
  }

  public class PropertyName : Node.PropertyName
  {
    public static readonly StringName _deathParticles = StringName.op_Implicit(nameof (_deathParticles));
    public static readonly StringName _energyParticlesFront = StringName.op_Implicit(nameof (_energyParticlesFront));
    public static readonly StringName _energyParticlesBack = StringName.op_Implicit(nameof (_energyParticlesBack));
    public static readonly StringName _plowStartTarget = StringName.op_Implicit(nameof (_plowStartTarget));
    public static readonly StringName _plowEndTarget = StringName.op_Implicit(nameof (_plowEndTarget));
    public static readonly StringName _parent = StringName.op_Implicit(nameof (_parent));
    public static readonly StringName _globalPlowTarget = StringName.op_Implicit(nameof (_globalPlowTarget));
    public static readonly StringName _globalPlowEndTarget = StringName.op_Implicit(nameof (_globalPlowEndTarget));
  }

  public class SignalName : Node.SignalName
  {
  }
}
