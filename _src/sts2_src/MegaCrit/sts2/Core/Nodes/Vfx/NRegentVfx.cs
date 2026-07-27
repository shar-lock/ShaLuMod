// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Vfx.NRegentVfx
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
[ScriptPath("res://src/Core/Nodes/Vfx/NRegentVfx.cs")]
public class NRegentVfx : Node
{
  private GpuParticles2D _deathParticlesArm;
  private GpuParticles2D _deathParticlesChest;
  private GpuParticles2D _deathParticlesBack;
  private GpuParticles2D _deathParticlesLeg;
  private GpuParticles2D _deathParticlesLegL;
  private GpuParticles2D _explosionParticles;
  private MegaSprite _weapon;
  private MegaSprite _weapon2;
  private GpuParticles2D _attackParticlesSmall;
  private GpuParticles2D _attackParticlesSmall2;
  private GpuParticles2D _attackParticlesLarge;
  private MegaAnimationState? _weaponAnimState;
  private MegaAnimationState? _weaponAnimState2;
  private int _curWeapon = 1;
  private Node2D _parent;
  private MegaSprite _animController;

  public override void _Ready()
  {
    this._parent = this.GetParent<Node2D>();
    this._animController = new MegaSprite(Variant.op_Implicit((GodotObject) this._parent));
    this._animController.ConnectAnimationEvent(Callable.From<GodotObject, GodotObject, GodotObject, GodotObject>(new Action<GodotObject, GodotObject, GodotObject, GodotObject>(this.OnAnimationEvent)));
    this._animController.ConnectAnimationStarted(Callable.From<GodotObject, GodotObject, GodotObject>(new Action<GodotObject, GodotObject, GodotObject>(this.OnAnimationStart)));
    this._deathParticlesArm = ((Node) this._parent).GetNode<GpuParticles2D>(NodePath.op_Implicit("SpineArmBone/Particles"));
    this._deathParticlesChest = ((Node) this._parent).GetNode<GpuParticles2D>(NodePath.op_Implicit("SpineChestBone/Particles"));
    this._deathParticlesBack = ((Node) this._parent).GetNode<GpuParticles2D>(NodePath.op_Implicit("SpineChestBone/ParticlesBack"));
    this._deathParticlesLeg = ((Node) this._parent).GetNode<GpuParticles2D>(NodePath.op_Implicit("SpineLegBone/Particles"));
    this._deathParticlesLegL = ((Node) this._parent).GetNode<GpuParticles2D>(NodePath.op_Implicit("SpineLegBoneL/Particles"));
    this._explosionParticles = ((Node) this._parent).GetNode<GpuParticles2D>(NodePath.op_Implicit("Explosion"));
    this._weapon = new MegaSprite(Variant.op_Implicit((GodotObject) ((Node) this._parent).GetNode(NodePath.op_Implicit("Weapons/WeaponAnim1"))));
    this._weapon2 = new MegaSprite(Variant.op_Implicit((GodotObject) ((Node) this._parent).GetNode(NodePath.op_Implicit("Weapons/WeaponAnim2"))));
    this.RunWhenSpineReady(this._weapon, (Action<MegaAnimationState>) (animState => this._weaponAnimState = animState));
    this.RunWhenSpineReady(this._weapon2, (Action<MegaAnimationState>) (animState => this._weaponAnimState2 = animState));
    this._deathParticlesArm.Emitting = false;
    this._deathParticlesChest.Emitting = false;
    this._deathParticlesBack.Emitting = false;
    this._deathParticlesLeg.Emitting = false;
    this._deathParticlesLegL.Emitting = false;
    this._explosionParticles.Emitting = false;
  }

  private void OnAnimationEvent(
    GodotObject _,
    GodotObject __,
    GodotObject ___,
    GodotObject spineEvent)
  {
    switch (new MegaEvent(Variant.op_Implicit(spineEvent)).GetData().GetEventName())
    {
      case "death_particles_start":
        this.TurnOnDying();
        break;
      case "death_particles_start2":
        this.TurnOnDying2();
        break;
      case "death_particles_end":
        this.TurnOffDying();
        break;
      case "explode_dead":
        this.Explode();
        break;
      case "explode_end":
        this.DisableExplode();
        break;
      case "attack1":
        this.Attack();
        break;
    }
  }

  private void TurnOnDying()
  {
    this._deathParticlesArm.Restart();
    this._deathParticlesLeg.Restart();
    this._deathParticlesLegL.Restart();
  }

  private void TurnOnDying2()
  {
    this._deathParticlesChest.Restart();
    this._deathParticlesBack.Restart();
  }

  private void TurnOffDying()
  {
    this._deathParticlesArm.Emitting = false;
    this._deathParticlesChest.Emitting = false;
    this._deathParticlesBack.Emitting = false;
    this._deathParticlesLeg.Emitting = false;
    this._deathParticlesLegL.Emitting = false;
  }

  private void Explode() => this._explosionParticles.Restart();

  private void DisableExplode() => this._explosionParticles.Emitting = false;

  private void Attack()
  {
    if (this._curWeapon == 1)
    {
      this._weaponAnimState?.SetAnimation("attack", false);
      this._curWeapon = 2;
    }
    else
    {
      this._weaponAnimState2?.SetAnimation("attack2", false);
      this._curWeapon = 1;
    }
  }

  private void OnAnimationStart(
    GodotObject spineSprite,
    GodotObject animationState,
    GodotObject trackEntry)
  {
    if (!(new MegaAnimationState(Variant.op_Implicit(animationState)).GetCurrentAnimationName() != "die"))
      return;
    this.DisableExplode();
    this.TurnOffDying();
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(9)
    {
      new MethodInfo(NRegentVfx.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NRegentVfx.MethodName.OnAnimationEvent, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("_"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Object"), false),
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("__"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Object"), false),
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("___"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Object"), false),
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("spineEvent"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Object"), false)
      }, (List<Variant>) null),
      new MethodInfo(NRegentVfx.MethodName.TurnOnDying, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NRegentVfx.MethodName.TurnOnDying2, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NRegentVfx.MethodName.TurnOffDying, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NRegentVfx.MethodName.Explode, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NRegentVfx.MethodName.DisableExplode, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NRegentVfx.MethodName.Attack, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NRegentVfx.MethodName.OnAnimationStart, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
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
    if (StringName.op_Equality(ref method, NRegentVfx.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      base._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NRegentVfx.MethodName.OnAnimationEvent) && ((NativeVariantPtrArgs) ref args).Count == 4)
    {
      this.OnAnimationEvent(VariantUtils.ConvertTo<GodotObject>(ref ((NativeVariantPtrArgs) ref args)[0]), VariantUtils.ConvertTo<GodotObject>(ref ((NativeVariantPtrArgs) ref args)[1]), VariantUtils.ConvertTo<GodotObject>(ref ((NativeVariantPtrArgs) ref args)[2]), VariantUtils.ConvertTo<GodotObject>(ref ((NativeVariantPtrArgs) ref args)[3]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NRegentVfx.MethodName.TurnOnDying) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.TurnOnDying();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NRegentVfx.MethodName.TurnOnDying2) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.TurnOnDying2();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NRegentVfx.MethodName.TurnOffDying) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.TurnOffDying();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NRegentVfx.MethodName.Explode) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.Explode();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NRegentVfx.MethodName.DisableExplode) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.DisableExplode();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NRegentVfx.MethodName.Attack) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.Attack();
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NRegentVfx.MethodName.OnAnimationStart) || ((NativeVariantPtrArgs) ref args).Count != 3)
      return base.InvokeGodotClassMethod(ref method, args, ref ret);
    this.OnAnimationStart(VariantUtils.ConvertTo<GodotObject>(ref ((NativeVariantPtrArgs) ref args)[0]), VariantUtils.ConvertTo<GodotObject>(ref ((NativeVariantPtrArgs) ref args)[1]), VariantUtils.ConvertTo<GodotObject>(ref ((NativeVariantPtrArgs) ref args)[2]));
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NRegentVfx.MethodName._Ready) || StringName.op_Equality(ref method, NRegentVfx.MethodName.OnAnimationEvent) || StringName.op_Equality(ref method, NRegentVfx.MethodName.TurnOnDying) || StringName.op_Equality(ref method, NRegentVfx.MethodName.TurnOnDying2) || StringName.op_Equality(ref method, NRegentVfx.MethodName.TurnOffDying) || StringName.op_Equality(ref method, NRegentVfx.MethodName.Explode) || StringName.op_Equality(ref method, NRegentVfx.MethodName.DisableExplode) || StringName.op_Equality(ref method, NRegentVfx.MethodName.Attack) || StringName.op_Equality(ref method, NRegentVfx.MethodName.OnAnimationStart) || base.HasGodotClassMethod(ref method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NRegentVfx.PropertyName._deathParticlesArm))
    {
      this._deathParticlesArm = VariantUtils.ConvertTo<GpuParticles2D>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NRegentVfx.PropertyName._deathParticlesChest))
    {
      this._deathParticlesChest = VariantUtils.ConvertTo<GpuParticles2D>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NRegentVfx.PropertyName._deathParticlesBack))
    {
      this._deathParticlesBack = VariantUtils.ConvertTo<GpuParticles2D>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NRegentVfx.PropertyName._deathParticlesLeg))
    {
      this._deathParticlesLeg = VariantUtils.ConvertTo<GpuParticles2D>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NRegentVfx.PropertyName._deathParticlesLegL))
    {
      this._deathParticlesLegL = VariantUtils.ConvertTo<GpuParticles2D>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NRegentVfx.PropertyName._explosionParticles))
    {
      this._explosionParticles = VariantUtils.ConvertTo<GpuParticles2D>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NRegentVfx.PropertyName._attackParticlesSmall))
    {
      this._attackParticlesSmall = VariantUtils.ConvertTo<GpuParticles2D>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NRegentVfx.PropertyName._attackParticlesSmall2))
    {
      this._attackParticlesSmall2 = VariantUtils.ConvertTo<GpuParticles2D>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NRegentVfx.PropertyName._attackParticlesLarge))
    {
      this._attackParticlesLarge = VariantUtils.ConvertTo<GpuParticles2D>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NRegentVfx.PropertyName._curWeapon))
    {
      this._curWeapon = VariantUtils.ConvertTo<int>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NRegentVfx.PropertyName._parent))
      return ((GodotObject) this).SetGodotClassPropertyValue(ref name, ref value);
    this._parent = VariantUtils.ConvertTo<Node2D>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NRegentVfx.PropertyName._deathParticlesArm))
    {
      value = VariantUtils.CreateFrom<GpuParticles2D>(ref this._deathParticlesArm);
      return true;
    }
    if (StringName.op_Equality(ref name, NRegentVfx.PropertyName._deathParticlesChest))
    {
      value = VariantUtils.CreateFrom<GpuParticles2D>(ref this._deathParticlesChest);
      return true;
    }
    if (StringName.op_Equality(ref name, NRegentVfx.PropertyName._deathParticlesBack))
    {
      value = VariantUtils.CreateFrom<GpuParticles2D>(ref this._deathParticlesBack);
      return true;
    }
    if (StringName.op_Equality(ref name, NRegentVfx.PropertyName._deathParticlesLeg))
    {
      value = VariantUtils.CreateFrom<GpuParticles2D>(ref this._deathParticlesLeg);
      return true;
    }
    if (StringName.op_Equality(ref name, NRegentVfx.PropertyName._deathParticlesLegL))
    {
      value = VariantUtils.CreateFrom<GpuParticles2D>(ref this._deathParticlesLegL);
      return true;
    }
    if (StringName.op_Equality(ref name, NRegentVfx.PropertyName._explosionParticles))
    {
      value = VariantUtils.CreateFrom<GpuParticles2D>(ref this._explosionParticles);
      return true;
    }
    if (StringName.op_Equality(ref name, NRegentVfx.PropertyName._attackParticlesSmall))
    {
      value = VariantUtils.CreateFrom<GpuParticles2D>(ref this._attackParticlesSmall);
      return true;
    }
    if (StringName.op_Equality(ref name, NRegentVfx.PropertyName._attackParticlesSmall2))
    {
      value = VariantUtils.CreateFrom<GpuParticles2D>(ref this._attackParticlesSmall2);
      return true;
    }
    if (StringName.op_Equality(ref name, NRegentVfx.PropertyName._attackParticlesLarge))
    {
      value = VariantUtils.CreateFrom<GpuParticles2D>(ref this._attackParticlesLarge);
      return true;
    }
    if (StringName.op_Equality(ref name, NRegentVfx.PropertyName._curWeapon))
    {
      value = VariantUtils.CreateFrom<int>(ref this._curWeapon);
      return true;
    }
    if (!StringName.op_Equality(ref name, NRegentVfx.PropertyName._parent))
      return ((GodotObject) this).GetGodotClassPropertyValue(ref name, ref value);
    value = VariantUtils.CreateFrom<Node2D>(ref this._parent);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NRegentVfx.PropertyName._deathParticlesArm, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NRegentVfx.PropertyName._deathParticlesChest, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NRegentVfx.PropertyName._deathParticlesBack, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NRegentVfx.PropertyName._deathParticlesLeg, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NRegentVfx.PropertyName._deathParticlesLegL, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NRegentVfx.PropertyName._explosionParticles, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NRegentVfx.PropertyName._attackParticlesSmall, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NRegentVfx.PropertyName._attackParticlesSmall2, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NRegentVfx.PropertyName._attackParticlesLarge, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 2L, NRegentVfx.PropertyName._curWeapon, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NRegentVfx.PropertyName._parent, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).SaveGodotObjectData(info);
    info.AddProperty(NRegentVfx.PropertyName._deathParticlesArm, Variant.From<GpuParticles2D>(ref this._deathParticlesArm));
    info.AddProperty(NRegentVfx.PropertyName._deathParticlesChest, Variant.From<GpuParticles2D>(ref this._deathParticlesChest));
    info.AddProperty(NRegentVfx.PropertyName._deathParticlesBack, Variant.From<GpuParticles2D>(ref this._deathParticlesBack));
    info.AddProperty(NRegentVfx.PropertyName._deathParticlesLeg, Variant.From<GpuParticles2D>(ref this._deathParticlesLeg));
    info.AddProperty(NRegentVfx.PropertyName._deathParticlesLegL, Variant.From<GpuParticles2D>(ref this._deathParticlesLegL));
    info.AddProperty(NRegentVfx.PropertyName._explosionParticles, Variant.From<GpuParticles2D>(ref this._explosionParticles));
    info.AddProperty(NRegentVfx.PropertyName._attackParticlesSmall, Variant.From<GpuParticles2D>(ref this._attackParticlesSmall));
    info.AddProperty(NRegentVfx.PropertyName._attackParticlesSmall2, Variant.From<GpuParticles2D>(ref this._attackParticlesSmall2));
    info.AddProperty(NRegentVfx.PropertyName._attackParticlesLarge, Variant.From<GpuParticles2D>(ref this._attackParticlesLarge));
    info.AddProperty(NRegentVfx.PropertyName._curWeapon, Variant.From<int>(ref this._curWeapon));
    info.AddProperty(NRegentVfx.PropertyName._parent, Variant.From<Node2D>(ref this._parent));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NRegentVfx.PropertyName._deathParticlesArm, ref variant1))
      this._deathParticlesArm = ((Variant) ref variant1).As<GpuParticles2D>();
    Variant variant2;
    if (info.TryGetProperty(NRegentVfx.PropertyName._deathParticlesChest, ref variant2))
      this._deathParticlesChest = ((Variant) ref variant2).As<GpuParticles2D>();
    Variant variant3;
    if (info.TryGetProperty(NRegentVfx.PropertyName._deathParticlesBack, ref variant3))
      this._deathParticlesBack = ((Variant) ref variant3).As<GpuParticles2D>();
    Variant variant4;
    if (info.TryGetProperty(NRegentVfx.PropertyName._deathParticlesLeg, ref variant4))
      this._deathParticlesLeg = ((Variant) ref variant4).As<GpuParticles2D>();
    Variant variant5;
    if (info.TryGetProperty(NRegentVfx.PropertyName._deathParticlesLegL, ref variant5))
      this._deathParticlesLegL = ((Variant) ref variant5).As<GpuParticles2D>();
    Variant variant6;
    if (info.TryGetProperty(NRegentVfx.PropertyName._explosionParticles, ref variant6))
      this._explosionParticles = ((Variant) ref variant6).As<GpuParticles2D>();
    Variant variant7;
    if (info.TryGetProperty(NRegentVfx.PropertyName._attackParticlesSmall, ref variant7))
      this._attackParticlesSmall = ((Variant) ref variant7).As<GpuParticles2D>();
    Variant variant8;
    if (info.TryGetProperty(NRegentVfx.PropertyName._attackParticlesSmall2, ref variant8))
      this._attackParticlesSmall2 = ((Variant) ref variant8).As<GpuParticles2D>();
    Variant variant9;
    if (info.TryGetProperty(NRegentVfx.PropertyName._attackParticlesLarge, ref variant9))
      this._attackParticlesLarge = ((Variant) ref variant9).As<GpuParticles2D>();
    Variant variant10;
    if (info.TryGetProperty(NRegentVfx.PropertyName._curWeapon, ref variant10))
      this._curWeapon = ((Variant) ref variant10).As<int>();
    Variant variant11;
    if (!info.TryGetProperty(NRegentVfx.PropertyName._parent, ref variant11))
      return;
    this._parent = ((Variant) ref variant11).As<Node2D>();
  }

  public class MethodName : Node.MethodName
  {
    public static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName OnAnimationEvent = StringName.op_Implicit(nameof (OnAnimationEvent));
    public static readonly StringName TurnOnDying = StringName.op_Implicit(nameof (TurnOnDying));
    public static readonly StringName TurnOnDying2 = StringName.op_Implicit(nameof (TurnOnDying2));
    public static readonly StringName TurnOffDying = StringName.op_Implicit(nameof (TurnOffDying));
    public static readonly StringName Explode = StringName.op_Implicit(nameof (Explode));
    public static readonly StringName DisableExplode = StringName.op_Implicit(nameof (DisableExplode));
    public static readonly StringName Attack = StringName.op_Implicit(nameof (Attack));
    public static readonly StringName OnAnimationStart = StringName.op_Implicit(nameof (OnAnimationStart));
  }

  public class PropertyName : Node.PropertyName
  {
    public static readonly StringName _deathParticlesArm = StringName.op_Implicit(nameof (_deathParticlesArm));
    public static readonly StringName _deathParticlesChest = StringName.op_Implicit(nameof (_deathParticlesChest));
    public static readonly StringName _deathParticlesBack = StringName.op_Implicit(nameof (_deathParticlesBack));
    public static readonly StringName _deathParticlesLeg = StringName.op_Implicit(nameof (_deathParticlesLeg));
    public static readonly StringName _deathParticlesLegL = StringName.op_Implicit(nameof (_deathParticlesLegL));
    public static readonly StringName _explosionParticles = StringName.op_Implicit(nameof (_explosionParticles));
    public static readonly StringName _attackParticlesSmall = StringName.op_Implicit(nameof (_attackParticlesSmall));
    public static readonly StringName _attackParticlesSmall2 = StringName.op_Implicit(nameof (_attackParticlesSmall2));
    public static readonly StringName _attackParticlesLarge = StringName.op_Implicit(nameof (_attackParticlesLarge));
    public static readonly StringName _curWeapon = StringName.op_Implicit(nameof (_curWeapon));
    public static readonly StringName _parent = StringName.op_Implicit(nameof (_parent));
  }

  public class SignalName : Node.SignalName
  {
  }
}
