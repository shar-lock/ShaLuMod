// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Runs.RunHistoryPlayer
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Saves.Runs;
using System.Collections.Generic;
using System.Text.Json.Serialization;

#nullable enable
namespace MegaCrit.Sts2.Core.Runs;

public class RunHistoryPlayer
{
  [JsonPropertyName("id")]
  public ulong Id { get; init; }

  [JsonPropertyName("character")]
  public ModelId Character { get; init; } = ModelId.none;

  [JsonPropertyName("deck")]
  public IEnumerable<SerializableCard> Deck { get; set; } = (IEnumerable<SerializableCard>) new List<SerializableCard>();

  [JsonPropertyName("relics")]
  public IEnumerable<SerializableRelic> Relics { get; set; } = (IEnumerable<SerializableRelic>) new List<SerializableRelic>();

  [JsonPropertyName("potions")]
  public IEnumerable<SerializablePotion> Potions { get; set; } = (IEnumerable<SerializablePotion>) new List<SerializablePotion>();

  [JsonPropertyName("badges")]
  public IEnumerable<SerializableBadge> Badges { get; set; } = (IEnumerable<SerializableBadge>) new List<SerializableBadge>();

  [JsonPropertyName("max_potion_slot_count")]
  public int MaxPotionSlotCount { get; set; } = 3;
}
