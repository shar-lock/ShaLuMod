// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Cards.BansheesCry
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Combat.History.Entries;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Cards;

public sealed class BansheesCry : CardModel
{
  public BansheesCry()
    : base(9, CardType.Attack, CardRarity.Rare, TargetType.AllEnemies)
  {
  }

  protected override IEnumerable<IHoverTip> ExtraHoverTips
  {
    get
    {
      return (IEnumerable<IHoverTip>) new \u003C\u003Ez__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromKeyword(CardKeyword.Ethereal));
    }
  }

  protected override IEnumerable<DynamicVar> CanonicalVars
  {
    get
    {
      return (IEnumerable<DynamicVar>) new \u003C\u003Ez__ReadOnlyArray<DynamicVar>(new DynamicVar[2]
      {
        (DynamicVar) new DamageVar(33M, ValueProp.Move),
        (DynamicVar) new EnergyVar(2)
      });
    }
  }

  protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
  {
    AttackCommand attackCommand = await DamageCmd.Attack(this.DynamicVars.Damage.BaseValue).FromCard((CardModel) this, cardPlay).TargetingAllOpponents(this.CombatState).WithHitFx("vfx/vfx_attack_slash").Execute(choiceContext);
  }

  protected override void OnUpgrade() => this.EnergyCost.UpgradeBy(-2);

  public override Task AfterCardEnteredCombat(CardModel card)
  {
    if (card != this || this.IsClone)
      return Task.CompletedTask;
    this.EnergyCost.AddThisCombat(-CombatManager.Instance.History.CardPlaysFinished.Count<CardPlayFinishedEntry>((Func<CardPlayFinishedEntry, bool>) (e => e.WasEthereal && e.CardPlay.Player == this.Owner)) * this.DynamicVars.Energy.IntValue);
    return Task.CompletedTask;
  }

  public override Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
  {
    if (cardPlay.Card.Owner != this.Owner || !cardPlay.Card.Keywords.Contains(CardKeyword.Ethereal))
      return Task.CompletedTask;
    this.EnergyCost.AddThisCombat(-this.DynamicVars.Energy.IntValue);
    return Task.CompletedTask;
  }
}
