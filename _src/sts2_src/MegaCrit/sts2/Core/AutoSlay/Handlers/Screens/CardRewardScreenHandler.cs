// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.AutoSlay.Handlers.Screens.CardRewardScreenHandler
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using MegaCrit.Sts2.Core.AutoSlay.Helpers;
using MegaCrit.Sts2.Core.Nodes.Cards.Holders;
using MegaCrit.Sts2.Core.Nodes.Screens.CardSelection;
using MegaCrit.Sts2.Core.Random;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.AutoSlay.Handlers.Screens;

public class CardRewardScreenHandler : IScreenHandler, IHandler
{
  public Type ScreenType => typeof (NCardRewardSelectionScreen);

  public TimeSpan Timeout => TimeSpan.FromSeconds(30L);

  public async Task HandleAsync(Rng random, CancellationToken ct)
  {
    AutoSlayLog.EnterScreen("NCardRewardSelectionScreen");
    NCardRewardSelectionScreen screen = AutoSlayer.GetCurrentScreen<NCardRewardSelectionScreen>();
    await Task.Delay(400, ct);
    List<NCardHolder> all = UiHelper.FindAll<NCardHolder>((Node) screen);
    if (all.Count == 0)
    {
      AutoSlayLog.Warn("No card holders found in card reward screen");
    }
    else
    {
      NCardHolder ncardHolder = random.NextItem<NCardHolder>((IEnumerable<NCardHolder>) all);
      AutoSlayLog.Action("Selecting card reward");
      ((GodotObject) ncardHolder).EmitSignal(NCardHolder.SignalName.Pressed, new Variant[1]
      {
        Variant.op_Implicit((GodotObject) ncardHolder)
      });
      await WaitHelper.Until((Func<bool>) (() => !GodotObject.IsInstanceValid((GodotObject) screen) || !((CanvasItem) screen).IsVisibleInTree()), ct, new TimeSpan?(TimeSpan.FromSeconds(10L)), "Card reward screen did not close after selection");
      AutoSlayLog.ExitScreen("NCardRewardSelectionScreen");
    }
  }
}
