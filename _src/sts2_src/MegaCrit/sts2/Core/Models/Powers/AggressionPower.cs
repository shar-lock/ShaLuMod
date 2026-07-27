// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Powers.AggressionPower
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Powers;

public sealed class AggressionPower : PowerModel
{
  public override PowerType Type => PowerType.Buff;

  public override PowerStackType StackType => PowerStackType.Counter;

  public override async Task BeforeSideTurnStart(
    PlayerChoiceContext choiceContext,
    CombatSide side,
    IReadOnlyList<Creature> participants,
    ICombatState combatState)
  {
    if (!participants.Contains<Creature>(this.Owner))
      return;
    foreach (CardModel card in PileType.Discard.GetPile(this.Owner.Player).Cards.Where<CardModel>((Func<CardModel, bool>) (c => c.Type == CardType.Attack)).ToList<CardModel>().UnstableShuffle<CardModel>(this.Owner.Player.RunState.Rng.CombatCardSelection).Take<CardModel>(this.Amount))
    {
      CardPileAddResult cardPileAddResult = await CardPileCmd.Add(card, PileType.Hand);
      if (card.IsUpgradable)
        CardCmd.Upgrade(card);
    }
  }
}
