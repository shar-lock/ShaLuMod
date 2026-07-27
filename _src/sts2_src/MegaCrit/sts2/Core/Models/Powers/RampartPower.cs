// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Powers.RampartPower
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Models.Monsters;
using MegaCrit.Sts2.Core.ValueProps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Powers;

public sealed class RampartPower : PowerModel
{
  public override PowerType Type => PowerType.Buff;

  public override PowerStackType StackType => PowerStackType.Counter;

  public override bool ShouldScaleInMultiplayer => true;

  public override async Task AfterSideTurnStart(
    CombatSide side,
    IReadOnlyList<Creature> participants,
    ICombatState combatState)
  {
    if (side != CombatSide.Player || CombatManager.Instance.PlayersTakingExtraTurn.Count > 0)
      return;
    foreach (Creature creature in this.CombatState.Enemies.Where<Creature>((Func<Creature, bool>) (c => c.Monster is TurretOperator)))
    {
      Decimal num = await CreatureCmd.GainBlock(creature, (Decimal) this.Amount, ValueProp.Unpowered, (CardPlay) null);
    }
  }
}
