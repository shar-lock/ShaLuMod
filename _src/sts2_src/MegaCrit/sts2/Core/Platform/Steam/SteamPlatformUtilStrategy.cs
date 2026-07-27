// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Platform.Steam.SteamPlatformUtilStrategy
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Multiplayer.Game;
using MegaCrit.Sts2.Core.Multiplayer.Transport.Steam;
using Steamworks;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Platform.Steam;

public class SteamPlatformUtilStrategy : IPlatformUtilStrategy
{
  private const string _richPresenceDisplayKey = "steam_display";
  private const string _richPresenceGroupKey = "steam_player_group";
  private const string _richPresenceGroupSizeKey = "steam_player_group_size";
  private PlatformBranch? _branch;
  private bool _isPlatformOverlayOpen;
  private Callback<GameOverlayActivated_t>? _steamOverlayCallback;

  public bool SupportsInviteDialog => SteamUtils.IsOverlayEnabled();

  public bool IsPlatformOverlayOpen => this._isPlatformOverlayOpen;

  public SteamPlatformUtilStrategy()
  {
    // ISSUE: method pointer
    this._steamOverlayCallback = Callback<GameOverlayActivated_t>.Create(new Callback<GameOverlayActivated_t>.DispatchDelegate((object) this, __methodptr(OnSteamOverlayToggled)));
  }

  ~SteamPlatformUtilStrategy() => this._steamOverlayCallback?.Dispose();

  public string GetPlayerName(ulong playerId)
  {
    string str = (long) playerId != (long) SteamUser.GetSteamID().m_SteamID ? SteamFriends.GetFriendPersonaName(new CSteamID(playerId)) : SteamFriends.GetPersonaName();
    return !string.IsNullOrEmpty(str) ? str : playerId.ToString();
  }

  public ulong GetLocalPlayerId() => SteamUser.GetSteamID().m_SteamID;

  public Task<IEnumerable<ulong>> GetFriendsWithOpenLobbies()
  {
    List<ulong> ulongList = new List<ulong>();
    int friendCount = SteamFriends.GetFriendCount((EFriendFlags) 4);
    for (int index = 0; index < friendCount; ++index)
    {
      CSteamID friendByIndex = SteamFriends.GetFriendByIndex(index, (EFriendFlags) 4);
      FriendGameInfo_t friendGameInfoT;
      if (SteamFriends.GetFriendGamePlayed(friendByIndex, ref friendGameInfoT) && friendGameInfoT.m_gameID.m_GameID == 2868840UL && ((CSteamID) ref friendGameInfoT.m_steamIDLobby).IsValid())
        ulongList.Add(friendByIndex.m_SteamID);
    }
    return Task.FromResult<IEnumerable<ulong>>((IEnumerable<ulong>) ulongList);
  }

  public void OpenInviteDialog(INetGameService netService)
  {
    if (!SteamUtils.IsOverlayEnabled())
    {
      Log.Error("Tried to open invite dialog, but the player has disabled the steam overlay");
    }
    else
    {
      CSteamID csteamId;
      switch (netService)
      {
        case INetHostGameService netHostGameService when netHostGameService.NetHost is SteamHost netHost:
          if (!netHost.LobbyId.HasValue)
          {
            Log.Warn("Tried to open invite dialog but steam host is not yet in a lobby");
            return;
          }
          csteamId = netHost.LobbyId.Value;
          break;
        case INetClientGameService clientGameService when clientGameService.NetClient is SteamClient netClient:
          if (!netClient.LobbyId.HasValue)
          {
            Log.Warn("Tried to open invite dialog but steam host is not yet in a lobby");
            return;
          }
          csteamId = netClient.LobbyId.Value;
          break;
        default:
          throw new InvalidOperationException($"Tried to open invite dialog for non-steam net service {netService}");
      }
      SteamFriends.ActivateGameOverlayInviteDialog(csteamId);
    }
  }

  public void OpenUrl(string url)
  {
    if (SteamUtils.IsOverlayEnabled())
      SteamFriends.ActivateGameOverlayToWebPage(url, (EActivateGameOverlayToWebPageMode) 0);
    else
      OS.ShellOpen(url);
  }

  public void OpenVirtualKeyboard()
  {
    SteamUtils.ShowFloatingGamepadTextInput((EFloatingGamepadTextInputMode) 0, 100, 200, 300, 50);
  }

  public void CloseVirtualKeyboard() => SteamUtils.DismissFloatingGamepadTextInput();

  public void SetRichPresence(string token, string? playerGroup, int? groupSize)
  {
    SteamFriends.SetRichPresence("steam_display", "#" + token);
    SteamFriends.SetRichPresence("steam_player_group", playerGroup);
    SteamFriends.SetRichPresence("steam_player_group_size", groupSize?.ToString());
  }

  public void SetRichPresenceValue(string key, string? value)
  {
    SteamFriends.SetRichPresence(key, value);
  }

  public void ClearRichPresence() => SteamFriends.ClearRichPresence();

  public string? GetThreeLetterLanguageCode()
  {
    string rawLanguage = this.GetRawLanguage();
    string letterLanguageCode;
    if (rawLanguage != null)
    {
      switch (rawLanguage.Length)
      {
        case 4:
          if (rawLanguage == "thai")
          {
            letterLanguageCode = "tha";
            goto label_58;
          }
          break;
        case 5:
          switch (rawLanguage[0])
          {
            case 'c':
              if (rawLanguage == "czech")
              {
                letterLanguageCode = "cze";
                goto label_58;
              }
              break;
            case 'd':
              if (rawLanguage == "dutch")
              {
                letterLanguageCode = "dut";
                goto label_58;
              }
              break;
            case 'g':
              if (rawLanguage == "greek")
              {
                letterLanguageCode = "gre";
                goto label_58;
              }
              break;
            case 'l':
              if (rawLanguage == "latam")
              {
                letterLanguageCode = "esp";
                goto label_58;
              }
              break;
          }
          break;
        case 6:
          switch (rawLanguage[0])
          {
            case 'a':
              if (rawLanguage == "arabic")
              {
                letterLanguageCode = "ara";
                goto label_58;
              }
              break;
            case 'f':
              if (rawLanguage == "french")
              {
                letterLanguageCode = "fra";
                goto label_58;
              }
              break;
            case 'g':
              if (rawLanguage == "german")
              {
                letterLanguageCode = "deu";
                goto label_58;
              }
              break;
            case 'k':
              if (rawLanguage == "korean")
              {
                letterLanguageCode = "kor";
                goto label_58;
              }
              break;
            case 'p':
              if (rawLanguage == "polish")
              {
                letterLanguageCode = "pol";
                goto label_58;
              }
              break;
          }
          break;
        case 7:
          switch (rawLanguage[0])
          {
            case 'e':
              if (rawLanguage == "english")
              {
                letterLanguageCode = "eng";
                goto label_58;
              }
              break;
            case 'i':
              if (rawLanguage == "italian")
              {
                letterLanguageCode = "ita";
                goto label_58;
              }
              break;
            case 'r':
              if (rawLanguage == "russian")
              {
                letterLanguageCode = "rus";
                goto label_58;
              }
              break;
            case 's':
              switch (rawLanguage)
              {
                case "spanish":
                  letterLanguageCode = "spa";
                  goto label_58;
                case "swedish":
                  letterLanguageCode = "swe";
                  goto label_58;
              }
              break;
            case 't':
              if (rawLanguage == "turkish")
              {
                letterLanguageCode = "tur";
                goto label_58;
              }
              break;
          }
          break;
        case 8:
          switch (rawLanguage[0])
          {
            case 'j':
              if (rawLanguage == "japanese")
              {
                letterLanguageCode = "jpn";
                goto label_58;
              }
              break;
            case 's':
              if (rawLanguage == "schinese")
              {
                letterLanguageCode = "zhs";
                goto label_58;
              }
              break;
            case 't':
              if (rawLanguage == "tchinese")
              {
                letterLanguageCode = "zht";
                goto label_58;
              }
              break;
          }
          break;
        case 9:
          switch (rawLanguage[0])
          {
            case 'b':
              if (rawLanguage == "brazilian")
              {
                letterLanguageCode = "ptb";
                goto label_58;
              }
              break;
            case 'n':
              if (rawLanguage == "norwegian")
              {
                letterLanguageCode = "nor";
                goto label_58;
              }
              break;
            case 'u':
              if (rawLanguage == "ukrainian")
              {
                letterLanguageCode = "ukr";
                goto label_58;
              }
              break;
          }
          break;
        case 10:
          switch (rawLanguage[0])
          {
            case 'i':
              if (rawLanguage == "indonesian")
              {
                letterLanguageCode = "ind";
                goto label_58;
              }
              break;
            case 'p':
              if (rawLanguage == "portuguese")
              {
                letterLanguageCode = "por";
                goto label_58;
              }
              break;
            case 'v':
              if (rawLanguage == "vietnamese")
              {
                letterLanguageCode = "vie";
                goto label_58;
              }
              break;
          }
          break;
      }
    }
    letterLanguageCode = (string) null;
label_58:
    return letterLanguageCode;
  }

  public PlatformBranch GetPlatformBranch()
  {
    if (this._branch.HasValue)
      return this._branch.Value;
    string name;
    if (!SteamApps.GetCurrentBetaName(ref name, 128 /*0x80*/))
    {
      this._branch = new PlatformBranch?(PlatformBranch.Production);
    }
    else
    {
      PlatformBranch? nullable = PlatformBranchExtensions.FromName(name);
      if (nullable.HasValue)
      {
        this._branch = nullable;
      }
      else
      {
        Log.Error($"User is on Steam beta branch {name} which could not be mapped to any {"PlatformBranch"}! Defaulting to None");
        this._branch = new PlatformBranch?(PlatformBranch.None);
      }
    }
    return this._branch.Value;
  }

  public string GetRawLanguage() => SteamUtils.GetSteamUILanguage();

  public SupportedWindowMode GetSupportedWindowMode()
  {
    return SteamUtils.IsSteamInBigPictureMode() ? SupportedWindowMode.FullscreenOnlyDisplayToggle : SupportedWindowMode.Any;
  }

  private void OnSteamOverlayToggled(GameOverlayActivated_t callback)
  {
    this._isPlatformOverlayOpen = callback.m_bActive == (byte) 1;
  }
}
