// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Relics.MummifiedHand
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Random;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Relics;

public sealed class MummifiedHand : RelicModel
{
  public override RelicRarity Rarity => RelicRarity.Rare;

  public override Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
  {
    if (!CombatManager.Instance.IsInProgress || cardPlay.Card.Owner != this.Owner || cardPlay.Card.Type != CardType.Power)
      return Task.CompletedTask;
    Rng combatCardSelection = this.Owner.RunState.Rng.CombatCardSelection;
    IReadOnlyList<CardModel> cards = PileType.Hand.GetPile(this.Owner).Cards;
    List<CardModel> list = cards.Where<CardModel>((Func<CardModel, bool>) (c => c.EnergyCost.GetWithModifiers(CostModifiers.None) > 0 || c.BaseStarCost > 0)).ToList<CardModel>();
    (((combatCardSelection.NextItem<CardModel>(list.Where<CardModel>((Func<CardModel, bool>) (c => c.CostsEnergyOrStars(true)))) ?? combatCardSelection.NextItem<CardModel>(cards.Where<CardModel>((Func<CardModel, bool>) (c => c.CostsEnergyOrStars(true))))) ?? combatCardSelection.NextItem<CardModel>((IEnumerable<CardModel>) list)) ?? combatCardSelection.NextItem<CardModel>((IEnumerable<CardModel>) cards))?.SetToFreeThisTurn();
    return Task.CompletedTask;
  }
}
