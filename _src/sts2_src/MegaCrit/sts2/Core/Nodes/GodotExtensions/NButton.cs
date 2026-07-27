// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.GodotExtensions.NButton
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using System;
using System.Collections.Generic;
using System.ComponentModel;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.GodotExtensions;

[ScriptPath("res://src/Core/Nodes/GodotExtensions/NButton.cs")]
public class NButton : NClickableControl
{
  protected TextureRect? _controllerHotkeyIcon;

  protected virtual string? ClickedSfx => "event:/sfx/ui/clicks/ui_click";

  protected virtual string? HoveredSfx => "event:/sfx/ui/clicks/ui_hover";

  protected virtual string[] Hotkeys => Array.Empty<string>();

  protected virtual string? ControllerIconHotkey
  {
    get => this.Hotkeys.Length == 0 ? (string) null : this.Hotkeys[0];
  }

  private bool HasControllerHotkey => this.Hotkeys.Length != 0;

  public override void _Ready()
  {
    if (((object) this).GetType() != typeof (NButton))
    {
      Log.Error($"{((object) this).GetType()}");
      throw new InvalidOperationException("Don't call base._Ready()! Call ConnectSignals() instead.");
    }
    this.ConnectSignals();
  }

  protected override void ConnectSignals()
  {
    base.ConnectSignals();
    if (this.HasControllerHotkey)
      this.RegisterHotkeys();
    this.GetControllerIconNode();
    this.UpdateControllerButton();
  }

  protected virtual void GetControllerIconNode()
  {
    this._controllerHotkeyIcon = ((Node) this).GetNodeOrNull<TextureRect>(NodePath.op_Implicit("%ControllerIcon"));
  }

  public override void _EnterTree()
  {
    if (NControllerManager.Instance != null)
    {
      ((GodotObject) NControllerManager.Instance).Connect(NControllerManager.SignalName.MouseDetected, Callable.From(new Action(this.UpdateControllerButton)), 0U);
      ((GodotObject) NControllerManager.Instance).Connect(NControllerManager.SignalName.ControllerDetected, Callable.From(new Action(this.UpdateControllerButton)), 0U);
    }
    if (NInputManager.Instance == null)
      return;
    ((GodotObject) NInputManager.Instance).Connect(NInputManager.SignalName.InputRebound, Callable.From(new Action(this.UpdateControllerButton)), 0U);
  }

  public override void _Input(InputEvent inputEvent) => this.CheckMouseDragThreshold(inputEvent);

  protected override void OnPress()
  {
    if (this.ClickedSfx == null)
      return;
    SfxCmd.Play(this.ClickedSfx);
  }

  protected override void OnFocus()
  {
    if (this.HoveredSfx == null)
      return;
    SfxCmd.Play(this.HoveredSfx);
  }

  protected override void OnEnable()
  {
    this.RegisterHotkeys();
    this.UpdateControllerButton();
  }

  protected override void OnDisable()
  {
    this.UnregisterHotkeys();
    this.UpdateControllerButton();
  }

  protected void UpdateControllerButton()
  {
    if (this._controllerHotkeyIcon == null)
      return;
    NControllerManager instance = NControllerManager.Instance;
    if (instance == null)
      return;
    ((CanvasItem) this._controllerHotkeyIcon).Visible = instance.IsUsingController && this._isEnabled;
    if (this.ControllerIconHotkey == null)
      return;
    Texture2D hotkeyIcon = NInputManager.Instance.GetHotkeyIcon(this.ControllerIconHotkey);
    if (hotkeyIcon == null)
      return;
    this._controllerHotkeyIcon.Texture = hotkeyIcon;
  }

  protected void RegisterHotkeys()
  {
    if (!this.HasControllerHotkey || !this._isEnabled)
      return;
    foreach (string hotkey in this.Hotkeys)
    {
      NHotkeyManager.Instance.PushHotkeyPressedBinding(hotkey, new Action(((NClickableControl) this).OnPressHandler));
      NHotkeyManager.Instance.PushHotkeyReleasedBinding(hotkey, new Action(((NClickableControl) this).OnReleaseHandler));
    }
  }

  protected void UnregisterHotkeys()
  {
    if (!this.HasControllerHotkey)
      return;
    foreach (string hotkey in this.Hotkeys)
    {
      NHotkeyManager.Instance.RemoveHotkeyPressedBinding(hotkey, new Action(((NClickableControl) this).OnPressHandler));
      NHotkeyManager.Instance.RemoveHotkeyReleasedBinding(hotkey, new Action(((NClickableControl) this).OnReleaseHandler));
    }
  }

  public override void _ExitTree()
  {
    if (NControllerManager.Instance != null)
    {
      ((GodotObject) NControllerManager.Instance).Disconnect(NControllerManager.SignalName.ControllerDetected, Callable.From(new Action(this.UpdateControllerButton)));
      ((GodotObject) NControllerManager.Instance).Disconnect(NControllerManager.SignalName.MouseDetected, Callable.From(new Action(this.UpdateControllerButton)));
    }
    if (NInputManager.Instance != null)
      ((GodotObject) NInputManager.Instance).Disconnect(NInputManager.SignalName.InputRebound, Callable.From(new Action(this.UpdateControllerButton)));
    this.UnregisterHotkeys();
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(13)
    {
      new MethodInfo(NButton.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NButton.MethodName.ConnectSignals, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NButton.MethodName.GetControllerIconNode, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NButton.MethodName._EnterTree, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NButton.MethodName._Input, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("inputEvent"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("InputEvent"), false)
      }, (List<Variant>) null),
      new MethodInfo(NButton.MethodName.OnPress, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NButton.MethodName.OnFocus, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NButton.MethodName.OnEnable, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NButton.MethodName.OnDisable, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NButton.MethodName.UpdateControllerButton, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NButton.MethodName.RegisterHotkeys, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NButton.MethodName.UnregisterHotkeys, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NButton.MethodName._ExitTree, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NButton.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NButton.MethodName.ConnectSignals) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.ConnectSignals();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NButton.MethodName.GetControllerIconNode) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.GetControllerIconNode();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NButton.MethodName._EnterTree) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._EnterTree();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NButton.MethodName._Input) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      ((Node) this)._Input(VariantUtils.ConvertTo<InputEvent>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NButton.MethodName.OnPress) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnPress();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NButton.MethodName.OnFocus) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnFocus();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NButton.MethodName.OnEnable) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnEnable();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NButton.MethodName.OnDisable) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnDisable();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NButton.MethodName.UpdateControllerButton) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.UpdateControllerButton();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NButton.MethodName.RegisterHotkeys) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.RegisterHotkeys();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NButton.MethodName.UnregisterHotkeys) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.UnregisterHotkeys();
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NButton.MethodName._ExitTree) || ((NativeVariantPtrArgs) ref args).Count != 0)
      return base.InvokeGodotClassMethod(in method, args, out ret);
    ((Node) this)._ExitTree();
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NButton.MethodName._Ready) || StringName.op_Equality(ref method, NButton.MethodName.ConnectSignals) || StringName.op_Equality(ref method, NButton.MethodName.GetControllerIconNode) || StringName.op_Equality(ref method, NButton.MethodName._EnterTree) || StringName.op_Equality(ref method, NButton.MethodName._Input) || StringName.op_Equality(ref method, NButton.MethodName.OnPress) || StringName.op_Equality(ref method, NButton.MethodName.OnFocus) || StringName.op_Equality(ref method, NButton.MethodName.OnEnable) || StringName.op_Equality(ref method, NButton.MethodName.OnDisable) || StringName.op_Equality(ref method, NButton.MethodName.UpdateControllerButton) || StringName.op_Equality(ref method, NButton.MethodName.RegisterHotkeys) || StringName.op_Equality(ref method, NButton.MethodName.UnregisterHotkeys) || StringName.op_Equality(ref method, NButton.MethodName._ExitTree) || base.HasGodotClassMethod(in method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (!StringName.op_Equality(ref name, NButton.PropertyName._controllerHotkeyIcon))
      return base.SetGodotClassPropertyValue(in name, in value);
    this._controllerHotkeyIcon = VariantUtils.ConvertTo<TextureRect>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NButton.PropertyName.ClickedSfx))
    {
      ref godot_variant local = ref value;
      string clickedSfx = this.ClickedSfx;
      godot_variant from = VariantUtils.CreateFrom<string>(ref clickedSfx);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NButton.PropertyName.HoveredSfx))
    {
      ref godot_variant local = ref value;
      string hoveredSfx = this.HoveredSfx;
      godot_variant from = VariantUtils.CreateFrom<string>(ref hoveredSfx);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NButton.PropertyName.Hotkeys))
    {
      ref godot_variant local = ref value;
      string[] hotkeys = this.Hotkeys;
      godot_variant from = VariantUtils.CreateFrom<string[]>(ref hotkeys);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NButton.PropertyName.ControllerIconHotkey))
    {
      ref godot_variant local = ref value;
      string controllerIconHotkey = this.ControllerIconHotkey;
      godot_variant from = VariantUtils.CreateFrom<string>(ref controllerIconHotkey);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NButton.PropertyName.HasControllerHotkey))
    {
      ref godot_variant local = ref value;
      bool controllerHotkey = this.HasControllerHotkey;
      godot_variant from = VariantUtils.CreateFrom<bool>(ref controllerHotkey);
      local = from;
      return true;
    }
    if (!StringName.op_Equality(ref name, NButton.PropertyName._controllerHotkeyIcon))
      return base.GetGodotClassPropertyValue(in name, out value);
    value = VariantUtils.CreateFrom<TextureRect>(ref this._controllerHotkeyIcon);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 4L, NButton.PropertyName.ClickedSfx, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 4L, NButton.PropertyName.HoveredSfx, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 34L, NButton.PropertyName.Hotkeys, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 4L, NButton.PropertyName.ControllerIconHotkey, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 1L, NButton.PropertyName.HasControllerHotkey, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NButton.PropertyName._controllerHotkeyIcon, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    base.SaveGodotObjectData(info);
    info.AddProperty(NButton.PropertyName._controllerHotkeyIcon, Variant.From<TextureRect>(ref this._controllerHotkeyIcon));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    base.RestoreGodotObjectData(info);
    Variant variant;
    if (!info.TryGetProperty(NButton.PropertyName._controllerHotkeyIcon, ref variant))
      return;
    this._controllerHotkeyIcon = ((Variant) ref variant).As<TextureRect>();
  }

  public new class MethodName : NClickableControl.MethodName
  {
    public static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public new static readonly StringName ConnectSignals = StringName.op_Implicit(nameof (ConnectSignals));
    public static readonly StringName GetControllerIconNode = StringName.op_Implicit(nameof (GetControllerIconNode));
    public static readonly StringName _EnterTree = StringName.op_Implicit(nameof (_EnterTree));
    public static readonly StringName _Input = StringName.op_Implicit(nameof (_Input));
    public new static readonly StringName OnPress = StringName.op_Implicit(nameof (OnPress));
    public new static readonly StringName OnFocus = StringName.op_Implicit(nameof (OnFocus));
    public new static readonly StringName OnEnable = StringName.op_Implicit(nameof (OnEnable));
    public new static readonly StringName OnDisable = StringName.op_Implicit(nameof (OnDisable));
    public static readonly StringName UpdateControllerButton = StringName.op_Implicit(nameof (UpdateControllerButton));
    public static readonly StringName RegisterHotkeys = StringName.op_Implicit(nameof (RegisterHotkeys));
    public static readonly StringName UnregisterHotkeys = StringName.op_Implicit(nameof (UnregisterHotkeys));
    public static readonly StringName _ExitTree = StringName.op_Implicit(nameof (_ExitTree));
  }

  public new class PropertyName : NClickableControl.PropertyName
  {
    public static readonly StringName ClickedSfx = StringName.op_Implicit(nameof (ClickedSfx));
    public static readonly StringName HoveredSfx = StringName.op_Implicit(nameof (HoveredSfx));
    public static readonly StringName Hotkeys = StringName.op_Implicit(nameof (Hotkeys));
    public static readonly StringName ControllerIconHotkey = StringName.op_Implicit(nameof (ControllerIconHotkey));
    public static readonly StringName HasControllerHotkey = StringName.op_Implicit(nameof (HasControllerHotkey));
    public static readonly StringName _controllerHotkeyIcon = StringName.op_Implicit(nameof (_controllerHotkeyIcon));
  }

  public new class SignalName : NClickableControl.SignalName
  {
  }
}
