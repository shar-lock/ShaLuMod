// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Runs.Metrics.MetricUtilities
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using MegaCrit.Sts2.Core.Achievements;
using MegaCrit.Sts2.Core.Debug;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Map;
using MegaCrit.Sts2.Core.Modding;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Platform;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Runs.History;
using MegaCrit.Sts2.Core.Saves;
using MegaCrit.Sts2.Core.Saves.Runs;
using MegaCrit.Sts2.Core.Timeline;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Runs.Metrics;

public static class MetricUtilities
{
  private const string _metricsRunDataEndpoint = "https://sts2-metric-upload.megacrit.com/record_data/";
  private const string _metricsAchievementDataEndpoint = "https://sts2-metric-upload.megacrit.com/record_achievement/";
  private const string _metricsEpochDataEndpoint = "https://sts2-metric-upload.megacrit.com/record_epoch/";
  private const string _metricsSettingsDataEndpoint = "https://sts2-metric-upload.megacrit.com/record_settings/";
  private const int _runLengthThreshold = 5;
  private static readonly HttpClient _httpClient = new HttpClient()
  {
    Timeout = TimeSpan.FromSeconds(15L)
  };

  public static void UploadRunMetrics(SerializableRun run, bool isVictory, ulong localPlayerId)
  {
    try
    {
      MetricUtilities.UploadRunMetricsInternal(run, isVictory, localPlayerId);
    }
    catch (Exception ex)
    {
      Log.Error($"Failed to upload run metrics: {ex.Message}\n{ex.StackTrace}");
      SentryService.CaptureException(ex);
    }
  }

  private static bool ShouldUploadMetrics()
  {
    if (ReleaseInfoManager.Instance.ReleaseInfo == null)
    {
      Log.Info("Skipping metrics upload, this is a debug build.");
      return false;
    }
    if (!SaveManager.Instance.PrefsSave.UploadData)
    {
      Log.Info("Skipping metrics upload, user has upload data unset in settings");
      return false;
    }
    if (SaveManager.Instance.SettingsSave.FullConsole)
    {
      Log.Info("Skipping metrics upload, user has enabled full console");
      return false;
    }
    if (!OS.HasFeature("editor"))
      return true;
    Log.Info("Skipping metrics upload since we're in the editor.");
    return false;
  }

  private static void UploadRunMetricsInternal(
    SerializableRun run,
    bool isVictory,
    ulong localPlayerId)
  {
    if (!MetricUtilities.ShouldUploadMetrics())
      return;
    if (RunManager.Instance.IsAbandoned)
      Log.Info("Skipping metrics upload, run was abandoned.");
    else if (SaveManager.Instance.Progress.NumberOfRuns <= 1)
      Log.Info("Skipping metrics upload, this is our first run ever.");
    else if (ModManager.IsRunningModded())
    {
      Log.Info("Skipping metrics upload, we're running modded.");
      ModManager.CallMetricsHooks(run, isVictory, localPlayerId);
    }
    else
    {
      if (run.GameMode != GameMode.Standard)
        return;
      List<MapPointHistoryEntry> list1 = run.MapPointHistory.SelectMany<List<MapPointHistoryEntry>, MapPointHistoryEntry>((Func<List<MapPointHistoryEntry>, IEnumerable<MapPointHistoryEntry>>) (logs => (IEnumerable<MapPointHistoryEntry>) logs)).ToList<MapPointHistoryEntry>();
      if (list1.Count < 5)
        return;
      LocManager.Instance.StartOverridingLanguageAsEnglish();
      try
      {
        ModelId modelId = ModelId.none;
        List<MapPointHistoryEntry> source = run.MapPointHistory.LastOrDefault<List<MapPointHistoryEntry>>();
        MapPointHistoryEntry pointHistoryEntry = source != null ? source.LastOrDefault<MapPointHistoryEntry>() : (MapPointHistoryEntry) null;
        if (!isVictory && pointHistoryEntry != null && pointHistoryEntry.Rooms.Last<MapPointRoomHistoryEntry>().RoomType.IsCombatRoom())
          modelId = pointHistoryEntry.Rooms.Last<MapPointRoomHistoryEntry>().ModelId;
        SerializablePlayer localPlayer = run.Players.First<SerializablePlayer>((Func<SerializablePlayer, bool>) (p => (long) p.NetId == (long) localPlayerId));
        List<EncounterMetric> list2 = list1.Where<MapPointHistoryEntry>((Func<MapPointHistoryEntry, bool>) (e => e.Rooms.Last<MapPointRoomHistoryEntry>().RoomType.IsCombatRoom())).Select<MapPointHistoryEntry, EncounterMetric>((Func<MapPointHistoryEntry, EncounterMetric>) (e => new EncounterMetric(e.Rooms.Last<MapPointRoomHistoryEntry>().ModelId.Entry, int.Min(e.GetEntry(localPlayerId).DamageTaken, localPlayer.MaxHp), e.Rooms.Last<MapPointRoomHistoryEntry>().TurnsTaken + 1))).ToList<EncounterMetric>();
        List<CardChoiceMetric> list3 = list1.Where<MapPointHistoryEntry>((Func<MapPointHistoryEntry, bool>) (e => e.GetEntry(localPlayerId).CardChoices.Count > 0)).Select<MapPointHistoryEntry, CardChoiceMetric>((Func<MapPointHistoryEntry, CardChoiceMetric>) (e => new CardChoiceMetric(e.GetEntry(localPlayerId).CardChoices))).ToList<CardChoiceMetric>();
        List<AncientMetric> list4 = list1.Where<MapPointHistoryEntry>((Func<MapPointHistoryEntry, bool>) (e => e.MapPointType == MapPointType.Ancient)).Where<MapPointHistoryEntry>((Func<MapPointHistoryEntry, bool>) (e => e.GetEntry(localPlayerId).AncientChoices.Count > 0)).Select<MapPointHistoryEntry, AncientMetric>((Func<MapPointHistoryEntry, AncientMetric>) (e => new AncientMetric(e, e.GetEntry(localPlayerId)))).ToList<AncientMetric>();
        List<ActWinMetric> actWinMetricList = new List<ActWinMetric>();
        List<EventChoiceMetric> eventChoiceMetricList = new List<EventChoiceMetric>();
        for (int index = 0; index < run.MapPointHistory.Count; ++index)
        {
          foreach (MapPointHistoryEntry entry in run.MapPointHistory[index])
          {
            if (entry.Rooms.First<MapPointRoomHistoryEntry>().RoomType == RoomType.Event && entry.GetEntry(localPlayerId).EventChoices.Count != 0 && entry.MapPointType != MapPointType.Ancient)
              eventChoiceMetricList.Add(new EventChoiceMetric(entry, localPlayerId, run.Acts[index]));
          }
          bool win = index < run.MapPointHistory.Count - 1 | isVictory;
          actWinMetricList.Add(new ActWinMetric(run.Acts[index].Id.Entry, win));
        }
        ProgressState progress = SaveManager.Instance.Progress;
        MetricUtilities.UploadRunMetrics(new RunMetrics()
        {
          Ascension = run.Ascension,
          TotalPlaytime = (float) progress.TotalPlaytime,
          TotalWinRate = (float) progress.Wins / (float) progress.NumberOfRuns,
          NumReloads = run.NumReloads,
          BuildId = ReleaseInfoManager.Instance.ReleaseInfo?.Version ?? "NON-RELEASE-VERSION",
          BuildType = PlatformUtil.GetPlatformBranch().ToName(),
          PlayerId = progress.UniqueId,
          Character = localPlayer.CharacterId,
          NumPlayers = run.Players.Count,
          Team = run.Players.Count > 1 ? run.Players.Select<SerializablePlayer, ModelId>((Func<SerializablePlayer, ModelId>) (p => p.CharacterId)).ToList<ModelId>() : new List<ModelId>(),
          Win = isVictory,
          FloorReached = list1.Count,
          KilledByEncounter = modelId,
          Deck = localPlayer.Deck.Select<SerializableCard, ModelId>((Func<SerializableCard, ModelId>) (c => c.Id)),
          Relics = localPlayer.Relics.Select<SerializableRelic, ModelId>((Func<SerializableRelic, ModelId>) (r => r.Id)),
          RunPlaytime = run.WinTime > 0L ? (float) run.WinTime : (float) run.RunTime,
          Encounters = list2,
          CardChoices = list3,
          EventChoices = eventChoiceMetricList,
          AncientChoices = list4,
          ActWins = actWinMetricList,
          CampfireUpgrades = list1.Where<MapPointHistoryEntry>((Func<MapPointHistoryEntry, bool>) (e => e.MapPointType == MapPointType.RestSite)).SelectMany<MapPointHistoryEntry, ModelId>((Func<MapPointHistoryEntry, IEnumerable<ModelId>>) (e => (IEnumerable<ModelId>) e.GetEntry(localPlayerId).UpgradedCards)).Select<ModelId, string>((Func<ModelId, string>) (c => c.Entry)).ToList<string>(),
          RelicBuys = list1.SelectMany<MapPointHistoryEntry, ModelId>((Func<MapPointHistoryEntry, IEnumerable<ModelId>>) (e => (IEnumerable<ModelId>) e.GetEntry(localPlayerId).BoughtRelics)).Select<ModelId, string>((Func<ModelId, string>) (r => r.Entry)).ToList<string>(),
          PotionBuys = list1.SelectMany<MapPointHistoryEntry, ModelId>((Func<MapPointHistoryEntry, IEnumerable<ModelId>>) (e => (IEnumerable<ModelId>) e.GetEntry(localPlayerId).BoughtPotions)).Select<ModelId, string>((Func<ModelId, string>) (p => p.Entry)).ToList<string>(),
          ColorlessBuys = list1.SelectMany<MapPointHistoryEntry, ModelId>((Func<MapPointHistoryEntry, IEnumerable<ModelId>>) (e => (IEnumerable<ModelId>) e.GetEntry(localPlayerId).BoughtColorless)).Select<ModelId, string>((Func<ModelId, string>) (c => c.Entry)).ToList<string>(),
          PotionDiscards = list1.SelectMany<MapPointHistoryEntry, ModelId>((Func<MapPointHistoryEntry, IEnumerable<ModelId>>) (e => (IEnumerable<ModelId>) e.GetEntry(localPlayerId).PotionDiscarded)).Select<ModelId, string>((Func<ModelId, string>) (p => p.Entry)).ToList<string>()
        });
      }
      finally
      {
        LocManager.Instance.StopOverridingLanguageAsEnglish();
      }
    }
  }

  private static void UploadRunMetrics(RunMetrics run)
  {
    Log.Info("Uploading run metrics...");
    TaskHelper.RunSafely(MetricUtilities.PutRequest("https://sts2-metric-upload.megacrit.com/record_data/", JsonSerializer.Serialize<RunMetrics>(run, MetricsSerializerContext.Default.RunMetrics), "Run metrics"));
  }

  public static void UploadAchievementMetric(Achievement achievement)
  {
    if (!MetricUtilities.ShouldUploadMetrics())
      return;
    string json = JsonSerializer.Serialize<AchievementMetric>(new AchievementMetric()
    {
      BuildId = ReleaseInfoManager.Instance.ReleaseInfo?.Version ?? "NON-RELEASE-VERSION",
      Achievement = achievement.ToString(),
      TotalRuns = SaveManager.Instance.Progress.NumberOfRuns,
      TotalPlaytime = SaveManager.Instance.Progress.TotalPlaytime,
      TotalAchievements = SaveManager.Instance.Progress.UnlockedAchievements.Count
    }, MetricsSerializerContext.Default.AchievementMetric);
    Log.Info($"Uploading achievement metric for achievement {achievement}");
    TaskHelper.RunSafely(MetricUtilities.PutRequest("https://sts2-metric-upload.megacrit.com/record_achievement/", json, $"Achievement {achievement}"));
  }

  public static void UploadEpochMetric(string epochId)
  {
    if (!MetricUtilities.ShouldUploadMetrics())
      return;
    string json = JsonSerializer.Serialize<EpochMetric>(new EpochMetric()
    {
      BuildId = ReleaseInfoManager.Instance.ReleaseInfo?.Version ?? "NON-RELEASE-VERSION",
      Epoch = epochId.Replace("_EPOCH", ""),
      TotalRuns = SaveManager.Instance.Progress.NumberOfRuns,
      TotalPlaytime = SaveManager.Instance.Progress.TotalPlaytime,
      TotalEpochs = SaveManager.Instance.Progress.Epochs.Count
    }, MetricsSerializerContext.Default.EpochMetric);
    Log.Info("Uploading epoch metric for epoch " + epochId);
    TaskHelper.RunSafely(MetricUtilities.PutRequest("https://sts2-metric-upload.megacrit.com/record_epoch/", json, "Epoch " + epochId));
  }

  public static void UploadSettingsMetric()
  {
    if (!MetricUtilities.ShouldUploadMetrics())
      return;
    long num = Variant.op_Explicit(OS.GetMemoryInfo()[Variant.op_Implicit("physical")]);
    string json = JsonSerializer.Serialize<SettingsDataMetric>(new SettingsDataMetric()
    {
      BuildId = ReleaseInfoManager.Instance.ReleaseInfo?.Version ?? "NON-RELEASE-VERSION",
      Os = OS.GetName(),
      Platform = "Steam",
      SystemRam = num == -1L ? -1 : (int) Math.Round((double) num / 1073741824.0),
      LanguageCode = SaveManager.Instance.SettingsSave.Language,
      FastModeType = SaveManager.Instance.PrefsSave.FastMode,
      Screenshake = SaveManager.Instance.PrefsSave.ScreenShakeOptionIndex,
      ShowRunTimer = SaveManager.Instance.PrefsSave.ShowRunTimer,
      PhobiaMode = SaveManager.Instance.PrefsSave.PhobiaMode,
      ShowCardIndices = SaveManager.Instance.PrefsSave.ShowCardIndices,
      DisplayCount = DisplayServer.GetScreenCount(),
      DisplayResolution = SaveManager.Instance.SettingsSave.WindowSize,
      Fullscreen = SaveManager.Instance.SettingsSave.Fullscreen,
      AspectRatio = SaveManager.Instance.SettingsSave.AspectRatioSetting,
      ResizeWindows = SaveManager.Instance.SettingsSave.ResizeWindows,
      VSync = SaveManager.Instance.SettingsSave.VSync,
      FpsLimit = SaveManager.Instance.SettingsSave.FpsLimit,
      Msaa = SaveManager.Instance.SettingsSave.Msaa
    }, MetricsSerializerContext.Default.SettingsDataMetric);
    Log.Info("Completed 5 runs! Uploading settings metrics");
    TaskHelper.RunSafely(MetricUtilities.PutRequest("https://sts2-metric-upload.megacrit.com/record_settings/", json, "Settings data metrics"));
  }

  private static async Task PutRequest(string url, string json, string context)
  {
    ByteArrayContent content = new ByteArrayContent(Encoding.UTF8.GetBytes(json));
    content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
    HttpResponseMessage response;
    try
    {
      response = await MetricUtilities._httpClient.PutAsync(url, (HttpContent) content);
    }
    catch (Exception ex) when (
    {
      // ISSUE: unable to correctly present filter
      bool flag;
      switch (ex)
      {
        case HttpRequestException _:
        case TaskCanceledException _:
          flag = true;
          break;
        default:
          flag = false;
          break;
      }
      if (flag)
      {
        SuccessfulFiltering;
      }
      else
        throw;
    }
    )
    {
      Log.Warn($"Metrics upload for '{context}' failed due to network error: {ex.Message}");
      response = (HttpResponseMessage) null;
      return;
    }
    if (response.IsSuccessStatusCode)
    {
      Log.Info($"Metric for '{context}' successfully uploaded!");
      response = (HttpResponseMessage) null;
    }
    else
    {
      string str = await response.Content.ReadAsStringAsync();
      Log.Warn($"Metrics upload request for '{context}' failed with status code: {response.StatusCode}");
      Log.Info("Response body: " + str);
      response = (HttpResponseMessage) null;
    }
  }
}
