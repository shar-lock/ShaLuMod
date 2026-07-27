// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Screens.Map.MapSplitVoteAnimation
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using MegaCrit.Sts2.Core.Audio.Debug;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Map;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Random;
using MegaCrit.Sts2.Core.Runs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Screens.Map;

public class MapSplitVoteAnimation
{
  private NMapScreen _mapScreen;
  private RunState _runState;
  private Dictionary<MapCoord, NMapPoint> _mapPointDictionary;
  private int _ticks;
  private Player _winner;
  private List<Player> _sortedPlayers;
  private Player? _currentlyHighlightedPlayer;

  public MapSplitVoteAnimation(
    NMapScreen mapScreen,
    RunState runState,
    Dictionary<MapCoord, NMapPoint> mapPointDictionary)
  {
    this._mapScreen = mapScreen;
    this._runState = runState;
    this._mapPointDictionary = mapPointDictionary;
  }

  public async Task TryPlay(MapCoord selectedCoord)
  {
    MapCoord? nullable1 = new MapCoord?();
    bool flag = true;
    List<Player> items = new List<Player>();
    foreach (KeyValuePair<Player, MapCoord?> playerVote in this._mapScreen.PlayerVoteDictionary)
    {
      MapCoord? nullable2;
      if (!nullable1.HasValue)
      {
        nullable1 = playerVote.Value;
      }
      else
      {
        MapCoord? nullable3 = playerVote.Value;
        nullable2 = nullable1;
        if ((nullable3.HasValue == nullable2.HasValue ? (nullable3.HasValue ? (nullable3.GetValueOrDefault() != nullable2.GetValueOrDefault() ? 1 : 0) : 0) : 1) != 0)
          flag = false;
      }
      nullable2 = playerVote.Value;
      MapCoord mapCoord = selectedCoord;
      if ((nullable2.HasValue ? (nullable2.GetValueOrDefault() == mapCoord ? 1 : 0) : 0) != 0)
        items.Add(playerVote.Key);
    }
    if (flag)
      return;
    Rng rng = new Rng(this._runState.Rng.Seed + (ulong) this._runState.ActFloor);
    this._ticks = rng.NextInt(12, 18);
    float num = rng.NextFloat(0.05f, 0.3f);
    this._winner = rng.NextItem<Player>((IEnumerable<Player>) items);
    this._sortedPlayers = this._mapScreen.PlayerVoteDictionary.ToList<KeyValuePair<Player, MapCoord?>>().Where<KeyValuePair<Player, MapCoord?>>((Func<KeyValuePair<Player, MapCoord?>, bool>) (p => p.Value.HasValue)).OrderBy<KeyValuePair<Player, MapCoord?>, MapCoord?>((Func<KeyValuePair<Player, MapCoord?>, MapCoord?>) (p => p.Value), (IComparer<MapCoord?>) Comparer<MapCoord?>.Create(new Comparison<MapCoord?>(this.MapCoordComparer))).ThenBy<KeyValuePair<Player, MapCoord?>, int>((Func<KeyValuePair<Player, MapCoord?>, int>) (p => this._mapPointDictionary[p.Value.Value].VoteContainer.GetVoteIndex(p.Key))).Select<KeyValuePair<Player, MapCoord?>, Player>((Func<KeyValuePair<Player, MapCoord?>, Player>) (p => p.Key)).ToList<Player>();
    Tween tween = ((Node) this._mapScreen).CreateTween();
    tween.TweenMethod(Callable.From<float>(new Action<float>(this.TickSplitVoteAnimation)), Variant.op_Implicit(0.0f), Variant.op_Implicit(1f), 1.2000000476837158).SetTrans((Tween.TransitionType) 1L).SetEase((Tween.EaseType) 1L);
    tween.TweenInterval((double) num);
    if (!await tween.AwaitFinished((Node) this._mapScreen))
      return;
    this._mapPointDictionary[selectedCoord].VoteContainer.BouncePlayers();
  }

  private int MapCoordComparer(MapCoord? a, MapCoord? b)
  {
    MapCoord? nullable1 = a;
    MapCoord? nullable2 = b;
    if ((nullable1.HasValue == nullable2.HasValue ? (nullable1.HasValue ? (nullable1.GetValueOrDefault() == nullable2.GetValueOrDefault() ? 1 : 0) : 1) : 0) != 0)
      return 0;
    if (!a.HasValue)
      return 1;
    if (!b.HasValue)
      return -1;
    if (a.Value.col != b.Value.col)
      return a.Value.col.CompareTo(b.Value.col);
    return a.Value.row != b.Value.row ? a.Value.row.CompareTo(b.Value.row) : 0;
  }

  private void TickSplitVoteAnimation(float value)
  {
    int num = Mathf.RoundToInt(value * (float) this._ticks);
    int index = ((this._sortedPlayers.IndexOf(this._winner) - (this._ticks - num)) % this._sortedPlayers.Count + this._sortedPlayers.Count) % this._sortedPlayers.Count;
    if (this._currentlyHighlightedPlayer == this._sortedPlayers[index])
      return;
    this.HighlightPlayer(this._sortedPlayers[index]);
    NDebugAudioManager.Instance.Play("map_split_tick.mp3", 0.15f, PitchVariance.Small);
  }

  private void HighlightPlayer(Player? player)
  {
    if (this._currentlyHighlightedPlayer == player)
      return;
    MapCoord? playerVote;
    if (this._currentlyHighlightedPlayer != null)
    {
      playerVote = this._mapScreen.PlayerVoteDictionary[this._currentlyHighlightedPlayer];
      this._mapPointDictionary[playerVote.Value].VoteContainer.SetPlayerHighlighted(this._currentlyHighlightedPlayer, false);
    }
    this._currentlyHighlightedPlayer = player;
    if (player == null)
      return;
    playerVote = this._mapScreen.PlayerVoteDictionary[player];
    this._mapPointDictionary[playerVote.Value].VoteContainer.SetPlayerHighlighted(player, true);
  }
}
