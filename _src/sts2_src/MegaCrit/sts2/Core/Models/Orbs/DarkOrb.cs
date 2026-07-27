// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Orbs.DarkOrb
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.ValueProps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Orbs;

public class DarkOrb : OrbModel
{
  private Decimal _evokeVal = 6M;

  protected override string ChannelSfx => "event:/sfx/characters/defect/defect_dark_channel";

  public override Color DarkenedColor => new Color("9001d3");

  public override Decimal PassiveVal => this.ModifyOrbValue(6M);

  public override Decimal EvokeVal => this._evokeVal;

  public override async Task BeforeTurnEndOrbTrigger(PlayerChoiceContext choiceContext)
  {
    await this.TriggerPassive(choiceContext, (Creature) null);
  }

  public override Task Passive(PlayerChoiceContext choiceContext, Creature? target)
  {
    if (target != null)
      throw new InvalidOperationException("Dark orbs cannot target creatures.");
    this.ActivatePassive();
    this._evokeVal += this.PassiveVal;
    NCombatRoom.Instance?.GetCreatureNode(this.Owner.Creature)?.OrbManager?.UpdateVisuals(OrbEvokeType.None);
    return Task.CompletedTask;
  }

  public override async Task<IEnumerable<Creature>> Evoke(PlayerChoiceContext playerChoiceContext)
  {
    IReadOnlyList<Creature> hittableEnemies = this.CombatState.HittableEnemies;
    if (hittableEnemies.Count == 0)
      return (IEnumerable<Creature>) Array.Empty<Creature>();
    this.PlayEvokeSfx();
    Creature weakestEnemy = Enumerable.MinBy<Creature, int>((IEnumerable<Creature>) hittableEnemies, (Func<Creature, int>) (c => c.CurrentHp));
    this.ActivateEvoke(new Creature[1]{ weakestEnemy });
    IEnumerable<DamageResult> damageResults = await CreatureCmd.Damage(playerChoiceContext, weakestEnemy, this.EvokeVal, ValueProp.Unpowered, this.Owner.Creature);
    // ISSUE: object of a compiler-generated type is created
    return (IEnumerable<Creature>) new \u003C\u003Ez__ReadOnlySingleElementList<Creature>(weakestEnemy);
  }
}
