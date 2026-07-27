// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Multiplayer.Game.INetGameService
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Entities.Multiplayer;
using MegaCrit.Sts2.Core.Multiplayer.Quality;
using MegaCrit.Sts2.Core.Multiplayer.Serialization;
using MegaCrit.Sts2.Core.Platform;
using System;

#nullable enable
namespace MegaCrit.Sts2.Core.Multiplayer.Game;

public interface INetGameService
{
  ulong NetId { get; }

  bool IsConnected { get; }

  bool IsGameLoading { get; }

  event Action<NetErrorInfo>? Disconnected;

  NetGameType Type { get; }

  PlatformType Platform { get; }

  void SendMessage<T>(T message, ulong playerId) where T : INetMessage;

  void SendMessage<T>(T message) where T : INetMessage;

  void RegisterMessageHandler<T>(MessageHandlerDelegate<T> messageHandlerDelegate) where T : INetMessage;

  void UnregisterMessageHandler<T>(MessageHandlerDelegate<T> messageHandlerDelegate) where T : INetMessage;

  void Update();

  void Disconnect(NetError reason, bool now = false);

  ConnectionStats? GetStatsForPeer(ulong peerId);

  void SetGameLoading(bool isLoading);

  void SetBufferMessages(bool bufferMessages);

  string? GetRawLobbyIdentifier();
}
