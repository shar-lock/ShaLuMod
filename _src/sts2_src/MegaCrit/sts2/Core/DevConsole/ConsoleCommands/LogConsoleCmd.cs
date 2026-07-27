// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.DevConsole.ConsoleCommands.LogConsoleCmd
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Logging;
using System;

#nullable enable
namespace MegaCrit.Sts2.Core.DevConsole.ConsoleCommands;

public class LogConsoleCmd : AbstractConsoleCmd
{
  public override string CmdName => "log";

  public override string Args => "[type:string] <level:string>";

  public override string Description
  {
    get
    {
      return $"Set log level for specific log types. Type can be: {string.Join(",", Enum.GetNames<LogType>())}. Levels can be: {string.Join(",", Enum.GetNames<LogLevel>())}";
    }
  }

  public override bool IsNetworked => false;

  public override bool DebugOnly => false;

  public override CmdResult Process(Player? issuingPlayer, string[] args)
  {
    if (args.Length == 0)
      return new CmdResult(false, "At least one arg must be supplied!");
    LogLevel? enumVal1;
    LogType? enumVal2;
    if (LogConsoleCmd.TryParseEnumCaseInsensitive<LogLevel>(args[0], out enumVal1))
    {
      enumVal2 = new LogType?(LogType.Generic);
    }
    else
    {
      if (!LogConsoleCmd.TryParseEnumCaseInsensitive<LogType>(args[0], out enumVal2))
        return new CmdResult(false, $"First argument '{args[0]}' could not be parsed as either a log level or type!");
      if (args.Length <= 1)
        return new CmdResult(false, "Must supply a log level as the second argument!");
      if (!LogConsoleCmd.TryParseEnumCaseInsensitive<LogLevel>(args[1], out enumVal1))
        return new CmdResult(false, $"Second argument '{args[1]}' could not be parsed as a log level!");
    }
    Logger.logLevelTypeMap[enumVal2.Value] = enumVal1.Value;
    return new CmdResult(true, $"Logging level for {enumVal2} set to {enumVal1}");
  }

  public static bool TryParseEnumCaseInsensitive<T>(string str, out T? enumVal) where T : struct, Enum
  {
    foreach (T obj in Enum.GetValues<T>())
    {
      if (str.Equals(obj.ToString(), StringComparison.OrdinalIgnoreCase))
      {
        enumVal = new T?(obj);
        return true;
      }
    }
    enumVal = new T?();
    return false;
  }
}
