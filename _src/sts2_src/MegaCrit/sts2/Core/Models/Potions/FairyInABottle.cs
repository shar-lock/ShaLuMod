// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Potions.FairyInABottle
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Potions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using System;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Potions;

public sealed class FairyInABottle : PotionModel
{
  public override PotionRarity Rarity => PotionRarity.Rare;

  public override PotionUsage Usage => PotionUsage.Automatic;

  public override TargetType TargetType => TargetType.Self;

  public override bool CanBeGeneratedInCombat => false;

  protected override async Task OnUse(PlayerChoiceContext choiceContext, Creature? target)
  {
    PotionModel.AssertValidForTargetedPotion(target);
    await CreatureCmd.Heal(target, Math.Max((Decimal) target.MaxHp * 0.3M, 1M));
  }

  public override bool ShouldDie(Creature creature) => creature != this.Owner.Creature;

  public override async Task AfterPreventingDeath(Creature creature)
  {
    await this.OnUseWrapper((PlayerChoiceContext) new ThrowingPlayerChoiceContext(), creature);
  }
}
