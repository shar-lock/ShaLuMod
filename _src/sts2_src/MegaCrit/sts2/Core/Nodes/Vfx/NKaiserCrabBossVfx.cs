// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Vfx.NKaiserCrabBossVfx
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Bindings.MegaSpine;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using System;
using System.Collections.Generic;
using System.ComponentModel;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Vfx;

[ScriptPath("res://src/Core/Nodes/Vfx/NKaiserCrabBossVfx.cs")]
public class NKaiserCrabBossVfx : Node
{
  private MegaSprite _megaSprite;
  private GpuParticles2D _regenSplatParticles;
  private GpuParticles2D _plowChunkParticles;
  private GpuParticles2D _steamParticles1;
  private GpuParticles2D _steamParticles2;
  private GpuParticles2D _steamParticles3;
  private GpuParticles2D _smokeParticles;
  private GpuParticles2D _sparkParticles;
  private GpuParticles2D _spittleParticles;
  private Node2D _leftArmExplosionPosition;
  private Node2D _parent;

  public override void _Ready()
  {
    this._parent = this.GetParent<Node2D>();
    this._regenSplatParticles = ((Node) this._parent).GetNode<GpuParticles2D>(NodePath.op_Implicit("RegenSplatSlot/RegenSplatParticles"));
    this._plowChunkParticles = ((Node) this._parent).GetNode<GpuParticles2D>(NodePath.op_Implicit("PlowChunkSlot/PlowChunkParticles"));
    this._steamParticles1 = ((Node) this._parent).GetNode<GpuParticles2D>(NodePath.op_Implicit("RocketSlot/SteamParticles1"));
    this._steamParticles2 = ((Node) this._parent).GetNode<GpuParticles2D>(NodePath.op_Implicit("RocketSlot/SteamParticles2"));
    this._steamParticles3 = ((Node) this._parent).GetNode<GpuParticles2D>(NodePath.op_Implicit("RocketSlot/SteamParticles3"));
    this._sparkParticles = ((Node) this._parent).GetNode<GpuParticles2D>(NodePath.op_Implicit("RocketSlot/SparkParticles"));
    this._smokeParticles = ((Node) this._parent).GetNode<GpuParticles2D>(NodePath.op_Implicit("RocketSlot/SmokeParticles"));
    this._leftArmExplosionPosition = ((Node) this._parent).GetNode<Node2D>(NodePath.op_Implicit("%LeftArmExplosionPosition"));
    this._spittleParticles = ((Node) this._parent).GetNode<GpuParticles2D>(NodePath.op_Implicit("SpittleSlot/SpittleParticles"));
    this._megaSprite = new MegaSprite(Variant.op_Implicit((GodotObject) this.GetParent<Node2D>()));
    this._megaSprite.ConnectAnimationEvent(Callable.From<GodotObject, GodotObject, GodotObject, GodotObject>(new Action<GodotObject, GodotObject, GodotObject, GodotObject>(this.OnAnimationEvent)));
    this._megaSprite.ConnectAnimationStarted(Callable.From<GodotObject, GodotObject, GodotObject>(new Action<GodotObject, GodotObject, GodotObject>(this.OnAnimationStart)));
    this._regenSplatParticles.Emitting = false;
    this._plowChunkParticles.Emitting = false;
    this._steamParticles1.Emitting = false;
    this._steamParticles2.Emitting = false;
    this._steamParticles3.Emitting = false;
    this._spittleParticles.Emitting = false;
    this._sparkParticles.Emitting = false;
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
      case 14:
        switch (eventName[0])
        {
          case 'c':
            if (!(eventName == "claw_explode_l"))
              return;
            this.OnClawLExplode();
            return;
          case 'd':
            if (!(eventName == "death_spit_end"))
              return;
            this.OnDeathSpitEnd();
            return;
          default:
            return;
        }
      case 15:
        if (!(eventName == "plow_chunks_end"))
          break;
        this.OnPlowChunksEnd();
        break;
      case 16 /*0x10*/:
        switch (eventName[0])
        {
          case 'c':
            if (!(eventName == "charge_steam_end"))
              return;
            this.OnChargeSteamEnd();
            return;
          case 'd':
            if (!(eventName == "death_spit_start"))
              return;
            this.OnDeathSpitStart();
            return;
          case 'r':
            if (!(eventName == "regen_splats_end"))
              return;
            this.OnRegenSplatsEnd();
            return;
          default:
            return;
        }
      case 17:
        switch (eventName[0])
        {
          case 'p':
            if (!(eventName == "plow_chunks_start"))
              return;
            this.OnPlowChunksStart();
            return;
          case 'r':
            if (!(eventName == "rocket_thrust_end"))
              return;
            this.OnRocketThrustEnd();
            return;
          default:
            return;
        }
      case 18:
        switch (eventName[0])
        {
          case 'c':
            if (!(eventName == "charge_steam_start"))
              return;
            this.OnChargeSteamStart();
            return;
          case 'r':
            if (!(eventName == "regen_splats_start"))
              return;
            this.OnRegenSplatsStart();
            return;
          default:
            return;
        }
      case 19:
        if (!(eventName == "rocket_thrust_start"))
          break;
        this.OnRocketThrustStart();
        break;
    }
  }

  private void OnAnimationStart(
    GodotObject spineSprite,
    GodotObject animationState,
    GodotObject trackEntry)
  {
    string currentAnimationName = new MegaAnimationState(Variant.op_Implicit(animationState)).GetCurrentAnimationName(2);
    if (currentAnimationName != "right/charged_loop" && currentAnimationName != "right/charge_up" && currentAnimationName != "right/hurt_charged")
      this.OnChargeSteamEnd();
    if (!(currentAnimationName != "right/attack_heavy"))
      return;
    this.OnRocketThrustEnd();
  }

  private void OnChargeSteamStart()
  {
    this._steamParticles1.Restart();
    this._steamParticles2.Restart();
    this._steamParticles3.Restart();
  }

  private void OnChargeSteamEnd()
  {
    this._steamParticles1.Emitting = false;
    this._steamParticles2.Emitting = false;
    this._steamParticles3.Emitting = false;
  }

  private void OnDeathSpitStart() => this._spittleParticles.Restart();

  private void OnDeathSpitEnd() => this._spittleParticles.Emitting = false;

  private void OnLeftEmbersStart()
  {
  }

  private void OnPlowChunksStart() => this._plowChunkParticles.Restart();

  private void OnPlowChunksEnd() => this._plowChunkParticles.Emitting = false;

  private void OnRegenSplatsStart() => this._regenSplatParticles.Restart();

  private void OnRegenSplatsEnd() => this._regenSplatParticles.Emitting = false;

  private void OnRocketThrustStart()
  {
    this._smokeParticles.Restart();
    this._sparkParticles.Restart();
  }

  private void OnRocketThrustEnd()
  {
    this._smokeParticles.Emitting = false;
    this._sparkParticles.Emitting = false;
  }

  private void OnClawLExplode()
  {
    VfxCmd.PlayVfx(this._leftArmExplosionPosition.GlobalPosition, "vfx/monsters/kaiser_crab_boss_explosion", NCombatRoom.Instance?.CombatVfxContainer);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(15)
    {
      new MethodInfo(NKaiserCrabBossVfx.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NKaiserCrabBossVfx.MethodName.OnAnimationEvent, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("_"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Object"), false),
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("__"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Object"), false),
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("___"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Object"), false),
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("spineEvent"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Object"), false)
      }, (List<Variant>) null),
      new MethodInfo(NKaiserCrabBossVfx.MethodName.OnAnimationStart, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("spineSprite"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Object"), false),
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("animationState"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Object"), false),
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("trackEntry"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Object"), false)
      }, (List<Variant>) null),
      new MethodInfo(NKaiserCrabBossVfx.MethodName.OnChargeSteamStart, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NKaiserCrabBossVfx.MethodName.OnChargeSteamEnd, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NKaiserCrabBossVfx.MethodName.OnDeathSpitStart, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NKaiserCrabBossVfx.MethodName.OnDeathSpitEnd, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NKaiserCrabBossVfx.MethodName.OnLeftEmbersStart, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NKaiserCrabBossVfx.MethodName.OnPlowChunksStart, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NKaiserCrabBossVfx.MethodName.OnPlowChunksEnd, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NKaiserCrabBossVfx.MethodName.OnRegenSplatsStart, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NKaiserCrabBossVfx.MethodName.OnRegenSplatsEnd, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NKaiserCrabBossVfx.MethodName.OnRocketThrustStart, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NKaiserCrabBossVfx.MethodName.OnRocketThrustEnd, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NKaiserCrabBossVfx.MethodName.OnClawLExplode, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NKaiserCrabBossVfx.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      base._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NKaiserCrabBossVfx.MethodName.OnAnimationEvent) && ((NativeVariantPtrArgs) ref args).Count == 4)
    {
      this.OnAnimationEvent(VariantUtils.ConvertTo<GodotObject>(ref ((NativeVariantPtrArgs) ref args)[0]), VariantUtils.ConvertTo<GodotObject>(ref ((NativeVariantPtrArgs) ref args)[1]), VariantUtils.ConvertTo<GodotObject>(ref ((NativeVariantPtrArgs) ref args)[2]), VariantUtils.ConvertTo<GodotObject>(ref ((NativeVariantPtrArgs) ref args)[3]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NKaiserCrabBossVfx.MethodName.OnAnimationStart) && ((NativeVariantPtrArgs) ref args).Count == 3)
    {
      this.OnAnimationStart(VariantUtils.ConvertTo<GodotObject>(ref ((NativeVariantPtrArgs) ref args)[0]), VariantUtils.ConvertTo<GodotObject>(ref ((NativeVariantPtrArgs) ref args)[1]), VariantUtils.ConvertTo<GodotObject>(ref ((NativeVariantPtrArgs) ref args)[2]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NKaiserCrabBossVfx.MethodName.OnChargeSteamStart) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnChargeSteamStart();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NKaiserCrabBossVfx.MethodName.OnChargeSteamEnd) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnChargeSteamEnd();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NKaiserCrabBossVfx.MethodName.OnDeathSpitStart) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnDeathSpitStart();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NKaiserCrabBossVfx.MethodName.OnDeathSpitEnd) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnDeathSpitEnd();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NKaiserCrabBossVfx.MethodName.OnLeftEmbersStart) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnLeftEmbersStart();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NKaiserCrabBossVfx.MethodName.OnPlowChunksStart) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnPlowChunksStart();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NKaiserCrabBossVfx.MethodName.OnPlowChunksEnd) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnPlowChunksEnd();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NKaiserCrabBossVfx.MethodName.OnRegenSplatsStart) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnRegenSplatsStart();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NKaiserCrabBossVfx.MethodName.OnRegenSplatsEnd) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnRegenSplatsEnd();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NKaiserCrabBossVfx.MethodName.OnRocketThrustStart) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnRocketThrustStart();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NKaiserCrabBossVfx.MethodName.OnRocketThrustEnd) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnRocketThrustEnd();
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NKaiserCrabBossVfx.MethodName.OnClawLExplode) || ((NativeVariantPtrArgs) ref args).Count != 0)
      return base.InvokeGodotClassMethod(ref method, args, ref ret);
    this.OnClawLExplode();
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NKaiserCrabBossVfx.MethodName._Ready) || StringName.op_Equality(ref method, NKaiserCrabBossVfx.MethodName.OnAnimationEvent) || StringName.op_Equality(ref method, NKaiserCrabBossVfx.MethodName.OnAnimationStart) || StringName.op_Equality(ref method, NKaiserCrabBossVfx.MethodName.OnChargeSteamStart) || StringName.op_Equality(ref method, NKaiserCrabBossVfx.MethodName.OnChargeSteamEnd) || StringName.op_Equality(ref method, NKaiserCrabBossVfx.MethodName.OnDeathSpitStart) || StringName.op_Equality(ref method, NKaiserCrabBossVfx.MethodName.OnDeathSpitEnd) || StringName.op_Equality(ref method, NKaiserCrabBossVfx.MethodName.OnLeftEmbersStart) || StringName.op_Equality(ref method, NKaiserCrabBossVfx.MethodName.OnPlowChunksStart) || StringName.op_Equality(ref method, NKaiserCrabBossVfx.MethodName.OnPlowChunksEnd) || StringName.op_Equality(ref method, NKaiserCrabBossVfx.MethodName.OnRegenSplatsStart) || StringName.op_Equality(ref method, NKaiserCrabBossVfx.MethodName.OnRegenSplatsEnd) || StringName.op_Equality(ref method, NKaiserCrabBossVfx.MethodName.OnRocketThrustStart) || StringName.op_Equality(ref method, NKaiserCrabBossVfx.MethodName.OnRocketThrustEnd) || StringName.op_Equality(ref method, NKaiserCrabBossVfx.MethodName.OnClawLExplode) || base.HasGodotClassMethod(ref method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NKaiserCrabBossVfx.PropertyName._regenSplatParticles))
    {
      this._regenSplatParticles = VariantUtils.ConvertTo<GpuParticles2D>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NKaiserCrabBossVfx.PropertyName._plowChunkParticles))
    {
      this._plowChunkParticles = VariantUtils.ConvertTo<GpuParticles2D>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NKaiserCrabBossVfx.PropertyName._steamParticles1))
    {
      this._steamParticles1 = VariantUtils.ConvertTo<GpuParticles2D>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NKaiserCrabBossVfx.PropertyName._steamParticles2))
    {
      this._steamParticles2 = VariantUtils.ConvertTo<GpuParticles2D>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NKaiserCrabBossVfx.PropertyName._steamParticles3))
    {
      this._steamParticles3 = VariantUtils.ConvertTo<GpuParticles2D>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NKaiserCrabBossVfx.PropertyName._smokeParticles))
    {
      this._smokeParticles = VariantUtils.ConvertTo<GpuParticles2D>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NKaiserCrabBossVfx.PropertyName._sparkParticles))
    {
      this._sparkParticles = VariantUtils.ConvertTo<GpuParticles2D>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NKaiserCrabBossVfx.PropertyName._spittleParticles))
    {
      this._spittleParticles = VariantUtils.ConvertTo<GpuParticles2D>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NKaiserCrabBossVfx.PropertyName._leftArmExplosionPosition))
    {
      this._leftArmExplosionPosition = VariantUtils.ConvertTo<Node2D>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NKaiserCrabBossVfx.PropertyName._parent))
      return ((GodotObject) this).SetGodotClassPropertyValue(ref name, ref value);
    this._parent = VariantUtils.ConvertTo<Node2D>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NKaiserCrabBossVfx.PropertyName._regenSplatParticles))
    {
      value = VariantUtils.CreateFrom<GpuParticles2D>(ref this._regenSplatParticles);
      return true;
    }
    if (StringName.op_Equality(ref name, NKaiserCrabBossVfx.PropertyName._plowChunkParticles))
    {
      value = VariantUtils.CreateFrom<GpuParticles2D>(ref this._plowChunkParticles);
      return true;
    }
    if (StringName.op_Equality(ref name, NKaiserCrabBossVfx.PropertyName._steamParticles1))
    {
      value = VariantUtils.CreateFrom<GpuParticles2D>(ref this._steamParticles1);
      return true;
    }
    if (StringName.op_Equality(ref name, NKaiserCrabBossVfx.PropertyName._steamParticles2))
    {
      value = VariantUtils.CreateFrom<GpuParticles2D>(ref this._steamParticles2);
      return true;
    }
    if (StringName.op_Equality(ref name, NKaiserCrabBossVfx.PropertyName._steamParticles3))
    {
      value = VariantUtils.CreateFrom<GpuParticles2D>(ref this._steamParticles3);
      return true;
    }
    if (StringName.op_Equality(ref name, NKaiserCrabBossVfx.PropertyName._smokeParticles))
    {
      value = VariantUtils.CreateFrom<GpuParticles2D>(ref this._smokeParticles);
      return true;
    }
    if (StringName.op_Equality(ref name, NKaiserCrabBossVfx.PropertyName._sparkParticles))
    {
      value = VariantUtils.CreateFrom<GpuParticles2D>(ref this._sparkParticles);
      return true;
    }
    if (StringName.op_Equality(ref name, NKaiserCrabBossVfx.PropertyName._spittleParticles))
    {
      value = VariantUtils.CreateFrom<GpuParticles2D>(ref this._spittleParticles);
      return true;
    }
    if (StringName.op_Equality(ref name, NKaiserCrabBossVfx.PropertyName._leftArmExplosionPosition))
    {
      value = VariantUtils.CreateFrom<Node2D>(ref this._leftArmExplosionPosition);
      return true;
    }
    if (!StringName.op_Equality(ref name, NKaiserCrabBossVfx.PropertyName._parent))
      return ((GodotObject) this).GetGodotClassPropertyValue(ref name, ref value);
    value = VariantUtils.CreateFrom<Node2D>(ref this._parent);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NKaiserCrabBossVfx.PropertyName._regenSplatParticles, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NKaiserCrabBossVfx.PropertyName._plowChunkParticles, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NKaiserCrabBossVfx.PropertyName._steamParticles1, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NKaiserCrabBossVfx.PropertyName._steamParticles2, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NKaiserCrabBossVfx.PropertyName._steamParticles3, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NKaiserCrabBossVfx.PropertyName._smokeParticles, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NKaiserCrabBossVfx.PropertyName._sparkParticles, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NKaiserCrabBossVfx.PropertyName._spittleParticles, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NKaiserCrabBossVfx.PropertyName._leftArmExplosionPosition, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NKaiserCrabBossVfx.PropertyName._parent, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).SaveGodotObjectData(info);
    info.AddProperty(NKaiserCrabBossVfx.PropertyName._regenSplatParticles, Variant.From<GpuParticles2D>(ref this._regenSplatParticles));
    info.AddProperty(NKaiserCrabBossVfx.PropertyName._plowChunkParticles, Variant.From<GpuParticles2D>(ref this._plowChunkParticles));
    info.AddProperty(NKaiserCrabBossVfx.PropertyName._steamParticles1, Variant.From<GpuParticles2D>(ref this._steamParticles1));
    info.AddProperty(NKaiserCrabBossVfx.PropertyName._steamParticles2, Variant.From<GpuParticles2D>(ref this._steamParticles2));
    info.AddProperty(NKaiserCrabBossVfx.PropertyName._steamParticles3, Variant.From<GpuParticles2D>(ref this._steamParticles3));
    info.AddProperty(NKaiserCrabBossVfx.PropertyName._smokeParticles, Variant.From<GpuParticles2D>(ref this._smokeParticles));
    info.AddProperty(NKaiserCrabBossVfx.PropertyName._sparkParticles, Variant.From<GpuParticles2D>(ref this._sparkParticles));
    info.AddProperty(NKaiserCrabBossVfx.PropertyName._spittleParticles, Variant.From<GpuParticles2D>(ref this._spittleParticles));
    info.AddProperty(NKaiserCrabBossVfx.PropertyName._leftArmExplosionPosition, Variant.From<Node2D>(ref this._leftArmExplosionPosition));
    info.AddProperty(NKaiserCrabBossVfx.PropertyName._parent, Variant.From<Node2D>(ref this._parent));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NKaiserCrabBossVfx.PropertyName._regenSplatParticles, ref variant1))
      this._regenSplatParticles = ((Variant) ref variant1).As<GpuParticles2D>();
    Variant variant2;
    if (info.TryGetProperty(NKaiserCrabBossVfx.PropertyName._plowChunkParticles, ref variant2))
      this._plowChunkParticles = ((Variant) ref variant2).As<GpuParticles2D>();
    Variant variant3;
    if (info.TryGetProperty(NKaiserCrabBossVfx.PropertyName._steamParticles1, ref variant3))
      this._steamParticles1 = ((Variant) ref variant3).As<GpuParticles2D>();
    Variant variant4;
    if (info.TryGetProperty(NKaiserCrabBossVfx.PropertyName._steamParticles2, ref variant4))
      this._steamParticles2 = ((Variant) ref variant4).As<GpuParticles2D>();
    Variant variant5;
    if (info.TryGetProperty(NKaiserCrabBossVfx.PropertyName._steamParticles3, ref variant5))
      this._steamParticles3 = ((Variant) ref variant5).As<GpuParticles2D>();
    Variant variant6;
    if (info.TryGetProperty(NKaiserCrabBossVfx.PropertyName._smokeParticles, ref variant6))
      this._smokeParticles = ((Variant) ref variant6).As<GpuParticles2D>();
    Variant variant7;
    if (info.TryGetProperty(NKaiserCrabBossVfx.PropertyName._sparkParticles, ref variant7))
      this._sparkParticles = ((Variant) ref variant7).As<GpuParticles2D>();
    Variant variant8;
    if (info.TryGetProperty(NKaiserCrabBossVfx.PropertyName._spittleParticles, ref variant8))
      this._spittleParticles = ((Variant) ref variant8).As<GpuParticles2D>();
    Variant variant9;
    if (info.TryGetProperty(NKaiserCrabBossVfx.PropertyName._leftArmExplosionPosition, ref variant9))
      this._leftArmExplosionPosition = ((Variant) ref variant9).As<Node2D>();
    Variant variant10;
    if (!info.TryGetProperty(NKaiserCrabBossVfx.PropertyName._parent, ref variant10))
      return;
    this._parent = ((Variant) ref variant10).As<Node2D>();
  }

  public class MethodName : Node.MethodName
  {
    public static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName OnAnimationEvent = StringName.op_Implicit(nameof (OnAnimationEvent));
    public static readonly StringName OnAnimationStart = StringName.op_Implicit(nameof (OnAnimationStart));
    public static readonly StringName OnChargeSteamStart = StringName.op_Implicit(nameof (OnChargeSteamStart));
    public static readonly StringName OnChargeSteamEnd = StringName.op_Implicit(nameof (OnChargeSteamEnd));
    public static readonly StringName OnDeathSpitStart = StringName.op_Implicit(nameof (OnDeathSpitStart));
    public static readonly StringName OnDeathSpitEnd = StringName.op_Implicit(nameof (OnDeathSpitEnd));
    public static readonly StringName OnLeftEmbersStart = StringName.op_Implicit(nameof (OnLeftEmbersStart));
    public static readonly StringName OnPlowChunksStart = StringName.op_Implicit(nameof (OnPlowChunksStart));
    public static readonly StringName OnPlowChunksEnd = StringName.op_Implicit(nameof (OnPlowChunksEnd));
    public static readonly StringName OnRegenSplatsStart = StringName.op_Implicit(nameof (OnRegenSplatsStart));
    public static readonly StringName OnRegenSplatsEnd = StringName.op_Implicit(nameof (OnRegenSplatsEnd));
    public static readonly StringName OnRocketThrustStart = StringName.op_Implicit(nameof (OnRocketThrustStart));
    public static readonly StringName OnRocketThrustEnd = StringName.op_Implicit(nameof (OnRocketThrustEnd));
    public static readonly StringName OnClawLExplode = StringName.op_Implicit(nameof (OnClawLExplode));
  }

  public class PropertyName : Node.PropertyName
  {
    public static readonly StringName _regenSplatParticles = StringName.op_Implicit(nameof (_regenSplatParticles));
    public static readonly StringName _plowChunkParticles = StringName.op_Implicit(nameof (_plowChunkParticles));
    public static readonly StringName _steamParticles1 = StringName.op_Implicit(nameof (_steamParticles1));
    public static readonly StringName _steamParticles2 = StringName.op_Implicit(nameof (_steamParticles2));
    public static readonly StringName _steamParticles3 = StringName.op_Implicit(nameof (_steamParticles3));
    public static readonly StringName _smokeParticles = StringName.op_Implicit(nameof (_smokeParticles));
    public static readonly StringName _sparkParticles = StringName.op_Implicit(nameof (_sparkParticles));
    public static readonly StringName _spittleParticles = StringName.op_Implicit(nameof (_spittleParticles));
    public static readonly StringName _leftArmExplosionPosition = StringName.op_Implicit(nameof (_leftArmExplosionPosition));
    public static readonly StringName _parent = StringName.op_Implicit(nameof (_parent));
  }

  public class SignalName : Node.SignalName
  {
  }
}
