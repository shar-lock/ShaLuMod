// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Modifiers.Draft
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Models.Relics;
using MegaCrit.Sts2.Core.Rewards;
using MegaCrit.Sts2.Core.Runs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Modifiers;

public class Draft : ModifierModel
{
  public override bool ClearsPlayerDeck => true;

  public override Func<Task> GenerateNeowOption(EventModel eventModel)
  {
    return (Func<Task>) (() => Draft.OfferRewards(eventModel.Owner));
  }

  private static async Task OfferRewards(Player player)
  {
    // ISSUE: object of a compiler-generated type is created
    CardCreationOptions creationOptions = new CardCreationOptions((IEnumerable<CardPoolModel>) new \u003C\u003Ez__ReadOnlySingleElementList<CardPoolModel>(player.Character.CardPool), CardCreationSource.Other, CardRarityOddsType.RegularEncounter).WithFlags(CardCreationFlags.NoUpgradeRoll);
    for (int i = 0; i < 10; ++i)
    {
      CardReward cardReward = new CardReward(creationOptions, 3, player)
      {
        CanSkip = false
      };
      cardReward.Populate();
      int num = await cardReward.SelectUnsynchronized() ? 1 : 0;
    }
    foreach (Player player1 in (IEnumerable<Player>) player.RunState.Players)
      player1.RelicGrabBag.Remove<PandorasBox>();
    player.RunState.SharedRelicGrabBag.Remove<PandorasBox>();
    creationOptions = (CardCreationOptions) null;
  }
}
