// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Cards.SpoilsMap
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Map;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.Saves.Runs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Cards;

public sealed class SpoilsMap : CardModel
{
  private int _spoilsActIndex = -1;

  public SpoilsMap()
    : base(-1, CardType.Quest, CardRarity.Quest, TargetType.Self)
  {
  }

  public override int MaxUpgradeLevel => 0;

  protected override IEnumerable<DynamicVar> CanonicalVars
  {
    get
    {
      return (IEnumerable<DynamicVar>) new \u003C\u003Ez__ReadOnlySingleElementList<DynamicVar>((DynamicVar) new GoldVar(600));
    }
  }

  public override IEnumerable<CardKeyword> CanonicalKeywords
  {
    get
    {
      return (IEnumerable<CardKeyword>) new \u003C\u003Ez__ReadOnlySingleElementList<CardKeyword>(CardKeyword.Unplayable);
    }
  }

  [SavedProperty]
  public int SpoilsActIndex
  {
    get => this._spoilsActIndex;
    set
    {
      this.AssertMutable();
      this._spoilsActIndex = value;
    }
  }

  public MapCoord? SpoilsCoord { get; private set; }

  public override void AfterCreated() => this.SpoilsActIndex = 1;

  public override ActMap ModifyGeneratedMap(IRunState runState, ActMap map, int actIndex)
  {
    if (actIndex != this.SpoilsActIndex)
      return map;
    CardPile pile = this.Pile;
    return (pile != null ? (pile.Type != PileType.Deck ? 1 : 0) : 1) != 0 ? map : (ActMap) new SpoilsActMap(runState);
  }

  public override ActMap ModifyGeneratedMapLate(IRunState runState, ActMap map, int actIndex)
  {
    if (actIndex != this.SpoilsActIndex)
      return map;
    CardPile pile = this.Pile;
    if ((pile != null ? (pile.Type != PileType.Deck ? 1 : 0) : 1) != 0)
      return map;
    MapPoint mapPoint = map.GetAllMapPoints().FirstOrDefault<MapPoint>((Func<MapPoint, bool>) (p => p.PointType == MapPointType.Treasure));
    if (mapPoint != null)
      this.SpoilsCoord = new MapCoord?(mapPoint.coord);
    return map;
  }

  public override Task AfterMapGenerated(ActMap map, int actIndex)
  {
    CardPile pile = this.Pile;
    if ((pile != null ? (pile.Type != PileType.Deck ? 1 : 0) : 1) != 0 || actIndex != this.SpoilsActIndex || !this.SpoilsCoord.HasValue)
      return Task.CompletedTask;
    ActMap actMap1 = map;
    MapCoord? spoilsCoord = this.SpoilsCoord;
    MapCoord coord1 = spoilsCoord.Value;
    if (actMap1.HasPoint(coord1))
    {
      ActMap actMap2 = map;
      spoilsCoord = this.SpoilsCoord;
      MapCoord coord2 = spoilsCoord.Value;
      actMap2.GetPoint(coord2)?.AddQuest((AbstractModel) this);
    }
    return Task.CompletedTask;
  }

  public override Task BeforeCardRemoved(CardModel card)
  {
    if (card != this || this.SpoilsActIndex != this.Owner.RunState.CurrentActIndex || !this.SpoilsCoord.HasValue)
      return Task.CompletedTask;
    this.Owner.RunState.Map.GetPoint(this.SpoilsCoord.Value)?.RemoveQuest((AbstractModel) this);
    return Task.CompletedTask;
  }

  public async Task<int> OnQuestComplete()
  {
    await PlayerCmd.GainGold(this.DynamicVars.Gold.BaseValue, this.Owner);
    PlayerCmd.CompleteQuest((CardModel) this);
    await CardPileCmd.RemoveFromDeck((CardModel) this);
    return this.DynamicVars.Gold.IntValue;
  }
}
