// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.AutoSlay.Handlers.Screens.ChooseACardScreenHandler
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

public class ChooseACardScreenHandler : IScreenHandler, IHandler
{
  public Type ScreenType => typeof (NChooseACardSelectionScreen);

  public TimeSpan Timeout => TimeSpan.FromSeconds(30L);

  public async Task HandleAsync(Rng random, CancellationToken ct)
  {
    AutoSlayLog.EnterScreen("NChooseACardSelectionScreen");
    List<NCardHolder> all = UiHelper.FindAll<NCardHolder>((Node) AutoSlayer.GetCurrentScreen<NChooseACardSelectionScreen>());
    if (all.Count == 0)
    {
      AutoSlayLog.Warn("No card holders found in choose-a-card screen");
    }
    else
    {
      NCardHolder ncardHolder = random.NextItem<NCardHolder>((IEnumerable<NCardHolder>) all);
      AutoSlayLog.Action("Selecting card");
      ((GodotObject) ncardHolder).EmitSignal(NCardHolder.SignalName.Pressed, new Variant[1]
      {
        Variant.op_Implicit((GodotObject) ncardHolder)
      });
      await Task.Delay(100, ct);
      AutoSlayLog.ExitScreen("NChooseACardSelectionScreen");
    }
  }
}
