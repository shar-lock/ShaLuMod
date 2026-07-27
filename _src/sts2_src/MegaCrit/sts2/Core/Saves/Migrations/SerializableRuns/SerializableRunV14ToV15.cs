// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Saves.Migrations.SerializableRuns.SerializableRunV14ToV15
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.Saves.Runs;
using System;
using System.Collections.Generic;

#nullable enable
namespace MegaCrit.Sts2.Core.Saves.Migrations.SerializableRuns;

[Migration(typeof (SerializableRun), 14, 15)]
public class SerializableRunV14ToV15 : MigrationBase<SerializableRun>
{
  protected override void ApplyMigration(MigratingData saveData)
  {
    Log.Info("SerializableRun migration v14 -> v15: Adding GameMode to save files, initialized as Standard");
    DateTimeOffset? asOrNull = saveData.GetAsOrNull<DateTimeOffset>("dailyTime");
    List<SerializableModifier> serializableModifierList = saveData.GetAs<List<SerializableModifier>>("modifiers");
    if (asOrNull.HasValue)
      saveData.Set<GameMode>("game_mode", GameMode.Daily);
    else if (serializableModifierList != null && serializableModifierList.Count > 0)
      saveData.Set<GameMode>("game_mode", GameMode.Custom);
    else
      saveData.Set<GameMode>("game_mode", GameMode.Standard);
  }
}
