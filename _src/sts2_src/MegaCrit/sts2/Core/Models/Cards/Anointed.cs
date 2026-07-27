// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Cards.Anointed
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Cards;

public sealed class Anointed : CardModel
{
  public Anointed()
    : base(1, CardType.Skill, CardRarity.Rare, TargetType.Self)
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
    int count = CardPile.MaxCardsInHand - PileType.Hand.GetPile(this.Owner).Cards.Count;
    IReadOnlyList<CardPileAddResult> cardPileAddResultList = await CardPileCmd.Add((IEnumerable<CardModel>) PileType.Draw.GetPile(this.Owner).Cards.Where<CardModel>((Func<CardModel, bool>) (c => c.Rarity == CardRarity.Rare)).TakeRandom<CardModel>(count, this.Owner.RunState.Rng.CombatCardSelection).ToList<CardModel>(), PileType.Hand);
  }

  protected override void OnUpgrade() => this.AddKeyword(CardKeyword.Retain);
}
