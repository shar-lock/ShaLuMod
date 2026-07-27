// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.ControllerInput.GodotControllerInputStrategy
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using MegaCrit.Sts2.Core.ControllerInput.ControllerConfigs;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using System.Collections.Generic;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.ControllerInput;

public class GodotControllerInputStrategy : IControllerInputStrategy
{
  private static readonly StringName _rStickLeftRaw = StringName.op_Implicit("raw_r_stick_left");
  private static readonly StringName _rStickRightRaw = StringName.op_Implicit("raw_r_stick_right");
  private static readonly StringName _rStickUpRaw = StringName.op_Implicit("raw_r_stick_up");
  private static readonly StringName _rStickDownRaw = StringName.op_Implicit("raw_r_stick_down");
  private static readonly StringName _lStickLeftRaw = StringName.op_Implicit("raw_l_stick_left");
  private static readonly StringName _lStickRightRaw = StringName.op_Implicit("raw_l_stick_right");
  private static readonly StringName _lStickUpRaw = StringName.op_Implicit("raw_l_stick_up");
  private static readonly StringName _lStickDownRaw = StringName.op_Implicit("raw_l_stick_down");
  private static readonly StringName _leftTriggerRaw = StringName.op_Implicit("raw_left_trigger");
  private static readonly StringName _rightTriggerRaw = StringName.op_Implicit("raw_right_trigger");
  private readonly Dictionary<StringName, StringName[]> _analogToDigitalInput = new Dictionary<StringName, StringName[]>()
  {
    {
      GodotControllerInputStrategy._rStickUpRaw,
      new StringName[1]{ Controller.rStickUp }
    },
    {
      GodotControllerInputStrategy._rStickDownRaw,
      new StringName[1]{ Controller.rStickDown }
    },
    {
      GodotControllerInputStrategy._rStickLeftRaw,
      new StringName[1]{ Controller.rStickLeft }
    },
    {
      GodotControllerInputStrategy._rStickRightRaw,
      new StringName[1]{ Controller.rStickRight }
    },
    {
      GodotControllerInputStrategy._leftTriggerRaw,
      new StringName[1]{ Controller.leftTrigger }
    },
    {
      GodotControllerInputStrategy._rightTriggerRaw,
      new StringName[1]{ Controller.rightTrigger }
    },
    {
      GodotControllerInputStrategy._lStickUpRaw,
      new StringName[2]{ Controller.dPadUp, Controller.lStickUp }
    },
    {
      GodotControllerInputStrategy._lStickDownRaw,
      new StringName[2]
      {
        Controller.dPadDown,
        Controller.lStickDown
      }
    },
    {
      GodotControllerInputStrategy._lStickLeftRaw,
      new StringName[2]
      {
        Controller.dPadLeft,
        Controller.lStickLeft
      }
    },
    {
      GodotControllerInputStrategy._lStickRightRaw,
      new StringName[2]
      {
        Controller.dPadRight,
        Controller.lStickRight
      }
    }
  };
  private string? _currentControllerType;
  private ControllerConfig? _controllerConfig;

  public ControllerConfig? ControllerConfig
  {
    get
    {
      if (this._controllerConfig == null)
        this.UpdateControllerConfig();
      return this._controllerConfig;
    }
  }

  public Task Init()
  {
    this.UpdateControllerConfig();
    return (Task) Task.FromResult<bool>(true);
  }

  public void ProcessInput()
  {
    foreach (StringName allControllerInput in Controller.AllControllerInputs)
    {
      if (Input.IsActionJustPressed(allControllerInput, false))
        this.UpdateControllerConfig();
    }
    foreach (KeyValuePair<StringName, StringName[]> keyValuePair in this._analogToDigitalInput)
    {
      if (Input.IsActionJustPressed(keyValuePair.Key, false))
      {
        foreach (StringName stringName in keyValuePair.Value)
          Input.ParseInputEvent((InputEvent) new InputEventAction()
          {
            Action = stringName,
            Pressed = true
          });
      }
      else if (Input.IsActionJustReleased(keyValuePair.Key, false))
      {
        foreach (StringName stringName in keyValuePair.Value)
          Input.ParseInputEvent((InputEvent) new InputEventAction()
          {
            Action = stringName,
            Pressed = false
          });
      }
    }
  }

  private void UpdateControllerConfig()
  {
    if (Input.GetConnectedJoypads().Count == 0)
    {
      this._controllerConfig = (ControllerConfig) new SteamControllerConfig();
    }
    else
    {
      string joyName = Input.GetJoyName(0);
      if (joyName == this._currentControllerType)
        return;
      this._currentControllerType = joyName;
      this._controllerConfig = this._currentControllerType.Contains("Xbox One") || this._currentControllerType.Contains("XInput") ? (ControllerConfig) new XboxOneConfig() : (!this._currentControllerType.Contains("Xbox 360") ? (!this._currentControllerType.Contains("PS3") ? (this._currentControllerType.Contains("PS4") || this._currentControllerType.Contains("DualSense") ? (ControllerConfig) new Ps4Config() : (!this._currentControllerType.Contains("PS5") ? (!this._currentControllerType.Contains("Switch") ? (ControllerConfig) new SteamControllerConfig() : (ControllerConfig) new SwitchConfig()) : (ControllerConfig) new Ps4Config())) : (ControllerConfig) new Ps4Config()) : (ControllerConfig) new Xbox360Config());
      NControllerManager.Instance?.OnControllerTypeChanged();
    }
  }

  public Texture2D? GetHotkeyIcon(string hotkey) => this._controllerConfig?.GetButtonIcon(hotkey);

  public Dictionary<StringName, StringName> GetDefaultControllerInputMap
  {
    get
    {
      if (this.ControllerConfig == null)
        this.UpdateControllerConfig();
      return this.ControllerConfig.DefaultControllerInputMap;
    }
  }

  public string GetControllerName()
  {
    return Input.GetConnectedJoypads().Count == 0 ? "NONE" : Input.GetJoyName(0);
  }

  public Vector2 GetLeftAnalogStickDirection()
  {
    return Input.GetVector(GodotControllerInputStrategy._lStickLeftRaw, GodotControllerInputStrategy._lStickRightRaw, GodotControllerInputStrategy._lStickUpRaw, GodotControllerInputStrategy._lStickDownRaw, -1f);
  }

  public bool ShouldAllowControllerRebinding => true;
}
