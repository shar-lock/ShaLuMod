// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Saves.GodotFileIo
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using MegaCrit.Sts2.Core.Exceptions;
using MegaCrit.Sts2.Core.Logging;
using System;
using System.IO;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Saves;

public class GodotFileIo : ISaveStore
{
  public string SaveDir { get; set; }

  public GodotFileIo(string saveDir)
  {
    this.SaveDir = saveDir;
    this.CreateDirectory(this.SaveDir);
  }

  public string GetFullPath(string filename)
  {
    return filename.StartsWith(this.SaveDir) ? filename : $"{this.SaveDir}/{filename}";
  }

  public string? ReadFile(string path)
  {
    path = this.GetFullPath(path);
    using (FileAccess fileAccess = FileAccess.Open(path, (FileAccess.ModeFlags) 1L))
    {
      if (fileAccess == null)
      {
        Error openError = FileAccess.GetOpenError();
        if (openError == 7L)
        {
          Log.Warn($"Tried to read file at {path}, but there was no such file");
          return (string) null;
        }
        throw new SaveException($"Failed to open file for reading. path='{path}' error={openError}");
      }
      string asText = fileAccess.GetAsText(false);
      fileAccess.Close();
      return asText;
    }
  }

  public async Task<string?> ReadFileAsync(string path)
  {
    path = this.GetFullPath(path);
    GodotFileIo.ValidateGodotFilePath(path);
    MemoryStream memoryStream;
    await using (FileAccessStream stream = new FileAccessStream(path, (FileAccess.ModeFlags) 1L))
    {
      memoryStream = new MemoryStream();
      try
      {
        await stream.CopyToAsync((Stream) memoryStream);
        return Encoding.UTF8.GetString(memoryStream.ToArray());
      }
      finally
      {
        ((IDisposable) memoryStream)?.Dispose();
      }
    }
    memoryStream = (MemoryStream) null;
    string str;
    return str;
  }

  public DateTimeOffset GetLastModifiedTime(string path)
  {
    path = this.GetFullPath(path);
    return SaveTimestamps.FromUnixTimeSecondsOrEpoch((long) FileAccess.GetModifiedTime(path), path);
  }

  public int GetFileSize(string path)
  {
    path = this.GetFullPath(path);
    return (int) FileAccess.GetSize(path);
  }

  public void SetLastModifiedTime(string path, DateTimeOffset time)
  {
    path = this.GetFullPath(path);
    File.SetLastWriteTimeUtc(ProjectSettings.GlobalizePath(path), time.UtcDateTime);
  }

  public void WriteFile(string path, string content)
  {
    if (string.IsNullOrWhiteSpace(content))
      Log.Error($"The content is empty for path='{path}'");
    else
      this.WriteFile(path, Encoding.UTF8.GetBytes(content));
  }

  public void WriteFile(string path, byte[] bytes)
  {
    path = this.GetFullPath(path);
    GodotFileIo.ValidateGodotFilePath(path);
    this.CopyBackup(path);
    string tempPath = path + ".tmp";
    FileWriteRetry.Run(tempPath, (Action) (() => this.StoreBufferToFile(tempPath, bytes, path)));
    GodotFileIo.FsyncFile(tempPath);
    this.RenameFile(tempPath, path);
    Log.Info($"Wrote {bytes.Length} bytes to path={path} save_dir={this.SaveDir}");
  }

  private void StoreBufferToFile(string tempPath, byte[] bytes, string path)
  {
    using (FileAccess fileAccess = FileAccess.Open(tempPath, (FileAccess.ModeFlags) 2L))
    {
      if (fileAccess == null)
        throw new SaveException($"Failed to open file for writing. path='{tempPath}' error={FileAccess.GetOpenError()}");
      if (!fileAccess.StoreBuffer(bytes))
        throw new SaveException($"Failed to write {bytes.Length} bytes to path={path} save_dir={this.SaveDir}. Error: {fileAccess.GetError()}");
    }
  }

  public Task WriteFileAsync(string path, string content)
  {
    return this.WriteFileAsync(path, Encoding.UTF8.GetBytes(content));
  }

  public async Task WriteFileAsync(string path, byte[] bytes)
  {
    path = this.GetFullPath(path);
    GodotFileIo.ValidateGodotFilePath(path);
    this.CopyBackup(path);
    string tempPath = path + ".tmp";
    long bytesWritten = 0;
    await FileWriteRetry.RunAsync(tempPath, (Func<Task>) (async () =>
    {
      FileAccessStream stream = new FileAccessStream(tempPath, (FileAccess.ModeFlags) 2L);
      object obj = (object) null;
      int num = 0;
      try
      {
        await stream.WriteAsync(ReadOnlyMemory<byte>.op_Implicit(bytes), new CancellationToken());
        bytesWritten = ((Stream) stream).Position;
        stream.Close();
        num = 1;
      }
      catch (object ex)
      {
        obj = ex;
      }
      if (stream != null)
        await stream.DisposeAsync();
      object obj1 = obj;
      if (obj1 != null)
      {
        if (!(obj1 is Exception source2))
          throw obj1;
        ExceptionDispatchInfo.Capture(source2).Throw();
      }
      if (num == 1)
      {
        stream = (FileAccessStream) null;
      }
      else
      {
        obj = (object) null;
        stream = (FileAccessStream) null;
        stream = (FileAccessStream) null;
      }
    }));
    GodotFileIo.FsyncFile(tempPath);
    this.RenameFile(tempPath, path);
    Log.Info($"Wrote {bytesWritten} bytes to path={path} save_dir={this.SaveDir}");
  }

  public bool FileExists(string path) => FileAccess.FileExists(this.GetFullPath(path));

  public bool DirectoryExists(string path) => DirAccess.DirExistsAbsolute(this.GetFullPath(path));

  public void DeleteFile(string path)
  {
    Error error = DirAccess.RemoveAbsolute(this.GetFullPath(path));
    if (error == null || error == 7L)
      return;
    Log.Error($"Error deleting path {path}: {error}");
  }

  public void RenameFile(string sourcePath, string destinationPath)
  {
    sourcePath = this.FileExists(sourcePath) ? this.GetFullPath(sourcePath) : throw new SaveException("Cannot rename file: source does not exist. source=" + this.GetFullPath(sourcePath));
    destinationPath = this.GetFullPath(destinationPath);
    Error error = (Error) 1L;
    for (int index = 1; index <= 4; ++index)
    {
      error = DirAccess.RenameAbsolute(sourcePath, destinationPath);
      if (error == null)
        return;
      if (FileAccess.FileExists(destinationPath) && !FileAccess.FileExists(sourcePath))
      {
        Log.Warn($"Rename reported error={error} but destination exists, treating as success. source={sourcePath}");
        return;
      }
      if (index < 4)
      {
        Log.Warn($"Rename failed (attempt {index}/{4}), retrying. error={error} source={sourcePath}");
        Thread.Sleep(50);
      }
    }
    Thread.Sleep(100);
    if (FileAccess.FileExists(destinationPath) && !FileAccess.FileExists(sourcePath))
      Log.Warn("Rename appeared to fail but destination exists after delay, treating as success. source=" + sourcePath);
    else
      throw new SaveException($"Failed to rename file. error={error} source={sourcePath} destination={destinationPath} source_exists={FileAccess.FileExists(sourcePath)} destination_exists={FileAccess.FileExists(destinationPath)}");
  }

  public string[] GetFilesInDirectory(string directoryPath)
  {
    directoryPath = this.GetFullPath(directoryPath);
    return DirAccess.GetFilesAt(directoryPath);
  }

  public string[] GetDirectoriesInDirectory(string directoryPath)
  {
    directoryPath = this.GetFullPath(directoryPath);
    return DirAccess.GetDirectoriesAt(directoryPath);
  }

  public void CreateDirectory(string directoryPath)
  {
    directoryPath = this.GetFullPath(directoryPath);
    if (DirAccess.DirExistsAbsolute(directoryPath))
      return;
    DirAccess.MakeDirRecursiveAbsolute(directoryPath);
  }

  public void DeleteDirectory(string directoryPath)
  {
    directoryPath = this.GetFullPath(directoryPath);
    if (!DirAccess.DirExistsAbsolute(directoryPath))
      return;
    using (DirAccess dirAccess = DirAccess.Open(directoryPath))
    {
      dirAccess.IncludeHidden = true;
      foreach (string file in dirAccess.GetFiles())
      {
        Error error = dirAccess.Remove(file);
        if (error != null)
          throw new InvalidOperationException($"Got error {error} trying to delete file {file} in directory {directoryPath}");
      }
      foreach (string directory in dirAccess.GetDirectories())
        this.DeleteDirectory($"{directoryPath}/{directory}");
      Error error1 = dirAccess.Remove("");
      if (error1 != null)
        throw new InvalidOperationException($"Got error {error1} trying to delete directory {directoryPath}");
    }
  }

  public void DeleteTemporaryFiles(string directoryPath)
  {
    directoryPath = this.GetFullPath(directoryPath);
    using (DirAccess dirAccess = DirAccess.Open(directoryPath))
    {
      if (dirAccess == null)
        return;
      foreach (string file in dirAccess.GetFiles())
      {
        if (file.EndsWith(".tmp"))
        {
          Log.Info($"Cleaning up orphaned {file} in {directoryPath}");
          Error error = dirAccess.Remove(file);
          if (error != null)
            Log.Warn($"Couldn't delete temporary file {file} in {directoryPath}, error={error}");
        }
      }
    }
  }

  private void CopyBackup(string fullPath)
  {
    if (!FileAccess.FileExists(fullPath))
      return;
    string destinationPath = fullPath + ".backup";
    string str = fullPath + ".backup.tmp";
    using (FileAccess fileAccess1 = FileAccess.Open(fullPath, (FileAccess.ModeFlags) 1L))
    {
      if (fileAccess1 == null)
      {
        Log.Warn($"Failed to open source for backup copy. path={fullPath} error={FileAccess.GetOpenError()}");
      }
      else
      {
        byte[] buffer = fileAccess1.GetBuffer((long) fileAccess1.GetLength());
        fileAccess1.Close();
        using (FileAccess fileAccess2 = FileAccess.Open(str, (FileAccess.ModeFlags) 2L))
        {
          if (fileAccess2 == null)
            Log.Warn($"Failed to open backup for writing. path={str} error={FileAccess.GetOpenError()}");
          else if (!fileAccess2.StoreBuffer(buffer))
          {
            Log.Warn($"Copying backup from {fullPath} to {str} failed: {fileAccess2.GetError()}");
          }
          else
          {
            fileAccess2.Close();
            try
            {
              GodotFileIo.FsyncFile(str);
              this.RenameFile(str, destinationPath);
            }
            catch (Exception ex)
            {
              Log.Warn($"Failed to finalize backup for {fullPath}: {ex.Message}");
            }
          }
        }
      }
    }
  }

  private static void FsyncFile(string godotPath)
  {
    try
    {
      using (FileStream fileStream = new FileStream(ProjectSettings.GlobalizePath(godotPath), (FileMode) 3, (FileAccess) 2, (FileShare) 3))
        fileStream.Flush(true);
    }
    catch (Exception ex)
    {
      Log.Warn($"Failed to fsync {godotPath}: {ex.Message}");
    }
  }

  private static void ValidateGodotFilePath(string godotFilePath)
  {
    string str = godotFilePath.Contains("://") ? StringExtensions.GetBaseDir(godotFilePath) : throw new SaveException($"The path='{godotFilePath}' is not a godot file path");
    if (DirAccess.DirExistsAbsolute(str))
      return;
    DirAccess.MakeDirRecursiveAbsolute(str);
  }
}
