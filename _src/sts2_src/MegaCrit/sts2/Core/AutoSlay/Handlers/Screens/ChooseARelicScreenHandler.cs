// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.AutoSlay.Handlers.Screens.ChooseARelicScreenHandler
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using MegaCrit.Sts2.Core.AutoSlay.Helpers;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.Screens;
using MegaCrit.Sts2.Core.Random;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.AutoSlay.Handlers.Screens;

public class ChooseARelicScreenHandler : IScreenHandler, IHandler
{
  public Type ScreenType => typeof (NChooseARelicSelection);

  public TimeSpan Timeout => TimeSpan.FromSeconds(30L);

  public async Task HandleAsync(Rng random, CancellationToken ct)
  {
    AutoSlayLog.EnterScreen("NChooseARelicSelection");
    List<NClickableControl> all = UiHelper.FindAll<NClickableControl>((Node) AutoSlayer.GetCurrentScreen<NChooseARelicSelection>());
    if (all.Count == 0)
    {
      AutoSlayLog.Warn("No clickable elements found in relic selection screen");
    }
    else
    {
      NClickableControl button = random.NextItem<NClickableControl>((IEnumerable<NClickableControl>) all);
      AutoSlayLog.Action("Selecting relic");
      await UiHelper.Click(button);
      AutoSlayLog.ExitScreen("NChooseARelicSelection");
    }
  }
}
