// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Odds.UnknownMapPointOdds
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Hooks;
using MegaCrit.Sts2.Core.Map;
using MegaCrit.Sts2.Core.Random;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.Runs.History;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable enable
namespace MegaCrit.Sts2.Core.Odds;

public class UnknownMapPointOdds(Rng rng) : AbstractOdds(0.0f, rng)
{
  public const float baseMonsterOdds = 0.1f;
  public const float baseEliteOdds = -1f;
  public const float baseTreasureOdds = 0.02f;
  public const float baseShopOdds = 0.03f;
  private readonly Dictionary<RoomType, float> _baseOdds = new Dictionary<RoomType, float>()
  {
    [RoomType.Monster] = 0.1f,
    [RoomType.Elite] = -1f,
    [RoomType.Treasure] = 0.02f,
    [RoomType.Shop] = 0.03f
  };
  private readonly Dictionary<RoomType, float> _nonEventOdds = new Dictionary<RoomType, float>()
  {
    [RoomType.Monster] = 0.1f,
    [RoomType.Elite] = -1f,
    [RoomType.Treasure] = 0.02f,
    [RoomType.Shop] = 0.03f
  };

  public float MonsterOdds
  {
    get => this._nonEventOdds[RoomType.Monster];
    set => this._nonEventOdds[RoomType.Monster] = value;
  }

  public float EliteOdds
  {
    get => this._nonEventOdds[RoomType.Elite];
    set => this._nonEventOdds[RoomType.Elite] = value;
  }

  public float TreasureOdds
  {
    get => this._nonEventOdds[RoomType.Treasure];
    set => this._nonEventOdds[RoomType.Treasure] = value;
  }

  public float ShopOdds
  {
    get => this._nonEventOdds[RoomType.Shop];
    set => this._nonEventOdds[RoomType.Shop] = value;
  }

  public float EventOdds
  {
    get
    {
      return Math.Max(0.0f, 1f - this._nonEventOdds.Values.Where<float>((Func<float, bool>) (v => (double) v > 0.0)).Sum());
    }
  }

  public void SetBaseOdds(RoomType roomType, float baseOdds) => this._baseOdds[roomType] = baseOdds;

  public RoomType Roll(IEnumerable<RoomType> blacklist, IRunState runState)
  {
    if (runState.UnlockState.NumberOfRuns == 0)
    {
      int num = runState.MapPointHistory.SelectMany<IReadOnlyList<MapPointHistoryEntry>, MapPointHistoryEntry>((Func<IReadOnlyList<MapPointHistoryEntry>, IEnumerable<MapPointHistoryEntry>>) (l => (IEnumerable<MapPointHistoryEntry>) l)).Count<MapPointHistoryEntry>((Func<MapPointHistoryEntry, bool>) (p => p.MapPointType == MapPointType.Unknown));
      if (num < 2)
        return RoomType.Event;
      if (num == 2)
        return RoomType.Monster;
    }
    IReadOnlySet<RoomType> hashSet = (IReadOnlySet<RoomType>) this._nonEventOdds.Keys.Append<RoomType>(RoomType.Event).Except<RoomType>(blacklist).ToHashSet<RoomType>();
    IReadOnlySet<RoomType> ireadOnlySet = Hook.ModifyUnknownMapPointRoomTypes(runState, hashSet);
    RoomType roomType1 = ireadOnlySet.Contains(RoomType.Event) ? RoomType.Event : Enumerable.Order<RoomType>((IEnumerable<RoomType>) ireadOnlySet).First<RoomType>();
    float num1 = this._rng.NextFloat();
    float num2 = 0.0f;
    RoomType key;
    float num3;
    foreach (KeyValuePair<RoomType, float> nonEventOdd in this._nonEventOdds)
    {
      nonEventOdd.Deconstruct(ref key, ref num3);
      RoomType roomType2 = key;
      float num4 = num3;
      if (ireadOnlySet.Contains(roomType2) && (double) num4 >= 0.0)
      {
        num2 += num4;
        if ((double) num1 <= (double) num2)
        {
          roomType1 = roomType2;
          break;
        }
      }
    }
    foreach (KeyValuePair<RoomType, float> baseOdd in this._baseOdds)
    {
      baseOdd.Deconstruct(ref key, ref num3);
      RoomType roomType3 = key;
      float oddsIncrease = num3;
      if (roomType1 == roomType3)
        this._nonEventOdds[roomType3] = oddsIncrease;
      else if (ireadOnlySet.Contains(roomType3))
      {
        float num5 = Hook.ModifyOddsIncreaseForUnrolledRoomType(runState, roomType3, oddsIncrease);
        Dictionary<RoomType, float> nonEventOdds = this._nonEventOdds;
        key = roomType3;
        nonEventOdds[key] += num5;
      }
    }
    return roomType1;
  }

  public void ResetToBase()
  {
    foreach (KeyValuePair<RoomType, float> baseOdd in this._baseOdds)
    {
      RoomType key;
      float num;
      baseOdd.Deconstruct(ref key, ref num);
      this._nonEventOdds[key] = num;
    }
  }
}
