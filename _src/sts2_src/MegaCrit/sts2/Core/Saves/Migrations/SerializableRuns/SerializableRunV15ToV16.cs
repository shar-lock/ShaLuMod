// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Saves.Migrations.SerializableRuns.SerializableRunV15ToV16
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Saves.Migrations.Shared;
using System.Text.Json.Nodes;

#nullable enable
namespace MegaCrit.Sts2.Core.Saves.Migrations.SerializableRuns;

[Migration(typeof (SerializableRun), 15, 16 /*0x10*/)]
public class SerializableRunV15ToV16 : MigrationBase<SerializableRun>
{
  protected override void ApplyMigration(MigratingData saveData)
  {
    Log.Info("SerializableRun migration v15 -> v16: Migrating renamed/deleted ModelIds");
    SharedMigrationHelper.ReplaceModelIds((JsonNode) saveData.GetRawNode(), SharedMigrationHelper.v100Renames);
  }
}
