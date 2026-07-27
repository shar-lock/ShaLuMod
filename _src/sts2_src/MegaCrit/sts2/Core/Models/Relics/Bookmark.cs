// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Relics.Bookmark
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Relics;

public sealed class Bookmark : RelicModel
{
  public override RelicRarity Rarity => RelicRarity.Rare;

  protected override IEnumerable<IHoverTip> ExtraHoverTips
  {
    get
    {
      return (IEnumerable<IHoverTip>) new \u003C\u003Ez__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromKeyword(CardKeyword.Retain));
    }
  }

  public override Task AfterFlush(
    PlayerChoiceContext choiceContext,
    Player player,
    IReadOnlyCollection<CardModel> flushedCards,
    IReadOnlyCollection<CardModel> retainedCards)
  {
    if (player != this.Owner)
      return Task.CompletedTask;
    List<CardModel> list = retainedCards.Where<CardModel>((Func<CardModel, bool>) (c => !c.EnergyCost.CostsX && c.EnergyCost.GetWithModifiers(CostModifiers.Local) > 0)).ToList<CardModel>();
    if (list.Count == 0)
      return Task.CompletedTask;
    this.Flash();
    this.Owner.RunState.Rng.CombatCardSelection.NextItem<CardModel>((IEnumerable<CardModel>) list)?.EnergyCost.AddUntilPlayed(-1);
    return Task.CompletedTask;
  }
}
