// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Helpers.TaskHelper
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Debug;
using MegaCrit.Sts2.Core.Logging;
using System;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Helpers;

public static class TaskHelper
{
  public static Task RunSafely(Task task) => TaskHelper.LogTaskExceptions(task);

  private static async Task LogTaskExceptions(Task task)
  {
    try
    {
      await task;
    }
    catch (Exception ex)
    {
      if (!(ex is OperationCanceledException))
      {
        Log.Error(ex.ToString());
        SentryService.CaptureException(ex);
      }
      throw;
    }
  }

  public static async Task WhenAny(params Task[] tasks) => await await Task.WhenAny(tasks);
}
