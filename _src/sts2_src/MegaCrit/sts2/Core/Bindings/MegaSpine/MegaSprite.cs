// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Bindings.MegaSpine.MegaSprite
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using System;
using System.Collections.Generic;

#nullable enable
namespace MegaCrit.Sts2.Core.Bindings.MegaSpine;

public class MegaSprite(Variant native) : MegaSpineBinding(native)
{
  public const string spineClassName = "SpineSprite";

  protected override string SpineClassName => "SpineSprite";

  protected override IEnumerable<string> SpineMethods
  {
    get
    {
      return (IEnumerable<string>) new \u003C\u003Ez__ReadOnlyArray<string>(new string[7]
      {
        "get_animation_state",
        "get_additive_material",
        "get_normal_material",
        "get_skeleton",
        "new_skin",
        "set_normal_material",
        "set_skeleton_data_res"
      });
    }
  }

  protected override IEnumerable<string> SpineSignals
  {
    get
    {
      return (IEnumerable<string>) new \u003C\u003Ez__ReadOnlyArray<string>(new string[10]
      {
        "animation_started",
        "animation_interrupted",
        "animation_ended",
        "animation_completed",
        "animation_disposed",
        "animation_event",
        "before_animation_state_update",
        "before_animation_state_apply",
        "before_world_transforms_change",
        "world_transforms_changed"
      });
    }
  }

  public Error ConnectAnimationStarted(Callable callable)
  {
    return this.Connect("animation_started", callable);
  }

  public Error ConnectAnimationInterrupted(Callable callable)
  {
    return this.Connect("animation_interrupted", callable);
  }

  public Error ConnectAnimationEnded(Callable callable)
  {
    return this.Connect("animation_ended", callable);
  }

  public Error ConnectAnimationCompleted(Callable callable)
  {
    return this.Connect("animation_completed", callable);
  }

  public Error ConnectAnimationDisposed(Callable callable)
  {
    return this.Connect("animation_disposed", callable);
  }

  public Error ConnectAnimationEvent(Callable callable)
  {
    return this.Connect("animation_event", callable);
  }

  public Error ConnectBeforeAnimationStateUpdate(Callable callable)
  {
    return this.Connect("before_animation_state_update", callable);
  }

  public Error ConnectBeforeAnimationStateApply(Callable callable)
  {
    return this.Connect("before_animation_state_apply", callable);
  }

  public Error ConnectBeforeWorldTransformsChange(Callable callable)
  {
    return this.Connect("before_world_transforms_change", callable);
  }

  public Error ConnectWorldTransformsChanged(Callable callable)
  {
    return this.Connect("world_transforms_changed", callable);
  }

  public void DisconnectAnimationStarted(Callable callable)
  {
    this.Disconnect("animation_started", callable);
  }

  public void DisconnectAnimationInterrupted(Callable callable)
  {
    this.Disconnect("animation_interrupted", callable);
  }

  public void DisconnectAnimationEnded(Callable callable)
  {
    this.Disconnect("animation_ended", callable);
  }

  public void DisconnectAnimationCompleted(Callable callable)
  {
    this.Disconnect("animation_completed", callable);
  }

  public void DisconnectAnimationDisposed(Callable callable)
  {
    this.Disconnect("animation_disposed", callable);
  }

  public void DisconnectAnimationEvent(Callable callable)
  {
    this.Disconnect("animation_event", callable);
  }

  public void DisconnectBeforeAnimationStateUpdate(Callable callable)
  {
    this.Disconnect("before_animation_state_update", callable);
  }

  public void DisconnectBeforeAnimationStateApply(Callable callable)
  {
    this.Disconnect("before_animation_state_apply", callable);
  }

  public void DisconnectBeforeWorldTransformsChange(Callable callable)
  {
    this.Disconnect("before_world_transforms_change", callable);
  }

  public void DisconnectWorldTransformsChanged(Callable callable)
  {
    this.Disconnect("world_transforms_changed", callable);
  }

  public bool HasAnimation(string animId)
  {
    MegaSkeleton skeleton = this.GetSkeleton();
    return skeleton != null && skeleton.GetData().HasAnimation(animId);
  }

  public MegaAnimationState GetAnimationState()
  {
    return this.TryGetAnimationState() ?? throw new InvalidOperationException("GetAnimationState() was called before the SpineSprite's skeleton finished initializing. Godot runs _Ready() bottom-up and the skeleton loads asynchronously; drive animations from a spine-ready callback (Node.RunWhenSpineReady) or gate on IsAnimationStateReady()/TryGetAnimationState().");
  }

  public MegaAnimationState? TryGetAnimationState()
  {
    Variant native = this.Call("get_animation_state");
    return ((Variant) ref native).VariantType != 24L || ((Variant) ref native).AsGodotObject() == null ? (MegaAnimationState) null : new MegaAnimationState(native);
  }

  public bool IsAnimationStateReady()
  {
    return this.GetSkeleton() != null && this.TryGetAnimationState() != null;
  }

  public MegaSkeleton? GetSkeleton()
  {
    Variant? nullable = this.CallNullable("get_skeleton");
    return !nullable.HasValue ? (MegaSkeleton) null : new MegaSkeleton(nullable.Value);
  }

  public Material? GetAdditiveMaterial()
  {
    Variant? nullable = this.CallNullable("get_additive_material");
    ref Variant? local = ref nullable;
    if (!local.HasValue)
      return (Material) null;
    Variant valueOrDefault = local.GetValueOrDefault();
    return ((Variant) ref valueOrDefault).As<Material>();
  }

  public Material? GetNormalMaterial()
  {
    Variant? nullable = this.CallNullable("get_normal_material");
    ref Variant? local = ref nullable;
    if (!local.HasValue)
      return (Material) null;
    Variant valueOrDefault = local.GetValueOrDefault();
    return ((Variant) ref valueOrDefault).As<Material>();
  }

  public MegaSkin NewSkin(string name)
  {
    return new MegaSkin(this.Call("new_skin", Variant.op_Implicit(name)));
  }

  public void SetNormalMaterial(Material material)
  {
    this.Call("set_normal_material", Variant.op_Implicit((GodotObject) material));
  }

  public void SetSkeletonDataRes(MegaSkeletonDataResource skeletonData)
  {
    this.Call("set_skeleton_data_res", Variant.op_Implicit(skeletonData.BoundObject));
  }
}
