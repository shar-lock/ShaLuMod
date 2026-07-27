// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Map.MapCoord
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Multiplayer.Serialization;
using System;
using System.Text.Json.Serialization;

#nullable enable
namespace MegaCrit.Sts2.Core.Map;

[Serializable]
public struct MapCoord(int col, int row) : 
  IEquatable<MapCoord>,
  IComparable<MapCoord>,
  IPacketSerializable
{
  [JsonInclude]
  public int col = col;
  [JsonInclude]
  public int row = row;

  public void Serialize(PacketWriter writer)
  {
    writer.WriteByte((byte) this.col);
    writer.WriteByte((byte) this.row);
  }

  public void Deserialize(PacketReader reader)
  {
    this.col = (int) reader.ReadByte();
    this.row = (int) reader.ReadByte();
  }

  public static bool operator ==(MapCoord first, MapCoord second) => first.Equals(second);

  public static bool operator !=(MapCoord first, MapCoord second) => !(first == second);

  public bool Equals(MapCoord other) => this.col == other.col && this.row == other.row;

  public override bool Equals(object? obj) => obj is MapCoord other && this.Equals(other);

  public override int GetHashCode() => (this.col, this.row).GetHashCode();

  public int CompareTo(MapCoord other) => (this.col, this.row).CompareTo((other.col, other.row));

  public override string ToString() => $"MapCoord ({this.col}, {this.row})";
}
