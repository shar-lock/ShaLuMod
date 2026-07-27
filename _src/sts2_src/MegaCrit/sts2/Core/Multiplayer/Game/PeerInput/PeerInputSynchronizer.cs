// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Multiplayer.Game.PeerInput.PeerInputSynchronizer
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using MegaCrit.Sts2.Core.Entities.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Multiplayer.Messages.Game.Sync;
using MegaCrit.Sts2.Core.TestSupport;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Multiplayer.Game.PeerInput;

public class PeerInputSynchronizer : IDisposable
{
  public const int minUpdateMsec = 50;
  private readonly INetGameService _netService;
  private readonly List<PeerInputSynchronizer.PeerInputState> _inputStates = new List<PeerInputSynchronizer.PeerInputState>();
  private ulong _lastSyncMsec;
  private Task? _syncMessageTask;
  private PeerInputMessage? _syncMessageToSend;
  private INetCursorPositionTranslator? _cursorTranslator;
  private Logger _logger = new Logger(nameof (PeerInputSynchronizer), LogType.VisualSync);
  public Func<ulong>? mockGetTicksMsec;
  public Func<int, Task>? mockDelay;
  public Func<Task>? mockWaitSmall;

  public INetGameService NetService => this._netService;

  public event Action<ulong>? StateAdded;

  public event Action<ulong>? StateRemoved;

  public event Action<ulong>? StateChanged;

  public event Action<ulong, NetScreenType>? ScreenChanged;

  public PeerInputSynchronizer(INetGameService netService)
  {
    this._netService = netService;
    this._netService.RegisterMessageHandler<PeerInputMessage>(new MessageHandlerDelegate<PeerInputMessage>(this.HandlePeerInputMessage));
    this.GetOrCreateStateForPlayer(this._netService.NetId);
  }

  public void Dispose()
  {
    this._netService.UnregisterMessageHandler<PeerInputMessage>(new MessageHandlerDelegate<PeerInputMessage>(this.HandlePeerInputMessage));
  }

  public void SyncLocalMousePos(Vector2 mouseScreenPos, Control? rootControl)
  {
    PeerInputSynchronizer.PeerInputState stateForPlayer = this.GetOrCreateStateForPlayer(this._netService.NetId);
    if (this._syncMessageToSend == null)
      this._syncMessageToSend = new PeerInputMessage();
    INetCursorPositionTranslator cursorTranslator = this._cursorTranslator;
    Vector2 vector2 = cursorTranslator != null ? cursorTranslator.GetNetPositionFromScreenPosition(mouseScreenPos) : NetCursorHelper.GetNormalizedPosition(mouseScreenPos, rootControl);
    this._syncMessageToSend.netMousePos = new Vector2?(vector2);
    stateForPlayer.netMousePosition = vector2;
    Action<ulong> stateChanged = this.StateChanged;
    if (stateChanged != null)
      stateChanged(this._netService.NetId);
    this.TrySendSyncMessage();
  }

  public void SyncLocalControllerFocus(Vector2 focusPosition, Control? rootControl)
  {
    PeerInputSynchronizer.PeerInputState stateForPlayer = this.GetOrCreateStateForPlayer(this._netService.NetId);
    if (this._syncMessageToSend == null)
      this._syncMessageToSend = new PeerInputMessage();
    INetCursorPositionTranslator cursorTranslator = this._cursorTranslator;
    Vector2 vector2 = cursorTranslator != null ? cursorTranslator.GetNetPositionFromScreenPosition(focusPosition) : NetCursorHelper.GetNormalizedPosition(focusPosition, rootControl);
    this._syncMessageToSend.controllerFocusPosition = new Vector2?(vector2);
    stateForPlayer.controllerFocusPosition = vector2;
    Action<ulong> stateChanged = this.StateChanged;
    if (stateChanged != null)
      stateChanged(this._netService.NetId);
    this.TrySendSyncMessage();
  }

  public void SyncLocalIsUsingController(bool isUsingController)
  {
    PeerInputSynchronizer.PeerInputState stateForPlayer = this.GetOrCreateStateForPlayer(this._netService.NetId);
    if (this._syncMessageToSend == null)
      this._syncMessageToSend = new PeerInputMessage();
    this._syncMessageToSend.isUsingController = isUsingController;
    stateForPlayer.isUsingController = isUsingController;
    Action<ulong> stateChanged = this.StateChanged;
    if (stateChanged != null)
      stateChanged(this._netService.NetId);
    this.TrySendSyncMessage();
  }

  public void SyncLocalMouseDown(bool mouseDown)
  {
    PeerInputSynchronizer.PeerInputState stateForPlayer = this.GetOrCreateStateForPlayer(this._netService.NetId);
    if (this._syncMessageToSend == null)
      this._syncMessageToSend = new PeerInputMessage();
    stateForPlayer.isMouseDown = mouseDown;
    stateForPlayer.isUsingController = false;
    Action<ulong> stateChanged = this.StateChanged;
    if (stateChanged != null)
      stateChanged(this._netService.NetId);
    this.TrySendSyncMessage();
  }

  public void SyncLocalScreen(NetScreenType netScreenType)
  {
    PeerInputSynchronizer.PeerInputState stateForPlayer = this.GetOrCreateStateForPlayer(this._netService.NetId);
    if (this._syncMessageToSend == null)
      this._syncMessageToSend = new PeerInputMessage();
    if (stateForPlayer.netScreenType == netScreenType)
      return;
    this._logger.Debug($"Local screen changed: {stateForPlayer.netScreenType}->{netScreenType}");
    stateForPlayer.netScreenType = netScreenType;
    this.TrySendSyncMessage();
    Action<ulong> stateChanged = this.StateChanged;
    if (stateChanged == null)
      return;
    stateChanged(this._netService.NetId);
  }

  public void SyncLocalHoveredModel(AbstractModel? model)
  {
    PeerInputSynchronizer.PeerInputState stateForPlayer = this.GetOrCreateStateForPlayer(this._netService.NetId);
    if (this._syncMessageToSend == null)
      this._syncMessageToSend = new PeerInputMessage();
    HoveredModelData hoveredModelData = HoveredModelData.FromModel(model);
    if (hoveredModelData.Equals(stateForPlayer.hoveredModelData))
      return;
    stateForPlayer.hoveredModelData = hoveredModelData;
    this.TrySendSyncMessage();
    Action<ulong> stateChanged = this.StateChanged;
    if (stateChanged == null)
      return;
    stateChanged(this._netService.NetId);
  }

  public void SyncLocalIsTargeting(bool isTargeting)
  {
    PeerInputSynchronizer.PeerInputState stateForPlayer = this.GetOrCreateStateForPlayer(this._netService.NetId);
    if (this._syncMessageToSend == null)
      this._syncMessageToSend = new PeerInputMessage();
    if (stateForPlayer.isTargeting == isTargeting)
      return;
    stateForPlayer.isTargeting = isTargeting;
    this.TrySendSyncMessage();
    Action<ulong> stateChanged = this.StateChanged;
    if (stateChanged == null)
      return;
    stateChanged(this._netService.NetId);
  }

  private void TrySendSyncMessage()
  {
    if (this._syncMessageTask != null)
      return;
    int delayMsec = (int) ((long) this._lastSyncMsec + 50L - (long) this.GetTicksMsec());
    if (delayMsec <= 0)
      this._syncMessageTask = TaskHelper.RunSafely(this.SendSyncMessageAfterSmallDelay());
    else
      this._syncMessageTask = TaskHelper.RunSafely(this.QueueSyncMessage(delayMsec));
  }

  private async Task QueueSyncMessage(int delayMsec)
  {
    Func<int, Task> mockDelay = this.mockDelay;
    await ((mockDelay != null ? mockDelay(delayMsec) : (Task) null) ?? Task.Delay(delayMsec));
    this.SendSyncMessage();
  }

  private async Task SendSyncMessageAfterSmallDelay()
  {
    if (this.mockWaitSmall != null)
      await this.mockWaitSmall();
    else
      await Task.Yield();
    this.SendSyncMessage();
  }

  private void SendSyncMessage()
  {
    if (!this._netService.IsConnected)
      return;
    PeerInputSynchronizer.PeerInputState stateForPlayer = this.GetOrCreateStateForPlayer(this._netService.NetId);
    this._syncMessageToSend.mouseDown = stateForPlayer.isMouseDown;
    this._syncMessageToSend.screenType = stateForPlayer.netScreenType;
    this._syncMessageToSend.isTargeting = stateForPlayer.isTargeting;
    this._syncMessageToSend.hoveredModelData = stateForPlayer.hoveredModelData;
    this._syncMessageToSend.isUsingController = stateForPlayer.isUsingController;
    this._syncMessageToSend.controllerFocusPosition = new Vector2?(stateForPlayer.controllerFocusPosition);
    this._netService.SendMessage<PeerInputMessage>(this._syncMessageToSend);
    this._lastSyncMsec = this.GetTicksMsec();
    this._syncMessageToSend = (PeerInputMessage) null;
    this._syncMessageTask = (Task) null;
  }

  private PeerInputSynchronizer.PeerInputState? GetStateForPlayer(ulong playerId)
  {
    int index = this._inputStates.FindIndex((Predicate<PeerInputSynchronizer.PeerInputState>) (s => (long) s.playerId == (long) playerId));
    return index >= 0 ? this._inputStates[index] : (PeerInputSynchronizer.PeerInputState) null;
  }

  private PeerInputSynchronizer.PeerInputState GetOrCreateStateForPlayer(ulong playerId)
  {
    PeerInputSynchronizer.PeerInputState stateForPlayer = this.GetStateForPlayer(playerId);
    if (stateForPlayer == null)
    {
      stateForPlayer = new PeerInputSynchronizer.PeerInputState()
      {
        playerId = playerId
      };
      this._inputStates.Add(stateForPlayer);
      Action<ulong> stateAdded = this.StateAdded;
      if (stateAdded != null)
        stateAdded(playerId);
    }
    return stateForPlayer;
  }

  public void StartOverridingCursorPositioning(INetCursorPositionTranslator positionTranslator)
  {
    this._cursorTranslator = positionTranslator;
  }

  public void StopOverridingCursorPositioning()
  {
    this._cursorTranslator = (INetCursorPositionTranslator) null;
  }

  private void HandlePeerInputMessage(PeerInputMessage message, ulong senderId)
  {
    PeerInputSynchronizer.PeerInputState stateForPlayer = this.GetOrCreateStateForPlayer(senderId);
    if (stateForPlayer.isMouseDown != message.mouseDown)
      this._logger.Debug($"Mouse down state for {senderId} changed: {stateForPlayer.isMouseDown}->{message.mouseDown}");
    if (stateForPlayer.netScreenType != message.screenType)
      this._logger.Debug($"Remote screen for {senderId} changed: {stateForPlayer.netScreenType}->{message.screenType}");
    if (stateForPlayer.isTargeting != message.isTargeting)
      this._logger.Debug($"Targeting state for {senderId} changed: {stateForPlayer.isTargeting}->{message.isTargeting}");
    if (!stateForPlayer.hoveredModelData.Equals(message.hoveredModelData))
      this._logger.Debug($"Hovered model for {senderId} changed: {stateForPlayer.hoveredModelData}->{message.hoveredModelData}");
    if (!stateForPlayer.controllerFocusPosition.Equals((object) message.controllerFocusPosition))
      this._logger.Debug($"Controller focus position for {senderId} changed: {stateForPlayer.controllerFocusPosition}->{message.controllerFocusPosition}");
    if (!stateForPlayer.isUsingController.Equals(message.isUsingController))
      this._logger.Debug($"Using controller state state for {senderId} changed: {stateForPlayer.isUsingController}->{message.isUsingController}");
    NetScreenType netScreenType = stateForPlayer.netScreenType;
    PeerInputSynchronizer.PeerInputState peerInputState1 = stateForPlayer;
    Vector2? nullable = message.netMousePos;
    Vector2 vector2_1 = nullable ?? stateForPlayer.netMousePosition;
    peerInputState1.netMousePosition = vector2_1;
    stateForPlayer.isMouseDown = message.mouseDown;
    stateForPlayer.netScreenType = message.screenType;
    stateForPlayer.isTargeting = message.isTargeting;
    stateForPlayer.hoveredModelData = message.hoveredModelData;
    stateForPlayer.isUsingController = message.isUsingController;
    PeerInputSynchronizer.PeerInputState peerInputState2 = stateForPlayer;
    nullable = message.controllerFocusPosition;
    Vector2 vector2_2 = nullable ?? stateForPlayer.controllerFocusPosition;
    peerInputState2.controllerFocusPosition = vector2_2;
    Action<ulong> stateChanged = this.StateChanged;
    if (stateChanged != null)
      stateChanged(senderId);
    if (netScreenType == stateForPlayer.netScreenType)
      return;
    Action<ulong, NetScreenType> screenChanged = this.ScreenChanged;
    if (screenChanged == null)
      return;
    screenChanged(senderId, netScreenType);
  }

  public Vector2 GetControlSpaceFocusPosition(ulong playerId, Control? rootControl)
  {
    PeerInputSynchronizer.PeerInputState stateForPlayer = this.GetOrCreateStateForPlayer(playerId);
    Vector2 vector2 = stateForPlayer.isUsingController ? stateForPlayer.controllerFocusPosition : stateForPlayer.netMousePosition;
    if (this._cursorTranslator == null)
      return NetCursorHelper.GetControlSpacePosition(vector2, rootControl);
    Vector2 positionFromNetPosition = this._cursorTranslator.GetScreenPositionFromNetPosition(vector2);
    if (rootControl != null)
      return Transform2D.op_Multiply(((CanvasItem) rootControl).GetGlobalTransformWithCanvas(), positionFromNetPosition);
    if (TestMode.IsOn)
      return positionFromNetPosition;
    throw new InvalidOperationException("Root node should only be null in tests!");
  }

  public bool GetMouseDown(ulong playerId) => this.GetOrCreateStateForPlayer(playerId).isMouseDown;

  public NetScreenType GetScreenType(ulong playerId)
  {
    return this.GetOrCreateStateForPlayer(playerId).netScreenType;
  }

  public HoveredModelData GetHoveredModelData(ulong playerId)
  {
    return this.GetOrCreateStateForPlayer(playerId).hoveredModelData;
  }

  public bool GetIsTargeting(ulong playerId)
  {
    return this.GetOrCreateStateForPlayer(playerId).isTargeting;
  }

  public void OnPlayerDisconnected(ulong playerId)
  {
    this._logger.Debug($"Disconnected player {playerId}, removing PeerInputState");
    this._inputStates.RemoveAll((Predicate<PeerInputSynchronizer.PeerInputState>) (p => (long) p.playerId == (long) playerId));
    Action<ulong> stateRemoved = this.StateRemoved;
    if (stateRemoved == null)
      return;
    stateRemoved(playerId);
  }

  private ulong GetTicksMsec()
  {
    Func<ulong> mockGetTicksMsec = this.mockGetTicksMsec;
    return mockGetTicksMsec == null ? Time.GetTicksMsec() : mockGetTicksMsec();
  }

  private class PeerInputState
  {
    public ulong playerId;
    public Vector2 netMousePosition;
    public bool isMouseDown;
    public NetScreenType netScreenType;
    public HoveredModelData hoveredModelData;
    public bool isTargeting;
    public bool isUsingController;
    public Vector2 controllerFocusPosition;
  }
}
