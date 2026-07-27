// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.ControllerInput.SteamControllerInputStrategy
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using MegaCrit.Sts2.Core.ControllerInput.ControllerConfigs;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Platform.Steam;
using Steamworks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.ControllerInput;

public class SteamControllerInputStrategy : IControllerInputStrategy
{
  private readonly Dictionary<EInputActionOrigin, StringName> _steamInputsToMegaInputs = new Dictionary<EInputActionOrigin, StringName>()
  {
    {
      (EInputActionOrigin) 1,
      Controller.faceButtonSouth
    },
    {
      (EInputActionOrigin) 2,
      Controller.faceButtonEast
    },
    {
      (EInputActionOrigin) 3,
      Controller.faceButtonWest
    },
    {
      (EInputActionOrigin) 4,
      Controller.faceButtonNorth
    },
    {
      (EInputActionOrigin) 5,
      Controller.leftBumper
    },
    {
      (EInputActionOrigin) 6,
      Controller.rightBumper
    },
    {
      (EInputActionOrigin) 25,
      Controller.leftTrigger
    },
    {
      (EInputActionOrigin) 26,
      Controller.leftTrigger
    },
    {
      (EInputActionOrigin) 27,
      Controller.rightTrigger
    },
    {
      (EInputActionOrigin) 28,
      Controller.rightTrigger
    },
    {
      (EInputActionOrigin) 31 /*0x1F*/,
      Controller.lStickUp
    },
    {
      (EInputActionOrigin) 32 /*0x20*/,
      Controller.lStickDown
    },
    {
      (EInputActionOrigin) 33,
      Controller.lStickLeft
    },
    {
      (EInputActionOrigin) 34,
      Controller.lStickRight
    },
    {
      (EInputActionOrigin) 30,
      Controller.lStickPress
    },
    {
      (EInputActionOrigin) 9,
      Controller.startButton
    },
    {
      (EInputActionOrigin) 10,
      Controller.selectButton
    },
    {
      (EInputActionOrigin) 50,
      Controller.faceButtonSouth
    },
    {
      (EInputActionOrigin) 51,
      Controller.faceButtonEast
    },
    {
      (EInputActionOrigin) 53,
      Controller.faceButtonWest
    },
    {
      (EInputActionOrigin) 52,
      Controller.faceButtonNorth
    },
    {
      (EInputActionOrigin) 54,
      Controller.leftBumper
    },
    {
      (EInputActionOrigin) 55,
      Controller.rightBumper
    },
    {
      (EInputActionOrigin) 79,
      Controller.leftTrigger
    },
    {
      (EInputActionOrigin) 80 /*0x50*/,
      Controller.leftTrigger
    },
    {
      (EInputActionOrigin) 81,
      Controller.rightTrigger
    },
    {
      (EInputActionOrigin) 82,
      Controller.rightTrigger
    },
    {
      (EInputActionOrigin) 95,
      Controller.dPadUp
    },
    {
      (EInputActionOrigin) 96 /*0x60*/,
      Controller.dPadDown
    },
    {
      (EInputActionOrigin) 97,
      Controller.dPadLeft
    },
    {
      (EInputActionOrigin) 98,
      Controller.dPadRight
    },
    {
      (EInputActionOrigin) 85,
      Controller.lStickUp
    },
    {
      (EInputActionOrigin) 86,
      Controller.lStickDown
    },
    {
      (EInputActionOrigin) 87,
      Controller.lStickLeft
    },
    {
      (EInputActionOrigin) 88,
      Controller.lStickRight
    },
    {
      (EInputActionOrigin) 84,
      Controller.lStickPress
    },
    {
      (EInputActionOrigin) 56,
      Controller.startButton
    },
    {
      (EInputActionOrigin) 57,
      Controller.selectButton
    },
    {
      (EInputActionOrigin) 74,
      Controller.ps4Touchpad
    },
    {
      (EInputActionOrigin) 67,
      Controller.ps4Touchpad
    },
    {
      (EInputActionOrigin) 60,
      Controller.ps4Touchpad
    },
    {
      (EInputActionOrigin) 258,
      Controller.faceButtonSouth
    },
    {
      (EInputActionOrigin) 259,
      Controller.faceButtonEast
    },
    {
      (EInputActionOrigin) 261,
      Controller.faceButtonWest
    },
    {
      (EInputActionOrigin) 260,
      Controller.faceButtonNorth
    },
    {
      (EInputActionOrigin) 262,
      Controller.leftBumper
    },
    {
      (EInputActionOrigin) 263,
      Controller.rightBumper
    },
    {
      (EInputActionOrigin) 288,
      Controller.leftTrigger
    },
    {
      (EInputActionOrigin) 289,
      Controller.leftTrigger
    },
    {
      (EInputActionOrigin) 290,
      Controller.rightTrigger
    },
    {
      (EInputActionOrigin) 291,
      Controller.rightTrigger
    },
    {
      (EInputActionOrigin) 304,
      Controller.dPadUp
    },
    {
      (EInputActionOrigin) 305,
      Controller.dPadDown
    },
    {
      (EInputActionOrigin) 306,
      Controller.dPadLeft
    },
    {
      (EInputActionOrigin) 307,
      Controller.dPadRight
    },
    {
      (EInputActionOrigin) 294,
      Controller.lStickUp
    },
    {
      (EInputActionOrigin) 295,
      Controller.lStickDown
    },
    {
      (EInputActionOrigin) 296,
      Controller.lStickLeft
    },
    {
      (EInputActionOrigin) 297,
      Controller.lStickRight
    },
    {
      (EInputActionOrigin) 293,
      Controller.lStickPress
    },
    {
      (EInputActionOrigin) 264,
      Controller.startButton
    },
    {
      (EInputActionOrigin) 265,
      Controller.selectButton
    },
    {
      (EInputActionOrigin) 283,
      Controller.ps4Touchpad
    },
    {
      (EInputActionOrigin) 276,
      Controller.ps4Touchpad
    },
    {
      (EInputActionOrigin) 269,
      Controller.ps4Touchpad
    },
    {
      (EInputActionOrigin) 114,
      Controller.faceButtonSouth
    },
    {
      (EInputActionOrigin) 115,
      Controller.faceButtonEast
    },
    {
      (EInputActionOrigin) 116,
      Controller.faceButtonWest
    },
    {
      (EInputActionOrigin) 117,
      Controller.faceButtonNorth
    },
    {
      (EInputActionOrigin) 118,
      Controller.leftBumper
    },
    {
      (EInputActionOrigin) 119,
      Controller.rightBumper
    },
    {
      (EInputActionOrigin) 122,
      Controller.leftTrigger
    },
    {
      (EInputActionOrigin) 123,
      Controller.leftTrigger
    },
    {
      (EInputActionOrigin) 124,
      Controller.rightTrigger
    },
    {
      (EInputActionOrigin) 125,
      Controller.rightTrigger
    },
    {
      (EInputActionOrigin) 138,
      Controller.dPadUp
    },
    {
      (EInputActionOrigin) 139,
      Controller.dPadDown
    },
    {
      (EInputActionOrigin) 140,
      Controller.dPadLeft
    },
    {
      (EInputActionOrigin) 141,
      Controller.dPadRight
    },
    {
      (EInputActionOrigin) 128 /*0x80*/,
      Controller.lStickUp
    },
    {
      (EInputActionOrigin) 129,
      Controller.lStickDown
    },
    {
      (EInputActionOrigin) 130,
      Controller.lStickLeft
    },
    {
      (EInputActionOrigin) 131,
      Controller.lStickRight
    },
    {
      (EInputActionOrigin) (int) sbyte.MaxValue,
      Controller.lStickPress
    },
    {
      (EInputActionOrigin) 120,
      Controller.startButton
    },
    {
      (EInputActionOrigin) 121,
      Controller.selectButton
    },
    {
      (EInputActionOrigin) 153,
      Controller.faceButtonSouth
    },
    {
      (EInputActionOrigin) 154,
      Controller.faceButtonEast
    },
    {
      (EInputActionOrigin) 155,
      Controller.faceButtonWest
    },
    {
      (EInputActionOrigin) 156,
      Controller.faceButtonNorth
    },
    {
      (EInputActionOrigin) 157,
      Controller.leftBumper
    },
    {
      (EInputActionOrigin) 158,
      Controller.rightBumper
    },
    {
      (EInputActionOrigin) 161,
      Controller.leftTrigger
    },
    {
      (EInputActionOrigin) 162,
      Controller.leftTrigger
    },
    {
      (EInputActionOrigin) 163,
      Controller.rightTrigger
    },
    {
      (EInputActionOrigin) 164,
      Controller.rightTrigger
    },
    {
      (EInputActionOrigin) 177,
      Controller.dPadUp
    },
    {
      (EInputActionOrigin) 178,
      Controller.dPadDown
    },
    {
      (EInputActionOrigin) 179,
      Controller.dPadLeft
    },
    {
      (EInputActionOrigin) 180,
      Controller.dPadRight
    },
    {
      (EInputActionOrigin) 159,
      Controller.startButton
    },
    {
      (EInputActionOrigin) 160 /*0xA0*/,
      Controller.selectButton
    },
    {
      (EInputActionOrigin) 192 /*0xC0*/,
      Controller.faceButtonEast
    },
    {
      (EInputActionOrigin) 193,
      Controller.faceButtonSouth
    },
    {
      (EInputActionOrigin) 194,
      Controller.faceButtonNorth
    },
    {
      (EInputActionOrigin) 195,
      Controller.faceButtonWest
    },
    {
      (EInputActionOrigin) 196,
      Controller.leftBumper
    },
    {
      (EInputActionOrigin) 197,
      Controller.rightBumper
    },
    {
      (EInputActionOrigin) 217,
      Controller.dPadUp
    },
    {
      (EInputActionOrigin) 218,
      Controller.dPadDown
    },
    {
      (EInputActionOrigin) 219,
      Controller.dPadLeft
    },
    {
      (EInputActionOrigin) 220,
      Controller.dPadRight
    },
    {
      (EInputActionOrigin) 198,
      Controller.startButton
    },
    {
      (EInputActionOrigin) 199,
      Controller.selectButton
    },
    {
      (EInputActionOrigin) 333,
      Controller.faceButtonSouth
    },
    {
      (EInputActionOrigin) 334,
      Controller.faceButtonEast
    },
    {
      (EInputActionOrigin) 335,
      Controller.faceButtonWest
    },
    {
      (EInputActionOrigin) 336,
      Controller.faceButtonNorth
    },
    {
      (EInputActionOrigin) 337,
      Controller.leftBumper
    },
    {
      (EInputActionOrigin) 338,
      Controller.rightBumper
    },
    {
      (EInputActionOrigin) 356,
      Controller.leftTrigger
    },
    {
      (EInputActionOrigin) 358,
      Controller.rightTrigger
    },
    {
      (EInputActionOrigin) 360,
      Controller.lStickPress
    },
    {
      (EInputActionOrigin) 367,
      Controller.lStickPress
    },
    {
      (EInputActionOrigin) 378,
      Controller.dPadUp
    },
    {
      (EInputActionOrigin) 379,
      Controller.dPadDown
    },
    {
      (EInputActionOrigin) 380,
      Controller.dPadLeft
    },
    {
      (EInputActionOrigin) 381,
      Controller.dPadRight
    },
    {
      (EInputActionOrigin) 339,
      Controller.startButton
    },
    {
      (EInputActionOrigin) 340,
      Controller.selectButton
    }
  };
  private InputHandle_t? _currentControllerHandle;
  private InputActionSetHandle_t? _currentActionSetHandle;
  private ESteamInputType? _currentInputType;
  private ControllerConfig? _controllerConfig;
  private readonly List<string> _pressedInputs = new List<string>();
  private readonly Dictionary<StringName, InputEventAction> _inputEvents = new Dictionary<StringName, InputEventAction>();
  private readonly IControllerInputStrategy _fallbackStrategy = (IControllerInputStrategy) new GodotControllerInputStrategy();
  private double _nextControllerCheckTime;
  private readonly Dictionary<StringName, InputDigitalActionHandle_t> _digitalActionHandleCache = new Dictionary<StringName, InputDigitalActionHandle_t>();
  private bool _attemptedHandleCacheRebuild;
  private Dictionary<EInputActionOrigin, Texture2D> _fallbackSteamGlyphs = new Dictionary<EInputActionOrigin, Texture2D>();
  private InputAnalogActionHandle_t _joystickActionHandle;
  private Vector2 _lStickPosition;
  private InputEventJoypadMotion _joystickXAxis;
  private InputEventJoypadMotion _joystickYAxis;
  private StringName _up;
  private StringName _down;
  private StringName _left;
  private StringName _right;

  public ControllerConfig? ControllerConfig
  {
    get => this._controllerConfig ?? this._fallbackStrategy.ControllerConfig;
  }

  public async Task Init()
  {
    double num = (double) await NControllerManager.Instance.AwaitProcessFrame();
    if (SteamInitializer.Initialized)
    {
      try
      {
        SteamInput.Init(false);
        this.UpdateControllerConnections();
        InputEventJoypadMotion eventJoypadMotion1 = new InputEventJoypadMotion();
        eventJoypadMotion1.Axis = (JoyAxis) 0L;
        ((InputEvent) eventJoypadMotion1).Device = 0;
        this._joystickXAxis = eventJoypadMotion1;
        InputEventJoypadMotion eventJoypadMotion2 = new InputEventJoypadMotion();
        eventJoypadMotion2.Axis = (JoyAxis) 1L;
        ((InputEvent) eventJoypadMotion2).Device = 0;
        this._joystickYAxis = eventJoypadMotion2;
        this._up = new StringName("Up");
        this._down = new StringName("Down");
        this._left = new StringName("Left");
        this._right = new StringName("Right");
        this._joystickActionHandle = SteamInput.GetAnalogActionHandle("Joystick");
      }
      catch (InvalidOperationException ex)
      {
        Log.Error("Failed to initialize Steam Input: " + ex.Message);
      }
    }
    else
      Log.Warn("Cannot initialize Steam Input because Steamworks is not initialized. Falling back to standard input.");
    await this._fallbackStrategy.Init();
  }

  public void ProcessInput()
  {
    if (!SteamInitializer.Initialized)
    {
      this._fallbackStrategy.ProcessInput();
    }
    else
    {
      double num = (double) Time.GetTicksMsec() / 1000.0;
      if (num >= this._nextControllerCheckTime)
      {
        this._nextControllerCheckTime = num + 1.0;
        this.UpdateControllerConnections();
      }
      if (!this._currentControllerHandle.HasValue)
      {
        this._fallbackStrategy.ProcessInput();
      }
      else
      {
        try
        {
          SteamInput.RunFrame(true);
          this.ProcessDigitalInputs();
          this.ProcessAnalogInputs();
        }
        catch (InvalidOperationException ex)
        {
          Log.Error("Error running Steam Input frame: " + ex.Message);
          this._currentControllerHandle = new InputHandle_t?();
          this._fallbackStrategy.ProcessInput();
        }
      }
    }
  }

  private void ProcessDigitalInputs()
  {
    if (this._controllerConfig == null)
      return;
    if (this._digitalActionHandleCache.Count == 0 && !this._attemptedHandleCacheRebuild)
    {
      this._attemptedHandleCacheRebuild = true;
      this.UpdateInputMap();
    }
    foreach (KeyValuePair<string, StringName> steamInputController in this._controllerConfig.SteamInputControllerMap)
    {
      InputDigitalActionHandle_t digitalActionHandleT;
      if (!this._digitalActionHandleCache.TryGetValue(StringName.op_Implicit(steamInputController.Key), out digitalActionHandleT))
      {
        Log.Error($"The input {steamInputController.Key} was not cached during initialization. Skipping...");
      }
      else
      {
        bool flag1 = SteamInput.GetDigitalActionData(this._currentControllerHandle.Value, digitalActionHandleT).bState == (byte) 1;
        bool flag2 = this._pressedInputs.Contains(steamInputController.Key);
        if (flag1 && !flag2)
          this._pressedInputs.Add(steamInputController.Key);
        else if (!flag1 & flag2)
          this._pressedInputs.Remove(steamInputController.Key);
        if (flag1 && !flag2)
        {
          InputEventAction inputEvent = this._inputEvents[StringName.op_Implicit(steamInputController.Key)];
          inputEvent.Pressed = true;
          Input.ParseInputEvent((InputEvent) inputEvent);
        }
        else if (!flag1 & flag2)
        {
          InputEventAction inputEvent = this._inputEvents[StringName.op_Implicit(steamInputController.Key)];
          inputEvent.Pressed = false;
          Input.ParseInputEvent((InputEvent) inputEvent);
        }
      }
    }
  }

  private void ProcessAnalogInputs()
  {
    InputAnalogActionData_t analogActionData = SteamInput.GetAnalogActionData(this._currentControllerHandle.Value, this._joystickActionHandle);
    Vector2 vector2;
    // ISSUE: explicit constructor call
    ((Vector2) ref vector2).\u002Ector(analogActionData.x, analogActionData.y);
    if ((double) ((Vector2) ref vector2).DistanceTo(this._lStickPosition) > 0.05000000074505806)
    {
      InputEventJoypadMotion joystickXaxis = this._joystickXAxis;
      joystickXaxis.AxisValue = vector2.X;
      InputEventJoypadMotion joystickYaxis = this._joystickYAxis;
      joystickYaxis.AxisValue = -vector2.Y;
      Input.ParseInputEvent((InputEvent) joystickXaxis);
      Input.ParseInputEvent((InputEvent) joystickYaxis);
    }
    if ((double) vector2.Y >= 0.5 && !this._pressedInputs.Contains("Joy_Up"))
    {
      InputEventAction inputEvent = this._inputEvents[this._up];
      inputEvent.Pressed = true;
      Input.ParseInputEvent((InputEvent) inputEvent);
      this._pressedInputs.Add("Joy_Up");
    }
    else if ((double) vector2.Y < 0.5 && this._pressedInputs.Contains("Joy_Up"))
    {
      InputEventAction inputEvent = this._inputEvents[this._up];
      inputEvent.Pressed = false;
      Input.ParseInputEvent((InputEvent) inputEvent);
      this._pressedInputs.Remove("Joy_Up");
    }
    if ((double) vector2.Y <= -0.5 && !this._pressedInputs.Contains("Joy_Down"))
    {
      InputEventAction inputEvent = this._inputEvents[this._down];
      inputEvent.Pressed = true;
      Input.ParseInputEvent((InputEvent) inputEvent);
      this._pressedInputs.Add("Joy_Down");
    }
    else if ((double) vector2.Y > -0.5 && this._pressedInputs.Contains("Joy_Down"))
    {
      InputEventAction inputEvent = this._inputEvents[this._down];
      inputEvent.Pressed = false;
      Input.ParseInputEvent((InputEvent) inputEvent);
      this._pressedInputs.Remove("Joy_Down");
    }
    if ((double) vector2.X <= -0.5 && !this._pressedInputs.Contains("Joy_Left"))
    {
      InputEventAction inputEvent = this._inputEvents[this._left];
      inputEvent.Pressed = true;
      Input.ParseInputEvent((InputEvent) inputEvent);
      this._pressedInputs.Add("Joy_Left");
    }
    else if ((double) vector2.X > -0.5 && this._pressedInputs.Contains("Joy_Left"))
    {
      InputEventAction inputEvent = this._inputEvents[this._left];
      inputEvent.Pressed = false;
      Input.ParseInputEvent((InputEvent) inputEvent);
      this._pressedInputs.Remove("Joy_Left");
    }
    if ((double) vector2.X >= 0.5 && !this._pressedInputs.Contains("Joy_Right"))
    {
      InputEventAction inputEvent = this._inputEvents[this._right];
      inputEvent.Pressed = true;
      Input.ParseInputEvent((InputEvent) inputEvent);
      this._pressedInputs.Add("Joy_Right");
    }
    else if ((double) vector2.X < 0.5 && this._pressedInputs.Contains("Joy_Right"))
    {
      InputEventAction inputEvent = this._inputEvents[this._right];
      inputEvent.Pressed = false;
      Input.ParseInputEvent((InputEvent) inputEvent);
      this._pressedInputs.Remove("Joy_Right");
    }
    this._lStickPosition = vector2;
  }

  private void UpdateControllerConnections()
  {
    try
    {
      InputHandle_t[] inputHandleTArray = new InputHandle_t[16 /*0x10*/];
      if (SteamInput.GetConnectedControllers(inputHandleTArray) == 0)
      {
        this._currentControllerHandle = new InputHandle_t?();
        this._currentInputType = new ESteamInputType?();
        if (this._controllerConfig != null)
          return;
        this.UpdateControllerConfig((ESteamInputType) 14);
      }
      else
      {
        this._currentControllerHandle = new InputHandle_t?(inputHandleTArray[0]);
        ESteamInputType inputTypeForHandle = SteamInput.GetInputTypeForHandle(this._currentControllerHandle.Value);
        ESteamInputType? currentInputType = this._currentInputType;
        ESteamInputType esteamInputType = inputTypeForHandle;
        if (!(currentInputType.GetValueOrDefault() == esteamInputType & currentInputType.HasValue) || this._controllerConfig == null)
        {
          this._currentInputType = new ESteamInputType?(inputTypeForHandle);
          this._attemptedHandleCacheRebuild = false;
          this.UpdateControllerConfig(inputTypeForHandle);
          this.UpdateInputMap();
        }
        this._currentActionSetHandle = new InputActionSetHandle_t?(SteamInput.GetActionSetHandle("Controls"));
        SteamInput.ActivateActionSet(this._currentControllerHandle.Value, this._currentActionSetHandle.Value);
      }
    }
    catch (InvalidOperationException ex)
    {
      Log.Error("Failed to connect to Steam controller: " + ex.Message);
      this._currentControllerHandle = new InputHandle_t?();
    }
  }

  private void UpdateControllerConfig(ESteamInputType controllerType)
  {
    ControllerConfig controllerConfig = this._controllerConfig;
    switch (controllerType - 1)
    {
      case 1:
        this._controllerConfig = (ControllerConfig) new Xbox360Config();
        break;
      case 2:
        this._controllerConfig = (ControllerConfig) new XboxOneConfig();
        break;
      case 4:
      case 11:
      case 12:
        this._controllerConfig = (ControllerConfig) new Ps4Config();
        break;
      case 7:
      case 8:
      case 9:
        this._controllerConfig = (ControllerConfig) new SwitchConfig();
        break;
      default:
        this._controllerConfig = (ControllerConfig) new SteamControllerConfig();
        break;
    }
    if (controllerConfig == null || controllerConfig.ControllerMappingType == this._controllerConfig.ControllerMappingType)
      return;
    NControllerManager.Instance?.OnControllerTypeChanged();
  }

  public string GetControllerName()
  {
    return this._currentInputType.HasValue ? this._currentInputType.Value.ToString() : this._fallbackStrategy.GetControllerName();
  }

  private void UpdateInputMap()
  {
    if (this._controllerConfig == null)
      return;
    this._inputEvents.Clear();
    this._digitalActionHandleCache.Clear();
    foreach (KeyValuePair<string, StringName> steamInputController in this._controllerConfig.SteamInputControllerMap)
      this._inputEvents[StringName.op_Implicit(steamInputController.Key)] = new InputEventAction()
      {
        Action = steamInputController.Value
      };
    foreach (string key1 in this._controllerConfig.SteamInputControllerMap.Keys)
    {
      StringName key2 = StringName.op_Implicit(key1);
      try
      {
        InputDigitalActionHandle_t digitalActionHandle = SteamInput.GetDigitalActionHandle(StringName.op_Implicit(key2));
        this._digitalActionHandleCache[key2] = digitalActionHandle;
      }
      catch (InvalidOperationException ex)
      {
        Log.Error($"Failed to cache digital action handle for {key2}: {ex.Message}");
      }
    }
  }

  public Texture2D? GetHotkeyIcon(string hotkey)
  {
    if (!SteamInitializer.Initialized || !this._currentControllerHandle.HasValue)
      return this._fallbackStrategy.GetHotkeyIcon(hotkey);
    string key1 = (this.ControllerConfig != null ? (IEnumerable<KeyValuePair<string, StringName>>) this.ControllerConfig.SteamInputControllerMap : (IEnumerable<KeyValuePair<string, StringName>>) new SteamControllerConfig().SteamInputControllerMap).FirstOrDefault<KeyValuePair<string, StringName>>((Func<KeyValuePair<string, StringName>, bool>) (kvp => StringName.op_Equality(kvp.Value, StringName.op_Implicit(hotkey)))).Key;
    if (key1 == null)
      return this.ControllerConfig?.GetButtonIcon(hotkey);
    EInputActionOrigin[] einputActionOriginArray = new EInputActionOrigin[8];
    InputDigitalActionHandle_t digitalActionHandleT;
    if (!this._digitalActionHandleCache.TryGetValue(StringName.op_Implicit(key1), out digitalActionHandleT))
    {
      Log.Error($"The input {key1} was not cached during initialization.");
      return this.ControllerConfig?.GetButtonIcon(key1);
    }
    SteamInput.GetDigitalActionOrigins(this._currentControllerHandle.Value, this._currentActionSetHandle.Value, digitalActionHandleT, einputActionOriginArray);
    if (einputActionOriginArray.Length != 0 && (int) einputActionOriginArray[0] != 0)
    {
      EInputActionOrigin key2 = SteamInput.TranslateActionOrigin(SteamInput.GetInputTypeForHandle(this._currentControllerHandle.Value), (EInputActionOrigin) (int) einputActionOriginArray[0]);
      StringName stringName;
      if (this._steamInputsToMegaInputs.TryGetValue(key2, out stringName))
        return this.ControllerConfig?.GetButtonIcon(StringName.op_Implicit(stringName));
      if (!this._fallbackSteamGlyphs.ContainsKey(key2))
      {
        Image image = Image.LoadFromFile(SteamInput.GetGlyphSVGForActionOrigin(key2, 0U));
        this._fallbackSteamGlyphs.Add(key2, (Texture2D) ImageTexture.CreateFromImage(image));
      }
      return this._fallbackSteamGlyphs[key2];
    }
    return this.ControllerConfig?.GetButtonIcon(hotkey);
  }

  public Vector2 GetLeftAnalogStickDirection()
  {
    return SteamInitializer.Initialized ? new Vector2(this._lStickPosition.X, -this._lStickPosition.Y) : this._fallbackStrategy.GetLeftAnalogStickDirection();
  }

  public Dictionary<StringName, StringName> GetDefaultControllerInputMap
  {
    get
    {
      return this.ControllerConfig == null ? this._fallbackStrategy.ControllerConfig.DefaultControllerInputMap : this.ControllerConfig.DefaultControllerInputMap;
    }
  }

  public bool ShouldAllowControllerRebinding
  {
    get => !SteamInitializer.Initialized && this._fallbackStrategy.ShouldAllowControllerRebinding;
  }
}
