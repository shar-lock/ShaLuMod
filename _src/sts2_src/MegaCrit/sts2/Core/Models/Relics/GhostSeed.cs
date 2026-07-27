// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Relics.GhostSeed
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Rooms;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Relics;

public sealed class GhostSeed : RelicModel
{
  public override RelicRarity Rarity => RelicRarity.Shop;

  protected override IEnumerable<IHoverTip> ExtraHoverTips
  {
    get
    {
      return (IEnumerable<IHoverTip>) new \u003C\u003Ez__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromKeyword(CardKeyword.Ethereal));
    }
  }

  public override Task AfterCardEnteredCombat(CardModel card)
  {
    if (!GhostSeed.CanAffect(card) || card.Owner != this.Owner)
      return Task.CompletedTask;
    CardCmd.ApplyKeyword(card, CardKeyword.Ethereal);
    return Task.CompletedTask;
  }

  public override Task AfterRoomEntered(AbstractRoom room)
  {
    if (!(room is CombatRoom))
      return Task.CompletedTask;
    foreach (CardModel allCard in this.Owner.PlayerCombatState.AllCards)
    {
      if (GhostSeed.CanAffect(allCard))
        CardCmd.ApplyKeyword(allCard, CardKeyword.Ethereal);
    }
    return Task.CompletedTask;
  }

  private static bool CanAffect(CardModel card)
  {
    return card.Rarity == CardRarity.Basic && (card.Tags.Contains<CardTag>(CardTag.Strike) || card.Tags.Contains<CardTag>(CardTag.Defend)) && !card.GetKeywordsWithSources(KeywordSources.Local).Contains(CardKeyword.Ethereal);
  }
}
