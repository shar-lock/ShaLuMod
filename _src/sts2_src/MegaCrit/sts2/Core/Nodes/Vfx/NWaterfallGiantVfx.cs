// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Vfx.NWaterfallGiantVfx
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
[ScriptPath("res://src/Core/Nodes/Vfx/NWaterfallGiantVfx.cs")]
public class NWaterfallGiantVfx : Node
{
  private GpuParticles2D _steam1Particles;
  private GpuParticles2D _steam2Particles;
  private GpuParticles2D _steam3Particles;
  private GpuParticles2D _steam4Particles;
  private GpuParticles2D _steam5Particles;
  private GpuParticles2D _steam6Particles;
  private GpuParticles2D _steamLeakParticles1;
  private GpuParticles2D _steamLeakParticles2;
  private GpuParticles2D _steamLeakParticles3;
  private GpuParticles2D _mistParticles;
  private GpuParticles2D _mouthParticles;
  private GpuParticles2D _dropletParticles;
  private ParticleProcessMaterial _leakProcMat1;
  private ParticleProcessMaterial _leakProcMat2;
  private ParticleProcessMaterial _leakProcMat3;
  private bool _isDead;
  private Node2D _parent;
  private MegaSprite _animController;

  public override void _Ready()
  {
    this._parent = this.GetParent<Node2D>();
    this._animController = new MegaSprite(Variant.op_Implicit((GodotObject) this._parent));
    this._animController.ConnectAnimationEvent(Callable.From<GodotObject, GodotObject, GodotObject, GodotObject>(new Action<GodotObject, GodotObject, GodotObject, GodotObject>(this.OnAnimationEvent)));
    this._steam1Particles = ((Node) this._parent).GetNode<GpuParticles2D>(NodePath.op_Implicit("SteamSlot1/steamParticles1"));
    this._steam2Particles = ((Node) this._parent).GetNode<GpuParticles2D>(NodePath.op_Implicit("SteamSlot2/steamParticles2"));
    this._steam3Particles = ((Node) this._parent).GetNode<GpuParticles2D>(NodePath.op_Implicit("SteamSlot3/steamParticles3"));
    this._steam4Particles = ((Node) this._parent).GetNode<GpuParticles2D>(NodePath.op_Implicit("SteamSlot4/steamParticles4"));
    this._steam5Particles = ((Node) this._parent).GetNode<GpuParticles2D>(NodePath.op_Implicit("SteamSlot5/steamParticles5"));
    this._steam6Particles = ((Node) this._parent).GetNode<GpuParticles2D>(NodePath.op_Implicit("SteamSlot6/steamParticles6"));
    this._steamLeakParticles1 = ((Node) this._parent).GetNode<GpuParticles2D>(NodePath.op_Implicit("SteamLeakSlot1/steamLeakParticles1"));
    this._steamLeakParticles2 = ((Node) this._parent).GetNode<GpuParticles2D>(NodePath.op_Implicit("SteamLeakSlot2/steamLeakParticles2"));
    this._steamLeakParticles3 = ((Node) this._parent).GetNode<GpuParticles2D>(NodePath.op_Implicit("SteamLeakSlot3/steamLeakParticles3"));
    this._mistParticles = ((Node) this._parent).GetNode<GpuParticles2D>(NodePath.op_Implicit("MistSlot/MistParticles"));
    this._dropletParticles = ((Node) this._parent).GetNode<GpuParticles2D>(NodePath.op_Implicit("MistSlot/Droplets"));
    this._mouthParticles = ((Node) this._parent).GetNode<GpuParticles2D>(NodePath.op_Implicit("MouthDropletsSlot/MouthDroplets"));
    this._leakProcMat1 = (ParticleProcessMaterial) this._steamLeakParticles1.ProcessMaterial;
    this._leakProcMat2 = (ParticleProcessMaterial) this._steamLeakParticles2.ProcessMaterial;
    this._leakProcMat3 = (ParticleProcessMaterial) this._steamLeakParticles3.ProcessMaterial;
    this._steam1Particles.Emitting = false;
    this._steam2Particles.Emitting = false;
    this._steam3Particles.Emitting = false;
    this._steam4Particles.Emitting = false;
    this._steam5Particles.Emitting = false;
    this._steam6Particles.Emitting = false;
    this._steamLeakParticles1.Emitting = false;
    this._steamLeakParticles2.Emitting = false;
    this._steamLeakParticles3.Emitting = false;
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
      case 7:
        if (!(eventName == "explode"))
          break;
        this.Explode();
        break;
      case 8:
        switch (eventName[7])
        {
          case '1':
            if (!(eventName == "buildup1"))
              return;
            this.Buildup1();
            return;
          case '2':
            if (!(eventName == "buildup2"))
              return;
            this.Buildup2();
            return;
          case '3':
            if (!(eventName == "buildup3"))
              return;
            this.Buildup3();
            return;
          default:
            return;
        }
      case 11:
        switch (eventName[6])
        {
          case '1':
            if (!(eventName == "steam_1_end"))
              return;
            this.EndSteam1();
            return;
          case '2':
            if (!(eventName == "steam_2_end"))
              return;
            this.EndSteam2();
            return;
          case '3':
            if (!(eventName == "steam_3_end"))
              return;
            this.EndSteam3();
            return;
          case '4':
            return;
          case '5':
            if (!(eventName == "steam_5_end"))
              return;
            this.EndSteam5();
            return;
          default:
            return;
        }
      case 13:
        switch (eventName[6])
        {
          case '1':
            if (!(eventName == "steam_1_start"))
              return;
            this.StartSteam1();
            return;
          case '2':
            if (!(eventName == "steam_2_start"))
              return;
            this.StartSteam2();
            return;
          case '3':
            if (!(eventName == "steam_3_start"))
              return;
            this.StartSteam3();
            return;
          case '4':
            return;
          case '5':
            if (!(eventName == "steam_5_start"))
              return;
            this.StartSteam5();
            return;
          case 'a':
            if (!(eventName == "waterfall_end"))
              return;
            this.EndWaterfall();
            return;
          default:
            return;
        }
      case 15:
        if (!(eventName == "waterfall_start"))
          break;
        this.StartWaterfall();
        break;
      case 17:
        if (!(eventName == "clear_death_steam"))
          break;
        this.ClearDeathSteam();
        break;
    }
  }

  private void StartSteam1() => this.EmitGracefully(this._steam1Particles);

  private void EndSteam1() => this._steam1Particles.Emitting = false;

  private void StartSteam2() => this.EmitGracefully(this._steam2Particles);

  private void EndSteam2() => this._steam2Particles.Emitting = false;

  private void StartSteam3()
  {
    this.EmitGracefully(this._steam3Particles);
    this.EmitGracefully(this._steam4Particles);
  }

  private void EndSteam3()
  {
    this._steam3Particles.Emitting = false;
    this._steam4Particles.Emitting = false;
  }

  private void StartSteam5()
  {
    this.EmitGracefully(this._steam5Particles);
    this.EmitGracefully(this._steam6Particles);
  }

  private void EndSteam5()
  {
    this._steam5Particles.Emitting = false;
    this._steam6Particles.Emitting = false;
  }

  private void StartWaterfall()
  {
    this._mouthParticles.Emitting = true;
    this._dropletParticles.Emitting = true;
    this._mistParticles.Emitting = true;
    this._isDead = false;
  }

  private void EndWaterfall()
  {
    this._mouthParticles.Emitting = false;
    this._dropletParticles.Emitting = false;
    this._mistParticles.Emitting = false;
  }

  private void Explode()
  {
    ((CanvasItem) this._steam1Particles).Visible = false;
    ((CanvasItem) this._steam2Particles).Visible = false;
    ((CanvasItem) this._steam3Particles).Visible = false;
    ((CanvasItem) this._steam4Particles).Visible = false;
    ((CanvasItem) this._steam5Particles).Visible = false;
    ((CanvasItem) this._steam6Particles).Visible = false;
    ((CanvasItem) this._steamLeakParticles1).Visible = false;
    ((CanvasItem) this._steamLeakParticles2).Visible = false;
    ((CanvasItem) this._steamLeakParticles3).Visible = false;
    this._isDead = true;
  }

  private void Buildup1()
  {
    if (this._isDead)
      return;
    this.EmitGracefully(this._steamLeakParticles1);
    this.EmitGracefully(this._steamLeakParticles2);
    this.EmitGracefully(this._steamLeakParticles3);
    this._leakProcMat1.ScaleMin = this._leakProcMat2.ScaleMin = this._leakProcMat3.ScaleMin = 0.2f;
    this._leakProcMat1.ScaleMax = this._leakProcMat2.ScaleMax = this._leakProcMat3.ScaleMax = 0.4f;
    this._steamLeakParticles1.Amount = this._steamLeakParticles2.Amount = this._steamLeakParticles3.Amount = 10;
    this._steamLeakParticles1.Lifetime = this._steamLeakParticles2.Lifetime = this._steamLeakParticles3.Lifetime = 0.37000000476837158;
  }

  private void Buildup2()
  {
    if (this._isDead)
      return;
    this.EmitGracefully(this._steamLeakParticles1);
    this.EmitGracefully(this._steamLeakParticles2);
    this.EmitGracefully(this._steamLeakParticles3);
    this._leakProcMat1.ScaleMin = this._leakProcMat2.ScaleMin = this._leakProcMat3.ScaleMin = 0.3f;
    this._leakProcMat1.ScaleMax = this._leakProcMat2.ScaleMax = this._leakProcMat3.ScaleMax = 0.6f;
    this._steamLeakParticles1.Amount = this._steamLeakParticles2.Amount = this._steamLeakParticles3.Amount = 20;
    this._steamLeakParticles1.Lifetime = this._steamLeakParticles2.Lifetime = this._steamLeakParticles3.Lifetime = 0.75;
  }

  private void Buildup3()
  {
    if (this._isDead)
      return;
    this.EmitGracefully(this._steamLeakParticles1);
    this.EmitGracefully(this._steamLeakParticles2);
    this.EmitGracefully(this._steamLeakParticles3);
    this._leakProcMat1.ScaleMin = this._leakProcMat2.ScaleMin = this._leakProcMat3.ScaleMin = 0.4f;
    this._leakProcMat1.ScaleMax = this._leakProcMat2.ScaleMax = this._leakProcMat3.ScaleMax = 0.8f;
    this._steamLeakParticles1.Amount = this._steamLeakParticles2.Amount = this._steamLeakParticles3.Amount = 28;
    this._steamLeakParticles1.Lifetime = this._steamLeakParticles2.Lifetime = this._steamLeakParticles3.Lifetime = 1.2;
  }

  private void ClearDeathSteam()
  {
    this._steam3Particles.Emitting = false;
    this._steam4Particles.Emitting = false;
    this._steam5Particles.Emitting = false;
    this._steam6Particles.Emitting = false;
    ((CanvasItem) this._steam3Particles).Visible = false;
    ((CanvasItem) this._steam4Particles).Visible = false;
    ((CanvasItem) this._steam5Particles).Visible = false;
    ((CanvasItem) this._steam6Particles).Visible = false;
    this._isDead = false;
  }

  private void EmitGracefully(GpuParticles2D emitter)
  {
    if (!((CanvasItem) emitter).Visible)
    {
      ((CanvasItem) emitter).Visible = true;
      emitter.Restart();
    }
    else
      emitter.Emitting = true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(18)
    {
      new MethodInfo(NWaterfallGiantVfx.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NWaterfallGiantVfx.MethodName.OnAnimationEvent, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("_"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Object"), false),
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("__"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Object"), false),
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("___"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Object"), false),
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("spineEvent"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Object"), false)
      }, (List<Variant>) null),
      new MethodInfo(NWaterfallGiantVfx.MethodName.StartSteam1, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NWaterfallGiantVfx.MethodName.EndSteam1, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NWaterfallGiantVfx.MethodName.StartSteam2, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NWaterfallGiantVfx.MethodName.EndSteam2, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NWaterfallGiantVfx.MethodName.StartSteam3, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NWaterfallGiantVfx.MethodName.EndSteam3, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NWaterfallGiantVfx.MethodName.StartSteam5, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NWaterfallGiantVfx.MethodName.EndSteam5, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NWaterfallGiantVfx.MethodName.StartWaterfall, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NWaterfallGiantVfx.MethodName.EndWaterfall, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NWaterfallGiantVfx.MethodName.Explode, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NWaterfallGiantVfx.MethodName.Buildup1, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NWaterfallGiantVfx.MethodName.Buildup2, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NWaterfallGiantVfx.MethodName.Buildup3, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NWaterfallGiantVfx.MethodName.ClearDeathSteam, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NWaterfallGiantVfx.MethodName.EmitGracefully, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("emitter"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("GPUParticles2D"), false)
      }, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NWaterfallGiantVfx.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      base._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NWaterfallGiantVfx.MethodName.OnAnimationEvent) && ((NativeVariantPtrArgs) ref args).Count == 4)
    {
      this.OnAnimationEvent(VariantUtils.ConvertTo<GodotObject>(ref ((NativeVariantPtrArgs) ref args)[0]), VariantUtils.ConvertTo<GodotObject>(ref ((NativeVariantPtrArgs) ref args)[1]), VariantUtils.ConvertTo<GodotObject>(ref ((NativeVariantPtrArgs) ref args)[2]), VariantUtils.ConvertTo<GodotObject>(ref ((NativeVariantPtrArgs) ref args)[3]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NWaterfallGiantVfx.MethodName.StartSteam1) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.StartSteam1();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NWaterfallGiantVfx.MethodName.EndSteam1) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.EndSteam1();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NWaterfallGiantVfx.MethodName.StartSteam2) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.StartSteam2();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NWaterfallGiantVfx.MethodName.EndSteam2) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.EndSteam2();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NWaterfallGiantVfx.MethodName.StartSteam3) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.StartSteam3();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NWaterfallGiantVfx.MethodName.EndSteam3) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.EndSteam3();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NWaterfallGiantVfx.MethodName.StartSteam5) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.StartSteam5();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NWaterfallGiantVfx.MethodName.EndSteam5) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.EndSteam5();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NWaterfallGiantVfx.MethodName.StartWaterfall) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.StartWaterfall();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NWaterfallGiantVfx.MethodName.EndWaterfall) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.EndWaterfall();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NWaterfallGiantVfx.MethodName.Explode) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.Explode();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NWaterfallGiantVfx.MethodName.Buildup1) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.Buildup1();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NWaterfallGiantVfx.MethodName.Buildup2) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.Buildup2();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NWaterfallGiantVfx.MethodName.Buildup3) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.Buildup3();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NWaterfallGiantVfx.MethodName.ClearDeathSteam) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.ClearDeathSteam();
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NWaterfallGiantVfx.MethodName.EmitGracefully) || ((NativeVariantPtrArgs) ref args).Count != 1)
      return base.InvokeGodotClassMethod(ref method, args, ref ret);
    this.EmitGracefully(VariantUtils.ConvertTo<GpuParticles2D>(ref ((NativeVariantPtrArgs) ref args)[0]));
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NWaterfallGiantVfx.MethodName._Ready) || StringName.op_Equality(ref method, NWaterfallGiantVfx.MethodName.OnAnimationEvent) || StringName.op_Equality(ref method, NWaterfallGiantVfx.MethodName.StartSteam1) || StringName.op_Equality(ref method, NWaterfallGiantVfx.MethodName.EndSteam1) || StringName.op_Equality(ref method, NWaterfallGiantVfx.MethodName.StartSteam2) || StringName.op_Equality(ref method, NWaterfallGiantVfx.MethodName.EndSteam2) || StringName.op_Equality(ref method, NWaterfallGiantVfx.MethodName.StartSteam3) || StringName.op_Equality(ref method, NWaterfallGiantVfx.MethodName.EndSteam3) || StringName.op_Equality(ref method, NWaterfallGiantVfx.MethodName.StartSteam5) || StringName.op_Equality(ref method, NWaterfallGiantVfx.MethodName.EndSteam5) || StringName.op_Equality(ref method, NWaterfallGiantVfx.MethodName.StartWaterfall) || StringName.op_Equality(ref method, NWaterfallGiantVfx.MethodName.EndWaterfall) || StringName.op_Equality(ref method, NWaterfallGiantVfx.MethodName.Explode) || StringName.op_Equality(ref method, NWaterfallGiantVfx.MethodName.Buildup1) || StringName.op_Equality(ref method, NWaterfallGiantVfx.MethodName.Buildup2) || StringName.op_Equality(ref method, NWaterfallGiantVfx.MethodName.Buildup3) || StringName.op_Equality(ref method, NWaterfallGiantVfx.MethodName.ClearDeathSteam) || StringName.op_Equality(ref method, NWaterfallGiantVfx.MethodName.EmitGracefully) || base.HasGodotClassMethod(ref method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NWaterfallGiantVfx.PropertyName._steam1Particles))
    {
      this._steam1Particles = VariantUtils.ConvertTo<GpuParticles2D>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NWaterfallGiantVfx.PropertyName._steam2Particles))
    {
      this._steam2Particles = VariantUtils.ConvertTo<GpuParticles2D>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NWaterfallGiantVfx.PropertyName._steam3Particles))
    {
      this._steam3Particles = VariantUtils.ConvertTo<GpuParticles2D>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NWaterfallGiantVfx.PropertyName._steam4Particles))
    {
      this._steam4Particles = VariantUtils.ConvertTo<GpuParticles2D>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NWaterfallGiantVfx.PropertyName._steam5Particles))
    {
      this._steam5Particles = VariantUtils.ConvertTo<GpuParticles2D>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NWaterfallGiantVfx.PropertyName._steam6Particles))
    {
      this._steam6Particles = VariantUtils.ConvertTo<GpuParticles2D>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NWaterfallGiantVfx.PropertyName._steamLeakParticles1))
    {
      this._steamLeakParticles1 = VariantUtils.ConvertTo<GpuParticles2D>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NWaterfallGiantVfx.PropertyName._steamLeakParticles2))
    {
      this._steamLeakParticles2 = VariantUtils.ConvertTo<GpuParticles2D>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NWaterfallGiantVfx.PropertyName._steamLeakParticles3))
    {
      this._steamLeakParticles3 = VariantUtils.ConvertTo<GpuParticles2D>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NWaterfallGiantVfx.PropertyName._mistParticles))
    {
      this._mistParticles = VariantUtils.ConvertTo<GpuParticles2D>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NWaterfallGiantVfx.PropertyName._mouthParticles))
    {
      this._mouthParticles = VariantUtils.ConvertTo<GpuParticles2D>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NWaterfallGiantVfx.PropertyName._dropletParticles))
    {
      this._dropletParticles = VariantUtils.ConvertTo<GpuParticles2D>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NWaterfallGiantVfx.PropertyName._leakProcMat1))
    {
      this._leakProcMat1 = VariantUtils.ConvertTo<ParticleProcessMaterial>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NWaterfallGiantVfx.PropertyName._leakProcMat2))
    {
      this._leakProcMat2 = VariantUtils.ConvertTo<ParticleProcessMaterial>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NWaterfallGiantVfx.PropertyName._leakProcMat3))
    {
      this._leakProcMat3 = VariantUtils.ConvertTo<ParticleProcessMaterial>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NWaterfallGiantVfx.PropertyName._isDead))
    {
      this._isDead = VariantUtils.ConvertTo<bool>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NWaterfallGiantVfx.PropertyName._parent))
      return ((GodotObject) this).SetGodotClassPropertyValue(ref name, ref value);
    this._parent = VariantUtils.ConvertTo<Node2D>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NWaterfallGiantVfx.PropertyName._steam1Particles))
    {
      value = VariantUtils.CreateFrom<GpuParticles2D>(ref this._steam1Particles);
      return true;
    }
    if (StringName.op_Equality(ref name, NWaterfallGiantVfx.PropertyName._steam2Particles))
    {
      value = VariantUtils.CreateFrom<GpuParticles2D>(ref this._steam2Particles);
      return true;
    }
    if (StringName.op_Equality(ref name, NWaterfallGiantVfx.PropertyName._steam3Particles))
    {
      value = VariantUtils.CreateFrom<GpuParticles2D>(ref this._steam3Particles);
      return true;
    }
    if (StringName.op_Equality(ref name, NWaterfallGiantVfx.PropertyName._steam4Particles))
    {
      value = VariantUtils.CreateFrom<GpuParticles2D>(ref this._steam4Particles);
      return true;
    }
    if (StringName.op_Equality(ref name, NWaterfallGiantVfx.PropertyName._steam5Particles))
    {
      value = VariantUtils.CreateFrom<GpuParticles2D>(ref this._steam5Particles);
      return true;
    }
    if (StringName.op_Equality(ref name, NWaterfallGiantVfx.PropertyName._steam6Particles))
    {
      value = VariantUtils.CreateFrom<GpuParticles2D>(ref this._steam6Particles);
      return true;
    }
    if (StringName.op_Equality(ref name, NWaterfallGiantVfx.PropertyName._steamLeakParticles1))
    {
      value = VariantUtils.CreateFrom<GpuParticles2D>(ref this._steamLeakParticles1);
      return true;
    }
    if (StringName.op_Equality(ref name, NWaterfallGiantVfx.PropertyName._steamLeakParticles2))
    {
      value = VariantUtils.CreateFrom<GpuParticles2D>(ref this._steamLeakParticles2);
      return true;
    }
    if (StringName.op_Equality(ref name, NWaterfallGiantVfx.PropertyName._steamLeakParticles3))
    {
      value = VariantUtils.CreateFrom<GpuParticles2D>(ref this._steamLeakParticles3);
      return true;
    }
    if (StringName.op_Equality(ref name, NWaterfallGiantVfx.PropertyName._mistParticles))
    {
      value = VariantUtils.CreateFrom<GpuParticles2D>(ref this._mistParticles);
      return true;
    }
    if (StringName.op_Equality(ref name, NWaterfallGiantVfx.PropertyName._mouthParticles))
    {
      value = VariantUtils.CreateFrom<GpuParticles2D>(ref this._mouthParticles);
      return true;
    }
    if (StringName.op_Equality(ref name, NWaterfallGiantVfx.PropertyName._dropletParticles))
    {
      value = VariantUtils.CreateFrom<GpuParticles2D>(ref this._dropletParticles);
      return true;
    }
    if (StringName.op_Equality(ref name, NWaterfallGiantVfx.PropertyName._leakProcMat1))
    {
      value = VariantUtils.CreateFrom<ParticleProcessMaterial>(ref this._leakProcMat1);
      return true;
    }
    if (StringName.op_Equality(ref name, NWaterfallGiantVfx.PropertyName._leakProcMat2))
    {
      value = VariantUtils.CreateFrom<ParticleProcessMaterial>(ref this._leakProcMat2);
      return true;
    }
    if (StringName.op_Equality(ref name, NWaterfallGiantVfx.PropertyName._leakProcMat3))
    {
      value = VariantUtils.CreateFrom<ParticleProcessMaterial>(ref this._leakProcMat3);
      return true;
    }
    if (StringName.op_Equality(ref name, NWaterfallGiantVfx.PropertyName._isDead))
    {
      value = VariantUtils.CreateFrom<bool>(ref this._isDead);
      return true;
    }
    if (!StringName.op_Equality(ref name, NWaterfallGiantVfx.PropertyName._parent))
      return ((GodotObject) this).GetGodotClassPropertyValue(ref name, ref value);
    value = VariantUtils.CreateFrom<Node2D>(ref this._parent);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NWaterfallGiantVfx.PropertyName._steam1Particles, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NWaterfallGiantVfx.PropertyName._steam2Particles, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NWaterfallGiantVfx.PropertyName._steam3Particles, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NWaterfallGiantVfx.PropertyName._steam4Particles, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NWaterfallGiantVfx.PropertyName._steam5Particles, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NWaterfallGiantVfx.PropertyName._steam6Particles, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NWaterfallGiantVfx.PropertyName._steamLeakParticles1, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NWaterfallGiantVfx.PropertyName._steamLeakParticles2, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NWaterfallGiantVfx.PropertyName._steamLeakParticles3, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NWaterfallGiantVfx.PropertyName._mistParticles, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NWaterfallGiantVfx.PropertyName._mouthParticles, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NWaterfallGiantVfx.PropertyName._dropletParticles, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NWaterfallGiantVfx.PropertyName._leakProcMat1, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NWaterfallGiantVfx.PropertyName._leakProcMat2, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NWaterfallGiantVfx.PropertyName._leakProcMat3, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 1L, NWaterfallGiantVfx.PropertyName._isDead, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NWaterfallGiantVfx.PropertyName._parent, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).SaveGodotObjectData(info);
    info.AddProperty(NWaterfallGiantVfx.PropertyName._steam1Particles, Variant.From<GpuParticles2D>(ref this._steam1Particles));
    info.AddProperty(NWaterfallGiantVfx.PropertyName._steam2Particles, Variant.From<GpuParticles2D>(ref this._steam2Particles));
    info.AddProperty(NWaterfallGiantVfx.PropertyName._steam3Particles, Variant.From<GpuParticles2D>(ref this._steam3Particles));
    info.AddProperty(NWaterfallGiantVfx.PropertyName._steam4Particles, Variant.From<GpuParticles2D>(ref this._steam4Particles));
    info.AddProperty(NWaterfallGiantVfx.PropertyName._steam5Particles, Variant.From<GpuParticles2D>(ref this._steam5Particles));
    info.AddProperty(NWaterfallGiantVfx.PropertyName._steam6Particles, Variant.From<GpuParticles2D>(ref this._steam6Particles));
    info.AddProperty(NWaterfallGiantVfx.PropertyName._steamLeakParticles1, Variant.From<GpuParticles2D>(ref this._steamLeakParticles1));
    info.AddProperty(NWaterfallGiantVfx.PropertyName._steamLeakParticles2, Variant.From<GpuParticles2D>(ref this._steamLeakParticles2));
    info.AddProperty(NWaterfallGiantVfx.PropertyName._steamLeakParticles3, Variant.From<GpuParticles2D>(ref this._steamLeakParticles3));
    info.AddProperty(NWaterfallGiantVfx.PropertyName._mistParticles, Variant.From<GpuParticles2D>(ref this._mistParticles));
    info.AddProperty(NWaterfallGiantVfx.PropertyName._mouthParticles, Variant.From<GpuParticles2D>(ref this._mouthParticles));
    info.AddProperty(NWaterfallGiantVfx.PropertyName._dropletParticles, Variant.From<GpuParticles2D>(ref this._dropletParticles));
    info.AddProperty(NWaterfallGiantVfx.PropertyName._leakProcMat1, Variant.From<ParticleProcessMaterial>(ref this._leakProcMat1));
    info.AddProperty(NWaterfallGiantVfx.PropertyName._leakProcMat2, Variant.From<ParticleProcessMaterial>(ref this._leakProcMat2));
    info.AddProperty(NWaterfallGiantVfx.PropertyName._leakProcMat3, Variant.From<ParticleProcessMaterial>(ref this._leakProcMat3));
    info.AddProperty(NWaterfallGiantVfx.PropertyName._isDead, Variant.From<bool>(ref this._isDead));
    info.AddProperty(NWaterfallGiantVfx.PropertyName._parent, Variant.From<Node2D>(ref this._parent));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NWaterfallGiantVfx.PropertyName._steam1Particles, ref variant1))
      this._steam1Particles = ((Variant) ref variant1).As<GpuParticles2D>();
    Variant variant2;
    if (info.TryGetProperty(NWaterfallGiantVfx.PropertyName._steam2Particles, ref variant2))
      this._steam2Particles = ((Variant) ref variant2).As<GpuParticles2D>();
    Variant variant3;
    if (info.TryGetProperty(NWaterfallGiantVfx.PropertyName._steam3Particles, ref variant3))
      this._steam3Particles = ((Variant) ref variant3).As<GpuParticles2D>();
    Variant variant4;
    if (info.TryGetProperty(NWaterfallGiantVfx.PropertyName._steam4Particles, ref variant4))
      this._steam4Particles = ((Variant) ref variant4).As<GpuParticles2D>();
    Variant variant5;
    if (info.TryGetProperty(NWaterfallGiantVfx.PropertyName._steam5Particles, ref variant5))
      this._steam5Particles = ((Variant) ref variant5).As<GpuParticles2D>();
    Variant variant6;
    if (info.TryGetProperty(NWaterfallGiantVfx.PropertyName._steam6Particles, ref variant6))
      this._steam6Particles = ((Variant) ref variant6).As<GpuParticles2D>();
    Variant variant7;
    if (info.TryGetProperty(NWaterfallGiantVfx.PropertyName._steamLeakParticles1, ref variant7))
      this._steamLeakParticles1 = ((Variant) ref variant7).As<GpuParticles2D>();
    Variant variant8;
    if (info.TryGetProperty(NWaterfallGiantVfx.PropertyName._steamLeakParticles2, ref variant8))
      this._steamLeakParticles2 = ((Variant) ref variant8).As<GpuParticles2D>();
    Variant variant9;
    if (info.TryGetProperty(NWaterfallGiantVfx.PropertyName._steamLeakParticles3, ref variant9))
      this._steamLeakParticles3 = ((Variant) ref variant9).As<GpuParticles2D>();
    Variant variant10;
    if (info.TryGetProperty(NWaterfallGiantVfx.PropertyName._mistParticles, ref variant10))
      this._mistParticles = ((Variant) ref variant10).As<GpuParticles2D>();
    Variant variant11;
    if (info.TryGetProperty(NWaterfallGiantVfx.PropertyName._mouthParticles, ref variant11))
      this._mouthParticles = ((Variant) ref variant11).As<GpuParticles2D>();
    Variant variant12;
    if (info.TryGetProperty(NWaterfallGiantVfx.PropertyName._dropletParticles, ref variant12))
      this._dropletParticles = ((Variant) ref variant12).As<GpuParticles2D>();
    Variant variant13;
    if (info.TryGetProperty(NWaterfallGiantVfx.PropertyName._leakProcMat1, ref variant13))
      this._leakProcMat1 = ((Variant) ref variant13).As<ParticleProcessMaterial>();
    Variant variant14;
    if (info.TryGetProperty(NWaterfallGiantVfx.PropertyName._leakProcMat2, ref variant14))
      this._leakProcMat2 = ((Variant) ref variant14).As<ParticleProcessMaterial>();
    Variant variant15;
    if (info.TryGetProperty(NWaterfallGiantVfx.PropertyName._leakProcMat3, ref variant15))
      this._leakProcMat3 = ((Variant) ref variant15).As<ParticleProcessMaterial>();
    Variant variant16;
    if (info.TryGetProperty(NWaterfallGiantVfx.PropertyName._isDead, ref variant16))
      this._isDead = ((Variant) ref variant16).As<bool>();
    Variant variant17;
    if (!info.TryGetProperty(NWaterfallGiantVfx.PropertyName._parent, ref variant17))
      return;
    this._parent = ((Variant) ref variant17).As<Node2D>();
  }

  public class MethodName : Node.MethodName
  {
    public static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName OnAnimationEvent = StringName.op_Implicit(nameof (OnAnimationEvent));
    public static readonly StringName StartSteam1 = StringName.op_Implicit(nameof (StartSteam1));
    public static readonly StringName EndSteam1 = StringName.op_Implicit(nameof (EndSteam1));
    public static readonly StringName StartSteam2 = StringName.op_Implicit(nameof (StartSteam2));
    public static readonly StringName EndSteam2 = StringName.op_Implicit(nameof (EndSteam2));
    public static readonly StringName StartSteam3 = StringName.op_Implicit(nameof (StartSteam3));
    public static readonly StringName EndSteam3 = StringName.op_Implicit(nameof (EndSteam3));
    public static readonly StringName StartSteam5 = StringName.op_Implicit(nameof (StartSteam5));
    public static readonly StringName EndSteam5 = StringName.op_Implicit(nameof (EndSteam5));
    public static readonly StringName StartWaterfall = StringName.op_Implicit(nameof (StartWaterfall));
    public static readonly StringName EndWaterfall = StringName.op_Implicit(nameof (EndWaterfall));
    public static readonly StringName Explode = StringName.op_Implicit(nameof (Explode));
    public static readonly StringName Buildup1 = StringName.op_Implicit(nameof (Buildup1));
    public static readonly StringName Buildup2 = StringName.op_Implicit(nameof (Buildup2));
    public static readonly StringName Buildup3 = StringName.op_Implicit(nameof (Buildup3));
    public static readonly StringName ClearDeathSteam = StringName.op_Implicit(nameof (ClearDeathSteam));
    public static readonly StringName EmitGracefully = StringName.op_Implicit(nameof (EmitGracefully));
  }

  public class PropertyName : Node.PropertyName
  {
    public static readonly StringName _steam1Particles = StringName.op_Implicit(nameof (_steam1Particles));
    public static readonly StringName _steam2Particles = StringName.op_Implicit(nameof (_steam2Particles));
    public static readonly StringName _steam3Particles = StringName.op_Implicit(nameof (_steam3Particles));
    public static readonly StringName _steam4Particles = StringName.op_Implicit(nameof (_steam4Particles));
    public static readonly StringName _steam5Particles = StringName.op_Implicit(nameof (_steam5Particles));
    public static readonly StringName _steam6Particles = StringName.op_Implicit(nameof (_steam6Particles));
    public static readonly StringName _steamLeakParticles1 = StringName.op_Implicit(nameof (_steamLeakParticles1));
    public static readonly StringName _steamLeakParticles2 = StringName.op_Implicit(nameof (_steamLeakParticles2));
    public static readonly StringName _steamLeakParticles3 = StringName.op_Implicit(nameof (_steamLeakParticles3));
    public static readonly StringName _mistParticles = StringName.op_Implicit(nameof (_mistParticles));
    public static readonly StringName _mouthParticles = StringName.op_Implicit(nameof (_mouthParticles));
    public static readonly StringName _dropletParticles = StringName.op_Implicit(nameof (_dropletParticles));
    public static readonly StringName _leakProcMat1 = StringName.op_Implicit(nameof (_leakProcMat1));
    public static readonly StringName _leakProcMat2 = StringName.op_Implicit(nameof (_leakProcMat2));
    public static readonly StringName _leakProcMat3 = StringName.op_Implicit(nameof (_leakProcMat3));
    public static readonly StringName _isDead = StringName.op_Implicit(nameof (_isDead));
    public static readonly StringName _parent = StringName.op_Implicit(nameof (_parent));
  }

  public class SignalName : Node.SignalName
  {
  }
}
