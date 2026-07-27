// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Saves.Managers.RunHistorySaveManager
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.Saves.Migrations;
using System.Collections.Generic;
using System.IO;
using System.Text;

#nullable enable
namespace MegaCrit.Sts2.Core.Saves.Managers;

public class RunHistorySaveManager
{
  public const int maxCloudBytes = 5242880 /*0x500000*/;
  public const int maxCloudFileCount = 100;
  private const string _historyDirName = "history";
  private readonly ISaveStore _saveStore;
  private readonly MigrationManager _migrationManager;
  private readonly IProfileIdProvider _profileIdProvider;

  private string HistoryPath
  {
    get => RunHistorySaveManager.GetHistoryPath(this._profileIdProvider.CurrentProfileId);
  }

  public RunHistorySaveManager(
    int profileId,
    ISaveStore saveStore,
    MigrationManager migrationManager)
    : this(saveStore, migrationManager, (IProfileIdProvider) new StaticProfileIdProvider(profileId))
  {
  }

  public RunHistorySaveManager(
    ISaveStore saveStore,
    MigrationManager migrationManager,
    IProfileIdProvider profileIdProvider)
  {
    this._saveStore = saveStore;
    this._migrationManager = migrationManager;
    this._profileIdProvider = profileIdProvider;
  }

  public void CreateRunHistoryDirectory() => this._saveStore.CreateDirectory(this.HistoryPath);

  public static string GetHistoryPath(int profileId, bool? forceModState = null)
  {
    return StringExtensions.PathJoin(StringExtensions.PathJoin(UserDataPathProvider.GetProfileDir(profileId, forceModState), UserDataPathProvider.SavesDir), "history");
  }

  public void SaveHistory(RunHistory history)
  {
    history.SchemaVersion = this._migrationManager.GetLatestVersion<RunHistory>();
    string json = JsonSerializationUtility.ToJson<RunHistory>(history);
    this.SaveHistoryInternal(Path.Combine(this.HistoryPath, $"{history.StartTime}.run"), json);
    Log.Info($"Saved run history: {history.StartTime}.run");
  }

  private void SaveHistoryInternal(string path, string content)
  {
    if (this._saveStore is CloudSaveStore saveStore)
    {
      int byteCount = Encoding.UTF8.GetByteCount(content);
      saveStore.ForgetFilesInDirectoryBeforeWritingIfNecessary(this.HistoryPath, byteCount, 5242880 /*0x500000*/, 100);
    }
    this._saveStore.WriteFile(path, content);
  }

  public int GetHistoryCount()
  {
    return !this._saveStore.DirectoryExists(this.HistoryPath) ? 0 : this._saveStore.GetFilesInDirectory(this.HistoryPath).Length;
  }

  public ReadSaveResult<RunHistory> LoadHistory(string fileName)
  {
    ReadSaveResult<RunHistory> readSaveResult = this._migrationManager.LoadSave<RunHistory>(Path.Combine(this.HistoryPath, fileName));
    if (readSaveResult.Success)
      Log.Info("Successfully loaded run history: " + fileName);
    else
      Log.Warn($"Failed to load run history {fileName}: {readSaveResult.Status}");
    return readSaveResult;
  }

  public List<string> LoadAllRunHistoryNames()
  {
    string[] filesInDirectory = this._saveStore.GetFilesInDirectory(this.HistoryPath);
    int num = 0;
    List<string> stringList = new List<string>();
    foreach (string str in filesInDirectory)
    {
      if (str.EndsWith(".corrupt"))
        ++num;
      else if (!str.EndsWith(".backup"))
        stringList.Add(str);
    }
    if (num > 0)
      Log.Warn($"Skipping {num} corrupt save files in history directory");
    Log.Debug($"Found {stringList.Count} run history files");
    return stringList;
  }
}
