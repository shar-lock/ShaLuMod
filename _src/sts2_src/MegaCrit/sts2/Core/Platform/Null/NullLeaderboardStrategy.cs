// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Platform.Null.NullLeaderboardStrategy
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Leaderboard;
using MegaCrit.Sts2.Core.Saves;
using MegaCrit.Sts2.Core.TestSupport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Platform.Null;

public class NullLeaderboardStrategy : ILeaderboardStrategy
{
  private readonly GodotFileIo _fileIo = new GodotFileIo(UserDataPathProvider.GetAccountScopedBasePath((string) null));
  private List<NullLeaderboard> _leaderboards;

  public PlatformType Platform => PlatformType.None;

  public NullLeaderboardStrategy() => this._leaderboards = this.Read();

  public Task<ILeaderboardHandle?> GetLeaderboard(string name, CancellationToken cancelToken)
  {
    foreach (NullLeaderboard leaderboard in this._leaderboards)
    {
      if (leaderboard.name == name)
        return Task.FromResult<ILeaderboardHandle>((ILeaderboardHandle) new NullLeaderboardHandle()
        {
          leaderboard = leaderboard
        });
    }
    return Task.FromResult<ILeaderboardHandle>((ILeaderboardHandle) null);
  }

  public async Task<ILeaderboardHandle> GetOrCreateLeaderboard(
    string name,
    CancellationToken cancelToken)
  {
    await this.CheckRefreshLeaderboard((NullLeaderboardHandle) null, PlatformUtil.GetLocalPlayerId(PlatformType.None));
    ILeaderboardHandle leaderboard = await this.GetLeaderboard(name, cancelToken);
    if (leaderboard != null)
      return leaderboard;
    NullLeaderboard nullLeaderboard = new NullLeaderboard()
    {
      name = name,
      entries = new List<NullLeaderboardFileEntry>()
    };
    this._leaderboards.Add(nullLeaderboard);
    this.Write(this._leaderboards);
    return (ILeaderboardHandle) new NullLeaderboardHandle()
    {
      leaderboard = nullLeaderboard
    };
  }

  public async Task UploadLocalScore(
    ILeaderboardHandle handleInterface,
    int score,
    IReadOnlyList<ulong> userIds)
  {
    NullLeaderboardHandle handle = (NullLeaderboardHandle) handleInterface;
    ulong id = PlatformUtil.GetLocalPlayerId(PlatformType.None);
    await this.CheckRefreshLeaderboard(handle, id);
    handle.leaderboard.entries.Add(new NullLeaderboardFileEntry()
    {
      name = PlatformUtil.GetPlayerNameRaw(PlatformType.None, id),
      id = id,
      score = score,
      userIds = userIds.ToList<ulong>()
    });
    this.Write(this._leaderboards);
    handle = (NullLeaderboardHandle) null;
  }

  public async Task<List<LeaderboardEntry>> QueryLeaderboard(
    ILeaderboardHandle handleInterface,
    LeaderboardQueryType type,
    int startIndex,
    int count,
    CancellationToken cancelToken)
  {
    NullLeaderboardHandle handle = (NullLeaderboardHandle) handleInterface;
    ulong id = PlatformUtil.GetLocalPlayerId(PlatformType.None);
    await this.CheckRefreshLeaderboard(handle, id);
    switch (type)
    {
      case LeaderboardQueryType.Global:
      case LeaderboardQueryType.FriendsOnly:
        int startIndex1 = startIndex;
        List<NullLeaderboardFileEntry> entries1 = handle.leaderboard.entries;
        int num1 = startIndex;
        int num2 = num1;
        int num3 = Math.Min(startIndex + count, handle.leaderboard.entries.Count) - num1;
        List<NullLeaderboardFileEntry> nullEntries1 = entries1.Slice(num2, num3);
        return this.ToLeaderboardEntries(startIndex1, nullEntries1);
      case LeaderboardQueryType.AroundUser:
        int index = handle.leaderboard.entries.FindIndex((Predicate<NullLeaderboardFileEntry>) (e => (long) e.id == (long) id));
        if (index < 0)
          return new List<LeaderboardEntry>();
        int num4 = index + startIndex;
        int num5 = Math.Max(0, num4);
        int num6 = Math.Min(handle.leaderboard.entries.Count, num4 + count);
        if (num5 >= num6)
          return new List<LeaderboardEntry>();
        int startIndex2 = num5;
        List<NullLeaderboardFileEntry> entries2 = handle.leaderboard.entries;
        int num7 = num5;
        int num8 = num7;
        int num9 = num6 - num7;
        List<NullLeaderboardFileEntry> nullEntries2 = entries2.Slice(num8, num9);
        return this.ToLeaderboardEntries(startIndex2, nullEntries2);
      default:
        throw new ArgumentOutOfRangeException(nameof (type), (object) type, (string) null);
    }
  }

  public async Task<List<LeaderboardEntry>> QueryLeaderboardForUsers(
    ILeaderboardHandle handleInterface,
    IReadOnlyList<ulong> userIds,
    CancellationToken cancelToken)
  {
    NullLeaderboardHandle handle = (NullLeaderboardHandle) handleInterface;
    ulong localPlayerId = PlatformUtil.GetLocalPlayerId(PlatformType.None);
    await this.CheckRefreshLeaderboard(handle, localPlayerId);
    List<LeaderboardEntry> leaderboardEntryList1 = new List<LeaderboardEntry>();
    for (int index = 0; index < handle.leaderboard.entries.Count; ++index)
    {
      NullLeaderboardFileEntry entry = handle.leaderboard.entries[index];
      if (userIds.Contains<ulong>(entry.id))
        leaderboardEntryList1.Add(this.ToLeaderboardEntry(index, entry));
    }
    List<LeaderboardEntry> leaderboardEntryList2 = leaderboardEntryList1;
    handle = (NullLeaderboardHandle) null;
    return leaderboardEntryList2;
  }

  public int GetLeaderboardEntryCount(ILeaderboardHandle handleInterface)
  {
    return ((NullLeaderboardHandle) handleInterface).leaderboard.entries.Count;
  }

  public void DebugAddEntry(ILeaderboardHandle handleInterface, LeaderboardEntry entry)
  {
    NullLeaderboardHandle leaderboardHandle = (NullLeaderboardHandle) handleInterface;
    NullLeaderboardFileEntry leaderboardFileEntry = new NullLeaderboardFileEntry()
    {
      name = entry.name,
      id = entry.id,
      score = entry.score
    };
    leaderboardHandle.leaderboard.entries.Add(leaderboardFileEntry);
    leaderboardHandle.leaderboard.entries.Sort((Comparison<NullLeaderboardFileEntry>) ((x, y) => y.score.CompareTo(x.score)));
    this.Write(this._leaderboards);
  }

  private List<LeaderboardEntry> ToLeaderboardEntries(
    int startIndex,
    List<NullLeaderboardFileEntry> nullEntries)
  {
    List<LeaderboardEntry> leaderboardEntries = new List<LeaderboardEntry>();
    for (int index = 0; index < nullEntries.Count; ++index)
      leaderboardEntries.Add(this.ToLeaderboardEntry(startIndex + index, nullEntries[index]));
    return leaderboardEntries;
  }

  private LeaderboardEntry ToLeaderboardEntry(int rank, NullLeaderboardFileEntry nullEntry)
  {
    return new LeaderboardEntry()
    {
      name = nullEntry.name,
      score = nullEntry.score,
      rank = rank,
      id = 1,
      userIds = nullEntry.userIds
    };
  }

  private List<NullLeaderboard> Read()
  {
    if (TestMode.IsOn)
      return new List<NullLeaderboard>();
    string json = this._fileIo.ReadFile("leaderboards.save");
    if (json == null)
      return new List<NullLeaderboard>();
    ReadSaveResult<NullLeaderboardFile> readSaveResult = JsonSerializationUtility.FromJson<NullLeaderboardFile>(json);
    return readSaveResult.Status != ReadSaveStatus.Success ? new List<NullLeaderboard>() : readSaveResult.SaveData.leaderboards;
  }

  private void Write(List<NullLeaderboard> leaderboards)
  {
    if (TestMode.IsOn)
      return;
    this._fileIo.WriteFile("leaderboards.save", JsonSerializer.Serialize<NullLeaderboardFile>(new NullLeaderboardFile()
    {
      leaderboards = leaderboards
    }, JsonSerializationUtility.GetTypeInfo<NullLeaderboardFile>()));
  }

  private async Task CheckRefreshLeaderboard(NullLeaderboardHandle? handle, ulong id)
  {
    if (!CommandLineHelper.HasArg("fastmp"))
      ;
    else
    {
      await Task.Delay((int) ((double) id * 0.5));
      this._leaderboards = this.Read();
      if (handle == null)
        ;
      else
        handle.leaderboard = this._leaderboards.First<NullLeaderboard>((Func<NullLeaderboard, bool>) (l => l.name == handle.leaderboard.name));
    }
  }
}
