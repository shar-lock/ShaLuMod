// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Modifiers.CursedRun
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Models.CardPools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Modifiers;

public class CursedRun : ModifierModel
{
  public override async Task AfterActEntered()
  {
    foreach (Player player in (IEnumerable<Player>) this.RunState.Players)
    {
      CardPileAddResult result = await CardPileCmd.Add(player.RunState.CreateCard(this.RunState.Rng.Niche.NextItem<CardModel>(ModelDb.CardPool<CurseCardPool>().GetUnlockedCards(player.UnlockState, player.RunState.CardMultiplayerConstraint).Where<CardModel>((Func<CardModel, bool>) (c => c.CanBeGeneratedByModifiers))), player), PileType.Deck);
      if (LocalContext.IsMe(player))
        CardCmd.PreviewCardPileAdd(result);
    }
  }
}
