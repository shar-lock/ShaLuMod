// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.DevConsole.ConsoleCommands.RelicConsoleCmd
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Commands;
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

public class RelicConsoleCmd : AbstractConsoleCmd
{
  public override string CmdName => "relic";

  public override string Args => "[add|remove] <relic-id:string>";

  public override string Description => "Adds/Removes relic from player (add by default)";

  public override bool IsNetworked => true;

  public override CmdResult Process(Player? issuingPlayer, string[] args)
  {
    if (args.Length < 1)
      return new CmdResult(false, this.CmdName + " requires a relic name");
    if (issuingPlayer == null)
      return new CmdResult(false, "A run is currently not in progress!");
    string id;
    if (args[0].ToLowerInvariant().Equals("add") || args[0].ToLowerInvariant().Equals("remove"))
    {
      if (args.Length < 2)
        return new CmdResult(false, "You need to specify a relic!");
      id = args[1];
    }
    else
      id = args[0];
    RelicModel relicById1 = RelicConsoleCmd.GetRelicById(id);
    if (relicById1 == null)
      return new CmdResult(false, $"Unable to create relic '{id}'.");
    if (args[0].ToLowerInvariant().Equals("remove"))
    {
      RelicModel relicById2 = issuingPlayer.GetRelicById(relicById1.Id);
      if (relicById2 == null)
        return new CmdResult(false, "Unable to find relic in player!");
      TaskHelper.RunSafely(RelicCmd.Remove(relicById2));
      return new CmdResult(true, "Relic removed!");
    }
    TaskHelper.RunSafely((Task) RelicCmd.Obtain(relicById1.ToMutable(), issuingPlayer));
    return new CmdResult(true, $"Added relic '{id}'");
  }

  private static RelicModel? GetRelicById(string id)
  {
    id = id.ToUpperInvariant();
    List<RelicModel> list = ModelDb.AllRelics.Where<RelicModel>((Func<RelicModel, bool>) (r => r.Id.Entry.Contains(id))).ToList<RelicModel>();
    return list.FirstOrDefault<RelicModel>((Func<RelicModel, bool>) (r => r.Id.Entry == id)) ?? list.FirstOrDefault<RelicModel>((Func<RelicModel, bool>) (r => r.Id.Entry.StartsWith(id))) ?? list.FirstOrDefault<RelicModel>();
  }

  public override CompletionResult GetArgumentCompletions(Player? player, string[] args)
  {
    if (args.Length == 0 || args.Length == 1 && string.IsNullOrWhiteSpace(args[0]))
    {
      CompletionResult argumentCompletions = new CompletionResult();
      int capacity = 2;
      List<string> stringList = new List<string>(capacity);
      CollectionsMarshal.SetCount<string>(stringList, capacity);
      Span<string> span = CollectionsMarshal.AsSpan<string>(stringList);
      int num1 = 0;
      span[num1] = "add";
      int num2 = num1 + 1;
      span[num2] = "remove";
      argumentCompletions.Candidates = stringList;
      argumentCompletions.Type = CompletionType.Subcommand;
      argumentCompletions.ArgumentContext = this.CmdName;
      return argumentCompletions;
    }
    if (args.Length == 1)
    {
      List<string> candidates = new List<string>();
      string lowerInvariant = args[0].ToLowerInvariant();
      if ("add".StartsWith(lowerInvariant))
        candidates.Add("add");
      if ("remove".StartsWith(lowerInvariant))
        candidates.Add("remove");
      return candidates.Count > 0 ? this.CompleteArgument((IEnumerable<string>) candidates, Array.Empty<string>(), lowerInvariant, CompletionType.Subcommand) : this.CompleteArgument((IEnumerable<string>) ModelDb.AllRelics.Select<RelicModel, string>((Func<RelicModel, string>) (r => r.Id.Entry)).ToList<string>(), Array.Empty<string>(), args[0], matchPredicate: (Func<string, string, bool>) ((candidate, partialArg) => candidate.Contains(partialArg, StringComparison.OrdinalIgnoreCase)));
    }
    if (args.Length == 2)
      return this.CompleteArgument((IEnumerable<string>) ModelDb.AllRelics.Select<RelicModel, string>((Func<RelicModel, string>) (r => r.Id.Entry)).ToList<string>(), new string[1]
      {
        args[0]
      }, args[1], matchPredicate: (Func<string, string, bool>) ((candidate, partial) => candidate.Contains(partial, StringComparison.OrdinalIgnoreCase)));
    return new CompletionResult()
    {
      Type = CompletionType.Argument,
      ArgumentContext = this.CmdName
    };
  }
}
