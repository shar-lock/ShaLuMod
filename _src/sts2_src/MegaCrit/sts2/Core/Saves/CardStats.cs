// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Saves.CardStats
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Models;
using System.Text.Json.Serialization;

#nullable enable
namespace MegaCrit.Sts2.Core.Saves;

public class CardStats
{
  [JsonPropertyName("id")]
  public ModelId? Id { get; init; }

  [JsonPropertyName("times_picked")]
  public long TimesPicked { get; set; }

  [JsonPropertyName("times_skipped")]
  public long TimesSkipped { get; set; }

  [JsonPropertyName("times_won")]
  public long TimesWon { get; set; }

  [JsonPropertyName("times_lost")]
  public long TimesLost { get; set; }
}
