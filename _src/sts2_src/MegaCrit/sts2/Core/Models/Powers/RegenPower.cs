// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Powers.RegenPower
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Powers;

public sealed class RegenPower : PowerModel
{
  public override PowerType Type => PowerType.Buff;

  public override PowerStackType StackType => PowerStackType.Counter;

  public override bool ShouldScaleInMultiplayer => true;

  public override async Task BeforeSideTurnEndEarly(
    PlayerChoiceContext choiceContext,
    CombatSide side,
    IEnumerable<Creature> participants)
  {
    if (!participants.Contains<Creature>(this.Owner) || this.Owner.IsDead)
      return;
    this.Flash();
    await CreatureCmd.Heal(this.Owner, (Decimal) this.Amount);
    await PowerCmd.Decrement((PowerModel) this);
  }
}
