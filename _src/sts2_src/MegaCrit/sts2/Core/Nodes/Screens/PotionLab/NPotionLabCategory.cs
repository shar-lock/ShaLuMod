// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Screens.PotionLab.NPotionLabCategory
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.Entities.Potions;
using MegaCrit.Sts2.Core.Entities.UI;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Unlocks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Screens.PotionLab;

[ScriptPath("res://src/Core/Nodes/Screens/PotionLab/NPotionLabCategory.cs")]
public class NPotionLabCategory : VBoxContainer
{
  private MegaRichTextLabel _headerLabel;
  private GridContainer _potionContainer;

  public override void _Ready()
  {
    this._headerLabel = ((Node) this).GetNode<MegaRichTextLabel>(NodePath.op_Implicit("Header"));
    this._potionContainer = ((Node) this).GetNode<GridContainer>(NodePath.op_Implicit("%PotionsContainer"));
  }

  public void LoadPotions(
    PotionRarity potionRarity,
    LocString header,
    HashSet<PotionModel> seenPotions,
    UnlockState unlockState,
    HashSet<PotionModel> allUnlockedPotions,
    PotionRarity? secondRarity = null)
  {
    this._headerLabel.Text = header.GetFormattedText();
    IEnumerable<PotionModel> potionModels = ModelDb.AllPotions.Where<PotionModel>((Func<PotionModel, bool>) (relic =>
    {
      if (relic.Rarity == potionRarity)
        return true;
      int rarity = (int) relic.Rarity;
      PotionRarity? nullable = secondRarity;
      int valueOrDefault = (int) nullable.GetValueOrDefault();
      return rarity == valueOrDefault & nullable.HasValue;
    }));
    List<PotionModel> second = new List<PotionModel>();
    List<PotionModel> first = new List<PotionModel>();
    foreach (PotionPoolModel characterPotionPool in ModelDb.AllCharacterPotionPools)
    {
      foreach (PotionModel potionModel in potionModels)
      {
        if (characterPotionPool.AllPotionIds.Contains<ModelId>(potionModel.Id))
          second.Add(potionModel);
      }
    }
    foreach (PotionModel potionModel in potionModels)
    {
      if (!second.Contains(potionModel))
        first.Add(potionModel);
    }
    first.Sort((Comparison<PotionModel>) ((p1, p2) => LocManager.Instance.StringComparer.Compare(p1.Title.GetFormattedText(), p2.Title.GetFormattedText())));
    foreach (PotionModel potionModel in first.Concat<PotionModel>((IEnumerable<PotionModel>) second))
    {
      ModelVisibility visibility = !allUnlockedPotions.Contains(potionModel) ? ModelVisibility.Locked : (!seenPotions.Contains(potionModel) ? ModelVisibility.NotSeen : ModelVisibility.Visible);
      ((Node) this._potionContainer).AddChildSafely((Node) NLabPotionHolder.Create(potionModel.ToMutable(), visibility));
    }
  }

  public void ClearPotions()
  {
    foreach (Node child in ((Node) this._potionContainer).GetChildren(false))
      child.QueueFreeSafely();
  }

  public Control? DefaultFocusedControl
  {
    get
    {
      return ((Node) this._potionContainer).GetChildCount(false) <= 0 ? (Control) null : ((Node) this._potionContainer).GetChild<Control>(0, false);
    }
  }

  public List<IReadOnlyList<Control>> GetGridItems()
  {
    List<IReadOnlyList<Control>> gridItems = new List<IReadOnlyList<Control>>();
    for (int count = 0; count < ((Node) this._potionContainer).GetChildren(false).Count; count += this._potionContainer.Columns)
      gridItems.Add((IReadOnlyList<Control>) ((IEnumerable) ((Node) this._potionContainer).GetChildren(false)).OfType<Control>().Skip<Control>(count).Take<Control>(this._potionContainer.Columns).ToList<Control>());
    return gridItems;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(2)
    {
      new MethodInfo(NPotionLabCategory.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NPotionLabCategory.MethodName.ClearPotions, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NPotionLabCategory.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NPotionLabCategory.MethodName.ClearPotions) || ((NativeVariantPtrArgs) ref args).Count != 0)
      return base.InvokeGodotClassMethod(ref method, args, ref ret);
    this.ClearPotions();
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NPotionLabCategory.MethodName._Ready) || StringName.op_Equality(ref method, NPotionLabCategory.MethodName.ClearPotions) || base.HasGodotClassMethod(ref method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NPotionLabCategory.PropertyName._headerLabel))
    {
      this._headerLabel = VariantUtils.ConvertTo<MegaRichTextLabel>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NPotionLabCategory.PropertyName._potionContainer))
      return ((GodotObject) this).SetGodotClassPropertyValue(ref name, ref value);
    this._potionContainer = VariantUtils.ConvertTo<GridContainer>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NPotionLabCategory.PropertyName.DefaultFocusedControl))
    {
      ref godot_variant local = ref value;
      Control defaultFocusedControl = this.DefaultFocusedControl;
      godot_variant from = VariantUtils.CreateFrom<Control>(ref defaultFocusedControl);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NPotionLabCategory.PropertyName._headerLabel))
    {
      value = VariantUtils.CreateFrom<MegaRichTextLabel>(ref this._headerLabel);
      return true;
    }
    if (!StringName.op_Equality(ref name, NPotionLabCategory.PropertyName._potionContainer))
      return ((GodotObject) this).GetGodotClassPropertyValue(ref name, ref value);
    value = VariantUtils.CreateFrom<GridContainer>(ref this._potionContainer);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NPotionLabCategory.PropertyName._headerLabel, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NPotionLabCategory.PropertyName._potionContainer, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NPotionLabCategory.PropertyName.DefaultFocusedControl, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).SaveGodotObjectData(info);
    info.AddProperty(NPotionLabCategory.PropertyName._headerLabel, Variant.From<MegaRichTextLabel>(ref this._headerLabel));
    info.AddProperty(NPotionLabCategory.PropertyName._potionContainer, Variant.From<GridContainer>(ref this._potionContainer));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NPotionLabCategory.PropertyName._headerLabel, ref variant1))
      this._headerLabel = ((Variant) ref variant1).As<MegaRichTextLabel>();
    Variant variant2;
    if (!info.TryGetProperty(NPotionLabCategory.PropertyName._potionContainer, ref variant2))
      return;
    this._potionContainer = ((Variant) ref variant2).As<GridContainer>();
  }

  public class MethodName : VBoxContainer.MethodName
  {
    public static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName ClearPotions = StringName.op_Implicit(nameof (ClearPotions));
  }

  public class PropertyName : VBoxContainer.PropertyName
  {
    public static readonly StringName DefaultFocusedControl = StringName.op_Implicit(nameof (DefaultFocusedControl));
    public static readonly StringName _headerLabel = StringName.op_Implicit(nameof (_headerLabel));
    public static readonly StringName _potionContainer = StringName.op_Implicit(nameof (_potionContainer));
  }

  public class SignalName : VBoxContainer.SignalName
  {
  }
}
