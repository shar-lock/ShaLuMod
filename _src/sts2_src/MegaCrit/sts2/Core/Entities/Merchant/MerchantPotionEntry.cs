// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Entities.Merchant.MerchantPotionEntry
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Gold;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Potions;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.Saves;
using MegaCrit.Sts2.Core.TestSupport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Entities.Merchant;

public sealed class MerchantPotionEntry : MerchantEntry
{
  public PotionModel? Model { get; private set; }

  public override bool IsStocked => this.Model != null;

  public MerchantPotionEntry(PotionModel potion, Player player)
    : base(player)
  {
    potion.AssertMutable();
    this.Model = potion;
    this.CalcCost();
    SaveManager.Instance.MarkPotionAsSeen(this.Model);
  }

  public MerchantPotionEntry(Player player)
    : base(player)
  {
    this.FillSlot((IEnumerable<PotionModel>) Array.Empty<PotionModel>());
  }

  private void FillSlot(IEnumerable<PotionModel> blacklist)
  {
    this.Model = PotionFactory.CreateRandomPotionOutOfCombat(this._player, this._player.PlayerRng.Shops, blacklist).ToMutable();
    this.CalcCost();
    SaveManager.Instance.MarkPotionAsSeen(this.Model);
  }

  private static int GetCost(PotionRarity rarity)
  {
    int cost;
    switch (rarity)
    {
      case PotionRarity.Uncommon:
        cost = 75;
        break;
      case PotionRarity.Rare:
        cost = 100;
        break;
      default:
        cost = 50;
        break;
    }
    return cost;
  }

  public override void CalcCost()
  {
    this._cost = this.Model != null ? MerchantPotionEntry.GetCost(this.Model.Rarity) : throw new InvalidOperationException("There is no item to purchase.");
    if (!TestMode.IsOff)
      return;
    this._cost = (int) Mathf.Round((float) this._cost * this._player.PlayerRng.Shops.NextFloat(0.95f, 1.05f));
  }

  protected override async Task<(bool, int)> OnTryPurchase(
    MerchantInventory? inventory,
    bool ignoreCost)
  {
    if (this.Model == null)
      throw new InvalidOperationException("There is no item to purchase.");
    PotionProcureResult procure = await PotionCmd.TryToProcure(this.Model, this._player);
    if (!procure.success)
    {
      this.InvokePurchaseFailed(procure.failureReason == PotionProcureFailureReason.NotAllowed ? PurchaseStatus.FailureForbidden : PurchaseStatus.FailureSpace);
      return (false, 0);
    }
    if (!ignoreCost)
      await PlayerCmd.LoseGold((Decimal) this.Cost, this._player, GoldLossType.Spent);
    this._player.RunState.CurrentMapPointHistoryEntry?.GetEntry(this._player.NetId).BoughtPotions.Add(this.Model.Id);
    RunManager.Instance.RewardSynchronizer.SyncLocalGoldLost(this.Cost);
    RunManager.Instance.RewardSynchronizer.SyncLocalObtainedPotion(this.Model);
    return (true, ignoreCost ? 0 : this.Cost);
  }

  protected override void ClearAfterPurchase() => this.Model = (PotionModel) null;

  protected override void RestockAfterPurchase(MerchantInventory? inventory)
  {
    HashSet<PotionModel> potionModelSet = (inventory != null ? inventory.PotionEntries.Select<MerchantPotionEntry, PotionModel>((Func<MerchantPotionEntry, PotionModel>) (e => e.Model?.CanonicalInstance)).OfType<PotionModel>().ToHashSet<PotionModel>() : (HashSet<PotionModel>) null) ?? new HashSet<PotionModel>();
    this.FillSlot((IEnumerable<PotionModel>) Array.Empty<PotionModel>());
  }
}
