// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Logging.EditorLogPrinter
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using System;
using System.Diagnostics;
using System.IO;

#nullable enable
namespace MegaCrit.Sts2.Core.Logging;

public class EditorLogPrinter : ILogPrinter
{
  private static string GetColorCode(LogLevel level)
  {
    switch (level)
    {
      case LogLevel.VeryDebug:
        return "#B180D3";
      case LogLevel.Load:
        return "#87CEEB";
      case LogLevel.Debug:
        return "#B180D3";
      case LogLevel.Info:
        return "#76FF56";
      case LogLevel.Warn:
        return "#FFCB3D";
      case LogLevel.Error:
        return "#FF4747";
      default:
        throw new ArgumentOutOfRangeException(nameof (level), (object) level, (string) null);
    }
  }

  private static bool LogFullStack(LogLevel level)
  {
    switch (level)
    {
      case LogLevel.VeryDebug:
        return false;
      case LogLevel.Load:
        return false;
      case LogLevel.Debug:
        return false;
      case LogLevel.Info:
        return false;
      case LogLevel.Warn:
        return false;
      case LogLevel.Error:
        return true;
      default:
        throw new ArgumentOutOfRangeException(nameof (level), (object) level, (string) null);
    }
  }

  public void Print(LogLevel level, string text, int skipFrames)
  {
    ++skipFrames;
    string upperInvariant = level.ToString().ToUpperInvariant();
    string str1 = $"[b][color={EditorLogPrinter.GetColorCode(level)}][{upperInvariant}][/color][/b] {text}";
    string str2 = "";
    if (level != LogLevel.Warn)
    {
      if (EditorLogPrinter.LogFullStack(level))
      {
        str2 = $"\n{new StackTrace(skipFrames, true)}";
      }
      else
      {
        StackFrame stackFrame = new StackFrame(skipFrames, true);
        DiagnosticMethodInfo diagnosticMethodInfo = DiagnosticMethodInfo.Create(stackFrame);
        int fileLineNumber = stackFrame.GetFileLineNumber();
        string fileName = Path.GetFileName(stackFrame.GetFileName());
        str2 = $" line {fileLineNumber}:{diagnosticMethodInfo?.Name}() in {fileName}";
      }
    }
    if (level == LogLevel.Error)
    {
      string str3 = $"[{upperInvariant}] {text}{str2}";
      GD.PrintErr(str3);
      GD.PushError(str3);
    }
    if (level == LogLevel.Warn)
      GD.PrintRich(str1 ?? "");
    else
      GD.PrintRich($"{str1}[color=#606060]{str2}[/color]");
  }
}
