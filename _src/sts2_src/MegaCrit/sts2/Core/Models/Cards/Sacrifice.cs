// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Cards.Sacrifice
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Monsters;
using MegaCrit.Sts2.Core.ValueProps;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Cards;

public sealed class Sacrifice : CardModel
{
  public Sacrifice()
    : base(1, CardType.Skill, CardRarity.Rare, TargetType.Self)
  {
  }

  protected override bool ShouldGlowRedInternal => this.Owner.IsOstyMissing;

  public override IEnumerable<CardKeyword> CanonicalKeywords
  {
    get
    {
      return (IEnumerable<CardKeyword>) new \u003C\u003Ez__ReadOnlySingleElementList<CardKeyword>(CardKeyword.Retain);
    }
  }

  public override bool GainsBlock => true;

  protected override IEnumerable<DynamicVar> CanonicalVars
  {
    get
    {
      return (IEnumerable<DynamicVar>) new \u003C\u003Ez__ReadOnlyArray<DynamicVar>(new DynamicVar[3]
      {
        (DynamicVar) new CalculationBaseVar(0M),
        (DynamicVar) new CalculationExtraVar(1M),
        (DynamicVar) new CalculatedBlockVar(ValueProp.Move).WithMultiplier((Func<CardModel, Creature, Decimal>) ((card, _) => (Decimal) (card.Owner.IsOstyAlive ? card.Owner.Osty.MaxHp * 2 : 0)))
      });
    }
  }

  protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
  {
    if (Osty.CheckMissingWithAnim(this.Owner))
      return;
    Decimal blockGain = this.DynamicVars.CalculatedBlock.Calculate(cardPlay.Target);
    await CreatureCmd.TriggerAnim(this.Owner.Creature, "Cast", this.Owner.Character.CastAnimDelay);
    await CreatureCmd.Kill(this.Owner.Osty);
    Decimal num = await CreatureCmd.GainBlock(this.Owner.Creature, blockGain, this.DynamicVars.CalculatedBlock.Props, cardPlay);
  }

  protected override void OnUpgrade() => this.EnergyCost.UpgradeBy(-1);
}
