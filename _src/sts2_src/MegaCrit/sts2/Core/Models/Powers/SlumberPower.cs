// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Powers.SlumberPower
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.Monsters;
using MegaCrit.Sts2.Core.ValueProps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Powers;

public sealed class SlumberPower : PowerModel
{
  public override PowerType Type => PowerType.Buff;

  public override PowerStackType StackType => PowerStackType.Counter;

  public override async Task AfterDamageReceived(
    PlayerChoiceContext choiceContext,
    Creature target,
    DamageResult result,
    ValueProp props,
    Creature? dealer,
    CardModel? cardSource)
  {
    if (target != this.Owner || result.UnblockedDamage == 0)
      return;
    await PowerCmd.Decrement((PowerModel) this);
    if (this.Amount > 0)
      return;
    await CreatureCmd.Stun(this.Owner, new Func<IReadOnlyList<Creature>, Task>(((SlumberingBeetle) this.Owner.Monster).WakeUpMove), "ROLL_OUT_MOVE");
  }

  public override Task AfterRemoved(Creature oldOwner)
  {
    SfxCmd.StopLoop("event:/sfx/enemy/enemy_attacks/slumbering_beetle/slumbering_beetle_sleep_loop");
    return Task.CompletedTask;
  }

  public override async Task AfterSideTurnEnd(
    PlayerChoiceContext choiceContext,
    CombatSide side,
    IEnumerable<Creature> participants)
  {
    if (!participants.Contains<Creature>(this.Owner))
      return;
    await PowerCmd.Decrement((PowerModel) this);
    if (this.Amount > 0)
      return;
    await ((SlumberingBeetle) this.Owner.Monster).WakeUpMove((IReadOnlyList<Creature>) Array.Empty<Creature>());
  }
}
