// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.ControllerInput.ControllerConfig
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using MegaCrit.Sts2.Core.ControllerInput.ControllerConfigs;
using MegaCrit.Sts2.Core.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable enable
namespace MegaCrit.Sts2.Core.ControllerInput;

public abstract class ControllerConfig
{
  private Dictionary<string, string>? _glyphs;

  protected abstract string FolderPath { get; }

  public virtual ControllerMappingType ControllerMappingType => ControllerMappingType.Default;

  public virtual Dictionary<string, StringName> SteamInputControllerMap
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
          Controller.selectButton
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

  public virtual Dictionary<StringName, StringName> DefaultControllerInputMap
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
          Controller.selectButton
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

  protected virtual string FaceButtonNorthGlyph
  {
    get => ImageHelper.GetImagePath(this.FolderPath + "/y.tres");
  }

  protected virtual string FaceButtonSouthGlyph
  {
    get => ImageHelper.GetImagePath(this.FolderPath + "/a.tres");
  }

  protected virtual string FaceButtonEastGlyph
  {
    get => ImageHelper.GetImagePath(this.FolderPath + "/b.tres");
  }

  protected virtual string FaceButtonWestGlyph
  {
    get => ImageHelper.GetImagePath(this.FolderPath + "/x.tres");
  }

  private string LeftTriggerGlyph => ImageHelper.GetImagePath(this.FolderPath + "/lt.tres");

  private string RightTriggerGlyph => ImageHelper.GetImagePath(this.FolderPath + "/rt.tres");

  private string SelectButtonGlyph => ImageHelper.GetImagePath(this.FolderPath + "/back.tres");

  private string LeftBumperGlyph => ImageHelper.GetImagePath(this.FolderPath + "/lb.tres");

  private string RightBumperGlyph => ImageHelper.GetImagePath(this.FolderPath + "/rb.tres");

  private string StartButtonGlyph => ImageHelper.GetImagePath(this.FolderPath + "/start.tres");

  private string JoystickPressGlyph => ImageHelper.GetImagePath(this.FolderPath + "/ls.tres");

  private string DPadNorth => ImageHelper.GetImagePath(this.FolderPath + "/up.tres");

  private string DPadSouth => ImageHelper.GetImagePath(this.FolderPath + "/down.tres");

  private string DPadWest => ImageHelper.GetImagePath(this.FolderPath + "/left.tres");

  private string DPadEast => ImageHelper.GetImagePath(this.FolderPath + "/right.tres");

  private string Ps4Touchpad => ImageHelper.GetImagePath(this.FolderPath + "/touchpad.tres");

  private string RightJoystickNorth => ImageHelper.GetImagePath(this.FolderPath + "/rs_up.tres");

  private string RightJoystickSouth => ImageHelper.GetImagePath(this.FolderPath + "/rs_down.tres");

  private string RightJoystickEast => ImageHelper.GetImagePath(this.FolderPath + "/rs_right.tres");

  private string RightJoystickWest => ImageHelper.GetImagePath(this.FolderPath + "/rs_left.tres");

  private Dictionary<string, string> GlyphMap
  {
    get
    {
      if (this._glyphs == null)
        this._glyphs = new Dictionary<string, string>()
        {
          {
            StringName.op_Implicit(Controller.faceButtonNorth),
            this.FaceButtonNorthGlyph
          },
          {
            StringName.op_Implicit(Controller.faceButtonSouth),
            this.FaceButtonSouthGlyph
          },
          {
            StringName.op_Implicit(Controller.faceButtonEast),
            this.FaceButtonEastGlyph
          },
          {
            StringName.op_Implicit(Controller.faceButtonWest),
            this.FaceButtonWestGlyph
          },
          {
            StringName.op_Implicit(Controller.leftTrigger),
            this.LeftTriggerGlyph
          },
          {
            StringName.op_Implicit(Controller.rightTrigger),
            this.RightTriggerGlyph
          },
          {
            StringName.op_Implicit(Controller.leftBumper),
            this.LeftBumperGlyph
          },
          {
            StringName.op_Implicit(Controller.rightBumper),
            this.RightBumperGlyph
          },
          {
            StringName.op_Implicit(Controller.selectButton),
            this.SelectButtonGlyph
          },
          {
            StringName.op_Implicit(Controller.startButton),
            this.StartButtonGlyph
          },
          {
            StringName.op_Implicit(Controller.lStickPress),
            this.JoystickPressGlyph
          },
          {
            StringName.op_Implicit(Controller.dPadUp),
            this.DPadNorth
          },
          {
            StringName.op_Implicit(Controller.dPadDown),
            this.DPadSouth
          },
          {
            StringName.op_Implicit(Controller.dPadRight),
            this.DPadEast
          },
          {
            StringName.op_Implicit(Controller.dPadLeft),
            this.DPadWest
          },
          {
            StringName.op_Implicit(Controller.ps4Touchpad),
            this.Ps4Touchpad
          },
          {
            StringName.op_Implicit(Controller.rStickUp),
            this.RightJoystickNorth
          },
          {
            StringName.op_Implicit(Controller.rStickDown),
            this.RightJoystickSouth
          },
          {
            StringName.op_Implicit(Controller.rStickLeft),
            this.RightJoystickWest
          },
          {
            StringName.op_Implicit(Controller.rStickRight),
            this.RightJoystickEast
          }
        };
      return this._glyphs;
    }
  }

  public Texture2D? GetButtonIcon(string button)
  {
    string str;
    return this.GlyphMap.TryGetValue(button, out str) ? ResourceLoader.Load<Texture2D>(str, (string) null, (ResourceLoader.CacheMode) 1L) : (Texture2D) null;
  }

  public IEnumerable<string> AssetPaths
  {
    get
    {
      return this.GlyphMap.Values.Where<string>((Func<string, bool>) (path => ResourceLoader.Exists(path, "")));
    }
  }

  public static IEnumerable<string> AllAssetPaths
  {
    get
    {
      return ((IEnumerable<ControllerConfig>) new ControllerConfig[4]
      {
        (ControllerConfig) new SteamControllerConfig(),
        (ControllerConfig) new Ps4Config(),
        (ControllerConfig) new Xbox360Config(),
        (ControllerConfig) new XboxOneConfig()
      }).SelectMany<ControllerConfig, string>((Func<ControllerConfig, IEnumerable<string>>) (c => c.AssetPaths));
    }
  }
}
