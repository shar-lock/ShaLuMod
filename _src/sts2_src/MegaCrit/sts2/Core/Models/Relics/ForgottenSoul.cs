// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Relics.ForgottenSoul
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using System.Collections.Generic;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Relics;

public sealed class ForgottenSoul : RelicModel
{
  public override RelicRarity Rarity => RelicRarity.Event;

  protected override IEnumerable<DynamicVar> CanonicalVars
  {
    get
    {
      return (IEnumerable<DynamicVar>) new \u003C\u003Ez__ReadOnlySingleElementList<DynamicVar>((DynamicVar) new DamageVar(1M, ValueProp.Unpowered));
    }
  }

  public override async Task AfterCardExhausted(
    PlayerChoiceContext choiceContext,
    CardModel card,
    bool _)
  {
    if (card.Owner != this.Owner)
      return;
    this.Flash();
    Creature target = this.Owner.RunState.Rng.CombatTargets.NextItem<Creature>((IEnumerable<Creature>) this.Owner.Creature.CombatState.HittableEnemies);
    if (target == null)
      return;
    VfxCmd.PlayOnCreatureCenter(target, "vfx/vfx_attack_blunt");
    IEnumerable<DamageResult> damageResults = await CreatureCmd.Damage(choiceContext, target, this.DynamicVars.Damage, this.Owner.Creature);
  }
}
