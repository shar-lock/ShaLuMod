// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Unlocks.SerializableUnlockState
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Multiplayer.Serialization;
using System.Collections.Generic;
using System.Text.Json.Serialization;

#nullable enable
namespace MegaCrit.Sts2.Core.Unlocks;

public class SerializableUnlockState : IPacketSerializable
{
  [JsonPropertyName("unlocked_epochs")]
  public List<string> UnlockedEpochs { get; set; } = new List<string>();

  [JsonPropertyName("encounters_seen")]
  public List<ModelId> EncountersSeen { get; set; } = new List<ModelId>();

  [JsonPropertyName("number_of_runs")]
  public int NumberOfRuns { get; set; }

  public void Serialize(PacketWriter writer)
  {
    writer.WriteInt(this.UnlockedEpochs.Count);
    foreach (string unlockedEpoch in this.UnlockedEpochs)
      writer.WriteEpochId(unlockedEpoch);
    writer.WriteModelEntriesInList((IReadOnlyCollection<ModelId>) this.EncountersSeen);
    writer.WriteInt(this.NumberOfRuns, 16 /*0x10*/);
  }

  public void Deserialize(PacketReader reader)
  {
    int num = reader.ReadInt();
    for (int index = 0; index < num; ++index)
      this.UnlockedEpochs.Add(reader.ReadEpochId());
    this.EncountersSeen = reader.ReadModelIdListAssumingType<EncounterModel>();
    this.NumberOfRuns = reader.ReadInt(16 /*0x10*/);
  }
}
