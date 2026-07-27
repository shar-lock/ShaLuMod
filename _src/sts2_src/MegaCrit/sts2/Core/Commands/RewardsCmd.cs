// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Commands.RewardsCmd
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Rewards;
using MegaCrit.Sts2.Core.Rooms;
using System.Collections.Generic;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Commands;

public static class RewardsCmd
{
  public static async Task OfferForRoomEnd(Player player, AbstractRoom room)
  {
    RewardsSet rewardsSet;
    if (room is CombatRoom combatRoom)
    {
      EncounterModel encounter = combatRoom.Encounter;
      if (encounter != null && !encounter.ShouldGiveRewards)
      {
        rewardsSet = new RewardsSet(player).EmptyForRoom(room);
        goto label_4;
      }
    }
    rewardsSet = new RewardsSet(player).WithRewardsFromRoom(room);
label_4:
    await rewardsSet.Offer();
  }

  public static async Task OfferCustom(Player player, List<Reward> rewards)
  {
    await new RewardsSet(player).WithCustomRewards(rewards).Offer();
  }

  public static async Task<RewardsSet> GenerateForRoomEnd(Player player, AbstractRoom room)
  {
    RewardsSet set = new RewardsSet(player).WithRewardsFromRoom(room);
    await set.GenerateWithoutOffering();
    RewardsSet forRoomEnd = set;
    set = (RewardsSet) null;
    return forRoomEnd;
  }

  public static async Task<RewardsSet> GenerateCustom(Player player, List<Reward> rewards)
  {
    RewardsSet set = new RewardsSet(player).WithCustomRewards(rewards);
    await set.GenerateWithoutOffering();
    RewardsSet custom = set;
    set = (RewardsSet) null;
    return custom;
  }
}
