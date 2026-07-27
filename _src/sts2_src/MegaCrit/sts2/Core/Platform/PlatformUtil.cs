// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Platform.PlatformUtil
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Multiplayer.Game;
using MegaCrit.Sts2.Core.Platform.Null;
using MegaCrit.Sts2.Core.Platform.Steam;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Platform;

public static class PlatformUtil
{
  private static readonly NullPlatformUtilStrategy _null = new NullPlatformUtilStrategy();
  private static readonly SteamPlatformUtilStrategy _steam = new SteamPlatformUtilStrategy();

  public static PlatformType PrimaryPlatform
  {
    get => SteamInitializer.Initialized ? PlatformType.Steam : PlatformType.None;
  }

  private static IPlatformUtilStrategy GetPlatformUtil(PlatformType platformType)
  {
    if (platformType == PlatformType.None)
      return (IPlatformUtilStrategy) PlatformUtil._null;
    if (platformType == PlatformType.Steam)
      return (IPlatformUtilStrategy) PlatformUtil._steam;
    throw new ArgumentOutOfRangeException(nameof (platformType), (object) platformType, (string) null);
  }

  public static string GetPlayerName(PlatformType platformType, ulong playerId)
  {
    return PlatformUtil.GetPlayerNameRaw(platformType, playerId).EscapeBbcodeTags();
  }

  public static string GetPlayerNameRaw(PlatformType platformType, ulong playerId)
  {
    return PlatformUtil.GetPlatformUtil(platformType).GetPlayerName(playerId);
  }

  public static ulong GetLocalPlayerId(PlatformType platformType)
  {
    return PlatformUtil.GetPlatformUtil(platformType).GetLocalPlayerId();
  }

  public static Task<IEnumerable<ulong>> GetFriendsWithOpenLobbies(PlatformType platformType)
  {
    return PlatformUtil.GetPlatformUtil(platformType).GetFriendsWithOpenLobbies();
  }

  public static bool SupportsInviteDialog(PlatformType platformType)
  {
    return PlatformUtil.GetPlatformUtil(platformType).SupportsInviteDialog;
  }

  public static void OpenInviteDialog(INetGameService netService)
  {
    PlatformUtil.GetPlatformUtil(netService.Platform).OpenInviteDialog(netService);
  }

  public static void OpenUrl(string url)
  {
    PlatformUtil.GetPlatformUtil(PlatformUtil.PrimaryPlatform).OpenUrl(url);
  }

  public static void OpenVirtualKeyboard()
  {
    PlatformUtil.GetPlatformUtil(PlatformUtil.PrimaryPlatform).OpenVirtualKeyboard();
  }

  public static void CloseVirtualKeyboard()
  {
    PlatformUtil.GetPlatformUtil(PlatformUtil.PrimaryPlatform).CloseVirtualKeyboard();
  }

  public static PlatformBranch GetPlatformBranch()
  {
    return PlatformUtil.GetPlatformUtil(PlatformUtil.PrimaryPlatform).GetPlatformBranch();
  }

  public static string? GetThreeLetterLanguageCode()
  {
    return PlatformUtil.GetPlatformUtil(PlatformUtil.PrimaryPlatform).GetThreeLetterLanguageCode();
  }

  public static string GetRawLanguage()
  {
    return PlatformUtil.GetPlatformUtil(PlatformUtil.PrimaryPlatform).GetRawLanguage();
  }

  public static void SetRichPresence(string token, string? playerGroup, int? groupSize)
  {
    PlatformUtil.GetPlatformUtil(PlatformUtil.PrimaryPlatform).SetRichPresence(token, playerGroup, groupSize);
  }

  public static void SetRichPresenceValue(string key, string? value)
  {
    PlatformUtil.GetPlatformUtil(PlatformUtil.PrimaryPlatform).SetRichPresenceValue(key, value);
  }

  public static void ClearRichPresence()
  {
    PlatformUtil.GetPlatformUtil(PlatformUtil.PrimaryPlatform).ClearRichPresence();
  }

  public static SupportedWindowMode GetSupportedWindowMode()
  {
    return PlatformUtil.GetPlatformUtil(PlatformUtil.PrimaryPlatform).GetSupportedWindowMode();
  }

  public static bool IsPlatformOverlayOpen()
  {
    return PlatformUtil.GetPlatformUtil(PlatformUtil.PrimaryPlatform).IsPlatformOverlayOpen;
  }
}
