// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Cards.Impatience
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Cards;

public sealed class Impatience : CardModel
{
  public Impatience()
    : base(0, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
  {
  }

  protected override bool ShouldGlowGoldInternal
  {
    get
    {
      return PileType.Hand.GetPile(this.Owner).Cards.All<CardModel>((Func<CardModel, bool>) (c => c.Type != CardType.Attack));
    }
  }

  protected override IEnumerable<DynamicVar> CanonicalVars
  {
    get
    {
      return (IEnumerable<DynamicVar>) new \u003C\u003Ez__ReadOnlySingleElementList<DynamicVar>((DynamicVar) new CardsVar(2));
    }
  }

  protected override void OnUpgrade() => this.DynamicVars.Cards.UpgradeValueBy(1M);

  protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
  {
    if (PileType.Hand.GetPile(this.Owner).Cards.Any<CardModel>((Func<CardModel, bool>) (c => c.Type == CardType.Attack)))
      return;
    IEnumerable<CardModel> cardModels = await CardPileCmd.Draw(choiceContext, this.DynamicVars.Cards.BaseValue, this.Owner);
  }
}
