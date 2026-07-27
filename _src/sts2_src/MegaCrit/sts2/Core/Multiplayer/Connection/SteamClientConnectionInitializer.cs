// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Multiplayer.Connection.SteamClientConnectionInitializer
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Entities.Multiplayer;
using MegaCrit.Sts2.Core.Multiplayer.Game;
using MegaCrit.Sts2.Core.Multiplayer.Transport;
using MegaCrit.Sts2.Core.Multiplayer.Transport.Steam;
using MegaCrit.Sts2.Core.Platform;
using System;
using System.Threading;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Multiplayer.Connection;

public class SteamClientConnectionInitializer : IClientConnectionInitializer
{
  private ulong? _playerSteamId;
  private ulong? _lobbySteamId;

  public static SteamClientConnectionInitializer FromPlayer(ulong playerSteamId)
  {
    return new SteamClientConnectionInitializer()
    {
      _playerSteamId = new ulong?(playerSteamId)
    };
  }

  public static SteamClientConnectionInitializer FromLobby(ulong lobbySteamId)
  {
    return new SteamClientConnectionInitializer()
    {
      _lobbySteamId = new ulong?(lobbySteamId)
    };
  }

  public async Task<NetErrorInfo?> Connect(
    INetClientGameService gameService,
    CancellationToken cancelToken = default (CancellationToken))
  {
    SteamClient client = !gameService.IsConnected ? new SteamClient((INetClientHandler) gameService) : throw new InvalidOperationException("NetClientGameService must not be connected when passed to SteamClientConnectionInitializer!");
    gameService.Initialize((NetClient) client, PlatformType.Steam);
    if (this._playerSteamId.HasValue)
      return await client.ConnectToLobbyOwnedByFriend(this._playerSteamId.Value, cancelToken);
    if (this._lobbySteamId.HasValue)
      return await client.ConnectToLobby(this._lobbySteamId.Value, cancelToken);
    throw new InvalidOperationException("Neither player nor lobby is set!");
  }

  public override string ToString()
  {
    return $"{nameof (SteamClientConnectionInitializer)} player: {this._playerSteamId} lobby: {this._lobbySteamId}";
  }
}
