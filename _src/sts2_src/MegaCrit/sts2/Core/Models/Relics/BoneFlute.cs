// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Relics.BoneFlute
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Monsters;
using MegaCrit.Sts2.Core.ValueProps;
using System.Collections.Generic;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Relics;

public sealed class BoneFlute : RelicModel
{
  public override RelicRarity Rarity => RelicRarity.Common;

  protected override IEnumerable<DynamicVar> CanonicalVars
  {
    get
    {
      return (IEnumerable<DynamicVar>) new \u003C\u003Ez__ReadOnlySingleElementList<DynamicVar>((DynamicVar) new BlockVar(2M, ValueProp.Unpowered));
    }
  }

  public override Task AfterAttack(PlayerChoiceContext choiceContext, AttackCommand command)
  {
    if (!(command.Attacker?.Monster is Osty) || command.Attacker.PetOwner?.Creature != this.Owner.Creature)
      return Task.CompletedTask;
    this.Flash();
    return (Task) CreatureCmd.GainBlock(this.Owner.Creature, this.DynamicVars.Block, (CardPlay) null);
  }
}
