// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Relics.LordsParasol
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Debug;
using MegaCrit.Sts2.Core.Entities.Merchant;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Nodes;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Screens.Map;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.TestSupport;
using Sentry;
using System.Collections.Generic;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Relics;

public sealed class LordsParasol : RelicModel
{
  public override RelicRarity Rarity => RelicRarity.Ancient;

  public override Task AfterRoomEntered(AbstractRoom room)
  {
    if (!(room is MerchantRoom merchantRoom))
      return Task.CompletedTask;
    TaskHelper.RunSafely(this.PurchaseEverything(merchantRoom.GetLocalInventory()));
    return Task.CompletedTask;
  }

  private async Task PurchaseEverything(MerchantInventory inventory)
  {
    if (inventory.Player != this.Owner)
      return;
    bool uiBlocked = false;
    try
    {
      if (TestMode.IsOff)
      {
        NRun.Instance?.GlobalUi.TopBar.Map.Disable();
        NRun.Instance?.GlobalUi.TopBar.Deck.Disable();
        NMapScreen.Instance?.SetTravelEnabled(false);
        if (NRun.Instance != null)
        {
          double num = (double) await ((Node) NRun.Instance).AwaitProcessFrame();
        }
        uiBlocked = true;
        NMerchantRoom.Instance?.Inventory.BlockInput();
        await Cmd.Wait(0.75f);
        NMerchantRoom.Instance?.Inventory.Open();
        await Cmd.Wait(1f);
      }
      foreach (MerchantCardEntry characterCardEntry in (IEnumerable<MerchantCardEntry>) inventory.CharacterCardEntries)
      {
        if (!characterCardEntry.IsStocked)
        {
          SentryService.CaptureMessage("LordsParasol tried to buy an out-of-stock character card: " + (characterCardEntry.CreationResult?.Card.Id.Entry ?? "NULL"), (SentryLevel) 1);
        }
        else
        {
          int num = await characterCardEntry.OnTryPurchaseWrapper(inventory, true) ? 1 : 0;
          await Cmd.Wait(0.25f);
        }
      }
      foreach (MerchantCardEntry colorlessCardEntry in (IEnumerable<MerchantCardEntry>) inventory.ColorlessCardEntries)
      {
        if (!colorlessCardEntry.IsStocked)
        {
          SentryService.CaptureMessage("LordsParasol tried to buy an out-of-stock colorless card: " + (colorlessCardEntry.CreationResult?.Card.Id.Entry ?? "NULL"), (SentryLevel) 1);
        }
        else
        {
          int num = await colorlessCardEntry.OnTryPurchaseWrapper(inventory, true) ? 1 : 0;
          await Cmd.Wait(0.25f);
        }
      }
      foreach (MerchantRelicEntry relicEntry in (IEnumerable<MerchantRelicEntry>) inventory.RelicEntries)
      {
        NRun.Instance?.GlobalUi.TopBar.Map.Enable();
        NRun.Instance?.GlobalUi.TopBar.Deck.Enable();
        int num = await relicEntry.OnTryPurchaseWrapper(inventory, true) ? 1 : 0;
        NRun.Instance?.GlobalUi.TopBar.Deck.Disable();
        NRun.Instance?.GlobalUi.TopBar.Map.Disable();
        await Cmd.Wait(0.25f);
      }
      foreach (MerchantEntry potionEntry in (IEnumerable<MerchantPotionEntry>) inventory.PotionEntries)
      {
        int num = await potionEntry.OnTryPurchaseWrapper(inventory, true) ? 1 : 0;
        await Cmd.Wait(0.25f);
      }
    }
    finally
    {
      if (uiBlocked)
      {
        NMerchantRoom.Instance?.Inventory.UnblockInput();
        NRun.Instance?.GlobalUi.TopBar.Map.Enable();
        NRun.Instance?.GlobalUi.TopBar.Deck.Enable();
        NMapScreen.Instance?.SetTravelEnabled(true);
      }
    }
    if (inventory.CardRemovalEntry == null || !RunManager.Instance.IsInProgress)
      return;
    NMapScreen.Instance?.SetTravelEnabled(false);
    int num1 = await inventory.CardRemovalEntry.OnTryPurchaseWrapper(inventory, true, false) ? 1 : 0;
    NMapScreen.Instance?.SetTravelEnabled(true);
  }
}
