// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Powers.AsleepPower
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

public sealed class AsleepPower : PowerModel
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
    LagavulinMatriarch monster;
    if (target != this.Owner)
      monster = (LagavulinMatriarch) null;
    else if (result.UnblockedDamage == 0)
    {
      monster = (LagavulinMatriarch) null;
    }
    else
    {
      if (this.Owner.HasPower<PlatingPower>())
        await PowerCmd.Remove((PowerModel) this.Owner.GetPower<PlatingPower>());
      monster = (LagavulinMatriarch) this.Owner.Monster;
      SfxCmd.Play("event:/sfx/enemy/enemy_attacks/lagavulin_matriarch/lagavulin_matriarch_awaken");
      await CreatureCmd.TriggerAnim(this.Owner, "Wake", 0.6f);
      monster.IsAwake = true;
      await CreatureCmd.Stun(this.Owner, new Func<IReadOnlyList<Creature>, Task>(monster.WakeUpMove), "SLASH_MOVE");
      await PowerCmd.Remove((PowerModel) this);
      monster = (LagavulinMatriarch) null;
    }
  }

  public override async Task BeforeSideTurnEndVeryEarly(
    PlayerChoiceContext choiceContext,
    CombatSide side,
    IEnumerable<Creature> participants)
  {
    if (!participants.Contains<Creature>(this.Owner) || this.Amount > 1 || !this.Owner.HasPower<PlatingPower>())
      return;
    await PowerCmd.Remove((PowerModel) this.Owner.GetPower<PlatingPower>());
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
    await ((LagavulinMatriarch) this.Owner.Monster).WakeUpMove((IReadOnlyList<Creature>) Array.Empty<Creature>());
  }
}
