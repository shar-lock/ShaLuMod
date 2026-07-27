// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Runs.RunLocation
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Map;
using MegaCrit.Sts2.Core.Multiplayer.Serialization;
using System;
using System.Collections.Generic;

#nullable enable
namespace MegaCrit.Sts2.Core.Runs;

public struct RunLocation : IEquatable<RunLocation>, IComparable<RunLocation>, IPacketSerializable
{
  public MapLocation mapLocation;
  public int? roomId;

  public RunLocation(MapLocation mapLocation, int? roomId)
  {
    this.mapLocation = mapLocation;
    this.roomId = roomId;
  }

  public RunLocation(int actIndex, MapCoord? coord, int? roomId)
  {
    this.mapLocation = new MapLocation(coord, actIndex);
    this.roomId = roomId;
  }

  public void Serialize(PacketWriter writer)
  {
    writer.WriteBool(this.roomId.HasValue);
    if (this.roomId.HasValue)
      writer.WriteInt(this.roomId.Value, 4);
    writer.Write<MapLocation>(this.mapLocation);
  }

  public void Deserialize(PacketReader reader)
  {
    if (reader.ReadBool())
      this.roomId = new int?(reader.ReadInt(4));
    this.mapLocation = reader.Read<MapLocation>();
  }

  public static bool operator ==(RunLocation first, RunLocation second) => first.Equals(second);

  public static bool operator !=(RunLocation first, RunLocation second) => !(first == second);

  public bool Equals(RunLocation other)
  {
    int? roomId1 = this.roomId;
    int? roomId2 = other.roomId;
    return roomId1.GetValueOrDefault() == roomId2.GetValueOrDefault() & roomId1.HasValue == roomId2.HasValue && this.mapLocation == other.mapLocation;
  }

  public override bool Equals(object? obj) => obj is RunLocation other && this.Equals(other);

  public override int GetHashCode()
  {
    int? roomId = this.roomId;
    int actIndex = this.mapLocation.actIndex;
    ref MapCoord? local1 = ref this.mapLocation.coord;
    int? nullable1 = local1.HasValue ? new int?(local1.GetValueOrDefault().col) : new int?();
    ref MapCoord? local2 = ref this.mapLocation.coord;
    int? nullable2 = local2.HasValue ? new int?(local2.GetValueOrDefault().row) : new int?();
    return (roomId, actIndex, nullable1, nullable2).GetHashCode();
  }

  public int CompareTo(RunLocation other)
  {
    return this.mapLocation != other.mapLocation ? this.mapLocation.CompareTo(other.mapLocation) : Comparer<int?>.Default.Compare(this.roomId, other.roomId);
  }

  public override string ToString() => $"{this.mapLocation} room {this.roomId}";
}
