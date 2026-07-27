// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.DevConsole.ConsoleCommands.KillConsoleCmd
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.DevConsole.ConsoleCommands;

public class KillConsoleCmd : AbstractConsoleCmd
{
  public override string CmdName => "kill";

  public override string Args => "<target-index:int>|'all'";

  public override string Description
  {
    get
    {
      return "Will kill one target if the index is given, or all if 'all', or the first if no arguments.";
    }
  }

  public override bool IsNetworked => true;

  public override CmdResult Process(Player? issuingPlayer, string[] args)
  {
    if (!CombatManager.Instance.IsInProgress)
      return new CmdResult(false, "This doesn't appear to be a combat!");
    List<Creature> creatureList = new List<Creature>();
    IReadOnlyList<Creature> list = (IReadOnlyList<Creature>) CombatManager.Instance.DebugOnlyGetState().Enemies.ToList<Creature>();
    if (args.Length == 0)
      creatureList.Add(list[0]);
    else if (args[0].Equals("all"))
    {
      foreach (Creature creature in (IEnumerable<Creature>) list)
        creatureList.Add(creature);
    }
    else
    {
      int result;
      if (!int.TryParse(args[0], out result))
        return new CmdResult(false, $"Invalid argument '{args[0]}'. Use a target index or 'all'.");
      if (result < 0 || result >= list.Count)
        return new CmdResult(false, $"Invalid target index {result}. Valid range: 0-{list.Count - 1}");
      creatureList.Add(list[result]);
    }
    IEnumerable<string> values = creatureList.Select<Creature, MonsterModel>((Func<Creature, MonsterModel>) (c => c.Monster)).Where<MonsterModel>((Func<MonsterModel, bool>) (m => m != null)).Select<MonsterModel, string>((Func<MonsterModel, string>) (m => m.Id.Entry.ToString()));
    TaskHelper.RunSafely(this.DoKill(creatureList));
    return new CmdResult(true, $"Killed: [{string.Join(",", values)}]");
  }

  private async Task DoKill(List<Creature> toKill)
  {
    foreach (Creature creature in toKill)
      await CreatureCmd.Kill(creature);
    int num = await CombatManager.Instance.CheckWinCondition() ? 1 : 0;
  }

  public override CompletionResult GetArgumentCompletions(Player? player, string[] args)
  {
    if (args.Length <= 1)
    {
      int capacity = 1;
      List<string> stringList = new List<string>(capacity);
      CollectionsMarshal.SetCount<string>(stringList, capacity);
      CollectionsMarshal.AsSpan<string>(stringList)[0] = "all";
      List<string> candidates = stringList;
      if (CombatManager.Instance.IsInProgress)
      {
        IReadOnlyList<Creature> enemies = CombatManager.Instance.DebugOnlyGetState()?.Enemies;
        if (enemies != null)
        {
          for (int index = 0; index < enemies.Count; ++index)
            candidates.Add(index.ToString());
        }
      }
      return this.CompleteArgument((IEnumerable<string>) candidates, Array.Empty<string>(), ((IEnumerable<string>) args).FirstOrDefault<string>() ?? "");
    }
    return new CompletionResult()
    {
      Type = CompletionType.Argument,
      ArgumentContext = this.CmdName
    };
  }
}
