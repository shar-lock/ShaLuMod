// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Bindings.MegaSpine.MegaTrackEntry
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using System.Collections.Generic;

#nullable enable
namespace MegaCrit.Sts2.Core.Bindings.MegaSpine;

public class MegaTrackEntry(Variant native) : MegaSpineBinding(native)
{
  protected override string SpineClassName => "SpineTrackEntry";

  protected override IEnumerable<string> SpineMethods
  {
    get
    {
      return (IEnumerable<string>) new \u003C\u003Ez__ReadOnlyArray<string>(new string[9]
      {
        "get_animation",
        "get_animation_end",
        "get_track_complete",
        "get_track_time",
        "is_complete",
        "set_loop",
        "set_time_scale",
        "set_track_time",
        "set_mix_duration"
      });
    }
  }

  private MegaAnimation GetAnimation()
  {
    using (Variant native = this.Call("get_animation"))
      return new MegaAnimation(native);
  }

  public string GetAnimationName()
  {
    using (MegaAnimation animation = this.GetAnimation())
      return animation.GetName();
  }

  public float GetAnimationDuration()
  {
    using (MegaAnimation animation = this.GetAnimation())
      return animation.GetDuration();
  }

  public float GetAnimationEnd()
  {
    Variant variant = this.Call("get_animation_end");
    return ((Variant) ref variant).AsSingle();
  }

  public float GetTrackComplete()
  {
    Variant variant = this.Call("get_track_complete");
    return ((Variant) ref variant).AsSingle();
  }

  public float GetTrackTime()
  {
    Variant variant = this.Call("get_track_time");
    return ((Variant) ref variant).AsSingle();
  }

  public bool IsComplete()
  {
    Variant variant = this.Call("is_complete");
    return ((Variant) ref variant).AsBool();
  }

  public void SetLoop(bool loop) => this.Call("set_loop", Variant.op_Implicit(loop));

  public void SetTimeScale(float scale) => this.Call("set_time_scale", Variant.op_Implicit(scale));

  public void SetTrackTime(float time) => this.Call("set_track_time", Variant.op_Implicit(time));

  public void SetMixDuration(float time)
  {
    this.Call("set_mix_duration", Variant.op_Implicit(time));
  }
}
