// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Cards.NEnchantPreview
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Cards.Holders;
using System;
using System.Collections.Generic;
using System.ComponentModel;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Cards;

[ScriptPath("res://src/Core/Nodes/Cards/NEnchantPreview.cs")]
public class NEnchantPreview : Control
{
  private Control _before;
  private Control _after;
  private Control _arrows;

  public override void _Ready()
  {
    this._before = ((Node) this).GetNode<Control>(NodePath.op_Implicit("%Before"));
    this._after = ((Node) this).GetNode<Control>(NodePath.op_Implicit("%After"));
    this._arrows = ((Node) this).GetNode<Control>(NodePath.op_Implicit("Arrows"));
  }

  public void Init(CardModel card, EnchantmentModel canonicalEnchantment, int amount)
  {
    canonicalEnchantment.AssertCanonical();
    this.RemoveExistingCards();
    NPreviewCardHolder child1 = NPreviewCardHolder.Create(NCard.Create(card), true, false);
    ((Node) this._before).AddChildSafely((Node) child1);
    child1.CardNode.UpdateVisuals(card.Pile.Type, CardPreviewMode.Normal);
    CardModel card1 = card.CardScope.CloneCard(card);
    EnchantmentModel mutable = canonicalEnchantment.ToMutable();
    card1.EnchantInternal(mutable, (Decimal) amount);
    card1.IsEnchantmentPreview = true;
    mutable.ModifyCard();
    NPreviewCardHolder child2 = NPreviewCardHolder.Create(NCard.Create(card1), true, false);
    ((Node) this._after).AddChildSafely((Node) child2);
    child2.CardNode.UpdateVisuals(PileType.None, CardPreviewMode.Normal);
  }

  private void RemoveExistingCards()
  {
    foreach (Node child in ((Node) this._before).GetChildren(false))
      child.QueueFreeSafely();
    foreach (Node child in ((Node) this._after).GetChildren(false))
      child.QueueFreeSafely();
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(2)
    {
      new MethodInfo(NEnchantPreview.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NEnchantPreview.MethodName.RemoveExistingCards, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NEnchantPreview.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NEnchantPreview.MethodName.RemoveExistingCards) || ((NativeVariantPtrArgs) ref args).Count != 0)
      return base.InvokeGodotClassMethod(ref method, args, ref ret);
    this.RemoveExistingCards();
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NEnchantPreview.MethodName._Ready) || StringName.op_Equality(ref method, NEnchantPreview.MethodName.RemoveExistingCards) || base.HasGodotClassMethod(ref method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NEnchantPreview.PropertyName._before))
    {
      this._before = VariantUtils.ConvertTo<Control>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NEnchantPreview.PropertyName._after))
    {
      this._after = VariantUtils.ConvertTo<Control>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NEnchantPreview.PropertyName._arrows))
      return ((GodotObject) this).SetGodotClassPropertyValue(ref name, ref value);
    this._arrows = VariantUtils.ConvertTo<Control>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NEnchantPreview.PropertyName._before))
    {
      value = VariantUtils.CreateFrom<Control>(ref this._before);
      return true;
    }
    if (StringName.op_Equality(ref name, NEnchantPreview.PropertyName._after))
    {
      value = VariantUtils.CreateFrom<Control>(ref this._after);
      return true;
    }
    if (!StringName.op_Equality(ref name, NEnchantPreview.PropertyName._arrows))
      return ((GodotObject) this).GetGodotClassPropertyValue(ref name, ref value);
    value = VariantUtils.CreateFrom<Control>(ref this._arrows);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NEnchantPreview.PropertyName._before, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NEnchantPreview.PropertyName._after, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NEnchantPreview.PropertyName._arrows, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).SaveGodotObjectData(info);
    info.AddProperty(NEnchantPreview.PropertyName._before, Variant.From<Control>(ref this._before));
    info.AddProperty(NEnchantPreview.PropertyName._after, Variant.From<Control>(ref this._after));
    info.AddProperty(NEnchantPreview.PropertyName._arrows, Variant.From<Control>(ref this._arrows));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NEnchantPreview.PropertyName._before, ref variant1))
      this._before = ((Variant) ref variant1).As<Control>();
    Variant variant2;
    if (info.TryGetProperty(NEnchantPreview.PropertyName._after, ref variant2))
      this._after = ((Variant) ref variant2).As<Control>();
    Variant variant3;
    if (!info.TryGetProperty(NEnchantPreview.PropertyName._arrows, ref variant3))
      return;
    this._arrows = ((Variant) ref variant3).As<Control>();
  }

  public class MethodName : Control.MethodName
  {
    public static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName RemoveExistingCards = StringName.op_Implicit(nameof (RemoveExistingCards));
  }

  public class PropertyName : Control.PropertyName
  {
    public static readonly StringName _before = StringName.op_Implicit(nameof (_before));
    public static readonly StringName _after = StringName.op_Implicit(nameof (_after));
    public static readonly StringName _arrows = StringName.op_Implicit(nameof (_arrows));
  }

  public class SignalName : Control.SignalName
  {
  }
}
