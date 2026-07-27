// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.DevConsole.ConsoleCommands.AchievementConsoleCmd
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Achievements;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Platform;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

#nullable enable
namespace MegaCrit.Sts2.Core.DevConsole.ConsoleCommands;

public class AchievementConsoleCmd : AbstractConsoleCmd
{
  public override string CmdName => "achievement";

  public override string Args => "<operation:string> [id:string]";

  public override string Description
  {
    get
    {
      return "Unlocks or revokes an achievement. If no achievement is provided, all achievements are unlocked or revoked.";
    }
  }

  public override bool IsNetworked => false;

  public override CmdResult Process(Player? issuingPlayer, string[] args)
  {
    int capacity = 3;
    List<string> stringList = new List<string>(capacity);
    CollectionsMarshal.SetCount<string>(stringList, capacity);
    Span<string> span = CollectionsMarshal.AsSpan<string>(stringList);
    int num1 = 0;
    span[num1] = "unlock";
    int num2 = num1 + 1;
    span[num2] = "revoke";
    int num3 = num2 + 1;
    span[num3] = "check";
    List<string> values = stringList;
    if (args.Length < 1 || !values.Contains(args[0]))
      return new CmdResult(false, $"First argument must be one of: {string.Join(",", (IEnumerable<string>) values)}.\n{this.Args}");
    Achievement? nullable = new Achievement?();
    if (args.Length >= 2)
    {
      string lowerInvariant = args[1].ToLowerInvariant();
      foreach (Achievement achievement in Enum.GetValues<Achievement>())
      {
        if (StringHelper.SnakeCase(achievement.ToString()) == lowerInvariant)
        {
          nullable = new Achievement?(achievement);
          break;
        }
      }
      if (!nullable.HasValue)
        return new CmdResult(false, $"Achievement '{args[1]}' unrecognized.\n{this.Args}");
    }
    if (args[0] == "unlock")
    {
      if (nullable.HasValue)
      {
        AchievementsUtil.Unlock(nullable.Value, issuingPlayer);
        return new CmdResult(true, $"Unlocked {nullable.Value}");
      }
      foreach (Achievement achievement in Enum.GetValues<Achievement>())
        AchievementsUtil.Unlock(achievement, issuingPlayer);
      return new CmdResult(true, "Unlocked all achievements");
    }
    if (args[0] == "revoke")
    {
      if (nullable.HasValue)
      {
        AchievementsUtil.Revoke(nullable.Value);
        return new CmdResult(true, $"Revoked {nullable.Value}");
      }
      foreach (Achievement achievement in Enum.GetValues<Achievement>())
        AchievementsUtil.Revoke(achievement);
      return new CmdResult(true, "Revoked all achievements");
    }
    if (!(args[0] == "check"))
      throw new NotImplementedException();
    if (!nullable.HasValue)
      return new CmdResult(false, "Achievement name must be provided");
    if (AchievementsUtil.IsUnlocked(nullable.Value))
      return new CmdResult(true, $"{nullable.Value} is unlocked");
    return new CmdResult(true, $"{nullable.Value} is not unlocked");
  }

  public override CompletionResult GetArgumentCompletions(Player? player, string[] args)
  {
    if (args.Length <= 1)
      return this.CompleteArgument((IEnumerable<string>) new List<string>()
      {
        "unlock",
        "revoke",
        "check"
      }, Array.Empty<string>(), ((IEnumerable<string>) args).FirstOrDefault<string>() ?? "", CompletionType.Subcommand);
    if (args.Length == 2)
    {
      List<string> list = ((IEnumerable<Achievement>) Enum.GetValues<Achievement>()).Select<Achievement, string>((Func<Achievement, string>) (a => StringHelper.SnakeCase(a.ToString()))).ToList<string>();
      list.Sort();
      return this.CompleteArgument((IEnumerable<string>) list, new string[1]
      {
        args[0]
      }, args[1]);
    }
    return new CompletionResult()
    {
      Type = CompletionType.Argument,
      ArgumentContext = this.CmdName
    };
  }
}
