// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Potions.NPotionPopup
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.ControllerInput;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Potions;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.GameActions;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.HoverTips;
using MegaCrit.Sts2.Core.Nodes.Screens.Capstones;
using MegaCrit.Sts2.Core.Nodes.Screens.CardSelection;
using MegaCrit.Sts2.Core.Nodes.Screens.Overlays;
using MegaCrit.Sts2.Core.Runs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Potions;

[ScriptPath("res://src/Core/Nodes/Potions/NPotionPopup.cs")]
public class NPotionPopup : Control
{
  private NPotionHolder _holder;
  private Control _popupContainer;
  private NPotionPopupButton _useButton;
  private NPotionPopupButton _discardButton;
  private Control _hoverTipBounds;
  private Tween? _tween;
  private Player? _subscribedPlayer;

  private PotionModel? Potion => this._holder.Potion?.Model;

  public bool IsUsable => this._useButton.IsEnabled;

  public bool IsMarkedForRemoval { get; private set; }

  private static string ScenePath => SceneHelper.GetScenePath("potions/potion_popup");

  public static IEnumerable<string> AssetPaths
  {
    get
    {
      return (IEnumerable<string>) new \u003C\u003Ez__ReadOnlySingleElementList<string>(NPotionPopup.ScenePath);
    }
  }

  public static NPotionPopup Create(NPotionHolder holder)
  {
    NPotionPopup npotionPopup = PreloadManager.Cache.GetScene(NPotionPopup.ScenePath).Instantiate<NPotionPopup>((PackedScene.GenEditState) 0L);
    npotionPopup._holder = holder;
    return npotionPopup;
  }

  public override void _Ready()
  {
    this.GlobalPosition = Vector2.op_Addition(Vector2.op_Addition(Vector2.op_Addition(this._holder.GlobalPosition, Vector2.op_Multiply(Vector2.op_Multiply(Vector2.Down, this._holder.Size.Y), 1.5f)), Vector2.op_Multiply(Vector2.op_Multiply(Vector2.Right, this._holder.Size), 0.5f)), Vector2.op_Multiply(Vector2.op_Multiply(Vector2.Left, this.Size), 0.5f));
    this._hoverTipBounds = ((Node) this).GetNode<Control>(NodePath.op_Implicit("%HoverTipBounds"));
    NHoverTipSet.CreateAndShow(this._hoverTipBounds, this._holder.Potion.Model.HoverTips, HoverTipAlignment.Right);
    NHoverTipSet.shouldBlockHoverTips = true;
    this._popupContainer = ((Node) this).GetNode<Control>(NodePath.op_Implicit("%Container"));
    this._useButton = ((Node) this).GetNode<NPotionPopupButton>(NodePath.op_Implicit("%UseButton"));
    ((GodotObject) this._useButton).Connect(NClickableControl.SignalName.Released, Callable.From<NButton>(new Action<NButton>(this.OnUseButtonPressed)), 0U);
    this._discardButton = ((Node) this).GetNode<NPotionPopupButton>(NodePath.op_Implicit("%DiscardButton"));
    this._discardButton.SetLocKey("POTION_POPUP.discard");
    ((GodotObject) this._discardButton).Connect(NClickableControl.SignalName.Released, Callable.From<NButton>(new Action<NButton>(this.OnDiscardButtonPressed)), 0U);
    this._useButton.FocusNeighborLeft = ((Node) this._useButton).GetPath();
    this._useButton.FocusNeighborRight = ((Node) this._useButton).GetPath();
    this._useButton.FocusNeighborTop = ((Node) this._useButton).GetPath();
    this._useButton.FocusNeighborBottom = ((Node) this._discardButton).GetPath();
    this._discardButton.FocusNeighborLeft = ((Node) this._discardButton).GetPath();
    this._discardButton.FocusNeighborRight = ((Node) this._discardButton).GetPath();
    this._discardButton.FocusNeighborTop = ((Node) this._useButton).GetPath();
    this._discardButton.FocusNeighborBottom = ((Node) this._discardButton).GetPath();
    if (this.Potion == null || this.Potion.IsQueued || this.Potion.Owner.Creature.IsDead)
    {
      this._useButton.Disable();
      this._discardButton.Disable();
    }
    else
    {
      switch (this.Potion.Usage)
      {
        case PotionUsage.None:
          throw new InvalidOperationException("No potions should have 'None' usage.");
        case PotionUsage.CombatOnly:
          CombatManager.Instance.StateTracker.CombatStateChanged += new Action<CombatState>(this.OnCombatStateChanged);
          CombatManager.Instance.TurnStarted += new Action<CombatState>(this.OnTurnStarted);
          CombatManager.Instance.PlayerEndedTurn += new Action<Player, bool>(this.OnPlayerEndTurnStatusChanged);
          CombatManager.Instance.PlayerUnendedTurn += new Action<Player>(this.OnPlayerEndTurnStatusChanged);
          if (NOverlayStack.Instance != null)
            NOverlayStack.Instance.Changed += new NOverlayStack.ChangedEventHandler(this.Remove);
          else
            Log.Warn("NOverlayStack.Instance was null when creating potion popup");
          if (NCapstoneContainer.Instance != null)
            NCapstoneContainer.Instance.Changed += new NCapstoneContainer.ChangedEventHandler(this.Remove);
          else
            Log.Warn("NCapstoneContainer.Instance was null when creating potion popup");
          this.RefreshButtons();
          break;
        case PotionUsage.AnyTime:
          this._useButton.Enable();
          break;
        case PotionUsage.Automatic:
          this._useButton.Disable();
          break;
        default:
          throw new ArgumentOutOfRangeException("Usage");
      }
      this._subscribedPlayer = this.Potion.Owner;
      this._subscribedPlayer.CanUseOrRemovePotionsChanged += new Action(this.RefreshButtons);
      if (!this.Potion.Owner.CanUseOrRemovePotions)
      {
        this._useButton.Disable();
        this._discardButton.Disable();
      }
      if (!this.Potion.PassesCustomUsabilityCheck)
        this._useButton.Disable();
      if (this._useButton.IsEnabled)
        this._useButton.TryGrabFocus();
      else if (this._discardButton.IsEnabled)
        this._discardButton.TryGrabFocus();
      else
        this.TryGrabFocus();
    }
    string locEntryKey;
    if (this.Potion == null)
    {
      locEntryKey = "POTION_POPUP.drink";
    }
    else
    {
      bool flag;
      switch (this.Potion.TargetType)
      {
        case TargetType.AnyEnemy:
        case TargetType.AllEnemies:
        case TargetType.TargetedNoCreature:
          flag = true;
          break;
        default:
          flag = false;
          break;
      }
      locEntryKey = flag || this.Potion.CanThrowAtAlly() ? "POTION_POPUP.throw" : "POTION_POPUP.drink";
    }
    this._useButton.SetLocKey(locEntryKey);
    this._tween?.Kill();
    ((CanvasItem) this).Modulate = Colors.Transparent;
    Control popupContainer = this._popupContainer;
    popupContainer.Position = Vector2.op_Addition(popupContainer.Position, Vector2.op_Multiply(Vector2.Up, 25f));
    this._tween = ((Node) this).CreateTween().SetParallel(true);
    this._tween.TweenProperty((GodotObject) this, NodePath.op_Implicit("modulate"), Variant.op_Implicit(Colors.White), 0.10000000149011612).SetTrans((Tween.TransitionType) 1L);
    this._tween.TweenProperty((GodotObject) this._popupContainer, NodePath.op_Implicit("position:y"), Variant.op_Implicit(this._popupContainer.Position.Y + 25f), 0.15000000596046448).SetEase((Tween.EaseType) 0L).SetTrans((Tween.TransitionType) 1L);
  }

  private void OnUseButtonPressed(NButton _)
  {
    TaskHelper.RunSafely(this.UsePotion());
    this.Remove();
  }

  private async Task UsePotion()
  {
    PotionModel potion;
    if (this.Potion == null)
    {
      potion = (PotionModel) null;
    }
    else
    {
      potion = this.Potion;
      potion.BeforeUse += new Action(DisableHolder);
      try
      {
        await this._holder.UsePotion();
        potion = (PotionModel) null;
      }
      finally
      {
        potion.BeforeUse -= new Action(DisableHolder);
      }
    }

    void DisableHolder() => this._holder.DisableUntilPotionRemoved();
  }

  private void OnDiscardButtonPressed(NButton _)
  {
    if (this.Potion == null)
      return;
    Player owner = this.Potion.Owner;
    int potionSlotIndex = owner.PotionSlots.IndexOf<PotionModel>(this._holder.Potion.Model);
    if (potionSlotIndex < 0)
      throw new InvalidOperationException($"Tried to discard potion {this._holder.Potion.Model} but it's not in the player's belt!");
    this._holder.DisableUntilPotionRemoved();
    RunManager.Instance.ActionQueueSynchronizer.RequestEnqueue((GameAction) new DiscardPotionGameAction(owner, (uint) potionSlotIndex, CombatManager.Instance.IsInProgress));
  }

  public override void _Input(InputEvent inputEvent)
  {
    if (inputEvent is InputEventMouseButton eventMouseButton)
    {
      if (eventMouseButton.ButtonIndex - 1L > 1L || !((InputEvent) eventMouseButton).IsReleased())
        return;
      Rect2 globalRect = this.GetGlobalRect();
      if (((Rect2) ref globalRect).HasPoint(((CanvasItem) this).GetGlobalMousePosition()))
        return;
      this.Remove();
    }
    else
    {
      if (!inputEvent.IsActionPressed(MegaInput.cancel, false, false))
        return;
      this.Remove();
      ((Node) this).GetViewport()?.SetInputAsHandled();
      this._holder.TryGrabFocus();
    }
  }

  public override void _ExitTree()
  {
    if (!this.IsMarkedForRemoval)
    {
      this.IsMarkedForRemoval = true;
      NHoverTipSet.shouldBlockHoverTips = false;
      NHoverTipSet.Remove(this._hoverTipBounds);
      this.DisconnectSignals();
    }
    this._tween?.Kill();
  }

  public void Remove()
  {
    if (this.IsMarkedForRemoval)
      return;
    this.IsMarkedForRemoval = true;
    NHoverTipSet.shouldBlockHoverTips = false;
    NHoverTipSet.Remove(this._hoverTipBounds);
    this.DisconnectSignals();
    this._useButton.Disable();
    this._discardButton.Disable();
    this._tween?.Kill();
    this._tween = ((Node) this).CreateTween().SetParallel(true);
    this._tween.TweenProperty((GodotObject) this, NodePath.op_Implicit("modulate"), Variant.op_Implicit(Colors.Transparent), 0.10000000149011612).SetTrans((Tween.TransitionType) 1L);
    this._tween.TweenProperty((GodotObject) this._popupContainer, NodePath.op_Implicit("position:y"), Variant.op_Implicit(-25f), 0.20000000298023224).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 1L);
    this._tween.Chain().TweenCallback(Callable.From(new Action(((GodotTreeExtensions) this).QueueFreeSafely)));
  }

  private void DisconnectSignals()
  {
    CombatManager.Instance.StateTracker.CombatStateChanged -= new Action<CombatState>(this.OnCombatStateChanged);
    CombatManager.Instance.TurnStarted -= new Action<CombatState>(this.OnTurnStarted);
    CombatManager.Instance.PlayerEndedTurn -= new Action<Player, bool>(this.OnPlayerEndTurnStatusChanged);
    CombatManager.Instance.PlayerUnendedTurn -= new Action<Player>(this.OnPlayerEndTurnStatusChanged);
    if (this._subscribedPlayer != null)
      this._subscribedPlayer.CanUseOrRemovePotionsChanged -= new Action(this.RefreshButtons);
    if (NOverlayStack.Instance != null)
      NOverlayStack.Instance.Changed -= new NOverlayStack.ChangedEventHandler(this.Remove);
    if (NCapstoneContainer.Instance == null)
      return;
    NCapstoneContainer.Instance.Changed -= new NCapstoneContainer.ChangedEventHandler(this.Remove);
  }

  private void OnTurnStarted(CombatState _) => this.RefreshButtons();

  private void OnPlayerEndTurnStatusChanged(Player _, bool __) => this.RefreshButtons();

  private void OnPlayerEndTurnStatusChanged(Player _) => this.RefreshButtons();

  private void OnCombatStateChanged(CombatState _) => this.RefreshButtons();

  private void RefreshButtons()
  {
    if (this.IsMarkedForRemoval)
      return;
    Creature creature = this.Potion?.Owner.Creature;
    this._discardButton.Enable();
    PotionModel potion1 = this.Potion;
    if ((potion1 != null ? (potion1.Usage == PotionUsage.AnyTime ? 1 : 0) : 0) != 0)
    {
      this._useButton.Enable();
    }
    else
    {
      if (creature != null && CombatManager.Instance.IsInProgress)
      {
        CombatSide? currentSide = creature.CombatState?.CurrentSide;
        CombatSide side = creature.Side;
        if (currentSide.GetValueOrDefault() == side & currentSide.HasValue && creature.IsAlive && !this.InACardSelectScreen && !CombatManager.Instance.PlayerActionsDisabled)
        {
          this._useButton.Enable();
          goto label_8;
        }
      }
      this._useButton.Disable();
    }
label_8:
    PotionModel potion2 = this.Potion;
    if (potion2 == null)
      return;
    Player owner = potion2.Owner;
    if (owner == null || owner.CanUseOrRemovePotions)
      return;
    this._useButton.Disable();
    this._discardButton.Disable();
  }

  private bool InACardSelectScreen
  {
    get
    {
      NPlayerHand instance = NPlayerHand.Instance;
      return (instance != null ? (instance.IsInCardSelection ? 1 : 0) : 0) != 0 || NOverlayStack.Instance?.Peek() is ICardSelector;
    }
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(9)
    {
      new MethodInfo(NPotionPopup.MethodName.Create, new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false), (MethodFlags) 33L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("holder"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false)
      }, (List<Variant>) null),
      new MethodInfo(NPotionPopup.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NPotionPopup.MethodName.OnUseButtonPressed, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("_"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false)
      }, (List<Variant>) null),
      new MethodInfo(NPotionPopup.MethodName.OnDiscardButtonPressed, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("_"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false)
      }, (List<Variant>) null),
      new MethodInfo(NPotionPopup.MethodName._Input, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("inputEvent"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("InputEvent"), false)
      }, (List<Variant>) null),
      new MethodInfo(NPotionPopup.MethodName._ExitTree, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NPotionPopup.MethodName.Remove, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NPotionPopup.MethodName.DisconnectSignals, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NPotionPopup.MethodName.RefreshButtons, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NPotionPopup.MethodName.Create) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      NPotionPopup npotionPopup = NPotionPopup.Create(VariantUtils.ConvertTo<NPotionHolder>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = VariantUtils.CreateFrom<NPotionPopup>(ref npotionPopup);
      return true;
    }
    if (StringName.op_Equality(ref method, NPotionPopup.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NPotionPopup.MethodName.OnUseButtonPressed) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.OnUseButtonPressed(VariantUtils.ConvertTo<NButton>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NPotionPopup.MethodName.OnDiscardButtonPressed) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.OnDiscardButtonPressed(VariantUtils.ConvertTo<NButton>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NPotionPopup.MethodName._Input) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      ((Node) this)._Input(VariantUtils.ConvertTo<InputEvent>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NPotionPopup.MethodName._ExitTree) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._ExitTree();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NPotionPopup.MethodName.Remove) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.Remove();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NPotionPopup.MethodName.DisconnectSignals) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.DisconnectSignals();
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NPotionPopup.MethodName.RefreshButtons) || ((NativeVariantPtrArgs) ref args).Count != 0)
      return base.InvokeGodotClassMethod(ref method, args, ref ret);
    this.RefreshButtons();
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static bool InvokeGodotClassStaticMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NPotionPopup.MethodName.Create) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      NPotionPopup npotionPopup = NPotionPopup.Create(VariantUtils.ConvertTo<NPotionHolder>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = VariantUtils.CreateFrom<NPotionPopup>(ref npotionPopup);
      return true;
    }
    ret = new godot_variant();
    return false;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NPotionPopup.MethodName.Create) || StringName.op_Equality(ref method, NPotionPopup.MethodName._Ready) || StringName.op_Equality(ref method, NPotionPopup.MethodName.OnUseButtonPressed) || StringName.op_Equality(ref method, NPotionPopup.MethodName.OnDiscardButtonPressed) || StringName.op_Equality(ref method, NPotionPopup.MethodName._Input) || StringName.op_Equality(ref method, NPotionPopup.MethodName._ExitTree) || StringName.op_Equality(ref method, NPotionPopup.MethodName.Remove) || StringName.op_Equality(ref method, NPotionPopup.MethodName.DisconnectSignals) || StringName.op_Equality(ref method, NPotionPopup.MethodName.RefreshButtons) || base.HasGodotClassMethod(ref method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NPotionPopup.PropertyName.IsMarkedForRemoval))
    {
      this.IsMarkedForRemoval = VariantUtils.ConvertTo<bool>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NPotionPopup.PropertyName._holder))
    {
      this._holder = VariantUtils.ConvertTo<NPotionHolder>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NPotionPopup.PropertyName._popupContainer))
    {
      this._popupContainer = VariantUtils.ConvertTo<Control>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NPotionPopup.PropertyName._useButton))
    {
      this._useButton = VariantUtils.ConvertTo<NPotionPopupButton>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NPotionPopup.PropertyName._discardButton))
    {
      this._discardButton = VariantUtils.ConvertTo<NPotionPopupButton>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NPotionPopup.PropertyName._hoverTipBounds))
    {
      this._hoverTipBounds = VariantUtils.ConvertTo<Control>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NPotionPopup.PropertyName._tween))
      return ((GodotObject) this).SetGodotClassPropertyValue(ref name, ref value);
    this._tween = VariantUtils.ConvertTo<Tween>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NPotionPopup.PropertyName.IsUsable))
    {
      ref godot_variant local = ref value;
      bool isUsable = this.IsUsable;
      godot_variant from = VariantUtils.CreateFrom<bool>(ref isUsable);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NPotionPopup.PropertyName.IsMarkedForRemoval))
    {
      ref godot_variant local = ref value;
      bool markedForRemoval = this.IsMarkedForRemoval;
      godot_variant from = VariantUtils.CreateFrom<bool>(ref markedForRemoval);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NPotionPopup.PropertyName.InACardSelectScreen))
    {
      ref godot_variant local = ref value;
      bool acardSelectScreen = this.InACardSelectScreen;
      godot_variant from = VariantUtils.CreateFrom<bool>(ref acardSelectScreen);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NPotionPopup.PropertyName._holder))
    {
      value = VariantUtils.CreateFrom<NPotionHolder>(ref this._holder);
      return true;
    }
    if (StringName.op_Equality(ref name, NPotionPopup.PropertyName._popupContainer))
    {
      value = VariantUtils.CreateFrom<Control>(ref this._popupContainer);
      return true;
    }
    if (StringName.op_Equality(ref name, NPotionPopup.PropertyName._useButton))
    {
      value = VariantUtils.CreateFrom<NPotionPopupButton>(ref this._useButton);
      return true;
    }
    if (StringName.op_Equality(ref name, NPotionPopup.PropertyName._discardButton))
    {
      value = VariantUtils.CreateFrom<NPotionPopupButton>(ref this._discardButton);
      return true;
    }
    if (StringName.op_Equality(ref name, NPotionPopup.PropertyName._hoverTipBounds))
    {
      value = VariantUtils.CreateFrom<Control>(ref this._hoverTipBounds);
      return true;
    }
    if (!StringName.op_Equality(ref name, NPotionPopup.PropertyName._tween))
      return ((GodotObject) this).GetGodotClassPropertyValue(ref name, ref value);
    value = VariantUtils.CreateFrom<Tween>(ref this._tween);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NPotionPopup.PropertyName._holder, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NPotionPopup.PropertyName._popupContainer, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NPotionPopup.PropertyName._useButton, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NPotionPopup.PropertyName._discardButton, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NPotionPopup.PropertyName._hoverTipBounds, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NPotionPopup.PropertyName._tween, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 1L, NPotionPopup.PropertyName.IsUsable, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 1L, NPotionPopup.PropertyName.IsMarkedForRemoval, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 1L, NPotionPopup.PropertyName.InACardSelectScreen, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).SaveGodotObjectData(info);
    GodotSerializationInfo serializationInfo = info;
    StringName markedForRemoval1 = NPotionPopup.PropertyName.IsMarkedForRemoval;
    bool markedForRemoval2 = this.IsMarkedForRemoval;
    Variant variant = Variant.From<bool>(ref markedForRemoval2);
    serializationInfo.AddProperty(markedForRemoval1, variant);
    info.AddProperty(NPotionPopup.PropertyName._holder, Variant.From<NPotionHolder>(ref this._holder));
    info.AddProperty(NPotionPopup.PropertyName._popupContainer, Variant.From<Control>(ref this._popupContainer));
    info.AddProperty(NPotionPopup.PropertyName._useButton, Variant.From<NPotionPopupButton>(ref this._useButton));
    info.AddProperty(NPotionPopup.PropertyName._discardButton, Variant.From<NPotionPopupButton>(ref this._discardButton));
    info.AddProperty(NPotionPopup.PropertyName._hoverTipBounds, Variant.From<Control>(ref this._hoverTipBounds));
    info.AddProperty(NPotionPopup.PropertyName._tween, Variant.From<Tween>(ref this._tween));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NPotionPopup.PropertyName.IsMarkedForRemoval, ref variant1))
      this.IsMarkedForRemoval = ((Variant) ref variant1).As<bool>();
    Variant variant2;
    if (info.TryGetProperty(NPotionPopup.PropertyName._holder, ref variant2))
      this._holder = ((Variant) ref variant2).As<NPotionHolder>();
    Variant variant3;
    if (info.TryGetProperty(NPotionPopup.PropertyName._popupContainer, ref variant3))
      this._popupContainer = ((Variant) ref variant3).As<Control>();
    Variant variant4;
    if (info.TryGetProperty(NPotionPopup.PropertyName._useButton, ref variant4))
      this._useButton = ((Variant) ref variant4).As<NPotionPopupButton>();
    Variant variant5;
    if (info.TryGetProperty(NPotionPopup.PropertyName._discardButton, ref variant5))
      this._discardButton = ((Variant) ref variant5).As<NPotionPopupButton>();
    Variant variant6;
    if (info.TryGetProperty(NPotionPopup.PropertyName._hoverTipBounds, ref variant6))
      this._hoverTipBounds = ((Variant) ref variant6).As<Control>();
    Variant variant7;
    if (!info.TryGetProperty(NPotionPopup.PropertyName._tween, ref variant7))
      return;
    this._tween = ((Variant) ref variant7).As<Tween>();
  }

  public class MethodName : Control.MethodName
  {
    public static readonly StringName Create = StringName.op_Implicit(nameof (Create));
    public static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName OnUseButtonPressed = StringName.op_Implicit(nameof (OnUseButtonPressed));
    public static readonly StringName OnDiscardButtonPressed = StringName.op_Implicit(nameof (OnDiscardButtonPressed));
    public static readonly StringName _Input = StringName.op_Implicit(nameof (_Input));
    public static readonly StringName _ExitTree = StringName.op_Implicit(nameof (_ExitTree));
    public static readonly StringName Remove = StringName.op_Implicit(nameof (Remove));
    public static readonly StringName DisconnectSignals = StringName.op_Implicit(nameof (DisconnectSignals));
    public static readonly StringName RefreshButtons = StringName.op_Implicit(nameof (RefreshButtons));
  }

  public class PropertyName : Control.PropertyName
  {
    public static readonly StringName IsUsable = StringName.op_Implicit(nameof (IsUsable));
    public static readonly StringName IsMarkedForRemoval = StringName.op_Implicit(nameof (IsMarkedForRemoval));
    public static readonly StringName InACardSelectScreen = StringName.op_Implicit(nameof (InACardSelectScreen));
    public static readonly StringName _holder = StringName.op_Implicit(nameof (_holder));
    public static readonly StringName _popupContainer = StringName.op_Implicit(nameof (_popupContainer));
    public static readonly StringName _useButton = StringName.op_Implicit(nameof (_useButton));
    public static readonly StringName _discardButton = StringName.op_Implicit(nameof (_discardButton));
    public static readonly StringName _hoverTipBounds = StringName.op_Implicit(nameof (_hoverTipBounds));
    public static readonly StringName _tween = StringName.op_Implicit(nameof (_tween));
  }

  public class SignalName : Control.SignalName
  {
  }
}
