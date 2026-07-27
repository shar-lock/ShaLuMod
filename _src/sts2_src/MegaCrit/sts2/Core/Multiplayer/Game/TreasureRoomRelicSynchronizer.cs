// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Multiplayer.Game.TreasureRoomRelicSynchronizer
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Entities.TreasureRelicPicking;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.GameActions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Hooks;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Relics;
using MegaCrit.Sts2.Core.Random;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.Runs.History;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable enable
namespace MegaCrit.Sts2.Core.Multiplayer.Game;

public class TreasureRoomRelicSynchronizer
{
  private readonly IPlayerCollection _playerCollection;
  private readonly ulong _localPlayerId;
  private readonly RelicGrabBag _sharedGrabBag;
  private readonly ActionQueueSynchronizer _actionQueueSynchronizer;
  private readonly Rng _rng;
  private readonly Logger _logger = new Logger(nameof (TreasureRoomRelicSynchronizer), LogType.GameSync);
  private List<RelicModel>? _currentRelics;
  private readonly List<TreasureRoomRelicSynchronizer.PlayerVote> _votes = new List<TreasureRoomRelicSynchronizer.PlayerVote>();
  private TreasureRoomRelicSynchronizer.PlayerVote? _predictedVote;
  private bool _singleplayerSkipped;

  public IReadOnlyList<RelicModel>? CurrentRelics
  {
    get => (IReadOnlyList<RelicModel>) this._currentRelics;
  }

  public event Action? VotesChanged;

  public event Action<List<RelicPickingResult>>? RelicsAwarded;

  private Player LocalPlayer => this._playerCollection.GetPlayer(this._localPlayerId);

  public TreasureRoomRelicSynchronizer(
    IPlayerCollection playerCollection,
    ulong localPlayerId,
    ActionQueueSynchronizer actionQueueSynchronizer,
    RelicGrabBag sharedGrabBag,
    Rng rng)
  {
    this._playerCollection = playerCollection;
    this._localPlayerId = localPlayerId;
    this._actionQueueSynchronizer = actionQueueSynchronizer;
    this._sharedGrabBag = sharedGrabBag;
    this._rng = rng;
  }

  public void BeginRelicPicking()
  {
    if (this.CurrentRelics != null)
      throw new InvalidOperationException("Attempted to start new relic picking session while one was already occurring!");
    this._currentRelics = new List<RelicModel>();
    this._votes.Clear();
    this._predictedVote = (TreasureRoomRelicSynchronizer.PlayerVote) null;
    foreach (Player player in (IEnumerable<Player>) this._playerCollection.Players)
    {
      this._votes.Add(new TreasureRoomRelicSynchronizer.PlayerVote()
      {
        voteReceived = false
      });
      IRunState runState = player.RunState;
      if (Hook.ShouldGenerateTreasure(runState, player))
      {
        RelicRarity rarity = RelicFactory.RollRarity(this._rng);
        this._currentRelics.Add(this.TryGetRelicForTutorial(player) ?? this._sharedGrabBag.PullFromFront(rarity, runState) ?? RelicFactory.FallbackRelic);
      }
    }
    if (this._currentRelics.Count > 0)
    {
      if (RunManager.Instance.IsSingleplayerOrFakeMultiplayer && this._playerCollection.Players.Count > 1)
      {
        foreach (Player player in (IEnumerable<Player>) this._playerCollection.Players)
        {
          if (player != this.LocalPlayer)
          {
            TreasureRoomRelicSynchronizer.PlayerVote vote = this._votes[this._playerCollection.GetPlayerSlotIndex(player)];
            vote.index = new int?(this._rng.NextInt(this._currentRelics.Count));
            vote.voteReceived = true;
          }
        }
      }
      Action votesChanged = this.VotesChanged;
      if (votesChanged == null)
        return;
      votesChanged();
    }
    else
      this.EndRelicVoting();
  }

  public void SkipRelicLocally() => this.PickRelicLocally(new int?());

  public void PickRelicLocally(int? index)
  {
    if (index.HasValue)
      this._logger.Debug($"Relic index {index} ({this._currentRelics?[index.Value]}) is being picked by local player {this.LocalPlayer.NetId}");
    else
      this._logger.Debug($"Relic has been skipped by local player {this.LocalPlayer.NetId}");
    if (this._currentRelics == null)
      throw new InvalidOperationException("Attempted to pick relic while relic picking is not active!");
    this._predictedVote = new TreasureRoomRelicSynchronizer.PlayerVote()
    {
      index = index,
      voteReceived = true
    };
    this._actionQueueSynchronizer.RequestEnqueue((GameAction) new PickRelicAction(this.LocalPlayer, index));
    Action votesChanged = this.VotesChanged;
    if (votesChanged == null)
      return;
    votesChanged();
  }

  public void OnPicked(Player player, int? index)
  {
    if (index.HasValue)
      this._logger.Debug($"Player {player} picked relic at index {index}: {this._currentRelics?[index.Value]}");
    else
      this._logger.Debug($"Player {player} skipped relic");
    if (this._currentRelics == null)
    {
      this._logger.Warn("Attempted to pick relic while relic picking is not active!");
    }
    else
    {
      int? nullable = index;
      int count = this._currentRelics.Count;
      if (nullable.GetValueOrDefault() >= count & nullable.HasValue)
        throw new IndexOutOfRangeException($"Attempted to pick relic at index {index}, but there are only {this._currentRelics.Count} to choose from!");
      if (!index.HasValue && this._playerCollection.Players.Count == 1)
      {
        this._singleplayerSkipped = true;
      }
      else
      {
        TreasureRoomRelicSynchronizer.PlayerVote vote = this._votes[this._playerCollection.GetPlayerSlotIndex(player)];
        vote.index = index;
        vote.voteReceived = true;
        Action votesChanged1 = this.VotesChanged;
        if (votesChanged1 != null)
          votesChanged1();
        if (!this._votes.All<TreasureRoomRelicSynchronizer.PlayerVote>((Func<TreasureRoomRelicSynchronizer.PlayerVote, bool>) (v => v.voteReceived)))
          return;
        if (this._predictedVote != null)
        {
          TreasureRoomRelicSynchronizer.PlayerVote predictedVote = this._predictedVote;
          this._predictedVote = (TreasureRoomRelicSynchronizer.PlayerVote) null;
          int? index1 = this._votes[this._playerCollection.GetPlayerSlotIndex(this.LocalPlayer)].index;
          int? index2 = predictedVote.index;
          if (!(index1.GetValueOrDefault() == index2.GetValueOrDefault() & index1.HasValue == index2.HasValue))
          {
            Action votesChanged2 = this.VotesChanged;
            if (votesChanged2 != null)
              votesChanged2();
          }
        }
        this.AwardRelics();
        this.EndRelicVoting();
      }
    }
  }

  private void AwardRelics()
  {
    Dictionary<int, List<Player>> dictionary = new Dictionary<int, List<Player>>();
    for (int key = 0; key < this._currentRelics.Count; ++key)
      dictionary[key] = new List<Player>();
    for (int index = 0; index < this._votes.Count; ++index)
    {
      Player player = this._playerCollection.Players[index];
      TreasureRoomRelicSynchronizer.PlayerVote vote = this._votes[index];
      if (vote.index.HasValue)
      {
        List<Player> valueOrDefault = CollectionExtensions.GetValueOrDefault<int, List<Player>>((IReadOnlyDictionary<int, List<Player>>) dictionary, vote.index.Value, new List<Player>());
        valueOrDefault.Add(player);
        dictionary[vote.index.Value] = valueOrDefault;
      }
    }
    List<RelicPickingResult> results = new List<RelicPickingResult>();
    List<RelicModel> list1 = new List<RelicModel>();
    foreach (KeyValuePair<int, List<Player>> keyValuePair in dictionary)
    {
      RelicModel currentRelic = this._currentRelics[keyValuePair.Key];
      if (keyValuePair.Value.Count == 0)
        list1.Add(currentRelic);
      else if (keyValuePair.Value.Count == 1)
        results.Add(new RelicPickingResult()
        {
          type = RelicPickingResultType.OnlyOnePlayerVoted,
          relic = currentRelic,
          player = keyValuePair.Value[0]
        });
      else if (keyValuePair.Value.Count > 1)
      {
        RelicPickingFightMove[] possibleMoves = Enum.GetValues<RelicPickingFightMove>();
        results.Add(RelicPickingResult.GenerateRelicFight(keyValuePair.Value, currentRelic, (Func<RelicPickingFightMove>) (() => this._rng.NextItem<RelicPickingFightMove>((IEnumerable<RelicPickingFightMove>) possibleMoves))));
      }
    }
    List<Player> list2 = this._playerCollection.Players.Where<Player>((Func<Player, bool>) (p =>
    {
      bool flag1 = results.Find((Predicate<RelicPickingResult>) (r => r.player == p)) != null;
      TreasureRoomRelicSynchronizer.PlayerVote vote = this._votes[this._playerCollection.GetPlayerSlotIndex(p)];
      bool flag2 = vote.voteReceived && !vote.index.HasValue;
      return !flag1 && !flag2;
    })).ToList<Player>();
    list1.StableShuffle<RelicModel>(this._rng);
    for (int index = 0; index < list1.Count; ++index)
    {
      if (index < list2.Count)
        results.Add(new RelicPickingResult()
        {
          type = RelicPickingResultType.ConsolationPrize,
          player = list2[index],
          relic = list1[index]
        });
      else
        results.Add(new RelicPickingResult()
        {
          type = RelicPickingResultType.Skipped,
          player = (Player) null,
          relic = list1[index]
        });
    }
    Action<List<RelicPickingResult>> relicsAwarded = this.RelicsAwarded;
    if (relicsAwarded == null)
      return;
    relicsAwarded(results);
  }

  public void OnRoomExited()
  {
    if (!this._singleplayerSkipped)
      return;
    this.EndRelicVoting();
  }

  private void EndRelicVoting()
  {
    this._currentRelics = (List<RelicModel>) null;
    this._singleplayerSkipped = false;
  }

  public TreasureRoomRelicSynchronizer.PlayerVote GetPlayerVote(Player player)
  {
    return player == this.LocalPlayer && this._predictedVote != null ? this._predictedVote : this._votes[this._playerCollection.GetPlayerSlotIndex(player)];
  }

  public void CompleteWithNoRelics()
  {
    this._logger.Debug("Completing relic picking with no relics (empty chest)");
    Action<List<RelicPickingResult>> relicsAwarded = this.RelicsAwarded;
    if (relicsAwarded != null)
      relicsAwarded(new List<RelicPickingResult>());
    this._currentRelics = (List<RelicModel>) null;
  }

  private RelicModel? TryGetRelicForTutorial(Player player)
  {
    if (this._playerCollection.Players.Count != 1 || player.UnlockState.NumberOfRuns != 0 || player.RunState.MapPointHistory.SelectMany<IReadOnlyList<MapPointHistoryEntry>, MapPointHistoryEntry>((Func<IReadOnlyList<MapPointHistoryEntry>, IEnumerable<MapPointHistoryEntry>>) (l => (IEnumerable<MapPointHistoryEntry>) l)).Count<MapPointHistoryEntry>((Func<MapPointHistoryEntry, bool>) (p => p.HasRoomOfType(RoomType.Treasure))) != 1 || !player.RelicGrabBag.Contains((RelicModel) ModelDb.Relic<Gorget>()))
      return (RelicModel) null;
    Log.Info("Forcing specific relic because it's the player's first treasure chest ever");
    player.RelicGrabBag.Remove<Gorget>();
    this._sharedGrabBag.Remove<Gorget>();
    return (RelicModel) ModelDb.Relic<Gorget>();
  }

  public class PlayerVote
  {
    public int? index;
    public bool voteReceived;
  }
}
