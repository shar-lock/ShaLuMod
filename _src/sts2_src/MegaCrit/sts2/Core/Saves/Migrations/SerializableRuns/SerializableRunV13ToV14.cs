// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Saves.Migrations.SerializableRuns.SerializableRunV13ToV14
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Models;
using System.Collections.Generic;
using System.Text.Json.Nodes;

#nullable enable
namespace MegaCrit.Sts2.Core.Saves.Migrations.SerializableRuns;

[Migration(typeof (SerializableRun), 13, 14)]
public class SerializableRunV13ToV14 : MigrationBase<SerializableRun>
{
  protected override void ApplyMigration(MigratingData saveData)
  {
    Log.Info("SerializableRun migration v13 -> v14: Migrating SerializableRun.CardPoolId to CardPoolIds");
    if (!(saveData.GetRawNode("pre_finished_room") is JsonObject rawNode) || !(((JsonNode) rawNode)["extra_rewards"] is JsonObject jsonObject1))
      return;
    foreach (KeyValuePair<string, JsonNode> keyValuePair in jsonObject1)
    {
      string str1;
      JsonNode jsonNode1;
      keyValuePair.Deconstruct(ref str1, ref jsonNode1);
      if (jsonNode1 is JsonArray jsonArray)
      {
        foreach (JsonNode jsonNode2 in jsonArray)
        {
          if (jsonNode2 is JsonObject jsonObject2)
          {
            if (!jsonObject2.ContainsKey("card_pools"))
              ((JsonNode) jsonObject2)["card_pools"] = (JsonNode) new JsonArray(new JsonNodeOptions?());
            JsonNode jsonNode3;
            if (jsonObject2.TryGetPropertyValue("card_pool", ref jsonNode3))
            {
              string str2 = jsonNode3?.GetValue<string>();
              jsonObject2.Remove("card_pool");
              if (str2 != null && !(str2 == ModelId.none.ToString()))
                ((JsonArray) ((JsonNode) jsonObject2)["card_pools"]).Add<string>(str2);
            }
          }
        }
      }
    }
  }
}
