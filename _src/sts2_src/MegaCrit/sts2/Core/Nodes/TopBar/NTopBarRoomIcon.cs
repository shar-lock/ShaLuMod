// Decompiled with JetBrains decompiler
// Type: MegaCrit.sts2.Core.Nodes.TopBar.NTopBarRoomIcon
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Map;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.HoverTips;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Runs;
using System;
using System.Collections.Generic;
using System.ComponentModel;

#nullable enable
namespace MegaCrit.sts2.Core.Nodes.TopBar;

[ScriptPath("res://src/Core/Nodes/TopBar/NTopBarRoomIcon.cs")]
public class NTopBarRoomIcon : NClickableControl
{
  private IRunState _runState;
  private TextureRect _roomIcon;
  private TextureRect _roomIconOutline;
  private MapPointType _debugMapPointTypeOverride;

  public override void _Ready()
  {
    this._roomIcon = ((Node) this).GetNode<TextureRect>(NodePath.op_Implicit("Icon"));
    this._roomIconOutline = ((Node) this).GetNode<TextureRect>(NodePath.op_Implicit("Icon/Outline"));
    this.ConnectSignals();
  }

  public override void _EnterTree()
  {
    RunManager.Instance.RoomEntered += new Action(this.UpdateIcon);
  }

  public override void _ExitTree()
  {
    RunManager.Instance.RoomEntered -= new Action(this.UpdateIcon);
  }

  public void Initialize(IRunState runState)
  {
    this._runState = runState;
    this.UpdateIcon();
  }

  public void DebugSetMapPointTypeOverride(MapPointType mapPointType)
  {
    if (mapPointType == MapPointType.Unassigned)
      return;
    this._debugMapPointTypeOverride = mapPointType;
  }

  public void DebugClearMapPointTypeOverride()
  {
    this._debugMapPointTypeOverride = MapPointType.Unassigned;
  }

  protected override void OnFocus()
  {
    string prefixForRoomType = this.GetHoverTipPrefixForRoomType();
    NHoverTipSet.CreateAndShow((Control) this._roomIcon, (IHoverTip) new HoverTip(new LocString("static_hover_tips", prefixForRoomType + ".title"), new LocString("static_hover_tips", prefixForRoomType + ".description")))?.SetGlobalPosition(Vector2.op_Addition(((Control) this._roomIcon).GlobalPosition, new Vector2(0.0f, this.Size.Y + 20f)), false);
  }

  private string GetHoverTipPrefixForRoomType()
  {
    switch (this.GetCurrentMapPointType())
    {
      case MapPointType.Unassigned:
        return "ROOM_MAP";
      case MapPointType.Unknown:
        return this.GetHoverTipPrefixForUnknownRoomType();
      case MapPointType.Shop:
        return "ROOM_MERCHANT";
      case MapPointType.Treasure:
        return "ROOM_TREASURE";
      case MapPointType.RestSite:
        return "ROOM_REST";
      case MapPointType.Monster:
        return "ROOM_ENEMY";
      case MapPointType.Elite:
        return "ROOM_ELITE";
      case MapPointType.Boss:
        return "ROOM_BOSS";
      case MapPointType.Ancient:
        return "ROOM_ANCIENT";
      default:
        throw new ArgumentOutOfRangeException();
    }
  }

  private string GetHoverTipPrefixForUnknownRoomType()
  {
    switch (this._runState.BaseRoom.RoomType)
    {
      case RoomType.Monster:
        return "ROOM_UNKNOWN_ENEMY";
      case RoomType.Treasure:
        return "ROOM_UNKNOWN_TREASURE";
      case RoomType.Shop:
        return "ROOM_UNKNOWN_MERCHANT";
      case RoomType.Event:
        return "ROOM_UNKNOWN_EVENT";
      default:
        throw new ArgumentOutOfRangeException();
    }
  }

  protected override void OnUnfocus() => NHoverTipSet.Remove((Control) this._roomIcon);

  private void UpdateIcon()
  {
    if (this._runState.CurrentRoom == null)
      return;
    AbstractRoom baseRoom = this._runState.BaseRoom;
    ActModel act = this._runState.Act;
    MapPointType currentMapPointType = this.GetCurrentMapPointType();
    ModelId modelId = (ModelId) null;
    switch (currentMapPointType)
    {
      case MapPointType.Boss:
        modelId = this._runState.CurrentMapPoint == this._runState.Map.SecondBossMapPoint ? act.SecondBossEncounter.Id : act.BossEncounter.Id;
        break;
      case MapPointType.Ancient:
        modelId = act.Ancient.Id;
        break;
    }
    string roomIconPath = ImageHelper.GetRoomIconPath(currentMapPointType, baseRoom.RoomType, modelId);
    if (roomIconPath != null)
    {
      ((CanvasItem) this._roomIcon).Visible = true;
      this._roomIcon.Texture = (Texture2D) PreloadManager.Cache.GetCompressedTexture2D(roomIconPath);
    }
    else
      ((CanvasItem) this._roomIcon).Visible = false;
    string roomIconOutlinePath = ImageHelper.GetRoomIconOutlinePath(currentMapPointType, baseRoom.RoomType, modelId);
    if (roomIconOutlinePath != null)
    {
      ((CanvasItem) this._roomIconOutline).Visible = true;
      this._roomIconOutline.Texture = (Texture2D) PreloadManager.Cache.GetCompressedTexture2D(roomIconOutlinePath);
    }
    else
      ((CanvasItem) this._roomIconOutline).Visible = false;
    if (!baseRoom.IsVictoryRoom)
      return;
    ((CanvasItem) this._roomIcon).Visible = false;
    ((CanvasItem) this._roomIconOutline).Visible = false;
    this.FocusMode = (Control.FocusModeEnum) 0L;
    this.MouseFilter = (Control.MouseFilterEnum) 2L;
  }

  private MapPointType GetCurrentMapPointType()
  {
    if (this._debugMapPointTypeOverride != MapPointType.Unassigned)
      return this._debugMapPointTypeOverride;
    MapPoint currentMapPoint = this._runState.CurrentMapPoint;
    return currentMapPoint == null ? MapPointType.Unassigned : currentMapPoint.PointType;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(11)
    {
      new MethodInfo(NTopBarRoomIcon.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NTopBarRoomIcon.MethodName._EnterTree, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NTopBarRoomIcon.MethodName._ExitTree, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NTopBarRoomIcon.MethodName.DebugSetMapPointTypeOverride, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 2L, StringName.op_Implicit("mapPointType"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NTopBarRoomIcon.MethodName.DebugClearMapPointTypeOverride, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NTopBarRoomIcon.MethodName.OnFocus, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NTopBarRoomIcon.MethodName.GetHoverTipPrefixForRoomType, new PropertyInfo((Variant.Type) 4L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NTopBarRoomIcon.MethodName.GetHoverTipPrefixForUnknownRoomType, new PropertyInfo((Variant.Type) 4L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NTopBarRoomIcon.MethodName.OnUnfocus, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NTopBarRoomIcon.MethodName.UpdateIcon, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NTopBarRoomIcon.MethodName.GetCurrentMapPointType, new PropertyInfo((Variant.Type) 2L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NTopBarRoomIcon.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NTopBarRoomIcon.MethodName._EnterTree) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._EnterTree();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NTopBarRoomIcon.MethodName._ExitTree) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._ExitTree();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NTopBarRoomIcon.MethodName.DebugSetMapPointTypeOverride) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.DebugSetMapPointTypeOverride(VariantUtils.ConvertTo<MapPointType>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NTopBarRoomIcon.MethodName.DebugClearMapPointTypeOverride) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.DebugClearMapPointTypeOverride();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NTopBarRoomIcon.MethodName.OnFocus) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnFocus();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NTopBarRoomIcon.MethodName.GetHoverTipPrefixForRoomType) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      string prefixForRoomType = this.GetHoverTipPrefixForRoomType();
      ret = VariantUtils.CreateFrom<string>(ref prefixForRoomType);
      return true;
    }
    if (StringName.op_Equality(ref method, NTopBarRoomIcon.MethodName.GetHoverTipPrefixForUnknownRoomType) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      string forUnknownRoomType = this.GetHoverTipPrefixForUnknownRoomType();
      ret = VariantUtils.CreateFrom<string>(ref forUnknownRoomType);
      return true;
    }
    if (StringName.op_Equality(ref method, NTopBarRoomIcon.MethodName.OnUnfocus) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnUnfocus();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NTopBarRoomIcon.MethodName.UpdateIcon) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.UpdateIcon();
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NTopBarRoomIcon.MethodName.GetCurrentMapPointType) || ((NativeVariantPtrArgs) ref args).Count != 0)
      return base.InvokeGodotClassMethod(in method, args, out ret);
    MapPointType currentMapPointType = this.GetCurrentMapPointType();
    ret = VariantUtils.CreateFrom<MapPointType>(ref currentMapPointType);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NTopBarRoomIcon.MethodName._Ready) || StringName.op_Equality(ref method, NTopBarRoomIcon.MethodName._EnterTree) || StringName.op_Equality(ref method, NTopBarRoomIcon.MethodName._ExitTree) || StringName.op_Equality(ref method, NTopBarRoomIcon.MethodName.DebugSetMapPointTypeOverride) || StringName.op_Equality(ref method, NTopBarRoomIcon.MethodName.DebugClearMapPointTypeOverride) || StringName.op_Equality(ref method, NTopBarRoomIcon.MethodName.OnFocus) || StringName.op_Equality(ref method, NTopBarRoomIcon.MethodName.GetHoverTipPrefixForRoomType) || StringName.op_Equality(ref method, NTopBarRoomIcon.MethodName.GetHoverTipPrefixForUnknownRoomType) || StringName.op_Equality(ref method, NTopBarRoomIcon.MethodName.OnUnfocus) || StringName.op_Equality(ref method, NTopBarRoomIcon.MethodName.UpdateIcon) || StringName.op_Equality(ref method, NTopBarRoomIcon.MethodName.GetCurrentMapPointType) || base.HasGodotClassMethod(in method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NTopBarRoomIcon.PropertyName._roomIcon))
    {
      this._roomIcon = VariantUtils.ConvertTo<TextureRect>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NTopBarRoomIcon.PropertyName._roomIconOutline))
    {
      this._roomIconOutline = VariantUtils.ConvertTo<TextureRect>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NTopBarRoomIcon.PropertyName._debugMapPointTypeOverride))
      return base.SetGodotClassPropertyValue(in name, in value);
    this._debugMapPointTypeOverride = VariantUtils.ConvertTo<MapPointType>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NTopBarRoomIcon.PropertyName._roomIcon))
    {
      value = VariantUtils.CreateFrom<TextureRect>(ref this._roomIcon);
      return true;
    }
    if (StringName.op_Equality(ref name, NTopBarRoomIcon.PropertyName._roomIconOutline))
    {
      value = VariantUtils.CreateFrom<TextureRect>(ref this._roomIconOutline);
      return true;
    }
    if (!StringName.op_Equality(ref name, NTopBarRoomIcon.PropertyName._debugMapPointTypeOverride))
      return base.GetGodotClassPropertyValue(in name, out value);
    value = VariantUtils.CreateFrom<MapPointType>(ref this._debugMapPointTypeOverride);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NTopBarRoomIcon.PropertyName._roomIcon, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NTopBarRoomIcon.PropertyName._roomIconOutline, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 2L, NTopBarRoomIcon.PropertyName._debugMapPointTypeOverride, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    base.SaveGodotObjectData(info);
    info.AddProperty(NTopBarRoomIcon.PropertyName._roomIcon, Variant.From<TextureRect>(ref this._roomIcon));
    info.AddProperty(NTopBarRoomIcon.PropertyName._roomIconOutline, Variant.From<TextureRect>(ref this._roomIconOutline));
    info.AddProperty(NTopBarRoomIcon.PropertyName._debugMapPointTypeOverride, Variant.From<MapPointType>(ref this._debugMapPointTypeOverride));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    base.RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NTopBarRoomIcon.PropertyName._roomIcon, ref variant1))
      this._roomIcon = ((Variant) ref variant1).As<TextureRect>();
    Variant variant2;
    if (info.TryGetProperty(NTopBarRoomIcon.PropertyName._roomIconOutline, ref variant2))
      this._roomIconOutline = ((Variant) ref variant2).As<TextureRect>();
    Variant variant3;
    if (!info.TryGetProperty(NTopBarRoomIcon.PropertyName._debugMapPointTypeOverride, ref variant3))
      return;
    this._debugMapPointTypeOverride = ((Variant) ref variant3).As<MapPointType>();
  }

  public new class MethodName : NClickableControl.MethodName
  {
    public static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName _EnterTree = StringName.op_Implicit(nameof (_EnterTree));
    public static readonly StringName _ExitTree = StringName.op_Implicit(nameof (_ExitTree));
    public static readonly StringName DebugSetMapPointTypeOverride = StringName.op_Implicit(nameof (DebugSetMapPointTypeOverride));
    public static readonly StringName DebugClearMapPointTypeOverride = StringName.op_Implicit(nameof (DebugClearMapPointTypeOverride));
    public new static readonly StringName OnFocus = StringName.op_Implicit(nameof (OnFocus));
    public static readonly StringName GetHoverTipPrefixForRoomType = StringName.op_Implicit(nameof (GetHoverTipPrefixForRoomType));
    public static readonly StringName GetHoverTipPrefixForUnknownRoomType = StringName.op_Implicit(nameof (GetHoverTipPrefixForUnknownRoomType));
    public new static readonly StringName OnUnfocus = StringName.op_Implicit(nameof (OnUnfocus));
    public static readonly StringName UpdateIcon = StringName.op_Implicit(nameof (UpdateIcon));
    public static readonly StringName GetCurrentMapPointType = StringName.op_Implicit(nameof (GetCurrentMapPointType));
  }

  public new class PropertyName : NClickableControl.PropertyName
  {
    public static readonly StringName _roomIcon = StringName.op_Implicit(nameof (_roomIcon));
    public static readonly StringName _roomIconOutline = StringName.op_Implicit(nameof (_roomIconOutline));
    public static readonly StringName _debugMapPointTypeOverride = StringName.op_Implicit(nameof (_debugMapPointTypeOverride));
  }

  public new class SignalName : NClickableControl.SignalName
  {
  }
}
