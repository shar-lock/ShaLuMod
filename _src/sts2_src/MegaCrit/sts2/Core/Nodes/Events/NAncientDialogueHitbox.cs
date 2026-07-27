// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Events.NAncientDialogueHitbox
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.ControllerInput;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Events;

[ScriptPath("res://src/Core/Nodes/Events/NAncientDialogueHitbox.cs")]
public class NAncientDialogueHitbox : NButton
{
  private MegaLabel _label;
  private TextureRect _arrow;
  private Tween? _loopTween;
  private Tween? _tween;
  private bool _isAnimating;

  protected override string[] Hotkeys
  {
    get
    {
      return new string[1]
      {
        StringName.op_Implicit(MegaInput.accept)
      };
    }
  }

  public string? GetHotkey() => ((IEnumerable<string>) this.Hotkeys).FirstOrDefault<string>();

  public override void _Ready()
  {
    this.ConnectSignals();
    this._label = ((Node) this).GetNode<MegaLabel>(NodePath.op_Implicit("%Label"));
    ((CanvasItem) this._label).SelfModulate = StsColors.transparentWhite;
    this._label.Text = string.Empty;
    this._arrow = ((Node) this).GetNode<TextureRect>(NodePath.op_Implicit("%Arrow"));
    ((CanvasItem) this._arrow).SelfModulate = StsColors.transparentWhite;
    this._loopTween = ((Node) this).CreateTween().SetParallel(true).SetLoops(0);
    this._loopTween.TweenProperty((GodotObject) this._arrow, NodePath.op_Implicit("position:x"), Variant.op_Implicit(((Control) this._arrow).Position.X + 4f), 0.4).SetEase((Tween.EaseType) 2L).SetTrans((Tween.TransitionType) 1L);
    this._loopTween.Chain().TweenProperty((GodotObject) this._arrow, NodePath.op_Implicit("position:x"), Variant.op_Implicit(((Control) this._arrow).Position.X - 4f), 0.6).SetEase((Tween.EaseType) 2L).SetTrans((Tween.TransitionType) 1L);
    Tween tween = ((Node) this).CreateTween().SetParallel(true);
    tween.TweenProperty((GodotObject) this._label, NodePath.op_Implicit("self_modulate:a"), Variant.op_Implicit(1f), 1.0).SetDelay(0.5);
    tween.TweenProperty((GodotObject) this._arrow, NodePath.op_Implicit("self_modulate:a"), Variant.op_Implicit(1f), 1.0).SetDelay(0.5);
  }

  protected override void OnRelease()
  {
    base.OnRelease();
    this._loopTween?.Play();
    this._tween?.Kill();
    this._tween = ((Node) this).CreateTween().SetParallel(true);
    this._tween.TweenProperty((GodotObject) this._label, NodePath.op_Implicit("scale"), Variant.op_Implicit(Vector2.One), 0.5).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 5L);
    this._tween.TweenProperty((GodotObject) this._arrow, NodePath.op_Implicit("scale"), Variant.op_Implicit(Vector2.One), 0.5).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 5L);
  }

  protected override void OnPress()
  {
    base.OnPress();
    this._loopTween?.Pause();
    ((Control) this._label).PivotOffset = Vector2.op_Multiply(((Control) this._label).Size, 0.5f);
    this._tween?.Kill();
    this._tween = ((Node) this).CreateTween().SetParallel(true);
    this._tween.TweenProperty((GodotObject) this._label, NodePath.op_Implicit("scale"), Variant.op_Implicit(Vector2.op_Multiply(Vector2.One, 0.9f)), 0.5).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 5L);
    this._tween.TweenProperty((GodotObject) this._arrow, NodePath.op_Implicit("scale"), Variant.op_Implicit(Vector2.op_Multiply(Vector2.One, 0.9f)), 0.5).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 5L);
    this._isAnimating = false;
  }

  protected override void OnFocus()
  {
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(5)
    {
      new MethodInfo(NAncientDialogueHitbox.MethodName.GetHotkey, new PropertyInfo((Variant.Type) 4L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NAncientDialogueHitbox.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NAncientDialogueHitbox.MethodName.OnRelease, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NAncientDialogueHitbox.MethodName.OnPress, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NAncientDialogueHitbox.MethodName.OnFocus, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NAncientDialogueHitbox.MethodName.GetHotkey) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      string hotkey = this.GetHotkey();
      ret = VariantUtils.CreateFrom<string>(ref hotkey);
      return true;
    }
    if (StringName.op_Equality(ref method, NAncientDialogueHitbox.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NAncientDialogueHitbox.MethodName.OnRelease) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnRelease();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NAncientDialogueHitbox.MethodName.OnPress) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnPress();
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NAncientDialogueHitbox.MethodName.OnFocus) || ((NativeVariantPtrArgs) ref args).Count != 0)
      return base.InvokeGodotClassMethod(in method, args, out ret);
    this.OnFocus();
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NAncientDialogueHitbox.MethodName.GetHotkey) || StringName.op_Equality(ref method, NAncientDialogueHitbox.MethodName._Ready) || StringName.op_Equality(ref method, NAncientDialogueHitbox.MethodName.OnRelease) || StringName.op_Equality(ref method, NAncientDialogueHitbox.MethodName.OnPress) || StringName.op_Equality(ref method, NAncientDialogueHitbox.MethodName.OnFocus) || base.HasGodotClassMethod(in method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NAncientDialogueHitbox.PropertyName._label))
    {
      this._label = VariantUtils.ConvertTo<MegaLabel>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NAncientDialogueHitbox.PropertyName._arrow))
    {
      this._arrow = VariantUtils.ConvertTo<TextureRect>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NAncientDialogueHitbox.PropertyName._loopTween))
    {
      this._loopTween = VariantUtils.ConvertTo<Tween>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NAncientDialogueHitbox.PropertyName._tween))
    {
      this._tween = VariantUtils.ConvertTo<Tween>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NAncientDialogueHitbox.PropertyName._isAnimating))
      return base.SetGodotClassPropertyValue(in name, in value);
    this._isAnimating = VariantUtils.ConvertTo<bool>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NAncientDialogueHitbox.PropertyName.Hotkeys))
    {
      ref godot_variant local = ref value;
      string[] hotkeys = this.Hotkeys;
      godot_variant from = VariantUtils.CreateFrom<string[]>(ref hotkeys);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NAncientDialogueHitbox.PropertyName._label))
    {
      value = VariantUtils.CreateFrom<MegaLabel>(ref this._label);
      return true;
    }
    if (StringName.op_Equality(ref name, NAncientDialogueHitbox.PropertyName._arrow))
    {
      value = VariantUtils.CreateFrom<TextureRect>(ref this._arrow);
      return true;
    }
    if (StringName.op_Equality(ref name, NAncientDialogueHitbox.PropertyName._loopTween))
    {
      value = VariantUtils.CreateFrom<Tween>(ref this._loopTween);
      return true;
    }
    if (StringName.op_Equality(ref name, NAncientDialogueHitbox.PropertyName._tween))
    {
      value = VariantUtils.CreateFrom<Tween>(ref this._tween);
      return true;
    }
    if (!StringName.op_Equality(ref name, NAncientDialogueHitbox.PropertyName._isAnimating))
      return base.GetGodotClassPropertyValue(in name, out value);
    value = VariantUtils.CreateFrom<bool>(ref this._isAnimating);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 34L, NAncientDialogueHitbox.PropertyName.Hotkeys, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NAncientDialogueHitbox.PropertyName._label, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NAncientDialogueHitbox.PropertyName._arrow, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NAncientDialogueHitbox.PropertyName._loopTween, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NAncientDialogueHitbox.PropertyName._tween, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 1L, NAncientDialogueHitbox.PropertyName._isAnimating, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    base.SaveGodotObjectData(info);
    info.AddProperty(NAncientDialogueHitbox.PropertyName._label, Variant.From<MegaLabel>(ref this._label));
    info.AddProperty(NAncientDialogueHitbox.PropertyName._arrow, Variant.From<TextureRect>(ref this._arrow));
    info.AddProperty(NAncientDialogueHitbox.PropertyName._loopTween, Variant.From<Tween>(ref this._loopTween));
    info.AddProperty(NAncientDialogueHitbox.PropertyName._tween, Variant.From<Tween>(ref this._tween));
    info.AddProperty(NAncientDialogueHitbox.PropertyName._isAnimating, Variant.From<bool>(ref this._isAnimating));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    base.RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NAncientDialogueHitbox.PropertyName._label, ref variant1))
      this._label = ((Variant) ref variant1).As<MegaLabel>();
    Variant variant2;
    if (info.TryGetProperty(NAncientDialogueHitbox.PropertyName._arrow, ref variant2))
      this._arrow = ((Variant) ref variant2).As<TextureRect>();
    Variant variant3;
    if (info.TryGetProperty(NAncientDialogueHitbox.PropertyName._loopTween, ref variant3))
      this._loopTween = ((Variant) ref variant3).As<Tween>();
    Variant variant4;
    if (info.TryGetProperty(NAncientDialogueHitbox.PropertyName._tween, ref variant4))
      this._tween = ((Variant) ref variant4).As<Tween>();
    Variant variant5;
    if (!info.TryGetProperty(NAncientDialogueHitbox.PropertyName._isAnimating, ref variant5))
      return;
    this._isAnimating = ((Variant) ref variant5).As<bool>();
  }

  public new class MethodName : NButton.MethodName
  {
    public static readonly StringName GetHotkey = StringName.op_Implicit(nameof (GetHotkey));
    public new static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public new static readonly StringName OnRelease = StringName.op_Implicit(nameof (OnRelease));
    public new static readonly StringName OnPress = StringName.op_Implicit(nameof (OnPress));
    public new static readonly StringName OnFocus = StringName.op_Implicit(nameof (OnFocus));
  }

  public new class PropertyName : NButton.PropertyName
  {
    public new static readonly StringName Hotkeys = StringName.op_Implicit(nameof (Hotkeys));
    public static readonly StringName _label = StringName.op_Implicit(nameof (_label));
    public static readonly StringName _arrow = StringName.op_Implicit(nameof (_arrow));
    public static readonly StringName _loopTween = StringName.op_Implicit(nameof (_loopTween));
    public static readonly StringName _tween = StringName.op_Implicit(nameof (_tween));
    public static readonly StringName _isAnimating = StringName.op_Implicit(nameof (_isAnimating));
  }

  public new class SignalName : NButton.SignalName
  {
  }
}
