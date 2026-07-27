// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Relics.VexingPuzzlebox
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using System.Linq;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Relics;

public sealed class VexingPuzzlebox : RelicModel
{
  public override RelicRarity Rarity => RelicRarity.Rare;

  public override async Task AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player)
  {
    if (player != this.Owner || this.Owner.PlayerCombatState.TurnNumber != 1)
      return;
    this.Flash();
    CardModel card = CardFactory.GetDistinctForCombat(this.Owner, this.Owner.Character.CardPool.GetUnlockedCards(this.Owner.UnlockState, this.Owner.RunState.CardMultiplayerConstraint), 1, this.Owner.RunState.Rng.CombatCardGeneration).First<CardModel>();
    card.SetToFreeThisTurn();
    CardPileAddResult combat = await CardPileCmd.AddGeneratedCardToCombat(card, PileType.Hand, player);
  }
}
