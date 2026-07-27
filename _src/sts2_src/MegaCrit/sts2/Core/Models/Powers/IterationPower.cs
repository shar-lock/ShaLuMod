// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Powers.IterationPower
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Combat.History.Entries;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Powers;

public sealed class IterationPower : PowerModel
{
  public override PowerType Type => PowerType.Buff;

  public override PowerStackType StackType => PowerStackType.Counter;

  public override async Task AfterCardDrawn(
    PlayerChoiceContext choiceContext,
    CardModel card,
    bool fromHandDraw)
  {
    if (card.Owner.Creature != this.Owner || card.Type != CardType.Status || CombatManager.Instance.History.Entries.OfType<CardDrawnEntry>().Count<CardDrawnEntry>((Func<CardDrawnEntry, bool>) (e => e.HappenedThisTurn(this.CombatState) && e.Actor == this.Owner && e.Card.Type == CardType.Status)) > 1)
      return;
    this.Flash();
    IEnumerable<CardModel> cardModels = await CardPileCmd.Draw(choiceContext, (Decimal) this.Amount, this.Owner.Player);
  }
}
