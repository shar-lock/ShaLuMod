// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Saves.FileWriteRetry
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Exceptions;
using MegaCrit.Sts2.Core.Logging;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Saves;

public static class FileWriteRetry
{
  public const int maxAttempts = 4;
  public const int retryDelayMs = 50;

  public static void Run(string path, Action writeAttempt, Action<int>? sleep = null)
  {
    if (sleep == null)
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      sleep = FileWriteRetry.\u003C\u003EO.\u003C0\u003E__Sleep ?? (FileWriteRetry.\u003C\u003EO.\u003C0\u003E__Sleep = new Action<int>(Thread.Sleep));
    }
    for (int index = 1; index <= 4; ++index)
    {
      try
      {
        writeAttempt();
        break;
      }
      catch (Exception ex) when (FileWriteRetry.IsTransient(ex) && index < 4)
      {
        Log.Warn($"File write failed (attempt {index}/{4}), retrying. path={path} error={ex.Message}");
        sleep(50);
      }
    }
  }

  public static async Task RunAsync(string path, Func<Task> writeAttempt, Func<int, Task>? delay = null)
  {
    if (delay == null)
      delay = (Func<int, Task>) (ms => Task.Delay(ms));
    for (int attempt = 1; attempt <= 4; ++attempt)
    {
      int num;
      try
      {
        await writeAttempt();
        break;
      }
      catch (Exception ex) when (FileWriteRetry.IsTransient(ex) && attempt < 4)
      {
        num = 1;
      }
      if (num == 1)
      {
        Log.Warn($"File write failed (attempt {attempt}/{4}), retrying. path={path} error={ex.Message}");
        await delay(50);
      }
    }
  }

  private static bool IsTransient(Exception e)
  {
    bool flag;
    switch (e)
    {
      case SaveException _:
      case IOException _:
        flag = true;
        break;
      default:
        flag = false;
        break;
    }
    return flag;
  }
}
