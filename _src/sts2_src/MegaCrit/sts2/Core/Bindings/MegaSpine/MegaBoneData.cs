// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Bindings.MegaSpine.MegaBoneData
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using System.Collections.Generic;

#nullable enable
namespace MegaCrit.Sts2.Core.Bindings.MegaSpine;

public class MegaBoneData(Variant native) : MegaSpineBinding(native)
{
  protected override string SpineClassName => "SpineBoneData";

  protected override IEnumerable<string> SpineMethods
  {
    get => (IEnumerable<string>) new \u003C\u003Ez__ReadOnlySingleElementList<string>("set_color");
  }

  public void SetColor(Color color) => this.Call("set_color", Variant.op_Implicit(color));
}
