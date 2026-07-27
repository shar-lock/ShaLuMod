// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Cards.NTransformPreview
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.UI;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Cards.Holders;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Random;
using MegaCrit.Sts2.Core.Saves;
using MegaCrit.Sts2.Core.Settings;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Cards;

[ScriptPath("res://src/Core/Nodes/Cards/NTransformPreview.cs")]
public class NTransformPreview : Control
{
  private Control _before;
  private Control _after;
  private Control _arrows;
  private CancellationTokenSource? _cancelTokenSource;

  public Vector2 SelectedCardPosition => this._before.GlobalPosition;

  public override void _Ready()
  {
    this._before = ((Node) this).GetNode<Control>(NodePath.op_Implicit("%Before"));
    this._after = ((Node) this).GetNode<Control>(NodePath.op_Implicit("%After"));
    this._arrows = ((Node) this).GetNode<Control>(NodePath.op_Implicit("Arrows"));
  }

  public override void _ExitTree() => this._cancelTokenSource?.Cancel();

  public void Initialize(
    IEnumerable<CardTransformation> cardTransformations)
  {
    this.RemoveExistingCards();
    this._cancelTokenSource?.Cancel();
    List<CardTransformation> list = cardTransformations.ToList<CardTransformation>();
    float num1 = Math.Min((this._before.GlobalPosition.X - 100f) / (float) ((double) list.Count * 300.0 + (double) (list.Count - 1) * 30.0), 1f);
    for (int index = 0; index < list.Count; ++index)
    {
      CardTransformation cardTransformation = list[index];
      NPlayerHand hand = NCombatRoom.Instance?.Ui.Hand;
      NPreviewCardHolder child = NPreviewCardHolder.Create(NCard.Create(cardTransformation.Original), hand == null, hand != null);
      ((Node) this._before).AddChildSafely((Node) child);
      child.FocusMode = (Control.FocusModeEnum) 2L;
      child.CardNode.UpdateVisuals(cardTransformation.Original.Pile.Type, CardPreviewMode.Normal);
      child.SetCardScale(Vector2.op_Multiply(Vector2.One, num1));
      int num2 = list.Count - index;
      child.Position = new Vector2((float) (-((double) num2 - 0.5) * 300.0 * (double) num1 - (double) (num2 - 1) * 30.0), 0.0f);
      NPreviewCardHolder npreviewCardHolder = NPreviewCardHolder.Create(cardTransformation.Replacement == null ? NCard.Create(cardTransformation.Original) : NCard.Create(cardTransformation.Replacement), true, false);
      npreviewCardHolder.FocusMode = (Control.FocusModeEnum) 0L;
      ((Node) this._after).AddChildSafely((Node) npreviewCardHolder);
      npreviewCardHolder.CardNode.UpdateVisuals(cardTransformation.Original.Pile.Type, CardPreviewMode.Normal);
      npreviewCardHolder.Scale = Vector2.op_Multiply(Vector2.One, num1);
      npreviewCardHolder.Position = new Vector2((float) (((double) index + 0.5) * 300.0 * (double) num1 + (double) index * 30.0), 0.0f);
      if (cardTransformation.Replacement == null)
      {
        npreviewCardHolder.Hitbox.MouseFilter = (Control.MouseFilterEnum) 2L;
        IEnumerable<CardModel> possibleTransformations = cardTransformation.ReplacementOptions == null ? CardFactory.GetDefaultTransformationOptions(cardTransformation.Original, cardTransformation.IsInCombat) : cardTransformation.ReplacementOptions;
        TaskHelper.RunSafely(this.CycleThroughCards(npreviewCardHolder, cardTransformation.Original.Pile, possibleTransformations));
      }
    }
  }

  public void Uninitialize() => this._cancelTokenSource?.Cancel();

  private async Task CycleThroughCards(
    NPreviewCardHolder holder,
    CardPile cardPile,
    IEnumerable<CardModel> possibleTransformations)
  {
    this._cancelTokenSource = new CancellationTokenSource();
    List<CardModel> cards = possibleTransformations.ToList<CardModel>();
    cards.UnstableShuffle<CardModel>(Rng.Chaotic);
    int cardIndex = 0;
    while (!this._cancelTokenSource.IsCancellationRequested)
    {
      if (holder.CardNode == null)
      {
        cards = (List<CardModel>) null;
        return;
      }
      holder.ReassignToCard(cards[cardIndex], cardPile.Type, (Creature) null, ModelVisibility.Visible);
      ++cardIndex;
      if (cardIndex >= cards.Count)
      {
        cards.UnstableShuffle<CardModel>(Rng.Chaotic);
        cardIndex = 0;
      }
      if (SaveManager.Instance.PrefsSave.FastMode == FastModeType.Instant)
        await Task.Delay(200, this._cancelTokenSource.Token);
      else
        await Cmd.Wait(0.2f, this._cancelTokenSource.Token, true);
    }
    cards = (List<CardModel>) null;
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
    return new List<MethodInfo>(4)
    {
      new MethodInfo(NTransformPreview.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NTransformPreview.MethodName._ExitTree, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NTransformPreview.MethodName.Uninitialize, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NTransformPreview.MethodName.RemoveExistingCards, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NTransformPreview.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NTransformPreview.MethodName._ExitTree) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._ExitTree();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NTransformPreview.MethodName.Uninitialize) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.Uninitialize();
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NTransformPreview.MethodName.RemoveExistingCards) || ((NativeVariantPtrArgs) ref args).Count != 0)
      return base.InvokeGodotClassMethod(ref method, args, ref ret);
    this.RemoveExistingCards();
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NTransformPreview.MethodName._Ready) || StringName.op_Equality(ref method, NTransformPreview.MethodName._ExitTree) || StringName.op_Equality(ref method, NTransformPreview.MethodName.Uninitialize) || StringName.op_Equality(ref method, NTransformPreview.MethodName.RemoveExistingCards) || base.HasGodotClassMethod(ref method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NTransformPreview.PropertyName._before))
    {
      this._before = VariantUtils.ConvertTo<Control>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NTransformPreview.PropertyName._after))
    {
      this._after = VariantUtils.ConvertTo<Control>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NTransformPreview.PropertyName._arrows))
      return ((GodotObject) this).SetGodotClassPropertyValue(ref name, ref value);
    this._arrows = VariantUtils.ConvertTo<Control>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NTransformPreview.PropertyName.SelectedCardPosition))
    {
      ref godot_variant local = ref value;
      Vector2 selectedCardPosition = this.SelectedCardPosition;
      godot_variant from = VariantUtils.CreateFrom<Vector2>(ref selectedCardPosition);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NTransformPreview.PropertyName._before))
    {
      value = VariantUtils.CreateFrom<Control>(ref this._before);
      return true;
    }
    if (StringName.op_Equality(ref name, NTransformPreview.PropertyName._after))
    {
      value = VariantUtils.CreateFrom<Control>(ref this._after);
      return true;
    }
    if (!StringName.op_Equality(ref name, NTransformPreview.PropertyName._arrows))
      return ((GodotObject) this).GetGodotClassPropertyValue(ref name, ref value);
    value = VariantUtils.CreateFrom<Control>(ref this._arrows);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NTransformPreview.PropertyName._before, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NTransformPreview.PropertyName._after, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NTransformPreview.PropertyName._arrows, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 5L, NTransformPreview.PropertyName.SelectedCardPosition, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).SaveGodotObjectData(info);
    info.AddProperty(NTransformPreview.PropertyName._before, Variant.From<Control>(ref this._before));
    info.AddProperty(NTransformPreview.PropertyName._after, Variant.From<Control>(ref this._after));
    info.AddProperty(NTransformPreview.PropertyName._arrows, Variant.From<Control>(ref this._arrows));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NTransformPreview.PropertyName._before, ref variant1))
      this._before = ((Variant) ref variant1).As<Control>();
    Variant variant2;
    if (info.TryGetProperty(NTransformPreview.PropertyName._after, ref variant2))
      this._after = ((Variant) ref variant2).As<Control>();
    Variant variant3;
    if (!info.TryGetProperty(NTransformPreview.PropertyName._arrows, ref variant3))
      return;
    this._arrows = ((Variant) ref variant3).As<Control>();
  }

  public class MethodName : Control.MethodName
  {
    public static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName _ExitTree = StringName.op_Implicit(nameof (_ExitTree));
    public static readonly StringName Uninitialize = StringName.op_Implicit(nameof (Uninitialize));
    public static readonly StringName RemoveExistingCards = StringName.op_Implicit(nameof (RemoveExistingCards));
  }

  public class PropertyName : Control.PropertyName
  {
    public static readonly StringName SelectedCardPosition = StringName.op_Implicit(nameof (SelectedCardPosition));
    public static readonly StringName _before = StringName.op_Implicit(nameof (_before));
    public static readonly StringName _after = StringName.op_Implicit(nameof (_after));
    public static readonly StringName _arrows = StringName.op_Implicit(nameof (_arrows));
  }

  public class SignalName : Control.SignalName
  {
  }
}
