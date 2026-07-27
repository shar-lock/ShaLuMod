// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Screens.Shops.NMerchantSlot
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.ControllerInput;
using MegaCrit.Sts2.Core.Entities.Merchant;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.HoverTips;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Screens.Shops;

[ScriptPath("res://src/Core/Nodes/Screens/Shops/NMerchantSlot.cs")]
public abstract class NMerchantSlot : Control
{
  private bool _isHovered;
  private static readonly Vector2 _hoverScale = Vector2.op_Multiply(Vector2.One, 0.8f);
  private static readonly Vector2 _smallScale = Vector2.op_Multiply(Vector2.One, 0.65f);
  protected NClickableControl _hitbox;
  protected MegaLabel _costLabel;
  private Tween? _hoverTween;
  private Tween? _purchaseFailedTween;
  private NMerchantInventory? _merchantRug;
  private bool _ignoreMouseRelease;
  private float? _originalVisualPosition;
  private 
  #nullable disable
  NMerchantSlot.HoveredEventHandler backing_Hovered;
  private NMerchantSlot.UnhoveredEventHandler backing_Unhovered;

  public 
  #nullable enable
  NClickableControl Hitbox => this._hitbox;

  public abstract MerchantEntry Entry { get; }

  protected abstract CanvasItem Visual { get; }

  protected Player? Player => this._merchantRug?.Inventory?.Player;

  public void Initialize(NMerchantInventory rug)
  {
    this._merchantRug = rug;
    this.Player.GoldChanged += new Action(this.UpdateVisual);
    ((GodotObject) this).Connect(NMerchantSlot.SignalName.Hovered, Callable.From<NMerchantSlot>(new Action<NMerchantSlot>(this.OnMerchantHandHovered)), 0U);
    ((GodotObject) this).Connect(NMerchantSlot.SignalName.Unhovered, Callable.From<NMerchantSlot>(new Action<NMerchantSlot>(this.OnMerchantHandUnhovered)), 0U);
  }

  public override void _Ready()
  {
    if (((object) this).GetType() != typeof (NMerchantSlot))
    {
      Log.Error($"{((object) this).GetType()}");
      throw new InvalidOperationException("Don't call base._Ready()! Call ConnectSignals() instead.");
    }
    this.ConnectSignals();
  }

  protected virtual void ConnectSignals()
  {
    this._hitbox = ((Node) this).GetNode<NClickableControl>(NodePath.op_Implicit("%Hitbox"));
    ((GodotObject) this).Connect(Control.SignalName.FocusEntered, Callable.From(new Action(this.OnFocus)), 0U);
    ((GodotObject) this).Connect(Control.SignalName.FocusExited, Callable.From(new Action(this.OnUnfocus)), 0U);
    ((GodotObject) this._hitbox).Connect(Control.SignalName.MouseEntered, Callable.From(new Action(this.OnFocus)), 0U);
    ((GodotObject) this._hitbox).Connect(Control.SignalName.MouseExited, Callable.From(new Action(this.OnUnfocus)), 0U);
    ((GodotObject) this._hitbox).Connect(NClickableControl.SignalName.MousePressed, Callable.From<InputEvent>(new Action<InputEvent>(this.OnMousePressed)), 0U);
    ((GodotObject) this._hitbox).Connect(NClickableControl.SignalName.MouseReleased, Callable.From<InputEvent>(new Action<InputEvent>(this.OnMouseReleased)), 0U);
    this._costLabel = ((Node) this).GetNode<MegaLabel>(NodePath.op_Implicit("%CostLabel"));
  }

  public override void _ExitTree()
  {
    ((GodotObject) this).Disconnect(NMerchantSlot.SignalName.Unhovered, Callable.From<NMerchantSlot>(new Action<NMerchantSlot>(this.OnMerchantHandUnhovered)));
    ((GodotObject) this._hitbox).Disconnect(Control.SignalName.MouseExited, Callable.From(new Action(this.OnUnfocus)));
    ((GodotObject) this).Disconnect(Control.SignalName.FocusExited, Callable.From(new Action(this.OnUnfocus)));
    this._hoverTween?.Kill();
    if (this.Player == null)
      return;
    this.Player.GoldChanged -= new Action(this.UpdateVisual);
  }

  public override void _GuiInput(InputEvent inputEvent)
  {
    if (inputEvent.IsActionPressed(MegaInput.select, false, false))
    {
      TaskHelper.RunSafely(this.OnSelected());
    }
    else
    {
      if (!inputEvent.IsActionPressed(MegaInput.accept, false, false))
        return;
      this.OnPreview();
      ((Node) this).GetViewport().SetInputAsHandled();
    }
  }

  private void OnMousePressed(InputEvent inputEvent) => this._ignoreMouseRelease = false;

  private void OnMouseReleased(InputEvent inputEvent)
  {
    if (!this._isHovered || this._ignoreMouseRelease || !(inputEvent is InputEventMouseButton eventMouseButton))
      return;
    if (eventMouseButton.ButtonIndex == 1L)
      TaskHelper.RunSafely(this.OnSelected());
    else
      this.OnPreview();
  }

  private void OnFocus()
  {
    this._isHovered = true;
    this._hoverTween?.Kill();
    this.Scale = NMerchantSlot._hoverScale;
    this.CreateHoverTip();
    ((GodotObject) this).EmitSignal(NMerchantSlot.SignalName.Hovered, new Variant[1]
    {
      Variant.op_Implicit((GodotObject) this)
    });
  }

  private void OnUnfocus()
  {
    this._isHovered = false;
    this._hoverTween?.Kill();
    this._hoverTween = ((Node) this).CreateTween();
    this._hoverTween.TweenProperty((GodotObject) this, NodePath.op_Implicit("scale"), Variant.op_Implicit(NMerchantSlot._smallScale), 0.5).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 5L);
    this.ClearHoverTip();
    ((GodotObject) this).EmitSignal(NMerchantSlot.SignalName.Unhovered, new Variant[1]
    {
      Variant.op_Implicit((GodotObject) this)
    });
  }

  private async Task OnSelected()
  {
    this.ClearHoverTip();
    await this.OnTryPurchase(this._merchantRug?.Inventory);
    MerchantEntry entry = this.Entry;
    if (entry == null || !entry.IsStocked || !this._isHovered)
      return;
    this.CreateHoverTip();
  }

  protected abstract Task OnTryPurchase(MerchantInventory? inventory);

  protected void TriggerMerchantHandToPointHere()
  {
    this._merchantRug?.MerchantHand.PointAtTarget((Control) this, Vector2.Zero);
    this._merchantRug?.MerchantHand.StopPointing(2f);
  }

  protected virtual void OnPreview()
  {
  }

  protected abstract void CreateHoverTip();

  protected void ClearHoverTip() => NHoverTipSet.Remove((Control) this);

  private void OnMerchantHandHovered(NMerchantSlot _)
  {
    this._merchantRug?.MerchantHand.PointAtTarget((Control) this, Vector2.Zero);
  }

  private void OnMerchantHandUnhovered(NMerchantSlot _)
  {
    this._merchantRug?.MerchantHand.StopPointing(2f);
  }

  protected void OnPurchaseFailed(PurchaseStatus status)
  {
    if (status == PurchaseStatus.Success)
      return;
    if (!this._originalVisualPosition.HasValue)
    {
      if (this.Visual is Node2D visual2)
        this._originalVisualPosition = new float?(visual2.Position.X);
      else if (this.Visual is Control visual1)
        this._originalVisualPosition = new float?(visual1.Position.X);
    }
    this._purchaseFailedTween?.Kill();
    this._purchaseFailedTween = ((Node) this).CreateTween();
    this._purchaseFailedTween.TweenMethod(Callable.From<float>(new Action<float>(this.WiggleAnimation)), Variant.op_Implicit(0.0f), Variant.op_Implicit(2f), 0.40000000596046448).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 4L);
    SfxCmd.Play("event:/sfx/npcs/merchant/merchant_dissapointment");
  }

  protected virtual void UpdateVisual()
  {
    if (!this.Entry.IsStocked)
      return;
    this._costLabel.SetTextAutoSize(this.Entry.Cost.ToString());
  }

  private void WiggleAnimation(float progress)
  {
    if (this.Visual is Node2D visual1)
    {
      Node2D node2D = visual1;
      Vector2 position = visual1.Position;
      position.X = this._originalVisualPosition.Value + (float) Math.Sin((double) progress * 3.1415927410125732 * 2.0) * 10f;
      Vector2 vector2 = position;
      node2D.Position = vector2;
    }
    else
    {
      if (!(this.Visual is Control visual))
        return;
      Control control = visual;
      Vector2 position = visual.Position;
      position.X = this._originalVisualPosition.Value + (float) Math.Sin((double) progress * 3.1415927410125732 * 2.0) * 10f;
      Vector2 vector2 = position;
      control.Position = vector2;
    }
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(18)
    {
      new MethodInfo(NMerchantSlot.MethodName.Initialize, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("rug"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false)
      }, (List<Variant>) null),
      new MethodInfo(NMerchantSlot.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NMerchantSlot.MethodName.ConnectSignals, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NMerchantSlot.MethodName._ExitTree, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NMerchantSlot.MethodName._GuiInput, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("inputEvent"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("InputEvent"), false)
      }, (List<Variant>) null),
      new MethodInfo(NMerchantSlot.MethodName.OnMousePressed, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("inputEvent"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("InputEvent"), false)
      }, (List<Variant>) null),
      new MethodInfo(NMerchantSlot.MethodName.OnMouseReleased, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("inputEvent"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("InputEvent"), false)
      }, (List<Variant>) null),
      new MethodInfo(NMerchantSlot.MethodName.OnFocus, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NMerchantSlot.MethodName.OnUnfocus, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NMerchantSlot.MethodName.TriggerMerchantHandToPointHere, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NMerchantSlot.MethodName.OnPreview, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NMerchantSlot.MethodName.CreateHoverTip, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NMerchantSlot.MethodName.ClearHoverTip, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NMerchantSlot.MethodName.OnMerchantHandHovered, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("_"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false)
      }, (List<Variant>) null),
      new MethodInfo(NMerchantSlot.MethodName.OnMerchantHandUnhovered, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("_"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false)
      }, (List<Variant>) null),
      new MethodInfo(NMerchantSlot.MethodName.OnPurchaseFailed, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 2L, StringName.op_Implicit("status"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NMerchantSlot.MethodName.UpdateVisual, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NMerchantSlot.MethodName.WiggleAnimation, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 3L, StringName.op_Implicit("progress"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NMerchantSlot.MethodName.Initialize) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.Initialize(VariantUtils.ConvertTo<NMerchantInventory>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NMerchantSlot.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NMerchantSlot.MethodName.ConnectSignals) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.ConnectSignals();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NMerchantSlot.MethodName._ExitTree) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._ExitTree();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NMerchantSlot.MethodName._GuiInput) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      base._GuiInput(VariantUtils.ConvertTo<InputEvent>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NMerchantSlot.MethodName.OnMousePressed) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.OnMousePressed(VariantUtils.ConvertTo<InputEvent>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NMerchantSlot.MethodName.OnMouseReleased) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.OnMouseReleased(VariantUtils.ConvertTo<InputEvent>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NMerchantSlot.MethodName.OnFocus) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnFocus();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NMerchantSlot.MethodName.OnUnfocus) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnUnfocus();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NMerchantSlot.MethodName.TriggerMerchantHandToPointHere) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.TriggerMerchantHandToPointHere();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NMerchantSlot.MethodName.OnPreview) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnPreview();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NMerchantSlot.MethodName.CreateHoverTip) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.CreateHoverTip();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NMerchantSlot.MethodName.ClearHoverTip) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.ClearHoverTip();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NMerchantSlot.MethodName.OnMerchantHandHovered) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.OnMerchantHandHovered(VariantUtils.ConvertTo<NMerchantSlot>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NMerchantSlot.MethodName.OnMerchantHandUnhovered) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.OnMerchantHandUnhovered(VariantUtils.ConvertTo<NMerchantSlot>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NMerchantSlot.MethodName.OnPurchaseFailed) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.OnPurchaseFailed(VariantUtils.ConvertTo<PurchaseStatus>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NMerchantSlot.MethodName.UpdateVisual) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.UpdateVisual();
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NMerchantSlot.MethodName.WiggleAnimation) || ((NativeVariantPtrArgs) ref args).Count != 1)
      return base.InvokeGodotClassMethod(ref method, args, ref ret);
    this.WiggleAnimation(VariantUtils.ConvertTo<float>(ref ((NativeVariantPtrArgs) ref args)[0]));
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NMerchantSlot.MethodName.Initialize) || StringName.op_Equality(ref method, NMerchantSlot.MethodName._Ready) || StringName.op_Equality(ref method, NMerchantSlot.MethodName.ConnectSignals) || StringName.op_Equality(ref method, NMerchantSlot.MethodName._ExitTree) || StringName.op_Equality(ref method, NMerchantSlot.MethodName._GuiInput) || StringName.op_Equality(ref method, NMerchantSlot.MethodName.OnMousePressed) || StringName.op_Equality(ref method, NMerchantSlot.MethodName.OnMouseReleased) || StringName.op_Equality(ref method, NMerchantSlot.MethodName.OnFocus) || StringName.op_Equality(ref method, NMerchantSlot.MethodName.OnUnfocus) || StringName.op_Equality(ref method, NMerchantSlot.MethodName.TriggerMerchantHandToPointHere) || StringName.op_Equality(ref method, NMerchantSlot.MethodName.OnPreview) || StringName.op_Equality(ref method, NMerchantSlot.MethodName.CreateHoverTip) || StringName.op_Equality(ref method, NMerchantSlot.MethodName.ClearHoverTip) || StringName.op_Equality(ref method, NMerchantSlot.MethodName.OnMerchantHandHovered) || StringName.op_Equality(ref method, NMerchantSlot.MethodName.OnMerchantHandUnhovered) || StringName.op_Equality(ref method, NMerchantSlot.MethodName.OnPurchaseFailed) || StringName.op_Equality(ref method, NMerchantSlot.MethodName.UpdateVisual) || StringName.op_Equality(ref method, NMerchantSlot.MethodName.WiggleAnimation) || base.HasGodotClassMethod(ref method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NMerchantSlot.PropertyName._isHovered))
    {
      this._isHovered = VariantUtils.ConvertTo<bool>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NMerchantSlot.PropertyName._hitbox))
    {
      this._hitbox = VariantUtils.ConvertTo<NClickableControl>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NMerchantSlot.PropertyName._costLabel))
    {
      this._costLabel = VariantUtils.ConvertTo<MegaLabel>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NMerchantSlot.PropertyName._hoverTween))
    {
      this._hoverTween = VariantUtils.ConvertTo<Tween>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NMerchantSlot.PropertyName._purchaseFailedTween))
    {
      this._purchaseFailedTween = VariantUtils.ConvertTo<Tween>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NMerchantSlot.PropertyName._merchantRug))
    {
      this._merchantRug = VariantUtils.ConvertTo<NMerchantInventory>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NMerchantSlot.PropertyName._ignoreMouseRelease))
      return ((GodotObject) this).SetGodotClassPropertyValue(ref name, ref value);
    this._ignoreMouseRelease = VariantUtils.ConvertTo<bool>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NMerchantSlot.PropertyName.Hitbox))
    {
      ref godot_variant local = ref value;
      NClickableControl hitbox = this.Hitbox;
      godot_variant from = VariantUtils.CreateFrom<NClickableControl>(ref hitbox);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NMerchantSlot.PropertyName.Visual))
    {
      ref godot_variant local = ref value;
      CanvasItem visual = this.Visual;
      godot_variant from = VariantUtils.CreateFrom<CanvasItem>(ref visual);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NMerchantSlot.PropertyName._isHovered))
    {
      value = VariantUtils.CreateFrom<bool>(ref this._isHovered);
      return true;
    }
    if (StringName.op_Equality(ref name, NMerchantSlot.PropertyName._hitbox))
    {
      value = VariantUtils.CreateFrom<NClickableControl>(ref this._hitbox);
      return true;
    }
    if (StringName.op_Equality(ref name, NMerchantSlot.PropertyName._costLabel))
    {
      value = VariantUtils.CreateFrom<MegaLabel>(ref this._costLabel);
      return true;
    }
    if (StringName.op_Equality(ref name, NMerchantSlot.PropertyName._hoverTween))
    {
      value = VariantUtils.CreateFrom<Tween>(ref this._hoverTween);
      return true;
    }
    if (StringName.op_Equality(ref name, NMerchantSlot.PropertyName._purchaseFailedTween))
    {
      value = VariantUtils.CreateFrom<Tween>(ref this._purchaseFailedTween);
      return true;
    }
    if (StringName.op_Equality(ref name, NMerchantSlot.PropertyName._merchantRug))
    {
      value = VariantUtils.CreateFrom<NMerchantInventory>(ref this._merchantRug);
      return true;
    }
    if (!StringName.op_Equality(ref name, NMerchantSlot.PropertyName._ignoreMouseRelease))
      return ((GodotObject) this).GetGodotClassPropertyValue(ref name, ref value);
    value = VariantUtils.CreateFrom<bool>(ref this._ignoreMouseRelease);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 1L, NMerchantSlot.PropertyName._isHovered, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NMerchantSlot.PropertyName._hitbox, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NMerchantSlot.PropertyName.Hitbox, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NMerchantSlot.PropertyName._costLabel, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NMerchantSlot.PropertyName._hoverTween, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NMerchantSlot.PropertyName._purchaseFailedTween, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NMerchantSlot.PropertyName._merchantRug, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 1L, NMerchantSlot.PropertyName._ignoreMouseRelease, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NMerchantSlot.PropertyName.Visual, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).SaveGodotObjectData(info);
    info.AddProperty(NMerchantSlot.PropertyName._isHovered, Variant.From<bool>(ref this._isHovered));
    info.AddProperty(NMerchantSlot.PropertyName._hitbox, Variant.From<NClickableControl>(ref this._hitbox));
    info.AddProperty(NMerchantSlot.PropertyName._costLabel, Variant.From<MegaLabel>(ref this._costLabel));
    info.AddProperty(NMerchantSlot.PropertyName._hoverTween, Variant.From<Tween>(ref this._hoverTween));
    info.AddProperty(NMerchantSlot.PropertyName._purchaseFailedTween, Variant.From<Tween>(ref this._purchaseFailedTween));
    info.AddProperty(NMerchantSlot.PropertyName._merchantRug, Variant.From<NMerchantInventory>(ref this._merchantRug));
    info.AddProperty(NMerchantSlot.PropertyName._ignoreMouseRelease, Variant.From<bool>(ref this._ignoreMouseRelease));
    info.AddSignalEventDelegate(NMerchantSlot.SignalName.Hovered, (Delegate) this.backing_Hovered);
    info.AddSignalEventDelegate(NMerchantSlot.SignalName.Unhovered, (Delegate) this.backing_Unhovered);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NMerchantSlot.PropertyName._isHovered, ref variant1))
      this._isHovered = ((Variant) ref variant1).As<bool>();
    Variant variant2;
    if (info.TryGetProperty(NMerchantSlot.PropertyName._hitbox, ref variant2))
      this._hitbox = ((Variant) ref variant2).As<NClickableControl>();
    Variant variant3;
    if (info.TryGetProperty(NMerchantSlot.PropertyName._costLabel, ref variant3))
      this._costLabel = ((Variant) ref variant3).As<MegaLabel>();
    Variant variant4;
    if (info.TryGetProperty(NMerchantSlot.PropertyName._hoverTween, ref variant4))
      this._hoverTween = ((Variant) ref variant4).As<Tween>();
    Variant variant5;
    if (info.TryGetProperty(NMerchantSlot.PropertyName._purchaseFailedTween, ref variant5))
      this._purchaseFailedTween = ((Variant) ref variant5).As<Tween>();
    Variant variant6;
    if (info.TryGetProperty(NMerchantSlot.PropertyName._merchantRug, ref variant6))
      this._merchantRug = ((Variant) ref variant6).As<NMerchantInventory>();
    Variant variant7;
    if (info.TryGetProperty(NMerchantSlot.PropertyName._ignoreMouseRelease, ref variant7))
      this._ignoreMouseRelease = ((Variant) ref variant7).As<bool>();
    NMerchantSlot.HoveredEventHandler hoveredEventHandler;
    if (info.TryGetSignalEventDelegate<NMerchantSlot.HoveredEventHandler>(NMerchantSlot.SignalName.Hovered, ref hoveredEventHandler))
      this.backing_Hovered = hoveredEventHandler;
    NMerchantSlot.UnhoveredEventHandler unhoveredEventHandler;
    if (!info.TryGetSignalEventDelegate<NMerchantSlot.UnhoveredEventHandler>(NMerchantSlot.SignalName.Unhovered, ref unhoveredEventHandler))
      return;
    this.backing_Unhovered = unhoveredEventHandler;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<MethodInfo> GetGodotSignalList()
  {
    return new List<MethodInfo>(2)
    {
      new MethodInfo(NMerchantSlot.SignalName.Hovered, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("slot"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false)
      }, (List<Variant>) null),
      new MethodInfo(NMerchantSlot.SignalName.Unhovered, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("slot"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false)
      }, (List<Variant>) null)
    };
  }

  public event NMerchantSlot.HoveredEventHandler Hovered
  {
    add => this.backing_Hovered += value;
    remove => this.backing_Hovered -= value;
  }

  protected void EmitSignalHovered(NMerchantSlot slot)
  {
    ((GodotObject) this).EmitSignal(NMerchantSlot.SignalName.Hovered, new Variant[1]
    {
      Variant.op_Implicit((GodotObject) slot)
    });
  }

  public event NMerchantSlot.UnhoveredEventHandler Unhovered
  {
    add => this.backing_Unhovered += value;
    remove => this.backing_Unhovered -= value;
  }

  protected void EmitSignalUnhovered(NMerchantSlot slot)
  {
    ((GodotObject) this).EmitSignal(NMerchantSlot.SignalName.Unhovered, new Variant[1]
    {
      Variant.op_Implicit((GodotObject) slot)
    });
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RaiseGodotClassSignalCallbacks(
    in godot_string_name signal,
    NativeVariantPtrArgs args)
  {
    if (StringName.op_Equality(ref signal, NMerchantSlot.SignalName.Hovered) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      NMerchantSlot.HoveredEventHandler backingHovered = this.backing_Hovered;
      if (backingHovered == null)
        return;
      backingHovered(VariantUtils.ConvertTo<NMerchantSlot>(ref ((NativeVariantPtrArgs) ref args)[0]));
    }
    else if (StringName.op_Equality(ref signal, NMerchantSlot.SignalName.Unhovered) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      NMerchantSlot.UnhoveredEventHandler backingUnhovered = this.backing_Unhovered;
      if (backingUnhovered == null)
        return;
      backingUnhovered(VariantUtils.ConvertTo<NMerchantSlot>(ref ((NativeVariantPtrArgs) ref args)[0]));
    }
    else
      ((GodotObject) this).RaiseGodotClassSignalCallbacks(ref signal, args);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassSignal(in godot_string_name signal)
  {
    return StringName.op_Equality(ref signal, NMerchantSlot.SignalName.Hovered) || StringName.op_Equality(ref signal, NMerchantSlot.SignalName.Unhovered) || base.HasGodotClassSignal(ref signal);
  }

  [Signal]
  public delegate void HoveredEventHandler(
  #nullable enable
  NMerchantSlot slot);

  [Signal]
  public delegate void UnhoveredEventHandler(NMerchantSlot slot);

  public class MethodName : Control.MethodName
  {
    public static readonly 
    #nullable disable
    StringName Initialize = StringName.op_Implicit(nameof (Initialize));
    public static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName ConnectSignals = StringName.op_Implicit(nameof (ConnectSignals));
    public static readonly StringName _ExitTree = StringName.op_Implicit(nameof (_ExitTree));
    public static readonly StringName _GuiInput = StringName.op_Implicit(nameof (_GuiInput));
    public static readonly StringName OnMousePressed = StringName.op_Implicit(nameof (OnMousePressed));
    public static readonly StringName OnMouseReleased = StringName.op_Implicit(nameof (OnMouseReleased));
    public static readonly StringName OnFocus = StringName.op_Implicit(nameof (OnFocus));
    public static readonly StringName OnUnfocus = StringName.op_Implicit(nameof (OnUnfocus));
    public static readonly StringName TriggerMerchantHandToPointHere = StringName.op_Implicit(nameof (TriggerMerchantHandToPointHere));
    public static readonly StringName OnPreview = StringName.op_Implicit(nameof (OnPreview));
    public static readonly StringName CreateHoverTip = StringName.op_Implicit(nameof (CreateHoverTip));
    public static readonly StringName ClearHoverTip = StringName.op_Implicit(nameof (ClearHoverTip));
    public static readonly StringName OnMerchantHandHovered = StringName.op_Implicit(nameof (OnMerchantHandHovered));
    public static readonly StringName OnMerchantHandUnhovered = StringName.op_Implicit(nameof (OnMerchantHandUnhovered));
    public static readonly StringName OnPurchaseFailed = StringName.op_Implicit(nameof (OnPurchaseFailed));
    public static readonly StringName UpdateVisual = StringName.op_Implicit(nameof (UpdateVisual));
    public static readonly StringName WiggleAnimation = StringName.op_Implicit(nameof (WiggleAnimation));
  }

  public class PropertyName : Control.PropertyName
  {
    public static readonly StringName Hitbox = StringName.op_Implicit(nameof (Hitbox));
    public static readonly StringName Visual = StringName.op_Implicit(nameof (Visual));
    public static readonly StringName _isHovered = StringName.op_Implicit(nameof (_isHovered));
    public static readonly StringName _hitbox = StringName.op_Implicit(nameof (_hitbox));
    public static readonly StringName _costLabel = StringName.op_Implicit(nameof (_costLabel));
    public static readonly StringName _hoverTween = StringName.op_Implicit(nameof (_hoverTween));
    public static readonly StringName _purchaseFailedTween = StringName.op_Implicit(nameof (_purchaseFailedTween));
    public static readonly StringName _merchantRug = StringName.op_Implicit(nameof (_merchantRug));
    public static readonly StringName _ignoreMouseRelease = StringName.op_Implicit(nameof (_ignoreMouseRelease));
  }

  public class SignalName : Control.SignalName
  {
    public static readonly StringName Hovered = StringName.op_Implicit(nameof (Hovered));
    public static readonly StringName Unhovered = StringName.op_Implicit(nameof (Unhovered));
  }
}
