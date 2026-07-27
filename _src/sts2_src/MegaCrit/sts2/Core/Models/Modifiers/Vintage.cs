// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Modifiers.Vintage
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Rewards;
using MegaCrit.Sts2.Core.Rooms;
using System.Collections.Generic;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Modifiers;

public class Vintage : ModifierModel
{
  public override bool TryModifyRewardsLate(Player player, List<Reward> rewards, AbstractRoom? room)
  {
    if (!(room is CombatRoom combatRoom) || combatRoom.Encounter.RoomType != RoomType.Monster)
      return false;
    for (int index = 0; index < rewards.Count; ++index)
    {
      if (rewards[index] is CardReward)
      {
        rewards.RemoveAt(index);
        RelicReward relicReward = new RelicReward(player);
        rewards.Insert(index, (Reward) relicReward);
      }
    }
    return true;
  }
}
