// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Powers.StampedePower
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Powers;

public sealed class StampedePower : PowerModel
{
  public override PowerType Type => PowerType.Buff;

  public override PowerStackType StackType => PowerStackType.Counter;

  public override async Task AfterAutoPostPlayPhaseEntered(
    PlayerChoiceContext choiceContext,
    Player player)
  {
    CardPile hand;
    if (player != this.Owner.Player)
    {
      hand = (CardPile) null;
    }
    else
    {
      hand = PileType.Hand.GetPile(this.Owner.Player);
      for (int i = 0; i < this.Amount; ++i)
      {
        CardModel card = this.Owner.Player.RunState.Rng.Shuffle.NextItem<CardModel>((IEnumerable<CardModel>) hand.Cards.Where<CardModel>((Func<CardModel, bool>) (c => c.Type == CardType.Attack && !c.Keywords.Contains(CardKeyword.Unplayable))).ToList<CardModel>());
        if (card != null)
          await CardCmd.AutoPlay(choiceContext, card, (Creature) null);
      }
      hand = (CardPile) null;
    }
  }
}
