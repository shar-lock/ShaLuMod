// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.AutoSlay.Handlers.Screens.SimpleCardSelectScreenHandler
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

public class SimpleCardSelectScreenHandler : IScreenHandler, IHandler
{
  public Type ScreenType => typeof (NSimpleCardSelectScreen);

  public TimeSpan Timeout => TimeSpan.FromSeconds(30L);

  public async Task HandleAsync(Rng random, CancellationToken ct)
  {
    AutoSlayLog.EnterScreen("NSimpleCardSelectScreen");
    NSimpleCardSelectScreen screen = AutoSlayer.GetCurrentScreen<NSimpleCardSelectScreen>();
    NCardGrid grid = UiHelper.FindFirst<NCardGrid>((Node) screen);
    NConfirmButton confirmButton;
    if (grid == null)
    {
      AutoSlayLog.Error("Card grid not found in simple card select screen");
      grid = (NCardGrid) null;
      confirmButton = (NConfirmButton) null;
    }
    else
    {
      confirmButton = ((Node) screen).GetNodeOrNull<NConfirmButton>(NodePath.op_Implicit("%Confirm"));
      HashSet<NGridCardHolder> selectedCards = new HashSet<NGridCardHolder>();
      for (int i = 0; i < 10; ++i)
      {
        ct.ThrowIfCancellationRequested();
        if (!GodotObject.IsInstanceValid((GodotObject) screen) || !((CanvasItem) screen).IsVisibleInTree())
        {
          AutoSlayLog.Info("Screen auto-closed after selections");
          break;
        }
        if (confirmButton != null && confirmButton.IsEnabled)
        {
          AutoSlayLog.Action("Clicking confirm button");
          await UiHelper.Click((NClickableControl) confirmButton);
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
        AutoSlayLog.Action($"Selecting card ({selectedCards.Count})");
        ((GodotObject) grid).EmitSignal(NCardGrid.SignalName.HolderPressed, new Variant[1]
        {
          Variant.op_Implicit((GodotObject) ngridCardHolder)
        });
        await Task.Delay(300, ct);
      }
      await WaitHelper.Until((Func<bool>) (() => !GodotObject.IsInstanceValid((GodotObject) screen) || !((CanvasItem) screen).IsVisibleInTree()), ct, new TimeSpan?(TimeSpan.FromSeconds(5L)), "Simple card select screen did not close");
      AutoSlayLog.ExitScreen("NSimpleCardSelectScreen");
      grid = (NCardGrid) null;
      confirmButton = (NConfirmButton) null;
    }
  }
}
