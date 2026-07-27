// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Debug.GitHelper
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using MegaCrit.Sts2.Core.Logging;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Debug;

public static class GitHelper
{
  public static Task<string?>? ShortCommitIdTask { get; private set; }

  public static string? ShortCommitId { get; private set; }

  public static async Task Initialize()
  {
    // ISSUE: reference to a compiler-generated field
    // ISSUE: reference to a compiler-generated field
    GitHelper.ShortCommitIdTask = Task.Run<string>(GitHelper.\u003C\u003EO.\u003C0\u003E__GetCommitId ?? (GitHelper.\u003C\u003EO.\u003C0\u003E__GetCommitId = new Func<string>(GitHelper.GetCommitId)));
    GitHelper.ShortCommitId = await GitHelper.ShortCommitIdTask;
  }

  private static string? GetCommitId()
  {
    if (!OS.HasFeature("editor"))
      return (string) null;
    using (Process process = Process.Start(new ProcessStartInfo()
    {
      FileName = "git",
      Arguments = "rev-parse --short HEAD",
      UseShellExecute = false,
      RedirectStandardOutput = true,
      CreateNoWindow = true
    }))
    {
      if (process == null)
      {
        Log.Error("Error: Unable to start git process to get the git commit id.");
        return (string) null;
      }
      string end = ((TextReader) process.StandardOutput).ReadToEnd();
      process.WaitForExit();
      return end.Trim();
    }
  }
}
