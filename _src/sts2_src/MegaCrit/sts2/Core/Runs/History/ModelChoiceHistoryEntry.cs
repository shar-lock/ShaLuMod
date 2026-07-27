// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Runs.History.ModelChoiceHistoryEntry
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Multiplayer.Serialization;
using System;
using System.Text.Json.Serialization;

#nullable enable
namespace MegaCrit.Sts2.Core.Runs.History;

[Serializable]
public struct ModelChoiceHistoryEntry(ModelId choice, bool wasPicked)
{
  [JsonPropertyName("choice")]
  public ModelId choice = choice;
  [JsonPropertyName("was_picked")]
  public bool wasPicked = wasPicked;

  public void Serialize<T>(PacketWriter writer) where T : AbstractModel
  {
    writer.WriteModelEntry(this.choice);
    writer.WriteBool(this.wasPicked);
  }

  public void Deserialize<T>(PacketReader reader) where T : AbstractModel
  {
    this.choice = reader.ReadModelIdAssumingType<T>();
    this.wasPicked = reader.ReadBool();
  }
}
