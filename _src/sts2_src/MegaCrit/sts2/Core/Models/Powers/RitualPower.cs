// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Powers.RitualPower
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Powers;

public sealed class RitualPower : PowerModel
{
  private bool _wasJustAppliedByEnemy;

  public override PowerType Type => PowerType.Buff;

  public override PowerStackType StackType => PowerStackType.Counter;

  protected override IEnumerable<IHoverTip> ExtraHoverTips
  {
    get
    {
      return (IEnumerable<IHoverTip>) new IHoverTip[1]
      {
        HoverTipFactory.FromPower<StrengthPower>()
      };
    }
  }

  private bool WasJustAppliedByEnemy
  {
    get => this._wasJustAppliedByEnemy;
    set
    {
      this.AssertMutable();
      this._wasJustAppliedByEnemy = value;
    }
  }

  public override Task AfterApplied(Creature? applier, CardModel? cardSource)
  {
    if (this.Owner.IsEnemy)
      this.WasJustAppliedByEnemy = true;
    return Task.CompletedTask;
  }

  public override async Task AfterSideTurnEnd(
    PlayerChoiceContext choiceContext,
    CombatSide side,
    IEnumerable<Creature> participants)
  {
    if (!participants.Contains<Creature>(this.Owner))
      return;
    if (this.WasJustAppliedByEnemy)
    {
      this.WasJustAppliedByEnemy = false;
    }
    else
    {
      this.Flash();
      StrengthPower strengthPower = await PowerCmd.Apply<StrengthPower>(choiceContext, this.Owner, (Decimal) this.Amount, this.Owner, (CardModel) null);
    }
  }
}
