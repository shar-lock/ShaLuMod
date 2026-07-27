// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Logging.Log
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using System;

#nullable enable
namespace MegaCrit.Sts2.Core.Logging;

public static class Log
{
  private static readonly Logger _logger = new Logger((string) null, LogType.Generic);

  public static event Action<LogLevel, string, int>? LogCallback;

  public static string Timestamp => DateTime.UtcNow.ToString("HH:mm:ss");

  public static void InvokeGlobalLogCallback(LogLevel logLevel, string log, int skipFrames)
  {
    Action<LogLevel, string, int> logCallback = Log.LogCallback;
    if (logCallback == null)
      return;
    logCallback(logLevel, log, skipFrames);
  }

  public static void Load(string text, int skipFrames = 2) => Log._logger.Load(text, skipFrames);

  public static void Debug(string text, int skipFrames = 2) => Log._logger.Debug(text, skipFrames);

  public static void VeryDebug(string text, int skipFrames = 2)
  {
    Log._logger.VeryDebug(text, skipFrames);
  }

  public static void Info(string text, int skipFrames = 2) => Log._logger.Info(text, skipFrames);

  public static void Warn(string text, int skipFrames = 2) => Log._logger.Warn(text, skipFrames);

  public static void Error(string text, int skipFrames = 2) => Log._logger.Error(text, skipFrames);

  public static void LogMessage(LogLevel level, LogType type, string text, int skipFrames = 1)
  {
    Log._logger.LogMessage(level, type, text, skipFrames);
  }
}
