// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Relics.ChoicesParadox
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Debug;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Exceptions;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Relics;

public sealed class ChoicesParadox : RelicModel
{
  public override RelicRarity Rarity => RelicRarity.Ancient;

  protected override IEnumerable<DynamicVar> CanonicalVars
  {
    get
    {
      return (IEnumerable<DynamicVar>) new \u003C\u003Ez__ReadOnlySingleElementList<DynamicVar>((DynamicVar) new CardsVar(5));
    }
  }

  protected override IEnumerable<IHoverTip> ExtraHoverTips
  {
    get
    {
      return (IEnumerable<IHoverTip>) new \u003C\u003Ez__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromKeyword(CardKeyword.Retain));
    }
  }

  public override async Task AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player)
  {
    if (player != this.Owner || this.Owner.PlayerCombatState.TurnNumber != 1)
      return;
    this.Flash();
    List<CardModel> list = CardFactory.GetDistinctForCombat(this.Owner, this.Owner.Character.CardPool.GetUnlockedCards(this.Owner.UnlockState, this.Owner.RunState.CardMultiplayerConstraint), this.DynamicVars.Cards.IntValue, this.Owner.RunState.Rng.CombatCardGeneration).ToList<CardModel>();
    if (list.Count == 0)
    {
      string str = "ChoicesParadox generated no cards for selection. Returning early to prevent softlock.";
      Log.Error(str);
      SentryService.CaptureException((Exception) new SoftlockException(str));
    }
    else
    {
      foreach (CardModel card in list)
      {
        CardKeyword[] cardKeywordArray = new CardKeyword[1]
        {
          CardKeyword.Retain
        };
        CardCmd.ApplyKeyword(card, cardKeywordArray);
      }
      foreach (CardModel card in await CardSelectCmd.FromSimpleGrid(choiceContext, (IReadOnlyList<CardModel>) list, this.Owner, new CardSelectorPrefs(RelicModel.L10NLookup("CHOICES_PARADOX.selectionScreenPrompt"), 1)))
      {
        CardPileAddResult combat = await CardPileCmd.AddGeneratedCardToCombat(card, PileType.Hand, this.Owner);
      }
    }
  }
}
