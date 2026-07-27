// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Screens.Settings.NSettingsToast
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.Localization;
using System.Collections.Generic;
using System.ComponentModel;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Screens.Settings;

[Tool]
[ScriptPath("res://src/Core/Nodes/Screens/Settings/NSettingsToast.cs")]
public class NSettingsToast : Control
{
  private MegaRichTextLabel _label;
  private Tween? _tween;
  private float _originalY;

  public override void _Ready()
  {
    this._label = ((Node) this).GetNode<MegaRichTextLabel>(NodePath.op_Implicit("Label"));
    this._originalY = this.Position.Y;
  }

  public void Show(LocString locString)
  {
    this._label.SetTextAutoSize(locString.GetFormattedText());
    this.Scale = Vector2.One;
    Vector2 position = this.Position;
    position.Y = this._originalY;
    this.Position = position;
    Color modulate = ((CanvasItem) this).Modulate;
    modulate.A = 1f;
    ((CanvasItem) this).Modulate = modulate;
    this._tween?.Kill();
    this._tween = ((Node) this).CreateTween();
    this._tween.TweenProperty((GodotObject) this, NodePath.op_Implicit("position:y"), Variant.op_Implicit(this._originalY - 120f), 0.25).SetTrans((Tween.TransitionType) 10L).SetEase((Tween.EaseType) 1L);
    this._tween.TweenInterval(1.0);
    this._tween.TweenProperty((GodotObject) this, NodePath.op_Implicit("modulate:a"), Variant.op_Implicit(0.0f), 0.5);
    this._tween.TweenProperty((GodotObject) this, NodePath.op_Implicit("scale"), Variant.op_Implicit(Vector2.op_Multiply(Vector2.One, 0.8f)), 0.5);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(1)
    {
      new MethodInfo(NSettingsToast.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (!StringName.op_Equality(ref method, NSettingsToast.MethodName._Ready) || ((NativeVariantPtrArgs) ref args).Count != 0)
      return base.InvokeGodotClassMethod(ref method, args, ref ret);
    ((Node) this)._Ready();
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NSettingsToast.MethodName._Ready) || base.HasGodotClassMethod(ref method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NSettingsToast.PropertyName._label))
    {
      this._label = VariantUtils.ConvertTo<MegaRichTextLabel>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NSettingsToast.PropertyName._tween))
    {
      this._tween = VariantUtils.ConvertTo<Tween>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NSettingsToast.PropertyName._originalY))
      return ((GodotObject) this).SetGodotClassPropertyValue(ref name, ref value);
    this._originalY = VariantUtils.ConvertTo<float>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NSettingsToast.PropertyName._label))
    {
      value = VariantUtils.CreateFrom<MegaRichTextLabel>(ref this._label);
      return true;
    }
    if (StringName.op_Equality(ref name, NSettingsToast.PropertyName._tween))
    {
      value = VariantUtils.CreateFrom<Tween>(ref this._tween);
      return true;
    }
    if (!StringName.op_Equality(ref name, NSettingsToast.PropertyName._originalY))
      return ((GodotObject) this).GetGodotClassPropertyValue(ref name, ref value);
    value = VariantUtils.CreateFrom<float>(ref this._originalY);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NSettingsToast.PropertyName._label, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NSettingsToast.PropertyName._tween, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 3L, NSettingsToast.PropertyName._originalY, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).SaveGodotObjectData(info);
    info.AddProperty(NSettingsToast.PropertyName._label, Variant.From<MegaRichTextLabel>(ref this._label));
    info.AddProperty(NSettingsToast.PropertyName._tween, Variant.From<Tween>(ref this._tween));
    info.AddProperty(NSettingsToast.PropertyName._originalY, Variant.From<float>(ref this._originalY));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NSettingsToast.PropertyName._label, ref variant1))
      this._label = ((Variant) ref variant1).As<MegaRichTextLabel>();
    Variant variant2;
    if (info.TryGetProperty(NSettingsToast.PropertyName._tween, ref variant2))
      this._tween = ((Variant) ref variant2).As<Tween>();
    Variant variant3;
    if (!info.TryGetProperty(NSettingsToast.PropertyName._originalY, ref variant3))
      return;
    this._originalY = ((Variant) ref variant3).As<float>();
  }

  public class MethodName : Control.MethodName
  {
    public static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
  }

  public class PropertyName : Control.PropertyName
  {
    public static readonly StringName _label = StringName.op_Implicit(nameof (_label));
    public static readonly StringName _tween = StringName.op_Implicit(nameof (_tween));
    public static readonly StringName _originalY = StringName.op_Implicit(nameof (_originalY));
  }

  public class SignalName : Control.SignalName
  {
  }
}
