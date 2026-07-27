// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Entities.Multiplayer.NetErrorInfo
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Platform;
using MegaCrit.Sts2.Core.Platform.Steam;
using Steamworks;
using System;
using System.Collections.Generic;
using System.Text;

#nullable enable
namespace MegaCrit.Sts2.Core.Entities.Multiplayer;

public readonly struct NetErrorInfo
{
  private readonly NetError? _reason;
  private readonly ConnectionFailureReason? _connectionReason;
  private readonly SteamDisconnectionReason? _steamReason;
  private readonly EResult? _lobbyCreationResult;
  private readonly EChatRoomEnterResponse? _lobbyEnterResponse;
  private readonly string? _debugReason;
  private readonly Error? _godotError;

  public bool SelfInitiated { get; }

  public ConnectionFailureExtraInfo? ConnectionExtraInfo { get; }

  public NetErrorInfo(NetError reason, bool selfInitiated)
  {
    this._connectionReason = new ConnectionFailureReason?();
    this._steamReason = new SteamDisconnectionReason?();
    this._lobbyCreationResult = new EResult?();
    this._lobbyEnterResponse = new EChatRoomEnterResponse?();
    this._debugReason = (string) null;
    this._godotError = new Error?();
    this.ConnectionExtraInfo = (ConnectionFailureExtraInfo) null;
    this._reason = new NetError?(reason);
    this.SelfInitiated = selfInitiated;
  }

  public NetErrorInfo(ConnectionFailureReason reason, ConnectionFailureExtraInfo? extraInfo = null)
  {
    this._reason = new NetError?();
    this._steamReason = new SteamDisconnectionReason?();
    this._lobbyCreationResult = new EResult?();
    this._lobbyEnterResponse = new EChatRoomEnterResponse?();
    this._debugReason = (string) null;
    this._godotError = new Error?();
    this._connectionReason = new ConnectionFailureReason?(reason);
    this.ConnectionExtraInfo = extraInfo;
    this.SelfInitiated = false;
  }

  public NetErrorInfo(SteamDisconnectionReason steamReason, string? debugReason, bool selfInitiated)
  {
    this._reason = new NetError?();
    this._connectionReason = new ConnectionFailureReason?();
    this._lobbyCreationResult = new EResult?();
    this._lobbyEnterResponse = new EChatRoomEnterResponse?();
    this._godotError = new Error?();
    this.ConnectionExtraInfo = (ConnectionFailureExtraInfo) null;
    this._steamReason = new SteamDisconnectionReason?(steamReason);
    this._debugReason = debugReason;
    this.SelfInitiated = selfInitiated;
  }

  public NetErrorInfo(EChatRoomEnterResponse lobbyEnterResponse)
  {
    this._reason = new NetError?();
    this._connectionReason = new ConnectionFailureReason?();
    this._steamReason = new SteamDisconnectionReason?();
    this._lobbyCreationResult = new EResult?();
    this._debugReason = (string) null;
    this._godotError = new Error?();
    this.ConnectionExtraInfo = (ConnectionFailureExtraInfo) null;
    this._lobbyEnterResponse = new EChatRoomEnterResponse?(lobbyEnterResponse);
    this.SelfInitiated = true;
  }

  public NetErrorInfo(EResult lobbyCreationResult)
  {
    this._reason = new NetError?();
    this._connectionReason = new ConnectionFailureReason?();
    this._steamReason = new SteamDisconnectionReason?();
    this._lobbyEnterResponse = new EChatRoomEnterResponse?();
    this._debugReason = (string) null;
    this._godotError = new Error?();
    this.ConnectionExtraInfo = (ConnectionFailureExtraInfo) null;
    this._lobbyCreationResult = new EResult?(lobbyCreationResult);
    this.SelfInitiated = true;
  }

  public NetErrorInfo(Error error)
  {
    this._reason = new NetError?();
    this._connectionReason = new ConnectionFailureReason?();
    this._steamReason = new SteamDisconnectionReason?();
    this._lobbyCreationResult = new EResult?();
    this._lobbyEnterResponse = new EChatRoomEnterResponse?();
    this._debugReason = (string) null;
    this.ConnectionExtraInfo = (ConnectionFailureExtraInfo) null;
    this._godotError = new Error?(error);
    this.SelfInitiated = true;
  }

  public NetError GetReason()
  {
    if (this._reason.HasValue)
      return this._reason.Value;
    if (this._connectionReason.HasValue)
    {
      ConnectionFailureReason unmatchedValue = this._connectionReason.Value;
      NetError reason;
      switch (unmatchedValue)
      {
        case ConnectionFailureReason.None:
          reason = NetError.None;
          break;
        case ConnectionFailureReason.LobbyFull:
          reason = NetError.LobbyFull;
          break;
        case ConnectionFailureReason.NotInSaveGame:
          reason = NetError.NotInSaveGame;
          break;
        case ConnectionFailureReason.RunInProgress:
          reason = NetError.RunInProgress;
          break;
        case ConnectionFailureReason.VersionMismatch:
          reason = NetError.VersionMismatch;
          break;
        case ConnectionFailureReason.ModMismatch:
          reason = NetError.ModMismatch;
          break;
        default:
          // ISSUE: reference to a compiler-generated method
          \u003CPrivateImplementationDetails\u003E.ThrowSwitchExpressionException((object) unmatchedValue);
          break;
      }
      return reason;
    }
    if (this._steamReason.HasValue)
      return this._steamReason.Value.ToApp();
    if (this._lobbyCreationResult.HasValue)
      return NetError.FailedToHost;
    if (this._lobbyEnterResponse.HasValue)
    {
      EChatRoomEnterResponse unmatchedValue = this._lobbyEnterResponse.Value;
      NetError reason;
      switch (unmatchedValue - 1)
      {
        case 0:
          reason = NetError.None;
          break;
        case 1:
          reason = NetError.InvalidJoin;
          break;
        case 2:
          reason = NetError.InternalError;
          break;
        case 3:
          reason = NetError.LobbyFull;
          break;
        case 4:
          reason = NetError.UnknownNetworkError;
          break;
        case 5:
          reason = NetError.JoinBlockedByUser;
          break;
        case 6:
          reason = NetError.UnknownNetworkError;
          break;
        case 7:
          reason = NetError.JoinBlockedByUser;
          break;
        case 8:
          reason = NetError.JoinBlockedByUser;
          break;
        case 9:
          reason = NetError.JoinBlockedByUser;
          break;
        case 10:
          reason = NetError.JoinBlockedByUser;
          break;
        case 14:
          reason = NetError.RateLimited;
          break;
        default:
          // ISSUE: reference to a compiler-generated method
          \u003CPrivateImplementationDetails\u003E.ThrowSwitchExpressionException((object) unmatchedValue);
          break;
      }
      return reason;
    }
    if (this._godotError.HasValue)
      return NetError.FailedToHost;
    throw new InvalidOperationException("Tried to get DisconnectionReason from DisconnectionInfo without any assigned errors");
  }

  public string GetErrorString()
  {
    if (this._reason.HasValue)
      return this._reason.Value.ToString();
    if (this._connectionReason.HasValue)
    {
      if (this._connectionReason.GetValueOrDefault() == ConnectionFailureReason.ModMismatch)
      {
        StringBuilder stringBuilder = new StringBuilder();
        List<string> missingModsOnHost = this.ConnectionExtraInfo?.missingModsOnHost;
        if (missingModsOnHost != null && missingModsOnHost.Count > 0)
        {
          LocString locString = new LocString("main_menu_ui", "NETWORK_ERROR.MOD_MISMATCH.description.missingOnHost");
          locString.Add("mods", string.Join(", ", (IEnumerable<string>) this.ConnectionExtraInfo.missingModsOnHost));
          stringBuilder.AppendLine(locString.GetFormattedText());
        }
        List<string> missingModsOnLocal = this.ConnectionExtraInfo?.missingModsOnLocal;
        if (missingModsOnLocal != null && missingModsOnLocal.Count > 0)
        {
          LocString locString = new LocString("main_menu_ui", "NETWORK_ERROR.MOD_MISMATCH.description.missingOnLocal");
          locString.Add("mods", string.Join(", ", (IEnumerable<string>) this.ConnectionExtraInfo.missingModsOnLocal));
          stringBuilder.AppendLine(locString.GetFormattedText());
        }
        return stringBuilder.ToString();
      }
      if (this._connectionReason.GetValueOrDefault() == ConnectionFailureReason.VersionMismatch)
      {
        if (this.ConnectionExtraInfo?.hostVersion != this.ConnectionExtraInfo?.localVersion)
        {
          PlatformBranch? hostBranch = (PlatformBranch?) this.ConnectionExtraInfo?.hostBranch;
          PlatformBranch? localBranch = (PlatformBranch?) this.ConnectionExtraInfo?.localBranch;
          if (!(hostBranch.GetValueOrDefault() == localBranch.GetValueOrDefault() & hostBranch.HasValue == localBranch.HasValue))
          {
            LocString locString1 = new LocString("main_menu_ui", "NETWORK_ERROR.VERSION_MISMATCH.description.branchMismatch");
            LocString locString2 = locString1;
            ConnectionFailureExtraInfo connectionExtraInfo1 = this.ConnectionExtraInfo;
            string variable1;
            if ((object) connectionExtraInfo1 == null)
            {
              variable1 = (string) null;
            }
            else
            {
              ref PlatformBranch? local = ref connectionExtraInfo1.hostBranch;
              variable1 = local.HasValue ? local.GetValueOrDefault().ToName() : (string) null;
            }
            if (variable1 == null)
              variable1 = "<null>";
            locString2.Add("hostBranch", variable1);
            LocString locString3 = locString1;
            ConnectionFailureExtraInfo connectionExtraInfo2 = this.ConnectionExtraInfo;
            string variable2;
            if ((object) connectionExtraInfo2 == null)
            {
              variable2 = (string) null;
            }
            else
            {
              ref PlatformBranch? local = ref connectionExtraInfo2.localBranch;
              variable2 = local.HasValue ? local.GetValueOrDefault().ToName() : (string) null;
            }
            if (variable2 == null)
              variable2 = "<null>";
            locString3.Add("localBranch", variable2);
            return locString1.GetFormattedText();
          }
          LocString locString = new LocString("main_menu_ui", "NETWORK_ERROR.VERSION_MISMATCH.description.versionMismatch");
          locString.Add("hostVersion", this.ConnectionExtraInfo?.hostVersion ?? "<null>");
          locString.Add("localVersion", this.ConnectionExtraInfo?.localVersion ?? "<null>");
          return locString.GetFormattedText();
        }
        ulong? hostHash = (ulong?) this.ConnectionExtraInfo?.hostHash;
        ulong? localHash = (ulong?) this.ConnectionExtraInfo?.localHash;
        if (!((long) hostHash.GetValueOrDefault() == (long) localHash.GetValueOrDefault() & hostHash.HasValue == localHash.HasValue))
        {
          LocString locString4 = new LocString("main_menu_ui", "NETWORK_ERROR.VERSION_MISMATCH.description.modelDbMismatch");
          LocString locString5 = locString4;
          ConnectionFailureExtraInfo connectionExtraInfo3 = this.ConnectionExtraInfo;
          string variable3;
          if ((object) connectionExtraInfo3 == null)
          {
            variable3 = (string) null;
          }
          else
          {
            ref ulong? local = ref connectionExtraInfo3.hostHash;
            variable3 = local.HasValue ? local.GetValueOrDefault().ToString() : (string) null;
          }
          if (variable3 == null)
            variable3 = "<null>";
          locString5.Add("hostHash", variable3);
          LocString locString6 = locString4;
          ConnectionFailureExtraInfo connectionExtraInfo4 = this.ConnectionExtraInfo;
          string variable4;
          if ((object) connectionExtraInfo4 == null)
          {
            variable4 = (string) null;
          }
          else
          {
            ref ulong? local = ref connectionExtraInfo4.localHash;
            variable4 = local.HasValue ? local.GetValueOrDefault().ToString() : (string) null;
          }
          if (variable4 == null)
            variable4 = "<null>";
          locString6.Add("localHash", variable4);
          return locString4.GetFormattedText();
        }
      }
      return this._connectionReason.Value.ToString();
    }
    if (this._steamReason.HasValue)
      return $"{this._steamReason} - {this._debugReason}";
    if (this._lobbyCreationResult.HasValue)
      return $"Lobby creation failed: {this._lobbyCreationResult.Value}";
    if (this._lobbyEnterResponse.HasValue)
      return $"Lobby join failed: {this._lobbyEnterResponse.Value}";
    return this._godotError.HasValue ? this._godotError.Value.ToString() : "<null>";
  }

  public override string ToString()
  {
    if (this._reason.HasValue)
      return $"DisconnectionReason {this._reason.Value} {this.SelfInitiated}";
    if (this._connectionReason.HasValue)
      return $"ConnectionFailureReason {this._connectionReason.Value} {this.SelfInitiated}";
    if (this._steamReason.HasValue)
      return $"SteamDisconnectionReason {this._steamReason.Value} {this._debugReason} {this.SelfInitiated}";
    if (this._lobbyCreationResult.HasValue)
      return $"EResult {this._lobbyCreationResult.Value} {this.SelfInitiated}";
    if (this._lobbyEnterResponse.HasValue)
      return $"EChatRoomEnterResponse {this._lobbyEnterResponse.Value} {this.SelfInitiated}";
    if (!this._godotError.HasValue)
      return "<null>";
    return $"Godot.Error {this._godotError.Value} {this.SelfInitiated}";
  }
}
