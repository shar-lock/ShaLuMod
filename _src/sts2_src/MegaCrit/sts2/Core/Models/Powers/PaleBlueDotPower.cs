// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Powers.PaleBlueDotPower
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Combat.History.Entries;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Powers;

public sealed class PaleBlueDotPower : PowerModel
{
  public const string cardPlayThresholdKey = "CardPlay";
  public const int cardPlayThresholdValue = 5;

  public override int DisplayAmount => Math.Max(0, 5 - this.AttacksPlayedThisTurn);

  public override PowerType Type => PowerType.Buff;

  public override PowerStackType StackType => PowerStackType.Counter;

  protected override IEnumerable<DynamicVar> CanonicalVars
  {
    get
    {
      return (IEnumerable<DynamicVar>) new \u003C\u003Ez__ReadOnlySingleElementList<DynamicVar>(new DynamicVar("CardPlay", 5M));
    }
  }

  protected override object InitInternalData() => (object) new PaleBlueDotPower.Data();

  public override async Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
  {
    if (cardPlay.Card.Owner != this.Owner.Player)
      return;
    PaleBlueDotPower.Data internalData = this.GetInternalData<PaleBlueDotPower.Data>();
    if (internalData.alreadyActivatedThisTurn)
      return;
    this.InvokeDisplayAmountChanged();
    if (this.AttacksPlayedThisTurn < 5)
      return;
    internalData.alreadyActivatedThisTurn = true;
    await Cmd.Wait(0.5f);
    DrawCardsNextTurnPower cardsNextTurnPower = await PowerCmd.Apply<DrawCardsNextTurnPower>(choiceContext, this.Owner, (Decimal) this.Amount, this.Owner, (CardModel) null);
  }

  public override Task AfterSideTurnEnd(
    PlayerChoiceContext choiceContext,
    CombatSide side,
    IEnumerable<Creature> participants)
  {
    if (!participants.Contains<Creature>(this.Owner))
      return Task.CompletedTask;
    this.GetInternalData<PaleBlueDotPower.Data>().alreadyActivatedThisTurn = false;
    this.InvokeDisplayAmountChanged();
    return Task.CompletedTask;
  }

  private int AttacksPlayedThisTurn
  {
    get
    {
      return CombatManager.Instance.History.CardPlaysFinished.Count<CardPlayFinishedEntry>((Func<CardPlayFinishedEntry, bool>) (c => c.HappenedThisTurn(this.Owner.CombatState) && c.CardPlay.Player == this.Owner.Player));
    }
  }

  private class Data
  {
    public bool alreadyActivatedThisTurn;
  }
}
