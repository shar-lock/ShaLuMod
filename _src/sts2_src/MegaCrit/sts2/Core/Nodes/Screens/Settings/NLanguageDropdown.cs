// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Screens.Settings.NLanguageDropdown
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Saves;
using System;
using System.Collections.Generic;
using System.ComponentModel;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Screens.Settings;

[ScriptPath("res://src/Core/Nodes/Screens/Settings/NLanguageDropdown.cs")]
public class NLanguageDropdown : NSettingsDropdown
{
  [Export]
  private PackedScene _dropdownItemScene;
  private static readonly Dictionary<string, string> _languageCodeToName = new Dictionary<string, string>()
  {
    {
      "ARA",
      "العربية"
    },
    {
      "BEN",
      "বাংলা"
    },
    {
      "CZE",
      "Čeština"
    },
    {
      "DEU",
      "Deutsch"
    },
    {
      "DUT",
      "Nederlands"
    },
    {
      "ENG",
      "English"
    },
    {
      "ESP",
      "Español (Latinoamérica)"
    },
    {
      "FIL",
      "Filipino"
    },
    {
      "FIN",
      "Suomi"
    },
    {
      "FRA",
      "Français"
    },
    {
      "GRE",
      "Ελληνικά"
    },
    {
      "HIN",
      "हिन्दी"
    },
    {
      "IND",
      "Bahasa Indonesia"
    },
    {
      "ITA",
      "Italiano"
    },
    {
      "JPN",
      "日本語"
    },
    {
      "KOR",
      "한국어"
    },
    {
      "MAL",
      "Bahasa Melayu"
    },
    {
      "NOR",
      "Norsk"
    },
    {
      "POL",
      "Polski"
    },
    {
      "POR",
      "Português"
    },
    {
      "PTB",
      "Português Brasileiro"
    },
    {
      "RUS",
      "Русский"
    },
    {
      "SPA",
      "Español (Castellano)"
    },
    {
      "SWE",
      "Svenska"
    },
    {
      "THA",
      "ไทย"
    },
    {
      "TUR",
      "Türkçe"
    },
    {
      "UKR",
      "Українська"
    },
    {
      "VIE",
      "Tiếng Việt"
    },
    {
      "ZHS",
      "中文"
    },
    {
      "ZHT",
      "繁體中文"
    }
  };

  private string CurrentLanguage => LocManager.Instance.Language;

  public override void _Ready()
  {
    this.ConnectSignals();
    this.PopulateOptions();
    this._currentOptionLabel.SetTextAutoSize(NLanguageDropdown.GetLanguageNameForCode(this.CurrentLanguage));
  }

  private void PopulateOptions()
  {
    this.ClearDropdownItems();
    foreach (string language in LocManager.Languages)
    {
      NLanguageDropdownItem child = this._dropdownItemScene.Instantiate<NLanguageDropdownItem>((PackedScene.GenEditState) 0L);
      ((Node) this._dropdownItems).AddChildSafely((Node) child);
      ((GodotObject) child).Connect(NDropdownItem.SignalName.Selected, Callable.From<NDropdownItem>(new Action<NDropdownItem>(this.OnDropdownItemSelected)), 0U);
      child.Init(language);
    }
    ((Node) this._dropdownItems).GetParent<NDropdownContainer>().RefreshLayout();
  }

  private void OnDropdownItemSelected(NDropdownItem nDropdownItem)
  {
    NLanguageDropdownItem nlanguageDropdownItem = (NLanguageDropdownItem) nDropdownItem;
    if (nlanguageDropdownItem.LanguageCode == this.CurrentLanguage)
      return;
    this.CloseDropdown();
    this._currentOptionLabel.SetTextAutoSize(NLanguageDropdown.GetLanguageNameForCode(nlanguageDropdownItem.LanguageCode));
    SaveManager.Instance.SettingsSave.Language = nlanguageDropdownItem.LanguageCode;
    LocManager.Instance.SetLanguage(nlanguageDropdownItem.LanguageCode);
    NGame.Instance.Relocalize();
    NGame.Instance.MainMenu.OpenSettingsMenu();
  }

  public static string GetLanguageNameForCode(string languageCode)
  {
    string languageNameForCode;
    if (!NLanguageDropdown._languageCodeToName.TryGetValue(languageCode.ToUpperInvariant(), out languageNameForCode))
      throw new InvalidOperationException($"Tried to get language name for code {languageCode} but it doesn't exist!");
    return languageNameForCode;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(4)
    {
      new MethodInfo(NLanguageDropdown.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NLanguageDropdown.MethodName.PopulateOptions, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NLanguageDropdown.MethodName.OnDropdownItemSelected, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("nDropdownItem"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false)
      }, (List<Variant>) null),
      new MethodInfo(NLanguageDropdown.MethodName.GetLanguageNameForCode, new PropertyInfo((Variant.Type) 4L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 33L, new List<PropertyInfo>()
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
    if (StringName.op_Equality(ref method, NLanguageDropdown.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NLanguageDropdown.MethodName.PopulateOptions) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.PopulateOptions();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NLanguageDropdown.MethodName.OnDropdownItemSelected) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.OnDropdownItemSelected(VariantUtils.ConvertTo<NDropdownItem>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NLanguageDropdown.MethodName.GetLanguageNameForCode) || ((NativeVariantPtrArgs) ref args).Count != 1)
      return base.InvokeGodotClassMethod(in method, args, out ret);
    string languageNameForCode = NLanguageDropdown.GetLanguageNameForCode(VariantUtils.ConvertTo<string>(ref ((NativeVariantPtrArgs) ref args)[0]));
    ret = VariantUtils.CreateFrom<string>(ref languageNameForCode);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static bool InvokeGodotClassStaticMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NLanguageDropdown.MethodName.GetLanguageNameForCode) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      string languageNameForCode = NLanguageDropdown.GetLanguageNameForCode(VariantUtils.ConvertTo<string>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = VariantUtils.CreateFrom<string>(ref languageNameForCode);
      return true;
    }
    ret = new godot_variant();
    return false;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NLanguageDropdown.MethodName._Ready) || StringName.op_Equality(ref method, NLanguageDropdown.MethodName.PopulateOptions) || StringName.op_Equality(ref method, NLanguageDropdown.MethodName.OnDropdownItemSelected) || StringName.op_Equality(ref method, NLanguageDropdown.MethodName.GetLanguageNameForCode) || base.HasGodotClassMethod(in method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (!StringName.op_Equality(ref name, NLanguageDropdown.PropertyName._dropdownItemScene))
      return base.SetGodotClassPropertyValue(in name, in value);
    this._dropdownItemScene = VariantUtils.ConvertTo<PackedScene>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NLanguageDropdown.PropertyName.CurrentLanguage))
    {
      ref godot_variant local = ref value;
      string currentLanguage = this.CurrentLanguage;
      godot_variant from = VariantUtils.CreateFrom<string>(ref currentLanguage);
      local = from;
      return true;
    }
    if (!StringName.op_Equality(ref name, NLanguageDropdown.PropertyName._dropdownItemScene))
      return base.GetGodotClassPropertyValue(in name, out value);
    value = VariantUtils.CreateFrom<PackedScene>(ref this._dropdownItemScene);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NLanguageDropdown.PropertyName._dropdownItemScene, (PropertyHint) 17L, "PackedScene", (PropertyUsageFlags) 4102L, true),
      new PropertyInfo((Variant.Type) 4L, NLanguageDropdown.PropertyName.CurrentLanguage, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    base.SaveGodotObjectData(info);
    info.AddProperty(NLanguageDropdown.PropertyName._dropdownItemScene, Variant.From<PackedScene>(ref this._dropdownItemScene));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    base.RestoreGodotObjectData(info);
    Variant variant;
    if (!info.TryGetProperty(NLanguageDropdown.PropertyName._dropdownItemScene, ref variant))
      return;
    this._dropdownItemScene = ((Variant) ref variant).As<PackedScene>();
  }

  public new class MethodName : NSettingsDropdown.MethodName
  {
    public new static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName PopulateOptions = StringName.op_Implicit(nameof (PopulateOptions));
    public static readonly StringName OnDropdownItemSelected = StringName.op_Implicit(nameof (OnDropdownItemSelected));
    public static readonly StringName GetLanguageNameForCode = StringName.op_Implicit(nameof (GetLanguageNameForCode));
  }

  public new class PropertyName : NSettingsDropdown.PropertyName
  {
    public static readonly StringName CurrentLanguage = StringName.op_Implicit(nameof (CurrentLanguage));
    public static readonly StringName _dropdownItemScene = StringName.op_Implicit(nameof (_dropdownItemScene));
  }

  public new class SignalName : NSettingsDropdown.SignalName
  {
  }
}
