// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Entities.Merchant.MerchantEntry
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Hooks;
using MegaCrit.Sts2.Core.Rooms;
using System;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Entities.Merchant;

public abstract class MerchantEntry
{
  protected readonly Player _player;
  protected int _cost;

  public int Cost
  {
    get
    {
      Decimal cost = (Decimal) this._cost;
      if (this._player.RunState.CurrentRoom is MerchantRoom)
        cost = Hook.ModifyMerchantPrice(this._player.RunState, this._player, this, (Decimal) this._cost);
      return (int) cost;
    }
  }

  public bool EnoughGold => this.Cost <= this._player.Gold;

  public abstract bool IsStocked { get; }

  public event Action<PurchaseStatus, MerchantEntry>? PurchaseCompleted;

  public void InvokePurchaseCompleted(MerchantEntry entry)
  {
    Action<PurchaseStatus, MerchantEntry> purchaseCompleted = this.PurchaseCompleted;
    if (purchaseCompleted == null)
      return;
    purchaseCompleted(PurchaseStatus.Success, entry);
  }

  public event Action<PurchaseStatus>? PurchaseFailed;

  public void InvokePurchaseFailed(PurchaseStatus status)
  {
    Action<PurchaseStatus> purchaseFailed = this.PurchaseFailed;
    if (purchaseFailed == null)
      return;
    purchaseFailed(status);
  }

  public event Action? EntryUpdated;

  protected MerchantEntry(Player player) => this._player = player;

  protected virtual void UpdateEntry()
  {
  }

  public void OnMerchantInventoryUpdated()
  {
    this.UpdateEntry();
    Action entryUpdated = this.EntryUpdated;
    if (entryUpdated == null)
      return;
    entryUpdated();
  }

  public abstract void CalcCost();

  public async Task<bool> OnTryPurchaseWrapper(MerchantInventory? inventory, bool ignoreCost = false)
  {
    if (!this.IsStocked)
    {
      this.InvokePurchaseFailed(PurchaseStatus.FailureOutOfStock);
      return false;
    }
    if (!this.EnoughGold && !ignoreCost)
    {
      this.InvokePurchaseFailed(PurchaseStatus.FailureGold);
      return false;
    }
    (bool flag, int goldSpent) = await this.OnTryPurchase(inventory, ignoreCost);
    if (flag)
    {
      if (this._player.RunState.CurrentRoom is MerchantRoom && Hook.ShouldRefillMerchantEntry(this._player.RunState, this, this._player))
        this.RestockAfterPurchase(inventory);
      else
        this.ClearAfterPurchase();
      await Hook.AfterItemPurchased(this._player.RunState, this._player, this, goldSpent);
      this.InvokePurchaseCompleted(this);
    }
    return flag;
  }

  protected abstract Task<(bool, int)> OnTryPurchase(MerchantInventory? inventory, bool ignoreCost);

  protected abstract void ClearAfterPurchase();

  protected abstract void RestockAfterPurchase(MerchantInventory? inventory);
}
