// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Screens.MainMenu.NSubmenu
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.Screens.ScreenContext;
using System;
using System.Collections.Generic;
using System.ComponentModel;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Screens.MainMenu;

[ScriptPath("res://src/Core/Nodes/Screens/MainMenu/NSubmenu.cs")]
public abstract class NSubmenu : Control, IScreenContext
{
  private NBackButton _backButton;
  protected NSubmenuStack _stack;
  protected Control? _lastFocusedControl;

  public override void _Ready()
  {
    if (((object) this).GetType() != typeof (NSubmenu))
    {
      Log.Error($"{((object) this).GetType()}");
      throw new InvalidOperationException("Don't call base._Ready()! Call ConnectSignals() instead.");
    }
    this.ConnectSignals();
  }

  protected virtual void ConnectSignals()
  {
    this._backButton = ((Node) this).GetNode<NBackButton>(NodePath.op_Implicit("BackButton"));
    ((GodotObject) this._backButton).Connect(NClickableControl.SignalName.Released, Callable.From<NButton>((Action<NButton>) (_ => this._stack.Pop())), 0U);
    this._backButton.Disable();
    ((GodotObject) this).Connect(CanvasItem.SignalName.VisibilityChanged, Callable.From(new Action(this.OnScreenVisibilityChange)), 0U);
  }

  public void HideBackButtonImmediately()
  {
    this._backButton.Disable();
    this._backButton.MoveToHidePosition();
  }

  public void SetStack(NSubmenuStack stack) => this._stack = stack;

  private void OnScreenVisibilityChange()
  {
    if (((CanvasItem) this).Visible)
    {
      this._backButton.MoveToHidePosition();
      this._backButton.Enable();
      this.OnSubmenuShown();
    }
    else
    {
      this._lastFocusedControl = ((Node) this).GetViewport()?.GuiGetFocusOwner();
      this._backButton.Disable();
      this.OnSubmenuHidden();
    }
  }

  public Control? DefaultFocusedControl => this._lastFocusedControl ?? this.InitialFocusedControl;

  protected abstract Control? InitialFocusedControl { get; }

  protected virtual void OnSubmenuShown()
  {
  }

  protected virtual void OnSubmenuHidden()
  {
  }

  public virtual void OnSubmenuOpened()
  {
  }

  public virtual void OnSubmenuClosed() => this._lastFocusedControl = (Control) null;

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(9)
    {
      new MethodInfo(NSubmenu.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NSubmenu.MethodName.ConnectSignals, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NSubmenu.MethodName.HideBackButtonImmediately, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NSubmenu.MethodName.SetStack, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("stack"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false)
      }, (List<Variant>) null),
      new MethodInfo(NSubmenu.MethodName.OnScreenVisibilityChange, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NSubmenu.MethodName.OnSubmenuShown, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NSubmenu.MethodName.OnSubmenuHidden, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NSubmenu.MethodName.OnSubmenuOpened, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NSubmenu.MethodName.OnSubmenuClosed, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NSubmenu.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NSubmenu.MethodName.ConnectSignals) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.ConnectSignals();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NSubmenu.MethodName.HideBackButtonImmediately) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.HideBackButtonImmediately();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NSubmenu.MethodName.SetStack) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.SetStack(VariantUtils.ConvertTo<NSubmenuStack>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NSubmenu.MethodName.OnScreenVisibilityChange) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnScreenVisibilityChange();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NSubmenu.MethodName.OnSubmenuShown) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnSubmenuShown();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NSubmenu.MethodName.OnSubmenuHidden) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnSubmenuHidden();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NSubmenu.MethodName.OnSubmenuOpened) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnSubmenuOpened();
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NSubmenu.MethodName.OnSubmenuClosed) || ((NativeVariantPtrArgs) ref args).Count != 0)
      return base.InvokeGodotClassMethod(ref method, args, ref ret);
    this.OnSubmenuClosed();
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NSubmenu.MethodName._Ready) || StringName.op_Equality(ref method, NSubmenu.MethodName.ConnectSignals) || StringName.op_Equality(ref method, NSubmenu.MethodName.HideBackButtonImmediately) || StringName.op_Equality(ref method, NSubmenu.MethodName.SetStack) || StringName.op_Equality(ref method, NSubmenu.MethodName.OnScreenVisibilityChange) || StringName.op_Equality(ref method, NSubmenu.MethodName.OnSubmenuShown) || StringName.op_Equality(ref method, NSubmenu.MethodName.OnSubmenuHidden) || StringName.op_Equality(ref method, NSubmenu.MethodName.OnSubmenuOpened) || StringName.op_Equality(ref method, NSubmenu.MethodName.OnSubmenuClosed) || base.HasGodotClassMethod(ref method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NSubmenu.PropertyName._backButton))
    {
      this._backButton = VariantUtils.ConvertTo<NBackButton>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NSubmenu.PropertyName._stack))
    {
      this._stack = VariantUtils.ConvertTo<NSubmenuStack>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NSubmenu.PropertyName._lastFocusedControl))
      return ((GodotObject) this).SetGodotClassPropertyValue(ref name, ref value);
    this._lastFocusedControl = VariantUtils.ConvertTo<Control>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NSubmenu.PropertyName.DefaultFocusedControl))
    {
      ref godot_variant local = ref value;
      Control defaultFocusedControl = this.DefaultFocusedControl;
      godot_variant from = VariantUtils.CreateFrom<Control>(ref defaultFocusedControl);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NSubmenu.PropertyName.InitialFocusedControl))
    {
      ref godot_variant local = ref value;
      Control initialFocusedControl = this.InitialFocusedControl;
      godot_variant from = VariantUtils.CreateFrom<Control>(ref initialFocusedControl);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NSubmenu.PropertyName._backButton))
    {
      value = VariantUtils.CreateFrom<NBackButton>(ref this._backButton);
      return true;
    }
    if (StringName.op_Equality(ref name, NSubmenu.PropertyName._stack))
    {
      value = VariantUtils.CreateFrom<NSubmenuStack>(ref this._stack);
      return true;
    }
    if (!StringName.op_Equality(ref name, NSubmenu.PropertyName._lastFocusedControl))
      return ((GodotObject) this).GetGodotClassPropertyValue(ref name, ref value);
    value = VariantUtils.CreateFrom<Control>(ref this._lastFocusedControl);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NSubmenu.PropertyName._backButton, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NSubmenu.PropertyName._stack, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NSubmenu.PropertyName._lastFocusedControl, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NSubmenu.PropertyName.DefaultFocusedControl, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NSubmenu.PropertyName.InitialFocusedControl, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).SaveGodotObjectData(info);
    info.AddProperty(NSubmenu.PropertyName._backButton, Variant.From<NBackButton>(ref this._backButton));
    info.AddProperty(NSubmenu.PropertyName._stack, Variant.From<NSubmenuStack>(ref this._stack));
    info.AddProperty(NSubmenu.PropertyName._lastFocusedControl, Variant.From<Control>(ref this._lastFocusedControl));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NSubmenu.PropertyName._backButton, ref variant1))
      this._backButton = ((Variant) ref variant1).As<NBackButton>();
    Variant variant2;
    if (info.TryGetProperty(NSubmenu.PropertyName._stack, ref variant2))
      this._stack = ((Variant) ref variant2).As<NSubmenuStack>();
    Variant variant3;
    if (!info.TryGetProperty(NSubmenu.PropertyName._lastFocusedControl, ref variant3))
      return;
    this._lastFocusedControl = ((Variant) ref variant3).As<Control>();
  }

  public class MethodName : Control.MethodName
  {
    public static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName ConnectSignals = StringName.op_Implicit(nameof (ConnectSignals));
    public static readonly StringName HideBackButtonImmediately = StringName.op_Implicit(nameof (HideBackButtonImmediately));
    public static readonly StringName SetStack = StringName.op_Implicit(nameof (SetStack));
    public static readonly StringName OnScreenVisibilityChange = StringName.op_Implicit(nameof (OnScreenVisibilityChange));
    public static readonly StringName OnSubmenuShown = StringName.op_Implicit(nameof (OnSubmenuShown));
    public static readonly StringName OnSubmenuHidden = StringName.op_Implicit(nameof (OnSubmenuHidden));
    public static readonly StringName OnSubmenuOpened = StringName.op_Implicit(nameof (OnSubmenuOpened));
    public static readonly StringName OnSubmenuClosed = StringName.op_Implicit(nameof (OnSubmenuClosed));
  }

  public class PropertyName : Control.PropertyName
  {
    public static readonly StringName DefaultFocusedControl = StringName.op_Implicit(nameof (DefaultFocusedControl));
    public static readonly StringName InitialFocusedControl = StringName.op_Implicit(nameof (InitialFocusedControl));
    public static readonly StringName _backButton = StringName.op_Implicit(nameof (_backButton));
    public static readonly StringName _stack = StringName.op_Implicit(nameof (_stack));
    public static readonly StringName _lastFocusedControl = StringName.op_Implicit(nameof (_lastFocusedControl));
  }

  public class SignalName : Control.SignalName
  {
  }
}
