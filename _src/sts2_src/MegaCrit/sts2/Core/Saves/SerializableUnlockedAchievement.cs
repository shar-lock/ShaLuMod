// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Saves.SerializableUnlockedAchievement
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using System.Text.Json.Serialization;

#nullable enable
namespace MegaCrit.Sts2.Core.Saves;

public record SerializableUnlockedAchievement
{
  [JsonPropertyName("achievement")]
  public string Achievement { get; init; }

  [JsonPropertyName("unlock_time")]
  public long UnlockTime { get; init; }

  public SerializableUnlockedAchievement()
  {
  }
}
