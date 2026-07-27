// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.AutoSlay.Handlers.Screens.DeckTransformScreenHandler
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using MegaCrit.Sts2.Core.AutoSlay.Helpers;
using MegaCrit.Sts2.Core.Nodes.Cards;
using MegaCrit.Sts2.Core.Nodes.Cards.Holders;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.Screens.CardSelection;
using MegaCrit.Sts2.Core.Random;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.AutoSlay.Handlers.Screens;

public class DeckTransformScreenHandler : IScreenHandler, IHandler
{
  public Type ScreenType => typeof (NDeckTransformSelectScreen);

  public TimeSpan Timeout => TimeSpan.FromSeconds(30L);

  public async Task HandleAsync(Rng random, CancellationToken ct)
  {
    AutoSlayLog.EnterScreen("NDeckTransformSelectScreen");
    NDeckTransformSelectScreen screen = AutoSlayer.GetCurrentScreen<NDeckTransformSelectScreen>();
    NCardGrid grid = UiHelper.FindFirst<NCardGrid>((Node) screen);
    NConfirmButton mainConfirmButton;
    if (grid == null)
    {
      AutoSlayLog.Error("Card grid not found in transform screen");
      grid = (NCardGrid) null;
      mainConfirmButton = (NConfirmButton) null;
    }
    else
    {
      Control previewContainer = ((Node) screen).GetNodeOrNull<Control>(NodePath.op_Implicit("%PreviewContainer"));
      mainConfirmButton = ((Node) screen).GetNodeOrNull<NConfirmButton>(NodePath.op_Implicit("Confirm"));
      HashSet<NGridCardHolder> selectedCards = new HashSet<NGridCardHolder>();
      for (int i = 0; i < 10; ++i)
      {
        ct.ThrowIfCancellationRequested();
        Control control1 = previewContainer;
        if ((control1 != null ? (((CanvasItem) control1).Visible ? 1 : 0) : 0) != 0)
        {
          AutoSlayLog.Info("Preview container appeared after selecting cards");
          break;
        }
        if (mainConfirmButton != null && mainConfirmButton.IsEnabled)
        {
          AutoSlayLog.Action("Clicking main confirm button");
          await UiHelper.Click((NClickableControl) mainConfirmButton);
          await Task.Delay(300, ct);
          await WaitHelper.Until((Func<bool>) (() =>
          {
            Control control2 = previewContainer;
            return control2 != null && ((CanvasItem) control2).Visible;
          }), ct, new TimeSpan?(TimeSpan.FromSeconds(5L)), "Preview container did not appear after confirm");
          break;
        }
        List<NGridCardHolder> list = UiHelper.FindAll<NGridCardHolder>((Node) screen).Where<NGridCardHolder>((Func<NGridCardHolder, bool>) (c => !selectedCards.Contains(c))).ToList<NGridCardHolder>();
        if (list.Count == 0)
        {
          AutoSlayLog.Warn("No more cards available to select");
          break;
        }
        NGridCardHolder ngridCardHolder = random.NextItem<NGridCardHolder>((IEnumerable<NGridCardHolder>) list);
        selectedCards.Add(ngridCardHolder);
        AutoSlayLog.Action($"Selecting card to transform ({selectedCards.Count})");
        ((GodotObject) grid).EmitSignal(NCardGrid.SignalName.HolderPressed, new Variant[1]
        {
          Variant.op_Implicit((GodotObject) ngridCardHolder)
        });
        await Task.Delay(300, ct);
      }
      await WaitHelper.Until((Func<bool>) (() =>
      {
        Control control = previewContainer;
        return control != null && ((CanvasItem) control).Visible;
      }), ct, new TimeSpan?(TimeSpan.FromSeconds(5L)), "Preview container did not appear");
      await Task.Delay(500, ct);
      NConfirmButton previewConfirmButton = ((Node) previewContainer)?.GetNodeOrNull<NConfirmButton>(NodePath.op_Implicit("Confirm"));
      if (previewConfirmButton == null)
      {
        AutoSlayLog.Error("Preview confirm button not found");
        grid = (NCardGrid) null;
        mainConfirmButton = (NConfirmButton) null;
      }
      else
      {
        await WaitHelper.Until((Func<bool>) (() => previewConfirmButton.IsEnabled), ct, new TimeSpan?(TimeSpan.FromSeconds(5L)), "Preview confirm button did not become enabled");
        AutoSlayLog.Action("Confirming transform");
        await UiHelper.Click((NClickableControl) previewConfirmButton);
        await WaitHelper.Until((Func<bool>) (() => !GodotObject.IsInstanceValid((GodotObject) screen) || !((CanvasItem) screen).IsVisibleInTree()), ct, new TimeSpan?(TimeSpan.FromSeconds(10L)), "Transform screen did not close after confirmation");
        AutoSlayLog.ExitScreen("NDeckTransformSelectScreen");
        grid = (NCardGrid) null;
        mainConfirmButton = (NConfirmButton) null;
      }
    }
  }
}
