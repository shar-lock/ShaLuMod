// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Combat.NCreatureStateDisplay
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.UI;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Random;
using MegaCrit.Sts2.Core.Saves;
using MegaCrit.Sts2.Core.Settings;
using System;
using System.Collections.Generic;
using System.ComponentModel;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Combat;

[ScriptPath("res://src/Core/Nodes/Combat/NCreatureStateDisplay.cs")]
public class NCreatureStateDisplay : Control
{
  private NPowerContainer _powerContainer;
  private Control _nameplateContainer;
  private MegaLabel _nameplateLabel;
  private NHealthBar _healthBar;
  private Control _hpBarHitbox;
  private Creature? _creature;
  private Vector2 _creatureSize;
  private Creature? _blockTrackingCreature;
  private Tween? _showHideTween;
  private Tween? _hoverTween;
  private static readonly Vector2 _healthBarAnimOffset = new Vector2(0.0f, 20f);
  private Vector2 _originalPosition;

  public override void _Ready()
  {
    this._powerContainer = ((Node) this).GetNode<NPowerContainer>(NodePath.op_Implicit("%PowerContainer"));
    this._nameplateContainer = ((Node) this).GetNode<Control>(NodePath.op_Implicit("%NameplateContainer"));
    this._nameplateLabel = ((Node) this).GetNode<MegaLabel>(NodePath.op_Implicit("%NameplateLabel"));
    this._healthBar = ((Node) this).GetNode<NHealthBar>(NodePath.op_Implicit("%HealthBar"));
    this._hpBarHitbox = ((Node) this).GetNode<Control>(NodePath.op_Implicit("%HpBarHitbox"));
    ((CanvasItem) this._nameplateContainer).Modulate = StsColors.transparentWhite;
    this._originalPosition = this.Position;
    ((GodotObject) this._hpBarHitbox).Connect(Control.SignalName.MouseEntered, Callable.From(new Action(this.OnHovered)), 0U);
    ((GodotObject) this._hpBarHitbox).Connect(Control.SignalName.MouseExited, Callable.From(new Action(this.OnUnhovered)), 0U);
  }

  public override void _EnterTree()
  {
    ((Node) this)._EnterTree();
    CombatManager.Instance.StateTracker.CombatStateChanged += new Action<CombatState>(this.OnCombatStateChanged);
    this.SubscribeToCreatureEvents();
    if (NCombatRoom.Instance == null)
      return;
    NCombatRoom.Instance.Ui.DebugToggleHpBar += new Action(this.DebugToggleVisibility);
  }

  public override void _ExitTree()
  {
    ((Node) this)._ExitTree();
    CombatManager.Instance.StateTracker.CombatStateChanged -= new Action<CombatState>(this.OnCombatStateChanged);
    if (this._creature != null)
    {
      this._creature.BlockChanged -= new Action<int, int>(this.AnimateInBlock);
      this._creature.Died -= new Action<Creature>(this.OnCreatureDied);
      this._creature.Revived -= new Action<Creature>(this.OnCreatureRevived);
    }
    if (this._blockTrackingCreature != null)
      this._blockTrackingCreature.BlockChanged -= new Action<int, int>(this.OnBlockTrackingCreatureBlockChanged);
    if (NCombatRoom.Instance == null)
      return;
    NCombatRoom.Instance.Ui.DebugToggleHpBar -= new Action(this.DebugToggleVisibility);
  }

  public void SetCreature(Creature creature)
  {
    this._creature = this._creature == null ? creature : throw new InvalidOperationException("Creature was already set.");
    this.SubscribeToCreatureEvents();
    this._nameplateLabel.SetTextAutoSize(creature.Name);
    this._powerContainer.SetCreature(this._creature);
    this._healthBar.SetCreature(this._creature);
    this.RefreshValues();
  }

  private void SubscribeToCreatureEvents()
  {
    if (this._creature == null)
      return;
    this._creature.BlockChanged += new Action<int, int>(this.AnimateInBlock);
    this._creature.Died += new Action<Creature>(this.OnCreatureDied);
    this._creature.Revived += new Action<Creature>(this.OnCreatureRevived);
  }

  private void DebugToggleVisibility()
  {
    ((CanvasItem) this).Visible = !NCombatUi.IsDebugHidingHpBar;
  }

  public void SetCreatureBounds(Control bounds)
  {
    this._healthBar.UpdateLayoutForCreatureBounds(bounds);
    this._nameplateContainer.GlobalPosition = new Vector2(bounds.GlobalPosition.X, this._nameplateContainer.GlobalPosition.Y);
    this._nameplateContainer.Size = new Vector2(bounds.Size.X * bounds.Scale.X, this._nameplateContainer.Size.Y);
    this._powerContainer.SetCreatureBounds(bounds);
    this.RefreshValues();
  }

  private void RefreshValues()
  {
    if (this._creature == null)
      return;
    this._nameplateLabel.SetTextAutoSize(this._creature.Name);
    this._healthBar.RefreshValues();
  }

  private void OnCombatStateChanged(CombatState _) => this.RefreshValues();

  private void OnHovered()
  {
    this._healthBar.FadeOutHpLabel(0.5f, 0.1f);
    this.ShowNameplate();
    if (NTargetManager.Instance.IsInSelection)
      return;
    NCombatRoom.Instance?.GetCreatureNode(this._creature)?.ShowHoverTips(this._creature.HoverTips);
  }

  private void OnUnhovered()
  {
    this._healthBar.FadeInHpLabel(0.5f);
    this.HideNameplate();
    NCombatRoom.Instance.GetCreatureNode(this._creature)?.HideHoverTips();
  }

  public void ShowNameplate()
  {
    this._hoverTween?.Kill();
    this._hoverTween = ((Node) this).CreateTween().SetParallel(true);
    this._hoverTween.TweenProperty((GodotObject) this._powerContainer, NodePath.op_Implicit("modulate:a"), Variant.op_Implicit(0.5f), 0.15);
    this._hoverTween.TweenProperty((GodotObject) this._nameplateContainer, NodePath.op_Implicit("modulate:a"), Variant.op_Implicit(1f), 0.15);
  }

  public void HideNameplate()
  {
    this._hoverTween?.Kill();
    this._hoverTween = ((Node) this).CreateTween().SetParallel(true);
    this._hoverTween.TweenProperty((GodotObject) this._powerContainer, NodePath.op_Implicit("modulate:a"), Variant.op_Implicit(1f), 0.2);
    this._hoverTween.TweenProperty((GodotObject) this._nameplateContainer, NodePath.op_Implicit("modulate:a"), Variant.op_Implicit(0.0f), 0.2);
  }

  public void HideImmediately()
  {
    this._showHideTween?.Kill();
    this._hoverTween?.Kill();
    Color modulate = ((CanvasItem) this).Modulate;
    modulate.A = 0.0f;
    ((CanvasItem) this).Modulate = modulate;
  }

  public void AnimateIn(HealthBarAnimMode mode)
  {
    if (SaveManager.Instance.PrefsSave.FastMode == FastModeType.Instant)
    {
      Color modulate = ((CanvasItem) this).Modulate;
      modulate.A = 1f;
      ((CanvasItem) this).Modulate = modulate;
      ((CanvasItem) this).Visible = true;
    }
    else
    {
      float num = 0.0f;
      ((CanvasItem) this).Visible = true;
      ((CanvasItem) this).Modulate = StsColors.transparentWhite;
      this.Position = Vector2.op_Subtraction(this.Position, NCreatureStateDisplay._healthBarAnimOffset);
      if (mode == HealthBarAnimMode.SpawnedAtCombatStart)
        num = Rng.Chaotic.NextFloat(1.3f, 1.7f);
      this._showHideTween?.Kill();
      this._showHideTween = ((Node) this).CreateTween().SetParallel(true);
      this._showHideTween.TweenProperty((GodotObject) this, NodePath.op_Implicit("modulate:a"), Variant.op_Implicit(1f), mode == HealthBarAnimMode.FromHidden ? 0.15 : 1.0).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 1L).SetDelay((double) num);
      this._showHideTween.TweenProperty((GodotObject) this, NodePath.op_Implicit("position"), Variant.op_Implicit(this._originalPosition), mode == HealthBarAnimMode.FromHidden ? 0.15 : 0.5).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 4L).SetDelay((double) num);
    }
  }

  private void AnimateInBlock(int oldBlock, int blockGain)
  {
    if (oldBlock != 0 || blockGain == 0)
      return;
    this._healthBar.AnimateInBlock(oldBlock, blockGain);
  }

  public void AnimateOut()
  {
    if (SaveManager.Instance.PrefsSave.FastMode == FastModeType.Instant)
    {
      Color modulate = ((CanvasItem) this).Modulate;
      modulate.A = 0.0f;
      ((CanvasItem) this).Modulate = modulate;
      ((CanvasItem) this).Visible = false;
    }
    else
    {
      this._showHideTween?.Kill();
      this._showHideTween = ((Node) this).CreateTween().SetParallel(true);
      this._showHideTween.TweenProperty((GodotObject) this, NodePath.op_Implicit("modulate:a"), Variant.op_Implicit(0.0f), 0.5).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 1L);
      this._showHideTween.TweenProperty((GodotObject) this, NodePath.op_Implicit("position"), Variant.op_Implicit(NCreatureStateDisplay._healthBarAnimOffset), 0.25).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 4L);
      this._showHideTween.Chain().TweenCallback(Callable.From<bool>((Func<bool>) (() => ((CanvasItem) this).Visible = false)));
    }
  }

  private void OnCreatureDied(Creature _)
  {
    this._hpBarHitbox.MouseFilter = (Control.MouseFilterEnum) 2L;
  }

  private void OnCreatureRevived(Creature _)
  {
    this._hpBarHitbox.MouseFilter = (Control.MouseFilterEnum) 0L;
  }

  public void TrackBlockStatus(Creature creature)
  {
    this._blockTrackingCreature = creature;
    this._blockTrackingCreature.BlockChanged += new Action<int, int>(this.OnBlockTrackingCreatureBlockChanged);
    this._healthBar.TrackBlockStatus(creature);
  }

  private void OnBlockTrackingCreatureBlockChanged(int oldBlock, int blockGain)
  {
    this._healthBar.RefreshValues();
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(16 /*0x10*/)
    {
      new MethodInfo(NCreatureStateDisplay.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCreatureStateDisplay.MethodName._EnterTree, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCreatureStateDisplay.MethodName._ExitTree, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCreatureStateDisplay.MethodName.SubscribeToCreatureEvents, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCreatureStateDisplay.MethodName.DebugToggleVisibility, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCreatureStateDisplay.MethodName.SetCreatureBounds, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("bounds"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false)
      }, (List<Variant>) null),
      new MethodInfo(NCreatureStateDisplay.MethodName.RefreshValues, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCreatureStateDisplay.MethodName.OnHovered, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCreatureStateDisplay.MethodName.OnUnhovered, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCreatureStateDisplay.MethodName.ShowNameplate, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCreatureStateDisplay.MethodName.HideNameplate, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCreatureStateDisplay.MethodName.HideImmediately, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCreatureStateDisplay.MethodName.AnimateIn, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 2L, StringName.op_Implicit("mode"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NCreatureStateDisplay.MethodName.AnimateInBlock, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 2L, StringName.op_Implicit("oldBlock"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false),
        new PropertyInfo((Variant.Type) 2L, StringName.op_Implicit("blockGain"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NCreatureStateDisplay.MethodName.AnimateOut, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCreatureStateDisplay.MethodName.OnBlockTrackingCreatureBlockChanged, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 2L, StringName.op_Implicit("oldBlock"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false),
        new PropertyInfo((Variant.Type) 2L, StringName.op_Implicit("blockGain"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NCreatureStateDisplay.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCreatureStateDisplay.MethodName._EnterTree) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._EnterTree();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCreatureStateDisplay.MethodName._ExitTree) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._ExitTree();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCreatureStateDisplay.MethodName.SubscribeToCreatureEvents) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.SubscribeToCreatureEvents();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCreatureStateDisplay.MethodName.DebugToggleVisibility) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.DebugToggleVisibility();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCreatureStateDisplay.MethodName.SetCreatureBounds) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.SetCreatureBounds(VariantUtils.ConvertTo<Control>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCreatureStateDisplay.MethodName.RefreshValues) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.RefreshValues();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCreatureStateDisplay.MethodName.OnHovered) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnHovered();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCreatureStateDisplay.MethodName.OnUnhovered) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnUnhovered();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCreatureStateDisplay.MethodName.ShowNameplate) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.ShowNameplate();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCreatureStateDisplay.MethodName.HideNameplate) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.HideNameplate();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCreatureStateDisplay.MethodName.HideImmediately) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.HideImmediately();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCreatureStateDisplay.MethodName.AnimateIn) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.AnimateIn(VariantUtils.ConvertTo<HealthBarAnimMode>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCreatureStateDisplay.MethodName.AnimateInBlock) && ((NativeVariantPtrArgs) ref args).Count == 2)
    {
      this.AnimateInBlock(VariantUtils.ConvertTo<int>(ref ((NativeVariantPtrArgs) ref args)[0]), VariantUtils.ConvertTo<int>(ref ((NativeVariantPtrArgs) ref args)[1]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCreatureStateDisplay.MethodName.AnimateOut) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.AnimateOut();
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NCreatureStateDisplay.MethodName.OnBlockTrackingCreatureBlockChanged) || ((NativeVariantPtrArgs) ref args).Count != 2)
      return base.InvokeGodotClassMethod(ref method, args, ref ret);
    this.OnBlockTrackingCreatureBlockChanged(VariantUtils.ConvertTo<int>(ref ((NativeVariantPtrArgs) ref args)[0]), VariantUtils.ConvertTo<int>(ref ((NativeVariantPtrArgs) ref args)[1]));
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NCreatureStateDisplay.MethodName._Ready) || StringName.op_Equality(ref method, NCreatureStateDisplay.MethodName._EnterTree) || StringName.op_Equality(ref method, NCreatureStateDisplay.MethodName._ExitTree) || StringName.op_Equality(ref method, NCreatureStateDisplay.MethodName.SubscribeToCreatureEvents) || StringName.op_Equality(ref method, NCreatureStateDisplay.MethodName.DebugToggleVisibility) || StringName.op_Equality(ref method, NCreatureStateDisplay.MethodName.SetCreatureBounds) || StringName.op_Equality(ref method, NCreatureStateDisplay.MethodName.RefreshValues) || StringName.op_Equality(ref method, NCreatureStateDisplay.MethodName.OnHovered) || StringName.op_Equality(ref method, NCreatureStateDisplay.MethodName.OnUnhovered) || StringName.op_Equality(ref method, NCreatureStateDisplay.MethodName.ShowNameplate) || StringName.op_Equality(ref method, NCreatureStateDisplay.MethodName.HideNameplate) || StringName.op_Equality(ref method, NCreatureStateDisplay.MethodName.HideImmediately) || StringName.op_Equality(ref method, NCreatureStateDisplay.MethodName.AnimateIn) || StringName.op_Equality(ref method, NCreatureStateDisplay.MethodName.AnimateInBlock) || StringName.op_Equality(ref method, NCreatureStateDisplay.MethodName.AnimateOut) || StringName.op_Equality(ref method, NCreatureStateDisplay.MethodName.OnBlockTrackingCreatureBlockChanged) || base.HasGodotClassMethod(ref method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NCreatureStateDisplay.PropertyName._powerContainer))
    {
      this._powerContainer = VariantUtils.ConvertTo<NPowerContainer>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCreatureStateDisplay.PropertyName._nameplateContainer))
    {
      this._nameplateContainer = VariantUtils.ConvertTo<Control>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCreatureStateDisplay.PropertyName._nameplateLabel))
    {
      this._nameplateLabel = VariantUtils.ConvertTo<MegaLabel>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCreatureStateDisplay.PropertyName._healthBar))
    {
      this._healthBar = VariantUtils.ConvertTo<NHealthBar>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCreatureStateDisplay.PropertyName._hpBarHitbox))
    {
      this._hpBarHitbox = VariantUtils.ConvertTo<Control>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCreatureStateDisplay.PropertyName._creatureSize))
    {
      this._creatureSize = VariantUtils.ConvertTo<Vector2>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCreatureStateDisplay.PropertyName._showHideTween))
    {
      this._showHideTween = VariantUtils.ConvertTo<Tween>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCreatureStateDisplay.PropertyName._hoverTween))
    {
      this._hoverTween = VariantUtils.ConvertTo<Tween>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NCreatureStateDisplay.PropertyName._originalPosition))
      return ((GodotObject) this).SetGodotClassPropertyValue(ref name, ref value);
    this._originalPosition = VariantUtils.ConvertTo<Vector2>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NCreatureStateDisplay.PropertyName._powerContainer))
    {
      value = VariantUtils.CreateFrom<NPowerContainer>(ref this._powerContainer);
      return true;
    }
    if (StringName.op_Equality(ref name, NCreatureStateDisplay.PropertyName._nameplateContainer))
    {
      value = VariantUtils.CreateFrom<Control>(ref this._nameplateContainer);
      return true;
    }
    if (StringName.op_Equality(ref name, NCreatureStateDisplay.PropertyName._nameplateLabel))
    {
      value = VariantUtils.CreateFrom<MegaLabel>(ref this._nameplateLabel);
      return true;
    }
    if (StringName.op_Equality(ref name, NCreatureStateDisplay.PropertyName._healthBar))
    {
      value = VariantUtils.CreateFrom<NHealthBar>(ref this._healthBar);
      return true;
    }
    if (StringName.op_Equality(ref name, NCreatureStateDisplay.PropertyName._hpBarHitbox))
    {
      value = VariantUtils.CreateFrom<Control>(ref this._hpBarHitbox);
      return true;
    }
    if (StringName.op_Equality(ref name, NCreatureStateDisplay.PropertyName._creatureSize))
    {
      value = VariantUtils.CreateFrom<Vector2>(ref this._creatureSize);
      return true;
    }
    if (StringName.op_Equality(ref name, NCreatureStateDisplay.PropertyName._showHideTween))
    {
      value = VariantUtils.CreateFrom<Tween>(ref this._showHideTween);
      return true;
    }
    if (StringName.op_Equality(ref name, NCreatureStateDisplay.PropertyName._hoverTween))
    {
      value = VariantUtils.CreateFrom<Tween>(ref this._hoverTween);
      return true;
    }
    if (!StringName.op_Equality(ref name, NCreatureStateDisplay.PropertyName._originalPosition))
      return ((GodotObject) this).GetGodotClassPropertyValue(ref name, ref value);
    value = VariantUtils.CreateFrom<Vector2>(ref this._originalPosition);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NCreatureStateDisplay.PropertyName._powerContainer, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NCreatureStateDisplay.PropertyName._nameplateContainer, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NCreatureStateDisplay.PropertyName._nameplateLabel, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NCreatureStateDisplay.PropertyName._healthBar, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NCreatureStateDisplay.PropertyName._hpBarHitbox, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 5L, NCreatureStateDisplay.PropertyName._creatureSize, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NCreatureStateDisplay.PropertyName._showHideTween, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NCreatureStateDisplay.PropertyName._hoverTween, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 5L, NCreatureStateDisplay.PropertyName._originalPosition, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).SaveGodotObjectData(info);
    info.AddProperty(NCreatureStateDisplay.PropertyName._powerContainer, Variant.From<NPowerContainer>(ref this._powerContainer));
    info.AddProperty(NCreatureStateDisplay.PropertyName._nameplateContainer, Variant.From<Control>(ref this._nameplateContainer));
    info.AddProperty(NCreatureStateDisplay.PropertyName._nameplateLabel, Variant.From<MegaLabel>(ref this._nameplateLabel));
    info.AddProperty(NCreatureStateDisplay.PropertyName._healthBar, Variant.From<NHealthBar>(ref this._healthBar));
    info.AddProperty(NCreatureStateDisplay.PropertyName._hpBarHitbox, Variant.From<Control>(ref this._hpBarHitbox));
    info.AddProperty(NCreatureStateDisplay.PropertyName._creatureSize, Variant.From<Vector2>(ref this._creatureSize));
    info.AddProperty(NCreatureStateDisplay.PropertyName._showHideTween, Variant.From<Tween>(ref this._showHideTween));
    info.AddProperty(NCreatureStateDisplay.PropertyName._hoverTween, Variant.From<Tween>(ref this._hoverTween));
    info.AddProperty(NCreatureStateDisplay.PropertyName._originalPosition, Variant.From<Vector2>(ref this._originalPosition));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NCreatureStateDisplay.PropertyName._powerContainer, ref variant1))
      this._powerContainer = ((Variant) ref variant1).As<NPowerContainer>();
    Variant variant2;
    if (info.TryGetProperty(NCreatureStateDisplay.PropertyName._nameplateContainer, ref variant2))
      this._nameplateContainer = ((Variant) ref variant2).As<Control>();
    Variant variant3;
    if (info.TryGetProperty(NCreatureStateDisplay.PropertyName._nameplateLabel, ref variant3))
      this._nameplateLabel = ((Variant) ref variant3).As<MegaLabel>();
    Variant variant4;
    if (info.TryGetProperty(NCreatureStateDisplay.PropertyName._healthBar, ref variant4))
      this._healthBar = ((Variant) ref variant4).As<NHealthBar>();
    Variant variant5;
    if (info.TryGetProperty(NCreatureStateDisplay.PropertyName._hpBarHitbox, ref variant5))
      this._hpBarHitbox = ((Variant) ref variant5).As<Control>();
    Variant variant6;
    if (info.TryGetProperty(NCreatureStateDisplay.PropertyName._creatureSize, ref variant6))
      this._creatureSize = ((Variant) ref variant6).As<Vector2>();
    Variant variant7;
    if (info.TryGetProperty(NCreatureStateDisplay.PropertyName._showHideTween, ref variant7))
      this._showHideTween = ((Variant) ref variant7).As<Tween>();
    Variant variant8;
    if (info.TryGetProperty(NCreatureStateDisplay.PropertyName._hoverTween, ref variant8))
      this._hoverTween = ((Variant) ref variant8).As<Tween>();
    Variant variant9;
    if (!info.TryGetProperty(NCreatureStateDisplay.PropertyName._originalPosition, ref variant9))
      return;
    this._originalPosition = ((Variant) ref variant9).As<Vector2>();
  }

  public class MethodName : Control.MethodName
  {
    public static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName _EnterTree = StringName.op_Implicit(nameof (_EnterTree));
    public static readonly StringName _ExitTree = StringName.op_Implicit(nameof (_ExitTree));
    public static readonly StringName SubscribeToCreatureEvents = StringName.op_Implicit(nameof (SubscribeToCreatureEvents));
    public static readonly StringName DebugToggleVisibility = StringName.op_Implicit(nameof (DebugToggleVisibility));
    public static readonly StringName SetCreatureBounds = StringName.op_Implicit(nameof (SetCreatureBounds));
    public static readonly StringName RefreshValues = StringName.op_Implicit(nameof (RefreshValues));
    public static readonly StringName OnHovered = StringName.op_Implicit(nameof (OnHovered));
    public static readonly StringName OnUnhovered = StringName.op_Implicit(nameof (OnUnhovered));
    public static readonly StringName ShowNameplate = StringName.op_Implicit(nameof (ShowNameplate));
    public static readonly StringName HideNameplate = StringName.op_Implicit(nameof (HideNameplate));
    public static readonly StringName HideImmediately = StringName.op_Implicit(nameof (HideImmediately));
    public static readonly StringName AnimateIn = StringName.op_Implicit(nameof (AnimateIn));
    public static readonly StringName AnimateInBlock = StringName.op_Implicit(nameof (AnimateInBlock));
    public static readonly StringName AnimateOut = StringName.op_Implicit(nameof (AnimateOut));
    public static readonly StringName OnBlockTrackingCreatureBlockChanged = StringName.op_Implicit(nameof (OnBlockTrackingCreatureBlockChanged));
  }

  public class PropertyName : Control.PropertyName
  {
    public static readonly StringName _powerContainer = StringName.op_Implicit(nameof (_powerContainer));
    public static readonly StringName _nameplateContainer = StringName.op_Implicit(nameof (_nameplateContainer));
    public static readonly StringName _nameplateLabel = StringName.op_Implicit(nameof (_nameplateLabel));
    public static readonly StringName _healthBar = StringName.op_Implicit(nameof (_healthBar));
    public static readonly StringName _hpBarHitbox = StringName.op_Implicit(nameof (_hpBarHitbox));
    public static readonly StringName _creatureSize = StringName.op_Implicit(nameof (_creatureSize));
    public static readonly StringName _showHideTween = StringName.op_Implicit(nameof (_showHideTween));
    public static readonly StringName _hoverTween = StringName.op_Implicit(nameof (_hoverTween));
    public static readonly StringName _originalPosition = StringName.op_Implicit(nameof (_originalPosition));
  }

  public class SignalName : Control.SignalName
  {
  }
}
