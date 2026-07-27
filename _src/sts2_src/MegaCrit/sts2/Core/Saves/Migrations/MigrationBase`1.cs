// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Saves.Migrations.MigrationBase`1
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using System;

#nullable enable
namespace MegaCrit.Sts2.Core.Saves.Migrations;

public abstract class MigrationBase<T> : IMigration<T>, IMigration where T : ISaveSchema
{
  private readonly Lazy<MigrationAttribute> _migrationAttribute;

  protected MigrationBase()
  {
    this._migrationAttribute = new Lazy<MigrationAttribute>((Func<MigrationAttribute>) (() =>
    {
      object[] customAttributes = this.GetType().GetCustomAttributes(typeof (MigrationAttribute), false);
      return customAttributes.Length != 0 ? (MigrationAttribute) customAttributes[0] : throw new InvalidOperationException(this.GetType().Name + " is missing the [Migration] attribute");
    }));
  }

  public int FromVersion => this._migrationAttribute.Value.FromVersion;

  public int ToVersion => this._migrationAttribute.Value.ToVersion;

  public Type SaveType => this._migrationAttribute.Value.SaveType;

  public MigratingData Migrate(MigratingData saveData)
  {
    this.ApplyMigration(saveData);
    saveData.Set<int>("schema_version", this.ToVersion);
    return saveData;
  }

  protected abstract void ApplyMigration(MigratingData saveData);
}
