// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.CommonUi.NInputManager
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.ControllerInput;
using MegaCrit.Sts2.Core.Debug;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Nodes.Debug;
using MegaCrit.Sts2.Core.Platform;
using MegaCrit.Sts2.Core.Saves;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.CommonUi;

[ScriptPath("res://src/Core/Nodes/CommonUi/NInputManager.cs")]
public class NInputManager : Node
{
  private readonly Dictionary<Key, StringName> _debugInputMap = new Dictionary<Key, StringName>()
  {
    {
      (Key) 49L,
      DebugHotkey.hideTopBar
    },
    {
      (Key) 50L,
      DebugHotkey.hideIntents
    },
    {
      (Key) 51L,
      DebugHotkey.hideCombatUi
    },
    {
      (Key) 52L,
      DebugHotkey.hidePlayContainer
    },
    {
      (Key) 53L,
      DebugHotkey.hideHand
    },
    {
      (Key) 54L,
      DebugHotkey.hideHpBars
    },
    {
      (Key) 55L,
      DebugHotkey.hideTextVfx
    },
    {
      (Key) 56L,
      DebugHotkey.hideTargetingUi
    },
    {
      (Key) 57L,
      DebugHotkey.slowRewards
    },
    {
      (Key) 48L /*0x30*/,
      DebugHotkey.hideVersionInfo
    },
    {
      (Key) 45L,
      DebugHotkey.speedDown
    },
    {
      (Key) 61L,
      DebugHotkey.speedUp
    },
    {
      (Key) 4194332L,
      DebugHotkey.hideRestSite
    },
    {
      (Key) 4194334L,
      DebugHotkey.hideEventUi
    },
    {
      (Key) 4194335L,
      DebugHotkey.hideProceedButton
    },
    {
      (Key) 4194336L /*0x400020*/,
      DebugHotkey.hideHoverTips
    },
    {
      (Key) 4194337L,
      DebugHotkey.hideMpCursors
    },
    {
      (Key) 4194338L,
      DebugHotkey.hideMpTargeting
    },
    {
      (Key) 4194340L,
      DebugHotkey.hideMpIntents
    },
    {
      (Key) 4194341L,
      DebugHotkey.hideMpHealthBars
    },
    {
      (Key) 85L,
      DebugHotkey.unlockCharacters
    }
  };
  public static readonly IReadOnlyList<StringName> remappableKeyboardInputs = (IReadOnlyList<StringName>) new List<StringName>()
  {
    MegaInput.select,
    MegaInput.cancel,
    MegaInput.viewMap,
    MegaInput.viewDeckAndTabLeft,
    MegaInput.viewDrawPile,
    MegaInput.viewDiscardPile,
    MegaInput.viewExhaustPileAndTabRight,
    MegaInput.accept,
    MegaInput.peek,
    MegaInput.up,
    MegaInput.down,
    MegaInput.left,
    MegaInput.right,
    MegaInput.selectCard1,
    MegaInput.selectCard2,
    MegaInput.selectCard3,
    MegaInput.selectCard4,
    MegaInput.selectCard5,
    MegaInput.selectCard6,
    MegaInput.selectCard7,
    MegaInput.selectCard8,
    MegaInput.selectCard9,
    MegaInput.selectCard10,
    MegaInput.releaseCard
  };
  public static readonly IReadOnlyList<StringName> remappableControllerInputs = (IReadOnlyList<StringName>) new List<StringName>()
  {
    MegaInput.select,
    MegaInput.cancel,
    MegaInput.viewMap,
    MegaInput.topPanel,
    MegaInput.viewDeckAndTabLeft,
    MegaInput.viewDrawPile,
    MegaInput.viewDiscardPile,
    MegaInput.viewExhaustPileAndTabRight,
    MegaInput.accept,
    MegaInput.peek,
    MegaInput.up,
    MegaInput.down,
    MegaInput.left,
    MegaInput.right
  };
  private Dictionary<StringName, Key> _keyboardInputMap = new Dictionary<StringName, Key>();
  private Dictionary<StringName, StringName> _controllerInputMap = new Dictionary<StringName, StringName>();
  private 
  #nullable disable
  NInputManager.InputReboundEventHandler backing_InputRebound;

  public static 
  #nullable enable
  NInputManager? Instance
  {
    get => NGame.Instance == null ? (NInputManager) null : NGame.Instance.InputManager;
  }

  private static Dictionary<StringName, Key> DefaultKeyboardInputMap
  {
    get
    {
      return new Dictionary<StringName, Key>()
      {
        {
          MegaInput.accept,
          (Key) 69L
        },
        {
          MegaInput.select,
          (Key) 4194309L /*0x400005*/
        },
        {
          MegaInput.viewDiscardPile,
          (Key) 83L
        },
        {
          MegaInput.viewDeckAndTabLeft,
          (Key) 68L
        },
        {
          MegaInput.viewExhaustPileAndTabRight,
          (Key) 88L
        },
        {
          MegaInput.viewDrawPile,
          (Key) 65L
        },
        {
          MegaInput.viewMap,
          (Key) 77L
        },
        {
          MegaInput.cancel,
          (Key) 4194305L /*0x400001*/
        },
        {
          MegaInput.peek,
          (Key) 32L /*0x20*/
        },
        {
          MegaInput.up,
          (Key) 4194320L /*0x400010*/
        },
        {
          MegaInput.down,
          (Key) 4194322L
        },
        {
          MegaInput.left,
          (Key) 4194319L /*0x40000F*/
        },
        {
          MegaInput.right,
          (Key) 4194321L
        },
        {
          MegaInput.pauseAndBack,
          (Key) 4194305L /*0x400001*/
        },
        {
          MegaInput.selectCard1,
          (Key) 49L
        },
        {
          MegaInput.selectCard2,
          (Key) 50L
        },
        {
          MegaInput.selectCard3,
          (Key) 51L
        },
        {
          MegaInput.selectCard4,
          (Key) 52L
        },
        {
          MegaInput.selectCard5,
          (Key) 53L
        },
        {
          MegaInput.selectCard6,
          (Key) 54L
        },
        {
          MegaInput.selectCard7,
          (Key) 55L
        },
        {
          MegaInput.selectCard8,
          (Key) 56L
        },
        {
          MegaInput.selectCard9,
          (Key) 57L
        },
        {
          MegaInput.selectCard10,
          (Key) 48L /*0x30*/
        },
        {
          MegaInput.releaseCard,
          (Key) 4194322L
        }
      };
    }
  }

  public NControllerManager ControllerManager { get; private set; }

  public override void _EnterTree()
  {
    this.ControllerManager = this.GetNode<NControllerManager>(NodePath.op_Implicit("%ControllerManager"));
  }

  public override void _Ready()
  {
    ((GodotObject) this.ControllerManager).Connect(NControllerManager.SignalName.ControllerTypeChanged, Callable.From(new Action(this.OnControllerTypeChanged)), 0U);
    TaskHelper.RunSafely(this.Init());
  }

  private async Task Init()
  {
    await this.ControllerManager.Init();
    SettingsSave settingsSave = SaveManager.Instance.SettingsSave;
    if (settingsSave.KeyboardMapping.Count > 0)
    {
      this._keyboardInputMap = new Dictionary<StringName, Key>((IDictionary<StringName, Key>) NInputManager.DefaultKeyboardInputMap);
      foreach (KeyValuePair<string, string> keyValuePair in settingsSave.KeyboardMapping)
      {
        Key result;
        if (Enum.TryParse<Key>(keyValuePair.Value, out result))
          this._keyboardInputMap[StringName.op_Implicit(keyValuePair.Key)] = result;
      }
    }
    else
    {
      this._keyboardInputMap = NInputManager.DefaultKeyboardInputMap;
      this.SaveKeyboardInputMapping();
    }
    if (settingsSave.ControllerMapping.Count > 0 && settingsSave.ControllerMappingType == this.ControllerManager.ControllerMappingType)
    {
      this._controllerInputMap = NInputManager.MergeSavedControllerBindings(this.ControllerManager.GetDefaultControllerInputMap, settingsSave.ControllerMapping);
    }
    else
    {
      this._controllerInputMap = this.ControllerManager.GetDefaultControllerInputMap;
      this.SaveControllerInputMapping();
    }
  }

  public static Dictionary<StringName, StringName> MergeSavedControllerBindings(
    Dictionary<StringName, StringName> defaults,
    Dictionary<string, string> savedMapping)
  {
    Dictionary<StringName, StringName> dictionary = new Dictionary<StringName, StringName>((IDictionary<StringName, StringName>) defaults);
    foreach (KeyValuePair<string, string> keyValuePair in savedMapping)
    {
      if (InputMap.HasAction(StringName.op_Implicit(keyValuePair.Value)))
        dictionary[StringName.op_Implicit(keyValuePair.Key)] = StringName.op_Implicit(keyValuePair.Value);
    }
    return dictionary;
  }

  public override void _UnhandledKeyInput(InputEvent inputEvent)
  {
    this.ProcessShortcutKeyInput(inputEvent);
    this.ProcessDebugKeyInput(inputEvent);
  }

  private void ProcessDebugKeyInput(InputEvent inputEvent)
  {
    if (!(inputEvent is InputEventKey inputEventKey) || PlatformUtil.IsPlatformOverlayOpen() || !DisplayServer.WindowIsFocused(0) || NDevConsole.IsConsoleVisible || !NGame.IsTrailerMode)
      return;
    foreach (KeyValuePair<Key, StringName> debugInput in this._debugInputMap)
    {
      if (inputEventKey.Keycode == debugInput.Key)
        Input.ParseInputEvent((InputEvent) new InputEventAction()
        {
          Action = debugInput.Value,
          Pressed = inputEvent.IsPressed()
        });
    }
  }

  private void ProcessShortcutKeyInput(InputEvent inputEvent)
  {
    if (NGame.Instance.Transition.InTransition || !NGame.IsGameFocusedWindow() || !(inputEvent is InputEventKey inputEventKey))
      return;
    foreach (KeyValuePair<StringName, Key> keyboardInput in this._keyboardInputMap)
    {
      if (inputEventKey.Keycode == keyboardInput.Value && !inputEvent.IsEcho())
        Input.ParseInputEvent((InputEvent) new InputEventAction()
        {
          Action = keyboardInput.Key,
          Pressed = inputEvent.IsPressed()
        });
    }
  }

  public override void _UnhandledInput(InputEvent inputEvent)
  {
    if (NGame.Instance.Transition.InTransition || !NGame.IsGameFocusedWindow())
      return;
    foreach (KeyValuePair<StringName, StringName> controllerInput in this._controllerInputMap)
    {
      if (inputEvent.IsActionPressed(controllerInput.Value, false, false))
        Input.ParseInputEvent((InputEvent) new InputEventAction()
        {
          Action = controllerInput.Key,
          Pressed = true
        });
      else if (inputEvent.IsActionReleased(controllerInput.Value, false))
        Input.ParseInputEvent((InputEvent) new InputEventAction()
        {
          Action = controllerInput.Key,
          Pressed = false
        });
    }
  }

  public Key GetShortcutKey(StringName input)
  {
    Key key;
    return !this._keyboardInputMap.TryGetValue(input, out key) ? (Key) 0L : key;
  }

  public Texture2D? GetHotkeyIcon(string hotkey)
  {
    StringName stringName;
    return this._controllerInputMap.TryGetValue(StringName.op_Implicit(hotkey), out stringName) ? this.ControllerManager.GetHotkeyIcon(StringName.op_Implicit(stringName)) : (Texture2D) null;
  }

  public void ModifyShortcutKey(StringName input, Key shortcutKey)
  {
    KeyValuePair<StringName, Key> keyValuePair = this._keyboardInputMap.FirstOrDefault<KeyValuePair<StringName, Key>>((Func<KeyValuePair<StringName, Key>, bool>) (kvp => kvp.Value == shortcutKey && NInputManager.remappableKeyboardInputs.Contains<StringName>(kvp.Key)));
    if (StringName.op_Inequality(keyValuePair.Key, (StringName) null))
    {
      Key keyboardInput = this._keyboardInputMap[input];
      this._keyboardInputMap[keyValuePair.Key] = keyboardInput;
    }
    this._keyboardInputMap[input] = shortcutKey;
    this.SaveKeyboardInputMapping();
    this.EmitSignalInputRebound();
  }

  public void ModifyControllerButton(StringName input, StringName controllerInput)
  {
    KeyValuePair<StringName, StringName> keyValuePair = this._controllerInputMap.FirstOrDefault<KeyValuePair<StringName, StringName>>((Func<KeyValuePair<StringName, StringName>, bool>) (kvp => StringName.op_Equality(kvp.Value, controllerInput) && NInputManager.remappableControllerInputs.Contains<StringName>(kvp.Key)));
    if (StringName.op_Inequality(keyValuePair.Key, (StringName) null))
    {
      StringName controllerInput1 = this._controllerInputMap[input];
      this._controllerInputMap[keyValuePair.Key] = controllerInput1;
    }
    this._controllerInputMap[input] = controllerInput;
    this.SaveControllerInputMapping();
    this.EmitSignalInputRebound();
  }

  public void ResetToDefaults()
  {
    this._keyboardInputMap = NInputManager.DefaultKeyboardInputMap;
    this._controllerInputMap = this.ControllerManager.GetDefaultControllerInputMap;
    this.SaveControllerInputMapping();
    this.SaveKeyboardInputMapping();
    this.EmitSignalInputRebound();
  }

  private void OnControllerTypeChanged()
  {
    if (this.ControllerManager.ControllerMappingType == SaveManager.Instance.SettingsSave.ControllerMappingType)
      return;
    this._controllerInputMap = this.ControllerManager.GetDefaultControllerInputMap;
    this.SaveControllerInputMapping();
    this.EmitSignalInputRebound();
  }

  private void SaveControllerInputMapping()
  {
    Dictionary<string, string> dictionary = new Dictionary<string, string>();
    foreach (KeyValuePair<StringName, StringName> controllerInput in this._controllerInputMap)
      dictionary.Add(controllerInput.Key.ToString(), controllerInput.Value.ToString());
    SaveManager.Instance.SettingsSave.ControllerMappingType = this.ControllerManager.ControllerMappingType;
    SaveManager.Instance.SettingsSave.ControllerMapping = dictionary;
    SaveManager.Instance.SaveSettings();
  }

  private void SaveKeyboardInputMapping()
  {
    Dictionary<string, string> dictionary = new Dictionary<string, string>();
    foreach (KeyValuePair<StringName, Key> keyboardInput in this._keyboardInputMap)
      dictionary.Add(keyboardInput.Key.ToString(), keyboardInput.Value.ToString());
    SaveManager.Instance.SettingsSave.KeyboardMapping = dictionary;
    SaveManager.Instance.SaveSettings();
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(14)
    {
      new MethodInfo(NInputManager.MethodName._EnterTree, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NInputManager.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NInputManager.MethodName._UnhandledKeyInput, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("inputEvent"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("InputEvent"), false)
      }, (List<Variant>) null),
      new MethodInfo(NInputManager.MethodName.ProcessDebugKeyInput, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("inputEvent"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("InputEvent"), false)
      }, (List<Variant>) null),
      new MethodInfo(NInputManager.MethodName.ProcessShortcutKeyInput, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("inputEvent"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("InputEvent"), false)
      }, (List<Variant>) null),
      new MethodInfo(NInputManager.MethodName._UnhandledInput, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("inputEvent"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("InputEvent"), false)
      }, (List<Variant>) null),
      new MethodInfo(NInputManager.MethodName.GetShortcutKey, new PropertyInfo((Variant.Type) 2L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 21L, StringName.op_Implicit("input"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NInputManager.MethodName.GetHotkeyIcon, new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Texture2D"), false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 4L, StringName.op_Implicit("hotkey"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NInputManager.MethodName.ModifyShortcutKey, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 21L, StringName.op_Implicit("input"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false),
        new PropertyInfo((Variant.Type) 2L, StringName.op_Implicit("shortcutKey"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NInputManager.MethodName.ModifyControllerButton, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 21L, StringName.op_Implicit("input"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false),
        new PropertyInfo((Variant.Type) 21L, StringName.op_Implicit("controllerInput"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NInputManager.MethodName.ResetToDefaults, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NInputManager.MethodName.OnControllerTypeChanged, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NInputManager.MethodName.SaveControllerInputMapping, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NInputManager.MethodName.SaveKeyboardInputMapping, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NInputManager.MethodName._EnterTree) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      base._EnterTree();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NInputManager.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      base._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NInputManager.MethodName._UnhandledKeyInput) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      base._UnhandledKeyInput(VariantUtils.ConvertTo<InputEvent>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NInputManager.MethodName.ProcessDebugKeyInput) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.ProcessDebugKeyInput(VariantUtils.ConvertTo<InputEvent>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NInputManager.MethodName.ProcessShortcutKeyInput) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.ProcessShortcutKeyInput(VariantUtils.ConvertTo<InputEvent>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NInputManager.MethodName._UnhandledInput) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      base._UnhandledInput(VariantUtils.ConvertTo<InputEvent>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NInputManager.MethodName.GetShortcutKey) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      Key shortcutKey = this.GetShortcutKey(VariantUtils.ConvertTo<StringName>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = VariantUtils.CreateFrom<Key>(ref shortcutKey);
      return true;
    }
    if (StringName.op_Equality(ref method, NInputManager.MethodName.GetHotkeyIcon) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      Texture2D hotkeyIcon = this.GetHotkeyIcon(VariantUtils.ConvertTo<string>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = VariantUtils.CreateFrom<Texture2D>(ref hotkeyIcon);
      return true;
    }
    if (StringName.op_Equality(ref method, NInputManager.MethodName.ModifyShortcutKey) && ((NativeVariantPtrArgs) ref args).Count == 2)
    {
      this.ModifyShortcutKey(VariantUtils.ConvertTo<StringName>(ref ((NativeVariantPtrArgs) ref args)[0]), VariantUtils.ConvertTo<Key>(ref ((NativeVariantPtrArgs) ref args)[1]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NInputManager.MethodName.ModifyControllerButton) && ((NativeVariantPtrArgs) ref args).Count == 2)
    {
      this.ModifyControllerButton(VariantUtils.ConvertTo<StringName>(ref ((NativeVariantPtrArgs) ref args)[0]), VariantUtils.ConvertTo<StringName>(ref ((NativeVariantPtrArgs) ref args)[1]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NInputManager.MethodName.ResetToDefaults) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.ResetToDefaults();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NInputManager.MethodName.OnControllerTypeChanged) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnControllerTypeChanged();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NInputManager.MethodName.SaveControllerInputMapping) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.SaveControllerInputMapping();
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NInputManager.MethodName.SaveKeyboardInputMapping) || ((NativeVariantPtrArgs) ref args).Count != 0)
      return base.InvokeGodotClassMethod(ref method, args, ref ret);
    this.SaveKeyboardInputMapping();
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NInputManager.MethodName._EnterTree) || StringName.op_Equality(ref method, NInputManager.MethodName._Ready) || StringName.op_Equality(ref method, NInputManager.MethodName._UnhandledKeyInput) || StringName.op_Equality(ref method, NInputManager.MethodName.ProcessDebugKeyInput) || StringName.op_Equality(ref method, NInputManager.MethodName.ProcessShortcutKeyInput) || StringName.op_Equality(ref method, NInputManager.MethodName._UnhandledInput) || StringName.op_Equality(ref method, NInputManager.MethodName.GetShortcutKey) || StringName.op_Equality(ref method, NInputManager.MethodName.GetHotkeyIcon) || StringName.op_Equality(ref method, NInputManager.MethodName.ModifyShortcutKey) || StringName.op_Equality(ref method, NInputManager.MethodName.ModifyControllerButton) || StringName.op_Equality(ref method, NInputManager.MethodName.ResetToDefaults) || StringName.op_Equality(ref method, NInputManager.MethodName.OnControllerTypeChanged) || StringName.op_Equality(ref method, NInputManager.MethodName.SaveControllerInputMapping) || StringName.op_Equality(ref method, NInputManager.MethodName.SaveKeyboardInputMapping) || base.HasGodotClassMethod(ref method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (!StringName.op_Equality(ref name, NInputManager.PropertyName.ControllerManager))
      return ((GodotObject) this).SetGodotClassPropertyValue(ref name, ref value);
    this.ControllerManager = VariantUtils.ConvertTo<NControllerManager>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (!StringName.op_Equality(ref name, NInputManager.PropertyName.ControllerManager))
      return ((GodotObject) this).GetGodotClassPropertyValue(ref name, ref value);
    ref godot_variant local = ref value;
    NControllerManager controllerManager = this.ControllerManager;
    godot_variant from = VariantUtils.CreateFrom<NControllerManager>(ref controllerManager);
    local = from;
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NInputManager.PropertyName.ControllerManager, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).SaveGodotObjectData(info);
    GodotSerializationInfo serializationInfo = info;
    StringName controllerManager1 = NInputManager.PropertyName.ControllerManager;
    NControllerManager controllerManager2 = this.ControllerManager;
    Variant variant = Variant.From<NControllerManager>(ref controllerManager2);
    serializationInfo.AddProperty(controllerManager1, variant);
    info.AddSignalEventDelegate(NInputManager.SignalName.InputRebound, (Delegate) this.backing_InputRebound);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).RestoreGodotObjectData(info);
    Variant variant;
    if (info.TryGetProperty(NInputManager.PropertyName.ControllerManager, ref variant))
      this.ControllerManager = ((Variant) ref variant).As<NControllerManager>();
    NInputManager.InputReboundEventHandler reboundEventHandler;
    if (!info.TryGetSignalEventDelegate<NInputManager.InputReboundEventHandler>(NInputManager.SignalName.InputRebound, ref reboundEventHandler))
      return;
    this.backing_InputRebound = reboundEventHandler;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<MethodInfo> GetGodotSignalList()
  {
    return new List<MethodInfo>(1)
    {
      new MethodInfo(NInputManager.SignalName.InputRebound, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  public event NInputManager.InputReboundEventHandler InputRebound
  {
    add => this.backing_InputRebound += value;
    remove => this.backing_InputRebound -= value;
  }

  protected void EmitSignalInputRebound()
  {
    ((GodotObject) this).EmitSignal(NInputManager.SignalName.InputRebound, Array.Empty<Variant>());
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RaiseGodotClassSignalCallbacks(
    in godot_string_name signal,
    NativeVariantPtrArgs args)
  {
    if (StringName.op_Equality(ref signal, NInputManager.SignalName.InputRebound) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      NInputManager.InputReboundEventHandler backingInputRebound = this.backing_InputRebound;
      if (backingInputRebound == null)
        return;
      backingInputRebound();
    }
    else
      ((GodotObject) this).RaiseGodotClassSignalCallbacks(ref signal, args);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassSignal(in godot_string_name signal)
  {
    return StringName.op_Equality(ref signal, NInputManager.SignalName.InputRebound) || base.HasGodotClassSignal(ref signal);
  }

  [Signal]
  public delegate void InputReboundEventHandler();

  public class MethodName : Node.MethodName
  {
    public static readonly StringName _EnterTree = StringName.op_Implicit(nameof (_EnterTree));
    public static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName _UnhandledKeyInput = StringName.op_Implicit(nameof (_UnhandledKeyInput));
    public static readonly StringName ProcessDebugKeyInput = StringName.op_Implicit(nameof (ProcessDebugKeyInput));
    public static readonly StringName ProcessShortcutKeyInput = StringName.op_Implicit(nameof (ProcessShortcutKeyInput));
    public static readonly StringName _UnhandledInput = StringName.op_Implicit(nameof (_UnhandledInput));
    public static readonly StringName GetShortcutKey = StringName.op_Implicit(nameof (GetShortcutKey));
    public static readonly StringName GetHotkeyIcon = StringName.op_Implicit(nameof (GetHotkeyIcon));
    public static readonly StringName ModifyShortcutKey = StringName.op_Implicit(nameof (ModifyShortcutKey));
    public static readonly StringName ModifyControllerButton = StringName.op_Implicit(nameof (ModifyControllerButton));
    public static readonly StringName ResetToDefaults = StringName.op_Implicit(nameof (ResetToDefaults));
    public static readonly StringName OnControllerTypeChanged = StringName.op_Implicit(nameof (OnControllerTypeChanged));
    public static readonly StringName SaveControllerInputMapping = StringName.op_Implicit(nameof (SaveControllerInputMapping));
    public static readonly StringName SaveKeyboardInputMapping = StringName.op_Implicit(nameof (SaveKeyboardInputMapping));
  }

  public class PropertyName : Node.PropertyName
  {
    public static readonly StringName ControllerManager = StringName.op_Implicit(nameof (ControllerManager));
  }

  public class SignalName : Node.SignalName
  {
    public static readonly StringName InputRebound = StringName.op_Implicit(nameof (InputRebound));
  }
}
