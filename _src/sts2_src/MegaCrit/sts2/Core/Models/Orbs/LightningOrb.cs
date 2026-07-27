// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Orbs.LightningOrb
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.ValueProps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Orbs;

public class LightningOrb : OrbModel
{
  protected override string PassiveSfx => "event:/sfx/characters/defect/defect_lightning_passive";

  protected override string EvokeSfx => "event:/sfx/characters/defect/defect_lightning_evoke";

  protected override string ChannelSfx => "event:/sfx/characters/defect/defect_lightning_channel";

  public override Color DarkenedColor => new Color("796606");

  public override Decimal PassiveVal => this.ModifyOrbValue(3M);

  public override Decimal EvokeVal => this.ModifyOrbValue(8M);

  public override async Task BeforeTurnEndOrbTrigger(PlayerChoiceContext choiceContext)
  {
    await this.TriggerPassive(choiceContext, (Creature) null);
  }

  public override async Task Passive(PlayerChoiceContext choiceContext, Creature? target)
  {
    this.ActivatePassive();
    IEnumerable<Creature> creatures = await this.ApplyLightningDamage(this.PassiveVal, target, choiceContext, false);
  }

  public override async Task<IEnumerable<Creature>> Evoke(PlayerChoiceContext playerChoiceContext)
  {
    return await this.ApplyLightningDamage(this.EvokeVal, (Creature) null, playerChoiceContext, true);
  }

  private async Task<IEnumerable<Creature>> ApplyLightningDamage(
    Decimal value,
    Creature? target,
    PlayerChoiceContext choiceContext,
    bool isEvoke)
  {
    List<Creature> list = this.CombatState.GetOpponentsOf(this.Owner.Creature).Where<Creature>((Func<Creature, bool>) (e => e.IsHittable)).ToList<Creature>();
    if (list.Count == 0)
      return (IEnumerable<Creature>) Array.Empty<Creature>();
    // ISSUE: object of a compiler-generated type is created
    // ISSUE: object of a compiler-generated type is created
    IReadOnlyList<Creature> targets = target == null ? (IReadOnlyList<Creature>) new \u003C\u003Ez__ReadOnlySingleElementList<Creature>(this.Owner.RunState.Rng.CombatTargets.NextItem<Creature>((IEnumerable<Creature>) list)) : (IReadOnlyList<Creature>) new \u003C\u003Ez__ReadOnlySingleElementList<Creature>(target);
    if (isEvoke)
      this.ActivateEvoke(targets.ToArray<Creature>());
    foreach (Creature target1 in (IEnumerable<Creature>) targets)
      VfxCmd.PlayOnCreature(target1, "vfx/vfx_attack_lightning");
    this.PlayEvokeSfx();
    IEnumerable<DamageResult> damageResults = await CreatureCmd.Damage(choiceContext, (IEnumerable<Creature>) targets, value, ValueProp.Unpowered, this.Owner.Creature);
    return (IEnumerable<Creature>) targets;
  }
}
