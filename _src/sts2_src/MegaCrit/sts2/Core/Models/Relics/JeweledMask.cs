// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Relics.JeweledMask
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Relics;

public sealed class JeweledMask : RelicModel
{
  public override RelicRarity Rarity => RelicRarity.Ancient;

  public override async Task BeforeHandDraw(
    Player player,
    PlayerChoiceContext choiceContext,
    ICombatState combatState)
  {
    if (player != this.Owner || this.Owner.PlayerCombatState.TurnNumber > 1)
      return;
    List<CardModel> list = PileType.Draw.GetPile(player).Cards.Where<CardModel>((Func<CardModel, bool>) (c => c.Type == CardType.Power)).ToList<CardModel>();
    if (list.Count == 0)
      return;
    CardModel card = player.RunState.Rng.CombatCardSelection.NextItem<CardModel>((IEnumerable<CardModel>) list);
    this.Flash();
    card.SetToFreeThisTurn();
    CardPileAddResult cardPileAddResult = await CardPileCmd.Add(card, PileType.Hand);
  }
}
