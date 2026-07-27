// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Platform.StatsManager
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Platform.Steam;
using MegaCrit.Sts2.Core.Saves;

#nullable disable
namespace MegaCrit.Sts2.Core.Platform;

public static class StatsManager
{
  public static void RefreshGlobalStats()
  {
    TaskHelper.RunSafely(SteamStatsManager.RefreshGlobalStats());
  }

  public static void IncrementArchitectDamage(int score)
  {
    SteamStatsManager.IncrementArchitectDamage(score);
  }

  public static long GetPersonalArchitectDamage() => SaveManager.Instance.Progress.ArchitectDamage;

  public static long? GetGlobalArchitectDamage()
  {
    if (SteamStatsManager.IsGlobalStatsReady)
    {
      long globalArchitectDamage = SteamStatsManager.GetGlobalArchitectDamage();
      if (globalArchitectDamage > 0L)
        return new long?(globalArchitectDamage);
    }
    return new long?();
  }
}
