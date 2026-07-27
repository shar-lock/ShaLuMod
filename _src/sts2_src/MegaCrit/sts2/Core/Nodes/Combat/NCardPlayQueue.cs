// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Combat.NCardPlayQueue
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.Entities.Actions;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.Exceptions;
using MegaCrit.Sts2.Core.Nodes.Cards;
using MegaCrit.Sts2.Core.Nodes.Cards.Holders;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.Multiplayer;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Runs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Combat;

[ScriptPath("res://src/Core/Nodes/Combat/NCardPlayQueue.cs")]
public class NCardPlayQueue : Control
{
  private List<NCardPlayQueue.QueueItem> _playQueue = new List<NCardPlayQueue.QueueItem>();

  public static NCardPlayQueue? Instance => NCombatRoom.Instance?.Ui.PlayQueue;

  public override void _Ready()
  {
    RunManager.Instance.ActionQueueSet.ActionEnqueued += new Action<GameAction>(this.OnActionEnqueued);
  }

  public override void _ExitTree()
  {
    RunManager.Instance.ActionQueueSet.ActionEnqueued -= new Action<GameAction>(this.OnActionEnqueued);
    this._playQueue.Clear();
  }

  public void OnLocalCardPlayed(PlayCardAction action, NCardHolder? holder, CardModel card)
  {
    NCard child = holder?.CardNode ?? NCard.Create(card);
    CardModel model = child.Model;
    if ((model != null ? (model.Pile?.Type.GetValueOrDefault() != PileType.Hand ? 1 : 0) : 1) != 0)
      return;
    NCardPlayQueue.QueueItem queueItem = new NCardPlayQueue.QueueItem()
    {
      card = child,
      action = (GameAction) action
    };
    if (((Node) child).IsInsideTree())
      ((Node) child).Reparent((Node) this, true);
    else
      ((Node) this).AddChildSafely((Node) child);
    ((Node) this).MoveChildSafely((Node) child, 0);
    if (holder != null && ((Node) holder).IsValid())
      NPlayerHand.Instance.RemoveCardHolder(holder);
    this._playQueue.Add(queueItem);
    this.TweenCardToQueuePosition(queueItem, this._playQueue.Count - 1);
  }

  private void OnActionEnqueued(GameAction action)
  {
    if (!(action is PlayCardAction action1))
      return;
    CardModel card = action1.NetCombatCard.ToCardModelOrNull();
    if (card == null)
    {
      try
      {
        card = ModelDb.GetById<CardModel>(action1.CardModelId);
      }
      catch (ModelNotFoundException ex)
      {
        card = (CardModel) ModelDb.Card<DeprecatedCard>();
      }
    }
    if (LocalContext.IsMe(action1.Player))
    {
      NCardHolder cardHolder = NPlayerHand.Instance.GetCardHolder(card);
      this.OnLocalCardPlayed(action1, cardHolder, card);
    }
    else
    {
      NMultiplayerPlayerIntentHandler playerIntentHandler = NCombatRoom.Instance.GetCreatureNode(action1.Player.Creature).PlayerIntentHandler;
      NCard child = NCard.Create(card);
      Vector2 vector2 = Vector2.op_Addition(playerIntentHandler.CardIntent.GlobalPosition, Vector2.op_Multiply(playerIntentHandler.CardIntent.Size, 0.5f));
      child.GlobalPosition = vector2;
      child.Scale = Vector2.op_Multiply(Vector2.One, 0.25f);
      ((Node) this).AddChildSafely((Node) child);
      ((Node) this).MoveChildSafely((Node) child, 0);
      NCardPlayQueue.QueueItem queueItem = new NCardPlayQueue.QueueItem()
      {
        card = child,
        action = (GameAction) action1
      };
      this._playQueue.Add(queueItem);
      this.UpdateCardVisuals(queueItem);
      this.TweenCardToQueuePosition(queueItem, this._playQueue.Count - 1);
    }
  }

  public void ReAddCardAfterPlayerChoice(NCard card, GameAction action)
  {
    if (action.State == GameActionState.Executing)
    {
      ((Node) card).Reparent((Node) NCombatRoom.Instance.Ui.PlayContainer, true);
      card.AnimCardToPlayPile();
    }
    else
    {
      NCardPlayQueue.QueueItem queueItem = new NCardPlayQueue.QueueItem()
      {
        card = card,
        action = action
      };
      ((Node) card).Reparent((Node) this, true);
      ((Node) this).MoveChildSafely((Node) card, 0);
      this._playQueue.Add(queueItem);
      this.TweenCardToQueuePosition(queueItem, this._playQueue.Count - 1);
      action.BeforeResumedAfterPlayerChoice += new Action<GameAction>(this.BeforeRemoteCardPlayResumedAfterPlayerChoice);
    }
  }

  private void BeforeRemoteCardPlayResumedAfterPlayerChoice(GameAction action)
  {
    action.BeforeResumedAfterPlayerChoice -= new Action<GameAction>(this.BeforeRemoteCardPlayResumedAfterPlayerChoice);
    int index = this._playQueue.FindIndex((Predicate<NCardPlayQueue.QueueItem>) (i => i.action == action));
    if (index < 0)
      return;
    NCardPlayQueue.QueueItem play = this._playQueue[index];
    this.RemoveCardFromQueue(index);
    ((Node) play.card).Reparent((Node) NCombatRoom.Instance.Ui.PlayContainer, true);
    play.card.AnimCardToPlayPile();
  }

  public void RemoveCardFromQueueForCancellation(PlayCardAction action)
  {
    int index = this._playQueue.FindIndex((Predicate<NCardPlayQueue.QueueItem>) (i => i.action == action));
    if (index < 0)
      return;
    this.RemoveCardFromQueueForCancellation(index);
  }

  public void RemoveCardFromQueueForCancellation(NCard card, bool forceReturnToHand = false)
  {
    int index = this._playQueue.FindIndex((Predicate<NCardPlayQueue.QueueItem>) (i => i.card == card));
    if (index < 0)
      return;
    this.RemoveCardFromQueueForCancellation(index, forceReturnToHand);
  }

  private void RemoveCardFromQueueForCancellation(int index, bool forceReturnToHand = false)
  {
    NCardPlayQueue.QueueItem play = this._playQueue[index];
    this.RemoveCardFromQueue(index);
    long ownerId = (long) play.action.OwnerId;
    ulong? netId = LocalContext.NetId;
    long valueOrDefault = (long) netId.GetValueOrDefault();
    if (ownerId == valueOrDefault & netId.HasValue)
    {
      CardModel model = play.card.Model;
      if (((model != null ? (model.Pile?.Type.GetValueOrDefault() == PileType.Hand ? 1 : 0) : 0) | (forceReturnToHand ? 1 : 0)) != 0)
        NPlayerHand.Instance.Add(play.card);
      else
        this.TweenCardForCancellation(play);
    }
    else
      this.TweenCardForCancellation(play);
  }

  public void UpdateCardBeforeExecution(PlayCardAction playCardAction)
  {
    int index = this._playQueue.FindIndex((Predicate<NCardPlayQueue.QueueItem>) (i => i.action == playCardAction));
    if (index < 0)
      return;
    NCardPlayQueue.QueueItem play = this._playQueue[index];
    play.card.Model = playCardAction.NetCombatCard.ToCardModel();
    this.UpdateCardVisuals(play);
    if (!LocalContext.IsMe(play.card.Model.Owner) || NPlayerHand.Instance?.GetCardHolder(play.card.Model) == null)
      return;
    NPlayerHand.Instance?.Remove(play.card.Model);
  }

  public void RemoveCardFromQueueForExecution(CardModel card)
  {
    int index = this._playQueue.FindIndex((Predicate<NCardPlayQueue.QueueItem>) (i => i.card.Model == card));
    if (index < 0)
      throw new InvalidOperationException();
    this.RemoveCardFromQueue(index);
  }

  private void UpdateCardVisuals(NCardPlayQueue.QueueItem item)
  {
    if (item.action is PlayCardAction action)
      item.card.SetPreviewTarget(action.Target);
    NCard card = item.card;
    CardPile pile = item.card.Model.Pile;
    int type = pile != null ? (int) pile.Type : 0;
    card.UpdateVisuals((PileType) type, CardPreviewMode.Normal);
  }

  private void RemoveCardFromQueue(NCard card)
  {
    int index = this._playQueue.FindIndex((Predicate<NCardPlayQueue.QueueItem>) (i => i.card == card));
    if (index < 0)
      return;
    this.RemoveCardFromQueue(index);
  }

  private void RemoveCardFromQueue(int index)
  {
    this._playQueue[index].currentTween?.Kill();
    this._playQueue.RemoveAt(index);
    this.TweenAllToQueuePosition();
  }

  private void TweenAllToQueuePosition()
  {
    for (int index = 0; index < this._playQueue.Count; ++index)
      this.TweenCardToQueuePosition(this._playQueue[index], index);
  }

  public NCard? GetCardNode(CardModel card)
  {
    return this._playQueue.FirstOrDefault<NCardPlayQueue.QueueItem>((Func<NCardPlayQueue.QueueItem, bool>) (i => i.card.Model == card))?.card;
  }

  public void AnimOut()
  {
    foreach (NCardPlayQueue.QueueItem play in this._playQueue)
    {
      play.currentTween?.Kill();
      long ownerId = (long) play.action.OwnerId;
      ulong? netId = LocalContext.NetId;
      long valueOrDefault = (long) netId.GetValueOrDefault();
      if (ownerId == valueOrDefault & netId.HasValue)
      {
        CardModel model = play.card.Model;
        if ((model != null ? (model.Pile?.Type.GetValueOrDefault() == PileType.Hand ? 1 : 0) : 0) != 0)
        {
          NPlayerHand.Instance.Add(play.card);
          continue;
        }
      }
      this.TweenCardForCancellation(play);
    }
    this._playQueue.Clear();
  }

  private void TweenCardForCancellation(NCardPlayQueue.QueueItem item)
  {
    item.currentTween?.Kill();
    item.currentTween = ((Node) this).CreateTween().SetParallel(true);
    item.currentTween.TweenProperty((GodotObject) item.card, NodePath.op_Implicit("position:y"), Variant.op_Implicit(30f), 0.5).AsRelative().SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 7L);
    item.currentTween.TweenProperty((GodotObject) item.card, NodePath.op_Implicit("modulate:a"), Variant.op_Implicit(0.0f), 0.5).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 7L);
    item.currentTween.Chain().TweenCallback(Callable.From(new Action(((GodotTreeExtensions) item.card).QueueFreeSafely)));
    item.currentTween.Play();
  }

  private void TweenCardToQueuePosition(NCardPlayQueue.QueueItem item, int queueIndex)
  {
    item.currentTween?.Kill();
    item.currentTween = ((Node) this).CreateTween().SetParallel(true);
    item.currentTween.TweenProperty((GodotObject) item.card, NodePath.op_Implicit("position"), Variant.op_Implicit(this.GetPositionForQueueIndex(item.card, queueIndex)), 0.34999999403953552).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 7L);
    item.currentTween.TweenProperty((GodotObject) item.card, NodePath.op_Implicit("scale"), Variant.op_Implicit(this.GetScaleForQueueIndex(queueIndex)), 0.34999999403953552).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 7L);
    item.currentTween.TweenProperty((GodotObject) item.card, NodePath.op_Implicit("modulate:a"), Variant.op_Implicit(1f), 0.34999999403953552).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 7L);
    item.currentTween.Play();
  }

  private Vector2 GetScaleForQueueIndex(int index)
  {
    ++index;
    return Vector2.op_Multiply(Vector2.op_Multiply((float) (1.0 - (double) index / (double) (index + 1)), Vector2.One), 0.8f);
  }

  private Vector2 GetPositionForQueueIndex(NCard card, int index)
  {
    ++index;
    float num = (float) index / (float) (index + 2);
    return Vector2.op_Addition(PileType.Play.GetTargetPosition(card), Vector2.op_Multiply(Vector2.op_Multiply(Vector2.Left, 300f), num));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(8)
    {
      new MethodInfo(NCardPlayQueue.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCardPlayQueue.MethodName._ExitTree, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCardPlayQueue.MethodName.RemoveCardFromQueueForCancellation, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("card"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false),
        new PropertyInfo((Variant.Type) 1L, StringName.op_Implicit("forceReturnToHand"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NCardPlayQueue.MethodName.RemoveCardFromQueue, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("card"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false)
      }, (List<Variant>) null),
      new MethodInfo(NCardPlayQueue.MethodName.TweenAllToQueuePosition, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCardPlayQueue.MethodName.AnimOut, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCardPlayQueue.MethodName.GetScaleForQueueIndex, new PropertyInfo((Variant.Type) 5L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 2L, StringName.op_Implicit("index"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NCardPlayQueue.MethodName.GetPositionForQueueIndex, new PropertyInfo((Variant.Type) 5L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("card"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false),
        new PropertyInfo((Variant.Type) 2L, StringName.op_Implicit("index"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NCardPlayQueue.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCardPlayQueue.MethodName._ExitTree) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._ExitTree();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCardPlayQueue.MethodName.RemoveCardFromQueueForCancellation) && ((NativeVariantPtrArgs) ref args).Count == 2)
    {
      this.RemoveCardFromQueueForCancellation(VariantUtils.ConvertTo<NCard>(ref ((NativeVariantPtrArgs) ref args)[0]), VariantUtils.ConvertTo<bool>(ref ((NativeVariantPtrArgs) ref args)[1]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCardPlayQueue.MethodName.RemoveCardFromQueue) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.RemoveCardFromQueue(VariantUtils.ConvertTo<NCard>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCardPlayQueue.MethodName.TweenAllToQueuePosition) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.TweenAllToQueuePosition();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCardPlayQueue.MethodName.AnimOut) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.AnimOut();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCardPlayQueue.MethodName.GetScaleForQueueIndex) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      Vector2 scaleForQueueIndex = this.GetScaleForQueueIndex(VariantUtils.ConvertTo<int>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = VariantUtils.CreateFrom<Vector2>(ref scaleForQueueIndex);
      return true;
    }
    if (!StringName.op_Equality(ref method, NCardPlayQueue.MethodName.GetPositionForQueueIndex) || ((NativeVariantPtrArgs) ref args).Count != 2)
      return base.InvokeGodotClassMethod(ref method, args, ref ret);
    Vector2 positionForQueueIndex = this.GetPositionForQueueIndex(VariantUtils.ConvertTo<NCard>(ref ((NativeVariantPtrArgs) ref args)[0]), VariantUtils.ConvertTo<int>(ref ((NativeVariantPtrArgs) ref args)[1]));
    ret = VariantUtils.CreateFrom<Vector2>(ref positionForQueueIndex);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NCardPlayQueue.MethodName._Ready) || StringName.op_Equality(ref method, NCardPlayQueue.MethodName._ExitTree) || StringName.op_Equality(ref method, NCardPlayQueue.MethodName.RemoveCardFromQueueForCancellation) || StringName.op_Equality(ref method, NCardPlayQueue.MethodName.RemoveCardFromQueue) || StringName.op_Equality(ref method, NCardPlayQueue.MethodName.TweenAllToQueuePosition) || StringName.op_Equality(ref method, NCardPlayQueue.MethodName.AnimOut) || StringName.op_Equality(ref method, NCardPlayQueue.MethodName.GetScaleForQueueIndex) || StringName.op_Equality(ref method, NCardPlayQueue.MethodName.GetPositionForQueueIndex) || base.HasGodotClassMethod(ref method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).SaveGodotObjectData(info);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).RestoreGodotObjectData(info);
  }

  private class QueueItem
  {
    public required 
    #nullable enable
    NCard card;
    public required GameAction action;
    public Tween? currentTween;
  }

  public class MethodName : Control.MethodName
  {
    public static readonly 
    #nullable disable
    StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName _ExitTree = StringName.op_Implicit(nameof (_ExitTree));
    public static readonly StringName RemoveCardFromQueueForCancellation = StringName.op_Implicit(nameof (RemoveCardFromQueueForCancellation));
    public static readonly StringName RemoveCardFromQueue = StringName.op_Implicit(nameof (RemoveCardFromQueue));
    public static readonly StringName TweenAllToQueuePosition = StringName.op_Implicit(nameof (TweenAllToQueuePosition));
    public static readonly StringName AnimOut = StringName.op_Implicit(nameof (AnimOut));
    public static readonly StringName GetScaleForQueueIndex = StringName.op_Implicit(nameof (GetScaleForQueueIndex));
    public static readonly StringName GetPositionForQueueIndex = StringName.op_Implicit(nameof (GetPositionForQueueIndex));
  }

  public class PropertyName : Control.PropertyName
  {
  }

  public class SignalName : Control.SignalName
  {
  }
}
