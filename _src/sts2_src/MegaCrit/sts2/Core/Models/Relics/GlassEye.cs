// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Relics.GlassEye
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Rewards;
using MegaCrit.Sts2.Core.Runs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Relics;

public sealed class GlassEye : RelicModel
{
  public override RelicRarity Rarity => RelicRarity.Ancient;

  public override async Task AfterObtained()
  {
    List<Reward> rewards = new List<Reward>();
    CardRarity[] cardRarityArray = new CardRarity[5]
    {
      CardRarity.Common,
      CardRarity.Common,
      CardRarity.Uncommon,
      CardRarity.Uncommon,
      CardRarity.Rare
    };
    foreach (CardRarity cardRarity in cardRarityArray)
    {
      CardRarity rarity = cardRarity;
      // ISSUE: object of a compiler-generated type is created
      CardCreationOptions options = CardCreationOptions.ForNonCombatWithUniformOdds((IEnumerable<CardPoolModel>) new \u003C\u003Ez__ReadOnlySingleElementList<CardPoolModel>(this.Owner.Character.CardPool), (Func<CardModel, bool>) (c => c.Rarity == rarity)).WithFlags(CardCreationFlags.NoRarityModification);
      rewards.Add((Reward) new CardReward(options, 3, this.Owner));
    }
    await RewardsCmd.OfferCustom(this.Owner, rewards);
  }
}
