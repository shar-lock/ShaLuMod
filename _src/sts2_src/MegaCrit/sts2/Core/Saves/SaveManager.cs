// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Saves.SaveManager
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Debug;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Multiplayer.Game;
using MegaCrit.Sts2.Core.Nodes;
using MegaCrit.Sts2.Core.Platform.Steam;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.Runs.Metrics;
using MegaCrit.Sts2.Core.Saves.Managers;
using MegaCrit.Sts2.Core.Saves.Migrations;
using MegaCrit.Sts2.Core.Saves.Test;
using MegaCrit.Sts2.Core.TestSupport;
using MegaCrit.Sts2.Core.Timeline;
using MegaCrit.Sts2.Core.Timeline.Epochs;
using MegaCrit.Sts2.Core.Unlocks;
using Steamworks;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Saves;

public class SaveManager : IProfileIdProvider
{
  private static SaveManager? _mockInstance;
  private static SaveManager? _instance;
  private readonly SettingsSaveManager _settingsSaveManager;
  private readonly ProgressSaveManager _progressSaveManager;
  private readonly RunSaveManager _runSaveManager;
  private readonly RunHistorySaveManager _runHistorySaveManager;
  private readonly PrefsSaveManager _prefsSaveManager;
  private readonly ProfileSaveManager _profileSaveManager;
  private readonly ISaveStore _saveStore;
  private readonly MigrationManager _migrationManager;
  private int? _currentProfileId;
  public const int totalAgnosticUnlocks = 18;
  private static readonly string[] _agnosticEpochUnlockOrder = new string[18]
  {
    EpochModel.GetId<Colorless1Epoch>(),
    EpochModel.GetId<Relic1Epoch>(),
    EpochModel.GetId<Potion1Epoch>(),
    EpochModel.GetId<UnderdocksEpoch>(),
    EpochModel.GetId<Colorless2Epoch>(),
    EpochModel.GetId<Relic2Epoch>(),
    EpochModel.GetId<Potion2Epoch>(),
    EpochModel.GetId<Act2BEpoch>(),
    EpochModel.GetId<Colorless3Epoch>(),
    EpochModel.GetId<Relic3Epoch>(),
    EpochModel.GetId<Act3BEpoch>(),
    EpochModel.GetId<Colorless4Epoch>(),
    EpochModel.GetId<Relic4Epoch>(),
    EpochModel.GetId<Event1Epoch>(),
    EpochModel.GetId<Colorless5Epoch>(),
    EpochModel.GetId<Relic5Epoch>(),
    EpochModel.GetId<Event2Epoch>(),
    EpochModel.GetId<Event3Epoch>()
  };

  public static SaveManager Instance
  {
    get
    {
      if (SaveManager._mockInstance != null)
        return SaveManager._mockInstance;
      if (SaveManager._instance == null)
        SaveManager._instance = SaveManager.ConstructDefault();
      return SaveManager._instance;
    }
  }

  public SettingsSave SettingsSave => this._settingsSaveManager.Settings;

  public PrefsSave PrefsSave => this._prefsSaveManager.Prefs;

  public ProgressState Progress
  {
    get => this._progressSaveManager.Progress;
    set => this._progressSaveManager.Progress = value;
  }

  public bool HasRunSave => this._runSaveManager.HasRunSave;

  public bool HasMultiplayerRunSave => this._runSaveManager.HasMultiplayerRunSave;

  public bool IsProfileInitialized => this._currentProfileId.HasValue;

  public int CurrentProfileId
  {
    get
    {
      return this._currentProfileId ?? throw new InvalidOperationException("InitProfileId must be called on SaveManager!");
    }
  }

  public Task? CurrentRunSaveTask { get; private set; }

  public event Action? Saved
  {
    add => this._runSaveManager.Saved += value;
    remove => this._runSaveManager.Saved -= value;
  }

  public event Action<int>? ProfileIdChanged;

  public static void MockInstanceForTesting(SaveManager saveManager)
  {
    SaveManager._mockInstance = saveManager;
  }

  public static void ClearInstanceForTesting() => SaveManager._mockInstance = (SaveManager) null;

  public SaveManager(ISaveStore saveStore, bool forceSynchronous = false)
    : this(saveStore, saveStore, forceSynchronous)
  {
  }

  public SaveManager(ISaveStore saveStore, ISaveStore localOnlyStore, bool forceSynchronous = false)
  {
    this._saveStore = saveStore;
    this._migrationManager = new MigrationManager(saveStore);
    this._settingsSaveManager = new SettingsSaveManager(localOnlyStore, this._migrationManager);
    this._profileSaveManager = new ProfileSaveManager(saveStore, this._migrationManager);
    this._prefsSaveManager = new PrefsSaveManager(saveStore, this._migrationManager, (IProfileIdProvider) this);
    this._progressSaveManager = new ProgressSaveManager(saveStore, this._migrationManager, (IProfileIdProvider) this);
    this._runSaveManager = new RunSaveManager(saveStore, this._migrationManager, (IProfileIdProvider) this, forceSynchronous);
    this._runHistorySaveManager = new RunHistorySaveManager(saveStore, this._migrationManager, (IProfileIdProvider) this);
  }

  private static SaveManager ConstructDefault()
  {
    if (TestMode.IsOn)
      return new SaveManager((ISaveStore) new MockGodotFileIo("user://test"));
    ISaveStore saveStore1 = (ISaveStore) new GodotFileIo(UserDataPathProvider.GetAccountScopedBasePath((string) null));
    ISaveStore saveStore2 = saveStore1;
    if (SteamInitializer.Initialized)
    {
      Log.Info($"Steam is enabled, we will write saves to steam storage. Enabled for account: {SteamRemoteStorage.IsCloudEnabledForAccount()}, app: {SteamRemoteStorage.IsCloudEnabledForApp()} ");
      SteamRemoteSaveStore cloudStore = new SteamRemoteSaveStore();
      saveStore2 = (ISaveStore) new CloudSaveStore(saveStore1, (ICloudSaveStore) cloudStore);
    }
    return new SaveManager(saveStore2, saveStore1);
  }

  public void InitProfileId(int? profileId = null)
  {
    this.CleanupTemporaryFiles();
    if (!profileId.HasValue)
    {
      this._profileSaveManager.LoadProfile();
      this._currentProfileId = new int?(this._profileSaveManager.Profile.LastProfileId);
    }
    else
      this._currentProfileId = new int?(profileId.Value);
    Log.Info("Profile-scoped data path initialized: " + UserDataPathProvider.GetProfileScopedBasePath(this.CurrentProfileId));
    this._runHistorySaveManager.CreateRunHistoryDirectory();
  }

  public int GetLatestSchemaVersion<T>() => this._migrationManager.GetLatestVersion<T>();

  public string GetProfileScopedPath(string userData)
  {
    return this._saveStore.GetFullPath(Path.Combine(UserDataPathProvider.GetProfileDir(this.CurrentProfileId), userData));
  }

  public void SwitchProfileId(int profileId)
  {
    Log.Info($"Switching save profiles to {profileId}");
    this._currentProfileId = new int?(profileId);
    this._profileSaveManager.Profile.LastProfileId = profileId;
    this.SaveProfile();
    this._runHistorySaveManager.CreateRunHistoryDirectory();
    Action<int> profileIdChanged = this.ProfileIdChanged;
    if (profileIdChanged == null)
      return;
    profileIdChanged(profileId);
  }

  public SaveBatchScope BeginSaveBatch()
  {
    if (this._saveStore is CloudSaveStore saveStore)
      saveStore.BeginSaveBatch();
    return new SaveBatchScope(this);
  }

  public void EndSaveBatch()
  {
    if (!(this._saveStore is CloudSaveStore saveStore))
      return;
    saveStore.EndSaveBatch();
  }

  public async Task SaveRun(AbstractRoom? preFinishedRoom, bool saveProgress = true)
  {
    if (this.CurrentRunSaveTask != null)
      await this.CurrentRunSaveTask;
    SaveBatchScope saveBatchScope = this.BeginSaveBatch();
    try
    {
      int num;
      if (num != 1 && saveProgress)
        this.SaveProgressFile();
      try
      {
        this.CurrentRunSaveTask = this._runSaveManager.SaveRun(preFinishedRoom);
        await this.CurrentRunSaveTask;
      }
      catch (Exception ex)
      {
        Log.Error($"Failed to save run: {ex}");
        SentryService.CaptureException(ex);
      }
      finally
      {
        this.CurrentRunSaveTask = (Task) null;
      }
    }
    finally
    {
      saveBatchScope.Dispose();
    }
    saveBatchScope = new SaveBatchScope();
  }

  public async Task IncrementNumReloads(SerializableRun save, NetGameType type, bool forceInTest = false)
  {
    ++save.NumReloads;
    bool flag1 = forceInTest || TestMode.IsOff;
    if (flag1)
    {
      bool flag2;
      switch (type)
      {
        case NetGameType.Singleplayer:
        case NetGameType.Host:
          flag2 = true;
          break;
        default:
          flag2 = false;
          break;
      }
      flag1 = flag2;
    }
    if (!flag1)
      return;
    if (this.CurrentRunSaveTask != null)
      await this.CurrentRunSaveTask;
    SaveBatchScope saveBatchScope = this.BeginSaveBatch();
    try
    {
      try
      {
        this.CurrentRunSaveTask = this._runSaveManager.SaveRun(save, type.IsMultiplayer());
        await this.CurrentRunSaveTask;
      }
      finally
      {
        this.CurrentRunSaveTask = (Task) null;
      }
    }
    finally
    {
      saveBatchScope.Dispose();
    }
    saveBatchScope = new SaveBatchScope();
  }

  public void UpdateProgressWithRunData(SerializableRun serializableRun, bool victory)
  {
    this._progressSaveManager.UpdateWithRunData(serializableRun, victory);
  }

  public void UpdateProgressAfterCombatWon(Player localPlayer, CombatRoom combatRoom)
  {
    this._progressSaveManager.UpdateAfterCombatWon(localPlayer, combatRoom);
  }

  public void DeleteCurrentRun() => this._runSaveManager.DeleteCurrentRun();

  public void DeleteCurrentMultiplayerRun() => this._runSaveManager.DeleteCurrentMultiplayerRun();

  public void DeleteProfile(int profileId)
  {
    string profileScopedBasePath = UserDataPathProvider.GetProfileScopedBasePath(profileId);
    Log.Info($"DELETING the profile id {profileId} at path {profileScopedBasePath}!!");
    this.DeleteDirectoryRecursive(UserDataPathProvider.GetProfileDir(profileId));
  }

  public void DeleteDirectoryRecursive(string directory)
  {
    this.DeleteInDirectoryRecursive(directory);
    Log.Info("Deleting directory at " + directory);
    this._saveStore.DeleteDirectory(directory);
  }

  private void DeleteInDirectoryRecursive(string directory)
  {
    foreach (string str1 in this._saveStore.GetDirectoriesInDirectory(directory))
    {
      string str2 = $"{directory}/{str1}";
      this.DeleteInDirectoryRecursive(str2);
      Log.Info("Deleting directory at " + str2);
      this._saveStore.DeleteDirectory(str2);
    }
    foreach (string str in this._saveStore.GetFilesInDirectory(directory))
    {
      string path = $"{directory}/{str}";
      Log.Info("Deleting file at " + path);
      this._saveStore.DeleteFile(path);
    }
    if (!(this._saveStore is CloudSaveStore saveStore))
      return;
    foreach (string str in saveStore.CloudStore.GetFilesInDirectory(directory))
    {
      string path = $"{directory}/{str}";
      Log.Info("Deleting cloud-only file at " + path);
      saveStore.CloudStore.DeleteFile(path);
    }
  }

  public void SaveSettings()
  {
    try
    {
      this._settingsSaveManager.SaveSettings();
    }
    catch (Exception ex)
    {
      Log.Error($"Failed to save settings: {ex}");
      SentryService.CaptureException(ex);
    }
  }

  public void SaveProfile()
  {
    try
    {
      this._profileSaveManager.SaveProfile();
    }
    catch (Exception ex)
    {
      Log.Error($"Failed to save profile: {ex}");
      SentryService.CaptureException(ex);
    }
  }

  public ReadSaveResult<SettingsSave> InitSettingsDataForTest()
  {
    this._settingsSaveManager.Settings = new SettingsSave();
    return new ReadSaveResult<SettingsSave>(this._settingsSaveManager.Settings);
  }

  public ReadSaveResult<PrefsSave> InitPrefsDataForTest()
  {
    this._prefsSaveManager.Prefs = new PrefsSave();
    return new ReadSaveResult<PrefsSave>(this._prefsSaveManager.Prefs);
  }

  public ReadSaveResult<SettingsSave> InitSettingsData()
  {
    return this._settingsSaveManager.LoadSettings();
  }

  public ReadSaveResult<PrefsSave> InitPrefsData() => this._prefsSaveManager.LoadPrefs();

  public ReadSaveResult<SerializableProgress> InitProgressData()
  {
    return this._progressSaveManager.LoadProgress();
  }

  public async Task SyncCloudToLocal()
  {
    List<Task> tasks;
    if (!(this._saveStore is CloudSaveStore saveStore))
    {
      tasks = (List<Task>) null;
    }
    else
    {
      Log.Info("Syncing cloud save files to the local save directory");
      SaveManager.DeleteSettingsFromCloud(saveStore);
      tasks = new List<Task>();
      foreach (Task enumerateCloudSyncTask in this.EnumerateCloudSyncTasks(saveStore))
      {
        tasks.Add(enumerateCloudSyncTask);
        if (tasks.Count >= 8)
        {
          await Task.WhenAll((IEnumerable<Task>) tasks);
          tasks.Clear();
        }
      }
      if (tasks.Count > 0)
        await Task.WhenAll((IEnumerable<Task>) tasks);
      this.CleanupStaleCurrentRunSaves();
      tasks = (List<Task>) null;
    }
  }

  private IEnumerable<Task> EnumerateCloudSyncTasks(CloudSaveStore cloudStore)
  {
    yield return cloudStore.SyncCloudToLocal(ProfileSaveManager.GetProfileSavePath());
    for (int i = 1; i <= 3; ++i)
    {
      yield return cloudStore.SyncCloudToLocal(ProgressSaveManager.GetProgressPathForProfile(i));
      yield return cloudStore.SyncCloudToLocal(RunSaveManager.GetRunSavePath(i, "current_run.save"));
      yield return cloudStore.SyncCloudToLocal(RunSaveManager.GetRunSavePath(i, "current_run_mp.save"));
      yield return cloudStore.SyncCloudToLocal(PrefsSaveManager.GetPrefsPath(i));
      foreach (Task task in cloudStore.SyncCloudToLocalDirectory(RunHistorySaveManager.GetHistoryPath(i)))
        yield return task;
    }
  }

  private static void DeleteSettingsFromCloud(CloudSaveStore cloudStore)
  {
    try
    {
      if (!cloudStore.CloudStore.FileExists("settings.save"))
        return;
      Log.Info("Deleting stale settings.save from cloud storage");
      cloudStore.CloudStore.DeleteFile("settings.save");
    }
    catch (Exception ex)
    {
      Log.Warn("Failed to delete settings from cloud: " + ex.Message);
    }
  }

  private void CleanupStaleCurrentRunSaves()
  {
    for (int profileId = 1; profileId <= 3; ++profileId)
    {
      this.CleanupStaleCurrentRunSaveForProfile(profileId, "current_run.save");
      this.CleanupStaleCurrentRunSaveForProfile(profileId, "current_run_mp.save");
    }
  }

  private void CleanupStaleCurrentRunSaveForProfile(int profileId, string runSaveFileName)
  {
    string runSavePath = RunSaveManager.GetRunSavePath(profileId, runSaveFileName);
    string path1 = runSavePath + ".backup";
    string path2 = (string) null;
    if (this._saveStore.FileExists(runSavePath))
      path2 = runSavePath;
    else if (this._saveStore.FileExists(path1))
      path2 = path1;
    if (path2 == null)
      return;
    try
    {
      string json = this._saveStore.ReadFile(path2);
      if (json == null)
      {
        Log.Warn($"Could not read {path2}, skipping staleness check");
      }
      else
      {
        long? startTimeFromRunSave = SaveManager.ExtractStartTimeFromRunSave(json);
        if (!startTimeFromRunSave.HasValue)
        {
          Log.Warn($"Could not extract start_time from {path2}, skipping staleness check");
        }
        else
        {
          string path3 = Path.Combine(RunHistorySaveManager.GetHistoryPath(profileId), $"{startTimeFromRunSave}.run");
          if (!this._saveStore.FileExists(path3))
            return;
          Log.Warn($"Deleting stale {runSaveFileName} for profile {profileId}: run with StartTime {startTimeFromRunSave} already exists in history at {path3}");
          this._saveStore.DeleteFile(runSavePath);
          this._saveStore.DeleteFile(runSavePath + ".backup");
        }
      }
    }
    catch (Exception ex)
    {
      Log.Warn($"Error checking for stale current_run.save in profile {profileId}: {ex.Message}");
    }
  }

  private void CleanupTemporaryFiles()
  {
    this._saveStore.DeleteTemporaryFiles("");
    for (int profileId = 1; profileId <= 3; ++profileId)
    {
      this._saveStore.DeleteTemporaryFiles(Path.Combine(UserDataPathProvider.GetProfileDir(profileId), UserDataPathProvider.SavesDir));
      this._saveStore.DeleteTemporaryFiles(RunHistorySaveManager.GetHistoryPath(profileId));
    }
  }

  private static long? ExtractStartTimeFromRunSave(string json)
  {
    try
    {
      using (JsonDocument jsonDocument = JsonDocument.Parse(json, new JsonDocumentOptions()))
      {
        JsonElement rootElement = jsonDocument.RootElement;
        JsonElement jsonElement;
        if (((JsonElement) ref rootElement).TryGetProperty("start_time", ref jsonElement))
          return new long?(((JsonElement) ref jsonElement).GetInt64());
      }
    }
    catch
    {
    }
    return new long?();
  }

  public bool ShouldOverwriteCloudWithLocal()
  {
    return this._saveStore is CloudSaveStore saveStore && (!saveStore.HasUserEnabledCloudSync() || this._saveStore.FileExists(ProfileSaveManager.GetProfileSavePath()) && !saveStore.HasCloudFiles());
  }

  public async Task OverwriteCloudWithLocal()
  {
    List<Task> tasks;
    if (!(this._saveStore is CloudSaveStore saveStore))
    {
      tasks = (List<Task>) null;
    }
    else
    {
      Log.Info("OVERWRITING cloud saves with local saves.");
      tasks = new List<Task>();
      foreach (Task cloudWithLocalTask in this.EnumerateOverwriteCloudWithLocalTasks(saveStore))
      {
        tasks.Add(cloudWithLocalTask);
        if (tasks.Count >= 8)
        {
          await Task.WhenAll((IEnumerable<Task>) tasks);
          tasks.Clear();
        }
      }
      if (tasks.Count <= 0)
      {
        tasks = (List<Task>) null;
      }
      else
      {
        await Task.WhenAll((IEnumerable<Task>) tasks);
        tasks = (List<Task>) null;
      }
    }
  }

  private IEnumerable<Task> EnumerateOverwriteCloudWithLocalTasks(CloudSaveStore cloudStore)
  {
    yield return cloudStore.OverwriteCloudWithLocal(ProfileSaveManager.GetProfileSavePath());
    for (int i = 1; i <= 3; ++i)
    {
      yield return cloudStore.OverwriteCloudWithLocal(ProgressSaveManager.GetProgressPathForProfile(i));
      yield return cloudStore.OverwriteCloudWithLocal(RunSaveManager.GetRunSavePath(i, "current_run.save"));
      yield return cloudStore.OverwriteCloudWithLocal(RunSaveManager.GetRunSavePath(i, "current_run_mp.save"));
      yield return cloudStore.OverwriteCloudWithLocal(PrefsSaveManager.GetPrefsPath(i));
      foreach (Task task in cloudStore.OverwriteCloudWithLocalDirectory(RunHistorySaveManager.GetHistoryPath(i), new int?(5242880 /*0x500000*/), new int?(100)))
        yield return task;
    }
  }

  public ReadSaveResult<SerializableRun> LoadRunSave() => this._runSaveManager.LoadRunSave();

  public ReadSaveResult<SerializableRun> LoadAndCanonicalizeMultiplayerRunSave(ulong localPlayerId)
  {
    return this._runSaveManager.LoadAndCanonicalizeMultiplayerRunSave(localPlayerId);
  }

  public void SaveRunHistory(RunHistory history)
  {
    try
    {
      this._runHistorySaveManager.SaveHistory(history);
    }
    catch (Exception ex)
    {
      Log.Error($"Failed to save run history: {ex}");
      SentryService.CaptureException(ex);
    }
  }

  public int GetRunHistoryCount() => this._runHistorySaveManager.GetHistoryCount();

  public List<string> GetAllRunHistoryNames()
  {
    return this._runHistorySaveManager.LoadAllRunHistoryNames();
  }

  public ReadSaveResult<RunHistory> LoadRunHistory(string fileName)
  {
    return this._runHistorySaveManager.LoadHistory(fileName);
  }

  public static string ToJson<[DynamicallyAccessedMembers] T>(T obj) where T : ISaveSchema
  {
    return JsonSerializationUtility.ToJson<T>(obj);
  }

  public static ReadSaveResult<T> FromJson<[DynamicallyAccessedMembers] T>(string json) where T : ISaveSchema, new()
  {
    return JsonSerializationUtility.FromJson<T>(json);
  }

  public bool SeenFtue(string ftueKey) => this._progressSaveManager.SeenFtue(ftueKey);

  public bool SeenPopup(string popupKey) => this._progressSaveManager.SeenPopup(popupKey);

  public void SaveProgressFile() => this._progressSaveManager.SaveProgress();

  public UnlockState GenerateUnlockStateFromProgress()
  {
    return this._progressSaveManager.GenerateUnlockState();
  }

  public void SavePrefsFile()
  {
    try
    {
      this._prefsSaveManager.SavePrefs();
    }
    catch (Exception ex)
    {
      Log.Error($"Failed to save prefs: {ex}");
      SentryService.CaptureException(ex);
    }
  }

  public void MarkFtueAsComplete(string ftueId)
  {
    this._progressSaveManager.MarkFtueAsComplete(ftueId);
  }

  public void SetFtuesEnabled(bool enabled) => this._progressSaveManager.SetFtuesEnabled(enabled);

  public void ResetFtues() => this._progressSaveManager.ResetFtues();

  public void MarkPotionAsSeen(PotionModel potion)
  {
    this._progressSaveManager.MarkPotionAsSeen(potion);
  }

  public void MarkCardAsSeen(CardModel card) => this._progressSaveManager.MarkCardAsSeen(card);

  public void MarkRelicAsSeen(RelicModel relic) => this._progressSaveManager.MarkRelicAsSeen(relic);

  public bool IsRelicSeen(RelicModel relic) => this.Progress.DiscoveredRelics.Contains(relic.Id);

  public void UnlockSlot(string epochId) => this.Progress.UnlockSlot(epochId);

  public void ObtainEpoch(string epochId) => this.Progress.ObtainEpoch(epochId);

  public void ObtainEpochOverride(string epochId, EpochState state)
  {
    this.Progress.ObtainEpochOverride(epochId, state);
  }

  public void RevealEpoch(string epochId, bool isDebug = false)
  {
    this.Progress.RevealEpoch(epochId);
    if (isDebug)
      return;
    MetricUtilities.UploadEpochMetric(epochId);
  }

  public void ResetTimelineProgress()
  {
    this.Progress.ResetEpochs();
    this.ObtainEpochOverride(EpochModel.GetId<NeowEpoch>(), EpochState.Obtained);
    this.SaveProgressFile();
  }

  public bool IsEpochRevealed<T>() where T : EpochModel
  {
    return this.Progress.IsEpochRevealed(EpochModel.GetId<T>());
  }

  public bool IsEpochRevealed(string id) => this.Progress.IsEpochRevealed(id);

  public int GetTotalUnlockedCards()
  {
    return ((IEnumerable<string>) SaveManager.GetCardUnlockEpochIds()).Count<string>(new Func<string, bool>(this.IsEpochRevealed)) * 3;
  }

  public static int GetUnlockableCardCount() => SaveManager.GetCardUnlockEpochIds().Length * 3;

  private static string[] GetCardUnlockEpochIds()
  {
    return new string[20]
    {
      EpochModel.GetId<Colorless1Epoch>(),
      EpochModel.GetId<Colorless2Epoch>(),
      EpochModel.GetId<Colorless3Epoch>(),
      EpochModel.GetId<Colorless4Epoch>(),
      EpochModel.GetId<Colorless5Epoch>(),
      EpochModel.GetId<Ironclad2Epoch>(),
      EpochModel.GetId<Ironclad5Epoch>(),
      EpochModel.GetId<Ironclad7Epoch>(),
      EpochModel.GetId<Silent2Epoch>(),
      EpochModel.GetId<Silent5Epoch>(),
      EpochModel.GetId<Silent7Epoch>(),
      EpochModel.GetId<Regent2Epoch>(),
      EpochModel.GetId<Regent5Epoch>(),
      EpochModel.GetId<Regent7Epoch>(),
      EpochModel.GetId<Defect2Epoch>(),
      EpochModel.GetId<Defect5Epoch>(),
      EpochModel.GetId<Defect7Epoch>(),
      EpochModel.GetId<Necrobinder2Epoch>(),
      EpochModel.GetId<Necrobinder5Epoch>(),
      EpochModel.GetId<Necrobinder7Epoch>()
    };
  }

  public int GetTotalUnlockedRelics()
  {
    return ((IEnumerable<string>) SaveManager.GetRelicUnlockEpochIds()).Count<string>(new Func<string, bool>(this.IsEpochRevealed)) * 3;
  }

  public static int GetUnlockableRelicCount() => SaveManager.GetRelicUnlockEpochIds().Length * 3;

  private static string[] GetRelicUnlockEpochIds()
  {
    return new string[15]
    {
      EpochModel.GetId<Relic1Epoch>(),
      EpochModel.GetId<Relic2Epoch>(),
      EpochModel.GetId<Relic3Epoch>(),
      EpochModel.GetId<Relic4Epoch>(),
      EpochModel.GetId<Relic5Epoch>(),
      EpochModel.GetId<Ironclad3Epoch>(),
      EpochModel.GetId<Ironclad6Epoch>(),
      EpochModel.GetId<Silent3Epoch>(),
      EpochModel.GetId<Silent6Epoch>(),
      EpochModel.GetId<Regent3Epoch>(),
      EpochModel.GetId<Regent6Epoch>(),
      EpochModel.GetId<Defect3Epoch>(),
      EpochModel.GetId<Defect6Epoch>(),
      EpochModel.GetId<Necrobinder3Epoch>(),
      EpochModel.GetId<Necrobinder6Epoch>()
    };
  }

  public int GetTotalUnlockedPotions()
  {
    return ((IEnumerable<string>) SaveManager.GetPotionUnlockEpochIds()).Count<string>(new Func<string, bool>(this.IsEpochRevealed)) * 3;
  }

  public static int GetUnlockablePotionCount() => SaveManager.GetPotionUnlockEpochIds().Length * 3;

  private static string[] GetPotionUnlockEpochIds()
  {
    return new string[7]
    {
      EpochModel.GetId<Potion1Epoch>(),
      EpochModel.GetId<Potion2Epoch>(),
      EpochModel.GetId<Ironclad4Epoch>(),
      EpochModel.GetId<Silent4Epoch>(),
      EpochModel.GetId<Regent4Epoch>(),
      EpochModel.GetId<Defect4Epoch>(),
      EpochModel.GetId<Necrobinder4Epoch>()
    };
  }

  public int GetAggregateAscensionProgress()
  {
    return this.Progress.CharacterStats.Values.Sum<CharacterStats>((Func<CharacterStats, int>) (stat => stat.MaxAscension));
  }

  public static int GetAggregateAscensionCount()
  {
    return ModelDb.AllCharacters.Count<CharacterModel>() * 10;
  }

  public int GetTotalKills()
  {
    return this.Progress.EnemyStats.Values.Sum<EnemyStats>((Func<EnemyStats, int>) (enemy => enemy.TotalWins));
  }

  public IEnumerable<SerializableEpoch> GetRevealableEpochs()
  {
    return this._progressSaveManager.GetRevealableEpochs();
  }

  public int GetDiscoveredEpochCount() => this.GetRevealableEpochs().Count<SerializableEpoch>();

  public bool IsNeowDiscovered()
  {
    SerializableEpoch serializableEpoch = this.Progress.Epochs.FirstOrDefault<SerializableEpoch>((Func<SerializableEpoch, bool>) (e => e.Id == EpochModel.GetId<NeowEpoch>()));
    return serializableEpoch != null && serializableEpoch.State != EpochState.Revealed;
  }

  public int GetUnlocksRemaining() => 18 - this.Progress.TotalUnlocks;

  public int GetCurrentScore() => this.Progress.CurrentScore;

  public string? IncrementUnlock()
  {
    ++this.Progress.TotalUnlocks;
    return this.GetEpochIdForUnlock();
  }

  private string? GetEpochIdForUnlock()
  {
    int index = this.Progress.TotalUnlocks - 1;
    return index < 0 || index >= SaveManager._agnosticEpochUnlockOrder.Length ? (string) null : SaveManager._agnosticEpochUnlockOrder[index];
  }

  public bool IsCompendiumAvailable() => this.Progress.NumberOfRuns > 0 || !NGame.IsReleaseGame();
}
