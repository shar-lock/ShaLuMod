// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Cards.Abundance
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

public sealed class Abundance : CardModel
{
  public Abundance()
    : base(1, CardType.Skill, CardRarity.Ancient, TargetType.Self)
  {
  }

  public override bool CanBeGeneratedInCombat => false;

  public override IEnumerable<CardKeyword> CanonicalKeywords
  {
    get
    {
      return (IEnumerable<CardKeyword>) new \u003C\u003Ez__ReadOnlySingleElementList<CardKeyword>(CardKeyword.Exhaust);
    }
  }

  protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
  {
    List<CardModel> list = CardFactory.GetDistinctForCombat(this.Owner, this.Owner.Character.CardPool.GetUnlockedCards(this.Owner.UnlockState, this.Owner.RunState.CardMultiplayerConstraint).Where<CardModel>((Func<CardModel, bool>) (c => c.Type == CardType.Power)), 3, this.Owner.RunState.Rng.CombatCardGeneration).ToList<CardModel>();
    if (this.IsUpgraded)
    {
      foreach (CardModel card in list)
        CardCmd.Upgrade(card);
    }
    CardModel card1 = await CardSelectCmd.FromChooseACardScreen(choiceContext, (IReadOnlyList<CardModel>) list, this.Owner);
    if (card1 == null)
      return;
    card1.SetToFreeThisTurn();
    CardPileAddResult combat = await CardPileCmd.AddGeneratedCardToCombat(card1, PileType.Hand, this.Owner);
  }
}
