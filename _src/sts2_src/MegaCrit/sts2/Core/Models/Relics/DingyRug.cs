// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Relics.DingyRug
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Runs;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Relics;

public sealed class DingyRug : RelicModel
{
  public override RelicRarity Rarity => RelicRarity.Shop;

  public override CardCreationOptions ModifyCardRewardCreationOptions(
    Player player,
    CardCreationOptions options)
  {
    // ISSUE: object of a compiler-generated type is created
    return this.Owner != player || options.Flags.HasFlag((Enum) CardCreationFlags.NoCardPoolModifications) || !options.Flags.HasFlag((Enum) CardCreationFlags.IsCardReward) ? options : options.WithCardPools(options.CardPools.Union<CardPoolModel>((IEnumerable<CardPoolModel>) new \u003C\u003Ez__ReadOnlySingleElementList<CardPoolModel>((CardPoolModel) ModelDb.CardPool<ColorlessCardPool>())));
  }
}
