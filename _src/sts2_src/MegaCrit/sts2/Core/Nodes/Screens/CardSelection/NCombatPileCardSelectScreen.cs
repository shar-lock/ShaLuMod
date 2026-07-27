// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Screens.CardSelection.NCombatPileCardSelectScreen
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Cards.Holders;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.Screens.CardLibrary;
using MegaCrit.Sts2.Core.Nodes.Screens.Overlays;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Screens.CardSelection;

[ScriptPath("res://src/Core/Nodes/Screens/CardSelection/NCombatPileCardSelectScreen.cs")]
public sealed class NCombatPileCardSelectScreen : NCardGridSelectionScreen
{
  private Control _bottomTextContainer;
  private MegaRichTextLabel _infoLabel;
  private NConfirmButton _confirmButton;
  private NCombatPilesContainer _combatPiles;
  private CancellationTokenSource _cts = new CancellationTokenSource();
  private HashSet<CardModel> _selectedCards = new HashSet<CardModel>();
  private CardSelectorPrefs _prefs;
  private CardPile _pile;
  private Func<CardModel, bool>? _filter;
  private List<CardCreationResult>? _cardResults;
  private bool _isSubscribedToPile;

  private static string ScenePath
  {
    get => SceneHelper.GetScenePath("screens/card_selection/combat_pile_card_select_screen");
  }

  public static IEnumerable<string> AssetPaths
  {
    get
    {
      return (IEnumerable<string>) new \u003C\u003Ez__ReadOnlySingleElementList<string>(NCombatPileCardSelectScreen.ScenePath);
    }
  }

  public static NCombatPileCardSelectScreen Create(
    CardPile pile,
    CardSelectorPrefs prefs,
    Func<CardModel, bool>? filter)
  {
    NCombatPileCardSelectScreen cardSelectScreen = PreloadManager.Cache.GetScene(NCombatPileCardSelectScreen.ScenePath).Instantiate<NCombatPileCardSelectScreen>((PackedScene.GenEditState) 0L);
    ((Node) cardSelectScreen).Name = StringName.op_Implicit(nameof (NCombatPileCardSelectScreen));
    cardSelectScreen._cardResults = (List<CardCreationResult>) null;
    cardSelectScreen._prefs = prefs;
    cardSelectScreen._cards = (IReadOnlyList<CardModel>) Array.Empty<CardModel>();
    cardSelectScreen._pile = pile;
    cardSelectScreen._filter = filter;
    return cardSelectScreen;
  }

  public override void _Ready()
  {
    this.ConnectSignalsAndInitGrid();
    this._confirmButton = ((Node) this).GetNode<NConfirmButton>(NodePath.op_Implicit("%Confirm"));
    this._bottomTextContainer = ((Node) this).GetNode<Control>(NodePath.op_Implicit("%BottomText"));
    this._infoLabel = ((Node) this._bottomTextContainer).GetNode<MegaRichTextLabel>(NodePath.op_Implicit("%BottomLabel"));
    this._infoLabel.Text = this._prefs.Prompt.GetFormattedText();
    this.UpdatePileContents();
    if (this._prefs.MinSelect == 0)
      this._confirmButton.Enable();
    else
      this._confirmButton.Disable();
    ((GodotObject) this._confirmButton).Connect(NClickableControl.SignalName.Released, Callable.From<NButton>((Action<NButton>) (_ => this.CompleteSelection())), 0U);
  }

  public override void _EnterTree()
  {
    this._cts = new CancellationTokenSource();
    this._pile.ContentsChanged += new Action(this.UpdatePileContents);
    this._isSubscribedToPile = true;
  }

  public override void _ExitTree()
  {
    base._ExitTree();
    this._cts.Cancel();
    this.UnsubscribeFromPile();
  }

  protected override void ConnectSignalsAndInitGrid()
  {
    base.ConnectSignalsAndInitGrid();
    this._combatPiles = ((Node) this).GetNode<NCombatPilesContainer>(NodePath.op_Implicit("%CombatPiles"));
    if (CombatManager.Instance.IsInProgress)
      this._combatPiles.Initialize(this._pile.Cards.First<CardModel>().Owner);
    this._combatPiles.Disable();
    ((CanvasItem) this._combatPiles).SetVisible(false);
    ((GodotObject) this._peekButton).Connect(NPeekButton.SignalName.Toggled, Callable.From<NPeekButton>((Action<NPeekButton>) (_ =>
    {
      if (this._peekButton.IsPeeking)
      {
        this._combatPiles.Enable();
        ((CanvasItem) this._combatPiles).SetVisible(true);
      }
      else
      {
        this._combatPiles.Disable();
        ((CanvasItem) this._combatPiles).SetVisible(false);
      }
    })), 0U);
  }

  protected override IEnumerable<Control> PeekButtonTargets
  {
    get
    {
      return (IEnumerable<Control>) new \u003C\u003Ez__ReadOnlySingleElementList<Control>(this._bottomTextContainer);
    }
  }

  public override void AfterOverlayOpened()
  {
    base.AfterOverlayOpened();
    TaskHelper.RunSafely(this.FlashRelicsOnModifiedCards());
  }

  private async Task FlashRelicsOnModifiedCards()
  {
    if (this._cardResults == null)
      return;
    double num = (double) await ((Node) this).AwaitProcessFrame(this._cts.Token);
    foreach (CardCreationResult cardResult in this._cardResults)
    {
      CardCreationResult result = cardResult;
      NGridCardHolder ngridCardHolder = this._grid.CurrentlyDisplayedCardHolders.FirstOrDefault<NGridCardHolder>((Func<NGridCardHolder, bool>) (h => h.CardModel == result.Card));
      if (ngridCardHolder != null && result.HasBeenModified)
      {
        foreach (RelicModel modifyingRelic in result.ModifyingRelics)
        {
          modifyingRelic.Flash();
          ngridCardHolder.CardNode?.FlashRelicOnCard(modifyingRelic);
        }
      }
    }
  }

  protected override void OnCardClicked(CardModel card)
  {
    if (this._selectedCards.Contains(card))
    {
      this._grid.UnhighlightCard(card);
      this._selectedCards.Remove(card);
    }
    else
    {
      if (this._selectedCards.Count < this._prefs.MaxSelect)
      {
        this._grid.HighlightCard(card);
        this._selectedCards.Add(card);
      }
      if (!this._prefs.RequireManualConfirmation)
        this.CheckIfSelectionComplete();
    }
    this.UpdateConfirmButton();
  }

  private void UpdateConfirmButton()
  {
    if (this._selectedCards.Count >= Mathf.Min(this._prefs.MinSelect, this._grid.CurrentlyDisplayedCards.Count<CardModel>()) && this._prefs.RequireManualConfirmation)
      this._confirmButton.Enable();
    else
      this._confirmButton.Disable();
  }

  private void CheckIfSelectionComplete()
  {
    if (this._selectedCards.Count < Mathf.Min(this._prefs.MaxSelect, this._grid.CurrentlyDisplayedCards.Count<CardModel>()))
      return;
    this.CompleteSelection();
  }

  private void CompleteSelection()
  {
    this.UnsubscribeFromPile();
    this._completionSource.SetResult((IEnumerable<CardModel>) this._selectedCards);
    NOverlayStack.Instance.Remove((IOverlayScreen) this);
  }

  private void UnsubscribeFromPile()
  {
    if (!this._isSubscribedToPile)
      return;
    this._pile.ContentsChanged -= new Action(this.UpdatePileContents);
    this._isSubscribedToPile = false;
  }

  private void UpdatePileContents()
  {
    IReadOnlyList<CardModel> validPileCards = this._filter != null ? (IReadOnlyList<CardModel>) this._pile.Cards.Where<CardModel>(this._filter).ToList<CardModel>() : this._pile.Cards;
    this._selectedCards = this._selectedCards.Where<CardModel>((Func<CardModel, bool>) (c => validPileCards.Contains<CardModel>(c))).ToHashSet<CardModel>();
    if (validPileCards.Count == 0)
    {
      this._selectedCards.Clear();
      this.CompleteSelection();
    }
    else if (validPileCards.Count == this._selectedCards.Count)
    {
      this.CompleteSelection();
    }
    else
    {
      List<SortingOrders> sortingOrdersList1;
      if (this._pile.Type != PileType.Draw)
      {
        int capacity = 1;
        List<SortingOrders> sortingOrdersList2 = new List<SortingOrders>(capacity);
        CollectionsMarshal.SetCount<SortingOrders>(sortingOrdersList2, capacity);
        CollectionsMarshal.AsSpan<SortingOrders>(sortingOrdersList2)[0] = SortingOrders.Ascending;
        sortingOrdersList1 = sortingOrdersList2;
      }
      else
      {
        int capacity = 2;
        sortingOrdersList1 = new List<SortingOrders>(capacity);
        CollectionsMarshal.SetCount<SortingOrders>(sortingOrdersList1, capacity);
        Span<SortingOrders> span = CollectionsMarshal.AsSpan<SortingOrders>(sortingOrdersList1);
        int num1 = 0;
        span[num1] = SortingOrders.RarityAscending;
        int num2 = num1 + 1;
        span[num2] = SortingOrders.AlphabetAscending;
      }
      List<SortingOrders> sortingPriority = sortingOrdersList1;
      this._grid.SetCards(validPileCards, this._pile.Type, sortingPriority);
      this.UpdateConfirmButton();
      foreach (CardModel selectedCard in this._selectedCards)
        this._grid.HighlightCard(selectedCard);
    }
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(10)
    {
      new MethodInfo(NCombatPileCardSelectScreen.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCombatPileCardSelectScreen.MethodName._EnterTree, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCombatPileCardSelectScreen.MethodName._ExitTree, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCombatPileCardSelectScreen.MethodName.ConnectSignalsAndInitGrid, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCombatPileCardSelectScreen.MethodName.AfterOverlayOpened, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCombatPileCardSelectScreen.MethodName.UpdateConfirmButton, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCombatPileCardSelectScreen.MethodName.CheckIfSelectionComplete, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCombatPileCardSelectScreen.MethodName.CompleteSelection, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCombatPileCardSelectScreen.MethodName.UnsubscribeFromPile, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCombatPileCardSelectScreen.MethodName.UpdatePileContents, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NCombatPileCardSelectScreen.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCombatPileCardSelectScreen.MethodName._EnterTree) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._EnterTree();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCombatPileCardSelectScreen.MethodName._ExitTree) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._ExitTree();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCombatPileCardSelectScreen.MethodName.ConnectSignalsAndInitGrid) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.ConnectSignalsAndInitGrid();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCombatPileCardSelectScreen.MethodName.AfterOverlayOpened) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.AfterOverlayOpened();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCombatPileCardSelectScreen.MethodName.UpdateConfirmButton) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.UpdateConfirmButton();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCombatPileCardSelectScreen.MethodName.CheckIfSelectionComplete) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.CheckIfSelectionComplete();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCombatPileCardSelectScreen.MethodName.CompleteSelection) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.CompleteSelection();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCombatPileCardSelectScreen.MethodName.UnsubscribeFromPile) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.UnsubscribeFromPile();
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NCombatPileCardSelectScreen.MethodName.UpdatePileContents) || ((NativeVariantPtrArgs) ref args).Count != 0)
      return base.InvokeGodotClassMethod(in method, args, out ret);
    this.UpdatePileContents();
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NCombatPileCardSelectScreen.MethodName._Ready) || StringName.op_Equality(ref method, NCombatPileCardSelectScreen.MethodName._EnterTree) || StringName.op_Equality(ref method, NCombatPileCardSelectScreen.MethodName._ExitTree) || StringName.op_Equality(ref method, NCombatPileCardSelectScreen.MethodName.ConnectSignalsAndInitGrid) || StringName.op_Equality(ref method, NCombatPileCardSelectScreen.MethodName.AfterOverlayOpened) || StringName.op_Equality(ref method, NCombatPileCardSelectScreen.MethodName.UpdateConfirmButton) || StringName.op_Equality(ref method, NCombatPileCardSelectScreen.MethodName.CheckIfSelectionComplete) || StringName.op_Equality(ref method, NCombatPileCardSelectScreen.MethodName.CompleteSelection) || StringName.op_Equality(ref method, NCombatPileCardSelectScreen.MethodName.UnsubscribeFromPile) || StringName.op_Equality(ref method, NCombatPileCardSelectScreen.MethodName.UpdatePileContents) || base.HasGodotClassMethod(in method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NCombatPileCardSelectScreen.PropertyName._bottomTextContainer))
    {
      this._bottomTextContainer = VariantUtils.ConvertTo<Control>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCombatPileCardSelectScreen.PropertyName._infoLabel))
    {
      this._infoLabel = VariantUtils.ConvertTo<MegaRichTextLabel>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCombatPileCardSelectScreen.PropertyName._confirmButton))
    {
      this._confirmButton = VariantUtils.ConvertTo<NConfirmButton>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCombatPileCardSelectScreen.PropertyName._combatPiles))
    {
      this._combatPiles = VariantUtils.ConvertTo<NCombatPilesContainer>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NCombatPileCardSelectScreen.PropertyName._isSubscribedToPile))
      return base.SetGodotClassPropertyValue(in name, in value);
    this._isSubscribedToPile = VariantUtils.ConvertTo<bool>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NCombatPileCardSelectScreen.PropertyName._bottomTextContainer))
    {
      value = VariantUtils.CreateFrom<Control>(ref this._bottomTextContainer);
      return true;
    }
    if (StringName.op_Equality(ref name, NCombatPileCardSelectScreen.PropertyName._infoLabel))
    {
      value = VariantUtils.CreateFrom<MegaRichTextLabel>(ref this._infoLabel);
      return true;
    }
    if (StringName.op_Equality(ref name, NCombatPileCardSelectScreen.PropertyName._confirmButton))
    {
      value = VariantUtils.CreateFrom<NConfirmButton>(ref this._confirmButton);
      return true;
    }
    if (StringName.op_Equality(ref name, NCombatPileCardSelectScreen.PropertyName._combatPiles))
    {
      value = VariantUtils.CreateFrom<NCombatPilesContainer>(ref this._combatPiles);
      return true;
    }
    if (!StringName.op_Equality(ref name, NCombatPileCardSelectScreen.PropertyName._isSubscribedToPile))
      return base.GetGodotClassPropertyValue(in name, out value);
    value = VariantUtils.CreateFrom<bool>(ref this._isSubscribedToPile);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NCombatPileCardSelectScreen.PropertyName._bottomTextContainer, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NCombatPileCardSelectScreen.PropertyName._infoLabel, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NCombatPileCardSelectScreen.PropertyName._confirmButton, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NCombatPileCardSelectScreen.PropertyName._combatPiles, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 1L, NCombatPileCardSelectScreen.PropertyName._isSubscribedToPile, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    base.SaveGodotObjectData(info);
    info.AddProperty(NCombatPileCardSelectScreen.PropertyName._bottomTextContainer, Variant.From<Control>(ref this._bottomTextContainer));
    info.AddProperty(NCombatPileCardSelectScreen.PropertyName._infoLabel, Variant.From<MegaRichTextLabel>(ref this._infoLabel));
    info.AddProperty(NCombatPileCardSelectScreen.PropertyName._confirmButton, Variant.From<NConfirmButton>(ref this._confirmButton));
    info.AddProperty(NCombatPileCardSelectScreen.PropertyName._combatPiles, Variant.From<NCombatPilesContainer>(ref this._combatPiles));
    info.AddProperty(NCombatPileCardSelectScreen.PropertyName._isSubscribedToPile, Variant.From<bool>(ref this._isSubscribedToPile));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    base.RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NCombatPileCardSelectScreen.PropertyName._bottomTextContainer, ref variant1))
      this._bottomTextContainer = ((Variant) ref variant1).As<Control>();
    Variant variant2;
    if (info.TryGetProperty(NCombatPileCardSelectScreen.PropertyName._infoLabel, ref variant2))
      this._infoLabel = ((Variant) ref variant2).As<MegaRichTextLabel>();
    Variant variant3;
    if (info.TryGetProperty(NCombatPileCardSelectScreen.PropertyName._confirmButton, ref variant3))
      this._confirmButton = ((Variant) ref variant3).As<NConfirmButton>();
    Variant variant4;
    if (info.TryGetProperty(NCombatPileCardSelectScreen.PropertyName._combatPiles, ref variant4))
      this._combatPiles = ((Variant) ref variant4).As<NCombatPilesContainer>();
    Variant variant5;
    if (!info.TryGetProperty(NCombatPileCardSelectScreen.PropertyName._isSubscribedToPile, ref variant5))
      return;
    this._isSubscribedToPile = ((Variant) ref variant5).As<bool>();
  }

  public new class MethodName : NCardGridSelectionScreen.MethodName
  {
    public new static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName _EnterTree = StringName.op_Implicit(nameof (_EnterTree));
    public new static readonly StringName _ExitTree = StringName.op_Implicit(nameof (_ExitTree));
    public new static readonly StringName ConnectSignalsAndInitGrid = StringName.op_Implicit(nameof (ConnectSignalsAndInitGrid));
    public new static readonly StringName AfterOverlayOpened = StringName.op_Implicit(nameof (AfterOverlayOpened));
    public static readonly StringName UpdateConfirmButton = StringName.op_Implicit(nameof (UpdateConfirmButton));
    public static readonly StringName CheckIfSelectionComplete = StringName.op_Implicit(nameof (CheckIfSelectionComplete));
    public static readonly StringName CompleteSelection = StringName.op_Implicit(nameof (CompleteSelection));
    public static readonly StringName UnsubscribeFromPile = StringName.op_Implicit(nameof (UnsubscribeFromPile));
    public static readonly StringName UpdatePileContents = StringName.op_Implicit(nameof (UpdatePileContents));
  }

  public new class PropertyName : NCardGridSelectionScreen.PropertyName
  {
    public static readonly StringName _bottomTextContainer = StringName.op_Implicit(nameof (_bottomTextContainer));
    public static readonly StringName _infoLabel = StringName.op_Implicit(nameof (_infoLabel));
    public static readonly StringName _confirmButton = StringName.op_Implicit(nameof (_confirmButton));
    public static readonly StringName _combatPiles = StringName.op_Implicit(nameof (_combatPiles));
    public static readonly StringName _isSubscribedToPile = StringName.op_Implicit(nameof (_isSubscribedToPile));
  }

  public new class SignalName : NCardGridSelectionScreen.SignalName
  {
  }
}
