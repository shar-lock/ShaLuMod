// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Screens.Shops.NFakeMerchantInventory
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

#nullable disable
namespace MegaCrit.Sts2.Core.Nodes.Screens.Shops;

[ScriptPath("res://src/Core/Nodes/Screens/Shops/NFakeMerchantInventory.cs")]
public class NFakeMerchantInventory : NMerchantInventory
{
  protected override void UpdateNavigation()
  {
    Control relicContainer = this._relicContainer;
    List<NMerchantSlot> nmerchantSlotList = (relicContainer != null ? ((IEnumerable) ((Node) relicContainer).GetChildren(false)).OfType<NMerchantSlot>().ToList<NMerchantSlot>() : (List<NMerchantSlot>) null) ?? new List<NMerchantSlot>();
    // ISSUE: object of a compiler-generated type is created
    // ISSUE: object of a compiler-generated type is created
    // ISSUE: object of a compiler-generated type is created
    // ISSUE: object of a compiler-generated type is created
    List<List<NMerchantSlot>> list = new List<List<NMerchantSlot>>((IEnumerable<List<NMerchantSlot>>) new \u003C\u003Ez__ReadOnlyArray<List<NMerchantSlot>>(new List<NMerchantSlot>[3]
    {
      new List<NMerchantSlot>((IEnumerable<NMerchantSlot>) new \u003C\u003Ez__ReadOnlyArray<NMerchantSlot>(new NMerchantSlot[2]
      {
        nmerchantSlotList[0],
        nmerchantSlotList[1]
      })).Where<NMerchantSlot>((Func<NMerchantSlot, bool>) (r => r.Entry.IsStocked)).ToList<NMerchantSlot>(),
      new List<NMerchantSlot>((IEnumerable<NMerchantSlot>) new \u003C\u003Ez__ReadOnlyArray<NMerchantSlot>(new NMerchantSlot[3]
      {
        nmerchantSlotList[2],
        nmerchantSlotList[3],
        nmerchantSlotList[4]
      })).Where<NMerchantSlot>((Func<NMerchantSlot, bool>) (r => r.Entry.IsStocked)).ToList<NMerchantSlot>(),
      new List<NMerchantSlot>((IEnumerable<NMerchantSlot>) new \u003C\u003Ez__ReadOnlySingleElementList<NMerchantSlot>(nmerchantSlotList[5])).Where<NMerchantSlot>((Func<NMerchantSlot, bool>) (r => r.Entry.IsStocked)).ToList<NMerchantSlot>()
    })).Where<List<NMerchantSlot>>((Func<List<NMerchantSlot>, bool>) (r => r.Count > 0)).ToList<List<NMerchantSlot>>();
    for (int index1 = 0; index1 < list.Count; ++index1)
    {
      for (int index2 = 0; index2 < list[index1].Count; ++index2)
      {
        list[index1][index2].FocusNeighborLeft = index2 > 0 ? ((Node) list[index1][index2 - 1]).GetPath() : ((Node) list[index1][index2]).GetPath();
        list[index1][index2].FocusNeighborRight = index2 < list[index1].Count - 1 ? ((Node) list[index1][index2 + 1]).GetPath() : ((Node) list[index1][index2]).GetPath();
        if (index1 > 0)
          list[index1][index2].FocusNeighborTop = index2 < list[index1 - 1].Count ? ((Node) list[index1 - 1][index2]).GetPath() : ((Node) list[index1 - 1][list[index1 - 1].Count - 1]).GetPath();
        else
          list[index1][index2].FocusNeighborTop = ((Node) list[index1][index2]).GetPath();
        if (index1 < list.Count - 1)
          list[index1][index2].FocusNeighborBottom = index2 < list[index1 + 1].Count ? ((Node) list[index1 + 1][index2]).GetPath() : ((Node) list[index1 + 1][list[index1 + 1].Count - 1]).GetPath();
        else
          list[index1][index2].FocusNeighborBottom = ((Node) list[index1][index2]).GetPath();
      }
    }
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(1)
    {
      new MethodInfo(NFakeMerchantInventory.MethodName.UpdateNavigation, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (!StringName.op_Equality(ref method, NFakeMerchantInventory.MethodName.UpdateNavigation) || ((NativeVariantPtrArgs) ref args).Count != 0)
      return base.InvokeGodotClassMethod(in method, args, out ret);
    this.UpdateNavigation();
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NFakeMerchantInventory.MethodName.UpdateNavigation) || base.HasGodotClassMethod(in method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    base.SaveGodotObjectData(info);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    base.RestoreGodotObjectData(info);
  }

  public new class MethodName : NMerchantInventory.MethodName
  {
    public new static readonly StringName UpdateNavigation = StringName.op_Implicit(nameof (UpdateNavigation));
  }

  public new class PropertyName : NMerchantInventory.PropertyName
  {
  }

  public new class SignalName : NMerchantInventory.SignalName
  {
  }
}
