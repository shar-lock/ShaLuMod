// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Relics.LavaRock
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Rewards;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Saves.Runs;
using System.Collections.Generic;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Relics;

public sealed class LavaRock : RelicModel
{
  private const string _relicsKey = "Relics";
  private bool _hasTriggered;

  public override RelicRarity Rarity => RelicRarity.Ancient;

  public override bool ShowCounter => false;

  [SavedProperty]
  public bool HasTriggered
  {
    get => this._hasTriggered;
    set
    {
      this.AssertMutable();
      this._hasTriggered = value;
      this.InvokeDisplayAmountChanged();
    }
  }

  protected override IEnumerable<DynamicVar> CanonicalVars
  {
    get
    {
      return (IEnumerable<DynamicVar>) new \u003C\u003Ez__ReadOnlySingleElementList<DynamicVar>(new DynamicVar("Relics", 2M));
    }
  }

  public override bool TryModifyRewards(Player player, List<Reward> rewards, AbstractRoom? room)
  {
    if (player != this.Owner || (room != null ? (room.RoomType != RoomType.Boss ? 1 : 0) : 1) != 0 || this.Owner.RunState.CurrentActIndex != 0 || this.HasTriggered)
      return false;
    this.Flash();
    for (int index = 0; index < this.DynamicVars["Relics"].IntValue; ++index)
      rewards.Add((Reward) new RelicReward(player));
    this.HasTriggered = true;
    this.Status = RelicStatus.Disabled;
    return true;
  }
}
