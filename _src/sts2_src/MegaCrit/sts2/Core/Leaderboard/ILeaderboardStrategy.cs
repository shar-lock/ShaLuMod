// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Leaderboard.ILeaderboardStrategy
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Platform;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Leaderboard;

public interface ILeaderboardStrategy
{
  PlatformType Platform { get; }

  Task<ILeaderboardHandle> GetOrCreateLeaderboard(string name, CancellationToken cancelToken);

  Task<ILeaderboardHandle?> GetLeaderboard(string name, CancellationToken cancelToken);

  Task UploadLocalScore(ILeaderboardHandle handle, int score, IReadOnlyList<ulong> otherIds);

  Task<List<LeaderboardEntry>> QueryLeaderboard(
    ILeaderboardHandle handle,
    LeaderboardQueryType type,
    int startIndex,
    int count,
    CancellationToken cancelToken = default (CancellationToken));

  Task<List<LeaderboardEntry>> QueryLeaderboardForUsers(
    ILeaderboardHandle handle,
    IReadOnlyList<ulong> userIds,
    CancellationToken cancelToken = default (CancellationToken));

  int GetLeaderboardEntryCount(ILeaderboardHandle handle);
}
