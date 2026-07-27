// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Cards.Holders.NCardHolder
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.ControllerInput;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.UI;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.HoverTips;
using System;
using System.Collections.Generic;
using System.ComponentModel;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Cards.Holders;

[ScriptPath("res://src/Core/Nodes/Cards/Holders/NCardHolder.cs")]
public abstract class NCardHolder : Control
{
  public static readonly Vector2 smallScale = Vector2.op_Multiply(Vector2.One, 0.8f);
  protected NClickableControl _hitbox;
  protected bool _isHovered;
  protected bool _isFocused;
  protected Tween? _hoverTween;
  private InputEventMouseButton? _currentPressedAction;
  protected bool _isClickable = true;
  private 
  #nullable disable
  NCardHolder.PressedEventHandler backing_Pressed;
  private NCardHolder.AltPressedEventHandler backing_AltPressed;

  protected virtual Vector2 HoverScale => Vector2.One;

  public virtual Vector2 SmallScale => NCardHolder.smallScale;

  public 
  #nullable enable
  NClickableControl Hitbox => this._hitbox;

  public NCard? CardNode { get; protected set; }

  public virtual CardModel? CardModel => this.CardNode?.Model;

  public virtual bool IsShowingUpgradedCard
  {
    get
    {
      CardModel cardModel = this.CardModel;
      return cardModel != null && cardModel.IsUpgraded;
    }
  }

  protected bool CanBeFocused => this._isHovered;

  public override void _Ready()
  {
    if (((object) this).GetType() != typeof (NCardHolder))
    {
      Log.Error($"{((object) this).GetType()}");
      throw new InvalidOperationException("Don't call base._Ready()! Call ConnectSignals() instead.");
    }
    this.ConnectSignals();
  }

  public void SetClickable(bool isClickable) => this._isClickable = isClickable;

  protected void ConnectSignals()
  {
    if (this.CardNode != null)
      this.CardNode.Position = Vector2.Zero;
    ((GodotObject) this).Connect(Control.SignalName.FocusEntered, Callable.From(new Action(this.OnFocus)), 0U);
    ((GodotObject) this).Connect(Control.SignalName.FocusExited, Callable.From(new Action(this.OnUnfocus)), 0U);
    this._hitbox = ((Node) this).GetNode<NClickableControl>(NodePath.op_Implicit("%Hitbox"));
    ((GodotObject) this._hitbox).Connect(NClickableControl.SignalName.Focused, Callable.From<NClickableControl>((Action<NClickableControl>) (_ => this.OnFocus())), 0U);
    ((GodotObject) this._hitbox).Connect(NClickableControl.SignalName.Unfocused, Callable.From<NClickableControl>((Action<NClickableControl>) (_ => this.OnUnfocus())), 0U);
    ((GodotObject) this._hitbox).Connect(NClickableControl.SignalName.MousePressed, Callable.From<InputEvent>(new Action<InputEvent>(this.OnMousePressed)), 0U);
    ((GodotObject) this._hitbox).Connect(NClickableControl.SignalName.MouseReleased, Callable.From<InputEvent>(new Action<InputEvent>(this.OnMouseReleased)), 0U);
    ((GodotObject) this).Connect(Node.SignalName.ChildExitingTree, Callable.From<Node>(new Action<Node>(this.OnChildExitingTree)), 0U);
  }

  public override void _GuiInput(InputEvent inputEvent)
  {
    base._GuiInput(inputEvent);
    if (!this._isClickable || this.CardNode == null)
      return;
    if (inputEvent.IsActionPressed(MegaInput.select, false, false))
    {
      SfxCmd.Play("event:/sfx/ui/clicks/ui_click");
      ((GodotObject) this).CallDeferred(NCardHolder.MethodName.EmitPressed, Array.Empty<Variant>());
    }
    else
    {
      if (!inputEvent.IsActionPressed(MegaInput.accept, false, false))
        return;
      SfxCmd.Play("event:/sfx/ui/clicks/ui_click");
      ((GodotObject) this).CallDeferred(NCardHolder.MethodName.EmitAltPressed, Array.Empty<Variant>());
    }
  }

  private void EmitPressed()
  {
    ((GodotObject) this).EmitSignal(NCardHolder.SignalName.Pressed, new Variant[1]
    {
      Variant.op_Implicit((GodotObject) this)
    });
  }

  private void EmitAltPressed()
  {
    ((GodotObject) this).EmitSignal(NCardHolder.SignalName.AltPressed, new Variant[1]
    {
      Variant.op_Implicit((GodotObject) this)
    });
  }

  protected virtual void SetCard(NCard node)
  {
    this.CardNode = this.CardNode == null ? node : throw new InvalidOperationException("Cannot set a card node on a holder that already has one.");
    if (((Node) this.CardNode).GetParent() == null)
      ((Node) this).AddChildSafely((Node) node);
    else
      ((Node) node).Reparent((Node) this, true);
  }

  public void ReassignToCard(
    CardModel cardModel,
    PileType pileType,
    Creature? target,
    ModelVisibility visibility)
  {
    this.CardNode.Visibility = visibility;
    this.CardNode.Model = cardModel;
    this.CardNode.SetPreviewTarget(target);
    this.CardNode.UpdateVisuals(pileType, CardPreviewMode.Normal);
    this.OnCardReassigned();
  }

  protected virtual void OnCardReassigned()
  {
  }

  protected virtual void OnMousePressed(InputEvent inputEvent)
  {
    if (this._currentPressedAction != null || !(inputEvent is InputEventMouseButton eventMouseButton) || !this._isClickable)
      return;
    if (eventMouseButton.ButtonIndex - 1L <= 1L)
      SfxCmd.Play("event:/sfx/ui/clicks/ui_click");
    this._currentPressedAction = eventMouseButton;
  }

  protected virtual void OnMouseReleased(InputEvent inputEvent)
  {
    if (this.CardNode == null || !this._isHovered || this._currentPressedAction == null)
      return;
    if (inputEvent is InputEventMouseButton eventMouseButton && this._isClickable)
    {
      if (eventMouseButton.ButtonIndex != this._currentPressedAction.ButtonIndex)
        return;
      if (eventMouseButton.ButtonIndex == 1L)
        ((GodotObject) this).EmitSignal(NCardHolder.SignalName.Pressed, new Variant[1]
        {
          Variant.op_Implicit((GodotObject) this)
        });
      else
        ((GodotObject) this).EmitSignal(NCardHolder.SignalName.AltPressed, new Variant[1]
        {
          Variant.op_Implicit((GodotObject) this)
        });
    }
    this._currentPressedAction = (InputEventMouseButton) null;
  }

  protected virtual void OnFocus()
  {
    this._isHovered = true;
    this.RefreshFocusState();
  }

  protected virtual void CreateHoverTips()
  {
    if (this.CardNode == null)
      return;
    NHoverTipSet.CreateAndShow((Control) this, this.CardNode.Model.HoverTips)?.SetAlignmentForCardHolder(this);
  }

  protected void ClearHoverTips() => NHoverTipSet.Remove((Control) this);

  protected virtual void OnUnfocus()
  {
    this._isHovered = false;
    this._currentPressedAction = (InputEventMouseButton) null;
    this.RefreshFocusState();
  }

  protected void RefreshFocusState()
  {
    if (this._isFocused == this.CanBeFocused)
      return;
    this._isFocused = this.CanBeFocused;
    this.DoCardHoverEffects(this._isFocused);
  }

  protected virtual void DoCardHoverEffects(bool isHovered)
  {
    if (isHovered)
    {
      this._hoverTween?.Kill();
      this.Scale = this.HoverScale;
      if (this.CardNode.Visibility != ModelVisibility.Visible)
        return;
      this.CreateHoverTips();
    }
    else
    {
      if (isHovered)
        return;
      this._hoverTween?.Kill();
      this._hoverTween = ((Node) this).CreateTween();
      this._hoverTween.TweenProperty((GodotObject) this, NodePath.op_Implicit("scale"), Variant.op_Implicit(this.SmallScale), 0.5).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 5L);
      this.ClearHoverTips();
    }
  }

  private void OnChildExitingTree(Node node)
  {
    if (node != this.CardNode || node.GetParent() == this)
      return;
    this.ClearHoverTips();
    this.CardNode = (NCard) null;
  }

  public virtual void Clear()
  {
    if (this.CardNode == null)
      return;
    if (((Node) this.CardNode).GetParent() == this)
      ((Node) this).RemoveChildSafely((Node) this.CardNode);
    this.CardNode = (NCard) null;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(18)
    {
      new MethodInfo(NCardHolder.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCardHolder.MethodName.SetClickable, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 1L, StringName.op_Implicit("isClickable"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NCardHolder.MethodName.ConnectSignals, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCardHolder.MethodName._GuiInput, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("inputEvent"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("InputEvent"), false)
      }, (List<Variant>) null),
      new MethodInfo(NCardHolder.MethodName.EmitPressed, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCardHolder.MethodName.EmitAltPressed, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCardHolder.MethodName.SetCard, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("node"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false)
      }, (List<Variant>) null),
      new MethodInfo(NCardHolder.MethodName.OnCardReassigned, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCardHolder.MethodName.OnMousePressed, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("inputEvent"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("InputEvent"), false)
      }, (List<Variant>) null),
      new MethodInfo(NCardHolder.MethodName.OnMouseReleased, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("inputEvent"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("InputEvent"), false)
      }, (List<Variant>) null),
      new MethodInfo(NCardHolder.MethodName.OnFocus, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCardHolder.MethodName.CreateHoverTips, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCardHolder.MethodName.ClearHoverTips, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCardHolder.MethodName.OnUnfocus, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCardHolder.MethodName.RefreshFocusState, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCardHolder.MethodName.DoCardHoverEffects, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 1L, StringName.op_Implicit("isHovered"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NCardHolder.MethodName.OnChildExitingTree, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("node"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Node"), false)
      }, (List<Variant>) null),
      new MethodInfo(NCardHolder.MethodName.Clear, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NCardHolder.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCardHolder.MethodName.SetClickable) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.SetClickable(VariantUtils.ConvertTo<bool>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCardHolder.MethodName.ConnectSignals) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.ConnectSignals();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCardHolder.MethodName._GuiInput) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      base._GuiInput(VariantUtils.ConvertTo<InputEvent>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCardHolder.MethodName.EmitPressed) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.EmitPressed();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCardHolder.MethodName.EmitAltPressed) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.EmitAltPressed();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCardHolder.MethodName.SetCard) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.SetCard(VariantUtils.ConvertTo<NCard>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCardHolder.MethodName.OnCardReassigned) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnCardReassigned();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCardHolder.MethodName.OnMousePressed) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.OnMousePressed(VariantUtils.ConvertTo<InputEvent>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCardHolder.MethodName.OnMouseReleased) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.OnMouseReleased(VariantUtils.ConvertTo<InputEvent>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCardHolder.MethodName.OnFocus) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnFocus();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCardHolder.MethodName.CreateHoverTips) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.CreateHoverTips();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCardHolder.MethodName.ClearHoverTips) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.ClearHoverTips();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCardHolder.MethodName.OnUnfocus) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnUnfocus();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCardHolder.MethodName.RefreshFocusState) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.RefreshFocusState();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCardHolder.MethodName.DoCardHoverEffects) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.DoCardHoverEffects(VariantUtils.ConvertTo<bool>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCardHolder.MethodName.OnChildExitingTree) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.OnChildExitingTree(VariantUtils.ConvertTo<Node>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NCardHolder.MethodName.Clear) || ((NativeVariantPtrArgs) ref args).Count != 0)
      return base.InvokeGodotClassMethod(ref method, args, ref ret);
    this.Clear();
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NCardHolder.MethodName._Ready) || StringName.op_Equality(ref method, NCardHolder.MethodName.SetClickable) || StringName.op_Equality(ref method, NCardHolder.MethodName.ConnectSignals) || StringName.op_Equality(ref method, NCardHolder.MethodName._GuiInput) || StringName.op_Equality(ref method, NCardHolder.MethodName.EmitPressed) || StringName.op_Equality(ref method, NCardHolder.MethodName.EmitAltPressed) || StringName.op_Equality(ref method, NCardHolder.MethodName.SetCard) || StringName.op_Equality(ref method, NCardHolder.MethodName.OnCardReassigned) || StringName.op_Equality(ref method, NCardHolder.MethodName.OnMousePressed) || StringName.op_Equality(ref method, NCardHolder.MethodName.OnMouseReleased) || StringName.op_Equality(ref method, NCardHolder.MethodName.OnFocus) || StringName.op_Equality(ref method, NCardHolder.MethodName.CreateHoverTips) || StringName.op_Equality(ref method, NCardHolder.MethodName.ClearHoverTips) || StringName.op_Equality(ref method, NCardHolder.MethodName.OnUnfocus) || StringName.op_Equality(ref method, NCardHolder.MethodName.RefreshFocusState) || StringName.op_Equality(ref method, NCardHolder.MethodName.DoCardHoverEffects) || StringName.op_Equality(ref method, NCardHolder.MethodName.OnChildExitingTree) || StringName.op_Equality(ref method, NCardHolder.MethodName.Clear) || base.HasGodotClassMethod(ref method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NCardHolder.PropertyName.CardNode))
    {
      this.CardNode = VariantUtils.ConvertTo<NCard>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCardHolder.PropertyName._hitbox))
    {
      this._hitbox = VariantUtils.ConvertTo<NClickableControl>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCardHolder.PropertyName._isHovered))
    {
      this._isHovered = VariantUtils.ConvertTo<bool>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCardHolder.PropertyName._isFocused))
    {
      this._isFocused = VariantUtils.ConvertTo<bool>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCardHolder.PropertyName._hoverTween))
    {
      this._hoverTween = VariantUtils.ConvertTo<Tween>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCardHolder.PropertyName._currentPressedAction))
    {
      this._currentPressedAction = VariantUtils.ConvertTo<InputEventMouseButton>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NCardHolder.PropertyName._isClickable))
      return ((GodotObject) this).SetGodotClassPropertyValue(ref name, ref value);
    this._isClickable = VariantUtils.ConvertTo<bool>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NCardHolder.PropertyName.HoverScale))
    {
      ref godot_variant local = ref value;
      Vector2 hoverScale = this.HoverScale;
      godot_variant from = VariantUtils.CreateFrom<Vector2>(ref hoverScale);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NCardHolder.PropertyName.SmallScale))
    {
      ref godot_variant local = ref value;
      Vector2 smallScale = this.SmallScale;
      godot_variant from = VariantUtils.CreateFrom<Vector2>(ref smallScale);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NCardHolder.PropertyName.Hitbox))
    {
      ref godot_variant local = ref value;
      NClickableControl hitbox = this.Hitbox;
      godot_variant from = VariantUtils.CreateFrom<NClickableControl>(ref hitbox);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NCardHolder.PropertyName.CardNode))
    {
      ref godot_variant local = ref value;
      NCard cardNode = this.CardNode;
      godot_variant from = VariantUtils.CreateFrom<NCard>(ref cardNode);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NCardHolder.PropertyName.IsShowingUpgradedCard))
    {
      ref godot_variant local = ref value;
      bool showingUpgradedCard = this.IsShowingUpgradedCard;
      godot_variant from = VariantUtils.CreateFrom<bool>(ref showingUpgradedCard);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NCardHolder.PropertyName.CanBeFocused))
    {
      ref godot_variant local = ref value;
      bool canBeFocused = this.CanBeFocused;
      godot_variant from = VariantUtils.CreateFrom<bool>(ref canBeFocused);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NCardHolder.PropertyName._hitbox))
    {
      value = VariantUtils.CreateFrom<NClickableControl>(ref this._hitbox);
      return true;
    }
    if (StringName.op_Equality(ref name, NCardHolder.PropertyName._isHovered))
    {
      value = VariantUtils.CreateFrom<bool>(ref this._isHovered);
      return true;
    }
    if (StringName.op_Equality(ref name, NCardHolder.PropertyName._isFocused))
    {
      value = VariantUtils.CreateFrom<bool>(ref this._isFocused);
      return true;
    }
    if (StringName.op_Equality(ref name, NCardHolder.PropertyName._hoverTween))
    {
      value = VariantUtils.CreateFrom<Tween>(ref this._hoverTween);
      return true;
    }
    if (StringName.op_Equality(ref name, NCardHolder.PropertyName._currentPressedAction))
    {
      value = VariantUtils.CreateFrom<InputEventMouseButton>(ref this._currentPressedAction);
      return true;
    }
    if (!StringName.op_Equality(ref name, NCardHolder.PropertyName._isClickable))
      return ((GodotObject) this).GetGodotClassPropertyValue(ref name, ref value);
    value = VariantUtils.CreateFrom<bool>(ref this._isClickable);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 5L, NCardHolder.PropertyName.HoverScale, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 5L, NCardHolder.PropertyName.SmallScale, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NCardHolder.PropertyName._hitbox, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NCardHolder.PropertyName.Hitbox, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NCardHolder.PropertyName.CardNode, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 1L, NCardHolder.PropertyName.IsShowingUpgradedCard, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 1L, NCardHolder.PropertyName.CanBeFocused, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 1L, NCardHolder.PropertyName._isHovered, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 1L, NCardHolder.PropertyName._isFocused, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NCardHolder.PropertyName._hoverTween, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NCardHolder.PropertyName._currentPressedAction, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 1L, NCardHolder.PropertyName._isClickable, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).SaveGodotObjectData(info);
    GodotSerializationInfo serializationInfo = info;
    StringName cardNode1 = NCardHolder.PropertyName.CardNode;
    NCard cardNode2 = this.CardNode;
    Variant variant = Variant.From<NCard>(ref cardNode2);
    serializationInfo.AddProperty(cardNode1, variant);
    info.AddProperty(NCardHolder.PropertyName._hitbox, Variant.From<NClickableControl>(ref this._hitbox));
    info.AddProperty(NCardHolder.PropertyName._isHovered, Variant.From<bool>(ref this._isHovered));
    info.AddProperty(NCardHolder.PropertyName._isFocused, Variant.From<bool>(ref this._isFocused));
    info.AddProperty(NCardHolder.PropertyName._hoverTween, Variant.From<Tween>(ref this._hoverTween));
    info.AddProperty(NCardHolder.PropertyName._currentPressedAction, Variant.From<InputEventMouseButton>(ref this._currentPressedAction));
    info.AddProperty(NCardHolder.PropertyName._isClickable, Variant.From<bool>(ref this._isClickable));
    info.AddSignalEventDelegate(NCardHolder.SignalName.Pressed, (Delegate) this.backing_Pressed);
    info.AddSignalEventDelegate(NCardHolder.SignalName.AltPressed, (Delegate) this.backing_AltPressed);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NCardHolder.PropertyName.CardNode, ref variant1))
      this.CardNode = ((Variant) ref variant1).As<NCard>();
    Variant variant2;
    if (info.TryGetProperty(NCardHolder.PropertyName._hitbox, ref variant2))
      this._hitbox = ((Variant) ref variant2).As<NClickableControl>();
    Variant variant3;
    if (info.TryGetProperty(NCardHolder.PropertyName._isHovered, ref variant3))
      this._isHovered = ((Variant) ref variant3).As<bool>();
    Variant variant4;
    if (info.TryGetProperty(NCardHolder.PropertyName._isFocused, ref variant4))
      this._isFocused = ((Variant) ref variant4).As<bool>();
    Variant variant5;
    if (info.TryGetProperty(NCardHolder.PropertyName._hoverTween, ref variant5))
      this._hoverTween = ((Variant) ref variant5).As<Tween>();
    Variant variant6;
    if (info.TryGetProperty(NCardHolder.PropertyName._currentPressedAction, ref variant6))
      this._currentPressedAction = ((Variant) ref variant6).As<InputEventMouseButton>();
    Variant variant7;
    if (info.TryGetProperty(NCardHolder.PropertyName._isClickable, ref variant7))
      this._isClickable = ((Variant) ref variant7).As<bool>();
    NCardHolder.PressedEventHandler pressedEventHandler1;
    if (info.TryGetSignalEventDelegate<NCardHolder.PressedEventHandler>(NCardHolder.SignalName.Pressed, ref pressedEventHandler1))
      this.backing_Pressed = pressedEventHandler1;
    NCardHolder.AltPressedEventHandler pressedEventHandler2;
    if (!info.TryGetSignalEventDelegate<NCardHolder.AltPressedEventHandler>(NCardHolder.SignalName.AltPressed, ref pressedEventHandler2))
      return;
    this.backing_AltPressed = pressedEventHandler2;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<MethodInfo> GetGodotSignalList()
  {
    return new List<MethodInfo>(2)
    {
      new MethodInfo(NCardHolder.SignalName.Pressed, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("cardHolder"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false)
      }, (List<Variant>) null),
      new MethodInfo(NCardHolder.SignalName.AltPressed, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("cardHolder"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false)
      }, (List<Variant>) null)
    };
  }

  public event NCardHolder.PressedEventHandler Pressed
  {
    add => this.backing_Pressed += value;
    remove => this.backing_Pressed -= value;
  }

  protected void EmitSignalPressed(NCardHolder cardHolder)
  {
    ((GodotObject) this).EmitSignal(NCardHolder.SignalName.Pressed, new Variant[1]
    {
      Variant.op_Implicit((GodotObject) cardHolder)
    });
  }

  public event NCardHolder.AltPressedEventHandler AltPressed
  {
    add => this.backing_AltPressed += value;
    remove => this.backing_AltPressed -= value;
  }

  protected void EmitSignalAltPressed(NCardHolder cardHolder)
  {
    ((GodotObject) this).EmitSignal(NCardHolder.SignalName.AltPressed, new Variant[1]
    {
      Variant.op_Implicit((GodotObject) cardHolder)
    });
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RaiseGodotClassSignalCallbacks(
    in godot_string_name signal,
    NativeVariantPtrArgs args)
  {
    if (StringName.op_Equality(ref signal, NCardHolder.SignalName.Pressed) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      NCardHolder.PressedEventHandler backingPressed = this.backing_Pressed;
      if (backingPressed == null)
        return;
      backingPressed(VariantUtils.ConvertTo<NCardHolder>(ref ((NativeVariantPtrArgs) ref args)[0]));
    }
    else if (StringName.op_Equality(ref signal, NCardHolder.SignalName.AltPressed) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      NCardHolder.AltPressedEventHandler backingAltPressed = this.backing_AltPressed;
      if (backingAltPressed == null)
        return;
      backingAltPressed(VariantUtils.ConvertTo<NCardHolder>(ref ((NativeVariantPtrArgs) ref args)[0]));
    }
    else
      ((GodotObject) this).RaiseGodotClassSignalCallbacks(ref signal, args);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassSignal(in godot_string_name signal)
  {
    return StringName.op_Equality(ref signal, NCardHolder.SignalName.Pressed) || StringName.op_Equality(ref signal, NCardHolder.SignalName.AltPressed) || base.HasGodotClassSignal(ref signal);
  }

  [Signal]
  public delegate void PressedEventHandler(
  #nullable enable
  NCardHolder cardHolder);

  [Signal]
  public delegate void AltPressedEventHandler(NCardHolder cardHolder);

  public class MethodName : Control.MethodName
  {
    public static readonly 
    #nullable disable
    StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName SetClickable = StringName.op_Implicit(nameof (SetClickable));
    public static readonly StringName ConnectSignals = StringName.op_Implicit(nameof (ConnectSignals));
    public static readonly StringName _GuiInput = StringName.op_Implicit(nameof (_GuiInput));
    public static readonly StringName EmitPressed = StringName.op_Implicit(nameof (EmitPressed));
    public static readonly StringName EmitAltPressed = StringName.op_Implicit(nameof (EmitAltPressed));
    public static readonly StringName SetCard = StringName.op_Implicit(nameof (SetCard));
    public static readonly StringName OnCardReassigned = StringName.op_Implicit(nameof (OnCardReassigned));
    public static readonly StringName OnMousePressed = StringName.op_Implicit(nameof (OnMousePressed));
    public static readonly StringName OnMouseReleased = StringName.op_Implicit(nameof (OnMouseReleased));
    public static readonly StringName OnFocus = StringName.op_Implicit(nameof (OnFocus));
    public static readonly StringName CreateHoverTips = StringName.op_Implicit(nameof (CreateHoverTips));
    public static readonly StringName ClearHoverTips = StringName.op_Implicit(nameof (ClearHoverTips));
    public static readonly StringName OnUnfocus = StringName.op_Implicit(nameof (OnUnfocus));
    public static readonly StringName RefreshFocusState = StringName.op_Implicit(nameof (RefreshFocusState));
    public static readonly StringName DoCardHoverEffects = StringName.op_Implicit(nameof (DoCardHoverEffects));
    public static readonly StringName OnChildExitingTree = StringName.op_Implicit(nameof (OnChildExitingTree));
    public static readonly StringName Clear = StringName.op_Implicit(nameof (Clear));
  }

  public class PropertyName : Control.PropertyName
  {
    public static readonly StringName HoverScale = StringName.op_Implicit(nameof (HoverScale));
    public static readonly StringName SmallScale = StringName.op_Implicit(nameof (SmallScale));
    public static readonly StringName Hitbox = StringName.op_Implicit(nameof (Hitbox));
    public static readonly StringName CardNode = StringName.op_Implicit(nameof (CardNode));
    public static readonly StringName IsShowingUpgradedCard = StringName.op_Implicit(nameof (IsShowingUpgradedCard));
    public static readonly StringName CanBeFocused = StringName.op_Implicit(nameof (CanBeFocused));
    public static readonly StringName _hitbox = StringName.op_Implicit(nameof (_hitbox));
    public static readonly StringName _isHovered = StringName.op_Implicit(nameof (_isHovered));
    public static readonly StringName _isFocused = StringName.op_Implicit(nameof (_isFocused));
    public static readonly StringName _hoverTween = StringName.op_Implicit(nameof (_hoverTween));
    public static readonly StringName _currentPressedAction = StringName.op_Implicit(nameof (_currentPressedAction));
    public static readonly StringName _isClickable = StringName.op_Implicit(nameof (_isClickable));
  }

  public class SignalName : Control.SignalName
  {
    public static readonly StringName Pressed = StringName.op_Implicit(nameof (Pressed));
    public static readonly StringName AltPressed = StringName.op_Implicit(nameof (AltPressed));
  }
}
