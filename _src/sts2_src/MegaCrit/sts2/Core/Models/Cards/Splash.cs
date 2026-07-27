// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Cards.Splash
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Cards;

public sealed class Splash : CardModel
{
  private CardModel? _mockGeneratedCard;

  public Splash()
    : base(1, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
  {
  }

  protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
  {
    CardModel card1;
    if (this._mockGeneratedCard == null)
    {
      List<CardPoolModel> list1 = this.Owner.UnlockState.CharacterCardPools.ToList<CardPoolModel>();
      if (list1.Count > 1)
        list1.Remove(this.Owner.Character.CardPool);
      List<CardModel> list2 = CardFactory.GetDistinctForCombat(this.Owner, list1.SelectMany<CardPoolModel, CardModel>((Func<CardPoolModel, IEnumerable<CardModel>>) (c => c.GetUnlockedCards(this.Owner.UnlockState, this.Owner.RunState.CardMultiplayerConstraint))).Where<CardModel>((Func<CardModel, bool>) (c => c.Type == CardType.Attack)), 3, this.Owner.RunState.Rng.CombatCardGeneration).ToList<CardModel>();
      if (this.IsUpgraded)
      {
        foreach (CardModel card2 in list2)
          CardCmd.Upgrade(card2);
      }
      card1 = await CardSelectCmd.FromChooseACardScreen(choiceContext, (IReadOnlyList<CardModel>) list2, this.Owner, true);
    }
    else
    {
      card1 = this._mockGeneratedCard;
      if (this.IsUpgraded)
        CardCmd.Upgrade(card1);
    }
    if (card1 == null)
      return;
    card1.SetToFreeThisTurn();
    CardPileAddResult combat = await CardPileCmd.AddGeneratedCardToCombat(card1, PileType.Hand, this.Owner);
  }

  public void MockGeneratedCard(CardModel card)
  {
    this.AssertMutable();
    this._mockGeneratedCard = card;
  }
}
