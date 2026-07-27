// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Screens.CardLibrary.NCardLibrary
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Models.Characters;
using MegaCrit.Sts2.Core.Nodes.Cards;
using MegaCrit.Sts2.Core.Nodes.Cards.Holders;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.Screens.MainMenu;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.Saves;
using MegaCrit.Sts2.Core.TestSupport;
using MegaCrit.Sts2.Core.Unlocks;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Screens.CardLibrary;

[ScriptPath("res://src/Core/Nodes/Screens/CardLibrary/NCardLibrary.cs")]
public sealed class NCardLibrary : NSubmenu
{
  private const int _delayAfterTextFilterChangedMsec = 250;
  private static readonly string _scenePath = SceneHelper.GetScenePath("screens/card_library/card_library");
  private readonly LocString _cardCountLocString;
  private readonly LocString _noResultsLocString;
  private IRunState? _runState;
  private NCardLibraryGrid _grid;
  private NSearchBar _searchBar;
  private readonly Dictionary<string, Func<CardModel, bool>> _specialSearchbarKeywords;
  private readonly Dictionary<CharacterModel, NCardPoolFilter> _cardPoolFilters;
  private NCardPoolFilter _ironcladFilter;
  private NCardPoolFilter _silentFilter;
  private NCardPoolFilter _defectFilter;
  private NCardPoolFilter _regentFilter;
  private NCardPoolFilter _necrobinderFilter;
  private NCardPoolFilter _colorlessFilter;
  private NCardPoolFilter _ancientsFilter;
  private NCardPoolFilter _miscPoolFilter;
  private readonly Dictionary<NCardPoolFilter, Func<CardModel, bool>> _poolFilters;
  private NCardViewSortButton _typeSorter;
  private NCardTypeTickbox _attackFilter;
  private NCardTypeTickbox _skillFilter;
  private NCardTypeTickbox _powerFilter;
  private NCardTypeTickbox _otherTypeFilter;
  private readonly Dictionary<NCardTypeTickbox, Func<CardModel, bool>> _cardTypeFilters;
  private NCardViewSortButton _raritySorter;
  private NCardRarityTickbox _commonFilter;
  private NCardRarityTickbox _uncommonFilter;
  private NCardRarityTickbox _rareFilter;
  private NCardRarityTickbox _otherFilter;
  private readonly Dictionary<NCardRarityTickbox, Func<CardModel, bool>> _rarityFilters;
  private NCardViewSortButton _costSorter;
  private NCardCostTickbox _zeroFilter;
  private NCardCostTickbox _oneFilter;
  private NCardCostTickbox _twoFilter;
  private NCardCostTickbox _threePlusFilter;
  private NCardCostTickbox _xFilter;
  private readonly Dictionary<NCardCostTickbox, Func<CardModel, bool>> _costFilters;
  private NCardViewSortButton _alphabetSorter;
  private NLibraryStatTickbox _viewMultiplayerCards;
  private NLibraryStatTickbox _viewStats;
  private NLibraryStatTickbox _viewUpgrades;
  private MegaRichTextLabel _cardCountLabel;
  private MegaRichTextLabel _noResultsLabel;
  private CancellationTokenSource? _displayCardsShortDelayCancelToken;
  private readonly List<SortingOrders> _sortingPriority;
  private Func<CardModel, bool> _filter;
  private Control? _lastHoveredControl;

  public static string[] AssetPaths
  {
    get => new string[1]{ NCardLibrary._scenePath };
  }

  public static NCardLibrary? Create()
  {
    return TestMode.IsOn ? (NCardLibrary) null : PreloadManager.Cache.GetScene(NCardLibrary._scenePath).Instantiate<NCardLibrary>((PackedScene.GenEditState) 0L);
  }

  public override void _Ready()
  {
    this.ConnectSignals();
    this._grid = ((Node) this).GetNode<NCardLibraryGrid>(NodePath.op_Implicit("%CardGrid"));
    ((GodotObject) this._grid).Connect(NCardGrid.SignalName.HolderPressed, Callable.From<NCardHolder>(new Action<NCardHolder>(this.ShowCardDetail)), 0U);
    ((GodotObject) this._grid).Connect(NCardGrid.SignalName.HolderAltPressed, Callable.From<NCardHolder>(new Action<NCardHolder>(this.ShowCardDetail)), 0U);
    this._cardCountLabel = ((Node) this).GetNode<MegaRichTextLabel>(NodePath.op_Implicit("%CardCountLabel"));
    this._noResultsLabel = ((Node) this).GetNode<MegaRichTextLabel>(NodePath.op_Implicit("%NoResultsLabel"));
    this._noResultsLabel.Text = this._noResultsLocString.GetFormattedText();
    this._searchBar = ((Node) this).GetNode<NSearchBar>(NodePath.op_Implicit("%SearchBar"));
    ((GodotObject) this._searchBar).Connect(NSearchBar.SignalName.QueryChanged, Callable.From<string>(new Action<string>(this.SearchBarQueryChanged)), 0U);
    ((GodotObject) this._searchBar).Connect(NSearchBar.SignalName.QuerySubmitted, Callable.From<string>(new Action<string>(this.SearchBarQuerySubmitted)), 0U);
    this._ironcladFilter = ((Node) this).GetNode<NCardPoolFilter>(NodePath.op_Implicit("%IroncladPool"));
    this._silentFilter = ((Node) this).GetNode<NCardPoolFilter>(NodePath.op_Implicit("%SilentPool"));
    this._defectFilter = ((Node) this).GetNode<NCardPoolFilter>(NodePath.op_Implicit("%DefectPool"));
    this._regentFilter = ((Node) this).GetNode<NCardPoolFilter>(NodePath.op_Implicit("%RegentPool"));
    this._necrobinderFilter = ((Node) this).GetNode<NCardPoolFilter>(NodePath.op_Implicit("%NecrobinderPool"));
    this._colorlessFilter = ((Node) this).GetNode<NCardPoolFilter>(NodePath.op_Implicit("%ColorlessPool"));
    this._ancientsFilter = ((Node) this).GetNode<NCardPoolFilter>(NodePath.op_Implicit("%AncientsPool"));
    this._miscPoolFilter = ((Node) this).GetNode<NCardPoolFilter>(NodePath.op_Implicit("%MiscPool"));
    Callable callable1 = Callable.From<NCardPoolFilter>(new Action<NCardPoolFilter>(this.UpdateCardPoolFilter));
    ((GodotObject) this._ironcladFilter).Connect(NCardPoolFilter.SignalName.Toggled, callable1, 0U);
    ((GodotObject) this._silentFilter).Connect(NCardPoolFilter.SignalName.Toggled, callable1, 0U);
    ((GodotObject) this._defectFilter).Connect(NCardPoolFilter.SignalName.Toggled, callable1, 0U);
    ((GodotObject) this._regentFilter).Connect(NCardPoolFilter.SignalName.Toggled, callable1, 0U);
    ((GodotObject) this._necrobinderFilter).Connect(NCardPoolFilter.SignalName.Toggled, callable1, 0U);
    ((GodotObject) this._colorlessFilter).Connect(NCardPoolFilter.SignalName.Toggled, callable1, 0U);
    ((GodotObject) this._ancientsFilter).Connect(NCardPoolFilter.SignalName.Toggled, callable1, 0U);
    ((GodotObject) this._miscPoolFilter).Connect(NCardPoolFilter.SignalName.Toggled, callable1, 0U);
    this._poolFilters.Add(this._ironcladFilter, (Func<CardModel, bool>) (c => c.Pool is IroncladCardPool));
    this._poolFilters.Add(this._silentFilter, (Func<CardModel, bool>) (c => c.Pool is SilentCardPool));
    this._poolFilters.Add(this._defectFilter, (Func<CardModel, bool>) (c => c.Pool is DefectCardPool));
    this._poolFilters.Add(this._regentFilter, (Func<CardModel, bool>) (c => c.Pool is RegentCardPool));
    this._poolFilters.Add(this._necrobinderFilter, (Func<CardModel, bool>) (c => c.Pool is NecrobinderCardPool));
    this._poolFilters.Add(this._colorlessFilter, (Func<CardModel, bool>) (c => c.Pool is ColorlessCardPool));
    this._poolFilters.Add(this._ancientsFilter, (Func<CardModel, bool>) (c => c.Rarity == CardRarity.Ancient));
    this._poolFilters.Add(this._miscPoolFilter, (Func<CardModel, bool>) (c =>
    {
      bool flag;
      switch (c.Rarity)
      {
        case CardRarity.Event:
        case CardRarity.Token:
        case CardRarity.Status:
        case CardRarity.Curse:
        case CardRarity.Quest:
          flag = true;
          break;
        default:
          flag = false;
          break;
      }
      return flag;
    }));
    this._typeSorter = ((Node) this).GetNode<NCardViewSortButton>(NodePath.op_Implicit("%CardTypeSorter"));
    ((GodotObject) this._typeSorter).Connect(NClickableControl.SignalName.Released, Callable.From<NButton>(new Action<NButton>(this.OnCardTypeSort)), 0U);
    this._attackFilter = ((Node) this).GetNode<NCardTypeTickbox>(NodePath.op_Implicit("%AttackType"));
    this._skillFilter = ((Node) this).GetNode<NCardTypeTickbox>(NodePath.op_Implicit("%SkillType"));
    this._powerFilter = ((Node) this).GetNode<NCardTypeTickbox>(NodePath.op_Implicit("%PowerType"));
    this._otherTypeFilter = ((Node) this).GetNode<NCardTypeTickbox>(NodePath.op_Implicit("%OtherType"));
    Callable callable2 = Callable.From<NCardTypeTickbox>(new Action<NCardTypeTickbox>(this.UpdateTypeFilter));
    ((GodotObject) this._attackFilter).Connect(NCardTypeTickbox.SignalName.Toggled, callable2, 0U);
    ((GodotObject) this._skillFilter).Connect(NCardTypeTickbox.SignalName.Toggled, callable2, 0U);
    ((GodotObject) this._powerFilter).Connect(NCardTypeTickbox.SignalName.Toggled, callable2, 0U);
    ((GodotObject) this._otherTypeFilter).Connect(NCardTypeTickbox.SignalName.Toggled, callable2, 0U);
    this._cardTypeFilters.Add(this._attackFilter, (Func<CardModel, bool>) (c => c.Type == CardType.Attack));
    this._cardTypeFilters.Add(this._skillFilter, (Func<CardModel, bool>) (c => c.Type == CardType.Skill));
    this._cardTypeFilters.Add(this._powerFilter, (Func<CardModel, bool>) (c => c.Type == CardType.Power));
    this._cardTypeFilters.Add(this._otherTypeFilter, (Func<CardModel, bool>) (c =>
    {
      bool flag;
      switch (c.Type)
      {
        case CardType.Attack:
        case CardType.Skill:
        case CardType.Power:
          flag = true;
          break;
        default:
          flag = false;
          break;
      }
      return !flag;
    }));
    this._raritySorter = ((Node) this).GetNode<NCardViewSortButton>(NodePath.op_Implicit("%RaritySorter"));
    ((GodotObject) this._raritySorter).Connect(NClickableControl.SignalName.Released, Callable.From<NButton>(new Action<NButton>(this.OnRaritySort)), 0U);
    this._commonFilter = ((Node) this).GetNode<NCardRarityTickbox>(NodePath.op_Implicit("%CommonRarity"));
    this._uncommonFilter = ((Node) this).GetNode<NCardRarityTickbox>(NodePath.op_Implicit("%UncommonRarity"));
    this._rareFilter = ((Node) this).GetNode<NCardRarityTickbox>(NodePath.op_Implicit("%RareRarity"));
    this._otherFilter = ((Node) this).GetNode<NCardRarityTickbox>(NodePath.op_Implicit("%OtherRarity"));
    Callable callable3 = Callable.From<NTickbox>(new Action<NTickbox>(this.UpdateRarityFilter));
    ((GodotObject) this._commonFilter).Connect(NTickbox.SignalName.Toggled, callable3, 0U);
    ((GodotObject) this._uncommonFilter).Connect(NTickbox.SignalName.Toggled, callable3, 0U);
    ((GodotObject) this._rareFilter).Connect(NTickbox.SignalName.Toggled, callable3, 0U);
    ((GodotObject) this._otherFilter).Connect(NTickbox.SignalName.Toggled, callable3, 0U);
    this._rarityFilters.Add(this._commonFilter, (Func<CardModel, bool>) (c => c.Rarity == CardRarity.Common));
    this._rarityFilters.Add(this._uncommonFilter, (Func<CardModel, bool>) (c => c.Rarity == CardRarity.Uncommon));
    this._rarityFilters.Add(this._rareFilter, (Func<CardModel, bool>) (c => c.Rarity == CardRarity.Rare));
    this._rarityFilters.Add(this._otherFilter, (Func<CardModel, bool>) (c =>
    {
      bool flag;
      switch (c.Rarity)
      {
        case CardRarity.Common:
        case CardRarity.Uncommon:
        case CardRarity.Rare:
          flag = true;
          break;
        default:
          flag = false;
          break;
      }
      return !flag;
    }));
    this._costSorter = ((Node) this).GetNode<NCardViewSortButton>(NodePath.op_Implicit("%CostSorter"));
    ((GodotObject) this._costSorter).Connect(NClickableControl.SignalName.Released, Callable.From<NButton>(new Action<NButton>(this.OnCostSort)), 0U);
    this._zeroFilter = ((Node) this).GetNode<NCardCostTickbox>(NodePath.op_Implicit("%Cost0"));
    this._oneFilter = ((Node) this).GetNode<NCardCostTickbox>(NodePath.op_Implicit("%Cost1"));
    this._twoFilter = ((Node) this).GetNode<NCardCostTickbox>(NodePath.op_Implicit("%Cost2"));
    this._threePlusFilter = ((Node) this).GetNode<NCardCostTickbox>(NodePath.op_Implicit("%Cost3+"));
    this._xFilter = ((Node) this).GetNode<NCardCostTickbox>(NodePath.op_Implicit("%CostX"));
    Callable callable4 = Callable.From<NCardCostTickbox>(new Action<NCardCostTickbox>(this.UpdateCostFilter));
    ((GodotObject) this._zeroFilter).Connect(NClickableControl.SignalName.Released, callable4, 0U);
    ((GodotObject) this._oneFilter).Connect(NClickableControl.SignalName.Released, callable4, 0U);
    ((GodotObject) this._twoFilter).Connect(NClickableControl.SignalName.Released, callable4, 0U);
    ((GodotObject) this._threePlusFilter).Connect(NClickableControl.SignalName.Released, callable4, 0U);
    ((GodotObject) this._xFilter).Connect(NClickableControl.SignalName.Released, callable4, 0U);
    this._costFilters.Add(this._zeroFilter, (Func<CardModel, bool>) (c =>
    {
      CardEnergyCost energyCost = c.EnergyCost;
      return energyCost != null && energyCost.Canonical <= 0 && !energyCost.CostsX;
    }));
    this._costFilters.Add(this._oneFilter, (Func<CardModel, bool>) (c => c.EnergyCost.Canonical == 1));
    this._costFilters.Add(this._twoFilter, (Func<CardModel, bool>) (c => c.EnergyCost.Canonical == 2));
    this._costFilters.Add(this._threePlusFilter, (Func<CardModel, bool>) (c => c.EnergyCost.Canonical >= 3));
    this._costFilters.Add(this._xFilter, (Func<CardModel, bool>) (c => c.EnergyCost.CostsX || c.HasStarCostX));
    this._alphabetSorter = ((Node) this).GetNode<NCardViewSortButton>(NodePath.op_Implicit("%AlphabetSorter"));
    ((GodotObject) this._alphabetSorter).Connect(NClickableControl.SignalName.Released, Callable.From<NButton>(new Action<NButton>(this.OnAlphabetSort)), 0U);
    this._viewStats = ((Node) this).GetNode<NLibraryStatTickbox>(NodePath.op_Implicit("%Stats"));
    this._viewUpgrades = ((Node) this).GetNode<NLibraryStatTickbox>(NodePath.op_Implicit("%Upgrades"));
    this._viewMultiplayerCards = ((Node) this).GetNode<NLibraryStatTickbox>(NodePath.op_Implicit("%MultiplayerCards"));
    ((GodotObject) this._viewStats).Connect(NTickbox.SignalName.Toggled, Callable.From<NTickbox>(new Action<NTickbox>(this.ToggleShowStats)), 0U);
    ((GodotObject) this._viewUpgrades).Connect(NTickbox.SignalName.Toggled, Callable.From<NTickbox>(new Action<NTickbox>(this.ToggleShowUpgrades)), 0U);
    ((GodotObject) this._viewMultiplayerCards).Connect(NTickbox.SignalName.Toggled, Callable.From<NTickbox>(new Action<NTickbox>(this.ToggleFilterMultiplayerCards)), 0U);
    this._typeSorter.SetLabel(new LocString("gameplay_ui", "SORT_TYPE").GetRawText());
    this._raritySorter.SetLabel(new LocString("gameplay_ui", "SORT_RARITY").GetRawText());
    this._costSorter.SetLabel(new LocString("gameplay_ui", "SORT_COST").GetRawText());
    this._alphabetSorter.SetLabel(new LocString("gameplay_ui", "SORT_ALPHABET").GetRawText());
    this._commonFilter.SetLabel(new LocString("card_library", "RARITY_COMMON").GetRawText());
    this._uncommonFilter.SetLabel(new LocString("card_library", "RARITY_UNCOMMON").GetRawText());
    this._rareFilter.SetLabel(new LocString("card_library", "RARITY_RARE").GetRawText());
    this._otherFilter.SetLabel(new LocString("card_library", "RARITY_OTHER").GetRawText());
    this._viewStats.SetLabel(new LocString("card_library", "VIEW_STATS").GetRawText());
    this._viewUpgrades.SetLabel(new LocString("card_library", "VIEW_UPGRADES").GetRawText());
    this._viewMultiplayerCards.SetLabel(new LocString("card_library", "VIEW_MULTIPLAYER_CARDS").GetRawText());
    this._colorlessFilter.Loc = new LocString("card_library", "POOL_COLORLESS_TIP");
    this._ancientsFilter.Loc = new LocString("card_library", "POOL_ANCIENT_TIP");
    this._miscPoolFilter.Loc = new LocString("card_library", "POOL_MISC_TIP");
    this._attackFilter.Loc = new LocString("card_library", "TYPE_ATTACK_TIP");
    this._skillFilter.Loc = new LocString("card_library", "TYPE_SKILL_TIP");
    this._powerFilter.Loc = new LocString("card_library", "TYPE_POWER_TIP");
    this._otherTypeFilter.Loc = new LocString("card_library", "TYPE_OTHER_TIP");
    this._commonFilter.Loc = new LocString("card_library", "RARITY_COMMON_TIP");
    this._uncommonFilter.Loc = new LocString("card_library", "RARITY_UNCOMMON_TIP");
    this._rareFilter.Loc = new LocString("card_library", "RARITY_RARE_TIP");
    this._otherFilter.Loc = new LocString("card_library", "RARITY_OTHER_TIP");
    this._zeroFilter.Loc = new LocString("card_library", "COST_ZERO_TIP");
    this._oneFilter.Loc = new LocString("card_library", "COST_ONE_TIP");
    this._twoFilter.Loc = new LocString("card_library", "COST_TWO_TIP");
    this._threePlusFilter.Loc = new LocString("card_library", "COST_THREE_TIP");
    this._xFilter.Loc = new LocString("card_library", "COST_X_TIP");
    this._cardPoolFilters.Add((CharacterModel) ModelDb.Character<Ironclad>(), this._ironcladFilter);
    this._cardPoolFilters.Add((CharacterModel) ModelDb.Character<Silent>(), this._silentFilter);
    this._cardPoolFilters.Add((CharacterModel) ModelDb.Character<Defect>(), this._defectFilter);
    this._cardPoolFilters.Add((CharacterModel) ModelDb.Character<Necrobinder>(), this._necrobinderFilter);
    this._cardPoolFilters.Add((CharacterModel) ModelDb.Character<Regent>(), this._regentFilter);
    UnlockState stateFromProgress = SaveManager.Instance.GenerateUnlockStateFromProgress();
    foreach (KeyValuePair<CharacterModel, NCardPoolFilter> cardPoolFilter in this._cardPoolFilters)
      ((CanvasItem) cardPoolFilter.Value).Visible = stateFromProgress.Characters.Contains<CharacterModel>(cardPoolFilter.Key);
    foreach (CardRarity cardRarity in Enum.GetValues<CardRarity>())
    {
      CardRarity keyword = cardRarity;
      this._specialSearchbarKeywords.Add(keyword.ToString().ToLowerInvariant(), (Func<CardModel, bool>) (c => c.Rarity == keyword));
    }
    ((GodotObject) this._ironcladFilter).Connect(Control.SignalName.FocusEntered, Callable.From((Action) (() => this._lastHoveredControl = (Control) this._ironcladFilter)), 0U);
    ((GodotObject) this._silentFilter).Connect(Control.SignalName.FocusEntered, Callable.From((Action) (() => this._lastHoveredControl = (Control) this._silentFilter)), 0U);
    ((GodotObject) this._defectFilter).Connect(Control.SignalName.FocusEntered, Callable.From((Action) (() => this._lastHoveredControl = (Control) this._defectFilter)), 0U);
    ((GodotObject) this._regentFilter).Connect(Control.SignalName.FocusEntered, Callable.From((Action) (() => this._lastHoveredControl = (Control) this._regentFilter)), 0U);
    ((GodotObject) this._necrobinderFilter).Connect(Control.SignalName.FocusEntered, Callable.From((Action) (() => this._lastHoveredControl = (Control) this._necrobinderFilter)), 0U);
    ((GodotObject) this._colorlessFilter).Connect(Control.SignalName.FocusEntered, Callable.From((Action) (() => this._lastHoveredControl = (Control) this._colorlessFilter)), 0U);
    ((GodotObject) this._ancientsFilter).Connect(Control.SignalName.FocusEntered, Callable.From((Action) (() => this._lastHoveredControl = (Control) this._ancientsFilter)), 0U);
    ((GodotObject) this._miscPoolFilter).Connect(Control.SignalName.FocusEntered, Callable.From((Action) (() => this._lastHoveredControl = (Control) this._miscPoolFilter)), 0U);
    ((GodotObject) this._attackFilter).Connect(Control.SignalName.FocusEntered, Callable.From((Action) (() => this._lastHoveredControl = (Control) this._attackFilter)), 0U);
    ((GodotObject) this._skillFilter).Connect(Control.SignalName.FocusEntered, Callable.From((Action) (() => this._lastHoveredControl = (Control) this._skillFilter)), 0U);
    ((GodotObject) this._powerFilter).Connect(Control.SignalName.FocusEntered, Callable.From((Action) (() => this._lastHoveredControl = (Control) this._powerFilter)), 0U);
    ((GodotObject) this._otherTypeFilter).Connect(Control.SignalName.FocusEntered, Callable.From((Action) (() => this._lastHoveredControl = (Control) this._otherTypeFilter)), 0U);
    ((GodotObject) this._commonFilter).Connect(Control.SignalName.FocusEntered, Callable.From((Action) (() => this._lastHoveredControl = (Control) this._commonFilter)), 0U);
    ((GodotObject) this._uncommonFilter).Connect(Control.SignalName.FocusEntered, Callable.From((Action) (() => this._lastHoveredControl = (Control) this._uncommonFilter)), 0U);
    ((GodotObject) this._rareFilter).Connect(Control.SignalName.FocusEntered, Callable.From((Action) (() => this._lastHoveredControl = (Control) this._rareFilter)), 0U);
    ((GodotObject) this._otherFilter).Connect(Control.SignalName.FocusEntered, Callable.From((Action) (() => this._lastHoveredControl = (Control) this._otherFilter)), 0U);
    ((GodotObject) this._zeroFilter).Connect(Control.SignalName.FocusEntered, Callable.From((Action) (() => this._lastHoveredControl = (Control) this._zeroFilter)), 0U);
    ((GodotObject) this._oneFilter).Connect(Control.SignalName.FocusEntered, Callable.From((Action) (() => this._lastHoveredControl = (Control) this._oneFilter)), 0U);
    ((GodotObject) this._twoFilter).Connect(Control.SignalName.FocusEntered, Callable.From((Action) (() => this._lastHoveredControl = (Control) this._twoFilter)), 0U);
    ((GodotObject) this._threePlusFilter).Connect(Control.SignalName.FocusEntered, Callable.From((Action) (() => this._lastHoveredControl = (Control) this._threePlusFilter)), 0U);
    ((GodotObject) this._alphabetSorter).Connect(Control.SignalName.FocusEntered, Callable.From((Action) (() => this._lastHoveredControl = (Control) this._alphabetSorter)), 0U);
    ((GodotObject) this._viewUpgrades).Connect(Control.SignalName.FocusEntered, Callable.From((Action) (() => this._lastHoveredControl = (Control) this._viewUpgrades)), 0U);
  }

  public void Initialize(IRunState runState) => this._runState = runState;

  private void OnCardTypeSort(NButton button)
  {
    this._sortingPriority.Remove(SortingOrders.TypeAscending);
    this._sortingPriority.Remove(SortingOrders.TypeDescending);
    if (this._typeSorter.IsDescending)
      this._sortingPriority.Insert(0, SortingOrders.TypeDescending);
    else
      this._sortingPriority.Insert(0, SortingOrders.TypeAscending);
    TaskHelper.RunSafely(this.DisplayCards());
  }

  private void OnRaritySort(NButton button)
  {
    this._sortingPriority.Remove(SortingOrders.RarityAscending);
    this._sortingPriority.Remove(SortingOrders.RarityDescending);
    if (this._raritySorter.IsDescending)
      this._sortingPriority.Insert(0, SortingOrders.RarityAscending);
    else
      this._sortingPriority.Insert(0, SortingOrders.RarityDescending);
    TaskHelper.RunSafely(this.DisplayCards());
  }

  private void OnCostSort(NButton button)
  {
    this._sortingPriority.Remove(SortingOrders.CostAscending);
    this._sortingPriority.Remove(SortingOrders.CostDescending);
    if (this._costSorter.IsDescending)
      this._sortingPriority.Insert(0, SortingOrders.CostDescending);
    else
      this._sortingPriority.Insert(0, SortingOrders.CostAscending);
    TaskHelper.RunSafely(this.DisplayCards());
  }

  private void OnAlphabetSort(NButton button)
  {
    this._sortingPriority.Remove(SortingOrders.AlphabetAscending);
    this._sortingPriority.Remove(SortingOrders.AlphabetDescending);
    if (this._alphabetSorter.IsDescending)
      this._sortingPriority.Insert(0, SortingOrders.AlphabetDescending);
    else
      this._sortingPriority.Insert(0, SortingOrders.AlphabetAscending);
    TaskHelper.RunSafely(this.DisplayCards());
  }

  public override void OnSubmenuOpened()
  {
    this._grid.RefreshVisibility();
    CharacterModel character = LocalContext.GetMe((IPlayerCollection) this._runState)?.Character;
    this._searchBar.ClearText();
    if (character != null)
    {
      foreach (NCardPoolFilter key in this._poolFilters.Keys)
        key.IsSelected = this._cardPoolFilters[character] == key;
    }
    else
    {
      foreach (NCardPoolFilter key in this._poolFilters.Keys)
        key.IsSelected = key == this._ironcladFilter;
    }
    foreach (NCardTypeTickbox key in this._cardTypeFilters.Keys)
      key.IsTicked = false;
    foreach (NTickbox key in this._rarityFilters.Keys)
      key.IsTicked = false;
    foreach (NCardCostTickbox key in this._costFilters.Keys)
      key.IsTicked = false;
    this._typeSorter.IsDescending = true;
    this._raritySorter.IsDescending = true;
    this._costSorter.IsDescending = true;
    this._alphabetSorter.IsDescending = true;
    this._viewUpgrades.IsTicked = false;
    this._viewStats.IsTicked = false;
    this._viewMultiplayerCards.IsTicked = true;
    this.ToggleShowStats((NTickbox) this._viewStats);
    this.ToggleShowUpgrades((NTickbox) this._viewUpgrades);
    this.UpdateFilter();
  }

  public override void OnSubmenuClosed() => this._grid.ClearGrid();

  private async Task DisplayCardsAfterShortDelay()
  {
    if (this._displayCardsShortDelayCancelToken != null)
      await this._displayCardsShortDelayCancelToken.CancelAsync();
    if (!this._grid.IsAnimatingOut)
      TaskHelper.RunSafely(this._grid.AnimateOut());
    CancellationTokenSource cancelToken = new CancellationTokenSource();
    this._displayCardsShortDelayCancelToken = cancelToken;
    await Task.Delay(250, cancelToken.Token);
    if (cancelToken.IsCancellationRequested)
    {
      cancelToken = (CancellationTokenSource) null;
    }
    else
    {
      await this.DisplayCards();
      cancelToken = (CancellationTokenSource) null;
    }
  }

  private async Task DisplayCards()
  {
    if (this._displayCardsShortDelayCancelToken != null)
      await this._displayCardsShortDelayCancelToken.CancelAsync();
    await Task.Yield();
    this._grid.FilterCards(this._filter, this._sortingPriority);
    this._cardCountLocString.Add("Amount", (Decimal) this._grid.VisibleCards.Count<CardModel>());
    this._cardCountLabel.Text = $"[center]{this._cardCountLocString.GetFormattedText()}[/center]";
    ((CanvasItem) this._noResultsLabel).Visible = !this._grid.VisibleCards.Any<CardModel>();
  }

  private void ToggleShowStats(NTickbox tickbox) => this._grid.ShowStats = tickbox.IsTicked;

  private void ToggleShowUpgrades(NTickbox tickbox)
  {
    this._grid.IsShowingUpgrades = tickbox.IsTicked;
    if (string.IsNullOrWhiteSpace(this._searchBar.Text))
      return;
    this.UpdateFilter();
  }

  private void ToggleFilterMultiplayerCards(NTickbox tickbox) => this.UpdateFilter();

  private void UpdateCardPoolFilter(NCardPoolFilter filter)
  {
    if (filter.IsSelected)
    {
      foreach (NCardPoolFilter key in this._poolFilters.Keys)
      {
        if (key != filter)
          key.IsSelected = false;
      }
    }
    bool flag = true;
    foreach (KeyValuePair<NCardPoolFilter, Func<CardModel, bool>> poolFilter in this._poolFilters)
    {
      NCardPoolFilter key = poolFilter.Key;
      if (key.IsSelected && key != this._miscPoolFilter && key != this._ancientsFilter)
      {
        flag = false;
        break;
      }
    }
    foreach (NCardRarityTickbox key in this._rarityFilters.Keys)
    {
      if (flag)
        key.Disable();
      else
        key.Enable();
    }
    this.UpdateFilter();
  }

  private void UpdateTypeFilter(NCardTypeTickbox tickbox) => this.UpdateFilter();

  private void UpdateRarityFilter(NTickbox tickbox) => this.UpdateFilter();

  private void UpdateCostFilter(NCardCostTickbox tickbox) => this.UpdateFilter();

  private void SearchBarQueryChanged(string _ = "") => this.UpdateFilter(true);

  private void SearchBarQuerySubmitted(string _ = "") => this.UpdateFilter();

  private void UpdateFilter(bool isTextInput = false)
  {
    List<Func<CardModel, bool>> activeRarityFilters = new List<Func<CardModel, bool>>();
    bool flag = true;
    foreach (KeyValuePair<NCardPoolFilter, Func<CardModel, bool>> poolFilter1 in this._poolFilters)
    {
      if (poolFilter1.Key.IsSelected && poolFilter1.Key != this._miscPoolFilter && poolFilter1.Key != this._ancientsFilter)
      {
        flag = false;
        break;
      }
    }
    Func<CardModel, bool> func1;
    if (!flag)
    {
      foreach (KeyValuePair<NCardRarityTickbox, Func<CardModel, bool>> rarityFilter in this._rarityFilters)
      {
        NCardRarityTickbox ncardRarityTickbox;
        rarityFilter.Deconstruct(ref ncardRarityTickbox, ref func1);
        NTickbox ntickbox = (NTickbox) ncardRarityTickbox;
        Func<CardModel, bool> func2 = func1;
        if (ntickbox.IsTicked)
          activeRarityFilters.Add(func2);
      }
    }
    if (activeRarityFilters.Count == 0)
      activeRarityFilters.Add((Func<CardModel, bool>) (_ => true));
    List<Func<CardModel, bool>> activeCardTypeFilter = new List<Func<CardModel, bool>>();
    foreach (KeyValuePair<NCardTypeTickbox, Func<CardModel, bool>> cardTypeFilter in this._cardTypeFilters)
    {
      NCardTypeTickbox ncardTypeTickbox1;
      cardTypeFilter.Deconstruct(ref ncardTypeTickbox1, ref func1);
      NCardTypeTickbox ncardTypeTickbox2 = ncardTypeTickbox1;
      Func<CardModel, bool> func3 = func1;
      if (ncardTypeTickbox2.IsTicked)
        activeCardTypeFilter.Add(func3);
    }
    if (activeCardTypeFilter.Count == 0)
      activeCardTypeFilter.Add((Func<CardModel, bool>) (_ => true));
    List<Func<CardModel, bool>> poolFilter = new List<Func<CardModel, bool>>();
    foreach (KeyValuePair<NCardPoolFilter, Func<CardModel, bool>> poolFilter2 in this._poolFilters)
    {
      if (poolFilter2.Key.IsSelected)
        poolFilter.Add(poolFilter2.Value);
    }
    List<Func<CardModel, bool>> activeCostFilter = new List<Func<CardModel, bool>>();
    foreach (KeyValuePair<NCardCostTickbox, Func<CardModel, bool>> costFilter in this._costFilters)
    {
      NCardCostTickbox ncardCostTickbox1;
      costFilter.Deconstruct(ref ncardCostTickbox1, ref func1);
      NCardCostTickbox ncardCostTickbox2 = ncardCostTickbox1;
      Func<CardModel, bool> func4 = func1;
      if (ncardCostTickbox2.IsTicked)
        activeCostFilter.Add(func4);
    }
    if (activeCostFilter.Count == 0)
      activeCostFilter.Add((Func<CardModel, bool>) (_ => true));
    Func<CardModel, bool> multiplayerCardFilter = (Func<CardModel, bool>) (c => true);
    if (!this._viewMultiplayerCards.IsTicked)
      multiplayerCardFilter = (Func<CardModel, bool>) (c => c.MultiplayerConstraint != CardMultiplayerConstraint.MultiplayerOnly);
    this._filter = (Func<CardModel, bool>) (c => activeCostFilter.Any<Func<CardModel, bool>>((Func<Func<CardModel, bool>, bool>) (filter => filter(c))) && activeRarityFilters.Any<Func<CardModel, bool>>((Func<Func<CardModel, bool>, bool>) (filter => filter(c))) && activeCardTypeFilter.Any<Func<CardModel, bool>>((Func<Func<CardModel, bool>, bool>) (filter => filter(c))) && poolFilter.Any<Func<CardModel, bool>>((Func<Func<CardModel, bool>, bool>) (filter => filter(c))) && TextFilter(c) && multiplayerCardFilter(c));
    TaskHelper.RunSafely(!isTextInput ? this.DisplayCards() : this.DisplayCardsAfterShortDelay());

    bool TextFilter(CardModel card)
    {
      if (string.IsNullOrWhiteSpace(this._searchBar.Text))
        return true;
      if (!SaveManager.Instance.Progress.DiscoveredCards.Contains(card.Id))
        return false;
      string title = card.Title;
      string text;
      if (this._viewUpgrades.IsTicked && card.IsUpgradable)
      {
        CardModel cardModel = (CardModel) card.MutableClone();
        cardModel.UpgradeInternal();
        cardModel.UpdateDynamicVarPreview(CardPreviewMode.Upgrade, (Creature) null, card.DynamicVars);
        text = cardModel.GetDescriptionForUpgradePreview().StripBbCode();
      }
      else
        text = card.GetDescriptionForPile(PileType.None).StripBbCode();
      \u003C\u003Ey__InlineArray2<string> buffer = new \u003C\u003Ey__InlineArray2<string>();
      // ISSUE: reference to a compiler-generated method
      \u003CPrivateImplementationDetails\u003E.InlineArrayElementRef<\u003C\u003Ey__InlineArray2<string>, string>(ref buffer, 0) = title;
      // ISSUE: reference to a compiler-generated method
      \u003CPrivateImplementationDetails\u003E.InlineArrayElementRef<\u003C\u003Ey__InlineArray2<string>, string>(ref buffer, 1) = NSearchBar.RemoveHtmlTags(text);
      // ISSUE: reference to a compiler-generated method
      string str = NSearchBar.Normalize(string.Join(" ", \u003CPrivateImplementationDetails\u003E.InlineArrayAsReadOnlySpan<\u003C\u003Ey__InlineArray2<string>, string>(in buffer, 2)));
      string lowerInvariant = this._searchBar.Text.ToLowerInvariant();
      Func<CardModel, bool> func;
      return this._specialSearchbarKeywords.TryGetValue(lowerInvariant, out func) && func(card) || str.Contains(lowerInvariant);
    }
  }

  private void ShowCardDetail(NCardHolder holder)
  {
    if (!SaveManager.Instance.Progress.DiscoveredCards.Contains(holder.CardModel.Id))
      return;
    this._lastHoveredControl = (Control) holder;
    List<CardModel> list = this._grid.VisibleCards.Where<CardModel>((Func<CardModel, bool>) (c => SaveManager.Instance.Progress.DiscoveredCards.Contains(c.Id))).ToList<CardModel>();
    NGame.Instance.GetInspectCardScreen().Open(list, list.IndexOf(holder.CardModel), this._viewUpgrades.IsTicked);
  }

  protected override Control InitialFocusedControl
  {
    get => this._lastHoveredControl ?? (Control) this._ironcladFilter;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(19)
    {
      new MethodInfo(NCardLibrary.MethodName.Create, new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false), (MethodFlags) 33L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCardLibrary.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCardLibrary.MethodName.OnCardTypeSort, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("button"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false)
      }, (List<Variant>) null),
      new MethodInfo(NCardLibrary.MethodName.OnRaritySort, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("button"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false)
      }, (List<Variant>) null),
      new MethodInfo(NCardLibrary.MethodName.OnCostSort, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("button"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false)
      }, (List<Variant>) null),
      new MethodInfo(NCardLibrary.MethodName.OnAlphabetSort, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("button"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false)
      }, (List<Variant>) null),
      new MethodInfo(NCardLibrary.MethodName.OnSubmenuOpened, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCardLibrary.MethodName.OnSubmenuClosed, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCardLibrary.MethodName.ToggleShowStats, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("tickbox"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false)
      }, (List<Variant>) null),
      new MethodInfo(NCardLibrary.MethodName.ToggleShowUpgrades, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("tickbox"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false)
      }, (List<Variant>) null),
      new MethodInfo(NCardLibrary.MethodName.ToggleFilterMultiplayerCards, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("tickbox"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false)
      }, (List<Variant>) null),
      new MethodInfo(NCardLibrary.MethodName.UpdateCardPoolFilter, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("filter"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false)
      }, (List<Variant>) null),
      new MethodInfo(NCardLibrary.MethodName.UpdateTypeFilter, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("tickbox"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false)
      }, (List<Variant>) null),
      new MethodInfo(NCardLibrary.MethodName.UpdateRarityFilter, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("tickbox"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false)
      }, (List<Variant>) null),
      new MethodInfo(NCardLibrary.MethodName.UpdateCostFilter, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("tickbox"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false)
      }, (List<Variant>) null),
      new MethodInfo(NCardLibrary.MethodName.SearchBarQueryChanged, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 4L, StringName.op_Implicit("_"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NCardLibrary.MethodName.SearchBarQuerySubmitted, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 4L, StringName.op_Implicit("_"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NCardLibrary.MethodName.UpdateFilter, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 1L, StringName.op_Implicit("isTextInput"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NCardLibrary.MethodName.ShowCardDetail, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
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
    if (StringName.op_Equality(ref method, NCardLibrary.MethodName.Create) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      NCardLibrary ncardLibrary = NCardLibrary.Create();
      ret = VariantUtils.CreateFrom<NCardLibrary>(ref ncardLibrary);
      return true;
    }
    if (StringName.op_Equality(ref method, NCardLibrary.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCardLibrary.MethodName.OnCardTypeSort) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.OnCardTypeSort(VariantUtils.ConvertTo<NButton>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCardLibrary.MethodName.OnRaritySort) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.OnRaritySort(VariantUtils.ConvertTo<NButton>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCardLibrary.MethodName.OnCostSort) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.OnCostSort(VariantUtils.ConvertTo<NButton>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCardLibrary.MethodName.OnAlphabetSort) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.OnAlphabetSort(VariantUtils.ConvertTo<NButton>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCardLibrary.MethodName.OnSubmenuOpened) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnSubmenuOpened();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCardLibrary.MethodName.OnSubmenuClosed) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnSubmenuClosed();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCardLibrary.MethodName.ToggleShowStats) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.ToggleShowStats(VariantUtils.ConvertTo<NTickbox>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCardLibrary.MethodName.ToggleShowUpgrades) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.ToggleShowUpgrades(VariantUtils.ConvertTo<NTickbox>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCardLibrary.MethodName.ToggleFilterMultiplayerCards) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.ToggleFilterMultiplayerCards(VariantUtils.ConvertTo<NTickbox>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCardLibrary.MethodName.UpdateCardPoolFilter) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.UpdateCardPoolFilter(VariantUtils.ConvertTo<NCardPoolFilter>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCardLibrary.MethodName.UpdateTypeFilter) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.UpdateTypeFilter(VariantUtils.ConvertTo<NCardTypeTickbox>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCardLibrary.MethodName.UpdateRarityFilter) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.UpdateRarityFilter(VariantUtils.ConvertTo<NTickbox>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCardLibrary.MethodName.UpdateCostFilter) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.UpdateCostFilter(VariantUtils.ConvertTo<NCardCostTickbox>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCardLibrary.MethodName.SearchBarQueryChanged) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.SearchBarQueryChanged(VariantUtils.ConvertTo<string>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCardLibrary.MethodName.SearchBarQuerySubmitted) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.SearchBarQuerySubmitted(VariantUtils.ConvertTo<string>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCardLibrary.MethodName.UpdateFilter) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.UpdateFilter(VariantUtils.ConvertTo<bool>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NCardLibrary.MethodName.ShowCardDetail) || ((NativeVariantPtrArgs) ref args).Count != 1)
      return base.InvokeGodotClassMethod(in method, args, out ret);
    this.ShowCardDetail(VariantUtils.ConvertTo<NCardHolder>(ref ((NativeVariantPtrArgs) ref args)[0]));
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static bool InvokeGodotClassStaticMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NCardLibrary.MethodName.Create) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      NCardLibrary ncardLibrary = NCardLibrary.Create();
      ret = VariantUtils.CreateFrom<NCardLibrary>(ref ncardLibrary);
      return true;
    }
    ret = new godot_variant();
    return false;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NCardLibrary.MethodName.Create) || StringName.op_Equality(ref method, NCardLibrary.MethodName._Ready) || StringName.op_Equality(ref method, NCardLibrary.MethodName.OnCardTypeSort) || StringName.op_Equality(ref method, NCardLibrary.MethodName.OnRaritySort) || StringName.op_Equality(ref method, NCardLibrary.MethodName.OnCostSort) || StringName.op_Equality(ref method, NCardLibrary.MethodName.OnAlphabetSort) || StringName.op_Equality(ref method, NCardLibrary.MethodName.OnSubmenuOpened) || StringName.op_Equality(ref method, NCardLibrary.MethodName.OnSubmenuClosed) || StringName.op_Equality(ref method, NCardLibrary.MethodName.ToggleShowStats) || StringName.op_Equality(ref method, NCardLibrary.MethodName.ToggleShowUpgrades) || StringName.op_Equality(ref method, NCardLibrary.MethodName.ToggleFilterMultiplayerCards) || StringName.op_Equality(ref method, NCardLibrary.MethodName.UpdateCardPoolFilter) || StringName.op_Equality(ref method, NCardLibrary.MethodName.UpdateTypeFilter) || StringName.op_Equality(ref method, NCardLibrary.MethodName.UpdateRarityFilter) || StringName.op_Equality(ref method, NCardLibrary.MethodName.UpdateCostFilter) || StringName.op_Equality(ref method, NCardLibrary.MethodName.SearchBarQueryChanged) || StringName.op_Equality(ref method, NCardLibrary.MethodName.SearchBarQuerySubmitted) || StringName.op_Equality(ref method, NCardLibrary.MethodName.UpdateFilter) || StringName.op_Equality(ref method, NCardLibrary.MethodName.ShowCardDetail) || base.HasGodotClassMethod(in method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NCardLibrary.PropertyName._grid))
    {
      this._grid = VariantUtils.ConvertTo<NCardLibraryGrid>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCardLibrary.PropertyName._searchBar))
    {
      this._searchBar = VariantUtils.ConvertTo<NSearchBar>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCardLibrary.PropertyName._ironcladFilter))
    {
      this._ironcladFilter = VariantUtils.ConvertTo<NCardPoolFilter>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCardLibrary.PropertyName._silentFilter))
    {
      this._silentFilter = VariantUtils.ConvertTo<NCardPoolFilter>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCardLibrary.PropertyName._defectFilter))
    {
      this._defectFilter = VariantUtils.ConvertTo<NCardPoolFilter>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCardLibrary.PropertyName._regentFilter))
    {
      this._regentFilter = VariantUtils.ConvertTo<NCardPoolFilter>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCardLibrary.PropertyName._necrobinderFilter))
    {
      this._necrobinderFilter = VariantUtils.ConvertTo<NCardPoolFilter>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCardLibrary.PropertyName._colorlessFilter))
    {
      this._colorlessFilter = VariantUtils.ConvertTo<NCardPoolFilter>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCardLibrary.PropertyName._ancientsFilter))
    {
      this._ancientsFilter = VariantUtils.ConvertTo<NCardPoolFilter>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCardLibrary.PropertyName._miscPoolFilter))
    {
      this._miscPoolFilter = VariantUtils.ConvertTo<NCardPoolFilter>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCardLibrary.PropertyName._typeSorter))
    {
      this._typeSorter = VariantUtils.ConvertTo<NCardViewSortButton>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCardLibrary.PropertyName._attackFilter))
    {
      this._attackFilter = VariantUtils.ConvertTo<NCardTypeTickbox>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCardLibrary.PropertyName._skillFilter))
    {
      this._skillFilter = VariantUtils.ConvertTo<NCardTypeTickbox>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCardLibrary.PropertyName._powerFilter))
    {
      this._powerFilter = VariantUtils.ConvertTo<NCardTypeTickbox>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCardLibrary.PropertyName._otherTypeFilter))
    {
      this._otherTypeFilter = VariantUtils.ConvertTo<NCardTypeTickbox>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCardLibrary.PropertyName._raritySorter))
    {
      this._raritySorter = VariantUtils.ConvertTo<NCardViewSortButton>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCardLibrary.PropertyName._commonFilter))
    {
      this._commonFilter = VariantUtils.ConvertTo<NCardRarityTickbox>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCardLibrary.PropertyName._uncommonFilter))
    {
      this._uncommonFilter = VariantUtils.ConvertTo<NCardRarityTickbox>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCardLibrary.PropertyName._rareFilter))
    {
      this._rareFilter = VariantUtils.ConvertTo<NCardRarityTickbox>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCardLibrary.PropertyName._otherFilter))
    {
      this._otherFilter = VariantUtils.ConvertTo<NCardRarityTickbox>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCardLibrary.PropertyName._costSorter))
    {
      this._costSorter = VariantUtils.ConvertTo<NCardViewSortButton>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCardLibrary.PropertyName._zeroFilter))
    {
      this._zeroFilter = VariantUtils.ConvertTo<NCardCostTickbox>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCardLibrary.PropertyName._oneFilter))
    {
      this._oneFilter = VariantUtils.ConvertTo<NCardCostTickbox>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCardLibrary.PropertyName._twoFilter))
    {
      this._twoFilter = VariantUtils.ConvertTo<NCardCostTickbox>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCardLibrary.PropertyName._threePlusFilter))
    {
      this._threePlusFilter = VariantUtils.ConvertTo<NCardCostTickbox>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCardLibrary.PropertyName._xFilter))
    {
      this._xFilter = VariantUtils.ConvertTo<NCardCostTickbox>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCardLibrary.PropertyName._alphabetSorter))
    {
      this._alphabetSorter = VariantUtils.ConvertTo<NCardViewSortButton>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCardLibrary.PropertyName._viewMultiplayerCards))
    {
      this._viewMultiplayerCards = VariantUtils.ConvertTo<NLibraryStatTickbox>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCardLibrary.PropertyName._viewStats))
    {
      this._viewStats = VariantUtils.ConvertTo<NLibraryStatTickbox>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCardLibrary.PropertyName._viewUpgrades))
    {
      this._viewUpgrades = VariantUtils.ConvertTo<NLibraryStatTickbox>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCardLibrary.PropertyName._cardCountLabel))
    {
      this._cardCountLabel = VariantUtils.ConvertTo<MegaRichTextLabel>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCardLibrary.PropertyName._noResultsLabel))
    {
      this._noResultsLabel = VariantUtils.ConvertTo<MegaRichTextLabel>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NCardLibrary.PropertyName._lastHoveredControl))
      return base.SetGodotClassPropertyValue(in name, in value);
    this._lastHoveredControl = VariantUtils.ConvertTo<Control>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NCardLibrary.PropertyName.InitialFocusedControl))
    {
      ref godot_variant local = ref value;
      Control initialFocusedControl = this.InitialFocusedControl;
      godot_variant from = VariantUtils.CreateFrom<Control>(ref initialFocusedControl);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NCardLibrary.PropertyName._grid))
    {
      value = VariantUtils.CreateFrom<NCardLibraryGrid>(ref this._grid);
      return true;
    }
    if (StringName.op_Equality(ref name, NCardLibrary.PropertyName._searchBar))
    {
      value = VariantUtils.CreateFrom<NSearchBar>(ref this._searchBar);
      return true;
    }
    if (StringName.op_Equality(ref name, NCardLibrary.PropertyName._ironcladFilter))
    {
      value = VariantUtils.CreateFrom<NCardPoolFilter>(ref this._ironcladFilter);
      return true;
    }
    if (StringName.op_Equality(ref name, NCardLibrary.PropertyName._silentFilter))
    {
      value = VariantUtils.CreateFrom<NCardPoolFilter>(ref this._silentFilter);
      return true;
    }
    if (StringName.op_Equality(ref name, NCardLibrary.PropertyName._defectFilter))
    {
      value = VariantUtils.CreateFrom<NCardPoolFilter>(ref this._defectFilter);
      return true;
    }
    if (StringName.op_Equality(ref name, NCardLibrary.PropertyName._regentFilter))
    {
      value = VariantUtils.CreateFrom<NCardPoolFilter>(ref this._regentFilter);
      return true;
    }
    if (StringName.op_Equality(ref name, NCardLibrary.PropertyName._necrobinderFilter))
    {
      value = VariantUtils.CreateFrom<NCardPoolFilter>(ref this._necrobinderFilter);
      return true;
    }
    if (StringName.op_Equality(ref name, NCardLibrary.PropertyName._colorlessFilter))
    {
      value = VariantUtils.CreateFrom<NCardPoolFilter>(ref this._colorlessFilter);
      return true;
    }
    if (StringName.op_Equality(ref name, NCardLibrary.PropertyName._ancientsFilter))
    {
      value = VariantUtils.CreateFrom<NCardPoolFilter>(ref this._ancientsFilter);
      return true;
    }
    if (StringName.op_Equality(ref name, NCardLibrary.PropertyName._miscPoolFilter))
    {
      value = VariantUtils.CreateFrom<NCardPoolFilter>(ref this._miscPoolFilter);
      return true;
    }
    if (StringName.op_Equality(ref name, NCardLibrary.PropertyName._typeSorter))
    {
      value = VariantUtils.CreateFrom<NCardViewSortButton>(ref this._typeSorter);
      return true;
    }
    if (StringName.op_Equality(ref name, NCardLibrary.PropertyName._attackFilter))
    {
      value = VariantUtils.CreateFrom<NCardTypeTickbox>(ref this._attackFilter);
      return true;
    }
    if (StringName.op_Equality(ref name, NCardLibrary.PropertyName._skillFilter))
    {
      value = VariantUtils.CreateFrom<NCardTypeTickbox>(ref this._skillFilter);
      return true;
    }
    if (StringName.op_Equality(ref name, NCardLibrary.PropertyName._powerFilter))
    {
      value = VariantUtils.CreateFrom<NCardTypeTickbox>(ref this._powerFilter);
      return true;
    }
    if (StringName.op_Equality(ref name, NCardLibrary.PropertyName._otherTypeFilter))
    {
      value = VariantUtils.CreateFrom<NCardTypeTickbox>(ref this._otherTypeFilter);
      return true;
    }
    if (StringName.op_Equality(ref name, NCardLibrary.PropertyName._raritySorter))
    {
      value = VariantUtils.CreateFrom<NCardViewSortButton>(ref this._raritySorter);
      return true;
    }
    if (StringName.op_Equality(ref name, NCardLibrary.PropertyName._commonFilter))
    {
      value = VariantUtils.CreateFrom<NCardRarityTickbox>(ref this._commonFilter);
      return true;
    }
    if (StringName.op_Equality(ref name, NCardLibrary.PropertyName._uncommonFilter))
    {
      value = VariantUtils.CreateFrom<NCardRarityTickbox>(ref this._uncommonFilter);
      return true;
    }
    if (StringName.op_Equality(ref name, NCardLibrary.PropertyName._rareFilter))
    {
      value = VariantUtils.CreateFrom<NCardRarityTickbox>(ref this._rareFilter);
      return true;
    }
    if (StringName.op_Equality(ref name, NCardLibrary.PropertyName._otherFilter))
    {
      value = VariantUtils.CreateFrom<NCardRarityTickbox>(ref this._otherFilter);
      return true;
    }
    if (StringName.op_Equality(ref name, NCardLibrary.PropertyName._costSorter))
    {
      value = VariantUtils.CreateFrom<NCardViewSortButton>(ref this._costSorter);
      return true;
    }
    if (StringName.op_Equality(ref name, NCardLibrary.PropertyName._zeroFilter))
    {
      value = VariantUtils.CreateFrom<NCardCostTickbox>(ref this._zeroFilter);
      return true;
    }
    if (StringName.op_Equality(ref name, NCardLibrary.PropertyName._oneFilter))
    {
      value = VariantUtils.CreateFrom<NCardCostTickbox>(ref this._oneFilter);
      return true;
    }
    if (StringName.op_Equality(ref name, NCardLibrary.PropertyName._twoFilter))
    {
      value = VariantUtils.CreateFrom<NCardCostTickbox>(ref this._twoFilter);
      return true;
    }
    if (StringName.op_Equality(ref name, NCardLibrary.PropertyName._threePlusFilter))
    {
      value = VariantUtils.CreateFrom<NCardCostTickbox>(ref this._threePlusFilter);
      return true;
    }
    if (StringName.op_Equality(ref name, NCardLibrary.PropertyName._xFilter))
    {
      value = VariantUtils.CreateFrom<NCardCostTickbox>(ref this._xFilter);
      return true;
    }
    if (StringName.op_Equality(ref name, NCardLibrary.PropertyName._alphabetSorter))
    {
      value = VariantUtils.CreateFrom<NCardViewSortButton>(ref this._alphabetSorter);
      return true;
    }
    if (StringName.op_Equality(ref name, NCardLibrary.PropertyName._viewMultiplayerCards))
    {
      value = VariantUtils.CreateFrom<NLibraryStatTickbox>(ref this._viewMultiplayerCards);
      return true;
    }
    if (StringName.op_Equality(ref name, NCardLibrary.PropertyName._viewStats))
    {
      value = VariantUtils.CreateFrom<NLibraryStatTickbox>(ref this._viewStats);
      return true;
    }
    if (StringName.op_Equality(ref name, NCardLibrary.PropertyName._viewUpgrades))
    {
      value = VariantUtils.CreateFrom<NLibraryStatTickbox>(ref this._viewUpgrades);
      return true;
    }
    if (StringName.op_Equality(ref name, NCardLibrary.PropertyName._cardCountLabel))
    {
      value = VariantUtils.CreateFrom<MegaRichTextLabel>(ref this._cardCountLabel);
      return true;
    }
    if (StringName.op_Equality(ref name, NCardLibrary.PropertyName._noResultsLabel))
    {
      value = VariantUtils.CreateFrom<MegaRichTextLabel>(ref this._noResultsLabel);
      return true;
    }
    if (!StringName.op_Equality(ref name, NCardLibrary.PropertyName._lastHoveredControl))
      return base.GetGodotClassPropertyValue(in name, out value);
    value = VariantUtils.CreateFrom<Control>(ref this._lastHoveredControl);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NCardLibrary.PropertyName._grid, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NCardLibrary.PropertyName._searchBar, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NCardLibrary.PropertyName._ironcladFilter, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NCardLibrary.PropertyName._silentFilter, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NCardLibrary.PropertyName._defectFilter, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NCardLibrary.PropertyName._regentFilter, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NCardLibrary.PropertyName._necrobinderFilter, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NCardLibrary.PropertyName._colorlessFilter, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NCardLibrary.PropertyName._ancientsFilter, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NCardLibrary.PropertyName._miscPoolFilter, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NCardLibrary.PropertyName._typeSorter, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NCardLibrary.PropertyName._attackFilter, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NCardLibrary.PropertyName._skillFilter, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NCardLibrary.PropertyName._powerFilter, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NCardLibrary.PropertyName._otherTypeFilter, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NCardLibrary.PropertyName._raritySorter, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NCardLibrary.PropertyName._commonFilter, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NCardLibrary.PropertyName._uncommonFilter, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NCardLibrary.PropertyName._rareFilter, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NCardLibrary.PropertyName._otherFilter, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NCardLibrary.PropertyName._costSorter, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NCardLibrary.PropertyName._zeroFilter, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NCardLibrary.PropertyName._oneFilter, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NCardLibrary.PropertyName._twoFilter, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NCardLibrary.PropertyName._threePlusFilter, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NCardLibrary.PropertyName._xFilter, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NCardLibrary.PropertyName._alphabetSorter, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NCardLibrary.PropertyName._viewMultiplayerCards, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NCardLibrary.PropertyName._viewStats, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NCardLibrary.PropertyName._viewUpgrades, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NCardLibrary.PropertyName._cardCountLabel, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NCardLibrary.PropertyName._noResultsLabel, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NCardLibrary.PropertyName._lastHoveredControl, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NCardLibrary.PropertyName.InitialFocusedControl, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    base.SaveGodotObjectData(info);
    info.AddProperty(NCardLibrary.PropertyName._grid, Variant.From<NCardLibraryGrid>(ref this._grid));
    info.AddProperty(NCardLibrary.PropertyName._searchBar, Variant.From<NSearchBar>(ref this._searchBar));
    info.AddProperty(NCardLibrary.PropertyName._ironcladFilter, Variant.From<NCardPoolFilter>(ref this._ironcladFilter));
    info.AddProperty(NCardLibrary.PropertyName._silentFilter, Variant.From<NCardPoolFilter>(ref this._silentFilter));
    info.AddProperty(NCardLibrary.PropertyName._defectFilter, Variant.From<NCardPoolFilter>(ref this._defectFilter));
    info.AddProperty(NCardLibrary.PropertyName._regentFilter, Variant.From<NCardPoolFilter>(ref this._regentFilter));
    info.AddProperty(NCardLibrary.PropertyName._necrobinderFilter, Variant.From<NCardPoolFilter>(ref this._necrobinderFilter));
    info.AddProperty(NCardLibrary.PropertyName._colorlessFilter, Variant.From<NCardPoolFilter>(ref this._colorlessFilter));
    info.AddProperty(NCardLibrary.PropertyName._ancientsFilter, Variant.From<NCardPoolFilter>(ref this._ancientsFilter));
    info.AddProperty(NCardLibrary.PropertyName._miscPoolFilter, Variant.From<NCardPoolFilter>(ref this._miscPoolFilter));
    info.AddProperty(NCardLibrary.PropertyName._typeSorter, Variant.From<NCardViewSortButton>(ref this._typeSorter));
    info.AddProperty(NCardLibrary.PropertyName._attackFilter, Variant.From<NCardTypeTickbox>(ref this._attackFilter));
    info.AddProperty(NCardLibrary.PropertyName._skillFilter, Variant.From<NCardTypeTickbox>(ref this._skillFilter));
    info.AddProperty(NCardLibrary.PropertyName._powerFilter, Variant.From<NCardTypeTickbox>(ref this._powerFilter));
    info.AddProperty(NCardLibrary.PropertyName._otherTypeFilter, Variant.From<NCardTypeTickbox>(ref this._otherTypeFilter));
    info.AddProperty(NCardLibrary.PropertyName._raritySorter, Variant.From<NCardViewSortButton>(ref this._raritySorter));
    info.AddProperty(NCardLibrary.PropertyName._commonFilter, Variant.From<NCardRarityTickbox>(ref this._commonFilter));
    info.AddProperty(NCardLibrary.PropertyName._uncommonFilter, Variant.From<NCardRarityTickbox>(ref this._uncommonFilter));
    info.AddProperty(NCardLibrary.PropertyName._rareFilter, Variant.From<NCardRarityTickbox>(ref this._rareFilter));
    info.AddProperty(NCardLibrary.PropertyName._otherFilter, Variant.From<NCardRarityTickbox>(ref this._otherFilter));
    info.AddProperty(NCardLibrary.PropertyName._costSorter, Variant.From<NCardViewSortButton>(ref this._costSorter));
    info.AddProperty(NCardLibrary.PropertyName._zeroFilter, Variant.From<NCardCostTickbox>(ref this._zeroFilter));
    info.AddProperty(NCardLibrary.PropertyName._oneFilter, Variant.From<NCardCostTickbox>(ref this._oneFilter));
    info.AddProperty(NCardLibrary.PropertyName._twoFilter, Variant.From<NCardCostTickbox>(ref this._twoFilter));
    info.AddProperty(NCardLibrary.PropertyName._threePlusFilter, Variant.From<NCardCostTickbox>(ref this._threePlusFilter));
    info.AddProperty(NCardLibrary.PropertyName._xFilter, Variant.From<NCardCostTickbox>(ref this._xFilter));
    info.AddProperty(NCardLibrary.PropertyName._alphabetSorter, Variant.From<NCardViewSortButton>(ref this._alphabetSorter));
    info.AddProperty(NCardLibrary.PropertyName._viewMultiplayerCards, Variant.From<NLibraryStatTickbox>(ref this._viewMultiplayerCards));
    info.AddProperty(NCardLibrary.PropertyName._viewStats, Variant.From<NLibraryStatTickbox>(ref this._viewStats));
    info.AddProperty(NCardLibrary.PropertyName._viewUpgrades, Variant.From<NLibraryStatTickbox>(ref this._viewUpgrades));
    info.AddProperty(NCardLibrary.PropertyName._cardCountLabel, Variant.From<MegaRichTextLabel>(ref this._cardCountLabel));
    info.AddProperty(NCardLibrary.PropertyName._noResultsLabel, Variant.From<MegaRichTextLabel>(ref this._noResultsLabel));
    info.AddProperty(NCardLibrary.PropertyName._lastHoveredControl, Variant.From<Control>(ref this._lastHoveredControl));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    base.RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NCardLibrary.PropertyName._grid, ref variant1))
      this._grid = ((Variant) ref variant1).As<NCardLibraryGrid>();
    Variant variant2;
    if (info.TryGetProperty(NCardLibrary.PropertyName._searchBar, ref variant2))
      this._searchBar = ((Variant) ref variant2).As<NSearchBar>();
    Variant variant3;
    if (info.TryGetProperty(NCardLibrary.PropertyName._ironcladFilter, ref variant3))
      this._ironcladFilter = ((Variant) ref variant3).As<NCardPoolFilter>();
    Variant variant4;
    if (info.TryGetProperty(NCardLibrary.PropertyName._silentFilter, ref variant4))
      this._silentFilter = ((Variant) ref variant4).As<NCardPoolFilter>();
    Variant variant5;
    if (info.TryGetProperty(NCardLibrary.PropertyName._defectFilter, ref variant5))
      this._defectFilter = ((Variant) ref variant5).As<NCardPoolFilter>();
    Variant variant6;
    if (info.TryGetProperty(NCardLibrary.PropertyName._regentFilter, ref variant6))
      this._regentFilter = ((Variant) ref variant6).As<NCardPoolFilter>();
    Variant variant7;
    if (info.TryGetProperty(NCardLibrary.PropertyName._necrobinderFilter, ref variant7))
      this._necrobinderFilter = ((Variant) ref variant7).As<NCardPoolFilter>();
    Variant variant8;
    if (info.TryGetProperty(NCardLibrary.PropertyName._colorlessFilter, ref variant8))
      this._colorlessFilter = ((Variant) ref variant8).As<NCardPoolFilter>();
    Variant variant9;
    if (info.TryGetProperty(NCardLibrary.PropertyName._ancientsFilter, ref variant9))
      this._ancientsFilter = ((Variant) ref variant9).As<NCardPoolFilter>();
    Variant variant10;
    if (info.TryGetProperty(NCardLibrary.PropertyName._miscPoolFilter, ref variant10))
      this._miscPoolFilter = ((Variant) ref variant10).As<NCardPoolFilter>();
    Variant variant11;
    if (info.TryGetProperty(NCardLibrary.PropertyName._typeSorter, ref variant11))
      this._typeSorter = ((Variant) ref variant11).As<NCardViewSortButton>();
    Variant variant12;
    if (info.TryGetProperty(NCardLibrary.PropertyName._attackFilter, ref variant12))
      this._attackFilter = ((Variant) ref variant12).As<NCardTypeTickbox>();
    Variant variant13;
    if (info.TryGetProperty(NCardLibrary.PropertyName._skillFilter, ref variant13))
      this._skillFilter = ((Variant) ref variant13).As<NCardTypeTickbox>();
    Variant variant14;
    if (info.TryGetProperty(NCardLibrary.PropertyName._powerFilter, ref variant14))
      this._powerFilter = ((Variant) ref variant14).As<NCardTypeTickbox>();
    Variant variant15;
    if (info.TryGetProperty(NCardLibrary.PropertyName._otherTypeFilter, ref variant15))
      this._otherTypeFilter = ((Variant) ref variant15).As<NCardTypeTickbox>();
    Variant variant16;
    if (info.TryGetProperty(NCardLibrary.PropertyName._raritySorter, ref variant16))
      this._raritySorter = ((Variant) ref variant16).As<NCardViewSortButton>();
    Variant variant17;
    if (info.TryGetProperty(NCardLibrary.PropertyName._commonFilter, ref variant17))
      this._commonFilter = ((Variant) ref variant17).As<NCardRarityTickbox>();
    Variant variant18;
    if (info.TryGetProperty(NCardLibrary.PropertyName._uncommonFilter, ref variant18))
      this._uncommonFilter = ((Variant) ref variant18).As<NCardRarityTickbox>();
    Variant variant19;
    if (info.TryGetProperty(NCardLibrary.PropertyName._rareFilter, ref variant19))
      this._rareFilter = ((Variant) ref variant19).As<NCardRarityTickbox>();
    Variant variant20;
    if (info.TryGetProperty(NCardLibrary.PropertyName._otherFilter, ref variant20))
      this._otherFilter = ((Variant) ref variant20).As<NCardRarityTickbox>();
    Variant variant21;
    if (info.TryGetProperty(NCardLibrary.PropertyName._costSorter, ref variant21))
      this._costSorter = ((Variant) ref variant21).As<NCardViewSortButton>();
    Variant variant22;
    if (info.TryGetProperty(NCardLibrary.PropertyName._zeroFilter, ref variant22))
      this._zeroFilter = ((Variant) ref variant22).As<NCardCostTickbox>();
    Variant variant23;
    if (info.TryGetProperty(NCardLibrary.PropertyName._oneFilter, ref variant23))
      this._oneFilter = ((Variant) ref variant23).As<NCardCostTickbox>();
    Variant variant24;
    if (info.TryGetProperty(NCardLibrary.PropertyName._twoFilter, ref variant24))
      this._twoFilter = ((Variant) ref variant24).As<NCardCostTickbox>();
    Variant variant25;
    if (info.TryGetProperty(NCardLibrary.PropertyName._threePlusFilter, ref variant25))
      this._threePlusFilter = ((Variant) ref variant25).As<NCardCostTickbox>();
    Variant variant26;
    if (info.TryGetProperty(NCardLibrary.PropertyName._xFilter, ref variant26))
      this._xFilter = ((Variant) ref variant26).As<NCardCostTickbox>();
    Variant variant27;
    if (info.TryGetProperty(NCardLibrary.PropertyName._alphabetSorter, ref variant27))
      this._alphabetSorter = ((Variant) ref variant27).As<NCardViewSortButton>();
    Variant variant28;
    if (info.TryGetProperty(NCardLibrary.PropertyName._viewMultiplayerCards, ref variant28))
      this._viewMultiplayerCards = ((Variant) ref variant28).As<NLibraryStatTickbox>();
    Variant variant29;
    if (info.TryGetProperty(NCardLibrary.PropertyName._viewStats, ref variant29))
      this._viewStats = ((Variant) ref variant29).As<NLibraryStatTickbox>();
    Variant variant30;
    if (info.TryGetProperty(NCardLibrary.PropertyName._viewUpgrades, ref variant30))
      this._viewUpgrades = ((Variant) ref variant30).As<NLibraryStatTickbox>();
    Variant variant31;
    if (info.TryGetProperty(NCardLibrary.PropertyName._cardCountLabel, ref variant31))
      this._cardCountLabel = ((Variant) ref variant31).As<MegaRichTextLabel>();
    Variant variant32;
    if (info.TryGetProperty(NCardLibrary.PropertyName._noResultsLabel, ref variant32))
      this._noResultsLabel = ((Variant) ref variant32).As<MegaRichTextLabel>();
    Variant variant33;
    if (!info.TryGetProperty(NCardLibrary.PropertyName._lastHoveredControl, ref variant33))
      return;
    this._lastHoveredControl = ((Variant) ref variant33).As<Control>();
  }

  public NCardLibrary()
  {
    int capacity = 4;
    List<SortingOrders> sortingOrdersList = new List<SortingOrders>(capacity);
    CollectionsMarshal.SetCount<SortingOrders>(sortingOrdersList, capacity);
    Span<SortingOrders> span = CollectionsMarshal.AsSpan<SortingOrders>(sortingOrdersList);
    int num1 = 0;
    span[num1] = SortingOrders.RarityAscending;
    int num2 = num1 + 1;
    span[num2] = SortingOrders.TypeAscending;
    int num3 = num2 + 1;
    span[num3] = SortingOrders.CostAscending;
    int num4 = num3 + 1;
    span[num4] = SortingOrders.AlphabetAscending;
    this._sortingPriority = sortingOrdersList;
    this._filter = (Func<CardModel, bool>) (_ => true);
    // ISSUE: explicit constructor call
    base.\u002Ector();
  }

  public new class MethodName : NSubmenu.MethodName
  {
    public static readonly StringName Create = StringName.op_Implicit(nameof (Create));
    public new static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName OnCardTypeSort = StringName.op_Implicit(nameof (OnCardTypeSort));
    public static readonly StringName OnRaritySort = StringName.op_Implicit(nameof (OnRaritySort));
    public static readonly StringName OnCostSort = StringName.op_Implicit(nameof (OnCostSort));
    public static readonly StringName OnAlphabetSort = StringName.op_Implicit(nameof (OnAlphabetSort));
    public new static readonly StringName OnSubmenuOpened = StringName.op_Implicit(nameof (OnSubmenuOpened));
    public new static readonly StringName OnSubmenuClosed = StringName.op_Implicit(nameof (OnSubmenuClosed));
    public static readonly StringName ToggleShowStats = StringName.op_Implicit(nameof (ToggleShowStats));
    public static readonly StringName ToggleShowUpgrades = StringName.op_Implicit(nameof (ToggleShowUpgrades));
    public static readonly StringName ToggleFilterMultiplayerCards = StringName.op_Implicit(nameof (ToggleFilterMultiplayerCards));
    public static readonly StringName UpdateCardPoolFilter = StringName.op_Implicit(nameof (UpdateCardPoolFilter));
    public static readonly StringName UpdateTypeFilter = StringName.op_Implicit(nameof (UpdateTypeFilter));
    public static readonly StringName UpdateRarityFilter = StringName.op_Implicit(nameof (UpdateRarityFilter));
    public static readonly StringName UpdateCostFilter = StringName.op_Implicit(nameof (UpdateCostFilter));
    public static readonly StringName SearchBarQueryChanged = StringName.op_Implicit(nameof (SearchBarQueryChanged));
    public static readonly StringName SearchBarQuerySubmitted = StringName.op_Implicit(nameof (SearchBarQuerySubmitted));
    public static readonly StringName UpdateFilter = StringName.op_Implicit(nameof (UpdateFilter));
    public static readonly StringName ShowCardDetail = StringName.op_Implicit(nameof (ShowCardDetail));
  }

  public new class PropertyName : NSubmenu.PropertyName
  {
    public new static readonly StringName InitialFocusedControl = StringName.op_Implicit(nameof (InitialFocusedControl));
    public static readonly StringName _grid = StringName.op_Implicit(nameof (_grid));
    public static readonly StringName _searchBar = StringName.op_Implicit(nameof (_searchBar));
    public static readonly StringName _ironcladFilter = StringName.op_Implicit(nameof (_ironcladFilter));
    public static readonly StringName _silentFilter = StringName.op_Implicit(nameof (_silentFilter));
    public static readonly StringName _defectFilter = StringName.op_Implicit(nameof (_defectFilter));
    public static readonly StringName _regentFilter = StringName.op_Implicit(nameof (_regentFilter));
    public static readonly StringName _necrobinderFilter = StringName.op_Implicit(nameof (_necrobinderFilter));
    public static readonly StringName _colorlessFilter = StringName.op_Implicit(nameof (_colorlessFilter));
    public static readonly StringName _ancientsFilter = StringName.op_Implicit(nameof (_ancientsFilter));
    public static readonly StringName _miscPoolFilter = StringName.op_Implicit(nameof (_miscPoolFilter));
    public static readonly StringName _typeSorter = StringName.op_Implicit(nameof (_typeSorter));
    public static readonly StringName _attackFilter = StringName.op_Implicit(nameof (_attackFilter));
    public static readonly StringName _skillFilter = StringName.op_Implicit(nameof (_skillFilter));
    public static readonly StringName _powerFilter = StringName.op_Implicit(nameof (_powerFilter));
    public static readonly StringName _otherTypeFilter = StringName.op_Implicit(nameof (_otherTypeFilter));
    public static readonly StringName _raritySorter = StringName.op_Implicit(nameof (_raritySorter));
    public static readonly StringName _commonFilter = StringName.op_Implicit(nameof (_commonFilter));
    public static readonly StringName _uncommonFilter = StringName.op_Implicit(nameof (_uncommonFilter));
    public static readonly StringName _rareFilter = StringName.op_Implicit(nameof (_rareFilter));
    public static readonly StringName _otherFilter = StringName.op_Implicit(nameof (_otherFilter));
    public static readonly StringName _costSorter = StringName.op_Implicit(nameof (_costSorter));
    public static readonly StringName _zeroFilter = StringName.op_Implicit(nameof (_zeroFilter));
    public static readonly StringName _oneFilter = StringName.op_Implicit(nameof (_oneFilter));
    public static readonly StringName _twoFilter = StringName.op_Implicit(nameof (_twoFilter));
    public static readonly StringName _threePlusFilter = StringName.op_Implicit(nameof (_threePlusFilter));
    public static readonly StringName _xFilter = StringName.op_Implicit(nameof (_xFilter));
    public static readonly StringName _alphabetSorter = StringName.op_Implicit(nameof (_alphabetSorter));
    public static readonly StringName _viewMultiplayerCards = StringName.op_Implicit(nameof (_viewMultiplayerCards));
    public static readonly StringName _viewStats = StringName.op_Implicit(nameof (_viewStats));
    public static readonly StringName _viewUpgrades = StringName.op_Implicit(nameof (_viewUpgrades));
    public static readonly StringName _cardCountLabel = StringName.op_Implicit(nameof (_cardCountLabel));
    public static readonly StringName _noResultsLabel = StringName.op_Implicit(nameof (_noResultsLabel));
    public static readonly StringName _lastHoveredControl = StringName.op_Implicit(nameof (_lastHoveredControl));
  }

  public new class SignalName : NSubmenu.SignalName
  {
  }
}
