// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.AutoSlay.Handlers.Screens.RewardsScreenHandler
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using MegaCrit.Sts2.Core.AutoSlay.Helpers;
using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.Rewards;
using MegaCrit.Sts2.Core.Nodes.Screens;
using MegaCrit.Sts2.Core.Nodes.Screens.Map;
using MegaCrit.Sts2.Core.Nodes.Screens.Overlays;
using MegaCrit.Sts2.Core.Random;
using MegaCrit.Sts2.Core.Rewards;
using MegaCrit.Sts2.Core.Runs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.AutoSlay.Handlers.Screens;

public class RewardsScreenHandler : IScreenHandler, IHandler
{
  public Type ScreenType => typeof (NRewardsScreen);

  public TimeSpan Timeout => TimeSpan.FromSeconds(30L);

  public async Task HandleAsync(Rng random, CancellationToken ct)
  {
    AutoSlayLog.EnterScreen("NRewardsScreen");
    NRewardsScreen screen = AutoSlayer.GetCurrentScreen<NRewardsScreen>();
    HashSet<NRewardButton> attemptedButtons = new HashSet<NRewardButton>();
    while (true)
    {
      ct.ThrowIfCancellationRequested();
      Player me = LocalContext.GetMe((IPlayerCollection) RunManager.Instance.DebugOnlyGetState());
      bool hasPotionSlots = me != null && me.HasOpenPotionSlots;
      NRewardButton button = UiHelper.FindAll<NRewardButton>((Node) screen).FirstOrDefault<NRewardButton>((Func<NRewardButton, bool>) (b => b.IsEnabled && !attemptedButtons.Contains(b) && !(b.Reward is PotionReward) | hasPotionSlots));
      if (button != null)
      {
        attemptedButtons.Add(button);
        AutoSlayLog.Action("Clicking reward button: " + (button.Reward?.GetType().Name ?? "unknown"));
        await UiHelper.Click((NClickableControl) button);
        await Task.Delay(500, ct);
        IOverlayScreen overlayScreen = NOverlayStack.Instance?.Peek();
        if (overlayScreen == null || overlayScreen == screen)
          ;
        else
          break;
      }
      else
        goto label_7;
    }
    AutoSlayLog.Action("Child screen opened, returning to drain loop");
    AutoSlayLog.ExitScreen("NRewardsScreen");
    return;
label_7:
    NProceedButton first = UiHelper.FindFirst<NProceedButton>((Node) screen);
    if (first != null)
    {
      AutoSlayLog.Action("Clicking proceed");
      await UiHelper.Click((NClickableControl) first);
      await WaitHelper.Until((Func<bool>) (() =>
      {
        if (!GodotObject.IsInstanceValid((GodotObject) screen) || NOverlayStack.Instance?.Peek() != screen)
          return true;
        NMapScreen instance = NMapScreen.Instance;
        return instance != null && instance.IsOpen;
      }), ct, new TimeSpan?(TimeSpan.FromSeconds(10L)), "Rewards screen did not close or map did not open after clicking proceed");
    }
    AutoSlayLog.ExitScreen("NRewardsScreen");
  }
}
