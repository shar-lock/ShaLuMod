// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Screens.Settings.NResolutionDropdownItem
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using System.Collections.Generic;
using System.ComponentModel;

#nullable disable
namespace MegaCrit.Sts2.Core.Nodes.Screens.Settings;

[ScriptPath("res://src/Core/Nodes/Screens/Settings/NResolutionDropdownItem.cs")]
public class NResolutionDropdownItem : NDropdownItem
{
  public Vector2I resolution;

  public void Init(Vector2I setResolution)
  {
    this.resolution = setResolution;
    this._label.SetTextAutoSize($"{this.resolution.X} x {this.resolution.Y}");
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(1)
    {
      new MethodInfo(NResolutionDropdownItem.MethodName.Init, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 6L, StringName.op_Implicit("setResolution"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (!StringName.op_Equality(ref method, NResolutionDropdownItem.MethodName.Init) || ((NativeVariantPtrArgs) ref args).Count != 1)
      return base.InvokeGodotClassMethod(in method, args, out ret);
    this.Init(VariantUtils.ConvertTo<Vector2I>(ref ((NativeVariantPtrArgs) ref args)[0]));
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NResolutionDropdownItem.MethodName.Init) || base.HasGodotClassMethod(in method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (!StringName.op_Equality(ref name, NResolutionDropdownItem.PropertyName.resolution))
      return base.SetGodotClassPropertyValue(in name, in value);
    this.resolution = VariantUtils.ConvertTo<Vector2I>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (!StringName.op_Equality(ref name, NResolutionDropdownItem.PropertyName.resolution))
      return base.GetGodotClassPropertyValue(in name, out value);
    value = VariantUtils.CreateFrom<Vector2I>(ref this.resolution);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 6L, NResolutionDropdownItem.PropertyName.resolution, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    base.SaveGodotObjectData(info);
    info.AddProperty(NResolutionDropdownItem.PropertyName.resolution, Variant.From<Vector2I>(ref this.resolution));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    base.RestoreGodotObjectData(info);
    Variant variant;
    if (!info.TryGetProperty(NResolutionDropdownItem.PropertyName.resolution, ref variant))
      return;
    this.resolution = ((Variant) ref variant).As<Vector2I>();
  }

  public new class MethodName : NDropdownItem.MethodName
  {
    public static readonly StringName Init = StringName.op_Implicit(nameof (Init));
  }

  public new class PropertyName : NDropdownItem.PropertyName
  {
    public static readonly StringName resolution = StringName.op_Implicit(nameof (resolution));
  }

  public new class SignalName : NDropdownItem.SignalName
  {
  }
}
