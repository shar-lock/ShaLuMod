// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Multiplayer.Transport.NetHost
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Entities.Multiplayer;
using System.Collections.Generic;

#nullable enable
namespace MegaCrit.Sts2.Core.Multiplayer.Transport;

public abstract class NetHost
{
  protected INetHostHandler _handler;

  public abstract IEnumerable<ulong> ConnectedPeerIds { get; }

  public abstract bool IsConnected { get; }

  public abstract ulong NetId { get; }

  protected NetHost(INetHostHandler handler) => this._handler = handler;

  public abstract void Update();

  public abstract void SetHostIsClosed(bool isClosed);

  public abstract void SendMessageToClient(
    ulong peerId,
    byte[] bytes,
    int length,
    NetTransferMode mode,
    int channel = 0);

  public abstract void SendMessageToAll(
    byte[] bytes,
    int length,
    NetTransferMode mode,
    int channel = 0);

  public abstract void DisconnectClient(ulong peerId, NetError reason, bool now = false);

  public abstract void StopHost(NetError reason, bool now = false);

  public abstract string? GetRawLobbyIdentifier();
}
