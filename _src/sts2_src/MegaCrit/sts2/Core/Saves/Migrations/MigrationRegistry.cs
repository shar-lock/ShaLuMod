// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Saves.Migrations.MigrationRegistry
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Logging;
using System;
using System.Collections.Generic;
using System.Reflection;

#nullable enable
namespace MegaCrit.Sts2.Core.Saves.Migrations;

public class MigrationRegistry
{
  public Dictionary<Type, List<IMigration>> Migrations { get; } = new Dictionary<Type, List<IMigration>>();

  public void RegisterAllMigrations(MigrationManager manager)
  {
    try
    {
      Assembly assembly = typeof (IMigration).Assembly;
      Type type1 = typeof (IMigration);
      for (int i = 0; i < IMigrationSubtypes.Count; ++i)
      {
        Type type2 = IMigrationSubtypes.Get(i);
        try
        {
          if (Activator.CreateInstance(type2) is IMigration instance)
          {
            manager.RegisterMigration(instance);
            Log.Debug($"Registered migration for {instance.SaveType.Name} from v{instance.FromVersion} to v{instance.ToVersion}");
          }
          else
            Log.Error($"Failed to instantiate migration {type2.Name}: Created instance is not an IMigration");
        }
        catch (Exception ex)
        {
          Log.Error($"Failed to instantiate migration {type2.Name}: {ex.Message}");
        }
      }
      Log.Info($"Registered {IMigrationSubtypes.Count} migrations");
    }
    catch (Exception ex)
    {
      Log.Error("Error registering migrations: " + ex.Message);
    }
  }
}
