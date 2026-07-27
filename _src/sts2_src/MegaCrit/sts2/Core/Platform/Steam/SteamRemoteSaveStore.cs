// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Platform.Steam.SteamRemoteSaveStore
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Multiplayer.Transport.Steam;
using MegaCrit.Sts2.Core.Saves;
using Steamworks;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Platform.Steam;

public class SteamRemoteSaveStore : ICloudSaveStore, ISaveStore
{
  public string ReadFile(string path)
  {
    path = this.CanonicalizePath(path);
    int fileSize = SteamRemoteStorage.GetFileSize(path);
    if (fileSize == 0)
    {
      if (!SteamRemoteStorage.FileExists(path))
        throw new FileNotFoundException("Steam remote storage file not found: " + path);
      return string.Empty;
    }
    byte[] numArray = new byte[fileSize];
    int num = SteamRemoteStorage.FileRead(path, numArray, fileSize);
    if (num == 0)
      throw new InvalidOperationException($"Steam remote storage read failed for {path}. Expected {fileSize} bytes, got 0. Steam storage may be corrupted or unavailable.");
    if (num != fileSize)
      Log.Warn($"Steam read returned {num} bytes but expected {fileSize} for {path}. Using partial data.");
    return Encoding.UTF8.GetString(numArray);
  }

  public async Task<string?> ReadFileAsync(string path)
  {
    path = this.CanonicalizePath(path);
    int byteCount = SteamRemoteStorage.GetFileSize(path);
    if (byteCount == 0)
      return string.Empty;
    SteamAPICall_t call = SteamRemoteStorage.FileReadAsync(path, 0U, (uint) byteCount);
    if (SteamAPICall_t.op_Equality(call, SteamAPICall_t.Invalid))
      throw new InvalidOperationException($"Steam remote storage async read request returned invalid API call for path {path} (bytes {byteCount})");
    using (SteamCallResult<RemoteStorageFileReadAsyncComplete_t> callResult = new SteamCallResult<RemoteStorageFileReadAsyncComplete_t>(call, SteamInitializer.DisconnectToken))
    {
      RemoteStorageFileReadAsyncComplete_t task = await callResult.Task;
      if (task.m_eResult != 1)
        throw new SteamRemoteSaveStoreException($"Steam remote storage async read request returned error {task.m_eResult} for path {path}", task.m_eResult);
      byte[] numArray = new byte[byteCount];
      SteamRemoteStorage.FileReadAsyncComplete(task.m_hFileReadAsync, numArray, task.m_cubRead);
      return Encoding.UTF8.GetString(numArray);
    }
  }

  public void WriteFile(string path, string content)
  {
    this.WriteFile(path, Encoding.UTF8.GetBytes(content));
  }

  public void WriteFile(string path, byte[] bytes)
  {
    path = this.CanonicalizePath(path);
    if (!SteamRemoteStorage.FileWrite(path, bytes, bytes.Length))
      throw new InvalidOperationException("Steam Cloud write failed. See ISteamRemoteStorage documentation for possible reasons.");
    Log.Info($"Wrote {bytes.Length} bytes to {path} in steam remote store");
  }

  public Task WriteFileAsync(string path, string content)
  {
    return this.WriteFileAsync(path, Encoding.UTF8.GetBytes(content));
  }

  public async Task WriteFileAsync(string path, byte[] bytes)
  {
    path = this.CanonicalizePath(path);
    SteamAPICall_t call = SteamRemoteStorage.FileWriteAsync(path, bytes, (uint) bytes.Length);
    if (SteamAPICall_t.op_Equality(call, SteamAPICall_t.Invalid))
      throw new InvalidOperationException($"Steam remote storage async write request returned invalid API call for path {path} ({bytes.Length} bytes)");
    using (SteamCallResult<RemoteStorageFileWriteAsyncComplete_t> callResult = new SteamCallResult<RemoteStorageFileWriteAsyncComplete_t>(call, SteamInitializer.DisconnectToken))
    {
      RemoteStorageFileWriteAsyncComplete_t task = await callResult.Task;
      if (task.m_eResult != 1)
        throw new InvalidOperationException($"Steam remote storage async write request returned error {task.m_eResult}");
      Log.Info($"Wrote {bytes.Length} bytes to {path} in steam remote store");
    }
  }

  public bool FileExists(string path) => SteamRemoteStorage.FileExists(this.CanonicalizePath(path));

  public bool DirectoryExists(string path) => true;

  public void DeleteFile(string path)
  {
    path = this.CanonicalizePath(path);
    bool flag1 = SteamRemoteStorage.FileExists(path);
    bool flag2 = SteamRemoteStorage.FileDelete(path);
    if (!flag2 & flag1)
      Log.Error($"Steam remote storage delete FAILED for {path}. File existed but could not be deleted.");
    else if (!flag2 && !flag1)
      Log.Debug($"Steam delete called on non-existent file {path} (no-op)");
    else
      Log.Debug($"Deleted {path} from Steam remote storage");
  }

  public void RenameFile(string sourcePath, string destinationPath)
  {
    sourcePath = this.CanonicalizePath(sourcePath);
    destinationPath = this.CanonicalizePath(destinationPath);
    string content = this.ReadFile(sourcePath);
    this.WriteFile(destinationPath, content);
    this.DeleteFile(sourcePath);
  }

  public string[] GetFilesInDirectory(string directoryPath)
  {
    directoryPath = this.CanonicalizePath(directoryPath);
    int fileCount = SteamRemoteStorage.GetFileCount();
    List<string> stringList = new List<string>();
    for (int index = 0; index < fileCount; ++index)
    {
      int num;
      string fileNameAndSize = SteamRemoteStorage.GetFileNameAndSize(index, ref num);
      if (fileNameAndSize.StartsWith(directoryPath))
      {
        string str = fileNameAndSize.Substring(directoryPath.Length + 1);
        if (!str.Contains('/') && !str.Contains('\\'))
          stringList.Add(str);
      }
    }
    return stringList.ToArray();
  }

  public string[] GetDirectoriesInDirectory(string directoryPath)
  {
    throw new NotImplementedException();
  }

  public void CreateDirectory(string directoryPath)
  {
  }

  public void DeleteDirectory(string directoryPath)
  {
  }

  public void DeleteTemporaryFiles(string directoryPath)
  {
  }

  public string GetFullPath(string filename) => throw new NotImplementedException();

  public DateTimeOffset GetLastModifiedTime(string path)
  {
    path = this.CanonicalizePath(path);
    return SaveTimestamps.FromUnixTimeSecondsOrEpoch(SteamRemoteStorage.GetFileTimestamp(path), path);
  }

  public int GetFileSize(string path)
  {
    path = this.CanonicalizePath(path);
    return SteamRemoteStorage.GetFileSize(path);
  }

  public void SetLastModifiedTime(string path, DateTimeOffset time)
  {
    throw new NotImplementedException();
  }

  public string CanonicalizePath(string path) => path.Replace("user://", "").Replace("\\", "/");

  public bool HasCloudFiles() => SteamRemoteStorage.GetFileCount() > 0;

  public void ForgetFile(string path)
  {
    path = this.CanonicalizePath(path);
    if (!SteamRemoteStorage.FileForget(path))
      throw new InvalidOperationException($"Tried to forget file at path {path} from steam storage, but false was returned from {"FileForget"}!");
  }

  public bool IsFilePersisted(string path)
  {
    path = this.CanonicalizePath(path);
    return SteamRemoteStorage.FilePersisted(path);
  }

  public void BeginSaveBatch()
  {
    if (SteamRemoteStorage.BeginFileWriteBatch())
      return;
    Log.Warn("SteamRemoteStorage.BeginFileWriteBatch returned false (a batch may already be in progress)");
  }

  public void EndSaveBatch()
  {
    if (SteamRemoteStorage.EndFileWriteBatch())
      return;
    Log.Warn("SteamRemoteStorage.EndFileWriteBatch returned false (no batch was in progress)");
  }

  public bool HasUserEnabledCloudSync()
  {
    return SteamRemoteStorage.IsCloudEnabledForAccount() && SteamRemoteStorage.IsCloudEnabledForApp();
  }
}
