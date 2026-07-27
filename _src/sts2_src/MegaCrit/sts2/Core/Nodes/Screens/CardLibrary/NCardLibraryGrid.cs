// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Screens.CardLibrary.NCardLibraryGrid
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.UI;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Cards;
using MegaCrit.Sts2.Core.Nodes.Cards.Holders;
using MegaCrit.Sts2.Core.Saves;
using MegaCrit.Sts2.Core.Unlocks;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Screens.CardLibrary;

[ScriptPath("res://src/Core/Nodes/Screens/CardLibrary/NCardLibraryGrid.cs")]
public class NCardLibraryGrid : NCardGrid
{
  private readonly List<CardModel> _allCards = new List<CardModel>();
  private HashSet<ModelId> _seenCards;
  private HashSet<CardModel> _unlockedCards;
  private bool _showStats;

  protected override bool IsCardLibrary => true;

  protected override bool CenterGrid => false;

  public bool ShowStats
  {
    get => this._showStats;
    set
    {
      this._showStats = value;
      foreach (NGridCardHolder ngridCardHolder in this._cardRows.SelectMany<List<NGridCardHolder>, NGridCardHolder>((Func<List<NGridCardHolder>, IEnumerable<NGridCardHolder>>) (r => (IEnumerable<NGridCardHolder>) r)))
      {
        if (ngridCardHolder.CardNode != null)
          ((CanvasItem) ((Node) ngridCardHolder).GetNode<NCardLibraryStats>(NodePath.op_Implicit("CardLibraryStats"))).Visible = this._showStats;
      }
    }
  }

  public override void _Ready()
  {
    this.ConnectSignals();
    List<CardPoolModel> list = ModelDb.AllCardPools.ToList<CardPoolModel>();
    foreach (CardModel allCard in ModelDb.AllCards)
    {
      if (allCard.ShouldShowInCardLibrary)
        this._allCards.Add(allCard);
    }
    this._allCards.Sort((IComparer<CardModel>) new NCardLibraryGrid.InitialSorter(list));
    this.RefreshVisibility();
  }

  public void RefreshVisibility()
  {
    this._seenCards = ((IEnumerable<ModelId>) SaveManager.Instance.Progress.DiscoveredCards).ToHashSet<ModelId>();
    UnlockState unlockState = SaveManager.Instance.GenerateUnlockStateFromProgress();
    this._unlockedCards = ModelDb.AllCardPools.Select<CardPoolModel, IEnumerable<CardModel>>((Func<CardPoolModel, IEnumerable<CardModel>>) (p => p.GetUnlockedCards(unlockState, CardMultiplayerConstraint.None))).SelectMany<IEnumerable<CardModel>, CardModel>((Func<IEnumerable<CardModel>, IEnumerable<CardModel>>) (c => c)).ToHashSet<CardModel>();
  }

  public void FilterCards(Func<CardModel, bool> filter)
  {
    Func<CardModel, bool> filter1 = filter;
    int capacity = 1;
    List<SortingOrders> sortingPriority = new List<SortingOrders>(capacity);
    CollectionsMarshal.SetCount<SortingOrders>(sortingPriority, capacity);
    CollectionsMarshal.AsSpan<SortingOrders>(sortingPriority)[0] = SortingOrders.AlphabetAscending;
    this.FilterCards(filter1, sortingPriority);
  }

  public void FilterCards(Func<CardModel, bool> filter, List<SortingOrders> sortingPriority)
  {
    this.DisplayCards(this._allCards.Where<CardModel>(filter).ToList<CardModel>(), sortingPriority);
  }

  private void DisplayCards(List<CardModel> cards, List<SortingOrders> sortingPriority)
  {
    this.SetCards((IReadOnlyList<CardModel>) cards, PileType.None, sortingPriority, Task.CompletedTask);
  }

  protected override void InitGrid()
  {
    base.InitGrid();
    foreach (NGridCardHolder ngridCardHolder in this._cardRows.SelectMany<List<NGridCardHolder>, NGridCardHolder>((Func<List<NGridCardHolder>, IEnumerable<NGridCardHolder>>) (r => (IEnumerable<NGridCardHolder>) r)))
    {
      if (ngridCardHolder.CardNode != null)
      {
        bool flag = this._seenCards.Contains(ngridCardHolder.CardNode.Model.Id);
        ngridCardHolder.EnsureCardLibraryStatsExists();
        NCardLibraryStats cardLibraryStats = ngridCardHolder.CardLibraryStats;
        cardLibraryStats.UpdateStats(ngridCardHolder.CardNode.Model);
        ((CanvasItem) cardLibraryStats).Visible = this.ShowStats;
        ngridCardHolder.Hitbox.MouseDefaultCursorShape = flag ? (Control.CursorShape) 16L /*0x10*/ : (Control.CursorShape) 0L;
      }
    }
  }

  protected override void AssignCardsToRow(List<NGridCardHolder> row, int startIndex)
  {
    base.AssignCardsToRow(row, startIndex);
    foreach (NGridCardHolder ngridCardHolder in row)
    {
      if (ngridCardHolder.CardNode != null)
      {
        CardModel model = ngridCardHolder.CardNode.Model;
        bool flag = this._seenCards.Contains(model.Id);
        ngridCardHolder.CardLibraryStats.UpdateStats(model);
        ngridCardHolder.Hitbox.MouseDefaultCursorShape = flag ? (Control.CursorShape) 16L /*0x10*/ : (Control.CursorShape) 0L;
      }
    }
  }

  protected override ModelVisibility GetCardVisibility(CardModel card)
  {
    if (!this._unlockedCards.Contains(card))
      return ModelVisibility.Locked;
    return !this._seenCards.Contains(card.Id) ? ModelVisibility.NotSeen : ModelVisibility.Visible;
  }

  public IEnumerable<CardModel> VisibleCards => (IEnumerable<CardModel>) this._cards;

  protected override void UpdateGridNavigation()
  {
    for (int index1 = 0; index1 < this._cardRows.Count; ++index1)
    {
      for (int index2 = 0; index2 < this._cardRows[index1].Count; ++index2)
      {
        NCardHolder ncardHolder = (NCardHolder) this._cardRows[index1][index2];
        ncardHolder.FocusNeighborLeft = index2 > 0 ? ((Node) this._cardRows[index1][index2 - 1]).GetPath() : (NodePath) null;
        ncardHolder.FocusNeighborRight = index2 < this._cardRows[index1].Count - 1 ? ((Node) this._cardRows[index1][index2 + 1]).GetPath() : (NodePath) null;
        ncardHolder.FocusNeighborTop = index1 > 0 ? ((Node) this._cardRows[index1 - 1][index2]).GetPath() : (NodePath) null;
        ncardHolder.FocusNeighborBottom = index1 >= this._cardRows.Count - 1 || index2 >= this._cardRows[index1 + 1].Count ? (NodePath) null : ((Node) this._cardRows[index1 + 1][index2]).GetPath();
      }
    }
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(4)
    {
      new MethodInfo(NCardLibraryGrid.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCardLibraryGrid.MethodName.RefreshVisibility, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCardLibraryGrid.MethodName.InitGrid, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCardLibraryGrid.MethodName.UpdateGridNavigation, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NCardLibraryGrid.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCardLibraryGrid.MethodName.RefreshVisibility) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.RefreshVisibility();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCardLibraryGrid.MethodName.InitGrid) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.InitGrid();
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NCardLibraryGrid.MethodName.UpdateGridNavigation) || ((NativeVariantPtrArgs) ref args).Count != 0)
      return base.InvokeGodotClassMethod(in method, args, out ret);
    this.UpdateGridNavigation();
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NCardLibraryGrid.MethodName._Ready) || StringName.op_Equality(ref method, NCardLibraryGrid.MethodName.RefreshVisibility) || StringName.op_Equality(ref method, NCardLibraryGrid.MethodName.InitGrid) || StringName.op_Equality(ref method, NCardLibraryGrid.MethodName.UpdateGridNavigation) || base.HasGodotClassMethod(in method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NCardLibraryGrid.PropertyName.ShowStats))
    {
      this.ShowStats = VariantUtils.ConvertTo<bool>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NCardLibraryGrid.PropertyName._showStats))
      return base.SetGodotClassPropertyValue(in name, in value);
    this._showStats = VariantUtils.ConvertTo<bool>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NCardLibraryGrid.PropertyName.IsCardLibrary))
    {
      ref godot_variant local = ref value;
      bool isCardLibrary = this.IsCardLibrary;
      godot_variant from = VariantUtils.CreateFrom<bool>(ref isCardLibrary);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NCardLibraryGrid.PropertyName.CenterGrid))
    {
      ref godot_variant local = ref value;
      bool centerGrid = this.CenterGrid;
      godot_variant from = VariantUtils.CreateFrom<bool>(ref centerGrid);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NCardLibraryGrid.PropertyName.ShowStats))
    {
      ref godot_variant local = ref value;
      bool showStats = this.ShowStats;
      godot_variant from = VariantUtils.CreateFrom<bool>(ref showStats);
      local = from;
      return true;
    }
    if (!StringName.op_Equality(ref name, NCardLibraryGrid.PropertyName._showStats))
      return base.GetGodotClassPropertyValue(in name, out value);
    value = VariantUtils.CreateFrom<bool>(ref this._showStats);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 1L, NCardLibraryGrid.PropertyName.IsCardLibrary, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 1L, NCardLibraryGrid.PropertyName.CenterGrid, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 1L, NCardLibraryGrid.PropertyName._showStats, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 1L, NCardLibraryGrid.PropertyName.ShowStats, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    base.SaveGodotObjectData(info);
    GodotSerializationInfo serializationInfo = info;
    StringName showStats1 = NCardLibraryGrid.PropertyName.ShowStats;
    bool showStats2 = this.ShowStats;
    Variant variant = Variant.From<bool>(ref showStats2);
    serializationInfo.AddProperty(showStats1, variant);
    info.AddProperty(NCardLibraryGrid.PropertyName._showStats, Variant.From<bool>(ref this._showStats));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    base.RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NCardLibraryGrid.PropertyName.ShowStats, ref variant1))
      this.ShowStats = ((Variant) ref variant1).As<bool>();
    Variant variant2;
    if (!info.TryGetProperty(NCardLibraryGrid.PropertyName._showStats, ref variant2))
      return;
    this._showStats = ((Variant) ref variant2).As<bool>();
  }

  private struct InitialSorter(
  #nullable enable
  List<CardPoolModel> cardPoolModels) : IComparer<CardModel>
  {
    private List<CardPoolModel> _cardPoolModels = cardPoolModels;

    public int Compare(CardModel? x, CardModel? y)
    {
      if (x == null)
        return y != null ? -1 : 0;
      if (y == null)
        return 1;
      int num1 = this._cardPoolModels.IndexOf(x.Pool).CompareTo(this._cardPoolModels.IndexOf(y.Pool));
      if (num1 != 0)
        return num1;
      int num2 = x.Rarity.CompareTo((object) y.Rarity);
      return num2 != 0 ? num2 : x.Id.CompareTo(y.Id);
    }
  }

  public new class MethodName : NCardGrid.MethodName
  {
    public new static readonly 
    #nullable disable
    StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName RefreshVisibility = StringName.op_Implicit(nameof (RefreshVisibility));
    public new static readonly StringName InitGrid = StringName.op_Implicit(nameof (InitGrid));
    public new static readonly StringName UpdateGridNavigation = StringName.op_Implicit(nameof (UpdateGridNavigation));
  }

  public new class PropertyName : NCardGrid.PropertyName
  {
    public new static readonly StringName IsCardLibrary = StringName.op_Implicit(nameof (IsCardLibrary));
    public new static readonly StringName CenterGrid = StringName.op_Implicit(nameof (CenterGrid));
    public static readonly StringName ShowStats = StringName.op_Implicit(nameof (ShowStats));
    public static readonly StringName _showStats = StringName.op_Implicit(nameof (_showStats));
  }

  public new class SignalName : NCardGrid.SignalName
  {
  }
}
