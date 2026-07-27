// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Platform.IPlatformUtilStrategy
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Multiplayer.Game;
using System.Collections.Generic;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Platform;

internal interface IPlatformUtilStrategy
{
  bool SupportsInviteDialog { get; }

  bool IsPlatformOverlayOpen { get; }

  string GetPlayerName(ulong playerId);

  ulong GetLocalPlayerId();

  Task<IEnumerable<ulong>> GetFriendsWithOpenLobbies();

  void OpenInviteDialog(INetGameService gameService);

  void OpenUrl(string url);

  void OpenVirtualKeyboard();

  void CloseVirtualKeyboard();

  PlatformBranch GetPlatformBranch();

  string? GetThreeLetterLanguageCode();

  string GetRawLanguage();

  void SetRichPresence(string token, string? playerGroup, int? groupSize);

  void SetRichPresenceValue(string key, string? value);

  void ClearRichPresence();

  SupportedWindowMode GetSupportedWindowMode();
}
