// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Saves.AccountScopeUserDataMigrator
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using MegaCrit.Sts2.Core.Logging;
using System.IO;

#nullable disable
namespace MegaCrit.Sts2.Core.Saves;

public static class AccountScopeUserDataMigrator
{
  private static bool _migrationPerformed;

  public static void MigrateToUserScopedDirectories()
  {
    if (!AccountScopeUserDataMigrator.HasLegacyData())
    {
      Log.VeryDebug("No legacy unscoped data found, skipping migration");
    }
    else
    {
      Log.Info("Starting migration of legacy unscoped data to user-scoped directories");
      string legacyBasePath = ProjectSettings.GlobalizePath("user://");
      string newBasePath = ProjectSettings.GlobalizePath(UserDataPathProvider.GetProfileScopedBasePath(1));
      Directory.CreateDirectory(newBasePath);
      AccountScopeUserDataMigrator._migrationPerformed = MigrationUtil.MigrateDirectory("saves", legacyBasePath, newBasePath) | MigrationUtil.MigrateDirectory("replays", legacyBasePath, newBasePath) | MigrationUtil.MigrateFile("console_history.log", legacyBasePath, newBasePath);
      if (AccountScopeUserDataMigrator._migrationPerformed)
        Log.Info("Migration to user-scoped directories completed successfully");
      else
        Log.Info("No items were migrated (all destinations already existed)");
    }
  }

  private static bool HasLegacyData()
  {
    string str = ProjectSettings.GlobalizePath("user://");
    return Directory.Exists(Path.Combine(str, "saves")) || Directory.Exists(Path.Combine(str, "replays")) || File.Exists(Path.Combine(str, "console_history.log"));
  }

  public static void ArchiveLegacyData()
  {
    if (!AccountScopeUserDataMigrator._migrationPerformed)
    {
      Log.VeryDebug("No migration was performed, skipping archival of legacy data");
    }
    else
    {
      if (!AccountScopeUserDataMigrator.HasLegacyData())
        return;
      Log.Info("Archiving legacy unscoped data after successful migration");
      string str = ProjectSettings.GlobalizePath("user://");
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
      Log.Info("Legacy data archived to 'legacy_backup' folder");
    }
  }
}
