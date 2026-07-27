// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Cards.Discovery
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Cards;

public sealed class Discovery : CardModel
{
  private CardModel? _mockSelectedCard;

  public Discovery()
    : base(1, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
  {
  }

  public override IEnumerable<CardKeyword> CanonicalKeywords
  {
    get
    {
      return (IEnumerable<CardKeyword>) new \u003C\u003Ez__ReadOnlySingleElementList<CardKeyword>(CardKeyword.Exhaust);
    }
  }

  protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
  {
    CardModel card;
    if (this._mockSelectedCard == null)
      card = await CardSelectCmd.FromChooseACardScreen(choiceContext, (IReadOnlyList<CardModel>) CardFactory.GetDistinctForCombat(this.Owner, this.Owner.Character.CardPool.GetUnlockedCards(this.Owner.UnlockState, this.Owner.RunState.CardMultiplayerConstraint), 3, this.Owner.RunState.Rng.CombatCardGeneration).ToList<CardModel>(), this.Owner, true);
    else
      card = this._mockSelectedCard;
    if (card == null)
      return;
    card.SetToFreeThisTurn();
    CardPileAddResult combat = await CardPileCmd.AddGeneratedCardToCombat(card, PileType.Hand, this.Owner);
  }

  protected override void OnUpgrade() => this.RemoveKeyword(CardKeyword.Exhaust);

  public void MockSelectedCard(CardModel card)
  {
    this.AssertMutable();
    this._mockSelectedCard = card;
  }
}
