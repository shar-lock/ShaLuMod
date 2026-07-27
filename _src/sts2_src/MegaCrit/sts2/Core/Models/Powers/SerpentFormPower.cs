// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Powers.SerpentFormPower
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Random;
using MegaCrit.Sts2.Core.ValueProps;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Powers;

public sealed class SerpentFormPower : PowerModel
{
  public override PowerType Type => PowerType.Buff;

  public override PowerStackType StackType => PowerStackType.Counter;

  protected override object InitInternalData() => (object) new SerpentFormPower.Data();

  public override Task BeforeCardPlayed(CardPlay cardPlay)
  {
    if (cardPlay.Card.Owner != this.Owner.Player)
      return Task.CompletedTask;
    this.GetInternalData<SerpentFormPower.Data>().amountsForPlayedCards.Add(cardPlay.Card, this.Amount);
    return Task.CompletedTask;
  }

  public override async Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
  {
    int damage;
    if (cardPlay.Card.Owner != this.Owner.Player || !this.GetInternalData<SerpentFormPower.Data>().amountsForPlayedCards.Remove(cardPlay.Card, ref damage) || damage <= 0)
      return;
    await Cmd.CustomScaledWait(0.1f, 0.2f);
    Rng combatTargets = this.Owner.Player.RunState.Rng.CombatTargets;
    ICombatState combatState = this.Owner.CombatState;
    object items = (combatState != null ? (object) combatState.HittableEnemies : (object) null) ?? (object) Array.Empty<Creature>();
    Creature target = combatTargets.NextItem<Creature>((IEnumerable<Creature>) items);
    if (target == null)
      return;
    VfxCmd.PlayOnCreatureCenter(target, "vfx/vfx_attack_blunt");
    IEnumerable<DamageResult> damageResults = await CreatureCmd.Damage(choiceContext, target, (Decimal) damage, ValueProp.Unpowered, this.Owner);
  }

  private class Data
  {
    public readonly Dictionary<CardModel, int> amountsForPlayedCards = new Dictionary<CardModel, int>();
  }
}
