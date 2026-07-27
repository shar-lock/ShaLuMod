// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Screens.Bestiary.NBestiaryMoveButton
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using System.Collections.Generic;
using System.ComponentModel;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Screens.Bestiary;

[ScriptPath("res://src/Core/Nodes/Screens/Bestiary/NBestiaryMoveButton.cs")]
public class NBestiaryMoveButton : NButton
{
  private static readonly string _scenePath = SceneHelper.GetScenePath("screens/bestiary/bestiary_move_button");
  private Control _buttonAnimator;
  private MegaLabel _label;
  private Tween? _tween;
  private Tween? _clickTween;
  private string[] _hotkeys;

  protected override string? ClickedSfx => (string) null;

  protected override string? HoveredSfx => (string) null;

  public BestiaryMonsterMove Move { get; private set; }

  protected override string[] Hotkeys => this._hotkeys;

  public override void _Ready()
  {
    this.ConnectSignals();
    this._label = ((Node) this).GetNode<MegaLabel>(NodePath.op_Implicit("%Label"));
    this._buttonAnimator = ((Node) this).GetNode<Control>(NodePath.op_Implicit("%ButtonAnimator"));
    this._label.SetTextAutoSize(this.Move.displayName);
    ((Control) this._label).PivotOffset = new Vector2(0.0f, ((Control) this._label).Size.Y * 0.5f);
    this._buttonAnimator.PivotOffset = new Vector2(0.0f, this._buttonAnimator.Size.Y * 0.5f);
  }

  public static NBestiaryMoveButton Create(BestiaryMonsterMove move, StringName setHotkey)
  {
    NBestiaryMoveButton nbestiaryMoveButton = PreloadManager.Cache.GetAsset<PackedScene>(NBestiaryMoveButton._scenePath).Instantiate<NBestiaryMoveButton>((PackedScene.GenEditState) 0L);
    nbestiaryMoveButton._hotkeys = new string[1]
    {
      StringName.op_Implicit(setHotkey)
    };
    nbestiaryMoveButton.Move = move;
    return nbestiaryMoveButton;
  }

  protected override void OnRelease()
  {
    base.OnRelease();
    ((CanvasItem) this._label).Modulate = StsColors.green;
    this._clickTween?.Kill();
    this._clickTween = ((Node) this).CreateTween().SetParallel(true);
    this._clickTween.TweenProperty((GodotObject) this._buttonAnimator, NodePath.op_Implicit("scale"), Variant.op_Implicit(Vector2.One), 0.3).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 9L);
    this._clickTween.TweenProperty((GodotObject) this._label, NodePath.op_Implicit("modulate"), Variant.op_Implicit(StsColors.cream), 0.2);
  }

  protected override void OnPress()
  {
    base.OnPress();
    this._clickTween?.Kill();
    this._clickTween = ((Node) this).CreateTween().SetParallel(true);
    this._clickTween.TweenProperty((GodotObject) this._buttonAnimator, NodePath.op_Implicit("scale"), Variant.op_Implicit(Vector2.op_Multiply(Vector2.One, 0.9f)), 0.2).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 5L);
    this._clickTween.TweenProperty((GodotObject) this._label, NodePath.op_Implicit("modulate"), Variant.op_Implicit(StsColors.lightGray), 0.05);
  }

  protected override void OnFocus()
  {
    base.OnFocus();
    this._tween?.Kill();
    this._tween = ((Node) this).CreateTween().SetParallel(true);
    this._tween.TweenProperty((GodotObject) this._label, NodePath.op_Implicit("scale"), Variant.op_Implicit(Vector2.op_Multiply(Vector2.One, 1.05f)), 0.05);
  }

  protected override void OnUnfocus()
  {
    this._tween?.Kill();
    this._tween = ((Node) this).CreateTween().SetParallel(true);
    this._tween.TweenProperty((GodotObject) this._label, NodePath.op_Implicit("scale"), Variant.op_Implicit(Vector2.One), 0.25).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 7L);
    this._clickTween?.Kill();
    this._clickTween = ((Node) this).CreateTween().SetParallel(true);
    this._clickTween.TweenProperty((GodotObject) this._buttonAnimator, NodePath.op_Implicit("scale"), Variant.op_Implicit(Vector2.One), 0.1).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 5L);
    this._clickTween.TweenProperty((GodotObject) this._label, NodePath.op_Implicit("modulate"), Variant.op_Implicit(StsColors.cream), 0.1);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(5)
    {
      new MethodInfo(NBestiaryMoveButton.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NBestiaryMoveButton.MethodName.OnRelease, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NBestiaryMoveButton.MethodName.OnPress, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NBestiaryMoveButton.MethodName.OnFocus, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NBestiaryMoveButton.MethodName.OnUnfocus, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NBestiaryMoveButton.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NBestiaryMoveButton.MethodName.OnRelease) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnRelease();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NBestiaryMoveButton.MethodName.OnPress) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnPress();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NBestiaryMoveButton.MethodName.OnFocus) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnFocus();
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NBestiaryMoveButton.MethodName.OnUnfocus) || ((NativeVariantPtrArgs) ref args).Count != 0)
      return base.InvokeGodotClassMethod(in method, args, out ret);
    this.OnUnfocus();
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NBestiaryMoveButton.MethodName._Ready) || StringName.op_Equality(ref method, NBestiaryMoveButton.MethodName.OnRelease) || StringName.op_Equality(ref method, NBestiaryMoveButton.MethodName.OnPress) || StringName.op_Equality(ref method, NBestiaryMoveButton.MethodName.OnFocus) || StringName.op_Equality(ref method, NBestiaryMoveButton.MethodName.OnUnfocus) || base.HasGodotClassMethod(in method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NBestiaryMoveButton.PropertyName._buttonAnimator))
    {
      this._buttonAnimator = VariantUtils.ConvertTo<Control>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NBestiaryMoveButton.PropertyName._label))
    {
      this._label = VariantUtils.ConvertTo<MegaLabel>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NBestiaryMoveButton.PropertyName._tween))
    {
      this._tween = VariantUtils.ConvertTo<Tween>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NBestiaryMoveButton.PropertyName._clickTween))
    {
      this._clickTween = VariantUtils.ConvertTo<Tween>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NBestiaryMoveButton.PropertyName._hotkeys))
      return base.SetGodotClassPropertyValue(in name, in value);
    this._hotkeys = VariantUtils.ConvertTo<string[]>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NBestiaryMoveButton.PropertyName.ClickedSfx))
    {
      ref godot_variant local = ref value;
      string clickedSfx = this.ClickedSfx;
      godot_variant from = VariantUtils.CreateFrom<string>(ref clickedSfx);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NBestiaryMoveButton.PropertyName.HoveredSfx))
    {
      ref godot_variant local = ref value;
      string hoveredSfx = this.HoveredSfx;
      godot_variant from = VariantUtils.CreateFrom<string>(ref hoveredSfx);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NBestiaryMoveButton.PropertyName.Hotkeys))
    {
      ref godot_variant local = ref value;
      string[] hotkeys = this.Hotkeys;
      godot_variant from = VariantUtils.CreateFrom<string[]>(ref hotkeys);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NBestiaryMoveButton.PropertyName._buttonAnimator))
    {
      value = VariantUtils.CreateFrom<Control>(ref this._buttonAnimator);
      return true;
    }
    if (StringName.op_Equality(ref name, NBestiaryMoveButton.PropertyName._label))
    {
      value = VariantUtils.CreateFrom<MegaLabel>(ref this._label);
      return true;
    }
    if (StringName.op_Equality(ref name, NBestiaryMoveButton.PropertyName._tween))
    {
      value = VariantUtils.CreateFrom<Tween>(ref this._tween);
      return true;
    }
    if (StringName.op_Equality(ref name, NBestiaryMoveButton.PropertyName._clickTween))
    {
      value = VariantUtils.CreateFrom<Tween>(ref this._clickTween);
      return true;
    }
    if (!StringName.op_Equality(ref name, NBestiaryMoveButton.PropertyName._hotkeys))
      return base.GetGodotClassPropertyValue(in name, out value);
    value = VariantUtils.CreateFrom<string[]>(ref this._hotkeys);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 4L, NBestiaryMoveButton.PropertyName.ClickedSfx, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 4L, NBestiaryMoveButton.PropertyName.HoveredSfx, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NBestiaryMoveButton.PropertyName._buttonAnimator, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NBestiaryMoveButton.PropertyName._label, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NBestiaryMoveButton.PropertyName._tween, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NBestiaryMoveButton.PropertyName._clickTween, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 34L, NBestiaryMoveButton.PropertyName._hotkeys, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 34L, NBestiaryMoveButton.PropertyName.Hotkeys, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    base.SaveGodotObjectData(info);
    info.AddProperty(NBestiaryMoveButton.PropertyName._buttonAnimator, Variant.From<Control>(ref this._buttonAnimator));
    info.AddProperty(NBestiaryMoveButton.PropertyName._label, Variant.From<MegaLabel>(ref this._label));
    info.AddProperty(NBestiaryMoveButton.PropertyName._tween, Variant.From<Tween>(ref this._tween));
    info.AddProperty(NBestiaryMoveButton.PropertyName._clickTween, Variant.From<Tween>(ref this._clickTween));
    info.AddProperty(NBestiaryMoveButton.PropertyName._hotkeys, Variant.From<string[]>(ref this._hotkeys));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    base.RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NBestiaryMoveButton.PropertyName._buttonAnimator, ref variant1))
      this._buttonAnimator = ((Variant) ref variant1).As<Control>();
    Variant variant2;
    if (info.TryGetProperty(NBestiaryMoveButton.PropertyName._label, ref variant2))
      this._label = ((Variant) ref variant2).As<MegaLabel>();
    Variant variant3;
    if (info.TryGetProperty(NBestiaryMoveButton.PropertyName._tween, ref variant3))
      this._tween = ((Variant) ref variant3).As<Tween>();
    Variant variant4;
    if (info.TryGetProperty(NBestiaryMoveButton.PropertyName._clickTween, ref variant4))
      this._clickTween = ((Variant) ref variant4).As<Tween>();
    Variant variant5;
    if (!info.TryGetProperty(NBestiaryMoveButton.PropertyName._hotkeys, ref variant5))
      return;
    this._hotkeys = ((Variant) ref variant5).As<string[]>();
  }

  public new class MethodName : NButton.MethodName
  {
    public new static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public new static readonly StringName OnRelease = StringName.op_Implicit(nameof (OnRelease));
    public new static readonly StringName OnPress = StringName.op_Implicit(nameof (OnPress));
    public new static readonly StringName OnFocus = StringName.op_Implicit(nameof (OnFocus));
    public new static readonly StringName OnUnfocus = StringName.op_Implicit(nameof (OnUnfocus));
  }

  public new class PropertyName : NButton.PropertyName
  {
    public new static readonly StringName ClickedSfx = StringName.op_Implicit(nameof (ClickedSfx));
    public new static readonly StringName HoveredSfx = StringName.op_Implicit(nameof (HoveredSfx));
    public new static readonly StringName Hotkeys = StringName.op_Implicit(nameof (Hotkeys));
    public static readonly StringName _buttonAnimator = StringName.op_Implicit(nameof (_buttonAnimator));
    public static readonly StringName _label = StringName.op_Implicit(nameof (_label));
    public static readonly StringName _tween = StringName.op_Implicit(nameof (_tween));
    public static readonly StringName _clickTween = StringName.op_Implicit(nameof (_clickTween));
    public static readonly StringName _hotkeys = StringName.op_Implicit(nameof (_hotkeys));
  }

  public new class SignalName : NButton.SignalName
  {
  }
}
