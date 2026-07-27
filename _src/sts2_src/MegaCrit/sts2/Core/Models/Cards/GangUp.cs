// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Cards.GangUp
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

public sealed class GangUp : CardModel
{
  public GangUp()
    : base(1, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy)
  {
  }

  public override CardMultiplayerConstraint MultiplayerConstraint
  {
    get => CardMultiplayerConstraint.MultiplayerOnly;
  }

  protected override IEnumerable<DynamicVar> CanonicalVars
  {
    get
    {
      return (IEnumerable<DynamicVar>) new \u003C\u003Ez__ReadOnlyArray<DynamicVar>(new DynamicVar[3]
      {
        (DynamicVar) new CalculationBaseVar(5M),
        (DynamicVar) new ExtraDamageVar(5M),
        (DynamicVar) new CalculatedDamageVar(ValueProp.Move).WithMultiplier((Func<CardModel, Creature, Decimal>) ((card, target) => (Decimal) CombatManager.Instance.History.Entries.OfType<DamageReceivedEntry>().Count<DamageReceivedEntry>((Func<DamageReceivedEntry, bool>) (e => e.Receiver == target && e.Result.Props.IsPoweredAttack() && e.HappenedThisTurn(card.CombatState) && e.Dealer != null && e.Dealer != card.Owner.Creature && e.Dealer.Side == card.Owner.Creature.Side))))
      });
    }
  }

  protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
  {
    ArgumentNullException.ThrowIfNull((object) cardPlay.Target, "cardPlay.Target");
    AttackCommand attackCommand = await DamageCmd.Attack(this.DynamicVars.CalculatedDamage).FromCard((CardModel) this, cardPlay).Targeting(cardPlay.Target).WithHitFx("vfx/vfx_attack_blunt", tmpSfx: "blunt_attack.mp3").Execute(choiceContext);
  }

  protected override void OnUpgrade() => this.DynamicVars.ExtraDamage.UpgradeValueBy(2M);
}
