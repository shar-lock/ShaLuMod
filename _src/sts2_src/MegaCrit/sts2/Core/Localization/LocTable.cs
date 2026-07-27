// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Localization.LocTable
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using System;
using System.Collections.Generic;
using System.Linq;

#nullable enable
namespace MegaCrit.Sts2.Core.Localization;

public class LocTable
{
  private readonly string _name;
  private readonly Dictionary<string, string> _translations;
  private readonly LocTable? _fallback;

  public LocTable(string name, Dictionary<string, string> data, LocTable? fallback = null)
  {
    this._name = name;
    this._translations = data;
    this._fallback = fallback;
  }

  public IEnumerable<string> Keys => (IEnumerable<string>) this._translations.Keys;

  public void MergeWith(Dictionary<string, string> otherTable)
  {
    foreach (KeyValuePair<string, string> keyValuePair in otherTable)
      this._translations[keyValuePair.Key] = keyValuePair.Value;
  }

  public LocString GetLocString(string key)
  {
    if (!this._translations.ContainsKey(key))
    {
      LocTable fallback = this._fallback;
      if ((fallback != null ? (fallback.HasEntry(key) ? 1 : 0) : 0) == 0)
        throw new LocException($"Key={key} not found in table={this._name}");
    }
    return new LocString(this._name, key);
  }

  public string GetRawText(string key)
  {
    string rawText;
    if (this._translations.TryGetValue(key, out rawText))
      return rawText;
    return this._fallback != null ? this._fallback.GetRawText(key) : throw new LocException($"Key={key} not found in table={this._name}");
  }

  public IReadOnlyList<LocString> GetLocStringsWithPrefix(string keyPrefix)
  {
    HashSet<string> source = new HashSet<string>(this._translations.Keys.Where<string>((Func<string, bool>) (k => k.StartsWith(keyPrefix))));
    if (this._fallback != null)
    {
      foreach (string str in this._fallback.Keys.Where<string>((Func<string, bool>) (k => k.StartsWith(keyPrefix))))
        source.Add(str);
    }
    return (IReadOnlyList<LocString>) source.Select<string, LocString>((Func<string, LocString>) (k => new LocString(this._name, k))).ToList<LocString>();
  }

  public bool IsLocalKey(string key) => this._translations.ContainsKey(key);

  public bool HasEntry(string key)
  {
    if (this._translations.ContainsKey(key))
      return true;
    LocTable fallback = this._fallback;
    return fallback != null && fallback.HasEntry(key);
  }
}
