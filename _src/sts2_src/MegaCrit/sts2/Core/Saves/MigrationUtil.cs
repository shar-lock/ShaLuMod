// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Saves.MigrationUtil
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Logging;
using System;
using System.IO;

#nullable enable
namespace MegaCrit.Sts2.Core.Saves;

public static class MigrationUtil
{
  public static bool MigrateDirectory(
    string directoryName,
    string legacyBasePath,
    string newBasePath)
  {
    string sourceDir = Path.Combine(legacyBasePath, directoryName);
    string targetDir = Path.Combine(newBasePath, directoryName);
    if (!Directory.Exists(sourceDir))
    {
      Log.Info($"No legacy {directoryName} directory found, skipping");
      return false;
    }
    if (Directory.Exists(targetDir))
    {
      Log.Info($"Skipping migration because {targetDir} already exists");
      return false;
    }
    Log.Info($"Migrating {directoryName} directory from {sourceDir} to {targetDir}");
    try
    {
      Directory.CreateDirectory(targetDir);
      MigrationUtil.CopyDirectoryRecursively(sourceDir, targetDir);
      Log.Info($"Successfully migrated {directoryName} directory");
      return true;
    }
    catch (Exception ex)
    {
      Log.Error($"Failed to migrate {directoryName} directory: {ex.Message}");
      return false;
    }
  }

  public static bool MigrateFile(string fileName, string legacyBasePath, string newBasePath)
  {
    string str1 = Path.Combine(legacyBasePath, fileName);
    string str2 = Path.Combine(newBasePath, fileName);
    if (!File.Exists(str1))
    {
      Log.Info($"No legacy {fileName} file found, skipping");
      return false;
    }
    Log.Info($"Migrating {fileName} from {str1} to {str2}");
    try
    {
      Directory.CreateDirectory(Path.GetDirectoryName(str2));
      File.Copy(str1, str2, true);
      Log.Info("Successfully migrated " + fileName);
      return true;
    }
    catch (Exception ex)
    {
      Log.Error($"Failed to migrate {fileName}: {ex.Message}");
      return false;
    }
  }

  public static void CopyDirectoryRecursively(string sourceDir, string targetDir)
  {
    Directory.CreateDirectory(targetDir);
    foreach (string file in Directory.GetFiles(sourceDir))
    {
      string fileName = Path.GetFileName(file);
      string str = Path.Combine(targetDir, fileName);
      File.Copy(file, str, true);
    }
    foreach (string directory in Directory.GetDirectories(sourceDir))
    {
      string fileName = Path.GetFileName(directory);
      string targetDir1 = Path.Combine(targetDir, fileName);
      MigrationUtil.CopyDirectoryRecursively(directory, targetDir1);
    }
  }

  public static void ArchiveLegacyDirectory(string directoryPath, string archivePath)
  {
    if (!Directory.Exists(directoryPath))
      return;
    try
    {
      string fileName = Path.GetFileName(directoryPath);
      string str = Path.Combine(archivePath, fileName);
      Directory.Move(directoryPath, str);
      Log.Info($"Archived legacy directory: {directoryPath} -> {str}");
    }
    catch (Exception ex)
    {
      Log.Warn($"Could not archive legacy directory {directoryPath}: {ex.Message}");
    }
  }

  public static void ArchiveLegacyFile(string filePath, string archivePath)
  {
    if (!File.Exists(filePath))
      return;
    try
    {
      string fileName = Path.GetFileName(filePath);
      string str = Path.Combine(archivePath, fileName);
      File.Move(filePath, str);
      Log.Info($"Archived legacy file: {filePath} -> {str}");
    }
    catch (Exception ex)
    {
      Log.Warn($"Could not archive legacy file {filePath}: {ex.Message}");
    }
  }
}
