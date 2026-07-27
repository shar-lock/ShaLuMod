// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Rooms.TreasureRoom
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Hooks;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Rewards;
using MegaCrit.Sts2.Core.Runs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Rooms;

public class TreasureRoom : AbstractRoom
{
  private IRunState? _runState;

  public override RoomType RoomType => RoomType.Treasure;

  public override ModelId? ModelId => (ModelId) null;

  public TreasureRoom(int actIndex)
  {
    if (actIndex < 0 || actIndex > 2)
      throw new ArgumentOutOfRangeException(nameof (actIndex), "must be between 0 and 2");
  }

  public override async Task EnterInternal(IRunState? runState, bool isRestoringRoomStackBase)
  {
    if (isRestoringRoomStackBase)
      throw new InvalidOperationException("TreasureRoom does not support room stack reconstruction.");
    if (runState != null)
    {
      await PreloadManager.LoadRoomTreasureAssets(runState.Act);
      NRun.Instance?.SetCurrentRoom((Control) NTreasureRoom.Create(this, runState));
      await Hook.AfterRoomEntered(runState, (AbstractRoom) this);
      this._runState = runState;
    }
    RunManager.Instance.TreasureRoomRelicSynchronizer.BeginRelicPicking();
  }

  public override Task Exit(IRunState? runState)
  {
    RunManager.Instance.TreasureRoomRelicSynchronizer.OnRoomExited();
    return Task.CompletedTask;
  }

  public override Task Resume(AbstractRoom _, IRunState? runState)
  {
    throw new NotImplementedException();
  }

  public Task<int> DoNormalRewards()
  {
    return RunManager.Instance.OneOffSynchronizer.DoLocalTreasureRoomRewards();
  }

  public async Task DoExtraRewardsIfNeeded()
  {
    Task localTask = (Task) null;
    List<RewardsSet> rewards = new List<RewardsSet>();
    foreach (Player player in (IEnumerable<Player>) this._runState.Players)
    {
      List<RewardsSet> rewardsSetList = rewards;
      rewardsSetList.Add(await RewardsCmd.GenerateForRoomEnd(player, (AbstractRoom) this));
      rewardsSetList = (List<RewardsSet>) null;
    }
    foreach (RewardsSet rewardsSet in rewards)
    {
      Task task = TaskHelper.RunSafely(rewardsSet.Offer());
      if (LocalContext.IsMe(rewardsSet.Player))
        localTask = task;
    }
    if (localTask == null)
      throw new InvalidOperationException("Tried to do extra rewards, but the local player is not in the run state!");
    await localTask;
    localTask = (Task) null;
    rewards = (List<RewardsSet>) null;
  }
}
