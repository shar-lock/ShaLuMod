// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Powers.BurrowedPower
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.Monsters;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Powers;

public sealed class BurrowedPower : PowerModel
{
  public override PowerType Type => PowerType.Buff;

  public override PowerStackType StackType => PowerStackType.Single;

  public override bool ShouldClearBlock(Creature creature) => this.Owner != creature;

  public override async Task AfterBlockBroken(
    PlayerChoiceContext choiceContext,
    Creature target,
    Creature? breaker)
  {
    if (target != this.Owner)
      tunneler = (Tunneler) null;
    else if (!(target.Monster is Tunneler tunneler))
    {
      tunneler = (Tunneler) null;
    }
    else
    {
      await tunneler.GetStunned();
      await CreatureCmd.Stun(this.Owner, new Func<IReadOnlyList<Creature>, Task>(tunneler.StillDizzyMove), "BITE_MOVE");
      await PowerCmd.Remove<BurrowedPower>(this.Owner);
      tunneler = (Tunneler) null;
    }
  }

  public override async Task AfterRemoved(Creature oldOwner)
  {
    await CreatureCmd.LoseBlock((PlayerChoiceContext) new BlockingPlayerChoiceContext(), oldOwner, 999999999M, (Creature) null);
  }
}
