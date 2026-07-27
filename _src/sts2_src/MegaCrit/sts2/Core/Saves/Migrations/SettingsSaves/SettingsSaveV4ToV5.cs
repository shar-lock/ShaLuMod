// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Saves.Migrations.SettingsSaves.SettingsSaveV4ToV5
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

#nullable enable
namespace MegaCrit.Sts2.Core.Saves.Migrations.SettingsSaves;

[Migration(typeof (SettingsSave), 4, 5)]
public class SettingsSaveV4ToV5 : MigrationBase<SettingsSave>
{
  protected override void ApplyMigration(MigratingData saveData)
  {
    saveData.Remove("disabled_mods");
  }
}
