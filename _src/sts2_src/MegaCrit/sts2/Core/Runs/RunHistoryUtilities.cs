// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Runs.RunHistoryUtilities
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Debug;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Badges;
using MegaCrit.Sts2.Core.Platform;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Runs.History;
using MegaCrit.Sts2.Core.Saves;
using MegaCrit.Sts2.Core.Saves.Runs;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable enable
namespace MegaCrit.Sts2.Core.Runs;

public static class RunHistoryUtilities
{
  public static void CreateRunHistoryEntry(
    SerializableRun run,
    bool victory,
    bool isAbandoned,
    PlatformType platformType)
  {
    ModelId modelId1 = ModelId.none;
    ModelId modelId2 = ModelId.none;
    List<MapPointHistoryEntry> source = run.MapPointHistory.LastOrDefault<List<MapPointHistoryEntry>>();
    MapPointHistoryEntry pointHistoryEntry = source != null ? source.LastOrDefault<MapPointHistoryEntry>() : (MapPointHistoryEntry) null;
    if (!victory && pointHistoryEntry != null)
    {
      MapPointRoomHistoryEntry roomHistoryEntry = pointHistoryEntry.Rooms.Last<MapPointRoomHistoryEntry>();
      RoomType roomType = roomHistoryEntry.RoomType;
      if (roomType.IsCombatRoom())
        modelId1 = roomHistoryEntry.ModelId;
      else if (roomType == RoomType.Event)
        modelId2 = roomHistoryEntry.ModelId;
    }
    List<RunHistoryPlayer> runHistoryPlayerList = new List<RunHistoryPlayer>();
    foreach (SerializablePlayer player in run.Players)
    {
      RunHistoryPlayer runHistoryPlayer = new RunHistoryPlayer()
      {
        Id = player.NetId,
        Character = player.CharacterId,
        Deck = (IEnumerable<SerializableCard>) player.Deck,
        Relics = (IEnumerable<SerializableRelic>) player.Relics,
        Potions = (IEnumerable<SerializablePotion>) player.Potions,
        Badges = RunHistoryUtilities.GetBadgesForPlayer(run, player, victory, isAbandoned),
        MaxPotionSlotCount = player.MaxPotionSlotCount
      };
      runHistoryPlayerList.Add(runHistoryPlayer);
    }
    RunHistory history = new RunHistory()
    {
      BuildId = ReleaseInfoManager.Instance.ReleaseInfo?.Version ?? "NON-RELEASE-VERSION",
      PlatformType = platformType,
      Players = runHistoryPlayerList,
      GameMode = RunHistoryUtilities.GetGameMode(run),
      Win = victory,
      KilledByEncounter = modelId1,
      KilledByEvent = modelId2,
      WasAbandoned = isAbandoned,
      Seed = run.SerializableRng.Seed,
      StartTime = run.StartTime,
      RunTime = run.WinTime > 0L ? (float) run.WinTime : (float) run.RunTime,
      MapPointHistory = run.MapPointHistory,
      Ascension = run.Ascension,
      Acts = run.Acts.Select<SerializableActModel, ModelId>((Func<SerializableActModel, ModelId>) (a => a.Id)).ToList<ModelId>(),
      Modifiers = run.Modifiers
    };
    SaveManager.Instance.SaveRunHistory(history);
    Log.Info("Created Run History entry!");
    if (!RunManager.Instance.IsInProgress)
      return;
    RunManager.Instance.History = history;
  }

  private static IEnumerable<SerializableBadge> GetBadgesForPlayer(
    SerializableRun run,
    SerializablePlayer player,
    bool won,
    bool isAbandoned)
  {
    List<SerializableBadge> badgesForPlayer = new List<SerializableBadge>();
    if (isAbandoned)
      return (IEnumerable<SerializableBadge>) badgesForPlayer;
    foreach (Badge badge in ScoreUtility.GetBadges(run, player.NetId, won))
      badgesForPlayer.Add(badge.ToSerializable());
    return (IEnumerable<SerializableBadge>) badgesForPlayer;
  }

  private static GameMode GetGameMode(SerializableRun run) => run.GameMode;
}
