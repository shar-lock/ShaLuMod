// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Cards.Catastrophe
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Cards;

public sealed class Catastrophe : CardModel
{
  public Catastrophe()
    : base(2, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
  {
  }

  protected override IEnumerable<DynamicVar> CanonicalVars
  {
    get
    {
      return (IEnumerable<DynamicVar>) new \u003C\u003Ez__ReadOnlySingleElementList<DynamicVar>((DynamicVar) new CardsVar(2));
    }
  }

  protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
  {
    for (int i = 0; i < this.DynamicVars.Cards.IntValue; ++i)
    {
      CardModel card = PileType.Draw.GetPile(this.Owner).Cards.Where<CardModel>((Func<CardModel, bool>) (c => !c.Keywords.Contains(CardKeyword.Unplayable))).ToList<CardModel>().StableShuffle<CardModel>(this.Owner.RunState.Rng.Shuffle).FirstOrDefault<CardModel>() ?? PileType.Draw.GetPile(this.Owner).Cards.ToList<CardModel>().StableShuffle<CardModel>(this.Owner.RunState.Rng.Shuffle).FirstOrDefault<CardModel>();
      if (card != null)
        await CardCmd.AutoPlay(choiceContext, card, (Creature) null);
    }
  }

  protected override void OnUpgrade() => this.DynamicVars.Cards.UpgradeValueBy(1M);
}
