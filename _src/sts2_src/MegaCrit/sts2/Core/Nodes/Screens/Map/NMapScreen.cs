// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Screens.Map.NMapScreen
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Audio.Debug;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.ControllerInput;
using MegaCrit.Sts2.Core.Debug;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Hooks;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Map;
using MegaCrit.Sts2.Core.Multiplayer.Game;
using MegaCrit.Sts2.Core.Multiplayer.Game.PeerInput;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Nodes.Ftue;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.Screens.Capstones;
using MegaCrit.Sts2.Core.Nodes.Screens.ScreenContext;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.Random;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.Saves;
using MegaCrit.Sts2.Core.Settings;
using MegaCrit.Sts2.Core.TestSupport;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Screens.Map;

[ScriptPath("res://src/Core/Nodes/Screens/Map/NMapScreen.cs")]
public class NMapScreen : Control, IScreenContext, INetCursorPositionTranslator
{
  private ActMap _map = (ActMap) NullActMap.Instance;
  private Control _mapContainer;
  private Control _pathsContainer;
  private Control _points;
  private NBossMapPoint? _bossPointNode;
  private NBossMapPoint? _secondBossPointNode;
  private NMapPoint? _startingPointNode;
  private NMapBg _mapBgContainer;
  private NMapMarker _marker;
  private NBackButton _backButton;
  private TextureRect _drawingToolsHotkeyIcon;
  private Control _drawingTools;
  private NMapDrawButton _mapDrawingButton;
  private NMapEraseButton _mapErasingButton;
  private NMapClearButton _mapClearButton;
  private Control _mapLegend;
  private Control _legendItems;
  private TextureRect _legendHotkeyIcon;
  private Control _backstop;
  private Tween? _tween;
  private Vector2 _startDragPos;
  private Vector2 _targetDragPos;
  private bool _isDragging;
  private bool _hasPlayedAnimation;
  private readonly Dictionary<MapCoord, NMapPoint> _mapPointDictionary = new Dictionary<MapCoord, NMapPoint>();
  private readonly Dictionary<(MapCoord, MapCoord), IReadOnlyList<TextureRect>> _paths = new Dictionary<(MapCoord, MapCoord), IReadOnlyList<TextureRect>>();
  private float _controllerScrollAmount = 400f;
  private const float _scrollLimitTop = 1800f;
  private const float _scrollLimitBottom = -600f;
  private const float _totalHeight = 2325f;
  private const float _totalWidth = 1050f;
  private float _distX;
  private float _distY;
  private const float _pointJitterX = 21f;
  private const float _pointJitterY = 25f;
  private const float _tickDist = 22f;
  private const float _pathPosJitter = 3f;
  private const float _pathAngleJitter = 0.1f;
  private static readonly Vector2 _tickTraveledScale = Vector2.op_Multiply(Vector2.One, 1.2f);
  private Tween? _actAnimTween;
  private float _mapScrollAnimTimer;
  private const string _mapTickScenePath = "res://scenes/ui/map_dot.tscn";
  private readonly double _mapAnimStartDelay = SaveManager.Instance.PrefsSave.FastMode == FastModeType.Fast ? 0.5 : 1.0;
  private readonly double _mapAnimDuration = SaveManager.Instance.PrefsSave.FastMode == FastModeType.Fast ? 1.5 : 3.0;
  private bool _canInterruptAnim;
  private bool _isInputDisabled;
  private RunState _runState;
  private Tween? _promptTween;
  private NMapDrawingInput? _drawingInput;
  private 
  #nullable disable
  NMapScreen.OpenedEventHandler backing_Opened;
  private NMapScreen.ClosedEventHandler backing_Closed;

  public static 
  #nullable enable
  NMapScreen? Instance => NRun.Instance?.GlobalUi.MapScreen;

  public bool IsOpen { get; private set; }

  public bool IsTravelEnabled { get; private set; }

  public bool IsDebugTravelEnabled { get; private set; }

  private float MapLegendX => this.Size.X * 0.8f;

  public bool IsTraveling { get; set; }

  public event Action<MapPointType>? PointTypeHighlighted;

  public Dictionary<Player, MapCoord?> PlayerVoteDictionary { get; } = new Dictionary<Player, MapCoord?>();

  public NMapDrawings Drawings { get; private set; }

  public override void _Ready()
  {
    ((Node) this).GetNode<MegaLabel>(NodePath.op_Implicit("MapLegend/Header")).SetTextAutoSize(new LocString("map", "LEGEND_HEADER").GetFormattedText());
    this._mapContainer = ((Node) this).GetNode<Control>(NodePath.op_Implicit("TheMap"));
    this._mapBgContainer = ((Node) this).GetNode<NMapBg>(NodePath.op_Implicit("%MapBg"));
    this._pathsContainer = ((Node) this).GetNode<Control>(NodePath.op_Implicit("TheMap/Paths"));
    this._points = ((Node) this).GetNode<Control>(NodePath.op_Implicit("TheMap/Points"));
    this._marker = ((Node) this).GetNode<NMapMarker>(NodePath.op_Implicit("TheMap/MapMarker"));
    this.Drawings = ((Node) this).GetNode<NMapDrawings>(NodePath.op_Implicit("TheMap/Drawings"));
    this._backButton = ((Node) this).GetNode<NBackButton>(NodePath.op_Implicit("Back"));
    ((GodotObject) this._backButton).Connect(NClickableControl.SignalName.Released, Callable.From<NButton>(new Action<NButton>(this.OnBackButtonPressed)), 0U);
    this._backButton.Disable();
    this._mapLegend = ((Node) this).GetNode<Control>(NodePath.op_Implicit("MapLegend"));
    this._legendItems = ((Node) this).GetNode<Control>(NodePath.op_Implicit("MapLegend/LegendItems"));
    this._legendHotkeyIcon = ((Node) this).GetNode<TextureRect>(NodePath.op_Implicit("MapLegend/LegendHotkeyIcon"));
    this._drawingToolsHotkeyIcon = ((Node) this).GetNode<TextureRect>(NodePath.op_Implicit("DrawingToolsHotkey"));
    this._backstop = ((Node) this).GetNode<Control>(NodePath.op_Implicit("%Backstop"));
    this._drawingTools = ((Node) this).GetNode<Control>(NodePath.op_Implicit("%DrawingTools"));
    this._mapDrawingButton = ((Node) this).GetNode<NMapDrawButton>(NodePath.op_Implicit("%DrawButton"));
    ((GodotObject) this._mapDrawingButton).Connect(NClickableControl.SignalName.Released, Callable.From<NButton>(new Action<NButton>(this.OnMapDrawingButtonPressed)), 0U);
    this._mapErasingButton = ((Node) this).GetNode<NMapEraseButton>(NodePath.op_Implicit("%EraseButton"));
    ((GodotObject) this._mapErasingButton).Connect(NClickableControl.SignalName.Released, Callable.From<NButton>(new Action<NButton>(this.OnMapErasingButtonPressed)), 0U);
    this._mapClearButton = ((Node) this).GetNode<NMapClearButton>(NodePath.op_Implicit("%ClearButton"));
    ((GodotObject) this._mapClearButton).Connect(NClickableControl.SignalName.Released, Callable.From<NButton>(new Action<NButton>(this.OnClearMapDrawingButtonPressed)), 0U);
    RunManager.Instance.MapSelectionSynchronizer.PlayerVoteChanged += new Action<Player, MapVote?, MapVote?>(this.OnPlayerVoteChanged);
    RunManager.Instance.MapSelectionSynchronizer.PlayerVoteCancelled += new Action<Player>(this.OnPlayerVoteCancelled);
    ((Node) this).ProcessMode = ((CanvasItem) this).Visible ? (Node.ProcessModeEnum) 0L : (Node.ProcessModeEnum) 4L;
    ((GodotObject) this).Connect(CanvasItem.SignalName.VisibilityChanged, Callable.From(new Action(this.OnVisibilityChanged)), 0U);
    Callable callable = Callable.From<Error>((Func<Error>) (() => ((GodotObject) NCapstoneContainer.Instance).Connect(NCapstoneContainer.SignalName.Changed, Callable.From(new Action(this.OnCapstoneChanged)), 0U)));
    ((Callable) ref callable).CallDeferred(Array.Empty<Variant>());
    List<NMapLegendItem> list = ((IEnumerable) ((Node) this._legendItems).GetChildren(false)).OfType<NMapLegendItem>().ToList<NMapLegendItem>();
    for (int index = 0; index < list.Count; ++index)
    {
      list[index].FocusNeighborTop = index > 0 ? ((Node) list[index - 1]).GetPath() : ((Node) list[index]).GetPath();
      list[index].FocusNeighborBottom = index < list.Count - 1 ? ((Node) list[index + 1]).GetPath() : ((Node) list[index]).GetPath();
      list[index].FocusNeighborRight = ((Node) list[index]).GetPath();
    }
    ((GodotObject) NControllerManager.Instance).Connect(NControllerManager.SignalName.MouseDetected, Callable.From(new Action(this.UpdateHotkeyDisplay)), 0U);
    ((GodotObject) NControllerManager.Instance).Connect(NControllerManager.SignalName.ControllerDetected, Callable.From(new Action(this.UpdateHotkeyDisplay)), 0U);
    ((GodotObject) NInputManager.Instance).Connect(NInputManager.SignalName.InputRebound, Callable.From(new Action(this.UpdateHotkeyDisplay)), 0U);
    this.UpdateHotkeyDisplay();
  }

  public override void _ExitTree()
  {
    RunManager.Instance.MapSelectionSynchronizer.PlayerVoteChanged -= new Action<Player, MapVote?, MapVote?>(this.OnPlayerVoteChanged);
    RunManager.Instance.MapSelectionSynchronizer.PlayerVoteCancelled -= new Action<Player>(this.OnPlayerVoteCancelled);
  }

  public void Initialize(RunState runState)
  {
    this._runState = runState;
    this.Drawings.Initialize(RunManager.Instance.NetService, (IPlayerCollection) this._runState, RunManager.Instance.InputSynchronizer);
    this._marker.Initialize(LocalContext.GetMe((IPlayerCollection) this._runState));
    this._mapBgContainer.Initialize((IRunState) this._runState);
  }

  public void SetMap(ActMap map, ulong seed, bool clearDrawings)
  {
    this._map = map;
    this._mapPointDictionary.Clear();
    this._paths.Clear();
    this.RemoveAllMapPointsAndPaths();
    this._marker.ResetMapPoint();
    if (clearDrawings)
      this.Drawings.ClearAllLines();
    this._hasPlayedAnimation = false;
    int rowCount = map.GetRowCount();
    int columnCount = map.GetColumnCount();
    float num1 = map.SecondBossMapPoint != null ? 0.9f : 1f;
    this._distY = 2325f / (float) (rowCount - 1) * num1;
    this._distX = 1050f / (float) columnCount;
    Rng rng = new Rng(seed, $"map_jitter_{this._runState.CurrentActIndex}");
    Vector2 vector2_1;
    // ISSUE: explicit constructor call
    ((Vector2) ref vector2_1).\u002Ector(-500f, 740f);
    Vector2 vector2_2;
    // ISSUE: explicit constructor call
    ((Vector2) ref vector2_2).\u002Ector(this._distX, -this._distY);
    foreach (MapPoint allMapPoint in map.GetAllMapPoints())
    {
      NNormalMapPoint child = NNormalMapPoint.Create(allMapPoint, this, (IRunState) this._runState);
      child.Position = Vector2.op_Addition(Vector2.op_Multiply(new Vector2((float) allMapPoint.coord.col, (float) allMapPoint.coord.row), vector2_2), vector2_1);
      float num2 = rng.NextFloat(-21f, 21f);
      float num3 = rng.NextFloat(-25f, 25f);
      NNormalMapPoint nnormalMapPoint = child;
      nnormalMapPoint.Position = Vector2.op_Addition(nnormalMapPoint.Position, new Vector2(num2, num3));
      this._mapPointDictionary.Add(allMapPoint.coord, (NMapPoint) child);
      ((Node) this._points).AddChildSafely((Node) child);
      child.SetAngle(Rng.Chaotic.NextGaussianFloat(stdDev: 8f));
    }
    this._bossPointNode = NBossMapPoint.Create(map.BossMapPoint, this, (IRunState) this._runState);
    this._bossPointNode.Position = new Vector2(-200f, -1980f * num1);
    ((Node) this._points).AddChildSafely((Node) this._bossPointNode);
    this._mapPointDictionary[map.BossMapPoint.coord] = (NMapPoint) this._bossPointNode;
    if (map.SecondBossMapPoint != null)
    {
      this._bossPointNode.Scale = new Vector2(0.75f, 0.75f);
      this._secondBossPointNode = NBossMapPoint.Create(map.SecondBossMapPoint, this, (IRunState) this._runState);
      this._secondBossPointNode.Position = new Vector2(-200f, -2280f * num1);
      this._secondBossPointNode.Scale = new Vector2(0.75f, 0.75f);
      ((Node) this._points).AddChildSafely((Node) this._secondBossPointNode);
      this._mapPointDictionary[map.SecondBossMapPoint.coord] = (NMapPoint) this._secondBossPointNode;
    }
    if (map.StartingMapPoint.PointType == MapPointType.Ancient)
    {
      this._startingPointNode = (NMapPoint) NAncientMapPoint.Create(map.StartingMapPoint, this, (IRunState) this._runState);
      this._startingPointNode.Position = new Vector2(-80f, (float) ((double) map.StartingMapPoint.coord.row * -(double) this._distY + 720.0));
    }
    else
    {
      this._startingPointNode = (NMapPoint) NNormalMapPoint.Create(map.StartingMapPoint, this, (IRunState) this._runState);
      this._startingPointNode.Position = new Vector2(-80f, (float) ((double) map.StartingMapPoint.coord.row * -(double) this._distY + 800.0));
    }
    ((Node) this._points).AddChildSafely((Node) this._startingPointNode);
    this._mapPointDictionary[map.StartingMapPoint.coord] = this._startingPointNode;
    foreach (MapPoint allMapPoint in map.GetAllMapPoints())
      this.DrawPaths(this._mapPointDictionary[allMapPoint.coord], allMapPoint);
    this.DrawPaths(this._startingPointNode, map.StartingMapPoint);
    this.DrawPaths((NMapPoint) this._bossPointNode, map.BossMapPoint);
    IReadOnlyList<MapCoord> visitedMapCoords = this._runState.VisitedMapCoords;
    for (int index = 0; index < visitedMapCoords.Count - 1; ++index)
    {
      IReadOnlyList<TextureRect> textureRectList;
      if (this._paths.TryGetValue((visitedMapCoords[index], visitedMapCoords[index + 1]), out textureRectList))
      {
        foreach (TextureRect textureRect in (IEnumerable<TextureRect>) textureRectList)
        {
          ((CanvasItem) textureRect).Modulate = this._runState.Act.MapTraveledColor;
          ((Control) textureRect).Scale = NMapScreen._tickTraveledScale;
        }
      }
    }
    this.InitMapVotes();
    this.RefreshAllMapPointVotes();
    for (int row = 0; row < map.GetRowCount(); ++row)
    {
      List<NMapPoint> list = map.GetPointsInRow(row).Select<MapPoint, NMapPoint>((Func<MapPoint, NMapPoint>) (p => this._mapPointDictionary[p.coord])).ToList<NMapPoint>();
      for (int index = 0; index < list.Count; ++index)
      {
        list[index].FocusNeighborLeft = index > 0 ? ((Node) list[index - 1]).GetPath() : ((Node) list[index]).GetPath();
        list[index].FocusNeighborRight = index < list.Count - 1 ? ((Node) list[index + 1]).GetPath() : ((Node) list[index]).GetPath();
        list[index].FocusNeighborTop = ((Node) list[index]).GetPath();
        list[index].FocusNeighborBottom = ((Node) list[index]).GetPath();
      }
    }
    this._startingPointNode.FocusNeighborLeft = ((Node) this._startingPointNode).GetPath();
    this._startingPointNode.FocusNeighborRight = ((Node) this._startingPointNode).GetPath();
    this._startingPointNode.FocusNeighborTop = ((Node) this._startingPointNode).GetPath();
    this._startingPointNode.FocusNeighborBottom = ((Node) this._startingPointNode).GetPath();
    this._bossPointNode.FocusNeighborLeft = ((Node) this._bossPointNode).GetPath();
    this._bossPointNode.FocusNeighborRight = ((Node) this._bossPointNode).GetPath();
    this._bossPointNode.FocusNeighborBottom = ((Node) this._bossPointNode).GetPath();
    if (this._secondBossPointNode != null)
    {
      this._bossPointNode.FocusNeighborTop = ((Node) this._secondBossPointNode).GetPath();
      this._secondBossPointNode.FocusNeighborBottom = ((Node) this._bossPointNode).GetPath();
      this._secondBossPointNode.FocusNeighborLeft = ((Node) this._secondBossPointNode).GetPath();
      this._secondBossPointNode.FocusNeighborRight = ((Node) this._secondBossPointNode).GetPath();
      this._secondBossPointNode.FocusNeighborTop = ((Node) this._secondBossPointNode).GetPath();
    }
    else
      this._bossPointNode.FocusNeighborTop = ((Node) this._bossPointNode).GetPath();
    if (!((CanvasItem) this).IsVisible())
      return;
    this.RecalculateTravelability();
    this.RefreshAllPointVisuals();
  }

  private void DrawPaths(NMapPoint mapPointNode, MapPoint mapPoint)
  {
    foreach (MapPoint child in mapPoint.Children)
    {
      NMapPoint point;
      if (!this._mapPointDictionary.TryGetValue(child.coord, out point))
        throw new InvalidOperationException($"Map point child with coord {child.coord} is not in the map point dictionary!");
      IReadOnlyList<TextureRect> path = this.CreatePath(this.GetLineEndpoint(mapPointNode), this.GetLineEndpoint(point));
      this._paths.Add((mapPoint.coord, child.coord), path);
    }
  }

  private Vector2 GetLineEndpoint(NMapPoint point)
  {
    return point is NNormalMapPoint ? point.Position : Vector2.op_Addition(point.Position, Vector2.op_Multiply(point.Size, 0.5f));
  }

  private void RecalculateTravelability()
  {
    if (this._runState.VisitedMapCoords.Any<MapCoord>())
    {
      foreach (NMapPoint nmapPoint in this._mapPointDictionary.Values)
        nmapPoint.State = MapPointState.Untravelable;
      foreach (MapCoord visitedMapCoord in (IEnumerable<MapCoord>) this._runState.VisitedMapCoords)
      {
        NMapPoint nmapPoint;
        if (this._mapPointDictionary.TryGetValue(visitedMapCoord, out nmapPoint))
          nmapPoint.State = MapPointState.Traveled;
        else
          Log.Error($"VisitedMapCoord {visitedMapCoord} not found in map point dictionary, map may have been regenerated");
      }
      IReadOnlyList<MapCoord> visitedMapCoords = this._runState.VisitedMapCoords;
      MapCoord key = visitedMapCoords[visitedMapCoords.Count - 1];
      if (this._secondBossPointNode != null && key == this._bossPointNode.Point.coord)
        this._secondBossPointNode.State = MapPointState.Travelable;
      else if (key.row == this._map.GetRowCount() - 1)
      {
        this._bossPointNode.State = MapPointState.Travelable;
      }
      else
      {
        NMapPoint nmapPoint;
        if (!this._mapPointDictionary.TryGetValue(key, out nmapPoint))
        {
          Log.Error($"Last visited coord {key} not found in map, falling back to starting point");
          this._startingPointNode.State = MapPointState.Travelable;
        }
        else
        {
          foreach (MapPoint mapPoint in MapTravel.GetTravelablePointsFrom((IRunState) this._runState, nmapPoint.Point))
            this._mapPointDictionary[mapPoint.coord].State = MapPointState.Travelable;
        }
      }
    }
    else
      this._startingPointNode.State = MapPointState.Travelable;
  }

  private void InitMapVotes()
  {
    foreach (Player player in (IEnumerable<Player>) this._runState.Players)
    {
      MapVote? vote = RunManager.Instance.MapSelectionSynchronizer.GetVote(player);
      ref MapVote? local = ref vote;
      MapCoord? nullable = local.HasValue ? new MapCoord?(local.GetValueOrDefault().coord) : new MapCoord?();
      if (nullable.HasValue)
        this.OnPlayerVoteChangedInternal(player, new MapCoord?(), new MapCoord?(nullable.Value));
    }
  }

  public void OnMapPointSelectedLocally(NMapPoint point)
  {
    Player me = LocalContext.GetMe((IPlayerCollection) this._runState);
    MapCoord? nullable1;
    if (this.PlayerVoteDictionary.TryGetValue(me, out nullable1))
    {
      MapCoord? nullable2 = nullable1;
      MapCoord coord = point.Point.coord;
      if ((nullable2.HasValue ? (nullable2.GetValueOrDefault() != coord ? 1 : 0) : 1) == 0)
      {
        if (this._runState.Players.Count <= 1)
          return;
        RunManager.Instance.FlavorSynchronizer.SendMapPing(point.Point.coord);
        return;
      }
    }
    Player player = me;
    MapVote? vote = RunManager.Instance.MapSelectionSynchronizer.GetVote(me);
    ref MapVote? local = ref vote;
    MapCoord? oldCoord = local.HasValue ? new MapCoord?(local.GetValueOrDefault().coord) : new MapCoord?();
    MapCoord? newCoord = new MapCoord?(point.Point.coord);
    this.OnPlayerVoteChangedInternal(player, oldCoord, newCoord);
    MapLocation source = new MapLocation(this._runState.CurrentMapCoord, this._runState.CurrentActIndex);
    MapVote mapVote = new MapVote()
    {
      coord = point.Point.coord,
      mapGenerationCount = RunManager.Instance.MapSelectionSynchronizer.MapGenerationCount
    };
    RunManager.Instance.ActionQueueSynchronizer.RequestEnqueue((GameAction) new VoteForMapCoordAction(LocalContext.GetMe((IPlayerCollection) this._runState), source, new MapVote?(mapVote)));
  }

  private void OnPlayerVoteChanged(Player player, MapVote? oldLocation, MapVote? newLocation)
  {
    Log.Info($"Player vote changed for {player.NetId}: {oldLocation}->{newLocation}");
    if (LocalContext.IsMe(player))
      return;
    this.OnPlayerVoteChangedInternal(player, oldLocation?.coord, newLocation?.coord);
  }

  private void OnPlayerVoteCancelled(Player player)
  {
    Log.Info($"Player vote cancelled for {player.NetId}");
    this.OnPlayerVoteChangedInternal(player, this.PlayerVoteDictionary[player], new MapCoord?());
  }

  private void OnPlayerVoteChangedInternal(Player player, MapCoord? oldCoord, MapCoord? newCoord)
  {
    if (this._runState.Players.Count <= 1)
      return;
    this.PlayerVoteDictionary[player] = newCoord;
    MapLocation mapLocation;
    if (oldCoord.HasValue)
      this._mapPointDictionary[oldCoord.Value].VoteContainer.RefreshPlayerVotes();
    else if (this._runState.MapLocation.coord.HasValue)
    {
      Dictionary<MapCoord, NMapPoint> mapPointDictionary = this._mapPointDictionary;
      mapLocation = this._runState.MapLocation;
      MapCoord key = mapLocation.coord.Value;
      mapPointDictionary[key].VoteContainer.RefreshPlayerVotes();
    }
    if (newCoord.HasValue)
    {
      this._mapPointDictionary[newCoord.Value].VoteContainer.RefreshPlayerVotes();
    }
    else
    {
      mapLocation = this._runState.MapLocation;
      if (!mapLocation.coord.HasValue)
        return;
      Dictionary<MapCoord, NMapPoint> mapPointDictionary = this._mapPointDictionary;
      mapLocation = this._runState.MapLocation;
      MapCoord key = mapLocation.coord.Value;
      mapPointDictionary[key].VoteContainer.RefreshPlayerVotes();
    }
  }

  public void InitMarker(MapCoord coord)
  {
    this._marker.SetMapPoint(this._mapPointDictionary[coord]);
  }

  public async Task TravelToMapCoord(MapCoord coord)
  {
    this.IsTraveling = true;
    this.RecalculateTravelability();
    if (NCapstoneContainer.Instance.CurrentCapstoneScreen is NDeckViewScreen)
      NCapstoneContainer.Instance.Close();
    this._marker.HideMapPoint();
    this.IsTravelEnabled = false;
    await new MapSplitVoteAnimation(this, this._runState, this._mapPointDictionary).TryPlay(coord);
    NMapPoint node = this._mapPointDictionary[coord];
    node.OnSelected();
    float scaleMultiplier = 1f;
    switch (node)
    {
      case NAncientMapPoint _:
        scaleMultiplier = 1.5f;
        break;
      case NBossMapPoint _:
        scaleMultiplier = 2f;
        break;
    }
    NMapNodeSelectVfx child = NMapNodeSelectVfx.Create(scaleMultiplier);
    SfxCmd.Play("event:/sfx/ui/map/map_select");
    ((Node) node).AddChildSafely((Node) child);
    NMapNodeSelectVfx nmapNodeSelectVfx = child;
    nmapNodeSelectVfx.Position = Vector2.op_Addition(nmapNodeSelectVfx.Position, node.PivotOffset);
    ((Node) node).MoveChildSafely((Node) child, ((Node) node.VoteContainer).GetIndex(false));
    IReadOnlyList<MapCoord> visitedMapCoords = this._runState.VisitedMapCoords;
    SfxCmd.Play("event:/sfx/ui/wipe_map");
    Task fadeOutTask = TaskHelper.RunSafely(RunManager.Instance.FadeOut());
    if (visitedMapCoords.Any<MapCoord>())
    {
      Dictionary<(MapCoord, MapCoord), IReadOnlyList<TextureRect>> paths = this._paths;
      IReadOnlyList<MapCoord> mapCoordList = visitedMapCoords;
      (MapCoord, MapCoord) key = (mapCoordList[mapCoordList.Count - 1], node.Point.coord);
      IReadOnlyList<TextureRect> textureRectList;
      ref IReadOnlyList<TextureRect> local = ref textureRectList;
      if (paths.TryGetValue(key, out local))
      {
        float num;
        switch (SaveManager.Instance.PrefsSave.FastMode)
        {
          case FastModeType.Normal:
            num = 0.8f;
            break;
          case FastModeType.Fast:
            num = 0.3f;
            break;
          default:
            num = 0.0f;
            break;
        }
        float waitPerTick = num / (float) textureRectList.Count;
        foreach (TextureRect tick in (IEnumerable<TextureRect>) textureRectList)
        {
          await Cmd.Wait(waitPerTick);
          ((CanvasItem) tick).Modulate = StsColors.pathDotTraveled;
          ((Node) this).CreateTween().TweenProperty((GodotObject) tick, NodePath.op_Implicit("scale"), Variant.op_Implicit(NMapScreen._tickTraveledScale), 0.4).From(Variant.op_Implicit(Vector2.op_Multiply(Vector2.One, 1.7f))).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 7L);
        }
      }
    }
    this._marker.SetMapPoint(node);
    await fadeOutTask;
    await RunManager.Instance.EnterMapCoord(coord);
    TaskHelper.RunSafely(RunManager.Instance.FadeIn());
    this.RefreshAllPointVisuals();
    this.PlayerVoteDictionary.Clear();
    this.RefreshAllMapPointVotes();
    node = (NMapPoint) null;
    fadeOutTask = (Task) null;
  }

  public void RefreshAllMapPointVotes()
  {
    foreach (NMapPoint nmapPoint in this._mapPointDictionary.Values)
      nmapPoint.VoteContainer.RefreshPlayerVotes();
  }

  private void RemoveAllMapPointsAndPaths()
  {
    ((Node) this._points).FreeChildren();
    ((Node) this._pathsContainer).FreeChildren();
    NBossMapPoint bossPointNode = this._bossPointNode;
    if (bossPointNode != null)
      ((Node) bossPointNode).QueueFreeSafely();
    NBossMapPoint secondBossPointNode = this._secondBossPointNode;
    if (secondBossPointNode != null)
      ((Node) secondBossPointNode).QueueFreeSafely();
    NMapPoint startingPointNode = this._startingPointNode;
    if (startingPointNode == null)
      return;
    ((Node) startingPointNode).QueueFreeSafely();
  }

  private IReadOnlyList<TextureRect> CreatePath(Vector2 start, Vector2 end)
  {
    List<TextureRect> path = new List<TextureRect>();
    Vector2 vector2_1 = Vector2.op_Subtraction(end, start);
    Vector2 vector2_2 = ((Vector2) ref vector2_1).Normalized();
    float num1 = ((Vector2) ref vector2_2).Angle() + 1.57079637f;
    int num2 = (int) ((double) ((Vector2) ref start).DistanceTo(end) / 22.0) + 1;
    for (int index = 1; index < num2; ++index)
    {
      float num3 = (float) index * 22f;
      TextureRect child = PreloadManager.Cache.GetScene("res://scenes/ui/map_dot.tscn").Instantiate<TextureRect>((PackedScene.GenEditState) 0L);
      ((Control) child).Position = Vector2.op_Addition(start, Vector2.op_Multiply(vector2_2, num3));
      TextureRect textureRect1 = child;
      ((Control) textureRect1).Position = Vector2.op_Subtraction(((Control) textureRect1).Position, new Vector2((float) ((double) this.Size.X * 0.5 - 20.0), (float) ((double) this.Size.Y * 0.5 - 20.0)));
      TextureRect textureRect2 = child;
      ((Control) textureRect2).Position = Vector2.op_Addition(((Control) textureRect2).Position, new Vector2(Rng.Chaotic.NextFloat(-3f, 3f), Rng.Chaotic.NextFloat(-3f, 3f)));
      child.FlipH = Rng.Chaotic.NextBool();
      ((Control) child).Rotation = num1 + Rng.Chaotic.NextGaussianFloat(stdDev: 0.1f);
      ((CanvasItem) child).Modulate = this._runState.Act.MapUntraveledColor;
      ((Node) this._pathsContainer).AddChildSafely((Node) child);
      path.Add(child);
    }
    return (IReadOnlyList<TextureRect>) path;
  }

  public static IEnumerable<string> AssetPaths
  {
    get => NMapDrawings.AssetPaths.Append<string>("res://scenes/ui/map_dot.tscn");
  }

  public override void _Process(double delta)
  {
    if (!((CanvasItem) this).IsVisibleInTree() || this._actAnimTween != null && this._actAnimTween.IsRunning())
      return;
    this.UpdateScrollPosition(delta);
  }

  private void UpdateScrollPosition(double delta)
  {
    if (Vector2.op_Inequality(this._mapContainer.Position, this._targetDragPos))
    {
      float num1 = (float) Mathf.Sign(this._mapContainer.Position.Y - this._targetDragPos.Y);
      Control mapContainer = this._mapContainer;
      Vector2 position = this._mapContainer.Position;
      Vector2 vector2 = ((Vector2) ref position).Lerp(this._targetDragPos, (float) delta * 15f);
      mapContainer.Position = vector2;
      float num2 = (float) Mathf.Sign(this._mapContainer.Position.Y - this._targetDragPos.Y);
      if ((double) Math.Abs(this._mapContainer.Position.Y - this._targetDragPos.Y) < 0.5 || !Mathf.IsEqualApprox(num1, num2))
        this._mapContainer.Position = this._targetDragPos;
    }
    if (!this._isDragging)
    {
      if ((double) this._targetDragPos.Y < -600.0)
        this._targetDragPos = ((Vector2) ref this._targetDragPos).Lerp(new Vector2(0.0f, -600f), (float) delta * 12f);
      else if ((double) this._targetDragPos.Y > 1800.0)
        this._targetDragPos = ((Vector2) ref this._targetDragPos).Lerp(new Vector2(0.0f, 1800f), (float) delta * 12f);
    }
    NGame.Instance.RemoteCursorContainer.ForceUpdateAllCursors();
  }

  public override void _GuiInput(InputEvent inputEvent)
  {
    if (!((CanvasItem) this).IsVisibleInTree())
      return;
    this.ProcessMouseEvent(inputEvent);
    this.ProcessScrollEvent(inputEvent);
  }

  private void ProcessMouseEvent(InputEvent inputEvent)
  {
    this.ProcessMouseDrawingEvent(inputEvent);
    if (this._drawingInput != null)
      return;
    if (this._isDragging && inputEvent is InputEventMouseMotion eventMouseMotion1)
      this._targetDragPos = Vector2.op_Addition(this._targetDragPos, new Vector2(0.0f, eventMouseMotion1.Relative.Y));
    else if (inputEvent is InputEventMouseButton eventMouseButton)
    {
      if (eventMouseButton.ButtonIndex == 1L)
      {
        if (eventMouseButton.Pressed && this.CanScroll())
        {
          this._isDragging = true;
          this._startDragPos = this._mapContainer.Position;
          this._targetDragPos = this._startDragPos;
          this.TryCancelStartOfActAnim();
        }
        else
          this._isDragging = false;
      }
      else if (!eventMouseButton.Pressed)
        this._isDragging = false;
    }
    if (!(inputEvent is InputEventMouseMotion eventMouseMotion2) || !this.Drawings.IsLocalDrawing())
      return;
    NMapDrawings drawings = this.Drawings;
    Transform2D globalTransform = ((CanvasItem) this.Drawings).GetGlobalTransform();
    Vector2 position = Transform2D.op_Multiply(((Transform2D) ref globalTransform).Inverse(), ((InputEventMouse) eventMouseMotion2).GlobalPosition);
    drawings.UpdateCurrentLinePositionLocal(position);
  }

  private void ProcessMouseDrawingEvent(InputEvent inputEvent)
  {
    if (this._isInputDisabled || this._actAnimTween != null && this._actAnimTween.IsRunning() || this._drawingInput != null || !(inputEvent is InputEventMouseButton eventMouseButton) || !eventMouseButton.Pressed)
      return;
    if (eventMouseButton.ButtonIndex == 2L)
      this._drawingInput = NMapDrawingInput.Create(this.Drawings, DrawingMode.Drawing, true);
    else if (eventMouseButton.ButtonIndex == 3L)
      this._drawingInput = NMapDrawingInput.Create(this.Drawings, DrawingMode.Erasing, true);
    ((GodotObject) this._drawingInput)?.Connect(NMapDrawingInput.SignalName.Finished, Callable.From((Action) (() =>
    {
      this._drawingInput = (NMapDrawingInput) null;
      this.UpdateDrawingButtonStates();
    })), 0U);
    ((Node) this).AddChildSafely((Node) this._drawingInput);
  }

  private void ProcessScrollEvent(InputEvent inputEvent)
  {
    if (!this.CanScroll())
      return;
    this._targetDragPos = Vector2.op_Addition(this._targetDragPos, new Vector2(0.0f, ScrollHelper.GetDragForScrollEvent(inputEvent)));
    bool flag;
    switch (inputEvent)
    {
      case InputEventMouseButton _:
      case InputEventPanGesture _:
        flag = true;
        break;
      default:
        flag = false;
        break;
    }
    if (!flag)
      return;
    this.TryCancelStartOfActAnim();
  }

  private void ProcessControllerEvent(InputEvent inputEvent)
  {
    if (inputEvent.IsActionPressed(MegaInput.up, false, false) && this.CanScroll())
    {
      this._targetDragPos = Vector2.op_Addition(this._targetDragPos, new Vector2(0.0f, this._controllerScrollAmount));
      this.TryCancelStartOfActAnim();
    }
    else if (inputEvent.IsActionPressed(MegaInput.down, false, false) && this.CanScroll())
    {
      this._targetDragPos = Vector2.op_Addition(this._targetDragPos, new Vector2(0.0f, -this._controllerScrollAmount));
      this.TryCancelStartOfActAnim();
    }
    else
    {
      if (!inputEvent.IsActionPressed(MegaInput.right, false, false) && !inputEvent.IsActionPressed(MegaInput.left, false, false) && !inputEvent.IsActionPressed(MegaInput.select, false, false))
        return;
      if (this._runState.ActFloor == 0)
      {
        this._targetDragPos = new Vector2(0.0f, -600f);
      }
      else
      {
        MapCoord? currentMapCoord = this._runState.CurrentMapCoord;
        ref MapCoord? local = ref currentMapCoord;
        this._targetDragPos = new Vector2(0.0f, (float) ((local.HasValue ? (double) local.GetValueOrDefault().row : 0.0) * (double) this._distY - 600.0));
      }
    }
  }

  public void SetTravelEnabled(bool enabled)
  {
    this.IsTravelEnabled = enabled && Hook.ShouldProceedToNextMapPoint((IRunState) this._runState);
    this.RefreshAllPointVisuals();
  }

  public void SetDebugTravelEnabled(bool enabled)
  {
    this.IsDebugTravelEnabled = enabled;
    this.RefreshAllPointVisuals();
  }

  public void RefreshAllPointVisuals()
  {
    foreach (NMapPoint nmapPoint in this._mapPointDictionary.Values)
      nmapPoint.RefreshVisualsInstantly();
    NMapPoint control = this._mapPointDictionary.Values.FirstOrDefault<NMapPoint>((Func<NMapPoint, bool>) (n => n.IsEnabled));
    if (control == null)
      return;
    control.TryGrabFocus();
  }

  private void PlayStartOfActAnimation()
  {
    if (this._hasPlayedAnimation)
    {
      Log.Warn("Tried to play start of act animation twice! Ignoring second try");
    }
    else
    {
      this._hasPlayedAnimation = true;
      NActBanner child = NActBanner.Create(this._runState.Act, this._runState.CurrentActIndex);
      NRun instance = NRun.Instance;
      if (instance != null)
        ((Node) instance.GlobalUi.MapScreen).AddChildSafely((Node) child);
      TaskHelper.RunSafely(this.StartOfActAnim());
    }
  }

  private async Task StartOfActAnim()
  {
    this._mapContainer.Position = new Vector2(0.0f, 1800f);
    this._actAnimTween?.Kill();
    this._actAnimTween = ((Node) this).CreateTween().SetParallel(true);
    this._actAnimTween.TweenInterval(this._mapAnimStartDelay);
    this._actAnimTween.Chain();
    Vector2 vector2;
    // ISSUE: explicit constructor call
    ((Vector2) ref vector2).\u002Ector(0.0f, -600f);
    this._actAnimTween.TweenProperty((GodotObject) this._mapContainer, NodePath.op_Implicit("position:y"), Variant.op_Implicit(-600f), this._mapAnimDuration).SetEase((Tween.EaseType) 2L).SetTrans((Tween.TransitionType) 5L);
    this._actAnimTween.TweenCallback(Callable.From(new Action(this.SetInterruptable))).SetDelay(this._mapAnimDuration * 0.25);
    this._targetDragPos = vector2;
    if (!await this._actAnimTween.AwaitFinished((Node) this))
      return;
    this._actAnimTween = (Tween) null;
    this.InitMapPrompt();
  }

  private void InitMapPrompt()
  {
    if (TestMode.IsOn || SaveManager.Instance.SeenFtue("map_select_ftue"))
      return;
    TaskHelper.RunSafely(this.MapFtueCheck());
  }

  private async Task MapFtueCheck()
  {
    await Task.Delay(100);
    NMapSelectFtue modalToCreate = NMapSelectFtue.Create((Control) this._startingPointNode);
    NModalContainer.Instance.Add((Node) modalToCreate);
    SaveManager.Instance.MarkFtueAsComplete("map_select_ftue");
    await modalToCreate.WaitForPlayerToConfirm();
  }

  private void SetInterruptable() => this._canInterruptAnim = true;

  private bool CanScroll()
  {
    return (this._actAnimTween == null || this._canInterruptAnim) && !this._isInputDisabled;
  }

  private void TryCancelStartOfActAnim()
  {
    if (this._actAnimTween == null || !this._canInterruptAnim)
      return;
    this._actAnimTween?.Kill();
    this._actAnimTween = (Tween) null;
    this._canInterruptAnim = false;
    this._isDragging = false;
    this._targetDragPos = new Vector2(0.0f, -600f);
    TaskHelper.RunSafely(this.DisableInputVeryBriefly());
  }

  private async Task DisableInputVeryBriefly()
  {
    this._isInputDisabled = true;
    this._drawingInput?.StopDrawing();
    await Task.Delay(200);
    this._isInputDisabled = false;
    this.InitMapPrompt();
  }

  private void OnVisibilityChanged()
  {
    if (((CanvasItem) this).Visible)
    {
      RunManager.Instance.InputSynchronizer.StartOverridingCursorPositioning((INetCursorPositionTranslator) this);
    }
    else
    {
      this._isDragging = false;
      RunManager.Instance.InputSynchronizer.StopOverridingCursorPositioning();
      this._backButton.Disable();
      this.Drawings.StopLineLocal();
      this.Drawings.SetDrawingModeLocal(DrawingMode.None);
      this._drawingInput?.StopDrawing();
      this.UpdateDrawingButtonStates();
    }
  }

  private void OnCapstoneChanged()
  {
    Control backstop = this._backstop;
    NCapstoneContainer instance = NCapstoneContainer.Instance;
    int num = (instance != null ? (instance.InUse ? 1 : 0) : 0) == 0 ? 1 : 0;
    ((CanvasItem) backstop).Visible = num != 0;
    if (!((CanvasItem) this).Visible)
      return;
    if (!((CanvasItem) this._backstop).Visible)
      NRun.Instance.GlobalUi.TopBar.Map.StopOscillation();
    else
      NRun.Instance.GlobalUi.TopBar.Map.StartOscillation();
  }

  public void Close(bool animateOut = true)
  {
    if (!this.IsOpen)
      return;
    this.IsOpen = false;
    this.FocusMode = (Control.FocusModeEnum) 0L;
    NRun.Instance.GlobalUi.TopBar.Map.StopOscillation();
    NHotkeyManager.Instance.RemoveHotkeyPressedBinding(StringName.op_Implicit(MegaInput.accept), new Action(this.OnLegendHotkeyPressed));
    NHotkeyManager.Instance.RemoveHotkeyPressedBinding(StringName.op_Implicit(MegaInput.viewExhaustPileAndTabRight), new Action(this.OnDrawingToolsHotkeyPressed));
    if (RunManager.Instance.IsSingleplayerOrFakeMultiplayer)
      CombatManager.Instance.Unpause();
    this._backButton.Disable();
    ActiveScreenContext.Instance.Update();
    this.EmitSignalClosed();
    if (animateOut)
    {
      TaskHelper.RunSafely(this.AnimClose());
      SfxCmd.Play("event:/sfx/ui/map/map_close");
    }
    else
    {
      ((CanvasItem) this).Visible = false;
      ((Node) this).ProcessMode = (Node.ProcessModeEnum) 4L;
    }
  }

  private async Task AnimClose()
  {
    this._tween?.Kill();
    this._tween = ((Node) this).CreateTween().SetParallel(true);
    this._tween.TweenProperty((GodotObject) this._backstop, NodePath.op_Implicit("modulate:a"), Variant.op_Implicit(0.0f), 0.15);
    this._tween.TweenProperty((GodotObject) this._points, NodePath.op_Implicit("modulate:a"), Variant.op_Implicit(0.0f), 0.15);
    this._tween.TweenProperty((GodotObject) this._mapContainer, NodePath.op_Implicit("modulate"), Variant.op_Implicit(StsColors.transparentBlack), 0.25).SetDelay(0.1);
    this._tween.TweenProperty((GodotObject) this._mapContainer, NodePath.op_Implicit("position:y"), Variant.op_Implicit(this._mapContainer.Position.Y + 200f), 0.25).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 7L);
    this._tween.TweenProperty((GodotObject) this._mapLegend, NodePath.op_Implicit("modulate:a"), Variant.op_Implicit(0.0f), 0.15);
    this._tween.TweenProperty((GodotObject) this._mapLegend, NodePath.op_Implicit("position:x"), Variant.op_Implicit(this.MapLegendX + 120f), 0.25).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 7L);
    this._tween.TweenProperty((GodotObject) this._drawingTools, NodePath.op_Implicit("modulate:a"), Variant.op_Implicit(0.0f), 0.15);
    if (!await this._tween.AwaitFinished((Node) this))
      return;
    ((CanvasItem) this).Visible = false;
    ((Node) this).ProcessMode = (Node.ProcessModeEnum) 4L;
  }

  public NMapScreen Open(bool isOpenedFromTopBar = false)
  {
    if (this.IsOpen)
      return this;
    this.IsOpen = true;
    ((CanvasItem) this).Visible = true;
    this._backButton.MoveToHidePosition();
    NHotkeyManager.Instance.PushHotkeyPressedBinding(StringName.op_Implicit(MegaInput.accept), new Action(this.OnLegendHotkeyPressed));
    NHotkeyManager.Instance.PushHotkeyPressedBinding(StringName.op_Implicit(MegaInput.viewExhaustPileAndTabRight), new Action(this.OnDrawingToolsHotkeyPressed));
    if (this._runState.ActFloor > 0)
      this._backButton.Enable();
    ((Node) this).ProcessMode = (Node.ProcessModeEnum) 0L;
    NRun.Instance.GlobalUi.TopBar.Map.StartOscillation();
    if (RunManager.Instance.IsSingleplayerOrFakeMultiplayer)
      CombatManager.Instance.Pause();
    if ((this._runState.CurrentActIndex != 0 || !this._runState.ExtraFields.StartedWithNeow ? this._runState.ActFloor == 0 : this._runState.ActFloor == 1) && !this._hasPlayedAnimation)
    {
      if (!isOpenedFromTopBar && (SaveManager.Instance.PrefsSave.FastMode < FastModeType.Fast || !SaveManager.Instance.SeenFtue("map_select_ftue")))
      {
        this.PlayStartOfActAnimation();
      }
      else
      {
        this._hasPlayedAnimation = true;
        Control mapContainer = this._mapContainer;
        Vector2 position = this._mapContainer.Position;
        position.Y = -600f;
        Vector2 vector2 = position;
        mapContainer.Position = vector2;
        this._targetDragPos = new Vector2(0.0f, -600f);
        ((Node) NRun.Instance.GlobalUi.MapScreen).AddChildSafely((Node) NActBanner.Create(this._runState.Act, this._runState.CurrentActIndex));
      }
    }
    else
    {
      MapCoord? currentMapCoord = this._runState.CurrentMapCoord;
      ref MapCoord? local = ref currentMapCoord;
      int row = local.HasValue ? local.GetValueOrDefault().row : 0;
      this._targetDragPos = new Vector2(0.0f, (float) ((double) row * (double) this._distY - 600.0));
      this._mapContainer.Position = new Vector2(0.0f, (float) ((double) row * (double) this._distY - 600.0));
      Control points = this._points;
      Color modulate1 = ((CanvasItem) this._points).Modulate;
      modulate1.A = 0.0f;
      Color color1 = modulate1;
      ((CanvasItem) points).Modulate = color1;
      Control backstop = this._backstop;
      Color modulate2 = ((CanvasItem) this._backstop).Modulate;
      modulate2.A = 0.0f;
      Color color2 = modulate2;
      ((CanvasItem) backstop).Modulate = color2;
      ((CanvasItem) this._mapLegend).Modulate = StsColors.transparentBlack;
      ((CanvasItem) this._drawingTools).Modulate = StsColors.transparentBlack;
    }
    Control mapLegend = this._mapLegend;
    Color modulate3 = ((CanvasItem) this._mapLegend).Modulate;
    modulate3.A = 0.0f;
    Color color3 = modulate3;
    ((CanvasItem) mapLegend).Modulate = color3;
    Control drawingTools = this._drawingTools;
    Color modulate4 = ((CanvasItem) this._drawingTools).Modulate;
    modulate4.A = 0.0f;
    Color color4 = modulate4;
    ((CanvasItem) drawingTools).Modulate = color4;
    this._tween?.Kill();
    this._tween = ((Node) this).CreateTween().SetParallel(true);
    this._tween.TweenProperty((GodotObject) this._backstop, NodePath.op_Implicit("modulate:a"), Variant.op_Implicit(0.85f), 0.25);
    this._tween.TweenProperty((GodotObject) this._mapContainer, NodePath.op_Implicit("modulate"), Variant.op_Implicit(Colors.White), 0.25).From(Variant.op_Implicit(StsColors.transparentBlack));
    this._tween.TweenProperty((GodotObject) this._mapLegend, NodePath.op_Implicit("modulate"), Variant.op_Implicit(Colors.White), 0.25).SetDelay(0.1);
    this._tween.TweenProperty((GodotObject) this._mapLegend, NodePath.op_Implicit("position:x"), Variant.op_Implicit(this.MapLegendX), 0.25).From(Variant.op_Implicit(this.MapLegendX + 120f)).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 10L).SetDelay(0.1);
    this._tween.TweenProperty((GodotObject) this._drawingTools, NodePath.op_Implicit("modulate"), Variant.op_Implicit(Colors.White), 0.25).SetDelay(0.2);
    this._tween.TweenProperty((GodotObject) this._points, NodePath.op_Implicit("modulate:a"), Variant.op_Implicit(1f), 0.25).SetDelay(0.1);
    this.RecalculateTravelability();
    if (this._runState.VisitedMapCoords.Count != 0)
    {
      IReadOnlyList<MapCoord> visitedMapCoords = this._runState.VisitedMapCoords;
      MapCoord key = visitedMapCoords[visitedMapCoords.Count - 1];
      if (this._bossPointNode.Point.coord.row != key.row && this._startingPointNode.Point.coord.row != key.row)
      {
        NMapPoint node;
        if (this._mapPointDictionary.TryGetValue(key, out node))
          this._marker.SetMapPoint(node);
        else
          Log.Error($"Last visited coord {key} not found in map, marker not placed");
      }
    }
    SfxCmd.Play("event:/sfx/ui/map/map_open");
    ActiveScreenContext.Instance.Update();
    this.EmitSignalOpened();
    if (this._mapPointDictionary.Values.FirstOrDefault<NMapPoint>((Func<NMapPoint, bool>) (n => n.IsEnabled)) == null)
      this.FocusMode = (Control.FocusModeEnum) 2L;
    return this;
  }

  private void OnBackButtonPressed(NButton _) => this.Close();

  public override void _Input(InputEvent inputEvent)
  {
    if (!(((Node) this).GetViewport().GuiGetFocusOwner() is NMapPoint) && !this.HasFocus() || !ActiveScreenContext.Instance.IsCurrent((IScreenContext) this))
      return;
    if (inputEvent.IsActionReleased(DebugHotkey.unlockCharacters, false))
    {
      ((CanvasItem) this._mapLegend).Visible = !((CanvasItem) this._mapLegend).Visible;
      ((Node) NGame.Instance).AddChildSafely((Node) NFullscreenTextVfx.Create(((CanvasItem) this._mapLegend).Visible ? "Show Legend" : "Hide Legend"));
    }
    if (!((CanvasItem) this).IsVisibleInTree())
      return;
    this.ProcessControllerEvent(inputEvent);
  }

  private void OnMapDrawingButtonPressed(NButton _)
  {
    NMapDrawingInput drawingInput = this._drawingInput;
    if (drawingInput != null && drawingInput.DrawingMode == DrawingMode.Drawing)
    {
      this._drawingInput?.StopDrawing();
    }
    else
    {
      this._drawingInput?.StopDrawing();
      this._drawingInput = NMapDrawingInput.Create(this.Drawings, DrawingMode.Drawing);
      ((GodotObject) this._drawingInput).Connect(NMapDrawingInput.SignalName.Finished, Callable.From((Action) (() =>
      {
        this._drawingInput = (NMapDrawingInput) null;
        this.UpdateDrawingButtonStates();
      })), 0U);
      ((Node) this).AddChildSafely((Node) this._drawingInput);
    }
    this.UpdateDrawingButtonStates();
  }

  private void OnMapErasingButtonPressed(NButton _)
  {
    NMapDrawingInput drawingInput = this._drawingInput;
    if (drawingInput != null && drawingInput.DrawingMode == DrawingMode.Erasing)
    {
      this._drawingInput?.StopDrawing();
    }
    else
    {
      this._drawingInput?.StopDrawing();
      this._drawingInput = NMapDrawingInput.Create(this.Drawings, DrawingMode.Erasing);
      ((GodotObject) this._drawingInput).Connect(NMapDrawingInput.SignalName.Finished, Callable.From((Action) (() =>
      {
        this._drawingInput = (NMapDrawingInput) null;
        this.UpdateDrawingButtonStates();
      })), 0U);
      ((Node) this).AddChildSafely((Node) this._drawingInput);
    }
    this.UpdateDrawingButtonStates();
  }

  private void UpdateDrawingButtonStates()
  {
    this._mapDrawingButton.SetIsDrawing(this.Drawings.GetLocalDrawingMode() == DrawingMode.Drawing);
    this._mapErasingButton.SetIsErasing(this.Drawings.GetLocalDrawingMode() == DrawingMode.Erasing);
  }

  private void OnClearMapDrawingButtonPressed(NButton _)
  {
    this.Drawings.ClearDrawnLinesLocal();
    SfxCmd.Play("event:/sfx/ui/map/map_erase");
    this.UpdateDrawingButtonStates();
  }

  public void HighlightPointType(MapPointType pointType)
  {
    Action<MapPointType> pointTypeHighlighted = this.PointTypeHighlighted;
    if (pointTypeHighlighted == null)
      return;
    pointTypeHighlighted(pointType);
  }

  public Control DefaultFocusedControl
  {
    get
    {
      return (Control) this._mapPointDictionary.Values.FirstOrDefault<NMapPoint>((Func<NMapPoint, bool>) (n => n.IsEnabled)) ?? (Control) this;
    }
  }

  public void PingMapCoord(MapCoord coord, Player player)
  {
    NMapPoint parent;
    if (!this._mapPointDictionary.TryGetValue(coord, out parent))
    {
      Log.Error($"Someone tried to ping map coord {coord} that doesn't exist!");
    }
    else
    {
      NMapPingVfx child = NMapPingVfx.Create();
      ((CanvasItem) child).Modulate = player.Character.MapDrawingColor;
      ((Node) parent).AddChildSafely((Node) child);
      ((Node) parent).MoveChildSafely((Node) child, 0);
      child.Position = Vector2.Zero;
      NMapPingVfx nmapPingVfx = child;
      nmapPingVfx.Size = Vector2.op_Multiply(nmapPingVfx.Size, parent.Size.X * (1f / 64f));
      child.PivotOffset = Vector2.op_Multiply(child.Size, 0.5f);
      NRun.Instance.GlobalUi.MultiplayerPlayerContainer.FlashPlayerReady(player);
      NDebugAudioManager.Instance.Play("map_ping.mp3", variance: PitchVariance.Medium);
    }
  }

  private void OnLegendHotkeyPressed()
  {
    List<NMapLegendItem> list = ((IEnumerable) ((Node) this._legendItems).GetChildren(false)).OfType<NMapLegendItem>().ToList<NMapLegendItem>();
    if (list.Any<NMapLegendItem>((Func<NMapLegendItem, bool>) (c => ((Node) this).GetViewport().GuiGetFocusOwner() == c)))
    {
      NMapPoint control = this._mapPointDictionary.Values.FirstOrDefault<NMapPoint>((Func<NMapPoint, bool>) (n => n.IsEnabled));
      if (control == null)
        return;
      control.TryGrabFocus();
    }
    else
    {
      NMapPoint nmapPoint = this._mapPointDictionary.Values.LastOrDefault<NMapPoint>((Func<NMapPoint, bool>) (n => n.IsEnabled));
      if (nmapPoint != null)
      {
        foreach (NMapLegendItem nmapLegendItem in list)
        {
          if (nmapPoint != null)
            nmapLegendItem.FocusNeighborLeft = ((Node) nmapPoint).GetPath();
          else
            nmapLegendItem.FocusNeighborLeft = ((Node) this).GetPath();
        }
      }
      list[0].TryGrabFocus();
    }
  }

  private void OnDrawingToolsHotkeyPressed()
  {
    NMapDrawingInput drawingInput = this._drawingInput;
    if (drawingInput != null && drawingInput.DrawingMode == DrawingMode.Erasing)
      this._mapErasingButton.TryGrabFocus();
    else
      this._mapDrawingButton.TryGrabFocus();
  }

  public Vector2 GetNetPositionFromScreenPosition(Vector2 screenPosition)
  {
    Transform2D transformWithCanvas = ((CanvasItem) this._mapBgContainer).GetGlobalTransformWithCanvas();
    Vector2 vector2_1 = Transform2D.op_Multiply(((Transform2D) ref transformWithCanvas).Inverse(), screenPosition);
    Vector2 vector2_2 = Vector2.op_Multiply(((Control) this._mapBgContainer).Size, 0.5f);
    Vector2 vector2_3;
    // ISSUE: explicit constructor call
    ((Vector2) ref vector2_3).\u002Ector(960f, vector2_2.Y);
    return Vector2.op_Division(Vector2.op_Subtraction(vector2_1, vector2_2), vector2_3);
  }

  private Vector2 GetMapPositionFromNetPosition(Vector2 netPosition)
  {
    Vector2 vector2_1 = Vector2.op_Multiply(((Control) this._mapBgContainer).Size, 0.5f);
    Vector2 vector2_2;
    // ISSUE: explicit constructor call
    ((Vector2) ref vector2_2).\u002Ector(960f, vector2_1.Y);
    return Vector2.op_Addition(Vector2.op_Multiply(netPosition, vector2_2), vector2_1);
  }

  public Vector2 GetScreenPositionFromNetPosition(Vector2 netPosition)
  {
    Vector2 positionFromNetPosition = this.GetMapPositionFromNetPosition(netPosition);
    return Transform2D.op_Multiply(((CanvasItem) this._mapBgContainer).GetGlobalTransformWithCanvas(), positionFromNetPosition);
  }

  public bool IsNodeOnScreen(NMapPoint mapPoint)
  {
    float y = mapPoint.GlobalPosition.Y;
    return (double) y > 0.0 && (double) y < (double) this.Size.Y;
  }

  public void CleanUp()
  {
    if (!RunManager.Instance.IsSingleplayerOrFakeMultiplayer)
      return;
    CombatManager.Instance.Unpause();
  }

  private void UpdateHotkeyDisplay()
  {
    ((CanvasItem) this._legendHotkeyIcon).Visible = NControllerManager.Instance.IsUsingController;
    this._legendHotkeyIcon.Texture = NInputManager.Instance.GetHotkeyIcon(StringName.op_Implicit(MegaInput.accept));
    ((CanvasItem) this._drawingToolsHotkeyIcon).Visible = NControllerManager.Instance.IsUsingController;
    this._drawingToolsHotkeyIcon.Texture = NInputManager.Instance.GetHotkeyIcon(StringName.op_Implicit(MegaInput.viewExhaustPileAndTabRight));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(42)
    {
      new MethodInfo(NMapScreen.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NMapScreen.MethodName._ExitTree, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NMapScreen.MethodName.GetLineEndpoint, new PropertyInfo((Variant.Type) 5L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("point"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false)
      }, (List<Variant>) null),
      new MethodInfo(NMapScreen.MethodName.RecalculateTravelability, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NMapScreen.MethodName.InitMapVotes, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NMapScreen.MethodName.OnMapPointSelectedLocally, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("point"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false)
      }, (List<Variant>) null),
      new MethodInfo(NMapScreen.MethodName.RefreshAllMapPointVotes, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NMapScreen.MethodName.RemoveAllMapPointsAndPaths, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NMapScreen.MethodName._Process, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 3L, StringName.op_Implicit("delta"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NMapScreen.MethodName.UpdateScrollPosition, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 3L, StringName.op_Implicit("delta"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NMapScreen.MethodName._GuiInput, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("inputEvent"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("InputEvent"), false)
      }, (List<Variant>) null),
      new MethodInfo(NMapScreen.MethodName.ProcessMouseEvent, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("inputEvent"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("InputEvent"), false)
      }, (List<Variant>) null),
      new MethodInfo(NMapScreen.MethodName.ProcessMouseDrawingEvent, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("inputEvent"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("InputEvent"), false)
      }, (List<Variant>) null),
      new MethodInfo(NMapScreen.MethodName.ProcessScrollEvent, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("inputEvent"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("InputEvent"), false)
      }, (List<Variant>) null),
      new MethodInfo(NMapScreen.MethodName.ProcessControllerEvent, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("inputEvent"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("InputEvent"), false)
      }, (List<Variant>) null),
      new MethodInfo(NMapScreen.MethodName.SetTravelEnabled, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 1L, StringName.op_Implicit("enabled"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NMapScreen.MethodName.SetDebugTravelEnabled, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 1L, StringName.op_Implicit("enabled"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NMapScreen.MethodName.RefreshAllPointVisuals, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NMapScreen.MethodName.PlayStartOfActAnimation, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NMapScreen.MethodName.InitMapPrompt, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NMapScreen.MethodName.SetInterruptable, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NMapScreen.MethodName.CanScroll, new PropertyInfo((Variant.Type) 1L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NMapScreen.MethodName.TryCancelStartOfActAnim, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NMapScreen.MethodName.OnVisibilityChanged, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NMapScreen.MethodName.OnCapstoneChanged, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NMapScreen.MethodName.Close, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 1L, StringName.op_Implicit("animateOut"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NMapScreen.MethodName.Open, new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 1L, StringName.op_Implicit("isOpenedFromTopBar"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NMapScreen.MethodName.OnBackButtonPressed, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("_"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false)
      }, (List<Variant>) null),
      new MethodInfo(NMapScreen.MethodName._Input, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("inputEvent"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("InputEvent"), false)
      }, (List<Variant>) null),
      new MethodInfo(NMapScreen.MethodName.OnMapDrawingButtonPressed, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("_"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false)
      }, (List<Variant>) null),
      new MethodInfo(NMapScreen.MethodName.OnMapErasingButtonPressed, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("_"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false)
      }, (List<Variant>) null),
      new MethodInfo(NMapScreen.MethodName.UpdateDrawingButtonStates, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NMapScreen.MethodName.OnClearMapDrawingButtonPressed, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("_"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false)
      }, (List<Variant>) null),
      new MethodInfo(NMapScreen.MethodName.HighlightPointType, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 2L, StringName.op_Implicit("pointType"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NMapScreen.MethodName.OnLegendHotkeyPressed, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NMapScreen.MethodName.OnDrawingToolsHotkeyPressed, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NMapScreen.MethodName.GetNetPositionFromScreenPosition, new PropertyInfo((Variant.Type) 5L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 5L, StringName.op_Implicit("screenPosition"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NMapScreen.MethodName.GetMapPositionFromNetPosition, new PropertyInfo((Variant.Type) 5L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 5L, StringName.op_Implicit("netPosition"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NMapScreen.MethodName.GetScreenPositionFromNetPosition, new PropertyInfo((Variant.Type) 5L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 5L, StringName.op_Implicit("netPosition"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NMapScreen.MethodName.IsNodeOnScreen, new PropertyInfo((Variant.Type) 1L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("mapPoint"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false)
      }, (List<Variant>) null),
      new MethodInfo(NMapScreen.MethodName.CleanUp, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NMapScreen.MethodName.UpdateHotkeyDisplay, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NMapScreen.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NMapScreen.MethodName._ExitTree) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._ExitTree();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NMapScreen.MethodName.GetLineEndpoint) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      Vector2 lineEndpoint = this.GetLineEndpoint(VariantUtils.ConvertTo<NMapPoint>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = VariantUtils.CreateFrom<Vector2>(ref lineEndpoint);
      return true;
    }
    if (StringName.op_Equality(ref method, NMapScreen.MethodName.RecalculateTravelability) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.RecalculateTravelability();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NMapScreen.MethodName.InitMapVotes) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.InitMapVotes();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NMapScreen.MethodName.OnMapPointSelectedLocally) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.OnMapPointSelectedLocally(VariantUtils.ConvertTo<NMapPoint>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NMapScreen.MethodName.RefreshAllMapPointVotes) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.RefreshAllMapPointVotes();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NMapScreen.MethodName.RemoveAllMapPointsAndPaths) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.RemoveAllMapPointsAndPaths();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NMapScreen.MethodName._Process) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      ((Node) this)._Process(VariantUtils.ConvertTo<double>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NMapScreen.MethodName.UpdateScrollPosition) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.UpdateScrollPosition(VariantUtils.ConvertTo<double>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NMapScreen.MethodName._GuiInput) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      base._GuiInput(VariantUtils.ConvertTo<InputEvent>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NMapScreen.MethodName.ProcessMouseEvent) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.ProcessMouseEvent(VariantUtils.ConvertTo<InputEvent>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NMapScreen.MethodName.ProcessMouseDrawingEvent) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.ProcessMouseDrawingEvent(VariantUtils.ConvertTo<InputEvent>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NMapScreen.MethodName.ProcessScrollEvent) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.ProcessScrollEvent(VariantUtils.ConvertTo<InputEvent>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NMapScreen.MethodName.ProcessControllerEvent) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.ProcessControllerEvent(VariantUtils.ConvertTo<InputEvent>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NMapScreen.MethodName.SetTravelEnabled) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.SetTravelEnabled(VariantUtils.ConvertTo<bool>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NMapScreen.MethodName.SetDebugTravelEnabled) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.SetDebugTravelEnabled(VariantUtils.ConvertTo<bool>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NMapScreen.MethodName.RefreshAllPointVisuals) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.RefreshAllPointVisuals();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NMapScreen.MethodName.PlayStartOfActAnimation) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.PlayStartOfActAnimation();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NMapScreen.MethodName.InitMapPrompt) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.InitMapPrompt();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NMapScreen.MethodName.SetInterruptable) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.SetInterruptable();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NMapScreen.MethodName.CanScroll) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      bool flag = this.CanScroll();
      ret = VariantUtils.CreateFrom<bool>(ref flag);
      return true;
    }
    if (StringName.op_Equality(ref method, NMapScreen.MethodName.TryCancelStartOfActAnim) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.TryCancelStartOfActAnim();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NMapScreen.MethodName.OnVisibilityChanged) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnVisibilityChanged();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NMapScreen.MethodName.OnCapstoneChanged) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnCapstoneChanged();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NMapScreen.MethodName.Close) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.Close(VariantUtils.ConvertTo<bool>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NMapScreen.MethodName.Open) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      NMapScreen nmapScreen = this.Open(VariantUtils.ConvertTo<bool>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = VariantUtils.CreateFrom<NMapScreen>(ref nmapScreen);
      return true;
    }
    if (StringName.op_Equality(ref method, NMapScreen.MethodName.OnBackButtonPressed) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.OnBackButtonPressed(VariantUtils.ConvertTo<NButton>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NMapScreen.MethodName._Input) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      ((Node) this)._Input(VariantUtils.ConvertTo<InputEvent>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NMapScreen.MethodName.OnMapDrawingButtonPressed) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.OnMapDrawingButtonPressed(VariantUtils.ConvertTo<NButton>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NMapScreen.MethodName.OnMapErasingButtonPressed) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.OnMapErasingButtonPressed(VariantUtils.ConvertTo<NButton>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NMapScreen.MethodName.UpdateDrawingButtonStates) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.UpdateDrawingButtonStates();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NMapScreen.MethodName.OnClearMapDrawingButtonPressed) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.OnClearMapDrawingButtonPressed(VariantUtils.ConvertTo<NButton>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NMapScreen.MethodName.HighlightPointType) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.HighlightPointType(VariantUtils.ConvertTo<MapPointType>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NMapScreen.MethodName.OnLegendHotkeyPressed) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnLegendHotkeyPressed();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NMapScreen.MethodName.OnDrawingToolsHotkeyPressed) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnDrawingToolsHotkeyPressed();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NMapScreen.MethodName.GetNetPositionFromScreenPosition) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      Vector2 fromScreenPosition = this.GetNetPositionFromScreenPosition(VariantUtils.ConvertTo<Vector2>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = VariantUtils.CreateFrom<Vector2>(ref fromScreenPosition);
      return true;
    }
    if (StringName.op_Equality(ref method, NMapScreen.MethodName.GetMapPositionFromNetPosition) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      Vector2 positionFromNetPosition = this.GetMapPositionFromNetPosition(VariantUtils.ConvertTo<Vector2>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = VariantUtils.CreateFrom<Vector2>(ref positionFromNetPosition);
      return true;
    }
    if (StringName.op_Equality(ref method, NMapScreen.MethodName.GetScreenPositionFromNetPosition) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      Vector2 positionFromNetPosition = this.GetScreenPositionFromNetPosition(VariantUtils.ConvertTo<Vector2>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = VariantUtils.CreateFrom<Vector2>(ref positionFromNetPosition);
      return true;
    }
    if (StringName.op_Equality(ref method, NMapScreen.MethodName.IsNodeOnScreen) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      bool flag = this.IsNodeOnScreen(VariantUtils.ConvertTo<NMapPoint>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = VariantUtils.CreateFrom<bool>(ref flag);
      return true;
    }
    if (StringName.op_Equality(ref method, NMapScreen.MethodName.CleanUp) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.CleanUp();
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NMapScreen.MethodName.UpdateHotkeyDisplay) || ((NativeVariantPtrArgs) ref args).Count != 0)
      return base.InvokeGodotClassMethod(ref method, args, ref ret);
    this.UpdateHotkeyDisplay();
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NMapScreen.MethodName._Ready) || StringName.op_Equality(ref method, NMapScreen.MethodName._ExitTree) || StringName.op_Equality(ref method, NMapScreen.MethodName.GetLineEndpoint) || StringName.op_Equality(ref method, NMapScreen.MethodName.RecalculateTravelability) || StringName.op_Equality(ref method, NMapScreen.MethodName.InitMapVotes) || StringName.op_Equality(ref method, NMapScreen.MethodName.OnMapPointSelectedLocally) || StringName.op_Equality(ref method, NMapScreen.MethodName.RefreshAllMapPointVotes) || StringName.op_Equality(ref method, NMapScreen.MethodName.RemoveAllMapPointsAndPaths) || StringName.op_Equality(ref method, NMapScreen.MethodName._Process) || StringName.op_Equality(ref method, NMapScreen.MethodName.UpdateScrollPosition) || StringName.op_Equality(ref method, NMapScreen.MethodName._GuiInput) || StringName.op_Equality(ref method, NMapScreen.MethodName.ProcessMouseEvent) || StringName.op_Equality(ref method, NMapScreen.MethodName.ProcessMouseDrawingEvent) || StringName.op_Equality(ref method, NMapScreen.MethodName.ProcessScrollEvent) || StringName.op_Equality(ref method, NMapScreen.MethodName.ProcessControllerEvent) || StringName.op_Equality(ref method, NMapScreen.MethodName.SetTravelEnabled) || StringName.op_Equality(ref method, NMapScreen.MethodName.SetDebugTravelEnabled) || StringName.op_Equality(ref method, NMapScreen.MethodName.RefreshAllPointVisuals) || StringName.op_Equality(ref method, NMapScreen.MethodName.PlayStartOfActAnimation) || StringName.op_Equality(ref method, NMapScreen.MethodName.InitMapPrompt) || StringName.op_Equality(ref method, NMapScreen.MethodName.SetInterruptable) || StringName.op_Equality(ref method, NMapScreen.MethodName.CanScroll) || StringName.op_Equality(ref method, NMapScreen.MethodName.TryCancelStartOfActAnim) || StringName.op_Equality(ref method, NMapScreen.MethodName.OnVisibilityChanged) || StringName.op_Equality(ref method, NMapScreen.MethodName.OnCapstoneChanged) || StringName.op_Equality(ref method, NMapScreen.MethodName.Close) || StringName.op_Equality(ref method, NMapScreen.MethodName.Open) || StringName.op_Equality(ref method, NMapScreen.MethodName.OnBackButtonPressed) || StringName.op_Equality(ref method, NMapScreen.MethodName._Input) || StringName.op_Equality(ref method, NMapScreen.MethodName.OnMapDrawingButtonPressed) || StringName.op_Equality(ref method, NMapScreen.MethodName.OnMapErasingButtonPressed) || StringName.op_Equality(ref method, NMapScreen.MethodName.UpdateDrawingButtonStates) || StringName.op_Equality(ref method, NMapScreen.MethodName.OnClearMapDrawingButtonPressed) || StringName.op_Equality(ref method, NMapScreen.MethodName.HighlightPointType) || StringName.op_Equality(ref method, NMapScreen.MethodName.OnLegendHotkeyPressed) || StringName.op_Equality(ref method, NMapScreen.MethodName.OnDrawingToolsHotkeyPressed) || StringName.op_Equality(ref method, NMapScreen.MethodName.GetNetPositionFromScreenPosition) || StringName.op_Equality(ref method, NMapScreen.MethodName.GetMapPositionFromNetPosition) || StringName.op_Equality(ref method, NMapScreen.MethodName.GetScreenPositionFromNetPosition) || StringName.op_Equality(ref method, NMapScreen.MethodName.IsNodeOnScreen) || StringName.op_Equality(ref method, NMapScreen.MethodName.CleanUp) || StringName.op_Equality(ref method, NMapScreen.MethodName.UpdateHotkeyDisplay) || base.HasGodotClassMethod(ref method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NMapScreen.PropertyName.IsOpen))
    {
      this.IsOpen = VariantUtils.ConvertTo<bool>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NMapScreen.PropertyName.IsTravelEnabled))
    {
      this.IsTravelEnabled = VariantUtils.ConvertTo<bool>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NMapScreen.PropertyName.IsDebugTravelEnabled))
    {
      this.IsDebugTravelEnabled = VariantUtils.ConvertTo<bool>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NMapScreen.PropertyName.IsTraveling))
    {
      this.IsTraveling = VariantUtils.ConvertTo<bool>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NMapScreen.PropertyName.Drawings))
    {
      this.Drawings = VariantUtils.ConvertTo<NMapDrawings>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NMapScreen.PropertyName._mapContainer))
    {
      this._mapContainer = VariantUtils.ConvertTo<Control>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NMapScreen.PropertyName._pathsContainer))
    {
      this._pathsContainer = VariantUtils.ConvertTo<Control>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NMapScreen.PropertyName._points))
    {
      this._points = VariantUtils.ConvertTo<Control>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NMapScreen.PropertyName._bossPointNode))
    {
      this._bossPointNode = VariantUtils.ConvertTo<NBossMapPoint>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NMapScreen.PropertyName._secondBossPointNode))
    {
      this._secondBossPointNode = VariantUtils.ConvertTo<NBossMapPoint>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NMapScreen.PropertyName._startingPointNode))
    {
      this._startingPointNode = VariantUtils.ConvertTo<NMapPoint>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NMapScreen.PropertyName._mapBgContainer))
    {
      this._mapBgContainer = VariantUtils.ConvertTo<NMapBg>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NMapScreen.PropertyName._marker))
    {
      this._marker = VariantUtils.ConvertTo<NMapMarker>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NMapScreen.PropertyName._backButton))
    {
      this._backButton = VariantUtils.ConvertTo<NBackButton>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NMapScreen.PropertyName._drawingToolsHotkeyIcon))
    {
      this._drawingToolsHotkeyIcon = VariantUtils.ConvertTo<TextureRect>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NMapScreen.PropertyName._drawingTools))
    {
      this._drawingTools = VariantUtils.ConvertTo<Control>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NMapScreen.PropertyName._mapDrawingButton))
    {
      this._mapDrawingButton = VariantUtils.ConvertTo<NMapDrawButton>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NMapScreen.PropertyName._mapErasingButton))
    {
      this._mapErasingButton = VariantUtils.ConvertTo<NMapEraseButton>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NMapScreen.PropertyName._mapClearButton))
    {
      this._mapClearButton = VariantUtils.ConvertTo<NMapClearButton>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NMapScreen.PropertyName._mapLegend))
    {
      this._mapLegend = VariantUtils.ConvertTo<Control>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NMapScreen.PropertyName._legendItems))
    {
      this._legendItems = VariantUtils.ConvertTo<Control>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NMapScreen.PropertyName._legendHotkeyIcon))
    {
      this._legendHotkeyIcon = VariantUtils.ConvertTo<TextureRect>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NMapScreen.PropertyName._backstop))
    {
      this._backstop = VariantUtils.ConvertTo<Control>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NMapScreen.PropertyName._tween))
    {
      this._tween = VariantUtils.ConvertTo<Tween>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NMapScreen.PropertyName._startDragPos))
    {
      this._startDragPos = VariantUtils.ConvertTo<Vector2>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NMapScreen.PropertyName._targetDragPos))
    {
      this._targetDragPos = VariantUtils.ConvertTo<Vector2>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NMapScreen.PropertyName._isDragging))
    {
      this._isDragging = VariantUtils.ConvertTo<bool>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NMapScreen.PropertyName._hasPlayedAnimation))
    {
      this._hasPlayedAnimation = VariantUtils.ConvertTo<bool>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NMapScreen.PropertyName._controllerScrollAmount))
    {
      this._controllerScrollAmount = VariantUtils.ConvertTo<float>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NMapScreen.PropertyName._distX))
    {
      this._distX = VariantUtils.ConvertTo<float>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NMapScreen.PropertyName._distY))
    {
      this._distY = VariantUtils.ConvertTo<float>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NMapScreen.PropertyName._actAnimTween))
    {
      this._actAnimTween = VariantUtils.ConvertTo<Tween>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NMapScreen.PropertyName._mapScrollAnimTimer))
    {
      this._mapScrollAnimTimer = VariantUtils.ConvertTo<float>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NMapScreen.PropertyName._canInterruptAnim))
    {
      this._canInterruptAnim = VariantUtils.ConvertTo<bool>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NMapScreen.PropertyName._isInputDisabled))
    {
      this._isInputDisabled = VariantUtils.ConvertTo<bool>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NMapScreen.PropertyName._promptTween))
    {
      this._promptTween = VariantUtils.ConvertTo<Tween>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NMapScreen.PropertyName._drawingInput))
      return ((GodotObject) this).SetGodotClassPropertyValue(ref name, ref value);
    this._drawingInput = VariantUtils.ConvertTo<NMapDrawingInput>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NMapScreen.PropertyName.IsOpen))
    {
      ref godot_variant local = ref value;
      bool isOpen = this.IsOpen;
      godot_variant from = VariantUtils.CreateFrom<bool>(ref isOpen);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NMapScreen.PropertyName.IsTravelEnabled))
    {
      ref godot_variant local = ref value;
      bool isTravelEnabled = this.IsTravelEnabled;
      godot_variant from = VariantUtils.CreateFrom<bool>(ref isTravelEnabled);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NMapScreen.PropertyName.IsDebugTravelEnabled))
    {
      ref godot_variant local = ref value;
      bool debugTravelEnabled = this.IsDebugTravelEnabled;
      godot_variant from = VariantUtils.CreateFrom<bool>(ref debugTravelEnabled);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NMapScreen.PropertyName.MapLegendX))
    {
      ref godot_variant local = ref value;
      float mapLegendX = this.MapLegendX;
      godot_variant from = VariantUtils.CreateFrom<float>(ref mapLegendX);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NMapScreen.PropertyName.IsTraveling))
    {
      ref godot_variant local = ref value;
      bool isTraveling = this.IsTraveling;
      godot_variant from = VariantUtils.CreateFrom<bool>(ref isTraveling);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NMapScreen.PropertyName.Drawings))
    {
      ref godot_variant local = ref value;
      NMapDrawings drawings = this.Drawings;
      godot_variant from = VariantUtils.CreateFrom<NMapDrawings>(ref drawings);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NMapScreen.PropertyName.DefaultFocusedControl))
    {
      ref godot_variant local = ref value;
      Control defaultFocusedControl = this.DefaultFocusedControl;
      godot_variant from = VariantUtils.CreateFrom<Control>(ref defaultFocusedControl);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NMapScreen.PropertyName._mapContainer))
    {
      value = VariantUtils.CreateFrom<Control>(ref this._mapContainer);
      return true;
    }
    if (StringName.op_Equality(ref name, NMapScreen.PropertyName._pathsContainer))
    {
      value = VariantUtils.CreateFrom<Control>(ref this._pathsContainer);
      return true;
    }
    if (StringName.op_Equality(ref name, NMapScreen.PropertyName._points))
    {
      value = VariantUtils.CreateFrom<Control>(ref this._points);
      return true;
    }
    if (StringName.op_Equality(ref name, NMapScreen.PropertyName._bossPointNode))
    {
      value = VariantUtils.CreateFrom<NBossMapPoint>(ref this._bossPointNode);
      return true;
    }
    if (StringName.op_Equality(ref name, NMapScreen.PropertyName._secondBossPointNode))
    {
      value = VariantUtils.CreateFrom<NBossMapPoint>(ref this._secondBossPointNode);
      return true;
    }
    if (StringName.op_Equality(ref name, NMapScreen.PropertyName._startingPointNode))
    {
      value = VariantUtils.CreateFrom<NMapPoint>(ref this._startingPointNode);
      return true;
    }
    if (StringName.op_Equality(ref name, NMapScreen.PropertyName._mapBgContainer))
    {
      value = VariantUtils.CreateFrom<NMapBg>(ref this._mapBgContainer);
      return true;
    }
    if (StringName.op_Equality(ref name, NMapScreen.PropertyName._marker))
    {
      value = VariantUtils.CreateFrom<NMapMarker>(ref this._marker);
      return true;
    }
    if (StringName.op_Equality(ref name, NMapScreen.PropertyName._backButton))
    {
      value = VariantUtils.CreateFrom<NBackButton>(ref this._backButton);
      return true;
    }
    if (StringName.op_Equality(ref name, NMapScreen.PropertyName._drawingToolsHotkeyIcon))
    {
      value = VariantUtils.CreateFrom<TextureRect>(ref this._drawingToolsHotkeyIcon);
      return true;
    }
    if (StringName.op_Equality(ref name, NMapScreen.PropertyName._drawingTools))
    {
      value = VariantUtils.CreateFrom<Control>(ref this._drawingTools);
      return true;
    }
    if (StringName.op_Equality(ref name, NMapScreen.PropertyName._mapDrawingButton))
    {
      value = VariantUtils.CreateFrom<NMapDrawButton>(ref this._mapDrawingButton);
      return true;
    }
    if (StringName.op_Equality(ref name, NMapScreen.PropertyName._mapErasingButton))
    {
      value = VariantUtils.CreateFrom<NMapEraseButton>(ref this._mapErasingButton);
      return true;
    }
    if (StringName.op_Equality(ref name, NMapScreen.PropertyName._mapClearButton))
    {
      value = VariantUtils.CreateFrom<NMapClearButton>(ref this._mapClearButton);
      return true;
    }
    if (StringName.op_Equality(ref name, NMapScreen.PropertyName._mapLegend))
    {
      value = VariantUtils.CreateFrom<Control>(ref this._mapLegend);
      return true;
    }
    if (StringName.op_Equality(ref name, NMapScreen.PropertyName._legendItems))
    {
      value = VariantUtils.CreateFrom<Control>(ref this._legendItems);
      return true;
    }
    if (StringName.op_Equality(ref name, NMapScreen.PropertyName._legendHotkeyIcon))
    {
      value = VariantUtils.CreateFrom<TextureRect>(ref this._legendHotkeyIcon);
      return true;
    }
    if (StringName.op_Equality(ref name, NMapScreen.PropertyName._backstop))
    {
      value = VariantUtils.CreateFrom<Control>(ref this._backstop);
      return true;
    }
    if (StringName.op_Equality(ref name, NMapScreen.PropertyName._tween))
    {
      value = VariantUtils.CreateFrom<Tween>(ref this._tween);
      return true;
    }
    if (StringName.op_Equality(ref name, NMapScreen.PropertyName._startDragPos))
    {
      value = VariantUtils.CreateFrom<Vector2>(ref this._startDragPos);
      return true;
    }
    if (StringName.op_Equality(ref name, NMapScreen.PropertyName._targetDragPos))
    {
      value = VariantUtils.CreateFrom<Vector2>(ref this._targetDragPos);
      return true;
    }
    if (StringName.op_Equality(ref name, NMapScreen.PropertyName._isDragging))
    {
      value = VariantUtils.CreateFrom<bool>(ref this._isDragging);
      return true;
    }
    if (StringName.op_Equality(ref name, NMapScreen.PropertyName._hasPlayedAnimation))
    {
      value = VariantUtils.CreateFrom<bool>(ref this._hasPlayedAnimation);
      return true;
    }
    if (StringName.op_Equality(ref name, NMapScreen.PropertyName._controllerScrollAmount))
    {
      value = VariantUtils.CreateFrom<float>(ref this._controllerScrollAmount);
      return true;
    }
    if (StringName.op_Equality(ref name, NMapScreen.PropertyName._distX))
    {
      value = VariantUtils.CreateFrom<float>(ref this._distX);
      return true;
    }
    if (StringName.op_Equality(ref name, NMapScreen.PropertyName._distY))
    {
      value = VariantUtils.CreateFrom<float>(ref this._distY);
      return true;
    }
    if (StringName.op_Equality(ref name, NMapScreen.PropertyName._actAnimTween))
    {
      value = VariantUtils.CreateFrom<Tween>(ref this._actAnimTween);
      return true;
    }
    if (StringName.op_Equality(ref name, NMapScreen.PropertyName._mapScrollAnimTimer))
    {
      value = VariantUtils.CreateFrom<float>(ref this._mapScrollAnimTimer);
      return true;
    }
    if (StringName.op_Equality(ref name, NMapScreen.PropertyName._mapAnimStartDelay))
    {
      value = VariantUtils.CreateFrom<double>(ref this._mapAnimStartDelay);
      return true;
    }
    if (StringName.op_Equality(ref name, NMapScreen.PropertyName._mapAnimDuration))
    {
      value = VariantUtils.CreateFrom<double>(ref this._mapAnimDuration);
      return true;
    }
    if (StringName.op_Equality(ref name, NMapScreen.PropertyName._canInterruptAnim))
    {
      value = VariantUtils.CreateFrom<bool>(ref this._canInterruptAnim);
      return true;
    }
    if (StringName.op_Equality(ref name, NMapScreen.PropertyName._isInputDisabled))
    {
      value = VariantUtils.CreateFrom<bool>(ref this._isInputDisabled);
      return true;
    }
    if (StringName.op_Equality(ref name, NMapScreen.PropertyName._promptTween))
    {
      value = VariantUtils.CreateFrom<Tween>(ref this._promptTween);
      return true;
    }
    if (!StringName.op_Equality(ref name, NMapScreen.PropertyName._drawingInput))
      return ((GodotObject) this).GetGodotClassPropertyValue(ref name, ref value);
    value = VariantUtils.CreateFrom<NMapDrawingInput>(ref this._drawingInput);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 1L, NMapScreen.PropertyName.IsOpen, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 1L, NMapScreen.PropertyName.IsTravelEnabled, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 1L, NMapScreen.PropertyName.IsDebugTravelEnabled, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NMapScreen.PropertyName._mapContainer, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NMapScreen.PropertyName._pathsContainer, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NMapScreen.PropertyName._points, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NMapScreen.PropertyName._bossPointNode, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NMapScreen.PropertyName._secondBossPointNode, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NMapScreen.PropertyName._startingPointNode, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NMapScreen.PropertyName._mapBgContainer, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NMapScreen.PropertyName._marker, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NMapScreen.PropertyName._backButton, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NMapScreen.PropertyName._drawingToolsHotkeyIcon, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NMapScreen.PropertyName._drawingTools, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NMapScreen.PropertyName._mapDrawingButton, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NMapScreen.PropertyName._mapErasingButton, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NMapScreen.PropertyName._mapClearButton, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NMapScreen.PropertyName._mapLegend, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NMapScreen.PropertyName._legendItems, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NMapScreen.PropertyName._legendHotkeyIcon, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NMapScreen.PropertyName._backstop, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NMapScreen.PropertyName._tween, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 5L, NMapScreen.PropertyName._startDragPos, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 5L, NMapScreen.PropertyName._targetDragPos, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 1L, NMapScreen.PropertyName._isDragging, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 1L, NMapScreen.PropertyName._hasPlayedAnimation, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 3L, NMapScreen.PropertyName.MapLegendX, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 1L, NMapScreen.PropertyName.IsTraveling, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 3L, NMapScreen.PropertyName._controllerScrollAmount, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 3L, NMapScreen.PropertyName._distX, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 3L, NMapScreen.PropertyName._distY, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NMapScreen.PropertyName._actAnimTween, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 3L, NMapScreen.PropertyName._mapScrollAnimTimer, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 3L, NMapScreen.PropertyName._mapAnimStartDelay, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 3L, NMapScreen.PropertyName._mapAnimDuration, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 1L, NMapScreen.PropertyName._canInterruptAnim, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 1L, NMapScreen.PropertyName._isInputDisabled, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NMapScreen.PropertyName._promptTween, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NMapScreen.PropertyName.Drawings, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NMapScreen.PropertyName._drawingInput, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NMapScreen.PropertyName.DefaultFocusedControl, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).SaveGodotObjectData(info);
    GodotSerializationInfo serializationInfo1 = info;
    StringName isOpen1 = NMapScreen.PropertyName.IsOpen;
    bool isOpen2 = this.IsOpen;
    Variant variant1 = Variant.From<bool>(ref isOpen2);
    serializationInfo1.AddProperty(isOpen1, variant1);
    GodotSerializationInfo serializationInfo2 = info;
    StringName isTravelEnabled1 = NMapScreen.PropertyName.IsTravelEnabled;
    bool isTravelEnabled2 = this.IsTravelEnabled;
    Variant variant2 = Variant.From<bool>(ref isTravelEnabled2);
    serializationInfo2.AddProperty(isTravelEnabled1, variant2);
    GodotSerializationInfo serializationInfo3 = info;
    StringName debugTravelEnabled1 = NMapScreen.PropertyName.IsDebugTravelEnabled;
    bool debugTravelEnabled2 = this.IsDebugTravelEnabled;
    Variant variant3 = Variant.From<bool>(ref debugTravelEnabled2);
    serializationInfo3.AddProperty(debugTravelEnabled1, variant3);
    GodotSerializationInfo serializationInfo4 = info;
    StringName isTraveling1 = NMapScreen.PropertyName.IsTraveling;
    bool isTraveling2 = this.IsTraveling;
    Variant variant4 = Variant.From<bool>(ref isTraveling2);
    serializationInfo4.AddProperty(isTraveling1, variant4);
    GodotSerializationInfo serializationInfo5 = info;
    StringName drawings1 = NMapScreen.PropertyName.Drawings;
    NMapDrawings drawings2 = this.Drawings;
    Variant variant5 = Variant.From<NMapDrawings>(ref drawings2);
    serializationInfo5.AddProperty(drawings1, variant5);
    info.AddProperty(NMapScreen.PropertyName._mapContainer, Variant.From<Control>(ref this._mapContainer));
    info.AddProperty(NMapScreen.PropertyName._pathsContainer, Variant.From<Control>(ref this._pathsContainer));
    info.AddProperty(NMapScreen.PropertyName._points, Variant.From<Control>(ref this._points));
    info.AddProperty(NMapScreen.PropertyName._bossPointNode, Variant.From<NBossMapPoint>(ref this._bossPointNode));
    info.AddProperty(NMapScreen.PropertyName._secondBossPointNode, Variant.From<NBossMapPoint>(ref this._secondBossPointNode));
    info.AddProperty(NMapScreen.PropertyName._startingPointNode, Variant.From<NMapPoint>(ref this._startingPointNode));
    info.AddProperty(NMapScreen.PropertyName._mapBgContainer, Variant.From<NMapBg>(ref this._mapBgContainer));
    info.AddProperty(NMapScreen.PropertyName._marker, Variant.From<NMapMarker>(ref this._marker));
    info.AddProperty(NMapScreen.PropertyName._backButton, Variant.From<NBackButton>(ref this._backButton));
    info.AddProperty(NMapScreen.PropertyName._drawingToolsHotkeyIcon, Variant.From<TextureRect>(ref this._drawingToolsHotkeyIcon));
    info.AddProperty(NMapScreen.PropertyName._drawingTools, Variant.From<Control>(ref this._drawingTools));
    info.AddProperty(NMapScreen.PropertyName._mapDrawingButton, Variant.From<NMapDrawButton>(ref this._mapDrawingButton));
    info.AddProperty(NMapScreen.PropertyName._mapErasingButton, Variant.From<NMapEraseButton>(ref this._mapErasingButton));
    info.AddProperty(NMapScreen.PropertyName._mapClearButton, Variant.From<NMapClearButton>(ref this._mapClearButton));
    info.AddProperty(NMapScreen.PropertyName._mapLegend, Variant.From<Control>(ref this._mapLegend));
    info.AddProperty(NMapScreen.PropertyName._legendItems, Variant.From<Control>(ref this._legendItems));
    info.AddProperty(NMapScreen.PropertyName._legendHotkeyIcon, Variant.From<TextureRect>(ref this._legendHotkeyIcon));
    info.AddProperty(NMapScreen.PropertyName._backstop, Variant.From<Control>(ref this._backstop));
    info.AddProperty(NMapScreen.PropertyName._tween, Variant.From<Tween>(ref this._tween));
    info.AddProperty(NMapScreen.PropertyName._startDragPos, Variant.From<Vector2>(ref this._startDragPos));
    info.AddProperty(NMapScreen.PropertyName._targetDragPos, Variant.From<Vector2>(ref this._targetDragPos));
    info.AddProperty(NMapScreen.PropertyName._isDragging, Variant.From<bool>(ref this._isDragging));
    info.AddProperty(NMapScreen.PropertyName._hasPlayedAnimation, Variant.From<bool>(ref this._hasPlayedAnimation));
    info.AddProperty(NMapScreen.PropertyName._controllerScrollAmount, Variant.From<float>(ref this._controllerScrollAmount));
    info.AddProperty(NMapScreen.PropertyName._distX, Variant.From<float>(ref this._distX));
    info.AddProperty(NMapScreen.PropertyName._distY, Variant.From<float>(ref this._distY));
    info.AddProperty(NMapScreen.PropertyName._actAnimTween, Variant.From<Tween>(ref this._actAnimTween));
    info.AddProperty(NMapScreen.PropertyName._mapScrollAnimTimer, Variant.From<float>(ref this._mapScrollAnimTimer));
    info.AddProperty(NMapScreen.PropertyName._canInterruptAnim, Variant.From<bool>(ref this._canInterruptAnim));
    info.AddProperty(NMapScreen.PropertyName._isInputDisabled, Variant.From<bool>(ref this._isInputDisabled));
    info.AddProperty(NMapScreen.PropertyName._promptTween, Variant.From<Tween>(ref this._promptTween));
    info.AddProperty(NMapScreen.PropertyName._drawingInput, Variant.From<NMapDrawingInput>(ref this._drawingInput));
    info.AddSignalEventDelegate(NMapScreen.SignalName.Opened, (Delegate) this.backing_Opened);
    info.AddSignalEventDelegate(NMapScreen.SignalName.Closed, (Delegate) this.backing_Closed);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NMapScreen.PropertyName.IsOpen, ref variant1))
      this.IsOpen = ((Variant) ref variant1).As<bool>();
    Variant variant2;
    if (info.TryGetProperty(NMapScreen.PropertyName.IsTravelEnabled, ref variant2))
      this.IsTravelEnabled = ((Variant) ref variant2).As<bool>();
    Variant variant3;
    if (info.TryGetProperty(NMapScreen.PropertyName.IsDebugTravelEnabled, ref variant3))
      this.IsDebugTravelEnabled = ((Variant) ref variant3).As<bool>();
    Variant variant4;
    if (info.TryGetProperty(NMapScreen.PropertyName.IsTraveling, ref variant4))
      this.IsTraveling = ((Variant) ref variant4).As<bool>();
    Variant variant5;
    if (info.TryGetProperty(NMapScreen.PropertyName.Drawings, ref variant5))
      this.Drawings = ((Variant) ref variant5).As<NMapDrawings>();
    Variant variant6;
    if (info.TryGetProperty(NMapScreen.PropertyName._mapContainer, ref variant6))
      this._mapContainer = ((Variant) ref variant6).As<Control>();
    Variant variant7;
    if (info.TryGetProperty(NMapScreen.PropertyName._pathsContainer, ref variant7))
      this._pathsContainer = ((Variant) ref variant7).As<Control>();
    Variant variant8;
    if (info.TryGetProperty(NMapScreen.PropertyName._points, ref variant8))
      this._points = ((Variant) ref variant8).As<Control>();
    Variant variant9;
    if (info.TryGetProperty(NMapScreen.PropertyName._bossPointNode, ref variant9))
      this._bossPointNode = ((Variant) ref variant9).As<NBossMapPoint>();
    Variant variant10;
    if (info.TryGetProperty(NMapScreen.PropertyName._secondBossPointNode, ref variant10))
      this._secondBossPointNode = ((Variant) ref variant10).As<NBossMapPoint>();
    Variant variant11;
    if (info.TryGetProperty(NMapScreen.PropertyName._startingPointNode, ref variant11))
      this._startingPointNode = ((Variant) ref variant11).As<NMapPoint>();
    Variant variant12;
    if (info.TryGetProperty(NMapScreen.PropertyName._mapBgContainer, ref variant12))
      this._mapBgContainer = ((Variant) ref variant12).As<NMapBg>();
    Variant variant13;
    if (info.TryGetProperty(NMapScreen.PropertyName._marker, ref variant13))
      this._marker = ((Variant) ref variant13).As<NMapMarker>();
    Variant variant14;
    if (info.TryGetProperty(NMapScreen.PropertyName._backButton, ref variant14))
      this._backButton = ((Variant) ref variant14).As<NBackButton>();
    Variant variant15;
    if (info.TryGetProperty(NMapScreen.PropertyName._drawingToolsHotkeyIcon, ref variant15))
      this._drawingToolsHotkeyIcon = ((Variant) ref variant15).As<TextureRect>();
    Variant variant16;
    if (info.TryGetProperty(NMapScreen.PropertyName._drawingTools, ref variant16))
      this._drawingTools = ((Variant) ref variant16).As<Control>();
    Variant variant17;
    if (info.TryGetProperty(NMapScreen.PropertyName._mapDrawingButton, ref variant17))
      this._mapDrawingButton = ((Variant) ref variant17).As<NMapDrawButton>();
    Variant variant18;
    if (info.TryGetProperty(NMapScreen.PropertyName._mapErasingButton, ref variant18))
      this._mapErasingButton = ((Variant) ref variant18).As<NMapEraseButton>();
    Variant variant19;
    if (info.TryGetProperty(NMapScreen.PropertyName._mapClearButton, ref variant19))
      this._mapClearButton = ((Variant) ref variant19).As<NMapClearButton>();
    Variant variant20;
    if (info.TryGetProperty(NMapScreen.PropertyName._mapLegend, ref variant20))
      this._mapLegend = ((Variant) ref variant20).As<Control>();
    Variant variant21;
    if (info.TryGetProperty(NMapScreen.PropertyName._legendItems, ref variant21))
      this._legendItems = ((Variant) ref variant21).As<Control>();
    Variant variant22;
    if (info.TryGetProperty(NMapScreen.PropertyName._legendHotkeyIcon, ref variant22))
      this._legendHotkeyIcon = ((Variant) ref variant22).As<TextureRect>();
    Variant variant23;
    if (info.TryGetProperty(NMapScreen.PropertyName._backstop, ref variant23))
      this._backstop = ((Variant) ref variant23).As<Control>();
    Variant variant24;
    if (info.TryGetProperty(NMapScreen.PropertyName._tween, ref variant24))
      this._tween = ((Variant) ref variant24).As<Tween>();
    Variant variant25;
    if (info.TryGetProperty(NMapScreen.PropertyName._startDragPos, ref variant25))
      this._startDragPos = ((Variant) ref variant25).As<Vector2>();
    Variant variant26;
    if (info.TryGetProperty(NMapScreen.PropertyName._targetDragPos, ref variant26))
      this._targetDragPos = ((Variant) ref variant26).As<Vector2>();
    Variant variant27;
    if (info.TryGetProperty(NMapScreen.PropertyName._isDragging, ref variant27))
      this._isDragging = ((Variant) ref variant27).As<bool>();
    Variant variant28;
    if (info.TryGetProperty(NMapScreen.PropertyName._hasPlayedAnimation, ref variant28))
      this._hasPlayedAnimation = ((Variant) ref variant28).As<bool>();
    Variant variant29;
    if (info.TryGetProperty(NMapScreen.PropertyName._controllerScrollAmount, ref variant29))
      this._controllerScrollAmount = ((Variant) ref variant29).As<float>();
    Variant variant30;
    if (info.TryGetProperty(NMapScreen.PropertyName._distX, ref variant30))
      this._distX = ((Variant) ref variant30).As<float>();
    Variant variant31;
    if (info.TryGetProperty(NMapScreen.PropertyName._distY, ref variant31))
      this._distY = ((Variant) ref variant31).As<float>();
    Variant variant32;
    if (info.TryGetProperty(NMapScreen.PropertyName._actAnimTween, ref variant32))
      this._actAnimTween = ((Variant) ref variant32).As<Tween>();
    Variant variant33;
    if (info.TryGetProperty(NMapScreen.PropertyName._mapScrollAnimTimer, ref variant33))
      this._mapScrollAnimTimer = ((Variant) ref variant33).As<float>();
    Variant variant34;
    if (info.TryGetProperty(NMapScreen.PropertyName._canInterruptAnim, ref variant34))
      this._canInterruptAnim = ((Variant) ref variant34).As<bool>();
    Variant variant35;
    if (info.TryGetProperty(NMapScreen.PropertyName._isInputDisabled, ref variant35))
      this._isInputDisabled = ((Variant) ref variant35).As<bool>();
    Variant variant36;
    if (info.TryGetProperty(NMapScreen.PropertyName._promptTween, ref variant36))
      this._promptTween = ((Variant) ref variant36).As<Tween>();
    Variant variant37;
    if (info.TryGetProperty(NMapScreen.PropertyName._drawingInput, ref variant37))
      this._drawingInput = ((Variant) ref variant37).As<NMapDrawingInput>();
    NMapScreen.OpenedEventHandler openedEventHandler;
    if (info.TryGetSignalEventDelegate<NMapScreen.OpenedEventHandler>(NMapScreen.SignalName.Opened, ref openedEventHandler))
      this.backing_Opened = openedEventHandler;
    NMapScreen.ClosedEventHandler closedEventHandler;
    if (!info.TryGetSignalEventDelegate<NMapScreen.ClosedEventHandler>(NMapScreen.SignalName.Closed, ref closedEventHandler))
      return;
    this.backing_Closed = closedEventHandler;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<MethodInfo> GetGodotSignalList()
  {
    return new List<MethodInfo>(2)
    {
      new MethodInfo(NMapScreen.SignalName.Opened, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NMapScreen.SignalName.Closed, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  public event NMapScreen.OpenedEventHandler Opened
  {
    add => this.backing_Opened += value;
    remove => this.backing_Opened -= value;
  }

  protected void EmitSignalOpened()
  {
    ((GodotObject) this).EmitSignal(NMapScreen.SignalName.Opened, Array.Empty<Variant>());
  }

  public event NMapScreen.ClosedEventHandler Closed
  {
    add => this.backing_Closed += value;
    remove => this.backing_Closed -= value;
  }

  protected void EmitSignalClosed()
  {
    ((GodotObject) this).EmitSignal(NMapScreen.SignalName.Closed, Array.Empty<Variant>());
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RaiseGodotClassSignalCallbacks(
    in godot_string_name signal,
    NativeVariantPtrArgs args)
  {
    if (StringName.op_Equality(ref signal, NMapScreen.SignalName.Opened) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      NMapScreen.OpenedEventHandler backingOpened = this.backing_Opened;
      if (backingOpened == null)
        return;
      backingOpened();
    }
    else if (StringName.op_Equality(ref signal, NMapScreen.SignalName.Closed) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      NMapScreen.ClosedEventHandler backingClosed = this.backing_Closed;
      if (backingClosed == null)
        return;
      backingClosed();
    }
    else
      ((GodotObject) this).RaiseGodotClassSignalCallbacks(ref signal, args);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassSignal(in godot_string_name signal)
  {
    return StringName.op_Equality(ref signal, NMapScreen.SignalName.Opened) || StringName.op_Equality(ref signal, NMapScreen.SignalName.Closed) || base.HasGodotClassSignal(ref signal);
  }

  [Signal]
  public delegate void OpenedEventHandler();

  [Signal]
  public delegate void ClosedEventHandler();

  public class MethodName : Control.MethodName
  {
    public static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName _ExitTree = StringName.op_Implicit(nameof (_ExitTree));
    public static readonly StringName GetLineEndpoint = StringName.op_Implicit(nameof (GetLineEndpoint));
    public static readonly StringName RecalculateTravelability = StringName.op_Implicit(nameof (RecalculateTravelability));
    public static readonly StringName InitMapVotes = StringName.op_Implicit(nameof (InitMapVotes));
    public static readonly StringName OnMapPointSelectedLocally = StringName.op_Implicit(nameof (OnMapPointSelectedLocally));
    public static readonly StringName RefreshAllMapPointVotes = StringName.op_Implicit(nameof (RefreshAllMapPointVotes));
    public static readonly StringName RemoveAllMapPointsAndPaths = StringName.op_Implicit(nameof (RemoveAllMapPointsAndPaths));
    public static readonly StringName _Process = StringName.op_Implicit(nameof (_Process));
    public static readonly StringName UpdateScrollPosition = StringName.op_Implicit(nameof (UpdateScrollPosition));
    public static readonly StringName _GuiInput = StringName.op_Implicit(nameof (_GuiInput));
    public static readonly StringName ProcessMouseEvent = StringName.op_Implicit(nameof (ProcessMouseEvent));
    public static readonly StringName ProcessMouseDrawingEvent = StringName.op_Implicit(nameof (ProcessMouseDrawingEvent));
    public static readonly StringName ProcessScrollEvent = StringName.op_Implicit(nameof (ProcessScrollEvent));
    public static readonly StringName ProcessControllerEvent = StringName.op_Implicit(nameof (ProcessControllerEvent));
    public static readonly StringName SetTravelEnabled = StringName.op_Implicit(nameof (SetTravelEnabled));
    public static readonly StringName SetDebugTravelEnabled = StringName.op_Implicit(nameof (SetDebugTravelEnabled));
    public static readonly StringName RefreshAllPointVisuals = StringName.op_Implicit(nameof (RefreshAllPointVisuals));
    public static readonly StringName PlayStartOfActAnimation = StringName.op_Implicit(nameof (PlayStartOfActAnimation));
    public static readonly StringName InitMapPrompt = StringName.op_Implicit(nameof (InitMapPrompt));
    public static readonly StringName SetInterruptable = StringName.op_Implicit(nameof (SetInterruptable));
    public static readonly StringName CanScroll = StringName.op_Implicit(nameof (CanScroll));
    public static readonly StringName TryCancelStartOfActAnim = StringName.op_Implicit(nameof (TryCancelStartOfActAnim));
    public static readonly StringName OnVisibilityChanged = StringName.op_Implicit(nameof (OnVisibilityChanged));
    public static readonly StringName OnCapstoneChanged = StringName.op_Implicit(nameof (OnCapstoneChanged));
    public static readonly StringName Close = StringName.op_Implicit(nameof (Close));
    public static readonly StringName Open = StringName.op_Implicit(nameof (Open));
    public static readonly StringName OnBackButtonPressed = StringName.op_Implicit(nameof (OnBackButtonPressed));
    public static readonly StringName _Input = StringName.op_Implicit(nameof (_Input));
    public static readonly StringName OnMapDrawingButtonPressed = StringName.op_Implicit(nameof (OnMapDrawingButtonPressed));
    public static readonly StringName OnMapErasingButtonPressed = StringName.op_Implicit(nameof (OnMapErasingButtonPressed));
    public static readonly StringName UpdateDrawingButtonStates = StringName.op_Implicit(nameof (UpdateDrawingButtonStates));
    public static readonly StringName OnClearMapDrawingButtonPressed = StringName.op_Implicit(nameof (OnClearMapDrawingButtonPressed));
    public static readonly StringName HighlightPointType = StringName.op_Implicit(nameof (HighlightPointType));
    public static readonly StringName OnLegendHotkeyPressed = StringName.op_Implicit(nameof (OnLegendHotkeyPressed));
    public static readonly StringName OnDrawingToolsHotkeyPressed = StringName.op_Implicit(nameof (OnDrawingToolsHotkeyPressed));
    public static readonly StringName GetNetPositionFromScreenPosition = StringName.op_Implicit(nameof (GetNetPositionFromScreenPosition));
    public static readonly StringName GetMapPositionFromNetPosition = StringName.op_Implicit(nameof (GetMapPositionFromNetPosition));
    public static readonly StringName GetScreenPositionFromNetPosition = StringName.op_Implicit(nameof (GetScreenPositionFromNetPosition));
    public static readonly StringName IsNodeOnScreen = StringName.op_Implicit(nameof (IsNodeOnScreen));
    public static readonly StringName CleanUp = StringName.op_Implicit(nameof (CleanUp));
    public static readonly StringName UpdateHotkeyDisplay = StringName.op_Implicit(nameof (UpdateHotkeyDisplay));
  }

  public class PropertyName : Control.PropertyName
  {
    public static readonly StringName IsOpen = StringName.op_Implicit(nameof (IsOpen));
    public static readonly StringName IsTravelEnabled = StringName.op_Implicit(nameof (IsTravelEnabled));
    public static readonly StringName IsDebugTravelEnabled = StringName.op_Implicit(nameof (IsDebugTravelEnabled));
    public static readonly StringName MapLegendX = StringName.op_Implicit(nameof (MapLegendX));
    public static readonly StringName IsTraveling = StringName.op_Implicit(nameof (IsTraveling));
    public static readonly StringName Drawings = StringName.op_Implicit(nameof (Drawings));
    public static readonly StringName DefaultFocusedControl = StringName.op_Implicit(nameof (DefaultFocusedControl));
    public static readonly StringName _mapContainer = StringName.op_Implicit(nameof (_mapContainer));
    public static readonly StringName _pathsContainer = StringName.op_Implicit(nameof (_pathsContainer));
    public static readonly StringName _points = StringName.op_Implicit(nameof (_points));
    public static readonly StringName _bossPointNode = StringName.op_Implicit(nameof (_bossPointNode));
    public static readonly StringName _secondBossPointNode = StringName.op_Implicit(nameof (_secondBossPointNode));
    public static readonly StringName _startingPointNode = StringName.op_Implicit(nameof (_startingPointNode));
    public static readonly StringName _mapBgContainer = StringName.op_Implicit(nameof (_mapBgContainer));
    public static readonly StringName _marker = StringName.op_Implicit(nameof (_marker));
    public static readonly StringName _backButton = StringName.op_Implicit(nameof (_backButton));
    public static readonly StringName _drawingToolsHotkeyIcon = StringName.op_Implicit(nameof (_drawingToolsHotkeyIcon));
    public static readonly StringName _drawingTools = StringName.op_Implicit(nameof (_drawingTools));
    public static readonly StringName _mapDrawingButton = StringName.op_Implicit(nameof (_mapDrawingButton));
    public static readonly StringName _mapErasingButton = StringName.op_Implicit(nameof (_mapErasingButton));
    public static readonly StringName _mapClearButton = StringName.op_Implicit(nameof (_mapClearButton));
    public static readonly StringName _mapLegend = StringName.op_Implicit(nameof (_mapLegend));
    public static readonly StringName _legendItems = StringName.op_Implicit(nameof (_legendItems));
    public static readonly StringName _legendHotkeyIcon = StringName.op_Implicit(nameof (_legendHotkeyIcon));
    public static readonly StringName _backstop = StringName.op_Implicit(nameof (_backstop));
    public static readonly StringName _tween = StringName.op_Implicit(nameof (_tween));
    public static readonly StringName _startDragPos = StringName.op_Implicit(nameof (_startDragPos));
    public static readonly StringName _targetDragPos = StringName.op_Implicit(nameof (_targetDragPos));
    public static readonly StringName _isDragging = StringName.op_Implicit(nameof (_isDragging));
    public static readonly StringName _hasPlayedAnimation = StringName.op_Implicit(nameof (_hasPlayedAnimation));
    public static readonly StringName _controllerScrollAmount = StringName.op_Implicit(nameof (_controllerScrollAmount));
    public static readonly StringName _distX = StringName.op_Implicit(nameof (_distX));
    public static readonly StringName _distY = StringName.op_Implicit(nameof (_distY));
    public static readonly StringName _actAnimTween = StringName.op_Implicit(nameof (_actAnimTween));
    public static readonly StringName _mapScrollAnimTimer = StringName.op_Implicit(nameof (_mapScrollAnimTimer));
    public static readonly StringName _mapAnimStartDelay = StringName.op_Implicit(nameof (_mapAnimStartDelay));
    public static readonly StringName _mapAnimDuration = StringName.op_Implicit(nameof (_mapAnimDuration));
    public static readonly StringName _canInterruptAnim = StringName.op_Implicit(nameof (_canInterruptAnim));
    public static readonly StringName _isInputDisabled = StringName.op_Implicit(nameof (_isInputDisabled));
    public static readonly StringName _promptTween = StringName.op_Implicit(nameof (_promptTween));
    public static readonly StringName _drawingInput = StringName.op_Implicit(nameof (_drawingInput));
  }

  public class SignalName : Control.SignalName
  {
    public static readonly StringName Opened = StringName.op_Implicit(nameof (Opened));
    public static readonly StringName Closed = StringName.op_Implicit(nameof (Closed));
  }
}
