// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Screens.Timeline.NEpochCard
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Random;
using MegaCrit.Sts2.Core.Timeline;
using System.Collections.Generic;
using System.ComponentModel;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Screens.Timeline;

[ScriptPath("res://src/Core/Nodes/Screens/Timeline/NEpochCard.cs")]
public class NEpochCard : Control
{
  private TextureRect _glow;
  private TextureRect _mask;
  private TextureRect _portrait;
  private bool _isHovered;
  private bool _isHoverable = true;
  private bool _isHeld;
  private bool _isWigglyUnlockPreviewMode;
  private Tween? _glowTween;
  private Vector2 _targetScale;
  private float _time;
  private float _noiseSpeed = 0.25f;
  private FastNoiseLite _noise;
  private Tween? _denyTween;
  private Tween? _transparencyTween;
  private Tween? _scaleTween;
  private Color _blueGlowColor = new Color("2de5ff80");
  private Color _goldGlowColor = new Color("ffd92e80");

  public void Init(EpochModel epochModel)
  {
    this._glow = ((Node) this).GetNode<TextureRect>(NodePath.op_Implicit("%GlowPlaceholder"));
    this._portrait = ((Node) this).GetNode<TextureRect>(NodePath.op_Implicit("%Portrait"));
    this._portrait.Texture = epochModel.RealPortrait;
    this.Scale = Vector2.One;
  }

  public override void _Ready()
  {
    this._mask = ((Node) this).GetNode<TextureRect>(NodePath.op_Implicit("%Mask"));
  }

  public override void _Process(double delta)
  {
    if (!this._isWigglyUnlockPreviewMode)
      return;
    this._time += this._noiseSpeed * (float) delta;
    this.Position = Vector2.op_Subtraction(new Vector2(42f * ((Noise) this._noise).GetNoise2D(this._time, 0.0f), 42f * ((Noise) this._noise).GetNoise2D(0.0f, this._time)), this.GetPivotOffset());
  }

  public void SetToWigglyUnlockPreviewMode()
  {
    this._noise = new FastNoiseLite();
    this._noise.Frequency = 0.2f;
    this._noise.SetSeed(Rng.Chaotic.NextInt());
    this._isWigglyUnlockPreviewMode = true;
    this.MouseFilter = (Control.MouseFilterEnum) 2L;
    this.Scale = Vector2.op_Multiply(Vector2.One, 1.2f);
  }

  private void GlowFlash()
  {
    ((CanvasItem) this).Modulate = new Color(1f, 1f, 1f, 0.75f);
    ((CanvasItem) this._glow).Modulate = Colors.Gold;
    this._glowTween?.Kill();
    this._glowTween = ((Node) this).CreateTween().SetParallel(true);
    this._glowTween.TweenProperty((GodotObject) this._glow, NodePath.op_Implicit("modulate:a"), Variant.op_Implicit(0.5f), 1.0).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 5L);
    this._glowTween.TweenProperty((GodotObject) this._glow, NodePath.op_Implicit("scale"), Variant.op_Implicit(Vector2.One), 1.0).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 5L).From(Variant.op_Implicit(Vector2.op_Multiply(Vector2.One, 1.5f)));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(4)
    {
      new MethodInfo(NEpochCard.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NEpochCard.MethodName._Process, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 3L, StringName.op_Implicit("delta"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NEpochCard.MethodName.SetToWigglyUnlockPreviewMode, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NEpochCard.MethodName.GlowFlash, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NEpochCard.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NEpochCard.MethodName._Process) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      ((Node) this)._Process(VariantUtils.ConvertTo<double>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NEpochCard.MethodName.SetToWigglyUnlockPreviewMode) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.SetToWigglyUnlockPreviewMode();
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NEpochCard.MethodName.GlowFlash) || ((NativeVariantPtrArgs) ref args).Count != 0)
      return base.InvokeGodotClassMethod(ref method, args, ref ret);
    this.GlowFlash();
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NEpochCard.MethodName._Ready) || StringName.op_Equality(ref method, NEpochCard.MethodName._Process) || StringName.op_Equality(ref method, NEpochCard.MethodName.SetToWigglyUnlockPreviewMode) || StringName.op_Equality(ref method, NEpochCard.MethodName.GlowFlash) || base.HasGodotClassMethod(ref method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NEpochCard.PropertyName._glow))
    {
      this._glow = VariantUtils.ConvertTo<TextureRect>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NEpochCard.PropertyName._mask))
    {
      this._mask = VariantUtils.ConvertTo<TextureRect>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NEpochCard.PropertyName._portrait))
    {
      this._portrait = VariantUtils.ConvertTo<TextureRect>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NEpochCard.PropertyName._isHovered))
    {
      this._isHovered = VariantUtils.ConvertTo<bool>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NEpochCard.PropertyName._isHoverable))
    {
      this._isHoverable = VariantUtils.ConvertTo<bool>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NEpochCard.PropertyName._isHeld))
    {
      this._isHeld = VariantUtils.ConvertTo<bool>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NEpochCard.PropertyName._isWigglyUnlockPreviewMode))
    {
      this._isWigglyUnlockPreviewMode = VariantUtils.ConvertTo<bool>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NEpochCard.PropertyName._glowTween))
    {
      this._glowTween = VariantUtils.ConvertTo<Tween>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NEpochCard.PropertyName._targetScale))
    {
      this._targetScale = VariantUtils.ConvertTo<Vector2>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NEpochCard.PropertyName._time))
    {
      this._time = VariantUtils.ConvertTo<float>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NEpochCard.PropertyName._noiseSpeed))
    {
      this._noiseSpeed = VariantUtils.ConvertTo<float>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NEpochCard.PropertyName._noise))
    {
      this._noise = VariantUtils.ConvertTo<FastNoiseLite>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NEpochCard.PropertyName._denyTween))
    {
      this._denyTween = VariantUtils.ConvertTo<Tween>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NEpochCard.PropertyName._transparencyTween))
    {
      this._transparencyTween = VariantUtils.ConvertTo<Tween>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NEpochCard.PropertyName._scaleTween))
    {
      this._scaleTween = VariantUtils.ConvertTo<Tween>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NEpochCard.PropertyName._blueGlowColor))
    {
      this._blueGlowColor = VariantUtils.ConvertTo<Color>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NEpochCard.PropertyName._goldGlowColor))
      return ((GodotObject) this).SetGodotClassPropertyValue(ref name, ref value);
    this._goldGlowColor = VariantUtils.ConvertTo<Color>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NEpochCard.PropertyName._glow))
    {
      value = VariantUtils.CreateFrom<TextureRect>(ref this._glow);
      return true;
    }
    if (StringName.op_Equality(ref name, NEpochCard.PropertyName._mask))
    {
      value = VariantUtils.CreateFrom<TextureRect>(ref this._mask);
      return true;
    }
    if (StringName.op_Equality(ref name, NEpochCard.PropertyName._portrait))
    {
      value = VariantUtils.CreateFrom<TextureRect>(ref this._portrait);
      return true;
    }
    if (StringName.op_Equality(ref name, NEpochCard.PropertyName._isHovered))
    {
      value = VariantUtils.CreateFrom<bool>(ref this._isHovered);
      return true;
    }
    if (StringName.op_Equality(ref name, NEpochCard.PropertyName._isHoverable))
    {
      value = VariantUtils.CreateFrom<bool>(ref this._isHoverable);
      return true;
    }
    if (StringName.op_Equality(ref name, NEpochCard.PropertyName._isHeld))
    {
      value = VariantUtils.CreateFrom<bool>(ref this._isHeld);
      return true;
    }
    if (StringName.op_Equality(ref name, NEpochCard.PropertyName._isWigglyUnlockPreviewMode))
    {
      value = VariantUtils.CreateFrom<bool>(ref this._isWigglyUnlockPreviewMode);
      return true;
    }
    if (StringName.op_Equality(ref name, NEpochCard.PropertyName._glowTween))
    {
      value = VariantUtils.CreateFrom<Tween>(ref this._glowTween);
      return true;
    }
    if (StringName.op_Equality(ref name, NEpochCard.PropertyName._targetScale))
    {
      value = VariantUtils.CreateFrom<Vector2>(ref this._targetScale);
      return true;
    }
    if (StringName.op_Equality(ref name, NEpochCard.PropertyName._time))
    {
      value = VariantUtils.CreateFrom<float>(ref this._time);
      return true;
    }
    if (StringName.op_Equality(ref name, NEpochCard.PropertyName._noiseSpeed))
    {
      value = VariantUtils.CreateFrom<float>(ref this._noiseSpeed);
      return true;
    }
    if (StringName.op_Equality(ref name, NEpochCard.PropertyName._noise))
    {
      value = VariantUtils.CreateFrom<FastNoiseLite>(ref this._noise);
      return true;
    }
    if (StringName.op_Equality(ref name, NEpochCard.PropertyName._denyTween))
    {
      value = VariantUtils.CreateFrom<Tween>(ref this._denyTween);
      return true;
    }
    if (StringName.op_Equality(ref name, NEpochCard.PropertyName._transparencyTween))
    {
      value = VariantUtils.CreateFrom<Tween>(ref this._transparencyTween);
      return true;
    }
    if (StringName.op_Equality(ref name, NEpochCard.PropertyName._scaleTween))
    {
      value = VariantUtils.CreateFrom<Tween>(ref this._scaleTween);
      return true;
    }
    if (StringName.op_Equality(ref name, NEpochCard.PropertyName._blueGlowColor))
    {
      value = VariantUtils.CreateFrom<Color>(ref this._blueGlowColor);
      return true;
    }
    if (!StringName.op_Equality(ref name, NEpochCard.PropertyName._goldGlowColor))
      return ((GodotObject) this).GetGodotClassPropertyValue(ref name, ref value);
    value = VariantUtils.CreateFrom<Color>(ref this._goldGlowColor);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NEpochCard.PropertyName._glow, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NEpochCard.PropertyName._mask, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NEpochCard.PropertyName._portrait, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 1L, NEpochCard.PropertyName._isHovered, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 1L, NEpochCard.PropertyName._isHoverable, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 1L, NEpochCard.PropertyName._isHeld, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 1L, NEpochCard.PropertyName._isWigglyUnlockPreviewMode, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NEpochCard.PropertyName._glowTween, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 5L, NEpochCard.PropertyName._targetScale, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 3L, NEpochCard.PropertyName._time, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 3L, NEpochCard.PropertyName._noiseSpeed, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NEpochCard.PropertyName._noise, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NEpochCard.PropertyName._denyTween, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NEpochCard.PropertyName._transparencyTween, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NEpochCard.PropertyName._scaleTween, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 20L, NEpochCard.PropertyName._blueGlowColor, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 20L, NEpochCard.PropertyName._goldGlowColor, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).SaveGodotObjectData(info);
    info.AddProperty(NEpochCard.PropertyName._glow, Variant.From<TextureRect>(ref this._glow));
    info.AddProperty(NEpochCard.PropertyName._mask, Variant.From<TextureRect>(ref this._mask));
    info.AddProperty(NEpochCard.PropertyName._portrait, Variant.From<TextureRect>(ref this._portrait));
    info.AddProperty(NEpochCard.PropertyName._isHovered, Variant.From<bool>(ref this._isHovered));
    info.AddProperty(NEpochCard.PropertyName._isHoverable, Variant.From<bool>(ref this._isHoverable));
    info.AddProperty(NEpochCard.PropertyName._isHeld, Variant.From<bool>(ref this._isHeld));
    info.AddProperty(NEpochCard.PropertyName._isWigglyUnlockPreviewMode, Variant.From<bool>(ref this._isWigglyUnlockPreviewMode));
    info.AddProperty(NEpochCard.PropertyName._glowTween, Variant.From<Tween>(ref this._glowTween));
    info.AddProperty(NEpochCard.PropertyName._targetScale, Variant.From<Vector2>(ref this._targetScale));
    info.AddProperty(NEpochCard.PropertyName._time, Variant.From<float>(ref this._time));
    info.AddProperty(NEpochCard.PropertyName._noiseSpeed, Variant.From<float>(ref this._noiseSpeed));
    info.AddProperty(NEpochCard.PropertyName._noise, Variant.From<FastNoiseLite>(ref this._noise));
    info.AddProperty(NEpochCard.PropertyName._denyTween, Variant.From<Tween>(ref this._denyTween));
    info.AddProperty(NEpochCard.PropertyName._transparencyTween, Variant.From<Tween>(ref this._transparencyTween));
    info.AddProperty(NEpochCard.PropertyName._scaleTween, Variant.From<Tween>(ref this._scaleTween));
    info.AddProperty(NEpochCard.PropertyName._blueGlowColor, Variant.From<Color>(ref this._blueGlowColor));
    info.AddProperty(NEpochCard.PropertyName._goldGlowColor, Variant.From<Color>(ref this._goldGlowColor));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NEpochCard.PropertyName._glow, ref variant1))
      this._glow = ((Variant) ref variant1).As<TextureRect>();
    Variant variant2;
    if (info.TryGetProperty(NEpochCard.PropertyName._mask, ref variant2))
      this._mask = ((Variant) ref variant2).As<TextureRect>();
    Variant variant3;
    if (info.TryGetProperty(NEpochCard.PropertyName._portrait, ref variant3))
      this._portrait = ((Variant) ref variant3).As<TextureRect>();
    Variant variant4;
    if (info.TryGetProperty(NEpochCard.PropertyName._isHovered, ref variant4))
      this._isHovered = ((Variant) ref variant4).As<bool>();
    Variant variant5;
    if (info.TryGetProperty(NEpochCard.PropertyName._isHoverable, ref variant5))
      this._isHoverable = ((Variant) ref variant5).As<bool>();
    Variant variant6;
    if (info.TryGetProperty(NEpochCard.PropertyName._isHeld, ref variant6))
      this._isHeld = ((Variant) ref variant6).As<bool>();
    Variant variant7;
    if (info.TryGetProperty(NEpochCard.PropertyName._isWigglyUnlockPreviewMode, ref variant7))
      this._isWigglyUnlockPreviewMode = ((Variant) ref variant7).As<bool>();
    Variant variant8;
    if (info.TryGetProperty(NEpochCard.PropertyName._glowTween, ref variant8))
      this._glowTween = ((Variant) ref variant8).As<Tween>();
    Variant variant9;
    if (info.TryGetProperty(NEpochCard.PropertyName._targetScale, ref variant9))
      this._targetScale = ((Variant) ref variant9).As<Vector2>();
    Variant variant10;
    if (info.TryGetProperty(NEpochCard.PropertyName._time, ref variant10))
      this._time = ((Variant) ref variant10).As<float>();
    Variant variant11;
    if (info.TryGetProperty(NEpochCard.PropertyName._noiseSpeed, ref variant11))
      this._noiseSpeed = ((Variant) ref variant11).As<float>();
    Variant variant12;
    if (info.TryGetProperty(NEpochCard.PropertyName._noise, ref variant12))
      this._noise = ((Variant) ref variant12).As<FastNoiseLite>();
    Variant variant13;
    if (info.TryGetProperty(NEpochCard.PropertyName._denyTween, ref variant13))
      this._denyTween = ((Variant) ref variant13).As<Tween>();
    Variant variant14;
    if (info.TryGetProperty(NEpochCard.PropertyName._transparencyTween, ref variant14))
      this._transparencyTween = ((Variant) ref variant14).As<Tween>();
    Variant variant15;
    if (info.TryGetProperty(NEpochCard.PropertyName._scaleTween, ref variant15))
      this._scaleTween = ((Variant) ref variant15).As<Tween>();
    Variant variant16;
    if (info.TryGetProperty(NEpochCard.PropertyName._blueGlowColor, ref variant16))
      this._blueGlowColor = ((Variant) ref variant16).As<Color>();
    Variant variant17;
    if (!info.TryGetProperty(NEpochCard.PropertyName._goldGlowColor, ref variant17))
      return;
    this._goldGlowColor = ((Variant) ref variant17).As<Color>();
  }

  public class MethodName : Control.MethodName
  {
    public static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName _Process = StringName.op_Implicit(nameof (_Process));
    public static readonly StringName SetToWigglyUnlockPreviewMode = StringName.op_Implicit(nameof (SetToWigglyUnlockPreviewMode));
    public static readonly StringName GlowFlash = StringName.op_Implicit(nameof (GlowFlash));
  }

  public class PropertyName : Control.PropertyName
  {
    public static readonly StringName _glow = StringName.op_Implicit(nameof (_glow));
    public static readonly StringName _mask = StringName.op_Implicit(nameof (_mask));
    public static readonly StringName _portrait = StringName.op_Implicit(nameof (_portrait));
    public static readonly StringName _isHovered = StringName.op_Implicit(nameof (_isHovered));
    public static readonly StringName _isHoverable = StringName.op_Implicit(nameof (_isHoverable));
    public static readonly StringName _isHeld = StringName.op_Implicit(nameof (_isHeld));
    public static readonly StringName _isWigglyUnlockPreviewMode = StringName.op_Implicit(nameof (_isWigglyUnlockPreviewMode));
    public static readonly StringName _glowTween = StringName.op_Implicit(nameof (_glowTween));
    public static readonly StringName _targetScale = StringName.op_Implicit(nameof (_targetScale));
    public static readonly StringName _time = StringName.op_Implicit(nameof (_time));
    public static readonly StringName _noiseSpeed = StringName.op_Implicit(nameof (_noiseSpeed));
    public static readonly StringName _noise = StringName.op_Implicit(nameof (_noise));
    public static readonly StringName _denyTween = StringName.op_Implicit(nameof (_denyTween));
    public static readonly StringName _transparencyTween = StringName.op_Implicit(nameof (_transparencyTween));
    public static readonly StringName _scaleTween = StringName.op_Implicit(nameof (_scaleTween));
    public static readonly StringName _blueGlowColor = StringName.op_Implicit(nameof (_blueGlowColor));
    public static readonly StringName _goldGlowColor = StringName.op_Implicit(nameof (_goldGlowColor));
  }

  public class SignalName : Control.SignalName
  {
  }
}
