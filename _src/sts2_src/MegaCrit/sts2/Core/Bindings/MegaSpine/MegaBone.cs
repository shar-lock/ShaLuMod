// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Bindings.MegaSpine.MegaBone
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using System.Collections.Generic;

#nullable enable
namespace MegaCrit.Sts2.Core.Bindings.MegaSpine;

public class MegaBone(Variant native) : MegaSpineBinding(native)
{
  protected override string SpineClassName => "SpineBone";

  protected override IEnumerable<string> SpineMethods
  {
    get
    {
      return (IEnumerable<string>) new \u003C\u003Ez__ReadOnlyArray<string>(new string[4]
      {
        "get_data",
        "set_rotation",
        "set_scale_x",
        "set_scale_y"
      });
    }
  }

  public MegaBoneData GetData() => new MegaBoneData(this.Call("get_data"));

  public void SetRotation(float rotation)
  {
    this.Call("set_rotation", Variant.op_Implicit(rotation));
  }

  public void SetScaleX(float scaleX) => this.Call("set_scale_x", Variant.op_Implicit(scaleX));

  public void SetScaleY(float scaleY) => this.Call("set_scale_y", Variant.op_Implicit(scaleY));

  public void Hide()
  {
    this.SetScaleX(0.0f);
    this.SetScaleY(0.0f);
  }
}
