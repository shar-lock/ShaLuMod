// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Cards.DecisionsDecisions
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Cards;

public sealed class DecisionsDecisions : CardModel
{
  public DecisionsDecisions()
    : base(0, CardType.Skill, CardRarity.Rare, TargetType.Self)
  {
  }

  public override int CanonicalStarCost => 6;

  protected override IEnumerable<DynamicVar> CanonicalVars
  {
    get
    {
      return (IEnumerable<DynamicVar>) new \u003C\u003Ez__ReadOnlyArray<DynamicVar>(new DynamicVar[2]
      {
        (DynamicVar) new CardsVar(3),
        (DynamicVar) new RepeatVar(3)
      });
    }
  }

  public override IEnumerable<CardKeyword> CanonicalKeywords
  {
    get
    {
      return (IEnumerable<CardKeyword>) new \u003C\u003Ez__ReadOnlySingleElementList<CardKeyword>(CardKeyword.Exhaust);
    }
  }

  protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
  {
    await CreatureCmd.TriggerAnim(this.Owner.Creature, "Cast", this.Owner.Character.CastAnimDelay);
    IEnumerable<CardModel> cardModels = await CardPileCmd.Draw(choiceContext, (Decimal) this.DynamicVars.Cards.IntValue, this.Owner);
    CardSelectorPrefs prefs = new CardSelectorPrefs(this.SelectionScreenPrompt, 1)
    {
      PretendCardsCanBePlayed = true
    };
    CardModel card = (await CardSelectCmd.FromHand(choiceContext, this.Owner, prefs, (Func<CardModel, bool>) (c => c.Type == CardType.Skill && !c.Keywords.Contains(CardKeyword.Unplayable)), (AbstractModel) this)).FirstOrDefault<CardModel>();
    if (card == null)
    {
      card = (CardModel) null;
    }
    else
    {
      for (int i = 0; i < this.DynamicVars.Repeat.IntValue; ++i)
        await CardCmd.AutoPlay(choiceContext, card, (Creature) null);
      card = (CardModel) null;
    }
  }

  protected override void OnUpgrade() => this.DynamicVars.Cards.UpgradeValueBy(2M);
}
