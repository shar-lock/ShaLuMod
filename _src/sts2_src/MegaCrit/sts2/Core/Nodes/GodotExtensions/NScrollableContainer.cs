// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.GodotExtensions.NScrollableContainer
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.ControllerInput;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using System;
using System.Collections.Generic;
using System.ComponentModel;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.GodotExtensions;

[ScriptPath("res://src/Core/Nodes/GodotExtensions/NScrollableContainer.cs")]
public class NScrollableContainer : Control
{
  private float _controllerScrollAmount = 400f;
  private float _startDragPosY;
  private float _targetDragPosY;
  private bool _isDragging;
  private float _paddingTop;
  private float _paddingBottom;
  private Control? _content;
  private bool _scrollbarPressed;
  private bool _disableScrollingIfContentFits;

  private float ScrollViewportTop
  {
    get => this._content != null ? ((Node) this._content).GetParent<Control>().Position.Y : 0.0f;
  }

  private float ScrollViewportSize
  {
    get => this._content != null ? ((Node) this._content).GetParent<Control>().Size.Y : 0.0f;
  }

  private float ScrollLimitBottom
  {
    get
    {
      return this._content != null ? (float) -((double) this._paddingBottom + (double) this._paddingTop + (double) this._content.Size.Y) + ((Node) this._content).GetParent<Control>().Size.Y : 0.0f;
    }
  }

  public NScrollbar Scrollbar { get; private set; }

  public override void _Ready()
  {
    this._content = ((Node) this).GetNodeOrNull<Control>(NodePath.op_Implicit("Content")) ?? ((Node) this).GetNodeOrNull<Control>(NodePath.op_Implicit("Mask/Content"));
    this.Scrollbar = ((Node) this).GetNode<NScrollbar>(NodePath.op_Implicit("Scrollbar"));
    this.SetContent(this._content);
    ((CanvasItem) this.Scrollbar).Visible = false;
    ((GodotObject) this.Scrollbar).Connect(NScrollbar.SignalName.MousePressed, Callable.From<InputEvent>((Action<InputEvent>) (_ => this._scrollbarPressed = true)), 0U);
    ((GodotObject) this.Scrollbar).Connect(NScrollbar.SignalName.MouseReleased, Callable.From<InputEvent>((Action<InputEvent>) (_ => this._scrollbarPressed = false)), 0U);
  }

  public void SetContent(Control? content, float paddingTop = 0.0f, float paddingBottom = 0.0f)
  {
    Callable callable = Callable.From(new Action(this.UpdateScrollLimitBottom));
    if (this._content != null && ((GodotObject) this._content).IsConnected(CanvasItem.SignalName.ItemRectChanged, callable))
      ((GodotObject) this._content).Disconnect(CanvasItem.SignalName.ItemRectChanged, callable);
    this._content = content;
    if (this._content == null)
      return;
    ((GodotObject) this._content).Connect(CanvasItem.SignalName.ItemRectChanged, Callable.From(new Action(this.UpdateScrollLimitBottom)), 0U);
    this.UpdatePadding(paddingTop, paddingBottom);
  }

  public void UpdatePadding(float paddingTop = 0.0f, float paddingBottom = 0.0f)
  {
    this._paddingTop = paddingTop;
    this._paddingBottom = paddingBottom;
    this.UpdateScrollLimitBottom();
  }

  public void DisableScrollingIfContentFits() => this._disableScrollingIfContentFits = true;

  public override void _EnterTree()
  {
    ((GodotObject) ((Node) this).GetViewport()).Connect(Viewport.SignalName.GuiFocusChanged, Callable.From<Control>(new Action<Control>(this.ProcessGuiFocus)), 0U);
  }

  public override void _ExitTree()
  {
    ((GodotObject) ((Node) this).GetViewport()).Disconnect(Viewport.SignalName.GuiFocusChanged, Callable.From<Control>(new Action<Control>(this.ProcessGuiFocus)));
  }

  private void UpdateScrollLimitBottom()
  {
    if (this._content == null)
      return;
    ((CanvasItem) this.Scrollbar).Visible = (double) this._content.Size.Y + (double) this._paddingTop + (double) this._paddingBottom > (double) this.ScrollViewportSize;
    ((Control) this.Scrollbar).MouseFilter = ((CanvasItem) this.Scrollbar).Visible ? (Control.MouseFilterEnum) 0L : (Control.MouseFilterEnum) 2L;
  }

  public override void _GuiInput(InputEvent inputEvent)
  {
    if (!((CanvasItem) this).IsVisibleInTree())
      return;
    this.ProcessMouseEvent(inputEvent);
    this.ProcessScrollEvent(inputEvent);
  }

  public override void _Input(InputEvent inputEvent)
  {
    if (!((CanvasItem) this).IsVisibleInTree())
      return;
    Viewport viewport = ((Node) this).GetViewport();
    if (viewport != null && viewport.GuiGetFocusOwner() != null)
      return;
    this.ProcessControllerEvent(inputEvent);
  }

  private void ProcessControllerEvent(InputEvent inputEvent)
  {
    if (inputEvent.IsActionPressed(MegaInput.up, false, false))
    {
      this._targetDragPosY += this._controllerScrollAmount;
    }
    else
    {
      if (!inputEvent.IsActionPressed(MegaInput.down, false, false))
        return;
      this._targetDragPosY += -this._controllerScrollAmount;
    }
  }

  private void ProcessMouseEvent(InputEvent inputEvent)
  {
    if (this._content == null)
      return;
    switch (inputEvent)
    {
      case InputEventMouseMotion eventMouseMotion:
        if (!this._isDragging)
          break;
        this._targetDragPosY += eventMouseMotion.Relative.Y;
        break;
      case InputEventMouseButton eventMouseButton:
        if (eventMouseButton.ButtonIndex == 1L)
        {
          this._isDragging = eventMouseButton.Pressed;
          if (!eventMouseButton.Pressed)
            break;
          this._startDragPosY = this._content.Position.Y - this._paddingTop;
          this._targetDragPosY = this._startDragPosY;
          break;
        }
        if (eventMouseButton.Pressed)
          break;
        this._isDragging = false;
        break;
    }
  }

  private void ProcessScrollEvent(InputEvent inputEvent)
  {
    this._targetDragPosY += ScrollHelper.GetDragForScrollEvent(inputEvent);
  }

  public override void _Process(double delta)
  {
    if (!((CanvasItem) this).IsVisibleInTree() || this._disableScrollingIfContentFits && !((CanvasItem) this.Scrollbar).Visible)
      return;
    this.UpdateScrollPosition(delta);
  }

  public void InstantlyScrollToTop()
  {
    if (this._content == null)
      throw new InvalidOperationException("No content to scroll!");
    this._targetDragPosY = 0.0f;
    Control content = this._content;
    Vector2 position = this._content.Position;
    position.Y = this._paddingTop;
    Vector2 vector2 = position;
    content.Position = vector2;
    this.Scrollbar.SetValueWithoutAnimation(0.0);
  }

  private void ProcessGuiFocus(Control focusedControl)
  {
    if (this._content == null || !((CanvasItem) this).IsVisibleInTree() || !NControllerManager.Instance.IsUsingController || focusedControl is NDropdownItem || !((Node) this._content).IsAncestorOf((Node) focusedControl))
      return;
    float num1 = this._content.GlobalPosition.Y - focusedControl.GlobalPosition.Y + this.ScrollViewportSize * 0.5f;
    float num2 = Mathf.Max(this.ScrollLimitBottom, 0.0f);
    float num3 = Mathf.Min(this.ScrollLimitBottom, 0.0f);
    this._targetDragPosY = Mathf.Clamp(num1, num3, num2);
  }

  private void UpdateScrollPosition(double delta)
  {
    if (this._content == null)
      return;
    float num1 = this._paddingTop + this._targetDragPosY;
    if (!Mathf.IsEqualApprox(this._content.Position.Y, num1))
    {
      float num2 = (float) Mathf.Sign(this._content.Position.Y - num1);
      float num3 = Mathf.Lerp(this._content.Position.Y, num1, (float) delta * 15f);
      float num4 = (float) Mathf.Sign(num3 - num1);
      if ((double) Math.Abs(num3 - num1) < 0.5 || !Mathf.IsEqualApprox(num2, num4))
        num3 = num1;
      Control content = this._content;
      Vector2 position = this._content.Position;
      position.Y = num3;
      Vector2 vector2 = position;
      content.Position = vector2;
      if (!this._scrollbarPressed && (double) this.ScrollLimitBottom < 0.0)
        this.Scrollbar.SetValueWithoutAnimation((double) Mathf.Clamp((this._content.Position.Y - this._paddingTop) / this.ScrollLimitBottom, 0.0f, 1f) * 100.0);
    }
    if (this._scrollbarPressed)
      this._targetDragPosY = Mathf.Lerp(0.0f, this.ScrollLimitBottom, (float) this.Scrollbar.Value * 0.01f);
    if (this._isDragging)
      return;
    if ((double) this._targetDragPosY < (double) Mathf.Min(this.ScrollLimitBottom, 0.0f))
    {
      this._targetDragPosY = Mathf.Lerp(this._targetDragPosY, this.ScrollLimitBottom, (float) delta * 12f);
    }
    else
    {
      if ((double) this._targetDragPosY <= (double) Mathf.Max(this.ScrollLimitBottom, 0.0f))
        return;
      this._targetDragPosY = Mathf.Lerp(this._targetDragPosY, 0.0f, (float) delta * 12f);
    }
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(16 /*0x10*/)
    {
      new MethodInfo(NScrollableContainer.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NScrollableContainer.MethodName.SetContent, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("content"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false),
        new PropertyInfo((Variant.Type) 3L, StringName.op_Implicit("paddingTop"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false),
        new PropertyInfo((Variant.Type) 3L, StringName.op_Implicit("paddingBottom"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NScrollableContainer.MethodName.UpdatePadding, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 3L, StringName.op_Implicit("paddingTop"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false),
        new PropertyInfo((Variant.Type) 3L, StringName.op_Implicit("paddingBottom"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NScrollableContainer.MethodName.DisableScrollingIfContentFits, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NScrollableContainer.MethodName._EnterTree, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NScrollableContainer.MethodName._ExitTree, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NScrollableContainer.MethodName.UpdateScrollLimitBottom, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NScrollableContainer.MethodName._GuiInput, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("inputEvent"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("InputEvent"), false)
      }, (List<Variant>) null),
      new MethodInfo(NScrollableContainer.MethodName._Input, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("inputEvent"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("InputEvent"), false)
      }, (List<Variant>) null),
      new MethodInfo(NScrollableContainer.MethodName.ProcessControllerEvent, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("inputEvent"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("InputEvent"), false)
      }, (List<Variant>) null),
      new MethodInfo(NScrollableContainer.MethodName.ProcessMouseEvent, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("inputEvent"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("InputEvent"), false)
      }, (List<Variant>) null),
      new MethodInfo(NScrollableContainer.MethodName.ProcessScrollEvent, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("inputEvent"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("InputEvent"), false)
      }, (List<Variant>) null),
      new MethodInfo(NScrollableContainer.MethodName._Process, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 3L, StringName.op_Implicit("delta"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NScrollableContainer.MethodName.InstantlyScrollToTop, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NScrollableContainer.MethodName.ProcessGuiFocus, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("focusedControl"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false)
      }, (List<Variant>) null),
      new MethodInfo(NScrollableContainer.MethodName.UpdateScrollPosition, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 3L, StringName.op_Implicit("delta"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NScrollableContainer.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NScrollableContainer.MethodName.SetContent) && ((NativeVariantPtrArgs) ref args).Count == 3)
    {
      this.SetContent(VariantUtils.ConvertTo<Control>(ref ((NativeVariantPtrArgs) ref args)[0]), VariantUtils.ConvertTo<float>(ref ((NativeVariantPtrArgs) ref args)[1]), VariantUtils.ConvertTo<float>(ref ((NativeVariantPtrArgs) ref args)[2]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NScrollableContainer.MethodName.UpdatePadding) && ((NativeVariantPtrArgs) ref args).Count == 2)
    {
      this.UpdatePadding(VariantUtils.ConvertTo<float>(ref ((NativeVariantPtrArgs) ref args)[0]), VariantUtils.ConvertTo<float>(ref ((NativeVariantPtrArgs) ref args)[1]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NScrollableContainer.MethodName.DisableScrollingIfContentFits) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.DisableScrollingIfContentFits();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NScrollableContainer.MethodName._EnterTree) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._EnterTree();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NScrollableContainer.MethodName._ExitTree) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._ExitTree();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NScrollableContainer.MethodName.UpdateScrollLimitBottom) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.UpdateScrollLimitBottom();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NScrollableContainer.MethodName._GuiInput) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      base._GuiInput(VariantUtils.ConvertTo<InputEvent>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NScrollableContainer.MethodName._Input) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      ((Node) this)._Input(VariantUtils.ConvertTo<InputEvent>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NScrollableContainer.MethodName.ProcessControllerEvent) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.ProcessControllerEvent(VariantUtils.ConvertTo<InputEvent>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NScrollableContainer.MethodName.ProcessMouseEvent) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.ProcessMouseEvent(VariantUtils.ConvertTo<InputEvent>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NScrollableContainer.MethodName.ProcessScrollEvent) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.ProcessScrollEvent(VariantUtils.ConvertTo<InputEvent>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NScrollableContainer.MethodName._Process) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      ((Node) this)._Process(VariantUtils.ConvertTo<double>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NScrollableContainer.MethodName.InstantlyScrollToTop) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.InstantlyScrollToTop();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NScrollableContainer.MethodName.ProcessGuiFocus) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.ProcessGuiFocus(VariantUtils.ConvertTo<Control>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NScrollableContainer.MethodName.UpdateScrollPosition) || ((NativeVariantPtrArgs) ref args).Count != 1)
      return base.InvokeGodotClassMethod(ref method, args, ref ret);
    this.UpdateScrollPosition(VariantUtils.ConvertTo<double>(ref ((NativeVariantPtrArgs) ref args)[0]));
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NScrollableContainer.MethodName._Ready) || StringName.op_Equality(ref method, NScrollableContainer.MethodName.SetContent) || StringName.op_Equality(ref method, NScrollableContainer.MethodName.UpdatePadding) || StringName.op_Equality(ref method, NScrollableContainer.MethodName.DisableScrollingIfContentFits) || StringName.op_Equality(ref method, NScrollableContainer.MethodName._EnterTree) || StringName.op_Equality(ref method, NScrollableContainer.MethodName._ExitTree) || StringName.op_Equality(ref method, NScrollableContainer.MethodName.UpdateScrollLimitBottom) || StringName.op_Equality(ref method, NScrollableContainer.MethodName._GuiInput) || StringName.op_Equality(ref method, NScrollableContainer.MethodName._Input) || StringName.op_Equality(ref method, NScrollableContainer.MethodName.ProcessControllerEvent) || StringName.op_Equality(ref method, NScrollableContainer.MethodName.ProcessMouseEvent) || StringName.op_Equality(ref method, NScrollableContainer.MethodName.ProcessScrollEvent) || StringName.op_Equality(ref method, NScrollableContainer.MethodName._Process) || StringName.op_Equality(ref method, NScrollableContainer.MethodName.InstantlyScrollToTop) || StringName.op_Equality(ref method, NScrollableContainer.MethodName.ProcessGuiFocus) || StringName.op_Equality(ref method, NScrollableContainer.MethodName.UpdateScrollPosition) || base.HasGodotClassMethod(ref method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NScrollableContainer.PropertyName.Scrollbar))
    {
      this.Scrollbar = VariantUtils.ConvertTo<NScrollbar>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NScrollableContainer.PropertyName._controllerScrollAmount))
    {
      this._controllerScrollAmount = VariantUtils.ConvertTo<float>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NScrollableContainer.PropertyName._startDragPosY))
    {
      this._startDragPosY = VariantUtils.ConvertTo<float>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NScrollableContainer.PropertyName._targetDragPosY))
    {
      this._targetDragPosY = VariantUtils.ConvertTo<float>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NScrollableContainer.PropertyName._isDragging))
    {
      this._isDragging = VariantUtils.ConvertTo<bool>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NScrollableContainer.PropertyName._paddingTop))
    {
      this._paddingTop = VariantUtils.ConvertTo<float>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NScrollableContainer.PropertyName._paddingBottom))
    {
      this._paddingBottom = VariantUtils.ConvertTo<float>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NScrollableContainer.PropertyName._content))
    {
      this._content = VariantUtils.ConvertTo<Control>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NScrollableContainer.PropertyName._scrollbarPressed))
    {
      this._scrollbarPressed = VariantUtils.ConvertTo<bool>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NScrollableContainer.PropertyName._disableScrollingIfContentFits))
      return ((GodotObject) this).SetGodotClassPropertyValue(ref name, ref value);
    this._disableScrollingIfContentFits = VariantUtils.ConvertTo<bool>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NScrollableContainer.PropertyName.ScrollViewportTop))
    {
      ref godot_variant local = ref value;
      float scrollViewportTop = this.ScrollViewportTop;
      godot_variant from = VariantUtils.CreateFrom<float>(ref scrollViewportTop);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NScrollableContainer.PropertyName.ScrollViewportSize))
    {
      ref godot_variant local = ref value;
      float scrollViewportSize = this.ScrollViewportSize;
      godot_variant from = VariantUtils.CreateFrom<float>(ref scrollViewportSize);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NScrollableContainer.PropertyName.ScrollLimitBottom))
    {
      ref godot_variant local = ref value;
      float scrollLimitBottom = this.ScrollLimitBottom;
      godot_variant from = VariantUtils.CreateFrom<float>(ref scrollLimitBottom);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NScrollableContainer.PropertyName.Scrollbar))
    {
      ref godot_variant local = ref value;
      NScrollbar scrollbar = this.Scrollbar;
      godot_variant from = VariantUtils.CreateFrom<NScrollbar>(ref scrollbar);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NScrollableContainer.PropertyName._controllerScrollAmount))
    {
      value = VariantUtils.CreateFrom<float>(ref this._controllerScrollAmount);
      return true;
    }
    if (StringName.op_Equality(ref name, NScrollableContainer.PropertyName._startDragPosY))
    {
      value = VariantUtils.CreateFrom<float>(ref this._startDragPosY);
      return true;
    }
    if (StringName.op_Equality(ref name, NScrollableContainer.PropertyName._targetDragPosY))
    {
      value = VariantUtils.CreateFrom<float>(ref this._targetDragPosY);
      return true;
    }
    if (StringName.op_Equality(ref name, NScrollableContainer.PropertyName._isDragging))
    {
      value = VariantUtils.CreateFrom<bool>(ref this._isDragging);
      return true;
    }
    if (StringName.op_Equality(ref name, NScrollableContainer.PropertyName._paddingTop))
    {
      value = VariantUtils.CreateFrom<float>(ref this._paddingTop);
      return true;
    }
    if (StringName.op_Equality(ref name, NScrollableContainer.PropertyName._paddingBottom))
    {
      value = VariantUtils.CreateFrom<float>(ref this._paddingBottom);
      return true;
    }
    if (StringName.op_Equality(ref name, NScrollableContainer.PropertyName._content))
    {
      value = VariantUtils.CreateFrom<Control>(ref this._content);
      return true;
    }
    if (StringName.op_Equality(ref name, NScrollableContainer.PropertyName._scrollbarPressed))
    {
      value = VariantUtils.CreateFrom<bool>(ref this._scrollbarPressed);
      return true;
    }
    if (!StringName.op_Equality(ref name, NScrollableContainer.PropertyName._disableScrollingIfContentFits))
      return ((GodotObject) this).GetGodotClassPropertyValue(ref name, ref value);
    value = VariantUtils.CreateFrom<bool>(ref this._disableScrollingIfContentFits);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 3L, NScrollableContainer.PropertyName._controllerScrollAmount, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 3L, NScrollableContainer.PropertyName._startDragPosY, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 3L, NScrollableContainer.PropertyName._targetDragPosY, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 1L, NScrollableContainer.PropertyName._isDragging, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 3L, NScrollableContainer.PropertyName.ScrollViewportTop, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 3L, NScrollableContainer.PropertyName.ScrollViewportSize, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 3L, NScrollableContainer.PropertyName.ScrollLimitBottom, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 3L, NScrollableContainer.PropertyName._paddingTop, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 3L, NScrollableContainer.PropertyName._paddingBottom, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NScrollableContainer.PropertyName._content, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 1L, NScrollableContainer.PropertyName._scrollbarPressed, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 1L, NScrollableContainer.PropertyName._disableScrollingIfContentFits, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NScrollableContainer.PropertyName.Scrollbar, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).SaveGodotObjectData(info);
    GodotSerializationInfo serializationInfo = info;
    StringName scrollbar1 = NScrollableContainer.PropertyName.Scrollbar;
    NScrollbar scrollbar2 = this.Scrollbar;
    Variant variant = Variant.From<NScrollbar>(ref scrollbar2);
    serializationInfo.AddProperty(scrollbar1, variant);
    info.AddProperty(NScrollableContainer.PropertyName._controllerScrollAmount, Variant.From<float>(ref this._controllerScrollAmount));
    info.AddProperty(NScrollableContainer.PropertyName._startDragPosY, Variant.From<float>(ref this._startDragPosY));
    info.AddProperty(NScrollableContainer.PropertyName._targetDragPosY, Variant.From<float>(ref this._targetDragPosY));
    info.AddProperty(NScrollableContainer.PropertyName._isDragging, Variant.From<bool>(ref this._isDragging));
    info.AddProperty(NScrollableContainer.PropertyName._paddingTop, Variant.From<float>(ref this._paddingTop));
    info.AddProperty(NScrollableContainer.PropertyName._paddingBottom, Variant.From<float>(ref this._paddingBottom));
    info.AddProperty(NScrollableContainer.PropertyName._content, Variant.From<Control>(ref this._content));
    info.AddProperty(NScrollableContainer.PropertyName._scrollbarPressed, Variant.From<bool>(ref this._scrollbarPressed));
    info.AddProperty(NScrollableContainer.PropertyName._disableScrollingIfContentFits, Variant.From<bool>(ref this._disableScrollingIfContentFits));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NScrollableContainer.PropertyName.Scrollbar, ref variant1))
      this.Scrollbar = ((Variant) ref variant1).As<NScrollbar>();
    Variant variant2;
    if (info.TryGetProperty(NScrollableContainer.PropertyName._controllerScrollAmount, ref variant2))
      this._controllerScrollAmount = ((Variant) ref variant2).As<float>();
    Variant variant3;
    if (info.TryGetProperty(NScrollableContainer.PropertyName._startDragPosY, ref variant3))
      this._startDragPosY = ((Variant) ref variant3).As<float>();
    Variant variant4;
    if (info.TryGetProperty(NScrollableContainer.PropertyName._targetDragPosY, ref variant4))
      this._targetDragPosY = ((Variant) ref variant4).As<float>();
    Variant variant5;
    if (info.TryGetProperty(NScrollableContainer.PropertyName._isDragging, ref variant5))
      this._isDragging = ((Variant) ref variant5).As<bool>();
    Variant variant6;
    if (info.TryGetProperty(NScrollableContainer.PropertyName._paddingTop, ref variant6))
      this._paddingTop = ((Variant) ref variant6).As<float>();
    Variant variant7;
    if (info.TryGetProperty(NScrollableContainer.PropertyName._paddingBottom, ref variant7))
      this._paddingBottom = ((Variant) ref variant7).As<float>();
    Variant variant8;
    if (info.TryGetProperty(NScrollableContainer.PropertyName._content, ref variant8))
      this._content = ((Variant) ref variant8).As<Control>();
    Variant variant9;
    if (info.TryGetProperty(NScrollableContainer.PropertyName._scrollbarPressed, ref variant9))
      this._scrollbarPressed = ((Variant) ref variant9).As<bool>();
    Variant variant10;
    if (!info.TryGetProperty(NScrollableContainer.PropertyName._disableScrollingIfContentFits, ref variant10))
      return;
    this._disableScrollingIfContentFits = ((Variant) ref variant10).As<bool>();
  }

  public class MethodName : Control.MethodName
  {
    public static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName SetContent = StringName.op_Implicit(nameof (SetContent));
    public static readonly StringName UpdatePadding = StringName.op_Implicit(nameof (UpdatePadding));
    public static readonly StringName DisableScrollingIfContentFits = StringName.op_Implicit(nameof (DisableScrollingIfContentFits));
    public static readonly StringName _EnterTree = StringName.op_Implicit(nameof (_EnterTree));
    public static readonly StringName _ExitTree = StringName.op_Implicit(nameof (_ExitTree));
    public static readonly StringName UpdateScrollLimitBottom = StringName.op_Implicit(nameof (UpdateScrollLimitBottom));
    public static readonly StringName _GuiInput = StringName.op_Implicit(nameof (_GuiInput));
    public static readonly StringName _Input = StringName.op_Implicit(nameof (_Input));
    public static readonly StringName ProcessControllerEvent = StringName.op_Implicit(nameof (ProcessControllerEvent));
    public static readonly StringName ProcessMouseEvent = StringName.op_Implicit(nameof (ProcessMouseEvent));
    public static readonly StringName ProcessScrollEvent = StringName.op_Implicit(nameof (ProcessScrollEvent));
    public static readonly StringName _Process = StringName.op_Implicit(nameof (_Process));
    public static readonly StringName InstantlyScrollToTop = StringName.op_Implicit(nameof (InstantlyScrollToTop));
    public static readonly StringName ProcessGuiFocus = StringName.op_Implicit(nameof (ProcessGuiFocus));
    public static readonly StringName UpdateScrollPosition = StringName.op_Implicit(nameof (UpdateScrollPosition));
  }

  public class PropertyName : Control.PropertyName
  {
    public static readonly StringName ScrollViewportTop = StringName.op_Implicit(nameof (ScrollViewportTop));
    public static readonly StringName ScrollViewportSize = StringName.op_Implicit(nameof (ScrollViewportSize));
    public static readonly StringName ScrollLimitBottom = StringName.op_Implicit(nameof (ScrollLimitBottom));
    public static readonly StringName Scrollbar = StringName.op_Implicit(nameof (Scrollbar));
    public static readonly StringName _controllerScrollAmount = StringName.op_Implicit(nameof (_controllerScrollAmount));
    public static readonly StringName _startDragPosY = StringName.op_Implicit(nameof (_startDragPosY));
    public static readonly StringName _targetDragPosY = StringName.op_Implicit(nameof (_targetDragPosY));
    public static readonly StringName _isDragging = StringName.op_Implicit(nameof (_isDragging));
    public static readonly StringName _paddingTop = StringName.op_Implicit(nameof (_paddingTop));
    public static readonly StringName _paddingBottom = StringName.op_Implicit(nameof (_paddingBottom));
    public static readonly StringName _content = StringName.op_Implicit(nameof (_content));
    public static readonly StringName _scrollbarPressed = StringName.op_Implicit(nameof (_scrollbarPressed));
    public static readonly StringName _disableScrollingIfContentFits = StringName.op_Implicit(nameof (_disableScrollingIfContentFits));
  }

  public class SignalName : Control.SignalName
  {
  }
}
