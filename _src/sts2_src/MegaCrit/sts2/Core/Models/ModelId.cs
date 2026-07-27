// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.ModelId
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Helpers;
using System;
using System.Runtime.CompilerServices;
using System.Text.Json;

#nullable enable
namespace MegaCrit.Sts2.Core.Models;

public record ModelId : IComparable<ModelId>
{
  public static readonly ModelId none = new ModelId("NONE", "NONE");
  private const string _bannedSuffix = "_MODEL";

  public string Category { get; }

  public string Entry { get; }

  public ModelId(string category, string entry)
  {
    this.Category = !category.EndsWith("_MODEL") ? category : throw new ArgumentException("Category cannot end with '_MODEL'.", nameof (category));
    this.Entry = entry;
  }

  public static ModelId Deserialize(string json)
  {
    string[] strArray = json.Split('.', StringSplitOptions.None);
    return strArray.Length == 2 ? new ModelId(strArray[0], strArray[1]) : throw new JsonException($"'{json}' does not match the expected ModelId form.");
  }

  public override string ToString() => $"{this.Category}.{this.Entry}";

  public int CompareTo(ModelId? other)
  {
    int num = string.Compare(this.Category, other?.Category, StringComparison.Ordinal);
    return num != 0 ? num : string.Compare(this.Entry, other?.Entry, StringComparison.Ordinal);
  }

  public static string SlugifyCategory<T>() => ModelId.SlugifyCategory(typeof (T).Name);

  public static string SlugifyCategory(string category)
  {
    string str1 = StringHelper.Slugify(category);
    if (str1.EndsWith("_MODEL"))
    {
      string str2 = str1;
      int length = "_MODEL".Length;
      str1 = str2.Substring(0, str2.Length - length);
    }
    return str1;
  }

  [CompilerGenerated]
  protected ModelId(ModelId original)
  {
    this.Category = original.Category;
    this.Entry = original.Entry;
  }
}
