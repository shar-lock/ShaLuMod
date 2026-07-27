// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.CommonUi.NDropdownContainer
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.CommonUi;

[ScriptPath("res://src/Core/Nodes/CommonUi/NDropdownContainer.cs")]
public class NDropdownContainer : Control
{
  private NDropdownScrollbar _scrollbar;
  private Control _scrollbarTrain;
  private VBoxContainer _dropdownItems;
  private float _maxHeight;
  private float _contentHeight;
  private Vector2 _startDragPos;
  private Vector2 _targetDragPos = Vector2.Zero;
  private const float _scrollLimitTop = 0.0f;
  private float _scrollLimitBottom;
  private bool _isDragging;

  public override void _Ready()
  {
    this._scrollbar = ((Node) this).GetNode<NDropdownScrollbar>(NodePath.op_Implicit("Scrollbar"));
    this._scrollbarTrain = ((Node) this).GetNode<Control>(NodePath.op_Implicit("Scrollbar/Train"));
    this._dropdownItems = ((Node) this).GetNode<VBoxContainer>(NodePath.op_Implicit("VBoxContainer"));
    ((GodotObject) this).Connect(CanvasItem.SignalName.VisibilityChanged, Callable.From(new Action(this.OnVisibilityChange)), 0U);
    this._maxHeight = this.Size.Y;
  }

  public override void _EnterTree()
  {
    ((Node) this)._EnterTree();
    ((GodotObject) ((Node) this).GetViewport()).Connect(Viewport.SignalName.GuiFocusChanged, Callable.From<Control>(new Action<Control>(this.ProcessGuiFocus)), 0U);
  }

  public override void _ExitTree()
  {
    ((Node) this)._ExitTree();
    ((GodotObject) ((Node) this).GetViewport()).Disconnect(Viewport.SignalName.GuiFocusChanged, Callable.From<Control>(new Action<Control>(this.ProcessGuiFocus)));
  }

  private void OnVisibilityChange()
  {
    if (!((CanvasItem) this).Visible)
      return;
    this._isDragging = false;
  }

  public void RefreshLayout() => ((CanvasItem) this._scrollbar).Visible = this.IsScrollbarNeeded();

  private bool IsScrollbarNeeded()
  {
    this._contentHeight = 0.0f;
    foreach (Node child in ((Node) this._dropdownItems).GetChildren(false))
    {
      if (child is Control control)
        this._contentHeight += control.Size.Y;
    }
    this._scrollLimitBottom = -this._contentHeight + this._maxHeight;
    if ((double) this._contentHeight > (double) this._maxHeight)
    {
      this.Size = new Vector2(this.Size.X, this._maxHeight);
      this._scrollbar.RefreshTrainBounds();
      return true;
    }
    this.Size = new Vector2(this.Size.X, this._contentHeight);
    return false;
  }

  public override void _Process(double delta)
  {
    if (!((CanvasItem) this).IsVisibleInTree())
      return;
    this.UpdateScrollPosition(delta);
    this.UpdateScrollbar();
  }

  private void ProcessGuiFocus(Control focusedControl)
  {
    if (!((CanvasItem) this).IsVisibleInTree() || !((CanvasItem) this._scrollbar).Visible || !NControllerManager.Instance.IsUsingController || !((Node) this._dropdownItems).IsAncestorOf((Node) focusedControl))
      return;
    this._targetDragPos = new Vector2(this._targetDragPos.X, Mathf.Clamp(((Control) this._dropdownItems).GlobalPosition.Y - focusedControl.GlobalPosition.Y + this.Size.Y * 0.5f, this._scrollLimitBottom, 0.0f));
  }

  private void UpdateScrollPosition(double delta)
  {
    if (!((CanvasItem) this._scrollbar).Visible)
      return;
    if (Vector2.op_Inequality(((Control) this._dropdownItems).Position, this._targetDragPos))
    {
      float num1 = (float) Mathf.Sign(((Control) this._dropdownItems).Position.Y - this._targetDragPos.Y);
      VBoxContainer dropdownItems = this._dropdownItems;
      Vector2 position = ((Control) this._dropdownItems).Position;
      Vector2 vector2 = ((Vector2) ref position).Lerp(this._targetDragPos, (float) delta * 15f);
      ((Control) dropdownItems).Position = vector2;
      float num2 = (float) Mathf.Sign(((Control) this._dropdownItems).Position.Y - this._targetDragPos.Y);
      if ((double) Math.Abs(((Control) this._dropdownItems).Position.Y - this._targetDragPos.Y) < 0.5 || !Mathf.IsEqualApprox(num1, num2))
        ((Control) this._dropdownItems).Position = this._targetDragPos;
    }
    if (this._isDragging)
      return;
    if ((double) this._targetDragPos.Y < (double) this._scrollLimitBottom)
    {
      this._targetDragPos = ((Vector2) ref this._targetDragPos).Lerp(new Vector2(0.0f, this._scrollLimitBottom), (float) delta * 12f);
    }
    else
    {
      if ((double) this._targetDragPos.Y <= 0.0)
        return;
      this._targetDragPos = ((Vector2) ref this._targetDragPos).Lerp(new Vector2(0.0f, 0.0f), (float) delta * 12f);
    }
  }

  private void UpdateScrollbar()
  {
    if (!((CanvasItem) this._scrollbar).Visible || this._scrollbar.hasControl)
      return;
    this._scrollbar.SetTrainPositionFromPercentage(Mathf.Clamp(1f - (float) (((double) ((Control) this._dropdownItems).Position.Y - (double) this._scrollLimitBottom) / (0.0 - (double) this._scrollLimitBottom)), 0.0f, 1f));
  }

  public void UpdatePositionBasedOnTrain(float trainPosition)
  {
    this._targetDragPos = new Vector2(this._targetDragPos.X, this._scrollLimitBottom + trainPosition * (0.0f - this._scrollLimitBottom));
  }

  public override void _GuiInput(InputEvent inputEvent)
  {
    this.ProcessMouseEvent(inputEvent);
    this.ProcessScrollEvent(inputEvent);
  }

  private void ProcessMouseEvent(InputEvent inputEvent)
  {
    if (this._isDragging && inputEvent is InputEventMouseMotion eventMouseMotion)
    {
      this._targetDragPos = Vector2.op_Addition(this._targetDragPos, new Vector2(0.0f, eventMouseMotion.Relative.Y));
    }
    else
    {
      if (!(inputEvent is InputEventMouseButton eventMouseButton))
        return;
      if (eventMouseButton.ButtonIndex == 1L)
      {
        if (eventMouseButton.Pressed)
        {
          this._isDragging = true;
          this._startDragPos = ((Control) this._dropdownItems).Position;
          this._targetDragPos = this._startDragPos;
        }
        else
          this._isDragging = false;
      }
      else
      {
        if (eventMouseButton.Pressed)
          return;
        this._isDragging = false;
      }
    }
  }

  private void ProcessScrollEvent(InputEvent inputEvent)
  {
    this._targetDragPos = Vector2.op_Addition(this._targetDragPos, new Vector2(0.0f, ScrollHelper.GetDragForScrollEvent(inputEvent)));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(14)
    {
      new MethodInfo(NDropdownContainer.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NDropdownContainer.MethodName._EnterTree, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NDropdownContainer.MethodName._ExitTree, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NDropdownContainer.MethodName.OnVisibilityChange, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NDropdownContainer.MethodName.RefreshLayout, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NDropdownContainer.MethodName.IsScrollbarNeeded, new PropertyInfo((Variant.Type) 1L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NDropdownContainer.MethodName._Process, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 3L, StringName.op_Implicit("delta"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NDropdownContainer.MethodName.ProcessGuiFocus, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("focusedControl"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false)
      }, (List<Variant>) null),
      new MethodInfo(NDropdownContainer.MethodName.UpdateScrollPosition, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 3L, StringName.op_Implicit("delta"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NDropdownContainer.MethodName.UpdateScrollbar, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NDropdownContainer.MethodName.UpdatePositionBasedOnTrain, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 3L, StringName.op_Implicit("trainPosition"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NDropdownContainer.MethodName._GuiInput, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("inputEvent"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("InputEvent"), false)
      }, (List<Variant>) null),
      new MethodInfo(NDropdownContainer.MethodName.ProcessMouseEvent, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("inputEvent"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("InputEvent"), false)
      }, (List<Variant>) null),
      new MethodInfo(NDropdownContainer.MethodName.ProcessScrollEvent, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("inputEvent"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("InputEvent"), false)
      }, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NDropdownContainer.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NDropdownContainer.MethodName._EnterTree) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._EnterTree();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NDropdownContainer.MethodName._ExitTree) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._ExitTree();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NDropdownContainer.MethodName.OnVisibilityChange) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnVisibilityChange();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NDropdownContainer.MethodName.RefreshLayout) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.RefreshLayout();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NDropdownContainer.MethodName.IsScrollbarNeeded) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      bool flag = this.IsScrollbarNeeded();
      ret = VariantUtils.CreateFrom<bool>(ref flag);
      return true;
    }
    if (StringName.op_Equality(ref method, NDropdownContainer.MethodName._Process) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      ((Node) this)._Process(VariantUtils.ConvertTo<double>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NDropdownContainer.MethodName.ProcessGuiFocus) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.ProcessGuiFocus(VariantUtils.ConvertTo<Control>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NDropdownContainer.MethodName.UpdateScrollPosition) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.UpdateScrollPosition(VariantUtils.ConvertTo<double>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NDropdownContainer.MethodName.UpdateScrollbar) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.UpdateScrollbar();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NDropdownContainer.MethodName.UpdatePositionBasedOnTrain) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.UpdatePositionBasedOnTrain(VariantUtils.ConvertTo<float>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NDropdownContainer.MethodName._GuiInput) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      base._GuiInput(VariantUtils.ConvertTo<InputEvent>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NDropdownContainer.MethodName.ProcessMouseEvent) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.ProcessMouseEvent(VariantUtils.ConvertTo<InputEvent>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NDropdownContainer.MethodName.ProcessScrollEvent) || ((NativeVariantPtrArgs) ref args).Count != 1)
      return base.InvokeGodotClassMethod(ref method, args, ref ret);
    this.ProcessScrollEvent(VariantUtils.ConvertTo<InputEvent>(ref ((NativeVariantPtrArgs) ref args)[0]));
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NDropdownContainer.MethodName._Ready) || StringName.op_Equality(ref method, NDropdownContainer.MethodName._EnterTree) || StringName.op_Equality(ref method, NDropdownContainer.MethodName._ExitTree) || StringName.op_Equality(ref method, NDropdownContainer.MethodName.OnVisibilityChange) || StringName.op_Equality(ref method, NDropdownContainer.MethodName.RefreshLayout) || StringName.op_Equality(ref method, NDropdownContainer.MethodName.IsScrollbarNeeded) || StringName.op_Equality(ref method, NDropdownContainer.MethodName._Process) || StringName.op_Equality(ref method, NDropdownContainer.MethodName.ProcessGuiFocus) || StringName.op_Equality(ref method, NDropdownContainer.MethodName.UpdateScrollPosition) || StringName.op_Equality(ref method, NDropdownContainer.MethodName.UpdateScrollbar) || StringName.op_Equality(ref method, NDropdownContainer.MethodName.UpdatePositionBasedOnTrain) || StringName.op_Equality(ref method, NDropdownContainer.MethodName._GuiInput) || StringName.op_Equality(ref method, NDropdownContainer.MethodName.ProcessMouseEvent) || StringName.op_Equality(ref method, NDropdownContainer.MethodName.ProcessScrollEvent) || base.HasGodotClassMethod(ref method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NDropdownContainer.PropertyName._scrollbar))
    {
      this._scrollbar = VariantUtils.ConvertTo<NDropdownScrollbar>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NDropdownContainer.PropertyName._scrollbarTrain))
    {
      this._scrollbarTrain = VariantUtils.ConvertTo<Control>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NDropdownContainer.PropertyName._dropdownItems))
    {
      this._dropdownItems = VariantUtils.ConvertTo<VBoxContainer>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NDropdownContainer.PropertyName._maxHeight))
    {
      this._maxHeight = VariantUtils.ConvertTo<float>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NDropdownContainer.PropertyName._contentHeight))
    {
      this._contentHeight = VariantUtils.ConvertTo<float>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NDropdownContainer.PropertyName._startDragPos))
    {
      this._startDragPos = VariantUtils.ConvertTo<Vector2>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NDropdownContainer.PropertyName._targetDragPos))
    {
      this._targetDragPos = VariantUtils.ConvertTo<Vector2>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NDropdownContainer.PropertyName._scrollLimitBottom))
    {
      this._scrollLimitBottom = VariantUtils.ConvertTo<float>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NDropdownContainer.PropertyName._isDragging))
      return ((GodotObject) this).SetGodotClassPropertyValue(ref name, ref value);
    this._isDragging = VariantUtils.ConvertTo<bool>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NDropdownContainer.PropertyName._scrollbar))
    {
      value = VariantUtils.CreateFrom<NDropdownScrollbar>(ref this._scrollbar);
      return true;
    }
    if (StringName.op_Equality(ref name, NDropdownContainer.PropertyName._scrollbarTrain))
    {
      value = VariantUtils.CreateFrom<Control>(ref this._scrollbarTrain);
      return true;
    }
    if (StringName.op_Equality(ref name, NDropdownContainer.PropertyName._dropdownItems))
    {
      value = VariantUtils.CreateFrom<VBoxContainer>(ref this._dropdownItems);
      return true;
    }
    if (StringName.op_Equality(ref name, NDropdownContainer.PropertyName._maxHeight))
    {
      value = VariantUtils.CreateFrom<float>(ref this._maxHeight);
      return true;
    }
    if (StringName.op_Equality(ref name, NDropdownContainer.PropertyName._contentHeight))
    {
      value = VariantUtils.CreateFrom<float>(ref this._contentHeight);
      return true;
    }
    if (StringName.op_Equality(ref name, NDropdownContainer.PropertyName._startDragPos))
    {
      value = VariantUtils.CreateFrom<Vector2>(ref this._startDragPos);
      return true;
    }
    if (StringName.op_Equality(ref name, NDropdownContainer.PropertyName._targetDragPos))
    {
      value = VariantUtils.CreateFrom<Vector2>(ref this._targetDragPos);
      return true;
    }
    if (StringName.op_Equality(ref name, NDropdownContainer.PropertyName._scrollLimitBottom))
    {
      value = VariantUtils.CreateFrom<float>(ref this._scrollLimitBottom);
      return true;
    }
    if (!StringName.op_Equality(ref name, NDropdownContainer.PropertyName._isDragging))
      return ((GodotObject) this).GetGodotClassPropertyValue(ref name, ref value);
    value = VariantUtils.CreateFrom<bool>(ref this._isDragging);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NDropdownContainer.PropertyName._scrollbar, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NDropdownContainer.PropertyName._scrollbarTrain, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NDropdownContainer.PropertyName._dropdownItems, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 3L, NDropdownContainer.PropertyName._maxHeight, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 3L, NDropdownContainer.PropertyName._contentHeight, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 5L, NDropdownContainer.PropertyName._startDragPos, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 5L, NDropdownContainer.PropertyName._targetDragPos, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 3L, NDropdownContainer.PropertyName._scrollLimitBottom, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 1L, NDropdownContainer.PropertyName._isDragging, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).SaveGodotObjectData(info);
    info.AddProperty(NDropdownContainer.PropertyName._scrollbar, Variant.From<NDropdownScrollbar>(ref this._scrollbar));
    info.AddProperty(NDropdownContainer.PropertyName._scrollbarTrain, Variant.From<Control>(ref this._scrollbarTrain));
    info.AddProperty(NDropdownContainer.PropertyName._dropdownItems, Variant.From<VBoxContainer>(ref this._dropdownItems));
    info.AddProperty(NDropdownContainer.PropertyName._maxHeight, Variant.From<float>(ref this._maxHeight));
    info.AddProperty(NDropdownContainer.PropertyName._contentHeight, Variant.From<float>(ref this._contentHeight));
    info.AddProperty(NDropdownContainer.PropertyName._startDragPos, Variant.From<Vector2>(ref this._startDragPos));
    info.AddProperty(NDropdownContainer.PropertyName._targetDragPos, Variant.From<Vector2>(ref this._targetDragPos));
    info.AddProperty(NDropdownContainer.PropertyName._scrollLimitBottom, Variant.From<float>(ref this._scrollLimitBottom));
    info.AddProperty(NDropdownContainer.PropertyName._isDragging, Variant.From<bool>(ref this._isDragging));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NDropdownContainer.PropertyName._scrollbar, ref variant1))
      this._scrollbar = ((Variant) ref variant1).As<NDropdownScrollbar>();
    Variant variant2;
    if (info.TryGetProperty(NDropdownContainer.PropertyName._scrollbarTrain, ref variant2))
      this._scrollbarTrain = ((Variant) ref variant2).As<Control>();
    Variant variant3;
    if (info.TryGetProperty(NDropdownContainer.PropertyName._dropdownItems, ref variant3))
      this._dropdownItems = ((Variant) ref variant3).As<VBoxContainer>();
    Variant variant4;
    if (info.TryGetProperty(NDropdownContainer.PropertyName._maxHeight, ref variant4))
      this._maxHeight = ((Variant) ref variant4).As<float>();
    Variant variant5;
    if (info.TryGetProperty(NDropdownContainer.PropertyName._contentHeight, ref variant5))
      this._contentHeight = ((Variant) ref variant5).As<float>();
    Variant variant6;
    if (info.TryGetProperty(NDropdownContainer.PropertyName._startDragPos, ref variant6))
      this._startDragPos = ((Variant) ref variant6).As<Vector2>();
    Variant variant7;
    if (info.TryGetProperty(NDropdownContainer.PropertyName._targetDragPos, ref variant7))
      this._targetDragPos = ((Variant) ref variant7).As<Vector2>();
    Variant variant8;
    if (info.TryGetProperty(NDropdownContainer.PropertyName._scrollLimitBottom, ref variant8))
      this._scrollLimitBottom = ((Variant) ref variant8).As<float>();
    Variant variant9;
    if (!info.TryGetProperty(NDropdownContainer.PropertyName._isDragging, ref variant9))
      return;
    this._isDragging = ((Variant) ref variant9).As<bool>();
  }

  public class MethodName : Control.MethodName
  {
    public static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName _EnterTree = StringName.op_Implicit(nameof (_EnterTree));
    public static readonly StringName _ExitTree = StringName.op_Implicit(nameof (_ExitTree));
    public static readonly StringName OnVisibilityChange = StringName.op_Implicit(nameof (OnVisibilityChange));
    public static readonly StringName RefreshLayout = StringName.op_Implicit(nameof (RefreshLayout));
    public static readonly StringName IsScrollbarNeeded = StringName.op_Implicit(nameof (IsScrollbarNeeded));
    public static readonly StringName _Process = StringName.op_Implicit(nameof (_Process));
    public static readonly StringName ProcessGuiFocus = StringName.op_Implicit(nameof (ProcessGuiFocus));
    public static readonly StringName UpdateScrollPosition = StringName.op_Implicit(nameof (UpdateScrollPosition));
    public static readonly StringName UpdateScrollbar = StringName.op_Implicit(nameof (UpdateScrollbar));
    public static readonly StringName UpdatePositionBasedOnTrain = StringName.op_Implicit(nameof (UpdatePositionBasedOnTrain));
    public static readonly StringName _GuiInput = StringName.op_Implicit(nameof (_GuiInput));
    public static readonly StringName ProcessMouseEvent = StringName.op_Implicit(nameof (ProcessMouseEvent));
    public static readonly StringName ProcessScrollEvent = StringName.op_Implicit(nameof (ProcessScrollEvent));
  }

  public class PropertyName : Control.PropertyName
  {
    public static readonly StringName _scrollbar = StringName.op_Implicit(nameof (_scrollbar));
    public static readonly StringName _scrollbarTrain = StringName.op_Implicit(nameof (_scrollbarTrain));
    public static readonly StringName _dropdownItems = StringName.op_Implicit(nameof (_dropdownItems));
    public static readonly StringName _maxHeight = StringName.op_Implicit(nameof (_maxHeight));
    public static readonly StringName _contentHeight = StringName.op_Implicit(nameof (_contentHeight));
    public static readonly StringName _startDragPos = StringName.op_Implicit(nameof (_startDragPos));
    public static readonly StringName _targetDragPos = StringName.op_Implicit(nameof (_targetDragPos));
    public static readonly StringName _scrollLimitBottom = StringName.op_Implicit(nameof (_scrollLimitBottom));
    public static readonly StringName _isDragging = StringName.op_Implicit(nameof (_isDragging));
  }

  public class SignalName : Control.SignalName
  {
  }
}
