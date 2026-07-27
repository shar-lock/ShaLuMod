// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Relics.Driftwood
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Rewards;
using MegaCrit.Sts2.Core.Rooms;
using System.Collections.Generic;
using System.Linq;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Relics;

public class Driftwood : RelicModel
{
  public override RelicRarity Rarity => RelicRarity.Ancient;

  public override bool TryModifyRewardsLate(Player player, List<Reward> rewards, AbstractRoom? room)
  {
    if (player != this.Owner)
      return false;
    foreach (CardReward cardReward in rewards.OfType<CardReward>())
      cardReward.CanReroll = true;
    return true;
  }
}
