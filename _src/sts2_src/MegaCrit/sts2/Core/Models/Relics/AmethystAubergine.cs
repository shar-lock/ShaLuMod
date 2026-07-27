// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Relics.AmethystAubergine
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Rewards;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Runs;
using System.Collections.Generic;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Relics;

public sealed class AmethystAubergine : RelicModel
{
  public override RelicRarity Rarity => RelicRarity.Common;

  public override bool IsAllowed(IRunState runState)
  {
    return RelicModel.IsBeforeAct3TreasureChest(runState);
  }

  public override bool IsAllowedInShops => false;

  protected override IEnumerable<DynamicVar> CanonicalVars
  {
    get
    {
      return (IEnumerable<DynamicVar>) new \u003C\u003Ez__ReadOnlySingleElementList<DynamicVar>((DynamicVar) new GoldVar(15));
    }
  }

  public override bool TryModifyRewards(Player player, List<Reward> rewards, AbstractRoom? room)
  {
    if (player != this.Owner || room == null || !room.RoomType.IsCombatRoom() || room.RoomType == RoomType.Boss && player.RunState.CurrentActIndex >= player.RunState.Acts.Count - 1)
      return false;
    rewards.Add((Reward) new GoldReward(this.DynamicVars.Gold.IntValue, player));
    return true;
  }

  public override Task AfterModifyingRewards()
  {
    this.Flash();
    return Task.CompletedTask;
  }
}
