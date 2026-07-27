// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Events.EventSplitVoteAnimation
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using MegaCrit.Sts2.Core.Audio.Debug;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Random;
using MegaCrit.Sts2.Core.Runs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Events;

public class EventSplitVoteAnimation
{
  private NEventLayout _eventLayout;
  private IRunState _runState;
  private int _ticks;
  private Player _winner;
  private readonly List<Player> _sortedPlayers = new List<Player>();
  private Player? _currentlyHighlightedPlayer;

  public EventSplitVoteAnimation(NEventLayout eventLayout, IRunState runState)
  {
    this._eventLayout = eventLayout;
    this._runState = runState;
  }

  public async Task TryPlay(NEventOptionButton chosenButton)
  {
    if (this._eventLayout.OptionButtons.Count<NEventOptionButton>((Func<NEventOptionButton, bool>) (b => b.VoteContainer.Players.Any<Player>())) == 1)
      return;
    Rng rng = new Rng(this._runState.Rng.Seed + (ulong) this._runState.ActFloor);
    this._ticks = rng.NextInt(12, 18);
    float num = rng.NextFloat(0.05f, 0.3f);
    this._winner = rng.NextItem<Player>(chosenButton.VoteContainer.Players);
    foreach (NEventOptionButton optionButton in this._eventLayout.OptionButtons)
      this._sortedPlayers.AddRange(optionButton.VoteContainer.Players);
    Tween tween = ((Node) this._eventLayout).CreateTween();
    tween.TweenMethod(Callable.From<float>(new Action<float>(this.TickSplitVoteAnimation)), Variant.op_Implicit(0.0f), Variant.op_Implicit(1f), 1.2000000476837158).SetTrans((Tween.TransitionType) 1L).SetEase((Tween.EaseType) 1L);
    tween.TweenInterval((double) num);
    if (!await tween.AwaitFinished((Node) this._eventLayout))
      return;
    chosenButton.VoteContainer.BouncePlayers();
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
    if (this._currentlyHighlightedPlayer != null)
    {
      foreach (NEventOptionButton optionButton in this._eventLayout.OptionButtons)
      {
        if (optionButton.VoteContainer.Players.Contains<Player>(this._currentlyHighlightedPlayer))
          optionButton.VoteContainer.SetPlayerHighlighted(this._currentlyHighlightedPlayer, false);
      }
    }
    this._currentlyHighlightedPlayer = player;
    if (player == null)
      return;
    foreach (NEventOptionButton optionButton in this._eventLayout.OptionButtons)
    {
      if (optionButton.VoteContainer.Players.Contains<Player>(player))
        optionButton.VoteContainer.SetPlayerHighlighted(player, true);
    }
  }
}
