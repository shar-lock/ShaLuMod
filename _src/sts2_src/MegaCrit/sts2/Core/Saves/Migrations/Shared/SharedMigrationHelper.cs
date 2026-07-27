// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Saves.Migrations.Shared.SharedMigrationHelper
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Nodes;

#nullable enable
namespace MegaCrit.Sts2.Core.Saves.Migrations.Shared;

public static class SharedMigrationHelper
{
  public static readonly IReadOnlyDictionary<string, string> v100Renames = (IReadOnlyDictionary<string, string>) new Dictionary<string, string>()
  {
    ["CARD.PREPARE"] = "CARD.PREPARED",
    ["ENCOUNTER.TOADPOLES_NORMAL"] = "ENCOUNTER.SEAPUNK_NORMAL",
    ["MONSTER.DOOR"] = "MONSTER.DEPRECATED_MONSTER"
  };

  public static void ReplaceModelIds(JsonNode? node, IReadOnlyDictionary<string, string> renames)
  {
    switch (node)
    {
      case JsonObject source:
        using (List<string>.Enumerator enumerator = new List<string>(((IEnumerable<KeyValuePair<string, JsonNode>>) source).Select<KeyValuePair<string, JsonNode>, string>((Func<KeyValuePair<string, JsonNode>, string>) (kvp => kvp.Key))).GetEnumerator())
        {
          while (enumerator.MoveNext())
          {
            string current = enumerator.Current;
            JsonNode node1 = ((JsonNode) source)[current];
            string key;
            string str;
            if (node1 is JsonValue jsonValue && jsonValue.TryGetValue<string>(ref key) && renames.TryGetValue(key, out str))
              ((JsonNode) source)[current] = JsonNode.op_Implicit(str);
            else
              SharedMigrationHelper.ReplaceModelIds(node1, renames);
          }
          break;
        }
      case JsonArray jsonArray:
        for (int index = 0; index < jsonArray.Count; ++index)
        {
          JsonNode node2 = ((JsonNode) jsonArray)[index];
          string key;
          string str;
          if (node2 is JsonValue jsonValue && jsonValue.TryGetValue<string>(ref key) && renames.TryGetValue(key, out str))
            ((JsonNode) jsonArray)[index] = JsonNode.op_Implicit(str);
          else
            SharedMigrationHelper.ReplaceModelIds(node2, renames);
        }
        break;
    }
  }

  public static void MigrateMapPointHistoryRooms(JsonNode? jsonNode)
  {
    if (!(jsonNode is JsonArray jsonArray1))
      return;
    foreach (JsonNode jsonNode1 in jsonArray1)
    {
      if (jsonNode1 is JsonArray jsonArray5)
      {
        foreach (JsonNode jsonNode2 in jsonArray5)
        {
          JsonNode jsonNode3;
          if (jsonNode2 is JsonObject jsonObject && jsonObject.TryGetPropertyValue("room_types", ref jsonNode3) && jsonNode3 is JsonArray jsonArray4)
          {
            JsonArray jsonArray2 = new JsonArray(new JsonNodeOptions?());
            jsonObject.Add("rooms", (JsonNode) jsonArray2);
            foreach (JsonNode jsonNode4 in jsonArray4)
            {
              if (jsonNode4 != null)
              {
                JsonObject jsonObject1 = new JsonObject(new JsonNodeOptions?());
                jsonObject1.Add("room_type", jsonNode4.DeepClone());
                JsonObject jsonObject2 = jsonObject1;
                jsonArray2.Add((JsonNode) jsonObject2);
              }
            }
            jsonObject.Remove("room_types");
            JsonObject jsonObject3 = (JsonObject) ((JsonNode) jsonArray2)[0];
            JsonArray jsonArray3 = jsonArray2;
            JsonObject jsonObject4 = (JsonObject) ((JsonNode) jsonArray3)[jsonArray3.Count - 1];
            JsonNode jsonNode5;
            if (jsonObject.TryGetPropertyValue("model_id", ref jsonNode5))
            {
              jsonObject3.Add("model_id", jsonNode5.DeepClone());
              jsonObject.Remove("model_id");
            }
            JsonNode jsonNode6;
            if (jsonObject.TryGetPropertyValue("monster_ids", ref jsonNode6))
            {
              jsonObject4.Add("monster_ids", jsonNode6.DeepClone());
              jsonObject.Remove("monster_ids");
            }
            JsonNode jsonNode7;
            if (jsonObject.TryGetPropertyValue("turns_taken", ref jsonNode7))
            {
              jsonObject4.Add("turns_taken", jsonNode7.DeepClone());
              jsonObject.Remove("turns_taken");
            }
          }
        }
      }
    }
  }

  public static void RecursiveRemoveSchema(JsonNode node, int depth = 0)
  {
    switch (node)
    {
      case JsonObject jsonObject:
        if (depth > 0)
          jsonObject.Remove("schema_version");
        using (IEnumerator<KeyValuePair<string, JsonNode>> enumerator = jsonObject.GetEnumerator())
        {
          while (enumerator.MoveNext())
          {
            KeyValuePair<string, JsonNode> current = enumerator.Current;
            if (current.Value != null)
              SharedMigrationHelper.RecursiveRemoveSchema(current.Value, depth + 1);
          }
          break;
        }
      case JsonArray jsonArray:
        using (IEnumerator<JsonNode> enumerator = jsonArray.GetEnumerator())
        {
          while (enumerator.MoveNext())
          {
            JsonNode current = enumerator.Current;
            if (current != null)
              SharedMigrationHelper.RecursiveRemoveSchema(current, depth + 1);
          }
          break;
        }
    }
  }

  public static void MigrateMapPointHistoryCardChoices(JsonNode? jsonNode)
  {
    if (!(jsonNode is JsonArray jsonArray1))
      return;
    foreach (JsonNode jsonNode1 in jsonArray1)
    {
      if (jsonNode1 is JsonArray jsonArray10)
      {
        foreach (JsonNode jsonNode2 in jsonArray10)
        {
          JsonNode jsonNode3;
          if (jsonNode2 is JsonObject jsonObject6 && jsonObject6.TryGetPropertyValue("player_stats", ref jsonNode3) && jsonNode3 is JsonArray jsonArray9)
          {
            foreach (JsonNode jsonNode4 in jsonArray9)
            {
              if (jsonNode4 is JsonObject jsonObject5)
              {
                JsonNode jsonNode5;
                if (jsonObject5.TryGetPropertyValue("cards_gained", ref jsonNode5) && jsonNode5 is JsonArray jsonArray3)
                {
                  JsonArray jsonArray2 = new JsonArray(new JsonNodeOptions?());
                  foreach (JsonNode jsonNode6 in jsonArray3)
                  {
                    JsonObject jsonObject = new JsonObject(new JsonNodeOptions?());
                    ((JsonNode) jsonObject)["id"] = JsonNode.op_Implicit(jsonNode6.GetValue<string>());
                    ((JsonNode) jsonObject)["current_upgrade_level"] = JsonNode.op_Implicit(0);
                    jsonArray2.Add((JsonNode) jsonObject);
                  }
                  jsonNode4["cards_gained"] = (JsonNode) jsonArray2;
                }
                JsonNode jsonNode7;
                if (jsonObject5.TryGetPropertyValue("cards_removed", ref jsonNode7) && jsonNode7 is JsonArray jsonArray5)
                {
                  JsonArray jsonArray4 = new JsonArray(new JsonNodeOptions?());
                  foreach (JsonNode jsonNode8 in jsonArray5)
                  {
                    JsonObject jsonObject = new JsonObject(new JsonNodeOptions?());
                    ((JsonNode) jsonObject)["id"] = JsonNode.op_Implicit(jsonNode8.GetValue<string>());
                    ((JsonNode) jsonObject)["current_upgrade_level"] = JsonNode.op_Implicit(0);
                    jsonArray4.Add((JsonNode) jsonObject);
                  }
                  jsonNode4["cards_removed"] = (JsonNode) jsonArray4;
                }
                JsonNode jsonNode9;
                if (jsonObject5.TryGetPropertyValue("card_choices", ref jsonNode9))
                {
                  if (jsonNode9 is JsonArray jsonArray6)
                  {
                    foreach (JsonNode jsonNode10 in jsonArray6)
                    {
                      if (jsonNode10 is JsonObject jsonObject1)
                      {
                        JsonObject jsonObject = new JsonObject(new JsonNodeOptions?());
                        ((JsonNode) jsonObject)["id"] = JsonNode.op_Implicit(((JsonNode) jsonObject1)["choice"].GetValue<string>());
                        ((JsonNode) jsonObject)["current_upgrade_level"] = JsonNode.op_Implicit(0);
                        ((JsonNode) jsonObject1)["card"] = (JsonNode) jsonObject;
                        jsonObject1.Remove("choice");
                      }
                    }
                  }
                  else
                    continue;
                }
                JsonNode jsonNode11;
                if (jsonObject5.TryGetPropertyValue("cards_enchanted", ref jsonNode11))
                {
                  if (jsonNode11 is JsonArray jsonArray7)
                  {
                    foreach (JsonNode jsonNode12 in jsonArray7)
                    {
                      if (jsonNode12 is JsonObject jsonObject2)
                      {
                        JsonObject jsonObject = new JsonObject(new JsonNodeOptions?());
                        ((JsonNode) jsonObject)["id"] = JsonNode.op_Implicit(((JsonNode) jsonObject2)["card"].GetValue<string>());
                        ((JsonNode) jsonObject)["current_upgrade_level"] = JsonNode.op_Implicit(0);
                        ((JsonNode) jsonObject2)["card"] = (JsonNode) jsonObject;
                      }
                    }
                  }
                  else
                    continue;
                }
                JsonNode jsonNode13;
                if (jsonObject5.TryGetPropertyValue("cards_transformed", ref jsonNode13) && jsonNode13 is JsonArray jsonArray8)
                {
                  foreach (JsonNode jsonNode14 in jsonArray8)
                  {
                    if (jsonNode14 is JsonObject jsonObject)
                    {
                      JsonObject jsonObject3 = new JsonObject(new JsonNodeOptions?());
                      ((JsonNode) jsonObject3)["id"] = JsonNode.op_Implicit(((JsonNode) jsonObject)["original_card"].GetValue<string>());
                      ((JsonNode) jsonObject3)["current_upgrade_level"] = JsonNode.op_Implicit(0);
                      ((JsonNode) jsonObject)["original_card"] = (JsonNode) jsonObject3;
                      JsonObject jsonObject4 = new JsonObject(new JsonNodeOptions?());
                      ((JsonNode) jsonObject4)["id"] = JsonNode.op_Implicit(((JsonNode) jsonObject)["final_card"].GetValue<string>());
                      ((JsonNode) jsonObject4)["current_upgrade_level"] = JsonNode.op_Implicit(0);
                      ((JsonNode) jsonObject)["final_card"] = (JsonNode) jsonObject4;
                    }
                  }
                }
              }
            }
          }
        }
      }
    }
  }
}
