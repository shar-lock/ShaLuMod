// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Cards.Severance
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

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

public sealed class Severance : CardModel
{
  public Severance()
    : base(2, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy)
  {
  }

  protected override IEnumerable<DynamicVar> CanonicalVars
  {
    get
    {
      return (IEnumerable<DynamicVar>) new \u003C\u003Ez__ReadOnlySingleElementList<DynamicVar>((DynamicVar) new DamageVar(13M, ValueProp.Move));
    }
  }

  protected override IEnumerable<IHoverTip> ExtraHoverTips
  {
    get
    {
      return (IEnumerable<IHoverTip>) new \u003C\u003Ez__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromCard<Soul>());
    }
  }

  protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
  {
    ArgumentNullException.ThrowIfNull((object) cardPlay.Target, "cardPlay.Target");
    AttackCommand attackCommand = await DamageCmd.Attack(this.DynamicVars.Damage.BaseValue).FromCard((CardModel) this, cardPlay).Targeting(cardPlay.Target).WithHitFx("vfx/vfx_attack_slash").Execute(choiceContext);
    List<Soul> souls = Soul.Create(this.Owner, 3, this.CombatState).ToList<Soul>();
    CardPileAddResult drawResult = await CardPileCmd.AddGeneratedCardToCombat((CardModel) souls[0], PileType.Draw, this.Owner, CardPilePosition.Random);
    CardPileAddResult discardResult = await CardPileCmd.AddGeneratedCardToCombat((CardModel) souls[1], PileType.Discard, this.Owner);
    CardPileAddResult combat = await CardPileCmd.AddGeneratedCardToCombat((CardModel) souls[2], PileType.Hand, this.Owner);
    // ISSUE: object of a compiler-generated type is created
    CardCmd.PreviewCardPileAdd((IReadOnlyList<CardPileAddResult>) new \u003C\u003Ez__ReadOnlyArray<CardPileAddResult>(new CardPileAddResult[2]
    {
      drawResult,
      discardResult
    }));
    souls = (List<Soul>) null;
    drawResult = new CardPileAddResult();
    discardResult = new CardPileAddResult();
  }

  protected override void OnUpgrade() => this.DynamicVars.Damage.UpgradeValueBy(5M);
}
