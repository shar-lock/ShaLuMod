// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Combat.NStarCounter
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Nodes.HoverTips;
using System;
using System.Collections.Generic;
using System.ComponentModel;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Combat;

[ScriptPath("res://src/Core/Nodes/Combat/NStarCounter.cs")]
public class NStarCounter : Control
{
  private static readonly StringName _v = new StringName("v");
  private static readonly StringName _s = new StringName("s");
  private static readonly string _starGainVfxPath = SceneHelper.GetScenePath("vfx/star_gain_vfx");
  private Player? _player;
  private MegaRichTextLabel _label;
  private Control _rotationLayers;
  private Control _icon;
  private ShaderMaterial _hsv;
  private float _lerpingStarCount;
  private float _velocity;
  private int _displayedStarCount;
  private Tween? _hsvTween;
  private bool _isListeningToCombatState;
  private HoverTip _hoverTip;

  public static IEnumerable<string> AssetPaths
  {
    get
    {
      return (IEnumerable<string>) new \u003C\u003Ez__ReadOnlySingleElementList<string>(NStarCounter._starGainVfxPath);
    }
  }

  public override void _Ready()
  {
    this._label = ((Node) this).GetNode<MegaRichTextLabel>(NodePath.op_Implicit("%CountLabel"));
    this._rotationLayers = ((Node) this).GetNode<Control>(NodePath.op_Implicit("%RotationLayers"));
    this._icon = ((Node) this).GetNode<Control>(NodePath.op_Implicit("Icon"));
    this._hsv = (ShaderMaterial) ((CanvasItem) this._icon).Material;
    LocString description = new LocString("static_hover_tips", "STAR_COUNT.description");
    description.Add("singleStarIcon", "[img]res://images/packed/sprite_fonts/star_icon.png[/img]");
    this._hoverTip = new HoverTip(new LocString("static_hover_tips", "STAR_COUNT.title"), description);
    ((GodotObject) this).Connect(Control.SignalName.MouseEntered, Callable.From(new Action(this.OnHovered)), 0U);
    ((GodotObject) this).Connect(Control.SignalName.MouseExited, Callable.From(new Action(this.OnUnhovered)), 0U);
    ((CanvasItem) this).Visible = false;
  }

  public override void _EnterTree()
  {
    ((Node) this)._EnterTree();
    this.ConnectStarsChangedSignal();
  }

  public override void _ExitTree()
  {
    ((Node) this)._ExitTree();
    if (this._player == null || !this._isListeningToCombatState)
      return;
    this._player.PlayerCombatState.StarsChanged -= new Action<int, int>(this.OnStarsChanged);
    this._isListeningToCombatState = false;
  }

  private void ConnectStarsChangedSignal()
  {
    if (this._player == null || this._isListeningToCombatState)
      return;
    this._player.PlayerCombatState.StarsChanged += new Action<int, int>(this.OnStarsChanged);
    this._isListeningToCombatState = true;
  }

  public void Initialize(Player player)
  {
    this._player = player;
    this.ConnectStarsChangedSignal();
    this.RefreshVisibility();
  }

  private void OnHovered()
  {
    NHoverTipSet.CreateAndShow((Control) this, (IHoverTip) this._hoverTip)?.SetGlobalPosition(Vector2.op_Addition(this.GlobalPosition, new Vector2(-34f, -300f)), false);
  }

  private void OnUnhovered() => NHoverTipSet.Remove((Control) this);

  private void OnStarsChanged(int oldStars, int newStars)
  {
    this.UpdateStarCount(oldStars, newStars);
    this.RefreshVisibility();
  }

  public override void _Process(double delta)
  {
    if (this._player == null)
      return;
    float num = this._player.PlayerCombatState.Stars == 0 ? 5f : 30f;
    for (int index = 0; index < ((Node) this._rotationLayers).GetChildCount(false); ++index)
      ((Node) this._rotationLayers).GetChild<Control>(index, false).RotationDegrees += (float) delta * num * (float) (index + 1);
    this._lerpingStarCount = MathHelper.SmoothDamp(this._lerpingStarCount, (float) this._player.PlayerCombatState.Stars, ref this._velocity, 0.1f, (float) delta);
    this.SetStarCountText(Mathf.RoundToInt(this._lerpingStarCount));
  }

  private void UpdateStarCount(int oldCount, int newCount)
  {
    if (newCount < oldCount)
    {
      this._hsvTween?.Kill();
      this._hsv.SetShaderParameter(NStarCounter._v, Variant.op_Implicit(1f));
      this._lerpingStarCount = (float) newCount;
      this.SetStarCountText(newCount);
    }
    else
    {
      if (newCount <= oldCount)
        return;
      this._hsvTween?.Kill();
      this._hsvTween = ((Node) this).CreateTween();
      this._hsvTween.TweenMethod(Callable.From<float>(new Action<float>(this.UpdateShaderV)), Variant.op_Implicit(2f), Variant.op_Implicit(1f), 0.20000000298023224);
      Node2D child = PreloadManager.Cache.GetAsset<PackedScene>(NStarCounter._starGainVfxPath).Instantiate<Node2D>((PackedScene.GenEditState) 0L);
      ((Node) this).AddChildSafely((Node) child);
      ((Node) this).MoveChildSafely((Node) child, 0);
      child.Position = Vector2.op_Division(this.Size, 2f);
    }
  }

  private void SetStarCountText(int stars)
  {
    if (this._displayedStarCount == stars)
      return;
    this._displayedStarCount = stars;
    ((Control) this._label).AddThemeColorOverride(ThemeConstants.Label.FontColor, stars == 0 ? StsColors.red : StsColors.cream);
    this._label.Text = $"[center]{stars}[/center]";
    if (stars == 0)
    {
      this._hsv.SetShaderParameter(NStarCounter._s, Variant.op_Implicit(0.5f));
      this._hsv.SetShaderParameter(NStarCounter._v, Variant.op_Implicit(0.85f));
    }
    else
    {
      this._hsv.SetShaderParameter(NStarCounter._s, Variant.op_Implicit(1f));
      this._hsv.SetShaderParameter(NStarCounter._v, Variant.op_Implicit(1f));
    }
  }

  private void UpdateShaderV(float value)
  {
    this._hsv.SetShaderParameter(NStarCounter._v, Variant.op_Implicit(value));
  }

  private void RefreshVisibility()
  {
    if (this._player == null)
    {
      ((CanvasItem) this).Visible = false;
    }
    else
    {
      int stars = this._player.PlayerCombatState.Stars;
      ((CanvasItem) this).Visible = ((CanvasItem) this).Visible || this._player.Character.ShouldAlwaysShowStarCounter || stars > 0;
    }
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(12)
    {
      new MethodInfo(NStarCounter.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NStarCounter.MethodName._EnterTree, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NStarCounter.MethodName._ExitTree, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NStarCounter.MethodName.ConnectStarsChangedSignal, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NStarCounter.MethodName.OnHovered, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NStarCounter.MethodName.OnUnhovered, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NStarCounter.MethodName.OnStarsChanged, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 2L, StringName.op_Implicit("oldStars"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false),
        new PropertyInfo((Variant.Type) 2L, StringName.op_Implicit("newStars"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NStarCounter.MethodName._Process, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 3L, StringName.op_Implicit("delta"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NStarCounter.MethodName.UpdateStarCount, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 2L, StringName.op_Implicit("oldCount"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false),
        new PropertyInfo((Variant.Type) 2L, StringName.op_Implicit("newCount"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NStarCounter.MethodName.SetStarCountText, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 2L, StringName.op_Implicit("stars"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NStarCounter.MethodName.UpdateShaderV, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 3L, StringName.op_Implicit("value"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NStarCounter.MethodName.RefreshVisibility, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NStarCounter.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NStarCounter.MethodName._EnterTree) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._EnterTree();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NStarCounter.MethodName._ExitTree) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._ExitTree();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NStarCounter.MethodName.ConnectStarsChangedSignal) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.ConnectStarsChangedSignal();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NStarCounter.MethodName.OnHovered) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnHovered();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NStarCounter.MethodName.OnUnhovered) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnUnhovered();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NStarCounter.MethodName.OnStarsChanged) && ((NativeVariantPtrArgs) ref args).Count == 2)
    {
      this.OnStarsChanged(VariantUtils.ConvertTo<int>(ref ((NativeVariantPtrArgs) ref args)[0]), VariantUtils.ConvertTo<int>(ref ((NativeVariantPtrArgs) ref args)[1]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NStarCounter.MethodName._Process) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      ((Node) this)._Process(VariantUtils.ConvertTo<double>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NStarCounter.MethodName.UpdateStarCount) && ((NativeVariantPtrArgs) ref args).Count == 2)
    {
      this.UpdateStarCount(VariantUtils.ConvertTo<int>(ref ((NativeVariantPtrArgs) ref args)[0]), VariantUtils.ConvertTo<int>(ref ((NativeVariantPtrArgs) ref args)[1]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NStarCounter.MethodName.SetStarCountText) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.SetStarCountText(VariantUtils.ConvertTo<int>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NStarCounter.MethodName.UpdateShaderV) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.UpdateShaderV(VariantUtils.ConvertTo<float>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NStarCounter.MethodName.RefreshVisibility) || ((NativeVariantPtrArgs) ref args).Count != 0)
      return base.InvokeGodotClassMethod(ref method, args, ref ret);
    this.RefreshVisibility();
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NStarCounter.MethodName._Ready) || StringName.op_Equality(ref method, NStarCounter.MethodName._EnterTree) || StringName.op_Equality(ref method, NStarCounter.MethodName._ExitTree) || StringName.op_Equality(ref method, NStarCounter.MethodName.ConnectStarsChangedSignal) || StringName.op_Equality(ref method, NStarCounter.MethodName.OnHovered) || StringName.op_Equality(ref method, NStarCounter.MethodName.OnUnhovered) || StringName.op_Equality(ref method, NStarCounter.MethodName.OnStarsChanged) || StringName.op_Equality(ref method, NStarCounter.MethodName._Process) || StringName.op_Equality(ref method, NStarCounter.MethodName.UpdateStarCount) || StringName.op_Equality(ref method, NStarCounter.MethodName.SetStarCountText) || StringName.op_Equality(ref method, NStarCounter.MethodName.UpdateShaderV) || StringName.op_Equality(ref method, NStarCounter.MethodName.RefreshVisibility) || base.HasGodotClassMethod(ref method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NStarCounter.PropertyName._label))
    {
      this._label = VariantUtils.ConvertTo<MegaRichTextLabel>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NStarCounter.PropertyName._rotationLayers))
    {
      this._rotationLayers = VariantUtils.ConvertTo<Control>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NStarCounter.PropertyName._icon))
    {
      this._icon = VariantUtils.ConvertTo<Control>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NStarCounter.PropertyName._hsv))
    {
      this._hsv = VariantUtils.ConvertTo<ShaderMaterial>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NStarCounter.PropertyName._lerpingStarCount))
    {
      this._lerpingStarCount = VariantUtils.ConvertTo<float>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NStarCounter.PropertyName._velocity))
    {
      this._velocity = VariantUtils.ConvertTo<float>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NStarCounter.PropertyName._displayedStarCount))
    {
      this._displayedStarCount = VariantUtils.ConvertTo<int>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NStarCounter.PropertyName._hsvTween))
    {
      this._hsvTween = VariantUtils.ConvertTo<Tween>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NStarCounter.PropertyName._isListeningToCombatState))
      return ((GodotObject) this).SetGodotClassPropertyValue(ref name, ref value);
    this._isListeningToCombatState = VariantUtils.ConvertTo<bool>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NStarCounter.PropertyName._label))
    {
      value = VariantUtils.CreateFrom<MegaRichTextLabel>(ref this._label);
      return true;
    }
    if (StringName.op_Equality(ref name, NStarCounter.PropertyName._rotationLayers))
    {
      value = VariantUtils.CreateFrom<Control>(ref this._rotationLayers);
      return true;
    }
    if (StringName.op_Equality(ref name, NStarCounter.PropertyName._icon))
    {
      value = VariantUtils.CreateFrom<Control>(ref this._icon);
      return true;
    }
    if (StringName.op_Equality(ref name, NStarCounter.PropertyName._hsv))
    {
      value = VariantUtils.CreateFrom<ShaderMaterial>(ref this._hsv);
      return true;
    }
    if (StringName.op_Equality(ref name, NStarCounter.PropertyName._lerpingStarCount))
    {
      value = VariantUtils.CreateFrom<float>(ref this._lerpingStarCount);
      return true;
    }
    if (StringName.op_Equality(ref name, NStarCounter.PropertyName._velocity))
    {
      value = VariantUtils.CreateFrom<float>(ref this._velocity);
      return true;
    }
    if (StringName.op_Equality(ref name, NStarCounter.PropertyName._displayedStarCount))
    {
      value = VariantUtils.CreateFrom<int>(ref this._displayedStarCount);
      return true;
    }
    if (StringName.op_Equality(ref name, NStarCounter.PropertyName._hsvTween))
    {
      value = VariantUtils.CreateFrom<Tween>(ref this._hsvTween);
      return true;
    }
    if (!StringName.op_Equality(ref name, NStarCounter.PropertyName._isListeningToCombatState))
      return ((GodotObject) this).GetGodotClassPropertyValue(ref name, ref value);
    value = VariantUtils.CreateFrom<bool>(ref this._isListeningToCombatState);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NStarCounter.PropertyName._label, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NStarCounter.PropertyName._rotationLayers, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NStarCounter.PropertyName._icon, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NStarCounter.PropertyName._hsv, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 3L, NStarCounter.PropertyName._lerpingStarCount, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 3L, NStarCounter.PropertyName._velocity, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 2L, NStarCounter.PropertyName._displayedStarCount, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NStarCounter.PropertyName._hsvTween, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 1L, NStarCounter.PropertyName._isListeningToCombatState, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).SaveGodotObjectData(info);
    info.AddProperty(NStarCounter.PropertyName._label, Variant.From<MegaRichTextLabel>(ref this._label));
    info.AddProperty(NStarCounter.PropertyName._rotationLayers, Variant.From<Control>(ref this._rotationLayers));
    info.AddProperty(NStarCounter.PropertyName._icon, Variant.From<Control>(ref this._icon));
    info.AddProperty(NStarCounter.PropertyName._hsv, Variant.From<ShaderMaterial>(ref this._hsv));
    info.AddProperty(NStarCounter.PropertyName._lerpingStarCount, Variant.From<float>(ref this._lerpingStarCount));
    info.AddProperty(NStarCounter.PropertyName._velocity, Variant.From<float>(ref this._velocity));
    info.AddProperty(NStarCounter.PropertyName._displayedStarCount, Variant.From<int>(ref this._displayedStarCount));
    info.AddProperty(NStarCounter.PropertyName._hsvTween, Variant.From<Tween>(ref this._hsvTween));
    info.AddProperty(NStarCounter.PropertyName._isListeningToCombatState, Variant.From<bool>(ref this._isListeningToCombatState));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NStarCounter.PropertyName._label, ref variant1))
      this._label = ((Variant) ref variant1).As<MegaRichTextLabel>();
    Variant variant2;
    if (info.TryGetProperty(NStarCounter.PropertyName._rotationLayers, ref variant2))
      this._rotationLayers = ((Variant) ref variant2).As<Control>();
    Variant variant3;
    if (info.TryGetProperty(NStarCounter.PropertyName._icon, ref variant3))
      this._icon = ((Variant) ref variant3).As<Control>();
    Variant variant4;
    if (info.TryGetProperty(NStarCounter.PropertyName._hsv, ref variant4))
      this._hsv = ((Variant) ref variant4).As<ShaderMaterial>();
    Variant variant5;
    if (info.TryGetProperty(NStarCounter.PropertyName._lerpingStarCount, ref variant5))
      this._lerpingStarCount = ((Variant) ref variant5).As<float>();
    Variant variant6;
    if (info.TryGetProperty(NStarCounter.PropertyName._velocity, ref variant6))
      this._velocity = ((Variant) ref variant6).As<float>();
    Variant variant7;
    if (info.TryGetProperty(NStarCounter.PropertyName._displayedStarCount, ref variant7))
      this._displayedStarCount = ((Variant) ref variant7).As<int>();
    Variant variant8;
    if (info.TryGetProperty(NStarCounter.PropertyName._hsvTween, ref variant8))
      this._hsvTween = ((Variant) ref variant8).As<Tween>();
    Variant variant9;
    if (!info.TryGetProperty(NStarCounter.PropertyName._isListeningToCombatState, ref variant9))
      return;
    this._isListeningToCombatState = ((Variant) ref variant9).As<bool>();
  }

  public class MethodName : Control.MethodName
  {
    public static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName _EnterTree = StringName.op_Implicit(nameof (_EnterTree));
    public static readonly StringName _ExitTree = StringName.op_Implicit(nameof (_ExitTree));
    public static readonly StringName ConnectStarsChangedSignal = StringName.op_Implicit(nameof (ConnectStarsChangedSignal));
    public static readonly StringName OnHovered = StringName.op_Implicit(nameof (OnHovered));
    public static readonly StringName OnUnhovered = StringName.op_Implicit(nameof (OnUnhovered));
    public static readonly StringName OnStarsChanged = StringName.op_Implicit(nameof (OnStarsChanged));
    public static readonly StringName _Process = StringName.op_Implicit(nameof (_Process));
    public static readonly StringName UpdateStarCount = StringName.op_Implicit(nameof (UpdateStarCount));
    public static readonly StringName SetStarCountText = StringName.op_Implicit(nameof (SetStarCountText));
    public static readonly StringName UpdateShaderV = StringName.op_Implicit(nameof (UpdateShaderV));
    public static readonly StringName RefreshVisibility = StringName.op_Implicit(nameof (RefreshVisibility));
  }

  public class PropertyName : Control.PropertyName
  {
    public static readonly StringName _label = StringName.op_Implicit(nameof (_label));
    public static readonly StringName _rotationLayers = StringName.op_Implicit(nameof (_rotationLayers));
    public static readonly StringName _icon = StringName.op_Implicit(nameof (_icon));
    public static readonly StringName _hsv = StringName.op_Implicit(nameof (_hsv));
    public static readonly StringName _lerpingStarCount = StringName.op_Implicit(nameof (_lerpingStarCount));
    public static readonly StringName _velocity = StringName.op_Implicit(nameof (_velocity));
    public static readonly StringName _displayedStarCount = StringName.op_Implicit(nameof (_displayedStarCount));
    public static readonly StringName _hsvTween = StringName.op_Implicit(nameof (_hsvTween));
    public static readonly StringName _isListeningToCombatState = StringName.op_Implicit(nameof (_isListeningToCombatState));
  }

  public class SignalName : Control.SignalName
  {
  }
}
