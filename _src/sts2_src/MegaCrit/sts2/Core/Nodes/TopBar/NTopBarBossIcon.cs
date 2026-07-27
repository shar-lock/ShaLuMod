// Decompiled with JetBrains decompiler
// Type: MegaCrit.sts2.Core.Nodes.TopBar.NTopBarBossIcon
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

[ScriptPath("res://src/Core/Nodes/TopBar/NTopBarBossIcon.cs")]
public class NTopBarBossIcon : NClickableControl
{
  private static readonly LocString _bossHoverTipTitle = new LocString("static_hover_tips", "BOSS.title");
  private static readonly LocString _bossHoverTipDescription = new LocString("static_hover_tips", "BOSS.description");
  private static readonly LocString _doubleBossHoverTipTitle = new LocString("static_hover_tips", "DOUBLE_BOSS.title");
  private static readonly LocString _doubleBossHoverTipDescription = new LocString("static_hover_tips", "DOUBLE_BOSS.description");
  private TextureRect _bossIcon;
  private TextureRect _bossIconOutline;
  private TextureRect? _secondBossIcon;
  private TextureRect? _secondBossIconOutline;
  private static readonly StringName _tintColor = new StringName("tint_color");
  private const string _secondBossIconScenePath = "res://scenes/ui/top_bar/second_boss_icon.tscn";
  private IRunState _runState;

  public override void _Ready()
  {
    this._bossIcon = ((Node) this).GetNode<TextureRect>(NodePath.op_Implicit("Icon"));
    this._bossIconOutline = ((Node) this).GetNode<TextureRect>(NodePath.op_Implicit("Icon/Outline"));
    this.ConnectSignals();
  }

  public void Initialize(IRunState runState)
  {
    this._runState = runState;
    this.OnActEntered();
  }

  public override void _EnterTree()
  {
    RunManager.Instance.ActEntered += new Action(this.OnActEntered);
    RunManager.Instance.RoomEntered += new Action(this.OnRoomEntered);
  }

  public override void _ExitTree()
  {
    RunManager.Instance.ActEntered -= new Action(this.OnActEntered);
    RunManager.Instance.RoomEntered -= new Action(this.OnRoomEntered);
  }

  private void OnRoomEntered()
  {
    if (this._runState.CurrentRoom == null)
      return;
    AbstractRoom baseRoom = this._runState.BaseRoom;
    ((CanvasItem) this).Visible = baseRoom.RoomType != RoomType.Boss || this.ShouldOnlyShowSecondBossIcon;
    this.FocusMode = baseRoom.RoomType != RoomType.Boss || this.ShouldOnlyShowSecondBossIcon ? (Control.FocusModeEnum) 2L : (Control.FocusModeEnum) 0L;
    if (this.ShouldOnlyShowSecondBossIcon)
      this.RefreshBossIcon();
    this.RefreshSecondBossIconColor();
    if (!this._runState.CurrentRoom.IsVictoryRoom)
      return;
    ((CanvasItem) this._bossIcon).SetVisible(false);
    ((CanvasItem) this._bossIconOutline).SetVisible(false);
    ((CanvasItem) this._secondBossIcon)?.SetVisible(false);
    ((CanvasItem) this._secondBossIconOutline)?.SetVisible(false);
    this.FocusMode = (Control.FocusModeEnum) 0L;
    this.MouseFilter = (Control.MouseFilterEnum) 2L;
  }

  private bool ShouldOnlyShowSecondBossIcon
  {
    get
    {
      return this._runState.Map.SecondBossMapPoint != null && this._runState.CurrentMapPoint == this._runState.Map.BossMapPoint;
    }
  }

  private void OnActEntered() => this.RefreshBossIcon();

  public void RefreshBossIcon()
  {
    EncounterModel encounterModel = this.ShouldOnlyShowSecondBossIcon ? this._runState.Act.SecondBossEncounter : this._runState.Act.BossEncounter;
    string roomIconPath1 = ImageHelper.GetRoomIconPath(MapPointType.Boss, RoomType.Boss, encounterModel.Id);
    string roomIconOutlinePath1 = ImageHelper.GetRoomIconOutlinePath(MapPointType.Boss, RoomType.Boss, encounterModel.Id);
    this._bossIcon.Texture = PreloadManager.Cache.GetTexture2D(roomIconPath1);
    this._bossIconOutline.Texture = PreloadManager.Cache.GetTexture2D(roomIconOutlinePath1);
    EncounterModel secondBossEncounter = this._runState.Act.SecondBossEncounter;
    if (secondBossEncounter != null && !this.ShouldOnlyShowSecondBossIcon)
    {
      if (this._secondBossIcon == null)
      {
        this._secondBossIcon = GD.Load<PackedScene>("res://scenes/ui/top_bar/second_boss_icon.tscn").Instantiate<TextureRect>((PackedScene.GenEditState) 0L);
        this._secondBossIconOutline = ((Node) this._secondBossIcon).GetNode<TextureRect>(NodePath.op_Implicit("%Outline"));
        ((Control) this._secondBossIcon).MouseFilter = (Control.MouseFilterEnum) 1L;
        ((Control) this._secondBossIconOutline).MouseFilter = (Control.MouseFilterEnum) 1L;
        ((Node) this._bossIcon).AddChildSafely((Node) this._secondBossIcon);
        ((Control) this._secondBossIcon).Position = new Vector2(30f, 22f);
      }
      string roomIconPath2 = ImageHelper.GetRoomIconPath(MapPointType.Boss, RoomType.Boss, secondBossEncounter.Id);
      string roomIconOutlinePath2 = ImageHelper.GetRoomIconOutlinePath(MapPointType.Boss, RoomType.Boss, secondBossEncounter.Id);
      this._secondBossIcon.Texture = PreloadManager.Cache.GetTexture2D(roomIconPath2);
      this._secondBossIconOutline.Texture = PreloadManager.Cache.GetTexture2D(roomIconOutlinePath2);
      ((CanvasItem) this._secondBossIcon).Visible = true;
      this.RefreshSecondBossIconColor();
    }
    else
    {
      ((CanvasItem) this._secondBossIcon)?.SetVisible(false);
      ((CanvasItem) this._secondBossIconOutline)?.SetVisible(false);
    }
  }

  private void RefreshSecondBossIconColor()
  {
    if (!(((CanvasItem) this._secondBossIcon)?.Material is ShaderMaterial material1) || !(((CanvasItem) this._secondBossIconOutline)?.Material is ShaderMaterial material2))
      return;
    ActModel act = this._runState.Act;
    MapPoint currentMapPoint = this._runState.CurrentMapPoint;
    MapPoint bossMapPoint = this._runState.Map.BossMapPoint;
    MapPoint secondBossMapPoint = this._runState.Map.SecondBossMapPoint;
    Color color = currentMapPoint == bossMapPoint || currentMapPoint == secondBossMapPoint ? act.MapTraveledColor : act.MapUntraveledColor;
    material1.SetShaderParameter(NTopBarBossIcon._tintColor, Variant.op_Implicit(new Vector3(color.R, color.G, color.B)));
    material2.SetShaderParameter(NTopBarBossIcon._tintColor, Variant.op_Implicit(new Vector3(color.R, color.G, color.B)));
  }

  protected override void OnFocus()
  {
    EncounterModel bossEncounter = this._runState.Act.BossEncounter;
    EncounterModel secondBossEncounter = this._runState.Act.SecondBossEncounter;
    HoverTip hoverTip;
    if (secondBossEncounter != null && !this.ShouldOnlyShowSecondBossIcon)
    {
      NTopBarBossIcon._doubleBossHoverTipTitle.Add("BossName1", bossEncounter.Title);
      NTopBarBossIcon._doubleBossHoverTipTitle.Add("BossName2", secondBossEncounter.Title);
      NTopBarBossIcon._doubleBossHoverTipDescription.Add("BossName1", bossEncounter.Title);
      NTopBarBossIcon._doubleBossHoverTipDescription.Add("BossName2", secondBossEncounter.Title);
      hoverTip = new HoverTip(NTopBarBossIcon._doubleBossHoverTipTitle, NTopBarBossIcon._doubleBossHoverTipDescription);
    }
    else
    {
      NTopBarBossIcon._bossHoverTipTitle.Add("BossName", this.ShouldOnlyShowSecondBossIcon ? secondBossEncounter.Title : bossEncounter.Title);
      NTopBarBossIcon._bossHoverTipDescription.Add("BossName", this.ShouldOnlyShowSecondBossIcon ? secondBossEncounter.Title : bossEncounter.Title);
      hoverTip = new HoverTip(NTopBarBossIcon._bossHoverTipTitle, NTopBarBossIcon._bossHoverTipDescription);
    }
    NHoverTipSet.CreateAndShow((Control) this, (IHoverTip) hoverTip)?.SetGlobalPosition(Vector2.op_Addition(((Control) this._bossIcon).GlobalPosition, new Vector2(0.0f, this.Size.Y + 20f)), false);
  }

  protected override void OnUnfocus() => NHoverTipSet.Remove((Control) this);

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(9)
    {
      new MethodInfo(NTopBarBossIcon.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NTopBarBossIcon.MethodName._EnterTree, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NTopBarBossIcon.MethodName._ExitTree, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NTopBarBossIcon.MethodName.OnRoomEntered, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NTopBarBossIcon.MethodName.OnActEntered, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NTopBarBossIcon.MethodName.RefreshBossIcon, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NTopBarBossIcon.MethodName.RefreshSecondBossIconColor, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NTopBarBossIcon.MethodName.OnFocus, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NTopBarBossIcon.MethodName.OnUnfocus, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NTopBarBossIcon.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NTopBarBossIcon.MethodName._EnterTree) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._EnterTree();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NTopBarBossIcon.MethodName._ExitTree) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._ExitTree();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NTopBarBossIcon.MethodName.OnRoomEntered) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnRoomEntered();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NTopBarBossIcon.MethodName.OnActEntered) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnActEntered();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NTopBarBossIcon.MethodName.RefreshBossIcon) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.RefreshBossIcon();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NTopBarBossIcon.MethodName.RefreshSecondBossIconColor) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.RefreshSecondBossIconColor();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NTopBarBossIcon.MethodName.OnFocus) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnFocus();
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NTopBarBossIcon.MethodName.OnUnfocus) || ((NativeVariantPtrArgs) ref args).Count != 0)
      return base.InvokeGodotClassMethod(in method, args, out ret);
    this.OnUnfocus();
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NTopBarBossIcon.MethodName._Ready) || StringName.op_Equality(ref method, NTopBarBossIcon.MethodName._EnterTree) || StringName.op_Equality(ref method, NTopBarBossIcon.MethodName._ExitTree) || StringName.op_Equality(ref method, NTopBarBossIcon.MethodName.OnRoomEntered) || StringName.op_Equality(ref method, NTopBarBossIcon.MethodName.OnActEntered) || StringName.op_Equality(ref method, NTopBarBossIcon.MethodName.RefreshBossIcon) || StringName.op_Equality(ref method, NTopBarBossIcon.MethodName.RefreshSecondBossIconColor) || StringName.op_Equality(ref method, NTopBarBossIcon.MethodName.OnFocus) || StringName.op_Equality(ref method, NTopBarBossIcon.MethodName.OnUnfocus) || base.HasGodotClassMethod(in method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NTopBarBossIcon.PropertyName._bossIcon))
    {
      this._bossIcon = VariantUtils.ConvertTo<TextureRect>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NTopBarBossIcon.PropertyName._bossIconOutline))
    {
      this._bossIconOutline = VariantUtils.ConvertTo<TextureRect>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NTopBarBossIcon.PropertyName._secondBossIcon))
    {
      this._secondBossIcon = VariantUtils.ConvertTo<TextureRect>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NTopBarBossIcon.PropertyName._secondBossIconOutline))
      return base.SetGodotClassPropertyValue(in name, in value);
    this._secondBossIconOutline = VariantUtils.ConvertTo<TextureRect>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NTopBarBossIcon.PropertyName.ShouldOnlyShowSecondBossIcon))
    {
      ref godot_variant local = ref value;
      bool showSecondBossIcon = this.ShouldOnlyShowSecondBossIcon;
      godot_variant from = VariantUtils.CreateFrom<bool>(ref showSecondBossIcon);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NTopBarBossIcon.PropertyName._bossIcon))
    {
      value = VariantUtils.CreateFrom<TextureRect>(ref this._bossIcon);
      return true;
    }
    if (StringName.op_Equality(ref name, NTopBarBossIcon.PropertyName._bossIconOutline))
    {
      value = VariantUtils.CreateFrom<TextureRect>(ref this._bossIconOutline);
      return true;
    }
    if (StringName.op_Equality(ref name, NTopBarBossIcon.PropertyName._secondBossIcon))
    {
      value = VariantUtils.CreateFrom<TextureRect>(ref this._secondBossIcon);
      return true;
    }
    if (!StringName.op_Equality(ref name, NTopBarBossIcon.PropertyName._secondBossIconOutline))
      return base.GetGodotClassPropertyValue(in name, out value);
    value = VariantUtils.CreateFrom<TextureRect>(ref this._secondBossIconOutline);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NTopBarBossIcon.PropertyName._bossIcon, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NTopBarBossIcon.PropertyName._bossIconOutline, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NTopBarBossIcon.PropertyName._secondBossIcon, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NTopBarBossIcon.PropertyName._secondBossIconOutline, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 1L, NTopBarBossIcon.PropertyName.ShouldOnlyShowSecondBossIcon, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    base.SaveGodotObjectData(info);
    info.AddProperty(NTopBarBossIcon.PropertyName._bossIcon, Variant.From<TextureRect>(ref this._bossIcon));
    info.AddProperty(NTopBarBossIcon.PropertyName._bossIconOutline, Variant.From<TextureRect>(ref this._bossIconOutline));
    info.AddProperty(NTopBarBossIcon.PropertyName._secondBossIcon, Variant.From<TextureRect>(ref this._secondBossIcon));
    info.AddProperty(NTopBarBossIcon.PropertyName._secondBossIconOutline, Variant.From<TextureRect>(ref this._secondBossIconOutline));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    base.RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NTopBarBossIcon.PropertyName._bossIcon, ref variant1))
      this._bossIcon = ((Variant) ref variant1).As<TextureRect>();
    Variant variant2;
    if (info.TryGetProperty(NTopBarBossIcon.PropertyName._bossIconOutline, ref variant2))
      this._bossIconOutline = ((Variant) ref variant2).As<TextureRect>();
    Variant variant3;
    if (info.TryGetProperty(NTopBarBossIcon.PropertyName._secondBossIcon, ref variant3))
      this._secondBossIcon = ((Variant) ref variant3).As<TextureRect>();
    Variant variant4;
    if (!info.TryGetProperty(NTopBarBossIcon.PropertyName._secondBossIconOutline, ref variant4))
      return;
    this._secondBossIconOutline = ((Variant) ref variant4).As<TextureRect>();
  }

  public new class MethodName : NClickableControl.MethodName
  {
    public static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName _EnterTree = StringName.op_Implicit(nameof (_EnterTree));
    public static readonly StringName _ExitTree = StringName.op_Implicit(nameof (_ExitTree));
    public static readonly StringName OnRoomEntered = StringName.op_Implicit(nameof (OnRoomEntered));
    public static readonly StringName OnActEntered = StringName.op_Implicit(nameof (OnActEntered));
    public static readonly StringName RefreshBossIcon = StringName.op_Implicit(nameof (RefreshBossIcon));
    public static readonly StringName RefreshSecondBossIconColor = StringName.op_Implicit(nameof (RefreshSecondBossIconColor));
    public new static readonly StringName OnFocus = StringName.op_Implicit(nameof (OnFocus));
    public new static readonly StringName OnUnfocus = StringName.op_Implicit(nameof (OnUnfocus));
  }

  public new class PropertyName : NClickableControl.PropertyName
  {
    public static readonly StringName ShouldOnlyShowSecondBossIcon = StringName.op_Implicit(nameof (ShouldOnlyShowSecondBossIcon));
    public static readonly StringName _bossIcon = StringName.op_Implicit(nameof (_bossIcon));
    public static readonly StringName _bossIconOutline = StringName.op_Implicit(nameof (_bossIconOutline));
    public static readonly StringName _secondBossIcon = StringName.op_Implicit(nameof (_secondBossIcon));
    public static readonly StringName _secondBossIconOutline = StringName.op_Implicit(nameof (_secondBossIconOutline));
  }

  public new class SignalName : NClickableControl.SignalName
  {
  }
}
