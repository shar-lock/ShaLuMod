// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Logging.ConsoleLogPrinter
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using System.Diagnostics;

#nullable enable
namespace MegaCrit.Sts2.Core.Logging;

public class ConsoleLogPrinter : ILogPrinter
{
  public void Print(LogLevel logLevel, string text, int skipFrames)
  {
    ++skipFrames;
    string upperInvariant = logLevel.ToString().ToUpperInvariant();
    switch (logLevel)
    {
      case LogLevel.Warn:
        GD.Print($"[{upperInvariant}] {text}");
        break;
      case LogLevel.Error:
        StackTrace stackTrace = new StackTrace(skipFrames, true);
        GD.PrintErr($"[{upperInvariant}] {text}\n{stackTrace}");
        break;
      default:
        GD.Print($"[{upperInvariant}] {text}");
        break;
    }
  }
}
