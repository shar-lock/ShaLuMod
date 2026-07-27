// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Relics.DistinguishedCape
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Models.Cards;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Relics;

public sealed class DistinguishedCape : RelicModel
{
  private const string _cursesKey = "Curses";

  public override RelicRarity Rarity => RelicRarity.Ancient;

  public override bool HasUponPickupEffect => true;

  protected override IEnumerable<IHoverTip> ExtraHoverTips
  {
    get => HoverTipFactory.FromCardWithCardHoverTips<Apparition>();
  }

  protected override IEnumerable<DynamicVar> CanonicalVars
  {
    get
    {
      return (IEnumerable<DynamicVar>) new \u003C\u003Ez__ReadOnlyArray<DynamicVar>(new DynamicVar[2]
      {
        new DynamicVar("Curses", 2M),
        (DynamicVar) new CardsVar(3)
      });
    }
  }

  public override async Task AfterObtained()
  {
    List<CardModel> availableCurses = ModelDb.CardPool<CurseCardPool>().GetUnlockedCards(this.Owner.UnlockState, this.Owner.RunState.CardMultiplayerConstraint).Where<CardModel>((Func<CardModel, bool>) (c => c.CanBeGeneratedByModifiers)).OrderBy<CardModel, ModelId>((Func<CardModel, ModelId>) (c => c.Id)).ToList<CardModel>();
    List<CardPileAddResult> curseResults = new List<CardPileAddResult>();
    int i;
    for (i = 0; i < this.DynamicVars["Curses"].IntValue; ++i)
    {
      CardModel canonicalCard = this.Owner.RunState.Rng.Niche.NextItem<CardModel>((IEnumerable<CardModel>) availableCurses);
      availableCurses.Remove(canonicalCard);
      curseResults.Add(await CardPileCmd.Add(this.Owner.RunState.CreateCard(canonicalCard, this.Owner), PileType.Deck));
    }
    CardCmd.PreviewCardPileAdd((IReadOnlyList<CardPileAddResult>) curseResults, 2f);
    await Cmd.Wait(0.75f);
    List<CardPileAddResult> results = new List<CardPileAddResult>();
    for (i = 0; i < this.DynamicVars.Cards.IntValue; ++i)
    {
      CardModel card = (CardModel) this.Owner.RunState.CreateCard<Apparition>(this.Owner);
      List<CardPileAddResult> cardPileAddResultList = results;
      cardPileAddResultList.Add(await CardPileCmd.Add(card, PileType.Deck));
      cardPileAddResultList = (List<CardPileAddResult>) null;
    }
    CardCmd.PreviewCardPileAdd((IReadOnlyList<CardPileAddResult>) results, 2f);
    availableCurses = (List<CardModel>) null;
    curseResults = (List<CardPileAddResult>) null;
    results = (List<CardPileAddResult>) null;
  }
}
