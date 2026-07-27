// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Runs.Metrics.RunMetrics
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Models;
using System.Collections.Generic;

#nullable enable
namespace MegaCrit.Sts2.Core.Runs.Metrics;

public class RunMetrics
{
  public required string BuildId { get; init; }

  public required string PlayerId { get; init; }

  public required ModelId Character { get; init; }

  public required bool Win { get; init; }

  public required int NumPlayers { get; init; }

  public required List<ModelId> Team { get; init; }

  public required string BuildType { get; init; }

  public int Ascension { get; init; }

  public required float TotalPlaytime { get; init; }

  public required float TotalWinRate { get; init; }

  public required int NumReloads { get; init; }

  public required float RunPlaytime { get; init; }

  public required int FloorReached { get; init; }

  public required ModelId KilledByEncounter { get; init; }

  public required List<CardChoiceMetric> CardChoices { get; init; }

  public required List<string> CampfireUpgrades { get; init; }

  public required List<EventChoiceMetric> EventChoices { get; init; }

  public required List<AncientMetric> AncientChoices { get; init; }

  public required List<string> RelicBuys { get; init; }

  public required List<string> PotionBuys { get; init; }

  public required List<string> ColorlessBuys { get; init; }

  public required List<string> PotionDiscards { get; init; }

  public required List<EncounterMetric> Encounters { get; init; }

  public required List<ActWinMetric> ActWins { get; init; }

  public required IEnumerable<ModelId> Deck { get; init; }

  public required IEnumerable<ModelId> Relics { get; init; }
}
