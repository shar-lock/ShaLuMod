// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Orbs.Mock.MockCombatCleanupOrb
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Orbs.Mock;

public class MockCombatCleanupOrb : OrbModel
{
  public override bool IsMock => true;

  public override Decimal PassiveVal => 0M;

  public override Decimal EvokeVal => 0M;

  public override Color DarkenedColor => new Color("000000");

  public override Task<IEnumerable<Creature>> Evoke(PlayerChoiceContext playerChoiceContext)
  {
    this.Owner.Creature.CombatState.RemoveCreature(this.Owner.Creature);
    return Task.FromResult<IEnumerable<Creature>>((IEnumerable<Creature>) Array.Empty<Creature>());
  }
}
