// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Saves.Test.MockGodotFileIo
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Platform.Steam;
using Steamworks;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Saves.Test;

public class MockGodotFileIo : ISaveStore
{
  protected readonly ConcurrentDictionary<string, MockGodotFileIo.File> _files = new ConcurrentDictionary<string, MockGodotFileIo.File>();
  protected readonly ConcurrentDictionary<string, List<string>> _directories = new ConcurrentDictionary<string, List<string>>();
  protected readonly string _saveDir;
  public Func<DateTimeOffset>? getCurrentTime;
  public bool ShouldFailWrites;
  public bool ShouldFailTimestampSync;
  public bool DoSteamSpecificError;
  protected readonly ConcurrentDictionary<string, long> _rawTimestampSeconds = new ConcurrentDictionary<string, long>();

  public List<(string Method, object[] Args)> Calls { get; } = new List<(string, object[])>();

  public Action<string, string>? RenameFileAction { get; set; }

  public MockGodotFileIo(string saveDir)
  {
    this.CanonicalizePath(ref saveDir, false);
    this._saveDir = saveDir;
    this.CreateDirectory(this._saveDir);
  }

  public void SetRawTimestampSeconds(string path, long seconds)
  {
    this.CanonicalizePath(ref path);
    this._rawTimestampSeconds[path] = seconds;
  }

  public DateTimeOffset GetLastModifiedTime(string path)
  {
    this.CanonicalizePath(ref path);
    long seconds;
    if (this._rawTimestampSeconds.TryGetValue(path, out seconds))
      return SaveTimestamps.FromUnixTimeSecondsOrEpoch(seconds, path);
    MockGodotFileIo.File file;
    if (!this._files.TryGetValue(path, out file))
      throw new InvalidOperationException($"No file at {path}!");
    if (!file.lastModifiedTime.HasValue)
      throw new InvalidOperationException($"getCurrentTime was not set when file {path} was created!");
    return file.lastModifiedTime.Value;
  }

  public int GetFileSize(string path)
  {
    this.CanonicalizePath(ref path);
    MockGodotFileIo.File file;
    if (!this._files.TryGetValue(path, out file))
      throw new InvalidOperationException($"No file at {path}!");
    return Encoding.UTF8.GetByteCount(file.content);
  }

  public void SetLastModifiedTime(string path, DateTimeOffset time)
  {
    this.CanonicalizePath(ref path);
    if (this.ShouldFailTimestampSync)
      throw new IOException("Simulated timestamp sync failure for " + path);
    MockGodotFileIo.File file;
    if (!this._files.TryGetValue(path, out file))
      throw new InvalidOperationException($"No file at {path}!");
    file.lastModifiedTime = new DateTimeOffset?(time);
  }

  public string GetFullPath(string filename)
  {
    this.Calls.Add((nameof (GetFullPath), new object[1]
    {
      (object) filename
    }));
    this.CanonicalizePath(ref filename);
    return filename;
  }

  public void SetFileContent(string path, string content)
  {
    this.CanonicalizePath(ref path);
    MockGodotFileIo.File file;
    if (!this._files.TryGetValue(path, out file))
      throw new InvalidOperationException($"Cannot set content: no file at {path}. Write the file first.");
    file.content = content;
  }

  public string? ReadFile(string path)
  {
    this.CanonicalizePath(ref path);
    this.Calls.Add((nameof (ReadFile), new object[1]
    {
      (object) path
    }));
    MockGodotFileIo.File file;
    return !this._files.TryGetValue(path, out file) ? (string) null : file.content;
  }

  public Task<string?> ReadFileAsync(string path)
  {
    if (this.DoSteamSpecificError)
      throw new SteamRemoteSaveStoreException("Simulating Steam Error", (EResult) 9);
    this.CanonicalizePath(ref path);
    this.Calls.Add((nameof (ReadFileAsync), new object[1]
    {
      (object) path
    }));
    MockGodotFileIo.File file;
    return Task.FromResult<string>(this._files.TryGetValue(path, out file) ? file.content : (string) null);
  }

  public void WriteFile(string path, string content)
  {
    this.CanonicalizePath(ref path);
    this.Calls.Add((nameof (WriteFile), new object[2]
    {
      (object) path,
      (object) content
    }));
    if (this.ShouldFailWrites)
      throw new InvalidOperationException("Simulated write failure");
    string key = path + ".backup";
    MockGodotFileIo.File file1;
    if (this._files.TryGetValue(path, out file1))
      this._files[key] = file1;
    MockGodotFileIo.File file2 = new MockGodotFileIo.File();
    file2.content = content;
    Func<DateTimeOffset> getCurrentTime = this.getCurrentTime;
    file2.lastModifiedTime = getCurrentTime != null ? new DateTimeOffset?(getCurrentTime()) : new DateTimeOffset?();
    MockGodotFileIo.File file3 = file2;
    this._files[path] = file3;
  }

  public void WriteFile(string path, byte[] bytes)
  {
    this.WriteFile(path, Encoding.UTF8.GetString(bytes));
  }

  public Task WriteFileAsync(string path, string content)
  {
    this.WriteFile(path, content);
    return Task.CompletedTask;
  }

  public Task WriteFileAsync(string path, byte[] bytes)
  {
    this.WriteFile(path, Encoding.UTF8.GetString(bytes));
    return Task.CompletedTask;
  }

  public bool FileExists(string path)
  {
    this.CanonicalizePath(ref path);
    this.Calls.Add((nameof (FileExists), new object[1]
    {
      (object) path
    }));
    return this._files.ContainsKey(path);
  }

  public bool DirectoryExists(string path) => true;

  public void DeleteFile(string path)
  {
    this.CanonicalizePath(ref path);
    this.Calls.Add((nameof (DeleteFile), new object[1]
    {
      (object) path
    }));
    MockGodotFileIo.File file;
    CollectionExtensions.Remove<string, MockGodotFileIo.File>((IDictionary<string, MockGodotFileIo.File>) this._files, path, ref file);
  }

  public void RenameFile(string sourcePath, string destinationPath)
  {
    this.Calls.Add((nameof (RenameFile), new object[2]
    {
      (object) sourcePath,
      (object) destinationPath
    }));
    if (this.RenameFileAction != null)
    {
      this.CanonicalizePath(ref sourcePath, false);
      this.CanonicalizePath(ref destinationPath, false);
      this.RenameFileAction(sourcePath, destinationPath);
    }
    else
    {
      this.CanonicalizePath(ref sourcePath);
      this.CanonicalizePath(ref destinationPath);
      MockGodotFileIo.File file;
      if (!CollectionExtensions.Remove<string, MockGodotFileIo.File>((IDictionary<string, MockGodotFileIo.File>) this._files, sourcePath, ref file))
        return;
      this._files[destinationPath] = file;
    }
  }

  public string[] GetFilesInDirectory(string directoryPath)
  {
    this.CanonicalizePath(ref directoryPath);
    this.Calls.Add((nameof (GetFilesInDirectory), new object[1]
    {
      (object) directoryPath
    }));
    string prefix = directoryPath.EndsWith('/') ? directoryPath : directoryPath + "/";
    return this._files.Keys.Where<string>((Func<string, bool>) (path => path.StartsWith(prefix))).Select<string, string>((Func<string, string>) (path => Path.GetFileName(path))).ToArray<string>();
  }

  public string[] GetDirectoriesInDirectory(string directoryPath)
  {
    this.CanonicalizePath(ref directoryPath);
    this.Calls.Add((nameof (GetDirectoriesInDirectory), new object[1]
    {
      (object) directoryPath
    }));
    string prefix = directoryPath.EndsWith('/') ? directoryPath : directoryPath + "/";
    return this._files.Keys.Where<string>((Func<string, bool>) (path => path.StartsWith(prefix))).Select<string, string>((Func<string, string>) (path =>
    {
      string str = path;
      int startIndex = prefix.Length + 1;
      return str.Substring(startIndex, str.Length - startIndex);
    })).Select<string, string>((Func<string, string>) (path => ((FileSystemInfo) new DirectoryInfo(path).Root).Name)).ToArray<string>();
  }

  public void CreateDirectory(string directoryPath)
  {
    this.CanonicalizePath(ref directoryPath);
    this.Calls.Add((nameof (CreateDirectory), new object[1]
    {
      (object) directoryPath
    }));
    if (this._directories.ContainsKey(directoryPath))
      return;
    this._directories[directoryPath] = new List<string>();
  }

  public void DeleteDirectory(string directoryPath)
  {
    this.CanonicalizePath(ref directoryPath);
    this.Calls.Add((nameof (DeleteDirectory), new object[1]
    {
      (object) directoryPath
    }));
  }

  public void DeleteTemporaryFiles(string directoryPath)
  {
    this.CanonicalizePath(ref directoryPath);
    this.Calls.Add((nameof (DeleteTemporaryFiles), new object[1]
    {
      (object) directoryPath
    }));
    string prefix = directoryPath.EndsWith('/') ? directoryPath : directoryPath + "/";
    foreach (string str in this._files.Keys.Where<string>((Func<string, bool>) (path => path.StartsWith(prefix) && path.EndsWith(".tmp"))).ToList<string>())
    {
      MockGodotFileIo.File file;
      CollectionExtensions.Remove<string, MockGodotFileIo.File>((IDictionary<string, MockGodotFileIo.File>) this._files, str, ref file);
    }
  }

  protected void CanonicalizePath(ref string path, bool getFullPath = true)
  {
    path = path.Replace('\\', '/');
    if (!getFullPath)
      return;
    path = $"{this._saveDir}/{path}";
  }

  public static class Methods
  {
    public const string writeFile = "WriteFile";
    public const string writeFileAsync = "WriteFileAsync";
    public const string readFile = "ReadFile";
    public const string readFileAsync = "ReadFileAsync";
    public const string fileExists = "FileExists";
    public const string renameFile = "RenameFile";
    public const string deleteFile = "DeleteFile";
    public const string getFullPath = "GetFullPath";
    public const string getDirectoriesInDirectory = "GetDirectoriesInDirectory";
    public const string getFilesInDirectory = "GetFilesInDirectory";
    public const string createDirectory = "CreateDirectory";
    public const string deleteDirectory = "DeleteDirectory";
    public const string deleteTemporaryFiles = "DeleteTemporaryFiles";
    public const string getLastModifiedTime = "GetLastModifiedTime";
  }

  protected class File
  {
    public required string content;
    public DateTimeOffset? lastModifiedTime;
    public bool forgotten;
  }
}
