// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Multiplayer.NetSingleplayerGameService
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Entities.Multiplayer;
using MegaCrit.Sts2.Core.Multiplayer.Game;
using MegaCrit.Sts2.Core.Multiplayer.Quality;
using MegaCrit.Sts2.Core.Multiplayer.Serialization;
using MegaCrit.Sts2.Core.Platform;
using MegaCrit.Sts2.Core.Platform.Steam;
using System;

#nullable enable
namespace MegaCrit.Sts2.Core.Multiplayer;

public class NetSingleplayerGameService : INetGameService
{
  public const int defaultNetId = 1;
  private bool _isLoading;

  public bool IsConnected => true;

  public bool IsGameLoading => this._isLoading;

  public ulong NetId => 1;

  public NetGameType Type => NetGameType.Singleplayer;

  public PlatformType Platform
  {
    get => !SteamInitializer.Initialized ? PlatformType.None : PlatformType.Steam;
  }

  public event Action<NetErrorInfo>? Disconnected;

  public void SendMessage<T>(T message, ulong playerId) where T : INetMessage
  {
  }

  public void SendMessage<T>(T message) where T : INetMessage
  {
  }

  public void RegisterMessageHandler<T>(MessageHandlerDelegate<T> handler) where T : INetMessage
  {
  }

  public void UnregisterMessageHandler<T>(MessageHandlerDelegate<T> handler) where T : INetMessage
  {
  }

  public void Update()
  {
  }

  public void Disconnect(NetError reason, bool now = false)
  {
  }

  public ConnectionStats GetStatsForPeer(ulong peerId) => throw new NotImplementedException();

  public void SetGameLoading(bool isLoading) => this._isLoading = isLoading;

  public void SetBufferMessages(bool bufferMessages)
  {
  }

  public string? GetRawLobbyIdentifier() => (string) null;
}
