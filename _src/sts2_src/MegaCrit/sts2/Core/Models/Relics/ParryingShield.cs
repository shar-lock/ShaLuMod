// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Relics.ParryingShield
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Relics;

public sealed class ParryingShield : RelicModel
{
  public override RelicRarity Rarity => RelicRarity.Uncommon;

  protected override IEnumerable<DynamicVar> CanonicalVars
  {
    get
    {
      return (IEnumerable<DynamicVar>) new \u003C\u003Ez__ReadOnlyArray<DynamicVar>(new DynamicVar[2]
      {
        (DynamicVar) new BlockVar(10M, ValueProp.Unpowered),
        (DynamicVar) new DamageVar(6M, ValueProp.Unpowered)
      });
    }
  }

  public override async Task AfterSideTurnEnd(
    PlayerChoiceContext choiceContext,
    CombatSide side,
    IEnumerable<Creature> participants)
  {
    if (!participants.Contains<Creature>(this.Owner.Creature) || (Decimal) this.Owner.Creature.Block < this.DynamicVars.Block.BaseValue)
      return;
    Creature target = this.Owner.RunState.Rng.CombatTargets.NextItem<Creature>((IEnumerable<Creature>) this.Owner.Creature.CombatState.HittableEnemies);
    if (target == null)
      return;
    VfxCmd.PlayOnCreatureCenter(target, "vfx/vfx_attack_blunt");
    IEnumerable<DamageResult> damageResults = await CreatureCmd.Damage(choiceContext, target, this.DynamicVars.Damage, this.Owner.Creature);
  }
}
