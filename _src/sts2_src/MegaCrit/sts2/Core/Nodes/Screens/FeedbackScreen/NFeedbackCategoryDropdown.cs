// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Screens.FeedbackScreen.NFeedbackCategoryDropdown
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using System;
using System.Collections.Generic;
using System.ComponentModel;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Screens.FeedbackScreen;

[ScriptPath("res://src/Core/Nodes/Screens/FeedbackScreen/NFeedbackCategoryDropdown.cs")]
public class NFeedbackCategoryDropdown : NDropdown
{
  [Export]
  private PackedScene _dropdownItemScene;
  private NSelectionReticle _selectionReticle;
  private static readonly string[] _baseCategories = new string[3]
  {
    "bug",
    "balance",
    "feedback"
  };
  private static readonly LocString[] _baseCategoryLoc = new LocString[3]
  {
    new LocString("settings_ui", "FEEDBACK_CATEGORY.bug"),
    new LocString("settings_ui", "FEEDBACK_CATEGORY.balance"),
    new LocString("settings_ui", "FEEDBACK_CATEGORY.feedback")
  };
  private string[] _categories = Array.Empty<string>();
  private LocString[] _categoryLoc = Array.Empty<LocString>();
  private int _currentCategoryIndex;

  public string CurrentCategory => this._categories[this._currentCategoryIndex];

  public override void _Ready()
  {
    this.ConnectSignals();
    this._currentOptionHighlight = (Control) ((Node) this).GetNode<Panel>(NodePath.op_Implicit("%Highlight"));
    this._currentOptionLabel = ((Node) this).GetNode<MegaLabel>(NodePath.op_Implicit("%Label"));
    this.PopulateOptions();
    this._currentOptionLabel.SetTextAutoSize(this._categoryLoc[this._currentCategoryIndex].GetFormattedText());
    this._selectionReticle = ((Node) this).GetNode<NSelectionReticle>(NodePath.op_Implicit("SelectionReticle"));
  }

  protected override void OnFocus()
  {
    ((CanvasItem) this._currentOptionHighlight).Modulate = new Color("afcdde");
    if (!NControllerManager.Instance.IsUsingController)
      return;
    this._selectionReticle.OnSelect();
  }

  protected override void OnUnfocus()
  {
    this._selectionReticle.OnDeselect();
    ((CanvasItem) this._currentOptionHighlight).Modulate = Colors.White;
  }

  private void PopulateOptions()
  {
    if (LocManager.Instance.Language != "eng")
    {
      string[] baseCategories = NFeedbackCategoryDropdown._baseCategories;
      int num1 = 0;
      string[] strArray = new string[1 + baseCategories.Length];
      ReadOnlySpan<string> readOnlySpan1 = new ReadOnlySpan<string>(baseCategories);
      readOnlySpan1.CopyTo(new Span<string>(strArray).Slice(num1, readOnlySpan1.Length));
      int index1 = num1 + readOnlySpan1.Length;
      strArray[index1] = "translation";
      this._categories = strArray;
      LocString[] baseCategoryLoc = NFeedbackCategoryDropdown._baseCategoryLoc;
      int num2 = 0;
      LocString[] locStringArray = new LocString[1 + baseCategoryLoc.Length];
      ReadOnlySpan<LocString> readOnlySpan2 = new ReadOnlySpan<LocString>(baseCategoryLoc);
      readOnlySpan2.CopyTo(new Span<LocString>(locStringArray).Slice(num2, readOnlySpan2.Length));
      int index2 = num2 + readOnlySpan2.Length;
      locStringArray[index2] = new LocString("settings_ui", "FEEDBACK_CATEGORY.translation");
      this._categoryLoc = locStringArray;
    }
    else
    {
      this._categories = NFeedbackCategoryDropdown._baseCategories;
      this._categoryLoc = NFeedbackCategoryDropdown._baseCategoryLoc;
    }
    this._currentCategoryIndex = ((IReadOnlyList<string>) this._categories).IndexOf<string>("feedback");
    Control node = ((Node) this).GetNode<Control>(NodePath.op_Implicit("DropdownContainer/VBoxContainer"));
    foreach (Node child in ((Node) node).GetChildren(false))
    {
      ((Node) node).RemoveChildSafely(child);
      child.QueueFreeSafely();
    }
    for (int categoryIndex = 0; categoryIndex < this._categories.Length; ++categoryIndex)
    {
      NFeedbackCategoryDropdownItem child = this._dropdownItemScene.Instantiate<NFeedbackCategoryDropdownItem>((PackedScene.GenEditState) 0L);
      ((Node) node).AddChildSafely((Node) child);
      ((GodotObject) child).Connect(NDropdownItem.SignalName.Selected, Callable.From<NDropdownItem>(new Action<NDropdownItem>(this.OnDropdownItemSelected)), 0U);
      child.Init(categoryIndex, this._categoryLoc[categoryIndex].GetFormattedText());
    }
    ((Node) node).GetParent<NDropdownContainer>().RefreshLayout();
  }

  private void OnDropdownItemSelected(NDropdownItem item)
  {
    NFeedbackCategoryDropdownItem categoryDropdownItem = (NFeedbackCategoryDropdownItem) item;
    if (categoryDropdownItem.CategoryIndex == this._currentCategoryIndex)
      return;
    this.CloseDropdown();
    this._currentCategoryIndex = categoryDropdownItem.CategoryIndex;
    this._currentOptionLabel.SetTextAutoSize(this._categoryLoc[this._currentCategoryIndex].GetFormattedText());
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(5)
    {
      new MethodInfo(NFeedbackCategoryDropdown.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NFeedbackCategoryDropdown.MethodName.OnFocus, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NFeedbackCategoryDropdown.MethodName.OnUnfocus, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NFeedbackCategoryDropdown.MethodName.PopulateOptions, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NFeedbackCategoryDropdown.MethodName.OnDropdownItemSelected, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("item"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false)
      }, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NFeedbackCategoryDropdown.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NFeedbackCategoryDropdown.MethodName.OnFocus) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnFocus();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NFeedbackCategoryDropdown.MethodName.OnUnfocus) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnUnfocus();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NFeedbackCategoryDropdown.MethodName.PopulateOptions) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.PopulateOptions();
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NFeedbackCategoryDropdown.MethodName.OnDropdownItemSelected) || ((NativeVariantPtrArgs) ref args).Count != 1)
      return base.InvokeGodotClassMethod(in method, args, out ret);
    this.OnDropdownItemSelected(VariantUtils.ConvertTo<NDropdownItem>(ref ((NativeVariantPtrArgs) ref args)[0]));
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NFeedbackCategoryDropdown.MethodName._Ready) || StringName.op_Equality(ref method, NFeedbackCategoryDropdown.MethodName.OnFocus) || StringName.op_Equality(ref method, NFeedbackCategoryDropdown.MethodName.OnUnfocus) || StringName.op_Equality(ref method, NFeedbackCategoryDropdown.MethodName.PopulateOptions) || StringName.op_Equality(ref method, NFeedbackCategoryDropdown.MethodName.OnDropdownItemSelected) || base.HasGodotClassMethod(in method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NFeedbackCategoryDropdown.PropertyName._dropdownItemScene))
    {
      this._dropdownItemScene = VariantUtils.ConvertTo<PackedScene>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NFeedbackCategoryDropdown.PropertyName._selectionReticle))
    {
      this._selectionReticle = VariantUtils.ConvertTo<NSelectionReticle>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NFeedbackCategoryDropdown.PropertyName._categories))
    {
      this._categories = VariantUtils.ConvertTo<string[]>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NFeedbackCategoryDropdown.PropertyName._currentCategoryIndex))
      return base.SetGodotClassPropertyValue(in name, in value);
    this._currentCategoryIndex = VariantUtils.ConvertTo<int>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NFeedbackCategoryDropdown.PropertyName.CurrentCategory))
    {
      ref godot_variant local = ref value;
      string currentCategory = this.CurrentCategory;
      godot_variant from = VariantUtils.CreateFrom<string>(ref currentCategory);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NFeedbackCategoryDropdown.PropertyName._dropdownItemScene))
    {
      value = VariantUtils.CreateFrom<PackedScene>(ref this._dropdownItemScene);
      return true;
    }
    if (StringName.op_Equality(ref name, NFeedbackCategoryDropdown.PropertyName._selectionReticle))
    {
      value = VariantUtils.CreateFrom<NSelectionReticle>(ref this._selectionReticle);
      return true;
    }
    if (StringName.op_Equality(ref name, NFeedbackCategoryDropdown.PropertyName._categories))
    {
      value = VariantUtils.CreateFrom<string[]>(ref this._categories);
      return true;
    }
    if (!StringName.op_Equality(ref name, NFeedbackCategoryDropdown.PropertyName._currentCategoryIndex))
      return base.GetGodotClassPropertyValue(in name, out value);
    value = VariantUtils.CreateFrom<int>(ref this._currentCategoryIndex);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NFeedbackCategoryDropdown.PropertyName._dropdownItemScene, (PropertyHint) 17L, "PackedScene", (PropertyUsageFlags) 4102L, true),
      new PropertyInfo((Variant.Type) 24L, NFeedbackCategoryDropdown.PropertyName._selectionReticle, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 34L, NFeedbackCategoryDropdown.PropertyName._categories, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 2L, NFeedbackCategoryDropdown.PropertyName._currentCategoryIndex, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 4L, NFeedbackCategoryDropdown.PropertyName.CurrentCategory, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    base.SaveGodotObjectData(info);
    info.AddProperty(NFeedbackCategoryDropdown.PropertyName._dropdownItemScene, Variant.From<PackedScene>(ref this._dropdownItemScene));
    info.AddProperty(NFeedbackCategoryDropdown.PropertyName._selectionReticle, Variant.From<NSelectionReticle>(ref this._selectionReticle));
    info.AddProperty(NFeedbackCategoryDropdown.PropertyName._categories, Variant.From<string[]>(ref this._categories));
    info.AddProperty(NFeedbackCategoryDropdown.PropertyName._currentCategoryIndex, Variant.From<int>(ref this._currentCategoryIndex));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    base.RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NFeedbackCategoryDropdown.PropertyName._dropdownItemScene, ref variant1))
      this._dropdownItemScene = ((Variant) ref variant1).As<PackedScene>();
    Variant variant2;
    if (info.TryGetProperty(NFeedbackCategoryDropdown.PropertyName._selectionReticle, ref variant2))
      this._selectionReticle = ((Variant) ref variant2).As<NSelectionReticle>();
    Variant variant3;
    if (info.TryGetProperty(NFeedbackCategoryDropdown.PropertyName._categories, ref variant3))
      this._categories = ((Variant) ref variant3).As<string[]>();
    Variant variant4;
    if (!info.TryGetProperty(NFeedbackCategoryDropdown.PropertyName._currentCategoryIndex, ref variant4))
      return;
    this._currentCategoryIndex = ((Variant) ref variant4).As<int>();
  }

  public new class MethodName : NDropdown.MethodName
  {
    public new static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public new static readonly StringName OnFocus = StringName.op_Implicit(nameof (OnFocus));
    public new static readonly StringName OnUnfocus = StringName.op_Implicit(nameof (OnUnfocus));
    public static readonly StringName PopulateOptions = StringName.op_Implicit(nameof (PopulateOptions));
    public static readonly StringName OnDropdownItemSelected = StringName.op_Implicit(nameof (OnDropdownItemSelected));
  }

  public new class PropertyName : NDropdown.PropertyName
  {
    public static readonly StringName CurrentCategory = StringName.op_Implicit(nameof (CurrentCategory));
    public static readonly StringName _dropdownItemScene = StringName.op_Implicit(nameof (_dropdownItemScene));
    public static readonly StringName _selectionReticle = StringName.op_Implicit(nameof (_selectionReticle));
    public static readonly StringName _categories = StringName.op_Implicit(nameof (_categories));
    public static readonly StringName _currentCategoryIndex = StringName.op_Implicit(nameof (_currentCategoryIndex));
  }

  public new class SignalName : NDropdown.SignalName
  {
  }
}
