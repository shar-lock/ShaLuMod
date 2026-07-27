// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Screens.RunHistoryScreen.NDeckHistory
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Saves;
using MegaCrit.Sts2.Core.Saves.Runs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Screens.RunHistoryScreen;

[ScriptPath("res://src/Core/Nodes/Screens/RunHistoryScreen/NDeckHistory.cs")]
public class NDeckHistory : VBoxContainer
{
  private readonly LocString _deckHeader = new LocString("run_history", "DECK_HISTORY.header");
  private readonly LocString _cardCategories = new LocString("run_history", "DECK_HISTORY.categories");
  private MegaRichTextLabel _headerLabel;
  private Control _cardContainer;
  private readonly List<CardModel> _allCards = new List<CardModel>();
  private 
  #nullable disable
  NDeckHistory.HoveredEventHandler backing_Hovered;
  private NDeckHistory.UnhoveredEventHandler backing_Unhovered;

  public override void _Ready()
  {
    this._headerLabel = ((Node) this).GetNode<MegaRichTextLabel>(NodePath.op_Implicit("Header"));
    this._cardContainer = ((Node) this).GetNode<Control>(NodePath.op_Implicit("%CardContainer"));
  }

  public void LoadDeck(
  #nullable enable
  Player player, IEnumerable<SerializableCard> cards)
  {
    StringBuilder stringBuilder1 = new StringBuilder();
    Dictionary<CardRarity, int> dictionary1 = new Dictionary<CardRarity, int>();
    foreach (CardRarity key in Enum.GetValues<CardRarity>())
      dictionary1.Add(key, 0);
    List<SerializableCard> list = cards.ToList<SerializableCard>();
    CardRarity rarity;
    int num;
    foreach (SerializableCard serializableCard in list)
    {
      CardModel cardModel = SaveUtil.CardOrDeprecated(serializableCard.Id);
      Dictionary<CardRarity, int> dictionary2 = dictionary1;
      rarity = cardModel.Rarity;
      num = dictionary2[rarity]++;
    }
    this._deckHeader.Add("totalCards", (Decimal) list.Count);
    foreach (KeyValuePair<CardRarity, int> keyValuePair in dictionary1)
    {
      keyValuePair.Deconstruct(ref rarity, ref num);
      CardRarity cardRarity = rarity;
      int variable = num;
      this._cardCategories.Add(cardRarity.ToString() + "Cards", (Decimal) variable);
    }
    StringBuilder stringBuilder2 = stringBuilder1;
    StringBuilder stringBuilder3 = stringBuilder2;
    StringBuilder.AppendInterpolatedStringHandler interpolatedStringHandler;
    // ISSUE: explicit constructor call
    ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).\u002Ector(20, 1, stringBuilder2);
    ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendLiteral("[gold][b]");
    ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendFormatted(this._deckHeader.GetFormattedText());
    ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendLiteral("[/b][/gold]");
    ref StringBuilder.AppendInterpolatedStringHandler local = ref interpolatedStringHandler;
    stringBuilder3.Append(ref local);
    stringBuilder1.Append(this._cardCategories.GetFormattedText().Trim(','));
    this._headerLabel.Text = stringBuilder1.ToString();
    this.PopulateCards(player, (IEnumerable<SerializableCard>) list);
  }

  private void PopulateCards(Player player, IEnumerable<SerializableCard> cards)
  {
    foreach (Node child in ((Node) this._cardContainer).GetChildren(false))
      child.QueueFreeSafely();
    this._allCards.Clear();
    foreach (IGrouping<SerializableCard, SerializableCard> source in cards.GroupBy<SerializableCard, SerializableCard>((Func<SerializableCard, SerializableCard>) (x => x)))
    {
      CardModel card = CardModel.FromSerializable(source.Key);
      card.Owner = player;
      this._allCards.Add(card);
      NDeckHistoryEntry entry = NDeckHistoryEntry.Create(card, source.Count<SerializableCard>(), source.Where<SerializableCard>((Func<SerializableCard, bool>) (c => c.FloorAddedToDeck.HasValue)).Select<SerializableCard, int>((Func<SerializableCard, int>) (c => c.FloorAddedToDeck.Value)));
      ((GodotObject) entry).Connect(NDeckHistoryEntry.SignalName.Clicked, Callable.From<NDeckHistoryEntry>(new Action<NDeckHistoryEntry>(this.ShowEntry)), 0U);
      ((GodotObject) entry).Connect(NClickableControl.SignalName.Focused, Callable.From<NClickableControl>((Action<NClickableControl>) (_ => ((GodotObject) this).EmitSignal(NDeckHistory.SignalName.Hovered, new Variant[1]
      {
        Variant.op_Implicit((GodotObject) entry)
      }))), 0U);
      ((GodotObject) entry).Connect(NClickableControl.SignalName.Unfocused, Callable.From<NClickableControl>((Action<NClickableControl>) (_ => ((GodotObject) this).EmitSignal(NDeckHistory.SignalName.Unhovered, new Variant[1]
      {
        Variant.op_Implicit((GodotObject) entry)
      }))), 0U);
      ((Node) this._cardContainer).AddChildSafely((Node) entry);
    }
  }

  private void ShowEntry(NDeckHistoryEntry entry)
  {
    NGame.Instance.GetInspectCardScreen().Open(this._allCards, this._allCards.IndexOf(entry.Card));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(2)
    {
      new MethodInfo(NDeckHistory.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NDeckHistory.MethodName.ShowEntry, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("entry"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false)
      }, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NDeckHistory.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NDeckHistory.MethodName.ShowEntry) || ((NativeVariantPtrArgs) ref args).Count != 1)
      return base.InvokeGodotClassMethod(ref method, args, ref ret);
    this.ShowEntry(VariantUtils.ConvertTo<NDeckHistoryEntry>(ref ((NativeVariantPtrArgs) ref args)[0]));
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NDeckHistory.MethodName._Ready) || StringName.op_Equality(ref method, NDeckHistory.MethodName.ShowEntry) || base.HasGodotClassMethod(ref method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NDeckHistory.PropertyName._headerLabel))
    {
      this._headerLabel = VariantUtils.ConvertTo<MegaRichTextLabel>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NDeckHistory.PropertyName._cardContainer))
      return ((GodotObject) this).SetGodotClassPropertyValue(ref name, ref value);
    this._cardContainer = VariantUtils.ConvertTo<Control>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NDeckHistory.PropertyName._headerLabel))
    {
      value = VariantUtils.CreateFrom<MegaRichTextLabel>(ref this._headerLabel);
      return true;
    }
    if (!StringName.op_Equality(ref name, NDeckHistory.PropertyName._cardContainer))
      return ((GodotObject) this).GetGodotClassPropertyValue(ref name, ref value);
    value = VariantUtils.CreateFrom<Control>(ref this._cardContainer);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NDeckHistory.PropertyName._headerLabel, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NDeckHistory.PropertyName._cardContainer, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).SaveGodotObjectData(info);
    info.AddProperty(NDeckHistory.PropertyName._headerLabel, Variant.From<MegaRichTextLabel>(ref this._headerLabel));
    info.AddProperty(NDeckHistory.PropertyName._cardContainer, Variant.From<Control>(ref this._cardContainer));
    info.AddSignalEventDelegate(NDeckHistory.SignalName.Hovered, (Delegate) this.backing_Hovered);
    info.AddSignalEventDelegate(NDeckHistory.SignalName.Unhovered, (Delegate) this.backing_Unhovered);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NDeckHistory.PropertyName._headerLabel, ref variant1))
      this._headerLabel = ((Variant) ref variant1).As<MegaRichTextLabel>();
    Variant variant2;
    if (info.TryGetProperty(NDeckHistory.PropertyName._cardContainer, ref variant2))
      this._cardContainer = ((Variant) ref variant2).As<Control>();
    NDeckHistory.HoveredEventHandler hoveredEventHandler;
    if (info.TryGetSignalEventDelegate<NDeckHistory.HoveredEventHandler>(NDeckHistory.SignalName.Hovered, ref hoveredEventHandler))
      this.backing_Hovered = hoveredEventHandler;
    NDeckHistory.UnhoveredEventHandler unhoveredEventHandler;
    if (!info.TryGetSignalEventDelegate<NDeckHistory.UnhoveredEventHandler>(NDeckHistory.SignalName.Unhovered, ref unhoveredEventHandler))
      return;
    this.backing_Unhovered = unhoveredEventHandler;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<MethodInfo> GetGodotSignalList()
  {
    return new List<MethodInfo>(2)
    {
      new MethodInfo(NDeckHistory.SignalName.Hovered, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("deckHistoryEntry"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false)
      }, (List<Variant>) null),
      new MethodInfo(NDeckHistory.SignalName.Unhovered, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("deckHistoryEntry"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false)
      }, (List<Variant>) null)
    };
  }

  public event NDeckHistory.HoveredEventHandler Hovered
  {
    add => this.backing_Hovered += value;
    remove => this.backing_Hovered -= value;
  }

  protected void EmitSignalHovered(NDeckHistoryEntry deckHistoryEntry)
  {
    ((GodotObject) this).EmitSignal(NDeckHistory.SignalName.Hovered, new Variant[1]
    {
      Variant.op_Implicit((GodotObject) deckHistoryEntry)
    });
  }

  public event NDeckHistory.UnhoveredEventHandler Unhovered
  {
    add => this.backing_Unhovered += value;
    remove => this.backing_Unhovered -= value;
  }

  protected void EmitSignalUnhovered(NDeckHistoryEntry deckHistoryEntry)
  {
    ((GodotObject) this).EmitSignal(NDeckHistory.SignalName.Unhovered, new Variant[1]
    {
      Variant.op_Implicit((GodotObject) deckHistoryEntry)
    });
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RaiseGodotClassSignalCallbacks(
    in godot_string_name signal,
    NativeVariantPtrArgs args)
  {
    if (StringName.op_Equality(ref signal, NDeckHistory.SignalName.Hovered) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      NDeckHistory.HoveredEventHandler backingHovered = this.backing_Hovered;
      if (backingHovered == null)
        return;
      backingHovered(VariantUtils.ConvertTo<NDeckHistoryEntry>(ref ((NativeVariantPtrArgs) ref args)[0]));
    }
    else if (StringName.op_Equality(ref signal, NDeckHistory.SignalName.Unhovered) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      NDeckHistory.UnhoveredEventHandler backingUnhovered = this.backing_Unhovered;
      if (backingUnhovered == null)
        return;
      backingUnhovered(VariantUtils.ConvertTo<NDeckHistoryEntry>(ref ((NativeVariantPtrArgs) ref args)[0]));
    }
    else
      ((GodotObject) this).RaiseGodotClassSignalCallbacks(ref signal, args);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassSignal(in godot_string_name signal)
  {
    return StringName.op_Equality(ref signal, NDeckHistory.SignalName.Hovered) || StringName.op_Equality(ref signal, NDeckHistory.SignalName.Unhovered) || base.HasGodotClassSignal(ref signal);
  }

  [Signal]
  public delegate void HoveredEventHandler(
  #nullable enable
  NDeckHistoryEntry deckHistoryEntry);

  [Signal]
  public delegate void UnhoveredEventHandler(NDeckHistoryEntry deckHistoryEntry);

  public class MethodName : VBoxContainer.MethodName
  {
    public static readonly 
    #nullable disable
    StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName ShowEntry = StringName.op_Implicit(nameof (ShowEntry));
  }

  public class PropertyName : VBoxContainer.PropertyName
  {
    public static readonly StringName _headerLabel = StringName.op_Implicit(nameof (_headerLabel));
    public static readonly StringName _cardContainer = StringName.op_Implicit(nameof (_cardContainer));
  }

  public class SignalName : VBoxContainer.SignalName
  {
    public static readonly StringName Hovered = StringName.op_Implicit(nameof (Hovered));
    public static readonly StringName Unhovered = StringName.op_Implicit(nameof (Unhovered));
  }
}
