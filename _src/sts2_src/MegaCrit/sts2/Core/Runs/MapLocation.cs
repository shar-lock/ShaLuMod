// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Runs.MapLocation
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Map;
using MegaCrit.Sts2.Core.Multiplayer.Serialization;
using System;
using System.Runtime.CompilerServices;

#nullable enable
namespace MegaCrit.Sts2.Core.Runs;

public struct MapLocation(MapCoord? coord, int actIndex) : 
  IEquatable<MapLocation>,
  IComparable<MapLocation>,
  IPacketSerializable
{
  public int actIndex = actIndex;
  public MapCoord? coord = coord;

  public void Serialize(PacketWriter writer)
  {
    writer.WriteInt(this.actIndex, 4);
    writer.WriteBool(this.coord.HasValue);
    if (!this.coord.HasValue)
      return;
    writer.Write<MapCoord>(this.coord.Value);
  }

  public void Deserialize(PacketReader reader)
  {
    this.actIndex = reader.ReadInt(4);
    if (!reader.ReadBool())
      return;
    this.coord = new MapCoord?(reader.Read<MapCoord>());
  }

  public static bool operator ==(MapLocation first, MapLocation second) => first.Equals(second);

  public static bool operator !=(MapLocation first, MapLocation second) => !(first == second);

  public bool Equals(MapLocation other)
  {
    if (this.actIndex != other.actIndex)
      return false;
    MapCoord? coord1 = this.coord;
    MapCoord? coord2 = other.coord;
    if (coord1.HasValue != coord2.HasValue)
      return false;
    return !coord1.HasValue || coord1.GetValueOrDefault() == coord2.GetValueOrDefault();
  }

  public override bool Equals(object? obj) => obj is MapLocation other && this.Equals(other);

  public override int GetHashCode()
  {
    int actIndex = this.actIndex;
    ref MapCoord? local1 = ref this.coord;
    int? nullable1 = local1.HasValue ? new int?(local1.GetValueOrDefault().col) : new int?();
    ref MapCoord? local2 = ref this.coord;
    int? nullable2 = local2.HasValue ? new int?(local2.GetValueOrDefault().row) : new int?();
    return (actIndex, nullable1, nullable2).GetHashCode();
  }

  public int CompareTo(MapLocation other)
  {
    if (this.actIndex != other.actIndex)
      return this.actIndex.CompareTo(other.actIndex);
    if (!this.coord.HasValue && !other.coord.HasValue)
      return 0;
    if (!this.coord.HasValue && other.coord.HasValue)
      return -1;
    return this.coord.HasValue && !other.coord.HasValue ? 1 : this.coord.Value.CompareTo(other.coord.Value);
  }

  public override string ToString()
  {
    DefaultInterpolatedStringHandler interpolatedStringHandler = new DefaultInterpolatedStringHandler(13, 2);
    interpolatedStringHandler.AppendLiteral("act ");
    interpolatedStringHandler.AppendFormatted<int>(this.actIndex);
    interpolatedStringHandler.AppendLiteral(" coord (");
    ref DefaultInterpolatedStringHandler local = ref interpolatedStringHandler;
    string str;
    if (!this.coord.HasValue)
      str = "null";
    else
      str = $"{this.coord.Value.col}, {this.coord.Value.row}";
    local.AppendFormatted(str);
    interpolatedStringHandler.AppendLiteral(")");
    return interpolatedStringHandler.ToStringAndClear();
  }
}
