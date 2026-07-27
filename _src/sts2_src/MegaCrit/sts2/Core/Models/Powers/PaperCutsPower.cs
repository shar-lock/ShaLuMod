// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Powers.PaperCutsPower
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.ValueProps;
using System;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Powers;

public sealed class PaperCutsPower : PowerModel
{
  public override PowerType Type => PowerType.Buff;

  public override PowerStackType StackType => PowerStackType.Counter;

  public override async Task AfterDamageGiven(
    PlayerChoiceContext choiceContext,
    Creature? dealer,
    DamageResult result,
    ValueProp props,
    Creature target,
    CardModel? cardSource)
  {
    if (dealer != this.Owner || !target.IsPlayer || !props.IsPoweredAttack() || result.UnblockedDamage <= 0)
      return;
    await CreatureCmd.LoseMaxHp(choiceContext, target, (Decimal) this.Amount, false);
  }
}
