// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.CommonUi.NHotkeyManager
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.ControllerInput;
using MegaCrit.Sts2.Core.Nodes.Debug;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.CommonUi;

[ScriptPath("res://src/Core/Nodes/CommonUi/NHotkeyManager.cs")]
public class NHotkeyManager : Node
{
  private readonly Dictionary<StringName, List<Action>> _hotkeyPressedBindings = new Dictionary<StringName, List<Action>>();
  private readonly Dictionary<StringName, List<Action>> _hotkeyReleasedBindings = new Dictionary<StringName, List<Action>>();
  private Dictionary<Node, Action> _blockingScreens = new Dictionary<Node, Action>();

  public static NHotkeyManager? Instance
  {
    get => NGame.Instance == null ? (NHotkeyManager) null : NGame.Instance.HotkeyManager;
  }

  public void PushHotkeyPressedBinding(string hotkey, Action action)
  {
    if (!this._hotkeyPressedBindings.ContainsKey(StringName.op_Implicit(hotkey)))
      this._hotkeyPressedBindings.Add(StringName.op_Implicit(hotkey), new List<Action>());
    if (this._hotkeyPressedBindings[StringName.op_Implicit(hotkey)].Contains(action))
      return;
    this._hotkeyPressedBindings[StringName.op_Implicit(hotkey)].Add(action);
  }

  public void RemoveHotkeyPressedBinding(string hotkey, Action action)
  {
    List<Action> actionList;
    if (!this._hotkeyPressedBindings.TryGetValue(StringName.op_Implicit(hotkey), out actionList))
      return;
    actionList.Remove(action);
    if (this._hotkeyPressedBindings[StringName.op_Implicit(hotkey)].Count != 0)
      return;
    this._hotkeyPressedBindings.Remove(StringName.op_Implicit(hotkey));
  }

  public void PushHotkeyReleasedBinding(string hotkey, Action action)
  {
    if (!this._hotkeyReleasedBindings.ContainsKey(StringName.op_Implicit(hotkey)))
      this._hotkeyReleasedBindings.Add(StringName.op_Implicit(hotkey), new List<Action>());
    if (this._hotkeyReleasedBindings[StringName.op_Implicit(hotkey)].Contains(action))
      return;
    this._hotkeyReleasedBindings[StringName.op_Implicit(hotkey)].Add(action);
  }

  public void RemoveHotkeyReleasedBinding(string hotkey, Action action)
  {
    List<Action> actionList;
    if (!this._hotkeyReleasedBindings.TryGetValue(StringName.op_Implicit(hotkey), out actionList))
      return;
    actionList.Remove(action);
    if (this._hotkeyReleasedBindings[StringName.op_Implicit(hotkey)].Count != 0)
      return;
    this._hotkeyReleasedBindings.Remove(StringName.op_Implicit(hotkey));
  }

  public void AddBlockingScreen(Node screen)
  {
    Action action = (Action) (() => { });
    foreach (string allInput in MegaInput.AllInputs)
      this.PushHotkeyPressedBinding(allInput, action);
    foreach (string allInput in MegaInput.AllInputs)
      this.PushHotkeyReleasedBinding(allInput, action);
    this._blockingScreens.Add(screen, action);
  }

  public void RemoveBlockingScreen(Node screen)
  {
    Action action;
    if (!this._blockingScreens.TryGetValue(screen, out action))
      return;
    foreach (string allInput in MegaInput.AllInputs)
      this.RemoveHotkeyPressedBinding(allInput, action);
    foreach (string allInput in MegaInput.AllInputs)
      this.RemoveHotkeyReleasedBinding(allInput, action);
    this._blockingScreens.Remove(screen);
  }

  public override void _UnhandledInput(InputEvent inputEvent)
  {
    if (!NGame.IsGameFocusedWindow() || NDevConsole.IsConsoleVisible)
      return;
    Viewport viewport = this.GetViewport();
    Control focusOwner = viewport?.GuiGetFocusOwner();
    if (focusOwner != null && (focusOwner is LineEdit lineEdit && lineEdit.IsEditing() || focusOwner is NMegaTextEdit nmegaTextEdit && nmegaTextEdit.IsEditing()))
      return;
    foreach (KeyValuePair<StringName, List<Action>> hotkeyPressedBinding in this._hotkeyPressedBindings)
    {
      if (inputEvent.IsActionPressed(hotkeyPressedBinding.Key, false, false) && !inputEvent.IsEcho())
      {
        Action action = hotkeyPressedBinding.Value.LastOrDefault<Action>();
        if (action != null)
        {
          Callable callable = Callable.From(new Action(action.Invoke));
          ((Callable) ref callable).CallDeferred(Array.Empty<Variant>());
          viewport?.SetInputAsHandled();
        }
      }
    }
    foreach (KeyValuePair<StringName, List<Action>> hotkeyReleasedBinding in this._hotkeyReleasedBindings)
    {
      if (inputEvent.IsActionReleased(hotkeyReleasedBinding.Key, false))
      {
        Action action = hotkeyReleasedBinding.Value.LastOrDefault<Action>();
        if (action != null)
        {
          Callable callable = Callable.From(new Action(action.Invoke));
          ((Callable) ref callable).CallDeferred(Array.Empty<Variant>());
          viewport?.SetInputAsHandled();
        }
      }
    }
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(3)
    {
      new MethodInfo(NHotkeyManager.MethodName.AddBlockingScreen, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("screen"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Node"), false)
      }, (List<Variant>) null),
      new MethodInfo(NHotkeyManager.MethodName.RemoveBlockingScreen, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("screen"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Node"), false)
      }, (List<Variant>) null),
      new MethodInfo(NHotkeyManager.MethodName._UnhandledInput, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
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
    if (StringName.op_Equality(ref method, NHotkeyManager.MethodName.AddBlockingScreen) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.AddBlockingScreen(VariantUtils.ConvertTo<Node>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NHotkeyManager.MethodName.RemoveBlockingScreen) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.RemoveBlockingScreen(VariantUtils.ConvertTo<Node>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NHotkeyManager.MethodName._UnhandledInput) || ((NativeVariantPtrArgs) ref args).Count != 1)
      return base.InvokeGodotClassMethod(ref method, args, ref ret);
    base._UnhandledInput(VariantUtils.ConvertTo<InputEvent>(ref ((NativeVariantPtrArgs) ref args)[0]));
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NHotkeyManager.MethodName.AddBlockingScreen) || StringName.op_Equality(ref method, NHotkeyManager.MethodName.RemoveBlockingScreen) || StringName.op_Equality(ref method, NHotkeyManager.MethodName._UnhandledInput) || base.HasGodotClassMethod(ref method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).SaveGodotObjectData(info);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).RestoreGodotObjectData(info);
  }

  public class MethodName : Node.MethodName
  {
    public static readonly StringName AddBlockingScreen = StringName.op_Implicit(nameof (AddBlockingScreen));
    public static readonly StringName RemoveBlockingScreen = StringName.op_Implicit(nameof (RemoveBlockingScreen));
    public static readonly StringName _UnhandledInput = StringName.op_Implicit(nameof (_UnhandledInput));
  }

  public class PropertyName : Node.PropertyName
  {
  }

  public class SignalName : Node.SignalName
  {
  }
}
