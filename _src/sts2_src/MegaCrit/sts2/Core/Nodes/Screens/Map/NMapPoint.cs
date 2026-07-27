// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Screens.Map.NMapPoint
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Map;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.HoverTips;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.Runs.History;
using MegaCrit.Sts2.Core.Saves;
using MegaCrit.Sts2.Core.TestSupport;
using System;
using System.Collections.Generic;
using System.ComponentModel;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Screens.Map;

[ScriptPath("res://src/Core/Nodes/Screens/Map/NMapPoint.cs")]
public abstract class NMapPoint : NButton
{
  private MapPointState _state = MapPointState.Untravelable;
  protected IRunState _runState;
  protected Color _outlineColor = new Color(1f, 1f, 1f, 0.75f);
  protected const double _pressDownDur = 0.3;
  protected const double _unhoverAnimDur = 0.5;
  protected NSelectionReticle _controllerSelectionReticle;
  protected NMapScreen _screen;

  protected abstract Color TraveledColor { get; }

  protected abstract Color UntravelableColor { get; }

  protected abstract Color HoveredColor { get; }

  protected abstract Vector2 HoverScale { get; }

  protected abstract Vector2 DownScale { get; }

  protected override bool AllowFocusWhileDisabled => true;

  public NMultiplayerVoteContainer VoteContainer { get; set; }

  protected bool IsTravelable
  {
    get
    {
      NMapScreen screen = this._screen;
      if (screen != null && screen.IsDebugTravelEnabled && !screen.IsTraveling)
        return true;
      return this._screen.IsTravelEnabled && this.State == MapPointState.Travelable;
    }
  }

  public MapPoint Point { get; protected set; }

  public MapPointState State
  {
    get => this._state;
    set
    {
      if (value == this._state)
        return;
      this._state = value;
      this.RefreshVisualsInstantly();
    }
  }

  public override void _Ready()
  {
    if (((object) this).GetType() != typeof (NMapPoint))
    {
      Log.Error($"{((object) this).GetType()}");
      throw new InvalidOperationException("Don't call base._Ready()! Call ConnectSignals() instead.");
    }
    this.ConnectSignals();
  }

  protected override void ConnectSignals()
  {
    base.ConnectSignals();
    this.VoteContainer = ((Node) this).GetNode<NMultiplayerVoteContainer>(NodePath.op_Implicit("%MapPointVoteContainer"));
    this._controllerSelectionReticle = ((Node) this).GetNode<NSelectionReticle>(NodePath.op_Implicit("%SelectionReticle"));
    this.VoteContainer.Initialize(new NMultiplayerVoteContainer.PlayerVotedDelegate(this.ShouldDisplayPlayerVote), this._runState.Players);
  }

  protected bool IsInputAllowed()
  {
    return !this._screen.IsTraveling && this._screen.Drawings.GetLocalDrawingMode() == DrawingMode.None;
  }

  private bool ShouldDisplayPlayerVote(Player player)
  {
    MapCoord? nullable1;
    if (this._screen.PlayerVoteDictionary.TryGetValue(player, out nullable1) && nullable1.HasValue)
    {
      MapCoord? nullable2 = nullable1;
      MapCoord coord = this.Point.coord;
      return nullable2.HasValue && nullable2.GetValueOrDefault() == coord;
    }
    MapCoord? coord1 = this._runState.MapLocation.coord;
    MapCoord coord2 = this.Point.coord;
    return coord1.HasValue && coord1.GetValueOrDefault() == coord2;
  }

  public void RefreshVisualsInstantly()
  {
    this._controllerSelectionReticle.OnDeselect();
    this.RefreshColorInstantly();
    this.RefreshState();
  }

  public virtual void OnSelected()
  {
  }

  protected sealed override void OnRelease()
  {
    if (!this.IsTravelable || this.Point.coord.row == 0 && TestMode.IsOff && !SaveManager.Instance.SeenFtue("map_select_ftue") || this._screen.Drawings.GetLocalDrawingMode() != DrawingMode.None || !this._screen.IsNodeOnScreen(this) && NControllerManager.Instance.IsUsingController)
      return;
    this._screen.OnMapPointSelectedLocally(this);
  }

  protected virtual void RefreshColorInstantly()
  {
  }

  protected virtual void RefreshState()
  {
    if (this.IsTravelable)
      this.Enable();
    else
      this.Disable();
  }

  protected Color TargetColor
  {
    get
    {
      bool flag;
      switch (this.State)
      {
        case MapPointState.Travelable:
        case MapPointState.Traveled:
          flag = true;
          break;
        default:
          flag = false;
          break;
      }
      return flag ? this.TraveledColor : this.UntravelableColor;
    }
  }

  protected override void OnFocus()
  {
    if (!this.IsInputAllowed())
      return;
    if (this._isEnabled && NControllerManager.Instance.IsUsingController)
      this._controllerSelectionReticle.OnSelect();
    if (this._state != MapPointState.Traveled)
      return;
    MapCoord? coord1 = this._runState.MapLocation.coord;
    MapCoord coord2 = this.Point.coord;
    if ((coord1.HasValue ? (coord1.GetValueOrDefault() != coord2 ? 1 : 0) : 1) == 0 || NControllerManager.Instance.IsUsingController)
      return;
    MapPointHistoryEntry historyEntryFor = this._runState.GetHistoryEntryFor(new MapLocation(new MapCoord?(this.Point.coord), this._runState.CurrentActIndex));
    if (historyEntryFor == null)
      return;
    int floorNum = this.Point.coord.row + 1;
    for (int index = 0; index < this._runState.MapPointHistory.Count - 1; ++index)
      floorNum += this._runState.MapPointHistory[index].Count;
    NHoverTipSet tip = NHoverTipSet.CreateAndShowMapPointHistory((Control) this, NMapPointHistoryHoverTip.Create(floorNum, LocalContext.NetId.Value, historyEntryFor));
    Callable callable = Callable.From((Action) (() => tip.SetAlignment((Control) this, HoverTip.GetHoverTipAlignment((Control) this))));
    ((Callable) ref callable).CallDeferred(Array.Empty<Variant>());
  }

  protected override void OnUnfocus()
  {
    this._controllerSelectionReticle.OnDeselect();
    NHoverTipSet.Remove((Control) this);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(10)
    {
      new MethodInfo(NMapPoint.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NMapPoint.MethodName.ConnectSignals, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NMapPoint.MethodName.IsInputAllowed, new PropertyInfo((Variant.Type) 1L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NMapPoint.MethodName.RefreshVisualsInstantly, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NMapPoint.MethodName.OnSelected, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NMapPoint.MethodName.OnRelease, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NMapPoint.MethodName.RefreshColorInstantly, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NMapPoint.MethodName.RefreshState, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NMapPoint.MethodName.OnFocus, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NMapPoint.MethodName.OnUnfocus, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NMapPoint.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NMapPoint.MethodName.ConnectSignals) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.ConnectSignals();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NMapPoint.MethodName.IsInputAllowed) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      bool flag = this.IsInputAllowed();
      ret = VariantUtils.CreateFrom<bool>(ref flag);
      return true;
    }
    if (StringName.op_Equality(ref method, NMapPoint.MethodName.RefreshVisualsInstantly) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.RefreshVisualsInstantly();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NMapPoint.MethodName.OnSelected) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnSelected();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NMapPoint.MethodName.OnRelease) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnRelease();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NMapPoint.MethodName.RefreshColorInstantly) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.RefreshColorInstantly();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NMapPoint.MethodName.RefreshState) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.RefreshState();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NMapPoint.MethodName.OnFocus) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnFocus();
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NMapPoint.MethodName.OnUnfocus) || ((NativeVariantPtrArgs) ref args).Count != 0)
      return base.InvokeGodotClassMethod(in method, args, out ret);
    this.OnUnfocus();
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NMapPoint.MethodName._Ready) || StringName.op_Equality(ref method, NMapPoint.MethodName.ConnectSignals) || StringName.op_Equality(ref method, NMapPoint.MethodName.IsInputAllowed) || StringName.op_Equality(ref method, NMapPoint.MethodName.RefreshVisualsInstantly) || StringName.op_Equality(ref method, NMapPoint.MethodName.OnSelected) || StringName.op_Equality(ref method, NMapPoint.MethodName.OnRelease) || StringName.op_Equality(ref method, NMapPoint.MethodName.RefreshColorInstantly) || StringName.op_Equality(ref method, NMapPoint.MethodName.RefreshState) || StringName.op_Equality(ref method, NMapPoint.MethodName.OnFocus) || StringName.op_Equality(ref method, NMapPoint.MethodName.OnUnfocus) || base.HasGodotClassMethod(in method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NMapPoint.PropertyName.VoteContainer))
    {
      this.VoteContainer = VariantUtils.ConvertTo<NMultiplayerVoteContainer>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NMapPoint.PropertyName.State))
    {
      this.State = VariantUtils.ConvertTo<MapPointState>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NMapPoint.PropertyName._state))
    {
      this._state = VariantUtils.ConvertTo<MapPointState>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NMapPoint.PropertyName._outlineColor))
    {
      this._outlineColor = VariantUtils.ConvertTo<Color>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NMapPoint.PropertyName._controllerSelectionReticle))
    {
      this._controllerSelectionReticle = VariantUtils.ConvertTo<NSelectionReticle>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NMapPoint.PropertyName._screen))
      return base.SetGodotClassPropertyValue(in name, in value);
    this._screen = VariantUtils.ConvertTo<NMapScreen>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NMapPoint.PropertyName.TraveledColor))
    {
      ref godot_variant local = ref value;
      Color traveledColor = this.TraveledColor;
      godot_variant from = VariantUtils.CreateFrom<Color>(ref traveledColor);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NMapPoint.PropertyName.UntravelableColor))
    {
      ref godot_variant local = ref value;
      Color untravelableColor = this.UntravelableColor;
      godot_variant from = VariantUtils.CreateFrom<Color>(ref untravelableColor);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NMapPoint.PropertyName.HoveredColor))
    {
      ref godot_variant local = ref value;
      Color hoveredColor = this.HoveredColor;
      godot_variant from = VariantUtils.CreateFrom<Color>(ref hoveredColor);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NMapPoint.PropertyName.HoverScale))
    {
      ref godot_variant local = ref value;
      Vector2 hoverScale = this.HoverScale;
      godot_variant from = VariantUtils.CreateFrom<Vector2>(ref hoverScale);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NMapPoint.PropertyName.DownScale))
    {
      ref godot_variant local = ref value;
      Vector2 downScale = this.DownScale;
      godot_variant from = VariantUtils.CreateFrom<Vector2>(ref downScale);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NMapPoint.PropertyName.AllowFocusWhileDisabled))
    {
      ref godot_variant local = ref value;
      bool focusWhileDisabled = this.AllowFocusWhileDisabled;
      godot_variant from = VariantUtils.CreateFrom<bool>(ref focusWhileDisabled);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NMapPoint.PropertyName.VoteContainer))
    {
      ref godot_variant local = ref value;
      NMultiplayerVoteContainer voteContainer = this.VoteContainer;
      godot_variant from = VariantUtils.CreateFrom<NMultiplayerVoteContainer>(ref voteContainer);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NMapPoint.PropertyName.IsTravelable))
    {
      ref godot_variant local = ref value;
      bool isTravelable = this.IsTravelable;
      godot_variant from = VariantUtils.CreateFrom<bool>(ref isTravelable);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NMapPoint.PropertyName.State))
    {
      ref godot_variant local = ref value;
      MapPointState state = this.State;
      godot_variant from = VariantUtils.CreateFrom<MapPointState>(ref state);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NMapPoint.PropertyName.TargetColor))
    {
      ref godot_variant local = ref value;
      Color targetColor = this.TargetColor;
      godot_variant from = VariantUtils.CreateFrom<Color>(ref targetColor);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NMapPoint.PropertyName._state))
    {
      value = VariantUtils.CreateFrom<MapPointState>(ref this._state);
      return true;
    }
    if (StringName.op_Equality(ref name, NMapPoint.PropertyName._outlineColor))
    {
      value = VariantUtils.CreateFrom<Color>(ref this._outlineColor);
      return true;
    }
    if (StringName.op_Equality(ref name, NMapPoint.PropertyName._controllerSelectionReticle))
    {
      value = VariantUtils.CreateFrom<NSelectionReticle>(ref this._controllerSelectionReticle);
      return true;
    }
    if (!StringName.op_Equality(ref name, NMapPoint.PropertyName._screen))
      return base.GetGodotClassPropertyValue(in name, out value);
    value = VariantUtils.CreateFrom<NMapScreen>(ref this._screen);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 2L, NMapPoint.PropertyName._state, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 20L, NMapPoint.PropertyName.TraveledColor, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 20L, NMapPoint.PropertyName.UntravelableColor, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 20L, NMapPoint.PropertyName.HoveredColor, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 20L, NMapPoint.PropertyName._outlineColor, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 5L, NMapPoint.PropertyName.HoverScale, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 5L, NMapPoint.PropertyName.DownScale, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NMapPoint.PropertyName._controllerSelectionReticle, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 1L, NMapPoint.PropertyName.AllowFocusWhileDisabled, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NMapPoint.PropertyName._screen, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NMapPoint.PropertyName.VoteContainer, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 1L, NMapPoint.PropertyName.IsTravelable, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 2L, NMapPoint.PropertyName.State, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 20L, NMapPoint.PropertyName.TargetColor, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    base.SaveGodotObjectData(info);
    GodotSerializationInfo serializationInfo1 = info;
    StringName voteContainer1 = NMapPoint.PropertyName.VoteContainer;
    NMultiplayerVoteContainer voteContainer2 = this.VoteContainer;
    Variant variant1 = Variant.From<NMultiplayerVoteContainer>(ref voteContainer2);
    serializationInfo1.AddProperty(voteContainer1, variant1);
    GodotSerializationInfo serializationInfo2 = info;
    StringName state1 = NMapPoint.PropertyName.State;
    MapPointState state2 = this.State;
    Variant variant2 = Variant.From<MapPointState>(ref state2);
    serializationInfo2.AddProperty(state1, variant2);
    info.AddProperty(NMapPoint.PropertyName._state, Variant.From<MapPointState>(ref this._state));
    info.AddProperty(NMapPoint.PropertyName._outlineColor, Variant.From<Color>(ref this._outlineColor));
    info.AddProperty(NMapPoint.PropertyName._controllerSelectionReticle, Variant.From<NSelectionReticle>(ref this._controllerSelectionReticle));
    info.AddProperty(NMapPoint.PropertyName._screen, Variant.From<NMapScreen>(ref this._screen));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    base.RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NMapPoint.PropertyName.VoteContainer, ref variant1))
      this.VoteContainer = ((Variant) ref variant1).As<NMultiplayerVoteContainer>();
    Variant variant2;
    if (info.TryGetProperty(NMapPoint.PropertyName.State, ref variant2))
      this.State = ((Variant) ref variant2).As<MapPointState>();
    Variant variant3;
    if (info.TryGetProperty(NMapPoint.PropertyName._state, ref variant3))
      this._state = ((Variant) ref variant3).As<MapPointState>();
    Variant variant4;
    if (info.TryGetProperty(NMapPoint.PropertyName._outlineColor, ref variant4))
      this._outlineColor = ((Variant) ref variant4).As<Color>();
    Variant variant5;
    if (info.TryGetProperty(NMapPoint.PropertyName._controllerSelectionReticle, ref variant5))
      this._controllerSelectionReticle = ((Variant) ref variant5).As<NSelectionReticle>();
    Variant variant6;
    if (!info.TryGetProperty(NMapPoint.PropertyName._screen, ref variant6))
      return;
    this._screen = ((Variant) ref variant6).As<NMapScreen>();
  }

  public new class MethodName : NButton.MethodName
  {
    public new static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public new static readonly StringName ConnectSignals = StringName.op_Implicit(nameof (ConnectSignals));
    public static readonly StringName IsInputAllowed = StringName.op_Implicit(nameof (IsInputAllowed));
    public static readonly StringName RefreshVisualsInstantly = StringName.op_Implicit(nameof (RefreshVisualsInstantly));
    public static readonly StringName OnSelected = StringName.op_Implicit(nameof (OnSelected));
    public new static readonly StringName OnRelease = StringName.op_Implicit(nameof (OnRelease));
    public static readonly StringName RefreshColorInstantly = StringName.op_Implicit(nameof (RefreshColorInstantly));
    public static readonly StringName RefreshState = StringName.op_Implicit(nameof (RefreshState));
    public new static readonly StringName OnFocus = StringName.op_Implicit(nameof (OnFocus));
    public new static readonly StringName OnUnfocus = StringName.op_Implicit(nameof (OnUnfocus));
  }

  public new class PropertyName : NButton.PropertyName
  {
    public static readonly StringName TraveledColor = StringName.op_Implicit(nameof (TraveledColor));
    public static readonly StringName UntravelableColor = StringName.op_Implicit(nameof (UntravelableColor));
    public static readonly StringName HoveredColor = StringName.op_Implicit(nameof (HoveredColor));
    public static readonly StringName HoverScale = StringName.op_Implicit(nameof (HoverScale));
    public static readonly StringName DownScale = StringName.op_Implicit(nameof (DownScale));
    public new static readonly StringName AllowFocusWhileDisabled = StringName.op_Implicit(nameof (AllowFocusWhileDisabled));
    public static readonly StringName VoteContainer = StringName.op_Implicit(nameof (VoteContainer));
    public static readonly StringName IsTravelable = StringName.op_Implicit(nameof (IsTravelable));
    public static readonly StringName State = StringName.op_Implicit(nameof (State));
    public static readonly StringName TargetColor = StringName.op_Implicit(nameof (TargetColor));
    public static readonly StringName _state = StringName.op_Implicit(nameof (_state));
    public static readonly StringName _outlineColor = StringName.op_Implicit(nameof (_outlineColor));
    public static readonly StringName _controllerSelectionReticle = StringName.op_Implicit(nameof (_controllerSelectionReticle));
    public static readonly StringName _screen = StringName.op_Implicit(nameof (_screen));
  }

  public new class SignalName : NButton.SignalName
  {
  }
}
