// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Screens.CustomRun.NCustomRunRandomizeButton
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.ControllerInput;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using System.Collections.Generic;
using System.ComponentModel;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Screens.CustomRun;

[ScriptPath("res://src/Core/Nodes/Screens/CustomRun/NCustomRunRandomizeButton.cs")]
public class NCustomRunRandomizeButton : NButton
{
  private static readonly StringName _v = new StringName("v");
  private static readonly StringName _s = new StringName("s");
  private ShaderMaterial _shaderMaterial;
  private MegaRichTextLabel _label;

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
    this._label = ((Node) this).GetNode<MegaRichTextLabel>(NodePath.op_Implicit("Label"));
    this._label.SetTextAutoSize(new LocString("main_menu_ui", "CUSTOM_RUN_SCREEN.RANDOMIZE").GetFormattedText());
    this._shaderMaterial = (ShaderMaterial) ((CanvasItem) ((Node) this).GetNode<Control>(NodePath.op_Implicit("Background"))).Material;
  }

  protected override void OnFocus()
  {
    this._shaderMaterial.SetShaderParameter(NCustomRunRandomizeButton._s, Variant.op_Implicit(1.1f));
    this._shaderMaterial.SetShaderParameter(NCustomRunRandomizeButton._v, Variant.op_Implicit(1.1f));
  }

  protected override void OnUnfocus()
  {
    this._shaderMaterial.SetShaderParameter(NCustomRunRandomizeButton._s, Variant.op_Implicit(1f));
    this._shaderMaterial.SetShaderParameter(NCustomRunRandomizeButton._v, Variant.op_Implicit(1f));
  }

  protected override void OnEnable()
  {
    base.OnEnable();
    this._shaderMaterial.SetShaderParameter(NCustomRunRandomizeButton._s, Variant.op_Implicit(1f));
    this._shaderMaterial.SetShaderParameter(NCustomRunRandomizeButton._v, Variant.op_Implicit(1f));
    ((CanvasItem) this._label).Modulate = Colors.White;
  }

  protected override void OnDisable()
  {
    base.OnDisable();
    this._shaderMaterial.SetShaderParameter(NCustomRunRandomizeButton._s, Variant.op_Implicit(0.0f));
    this._shaderMaterial.SetShaderParameter(NCustomRunRandomizeButton._v, Variant.op_Implicit(0.5f));
    ((CanvasItem) this._label).Modulate = StsColors.gray;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(5)
    {
      new MethodInfo(NCustomRunRandomizeButton.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCustomRunRandomizeButton.MethodName.OnFocus, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCustomRunRandomizeButton.MethodName.OnUnfocus, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCustomRunRandomizeButton.MethodName.OnEnable, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCustomRunRandomizeButton.MethodName.OnDisable, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NCustomRunRandomizeButton.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCustomRunRandomizeButton.MethodName.OnFocus) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnFocus();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCustomRunRandomizeButton.MethodName.OnUnfocus) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnUnfocus();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCustomRunRandomizeButton.MethodName.OnEnable) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnEnable();
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NCustomRunRandomizeButton.MethodName.OnDisable) || ((NativeVariantPtrArgs) ref args).Count != 0)
      return base.InvokeGodotClassMethod(in method, args, out ret);
    this.OnDisable();
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NCustomRunRandomizeButton.MethodName._Ready) || StringName.op_Equality(ref method, NCustomRunRandomizeButton.MethodName.OnFocus) || StringName.op_Equality(ref method, NCustomRunRandomizeButton.MethodName.OnUnfocus) || StringName.op_Equality(ref method, NCustomRunRandomizeButton.MethodName.OnEnable) || StringName.op_Equality(ref method, NCustomRunRandomizeButton.MethodName.OnDisable) || base.HasGodotClassMethod(in method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NCustomRunRandomizeButton.PropertyName._shaderMaterial))
    {
      this._shaderMaterial = VariantUtils.ConvertTo<ShaderMaterial>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NCustomRunRandomizeButton.PropertyName._label))
      return base.SetGodotClassPropertyValue(in name, in value);
    this._label = VariantUtils.ConvertTo<MegaRichTextLabel>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NCustomRunRandomizeButton.PropertyName.Hotkeys))
    {
      ref godot_variant local = ref value;
      string[] hotkeys = this.Hotkeys;
      godot_variant from = VariantUtils.CreateFrom<string[]>(ref hotkeys);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NCustomRunRandomizeButton.PropertyName._shaderMaterial))
    {
      value = VariantUtils.CreateFrom<ShaderMaterial>(ref this._shaderMaterial);
      return true;
    }
    if (!StringName.op_Equality(ref name, NCustomRunRandomizeButton.PropertyName._label))
      return base.GetGodotClassPropertyValue(in name, out value);
    value = VariantUtils.CreateFrom<MegaRichTextLabel>(ref this._label);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NCustomRunRandomizeButton.PropertyName._shaderMaterial, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NCustomRunRandomizeButton.PropertyName._label, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 34L, NCustomRunRandomizeButton.PropertyName.Hotkeys, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    base.SaveGodotObjectData(info);
    info.AddProperty(NCustomRunRandomizeButton.PropertyName._shaderMaterial, Variant.From<ShaderMaterial>(ref this._shaderMaterial));
    info.AddProperty(NCustomRunRandomizeButton.PropertyName._label, Variant.From<MegaRichTextLabel>(ref this._label));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    base.RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NCustomRunRandomizeButton.PropertyName._shaderMaterial, ref variant1))
      this._shaderMaterial = ((Variant) ref variant1).As<ShaderMaterial>();
    Variant variant2;
    if (!info.TryGetProperty(NCustomRunRandomizeButton.PropertyName._label, ref variant2))
      return;
    this._label = ((Variant) ref variant2).As<MegaRichTextLabel>();
  }

  public new class MethodName : NButton.MethodName
  {
    public new static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public new static readonly StringName OnFocus = StringName.op_Implicit(nameof (OnFocus));
    public new static readonly StringName OnUnfocus = StringName.op_Implicit(nameof (OnUnfocus));
    public new static readonly StringName OnEnable = StringName.op_Implicit(nameof (OnEnable));
    public new static readonly StringName OnDisable = StringName.op_Implicit(nameof (OnDisable));
  }

  public new class PropertyName : NButton.PropertyName
  {
    public new static readonly StringName Hotkeys = StringName.op_Implicit(nameof (Hotkeys));
    public static readonly StringName _shaderMaterial = StringName.op_Implicit(nameof (_shaderMaterial));
    public static readonly StringName _label = StringName.op_Implicit(nameof (_label));
  }

  public new class SignalName : NButton.SignalName
  {
  }
}
