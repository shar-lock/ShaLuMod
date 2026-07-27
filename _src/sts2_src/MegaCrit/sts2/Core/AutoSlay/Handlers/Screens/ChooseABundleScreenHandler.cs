// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.AutoSlay.Handlers.Screens.ChooseABundleScreenHandler
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using MegaCrit.Sts2.Core.AutoSlay.Helpers;
using MegaCrit.Sts2.Core.Nodes.Cards;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.Screens.CardSelection;
using MegaCrit.Sts2.Core.Random;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.AutoSlay.Handlers.Screens;

public class ChooseABundleScreenHandler : IScreenHandler, IHandler
{
  public Type ScreenType => typeof (NChooseABundleSelectionScreen);

  public TimeSpan Timeout => TimeSpan.FromSeconds(30L);

  public async Task HandleAsync(Rng random, CancellationToken ct)
  {
    AutoSlayLog.EnterScreen("NChooseABundleSelectionScreen");
    NChooseABundleSelectionScreen screen = AutoSlayer.GetCurrentScreen<NChooseABundleSelectionScreen>();
    List<NCardBundle> all = UiHelper.FindAll<NCardBundle>((Node) screen);
    if (all.Count == 0)
    {
      AutoSlayLog.Warn("No bundles found in bundle selection screen");
    }
    else
    {
      NCardBundle ncardBundle = random.NextItem<NCardBundle>((IEnumerable<NCardBundle>) all);
      AutoSlayLog.Action("Selecting card bundle");
      await UiHelper.Click(ncardBundle.Hitbox);
      await Task.Delay(500, ct);
      NConfirmButton first = UiHelper.FindFirst<NConfirmButton>((Node) screen);
      if (first != null)
      {
        AutoSlayLog.Action("Confirming bundle selection");
        await UiHelper.Click((NClickableControl) first);
        await WaitHelper.Until((Func<bool>) (() => !GodotObject.IsInstanceValid((GodotObject) screen) || !((CanvasItem) screen).IsVisibleInTree()), ct, new TimeSpan?(TimeSpan.FromSeconds(10L)), "Bundle selection screen did not close after confirmation");
      }
      AutoSlayLog.ExitScreen("NChooseABundleSelectionScreen");
    }
  }
}
