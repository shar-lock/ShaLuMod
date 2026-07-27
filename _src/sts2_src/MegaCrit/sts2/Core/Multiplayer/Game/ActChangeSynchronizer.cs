// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Multiplayer.Game.ActChangeSynchronizer
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Nodes.Screens;
using MegaCrit.Sts2.Core.Nodes.Screens.Overlays;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Runs;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable enable
namespace MegaCrit.Sts2.Core.Multiplayer.Game;

public class ActChangeSynchronizer
{
  private readonly RunState _runState;
  private readonly List<bool> _readyPlayers = new List<bool>();
  private readonly Logger _logger = new Logger(nameof (ActChangeSynchronizer), LogType.GameSync);
  private int _lastTransitioningActIndex = -1;

  public ActChangeSynchronizer(RunState runState)
  {
    this._runState = runState;
    for (int index = 0; index < runState.Players.Count; ++index)
      this._readyPlayers.Add(false);
  }

  public void SetLocalPlayerReady()
  {
    this._logger.Info("Local player ready to move to next act");
    Player me = LocalContext.GetMe((IPlayerCollection) this._runState);
    if (me == null)
      return;
    RunManager.Instance.ActionQueueSynchronizer.RequestEnqueue((GameAction) new VoteToMoveToNextActAction(me, this._runState.CurrentActIndex));
  }

  public bool IsWaitingForOtherPlayers()
  {
    int playerSlotIndex = this._runState.GetPlayerSlotIndex(LocalContext.NetId.Value);
    for (int index = 0; index < this._readyPlayers.Count; ++index)
    {
      if (!this._readyPlayers[index] && index != playerSlotIndex)
        return true;
    }
    return false;
  }

  public void OnPlayerReady(Player player, int actIndex)
  {
    this._logger.Debug($"Player {player.NetId} ready to move to next act from {actIndex}");
    AbstractRoom currentRoom = this._runState.CurrentRoom;
    if ((currentRoom == null || !currentRoom.IsVictoryRoom) && (actIndex < this._runState.CurrentActIndex || actIndex <= this._lastTransitioningActIndex))
    {
      this._logger.Warn($"Player {player.NetId} tried to transition to next act from index {actIndex}, but the current index is {this._runState.CurrentActIndex} and we last transitioned from {this._lastTransitioningActIndex}. Ignoring.");
    }
    else
    {
      this._readyPlayers[this._runState.GetPlayerSlotIndex(player)] = true;
      if (!this._readyPlayers.All<bool>((Func<bool, bool>) (x => x)))
        return;
      this.MoveToNextAct();
    }
  }

  private void MoveToNextAct()
  {
    for (int index = 0; index < this._readyPlayers.Count; ++index)
      this._readyPlayers[index] = false;
    this._logger.Info("All players ready to move to next act, beginning transition");
    this._lastTransitioningActIndex = this._runState.CurrentActIndex;
    ++this._runState.ActFloor;
    TaskHelper.RunSafely(RunManager.Instance.EnterNextAct());
    if (!(NOverlayStack.Instance?.Peek() is NRewardsScreen nrewardsScreen))
      return;
    nrewardsScreen.HideWaitingForPlayersScreen();
  }
}
