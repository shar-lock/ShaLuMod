// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Screens.Map.NMapLegendItem
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Map;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.HoverTips;
using System;
using System.Collections.Generic;
using System.ComponentModel;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Screens.Map;

[ScriptPath("res://src/Core/Nodes/Screens/Map/NMapLegendItem.cs")]
public class NMapLegendItem : NButton
{
  private TextureRect _icon;
  private HoverTip _hoverTip;
  private Tween? _scaleDownTween;
  private static readonly Vector2 _hoverScale = Vector2.op_Multiply(Vector2.One, 1.25f);
  private const float _unhoverAnimDur = 0.5f;
  private MapPointType _pointType;

  public override void _Ready()
  {
    this.ConnectSignals();
    this.SetLocalizedFields(StringName.op_Implicit(((Node) this).Name));
    this.SetMapPointType(StringName.op_Implicit(((Node) this).Name));
    this._icon = ((Node) this).GetNode<TextureRect>(NodePath.op_Implicit("Icon"));
  }

  private void SetMapPointType(string name)
  {
    MapPointType mapPointType;
    switch (name)
    {
      case "UnknownLegendItem":
        mapPointType = MapPointType.Unknown;
        break;
      case "MerchantLegendItem":
        mapPointType = MapPointType.Shop;
        break;
      case "TreasureLegendItem":
        mapPointType = MapPointType.Treasure;
        break;
      case "RestSiteLegendItem":
        mapPointType = MapPointType.RestSite;
        break;
      case "EnemyLegendItem":
        mapPointType = MapPointType.Monster;
        break;
      case "EliteLegendItem":
        mapPointType = MapPointType.Elite;
        break;
      default:
        throw new ArgumentOutOfRangeException($"Unknown Node {name} when setting MapLegend localization.");
    }
    this._pointType = mapPointType;
  }

  private void SetLocalizedFields(string name)
  {
    string str1;
    switch (name)
    {
      case "UnknownLegendItem":
        str1 = "LEGEND_UNKNOWN";
        break;
      case "MerchantLegendItem":
        str1 = "LEGEND_MERCHANT";
        break;
      case "TreasureLegendItem":
        str1 = "LEGEND_TREASURE";
        break;
      case "RestSiteLegendItem":
        str1 = "LEGEND_REST";
        break;
      case "EnemyLegendItem":
        str1 = "LEGEND_ENEMY";
        break;
      case "EliteLegendItem":
        str1 = "LEGEND_ELITE";
        break;
      default:
        throw new ArgumentOutOfRangeException($"Unknown Node {name} when setting MapLegend localization.");
    }
    string str2 = str1;
    ((Node) this).GetNode<MegaLabel>(NodePath.op_Implicit("MegaLabel")).SetTextAutoSize(new LocString("map", str2 + ".title").GetFormattedText());
    this._hoverTip = new HoverTip(new LocString("map", str2 + ".hoverTip.title"), new LocString("map", str2 + ".hoverTip.description"));
  }

  protected override void OnFocus()
  {
    this._scaleDownTween?.Kill();
    ((Control) this._icon).Scale = NMapLegendItem._hoverScale;
    NHoverTipSet andShow = NHoverTipSet.CreateAndShow((Control) this, (IHoverTip) this._hoverTip);
    Control parent = ((Node) this).GetParent<Control>();
    andShow?.SetGlobalPosition(Vector2.op_Addition(parent.GlobalPosition, new Vector2(parent.Size.X - andShow.Size.X, parent.Size.Y)), false);
    NMapScreen.Instance.HighlightPointType(this._pointType);
  }

  protected override void OnUnfocus()
  {
    this._scaleDownTween = ((Node) this).CreateTween().SetParallel(true);
    this._scaleDownTween.TweenProperty((GodotObject) this._icon, NodePath.op_Implicit("scale"), Variant.op_Implicit(Vector2.One), 0.5).From(Variant.op_Implicit(NMapLegendItem._hoverScale)).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 5L);
    NMapScreen.Instance.HighlightPointType(MapPointType.Unassigned);
    NHoverTipSet.Remove((Control) this);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(5)
    {
      new MethodInfo(NMapLegendItem.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NMapLegendItem.MethodName.SetMapPointType, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 4L, StringName.op_Implicit("name"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NMapLegendItem.MethodName.SetLocalizedFields, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 4L, StringName.op_Implicit("name"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NMapLegendItem.MethodName.OnFocus, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NMapLegendItem.MethodName.OnUnfocus, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NMapLegendItem.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NMapLegendItem.MethodName.SetMapPointType) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.SetMapPointType(VariantUtils.ConvertTo<string>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NMapLegendItem.MethodName.SetLocalizedFields) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.SetLocalizedFields(VariantUtils.ConvertTo<string>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NMapLegendItem.MethodName.OnFocus) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnFocus();
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NMapLegendItem.MethodName.OnUnfocus) || ((NativeVariantPtrArgs) ref args).Count != 0)
      return base.InvokeGodotClassMethod(in method, args, out ret);
    this.OnUnfocus();
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NMapLegendItem.MethodName._Ready) || StringName.op_Equality(ref method, NMapLegendItem.MethodName.SetMapPointType) || StringName.op_Equality(ref method, NMapLegendItem.MethodName.SetLocalizedFields) || StringName.op_Equality(ref method, NMapLegendItem.MethodName.OnFocus) || StringName.op_Equality(ref method, NMapLegendItem.MethodName.OnUnfocus) || base.HasGodotClassMethod(in method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NMapLegendItem.PropertyName._icon))
    {
      this._icon = VariantUtils.ConvertTo<TextureRect>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NMapLegendItem.PropertyName._scaleDownTween))
    {
      this._scaleDownTween = VariantUtils.ConvertTo<Tween>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NMapLegendItem.PropertyName._pointType))
      return base.SetGodotClassPropertyValue(in name, in value);
    this._pointType = VariantUtils.ConvertTo<MapPointType>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NMapLegendItem.PropertyName._icon))
    {
      value = VariantUtils.CreateFrom<TextureRect>(ref this._icon);
      return true;
    }
    if (StringName.op_Equality(ref name, NMapLegendItem.PropertyName._scaleDownTween))
    {
      value = VariantUtils.CreateFrom<Tween>(ref this._scaleDownTween);
      return true;
    }
    if (!StringName.op_Equality(ref name, NMapLegendItem.PropertyName._pointType))
      return base.GetGodotClassPropertyValue(in name, out value);
    value = VariantUtils.CreateFrom<MapPointType>(ref this._pointType);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NMapLegendItem.PropertyName._icon, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NMapLegendItem.PropertyName._scaleDownTween, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 2L, NMapLegendItem.PropertyName._pointType, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    base.SaveGodotObjectData(info);
    info.AddProperty(NMapLegendItem.PropertyName._icon, Variant.From<TextureRect>(ref this._icon));
    info.AddProperty(NMapLegendItem.PropertyName._scaleDownTween, Variant.From<Tween>(ref this._scaleDownTween));
    info.AddProperty(NMapLegendItem.PropertyName._pointType, Variant.From<MapPointType>(ref this._pointType));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    base.RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NMapLegendItem.PropertyName._icon, ref variant1))
      this._icon = ((Variant) ref variant1).As<TextureRect>();
    Variant variant2;
    if (info.TryGetProperty(NMapLegendItem.PropertyName._scaleDownTween, ref variant2))
      this._scaleDownTween = ((Variant) ref variant2).As<Tween>();
    Variant variant3;
    if (!info.TryGetProperty(NMapLegendItem.PropertyName._pointType, ref variant3))
      return;
    this._pointType = ((Variant) ref variant3).As<MapPointType>();
  }

  public new class MethodName : NButton.MethodName
  {
    public new static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName SetMapPointType = StringName.op_Implicit(nameof (SetMapPointType));
    public static readonly StringName SetLocalizedFields = StringName.op_Implicit(nameof (SetLocalizedFields));
    public new static readonly StringName OnFocus = StringName.op_Implicit(nameof (OnFocus));
    public new static readonly StringName OnUnfocus = StringName.op_Implicit(nameof (OnUnfocus));
  }

  public new class PropertyName : NButton.PropertyName
  {
    public static readonly StringName _icon = StringName.op_Implicit(nameof (_icon));
    public static readonly StringName _scaleDownTween = StringName.op_Implicit(nameof (_scaleDownTween));
    public static readonly StringName _pointType = StringName.op_Implicit(nameof (_pointType));
  }

  public new class SignalName : NButton.SignalName
  {
  }
}
