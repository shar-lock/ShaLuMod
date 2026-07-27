// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Combat.NCardPlay
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Cards;
using MegaCrit.Sts2.Core.Nodes.Cards.Holders;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Nodes.Ftue;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Screens.ScreenContext;
using MegaCrit.Sts2.Core.Saves;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Combat;

[ScriptPath("res://src/Core/Nodes/Combat/NCardPlay.cs")]
public abstract class NCardPlay : Node
{
  private static int _totalCardsPlayedForFtue;
  private const int _numCardPlayedUntilDisableFtue = 8;
  protected Viewport _viewport;
  private bool _isTryingToPlayCard;
  private 
  #nullable disable
  NCardPlay.FinishedEventHandler backing_Finished;

  public 
  #nullable enable
  NHandCardHolder Holder { get; protected set; }

  protected NCard? CardNode => this.Holder.CardNode;

  protected CardModel? Card => this.CardNode?.Model;

  protected NCreature? CardOwnerNode
  {
    get => NCombatRoom.Instance.GetCreatureNode(this.Card?.Owner.Creature);
  }

  public Player Player { get; protected set; }

  public override void _Ready() => this._viewport = this.GetViewport();

  public abstract void Start();

  protected void TryPlayCard(Creature? target)
  {
    if (this.Card == null)
      return;
    if (this.Card.TargetType == TargetType.AnyEnemy && target == null)
      this.CancelPlayCard();
    else if (this.Card.TargetType == TargetType.AnyAlly && target == null)
      this.CancelPlayCard();
    else if (!this.Holder.CardModel.CanPlayTargeting(target))
    {
      this.CannotPlayThisCardFtueCheck(this.Holder.CardModel);
      this.CancelPlayCard();
    }
    else
    {
      CardModel card = this.Card;
      this._isTryingToPlayCard = true;
      bool flag1;
      switch (card.TargetType)
      {
        case TargetType.AnyEnemy:
        case TargetType.AnyAlly:
          flag1 = true;
          break;
        default:
          flag1 = false;
          break;
      }
      bool flag2;
      if (flag1)
      {
        if (NCombatRoom.Instance != null)
          NCombatRoom.Instance.LastTargetedCreature = target;
        flag2 = card.TryManualPlay(target);
      }
      else
      {
        bool flag3 = NCombatRoom.Instance != null;
        if (flag3)
        {
          bool flag4;
          switch (card.TargetType)
          {
            case TargetType.AllEnemies:
            case TargetType.AllAllies:
              flag4 = true;
              break;
            default:
              flag4 = false;
              break;
          }
          flag3 = flag4;
        }
        if (flag3)
          NCombatRoom.Instance.LastTargetedCreature = (Creature) null;
        flag2 = card.TryManualPlay((Creature) null);
      }
      this._isTryingToPlayCard = false;
      if (flag2)
      {
        this.AutoDisableCannotPlayCardFtueCheck();
        bool flag5;
        switch (card.TargetType)
        {
          case TargetType.AnyEnemy:
          case TargetType.AnyAlly:
            flag5 = true;
            break;
          default:
            flag5 = false;
            break;
        }
        if (flag5 && ((Node) this.Holder).IsInsideTree())
        {
          Rect2 visibleRect = this.GetViewport().GetVisibleRect();
          Vector2 size = ((Rect2) ref visibleRect).Size;
          this.Holder.SetTargetPosition(new Vector2(size.X / 2f, size.Y - this.Holder.Size.Y));
        }
        this.Cleanup(true);
        ActiveScreenContext.Instance.FocusOnDefaultControl();
      }
      else
        this.CancelPlayCard();
    }
  }

  public void CancelPlayCard()
  {
    if (this._isTryingToPlayCard)
      return;
    this.ClearTarget();
    this.Cleanup(false);
    this.OnCancelPlayCard();
  }

  protected virtual void OnCancelPlayCard()
  {
  }

  protected void Cleanup(bool isFinished)
  {
    this.HideTargetingVisuals();
    this.HideEvokingOrbs();
    if (!GodotObject.IsInstanceValid((GodotObject) this))
      return;
    ((GodotObject) this).EmitSignal(NCardPlay.SignalName.Finished, new Variant[1]
    {
      Variant.op_Implicit(isFinished)
    });
    this.QueueFreeSafely();
  }

  protected void OnCreatureHover(NCreature creature)
  {
    this.CardNode?.SetPreviewTarget(creature.Entity);
  }

  protected void OnCreatureUnhover(NCreature _) => this.ClearTarget();

  protected void CenterCard()
  {
    Rect2 visibleRect = this._viewport.GetVisibleRect();
    Vector2 size = ((Rect2) ref visibleRect).Size;
    this.Holder.SetTargetPosition(new Vector2(size.X / 2f, size.Y - (float) ((double) this.Holder.Hitbox.Size.Y * 0.75 / 2.0)));
    this.Holder.SetTargetScale(Vector2.op_Multiply(Vector2.One, 0.75f));
  }

  protected void CannotPlayThisCardFtueCheck(CardModel card)
  {
    UnplayableReason reason;
    if (SaveManager.Instance.SeenFtue("cannot_play_card_ftue") || card.CanPlay(out reason, out AbstractModel _) || reason != UnplayableReason.EnergyCostTooHigh)
      return;
    NModalContainer.Instance.Add((Node) NCannotPlayCardFtue.Create());
    SaveManager.Instance.MarkFtueAsComplete("cannot_play_card_ftue");
  }

  protected void HideTargetingVisuals()
  {
    foreach (NCreature creatureNode in NCombatRoom.Instance.CreatureNodes)
      creatureNode.HideMultiselectReticle();
    this.CardNode?.SetPreviewTarget((Creature) null);
    this.CardNode?.UpdateVisuals(this.Card?.Pile?.Type.GetValueOrDefault(), CardPreviewMode.Normal);
  }

  protected void ShowMultiCreatureTargetingVisuals()
  {
    if (this.CardNode == null || this.Card?.CombatState == null)
      return;
    bool flag;
    switch (this.Card.TargetType)
    {
      case TargetType.AllEnemies:
      case TargetType.RandomEnemy:
        flag = true;
        break;
      default:
        flag = false;
        break;
    }
    if (flag)
    {
      IReadOnlyList<Creature> hittableEnemies = this.Card.CombatState.HittableEnemies;
      if (hittableEnemies.Count == 1)
        this.CardNode.SetPreviewTarget(hittableEnemies[0]);
      this.CardNode.UpdateVisuals(this.CardNode.Model?.Pile?.Type.GetValueOrDefault(), CardPreviewMode.MultiCreatureTargeting);
      foreach (Creature creature in (IEnumerable<Creature>) hittableEnemies)
        NCombatRoom.Instance.GetCreatureNode(creature)?.ShowMultiselectReticle();
    }
    if (this.Card.TargetType == TargetType.AllAllies)
    {
      foreach (Creature creature in this.Card.CombatState.PlayerCreatures.Where<Creature>((Func<Creature, bool>) (c => c.IsAlive)))
        NCombatRoom.Instance.GetCreatureNode(creature)?.ShowMultiselectReticle();
    }
    else if (this.Card.TargetType == TargetType.Self)
    {
      NCombatRoom.Instance.GetCreatureNode(this.Card.Owner.Creature)?.ShowMultiselectReticle();
    }
    else
    {
      if (this.Card.TargetType != TargetType.Osty)
        return;
      NCombatRoom.Instance.GetCreatureNode(this.Card.Owner.Osty)?.ShowMultiselectReticle();
    }
  }

  private void AutoDisableCannotPlayCardFtueCheck()
  {
    ++NCardPlay._totalCardsPlayedForFtue;
    if (NCardPlay._totalCardsPlayedForFtue != 8 || SaveManager.Instance.SeenFtue("cannot_play_card_ftue"))
      return;
    Log.Info("Cannot play cards FTUE was disabled, the player never saw it!!");
    SaveManager.Instance.MarkFtueAsComplete("cannot_play_card_ftue");
  }

  protected void TryShowEvokingOrbs()
  {
    if (this.Card == null)
      return;
    this.CardOwnerNode?.OrbManager?.UpdateVisuals(this.Card.OrbEvokeType);
  }

  private void HideEvokingOrbs()
  {
    this.CardOwnerNode?.OrbManager?.UpdateVisuals(OrbEvokeType.None);
  }

  private void ClearTarget() => this.CardNode?.SetPreviewTarget((Creature) null);

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(14)
    {
      new MethodInfo(NCardPlay.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCardPlay.MethodName.Start, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCardPlay.MethodName.CancelPlayCard, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCardPlay.MethodName.OnCancelPlayCard, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCardPlay.MethodName.Cleanup, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 1L, StringName.op_Implicit("isFinished"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NCardPlay.MethodName.OnCreatureHover, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("creature"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false)
      }, (List<Variant>) null),
      new MethodInfo(NCardPlay.MethodName.OnCreatureUnhover, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("_"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false)
      }, (List<Variant>) null),
      new MethodInfo(NCardPlay.MethodName.CenterCard, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCardPlay.MethodName.HideTargetingVisuals, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCardPlay.MethodName.ShowMultiCreatureTargetingVisuals, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCardPlay.MethodName.AutoDisableCannotPlayCardFtueCheck, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCardPlay.MethodName.TryShowEvokingOrbs, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCardPlay.MethodName.HideEvokingOrbs, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCardPlay.MethodName.ClearTarget, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NCardPlay.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      base._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCardPlay.MethodName.Start) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.Start();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCardPlay.MethodName.CancelPlayCard) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.CancelPlayCard();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCardPlay.MethodName.OnCancelPlayCard) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnCancelPlayCard();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCardPlay.MethodName.Cleanup) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.Cleanup(VariantUtils.ConvertTo<bool>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCardPlay.MethodName.OnCreatureHover) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.OnCreatureHover(VariantUtils.ConvertTo<NCreature>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCardPlay.MethodName.OnCreatureUnhover) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.OnCreatureUnhover(VariantUtils.ConvertTo<NCreature>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCardPlay.MethodName.CenterCard) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.CenterCard();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCardPlay.MethodName.HideTargetingVisuals) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.HideTargetingVisuals();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCardPlay.MethodName.ShowMultiCreatureTargetingVisuals) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.ShowMultiCreatureTargetingVisuals();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCardPlay.MethodName.AutoDisableCannotPlayCardFtueCheck) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.AutoDisableCannotPlayCardFtueCheck();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCardPlay.MethodName.TryShowEvokingOrbs) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.TryShowEvokingOrbs();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCardPlay.MethodName.HideEvokingOrbs) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.HideEvokingOrbs();
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NCardPlay.MethodName.ClearTarget) || ((NativeVariantPtrArgs) ref args).Count != 0)
      return base.InvokeGodotClassMethod(ref method, args, ref ret);
    this.ClearTarget();
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NCardPlay.MethodName._Ready) || StringName.op_Equality(ref method, NCardPlay.MethodName.Start) || StringName.op_Equality(ref method, NCardPlay.MethodName.CancelPlayCard) || StringName.op_Equality(ref method, NCardPlay.MethodName.OnCancelPlayCard) || StringName.op_Equality(ref method, NCardPlay.MethodName.Cleanup) || StringName.op_Equality(ref method, NCardPlay.MethodName.OnCreatureHover) || StringName.op_Equality(ref method, NCardPlay.MethodName.OnCreatureUnhover) || StringName.op_Equality(ref method, NCardPlay.MethodName.CenterCard) || StringName.op_Equality(ref method, NCardPlay.MethodName.HideTargetingVisuals) || StringName.op_Equality(ref method, NCardPlay.MethodName.ShowMultiCreatureTargetingVisuals) || StringName.op_Equality(ref method, NCardPlay.MethodName.AutoDisableCannotPlayCardFtueCheck) || StringName.op_Equality(ref method, NCardPlay.MethodName.TryShowEvokingOrbs) || StringName.op_Equality(ref method, NCardPlay.MethodName.HideEvokingOrbs) || StringName.op_Equality(ref method, NCardPlay.MethodName.ClearTarget) || base.HasGodotClassMethod(ref method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NCardPlay.PropertyName.Holder))
    {
      this.Holder = VariantUtils.ConvertTo<NHandCardHolder>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCardPlay.PropertyName._viewport))
    {
      this._viewport = VariantUtils.ConvertTo<Viewport>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NCardPlay.PropertyName._isTryingToPlayCard))
      return ((GodotObject) this).SetGodotClassPropertyValue(ref name, ref value);
    this._isTryingToPlayCard = VariantUtils.ConvertTo<bool>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NCardPlay.PropertyName.Holder))
    {
      ref godot_variant local = ref value;
      NHandCardHolder holder = this.Holder;
      godot_variant from = VariantUtils.CreateFrom<NHandCardHolder>(ref holder);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NCardPlay.PropertyName.CardNode))
    {
      ref godot_variant local = ref value;
      NCard cardNode = this.CardNode;
      godot_variant from = VariantUtils.CreateFrom<NCard>(ref cardNode);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NCardPlay.PropertyName.CardOwnerNode))
    {
      ref godot_variant local = ref value;
      NCreature cardOwnerNode = this.CardOwnerNode;
      godot_variant from = VariantUtils.CreateFrom<NCreature>(ref cardOwnerNode);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NCardPlay.PropertyName._viewport))
    {
      value = VariantUtils.CreateFrom<Viewport>(ref this._viewport);
      return true;
    }
    if (!StringName.op_Equality(ref name, NCardPlay.PropertyName._isTryingToPlayCard))
      return ((GodotObject) this).GetGodotClassPropertyValue(ref name, ref value);
    value = VariantUtils.CreateFrom<bool>(ref this._isTryingToPlayCard);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NCardPlay.PropertyName.Holder, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NCardPlay.PropertyName.CardNode, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NCardPlay.PropertyName.CardOwnerNode, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NCardPlay.PropertyName._viewport, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 1L, NCardPlay.PropertyName._isTryingToPlayCard, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).SaveGodotObjectData(info);
    GodotSerializationInfo serializationInfo = info;
    StringName holder1 = NCardPlay.PropertyName.Holder;
    NHandCardHolder holder2 = this.Holder;
    Variant variant = Variant.From<NHandCardHolder>(ref holder2);
    serializationInfo.AddProperty(holder1, variant);
    info.AddProperty(NCardPlay.PropertyName._viewport, Variant.From<Viewport>(ref this._viewport));
    info.AddProperty(NCardPlay.PropertyName._isTryingToPlayCard, Variant.From<bool>(ref this._isTryingToPlayCard));
    info.AddSignalEventDelegate(NCardPlay.SignalName.Finished, (Delegate) this.backing_Finished);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NCardPlay.PropertyName.Holder, ref variant1))
      this.Holder = ((Variant) ref variant1).As<NHandCardHolder>();
    Variant variant2;
    if (info.TryGetProperty(NCardPlay.PropertyName._viewport, ref variant2))
      this._viewport = ((Variant) ref variant2).As<Viewport>();
    Variant variant3;
    if (info.TryGetProperty(NCardPlay.PropertyName._isTryingToPlayCard, ref variant3))
      this._isTryingToPlayCard = ((Variant) ref variant3).As<bool>();
    NCardPlay.FinishedEventHandler finishedEventHandler;
    if (!info.TryGetSignalEventDelegate<NCardPlay.FinishedEventHandler>(NCardPlay.SignalName.Finished, ref finishedEventHandler))
      return;
    this.backing_Finished = finishedEventHandler;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<MethodInfo> GetGodotSignalList()
  {
    return new List<MethodInfo>(1)
    {
      new MethodInfo(NCardPlay.SignalName.Finished, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 1L, StringName.op_Implicit("success"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null)
    };
  }

  public event NCardPlay.FinishedEventHandler Finished
  {
    add => this.backing_Finished += value;
    remove => this.backing_Finished -= value;
  }

  protected void EmitSignalFinished(bool success)
  {
    ((GodotObject) this).EmitSignal(NCardPlay.SignalName.Finished, new Variant[1]
    {
      Variant.op_Implicit(success)
    });
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RaiseGodotClassSignalCallbacks(
    in godot_string_name signal,
    NativeVariantPtrArgs args)
  {
    if (StringName.op_Equality(ref signal, NCardPlay.SignalName.Finished) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      NCardPlay.FinishedEventHandler backingFinished = this.backing_Finished;
      if (backingFinished == null)
        return;
      backingFinished(VariantUtils.ConvertTo<bool>(ref ((NativeVariantPtrArgs) ref args)[0]));
    }
    else
      ((GodotObject) this).RaiseGodotClassSignalCallbacks(ref signal, args);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassSignal(in godot_string_name signal)
  {
    return StringName.op_Equality(ref signal, NCardPlay.SignalName.Finished) || base.HasGodotClassSignal(ref signal);
  }

  [Signal]
  public delegate void FinishedEventHandler(bool success);

  public class MethodName : Node.MethodName
  {
    public static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName Start = StringName.op_Implicit(nameof (Start));
    public static readonly StringName CancelPlayCard = StringName.op_Implicit(nameof (CancelPlayCard));
    public static readonly StringName OnCancelPlayCard = StringName.op_Implicit(nameof (OnCancelPlayCard));
    public static readonly StringName Cleanup = StringName.op_Implicit(nameof (Cleanup));
    public static readonly StringName OnCreatureHover = StringName.op_Implicit(nameof (OnCreatureHover));
    public static readonly StringName OnCreatureUnhover = StringName.op_Implicit(nameof (OnCreatureUnhover));
    public static readonly StringName CenterCard = StringName.op_Implicit(nameof (CenterCard));
    public static readonly StringName HideTargetingVisuals = StringName.op_Implicit(nameof (HideTargetingVisuals));
    public static readonly StringName ShowMultiCreatureTargetingVisuals = StringName.op_Implicit(nameof (ShowMultiCreatureTargetingVisuals));
    public static readonly StringName AutoDisableCannotPlayCardFtueCheck = StringName.op_Implicit(nameof (AutoDisableCannotPlayCardFtueCheck));
    public static readonly StringName TryShowEvokingOrbs = StringName.op_Implicit(nameof (TryShowEvokingOrbs));
    public static readonly StringName HideEvokingOrbs = StringName.op_Implicit(nameof (HideEvokingOrbs));
    public static readonly StringName ClearTarget = StringName.op_Implicit(nameof (ClearTarget));
  }

  public class PropertyName : Node.PropertyName
  {
    public static readonly StringName Holder = StringName.op_Implicit(nameof (Holder));
    public static readonly StringName CardNode = StringName.op_Implicit(nameof (CardNode));
    public static readonly StringName CardOwnerNode = StringName.op_Implicit(nameof (CardOwnerNode));
    public static readonly StringName _viewport = StringName.op_Implicit(nameof (_viewport));
    public static readonly StringName _isTryingToPlayCard = StringName.op_Implicit(nameof (_isTryingToPlayCard));
  }

  public class SignalName : Node.SignalName
  {
    public static readonly StringName Finished = StringName.op_Implicit(nameof (Finished));
  }
}
