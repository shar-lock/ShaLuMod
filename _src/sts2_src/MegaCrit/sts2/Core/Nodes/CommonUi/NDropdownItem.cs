// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.CommonUi.NDropdownItem
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using System;
using System.Collections.Generic;
using System.ComponentModel;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.CommonUi;

[ScriptPath("res://src/Core/Nodes/CommonUi/NDropdownItem.cs")]
public class NDropdownItem : NButton
{
  private ColorRect _highlight;
  protected MegaLabel _label;
  protected MegaRichTextLabel? _richLabel;
  private 
  #nullable disable
  NDropdownItem.SelectedEventHandler backing_Selected;

  public 
  #nullable enable
  string Text
  {
    get => this._label.Text;
    set => this._label.SetTextAutoSize(value);
  }

  public override void _Ready()
  {
    this.ConnectSignals();
    this._highlight = ((Node) this).GetNode<ColorRect>(NodePath.op_Implicit("Highlight"));
    this._label = ((Node) this).GetNodeOrNull<MegaLabel>(NodePath.op_Implicit("Label"));
    this._richLabel = ((Node) this).GetNodeOrNull<MegaRichTextLabel>(NodePath.op_Implicit("RichLabel"));
  }

  protected override void OnFocus() => ((CanvasItem) this._highlight).Visible = true;

  protected override void OnUnfocus() => ((CanvasItem) this._highlight).Visible = false;

  protected override void OnPress() => ((CanvasItem) this._highlight).Visible = false;

  protected sealed override void OnRelease()
  {
    ((CanvasItem) this._highlight).Visible = true;
    ((GodotObject) this).EmitSignal(NDropdownItem.SignalName.Selected, new Variant[1]
    {
      Variant.op_Implicit((GodotObject) this)
    });
  }

  public void UnhoverSelection() => this.OnUnfocus();

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(6)
    {
      new MethodInfo(NDropdownItem.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NDropdownItem.MethodName.OnFocus, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NDropdownItem.MethodName.OnUnfocus, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NDropdownItem.MethodName.OnPress, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NDropdownItem.MethodName.OnRelease, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NDropdownItem.MethodName.UnhoverSelection, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NDropdownItem.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NDropdownItem.MethodName.OnFocus) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnFocus();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NDropdownItem.MethodName.OnUnfocus) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnUnfocus();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NDropdownItem.MethodName.OnPress) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnPress();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NDropdownItem.MethodName.OnRelease) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnRelease();
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NDropdownItem.MethodName.UnhoverSelection) || ((NativeVariantPtrArgs) ref args).Count != 0)
      return base.InvokeGodotClassMethod(in method, args, out ret);
    this.UnhoverSelection();
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NDropdownItem.MethodName._Ready) || StringName.op_Equality(ref method, NDropdownItem.MethodName.OnFocus) || StringName.op_Equality(ref method, NDropdownItem.MethodName.OnUnfocus) || StringName.op_Equality(ref method, NDropdownItem.MethodName.OnPress) || StringName.op_Equality(ref method, NDropdownItem.MethodName.OnRelease) || StringName.op_Equality(ref method, NDropdownItem.MethodName.UnhoverSelection) || base.HasGodotClassMethod(in method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NDropdownItem.PropertyName.Text))
    {
      this.Text = VariantUtils.ConvertTo<string>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NDropdownItem.PropertyName._highlight))
    {
      this._highlight = VariantUtils.ConvertTo<ColorRect>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NDropdownItem.PropertyName._label))
    {
      this._label = VariantUtils.ConvertTo<MegaLabel>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NDropdownItem.PropertyName._richLabel))
      return base.SetGodotClassPropertyValue(in name, in value);
    this._richLabel = VariantUtils.ConvertTo<MegaRichTextLabel>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NDropdownItem.PropertyName.Text))
    {
      ref godot_variant local = ref value;
      string text = this.Text;
      godot_variant from = VariantUtils.CreateFrom<string>(ref text);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NDropdownItem.PropertyName._highlight))
    {
      value = VariantUtils.CreateFrom<ColorRect>(ref this._highlight);
      return true;
    }
    if (StringName.op_Equality(ref name, NDropdownItem.PropertyName._label))
    {
      value = VariantUtils.CreateFrom<MegaLabel>(ref this._label);
      return true;
    }
    if (!StringName.op_Equality(ref name, NDropdownItem.PropertyName._richLabel))
      return base.GetGodotClassPropertyValue(in name, out value);
    value = VariantUtils.CreateFrom<MegaRichTextLabel>(ref this._richLabel);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NDropdownItem.PropertyName._highlight, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NDropdownItem.PropertyName._label, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NDropdownItem.PropertyName._richLabel, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 4L, NDropdownItem.PropertyName.Text, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    base.SaveGodotObjectData(info);
    GodotSerializationInfo serializationInfo = info;
    StringName text1 = NDropdownItem.PropertyName.Text;
    string text2 = this.Text;
    Variant variant = Variant.From<string>(ref text2);
    serializationInfo.AddProperty(text1, variant);
    info.AddProperty(NDropdownItem.PropertyName._highlight, Variant.From<ColorRect>(ref this._highlight));
    info.AddProperty(NDropdownItem.PropertyName._label, Variant.From<MegaLabel>(ref this._label));
    info.AddProperty(NDropdownItem.PropertyName._richLabel, Variant.From<MegaRichTextLabel>(ref this._richLabel));
    info.AddSignalEventDelegate(NDropdownItem.SignalName.Selected, (Delegate) this.backing_Selected);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    base.RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NDropdownItem.PropertyName.Text, ref variant1))
      this.Text = ((Variant) ref variant1).As<string>();
    Variant variant2;
    if (info.TryGetProperty(NDropdownItem.PropertyName._highlight, ref variant2))
      this._highlight = ((Variant) ref variant2).As<ColorRect>();
    Variant variant3;
    if (info.TryGetProperty(NDropdownItem.PropertyName._label, ref variant3))
      this._label = ((Variant) ref variant3).As<MegaLabel>();
    Variant variant4;
    if (info.TryGetProperty(NDropdownItem.PropertyName._richLabel, ref variant4))
      this._richLabel = ((Variant) ref variant4).As<MegaRichTextLabel>();
    NDropdownItem.SelectedEventHandler selectedEventHandler;
    if (!info.TryGetSignalEventDelegate<NDropdownItem.SelectedEventHandler>(NDropdownItem.SignalName.Selected, ref selectedEventHandler))
      return;
    this.backing_Selected = selectedEventHandler;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static List<MethodInfo> GetGodotSignalList()
  {
    return new List<MethodInfo>(1)
    {
      new MethodInfo(NDropdownItem.SignalName.Selected, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("cardHolder"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false)
      }, (List<Variant>) null)
    };
  }

  public event NDropdownItem.SelectedEventHandler Selected
  {
    add => this.backing_Selected += value;
    remove => this.backing_Selected -= value;
  }

  protected void EmitSignalSelected(NDropdownItem cardHolder)
  {
    ((GodotObject) this).EmitSignal(NDropdownItem.SignalName.Selected, new Variant[1]
    {
      Variant.op_Implicit((GodotObject) cardHolder)
    });
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RaiseGodotClassSignalCallbacks(
    in godot_string_name signal,
    NativeVariantPtrArgs args)
  {
    if (StringName.op_Equality(ref signal, NDropdownItem.SignalName.Selected) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      NDropdownItem.SelectedEventHandler backingSelected = this.backing_Selected;
      if (backingSelected == null)
        return;
      backingSelected(VariantUtils.ConvertTo<NDropdownItem>(ref ((NativeVariantPtrArgs) ref args)[0]));
    }
    else
      base.RaiseGodotClassSignalCallbacks(in signal, args);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassSignal(in godot_string_name signal)
  {
    return StringName.op_Equality(ref signal, NDropdownItem.SignalName.Selected) || base.HasGodotClassSignal(in signal);
  }

  [Signal]
  public delegate void SelectedEventHandler(
  #nullable enable
  NDropdownItem cardHolder);

  public new class MethodName : NButton.MethodName
  {
    public new static readonly 
    #nullable disable
    StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public new static readonly StringName OnFocus = StringName.op_Implicit(nameof (OnFocus));
    public new static readonly StringName OnUnfocus = StringName.op_Implicit(nameof (OnUnfocus));
    public new static readonly StringName OnPress = StringName.op_Implicit(nameof (OnPress));
    public new static readonly StringName OnRelease = StringName.op_Implicit(nameof (OnRelease));
    public static readonly StringName UnhoverSelection = StringName.op_Implicit(nameof (UnhoverSelection));
  }

  public new class PropertyName : NButton.PropertyName
  {
    public static readonly StringName Text = StringName.op_Implicit(nameof (Text));
    public static readonly StringName _highlight = StringName.op_Implicit(nameof (_highlight));
    public static readonly StringName _label = StringName.op_Implicit(nameof (_label));
    public static readonly StringName _richLabel = StringName.op_Implicit(nameof (_richLabel));
  }

  public new class SignalName : NButton.SignalName
  {
    public static readonly StringName Selected = StringName.op_Implicit(nameof (Selected));
  }
}
