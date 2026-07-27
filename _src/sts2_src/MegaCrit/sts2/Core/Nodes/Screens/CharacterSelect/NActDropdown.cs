// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Screens.CharacterSelect.NActDropdown
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Screens.CharacterSelect;

[ScriptPath("res://src/Core/Nodes/Screens/CharacterSelect/NActDropdown.cs")]
public class NActDropdown : NDropdown
{
  private static readonly string[] _options = new string[3]
  {
    "random",
    "overgrowth",
    "underdocks"
  };
  private int _currentOptionIndex = ((IReadOnlyList<string>) NActDropdown._options).IndexOf<string>("random");

  public string CurrentOption => NActDropdown._options[this._currentOptionIndex];

  public override void _Ready()
  {
    this.ConnectSignals();
    this.PopulateOptions();
  }

  protected override void OnFocus()
  {
    ((CanvasItem) this._currentOptionHighlight).Modulate = new Color("afcdde");
  }

  protected override void OnUnfocus()
  {
    ((CanvasItem) this._currentOptionHighlight).Modulate = Colors.White;
  }

  private void PopulateOptions()
  {
    List<NDropdownItem> list = this.GetDropdownItems().ToList<NDropdownItem>();
    for (int index = 0; index < NActDropdown._options.Length; ++index)
    {
      NDropdownItem ndropdownItem1 = list[index];
      string option = NActDropdown._options[index];
      ((GodotObject) ndropdownItem1).Connect(NDropdownItem.SignalName.Selected, Callable.From<NDropdownItem>(new Action<NDropdownItem>(this.OnDropdownItemSelected)), 0U);
      NDropdownItem ndropdownItem2 = ndropdownItem1;
      DefaultInterpolatedStringHandler interpolatedStringHandler = new DefaultInterpolatedStringHandler(0, 2);
      interpolatedStringHandler.AppendFormatted<char>(char.ToUpperInvariant(option[0]));
      ref DefaultInterpolatedStringHandler local = ref interpolatedStringHandler;
      string str1 = option;
      string str2 = str1.Substring(1, str1.Length - 1);
      local.AppendFormatted(str2);
      string stringAndClear = interpolatedStringHandler.ToStringAndClear();
      ndropdownItem2.Text = stringAndClear;
    }
    ((Node) this.GetDropdownContainer()).GetParent<NDropdownContainer>().RefreshLayout();
  }

  private void OnDropdownItemSelected(NDropdownItem item)
  {
    this.CloseDropdown();
    this._currentOptionIndex = this.GetDropdownItems().ToList<NDropdownItem>().IndexOf(item);
    this._currentOptionLabel.SetTextAutoSize(item.Text);
  }

  private Control GetDropdownContainer()
  {
    return ((Node) this).GetNode<Control>(NodePath.op_Implicit("DropdownContainer/VBoxContainer"));
  }

  private IEnumerable<NDropdownItem> GetDropdownItems()
  {
    return ((IEnumerable) ((Node) this.GetDropdownContainer()).GetChildren(false)).OfType<NDropdownItem>();
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(6)
    {
      new MethodInfo(NActDropdown.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NActDropdown.MethodName.OnFocus, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NActDropdown.MethodName.OnUnfocus, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NActDropdown.MethodName.PopulateOptions, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NActDropdown.MethodName.OnDropdownItemSelected, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("item"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false)
      }, (List<Variant>) null),
      new MethodInfo(NActDropdown.MethodName.GetDropdownContainer, new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NActDropdown.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NActDropdown.MethodName.OnFocus) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnFocus();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NActDropdown.MethodName.OnUnfocus) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnUnfocus();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NActDropdown.MethodName.PopulateOptions) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.PopulateOptions();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NActDropdown.MethodName.OnDropdownItemSelected) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.OnDropdownItemSelected(VariantUtils.ConvertTo<NDropdownItem>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NActDropdown.MethodName.GetDropdownContainer) || ((NativeVariantPtrArgs) ref args).Count != 0)
      return base.InvokeGodotClassMethod(in method, args, out ret);
    Control dropdownContainer = this.GetDropdownContainer();
    ret = VariantUtils.CreateFrom<Control>(ref dropdownContainer);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NActDropdown.MethodName._Ready) || StringName.op_Equality(ref method, NActDropdown.MethodName.OnFocus) || StringName.op_Equality(ref method, NActDropdown.MethodName.OnUnfocus) || StringName.op_Equality(ref method, NActDropdown.MethodName.PopulateOptions) || StringName.op_Equality(ref method, NActDropdown.MethodName.OnDropdownItemSelected) || StringName.op_Equality(ref method, NActDropdown.MethodName.GetDropdownContainer) || base.HasGodotClassMethod(in method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (!StringName.op_Equality(ref name, NActDropdown.PropertyName._currentOptionIndex))
      return base.SetGodotClassPropertyValue(in name, in value);
    this._currentOptionIndex = VariantUtils.ConvertTo<int>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NActDropdown.PropertyName.CurrentOption))
    {
      ref godot_variant local = ref value;
      string currentOption = this.CurrentOption;
      godot_variant from = VariantUtils.CreateFrom<string>(ref currentOption);
      local = from;
      return true;
    }
    if (!StringName.op_Equality(ref name, NActDropdown.PropertyName._currentOptionIndex))
      return base.GetGodotClassPropertyValue(in name, out value);
    value = VariantUtils.CreateFrom<int>(ref this._currentOptionIndex);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 2L, NActDropdown.PropertyName._currentOptionIndex, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 4L, NActDropdown.PropertyName.CurrentOption, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    base.SaveGodotObjectData(info);
    info.AddProperty(NActDropdown.PropertyName._currentOptionIndex, Variant.From<int>(ref this._currentOptionIndex));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    base.RestoreGodotObjectData(info);
    Variant variant;
    if (!info.TryGetProperty(NActDropdown.PropertyName._currentOptionIndex, ref variant))
      return;
    this._currentOptionIndex = ((Variant) ref variant).As<int>();
  }

  public new class MethodName : NDropdown.MethodName
  {
    public new static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public new static readonly StringName OnFocus = StringName.op_Implicit(nameof (OnFocus));
    public new static readonly StringName OnUnfocus = StringName.op_Implicit(nameof (OnUnfocus));
    public static readonly StringName PopulateOptions = StringName.op_Implicit(nameof (PopulateOptions));
    public static readonly StringName OnDropdownItemSelected = StringName.op_Implicit(nameof (OnDropdownItemSelected));
    public static readonly StringName GetDropdownContainer = StringName.op_Implicit(nameof (GetDropdownContainer));
  }

  public new class PropertyName : NDropdown.PropertyName
  {
    public static readonly StringName CurrentOption = StringName.op_Implicit(nameof (CurrentOption));
    public static readonly StringName _currentOptionIndex = StringName.op_Implicit(nameof (_currentOptionIndex));
  }

  public new class SignalName : NDropdown.SignalName
  {
  }
}
