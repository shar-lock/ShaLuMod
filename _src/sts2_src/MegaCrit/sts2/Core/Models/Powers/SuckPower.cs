// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Powers.SuckPower
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.ValueProps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Powers;

public sealed class SuckPower : PowerModel
{
  public override PowerType Type => PowerType.Buff;

  public override PowerStackType StackType => PowerStackType.Counter;

  protected override IEnumerable<IHoverTip> ExtraHoverTips
  {
    get
    {
      return (IEnumerable<IHoverTip>) new \u003C\u003Ez__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromPower<StrengthPower>());
    }
  }

  public override async Task AfterAttack(PlayerChoiceContext choiceContext, AttackCommand command)
  {
    if (command.Attacker != this.Owner || command.TargetSide == this.Owner.Side || !command.DamageProps.IsPoweredAttack())
      return;
    List<List<DamageResult>> list = command.Results.ToList<List<DamageResult>>();
    int num = 0;
    foreach (List<DamageResult> source in list)
    {
      foreach (DamageResult damageResult in source.Where<DamageResult>((Func<DamageResult, bool>) (r => r.Receiver.IsPet)).ToList<DamageResult>())
      {
        DamageResult petHit = damageResult;
        source.RemoveAll((Predicate<DamageResult>) (r => r.Receiver == petHit.Receiver.PetOwner?.Creature));
      }
      if (source.Any<DamageResult>((Func<DamageResult, bool>) (r => r.UnblockedDamage > 0)))
        ++num;
    }
    if (num <= 0)
      return;
    this.Flash();
    StrengthPower strengthPower = await PowerCmd.Apply<StrengthPower>(choiceContext, this.Owner, (Decimal) (this.Amount * num), this.Owner, (CardModel) null);
  }
}
