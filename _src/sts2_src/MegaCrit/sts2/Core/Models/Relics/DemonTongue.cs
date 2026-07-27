// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Relics.DemonTongue
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.ValueProps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Relics;

public sealed class DemonTongue : RelicModel
{
  private bool _triggeredThisTurn;

  public override RelicRarity Rarity => RelicRarity.Rare;

  public override async Task AfterDamageReceived(
    PlayerChoiceContext choiceContext,
    Creature target,
    DamageResult result,
    ValueProp props,
    Creature? dealer,
    CardModel? cardSource)
  {
    if (this.Owner.Creature.CombatState == null || this.Owner.Creature.CombatState.CurrentSide != this.Owner.Creature.Side || target != this.Owner.Creature || result.UnblockedDamage <= 0 || this._triggeredThisTurn)
      return;
    this._triggeredThisTurn = true;
    this.Flash();
    await CreatureCmd.Heal(this.Owner.Creature, (Decimal) result.UnblockedDamage);
  }

  public override Task BeforeSideTurnStart(
    PlayerChoiceContext choiceContext,
    CombatSide side,
    IReadOnlyList<Creature> participants,
    ICombatState combatState)
  {
    if (!participants.Contains<Creature>(this.Owner.Creature))
      return Task.CompletedTask;
    this._triggeredThisTurn = false;
    return Task.CompletedTask;
  }
}
