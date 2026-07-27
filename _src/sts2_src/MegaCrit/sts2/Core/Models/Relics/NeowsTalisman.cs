// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Relics.NeowsTalisman
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Relics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Relics;

public sealed class NeowsTalisman : RelicModel
{
  public override RelicRarity Rarity => RelicRarity.Ancient;

  public override bool HasUponPickupEffect => true;

  public override Task AfterObtained()
  {
    List<CardModel> list = PileType.Deck.GetPile(this.Owner).Cards.Where<CardModel>((Func<CardModel, bool>) (c => c.Rarity == CardRarity.Basic)).ToList<CardModel>();
    // ISSUE: object of a compiler-generated type is created
    foreach (CardModel card in (IEnumerable<CardModel>) new \u003C\u003Ez__ReadOnlyArray<CardModel>(new CardModel[2]
    {
      list.LastOrDefault<CardModel>((Func<CardModel, bool>) (c => c.Tags.Contains<CardTag>(CardTag.Strike))),
      list.LastOrDefault<CardModel>((Func<CardModel, bool>) (c => c.Tags.Contains<CardTag>(CardTag.Defend)))
    }))
    {
      if (card != null)
        CardCmd.Upgrade(card);
    }
    return Task.CompletedTask;
  }
}
