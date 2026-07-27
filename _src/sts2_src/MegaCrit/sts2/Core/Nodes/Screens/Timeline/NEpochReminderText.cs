// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Screens.Timeline.NEpochReminderText
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Saves;
using System.Collections.Generic;
using System.ComponentModel;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Screens.Timeline;

[ScriptPath("res://src/Core/Nodes/Screens/Timeline/NEpochReminderText.cs")]
public class NEpochReminderText : Control
{
  private MegaRichTextLabel _label;
  private LocString _loc;
  private Tween? _tween;
  private Control _vfxHolder;

  public override void _Ready()
  {
    this._label = ((Node) this).GetNode<MegaRichTextLabel>(NodePath.op_Implicit("%ReminderLabel"));
  }

  public void AnimateIn()
  {
    int discoveredEpochCount = SaveManager.Instance.GetDiscoveredEpochCount();
    if (discoveredEpochCount == 0)
      return;
    ((CanvasItem) this).Visible = true;
    this._loc = new LocString("timeline", "REMINDER_TEXT");
    this._loc.AddObj("RevealableEpochCount", (object) discoveredEpochCount);
    this._label.Text = this._loc.GetFormattedText();
    this._tween?.Kill();
    this._tween = ((Node) this).CreateTween().SetLoops(0);
    this._tween.TweenProperty((GodotObject) this._label, NodePath.op_Implicit("modulate:a"), Variant.op_Implicit(1f), 0.8);
    this._tween.TweenProperty((GodotObject) this._label, NodePath.op_Implicit("modulate:a"), Variant.op_Implicit(0.25f), 0.8);
  }

  public void AnimateOut()
  {
    this._tween?.Kill();
    this._tween = ((Node) this).CreateTween();
    this._tween.TweenProperty((GodotObject) this._label, NodePath.op_Implicit("modulate:a"), Variant.op_Implicit(0.0f), 0.25);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(3)
    {
      new MethodInfo(NEpochReminderText.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NEpochReminderText.MethodName.AnimateIn, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NEpochReminderText.MethodName.AnimateOut, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NEpochReminderText.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NEpochReminderText.MethodName.AnimateIn) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.AnimateIn();
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NEpochReminderText.MethodName.AnimateOut) || ((NativeVariantPtrArgs) ref args).Count != 0)
      return base.InvokeGodotClassMethod(ref method, args, ref ret);
    this.AnimateOut();
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NEpochReminderText.MethodName._Ready) || StringName.op_Equality(ref method, NEpochReminderText.MethodName.AnimateIn) || StringName.op_Equality(ref method, NEpochReminderText.MethodName.AnimateOut) || base.HasGodotClassMethod(ref method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NEpochReminderText.PropertyName._label))
    {
      this._label = VariantUtils.ConvertTo<MegaRichTextLabel>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NEpochReminderText.PropertyName._tween))
    {
      this._tween = VariantUtils.ConvertTo<Tween>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NEpochReminderText.PropertyName._vfxHolder))
      return ((GodotObject) this).SetGodotClassPropertyValue(ref name, ref value);
    this._vfxHolder = VariantUtils.ConvertTo<Control>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NEpochReminderText.PropertyName._label))
    {
      value = VariantUtils.CreateFrom<MegaRichTextLabel>(ref this._label);
      return true;
    }
    if (StringName.op_Equality(ref name, NEpochReminderText.PropertyName._tween))
    {
      value = VariantUtils.CreateFrom<Tween>(ref this._tween);
      return true;
    }
    if (!StringName.op_Equality(ref name, NEpochReminderText.PropertyName._vfxHolder))
      return ((GodotObject) this).GetGodotClassPropertyValue(ref name, ref value);
    value = VariantUtils.CreateFrom<Control>(ref this._vfxHolder);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NEpochReminderText.PropertyName._label, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NEpochReminderText.PropertyName._tween, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NEpochReminderText.PropertyName._vfxHolder, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).SaveGodotObjectData(info);
    info.AddProperty(NEpochReminderText.PropertyName._label, Variant.From<MegaRichTextLabel>(ref this._label));
    info.AddProperty(NEpochReminderText.PropertyName._tween, Variant.From<Tween>(ref this._tween));
    info.AddProperty(NEpochReminderText.PropertyName._vfxHolder, Variant.From<Control>(ref this._vfxHolder));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NEpochReminderText.PropertyName._label, ref variant1))
      this._label = ((Variant) ref variant1).As<MegaRichTextLabel>();
    Variant variant2;
    if (info.TryGetProperty(NEpochReminderText.PropertyName._tween, ref variant2))
      this._tween = ((Variant) ref variant2).As<Tween>();
    Variant variant3;
    if (!info.TryGetProperty(NEpochReminderText.PropertyName._vfxHolder, ref variant3))
      return;
    this._vfxHolder = ((Variant) ref variant3).As<Control>();
  }

  public class MethodName : Control.MethodName
  {
    public static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName AnimateIn = StringName.op_Implicit(nameof (AnimateIn));
    public static readonly StringName AnimateOut = StringName.op_Implicit(nameof (AnimateOut));
  }

  public class PropertyName : Control.PropertyName
  {
    public static readonly StringName _label = StringName.op_Implicit(nameof (_label));
    public static readonly StringName _tween = StringName.op_Implicit(nameof (_tween));
    public static readonly StringName _vfxHolder = StringName.op_Implicit(nameof (_vfxHolder));
  }

  public class SignalName : Control.SignalName
  {
  }
}
