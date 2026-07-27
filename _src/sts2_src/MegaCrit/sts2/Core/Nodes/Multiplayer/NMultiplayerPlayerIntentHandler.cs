// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Multiplayer.NMultiplayerPlayerIntentHandler
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Cards;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.HoverTips;
using MegaCrit.Sts2.Core.Nodes.Potions;
using MegaCrit.Sts2.Core.Nodes.Relics;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.Saves;
using MegaCrit.Sts2.Core.Settings;
using MegaCrit.Sts2.Core.TestSupport;
using System;
using System.Collections.Generic;
using System.ComponentModel;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Multiplayer;

[ScriptPath("res://src/Core/Nodes/Multiplayer/NMultiplayerPlayerIntentHandler.cs")]
public class NMultiplayerPlayerIntentHandler : Control
{
  private const string _scenePath = "combat/multiplayer_player_intent";
  private NMultiplayerCardIntent _cardIntent;
  private NRelic _relicIntent;
  private NPotion _potionIntent;
  private NPower _powerIntent;
  private Control _hitbox;
  private NRemoteTargetingIndicator _targetingIndicator;
  private MegaRichTextLabel _cardThinkyDots;
  private MegaRichTextLabel _relicThinkyDots;
  private MegaRichTextLabel _potionThinkyDots;
  private MegaRichTextLabel _powerThinkyDots;
  private bool _shouldShowHoverTip;
  private Player _player;
  private AbstractModel? _displayedModel;
  private NHoverTipSet? _hoverTips;
  private bool _isInPlayerChoice;
  private NCard? _cardInPlayAwaitingPlayerChoice;
  private Tween? _tween;

  public NMultiplayerCardIntent CardIntent => this._cardIntent;

  public static NMultiplayerPlayerIntentHandler? Create(Player player)
  {
    if (TestMode.IsOn)
      return (NMultiplayerPlayerIntentHandler) null;
    if (RunManager.Instance.IsSingleplayerOrFakeMultiplayer)
      return (NMultiplayerPlayerIntentHandler) null;
    NMultiplayerPlayerIntentHandler playerIntentHandler = PreloadManager.Cache.GetScene(SceneHelper.GetScenePath("combat/multiplayer_player_intent")).Instantiate<NMultiplayerPlayerIntentHandler>((PackedScene.GenEditState) 0L);
    playerIntentHandler._player = player;
    return playerIntentHandler;
  }

  public override void _Ready()
  {
    this._cardIntent = ((Node) this).GetNode<NMultiplayerCardIntent>(NodePath.op_Implicit("%CardIntent"));
    this._relicIntent = ((Node) this).GetNode<NRelic>(NodePath.op_Implicit("%RelicIntent"));
    this._potionIntent = ((Node) this).GetNode<NPotion>(NodePath.op_Implicit("%PotionIntent"));
    this._powerIntent = ((Node) this).GetNode<NPower>(NodePath.op_Implicit("%PowerIntent"));
    this._hitbox = ((Node) this).GetNode<Control>(NodePath.op_Implicit("%Hitbox"));
    this._targetingIndicator = ((Node) this).GetNode<NRemoteTargetingIndicator>(NodePath.op_Implicit("%TargetingIndicator"));
    this._cardThinkyDots = ((Node) this._cardIntent).GetNode<MegaRichTextLabel>(NodePath.op_Implicit("ThinkyDots"));
    this._relicThinkyDots = ((Node) this._relicIntent).GetNode<MegaRichTextLabel>(NodePath.op_Implicit("ThinkyDots"));
    this._potionThinkyDots = ((Node) this._potionIntent).GetNode<MegaRichTextLabel>(NodePath.op_Implicit("ThinkyDots"));
    this._powerThinkyDots = ((Node) this._powerIntent).GetNode<MegaRichTextLabel>(NodePath.op_Implicit("ThinkyDots"));
    this._targetingIndicator.Initialize(this._player);
    ((CanvasItem) this._cardIntent).Visible = false;
    ((CanvasItem) this._relicIntent).Visible = false;
    ((CanvasItem) this._potionIntent).Visible = false;
    ((CanvasItem) this._powerIntent).Visible = false;
    this.HideThinkyDots();
    RunManager.Instance.ActionQueueSet.ActionEnqueued += new Action<GameAction>(this.OnActionEnqueued);
    if (!LocalContext.IsMe(this._player))
    {
      ((GodotObject) this._hitbox).Connect(Control.SignalName.FocusEntered, Callable.From(new Action(this.OnHitboxEntered)), 0U);
      ((GodotObject) this._hitbox).Connect(Control.SignalName.FocusExited, Callable.From(new Action(this.OnHitboxExited)), 0U);
      ((GodotObject) this._hitbox).Connect(Control.SignalName.MouseEntered, Callable.From(new Action(this.OnHitboxEntered)), 0U);
      ((GodotObject) this._hitbox).Connect(Control.SignalName.MouseExited, Callable.From(new Action(this.OnHitboxExited)), 0U);
      RunManager.Instance.HoveredModelTracker.HoverChanged += new Action<ulong>(this.OnHoverChanged);
      RunManager.Instance.InputSynchronizer.StateChanged += new Action<ulong>(this.OnPeerInputStateChanged);
      RunManager.Instance.InputSynchronizer.StateRemoved += new Action<ulong>(this.OnPeerInputStateRemoved);
    }
    ((Node) this).ProcessMode = (Node.ProcessModeEnum) 4L;
  }

  public override void _ExitTree()
  {
    RunManager.Instance.ActionQueueSet.ActionEnqueued -= new Action<GameAction>(this.OnActionEnqueued);
    if (LocalContext.IsMe(this._player))
      return;
    RunManager.Instance.HoveredModelTracker.HoverChanged -= new Action<ulong>(this.OnHoverChanged);
    RunManager.Instance.InputSynchronizer.StateChanged -= new Action<ulong>(this.OnPeerInputStateChanged);
    RunManager.Instance.InputSynchronizer.StateRemoved -= new Action<ulong>(this.OnPeerInputStateRemoved);
  }

  private void OnHitboxEntered()
  {
    this._shouldShowHoverTip = true;
    this.RefreshHoverTips();
  }

  private void OnHitboxExited()
  {
    this._shouldShowHoverTip = false;
    this.RefreshHoverTips();
  }

  private void OnHoverChanged(ulong playerId)
  {
    if ((long) this._player.NetId != (long) playerId)
      return;
    this.RefreshHoverDisplay();
  }

  private void RefreshHoverDisplay()
  {
    if (this._isInPlayerChoice)
      return;
    this._tween?.Kill();
    this.HideThinkyDots();
    AbstractModel abstractModel = RunManager.Instance.HoveredModelTracker.GetHoveredModel(this._player.NetId);
    if (NCombatUi.IsDebugHideMpIntents)
      abstractModel = (AbstractModel) null;
    if (this._displayedModel == abstractModel)
      return;
    ((CanvasItem) this).Modulate = StsColors.halfTransparentWhite;
    ((CanvasItem) this._cardIntent).Visible = false;
    ((CanvasItem) this._relicIntent).Visible = false;
    ((CanvasItem) this._potionIntent).Visible = false;
    ((CanvasItem) this._powerIntent).Visible = false;
    ((CanvasItem) this._hitbox).Visible = abstractModel != null;
    switch (abstractModel)
    {
      case null:
        this.RefreshHoverTips();
        this._displayedModel = abstractModel;
        break;
      case CardModel cardModel:
        ((CanvasItem) this._cardIntent).Visible = true;
        this._cardIntent.Card = cardModel;
        this._hitbox.Position = this._cardIntent.Position;
        this._hitbox.Size = this._cardIntent.Size;
        goto case null;
      case PotionModel potionModel:
        ((CanvasItem) this._potionIntent).Visible = true;
        this._potionIntent.Model = potionModel;
        this._hitbox.Position = this._potionIntent.Position;
        this._hitbox.Size = this._potionIntent.Size;
        goto case null;
      case RelicModel relicModel:
        ((CanvasItem) this._relicIntent).Visible = true;
        this._relicIntent.Model = relicModel;
        this._hitbox.Position = this._relicIntent.Position;
        this._hitbox.Size = this._relicIntent.Size;
        goto case null;
      case PowerModel powerModel:
        ((CanvasItem) this._powerIntent).Visible = true;
        this._powerIntent.Model = powerModel;
        this._hitbox.Position = this._powerIntent.Position;
        this._hitbox.Size = this._powerIntent.Size;
        goto case null;
      default:
        throw new InvalidOperationException($"Player {this._player.NetId} hovering unsupported model {abstractModel}");
    }
  }

  private void OnPeerInputStateChanged(ulong playerId)
  {
    if ((long) playerId != (long) this._player.NetId)
      return;
    bool isTargeting = RunManager.Instance.InputSynchronizer.GetIsTargeting(this._player.NetId);
    if (isTargeting && !((CanvasItem) this._targetingIndicator).Visible)
    {
      this._targetingIndicator.StartDrawingFrom(Vector2.Zero);
      ((Node) this).ProcessMode = (Node.ProcessModeEnum) 0L;
    }
    else
    {
      if (isTargeting || !((CanvasItem) this._targetingIndicator).Visible)
        return;
      this._targetingIndicator.StopDrawing();
      ((Node) this).ProcessMode = (Node.ProcessModeEnum) 4L;
    }
  }

  private void OnPeerInputStateRemoved(ulong playerId)
  {
    if ((long) playerId != (long) this._player.NetId || !((CanvasItem) this._targetingIndicator).Visible)
      return;
    this._targetingIndicator.StopDrawing();
    ((Node) this).ProcessMode = (Node.ProcessModeEnum) 4L;
  }

  public override void _Process(double delta)
  {
    this._targetingIndicator.UpdateDrawingTo(Vector2.op_Subtraction(NGame.Instance.RemoteCursorContainer.GetCursorPosition(this._player.NetId), this._targetingIndicator.GlobalPosition));
  }

  private void OnActionEnqueued(GameAction action)
  {
    if ((long) action.OwnerId != (long) this._player.NetId)
      return;
    action.BeforeExecuted += new Action<GameAction>(this.BeforeActionExecuted);
  }

  private void BeforeActionExecuted(GameAction action)
  {
    action.BeforeExecuted -= new Action<GameAction>(this.BeforeActionExecuted);
    action.BeforePausedForPlayerChoice += new Action<GameAction>(this.BeforeActionPausedForPlayerChoice);
    action.BeforeReadyToResumeAfterPlayerChoice += new Action<GameAction>(this.BeforeActionReadyToResumeAfterPlayerChoice);
    action.AfterFinished += new Action<GameAction>(this.UnsubscribeFromAction);
    action.BeforeCancelled += new Action<GameAction>(this.UnsubscribeFromAction);
  }

  private void UnsubscribeFromAction(GameAction action)
  {
    action.BeforePausedForPlayerChoice -= new Action<GameAction>(this.BeforeActionPausedForPlayerChoice);
    action.BeforeReadyToResumeAfterPlayerChoice -= new Action<GameAction>(this.BeforeActionReadyToResumeAfterPlayerChoice);
    action.AfterFinished -= new Action<GameAction>(this.UnsubscribeFromAction);
    action.BeforeCancelled -= new Action<GameAction>(this.UnsubscribeFromAction);
  }

  private void BeforeActionPausedForPlayerChoice(GameAction action)
  {
    AbstractModel abstractModel = (AbstractModel) null;
    switch (action)
    {
      case PlayCardAction playCardAction:
        abstractModel = playCardAction.PlayerChoiceContext?.LastInvolvedModel;
        break;
      case UsePotionAction usePotionAction:
        abstractModel = usePotionAction.PlayerChoiceContext?.LastInvolvedModel;
        break;
      case GenericHookGameAction genericHookGameAction:
        abstractModel = genericHookGameAction.ChoiceContext?.LastInvolvedModel;
        break;
    }
    if (abstractModel == null)
      return;
    this._isInPlayerChoice = true;
    ((CanvasItem) this._cardIntent).Visible = false;
    ((CanvasItem) this._relicIntent).Visible = false;
    ((CanvasItem) this._potionIntent).Visible = false;
    ((CanvasItem) this._powerIntent).Visible = false;
    ((CanvasItem) this._hitbox).Visible = false;
    this._cardInPlayAwaitingPlayerChoice = (NCard) null;
    if (abstractModel is CardModel card)
    {
      NCard onTable = NCard.FindOnTable(card);
      ((CanvasItem) this._cardThinkyDots).Visible = true;
      ((Node) this._cardThinkyDots).ProcessMode = (Node.ProcessModeEnum) 3L;
      if (onTable != null)
      {
        Tween playPileTween = onTable.PlayPileTween;
        if (playPileTween != null)
          playPileTween.FastForwardToCompletion();
        Tween tween = ((Node) onTable).CreateTween();
        tween.Parallel().TweenProperty((GodotObject) onTable, NodePath.op_Implicit("position"), Variant.op_Implicit(Vector2.op_Addition(this._cardIntent.GlobalPosition, Vector2.op_Division(this._cardIntent.Size, 2f))), SaveManager.Instance.PrefsSave.FastMode == FastModeType.Fast ? 0.20000000298023224 : 0.30000001192092896);
        tween.Parallel().TweenProperty((GodotObject) onTable, NodePath.op_Implicit("scale"), Variant.op_Implicit(Vector2.op_Multiply(Vector2.One, 0.25f)), SaveManager.Instance.PrefsSave.FastMode == FastModeType.Fast ? 0.20000000298023224 : 0.30000001192092896);
        this._cardInPlayAwaitingPlayerChoice = onTable;
        ((Node) this._cardThinkyDots).Reparent(((Node) onTable).GetParent(), true);
        ((Node) this._hitbox).Reparent(((Node) onTable).GetParent(), true);
      }
      else
      {
        this._cardIntent.Card = card;
        ((CanvasItem) this._cardIntent).Visible = true;
      }
      ((CanvasItem) this._hitbox).Visible = true;
      this._hitbox.GlobalPosition = this._cardIntent.GlobalPosition;
      this._hitbox.Size = this._cardIntent.Size;
    }
    else if (abstractModel is RelicModel relicModel)
    {
      this._relicIntent.Model = relicModel;
      ((CanvasItem) this._relicIntent).Visible = true;
      ((CanvasItem) this._relicThinkyDots).Visible = true;
      ((Node) this._relicThinkyDots).ProcessMode = (Node.ProcessModeEnum) 3L;
      ((CanvasItem) this._hitbox).Visible = true;
      this._hitbox.Position = this._relicIntent.Position;
      this._hitbox.Size = this._relicIntent.Size;
    }
    else if (abstractModel is PotionModel potionModel)
    {
      this._potionIntent.Model = potionModel;
      ((CanvasItem) this._potionIntent).Visible = true;
      ((CanvasItem) this._potionThinkyDots).Visible = true;
      ((Node) this._potionThinkyDots).ProcessMode = (Node.ProcessModeEnum) 3L;
      ((CanvasItem) this._hitbox).Visible = true;
      this._hitbox.Position = this._potionIntent.Position;
      this._hitbox.Size = this._potionIntent.Size;
    }
    else if (abstractModel is PowerModel powerModel)
    {
      this._powerIntent.Model = powerModel;
      ((CanvasItem) this._powerIntent).Visible = true;
      ((CanvasItem) this._powerThinkyDots).Visible = true;
      ((Node) this._powerThinkyDots).ProcessMode = (Node.ProcessModeEnum) 3L;
      ((CanvasItem) this._hitbox).Visible = true;
      this._hitbox.Position = this._powerIntent.Position;
      this._hitbox.Size = this._powerIntent.Size;
    }
    this.RefreshHoverTips();
    ((CanvasItem) this).Modulate = StsColors.transparentWhite;
    this._tween?.Kill();
    this._tween = ((Node) this).GetTree().CreateTween();
    this._tween.TweenProperty((GodotObject) this, NodePath.op_Implicit("modulate"), Variant.op_Implicit(Colors.White), 0.25);
  }

  private void BeforeActionReadyToResumeAfterPlayerChoice(GameAction action)
  {
    this._tween?.Kill();
    this._tween = ((Node) this).GetTree().CreateTween();
    this._tween.TweenProperty((GodotObject) this, NodePath.op_Implicit("modulate"), Variant.op_Implicit(StsColors.transparentWhite), 0.15000000596046448);
    this._tween.TweenCallback(Callable.From(new Action(this.HideThinkyDots)));
    this._isInPlayerChoice = false;
    if (this._cardInPlayAwaitingPlayerChoice == null)
      return;
    ((Node) this._cardThinkyDots).Reparent((Node) this._cardIntent, true);
    ((Node) this._hitbox).Reparent((Node) this, true);
    NCardPlayQueue.Instance.ReAddCardAfterPlayerChoice(this._cardInPlayAwaitingPlayerChoice, action);
    this._cardInPlayAwaitingPlayerChoice = (NCard) null;
    this.RefreshHoverTips();
  }

  private void HideThinkyDots()
  {
    ((CanvasItem) this._cardThinkyDots).Visible = false;
    ((CanvasItem) this._relicThinkyDots).Visible = false;
    ((CanvasItem) this._potionThinkyDots).Visible = false;
    ((CanvasItem) this._powerThinkyDots).Visible = false;
    ((Node) this._cardThinkyDots).ProcessMode = (Node.ProcessModeEnum) 4L;
    ((Node) this._relicThinkyDots).ProcessMode = (Node.ProcessModeEnum) 4L;
    ((Node) this._potionThinkyDots).ProcessMode = (Node.ProcessModeEnum) 4L;
    ((Node) this._powerThinkyDots).ProcessMode = (Node.ProcessModeEnum) 4L;
  }

  private void RefreshHoverTips()
  {
    if (LocalContext.IsMe(this._player))
      return;
    if (NCombatUi.IsDebugHideTargetingUi)
      this._shouldShowHoverTip = false;
    else if (!((CanvasItem) this._hitbox).Visible)
      this._shouldShowHoverTip = false;
    NHoverTipSet.Remove((Control) this);
    if (!this._shouldShowHoverTip)
      return;
    List<IHoverTip> hoverTipList = new List<IHoverTip>();
    if (this._cardInPlayAwaitingPlayerChoice != null)
    {
      hoverTipList.Add(HoverTipFactory.FromCard(this._cardInPlayAwaitingPlayerChoice.Model));
      hoverTipList.AddRange(this._cardInPlayAwaitingPlayerChoice.Model.HoverTips);
    }
    else if (((CanvasItem) this._cardIntent).Visible)
    {
      hoverTipList.Add(HoverTipFactory.FromCard(this._cardIntent.Card));
      hoverTipList.AddRange(this._cardIntent.Card.HoverTips);
    }
    else if (((CanvasItem) this._relicIntent).Visible)
      hoverTipList.AddRange(this._relicIntent.Model.HoverTips);
    else if (((CanvasItem) this._potionIntent).Visible)
    {
      hoverTipList.Add(HoverTipFactory.FromPotion(this._potionIntent.Model));
      hoverTipList.AddRange(this._potionIntent.Model.HoverTips);
    }
    if (hoverTipList.Count <= 0)
      return;
    NHoverTipSet.CreateAndShow((Control) this, (IEnumerable<IHoverTip>) hoverTipList, HoverTipAlignment.Right);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(11)
    {
      new MethodInfo(NMultiplayerPlayerIntentHandler.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NMultiplayerPlayerIntentHandler.MethodName._ExitTree, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NMultiplayerPlayerIntentHandler.MethodName.OnHitboxEntered, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NMultiplayerPlayerIntentHandler.MethodName.OnHitboxExited, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NMultiplayerPlayerIntentHandler.MethodName.OnHoverChanged, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 2L, StringName.op_Implicit("playerId"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NMultiplayerPlayerIntentHandler.MethodName.RefreshHoverDisplay, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NMultiplayerPlayerIntentHandler.MethodName.OnPeerInputStateChanged, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 2L, StringName.op_Implicit("playerId"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NMultiplayerPlayerIntentHandler.MethodName.OnPeerInputStateRemoved, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 2L, StringName.op_Implicit("playerId"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NMultiplayerPlayerIntentHandler.MethodName._Process, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 3L, StringName.op_Implicit("delta"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NMultiplayerPlayerIntentHandler.MethodName.HideThinkyDots, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NMultiplayerPlayerIntentHandler.MethodName.RefreshHoverTips, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NMultiplayerPlayerIntentHandler.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NMultiplayerPlayerIntentHandler.MethodName._ExitTree) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._ExitTree();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NMultiplayerPlayerIntentHandler.MethodName.OnHitboxEntered) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnHitboxEntered();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NMultiplayerPlayerIntentHandler.MethodName.OnHitboxExited) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnHitboxExited();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NMultiplayerPlayerIntentHandler.MethodName.OnHoverChanged) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.OnHoverChanged(VariantUtils.ConvertTo<ulong>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NMultiplayerPlayerIntentHandler.MethodName.RefreshHoverDisplay) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.RefreshHoverDisplay();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NMultiplayerPlayerIntentHandler.MethodName.OnPeerInputStateChanged) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.OnPeerInputStateChanged(VariantUtils.ConvertTo<ulong>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NMultiplayerPlayerIntentHandler.MethodName.OnPeerInputStateRemoved) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.OnPeerInputStateRemoved(VariantUtils.ConvertTo<ulong>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NMultiplayerPlayerIntentHandler.MethodName._Process) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      ((Node) this)._Process(VariantUtils.ConvertTo<double>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NMultiplayerPlayerIntentHandler.MethodName.HideThinkyDots) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.HideThinkyDots();
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NMultiplayerPlayerIntentHandler.MethodName.RefreshHoverTips) || ((NativeVariantPtrArgs) ref args).Count != 0)
      return base.InvokeGodotClassMethod(ref method, args, ref ret);
    this.RefreshHoverTips();
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NMultiplayerPlayerIntentHandler.MethodName._Ready) || StringName.op_Equality(ref method, NMultiplayerPlayerIntentHandler.MethodName._ExitTree) || StringName.op_Equality(ref method, NMultiplayerPlayerIntentHandler.MethodName.OnHitboxEntered) || StringName.op_Equality(ref method, NMultiplayerPlayerIntentHandler.MethodName.OnHitboxExited) || StringName.op_Equality(ref method, NMultiplayerPlayerIntentHandler.MethodName.OnHoverChanged) || StringName.op_Equality(ref method, NMultiplayerPlayerIntentHandler.MethodName.RefreshHoverDisplay) || StringName.op_Equality(ref method, NMultiplayerPlayerIntentHandler.MethodName.OnPeerInputStateChanged) || StringName.op_Equality(ref method, NMultiplayerPlayerIntentHandler.MethodName.OnPeerInputStateRemoved) || StringName.op_Equality(ref method, NMultiplayerPlayerIntentHandler.MethodName._Process) || StringName.op_Equality(ref method, NMultiplayerPlayerIntentHandler.MethodName.HideThinkyDots) || StringName.op_Equality(ref method, NMultiplayerPlayerIntentHandler.MethodName.RefreshHoverTips) || base.HasGodotClassMethod(ref method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NMultiplayerPlayerIntentHandler.PropertyName._cardIntent))
    {
      this._cardIntent = VariantUtils.ConvertTo<NMultiplayerCardIntent>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NMultiplayerPlayerIntentHandler.PropertyName._relicIntent))
    {
      this._relicIntent = VariantUtils.ConvertTo<NRelic>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NMultiplayerPlayerIntentHandler.PropertyName._potionIntent))
    {
      this._potionIntent = VariantUtils.ConvertTo<NPotion>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NMultiplayerPlayerIntentHandler.PropertyName._powerIntent))
    {
      this._powerIntent = VariantUtils.ConvertTo<NPower>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NMultiplayerPlayerIntentHandler.PropertyName._hitbox))
    {
      this._hitbox = VariantUtils.ConvertTo<Control>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NMultiplayerPlayerIntentHandler.PropertyName._targetingIndicator))
    {
      this._targetingIndicator = VariantUtils.ConvertTo<NRemoteTargetingIndicator>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NMultiplayerPlayerIntentHandler.PropertyName._cardThinkyDots))
    {
      this._cardThinkyDots = VariantUtils.ConvertTo<MegaRichTextLabel>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NMultiplayerPlayerIntentHandler.PropertyName._relicThinkyDots))
    {
      this._relicThinkyDots = VariantUtils.ConvertTo<MegaRichTextLabel>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NMultiplayerPlayerIntentHandler.PropertyName._potionThinkyDots))
    {
      this._potionThinkyDots = VariantUtils.ConvertTo<MegaRichTextLabel>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NMultiplayerPlayerIntentHandler.PropertyName._powerThinkyDots))
    {
      this._powerThinkyDots = VariantUtils.ConvertTo<MegaRichTextLabel>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NMultiplayerPlayerIntentHandler.PropertyName._shouldShowHoverTip))
    {
      this._shouldShowHoverTip = VariantUtils.ConvertTo<bool>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NMultiplayerPlayerIntentHandler.PropertyName._hoverTips))
    {
      this._hoverTips = VariantUtils.ConvertTo<NHoverTipSet>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NMultiplayerPlayerIntentHandler.PropertyName._isInPlayerChoice))
    {
      this._isInPlayerChoice = VariantUtils.ConvertTo<bool>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NMultiplayerPlayerIntentHandler.PropertyName._cardInPlayAwaitingPlayerChoice))
    {
      this._cardInPlayAwaitingPlayerChoice = VariantUtils.ConvertTo<NCard>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NMultiplayerPlayerIntentHandler.PropertyName._tween))
      return ((GodotObject) this).SetGodotClassPropertyValue(ref name, ref value);
    this._tween = VariantUtils.ConvertTo<Tween>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NMultiplayerPlayerIntentHandler.PropertyName.CardIntent))
    {
      ref godot_variant local = ref value;
      NMultiplayerCardIntent cardIntent = this.CardIntent;
      godot_variant from = VariantUtils.CreateFrom<NMultiplayerCardIntent>(ref cardIntent);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NMultiplayerPlayerIntentHandler.PropertyName._cardIntent))
    {
      value = VariantUtils.CreateFrom<NMultiplayerCardIntent>(ref this._cardIntent);
      return true;
    }
    if (StringName.op_Equality(ref name, NMultiplayerPlayerIntentHandler.PropertyName._relicIntent))
    {
      value = VariantUtils.CreateFrom<NRelic>(ref this._relicIntent);
      return true;
    }
    if (StringName.op_Equality(ref name, NMultiplayerPlayerIntentHandler.PropertyName._potionIntent))
    {
      value = VariantUtils.CreateFrom<NPotion>(ref this._potionIntent);
      return true;
    }
    if (StringName.op_Equality(ref name, NMultiplayerPlayerIntentHandler.PropertyName._powerIntent))
    {
      value = VariantUtils.CreateFrom<NPower>(ref this._powerIntent);
      return true;
    }
    if (StringName.op_Equality(ref name, NMultiplayerPlayerIntentHandler.PropertyName._hitbox))
    {
      value = VariantUtils.CreateFrom<Control>(ref this._hitbox);
      return true;
    }
    if (StringName.op_Equality(ref name, NMultiplayerPlayerIntentHandler.PropertyName._targetingIndicator))
    {
      value = VariantUtils.CreateFrom<NRemoteTargetingIndicator>(ref this._targetingIndicator);
      return true;
    }
    if (StringName.op_Equality(ref name, NMultiplayerPlayerIntentHandler.PropertyName._cardThinkyDots))
    {
      value = VariantUtils.CreateFrom<MegaRichTextLabel>(ref this._cardThinkyDots);
      return true;
    }
    if (StringName.op_Equality(ref name, NMultiplayerPlayerIntentHandler.PropertyName._relicThinkyDots))
    {
      value = VariantUtils.CreateFrom<MegaRichTextLabel>(ref this._relicThinkyDots);
      return true;
    }
    if (StringName.op_Equality(ref name, NMultiplayerPlayerIntentHandler.PropertyName._potionThinkyDots))
    {
      value = VariantUtils.CreateFrom<MegaRichTextLabel>(ref this._potionThinkyDots);
      return true;
    }
    if (StringName.op_Equality(ref name, NMultiplayerPlayerIntentHandler.PropertyName._powerThinkyDots))
    {
      value = VariantUtils.CreateFrom<MegaRichTextLabel>(ref this._powerThinkyDots);
      return true;
    }
    if (StringName.op_Equality(ref name, NMultiplayerPlayerIntentHandler.PropertyName._shouldShowHoverTip))
    {
      value = VariantUtils.CreateFrom<bool>(ref this._shouldShowHoverTip);
      return true;
    }
    if (StringName.op_Equality(ref name, NMultiplayerPlayerIntentHandler.PropertyName._hoverTips))
    {
      value = VariantUtils.CreateFrom<NHoverTipSet>(ref this._hoverTips);
      return true;
    }
    if (StringName.op_Equality(ref name, NMultiplayerPlayerIntentHandler.PropertyName._isInPlayerChoice))
    {
      value = VariantUtils.CreateFrom<bool>(ref this._isInPlayerChoice);
      return true;
    }
    if (StringName.op_Equality(ref name, NMultiplayerPlayerIntentHandler.PropertyName._cardInPlayAwaitingPlayerChoice))
    {
      value = VariantUtils.CreateFrom<NCard>(ref this._cardInPlayAwaitingPlayerChoice);
      return true;
    }
    if (!StringName.op_Equality(ref name, NMultiplayerPlayerIntentHandler.PropertyName._tween))
      return ((GodotObject) this).GetGodotClassPropertyValue(ref name, ref value);
    value = VariantUtils.CreateFrom<Tween>(ref this._tween);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NMultiplayerPlayerIntentHandler.PropertyName._cardIntent, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NMultiplayerPlayerIntentHandler.PropertyName._relicIntent, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NMultiplayerPlayerIntentHandler.PropertyName._potionIntent, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NMultiplayerPlayerIntentHandler.PropertyName._powerIntent, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NMultiplayerPlayerIntentHandler.PropertyName._hitbox, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NMultiplayerPlayerIntentHandler.PropertyName._targetingIndicator, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NMultiplayerPlayerIntentHandler.PropertyName._cardThinkyDots, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NMultiplayerPlayerIntentHandler.PropertyName._relicThinkyDots, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NMultiplayerPlayerIntentHandler.PropertyName._potionThinkyDots, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NMultiplayerPlayerIntentHandler.PropertyName._powerThinkyDots, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 1L, NMultiplayerPlayerIntentHandler.PropertyName._shouldShowHoverTip, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NMultiplayerPlayerIntentHandler.PropertyName._hoverTips, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 1L, NMultiplayerPlayerIntentHandler.PropertyName._isInPlayerChoice, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NMultiplayerPlayerIntentHandler.PropertyName._cardInPlayAwaitingPlayerChoice, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NMultiplayerPlayerIntentHandler.PropertyName._tween, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NMultiplayerPlayerIntentHandler.PropertyName.CardIntent, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).SaveGodotObjectData(info);
    info.AddProperty(NMultiplayerPlayerIntentHandler.PropertyName._cardIntent, Variant.From<NMultiplayerCardIntent>(ref this._cardIntent));
    info.AddProperty(NMultiplayerPlayerIntentHandler.PropertyName._relicIntent, Variant.From<NRelic>(ref this._relicIntent));
    info.AddProperty(NMultiplayerPlayerIntentHandler.PropertyName._potionIntent, Variant.From<NPotion>(ref this._potionIntent));
    info.AddProperty(NMultiplayerPlayerIntentHandler.PropertyName._powerIntent, Variant.From<NPower>(ref this._powerIntent));
    info.AddProperty(NMultiplayerPlayerIntentHandler.PropertyName._hitbox, Variant.From<Control>(ref this._hitbox));
    info.AddProperty(NMultiplayerPlayerIntentHandler.PropertyName._targetingIndicator, Variant.From<NRemoteTargetingIndicator>(ref this._targetingIndicator));
    info.AddProperty(NMultiplayerPlayerIntentHandler.PropertyName._cardThinkyDots, Variant.From<MegaRichTextLabel>(ref this._cardThinkyDots));
    info.AddProperty(NMultiplayerPlayerIntentHandler.PropertyName._relicThinkyDots, Variant.From<MegaRichTextLabel>(ref this._relicThinkyDots));
    info.AddProperty(NMultiplayerPlayerIntentHandler.PropertyName._potionThinkyDots, Variant.From<MegaRichTextLabel>(ref this._potionThinkyDots));
    info.AddProperty(NMultiplayerPlayerIntentHandler.PropertyName._powerThinkyDots, Variant.From<MegaRichTextLabel>(ref this._powerThinkyDots));
    info.AddProperty(NMultiplayerPlayerIntentHandler.PropertyName._shouldShowHoverTip, Variant.From<bool>(ref this._shouldShowHoverTip));
    info.AddProperty(NMultiplayerPlayerIntentHandler.PropertyName._hoverTips, Variant.From<NHoverTipSet>(ref this._hoverTips));
    info.AddProperty(NMultiplayerPlayerIntentHandler.PropertyName._isInPlayerChoice, Variant.From<bool>(ref this._isInPlayerChoice));
    info.AddProperty(NMultiplayerPlayerIntentHandler.PropertyName._cardInPlayAwaitingPlayerChoice, Variant.From<NCard>(ref this._cardInPlayAwaitingPlayerChoice));
    info.AddProperty(NMultiplayerPlayerIntentHandler.PropertyName._tween, Variant.From<Tween>(ref this._tween));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NMultiplayerPlayerIntentHandler.PropertyName._cardIntent, ref variant1))
      this._cardIntent = ((Variant) ref variant1).As<NMultiplayerCardIntent>();
    Variant variant2;
    if (info.TryGetProperty(NMultiplayerPlayerIntentHandler.PropertyName._relicIntent, ref variant2))
      this._relicIntent = ((Variant) ref variant2).As<NRelic>();
    Variant variant3;
    if (info.TryGetProperty(NMultiplayerPlayerIntentHandler.PropertyName._potionIntent, ref variant3))
      this._potionIntent = ((Variant) ref variant3).As<NPotion>();
    Variant variant4;
    if (info.TryGetProperty(NMultiplayerPlayerIntentHandler.PropertyName._powerIntent, ref variant4))
      this._powerIntent = ((Variant) ref variant4).As<NPower>();
    Variant variant5;
    if (info.TryGetProperty(NMultiplayerPlayerIntentHandler.PropertyName._hitbox, ref variant5))
      this._hitbox = ((Variant) ref variant5).As<Control>();
    Variant variant6;
    if (info.TryGetProperty(NMultiplayerPlayerIntentHandler.PropertyName._targetingIndicator, ref variant6))
      this._targetingIndicator = ((Variant) ref variant6).As<NRemoteTargetingIndicator>();
    Variant variant7;
    if (info.TryGetProperty(NMultiplayerPlayerIntentHandler.PropertyName._cardThinkyDots, ref variant7))
      this._cardThinkyDots = ((Variant) ref variant7).As<MegaRichTextLabel>();
    Variant variant8;
    if (info.TryGetProperty(NMultiplayerPlayerIntentHandler.PropertyName._relicThinkyDots, ref variant8))
      this._relicThinkyDots = ((Variant) ref variant8).As<MegaRichTextLabel>();
    Variant variant9;
    if (info.TryGetProperty(NMultiplayerPlayerIntentHandler.PropertyName._potionThinkyDots, ref variant9))
      this._potionThinkyDots = ((Variant) ref variant9).As<MegaRichTextLabel>();
    Variant variant10;
    if (info.TryGetProperty(NMultiplayerPlayerIntentHandler.PropertyName._powerThinkyDots, ref variant10))
      this._powerThinkyDots = ((Variant) ref variant10).As<MegaRichTextLabel>();
    Variant variant11;
    if (info.TryGetProperty(NMultiplayerPlayerIntentHandler.PropertyName._shouldShowHoverTip, ref variant11))
      this._shouldShowHoverTip = ((Variant) ref variant11).As<bool>();
    Variant variant12;
    if (info.TryGetProperty(NMultiplayerPlayerIntentHandler.PropertyName._hoverTips, ref variant12))
      this._hoverTips = ((Variant) ref variant12).As<NHoverTipSet>();
    Variant variant13;
    if (info.TryGetProperty(NMultiplayerPlayerIntentHandler.PropertyName._isInPlayerChoice, ref variant13))
      this._isInPlayerChoice = ((Variant) ref variant13).As<bool>();
    Variant variant14;
    if (info.TryGetProperty(NMultiplayerPlayerIntentHandler.PropertyName._cardInPlayAwaitingPlayerChoice, ref variant14))
      this._cardInPlayAwaitingPlayerChoice = ((Variant) ref variant14).As<NCard>();
    Variant variant15;
    if (!info.TryGetProperty(NMultiplayerPlayerIntentHandler.PropertyName._tween, ref variant15))
      return;
    this._tween = ((Variant) ref variant15).As<Tween>();
  }

  public class MethodName : Control.MethodName
  {
    public static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName _ExitTree = StringName.op_Implicit(nameof (_ExitTree));
    public static readonly StringName OnHitboxEntered = StringName.op_Implicit(nameof (OnHitboxEntered));
    public static readonly StringName OnHitboxExited = StringName.op_Implicit(nameof (OnHitboxExited));
    public static readonly StringName OnHoverChanged = StringName.op_Implicit(nameof (OnHoverChanged));
    public static readonly StringName RefreshHoverDisplay = StringName.op_Implicit(nameof (RefreshHoverDisplay));
    public static readonly StringName OnPeerInputStateChanged = StringName.op_Implicit(nameof (OnPeerInputStateChanged));
    public static readonly StringName OnPeerInputStateRemoved = StringName.op_Implicit(nameof (OnPeerInputStateRemoved));
    public static readonly StringName _Process = StringName.op_Implicit(nameof (_Process));
    public static readonly StringName HideThinkyDots = StringName.op_Implicit(nameof (HideThinkyDots));
    public static readonly StringName RefreshHoverTips = StringName.op_Implicit(nameof (RefreshHoverTips));
  }

  public class PropertyName : Control.PropertyName
  {
    public static readonly StringName CardIntent = StringName.op_Implicit(nameof (CardIntent));
    public static readonly StringName _cardIntent = StringName.op_Implicit(nameof (_cardIntent));
    public static readonly StringName _relicIntent = StringName.op_Implicit(nameof (_relicIntent));
    public static readonly StringName _potionIntent = StringName.op_Implicit(nameof (_potionIntent));
    public static readonly StringName _powerIntent = StringName.op_Implicit(nameof (_powerIntent));
    public static readonly StringName _hitbox = StringName.op_Implicit(nameof (_hitbox));
    public static readonly StringName _targetingIndicator = StringName.op_Implicit(nameof (_targetingIndicator));
    public static readonly StringName _cardThinkyDots = StringName.op_Implicit(nameof (_cardThinkyDots));
    public static readonly StringName _relicThinkyDots = StringName.op_Implicit(nameof (_relicThinkyDots));
    public static readonly StringName _potionThinkyDots = StringName.op_Implicit(nameof (_potionThinkyDots));
    public static readonly StringName _powerThinkyDots = StringName.op_Implicit(nameof (_powerThinkyDots));
    public static readonly StringName _shouldShowHoverTip = StringName.op_Implicit(nameof (_shouldShowHoverTip));
    public static readonly StringName _hoverTips = StringName.op_Implicit(nameof (_hoverTips));
    public static readonly StringName _isInPlayerChoice = StringName.op_Implicit(nameof (_isInPlayerChoice));
    public static readonly StringName _cardInPlayAwaitingPlayerChoice = StringName.op_Implicit(nameof (_cardInPlayAwaitingPlayerChoice));
    public static readonly StringName _tween = StringName.op_Implicit(nameof (_tween));
  }

  public class SignalName : Control.SignalName
  {
  }
}
