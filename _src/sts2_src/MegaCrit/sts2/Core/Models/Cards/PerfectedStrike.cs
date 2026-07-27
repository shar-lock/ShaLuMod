// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Cards.PerfectedStrike
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Characters;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.ValueProps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Cards;

public sealed class PerfectedStrike : CardModel
{
  public PerfectedStrike()
    : base(2, CardType.Attack, CardRarity.Common, TargetType.AnyEnemy)
  {
  }

  protected override HashSet<CardTag> CanonicalTags
  {
    get => new HashSet<CardTag>() { CardTag.Strike };
  }

  protected override IEnumerable<DynamicVar> CanonicalVars
  {
    get
    {
      return (IEnumerable<DynamicVar>) new \u003C\u003Ez__ReadOnlyArray<DynamicVar>(new DynamicVar[3]
      {
        (DynamicVar) new CalculationBaseVar(6M),
        (DynamicVar) new ExtraDamageVar(2M),
        (DynamicVar) new CalculatedDamageVar(ValueProp.Move).WithMultiplier((Func<CardModel, Creature, Decimal>) ((card, _) => (Decimal) card.Owner.PlayerCombatState.AllCards.Count<CardModel>((Func<CardModel, bool>) (c => c.Tags.Contains<CardTag>(CardTag.Strike)))))
      });
    }
  }

  protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
  {
    ArgumentNullException.ThrowIfNull((object) cardPlay.Target, "cardPlay.Target");
    AttackCommand attackCommand1 = DamageCmd.Attack(this.DynamicVars.CalculatedDamage).FromCard((CardModel) this, cardPlay).Targeting(cardPlay.Target).WithHitFx(tmpSfx: "heavy_attack.mp3").WithHitVfxNode((Func<Creature, Node2D>) (t => (Node2D) NBigSlashVfx.Create(t))).WithHitVfxNode((Func<Creature, Node2D>) (t => (Node2D) NBigSlashImpactVfx.Create(t)));
    if (this.DynamicVars.CalculatedDamage.Calculate(cardPlay.Target) > 12M)
      attackCommand1.WithAttackerAnim(Ironclad.GetHeavyAnimIfApplicable(this.Owner.Character), Ironclad.GetHeavyAttackDelayIfApplicable(this.Owner.Character));
    AttackCommand attackCommand2 = await attackCommand1.Execute(choiceContext);
  }

  protected override void OnUpgrade() => this.DynamicVars.ExtraDamage.UpgradeValueBy(1M);
}
