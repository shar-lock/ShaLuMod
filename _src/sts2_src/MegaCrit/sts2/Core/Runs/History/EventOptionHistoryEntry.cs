// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Runs.History.EventOptionHistoryEntry
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Multiplayer.Serialization;
using MegaCrit.Sts2.Core.Saves.Runs;
using System.Collections.Generic;
using System.Text.Json.Serialization;

#nullable enable
namespace MegaCrit.Sts2.Core.Runs.History;

public struct EventOptionHistoryEntry : IPacketSerializable
{
  [JsonPropertyName("title")]
  public LocString Title { get; set; }

  [JsonInclude]
  [JsonPropertyName("variables")]
  [JsonConverter(typeof (LocStringVariablesJsonConverter))]
  [JsonSerializeCondition(SerializationCondition.SaveIfNotCollectionEmptyOrNull)]
  public Dictionary<string, object>? Variables { get; set; }

  public void Serialize(PacketWriter writer)
  {
    writer.WriteString(this.Title.LocTable);
    writer.WriteString(this.Title.LocEntryKey);
    writer.WriteBool(this.Variables != null);
    if (this.Variables == null)
      return;
    List<(string, SerializableDynamicVar)> valueTupleList = new List<(string, SerializableDynamicVar)>();
    foreach (KeyValuePair<string, object> variable in this.Variables)
    {
      SerializableDynamicVar? nullable = SerializableDynamicVar.FromDynamicVar(variable.Value);
      if (nullable.HasValue)
        valueTupleList.Add((variable.Key, nullable.Value));
    }
    writer.WriteInt(valueTupleList.Count);
    foreach ((string str, SerializableDynamicVar val) in valueTupleList)
    {
      writer.WriteString(str);
      writer.Write<SerializableDynamicVar>(val);
    }
  }

  public void Deserialize(PacketReader reader)
  {
    this.Title = new LocString(reader.ReadString(), reader.ReadString());
    if (!reader.ReadBool())
      return;
    this.Variables = new Dictionary<string, object>();
    int num = reader.ReadInt();
    for (int index = 0; index < num; ++index)
    {
      string str = reader.ReadString();
      this.Variables[str] = reader.Read<SerializableDynamicVar>().ToDynamicVar(str);
    }
  }
}
