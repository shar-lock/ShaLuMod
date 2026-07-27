// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Powers.SummonNextTurnPower
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Powers;

public sealed class SummonNextTurnPower : PowerModel
{
  public override PowerType Type => PowerType.Buff;

  public override PowerStackType StackType => PowerStackType.Counter;

  protected override IEnumerable<IHoverTip> ExtraHoverTips
  {
    get
    {
      return (IEnumerable<IHoverTip>) new \u003C\u003Ez__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.Static(StaticHoverTip.SummonStatic));
    }
  }

  public override async Task AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player)
  {
    if (player != this.Owner.Player || this.AmountOnTurnStart == 0)
      return;
    SummonResult summonResult = await OstyCmd.Summon(choiceContext, this.Owner.Player, (Decimal) this.Amount, (AbstractModel) this);
    await PowerCmd.Remove((PowerModel) this);
  }
}
