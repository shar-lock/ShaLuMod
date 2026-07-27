// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Cards.Enlightenment
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using System.Collections.Generic;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Cards;

public sealed class Enlightenment : CardModel
{
  public Enlightenment()
    : base(0, CardType.Skill, CardRarity.Event, TargetType.Self)
  {
  }

  public override IEnumerable<CardKeyword> CanonicalKeywords
  {
    get
    {
      return (IEnumerable<CardKeyword>) new \u003C\u003Ez__ReadOnlySingleElementList<CardKeyword>(CardKeyword.Exhaust);
    }
  }

  protected override Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
  {
    foreach (CardModel card in (IEnumerable<CardModel>) PileType.Hand.GetPile(this.Owner).Cards)
    {
      if (this.IsUpgraded)
        card.EnergyCost.SetThisCombat(1, true);
      else
        card.EnergyCost.SetThisTurnOrUntilPlayed(1, true);
    }
    return Task.CompletedTask;
  }
}
