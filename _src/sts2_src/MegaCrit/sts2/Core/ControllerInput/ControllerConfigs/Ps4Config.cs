// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.ControllerInput.ControllerConfigs.Ps4Config
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using System.Collections.Generic;

#nullable enable
namespace MegaCrit.Sts2.Core.ControllerInput.ControllerConfigs;

public class Ps4Config : ControllerConfig
{
  protected override string FolderPath => "atlases/controller_atlas.sprites/ps4";

  public override ControllerMappingType ControllerMappingType => ControllerMappingType.Playstation;

  public override Dictionary<string, StringName> SteamInputControllerMap
  {
    get
    {
      return new Dictionary<string, StringName>()
      {
        {
          "Confirm",
          Controller.faceButtonNorth
        },
        {
          "Cancel",
          Controller.faceButtonEast
        },
        {
          "Up",
          Controller.dPadUp
        },
        {
          "Down",
          Controller.dPadDown
        },
        {
          "Left",
          Controller.dPadLeft
        },
        {
          "Right",
          Controller.dPadRight
        },
        {
          "Select",
          Controller.faceButtonSouth
        },
        {
          "Top_Panel",
          Controller.faceButtonWest
        },
        {
          "View_Draw_Pile",
          Controller.leftTrigger
        },
        {
          "View_Discard_Pile",
          Controller.rightTrigger
        },
        {
          "Tab_Right",
          Controller.rightBumper
        },
        {
          "Tab_Left",
          Controller.leftBumper
        },
        {
          "View_Map",
          Controller.ps4Touchpad
        },
        {
          "Settings",
          Controller.startButton
        },
        {
          "Peek",
          Controller.lStickPress
        }
      };
    }
  }

  public override Dictionary<StringName, StringName> DefaultControllerInputMap
  {
    get
    {
      return new Dictionary<StringName, StringName>()
      {
        {
          MegaInput.accept,
          Controller.faceButtonNorth
        },
        {
          MegaInput.cancel,
          Controller.faceButtonEast
        },
        {
          MegaInput.select,
          Controller.faceButtonSouth
        },
        {
          MegaInput.viewExhaustPileAndTabRight,
          Controller.rightBumper
        },
        {
          MegaInput.viewDeckAndTabLeft,
          Controller.leftBumper
        },
        {
          MegaInput.topPanel,
          Controller.faceButtonWest
        },
        {
          MegaInput.viewDrawPile,
          Controller.leftTrigger
        },
        {
          MegaInput.viewDiscardPile,
          Controller.rightTrigger
        },
        {
          MegaInput.viewMap,
          Controller.ps4Touchpad
        },
        {
          MegaInput.peek,
          Controller.lStickPress
        },
        {
          MegaInput.up,
          Controller.dPadUp
        },
        {
          MegaInput.down,
          Controller.dPadDown
        },
        {
          MegaInput.left,
          Controller.dPadLeft
        },
        {
          MegaInput.right,
          Controller.dPadRight
        },
        {
          MegaInput.altUp,
          Controller.rStickUp
        },
        {
          MegaInput.altDown,
          Controller.rStickDown
        },
        {
          MegaInput.altLeft,
          Controller.rStickLeft
        },
        {
          MegaInput.altRight,
          Controller.rStickRight
        },
        {
          MegaInput.pauseAndBack,
          Controller.startButton
        }
      };
    }
  }
}
