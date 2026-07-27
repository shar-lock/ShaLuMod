// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Screens.Map.NAncientMapPoint
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Map;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Random;
using MegaCrit.Sts2.Core.Runs;
using System.Collections.Generic;
using System.ComponentModel;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Screens.Map;

[ScriptPath("res://src/Core/Nodes/Screens/Map/NAncientMapPoint.cs")]
public class NAncientMapPoint : NMapPoint
{
  private TextureRect _icon;
  private TextureRect _outline;
  private Tween? _tween;
  private const float _pulseSpeed = 4f;
  private const float _scaleAmount = 0.05f;
  private const float _scaleBase = 1f;
  private float _elapsedTime = Rng.Chaotic.NextFloat(3140f);

  protected override Color TraveledColor => StsColors.pathDotTraveled;

  protected override Color UntravelableColor => StsColors.bossNodeUntraveled;

  protected override Color HoveredColor => StsColors.pathDotTraveled;

  protected override Vector2 HoverScale => Vector2.op_Multiply(Vector2.One, 1.1f);

  protected override Vector2 DownScale => Vector2.op_Multiply(Vector2.One, 0.9f);

  private static string UntravelableMaterialPath
  {
    get => "res://materials/boss_map_point_unavailable.tres";
  }

  private static string AncientMapPointPath => SceneHelper.GetScenePath("ui/ancient_map_point");

  public static IEnumerable<string> AssetPaths
  {
    get
    {
      return (IEnumerable<string>) new \u003C\u003Ez__ReadOnlyArray<string>(new string[2]
      {
        NAncientMapPoint.UntravelableMaterialPath,
        NAncientMapPoint.AncientMapPointPath
      });
    }
  }

  private Material? TargetMaterial
  {
    get
    {
      return this.IsTravelable || this.State == MapPointState.Traveled ? (Material) null : PreloadManager.Cache.GetMaterial(NAncientMapPoint.UntravelableMaterialPath);
    }
  }

  public static NAncientMapPoint Create(MapPoint point, NMapScreen screen, IRunState runState)
  {
    NAncientMapPoint nancientMapPoint = PreloadManager.Cache.GetScene(NAncientMapPoint.AncientMapPointPath).Instantiate<NAncientMapPoint>((PackedScene.GenEditState) 0L);
    nancientMapPoint.Point = point;
    nancientMapPoint._screen = screen;
    nancientMapPoint._runState = runState;
    return nancientMapPoint;
  }

  public override void _Ready()
  {
    this.ConnectSignals();
    this._icon = ((Node) this).GetNode<TextureRect>(NodePath.op_Implicit("Icon"));
    this._icon.Texture = this._runState.Act.Ancient.MapIcon;
    this._outline = ((Node) this).GetNode<TextureRect>(NodePath.op_Implicit("Icon/Outline"));
    this._outline.Texture = this._runState.Act.Ancient.MapIconOutline;
    ((CanvasItem) this._outline).Modulate = this._runState.Act.MapBgColor;
    this.RefreshColorInstantly();
  }

  public override void _Process(double delta)
  {
    if (!this._isEnabled)
      return;
    if (!this.IsFocused && this.IsInputAllowed())
    {
      this._elapsedTime += (float) delta * 4f;
      this.Scale = Vector2.op_Multiply(Vector2.One, (float) ((double) Mathf.Sin(this._elapsedTime) * 0.05000000074505806 + 1.0));
    }
    else
    {
      Vector2 scale = this.Scale;
      this.Scale = ((Vector2) ref scale).Lerp(Vector2.One, 0.5f);
    }
  }

  protected override void OnFocus()
  {
    base.OnFocus();
    if (!this.IsInputAllowed())
      return;
    this.AnimHover();
    if (!NControllerManager.Instance.IsUsingController)
      return;
    this._controllerSelectionReticle.OnSelect();
  }

  protected override void OnUnfocus()
  {
    base.OnUnfocus();
    this.AnimUnhover();
    this._controllerSelectionReticle.OnDeselect();
  }

  protected override void OnPress()
  {
    if (!this.IsTravelable)
      return;
    this.AnimPressDown();
    this._controllerSelectionReticle.OnDeselect();
  }

  public override void OnSelected()
  {
    this.State = MapPointState.Traveled;
    this._tween?.Kill();
    this._tween = ((Node) this).CreateTween().SetParallel(true);
    this._tween.TweenProperty((GodotObject) this._icon, NodePath.op_Implicit("scale"), Variant.op_Implicit(Vector2.One), 0.3).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 10L);
    this._tween.TweenProperty((GodotObject) this._icon, NodePath.op_Implicit("self_modulate"), Variant.op_Implicit(this.TargetColor), 0.3).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 5L);
    this._tween.TweenProperty((GodotObject) this._outline, NodePath.op_Implicit("modulate"), Variant.op_Implicit(this._runState.Act.MapBgColor), 0.3).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 5L);
  }

  protected override void RefreshColorInstantly()
  {
    ((CanvasItem) this._icon).SelfModulate = this.TargetColor;
  }

  private void AnimHover()
  {
    this._tween?.Kill();
    this._tween = ((Node) this).CreateTween().SetParallel(true);
    this._tween.TweenProperty((GodotObject) this._icon, NodePath.op_Implicit("scale"), Variant.op_Implicit(this.HoverScale), 0.05);
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
    this._tween.TweenProperty((GodotObject) this._icon, NodePath.op_Implicit("self_modulate"), Variant.op_Implicit(this.TargetColor), 0.5).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 5L);
    this._tween.TweenProperty((GodotObject) this._outline, NodePath.op_Implicit("modulate"), Variant.op_Implicit(this._runState.Act.MapBgColor), 0.5).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 5L);
  }

  private void AnimPressDown()
  {
    this._tween?.Kill();
    this._tween = ((Node) this).CreateTween().SetParallel(true);
    this._tween.TweenProperty((GodotObject) this._icon, NodePath.op_Implicit("scale"), Variant.op_Implicit(this.DownScale), 0.3).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 7L);
    this._tween.TweenProperty((GodotObject) this._outline, NodePath.op_Implicit("modulate"), Variant.op_Implicit(this._runState.Act.MapBgColor), 0.5).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 5L);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(10)
    {
      new MethodInfo(NAncientMapPoint.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NAncientMapPoint.MethodName._Process, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 3L, StringName.op_Implicit("delta"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NAncientMapPoint.MethodName.OnFocus, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NAncientMapPoint.MethodName.OnUnfocus, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NAncientMapPoint.MethodName.OnPress, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NAncientMapPoint.MethodName.OnSelected, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NAncientMapPoint.MethodName.RefreshColorInstantly, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NAncientMapPoint.MethodName.AnimHover, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NAncientMapPoint.MethodName.AnimUnhover, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NAncientMapPoint.MethodName.AnimPressDown, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NAncientMapPoint.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NAncientMapPoint.MethodName._Process) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      ((Node) this)._Process(VariantUtils.ConvertTo<double>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NAncientMapPoint.MethodName.OnFocus) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnFocus();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NAncientMapPoint.MethodName.OnUnfocus) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnUnfocus();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NAncientMapPoint.MethodName.OnPress) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnPress();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NAncientMapPoint.MethodName.OnSelected) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnSelected();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NAncientMapPoint.MethodName.RefreshColorInstantly) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.RefreshColorInstantly();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NAncientMapPoint.MethodName.AnimHover) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.AnimHover();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NAncientMapPoint.MethodName.AnimUnhover) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.AnimUnhover();
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NAncientMapPoint.MethodName.AnimPressDown) || ((NativeVariantPtrArgs) ref args).Count != 0)
      return base.InvokeGodotClassMethod(in method, args, out ret);
    this.AnimPressDown();
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NAncientMapPoint.MethodName._Ready) || StringName.op_Equality(ref method, NAncientMapPoint.MethodName._Process) || StringName.op_Equality(ref method, NAncientMapPoint.MethodName.OnFocus) || StringName.op_Equality(ref method, NAncientMapPoint.MethodName.OnUnfocus) || StringName.op_Equality(ref method, NAncientMapPoint.MethodName.OnPress) || StringName.op_Equality(ref method, NAncientMapPoint.MethodName.OnSelected) || StringName.op_Equality(ref method, NAncientMapPoint.MethodName.RefreshColorInstantly) || StringName.op_Equality(ref method, NAncientMapPoint.MethodName.AnimHover) || StringName.op_Equality(ref method, NAncientMapPoint.MethodName.AnimUnhover) || StringName.op_Equality(ref method, NAncientMapPoint.MethodName.AnimPressDown) || base.HasGodotClassMethod(in method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NAncientMapPoint.PropertyName._icon))
    {
      this._icon = VariantUtils.ConvertTo<TextureRect>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NAncientMapPoint.PropertyName._outline))
    {
      this._outline = VariantUtils.ConvertTo<TextureRect>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NAncientMapPoint.PropertyName._tween))
    {
      this._tween = VariantUtils.ConvertTo<Tween>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NAncientMapPoint.PropertyName._elapsedTime))
      return base.SetGodotClassPropertyValue(in name, in value);
    this._elapsedTime = VariantUtils.ConvertTo<float>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NAncientMapPoint.PropertyName.TraveledColor))
    {
      ref godot_variant local = ref value;
      Color traveledColor = this.TraveledColor;
      godot_variant from = VariantUtils.CreateFrom<Color>(ref traveledColor);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NAncientMapPoint.PropertyName.UntravelableColor))
    {
      ref godot_variant local = ref value;
      Color untravelableColor = this.UntravelableColor;
      godot_variant from = VariantUtils.CreateFrom<Color>(ref untravelableColor);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NAncientMapPoint.PropertyName.HoveredColor))
    {
      ref godot_variant local = ref value;
      Color hoveredColor = this.HoveredColor;
      godot_variant from = VariantUtils.CreateFrom<Color>(ref hoveredColor);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NAncientMapPoint.PropertyName.HoverScale))
    {
      ref godot_variant local = ref value;
      Vector2 hoverScale = this.HoverScale;
      godot_variant from = VariantUtils.CreateFrom<Vector2>(ref hoverScale);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NAncientMapPoint.PropertyName.DownScale))
    {
      ref godot_variant local = ref value;
      Vector2 downScale = this.DownScale;
      godot_variant from = VariantUtils.CreateFrom<Vector2>(ref downScale);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NAncientMapPoint.PropertyName.TargetMaterial))
    {
      ref godot_variant local = ref value;
      Material targetMaterial = this.TargetMaterial;
      godot_variant from = VariantUtils.CreateFrom<Material>(ref targetMaterial);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NAncientMapPoint.PropertyName._icon))
    {
      value = VariantUtils.CreateFrom<TextureRect>(ref this._icon);
      return true;
    }
    if (StringName.op_Equality(ref name, NAncientMapPoint.PropertyName._outline))
    {
      value = VariantUtils.CreateFrom<TextureRect>(ref this._outline);
      return true;
    }
    if (StringName.op_Equality(ref name, NAncientMapPoint.PropertyName._tween))
    {
      value = VariantUtils.CreateFrom<Tween>(ref this._tween);
      return true;
    }
    if (!StringName.op_Equality(ref name, NAncientMapPoint.PropertyName._elapsedTime))
      return base.GetGodotClassPropertyValue(in name, out value);
    value = VariantUtils.CreateFrom<float>(ref this._elapsedTime);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NAncientMapPoint.PropertyName._icon, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NAncientMapPoint.PropertyName._outline, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NAncientMapPoint.PropertyName._tween, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 20L, NAncientMapPoint.PropertyName.TraveledColor, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 20L, NAncientMapPoint.PropertyName.UntravelableColor, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 20L, NAncientMapPoint.PropertyName.HoveredColor, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 5L, NAncientMapPoint.PropertyName.HoverScale, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 5L, NAncientMapPoint.PropertyName.DownScale, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 3L, NAncientMapPoint.PropertyName._elapsedTime, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NAncientMapPoint.PropertyName.TargetMaterial, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    base.SaveGodotObjectData(info);
    info.AddProperty(NAncientMapPoint.PropertyName._icon, Variant.From<TextureRect>(ref this._icon));
    info.AddProperty(NAncientMapPoint.PropertyName._outline, Variant.From<TextureRect>(ref this._outline));
    info.AddProperty(NAncientMapPoint.PropertyName._tween, Variant.From<Tween>(ref this._tween));
    info.AddProperty(NAncientMapPoint.PropertyName._elapsedTime, Variant.From<float>(ref this._elapsedTime));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    base.RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NAncientMapPoint.PropertyName._icon, ref variant1))
      this._icon = ((Variant) ref variant1).As<TextureRect>();
    Variant variant2;
    if (info.TryGetProperty(NAncientMapPoint.PropertyName._outline, ref variant2))
      this._outline = ((Variant) ref variant2).As<TextureRect>();
    Variant variant3;
    if (info.TryGetProperty(NAncientMapPoint.PropertyName._tween, ref variant3))
      this._tween = ((Variant) ref variant3).As<Tween>();
    Variant variant4;
    if (!info.TryGetProperty(NAncientMapPoint.PropertyName._elapsedTime, ref variant4))
      return;
    this._elapsedTime = ((Variant) ref variant4).As<float>();
  }

  public new class MethodName : NMapPoint.MethodName
  {
    public new static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName _Process = StringName.op_Implicit(nameof (_Process));
    public new static readonly StringName OnFocus = StringName.op_Implicit(nameof (OnFocus));
    public new static readonly StringName OnUnfocus = StringName.op_Implicit(nameof (OnUnfocus));
    public new static readonly StringName OnPress = StringName.op_Implicit(nameof (OnPress));
    public new static readonly StringName OnSelected = StringName.op_Implicit(nameof (OnSelected));
    public new static readonly StringName RefreshColorInstantly = StringName.op_Implicit(nameof (RefreshColorInstantly));
    public static readonly StringName AnimHover = StringName.op_Implicit(nameof (AnimHover));
    public static readonly StringName AnimUnhover = StringName.op_Implicit(nameof (AnimUnhover));
    public static readonly StringName AnimPressDown = StringName.op_Implicit(nameof (AnimPressDown));
  }

  public new class PropertyName : NMapPoint.PropertyName
  {
    public new static readonly StringName TraveledColor = StringName.op_Implicit(nameof (TraveledColor));
    public new static readonly StringName UntravelableColor = StringName.op_Implicit(nameof (UntravelableColor));
    public new static readonly StringName HoveredColor = StringName.op_Implicit(nameof (HoveredColor));
    public new static readonly StringName HoverScale = StringName.op_Implicit(nameof (HoverScale));
    public new static readonly StringName DownScale = StringName.op_Implicit(nameof (DownScale));
    public static readonly StringName TargetMaterial = StringName.op_Implicit(nameof (TargetMaterial));
    public static readonly StringName _icon = StringName.op_Implicit(nameof (_icon));
    public static readonly StringName _outline = StringName.op_Implicit(nameof (_outline));
    public static readonly StringName _tween = StringName.op_Implicit(nameof (_tween));
    public static readonly StringName _elapsedTime = StringName.op_Implicit(nameof (_elapsedTime));
  }

  public new class SignalName : NMapPoint.SignalName
  {
  }
}
