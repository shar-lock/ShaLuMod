// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Multiplayer.Connection.ENetClientConnectionInitializer
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Entities.Multiplayer;
using MegaCrit.Sts2.Core.Multiplayer.Game;
using MegaCrit.Sts2.Core.Multiplayer.Transport;
using MegaCrit.Sts2.Core.Multiplayer.Transport.ENet;
using MegaCrit.Sts2.Core.Platform;
using System;
using System.Threading;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Multiplayer.Connection;

public class ENetClientConnectionInitializer : IClientConnectionInitializer
{
  private readonly ulong _netId;
  private readonly string _ip;
  private readonly ushort _port;

  public ENetClientConnectionInitializer(ulong netId, string ip, ushort port)
  {
    this._netId = netId;
    this._ip = ip;
    this._port = port;
  }

  public async Task<NetErrorInfo?> Connect(
    INetClientGameService netService,
    CancellationToken cancelToken = default (CancellationToken))
  {
    ENetClient client = !netService.IsConnected ? new ENetClient((INetClientHandler) netService) : throw new InvalidOperationException("NetClientGameService must not be connected when passed to ENetClientConnectionInitializer!");
    netService.Initialize((NetClient) client, PlatformType.None);
    return await client.ConnectToHost(this._netId, this._ip, this._port, cancelToken);
  }
}
