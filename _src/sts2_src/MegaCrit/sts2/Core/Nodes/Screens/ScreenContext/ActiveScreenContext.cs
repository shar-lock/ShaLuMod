// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Screens.ScreenContext.ActiveScreenContext
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Nodes.Events;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Screens.Capstones;
using MegaCrit.Sts2.Core.Nodes.Screens.MainMenu;
using MegaCrit.Sts2.Core.Nodes.Screens.Map;
using MegaCrit.Sts2.Core.Nodes.Screens.Overlays;
using MegaCrit.Sts2.Core.Nodes.Screens.Timeline;
using System;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Screens.ScreenContext;

public class ActiveScreenContext
{
  private static ActiveScreenContext? _instance;

  public event Action? Updated;

  public void Update()
  {
    Action updated = this.Updated;
    if (updated == null)
      return;
    updated();
  }

  public static ActiveScreenContext Instance
  {
    get
    {
      return ActiveScreenContext._instance ?? (ActiveScreenContext._instance = new ActiveScreenContext());
    }
  }

  public IScreenContext? GetCurrentScreen()
  {
    if (NGame.Instance?.FeedbackScreen != null && ((CanvasItem) NGame.Instance.FeedbackScreen).Visible)
      return (IScreenContext) NGame.Instance.FeedbackScreen;
    if (NModalContainer.Instance?.OpenModal != null)
      return NModalContainer.Instance.OpenModal;
    if (NGame.Instance?.InspectCardScreen != null && ((CanvasItem) NGame.Instance.InspectCardScreen).Visible)
      return (IScreenContext) NGame.Instance.InspectCardScreen;
    if (NGame.Instance?.InspectRelicScreen != null && ((CanvasItem) NGame.Instance.InspectRelicScreen).Visible)
      return (IScreenContext) NGame.Instance.InspectRelicScreen;
    if (NGame.Instance?.LogoAnimation != null)
      return (IScreenContext) NGame.Instance.LogoAnimation;
    if (NGame.Instance?.MainMenu != null)
    {
      NMainMenu mainMenu = NGame.Instance.MainMenu;
      if (mainMenu.PatchNotesScreen.IsOpen)
        return (IScreenContext) mainMenu.PatchNotesScreen;
      NMainMenuSubmenuStack submenuStack = mainMenu.SubmenuStack;
      if (!submenuStack.SubmenusOpen)
        return (IScreenContext) NGame.Instance.MainMenu;
      return submenuStack.Peek() is NTimelineScreen ntimelineScreen && ntimelineScreen.CurrentUnlockScreen != null ? (IScreenContext) ntimelineScreen.CurrentUnlockScreen : (IScreenContext) submenuStack.Peek();
    }
    if (NRun.Instance != null)
    {
      NRun instance = NRun.Instance;
      if (NCapstoneContainer.Instance.CurrentCapstoneScreen != null)
        return (IScreenContext) NCapstoneContainer.Instance.CurrentCapstoneScreen;
      if (NMapScreen.Instance.IsOpen)
        return (IScreenContext) NMapScreen.Instance;
      if (NOverlayStack.Instance.ScreenCount > 0)
        return (IScreenContext) NOverlayStack.Instance.Peek();
      if (instance.EventRoom != null)
      {
        if (instance.EventRoom.CustomEventNode != null)
          return instance.EventRoom.CustomEventNode.CurrentScreenContext;
        if (!(instance.EventRoom.Layout is NCombatEventLayout layout))
          return (IScreenContext) instance.EventRoom;
        return !layout.HasCombatStarted ? (IScreenContext) instance.EventRoom : (IScreenContext) instance.EventRoom.EmbeddedCombatRoom;
      }
      if (instance.CombatRoom != null)
        return (IScreenContext) instance.CombatRoom;
      if (instance.TreasureRoom != null)
        return (IScreenContext) instance.TreasureRoom;
      if (instance.RestSiteRoom != null)
        return (IScreenContext) instance.RestSiteRoom;
      if (instance.MapRoom != null)
        return (IScreenContext) instance.MapRoom;
      if (instance.MerchantRoom != null)
      {
        NMerchantRoom merchantRoom = instance.MerchantRoom;
        return merchantRoom.Inventory.IsOpen ? (IScreenContext) merchantRoom.Inventory : (IScreenContext) merchantRoom;
      }
    }
    return (IScreenContext) null;
  }

  public bool IsCurrent(IScreenContext screen) => screen == this.GetCurrentScreen();

  public void FocusOnDefaultControl()
  {
    Control defaultFocusedControl = this.GetCurrentScreen()?.DefaultFocusedControl;
    if (defaultFocusedControl != null)
      defaultFocusedControl.TryGrabFocus();
    else
      ((Node) NGame.Instance).GetViewport()?.GuiReleaseFocus();
  }
}
