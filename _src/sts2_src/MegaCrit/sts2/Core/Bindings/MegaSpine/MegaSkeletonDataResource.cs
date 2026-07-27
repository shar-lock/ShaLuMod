// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Bindings.MegaSpine.MegaSkeletonDataResource
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Collections;
using System.Collections.Generic;

#nullable enable
namespace MegaCrit.Sts2.Core.Bindings.MegaSpine;

public class MegaSkeletonDataResource(Variant native) : MegaSpineBinding(native)
{
  protected override string SpineClassName => "SpineSkeletonDataResource";

  protected override IEnumerable<string> SpineMethods
  {
    get
    {
      return (IEnumerable<string>) new \u003C\u003Ez__ReadOnlyArray<string>(new string[4]
      {
        "find_animation",
        "find_skin",
        "get_animations",
        "get_skins"
      });
    }
  }

  public MegaSkin? FindSkin(string skinName)
  {
    Variant native = this.Call("find_skin", Variant.op_Implicit(skinName));
    return ((Variant) ref native).AsGodotObject() == null ? (MegaSkin) null : new MegaSkin(native);
  }

  public bool HasAnimation(string animName)
  {
    Variant variant = this.Call("find_animation", Variant.op_Implicit(animName));
    return ((Variant) ref variant).AsGodotObject() != null;
  }

  public IReadOnlyList<string> GetAnimationNames()
  {
    Array<GodotObject> array = Array<GodotObject>.op_Explicit(this.Call("get_animations"));
    List<string> animationNames = new List<string>(array.Count);
    foreach (GodotObject godotObject in array)
    {
      MegaAnimation megaAnimation = new MegaAnimation(Variant.op_Implicit(godotObject));
      animationNames.Add(megaAnimation.GetName());
    }
    return (IReadOnlyList<string>) animationNames;
  }

  public Array<GodotObject> GetSkins() => Array<GodotObject>.op_Explicit(this.Call("get_skins"));
}
