// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Relics.TinyMailbox
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Rewards;
using System.Collections.Generic;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Relics;

public sealed class TinyMailbox : RelicModel
{
  public override RelicRarity Rarity => RelicRarity.Uncommon;

  public override bool TryModifyRestSiteHealRewards(
    Player player,
    List<Reward> rewards,
    bool isMimicked)
  {
    if (player != this.Owner)
      return false;
    rewards.Add((Reward) new PotionReward(player));
    rewards.Add((Reward) new PotionReward(player));
    this.Flash();
    return true;
  }

  public override IReadOnlyList<LocString> ModifyExtraRestSiteHealText(
    Player player,
    IReadOnlyList<LocString> currentExtraText)
  {
    if (!LocalContext.IsMe(this.Owner))
      return currentExtraText;
    IReadOnlyList<LocString> locStringList = currentExtraText;
    int index = 0;
    LocString[] items = new LocString[1 + locStringList.Count];
    foreach (LocString locString in (IEnumerable<LocString>) locStringList)
    {
      items[index] = locString;
      ++index;
    }
    items[index] = this.AdditionalRestSiteHealText;
    // ISSUE: object of a compiler-generated type is created
    return (IReadOnlyList<LocString>) new \u003C\u003Ez__ReadOnlyArray<LocString>(items);
  }
}
