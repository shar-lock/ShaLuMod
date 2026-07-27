// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.AutoSlay.Handlers.Screens.DeckEnchantScreenHandler
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
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.AutoSlay.Handlers.Screens;

public class DeckEnchantScreenHandler : IScreenHandler, IHandler
{
  public Type ScreenType => typeof (NDeckEnchantSelectScreen);

  public TimeSpan Timeout => TimeSpan.FromSeconds(30L);

  public async Task HandleAsync(Rng random, CancellationToken ct)
  {
    AutoSlayLog.EnterScreen("NDeckEnchantSelectScreen");
    NDeckEnchantSelectScreen screen = AutoSlayer.GetCurrentScreen<NDeckEnchantSelectScreen>();
    List<NGridCardHolder> cards = new List<NGridCardHolder>();
    await WaitHelper.Until((Func<bool>) (() =>
    {
      cards = UiHelper.FindAll<NGridCardHolder>((Node) screen);
      return cards.Count > 0;
    }), ct, new TimeSpan?(TimeSpan.FromSeconds(5L)), "No cards found in enchant screen");
    await Task.Delay(300, ct);
    int maxSelections = Math.Min(cards.Count, 5);
    List<NGridCardHolder> selectedCards = new List<NGridCardHolder>();
    Control singlePreviewContainer;
    Control multiPreviewContainer;
    for (int i = 0; i < maxSelections; ++i)
    {
      singlePreviewContainer = ((Node) screen).GetNodeOrNull<Control>(NodePath.op_Implicit("%EnchantSinglePreviewContainer"));
      multiPreviewContainer = ((Node) screen).GetNodeOrNull<Control>(NodePath.op_Implicit("%EnchantMultiPreviewContainer"));
      NConfirmButton nodeOrNull = ((Node) screen).GetNodeOrNull<NConfirmButton>(NodePath.op_Implicit("Confirm"));
      Control control1 = singlePreviewContainer;
      if ((control1 != null ? (((CanvasItem) control1).Visible ? 1 : 0) : 0) == 0)
      {
        Control control2 = multiPreviewContainer;
        if ((control2 != null ? (((CanvasItem) control2).Visible ? 1 : 0) : 0) == 0 && (nodeOrNull == null || !nodeOrNull.IsEnabled))
        {
          List<NGridCardHolder> list = cards.Where<NGridCardHolder>((Func<NGridCardHolder, bool>) (c => !selectedCards.Contains(c))).ToList<NGridCardHolder>();
          if (list.Count != 0)
          {
            NGridCardHolder ngridCardHolder = random.NextItem<NGridCardHolder>((IEnumerable<NGridCardHolder>) list);
            selectedCards.Add(ngridCardHolder);
            AutoSlayLog.Action($"Selecting card to enchant ({cards.Count} cards available)");
            ((GodotObject) ngridCardHolder).EmitSignal(NCardHolder.SignalName.Pressed, new Variant[1]
            {
              Variant.op_Implicit((GodotObject) ngridCardHolder)
            });
            await Task.Delay(200, ct);
          }
          else
            break;
        }
        else
          break;
      }
      else
        break;
    }
    singlePreviewContainer = ((Node) screen).GetNodeOrNull<Control>(NodePath.op_Implicit("%EnchantSinglePreviewContainer"));
    multiPreviewContainer = ((Node) screen).GetNodeOrNull<Control>(NodePath.op_Implicit("%EnchantMultiPreviewContainer"));
    NConfirmButton nodeOrNull1 = ((Node) screen).GetNodeOrNull<NConfirmButton>(NodePath.op_Implicit("Confirm"));
    Control control3 = singlePreviewContainer;
    if ((control3 != null ? (!((CanvasItem) control3).Visible ? 1 : 0) : 1) != 0)
    {
      Control control4 = multiPreviewContainer;
      if ((control4 != null ? (!((CanvasItem) control4).Visible ? 1 : 0) : 1) != 0 && nodeOrNull1 != null && nodeOrNull1.IsEnabled)
      {
        AutoSlayLog.Action("Clicking main confirm button to show preview");
        await UiHelper.Click((NClickableControl) nodeOrNull1);
        await Task.Delay(200, ct);
      }
    }
    Control visiblePreviewContainer = (Control) null;
    await WaitHelper.Until((Func<bool>) (() =>
    {
      singlePreviewContainer = ((Node) screen).GetNodeOrNull<Control>(NodePath.op_Implicit("%EnchantSinglePreviewContainer"));
      multiPreviewContainer = ((Node) screen).GetNodeOrNull<Control>(NodePath.op_Implicit("%EnchantMultiPreviewContainer"));
      Control control5 = singlePreviewContainer;
      if ((control5 != null ? (((CanvasItem) control5).Visible ? 1 : 0) : 0) != 0)
      {
        visiblePreviewContainer = singlePreviewContainer;
        return true;
      }
      Control control6 = multiPreviewContainer;
      if ((control6 != null ? (((CanvasItem) control6).Visible ? 1 : 0) : 0) == 0)
        return false;
      visiblePreviewContainer = multiPreviewContainer;
      return true;
    }), ct, new TimeSpan?(TimeSpan.FromSeconds(5L)), "Enchant preview container did not appear");
    await Task.Delay(500, ct);
    NConfirmButton confirmButton = ((Node) visiblePreviewContainer)?.GetNodeOrNull<NConfirmButton>(NodePath.op_Implicit("Confirm"));
    if (confirmButton == null)
      confirmButton = UiHelper.FindAll<NConfirmButton>((Node) screen).FirstOrDefault<NConfirmButton>((Func<NConfirmButton, bool>) (b => b.IsEnabled));
    if (confirmButton != null)
    {
      await WaitHelper.Until((Func<bool>) (() => confirmButton.IsEnabled), ct, new TimeSpan?(TimeSpan.FromSeconds(5L)), "Enchant confirm button did not become enabled");
      AutoSlayLog.Action("Confirming enchant");
      await UiHelper.Click((NClickableControl) confirmButton);
      await WaitHelper.Until((Func<bool>) (() => !GodotObject.IsInstanceValid((GodotObject) screen) || !((CanvasItem) screen).IsVisibleInTree()), ct, new TimeSpan?(TimeSpan.FromSeconds(10L)), "Enchant screen did not close after confirmation");
    }
    else
      AutoSlayLog.Error("No confirm button found on enchant screen");
    AutoSlayLog.ExitScreen("NDeckEnchantSelectScreen");
  }
}
