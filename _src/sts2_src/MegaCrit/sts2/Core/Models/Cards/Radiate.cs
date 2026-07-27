// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Cards.Radiate
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Combat.History.Entries;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Cards;

public sealed class Radiate : CardModel
{
  private const string _calculatedHitsKey = "CalculatedHits";

  public Radiate()
    : base(0, CardType.Attack, CardRarity.Uncommon, TargetType.AllEnemies)
  {
  }

  protected override IEnumerable<DynamicVar> CanonicalVars
  {
    get
    {
      return (IEnumerable<DynamicVar>) new \u003C\u003Ez__ReadOnlyArray<DynamicVar>(new DynamicVar[5]
      {
        (DynamicVar) new DamageVar(3M, ValueProp.Move),
        (DynamicVar) new StarsVar(1),
        (DynamicVar) new CalculationBaseVar(0M),
        (DynamicVar) new CalculationExtraVar(1M),
        (DynamicVar) new CalculatedVar("CalculatedHits").WithMultiplier((Func<CardModel, Creature, Decimal>) ((card, _) => (Decimal) CombatManager.Instance.History.Entries.OfType<StarsModifiedEntry>().Where<StarsModifiedEntry>((Func<StarsModifiedEntry, bool>) (e => e.HappenedThisTurn(card.CombatState) && e.Amount > 0 && e.Actor == card.Owner.Creature)).Sum<StarsModifiedEntry>((Func<StarsModifiedEntry, int>) (e => e.Amount))))
      });
    }
  }

  protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
  {
    AttackCommand attackCommand = await DamageCmd.Attack(this.DynamicVars.Damage.BaseValue).WithHitCount((int) ((CalculatedVar) this.DynamicVars["CalculatedHits"]).Calculate(cardPlay.Target)).FromCard((CardModel) this, cardPlay).TargetingAllOpponents(this.CombatState).WithHitFx("vfx/vfx_starry_impact", tmpSfx: "slash_attack.mp3").SpawningHitVfxOnEachCreature().Execute(choiceContext);
  }

  protected override void OnUpgrade() => this.DynamicVars.Damage.UpgradeValueBy(1M);
}
