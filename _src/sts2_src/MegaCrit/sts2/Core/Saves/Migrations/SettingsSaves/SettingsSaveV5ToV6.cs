// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Saves.Migrations.SettingsSaves.SettingsSaveV5ToV6
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using MegaCrit.Sts2.Core.ControllerInput;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Nodes;

#nullable enable
namespace MegaCrit.Sts2.Core.Saves.Migrations.SettingsSaves;

[Migration(typeof (SettingsSave), 5, 6)]
public class SettingsSaveV5ToV6 : MigrationBase<SettingsSave>
{
  private readonly Dictionary<string, string> _oldToNewMappings = new Dictionary<string, string>()
  {
    {
      "controller_d_pad_north",
      StringName.op_Implicit(Controller.dPadUp)
    },
    {
      "controller_d_pad_south",
      StringName.op_Implicit(Controller.dPadDown)
    },
    {
      "controller_d_pad_west",
      StringName.op_Implicit(Controller.dPadLeft)
    },
    {
      "controller_d_pad_east",
      StringName.op_Implicit(Controller.dPadRight)
    },
    {
      "controller_joystick_up",
      StringName.op_Implicit(Controller.lStickUp)
    },
    {
      "controller_joystick_down",
      StringName.op_Implicit(Controller.lStickDown)
    },
    {
      "controller_joystick_left",
      StringName.op_Implicit(Controller.lStickLeft)
    },
    {
      "controller_joystick_right",
      StringName.op_Implicit(Controller.lStickRight)
    },
    {
      "controller_joystick_press",
      StringName.op_Implicit(Controller.lStickPress)
    },
    {
      "controller_l_joystick_up",
      StringName.op_Implicit(Controller.lStickUp)
    },
    {
      "controller_l_joystick_down",
      StringName.op_Implicit(Controller.lStickDown)
    },
    {
      "controller_l_joystick_left",
      StringName.op_Implicit(Controller.lStickLeft)
    },
    {
      "controller_l_joystick_right",
      StringName.op_Implicit(Controller.lStickRight)
    },
    {
      "controller_l_joystick_press",
      StringName.op_Implicit(Controller.lStickPress)
    },
    {
      "controller_r_joystick_up",
      StringName.op_Implicit(Controller.rStickUp)
    },
    {
      "controller_r_joystick_down",
      StringName.op_Implicit(Controller.rStickDown)
    },
    {
      "controller_r_joystick_left",
      StringName.op_Implicit(Controller.rStickLeft)
    },
    {
      "controller_r_joystick_right",
      StringName.op_Implicit(Controller.rStickRight)
    }
  };

  protected override void ApplyMigration(MigratingData saveData)
  {
    if (!(saveData.GetRawNode("controller_mapping") is JsonObject rawNode))
      return;
    if (!rawNode.ContainsKey("ui_alt_up"))
      ((JsonNode) rawNode)["ui_alt_up"] = JsonNode.op_Implicit(Controller.rStickUp.ToString());
    if (!rawNode.ContainsKey("ui_alt_down"))
      ((JsonNode) rawNode)["ui_alt_down"] = JsonNode.op_Implicit(Controller.rStickDown.ToString());
    if (!rawNode.ContainsKey("ui_alt_left"))
      ((JsonNode) rawNode)["ui_alt_left"] = JsonNode.op_Implicit(Controller.rStickLeft.ToString());
    if (!rawNode.ContainsKey("ui_alt_right"))
      ((JsonNode) rawNode)["ui_alt_right"] = JsonNode.op_Implicit(Controller.rStickRight.ToString());
    foreach (string str1 in ((IEnumerable<KeyValuePair<string, JsonNode>>) rawNode).Select<KeyValuePair<string, JsonNode>, string>((Func<KeyValuePair<string, JsonNode>, string>) (kvp => kvp.Key)).ToList<string>())
    {
      if (rawNode.ContainsKey(str1))
      {
        string str2 = ((JsonNode) rawNode)[str1].GetValue<string>();
        foreach (KeyValuePair<string, string> oldToNewMapping in this._oldToNewMappings)
        {
          if (oldToNewMapping.Key == str2)
            ((JsonNode) rawNode)[str1] = JsonNode.op_Implicit(oldToNewMapping.Value);
        }
      }
    }
  }
}
