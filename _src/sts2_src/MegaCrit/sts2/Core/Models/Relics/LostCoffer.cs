// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Relics.LostCoffer
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Rewards;
using MegaCrit.Sts2.Core.Runs;
using System.Collections.Generic;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Relics;

public sealed class LostCoffer : RelicModel
{
  public override RelicRarity Rarity => RelicRarity.Ancient;

  public override bool HasUponPickupEffect => true;

  public override async Task AfterObtained()
  {
    List<Reward> rewards = new List<Reward>();
    // ISSUE: object of a compiler-generated type is created
    CardCreationOptions options = new CardCreationOptions((IEnumerable<CardPoolModel>) new \u003C\u003Ez__ReadOnlySingleElementList<CardPoolModel>(this.Owner.Character.CardPool), CardCreationSource.Other, CardRarityOddsType.RegularEncounter);
    rewards.Add((Reward) new CardReward(options, 3, this.Owner));
    rewards.Add((Reward) new PotionReward(this.Owner));
    await RewardsCmd.OfferCustom(this.Owner, rewards);
  }
}
