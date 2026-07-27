// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Relics.MassiveScroll
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.Runs.History;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Relics;

public sealed class MassiveScroll : RelicModel
{
  public override RelicRarity Rarity => RelicRarity.Ancient;

  public override bool IsAllowed(IRunState runState) => runState.Players.Count > 1;

  public override async Task AfterObtained()
  {
    // ISSUE: object of a compiler-generated type is created
    List<CardModel> options = CardFactory.CreateForReward(this.Owner, 3, new CardCreationOptions((IEnumerable<CardPoolModel>) new \u003C\u003Ez__ReadOnlyArray<CardPoolModel>(new CardPoolModel[2]
    {
      this.Owner.Character.CardPool,
      (CardPoolModel) ModelDb.CardPool<ColorlessCardPool>()
    }), CardCreationSource.Other, CardRarityOddsType.RegularEncounter, (Func<CardModel, bool>) (c => c.MultiplayerConstraint == CardMultiplayerConstraint.MultiplayerOnly))).Select<CardCreationResult, CardModel>((Func<CardCreationResult, CardModel>) (r => r.Card)).ToList<CardModel>();
    CardModel chosenCard = await CardSelectCmd.FromChooseACardScreen((PlayerChoiceContext) new BlockingPlayerChoiceContext(), (IReadOnlyList<CardModel>) options, this.Owner, true);
    if (chosenCard != null)
      CardCmd.PreviewCardPileAdd(await CardPileCmd.Add(chosenCard, PileType.Deck));
    foreach (CardModel card in options)
    {
      if (card != chosenCard)
        this.Owner.RunState.CurrentMapPointHistoryEntry?.GetEntry(this.Owner.NetId).CardChoices.Add(new CardChoiceHistoryEntry(card, false));
    }
    options = (List<CardModel>) null;
    chosenCard = (CardModel) null;
  }
}
