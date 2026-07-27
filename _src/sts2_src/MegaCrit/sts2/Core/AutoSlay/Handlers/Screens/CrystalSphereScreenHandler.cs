// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.AutoSlay.Handlers.Screens.CrystalSphereScreenHandler
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using MegaCrit.Sts2.Core.AutoSlay.Helpers;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Nodes.Events.Custom.CrystalSphere;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.Screens.Map;
using MegaCrit.Sts2.Core.Nodes.Screens.Overlays;
using MegaCrit.Sts2.Core.Random;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.AutoSlay.Handlers.Screens;

public class CrystalSphereScreenHandler : IScreenHandler, IHandler
{
  public Type ScreenType => typeof (NCrystalSphereScreen);

  public TimeSpan Timeout => TimeSpan.FromSeconds(120L);

  public async Task HandleAsync(Rng random, CancellationToken ct)
  {
    AutoSlayLog.EnterScreen("NCrystalSphereScreen");
    NCrystalSphereScreen screen = AutoSlayer.GetCurrentScreen<NCrystalSphereScreen>();
    await Task.Delay(1000, ct);
    NProceedButton proceedButton = ((Node) screen).GetNodeOrNull<NProceedButton>(NodePath.op_Implicit("%ProceedButton"));
    NProceedButton nproceedButton1 = proceedButton;
    if ((nproceedButton1 != null ? (nproceedButton1.IsEnabled ? 1 : 0) : 0) != 0)
    {
      AutoSlayLog.Action("Clicking Crystal Sphere proceed button");
      await UiHelper.Click((NClickableControl) proceedButton);
      await WaitHelper.Until((Func<bool>) (() =>
      {
        if (!GodotObject.IsInstanceValid((GodotObject) screen) || !((CanvasItem) screen).IsVisibleInTree())
          return true;
        NMapScreen instance = NMapScreen.Instance;
        return instance != null && ((CanvasItem) instance).IsVisibleInTree();
      }), ct, new TimeSpan?(TimeSpan.FromSeconds(10L)), "Crystal Sphere screen did not close after clicking proceed");
      if (GodotObject.IsInstanceValid((GodotObject) screen) && ((CanvasItem) screen).IsVisibleInTree())
      {
        NMapScreen instance = NMapScreen.Instance;
        if (instance != null && ((CanvasItem) instance).IsVisibleInTree())
        {
          AutoSlayLog.Info("Map opened, manually removing Crystal Sphere screen from overlay stack");
          NOverlayStack.Instance?.Remove((IOverlayScreen) screen);
          await Task.Delay(100, ct);
        }
      }
      AutoSlayLog.ExitScreen("NCrystalSphereScreen");
    }
    else
    {
      int maxClicks = 20;
      int clicks = 0;
      int lastClickableCount = int.MaxValue;
      int noProgressCount = 0;
      while (clicks < maxClicks)
      {
        ct.ThrowIfCancellationRequested();
        if (GodotObject.IsInstanceValid((GodotObject) screen) && ((CanvasItem) screen).IsVisibleInTree())
        {
          IOverlayScreen overlayScreen = NOverlayStack.Instance?.Peek();
          if (overlayScreen != null && overlayScreen != screen)
          {
            AutoSlayLog.Info("Child screen appeared (rewards), returning to drain loop");
            AutoSlayLog.ExitScreen("NCrystalSphereScreen");
            return;
          }
          proceedButton = ((Node) screen).GetNodeOrNull<NProceedButton>(NodePath.op_Implicit("%ProceedButton"));
          NProceedButton nproceedButton2 = proceedButton;
          if ((nproceedButton2 != null ? (nproceedButton2.IsEnabled ? 1 : 0) : 0) == 0)
          {
            Control nodeOrNull = ((Node) screen).GetNodeOrNull<Control>(NodePath.op_Implicit("%Cells"));
            if (nodeOrNull == null)
            {
              await Task.Delay(100, ct);
            }
            else
            {
              List<NCrystalSphereCell> list = UiHelper.FindAll<NCrystalSphereCell>((Node) nodeOrNull).Where<NCrystalSphereCell>((Func<NCrystalSphereCell, bool>) (c => ((CanvasItem) c).Visible && c.Entity.IsHidden)).ToList<NCrystalSphereCell>();
              if (list.Count == 0)
              {
                AutoSlayLog.Info("No more clickable cells, waiting for rewards or proceed");
                break;
              }
              if (list.Count >= lastClickableCount)
              {
                ++noProgressCount;
                if (noProgressCount > 5)
                {
                  AutoSlayLog.Info($"No progress after {noProgressCount} clicks, divinations likely exhausted");
                  break;
                }
              }
              else
                noProgressCount = 0;
              lastClickableCount = list.Count;
              NCrystalSphereCell ncrystalSphereCell = random.NextItem<NCrystalSphereCell>((IEnumerable<NCrystalSphereCell>) list);
              AutoSlayLog.Info($"Clicking crystal sphere cell at ({ncrystalSphereCell.Entity.X}, {ncrystalSphereCell.Entity.Y}), click {clicks + 1}, {list.Count} clickable");
              ((GodotObject) ncrystalSphereCell).EmitSignal(NClickableControl.SignalName.Released, new Variant[1]
              {
                Variant.op_Implicit((GodotObject) ncrystalSphereCell)
              });
              await Task.Delay(500, ct);
              ++clicks;
            }
          }
          else
            break;
        }
        else
          break;
      }
      await WaitHelper.Until((Func<bool>) (() =>
      {
        proceedButton = ((Node) screen).GetNodeOrNull<NProceedButton>(NodePath.op_Implicit("%ProceedButton"));
        NProceedButton nproceedButton3 = proceedButton;
        if ((nproceedButton3 != null ? (nproceedButton3.IsEnabled ? 1 : 0) : 0) != 0 || !GodotObject.IsInstanceValid((GodotObject) screen) || !((CanvasItem) screen).IsVisibleInTree())
          return true;
        IOverlayScreen overlayScreen = NOverlayStack.Instance?.Peek();
        return overlayScreen != null && overlayScreen != screen;
      }), ct, new TimeSpan?(TimeSpan.FromSeconds(15L)), "Crystal Sphere: neither proceed button nor rewards screen appeared");
      IOverlayScreen overlayScreen1 = NOverlayStack.Instance?.Peek();
      if (overlayScreen1 != null && overlayScreen1 != screen)
      {
        AutoSlayLog.Info("Rewards screen appeared, returning to drain loop");
        AutoSlayLog.ExitScreen("NCrystalSphereScreen");
      }
      else if (!GodotObject.IsInstanceValid((GodotObject) screen) || !((CanvasItem) screen).IsVisibleInTree())
      {
        AutoSlayLog.ExitScreen("NCrystalSphereScreen");
      }
      else
      {
        proceedButton = ((Node) screen).GetNodeOrNull<NProceedButton>(NodePath.op_Implicit("%ProceedButton"));
        if (proceedButton != null && proceedButton.IsEnabled)
        {
          AutoSlayLog.Action("Clicking Crystal Sphere proceed button");
          await UiHelper.Click((NClickableControl) proceedButton);
          await WaitHelper.Until((Func<bool>) (() =>
          {
            if (!GodotObject.IsInstanceValid((GodotObject) screen) || !((CanvasItem) screen).IsVisibleInTree())
              return true;
            NMapScreen instance = NMapScreen.Instance;
            return instance != null && ((CanvasItem) instance).IsVisibleInTree();
          }), ct, new TimeSpan?(TimeSpan.FromSeconds(10L)), "Crystal Sphere screen did not close after clicking proceed");
          if (GodotObject.IsInstanceValid((GodotObject) screen) && ((CanvasItem) screen).IsVisibleInTree())
          {
            NMapScreen instance = NMapScreen.Instance;
            if (instance != null && ((CanvasItem) instance).IsVisibleInTree())
            {
              AutoSlayLog.Info("Map opened, manually removing Crystal Sphere screen from overlay stack");
              NOverlayStack.Instance?.Remove((IOverlayScreen) screen);
              await Task.Delay(100, ct);
            }
          }
        }
        AutoSlayLog.ExitScreen("NCrystalSphereScreen");
      }
    }
  }
}
