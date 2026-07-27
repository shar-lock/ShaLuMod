// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Cards.NCardGrid
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.UI;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Cards.Holders;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.Screens.CardLibrary;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Cards;

[ScriptPath("res://src/Core/Nodes/Cards/NCardGrid.cs")]
public class NCardGrid : Control
{
  private Dictionary<SortingOrders, Func<CardModel, CardModel, int>>? _sortingAlgorithms;
  private float _startDrag;
  private float _targetDrag;
  private bool _isDragging;
  private bool _scrollingEnabled = true;
  private const float _topMargin = 80f;
  private const float _bottomMargin = 320f;
  protected Control _scrollContainer;
  private bool _scrollbarPressed;
  private NScrollbar _scrollbar;
  private int _slidingWindowCardIndex;
  private PileType _pileType;
  protected Vector2 _cardSize;
  protected readonly List<CardModel> _cards = new List<CardModel>();
  protected readonly List<List<NGridCardHolder>> _cardRows = new List<List<NGridCardHolder>>();
  private readonly List<CardModel> _highlightedCards = new List<CardModel>();
  private Task? _animatingOutTask;
  private bool _cardsAnimatingOutForSetCards;
  private CancellationTokenSource _cts = new CancellationTokenSource();
  private CancellationTokenSource? _setCardsCancellation;
  private bool _isShowingUpgrades;
  private NCardHolder? _lastFocusedHolder;
  private readonly List<CardModel> _cardsCache = new List<CardModel>();
  private readonly List<CardModel> _sortedCardsCache = new List<CardModel>();
  private bool _needsReinit;
  private 
  #nullable disable
  NCardGrid.HolderPressedEventHandler backing_HolderPressed;
  private NCardGrid.HolderAltPressedEventHandler backing_HolderAltPressed;

  private 
  #nullable enable
  Dictionary<SortingOrders, Func<CardModel, CardModel, int>> SortingAlgorithms
  {
    get
    {
      Dictionary<SortingOrders, Func<CardModel, CardModel, int>> sortingAlgorithms1 = this._sortingAlgorithms;
      if (sortingAlgorithms1 != null)
        return sortingAlgorithms1;
      Dictionary<SortingOrders, Func<CardModel, CardModel, int>> dictionary = new Dictionary<SortingOrders, Func<CardModel, CardModel, int>>();
      dictionary.Add(SortingOrders.RarityAscending, (Func<CardModel, CardModel, int>) ((a, b) => this.GetCardRarityComparisonValue(a).CompareTo(this.GetCardRarityComparisonValue(b))));
      dictionary.Add(SortingOrders.CostAscending, (Func<CardModel, CardModel, int>) ((a, b) => a.EnergyCost.GetResolved().CompareTo(b.EnergyCost.GetResolved())));
      dictionary.Add(SortingOrders.TypeAscending, (Func<CardModel, CardModel, int>) ((a, b) => a.Type.CompareTo((object) b.Type)));
      dictionary.Add(SortingOrders.AlphabetAscending, (Func<CardModel, CardModel, int>) ((a, b) => string.Compare(a.Title, b.Title, LocManager.Instance.CultureInfo, CompareOptions.None)));
      dictionary.Add(SortingOrders.RarityDescending, (Func<CardModel, CardModel, int>) ((a, b) => -this.GetCardRarityComparisonValue(a).CompareTo(this.GetCardRarityComparisonValue(b))));
      dictionary.Add(SortingOrders.CostDescending, (Func<CardModel, CardModel, int>) ((a, b) => -a.EnergyCost.GetResolved().CompareTo(b.EnergyCost.GetResolved())));
      dictionary.Add(SortingOrders.TypeDescending, (Func<CardModel, CardModel, int>) ((a, b) => -a.Type.CompareTo((object) b.Type)));
      dictionary.Add(SortingOrders.AlphabetDescending, (Func<CardModel, CardModel, int>) ((a, b) => -string.Compare(a.Title, b.Title, LocManager.Instance.CultureInfo, CompareOptions.None)));
      dictionary.Add(SortingOrders.Ascending, (Func<CardModel, CardModel, int>) ((a, b) => this._cards.IndexOf(a).CompareTo(this._cards.IndexOf(b))));
      dictionary.Add(SortingOrders.Descending, (Func<CardModel, CardModel, int>) ((a, b) => -this._cards.IndexOf(a).CompareTo(this._cards.IndexOf(b))));
      Dictionary<SortingOrders, Func<CardModel, CardModel, int>> sortingAlgorithms2 = dictionary;
      this._sortingAlgorithms = dictionary;
      return sortingAlgorithms2;
    }
  }

  private int CompareCardVisibility(CardModel a, CardModel b)
  {
    return (this.GetCardVisibility(a) == ModelVisibility.Locked).CompareTo(this.GetCardVisibility(b) == ModelVisibility.Locked);
  }

  private int GetCardRarityComparisonValue(CardModel a)
  {
    if (a.Rarity <= CardRarity.Ancient)
      return (int) a.Rarity;
    switch (a.Rarity)
    {
      case CardRarity.Event:
        return 8;
      case CardRarity.Token:
        return 10;
      case CardRarity.Status:
        return 6;
      case CardRarity.Curse:
        return 7;
      case CardRarity.Quest:
        return 9;
      default:
        throw new ArgumentOutOfRangeException(nameof (a), (object) a, (string) null);
    }
  }

  private bool CanScroll => this._scrollingEnabled && ((CanvasItem) this).Visible;

  private int DisplayedRows { get; set; }

  protected int Columns
  {
    get
    {
      return (int) (((double) this._scrollContainer.Size.X + (double) this.CardPadding) / ((double) this._cardSize.X + (double) this.CardPadding));
    }
  }

  protected float CardPadding => 40f;

  protected virtual bool IsCardLibrary => false;

  private float ScrollLimitBottom
  {
    get
    {
      return (double) this.Size.Y <= (double) this._scrollContainer.Size.Y ? this.Size.Y - this._scrollContainer.Size.Y : (float) (((double) this.Size.Y - (double) this._scrollContainer.Size.Y) * 0.5);
    }
  }

  protected float ScrollLimitTop
  {
    get
    {
      return (double) this.Size.Y <= (double) this._scrollContainer.Size.Y || !this.CenterGrid ? 0.0f : (float) (((double) this.Size.Y - (double) this._scrollContainer.Size.Y) * 0.5);
    }
  }

  public IEnumerable<NGridCardHolder> CurrentlyDisplayedCardHolders
  {
    get
    {
      return this._cardRows.SelectMany<List<NGridCardHolder>, NGridCardHolder>((Func<List<NGridCardHolder>, IEnumerable<NGridCardHolder>>) (r => (IEnumerable<NGridCardHolder>) r));
    }
  }

  public IEnumerable<CardModel> CurrentlyDisplayedCards
  {
    get
    {
      return this.CurrentlyDisplayedCardHolders.Select<NGridCardHolder, CardModel>((Func<NGridCardHolder, CardModel>) (h => h.CardModel));
    }
  }

  public bool IsAnimatingOut
  {
    get
    {
      Task animatingOutTask = this._animatingOutTask;
      return animatingOutTask != null && !animatingOutTask.IsCompleted;
    }
  }

  public bool IsShowingUpgrades
  {
    get => this._isShowingUpgrades;
    set
    {
      this._isShowingUpgrades = value;
      foreach (List<NGridCardHolder> cardRow in this._cardRows)
      {
        foreach (NGridCardHolder ngridCardHolder in cardRow)
        {
          if ((this._isShowingUpgrades || ngridCardHolder.CardModel.CanonicalInstance.IsUpgradable) && (!this._isShowingUpgrades || ngridCardHolder.CardModel.IsUpgradable))
            ngridCardHolder.SetIsPreviewingUpgrade(this._isShowingUpgrades);
        }
      }
    }
  }

  public int YOffset { get; set; }

  protected virtual bool CenterGrid => true;

  public override void _Ready()
  {
    if (((object) this).GetType() != typeof (NCardGrid))
    {
      Log.Error($"{((object) this).GetType()}");
      throw new InvalidOperationException("Don't call base._Ready()! Call ConnectSignals() instead.");
    }
    this.ConnectSignals();
  }

  protected virtual void ConnectSignals()
  {
    this._scrollContainer = ((Node) this).GetNode<Control>(NodePath.op_Implicit("%ScrollContainer"));
    this._scrollbar = ((Node) this).GetNode<NScrollbar>(NodePath.op_Implicit("Scrollbar"));
    this._cardSize = Vector2.op_Multiply(NCard.defaultSize, NCardHolder.smallScale);
    ((GodotObject) this._scrollContainer).Connect(CanvasItem.SignalName.ItemRectChanged, Callable.From(new Action(this.UpdateScrollLimitBottom)), 0U);
    ((CanvasItem) this._scrollbar).Visible = false;
    ((GodotObject) this._scrollbar).Connect(NScrollbar.SignalName.MousePressed, Callable.From<InputEvent>((Action<InputEvent>) (_ => this._scrollbarPressed = true)), 0U);
    ((GodotObject) this._scrollbar).Connect(NScrollbar.SignalName.MouseReleased, Callable.From<InputEvent>((Action<InputEvent>) (_ => this._scrollbarPressed = false)), 0U);
  }

  public override void _EnterTree()
  {
    ((Node) this)._EnterTree();
    this._cts = new CancellationTokenSource();
    ((GodotObject) ((Node) this).GetViewport()).Connect(Viewport.SignalName.GuiFocusChanged, Callable.From<Control>(new Action<Control>(this.ProcessGuiFocus)), 0U);
  }

  public override void _ExitTree()
  {
    ((Node) this)._ExitTree();
    this._cts.Cancel();
    this._setCardsCancellation?.Cancel();
    ((GodotObject) ((Node) this).GetViewport()).Disconnect(Viewport.SignalName.GuiFocusChanged, Callable.From<Control>(new Action<Control>(this.ProcessGuiFocus)));
    foreach (List<NGridCardHolder> cardRow in this._cardRows)
    {
      foreach (Node node in cardRow)
        node.QueueFreeSafely();
    }
    this._cardRows.Clear();
  }

  private void UpdateScrollLimitBottom()
  {
    float num = this.Size.Y + 320f;
    ((CanvasItem) this._scrollbar).Visible = (double) this._scrollContainer.Size.Y > (double) num && this.CanScroll;
    ((Control) this._scrollbar).MouseFilter = (double) this._scrollContainer.Size.Y <= (double) num || !this.CanScroll ? (Control.MouseFilterEnum) 2L : (Control.MouseFilterEnum) 0L;
  }

  public override void _GuiInput(InputEvent inputEvent)
  {
    if (!((CanvasItem) this).IsVisibleInTree())
      return;
    this.ProcessMouseEvent(inputEvent);
    this.ProcessScrollEvent(inputEvent);
  }

  public override void _Process(double delta)
  {
    if (!((CanvasItem) this).IsVisibleInTree() || !this.CanScroll)
      return;
    this.UpdateScrollPosition(delta);
    if (!this._needsReinit)
      return;
    this.InitGrid();
  }

  public override void _Notification(int what)
  {
    ((GodotObject) this)._Notification(what);
    if (what != 40 || !((Node) this).IsNodeReady())
      return;
    this._needsReinit = true;
  }

  public void SetScrollPosition(float scrollY)
  {
    this._targetDrag = scrollY;
    this._scrollContainer.Position = new Vector2(this._scrollContainer.Position.X, scrollY);
  }

  public void SetCanScroll(bool canScroll)
  {
    this._scrollingEnabled = canScroll;
    if (this.CanScroll)
      return;
    this._isDragging = false;
  }

  public void InsetForTopBar() => this.SetAnchorAndOffset((Side) 1L, 0.0f, 80f, false);

  private void ProcessMouseEvent(InputEvent inputEvent)
  {
    if (this._isDragging && inputEvent is InputEventMouseMotion eventMouseMotion)
    {
      this._targetDrag += eventMouseMotion.Relative.Y;
    }
    else
    {
      if (!(inputEvent is InputEventMouseButton eventMouseButton))
        return;
      if (eventMouseButton.ButtonIndex == 1L)
      {
        if (eventMouseButton.Pressed)
        {
          this._isDragging = true;
          this._startDrag = this._scrollContainer.Position.Y;
          this._targetDrag = this._startDrag;
        }
        else
          this._isDragging = false;
      }
      else
      {
        if (eventMouseButton.Pressed)
          return;
        this._isDragging = false;
      }
    }
  }

  private void ProcessScrollEvent(InputEvent inputEvent)
  {
    this._targetDrag += ScrollHelper.GetDragForScrollEvent(inputEvent);
  }

  private void ProcessGuiFocus(Control focusedControl)
  {
    if (!((CanvasItem) this).IsVisibleInTree() || !this.CanScroll || !NControllerManager.Instance.IsUsingController || ((Node) focusedControl).GetParent() != this._scrollContainer)
      return;
    this._targetDrag = Math.Clamp((float) (-(double) focusedControl.Position.Y + (double) this.Size.Y * 0.5), Math.Min(Math.Min(this.ScrollLimitTop, this.ScrollLimitBottom), 0.0f), Math.Max(Math.Min(this.ScrollLimitTop, this.ScrollLimitBottom), 0.0f));
  }

  private void UpdateScrollPosition(double delta)
  {
    float num1 = this._scrollContainer.Position.Y;
    if ((double) Math.Abs(num1 - this._targetDrag) > 0.10000000149011612)
    {
      float num2 = (float) Mathf.Sign(num1 - this._targetDrag);
      num1 = Mathf.Lerp(num1, this._targetDrag, Mathf.Clamp((float) delta * 15f, 0.0f, 1f));
      float num3 = (float) Mathf.Sign(num1 - this._targetDrag);
      if ((double) Math.Abs(num1 - this._targetDrag) < 0.5 || !Mathf.IsEqualApprox(num2, num3))
        num1 = this._targetDrag;
      this.AllocateCardHolders();
      if (!this._scrollbarPressed && this.CanScroll)
        this._scrollbar.SetValueWithoutAnimation((double) Mathf.Clamp(this._scrollContainer.Position.Y / this.ScrollLimitBottom, 0.0f, 1f) * 100.0);
    }
    if (this._scrollbarPressed)
    {
      this._targetDrag = Mathf.Lerp(0.0f, this.ScrollLimitBottom, (float) this._scrollbar.Value / 100f);
      this.AllocateCardHolders();
    }
    if (!this._isDragging)
    {
      if ((double) this._targetDrag < (double) Mathf.Min(this.ScrollLimitBottom, this.ScrollLimitTop))
        this._targetDrag = Mathf.Lerp(this._targetDrag, Mathf.Min(this.ScrollLimitBottom, this.ScrollLimitTop), (float) delta * 12f);
      else if ((double) this._targetDrag > (double) Mathf.Max(this.ScrollLimitTop, this.ScrollLimitBottom))
        this._targetDrag = Mathf.Lerp(this._targetDrag, Mathf.Max(this.ScrollLimitTop, this.ScrollLimitBottom), (float) delta * 12f);
    }
    this._scrollContainer.Position = new Vector2(this._scrollContainer.Position.X, num1);
  }

  public void ClearGrid()
  {
    this._cardsCache.Clear();
    this._cards.Clear();
    TaskHelper.RunSafely(this.InitGrid((Task) null));
  }

  public void SetCards(
    IReadOnlyList<CardModel> cardsToDisplay,
    PileType pileType,
    List<SortingOrders> sortingPriority,
    Task? taskToWaitOn = null)
  {
    this._cardsCache.Clear();
    this._cardsCache.AddRange((IEnumerable<CardModel>) cardsToDisplay);
    if (sortingPriority[0] == SortingOrders.Descending)
      this._cardsCache.Reverse();
    else if (sortingPriority[0] != SortingOrders.Ascending)
      this._cardsCache.Sort((Comparison<CardModel>) ((x, y) =>
      {
        foreach (SortingOrders key in sortingPriority)
        {
          int num = this.SortingAlgorithms[key](x, y);
          if (num != 0)
            return num;
        }
        return x.Id.CompareTo(y.Id);
      }));
    if (this.IsCardLibrary)
    {
      this._sortedCardsCache.Clear();
      this._sortedCardsCache.AddRange((IEnumerable<CardModel>) this._cardsCache.OrderBy<CardModel, CardModel>((Func<CardModel, CardModel>) (c => c), (IComparer<CardModel>) Comparer<CardModel>.Create(new Comparison<CardModel>(this.CompareCardVisibility))));
      this._cardsCache.Clear();
      this._cardsCache.AddRange((IEnumerable<CardModel>) this._sortedCardsCache);
    }
    this._cards.Clear();
    this._cards.AddRange((IEnumerable<CardModel>) this._cardsCache);
    this._pileType = pileType;
    if (this._cardsAnimatingOutForSetCards)
      return;
    TaskHelper.RunSafely(this.InitGrid(taskToWaitOn));
  }

  public Task AnimateOut()
  {
    this._animatingOutTask = this.AnimateOutInternal();
    return this._animatingOutTask;
  }

  private async Task AnimateOutInternal()
  {
    if (!this.IsCardLibrary)
      return;
    List<NGridCardHolder> list = this._cardRows.SelectMany<List<NGridCardHolder>, NGridCardHolder>((Func<List<NGridCardHolder>, IEnumerable<NGridCardHolder>>) (c => (IEnumerable<NGridCardHolder>) c)).ToList<NGridCardHolder>();
    if (list.Count <= 0)
      return;
    Tween tween = ((Node) this).CreateTween().SetParallel(true);
    foreach (NGridCardHolder ngridCardHolder in list)
    {
      tween.TweenProperty((GodotObject) ngridCardHolder, NodePath.op_Implicit("position:y"), Variant.op_Implicit(ngridCardHolder.Position.Y + 40f), 0.2).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 5L);
      tween.TweenProperty((GodotObject) ngridCardHolder, NodePath.op_Implicit("modulate:a"), Variant.op_Implicit(0.0f), 0.2);
    }
    bool flag = await tween.AwaitFinished((Node) this);
  }

  private async Task AnimateIn()
  {
    if (!this.IsCardLibrary)
      return;
    List<NGridCardHolder> list = this._cardRows.SelectMany<List<NGridCardHolder>, NGridCardHolder>((Func<List<NGridCardHolder>, IEnumerable<NGridCardHolder>>) (c => (IEnumerable<NGridCardHolder>) c)).ToList<NGridCardHolder>();
    if (list.Count <= 0)
      return;
    Tween tween = ((Node) this).CreateTween().SetParallel(true);
    for (int index = 0; index < list.Count; ++index)
    {
      NGridCardHolder ngridCardHolder1 = list[index];
      float num = (float) ((double) index / (double) list.Count * 0.20000000298023224);
      float y = ngridCardHolder1.Position.Y;
      NGridCardHolder ngridCardHolder2 = ngridCardHolder1;
      Vector2 position = ngridCardHolder1.Position;
      position.Y = ngridCardHolder1.Position.Y + 40f;
      Vector2 vector2 = position;
      ngridCardHolder2.Position = vector2;
      NGridCardHolder ngridCardHolder3 = ngridCardHolder1;
      Color modulate = ((CanvasItem) ngridCardHolder1).Modulate;
      modulate.A = 0.0f;
      Color color = modulate;
      ((CanvasItem) ngridCardHolder3).Modulate = color;
      tween.TweenProperty((GodotObject) ngridCardHolder1, NodePath.op_Implicit("position:y"), Variant.op_Implicit(y), 0.4).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 10L).SetDelay((double) num);
      tween.TweenProperty((GodotObject) ngridCardHolder1, NodePath.op_Implicit("modulate:a"), Variant.op_Implicit(1f), 0.4).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 5L).SetDelay((double) num);
    }
    this._setCardsCancellation = new CancellationTokenSource();
    while (tween.IsRunning())
    {
      if (this._setCardsCancellation.IsCancellationRequested)
        tween.Kill();
      double num = (double) await ((Node) this).AwaitProcessFrame(this._cts.Token);
    }
    tween = (Tween) null;
  }

  private async Task InitGrid(Task? taskToWaitOn)
  {
    if (this._setCardsCancellation != null)
      await this._setCardsCancellation.CancelAsync();
    this._cardsAnimatingOutForSetCards = true;
    Task animatingOutTask = this._animatingOutTask;
    if (animatingOutTask != null && !animatingOutTask.IsCompleted)
      await this._animatingOutTask;
    else
      await this.AnimateOut();
    if (taskToWaitOn != null)
      await taskToWaitOn;
    this._cardsAnimatingOutForSetCards = false;
    this.InitGrid();
    this.SetScrollPosition(this.ScrollLimitTop);
    await this.AnimateIn();
  }

  private int CalculateRowsNeeded()
  {
    return Mathf.Min(Mathf.CeilToInt((float) (((double) this.Size.Y + (double) this.CardPadding) / ((double) this._cardSize.Y + (double) this.CardPadding))) + 2, this.GetTotalRowCount());
  }

  protected virtual void InitGrid()
  {
    this._scrollContainer.Position = new Vector2(this._scrollContainer.Position.X, this.ScrollLimitTop);
    this._slidingWindowCardIndex = 0;
    this._scrollbar.Value = 0.0;
    this.DisplayedRows = this.CalculateRowsNeeded();
    foreach (List<NGridCardHolder> cardRow in this._cardRows)
    {
      foreach (NGridCardHolder ngridCardHolder1 in cardRow)
      {
        NGridCardHolder ngridCardHolder2 = ngridCardHolder1;
        ((Node) ngridCardHolder2).Name = StringName.op_Implicit(StringName.op_Implicit(((Node) ngridCardHolder2).Name) + "-OLD");
        ((Node) ngridCardHolder1).QueueFreeSafely();
      }
    }
    this._cardRows.Clear();
    if (this._cards.Count != 0)
    {
      int index1 = 0;
      for (int index2 = 0; index2 < this.DisplayedRows; ++index2)
      {
        List<NGridCardHolder> ngridCardHolderList = new List<NGridCardHolder>();
        for (int index3 = 0; index3 < this.Columns && index1 < this._cards.Count; ++index3)
        {
          CardModel card = this._cards[index1];
          NCard cardNode = NCard.Create(card, this.GetCardVisibility(card));
          NGridCardHolder child = NGridCardHolder.Create(cardNode);
          ngridCardHolderList.Add(child);
          ((GodotObject) child).Connect(NCardHolder.SignalName.Pressed, Callable.From<NCardHolder>(new Action<NCardHolder>(this.OnHolderPressed)), 0U);
          ((GodotObject) child).Connect(NCardHolder.SignalName.AltPressed, Callable.From<NCardHolder>(new Action<NCardHolder>(this.OnHolderAltPressed)), 0U);
          ((CanvasItem) child).Visible = true;
          child.MouseFilter = (Control.MouseFilterEnum) 1L;
          child.Scale = child.SmallScale;
          ((Node) this._scrollContainer).AddChildSafely((Node) child);
          cardNode.UpdateVisuals(this._pileType, CardPreviewMode.Normal);
          if (child.CardModel.IsUpgradable)
            child.SetIsPreviewingUpgrade(this.IsShowingUpgrades);
          ++index1;
        }
        this._cardRows.Add(ngridCardHolderList);
      }
    }
    ((GodotObject) this._scrollContainer).SetDeferred(Control.PropertyName.Size, Variant.op_Implicit(new Vector2(this._scrollContainer.Size.X, (float) ((double) this.GetContainedCardsSize().Y + 80.0 + 320.0) + (float) this.YOffset)));
    this.UpdateGridPositions(0);
    this.UpdateGridNavigation();
    this._needsReinit = false;
  }

  private Vector2 GetContainedCardsSize()
  {
    int totalRowCount = this.GetTotalRowCount();
    return Vector2.op_Addition(Vector2.op_Multiply(new Vector2((float) this.Columns, (float) totalRowCount), this._cardSize), Vector2.op_Multiply(new Vector2((float) (this.Columns - 1), (float) (totalRowCount - 1)), this.CardPadding));
  }

  private void ReflowColumns()
  {
    if (this._cards.Count == 0)
      return;
    this.InitGrid();
  }

  private void UpdateGridPositions(int index)
  {
    Vector2 vector2 = Vector2.op_Addition(new Vector2((float) (((double) this._scrollContainer.Size.X - (double) this.GetContainedCardsSize().X) * 0.5), (float) this.YOffset + 80f), Vector2.op_Multiply(this._cardSize, 0.5f));
    foreach (List<NGridCardHolder> cardRow in this._cardRows)
    {
      foreach (NGridCardHolder ngridCardHolder in cardRow)
      {
        int num1 = index / this.Columns;
        int num2 = index % this.Columns;
        ngridCardHolder.Position = Vector2.op_Addition(vector2, new Vector2((float) num2 * (this._cardSize.X + this.CardPadding), (float) num1 * (this._cardSize.Y + this.CardPadding)));
        ++index;
      }
    }
  }

  public NGridCardHolder? GetCardHolder(CardModel model)
  {
    return this._cardRows.SelectMany<List<NGridCardHolder>, NGridCardHolder>((Func<List<NGridCardHolder>, IEnumerable<NGridCardHolder>>) (row => (IEnumerable<NGridCardHolder>) row)).FirstOrDefault<NGridCardHolder>((Func<NGridCardHolder, bool>) (h => h.CardModel == model));
  }

  public NCard? GetCardNode(CardModel model) => this.GetCardHolder(model)?.CardNode;

  public IEnumerable<NGridCardHolder>? GetTopRowOfCardNodes()
  {
    return (IEnumerable<NGridCardHolder>) this._cardRows.FirstOrDefault<List<NGridCardHolder>>();
  }

  private void OnHolderPressed(NCardHolder holder)
  {
    this._lastFocusedHolder = holder;
    ((GodotObject) this).EmitSignal(NCardGrid.SignalName.HolderPressed, new Variant[1]
    {
      Variant.op_Implicit((GodotObject) holder)
    });
  }

  private void OnHolderAltPressed(NCardHolder holder)
  {
    this._lastFocusedHolder = holder;
    ((GodotObject) this).EmitSignal(NCardGrid.SignalName.HolderAltPressed, new Variant[1]
    {
      Variant.op_Implicit((GodotObject) holder)
    });
  }

  private int GetTotalRowCount()
  {
    return Mathf.CeilToInt((float) this._cards.Count / (float) this.Columns);
  }

  private void AllocateCardHolders()
  {
    if (this._cardRows.Count == 0)
      return;
    Rect2 viewportRect = ((CanvasItem) this).GetViewportRect();
    float y1 = ((Rect2) ref viewportRect).Size.Y;
    float y2 = this._cardRows[0][0].GlobalPosition.Y;
    List<List<NGridCardHolder>> cardRows1 = this._cardRows;
    float y3 = cardRows1[cardRows1.Count - 1][0].GlobalPosition.Y;
    if ((double) Mathf.Abs(y2 - 0.0f) > (double) this.Size.Y * 2.0)
      this.ReallocateAll();
    else if ((double) y2 > 0.0)
    {
      List<List<NGridCardHolder>> cardRows2 = this._cardRows;
      this.ReallocateAbove(cardRows2[cardRows2.Count - 1]);
    }
    else
    {
      if ((double) y3 >= (double) y1)
        return;
      this.ReallocateBelow(this._cardRows[0]);
    }
  }

  private void ReallocateAll()
  {
    this._slidingWindowCardIndex = Mathf.Max(0, this._slidingWindowCardIndex - this.Columns * Mathf.RoundToInt((this._cardRows[0][0].GlobalPosition.Y - 0.0f) / (this._cardSize.Y + this.CardPadding)));
    int count = this._cardRows.Count;
    for (int index = 0; index < count; ++index)
      this.AssignCardsToRow(this._cardRows[index], this._slidingWindowCardIndex + index * this.Columns);
    this.UpdateGridPositions(this._slidingWindowCardIndex);
    this.UpdateGridNavigation();
  }

  private void ReallocateAbove(List<NGridCardHolder> row)
  {
    int num = this._slidingWindowCardIndex - this.Columns;
    if (num < 0)
      return;
    this._slidingWindowCardIndex = num;
    this._cardRows.RemoveAt(this._cardRows.Count - 1);
    this.AssignCardsToRow(row, this._slidingWindowCardIndex);
    this._cardRows.Insert(0, row);
    float y = this._cardRows[1][0].Position.Y;
    foreach (NGridCardHolder ngridCardHolder1 in row)
    {
      NGridCardHolder ngridCardHolder2 = ngridCardHolder1;
      Vector2 position = ngridCardHolder1.Position;
      position.Y = y - this._cardSize.Y - this.CardPadding;
      Vector2 vector2 = position;
      ngridCardHolder2.Position = vector2;
    }
    this.UpdateGridNavigation();
  }

  private void ReallocateBelow(List<NGridCardHolder> row)
  {
    int startIndex = this._slidingWindowCardIndex + this.Columns * this.DisplayedRows;
    if (startIndex >= this._cards.Count)
      return;
    this._slidingWindowCardIndex += this.Columns;
    this._cardRows.RemoveAt(0);
    this.AssignCardsToRow(row, startIndex);
    this._cardRows.Add(row);
    List<List<NGridCardHolder>> cardRows = this._cardRows;
    float y = cardRows[cardRows.Count - 2][0].Position.Y;
    foreach (NGridCardHolder ngridCardHolder1 in row)
    {
      NGridCardHolder ngridCardHolder2 = ngridCardHolder1;
      Vector2 position = ngridCardHolder1.Position;
      position.Y = y + this._cardSize.Y + this.CardPadding;
      Vector2 vector2 = position;
      ngridCardHolder2.Position = vector2;
    }
    this.UpdateGridNavigation();
  }

  public void HighlightCard(CardModel card)
  {
    this._highlightedCards.Add(card);
    this.GetCardNode(card)?.CardHighlight.AnimShow();
  }

  public void UnhighlightCard(CardModel card)
  {
    this._highlightedCards.Remove(card);
    this.GetCardNode(card)?.CardHighlight.AnimHide();
  }

  protected virtual void AssignCardsToRow(List<NGridCardHolder> row, int startIndex)
  {
    for (int index = 0; index < row.Count; ++index)
    {
      NGridCardHolder ngridCardHolder = row[index];
      if (startIndex + index >= this._cards.Count)
      {
        ((CanvasItem) ngridCardHolder).Visible = false;
      }
      else
      {
        CardModel card = this._cards[startIndex + index];
        ngridCardHolder.ReassignToCard(card, PileType.None, (Creature) null, this.GetCardVisibility(card));
        ((CanvasItem) ngridCardHolder).Visible = true;
        if (this._highlightedCards.Contains(card))
          ngridCardHolder.CardNode.CardHighlight.AnimShow();
        else
          ngridCardHolder.CardNode.CardHighlight.AnimHide();
        if (this._isShowingUpgrades && card.IsUpgradable)
          ngridCardHolder.SetIsPreviewingUpgrade(true);
      }
    }
  }

  protected virtual ModelVisibility GetCardVisibility(CardModel card) => ModelVisibility.Visible;

  public Control? DefaultFocusedControl
  {
    get
    {
      if (this._lastFocusedHolder != null)
        return (Control) this._lastFocusedHolder;
      return this._cards.Count == 0 ? (Control) null : (Control) this._cardRows[0][0];
    }
  }

  public Control? FocusedControlFromTopBar
  {
    get => this._cards.Count != 0 ? (Control) this._cardRows[0][0] : (Control) null;
  }

  protected virtual void UpdateGridNavigation()
  {
    for (int index1 = 0; index1 < this._cardRows.Count; ++index1)
    {
      for (int index2 = 0; index2 < this._cardRows[index1].Count; ++index2)
      {
        NCardHolder ncardHolder = (NCardHolder) this._cardRows[index1][index2];
        ncardHolder.FocusNeighborLeft = index2 > 0 ? ((Node) this._cardRows[index1][index2 - 1]).GetPath() : ((Node) this._cardRows[index1][this._cardRows[index1].Count - 1]).GetPath();
        ncardHolder.FocusNeighborRight = index2 < this._cardRows[index1].Count - 1 ? ((Node) this._cardRows[index1][index2 + 1]).GetPath() : ((Node) this._cardRows[index1][0]).GetPath();
        ncardHolder.FocusNeighborTop = index1 > 0 ? ((Node) this._cardRows[index1 - 1][index2]).GetPath() : ((Node) this._cardRows[index1][index2]).GetPath();
        ncardHolder.FocusNeighborBottom = index1 >= this._cardRows.Count - 1 || index2 >= this._cardRows[index1 + 1].Count ? ((Node) this._cardRows[index1][index2]).GetPath() : ((Node) this._cardRows[index1 + 1][index2]).GetPath();
      }
    }
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(27)
    {
      new MethodInfo(NCardGrid.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCardGrid.MethodName.ConnectSignals, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCardGrid.MethodName._EnterTree, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCardGrid.MethodName._ExitTree, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCardGrid.MethodName.UpdateScrollLimitBottom, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCardGrid.MethodName._GuiInput, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("inputEvent"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("InputEvent"), false)
      }, (List<Variant>) null),
      new MethodInfo(NCardGrid.MethodName._Process, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 3L, StringName.op_Implicit("delta"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NCardGrid.MethodName._Notification, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 2L, StringName.op_Implicit("what"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NCardGrid.MethodName.SetScrollPosition, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 3L, StringName.op_Implicit("scrollY"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NCardGrid.MethodName.SetCanScroll, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 1L, StringName.op_Implicit("canScroll"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NCardGrid.MethodName.InsetForTopBar, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCardGrid.MethodName.ProcessMouseEvent, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("inputEvent"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("InputEvent"), false)
      }, (List<Variant>) null),
      new MethodInfo(NCardGrid.MethodName.ProcessScrollEvent, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("inputEvent"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("InputEvent"), false)
      }, (List<Variant>) null),
      new MethodInfo(NCardGrid.MethodName.ProcessGuiFocus, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("focusedControl"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false)
      }, (List<Variant>) null),
      new MethodInfo(NCardGrid.MethodName.UpdateScrollPosition, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 3L, StringName.op_Implicit("delta"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NCardGrid.MethodName.ClearGrid, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCardGrid.MethodName.CalculateRowsNeeded, new PropertyInfo((Variant.Type) 2L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCardGrid.MethodName.InitGrid, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCardGrid.MethodName.GetContainedCardsSize, new PropertyInfo((Variant.Type) 5L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCardGrid.MethodName.ReflowColumns, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCardGrid.MethodName.UpdateGridPositions, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 2L, StringName.op_Implicit("index"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NCardGrid.MethodName.OnHolderPressed, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("holder"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false)
      }, (List<Variant>) null),
      new MethodInfo(NCardGrid.MethodName.OnHolderAltPressed, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("holder"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false)
      }, (List<Variant>) null),
      new MethodInfo(NCardGrid.MethodName.GetTotalRowCount, new PropertyInfo((Variant.Type) 2L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCardGrid.MethodName.AllocateCardHolders, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCardGrid.MethodName.ReallocateAll, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCardGrid.MethodName.UpdateGridNavigation, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NCardGrid.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCardGrid.MethodName.ConnectSignals) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.ConnectSignals();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCardGrid.MethodName._EnterTree) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._EnterTree();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCardGrid.MethodName._ExitTree) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._ExitTree();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCardGrid.MethodName.UpdateScrollLimitBottom) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.UpdateScrollLimitBottom();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCardGrid.MethodName._GuiInput) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      base._GuiInput(VariantUtils.ConvertTo<InputEvent>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCardGrid.MethodName._Process) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      ((Node) this)._Process(VariantUtils.ConvertTo<double>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCardGrid.MethodName._Notification) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      ((GodotObject) this)._Notification(VariantUtils.ConvertTo<int>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCardGrid.MethodName.SetScrollPosition) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.SetScrollPosition(VariantUtils.ConvertTo<float>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCardGrid.MethodName.SetCanScroll) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.SetCanScroll(VariantUtils.ConvertTo<bool>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCardGrid.MethodName.InsetForTopBar) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.InsetForTopBar();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCardGrid.MethodName.ProcessMouseEvent) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.ProcessMouseEvent(VariantUtils.ConvertTo<InputEvent>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCardGrid.MethodName.ProcessScrollEvent) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.ProcessScrollEvent(VariantUtils.ConvertTo<InputEvent>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCardGrid.MethodName.ProcessGuiFocus) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.ProcessGuiFocus(VariantUtils.ConvertTo<Control>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCardGrid.MethodName.UpdateScrollPosition) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.UpdateScrollPosition(VariantUtils.ConvertTo<double>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCardGrid.MethodName.ClearGrid) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.ClearGrid();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCardGrid.MethodName.CalculateRowsNeeded) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      int rowsNeeded = this.CalculateRowsNeeded();
      ret = VariantUtils.CreateFrom<int>(ref rowsNeeded);
      return true;
    }
    if (StringName.op_Equality(ref method, NCardGrid.MethodName.InitGrid) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.InitGrid();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCardGrid.MethodName.GetContainedCardsSize) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      Vector2 containedCardsSize = this.GetContainedCardsSize();
      ret = VariantUtils.CreateFrom<Vector2>(ref containedCardsSize);
      return true;
    }
    if (StringName.op_Equality(ref method, NCardGrid.MethodName.ReflowColumns) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.ReflowColumns();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCardGrid.MethodName.UpdateGridPositions) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.UpdateGridPositions(VariantUtils.ConvertTo<int>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCardGrid.MethodName.OnHolderPressed) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.OnHolderPressed(VariantUtils.ConvertTo<NCardHolder>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCardGrid.MethodName.OnHolderAltPressed) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.OnHolderAltPressed(VariantUtils.ConvertTo<NCardHolder>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCardGrid.MethodName.GetTotalRowCount) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      int totalRowCount = this.GetTotalRowCount();
      ret = VariantUtils.CreateFrom<int>(ref totalRowCount);
      return true;
    }
    if (StringName.op_Equality(ref method, NCardGrid.MethodName.AllocateCardHolders) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.AllocateCardHolders();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCardGrid.MethodName.ReallocateAll) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.ReallocateAll();
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NCardGrid.MethodName.UpdateGridNavigation) || ((NativeVariantPtrArgs) ref args).Count != 0)
      return base.InvokeGodotClassMethod(ref method, args, ref ret);
    this.UpdateGridNavigation();
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NCardGrid.MethodName._Ready) || StringName.op_Equality(ref method, NCardGrid.MethodName.ConnectSignals) || StringName.op_Equality(ref method, NCardGrid.MethodName._EnterTree) || StringName.op_Equality(ref method, NCardGrid.MethodName._ExitTree) || StringName.op_Equality(ref method, NCardGrid.MethodName.UpdateScrollLimitBottom) || StringName.op_Equality(ref method, NCardGrid.MethodName._GuiInput) || StringName.op_Equality(ref method, NCardGrid.MethodName._Process) || StringName.op_Equality(ref method, NCardGrid.MethodName._Notification) || StringName.op_Equality(ref method, NCardGrid.MethodName.SetScrollPosition) || StringName.op_Equality(ref method, NCardGrid.MethodName.SetCanScroll) || StringName.op_Equality(ref method, NCardGrid.MethodName.InsetForTopBar) || StringName.op_Equality(ref method, NCardGrid.MethodName.ProcessMouseEvent) || StringName.op_Equality(ref method, NCardGrid.MethodName.ProcessScrollEvent) || StringName.op_Equality(ref method, NCardGrid.MethodName.ProcessGuiFocus) || StringName.op_Equality(ref method, NCardGrid.MethodName.UpdateScrollPosition) || StringName.op_Equality(ref method, NCardGrid.MethodName.ClearGrid) || StringName.op_Equality(ref method, NCardGrid.MethodName.CalculateRowsNeeded) || StringName.op_Equality(ref method, NCardGrid.MethodName.InitGrid) || StringName.op_Equality(ref method, NCardGrid.MethodName.GetContainedCardsSize) || StringName.op_Equality(ref method, NCardGrid.MethodName.ReflowColumns) || StringName.op_Equality(ref method, NCardGrid.MethodName.UpdateGridPositions) || StringName.op_Equality(ref method, NCardGrid.MethodName.OnHolderPressed) || StringName.op_Equality(ref method, NCardGrid.MethodName.OnHolderAltPressed) || StringName.op_Equality(ref method, NCardGrid.MethodName.GetTotalRowCount) || StringName.op_Equality(ref method, NCardGrid.MethodName.AllocateCardHolders) || StringName.op_Equality(ref method, NCardGrid.MethodName.ReallocateAll) || StringName.op_Equality(ref method, NCardGrid.MethodName.UpdateGridNavigation) || base.HasGodotClassMethod(ref method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NCardGrid.PropertyName.DisplayedRows))
    {
      this.DisplayedRows = VariantUtils.ConvertTo<int>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCardGrid.PropertyName.IsShowingUpgrades))
    {
      this.IsShowingUpgrades = VariantUtils.ConvertTo<bool>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCardGrid.PropertyName.YOffset))
    {
      this.YOffset = VariantUtils.ConvertTo<int>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCardGrid.PropertyName._startDrag))
    {
      this._startDrag = VariantUtils.ConvertTo<float>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCardGrid.PropertyName._targetDrag))
    {
      this._targetDrag = VariantUtils.ConvertTo<float>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCardGrid.PropertyName._isDragging))
    {
      this._isDragging = VariantUtils.ConvertTo<bool>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCardGrid.PropertyName._scrollingEnabled))
    {
      this._scrollingEnabled = VariantUtils.ConvertTo<bool>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCardGrid.PropertyName._scrollContainer))
    {
      this._scrollContainer = VariantUtils.ConvertTo<Control>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCardGrid.PropertyName._scrollbarPressed))
    {
      this._scrollbarPressed = VariantUtils.ConvertTo<bool>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCardGrid.PropertyName._scrollbar))
    {
      this._scrollbar = VariantUtils.ConvertTo<NScrollbar>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCardGrid.PropertyName._slidingWindowCardIndex))
    {
      this._slidingWindowCardIndex = VariantUtils.ConvertTo<int>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCardGrid.PropertyName._pileType))
    {
      this._pileType = VariantUtils.ConvertTo<PileType>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCardGrid.PropertyName._cardSize))
    {
      this._cardSize = VariantUtils.ConvertTo<Vector2>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCardGrid.PropertyName._cardsAnimatingOutForSetCards))
    {
      this._cardsAnimatingOutForSetCards = VariantUtils.ConvertTo<bool>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCardGrid.PropertyName._isShowingUpgrades))
    {
      this._isShowingUpgrades = VariantUtils.ConvertTo<bool>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCardGrid.PropertyName._lastFocusedHolder))
    {
      this._lastFocusedHolder = VariantUtils.ConvertTo<NCardHolder>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NCardGrid.PropertyName._needsReinit))
      return ((GodotObject) this).SetGodotClassPropertyValue(ref name, ref value);
    this._needsReinit = VariantUtils.ConvertTo<bool>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NCardGrid.PropertyName.CanScroll))
    {
      ref godot_variant local = ref value;
      bool canScroll = this.CanScroll;
      godot_variant from = VariantUtils.CreateFrom<bool>(ref canScroll);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NCardGrid.PropertyName.DisplayedRows))
    {
      ref godot_variant local = ref value;
      int displayedRows = this.DisplayedRows;
      godot_variant from = VariantUtils.CreateFrom<int>(ref displayedRows);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NCardGrid.PropertyName.Columns))
    {
      ref godot_variant local = ref value;
      int columns = this.Columns;
      godot_variant from = VariantUtils.CreateFrom<int>(ref columns);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NCardGrid.PropertyName.CardPadding))
    {
      ref godot_variant local = ref value;
      float cardPadding = this.CardPadding;
      godot_variant from = VariantUtils.CreateFrom<float>(ref cardPadding);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NCardGrid.PropertyName.IsCardLibrary))
    {
      ref godot_variant local = ref value;
      bool isCardLibrary = this.IsCardLibrary;
      godot_variant from = VariantUtils.CreateFrom<bool>(ref isCardLibrary);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NCardGrid.PropertyName.ScrollLimitBottom))
    {
      ref godot_variant local = ref value;
      float scrollLimitBottom = this.ScrollLimitBottom;
      godot_variant from = VariantUtils.CreateFrom<float>(ref scrollLimitBottom);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NCardGrid.PropertyName.ScrollLimitTop))
    {
      ref godot_variant local = ref value;
      float scrollLimitTop = this.ScrollLimitTop;
      godot_variant from = VariantUtils.CreateFrom<float>(ref scrollLimitTop);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NCardGrid.PropertyName.IsAnimatingOut))
    {
      ref godot_variant local = ref value;
      bool isAnimatingOut = this.IsAnimatingOut;
      godot_variant from = VariantUtils.CreateFrom<bool>(ref isAnimatingOut);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NCardGrid.PropertyName.IsShowingUpgrades))
    {
      ref godot_variant local = ref value;
      bool isShowingUpgrades = this.IsShowingUpgrades;
      godot_variant from = VariantUtils.CreateFrom<bool>(ref isShowingUpgrades);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NCardGrid.PropertyName.YOffset))
    {
      ref godot_variant local = ref value;
      int yoffset = this.YOffset;
      godot_variant from = VariantUtils.CreateFrom<int>(ref yoffset);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NCardGrid.PropertyName.CenterGrid))
    {
      ref godot_variant local = ref value;
      bool centerGrid = this.CenterGrid;
      godot_variant from = VariantUtils.CreateFrom<bool>(ref centerGrid);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NCardGrid.PropertyName.DefaultFocusedControl))
    {
      ref godot_variant local = ref value;
      Control defaultFocusedControl = this.DefaultFocusedControl;
      godot_variant from = VariantUtils.CreateFrom<Control>(ref defaultFocusedControl);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NCardGrid.PropertyName.FocusedControlFromTopBar))
    {
      ref godot_variant local = ref value;
      Control controlFromTopBar = this.FocusedControlFromTopBar;
      godot_variant from = VariantUtils.CreateFrom<Control>(ref controlFromTopBar);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NCardGrid.PropertyName._startDrag))
    {
      value = VariantUtils.CreateFrom<float>(ref this._startDrag);
      return true;
    }
    if (StringName.op_Equality(ref name, NCardGrid.PropertyName._targetDrag))
    {
      value = VariantUtils.CreateFrom<float>(ref this._targetDrag);
      return true;
    }
    if (StringName.op_Equality(ref name, NCardGrid.PropertyName._isDragging))
    {
      value = VariantUtils.CreateFrom<bool>(ref this._isDragging);
      return true;
    }
    if (StringName.op_Equality(ref name, NCardGrid.PropertyName._scrollingEnabled))
    {
      value = VariantUtils.CreateFrom<bool>(ref this._scrollingEnabled);
      return true;
    }
    if (StringName.op_Equality(ref name, NCardGrid.PropertyName._scrollContainer))
    {
      value = VariantUtils.CreateFrom<Control>(ref this._scrollContainer);
      return true;
    }
    if (StringName.op_Equality(ref name, NCardGrid.PropertyName._scrollbarPressed))
    {
      value = VariantUtils.CreateFrom<bool>(ref this._scrollbarPressed);
      return true;
    }
    if (StringName.op_Equality(ref name, NCardGrid.PropertyName._scrollbar))
    {
      value = VariantUtils.CreateFrom<NScrollbar>(ref this._scrollbar);
      return true;
    }
    if (StringName.op_Equality(ref name, NCardGrid.PropertyName._slidingWindowCardIndex))
    {
      value = VariantUtils.CreateFrom<int>(ref this._slidingWindowCardIndex);
      return true;
    }
    if (StringName.op_Equality(ref name, NCardGrid.PropertyName._pileType))
    {
      value = VariantUtils.CreateFrom<PileType>(ref this._pileType);
      return true;
    }
    if (StringName.op_Equality(ref name, NCardGrid.PropertyName._cardSize))
    {
      value = VariantUtils.CreateFrom<Vector2>(ref this._cardSize);
      return true;
    }
    if (StringName.op_Equality(ref name, NCardGrid.PropertyName._cardsAnimatingOutForSetCards))
    {
      value = VariantUtils.CreateFrom<bool>(ref this._cardsAnimatingOutForSetCards);
      return true;
    }
    if (StringName.op_Equality(ref name, NCardGrid.PropertyName._isShowingUpgrades))
    {
      value = VariantUtils.CreateFrom<bool>(ref this._isShowingUpgrades);
      return true;
    }
    if (StringName.op_Equality(ref name, NCardGrid.PropertyName._lastFocusedHolder))
    {
      value = VariantUtils.CreateFrom<NCardHolder>(ref this._lastFocusedHolder);
      return true;
    }
    if (!StringName.op_Equality(ref name, NCardGrid.PropertyName._needsReinit))
      return ((GodotObject) this).GetGodotClassPropertyValue(ref name, ref value);
    value = VariantUtils.CreateFrom<bool>(ref this._needsReinit);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 3L, NCardGrid.PropertyName._startDrag, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 3L, NCardGrid.PropertyName._targetDrag, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 1L, NCardGrid.PropertyName._isDragging, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 1L, NCardGrid.PropertyName._scrollingEnabled, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 1L, NCardGrid.PropertyName.CanScroll, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 2L, NCardGrid.PropertyName.DisplayedRows, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 2L, NCardGrid.PropertyName.Columns, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 3L, NCardGrid.PropertyName.CardPadding, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 1L, NCardGrid.PropertyName.IsCardLibrary, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NCardGrid.PropertyName._scrollContainer, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 3L, NCardGrid.PropertyName.ScrollLimitBottom, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 3L, NCardGrid.PropertyName.ScrollLimitTop, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 1L, NCardGrid.PropertyName._scrollbarPressed, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NCardGrid.PropertyName._scrollbar, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 2L, NCardGrid.PropertyName._slidingWindowCardIndex, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 2L, NCardGrid.PropertyName._pileType, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 5L, NCardGrid.PropertyName._cardSize, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 1L, NCardGrid.PropertyName._cardsAnimatingOutForSetCards, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 1L, NCardGrid.PropertyName._isShowingUpgrades, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NCardGrid.PropertyName._lastFocusedHolder, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 1L, NCardGrid.PropertyName.IsAnimatingOut, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 1L, NCardGrid.PropertyName.IsShowingUpgrades, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 1L, NCardGrid.PropertyName._needsReinit, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 2L, NCardGrid.PropertyName.YOffset, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 1L, NCardGrid.PropertyName.CenterGrid, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NCardGrid.PropertyName.DefaultFocusedControl, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NCardGrid.PropertyName.FocusedControlFromTopBar, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).SaveGodotObjectData(info);
    GodotSerializationInfo serializationInfo1 = info;
    StringName displayedRows1 = NCardGrid.PropertyName.DisplayedRows;
    int displayedRows2 = this.DisplayedRows;
    Variant variant1 = Variant.From<int>(ref displayedRows2);
    serializationInfo1.AddProperty(displayedRows1, variant1);
    GodotSerializationInfo serializationInfo2 = info;
    StringName isShowingUpgrades1 = NCardGrid.PropertyName.IsShowingUpgrades;
    bool isShowingUpgrades2 = this.IsShowingUpgrades;
    Variant variant2 = Variant.From<bool>(ref isShowingUpgrades2);
    serializationInfo2.AddProperty(isShowingUpgrades1, variant2);
    GodotSerializationInfo serializationInfo3 = info;
    StringName yoffset1 = NCardGrid.PropertyName.YOffset;
    int yoffset2 = this.YOffset;
    Variant variant3 = Variant.From<int>(ref yoffset2);
    serializationInfo3.AddProperty(yoffset1, variant3);
    info.AddProperty(NCardGrid.PropertyName._startDrag, Variant.From<float>(ref this._startDrag));
    info.AddProperty(NCardGrid.PropertyName._targetDrag, Variant.From<float>(ref this._targetDrag));
    info.AddProperty(NCardGrid.PropertyName._isDragging, Variant.From<bool>(ref this._isDragging));
    info.AddProperty(NCardGrid.PropertyName._scrollingEnabled, Variant.From<bool>(ref this._scrollingEnabled));
    info.AddProperty(NCardGrid.PropertyName._scrollContainer, Variant.From<Control>(ref this._scrollContainer));
    info.AddProperty(NCardGrid.PropertyName._scrollbarPressed, Variant.From<bool>(ref this._scrollbarPressed));
    info.AddProperty(NCardGrid.PropertyName._scrollbar, Variant.From<NScrollbar>(ref this._scrollbar));
    info.AddProperty(NCardGrid.PropertyName._slidingWindowCardIndex, Variant.From<int>(ref this._slidingWindowCardIndex));
    info.AddProperty(NCardGrid.PropertyName._pileType, Variant.From<PileType>(ref this._pileType));
    info.AddProperty(NCardGrid.PropertyName._cardSize, Variant.From<Vector2>(ref this._cardSize));
    info.AddProperty(NCardGrid.PropertyName._cardsAnimatingOutForSetCards, Variant.From<bool>(ref this._cardsAnimatingOutForSetCards));
    info.AddProperty(NCardGrid.PropertyName._isShowingUpgrades, Variant.From<bool>(ref this._isShowingUpgrades));
    info.AddProperty(NCardGrid.PropertyName._lastFocusedHolder, Variant.From<NCardHolder>(ref this._lastFocusedHolder));
    info.AddProperty(NCardGrid.PropertyName._needsReinit, Variant.From<bool>(ref this._needsReinit));
    info.AddSignalEventDelegate(NCardGrid.SignalName.HolderPressed, (Delegate) this.backing_HolderPressed);
    info.AddSignalEventDelegate(NCardGrid.SignalName.HolderAltPressed, (Delegate) this.backing_HolderAltPressed);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NCardGrid.PropertyName.DisplayedRows, ref variant1))
      this.DisplayedRows = ((Variant) ref variant1).As<int>();
    Variant variant2;
    if (info.TryGetProperty(NCardGrid.PropertyName.IsShowingUpgrades, ref variant2))
      this.IsShowingUpgrades = ((Variant) ref variant2).As<bool>();
    Variant variant3;
    if (info.TryGetProperty(NCardGrid.PropertyName.YOffset, ref variant3))
      this.YOffset = ((Variant) ref variant3).As<int>();
    Variant variant4;
    if (info.TryGetProperty(NCardGrid.PropertyName._startDrag, ref variant4))
      this._startDrag = ((Variant) ref variant4).As<float>();
    Variant variant5;
    if (info.TryGetProperty(NCardGrid.PropertyName._targetDrag, ref variant5))
      this._targetDrag = ((Variant) ref variant5).As<float>();
    Variant variant6;
    if (info.TryGetProperty(NCardGrid.PropertyName._isDragging, ref variant6))
      this._isDragging = ((Variant) ref variant6).As<bool>();
    Variant variant7;
    if (info.TryGetProperty(NCardGrid.PropertyName._scrollingEnabled, ref variant7))
      this._scrollingEnabled = ((Variant) ref variant7).As<bool>();
    Variant variant8;
    if (info.TryGetProperty(NCardGrid.PropertyName._scrollContainer, ref variant8))
      this._scrollContainer = ((Variant) ref variant8).As<Control>();
    Variant variant9;
    if (info.TryGetProperty(NCardGrid.PropertyName._scrollbarPressed, ref variant9))
      this._scrollbarPressed = ((Variant) ref variant9).As<bool>();
    Variant variant10;
    if (info.TryGetProperty(NCardGrid.PropertyName._scrollbar, ref variant10))
      this._scrollbar = ((Variant) ref variant10).As<NScrollbar>();
    Variant variant11;
    if (info.TryGetProperty(NCardGrid.PropertyName._slidingWindowCardIndex, ref variant11))
      this._slidingWindowCardIndex = ((Variant) ref variant11).As<int>();
    Variant variant12;
    if (info.TryGetProperty(NCardGrid.PropertyName._pileType, ref variant12))
      this._pileType = ((Variant) ref variant12).As<PileType>();
    Variant variant13;
    if (info.TryGetProperty(NCardGrid.PropertyName._cardSize, ref variant13))
      this._cardSize = ((Variant) ref variant13).As<Vector2>();
    Variant variant14;
    if (info.TryGetProperty(NCardGrid.PropertyName._cardsAnimatingOutForSetCards, ref variant14))
      this._cardsAnimatingOutForSetCards = ((Variant) ref variant14).As<bool>();
    Variant variant15;
    if (info.TryGetProperty(NCardGrid.PropertyName._isShowingUpgrades, ref variant15))
      this._isShowingUpgrades = ((Variant) ref variant15).As<bool>();
    Variant variant16;
    if (info.TryGetProperty(NCardGrid.PropertyName._lastFocusedHolder, ref variant16))
      this._lastFocusedHolder = ((Variant) ref variant16).As<NCardHolder>();
    Variant variant17;
    if (info.TryGetProperty(NCardGrid.PropertyName._needsReinit, ref variant17))
      this._needsReinit = ((Variant) ref variant17).As<bool>();
    NCardGrid.HolderPressedEventHandler pressedEventHandler1;
    if (info.TryGetSignalEventDelegate<NCardGrid.HolderPressedEventHandler>(NCardGrid.SignalName.HolderPressed, ref pressedEventHandler1))
      this.backing_HolderPressed = pressedEventHandler1;
    NCardGrid.HolderAltPressedEventHandler pressedEventHandler2;
    if (!info.TryGetSignalEventDelegate<NCardGrid.HolderAltPressedEventHandler>(NCardGrid.SignalName.HolderAltPressed, ref pressedEventHandler2))
      return;
    this.backing_HolderAltPressed = pressedEventHandler2;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<MethodInfo> GetGodotSignalList()
  {
    return new List<MethodInfo>(2)
    {
      new MethodInfo(NCardGrid.SignalName.HolderPressed, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("card"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false)
      }, (List<Variant>) null),
      new MethodInfo(NCardGrid.SignalName.HolderAltPressed, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("card"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false)
      }, (List<Variant>) null)
    };
  }

  public event NCardGrid.HolderPressedEventHandler HolderPressed
  {
    add => this.backing_HolderPressed += value;
    remove => this.backing_HolderPressed -= value;
  }

  protected void EmitSignalHolderPressed(NCardHolder card)
  {
    ((GodotObject) this).EmitSignal(NCardGrid.SignalName.HolderPressed, new Variant[1]
    {
      Variant.op_Implicit((GodotObject) card)
    });
  }

  public event NCardGrid.HolderAltPressedEventHandler HolderAltPressed
  {
    add => this.backing_HolderAltPressed += value;
    remove => this.backing_HolderAltPressed -= value;
  }

  protected void EmitSignalHolderAltPressed(NCardHolder card)
  {
    ((GodotObject) this).EmitSignal(NCardGrid.SignalName.HolderAltPressed, new Variant[1]
    {
      Variant.op_Implicit((GodotObject) card)
    });
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RaiseGodotClassSignalCallbacks(
    in godot_string_name signal,
    NativeVariantPtrArgs args)
  {
    if (StringName.op_Equality(ref signal, NCardGrid.SignalName.HolderPressed) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      NCardGrid.HolderPressedEventHandler backingHolderPressed = this.backing_HolderPressed;
      if (backingHolderPressed == null)
        return;
      backingHolderPressed(VariantUtils.ConvertTo<NCardHolder>(ref ((NativeVariantPtrArgs) ref args)[0]));
    }
    else if (StringName.op_Equality(ref signal, NCardGrid.SignalName.HolderAltPressed) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      NCardGrid.HolderAltPressedEventHandler holderAltPressed = this.backing_HolderAltPressed;
      if (holderAltPressed == null)
        return;
      holderAltPressed(VariantUtils.ConvertTo<NCardHolder>(ref ((NativeVariantPtrArgs) ref args)[0]));
    }
    else
      ((GodotObject) this).RaiseGodotClassSignalCallbacks(ref signal, args);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassSignal(in godot_string_name signal)
  {
    return StringName.op_Equality(ref signal, NCardGrid.SignalName.HolderPressed) || StringName.op_Equality(ref signal, NCardGrid.SignalName.HolderAltPressed) || base.HasGodotClassSignal(ref signal);
  }

  [Signal]
  public delegate void HolderPressedEventHandler(
  #nullable enable
  NCardHolder card);

  [Signal]
  public delegate void HolderAltPressedEventHandler(NCardHolder card);

  public class MethodName : Control.MethodName
  {
    public static readonly 
    #nullable disable
    StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName ConnectSignals = StringName.op_Implicit(nameof (ConnectSignals));
    public static readonly StringName _EnterTree = StringName.op_Implicit(nameof (_EnterTree));
    public static readonly StringName _ExitTree = StringName.op_Implicit(nameof (_ExitTree));
    public static readonly StringName UpdateScrollLimitBottom = StringName.op_Implicit(nameof (UpdateScrollLimitBottom));
    public static readonly StringName _GuiInput = StringName.op_Implicit(nameof (_GuiInput));
    public static readonly StringName _Process = StringName.op_Implicit(nameof (_Process));
    public static readonly StringName _Notification = StringName.op_Implicit(nameof (_Notification));
    public static readonly StringName SetScrollPosition = StringName.op_Implicit(nameof (SetScrollPosition));
    public static readonly StringName SetCanScroll = StringName.op_Implicit(nameof (SetCanScroll));
    public static readonly StringName InsetForTopBar = StringName.op_Implicit(nameof (InsetForTopBar));
    public static readonly StringName ProcessMouseEvent = StringName.op_Implicit(nameof (ProcessMouseEvent));
    public static readonly StringName ProcessScrollEvent = StringName.op_Implicit(nameof (ProcessScrollEvent));
    public static readonly StringName ProcessGuiFocus = StringName.op_Implicit(nameof (ProcessGuiFocus));
    public static readonly StringName UpdateScrollPosition = StringName.op_Implicit(nameof (UpdateScrollPosition));
    public static readonly StringName ClearGrid = StringName.op_Implicit(nameof (ClearGrid));
    public static readonly StringName CalculateRowsNeeded = StringName.op_Implicit(nameof (CalculateRowsNeeded));
    public static readonly StringName InitGrid = StringName.op_Implicit(nameof (InitGrid));
    public static readonly StringName GetContainedCardsSize = StringName.op_Implicit(nameof (GetContainedCardsSize));
    public static readonly StringName ReflowColumns = StringName.op_Implicit(nameof (ReflowColumns));
    public static readonly StringName UpdateGridPositions = StringName.op_Implicit(nameof (UpdateGridPositions));
    public static readonly StringName OnHolderPressed = StringName.op_Implicit(nameof (OnHolderPressed));
    public static readonly StringName OnHolderAltPressed = StringName.op_Implicit(nameof (OnHolderAltPressed));
    public static readonly StringName GetTotalRowCount = StringName.op_Implicit(nameof (GetTotalRowCount));
    public static readonly StringName AllocateCardHolders = StringName.op_Implicit(nameof (AllocateCardHolders));
    public static readonly StringName ReallocateAll = StringName.op_Implicit(nameof (ReallocateAll));
    public static readonly StringName UpdateGridNavigation = StringName.op_Implicit(nameof (UpdateGridNavigation));
  }

  public class PropertyName : Control.PropertyName
  {
    public static readonly StringName CanScroll = StringName.op_Implicit(nameof (CanScroll));
    public static readonly StringName DisplayedRows = StringName.op_Implicit(nameof (DisplayedRows));
    public static readonly StringName Columns = StringName.op_Implicit(nameof (Columns));
    public static readonly StringName CardPadding = StringName.op_Implicit(nameof (CardPadding));
    public static readonly StringName IsCardLibrary = StringName.op_Implicit(nameof (IsCardLibrary));
    public static readonly StringName ScrollLimitBottom = StringName.op_Implicit(nameof (ScrollLimitBottom));
    public static readonly StringName ScrollLimitTop = StringName.op_Implicit(nameof (ScrollLimitTop));
    public static readonly StringName IsAnimatingOut = StringName.op_Implicit(nameof (IsAnimatingOut));
    public static readonly StringName IsShowingUpgrades = StringName.op_Implicit(nameof (IsShowingUpgrades));
    public static readonly StringName YOffset = StringName.op_Implicit(nameof (YOffset));
    public static readonly StringName CenterGrid = StringName.op_Implicit(nameof (CenterGrid));
    public static readonly StringName DefaultFocusedControl = StringName.op_Implicit(nameof (DefaultFocusedControl));
    public static readonly StringName FocusedControlFromTopBar = StringName.op_Implicit(nameof (FocusedControlFromTopBar));
    public static readonly StringName _startDrag = StringName.op_Implicit(nameof (_startDrag));
    public static readonly StringName _targetDrag = StringName.op_Implicit(nameof (_targetDrag));
    public static readonly StringName _isDragging = StringName.op_Implicit(nameof (_isDragging));
    public static readonly StringName _scrollingEnabled = StringName.op_Implicit(nameof (_scrollingEnabled));
    public static readonly StringName _scrollContainer = StringName.op_Implicit(nameof (_scrollContainer));
    public static readonly StringName _scrollbarPressed = StringName.op_Implicit(nameof (_scrollbarPressed));
    public static readonly StringName _scrollbar = StringName.op_Implicit(nameof (_scrollbar));
    public static readonly StringName _slidingWindowCardIndex = StringName.op_Implicit(nameof (_slidingWindowCardIndex));
    public static readonly StringName _pileType = StringName.op_Implicit(nameof (_pileType));
    public static readonly StringName _cardSize = StringName.op_Implicit(nameof (_cardSize));
    public static readonly StringName _cardsAnimatingOutForSetCards = StringName.op_Implicit(nameof (_cardsAnimatingOutForSetCards));
    public static readonly StringName _isShowingUpgrades = StringName.op_Implicit(nameof (_isShowingUpgrades));
    public static readonly StringName _lastFocusedHolder = StringName.op_Implicit(nameof (_lastFocusedHolder));
    public static readonly StringName _needsReinit = StringName.op_Implicit(nameof (_needsReinit));
  }

  public class SignalName : Control.SignalName
  {
    public static readonly StringName HolderPressed = StringName.op_Implicit(nameof (HolderPressed));
    public static readonly StringName HolderAltPressed = StringName.op_Implicit(nameof (HolderAltPressed));
  }
}
