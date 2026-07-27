// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Cards.Purity
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Cards;

public sealed class Purity : CardModel
{
  public Purity()
    : base(0, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
  {
  }

  public override IEnumerable<CardKeyword> CanonicalKeywords
  {
    get
    {
      return (IEnumerable<CardKeyword>) new \u003C\u003Ez__ReadOnlyArray<CardKeyword>(new CardKeyword[2]
      {
        CardKeyword.Retain,
        CardKeyword.Exhaust
      });
    }
  }

  protected override IEnumerable<DynamicVar> CanonicalVars
  {
    get
    {
      return (IEnumerable<DynamicVar>) new \u003C\u003Ez__ReadOnlySingleElementList<DynamicVar>((DynamicVar) new CardsVar(3));
    }
  }

  protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
  {
    CardSelectorPrefs prefs = new CardSelectorPrefs(this.SelectionScreenPrompt, 0, this.DynamicVars.Cards.IntValue);
    foreach (CardModel card in await CardSelectCmd.FromHand(choiceContext, this.Owner, prefs, (Func<CardModel, bool>) null, (AbstractModel) this))
      await CardCmd.Exhaust(choiceContext, card);
  }

  protected override void OnUpgrade() => this.DynamicVars.Cards.UpgradeValueBy(2M);
}
