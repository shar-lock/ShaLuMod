// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Localization.LocManager
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using MegaCrit.Sts2.Core.Debug;
using MegaCrit.Sts2.Core.Localization.Formatters;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Modding;
using MegaCrit.Sts2.Core.Platform;
using MegaCrit.Sts2.Core.Saves;
using MegaCrit.Sts2.Core.TestSupport;
using Sentry;
using SmartFormat;
using SmartFormat.Core.Extensions;
using SmartFormat.Core.Formatting;
using SmartFormat.Core.Parsing;
using SmartFormat.Core.Settings;
using SmartFormat.Extensions;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text.Json;
using System.Text.RegularExpressions;

#nullable enable
namespace MegaCrit.Sts2.Core.Localization;

public class LocManager
{
  private Dictionary<string, LocTable> _tables = new Dictionary<string, LocTable>();
  private Dictionary<string, LocTable>? _engTables;
  private LocManager.PreOverrideState? _stateBeforeOverridingWithEnglish;
  private static SmartFormatter _smartFormatter = (SmartFormatter) null;
  private const string _weblateProjectSlug = "slaythespire2";
  private static readonly Dictionary<string, string> _weblateToGameLanguage = new Dictionary<string, string>()
  {
    {
      "de",
      "deu"
    },
    {
      "es",
      "spa"
    },
    {
      "es_LATAM",
      "esp"
    },
    {
      "fr",
      "fra"
    },
    {
      "it",
      "ita"
    },
    {
      "ja",
      "jpn"
    },
    {
      "ko",
      "kor"
    },
    {
      "pl",
      "pol"
    },
    {
      "pt_BR",
      "ptb"
    },
    {
      "ru",
      "rus"
    },
    {
      "th",
      "tha"
    },
    {
      "tr",
      "tur"
    },
    {
      "zh_Hans",
      "zhs"
    },
    {
      "zh_Hant",
      "zht"
    }
  };
  private static readonly Dictionary<string, string> _gameToWeblateLanguage = LocManager._weblateToGameLanguage.ToDictionary<KeyValuePair<string, string>, string, string>((Func<KeyValuePair<string, string>, string>) (kvp => kvp.Value), (Func<KeyValuePair<string, string>, string>) (kvp => kvp.Key));
  private Dictionary<string, int> _languageKeyCount = new Dictionary<string, int>();
  public const string locOverrideDir = "user://localization_override";
  private readonly List<LocManager.LocaleChangeCallback> _localeChangeCallbacks = new List<LocManager.LocaleChangeCallback>();
  private static readonly CultureInfo _englishCultureInfo;

  public static LocManager Instance { get; private set; } = (LocManager) null;

  private static string LocalizationAssetDir => "res://localization";

  public bool OverridesActive { get; private set; }

  public IReadOnlyList<LocValidationError> ValidationErrors { get; private set; } = (IReadOnlyList<LocValidationError>) Array.Empty<LocValidationError>();

  public string Language { get; private set; }

  public static List<string> Languages { get; }

  public CultureInfo CultureInfo { get; private set; }

  public StringComparer StringComparer { get; private set; }

  private static CultureInfo GetCultureInfoSafe(string name)
  {
    try
    {
      return CultureInfo.GetCultureInfo(name);
    }
    catch (CultureNotFoundException ex)
    {
      return CultureInfo.InvariantCulture;
    }
  }

  public static void Initialize()
  {
    LocManager.Instance = new LocManager();
    string str = ProjectSettings.GlobalizePath("user://localization_override");
    if (DirAccess.DirExistsAbsolute(str))
      return;
    DirAccess.MakeDirAbsolute(str);
  }

  public LocManager()
  {
    if (LocManager._englishCultureInfo.Equals((object) CultureInfo.InvariantCulture))
      Log.Warn("Running in .NET globalization-invariant mode. Locale-specific formatting will be disabled.");
    string language = SaveManager.Instance.SettingsSave.Language;
    if (string.IsNullOrEmpty(language))
    {
      string rawLanguage = PlatformUtil.GetRawLanguage();
      language = PlatformUtil.GetThreeLetterLanguageCode();
      if (language == null || !LocManager.Languages.Contains(language))
      {
        Log.Warn($"Could not initialize language from platform language: {rawLanguage} (resolved: {language}). Defaulting to english");
        language = "eng";
      }
      else
        Log.Info($"Initializing language for the first time from platform locale: {rawLanguage} -> {language}");
      SaveManager.Instance.SettingsSave.Language = language;
    }
    else if (!LocManager.Languages.Contains(language))
    {
      Log.Warn($"Saved language '{language}' is not supported. Defaulting to english");
      language = "eng";
      SaveManager.Instance.SettingsSave.Language = language;
    }
    this.SetLanguage(language);
    this.LoadLocFormatters();
    this.LoadLocCompletionFile();
  }

  private CultureInfo CultureInfoFromThreeLetterCode(string language)
  {
    string str1;
    if (language != null && language.Length == 3)
    {
      switch (language[0])
      {
        case 'd':
          if (language == "deu")
          {
            str1 = "de";
            goto label_29;
          }
          break;
        case 'e':
          switch (language)
          {
            case "eng":
              str1 = "en";
              goto label_29;
            case "esp":
              str1 = "es-419";
              goto label_29;
          }
          break;
        case 'f':
          if (language == "fra")
          {
            str1 = "fr";
            goto label_29;
          }
          break;
        case 'i':
          if (language == "ita")
          {
            str1 = "it";
            goto label_29;
          }
          break;
        case 'j':
          if (language == "jpn")
          {
            str1 = "ja";
            goto label_29;
          }
          break;
        case 'k':
          if (language == "kor")
          {
            str1 = "ko";
            goto label_29;
          }
          break;
        case 'p':
          switch (language)
          {
            case "pol":
              str1 = "pl";
              goto label_29;
            case "ptb":
              str1 = "pt-br";
              goto label_29;
          }
          break;
        case 'r':
          if (language == "rus")
          {
            str1 = "ru";
            goto label_29;
          }
          break;
        case 's':
          if (language == "spa")
          {
            str1 = "es-ES";
            goto label_29;
          }
          break;
        case 't':
          switch (language)
          {
            case "tha":
              str1 = "th";
              goto label_29;
            case "tur":
              str1 = "tr";
              goto label_29;
          }
          break;
        case 'z':
          switch (language)
          {
            case "zhs":
              str1 = "zh-hans";
              goto label_29;
            case "zht":
              str1 = "zh-hant";
              goto label_29;
          }
          break;
      }
    }
    str1 = (string) null;
label_29:
    string name = str1;
    if (name != null)
      return LocManager.GetCultureInfoSafe(name);
    string str2 = $"Language code {language} could not be mapped to CultureInfo! Add a new manual mapping";
    Log.Error(str2);
    SentryService.CaptureMessage(str2, (SentryLevel) 1);
    return LocManager.GetCultureInfoSafe("en");
  }

  private void LoadLocFormatters()
  {
    LocManager._smartFormatter = new SmartFormatter((SmartSettings) null);
    ListFormatter listFormatter = new ListFormatter();
    LocManager._smartFormatter.AddExtensions(new ISource[5]
    {
      (ISource) listFormatter,
      (ISource) new DictionarySource(),
      (ISource) new ValueTupleSource(),
      (ISource) new ReflectionSource(),
      (ISource) new DefaultSource()
    });
    LocManager._smartFormatter.AddExtensions(new IFormatter[16 /*0x10*/]
    {
      (IFormatter) listFormatter,
      (IFormatter) new PluralLocalizationFormatter(),
      (IFormatter) new ConditionalFormatter(),
      (IFormatter) new ChooseFormatter(),
      (IFormatter) new SubStringFormatter(),
      (IFormatter) new IsMatchFormatter(),
      (IFormatter) new LocaleNumberFormatter(),
      (IFormatter) new DefaultFormatter(),
      (IFormatter) new AbsoluteValueFormatter(),
      (IFormatter) new EnergyIconsFormatter(),
      (IFormatter) new StarIconsFormatter(),
      (IFormatter) new HighlightDifferencesFormatter(),
      (IFormatter) new HighlightDifferencesInverseFormatter(),
      (IFormatter) new PercentMoreFormatter(),
      (IFormatter) new PercentLessFormatter(),
      (IFormatter) new ShowIfUpgradedFormatter()
    });
    Smart.Default = LocManager._smartFormatter;
  }

  private void LoadLocCompletionFile()
  {
    using (FileAccess fileAccess = FileAccess.Open("res://localization/completion.json", (FileAccess.ModeFlags) 1L))
    {
      if (fileAccess == null)
        throw new LocException("Cannot find language completion file: res://localization/completion.json");
      this._languageKeyCount = JsonSerializer.Deserialize<Dictionary<string, int>>(fileAccess.GetAsText(false), LocManagerSerializerContext.Default.DictionaryStringInt32);
    }
  }

  public float GetLanguageCompletion(string language)
  {
    int num;
    if (!this._languageKeyCount.TryGetValue(language, out num))
      num = 0;
    return (float) num / (float) this._languageKeyCount["eng"];
  }

  public string SmartFormat(LocString locString, Dictionary<string, object> variables)
  {
    string rawText = locString.GetRawText();
    CultureInfo cultureInfo = this.GetTable(locString.LocTable).IsLocalKey(locString.LocEntryKey) ? this.CultureInfo : LocManager._englishCultureInfo;
    try
    {
      return LocManager._smartFormatter.Format((IFormatProvider) cultureInfo, rawText, new object[1]
      {
        (object) variables
      });
    }
    catch (Exception ex) when (
    {
      // ISSUE: unable to correctly present filter
      bool flag;
      switch (ex)
      {
        case FormattingException _:
        case ParsingErrors _:
          flag = true;
          break;
        default:
          flag = false;
          break;
      }
      if (flag)
      {
        SuccessfulFiltering;
      }
      else
        throw;
    }
    )
    {
      if (TestMode.IsOn)
        throw;
      string message = $"message={ex.Message}\ntable={locString.LocTable} key={locString.LocEntryKey} variables={LocManager.ToString(variables)}";
      Log.Error("Localization formatting error! " + message);
      string errorPattern = Regex.Replace(ex.Message.Split('\n', StringSplitOptions.None)[0], " at \\d+$", "");
      SentryService.CaptureException((Exception) new LocException(message), (Action<Scope>) (scope => EventLikeExtensions.SetFingerprint((IEventLike) scope, new string[2]
      {
        "LocException",
        errorPattern
      })));
      return rawText;
    }
  }

  private static string ConvertToW(string input)
  {
    int num1 = 0;
    int num2 = 0;
    char[] chArray = new char[input.Length];
    for (int index = 0; index < input.Length; ++index)
    {
      char c = input[index];
      switch (c)
      {
        case '[':
          ++num2;
          break;
        case ']':
          --num2;
          break;
        case '{':
          ++num1;
          break;
        case '}':
          --num1;
          break;
      }
      chArray[index] = !char.IsLetter(c) || num1 != 0 || num2 != 0 ? c : (char.IsUpper(c) ? 'W' : 'w');
    }
    return new string(chArray);
  }

  private static string ToString(Dictionary<string, object> variables)
  {
    return $"{{{string.Join(",", variables.Select<KeyValuePair<string, object>, string>((Func<KeyValuePair<string, object>, string>) (kp => $"{kp.Key}:{kp.Value}")))}}}";
  }

  [MemberNotNull(new string[] {"CultureInfo", "StringComparer", "Language"})]
  public void SetLanguage(string language)
  {
    (Dictionary<string, LocTable> tables, bool overridesActive, List<LocValidationError> validationErrors) = LocManager.LoadTablesFromPath(language);
    this.SetLanguageInternal(language, tables, overridesActive, validationErrors);
  }

  [MemberNotNull(new string[] {"CultureInfo", "StringComparer", "Language"})]
  private void SetLanguageInternal(
    string language,
    Dictionary<string, LocTable> tables,
    bool overridesActive,
    List<LocValidationError> validationErrors)
  {
    this._tables = tables;
    this.OverridesActive = overridesActive;
    this.ValidationErrors = (IReadOnlyList<LocValidationError>) validationErrors.AsReadOnly();
    this.Language = language;
    if (this.OverridesActive)
      Log.Info($"Localization overrides are active for language '{language}'");
    this.CultureInfo = this.CultureInfoFromThreeLetterCode(this.Language);
    this.StringComparer = StringComparer.Create(this.CultureInfo, (CompareOptions) 0);
    if (TestMode.IsOn)
    {
      Callable callable = Callable.From(new Action(this.TriggerLocaleChange));
      ((Callable) ref callable).CallDeferred(Array.Empty<Variant>());
    }
    else
      this.TriggerLocaleChange();
  }

  public void StartOverridingLanguageAsEnglish()
  {
    if (this._engTables == null)
    {
      if (!this.OverridesActive && this.Language == "eng")
      {
        this._engTables = this._tables;
      }
      else
      {
        (Dictionary<string, LocTable> tables, bool overridesActive, List<LocValidationError> validationErrors) tuple = LocManager.LoadTablesFromPath("eng", false);
        this._engTables = tuple.tables;
        if (tuple.overridesActive)
          throw new InvalidOperationException("Overrides should never be active when overriding as english!");
      }
    }
    this._stateBeforeOverridingWithEnglish = new LocManager.PreOverrideState()
    {
      language = this.Language,
      overridesActive = this.OverridesActive,
      validationErrors = this.ValidationErrors,
      tables = this._tables
    };
    this.SetLanguageInternal("eng", this._engTables, false, new List<LocValidationError>());
  }

  public void StopOverridingLanguageAsEnglish()
  {
    if (this._stateBeforeOverridingWithEnglish == null)
    {
      Log.Error("StopOverridingLanguageAsEnglish called, but we aren't overriding with english!");
    }
    else
    {
      this.SetLanguageInternal(this._stateBeforeOverridingWithEnglish.language, this._stateBeforeOverridingWithEnglish.tables, this._stateBeforeOverridingWithEnglish.overridesActive, this._stateBeforeOverridingWithEnglish.validationErrors.ToList<LocValidationError>());
      this._stateBeforeOverridingWithEnglish = (LocManager.PreOverrideState) null;
    }
  }

  private static (Dictionary<string, LocTable> tables, bool overridesActive, List<LocValidationError> validationErrors) LoadTablesFromPath(
    string language,
    bool allowOverride = true)
  {
    Dictionary<string, LocTable> dictionary1 = (Dictionary<string, LocTable>) null;
    if (language != "eng")
      dictionary1 = LocManager.LoadTablesFromPath("eng").tables;
    Dictionary<string, LocTable> dictionary2 = new Dictionary<string, LocTable>();
    List<LocValidationError> validationErrors = new List<LocValidationError>();
    string path = $"{LocManager.LocalizationAssetDir}/{language}";
    bool flag1 = false;
    bool flag2 = false;
    if (!DirAccess.DirExistsAbsolute(path))
    {
      Log.Warn($"Dir path {path} for language {language} does not exist, falling back to eng");
      path = LocManager.LocalizationAssetDir + "/eng";
      flag1 = OS.IsDebugBuild();
    }
    string globalizedOverrideDir = ProjectSettings.GlobalizePath("user://localization_override");
    string str1 = Path.Combine(globalizedOverrideDir, language);
    bool flag3 = DirAccess.DirExistsAbsolute(str1);
    string str2 = Path.Combine(globalizedOverrideDir, "slaythespire2");
    bool flag4 = DirAccess.DirExistsAbsolute(str2);
    if (flag3)
      Log.Info("Found flat localization override directory: " + str1);
    if (flag4)
      Log.Info("Found Weblate nested override directory: " + str2);
    Log.Info("Loading locale path=" + path);
    foreach (string localizationFile in LocManager.ListLocalizationFiles(path))
    {
      string withoutExtension = Path.GetFileNameWithoutExtension(localizationFile);
      Dictionary<string, string> dictionary3 = LocManager.LoadTable($"{path}/{localizationFile}");
      if (flag1)
        dictionary3 = dictionary3.ToDictionary<KeyValuePair<string, string>, string, string>((Func<KeyValuePair<string, string>, string>) (kvp => kvp.Key), (Func<KeyValuePair<string, string>, string>) (kvp => LocManager.ConvertToW(kvp.Value)));
      LocTable valueOrDefault = dictionary1 != null ? CollectionExtensions.GetValueOrDefault<string, LocTable>((IReadOnlyDictionary<string, LocTable>) dictionary1, withoutExtension) : (LocTable) null;
      LocTable locTable = new LocTable(withoutExtension, dictionary3, valueOrDefault);
      if (!flag1 & allowOverride)
      {
        if (flag4 && LocManager.TryLoadWeblateNestedOverrides(globalizedOverrideDir, language, localizationFile, locTable, validationErrors))
          flag2 = true;
        if (flag3 && LocManager.TryLoadOverrideFile(Path.Combine(str1, localizationFile), locTable, validationErrors))
          flag2 = true;
      }
      foreach (string moddedLocTable in ModManager.GetModdedLocTables(language, localizationFile))
      {
        Log.Info($"Found loc table from mod: {language} {localizationFile}. Merging with base loc table");
        Dictionary<string, string> dictionary4 = LocManager.LoadTable(moddedLocTable);
        if (flag1)
          dictionary4 = dictionary4.ToDictionary<KeyValuePair<string, string>, string, string>((Func<KeyValuePair<string, string>, string>) (kvp => kvp.Key), (Func<KeyValuePair<string, string>, string>) (kvp => LocManager.ConvertToW(kvp.Value)));
        locTable.MergeWith(dictionary4);
      }
      dictionary2[withoutExtension] = locTable;
    }
    return (dictionary2, flag2, validationErrors);
  }

  public LocTable GetTable(string name)
  {
    LocTable table;
    if (this._tables.TryGetValue(name, out table))
      return table;
    throw new LocException($"The loc table='{name}' does not exist!");
  }

  private static Dictionary<string, string> LoadTable(string path)
  {
    using (FileAccess fileAccess = FileAccess.Open(path, (FileAccess.ModeFlags) 1L))
    {
      string str = fileAccess != null ? fileAccess.GetAsText(false) : throw new LocException("Cannot find language file: " + path);
      try
      {
        return JsonSerializer.Deserialize<Dictionary<string, string>>(str, LocManagerSerializerContext.Default.DictionaryStringString);
      }
      catch (Exception ex)
      {
        throw new LocException("Failed to parse language file: " + path, ex);
      }
    }
  }

  private static IEnumerable<string> ListLocalizationFiles(string path)
  {
    using (DirAccess dirAccess = DirAccess.Open(path))
      return dirAccess != null ? (IEnumerable<string>) ((IEnumerable<string>) dirAccess.GetFiles()).Where<string>((Func<string, bool>) (s => s.EndsWith(".json"))).ToArray<string>() : throw new LocException("Path does not exist: " + path);
  }

  private static bool TryLoadOverrideFile(
    string overrideFilePath,
    LocTable locTable,
    List<LocValidationError> validationErrors)
  {
    if (!FileAccess.FileExists(overrideFilePath))
      return false;
    Log.Info("Loading localization override: " + overrideFilePath);
    try
    {
      Dictionary<string, string> dictionary = LocManager.LoadTable(overrideFilePath);
      Dictionary<string, string> otherTable = new Dictionary<string, string>();
      foreach (KeyValuePair<string, string> keyValuePair in dictionary)
      {
        string errorMessage;
        if (keyValuePair.Key.StartsWith("EXTENSION.") || LocValidator.ValidateFormatString(keyValuePair.Value, out errorMessage))
        {
          otherTable[keyValuePair.Key] = keyValuePair.Value;
        }
        else
        {
          validationErrors.Add(new LocValidationError(overrideFilePath, keyValuePair.Key, errorMessage));
          Log.Warn("[LocValidation] Invalid format in override file: " + overrideFilePath);
          Log.Warn("  Key: " + keyValuePair.Key);
          Log.Warn("  Error: " + errorMessage);
        }
      }
      locTable.MergeWith(otherTable);
      return true;
    }
    catch (LocException ex)
    {
      validationErrors.Add(new LocValidationError(overrideFilePath, "(JSON parsing error)", ex.InnerException?.Message ?? ex.Message));
      Log.Warn("[LocValidation] Failed to parse override file: " + overrideFilePath);
      Log.Warn("  Error: " + (ex.InnerException?.Message ?? ex.Message));
      return false;
    }
  }

  private static bool TryLoadWeblateNestedOverrides(
    string globalizedOverrideDir,
    string language,
    string filename,
    LocTable locTable,
    List<LocValidationError> validationErrors)
  {
    string str;
    if (!LocManager._gameToWeblateLanguage.TryGetValue(language, out str))
      return false;
    string withoutExtension = Path.GetFileNameWithoutExtension(filename);
    \u003C\u003Ey__InlineArray5<string> buffer = new \u003C\u003Ey__InlineArray5<string>();
    // ISSUE: reference to a compiler-generated method
    \u003CPrivateImplementationDetails\u003E.InlineArrayElementRef<\u003C\u003Ey__InlineArray5<string>, string>(ref buffer, 0) = globalizedOverrideDir;
    // ISSUE: reference to a compiler-generated method
    \u003CPrivateImplementationDetails\u003E.InlineArrayElementRef<\u003C\u003Ey__InlineArray5<string>, string>(ref buffer, 1) = "slaythespire2";
    // ISSUE: reference to a compiler-generated method
    \u003CPrivateImplementationDetails\u003E.InlineArrayElementRef<\u003C\u003Ey__InlineArray5<string>, string>(ref buffer, 2) = withoutExtension;
    // ISSUE: reference to a compiler-generated method
    \u003CPrivateImplementationDetails\u003E.InlineArrayElementRef<\u003C\u003Ey__InlineArray5<string>, string>(ref buffer, 3) = str;
    // ISSUE: reference to a compiler-generated method
    \u003CPrivateImplementationDetails\u003E.InlineArrayElementRef<\u003C\u003Ey__InlineArray5<string>, string>(ref buffer, 4) = filename;
    // ISSUE: reference to a compiler-generated method
    string overrideFilePath = Path.Combine(\u003CPrivateImplementationDetails\u003E.InlineArrayAsReadOnlySpan<\u003C\u003Ey__InlineArray5<string>, string>(in buffer, 5));
    if (!LocManager.TryLoadOverrideFile(overrideFilePath, locTable, validationErrors))
      return false;
    Log.Info("Found Weblate nested override structure: " + overrideFilePath);
    return true;
  }

  public void SubscribeToLocaleChange(LocManager.LocaleChangeCallback callback)
  {
    this._localeChangeCallbacks.Add(callback);
  }

  public void UnsubscribeToLocaleChange(LocManager.LocaleChangeCallback callback)
  {
    this._localeChangeCallbacks.Remove(callback);
  }

  private void TriggerLocaleChange()
  {
    TranslationServer.SetLocale(this.CultureInfo.Name);
    foreach (LocManager.LocaleChangeCallback localeChangeCallback in this._localeChangeCallbacks)
      localeChangeCallback();
    GC.Collect();
  }

  static LocManager()
  {
    int capacity = 15;
    List<string> stringList = new List<string>(capacity);
    CollectionsMarshal.SetCount<string>(stringList, capacity);
    Span<string> span = CollectionsMarshal.AsSpan<string>(stringList);
    int num1 = 0;
    span[num1] = "eng";
    int num2 = num1 + 1;
    span[num2] = "zhs";
    int num3 = num2 + 1;
    span[num3] = "zht";
    int num4 = num3 + 1;
    span[num4] = "deu";
    int num5 = num4 + 1;
    span[num5] = "esp";
    int num6 = num5 + 1;
    span[num6] = "fra";
    int num7 = num6 + 1;
    span[num7] = "ita";
    int num8 = num7 + 1;
    span[num8] = "jpn";
    int num9 = num8 + 1;
    span[num9] = "kor";
    int num10 = num9 + 1;
    span[num10] = "pol";
    int num11 = num10 + 1;
    span[num11] = "ptb";
    int num12 = num11 + 1;
    span[num12] = "rus";
    int num13 = num12 + 1;
    span[num13] = "spa";
    int num14 = num13 + 1;
    span[num14] = "tha";
    int num15 = num14 + 1;
    span[num15] = "tur";
    LocManager.Languages = stringList;
    LocManager._englishCultureInfo = LocManager.GetCultureInfoSafe("en");
  }

  private class PreOverrideState
  {
    public required string language;
    public bool overridesActive;
    public required IReadOnlyList<LocValidationError> validationErrors;
    public required Dictionary<string, LocTable> tables;
  }

  public delegate void LocaleChangeCallback();
}
