// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Powers.TenderPower
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Powers;

public sealed class TenderPower : PowerModel
{
  private int _cardsPlayedThisTurn;

  public override PowerType Type => PowerType.Debuff;

  public override PowerStackType StackType => PowerStackType.Counter;

  public override int DisplayAmount => this.CardsPlayedThisTurn;

  protected override IEnumerable<IHoverTip> ExtraHoverTips
  {
    get
    {
      return (IEnumerable<IHoverTip>) new \u003C\u003Ez__ReadOnlyArray<IHoverTip>(new IHoverTip[2]
      {
        HoverTipFactory.FromPower<StrengthPower>(),
        HoverTipFactory.FromPower<DexterityPower>()
      });
    }
  }

  private int CardsPlayedThisTurn
  {
    get => this._cardsPlayedThisTurn;
    set
    {
      this.AssertMutable();
      this._cardsPlayedThisTurn = value;
      this.InvokeDisplayAmountChanged();
    }
  }

  public override async Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
  {
    if (cardPlay.Card.Owner != this.Owner.Player)
      return;
    this.CardsPlayedThisTurn++;
    this.Flash();
    StrengthPower strengthPower = await PowerCmd.Apply<StrengthPower>(choiceContext, this.Owner, -1M, this.Applier, (CardModel) null, true);
    DexterityPower dexterityPower = await PowerCmd.Apply<DexterityPower>(choiceContext, this.Owner, -1M, this.Applier, (CardModel) null, true);
  }

  public override async Task AfterSideTurnEnd(
    PlayerChoiceContext choiceContext,
    CombatSide side,
    IEnumerable<Creature> participants)
  {
    if (!participants.Contains<Creature>(this.Owner))
      return;
    StrengthPower strengthPower = await PowerCmd.Apply<StrengthPower>(choiceContext, this.Owner, (Decimal) this.CardsPlayedThisTurn, this.Applier, (CardModel) null, true);
    DexterityPower dexterityPower = await PowerCmd.Apply<DexterityPower>(choiceContext, this.Owner, (Decimal) this.CardsPlayedThisTurn, this.Applier, (CardModel) null, true);
    this.CardsPlayedThisTurn = 0;
  }
}
