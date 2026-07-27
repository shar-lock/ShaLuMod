// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Orbs.GlassOrb
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

public class GlassOrb : OrbModel
{
  private Decimal _passiveVal = 4M;

  protected override string ChannelSfx => "event:/sfx/characters/defect/defect_glass_channel";

  public override Color DarkenedColor => new Color("008585");

  public override Decimal PassiveVal => this.ModifyOrbValue(this._passiveVal);

  public override Decimal EvokeVal => this.PassiveVal * 2M;

  public override async Task BeforeTurnEndOrbTrigger(PlayerChoiceContext choiceContext)
  {
    await this.TriggerPassive(choiceContext, (Creature) null);
  }

  public override async Task Passive(PlayerChoiceContext choiceContext, Creature? target)
  {
    List<Creature> list = this.CombatState.HittableEnemies.Where<Creature>((Func<Creature, bool>) (e => e.IsHittable)).ToList<Creature>();
    Decimal passiveVal = this.PassiveVal;
    if (passiveVal <= 0M)
      return;
    this.ActivatePassive();
    this.PlayPassiveSfx();
    this._passiveVal = Math.Max(0M, this._passiveVal - 1M);
    IEnumerable<DamageResult> damageResults = await CreatureCmd.Damage(choiceContext, (IEnumerable<Creature>) list, passiveVal, ValueProp.Unpowered, this.Owner.Creature);
  }

  public override async Task<IEnumerable<Creature>> Evoke(PlayerChoiceContext playerChoiceContext)
  {
    List<Creature> enemies = this.CombatState.HittableEnemies.Where<Creature>((Func<Creature, bool>) (e => e.IsHittable)).ToList<Creature>();
    if (this.EvokeVal <= 0M)
      return (IEnumerable<Creature>) Array.Empty<Creature>();
    this.ActivateEvoke(enemies.ToArray());
    IEnumerable<DamageResult> damageResults = await CreatureCmd.Damage(playerChoiceContext, (IEnumerable<Creature>) enemies, this.EvokeVal, ValueProp.Unpowered, this.Owner.Creature);
    return (IEnumerable<Creature>) enemies;
  }
}
