// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Daily.DailyRunUtility
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Leaderboard;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Nodes;
using MegaCrit.Sts2.Core.Platform;
using MegaCrit.Sts2.Core.Saves.Runs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Daily;

public static class DailyRunUtility
{
  public static async Task UploadScore(
    DateTimeOffset time,
    int score,
    List<SerializablePlayer> players)
  {
    List<ulong> playerIdsInRun = players.Select<SerializablePlayer, ulong>((Func<SerializablePlayer, ulong>) (p => p.NetId)).ToList<ulong>();
    if (playerIdsInRun != null && playerIdsInRun.Count == 1 && playerIdsInRun[0] == 1UL)
      playerIdsInRun[0] = PlatformUtil.GetLocalPlayerId(LeaderboardManager.CurrentPlatform);
    string leaderboardName = DailyRunUtility.GetLeaderboardName(time, players.Count);
    if (!await DailyRunUtility.ShouldUploadScore(await LeaderboardManager.GetLeaderboard(leaderboardName), (IReadOnlyList<ulong>) playerIdsInRun))
    {
      Log.Info($"Player already uploaded score for daily {time}, ignoring new score");
      playerIdsInRun = (List<ulong>) null;
      leaderboardName = (string) null;
    }
    else
    {
      await LeaderboardManager.UploadLocalScore(await LeaderboardManager.GetOrCreateLeaderboard(leaderboardName), score, (IReadOnlyList<ulong>) playerIdsInRun);
      Log.Info($"Uploaded score of {score} for daily {time} to leaderboard {leaderboardName}");
      playerIdsInRun = (List<ulong>) null;
      leaderboardName = (string) null;
    }
  }

  public static async Task<bool> ShouldUploadScore(
    ILeaderboardHandle? handle,
    IReadOnlyList<ulong> playerIdsInRun,
    CancellationToken cancelToken = default (CancellationToken))
  {
    return handle == null || (await LeaderboardManager.QueryLeaderboardForUsers(handle, playerIdsInRun, cancelToken)).Count <= 0;
  }

  public static string GetLeaderboardName(DateTimeOffset dateTime, int playerCount)
  {
    bool flag;
    switch (PlatformUtil.GetPlatformBranch())
    {
      case PlatformBranch.PublicBeta:
      case PlatformBranch.PrivateBeta:
      case PlatformBranch.DevTest:
        flag = true;
        break;
      default:
        flag = false;
        break;
    }
    if (flag || !NGame.IsReleaseGame())
      return $"{dateTime.Year}_{dateTime.Month:D2}_{dateTime.Day:D2}_{playerCount}p_BETA";
    return $"{dateTime.Year}_{dateTime.Month:D2}_{dateTime.Day:D2}_{playerCount}p";
  }

  public static DateTimeOffset? AddLeaderboardDays(DateTimeOffset day, int days)
  {
    try
    {
      return new DateTimeOffset?(day + TimeSpan.FromDays(days));
    }
    catch (ArgumentOutOfRangeException ex)
    {
      return new DateTimeOffset?();
    }
  }
}
