// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Modifiers.AllStar
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Random;
using MegaCrit.Sts2.Core.Runs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Modifiers;

public class AllStar : ModifierModel
{
  public override Func<Task> GenerateNeowOption(EventModel eventModel)
  {
    return (Func<Task>) (() => AllStar.ObtainCards(eventModel.Owner, eventModel.Rng));
  }

  private static async Task ObtainCards(Player player, Rng rng)
  {
    List<CardPileAddResult> results = new List<CardPileAddResult>();
    for (int i = 0; i < 5; ++i)
    {
      // ISSUE: object of a compiler-generated type is created
      results.Add(await CardPileCmd.Add(CardFactory.CreateForReward(player, 1, CardCreationOptions.ForNonCombatWithUniformOdds((IEnumerable<CardPoolModel>) new \u003C\u003Ez__ReadOnlySingleElementList<CardPoolModel>((CardPoolModel) ModelDb.CardPool<ColorlessCardPool>())).WithFlags(CardCreationFlags.NoRarityModification | CardCreationFlags.NoCardPoolModifications)).First<CardCreationResult>().Card, PileType.Deck));
    }
    CardCmd.PreviewCardPileAdd((IReadOnlyList<CardPileAddResult>) results);
    await Cmd.CustomScaledWait(0.6f, 1.2f);
    results = (List<CardPileAddResult>) null;
  }
}
