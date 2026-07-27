// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Powers.PanachePower
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
using MegaCrit.Sts2.Core.ValueProps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Powers;

public sealed class PanachePower : PowerModel
{
  private const int _baseCardsLeft = 5;
  private const string _cardsLeftKey = "CardsLeft";

  public override PowerType Type => PowerType.Buff;

  public override PowerStackType StackType => PowerStackType.Counter;

  public override int DisplayAmount => this.DynamicVars["CardsLeft"].IntValue;

  public override PowerInstanceType InstanceType => PowerInstanceType.Instanced;

  protected override IEnumerable<DynamicVar> CanonicalVars
  {
    get
    {
      return (IEnumerable<DynamicVar>) new \u003C\u003Ez__ReadOnlySingleElementList<DynamicVar>(new DynamicVar("CardsLeft", 5M));
    }
  }

  protected override object InitInternalData() => (object) new PanachePower.Data();

  public override async Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
  {
    PanachePower.Data data;
    if (cardPlay.Card.Owner != this.Owner.Player)
    {
      data = (PanachePower.Data) null;
    }
    else
    {
      data = this.GetInternalData<PanachePower.Data>();
      if (data.alreadyApplied)
      {
        --this.DynamicVars["CardsLeft"].BaseValue;
        this.InvokeDisplayAmountChanged();
        if (this.DynamicVars["CardsLeft"].IntValue <= 0)
        {
          await Cmd.Wait(0.5f);
          IEnumerable<DamageResult> damageResults = await CreatureCmd.Damage(choiceContext, (IEnumerable<Creature>) this.CombatState.HittableEnemies, (Decimal) this.Amount, ValueProp.Unpowered, this.Owner);
          this.DynamicVars["CardsLeft"].BaseValue = 5M;
          this.InvokeDisplayAmountChanged();
        }
      }
      data.alreadyApplied = true;
      data = (PanachePower.Data) null;
    }
  }

  public override Task AfterSideTurnEnd(
    PlayerChoiceContext choiceContext,
    CombatSide side,
    IEnumerable<Creature> participants)
  {
    if (!participants.Contains<Creature>(this.Owner))
      return Task.CompletedTask;
    this.DynamicVars["CardsLeft"].BaseValue = 5M;
    this.InvokeDisplayAmountChanged();
    return Task.CompletedTask;
  }

  private class Data
  {
    public bool alreadyApplied;
  }
}
