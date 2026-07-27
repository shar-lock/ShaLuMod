// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Saves.Migrations.SerializableRuns.SerializableRunV18ToV19
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Entities.Rngs;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Random;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Nodes;

#nullable enable
namespace MegaCrit.Sts2.Core.Saves.Migrations.SerializableRuns;

[Migration(typeof (SerializableRun), 18, 19)]
public class SerializableRunV18ToV19 : MigrationBase<SerializableRun>
{
  public const string seedPrefix = "old";

  protected override void ApplyMigration(MigratingData saveData)
  {
    this.MigrateRunRngs(saveData);
    this.MigratePlayerRngs(saveData);
    Log.Info("SerializableRun migration v18 -> v19: Upgraded RNG serialization");
  }

  private void MigrateRunRngs(MigratingData saveData)
  {
    string str1;
    if (!(saveData.GetRawNode("rng") is JsonObject rawNode) || !(((JsonNode) rawNode)["seed"] is JsonValue jsonValue1) || !jsonValue1.TryGetValue<string>(ref str1) || !(((JsonNode) rawNode)["counters"] is JsonObject jsonObject1))
      return;
    JsonObject jsonObject2 = new JsonObject(new JsonNodeOptions?());
    foreach (KeyValuePair<string, JsonNode> keyValuePair in jsonObject1)
    {
      int counter;
      RunRngType result;
      if (keyValuePair.Value is JsonValue jsonValue2 && jsonValue2.TryGetValue<int>(ref counter) && Enum.TryParse<RunRngType>(keyValuePair.Key, out result))
      {
        string type = StringHelper.SnakeCase(result.ToString());
        MegaRandom rng1 = SerializableRunV18ToV19.GenerateRng((ulong) StringHelper.GetDeterministicHashCodeOld(str1), type, counter);
        SerializableRng rng2 = new SerializableRng()
        {
          counter = counter
        };
        rng1.FillSerializableState(rng2);
        string str2 = JsonSerializer.Serialize<SerializableRng>(rng2, JsonSerializationUtility.GetTypeInfo<SerializableRng>());
        ((JsonNode) jsonObject2)[keyValuePair.Key] = JsonNode.Parse(str2, new JsonNodeOptions?(), new JsonDocumentOptions());
      }
    }
    rawNode.Remove("counters");
    ((JsonNode) rawNode)["rngs"] = (JsonNode) jsonObject2;
    ((JsonNode) rawNode)["seed"] = JsonNode.op_Implicit("old" + str1);
  }

  private void MigratePlayerRngs(MigratingData saveData)
  {
    if (!(saveData.GetRawNode("players") is JsonArray rawNode))
      return;
    foreach (JsonNode jsonNode in rawNode)
    {
      ulong seed;
      if (jsonNode?["rng"] is JsonObject jsonObject1 && ((JsonNode) jsonObject1)["seed"] is JsonValue jsonValue1 && jsonValue1.TryGetValue<ulong>(ref seed) && ((JsonNode) jsonObject1)["counters"] is JsonObject jsonObject2)
      {
        JsonObject jsonObject = new JsonObject(new JsonNodeOptions?());
        foreach (KeyValuePair<string, JsonNode> keyValuePair in jsonObject2)
        {
          int counter;
          PlayerRngType result;
          if (keyValuePair.Value is JsonValue jsonValue && jsonValue.TryGetValue<int>(ref counter) && Enum.TryParse<PlayerRngType>(keyValuePair.Key, out result))
          {
            string type = StringHelper.SnakeCase(result.ToString());
            MegaRandom rng1 = SerializableRunV18ToV19.GenerateRng(seed, type, counter);
            SerializableRng rng2 = new SerializableRng()
            {
              counter = counter
            };
            rng1.FillSerializableState(rng2);
            string str = JsonSerializer.Serialize<SerializableRng>(rng2, JsonSerializationUtility.GetTypeInfo<SerializableRng>());
            ((JsonNode) jsonObject)[keyValuePair.Key] = JsonNode.Parse(str, new JsonNodeOptions?(), new JsonDocumentOptions());
          }
        }
        jsonObject1.Remove("counters");
        ((JsonNode) jsonObject1)["rngs"] = (JsonNode) jsonObject;
      }
    }
  }

  public static MegaRandom GenerateRng(ulong seed, string type, int counter)
  {
    MegaRandom rng = new MegaRandom((ulong) ((uint) seed + (uint) StringHelper.GetDeterministicHashCodeOld(type)));
    for (int index = 0; index < counter; ++index)
    {
      long num = (long) rng.NextULong();
    }
    return rng;
  }
}
