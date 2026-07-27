// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Entities.Merchant.MerchantRelicEntry
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Gold;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.Saves;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Entities.Merchant;

public sealed class MerchantRelicEntry : MerchantEntry
{
  public RelicModel? Model { get; private set; }

  public override bool IsStocked => this.Model != null;

  public MerchantRelicEntry(RelicRarity rarity, Player player)
    : base(player)
  {
    this.FillSlot(rarity);
  }

  public MerchantRelicEntry(RelicModel relic, Player player)
    : base(player)
  {
    this.SetModel(relic);
  }

  private void FillSlot(RelicRarity rarity, IEnumerable<RelicModel>? blacklist = null)
  {
    this.SetModel(RelicFactory.PullNextRelicFromBack(this._player, rarity, (Func<RelicModel, bool>) (r => (blacklist == null || !blacklist.Contains<RelicModel>(r)) && r.IsAllowedInShops)).ToMutable());
  }

  public override void CalcCost()
  {
    this._cost = (int) Math.Round((double) this.Model.MerchantCost * (double) this._player.PlayerRng.Shops.NextFloat(0.85f, 1.15f));
  }

  private void SetModel(RelicModel model)
  {
    model.AssertMutable();
    this.Model = model;
    this.CalcCost();
    SaveManager.Instance.MarkRelicAsSeen(this.Model);
  }

  protected override async Task<(bool, int)> OnTryPurchase(
    MerchantInventory? inventory,
    bool ignoreCost)
  {
    if (!ignoreCost)
      await PlayerCmd.LoseGold((Decimal) this.Cost, this._player, GoldLossType.Spent);
    this._player.RunState.CurrentMapPointHistoryEntry?.GetEntry(this._player.NetId).BoughtRelics.Add(this.Model.Id);
    RelicModel relicModel = await RelicCmd.Obtain(this.Model, this._player);
    RunManager.Instance.RewardSynchronizer.SyncLocalGoldLost(this.Cost);
    RunManager.Instance.RewardSynchronizer.SyncLocalObtainedRelic(this.Model);
    return (true, ignoreCost ? 0 : this.Cost);
  }

  protected override void ClearAfterPurchase() => this.Model = (RelicModel) null;

  protected override void RestockAfterPurchase(MerchantInventory? inventory)
  {
    HashSet<RelicModel> blacklist = (inventory != null ? inventory.RelicEntries.Select<MerchantRelicEntry, RelicModel>((Func<MerchantRelicEntry, RelicModel>) (e => e.Model?.CanonicalInstance)).OfType<RelicModel>().ToHashSet<RelicModel>() : (HashSet<RelicModel>) null) ?? new HashSet<RelicModel>();
    this.FillSlot(RelicFactory.RollRarity(this._player), (IEnumerable<RelicModel>) blacklist);
  }
}
