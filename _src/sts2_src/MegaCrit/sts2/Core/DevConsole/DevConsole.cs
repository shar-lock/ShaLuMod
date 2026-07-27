// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.DevConsole.DevConsole
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.DevConsole.ConsoleCommands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.Saves;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

#nullable enable
namespace MegaCrit.Sts2.Core.DevConsole;

public class DevConsole
{
  private readonly Dictionary<string, AbstractConsoleCmd> _commands = new Dictionary<string, AbstractConsoleCmd>();
  public readonly FixedSizedQueue<string> history;
  public int historyIndex;
  private readonly string _historyFilePath;

  public DevConsole(bool shouldAllowDebugCommands)
  {
    this._historyFilePath = UserDataPathProvider.GetAccountScopedBasePath("console_history.log");
    this.history = new FixedSizedQueue<string>(40);
    this.LoadCommandHistory();
    foreach (Type type in AbstractConsoleCmdSubtypes.All.Concat<Type>(ReflectionHelper.GetSubtypesInMods<AbstractConsoleCmd>()))
    {
      AbstractConsoleCmd instance = (AbstractConsoleCmd) Activator.CreateInstance(type);
      if (!instance.DebugOnly || shouldAllowDebugCommands)
        this.RegisterCommand(instance);
    }
  }

  private void RegisterCommand(AbstractConsoleCmd consoleCmd)
  {
    this._commands[consoleCmd.CmdName] = consoleCmd;
  }

  private void LoadCommandHistory()
  {
    using (FileAccess fileAccess = FileAccess.Open(this._historyFilePath, (FileAccess.ModeFlags) 1L))
    {
      if (fileAccess == null)
        return;
      while (fileAccess.GetPosition() < fileAccess.GetLength())
        this.history.Add(fileAccess.GetLine());
    }
  }

  private void SaveCommandHistory()
  {
    using (FileAccess fileAccess = FileAccess.Open(this._historyFilePath, (FileAccess.ModeFlags) 2L))
    {
      if (fileAccess == null)
        return;
      foreach (string str in (List<string>) this.history)
        fileAccess.StoreLine(str);
    }
  }

  public CompletionResult GetCompletionResults(string inputBuffer)
  {
    string[] source = inputBuffer.EndsWith(' ') ? ((IEnumerable<string>) inputBuffer.Trim().Split(' ', StringSplitOptions.None)).Append<string>(string.Empty).ToArray<string>() : inputBuffer.Trim().Split(' ', StringSplitOptions.None);
    string str1 = ((IEnumerable<string>) source).First<string>();
    if (this._commands.ContainsKey(str1) && source.Length == 1)
    {
      CompletionResult completionResults = new CompletionResult();
      int capacity = 1;
      List<string> stringList = new List<string>(capacity);
      CollectionsMarshal.SetCount<string>(stringList, capacity);
      CollectionsMarshal.AsSpan<string>(stringList)[0] = str1;
      completionResults.Candidates = stringList;
      completionResults.CommonPrefix = str1 + " ";
      completionResults.Type = CompletionType.Command;
      completionResults.CommandPrefix = "";
      completionResults.ArgumentIndex = -1;
      completionResults.ArgumentContext = "";
      return completionResults;
    }
    AbstractConsoleCmd abstractConsoleCmd;
    if (this._commands.TryGetValue(str1.Trim().ToLowerInvariant(), out abstractConsoleCmd))
    {
      string[] array1 = ((IEnumerable<string>) source).Skip<string>(1).ToArray<string>();
      string str2 = str1 + " ";
      if (array1.Length != 0)
      {
        string[] array2 = ((IEnumerable<string>) array1).Take<string>(array1.Length - 1).ToArray<string>();
        if (array2.Length != 0)
          str2 = $"{str2}{string.Join(" ", array2)} ";
      }
      CompletionType completionType = array1.Length <= 1 ? CompletionType.Subcommand : CompletionType.Argument;
      int num = array1.Length - 1;
      CompletionResult argumentCompletions = abstractConsoleCmd.GetArgumentCompletions(LocalContext.GetMe((IPlayerCollection) RunManager.Instance.DebugOnlyGetState()), array1);
      if (argumentCompletions.Candidates.Count > 0)
      {
        argumentCompletions.Type = completionType;
        argumentCompletions.CommandPrefix = str2;
        argumentCompletions.ArgumentIndex = num;
        argumentCompletions.ArgumentContext = str1;
        if (string.IsNullOrEmpty(argumentCompletions.CommonPrefix))
          argumentCompletions.CommonPrefix = inputBuffer;
      }
      else
      {
        argumentCompletions.Type = completionType;
        argumentCompletions.CommandPrefix = str2;
        argumentCompletions.ArgumentIndex = num;
        argumentCompletions.ArgumentContext = str1;
      }
      return argumentCompletions;
    }
    if (source.Length > 1)
      return new CompletionResult()
      {
        Type = CompletionType.Command,
        CommandPrefix = "",
        ArgumentIndex = -1,
        ArgumentContext = ""
      };
    List<string> list = this._commands.Values.Select<AbstractConsoleCmd, string>((Func<AbstractConsoleCmd, string>) (s => s.CmdName)).ToList<string>();
    return MegaCrit.Sts2.Core.DevConsole.DevConsole.GetCompletionResultsFromTokens(str1, list, inputBuffer);
  }

  public static string? CalculateGhostText(string inputText, CompletionResult result)
  {
    if (result.Candidates.Count != 1 || string.IsNullOrEmpty(result.CommonPrefix))
      return (string) null;
    string commonPrefix = result.CommonPrefix;
    if (!commonPrefix.StartsWith(inputText, StringComparison.OrdinalIgnoreCase))
      return (string) null;
    string str = commonPrefix.Substring(inputText.Length);
    return new string(' ', inputText.Length) + str;
  }

  private static CompletionResult GetCompletionResultsFromTokens(
    string partialToken,
    List<string> possibleTokens,
    string originalInput)
  {
    List<string> stringList1 = new List<string>();
    List<string> stringList2 = new List<string>();
    foreach (string possibleToken in possibleTokens)
    {
      if (possibleToken.StartsWith(partialToken, StringComparison.OrdinalIgnoreCase))
        stringList1.Add(possibleToken);
      else if (possibleToken.Contains(partialToken, StringComparison.OrdinalIgnoreCase))
        stringList2.Add(possibleToken);
    }
    List<string> stringList3 = stringList1;
    List<string> stringList4 = stringList2;
    int capacity = stringList3.Count + stringList4.Count;
    List<string> stringList5 = new List<string>(capacity);
    CollectionsMarshal.SetCount<string>(stringList5, capacity);
    Span<string> span1 = CollectionsMarshal.AsSpan<string>(stringList5);
    int num1 = 0;
    Span<string> span2 = CollectionsMarshal.AsSpan<string>(stringList3);
    span2.CopyTo(span1.Slice(num1, span2.Length));
    int num2 = num1 + span2.Length;
    Span<string> span3 = CollectionsMarshal.AsSpan<string>(stringList4);
    span3.CopyTo(span1.Slice(num2, span3.Length));
    int num3 = num2 + span3.Length;
    List<string> stringList6 = stringList5;
    if (stringList6.Count == 0)
      return new CompletionResult()
      {
        Type = CompletionType.Command,
        CommandPrefix = "",
        ArgumentIndex = -1,
        ArgumentContext = ""
      };
    if (stringList6.Count == 1)
    {
      string str = stringList1.Count == 1 ? MegaCrit.Sts2.Core.DevConsole.DevConsole.ReplaceLastToken(originalInput, stringList6.First<string>()) + " " : originalInput;
      return new CompletionResult()
      {
        Candidates = stringList6,
        CommonPrefix = str,
        Type = CompletionType.Command,
        CommandPrefix = "",
        ArgumentIndex = -1,
        ArgumentContext = ""
      };
    }
    return new CompletionResult()
    {
      Candidates = stringList6,
      CommonPrefix = MegaCrit.Sts2.Core.DevConsole.DevConsole.ReplaceLastToken(originalInput, MegaCrit.Sts2.Core.DevConsole.DevConsole.LongestCommonSubstring(stringList6)),
      Type = CompletionType.Command,
      CommandPrefix = "",
      ArgumentIndex = -1,
      ArgumentContext = ""
    };
  }

  private static string ReplaceLastToken(string text, string replacement)
  {
    if (string.IsNullOrWhiteSpace(replacement))
      return text;
    string[] source = text.Trim().Split(' ', StringSplitOptions.None);
    string[] strArray = source.Length != 0 ? ((IEnumerable<string>) source).Take<string>(source.Length - 1).ToArray<string>() : Array.Empty<string>();
    return (strArray.Length != 0 ? string.Join(" ", strArray) + " " : string.Empty) + replacement;
  }

  private static string LongestCommonSubstring(List<string> cmdCandidates)
  {
    int num = cmdCandidates.Select<string, int>((Func<string, int>) (cmd => cmd.Length)).Min();
    string str = cmdCandidates.First<string>();
    for (int index = 0; index < int.MaxValue; ++index)
    {
      foreach (string cmdCandidate in cmdCandidates)
      {
        if (num == index || (int) char.ToLowerInvariant(cmdCandidate[index]) != (int) char.ToLowerInvariant(str[index]))
          return cmdCandidate.Substring(0, index);
      }
    }
    return string.Empty;
  }

  public CmdResult ProcessCommand(string inputValue)
  {
    inputValue = inputValue.Trim();
    this.history.Enqueue(inputValue);
    this.historyIndex = 0;
    this.SaveCommandHistory();
    string[] tokens = inputValue.Split(' ', StringSplitOptions.None);
    Player me = LocalContext.GetMe((IPlayerCollection) RunManager.Instance.DebugOnlyGetState());
    AbstractConsoleCmd abstractConsoleCmd;
    if (!RunManager.Instance.IsSingleplayerOrFakeMultiplayer && this._commands.TryGetValue(tokens[0].ToLowerInvariant(), out abstractConsoleCmd) && abstractConsoleCmd.IsNetworked && me != null)
    {
      RunManager.Instance.ActionQueueSynchronizer.RequestEnqueue((GameAction) new ConsoleCmdGameAction(me, inputValue, CombatManager.Instance.IsInProgress));
      return new CmdResult(true, $"Enqueued {tokens[0]} command: '{inputValue}'");
    }
    CmdResult cmdResult = this.ProcessCommandInternal(me, tokens);
    if (cmdResult.task != null)
      TaskHelper.RunSafely(cmdResult.task);
    return cmdResult;
  }

  public CmdResult ProcessNetCommand(Player? player, string inputValue)
  {
    Log.Info($"Executing DevConsole command (player {player?.NetId}): `{inputValue}`");
    string[] tokens = inputValue.Split(' ', StringSplitOptions.None);
    return this.ProcessCommandInternal(player, tokens);
  }

  private CmdResult ProcessCommandInternal(Player? player, string[] tokens)
  {
    string token = tokens[0];
    string[] array = ((IEnumerable<string>) tokens).Skip<string>(1).ToArray<string>();
    return this.ProcessCommand(player, token, array);
  }

  private CmdResult ProcessCommand(Player? player, string cmdName, string[] args)
  {
    args = ((IEnumerable<string>) args).Where<string>((Func<string, bool>) (a => !string.IsNullOrWhiteSpace(a))).ToArray<string>();
    if (cmdName.Equals("help"))
      return this.HelpCmd(args);
    AbstractConsoleCmd abstractConsoleCmd;
    if (!this._commands.TryGetValue(cmdName.ToLowerInvariant(), out abstractConsoleCmd))
      return new CmdResult(false, $"The command '{cmdName}' does not exist.\nYou can use the 'help' to get a list of all possible commands.\n");
    Log.Info($"DevConsole: {cmdName} {string.Join(" ", args)}");
    return abstractConsoleCmd.Process(player, args);
  }

  private CmdResult HelpCmd(string[] args)
  {
    if (args.Length != 0)
    {
      AbstractConsoleCmd abstractConsoleCmd;
      if (!this._commands.TryGetValue(args[0], out abstractConsoleCmd))
        return new CmdResult(false, $"No command named {args[0]} found!");
      return new CmdResult(true, $"[gold]{abstractConsoleCmd.CmdName}[/gold] {abstractConsoleCmd.Args}\n\t{abstractConsoleCmd.Description}");
    }
    int totalWidth = this._commands.Values.Select<AbstractConsoleCmd, int>((Func<AbstractConsoleCmd, int>) (c => c.CmdName.Length)).Max();
    StringBuilder stringBuilder1 = new StringBuilder().Append("Usage:\n");
    foreach (KeyValuePair<string, AbstractConsoleCmd> command in this._commands)
    {
      StringBuilder stringBuilder2 = stringBuilder1;
      StringBuilder stringBuilder3 = stringBuilder2;
      StringBuilder.AppendInterpolatedStringHandler interpolatedStringHandler = new StringBuilder.AppendInterpolatedStringHandler(18, 2, stringBuilder2);
      ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendLiteral("\t[gold]");
      ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendFormatted(command.Value.CmdName.PadRight(totalWidth));
      ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendLiteral("[/gold] - ");
      ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendFormatted(command.Value.Description);
      ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendLiteral("\n");
      ref StringBuilder.AppendInterpolatedStringHandler local = ref interpolatedStringHandler;
      stringBuilder3.Append(ref local);
    }
    stringBuilder1.Append("Use 'help <cmd>' to obtain help on a specific command.");
    return new CmdResult(true, stringBuilder1.ToString());
  }
}
