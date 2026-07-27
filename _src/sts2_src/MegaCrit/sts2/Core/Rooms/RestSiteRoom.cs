// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Rooms.RestSiteRoom
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Entities.RestSite;
using MegaCrit.Sts2.Core.GameActions;
using MegaCrit.Sts2.Core.Hooks;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Multiplayer.Game;
using MegaCrit.Sts2.Core.Nodes;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Runs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Rooms;

public class RestSiteRoom : AbstractRoom
{
  private RestSiteSynchronizer? _synchronizer;

  public override RoomType RoomType => RoomType.RestSite;

  public override ModelId? ModelId => (ModelId) null;

  public IReadOnlyList<RestSiteOption> Options
  {
    get
    {
      return this._synchronizer?.GetLocalOptions() ?? (IReadOnlyList<RestSiteOption>) Array.Empty<RestSiteOption>();
    }
  }

  public override async Task EnterInternal(IRunState? runState, bool isRestoringRoomStackBase)
  {
    if (isRestoringRoomStackBase)
      throw new InvalidOperationException("RestSiteRoom does not support room stack reconstruction.");
    this._synchronizer = RunManager.Instance.RestSiteSynchronizer;
    this._synchronizer.BeginRestSite();
    if (runState == null)
      return;
    await PreloadManager.LoadRoomRestSite(runState.Act, (IEnumerable<RestSiteOption>) this.Options);
    this.ShowRoomNode(runState);
    await Hook.AfterRoomEntered(runState, (AbstractRoom) this);
  }

  public override async Task Exit(IRunState? runState)
  {
    RunManager.Instance.RestSiteSynchronizer.BeforeLocalRestSiteExited();
    NRestSiteRoom.Instance?.BeforeExitingRoom();
    await RunManager.Instance.RestSiteSynchronizer.AfterAllRestSitesCompleted();
    RunManager.Instance.ChecksumTracker.GenerateChecksum("Exiting rest site room", (GameAction) null);
  }

  public override Task Resume(AbstractRoom _, IRunState? runState)
  {
    this.ShowRoomNode(runState);
    return Task.CompletedTask;
  }

  private void ShowRoomNode(IRunState runState)
  {
    NRun.Instance?.SetCurrentRoom((Control) NRestSiteRoom.Create(this, runState));
  }
}
