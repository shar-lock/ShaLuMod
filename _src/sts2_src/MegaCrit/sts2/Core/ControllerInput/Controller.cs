// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.ControllerInput.Controller
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;

#nullable enable
namespace MegaCrit.Sts2.Core.ControllerInput;

public static class Controller
{
  public static readonly StringName leftTrigger = StringName.op_Implicit("controller_left_trigger");
  public static readonly StringName rightTrigger = StringName.op_Implicit("controller_right_trigger");
  public static readonly StringName leftBumper = StringName.op_Implicit("controller_left_bumper");
  public static readonly StringName rightBumper = StringName.op_Implicit("controller_right_bumper");
  public static readonly StringName faceButtonNorth = StringName.op_Implicit("controller_face_button_north");
  public static readonly StringName faceButtonSouth = StringName.op_Implicit("controller_face_button_south");
  public static readonly StringName faceButtonEast = StringName.op_Implicit("controller_face_button_east");
  public static readonly StringName faceButtonWest = StringName.op_Implicit("controller_face_button_west");
  public static readonly StringName startButton = StringName.op_Implicit("controller_start_button");
  public static readonly StringName selectButton = StringName.op_Implicit("controller_select_button");
  public static readonly StringName dPadUp = StringName.op_Implicit("controller_d_pad_up");
  public static readonly StringName dPadDown = StringName.op_Implicit("controller_d_pad_down");
  public static readonly StringName dPadLeft = StringName.op_Implicit("controller_d_pad_left");
  public static readonly StringName dPadRight = StringName.op_Implicit("controller_d_pad_right");
  public static readonly StringName lStickPress = StringName.op_Implicit("controller_l_stick_press");
  public static readonly StringName lStickLeft = StringName.op_Implicit("controller_l_stick_left");
  public static readonly StringName lStickRight = StringName.op_Implicit("controller_l_stick_right");
  public static readonly StringName lStickUp = StringName.op_Implicit("controller_l_stick_up");
  public static readonly StringName lStickDown = StringName.op_Implicit("controller_l_stick_down");
  public static readonly StringName rStickLeft = StringName.op_Implicit("controller_r_stick_left");
  public static readonly StringName rStickRight = StringName.op_Implicit("controller_r_stick_right");
  public static readonly StringName rStickUp = StringName.op_Implicit("controller_r_stick_up");
  public static readonly StringName rStickDown = StringName.op_Implicit("controller_r_stick_down");
  public static readonly StringName ps4Touchpad = StringName.op_Implicit("ui_controller_touch_pad");

  public static StringName[] AllControllerInputs
  {
    get
    {
      return new StringName[24]
      {
        Controller.dPadRight,
        Controller.dPadUp,
        Controller.dPadDown,
        Controller.dPadLeft,
        Controller.faceButtonEast,
        Controller.faceButtonNorth,
        Controller.faceButtonSouth,
        Controller.faceButtonWest,
        Controller.lStickDown,
        Controller.lStickLeft,
        Controller.lStickPress,
        Controller.lStickRight,
        Controller.lStickUp,
        Controller.leftBumper,
        Controller.leftTrigger,
        Controller.rightBumper,
        Controller.rightTrigger,
        Controller.selectButton,
        Controller.startButton,
        Controller.ps4Touchpad,
        Controller.rStickLeft,
        Controller.rStickRight,
        Controller.rStickUp,
        Controller.rStickDown
      };
    }
  }
}
