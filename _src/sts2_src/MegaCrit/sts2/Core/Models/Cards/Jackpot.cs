// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Cards.Jackpot
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Cards;

public sealed class Jackpot : CardModel
{
  public Jackpot()
    : base(3, CardType.Attack, CardRarity.Rare, TargetType.AnyEnemy)
  {
  }

  protected override IEnumerable<DynamicVar> CanonicalVars
  {
    get
    {
      return (IEnumerable<DynamicVar>) new \u003C\u003Ez__ReadOnlyArray<DynamicVar>(new DynamicVar[2]
      {
        (DynamicVar) new DamageVar(25M, ValueProp.Move),
        (DynamicVar) new CardsVar(3)
      });
    }
  }

  protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
  {
    ArgumentNullException.ThrowIfNull((object) cardPlay.Target, "cardPlay.Target");
    AttackCommand attackCommand = await DamageCmd.Attack(this.DynamicVars.Damage.BaseValue).FromCard((CardModel) this, cardPlay).Targeting(cardPlay.Target).WithHitFx("vfx/vfx_attack_slash").Execute(choiceContext);
    foreach (CardModel card in CardFactory.GetForCombat(this.Owner, this.Owner.Character.CardPool.GetUnlockedCards(this.Owner.UnlockState, this.Owner.RunState.CardMultiplayerConstraint).Where<CardModel>((Func<CardModel, bool>) (c =>
    {
      CardEnergyCost energyCost = c.EnergyCost;
      return energyCost != null && energyCost.Canonical == 0 && !energyCost.CostsX;
    })), this.DynamicVars.Cards.IntValue, this.Owner.RunState.Rng.CombatCardGeneration))
    {
      if (this.IsUpgraded)
        CardCmd.Upgrade(card);
      CardPileAddResult combat = await CardPileCmd.AddGeneratedCardToCombat(card, PileType.Hand, this.Owner);
    }
  }

  protected override void OnUpgrade() => this.DynamicVars.Damage.UpgradeValueBy(5M);
}
