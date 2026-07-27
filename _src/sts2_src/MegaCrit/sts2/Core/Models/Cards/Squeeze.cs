// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Cards.Squeeze
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Monsters;
using MegaCrit.Sts2.Core.ValueProps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Cards;

public sealed class Squeeze : CardModel
{
  public Squeeze()
    : base(3, CardType.Attack, CardRarity.Rare, TargetType.AnyEnemy)
  {
  }

  protected override bool ShouldGlowRedInternal => this.Owner.IsOstyMissing;

  protected override HashSet<CardTag> CanonicalTags
  {
    get => new HashSet<CardTag>() { CardTag.OstyAttack };
  }

  protected override IEnumerable<DynamicVar> CanonicalVars
  {
    get
    {
      return (IEnumerable<DynamicVar>) new \u003C\u003Ez__ReadOnlyArray<DynamicVar>(new DynamicVar[3]
      {
        (DynamicVar) new CalculationBaseVar(25M),
        (DynamicVar) new ExtraDamageVar(5M).FromOsty(),
        (DynamicVar) new CalculatedDamageVar(ValueProp.Move).FromOsty().WithMultiplier((Func<CardModel, Creature, Decimal>) ((card, _) => (Decimal) card.Owner.PlayerCombatState.AllCards.Count<CardModel>((Func<CardModel, bool>) (c => c.Tags.Contains<CardTag>(CardTag.OstyAttack) && c != card))))
      });
    }
  }

  protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
  {
    ArgumentNullException.ThrowIfNull((object) cardPlay.Target, "cardPlay.Target");
    if (Osty.CheckMissingWithAnim(this.Owner))
      return;
    AttackCommand attackCommand = await DamageCmd.Attack(this.DynamicVars.CalculatedDamage).FromOsty(this.Owner.Osty, (CardModel) this, cardPlay).Targeting(cardPlay.Target).WithHitFx("vfx/vfx_attack_blunt", tmpSfx: "blunt_attack.mp3").Execute(choiceContext);
  }

  protected override void OnUpgrade()
  {
    this.DynamicVars.CalculationBase.UpgradeValueBy(5M);
    this.DynamicVars.ExtraDamage.UpgradeValueBy(1M);
  }
}
