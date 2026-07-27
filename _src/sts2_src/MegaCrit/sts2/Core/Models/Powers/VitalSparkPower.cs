// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Powers.VitalSparkPower
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Afflictions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Powers;

public sealed class VitalSparkPower : PowerModel
{
  public override PowerType Type => PowerType.Buff;

  public override PowerStackType StackType => PowerStackType.Counter;

  protected override IEnumerable<DynamicVar> CanonicalVars
  {
    get
    {
      return (IEnumerable<DynamicVar>) new \u003C\u003Ez__ReadOnlySingleElementList<DynamicVar>((DynamicVar) new StringVar("AfflictionTitle", ModelDb.Affliction<Tainted>().Title.GetFormattedText()));
    }
  }

  protected override IEnumerable<IHoverTip> ExtraHoverTips
  {
    get => HoverTipFactory.FromAffliction<Tainted>(this.Amount);
  }

  public override async Task BeforeCombatStart()
  {
    foreach (Creature creature in this.Owner.CombatState.Allies.ToList<Creature>())
    {
      if (creature.IsPlayer)
      {
        foreach (CardModel card in creature.Player.PlayerCombatState.AllCards.Where<CardModel>((Func<CardModel, bool>) (c => c.Type == CardType.Skill)))
        {
          Tainted tainted = await CardCmd.Afflict<Tainted>(card, (Decimal) this.Amount);
        }
      }
    }
  }

  public override async Task AfterCardEnteredCombat(CardModel card)
  {
    if (card.Affliction != null || card.Type != CardType.Skill)
      return;
    Tainted tainted = await CardCmd.Afflict<Tainted>(card, (Decimal) this.Amount);
  }

  public override async Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
  {
    if (!(cardPlay.Card.Affliction is Tainted))
      return;
    this.Flash();
    TaintedPower taintedPower = await PowerCmd.Apply<TaintedPower>(choiceContext, cardPlay.Card.Owner.Creature, (Decimal) this.Amount, (Creature) null, (CardModel) null);
  }

  public override Task AfterRemoved(Creature oldOwner)
  {
    if (oldOwner.CombatState == null)
      return Task.CompletedTask;
    foreach (Creature creature in oldOwner.CombatState.Allies.ToList<Creature>())
    {
      if (creature.IsPlayer)
      {
        foreach (CardModel card in creature.Player.PlayerCombatState.AllCards.Where<CardModel>((Func<CardModel, bool>) (c => c.Affliction is Tainted)))
          CardCmd.ClearAffliction(card);
      }
    }
    return Task.CompletedTask;
  }

  public override Task AfterPowerAmountChanged(
    PlayerChoiceContext choiceContext,
    PowerModel power,
    Decimal amount,
    Creature? applier,
    CardModel? cardSource)
  {
    if (power != this)
      return Task.CompletedTask;
    foreach (Creature creature in this.Owner.CombatState.Allies.ToList<Creature>())
    {
      if (creature.IsPlayer)
      {
        foreach (CardModel cardModel in creature.Player.PlayerCombatState.AllCards.Where<CardModel>((Func<CardModel, bool>) (c => c.Affliction is Tainted)))
          cardModel.Affliction.Amount = this.Amount;
      }
    }
    return Task.CompletedTask;
  }
}
