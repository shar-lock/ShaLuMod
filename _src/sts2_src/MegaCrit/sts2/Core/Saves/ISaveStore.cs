// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Saves.ISaveStore
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using System;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Saves;

public interface ISaveStore
{
  string? ReadFile(string path);

  Task<string?> ReadFileAsync(string path);

  void WriteFile(string path, string content);

  void WriteFile(string path, byte[] content);

  Task WriteFileAsync(string path, string content);

  Task WriteFileAsync(string path, byte[] content);

  bool FileExists(string path);

  bool DirectoryExists(string path);

  void DeleteFile(string path);

  void RenameFile(string sourcePath, string destinationPath);

  string[] GetFilesInDirectory(string directoryPath);

  string[] GetDirectoriesInDirectory(string directoryPath);

  void CreateDirectory(string directoryPath);

  void DeleteDirectory(string directoryPath);

  void DeleteTemporaryFiles(string directoryPath);

  DateTimeOffset GetLastModifiedTime(string path);

  int GetFileSize(string path);

  void SetLastModifiedTime(string path, DateTimeOffset time);

  string GetFullPath(string filename);
}
