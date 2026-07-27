// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Screens.DailyRun.NGlobalRankTickbox
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.ControllerInput;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using System.Collections.Generic;
using System.ComponentModel;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Screens.DailyRun;

[ScriptPath("res://src/Core/Nodes/Screens/DailyRun/NGlobalRankTickbox.cs")]
public class NGlobalRankTickbox : NTickbox
{
  private MegaLabel _label;
  private Tween? _labelTween;

  protected override string[] Hotkeys
  {
    get
    {
      return new string[1]
      {
        StringName.op_Implicit(MegaInput.peek)
      };
    }
  }

  public override void _Ready()
  {
    this.ConnectSignals();
    this._label = ((Node) this).GetNode<MegaLabel>(NodePath.op_Implicit("Label"));
    this.IsTicked = false;
  }

  protected override void OnRelease()
  {
    base.OnRelease();
    this._labelTween?.Kill();
    this._labelTween = ((Node) this).CreateTween().SetParallel(true);
    this._labelTween.TweenProperty((GodotObject) this._label, NodePath.op_Implicit("scale"), Variant.op_Implicit(Vector2.op_Multiply(Vector2.One, 1.1f)), 0.25).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 9L);
    this._labelTween.TweenProperty((GodotObject) this._label, NodePath.op_Implicit("self_modulate"), Variant.op_Implicit(Colors.White), 0.05);
  }

  protected override void OnFocus()
  {
    base.OnFocus();
    this._labelTween?.Kill();
    this._labelTween = ((Node) this).CreateTween().SetParallel(true);
    this._labelTween.TweenProperty((GodotObject) this._label, NodePath.op_Implicit("scale"), Variant.op_Implicit(Vector2.op_Multiply(Vector2.One, 1.1f)), 0.05);
  }

  protected override void OnUnfocus()
  {
    base.OnUnfocus();
    this._labelTween?.Kill();
    this._labelTween = ((Node) this).CreateTween().SetParallel(true);
    this._labelTween.TweenProperty((GodotObject) this._label, NodePath.op_Implicit("scale"), Variant.op_Implicit(Vector2.One), 0.5).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 5L);
  }

  protected override void OnPress()
  {
    base.OnPress();
    this._labelTween?.Kill();
    this._labelTween = ((Node) this).CreateTween().SetParallel(true);
    this._labelTween.TweenProperty((GodotObject) this._label, NodePath.op_Implicit("scale"), Variant.op_Implicit(Vector2.One), 0.5).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 5L);
    this._labelTween.TweenProperty((GodotObject) this._label, NodePath.op_Implicit("self_modulate"), Variant.op_Implicit(StsColors.lightGray), 0.5).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 5L);
  }

  public void SetLabel(string text) => this._label.SetTextAutoSize(text);

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(6)
    {
      new MethodInfo(NGlobalRankTickbox.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NGlobalRankTickbox.MethodName.OnRelease, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NGlobalRankTickbox.MethodName.OnFocus, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NGlobalRankTickbox.MethodName.OnUnfocus, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NGlobalRankTickbox.MethodName.OnPress, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NGlobalRankTickbox.MethodName.SetLabel, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 4L, StringName.op_Implicit("text"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NGlobalRankTickbox.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NGlobalRankTickbox.MethodName.OnRelease) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnRelease();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NGlobalRankTickbox.MethodName.OnFocus) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnFocus();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NGlobalRankTickbox.MethodName.OnUnfocus) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnUnfocus();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NGlobalRankTickbox.MethodName.OnPress) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnPress();
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NGlobalRankTickbox.MethodName.SetLabel) || ((NativeVariantPtrArgs) ref args).Count != 1)
      return base.InvokeGodotClassMethod(in method, args, out ret);
    this.SetLabel(VariantUtils.ConvertTo<string>(ref ((NativeVariantPtrArgs) ref args)[0]));
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NGlobalRankTickbox.MethodName._Ready) || StringName.op_Equality(ref method, NGlobalRankTickbox.MethodName.OnRelease) || StringName.op_Equality(ref method, NGlobalRankTickbox.MethodName.OnFocus) || StringName.op_Equality(ref method, NGlobalRankTickbox.MethodName.OnUnfocus) || StringName.op_Equality(ref method, NGlobalRankTickbox.MethodName.OnPress) || StringName.op_Equality(ref method, NGlobalRankTickbox.MethodName.SetLabel) || base.HasGodotClassMethod(in method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NGlobalRankTickbox.PropertyName._label))
    {
      this._label = VariantUtils.ConvertTo<MegaLabel>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NGlobalRankTickbox.PropertyName._labelTween))
      return base.SetGodotClassPropertyValue(in name, in value);
    this._labelTween = VariantUtils.ConvertTo<Tween>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NGlobalRankTickbox.PropertyName.Hotkeys))
    {
      ref godot_variant local = ref value;
      string[] hotkeys = this.Hotkeys;
      godot_variant from = VariantUtils.CreateFrom<string[]>(ref hotkeys);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NGlobalRankTickbox.PropertyName._label))
    {
      value = VariantUtils.CreateFrom<MegaLabel>(ref this._label);
      return true;
    }
    if (!StringName.op_Equality(ref name, NGlobalRankTickbox.PropertyName._labelTween))
      return base.GetGodotClassPropertyValue(in name, out value);
    value = VariantUtils.CreateFrom<Tween>(ref this._labelTween);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NGlobalRankTickbox.PropertyName._label, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NGlobalRankTickbox.PropertyName._labelTween, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 34L, NGlobalRankTickbox.PropertyName.Hotkeys, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    base.SaveGodotObjectData(info);
    info.AddProperty(NGlobalRankTickbox.PropertyName._label, Variant.From<MegaLabel>(ref this._label));
    info.AddProperty(NGlobalRankTickbox.PropertyName._labelTween, Variant.From<Tween>(ref this._labelTween));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    base.RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NGlobalRankTickbox.PropertyName._label, ref variant1))
      this._label = ((Variant) ref variant1).As<MegaLabel>();
    Variant variant2;
    if (!info.TryGetProperty(NGlobalRankTickbox.PropertyName._labelTween, ref variant2))
      return;
    this._labelTween = ((Variant) ref variant2).As<Tween>();
  }

  public new class MethodName : NTickbox.MethodName
  {
    public new static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public new static readonly StringName OnRelease = StringName.op_Implicit(nameof (OnRelease));
    public new static readonly StringName OnFocus = StringName.op_Implicit(nameof (OnFocus));
    public new static readonly StringName OnUnfocus = StringName.op_Implicit(nameof (OnUnfocus));
    public new static readonly StringName OnPress = StringName.op_Implicit(nameof (OnPress));
    public static readonly StringName SetLabel = StringName.op_Implicit(nameof (SetLabel));
  }

  public new class PropertyName : NTickbox.PropertyName
  {
    public new static readonly StringName Hotkeys = StringName.op_Implicit(nameof (Hotkeys));
    public static readonly StringName _label = StringName.op_Implicit(nameof (_label));
    public static readonly StringName _labelTween = StringName.op_Implicit(nameof (_labelTween));
  }

  public new class SignalName : NTickbox.SignalName
  {
  }
}
