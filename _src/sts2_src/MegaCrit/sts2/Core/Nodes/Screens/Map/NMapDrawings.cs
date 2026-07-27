// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Screens.Map.NMapDrawings
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.Entities.Multiplayer;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Multiplayer.Game;
using MegaCrit.Sts2.Core.Multiplayer.Game.PeerInput;
using MegaCrit.Sts2.Core.Multiplayer.Messages.Game.Flavor;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.Saves;
using MegaCrit.Sts2.Core.Saves.MapDrawing;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Screens.Map;

[ScriptPath("res://src/Core/Nodes/Screens/Map/NMapDrawings.cs")]
public class NMapDrawings : Control
{
  private const int _minUpdateMsec = 50;
  private static readonly string _lineDrawScenePath = SceneHelper.GetScenePath("screens/map/map_line_draw");
  private static readonly string _lineEraseScenePath = SceneHelper.GetScenePath("screens/map/map_line_erase");
  private static readonly string _playerDrawingPath = SceneHelper.GetScenePath("screens/map/map_drawing");
  public const string drawingCursorPath = "res://images/packed/common_ui/cursor_quill.png";
  public const string drawingCursorTiltedPath = "res://images/packed/common_ui/cursor_quill_tilted.png";
  public static readonly Vector2 drawingCursorHotspot = new Vector2(2f, 56f);
  public const string erasingCursorPath = "res://images/packed/common_ui/cursor_eraser.png";
  public const string erasingCursorTiltedPath = "res://images/packed/common_ui/cursor_eraser_tilted.png";
  public static readonly Vector2 erasingCursorHotspot = new Vector2(24f, 58f);
  private const float _minimumPointDistance = 2f;
  private INetGameService _netService;
  private IPlayerCollection _playerCollection;
  private PeerInputSynchronizer _inputSynchronizer;
  private PackedScene _lineDrawScene;
  private PackedScene _lineEraseScene;
  private NCursorManager _cursorManager;
  private Material _eraserMaterial;
  private Vector2 _defaultSize;
  private readonly List<NMapDrawings.DrawingState> _drawingStates = new List<NMapDrawings.DrawingState>();
  private MapDrawingMessage? _queuedMessage;
  private ulong _lastMessageMsec;
  private Task? _sendMessageTask;

  private static IEnumerable<string> SelfAssetPaths
  {
    get
    {
      return (IEnumerable<string>) new \u003C\u003Ez__ReadOnlyArray<string>(new string[7]
      {
        NMapDrawings._lineDrawScenePath,
        NMapDrawings._lineEraseScenePath,
        NMapDrawings._playerDrawingPath,
        "res://images/packed/common_ui/cursor_quill.png",
        "res://images/packed/common_ui/cursor_quill_tilted.png",
        "res://images/packed/common_ui/cursor_eraser.png",
        "res://images/packed/common_ui/cursor_eraser_tilted.png"
      });
    }
  }

  public static IEnumerable<string> AssetPaths
  {
    get => NMapDrawings.SelfAssetPaths.Concat<string>(NMapDrawButton.AssetPaths);
  }

  public override void _Ready()
  {
    this._lineDrawScene = PreloadManager.Cache.GetScene(NMapDrawings._lineDrawScenePath);
    this._lineEraseScene = PreloadManager.Cache.GetScene(NMapDrawings._lineEraseScenePath);
    this._cursorManager = NGame.Instance.CursorManager;
    Line2D line2D = this._lineEraseScene.Instantiate<Line2D>((PackedScene.GenEditState) 0L);
    this._eraserMaterial = ((CanvasItem) line2D).Material;
    ((Node) line2D).QueueFreeSafely();
    this._defaultSize = this.Size;
  }

  public void Initialize(
    INetGameService netService,
    IPlayerCollection playerCollection,
    PeerInputSynchronizer inputSynchronizer)
  {
    this._netService = netService;
    this._playerCollection = playerCollection;
    this._inputSynchronizer = inputSynchronizer;
    this._netService.RegisterMessageHandler<MapDrawingMessage>(new MessageHandlerDelegate<MapDrawingMessage>(this.HandleDrawingMessage));
    this._netService.RegisterMessageHandler<ClearMapDrawingsMessage>(new MessageHandlerDelegate<ClearMapDrawingsMessage>(this.HandleClearMapDrawingsMessage));
    this._netService.RegisterMessageHandler<MapDrawingModeChangedMessage>(new MessageHandlerDelegate<MapDrawingModeChangedMessage>(this.HandleMapDrawingModeChangedMessage));
    inputSynchronizer.ScreenChanged += new Action<ulong, NetScreenType>(this.OnPlayerScreenChanged);
  }

  public override void _ExitTree()
  {
    this._netService.UnregisterMessageHandler<MapDrawingMessage>(new MessageHandlerDelegate<MapDrawingMessage>(this.HandleDrawingMessage));
    this._netService.UnregisterMessageHandler<ClearMapDrawingsMessage>(new MessageHandlerDelegate<ClearMapDrawingsMessage>(this.HandleClearMapDrawingsMessage));
    this._netService.UnregisterMessageHandler<MapDrawingModeChangedMessage>(new MessageHandlerDelegate<MapDrawingModeChangedMessage>(this.HandleMapDrawingModeChangedMessage));
    this._inputSynchronizer.ScreenChanged -= new Action<ulong, NetScreenType>(this.OnPlayerScreenChanged);
  }

  public void BeginLineLocal(Vector2 position, DrawingMode? overrideDrawingMode)
  {
    this.BeginLine(this.GetDrawingStateForPlayer(this._netService.NetId), position, overrideDrawingMode);
    this.QueueOrSendEvent(new NetMapDrawingEvent()
    {
      type = MapDrawingEventType.BeginLine,
      position = this.ToNetPosition(position),
      overrideDrawingMode = overrideDrawingMode
    });
  }

  public void UpdateCurrentLinePositionLocal(Vector2 position)
  {
    NMapDrawings.DrawingState drawingStateForPlayer = this.GetDrawingStateForPlayer(this._netService.NetId);
    this.UpdateCurrentLinePosition(drawingStateForPlayer, position);
    this.QueueOrSendEvent(new NetMapDrawingEvent()
    {
      type = MapDrawingEventType.ContinueLine,
      position = this.ToNetPosition(position),
      overrideDrawingMode = drawingStateForPlayer.overrideDrawingMode
    });
  }

  public void StopLineLocal()
  {
    this.StopDrawingLine(this.GetDrawingStateForPlayer(this._netService.NetId));
    this.QueueOrSendEvent(new NetMapDrawingEvent()
    {
      type = MapDrawingEventType.EndLine
    });
  }

  public void SetDrawingModeLocal(DrawingMode drawingMode)
  {
    this.SetDrawingMode(this.GetDrawingStateForPlayer(this._netService.NetId), drawingMode);
    this._netService.SendMessage<MapDrawingModeChangedMessage>(new MapDrawingModeChangedMessage()
    {
      drawingMode = drawingMode
    });
    this.UpdateLocalCursor();
  }

  public void ClearDrawnLinesLocal()
  {
    this.ClearAllLinesForPlayer(this.GetDrawingStateForPlayer(this._netService.NetId));
    this.UpdateLocalCursor();
    this._netService.SendMessage<ClearMapDrawingsMessage>(new ClearMapDrawingsMessage());
  }

  public bool IsDrawing(ulong playerId) => this.GetDrawingStateForPlayer(playerId).IsDrawing;

  public bool IsLocalDrawing() => this.GetDrawingStateForPlayer(this._netService.NetId).IsDrawing;

  public DrawingMode GetDrawingMode(ulong playerId)
  {
    return this.GetDrawingStateForPlayer(playerId).CurrentDrawingMode;
  }

  public DrawingMode GetLocalDrawingMode(bool useOverride = true)
  {
    return !useOverride ? this.GetDrawingStateForPlayer(this._netService.NetId).drawingMode : this.GetDrawingStateForPlayer(this._netService.NetId).CurrentDrawingMode;
  }

  private void QueueOrSendEvent(NetMapDrawingEvent ev)
  {
    if (this._queuedMessage == null)
      this._queuedMessage = new MapDrawingMessage();
    if (!this._queuedMessage.TryAddEvent(ev))
    {
      this._queuedMessage.drawingMode = new DrawingMode?(this.GetDrawingStateForPlayer(this._netService.NetId).drawingMode);
      this._netService.SendMessage<MapDrawingMessage>(this._queuedMessage);
      this._queuedMessage = new MapDrawingMessage();
      if (!this._queuedMessage.TryAddEvent(ev))
        throw new InvalidOperationException();
    }
    this.TrySendSyncMessage();
    this.UpdateLocalCursor();
  }

  private Vector2 ToNetPosition(Vector2 pos)
  {
    pos.X -= this.Size.X * 0.5f;
    pos = Vector2.op_Division(pos, new Vector2(960f, this.Size.Y));
    return pos;
  }

  private Vector2 FromNetPosition(Vector2 pos)
  {
    pos = Vector2.op_Multiply(pos, new Vector2(960f, this.Size.Y));
    pos.X += this.Size.X * 0.5f;
    return pos;
  }

  private void HandleDrawingMessage(MapDrawingMessage message, ulong senderId)
  {
    NMapDrawings.DrawingState drawingStateForPlayer = this.GetDrawingStateForPlayer(senderId);
    foreach (NetMapDrawingEvent netMapDrawingEvent in (IEnumerable<NetMapDrawingEvent>) message.Events)
    {
      if (netMapDrawingEvent.type == MapDrawingEventType.BeginLine)
      {
        if (this.GetDrawingMode(senderId) != DrawingMode.None)
          this.StopDrawingLine(drawingStateForPlayer);
        this.BeginLine(drawingStateForPlayer, this.FromNetPosition(netMapDrawingEvent.position), netMapDrawingEvent.overrideDrawingMode);
      }
      else if (netMapDrawingEvent.type == MapDrawingEventType.ContinueLine)
      {
        if (!drawingStateForPlayer.IsDrawing)
        {
          if (message.drawingMode.HasValue)
          {
            int drawingMode1 = (int) drawingStateForPlayer.drawingMode;
            DrawingMode? drawingMode2 = message.drawingMode;
            int valueOrDefault = (int) drawingMode2.GetValueOrDefault();
            if (!(drawingMode1 == valueOrDefault & drawingMode2.HasValue))
              this.SetDrawingMode(drawingStateForPlayer, message.drawingMode.Value);
          }
          this.BeginLine(drawingStateForPlayer, this.FromNetPosition(netMapDrawingEvent.position), netMapDrawingEvent.overrideDrawingMode);
        }
        this.UpdateCurrentLinePosition(drawingStateForPlayer, this.FromNetPosition(netMapDrawingEvent.position));
      }
      else
        this.StopDrawingLine(drawingStateForPlayer);
    }
  }

  private void HandleClearMapDrawingsMessage(ClearMapDrawingsMessage message, ulong senderId)
  {
    this.ClearAllLinesForPlayer(this.GetDrawingStateForPlayer(senderId));
  }

  private void HandleMapDrawingModeChangedMessage(
    MapDrawingModeChangedMessage message,
    ulong senderId)
  {
    this.SetDrawingMode(this.GetDrawingStateForPlayer(senderId), message.drawingMode);
  }

  private void BeginLine(
    NMapDrawings.DrawingState state,
    Vector2 position,
    DrawingMode? overrideDrawingMode)
  {
    Player player = this._playerCollection.GetPlayer(state.playerId);
    DrawingMode drawingMode = (DrawingMode) ((int) overrideDrawingMode ?? (int) state.drawingMode);
    if (drawingMode == DrawingMode.None)
      throw new InvalidOperationException($"Player {state.playerId} is not currently in a drawing mode and no override was passed!");
    state.overrideDrawingMode = overrideDrawingMode;
    state.currentlyDrawingLine = this.CreateLineForPlayer(player, drawingMode == DrawingMode.Erasing);
    state.currentlyDrawingLine.AddPoint(Vector2.op_Multiply(position, 0.5f), -1);
    state.currentlyDrawingLine.AddPoint(Vector2.op_Addition(Vector2.op_Multiply(position, 0.5f), new Vector2(0.0f, 0.5f)), -1);
    ((Node) state.drawViewport).AddChildSafely((Node) state.currentlyDrawingLine);
    NGame.Instance.RemoteCursorContainer.DrawingCursorStateChanged(state.playerId);
  }

  private Line2D CreateLineForPlayer(Player player, bool isErasing)
  {
    Line2D lineForPlayer = (isErasing ? this._lineEraseScene : this._lineDrawScene).Instantiate<Line2D>((PackedScene.GenEditState) 0L);
    lineForPlayer.DefaultColor = player.Character.MapDrawingColor;
    lineForPlayer.ClearPoints();
    ((Node2D) lineForPlayer).Position = Vector2.Zero;
    return lineForPlayer;
  }

  private void StopDrawingLine(NMapDrawings.DrawingState state)
  {
    state.overrideDrawingMode = new DrawingMode?();
    state.currentlyDrawingLine = (Line2D) null;
    NGame.Instance.RemoteCursorContainer.DrawingCursorStateChanged(state.playerId);
  }

  private void SetDrawingMode(NMapDrawings.DrawingState state, DrawingMode drawingMode)
  {
    if (state.drawingMode == drawingMode)
      return;
    state.drawingMode = drawingMode;
    NGame.Instance.RemoteCursorContainer.DrawingCursorStateChanged(state.playerId);
  }

  private void UpdateCurrentLinePosition(NMapDrawings.DrawingState state, Vector2 position)
  {
    if (state.currentlyDrawingLine == null)
      throw new InvalidOperationException($"Tried to update current line position for player {state.playerId}, but they are not currently drawing a line!");
    Vector2[] points = state.currentlyDrawingLine.Points;
    Vector2 vector2 = points[points.Length - 1];
    if ((double) ((Vector2) ref vector2).DistanceSquaredTo(position) < 4.0)
      return;
    state.currentlyDrawingLine.AddPoint(Vector2.op_Multiply(position, 0.5f), -1);
  }

  private NMapDrawings.DrawingState GetDrawingStateForPlayer(ulong playerId)
  {
    NMapDrawings.DrawingState state = this._drawingStates.FirstOrDefault<NMapDrawings.DrawingState>((Func<NMapDrawings.DrawingState, bool>) (s => (long) s.playerId == (long) playerId));
    if (state == null)
    {
      Control child = PreloadManager.Cache.GetScene(NMapDrawings._playerDrawingPath).Instantiate<Control>((PackedScene.GenEditState) 0L);
      ((Node) this).AddChildSafely((Node) child);
      state = new NMapDrawings.DrawingState()
      {
        playerId = playerId,
        drawViewport = ((Node) child).GetNode<SubViewport>(NodePath.op_Implicit("DrawViewport")),
        drawingTexture = ((Node) child).GetNode<TextureRect>(NodePath.op_Implicit("DrawViewportTextureRect"))
      };
      TaskHelper.RunSafely(this.SetVisibleLater(state));
      this._drawingStates.Add(state);
    }
    return state;
  }

  private async Task SetVisibleLater(NMapDrawings.DrawingState state)
  {
    state.drawViewport.RenderTargetUpdateMode = (SubViewport.UpdateMode) 4L;
    ((CanvasItem) state.drawingTexture).Visible = false;
    double num1 = (double) await ((Node) this).AwaitProcessFrame();
    double num2 = (double) await ((Node) this).AwaitProcessFrame();
    double num3 = (double) await ((Node) this).AwaitProcessFrame();
    ((CanvasItem) state.drawingTexture).Visible = this.ShouldShowMapDrawing(state);
    state.drawViewport.RenderTargetUpdateMode = (SubViewport.UpdateMode) 2L;
  }

  public void UpdateVisibilityFromSettings()
  {
    foreach (NMapDrawings.DrawingState drawingState in this._drawingStates)
      ((CanvasItem) drawingState.drawingTexture).Visible = this.ShouldShowMapDrawing(drawingState);
  }

  private bool ShouldShowMapDrawing(NMapDrawings.DrawingState state)
  {
    if (SaveManager.Instance.PrefsSave.ShowMultiplayerDrawings)
      return true;
    ulong? netId = LocalContext.NetId;
    ulong playerId = state.playerId;
    return (long) netId.GetValueOrDefault() == (long) playerId & netId.HasValue;
  }

  public void ClearAllLines()
  {
    foreach (NMapDrawings.DrawingState drawingState in this._drawingStates)
    {
      foreach (Node node in ((IEnumerable) ((Node) drawingState.drawViewport).GetChildren(false)).OfType<Line2D>())
        node.QueueFreeSafely();
    }
  }

  public SerializableMapDrawings GetSerializableMapDrawings()
  {
    SerializableMapDrawings serializableMapDrawings = new SerializableMapDrawings();
    foreach (NMapDrawings.DrawingState drawingState in this._drawingStates)
    {
      SerializablePlayerMapDrawings playerMapDrawings = new SerializablePlayerMapDrawings()
      {
        playerId = drawingState.playerId
      };
      serializableMapDrawings.drawings.Add(playerMapDrawings);
      foreach (Line2D line2D in ((IEnumerable) ((Node) drawingState.drawViewport).GetChildren(false)).OfType<Line2D>())
      {
        SerializableMapDrawingLine serializableMapDrawingLine = new SerializableMapDrawingLine()
        {
          mapPoints = new List<Vector2>()
        };
        serializableMapDrawingLine.isEraser = ((CanvasItem) line2D).Material == this._eraserMaterial;
        playerMapDrawings.lines.Add(serializableMapDrawingLine);
        foreach (Vector2 point in line2D.Points)
          serializableMapDrawingLine.mapPoints.Add(this.ToNetPosition(point));
      }
    }
    return serializableMapDrawings;
  }

  public void LoadDrawings(SerializableMapDrawings drawings)
  {
    foreach (SerializablePlayerMapDrawings drawing in drawings.drawings)
    {
      Player player = this._playerCollection.GetPlayer(drawing.playerId);
      if (player == null)
      {
        Log.Warn($"Player {drawing.playerId} has map drawings, but doesn't exist in the run!");
      }
      else
      {
        NMapDrawings.DrawingState drawingStateForPlayer = this.GetDrawingStateForPlayer(drawing.playerId);
        foreach (SerializableMapDrawingLine line in drawing.lines)
        {
          Line2D lineForPlayer = this.CreateLineForPlayer(player, line.isEraser);
          ((Node) drawingStateForPlayer.drawViewport).AddChildSafely((Node) lineForPlayer);
          foreach (Vector2 mapPoint in line.mapPoints)
            lineForPlayer.AddPoint(this.FromNetPosition(mapPoint), -1);
        }
      }
    }
  }

  private void ClearAllLinesForPlayer(NMapDrawings.DrawingState state)
  {
    foreach (Node node in ((IEnumerable) ((Node) state.drawViewport).GetChildren(false)).OfType<Line2D>())
      node.QueueFreeSafely();
    this.SetDrawingMode(state, DrawingMode.None);
  }

  private void OnPlayerScreenChanged(ulong playerId, NetScreenType oldScreenType)
  {
    if ((long) playerId == (long) this._netService.NetId)
      return;
    NetScreenType screenType = this._inputSynchronizer.GetScreenType(playerId);
    if (oldScreenType != NetScreenType.Map || screenType == NetScreenType.Map)
      return;
    NMapDrawings.DrawingState drawingStateForPlayer = this.GetDrawingStateForPlayer(playerId);
    if (drawingStateForPlayer.IsDrawing)
      this.StopDrawingLine(drawingStateForPlayer);
    if (drawingStateForPlayer.drawingMode == DrawingMode.None)
      return;
    this.SetDrawingMode(drawingStateForPlayer, DrawingMode.None);
  }

  private void TrySendSyncMessage()
  {
    if (this._sendMessageTask != null || !this._netService.IsConnected)
      return;
    int delayMsec = (int) ((long) this._lastMessageMsec + 50L - (long) Time.GetTicksMsec());
    if (delayMsec <= 0)
      this._sendMessageTask = TaskHelper.RunSafely(this.SendSyncMessageAfterSmallDelay());
    else
      this._sendMessageTask = TaskHelper.RunSafely(this.QueueSyncMessage(delayMsec));
  }

  private async Task QueueSyncMessage(int delayMsec)
  {
    await Task.Delay(delayMsec);
    this.SendSyncMessage();
  }

  private async Task SendSyncMessageAfterSmallDelay()
  {
    await Task.Yield();
    this.SendSyncMessage();
  }

  private void SendSyncMessage()
  {
    if (!this._netService.IsConnected)
      return;
    this._queuedMessage.drawingMode = new DrawingMode?(this.GetDrawingStateForPlayer(this._netService.NetId).drawingMode);
    this._netService.SendMessage<MapDrawingMessage>(this._queuedMessage);
    this._lastMessageMsec = Time.GetTicksMsec();
    this._queuedMessage = (MapDrawingMessage) null;
    this._sendMessageTask = (Task) null;
  }

  private void UpdateLocalCursor()
  {
    NMapDrawings.DrawingState drawingStateForPlayer = this.GetDrawingStateForPlayer(this._netService.NetId);
    if (drawingStateForPlayer.CurrentDrawingMode == DrawingMode.Drawing)
    {
      Image asset = PreloadManager.Cache.GetAsset<Image>("res://images/packed/common_ui/cursor_quill.png");
      this._cursorManager.OverrideCursor(PreloadManager.Cache.GetAsset<Image>("res://images/packed/common_ui/cursor_quill_tilted.png"), asset, NMapDrawings.drawingCursorHotspot);
    }
    else if (drawingStateForPlayer.CurrentDrawingMode == DrawingMode.Erasing)
    {
      Image asset = PreloadManager.Cache.GetAsset<Image>("res://images/packed/common_ui/cursor_eraser.png");
      this._cursorManager.OverrideCursor(PreloadManager.Cache.GetAsset<Image>("res://images/packed/common_ui/cursor_eraser_tilted.png"), asset, NMapDrawings.erasingCursorHotspot);
    }
    else
      this._cursorManager.StopOverridingCursor();
  }

  public void RepositionBasedOnBackground(Control mapBg)
  {
    this.Position = new Vector2(mapBg.Position.X + (float) (((double) mapBg.Size.X - (double) this.Size.X) * 0.5), mapBg.Position.Y);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(19)
    {
      new MethodInfo(NMapDrawings.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NMapDrawings.MethodName._ExitTree, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NMapDrawings.MethodName.UpdateCurrentLinePositionLocal, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 5L, StringName.op_Implicit("position"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NMapDrawings.MethodName.StopLineLocal, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NMapDrawings.MethodName.SetDrawingModeLocal, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 2L, StringName.op_Implicit("drawingMode"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NMapDrawings.MethodName.ClearDrawnLinesLocal, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NMapDrawings.MethodName.IsDrawing, new PropertyInfo((Variant.Type) 1L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 2L, StringName.op_Implicit("playerId"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NMapDrawings.MethodName.IsLocalDrawing, new PropertyInfo((Variant.Type) 1L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NMapDrawings.MethodName.GetDrawingMode, new PropertyInfo((Variant.Type) 2L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 2L, StringName.op_Implicit("playerId"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NMapDrawings.MethodName.GetLocalDrawingMode, new PropertyInfo((Variant.Type) 2L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 1L, StringName.op_Implicit("useOverride"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NMapDrawings.MethodName.ToNetPosition, new PropertyInfo((Variant.Type) 5L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 5L, StringName.op_Implicit("pos"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NMapDrawings.MethodName.FromNetPosition, new PropertyInfo((Variant.Type) 5L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 5L, StringName.op_Implicit("pos"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NMapDrawings.MethodName.UpdateVisibilityFromSettings, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NMapDrawings.MethodName.ClearAllLines, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NMapDrawings.MethodName.OnPlayerScreenChanged, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 2L, StringName.op_Implicit("playerId"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false),
        new PropertyInfo((Variant.Type) 2L, StringName.op_Implicit("oldScreenType"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NMapDrawings.MethodName.TrySendSyncMessage, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NMapDrawings.MethodName.SendSyncMessage, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NMapDrawings.MethodName.UpdateLocalCursor, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NMapDrawings.MethodName.RepositionBasedOnBackground, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("mapBg"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false)
      }, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NMapDrawings.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NMapDrawings.MethodName._ExitTree) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._ExitTree();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NMapDrawings.MethodName.UpdateCurrentLinePositionLocal) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.UpdateCurrentLinePositionLocal(VariantUtils.ConvertTo<Vector2>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NMapDrawings.MethodName.StopLineLocal) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.StopLineLocal();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NMapDrawings.MethodName.SetDrawingModeLocal) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.SetDrawingModeLocal(VariantUtils.ConvertTo<DrawingMode>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NMapDrawings.MethodName.ClearDrawnLinesLocal) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.ClearDrawnLinesLocal();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NMapDrawings.MethodName.IsDrawing) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      bool flag = this.IsDrawing(VariantUtils.ConvertTo<ulong>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = VariantUtils.CreateFrom<bool>(ref flag);
      return true;
    }
    if (StringName.op_Equality(ref method, NMapDrawings.MethodName.IsLocalDrawing) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      bool flag = this.IsLocalDrawing();
      ret = VariantUtils.CreateFrom<bool>(ref flag);
      return true;
    }
    if (StringName.op_Equality(ref method, NMapDrawings.MethodName.GetDrawingMode) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      DrawingMode drawingMode = this.GetDrawingMode(VariantUtils.ConvertTo<ulong>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = VariantUtils.CreateFrom<DrawingMode>(ref drawingMode);
      return true;
    }
    if (StringName.op_Equality(ref method, NMapDrawings.MethodName.GetLocalDrawingMode) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      DrawingMode localDrawingMode = this.GetLocalDrawingMode(VariantUtils.ConvertTo<bool>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = VariantUtils.CreateFrom<DrawingMode>(ref localDrawingMode);
      return true;
    }
    if (StringName.op_Equality(ref method, NMapDrawings.MethodName.ToNetPosition) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      Vector2 netPosition = this.ToNetPosition(VariantUtils.ConvertTo<Vector2>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = VariantUtils.CreateFrom<Vector2>(ref netPosition);
      return true;
    }
    if (StringName.op_Equality(ref method, NMapDrawings.MethodName.FromNetPosition) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      Vector2 vector2 = this.FromNetPosition(VariantUtils.ConvertTo<Vector2>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = VariantUtils.CreateFrom<Vector2>(ref vector2);
      return true;
    }
    if (StringName.op_Equality(ref method, NMapDrawings.MethodName.UpdateVisibilityFromSettings) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.UpdateVisibilityFromSettings();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NMapDrawings.MethodName.ClearAllLines) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.ClearAllLines();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NMapDrawings.MethodName.OnPlayerScreenChanged) && ((NativeVariantPtrArgs) ref args).Count == 2)
    {
      this.OnPlayerScreenChanged(VariantUtils.ConvertTo<ulong>(ref ((NativeVariantPtrArgs) ref args)[0]), VariantUtils.ConvertTo<NetScreenType>(ref ((NativeVariantPtrArgs) ref args)[1]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NMapDrawings.MethodName.TrySendSyncMessage) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.TrySendSyncMessage();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NMapDrawings.MethodName.SendSyncMessage) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.SendSyncMessage();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NMapDrawings.MethodName.UpdateLocalCursor) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.UpdateLocalCursor();
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NMapDrawings.MethodName.RepositionBasedOnBackground) || ((NativeVariantPtrArgs) ref args).Count != 1)
      return base.InvokeGodotClassMethod(ref method, args, ref ret);
    this.RepositionBasedOnBackground(VariantUtils.ConvertTo<Control>(ref ((NativeVariantPtrArgs) ref args)[0]));
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NMapDrawings.MethodName._Ready) || StringName.op_Equality(ref method, NMapDrawings.MethodName._ExitTree) || StringName.op_Equality(ref method, NMapDrawings.MethodName.UpdateCurrentLinePositionLocal) || StringName.op_Equality(ref method, NMapDrawings.MethodName.StopLineLocal) || StringName.op_Equality(ref method, NMapDrawings.MethodName.SetDrawingModeLocal) || StringName.op_Equality(ref method, NMapDrawings.MethodName.ClearDrawnLinesLocal) || StringName.op_Equality(ref method, NMapDrawings.MethodName.IsDrawing) || StringName.op_Equality(ref method, NMapDrawings.MethodName.IsLocalDrawing) || StringName.op_Equality(ref method, NMapDrawings.MethodName.GetDrawingMode) || StringName.op_Equality(ref method, NMapDrawings.MethodName.GetLocalDrawingMode) || StringName.op_Equality(ref method, NMapDrawings.MethodName.ToNetPosition) || StringName.op_Equality(ref method, NMapDrawings.MethodName.FromNetPosition) || StringName.op_Equality(ref method, NMapDrawings.MethodName.UpdateVisibilityFromSettings) || StringName.op_Equality(ref method, NMapDrawings.MethodName.ClearAllLines) || StringName.op_Equality(ref method, NMapDrawings.MethodName.OnPlayerScreenChanged) || StringName.op_Equality(ref method, NMapDrawings.MethodName.TrySendSyncMessage) || StringName.op_Equality(ref method, NMapDrawings.MethodName.SendSyncMessage) || StringName.op_Equality(ref method, NMapDrawings.MethodName.UpdateLocalCursor) || StringName.op_Equality(ref method, NMapDrawings.MethodName.RepositionBasedOnBackground) || base.HasGodotClassMethod(ref method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NMapDrawings.PropertyName._lineDrawScene))
    {
      this._lineDrawScene = VariantUtils.ConvertTo<PackedScene>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NMapDrawings.PropertyName._lineEraseScene))
    {
      this._lineEraseScene = VariantUtils.ConvertTo<PackedScene>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NMapDrawings.PropertyName._cursorManager))
    {
      this._cursorManager = VariantUtils.ConvertTo<NCursorManager>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NMapDrawings.PropertyName._eraserMaterial))
    {
      this._eraserMaterial = VariantUtils.ConvertTo<Material>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NMapDrawings.PropertyName._defaultSize))
    {
      this._defaultSize = VariantUtils.ConvertTo<Vector2>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NMapDrawings.PropertyName._lastMessageMsec))
      return ((GodotObject) this).SetGodotClassPropertyValue(ref name, ref value);
    this._lastMessageMsec = VariantUtils.ConvertTo<ulong>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NMapDrawings.PropertyName._lineDrawScene))
    {
      value = VariantUtils.CreateFrom<PackedScene>(ref this._lineDrawScene);
      return true;
    }
    if (StringName.op_Equality(ref name, NMapDrawings.PropertyName._lineEraseScene))
    {
      value = VariantUtils.CreateFrom<PackedScene>(ref this._lineEraseScene);
      return true;
    }
    if (StringName.op_Equality(ref name, NMapDrawings.PropertyName._cursorManager))
    {
      value = VariantUtils.CreateFrom<NCursorManager>(ref this._cursorManager);
      return true;
    }
    if (StringName.op_Equality(ref name, NMapDrawings.PropertyName._eraserMaterial))
    {
      value = VariantUtils.CreateFrom<Material>(ref this._eraserMaterial);
      return true;
    }
    if (StringName.op_Equality(ref name, NMapDrawings.PropertyName._defaultSize))
    {
      value = VariantUtils.CreateFrom<Vector2>(ref this._defaultSize);
      return true;
    }
    if (!StringName.op_Equality(ref name, NMapDrawings.PropertyName._lastMessageMsec))
      return ((GodotObject) this).GetGodotClassPropertyValue(ref name, ref value);
    value = VariantUtils.CreateFrom<ulong>(ref this._lastMessageMsec);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NMapDrawings.PropertyName._lineDrawScene, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NMapDrawings.PropertyName._lineEraseScene, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NMapDrawings.PropertyName._cursorManager, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NMapDrawings.PropertyName._eraserMaterial, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 5L, NMapDrawings.PropertyName._defaultSize, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 2L, NMapDrawings.PropertyName._lastMessageMsec, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).SaveGodotObjectData(info);
    info.AddProperty(NMapDrawings.PropertyName._lineDrawScene, Variant.From<PackedScene>(ref this._lineDrawScene));
    info.AddProperty(NMapDrawings.PropertyName._lineEraseScene, Variant.From<PackedScene>(ref this._lineEraseScene));
    info.AddProperty(NMapDrawings.PropertyName._cursorManager, Variant.From<NCursorManager>(ref this._cursorManager));
    info.AddProperty(NMapDrawings.PropertyName._eraserMaterial, Variant.From<Material>(ref this._eraserMaterial));
    info.AddProperty(NMapDrawings.PropertyName._defaultSize, Variant.From<Vector2>(ref this._defaultSize));
    info.AddProperty(NMapDrawings.PropertyName._lastMessageMsec, Variant.From<ulong>(ref this._lastMessageMsec));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NMapDrawings.PropertyName._lineDrawScene, ref variant1))
      this._lineDrawScene = ((Variant) ref variant1).As<PackedScene>();
    Variant variant2;
    if (info.TryGetProperty(NMapDrawings.PropertyName._lineEraseScene, ref variant2))
      this._lineEraseScene = ((Variant) ref variant2).As<PackedScene>();
    Variant variant3;
    if (info.TryGetProperty(NMapDrawings.PropertyName._cursorManager, ref variant3))
      this._cursorManager = ((Variant) ref variant3).As<NCursorManager>();
    Variant variant4;
    if (info.TryGetProperty(NMapDrawings.PropertyName._eraserMaterial, ref variant4))
      this._eraserMaterial = ((Variant) ref variant4).As<Material>();
    Variant variant5;
    if (info.TryGetProperty(NMapDrawings.PropertyName._defaultSize, ref variant5))
      this._defaultSize = ((Variant) ref variant5).As<Vector2>();
    Variant variant6;
    if (!info.TryGetProperty(NMapDrawings.PropertyName._lastMessageMsec, ref variant6))
      return;
    this._lastMessageMsec = ((Variant) ref variant6).As<ulong>();
  }

  private class DrawingState
  {
    public DrawingMode? overrideDrawingMode;
    public DrawingMode drawingMode;
    public ulong playerId;
    public 
    #nullable enable
    Line2D? currentlyDrawingLine;
    public required SubViewport drawViewport;
    public required TextureRect drawingTexture;

    public bool IsDrawing => this.currentlyDrawingLine != null;

    public DrawingMode CurrentDrawingMode => this.overrideDrawingMode ?? this.drawingMode;
  }

  public class MethodName : Control.MethodName
  {
    public static readonly 
    #nullable disable
    StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName _ExitTree = StringName.op_Implicit(nameof (_ExitTree));
    public static readonly StringName UpdateCurrentLinePositionLocal = StringName.op_Implicit(nameof (UpdateCurrentLinePositionLocal));
    public static readonly StringName StopLineLocal = StringName.op_Implicit(nameof (StopLineLocal));
    public static readonly StringName SetDrawingModeLocal = StringName.op_Implicit(nameof (SetDrawingModeLocal));
    public static readonly StringName ClearDrawnLinesLocal = StringName.op_Implicit(nameof (ClearDrawnLinesLocal));
    public static readonly StringName IsDrawing = StringName.op_Implicit(nameof (IsDrawing));
    public static readonly StringName IsLocalDrawing = StringName.op_Implicit(nameof (IsLocalDrawing));
    public static readonly StringName GetDrawingMode = StringName.op_Implicit(nameof (GetDrawingMode));
    public static readonly StringName GetLocalDrawingMode = StringName.op_Implicit(nameof (GetLocalDrawingMode));
    public static readonly StringName ToNetPosition = StringName.op_Implicit(nameof (ToNetPosition));
    public static readonly StringName FromNetPosition = StringName.op_Implicit(nameof (FromNetPosition));
    public static readonly StringName UpdateVisibilityFromSettings = StringName.op_Implicit(nameof (UpdateVisibilityFromSettings));
    public static readonly StringName ClearAllLines = StringName.op_Implicit(nameof (ClearAllLines));
    public static readonly StringName OnPlayerScreenChanged = StringName.op_Implicit(nameof (OnPlayerScreenChanged));
    public static readonly StringName TrySendSyncMessage = StringName.op_Implicit(nameof (TrySendSyncMessage));
    public static readonly StringName SendSyncMessage = StringName.op_Implicit(nameof (SendSyncMessage));
    public static readonly StringName UpdateLocalCursor = StringName.op_Implicit(nameof (UpdateLocalCursor));
    public static readonly StringName RepositionBasedOnBackground = StringName.op_Implicit(nameof (RepositionBasedOnBackground));
  }

  public class PropertyName : Control.PropertyName
  {
    public static readonly StringName _lineDrawScene = StringName.op_Implicit(nameof (_lineDrawScene));
    public static readonly StringName _lineEraseScene = StringName.op_Implicit(nameof (_lineEraseScene));
    public static readonly StringName _cursorManager = StringName.op_Implicit(nameof (_cursorManager));
    public static readonly StringName _eraserMaterial = StringName.op_Implicit(nameof (_eraserMaterial));
    public static readonly StringName _defaultSize = StringName.op_Implicit(nameof (_defaultSize));
    public static readonly StringName _lastMessageMsec = StringName.op_Implicit(nameof (_lastMessageMsec));
  }

  public class SignalName : Control.SignalName
  {
  }
}
