// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Cards.BeatDown
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Combat;
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

public sealed class BeatDown : CardModel
{
  public BeatDown()
    : base(3, CardType.Skill, CardRarity.Rare, TargetType.RandomEnemy)
  {
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
    foreach (CardModel card in PileType.Discard.GetPile(this.Owner).Cards.Where<CardModel>((Func<CardModel, bool>) (c => c.Type == CardType.Attack && !c.Keywords.Contains(CardKeyword.Unplayable))).ToList<CardModel>().StableShuffle<CardModel>(this.Owner.RunState.Rng.Shuffle).Take<CardModel>(this.DynamicVars.Cards.IntValue))
    {
      if (!CombatManager.Instance.IsOverOrEnding)
      {
        if (card.TargetType == TargetType.AnyEnemy)
        {
          Creature target = this.Owner.RunState.Rng.CombatTargets.NextItem<Creature>((IEnumerable<Creature>) this.CombatState.HittableEnemies);
          await CardCmd.AutoPlay(choiceContext, card, target);
        }
        else
          await CardCmd.AutoPlay(choiceContext, card, (Creature) null);
      }
      else
        break;
    }
  }

  protected override void OnUpgrade() => this.DynamicVars.Cards.UpgradeValueBy(1M);
}
