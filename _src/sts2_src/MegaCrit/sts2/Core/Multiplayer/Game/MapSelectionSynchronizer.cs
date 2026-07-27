// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Multiplayer.Game.MapSelectionSynchronizer
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Map;
using MegaCrit.Sts2.Core.Random;
using MegaCrit.Sts2.Core.Runs;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable enable
namespace MegaCrit.Sts2.Core.Multiplayer.Game;

public class MapSelectionSynchronizer
{
  private readonly INetGameService _netService;
  private readonly ActionQueueSynchronizer _actionQueueSynchronizer;
  private readonly RunState _runState;
  private MapLocation _acceptingVotesFromSource;
  private readonly List<MapVote?> _votes = new List<MapVote?>();
  private readonly Logger _logger = new Logger(nameof (MapSelectionSynchronizer), LogType.GameSync);
  private readonly Rng _multiplayerMapPointSelection;

  public int MapGenerationCount { get; private set; }

  public event Action<Player, MapVote?, MapVote?>? PlayerVoteChanged;

  public event Action<Player>? PlayerVoteCancelled;

  public event Action? PlayerVotesCleared;

  public MapSelectionSynchronizer(
    INetGameService netService,
    ActionQueueSynchronizer actionQueueSynchronizer,
    RunState runState)
  {
    this._netService = netService;
    this._actionQueueSynchronizer = actionQueueSynchronizer;
    this._runState = runState;
    this._multiplayerMapPointSelection = new Rng(this._runState.Rng.Seed, "map_point_selection");
    this.OnLocationChanged(this._runState.MapLocation);
  }

  public void PlayerVotedForMapCoord(Player player, MapLocation source, MapVote? destination)
  {
    if (this._acceptingVotesFromSource != source)
    {
      this._logger.Warn($"Received map vote from player {player.NetId} for source {source}, but we're currently accepting votes for {this._acceptingVotesFromSource}");
    }
    else
    {
      int? mapGenerationCount1 = destination?.mapGenerationCount;
      int mapGenerationCount2 = this.MapGenerationCount;
      if (mapGenerationCount1.GetValueOrDefault() < mapGenerationCount2 & mapGenerationCount1.HasValue)
      {
        this._logger.Warn($"Received map vote from player {player.NetId} for destination {destination}, but the map generation count is lower than our current: {this.MapGenerationCount}");
      }
      else
      {
        int playerSlotIndex = this._runState.GetPlayerSlotIndex(player);
        MapVote? vote = this._votes[playerSlotIndex];
        this._votes[playerSlotIndex] = destination;
        Action<Player, MapVote?, MapVote?> playerVoteChanged = this.PlayerVoteChanged;
        if (playerVoteChanged != null)
          playerVoteChanged(player, vote, destination);
        if (destination.HasValue)
          this._logger.Debug($"Received vote to move to {destination} from player {player.NetId} (slot {playerSlotIndex})");
        else
          this._logger.Debug($"Received cancellation of vote from player {player.NetId} (slot {playerSlotIndex})");
        if (!this._votes.All<MapVote?>((Func<MapVote?, bool>) (p => p.HasValue && p.Value.mapGenerationCount == this.MapGenerationCount)) || this._netService.Type == NetGameType.Client)
          return;
        this._logger.Debug("All votes received and we are host, choosing coordinate");
        this.MoveToMapCoord();
      }
    }
  }

  public MapVote? GetVote(Player player) => this._votes[this._runState.GetPlayerSlotIndex(player)];

  private void MoveToMapCoord()
  {
    if (this._netService.Type == NetGameType.Client)
      throw new InvalidOperationException("Only host should be moving to new map point!");
    MapCoord coord = this._multiplayerMapPointSelection.NextItem<MapVote?>((IEnumerable<MapVote?>) this._votes).Value.coord;
    this._acceptingVotesFromSource.coord = new MapCoord?(coord);
    this._logger.Debug($"Moving to coordinate {coord}");
    this._actionQueueSynchronizer.RequestEnqueue((GameAction) new MoveToMapCoordAction(LocalContext.GetMe((IPlayerCollection) this._runState), coord));
  }

  public void OnLocationChanged(MapLocation location)
  {
    this._acceptingVotesFromSource = location;
    this._votes.Clear();
    for (int index = 0; index < this._runState.Players.Count; ++index)
      this._votes.Add(new MapVote?());
    Action playerVotesCleared = this.PlayerVotesCleared;
    if (playerVotesCleared == null)
      return;
    playerVotesCleared();
  }

  public void BeforeMapGenerated()
  {
    ++this.MapGenerationCount;
    for (int index = 0; index < this._votes.Count; ++index)
    {
      MapVote? vote = this._votes[index];
      if (vote.HasValue && vote.Value.mapGenerationCount < this.MapGenerationCount)
      {
        Player player = this._runState.Players[index];
        this._logger.Debug($"Cancelling map vote for player {player.NetId} because the map has re-generated and their vote is old");
        this._votes[index] = new MapVote?();
        Action<Player> playerVoteCancelled = this.PlayerVoteCancelled;
        if (playerVoteCancelled != null)
          playerVoteCancelled(player);
      }
    }
  }
}
