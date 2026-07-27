// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Bindings.MegaSpine.MegaSkeleton
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using System.Collections.Generic;

#nullable enable
namespace MegaCrit.Sts2.Core.Bindings.MegaSpine;

public class MegaSkeleton(Variant native) : MegaSpineBinding(native)
{
  protected override string SpineClassName => "SpineSkeleton";

  protected override IEnumerable<string> SpineMethods
  {
    get
    {
      return (IEnumerable<string>) new \u003C\u003Ez__ReadOnlyArray<string>(new string[6]
      {
        "find_bone",
        "get_bounds",
        "get_data",
        "set_skin",
        "set_skin_by_name",
        "set_slots_to_setup_pose"
      });
    }
  }

  public MegaBone? FindBone(string boneName)
  {
    Variant native = this.Call("find_bone", Variant.op_Implicit(boneName));
    return ((Variant) ref native).AsGodotObject() == null ? (MegaBone) null : new MegaBone(native);
  }

  public Rect2 GetBounds()
  {
    Variant variant = this.Call("get_bounds");
    return ((Variant) ref variant).As<Rect2>();
  }

  public MegaSkeletonDataResource GetData() => new MegaSkeletonDataResource(this.Call("get_data"));

  public void SetSkin(MegaSkin? skin)
  {
    if (skin == null)
      return;
    this.Call("set_skin", Variant.op_Implicit(skin.BoundObject));
  }

  public void SetSkinByName(string skinName)
  {
    this.Call("set_skin_by_name", Variant.op_Implicit(skinName));
  }

  public void SetSlotsToSetupPose() => this.Call("set_slots_to_setup_pose");
}
