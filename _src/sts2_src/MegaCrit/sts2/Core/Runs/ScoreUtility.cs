// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Runs.ScoreUtility
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Models.Badges;
using MegaCrit.Sts2.Core.Nodes.Screens.DailyRun;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Runs.History;
using MegaCrit.Sts2.Core.Saves;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable enable
namespace MegaCrit.Sts2.Core.Runs;

public static class ScoreUtility
{
  public const int clientScore = -999999999;

  public static int CalculateScore(IRunState runState, bool won)
  {
    return ScoreUtility.CalculateScore(runState.MapPointHistory, runState.AscensionLevel, won, runState.Players.Count);
  }

  public static int CalculateScore(SerializableRun run, bool won)
  {
    return ScoreUtility.CalculateScore((IReadOnlyList<IReadOnlyList<MapPointHistoryEntry>>) run.MapPointHistory, run.Ascension, won, run.Players.Count);
  }

  private static int CalculateScore(
    IReadOnlyList<IReadOnlyList<MapPointHistoryEntry>> history,
    int ascension,
    bool won,
    int playerCount)
  {
    return (int) ((double) (0 + ScoreUtility.GetScoreForFloor(history) + ScoreUtility.GetScoreForGoldGained(history, playerCount) + ScoreUtility.GetScoreForElitesKilled(ScoreUtility.GetElitesKilledCount(history)) + ScoreUtility.GetScoreForBossesSlain(ScoreUtility.GetBossesSlainCount(history, won))) * (1.0 + (double) ascension * 0.1));
  }

  public static int GetScoreForFloor(
    IReadOnlyList<IReadOnlyList<MapPointHistoryEntry>> history)
  {
    int scoreForFloor = 0;
    int count = history.Count;
    for (int index = 0; index < count; ++index)
      scoreForFloor += history[index].Count * 10 * (index + 1);
    return scoreForFloor;
  }

  public static int GetElitesKilledCount(
    IReadOnlyList<IReadOnlyList<MapPointHistoryEntry>> history)
  {
    List<MapPointRoomHistoryEntry> list = history.SelectMany<IReadOnlyList<MapPointHistoryEntry>, MapPointHistoryEntry>((Func<IReadOnlyList<MapPointHistoryEntry>, IEnumerable<MapPointHistoryEntry>>) (actEntries => (IEnumerable<MapPointHistoryEntry>) actEntries)).SelectMany<MapPointHistoryEntry, MapPointRoomHistoryEntry>((Func<MapPointHistoryEntry, IEnumerable<MapPointRoomHistoryEntry>>) (e => (IEnumerable<MapPointRoomHistoryEntry>) e.Rooms)).ToList<MapPointRoomHistoryEntry>();
    int elitesKilledCount = list.Count<MapPointRoomHistoryEntry>((Func<MapPointRoomHistoryEntry, bool>) (r => r.RoomType == RoomType.Elite));
    if (list.Count > 0 && list.Last<MapPointRoomHistoryEntry>().RoomType == RoomType.Elite)
      --elitesKilledCount;
    return elitesKilledCount;
  }

  public static int GetScoreForElitesKilled(int elitesKilled) => elitesKilled * 50;

  public static int GetBossesSlainCount(
    IReadOnlyList<IReadOnlyList<MapPointHistoryEntry>> history,
    bool won)
  {
    int bossesSlainCount = 0;
    foreach (IReadOnlyList<MapPointHistoryEntry> source in (IEnumerable<IReadOnlyList<MapPointHistoryEntry>>) history)
    {
      foreach (MapPointHistoryEntry pointHistoryEntry in (IEnumerable<MapPointHistoryEntry>) source)
      {
        foreach (MapPointRoomHistoryEntry room in pointHistoryEntry.Rooms)
        {
          bool flag = !won && source == history.Last<IReadOnlyList<MapPointHistoryEntry>>() && pointHistoryEntry == source.Last<MapPointHistoryEntry>() && room == pointHistoryEntry.Rooms.Last<MapPointRoomHistoryEntry>();
          if (room.RoomType == RoomType.Boss && !flag)
            ++bossesSlainCount;
        }
      }
    }
    return bossesSlainCount;
  }

  public static int GetScoreForBossesSlain(int bossCount) => bossCount * 100;

  public static int GetScoreForGoldGained(
    IReadOnlyList<IReadOnlyList<MapPointHistoryEntry>> history,
    int playerCount)
  {
    return history.SelectMany<IReadOnlyList<MapPointHistoryEntry>, MapPointHistoryEntry>((Func<IReadOnlyList<MapPointHistoryEntry>, IEnumerable<MapPointHistoryEntry>>) (actEntries => (IEnumerable<MapPointHistoryEntry>) actEntries)).SelectMany<MapPointHistoryEntry, PlayerMapPointHistoryEntry>((Func<MapPointHistoryEntry, IEnumerable<PlayerMapPointHistoryEntry>>) (e => (IEnumerable<PlayerMapPointHistoryEntry>) e.PlayerStats)).Sum<PlayerMapPointHistoryEntry>((Func<PlayerMapPointHistoryEntry, int>) (p => p.GoldGained)) / (100 * playerCount);
  }

  public static List<Badge> GetBadges(SerializableRun run, ulong playerId, bool won)
  {
    List<Badge> badges = new List<Badge>();
    foreach (Badge badge in (IEnumerable<Badge>) BadgePool.CreateAll(run, playerId, won))
    {
      if ((!badge.RequiresWin || won) && (!badge.MultiplayerOnly || run.Players.Count != 1) && badge.IsObtained())
        badges.Add(badge);
    }
    return badges;
  }

  public static int CalculateDailyScore(
    SerializableRun run,
    ulong localPlayerNetId,
    bool isVictory)
  {
    return (isVictory ? 2 : 1) * 100000000 + Math.Clamp(run.FloorReached, 0, 99) * 1000000 + Math.Clamp(ScoreUtility.GetBadges(run, localPlayerNetId, isVictory).Count, 0, 99) * 10000 + (9999 - (int) Math.Clamp(isVictory ? run.WinTime : run.RunTime, 0L, 9999L));
  }

  public static DecodedDailyScore DecodeDailyScore(int encodedScore)
  {
    int num1 = encodedScore / 100000000;
    int num2 = encodedScore / 1000000 % 100;
    int num3 = encodedScore / 10000 % 100;
    int num4 = 9999 - encodedScore % 10000;
    bool flag1;
    switch (num1)
    {
      case 1:
      case 2:
      case 3:
        flag1 = true;
        break;
      default:
        flag1 = false;
        break;
    }
    bool flag2 = flag1 && num2 >= 0 && num2 <= 99 && num3 >= 0 && num3 <= 99 && num4 >= 0 && num4 <= 9999;
    return new DecodedDailyScore()
    {
      isValid = flag2,
      victory = num1,
      floors = num2,
      badges = num3,
      runTime = num4
    };
  }
}
