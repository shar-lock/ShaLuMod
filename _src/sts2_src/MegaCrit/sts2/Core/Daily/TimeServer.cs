// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Daily.TimeServer
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Logging;
using System;
using System.Net.Http;
using System.Runtime.ExceptionServices;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Daily;

public static class TimeServer
{
  private const string _timeServerUrl = "https://time.megacrit.com";
  private const int _maxRetry = 2;
  private const int _waitTimeCap = 4;
  private static readonly Logger _logger = new Logger(nameof (TimeServer), LogType.Network);

  public static Task<TimeServerResult?>? RequestTimeTask { get; private set; }

  private static async Task<TimeServerResult?> RequestTime(string url)
  {
    using (HttpClient client = new HttpClient()
    {
      Timeout = TimeSpan.FromSeconds(5L)
    })
    {
      Exception exception = (Exception) null;
      int retries = 0;
      while (retries < 2)
      {
        long? nullable;
        try
        {
          nullable = await TimeServer.RequestTimeInternal(client, url);
        }
        catch (Exception ex)
        {
          Log.Warn($"Caught exception while requesting server time of type {ex.GetType()}");
          exception = ex;
          nullable = new long?();
        }
        if (nullable.HasValue)
          return new TimeServerResult?(new TimeServerResult()
          {
            serverTime = DateTimeOffset.FromUnixTimeSeconds(nullable.Value),
            localReceivedTime = DateTimeOffset.UtcNow
          });
        ++retries;
        Log.Info($"Retries: {retries}/{2}");
        if (retries < 2)
          await Task.Delay(TimeSpan.FromSeconds(Math.Min((long) Math.Pow(2.0, (double) retries), 4L)));
      }
      TimeServer._logger.Warn("Gave up trying to retrieve server time");
      if (exception != null)
        ExceptionDispatchInfo.Capture(exception).Throw();
      return new TimeServerResult?();
    }
  }

  private static async Task<long?> RequestTimeInternal(HttpClient client, string url)
  {
    HttpResponseMessage response = await client.GetAsync(url);
    string s = await response.Content.ReadAsStringAsync();
    if (response.IsSuccessStatusCode)
    {
      TimeServer._logger.Info("Successfully queried time server. Response: " + s);
      long result;
      if (long.TryParse(s, out result))
        return new long?(result);
    }
    else
      TimeServer._logger.Info($"Failed to retrieve server time. Status: {response.StatusCode} Response: {s}");
    return new long?();
  }

  public static Task<TimeServerResult?> FetchDailyTime()
  {
    TimeServer.RequestTimeTask = TimeServer.RequestTime("https://time.megacrit.com");
    return TimeServer.RequestTimeTask;
  }
}
