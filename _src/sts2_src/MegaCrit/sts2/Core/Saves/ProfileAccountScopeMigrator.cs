// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Saves.ProfileAccountScopeMigrator
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using MegaCrit.Sts2.Core.Logging;
using System.IO;

#nullable disable
namespace MegaCrit.Sts2.Core.Saves;

public static class ProfileAccountScopeMigrator
{
  private static bool _migrationPerformed;

  public static void MigrateToProfileScopedDirectories()
  {
    if (!ProfileAccountScopeMigrator.HasLegacyData())
    {
      Log.VeryDebug("No legacy unscoped data found, skipping migration");
    }
    else
    {
      Log.Info("Starting migration of account-scoped data to profile-scoped directories");
      string str = ProjectSettings.GlobalizePath(UserDataPathProvider.GetAccountScopedBasePath(""));
      string newBasePath = ProjectSettings.GlobalizePath(UserDataPathProvider.GetProfileScopedBasePath(1));
      Directory.CreateDirectory(newBasePath);
      ProfileAccountScopeMigrator._migrationPerformed = MigrationUtil.MigrateFile("settings.save", str + "/saves", str) | MigrationUtil.MigrateDirectory("saves", str, newBasePath) | MigrationUtil.MigrateDirectory("replays", str, newBasePath) | MigrationUtil.MigrateFile("console_history.log", str, newBasePath);
      if (ProfileAccountScopeMigrator._migrationPerformed)
        Log.Info("Migration to profile-scoped directories completed successfully");
      else
        Log.Info("No items were migrated (all destinations already existed)");
    }
  }

  private static bool HasLegacyData()
  {
    return DirAccess.DirExistsAbsolute(UserDataPathProvider.GetAccountScopedBasePath("saves")) || DirAccess.DirExistsAbsolute(UserDataPathProvider.GetAccountScopedBasePath("replays"));
  }

  public static void ArchiveLegacyData()
  {
    if (!ProfileAccountScopeMigrator._migrationPerformed)
    {
      Log.VeryDebug("No migration was performed, skipping archival of legacy data");
    }
    else
    {
      if (!ProfileAccountScopeMigrator.HasLegacyData())
        return;
      Log.Info("Archiving account-scoped data after successful profile-scoped migration");
      string str = ProjectSettings.GlobalizePath(UserDataPathProvider.GetAccountScopedBasePath(""));
      string archivePath = Path.Combine(str, "legacy_backup");
      if (Directory.Exists(archivePath))
      {
        Log.Warn("Deleting legacy data archive that already exists");
        Directory.Delete(archivePath, true);
      }
      Directory.CreateDirectory(archivePath);
      MigrationUtil.ArchiveLegacyDirectory(Path.Combine(str, "saves"), archivePath);
      MigrationUtil.ArchiveLegacyDirectory(Path.Combine(str, "replays"), archivePath);
      MigrationUtil.ArchiveLegacyFile(Path.Combine(str, "console_history.log"), archivePath);
      File.Delete(Path.Combine(ProjectSettings.GlobalizePath(UserDataPathProvider.GetProfileScopedBasePath(1)), "saves/settings.save"));
      Log.Info("Legacy data archived to 'legacy_backup' folder");
    }
  }
}
