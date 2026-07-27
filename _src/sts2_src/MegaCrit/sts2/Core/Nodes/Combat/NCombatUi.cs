// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Combat.NCombatUi
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.Debug;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Cards;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Screens.ScreenContext;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Runs;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Combat;

[ScriptPath("res://src/Core/Nodes/Combat/NCombatUi.cs")]
public class NCombatUi : Control
{
  private NStarCounter _starCounter;
  private NEnergyCounter _energyCounter;
  private NCombatPilesContainer _combatPilesContainer;
  private readonly Dictionary<NCard, Vector2> _originalPlayContainerCardPositions = new Dictionary<NCard, Vector2>();
  private readonly Dictionary<NCard, Vector2> _originalPlayContainerCardScales = new Dictionary<NCard, Vector2>();
  private Tween? _playContainerPeekModeTween;
  private readonly CancellationTokenSource _cts = new CancellationTokenSource();
  private int _originalHandChildIndex;
  private CombatState _state;
  private static bool _isDebugSlowRewards;
  private static bool _isDebugHidden;
  private static bool _isDebugHidingHand;

  public Control EnergyCounterContainer { get; private set; }

  public NEndTurnButton EndTurnButton { get; private set; }

  private NPingButton PingButton { get; set; }

  public NDrawPileButton DrawPile => this._combatPilesContainer.DrawPile;

  public NDiscardPileButton DiscardPile => this._combatPilesContainer.DiscardPile;

  public NExhaustPileButton ExhaustPile => this._combatPilesContainer.ExhaustPile;

  public NPlayerHand Hand { get; private set; }

  public Control PlayContainer { get; private set; }

  public NCardPlayQueue PlayQueue { get; private set; }

  public Control CardPreviewContainer { get; private set; }

  public NMessyCardPreviewContainer MessyCardPreviewContainer { get; private set; }

  private IEnumerable<NCard> PlayContainerCards
  {
    get => ((IEnumerable) ((Node) this.PlayContainer).GetChildren(false)).OfType<NCard>();
  }

  public static bool IsDebugHidingIntent { get; private set; }

  public static bool IsDebugHidingPlayContainer { get; private set; }

  public static bool IsDebugHidingHpBar { get; private set; }

  public static bool IsDebugHideTextVfx { get; private set; }

  public static bool IsDebugHideTargetingUi { get; private set; }

  public static bool IsDebugHideMpTargetingUi { get; private set; }

  public static bool IsDebugHideMpIntents { get; private set; }

  public event Action? DebugToggleIntent;

  public event Action? DebugToggleHpBar;

  public override void _Ready()
  {
    this.EnergyCounterContainer = ((Node) this).GetNode<Control>(NodePath.op_Implicit("%EnergyCounterContainer"));
    this._starCounter = ((Node) this).GetNode<NStarCounter>(NodePath.op_Implicit("%StarCounter"));
    this.EndTurnButton = ((Node) this).GetNode<NEndTurnButton>(NodePath.op_Implicit("%EndTurnButton"));
    this.PingButton = ((Node) this).GetNode<NPingButton>(NodePath.op_Implicit("%PingButton"));
    this._combatPilesContainer = ((Node) this).GetNode<NCombatPilesContainer>(NodePath.op_Implicit("%CombatPileContainer"));
    this.Hand = ((Node) this).GetNode<NPlayerHand>(NodePath.op_Implicit("%Hand"));
    this.PlayContainer = ((Node) this).GetNode<Control>(NodePath.op_Implicit("%PlayContainer"));
    this.PlayQueue = ((Node) this).GetNode<NCardPlayQueue>(NodePath.op_Implicit("%PlayQueue"));
    this.CardPreviewContainer = ((Node) this).GetNode<Control>(NodePath.op_Implicit("%CardPreviewContainer"));
    this.MessyCardPreviewContainer = ((Node) this).GetNode<NMessyCardPreviewContainer>(NodePath.op_Implicit("%MessyCardPreviewContainer"));
    if (!NCombatUi._isDebugHidden)
      return;
    foreach (Control control in ((IEnumerable) ((Node) this).GetChildren(false)).OfType<Control>())
    {
      if (control != this.Hand)
        ((CanvasItem) control).Modulate = NCombatUi._isDebugHidden ? Colors.Transparent : Colors.White;
    }
  }

  public override void _ExitTree()
  {
    this._cts.Cancel();
    this.DisconnectSignals();
  }

  public void Activate(CombatState state)
  {
    CombatManager.Instance.CombatEnded += new Action<CombatRoom>(this.OnCombatEnded);
    CombatManager.Instance.CombatWon += new Action<CombatRoom>(this.OnCombatWon);
    this._state = state;
    Player me = LocalContext.GetMe((ICombatState) this._state);
    this._combatPilesContainer.Initialize(me);
    this._starCounter.Initialize(me);
    this.EndTurnButton.Initialize(state);
    if (me.Character.ShouldAlwaysShowStarCounter)
      this.EnergyCounterContainer.SetPosition(new Vector2(100f, 806f), true);
    this._energyCounter = NEnergyCounter.Create(me);
    ((Node) this.EnergyCounterContainer).AddChildSafely((Node) this._energyCounter);
    ((Node) this._starCounter).Reparent((Node) this._energyCounter, true);
    ((CanvasItem) this).Visible = true;
    this.AnimIn();
  }

  public void Deactivate()
  {
    this.DisconnectSignals();
    ((CanvasItem) this).Visible = false;
  }

  private void DisconnectSignals()
  {
    CombatManager.Instance.CombatEnded -= new Action<CombatRoom>(this.OnCombatEnded);
    CombatManager.Instance.CombatWon -= new Action<CombatRoom>(this.OnCombatWon);
  }

  public void AddToPlayContainer(NCard card)
  {
    Node parent = ((Node) card).GetParent();
    if (parent != null)
      parent.RemoveChildSafely((Node) card);
    ((Node) this.PlayContainer).AddChildSafely((Node) card);
  }

  public NCard? GetCardFromPlayContainer(CardModel model)
  {
    return this.PlayContainerCards.FirstOrDefault<NCard>((Func<NCard, bool>) (n => n.Model == model));
  }

  private void OnCombatEnded(CombatRoom combatRoom)
  {
    this.AnimOut();
    this.PostCombatCleanUp();
  }

  private void OnCombatWon(CombatRoom room)
  {
    if (room.Encounter.ShouldGiveRewards)
      TaskHelper.RunSafely(this.ShowRewards(room));
    else
      TaskHelper.RunSafely(this.ProceedWithoutRewards());
  }

  public async Task ProceedWithoutRewards()
  {
    await Cmd.Wait(1f, this._cts.Token);
    await RunManager.Instance.ProceedFromTerminalRewardsScreen();
  }

  private async Task ShowRewards(CombatRoom room)
  {
    float num = 0.0f;
    foreach (NCreature removingCreatureNode in NCombatRoom.Instance.RemovingCreatureNodes)
    {
      if (removingCreatureNode != null && removingCreatureNode.HasSpineAnimation && removingCreatureNode.IsPlayingDeathAnimation)
      {
        num = Math.Max(num, removingCreatureNode.GetCurrentAnimationTimeRemaining());
      }
      else
      {
        MonsterModel monster = removingCreatureNode.Entity.Monster;
        if (monster != null && monster.HasDeathAnimLengthOverride)
          num = Math.Max(num, removingCreatureNode.Entity.Monster.DeathAnimLengthOverride);
      }
    }
    if (NCombatUi._isDebugSlowRewards)
      await Cmd.Wait(num + 3f, this._cts.Token);
    else if (room.RoomType != RoomType.Boss)
      await Cmd.CustomScaledWait(0.5f, num + 1f, cancellationToken: this._cts.Token);
    else
      await Cmd.CustomScaledWait(num * 0.5f, num + 1f, cancellationToken: this._cts.Token);
    await room.OfferRoomEndRewards();
  }

  private void AnimIn()
  {
    this.Hand.AnimIn();
    this._energyCounter.AnimIn();
    this._combatPilesContainer.AnimIn();
  }

  public void AnimOut()
  {
    this.Hand.AnimOut();
    this.PlayQueue.AnimOut();
    this.EndTurnButton.OnCombatEnded();
    this.PingButton.OnCombatEnded();
    this._energyCounter.AnimOut();
    this._combatPilesContainer.AnimOut();
  }

  private void PostCombatCleanUp()
  {
    ((Node) this).CreateTween().TweenProperty((GodotObject) this.PlayContainer, NodePath.op_Implicit("modulate"), Variant.op_Implicit(Colors.Transparent), 0.25);
  }

  public void OnHandSelectModeEntered()
  {
    this._originalHandChildIndex = ((Node) this.Hand).GetIndex(false);
    ((CanvasItem) this.Hand).MoveToFrontSafely();
    ActiveScreenContext.Instance.Update();
  }

  public void OnHandSelectModeExited()
  {
    ((Node) this).MoveChildSafely((Node) this.Hand, this._originalHandChildIndex);
    ActiveScreenContext.Instance.Update();
  }

  public void OnPeekButtonReady(NPeekButton peekButton)
  {
    ((GodotObject) peekButton).Connect(NPeekButton.SignalName.Toggled, Callable.From<NPeekButton>(new Action<NPeekButton>(this.OnPeekButtonToggled)), 0U);
  }

  private void OnPeekButtonToggled(NPeekButton peekButton)
  {
    if (this._playContainerPeekModeTween != null)
    {
      this._playContainerPeekModeTween.Pause();
      this._playContainerPeekModeTween.CustomStep(0.25);
      this._playContainerPeekModeTween.Kill();
      this._playContainerPeekModeTween = (Tween) null;
    }
    Rect2 viewportRect = ((CanvasItem) this).GetViewportRect();
    Vector2 size = ((Rect2) ref viewportRect).Size;
    if (peekButton.IsPeeking)
    {
      ((CanvasItem) this.PlayQueue).Hide();
      foreach (NCard playContainerCard in this.PlayContainerCards)
      {
        this._originalPlayContainerCardPositions[playContainerCard] = playContainerCard.Position;
        this._originalPlayContainerCardScales[playContainerCard] = playContainerCard.Scale;
        Vector2 globalPosition = ((Node2D) peekButton.CurrentCardMarker).GlobalPosition;
        Vector2 vector2 = Vector2.op_Multiply(playContainerCard.Scale, 0.5f);
        if (this._playContainerPeekModeTween == null)
          this._playContainerPeekModeTween = ((Node) this).CreateTween();
        this._playContainerPeekModeTween.TweenProperty((GodotObject) playContainerCard, NodePath.op_Implicit("global_position"), Variant.op_Implicit(globalPosition), 0.25).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 7L);
        this._playContainerPeekModeTween.Parallel().TweenProperty((GodotObject) playContainerCard, NodePath.op_Implicit("scale"), Variant.op_Implicit(vector2), 0.25).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 7L);
      }
    }
    else
    {
      ((CanvasItem) this.PlayQueue).Show();
      foreach (NCard playContainerCard in this.PlayContainerCards)
      {
        Vector2 vector2_1;
        Vector2 vector2_2 = this._originalPlayContainerCardPositions.TryGetValue(playContainerCard, out vector2_1) ? vector2_1 : Vector2.op_Multiply(size, 0.5f);
        Vector2 vector2_3;
        Vector2 vector2_4 = this._originalPlayContainerCardScales.TryGetValue(playContainerCard, out vector2_3) ? vector2_3 : Vector2.One;
        if (this._playContainerPeekModeTween == null)
          this._playContainerPeekModeTween = ((Node) this).CreateTween();
        this._playContainerPeekModeTween.TweenProperty((GodotObject) playContainerCard, NodePath.op_Implicit("position"), Variant.op_Implicit(vector2_2), 0.25).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 7L);
        this._playContainerPeekModeTween.Parallel().TweenProperty((GodotObject) playContainerCard, NodePath.op_Implicit("scale"), Variant.op_Implicit(vector2_4), 0.25).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 7L);
      }
      this._originalPlayContainerCardPositions.Clear();
      this._originalPlayContainerCardScales.Clear();
    }
    ActiveScreenContext.Instance.Update();
  }

  public void Enable()
  {
    NPlayerHand hand = this.Hand;
    if (hand != null && hand.IsInCardSelection)
    {
      NPeekButton peekButton = hand.PeekButton;
      if (peekButton != null && !peekButton.IsPeeking)
      {
        this._combatPilesContainer.Disable();
        goto label_4;
      }
    }
    this._combatPilesContainer.Enable();
label_4:
    if (this._state.CurrentSide == CombatSide.Player)
      this.EndTurnButton.RefreshEnabled();
    if (this._state.PlayerCreatures.Count > 1)
      this.PingButton.RefreshEnabled();
    bool flag;
    switch (this.Hand.CurrentMode)
    {
      case NPlayerHand.Mode.SimpleSelect:
      case NPlayerHand.Mode.UpgradeSelect:
        flag = true;
        break;
      default:
        flag = false;
        break;
    }
    if (!flag)
      return;
    this.Hand.PeekButton.Enable();
  }

  public void Disable()
  {
    this._combatPilesContainer.Disable();
    this.EndTurnButton.RefreshEnabled();
    this.PingButton.RefreshEnabled();
    this.Hand.PeekButton.Disable();
    this.Hand.CancelAllCardPlay();
  }

  public override void _Input(InputEvent inputEvent)
  {
    if (inputEvent.IsActionReleased(DebugHotkey.hideIntents, false))
    {
      NCombatUi.IsDebugHidingIntent = !NCombatUi.IsDebugHidingIntent;
      ((Node) NGame.Instance).AddChildSafely((Node) NFullscreenTextVfx.Create(NCombatUi.IsDebugHidingIntent ? "Hide Intents" : "Show Intents"));
      Action debugToggleIntent = this.DebugToggleIntent;
      if (debugToggleIntent != null)
        debugToggleIntent();
    }
    if (inputEvent.IsActionReleased(DebugHotkey.hideCombatUi, false))
    {
      NCombatUi._isDebugHidden = !NCombatUi._isDebugHidden;
      ((Node) NGame.Instance).AddChildSafely((Node) NFullscreenTextVfx.Create(NCombatUi._isDebugHidden ? "Hide Combat UI" : "Show Combat UI"));
      this.DebugHideCombatUi();
    }
    else if (inputEvent.IsActionReleased(DebugHotkey.hidePlayContainer, false))
    {
      NCombatUi.IsDebugHidingPlayContainer = !NCombatUi.IsDebugHidingPlayContainer;
      ((Node) NGame.Instance).AddChildSafely((Node) NFullscreenTextVfx.Create(NCombatUi.IsDebugHidingPlayContainer ? "Hide Played Card" : "Show Played Card"));
      this.DebugHideCombatUi();
    }
    else if (inputEvent.IsActionReleased(DebugHotkey.hideHand, false))
    {
      NCombatUi._isDebugHidingHand = !NCombatUi._isDebugHidingHand;
      ((Node) NGame.Instance).AddChildSafely((Node) NFullscreenTextVfx.Create(NCombatUi._isDebugHidingHand ? "Hide Hand Cards" : "Show Hand Cards"));
      this.DebugHideCombatUi();
    }
    else if (inputEvent.IsActionReleased(DebugHotkey.hideHpBars, false))
    {
      NCombatUi.IsDebugHidingHpBar = !NCombatUi.IsDebugHidingHpBar;
      ((Node) NGame.Instance).AddChildSafely((Node) NFullscreenTextVfx.Create(NCombatUi.IsDebugHidingHpBar ? "Hide HP Bars" : "Show HP Bars"));
      Action debugToggleHpBar = this.DebugToggleHpBar;
      if (debugToggleHpBar == null)
        return;
      debugToggleHpBar();
    }
    else if (inputEvent.IsActionReleased(DebugHotkey.hideTextVfx, false))
    {
      NCombatUi.IsDebugHideTextVfx = !NCombatUi.IsDebugHideTextVfx;
      ((Node) NGame.Instance).AddChildSafely((Node) NFullscreenTextVfx.Create(NCombatUi.IsDebugHideTextVfx ? "Hide Text Vfx" : "Show Text Vfx"));
    }
    else if (inputEvent.IsActionReleased(DebugHotkey.hideTargetingUi, false))
    {
      NCombatUi.IsDebugHideTargetingUi = !NCombatUi.IsDebugHideTargetingUi;
      ((Node) NGame.Instance).AddChildSafely((Node) NFullscreenTextVfx.Create(NCombatUi.IsDebugHideTargetingUi ? "Hide Targeting UI" : "Show Targeting UI"));
    }
    else if (inputEvent.IsActionReleased(DebugHotkey.slowRewards, false))
    {
      NCombatUi._isDebugSlowRewards = !NCombatUi._isDebugSlowRewards;
      ((Node) NGame.Instance).AddChildSafely((Node) NFullscreenTextVfx.Create(NCombatUi._isDebugSlowRewards ? "Slow Rewards Screens" : "Normal Rewards Screen"));
    }
    else if (inputEvent.IsActionReleased(DebugHotkey.hideMpTargeting, false))
    {
      NCombatUi.IsDebugHideMpTargetingUi = !NCombatUi.IsDebugHideMpTargetingUi;
      ((Node) NGame.Instance).AddChildSafely((Node) NFullscreenTextVfx.Create(NCombatUi.IsDebugHideMpTargetingUi ? "Hide MP Targeting" : "Show MP Targeting"));
    }
    else
    {
      if (!inputEvent.IsActionReleased(DebugHotkey.hideMpIntents, false))
        return;
      NCombatUi.IsDebugHideMpIntents = !NCombatUi.IsDebugHideMpIntents;
      ((Node) NGame.Instance).AddChildSafely((Node) NFullscreenTextVfx.Create(NCombatUi.IsDebugHideMpIntents ? "Hide MP Intents" : "Show MP Intents"));
    }
  }

  private void DebugHideCombatUi()
  {
    foreach (Control control in ((IEnumerable) ((Node) this).GetChildren(false)).OfType<Control>())
    {
      if (control == this.Hand)
        ((CanvasItem) control).Modulate = NCombatUi._isDebugHidingHand ? Colors.Transparent : Colors.White;
      else if (StringName.op_Equality(((Node) control).Name, NCombatUi.PropertyName.PlayContainer))
        ((CanvasItem) control).Modulate = NCombatUi.IsDebugHidingPlayContainer ? Colors.Transparent : Colors.White;
      else
        ((CanvasItem) control).Modulate = NCombatUi._isDebugHidden ? Colors.Transparent : Colors.White;
    }
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(16 /*0x10*/)
    {
      new MethodInfo(NCombatUi.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCombatUi.MethodName._ExitTree, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCombatUi.MethodName.Deactivate, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCombatUi.MethodName.DisconnectSignals, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCombatUi.MethodName.AddToPlayContainer, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("card"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false)
      }, (List<Variant>) null),
      new MethodInfo(NCombatUi.MethodName.AnimIn, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCombatUi.MethodName.AnimOut, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCombatUi.MethodName.PostCombatCleanUp, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCombatUi.MethodName.OnHandSelectModeEntered, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCombatUi.MethodName.OnHandSelectModeExited, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCombatUi.MethodName.OnPeekButtonReady, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("peekButton"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false)
      }, (List<Variant>) null),
      new MethodInfo(NCombatUi.MethodName.OnPeekButtonToggled, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("peekButton"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false)
      }, (List<Variant>) null),
      new MethodInfo(NCombatUi.MethodName.Enable, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCombatUi.MethodName.Disable, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCombatUi.MethodName._Input, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("inputEvent"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("InputEvent"), false)
      }, (List<Variant>) null),
      new MethodInfo(NCombatUi.MethodName.DebugHideCombatUi, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NCombatUi.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCombatUi.MethodName._ExitTree) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._ExitTree();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCombatUi.MethodName.Deactivate) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.Deactivate();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCombatUi.MethodName.DisconnectSignals) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.DisconnectSignals();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCombatUi.MethodName.AddToPlayContainer) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.AddToPlayContainer(VariantUtils.ConvertTo<NCard>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCombatUi.MethodName.AnimIn) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.AnimIn();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCombatUi.MethodName.AnimOut) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.AnimOut();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCombatUi.MethodName.PostCombatCleanUp) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.PostCombatCleanUp();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCombatUi.MethodName.OnHandSelectModeEntered) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnHandSelectModeEntered();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCombatUi.MethodName.OnHandSelectModeExited) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnHandSelectModeExited();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCombatUi.MethodName.OnPeekButtonReady) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.OnPeekButtonReady(VariantUtils.ConvertTo<NPeekButton>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCombatUi.MethodName.OnPeekButtonToggled) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.OnPeekButtonToggled(VariantUtils.ConvertTo<NPeekButton>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCombatUi.MethodName.Enable) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.Enable();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCombatUi.MethodName.Disable) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.Disable();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCombatUi.MethodName._Input) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      ((Node) this)._Input(VariantUtils.ConvertTo<InputEvent>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NCombatUi.MethodName.DebugHideCombatUi) || ((NativeVariantPtrArgs) ref args).Count != 0)
      return base.InvokeGodotClassMethod(ref method, args, ref ret);
    this.DebugHideCombatUi();
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NCombatUi.MethodName._Ready) || StringName.op_Equality(ref method, NCombatUi.MethodName._ExitTree) || StringName.op_Equality(ref method, NCombatUi.MethodName.Deactivate) || StringName.op_Equality(ref method, NCombatUi.MethodName.DisconnectSignals) || StringName.op_Equality(ref method, NCombatUi.MethodName.AddToPlayContainer) || StringName.op_Equality(ref method, NCombatUi.MethodName.AnimIn) || StringName.op_Equality(ref method, NCombatUi.MethodName.AnimOut) || StringName.op_Equality(ref method, NCombatUi.MethodName.PostCombatCleanUp) || StringName.op_Equality(ref method, NCombatUi.MethodName.OnHandSelectModeEntered) || StringName.op_Equality(ref method, NCombatUi.MethodName.OnHandSelectModeExited) || StringName.op_Equality(ref method, NCombatUi.MethodName.OnPeekButtonReady) || StringName.op_Equality(ref method, NCombatUi.MethodName.OnPeekButtonToggled) || StringName.op_Equality(ref method, NCombatUi.MethodName.Enable) || StringName.op_Equality(ref method, NCombatUi.MethodName.Disable) || StringName.op_Equality(ref method, NCombatUi.MethodName._Input) || StringName.op_Equality(ref method, NCombatUi.MethodName.DebugHideCombatUi) || base.HasGodotClassMethod(ref method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NCombatUi.PropertyName.EnergyCounterContainer))
    {
      this.EnergyCounterContainer = VariantUtils.ConvertTo<Control>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCombatUi.PropertyName.EndTurnButton))
    {
      this.EndTurnButton = VariantUtils.ConvertTo<NEndTurnButton>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCombatUi.PropertyName.PingButton))
    {
      this.PingButton = VariantUtils.ConvertTo<NPingButton>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCombatUi.PropertyName.Hand))
    {
      this.Hand = VariantUtils.ConvertTo<NPlayerHand>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCombatUi.PropertyName.PlayContainer))
    {
      this.PlayContainer = VariantUtils.ConvertTo<Control>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCombatUi.PropertyName.PlayQueue))
    {
      this.PlayQueue = VariantUtils.ConvertTo<NCardPlayQueue>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCombatUi.PropertyName.CardPreviewContainer))
    {
      this.CardPreviewContainer = VariantUtils.ConvertTo<Control>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCombatUi.PropertyName.MessyCardPreviewContainer))
    {
      this.MessyCardPreviewContainer = VariantUtils.ConvertTo<NMessyCardPreviewContainer>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCombatUi.PropertyName._starCounter))
    {
      this._starCounter = VariantUtils.ConvertTo<NStarCounter>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCombatUi.PropertyName._energyCounter))
    {
      this._energyCounter = VariantUtils.ConvertTo<NEnergyCounter>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCombatUi.PropertyName._combatPilesContainer))
    {
      this._combatPilesContainer = VariantUtils.ConvertTo<NCombatPilesContainer>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCombatUi.PropertyName._playContainerPeekModeTween))
    {
      this._playContainerPeekModeTween = VariantUtils.ConvertTo<Tween>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NCombatUi.PropertyName._originalHandChildIndex))
      return ((GodotObject) this).SetGodotClassPropertyValue(ref name, ref value);
    this._originalHandChildIndex = VariantUtils.ConvertTo<int>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NCombatUi.PropertyName.EnergyCounterContainer))
    {
      ref godot_variant local = ref value;
      Control counterContainer = this.EnergyCounterContainer;
      godot_variant from = VariantUtils.CreateFrom<Control>(ref counterContainer);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NCombatUi.PropertyName.EndTurnButton))
    {
      ref godot_variant local = ref value;
      NEndTurnButton endTurnButton = this.EndTurnButton;
      godot_variant from = VariantUtils.CreateFrom<NEndTurnButton>(ref endTurnButton);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NCombatUi.PropertyName.PingButton))
    {
      ref godot_variant local = ref value;
      NPingButton pingButton = this.PingButton;
      godot_variant from = VariantUtils.CreateFrom<NPingButton>(ref pingButton);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NCombatUi.PropertyName.DrawPile))
    {
      ref godot_variant local = ref value;
      NDrawPileButton drawPile = this.DrawPile;
      godot_variant from = VariantUtils.CreateFrom<NDrawPileButton>(ref drawPile);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NCombatUi.PropertyName.DiscardPile))
    {
      ref godot_variant local = ref value;
      NDiscardPileButton discardPile = this.DiscardPile;
      godot_variant from = VariantUtils.CreateFrom<NDiscardPileButton>(ref discardPile);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NCombatUi.PropertyName.ExhaustPile))
    {
      ref godot_variant local = ref value;
      NExhaustPileButton exhaustPile = this.ExhaustPile;
      godot_variant from = VariantUtils.CreateFrom<NExhaustPileButton>(ref exhaustPile);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NCombatUi.PropertyName.Hand))
    {
      ref godot_variant local = ref value;
      NPlayerHand hand = this.Hand;
      godot_variant from = VariantUtils.CreateFrom<NPlayerHand>(ref hand);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NCombatUi.PropertyName.PlayContainer))
    {
      ref godot_variant local = ref value;
      Control playContainer = this.PlayContainer;
      godot_variant from = VariantUtils.CreateFrom<Control>(ref playContainer);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NCombatUi.PropertyName.PlayQueue))
    {
      ref godot_variant local = ref value;
      NCardPlayQueue playQueue = this.PlayQueue;
      godot_variant from = VariantUtils.CreateFrom<NCardPlayQueue>(ref playQueue);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NCombatUi.PropertyName.CardPreviewContainer))
    {
      ref godot_variant local = ref value;
      Control previewContainer = this.CardPreviewContainer;
      godot_variant from = VariantUtils.CreateFrom<Control>(ref previewContainer);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NCombatUi.PropertyName.MessyCardPreviewContainer))
    {
      ref godot_variant local = ref value;
      NMessyCardPreviewContainer previewContainer = this.MessyCardPreviewContainer;
      godot_variant from = VariantUtils.CreateFrom<NMessyCardPreviewContainer>(ref previewContainer);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NCombatUi.PropertyName._starCounter))
    {
      value = VariantUtils.CreateFrom<NStarCounter>(ref this._starCounter);
      return true;
    }
    if (StringName.op_Equality(ref name, NCombatUi.PropertyName._energyCounter))
    {
      value = VariantUtils.CreateFrom<NEnergyCounter>(ref this._energyCounter);
      return true;
    }
    if (StringName.op_Equality(ref name, NCombatUi.PropertyName._combatPilesContainer))
    {
      value = VariantUtils.CreateFrom<NCombatPilesContainer>(ref this._combatPilesContainer);
      return true;
    }
    if (StringName.op_Equality(ref name, NCombatUi.PropertyName._playContainerPeekModeTween))
    {
      value = VariantUtils.CreateFrom<Tween>(ref this._playContainerPeekModeTween);
      return true;
    }
    if (!StringName.op_Equality(ref name, NCombatUi.PropertyName._originalHandChildIndex))
      return ((GodotObject) this).GetGodotClassPropertyValue(ref name, ref value);
    value = VariantUtils.CreateFrom<int>(ref this._originalHandChildIndex);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NCombatUi.PropertyName.EnergyCounterContainer, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NCombatUi.PropertyName.EndTurnButton, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NCombatUi.PropertyName.PingButton, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NCombatUi.PropertyName._starCounter, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NCombatUi.PropertyName._energyCounter, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NCombatUi.PropertyName._combatPilesContainer, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NCombatUi.PropertyName.DrawPile, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NCombatUi.PropertyName.DiscardPile, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NCombatUi.PropertyName.ExhaustPile, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NCombatUi.PropertyName.Hand, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NCombatUi.PropertyName.PlayContainer, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NCombatUi.PropertyName.PlayQueue, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NCombatUi.PropertyName.CardPreviewContainer, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NCombatUi.PropertyName.MessyCardPreviewContainer, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NCombatUi.PropertyName._playContainerPeekModeTween, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 2L, NCombatUi.PropertyName._originalHandChildIndex, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).SaveGodotObjectData(info);
    GodotSerializationInfo serializationInfo1 = info;
    StringName counterContainer1 = NCombatUi.PropertyName.EnergyCounterContainer;
    Control counterContainer2 = this.EnergyCounterContainer;
    Variant variant1 = Variant.From<Control>(ref counterContainer2);
    serializationInfo1.AddProperty(counterContainer1, variant1);
    GodotSerializationInfo serializationInfo2 = info;
    StringName endTurnButton1 = NCombatUi.PropertyName.EndTurnButton;
    NEndTurnButton endTurnButton2 = this.EndTurnButton;
    Variant variant2 = Variant.From<NEndTurnButton>(ref endTurnButton2);
    serializationInfo2.AddProperty(endTurnButton1, variant2);
    GodotSerializationInfo serializationInfo3 = info;
    StringName pingButton1 = NCombatUi.PropertyName.PingButton;
    NPingButton pingButton2 = this.PingButton;
    Variant variant3 = Variant.From<NPingButton>(ref pingButton2);
    serializationInfo3.AddProperty(pingButton1, variant3);
    GodotSerializationInfo serializationInfo4 = info;
    StringName hand1 = NCombatUi.PropertyName.Hand;
    NPlayerHand hand2 = this.Hand;
    Variant variant4 = Variant.From<NPlayerHand>(ref hand2);
    serializationInfo4.AddProperty(hand1, variant4);
    GodotSerializationInfo serializationInfo5 = info;
    StringName playContainer1 = NCombatUi.PropertyName.PlayContainer;
    Control playContainer2 = this.PlayContainer;
    Variant variant5 = Variant.From<Control>(ref playContainer2);
    serializationInfo5.AddProperty(playContainer1, variant5);
    GodotSerializationInfo serializationInfo6 = info;
    StringName playQueue1 = NCombatUi.PropertyName.PlayQueue;
    NCardPlayQueue playQueue2 = this.PlayQueue;
    Variant variant6 = Variant.From<NCardPlayQueue>(ref playQueue2);
    serializationInfo6.AddProperty(playQueue1, variant6);
    GodotSerializationInfo serializationInfo7 = info;
    StringName previewContainer1 = NCombatUi.PropertyName.CardPreviewContainer;
    Control previewContainer2 = this.CardPreviewContainer;
    Variant variant7 = Variant.From<Control>(ref previewContainer2);
    serializationInfo7.AddProperty(previewContainer1, variant7);
    GodotSerializationInfo serializationInfo8 = info;
    StringName previewContainer3 = NCombatUi.PropertyName.MessyCardPreviewContainer;
    NMessyCardPreviewContainer previewContainer4 = this.MessyCardPreviewContainer;
    Variant variant8 = Variant.From<NMessyCardPreviewContainer>(ref previewContainer4);
    serializationInfo8.AddProperty(previewContainer3, variant8);
    info.AddProperty(NCombatUi.PropertyName._starCounter, Variant.From<NStarCounter>(ref this._starCounter));
    info.AddProperty(NCombatUi.PropertyName._energyCounter, Variant.From<NEnergyCounter>(ref this._energyCounter));
    info.AddProperty(NCombatUi.PropertyName._combatPilesContainer, Variant.From<NCombatPilesContainer>(ref this._combatPilesContainer));
    info.AddProperty(NCombatUi.PropertyName._playContainerPeekModeTween, Variant.From<Tween>(ref this._playContainerPeekModeTween));
    info.AddProperty(NCombatUi.PropertyName._originalHandChildIndex, Variant.From<int>(ref this._originalHandChildIndex));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NCombatUi.PropertyName.EnergyCounterContainer, ref variant1))
      this.EnergyCounterContainer = ((Variant) ref variant1).As<Control>();
    Variant variant2;
    if (info.TryGetProperty(NCombatUi.PropertyName.EndTurnButton, ref variant2))
      this.EndTurnButton = ((Variant) ref variant2).As<NEndTurnButton>();
    Variant variant3;
    if (info.TryGetProperty(NCombatUi.PropertyName.PingButton, ref variant3))
      this.PingButton = ((Variant) ref variant3).As<NPingButton>();
    Variant variant4;
    if (info.TryGetProperty(NCombatUi.PropertyName.Hand, ref variant4))
      this.Hand = ((Variant) ref variant4).As<NPlayerHand>();
    Variant variant5;
    if (info.TryGetProperty(NCombatUi.PropertyName.PlayContainer, ref variant5))
      this.PlayContainer = ((Variant) ref variant5).As<Control>();
    Variant variant6;
    if (info.TryGetProperty(NCombatUi.PropertyName.PlayQueue, ref variant6))
      this.PlayQueue = ((Variant) ref variant6).As<NCardPlayQueue>();
    Variant variant7;
    if (info.TryGetProperty(NCombatUi.PropertyName.CardPreviewContainer, ref variant7))
      this.CardPreviewContainer = ((Variant) ref variant7).As<Control>();
    Variant variant8;
    if (info.TryGetProperty(NCombatUi.PropertyName.MessyCardPreviewContainer, ref variant8))
      this.MessyCardPreviewContainer = ((Variant) ref variant8).As<NMessyCardPreviewContainer>();
    Variant variant9;
    if (info.TryGetProperty(NCombatUi.PropertyName._starCounter, ref variant9))
      this._starCounter = ((Variant) ref variant9).As<NStarCounter>();
    Variant variant10;
    if (info.TryGetProperty(NCombatUi.PropertyName._energyCounter, ref variant10))
      this._energyCounter = ((Variant) ref variant10).As<NEnergyCounter>();
    Variant variant11;
    if (info.TryGetProperty(NCombatUi.PropertyName._combatPilesContainer, ref variant11))
      this._combatPilesContainer = ((Variant) ref variant11).As<NCombatPilesContainer>();
    Variant variant12;
    if (info.TryGetProperty(NCombatUi.PropertyName._playContainerPeekModeTween, ref variant12))
      this._playContainerPeekModeTween = ((Variant) ref variant12).As<Tween>();
    Variant variant13;
    if (!info.TryGetProperty(NCombatUi.PropertyName._originalHandChildIndex, ref variant13))
      return;
    this._originalHandChildIndex = ((Variant) ref variant13).As<int>();
  }

  public class MethodName : Control.MethodName
  {
    public static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName _ExitTree = StringName.op_Implicit(nameof (_ExitTree));
    public static readonly StringName Deactivate = StringName.op_Implicit(nameof (Deactivate));
    public static readonly StringName DisconnectSignals = StringName.op_Implicit(nameof (DisconnectSignals));
    public static readonly StringName AddToPlayContainer = StringName.op_Implicit(nameof (AddToPlayContainer));
    public static readonly StringName AnimIn = StringName.op_Implicit(nameof (AnimIn));
    public static readonly StringName AnimOut = StringName.op_Implicit(nameof (AnimOut));
    public static readonly StringName PostCombatCleanUp = StringName.op_Implicit(nameof (PostCombatCleanUp));
    public static readonly StringName OnHandSelectModeEntered = StringName.op_Implicit(nameof (OnHandSelectModeEntered));
    public static readonly StringName OnHandSelectModeExited = StringName.op_Implicit(nameof (OnHandSelectModeExited));
    public static readonly StringName OnPeekButtonReady = StringName.op_Implicit(nameof (OnPeekButtonReady));
    public static readonly StringName OnPeekButtonToggled = StringName.op_Implicit(nameof (OnPeekButtonToggled));
    public static readonly StringName Enable = StringName.op_Implicit(nameof (Enable));
    public static readonly StringName Disable = StringName.op_Implicit(nameof (Disable));
    public static readonly StringName _Input = StringName.op_Implicit(nameof (_Input));
    public static readonly StringName DebugHideCombatUi = StringName.op_Implicit(nameof (DebugHideCombatUi));
  }

  public class PropertyName : Control.PropertyName
  {
    public static readonly StringName EnergyCounterContainer = StringName.op_Implicit(nameof (EnergyCounterContainer));
    public static readonly StringName EndTurnButton = StringName.op_Implicit(nameof (EndTurnButton));
    public static readonly StringName PingButton = StringName.op_Implicit(nameof (PingButton));
    public static readonly StringName DrawPile = StringName.op_Implicit(nameof (DrawPile));
    public static readonly StringName DiscardPile = StringName.op_Implicit(nameof (DiscardPile));
    public static readonly StringName ExhaustPile = StringName.op_Implicit(nameof (ExhaustPile));
    public static readonly StringName Hand = StringName.op_Implicit(nameof (Hand));
    public static readonly StringName PlayContainer = StringName.op_Implicit(nameof (PlayContainer));
    public static readonly StringName PlayQueue = StringName.op_Implicit(nameof (PlayQueue));
    public static readonly StringName CardPreviewContainer = StringName.op_Implicit(nameof (CardPreviewContainer));
    public static readonly StringName MessyCardPreviewContainer = StringName.op_Implicit(nameof (MessyCardPreviewContainer));
    public static readonly StringName _starCounter = StringName.op_Implicit(nameof (_starCounter));
    public static readonly StringName _energyCounter = StringName.op_Implicit(nameof (_energyCounter));
    public static readonly StringName _combatPilesContainer = StringName.op_Implicit(nameof (_combatPilesContainer));
    public static readonly StringName _playContainerPeekModeTween = StringName.op_Implicit(nameof (_playContainerPeekModeTween));
    public static readonly StringName _originalHandChildIndex = StringName.op_Implicit(nameof (_originalHandChildIndex));
  }

  public class SignalName : Control.SignalName
  {
  }
}
