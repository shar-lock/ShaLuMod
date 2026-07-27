// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Vfx.NAeonGlassVfx
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

[ScriptPath("res://src/Core/Nodes/Vfx/NAeonGlassVfx.cs")]
public class NAeonGlassVfx : Node
{
  private bool _ringsSpinningNormal;
  private string? _curAnimName;
  private static readonly StringName _scrollSpeedString = new StringName("ScrollSpeed");
  private ShaderMaterial? _liquidShaderMat;
  private float _baseScrollSpeed;
  private Node2D _parent;
  private MegaSprite _animController;
  private GpuParticles2D _witherParticles;
  private GpuParticles2D _leakParticles;
  private GpuParticles2D _shardParticles;
  private GpuParticles2D _dumpParticles;
  private GpuParticles2D _topSparkParticles;
  private GpuParticles2D _bottomSparkParticles;
  private GpuParticles2D _groundDustParticles;
  private GpuParticles2D _groundChunkParticles;

  public override void _Ready()
  {
    this._parent = this.GetParent<Node2D>();
    this._animController = new MegaSprite(Variant.op_Implicit((GodotObject) this._parent));
    this._liquidShaderMat = new MegaSlotNode(Variant.op_Implicit((GodotObject) ((Node) this._parent).GetNode(NodePath.op_Implicit("LiquidSlot")))).GetNormalMaterial() as ShaderMaterial;
    if (this._liquidShaderMat != null)
      this._baseScrollSpeed = Variant.op_Explicit(this._liquidShaderMat.GetShaderParameter(NAeonGlassVfx._scrollSpeedString));
    this._witherParticles = ((Node) this._parent).GetNode<GpuParticles2D>(NodePath.op_Implicit("WitherSlot/WitherParticles"));
    this._leakParticles = ((Node) this._parent).GetNode<GpuParticles2D>(NodePath.op_Implicit("LiquidSlot/LeakParticles"));
    this._shardParticles = ((Node) this._parent).GetNode<GpuParticles2D>(NodePath.op_Implicit("GlassCenterSlot/ShardParticles"));
    this._dumpParticles = ((Node) this._parent).GetNode<GpuParticles2D>(NodePath.op_Implicit("GlassCenterSlot/DumpParticles"));
    this._topSparkParticles = ((Node) this._parent).GetNode<GpuParticles2D>(NodePath.op_Implicit("TopSparksSlot/TopSparkParticles"));
    this._bottomSparkParticles = ((Node) this._parent).GetNode<GpuParticles2D>(NodePath.op_Implicit("BottomSparksSlot/BottomSparkParticles"));
    this._groundDustParticles = ((Node) this._parent).GetNode<GpuParticles2D>(NodePath.op_Implicit("GroundPlowSlot/DustParticles"));
    this._groundChunkParticles = ((Node) this._parent).GetNode<GpuParticles2D>(NodePath.op_Implicit("GroundPlowSlot/ChunkParticles"));
    this._bottomSparkParticles.OneShot = true;
    this._topSparkParticles.OneShot = true;
    this._witherParticles.OneShot = true;
    this._shardParticles.OneShot = true;
    this._dumpParticles.OneShot = true;
    this.ResetVfx();
    this.RunWhenSpineReady(this._animController, (Action<MegaAnimationState>) (animState => animState.SetAnimation("_track1/rings_normal", trackId: 1)));
    this._animController.ConnectAnimationEvent(Callable.From<GodotObject, GodotObject, GodotObject, GodotObject>(new Action<GodotObject, GodotObject, GodotObject, GodotObject>(this.OnAnimationEvent)));
    this._animController.ConnectAnimationStarted(Callable.From<GodotObject, GodotObject, GodotObject>(new Action<GodotObject, GodotObject, GodotObject>(this.OnAnimationStart)));
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
        if (!(eventName == "end_die"))
          break;
        this.EndDie();
        break;
      case 9:
        if (!(eventName == "start_die"))
          break;
        this.StartDie();
        break;
      case 10:
        if (!(eventName == "end_wither"))
          break;
        this.EndWither();
        break;
      case 12:
        switch (eventName[6])
        {
          case 's':
            if (!(eventName == "start_sparks"))
              return;
            this.StartSparks();
            return;
          case 'w':
            if (!(eventName == "start_wither"))
              return;
            this.StartWither();
            return;
          default:
            return;
        }
      case 17:
        if (!(eventName == "end_ground_scrape"))
          break;
        this.EndScrape();
        break;
      case 19:
        if (!(eventName == "start_ground_scrape"))
          break;
        this.StartScrape();
        break;
    }
  }

  private void OnAnimationStart(
    GodotObject spineSprite,
    GodotObject animationState,
    GodotObject trackEntry)
  {
    this.ResetVfx();
    MegaAnimationState animationState1 = this._animController.GetAnimationState();
    string currentAnimationName = animationState1.GetCurrentAnimationName();
    if (currentAnimationName == this._curAnimName)
      return;
    this._curAnimName = currentAnimationName;
    if (currentAnimationName == "idle_loop" || currentAnimationName == "hurt" || currentAnimationName == "wither")
    {
      if (!this._ringsSpinningNormal)
        animationState1.SetAnimation("_track1/rings_normal", trackId: 1);
      this._ringsSpinningNormal = true;
    }
    switch (currentAnimationName)
    {
      case "attack_heavy":
        animationState1.SetAnimation("_track1/rings_attack_heavy", false, 1);
        animationState1.AddAnimation("_track1/rings_normal", trackId: 1);
        this._ringsSpinningNormal = false;
        break;
      case "attack_double":
        animationState1.SetAnimation("_track1/rings_attack_double", false, 1);
        animationState1.AddAnimation("_track1/rings_normal", trackId: 1);
        this._ringsSpinningNormal = false;
        break;
      case "die":
        animationState1.SetAnimation("_track1/rings_die", false, 1);
        this._ringsSpinningNormal = false;
        break;
    }
  }

  private void StartWither()
  {
    this._witherParticles.Restart();
    this._liquidShaderMat?.SetShaderParameter(NAeonGlassVfx._scrollSpeedString, Variant.op_Implicit(-2f));
  }

  private void EndWither() => this.ResetVfx();

  private void StartDie()
  {
    this._dumpParticles.Restart();
    this._leakParticles.Restart();
    this._shardParticles.Restart();
  }

  private void EndDie() => this._leakParticles.Emitting = false;

  private void StartSparks()
  {
    this._topSparkParticles.Restart();
    this._bottomSparkParticles.Restart();
  }

  private void StartScrape()
  {
    this._groundChunkParticles.Restart();
    this._groundDustParticles.Restart();
  }

  private void EndScrape()
  {
    this._groundChunkParticles.Emitting = false;
    this._groundDustParticles.Emitting = false;
  }

  private void ResetVfx()
  {
    this._liquidShaderMat?.SetShaderParameter(NAeonGlassVfx._scrollSpeedString, Variant.op_Implicit(this._baseScrollSpeed));
    this._witherParticles.Restart();
    this._witherParticles.Emitting = false;
    this._leakParticles.Restart();
    this._leakParticles.Emitting = false;
    this._shardParticles.Emitting = false;
    this._dumpParticles.Restart();
    this._dumpParticles.Emitting = false;
    this._topSparkParticles.Emitting = false;
    this._bottomSparkParticles.Emitting = false;
    this._groundChunkParticles.Emitting = false;
    this._groundDustParticles.Emitting = false;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(11)
    {
      new MethodInfo(NAeonGlassVfx.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NAeonGlassVfx.MethodName.OnAnimationEvent, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("_"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Object"), false),
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("__"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Object"), false),
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("___"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Object"), false),
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("spineEvent"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Object"), false)
      }, (List<Variant>) null),
      new MethodInfo(NAeonGlassVfx.MethodName.OnAnimationStart, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("spineSprite"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Object"), false),
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("animationState"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Object"), false),
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("trackEntry"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Object"), false)
      }, (List<Variant>) null),
      new MethodInfo(NAeonGlassVfx.MethodName.StartWither, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NAeonGlassVfx.MethodName.EndWither, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NAeonGlassVfx.MethodName.StartDie, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NAeonGlassVfx.MethodName.EndDie, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NAeonGlassVfx.MethodName.StartSparks, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NAeonGlassVfx.MethodName.StartScrape, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NAeonGlassVfx.MethodName.EndScrape, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NAeonGlassVfx.MethodName.ResetVfx, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NAeonGlassVfx.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      base._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NAeonGlassVfx.MethodName.OnAnimationEvent) && ((NativeVariantPtrArgs) ref args).Count == 4)
    {
      this.OnAnimationEvent(VariantUtils.ConvertTo<GodotObject>(ref ((NativeVariantPtrArgs) ref args)[0]), VariantUtils.ConvertTo<GodotObject>(ref ((NativeVariantPtrArgs) ref args)[1]), VariantUtils.ConvertTo<GodotObject>(ref ((NativeVariantPtrArgs) ref args)[2]), VariantUtils.ConvertTo<GodotObject>(ref ((NativeVariantPtrArgs) ref args)[3]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NAeonGlassVfx.MethodName.OnAnimationStart) && ((NativeVariantPtrArgs) ref args).Count == 3)
    {
      this.OnAnimationStart(VariantUtils.ConvertTo<GodotObject>(ref ((NativeVariantPtrArgs) ref args)[0]), VariantUtils.ConvertTo<GodotObject>(ref ((NativeVariantPtrArgs) ref args)[1]), VariantUtils.ConvertTo<GodotObject>(ref ((NativeVariantPtrArgs) ref args)[2]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NAeonGlassVfx.MethodName.StartWither) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.StartWither();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NAeonGlassVfx.MethodName.EndWither) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.EndWither();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NAeonGlassVfx.MethodName.StartDie) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.StartDie();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NAeonGlassVfx.MethodName.EndDie) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.EndDie();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NAeonGlassVfx.MethodName.StartSparks) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.StartSparks();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NAeonGlassVfx.MethodName.StartScrape) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.StartScrape();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NAeonGlassVfx.MethodName.EndScrape) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.EndScrape();
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NAeonGlassVfx.MethodName.ResetVfx) || ((NativeVariantPtrArgs) ref args).Count != 0)
      return base.InvokeGodotClassMethod(ref method, args, ref ret);
    this.ResetVfx();
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NAeonGlassVfx.MethodName._Ready) || StringName.op_Equality(ref method, NAeonGlassVfx.MethodName.OnAnimationEvent) || StringName.op_Equality(ref method, NAeonGlassVfx.MethodName.OnAnimationStart) || StringName.op_Equality(ref method, NAeonGlassVfx.MethodName.StartWither) || StringName.op_Equality(ref method, NAeonGlassVfx.MethodName.EndWither) || StringName.op_Equality(ref method, NAeonGlassVfx.MethodName.StartDie) || StringName.op_Equality(ref method, NAeonGlassVfx.MethodName.EndDie) || StringName.op_Equality(ref method, NAeonGlassVfx.MethodName.StartSparks) || StringName.op_Equality(ref method, NAeonGlassVfx.MethodName.StartScrape) || StringName.op_Equality(ref method, NAeonGlassVfx.MethodName.EndScrape) || StringName.op_Equality(ref method, NAeonGlassVfx.MethodName.ResetVfx) || base.HasGodotClassMethod(ref method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NAeonGlassVfx.PropertyName._ringsSpinningNormal))
    {
      this._ringsSpinningNormal = VariantUtils.ConvertTo<bool>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NAeonGlassVfx.PropertyName._curAnimName))
    {
      this._curAnimName = VariantUtils.ConvertTo<string>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NAeonGlassVfx.PropertyName._liquidShaderMat))
    {
      this._liquidShaderMat = VariantUtils.ConvertTo<ShaderMaterial>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NAeonGlassVfx.PropertyName._baseScrollSpeed))
    {
      this._baseScrollSpeed = VariantUtils.ConvertTo<float>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NAeonGlassVfx.PropertyName._parent))
    {
      this._parent = VariantUtils.ConvertTo<Node2D>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NAeonGlassVfx.PropertyName._witherParticles))
    {
      this._witherParticles = VariantUtils.ConvertTo<GpuParticles2D>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NAeonGlassVfx.PropertyName._leakParticles))
    {
      this._leakParticles = VariantUtils.ConvertTo<GpuParticles2D>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NAeonGlassVfx.PropertyName._shardParticles))
    {
      this._shardParticles = VariantUtils.ConvertTo<GpuParticles2D>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NAeonGlassVfx.PropertyName._dumpParticles))
    {
      this._dumpParticles = VariantUtils.ConvertTo<GpuParticles2D>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NAeonGlassVfx.PropertyName._topSparkParticles))
    {
      this._topSparkParticles = VariantUtils.ConvertTo<GpuParticles2D>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NAeonGlassVfx.PropertyName._bottomSparkParticles))
    {
      this._bottomSparkParticles = VariantUtils.ConvertTo<GpuParticles2D>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NAeonGlassVfx.PropertyName._groundDustParticles))
    {
      this._groundDustParticles = VariantUtils.ConvertTo<GpuParticles2D>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NAeonGlassVfx.PropertyName._groundChunkParticles))
      return ((GodotObject) this).SetGodotClassPropertyValue(ref name, ref value);
    this._groundChunkParticles = VariantUtils.ConvertTo<GpuParticles2D>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NAeonGlassVfx.PropertyName._ringsSpinningNormal))
    {
      value = VariantUtils.CreateFrom<bool>(ref this._ringsSpinningNormal);
      return true;
    }
    if (StringName.op_Equality(ref name, NAeonGlassVfx.PropertyName._curAnimName))
    {
      value = VariantUtils.CreateFrom<string>(ref this._curAnimName);
      return true;
    }
    if (StringName.op_Equality(ref name, NAeonGlassVfx.PropertyName._liquidShaderMat))
    {
      value = VariantUtils.CreateFrom<ShaderMaterial>(ref this._liquidShaderMat);
      return true;
    }
    if (StringName.op_Equality(ref name, NAeonGlassVfx.PropertyName._baseScrollSpeed))
    {
      value = VariantUtils.CreateFrom<float>(ref this._baseScrollSpeed);
      return true;
    }
    if (StringName.op_Equality(ref name, NAeonGlassVfx.PropertyName._parent))
    {
      value = VariantUtils.CreateFrom<Node2D>(ref this._parent);
      return true;
    }
    if (StringName.op_Equality(ref name, NAeonGlassVfx.PropertyName._witherParticles))
    {
      value = VariantUtils.CreateFrom<GpuParticles2D>(ref this._witherParticles);
      return true;
    }
    if (StringName.op_Equality(ref name, NAeonGlassVfx.PropertyName._leakParticles))
    {
      value = VariantUtils.CreateFrom<GpuParticles2D>(ref this._leakParticles);
      return true;
    }
    if (StringName.op_Equality(ref name, NAeonGlassVfx.PropertyName._shardParticles))
    {
      value = VariantUtils.CreateFrom<GpuParticles2D>(ref this._shardParticles);
      return true;
    }
    if (StringName.op_Equality(ref name, NAeonGlassVfx.PropertyName._dumpParticles))
    {
      value = VariantUtils.CreateFrom<GpuParticles2D>(ref this._dumpParticles);
      return true;
    }
    if (StringName.op_Equality(ref name, NAeonGlassVfx.PropertyName._topSparkParticles))
    {
      value = VariantUtils.CreateFrom<GpuParticles2D>(ref this._topSparkParticles);
      return true;
    }
    if (StringName.op_Equality(ref name, NAeonGlassVfx.PropertyName._bottomSparkParticles))
    {
      value = VariantUtils.CreateFrom<GpuParticles2D>(ref this._bottomSparkParticles);
      return true;
    }
    if (StringName.op_Equality(ref name, NAeonGlassVfx.PropertyName._groundDustParticles))
    {
      value = VariantUtils.CreateFrom<GpuParticles2D>(ref this._groundDustParticles);
      return true;
    }
    if (!StringName.op_Equality(ref name, NAeonGlassVfx.PropertyName._groundChunkParticles))
      return ((GodotObject) this).GetGodotClassPropertyValue(ref name, ref value);
    value = VariantUtils.CreateFrom<GpuParticles2D>(ref this._groundChunkParticles);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 1L, NAeonGlassVfx.PropertyName._ringsSpinningNormal, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 4L, NAeonGlassVfx.PropertyName._curAnimName, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NAeonGlassVfx.PropertyName._liquidShaderMat, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 3L, NAeonGlassVfx.PropertyName._baseScrollSpeed, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NAeonGlassVfx.PropertyName._parent, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NAeonGlassVfx.PropertyName._witherParticles, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NAeonGlassVfx.PropertyName._leakParticles, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NAeonGlassVfx.PropertyName._shardParticles, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NAeonGlassVfx.PropertyName._dumpParticles, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NAeonGlassVfx.PropertyName._topSparkParticles, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NAeonGlassVfx.PropertyName._bottomSparkParticles, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NAeonGlassVfx.PropertyName._groundDustParticles, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NAeonGlassVfx.PropertyName._groundChunkParticles, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).SaveGodotObjectData(info);
    info.AddProperty(NAeonGlassVfx.PropertyName._ringsSpinningNormal, Variant.From<bool>(ref this._ringsSpinningNormal));
    info.AddProperty(NAeonGlassVfx.PropertyName._curAnimName, Variant.From<string>(ref this._curAnimName));
    info.AddProperty(NAeonGlassVfx.PropertyName._liquidShaderMat, Variant.From<ShaderMaterial>(ref this._liquidShaderMat));
    info.AddProperty(NAeonGlassVfx.PropertyName._baseScrollSpeed, Variant.From<float>(ref this._baseScrollSpeed));
    info.AddProperty(NAeonGlassVfx.PropertyName._parent, Variant.From<Node2D>(ref this._parent));
    info.AddProperty(NAeonGlassVfx.PropertyName._witherParticles, Variant.From<GpuParticles2D>(ref this._witherParticles));
    info.AddProperty(NAeonGlassVfx.PropertyName._leakParticles, Variant.From<GpuParticles2D>(ref this._leakParticles));
    info.AddProperty(NAeonGlassVfx.PropertyName._shardParticles, Variant.From<GpuParticles2D>(ref this._shardParticles));
    info.AddProperty(NAeonGlassVfx.PropertyName._dumpParticles, Variant.From<GpuParticles2D>(ref this._dumpParticles));
    info.AddProperty(NAeonGlassVfx.PropertyName._topSparkParticles, Variant.From<GpuParticles2D>(ref this._topSparkParticles));
    info.AddProperty(NAeonGlassVfx.PropertyName._bottomSparkParticles, Variant.From<GpuParticles2D>(ref this._bottomSparkParticles));
    info.AddProperty(NAeonGlassVfx.PropertyName._groundDustParticles, Variant.From<GpuParticles2D>(ref this._groundDustParticles));
    info.AddProperty(NAeonGlassVfx.PropertyName._groundChunkParticles, Variant.From<GpuParticles2D>(ref this._groundChunkParticles));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NAeonGlassVfx.PropertyName._ringsSpinningNormal, ref variant1))
      this._ringsSpinningNormal = ((Variant) ref variant1).As<bool>();
    Variant variant2;
    if (info.TryGetProperty(NAeonGlassVfx.PropertyName._curAnimName, ref variant2))
      this._curAnimName = ((Variant) ref variant2).As<string>();
    Variant variant3;
    if (info.TryGetProperty(NAeonGlassVfx.PropertyName._liquidShaderMat, ref variant3))
      this._liquidShaderMat = ((Variant) ref variant3).As<ShaderMaterial>();
    Variant variant4;
    if (info.TryGetProperty(NAeonGlassVfx.PropertyName._baseScrollSpeed, ref variant4))
      this._baseScrollSpeed = ((Variant) ref variant4).As<float>();
    Variant variant5;
    if (info.TryGetProperty(NAeonGlassVfx.PropertyName._parent, ref variant5))
      this._parent = ((Variant) ref variant5).As<Node2D>();
    Variant variant6;
    if (info.TryGetProperty(NAeonGlassVfx.PropertyName._witherParticles, ref variant6))
      this._witherParticles = ((Variant) ref variant6).As<GpuParticles2D>();
    Variant variant7;
    if (info.TryGetProperty(NAeonGlassVfx.PropertyName._leakParticles, ref variant7))
      this._leakParticles = ((Variant) ref variant7).As<GpuParticles2D>();
    Variant variant8;
    if (info.TryGetProperty(NAeonGlassVfx.PropertyName._shardParticles, ref variant8))
      this._shardParticles = ((Variant) ref variant8).As<GpuParticles2D>();
    Variant variant9;
    if (info.TryGetProperty(NAeonGlassVfx.PropertyName._dumpParticles, ref variant9))
      this._dumpParticles = ((Variant) ref variant9).As<GpuParticles2D>();
    Variant variant10;
    if (info.TryGetProperty(NAeonGlassVfx.PropertyName._topSparkParticles, ref variant10))
      this._topSparkParticles = ((Variant) ref variant10).As<GpuParticles2D>();
    Variant variant11;
    if (info.TryGetProperty(NAeonGlassVfx.PropertyName._bottomSparkParticles, ref variant11))
      this._bottomSparkParticles = ((Variant) ref variant11).As<GpuParticles2D>();
    Variant variant12;
    if (info.TryGetProperty(NAeonGlassVfx.PropertyName._groundDustParticles, ref variant12))
      this._groundDustParticles = ((Variant) ref variant12).As<GpuParticles2D>();
    Variant variant13;
    if (!info.TryGetProperty(NAeonGlassVfx.PropertyName._groundChunkParticles, ref variant13))
      return;
    this._groundChunkParticles = ((Variant) ref variant13).As<GpuParticles2D>();
  }

  public class MethodName : Node.MethodName
  {
    public static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName OnAnimationEvent = StringName.op_Implicit(nameof (OnAnimationEvent));
    public static readonly StringName OnAnimationStart = StringName.op_Implicit(nameof (OnAnimationStart));
    public static readonly StringName StartWither = StringName.op_Implicit(nameof (StartWither));
    public static readonly StringName EndWither = StringName.op_Implicit(nameof (EndWither));
    public static readonly StringName StartDie = StringName.op_Implicit(nameof (StartDie));
    public static readonly StringName EndDie = StringName.op_Implicit(nameof (EndDie));
    public static readonly StringName StartSparks = StringName.op_Implicit(nameof (StartSparks));
    public static readonly StringName StartScrape = StringName.op_Implicit(nameof (StartScrape));
    public static readonly StringName EndScrape = StringName.op_Implicit(nameof (EndScrape));
    public static readonly StringName ResetVfx = StringName.op_Implicit(nameof (ResetVfx));
  }

  public class PropertyName : Node.PropertyName
  {
    public static readonly StringName _ringsSpinningNormal = StringName.op_Implicit(nameof (_ringsSpinningNormal));
    public static readonly StringName _curAnimName = StringName.op_Implicit(nameof (_curAnimName));
    public static readonly StringName _liquidShaderMat = StringName.op_Implicit(nameof (_liquidShaderMat));
    public static readonly StringName _baseScrollSpeed = StringName.op_Implicit(nameof (_baseScrollSpeed));
    public static readonly StringName _parent = StringName.op_Implicit(nameof (_parent));
    public static readonly StringName _witherParticles = StringName.op_Implicit(nameof (_witherParticles));
    public static readonly StringName _leakParticles = StringName.op_Implicit(nameof (_leakParticles));
    public static readonly StringName _shardParticles = StringName.op_Implicit(nameof (_shardParticles));
    public static readonly StringName _dumpParticles = StringName.op_Implicit(nameof (_dumpParticles));
    public static readonly StringName _topSparkParticles = StringName.op_Implicit(nameof (_topSparkParticles));
    public static readonly StringName _bottomSparkParticles = StringName.op_Implicit(nameof (_bottomSparkParticles));
    public static readonly StringName _groundDustParticles = StringName.op_Implicit(nameof (_groundDustParticles));
    public static readonly StringName _groundChunkParticles = StringName.op_Implicit(nameof (_groundChunkParticles));
  }

  public class SignalName : Node.SignalName
  {
  }
}
