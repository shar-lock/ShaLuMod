// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Orbs.PlasmaOrb
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Orbs;

public class PlasmaOrb : OrbModel
{
  protected override string ChannelSfx => "event:/sfx/characters/defect/defect_plasma_channel";

  public override Color DarkenedColor => new Color("008585");

  public override Decimal PassiveVal => 1M;

  public override Decimal EvokeVal => 2M;

  public override async Task AfterTurnStartOrbTrigger(PlayerChoiceContext choiceContext)
  {
    await this.TriggerPassive(choiceContext, (Creature) null);
  }

  public override async Task Passive(PlayerChoiceContext choiceContext, Creature? target)
  {
    if (target != null)
      throw new InvalidOperationException("Plasma orbs cannot target creatures.");
    this.ActivatePassive();
    await PlayerCmd.GainEnergy(this.PassiveVal, this.Owner);
  }

  public override async Task<IEnumerable<Creature>> Evoke(PlayerChoiceContext playerChoiceContext)
  {
    this.PlayEvokeSfx();
    this.ActivateEvoke(new Creature[1]
    {
      this.Owner.Creature
    });
    await PlayerCmd.GainEnergy(this.EvokeVal, this.Owner);
    // ISSUE: object of a compiler-generated type is created
    return (IEnumerable<Creature>) new \u003C\u003Ez__ReadOnlySingleElementList<Creature>(this.Owner.Creature);
  }
}
