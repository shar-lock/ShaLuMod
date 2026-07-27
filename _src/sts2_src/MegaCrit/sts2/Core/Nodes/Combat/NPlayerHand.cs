// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Combat.NPlayerHand
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.ControllerInput;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Cards;
using MegaCrit.Sts2.Core.Nodes.Cards.Holders;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Screens.Overlays;
using MegaCrit.Sts2.Core.Nodes.Screens.ScreenContext;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Runs;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Combat;

[ScriptPath("res://src/Core/Nodes/Combat/NPlayerHand.cs")]
public class NPlayerHand : Control
{
  private StringName[] _selectCardShortcuts = new StringName[10]
  {
    MegaInput.selectCard1,
    MegaInput.selectCard2,
    MegaInput.selectCard3,
    MegaInput.selectCard4,
    MegaInput.selectCard5,
    MegaInput.selectCard6,
    MegaInput.selectCard7,
    MegaInput.selectCard8,
    MegaInput.selectCard9,
    MegaInput.selectCard10
  };
  private Control _selectModeBackstop;
  private readonly List<CardModel> _selectedCards = new List<CardModel>();
  private CardSelectorPrefs _prefs;
  private TaskCompletionSource<IEnumerable<CardModel>>? _selectionCompletionSource;
  private Control _upgradePreviewContainer;
  private NSelectedHandCardContainer _selectedHandCardContainer;
  private NUpgradePreview _upgradePreview;
  private NConfirmButton _selectModeConfirmButton;
  private MegaRichTextLabel _selectionHeader;
  private NCardPlay? _currentCardPlay;
  private CombatState? _combatState;
  private NPlayerHand.Mode _currentMode = NPlayerHand.Mode.Play;
  private Func<CardModel, bool>? _currentSelectionFilter;
  private int _draggedHolderIndex = -1;
  private int _lastFocusedHolderIdx = -1;
  private readonly HashSet<NHandCardHolder> _holdersAwaitingQueue = new HashSet<NHandCardHolder>();
  private Tween? _animEnableTween;
  private bool _isDisabled;
  private const double _enableDisableDuration = 0.2;
  private static readonly Vector2 _disablePosition = new Vector2(0.0f, 100f);
  private static readonly Color _disableModulate = StsColors.gray;
  private Tween? _animInTween;
  private Tween? _animOutTween;
  private Tween? _selectedCardScaleTween;
  private const float _showHideAnimDuration = 0.8f;
  private static readonly Vector2 _showPosition = Vector2.Zero;
  private static readonly Vector2 _hidePosition = new Vector2(0.0f, 500f);
  private 
  #nullable disable
  NPlayerHand.ModeChangedEventHandler backing_ModeChanged;

  public static 
  #nullable enable
  NPlayerHand? Instance => NCombatRoom.Instance?.Ui.Hand;

  public Control CardHolderContainer { get; private set; }

  public NPeekButton PeekButton { get; private set; }

  public bool InCardPlay
  {
    get
    {
      return this._currentCardPlay != null && GodotObject.IsInstanceValid((GodotObject) this._currentCardPlay);
    }
  }

  public bool IsInCardSelection
  {
    get
    {
      bool isInCardSelection;
      switch (this.CurrentMode)
      {
        case NPlayerHand.Mode.SimpleSelect:
        case NPlayerHand.Mode.UpgradeSelect:
          isInCardSelection = true;
          break;
        default:
          isInCardSelection = false;
          break;
      }
      return isInCardSelection;
    }
  }

  public NPlayerHand.Mode CurrentMode
  {
    get => this._currentMode;
    private set
    {
      this._currentMode = value;
      ((GodotObject) this).EmitSignal(NPlayerHand.SignalName.ModeChanged, Array.Empty<Variant>());
    }
  }

  private bool HasDraggedHolder => this._draggedHolderIndex >= 0;

  public Func<CardModel, bool>? SelectModeGoldGlowOverride => this._prefs.ShouldGlowGold;

  public NHandCardHolder? FocusedHolder { get; private set; }

  public IReadOnlyList<NHandCardHolder> ActiveHolders
  {
    get
    {
      return (IReadOnlyList<NHandCardHolder>) this.Holders.Where<NHandCardHolder>((Func<NHandCardHolder, bool>) (child => ((CanvasItem) child).IsVisibleInTree())).ToList<NHandCardHolder>();
    }
  }

  private IReadOnlyList<NHandCardHolder> Holders
  {
    get
    {
      return (IReadOnlyList<NHandCardHolder>) ((IEnumerable) ((Node) this.CardHolderContainer).GetChildren(false)).OfType<NHandCardHolder>().ToList<NHandCardHolder>();
    }
  }

  public override void _Ready()
  {
    this._selectModeBackstop = ((Node) this).GetNode<Control>(NodePath.op_Implicit("%SelectModeBackstop"));
    this.CardHolderContainer = ((Node) this).GetNode<Control>(NodePath.op_Implicit("%CardHolderContainer"));
    this._upgradePreviewContainer = ((Node) this).GetNode<Control>(NodePath.op_Implicit("%UpgradePreviewContainer"));
    this._selectModeConfirmButton = ((Node) this).GetNode<NConfirmButton>(NodePath.op_Implicit("%SelectModeConfirmButton"));
    this._upgradePreview = ((Node) this).GetNode<NUpgradePreview>(NodePath.op_Implicit("%UpgradePreview"));
    this._selectionHeader = ((Node) this).GetNode<MegaRichTextLabel>(NodePath.op_Implicit("%SelectionHeader"));
    ((CanvasItem) this._selectionHeader).Visible = false;
    this._selectedHandCardContainer = ((Node) this).GetNode<NSelectedHandCardContainer>(NodePath.op_Implicit("%SelectedHandCardContainer"));
    this._selectedHandCardContainer.Hand = this;
    ((GodotObject) this._selectModeConfirmButton).Connect(NClickableControl.SignalName.Released, Callable.From<NButton>(new Action<NButton>(this.OnSelectModeConfirmButtonPressed)), 0U);
    this._selectModeConfirmButton.Disable();
    ((GodotObject) this._selectedHandCardContainer).Connect(Node.SignalName.ChildExitingTree, Callable.From<Node>(new Action<Node>(this.OnCardDeselected)), 0U);
    ((GodotObject) this._selectedHandCardContainer).Connect(Node.SignalName.ChildEnteredTree, Callable.From<Node>(new Action<Node>(this.OnCardSelected)), 0U);
    this.PeekButton = ((Node) this).GetNode<NPeekButton>(NodePath.op_Implicit("%PeekButton"));
    this.PeekButton.Disable();
    this.PeekButton.AddTargets(this._selectModeBackstop, this._upgradePreviewContainer, (Control) this._selectModeConfirmButton, (Control) this._selectionHeader, (Control) this._selectedHandCardContainer);
    ((GodotObject) this.PeekButton).Connect(NPeekButton.SignalName.Toggled, Callable.From<NPeekButton>(new Action<NPeekButton>(this.OnPeekButtonToggled)), 0U);
    ((GodotObject) this.CardHolderContainer).Connect(Control.SignalName.FocusEntered, Callable.From((Action) (() => this.DefaultFocusedControl.TryGrabFocus())), 0U);
    this.CardHolderContainer.FocusNeighborBottom = ((Node) this.CardHolderContainer).GetPath();
    this.CardHolderContainer.FocusNeighborLeft = ((Node) this.CardHolderContainer).GetPath();
    this.CardHolderContainer.FocusNeighborRight = ((Node) this.CardHolderContainer).GetPath();
  }

  public override void _EnterTree()
  {
    ((Node) this)._EnterTree();
    CombatManager.Instance.PlayerActionsDisabledChanged += new Action<CombatState>(this.OnPlayerActionsDisabledChanged);
    CombatManager.Instance.PlayerUnendedTurn += new Action<Player>(this.OnPlayerUnendedTurn);
    CombatManager.Instance.StateTracker.CombatStateChanged += new Action<CombatState>(this.OnCombatStateChanged);
    CombatManager.Instance.CombatEnded += new Action<CombatRoom>(this.OnCombatEnded);
  }

  public override void _ExitTree()
  {
    ((Node) this)._ExitTree();
    TaskCompletionSource<IEnumerable<CardModel>> completionSource = this._selectionCompletionSource;
    if (completionSource != null)
    {
      Task<IEnumerable<CardModel>> task = completionSource.Task;
      if (task != null && !((Task) task).IsCompleted)
        this._selectionCompletionSource.SetResult((IEnumerable<CardModel>) Array.Empty<CardModel>());
    }
    CombatManager.Instance.PlayerActionsDisabledChanged -= new Action<CombatState>(this.OnPlayerActionsDisabledChanged);
    CombatManager.Instance.PlayerUnendedTurn -= new Action<Player>(this.OnPlayerUnendedTurn);
    CombatManager.Instance.StateTracker.CombatStateChanged -= new Action<CombatState>(this.OnCombatStateChanged);
    CombatManager.Instance.CombatEnded -= new Action<CombatRoom>(this.OnCombatEnded);
  }

  public NCard? GetCard(CardModel card) => this.GetCardHolder(card)?.CardNode;

  public bool IsAwaitingPlay(NHandCardHolder? holder)
  {
    return holder != null && this._holdersAwaitingQueue.Contains(holder);
  }

  public NCardHolder? GetCardHolder(CardModel card)
  {
    return ((IEnumerable<NCardHolder>) this.Holders).Concat<NCardHolder>((IEnumerable<NCardHolder>) this._selectedHandCardContainer.Holders).Concat<NCardHolder>((IEnumerable<NCardHolder>) this._holdersAwaitingQueue).FirstOrDefault<NCardHolder>((Func<NCardHolder, bool>) (h => h.CardNode != null && h.CardNode.Model == card));
  }

  public NHandCardHolder Add(NCard card, int index = -1)
  {
    Vector2 globalPosition = card.GlobalPosition;
    NHandCardHolder holder = NHandCardHolder.Create(card, this);
    this.AddCardHolder(holder, index);
    holder.GlobalPosition = globalPosition;
    this.RefreshLayout();
    if (this.IsInCardSelection)
      this.UpdateSelectModeCardVisibility();
    return holder;
  }

  public void Remove(CardModel card)
  {
    NCardHolder cardHolder = this.GetCardHolder(card);
    if (cardHolder == null)
      throw new InvalidOperationException($"No holder for card {card.Id}");
    if (this.InCardPlay && card == this._currentCardPlay.Holder.CardModel)
      this._currentCardPlay.CancelPlayCard();
    this.RemoveCardHolder(cardHolder);
  }

  private void AddCardHolder(NHandCardHolder holder, int index)
  {
    ((Node) this.CardHolderContainer).AddChildSafely((Node) holder);
    if (index >= 0)
      ((Node) this.CardHolderContainer).MoveChildSafely((Node) holder, index);
    ((GodotObject) holder).Connect(NCardHolder.SignalName.Pressed, Callable.From<NCardHolder>(new Action<NCardHolder>(this.OnHolderPressed)), 0U);
    ((GodotObject) holder).Connect(NHandCardHolder.SignalName.HolderMouseClicked, Callable.From<NCardHolder>(new Action<NCardHolder>(this.OnHolderPressed)), 0U);
    ((GodotObject) holder).Connect(NHandCardHolder.SignalName.HolderFocused, Callable.From<NHandCardHolder>(new Action<NHandCardHolder>(this.OnHolderFocused)), 0U);
    ((GodotObject) holder).Connect(NHandCardHolder.SignalName.HolderUnfocused, Callable.From<NHandCardHolder>(new Action<NHandCardHolder>(this.OnHolderUnfocused)), 0U);
    this.RefreshLayout();
    if (!this.CardHolderContainer.HasFocus())
      return;
    holder.TryGrabFocus();
  }

  private int GetHandInsertIndex(CardModel card)
  {
    return HandLayoutHelper.GetInsertIndex<CardModel>(PileType.Hand.GetPile(card.Owner).Cards, (IEnumerable<CardModel>) this.Holders.Where<NHandCardHolder>((Func<NHandCardHolder, bool>) (holder => holder.CardNode?.Model != null)).Select<NHandCardHolder, CardModel>((Func<NHandCardHolder, CardModel>) (holder => holder.CardNode.Model)).ToList<CardModel>(), card);
  }

  public void RemoveCardHolder(NCardHolder holder)
  {
    if (holder is NHandCardHolder nhandCardHolder)
      this._holdersAwaitingQueue.Remove(nhandCardHolder);
    if (this.InCardPlay && this._currentCardPlay.Holder == holder)
      this._currentCardPlay.CancelPlayCard();
    bool flag = holder.HasFocus();
    ((Node) holder).GetParent().RemoveChildSafely((Node) holder);
    holder.Clear();
    ((Node) holder).QueueFreeSafely();
    this.RefreshLayout();
    if (!flag)
      return;
    this.DefaultFocusedControl.TryGrabFocus();
  }

  private void OnHolderFocused(NHandCardHolder holder)
  {
    this.FocusedHolder = holder;
    this._lastFocusedHolderIdx = ((Node) holder).GetIndex(false);
    RunManager.Instance.HoveredModelTracker.OnLocalCardHovered(this.FocusedHolder.CardModel);
    this.RefreshLayout();
  }

  private void OnHolderUnfocused(NHandCardHolder holder)
  {
    this.FocusedHolder = (NHandCardHolder) null;
    RunManager.Instance.HoveredModelTracker.OnLocalCardUnhovered();
    this.RefreshLayout();
  }

  public void TryCancelCardPlay(CardModel card)
  {
    if (!(this.GetCardHolder(card) is NHandCardHolder cardHolder) || !this.IsAwaitingPlay(cardHolder))
      return;
    this.ReturnHolderToHand(cardHolder);
    cardHolder.UpdateCard();
    if (this.InCardPlay && this._currentCardPlay.Holder == cardHolder)
      this._currentCardPlay.CancelPlayCard();
    else
      this.RefreshLayout();
  }

  public void CancelAllCardPlay()
  {
    if (this.InCardPlay)
      this._currentCardPlay.CancelPlayCard();
    foreach (NHandCardHolder holder in this._holdersAwaitingQueue.ToList<NHandCardHolder>())
      this.ReturnHolderToHand(holder);
  }

  private void ReturnHolderToHand(NHandCardHolder holder)
  {
    if (!this.IsAwaitingPlay(holder))
      return;
    this._holdersAwaitingQueue.Remove(holder);
    ((Node) holder).Reparent((Node) this.CardHolderContainer, true);
    int handInsertIndex = this.GetHandInsertIndex(holder.CardNode.Model);
    if (handInsertIndex >= 0)
      ((Node) this.CardHolderContainer).MoveChildSafely((Node) holder, handInsertIndex);
    holder.SetDefaultTargets();
  }

  public void ForceRefreshCardIndices() => this.RefreshLayout();

  private void RefreshLayout()
  {
    int count = this.ActiveHolders.Count;
    if (count <= 0)
      return;
    int handSize = count;
    Vector2 scale = HandPosHelper.GetScale(count);
    int num1 = -1;
    if (this.FocusedHolder != null)
      num1 = this.ActiveHolders.IndexOf<NHandCardHolder>(this.FocusedHolder);
    for (int index = 0; index < count; ++index)
    {
      int cardIndex = index;
      Vector2 position = HandPosHelper.GetPosition(handSize, cardIndex);
      if (num1 > -1)
      {
        float num2 = Mathf.Lerp(100f, 0.0f, Mathf.Min(1f, (float) Mathf.Abs(num1 - index) / 4f));
        position = Vector2.op_Addition(position, Vector2.op_Multiply(Vector2.op_Multiply(Vector2.Left, (float) Mathf.Sign(num1 - index)), num2));
      }
      NHandCardHolder activeHolder = this.ActiveHolders[index];
      if (num1 == index)
      {
        activeHolder.SetAngleInstantly(0.0f);
        activeHolder.SetScaleInstantly(Vector2.One);
        position.Y = (float) (-(double) activeHolder.Hitbox.Size.Y * 0.5 + 2.0);
        if (this._isDisabled)
          position = Vector2.op_Subtraction(position, NPlayerHand._disablePosition);
        activeHolder.Position = new Vector2(activeHolder.Position.X, position.Y);
        activeHolder.SetTargetPosition(position);
      }
      else
      {
        activeHolder.SetTargetPosition(position);
        activeHolder.SetTargetScale(scale);
        activeHolder.SetTargetAngle(HandPosHelper.GetAngle(handSize, cardIndex));
      }
      activeHolder.Hitbox.MouseFilter = this.HasDraggedHolder ? (Control.MouseFilterEnum) 2L : (Control.MouseFilterEnum) 0L;
      NHandCardHolder nhandCardHolder = activeHolder;
      NodePath path;
      if (index <= 0)
      {
        IReadOnlyList<NHandCardHolder> activeHolders = this.ActiveHolders;
        path = ((Node) activeHolders[activeHolders.Count - 1]).GetPath();
      }
      else
        path = ((Node) this.ActiveHolders[index - 1]).GetPath();
      nhandCardHolder.FocusNeighborLeft = path;
      activeHolder.FocusNeighborRight = index < this.ActiveHolders.Count - 1 ? ((Node) this.ActiveHolders[index + 1]).GetPath() : ((Node) this.ActiveHolders[0]).GetPath();
      activeHolder.FocusNeighborBottom = ((Node) activeHolder).GetPath();
      if (this.HasDraggedHolder && index >= this._draggedHolderIndex)
        activeHolder.SetIndexLabel(index + 2);
      else
        activeHolder.SetIndexLabel(index + 1);
    }
  }

  private void OnPlayerUnendedTurn(Player player)
  {
    this.UpdateHandDisabledState(player.Creature.CombatState);
  }

  private void OnPlayerActionsDisabledChanged(CombatState state)
  {
    this.UpdateHandDisabledState((ICombatState) state);
  }

  private void UpdateHandDisabledState(ICombatState state)
  {
    Player me = LocalContext.GetMe(state);
    bool flag1 = CombatManager.Instance.PlayerActionsDisabled;
    if (!flag1 && CombatManager.Instance.PlayersTakingExtraTurn.Count > 0 && me != null && !CombatManager.Instance.PlayersTakingExtraTurn.Contains<Player>(me))
      flag1 = true;
    if (flag1)
    {
      // ISSUE: object of a compiler-generated type is created
      bool flag2 = me == null || !state.Players.Except<Player>((IEnumerable<Player>) new \u003C\u003Ez__ReadOnlySingleElementList<Player>(me)).All<Player>(new Func<Player, bool>(CombatManager.Instance.IsPlayerReadyToEndTurn));
      if (!(state.CurrentSide == CombatSide.Enemy | flag2))
        return;
      this.AnimDisable();
    }
    else
      this.AnimEnable();
  }

  private void OnCombatStateChanged(CombatState state)
  {
    this._combatState = state;
    if (this.IsInCardSelection)
      this.RevalidateSelectionAfterStateChange();
    foreach (NHandCardHolder holder in (IEnumerable<NHandCardHolder>) this.Holders)
      holder.UpdateCard();
    foreach (NHandCardHolder holdersAwaiting in this._holdersAwaitingQueue)
      holdersAwaiting.UpdateCard();
    foreach (NCardHolder holder in this._selectedHandCardContainer.Holders)
      holder.CardNode?.UpdateVisuals(PileType.Hand, CardPreviewMode.Normal);
    this.UpdateHandDisabledState((ICombatState) state);
  }

  private void OnCombatEnded(CombatRoom _)
  {
    this.CancelAllCardPlay();
    this.CardHolderContainer.FocusMode = (Control.FocusModeEnum) 0L;
  }

  private void OnPeekButtonToggled(NPeekButton button)
  {
    if (button.IsPeeking)
    {
      NCombatRoom.Instance.EnableControllerNavigation();
      this._selectModeConfirmButton.Disable();
    }
    else
    {
      NCombatRoom.Instance.RestrictControllerNavigation((IEnumerable<Control>) Array.Empty<Control>());
      this.EnableControllerNavigation();
      this.RefreshSelectModeConfirmButton();
      Viewport viewport = ((Node) this).GetViewport();
      if (viewport != null && viewport.GuiGetFocusOwner() == this.CardHolderContainer)
        this.DefaultFocusedControl.TryGrabFocus();
    }
    this.UpdateSelectModeCardVisibility();
    ActiveScreenContext.Instance.Update();
  }

  public async Task<IEnumerable<CardModel>> SelectCards(
    CardSelectorPrefs prefs,
    Func<CardModel, bool>? filter,
    AbstractModel? source,
    NPlayerHand.Mode mode = NPlayerHand.Mode.SimpleSelect)
  {
    this.CancelAllCardPlay();
    ((CanvasItem) this._selectModeBackstop).Visible = true;
    this._selectModeBackstop.MouseFilter = (Control.MouseFilterEnum) 0L;
    Control selectModeBackstop = this._selectModeBackstop;
    Color selfModulate = ((CanvasItem) this._selectModeBackstop).SelfModulate;
    selfModulate.A = 0.0f;
    Color color = selfModulate;
    ((CanvasItem) selectModeBackstop).SelfModulate = color;
    Tween tween = ((Node) this).CreateTween();
    tween.TweenProperty((GodotObject) this._selectModeBackstop, NodePath.op_Implicit("self_modulate:a"), Variant.op_Implicit(1f), 0.20000000298023224);
    bool wasDisabled = this._isDisabled;
    if (this._isDisabled)
      this.AnimEnable();
    this.CurrentMode = mode;
    this._currentSelectionFilter = filter;
    NCombatRoom.Instance.RestrictControllerNavigation((IEnumerable<Control>) Array.Empty<Control>());
    NCombatRoom.Instance.Ui.OnHandSelectModeEntered();
    this.EnableControllerNavigation();
    this._prefs = prefs;
    this._selectionCompletionSource = new TaskCompletionSource<IEnumerable<CardModel>>();
    ((CanvasItem) this._selectionHeader).Visible = true;
    this._selectionHeader.Text = $"[center]{prefs.Prompt.GetFormattedText()}[/center]";
    this.PeekButton.Enable();
    this.UpdateSelectModeCardVisibility();
    this.RefreshSelectModeConfirmButton();
    IEnumerable<CardModel> task = await this._selectionCompletionSource.Task;
    if (!((Node) this).IsInsideTree())
      return task;
    tween.Kill();
    this.AfterCardsSelected(source);
    if (wasDisabled)
      this.AnimDisable();
    NCombatRoom.Instance?.EnableControllerNavigation();
    return task;
  }

  private void UpdateSelectModeCardVisibility()
  {
    if (this.CurrentMode != NPlayerHand.Mode.SimpleSelect && this.CurrentMode != NPlayerHand.Mode.UpgradeSelect)
      throw new InvalidOperationException("Can only be used when we are selecting a card");
    foreach (NHandCardHolder holder in (IEnumerable<NHandCardHolder>) this.Holders)
    {
      if (holder.CardNode != null)
      {
        if (this.PeekButton.IsPeeking)
        {
          ((CanvasItem) holder).Visible = true;
          holder.CardNode.SetPretendCardCanBePlayed(false);
          holder.CardNode.SetForceUnpoweredPreview(false);
        }
        else
        {
          NHandCardHolder nhandCardHolder = holder;
          Func<CardModel, bool> currentSelectionFilter = this._currentSelectionFilter;
          int num = currentSelectionFilter != null ? (currentSelectionFilter(holder.CardNode.Model) ? 1 : 0) : 1;
          ((CanvasItem) nhandCardHolder).Visible = num != 0;
          holder.CardNode.SetPretendCardCanBePlayed(this._prefs.PretendCardsCanBePlayed);
          holder.CardNode.SetForceUnpoweredPreview(this._prefs.UnpoweredPreviews);
        }
        holder.UpdateCard();
      }
    }
    this.RefreshLayout();
  }

  private void AfterCardsSelected(AbstractModel? source)
  {
    this._selectedCards.Clear();
    this.CurrentMode = NPlayerHand.Mode.Play;
    this._prefs = new CardSelectorPrefs();
    this._currentSelectionFilter = (Func<CardModel, bool>) null;
    foreach (NHandCardHolder holder in (IEnumerable<NHandCardHolder>) this.Holders)
    {
      holder.InSelectMode = false;
      ((CanvasItem) holder).Visible = true;
      holder.CardNode?.SetPretendCardCanBePlayed(false);
      holder.CardNode?.SetForceUnpoweredPreview(false);
      holder.UpdateCard();
    }
    this.RefreshLayout();
    ((CanvasItem) this._selectModeBackstop).Visible = false;
    this._selectModeBackstop.MouseFilter = (Control.MouseFilterEnum) 2L;
    ((Node) this).CreateTween().TweenProperty((GodotObject) this._selectModeBackstop, NodePath.op_Implicit("self_modulate:a"), Variant.op_Implicit(0.0f), 0.20000000298023224);
    this._selectModeConfirmButton.Disable();
    ((CanvasItem) this._upgradePreviewContainer).Visible = false;
    ((CanvasItem) this._selectionHeader).Visible = false;
    this.PeekButton.Disable();
    NCombatRoom.Instance.Ui.OnHandSelectModeExited();
    if (source != null)
      source.ExecutionFinished += new Action<AbstractModel>(this.OnSelectModeSourceFinished);
    else
      this.OnSelectModeSourceFinished((AbstractModel) null);
  }

  private void CancelHandSelectionIfNecessary()
  {
    if (!this.IsInCardSelection || this._selectionCompletionSource == null)
      return;
    this._selectionCompletionSource.SetCanceled();
    this.AfterCardsSelected((AbstractModel) null);
  }

  private void OnHolderPressed(NCardHolder holder)
  {
    if (this.PeekButton.IsPeeking)
    {
      this.PeekButton.Wiggle();
    }
    else
    {
      NHandCardHolder holder1 = (NHandCardHolder) holder;
      if (holder1.CardNode == null || !CombatManager.Instance.IsInProgress || NOverlayStack.Instance.ScreenCount > 0)
        return;
      switch (this.CurrentMode)
      {
        case NPlayerHand.Mode.None:
          break;
        case NPlayerHand.Mode.Play:
          if (!this.CanPlayCards())
            break;
          this.StartCardPlay(holder1, false);
          break;
        case NPlayerHand.Mode.SimpleSelect:
          this.SelectCardInSimpleMode(holder1);
          break;
        case NPlayerHand.Mode.UpgradeSelect:
          this.SelectCardInUpgradeMode(holder1);
          break;
        default:
          throw new ArgumentOutOfRangeException("CurrentMode");
      }
    }
  }

  private bool CanPlayCards() => !this.InCardPlay && this.AreCardActionsAllowed();

  private bool AreCardActionsAllowed()
  {
    if (CombatManager.Instance.PlayersTakingExtraTurn.Count > 0 && this._combatState != null)
    {
      Player me = LocalContext.GetMe((ICombatState) this._combatState);
      if (me == null || !CombatManager.Instance.PlayersTakingExtraTurn.Contains<Player>(me))
        return false;
    }
    return !CombatManager.Instance.PlayerActionsDisabled && !this.PeekButton.IsPeeking;
  }

  private void StartCardPlay(NHandCardHolder holder, bool startedViaShortcut)
  {
    this._draggedHolderIndex = ((Node) holder).GetIndex(false);
    this._holdersAwaitingQueue.Add(holder);
    ((Node) holder).Reparent((Node) this, true);
    holder.BeginDrag();
    this._currentCardPlay = NControllerManager.Instance.IsUsingController ? (NCardPlay) NControllerCardPlay.Create(holder) : (NCardPlay) NMouseCardPlay.Create(holder, this._selectCardShortcuts[this._draggedHolderIndex], startedViaShortcut);
    ((Node) this).AddChildSafely((Node) this._currentCardPlay);
    ((GodotObject) this._currentCardPlay).Connect(NCardPlay.SignalName.Finished, Callable.From<bool>((Action<bool>) (success =>
    {
      RunManager.Instance.HoveredModelTracker.OnLocalCardDeselected();
      if (!success)
        this.ReturnHolderToHand(holder);
      this._draggedHolderIndex = -1;
      this.RefreshLayout();
    })), 0U);
    RunManager.Instance.HoveredModelTracker.OnLocalCardSelected(holder.CardNode.Model);
    this._currentCardPlay.Start();
    this.RefreshLayout();
    holder.SetIndexLabel(this._draggedHolderIndex + 1);
  }

  private void SelectCardInSimpleMode(NHandCardHolder holder)
  {
    if (this._selectedCards.Count >= this._prefs.MaxSelect)
      this._selectedHandCardContainer.DeselectCard(this._selectedCards.Last<CardModel>());
    this._selectedCards.Add(holder.CardNode.Model);
    this._selectedHandCardContainer.Add(holder);
    this.RemoveCardHolder((NCardHolder) holder);
    this.RefreshSelectModeConfirmButton();
  }

  private void SelectCardInUpgradeMode(NHandCardHolder holder)
  {
    CardModel model = holder.CardNode.Model;
    if (this._selectedCards.Count != 0)
    {
      NCard card = NCard.Create(this._selectedCards.Last<CardModel>());
      card.GlobalPosition = this._upgradePreview.SelectedCardPosition;
      this.DeselectCard(card);
    }
    this._selectedCards.Add(model);
    ((CanvasItem) this._upgradePreviewContainer).Visible = true;
    this._upgradePreview.Card = model;
    this.RemoveCardHolder((NCardHolder) holder);
    this.RefreshSelectModeConfirmButton();
  }

  public void DeselectCard(NCard card)
  {
    if (!this.IsInCardSelection)
      throw new InvalidOperationException("Only valid when in Select Mode.");
    NHandCardHolder control = this.Add(card, this.GetHandInsertIndex(card.Model));
    control.InSelectMode = true;
    ((CanvasItem) control).Visible = true;
    this._selectedCards.Remove(card.Model);
    this.RefreshSelectModeConfirmButton();
    control.TryGrabFocus();
  }

  private void RevalidateSelectionAfterStateChange()
  {
    Func<CardModel, bool> filter = this._currentSelectionFilter;
    if (filter == null)
      return;
    if (this._upgradePreview.Card != null && !filter(this._upgradePreview.Card))
    {
      CardModel card = this._upgradePreview.Card;
      this._upgradePreview.Card = (CardModel) null;
      ((CanvasItem) this._upgradePreviewContainer).Visible = false;
      this._selectedCards.Remove(card);
      this.Add(NCard.Create(card), this.GetHandInsertIndex(card));
    }
    foreach (NCardHolder ncardHolder in this._selectedHandCardContainer.Holders.ToList<NSelectedHandCardHolder>())
    {
      CardModel model = ncardHolder.CardNode?.Model;
      if (model != null && !filter(model))
        this._selectedHandCardContainer.DeselectCard(model);
    }
    this.UpdateSelectModeCardVisibility();
    bool flag = this.Holders.Any<NHandCardHolder>((Func<NHandCardHolder, bool>) (h => h.CardNode != null && filter(h.CardNode.Model)));
    if (this._selectedCards.Count == 0 && !flag)
      this._selectionCompletionSource?.TrySetResult((IEnumerable<CardModel>) Array.Empty<CardModel>());
    else
      this.RefreshSelectModeConfirmButton();
  }

  private void OnSelectModeConfirmButtonPressed(NButton _)
  {
    this._selectionCompletionSource.SetResult((IEnumerable<CardModel>) this._selectedCards.ToList<CardModel>());
  }

  private void CheckIfSelectionComplete()
  {
    if (this._selectedCards.Count < this._prefs.MaxSelect)
      return;
    this._selectionCompletionSource.SetResult((IEnumerable<CardModel>) this._selectedCards.ToList<CardModel>());
  }

  private void RefreshSelectModeConfirmButton()
  {
    int count = this._selectedCards.Count;
    if (count >= this._prefs.MinSelect && count <= this._prefs.MaxSelect)
      this._selectModeConfirmButton.Enable();
    else
      this._selectModeConfirmButton.Disable();
  }

  private void OnSelectModeSourceFinished(AbstractModel? source)
  {
    foreach (NSelectedHandCardHolder nselectedHandCardHolder in this._selectedHandCardContainer.Holders.ToList<NSelectedHandCardHolder>())
    {
      NCard cardNode = nselectedHandCardHolder.CardNode;
      ((Node) nselectedHandCardHolder).QueueFreeSafely();
      this.Add(cardNode);
    }
    if (this._upgradePreview.Card != null)
    {
      this.Add(NCard.Create(this._upgradePreview.Card));
      this._upgradePreview.Card = (CardModel) null;
    }
    if (source == null)
      return;
    source.ExecutionFinished -= new Action<AbstractModel>(this.OnSelectModeSourceFinished);
  }

  public void AnimIn()
  {
    this._animOutTween?.Kill();
    this._animEnableTween?.Kill();
    this._animInTween = ((Node) this).CreateTween();
    this._animInTween.TweenProperty((GodotObject) this, NodePath.op_Implicit("position"), Variant.op_Implicit(NPlayerHand._showPosition), 0.800000011920929).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 5L);
  }

  public void AnimOut()
  {
    this.CancelHandSelectionIfNecessary();
    this._animInTween?.Kill();
    this._animEnableTween?.Kill();
    this._animOutTween = ((Node) this).CreateTween();
    this._animOutTween.TweenProperty((GodotObject) this, NodePath.op_Implicit("position"), Variant.op_Implicit(NPlayerHand._hidePosition), 0.800000011920929).SetEase((Tween.EaseType) 0L).SetTrans((Tween.TransitionType) 10L);
  }

  private void AnimDisable()
  {
    if (this._isDisabled)
      return;
    this._animEnableTween = ((Node) this).CreateTween().SetParallel(true);
    this._animEnableTween.TweenProperty((GodotObject) this, NodePath.op_Implicit("position"), Variant.op_Implicit(NPlayerHand._disablePosition), 0.2).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 7L);
    this._animEnableTween.TweenProperty((GodotObject) this, NodePath.op_Implicit("modulate"), Variant.op_Implicit(NPlayerHand._disableModulate), 0.2).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 7L);
    this._isDisabled = true;
    this.DisableControllerNavigation();
  }

  private void AnimEnable()
  {
    if (!this._isDisabled)
      return;
    this._animEnableTween = ((Node) this).CreateTween().SetParallel(true);
    this._animEnableTween.TweenProperty((GodotObject) this, NodePath.op_Implicit("position"), Variant.op_Implicit(NPlayerHand._showPosition), 0.2).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 7L);
    this._animEnableTween.TweenProperty((GodotObject) this, NodePath.op_Implicit("modulate"), Variant.op_Implicit(Colors.White), 0.2).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 7L);
    this._isDisabled = false;
    this.EnableControllerNavigation();
    this.DefaultFocusedControl.TryGrabFocus();
  }

  public void FlashPlayableHolders()
  {
    foreach (NHandCardHolder holder in (IEnumerable<NHandCardHolder>) this.Holders)
    {
      if (holder.CardNode != null && holder.CardNode.Model.CanPlay())
        holder.Flash();
    }
  }

  private void OnCardSelected(Node _)
  {
    this.UpdateSelectedCardContainer(((Node) this._selectedHandCardContainer).GetChildCount(false));
  }

  private void OnCardDeselected(Node _)
  {
    this.UpdateSelectedCardContainer(((Node) this._selectedHandCardContainer).GetChildCount(false) - 1);
  }

  private void UpdateSelectedCardContainer(int count)
  {
    float num1 = 1f;
    float num2 = this.Size.Y * 0.5f;
    if (count > 6)
    {
      num1 = 0.55f;
      num2 -= 150f;
    }
    else if (count > 3)
    {
      num1 = 0.8f;
      num2 -= 75f;
    }
    this._selectedCardScaleTween?.Kill();
    this._selectedCardScaleTween = ((Node) this).CreateTween().SetParallel(true);
    this._selectedCardScaleTween.TweenProperty((GodotObject) this._selectedHandCardContainer, NodePath.op_Implicit("position:y"), Variant.op_Implicit(num2), 0.5).SetTrans((Tween.TransitionType) 4L).SetEase((Tween.EaseType) 2L);
    this._selectedCardScaleTween.TweenProperty((GodotObject) this._selectedHandCardContainer, NodePath.op_Implicit("scale"), Variant.op_Implicit(Vector2.op_Multiply(Vector2.One, num1)), 0.5).SetTrans((Tween.TransitionType) 4L).SetEase((Tween.EaseType) 2L);
  }

  public Control DefaultFocusedControl
  {
    get
    {
      if (this.ActiveHolders.Count > 0)
        return this._lastFocusedHolderIdx >= 0 ? (Control) this.ActiveHolders[Mathf.Clamp(this._lastFocusedHolderIdx, 0, this.ActiveHolders.Count - 1)] : (Control) this.ActiveHolders[this.ActiveHolders.Count / 2];
      if (this.CurrentMode == NPlayerHand.Mode.SimpleSelect && this._selectedCards.Count > 0 && !this.PeekButton.IsPeeking)
        return (Control) this.GetCardHolder(this._selectedCards.First<CardModel>());
      return this.CurrentMode == NPlayerHand.Mode.UpgradeSelect && !this.PeekButton.IsPeeking ? this._upgradePreview.DefaultFocusedControl : this.CardHolderContainer;
    }
  }

  public void EnableControllerNavigation()
  {
    foreach (Control holder in (IEnumerable<NHandCardHolder>) this.Holders)
      holder.FocusMode = (Control.FocusModeEnum) 2L;
    if (!this.InCardPlay)
      return;
    this._currentCardPlay.Holder.FocusMode = (Control.FocusModeEnum) 2L;
  }

  public void DisableControllerNavigation()
  {
    foreach (Control holder in (IEnumerable<NHandCardHolder>) this.Holders)
      holder.FocusMode = (Control.FocusModeEnum) 0L;
    if (!this.InCardPlay)
      return;
    this._currentCardPlay.Holder.FocusMode = (Control.FocusModeEnum) 0L;
  }

  public override void _UnhandledInput(InputEvent input)
  {
    if (NControllerManager.Instance.IsUsingController || !ActiveScreenContext.Instance.IsCurrent((IScreenContext) NCombatRoom.Instance) || CombatManager.Instance.IsOverOrEnding)
      return;
    List<NHandCardHolder> nhandCardHolderList = new List<NHandCardHolder>();
    nhandCardHolderList.AddRange((IEnumerable<NHandCardHolder>) this.ActiveHolders);
    if (this.HasDraggedHolder)
      nhandCardHolderList.Insert(this._draggedHolderIndex, (NHandCardHolder) null);
    for (int index = 0; index < this._selectCardShortcuts.Length; ++index)
    {
      StringName selectCardShortcut = this._selectCardShortcuts[index];
      if (input.IsActionPressed(selectCardShortcut, false, false) && nhandCardHolderList.Count > index)
      {
        NHandCardHolder holder = nhandCardHolderList[index];
        if (holder != null)
        {
          if (NTargetManager.Instance.IsInSelection)
            NTargetManager.Instance.CancelTargeting();
          switch (this.CurrentMode)
          {
            case NPlayerHand.Mode.Play:
              if (this.AreCardActionsAllowed())
              {
                if (this.InCardPlay)
                  this._currentCardPlay.CancelPlayCard();
                this.StartCardPlay(holder, true);
                break;
              }
              break;
            case NPlayerHand.Mode.SimpleSelect:
              if (!this.PeekButton.IsPeeking)
              {
                this.SelectCardInSimpleMode(holder);
                break;
              }
              break;
            case NPlayerHand.Mode.UpgradeSelect:
              if (!this.PeekButton.IsPeeking)
              {
                this.SelectCardInUpgradeMode(holder);
                break;
              }
              break;
          }
          ((Node) this).GetViewport()?.SetInputAsHandled();
        }
      }
    }
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(38)
    {
      new MethodInfo(NPlayerHand.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NPlayerHand.MethodName._EnterTree, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NPlayerHand.MethodName._ExitTree, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NPlayerHand.MethodName.IsAwaitingPlay, new PropertyInfo((Variant.Type) 1L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("holder"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false)
      }, (List<Variant>) null),
      new MethodInfo(NPlayerHand.MethodName.Add, new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("card"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false),
        new PropertyInfo((Variant.Type) 2L, StringName.op_Implicit("index"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NPlayerHand.MethodName.AddCardHolder, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("holder"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false),
        new PropertyInfo((Variant.Type) 2L, StringName.op_Implicit("index"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NPlayerHand.MethodName.RemoveCardHolder, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("holder"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false)
      }, (List<Variant>) null),
      new MethodInfo(NPlayerHand.MethodName.OnHolderFocused, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("holder"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false)
      }, (List<Variant>) null),
      new MethodInfo(NPlayerHand.MethodName.OnHolderUnfocused, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("holder"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false)
      }, (List<Variant>) null),
      new MethodInfo(NPlayerHand.MethodName.CancelAllCardPlay, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NPlayerHand.MethodName.ReturnHolderToHand, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("holder"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false)
      }, (List<Variant>) null),
      new MethodInfo(NPlayerHand.MethodName.ForceRefreshCardIndices, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NPlayerHand.MethodName.RefreshLayout, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NPlayerHand.MethodName.OnPeekButtonToggled, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("button"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false)
      }, (List<Variant>) null),
      new MethodInfo(NPlayerHand.MethodName.UpdateSelectModeCardVisibility, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NPlayerHand.MethodName.CancelHandSelectionIfNecessary, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NPlayerHand.MethodName.OnHolderPressed, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("holder"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false)
      }, (List<Variant>) null),
      new MethodInfo(NPlayerHand.MethodName.CanPlayCards, new PropertyInfo((Variant.Type) 1L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NPlayerHand.MethodName.AreCardActionsAllowed, new PropertyInfo((Variant.Type) 1L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NPlayerHand.MethodName.StartCardPlay, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("holder"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false),
        new PropertyInfo((Variant.Type) 1L, StringName.op_Implicit("startedViaShortcut"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NPlayerHand.MethodName.SelectCardInSimpleMode, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("holder"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false)
      }, (List<Variant>) null),
      new MethodInfo(NPlayerHand.MethodName.SelectCardInUpgradeMode, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("holder"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false)
      }, (List<Variant>) null),
      new MethodInfo(NPlayerHand.MethodName.DeselectCard, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("card"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false)
      }, (List<Variant>) null),
      new MethodInfo(NPlayerHand.MethodName.RevalidateSelectionAfterStateChange, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NPlayerHand.MethodName.OnSelectModeConfirmButtonPressed, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("_"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false)
      }, (List<Variant>) null),
      new MethodInfo(NPlayerHand.MethodName.CheckIfSelectionComplete, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NPlayerHand.MethodName.RefreshSelectModeConfirmButton, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NPlayerHand.MethodName.AnimIn, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NPlayerHand.MethodName.AnimOut, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NPlayerHand.MethodName.AnimDisable, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NPlayerHand.MethodName.AnimEnable, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NPlayerHand.MethodName.FlashPlayableHolders, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NPlayerHand.MethodName.OnCardSelected, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("_"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Node"), false)
      }, (List<Variant>) null),
      new MethodInfo(NPlayerHand.MethodName.OnCardDeselected, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("_"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Node"), false)
      }, (List<Variant>) null),
      new MethodInfo(NPlayerHand.MethodName.UpdateSelectedCardContainer, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 2L, StringName.op_Implicit("count"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NPlayerHand.MethodName.EnableControllerNavigation, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NPlayerHand.MethodName.DisableControllerNavigation, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NPlayerHand.MethodName._UnhandledInput, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("input"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("InputEvent"), false)
      }, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NPlayerHand.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NPlayerHand.MethodName._EnterTree) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._EnterTree();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NPlayerHand.MethodName._ExitTree) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._ExitTree();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NPlayerHand.MethodName.IsAwaitingPlay) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      bool flag = this.IsAwaitingPlay(VariantUtils.ConvertTo<NHandCardHolder>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = VariantUtils.CreateFrom<bool>(ref flag);
      return true;
    }
    if (StringName.op_Equality(ref method, NPlayerHand.MethodName.Add) && ((NativeVariantPtrArgs) ref args).Count == 2)
    {
      NHandCardHolder nhandCardHolder = this.Add(VariantUtils.ConvertTo<NCard>(ref ((NativeVariantPtrArgs) ref args)[0]), VariantUtils.ConvertTo<int>(ref ((NativeVariantPtrArgs) ref args)[1]));
      ret = VariantUtils.CreateFrom<NHandCardHolder>(ref nhandCardHolder);
      return true;
    }
    if (StringName.op_Equality(ref method, NPlayerHand.MethodName.AddCardHolder) && ((NativeVariantPtrArgs) ref args).Count == 2)
    {
      this.AddCardHolder(VariantUtils.ConvertTo<NHandCardHolder>(ref ((NativeVariantPtrArgs) ref args)[0]), VariantUtils.ConvertTo<int>(ref ((NativeVariantPtrArgs) ref args)[1]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NPlayerHand.MethodName.RemoveCardHolder) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.RemoveCardHolder(VariantUtils.ConvertTo<NCardHolder>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NPlayerHand.MethodName.OnHolderFocused) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.OnHolderFocused(VariantUtils.ConvertTo<NHandCardHolder>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NPlayerHand.MethodName.OnHolderUnfocused) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.OnHolderUnfocused(VariantUtils.ConvertTo<NHandCardHolder>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NPlayerHand.MethodName.CancelAllCardPlay) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.CancelAllCardPlay();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NPlayerHand.MethodName.ReturnHolderToHand) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.ReturnHolderToHand(VariantUtils.ConvertTo<NHandCardHolder>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NPlayerHand.MethodName.ForceRefreshCardIndices) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.ForceRefreshCardIndices();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NPlayerHand.MethodName.RefreshLayout) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.RefreshLayout();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NPlayerHand.MethodName.OnPeekButtonToggled) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.OnPeekButtonToggled(VariantUtils.ConvertTo<NPeekButton>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NPlayerHand.MethodName.UpdateSelectModeCardVisibility) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.UpdateSelectModeCardVisibility();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NPlayerHand.MethodName.CancelHandSelectionIfNecessary) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.CancelHandSelectionIfNecessary();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NPlayerHand.MethodName.OnHolderPressed) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.OnHolderPressed(VariantUtils.ConvertTo<NCardHolder>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NPlayerHand.MethodName.CanPlayCards) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      bool flag = this.CanPlayCards();
      ret = VariantUtils.CreateFrom<bool>(ref flag);
      return true;
    }
    if (StringName.op_Equality(ref method, NPlayerHand.MethodName.AreCardActionsAllowed) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      bool flag = this.AreCardActionsAllowed();
      ret = VariantUtils.CreateFrom<bool>(ref flag);
      return true;
    }
    if (StringName.op_Equality(ref method, NPlayerHand.MethodName.StartCardPlay) && ((NativeVariantPtrArgs) ref args).Count == 2)
    {
      this.StartCardPlay(VariantUtils.ConvertTo<NHandCardHolder>(ref ((NativeVariantPtrArgs) ref args)[0]), VariantUtils.ConvertTo<bool>(ref ((NativeVariantPtrArgs) ref args)[1]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NPlayerHand.MethodName.SelectCardInSimpleMode) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.SelectCardInSimpleMode(VariantUtils.ConvertTo<NHandCardHolder>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NPlayerHand.MethodName.SelectCardInUpgradeMode) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.SelectCardInUpgradeMode(VariantUtils.ConvertTo<NHandCardHolder>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NPlayerHand.MethodName.DeselectCard) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.DeselectCard(VariantUtils.ConvertTo<NCard>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NPlayerHand.MethodName.RevalidateSelectionAfterStateChange) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.RevalidateSelectionAfterStateChange();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NPlayerHand.MethodName.OnSelectModeConfirmButtonPressed) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.OnSelectModeConfirmButtonPressed(VariantUtils.ConvertTo<NButton>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NPlayerHand.MethodName.CheckIfSelectionComplete) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.CheckIfSelectionComplete();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NPlayerHand.MethodName.RefreshSelectModeConfirmButton) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.RefreshSelectModeConfirmButton();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NPlayerHand.MethodName.AnimIn) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.AnimIn();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NPlayerHand.MethodName.AnimOut) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.AnimOut();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NPlayerHand.MethodName.AnimDisable) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.AnimDisable();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NPlayerHand.MethodName.AnimEnable) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.AnimEnable();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NPlayerHand.MethodName.FlashPlayableHolders) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.FlashPlayableHolders();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NPlayerHand.MethodName.OnCardSelected) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.OnCardSelected(VariantUtils.ConvertTo<Node>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NPlayerHand.MethodName.OnCardDeselected) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.OnCardDeselected(VariantUtils.ConvertTo<Node>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NPlayerHand.MethodName.UpdateSelectedCardContainer) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.UpdateSelectedCardContainer(VariantUtils.ConvertTo<int>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NPlayerHand.MethodName.EnableControllerNavigation) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.EnableControllerNavigation();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NPlayerHand.MethodName.DisableControllerNavigation) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.DisableControllerNavigation();
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NPlayerHand.MethodName._UnhandledInput) || ((NativeVariantPtrArgs) ref args).Count != 1)
      return base.InvokeGodotClassMethod(ref method, args, ref ret);
    ((Node) this)._UnhandledInput(VariantUtils.ConvertTo<InputEvent>(ref ((NativeVariantPtrArgs) ref args)[0]));
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NPlayerHand.MethodName._Ready) || StringName.op_Equality(ref method, NPlayerHand.MethodName._EnterTree) || StringName.op_Equality(ref method, NPlayerHand.MethodName._ExitTree) || StringName.op_Equality(ref method, NPlayerHand.MethodName.IsAwaitingPlay) || StringName.op_Equality(ref method, NPlayerHand.MethodName.Add) || StringName.op_Equality(ref method, NPlayerHand.MethodName.AddCardHolder) || StringName.op_Equality(ref method, NPlayerHand.MethodName.RemoveCardHolder) || StringName.op_Equality(ref method, NPlayerHand.MethodName.OnHolderFocused) || StringName.op_Equality(ref method, NPlayerHand.MethodName.OnHolderUnfocused) || StringName.op_Equality(ref method, NPlayerHand.MethodName.CancelAllCardPlay) || StringName.op_Equality(ref method, NPlayerHand.MethodName.ReturnHolderToHand) || StringName.op_Equality(ref method, NPlayerHand.MethodName.ForceRefreshCardIndices) || StringName.op_Equality(ref method, NPlayerHand.MethodName.RefreshLayout) || StringName.op_Equality(ref method, NPlayerHand.MethodName.OnPeekButtonToggled) || StringName.op_Equality(ref method, NPlayerHand.MethodName.UpdateSelectModeCardVisibility) || StringName.op_Equality(ref method, NPlayerHand.MethodName.CancelHandSelectionIfNecessary) || StringName.op_Equality(ref method, NPlayerHand.MethodName.OnHolderPressed) || StringName.op_Equality(ref method, NPlayerHand.MethodName.CanPlayCards) || StringName.op_Equality(ref method, NPlayerHand.MethodName.AreCardActionsAllowed) || StringName.op_Equality(ref method, NPlayerHand.MethodName.StartCardPlay) || StringName.op_Equality(ref method, NPlayerHand.MethodName.SelectCardInSimpleMode) || StringName.op_Equality(ref method, NPlayerHand.MethodName.SelectCardInUpgradeMode) || StringName.op_Equality(ref method, NPlayerHand.MethodName.DeselectCard) || StringName.op_Equality(ref method, NPlayerHand.MethodName.RevalidateSelectionAfterStateChange) || StringName.op_Equality(ref method, NPlayerHand.MethodName.OnSelectModeConfirmButtonPressed) || StringName.op_Equality(ref method, NPlayerHand.MethodName.CheckIfSelectionComplete) || StringName.op_Equality(ref method, NPlayerHand.MethodName.RefreshSelectModeConfirmButton) || StringName.op_Equality(ref method, NPlayerHand.MethodName.AnimIn) || StringName.op_Equality(ref method, NPlayerHand.MethodName.AnimOut) || StringName.op_Equality(ref method, NPlayerHand.MethodName.AnimDisable) || StringName.op_Equality(ref method, NPlayerHand.MethodName.AnimEnable) || StringName.op_Equality(ref method, NPlayerHand.MethodName.FlashPlayableHolders) || StringName.op_Equality(ref method, NPlayerHand.MethodName.OnCardSelected) || StringName.op_Equality(ref method, NPlayerHand.MethodName.OnCardDeselected) || StringName.op_Equality(ref method, NPlayerHand.MethodName.UpdateSelectedCardContainer) || StringName.op_Equality(ref method, NPlayerHand.MethodName.EnableControllerNavigation) || StringName.op_Equality(ref method, NPlayerHand.MethodName.DisableControllerNavigation) || StringName.op_Equality(ref method, NPlayerHand.MethodName._UnhandledInput) || base.HasGodotClassMethod(ref method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NPlayerHand.PropertyName.CardHolderContainer))
    {
      this.CardHolderContainer = VariantUtils.ConvertTo<Control>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NPlayerHand.PropertyName.PeekButton))
    {
      this.PeekButton = VariantUtils.ConvertTo<NPeekButton>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NPlayerHand.PropertyName.CurrentMode))
    {
      this.CurrentMode = VariantUtils.ConvertTo<NPlayerHand.Mode>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NPlayerHand.PropertyName.FocusedHolder))
    {
      this.FocusedHolder = VariantUtils.ConvertTo<NHandCardHolder>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NPlayerHand.PropertyName._selectCardShortcuts))
    {
      this._selectCardShortcuts = VariantUtils.ConvertTo<StringName[]>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NPlayerHand.PropertyName._selectModeBackstop))
    {
      this._selectModeBackstop = VariantUtils.ConvertTo<Control>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NPlayerHand.PropertyName._upgradePreviewContainer))
    {
      this._upgradePreviewContainer = VariantUtils.ConvertTo<Control>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NPlayerHand.PropertyName._selectedHandCardContainer))
    {
      this._selectedHandCardContainer = VariantUtils.ConvertTo<NSelectedHandCardContainer>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NPlayerHand.PropertyName._upgradePreview))
    {
      this._upgradePreview = VariantUtils.ConvertTo<NUpgradePreview>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NPlayerHand.PropertyName._selectModeConfirmButton))
    {
      this._selectModeConfirmButton = VariantUtils.ConvertTo<NConfirmButton>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NPlayerHand.PropertyName._selectionHeader))
    {
      this._selectionHeader = VariantUtils.ConvertTo<MegaRichTextLabel>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NPlayerHand.PropertyName._currentCardPlay))
    {
      this._currentCardPlay = VariantUtils.ConvertTo<NCardPlay>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NPlayerHand.PropertyName._currentMode))
    {
      this._currentMode = VariantUtils.ConvertTo<NPlayerHand.Mode>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NPlayerHand.PropertyName._draggedHolderIndex))
    {
      this._draggedHolderIndex = VariantUtils.ConvertTo<int>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NPlayerHand.PropertyName._lastFocusedHolderIdx))
    {
      this._lastFocusedHolderIdx = VariantUtils.ConvertTo<int>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NPlayerHand.PropertyName._animEnableTween))
    {
      this._animEnableTween = VariantUtils.ConvertTo<Tween>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NPlayerHand.PropertyName._isDisabled))
    {
      this._isDisabled = VariantUtils.ConvertTo<bool>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NPlayerHand.PropertyName._animInTween))
    {
      this._animInTween = VariantUtils.ConvertTo<Tween>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NPlayerHand.PropertyName._animOutTween))
    {
      this._animOutTween = VariantUtils.ConvertTo<Tween>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NPlayerHand.PropertyName._selectedCardScaleTween))
      return ((GodotObject) this).SetGodotClassPropertyValue(ref name, ref value);
    this._selectedCardScaleTween = VariantUtils.ConvertTo<Tween>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NPlayerHand.PropertyName.CardHolderContainer))
    {
      ref godot_variant local = ref value;
      Control cardHolderContainer = this.CardHolderContainer;
      godot_variant from = VariantUtils.CreateFrom<Control>(ref cardHolderContainer);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NPlayerHand.PropertyName.PeekButton))
    {
      ref godot_variant local = ref value;
      NPeekButton peekButton = this.PeekButton;
      godot_variant from = VariantUtils.CreateFrom<NPeekButton>(ref peekButton);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NPlayerHand.PropertyName.InCardPlay))
    {
      ref godot_variant local = ref value;
      bool inCardPlay = this.InCardPlay;
      godot_variant from = VariantUtils.CreateFrom<bool>(ref inCardPlay);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NPlayerHand.PropertyName.IsInCardSelection))
    {
      ref godot_variant local = ref value;
      bool isInCardSelection = this.IsInCardSelection;
      godot_variant from = VariantUtils.CreateFrom<bool>(ref isInCardSelection);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NPlayerHand.PropertyName.CurrentMode))
    {
      ref godot_variant local = ref value;
      NPlayerHand.Mode currentMode = this.CurrentMode;
      godot_variant from = VariantUtils.CreateFrom<NPlayerHand.Mode>(ref currentMode);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NPlayerHand.PropertyName.HasDraggedHolder))
    {
      ref godot_variant local = ref value;
      bool hasDraggedHolder = this.HasDraggedHolder;
      godot_variant from = VariantUtils.CreateFrom<bool>(ref hasDraggedHolder);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NPlayerHand.PropertyName.FocusedHolder))
    {
      ref godot_variant local = ref value;
      NHandCardHolder focusedHolder = this.FocusedHolder;
      godot_variant from = VariantUtils.CreateFrom<NHandCardHolder>(ref focusedHolder);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NPlayerHand.PropertyName.DefaultFocusedControl))
    {
      ref godot_variant local = ref value;
      Control defaultFocusedControl = this.DefaultFocusedControl;
      godot_variant from = VariantUtils.CreateFrom<Control>(ref defaultFocusedControl);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NPlayerHand.PropertyName._selectCardShortcuts))
    {
      value = VariantUtils.CreateFrom<StringName[]>(ref this._selectCardShortcuts);
      return true;
    }
    if (StringName.op_Equality(ref name, NPlayerHand.PropertyName._selectModeBackstop))
    {
      value = VariantUtils.CreateFrom<Control>(ref this._selectModeBackstop);
      return true;
    }
    if (StringName.op_Equality(ref name, NPlayerHand.PropertyName._upgradePreviewContainer))
    {
      value = VariantUtils.CreateFrom<Control>(ref this._upgradePreviewContainer);
      return true;
    }
    if (StringName.op_Equality(ref name, NPlayerHand.PropertyName._selectedHandCardContainer))
    {
      value = VariantUtils.CreateFrom<NSelectedHandCardContainer>(ref this._selectedHandCardContainer);
      return true;
    }
    if (StringName.op_Equality(ref name, NPlayerHand.PropertyName._upgradePreview))
    {
      value = VariantUtils.CreateFrom<NUpgradePreview>(ref this._upgradePreview);
      return true;
    }
    if (StringName.op_Equality(ref name, NPlayerHand.PropertyName._selectModeConfirmButton))
    {
      value = VariantUtils.CreateFrom<NConfirmButton>(ref this._selectModeConfirmButton);
      return true;
    }
    if (StringName.op_Equality(ref name, NPlayerHand.PropertyName._selectionHeader))
    {
      value = VariantUtils.CreateFrom<MegaRichTextLabel>(ref this._selectionHeader);
      return true;
    }
    if (StringName.op_Equality(ref name, NPlayerHand.PropertyName._currentCardPlay))
    {
      value = VariantUtils.CreateFrom<NCardPlay>(ref this._currentCardPlay);
      return true;
    }
    if (StringName.op_Equality(ref name, NPlayerHand.PropertyName._currentMode))
    {
      value = VariantUtils.CreateFrom<NPlayerHand.Mode>(ref this._currentMode);
      return true;
    }
    if (StringName.op_Equality(ref name, NPlayerHand.PropertyName._draggedHolderIndex))
    {
      value = VariantUtils.CreateFrom<int>(ref this._draggedHolderIndex);
      return true;
    }
    if (StringName.op_Equality(ref name, NPlayerHand.PropertyName._lastFocusedHolderIdx))
    {
      value = VariantUtils.CreateFrom<int>(ref this._lastFocusedHolderIdx);
      return true;
    }
    if (StringName.op_Equality(ref name, NPlayerHand.PropertyName._animEnableTween))
    {
      value = VariantUtils.CreateFrom<Tween>(ref this._animEnableTween);
      return true;
    }
    if (StringName.op_Equality(ref name, NPlayerHand.PropertyName._isDisabled))
    {
      value = VariantUtils.CreateFrom<bool>(ref this._isDisabled);
      return true;
    }
    if (StringName.op_Equality(ref name, NPlayerHand.PropertyName._animInTween))
    {
      value = VariantUtils.CreateFrom<Tween>(ref this._animInTween);
      return true;
    }
    if (StringName.op_Equality(ref name, NPlayerHand.PropertyName._animOutTween))
    {
      value = VariantUtils.CreateFrom<Tween>(ref this._animOutTween);
      return true;
    }
    if (!StringName.op_Equality(ref name, NPlayerHand.PropertyName._selectedCardScaleTween))
      return ((GodotObject) this).GetGodotClassPropertyValue(ref name, ref value);
    value = VariantUtils.CreateFrom<Tween>(ref this._selectedCardScaleTween);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 28L, NPlayerHand.PropertyName._selectCardShortcuts, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NPlayerHand.PropertyName.CardHolderContainer, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NPlayerHand.PropertyName.PeekButton, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NPlayerHand.PropertyName._selectModeBackstop, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NPlayerHand.PropertyName._upgradePreviewContainer, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NPlayerHand.PropertyName._selectedHandCardContainer, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NPlayerHand.PropertyName._upgradePreview, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NPlayerHand.PropertyName._selectModeConfirmButton, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NPlayerHand.PropertyName._selectionHeader, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NPlayerHand.PropertyName._currentCardPlay, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 1L, NPlayerHand.PropertyName.InCardPlay, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 1L, NPlayerHand.PropertyName.IsInCardSelection, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 2L, NPlayerHand.PropertyName._currentMode, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 2L, NPlayerHand.PropertyName.CurrentMode, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 2L, NPlayerHand.PropertyName._draggedHolderIndex, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 1L, NPlayerHand.PropertyName.HasDraggedHolder, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 2L, NPlayerHand.PropertyName._lastFocusedHolderIdx, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NPlayerHand.PropertyName._animEnableTween, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 1L, NPlayerHand.PropertyName._isDisabled, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NPlayerHand.PropertyName._animInTween, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NPlayerHand.PropertyName._animOutTween, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NPlayerHand.PropertyName._selectedCardScaleTween, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NPlayerHand.PropertyName.FocusedHolder, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NPlayerHand.PropertyName.DefaultFocusedControl, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).SaveGodotObjectData(info);
    GodotSerializationInfo serializationInfo1 = info;
    StringName cardHolderContainer1 = NPlayerHand.PropertyName.CardHolderContainer;
    Control cardHolderContainer2 = this.CardHolderContainer;
    Variant variant1 = Variant.From<Control>(ref cardHolderContainer2);
    serializationInfo1.AddProperty(cardHolderContainer1, variant1);
    GodotSerializationInfo serializationInfo2 = info;
    StringName peekButton1 = NPlayerHand.PropertyName.PeekButton;
    NPeekButton peekButton2 = this.PeekButton;
    Variant variant2 = Variant.From<NPeekButton>(ref peekButton2);
    serializationInfo2.AddProperty(peekButton1, variant2);
    GodotSerializationInfo serializationInfo3 = info;
    StringName currentMode1 = NPlayerHand.PropertyName.CurrentMode;
    NPlayerHand.Mode currentMode2 = this.CurrentMode;
    Variant variant3 = Variant.From<NPlayerHand.Mode>(ref currentMode2);
    serializationInfo3.AddProperty(currentMode1, variant3);
    GodotSerializationInfo serializationInfo4 = info;
    StringName focusedHolder1 = NPlayerHand.PropertyName.FocusedHolder;
    NHandCardHolder focusedHolder2 = this.FocusedHolder;
    Variant variant4 = Variant.From<NHandCardHolder>(ref focusedHolder2);
    serializationInfo4.AddProperty(focusedHolder1, variant4);
    info.AddProperty(NPlayerHand.PropertyName._selectCardShortcuts, Variant.From<StringName[]>(ref this._selectCardShortcuts));
    info.AddProperty(NPlayerHand.PropertyName._selectModeBackstop, Variant.From<Control>(ref this._selectModeBackstop));
    info.AddProperty(NPlayerHand.PropertyName._upgradePreviewContainer, Variant.From<Control>(ref this._upgradePreviewContainer));
    info.AddProperty(NPlayerHand.PropertyName._selectedHandCardContainer, Variant.From<NSelectedHandCardContainer>(ref this._selectedHandCardContainer));
    info.AddProperty(NPlayerHand.PropertyName._upgradePreview, Variant.From<NUpgradePreview>(ref this._upgradePreview));
    info.AddProperty(NPlayerHand.PropertyName._selectModeConfirmButton, Variant.From<NConfirmButton>(ref this._selectModeConfirmButton));
    info.AddProperty(NPlayerHand.PropertyName._selectionHeader, Variant.From<MegaRichTextLabel>(ref this._selectionHeader));
    info.AddProperty(NPlayerHand.PropertyName._currentCardPlay, Variant.From<NCardPlay>(ref this._currentCardPlay));
    info.AddProperty(NPlayerHand.PropertyName._currentMode, Variant.From<NPlayerHand.Mode>(ref this._currentMode));
    info.AddProperty(NPlayerHand.PropertyName._draggedHolderIndex, Variant.From<int>(ref this._draggedHolderIndex));
    info.AddProperty(NPlayerHand.PropertyName._lastFocusedHolderIdx, Variant.From<int>(ref this._lastFocusedHolderIdx));
    info.AddProperty(NPlayerHand.PropertyName._animEnableTween, Variant.From<Tween>(ref this._animEnableTween));
    info.AddProperty(NPlayerHand.PropertyName._isDisabled, Variant.From<bool>(ref this._isDisabled));
    info.AddProperty(NPlayerHand.PropertyName._animInTween, Variant.From<Tween>(ref this._animInTween));
    info.AddProperty(NPlayerHand.PropertyName._animOutTween, Variant.From<Tween>(ref this._animOutTween));
    info.AddProperty(NPlayerHand.PropertyName._selectedCardScaleTween, Variant.From<Tween>(ref this._selectedCardScaleTween));
    info.AddSignalEventDelegate(NPlayerHand.SignalName.ModeChanged, (Delegate) this.backing_ModeChanged);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NPlayerHand.PropertyName.CardHolderContainer, ref variant1))
      this.CardHolderContainer = ((Variant) ref variant1).As<Control>();
    Variant variant2;
    if (info.TryGetProperty(NPlayerHand.PropertyName.PeekButton, ref variant2))
      this.PeekButton = ((Variant) ref variant2).As<NPeekButton>();
    Variant variant3;
    if (info.TryGetProperty(NPlayerHand.PropertyName.CurrentMode, ref variant3))
      this.CurrentMode = ((Variant) ref variant3).As<NPlayerHand.Mode>();
    Variant variant4;
    if (info.TryGetProperty(NPlayerHand.PropertyName.FocusedHolder, ref variant4))
      this.FocusedHolder = ((Variant) ref variant4).As<NHandCardHolder>();
    Variant variant5;
    if (info.TryGetProperty(NPlayerHand.PropertyName._selectCardShortcuts, ref variant5))
      this._selectCardShortcuts = ((Variant) ref variant5).As<StringName[]>();
    Variant variant6;
    if (info.TryGetProperty(NPlayerHand.PropertyName._selectModeBackstop, ref variant6))
      this._selectModeBackstop = ((Variant) ref variant6).As<Control>();
    Variant variant7;
    if (info.TryGetProperty(NPlayerHand.PropertyName._upgradePreviewContainer, ref variant7))
      this._upgradePreviewContainer = ((Variant) ref variant7).As<Control>();
    Variant variant8;
    if (info.TryGetProperty(NPlayerHand.PropertyName._selectedHandCardContainer, ref variant8))
      this._selectedHandCardContainer = ((Variant) ref variant8).As<NSelectedHandCardContainer>();
    Variant variant9;
    if (info.TryGetProperty(NPlayerHand.PropertyName._upgradePreview, ref variant9))
      this._upgradePreview = ((Variant) ref variant9).As<NUpgradePreview>();
    Variant variant10;
    if (info.TryGetProperty(NPlayerHand.PropertyName._selectModeConfirmButton, ref variant10))
      this._selectModeConfirmButton = ((Variant) ref variant10).As<NConfirmButton>();
    Variant variant11;
    if (info.TryGetProperty(NPlayerHand.PropertyName._selectionHeader, ref variant11))
      this._selectionHeader = ((Variant) ref variant11).As<MegaRichTextLabel>();
    Variant variant12;
    if (info.TryGetProperty(NPlayerHand.PropertyName._currentCardPlay, ref variant12))
      this._currentCardPlay = ((Variant) ref variant12).As<NCardPlay>();
    Variant variant13;
    if (info.TryGetProperty(NPlayerHand.PropertyName._currentMode, ref variant13))
      this._currentMode = ((Variant) ref variant13).As<NPlayerHand.Mode>();
    Variant variant14;
    if (info.TryGetProperty(NPlayerHand.PropertyName._draggedHolderIndex, ref variant14))
      this._draggedHolderIndex = ((Variant) ref variant14).As<int>();
    Variant variant15;
    if (info.TryGetProperty(NPlayerHand.PropertyName._lastFocusedHolderIdx, ref variant15))
      this._lastFocusedHolderIdx = ((Variant) ref variant15).As<int>();
    Variant variant16;
    if (info.TryGetProperty(NPlayerHand.PropertyName._animEnableTween, ref variant16))
      this._animEnableTween = ((Variant) ref variant16).As<Tween>();
    Variant variant17;
    if (info.TryGetProperty(NPlayerHand.PropertyName._isDisabled, ref variant17))
      this._isDisabled = ((Variant) ref variant17).As<bool>();
    Variant variant18;
    if (info.TryGetProperty(NPlayerHand.PropertyName._animInTween, ref variant18))
      this._animInTween = ((Variant) ref variant18).As<Tween>();
    Variant variant19;
    if (info.TryGetProperty(NPlayerHand.PropertyName._animOutTween, ref variant19))
      this._animOutTween = ((Variant) ref variant19).As<Tween>();
    Variant variant20;
    if (info.TryGetProperty(NPlayerHand.PropertyName._selectedCardScaleTween, ref variant20))
      this._selectedCardScaleTween = ((Variant) ref variant20).As<Tween>();
    NPlayerHand.ModeChangedEventHandler changedEventHandler;
    if (!info.TryGetSignalEventDelegate<NPlayerHand.ModeChangedEventHandler>(NPlayerHand.SignalName.ModeChanged, ref changedEventHandler))
      return;
    this.backing_ModeChanged = changedEventHandler;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<MethodInfo> GetGodotSignalList()
  {
    return new List<MethodInfo>(1)
    {
      new MethodInfo(NPlayerHand.SignalName.ModeChanged, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  public event NPlayerHand.ModeChangedEventHandler ModeChanged
  {
    add => this.backing_ModeChanged += value;
    remove => this.backing_ModeChanged -= value;
  }

  protected void EmitSignalModeChanged()
  {
    ((GodotObject) this).EmitSignal(NPlayerHand.SignalName.ModeChanged, Array.Empty<Variant>());
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RaiseGodotClassSignalCallbacks(
    in godot_string_name signal,
    NativeVariantPtrArgs args)
  {
    if (StringName.op_Equality(ref signal, NPlayerHand.SignalName.ModeChanged) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      NPlayerHand.ModeChangedEventHandler backingModeChanged = this.backing_ModeChanged;
      if (backingModeChanged == null)
        return;
      backingModeChanged();
    }
    else
      ((GodotObject) this).RaiseGodotClassSignalCallbacks(ref signal, args);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassSignal(in godot_string_name signal)
  {
    return StringName.op_Equality(ref signal, NPlayerHand.SignalName.ModeChanged) || base.HasGodotClassSignal(ref signal);
  }

  [Signal]
  public delegate void ModeChangedEventHandler();

  public enum Mode
  {
    None,
    Play,
    SimpleSelect,
    UpgradeSelect,
  }

  public class MethodName : Control.MethodName
  {
    public static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName _EnterTree = StringName.op_Implicit(nameof (_EnterTree));
    public static readonly StringName _ExitTree = StringName.op_Implicit(nameof (_ExitTree));
    public static readonly StringName IsAwaitingPlay = StringName.op_Implicit(nameof (IsAwaitingPlay));
    public static readonly StringName Add = StringName.op_Implicit(nameof (Add));
    public static readonly StringName AddCardHolder = StringName.op_Implicit(nameof (AddCardHolder));
    public static readonly StringName RemoveCardHolder = StringName.op_Implicit(nameof (RemoveCardHolder));
    public static readonly StringName OnHolderFocused = StringName.op_Implicit(nameof (OnHolderFocused));
    public static readonly StringName OnHolderUnfocused = StringName.op_Implicit(nameof (OnHolderUnfocused));
    public static readonly StringName CancelAllCardPlay = StringName.op_Implicit(nameof (CancelAllCardPlay));
    public static readonly StringName ReturnHolderToHand = StringName.op_Implicit(nameof (ReturnHolderToHand));
    public static readonly StringName ForceRefreshCardIndices = StringName.op_Implicit(nameof (ForceRefreshCardIndices));
    public static readonly StringName RefreshLayout = StringName.op_Implicit(nameof (RefreshLayout));
    public static readonly StringName OnPeekButtonToggled = StringName.op_Implicit(nameof (OnPeekButtonToggled));
    public static readonly StringName UpdateSelectModeCardVisibility = StringName.op_Implicit(nameof (UpdateSelectModeCardVisibility));
    public static readonly StringName CancelHandSelectionIfNecessary = StringName.op_Implicit(nameof (CancelHandSelectionIfNecessary));
    public static readonly StringName OnHolderPressed = StringName.op_Implicit(nameof (OnHolderPressed));
    public static readonly StringName CanPlayCards = StringName.op_Implicit(nameof (CanPlayCards));
    public static readonly StringName AreCardActionsAllowed = StringName.op_Implicit(nameof (AreCardActionsAllowed));
    public static readonly StringName StartCardPlay = StringName.op_Implicit(nameof (StartCardPlay));
    public static readonly StringName SelectCardInSimpleMode = StringName.op_Implicit(nameof (SelectCardInSimpleMode));
    public static readonly StringName SelectCardInUpgradeMode = StringName.op_Implicit(nameof (SelectCardInUpgradeMode));
    public static readonly StringName DeselectCard = StringName.op_Implicit(nameof (DeselectCard));
    public static readonly StringName RevalidateSelectionAfterStateChange = StringName.op_Implicit(nameof (RevalidateSelectionAfterStateChange));
    public static readonly StringName OnSelectModeConfirmButtonPressed = StringName.op_Implicit(nameof (OnSelectModeConfirmButtonPressed));
    public static readonly StringName CheckIfSelectionComplete = StringName.op_Implicit(nameof (CheckIfSelectionComplete));
    public static readonly StringName RefreshSelectModeConfirmButton = StringName.op_Implicit(nameof (RefreshSelectModeConfirmButton));
    public static readonly StringName AnimIn = StringName.op_Implicit(nameof (AnimIn));
    public static readonly StringName AnimOut = StringName.op_Implicit(nameof (AnimOut));
    public static readonly StringName AnimDisable = StringName.op_Implicit(nameof (AnimDisable));
    public static readonly StringName AnimEnable = StringName.op_Implicit(nameof (AnimEnable));
    public static readonly StringName FlashPlayableHolders = StringName.op_Implicit(nameof (FlashPlayableHolders));
    public static readonly StringName OnCardSelected = StringName.op_Implicit(nameof (OnCardSelected));
    public static readonly StringName OnCardDeselected = StringName.op_Implicit(nameof (OnCardDeselected));
    public static readonly StringName UpdateSelectedCardContainer = StringName.op_Implicit(nameof (UpdateSelectedCardContainer));
    public static readonly StringName EnableControllerNavigation = StringName.op_Implicit(nameof (EnableControllerNavigation));
    public static readonly StringName DisableControllerNavigation = StringName.op_Implicit(nameof (DisableControllerNavigation));
    public static readonly StringName _UnhandledInput = StringName.op_Implicit(nameof (_UnhandledInput));
  }

  public class PropertyName : Control.PropertyName
  {
    public static readonly StringName CardHolderContainer = StringName.op_Implicit(nameof (CardHolderContainer));
    public static readonly StringName PeekButton = StringName.op_Implicit(nameof (PeekButton));
    public static readonly StringName InCardPlay = StringName.op_Implicit(nameof (InCardPlay));
    public static readonly StringName IsInCardSelection = StringName.op_Implicit(nameof (IsInCardSelection));
    public static readonly StringName CurrentMode = StringName.op_Implicit(nameof (CurrentMode));
    public static readonly StringName HasDraggedHolder = StringName.op_Implicit(nameof (HasDraggedHolder));
    public static readonly StringName FocusedHolder = StringName.op_Implicit(nameof (FocusedHolder));
    public static readonly StringName DefaultFocusedControl = StringName.op_Implicit(nameof (DefaultFocusedControl));
    public static readonly StringName _selectCardShortcuts = StringName.op_Implicit(nameof (_selectCardShortcuts));
    public static readonly StringName _selectModeBackstop = StringName.op_Implicit(nameof (_selectModeBackstop));
    public static readonly StringName _upgradePreviewContainer = StringName.op_Implicit(nameof (_upgradePreviewContainer));
    public static readonly StringName _selectedHandCardContainer = StringName.op_Implicit(nameof (_selectedHandCardContainer));
    public static readonly StringName _upgradePreview = StringName.op_Implicit(nameof (_upgradePreview));
    public static readonly StringName _selectModeConfirmButton = StringName.op_Implicit(nameof (_selectModeConfirmButton));
    public static readonly StringName _selectionHeader = StringName.op_Implicit(nameof (_selectionHeader));
    public static readonly StringName _currentCardPlay = StringName.op_Implicit(nameof (_currentCardPlay));
    public static readonly StringName _currentMode = StringName.op_Implicit(nameof (_currentMode));
    public static readonly StringName _draggedHolderIndex = StringName.op_Implicit(nameof (_draggedHolderIndex));
    public static readonly StringName _lastFocusedHolderIdx = StringName.op_Implicit(nameof (_lastFocusedHolderIdx));
    public static readonly StringName _animEnableTween = StringName.op_Implicit(nameof (_animEnableTween));
    public static readonly StringName _isDisabled = StringName.op_Implicit(nameof (_isDisabled));
    public static readonly StringName _animInTween = StringName.op_Implicit(nameof (_animInTween));
    public static readonly StringName _animOutTween = StringName.op_Implicit(nameof (_animOutTween));
    public static readonly StringName _selectedCardScaleTween = StringName.op_Implicit(nameof (_selectedCardScaleTween));
  }

  public class SignalName : Control.SignalName
  {
    public static readonly StringName ModeChanged = StringName.op_Implicit(nameof (ModeChanged));
  }
}
