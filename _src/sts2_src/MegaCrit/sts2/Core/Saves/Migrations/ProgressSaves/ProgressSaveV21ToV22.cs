// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Saves.Migrations.ProgressSaves.ProgressSaveV21ToV22
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Monsters;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Nodes;

#nullable enable
namespace MegaCrit.Sts2.Core.Saves.Migrations.ProgressSaves;

[Migration(typeof (SerializableProgress), 21, 22)]
public class ProgressSaveV21ToV22 : MigrationBase<SerializableProgress>
{
  protected override void ApplyMigration(MigratingData saveData)
  {
    Log.Info("Progress save migration v21 -> v22: Removing Osty, Byrdpip and Pael's Legion from enemy stats");
    string[] source = new string[3]
    {
      ModelDb.Monster<Osty>().Id.ToString(),
      ModelDb.Monster<PaelsLegion>().Id.ToString(),
      ModelDb.Monster<Byrdpip>().Id.ToString()
    };
    if (!(saveData.GetRawNode("enemy_stats") is JsonArray rawNode))
      return;
    List<JsonNode> jsonNodeList = new List<JsonNode>();
    foreach (JsonNode jsonNode1 in rawNode)
    {
      if (jsonNode1 != null)
      {
        JsonNode jsonNode2 = jsonNode1["enemy_id"];
        if (jsonNode2 != null && ((IEnumerable<string>) source).Contains<string>(jsonNode2.GetValue<string>()))
          jsonNodeList.Add(jsonNode1);
      }
    }
    foreach (JsonNode jsonNode in jsonNodeList)
      rawNode.Remove(jsonNode);
  }
}
