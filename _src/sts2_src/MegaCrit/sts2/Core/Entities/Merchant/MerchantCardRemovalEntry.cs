// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Entities.Merchant.MerchantCardRemovalEntry
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Entities.Ascension;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Hooks;
using MegaCrit.Sts2.Core.Nodes;
using MegaCrit.Sts2.Core.Runs;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Entities.Merchant;

public sealed class MerchantCardRemovalEntry : MerchantEntry
{
  public bool Used { get; private set; }

  public override bool IsStocked => !this.Used;

  private static int BaseCost
  {
    get => AscensionHelper.GetValueIfAscension(AscensionLevel.Inflation, 100, 75);
  }

  public static int PriceIncrease
  {
    get => AscensionHelper.GetValueIfAscension(AscensionLevel.Inflation, 50, 25);
  }

  public MerchantCardRemovalEntry(Player player)
    : base(player)
  {
    this.CalcCost();
  }

  public override void CalcCost()
  {
    this._cost = MerchantCardRemovalEntry.BaseCost + MerchantCardRemovalEntry.PriceIncrease * this._player.ExtraFields.CardShopRemovalsUsed;
  }

  public async Task<bool> OnTryPurchaseWrapper(
    MerchantInventory? inventory,
    bool ignoreCost = false,
    bool cancelable = true)
  {
    if (!RunManager.Instance.IsInProgress)
      return false;
    if (!this.EnoughGold && !ignoreCost)
    {
      this.InvokePurchaseFailed(PurchaseStatus.FailureGold);
      return false;
    }
    (bool flag, int goldSpent) = await this.OnTryPurchase(inventory, ignoreCost, cancelable);
    if (flag)
    {
      await Hook.AfterItemPurchased(this._player.RunState, this._player, (MerchantEntry) this, goldSpent);
      this.InvokePurchaseCompleted((MerchantEntry) this);
    }
    return flag;
  }

  protected override async Task<(bool, int)> OnTryPurchase(
    MerchantInventory? inventory,
    bool ignoreCost)
  {
    return await this.OnTryPurchase(inventory, ignoreCost, true);
  }

  private async Task<(bool, int)> OnTryPurchase(
    MerchantInventory? inventory,
    bool ignoreCost,
    bool cancelable)
  {
    if (this.Used)
      return (false, 0);
    int goldToSpend = ignoreCost ? 0 : this.Cost;
    bool flag = await RunManager.Instance.OneOffSynchronizer.DoLocalMerchantCardRemoval(goldToSpend, cancelable);
    if (flag)
      NRun.Instance?.MerchantRoom?.Inventory.OnCardRemovalUsed();
    return (flag, goldToSpend);
  }

  protected override void ClearAfterPurchase()
  {
  }

  protected override void RestockAfterPurchase(MerchantInventory? inventory)
  {
  }

  public void SetUsed() => this.Used = true;
}
