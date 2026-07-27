// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Runs.History.AncientChoiceHistoryEntry
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Multiplayer.Serialization;
using System;
using System.Text.Json.Serialization;

#nullable enable
namespace MegaCrit.Sts2.Core.Runs.History;

[Serializable]
public class AncientChoiceHistoryEntry : IPacketSerializable
{
  [JsonPropertyName("title")]
  public LocString Title { get; set; }

  [JsonPropertyName("was_chosen")]
  public bool WasChosen { get; set; }

  [JsonPropertyName("TextKey")]
  public string TextKey
  {
    get
    {
      string[] strArray = this.Title.LocEntryKey.Split(".", StringSplitOptions.None);
      return strArray[strArray.Length - 2];
    }
  }

  public AncientChoiceHistoryEntry() => this.Title = new LocString(string.Empty, string.Empty);

  public AncientChoiceHistoryEntry(LocString title, bool wasChosen)
  {
    this.Title = title;
    this.WasChosen = wasChosen;
  }

  public void Serialize(PacketWriter writer)
  {
    writer.WriteString(this.Title.LocTable);
    writer.WriteString(this.Title.LocEntryKey);
    writer.WriteBool(this.WasChosen);
  }

  public void Deserialize(PacketReader reader)
  {
    this.Title = new LocString(reader.ReadString(), reader.ReadString());
    this.WasChosen = reader.ReadBool();
  }
}
