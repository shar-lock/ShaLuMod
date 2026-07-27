// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Screens.FeedbackScreen.NFeedbackCategoryDropdownItem
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using System.Collections.Generic;
using System.ComponentModel;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Screens.FeedbackScreen;

[ScriptPath("res://src/Core/Nodes/Screens/FeedbackScreen/NFeedbackCategoryDropdownItem.cs")]
public class NFeedbackCategoryDropdownItem : NDropdownItem
{
  public int CategoryIndex { get; private set; }

  public void Init(int categoryIndex, string localizedCategory)
  {
    this.CategoryIndex = categoryIndex;
    this.Text = localizedCategory;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(1)
    {
      new MethodInfo(NFeedbackCategoryDropdownItem.MethodName.Init, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 2L, StringName.op_Implicit("categoryIndex"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false),
        new PropertyInfo((Variant.Type) 4L, StringName.op_Implicit("localizedCategory"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (!StringName.op_Equality(ref method, NFeedbackCategoryDropdownItem.MethodName.Init) || ((NativeVariantPtrArgs) ref args).Count != 2)
      return base.InvokeGodotClassMethod(in method, args, out ret);
    this.Init(VariantUtils.ConvertTo<int>(ref ((NativeVariantPtrArgs) ref args)[0]), VariantUtils.ConvertTo<string>(ref ((NativeVariantPtrArgs) ref args)[1]));
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NFeedbackCategoryDropdownItem.MethodName.Init) || base.HasGodotClassMethod(in method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (!StringName.op_Equality(ref name, NFeedbackCategoryDropdownItem.PropertyName.CategoryIndex))
      return base.SetGodotClassPropertyValue(in name, in value);
    this.CategoryIndex = VariantUtils.ConvertTo<int>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (!StringName.op_Equality(ref name, NFeedbackCategoryDropdownItem.PropertyName.CategoryIndex))
      return base.GetGodotClassPropertyValue(in name, out value);
    ref godot_variant local = ref value;
    int categoryIndex = this.CategoryIndex;
    godot_variant from = VariantUtils.CreateFrom<int>(ref categoryIndex);
    local = from;
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 2L, NFeedbackCategoryDropdownItem.PropertyName.CategoryIndex, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    base.SaveGodotObjectData(info);
    GodotSerializationInfo serializationInfo = info;
    StringName categoryIndex1 = NFeedbackCategoryDropdownItem.PropertyName.CategoryIndex;
    int categoryIndex2 = this.CategoryIndex;
    Variant variant = Variant.From<int>(ref categoryIndex2);
    serializationInfo.AddProperty(categoryIndex1, variant);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    base.RestoreGodotObjectData(info);
    Variant variant;
    if (!info.TryGetProperty(NFeedbackCategoryDropdownItem.PropertyName.CategoryIndex, ref variant))
      return;
    this.CategoryIndex = ((Variant) ref variant).As<int>();
  }

  public new class MethodName : NDropdownItem.MethodName
  {
    public static readonly StringName Init = StringName.op_Implicit(nameof (Init));
  }

  public new class PropertyName : NDropdownItem.PropertyName
  {
    public static readonly StringName CategoryIndex = StringName.op_Implicit(nameof (CategoryIndex));
  }

  public new class SignalName : NDropdownItem.SignalName
  {
  }
}
