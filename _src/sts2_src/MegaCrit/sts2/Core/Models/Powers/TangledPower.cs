// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Powers.TangledPower
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Afflictions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Powers;

public sealed class TangledPower : PowerModel
{
  public override PowerType Type => PowerType.Debuff;

  public override PowerStackType StackType => PowerStackType.Counter;

  protected override IEnumerable<DynamicVar> CanonicalVars
  {
    get
    {
      return (IEnumerable<DynamicVar>) new \u003C\u003Ez__ReadOnlySingleElementList<DynamicVar>((DynamicVar) new EnergyVar(1));
    }
  }

  public override async Task AfterApplied(Creature? applier, CardModel? cardSource)
  {
    foreach (CardModel card in this.Owner.Player.PlayerCombatState.AllCards.Where<CardModel>((Func<CardModel, bool>) (c => c.Type == CardType.Attack)))
    {
      Entangled entangled = await CardCmd.Afflict<Entangled>(card, 1M);
    }
  }

  public override async Task AfterCardEnteredCombat(CardModel card)
  {
    if (card.Owner != this.Owner.Player || card.Affliction != null || card.Type != CardType.Attack)
      return;
    Entangled entangled = await CardCmd.Afflict<Entangled>(card, 1M);
  }

  public override async Task AfterSideTurnEnd(
    PlayerChoiceContext choiceContext,
    CombatSide side,
    IEnumerable<Creature> participants)
  {
    if (!participants.Contains<Creature>(this.Owner))
      return;
    this.Flash();
    await PowerCmd.Remove((PowerModel) this);
  }

  public override Task AfterRemoved(Creature oldOwner)
  {
    foreach (CardModel card in oldOwner.Player.PlayerCombatState.AllCards.Where<CardModel>((Func<CardModel, bool>) (c => c.Affliction is Entangled)))
      CardCmd.ClearAffliction(card);
    return Task.CompletedTask;
  }

  public override bool TryModifyEnergyCostInCombat(
    CardModel card,
    Decimal originalCost,
    out Decimal modifiedCost)
  {
    if (!(card.Affliction is Entangled) || card.Owner != this.Owner.Player)
    {
      modifiedCost = originalCost;
      return false;
    }
    modifiedCost = originalCost + (Decimal) this.Amount;
    return true;
  }
}
