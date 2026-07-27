// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.GameInfo.Objects.IGameInfo
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using System.Text.Json.Serialization;

#nullable enable
namespace MegaCrit.Sts2.GameInfo.Objects;

[JsonDerivedType(typeof (AncientChoiceInfo))]
[JsonDerivedType(typeof (CardInfo))]
[JsonDerivedType(typeof (DailyMods))]
[JsonDerivedType(typeof (EncounterInfo))]
[JsonDerivedType(typeof (EventInfo))]
[JsonDerivedType(typeof (Keywords))]
[JsonDerivedType(typeof (NeowBonusInfo))]
[JsonDerivedType(typeof (PotionInfo))]
[JsonDerivedType(typeof (RelicInfo))]
[JsonDerivedType(typeof (EnchantmentInfo))]
public interface IGameInfo
{
  [JsonPropertyName("name")]
  string Name { get; }

  [JsonPropertyName("bot_keyword")]
  string BotKeyword { get; }

  [JsonPropertyName("bot_text")]
  string BotText { get; }
}
