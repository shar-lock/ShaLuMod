// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Saves.Runs.SerializableRelicGrabBag
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Multiplayer.Serialization;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

#nullable enable
namespace MegaCrit.Sts2.Core.Saves.Runs;

public class SerializableRelicGrabBag : IPacketSerializable
{
  [JsonPropertyName("relic_id_lists")]
  public Dictionary<RelicRarity, List<ModelId>> RelicIdLists { get; set; } = new Dictionary<RelicRarity, List<ModelId>>();

  public void Serialize(PacketWriter writer)
  {
    writer.WriteInt(this.RelicIdLists.Count);
    foreach (RelicRarity relicRarity in Enum.GetValues<RelicRarity>())
    {
      List<ModelId> modelIdList;
      if (this.RelicIdLists.TryGetValue(relicRarity, out modelIdList))
      {
        writer.WriteEnum<RelicRarity>(relicRarity);
        writer.WriteModelEntriesInList((IReadOnlyCollection<ModelId>) modelIdList);
      }
    }
  }

  public void Deserialize(PacketReader reader)
  {
    int num = reader.ReadInt();
    for (int index = 0; index < num; ++index)
      this.RelicIdLists[reader.ReadEnum<RelicRarity>()] = reader.ReadModelIdListAssumingType<RelicModel>();
  }
}
