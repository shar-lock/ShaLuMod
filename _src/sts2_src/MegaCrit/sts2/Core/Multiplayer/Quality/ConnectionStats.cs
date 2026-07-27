// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Multiplayer.Quality.ConnectionStats
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Multiplayer.Messages;

#nullable enable
namespace MegaCrit.Sts2.Core.Multiplayer.Quality;

public class ConnectionStats
{
  public const int ringBufferSize = 20;
  private const float _weightedAverageFactor = 0.2f;
  private HeartbeatStatus?[] _statuses = new HeartbeatStatus?[20];
  private int _nextIndex;
  private readonly Logger _logger;

  public ulong PeerId { get; private set; }

  public float PingMsec { get; private set; }

  public float PacketLoss { get; private set; }

  public ulong? LastReceivedTime { get; private set; }

  public bool RemoteIsLoading { get; private set; }

  public ConnectionStats(ulong peerId)
  {
    this.PeerId = peerId;
    this._logger = new Logger($"{nameof (ConnectionStats)} ({peerId})", LogType.Network);
  }

  public HeartbeatRequestMessage GenerateHeartbeat(ulong timeMsec)
  {
    this._logger.VeryDebug($"Generating heartbeat {this._nextIndex} for time {timeMsec}");
    int index = this._nextIndex % 20;
    HeartbeatStatus heartbeatStatus = new HeartbeatStatus()
    {
      counter = this._nextIndex,
      sentMsec = timeMsec
    };
    HeartbeatStatus? statuse = this._statuses[index];
    if (statuse.HasValue && !statuse.Value.receivedMsec.HasValue)
    {
      this._logger.VeryDebug($"Heartbeat {statuse.Value.counter} ({statuse.Value.sentMsec}) was never received, marking as lost");
      this.OnPacketLost();
    }
    this._statuses[index] = new HeartbeatStatus?(heartbeatStatus);
    HeartbeatRequestMessage heartbeat = new HeartbeatRequestMessage()
    {
      counter = this._nextIndex
    };
    ++this._nextIndex;
    return heartbeat;
  }

  public void OnHeartbeatReceived(HeartbeatResponseMessage message, ulong timeMsec)
  {
    this._logger.VeryDebug($"Received heartbeat for {message.counter}");
    int num = this._nextIndex - 20;
    if (message.counter < num || message.counter >= this._nextIndex)
    {
      this._logger.VeryDebug($"Counter {message.counter} is less than {num} and greater than {this._nextIndex}");
    }
    else
    {
      int index = message.counter % 20;
      if (index >= this._statuses.Length)
        return;
      HeartbeatStatus valueOrDefault = this._statuses[index].GetValueOrDefault();
      if (valueOrDefault.counter != message.counter)
        this._logger.VeryDebug($"Counter in message {message.counter} does not match counter at index {index}, which is {valueOrDefault.counter}");
      else if (valueOrDefault.receivedMsec.HasValue)
      {
        this._logger.VeryDebug($"Already received message for index {message.counter}");
      }
      else
      {
        valueOrDefault.receivedMsec = new ulong?(timeMsec);
        this._statuses[index] = new HeartbeatStatus?(valueOrDefault);
        if (!message.isLoading)
          this.PingMsec = Mathf.Lerp(this.PingMsec, (float) (int) ((long) valueOrDefault.receivedMsec.Value - (long) valueOrDefault.sentMsec), 0.2f);
        else
          Log.Debug("Not updating ping because sender is loading");
        this.PacketLoss = Mathf.Lerp(this.PacketLoss, 0.0f, 0.2f);
        this.LastReceivedTime = new ulong?(valueOrDefault.receivedMsec.Value);
        this.RemoteIsLoading = message.isLoading;
      }
    }
  }

  private void OnPacketLost() => this.PacketLoss = Mathf.Lerp(this.PacketLoss, 1f, 0.2f);
}
