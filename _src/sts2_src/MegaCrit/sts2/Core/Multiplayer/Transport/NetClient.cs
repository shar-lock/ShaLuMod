// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Multiplayer.Transport.NetClient
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Entities.Multiplayer;

#nullable enable
namespace MegaCrit.Sts2.Core.Multiplayer.Transport;

public abstract class NetClient
{
  protected INetClientHandler _handler;

  public abstract bool IsConnected { get; }

  public abstract ulong NetId { get; }

  public abstract ulong HostNetId { get; }

  protected NetClient(INetClientHandler handler) => this._handler = handler;

  public abstract void Update();

  public abstract void SendMessageToHost(
    byte[] bytes,
    int length,
    NetTransferMode mode,
    int channel = 0);

  public abstract void DisconnectFromHost(NetError reason, bool now = false);

  public abstract string? GetRawLobbyIdentifier();
}
