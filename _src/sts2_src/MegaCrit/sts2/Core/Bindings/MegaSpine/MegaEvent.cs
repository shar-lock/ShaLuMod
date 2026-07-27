// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Bindings.MegaSpine.MegaEvent
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using System.Collections.Generic;

#nullable enable
namespace MegaCrit.Sts2.Core.Bindings.MegaSpine;

public class MegaEvent(Variant native) : MegaSpineBinding(native)
{
  protected override string SpineClassName => "SpineEvent";

  protected override IEnumerable<string> SpineMethods
  {
    get => (IEnumerable<string>) new \u003C\u003Ez__ReadOnlySingleElementList<string>("get_data");
  }

  public MegaEventData GetData() => new MegaEventData(this.Call("get_data"));
}
