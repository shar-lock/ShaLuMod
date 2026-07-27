// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Runs.History.MapPointHistoryEntry
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Map;
using MegaCrit.Sts2.Core.Multiplayer.Serialization;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Saves.Runs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;

#nullable enable
namespace MegaCrit.Sts2.Core.Runs.History;

public class MapPointHistoryEntry : IPacketSerializable
{
  [JsonPropertyName("map_point_type")]
  public MapPointType MapPointType { get; set; }

  [JsonPropertyName("rooms")]
  [JsonSerializeCondition(SerializationCondition.SaveIfNotCollectionEmptyOrNull)]
  public List<MapPointRoomHistoryEntry> Rooms { get; set; } = new List<MapPointRoomHistoryEntry>();

  [JsonPropertyName("player_stats")]
  public List<PlayerMapPointHistoryEntry> PlayerStats { get; set; } = new List<PlayerMapPointHistoryEntry>();

  public MapPointHistoryEntry()
  {
  }

  public MapPointHistoryEntry(MapPointType mapPointType, IPlayerCollection playerCollection)
  {
    this.MapPointType = mapPointType;
    foreach (Player player in (IEnumerable<Player>) playerCollection.Players)
      this.PlayerStats.Add(new PlayerMapPointHistoryEntry()
      {
        PlayerId = player.NetId
      });
  }

  public PlayerMapPointHistoryEntry GetEntry(ulong playerId)
  {
    return this.PlayerStats.FirstOrDefault<PlayerMapPointHistoryEntry>((Func<PlayerMapPointHistoryEntry, bool>) (e => (long) e.PlayerId == (long) playerId)) ?? throw new InvalidOperationException($"Player with ID {playerId} not found in player stats for this run history! We have {string.Join<ulong>(",", this.PlayerStats.Select<PlayerMapPointHistoryEntry, ulong>((Func<PlayerMapPointHistoryEntry, ulong>) (p => p.PlayerId)))}");
  }

  public bool HasRoomOfType(RoomType roomType)
  {
    foreach (MapPointRoomHistoryEntry room in this.Rooms)
    {
      if (room.RoomType == roomType)
        return true;
    }
    return false;
  }

  public IEnumerable<MapPointRoomHistoryEntry> GetRoomsOfType(RoomType roomType)
  {
    foreach (MapPointRoomHistoryEntry room in this.Rooms)
    {
      if (room.RoomType == roomType)
        yield return room;
    }
  }

  public MapPointRoomHistoryEntry? FirstRoomOfType(RoomType roomType)
  {
    foreach (MapPointRoomHistoryEntry room in this.Rooms)
    {
      if (room.RoomType == roomType)
        return room;
    }
    return (MapPointRoomHistoryEntry) null;
  }

  public void Serialize(PacketWriter writer)
  {
    writer.WriteEnum<MapPointType>(this.MapPointType);
    writer.WriteList<MapPointRoomHistoryEntry>((IReadOnlyList<MapPointRoomHistoryEntry>) this.Rooms);
    writer.WriteList<PlayerMapPointHistoryEntry>((IReadOnlyList<PlayerMapPointHistoryEntry>) this.PlayerStats);
  }

  public void Deserialize(PacketReader reader)
  {
    this.MapPointType = reader.ReadEnum<MapPointType>();
    this.Rooms = reader.ReadList<MapPointRoomHistoryEntry>();
    this.PlayerStats = reader.ReadList<PlayerMapPointHistoryEntry>();
  }

  public MapPointHistoryEntry Anonymized()
  {
    return new MapPointHistoryEntry()
    {
      MapPointType = this.MapPointType,
      Rooms = this.Rooms,
      PlayerStats = this.PlayerStats.Select<PlayerMapPointHistoryEntry, PlayerMapPointHistoryEntry>((Func<PlayerMapPointHistoryEntry, PlayerMapPointHistoryEntry>) (p => p.Anonymized())).ToList<PlayerMapPointHistoryEntry>()
    };
  }
}
