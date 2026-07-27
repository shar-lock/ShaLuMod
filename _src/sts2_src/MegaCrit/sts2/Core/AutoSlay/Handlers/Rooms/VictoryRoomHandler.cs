// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.AutoSlay.Handlers.Rooms.VictoryRoomHandler
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.AutoSlay.Helpers;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Random;
using System;
using System.Threading;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.AutoSlay.Handlers.Rooms;

public class VictoryRoomHandler : IHandler
{
  public TimeSpan Timeout => TimeSpan.FromSeconds(30L);

  public async Task HandleAsync(Rng random, CancellationToken ct)
  {
    AutoSlayLog.Action("Waiting for victory room");
    await WaitHelper.Until((Func<bool>) (() => NCombatRoom.Instance != null), ct, new TimeSpan?(AutoSlayConfig.nodeWaitTimeout), "Victory room not ready");
    NProceedButton proceedButton = NCombatRoom.Instance.ProceedButton;
    await WaitHelper.Until((Func<bool>) (() => proceedButton.IsEnabled), ct, new TimeSpan?(TimeSpan.FromSeconds(10L)), "Proceed button not enabled");
    AutoSlayLog.Action("Clicking proceed on victory");
    await UiHelper.Click((NClickableControl) proceedButton);
    AutoSlayLog.Action("Victory room completed");
  }
}
