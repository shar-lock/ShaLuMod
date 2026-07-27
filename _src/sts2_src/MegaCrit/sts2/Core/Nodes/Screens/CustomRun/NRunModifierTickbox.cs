// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Screens.CustomRun.NRunModifierTickbox
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.TestSupport;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Screens.CustomRun;

[ScriptPath("res://src/Core/Nodes/Screens/CustomRun/NRunModifierTickbox.cs")]
public class NRunModifierTickbox : NTickbox
{
  public const string scenePath = "res://scenes/screens/custom_run/modifier_tickbox.tscn";
  private static readonly LocString _descriptionLoc = new LocString("main_menu_ui", "CUSTOM_RUN_SCREEN.MODIFIER_LABEL");
  private MegaRichTextLabel _label;
  private Control _highlight;

  public ModifierModel? Modifier { get; private set; }

  public static NRunModifierTickbox? Create(ModifierModel model)
  {
    if (TestMode.IsOn)
      return (NRunModifierTickbox) null;
    NRunModifierTickbox nrunModifierTickbox = PreloadManager.Cache.GetScene("res://scenes/screens/custom_run/modifier_tickbox.tscn").Instantiate<NRunModifierTickbox>((PackedScene.GenEditState) 0L);
    nrunModifierTickbox.Modifier = model;
    return nrunModifierTickbox;
  }

  protected override void OnEnable()
  {
    base.OnEnable();
    ((CanvasItem) this).Modulate = Colors.White;
  }

  protected override void OnDisable()
  {
    base.OnDisable();
    ((CanvasItem) this).Modulate = Colors.Gray;
  }

  public override void _Ready()
  {
    this.ConnectSignals();
    this._label = ((Node) this).GetNode<MegaRichTextLabel>(NodePath.op_Implicit("%Description"));
    this._highlight = ((Node) this).GetNode<Control>(NodePath.op_Implicit("Highlight"));
    if (this.Modifier != null)
    {
      string variable = ModelDb.GoodModifiers.FirstOrDefault<ModifierModel>((Func<ModifierModel, bool>) (m => m.GetType() == this.Modifier.GetType())) != null ? "green" : (ModelDb.BadModifiers.FirstOrDefault<ModifierModel>((Func<ModifierModel, bool>) (m => m.GetType() == this.Modifier.GetType())) != null ? "red" : "blue");
      NRunModifierTickbox._descriptionLoc.Add("color", variable);
      NRunModifierTickbox._descriptionLoc.Add("modifier_title", this.Modifier.Title.GetFormattedText());
      NRunModifierTickbox._descriptionLoc.Add("modifier_description", this.Modifier.Description.GetFormattedText());
      this._label.Text = NRunModifierTickbox._descriptionLoc.GetFormattedText();
    }
    this.IsTicked = false;
  }

  protected override void OnFocus()
  {
    base.OnFocus();
    if (!NControllerManager.Instance.IsUsingController)
      return;
    ((CanvasItem) this._highlight).Visible = true;
  }

  protected override void OnUnfocus()
  {
    base.OnUnfocus();
    ((CanvasItem) this._highlight).Visible = false;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(5)
    {
      new MethodInfo(NRunModifierTickbox.MethodName.OnEnable, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NRunModifierTickbox.MethodName.OnDisable, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NRunModifierTickbox.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NRunModifierTickbox.MethodName.OnFocus, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NRunModifierTickbox.MethodName.OnUnfocus, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NRunModifierTickbox.MethodName.OnEnable) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnEnable();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NRunModifierTickbox.MethodName.OnDisable) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnDisable();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NRunModifierTickbox.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NRunModifierTickbox.MethodName.OnFocus) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnFocus();
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NRunModifierTickbox.MethodName.OnUnfocus) || ((NativeVariantPtrArgs) ref args).Count != 0)
      return base.InvokeGodotClassMethod(in method, args, out ret);
    this.OnUnfocus();
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NRunModifierTickbox.MethodName.OnEnable) || StringName.op_Equality(ref method, NRunModifierTickbox.MethodName.OnDisable) || StringName.op_Equality(ref method, NRunModifierTickbox.MethodName._Ready) || StringName.op_Equality(ref method, NRunModifierTickbox.MethodName.OnFocus) || StringName.op_Equality(ref method, NRunModifierTickbox.MethodName.OnUnfocus) || base.HasGodotClassMethod(in method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NRunModifierTickbox.PropertyName._label))
    {
      this._label = VariantUtils.ConvertTo<MegaRichTextLabel>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NRunModifierTickbox.PropertyName._highlight))
      return base.SetGodotClassPropertyValue(in name, in value);
    this._highlight = VariantUtils.ConvertTo<Control>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NRunModifierTickbox.PropertyName._label))
    {
      value = VariantUtils.CreateFrom<MegaRichTextLabel>(ref this._label);
      return true;
    }
    if (!StringName.op_Equality(ref name, NRunModifierTickbox.PropertyName._highlight))
      return base.GetGodotClassPropertyValue(in name, out value);
    value = VariantUtils.CreateFrom<Control>(ref this._highlight);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NRunModifierTickbox.PropertyName._label, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NRunModifierTickbox.PropertyName._highlight, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    base.SaveGodotObjectData(info);
    info.AddProperty(NRunModifierTickbox.PropertyName._label, Variant.From<MegaRichTextLabel>(ref this._label));
    info.AddProperty(NRunModifierTickbox.PropertyName._highlight, Variant.From<Control>(ref this._highlight));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    base.RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NRunModifierTickbox.PropertyName._label, ref variant1))
      this._label = ((Variant) ref variant1).As<MegaRichTextLabel>();
    Variant variant2;
    if (!info.TryGetProperty(NRunModifierTickbox.PropertyName._highlight, ref variant2))
      return;
    this._highlight = ((Variant) ref variant2).As<Control>();
  }

  public new class MethodName : NTickbox.MethodName
  {
    public new static readonly StringName OnEnable = StringName.op_Implicit(nameof (OnEnable));
    public new static readonly StringName OnDisable = StringName.op_Implicit(nameof (OnDisable));
    public new static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public new static readonly StringName OnFocus = StringName.op_Implicit(nameof (OnFocus));
    public new static readonly StringName OnUnfocus = StringName.op_Implicit(nameof (OnUnfocus));
  }

  public new class PropertyName : NTickbox.PropertyName
  {
    public static readonly StringName _label = StringName.op_Implicit(nameof (_label));
    public static readonly StringName _highlight = StringName.op_Implicit(nameof (_highlight));
  }

  public new class SignalName : NTickbox.SignalName
  {
  }
}
