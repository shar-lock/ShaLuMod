// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Multiplayer.Game.MapVote
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Map;
using MegaCrit.Sts2.Core.Multiplayer.Serialization;

#nullable enable
namespace MegaCrit.Sts2.Core.Multiplayer.Game;

public struct MapVote : IPacketSerializable
{
  public int mapGenerationCount;
  public MapCoord coord;

  public void Serialize(PacketWriter writer)
  {
    writer.WriteInt(this.mapGenerationCount, 4);
    writer.Write<MapCoord>(this.coord);
  }

  public void Deserialize(PacketReader reader)
  {
    this.mapGenerationCount = reader.ReadInt(4);
    this.coord = reader.Read<MapCoord>();
  }

  public override string ToString()
  {
    return $"{nameof (MapVote)} (gen: {this.mapGenerationCount} coord: ({this.coord.col}, {this.coord.row}))";
  }
}
