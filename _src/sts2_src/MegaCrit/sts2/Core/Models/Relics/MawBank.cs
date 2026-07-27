// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Relics.MawBank
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Merchant;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Saves.Runs;
using System.Collections.Generic;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Relics;

public sealed class MawBank : RelicModel
{
  private bool _hasItemBeenBought;

  public override RelicRarity Rarity => RelicRarity.Event;

  protected override IEnumerable<DynamicVar> CanonicalVars
  {
    get
    {
      return (IEnumerable<DynamicVar>) new \u003C\u003Ez__ReadOnlySingleElementList<DynamicVar>((DynamicVar) new GoldVar(12));
    }
  }

  public override bool ShouldFlashOnPlayer => false;

  public override bool IsUsedUp => this.HasItemBeenBought;

  [SavedProperty]
  public bool HasItemBeenBought
  {
    get => this._hasItemBeenBought;
    set
    {
      this.AssertMutable();
      this._hasItemBeenBought = value;
      if (!this.IsUsedUp)
        return;
      this.Status = RelicStatus.Disabled;
    }
  }

  public override async Task AfterRoomEntered(AbstractRoom room)
  {
    if (this.Owner.RunState.BaseRoom != room || this.HasItemBeenBought)
      return;
    this.Flash();
    await PlayerCmd.GainGold(this.DynamicVars.Gold.BaseValue, this.Owner);
  }

  public override Task AfterItemPurchased(
    Player player,
    MerchantEntry itemPurchased,
    int goldSpent)
  {
    if (player != this.Owner || this.HasItemBeenBought || goldSpent <= 0)
      return Task.CompletedTask;
    this.Flash();
    this.HasItemBeenBought = true;
    return Task.CompletedTask;
  }
}
