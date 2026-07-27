// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Cards.Finisher
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Combat.History.Entries;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.ValueProps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Cards;

public sealed class Finisher : CardModel
{
  private const string _calculatedHitsKey = "CalculatedHits";

  public Finisher()
    : base(1, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy)
  {
  }

  protected override IEnumerable<DynamicVar> CanonicalVars
  {
    get
    {
      return (IEnumerable<DynamicVar>) new \u003C\u003Ez__ReadOnlyArray<DynamicVar>(new DynamicVar[4]
      {
        (DynamicVar) new DamageVar(6M, ValueProp.Move),
        (DynamicVar) new CalculationBaseVar(0M),
        (DynamicVar) new CalculationExtraVar(1M),
        (DynamicVar) new CalculatedVar("CalculatedHits").WithMultiplier((Func<CardModel, Creature, Decimal>) ((card, _) => (Decimal) CombatManager.Instance.History.CardPlaysFinished.Count<CardPlayFinishedEntry>((Func<CardPlayFinishedEntry, bool>) (e => e.HappenedThisTurn(card.CombatState) && e.CardPlay.Card.Type == CardType.Attack && e.CardPlay.Player == card.Owner))))
      });
    }
  }

  protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
  {
    ArgumentNullException.ThrowIfNull((object) cardPlay.Target, "cardPlay.Target");
    AttackCommand attackCommand = await DamageCmd.Attack(this.DynamicVars.Damage.BaseValue).WithHitCount((int) ((CalculatedVar) this.DynamicVars["CalculatedHits"]).Calculate(cardPlay.Target)).FromCard((CardModel) this, cardPlay).Targeting(cardPlay.Target).WithHitVfxNode((Func<Creature, Node2D>) (t => (Node2D) NStabVfx.Create(t, true))).WithHitFx(tmpSfx: "blunt_attack.mp3").Execute(choiceContext);
  }

  protected override void OnUpgrade() => this.DynamicVars.Damage.UpgradeValueBy(2M);
}
