// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Powers.BlackHolePower
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.ValueProps;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Powers;

public sealed class BlackHolePower : PowerModel
{
  public override PowerType Type => PowerType.Buff;

  public override PowerStackType StackType => PowerStackType.Counter;

  public override async Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
  {
    if (cardPlay.Resources.StarsSpent <= 0 || cardPlay.Card.Owner != this.Owner.Player || !cardPlay.IsLastInSeries)
      return;
    await this.DealDamageToAllEnemies();
  }

  public override async Task AfterStarsGained(int amount, Player gainer)
  {
    if (amount <= 0 || gainer != this.Owner.Player)
      return;
    await this.DealDamageToAllEnemies();
  }

  private async Task DealDamageToAllEnemies()
  {
    this.Flash();
    IEnumerable<DamageResult> damageResults = await CreatureCmd.Damage((PlayerChoiceContext) new BlockingPlayerChoiceContext(), (IEnumerable<Creature>) this.CombatState.HittableEnemies, (Decimal) this.Amount, ValueProp.Unpowered, this.Owner);
  }
}
