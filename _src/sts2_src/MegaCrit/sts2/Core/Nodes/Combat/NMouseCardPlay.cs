// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Combat.NMouseCardPlay
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
using MegaCrit.Sts2.Core.Nodes.Multiplayer;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Combat;

[ScriptPath("res://src/Core/Nodes/Combat/NMouseCardPlay.cs")]
public class NMouseCardPlay : NCardPlay
{
  private const float _fakeLowerEnterPlayZoneDistance = 100f;
  private const float _fakeUpperEnterPlayZoneDistance = 50f;
  private const float _playZoneScreenProportion = 0.75f;
  private const float _cancelZoneScreenProportion = 0.95f;
  private bool _hasLeftCardCancelZoneOnce;
  private float _dragStartYPosition;
  private Creature? _target;
  private bool _isLeftMouseDown;
  private CancellationTokenSource _cancellationTokenSource;
  private Callable _onCreatureHoverCallable;
  private Callable _onCreatureUnhoverCallable;
  private bool _signalsConnected;
  private StringName _cancelShortcut;
  private bool _skipStartCardDrag;

  private float PlayZoneThreshold
  {
    get
    {
      Rect2 visibleRect = this._viewport.GetVisibleRect();
      float num = ((Rect2) ref visibleRect).Size.Y * 0.75f;
      if (this._skipStartCardDrag)
        return num + 100f;
      return (double) this._dragStartYPosition > (double) num ? Mathf.Max(num, this._dragStartYPosition - 100f) : Mathf.Min(num, this._dragStartYPosition - 50f);
    }
  }

  private float CancelZoneThreshold
  {
    get
    {
      Rect2 visibleRect = this._viewport.GetVisibleRect();
      return ((Rect2) ref visibleRect).Size.Y * 0.95f;
    }
  }

  public static NMouseCardPlay Create(
    NHandCardHolder holder,
    StringName cancelShortcut,
    bool wasStartedWithShortcut)
  {
    NMouseCardPlay nmouseCardPlay = new NMouseCardPlay();
    nmouseCardPlay.Holder = holder;
    nmouseCardPlay.Player = holder.CardModel.Owner;
    nmouseCardPlay._cancelShortcut = cancelShortcut;
    nmouseCardPlay._skipStartCardDrag = wasStartedWithShortcut;
    return nmouseCardPlay;
  }

  public override void _Input(InputEvent inputEvent)
  {
    if (inputEvent is InputEventMouseButton eventMouseButton)
    {
      MouseButton buttonIndex = eventMouseButton.ButtonIndex;
      if (buttonIndex != 1L)
      {
        if (buttonIndex == 2L && ((InputEvent) eventMouseButton).IsPressed())
          this.CancelPlayCard();
      }
      else if (((InputEvent) eventMouseButton).IsPressed())
        this._isLeftMouseDown = true;
      else if (((InputEvent) eventMouseButton).IsReleased())
        this._isLeftMouseDown = false;
    }
    if (!inputEvent.IsActionPressed(this._cancelShortcut, false, false) && !inputEvent.IsActionPressed(MegaInput.releaseCard, false, false))
      return;
    this.CancelPlayCard();
    this.GetViewport()?.SetInputAsHandled();
  }

  public override void Start()
  {
    this._isLeftMouseDown = !this._skipStartCardDrag;
    this.Holder.Hitbox.MouseFilter = (Control.MouseFilterEnum) 2L;
    this._cancellationTokenSource = new CancellationTokenSource();
    this._onCreatureHoverCallable = Callable.From<NCreature>(new Action<NCreature>(((NCardPlay) this).OnCreatureHover));
    this._onCreatureUnhoverCallable = Callable.From<NCreature>(new Action<NCreature>(((NCardPlay) this).OnCreatureUnhover));
    TaskHelper.RunSafely(this.StartAsync());
  }

  public override void _EnterTree()
  {
    if (NControllerManager.Instance == null)
      return;
    ((GodotObject) NControllerManager.Instance).Connect(NControllerManager.SignalName.MouseDetected, Callable.From(new Action(((NCardPlay) this).CancelPlayCard)), 0U);
    ((GodotObject) NControllerManager.Instance).Connect(NControllerManager.SignalName.ControllerDetected, Callable.From(new Action(((NCardPlay) this).CancelPlayCard)), 0U);
  }

  public override void _ExitTree()
  {
    if (NControllerManager.Instance != null)
    {
      ((GodotObject) NControllerManager.Instance).Disconnect(NControllerManager.SignalName.MouseDetected, Callable.From(new Action(((NCardPlay) this).CancelPlayCard)));
      ((GodotObject) NControllerManager.Instance).Disconnect(NControllerManager.SignalName.ControllerDetected, Callable.From(new Action(((NCardPlay) this).CancelPlayCard)));
    }
    this._cancellationTokenSource.Cancel();
    this.DisconnectTargetingSignals();
  }

  private async Task StartAsync()
  {
    if (this.Card == null || this.CardNode == null)
      return;
    await this.StartCardDrag();
    if (this._cancellationTokenSource.IsCancellationRequested)
      return;
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
      this.CardNode.CardHighlight.AnimFlash();
      await this.TargetSelection(!this._skipStartCardDrag ? (this._isLeftMouseDown ? TargetMode.ReleaseMouseToTarget : TargetMode.ClickMouseToTarget) : TargetMode.ClickMouseToTarget);
      if (this._cancellationTokenSource.IsCancellationRequested)
        return;
      if (!this.IsCardInPlayZone())
        this.CancelPlayCard();
      if (this._cancellationTokenSource.IsCancellationRequested)
        return;
      this.TryPlayCard(this._target);
    }
  }

  private async Task StartCardDrag()
  {
    NDebugAudioManager.Instance?.Play("card_select.mp3", 0.5f);
    NHoverTipSet.Remove((Control) this.Holder);
    this._dragStartYPosition = this._viewport.GetMousePosition().Y;
    if (this._skipStartCardDrag)
      return;
    do
    {
      await this.LerpToMouse(this.Holder);
    }
    while (!this.IsCardInPlayZone() && !this._cancellationTokenSource.IsCancellationRequested);
  }

  private async Task TargetSelection(TargetMode targetMode)
  {
    if (this.Card == null)
      return;
    this.TryShowEvokingOrbs();
    this.CardNode?.CardHighlight.AnimFlash();
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
      await this.SingleCreatureTargeting(targetMode, this.Card.TargetType);
    else
      await this.MultiCreatureTargeting(targetMode);
  }

  private async Task SingleCreatureTargeting(TargetMode targetMode, TargetType targetType)
  {
    if (this._cancellationTokenSource.IsCancellationRequested)
      return;
    this.CenterCard();
    NTargetManager instance = NTargetManager.Instance;
    ((GodotObject) instance).Connect(NTargetManager.SignalName.CreatureHovered, this._onCreatureHoverCallable, 0U);
    ((GodotObject) instance).Connect(NTargetManager.SignalName.CreatureUnhovered, this._onCreatureUnhoverCallable, 0U);
    this._signalsConnected = true;
    try
    {
      instance.StartTargeting(targetType, (Control) this.CardNode, targetMode, (Func<bool>) (() => this.IsCardInCancelZone() || this._cancellationTokenSource.IsCancellationRequested), (Func<Node, bool>) null);
      Node actualValue = await instance.SelectionFinished();
      if (this._cancellationTokenSource.IsCancellationRequested)
        return;
      Creature creature;
      switch (actualValue)
      {
        case null:
          return;
        case NCreature ncreature:
          creature = ncreature.Entity;
          break;
        case NMultiplayerPlayerState nmultiplayerPlayerState:
          creature = nmultiplayerPlayerState.Player.Creature;
          break;
        default:
          throw new ArgumentOutOfRangeException("target", (object) actualValue, (string) null);
      }
      this._target = creature;
    }
    finally
    {
      this.DisconnectTargetingSignals();
    }
  }

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

  private async Task MultiCreatureTargeting(TargetMode targetMode)
  {
    bool isShowingTargetingVisuals = false;
    Func<bool> shouldFinishTargeting = targetMode == TargetMode.ReleaseMouseToTarget ? (Func<bool>) (() => !this._isLeftMouseDown) : (Func<bool>) (() => this._isLeftMouseDown);
    do
    {
      if (isShowingTargetingVisuals)
      {
        if (!this.IsCardInPlayZone())
        {
          this.HideTargetingVisuals();
          isShowingTargetingVisuals = false;
        }
      }
      else if (this.IsCardInPlayZone())
      {
        this.ShowMultiCreatureTargetingVisuals();
        isShowingTargetingVisuals = true;
      }
      await this.LerpToMouse(this.Holder);
    }
    while (!shouldFinishTargeting() && !this._cancellationTokenSource.IsCancellationRequested && !this.IsCardInCancelZone());
    if (this._cancellationTokenSource.IsCancellationRequested)
      shouldFinishTargeting = (Func<bool>) null;
    else if (!this.IsCardInCancelZone())
    {
      shouldFinishTargeting = (Func<bool>) null;
    }
    else
    {
      this.CancelPlayCard();
      shouldFinishTargeting = (Func<bool>) null;
    }
  }

  protected override void OnCancelPlayCard()
  {
    if (!GodotObject.IsInstanceValid((GodotObject) this) || !this.IsInsideTree())
      return;
    this.Holder.Hitbox.MouseFilter = (Control.MouseFilterEnum) 0L;
    this._cancellationTokenSource.Cancel();
  }

  private async Task LerpToMouse(NHandCardHolder cardHolder)
  {
    cardHolder.SetTargetPosition(this._viewport.GetMousePosition());
    double num = (double) await this.AwaitProcessFrame();
  }

  private bool IsCardInPlayZone()
  {
    return (double) this._viewport.GetMousePosition().Y < (double) this.PlayZoneThreshold;
  }

  private bool IsCardInCancelZone()
  {
    this._hasLeftCardCancelZoneOnce |= (double) this._viewport.GetMousePosition().Y <= (double) this.CancelZoneThreshold;
    return (double) this._viewport.GetMousePosition().Y > (double) this.CancelZoneThreshold && this._hasLeftCardCancelZoneOnce;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(9)
    {
      new MethodInfo(NMouseCardPlay.MethodName.Create, new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Node"), false), (MethodFlags) 33L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("holder"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false),
        new PropertyInfo((Variant.Type) 21L, StringName.op_Implicit("cancelShortcut"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false),
        new PropertyInfo((Variant.Type) 1L, StringName.op_Implicit("wasStartedWithShortcut"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NMouseCardPlay.MethodName._Input, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("inputEvent"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("InputEvent"), false)
      }, (List<Variant>) null),
      new MethodInfo(NMouseCardPlay.MethodName.Start, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NMouseCardPlay.MethodName._EnterTree, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NMouseCardPlay.MethodName._ExitTree, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NMouseCardPlay.MethodName.DisconnectTargetingSignals, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NMouseCardPlay.MethodName.OnCancelPlayCard, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NMouseCardPlay.MethodName.IsCardInPlayZone, new PropertyInfo((Variant.Type) 1L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NMouseCardPlay.MethodName.IsCardInCancelZone, new PropertyInfo((Variant.Type) 1L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NMouseCardPlay.MethodName.Create) && ((NativeVariantPtrArgs) ref args).Count == 3)
    {
      NMouseCardPlay nmouseCardPlay = NMouseCardPlay.Create(VariantUtils.ConvertTo<NHandCardHolder>(ref ((NativeVariantPtrArgs) ref args)[0]), VariantUtils.ConvertTo<StringName>(ref ((NativeVariantPtrArgs) ref args)[1]), VariantUtils.ConvertTo<bool>(ref ((NativeVariantPtrArgs) ref args)[2]));
      ret = VariantUtils.CreateFrom<NMouseCardPlay>(ref nmouseCardPlay);
      return true;
    }
    if (StringName.op_Equality(ref method, NMouseCardPlay.MethodName._Input) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      base._Input(VariantUtils.ConvertTo<InputEvent>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NMouseCardPlay.MethodName.Start) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.Start();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NMouseCardPlay.MethodName._EnterTree) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      base._EnterTree();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NMouseCardPlay.MethodName._ExitTree) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      base._ExitTree();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NMouseCardPlay.MethodName.DisconnectTargetingSignals) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.DisconnectTargetingSignals();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NMouseCardPlay.MethodName.OnCancelPlayCard) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnCancelPlayCard();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NMouseCardPlay.MethodName.IsCardInPlayZone) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      bool flag = this.IsCardInPlayZone();
      ret = VariantUtils.CreateFrom<bool>(ref flag);
      return true;
    }
    if (!StringName.op_Equality(ref method, NMouseCardPlay.MethodName.IsCardInCancelZone) || ((NativeVariantPtrArgs) ref args).Count != 0)
      return base.InvokeGodotClassMethod(in method, args, out ret);
    bool flag1 = this.IsCardInCancelZone();
    ret = VariantUtils.CreateFrom<bool>(ref flag1);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static bool InvokeGodotClassStaticMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NMouseCardPlay.MethodName.Create) && ((NativeVariantPtrArgs) ref args).Count == 3)
    {
      NMouseCardPlay nmouseCardPlay = NMouseCardPlay.Create(VariantUtils.ConvertTo<NHandCardHolder>(ref ((NativeVariantPtrArgs) ref args)[0]), VariantUtils.ConvertTo<StringName>(ref ((NativeVariantPtrArgs) ref args)[1]), VariantUtils.ConvertTo<bool>(ref ((NativeVariantPtrArgs) ref args)[2]));
      ret = VariantUtils.CreateFrom<NMouseCardPlay>(ref nmouseCardPlay);
      return true;
    }
    ret = new godot_variant();
    return false;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NMouseCardPlay.MethodName.Create) || StringName.op_Equality(ref method, NMouseCardPlay.MethodName._Input) || StringName.op_Equality(ref method, NMouseCardPlay.MethodName.Start) || StringName.op_Equality(ref method, NMouseCardPlay.MethodName._EnterTree) || StringName.op_Equality(ref method, NMouseCardPlay.MethodName._ExitTree) || StringName.op_Equality(ref method, NMouseCardPlay.MethodName.DisconnectTargetingSignals) || StringName.op_Equality(ref method, NMouseCardPlay.MethodName.OnCancelPlayCard) || StringName.op_Equality(ref method, NMouseCardPlay.MethodName.IsCardInPlayZone) || StringName.op_Equality(ref method, NMouseCardPlay.MethodName.IsCardInCancelZone) || base.HasGodotClassMethod(in method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NMouseCardPlay.PropertyName._hasLeftCardCancelZoneOnce))
    {
      this._hasLeftCardCancelZoneOnce = VariantUtils.ConvertTo<bool>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NMouseCardPlay.PropertyName._dragStartYPosition))
    {
      this._dragStartYPosition = VariantUtils.ConvertTo<float>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NMouseCardPlay.PropertyName._isLeftMouseDown))
    {
      this._isLeftMouseDown = VariantUtils.ConvertTo<bool>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NMouseCardPlay.PropertyName._onCreatureHoverCallable))
    {
      this._onCreatureHoverCallable = VariantUtils.ConvertTo<Callable>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NMouseCardPlay.PropertyName._onCreatureUnhoverCallable))
    {
      this._onCreatureUnhoverCallable = VariantUtils.ConvertTo<Callable>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NMouseCardPlay.PropertyName._signalsConnected))
    {
      this._signalsConnected = VariantUtils.ConvertTo<bool>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NMouseCardPlay.PropertyName._cancelShortcut))
    {
      this._cancelShortcut = VariantUtils.ConvertTo<StringName>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NMouseCardPlay.PropertyName._skipStartCardDrag))
      return base.SetGodotClassPropertyValue(in name, in value);
    this._skipStartCardDrag = VariantUtils.ConvertTo<bool>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NMouseCardPlay.PropertyName.PlayZoneThreshold))
    {
      ref godot_variant local = ref value;
      float playZoneThreshold = this.PlayZoneThreshold;
      godot_variant from = VariantUtils.CreateFrom<float>(ref playZoneThreshold);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NMouseCardPlay.PropertyName.CancelZoneThreshold))
    {
      ref godot_variant local = ref value;
      float cancelZoneThreshold = this.CancelZoneThreshold;
      godot_variant from = VariantUtils.CreateFrom<float>(ref cancelZoneThreshold);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NMouseCardPlay.PropertyName._hasLeftCardCancelZoneOnce))
    {
      value = VariantUtils.CreateFrom<bool>(ref this._hasLeftCardCancelZoneOnce);
      return true;
    }
    if (StringName.op_Equality(ref name, NMouseCardPlay.PropertyName._dragStartYPosition))
    {
      value = VariantUtils.CreateFrom<float>(ref this._dragStartYPosition);
      return true;
    }
    if (StringName.op_Equality(ref name, NMouseCardPlay.PropertyName._isLeftMouseDown))
    {
      value = VariantUtils.CreateFrom<bool>(ref this._isLeftMouseDown);
      return true;
    }
    if (StringName.op_Equality(ref name, NMouseCardPlay.PropertyName._onCreatureHoverCallable))
    {
      value = VariantUtils.CreateFrom<Callable>(ref this._onCreatureHoverCallable);
      return true;
    }
    if (StringName.op_Equality(ref name, NMouseCardPlay.PropertyName._onCreatureUnhoverCallable))
    {
      value = VariantUtils.CreateFrom<Callable>(ref this._onCreatureUnhoverCallable);
      return true;
    }
    if (StringName.op_Equality(ref name, NMouseCardPlay.PropertyName._signalsConnected))
    {
      value = VariantUtils.CreateFrom<bool>(ref this._signalsConnected);
      return true;
    }
    if (StringName.op_Equality(ref name, NMouseCardPlay.PropertyName._cancelShortcut))
    {
      value = VariantUtils.CreateFrom<StringName>(ref this._cancelShortcut);
      return true;
    }
    if (!StringName.op_Equality(ref name, NMouseCardPlay.PropertyName._skipStartCardDrag))
      return base.GetGodotClassPropertyValue(in name, out value);
    value = VariantUtils.CreateFrom<bool>(ref this._skipStartCardDrag);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 1L, NMouseCardPlay.PropertyName._hasLeftCardCancelZoneOnce, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 3L, NMouseCardPlay.PropertyName.PlayZoneThreshold, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 3L, NMouseCardPlay.PropertyName.CancelZoneThreshold, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 3L, NMouseCardPlay.PropertyName._dragStartYPosition, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 1L, NMouseCardPlay.PropertyName._isLeftMouseDown, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 25L, NMouseCardPlay.PropertyName._onCreatureHoverCallable, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 25L, NMouseCardPlay.PropertyName._onCreatureUnhoverCallable, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 1L, NMouseCardPlay.PropertyName._signalsConnected, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 21L, NMouseCardPlay.PropertyName._cancelShortcut, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 1L, NMouseCardPlay.PropertyName._skipStartCardDrag, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    base.SaveGodotObjectData(info);
    info.AddProperty(NMouseCardPlay.PropertyName._hasLeftCardCancelZoneOnce, Variant.From<bool>(ref this._hasLeftCardCancelZoneOnce));
    info.AddProperty(NMouseCardPlay.PropertyName._dragStartYPosition, Variant.From<float>(ref this._dragStartYPosition));
    info.AddProperty(NMouseCardPlay.PropertyName._isLeftMouseDown, Variant.From<bool>(ref this._isLeftMouseDown));
    info.AddProperty(NMouseCardPlay.PropertyName._onCreatureHoverCallable, Variant.From<Callable>(ref this._onCreatureHoverCallable));
    info.AddProperty(NMouseCardPlay.PropertyName._onCreatureUnhoverCallable, Variant.From<Callable>(ref this._onCreatureUnhoverCallable));
    info.AddProperty(NMouseCardPlay.PropertyName._signalsConnected, Variant.From<bool>(ref this._signalsConnected));
    info.AddProperty(NMouseCardPlay.PropertyName._cancelShortcut, Variant.From<StringName>(ref this._cancelShortcut));
    info.AddProperty(NMouseCardPlay.PropertyName._skipStartCardDrag, Variant.From<bool>(ref this._skipStartCardDrag));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    base.RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NMouseCardPlay.PropertyName._hasLeftCardCancelZoneOnce, ref variant1))
      this._hasLeftCardCancelZoneOnce = ((Variant) ref variant1).As<bool>();
    Variant variant2;
    if (info.TryGetProperty(NMouseCardPlay.PropertyName._dragStartYPosition, ref variant2))
      this._dragStartYPosition = ((Variant) ref variant2).As<float>();
    Variant variant3;
    if (info.TryGetProperty(NMouseCardPlay.PropertyName._isLeftMouseDown, ref variant3))
      this._isLeftMouseDown = ((Variant) ref variant3).As<bool>();
    Variant variant4;
    if (info.TryGetProperty(NMouseCardPlay.PropertyName._onCreatureHoverCallable, ref variant4))
      this._onCreatureHoverCallable = ((Variant) ref variant4).As<Callable>();
    Variant variant5;
    if (info.TryGetProperty(NMouseCardPlay.PropertyName._onCreatureUnhoverCallable, ref variant5))
      this._onCreatureUnhoverCallable = ((Variant) ref variant5).As<Callable>();
    Variant variant6;
    if (info.TryGetProperty(NMouseCardPlay.PropertyName._signalsConnected, ref variant6))
      this._signalsConnected = ((Variant) ref variant6).As<bool>();
    Variant variant7;
    if (info.TryGetProperty(NMouseCardPlay.PropertyName._cancelShortcut, ref variant7))
      this._cancelShortcut = ((Variant) ref variant7).As<StringName>();
    Variant variant8;
    if (!info.TryGetProperty(NMouseCardPlay.PropertyName._skipStartCardDrag, ref variant8))
      return;
    this._skipStartCardDrag = ((Variant) ref variant8).As<bool>();
  }

  public new class MethodName : NCardPlay.MethodName
  {
    public static readonly StringName Create = StringName.op_Implicit(nameof (Create));
    public static readonly StringName _Input = StringName.op_Implicit(nameof (_Input));
    public new static readonly StringName Start = StringName.op_Implicit(nameof (Start));
    public static readonly StringName _EnterTree = StringName.op_Implicit(nameof (_EnterTree));
    public static readonly StringName _ExitTree = StringName.op_Implicit(nameof (_ExitTree));
    public static readonly StringName DisconnectTargetingSignals = StringName.op_Implicit(nameof (DisconnectTargetingSignals));
    public new static readonly StringName OnCancelPlayCard = StringName.op_Implicit(nameof (OnCancelPlayCard));
    public static readonly StringName IsCardInPlayZone = StringName.op_Implicit(nameof (IsCardInPlayZone));
    public static readonly StringName IsCardInCancelZone = StringName.op_Implicit(nameof (IsCardInCancelZone));
  }

  public new class PropertyName : NCardPlay.PropertyName
  {
    public static readonly StringName PlayZoneThreshold = StringName.op_Implicit(nameof (PlayZoneThreshold));
    public static readonly StringName CancelZoneThreshold = StringName.op_Implicit(nameof (CancelZoneThreshold));
    public static readonly StringName _hasLeftCardCancelZoneOnce = StringName.op_Implicit(nameof (_hasLeftCardCancelZoneOnce));
    public static readonly StringName _dragStartYPosition = StringName.op_Implicit(nameof (_dragStartYPosition));
    public static readonly StringName _isLeftMouseDown = StringName.op_Implicit(nameof (_isLeftMouseDown));
    public static readonly StringName _onCreatureHoverCallable = StringName.op_Implicit(nameof (_onCreatureHoverCallable));
    public static readonly StringName _onCreatureUnhoverCallable = StringName.op_Implicit(nameof (_onCreatureUnhoverCallable));
    public static readonly StringName _signalsConnected = StringName.op_Implicit(nameof (_signalsConnected));
    public static readonly StringName _cancelShortcut = StringName.op_Implicit(nameof (_cancelShortcut));
    public static readonly StringName _skipStartCardDrag = StringName.op_Implicit(nameof (_skipStartCardDrag));
  }

  public new class SignalName : NCardPlay.SignalName
  {
  }
}
