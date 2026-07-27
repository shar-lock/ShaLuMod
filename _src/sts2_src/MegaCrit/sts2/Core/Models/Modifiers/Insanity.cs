// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Modifiers.Insanity
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.Models.Relics;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Random;
using MegaCrit.Sts2.Core.Runs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Modifiers;

public class Insanity : ModifierModel
{
  public override bool ClearsPlayerDeck => true;

  public override Func<Task> GenerateNeowOption(EventModel eventModel)
  {
    return (Func<Task>) (() => Insanity.ObtainCards(eventModel.Owner, eventModel.Rng));
  }

  private static async Task ObtainCards(Player player, Rng rng)
  {
    List<CardPileAddResult> results = new List<CardPileAddResult>();
    for (int i = 0; i < 30; ++i)
    {
      // ISSUE: object of a compiler-generated type is created
      results.Add(await CardPileCmd.Add(CardFactory.CreateForReward(player, 1, CardCreationOptions.ForNonCombatWithUniformOdds((IEnumerable<CardPoolModel>) new \u003C\u003Ez__ReadOnlySingleElementList<CardPoolModel>(player.Character.CardPool)).WithFlags(CardCreationFlags.NoRarityModification)).First<CardCreationResult>().Card, PileType.Deck));
    }
    foreach (CardPileAddResult result in results)
    {
      CardCmd.PreviewCardPileAdd(result, style: CardPreviewStyle.MessyLayout);
      await Cmd.CustomScaledWait(0.1f, 0.2f);
    }
    await Cmd.CustomScaledWait(0.6f, 1.2f);
    foreach (Player player1 in (IEnumerable<Player>) player.RunState.Players)
      player1.RelicGrabBag.Remove<PandorasBox>();
    player.RunState.SharedRelicGrabBag.Remove<PandorasBox>();
    results = (List<CardPileAddResult>) null;
  }
}
