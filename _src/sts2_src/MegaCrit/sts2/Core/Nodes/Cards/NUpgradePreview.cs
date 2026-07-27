// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Cards.NUpgradePreview
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
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using System;
using System.Collections.Generic;
using System.ComponentModel;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Cards;

[ScriptPath("res://src/Core/Nodes/Cards/NUpgradePreview.cs")]
public class NUpgradePreview : Control
{
  private Control _before;
  private Control _after;
  private Control _arrows;
  private CardModel? _card;

  public Vector2 SelectedCardPosition => this._before.GlobalPosition;

  public CardModel? Card
  {
    get => this._card;
    set
    {
      this._card = value;
      this.Reload();
    }
  }

  public override void _Ready()
  {
    this._before = ((Node) this).GetNode<Control>(NodePath.op_Implicit("%Before"));
    this._after = ((Node) this).GetNode<Control>(NodePath.op_Implicit("%After"));
    this._arrows = ((Node) this).GetNode<Control>(NodePath.op_Implicit("Arrows"));
  }

  private void Reload()
  {
    this.RemoveExistingCards();
    ((CanvasItem) this._arrows).Visible = this.Card != null;
    if (this.Card == null)
      return;
    NPlayerHand hand = NCombatRoom.Instance?.Ui.Hand;
    NPreviewCardHolder child1 = NPreviewCardHolder.Create(NCard.Create(this.Card), hand == null, hand != null);
    ((Node) this._before).AddChildSafely((Node) child1);
    child1.FocusMode = (Control.FocusModeEnum) 2L;
    child1.CardNode.UpdateVisuals(this.Card.Pile.Type, CardPreviewMode.Normal);
    if (hand != null)
      ((GodotObject) child1).Connect(NCardHolder.SignalName.Pressed, Callable.From<NCardHolder>(new Action<NCardHolder>(this.ReturnCard)), 0U);
    CardModel card = this.Card.CardScope.CloneCard(this.Card);
    card.UpgradeInternal();
    card.UpgradePreviewType = this.Card.Pile.IsCombatPile ? CardUpgradePreviewType.Combat : CardUpgradePreviewType.Deck;
    NPreviewCardHolder child2 = NPreviewCardHolder.Create(NCard.Create(card), true, false);
    child2.FocusMode = (Control.FocusModeEnum) 0L;
    ((Node) this._after).AddChildSafely((Node) child2);
    child2.CardNode.ShowUpgradePreview();
  }

  private void RemoveExistingCards()
  {
    foreach (Node child in ((Node) this._before).GetChildren(false))
      child.QueueFreeSafely();
    foreach (Node child in ((Node) this._after).GetChildren(false))
      child.QueueFreeSafely();
  }

  private void ReturnCard(NCardHolder holder)
  {
    holder.Pressed -= new NCardHolder.PressedEventHandler(this.ReturnCard);
    NCombatRoom.Instance?.Ui.Hand.DeselectCard(holder.CardNode);
    this.Card = (CardModel) null;
  }

  public Control DefaultFocusedControl
  {
    get => (Control) ((Node) this._before).GetChild<NPreviewCardHolder>(0, false);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(4)
    {
      new MethodInfo(NUpgradePreview.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NUpgradePreview.MethodName.Reload, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NUpgradePreview.MethodName.RemoveExistingCards, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NUpgradePreview.MethodName.ReturnCard, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("holder"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false)
      }, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NUpgradePreview.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NUpgradePreview.MethodName.Reload) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.Reload();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NUpgradePreview.MethodName.RemoveExistingCards) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.RemoveExistingCards();
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NUpgradePreview.MethodName.ReturnCard) || ((NativeVariantPtrArgs) ref args).Count != 1)
      return base.InvokeGodotClassMethod(ref method, args, ref ret);
    this.ReturnCard(VariantUtils.ConvertTo<NCardHolder>(ref ((NativeVariantPtrArgs) ref args)[0]));
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NUpgradePreview.MethodName._Ready) || StringName.op_Equality(ref method, NUpgradePreview.MethodName.Reload) || StringName.op_Equality(ref method, NUpgradePreview.MethodName.RemoveExistingCards) || StringName.op_Equality(ref method, NUpgradePreview.MethodName.ReturnCard) || base.HasGodotClassMethod(ref method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NUpgradePreview.PropertyName._before))
    {
      this._before = VariantUtils.ConvertTo<Control>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NUpgradePreview.PropertyName._after))
    {
      this._after = VariantUtils.ConvertTo<Control>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NUpgradePreview.PropertyName._arrows))
      return ((GodotObject) this).SetGodotClassPropertyValue(ref name, ref value);
    this._arrows = VariantUtils.ConvertTo<Control>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NUpgradePreview.PropertyName.SelectedCardPosition))
    {
      ref godot_variant local = ref value;
      Vector2 selectedCardPosition = this.SelectedCardPosition;
      godot_variant from = VariantUtils.CreateFrom<Vector2>(ref selectedCardPosition);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NUpgradePreview.PropertyName.DefaultFocusedControl))
    {
      ref godot_variant local = ref value;
      Control defaultFocusedControl = this.DefaultFocusedControl;
      godot_variant from = VariantUtils.CreateFrom<Control>(ref defaultFocusedControl);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NUpgradePreview.PropertyName._before))
    {
      value = VariantUtils.CreateFrom<Control>(ref this._before);
      return true;
    }
    if (StringName.op_Equality(ref name, NUpgradePreview.PropertyName._after))
    {
      value = VariantUtils.CreateFrom<Control>(ref this._after);
      return true;
    }
    if (!StringName.op_Equality(ref name, NUpgradePreview.PropertyName._arrows))
      return ((GodotObject) this).GetGodotClassPropertyValue(ref name, ref value);
    value = VariantUtils.CreateFrom<Control>(ref this._arrows);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NUpgradePreview.PropertyName._before, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NUpgradePreview.PropertyName._after, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NUpgradePreview.PropertyName._arrows, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 5L, NUpgradePreview.PropertyName.SelectedCardPosition, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NUpgradePreview.PropertyName.DefaultFocusedControl, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).SaveGodotObjectData(info);
    info.AddProperty(NUpgradePreview.PropertyName._before, Variant.From<Control>(ref this._before));
    info.AddProperty(NUpgradePreview.PropertyName._after, Variant.From<Control>(ref this._after));
    info.AddProperty(NUpgradePreview.PropertyName._arrows, Variant.From<Control>(ref this._arrows));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NUpgradePreview.PropertyName._before, ref variant1))
      this._before = ((Variant) ref variant1).As<Control>();
    Variant variant2;
    if (info.TryGetProperty(NUpgradePreview.PropertyName._after, ref variant2))
      this._after = ((Variant) ref variant2).As<Control>();
    Variant variant3;
    if (!info.TryGetProperty(NUpgradePreview.PropertyName._arrows, ref variant3))
      return;
    this._arrows = ((Variant) ref variant3).As<Control>();
  }

  public class MethodName : Control.MethodName
  {
    public static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName Reload = StringName.op_Implicit(nameof (Reload));
    public static readonly StringName RemoveExistingCards = StringName.op_Implicit(nameof (RemoveExistingCards));
    public static readonly StringName ReturnCard = StringName.op_Implicit(nameof (ReturnCard));
  }

  public class PropertyName : Control.PropertyName
  {
    public static readonly StringName SelectedCardPosition = StringName.op_Implicit(nameof (SelectedCardPosition));
    public static readonly StringName DefaultFocusedControl = StringName.op_Implicit(nameof (DefaultFocusedControl));
    public static readonly StringName _before = StringName.op_Implicit(nameof (_before));
    public static readonly StringName _after = StringName.op_Implicit(nameof (_after));
    public static readonly StringName _arrows = StringName.op_Implicit(nameof (_arrows));
  }

  public class SignalName : Control.SignalName
  {
  }
}
