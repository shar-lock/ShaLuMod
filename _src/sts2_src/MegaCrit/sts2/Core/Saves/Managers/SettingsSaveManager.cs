// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Saves.Managers.SettingsSaveManager
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using MegaCrit.Sts2.Core.Platform.Steam;
using MegaCrit.Sts2.Core.Saves.Migrations;
using Steamworks;

#nullable enable
namespace MegaCrit.Sts2.Core.Saves.Managers;

public class SettingsSaveManager
{
  public const string settingsSaveFileName = "settings.save";
  private readonly ISaveStore _saveStore;
  private readonly MigrationManager _migrationManager;

  private string SettingsPath => "settings.save";

  public SettingsSave Settings { get; set; }

  public SettingsSaveManager(ISaveStore saveStore, MigrationManager migrationManager)
  {
    this._saveStore = saveStore;
    this._migrationManager = migrationManager;
  }

  public void SaveSettings()
  {
    this.Settings.SchemaVersion = this._migrationManager.GetLatestVersion<SettingsSave>();
    this._saveStore.WriteFile(this.SettingsPath, JsonSerializationUtility.ToJson<SettingsSave>(this.Settings));
  }

  public ReadSaveResult<SettingsSave> LoadSettings()
  {
    ReadSaveResult<SettingsSave> readSaveResult = this._migrationManager.LoadSave<SettingsSave>(this.SettingsPath);
    if (!readSaveResult.Success || readSaveResult.SaveData == null)
    {
      this.Settings = this._migrationManager.CreateNewSave<SettingsSave>();
      this.ApplyPlatformDefaults(this.Settings);
      this.SaveSettings();
    }
    else
      this.Settings = readSaveResult.SaveData;
    return readSaveResult;
  }

  private void ApplyPlatformDefaults(SettingsSave settings)
  {
    if (!SteamInitializer.Initialized || !SteamUtils.IsSteamRunningOnSteamDeck())
      return;
    settings.Fullscreen = true;
    settings.WindowPosition = new Vector2I(0, 0);
  }
}
