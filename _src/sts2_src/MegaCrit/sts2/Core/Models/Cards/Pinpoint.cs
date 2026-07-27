// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Cards.Pinpoint
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Combat.History.Entries;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Cards;

public sealed class Pinpoint : CardModel
{
  public Pinpoint()
    : base(3, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy)
  {
  }

  protected override IEnumerable<DynamicVar> CanonicalVars
  {
    get
    {
      return (IEnumerable<DynamicVar>) new \u003C\u003Ez__ReadOnlySingleElementList<DynamicVar>((DynamicVar) new DamageVar(15M, ValueProp.Move));
    }
  }

  protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
  {
    ArgumentNullException.ThrowIfNull((object) cardPlay.Target, "cardPlay.Target");
    AttackCommand attackCommand = await DamageCmd.Attack(this.DynamicVars.Damage.BaseValue).FromCard((CardModel) this, cardPlay).Targeting(cardPlay.Target).WithHitFx("vfx/vfx_attack_slash").Execute(choiceContext);
  }

  protected override void OnUpgrade() => this.DynamicVars.Damage.UpgradeValueBy(4M);

  public override Task AfterCardEnteredCombat(CardModel card)
  {
    if (card != this || this.IsClone)
      return Task.CompletedTask;
    this.ReduceCostBy(CombatManager.Instance.History.CardPlaysFinished.Count<CardPlayFinishedEntry>((Func<CardPlayFinishedEntry, bool>) (e => e.CardPlay.Card.Type == CardType.Skill && e.CardPlay.Player == this.Owner && e.HappenedThisTurn(this.CombatState))));
    return Task.CompletedTask;
  }

  public override Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
  {
    if (cardPlay.Card.Owner != this.Owner || cardPlay.Card.Type != CardType.Skill)
      return Task.CompletedTask;
    this.ReduceCostBy(1);
    return Task.CompletedTask;
  }

  private void ReduceCostBy(int amount) => this.EnergyCost.AddThisTurn(-amount);
}
