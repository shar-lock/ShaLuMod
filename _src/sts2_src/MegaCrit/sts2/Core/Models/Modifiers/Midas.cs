// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Modifiers.Midas
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.RestSite;
using MegaCrit.Sts2.Core.Rewards;
using MegaCrit.Sts2.Core.Rooms;
using System.Collections.Generic;
using System.Linq;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Modifiers;

public class Midas : ModifierModel
{
  public override bool TryModifyRewardsLate(Player player, List<Reward> rewards, AbstractRoom? room)
  {
    List<Reward> collection = new List<Reward>();
    foreach (Reward reward in rewards)
    {
      if (reward is GoldReward goldReward)
        collection.Add((Reward) new GoldReward(goldReward.Amount * 2, player));
      else
        collection.Add(reward);
    }
    rewards.Clear();
    rewards.AddRange((IEnumerable<Reward>) collection);
    return true;
  }

  public override bool TryModifyRestSiteOptions(Player player, ICollection<RestSiteOption> options)
  {
    foreach (SmithRestSiteOption smithRestSiteOption in options.OfType<SmithRestSiteOption>().ToList<SmithRestSiteOption>())
      options.Remove((RestSiteOption) smithRestSiteOption);
    return true;
  }
}
