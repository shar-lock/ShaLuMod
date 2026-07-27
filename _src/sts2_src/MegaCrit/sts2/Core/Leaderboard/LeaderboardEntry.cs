// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Leaderboard.LeaderboardEntry
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using System.Collections.Generic;

#nullable enable
namespace MegaCrit.Sts2.Core.Leaderboard;

public class LeaderboardEntry
{
  public int rank;
  public required string name;
  public ulong id;
  public int score;
  public List<ulong> userIds = new List<ulong>();
}
