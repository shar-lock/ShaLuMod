// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.AutoSlay.Handlers.Rooms.RestSiteRoomHandler
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using MegaCrit.Sts2.Core.AutoSlay.Helpers;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.RestSite;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Screens.Overlays;
using MegaCrit.Sts2.Core.Random;
using MegaCrit.Sts2.Core.Rooms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.AutoSlay.Handlers.Rooms;

public class RestSiteRoomHandler : IRoomHandler, IHandler
{
  private const string _roomPath = "/root/Game/RootSceneContainer/Run/RoomContainer/RestSiteRoom";

  public RoomType[] HandledTypes
  {
    get => new RoomType[1]{ RoomType.RestSite };
  }

  public TimeSpan Timeout => TimeSpan.FromSeconds(30L);

  public async Task HandleAsync(Rng random, CancellationToken ct)
  {
    AutoSlayLog.Action("Waiting for rest site room");
    NRestSiteRoom room = await WaitHelper.ForNode<NRestSiteRoom>((Node) ((SceneTree) Engine.GetMainLoop()).Root, "/root/Game/RootSceneContainer/Run/RoomContainer/RestSiteRoom", ct);
    List<NRestSiteButton> list = UiHelper.FindAll<NRestSiteButton>((Node) room).Where<NRestSiteButton>((Func<NRestSiteButton, bool>) (b => b.Option.IsEnabled)).ToList<NRestSiteButton>();
    if (list.Count == 0)
    {
      AutoSlayLog.Warn("No clickable rest site buttons found");
      room = (NRestSiteRoom) null;
    }
    else
    {
      NRestSiteButton button = random.NextItem<NRestSiteButton>((IEnumerable<NRestSiteButton>) list);
      AutoSlayLog.Action("Selecting rest site option: " + button.Option.GetType().Name);
      await UiHelper.Click((NClickableControl) button);
      NProceedButton proceedButton = room.ProceedButton;
      await WaitHelper.Until((Func<bool>) (() =>
      {
        if (proceedButton.IsEnabled)
          return true;
        NOverlayStack instance = NOverlayStack.Instance;
        return instance != null && instance.ScreenCount > 0;
      }), ct, new TimeSpan?(TimeSpan.FromSeconds(10L)), "Rest site option did not respond");
      NOverlayStack instance1 = NOverlayStack.Instance;
      if ((instance1 != null ? (instance1.ScreenCount > 0 ? 1 : 0) : 0) != 0)
      {
        AutoSlayLog.Action("Overlay screen detected, deferring proceed to drain loop");
        room = (NRestSiteRoom) null;
      }
      else
      {
        AutoSlayLog.Action("Clicking proceed");
        await UiHelper.Click((NClickableControl) proceedButton);
        room = (NRestSiteRoom) null;
      }
    }
  }
}
