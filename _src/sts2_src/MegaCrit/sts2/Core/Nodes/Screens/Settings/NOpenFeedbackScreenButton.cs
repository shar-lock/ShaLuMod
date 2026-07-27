// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Screens.Settings.NOpenFeedbackScreenButton
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

[ScriptPath("res://src/Core/Nodes/Screens/Settings/NOpenFeedbackScreenButton.cs")]
public class NOpenFeedbackScreenButton : NSettingsButton
{
  private TextureRect _image;
  private MegaLabel _label;

  public override void _Ready()
  {
    this.ConnectSignals();
    this._image = ((Node) this).GetNode<TextureRect>(NodePath.op_Implicit("Image"));
    this._label = ((Node) this).GetNode<MegaLabel>(NodePath.op_Implicit("Label"));
    this._label.SetTextAutoSize(new LocString("settings_ui", "SEND_FEEDBACK_BUTTON_LABEL").GetFormattedText());
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(1)
    {
      new MethodInfo(NOpenFeedbackScreenButton.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (!StringName.op_Equality(ref method, NOpenFeedbackScreenButton.MethodName._Ready) || ((NativeVariantPtrArgs) ref args).Count != 0)
      return base.InvokeGodotClassMethod(in method, args, out ret);
    ((Node) this)._Ready();
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NOpenFeedbackScreenButton.MethodName._Ready) || base.HasGodotClassMethod(in method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NOpenFeedbackScreenButton.PropertyName._image))
    {
      this._image = VariantUtils.ConvertTo<TextureRect>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NOpenFeedbackScreenButton.PropertyName._label))
      return base.SetGodotClassPropertyValue(in name, in value);
    this._label = VariantUtils.ConvertTo<MegaLabel>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NOpenFeedbackScreenButton.PropertyName._image))
    {
      value = VariantUtils.CreateFrom<TextureRect>(ref this._image);
      return true;
    }
    if (!StringName.op_Equality(ref name, NOpenFeedbackScreenButton.PropertyName._label))
      return base.GetGodotClassPropertyValue(in name, out value);
    value = VariantUtils.CreateFrom<MegaLabel>(ref this._label);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NOpenFeedbackScreenButton.PropertyName._image, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NOpenFeedbackScreenButton.PropertyName._label, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    base.SaveGodotObjectData(info);
    info.AddProperty(NOpenFeedbackScreenButton.PropertyName._image, Variant.From<TextureRect>(ref this._image));
    info.AddProperty(NOpenFeedbackScreenButton.PropertyName._label, Variant.From<MegaLabel>(ref this._label));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    base.RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NOpenFeedbackScreenButton.PropertyName._image, ref variant1))
      this._image = ((Variant) ref variant1).As<TextureRect>();
    Variant variant2;
    if (!info.TryGetProperty(NOpenFeedbackScreenButton.PropertyName._label, ref variant2))
      return;
    this._label = ((Variant) ref variant2).As<MegaLabel>();
  }

  public new class MethodName : NSettingsButton.MethodName
  {
    public new static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
  }

  public new class PropertyName : NSettingsButton.PropertyName
  {
    public new static readonly StringName _image = StringName.op_Implicit(nameof (_image));
    public new static readonly StringName _label = StringName.op_Implicit(nameof (_label));
  }

  public new class SignalName : NSettingsButton.SignalName
  {
  }
}
