// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.AutoSlay.Handlers.Screens.DeckUpgradeScreenHandler
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using MegaCrit.Sts2.Core.AutoSlay.Helpers;
using MegaCrit.Sts2.Core.Nodes.Cards.Holders;
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

public class DeckUpgradeScreenHandler : IScreenHandler, IHandler
{
  public Type ScreenType => typeof (NDeckUpgradeSelectScreen);

  public TimeSpan Timeout => TimeSpan.FromSeconds(30L);

  public async Task HandleAsync(Rng random, CancellationToken ct)
  {
    AutoSlayLog.EnterScreen("NDeckUpgradeSelectScreen");
    NDeckUpgradeSelectScreen screen = AutoSlayer.GetCurrentScreen<NDeckUpgradeSelectScreen>();
    List<NGridCardHolder> cards = UiHelper.FindAll<NGridCardHolder>((Node) screen);
    if (cards.Count == 0)
    {
      AutoSlayLog.Warn("No cards found in upgrade screen");
      cards = (List<NGridCardHolder>) null;
    }
    else
    {
      int maxSelections = Math.Min(cards.Count, 5);
      for (int i = 0; i < maxSelections && GodotObject.IsInstanceValid((GodotObject) screen) && ((CanvasItem) screen).IsVisibleInTree(); ++i)
      {
        Control nodeOrNull1 = ((Node) screen).GetNodeOrNull<Control>(NodePath.op_Implicit("%UpgradeSinglePreviewContainer"));
        Control nodeOrNull2 = ((Node) screen).GetNodeOrNull<Control>(NodePath.op_Implicit("%UpgradeMultiPreviewContainer"));
        if ((nodeOrNull1 == null || !((CanvasItem) nodeOrNull1).Visible) && (nodeOrNull2 == null || !((CanvasItem) nodeOrNull2).Visible))
        {
          NGridCardHolder ngridCardHolder = random.NextItem<NGridCardHolder>((IEnumerable<NGridCardHolder>) cards);
          AutoSlayLog.Action("Selecting card to upgrade");
          ((GodotObject) ngridCardHolder).EmitSignal(NCardHolder.SignalName.Pressed, new Variant[1]
          {
            Variant.op_Implicit((GodotObject) ngridCardHolder)
          });
          cards.Remove(ngridCardHolder);
          await Task.Delay(300, ct);
        }
        else
          break;
      }
      Control visiblePreview = (Control) null;
      await WaitHelper.Until((Func<bool>) (() =>
      {
        Control nodeOrNull3 = ((Node) screen).GetNodeOrNull<Control>(NodePath.op_Implicit("%UpgradeSinglePreviewContainer"));
        Control nodeOrNull4 = ((Node) screen).GetNodeOrNull<Control>(NodePath.op_Implicit("%UpgradeMultiPreviewContainer"));
        if (nodeOrNull3 != null && ((CanvasItem) nodeOrNull3).Visible)
        {
          visiblePreview = nodeOrNull3;
          return true;
        }
        if (nodeOrNull4 != null && ((CanvasItem) nodeOrNull4).Visible)
        {
          visiblePreview = nodeOrNull4;
          return true;
        }
        return !GodotObject.IsInstanceValid((GodotObject) screen) || !((CanvasItem) screen).IsVisibleInTree();
      }), ct, new TimeSpan?(TimeSpan.FromSeconds(5L)), "Upgrade preview did not appear");
      if (!GodotObject.IsInstanceValid((GodotObject) screen) || !((CanvasItem) screen).IsVisibleInTree())
      {
        AutoSlayLog.ExitScreen("NDeckUpgradeSelectScreen");
        cards = (List<NGridCardHolder>) null;
      }
      else
      {
        NConfirmButton confirmButton = ((Node) visiblePreview)?.GetNodeOrNull<NConfirmButton>(NodePath.op_Implicit("Confirm"));
        if (confirmButton == null)
        {
          AutoSlayLog.Error("Preview confirm button not found");
          AutoSlayLog.ExitScreen("NDeckUpgradeSelectScreen");
          cards = (List<NGridCardHolder>) null;
        }
        else
        {
          await WaitHelper.Until((Func<bool>) (() => confirmButton.IsEnabled), ct, new TimeSpan?(TimeSpan.FromSeconds(5L)), "Upgrade confirm button did not become enabled");
          AutoSlayLog.Action("Confirming upgrade");
          await UiHelper.Click((NClickableControl) confirmButton);
          await WaitHelper.Until((Func<bool>) (() => !GodotObject.IsInstanceValid((GodotObject) screen) || !((CanvasItem) screen).IsVisibleInTree()), ct, new TimeSpan?(TimeSpan.FromSeconds(10L)), "Upgrade screen did not close after confirmation");
          AutoSlayLog.ExitScreen("NDeckUpgradeSelectScreen");
          cards = (List<NGridCardHolder>) null;
        }
      }
    }
  }
}
