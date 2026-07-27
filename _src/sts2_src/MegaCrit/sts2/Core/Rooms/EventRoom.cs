// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Rooms.EventRoom
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.GameActions;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Hooks;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.Saves;
using MegaCrit.Sts2.Core.Saves.Runs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Rooms;

public class EventRoom : AbstractRoom
{
  private bool _isPreFinished;

  public override RoomType RoomType => RoomType.Event;

  public override ModelId ModelId => this.CanonicalEvent.Id;

  public EventModel CanonicalEvent { get; }

  public EventModel LocalMutableEvent => RunManager.Instance.EventSynchronizer.GetLocalEvent();

  public Action<EventModel>? OnStart { private get; init; }

  public override bool IsPreFinished => this._isPreFinished;

  public EventRoom(EventModel eventModel)
  {
    eventModel.AssertCanonical();
    this.CanonicalEvent = eventModel;
  }

  public EventRoom(SerializableRoom serializableRoom)
  {
    this.CanonicalEvent = SaveUtil.EventOrDeprecated(serializableRoom.EventId);
    if (!serializableRoom.IsPreFinished)
      return;
    this.MarkPreFinished();
  }

  public override async Task EnterInternal(IRunState? runState, bool isRestoringRoomStackBase)
  {
    RunManager.Instance.EventSynchronizer.BeginEvent(this.CanonicalEvent, this.IsPreFinished, this.OnStart);
    foreach (EventModel eventModel in (IEnumerable<EventModel>) RunManager.Instance.EventSynchronizer.Events)
    {
      eventModel.StateChanged += new Action<EventModel>(this.OnEventStateChanged);
      if (eventModel.IsFinished && !this.IsPreFinished)
        this.OnEventStateChanged(eventModel);
    }
    EventModel localEvent = RunManager.Instance.EventSynchronizer.GetLocalEvent();
    RunManager.Instance.EventSynchronizer.GenerateInternalCombatStateIfNecessary(localEvent);
    await PreloadManager.LoadRoomEventAssets(this.CanonicalEvent, runState ?? (IRunState) NullRunState.Instance);
    if (!isRestoringRoomStackBase)
      NRun.Instance?.SetCurrentRoom((Control) NEventRoom.Create(localEvent, runState, this._isPreFinished));
    if (runState != null)
      await Hook.AfterRoomEntered(runState, (AbstractRoom) this);
    await localEvent.AfterEventStarted();
    localEvent = (EventModel) null;
  }

  public override async Task Exit(IRunState? runState)
  {
    await RunManager.Instance.EventSynchronizer.AwaitPendingOptionTasks();
    EventModel localEvent = RunManager.Instance.EventSynchronizer.GetLocalEvent();
    if (localEvent.IsDeterministic)
      RunManager.Instance.ChecksumTracker.GenerateChecksum($"Exiting event room {localEvent.Id}", (GameAction) null);
    RunManager.Instance.EventSynchronizer.BeforeExitingRoom();
    foreach (EventModel eventModel in (IEnumerable<EventModel>) RunManager.Instance.EventSynchronizer.Events)
    {
      eventModel.StateChanged -= new Action<EventModel>(this.OnEventStateChanged);
      eventModel.EnsureCleanup();
    }
  }

  public override Task Resume(AbstractRoom exitedRoom, IRunState? runState)
  {
    RunManager.Instance.EventSynchronizer.ResumeEvents(exitedRoom);
    NRun.Instance?.SetCurrentRoom((Control) NEventRoom.Create(RunManager.Instance.EventSynchronizer.GetLocalEvent(), runState, this._isPreFinished));
    return Task.CompletedTask;
  }

  public override SerializableRoom ToSerializable()
  {
    SerializableRoom serializable = base.ToSerializable();
    serializable.EventId = this.CanonicalEvent.Id;
    serializable.IsPreFinished = this.IsPreFinished;
    return serializable;
  }

  public void MarkPreFinished() => this._isPreFinished = true;

  private void OnEventStateChanged(EventModel eventModel)
  {
    if (!(eventModel is AncientEventModel))
      return;
    foreach (EventModel eventModel1 in (IEnumerable<EventModel>) RunManager.Instance.EventSynchronizer.Events)
    {
      if (!eventModel1.IsFinished)
        return;
    }
    this.MarkPreFinished();
    TaskHelper.RunSafely(SaveManager.Instance.SaveRun((AbstractRoom) this));
  }
}
