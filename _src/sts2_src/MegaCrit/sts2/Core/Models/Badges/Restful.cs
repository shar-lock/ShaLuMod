// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Badges.Restful
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.Runs.History;
using MegaCrit.Sts2.Core.Saves;
using System.Collections.Generic;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Badges;

public class Restful(SerializableRun run, bool won, ulong playerId) : Badge(run, won, playerId, "RESTFUL", true, false)
{
  public override BadgeRarity Rarity => BadgeRarity.Bronze;

  public override bool IsObtained()
  {
    int num = 0;
    foreach (List<MapPointHistoryEntry> pointHistoryEntryList in this._run.MapPointHistory)
    {
      foreach (MapPointHistoryEntry pointHistoryEntry in pointHistoryEntryList)
      {
        foreach (MapPointRoomHistoryEntry room in pointHistoryEntry.Rooms)
        {
          if (room.RoomType == RoomType.RestSite)
          {
            ++num;
            foreach (PlayerMapPointHistoryEntry playerStat in pointHistoryEntry.PlayerStats)
            {
              if ((long) playerStat.PlayerId == (long) this._localPlayer.NetId && !playerStat.RestSiteChoices.Contains("HEAL"))
                return false;
            }
          }
        }
      }
    }
    return num > 0;
  }
}
