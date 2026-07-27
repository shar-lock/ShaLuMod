// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Saves.SaveTimestamps
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Debug;
using MegaCrit.Sts2.Core.Logging;
using Sentry;
using System;

#nullable enable
namespace MegaCrit.Sts2.Core.Saves;

public static class SaveTimestamps
{
  private static readonly long _minUnixSeconds = DateTimeOffset.MinValue.ToUnixTimeSeconds();
  private static readonly long _maxUnixSeconds = DateTimeOffset.MaxValue.ToUnixTimeSeconds();

  public static DateTimeOffset FromUnixTimeSecondsOrEpoch(long seconds, string path)
  {
    if (seconds >= SaveTimestamps._minUnixSeconds && seconds <= SaveTimestamps._maxUnixSeconds)
      return DateTimeOffset.FromUnixTimeSeconds(seconds);
    Log.Warn($"Last-modified timestamp {seconds} for {path} is outside the representable DateTimeOffset range; treating it as the Unix epoch so the file re-syncs. (PRG-7045)");
    SentryService.CaptureMessage("Save-store last-modified timestamp out of DateTimeOffset range", (SentryLevel) 2, (Action<Scope>) (scope =>
    {
      scope.SetExtra("save.timestamp.raw_seconds", (object) seconds);
      scope.SetExtra("save.timestamp.path", (object) path);
    }));
    return DateTimeOffset.UnixEpoch;
  }
}
