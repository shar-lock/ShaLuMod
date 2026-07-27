// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.AutoSlay.Handlers.Rooms.TreasureRoomHandler
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using MegaCrit.Sts2.Core.AutoSlay.Helpers;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Screens.TreasureRoomRelic;
using MegaCrit.Sts2.Core.Random;
using MegaCrit.Sts2.Core.Rooms;
using System;
using System.Threading;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.AutoSlay.Handlers.Rooms;

public class TreasureRoomHandler : IRoomHandler, IHandler
{
  private const string _roomPath = "/root/Game/RootSceneContainer/Run/RoomContainer/TreasureRoom";

  public RoomType[] HandledTypes
  {
    get => new RoomType[1]{ RoomType.Treasure };
  }

  public TimeSpan Timeout => TimeSpan.FromSeconds(30L);

  public async Task HandleAsync(Rng random, CancellationToken ct)
  {
    AutoSlayLog.Action("Waiting for treasure room");
    NTreasureRoom room = await WaitHelper.ForNode<NTreasureRoom>((Node) ((SceneTree) Engine.GetMainLoop()).Root, "/root/Game/RootSceneContainer/Run/RoomContainer/TreasureRoom", ct);
    NClickableControl node = ((Node) room).GetNode<NClickableControl>(NodePath.op_Implicit("Chest"));
    AutoSlayLog.Action("Opening chest");
    await UiHelper.Click(node);
    await Task.Delay(1000, ct);
    foreach (NTreasureRoomRelicHolder button in UiHelper.FindAll<NTreasureRoomRelicHolder>((Node) room))
    {
      if (button.IsEnabled && ((CanvasItem) button).Visible)
      {
        AutoSlayLog.Action("Picking up relic");
        await UiHelper.Click((NClickableControl) button);
        await Task.Delay(500, ct);
      }
    }
    NProceedButton proceedButton = room.ProceedButton;
    await WaitHelper.Until((Func<bool>) (() => proceedButton.IsEnabled), ct, new TimeSpan?(TimeSpan.FromSeconds(5L)), "Proceed button not enabled after picking relics");
    AutoSlayLog.Action("Clicking proceed");
    await UiHelper.Click((NClickableControl) proceedButton);
    room = (NTreasureRoom) null;
  }
}
