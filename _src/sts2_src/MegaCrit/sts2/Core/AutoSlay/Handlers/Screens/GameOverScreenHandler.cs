// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.AutoSlay.Handlers.Screens.GameOverScreenHandler
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using MegaCrit.Sts2.Core.AutoSlay.Helpers;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.Screens.GameOverScreen;
using MegaCrit.Sts2.Core.Random;
using System;
using System.Threading;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.AutoSlay.Handlers.Screens;

public class GameOverScreenHandler : IScreenHandler, IHandler
{
  public Type ScreenType => typeof (NGameOverScreen);

  public TimeSpan Timeout => TimeSpan.FromMinutes(2L);

  public async Task HandleAsync(Rng random, CancellationToken ct)
  {
    AutoSlayLog.EnterScreen("NGameOverScreen");
    NGameOverScreen screen = AutoSlayer.GetCurrentScreen<NGameOverScreen>();
    NGameOverContinueButton continueButton = UiHelper.FindFirst<NGameOverContinueButton>((Node) screen);
    if (continueButton == null)
    {
      AutoSlayLog.Error("Continue button not found on game over screen");
    }
    else
    {
      await WaitHelper.Until((Func<bool>) (() => continueButton.IsEnabled), ct, new TimeSpan?(TimeSpan.FromSeconds(30L)), "Continue button did not become enabled");
      AutoSlayLog.Action("Clicking continue button");
      await UiHelper.Click((NClickableControl) continueButton);
      NReturnToMainMenuButton mainMenuButton = (NReturnToMainMenuButton) null;
      int waitCycles = 0;
      await WaitHelper.Until((Func<bool>) (() =>
      {
        if (!GodotObject.IsInstanceValid((GodotObject) screen) || !((CanvasItem) screen).IsVisibleInTree())
          return true;
        mainMenuButton = UiHelper.FindFirst<NReturnToMainMenuButton>((Node) screen);
        ++waitCycles;
        if (waitCycles % 20 == 0)
        {
          bool flag1 = mainMenuButton != null;
          NReturnToMainMenuButton toMainMenuButton1 = mainMenuButton;
          bool flag2 = toMainMenuButton1 != null && ((CanvasItem) toMainMenuButton1).Visible;
          NReturnToMainMenuButton toMainMenuButton2 = mainMenuButton;
          bool flag3 = toMainMenuButton2 != null && toMainMenuButton2.IsEnabled;
          AutoSlayLog.Info($"Waiting for main menu button: found={flag1}, visible={flag2}, enabled={flag3}");
          AutoSlayer.CurrentWatchdog?.Reset("Waiting for game over summary animation");
        }
        NReturnToMainMenuButton toMainMenuButton = mainMenuButton;
        return (toMainMenuButton != null ? (((CanvasItem) toMainMenuButton).Visible ? 1 : 0) : 0) != 0 && mainMenuButton.IsEnabled;
      }), ct, new TimeSpan?(TimeSpan.FromSeconds(90L)), "Main menu button did not become enabled");
      if (!GodotObject.IsInstanceValid((GodotObject) screen) || !((CanvasItem) screen).IsVisibleInTree())
      {
        AutoSlayLog.Action("Game over screen closed automatically");
        AutoSlayLog.ExitScreen("NGameOverScreen");
      }
      else
      {
        AutoSlayLog.Action("Clicking main menu button");
        await UiHelper.Click((NClickableControl) mainMenuButton);
        await WaitHelper.Until((Func<bool>) (() => !GodotObject.IsInstanceValid((GodotObject) screen) || !((CanvasItem) screen).IsVisibleInTree()), ct, new TimeSpan?(TimeSpan.FromSeconds(30L)), "Game over screen did not close");
        AutoSlayLog.ExitScreen("NGameOverScreen");
      }
    }
  }
}
