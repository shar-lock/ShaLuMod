// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Orbs.FrostOrb
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Orbs;

public class FrostOrb : OrbModel
{
  protected override string ChannelSfx => "event:/sfx/characters/defect/defect_frost_channel";

  public override Color DarkenedColor => new Color("7860a7");

  public override Decimal PassiveVal => this.ModifyOrbValue(2M);

  public override Decimal EvokeVal => this.ModifyOrbValue(5M);

  public override async Task BeforeTurnEndOrbTrigger(PlayerChoiceContext choiceContext)
  {
    await this.TriggerPassive(choiceContext, (Creature) null);
  }

  public override async Task Passive(PlayerChoiceContext choiceContext, Creature? target)
  {
    if (target != null)
      throw new InvalidOperationException("Frost orbs cannot target creatures.");
    this.ActivatePassive();
    this.PlayPassiveSfx();
    Decimal num1 = await CreatureCmd.GainBlock(this.Owner.Creature, this.PassiveVal, ValueProp.Unpowered, (CardPlay) null);
    if (!this.Owner.Creature.HasPower<HibernatePower>())
      return;
    foreach (Player player in (IEnumerable<Player>) this.CombatState.Players)
    {
      if (player != this.Owner)
      {
        Decimal num2 = await CreatureCmd.GainBlock(player.Creature, this.PassiveVal, ValueProp.Unpowered, (CardPlay) null);
      }
    }
  }

  public override async Task<IEnumerable<Creature>> Evoke(PlayerChoiceContext playerChoiceContext)
  {
    this.PlayEvokeSfx();
    this.ActivateEvoke(new Creature[1]
    {
      this.Owner.Creature
    });
    Decimal num1 = await CreatureCmd.GainBlock(this.Owner.Creature, this.EvokeVal, ValueProp.Unpowered, (CardPlay) null);
    if (!this.Owner.Creature.HasPower<HibernatePower>())
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerable<Creature>) new \u003C\u003Ez__ReadOnlySingleElementList<Creature>(this.Owner.Creature);
    }
    foreach (Player player in (IEnumerable<Player>) this.CombatState.Players)
    {
      if (player != this.Owner)
      {
        Decimal num2 = await CreatureCmd.GainBlock(player.Creature, this.EvokeVal, ValueProp.Unpowered, (CardPlay) null);
      }
    }
    return this.CombatState.Players.Select<Player, Creature>((Func<Player, Creature>) (p => p.Creature));
  }
}
