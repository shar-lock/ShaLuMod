// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Potions.OrobicAcid
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Potions;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Potions;

public sealed class OrobicAcid : PotionModel
{
  public override PotionRarity Rarity => PotionRarity.Rare;

  public override PotionUsage Usage => PotionUsage.CombatOnly;

  public override TargetType TargetType => TargetType.AnyPlayer;

  protected override async Task OnUse(PlayerChoiceContext choiceContext, Creature? target)
  {
    PotionModel.AssertValidForTargetedPotion(target);
    Player player = target.Player;
    List<CardModel> cards = new List<CardModel>();
    cards.AddRange(CardFactory.GetDistinctForCombat(player, player.Character.CardPool.GetUnlockedCards(player.UnlockState, player.RunState.CardMultiplayerConstraint).Where<CardModel>((Func<CardModel, bool>) (c => c.Type == CardType.Attack)), 1, player.RunState.Rng.CombatCardGeneration));
    cards.AddRange(CardFactory.GetDistinctForCombat(player, player.Character.CardPool.GetUnlockedCards(player.UnlockState, player.RunState.CardMultiplayerConstraint).Where<CardModel>((Func<CardModel, bool>) (c => c.Type == CardType.Skill)), 1, player.RunState.Rng.CombatCardGeneration));
    cards.AddRange(CardFactory.GetDistinctForCombat(player, player.Character.CardPool.GetUnlockedCards(player.UnlockState, player.RunState.CardMultiplayerConstraint).Where<CardModel>((Func<CardModel, bool>) (c => c.Type == CardType.Power)), 1, player.RunState.Rng.CombatCardGeneration));
    foreach (CardModel cardModel in cards)
      cardModel.SetToFreeThisTurn();
    IReadOnlyList<CardPileAddResult> combat = await CardPileCmd.AddGeneratedCardsToCombat((IEnumerable<CardModel>) cards, PileType.Hand, this.Owner);
  }
}
