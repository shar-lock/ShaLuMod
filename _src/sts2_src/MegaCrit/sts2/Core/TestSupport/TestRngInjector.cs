// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.TestSupport.TestRngInjector
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Models;
using System;
using System.Collections.Generic;

#nullable enable
namespace MegaCrit.Sts2.Core.TestSupport;

public static class TestRngInjector
{
  private static RelicModel? _relicOverride;
  private static RelicRarity? _relicRarityOverride;
  private static Action<List<CardModel>>? _initialShuffleOverride;
  private static List<CardModel>? _combatCardGenerationOverride;

  public static void SetRelicOverride<T>() where T : RelicModel
  {
    TestRngInjector._relicOverride = (RelicModel) ModelDb.Relic<T>();
  }

  public static RelicModel? ConsumeRelicOverride()
  {
    RelicModel relicOverride = TestRngInjector._relicOverride;
    TestRngInjector._relicOverride = (RelicModel) null;
    return relicOverride;
  }

  public static void SetRelicRarityOverride(RelicRarity relicRarity)
  {
    TestRngInjector._relicRarityOverride = new RelicRarity?(relicRarity);
  }

  public static RelicRarity? GetRelicRarityOverride() => TestRngInjector._relicRarityOverride;

  public static void SetCombatCardGenerationOverride(List<CardModel> cards)
  {
    TestRngInjector._combatCardGenerationOverride = cards;
  }

  public static List<CardModel>? ConsumeCombatCardGenerationOverride()
  {
    List<CardModel> generationOverride = TestRngInjector._combatCardGenerationOverride;
    TestRngInjector._combatCardGenerationOverride = (List<CardModel>) null;
    return generationOverride;
  }

  public static void SetInitialShuffleOverride(Action<List<CardModel>> reorder)
  {
    TestRngInjector._initialShuffleOverride = reorder;
  }

  public static Action<List<CardModel>>? ConsumeInitialShuffleOverride()
  {
    Action<List<CardModel>> initialShuffleOverride = TestRngInjector._initialShuffleOverride;
    TestRngInjector._initialShuffleOverride = (Action<List<CardModel>>) null;
    return initialShuffleOverride;
  }

  public static void Cleanup()
  {
    TestRngInjector._relicOverride = (RelicModel) null;
    TestRngInjector._relicRarityOverride = new RelicRarity?();
    TestRngInjector._initialShuffleOverride = (Action<List<CardModel>>) null;
    TestRngInjector._combatCardGenerationOverride = (List<CardModel>) null;
  }
}
