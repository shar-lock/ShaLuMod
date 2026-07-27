// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Relics.LargeCapsule
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.Characters;
using MegaCrit.Sts2.Core.TestSupport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Relics;

public sealed class LargeCapsule : RelicModel
{
  private const string _relicKey = "Relics";

  public override RelicRarity Rarity => RelicRarity.Ancient;

  protected override IEnumerable<DynamicVar> CanonicalVars
  {
    get
    {
      return (IEnumerable<DynamicVar>) new \u003C\u003Ez__ReadOnlySingleElementList<DynamicVar>((DynamicVar) new IntVar("Relics", 2M));
    }
  }

  public override async Task AfterObtained()
  {
    for (int i = 0; i < this.DynamicVars["Relics"].IntValue; ++i)
    {
      RelicModel relicModel = await RelicCmd.Obtain(RelicFactory.PullNextRelicFromFront(this.Owner).ToMutable(), this.Owner);
    }
    List<CardPileAddResult> cardPileAddResultList1 = new List<CardPileAddResult>(2);
    List<CardPileAddResult> cardPileAddResultList2 = cardPileAddResultList1;
    cardPileAddResultList2.Add(await CardPileCmd.Add(this.Owner.RunState.CreateCard(LargeCapsule.GetStrikeForCharacter(this.Owner.Character), this.Owner), PileType.Deck));
    List<CardPileAddResult> cardPileAddResultList3 = cardPileAddResultList1;
    cardPileAddResultList3.Add(await CardPileCmd.Add(this.Owner.RunState.CreateCard(LargeCapsule.GetDefendForCharacter(this.Owner.Character), this.Owner), PileType.Deck));
    List<CardPileAddResult> results = cardPileAddResultList1;
    cardPileAddResultList2 = (List<CardPileAddResult>) null;
    cardPileAddResultList3 = (List<CardPileAddResult>) null;
    cardPileAddResultList1 = (List<CardPileAddResult>) null;
    CardCmd.PreviewCardPileAdd((IReadOnlyList<CardPileAddResult>) results, 2f);
  }

  private static CardModel GetStrikeForCharacter(CharacterModel character)
  {
    return TestMode.IsOn && character is Deprived ? (CardModel) ModelDb.Card<StrikeIronclad>() : character.CardPool.AllCards.First<CardModel>((Func<CardModel, bool>) (c => c.Rarity == CardRarity.Basic && c.Tags.Contains<CardTag>(CardTag.Strike)));
  }

  private static CardModel GetDefendForCharacter(CharacterModel character)
  {
    return TestMode.IsOn && character is Deprived ? (CardModel) ModelDb.Card<DefendIronclad>() : character.CardPool.AllCards.First<CardModel>((Func<CardModel, bool>) (c => c.Rarity == CardRarity.Basic && c.Tags.Contains<CardTag>(CardTag.Defend)));
  }
}
