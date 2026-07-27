// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Platform.Null.NullPlatformUtilStrategy
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Multiplayer.Game;
using MegaCrit.Sts2.Core.Saves;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.Json;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Platform.Null;

public class NullPlatformUtilStrategy : IPlatformUtilStrategy
{
  private const string _multiplayerNamesFile = "mp_names.json";
  private List<NullMultiplayerName>? _mpNames;

  public ulong LocalPlayerId { get; } = 1;

  public NullPlatformUtilStrategy()
  {
    string s;
    ulong result;
    if (CommandLineHelper.TryGetValue("clientId", out s) && ulong.TryParse(s, out result))
      this.LocalPlayerId = result;
    GodotFileIo godotFileIo = new GodotFileIo(".");
    if (!godotFileIo.FileExists("mp_names.json"))
      return;
    this._mpNames = JsonSerializer.Deserialize<List<NullMultiplayerName>>(godotFileIo.ReadFile("mp_names.json"), JsonSerializationUtility.GetTypeInfo<List<NullMultiplayerName>>());
  }

  public bool SupportsInviteDialog => false;

  public bool IsPlatformOverlayOpen => false;

  public string GetPlayerName(ulong playerId)
  {
    if (this._mpNames != null)
    {
      foreach (NullMultiplayerName mpName in this._mpNames)
      {
        if ((long) mpName.netId == (long) playerId)
          return mpName.name;
      }
    }
    string playerName;
    switch (playerId)
    {
      case 1:
        playerName = "Test Host";
        break;
      case 1000:
        playerName = "Test Client 1";
        break;
      case 2000:
        playerName = "Test Client 2";
        break;
      case 3000:
        playerName = "Test Client 3";
        break;
      default:
        playerName = playerId.ToString();
        break;
    }
    return playerName;
  }

  public ulong GetLocalPlayerId() => this.LocalPlayerId;

  public Task<IEnumerable<ulong>> GetFriendsWithOpenLobbies()
  {
    return Task.FromResult<IEnumerable<ulong>>((IEnumerable<ulong>) Array.Empty<ulong>());
  }

  public void OpenInviteDialog(INetGameService netService) => throw new NotImplementedException();

  public void OpenUrl(string url) => OS.ShellOpen(url);

  public void OpenVirtualKeyboard()
  {
  }

  public void CloseVirtualKeyboard()
  {
  }

  public void SetRichPresence(string token, string? playerGroup, int? groupSize)
  {
  }

  public void SetRichPresenceValue(string key, string? value)
  {
  }

  public void ClearRichPresence()
  {
  }

  public PlatformBranch GetPlatformBranch() => PlatformBranch.None;

  public string? GetThreeLetterLanguageCode()
  {
    CultureInfo cultureInfo = new CultureInfo(this.GetRawLanguage());
    if (LocManager.Languages.Contains(cultureInfo.ThreeLetterISOLanguageName))
      return cultureInfo.ThreeLetterISOLanguageName;
    string str = cultureInfo.Name.ToLowerInvariant().Replace('-', '_');
    if (str.StartsWith("zh"))
      return str.StartsWith("zh_hans") || str.StartsWith("zh_cn") || !str.StartsWith("zh_hant") && !str.StartsWith("zh_tw") ? "zhs" : "zht";
    if (str.StartsWith("pt"))
      return str.StartsWith("pt_br") ? "ptb" : "por";
    Log.Error($"CultureInfo {cultureInfo} could not be mapped to a three-letter language code!");
    return (string) null;
  }

  public string GetRawLanguage() => OS.GetLocale().Replace('_', '-');

  public SupportedWindowMode GetSupportedWindowMode() => SupportedWindowMode.Any;
}
