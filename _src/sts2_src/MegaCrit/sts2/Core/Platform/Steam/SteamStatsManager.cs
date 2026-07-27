// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Platform.Steam.SteamStatsManager
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Logging;
using Steamworks;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Platform.Steam;

public static class SteamStatsManager
{
  private const string _architectDamageStat = "architect_damage";
  private static Callback<UserStatsReceived_t>? _userStatsReceivedCallback;
  private static bool _userStatsReady;
  private static bool _globalStatsReady;
  private static long _globalArchitectDamage;

  public static bool IsGlobalStatsReady => SteamStatsManager._globalStatsReady;

  public static void Initialize()
  {
    if (!SteamInitializer.Initialized)
      return;
    SteamStatsManager._userStatsReady = false;
    SteamStatsManager._globalStatsReady = false;
    SteamStatsManager._globalArchitectDamage = 0L;
    // ISSUE: reference to a compiler-generated field
    // ISSUE: reference to a compiler-generated field
    // ISSUE: method pointer
    SteamStatsManager._userStatsReceivedCallback = Callback<UserStatsReceived_t>.Create(SteamStatsManager.\u003C\u003EO.\u003C0\u003E__OnUserStatsReceived ?? (SteamStatsManager.\u003C\u003EO.\u003C0\u003E__OnUserStatsReceived = new Callback<UserStatsReceived_t>.DispatchDelegate((object) null, __methodptr(OnUserStatsReceived))));
    SteamUserStats.RequestCurrentStats();
    TaskHelper.RunSafely(SteamStatsManager.RefreshGlobalStats());
  }

  public static async Task RefreshGlobalStats()
  {
    // ISSUE: unable to decompile the method.
  }

  public static void IncrementArchitectDamage(int score)
  {
    if (!SteamStatsManager._userStatsReady)
    {
      Log.Warn("SteamStatsManager: Cannot increment architect damage, user stats not ready");
    }
    else
    {
      int num;
      bool stat = SteamUserStats.GetStat("architect_damage", ref num);
      bool flag1 = SteamUserStats.SetStat("architect_damage", num + score);
      bool flag2 = SteamUserStats.StoreStats();
      Log.Info($"SteamStatsManager: IncrementArchitectDamage by {score} (was {num}, now {num + score}) [get={stat}, set={flag1}, store={flag2}]");
    }
  }

  public static long GetGlobalArchitectDamage() => SteamStatsManager._globalArchitectDamage;

  private static void OnUserStatsReceived(UserStatsReceived_t result)
  {
    if (result.m_nGameID != 2868840UL)
      return;
    if (result.m_eResult == 1)
    {
      SteamStatsManager._userStatsReady = true;
      Log.Info("SteamStatsManager: User stats received");
    }
    else
      Log.Warn($"SteamStatsManager: User stats request failed with result {result.m_eResult}");
  }

  private static void OnGlobalStatsReceived(GlobalStatsReceived_t result)
  {
    if (result.m_nGameID != 2868840UL)
      return;
    if (result.m_eResult == 1)
    {
      long num;
      bool globalStat = SteamUserStats.GetGlobalStat("architect_damage", ref num);
      SteamStatsManager._globalArchitectDamage = num;
      SteamStatsManager._globalStatsReady = true;
      Log.Info($"SteamStatsManager: Global stats received (found={globalStat}), architect damage = {SteamStatsManager._globalArchitectDamage}");
    }
    else
      Log.Warn($"SteamStatsManager: Global stats request failed with result {result.m_eResult}");
  }
}
