// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Cards.Dowsing
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Map;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Saves.Runs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Cards;

public sealed class Dowsing : CardModel
{
  public const int maxRooms = 5;
  private const string _roomsKey = "Rooms";
  private int _roomsEntered;

  public Dowsing()
    : base(-1, CardType.Quest, CardRarity.Quest, TargetType.None)
  {
  }

  public override int MaxUpgradeLevel => 0;

  protected override IEnumerable<DynamicVar> CanonicalVars
  {
    get
    {
      return (IEnumerable<DynamicVar>) new \u003C\u003Ez__ReadOnlySingleElementList<DynamicVar>(new DynamicVar("Rooms", 5M));
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
  public int RoomsEntered
  {
    get => this._roomsEntered;
    set
    {
      this.AssertMutable();
      this._roomsEntered = value;
      this.DynamicVars["Rooms"].BaseValue = (Decimal) (5 - this.RoomsEntered);
    }
  }

  public override async Task AfterRoomEntered(AbstractRoom room)
  {
    CardPile pile = this.Pile;
    if ((pile != null ? (pile.Type != PileType.Deck ? 1 : 0) : 1) != 0 || this.Owner.RunState.CurrentRoomCount > 1)
      return;
    MapPoint currentMapPoint = this.Owner.RunState.CurrentMapPoint;
    if ((currentMapPoint != null ? (currentMapPoint.PointType != MapPointType.Unknown ? 1 : 0) : 1) != 0)
      return;
    this.RoomsEntered++;
    if (this.RoomsEntered < 5)
      return;
    PlayerCmd.CompleteQuest((CardModel) this);
    CardPileAddResult? nullable = await CardCmd.TransformTo<Abundance>((CardModel) this);
  }
}
