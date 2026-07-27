// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Combat.NControllerCardPlay
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Audio.Debug;
using MegaCrit.Sts2.Core.ControllerInput;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Cards.Holders;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.HoverTips;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Combat;

[ScriptPath("res://src/Core/Nodes/Combat/NControllerCardPlay.cs")]
public class NControllerCardPlay : NCardPlay
{
  private Callable _onCreatureHoverCallable;
  private Callable _onCreatureUnhoverCallable;
  private bool _signalsConnected;
  private 
  #nullable disable
  NControllerCardPlay.ConfirmedEventHandler backing_Confirmed;
  private NControllerCardPlay.CanceledEventHandler backing_Canceled;

  public override void _Input(
  #nullable enable
  InputEvent inputEvent)
  {
    if (!(inputEvent is InputEventAction inputEventAction))
      return;
    if (((InputEvent) inputEventAction).IsActionPressed(MegaInput.select, false, false))
    {
      ((GodotObject) this).EmitSignal(NControllerCardPlay.SignalName.Confirmed, Array.Empty<Variant>());
      this.GetViewport()?.SetInputAsHandled();
    }
    if (!((InputEvent) inputEventAction).IsActionPressed(MegaInput.cancel, false, false) && !((InputEvent) inputEventAction).IsActionPressed(MegaInput.topPanel, false, false))
      return;
    ((GodotObject) this).EmitSignal(NControllerCardPlay.SignalName.Canceled, Array.Empty<Variant>());
    if (!inputEvent.IsActionPressed(MegaInput.cancel, false, false))
      return;
    this.GetViewport().SetInputAsHandled();
  }

  public static NControllerCardPlay Create(NHandCardHolder holder)
  {
    NControllerCardPlay ncontrollerCardPlay = new NControllerCardPlay();
    ncontrollerCardPlay.Holder = holder;
    ncontrollerCardPlay.Player = holder.CardModel.Owner;
    return ncontrollerCardPlay;
  }

  public override void Start()
  {
    if (this.Card == null || this.CardNode == null)
      return;
    NDebugAudioManager.Instance?.Play("card_select.mp3");
    NHoverTipSet.Remove((Control) this.Holder);
    this._onCreatureHoverCallable = Callable.From<NCreature>(new Action<NCreature>(((NCardPlay) this).OnCreatureHover));
    this._onCreatureUnhoverCallable = Callable.From<NCreature>(new Action<NCreature>(((NCardPlay) this).OnCreatureUnhover));
    UnplayableReason reason;
    AbstractModel preventer;
    if (!this.Card.CanPlay(out reason, out preventer))
    {
      this.CannotPlayThisCardFtueCheck(this.Card);
      this.CancelPlayCard();
      LocString playerDialogueLine = reason.GetPlayerDialogueLine(preventer);
      if (playerDialogueLine == null)
        return;
      ((Node) NCombatRoom.Instance.CombatVfxContainer).AddChildSafely((Node) NThoughtBubbleVfx.Create(playerDialogueLine.GetFormattedText(), this.Card.Owner.Creature, new double?(1.0)));
    }
    else
    {
      this.TryShowEvokingOrbs();
      this.CardNode.CardHighlight.AnimFlash();
      this.CenterCard();
      bool flag;
      switch (this.Card.TargetType)
      {
        case TargetType.AnyEnemy:
        case TargetType.AnyAlly:
          flag = true;
          break;
        default:
          flag = false;
          break;
      }
      if (flag)
        TaskHelper.RunSafely(this.SingleCreatureTargeting(this.Card.TargetType));
      else
        this.MultiCreatureTargeting();
    }
  }

  private async Task SingleCreatureTargeting(TargetType targetType)
  {
    Creature owner = this.Card.Owner.Creature;
    List<Creature> source = new List<Creature>();
    switch (targetType)
    {
      case TargetType.AnyEnemy:
        source = owner.CombatState.GetOpponentsOf(owner).Where<Creature>((Func<Creature, bool>) (c => c.IsHittable)).ToList<Creature>();
        break;
      case TargetType.AnyAlly:
        source = this.Card.CombatState.PlayerCreatures.Where<Creature>((Func<Creature, bool>) (c => c.IsHittable && c != owner)).ToList<Creature>();
        break;
    }
    if (source.Count == 0)
    {
      this.CancelPlayCard();
    }
    else
    {
      List<NCreature> list = source.Select<Creature, NCreature>((Func<Creature, NCreature>) (c => NCombatRoom.Instance.GetCreatureNode(c))).OfType<NCreature>().ToList<NCreature>();
      if (list.Count == 0)
      {
        this.CancelPlayCard();
      }
      else
      {
        NTargetManager instance = NTargetManager.Instance;
        ((GodotObject) instance).Connect(NTargetManager.SignalName.CreatureHovered, this._onCreatureHoverCallable, 0U);
        ((GodotObject) instance).Connect(NTargetManager.SignalName.CreatureUnhovered, this._onCreatureUnhoverCallable, 0U);
        this._signalsConnected = true;
        try
        {
          instance.StartTargeting(targetType, (Control) this.CardNode, TargetMode.Controller, (Func<bool>) (() => !GodotObject.IsInstanceValid((GodotObject) this) || !NControllerManager.Instance.IsUsingController), (Func<Node, bool>) null);
          NCombatRoom.Instance.RestrictControllerNavigation(list.Select<NCreature, Control>((Func<NCreature, Control>) (n => n.Hitbox)));
          NCreature ncreature1 = list.First<NCreature>();
          if (NCombatRoom.Instance.LastTargetedCreature != null && NCombatRoom.Instance.LastTargetedCreature.IsHittable)
          {
            NCreature ncreature2 = list.FirstOrDefault<NCreature>((Func<NCreature, bool>) (c => c.Entity == NCombatRoom.Instance.LastTargetedCreature));
            if (ncreature2 != null)
              ncreature1 = ncreature2;
          }
          ncreature1.Hitbox.TryGrabFocus();
          NCreature ncreature3 = (NCreature) await instance.SelectionFinished();
          NCombatRoom.Instance.EnableControllerNavigation();
          if (!GodotObject.IsInstanceValid((GodotObject) this))
            return;
          if (ncreature3 != null)
            this.TryPlayCard(ncreature3.Entity);
          else
            this.CancelPlayCard();
        }
        finally
        {
          this.DisconnectTargetingSignals();
        }
      }
    }
  }

  public override void _ExitTree() => this.DisconnectTargetingSignals();

  private void DisconnectTargetingSignals()
  {
    if (!this._signalsConnected)
      return;
    this._signalsConnected = false;
    if (NRun.Instance == null)
      return;
    NTargetManager instance = NTargetManager.Instance;
    ((GodotObject) instance).Disconnect(NTargetManager.SignalName.CreatureHovered, this._onCreatureHoverCallable);
    ((GodotObject) instance).Disconnect(NTargetManager.SignalName.CreatureUnhovered, this._onCreatureUnhoverCallable);
  }

  private void MultiCreatureTargeting()
  {
    NCombatRoom.Instance.RestrictControllerNavigation((IEnumerable<Control>) Array.Empty<Control>());
    this.ShowMultiCreatureTargetingVisuals();
    ((GodotObject) this).Connect(NControllerCardPlay.SignalName.Confirmed, Callable.From((Action) (() =>
    {
      NCombatRoom.Instance.EnableControllerNavigation();
      this.TryPlayCard((Creature) null);
    })), 0U);
    ((GodotObject) this).Connect(NControllerCardPlay.SignalName.Canceled, Callable.From(new Action(((NCardPlay) this).CancelPlayCard)), 0U);
  }

  protected override void OnCancelPlayCard()
  {
    NCombatRoom.Instance.EnableControllerNavigation();
    this.Holder.TryGrabFocus();
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(7)
    {
      new MethodInfo(NControllerCardPlay.MethodName._Input, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("inputEvent"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("InputEvent"), false)
      }, (List<Variant>) null),
      new MethodInfo(NControllerCardPlay.MethodName.Create, new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Node"), false), (MethodFlags) 33L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("holder"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false)
      }, (List<Variant>) null),
      new MethodInfo(NControllerCardPlay.MethodName.Start, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NControllerCardPlay.MethodName._ExitTree, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NControllerCardPlay.MethodName.DisconnectTargetingSignals, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NControllerCardPlay.MethodName.MultiCreatureTargeting, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NControllerCardPlay.MethodName.OnCancelPlayCard, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NControllerCardPlay.MethodName._Input) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      base._Input(VariantUtils.ConvertTo<InputEvent>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NControllerCardPlay.MethodName.Create) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      NControllerCardPlay ncontrollerCardPlay = NControllerCardPlay.Create(VariantUtils.ConvertTo<NHandCardHolder>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = VariantUtils.CreateFrom<NControllerCardPlay>(ref ncontrollerCardPlay);
      return true;
    }
    if (StringName.op_Equality(ref method, NControllerCardPlay.MethodName.Start) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.Start();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NControllerCardPlay.MethodName._ExitTree) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      base._ExitTree();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NControllerCardPlay.MethodName.DisconnectTargetingSignals) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.DisconnectTargetingSignals();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NControllerCardPlay.MethodName.MultiCreatureTargeting) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.MultiCreatureTargeting();
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NControllerCardPlay.MethodName.OnCancelPlayCard) || ((NativeVariantPtrArgs) ref args).Count != 0)
      return base.InvokeGodotClassMethod(in method, args, out ret);
    this.OnCancelPlayCard();
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static bool InvokeGodotClassStaticMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NControllerCardPlay.MethodName.Create) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      NControllerCardPlay ncontrollerCardPlay = NControllerCardPlay.Create(VariantUtils.ConvertTo<NHandCardHolder>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = VariantUtils.CreateFrom<NControllerCardPlay>(ref ncontrollerCardPlay);
      return true;
    }
    ret = new godot_variant();
    return false;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NControllerCardPlay.MethodName._Input) || StringName.op_Equality(ref method, NControllerCardPlay.MethodName.Create) || StringName.op_Equality(ref method, NControllerCardPlay.MethodName.Start) || StringName.op_Equality(ref method, NControllerCardPlay.MethodName._ExitTree) || StringName.op_Equality(ref method, NControllerCardPlay.MethodName.DisconnectTargetingSignals) || StringName.op_Equality(ref method, NControllerCardPlay.MethodName.MultiCreatureTargeting) || StringName.op_Equality(ref method, NControllerCardPlay.MethodName.OnCancelPlayCard) || base.HasGodotClassMethod(in method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NControllerCardPlay.PropertyName._onCreatureHoverCallable))
    {
      this._onCreatureHoverCallable = VariantUtils.ConvertTo<Callable>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NControllerCardPlay.PropertyName._onCreatureUnhoverCallable))
    {
      this._onCreatureUnhoverCallable = VariantUtils.ConvertTo<Callable>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NControllerCardPlay.PropertyName._signalsConnected))
      return base.SetGodotClassPropertyValue(in name, in value);
    this._signalsConnected = VariantUtils.ConvertTo<bool>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NControllerCardPlay.PropertyName._onCreatureHoverCallable))
    {
      value = VariantUtils.CreateFrom<Callable>(ref this._onCreatureHoverCallable);
      return true;
    }
    if (StringName.op_Equality(ref name, NControllerCardPlay.PropertyName._onCreatureUnhoverCallable))
    {
      value = VariantUtils.CreateFrom<Callable>(ref this._onCreatureUnhoverCallable);
      return true;
    }
    if (!StringName.op_Equality(ref name, NControllerCardPlay.PropertyName._signalsConnected))
      return base.GetGodotClassPropertyValue(in name, out value);
    value = VariantUtils.CreateFrom<bool>(ref this._signalsConnected);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 25L, NControllerCardPlay.PropertyName._onCreatureHoverCallable, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 25L, NControllerCardPlay.PropertyName._onCreatureUnhoverCallable, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 1L, NControllerCardPlay.PropertyName._signalsConnected, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    base.SaveGodotObjectData(info);
    info.AddProperty(NControllerCardPlay.PropertyName._onCreatureHoverCallable, Variant.From<Callable>(ref this._onCreatureHoverCallable));
    info.AddProperty(NControllerCardPlay.PropertyName._onCreatureUnhoverCallable, Variant.From<Callable>(ref this._onCreatureUnhoverCallable));
    info.AddProperty(NControllerCardPlay.PropertyName._signalsConnected, Variant.From<bool>(ref this._signalsConnected));
    info.AddSignalEventDelegate(NControllerCardPlay.SignalName.Confirmed, (Delegate) this.backing_Confirmed);
    info.AddSignalEventDelegate(NControllerCardPlay.SignalName.Canceled, (Delegate) this.backing_Canceled);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    base.RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NControllerCardPlay.PropertyName._onCreatureHoverCallable, ref variant1))
      this._onCreatureHoverCallable = ((Variant) ref variant1).As<Callable>();
    Variant variant2;
    if (info.TryGetProperty(NControllerCardPlay.PropertyName._onCreatureUnhoverCallable, ref variant2))
      this._onCreatureUnhoverCallable = ((Variant) ref variant2).As<Callable>();
    Variant variant3;
    if (info.TryGetProperty(NControllerCardPlay.PropertyName._signalsConnected, ref variant3))
      this._signalsConnected = ((Variant) ref variant3).As<bool>();
    NControllerCardPlay.ConfirmedEventHandler confirmedEventHandler;
    if (info.TryGetSignalEventDelegate<NControllerCardPlay.ConfirmedEventHandler>(NControllerCardPlay.SignalName.Confirmed, ref confirmedEventHandler))
      this.backing_Confirmed = confirmedEventHandler;
    NControllerCardPlay.CanceledEventHandler canceledEventHandler;
    if (!info.TryGetSignalEventDelegate<NControllerCardPlay.CanceledEventHandler>(NControllerCardPlay.SignalName.Canceled, ref canceledEventHandler))
      return;
    this.backing_Canceled = canceledEventHandler;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static List<MethodInfo> GetGodotSignalList()
  {
    return new List<MethodInfo>(2)
    {
      new MethodInfo(NControllerCardPlay.SignalName.Confirmed, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NControllerCardPlay.SignalName.Canceled, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  public event NControllerCardPlay.ConfirmedEventHandler Confirmed
  {
    add => this.backing_Confirmed += value;
    remove => this.backing_Confirmed -= value;
  }

  protected void EmitSignalConfirmed()
  {
    ((GodotObject) this).EmitSignal(NControllerCardPlay.SignalName.Confirmed, Array.Empty<Variant>());
  }

  public event NControllerCardPlay.CanceledEventHandler Canceled
  {
    add => this.backing_Canceled += value;
    remove => this.backing_Canceled -= value;
  }

  protected void EmitSignalCanceled()
  {
    ((GodotObject) this).EmitSignal(NControllerCardPlay.SignalName.Canceled, Array.Empty<Variant>());
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RaiseGodotClassSignalCallbacks(
    in godot_string_name signal,
    NativeVariantPtrArgs args)
  {
    if (StringName.op_Equality(ref signal, NControllerCardPlay.SignalName.Confirmed) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      NControllerCardPlay.ConfirmedEventHandler backingConfirmed = this.backing_Confirmed;
      if (backingConfirmed == null)
        return;
      backingConfirmed();
    }
    else if (StringName.op_Equality(ref signal, NControllerCardPlay.SignalName.Canceled) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      NControllerCardPlay.CanceledEventHandler backingCanceled = this.backing_Canceled;
      if (backingCanceled == null)
        return;
      backingCanceled();
    }
    else
      base.RaiseGodotClassSignalCallbacks(in signal, args);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassSignal(in godot_string_name signal)
  {
    return StringName.op_Equality(ref signal, NControllerCardPlay.SignalName.Confirmed) || StringName.op_Equality(ref signal, NControllerCardPlay.SignalName.Canceled) || base.HasGodotClassSignal(in signal);
  }

  [Signal]
  public delegate void ConfirmedEventHandler();

  [Signal]
  public delegate void CanceledEventHandler();

  public new class MethodName : NCardPlay.MethodName
  {
    public static readonly StringName _Input = StringName.op_Implicit(nameof (_Input));
    public static readonly StringName Create = StringName.op_Implicit(nameof (Create));
    public new static readonly StringName Start = StringName.op_Implicit(nameof (Start));
    public static readonly StringName _ExitTree = StringName.op_Implicit(nameof (_ExitTree));
    public static readonly StringName DisconnectTargetingSignals = StringName.op_Implicit(nameof (DisconnectTargetingSignals));
    public static readonly StringName MultiCreatureTargeting = StringName.op_Implicit(nameof (MultiCreatureTargeting));
    public new static readonly StringName OnCancelPlayCard = StringName.op_Implicit(nameof (OnCancelPlayCard));
  }

  public new class PropertyName : NCardPlay.PropertyName
  {
    public static readonly StringName _onCreatureHoverCallable = StringName.op_Implicit(nameof (_onCreatureHoverCallable));
    public static readonly StringName _onCreatureUnhoverCallable = StringName.op_Implicit(nameof (_onCreatureUnhoverCallable));
    public static readonly StringName _signalsConnected = StringName.op_Implicit(nameof (_signalsConnected));
  }

  public new class SignalName : NCardPlay.SignalName
  {
    public static readonly StringName Confirmed = StringName.op_Implicit(nameof (Confirmed));
    public static readonly StringName Canceled = StringName.op_Implicit(nameof (Canceled));
  }
}
