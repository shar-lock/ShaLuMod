// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Multiplayer.Quality.NetQualityTracker
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Multiplayer.Game;
using MegaCrit.Sts2.Core.Multiplayer.Messages;
using System;
using System.Collections.Generic;

#nullable enable
namespace MegaCrit.Sts2.Core.Multiplayer.Quality;

public class NetQualityTracker : IDisposable
{
  public const int sendRateMsec = 200;
  public const int logRateMsec = 20000;
  private readonly INetGameService _netService;
  private readonly Logger _logger = new Logger(nameof (NetQualityTracker), LogType.Network);
  private readonly List<ConnectionStats> _stats = new List<ConnectionStats>();
  private ulong? _lastUpdateMsec;
  private ulong? _lastLogMsec;
  private bool _isLoading;
  public Func<ulong>? getTimeMsec;

  public bool IsGameLoading => this._isLoading;

  public NetQualityTracker(INetGameService netService)
  {
    this._netService = netService;
    netService.RegisterMessageHandler<HeartbeatRequestMessage>(new MessageHandlerDelegate<HeartbeatRequestMessage>(this.HandleHeartbeatRequestMessage));
    netService.RegisterMessageHandler<HeartbeatResponseMessage>(new MessageHandlerDelegate<HeartbeatResponseMessage>(this.HandleHeartbeatResponseMessage));
  }

  public void Dispose()
  {
    this._netService.UnregisterMessageHandler<HeartbeatRequestMessage>(new MessageHandlerDelegate<HeartbeatRequestMessage>(this.HandleHeartbeatRequestMessage));
    this._netService.UnregisterMessageHandler<HeartbeatResponseMessage>(new MessageHandlerDelegate<HeartbeatResponseMessage>(this.HandleHeartbeatResponseMessage));
  }

  private ulong GetCurrentTime()
  {
    Func<ulong> getTimeMsec = this.getTimeMsec;
    return getTimeMsec == null ? Time.GetTicksMsec() : getTimeMsec();
  }

  public void OnPeerConnected(ulong peerId) => this._stats.Add(new ConnectionStats(peerId));

  public void OnPeerDisconnected(ulong peerId)
  {
    this._stats.RemoveAll((Predicate<ConnectionStats>) (s => (long) s.PeerId == (long) peerId));
  }

  public void SetIsLoading(bool isLoading)
  {
    Log.Debug($"Loading set to {isLoading}");
    this._isLoading = isLoading;
  }

  public void Update()
  {
    ulong currentTime = this.GetCurrentTime();
    if (!this._lastUpdateMsec.HasValue)
      this._lastUpdateMsec = new ulong?(currentTime);
    else if (currentTime - this._lastUpdateMsec.Value >= 200UL)
    {
      foreach (ConnectionStats stat in this._stats)
        this._netService.SendMessage<HeartbeatRequestMessage>(stat.GenerateHeartbeat(currentTime), stat.PeerId);
      this._lastUpdateMsec = new ulong?(currentTime);
    }
    if (!this._logger.WillLog(LogLevel.Debug))
      return;
    if (!this._lastLogMsec.HasValue)
    {
      this._lastLogMsec = new ulong?(currentTime);
    }
    else
    {
      ulong num1 = currentTime;
      ulong? lastLogMsec = this._lastLogMsec;
      ulong? nullable = lastLogMsec.HasValue ? new ulong?(num1 - lastLogMsec.GetValueOrDefault()) : new ulong?();
      ulong num2 = 20000;
      if (!(nullable.GetValueOrDefault() >= num2 & nullable.HasValue) || this._stats.Count <= 0)
        return;
      this._lastLogMsec = new ulong?(currentTime);
      this._logger.Debug($"Connection statistics at {Log.Timestamp}:");
      foreach (ConnectionStats stat in this._stats)
        this._logger.Debug($"\t{stat.PeerId} - Ping: {stat.PingMsec}. Packet Loss: {stat.PacketLoss}.");
    }
  }

  private void HandleHeartbeatRequestMessage(HeartbeatRequestMessage message, ulong senderId)
  {
    this._netService.SendMessage<HeartbeatResponseMessage>(new HeartbeatResponseMessage()
    {
      counter = message.counter,
      isLoading = this._isLoading
    }, senderId);
  }

  private void HandleHeartbeatResponseMessage(HeartbeatResponseMessage message, ulong senderId)
  {
    this.GetStatsForPeer(senderId)?.OnHeartbeatReceived(message, this.GetCurrentTime());
  }

  public ConnectionStats? GetStatsForPeer(ulong peerId)
  {
    foreach (ConnectionStats stat in this._stats)
    {
      if ((long) stat.PeerId == (long) peerId)
        return stat;
    }
    return (ConnectionStats) null;
  }
}
