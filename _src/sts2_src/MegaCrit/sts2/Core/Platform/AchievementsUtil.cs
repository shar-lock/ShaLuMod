// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Platform.AchievementsUtil
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Achievements;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Platform.Null;
using MegaCrit.Sts2.Core.Saves;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable enable
namespace MegaCrit.Sts2.Core.Platform;

public static class AchievementsUtil
{
  private static readonly IAchievementStrategy _platform = (IAchievementStrategy) new NullAchievementStrategy();

  public static event Action? AchievementsChanged;

  public static void Unlock(Achievement achievement, Player? localPlayer)
  {
  }

  public static void Revoke(Achievement achievement)
  {
    if (!AchievementsUtil.IsUnlocked(achievement))
      return;
    Log.Debug($"Revoking achievement: {achievement}");
    AchievementsUtil._platform.Revoke(achievement);
    if (SaveManager.Instance.Progress.RemoveUnlockedAchievement(achievement))
      SaveManager.Instance.SaveProgressFile();
    Action achievementsChanged = AchievementsUtil.AchievementsChanged;
    if (achievementsChanged == null)
      return;
    achievementsChanged();
  }

  public static bool IsUnlocked(Achievement achievement)
  {
    return SaveManager.Instance.Progress.IsAchievementUnlocked(achievement);
  }

  public static int TotalAchievementCount() => Enum.GetValues<Achievement>().Length;

  public static int UnlockedAchievementCount()
  {
    // ISSUE: reference to a compiler-generated field
    // ISSUE: reference to a compiler-generated field
    return ((IEnumerable<Achievement>) Enum.GetValues<Achievement>()).Count<Achievement>(AchievementsUtil.\u003C\u003EO.\u003C0\u003E__IsUnlocked ?? (AchievementsUtil.\u003C\u003EO.\u003C0\u003E__IsUnlocked = new Func<Achievement, bool>(AchievementsUtil.IsUnlocked)));
  }
}
