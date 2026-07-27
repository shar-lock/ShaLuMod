// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.AutoSlay.Handlers.Rooms.ShopRoomHandler
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using MegaCrit.Sts2.Core.AutoSlay.Helpers;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Screens.Shops;
using MegaCrit.Sts2.Core.Random;
using MegaCrit.Sts2.Core.Rooms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.AutoSlay.Handlers.Rooms;

public class ShopRoomHandler : IRoomHandler, IHandler
{
  private const string _roomPath = "/root/Game/RootSceneContainer/Run/RoomContainer/MerchantRoom";

  public RoomType[] HandledTypes
  {
    get => new RoomType[1]{ RoomType.Shop };
  }

  public TimeSpan Timeout => TimeSpan.FromSeconds(120L);

  public async Task HandleAsync(Rng random, CancellationToken ct)
  {
    AutoSlayLog.Action("Waiting for shop room");
    NMerchantRoom room = await WaitHelper.ForNode<NMerchantRoom>((Node) ((SceneTree) Engine.GetMainLoop()).Root, "/root/Game/RootSceneContainer/Run/RoomContainer/MerchantRoom", ct);
    AutoSlayLog.Action("Opening merchant inventory");
    room.OpenInventory();
    await Task.Delay(500, ct);
    int maxAttempts = 50;
    int attempts = 0;
    while (attempts < maxAttempts)
    {
      ct.ThrowIfCancellationRequested();
      ++attempts;
      List<NMerchantSlot> list = room.Inventory.GetAllSlots().Where<NMerchantSlot>((Func<NMerchantSlot, bool>) (slot => !(slot is NMerchantCardRemoval))).Where<NMerchantSlot>((Func<NMerchantSlot, bool>) (slot => slot.Entry.IsStocked && slot.Entry.EnoughGold)).ToList<NMerchantSlot>();
      if (list.Count == 0)
      {
        AutoSlayLog.Action("No more affordable items to buy");
        break;
      }
      NMerchantSlot nmerchantSlot = random.NextItem<NMerchantSlot>((IEnumerable<NMerchantSlot>) list);
      AutoSlayLog.Action($"Buying item (cost: {nmerchantSlot.Entry.Cost})");
      int num = await nmerchantSlot.Entry.OnTryPurchaseWrapper(room.Inventory.Inventory) ? 1 : 0;
      await Task.Delay(300, ct);
    }
    NBackButton first = UiHelper.FindFirst<NBackButton>((Node) room);
    if (first != null)
    {
      AutoSlayLog.Action("Closing inventory");
      await UiHelper.Click((NClickableControl) first);
      await Task.Delay(300, ct);
    }
    NProceedButton proceedButton = room.ProceedButton;
    AutoSlayLog.Action("Clicking proceed");
    await UiHelper.Click((NClickableControl) proceedButton);
    room = (NMerchantRoom) null;
  }
}
