// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Cards.FlakCannon
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
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

public sealed class FlakCannon : CardModel
{
  private const string _calculatedHitsKey = "CalculatedHits";

  public FlakCannon()
    : base(2, CardType.Attack, CardRarity.Rare, TargetType.RandomEnemy)
  {
  }

  protected override IEnumerable<DynamicVar> CanonicalVars
  {
    get
    {
      return (IEnumerable<DynamicVar>) new \u003C\u003Ez__ReadOnlyArray<DynamicVar>(new DynamicVar[4]
      {
        (DynamicVar) new DamageVar(8M, ValueProp.Move),
        (DynamicVar) new CalculationBaseVar(0M),
        (DynamicVar) new CalculationExtraVar(1M),
        (DynamicVar) new CalculatedVar("CalculatedHits").WithMultiplier((Func<CardModel, Creature, Decimal>) ((card, _) => (Decimal) FlakCannon.GetStatuses(card.Owner).Count<CardModel>()))
      });
    }
  }

  protected override IEnumerable<IHoverTip> ExtraHoverTips
  {
    get
    {
      return (IEnumerable<IHoverTip>) new \u003C\u003Ez__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromKeyword(CardKeyword.Exhaust));
    }
  }

  protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
  {
    List<CardModel> list = FlakCannon.GetStatuses(this.Owner).ToList<CardModel>();
    int statusCount = (int) ((CalculatedVar) this.DynamicVars["CalculatedHits"]).Calculate(cardPlay.Target);
    foreach (CardModel card in list)
      await CardCmd.Exhaust(choiceContext, card);
    AttackCommand attackCommand = await DamageCmd.Attack(this.DynamicVars.Damage.BaseValue).WithHitCount(statusCount).FromCard((CardModel) this, cardPlay).TargetingRandomOpponents(this.CombatState).WithHitFx("vfx/vfx_attack_blunt", tmpSfx: "blunt_attack.mp3").Execute(choiceContext);
  }

  private static IEnumerable<CardModel> GetStatuses(Player owner)
  {
    return owner.PlayerCombatState.AllCards.Where<CardModel>((Func<CardModel, bool>) (c => c.Type == CardType.Status && c.Pile.Type != PileType.Exhaust));
  }

  protected override void OnUpgrade() => this.DynamicVars.Damage.UpgradeValueBy(3M);
}
