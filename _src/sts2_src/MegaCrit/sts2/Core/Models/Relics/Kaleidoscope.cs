// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Relics.Kaleidoscope
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Rewards;
using MegaCrit.Sts2.Core.Runs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Relics;

public sealed class Kaleidoscope : RelicModel
{
  public override bool IsAllowedAtNeow(Player player)
  {
    return base.IsAllowedAtNeow(player) && player.UnlockState.CharacterCardPools.Count<CardPoolModel>() == ModelDb.AllCharacters.Count<CharacterModel>();
  }

  public override RelicRarity Rarity => RelicRarity.Ancient;

  public override bool HasUponPickupEffect => true;

  protected override IEnumerable<DynamicVar> CanonicalVars
  {
    get
    {
      return (IEnumerable<DynamicVar>) new \u003C\u003Ez__ReadOnlySingleElementList<DynamicVar>((DynamicVar) new CardsVar(2));
    }
  }

  public override async Task AfterObtained()
  {
    List<Reward> rewards = new List<Reward>();
    CardCreationOptions rerollOptions = CardCreationOptions.ForNonCombatWithDefaultOdds((IEnumerable<CardPoolModel>) Array.Empty<CardPoolModel>());
    for (int index = 0; index < this.DynamicVars.Cards.IntValue; ++index)
    {
      List<CardModel> cardsToOffer = new List<CardModel>();
      foreach (CardPoolModel cardPoolModel in this.Owner.UnlockState.CharacterCardPools.Where<CardPoolModel>((Func<CardPoolModel, bool>) (p => p != this.Owner.Character.CardPool)).ToList<CardPoolModel>().StableShuffle<CardPoolModel>(this.Owner.RunState.Rng.Niche).Take<CardPoolModel>(3))
      {
        // ISSUE: object of a compiler-generated type is created
        CardCreationOptions options = new CardCreationOptions((IEnumerable<CardPoolModel>) new \u003C\u003Ez__ReadOnlySingleElementList<CardPoolModel>(cardPoolModel), CardCreationSource.Other, CardRarityOddsType.RegularEncounter).WithFlags(CardCreationFlags.NoCardPoolModifications);
        cardsToOffer.Add(CardFactory.CreateForReward(this.Owner, 1, options).First<CardCreationResult>().Card);
      }
      rewards.Add((Reward) new CardReward((IEnumerable<CardModel>) cardsToOffer, CardCreationSource.Other, this.Owner, rerollOptions));
    }
    await RewardsCmd.OfferCustom(this.Owner, rewards);
  }
}
