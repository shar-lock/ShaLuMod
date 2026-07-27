// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Badges.Healer
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

public class Healer(SerializableRun run, bool won, ulong playerId) : Badge(run, won, playerId, "HEALER", false, true)
{
  public override BadgeRarity Rarity
  {
    get
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
              foreach (PlayerMapPointHistoryEntry playerStat in pointHistoryEntry.PlayerStats)
              {
                if ((long) playerStat.PlayerId == (long) this._localPlayer.NetId && playerStat.RestSiteChoices.Contains("MEND"))
                  ++num;
              }
            }
          }
        }
      }
      BadgeRarity rarity;
      if (num < 3)
      {
        switch (num)
        {
          case 1:
            rarity = BadgeRarity.Bronze;
            break;
          case 2:
            rarity = BadgeRarity.Silver;
            break;
          default:
            rarity = BadgeRarity.None;
            break;
        }
      }
      else
        rarity = BadgeRarity.Gold;
      return rarity;
    }
  }

  public override bool IsObtained() => this.Rarity != 0;
}
