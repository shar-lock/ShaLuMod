// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Screens.Map.NBossMapPoint
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Bindings.MegaSpine;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Map;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Runs;
using System.Collections.Generic;
using System.ComponentModel;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Screens.Map;

[ScriptPath("res://src/Core/Nodes/Screens/Map/NBossMapPoint.cs")]
public class NBossMapPoint : NMapPoint
{
  private static readonly StringName _mapColor = new StringName("map_color");
  private static readonly StringName _blackLayerColor = new StringName("black_layer_color");
  private Tween? _hoverTween;
  private ActModel _act;
  private bool _usesSpine;
  private Node2D _spriteContainer;
  private Node2D _spineSprite;
  private MegaSprite _animController;
  private ShaderMaterial _material;
  private TextureRect _placeholderImage;
  private TextureRect _placeholderOutline;

  protected override Color TraveledColor => StsColors.pathDotTraveled;

  protected override Color UntravelableColor => StsColors.red;

  protected override Color HoveredColor => StsColors.pathDotTraveled;

  protected override Vector2 HoverScale => Vector2.op_Multiply(Vector2.One, 1.05f);

  protected override Vector2 DownScale => Vector2.op_Multiply(Vector2.One, 1.02f);

  private static string BossMapPointPath => SceneHelper.GetScenePath("ui/boss_map_point");

  public static IEnumerable<string> AssetPaths
  {
    get
    {
      return (IEnumerable<string>) new \u003C\u003Ez__ReadOnlySingleElementList<string>(NBossMapPoint.BossMapPointPath);
    }
  }

  public static NBossMapPoint Create(MapPoint point, NMapScreen screen, IRunState runState)
  {
    NBossMapPoint nbossMapPoint = PreloadManager.Cache.GetScene(NBossMapPoint.BossMapPointPath).Instantiate<NBossMapPoint>((PackedScene.GenEditState) 0L);
    nbossMapPoint.Point = point;
    nbossMapPoint._screen = screen;
    nbossMapPoint._runState = runState;
    nbossMapPoint._act = runState.Act;
    return nbossMapPoint;
  }

  public override void _Ready()
  {
    this.ConnectSignals();
    this.Disable();
    this._spriteContainer = ((Node) this).GetNode<Node2D>(NodePath.op_Implicit("%SpriteContainer"));
    this._spineSprite = ((Node) this).GetNode<Node2D>(NodePath.op_Implicit("%SpineSprite"));
    this._animController = new MegaSprite(Variant.op_Implicit((GodotObject) this._spineSprite));
    EncounterModel encounterModel = this.Point == this._runState.Map.SecondBossMapPoint ? this._runState.Act.SecondBossEncounter : this._runState.Act.BossEncounter;
    if (encounterModel.BossNodeSpineResource != null)
    {
      this._usesSpine = true;
      ((CanvasItem) this._spineSprite).Visible = true;
      this._animController.SetSkeletonDataRes(encounterModel.BossNodeSpineResource);
      this._animController.GetAnimationState().AddAnimation("animation");
      this._material = (ShaderMaterial) this._animController.GetNormalMaterial();
    }
    else
    {
      this._usesSpine = false;
      ((CanvasItem) this._spineSprite).Visible = false;
      this._placeholderImage = ((Node) this).GetNode<TextureRect>(NodePath.op_Implicit("%PlaceholderImage"));
      this._placeholderOutline = ((Node) this).GetNode<TextureRect>(NodePath.op_Implicit("%PlaceholderOutline"));
      ((CanvasItem) this._placeholderImage).Visible = true;
      this._placeholderImage.Texture = PreloadManager.Cache.GetAsset<Texture2D>(encounterModel.BossNodePath + ".png");
      this._placeholderOutline.Texture = PreloadManager.Cache.GetAsset<Texture2D>(encounterModel.BossNodePath + "_outline.png");
      ((CanvasItem) this._placeholderImage).SelfModulate = this._act.MapTraveledColor;
      ((CanvasItem) this._placeholderOutline).SelfModulate = this._act.MapBgColor;
    }
    this.RefreshColorInstantly();
  }

  protected override void OnFocus()
  {
    base.OnFocus();
    if (!this.IsInputAllowed() || !this.IsTravelable)
      return;
    this._hoverTween?.Kill();
    this._hoverTween = ((Node) this).CreateTween().SetParallel(true);
    this._hoverTween.TweenProperty((GodotObject) this._spriteContainer, NodePath.op_Implicit("scale"), Variant.op_Implicit(this.HoverScale), 0.05);
    int num = this._usesSpine ? 1 : 0;
  }

  protected override void OnUnfocus()
  {
    base.OnUnfocus();
    this._hoverTween?.Kill();
    this._hoverTween = ((Node) this).CreateTween().SetParallel(true);
    this._hoverTween.TweenProperty((GodotObject) this._spriteContainer, NodePath.op_Implicit("scale"), Variant.op_Implicit(Vector2.One), 0.5).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 7L);
    int num = this._usesSpine ? 1 : 0;
  }

  protected override void OnPress()
  {
    if (!this.IsTravelable)
      return;
    this._hoverTween?.Kill();
    this._hoverTween = ((Node) this).CreateTween().SetParallel(true);
    this._hoverTween.TweenProperty((GodotObject) this._spriteContainer, NodePath.op_Implicit("scale"), Variant.op_Implicit(this.DownScale), 0.3).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 5L);
  }

  public override void OnSelected() => this.State = MapPointState.Traveled;

  protected override void RefreshColorInstantly()
  {
    if (this._usesSpine)
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
      if (flag)
        this._material.SetShaderParameter(NBossMapPoint._blackLayerColor, Variant.op_Implicit(this._act.MapTraveledColor));
      else
        this._material.SetShaderParameter(NBossMapPoint._blackLayerColor, Variant.op_Implicit(this._act.MapUntraveledColor));
      this._material.SetShaderParameter(NBossMapPoint._mapColor, Variant.op_Implicit(this._act.MapBgColor));
    }
    else
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
      ((CanvasItem) this._placeholderImage).SelfModulate = flag ? this._act.MapTraveledColor : this._act.MapUntraveledColor;
      ((CanvasItem) this._placeholderOutline).SelfModulate = this._act.MapBgColor;
    }
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(6)
    {
      new MethodInfo(NBossMapPoint.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NBossMapPoint.MethodName.OnFocus, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NBossMapPoint.MethodName.OnUnfocus, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NBossMapPoint.MethodName.OnPress, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NBossMapPoint.MethodName.OnSelected, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NBossMapPoint.MethodName.RefreshColorInstantly, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NBossMapPoint.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NBossMapPoint.MethodName.OnFocus) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnFocus();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NBossMapPoint.MethodName.OnUnfocus) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnUnfocus();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NBossMapPoint.MethodName.OnPress) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnPress();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NBossMapPoint.MethodName.OnSelected) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnSelected();
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NBossMapPoint.MethodName.RefreshColorInstantly) || ((NativeVariantPtrArgs) ref args).Count != 0)
      return base.InvokeGodotClassMethod(in method, args, out ret);
    this.RefreshColorInstantly();
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NBossMapPoint.MethodName._Ready) || StringName.op_Equality(ref method, NBossMapPoint.MethodName.OnFocus) || StringName.op_Equality(ref method, NBossMapPoint.MethodName.OnUnfocus) || StringName.op_Equality(ref method, NBossMapPoint.MethodName.OnPress) || StringName.op_Equality(ref method, NBossMapPoint.MethodName.OnSelected) || StringName.op_Equality(ref method, NBossMapPoint.MethodName.RefreshColorInstantly) || base.HasGodotClassMethod(in method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NBossMapPoint.PropertyName._hoverTween))
    {
      this._hoverTween = VariantUtils.ConvertTo<Tween>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NBossMapPoint.PropertyName._usesSpine))
    {
      this._usesSpine = VariantUtils.ConvertTo<bool>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NBossMapPoint.PropertyName._spriteContainer))
    {
      this._spriteContainer = VariantUtils.ConvertTo<Node2D>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NBossMapPoint.PropertyName._spineSprite))
    {
      this._spineSprite = VariantUtils.ConvertTo<Node2D>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NBossMapPoint.PropertyName._material))
    {
      this._material = VariantUtils.ConvertTo<ShaderMaterial>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NBossMapPoint.PropertyName._placeholderImage))
    {
      this._placeholderImage = VariantUtils.ConvertTo<TextureRect>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NBossMapPoint.PropertyName._placeholderOutline))
      return base.SetGodotClassPropertyValue(in name, in value);
    this._placeholderOutline = VariantUtils.ConvertTo<TextureRect>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NBossMapPoint.PropertyName.TraveledColor))
    {
      ref godot_variant local = ref value;
      Color traveledColor = this.TraveledColor;
      godot_variant from = VariantUtils.CreateFrom<Color>(ref traveledColor);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NBossMapPoint.PropertyName.UntravelableColor))
    {
      ref godot_variant local = ref value;
      Color untravelableColor = this.UntravelableColor;
      godot_variant from = VariantUtils.CreateFrom<Color>(ref untravelableColor);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NBossMapPoint.PropertyName.HoveredColor))
    {
      ref godot_variant local = ref value;
      Color hoveredColor = this.HoveredColor;
      godot_variant from = VariantUtils.CreateFrom<Color>(ref hoveredColor);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NBossMapPoint.PropertyName.HoverScale))
    {
      ref godot_variant local = ref value;
      Vector2 hoverScale = this.HoverScale;
      godot_variant from = VariantUtils.CreateFrom<Vector2>(ref hoverScale);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NBossMapPoint.PropertyName.DownScale))
    {
      ref godot_variant local = ref value;
      Vector2 downScale = this.DownScale;
      godot_variant from = VariantUtils.CreateFrom<Vector2>(ref downScale);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NBossMapPoint.PropertyName._hoverTween))
    {
      value = VariantUtils.CreateFrom<Tween>(ref this._hoverTween);
      return true;
    }
    if (StringName.op_Equality(ref name, NBossMapPoint.PropertyName._usesSpine))
    {
      value = VariantUtils.CreateFrom<bool>(ref this._usesSpine);
      return true;
    }
    if (StringName.op_Equality(ref name, NBossMapPoint.PropertyName._spriteContainer))
    {
      value = VariantUtils.CreateFrom<Node2D>(ref this._spriteContainer);
      return true;
    }
    if (StringName.op_Equality(ref name, NBossMapPoint.PropertyName._spineSprite))
    {
      value = VariantUtils.CreateFrom<Node2D>(ref this._spineSprite);
      return true;
    }
    if (StringName.op_Equality(ref name, NBossMapPoint.PropertyName._material))
    {
      value = VariantUtils.CreateFrom<ShaderMaterial>(ref this._material);
      return true;
    }
    if (StringName.op_Equality(ref name, NBossMapPoint.PropertyName._placeholderImage))
    {
      value = VariantUtils.CreateFrom<TextureRect>(ref this._placeholderImage);
      return true;
    }
    if (!StringName.op_Equality(ref name, NBossMapPoint.PropertyName._placeholderOutline))
      return base.GetGodotClassPropertyValue(in name, out value);
    value = VariantUtils.CreateFrom<TextureRect>(ref this._placeholderOutline);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 20L, NBossMapPoint.PropertyName.TraveledColor, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 20L, NBossMapPoint.PropertyName.UntravelableColor, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 20L, NBossMapPoint.PropertyName.HoveredColor, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 5L, NBossMapPoint.PropertyName.HoverScale, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 5L, NBossMapPoint.PropertyName.DownScale, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NBossMapPoint.PropertyName._hoverTween, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 1L, NBossMapPoint.PropertyName._usesSpine, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NBossMapPoint.PropertyName._spriteContainer, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NBossMapPoint.PropertyName._spineSprite, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NBossMapPoint.PropertyName._material, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NBossMapPoint.PropertyName._placeholderImage, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NBossMapPoint.PropertyName._placeholderOutline, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    base.SaveGodotObjectData(info);
    info.AddProperty(NBossMapPoint.PropertyName._hoverTween, Variant.From<Tween>(ref this._hoverTween));
    info.AddProperty(NBossMapPoint.PropertyName._usesSpine, Variant.From<bool>(ref this._usesSpine));
    info.AddProperty(NBossMapPoint.PropertyName._spriteContainer, Variant.From<Node2D>(ref this._spriteContainer));
    info.AddProperty(NBossMapPoint.PropertyName._spineSprite, Variant.From<Node2D>(ref this._spineSprite));
    info.AddProperty(NBossMapPoint.PropertyName._material, Variant.From<ShaderMaterial>(ref this._material));
    info.AddProperty(NBossMapPoint.PropertyName._placeholderImage, Variant.From<TextureRect>(ref this._placeholderImage));
    info.AddProperty(NBossMapPoint.PropertyName._placeholderOutline, Variant.From<TextureRect>(ref this._placeholderOutline));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    base.RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NBossMapPoint.PropertyName._hoverTween, ref variant1))
      this._hoverTween = ((Variant) ref variant1).As<Tween>();
    Variant variant2;
    if (info.TryGetProperty(NBossMapPoint.PropertyName._usesSpine, ref variant2))
      this._usesSpine = ((Variant) ref variant2).As<bool>();
    Variant variant3;
    if (info.TryGetProperty(NBossMapPoint.PropertyName._spriteContainer, ref variant3))
      this._spriteContainer = ((Variant) ref variant3).As<Node2D>();
    Variant variant4;
    if (info.TryGetProperty(NBossMapPoint.PropertyName._spineSprite, ref variant4))
      this._spineSprite = ((Variant) ref variant4).As<Node2D>();
    Variant variant5;
    if (info.TryGetProperty(NBossMapPoint.PropertyName._material, ref variant5))
      this._material = ((Variant) ref variant5).As<ShaderMaterial>();
    Variant variant6;
    if (info.TryGetProperty(NBossMapPoint.PropertyName._placeholderImage, ref variant6))
      this._placeholderImage = ((Variant) ref variant6).As<TextureRect>();
    Variant variant7;
    if (!info.TryGetProperty(NBossMapPoint.PropertyName._placeholderOutline, ref variant7))
      return;
    this._placeholderOutline = ((Variant) ref variant7).As<TextureRect>();
  }

  public new class MethodName : NMapPoint.MethodName
  {
    public new static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public new static readonly StringName OnFocus = StringName.op_Implicit(nameof (OnFocus));
    public new static readonly StringName OnUnfocus = StringName.op_Implicit(nameof (OnUnfocus));
    public new static readonly StringName OnPress = StringName.op_Implicit(nameof (OnPress));
    public new static readonly StringName OnSelected = StringName.op_Implicit(nameof (OnSelected));
    public new static readonly StringName RefreshColorInstantly = StringName.op_Implicit(nameof (RefreshColorInstantly));
  }

  public new class PropertyName : NMapPoint.PropertyName
  {
    public new static readonly StringName TraveledColor = StringName.op_Implicit(nameof (TraveledColor));
    public new static readonly StringName UntravelableColor = StringName.op_Implicit(nameof (UntravelableColor));
    public new static readonly StringName HoveredColor = StringName.op_Implicit(nameof (HoveredColor));
    public new static readonly StringName HoverScale = StringName.op_Implicit(nameof (HoverScale));
    public new static readonly StringName DownScale = StringName.op_Implicit(nameof (DownScale));
    public static readonly StringName _hoverTween = StringName.op_Implicit(nameof (_hoverTween));
    public static readonly StringName _usesSpine = StringName.op_Implicit(nameof (_usesSpine));
    public static readonly StringName _spriteContainer = StringName.op_Implicit(nameof (_spriteContainer));
    public static readonly StringName _spineSprite = StringName.op_Implicit(nameof (_spineSprite));
    public static readonly StringName _material = StringName.op_Implicit(nameof (_material));
    public static readonly StringName _placeholderImage = StringName.op_Implicit(nameof (_placeholderImage));
    public static readonly StringName _placeholderOutline = StringName.op_Implicit(nameof (_placeholderOutline));
  }

  public new class SignalName : NMapPoint.SignalName
  {
  }
}
