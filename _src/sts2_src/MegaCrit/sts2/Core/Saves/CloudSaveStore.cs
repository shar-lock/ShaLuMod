// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Saves.CloudSaveStore
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Debug;
using MegaCrit.Sts2.Core.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Saves;

public class CloudSaveStore : ICloudSaveStore, ISaveStore
{
  private readonly CloudSyncFailureReporter _syncFailureReporter = new CloudSyncFailureReporter();

  public ISaveStore LocalStore { get; }

  public ICloudSaveStore CloudStore { get; }

  public CloudSaveStore(ISaveStore localStore, ICloudSaveStore cloudStore)
  {
    this.LocalStore = localStore;
    this.CloudStore = cloudStore;
  }

  public string? ReadFile(string path) => this.LocalStore.ReadFile(path);

  public Task<string?> ReadFileAsync(string path) => this.LocalStore.ReadFileAsync(path);

  public bool FileExists(string path) => this.LocalStore.FileExists(path);

  public bool DirectoryExists(string path) => this.LocalStore.DirectoryExists(path);

  public void WriteFile(string path, string content)
  {
    this.LocalStore.WriteFile(path, content);
    try
    {
      this.CloudStore.WriteFile(path, content);
      this.SyncLocalTimestamp(path);
    }
    catch (Exception ex)
    {
      Log.Warn($"Cloud write failed for {path}, local file preserved: {ex.Message}");
      this.SyncLocalTimestamp(path);
    }
  }

  public void WriteFile(string path, byte[] bytes)
  {
    this.LocalStore.WriteFile(path, bytes);
    try
    {
      this.CloudStore.WriteFile(path, bytes);
      this.SyncLocalTimestamp(path);
    }
    catch (Exception ex)
    {
      Log.Warn($"Cloud write failed for {path}, local file preserved: {ex.Message}");
      this.SyncLocalTimestamp(path);
    }
  }

  public async Task WriteFileAsync(string path, string content)
  {
    await this.LocalStore.WriteFileAsync(path, content);
    try
    {
      await this.CloudStore.WriteFileAsync(path, content);
      this.SyncLocalTimestamp(path);
    }
    catch (Exception ex)
    {
      Log.Warn($"Cloud write failed for {path}, local file preserved: {ex.Message}");
      this.SyncLocalTimestamp(path);
    }
  }

  public async Task WriteFileAsync(string path, byte[] bytes)
  {
    await this.LocalStore.WriteFileAsync(path, bytes);
    try
    {
      await this.CloudStore.WriteFileAsync(path, bytes);
      this.SyncLocalTimestamp(path);
    }
    catch (Exception ex)
    {
      Log.Warn($"Cloud write failed for {path}, local file preserved: {ex.Message}");
      this.SyncLocalTimestamp(path);
    }
  }

  public void DeleteFile(string path)
  {
    this.LocalStore.DeleteFile(path);
    try
    {
      this.CloudStore.DeleteFile(path);
    }
    catch (Exception ex)
    {
      Log.Warn($"Cloud delete failed for {path}, local delete preserved: {ex.Message}");
    }
  }

  public void RenameFile(string sourcePath, string destinationPath)
  {
    this.LocalStore.RenameFile(sourcePath, destinationPath);
    try
    {
      this.CloudStore.RenameFile(sourcePath, destinationPath);
    }
    catch (Exception ex)
    {
      Log.Warn($"Cloud rename failed for {sourcePath} -> {destinationPath}, local rename preserved: {ex.Message}");
    }
  }

  public string[] GetFilesInDirectory(string directoryPath)
  {
    return this.LocalStore.GetFilesInDirectory(directoryPath);
  }

  public string[] GetDirectoriesInDirectory(string directoryPath)
  {
    return this.LocalStore.GetDirectoriesInDirectory(directoryPath);
  }

  public void CreateDirectory(string directoryPath)
  {
    this.LocalStore.CreateDirectory(directoryPath);
    try
    {
      this.CloudStore.CreateDirectory(directoryPath);
    }
    catch (Exception ex)
    {
      Log.Warn($"Cloud create directory failed for {directoryPath}: {ex.Message}");
    }
  }

  public void DeleteDirectory(string directoryPath)
  {
    this.LocalStore.DeleteDirectory(directoryPath);
    try
    {
      this.CloudStore.DeleteDirectory(directoryPath);
    }
    catch (Exception ex)
    {
      Log.Warn($"Cloud delete directory failed for {directoryPath}: {ex.Message}");
    }
  }

  public void DeleteTemporaryFiles(string directoryPath)
  {
    this.LocalStore.DeleteTemporaryFiles(directoryPath);
    try
    {
      this.CloudStore.DeleteTemporaryFiles(directoryPath);
    }
    catch (Exception ex)
    {
      Log.Warn($"Cloud delete temporary files failed for {directoryPath}: {ex.Message}");
    }
  }

  public DateTimeOffset GetLastModifiedTime(string path)
  {
    return this.LocalStore.GetLastModifiedTime(path);
  }

  public int GetFileSize(string path) => this.LocalStore.GetFileSize(path);

  public void SetLastModifiedTime(string path, DateTimeOffset time)
  {
    this.LocalStore.SetLastModifiedTime(path, time);
  }

  public string GetFullPath(string filename) => this.LocalStore.GetFullPath(filename);

  public bool HasCloudFiles() => this.CloudStore.HasCloudFiles();

  public void ForgetFile(string path)
  {
    try
    {
      this.CloudStore.ForgetFile(path);
    }
    catch (Exception ex)
    {
      Log.Warn($"Cloud forget failed for {path}: {ex.Message}");
    }
  }

  public bool IsFilePersisted(string path) => this.CloudStore.IsFilePersisted(path);

  public void BeginSaveBatch() => this.CloudStore.BeginSaveBatch();

  public void EndSaveBatch() => this.CloudStore.EndSaveBatch();

  public async Task SyncCloudToLocal(string path)
  {
    try
    {
      await this.SyncCloudToLocalInternal(path);
    }
    catch (Exception ex)
    {
      Log.Warn($"SteamRemoteStorage: Failed to sync {path} from cloud, skipping: {ex.Message}");
      if (!this._syncFailureReporter.ShouldReport(ex))
        return;
      SentryService.CaptureException(ex);
    }
  }

  private async Task SyncCloudToLocalInternal(string path)
  {
    bool flag1 = this.CloudStore.FileExists(path);
    bool flag2 = this.LocalStore.FileExists(path);
    if (flag1)
    {
      DateTimeOffset lastModifiedTime = this.CloudStore.GetLastModifiedTime(path);
      DateTimeOffset? nullable1 = flag2 ? new DateTimeOffset?(this.LocalStore.GetLastModifiedTime(path)) : new DateTimeOffset?();
      int num;
      if (flag2)
      {
        DateTimeOffset dateTimeOffset = lastModifiedTime;
        DateTimeOffset? nullable2 = nullable1;
        num = nullable2.HasValue ? (dateTimeOffset != nullable2.GetValueOrDefault() ? 1 : 0) : 1;
      }
      else
        num = 1;
      bool flag3 = num != 0;
      if (!flag3 && string.IsNullOrWhiteSpace(this.LocalStore.ReadFile(path)))
      {
        Log.Warn($"Local file {path} appears corrupt (empty content) despite matching cloud timestamp, forcing re-download from cloud");
        flag3 = true;
      }
      if (flag3)
      {
        Log.Info($"Copying {path} from cloud to local. Local file exists: {flag2} Cloud save time: {lastModifiedTime} Local save time: {nullable1}");
        string content = await this.CloudStore.ReadFileAsync(path);
        if (string.IsNullOrWhiteSpace(content) || content[0] == char.MinValue)
        {
          Log.Warn($"Cloud file {path} has empty content, skipping download");
        }
        else
        {
          await this.LocalStore.WriteFileAsync(path, content);
          this.SyncLocalTimestamp(path);
        }
      }
      else
        Log.Debug($"Skipping sync for {path}, last modified time matches on local and remote ({lastModifiedTime})");
    }
    else if (flag2)
    {
      Log.Info($"Deleting {path} because it does not exist on remote");
      this.LocalStore.DeleteFile(path);
      this.LocalStore.DeleteFile(path + ".backup");
    }
    else
      Log.Debug($"Skipping sync for {path}, it doesn't exist on either local or cloud");
  }

  public IEnumerable<Task> SyncCloudToLocalDirectory(string directoryPath)
  {
    Log.Debug($"Syncing all files in {directoryPath} from cloud to local");
    HashSet<string> filePathsRead = new HashSet<string>();
    string[] strArray1 = Array.Empty<string>();
    try
    {
      if (this.CloudStore.DirectoryExists(directoryPath))
        strArray1 = this.CloudStore.GetFilesInDirectory(directoryPath);
    }
    catch (Exception ex)
    {
      Log.Warn($"Failed to list cloud files in {directoryPath}, skipping cloud sync: {ex.Message}");
      SentryService.CaptureException(ex);
    }
    string[] strArray = strArray1;
    int index;
    for (index = 0; index < strArray.Length; ++index)
    {
      string str = strArray[index];
      if (!CloudSaveStore.ShouldSyncFileToCloud(str))
      {
        this.DeleteStaleBackupFromCloud(directoryPath, str);
      }
      else
      {
        string path = $"{directoryPath}/{str}";
        filePathsRead.Add(path);
        Log.Debug($"Checking file {path} in cloud saves");
        yield return this.SyncCloudToLocal(path);
      }
    }
    strArray = (string[]) null;
    if (this.LocalStore.DirectoryExists(directoryPath))
    {
      strArray = this.LocalStore.GetFilesInDirectory(directoryPath);
      for (index = 0; index < strArray.Length; ++index)
      {
        string fileName = strArray[index];
        if (CloudSaveStore.ShouldSyncFileToCloud(fileName))
        {
          string path = $"{directoryPath}/{fileName}";
          if (!filePathsRead.Contains(path))
          {
            Log.Debug($"Checking file {path} in local saves");
            yield return this.SyncCloudToLocal(path);
          }
        }
      }
      strArray = (string[]) null;
    }
  }

  public async Task OverwriteCloudWithLocal(string path, bool forgetImmediately = false)
  {
    if (this.LocalStore.FileExists(path))
    {
      Log.Debug($"Writing file {path} to cloud");
      string content = await this.LocalStore.ReadFileAsync(path);
      try
      {
        await this.CloudStore.WriteFileAsync(path, content);
        if (forgetImmediately)
        {
          try
          {
            Log.Debug("Immediately forgetting " + path);
            this.CloudStore.ForgetFile(path);
          }
          catch (Exception ex)
          {
            Log.Warn($"Cloud forget failed for {path}: {ex.Message}");
          }
        }
        this.SyncLocalTimestamp(path);
      }
      catch (Exception ex)
      {
        Log.Warn($"Cloud write failed for {path}, local file preserved: {ex.Message}");
        this.SyncLocalTimestamp(path);
      }
    }
    else
    {
      try
      {
        if (!this.CloudStore.FileExists(path))
          return;
        Log.Debug($"Deleting file {path} from cloud because it doesn't exist on local");
        this.CloudStore.DeleteFile(path);
      }
      catch (Exception ex)
      {
        Log.Warn($"Cloud delete failed for {path}: {ex.Message}");
      }
    }
  }

  public IEnumerable<Task> OverwriteCloudWithLocalDirectory(
    string directoryPath,
    int? byteLimit,
    int? fileLimit)
  {
    Log.Debug($"Writing all files in directory {directoryPath} to cloud");
    HashSet<string> filePathsRead = new HashSet<string>();
    string[] strArray1 = Array.Empty<string>();
    try
    {
      if (this.CloudStore.DirectoryExists(directoryPath))
        strArray1 = this.CloudStore.GetFilesInDirectory(directoryPath);
    }
    catch (Exception ex)
    {
      Log.Warn($"Failed to list cloud files in {directoryPath}, skipping cloud delete sync: {ex.Message}");
      SentryService.CaptureException(ex);
    }
    string[] strArray = strArray1;
    int index;
    for (index = 0; index < strArray.Length; ++index)
    {
      string str = strArray[index];
      if (!CloudSaveStore.ShouldSyncFileToCloud(str))
      {
        this.DeleteStaleBackupFromCloud(directoryPath, str);
      }
      else
      {
        filePathsRead.Add(str);
        yield return this.OverwriteCloudWithLocal($"{directoryPath}/{str}");
      }
    }
    strArray = (string[]) null;
    if (this.LocalStore.DirectoryExists(directoryPath))
    {
      List<string> list = ((IEnumerable<string>) this.LocalStore.GetFilesInDirectory(directoryPath)).ToList<string>();
      index = 0;
      int totalFilesWritten = 0;
      if (byteLimit.HasValue || fileLimit.HasValue)
        list.Sort((Comparison<string>) ((p1, p2) => this.LocalStore.GetLastModifiedTime($"{directoryPath}/{p2}").CompareTo(this.LocalStore.GetLastModifiedTime($"{directoryPath}/{p1}"))));
      foreach (string fileName in list)
      {
        if (!filePathsRead.Contains(fileName) && CloudSaveStore.ShouldSyncFileToCloud(fileName))
        {
          string path = $"{directoryPath}/{fileName}";
          int bytesToWrite = this.LocalStore.GetFileSize(path);
          bool forgetImmediately = byteLimit.HasValue && index + bytesToWrite > byteLimit.Value || fileLimit.HasValue && totalFilesWritten + 1 > fileLimit.Value;
          if (forgetImmediately)
            Log.Info($"File {fileName} will be immediately forgotten after writing to cloud. Bytes written:{index + bytesToWrite}. Files written: {totalFilesWritten + 1}");
          yield return this.OverwriteCloudWithLocal(path, forgetImmediately);
          index += bytesToWrite;
          ++totalFilesWritten;
        }
      }
    }
  }

  public void ForgetFilesInDirectoryBeforeWritingIfNecessary(
    string directoryPath,
    int bytesToBeWritten,
    int byteLimit,
    int fileLimit)
  {
    try
    {
      this.ForgetFilesInDirectoryBeforeWritingIfNecessaryInternal(directoryPath, bytesToBeWritten, byteLimit, fileLimit);
    }
    catch (Exception ex)
    {
      Log.Warn($"Cloud quota management failed for {directoryPath}: {ex.Message}");
      SentryService.CaptureException(ex);
    }
  }

  private void ForgetFilesInDirectoryBeforeWritingIfNecessaryInternal(
    string directoryPath,
    int bytesToBeWritten,
    int byteLimit,
    int fileLimit)
  {
    int num1 = bytesToBeWritten;
    int num2 = 1;
    string[] filesInDirectory = this.CloudStore.GetFilesInDirectory(directoryPath);
    List<string> stringList1 = new List<string>();
    foreach (string fileName in filesInDirectory)
    {
      if (CloudSaveStore.ShouldSyncFileToCloud(fileName))
      {
        string path = $"{directoryPath}/{fileName}";
        if (this.CloudStore.IsFilePersisted(path))
        {
          stringList1.Add(path);
          num1 += this.CloudStore.GetFileSize(path);
          ++num2;
        }
      }
    }
    if (num1 <= byteLimit && num2 <= fileLimit)
      return;
    stringList1.Sort((Comparison<string>) ((p1, p2) => this.GetLastModifiedTime(p2).CompareTo(this.GetLastModifiedTime(p1))));
    while (num1 > byteLimit || num2 > fileLimit)
    {
      List<string> stringList2 = stringList1;
      string path = stringList2[stringList2.Count - 1];
      num1 -= this.CloudStore.GetFileSize(path);
      --num2;
      Log.Info($"Forgetting file {path} from cloud storage because we're past our quota. Bytes after forgetting: {num1}. Files after forgetting: {num2}");
      this.CloudStore.ForgetFile(path);
      stringList1.RemoveAt(stringList1.Count - 1);
    }
  }

  private static bool ShouldSyncFileToCloud(string fileName) => !fileName.EndsWith(".backup");

  private void DeleteStaleBackupFromCloud(string directoryPath, string cloudPath)
  {
    string path = $"{directoryPath}/{cloudPath}";
    Log.Info("Removing stale .backup file from cloud: " + path);
    try
    {
      this.CloudStore.DeleteFile(path);
    }
    catch (Exception ex)
    {
      Log.Warn($"Failed to remove .backup from cloud {path}: {ex.Message}");
    }
  }

  private void SyncLocalTimestamp(string path)
  {
    try
    {
      this.LocalStore.SetLastModifiedTime(path, this.CloudStore.GetLastModifiedTime(path));
    }
    catch (Exception ex)
    {
      Log.Warn($"Failed to sync timestamp for {path}, will re-sync on next launch: {ex.Message}");
    }
  }

  public bool HasUserEnabledCloudSync() => this.CloudStore.HasUserEnabledCloudSync();
}
