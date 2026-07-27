// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Rooms.RoomTypeExtensions
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

#nullable disable
namespace MegaCrit.Sts2.Core.Rooms;

public static class RoomTypeExtensions
{
  public static bool IsCombatRoom(this RoomType room)
  {
    bool flag;
    switch (room)
    {
      case RoomType.Monster:
      case RoomType.Elite:
      case RoomType.Boss:
        flag = true;
        break;
      default:
        flag = false;
        break;
    }
    return flag;
  }
}
