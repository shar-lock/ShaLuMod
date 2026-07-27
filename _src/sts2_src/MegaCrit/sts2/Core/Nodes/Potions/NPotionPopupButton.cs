// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Potions.NPotionPopupButton
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.HoverTips;
using System.Collections.Generic;
using System.ComponentModel;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Potions;

[ScriptPath("res://src/Core/Nodes/Potions/NPotionPopupButton.cs")]
public class NPotionPopupButton : NButton
{
  private TextureRect _background;
  private MegaLabel _label;
  private Tween? _currentTween;

  public override void _Ready()
  {
    this.ConnectSignals();
    this._background = ((Node) this).GetNode<TextureRect>(NodePath.op_Implicit("Background"));
    this._label = ((Node) this).GetNode<MegaLabel>(NodePath.op_Implicit("Label"));
    ((CanvasItem) this._background).Modulate = Colors.Transparent;
  }

  public void SetLocKey(string locEntryKey)
  {
    this._label.SetTextAutoSize(new LocString("gameplay_ui", locEntryKey).GetFormattedText());
  }

  protected override void OnFocus()
  {
    this._currentTween?.Kill();
    this._currentTween = ((Node) this).CreateTween();
    this._currentTween.TweenProperty((GodotObject) this._background, NodePath.op_Implicit("modulate:a"), Variant.op_Implicit(0.25f), 0.15000000596046448);
  }

  protected override void OnUnfocus()
  {
    NHoverTipSet.Remove((Control) this);
    this._currentTween?.Kill();
    this._currentTween = ((Node) this).CreateTween();
    this._currentTween.TweenProperty((GodotObject) this._background, NodePath.op_Implicit("modulate:a"), Variant.op_Implicit(0.0f), 0.15000000596046448);
  }

  protected override void OnDisable()
  {
    this._currentTween?.Kill();
    ((CanvasItem) this._background).Modulate = new Color(0.1f, 0.1f, 0.1f, 0.75f);
  }

  protected override void OnEnable()
  {
    this._currentTween?.Kill();
    ((CanvasItem) this._background).Modulate = new Color(1f, 1f, 1f, 0.0f);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(6)
    {
      new MethodInfo(NPotionPopupButton.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NPotionPopupButton.MethodName.SetLocKey, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 4L, StringName.op_Implicit("locEntryKey"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NPotionPopupButton.MethodName.OnFocus, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NPotionPopupButton.MethodName.OnUnfocus, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NPotionPopupButton.MethodName.OnDisable, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NPotionPopupButton.MethodName.OnEnable, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NPotionPopupButton.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NPotionPopupButton.MethodName.SetLocKey) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.SetLocKey(VariantUtils.ConvertTo<string>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NPotionPopupButton.MethodName.OnFocus) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnFocus();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NPotionPopupButton.MethodName.OnUnfocus) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnUnfocus();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NPotionPopupButton.MethodName.OnDisable) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnDisable();
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NPotionPopupButton.MethodName.OnEnable) || ((NativeVariantPtrArgs) ref args).Count != 0)
      return base.InvokeGodotClassMethod(in method, args, out ret);
    this.OnEnable();
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NPotionPopupButton.MethodName._Ready) || StringName.op_Equality(ref method, NPotionPopupButton.MethodName.SetLocKey) || StringName.op_Equality(ref method, NPotionPopupButton.MethodName.OnFocus) || StringName.op_Equality(ref method, NPotionPopupButton.MethodName.OnUnfocus) || StringName.op_Equality(ref method, NPotionPopupButton.MethodName.OnDisable) || StringName.op_Equality(ref method, NPotionPopupButton.MethodName.OnEnable) || base.HasGodotClassMethod(in method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NPotionPopupButton.PropertyName._background))
    {
      this._background = VariantUtils.ConvertTo<TextureRect>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NPotionPopupButton.PropertyName._label))
    {
      this._label = VariantUtils.ConvertTo<MegaLabel>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NPotionPopupButton.PropertyName._currentTween))
      return base.SetGodotClassPropertyValue(in name, in value);
    this._currentTween = VariantUtils.ConvertTo<Tween>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NPotionPopupButton.PropertyName._background))
    {
      value = VariantUtils.CreateFrom<TextureRect>(ref this._background);
      return true;
    }
    if (StringName.op_Equality(ref name, NPotionPopupButton.PropertyName._label))
    {
      value = VariantUtils.CreateFrom<MegaLabel>(ref this._label);
      return true;
    }
    if (!StringName.op_Equality(ref name, NPotionPopupButton.PropertyName._currentTween))
      return base.GetGodotClassPropertyValue(in name, out value);
    value = VariantUtils.CreateFrom<Tween>(ref this._currentTween);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NPotionPopupButton.PropertyName._background, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NPotionPopupButton.PropertyName._label, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NPotionPopupButton.PropertyName._currentTween, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    base.SaveGodotObjectData(info);
    info.AddProperty(NPotionPopupButton.PropertyName._background, Variant.From<TextureRect>(ref this._background));
    info.AddProperty(NPotionPopupButton.PropertyName._label, Variant.From<MegaLabel>(ref this._label));
    info.AddProperty(NPotionPopupButton.PropertyName._currentTween, Variant.From<Tween>(ref this._currentTween));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    base.RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NPotionPopupButton.PropertyName._background, ref variant1))
      this._background = ((Variant) ref variant1).As<TextureRect>();
    Variant variant2;
    if (info.TryGetProperty(NPotionPopupButton.PropertyName._label, ref variant2))
      this._label = ((Variant) ref variant2).As<MegaLabel>();
    Variant variant3;
    if (!info.TryGetProperty(NPotionPopupButton.PropertyName._currentTween, ref variant3))
      return;
    this._currentTween = ((Variant) ref variant3).As<Tween>();
  }

  public new class MethodName : NButton.MethodName
  {
    public new static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName SetLocKey = StringName.op_Implicit(nameof (SetLocKey));
    public new static readonly StringName OnFocus = StringName.op_Implicit(nameof (OnFocus));
    public new static readonly StringName OnUnfocus = StringName.op_Implicit(nameof (OnUnfocus));
    public new static readonly StringName OnDisable = StringName.op_Implicit(nameof (OnDisable));
    public new static readonly StringName OnEnable = StringName.op_Implicit(nameof (OnEnable));
  }

  public new class PropertyName : NButton.PropertyName
  {
    public static readonly StringName _background = StringName.op_Implicit(nameof (_background));
    public static readonly StringName _label = StringName.op_Implicit(nameof (_label));
    public static readonly StringName _currentTween = StringName.op_Implicit(nameof (_currentTween));
  }

  public new class SignalName : NButton.SignalName
  {
  }
}
