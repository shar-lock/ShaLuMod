// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Platform.Steam.SteamLeaderboardStrategy
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Leaderboard;
using MegaCrit.Sts2.Core.Multiplayer.Serialization;
using MegaCrit.Sts2.Core.Multiplayer.Transport.Steam;
using Steamworks;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Platform.Steam;

public class SteamLeaderboardStrategy : ILeaderboardStrategy
{
  private readonly PacketWriter _writer = new PacketWriter();
  private readonly int[] _cachedUserDetails = new int[64 /*0x40*/];
  private readonly CSteamID[] _cachedUsers = new CSteamID[10];

  public PlatformType Platform => PlatformType.Steam;

  public async Task<ILeaderboardHandle> GetOrCreateLeaderboard(
    string name,
    CancellationToken cancelToken)
  {
    ILeaderboardHandle leaderboard;
    using (SteamCallResult<LeaderboardFindResult_t> callResult = new SteamCallResult<LeaderboardFindResult_t>(SteamUserStats.FindOrCreateLeaderboard(name, (ELeaderboardSortMethod) 2, (ELeaderboardDisplayType) 1), cancelToken))
    {
      LeaderboardFindResult_t task = await callResult.Task;
      if (cancelToken.IsCancellationRequested)
        throw new TaskCanceledException();
      leaderboard = task.m_bLeaderboardFound != (byte) 0 ? (ILeaderboardHandle) new SteamLeaderboardHandle()
      {
        leaderboard = task.m_hSteamLeaderboard
      } : throw new InvalidOperationException("Steam FindOrCreateLeaderboard returned 0 from leaderboard found!");
    }
    return leaderboard;
  }

  public async Task<ILeaderboardHandle?> GetLeaderboard(string name, CancellationToken cancelToken)
  {
    using (SteamCallResult<LeaderboardFindResult_t> callResult = new SteamCallResult<LeaderboardFindResult_t>(SteamUserStats.FindLeaderboard(name), cancelToken))
    {
      LeaderboardFindResult_t task = await callResult.Task;
      if (cancelToken.IsCancellationRequested || task.m_bLeaderboardFound == (byte) 0)
        return (ILeaderboardHandle) null;
      return (ILeaderboardHandle) new SteamLeaderboardHandle()
      {
        leaderboard = task.m_hSteamLeaderboard
      };
    }
  }

  public async Task UploadLocalScore(
    ILeaderboardHandle handleInterface,
    int score,
    IReadOnlyList<ulong> userIds)
  {
    int scoreDetailsCount;
    using (SteamCallResult<LeaderboardScoreUploaded_t> callResult = new SteamCallResult<LeaderboardScoreUploaded_t>(SteamUserStats.UploadLeaderboardScore(((SteamLeaderboardHandle) handleInterface).leaderboard, (ELeaderboardUploadScoreMethod) 2, score, this.PackScoreDetails(userIds, out scoreDetailsCount), scoreDetailsCount), SteamInitializer.DisconnectToken))
    {
      if ((await callResult.Task).m_bSuccess == (byte) 0)
        throw new IOException("Steam score upload failed!");
    }
  }

  public async Task<List<LeaderboardEntry>> QueryLeaderboard(
    ILeaderboardHandle handleInterface,
    LeaderboardQueryType type,
    int startIndex,
    int count,
    CancellationToken cancelToken = default (CancellationToken))
  {
    SteamLeaderboardHandle leaderboardHandle = (SteamLeaderboardHandle) handleInterface;
    int num1 = startIndex;
    if (type == LeaderboardQueryType.Global)
      ++num1;
    using (SteamCallResult<LeaderboardScoresDownloaded_t> callResult = new SteamCallResult<LeaderboardScoresDownloaded_t>(SteamUserStats.DownloadLeaderboardEntries(leaderboardHandle.leaderboard, this.LeaderboardDataRequestTypeFrom(type), num1, num1 + count - 1), cancelToken))
    {
      LeaderboardScoresDownloaded_t task = await callResult.Task;
      List<LeaderboardEntry> leaderboardEntryList = new List<LeaderboardEntry>();
      if (cancelToken.IsCancellationRequested)
        return new List<LeaderboardEntry>();
      int num2 = 0;
      int num3 = task.m_cEntryCount;
      if (type == LeaderboardQueryType.FriendsOnly)
      {
        num2 = startIndex;
        num3 = Math.Min(startIndex + count, task.m_cEntryCount);
      }
      if (num2 >= num3)
        return new List<LeaderboardEntry>();
      for (int index = num2; index < num3; ++index)
      {
        LeaderboardEntry_t entry;
        if (!SteamUserStats.GetDownloadedLeaderboardEntry(task.m_hSteamLeaderboardEntries, index, ref entry, this._cachedUserDetails, this._cachedUserDetails.Length))
          throw new InvalidOperationException($"Failed to download leaderboard entry at index {index}. Start result index: {num2}. result count: {num3}");
        leaderboardEntryList.Add(this.LeaderboardEntryFromSteamType(entry));
      }
      return leaderboardEntryList;
    }
  }

  public async Task<List<LeaderboardEntry>> QueryLeaderboardForUsers(
    ILeaderboardHandle handleInterface,
    IReadOnlyList<ulong> userIds,
    CancellationToken cancelToken = default (CancellationToken))
  {
    SteamLeaderboardHandle leaderboardHandle = (SteamLeaderboardHandle) handleInterface;
    for (int index = 0; index < userIds.Count; ++index)
      this._cachedUsers[index] = new CSteamID(userIds[index]);
    using (SteamCallResult<LeaderboardScoresDownloaded_t> callResult = new SteamCallResult<LeaderboardScoresDownloaded_t>(SteamUserStats.DownloadLeaderboardEntriesForUsers(leaderboardHandle.leaderboard, this._cachedUsers, userIds.Count), cancelToken))
    {
      LeaderboardScoresDownloaded_t task = await callResult.Task;
      List<LeaderboardEntry> leaderboardEntryList = new List<LeaderboardEntry>();
      if (cancelToken.IsCancellationRequested)
        return new List<LeaderboardEntry>();
      for (int index = 0; index < task.m_cEntryCount; ++index)
      {
        LeaderboardEntry_t entry;
        if (!SteamUserStats.GetDownloadedLeaderboardEntry(task.m_hSteamLeaderboardEntries, index, ref entry, this._cachedUserDetails, this._cachedUserDetails.Length))
          throw new InvalidOperationException($"Failed to download leaderboard entry at index {index}");
        leaderboardEntryList.Add(this.LeaderboardEntryFromSteamType(entry));
      }
      return leaderboardEntryList;
    }
  }

  public int GetLeaderboardEntryCount(ILeaderboardHandle handleInterface)
  {
    return SteamUserStats.GetLeaderboardEntryCount(((SteamLeaderboardHandle) handleInterface).leaderboard);
  }

  private LeaderboardEntry LeaderboardEntryFromSteamType(LeaderboardEntry_t entry)
  {
    return new LeaderboardEntry()
    {
      id = entry.m_steamIDUser.m_SteamID,
      name = SteamFriends.GetFriendPersonaName(entry.m_steamIDUser),
      rank = entry.m_nGlobalRank - 1,
      score = entry.m_nScore,
      userIds = this.UnpackScoreDetails(this._cachedUserDetails, entry.m_cDetails)
    };
  }

  private ELeaderboardDataRequest LeaderboardDataRequestTypeFrom(LeaderboardQueryType type)
  {
    ELeaderboardDataRequest eleaderboardDataRequest;
    switch (type)
    {
      case LeaderboardQueryType.None:
        throw new InvalidOperationException("LeaderboardQueryType.None should never be passed to an API!");
      case LeaderboardQueryType.Global:
        eleaderboardDataRequest = (ELeaderboardDataRequest) 0;
        break;
      case LeaderboardQueryType.AroundUser:
        eleaderboardDataRequest = (ELeaderboardDataRequest) 1;
        break;
      case LeaderboardQueryType.FriendsOnly:
        eleaderboardDataRequest = (ELeaderboardDataRequest) 2;
        break;
      default:
        // ISSUE: reference to a compiler-generated method
        \u003CPrivateImplementationDetails\u003E.ThrowSwitchExpressionException((object) type);
        break;
    }
    return eleaderboardDataRequest;
  }

  private int[] PackScoreDetails(IReadOnlyList<ulong> userIds, out int scoreDetailsCount)
  {
    this._writer.Reset();
    foreach (ulong userId in (IEnumerable<ulong>) userIds)
      this._writer.WriteULong(userId);
    scoreDetailsCount = (int) Math.Ceiling((double) this._writer.BitPosition / 32.0);
    if (scoreDetailsCount > 64 /*0x40*/)
      throw new InvalidOperationException($"Tried to write {userIds.Count} user IDs into {scoreDetailsCount} integers, exceeding the max limit ({64 /*0x40*/})!");
    for (int index = 0; index < scoreDetailsCount; ++index)
      this._cachedUserDetails[index] = BitConverter.ToInt32(this._writer.Buffer, index * 4);
    return this._cachedUserDetails;
  }

  private List<ulong> UnpackScoreDetails(int[] scoreDetails, int scoreDetailsCount)
  {
    this._writer.Reset();
    for (int index = 0; index < scoreDetailsCount; ++index)
      this._writer.WriteInt(scoreDetails[index]);
    PacketReader packetReader = new PacketReader();
    packetReader.Reset(this._writer.Buffer);
    int num = (int) Math.Floor((double) this._writer.BitPosition / 64.0);
    List<ulong> ulongList = new List<ulong>();
    for (int index = 0; index < num; ++index)
      ulongList.Add(packetReader.ReadULong());
    return ulongList;
  }
}
