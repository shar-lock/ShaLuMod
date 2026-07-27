// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Cards.DrainPower
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Cards;

public sealed class DrainPower : CardModel
{
  public DrainPower()
    : base(1, CardType.Attack, CardRarity.Common, TargetType.AnyEnemy)
  {
  }

  protected override IEnumerable<DynamicVar> CanonicalVars
  {
    get
    {
      return (IEnumerable<DynamicVar>) new \u003C\u003Ez__ReadOnlyArray<DynamicVar>(new DynamicVar[2]
      {
        (DynamicVar) new DamageVar(10M, ValueProp.Move),
        (DynamicVar) new CardsVar(2)
      });
    }
  }

  protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
  {
    ArgumentNullException.ThrowIfNull((object) cardPlay.Target, "cardPlay.Target");
    AttackCommand attackCommand = await DamageCmd.Attack(this.DynamicVars.Damage.BaseValue).FromCard((CardModel) this, cardPlay).Targeting(cardPlay.Target).WithHitFx("vfx/vfx_attack_slash").Execute(choiceContext);
    foreach (CardModel card in PileType.Discard.GetPile(this.Owner).Cards.Where<CardModel>((Func<CardModel, bool>) (c => c.IsUpgradable)).TakeRandom<CardModel>(this.DynamicVars.Cards.IntValue, this.Owner.RunState.Rng.CombatCardSelection))
    {
      CardCmd.Upgrade(card);
      CardCmd.Preview(card);
    }
  }

  protected override void OnUpgrade()
  {
    this.DynamicVars.Damage.UpgradeValueBy(2M);
    this.DynamicVars.Cards.UpgradeValueBy(1M);
  }
}
