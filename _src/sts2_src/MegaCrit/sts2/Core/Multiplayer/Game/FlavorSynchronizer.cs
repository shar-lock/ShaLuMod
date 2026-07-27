// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Multiplayer.Game.FlavorSynchronizer
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Map;
using MegaCrit.Sts2.Core.Multiplayer.Messages.Game.Flavor;
using MegaCrit.Sts2.Core.Nodes;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.Runs;
using System;
using System.Collections.Generic;

#nullable enable
namespace MegaCrit.Sts2.Core.Multiplayer.Game;

public class FlavorSynchronizer : IDisposable
{
  private const ulong _pingDebounceMsec = 1000;
  private const ulong _mapPingDebounceMsec = 200;
  private readonly INetGameService _gameService;
  private readonly IPlayerCollection _playerCollection;
  private readonly ulong _localPlayerId;
  private ulong _nextAllowedPingTime;
  private readonly Dictionary<Player, NSpeechBubbleVfx?> _endTurnPingDialogues = new Dictionary<Player, NSpeechBubbleVfx>();

  public event Action<ulong>? OnEndTurnPingReceived;

  private Player LocalPlayer => this._playerCollection.GetPlayer(this._localPlayerId);

  public FlavorSynchronizer(
    INetGameService gameService,
    IPlayerCollection playerCollection,
    ulong localPlayerId)
  {
    this._gameService = gameService;
    this._playerCollection = playerCollection;
    this._localPlayerId = localPlayerId;
    this._gameService.RegisterMessageHandler<EndTurnPingMessage>(new MessageHandlerDelegate<EndTurnPingMessage>(this.HandleEndTurnPingMessage));
    this._gameService.RegisterMessageHandler<MapPingMessage>(new MessageHandlerDelegate<MapPingMessage>(this.HandleMapPingMessage));
  }

  public void Dispose()
  {
    this._gameService.UnregisterMessageHandler<EndTurnPingMessage>(new MessageHandlerDelegate<EndTurnPingMessage>(this.HandleEndTurnPingMessage));
    this._gameService.UnregisterMessageHandler<MapPingMessage>(new MessageHandlerDelegate<MapPingMessage>(this.HandleMapPingMessage));
  }

  public void SendEndTurnPing()
  {
    if (Time.GetTicksMsec() < this._nextAllowedPingTime)
      return;
    this._gameService.SendMessage<EndTurnPingMessage>(new EndTurnPingMessage());
    this._nextAllowedPingTime = Time.GetTicksMsec() + 1000UL;
    this.CreateEndTurnPingDialogueIfNecessary(this.LocalPlayer);
  }

  public void SendMapPing(MapCoord coord)
  {
    if (Time.GetTicksMsec() < this._nextAllowedPingTime)
      return;
    this._gameService.SendMessage<MapPingMessage>(new MapPingMessage()
    {
      coord = coord
    });
    this._nextAllowedPingTime = Time.GetTicksMsec() + 200UL;
    this.CreateMapPing(coord, this.LocalPlayer);
  }

  private void HandleEndTurnPingMessage(EndTurnPingMessage message, ulong senderId)
  {
    Action<ulong> turnPingReceived = this.OnEndTurnPingReceived;
    if (turnPingReceived != null)
      turnPingReceived(senderId);
    this.CreateEndTurnPingDialogueIfNecessary(this._playerCollection.GetPlayer(senderId));
  }

  private void CreateEndTurnPingDialogueIfNecessary(Player player)
  {
    if (NRun.Instance == null)
      return;
    NSpeechBubbleVfx child;
    this._endTurnPingDialogues.TryGetValue(player, out child);
    if (child != null && GodotObject.IsInstanceValid((GodotObject) child))
      ((Node) child).QueueFreeSafely();
    string str = player.Creature.IsDead ? "dead" : "alive";
    child = NSpeechBubbleVfx.Create(new LocString("characters", $"{player.Character.Id.Entry}.banter.{str}.endTurnPing").GetFormattedText(), player.Creature, 1.5, player.Character.SpeechBubbleColor);
    NCombatRoom instance = NCombatRoom.Instance;
    if (instance != null)
      ((Node) instance.CombatVfxContainer).AddChildSafely((Node) child);
    this._endTurnPingDialogues[player] = child;
  }

  private void HandleMapPingMessage(MapPingMessage message, ulong senderId)
  {
    this.CreateMapPing(message.coord, this._playerCollection.GetPlayer(senderId));
  }

  private void CreateMapPing(MapCoord coord, Player player)
  {
    if (NRun.Instance == null)
      return;
    NRun.Instance.GlobalUi.MapScreen.PingMapCoord(coord, player);
  }
}
