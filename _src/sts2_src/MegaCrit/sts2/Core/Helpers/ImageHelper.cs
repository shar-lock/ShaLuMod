// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Helpers.ImageHelper
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Map;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Rooms;

#nullable enable
namespace MegaCrit.Sts2.Core.Helpers;

public static class ImageHelper
{
  public static string GetImagePath(string innerPath)
  {
    if (innerPath.StartsWith('/'))
    {
      string str = innerPath;
      innerPath = str.Substring(1, str.Length - 1);
    }
    return "res://images/" + innerPath;
  }

  public static string? GetRoomIconPath(
    MapPointType mapPointType,
    RoomType roomType,
    ModelId? modelId)
  {
    if (mapPointType == MapPointType.Unassigned || roomType == RoomType.Map)
      return (string) null;
    string roomIconSuffix = ImageHelper.GetRoomIconSuffix(mapPointType, roomType, modelId);
    return roomIconSuffix == null ? (string) null : ImageHelper.GetImagePath($"ui/run_history/{roomIconSuffix}.png");
  }

  public static string? GetRoomIconOutlinePath(
    MapPointType mapPointType,
    RoomType roomType,
    ModelId? modelId)
  {
    if (mapPointType == MapPointType.Unassigned || roomType == RoomType.Map)
      return (string) null;
    string roomIconSuffix = ImageHelper.GetRoomIconSuffix(mapPointType, roomType, modelId);
    return roomIconSuffix == null ? (string) null : ImageHelper.GetImagePath($"ui/run_history/{roomIconSuffix}_outline.png");
  }

  private static string? GetRoomIconSuffix(
    MapPointType mapPointType,
    RoomType roomType,
    ModelId? modelId)
  {
    if (modelId != (ModelId) null)
      return modelId.Entry.ToLowerInvariant();
    if (roomType == RoomType.Boss)
      return (string) null;
    string lowerInvariant = StringHelper.Slugify(roomType.ToString()).ToLowerInvariant();
    return mapPointType == MapPointType.Unknown && roomType != RoomType.Event ? "unknown_" + lowerInvariant : lowerInvariant;
  }
}
