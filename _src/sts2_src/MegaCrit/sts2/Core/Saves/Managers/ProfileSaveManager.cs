// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Saves.Managers.ProfileSaveManager
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using MegaCrit.Sts2.Core.Saves.Migrations;

#nullable enable
namespace MegaCrit.Sts2.Core.Saves.Managers;

public class ProfileSaveManager
{
  public const int maxProfileCount = 3;
  public const string profileSaveFileName = "profile.save";
  private readonly ISaveStore _saveStore;
  private readonly MigrationManager _migrationManager;

  public ProfileSave Profile { get; private set; }

  public ProfileSaveManager(ISaveStore saveStore, MigrationManager migrationManager)
  {
    this._saveStore = saveStore;
    this._migrationManager = migrationManager;
  }

  public static string GetProfileSavePath(bool? forceModState = null)
  {
    return StringExtensions.PathJoin(UserDataPathProvider.GetAccountDir(forceModState), "profile.save");
  }

  public void SaveProfile()
  {
    this.Profile.SchemaVersion = this._migrationManager.GetLatestVersion<ProfileSave>();
    string json = JsonSerializationUtility.ToJson<ProfileSave>(this.Profile);
    this._saveStore.WriteFile(ProfileSaveManager.GetProfileSavePath(), json);
  }

  public ReadSaveResult<ProfileSave> LoadProfile()
  {
    ReadSaveResult<ProfileSave> readSaveResult = this._migrationManager.LoadSave<ProfileSave>(ProfileSaveManager.GetProfileSavePath());
    this.Profile = !readSaveResult.Success || readSaveResult.SaveData == null ? this._migrationManager.CreateNewSave<ProfileSave>() : readSaveResult.SaveData;
    return readSaveResult;
  }
}
