// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Odds.RunOddsSet
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Random;
using MegaCrit.Sts2.Core.Saves.Runs;

#nullable enable
namespace MegaCrit.Sts2.Core.Odds;

public class RunOddsSet
{
  public UnknownMapPointOdds UnknownMapPoint { get; private init; }

  private RunOddsSet()
  {
  }

  public RunOddsSet(Rng unknownMapPointRng)
  {
    this.UnknownMapPoint = new UnknownMapPointOdds(unknownMapPointRng);
  }

  public SerializableRunOddsSet ToSerializable()
  {
    return new SerializableRunOddsSet()
    {
      UnknownMapPointMonsterOddsValue = this.UnknownMapPoint.MonsterOdds,
      UnknownMapPointEliteOddsValue = this.UnknownMapPoint.EliteOdds,
      UnknownMapPointTreasureOddsValue = this.UnknownMapPoint.TreasureOdds,
      UnknownMapPointShopOddsValue = this.UnknownMapPoint.ShopOdds
    };
  }

  public static RunOddsSet FromSerializable(SerializableRunOddsSet save, Rng unknownMapPointRng)
  {
    return new RunOddsSet()
    {
      UnknownMapPoint = new UnknownMapPointOdds(unknownMapPointRng)
      {
        MonsterOdds = save.UnknownMapPointMonsterOddsValue,
        EliteOdds = save.UnknownMapPointEliteOddsValue,
        TreasureOdds = save.UnknownMapPointTreasureOddsValue,
        ShopOdds = save.UnknownMapPointShopOddsValue
      }
    };
  }
}
