// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Cards.AllForOne
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

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

public sealed class AllForOne : CardModel
{
  public AllForOne()
    : base(2, CardType.Attack, CardRarity.Rare, TargetType.AnyEnemy)
  {
  }

  protected override IEnumerable<DynamicVar> CanonicalVars
  {
    get
    {
      return (IEnumerable<DynamicVar>) new \u003C\u003Ez__ReadOnlySingleElementList<DynamicVar>((DynamicVar) new DamageVar(10M, ValueProp.Move));
    }
  }

  protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
  {
    ArgumentNullException.ThrowIfNull((object) cardPlay.Target, "cardPlay.Target");
    AttackCommand attackCommand = await DamageCmd.Attack(this.DynamicVars.Damage.BaseValue).FromCard((CardModel) this, cardPlay).Targeting(cardPlay.Target).WithHitFx("vfx/vfx_heavy_blunt", tmpSfx: "blunt_attack.mp3").WithHitVfxSpawnedAtBase().Execute(choiceContext);
    foreach (CardModel card in (IEnumerable<CardModel>) PileType.Discard.GetPile(this.Owner).Cards.Where<CardModel>(new Func<CardModel, bool>(this.Filter)).ToList<CardModel>())
    {
      CardPileAddResult cardPileAddResult = await CardPileCmd.Add(card, PileType.Hand);
    }
  }

  private bool Filter(CardModel card)
  {
    bool flag1 = card.EnergyCost.GetWithModifiers(CostModifiers.All) == 0 && !card.EnergyCost.CostsX;
    if (flag1)
    {
      bool flag2;
      switch (card.Type)
      {
        case CardType.Attack:
        case CardType.Skill:
        case CardType.Power:
          flag2 = true;
          break;
        default:
          flag2 = false;
          break;
      }
      flag1 = flag2;
    }
    return flag1;
  }

  protected override void OnUpgrade() => this.DynamicVars.Damage.UpgradeValueBy(4M);
}
