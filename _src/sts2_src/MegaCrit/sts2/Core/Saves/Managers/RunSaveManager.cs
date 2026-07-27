// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Saves.Managers.RunSaveManager
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Multiplayer.Game;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.Saves.Migrations;
using System;
using System.IO;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Saves.Managers;

public class RunSaveManager
{
  public const string runSaveFileName = "current_run.save";
  public const string multiplayerRunSaveFileName = "current_run_mp.save";
  private readonly ISaveStore _saveStore;
  private readonly MigrationManager _migrationManager;
  private readonly bool _forceSynchronous;
  private readonly IProfileIdProvider _profileIdProvider;

  private string CurrentRunSavePath
  {
    get
    {
      return RunSaveManager.GetRunSavePath(this._profileIdProvider.CurrentProfileId, "current_run.save");
    }
  }

  private string CurrentMultiplayerRunSavePath
  {
    get
    {
      return RunSaveManager.GetRunSavePath(this._profileIdProvider.CurrentProfileId, "current_run_mp.save");
    }
  }

  public event Action? Saved;

  public bool HasRunSave
  {
    get
    {
      return this._saveStore.FileExists(this.CurrentRunSavePath) || this._saveStore.FileExists(this.CurrentRunSavePath + ".backup");
    }
  }

  public bool HasMultiplayerRunSave
  {
    get
    {
      return this._saveStore.FileExists(this.CurrentMultiplayerRunSavePath) || this._saveStore.FileExists(this.CurrentMultiplayerRunSavePath + ".backup");
    }
  }

  public int SchemaVersion => this._migrationManager.GetLatestVersion<SerializableRun>();

  public RunSaveManager(
    int profileId,
    ISaveStore saveStore,
    MigrationManager migrationManager,
    bool forceSynchronous = false)
    : this(saveStore, migrationManager, (IProfileIdProvider) new StaticProfileIdProvider(profileId), forceSynchronous)
  {
  }

  public RunSaveManager(
    ISaveStore saveStore,
    MigrationManager migrationManager,
    IProfileIdProvider profileIdProvider,
    bool forceSynchronous = false)
  {
    this._saveStore = saveStore;
    this._migrationManager = migrationManager;
    this._forceSynchronous = forceSynchronous;
    this._profileIdProvider = profileIdProvider;
  }

  public async Task SaveRun(AbstractRoom? preFinishedRoom)
  {
    if (!RunManager.Instance.ShouldSave || RunManager.Instance.NetService.Type != NetGameType.Singleplayer && RunManager.Instance.NetService.Type != NetGameType.Host)
      return;
    await this.SaveRun(RunManager.Instance.ToSave(preFinishedRoom), RunManager.Instance.NetService.Type == NetGameType.Host);
  }

  public async Task SaveRun(SerializableRun save, bool isMultiplayer)
  {
    string savePath = isMultiplayer ? this.CurrentMultiplayerRunSavePath : this.CurrentRunSavePath;
    MemoryStream stream = new MemoryStream();
    try
    {
      if (!this._forceSynchronous)
      {
        await JsonSerializer.SerializeAsync<SerializableRun>((Stream) stream, save, JsonSerializationUtility.GetTypeInfo<SerializableRun>(), new CancellationToken());
        ((Stream) stream).Seek(0L, (SeekOrigin) 0);
        await this._saveStore.WriteFileAsync(savePath, stream.ToArray());
      }
      else
      {
        JsonSerializer.Serialize<SerializableRun>((Stream) stream, save, JsonSerializationUtility.GetTypeInfo<SerializableRun>());
        ((Stream) stream).Seek(0L, (SeekOrigin) 0);
        this._saveStore.WriteFile(savePath, stream.ToArray());
      }
      Action saved = this.Saved;
      if (saved == null)
      {
        savePath = (string) null;
        stream = (MemoryStream) null;
      }
      else
      {
        saved();
        savePath = (string) null;
        stream = (MemoryStream) null;
      }
    }
    finally
    {
      ((IDisposable) stream)?.Dispose();
    }
  }

  public ReadSaveResult<SerializableRun> LoadRunSave()
  {
    ReadSaveResult<SerializableRun> readSaveResult = this._migrationManager.LoadSave<SerializableRun>(this.CurrentRunSavePath);
    if (readSaveResult.Success)
      return readSaveResult;
    if (readSaveResult.Status == ReadSaveStatus.FileNotFound)
      Log.Info("Run save file not found at " + this.CurrentRunSavePath);
    else if (!readSaveResult.Status.IsRecoverable())
      Log.Error($"Failed to load run save: status={readSaveResult.Status} msg={readSaveResult.ErrorMessage}");
    else
      Log.Warn($"Run save had recoverable issues: status={readSaveResult.Status} msg={readSaveResult.ErrorMessage}");
    return readSaveResult;
  }

  public ReadSaveResult<SerializableRun> LoadMultiplayerRunSave()
  {
    ReadSaveResult<SerializableRun> readSaveResult = this._migrationManager.LoadSave<SerializableRun>(this.CurrentMultiplayerRunSavePath);
    if (readSaveResult.Success)
      return readSaveResult;
    if (readSaveResult.Status == ReadSaveStatus.FileNotFound)
      Log.Info("Multiplayer run save file not found at " + this.CurrentMultiplayerRunSavePath);
    else if (!readSaveResult.Status.IsRecoverable())
      Log.Error($"Failed to load multiplayer run save: status={readSaveResult.Status} msg={readSaveResult.ErrorMessage}");
    else
      Log.Warn($"Multiplayer run save had recoverable issues: status={readSaveResult.Status} msg={readSaveResult.ErrorMessage}");
    return readSaveResult;
  }

  public ReadSaveResult<SerializableRun> LoadAndCanonicalizeMultiplayerRunSave(ulong localPlayerId)
  {
    ReadSaveResult<SerializableRun> readSaveResult = this.LoadMultiplayerRunSave();
    if (readSaveResult != null && readSaveResult.Success)
    {
      if (readSaveResult.SaveData != null)
      {
        try
        {
          return new ReadSaveResult<SerializableRun>(RunManager.CanonicalizeSave(readSaveResult.SaveData, localPlayerId), ReadSaveStatus.Success);
        }
        catch (Exception ex)
        {
          Log.Error($"Multiplayer run save validation failed: {ex}");
          this.RenameBrokenMultiplayerRunSave(ReadSaveStatus.ValidationFailed);
          return new ReadSaveResult<SerializableRun>(ReadSaveStatus.ValidationFailed, $"Save file validation failed: {ex}");
        }
      }
    }
    return readSaveResult;
  }

  public void DeleteCurrentRun()
  {
    this._saveStore.DeleteFile(this.CurrentRunSavePath);
    this._saveStore.DeleteFile(this.CurrentRunSavePath + ".backup");
  }

  public void DeleteCurrentMultiplayerRun()
  {
    this._saveStore.DeleteFile(this.CurrentMultiplayerRunSavePath);
    this._saveStore.DeleteFile(this.CurrentMultiplayerRunSavePath + ".backup");
  }

  public void RenameBrokenMultiplayerRunSave(ReadSaveStatus status)
  {
    try
    {
      if (!this.HasMultiplayerRunSave)
        return;
      if (this._saveStore.FileExists(this.CurrentMultiplayerRunSavePath))
      {
        string corruptFilePath = CorruptFileHandler.GenerateCorruptFilePath(this.CurrentMultiplayerRunSavePath, status);
        this._saveStore.RenameFile(this.CurrentMultiplayerRunSavePath, corruptFilePath);
        Log.Error($"Corrupt multiplayer run save detected: Renamed '{this.CurrentMultiplayerRunSavePath}' to '{corruptFilePath}'");
      }
      string str = this.CurrentMultiplayerRunSavePath + ".backup";
      if (!this._saveStore.FileExists(str))
        return;
      string corruptFilePath1 = CorruptFileHandler.GenerateCorruptFilePath(str, status);
      this._saveStore.RenameFile(str, corruptFilePath1);
      Log.Error($"Corrupt multiplayer run backup detected: Renamed '{str}' to '{corruptFilePath1}'");
    }
    catch (Exception ex)
    {
      Log.Warn("Failed to rename broken multiplayer run save: " + ex.Message);
    }
  }

  public static string GetRunSavePath(int profileId, string fileName, bool? forceModState = null)
  {
    return StringExtensions.PathJoin(StringExtensions.PathJoin(UserDataPathProvider.GetProfileDir(profileId, forceModState), UserDataPathProvider.SavesDir), fileName);
  }
}
