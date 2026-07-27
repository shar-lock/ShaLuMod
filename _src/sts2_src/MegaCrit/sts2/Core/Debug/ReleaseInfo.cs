// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Debug.ReleaseInfo
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using System;
using System.Text.Json.Serialization;

#nullable enable
namespace MegaCrit.Sts2.Core.Debug;

public class ReleaseInfo
{
  [JsonPropertyName("commit")]
  public required string Commit { get; init; }

  [JsonPropertyName("version")]
  public required string Version { get; init; }

  [JsonPropertyName("date")]
  [JsonConverter(typeof (CustomDateTimeConverter))]
  public required DateTime Date { get; init; }

  [JsonPropertyName("branch")]
  public required string Branch { get; init; }

  [JsonPropertyName("main_assembly_hash")]
  public required int MainAssemblyHash { get; init; }
}
