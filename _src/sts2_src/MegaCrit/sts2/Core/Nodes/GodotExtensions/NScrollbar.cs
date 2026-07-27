// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.GodotExtensions.NScrollbar
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
namespace MegaCrit.Sts2.Core.Nodes.GodotExtensions;

[GlobalClass]
[ScriptPath("res://src/Core/Nodes/GodotExtensions/NScrollbar.cs")]
public class NScrollbar : Range
{
  private Control _handle;
  private float _currentHandlePosition;
  private float _currentVelocity;
  private bool _isDragging;
  private 
  #nullable disable
  NScrollbar.MouseReleasedEventHandler backing_MouseReleased;
  private NScrollbar.MousePressedEventHandler backing_MousePressed;

  public override void _Ready()
  {
    this._handle = ((Node) this).GetNode<Control>(NodePath.op_Implicit("%Handle"));
  }

  public override void _GuiInput(
  #nullable enable
  InputEvent inputEvent)
  {
    ((Control) this)._GuiInput(inputEvent);
    switch (inputEvent)
    {
      case InputEventMouseButton eventMouseButton:
        if (eventMouseButton.ButtonIndex - 1L > 1L)
          break;
        this._isDragging = ((InputEvent) eventMouseButton).IsPressed();
        this.SetValueBasedOnMousePosition(((InputEventMouse) eventMouseButton).Position);
        ((GodotObject) this).EmitSignal(((InputEvent) eventMouseButton).IsPressed() ? NScrollbar.SignalName.MousePressed : NScrollbar.SignalName.MouseReleased, new Variant[1]
        {
          Variant.op_Implicit((GodotObject) inputEvent)
        });
        break;
      case InputEventMouseMotion eventMouseMotion:
        if (!this._isDragging)
          break;
        this.SetValueBasedOnMousePosition(((InputEventMouse) eventMouseMotion).Position);
        break;
    }
  }

  private void SetValueBasedOnMousePosition(Vector2 mousePosition)
  {
    this.Value = (double) mousePosition.Y / (double) ((Control) this).Size.Y * this.MaxValue;
  }

  public void SetValueWithoutAnimation(double value)
  {
    this._currentHandlePosition = (float) value;
    this.Value = value;
    this.UpdateHandlePosition();
  }

  public override void _Process(double delta)
  {
    this._currentHandlePosition = (float) this.Value;
    this.UpdateHandlePosition();
  }

  private void UpdateHandlePosition()
  {
    this._handle.Position = new Vector2((float) (((double) ((Control) this).Size.X - (double) this._handle.Size.X) * 0.5), MathHelper.SmoothDamp(this._handle.Position.Y, (float) ((double) ((Control) this).Size.Y * (double) (this._currentHandlePosition / (float) this.MaxValue) - (double) this._handle.Size.Y * 0.5), ref this._currentVelocity, 0.05f, (float) ((Node) this).GetProcessDeltaTime()));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(6)
    {
      new MethodInfo(NScrollbar.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NScrollbar.MethodName._GuiInput, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("inputEvent"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("InputEvent"), false)
      }, (List<Variant>) null),
      new MethodInfo(NScrollbar.MethodName.SetValueBasedOnMousePosition, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 5L, StringName.op_Implicit("mousePosition"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NScrollbar.MethodName.SetValueWithoutAnimation, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 3L, StringName.op_Implicit("value"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NScrollbar.MethodName._Process, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 3L, StringName.op_Implicit("delta"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NScrollbar.MethodName.UpdateHandlePosition, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NScrollbar.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NScrollbar.MethodName._GuiInput) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      ((Control) this)._GuiInput(VariantUtils.ConvertTo<InputEvent>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NScrollbar.MethodName.SetValueBasedOnMousePosition) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.SetValueBasedOnMousePosition(VariantUtils.ConvertTo<Vector2>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NScrollbar.MethodName.SetValueWithoutAnimation) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.SetValueWithoutAnimation(VariantUtils.ConvertTo<double>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NScrollbar.MethodName._Process) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      ((Node) this)._Process(VariantUtils.ConvertTo<double>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NScrollbar.MethodName.UpdateHandlePosition) || ((NativeVariantPtrArgs) ref args).Count != 0)
      return base.InvokeGodotClassMethod(ref method, args, ref ret);
    this.UpdateHandlePosition();
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NScrollbar.MethodName._Ready) || StringName.op_Equality(ref method, NScrollbar.MethodName._GuiInput) || StringName.op_Equality(ref method, NScrollbar.MethodName.SetValueBasedOnMousePosition) || StringName.op_Equality(ref method, NScrollbar.MethodName.SetValueWithoutAnimation) || StringName.op_Equality(ref method, NScrollbar.MethodName._Process) || StringName.op_Equality(ref method, NScrollbar.MethodName.UpdateHandlePosition) || base.HasGodotClassMethod(ref method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NScrollbar.PropertyName._handle))
    {
      this._handle = VariantUtils.ConvertTo<Control>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NScrollbar.PropertyName._currentHandlePosition))
    {
      this._currentHandlePosition = VariantUtils.ConvertTo<float>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NScrollbar.PropertyName._currentVelocity))
    {
      this._currentVelocity = VariantUtils.ConvertTo<float>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NScrollbar.PropertyName._isDragging))
      return ((GodotObject) this).SetGodotClassPropertyValue(ref name, ref value);
    this._isDragging = VariantUtils.ConvertTo<bool>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NScrollbar.PropertyName._handle))
    {
      value = VariantUtils.CreateFrom<Control>(ref this._handle);
      return true;
    }
    if (StringName.op_Equality(ref name, NScrollbar.PropertyName._currentHandlePosition))
    {
      value = VariantUtils.CreateFrom<float>(ref this._currentHandlePosition);
      return true;
    }
    if (StringName.op_Equality(ref name, NScrollbar.PropertyName._currentVelocity))
    {
      value = VariantUtils.CreateFrom<float>(ref this._currentVelocity);
      return true;
    }
    if (!StringName.op_Equality(ref name, NScrollbar.PropertyName._isDragging))
      return ((GodotObject) this).GetGodotClassPropertyValue(ref name, ref value);
    value = VariantUtils.CreateFrom<bool>(ref this._isDragging);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NScrollbar.PropertyName._handle, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 3L, NScrollbar.PropertyName._currentHandlePosition, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 3L, NScrollbar.PropertyName._currentVelocity, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 1L, NScrollbar.PropertyName._isDragging, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).SaveGodotObjectData(info);
    info.AddProperty(NScrollbar.PropertyName._handle, Variant.From<Control>(ref this._handle));
    info.AddProperty(NScrollbar.PropertyName._currentHandlePosition, Variant.From<float>(ref this._currentHandlePosition));
    info.AddProperty(NScrollbar.PropertyName._currentVelocity, Variant.From<float>(ref this._currentVelocity));
    info.AddProperty(NScrollbar.PropertyName._isDragging, Variant.From<bool>(ref this._isDragging));
    info.AddSignalEventDelegate(NScrollbar.SignalName.MouseReleased, (Delegate) this.backing_MouseReleased);
    info.AddSignalEventDelegate(NScrollbar.SignalName.MousePressed, (Delegate) this.backing_MousePressed);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NScrollbar.PropertyName._handle, ref variant1))
      this._handle = ((Variant) ref variant1).As<Control>();
    Variant variant2;
    if (info.TryGetProperty(NScrollbar.PropertyName._currentHandlePosition, ref variant2))
      this._currentHandlePosition = ((Variant) ref variant2).As<float>();
    Variant variant3;
    if (info.TryGetProperty(NScrollbar.PropertyName._currentVelocity, ref variant3))
      this._currentVelocity = ((Variant) ref variant3).As<float>();
    Variant variant4;
    if (info.TryGetProperty(NScrollbar.PropertyName._isDragging, ref variant4))
      this._isDragging = ((Variant) ref variant4).As<bool>();
    NScrollbar.MouseReleasedEventHandler releasedEventHandler;
    if (info.TryGetSignalEventDelegate<NScrollbar.MouseReleasedEventHandler>(NScrollbar.SignalName.MouseReleased, ref releasedEventHandler))
      this.backing_MouseReleased = releasedEventHandler;
    NScrollbar.MousePressedEventHandler pressedEventHandler;
    if (!info.TryGetSignalEventDelegate<NScrollbar.MousePressedEventHandler>(NScrollbar.SignalName.MousePressed, ref pressedEventHandler))
      return;
    this.backing_MousePressed = pressedEventHandler;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<MethodInfo> GetGodotSignalList()
  {
    return new List<MethodInfo>(2)
    {
      new MethodInfo(NScrollbar.SignalName.MouseReleased, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("inputEvent"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("InputEvent"), false)
      }, (List<Variant>) null),
      new MethodInfo(NScrollbar.SignalName.MousePressed, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("inputEvent"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("InputEvent"), false)
      }, (List<Variant>) null)
    };
  }

  public event NScrollbar.MouseReleasedEventHandler MouseReleased
  {
    add => this.backing_MouseReleased += value;
    remove => this.backing_MouseReleased -= value;
  }

  protected void EmitSignalMouseReleased(InputEvent inputEvent)
  {
    ((GodotObject) this).EmitSignal(NScrollbar.SignalName.MouseReleased, new Variant[1]
    {
      Variant.op_Implicit((GodotObject) inputEvent)
    });
  }

  public event NScrollbar.MousePressedEventHandler MousePressed
  {
    add => this.backing_MousePressed += value;
    remove => this.backing_MousePressed -= value;
  }

  protected void EmitSignalMousePressed(InputEvent inputEvent)
  {
    ((GodotObject) this).EmitSignal(NScrollbar.SignalName.MousePressed, new Variant[1]
    {
      Variant.op_Implicit((GodotObject) inputEvent)
    });
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RaiseGodotClassSignalCallbacks(
    in godot_string_name signal,
    NativeVariantPtrArgs args)
  {
    if (StringName.op_Equality(ref signal, NScrollbar.SignalName.MouseReleased) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      NScrollbar.MouseReleasedEventHandler backingMouseReleased = this.backing_MouseReleased;
      if (backingMouseReleased == null)
        return;
      backingMouseReleased(VariantUtils.ConvertTo<InputEvent>(ref ((NativeVariantPtrArgs) ref args)[0]));
    }
    else if (StringName.op_Equality(ref signal, NScrollbar.SignalName.MousePressed) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      NScrollbar.MousePressedEventHandler backingMousePressed = this.backing_MousePressed;
      if (backingMousePressed == null)
        return;
      backingMousePressed(VariantUtils.ConvertTo<InputEvent>(ref ((NativeVariantPtrArgs) ref args)[0]));
    }
    else
      ((GodotObject) this).RaiseGodotClassSignalCallbacks(ref signal, args);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassSignal(in godot_string_name signal)
  {
    return StringName.op_Equality(ref signal, NScrollbar.SignalName.MouseReleased) || StringName.op_Equality(ref signal, NScrollbar.SignalName.MousePressed) || base.HasGodotClassSignal(ref signal);
  }

  [Signal]
  public delegate void MouseReleasedEventHandler(
  #nullable enable
  InputEvent inputEvent);

  [Signal]
  public delegate void MousePressedEventHandler(InputEvent inputEvent);

  public class MethodName : Range.MethodName
  {
    public static readonly 
    #nullable disable
    StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName _GuiInput = StringName.op_Implicit(nameof (_GuiInput));
    public static readonly StringName SetValueBasedOnMousePosition = StringName.op_Implicit(nameof (SetValueBasedOnMousePosition));
    public static readonly StringName SetValueWithoutAnimation = StringName.op_Implicit(nameof (SetValueWithoutAnimation));
    public static readonly StringName _Process = StringName.op_Implicit(nameof (_Process));
    public static readonly StringName UpdateHandlePosition = StringName.op_Implicit(nameof (UpdateHandlePosition));
  }

  public class PropertyName : Range.PropertyName
  {
    public static readonly StringName _handle = StringName.op_Implicit(nameof (_handle));
    public static readonly StringName _currentHandlePosition = StringName.op_Implicit(nameof (_currentHandlePosition));
    public static readonly StringName _currentVelocity = StringName.op_Implicit(nameof (_currentVelocity));
    public static readonly StringName _isDragging = StringName.op_Implicit(nameof (_isDragging));
  }

  public class SignalName : Range.SignalName
  {
    public static readonly StringName MouseReleased = StringName.op_Implicit(nameof (MouseReleased));
    public static readonly StringName MousePressed = StringName.op_Implicit(nameof (MousePressed));
  }
}
