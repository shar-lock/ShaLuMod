// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Saves.Managers.PrefsSaveManager
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using MegaCrit.Sts2.Core.Saves.Migrations;

#nullable enable
namespace MegaCrit.Sts2.Core.Saves.Managers;

public class PrefsSaveManager
{
  public const string fileName = "prefs.save";
  private readonly ISaveStore _saveStore;
  private readonly MigrationManager _migrationManager;
  private readonly IProfileIdProvider _profileIdProvider;

  public PrefsSave Prefs { get; set; }

  public PrefsSaveManager(int profileId, ISaveStore saveStore, MigrationManager migrationManager)
    : this(saveStore, migrationManager, (IProfileIdProvider) new StaticProfileIdProvider(profileId))
  {
  }

  public PrefsSaveManager(
    ISaveStore saveStore,
    MigrationManager migrationManager,
    IProfileIdProvider profileIdProvider)
  {
    this._saveStore = saveStore;
    this._migrationManager = migrationManager;
    this._profileIdProvider = profileIdProvider;
  }

  public static string GetPrefsPath(int profileId, bool? forceModState = null)
  {
    return StringExtensions.PathJoin(StringExtensions.PathJoin(UserDataPathProvider.GetProfileDir(profileId, forceModState), UserDataPathProvider.SavesDir), "prefs.save");
  }

  public void SavePrefs()
  {
    this.Prefs.SchemaVersion = this._migrationManager.GetLatestVersion<PrefsSave>();
    string json = JsonSerializationUtility.ToJson<PrefsSave>(this.Prefs);
    this._saveStore.WriteFile(PrefsSaveManager.GetPrefsPath(this._profileIdProvider.CurrentProfileId), json);
  }

  public ReadSaveResult<PrefsSave> LoadPrefs()
  {
    ReadSaveResult<PrefsSave> readSaveResult = this._migrationManager.LoadSave<PrefsSave>(PrefsSaveManager.GetPrefsPath(this._profileIdProvider.CurrentProfileId));
    if (!readSaveResult.Success || readSaveResult.SaveData == null)
    {
      this.Prefs = this._migrationManager.CreateNewSave<PrefsSave>();
      this.SavePrefs();
    }
    else
      this.Prefs = readSaveResult.SaveData;
    return readSaveResult;
  }
}
