// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Relics.FurCoat
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Map;
using MegaCrit.Sts2.Core.Random;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.Runs.History;
using MegaCrit.Sts2.Core.Saves.Runs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Relics;

public sealed class FurCoat : RelicModel
{
  private const string _combatsKey = "Combats";
  private int _furCoatActIndex = -1;

  public static LocString HistoryEntry
  {
    get
    {
      FurCoat furCoat = ModelDb.Relic<FurCoat>();
      LocString historyEntry = new LocString("relics", furCoat.Id.Entry + ".historyEntry");
      historyEntry.Add("Title", furCoat.Title);
      return historyEntry;
    }
  }

  public override RelicRarity Rarity => RelicRarity.Ancient;

  [SavedProperty]
  public int FurCoatActIndex
  {
    get => this._furCoatActIndex;
    set
    {
      this.AssertMutable();
      this._furCoatActIndex = value;
    }
  }

  [SavedProperty]
  private int[] FurCoatCoordCols { get; set; } = Array.Empty<int>();

  [SavedProperty]
  private int[] FurCoatCoordRows { get; set; } = Array.Empty<int>();

  [SavedProperty]
  private bool FurCoatCoordsSet { get; set; }

  protected override IEnumerable<DynamicVar> CanonicalVars
  {
    get
    {
      return (IEnumerable<DynamicVar>) new \u003C\u003Ez__ReadOnlySingleElementList<DynamicVar>(new DynamicVar("Combats", 7M));
    }
  }

  public override Task AfterObtained()
  {
    this.FurCoatActIndex = this.Owner.RunState.CurrentActIndex;
    this.AddMarkedRooms(this.Owner.RunState.Map);
    return Task.CompletedTask;
  }

  public override ActMap ModifyGeneratedMapLate(IRunState runState, ActMap map, int actIndex)
  {
    return this.AddMarkedRooms(map);
  }

  private ActMap AddMarkedRooms(ActMap map)
  {
    if (this.Owner.RunState.CurrentActIndex != this.FurCoatActIndex)
      return map;
    List<MapCoord> markedCoords = this.GetMarkedCoords();
    bool flag1 = markedCoords == null;
    if (!flag1)
      flag1 = !markedCoords.TrueForAll((Predicate<MapCoord>) (c =>
      {
        if (!map.HasPoint(c))
          return false;
        return map.GetPoint(c).PointType == MapPointType.Monster || map.GetPoint(c).PointType == MapPointType.Elite;
      }));
    if (flag1)
    {
      Rng rng = new Rng(this.Owner, this.Id);
      List<MapPoint> list1 = map.GetAllMapPoints().Where<MapPoint>((Func<MapPoint, bool>) (p =>
      {
        bool flag2;
        switch (p.PointType)
        {
          case MapPointType.Monster:
          case MapPointType.Elite:
            flag2 = true;
            break;
          default:
            flag2 = false;
            break;
        }
        return flag2 && !p.Quests.Any<AbstractModel>((Func<AbstractModel, bool>) (q => q is FurCoat));
      })).ToList<MapPoint>();
      list1.UnstableShuffle<MapPoint>(rng);
      int intValue = this.DynamicVars["Combats"].IntValue;
      List<MapPoint> list2 = list1.Take<MapPoint>(intValue).ToList<MapPoint>();
      this.FurCoatCoordCols = new int[list2.Count];
      this.FurCoatCoordRows = new int[list2.Count];
      for (int index = 0; index < list2.Count; ++index)
      {
        this.FurCoatCoordCols[index] = list2[index].coord.col;
        this.FurCoatCoordRows[index] = list2[index].coord.row;
      }
      this.FurCoatCoordsSet = true;
      foreach (MapPoint mapPoint in list2)
        mapPoint.AddQuest((AbstractModel) this);
    }
    else
    {
      foreach (MapCoord coord in markedCoords)
        (map.GetPoint(coord) ?? throw new InvalidOperationException($"Loaded a fur coat map with coordinate {coord}, but the generated map does not contain that coordinate!")).AddQuest((AbstractModel) this);
    }
    return map;
  }

  public override async Task BeforeCombatStart()
  {
    List<MapCoord> markedCoords = this.GetMarkedCoords();
    if (markedCoords == null || !markedCoords.Contains(this.Owner.RunState.CurrentMapPoint.coord))
      return;
    MapPointHistoryEntry pointHistoryEntry = this.Owner.RunState.CurrentMapPointHistoryEntry;
    if (pointHistoryEntry != null)
      pointHistoryEntry.GetEntry(this.Owner.NetId).IsAffectedByFurCoat = true;
    this.Flash();
    IReadOnlyList<Creature> hittableEnemies = this.Owner.Creature.CombatState.HittableEnemies;
    VfxCmd.PlayOnCreatureCenters((IEnumerable<Creature>) hittableEnemies, "vfx/vfx_bite");
    foreach (Creature creature in (IEnumerable<Creature>) hittableEnemies)
      await CreatureCmd.SetCurrentHp(creature, 1M);
  }

  public override async Task AfterCreatureAddedToCombat(Creature creature)
  {
    if (creature.Side != CombatSide.Enemy)
      return;
    List<MapCoord> markedCoords = this.GetMarkedCoords();
    if (markedCoords == null || !markedCoords.Contains(this.Owner.RunState.CurrentMapPoint.coord))
      return;
    this.Flash();
    VfxCmd.PlayOnCreatureCenter(creature, "vfx/vfx_bite");
    await CreatureCmd.SetCurrentHp(creature, 1M);
  }

  public List<MapCoord>? GetMarkedCoords()
  {
    if (!this.FurCoatCoordsSet)
      return (List<MapCoord>) null;
    List<MapCoord> markedCoords = new List<MapCoord>();
    for (int index = 0; index < this.FurCoatCoordCols.Length; ++index)
      markedCoords.Add(new MapCoord()
      {
        col = this.FurCoatCoordCols[index],
        row = this.FurCoatCoordRows[index]
      });
    return markedCoords;
  }
}
