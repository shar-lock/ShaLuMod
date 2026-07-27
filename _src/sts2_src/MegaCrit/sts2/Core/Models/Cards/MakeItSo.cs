// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Cards.MakeItSo
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

public sealed class MakeItSo : CardModel
{
  public MakeItSo()
    : base(0, CardType.Attack, CardRarity.Rare, TargetType.AnyEnemy)
  {
  }

  protected override IEnumerable<DynamicVar> CanonicalVars
  {
    get
    {
      return (IEnumerable<DynamicVar>) new \u003C\u003Ez__ReadOnlyArray<DynamicVar>(new DynamicVar[2]
      {
        (DynamicVar) new DamageVar(6M, ValueProp.Move),
        (DynamicVar) new CardsVar(3)
      });
    }
  }

  protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
  {
    ArgumentNullException.ThrowIfNull((object) cardPlay.Target, "cardPlay.Target");
    AttackCommand attackCommand = await DamageCmd.Attack(this.DynamicVars.Damage.BaseValue).FromCard((CardModel) this, cardPlay).Targeting(cardPlay.Target).WithHitFx("vfx/vfx_starry_impact").Execute(choiceContext);
  }

  public override async Task AfterCardPlayedLate(
    PlayerChoiceContext choiceContext,
    CardPlay cardPlay)
  {
    if (cardPlay.Card.Owner != this.Owner || cardPlay.Card.Type != CardType.Skill || this.Pile.Type == PileType.Hand || CombatManager.Instance.History.CardPlaysFinished.Count<CardPlayFinishedEntry>((Func<CardPlayFinishedEntry, bool>) (e => e.HappenedThisTurn(this.CombatState) && e.CardPlay.Card.Type == CardType.Skill && e.CardPlay.Player == this.Owner)) % this.DynamicVars.Cards.IntValue != 0)
      return;
    CardPileAddResult cardPileAddResult = await CardPileCmd.Add((CardModel) this, PileType.Hand);
  }

  protected override void OnUpgrade() => this.DynamicVars.Damage.UpgradeValueBy(3M);
}
