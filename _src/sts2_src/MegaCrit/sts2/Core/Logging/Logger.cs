// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Logging.Logger
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using MegaCrit.Sts2.Core.DevConsole.ConsoleCommands;
using MegaCrit.Sts2.Core.TestSupport;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable enable
namespace MegaCrit.Sts2.Core.Logging;

public class Logger
{
  private static readonly object _lockObj = new object();
  private static readonly bool _isRunningFromGodotEditor = Logger.GetIsRunningFromGodotEditor();
  private static readonly ILogPrinter _logPrinter = Logger._isRunningFromGodotEditor ? (ILogPrinter) new EditorLogPrinter() : (ILogPrinter) new ConsoleLogPrinter();
  public static readonly Dictionary<LogType, LogLevel> logLevelTypeMap = new Dictionary<LogType, LogLevel>()
  {
    {
      LogType.Network,
      LogLevel.Info
    },
    {
      LogType.Actions,
      LogLevel.Info
    },
    {
      LogType.Generic,
      LogLevel.Info
    },
    {
      LogType.GameSync,
      LogLevel.Info
    }
  };
  private readonly LogType _logType;

  public static LogLevel GlobalLogLevel { get; set; } = LogLevel.Info;

  private static bool GetIsRunningFromGodotEditor()
  {
    string[] cmdlineArgs = OS.GetCmdlineArgs();
    bool flag1 = ((IEnumerable<string>) cmdlineArgs).Any<string>((Func<string, bool>) (arg => arg == "--headless"));
    bool flag2 = ((IEnumerable<string>) cmdlineArgs).Any<string>((Func<string, bool>) (arg => arg.Contains("CiCoreRunner.tscn")));
    bool flag3 = OS.HasFeature("editor");
    return !(flag1 | flag2) && !TestMode.IsOn && flag3;
  }

  public string? Context { get; set; }

  public event Action<LogLevel, string, int>? LogCallback;

  static Logger()
  {
    string[] commandLineArgs = Environment.GetCommandLineArgs();
    for (int index = 0; index < commandLineArgs.Length; ++index)
    {
      if (!(commandLineArgs[index] != "-log"))
      {
        LogType? enumVal1;
        if (!LogConsoleCmd.TryParseEnumCaseInsensitive<LogType>(commandLineArgs[index + 1], out enumVal1))
          Logger._logPrinter.Print(LogLevel.Error, $"Invalid log command line argument! Could not parse {commandLineArgs[index + 1]} as LogType", 1);
        LogLevel? enumVal2;
        if (!LogConsoleCmd.TryParseEnumCaseInsensitive<LogLevel>(commandLineArgs[index + 2], out enumVal2))
          Logger._logPrinter.Print(LogLevel.Error, $"Invalid log command line argument! Could not parse {commandLineArgs[index + 2]} as LogLevel", 1);
        Logger.logLevelTypeMap[enumVal1.Value] = enumVal2.Value;
        Logger._logPrinter.Print(LogLevel.Info, $"Log level for {enumVal1} set to {enumVal2}", 1);
      }
    }
  }

  public Logger(string? context, LogType logType)
  {
    this.Context = context;
    this._logType = logType;
    // ISSUE: reference to a compiler-generated field
    // ISSUE: reference to a compiler-generated field
    this.LogCallback += Logger.\u003C\u003EO.\u003C0\u003E__InvokeGlobalLogCallback ?? (Logger.\u003C\u003EO.\u003C0\u003E__InvokeGlobalLogCallback = new Action<LogLevel, string, int>(Log.InvokeGlobalLogCallback));
  }

  public bool WillLog(LogLevel level)
  {
    LogLevel logLevel;
    LogLevel? nullable = Logger.logLevelTypeMap.TryGetValue(this._logType, out logLevel) ? new LogLevel?(logLevel) : new LogLevel?();
    return level >= (LogLevel) ((int) nullable ?? (int) Logger.GlobalLogLevel);
  }

  public void LogMessage(LogLevel level, string text, int skipFrames)
  {
    ++skipFrames;
    string text1 = this.Context != null ? $"[{this.Context}] {text}" : text;
    this.LogMessage(level, this._logType, text1, skipFrames);
  }

  public void LogMessage(LogLevel level, LogType type, string text, int skipFrames)
  {
    ++skipFrames;
    if (!this.WillLog(level))
      return;
    lock (Logger._lockObj)
    {
      Logger._logPrinter.Print(level, text, skipFrames);
      Action<LogLevel, string, int> logCallback = this.LogCallback;
      if (logCallback == null)
        return;
      logCallback(level, text, skipFrames);
    }
  }

  public void Load(string text, int skipFrames = 1)
  {
    this.LogMessage(LogLevel.Load, text, skipFrames);
  }

  public void Debug(string text, int skipFrames = 1)
  {
    this.LogMessage(LogLevel.Debug, text, skipFrames);
  }

  public void VeryDebug(string text, int skipFrames = 1)
  {
    this.LogMessage(LogLevel.VeryDebug, text, skipFrames);
  }

  public void Info(string text, int skipFrames = 1)
  {
    this.LogMessage(LogLevel.Info, text, skipFrames);
  }

  public void Warn(string text, int skipFrames = 1)
  {
    this.LogMessage(LogLevel.Warn, text, skipFrames);
  }

  public void Error(string text, int skipFrames = 1)
  {
    this.LogMessage(LogLevel.Error, text, skipFrames);
  }

  public static void SetLogLevelForType(LogType type, LogLevel? logLevel)
  {
    if (logLevel.HasValue)
      Logger.logLevelTypeMap[type] = logLevel.Value;
    else
      Logger.logLevelTypeMap.Remove(type);
  }
}
