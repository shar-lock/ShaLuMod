// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Screens.Map.NNormalMapPoint
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Map;
using MegaCrit.Sts2.Core.Nodes.HoverTips;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.Random;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.Runs.History;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Screens.Map;

[ScriptPath("res://src/Core/Nodes/Screens/Map/NNormalMapPoint.cs")]
public class NNormalMapPoint : NMapPoint
{
  private static readonly StringName _mapColor = new StringName("map_color");
  private Control _iconContainer;
  private TextureRect _icon;
  private TextureRect _questIcon;
  private TextureRect _outline;
  private NMapCircleVfx? _circleVfx;
  private Tween? _tween;
  private Tween? _pulseTween;
  private const float _pulseSpeed = 4f;
  private const float _scaleAmount = 0.25f;
  private const float _scaleBase = 1.2f;
  private float _elapsedTime = Rng.Chaotic.NextFloat(3140f);

  protected override Color TraveledColor => Colors.White;

  protected override Color UntravelableColor => StsColors.halfTransparentWhite;

  protected override Color HoveredColor => Colors.White;

  protected override Vector2 HoverScale => Vector2.op_Multiply(Vector2.One, 1.45f);

  protected override Vector2 DownScale => Vector2.op_Multiply(Vector2.One, 0.9f);

  private static string ScenePath => SceneHelper.GetScenePath("/ui/normal_map_point");

  public static IEnumerable<string> AssetPaths
  {
    get
    {
      return (IEnumerable<string>) new \u003C\u003Ez__ReadOnlySingleElementList<string>(NNormalMapPoint.ScenePath);
    }
  }

  private static string IconName(MapPointType pointType)
  {
    switch (pointType)
    {
      case MapPointType.Unassigned:
        return "map_unknown";
      case MapPointType.Unknown:
        return "map_unknown";
      case MapPointType.Shop:
        return "map_shop";
      case MapPointType.Treasure:
        return "map_chest";
      case MapPointType.RestSite:
        return "map_rest";
      case MapPointType.Monster:
        return "map_monster";
      case MapPointType.Elite:
        return "map_elite";
      case MapPointType.Boss:
        return string.Empty;
      case MapPointType.Ancient:
        return "map_unknown";
      default:
        throw new ArgumentOutOfRangeException(pointType.ToString());
    }
  }

  private static string IconPath(string filename)
  {
    return ImageHelper.GetImagePath($"atlases/ui_atlas.sprites/map/icons/{filename}.tres");
  }

  private static string OutlinePath(string filename)
  {
    return ImageHelper.GetImagePath($"atlases/compressed.sprites/map/{filename}_outline.tres");
  }

  private static string UnknownIconPath(RoomType pointType)
  {
    string str;
    switch (pointType)
    {
      case RoomType.Monster:
        str = "unknown_monster";
        break;
      case RoomType.Elite:
        str = "unknown_elite";
        break;
      case RoomType.Treasure:
        str = "unknown_chest";
        break;
      case RoomType.Shop:
        str = "unknown_shop";
        break;
      default:
        str = "unknown";
        break;
    }
    return ImageHelper.GetImagePath($"atlases/ui_atlas.sprites/map/icons/map_{str}.tres");
  }

  private static string UnknownOutlinePath(RoomType pointType)
  {
    string filename;
    switch (pointType)
    {
      case RoomType.Monster:
        filename = "map_monster";
        break;
      case RoomType.Elite:
        filename = "map_elite";
        break;
      case RoomType.Treasure:
        filename = "map_chest";
        break;
      case RoomType.Shop:
        filename = "map_shop";
        break;
      default:
        filename = "map_unknown";
        break;
    }
    return NNormalMapPoint.OutlinePath(filename);
  }

  public static NNormalMapPoint Create(MapPoint point, NMapScreen screen, IRunState runState)
  {
    NNormalMapPoint nnormalMapPoint = PreloadManager.Cache.GetScene(NNormalMapPoint.ScenePath).Instantiate<NNormalMapPoint>((PackedScene.GenEditState) 0L);
    nnormalMapPoint.Point = point;
    nnormalMapPoint._screen = screen;
    nnormalMapPoint._runState = runState;
    return nnormalMapPoint;
  }

  public override void _Ready()
  {
    this.ConnectSignals();
    this._iconContainer = ((Node) this).GetNode<Control>(NodePath.op_Implicit("%IconContainer"));
    this._icon = ((Node) this).GetNode<TextureRect>(NodePath.op_Implicit("%Icon"));
    this._outline = ((Node) this).GetNode<TextureRect>(NodePath.op_Implicit("%Outline"));
    this._questIcon = ((Node) this).GetNode<TextureRect>(NodePath.op_Implicit("%QuestIcon"));
    this.UpdateIcon();
    Color mapBgColor = this._runState.Act.MapBgColor;
    ((CanvasItem) this._outline).Modulate = mapBgColor;
    ((ShaderMaterial) ((CanvasItem) this._icon).Material).SetShaderParameter(NNormalMapPoint._mapColor, Variant.op_Implicit(((Color) ref mapBgColor).Lerp(Colors.Gray, 0.5f)));
    this.RefreshMarkedIconVisibility();
    this.RefreshColorInstantly();
    this.Disable();
  }

  public override void _EnterTree()
  {
    this.Point.NodeMarkedChanged += new Action(this.RefreshMarkedIconVisibility);
    NMapScreen.Instance.PointTypeHighlighted += new Action<MapPointType>(this.OnHighlightPointType);
  }

  private void RefreshMarkedIconVisibility()
  {
    ((CanvasItem) this._questIcon).Visible = this.Point.Quests.Count > 0;
  }

  public override void _ExitTree()
  {
    this.Point.NodeMarkedChanged -= new Action(this.RefreshMarkedIconVisibility);
    NMapScreen.Instance.PointTypeHighlighted -= new Action<MapPointType>(this.OnHighlightPointType);
  }

  public override void _Process(double delta)
  {
    if (!this._isEnabled)
      return;
    if (!this.IsFocused && this.IsInputAllowed())
    {
      this._elapsedTime += (float) delta * 4f;
      this._iconContainer.Scale = Vector2.op_Multiply(Vector2.One, (float) ((double) Mathf.Sin(this._elapsedTime) * 0.25 + 1.2000000476837158));
    }
    else
    {
      Control iconContainer = this._iconContainer;
      Vector2 scale = this._iconContainer.Scale;
      Vector2 vector2 = ((Vector2) ref scale).Lerp(Vector2.One, 0.5f);
      iconContainer.Scale = vector2;
    }
  }

  public override void OnSelected()
  {
    this.ShowCircleVfx(true);
    this.State = MapPointState.Traveled;
    this._tween?.Kill();
    this._tween = ((Node) this).CreateTween().SetParallel(true);
    this._tween.TweenProperty((GodotObject) this._iconContainer, NodePath.op_Implicit("scale"), Variant.op_Implicit(Vector2.One), 0.3).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 7L);
    this._tween.TweenProperty((GodotObject) this._icon, NodePath.op_Implicit("scale"), Variant.op_Implicit(Vector2.One), 0.3).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 10L);
    this._tween.TweenProperty((GodotObject) this._questIcon, NodePath.op_Implicit("scale"), Variant.op_Implicit(Vector2.One), 0.3).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 10L);
    this._tween.TweenProperty((GodotObject) this._icon, NodePath.op_Implicit("self_modulate"), Variant.op_Implicit(this.TargetColor), 0.3).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 5L);
    this._tween.TweenProperty((GodotObject) this._outline, NodePath.op_Implicit("modulate"), Variant.op_Implicit(this._runState.Act.MapBgColor), 0.3).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 5L);
  }

  protected override void OnFocus()
  {
    base.OnFocus();
    if (!this.IsInputAllowed())
      return;
    this.AnimHover();
  }

  protected override void OnUnfocus()
  {
    base.OnUnfocus();
    NHoverTipSet.Remove((Control) this);
    if (this._isEnabled)
      this._elapsedTime = 3.926991f;
    this.AnimUnhover();
  }

  protected override void OnPress()
  {
    if (!this.IsTravelable)
      return;
    this.AnimPressDown();
    NHoverTipSet.Remove((Control) this);
  }

  public void SetAngle(float degrees) => this._iconContainer.RotationDegrees = degrees;

  protected override void RefreshColorInstantly()
  {
    ((CanvasItem) this._icon).SelfModulate = this.TargetColor;
  }

  protected override void RefreshState()
  {
    base.RefreshState();
    this.UpdateIcon();
    if (this.State == MapPointState.Traveled)
      this.ShowCircleVfx(false);
    if (this.IsFocused)
      return;
    this._iconContainer.Scale = Vector2.One;
  }

  private void UpdateIcon()
  {
    if (this.Point.PointType != MapPointType.Unknown || this.State != MapPointState.Traveled)
    {
      this._icon.Texture = ResourceLoader.Load<Texture2D>(NNormalMapPoint.IconPath(NNormalMapPoint.IconName(this.Point.PointType)), (string) null, (ResourceLoader.CacheMode) 1L);
      this._outline.Texture = ResourceLoader.Load<Texture2D>(NNormalMapPoint.OutlinePath(NNormalMapPoint.IconName(this.Point.PointType)), (string) null, (ResourceLoader.CacheMode) 1L);
    }
    else
    {
      if (this._runState.MapPointHistory.Count <= this._runState.CurrentActIndex)
        return;
      IReadOnlyList<MapPointHistoryEntry> pointHistoryEntryList = this._runState.MapPointHistory[this._runState.CurrentActIndex];
      if (pointHistoryEntryList.Count <= this.Point.coord.row)
        return;
      RoomType roomType = pointHistoryEntryList[this.Point.coord.row].Rooms.First<MapPointRoomHistoryEntry>().RoomType;
      this._icon.Texture = ResourceLoader.Load<Texture2D>(NNormalMapPoint.UnknownIconPath(roomType), (string) null, (ResourceLoader.CacheMode) 1L);
      this._outline.Texture = ResourceLoader.Load<Texture2D>(NNormalMapPoint.UnknownOutlinePath(roomType), (string) null, (ResourceLoader.CacheMode) 1L);
    }
  }

  private void ShowCircleVfx(bool playAnim)
  {
    if (this._circleVfx != null)
      return;
    this._circleVfx = NMapCircleVfx.Create(this._runState, this.Point.coord, playAnim);
    ((Node) this).AddChildSafely((Node) this._circleVfx);
    NMapCircleVfx circleVfx = this._circleVfx;
    circleVfx.Position = Vector2.op_Addition(circleVfx.Position, this.PivotOffset);
    ((Node) this).MoveChildSafely((Node) this._circleVfx, ((Node) this.VoteContainer).GetIndex(false));
  }

  private void AnimHover()
  {
    this._tween?.Kill();
    this._tween = ((Node) this).CreateTween().SetParallel(true);
    this._tween.TweenProperty((GodotObject) this._icon, NodePath.op_Implicit("scale"), Variant.op_Implicit(this.HoverScale), 0.05);
    this._tween.TweenProperty((GodotObject) this._questIcon, NodePath.op_Implicit("scale"), Variant.op_Implicit(this.HoverScale), 0.05);
    this._tween.TweenProperty((GodotObject) this._icon, NodePath.op_Implicit("self_modulate"), Variant.op_Implicit(this.HoveredColor), 0.05);
    if (!this.IsTravelable)
      return;
    this._tween.TweenProperty((GodotObject) this._outline, NodePath.op_Implicit("modulate"), Variant.op_Implicit(this._outlineColor), 0.05);
  }

  private void AnimUnhover()
  {
    this._tween?.Kill();
    this._tween = ((Node) this).CreateTween().SetParallel(true);
    this._tween.TweenProperty((GodotObject) this._icon, NodePath.op_Implicit("scale"), Variant.op_Implicit(Vector2.One), 0.5).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 7L);
    this._tween.TweenProperty((GodotObject) this._questIcon, NodePath.op_Implicit("scale"), Variant.op_Implicit(Vector2.One), 0.5).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 7L);
    this._tween.TweenProperty((GodotObject) this._icon, NodePath.op_Implicit("self_modulate"), Variant.op_Implicit(this.TargetColor), 0.5).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 7L);
    this._tween.TweenProperty((GodotObject) this._outline, NodePath.op_Implicit("modulate"), Variant.op_Implicit(this._runState.Act.MapBgColor), 0.5).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 7L);
  }

  private void AnimPressDown()
  {
    this._tween?.Kill();
    this._tween = ((Node) this).CreateTween().SetParallel(true);
    this._tween.TweenProperty((GodotObject) this._icon, NodePath.op_Implicit("scale"), Variant.op_Implicit(this.DownScale), 0.3).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 7L);
    this._tween.TweenProperty((GodotObject) this._questIcon, NodePath.op_Implicit("scale"), Variant.op_Implicit(this.DownScale), 0.3).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 7L);
    this._tween.TweenProperty((GodotObject) this._outline, NodePath.op_Implicit("modulate"), Variant.op_Implicit(this._runState.Act.MapBgColor), 0.3).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 5L);
  }

  private void OnHighlightPointType(MapPointType pointType)
  {
    if (pointType == this.Point.PointType)
      this.AnimHover();
    else
      this.AnimUnhover();
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(23)
    {
      new MethodInfo(NNormalMapPoint.MethodName.IconName, new PropertyInfo((Variant.Type) 4L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 33L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 2L, StringName.op_Implicit("pointType"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NNormalMapPoint.MethodName.IconPath, new PropertyInfo((Variant.Type) 4L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 33L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 4L, StringName.op_Implicit("filename"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NNormalMapPoint.MethodName.OutlinePath, new PropertyInfo((Variant.Type) 4L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 33L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 4L, StringName.op_Implicit("filename"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NNormalMapPoint.MethodName.UnknownIconPath, new PropertyInfo((Variant.Type) 4L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 33L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 2L, StringName.op_Implicit("pointType"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NNormalMapPoint.MethodName.UnknownOutlinePath, new PropertyInfo((Variant.Type) 4L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 33L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 2L, StringName.op_Implicit("pointType"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NNormalMapPoint.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NNormalMapPoint.MethodName._EnterTree, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NNormalMapPoint.MethodName.RefreshMarkedIconVisibility, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NNormalMapPoint.MethodName._ExitTree, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NNormalMapPoint.MethodName._Process, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 3L, StringName.op_Implicit("delta"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NNormalMapPoint.MethodName.OnSelected, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NNormalMapPoint.MethodName.OnFocus, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NNormalMapPoint.MethodName.OnUnfocus, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NNormalMapPoint.MethodName.OnPress, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NNormalMapPoint.MethodName.SetAngle, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 3L, StringName.op_Implicit("degrees"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NNormalMapPoint.MethodName.RefreshColorInstantly, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NNormalMapPoint.MethodName.RefreshState, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NNormalMapPoint.MethodName.UpdateIcon, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NNormalMapPoint.MethodName.ShowCircleVfx, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 1L, StringName.op_Implicit("playAnim"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NNormalMapPoint.MethodName.AnimHover, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NNormalMapPoint.MethodName.AnimUnhover, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NNormalMapPoint.MethodName.AnimPressDown, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NNormalMapPoint.MethodName.OnHighlightPointType, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 2L, StringName.op_Implicit("pointType"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NNormalMapPoint.MethodName.IconName) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      string str = NNormalMapPoint.IconName(VariantUtils.ConvertTo<MapPointType>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = VariantUtils.CreateFrom<string>(ref str);
      return true;
    }
    if (StringName.op_Equality(ref method, NNormalMapPoint.MethodName.IconPath) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      string str = NNormalMapPoint.IconPath(VariantUtils.ConvertTo<string>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = VariantUtils.CreateFrom<string>(ref str);
      return true;
    }
    if (StringName.op_Equality(ref method, NNormalMapPoint.MethodName.OutlinePath) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      string str = NNormalMapPoint.OutlinePath(VariantUtils.ConvertTo<string>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = VariantUtils.CreateFrom<string>(ref str);
      return true;
    }
    if (StringName.op_Equality(ref method, NNormalMapPoint.MethodName.UnknownIconPath) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      string str = NNormalMapPoint.UnknownIconPath(VariantUtils.ConvertTo<RoomType>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = VariantUtils.CreateFrom<string>(ref str);
      return true;
    }
    if (StringName.op_Equality(ref method, NNormalMapPoint.MethodName.UnknownOutlinePath) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      string str = NNormalMapPoint.UnknownOutlinePath(VariantUtils.ConvertTo<RoomType>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = VariantUtils.CreateFrom<string>(ref str);
      return true;
    }
    if (StringName.op_Equality(ref method, NNormalMapPoint.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NNormalMapPoint.MethodName._EnterTree) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._EnterTree();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NNormalMapPoint.MethodName.RefreshMarkedIconVisibility) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.RefreshMarkedIconVisibility();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NNormalMapPoint.MethodName._ExitTree) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._ExitTree();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NNormalMapPoint.MethodName._Process) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      ((Node) this)._Process(VariantUtils.ConvertTo<double>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NNormalMapPoint.MethodName.OnSelected) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnSelected();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NNormalMapPoint.MethodName.OnFocus) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnFocus();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NNormalMapPoint.MethodName.OnUnfocus) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnUnfocus();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NNormalMapPoint.MethodName.OnPress) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnPress();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NNormalMapPoint.MethodName.SetAngle) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.SetAngle(VariantUtils.ConvertTo<float>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NNormalMapPoint.MethodName.RefreshColorInstantly) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.RefreshColorInstantly();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NNormalMapPoint.MethodName.RefreshState) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.RefreshState();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NNormalMapPoint.MethodName.UpdateIcon) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.UpdateIcon();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NNormalMapPoint.MethodName.ShowCircleVfx) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.ShowCircleVfx(VariantUtils.ConvertTo<bool>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NNormalMapPoint.MethodName.AnimHover) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.AnimHover();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NNormalMapPoint.MethodName.AnimUnhover) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.AnimUnhover();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NNormalMapPoint.MethodName.AnimPressDown) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.AnimPressDown();
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NNormalMapPoint.MethodName.OnHighlightPointType) || ((NativeVariantPtrArgs) ref args).Count != 1)
      return base.InvokeGodotClassMethod(in method, args, out ret);
    this.OnHighlightPointType(VariantUtils.ConvertTo<MapPointType>(ref ((NativeVariantPtrArgs) ref args)[0]));
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static bool InvokeGodotClassStaticMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NNormalMapPoint.MethodName.IconName) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      string str = NNormalMapPoint.IconName(VariantUtils.ConvertTo<MapPointType>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = VariantUtils.CreateFrom<string>(ref str);
      return true;
    }
    if (StringName.op_Equality(ref method, NNormalMapPoint.MethodName.IconPath) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      string str = NNormalMapPoint.IconPath(VariantUtils.ConvertTo<string>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = VariantUtils.CreateFrom<string>(ref str);
      return true;
    }
    if (StringName.op_Equality(ref method, NNormalMapPoint.MethodName.OutlinePath) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      string str = NNormalMapPoint.OutlinePath(VariantUtils.ConvertTo<string>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = VariantUtils.CreateFrom<string>(ref str);
      return true;
    }
    if (StringName.op_Equality(ref method, NNormalMapPoint.MethodName.UnknownIconPath) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      string str = NNormalMapPoint.UnknownIconPath(VariantUtils.ConvertTo<RoomType>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = VariantUtils.CreateFrom<string>(ref str);
      return true;
    }
    if (StringName.op_Equality(ref method, NNormalMapPoint.MethodName.UnknownOutlinePath) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      string str = NNormalMapPoint.UnknownOutlinePath(VariantUtils.ConvertTo<RoomType>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = VariantUtils.CreateFrom<string>(ref str);
      return true;
    }
    ret = new godot_variant();
    return false;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NNormalMapPoint.MethodName.IconName) || StringName.op_Equality(ref method, NNormalMapPoint.MethodName.IconPath) || StringName.op_Equality(ref method, NNormalMapPoint.MethodName.OutlinePath) || StringName.op_Equality(ref method, NNormalMapPoint.MethodName.UnknownIconPath) || StringName.op_Equality(ref method, NNormalMapPoint.MethodName.UnknownOutlinePath) || StringName.op_Equality(ref method, NNormalMapPoint.MethodName._Ready) || StringName.op_Equality(ref method, NNormalMapPoint.MethodName._EnterTree) || StringName.op_Equality(ref method, NNormalMapPoint.MethodName.RefreshMarkedIconVisibility) || StringName.op_Equality(ref method, NNormalMapPoint.MethodName._ExitTree) || StringName.op_Equality(ref method, NNormalMapPoint.MethodName._Process) || StringName.op_Equality(ref method, NNormalMapPoint.MethodName.OnSelected) || StringName.op_Equality(ref method, NNormalMapPoint.MethodName.OnFocus) || StringName.op_Equality(ref method, NNormalMapPoint.MethodName.OnUnfocus) || StringName.op_Equality(ref method, NNormalMapPoint.MethodName.OnPress) || StringName.op_Equality(ref method, NNormalMapPoint.MethodName.SetAngle) || StringName.op_Equality(ref method, NNormalMapPoint.MethodName.RefreshColorInstantly) || StringName.op_Equality(ref method, NNormalMapPoint.MethodName.RefreshState) || StringName.op_Equality(ref method, NNormalMapPoint.MethodName.UpdateIcon) || StringName.op_Equality(ref method, NNormalMapPoint.MethodName.ShowCircleVfx) || StringName.op_Equality(ref method, NNormalMapPoint.MethodName.AnimHover) || StringName.op_Equality(ref method, NNormalMapPoint.MethodName.AnimUnhover) || StringName.op_Equality(ref method, NNormalMapPoint.MethodName.AnimPressDown) || StringName.op_Equality(ref method, NNormalMapPoint.MethodName.OnHighlightPointType) || base.HasGodotClassMethod(in method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NNormalMapPoint.PropertyName._iconContainer))
    {
      this._iconContainer = VariantUtils.ConvertTo<Control>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NNormalMapPoint.PropertyName._icon))
    {
      this._icon = VariantUtils.ConvertTo<TextureRect>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NNormalMapPoint.PropertyName._questIcon))
    {
      this._questIcon = VariantUtils.ConvertTo<TextureRect>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NNormalMapPoint.PropertyName._outline))
    {
      this._outline = VariantUtils.ConvertTo<TextureRect>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NNormalMapPoint.PropertyName._circleVfx))
    {
      this._circleVfx = VariantUtils.ConvertTo<NMapCircleVfx>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NNormalMapPoint.PropertyName._tween))
    {
      this._tween = VariantUtils.ConvertTo<Tween>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NNormalMapPoint.PropertyName._pulseTween))
    {
      this._pulseTween = VariantUtils.ConvertTo<Tween>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NNormalMapPoint.PropertyName._elapsedTime))
      return base.SetGodotClassPropertyValue(in name, in value);
    this._elapsedTime = VariantUtils.ConvertTo<float>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NNormalMapPoint.PropertyName.TraveledColor))
    {
      ref godot_variant local = ref value;
      Color traveledColor = this.TraveledColor;
      godot_variant from = VariantUtils.CreateFrom<Color>(ref traveledColor);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NNormalMapPoint.PropertyName.UntravelableColor))
    {
      ref godot_variant local = ref value;
      Color untravelableColor = this.UntravelableColor;
      godot_variant from = VariantUtils.CreateFrom<Color>(ref untravelableColor);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NNormalMapPoint.PropertyName.HoveredColor))
    {
      ref godot_variant local = ref value;
      Color hoveredColor = this.HoveredColor;
      godot_variant from = VariantUtils.CreateFrom<Color>(ref hoveredColor);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NNormalMapPoint.PropertyName.HoverScale))
    {
      ref godot_variant local = ref value;
      Vector2 hoverScale = this.HoverScale;
      godot_variant from = VariantUtils.CreateFrom<Vector2>(ref hoverScale);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NNormalMapPoint.PropertyName.DownScale))
    {
      ref godot_variant local = ref value;
      Vector2 downScale = this.DownScale;
      godot_variant from = VariantUtils.CreateFrom<Vector2>(ref downScale);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NNormalMapPoint.PropertyName._iconContainer))
    {
      value = VariantUtils.CreateFrom<Control>(ref this._iconContainer);
      return true;
    }
    if (StringName.op_Equality(ref name, NNormalMapPoint.PropertyName._icon))
    {
      value = VariantUtils.CreateFrom<TextureRect>(ref this._icon);
      return true;
    }
    if (StringName.op_Equality(ref name, NNormalMapPoint.PropertyName._questIcon))
    {
      value = VariantUtils.CreateFrom<TextureRect>(ref this._questIcon);
      return true;
    }
    if (StringName.op_Equality(ref name, NNormalMapPoint.PropertyName._outline))
    {
      value = VariantUtils.CreateFrom<TextureRect>(ref this._outline);
      return true;
    }
    if (StringName.op_Equality(ref name, NNormalMapPoint.PropertyName._circleVfx))
    {
      value = VariantUtils.CreateFrom<NMapCircleVfx>(ref this._circleVfx);
      return true;
    }
    if (StringName.op_Equality(ref name, NNormalMapPoint.PropertyName._tween))
    {
      value = VariantUtils.CreateFrom<Tween>(ref this._tween);
      return true;
    }
    if (StringName.op_Equality(ref name, NNormalMapPoint.PropertyName._pulseTween))
    {
      value = VariantUtils.CreateFrom<Tween>(ref this._pulseTween);
      return true;
    }
    if (!StringName.op_Equality(ref name, NNormalMapPoint.PropertyName._elapsedTime))
      return base.GetGodotClassPropertyValue(in name, out value);
    value = VariantUtils.CreateFrom<float>(ref this._elapsedTime);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NNormalMapPoint.PropertyName._iconContainer, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NNormalMapPoint.PropertyName._icon, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NNormalMapPoint.PropertyName._questIcon, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NNormalMapPoint.PropertyName._outline, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NNormalMapPoint.PropertyName._circleVfx, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NNormalMapPoint.PropertyName._tween, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 20L, NNormalMapPoint.PropertyName.TraveledColor, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 20L, NNormalMapPoint.PropertyName.UntravelableColor, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 20L, NNormalMapPoint.PropertyName.HoveredColor, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 5L, NNormalMapPoint.PropertyName.HoverScale, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 5L, NNormalMapPoint.PropertyName.DownScale, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NNormalMapPoint.PropertyName._pulseTween, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 3L, NNormalMapPoint.PropertyName._elapsedTime, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    base.SaveGodotObjectData(info);
    info.AddProperty(NNormalMapPoint.PropertyName._iconContainer, Variant.From<Control>(ref this._iconContainer));
    info.AddProperty(NNormalMapPoint.PropertyName._icon, Variant.From<TextureRect>(ref this._icon));
    info.AddProperty(NNormalMapPoint.PropertyName._questIcon, Variant.From<TextureRect>(ref this._questIcon));
    info.AddProperty(NNormalMapPoint.PropertyName._outline, Variant.From<TextureRect>(ref this._outline));
    info.AddProperty(NNormalMapPoint.PropertyName._circleVfx, Variant.From<NMapCircleVfx>(ref this._circleVfx));
    info.AddProperty(NNormalMapPoint.PropertyName._tween, Variant.From<Tween>(ref this._tween));
    info.AddProperty(NNormalMapPoint.PropertyName._pulseTween, Variant.From<Tween>(ref this._pulseTween));
    info.AddProperty(NNormalMapPoint.PropertyName._elapsedTime, Variant.From<float>(ref this._elapsedTime));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    base.RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NNormalMapPoint.PropertyName._iconContainer, ref variant1))
      this._iconContainer = ((Variant) ref variant1).As<Control>();
    Variant variant2;
    if (info.TryGetProperty(NNormalMapPoint.PropertyName._icon, ref variant2))
      this._icon = ((Variant) ref variant2).As<TextureRect>();
    Variant variant3;
    if (info.TryGetProperty(NNormalMapPoint.PropertyName._questIcon, ref variant3))
      this._questIcon = ((Variant) ref variant3).As<TextureRect>();
    Variant variant4;
    if (info.TryGetProperty(NNormalMapPoint.PropertyName._outline, ref variant4))
      this._outline = ((Variant) ref variant4).As<TextureRect>();
    Variant variant5;
    if (info.TryGetProperty(NNormalMapPoint.PropertyName._circleVfx, ref variant5))
      this._circleVfx = ((Variant) ref variant5).As<NMapCircleVfx>();
    Variant variant6;
    if (info.TryGetProperty(NNormalMapPoint.PropertyName._tween, ref variant6))
      this._tween = ((Variant) ref variant6).As<Tween>();
    Variant variant7;
    if (info.TryGetProperty(NNormalMapPoint.PropertyName._pulseTween, ref variant7))
      this._pulseTween = ((Variant) ref variant7).As<Tween>();
    Variant variant8;
    if (!info.TryGetProperty(NNormalMapPoint.PropertyName._elapsedTime, ref variant8))
      return;
    this._elapsedTime = ((Variant) ref variant8).As<float>();
  }

  public new class MethodName : NMapPoint.MethodName
  {
    public static readonly StringName IconName = StringName.op_Implicit(nameof (IconName));
    public static readonly StringName IconPath = StringName.op_Implicit(nameof (IconPath));
    public static readonly StringName OutlinePath = StringName.op_Implicit(nameof (OutlinePath));
    public static readonly StringName UnknownIconPath = StringName.op_Implicit(nameof (UnknownIconPath));
    public static readonly StringName UnknownOutlinePath = StringName.op_Implicit(nameof (UnknownOutlinePath));
    public new static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public new static readonly StringName _EnterTree = StringName.op_Implicit(nameof (_EnterTree));
    public static readonly StringName RefreshMarkedIconVisibility = StringName.op_Implicit(nameof (RefreshMarkedIconVisibility));
    public new static readonly StringName _ExitTree = StringName.op_Implicit(nameof (_ExitTree));
    public static readonly StringName _Process = StringName.op_Implicit(nameof (_Process));
    public new static readonly StringName OnSelected = StringName.op_Implicit(nameof (OnSelected));
    public new static readonly StringName OnFocus = StringName.op_Implicit(nameof (OnFocus));
    public new static readonly StringName OnUnfocus = StringName.op_Implicit(nameof (OnUnfocus));
    public new static readonly StringName OnPress = StringName.op_Implicit(nameof (OnPress));
    public static readonly StringName SetAngle = StringName.op_Implicit(nameof (SetAngle));
    public new static readonly StringName RefreshColorInstantly = StringName.op_Implicit(nameof (RefreshColorInstantly));
    public new static readonly StringName RefreshState = StringName.op_Implicit(nameof (RefreshState));
    public static readonly StringName UpdateIcon = StringName.op_Implicit(nameof (UpdateIcon));
    public static readonly StringName ShowCircleVfx = StringName.op_Implicit(nameof (ShowCircleVfx));
    public static readonly StringName AnimHover = StringName.op_Implicit(nameof (AnimHover));
    public static readonly StringName AnimUnhover = StringName.op_Implicit(nameof (AnimUnhover));
    public static readonly StringName AnimPressDown = StringName.op_Implicit(nameof (AnimPressDown));
    public static readonly StringName OnHighlightPointType = StringName.op_Implicit(nameof (OnHighlightPointType));
  }

  public new class PropertyName : NMapPoint.PropertyName
  {
    public new static readonly StringName TraveledColor = StringName.op_Implicit(nameof (TraveledColor));
    public new static readonly StringName UntravelableColor = StringName.op_Implicit(nameof (UntravelableColor));
    public new static readonly StringName HoveredColor = StringName.op_Implicit(nameof (HoveredColor));
    public new static readonly StringName HoverScale = StringName.op_Implicit(nameof (HoverScale));
    public new static readonly StringName DownScale = StringName.op_Implicit(nameof (DownScale));
    public static readonly StringName _iconContainer = StringName.op_Implicit(nameof (_iconContainer));
    public static readonly StringName _icon = StringName.op_Implicit(nameof (_icon));
    public static readonly StringName _questIcon = StringName.op_Implicit(nameof (_questIcon));
    public static readonly StringName _outline = StringName.op_Implicit(nameof (_outline));
    public static readonly StringName _circleVfx = StringName.op_Implicit(nameof (_circleVfx));
    public static readonly StringName _tween = StringName.op_Implicit(nameof (_tween));
    public static readonly StringName _pulseTween = StringName.op_Implicit(nameof (_pulseTween));
    public static readonly StringName _elapsedTime = StringName.op_Implicit(nameof (_elapsedTime));
  }

  public new class SignalName : NMapPoint.SignalName
  {
  }
}
