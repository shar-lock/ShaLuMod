// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Localization.LocTextLabel
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using System;
using System.Collections.Generic;
using System.ComponentModel;

#nullable enable
namespace MegaCrit.Sts2.Core.Localization;

[ScriptPath("res://src/Core/Localization/LocTextLabel.cs")]
public class LocTextLabel : RichTextLabel
{
  [Export]
  private string? _localizationTable;
  [Export]
  private string? _localizationKey;
  private LocString? _locString;

  public string? LocalizationTable
  {
    get => this._localizationTable;
    set
    {
      if (this._localizationTable == value)
        return;
      this._localizationTable = value;
      this._locString = (LocString) null;
      this.UpdateLocalization();
    }
  }

  public string? LocalizationKey
  {
    get => this._localizationKey;
    set
    {
      if (this._localizationKey == value)
        return;
      this._localizationKey = value;
      this._locString = (LocString) null;
      this.UpdateLocalization();
    }
  }

  private void UpdateLocalization()
  {
    if (this._localizationTable == null)
      throw new InvalidOperationException("_localizationTable is null.");
    if (this._localizationKey == null)
      throw new InvalidOperationException("_localizationKey is null.");
    if (this._locString == null)
      this._locString = new LocString(this._localizationTable, this._localizationKey);
    this.Text = this._locString.GetFormattedText();
  }

  public override void _Ready() => this.UpdateLocalization();

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(2)
    {
      new MethodInfo(LocTextLabel.MethodName.UpdateLocalization, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(LocTextLabel.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, LocTextLabel.MethodName.UpdateLocalization) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.UpdateLocalization();
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, LocTextLabel.MethodName._Ready) || ((NativeVariantPtrArgs) ref args).Count != 0)
      return base.InvokeGodotClassMethod(ref method, args, ref ret);
    ((Node) this)._Ready();
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, LocTextLabel.MethodName.UpdateLocalization) || StringName.op_Equality(ref method, LocTextLabel.MethodName._Ready) || base.HasGodotClassMethod(ref method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, LocTextLabel.PropertyName.LocalizationTable))
    {
      this.LocalizationTable = VariantUtils.ConvertTo<string>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, LocTextLabel.PropertyName.LocalizationKey))
    {
      this.LocalizationKey = VariantUtils.ConvertTo<string>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, LocTextLabel.PropertyName._localizationTable))
    {
      this._localizationTable = VariantUtils.ConvertTo<string>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, LocTextLabel.PropertyName._localizationKey))
      return ((GodotObject) this).SetGodotClassPropertyValue(ref name, ref value);
    this._localizationKey = VariantUtils.ConvertTo<string>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, LocTextLabel.PropertyName.LocalizationTable))
    {
      ref godot_variant local = ref value;
      string localizationTable = this.LocalizationTable;
      godot_variant from = VariantUtils.CreateFrom<string>(ref localizationTable);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, LocTextLabel.PropertyName.LocalizationKey))
    {
      ref godot_variant local = ref value;
      string localizationKey = this.LocalizationKey;
      godot_variant from = VariantUtils.CreateFrom<string>(ref localizationKey);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, LocTextLabel.PropertyName._localizationTable))
    {
      value = VariantUtils.CreateFrom<string>(ref this._localizationTable);
      return true;
    }
    if (!StringName.op_Equality(ref name, LocTextLabel.PropertyName._localizationKey))
      return ((GodotObject) this).GetGodotClassPropertyValue(ref name, ref value);
    value = VariantUtils.CreateFrom<string>(ref this._localizationKey);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 4L, LocTextLabel.PropertyName._localizationTable, (PropertyHint) 0L, "", (PropertyUsageFlags) 4102L, true),
      new PropertyInfo((Variant.Type) 4L, LocTextLabel.PropertyName._localizationKey, (PropertyHint) 0L, "", (PropertyUsageFlags) 4102L, true),
      new PropertyInfo((Variant.Type) 4L, LocTextLabel.PropertyName.LocalizationTable, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 4L, LocTextLabel.PropertyName.LocalizationKey, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).SaveGodotObjectData(info);
    GodotSerializationInfo serializationInfo1 = info;
    StringName localizationTable1 = LocTextLabel.PropertyName.LocalizationTable;
    string localizationTable2 = this.LocalizationTable;
    Variant variant1 = Variant.From<string>(ref localizationTable2);
    serializationInfo1.AddProperty(localizationTable1, variant1);
    GodotSerializationInfo serializationInfo2 = info;
    StringName localizationKey1 = LocTextLabel.PropertyName.LocalizationKey;
    string localizationKey2 = this.LocalizationKey;
    Variant variant2 = Variant.From<string>(ref localizationKey2);
    serializationInfo2.AddProperty(localizationKey1, variant2);
    info.AddProperty(LocTextLabel.PropertyName._localizationTable, Variant.From<string>(ref this._localizationTable));
    info.AddProperty(LocTextLabel.PropertyName._localizationKey, Variant.From<string>(ref this._localizationKey));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(LocTextLabel.PropertyName.LocalizationTable, ref variant1))
      this.LocalizationTable = ((Variant) ref variant1).As<string>();
    Variant variant2;
    if (info.TryGetProperty(LocTextLabel.PropertyName.LocalizationKey, ref variant2))
      this.LocalizationKey = ((Variant) ref variant2).As<string>();
    Variant variant3;
    if (info.TryGetProperty(LocTextLabel.PropertyName._localizationTable, ref variant3))
      this._localizationTable = ((Variant) ref variant3).As<string>();
    Variant variant4;
    if (!info.TryGetProperty(LocTextLabel.PropertyName._localizationKey, ref variant4))
      return;
    this._localizationKey = ((Variant) ref variant4).As<string>();
  }

  public class MethodName : RichTextLabel.MethodName
  {
    public static readonly StringName UpdateLocalization = StringName.op_Implicit(nameof (UpdateLocalization));
    public static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
  }

  public class PropertyName : RichTextLabel.PropertyName
  {
    public static readonly StringName LocalizationTable = StringName.op_Implicit(nameof (LocalizationTable));
    public static readonly StringName LocalizationKey = StringName.op_Implicit(nameof (LocalizationKey));
    public static readonly StringName _localizationTable = StringName.op_Implicit(nameof (_localizationTable));
    public static readonly StringName _localizationKey = StringName.op_Implicit(nameof (_localizationKey));
  }

  public class SignalName : RichTextLabel.SignalName
  {
  }
}
