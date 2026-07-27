// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Relics.ScrollBoxes
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Hooks;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.Characters;
using MegaCrit.Sts2.Core.Random;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.TestSupport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Relics;

public sealed class ScrollBoxes : RelicModel
{
  private const int _clawBundleChancePercent = 1;

  public override RelicRarity Rarity => RelicRarity.Ancient;

  public override bool IsAllowedAtNeow(Player player)
  {
    return ScrollBoxes.CanGenerateBundles(player) && base.IsAllowedAtNeow(player);
  }

  public override async Task AfterObtained()
  {
    List<IReadOnlyList<CardModel>> randomBundles = ScrollBoxes.GenerateRandomBundles(this.Owner);
    List<IReadOnlyList<CardModel>> bundles = new List<IReadOnlyList<CardModel>>();
    foreach (IReadOnlyList<CardModel> source in randomBundles)
      bundles.Add((IReadOnlyList<CardModel>) source.Select<CardModel, CardModel>((Func<CardModel, CardModel>) (c => this.Owner.RunState.CreateCard(c, this.Owner))).ToList<CardModel>());
    foreach (CardModel card in await CardSelectCmd.FromChooseABundleScreen(this.Owner, (IReadOnlyList<IReadOnlyList<CardModel>>) bundles))
    {
      CardPileAddResult cardPileAddResult = await CardPileCmd.Add(card, PileType.Deck);
    }
  }

  public static bool CanGenerateBundles(Player player)
  {
    IEnumerable<CardModel> unlockedCards = ScrollBoxes.GetCardPool(player.Character).GetUnlockedCards(player.UnlockState, player.RunState.CardMultiplayerConstraint);
    int num1 = unlockedCards.Count<CardModel>((Func<CardModel, bool>) (c => c.Rarity == CardRarity.Common));
    int num2 = unlockedCards.Count<CardModel>((Func<CardModel, bool>) (c => c.Rarity == CardRarity.Uncommon));
    return num1 >= 4 && num2 >= 2;
  }

  public static List<IReadOnlyList<CardModel>> GenerateRandomBundles(Player player)
  {
    Rng rewards = player.PlayerRng.Rewards;
    bool flag = player.Character is Defect;
    CardPoolModel cardPool = ScrollBoxes.GetCardPool(player.Character);
    // ISSUE: object of a compiler-generated type is created
    CardCreationOptions options1 = CardCreationOptions.ForNonCombatWithUniformOdds((IEnumerable<CardPoolModel>) new \u003C\u003Ez__ReadOnlySingleElementList<CardPoolModel>(cardPool), (Func<CardModel, bool>) (c => c.Rarity == CardRarity.Common)).WithFlags(CardCreationFlags.NoRarityModification);
    CardCreationOptions cardCreationOptions1 = Hook.ModifyCardRewardCreationOptions(player.RunState, player, options1);
    // ISSUE: object of a compiler-generated type is created
    CardCreationOptions options2 = CardCreationOptions.ForNonCombatWithUniformOdds((IEnumerable<CardPoolModel>) new \u003C\u003Ez__ReadOnlySingleElementList<CardPoolModel>(cardPool), (Func<CardModel, bool>) (c => c.Rarity == CardRarity.Uncommon)).WithFlags(CardCreationFlags.NoRarityModification);
    CardCreationOptions cardCreationOptions2 = Hook.ModifyCardRewardCreationOptions(player.RunState, player, options2);
    List<CardModel> list1 = cardCreationOptions1.GetPossibleCards(player).ToList<CardModel>();
    List<CardModel> list2 = cardCreationOptions2.GetPossibleCards(player).ToList<CardModel>();
    List<IReadOnlyList<CardModel>> randomBundles = new List<IReadOnlyList<CardModel>>();
    HashSet<ModelId> usedCardIds = new HashSet<ModelId>();
    for (int index1 = 0; index1 < 2; ++index1)
    {
      if (flag && rewards.NextInt(100) < 1)
      {
        CardModel cardModel = (CardModel) ModelDb.Card<Claw>();
        // ISSUE: object of a compiler-generated type is created
        randomBundles.Add((IReadOnlyList<CardModel>) new \u003C\u003Ez__ReadOnlyArray<CardModel>(new CardModel[3]
        {
          cardModel,
          cardModel,
          cardModel
        }));
      }
      else
      {
        List<CardModel> cardModelList = new List<CardModel>();
        List<CardModel> list3 = list1.Where<CardModel>((Func<CardModel, bool>) (c => !usedCardIds.Contains(c.Id))).ToList<CardModel>();
        for (int index2 = 0; index2 < 2; ++index2)
        {
          CardModel cardModel = rewards.NextItem<CardModel>((IEnumerable<CardModel>) list3);
          cardModelList.Add(cardModel);
          usedCardIds.Add(cardModel.Id);
          list3.Remove(cardModel);
        }
        List<CardModel> list4 = list2.Where<CardModel>((Func<CardModel, bool>) (c => !usedCardIds.Contains(c.Id))).ToList<CardModel>();
        CardModel cardModel1 = rewards.NextItem<CardModel>((IEnumerable<CardModel>) list4);
        cardModelList.Add(cardModel1);
        usedCardIds.Add(cardModel1.Id);
        randomBundles.Add((IReadOnlyList<CardModel>) cardModelList);
      }
    }
    return randomBundles;
  }

  private static CardPoolModel GetCardPool(CharacterModel character)
  {
    return TestMode.IsOn && character is Deprived ? ModelDb.Character<Ironclad>().CardPool : character.CardPool;
  }
}
