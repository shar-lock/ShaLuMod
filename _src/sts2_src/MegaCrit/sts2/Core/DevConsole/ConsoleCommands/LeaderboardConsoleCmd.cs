// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.DevConsole.ConsoleCommands.LeaderboardConsoleCmd
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Daily;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Leaderboard;
using MegaCrit.Sts2.Core.Random;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.DevConsole.ConsoleCommands;

public class LeaderboardConsoleCmd : AbstractConsoleCmd
{
  private static readonly List<string> _validOptions;

  public override string CmdName => "leaderboard";

  public override string Args => "[option:string] [name:string] <score:int> [count:int]";

  public override string Description
  {
    get
    {
      return "Adds scores to the leaderboard. Option can be upload|random. If random, <count> random scores will be uploaded to leaderboard <name>. If upload, one score will be uploaded to leaderboard <name> with <score>.";
    }
  }

  public override bool IsNetworked => false;

  public override CmdResult Process(Player? issuingPlayer, string[] args)
  {
    if (args.Length == 0)
      return new CmdResult(false, "Option must be specified (random or upload).");
    string str = args[0];
    string leaderboardName = DailyRunUtility.GetLeaderboardName(DateTimeOffset.Now, 1);
    if (args.Length > 1 && args[1] != "-")
      leaderboardName = args[1];
    switch (str)
    {
      case "random":
        int result1 = 100;
        if (args.Length >= 3 && !int.TryParse(args[2], out result1))
          return new CmdResult(false, "Count must be a valid integer.");
        TaskHelper.RunSafely(this.UploadRandomScores(leaderboardName, result1));
        return new CmdResult(true, $"Adding {result1} entries to leaderboard {leaderboardName}");
      case "upload":
        if (args.Length < 3)
          return new CmdResult(false, "Score must be specified when upload is the option.\nUse - as the leaderboard name for the default.");
        int result2;
        if (!int.TryParse(args[2], out result2))
          return new CmdResult(false, "Score must be a valid integer.");
        TaskHelper.RunSafely(this.UploadScore(leaderboardName, result2));
        return new CmdResult(true, $"Uploading score {result2} to leaderboard {leaderboardName}");
      default:
        return new CmdResult(false, $"Invalid option {str}, must be upload or random");
    }
  }

  private async Task UploadRandomScores(string leaderboardName, int count)
  {
    ILeaderboardHandle leaderboard = await LeaderboardManager.GetOrCreateLeaderboard(leaderboardName);
    Rng rng = new Rng(Rng.Chaotic.NextUnsignedLong());
    for (int index = 0; index < count; ++index)
      LeaderboardManager.DebugAddEntry(leaderboard, new LeaderboardEntry()
      {
        id = (ulong) rng.NextUnsignedInt(),
        name = rng.NextUnsignedInt().ToString(),
        score = rng.NextInt()
      });
  }

  private async Task UploadScore(string leaderboardName, int score)
  {
    await LeaderboardManager.UploadLocalScore(await LeaderboardManager.GetOrCreateLeaderboard(leaderboardName), score, (IReadOnlyList<ulong>) Array.Empty<ulong>());
  }

  public override CompletionResult GetArgumentCompletions(Player? player, string[] args)
  {
    if (args.Length <= 1)
    {
      string partial = args.Length != 0 ? args[0] : "";
      List<string> stringList = !string.IsNullOrWhiteSpace(partial) ? LeaderboardConsoleCmd._validOptions.Where<string>((Func<string, bool>) (option => option.Contains(partial, StringComparison.OrdinalIgnoreCase))).ToList<string>() : LeaderboardConsoleCmd._validOptions;
      return new CompletionResult()
      {
        Candidates = stringList,
        Type = CompletionType.Argument,
        ArgumentContext = this.CmdName
      };
    }
    return new CompletionResult()
    {
      Type = CompletionType.Argument,
      ArgumentContext = this.CmdName
    };
  }

  static LeaderboardConsoleCmd()
  {
    int capacity = 2;
    List<string> stringList = new List<string>(capacity);
    CollectionsMarshal.SetCount<string>(stringList, capacity);
    Span<string> span = CollectionsMarshal.AsSpan<string>(stringList);
    int num1 = 0;
    span[num1] = "upload";
    int num2 = num1 + 1;
    span[num2] = "random";
    LeaderboardConsoleCmd._validOptions = stringList;
  }
}
