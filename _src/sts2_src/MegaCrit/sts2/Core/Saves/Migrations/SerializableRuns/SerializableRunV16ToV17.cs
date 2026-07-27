// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Saves.Migrations.SerializableRuns.SerializableRunV16ToV17
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Logging;
using System;
using System.Text.Json.Nodes;

#nullable enable
namespace MegaCrit.Sts2.Core.Saves.Migrations.SerializableRuns;

[Migration(typeof (SerializableRun), 16 /*0x10*/, 17)]
public class SerializableRunV16ToV17 : MigrationBase<SerializableRun>
{
  protected override void ApplyMigration(MigratingData saveData)
  {
    Log.Info("SerializableRun migration v16 -> v17: Migrating LastingCandy.CombatsSeen to LastingCandy.CombatRewardsSeen");
    JsonObject rawNode1 = saveData.GetRawNode();
    if (rawNode1 == null)
      return;
    bool flag = false;
    string str1;
    if (saveData.GetRawNode("pre_finished_room") is JsonObject rawNode2 && ((JsonNode) rawNode2)["room_type"] is JsonValue jsonValue1 && jsonValue1.TryGetValue<string>(ref str1) && (str1.Equals("monster", StringComparison.OrdinalIgnoreCase) || str1.Equals("elite", StringComparison.OrdinalIgnoreCase) || str1.Equals("boss", StringComparison.OrdinalIgnoreCase)))
    {
      string str2;
      flag = !string.Equals(!(((JsonNode) rawNode2)["encounter_id"] is JsonValue jsonValue) || !jsonValue.TryGetValue<string>(ref str2) ? (string) null : str2, "ENCOUNTER.BATTLEWORN_DUMMY_EVENT_ENCOUNTER", StringComparison.OrdinalIgnoreCase);
    }
    if (!(((JsonNode) rawNode1)["players"] is JsonArray jsonArray1))
      return;
    foreach (JsonNode jsonNode1 in jsonArray1)
    {
      if (jsonNode1 != null && jsonNode1["relics"] is JsonArray jsonArray3)
      {
        foreach (JsonNode jsonNode2 in jsonArray3)
        {
          if (jsonNode2 != null && jsonNode2["id"] != null && !(jsonNode2["id"].GetValue<string>() != "RELIC.LASTING_CANDY") && jsonNode2["props"] is JsonObject jsonObject && ((JsonNode) jsonObject)["ints"] is JsonArray jsonArray2)
          {
            foreach (JsonNode jsonNode3 in jsonArray2)
            {
              if (jsonNode3 != null && jsonNode3["name"] != null && !(jsonNode3["name"].GetValue<string>() != "CombatsSeen"))
              {
                jsonNode3["name"] = JsonNode.op_Implicit("CombatRewardsSeen");
                if (flag)
                  jsonNode3["value"] = JsonNode.op_Implicit(jsonNode3["value"].GetValue<int>() - 1);
              }
            }
          }
        }
      }
    }
  }
}
