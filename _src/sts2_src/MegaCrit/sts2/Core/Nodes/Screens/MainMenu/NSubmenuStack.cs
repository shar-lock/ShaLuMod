// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Screens.MainMenu.NSubmenuStack
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Nodes.Screens.ScreenContext;
using System;
using System.Collections.Generic;
using System.ComponentModel;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Screens.MainMenu;

[ScriptPath("res://src/Core/Nodes/Screens/MainMenu/NSubmenuStack.cs")]
public abstract class NSubmenuStack : Control
{
  private readonly Stack<NSubmenu> _submenus = new Stack<NSubmenu>();
  private NMainMenu? _mainMenu;
  private 
  #nullable disable
  NSubmenuStack.StackModifiedEventHandler backing_StackModified;

  public bool SubmenusOpen => this._submenus.Count > 0;

  public void InitializeForMainMenu(
  #nullable enable
  NMainMenu mainMenu) => this._mainMenu = mainMenu;

  public abstract T PushSubmenuType<T>() where T : NSubmenu;

  public abstract T GetSubmenuType<T>() where T : NSubmenu;

  public abstract NSubmenu PushSubmenuType(Type type);

  public abstract NSubmenu GetSubmenuType(Type type);

  public void Push(NSubmenu screen)
  {
    if (this._submenus.Count > 0)
    {
      NSubmenu nsubmenu = this._submenus.Peek();
      ((CanvasItem) nsubmenu).Visible = false;
      nsubmenu.MouseFilter = (Control.MouseFilterEnum) 2L;
    }
    screen.SetStack(this);
    this._submenus.Push(screen);
    screen.OnSubmenuOpened();
    ((CanvasItem) screen).Visible = true;
    screen.MouseFilter = (Control.MouseFilterEnum) 0L;
    this._mainMenu?.EnableBackstop();
    ActiveScreenContext.Instance.Update();
    ((GodotObject) this).EmitSignal(NSubmenuStack.SignalName.StackModified, Array.Empty<Variant>());
  }

  public void Pop()
  {
    NSubmenu nsubmenu1 = this._submenus.Pop();
    ((CanvasItem) nsubmenu1).Visible = false;
    nsubmenu1.MouseFilter = (Control.MouseFilterEnum) 2L;
    nsubmenu1.OnSubmenuClosed();
    if (this._submenus.Count > 0)
    {
      NSubmenu nsubmenu2 = this._submenus.Peek();
      ((CanvasItem) nsubmenu2).Visible = true;
      nsubmenu2.MouseFilter = (Control.MouseFilterEnum) 0L;
    }
    else
      this.HideBackstop();
    ActiveScreenContext.Instance.Update();
    ((GodotObject) this).EmitSignal(NSubmenuStack.SignalName.StackModified, Array.Empty<Variant>());
  }

  private void ShowBackstop() => this._mainMenu?.EnableBackstop();

  private void HideBackstop() => this._mainMenu?.DisableBackstop();

  public NSubmenu? Peek()
  {
    NSubmenu nsubmenu;
    return !this._submenus.TryPeek(ref nsubmenu) ? (NSubmenu) null : nsubmenu;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(6)
    {
      new MethodInfo(NSubmenuStack.MethodName.InitializeForMainMenu, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("mainMenu"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false)
      }, (List<Variant>) null),
      new MethodInfo(NSubmenuStack.MethodName.Push, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("screen"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false)
      }, (List<Variant>) null),
      new MethodInfo(NSubmenuStack.MethodName.Pop, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NSubmenuStack.MethodName.ShowBackstop, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NSubmenuStack.MethodName.HideBackstop, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NSubmenuStack.MethodName.Peek, new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NSubmenuStack.MethodName.InitializeForMainMenu) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.InitializeForMainMenu(VariantUtils.ConvertTo<NMainMenu>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NSubmenuStack.MethodName.Push) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.Push(VariantUtils.ConvertTo<NSubmenu>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NSubmenuStack.MethodName.Pop) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.Pop();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NSubmenuStack.MethodName.ShowBackstop) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.ShowBackstop();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NSubmenuStack.MethodName.HideBackstop) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.HideBackstop();
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NSubmenuStack.MethodName.Peek) || ((NativeVariantPtrArgs) ref args).Count != 0)
      return base.InvokeGodotClassMethod(ref method, args, ref ret);
    NSubmenu nsubmenu = this.Peek();
    ret = VariantUtils.CreateFrom<NSubmenu>(ref nsubmenu);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NSubmenuStack.MethodName.InitializeForMainMenu) || StringName.op_Equality(ref method, NSubmenuStack.MethodName.Push) || StringName.op_Equality(ref method, NSubmenuStack.MethodName.Pop) || StringName.op_Equality(ref method, NSubmenuStack.MethodName.ShowBackstop) || StringName.op_Equality(ref method, NSubmenuStack.MethodName.HideBackstop) || StringName.op_Equality(ref method, NSubmenuStack.MethodName.Peek) || base.HasGodotClassMethod(ref method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (!StringName.op_Equality(ref name, NSubmenuStack.PropertyName._mainMenu))
      return ((GodotObject) this).SetGodotClassPropertyValue(ref name, ref value);
    this._mainMenu = VariantUtils.ConvertTo<NMainMenu>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NSubmenuStack.PropertyName.SubmenusOpen))
    {
      ref godot_variant local = ref value;
      bool submenusOpen = this.SubmenusOpen;
      godot_variant from = VariantUtils.CreateFrom<bool>(ref submenusOpen);
      local = from;
      return true;
    }
    if (!StringName.op_Equality(ref name, NSubmenuStack.PropertyName._mainMenu))
      return ((GodotObject) this).GetGodotClassPropertyValue(ref name, ref value);
    value = VariantUtils.CreateFrom<NMainMenu>(ref this._mainMenu);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NSubmenuStack.PropertyName._mainMenu, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 1L, NSubmenuStack.PropertyName.SubmenusOpen, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).SaveGodotObjectData(info);
    info.AddProperty(NSubmenuStack.PropertyName._mainMenu, Variant.From<NMainMenu>(ref this._mainMenu));
    info.AddSignalEventDelegate(NSubmenuStack.SignalName.StackModified, (Delegate) this.backing_StackModified);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).RestoreGodotObjectData(info);
    Variant variant;
    if (info.TryGetProperty(NSubmenuStack.PropertyName._mainMenu, ref variant))
      this._mainMenu = ((Variant) ref variant).As<NMainMenu>();
    NSubmenuStack.StackModifiedEventHandler modifiedEventHandler;
    if (!info.TryGetSignalEventDelegate<NSubmenuStack.StackModifiedEventHandler>(NSubmenuStack.SignalName.StackModified, ref modifiedEventHandler))
      return;
    this.backing_StackModified = modifiedEventHandler;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<MethodInfo> GetGodotSignalList()
  {
    return new List<MethodInfo>(1)
    {
      new MethodInfo(NSubmenuStack.SignalName.StackModified, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  public event NSubmenuStack.StackModifiedEventHandler StackModified
  {
    add => this.backing_StackModified += value;
    remove => this.backing_StackModified -= value;
  }

  protected void EmitSignalStackModified()
  {
    ((GodotObject) this).EmitSignal(NSubmenuStack.SignalName.StackModified, Array.Empty<Variant>());
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RaiseGodotClassSignalCallbacks(
    in godot_string_name signal,
    NativeVariantPtrArgs args)
  {
    if (StringName.op_Equality(ref signal, NSubmenuStack.SignalName.StackModified) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      NSubmenuStack.StackModifiedEventHandler backingStackModified = this.backing_StackModified;
      if (backingStackModified == null)
        return;
      backingStackModified();
    }
    else
      ((GodotObject) this).RaiseGodotClassSignalCallbacks(ref signal, args);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassSignal(in godot_string_name signal)
  {
    return StringName.op_Equality(ref signal, NSubmenuStack.SignalName.StackModified) || base.HasGodotClassSignal(ref signal);
  }

  [Signal]
  public delegate void StackModifiedEventHandler();

  public class MethodName : Control.MethodName
  {
    public static readonly StringName InitializeForMainMenu = StringName.op_Implicit(nameof (InitializeForMainMenu));
    public static readonly StringName Push = StringName.op_Implicit(nameof (Push));
    public static readonly StringName Pop = StringName.op_Implicit(nameof (Pop));
    public static readonly StringName ShowBackstop = StringName.op_Implicit(nameof (ShowBackstop));
    public static readonly StringName HideBackstop = StringName.op_Implicit(nameof (HideBackstop));
    public static readonly StringName Peek = StringName.op_Implicit(nameof (Peek));
  }

  public class PropertyName : Control.PropertyName
  {
    public static readonly StringName SubmenusOpen = StringName.op_Implicit(nameof (SubmenusOpen));
    public static readonly StringName _mainMenu = StringName.op_Implicit(nameof (_mainMenu));
  }

  public class SignalName : Control.SignalName
  {
    public static readonly StringName StackModified = StringName.op_Implicit(nameof (StackModified));
  }
}
