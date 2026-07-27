// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Screens.Timeline.NSlotsContainer
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Screens.Timeline;

[ScriptPath("res://src/Core/Nodes/Screens/Timeline/NSlotsContainer.cs")]
public class NSlotsContainer : Control
{
  private Control _whatsMoved;
  private Vector2 _dragStartPosition;
  private Vector2 _targetPosition;
  private bool _isDragging;
  private const float _scrollSpeed = 50f;
  private const float _trackpadScrollSpeed = 20f;
  private const float _bounceBackStrength = 36f;
  private const float _lerpSmoothness = 20f;
  private Tween? _tween;
  private Control _epochSlots;

  public override void _Ready()
  {
    this._whatsMoved = ((Node) this).GetNode<Control>(NodePath.op_Implicit("%WhatsMoved"));
    this._epochSlots = ((Node) this).GetNode<Control>(NodePath.op_Implicit("%EpochSlots"));
    this._targetPosition = this._whatsMoved.Position;
    ((GodotObject) this).Connect(CanvasItem.SignalName.VisibilityChanged, Callable.From(new Action(this.OnToggleVisibility)), 0U);
  }

  public override void _EnterTree()
  {
    ((GodotObject) ((Node) this).GetViewport()).Connect(Viewport.SignalName.GuiFocusChanged, Callable.From<Control>(new Action<Control>(this.ProcessGuiFocus)), 0U);
  }

  public override void _ExitTree()
  {
    ((GodotObject) ((Node) this).GetViewport()).Disconnect(Viewport.SignalName.GuiFocusChanged, Callable.From<Control>(new Action<Control>(this.ProcessGuiFocus)));
  }

  public override void _GuiInput(InputEvent inputEvent)
  {
    this.ProcessPanEvent(inputEvent);
    this.ProcessScrollEvent(inputEvent);
  }

  private void ProcessPanEvent(InputEvent inputEvent)
  {
    switch (inputEvent)
    {
      case InputEventMouseButton eventMouseButton when eventMouseButton.ButtonIndex == 1L:
        if (eventMouseButton.Pressed)
        {
          this._isDragging = true;
          this._dragStartPosition = ((InputEventMouse) eventMouseButton).Position;
          break;
        }
        this._isDragging = false;
        break;
      case InputEventMouseMotion eventMouseMotion when this._isDragging:
        this._targetPosition = Vector2.op_Addition(this._targetPosition, new Vector2(Vector2.op_Subtraction(((InputEventMouse) eventMouseMotion).Position, this._dragStartPosition).X, 0.0f));
        this._dragStartPosition = ((InputEventMouse) eventMouseMotion).Position;
        break;
    }
  }

  private void ProcessGuiFocus(Control focusedControl)
  {
    if (!((CanvasItem) this).IsVisibleInTree() || !NControllerManager.Instance.IsUsingController || !((Node) this).IsAncestorOf((Node) focusedControl))
      return;
    this._targetPosition = new Vector2(this._whatsMoved.GlobalPosition.X - ((Node) focusedControl).GetParent<Control>().GlobalPosition.X, this._targetPosition.Y);
  }

  private void ProcessScrollEvent(InputEvent inputEvent)
  {
    switch (inputEvent)
    {
      case InputEventMouseButton eventMouseButton:
        if (eventMouseButton.ButtonIndex == 4L)
        {
          this._targetPosition = Vector2.op_Subtraction(this._targetPosition, new Vector2(50f, 0.0f));
          break;
        }
        if (eventMouseButton.ButtonIndex == 5L)
        {
          this._targetPosition = Vector2.op_Addition(this._targetPosition, new Vector2(50f, 0.0f));
          break;
        }
        if (eventMouseButton.ButtonIndex == 7L)
        {
          this._targetPosition = Vector2.op_Subtraction(this._targetPosition, new Vector2(50f, 0.0f));
          break;
        }
        if (eventMouseButton.ButtonIndex != 6L)
          break;
        this._targetPosition = Vector2.op_Addition(this._targetPosition, new Vector2(50f, 0.0f));
        break;
      case InputEventPanGesture inputEventPanGesture:
        this._targetPosition = Vector2.op_Addition(this._targetPosition, new Vector2((float) (-(double) inputEventPanGesture.Delta.X * 20.0), 0.0f));
        break;
    }
  }

  public override void _Process(double delta)
  {
    float num1 = (float) Mathf.Sign(this._whatsMoved.Position.X - this._targetPosition.X);
    Control whatsMoved = this._whatsMoved;
    Vector2 position = this._whatsMoved.Position;
    Vector2 vector2 = ((Vector2) ref position).Lerp(this._targetPosition, (float) delta * 20f);
    whatsMoved.Position = vector2;
    float num2 = (float) Mathf.Sign(this._whatsMoved.Position.X - this._targetPosition.X);
    if ((double) Math.Abs(this._whatsMoved.Position.X - this._targetPosition.X) < 0.5 || !Mathf.IsEqualApprox(num1, num2))
      this._whatsMoved.Position = this._targetPosition;
    if (this._isDragging)
      return;
    float num3 = this._targetPosition.X;
    float num4 = this._epochSlots.Position.X - this._whatsMoved.Size.X;
    float num5 = this._epochSlots.Position.X + this._epochSlots.Size.X - this._whatsMoved.Size.X;
    if ((double) num3 < (double) num4)
      num3 = Mathf.Lerp(num3, num4, (float) delta * 36f);
    else if ((double) num3 > (double) num5)
      num3 = Mathf.Lerp(num3, num5, (float) delta * 36f);
    this._targetPosition = new Vector2(num3, this._targetPosition.Y);
  }

  private void OnToggleVisibility()
  {
    if (!((CanvasItem) this).Visible)
      return;
    this._targetPosition = this._whatsMoved.Position;
    this._dragStartPosition = Vector2.Zero;
  }

  public void Reset()
  {
    this._whatsMoved.Position = new Vector2(-960f, this._whatsMoved.Position.Y);
  }

  public async Task LerpToSlot(float slotPositionX)
  {
    float num = (float) ((double) this._whatsMoved.GlobalPosition.X - (double) slotPositionX + 960.0 - 96.0 + ((double) this.Size.X - 1920.0) * 0.5);
    this._tween?.Kill();
    this._tween = ((Node) this).CreateTween();
    this._tween.TweenProperty((GodotObject) this._whatsMoved, NodePath.op_Implicit("global_position:x"), Variant.op_Implicit(num), 2.5).SetEase((Tween.EaseType) 2L).SetTrans((Tween.TransitionType) 7L);
    if (!await this._tween.AwaitFinished((Node) this))
      return;
    this._targetPosition = this._whatsMoved.Position;
  }

  public void SetEnabled(bool enabled)
  {
    this.FocusBehaviorRecursive = enabled ? (Control.FocusBehaviorRecursiveEnum) 2L : (Control.FocusBehaviorRecursiveEnum) 1L;
    this._isDragging = false;
  }

  public float GetInitX => this._whatsMoved.GlobalPosition.X;

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(11)
    {
      new MethodInfo(NSlotsContainer.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NSlotsContainer.MethodName._EnterTree, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NSlotsContainer.MethodName._ExitTree, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NSlotsContainer.MethodName._GuiInput, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("inputEvent"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("InputEvent"), false)
      }, (List<Variant>) null),
      new MethodInfo(NSlotsContainer.MethodName.ProcessPanEvent, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("inputEvent"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("InputEvent"), false)
      }, (List<Variant>) null),
      new MethodInfo(NSlotsContainer.MethodName.ProcessGuiFocus, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("focusedControl"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false)
      }, (List<Variant>) null),
      new MethodInfo(NSlotsContainer.MethodName.ProcessScrollEvent, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("inputEvent"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("InputEvent"), false)
      }, (List<Variant>) null),
      new MethodInfo(NSlotsContainer.MethodName._Process, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 3L, StringName.op_Implicit("delta"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NSlotsContainer.MethodName.OnToggleVisibility, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NSlotsContainer.MethodName.Reset, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NSlotsContainer.MethodName.SetEnabled, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 1L, StringName.op_Implicit("enabled"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NSlotsContainer.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NSlotsContainer.MethodName._EnterTree) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._EnterTree();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NSlotsContainer.MethodName._ExitTree) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._ExitTree();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NSlotsContainer.MethodName._GuiInput) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      base._GuiInput(VariantUtils.ConvertTo<InputEvent>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NSlotsContainer.MethodName.ProcessPanEvent) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.ProcessPanEvent(VariantUtils.ConvertTo<InputEvent>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NSlotsContainer.MethodName.ProcessGuiFocus) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.ProcessGuiFocus(VariantUtils.ConvertTo<Control>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NSlotsContainer.MethodName.ProcessScrollEvent) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.ProcessScrollEvent(VariantUtils.ConvertTo<InputEvent>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NSlotsContainer.MethodName._Process) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      ((Node) this)._Process(VariantUtils.ConvertTo<double>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NSlotsContainer.MethodName.OnToggleVisibility) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnToggleVisibility();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NSlotsContainer.MethodName.Reset) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.Reset();
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NSlotsContainer.MethodName.SetEnabled) || ((NativeVariantPtrArgs) ref args).Count != 1)
      return base.InvokeGodotClassMethod(ref method, args, ref ret);
    this.SetEnabled(VariantUtils.ConvertTo<bool>(ref ((NativeVariantPtrArgs) ref args)[0]));
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NSlotsContainer.MethodName._Ready) || StringName.op_Equality(ref method, NSlotsContainer.MethodName._EnterTree) || StringName.op_Equality(ref method, NSlotsContainer.MethodName._ExitTree) || StringName.op_Equality(ref method, NSlotsContainer.MethodName._GuiInput) || StringName.op_Equality(ref method, NSlotsContainer.MethodName.ProcessPanEvent) || StringName.op_Equality(ref method, NSlotsContainer.MethodName.ProcessGuiFocus) || StringName.op_Equality(ref method, NSlotsContainer.MethodName.ProcessScrollEvent) || StringName.op_Equality(ref method, NSlotsContainer.MethodName._Process) || StringName.op_Equality(ref method, NSlotsContainer.MethodName.OnToggleVisibility) || StringName.op_Equality(ref method, NSlotsContainer.MethodName.Reset) || StringName.op_Equality(ref method, NSlotsContainer.MethodName.SetEnabled) || base.HasGodotClassMethod(ref method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NSlotsContainer.PropertyName._whatsMoved))
    {
      this._whatsMoved = VariantUtils.ConvertTo<Control>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NSlotsContainer.PropertyName._dragStartPosition))
    {
      this._dragStartPosition = VariantUtils.ConvertTo<Vector2>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NSlotsContainer.PropertyName._targetPosition))
    {
      this._targetPosition = VariantUtils.ConvertTo<Vector2>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NSlotsContainer.PropertyName._isDragging))
    {
      this._isDragging = VariantUtils.ConvertTo<bool>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NSlotsContainer.PropertyName._tween))
    {
      this._tween = VariantUtils.ConvertTo<Tween>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NSlotsContainer.PropertyName._epochSlots))
      return ((GodotObject) this).SetGodotClassPropertyValue(ref name, ref value);
    this._epochSlots = VariantUtils.ConvertTo<Control>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NSlotsContainer.PropertyName.GetInitX))
    {
      ref godot_variant local = ref value;
      float getInitX = this.GetInitX;
      godot_variant from = VariantUtils.CreateFrom<float>(ref getInitX);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NSlotsContainer.PropertyName._whatsMoved))
    {
      value = VariantUtils.CreateFrom<Control>(ref this._whatsMoved);
      return true;
    }
    if (StringName.op_Equality(ref name, NSlotsContainer.PropertyName._dragStartPosition))
    {
      value = VariantUtils.CreateFrom<Vector2>(ref this._dragStartPosition);
      return true;
    }
    if (StringName.op_Equality(ref name, NSlotsContainer.PropertyName._targetPosition))
    {
      value = VariantUtils.CreateFrom<Vector2>(ref this._targetPosition);
      return true;
    }
    if (StringName.op_Equality(ref name, NSlotsContainer.PropertyName._isDragging))
    {
      value = VariantUtils.CreateFrom<bool>(ref this._isDragging);
      return true;
    }
    if (StringName.op_Equality(ref name, NSlotsContainer.PropertyName._tween))
    {
      value = VariantUtils.CreateFrom<Tween>(ref this._tween);
      return true;
    }
    if (!StringName.op_Equality(ref name, NSlotsContainer.PropertyName._epochSlots))
      return ((GodotObject) this).GetGodotClassPropertyValue(ref name, ref value);
    value = VariantUtils.CreateFrom<Control>(ref this._epochSlots);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NSlotsContainer.PropertyName._whatsMoved, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 5L, NSlotsContainer.PropertyName._dragStartPosition, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 5L, NSlotsContainer.PropertyName._targetPosition, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 1L, NSlotsContainer.PropertyName._isDragging, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NSlotsContainer.PropertyName._tween, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NSlotsContainer.PropertyName._epochSlots, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 3L, NSlotsContainer.PropertyName.GetInitX, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).SaveGodotObjectData(info);
    info.AddProperty(NSlotsContainer.PropertyName._whatsMoved, Variant.From<Control>(ref this._whatsMoved));
    info.AddProperty(NSlotsContainer.PropertyName._dragStartPosition, Variant.From<Vector2>(ref this._dragStartPosition));
    info.AddProperty(NSlotsContainer.PropertyName._targetPosition, Variant.From<Vector2>(ref this._targetPosition));
    info.AddProperty(NSlotsContainer.PropertyName._isDragging, Variant.From<bool>(ref this._isDragging));
    info.AddProperty(NSlotsContainer.PropertyName._tween, Variant.From<Tween>(ref this._tween));
    info.AddProperty(NSlotsContainer.PropertyName._epochSlots, Variant.From<Control>(ref this._epochSlots));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NSlotsContainer.PropertyName._whatsMoved, ref variant1))
      this._whatsMoved = ((Variant) ref variant1).As<Control>();
    Variant variant2;
    if (info.TryGetProperty(NSlotsContainer.PropertyName._dragStartPosition, ref variant2))
      this._dragStartPosition = ((Variant) ref variant2).As<Vector2>();
    Variant variant3;
    if (info.TryGetProperty(NSlotsContainer.PropertyName._targetPosition, ref variant3))
      this._targetPosition = ((Variant) ref variant3).As<Vector2>();
    Variant variant4;
    if (info.TryGetProperty(NSlotsContainer.PropertyName._isDragging, ref variant4))
      this._isDragging = ((Variant) ref variant4).As<bool>();
    Variant variant5;
    if (info.TryGetProperty(NSlotsContainer.PropertyName._tween, ref variant5))
      this._tween = ((Variant) ref variant5).As<Tween>();
    Variant variant6;
    if (!info.TryGetProperty(NSlotsContainer.PropertyName._epochSlots, ref variant6))
      return;
    this._epochSlots = ((Variant) ref variant6).As<Control>();
  }

  public class MethodName : Control.MethodName
  {
    public static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName _EnterTree = StringName.op_Implicit(nameof (_EnterTree));
    public static readonly StringName _ExitTree = StringName.op_Implicit(nameof (_ExitTree));
    public static readonly StringName _GuiInput = StringName.op_Implicit(nameof (_GuiInput));
    public static readonly StringName ProcessPanEvent = StringName.op_Implicit(nameof (ProcessPanEvent));
    public static readonly StringName ProcessGuiFocus = StringName.op_Implicit(nameof (ProcessGuiFocus));
    public static readonly StringName ProcessScrollEvent = StringName.op_Implicit(nameof (ProcessScrollEvent));
    public static readonly StringName _Process = StringName.op_Implicit(nameof (_Process));
    public static readonly StringName OnToggleVisibility = StringName.op_Implicit(nameof (OnToggleVisibility));
    public static readonly StringName Reset = StringName.op_Implicit(nameof (Reset));
    public static readonly StringName SetEnabled = StringName.op_Implicit(nameof (SetEnabled));
  }

  public class PropertyName : Control.PropertyName
  {
    public static readonly StringName GetInitX = StringName.op_Implicit(nameof (GetInitX));
    public static readonly StringName _whatsMoved = StringName.op_Implicit(nameof (_whatsMoved));
    public static readonly StringName _dragStartPosition = StringName.op_Implicit(nameof (_dragStartPosition));
    public static readonly StringName _targetPosition = StringName.op_Implicit(nameof (_targetPosition));
    public static readonly StringName _isDragging = StringName.op_Implicit(nameof (_isDragging));
    public static readonly StringName _tween = StringName.op_Implicit(nameof (_tween));
    public static readonly StringName _epochSlots = StringName.op_Implicit(nameof (_epochSlots));
  }

  public class SignalName : Control.SignalName
  {
  }
}
