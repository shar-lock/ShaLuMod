// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Vfx.NLivingGasVfx
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

[ScriptPath("res://src/Core/Nodes/Vfx/NLivingGasVfx.cs")]
public class NLivingGasVfx : Node
{
  private static readonly StringName _alphaStep = new StringName("AlphaStep");
  private GpuParticles2D _attackPuffParticles;
  private GpuParticles2D _attackSparkParticles;
  private GpuParticles2D _debuffPuffParticles;
  private GpuParticles2D _gasPuffParticles;
  private MegaSlotNode _smokeSlot1;
  private MegaSlotNode _smokeSlot2;
  private MegaSlotNode _smokeSlot3;
  private List<ShaderMaterial?> _smokeMaterials;
  private List<Vector2> _smokeSteps;
  private Node2D _parent;
  private MegaSprite _megaSprite;

  public override void _Ready()
  {
    this._parent = this.GetParent<Node2D>();
    this._attackPuffParticles = this.GetNode<GpuParticles2D>(NodePath.op_Implicit("../AttackFXNode/AttackPuffParticles"));
    this._attackSparkParticles = this.GetNode<GpuParticles2D>(NodePath.op_Implicit("../AttackFXNode/AttackSparkParticles"));
    this._debuffPuffParticles = this.GetNode<GpuParticles2D>(NodePath.op_Implicit("../AttackFXNode/DebuffPuffParticles"));
    this._gasPuffParticles = this.GetNode<GpuParticles2D>(NodePath.op_Implicit("../AttackFXNode/GasPuffParticles"));
    this._smokeSlot1 = new MegaSlotNode(Variant.op_Implicit((GodotObject) this.GetNode<Node2D>(NodePath.op_Implicit("../SmokeSlot1"))));
    this._smokeSlot2 = new MegaSlotNode(Variant.op_Implicit((GodotObject) this.GetNode<Node2D>(NodePath.op_Implicit("../SmokeSlot2"))));
    this._smokeSlot3 = new MegaSlotNode(Variant.op_Implicit((GodotObject) this.GetNode<Node2D>(NodePath.op_Implicit("../SmokeSlot3"))));
    this._smokeMaterials = new List<ShaderMaterial>();
    this._smokeMaterials.Add(this._smokeSlot1.GetNormalMaterial() as ShaderMaterial);
    this._smokeMaterials.Add(this._smokeSlot2.GetNormalMaterial() as ShaderMaterial);
    this._smokeMaterials.Add(this._smokeSlot3.GetNormalMaterial() as ShaderMaterial);
    this._smokeSteps = new List<Vector2>();
    this._smokeSteps.Add(Variant.op_Explicit(this._smokeMaterials[0].GetShaderParameter(NLivingGasVfx._alphaStep)));
    this._smokeSteps.Add(Variant.op_Explicit(this._smokeMaterials[1].GetShaderParameter(NLivingGasVfx._alphaStep)));
    this._smokeSteps.Add(Variant.op_Explicit(this._smokeMaterials[2].GetShaderParameter(NLivingGasVfx._alphaStep)));
    this._attackPuffParticles.Emitting = false;
    this._attackSparkParticles.Emitting = false;
    this._debuffPuffParticles.Emitting = false;
    this._gasPuffParticles.Emitting = false;
    this._megaSprite = new MegaSprite(Variant.op_Implicit((GodotObject) this.GetParent<Node2D>()));
    this._megaSprite.ConnectAnimationEvent(Callable.From<GodotObject, GodotObject, GodotObject, GodotObject>(new Action<GodotObject, GodotObject, GodotObject, GodotObject>(this.OnAnimationEvent)));
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
      case 8:
        if (!(eventName == "hurt_end"))
          break;
        this.OnHurtEnd();
        break;
      case 9:
        if (!(eventName == "dissipate"))
          break;
        this.OnDissipate();
        break;
      case 10:
        switch (eventName[0])
        {
          case 'a':
            if (!(eventName == "attack_end"))
              return;
            this.OnAttackEnd();
            return;
          case 'd':
            if (!(eventName == "debuff_end"))
              return;
            this.OnDebuffEnd();
            return;
          case 'h':
            if (!(eventName == "hurt_start"))
              return;
            this.OnHurtStart();
            return;
          default:
            return;
        }
      case 12:
        switch (eventName[0])
        {
          case 'a':
            if (!(eventName == "attack_start"))
              return;
            this.OnAttackStart();
            return;
          case 'd':
            if (!(eventName == "debuff_start"))
              return;
            this.OnDebuffStart();
            return;
          case 'r':
            if (!(eventName == "reconstitute"))
              return;
            this.OnReconstitute();
            return;
          default:
            return;
        }
      case 14:
        if (!(eventName == "gas_breath_end"))
          break;
        this.OnDeathBreathEnd();
        break;
      case 16 /*0x10*/:
        if (!(eventName == "gas_breath_start"))
          break;
        this.OnDeathBreathStart();
        break;
    }
  }

  private void OnAttackStart()
  {
    this._attackPuffParticles.Emitting = true;
    this._attackSparkParticles.Amount = 24;
    this._attackSparkParticles.Restart();
  }

  private void OnAttackEnd()
  {
    this._attackPuffParticles.Emitting = false;
    this._attackSparkParticles.Emitting = false;
  }

  private void OnHurtStart()
  {
    this._attackSparkParticles.Amount = 100;
    this._attackSparkParticles.Emitting = true;
  }

  private void OnHurtEnd() => this._attackSparkParticles.Emitting = false;

  private void OnDebuffStart() => this._debuffPuffParticles.Emitting = true;

  private void OnDebuffEnd() => this._debuffPuffParticles.Emitting = false;

  private void OnDissipate()
  {
    Tween tween = this.CreateTween();
    tween.TweenMethod(Callable.From<float>(new Action<float>(this.DissipateFunction)), Variant.op_Implicit(0.0f), Variant.op_Implicit(1f), 1.3999999761581421);
    tween.SetEase((Tween.EaseType) 0L).SetTrans((Tween.TransitionType) 3L);
  }

  private void DissipateFunction(float t)
  {
    for (int index = 0; index < this._smokeMaterials.Count; ++index)
      this._smokeMaterials[index].SetShaderParameter(NLivingGasVfx._alphaStep, Variant.op_Implicit(Vector2.op_Addition(this._smokeSteps[index], Vector2.op_Multiply(Vector2.op_Subtraction(Vector2.One, this._smokeSteps[index]), t))));
  }

  private void OnDeathBreathStart() => this._gasPuffParticles.Emitting = true;

  private void OnDeathBreathEnd() => this._gasPuffParticles.Emitting = false;

  private void OnReconstitute()
  {
    for (int index = 0; index < this._smokeMaterials.Count; ++index)
      this._smokeMaterials[index].SetShaderParameter(NLivingGasVfx._alphaStep, Variant.op_Implicit(this._smokeSteps[index]));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(13)
    {
      new MethodInfo(NLivingGasVfx.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NLivingGasVfx.MethodName.OnAnimationEvent, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("_"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Object"), false),
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("__"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Object"), false),
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("___"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Object"), false),
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("spineEvent"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Object"), false)
      }, (List<Variant>) null),
      new MethodInfo(NLivingGasVfx.MethodName.OnAttackStart, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NLivingGasVfx.MethodName.OnAttackEnd, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NLivingGasVfx.MethodName.OnHurtStart, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NLivingGasVfx.MethodName.OnHurtEnd, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NLivingGasVfx.MethodName.OnDebuffStart, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NLivingGasVfx.MethodName.OnDebuffEnd, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NLivingGasVfx.MethodName.OnDissipate, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NLivingGasVfx.MethodName.DissipateFunction, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 3L, StringName.op_Implicit("t"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NLivingGasVfx.MethodName.OnDeathBreathStart, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NLivingGasVfx.MethodName.OnDeathBreathEnd, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NLivingGasVfx.MethodName.OnReconstitute, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NLivingGasVfx.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      base._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NLivingGasVfx.MethodName.OnAnimationEvent) && ((NativeVariantPtrArgs) ref args).Count == 4)
    {
      this.OnAnimationEvent(VariantUtils.ConvertTo<GodotObject>(ref ((NativeVariantPtrArgs) ref args)[0]), VariantUtils.ConvertTo<GodotObject>(ref ((NativeVariantPtrArgs) ref args)[1]), VariantUtils.ConvertTo<GodotObject>(ref ((NativeVariantPtrArgs) ref args)[2]), VariantUtils.ConvertTo<GodotObject>(ref ((NativeVariantPtrArgs) ref args)[3]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NLivingGasVfx.MethodName.OnAttackStart) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnAttackStart();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NLivingGasVfx.MethodName.OnAttackEnd) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnAttackEnd();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NLivingGasVfx.MethodName.OnHurtStart) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnHurtStart();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NLivingGasVfx.MethodName.OnHurtEnd) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnHurtEnd();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NLivingGasVfx.MethodName.OnDebuffStart) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnDebuffStart();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NLivingGasVfx.MethodName.OnDebuffEnd) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnDebuffEnd();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NLivingGasVfx.MethodName.OnDissipate) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnDissipate();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NLivingGasVfx.MethodName.DissipateFunction) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.DissipateFunction(VariantUtils.ConvertTo<float>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NLivingGasVfx.MethodName.OnDeathBreathStart) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnDeathBreathStart();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NLivingGasVfx.MethodName.OnDeathBreathEnd) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnDeathBreathEnd();
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NLivingGasVfx.MethodName.OnReconstitute) || ((NativeVariantPtrArgs) ref args).Count != 0)
      return base.InvokeGodotClassMethod(ref method, args, ref ret);
    this.OnReconstitute();
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NLivingGasVfx.MethodName._Ready) || StringName.op_Equality(ref method, NLivingGasVfx.MethodName.OnAnimationEvent) || StringName.op_Equality(ref method, NLivingGasVfx.MethodName.OnAttackStart) || StringName.op_Equality(ref method, NLivingGasVfx.MethodName.OnAttackEnd) || StringName.op_Equality(ref method, NLivingGasVfx.MethodName.OnHurtStart) || StringName.op_Equality(ref method, NLivingGasVfx.MethodName.OnHurtEnd) || StringName.op_Equality(ref method, NLivingGasVfx.MethodName.OnDebuffStart) || StringName.op_Equality(ref method, NLivingGasVfx.MethodName.OnDebuffEnd) || StringName.op_Equality(ref method, NLivingGasVfx.MethodName.OnDissipate) || StringName.op_Equality(ref method, NLivingGasVfx.MethodName.DissipateFunction) || StringName.op_Equality(ref method, NLivingGasVfx.MethodName.OnDeathBreathStart) || StringName.op_Equality(ref method, NLivingGasVfx.MethodName.OnDeathBreathEnd) || StringName.op_Equality(ref method, NLivingGasVfx.MethodName.OnReconstitute) || base.HasGodotClassMethod(ref method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NLivingGasVfx.PropertyName._attackPuffParticles))
    {
      this._attackPuffParticles = VariantUtils.ConvertTo<GpuParticles2D>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NLivingGasVfx.PropertyName._attackSparkParticles))
    {
      this._attackSparkParticles = VariantUtils.ConvertTo<GpuParticles2D>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NLivingGasVfx.PropertyName._debuffPuffParticles))
    {
      this._debuffPuffParticles = VariantUtils.ConvertTo<GpuParticles2D>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NLivingGasVfx.PropertyName._gasPuffParticles))
    {
      this._gasPuffParticles = VariantUtils.ConvertTo<GpuParticles2D>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NLivingGasVfx.PropertyName._parent))
      return ((GodotObject) this).SetGodotClassPropertyValue(ref name, ref value);
    this._parent = VariantUtils.ConvertTo<Node2D>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NLivingGasVfx.PropertyName._attackPuffParticles))
    {
      value = VariantUtils.CreateFrom<GpuParticles2D>(ref this._attackPuffParticles);
      return true;
    }
    if (StringName.op_Equality(ref name, NLivingGasVfx.PropertyName._attackSparkParticles))
    {
      value = VariantUtils.CreateFrom<GpuParticles2D>(ref this._attackSparkParticles);
      return true;
    }
    if (StringName.op_Equality(ref name, NLivingGasVfx.PropertyName._debuffPuffParticles))
    {
      value = VariantUtils.CreateFrom<GpuParticles2D>(ref this._debuffPuffParticles);
      return true;
    }
    if (StringName.op_Equality(ref name, NLivingGasVfx.PropertyName._gasPuffParticles))
    {
      value = VariantUtils.CreateFrom<GpuParticles2D>(ref this._gasPuffParticles);
      return true;
    }
    if (!StringName.op_Equality(ref name, NLivingGasVfx.PropertyName._parent))
      return ((GodotObject) this).GetGodotClassPropertyValue(ref name, ref value);
    value = VariantUtils.CreateFrom<Node2D>(ref this._parent);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NLivingGasVfx.PropertyName._attackPuffParticles, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NLivingGasVfx.PropertyName._attackSparkParticles, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NLivingGasVfx.PropertyName._debuffPuffParticles, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NLivingGasVfx.PropertyName._gasPuffParticles, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NLivingGasVfx.PropertyName._parent, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).SaveGodotObjectData(info);
    info.AddProperty(NLivingGasVfx.PropertyName._attackPuffParticles, Variant.From<GpuParticles2D>(ref this._attackPuffParticles));
    info.AddProperty(NLivingGasVfx.PropertyName._attackSparkParticles, Variant.From<GpuParticles2D>(ref this._attackSparkParticles));
    info.AddProperty(NLivingGasVfx.PropertyName._debuffPuffParticles, Variant.From<GpuParticles2D>(ref this._debuffPuffParticles));
    info.AddProperty(NLivingGasVfx.PropertyName._gasPuffParticles, Variant.From<GpuParticles2D>(ref this._gasPuffParticles));
    info.AddProperty(NLivingGasVfx.PropertyName._parent, Variant.From<Node2D>(ref this._parent));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NLivingGasVfx.PropertyName._attackPuffParticles, ref variant1))
      this._attackPuffParticles = ((Variant) ref variant1).As<GpuParticles2D>();
    Variant variant2;
    if (info.TryGetProperty(NLivingGasVfx.PropertyName._attackSparkParticles, ref variant2))
      this._attackSparkParticles = ((Variant) ref variant2).As<GpuParticles2D>();
    Variant variant3;
    if (info.TryGetProperty(NLivingGasVfx.PropertyName._debuffPuffParticles, ref variant3))
      this._debuffPuffParticles = ((Variant) ref variant3).As<GpuParticles2D>();
    Variant variant4;
    if (info.TryGetProperty(NLivingGasVfx.PropertyName._gasPuffParticles, ref variant4))
      this._gasPuffParticles = ((Variant) ref variant4).As<GpuParticles2D>();
    Variant variant5;
    if (!info.TryGetProperty(NLivingGasVfx.PropertyName._parent, ref variant5))
      return;
    this._parent = ((Variant) ref variant5).As<Node2D>();
  }

  public class MethodName : Node.MethodName
  {
    public static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName OnAnimationEvent = StringName.op_Implicit(nameof (OnAnimationEvent));
    public static readonly StringName OnAttackStart = StringName.op_Implicit(nameof (OnAttackStart));
    public static readonly StringName OnAttackEnd = StringName.op_Implicit(nameof (OnAttackEnd));
    public static readonly StringName OnHurtStart = StringName.op_Implicit(nameof (OnHurtStart));
    public static readonly StringName OnHurtEnd = StringName.op_Implicit(nameof (OnHurtEnd));
    public static readonly StringName OnDebuffStart = StringName.op_Implicit(nameof (OnDebuffStart));
    public static readonly StringName OnDebuffEnd = StringName.op_Implicit(nameof (OnDebuffEnd));
    public static readonly StringName OnDissipate = StringName.op_Implicit(nameof (OnDissipate));
    public static readonly StringName DissipateFunction = StringName.op_Implicit(nameof (DissipateFunction));
    public static readonly StringName OnDeathBreathStart = StringName.op_Implicit(nameof (OnDeathBreathStart));
    public static readonly StringName OnDeathBreathEnd = StringName.op_Implicit(nameof (OnDeathBreathEnd));
    public static readonly StringName OnReconstitute = StringName.op_Implicit(nameof (OnReconstitute));
  }

  public class PropertyName : Node.PropertyName
  {
    public static readonly StringName _attackPuffParticles = StringName.op_Implicit(nameof (_attackPuffParticles));
    public static readonly StringName _attackSparkParticles = StringName.op_Implicit(nameof (_attackSparkParticles));
    public static readonly StringName _debuffPuffParticles = StringName.op_Implicit(nameof (_debuffPuffParticles));
    public static readonly StringName _gasPuffParticles = StringName.op_Implicit(nameof (_gasPuffParticles));
    public static readonly StringName _parent = StringName.op_Implicit(nameof (_parent));
  }

  public class SignalName : Node.SignalName
  {
  }
}
