// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Cards.Apotheosis
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using System.Collections.Generic;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Cards;

public sealed class Apotheosis : CardModel
{
  public Apotheosis()
    : base(2, CardType.Skill, CardRarity.Ancient, TargetType.Self)
  {
  }

  public override IEnumerable<CardKeyword> CanonicalKeywords
  {
    get
    {
      return (IEnumerable<CardKeyword>) new \u003C\u003Ez__ReadOnlyArray<CardKeyword>(new CardKeyword[2]
      {
        CardKeyword.Exhaust,
        CardKeyword.Innate
      });
    }
  }

  protected override Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
  {
    foreach (CardModel allCard in this.Owner.PlayerCombatState.AllCards)
    {
      if (allCard != this && allCard.IsUpgradable)
        CardCmd.Upgrade(allCard);
    }
    return Task.CompletedTask;
  }

  protected override void OnUpgrade() => this.EnergyCost.UpgradeBy(-1);
}
