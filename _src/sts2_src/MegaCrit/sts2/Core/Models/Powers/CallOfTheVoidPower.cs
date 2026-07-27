// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Powers.CallOfTheVoidPower
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Random;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Powers;

public sealed class CallOfTheVoidPower : PowerModel
{
  public override PowerType Type => PowerType.Buff;

  public override PowerStackType StackType => PowerStackType.Counter;

  protected override IEnumerable<IHoverTip> ExtraHoverTips
  {
    get
    {
      return (IEnumerable<IHoverTip>) new \u003C\u003Ez__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromKeyword(CardKeyword.Ethereal));
    }
  }

  public override async Task BeforeHandDraw(
    Player player,
    PlayerChoiceContext choiceContext,
    ICombatState combatState)
  {
    if (player != this.Owner.Player)
      return;
    IReadOnlyList<CardModel> list = (IReadOnlyList<CardModel>) this.Owner.Player.Character.CardPool.GetUnlockedCards(player.UnlockState, player.RunState.CardMultiplayerConstraint).Where<CardModel>((Func<CardModel, bool>) (c =>
    {
      bool flag;
      switch (c.Rarity)
      {
        case CardRarity.Basic:
        case CardRarity.Ancient:
          flag = true;
          break;
        default:
          flag = false;
          break;
      }
      return !flag;
    })).ToList<CardModel>();
    if (list.Count <= 0)
      return;
    CardModel[] cards = new CardModel[this.Amount];
    Rng combatCardGeneration = this.Owner.Player.RunState.Rng.CombatCardGeneration;
    for (int index = 0; index < this.Amount; ++index)
    {
      CardModel card = CardFactory.GetDistinctForCombat(player, (IEnumerable<CardModel>) list, 1, combatCardGeneration).First<CardModel>();
      cards[index] = card;
      CardCmd.ApplyKeyword(card, CardKeyword.Ethereal);
    }
    this.Flash();
    IReadOnlyList<CardPileAddResult> combat = await CardPileCmd.AddGeneratedCardsToCombat((IEnumerable<CardModel>) cards, PileType.Hand, this.Owner.Player);
  }
}
