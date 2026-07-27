// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.AutoSlay.AutoSlayLog
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Runs;
using System;
using System.IO;

#nullable enable
namespace MegaCrit.Sts2.Core.AutoSlay;

public static class AutoSlayLog
{
  private const string _prefix = "[AutoSlay]";
  private static StreamWriter? _fileWriter;
  private static readonly object _lock = new object();

  public static void OpenLogFile(string path)
  {
    lock (AutoSlayLog._lock)
    {
      ((TextWriter) AutoSlayLog._fileWriter)?.Dispose();
      AutoSlayLog._fileWriter = new StreamWriter(path, false)
      {
        AutoFlush = true
      };
    }
  }

  public static void CloseLogFile()
  {
    lock (AutoSlayLog._lock)
    {
      ((TextWriter) AutoSlayLog._fileWriter)?.Dispose();
      AutoSlayLog._fileWriter = (StreamWriter) null;
    }
  }

  public static void Info(string message)
  {
    string str = "[AutoSlay] " + message;
    Log.Info(str);
    AutoSlayLog.WriteToFile("INFO", str);
  }

  public static void Warn(string message)
  {
    string str = "[AutoSlay] " + message;
    Log.Warn(str);
    AutoSlayLog.WriteToFile("WARN", str);
  }

  public static void Error(string message)
  {
    string str = "[AutoSlay] " + message;
    Log.Error(str);
    AutoSlayLog.WriteToFile("ERROR", str);
  }

  public static void Error(string message, Exception ex)
  {
    string str = $"{"[AutoSlay]"} {message}: {ex.Message}\n{ex.StackTrace}";
    Log.Error(str);
    AutoSlayLog.WriteToFile("ERROR", str);
  }

  private static void WriteToFile(string level, string message)
  {
    lock (AutoSlayLog._lock)
    {
      StreamWriter fileWriter = AutoSlayLog._fileWriter;
      if (fileWriter == null)
        return;
      ((TextWriter) fileWriter).WriteLine($"{DateTime.Now:HH:mm:ss.fff} [{level}] {message}");
    }
  }

  public static void RunStarted(string seed)
  {
    string str = Path.Combine(OS.GetUserDataDir(), "logs", "godot.log");
    AutoSlayLog.Info("Starting run with seed=" + seed);
    AutoSlayLog.Info("Godot log: " + str);
  }

  public static void RunCompleted(string seed)
  {
    AutoSlayLog.Info("Run completed successfully with seed=" + seed);
  }

  public static void RunFailed(string seed, Exception ex)
  {
    AutoSlayLog.Error("Run failed with seed=" + seed, ex);
  }

  public static void EnterRoom(RoomType type, int act, int floor)
  {
    AutoSlayLog.Info($"Entering {type} room (Act {act + 1}, Floor {floor})");
  }

  public static void ExitRoom(RoomType type) => AutoSlayLog.Info($"Finished {type} room");

  public static void EnterScreen(string screenName)
  {
    AutoSlayLog.Info("Handling screen: " + screenName);
  }

  public static void ExitScreen(string screenName)
  {
    AutoSlayLog.Info("Finished screen: " + screenName);
  }

  public static void Action(string action) => AutoSlayLog.Info("Action: " + action);

  public static void StateSnapshot(RunState? runState)
  {
    if (runState == null)
      AutoSlayLog.Info("State: RunState is null");
    else
      AutoSlayLog.Info($"State: Floor={runState.TotalFloor}, Room={runState.CurrentRoom?.RoomType}, Act={runState.CurrentActIndex + 1}");
  }
}
