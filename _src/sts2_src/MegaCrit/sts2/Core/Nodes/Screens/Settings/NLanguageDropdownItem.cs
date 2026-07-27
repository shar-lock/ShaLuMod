// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Screens.Settings.NLanguageDropdownItem
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using System.Collections.Generic;
using System.ComponentModel;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Screens.Settings;

[ScriptPath("res://src/Core/Nodes/Screens/Settings/NLanguageDropdownItem.cs")]
public class NLanguageDropdownItem : NDropdownItem
{
  public const string languageWarningIconPath = "res://images/ui/language_warning.png";
  private const string _warnImageTag = "[img]res://images/ui/language_warning.png[/img]";

  public string LanguageCode { get; private set; }

  public void Init(string languageCode)
  {
    this.LanguageCode = languageCode;
    string text = NLanguageDropdown.GetLanguageNameForCode(languageCode);
    if ((double) LocManager.Instance.GetLanguageCompletion(languageCode) < 0.89999997615814209)
      text = "[img]res://images/ui/language_warning.png[/img]" + text;
    this._richLabel.SetTextAutoSize(text);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(1)
    {
      new MethodInfo(NLanguageDropdownItem.MethodName.Init, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 4L, StringName.op_Implicit("languageCode"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (!StringName.op_Equality(ref method, NLanguageDropdownItem.MethodName.Init) || ((NativeVariantPtrArgs) ref args).Count != 1)
      return base.InvokeGodotClassMethod(in method, args, out ret);
    this.Init(VariantUtils.ConvertTo<string>(ref ((NativeVariantPtrArgs) ref args)[0]));
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NLanguageDropdownItem.MethodName.Init) || base.HasGodotClassMethod(in method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (!StringName.op_Equality(ref name, NLanguageDropdownItem.PropertyName.LanguageCode))
      return base.SetGodotClassPropertyValue(in name, in value);
    this.LanguageCode = VariantUtils.ConvertTo<string>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (!StringName.op_Equality(ref name, NLanguageDropdownItem.PropertyName.LanguageCode))
      return base.GetGodotClassPropertyValue(in name, out value);
    ref godot_variant local = ref value;
    string languageCode = this.LanguageCode;
    godot_variant from = VariantUtils.CreateFrom<string>(ref languageCode);
    local = from;
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 4L, NLanguageDropdownItem.PropertyName.LanguageCode, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    base.SaveGodotObjectData(info);
    GodotSerializationInfo serializationInfo = info;
    StringName languageCode1 = NLanguageDropdownItem.PropertyName.LanguageCode;
    string languageCode2 = this.LanguageCode;
    Variant variant = Variant.From<string>(ref languageCode2);
    serializationInfo.AddProperty(languageCode1, variant);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    base.RestoreGodotObjectData(info);
    Variant variant;
    if (!info.TryGetProperty(NLanguageDropdownItem.PropertyName.LanguageCode, ref variant))
      return;
    this.LanguageCode = ((Variant) ref variant).As<string>();
  }

  public new class MethodName : NDropdownItem.MethodName
  {
    public static readonly StringName Init = StringName.op_Implicit(nameof (Init));
  }

  public new class PropertyName : NDropdownItem.PropertyName
  {
    public static readonly StringName LanguageCode = StringName.op_Implicit(nameof (LanguageCode));
  }

  public new class SignalName : NDropdownItem.SignalName
  {
  }
}
