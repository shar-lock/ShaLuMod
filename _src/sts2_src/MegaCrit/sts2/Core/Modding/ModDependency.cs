// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Modding.ModDependency
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using System.Text.Json.Serialization;

#nullable enable
namespace MegaCrit.Sts2.Core.Modding;

public class ModDependency
{
  [JsonPropertyName("id")]
  public string id;
  [JsonPropertyName("min_version")]
  public string? minVersion;

  public ModDependency(string id, string? minVersion = null)
  {
    this.id = id;
    this.minVersion = minVersion;
  }
}
