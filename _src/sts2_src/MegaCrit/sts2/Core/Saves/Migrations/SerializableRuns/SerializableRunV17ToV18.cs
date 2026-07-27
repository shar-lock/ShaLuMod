// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Saves.Migrations.SerializableRuns.SerializableRunV17ToV18
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Logging;
using System;
using System.Text.Json.Nodes;

#nullable enable
namespace MegaCrit.Sts2.Core.Saves.Migrations.SerializableRuns;

[Migration(typeof (SerializableRun), 17, 18)]
public class SerializableRunV17ToV18 : MigrationBase<SerializableRun>
{
  private const string _oldEncounterId = "ENCOUNTER.BATTLEWORN_DUMMY_EVENT_ENCOUNTER";
  private const string _v1EncounterId = "ENCOUNTER.BATTLEWORN_DUMMY_EVENT_V1_ENCOUNTER";
  private const string _v2EncounterId = "ENCOUNTER.BATTLEWORN_DUMMY_EVENT_V2_ENCOUNTER";
  private const string _v3EncounterId = "ENCOUNTER.BATTLEWORN_DUMMY_EVENT_V3_ENCOUNTER";
  private const string _settingKey = "Setting";

  protected override void ApplyMigration(MigratingData saveData)
  {
    string a;
    if (!(saveData.GetRawNode("pre_finished_room") is JsonObject rawNode) || !(((JsonNode) rawNode)["encounter_id"] is JsonValue jsonValue1) || !jsonValue1.TryGetValue<string>(ref a) || !string.Equals(a, "ENCOUNTER.BATTLEWORN_DUMMY_EVENT_ENCOUNTER", StringComparison.OrdinalIgnoreCase))
      return;
    string str1;
    string str2 = !((((JsonNode) rawNode)["encounter_state"] is JsonObject jsonObject ? ((JsonNode) jsonObject)["Setting"] : (JsonNode) null) is JsonValue jsonValue2) || !jsonValue2.TryGetValue<string>(ref str1) ? (string) null : str1;
    string str3;
    switch (str2)
    {
      case "Setting2":
        str3 = "ENCOUNTER.BATTLEWORN_DUMMY_EVENT_V2_ENCOUNTER";
        break;
      case "Setting3":
        str3 = "ENCOUNTER.BATTLEWORN_DUMMY_EVENT_V3_ENCOUNTER";
        break;
      default:
        str3 = "ENCOUNTER.BATTLEWORN_DUMMY_EVENT_V1_ENCOUNTER";
        break;
    }
    string str4 = str3;
    ((JsonNode) rawNode)["encounter_id"] = JsonNode.op_Implicit(str4);
    jsonObject?.Remove("Setting");
    Log.Info($"SerializableRun migration v17 -> v18: remapped Battleworn Dummy encounter to {str4} (setting: {str2 ?? "none"})");
  }
}
