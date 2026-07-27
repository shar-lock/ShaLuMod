// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Cards.Guards
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Cards;

public sealed class Guards : CardModel
{
  public Guards()
    : base(2, CardType.Skill, CardRarity.Rare, TargetType.Self)
  {
  }

  public override IEnumerable<CardKeyword> CanonicalKeywords
  {
    get
    {
      return (IEnumerable<CardKeyword>) new \u003C\u003Ez__ReadOnlySingleElementList<CardKeyword>(CardKeyword.Exhaust);
    }
  }

  protected override IEnumerable<IHoverTip> ExtraHoverTips
  {
    get
    {
      return (IEnumerable<IHoverTip>) new \u003C\u003Ez__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromCard<MinionSacrifice>(this.IsUpgraded));
    }
  }

  protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
  {
    await CreatureCmd.TriggerAnim(this.Owner.Creature, "Cast", this.Owner.Character.CastAnimDelay);
    CardSelectorPrefs prefs = new CardSelectorPrefs(this.SelectionScreenPrompt, 0, 999999999);
    foreach (CardModel original in (await CardSelectCmd.FromHand(choiceContext, this.Owner, prefs, (Func<CardModel, bool>) null, (AbstractModel) this)).ToList<CardModel>())
    {
      CardModel card = (CardModel) this.CombatState.CreateCard<MinionSacrifice>(this.Owner);
      if (this.IsUpgraded)
        CardCmd.Upgrade(card);
      CardPileAddResult? nullable = await CardCmd.Transform(original, card);
    }
  }
}
