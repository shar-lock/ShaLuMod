// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.DevConsole.ConsoleCommands.AbstractConsoleCmd
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.SourceGeneration;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable enable
namespace MegaCrit.Sts2.Core.DevConsole.ConsoleCommands;

[GenerateSubtypes]
public abstract class AbstractConsoleCmd
{
  public abstract string CmdName { get; }

  public abstract string Args { get; }

  public abstract string Description { get; }

  public abstract bool IsNetworked { get; }

  public virtual bool DebugOnly => true;

  public abstract CmdResult Process(Player? issuingPlayer, string[] args);

  public virtual CompletionResult GetArgumentCompletions(Player? player, string[] args)
  {
    return new CompletionResult()
    {
      Type = CompletionType.Argument,
      ArgumentContext = this.CmdName,
      ArgumentIndex = args.Length - 1,
      CommandPrefix = this.BuildPrefix(args)
    };
  }

  protected CompletionResult CompleteArgument(
    IEnumerable<string> candidates,
    string[] completedArgs,
    string partialArg,
    CompletionType type = CompletionType.Argument,
    Func<string, string, bool>? matchPredicate = null)
  {
    List<string> list = candidates.ToList<string>();
    if (matchPredicate == null)
      matchPredicate = (Func<string, string, bool>) ((candidate, partial) => candidate.StartsWith(partial, StringComparison.OrdinalIgnoreCase));
    List<string> filtered = !string.IsNullOrWhiteSpace(partialArg) ? list.Where<string>((Func<string, bool>) (c => matchPredicate(c, partialArg))).ToList<string>() : list;
    string prefix = this.BuildPrefix(completedArgs);
    string commonCompletion = this.CalculateCommonCompletion(filtered, prefix);
    return new CompletionResult()
    {
      Candidates = filtered,
      CommonPrefix = commonCompletion,
      Type = type,
      ArgumentContext = this.CmdName,
      ArgumentIndex = completedArgs.Length,
      CommandPrefix = prefix
    };
  }

  protected string BuildPrefix(string[] completedArgs)
  {
    return completedArgs.Length == 0 ? this.CmdName + " " : $"{this.CmdName} {string.Join(" ", completedArgs)} ";
  }

  protected static bool TryParseEnum<T>(string input, out T result) where T : struct, Enum
  {
    return Enum.TryParse<T>(input, true, out result) && Enum.IsDefined<T>(result);
  }

  private string CalculateCommonCompletion(List<string> filtered, string prefix)
  {
    if (filtered.Count == 0)
      return "";
    if (filtered.Count == 1)
      return $"{prefix}{filtered[0]} ";
    int num = filtered.Min<string>((Func<string, int>) (s => s.Length));
    string str = filtered[0];
    int length = 0;
    for (int i = 0; i < num; i++)
    {
      char c = str[i];
      if (filtered.All<string>((Func<string, bool>) (s => (int) char.ToLowerInvariant(s[i]) == (int) char.ToLowerInvariant(c))))
        length = i + 1;
      else
        break;
    }
    return length > 0 ? prefix + str.Substring(0, length) : "";
  }
}
