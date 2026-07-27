// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.AutoSlay.Handlers.Screens.MapScreenHandler
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using MegaCrit.Sts2.Core.AutoSlay.Helpers;
using MegaCrit.Sts2.Core.Map;
using MegaCrit.Sts2.Core.Nodes;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.Screens.Map;
using MegaCrit.Sts2.Core.Random;
using MegaCrit.Sts2.Core.Runs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.AutoSlay.Handlers.Screens;

public class MapScreenHandler : IScreenHandler, IHandler
{
  private TaskCompletionSource? _roomEnteredTcs;

  public Type ScreenType => typeof (NMapScreen);

  public TimeSpan Timeout => TimeSpan.FromSeconds(30L);

  public async Task HandleAsync(Rng random, CancellationToken ct)
  {
    AutoSlayLog.EnterScreen("NMapScreen");
    NRun runNode = ((Node) ((SceneTree) Engine.GetMainLoop()).Root).GetNode<NRun>(NodePath.op_Implicit("/root/Game/RootSceneContainer/Run"));
    await WaitHelper.Until((Func<bool>) (() => ((CanvasItem) runNode.GlobalUi.MapScreen).IsVisibleInTree()), ct, new TimeSpan?(AutoSlayConfig.mapScreenTimeout), "Map screen not visible");
    NMapScreen mapScreen = runNode.GlobalUi.MapScreen;
    NMapPoint nextRoom = (NMapPoint) null;
    try
    {
      await WaitHelper.Until((Func<bool>) (() =>
      {
        NMapPoint nmapPoint = nextRoom = MapScreenHandler.SelectNextRoom(mapScreen);
        return nmapPoint != null && nmapPoint.IsEnabled;
      }), ct, new TimeSpan?(AutoSlayConfig.mapPointEnabledTimeout), "Map point not enabled");
    }
    catch (AutoSlayTimeoutException ex)
    {
      AutoSlayLog.Info(string.Concat("[AutoSlay] Map point never became travelable: node=", nextRoom == null ? "none" : nextRoom.Point.coord.ToString(), ", state=", nextRoom == null ? "n/a" : nextRoom.State.ToString(), ", ", $"screenTravelEnabled={mapScreen.IsTravelEnabled}"));
      throw;
    }
    AutoSlayLog.Action($"Selecting room at ({nextRoom.Point.coord.row}, {nextRoom.Point.coord.col})");
    this._roomEnteredTcs = new TaskCompletionSource((TaskCreationOptions) 64 /*0x40*/);
    RunManager.Instance.RoomEntered += new Action(this.OnRoomEntered);
    try
    {
      await UiHelper.Click((NClickableControl) nextRoom);
      await WaitHelper.ForTask(this._roomEnteredTcs.Task, ct, new TimeSpan?(AutoSlayConfig.mapScreenTimeout), "Room not entered after map click");
    }
    finally
    {
      RunManager.Instance.RoomEntered -= new Action(this.OnRoomEntered);
      this._roomEnteredTcs = (TaskCompletionSource) null;
    }
    AutoSlayLog.ExitScreen("NMapScreen");
  }

  private static NMapPoint? SelectNextRoom(NMapScreen mapScreen)
  {
    List<NMapPoint> all = UiHelper.FindAll<NMapPoint>((Node) mapScreen);
    RunState state = RunManager.Instance.DebugOnlyGetState();
    if (state.VisitedMapCoords.Count == 0)
      return all.FirstOrDefault<NMapPoint>((Func<NMapPoint, bool>) (mp => mp.Point.coord.row == 0));
    IReadOnlyList<MapCoord> visitedMapCoords = state.VisitedMapCoords;
    MapCoord lastCoord = visitedMapCoords[visitedMapCoords.Count - 1];
    NMapPoint nmapPoint = all.FirstOrDefault<NMapPoint>((Func<NMapPoint, bool>) (mp => mp.Point.coord.Equals(lastCoord)));
    MapPoint child = nmapPoint != null ? nmapPoint.Point.Children.FirstOrDefault<MapPoint>() : (MapPoint) null;
    return child != null ? all.FirstOrDefault<NMapPoint>((Func<NMapPoint, bool>) (mp => mp.Point.coord.Equals(child.coord))) : (NMapPoint) null;
  }

  private void OnRoomEntered() => this._roomEnteredTcs?.TrySetResult();
}
