// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Modifiers.SealedDeck
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models.Relics;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Runs;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Modifiers;

public class SealedDeck : ModifierModel
{
  public override bool ClearsPlayerDeck => true;

  public override Func<Task> GenerateNeowOption(EventModel eventModel)
  {
    return (Func<Task>) (() => SealedDeck.ChooseCards(eventModel.Owner));
  }

  private static async Task ChooseCards(Player player)
  {
    // ISSUE: object of a compiler-generated type is created
    IEnumerable<CardCreationResult> list = (IEnumerable<CardCreationResult>) CardFactory.CreateForReward(player, 30, new CardCreationOptions((IEnumerable<CardPoolModel>) new \u003C\u003Ez__ReadOnlySingleElementList<CardPoolModel>(player.Character.CardPool), CardCreationSource.Other, CardRarityOddsType.RegularEncounter).WithFlags(CardCreationFlags.NoUpgradeRoll | CardCreationFlags.ForceRarityOddsChange | CardCreationFlags.IsCardReward)).ToList<CardCreationResult>();
    // ISSUE: reference to a compiler-generated field
    // ISSUE: reference to a compiler-generated field
    CardSelectorPrefs prefs = new CardSelectorPrefs(new LocString("modifiers", "SEALED_DECK.selectionPrompt"), 10)
    {
      Cancelable = false,
      RequireManualConfirmation = true,
      Comparison = SealedDeck.\u003C\u003EO.\u003C0\u003E__CompareCards ?? (SealedDeck.\u003C\u003EO.\u003C0\u003E__CompareCards = new Comparison<CardModel>(SealedDeck.CompareCards))
    };
    CardCmd.PreviewCardPileAdd(await CardPileCmd.Add((IEnumerable<CardModel>) (await CardSelectCmd.FromSimpleGridForRewards((PlayerChoiceContext) new BlockingPlayerChoiceContext(), list.ToList<CardCreationResult>(), player, prefs)).ToList<CardModel>(), PileType.Deck), style: CardPreviewStyle.GridLayout);
    foreach (Player player1 in (IEnumerable<Player>) player.RunState.Players)
      player1.RelicGrabBag.Remove<PandorasBox>();
    player.RunState.SharedRelicGrabBag.Remove<PandorasBox>();
  }

  private static int CompareCards(CardModel card1, CardModel card2)
  {
    return card1.Rarity != card2.Rarity ? card1.Rarity.CompareTo((object) card2.Rarity) : string.Compare(card1.Title, card2.Title, LocManager.Instance.CultureInfo, CompareOptions.None);
  }
}
