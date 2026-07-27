// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Multiplayer.Game.PeerInput.ScreenStateTracker
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using MegaCrit.Sts2.Core.Entities.Multiplayer;
using MegaCrit.Sts2.Core.Nodes.Screens;
using MegaCrit.Sts2.Core.Nodes.Screens.Capstones;
using MegaCrit.Sts2.Core.Nodes.Screens.Map;
using MegaCrit.Sts2.Core.Nodes.Screens.Overlays;
using MegaCrit.Sts2.Core.Runs;
using System;

#nullable enable
namespace MegaCrit.Sts2.Core.Multiplayer.Game.PeerInput;

public class ScreenStateTracker
{
  private NetScreenType _capstoneScreen;
  private NetScreenType _overlayScreen;
  private bool _mapScreenVisible;
  private bool _isInSharedRelicPicking;
  private readonly Callable _onRewardsScreenCompleted;

  public ScreenStateTracker(
    NMapScreen mapScreen,
    NCapstoneContainer capstoneContainer,
    NOverlayStack overlayStack)
  {
    this._onRewardsScreenCompleted = Callable.From(new Action(this.SyncLocalScreen));
    ((GodotObject) capstoneContainer).Connect(NCapstoneContainer.SignalName.Changed, Callable.From(new Action(this.OnCapstoneScreenChanged)), 0U);
    ((GodotObject) overlayStack).Connect(NOverlayStack.SignalName.Changed, Callable.From(new Action(this.OnOverlayStackChanged)), 0U);
    ((GodotObject) mapScreen).Connect(CanvasItem.SignalName.VisibilityChanged, Callable.From(new Action(this.OnMapScreenVisibilityChanged)), 0U);
  }

  private void OnCapstoneScreenChanged()
  {
    if (RunManager.Instance.IsSingleplayerOrFakeMultiplayer)
      return;
    ICapstoneScreen currentCapstoneScreen = NCapstoneContainer.Instance.CurrentCapstoneScreen;
    this._capstoneScreen = currentCapstoneScreen != null ? currentCapstoneScreen.ScreenType : NetScreenType.None;
    this.SyncLocalScreen();
  }

  private void OnOverlayStackChanged()
  {
    if (RunManager.Instance.IsSingleplayerOrFakeMultiplayer)
      return;
    IOverlayScreen overlayScreen = NOverlayStack.Instance.Peek();
    if (overlayScreen is NRewardsScreen nrewardsScreen && !((GodotObject) nrewardsScreen).IsConnected(NRewardsScreen.SignalName.Completed, this._onRewardsScreenCompleted))
      ((GodotObject) nrewardsScreen).Connect(NRewardsScreen.SignalName.Completed, this._onRewardsScreenCompleted, 0U);
    this._overlayScreen = overlayScreen != null ? overlayScreen.ScreenType : NetScreenType.None;
    this.SyncLocalScreen();
  }

  private void SyncLocalScreen()
  {
    RunManager.Instance.InputSynchronizer.SyncLocalScreen(this.GetCurrentScreen());
  }

  private void OnMapScreenVisibilityChanged()
  {
    this._mapScreenVisible = ((CanvasItem) NMapScreen.Instance).Visible;
    RunManager.Instance.InputSynchronizer.SyncLocalScreen(this.GetCurrentScreen());
  }

  public void SetIsInSharedRelicPickingScreen(bool isInSharedRelicPicking)
  {
    this._isInSharedRelicPicking = isInSharedRelicPicking;
    RunManager.Instance.InputSynchronizer.SyncLocalScreen(this.GetCurrentScreen());
  }

  private NetScreenType GetCurrentScreen()
  {
    if (this._capstoneScreen != NetScreenType.None)
      return this._capstoneScreen;
    if (this._mapScreenVisible)
      return NetScreenType.Map;
    if (this._overlayScreen == NetScreenType.Rewards)
    {
      if (NOverlayStack.Instance.Peek() is NRewardsScreen nrewardsScreen && !nrewardsScreen.IsComplete)
        return this._overlayScreen;
    }
    else if (this._overlayScreen != NetScreenType.None)
      return this._overlayScreen;
    return this._isInSharedRelicPicking ? NetScreenType.SharedRelicPicking : NetScreenType.Room;
  }
}
