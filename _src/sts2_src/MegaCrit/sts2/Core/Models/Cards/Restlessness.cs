// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Cards.Restlessness
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Cards;

public sealed class Restlessness : CardModel
{
  public Restlessness()
    : base(0, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
  {
  }

  protected override IEnumerable<DynamicVar> CanonicalVars
  {
    get
    {
      return (IEnumerable<DynamicVar>) new \u003C\u003Ez__ReadOnlyArray<DynamicVar>(new DynamicVar[2]
      {
        (DynamicVar) new CardsVar(2),
        (DynamicVar) new EnergyVar(2)
      });
    }
  }

  protected override IEnumerable<IHoverTip> ExtraHoverTips
  {
    get
    {
      return (IEnumerable<IHoverTip>) new \u003C\u003Ez__ReadOnlySingleElementList<IHoverTip>(this.EnergyHoverTip);
    }
  }

  public override IEnumerable<CardKeyword> CanonicalKeywords
  {
    get
    {
      return (IEnumerable<CardKeyword>) new \u003C\u003Ez__ReadOnlySingleElementList<CardKeyword>(CardKeyword.Retain);
    }
  }

  protected override bool ShouldGlowGoldInternal => this.IsOnlyCardInHand;

  protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
  {
    if (!this.IsOnlyCardInHand)
      return;
    for (int i = 0; i < this.DynamicVars.Cards.IntValue; ++i)
    {
      CardModel cardModel = await CardPileCmd.Draw(choiceContext, this.Owner);
    }
    await PlayerCmd.GainEnergy((Decimal) this.DynamicVars.Energy.IntValue, this.Owner);
  }

  protected override void OnUpgrade()
  {
    this.DynamicVars.Cards.UpgradeValueBy(1M);
    this.DynamicVars.Energy.UpgradeValueBy(1M);
  }

  private bool IsOnlyCardInHand
  {
    get
    {
      return !PileType.Hand.GetPile(this.Owner).Cards.Except<CardModel>((IEnumerable<CardModel>) new \u003C\u003Ez__ReadOnlySingleElementList<CardModel>((CardModel) this)).Any<CardModel>();
    }
  }
}
