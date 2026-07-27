// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Screens.NDeckViewScreen
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Audio.Debug;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Multiplayer;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Cards;
using MegaCrit.Sts2.Core.Nodes.Cards.Holders;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.Screens.Capstones;
using MegaCrit.Sts2.Core.Nodes.Screens.CardLibrary;
using MegaCrit.Sts2.Core.TestSupport;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Screens;

[ScriptPath("res://src/Core/Nodes/Screens/NDeckViewScreen.cs")]
public class NDeckViewScreen : NCardsViewScreen
{
  private Player _player;
  private CardPile _pile;
  private NCardViewSortButton _obtainedSorter;
  private NCardViewSortButton _typeSorter;
  private NCardViewSortButton _costSorter;
  private NCardViewSortButton _alphabetSorter;
  private Control _bg;
  private readonly List<SortingOrders> _sortingPriority;

  private static string ScenePath => SceneHelper.GetScenePath("screens/deck_view_screen");

  public static IEnumerable<string> AssetPaths
  {
    get
    {
      return (IEnumerable<string>) new \u003C\u003Ez__ReadOnlySingleElementList<string>(NDeckViewScreen.ScenePath);
    }
  }

  public override NetScreenType ScreenType => NetScreenType.DeckView;

  public static NDeckViewScreen? ShowScreen(Player player)
  {
    if (TestMode.IsOn)
      return (NDeckViewScreen) null;
    NDeckViewScreen screen = PreloadManager.Cache.GetScene(NDeckViewScreen.ScenePath).Instantiate<NDeckViewScreen>((PackedScene.GenEditState) 0L);
    screen._player = player;
    NDebugAudioManager.Instance?.Play("map_open.mp3");
    NCapstoneContainer.Instance.Open((ICapstoneScreen) screen);
    return screen;
  }

  public override void _Ready()
  {
    this._cards = this._pile.Cards.ToList<CardModel>();
    this._infoText = new LocString("gameplay_ui", "DECK_PILE_INFO");
    this._bg = ((Node) this).GetNode<Control>(NodePath.op_Implicit("%SortingBg"));
    this._obtainedSorter = ((Node) this).GetNode<NCardViewSortButton>(NodePath.op_Implicit("%ObtainedSorter"));
    this._typeSorter = ((Node) this).GetNode<NCardViewSortButton>(NodePath.op_Implicit("%CardTypeSorter"));
    this._costSorter = ((Node) this).GetNode<NCardViewSortButton>(NodePath.op_Implicit("%CostSorter"));
    this._alphabetSorter = ((Node) this).GetNode<NCardViewSortButton>(NodePath.op_Implicit("%AlphabeticalSorter"));
    ((GodotObject) this._obtainedSorter).Connect(NClickableControl.SignalName.Released, Callable.From<NButton>(new Action<NButton>(this.OnObtainedSort)), 0U);
    ((GodotObject) this._typeSorter).Connect(NClickableControl.SignalName.Released, Callable.From<NButton>(new Action<NButton>(this.OnCardTypeSort)), 0U);
    ((GodotObject) this._costSorter).Connect(NClickableControl.SignalName.Released, Callable.From<NButton>(new Action<NButton>(this.OnCostSort)), 0U);
    ((GodotObject) this._alphabetSorter).Connect(NClickableControl.SignalName.Released, Callable.From<NButton>(new Action<NButton>(this.OnAlphabetSort)), 0U);
    this._obtainedSorter.SetLabel(new LocString("gameplay_ui", "SORT_OBTAINED").GetRawText());
    this._typeSorter.SetLabel(new LocString("gameplay_ui", "SORT_TYPE").GetRawText());
    this._costSorter.SetLabel(new LocString("gameplay_ui", "SORT_COST").GetRawText());
    this._alphabetSorter.SetLabel(new LocString("gameplay_ui", "SORT_ALPHABET").GetRawText());
    ((Node) this).GetNode<MegaLabel>(NodePath.op_Implicit("%ViewUpgradesLabel")).SetTextAutoSize(new LocString("gameplay_ui", "VIEW_UPGRADES").GetFormattedText());
    ShaderMaterial frameMaterial = (ShaderMaterial) this._player.Character.CardPool.FrameMaterial;
    ((CanvasItem) this._bg).Material = (Material) frameMaterial;
    this._obtainedSorter.SetHue(frameMaterial);
    this._typeSorter.SetHue(frameMaterial);
    this._costSorter.SetHue(frameMaterial);
    this._alphabetSorter.SetHue(frameMaterial);
    this.ConnectSignals();
    this.DisplayCards();
    Control[] controlArray = new Control[4]
    {
      (Control) this._obtainedSorter,
      (Control) this._typeSorter,
      (Control) this._costSorter,
      (Control) this._alphabetSorter
    };
    for (int index = 0; index < controlArray.Length; ++index)
    {
      controlArray[index].FocusNeighborTop = ((Node) controlArray[index]).GetPath();
      controlArray[index].FocusNeighborBottom = this._grid.DefaultFocusedControl != null ? ((Node) this._grid.DefaultFocusedControl).GetPath() : ((Node) controlArray[index]).GetPath();
      controlArray[index].FocusNeighborLeft = index > 0 ? ((Node) controlArray[index - 1]).GetPath() : ((Node) controlArray[index]).GetPath();
      controlArray[index].FocusNeighborRight = index < controlArray.Length - 1 ? ((Node) controlArray[index + 1]).GetPath() : ((Node) controlArray[index]).GetPath();
    }
  }

  protected override void ConnectSignals()
  {
    base.ConnectSignals();
    ((GodotObject) this._grid).Connect(NCardGrid.SignalName.HolderPressed, Callable.From<NCardHolder>((Action<NCardHolder>) (h => this.ShowCardDetail(h.CardModel))), 0U);
    ((GodotObject) this._grid).Connect(NCardGrid.SignalName.HolderAltPressed, Callable.From<NCardHolder>((Action<NCardHolder>) (h => this.ShowCardDetail(h.CardModel))), 0U);
  }

  public override void _EnterTree()
  {
    ((Node) this)._EnterTree();
    this._pile = PileType.Deck.GetPile(this._player);
    this._pile.ContentsChanged += new Action(this.OnPileContentsChanged);
  }

  public override void _ExitTree()
  {
    ((Node) this)._ExitTree();
    this._pile.ContentsChanged -= new Action(this.OnPileContentsChanged);
  }

  public override void AfterCapstoneClosed()
  {
    base.AfterCapstoneClosed();
    NRun.Instance?.GlobalUi.TopBar.Deck.ToggleAnimState();
  }

  private void OnPileContentsChanged()
  {
    this._cards = this._pile.Cards.ToList<CardModel>();
    this.DisplayCards();
  }

  private void OnObtainedSort(NButton button)
  {
    this._sortingPriority.Remove(SortingOrders.Ascending);
    this._sortingPriority.Remove(SortingOrders.Descending);
    if (this._obtainedSorter.IsDescending)
      this._sortingPriority.Insert(0, SortingOrders.Descending);
    else
      this._sortingPriority.Insert(0, SortingOrders.Ascending);
    this.DisplayCards();
  }

  private void OnCardTypeSort(NButton button)
  {
    this._sortingPriority.Remove(SortingOrders.TypeAscending);
    this._sortingPriority.Remove(SortingOrders.TypeDescending);
    if (this._typeSorter.IsDescending)
      this._sortingPriority.Insert(0, SortingOrders.TypeDescending);
    else
      this._sortingPriority.Insert(0, SortingOrders.TypeAscending);
    this.DisplayCards();
  }

  private void OnCostSort(NButton button)
  {
    this._sortingPriority.Remove(SortingOrders.CostAscending);
    this._sortingPriority.Remove(SortingOrders.CostDescending);
    if (this._costSorter.IsDescending)
      this._sortingPriority.Insert(0, SortingOrders.CostDescending);
    else
      this._sortingPriority.Insert(0, SortingOrders.CostAscending);
    this.DisplayCards();
  }

  private void OnAlphabetSort(NButton button)
  {
    this._sortingPriority.Remove(SortingOrders.AlphabetAscending);
    this._sortingPriority.Remove(SortingOrders.AlphabetDescending);
    if (this._alphabetSorter.IsDescending)
      this._sortingPriority.Insert(0, SortingOrders.AlphabetDescending);
    else
      this._sortingPriority.Insert(0, SortingOrders.AlphabetAscending);
    this.DisplayCards();
  }

  private void DisplayCards()
  {
    this._grid.YOffset = 100;
    this._grid.SetCards((IReadOnlyList<CardModel>) this._cards, this._pile.Type, this._sortingPriority);
    IEnumerable<NGridCardHolder> topRowOfCardNodes = this._grid.GetTopRowOfCardNodes();
    if (topRowOfCardNodes == null)
      return;
    foreach (Control control in topRowOfCardNodes)
      control.FocusNeighborTop = ((Node) this._obtainedSorter).GetPath();
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(11)
    {
      new MethodInfo(NDeckViewScreen.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NDeckViewScreen.MethodName.ConnectSignals, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NDeckViewScreen.MethodName._EnterTree, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NDeckViewScreen.MethodName._ExitTree, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NDeckViewScreen.MethodName.AfterCapstoneClosed, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NDeckViewScreen.MethodName.OnPileContentsChanged, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NDeckViewScreen.MethodName.OnObtainedSort, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("button"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false)
      }, (List<Variant>) null),
      new MethodInfo(NDeckViewScreen.MethodName.OnCardTypeSort, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("button"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false)
      }, (List<Variant>) null),
      new MethodInfo(NDeckViewScreen.MethodName.OnCostSort, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("button"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false)
      }, (List<Variant>) null),
      new MethodInfo(NDeckViewScreen.MethodName.OnAlphabetSort, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("button"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false)
      }, (List<Variant>) null),
      new MethodInfo(NDeckViewScreen.MethodName.DisplayCards, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NDeckViewScreen.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NDeckViewScreen.MethodName.ConnectSignals) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.ConnectSignals();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NDeckViewScreen.MethodName._EnterTree) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._EnterTree();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NDeckViewScreen.MethodName._ExitTree) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._ExitTree();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NDeckViewScreen.MethodName.AfterCapstoneClosed) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.AfterCapstoneClosed();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NDeckViewScreen.MethodName.OnPileContentsChanged) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnPileContentsChanged();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NDeckViewScreen.MethodName.OnObtainedSort) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.OnObtainedSort(VariantUtils.ConvertTo<NButton>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NDeckViewScreen.MethodName.OnCardTypeSort) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.OnCardTypeSort(VariantUtils.ConvertTo<NButton>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NDeckViewScreen.MethodName.OnCostSort) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.OnCostSort(VariantUtils.ConvertTo<NButton>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NDeckViewScreen.MethodName.OnAlphabetSort) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.OnAlphabetSort(VariantUtils.ConvertTo<NButton>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NDeckViewScreen.MethodName.DisplayCards) || ((NativeVariantPtrArgs) ref args).Count != 0)
      return base.InvokeGodotClassMethod(in method, args, out ret);
    this.DisplayCards();
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NDeckViewScreen.MethodName._Ready) || StringName.op_Equality(ref method, NDeckViewScreen.MethodName.ConnectSignals) || StringName.op_Equality(ref method, NDeckViewScreen.MethodName._EnterTree) || StringName.op_Equality(ref method, NDeckViewScreen.MethodName._ExitTree) || StringName.op_Equality(ref method, NDeckViewScreen.MethodName.AfterCapstoneClosed) || StringName.op_Equality(ref method, NDeckViewScreen.MethodName.OnPileContentsChanged) || StringName.op_Equality(ref method, NDeckViewScreen.MethodName.OnObtainedSort) || StringName.op_Equality(ref method, NDeckViewScreen.MethodName.OnCardTypeSort) || StringName.op_Equality(ref method, NDeckViewScreen.MethodName.OnCostSort) || StringName.op_Equality(ref method, NDeckViewScreen.MethodName.OnAlphabetSort) || StringName.op_Equality(ref method, NDeckViewScreen.MethodName.DisplayCards) || base.HasGodotClassMethod(in method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NDeckViewScreen.PropertyName._obtainedSorter))
    {
      this._obtainedSorter = VariantUtils.ConvertTo<NCardViewSortButton>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NDeckViewScreen.PropertyName._typeSorter))
    {
      this._typeSorter = VariantUtils.ConvertTo<NCardViewSortButton>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NDeckViewScreen.PropertyName._costSorter))
    {
      this._costSorter = VariantUtils.ConvertTo<NCardViewSortButton>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NDeckViewScreen.PropertyName._alphabetSorter))
    {
      this._alphabetSorter = VariantUtils.ConvertTo<NCardViewSortButton>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NDeckViewScreen.PropertyName._bg))
      return base.SetGodotClassPropertyValue(in name, in value);
    this._bg = VariantUtils.ConvertTo<Control>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NDeckViewScreen.PropertyName.ScreenType))
    {
      ref godot_variant local = ref value;
      NetScreenType screenType = this.ScreenType;
      godot_variant from = VariantUtils.CreateFrom<NetScreenType>(ref screenType);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NDeckViewScreen.PropertyName._obtainedSorter))
    {
      value = VariantUtils.CreateFrom<NCardViewSortButton>(ref this._obtainedSorter);
      return true;
    }
    if (StringName.op_Equality(ref name, NDeckViewScreen.PropertyName._typeSorter))
    {
      value = VariantUtils.CreateFrom<NCardViewSortButton>(ref this._typeSorter);
      return true;
    }
    if (StringName.op_Equality(ref name, NDeckViewScreen.PropertyName._costSorter))
    {
      value = VariantUtils.CreateFrom<NCardViewSortButton>(ref this._costSorter);
      return true;
    }
    if (StringName.op_Equality(ref name, NDeckViewScreen.PropertyName._alphabetSorter))
    {
      value = VariantUtils.CreateFrom<NCardViewSortButton>(ref this._alphabetSorter);
      return true;
    }
    if (!StringName.op_Equality(ref name, NDeckViewScreen.PropertyName._bg))
      return base.GetGodotClassPropertyValue(in name, out value);
    value = VariantUtils.CreateFrom<Control>(ref this._bg);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 2L, NDeckViewScreen.PropertyName.ScreenType, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NDeckViewScreen.PropertyName._obtainedSorter, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NDeckViewScreen.PropertyName._typeSorter, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NDeckViewScreen.PropertyName._costSorter, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NDeckViewScreen.PropertyName._alphabetSorter, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NDeckViewScreen.PropertyName._bg, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    base.SaveGodotObjectData(info);
    info.AddProperty(NDeckViewScreen.PropertyName._obtainedSorter, Variant.From<NCardViewSortButton>(ref this._obtainedSorter));
    info.AddProperty(NDeckViewScreen.PropertyName._typeSorter, Variant.From<NCardViewSortButton>(ref this._typeSorter));
    info.AddProperty(NDeckViewScreen.PropertyName._costSorter, Variant.From<NCardViewSortButton>(ref this._costSorter));
    info.AddProperty(NDeckViewScreen.PropertyName._alphabetSorter, Variant.From<NCardViewSortButton>(ref this._alphabetSorter));
    info.AddProperty(NDeckViewScreen.PropertyName._bg, Variant.From<Control>(ref this._bg));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    base.RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NDeckViewScreen.PropertyName._obtainedSorter, ref variant1))
      this._obtainedSorter = ((Variant) ref variant1).As<NCardViewSortButton>();
    Variant variant2;
    if (info.TryGetProperty(NDeckViewScreen.PropertyName._typeSorter, ref variant2))
      this._typeSorter = ((Variant) ref variant2).As<NCardViewSortButton>();
    Variant variant3;
    if (info.TryGetProperty(NDeckViewScreen.PropertyName._costSorter, ref variant3))
      this._costSorter = ((Variant) ref variant3).As<NCardViewSortButton>();
    Variant variant4;
    if (info.TryGetProperty(NDeckViewScreen.PropertyName._alphabetSorter, ref variant4))
      this._alphabetSorter = ((Variant) ref variant4).As<NCardViewSortButton>();
    Variant variant5;
    if (!info.TryGetProperty(NDeckViewScreen.PropertyName._bg, ref variant5))
      return;
    this._bg = ((Variant) ref variant5).As<Control>();
  }

  public NDeckViewScreen()
  {
    int capacity = 4;
    List<SortingOrders> sortingOrdersList = new List<SortingOrders>(capacity);
    CollectionsMarshal.SetCount<SortingOrders>(sortingOrdersList, capacity);
    Span<SortingOrders> span = CollectionsMarshal.AsSpan<SortingOrders>(sortingOrdersList);
    int num1 = 0;
    span[num1] = SortingOrders.Ascending;
    int num2 = num1 + 1;
    span[num2] = SortingOrders.TypeAscending;
    int num3 = num2 + 1;
    span[num3] = SortingOrders.CostAscending;
    int num4 = num3 + 1;
    span[num4] = SortingOrders.AlphabetAscending;
    this._sortingPriority = sortingOrdersList;
    // ISSUE: explicit constructor call
    base.\u002Ector();
  }

  public new class MethodName : NCardsViewScreen.MethodName
  {
    public new static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public new static readonly StringName ConnectSignals = StringName.op_Implicit(nameof (ConnectSignals));
    public static readonly StringName _EnterTree = StringName.op_Implicit(nameof (_EnterTree));
    public static readonly StringName _ExitTree = StringName.op_Implicit(nameof (_ExitTree));
    public new static readonly StringName AfterCapstoneClosed = StringName.op_Implicit(nameof (AfterCapstoneClosed));
    public static readonly StringName OnPileContentsChanged = StringName.op_Implicit(nameof (OnPileContentsChanged));
    public static readonly StringName OnObtainedSort = StringName.op_Implicit(nameof (OnObtainedSort));
    public static readonly StringName OnCardTypeSort = StringName.op_Implicit(nameof (OnCardTypeSort));
    public static readonly StringName OnCostSort = StringName.op_Implicit(nameof (OnCostSort));
    public static readonly StringName OnAlphabetSort = StringName.op_Implicit(nameof (OnAlphabetSort));
    public static readonly StringName DisplayCards = StringName.op_Implicit(nameof (DisplayCards));
  }

  public new class PropertyName : NCardsViewScreen.PropertyName
  {
    public new static readonly StringName ScreenType = StringName.op_Implicit(nameof (ScreenType));
    public static readonly StringName _obtainedSorter = StringName.op_Implicit(nameof (_obtainedSorter));
    public static readonly StringName _typeSorter = StringName.op_Implicit(nameof (_typeSorter));
    public static readonly StringName _costSorter = StringName.op_Implicit(nameof (_costSorter));
    public static readonly StringName _alphabetSorter = StringName.op_Implicit(nameof (_alphabetSorter));
    public static readonly StringName _bg = StringName.op_Implicit(nameof (_bg));
  }

  public new class SignalName : NCardsViewScreen.SignalName
  {
  }
}
