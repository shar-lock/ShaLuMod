// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.GodotExtensions.NClickableControl
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.ControllerInput;
using System;
using System.Collections.Generic;
using System.ComponentModel;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.GodotExtensions;

[ScriptPath("res://src/Core/Nodes/GodotExtensions/NClickableControl.cs")]
public class NClickableControl : Control
{
  [Export]
  protected float _ignoreDragThreshold = -1f;
  protected bool _isEnabled = true;
  private bool _isHovered;
  private bool _isControllerFocused;
  private bool _isControllerNavigable;
  private Vector2 _beginDragPosition;
  private bool _isPressed;
  private static readonly StyleBoxEmpty _blankFocusStyle = new StyleBoxEmpty();
  private 
  #nullable disable
  NClickableControl.ReleasedEventHandler backing_Released;
  private NClickableControl.FocusedEventHandler backing_Focused;
  private NClickableControl.UnfocusedEventHandler backing_Unfocused;
  private NClickableControl.MouseReleasedEventHandler backing_MouseReleased;
  private NClickableControl.MousePressedEventHandler backing_MousePressed;

  protected virtual bool AllowFocusWhileDisabled => false;

  protected bool IsFocused { get; private set; }

  public bool IsEnabled => this._isEnabled;

  protected virtual void ConnectSignals()
  {
    ((GodotObject) this).Connect(Control.SignalName.FocusEntered, Callable.From(new Action(this.OnFocusHandler)), 0U);
    ((GodotObject) this).Connect(Control.SignalName.FocusExited, Callable.From(new Action(this.OnUnFocusHandler)), 0U);
    ((GodotObject) this).Connect(Control.SignalName.MouseEntered, Callable.From(new Action(this.OnHoverHandler)), 0U);
    ((GodotObject) this).Connect(Control.SignalName.MouseExited, Callable.From(new Action(this.OnUnhoverHandler)), 0U);
    ((GodotObject) this).Connect(NClickableControl.SignalName.MousePressed, Callable.From<InputEvent>(new Action<InputEvent>(this.HandleMousePress)), 0U);
    ((GodotObject) this).Connect(NClickableControl.SignalName.MouseReleased, Callable.From<InputEvent>(new Action<InputEvent>(this.HandleMouseRelease)), 0U);
    ((GodotObject) this).Connect(CanvasItem.SignalName.VisibilityChanged, Callable.From(new Action(this.OnVisibilityChanged)), 0U);
    this.AddThemeStyleboxOverride(ThemeConstants.Control.Focus, (StyleBox) NClickableControl._blankFocusStyle);
    this._isControllerNavigable = this.FocusMode == 2L;
    if (!this.HasFocus())
      return;
    this.OnFocusHandler();
  }

  private void OnVisibilityChanged()
  {
    if (((CanvasItem) this).IsVisibleInTree())
      return;
    this.OnUnFocusHandler();
  }

  private void OnFocusHandler()
  {
    this._isControllerFocused = true;
    this.RefreshFocus();
  }

  private void OnUnFocusHandler()
  {
    this._isControllerFocused = false;
    this.RefreshFocus();
  }

  private void HandleMousePress(
  #nullable enable
  InputEvent inputEvent)
  {
    if (!this._isEnabled || !((CanvasItem) this).IsVisibleInTree() || !this.IsFocused || !(inputEvent is InputEventMouseButton eventMouseButton) || eventMouseButton.ButtonIndex != 1L)
      return;
    this._isControllerFocused = false;
    this._beginDragPosition = ((InputEventMouse) eventMouseButton).GlobalPosition;
    this.OnPressHandler();
  }

  private void HandleMouseRelease(InputEvent inputEvent)
  {
    if (!this._isEnabled || !((CanvasItem) this).IsVisibleInTree() || !this.IsFocused || !(inputEvent is InputEventMouseButton eventMouseButton) || eventMouseButton.ButtonIndex != 1L)
      return;
    this.OnReleaseHandler();
  }

  private void OnHoverHandler()
  {
    this._isHovered = true;
    if (((Node) this).GetTree().Paused && !NGame.IsReleaseGame())
      return;
    this.RefreshFocus();
  }

  private void OnUnhoverHandler()
  {
    this._isHovered = false;
    if (((Node) this).GetTree().Paused && !NGame.IsReleaseGame())
      return;
    this.RefreshFocus();
  }

  protected void OnPressHandler()
  {
    this._isPressed = true;
    this.OnPress();
  }

  protected void OnReleaseHandler()
  {
    if (!this._isPressed)
      return;
    this._isPressed = false;
    this.OnRelease();
    ((GodotObject) this).EmitSignal(NClickableControl.SignalName.Released, new Variant[1]
    {
      Variant.op_Implicit((GodotObject) this)
    });
  }

  private void RefreshFocus()
  {
    bool flag = (this._isEnabled || this.AllowFocusWhileDisabled) && ((CanvasItem) this).IsVisibleInTree() && (this._isHovered || this._isControllerFocused);
    if (this.IsFocused == flag)
      return;
    this.IsFocused = flag;
    if (this.IsFocused)
    {
      ((GodotObject) this).EmitSignal(NClickableControl.SignalName.Focused, new Variant[1]
      {
        Variant.op_Implicit((GodotObject) this)
      });
      this.OnFocus();
    }
    else
    {
      ((GodotObject) this).EmitSignal(NClickableControl.SignalName.Unfocused, new Variant[1]
      {
        Variant.op_Implicit((GodotObject) this)
      });
      this.OnUnfocus();
    }
  }

  protected virtual void OnFocus()
  {
  }

  protected virtual void OnUnfocus()
  {
  }

  protected virtual void OnPress()
  {
  }

  protected virtual void OnRelease()
  {
  }

  public override void _GuiInput(InputEvent inputEvent)
  {
    if (inputEvent is InputEventMouseButton eventMouseButton && this._isEnabled && eventMouseButton.ButtonIndex - 1L <= 1L)
      ((GodotObject) this).EmitSignal(((InputEvent) eventMouseButton).IsPressed() ? NClickableControl.SignalName.MousePressed : NClickableControl.SignalName.MouseReleased, new Variant[1]
      {
        Variant.op_Implicit((GodotObject) inputEvent)
      });
    if (inputEvent.IsActionPressed(MegaInput.select, false, false))
    {
      this.OnPressHandler();
    }
    else
    {
      if (!inputEvent.IsActionReleased(MegaInput.select, false))
        return;
      this.OnReleaseHandler();
    }
  }

  protected void CheckMouseDragThreshold(InputEvent inputEvent)
  {
    if ((double) this._ignoreDragThreshold <= 0.0 || !this._isPressed || !(inputEvent is InputEventMouseMotion eventMouseMotion))
      return;
    Vector2 globalPosition = ((InputEventMouse) eventMouseMotion).GlobalPosition;
    if ((double) ((Vector2) ref globalPosition).DistanceTo(this._beginDragPosition) < (double) this._ignoreDragThreshold)
      return;
    this._isPressed = false;
  }

  public void DebugPress()
  {
    ((GodotObject) this).EmitSignal(NClickableControl.SignalName.MousePressed, new Variant[1]
    {
      Variant.op_Implicit((GodotObject) new InputEventMouseButton()
      {
        ButtonIndex = (MouseButton) 1L,
        Pressed = true
      })
    });
  }

  public void DebugRelease()
  {
    ((GodotObject) this).EmitSignal(NClickableControl.SignalName.MouseReleased, new Variant[1]
    {
      Variant.op_Implicit((GodotObject) new InputEventMouseButton()
      {
        ButtonIndex = (MouseButton) 1L,
        Pressed = false
      })
    });
  }

  public void ForceClick()
  {
    this.OnRelease();
    ((GodotObject) this).EmitSignal(NClickableControl.SignalName.Released, new Variant[1]
    {
      Variant.op_Implicit((GodotObject) this)
    });
  }

  public void SetEnabled(bool enabled)
  {
    if (enabled)
      this.Enable();
    else
      this.Disable();
  }

  public void Enable()
  {
    if (this._isEnabled)
      return;
    this._isEnabled = true;
    this.FocusMode = this._isControllerNavigable ? (Control.FocusModeEnum) 2L : (Control.FocusModeEnum) 0L;
    this.OnEnable();
    this.RefreshFocus();
    Callable callable = Callable.From((Action) (() => ((Node) this).SetProcessInput(true)));
    ((Callable) ref callable).CallDeferred(Array.Empty<Variant>());
  }

  public void Disable()
  {
    if (!this._isEnabled)
      return;
    this._isEnabled = false;
    this._isPressed = false;
    this.FocusMode = (Control.FocusModeEnum) 0L;
    this.OnDisable();
    this.RefreshFocus();
    ((Node) this).SetProcessInput(false);
  }

  protected virtual void OnEnable()
  {
  }

  protected virtual void OnDisable()
  {
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(25)
    {
      new MethodInfo(NClickableControl.MethodName.ConnectSignals, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NClickableControl.MethodName.OnVisibilityChanged, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NClickableControl.MethodName.OnFocusHandler, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NClickableControl.MethodName.OnUnFocusHandler, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NClickableControl.MethodName.HandleMousePress, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("inputEvent"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("InputEvent"), false)
      }, (List<Variant>) null),
      new MethodInfo(NClickableControl.MethodName.HandleMouseRelease, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("inputEvent"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("InputEvent"), false)
      }, (List<Variant>) null),
      new MethodInfo(NClickableControl.MethodName.OnHoverHandler, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NClickableControl.MethodName.OnUnhoverHandler, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NClickableControl.MethodName.OnPressHandler, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NClickableControl.MethodName.OnReleaseHandler, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NClickableControl.MethodName.RefreshFocus, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NClickableControl.MethodName.OnFocus, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NClickableControl.MethodName.OnUnfocus, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NClickableControl.MethodName.OnPress, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NClickableControl.MethodName.OnRelease, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NClickableControl.MethodName._GuiInput, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("inputEvent"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("InputEvent"), false)
      }, (List<Variant>) null),
      new MethodInfo(NClickableControl.MethodName.CheckMouseDragThreshold, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("inputEvent"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("InputEvent"), false)
      }, (List<Variant>) null),
      new MethodInfo(NClickableControl.MethodName.DebugPress, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NClickableControl.MethodName.DebugRelease, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NClickableControl.MethodName.ForceClick, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NClickableControl.MethodName.SetEnabled, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 1L, StringName.op_Implicit("enabled"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NClickableControl.MethodName.Enable, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NClickableControl.MethodName.Disable, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NClickableControl.MethodName.OnEnable, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NClickableControl.MethodName.OnDisable, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NClickableControl.MethodName.ConnectSignals) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.ConnectSignals();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NClickableControl.MethodName.OnVisibilityChanged) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnVisibilityChanged();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NClickableControl.MethodName.OnFocusHandler) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnFocusHandler();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NClickableControl.MethodName.OnUnFocusHandler) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnUnFocusHandler();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NClickableControl.MethodName.HandleMousePress) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.HandleMousePress(VariantUtils.ConvertTo<InputEvent>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NClickableControl.MethodName.HandleMouseRelease) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.HandleMouseRelease(VariantUtils.ConvertTo<InputEvent>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NClickableControl.MethodName.OnHoverHandler) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnHoverHandler();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NClickableControl.MethodName.OnUnhoverHandler) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnUnhoverHandler();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NClickableControl.MethodName.OnPressHandler) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnPressHandler();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NClickableControl.MethodName.OnReleaseHandler) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnReleaseHandler();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NClickableControl.MethodName.RefreshFocus) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.RefreshFocus();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NClickableControl.MethodName.OnFocus) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnFocus();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NClickableControl.MethodName.OnUnfocus) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnUnfocus();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NClickableControl.MethodName.OnPress) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnPress();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NClickableControl.MethodName.OnRelease) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnRelease();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NClickableControl.MethodName._GuiInput) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      base._GuiInput(VariantUtils.ConvertTo<InputEvent>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NClickableControl.MethodName.CheckMouseDragThreshold) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.CheckMouseDragThreshold(VariantUtils.ConvertTo<InputEvent>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NClickableControl.MethodName.DebugPress) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.DebugPress();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NClickableControl.MethodName.DebugRelease) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.DebugRelease();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NClickableControl.MethodName.ForceClick) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.ForceClick();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NClickableControl.MethodName.SetEnabled) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.SetEnabled(VariantUtils.ConvertTo<bool>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NClickableControl.MethodName.Enable) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.Enable();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NClickableControl.MethodName.Disable) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.Disable();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NClickableControl.MethodName.OnEnable) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnEnable();
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NClickableControl.MethodName.OnDisable) || ((NativeVariantPtrArgs) ref args).Count != 0)
      return base.InvokeGodotClassMethod(ref method, args, ref ret);
    this.OnDisable();
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NClickableControl.MethodName.ConnectSignals) || StringName.op_Equality(ref method, NClickableControl.MethodName.OnVisibilityChanged) || StringName.op_Equality(ref method, NClickableControl.MethodName.OnFocusHandler) || StringName.op_Equality(ref method, NClickableControl.MethodName.OnUnFocusHandler) || StringName.op_Equality(ref method, NClickableControl.MethodName.HandleMousePress) || StringName.op_Equality(ref method, NClickableControl.MethodName.HandleMouseRelease) || StringName.op_Equality(ref method, NClickableControl.MethodName.OnHoverHandler) || StringName.op_Equality(ref method, NClickableControl.MethodName.OnUnhoverHandler) || StringName.op_Equality(ref method, NClickableControl.MethodName.OnPressHandler) || StringName.op_Equality(ref method, NClickableControl.MethodName.OnReleaseHandler) || StringName.op_Equality(ref method, NClickableControl.MethodName.RefreshFocus) || StringName.op_Equality(ref method, NClickableControl.MethodName.OnFocus) || StringName.op_Equality(ref method, NClickableControl.MethodName.OnUnfocus) || StringName.op_Equality(ref method, NClickableControl.MethodName.OnPress) || StringName.op_Equality(ref method, NClickableControl.MethodName.OnRelease) || StringName.op_Equality(ref method, NClickableControl.MethodName._GuiInput) || StringName.op_Equality(ref method, NClickableControl.MethodName.CheckMouseDragThreshold) || StringName.op_Equality(ref method, NClickableControl.MethodName.DebugPress) || StringName.op_Equality(ref method, NClickableControl.MethodName.DebugRelease) || StringName.op_Equality(ref method, NClickableControl.MethodName.ForceClick) || StringName.op_Equality(ref method, NClickableControl.MethodName.SetEnabled) || StringName.op_Equality(ref method, NClickableControl.MethodName.Enable) || StringName.op_Equality(ref method, NClickableControl.MethodName.Disable) || StringName.op_Equality(ref method, NClickableControl.MethodName.OnEnable) || StringName.op_Equality(ref method, NClickableControl.MethodName.OnDisable) || base.HasGodotClassMethod(ref method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NClickableControl.PropertyName.IsFocused))
    {
      this.IsFocused = VariantUtils.ConvertTo<bool>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NClickableControl.PropertyName._ignoreDragThreshold))
    {
      this._ignoreDragThreshold = VariantUtils.ConvertTo<float>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NClickableControl.PropertyName._isEnabled))
    {
      this._isEnabled = VariantUtils.ConvertTo<bool>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NClickableControl.PropertyName._isHovered))
    {
      this._isHovered = VariantUtils.ConvertTo<bool>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NClickableControl.PropertyName._isControllerFocused))
    {
      this._isControllerFocused = VariantUtils.ConvertTo<bool>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NClickableControl.PropertyName._isControllerNavigable))
    {
      this._isControllerNavigable = VariantUtils.ConvertTo<bool>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NClickableControl.PropertyName._beginDragPosition))
    {
      this._beginDragPosition = VariantUtils.ConvertTo<Vector2>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NClickableControl.PropertyName._isPressed))
      return ((GodotObject) this).SetGodotClassPropertyValue(ref name, ref value);
    this._isPressed = VariantUtils.ConvertTo<bool>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NClickableControl.PropertyName.AllowFocusWhileDisabled))
    {
      ref godot_variant local = ref value;
      bool focusWhileDisabled = this.AllowFocusWhileDisabled;
      godot_variant from = VariantUtils.CreateFrom<bool>(ref focusWhileDisabled);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NClickableControl.PropertyName.IsFocused))
    {
      ref godot_variant local = ref value;
      bool isFocused = this.IsFocused;
      godot_variant from = VariantUtils.CreateFrom<bool>(ref isFocused);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NClickableControl.PropertyName.IsEnabled))
    {
      ref godot_variant local = ref value;
      bool isEnabled = this.IsEnabled;
      godot_variant from = VariantUtils.CreateFrom<bool>(ref isEnabled);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NClickableControl.PropertyName._ignoreDragThreshold))
    {
      value = VariantUtils.CreateFrom<float>(ref this._ignoreDragThreshold);
      return true;
    }
    if (StringName.op_Equality(ref name, NClickableControl.PropertyName._isEnabled))
    {
      value = VariantUtils.CreateFrom<bool>(ref this._isEnabled);
      return true;
    }
    if (StringName.op_Equality(ref name, NClickableControl.PropertyName._isHovered))
    {
      value = VariantUtils.CreateFrom<bool>(ref this._isHovered);
      return true;
    }
    if (StringName.op_Equality(ref name, NClickableControl.PropertyName._isControllerFocused))
    {
      value = VariantUtils.CreateFrom<bool>(ref this._isControllerFocused);
      return true;
    }
    if (StringName.op_Equality(ref name, NClickableControl.PropertyName._isControllerNavigable))
    {
      value = VariantUtils.CreateFrom<bool>(ref this._isControllerNavigable);
      return true;
    }
    if (StringName.op_Equality(ref name, NClickableControl.PropertyName._beginDragPosition))
    {
      value = VariantUtils.CreateFrom<Vector2>(ref this._beginDragPosition);
      return true;
    }
    if (!StringName.op_Equality(ref name, NClickableControl.PropertyName._isPressed))
      return ((GodotObject) this).GetGodotClassPropertyValue(ref name, ref value);
    value = VariantUtils.CreateFrom<bool>(ref this._isPressed);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 3L, NClickableControl.PropertyName._ignoreDragThreshold, (PropertyHint) 0L, "", (PropertyUsageFlags) 4102L, true),
      new PropertyInfo((Variant.Type) 1L, NClickableControl.PropertyName.AllowFocusWhileDisabled, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 1L, NClickableControl.PropertyName.IsFocused, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 1L, NClickableControl.PropertyName._isEnabled, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 1L, NClickableControl.PropertyName.IsEnabled, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 1L, NClickableControl.PropertyName._isHovered, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 1L, NClickableControl.PropertyName._isControllerFocused, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 1L, NClickableControl.PropertyName._isControllerNavigable, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 5L, NClickableControl.PropertyName._beginDragPosition, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 1L, NClickableControl.PropertyName._isPressed, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).SaveGodotObjectData(info);
    GodotSerializationInfo serializationInfo = info;
    StringName isFocused1 = NClickableControl.PropertyName.IsFocused;
    bool isFocused2 = this.IsFocused;
    Variant variant = Variant.From<bool>(ref isFocused2);
    serializationInfo.AddProperty(isFocused1, variant);
    info.AddProperty(NClickableControl.PropertyName._ignoreDragThreshold, Variant.From<float>(ref this._ignoreDragThreshold));
    info.AddProperty(NClickableControl.PropertyName._isEnabled, Variant.From<bool>(ref this._isEnabled));
    info.AddProperty(NClickableControl.PropertyName._isHovered, Variant.From<bool>(ref this._isHovered));
    info.AddProperty(NClickableControl.PropertyName._isControllerFocused, Variant.From<bool>(ref this._isControllerFocused));
    info.AddProperty(NClickableControl.PropertyName._isControllerNavigable, Variant.From<bool>(ref this._isControllerNavigable));
    info.AddProperty(NClickableControl.PropertyName._beginDragPosition, Variant.From<Vector2>(ref this._beginDragPosition));
    info.AddProperty(NClickableControl.PropertyName._isPressed, Variant.From<bool>(ref this._isPressed));
    info.AddSignalEventDelegate(NClickableControl.SignalName.Released, (Delegate) this.backing_Released);
    info.AddSignalEventDelegate(NClickableControl.SignalName.Focused, (Delegate) this.backing_Focused);
    info.AddSignalEventDelegate(NClickableControl.SignalName.Unfocused, (Delegate) this.backing_Unfocused);
    info.AddSignalEventDelegate(NClickableControl.SignalName.MouseReleased, (Delegate) this.backing_MouseReleased);
    info.AddSignalEventDelegate(NClickableControl.SignalName.MousePressed, (Delegate) this.backing_MousePressed);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NClickableControl.PropertyName.IsFocused, ref variant1))
      this.IsFocused = ((Variant) ref variant1).As<bool>();
    Variant variant2;
    if (info.TryGetProperty(NClickableControl.PropertyName._ignoreDragThreshold, ref variant2))
      this._ignoreDragThreshold = ((Variant) ref variant2).As<float>();
    Variant variant3;
    if (info.TryGetProperty(NClickableControl.PropertyName._isEnabled, ref variant3))
      this._isEnabled = ((Variant) ref variant3).As<bool>();
    Variant variant4;
    if (info.TryGetProperty(NClickableControl.PropertyName._isHovered, ref variant4))
      this._isHovered = ((Variant) ref variant4).As<bool>();
    Variant variant5;
    if (info.TryGetProperty(NClickableControl.PropertyName._isControllerFocused, ref variant5))
      this._isControllerFocused = ((Variant) ref variant5).As<bool>();
    Variant variant6;
    if (info.TryGetProperty(NClickableControl.PropertyName._isControllerNavigable, ref variant6))
      this._isControllerNavigable = ((Variant) ref variant6).As<bool>();
    Variant variant7;
    if (info.TryGetProperty(NClickableControl.PropertyName._beginDragPosition, ref variant7))
      this._beginDragPosition = ((Variant) ref variant7).As<Vector2>();
    Variant variant8;
    if (info.TryGetProperty(NClickableControl.PropertyName._isPressed, ref variant8))
      this._isPressed = ((Variant) ref variant8).As<bool>();
    NClickableControl.ReleasedEventHandler releasedEventHandler1;
    if (info.TryGetSignalEventDelegate<NClickableControl.ReleasedEventHandler>(NClickableControl.SignalName.Released, ref releasedEventHandler1))
      this.backing_Released = releasedEventHandler1;
    NClickableControl.FocusedEventHandler focusedEventHandler;
    if (info.TryGetSignalEventDelegate<NClickableControl.FocusedEventHandler>(NClickableControl.SignalName.Focused, ref focusedEventHandler))
      this.backing_Focused = focusedEventHandler;
    NClickableControl.UnfocusedEventHandler unfocusedEventHandler;
    if (info.TryGetSignalEventDelegate<NClickableControl.UnfocusedEventHandler>(NClickableControl.SignalName.Unfocused, ref unfocusedEventHandler))
      this.backing_Unfocused = unfocusedEventHandler;
    NClickableControl.MouseReleasedEventHandler releasedEventHandler2;
    if (info.TryGetSignalEventDelegate<NClickableControl.MouseReleasedEventHandler>(NClickableControl.SignalName.MouseReleased, ref releasedEventHandler2))
      this.backing_MouseReleased = releasedEventHandler2;
    NClickableControl.MousePressedEventHandler pressedEventHandler;
    if (!info.TryGetSignalEventDelegate<NClickableControl.MousePressedEventHandler>(NClickableControl.SignalName.MousePressed, ref pressedEventHandler))
      return;
    this.backing_MousePressed = pressedEventHandler;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<MethodInfo> GetGodotSignalList()
  {
    return new List<MethodInfo>(5)
    {
      new MethodInfo(NClickableControl.SignalName.Released, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("button"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false)
      }, (List<Variant>) null),
      new MethodInfo(NClickableControl.SignalName.Focused, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("button"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false)
      }, (List<Variant>) null),
      new MethodInfo(NClickableControl.SignalName.Unfocused, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("button"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false)
      }, (List<Variant>) null),
      new MethodInfo(NClickableControl.SignalName.MouseReleased, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("inputEvent"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("InputEvent"), false)
      }, (List<Variant>) null),
      new MethodInfo(NClickableControl.SignalName.MousePressed, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("inputEvent"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("InputEvent"), false)
      }, (List<Variant>) null)
    };
  }

  public event NClickableControl.ReleasedEventHandler Released
  {
    add => this.backing_Released += value;
    remove => this.backing_Released -= value;
  }

  protected void EmitSignalReleased(NClickableControl button)
  {
    ((GodotObject) this).EmitSignal(NClickableControl.SignalName.Released, new Variant[1]
    {
      Variant.op_Implicit((GodotObject) button)
    });
  }

  public event NClickableControl.FocusedEventHandler Focused
  {
    add => this.backing_Focused += value;
    remove => this.backing_Focused -= value;
  }

  protected void EmitSignalFocused(NClickableControl button)
  {
    ((GodotObject) this).EmitSignal(NClickableControl.SignalName.Focused, new Variant[1]
    {
      Variant.op_Implicit((GodotObject) button)
    });
  }

  public event NClickableControl.UnfocusedEventHandler Unfocused
  {
    add => this.backing_Unfocused += value;
    remove => this.backing_Unfocused -= value;
  }

  protected void EmitSignalUnfocused(NClickableControl button)
  {
    ((GodotObject) this).EmitSignal(NClickableControl.SignalName.Unfocused, new Variant[1]
    {
      Variant.op_Implicit((GodotObject) button)
    });
  }

  public event NClickableControl.MouseReleasedEventHandler MouseReleased
  {
    add => this.backing_MouseReleased += value;
    remove => this.backing_MouseReleased -= value;
  }

  protected void EmitSignalMouseReleased(InputEvent inputEvent)
  {
    ((GodotObject) this).EmitSignal(NClickableControl.SignalName.MouseReleased, new Variant[1]
    {
      Variant.op_Implicit((GodotObject) inputEvent)
    });
  }

  public event NClickableControl.MousePressedEventHandler MousePressed
  {
    add => this.backing_MousePressed += value;
    remove => this.backing_MousePressed -= value;
  }

  protected void EmitSignalMousePressed(InputEvent inputEvent)
  {
    ((GodotObject) this).EmitSignal(NClickableControl.SignalName.MousePressed, new Variant[1]
    {
      Variant.op_Implicit((GodotObject) inputEvent)
    });
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RaiseGodotClassSignalCallbacks(
    in godot_string_name signal,
    NativeVariantPtrArgs args)
  {
    if (StringName.op_Equality(ref signal, NClickableControl.SignalName.Released) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      NClickableControl.ReleasedEventHandler backingReleased = this.backing_Released;
      if (backingReleased == null)
        return;
      backingReleased(VariantUtils.ConvertTo<NClickableControl>(ref ((NativeVariantPtrArgs) ref args)[0]));
    }
    else if (StringName.op_Equality(ref signal, NClickableControl.SignalName.Focused) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      NClickableControl.FocusedEventHandler backingFocused = this.backing_Focused;
      if (backingFocused == null)
        return;
      backingFocused(VariantUtils.ConvertTo<NClickableControl>(ref ((NativeVariantPtrArgs) ref args)[0]));
    }
    else if (StringName.op_Equality(ref signal, NClickableControl.SignalName.Unfocused) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      NClickableControl.UnfocusedEventHandler backingUnfocused = this.backing_Unfocused;
      if (backingUnfocused == null)
        return;
      backingUnfocused(VariantUtils.ConvertTo<NClickableControl>(ref ((NativeVariantPtrArgs) ref args)[0]));
    }
    else if (StringName.op_Equality(ref signal, NClickableControl.SignalName.MouseReleased) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      NClickableControl.MouseReleasedEventHandler backingMouseReleased = this.backing_MouseReleased;
      if (backingMouseReleased == null)
        return;
      backingMouseReleased(VariantUtils.ConvertTo<InputEvent>(ref ((NativeVariantPtrArgs) ref args)[0]));
    }
    else if (StringName.op_Equality(ref signal, NClickableControl.SignalName.MousePressed) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      NClickableControl.MousePressedEventHandler backingMousePressed = this.backing_MousePressed;
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
    return StringName.op_Equality(ref signal, NClickableControl.SignalName.Released) || StringName.op_Equality(ref signal, NClickableControl.SignalName.Focused) || StringName.op_Equality(ref signal, NClickableControl.SignalName.Unfocused) || StringName.op_Equality(ref signal, NClickableControl.SignalName.MouseReleased) || StringName.op_Equality(ref signal, NClickableControl.SignalName.MousePressed) || base.HasGodotClassSignal(ref signal);
  }

  [Signal]
  public delegate void ReleasedEventHandler(
  #nullable enable
  NClickableControl button);

  [Signal]
  public delegate void FocusedEventHandler(NClickableControl button);

  [Signal]
  public delegate void UnfocusedEventHandler(NClickableControl button);

  [Signal]
  public delegate void MouseReleasedEventHandler(InputEvent inputEvent);

  [Signal]
  public delegate void MousePressedEventHandler(InputEvent inputEvent);

  public class MethodName : Control.MethodName
  {
    public static readonly 
    #nullable disable
    StringName ConnectSignals = StringName.op_Implicit(nameof (ConnectSignals));
    public static readonly StringName OnVisibilityChanged = StringName.op_Implicit(nameof (OnVisibilityChanged));
    public static readonly StringName OnFocusHandler = StringName.op_Implicit(nameof (OnFocusHandler));
    public static readonly StringName OnUnFocusHandler = StringName.op_Implicit(nameof (OnUnFocusHandler));
    public static readonly StringName HandleMousePress = StringName.op_Implicit(nameof (HandleMousePress));
    public static readonly StringName HandleMouseRelease = StringName.op_Implicit(nameof (HandleMouseRelease));
    public static readonly StringName OnHoverHandler = StringName.op_Implicit(nameof (OnHoverHandler));
    public static readonly StringName OnUnhoverHandler = StringName.op_Implicit(nameof (OnUnhoverHandler));
    public static readonly StringName OnPressHandler = StringName.op_Implicit(nameof (OnPressHandler));
    public static readonly StringName OnReleaseHandler = StringName.op_Implicit(nameof (OnReleaseHandler));
    public static readonly StringName RefreshFocus = StringName.op_Implicit(nameof (RefreshFocus));
    public static readonly StringName OnFocus = StringName.op_Implicit(nameof (OnFocus));
    public static readonly StringName OnUnfocus = StringName.op_Implicit(nameof (OnUnfocus));
    public static readonly StringName OnPress = StringName.op_Implicit(nameof (OnPress));
    public static readonly StringName OnRelease = StringName.op_Implicit(nameof (OnRelease));
    public static readonly StringName _GuiInput = StringName.op_Implicit(nameof (_GuiInput));
    public static readonly StringName CheckMouseDragThreshold = StringName.op_Implicit(nameof (CheckMouseDragThreshold));
    public static readonly StringName DebugPress = StringName.op_Implicit(nameof (DebugPress));
    public static readonly StringName DebugRelease = StringName.op_Implicit(nameof (DebugRelease));
    public static readonly StringName ForceClick = StringName.op_Implicit(nameof (ForceClick));
    public static readonly StringName SetEnabled = StringName.op_Implicit(nameof (SetEnabled));
    public static readonly StringName Enable = StringName.op_Implicit(nameof (Enable));
    public static readonly StringName Disable = StringName.op_Implicit(nameof (Disable));
    public static readonly StringName OnEnable = StringName.op_Implicit(nameof (OnEnable));
    public static readonly StringName OnDisable = StringName.op_Implicit(nameof (OnDisable));
  }

  public class PropertyName : Control.PropertyName
  {
    public static readonly StringName AllowFocusWhileDisabled = StringName.op_Implicit(nameof (AllowFocusWhileDisabled));
    public static readonly StringName IsFocused = StringName.op_Implicit(nameof (IsFocused));
    public static readonly StringName IsEnabled = StringName.op_Implicit(nameof (IsEnabled));
    public static readonly StringName _ignoreDragThreshold = StringName.op_Implicit(nameof (_ignoreDragThreshold));
    public static readonly StringName _isEnabled = StringName.op_Implicit(nameof (_isEnabled));
    public static readonly StringName _isHovered = StringName.op_Implicit(nameof (_isHovered));
    public static readonly StringName _isControllerFocused = StringName.op_Implicit(nameof (_isControllerFocused));
    public static readonly StringName _isControllerNavigable = StringName.op_Implicit(nameof (_isControllerNavigable));
    public static readonly StringName _beginDragPosition = StringName.op_Implicit(nameof (_beginDragPosition));
    public static readonly StringName _isPressed = StringName.op_Implicit(nameof (_isPressed));
  }

  public class SignalName : Control.SignalName
  {
    public static readonly StringName Released = StringName.op_Implicit(nameof (Released));
    public static readonly StringName Focused = StringName.op_Implicit(nameof (Focused));
    public static readonly StringName Unfocused = StringName.op_Implicit(nameof (Unfocused));
    public static readonly StringName MouseReleased = StringName.op_Implicit(nameof (MouseReleased));
    public static readonly StringName MousePressed = StringName.op_Implicit(nameof (MousePressed));
  }
}
