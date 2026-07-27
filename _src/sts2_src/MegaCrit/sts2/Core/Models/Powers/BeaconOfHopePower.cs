// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Powers.BeaconOfHopePower
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.ValueProps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Powers;

public sealed class BeaconOfHopePower : PowerModel
{
  private bool _hasAlreadyBeenGivenBlock;

  public override PowerType Type => PowerType.Buff;

  public override PowerStackType StackType => PowerStackType.Single;

  private bool HasAlreadyBeenGivenBlock
  {
    get => this._hasAlreadyBeenGivenBlock;
    set
    {
      this.AssertMutable();
      this._hasAlreadyBeenGivenBlock = value;
    }
  }

  public override async Task AfterBlockGained(
    Creature creature,
    Decimal amount,
    ValueProp props,
    CardModel? cardSource)
  {
    if (amount < 1M || creature != this.Owner || this.CombatState.CurrentSide != this.Owner.Side || this.HasAlreadyBeenGivenBlock)
      return;
    Decimal amountToGive = amount * 0.5M;
    if (amountToGive < 1M)
      return;
    IEnumerable<Creature> creatures = this.CombatState.GetTeammatesOf(this.Owner).Where<Creature>((Func<Creature, bool>) (c => c != null && c.IsAlive && c.IsPlayer && c.Player.Creature != this.Owner));
    this.HasAlreadyBeenGivenBlock = true;
    foreach (Creature creature1 in creatures)
    {
      Decimal num = await CreatureCmd.GainBlock(creature1, amountToGive, ValueProp.Unpowered, (CardPlay) null);
    }
    this.HasAlreadyBeenGivenBlock = false;
  }
}
