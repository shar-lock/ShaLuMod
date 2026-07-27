// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Timeline.EpochMetric
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using System.Text.Json.Serialization;

#nullable enable
namespace MegaCrit.Sts2.Core.Timeline;

public class EpochMetric
{
  [JsonPropertyName("buildId")]
  public required string BuildId { get; set; }

  [JsonPropertyName("epoch")]
  public required string Epoch { get; set; }

  [JsonPropertyName("totalEpochs")]
  public int TotalEpochs { get; set; }

  [JsonPropertyName("totalPlaytime")]
  public long TotalPlaytime { get; set; }

  [JsonPropertyName("totalRuns")]
  public int TotalRuns { get; set; }
}
