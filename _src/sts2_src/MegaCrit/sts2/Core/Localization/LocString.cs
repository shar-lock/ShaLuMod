// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Localization.LocString
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Random;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

#nullable enable
namespace MegaCrit.Sts2.Core.Localization;

public class LocString(string locTable, string locEntryKey) : IComparable<LocString>
{
  [JsonIgnore]
  private readonly Dictionary<string, object> _variables = new Dictionary<string, object>();

  [JsonPropertyName("table")]
  public string LocTable => locTable;

  [JsonPropertyName("key")]
  public string LocEntryKey => locEntryKey;

  [JsonIgnore]
  public bool IsEmpty
  {
    get => string.IsNullOrEmpty(this.LocEntryKey) && string.IsNullOrEmpty(this.LocTable);
  }

  [JsonIgnore]
  public IReadOnlyDictionary<string, object> Variables
  {
    get => (IReadOnlyDictionary<string, object>) this._variables;
  }

  public static bool Exists(string table, string key)
  {
    return LocManager.Instance.GetTable(table).HasEntry(key);
  }

  public static LocString? GetIfExists(string table, string key)
  {
    return !LocString.Exists(table, key) ? (LocString) null : new LocString(table, key);
  }

  public string GetFormattedText() => LocManager.Instance.SmartFormat(this, this._variables);

  public string GetRawText()
  {
    return LocManager.Instance.GetTable(this.LocTable).GetRawText(this.LocEntryKey);
  }

  public bool Exists() => LocString.Exists(this.LocTable, this.LocEntryKey);

  public static bool IsNullOrWhitespace(LocString? locString)
  {
    return locString == null || locString.IsEmpty || string.IsNullOrWhiteSpace(locString.GetRawText());
  }

  public static void SubscribeToLocaleChange(LocManager.LocaleChangeCallback callback)
  {
    LocManager.Instance.SubscribeToLocaleChange(callback);
  }

  public static void UnsubscribeToLocaleChange(LocManager.LocaleChangeCallback callback)
  {
    LocManager.Instance.UnsubscribeToLocaleChange(callback);
  }

  public void Add(DynamicVar dynamicVar) => this.AddObj(dynamicVar.Name, (object) dynamicVar);

  public void Add(string name, Decimal variable) => this.AddObj(name, (object) variable);

  public void Add(string name, bool variable) => this.AddObj(name, (object) variable);

  public void Add(string name, string variable) => this.AddObj(name, (object) variable);

  public void Add(string name, IList<string> variable) => this.AddObj(name, (object) variable);

  public void Add(string name, LocString variable)
  {
    this.AddObj(name, (object) variable.GetFormattedText());
  }

  public void AddObj(string name, object variable)
  {
    ArgumentException.ThrowIfNullOrEmpty(name, nameof (name));
    ArgumentNullException.ThrowIfNull(variable, nameof (variable));
    name = name.Replace(' ', '-');
    if (this._variables.TryAdd(name, variable))
      return;
    this._variables[name] = variable;
  }

  private object this[string key]
  {
    get => this._variables[key];
    set => this._variables[key] = value;
  }

  public static LocString KeyPathToLocString(string keyPath)
  {
    string[] strArray = keyPath.Split('/', StringSplitOptions.None);
    return new LocString(strArray[0], strArray[1]);
  }

  public void AddVariablesFrom(LocString smartDescription)
  {
    foreach (string key in smartDescription._variables.Keys)
      this.AddObj(key, smartDescription[key]);
  }

  public static LocString GetRandomWithPrefix(string table, string keyPrefix, Rng? rng = null)
  {
    IReadOnlyList<LocString> stringsWithPrefix = LocManager.Instance.GetTable(table).GetLocStringsWithPrefix(keyPrefix);
    if (rng == null)
      rng = Rng.Chaotic;
    return rng.NextItem<LocString>((IEnumerable<LocString>) stringsWithPrefix);
  }

  public int CompareTo(LocString? other)
  {
    return locTable != other?.LocTable ? string.Compare(this.LocTable, other?.LocTable, StringComparison.Ordinal) : string.Compare(this.LocEntryKey, other.LocEntryKey, StringComparison.Ordinal);
  }

  public override string ToString() => $"LocString table {this.LocTable} entry {locEntryKey}";
}
