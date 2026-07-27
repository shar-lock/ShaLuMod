// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Bindings.MegaSpine.MegaAnimationState
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using System.Collections.Generic;

#nullable enable
namespace MegaCrit.Sts2.Core.Bindings.MegaSpine;

public class MegaAnimationState(Variant native) : MegaSpineBinding(native)
{
  protected override string SpineClassName => "SpineAnimationState";

  protected override IEnumerable<string> SpineMethods
  {
    get
    {
      return (IEnumerable<string>) new \u003C\u003Ez__ReadOnlyArray<string>(new string[7]
      {
        "add_animation",
        "add_empty_animation",
        "apply",
        "get_current",
        "set_animation",
        "set_time_scale",
        "update"
      });
    }
  }

  public void AddAnimation(string animationName, float delay = 0.0f, bool loop = true, int trackId = 0)
  {
    using (this.Call("add_animation", Variant.op_Implicit(animationName), Variant.op_Implicit(delay), Variant.op_Implicit(loop), Variant.op_Implicit(trackId)))
      ;
  }

  public MegaTrackEntry AddAnimationTracked(
    string animationName,
    float delay = 0.0f,
    bool loop = true,
    int trackId = 0)
  {
    using (Variant native = this.Call("add_animation", Variant.op_Implicit(animationName), Variant.op_Implicit(delay), Variant.op_Implicit(loop), Variant.op_Implicit(trackId)))
      return new MegaTrackEntry(native);
  }

  public void Apply(MegaSkeleton skeleton)
  {
    this.Call("apply", Variant.op_Implicit(skeleton.BoundObject));
  }

  public MegaTrackEntry? GetCurrent(int trackIndex)
  {
    using (Variant native = this.Call("get_current", Variant.op_Implicit(trackIndex)))
      return ((Variant) ref native).VariantType != 24L ? (MegaTrackEntry) null : new MegaTrackEntry(native);
  }

  public string? GetCurrentAnimationName(int trackIndex = 0)
  {
    using (MegaTrackEntry current = this.GetCurrent(trackIndex))
      return current?.GetAnimationName();
  }

  public float? GetCurrentAnimationDuration(int trackIndex = 0)
  {
    using (MegaTrackEntry current = this.GetCurrent(trackIndex))
      return current?.GetAnimationDuration();
  }

  public void SetAnimation(string animationName, bool loop = true, int trackId = 0)
  {
    using (this.Call("set_animation", Variant.op_Implicit(animationName), Variant.op_Implicit(loop), Variant.op_Implicit(trackId)))
      ;
  }

  public void AddEmptyAnimation(int trackId = 0)
  {
    using (this.Call("add_empty_animation", Variant.op_Implicit(trackId), Variant.op_Implicit(0), Variant.op_Implicit(0)))
      ;
  }

  public void SetTimeScale(float scale) => this.Call("set_time_scale", Variant.op_Implicit(scale));

  public void Update(float delta) => this.Call("update", Variant.op_Implicit(delta));
}
