// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Bindings.MegaSpine.MegaSlotNode
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using System.Collections.Generic;

#nullable enable
namespace MegaCrit.Sts2.Core.Bindings.MegaSpine;

public class MegaSlotNode(Variant native) : MegaSpineBinding(native)
{
  protected override string SpineClassName => "SpineSlotNode";

  protected override IEnumerable<string> SpineMethods
  {
    get
    {
      return (IEnumerable<string>) new \u003C\u003Ez__ReadOnlySingleElementList<string>("get_normal_material");
    }
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
}
