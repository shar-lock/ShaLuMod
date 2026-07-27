// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Audio.ActBankLoadRetry
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Debug;
using MegaCrit.Sts2.Core.Logging;
using Sentry;
using System;
using System.Threading;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Audio;

public static class ActBankLoadRetry
{
  public const int maxAttempts = 3;
  public const int retryDelayMs = 50;

  public static bool Run(
    string bankPath,
    Func<bool> loadAttempt,
    Action<int>? sleep = null,
    Action<string, ActBankLoadRetry.LoadOutcome>? report = null)
  {
    if (sleep == null)
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      sleep = ActBankLoadRetry.\u003C\u003EO.\u003C0\u003E__Sleep ?? (ActBankLoadRetry.\u003C\u003EO.\u003C0\u003E__Sleep = new Action<int>(Thread.Sleep));
    }
    if (report == null)
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      report = ActBankLoadRetry.\u003C\u003EO.\u003C1\u003E__ReportToSentry ?? (ActBankLoadRetry.\u003C\u003EO.\u003C1\u003E__ReportToSentry = new Action<string, ActBankLoadRetry.LoadOutcome>(ActBankLoadRetry.ReportToSentry));
    }
    for (int Attempts = 1; Attempts <= 3; ++Attempts)
    {
      if (loadAttempt())
      {
        report(bankPath, new ActBankLoadRetry.LoadOutcome(true, Attempts));
        return true;
      }
      if (Attempts < 3)
      {
        Log.Warn($"Act music bank load failed (attempt {Attempts}/{3}), retrying. path={bankPath}");
        sleep(50);
      }
    }
    Log.Error($"Act music bank failed to load after {3} attempts; music will not follow combat state. path={bankPath}");
    report(bankPath, new ActBankLoadRetry.LoadOutcome(false, 3));
    return false;
  }

  private static void ReportToSentry(string bankPath, ActBankLoadRetry.LoadOutcome outcome)
  {
    if (outcome.Loaded && outcome.Attempts == 1)
      return;
    string result = outcome.Loaded ? "recovered" : "failed";
    SentryService.CaptureMessage("Act music bank load needed retries or failed", outcome.Loaded ? (SentryLevel) 1 : (SentryLevel) 2, (Action<Scope>) (scope =>
    {
      scope.SetTag("act_bank_load", result);
      scope.SetTag("act_bank_load_attempts", outcome.Attempts.ToString());
      scope.SetTag("act_bank_path", bankPath);
      EventLikeExtensions.SetFingerprint((IEventLike) scope, new string[2]
      {
        "act-bank-load",
        result
      });
    }));
  }

  public readonly record struct LoadOutcome(bool Loaded, int Attempts);
}
