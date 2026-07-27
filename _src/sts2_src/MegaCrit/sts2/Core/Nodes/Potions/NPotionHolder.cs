// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Potions.NPotionHolder
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Audio.Debug;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Models.Potions;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.HoverTips;
using MegaCrit.Sts2.Core.Nodes.Multiplayer;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Screens.Capstones;
using MegaCrit.Sts2.Core.Nodes.Screens.Overlays;
using MegaCrit.Sts2.Core.Random;
using MegaCrit.Sts2.Core.Runs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Potions;

[ScriptPath("res://src/Core/Nodes/Potions/NPotionHolder.cs")]
public class NPotionHolder : NClickableControl
{
  private Vector2 _potionScale = Vector2.op_Multiply(0.9f, Vector2.One);
  private TextureRect _emptyIcon;
  private NSelectionReticle _selectionReticle;
  private NPotionPopup? _popup;
  private bool _potionTargeting;
  private bool _isUsable;
  private Tween? _emptyPotionTween;
  private Tween? _hoverTween;
  private bool _disabledUntilPotionRemoved;
  private bool _isFocused;
  private CancellationTokenSource _cts = new CancellationTokenSource();
  private CancellationTokenSource? _cancelGrayOutPotionSource;

  private static HoverTip EmptyHoverTip
  {
    get
    {
      return new HoverTip(new LocString("static_hover_tips", "POTION_SLOT.title"), new LocString("static_hover_tips", "POTION_SLOT.description"));
    }
  }

  public NPotion? Potion { get; private set; }

  public bool HasPotion => this.Potion != null;

  public bool IsPotionUsable => this._popup.IsUsable;

  private static string ScenePath => SceneHelper.GetScenePath("/potions/potion_holder");

  public static IEnumerable<string> AssetPaths
  {
    get
    {
      return (IEnumerable<string>) new \u003C\u003Ez__ReadOnlySingleElementList<string>(NPotionHolder.ScenePath);
    }
  }

  public static NPotionHolder Create(bool isUsable)
  {
    NPotionHolder npotionHolder = PreloadManager.Cache.GetScene(NPotionHolder.ScenePath).Instantiate<NPotionHolder>((PackedScene.GenEditState) 0L);
    npotionHolder._isUsable = isUsable;
    return npotionHolder;
  }

  public override void _EnterTree() => this._cts = new CancellationTokenSource();

  public override void _Ready()
  {
    this._emptyIcon = ((Node) this).GetNode<TextureRect>(NodePath.op_Implicit("%EmptyIcon"));
    this._selectionReticle = ((Node) this).GetNode<NSelectionReticle>(NodePath.op_Implicit("%SelectionReticle"));
    this.ConnectSignals();
  }

  public override void _ExitTree()
  {
    this._cancelGrayOutPotionSource?.Cancel();
    this._cts.Cancel();
  }

  protected override void OnFocus()
  {
    if (this._isFocused)
      return;
    this._isFocused = true;
    this._hoverTween?.Kill();
    this._hoverTween = ((Node) this).CreateTween().SetParallel(true);
    if (this.Potion != null)
    {
      this.Potion.DoBounce();
      this._hoverTween.TweenProperty((GodotObject) this.Potion, NodePath.op_Implicit("scale"), Variant.op_Implicit(Vector2.op_Multiply(this._potionScale, 1.15f)), 0.05);
      NDebugAudioManager.Instance?.Play(Rng.Chaotic.NextItem<string>(TmpSfx.PotionSlosh), 0.5f, PitchVariance.Large);
      if (!GodotObject.IsInstanceValid((GodotObject) this._popup) || this._popup.IsMarkedForRemoval)
        NHoverTipSet.CreateAndShow((Control) this, this.Potion.Model.HoverTips, HoverTipAlignment.Center)?.SetGlobalPosition(Vector2.op_Addition(this.GlobalPosition, Vector2.op_Multiply(Vector2.op_Multiply(Vector2.Down, this.Size.Y), Mathf.Max(1.5f, this.Scale.Y))), false);
    }
    else
    {
      this._hoverTween.TweenProperty((GodotObject) this._emptyIcon, NodePath.op_Implicit("scale"), Variant.op_Implicit(Vector2.op_Multiply(this._potionScale, 1.15f)), 0.05);
      NHoverTipSet andShow = NHoverTipSet.CreateAndShow((Control) this, (IHoverTip) NPotionHolder.EmptyHoverTip);
      andShow?.SetGlobalPosition(Vector2.op_Addition(this.GlobalPosition, Vector2.op_Multiply(Vector2.op_Multiply(Vector2.Down, this.Size.Y), Mathf.Max(1.5f, this.Scale.Y))), false);
      andShow?.SetAlignment((Control) this, HoverTipAlignment.Center);
    }
    if (!NControllerManager.Instance.IsUsingController)
      return;
    this._selectionReticle.OnSelect();
  }

  protected override void OnUnfocus()
  {
    this._isFocused = false;
    NHoverTipSet.Remove((Control) this);
    this._hoverTween?.Kill();
    if (this.Potion != null)
    {
      if (!this._disabledUntilPotionRemoved)
      {
        this._hoverTween = ((Node) this).CreateTween().SetParallel(true);
        this._hoverTween.TweenProperty((GodotObject) this.Potion, NodePath.op_Implicit("scale"), Variant.op_Implicit(this._potionScale), 0.5).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 5L);
      }
    }
    else
    {
      this._hoverTween = ((Node) this).CreateTween().SetParallel(true);
      this._hoverTween.TweenProperty((GodotObject) this._emptyIcon, NodePath.op_Implicit("scale"), Variant.op_Implicit(this._potionScale), 0.5).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 5L);
    }
    this._selectionReticle.OnDeselect();
  }

  protected override void OnPress()
  {
    if (this.Potion == null || !this._isUsable)
      return;
    ((Node) this).GetViewport().SetInputAsHandled();
  }

  protected override void OnRelease()
  {
    if (this._isUsable)
      this.OpenPotionPopup();
    if (this.Potion == null || !this._isUsable)
      return;
    ((Node) this).GetViewport().SetInputAsHandled();
  }

  private void OpenPotionPopup()
  {
    if (!this.HasPotion || this.Potion.Model.Owner.RunState.IsGameOver || this._disabledUntilPotionRemoved)
      return;
    NHoverTipSet.Remove((Control) this);
    this._popup = NPotionPopup.Create(this);
    ((Node) this).AddChildSafely((Node) this._popup);
  }

  public void AddPotion(NPotion potion)
  {
    this.Potion = this.Potion == null ? potion : throw new InvalidOperationException("Slot already contains a potion");
    this._emptyPotionTween?.Kill();
    ((CanvasItem) this._emptyIcon).Modulate = Colors.Transparent;
    ((Node) this).AddChildSafely((Node) this.Potion);
    this.Potion.Scale = this._potionScale;
    this.Potion.PivotOffset = Vector2.op_Multiply(this.Potion.Size, 0.5f);
  }

  public void DisableUntilPotionRemoved()
  {
    if (this._popup != null && GodotObject.IsInstanceValid((GodotObject) this._popup))
      this._popup.Remove();
    this._disabledUntilPotionRemoved = true;
    TaskHelper.RunSafely(this.GrayPotionHolderUntilPlayedAfterDelay());
    this.TryGrabFocus();
  }

  private async Task GrayPotionHolderUntilPlayedAfterDelay()
  {
    this._cancelGrayOutPotionSource = new CancellationTokenSource();
    await Task.Delay(100, this._cancelGrayOutPotionSource.Token);
    if (this._cancelGrayOutPotionSource.IsCancellationRequested)
      return;
    ((CanvasItem) this).Modulate = StsColors.gray;
  }

  public void CancelPotionUseOrDiscard()
  {
    this._cancelGrayOutPotionSource?.Cancel();
    this._disabledUntilPotionRemoved = false;
    ((CanvasItem) this).Modulate = Colors.White;
  }

  public void RemoveUsedPotion()
  {
    if (this.Potion == null)
      throw new InvalidOperationException("This slot doesn't contain a potion");
    if (this._popup != null && GodotObject.IsInstanceValid((GodotObject) this._popup))
      this._popup.Remove();
    NHoverTipSet.Remove((Control) this);
    this._disabledUntilPotionRemoved = false;
    this._cancelGrayOutPotionSource?.Cancel();
    ((CanvasItem) this).Modulate = Colors.White;
    NPotion potionToRemove = this.Potion;
    this.Potion = (NPotion) null;
    Tween tween = ((Node) this).CreateTween();
    tween.TweenProperty((GodotObject) potionToRemove, NodePath.op_Implicit("scale"), Variant.op_Implicit(Vector2.Zero), 0.20000000298023224).SetEase((Tween.EaseType) 0L).SetTrans((Tween.TransitionType) 10L).FromCurrent();
    tween.TweenCallback(Callable.From((Action) (() =>
    {
      ((Node) this).RemoveChildSafely((Node) potionToRemove);
      ((Node) potionToRemove).QueueFreeSafely();
    })));
    if (this.IsFocused)
    {
      NHoverTipSet andShow = NHoverTipSet.CreateAndShow((Control) this, (IHoverTip) NPotionHolder.EmptyHoverTip);
      andShow?.SetGlobalPosition(Vector2.op_Addition(this.GlobalPosition, Vector2.op_Multiply(Vector2.op_Multiply(Vector2.Down, this.Size.Y), 1.5f)), false);
      andShow?.SetAlignment((Control) this, HoverTipAlignment.Center);
    }
    this._emptyPotionTween?.Kill();
    this._emptyPotionTween = ((Node) this).CreateTween();
    this._emptyPotionTween.TweenProperty((GodotObject) this._emptyIcon, NodePath.op_Implicit("modulate"), Variant.op_Implicit(Colors.White), 0.20000000298023224).SetDelay(0.20000000298023224);
  }

  public void DiscardPotion()
  {
    if (this.Potion == null)
      throw new InvalidOperationException("This slot doesn't contain a potion");
    if (this._popup != null && GodotObject.IsInstanceValid((GodotObject) this._popup))
      this._popup.Remove();
    this._disabledUntilPotionRemoved = false;
    this._cancelGrayOutPotionSource?.Cancel();
    ((CanvasItem) this).Modulate = Colors.White;
    NPotion potionToRemove = this.Potion;
    this.Potion = (NPotion) null;
    Tween tween = ((Node) this).CreateTween();
    tween.TweenProperty((GodotObject) potionToRemove, NodePath.op_Implicit("position:y"), Variant.op_Implicit(-100f), 0.40000000596046448).SetEase((Tween.EaseType) 0L).SetTrans((Tween.TransitionType) 10L);
    tween.TweenCallback(Callable.From((Action) (() =>
    {
      ((Node) this).RemoveChildSafely((Node) potionToRemove);
      ((Node) potionToRemove).QueueFreeSafely();
    })));
    this._emptyPotionTween?.Kill();
    this._emptyPotionTween = ((Node) this).CreateTween();
    this._emptyPotionTween.TweenProperty((GodotObject) this._emptyIcon, NodePath.op_Implicit("modulate"), Variant.op_Implicit(Colors.White), 0.20000000298023224).FromCurrent().SetDelay(0.20000000298023224);
  }

  public async Task UsePotion()
  {
    if (this.Potion == null)
    {
      Log.Warn("Tried to use potion in holder, but potion node is null!");
    }
    else
    {
      bool flag;
      switch (this.Potion.Model.TargetType)
      {
        case TargetType.AnyEnemy:
        case TargetType.TargetedNoCreature:
          flag = true;
          break;
        default:
          flag = false;
          break;
      }
      if (flag || this.Potion.Model.CanThrowAtAlly())
      {
        RunManager.Instance.HoveredModelTracker.OnLocalPotionSelected(this.Potion.Model);
        await this.TargetNode(this.Potion.Model.TargetType);
        RunManager.Instance.HoveredModelTracker.OnLocalPotionDeselected();
      }
      else
      {
        this.Potion.Model.EnqueueManualUse(this.Potion.Model.TargetType == TargetType.Self ? this.Potion.Model.Owner.Creature : (Creature) null);
        this.TryGrabFocus();
      }
    }
  }

  private async Task TargetNode(TargetType targetType)
  {
    Vector2 startPosition = Vector2.op_Addition(Vector2.op_Addition(this.GlobalPosition, Vector2.op_Multiply(Vector2.op_Multiply(Vector2.Right, this.Size.X), 0.5f)), Vector2.op_Multiply(Vector2.Down, 50f));
    NTargetManager instance = NTargetManager.Instance;
    bool isUsingController = NControllerManager.Instance.IsUsingController;
    instance.StartTargeting(targetType, startPosition, isUsingController ? TargetMode.Controller : TargetMode.ClickMouseToTarget, new Func<bool>(this.ShouldCancelTargeting), (Func<Node, bool>) null);
    Creature creature = this.Potion.Model.Owner.Creature;
    if (isUsingController && CombatManager.Instance.IsInProgress)
    {
      ICombatState combatState = creature.CombatState;
      IReadOnlyList<Creature> source;
      switch (targetType)
      {
        case TargetType.AnyEnemy:
          source = combatState.GetOpponentsOf(creature);
          break;
        case TargetType.AnyPlayer:
          source = combatState.GetTeammatesOf(creature);
          break;
        default:
          throw new ArgumentOutOfRangeException(nameof (targetType), (object) targetType, (string) null);
      }
      List<Creature> list = source.Where<Creature>((Func<Creature, bool>) (c => c.IsAlive)).ToList<Creature>();
      NCombatRoom.Instance.RestrictControllerNavigation(list.Select<Creature, Control>((Func<Creature, Control>) (c => NCombatRoom.Instance.GetCreatureNode(c).Hitbox)));
      NCombatRoom.Instance.GetCreatureNode(list.First<Creature>()).Hitbox.TryGrabFocus();
    }
    else if (isUsingController && targetType == TargetType.AnyPlayer)
    {
      NMultiplayerPlayerStateContainer multiplayerPlayerContainer = NRun.Instance.GlobalUi.MultiplayerPlayerContainer;
      NMultiplayerPlayerState firstPlayerState = multiplayerPlayerContainer.FirstPlayerState;
      if (firstPlayerState != null)
        firstPlayerState.Hitbox.TryGrabFocus();
      multiplayerPlayerContainer.LockNavigation();
    }
    NMerchantButton merchantButton = (NMerchantButton) null;
    Control.FocusBehaviorRecursiveEnum? savedFocusBehavior = new Control.FocusBehaviorRecursiveEnum?();
    Control merchantScreenContext = (Control) null;
    bool merchantButtonWasDisabled = false;
    if (isUsingController && this.Potion.Model is FoulPotion)
    {
      (merchantButton, merchantScreenContext) = FoulPotion.GetFoulPotionMerchantTarget(creature.Player.RunState.CurrentRoom);
      if (merchantButton != null)
      {
        if (merchantScreenContext != null)
        {
          savedFocusBehavior = new Control.FocusBehaviorRecursiveEnum?(merchantScreenContext.FocusBehaviorRecursive);
          merchantScreenContext.FocusBehaviorRecursive = (Control.FocusBehaviorRecursiveEnum) 2L;
        }
        if (!merchantButton.IsEnabled)
        {
          merchantButtonWasDisabled = true;
          merchantButton.Enable();
        }
      }
    }
    merchantButton?.SetFocusMode((Control.FocusModeEnum) 2L);
    NMerchantButton control = merchantButton;
    if (control != null)
      control.TryGrabFocus();
    try
    {
      Node actualValue = await instance.SelectionFinished();
      NCombatRoom.Instance?.EnableControllerNavigation();
      NRun.Instance.GlobalUi.MultiplayerPlayerContainer.UnlockNavigation();
      Creature target;
      switch (actualValue)
      {
        case null:
          goto label_37;
        case NCreature ncreature:
          target = ncreature.Entity;
          break;
        case NMultiplayerPlayerState nmultiplayerPlayerState:
          target = nmultiplayerPlayerState.Player.Creature;
          break;
        case NMerchantButton _:
          target = (Creature) null;
          break;
        default:
          throw new ArgumentOutOfRangeException("targetNode", (object) actualValue, (string) null);
      }
      this.Potion.Model.EnqueueManualUse(target);
    }
    finally
    {
      merchantButton?.SetFocusMode((Control.FocusModeEnum) 0L);
      if (merchantButtonWasDisabled)
        merchantButton.Disable();
      if (merchantScreenContext != null && savedFocusBehavior.HasValue)
        merchantScreenContext.FocusBehaviorRecursive = savedFocusBehavior.Value;
    }
label_37:
    this.TryGrabFocus();
    merchantButton = (NMerchantButton) null;
    merchantScreenContext = (Control) null;
  }

  private bool ShouldCancelTargeting()
  {
    if (this.Potion == null)
      return true;
    if (!CombatManager.Instance.IsInProgress)
      return false;
    return NOverlayStack.Instance.ScreenCount > 0 || NCapstoneContainer.Instance.InUse;
  }

  public async Task ShineOnStartOfCombat()
  {
    if (!this.HasPotion || !((Node) this.Potion).IsValid())
      return;
    this.Potion.DoBounce();
    await Cmd.Wait(0.25f, this._cts.Token);
    NDebugAudioManager.Instance?.Play(Rng.Chaotic.NextItem<string>(TmpSfx.PotionSlosh), 0.3f, PitchVariance.Large);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(15)
    {
      new MethodInfo(NPotionHolder.MethodName.Create, new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false), (MethodFlags) 33L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 1L, StringName.op_Implicit("isUsable"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NPotionHolder.MethodName._EnterTree, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NPotionHolder.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NPotionHolder.MethodName._ExitTree, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NPotionHolder.MethodName.OnFocus, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NPotionHolder.MethodName.OnUnfocus, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NPotionHolder.MethodName.OnPress, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NPotionHolder.MethodName.OnRelease, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NPotionHolder.MethodName.OpenPotionPopup, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NPotionHolder.MethodName.AddPotion, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("potion"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false)
      }, (List<Variant>) null),
      new MethodInfo(NPotionHolder.MethodName.DisableUntilPotionRemoved, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NPotionHolder.MethodName.CancelPotionUseOrDiscard, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NPotionHolder.MethodName.RemoveUsedPotion, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NPotionHolder.MethodName.DiscardPotion, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NPotionHolder.MethodName.ShouldCancelTargeting, new PropertyInfo((Variant.Type) 1L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NPotionHolder.MethodName.Create) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      NPotionHolder npotionHolder = NPotionHolder.Create(VariantUtils.ConvertTo<bool>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = VariantUtils.CreateFrom<NPotionHolder>(ref npotionHolder);
      return true;
    }
    if (StringName.op_Equality(ref method, NPotionHolder.MethodName._EnterTree) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._EnterTree();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NPotionHolder.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NPotionHolder.MethodName._ExitTree) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._ExitTree();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NPotionHolder.MethodName.OnFocus) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnFocus();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NPotionHolder.MethodName.OnUnfocus) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnUnfocus();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NPotionHolder.MethodName.OnPress) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnPress();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NPotionHolder.MethodName.OnRelease) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnRelease();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NPotionHolder.MethodName.OpenPotionPopup) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OpenPotionPopup();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NPotionHolder.MethodName.AddPotion) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.AddPotion(VariantUtils.ConvertTo<NPotion>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NPotionHolder.MethodName.DisableUntilPotionRemoved) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.DisableUntilPotionRemoved();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NPotionHolder.MethodName.CancelPotionUseOrDiscard) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.CancelPotionUseOrDiscard();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NPotionHolder.MethodName.RemoveUsedPotion) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.RemoveUsedPotion();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NPotionHolder.MethodName.DiscardPotion) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.DiscardPotion();
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NPotionHolder.MethodName.ShouldCancelTargeting) || ((NativeVariantPtrArgs) ref args).Count != 0)
      return base.InvokeGodotClassMethod(in method, args, out ret);
    bool flag = this.ShouldCancelTargeting();
    ret = VariantUtils.CreateFrom<bool>(ref flag);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static bool InvokeGodotClassStaticMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NPotionHolder.MethodName.Create) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      NPotionHolder npotionHolder = NPotionHolder.Create(VariantUtils.ConvertTo<bool>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = VariantUtils.CreateFrom<NPotionHolder>(ref npotionHolder);
      return true;
    }
    ret = new godot_variant();
    return false;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NPotionHolder.MethodName.Create) || StringName.op_Equality(ref method, NPotionHolder.MethodName._EnterTree) || StringName.op_Equality(ref method, NPotionHolder.MethodName._Ready) || StringName.op_Equality(ref method, NPotionHolder.MethodName._ExitTree) || StringName.op_Equality(ref method, NPotionHolder.MethodName.OnFocus) || StringName.op_Equality(ref method, NPotionHolder.MethodName.OnUnfocus) || StringName.op_Equality(ref method, NPotionHolder.MethodName.OnPress) || StringName.op_Equality(ref method, NPotionHolder.MethodName.OnRelease) || StringName.op_Equality(ref method, NPotionHolder.MethodName.OpenPotionPopup) || StringName.op_Equality(ref method, NPotionHolder.MethodName.AddPotion) || StringName.op_Equality(ref method, NPotionHolder.MethodName.DisableUntilPotionRemoved) || StringName.op_Equality(ref method, NPotionHolder.MethodName.CancelPotionUseOrDiscard) || StringName.op_Equality(ref method, NPotionHolder.MethodName.RemoveUsedPotion) || StringName.op_Equality(ref method, NPotionHolder.MethodName.DiscardPotion) || StringName.op_Equality(ref method, NPotionHolder.MethodName.ShouldCancelTargeting) || base.HasGodotClassMethod(in method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NPotionHolder.PropertyName.Potion))
    {
      this.Potion = VariantUtils.ConvertTo<NPotion>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NPotionHolder.PropertyName._potionScale))
    {
      this._potionScale = VariantUtils.ConvertTo<Vector2>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NPotionHolder.PropertyName._emptyIcon))
    {
      this._emptyIcon = VariantUtils.ConvertTo<TextureRect>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NPotionHolder.PropertyName._selectionReticle))
    {
      this._selectionReticle = VariantUtils.ConvertTo<NSelectionReticle>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NPotionHolder.PropertyName._popup))
    {
      this._popup = VariantUtils.ConvertTo<NPotionPopup>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NPotionHolder.PropertyName._potionTargeting))
    {
      this._potionTargeting = VariantUtils.ConvertTo<bool>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NPotionHolder.PropertyName._isUsable))
    {
      this._isUsable = VariantUtils.ConvertTo<bool>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NPotionHolder.PropertyName._emptyPotionTween))
    {
      this._emptyPotionTween = VariantUtils.ConvertTo<Tween>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NPotionHolder.PropertyName._hoverTween))
    {
      this._hoverTween = VariantUtils.ConvertTo<Tween>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NPotionHolder.PropertyName._disabledUntilPotionRemoved))
    {
      this._disabledUntilPotionRemoved = VariantUtils.ConvertTo<bool>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NPotionHolder.PropertyName._isFocused))
      return base.SetGodotClassPropertyValue(in name, in value);
    this._isFocused = VariantUtils.ConvertTo<bool>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NPotionHolder.PropertyName.Potion))
    {
      ref godot_variant local = ref value;
      NPotion potion = this.Potion;
      godot_variant from = VariantUtils.CreateFrom<NPotion>(ref potion);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NPotionHolder.PropertyName.HasPotion))
    {
      ref godot_variant local = ref value;
      bool hasPotion = this.HasPotion;
      godot_variant from = VariantUtils.CreateFrom<bool>(ref hasPotion);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NPotionHolder.PropertyName.IsPotionUsable))
    {
      ref godot_variant local = ref value;
      bool isPotionUsable = this.IsPotionUsable;
      godot_variant from = VariantUtils.CreateFrom<bool>(ref isPotionUsable);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NPotionHolder.PropertyName._potionScale))
    {
      value = VariantUtils.CreateFrom<Vector2>(ref this._potionScale);
      return true;
    }
    if (StringName.op_Equality(ref name, NPotionHolder.PropertyName._emptyIcon))
    {
      value = VariantUtils.CreateFrom<TextureRect>(ref this._emptyIcon);
      return true;
    }
    if (StringName.op_Equality(ref name, NPotionHolder.PropertyName._selectionReticle))
    {
      value = VariantUtils.CreateFrom<NSelectionReticle>(ref this._selectionReticle);
      return true;
    }
    if (StringName.op_Equality(ref name, NPotionHolder.PropertyName._popup))
    {
      value = VariantUtils.CreateFrom<NPotionPopup>(ref this._popup);
      return true;
    }
    if (StringName.op_Equality(ref name, NPotionHolder.PropertyName._potionTargeting))
    {
      value = VariantUtils.CreateFrom<bool>(ref this._potionTargeting);
      return true;
    }
    if (StringName.op_Equality(ref name, NPotionHolder.PropertyName._isUsable))
    {
      value = VariantUtils.CreateFrom<bool>(ref this._isUsable);
      return true;
    }
    if (StringName.op_Equality(ref name, NPotionHolder.PropertyName._emptyPotionTween))
    {
      value = VariantUtils.CreateFrom<Tween>(ref this._emptyPotionTween);
      return true;
    }
    if (StringName.op_Equality(ref name, NPotionHolder.PropertyName._hoverTween))
    {
      value = VariantUtils.CreateFrom<Tween>(ref this._hoverTween);
      return true;
    }
    if (StringName.op_Equality(ref name, NPotionHolder.PropertyName._disabledUntilPotionRemoved))
    {
      value = VariantUtils.CreateFrom<bool>(ref this._disabledUntilPotionRemoved);
      return true;
    }
    if (!StringName.op_Equality(ref name, NPotionHolder.PropertyName._isFocused))
      return base.GetGodotClassPropertyValue(in name, out value);
    value = VariantUtils.CreateFrom<bool>(ref this._isFocused);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 5L, NPotionHolder.PropertyName._potionScale, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NPotionHolder.PropertyName.Potion, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NPotionHolder.PropertyName._emptyIcon, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NPotionHolder.PropertyName._selectionReticle, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 1L, NPotionHolder.PropertyName.HasPotion, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NPotionHolder.PropertyName._popup, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 1L, NPotionHolder.PropertyName._potionTargeting, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 1L, NPotionHolder.PropertyName._isUsable, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NPotionHolder.PropertyName._emptyPotionTween, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NPotionHolder.PropertyName._hoverTween, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 1L, NPotionHolder.PropertyName._disabledUntilPotionRemoved, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 1L, NPotionHolder.PropertyName.IsPotionUsable, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 1L, NPotionHolder.PropertyName._isFocused, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    base.SaveGodotObjectData(info);
    GodotSerializationInfo serializationInfo = info;
    StringName potion1 = NPotionHolder.PropertyName.Potion;
    NPotion potion2 = this.Potion;
    Variant variant = Variant.From<NPotion>(ref potion2);
    serializationInfo.AddProperty(potion1, variant);
    info.AddProperty(NPotionHolder.PropertyName._potionScale, Variant.From<Vector2>(ref this._potionScale));
    info.AddProperty(NPotionHolder.PropertyName._emptyIcon, Variant.From<TextureRect>(ref this._emptyIcon));
    info.AddProperty(NPotionHolder.PropertyName._selectionReticle, Variant.From<NSelectionReticle>(ref this._selectionReticle));
    info.AddProperty(NPotionHolder.PropertyName._popup, Variant.From<NPotionPopup>(ref this._popup));
    info.AddProperty(NPotionHolder.PropertyName._potionTargeting, Variant.From<bool>(ref this._potionTargeting));
    info.AddProperty(NPotionHolder.PropertyName._isUsable, Variant.From<bool>(ref this._isUsable));
    info.AddProperty(NPotionHolder.PropertyName._emptyPotionTween, Variant.From<Tween>(ref this._emptyPotionTween));
    info.AddProperty(NPotionHolder.PropertyName._hoverTween, Variant.From<Tween>(ref this._hoverTween));
    info.AddProperty(NPotionHolder.PropertyName._disabledUntilPotionRemoved, Variant.From<bool>(ref this._disabledUntilPotionRemoved));
    info.AddProperty(NPotionHolder.PropertyName._isFocused, Variant.From<bool>(ref this._isFocused));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    base.RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NPotionHolder.PropertyName.Potion, ref variant1))
      this.Potion = ((Variant) ref variant1).As<NPotion>();
    Variant variant2;
    if (info.TryGetProperty(NPotionHolder.PropertyName._potionScale, ref variant2))
      this._potionScale = ((Variant) ref variant2).As<Vector2>();
    Variant variant3;
    if (info.TryGetProperty(NPotionHolder.PropertyName._emptyIcon, ref variant3))
      this._emptyIcon = ((Variant) ref variant3).As<TextureRect>();
    Variant variant4;
    if (info.TryGetProperty(NPotionHolder.PropertyName._selectionReticle, ref variant4))
      this._selectionReticle = ((Variant) ref variant4).As<NSelectionReticle>();
    Variant variant5;
    if (info.TryGetProperty(NPotionHolder.PropertyName._popup, ref variant5))
      this._popup = ((Variant) ref variant5).As<NPotionPopup>();
    Variant variant6;
    if (info.TryGetProperty(NPotionHolder.PropertyName._potionTargeting, ref variant6))
      this._potionTargeting = ((Variant) ref variant6).As<bool>();
    Variant variant7;
    if (info.TryGetProperty(NPotionHolder.PropertyName._isUsable, ref variant7))
      this._isUsable = ((Variant) ref variant7).As<bool>();
    Variant variant8;
    if (info.TryGetProperty(NPotionHolder.PropertyName._emptyPotionTween, ref variant8))
      this._emptyPotionTween = ((Variant) ref variant8).As<Tween>();
    Variant variant9;
    if (info.TryGetProperty(NPotionHolder.PropertyName._hoverTween, ref variant9))
      this._hoverTween = ((Variant) ref variant9).As<Tween>();
    Variant variant10;
    if (info.TryGetProperty(NPotionHolder.PropertyName._disabledUntilPotionRemoved, ref variant10))
      this._disabledUntilPotionRemoved = ((Variant) ref variant10).As<bool>();
    Variant variant11;
    if (!info.TryGetProperty(NPotionHolder.PropertyName._isFocused, ref variant11))
      return;
    this._isFocused = ((Variant) ref variant11).As<bool>();
  }

  public new class MethodName : NClickableControl.MethodName
  {
    public static readonly StringName Create = StringName.op_Implicit(nameof (Create));
    public static readonly StringName _EnterTree = StringName.op_Implicit(nameof (_EnterTree));
    public static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName _ExitTree = StringName.op_Implicit(nameof (_ExitTree));
    public new static readonly StringName OnFocus = StringName.op_Implicit(nameof (OnFocus));
    public new static readonly StringName OnUnfocus = StringName.op_Implicit(nameof (OnUnfocus));
    public new static readonly StringName OnPress = StringName.op_Implicit(nameof (OnPress));
    public new static readonly StringName OnRelease = StringName.op_Implicit(nameof (OnRelease));
    public static readonly StringName OpenPotionPopup = StringName.op_Implicit(nameof (OpenPotionPopup));
    public static readonly StringName AddPotion = StringName.op_Implicit(nameof (AddPotion));
    public static readonly StringName DisableUntilPotionRemoved = StringName.op_Implicit(nameof (DisableUntilPotionRemoved));
    public static readonly StringName CancelPotionUseOrDiscard = StringName.op_Implicit(nameof (CancelPotionUseOrDiscard));
    public static readonly StringName RemoveUsedPotion = StringName.op_Implicit(nameof (RemoveUsedPotion));
    public static readonly StringName DiscardPotion = StringName.op_Implicit(nameof (DiscardPotion));
    public static readonly StringName ShouldCancelTargeting = StringName.op_Implicit(nameof (ShouldCancelTargeting));
  }

  public new class PropertyName : NClickableControl.PropertyName
  {
    public static readonly StringName Potion = StringName.op_Implicit(nameof (Potion));
    public static readonly StringName HasPotion = StringName.op_Implicit(nameof (HasPotion));
    public static readonly StringName IsPotionUsable = StringName.op_Implicit(nameof (IsPotionUsable));
    public static readonly StringName _potionScale = StringName.op_Implicit(nameof (_potionScale));
    public static readonly StringName _emptyIcon = StringName.op_Implicit(nameof (_emptyIcon));
    public static readonly StringName _selectionReticle = StringName.op_Implicit(nameof (_selectionReticle));
    public static readonly StringName _popup = StringName.op_Implicit(nameof (_popup));
    public static readonly StringName _potionTargeting = StringName.op_Implicit(nameof (_potionTargeting));
    public static readonly StringName _isUsable = StringName.op_Implicit(nameof (_isUsable));
    public static readonly StringName _emptyPotionTween = StringName.op_Implicit(nameof (_emptyPotionTween));
    public static readonly StringName _hoverTween = StringName.op_Implicit(nameof (_hoverTween));
    public static readonly StringName _disabledUntilPotionRemoved = StringName.op_Implicit(nameof (_disabledUntilPotionRemoved));
    public static readonly StringName _isFocused = StringName.op_Implicit(nameof (_isFocused));
  }

  public new class SignalName : NClickableControl.SignalName
  {
  }
}
