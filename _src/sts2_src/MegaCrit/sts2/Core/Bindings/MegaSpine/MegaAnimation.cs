// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Bindings.MegaSpine.MegaAnimation
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using System.Collections.Generic;

#nullable enable
namespace MegaCrit.Sts2.Core.Bindings.MegaSpine;

public class MegaAnimation(Variant native) : MegaSpineBinding(native)
{
  protected override string SpineClassName => "SpineAnimation";

  protected override IEnumerable<string> SpineMethods
  {
    get
    {
      return (IEnumerable<string>) new \u003C\u003Ez__ReadOnlyArray<string>(new string[2]
      {
        "get_name",
        "get_duration"
      });
    }
  }

  public string GetName()
  {
    Variant variant = this.Call("get_name");
    return ((Variant) ref variant).AsString();
  }

  public float GetDuration()
  {
    Variant variant = this.Call("get_duration");
    return ((Variant) ref variant).AsSingle();
  }
}
