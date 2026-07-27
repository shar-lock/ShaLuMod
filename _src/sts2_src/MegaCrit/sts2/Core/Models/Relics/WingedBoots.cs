// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Relics.WingedBoots
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Map;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.Saves.Runs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Relics;

public sealed class WingedBoots : RelicModel
{
  private const string _roomsKey = "Rooms";
  private const int _roomCount = 3;
  private int _timesUsed;

  public override RelicRarity Rarity => RelicRarity.Ancient;

  public override bool IsUsedUp => this.TimesUsed >= 3;

  public override bool ShowCounter => !this.IsUsedUp;

  public override int DisplayAmount => 3 - this.TimesUsed;

  public override bool IsAllowed(IRunState runState) => runState.Players.Count == 1;

  protected override IEnumerable<DynamicVar> CanonicalVars
  {
    get
    {
      return (IEnumerable<DynamicVar>) new \u003C\u003Ez__ReadOnlySingleElementList<DynamicVar>(new DynamicVar("Rooms", 3M));
    }
  }

  [SavedProperty]
  public int TimesUsed
  {
    get => this._timesUsed;
    set
    {
      this.AssertMutable();
      this._timesUsed = value;
      this.DynamicVars["Rooms"].BaseValue = (Decimal) (3 - this._timesUsed);
      this.InvokeDisplayAmountChanged();
      this.CheckIfUsedUp();
    }
  }

  public override bool ShouldAllowFreeTravel() => !this.IsUsedUp;

  public override Task AfterRoomEntered(AbstractRoom room)
  {
    if (this.IsUsedUp || this.Owner.RunState.CurrentRoomCount > 1 || !(this.Owner.RunState is RunState runState) || runState.VisitedMapCoords.Count <= 1)
      return Task.CompletedTask;
    IReadOnlyList<MapCoord> visitedMapCoords = runState.VisitedMapCoords;
    MapCoord coord = visitedMapCoords[visitedMapCoords.Count - 2];
    MapPoint point = runState.Map.GetPoint(coord);
    if (point == null)
      return Task.CompletedTask;
    MapPoint currentMapPoint = this.Owner.RunState.CurrentMapPoint;
    if (currentMapPoint == null || point.Children.Contains(currentMapPoint))
      return Task.CompletedTask;
    ++this.TimesUsed;
    return Task.CompletedTask;
  }

  private void CheckIfUsedUp()
  {
    if (!this.IsUsedUp)
      return;
    this.Status = RelicStatus.Disabled;
  }
}
