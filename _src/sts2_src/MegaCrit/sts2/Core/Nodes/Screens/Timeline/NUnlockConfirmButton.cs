// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Screens.Timeline.NUnlockConfirmButton
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.ControllerInput;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using System.Collections.Generic;
using System.ComponentModel;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Screens.Timeline;

[ScriptPath("res://src/Core/Nodes/Screens/Timeline/NUnlockConfirmButton.cs")]
public class NUnlockConfirmButton : NButton
{
  private Tween? _tween;
  private Tween? _hoverTween;

  protected override string[] Hotkeys
  {
    get
    {
      return new string[2]
      {
        StringName.op_Implicit(MegaInput.select),
        StringName.op_Implicit(MegaInput.pauseAndBack)
      };
    }
  }

  public override void _Ready()
  {
    this.ConnectSignals();
    ((Node) this).GetNode<MegaLabel>(NodePath.op_Implicit("Label")).SetTextAutoSize(new LocString("timeline", "UNLOCK_CONFIRM").GetFormattedText());
    this._tween = ((Node) this).CreateTween();
    float y = this.Position.Y;
    this.Position = new Vector2(this.Position.X, y + 180f);
    this._tween.TweenProperty((GodotObject) this, NodePath.op_Implicit("position:y"), Variant.op_Implicit(y), 0.3).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 10L).SetDelay(0.1);
  }

  protected override void OnUnfocus()
  {
    base.OnUnfocus();
    this._hoverTween?.Kill();
    this._hoverTween = ((Node) this).CreateTween();
    this._hoverTween.TweenProperty((GodotObject) this, NodePath.op_Implicit("scale"), Variant.op_Implicit(Vector2.One), 0.5).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 7L);
  }

  protected override void OnFocus()
  {
    base.OnFocus();
    this._hoverTween?.Kill();
    this._hoverTween = ((Node) this).CreateTween();
    this._hoverTween.TweenProperty((GodotObject) this, NodePath.op_Implicit("scale"), Variant.op_Implicit(Vector2.op_Multiply(Vector2.One, 1.1f)), 0.05);
  }

  public override void _Process(double delta)
  {
  }

  protected override void OnPress()
  {
    base.OnPress();
    this._hoverTween?.Kill();
    this._hoverTween = ((Node) this).CreateTween().SetParallel(true);
    this._hoverTween.TweenProperty((GodotObject) this, NodePath.op_Implicit("scale"), Variant.op_Implicit(Vector2.op_Multiply(Vector2.One, 0.95f)), 0.25).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 5L);
  }

  protected override void OnRelease()
  {
    this.Disable();
    this._hoverTween?.Kill();
    this._hoverTween = ((Node) this).CreateTween().SetParallel(true);
    this._hoverTween.TweenProperty((GodotObject) this, NodePath.op_Implicit("scale"), Variant.op_Implicit(new Vector2(3f, 0.1f)), 0.3).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 5L);
    this._hoverTween.TweenProperty((GodotObject) this, NodePath.op_Implicit("modulate:a"), Variant.op_Implicit(0.0f), 0.25).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 7L);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(6)
    {
      new MethodInfo(NUnlockConfirmButton.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NUnlockConfirmButton.MethodName.OnUnfocus, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NUnlockConfirmButton.MethodName.OnFocus, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NUnlockConfirmButton.MethodName._Process, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 3L, StringName.op_Implicit("delta"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NUnlockConfirmButton.MethodName.OnPress, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NUnlockConfirmButton.MethodName.OnRelease, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NUnlockConfirmButton.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NUnlockConfirmButton.MethodName.OnUnfocus) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnUnfocus();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NUnlockConfirmButton.MethodName.OnFocus) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnFocus();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NUnlockConfirmButton.MethodName._Process) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      ((Node) this)._Process(VariantUtils.ConvertTo<double>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NUnlockConfirmButton.MethodName.OnPress) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnPress();
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NUnlockConfirmButton.MethodName.OnRelease) || ((NativeVariantPtrArgs) ref args).Count != 0)
      return base.InvokeGodotClassMethod(in method, args, out ret);
    this.OnRelease();
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NUnlockConfirmButton.MethodName._Ready) || StringName.op_Equality(ref method, NUnlockConfirmButton.MethodName.OnUnfocus) || StringName.op_Equality(ref method, NUnlockConfirmButton.MethodName.OnFocus) || StringName.op_Equality(ref method, NUnlockConfirmButton.MethodName._Process) || StringName.op_Equality(ref method, NUnlockConfirmButton.MethodName.OnPress) || StringName.op_Equality(ref method, NUnlockConfirmButton.MethodName.OnRelease) || base.HasGodotClassMethod(in method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NUnlockConfirmButton.PropertyName._tween))
    {
      this._tween = VariantUtils.ConvertTo<Tween>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NUnlockConfirmButton.PropertyName._hoverTween))
      return base.SetGodotClassPropertyValue(in name, in value);
    this._hoverTween = VariantUtils.ConvertTo<Tween>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NUnlockConfirmButton.PropertyName.Hotkeys))
    {
      ref godot_variant local = ref value;
      string[] hotkeys = this.Hotkeys;
      godot_variant from = VariantUtils.CreateFrom<string[]>(ref hotkeys);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NUnlockConfirmButton.PropertyName._tween))
    {
      value = VariantUtils.CreateFrom<Tween>(ref this._tween);
      return true;
    }
    if (!StringName.op_Equality(ref name, NUnlockConfirmButton.PropertyName._hoverTween))
      return base.GetGodotClassPropertyValue(in name, out value);
    value = VariantUtils.CreateFrom<Tween>(ref this._hoverTween);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NUnlockConfirmButton.PropertyName._tween, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NUnlockConfirmButton.PropertyName._hoverTween, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 34L, NUnlockConfirmButton.PropertyName.Hotkeys, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    base.SaveGodotObjectData(info);
    info.AddProperty(NUnlockConfirmButton.PropertyName._tween, Variant.From<Tween>(ref this._tween));
    info.AddProperty(NUnlockConfirmButton.PropertyName._hoverTween, Variant.From<Tween>(ref this._hoverTween));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    base.RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NUnlockConfirmButton.PropertyName._tween, ref variant1))
      this._tween = ((Variant) ref variant1).As<Tween>();
    Variant variant2;
    if (!info.TryGetProperty(NUnlockConfirmButton.PropertyName._hoverTween, ref variant2))
      return;
    this._hoverTween = ((Variant) ref variant2).As<Tween>();
  }

  public new class MethodName : NButton.MethodName
  {
    public new static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public new static readonly StringName OnUnfocus = StringName.op_Implicit(nameof (OnUnfocus));
    public new static readonly StringName OnFocus = StringName.op_Implicit(nameof (OnFocus));
    public static readonly StringName _Process = StringName.op_Implicit(nameof (_Process));
    public new static readonly StringName OnPress = StringName.op_Implicit(nameof (OnPress));
    public new static readonly StringName OnRelease = StringName.op_Implicit(nameof (OnRelease));
  }

  public new class PropertyName : NButton.PropertyName
  {
    public new static readonly StringName Hotkeys = StringName.op_Implicit(nameof (Hotkeys));
    public static readonly StringName _tween = StringName.op_Implicit(nameof (_tween));
    public static readonly StringName _hoverTween = StringName.op_Implicit(nameof (_hoverTween));
  }

  public new class SignalName : NButton.SignalName
  {
  }
}
