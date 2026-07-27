// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Relics.Shovel
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Entities.RestSite;
using MegaCrit.Sts2.Core.Runs;
using System.Collections.Generic;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Relics;

public sealed class Shovel : RelicModel
{
  public override RelicRarity Rarity => RelicRarity.Rare;

  public override bool IsAllowed(IRunState runState)
  {
    return RelicModel.IsBeforeAct3TreasureChest(runState);
  }

  public override bool TryModifyRestSiteOptions(Player player, ICollection<RestSiteOption> options)
  {
    if (player != this.Owner)
      return false;
    options.Add((RestSiteOption) new DigRestSiteOption(player));
    return true;
  }
}
