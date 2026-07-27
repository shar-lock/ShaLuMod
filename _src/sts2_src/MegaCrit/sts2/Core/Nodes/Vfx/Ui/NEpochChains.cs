// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Vfx.Ui.NEpochChains
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.Vfx.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Vfx.Ui;

[ScriptPath("res://src/Core/Nodes/Vfx/Ui/NEpochChains.cs")]
public class NEpochChains : TextureRect
{
  [Export]
  private float _duration = 0.5f;
  [Export]
  private Array<NParticlesContainer>? _particles;
  [Export]
  private NParticlesContainer? _endParticles;
  [Export]
  private Curve? _particlesCurve;
  [Export]
  private Curve _brightEnabledCurve;
  [Export]
  private Curve _erosionEnabledCurve;
  [Export]
  private Curve _erosionBaseCurve;
  private static readonly StringName _brightEnabledString = new StringName("bright_enabled");
  private static readonly StringName _erosionEnabledString = new StringName("erosion_enabled");
  private static readonly StringName _erosionBaseString = new StringName("erosion_base");
  private int _previousParticleIndex = -1;
  private ShaderMaterial? _asShaderMaterial;
  private 
  #nullable disable
  NEpochChains.OnAnimationFinishedEventHandler backing_OnAnimationFinished;

  private void UpdateParticles(int index)
  {
    if (this._previousParticleIndex == index)
      return;
    this._previousParticleIndex = index;
    for (int index1 = 0; index1 < this._particles.Count; ++index1)
    {
      if (index1 == index)
        this._particles[index1].Restart();
    }
  }

  private void SetProperties(float interpolation)
  {
    if (this._asShaderMaterial == null)
      return;
    float num1 = this._brightEnabledCurve.Sample(interpolation);
    float num2 = this._erosionEnabledCurve.Sample(interpolation);
    float num3 = this._erosionBaseCurve.Sample(interpolation);
    this._asShaderMaterial.SetShaderParameter(NEpochChains._brightEnabledString, Variant.op_Implicit(num1));
    this._asShaderMaterial.SetShaderParameter(NEpochChains._erosionEnabledString, Variant.op_Implicit(num2));
    this._asShaderMaterial.SetShaderParameter(NEpochChains._erosionBaseString, Variant.op_Implicit(num3));
  }

  public void Unlock() => TaskHelper.RunSafely(this.Unlocking());

  public async 
  #nullable enable
  Task Unlocking()
  {
    this._previousParticleIndex = -1;
    ((CanvasItem) this).SelfModulate = Colors.White;
    double timer = 0.0;
    Material originalMaterial = ((CanvasItem) this).Material;
    this._asShaderMaterial = (ShaderMaterial) ((Resource) originalMaterial).Duplicate(true);
    ((CanvasItem) this).Material = (Material) this._asShaderMaterial;
    this.SetProperties(0.0f);
    while (timer < (double) this._duration)
    {
      float interpolation = (float) timer / this._duration;
      float num1 = this._particlesCurve.Sample(interpolation);
      this.SetProperties(interpolation);
      this.UpdateParticles(Mathf.FloorToInt(num1));
      timer += ((Node) this).GetProcessDeltaTime();
      double num2 = (double) await ((Node) this).AwaitProcessFrame();
    }
    this.SetProperties(1f);
    ((CanvasItem) this).Material = originalMaterial;
    ((GodotObject) this._asShaderMaterial).Dispose();
    ((CanvasItem) this).SelfModulate = new Color(1f, 1f, 1f, 0.0f);
    this._endParticles.Restart();
    ((GodotObject) this).EmitSignal(NEpochChains.SignalName.OnAnimationFinished, Array.Empty<Variant>());
    originalMaterial = (Material) null;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(3)
    {
      new MethodInfo(NEpochChains.MethodName.UpdateParticles, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 2L, StringName.op_Implicit("index"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NEpochChains.MethodName.SetProperties, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 3L, StringName.op_Implicit("interpolation"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NEpochChains.MethodName.Unlock, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NEpochChains.MethodName.UpdateParticles) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.UpdateParticles(VariantUtils.ConvertTo<int>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NEpochChains.MethodName.SetProperties) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.SetProperties(VariantUtils.ConvertTo<float>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NEpochChains.MethodName.Unlock) || ((NativeVariantPtrArgs) ref args).Count != 0)
      return base.InvokeGodotClassMethod(ref method, args, ref ret);
    this.Unlock();
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NEpochChains.MethodName.UpdateParticles) || StringName.op_Equality(ref method, NEpochChains.MethodName.SetProperties) || StringName.op_Equality(ref method, NEpochChains.MethodName.Unlock) || base.HasGodotClassMethod(ref method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NEpochChains.PropertyName._duration))
    {
      this._duration = VariantUtils.ConvertTo<float>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NEpochChains.PropertyName._particles))
    {
      this._particles = VariantUtils.ConvertToArray<NParticlesContainer>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NEpochChains.PropertyName._endParticles))
    {
      this._endParticles = VariantUtils.ConvertTo<NParticlesContainer>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NEpochChains.PropertyName._particlesCurve))
    {
      this._particlesCurve = VariantUtils.ConvertTo<Curve>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NEpochChains.PropertyName._brightEnabledCurve))
    {
      this._brightEnabledCurve = VariantUtils.ConvertTo<Curve>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NEpochChains.PropertyName._erosionEnabledCurve))
    {
      this._erosionEnabledCurve = VariantUtils.ConvertTo<Curve>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NEpochChains.PropertyName._erosionBaseCurve))
    {
      this._erosionBaseCurve = VariantUtils.ConvertTo<Curve>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NEpochChains.PropertyName._previousParticleIndex))
    {
      this._previousParticleIndex = VariantUtils.ConvertTo<int>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NEpochChains.PropertyName._asShaderMaterial))
      return ((GodotObject) this).SetGodotClassPropertyValue(ref name, ref value);
    this._asShaderMaterial = VariantUtils.ConvertTo<ShaderMaterial>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NEpochChains.PropertyName._duration))
    {
      value = VariantUtils.CreateFrom<float>(ref this._duration);
      return true;
    }
    if (StringName.op_Equality(ref name, NEpochChains.PropertyName._particles))
    {
      value = VariantUtils.CreateFromArray<NParticlesContainer>(this._particles);
      return true;
    }
    if (StringName.op_Equality(ref name, NEpochChains.PropertyName._endParticles))
    {
      value = VariantUtils.CreateFrom<NParticlesContainer>(ref this._endParticles);
      return true;
    }
    if (StringName.op_Equality(ref name, NEpochChains.PropertyName._particlesCurve))
    {
      value = VariantUtils.CreateFrom<Curve>(ref this._particlesCurve);
      return true;
    }
    if (StringName.op_Equality(ref name, NEpochChains.PropertyName._brightEnabledCurve))
    {
      value = VariantUtils.CreateFrom<Curve>(ref this._brightEnabledCurve);
      return true;
    }
    if (StringName.op_Equality(ref name, NEpochChains.PropertyName._erosionEnabledCurve))
    {
      value = VariantUtils.CreateFrom<Curve>(ref this._erosionEnabledCurve);
      return true;
    }
    if (StringName.op_Equality(ref name, NEpochChains.PropertyName._erosionBaseCurve))
    {
      value = VariantUtils.CreateFrom<Curve>(ref this._erosionBaseCurve);
      return true;
    }
    if (StringName.op_Equality(ref name, NEpochChains.PropertyName._previousParticleIndex))
    {
      value = VariantUtils.CreateFrom<int>(ref this._previousParticleIndex);
      return true;
    }
    if (!StringName.op_Equality(ref name, NEpochChains.PropertyName._asShaderMaterial))
      return ((GodotObject) this).GetGodotClassPropertyValue(ref name, ref value);
    value = VariantUtils.CreateFrom<ShaderMaterial>(ref this._asShaderMaterial);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 3L, NEpochChains.PropertyName._duration, (PropertyHint) 0L, "", (PropertyUsageFlags) 4102L, true),
      new PropertyInfo((Variant.Type) 28L, NEpochChains.PropertyName._particles, (PropertyHint) 23L, "24/34:Node2D", (PropertyUsageFlags) 4102L, true),
      new PropertyInfo((Variant.Type) 24L, NEpochChains.PropertyName._endParticles, (PropertyHint) 34L, "Node2D", (PropertyUsageFlags) 4102L, true),
      new PropertyInfo((Variant.Type) 24L, NEpochChains.PropertyName._particlesCurve, (PropertyHint) 17L, "Curve", (PropertyUsageFlags) 4102L, true),
      new PropertyInfo((Variant.Type) 24L, NEpochChains.PropertyName._brightEnabledCurve, (PropertyHint) 17L, "Curve", (PropertyUsageFlags) 4102L, true),
      new PropertyInfo((Variant.Type) 24L, NEpochChains.PropertyName._erosionEnabledCurve, (PropertyHint) 17L, "Curve", (PropertyUsageFlags) 4102L, true),
      new PropertyInfo((Variant.Type) 24L, NEpochChains.PropertyName._erosionBaseCurve, (PropertyHint) 17L, "Curve", (PropertyUsageFlags) 4102L, true),
      new PropertyInfo((Variant.Type) 2L, NEpochChains.PropertyName._previousParticleIndex, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NEpochChains.PropertyName._asShaderMaterial, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).SaveGodotObjectData(info);
    info.AddProperty(NEpochChains.PropertyName._duration, Variant.From<float>(ref this._duration));
    info.AddProperty(NEpochChains.PropertyName._particles, Variant.CreateFrom<NParticlesContainer>(this._particles));
    info.AddProperty(NEpochChains.PropertyName._endParticles, Variant.From<NParticlesContainer>(ref this._endParticles));
    info.AddProperty(NEpochChains.PropertyName._particlesCurve, Variant.From<Curve>(ref this._particlesCurve));
    info.AddProperty(NEpochChains.PropertyName._brightEnabledCurve, Variant.From<Curve>(ref this._brightEnabledCurve));
    info.AddProperty(NEpochChains.PropertyName._erosionEnabledCurve, Variant.From<Curve>(ref this._erosionEnabledCurve));
    info.AddProperty(NEpochChains.PropertyName._erosionBaseCurve, Variant.From<Curve>(ref this._erosionBaseCurve));
    info.AddProperty(NEpochChains.PropertyName._previousParticleIndex, Variant.From<int>(ref this._previousParticleIndex));
    info.AddProperty(NEpochChains.PropertyName._asShaderMaterial, Variant.From<ShaderMaterial>(ref this._asShaderMaterial));
    info.AddSignalEventDelegate(NEpochChains.SignalName.OnAnimationFinished, (Delegate) this.backing_OnAnimationFinished);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NEpochChains.PropertyName._duration, ref variant1))
      this._duration = ((Variant) ref variant1).As<float>();
    Variant variant2;
    if (info.TryGetProperty(NEpochChains.PropertyName._particles, ref variant2))
      this._particles = ((Variant) ref variant2).AsGodotArray<NParticlesContainer>();
    Variant variant3;
    if (info.TryGetProperty(NEpochChains.PropertyName._endParticles, ref variant3))
      this._endParticles = ((Variant) ref variant3).As<NParticlesContainer>();
    Variant variant4;
    if (info.TryGetProperty(NEpochChains.PropertyName._particlesCurve, ref variant4))
      this._particlesCurve = ((Variant) ref variant4).As<Curve>();
    Variant variant5;
    if (info.TryGetProperty(NEpochChains.PropertyName._brightEnabledCurve, ref variant5))
      this._brightEnabledCurve = ((Variant) ref variant5).As<Curve>();
    Variant variant6;
    if (info.TryGetProperty(NEpochChains.PropertyName._erosionEnabledCurve, ref variant6))
      this._erosionEnabledCurve = ((Variant) ref variant6).As<Curve>();
    Variant variant7;
    if (info.TryGetProperty(NEpochChains.PropertyName._erosionBaseCurve, ref variant7))
      this._erosionBaseCurve = ((Variant) ref variant7).As<Curve>();
    Variant variant8;
    if (info.TryGetProperty(NEpochChains.PropertyName._previousParticleIndex, ref variant8))
      this._previousParticleIndex = ((Variant) ref variant8).As<int>();
    Variant variant9;
    if (info.TryGetProperty(NEpochChains.PropertyName._asShaderMaterial, ref variant9))
      this._asShaderMaterial = ((Variant) ref variant9).As<ShaderMaterial>();
    NEpochChains.OnAnimationFinishedEventHandler finishedEventHandler;
    if (!info.TryGetSignalEventDelegate<NEpochChains.OnAnimationFinishedEventHandler>(NEpochChains.SignalName.OnAnimationFinished, ref finishedEventHandler))
      return;
    this.backing_OnAnimationFinished = finishedEventHandler;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<MethodInfo> GetGodotSignalList()
  {
    return new List<MethodInfo>(1)
    {
      new MethodInfo(NEpochChains.SignalName.OnAnimationFinished, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  public event NEpochChains.OnAnimationFinishedEventHandler OnAnimationFinished
  {
    add => this.backing_OnAnimationFinished += value;
    remove => this.backing_OnAnimationFinished -= value;
  }

  protected void EmitSignalOnAnimationFinished()
  {
    ((GodotObject) this).EmitSignal(NEpochChains.SignalName.OnAnimationFinished, Array.Empty<Variant>());
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RaiseGodotClassSignalCallbacks(
    in godot_string_name signal,
    NativeVariantPtrArgs args)
  {
    if (StringName.op_Equality(ref signal, NEpochChains.SignalName.OnAnimationFinished) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      NEpochChains.OnAnimationFinishedEventHandler animationFinished = this.backing_OnAnimationFinished;
      if (animationFinished == null)
        return;
      animationFinished();
    }
    else
      ((GodotObject) this).RaiseGodotClassSignalCallbacks(ref signal, args);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassSignal(in godot_string_name signal)
  {
    return StringName.op_Equality(ref signal, NEpochChains.SignalName.OnAnimationFinished) || base.HasGodotClassSignal(ref signal);
  }

  [Signal]
  public delegate void OnAnimationFinishedEventHandler();

  public class MethodName : TextureRect.MethodName
  {
    public static readonly StringName UpdateParticles = StringName.op_Implicit(nameof (UpdateParticles));
    public static readonly StringName SetProperties = StringName.op_Implicit(nameof (SetProperties));
    public static readonly StringName Unlock = StringName.op_Implicit(nameof (Unlock));
  }

  public class PropertyName : TextureRect.PropertyName
  {
    public static readonly StringName _duration = StringName.op_Implicit(nameof (_duration));
    public static readonly StringName _particles = StringName.op_Implicit(nameof (_particles));
    public static readonly StringName _endParticles = StringName.op_Implicit(nameof (_endParticles));
    public static readonly StringName _particlesCurve = StringName.op_Implicit(nameof (_particlesCurve));
    public static readonly StringName _brightEnabledCurve = StringName.op_Implicit(nameof (_brightEnabledCurve));
    public static readonly StringName _erosionEnabledCurve = StringName.op_Implicit(nameof (_erosionEnabledCurve));
    public static readonly StringName _erosionBaseCurve = StringName.op_Implicit(nameof (_erosionBaseCurve));
    public static readonly StringName _previousParticleIndex = StringName.op_Implicit(nameof (_previousParticleIndex));
    public static readonly StringName _asShaderMaterial = StringName.op_Implicit(nameof (_asShaderMaterial));
  }

  public class SignalName : TextureRect.SignalName
  {
    public static readonly StringName OnAnimationFinished = StringName.op_Implicit(nameof (OnAnimationFinished));
  }
}
