// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Leaderboard.LeaderboardManager
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Platform;
using MegaCrit.Sts2.Core.Platform.Null;
using MegaCrit.Sts2.Core.Platform.Steam;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Leaderboard;

public static class LeaderboardManager
{
  private static ILeaderboardStrategy _strategy;

  public static void Initialize()
  {
    if (SteamInitializer.Initialized)
      LeaderboardManager._strategy = (ILeaderboardStrategy) new SteamLeaderboardStrategy();
    else
      LeaderboardManager._strategy = (ILeaderboardStrategy) new NullLeaderboardStrategy();
  }

  public static PlatformType CurrentPlatform => LeaderboardManager._strategy.Platform;

  public static Task<ILeaderboardHandle> GetOrCreateLeaderboard(
    string name,
    CancellationToken cancelToken = default (CancellationToken))
  {
    return LeaderboardManager._strategy.GetOrCreateLeaderboard(name, cancelToken);
  }

  public static Task<ILeaderboardHandle?> GetLeaderboard(string name, CancellationToken cancelToken = default (CancellationToken))
  {
    return LeaderboardManager._strategy.GetLeaderboard(name, cancelToken);
  }

  public static Task UploadLocalScore(
    ILeaderboardHandle handle,
    int score,
    IReadOnlyList<ulong> userIds)
  {
    return LeaderboardManager._strategy.UploadLocalScore(handle, score, userIds);
  }

  public static Task<List<LeaderboardEntry>> QueryLeaderboard(
    ILeaderboardHandle handle,
    LeaderboardQueryType type,
    int startIndex,
    int resultCount,
    CancellationToken cancelToken = default (CancellationToken))
  {
    return LeaderboardManager._strategy.QueryLeaderboard(handle, type, startIndex, resultCount, cancelToken);
  }

  public static Task<List<LeaderboardEntry>> QueryLeaderboardForUsers(
    ILeaderboardHandle handle,
    IReadOnlyList<ulong> userIds,
    CancellationToken cancelToken = default (CancellationToken))
  {
    return LeaderboardManager._strategy.QueryLeaderboardForUsers(handle, userIds, cancelToken);
  }

  public static int GetLeaderboardEntryCount(ILeaderboardHandle handle)
  {
    return LeaderboardManager._strategy.GetLeaderboardEntryCount(handle);
  }

  public static void DebugAddEntry(ILeaderboardHandle handle, LeaderboardEntry entry)
  {
    if (!(LeaderboardManager._strategy is NullLeaderboardStrategy strategy))
      throw new NotImplementedException();
    strategy.DebugAddEntry(handle, entry);
  }
}
