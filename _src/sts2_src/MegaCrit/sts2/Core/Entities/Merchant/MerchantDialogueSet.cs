// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Entities.Merchant.MerchantDialogueSet
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Localization;
using System;
using System.Collections.Generic;

#nullable enable
namespace MegaCrit.Sts2.Core.Entities.Merchant;

public class MerchantDialogueSet
{
  private readonly List<LocString> _welcomeLines = new List<LocString>();
  private readonly List<LocString> _openInventoryLines = new List<LocString>();
  private readonly List<LocString> _foulPotionLines = new List<LocString>();
  private readonly List<LocString> _playerDeadLines = new List<LocString>();
  private readonly List<LocString> _purchaseSuccessLines = new List<LocString>();
  private readonly List<LocString> _purchaseFailureGoldLines = new List<LocString>();
  private readonly List<LocString> _purchaseFailureSpaceLines = new List<LocString>();
  private readonly List<LocString> _purchaseFailureForbiddenLines = new List<LocString>();

  public IReadOnlyList<LocString> WelcomeLines => (IReadOnlyList<LocString>) this._welcomeLines;

  public IReadOnlyList<LocString> FoulPotionLines
  {
    get => (IReadOnlyList<LocString>) this._foulPotionLines;
  }

  public IReadOnlyList<LocString> PlayerDeadLines
  {
    get => (IReadOnlyList<LocString>) this._playerDeadLines;
  }

  public IReadOnlyList<LocString> OpenInventoryLines
  {
    get => (IReadOnlyList<LocString>) this._openInventoryLines;
  }

  public static MerchantDialogueSet CreateFromLocStrings(IEnumerable<LocString> locStrings)
  {
    MerchantDialogueSet fromLocStrings = new MerchantDialogueSet();
    foreach (LocString locString in locStrings)
    {
      string[] strArray = locString.LocEntryKey.Split('.', StringSplitOptions.None);
      string str = strArray[strArray.Length - 2];
      if (str != null)
      {
        List<LocString> locStringList;
        switch (str.Length)
        {
          case 7:
            if (str == "welcome")
            {
              locStringList = fromLocStrings._welcomeLines;
              break;
            }
            goto label_21;
          case 10:
            switch (str[0])
            {
              case 'f':
                if (str == "foulPotion")
                {
                  locStringList = fromLocStrings._foulPotionLines;
                  break;
                }
                goto label_21;
              case 'p':
                if (str == "playerDead")
                {
                  locStringList = fromLocStrings._playerDeadLines;
                  break;
                }
                goto label_21;
              default:
                goto label_21;
            }
            break;
          case 13:
            if (str == "openInventory")
            {
              locStringList = fromLocStrings._openInventoryLines;
              break;
            }
            goto label_21;
          case 15:
            if (str == "purchaseSuccess")
            {
              locStringList = fromLocStrings._purchaseSuccessLines;
              break;
            }
            goto label_21;
          case 19:
            if (str == "purchaseFailureGold")
            {
              locStringList = fromLocStrings._purchaseFailureGoldLines;
              break;
            }
            goto label_21;
          case 20:
            if (str == "purchaseFailureSpace")
            {
              locStringList = fromLocStrings._purchaseFailureSpaceLines;
              break;
            }
            goto label_21;
          case 24:
            if (str == "purchaseFailureForbidden")
            {
              locStringList = fromLocStrings._purchaseFailureForbiddenLines;
              break;
            }
            goto label_21;
          default:
            goto label_21;
        }
        locStringList.Add(locString);
        continue;
      }
label_21:
      throw new InvalidOperationException("Unexpected merchant dialogue key: " + str);
    }
    return fromLocStrings;
  }

  public IReadOnlyList<LocString> GetPurchaseSuccessLines(PurchaseStatus status)
  {
    switch (status)
    {
      case PurchaseStatus.Success:
        return (IReadOnlyList<LocString>) this._purchaseSuccessLines;
      case PurchaseStatus.FailureGold:
        return (IReadOnlyList<LocString>) this._purchaseFailureGoldLines;
      case PurchaseStatus.FailureSpace:
        return (IReadOnlyList<LocString>) this._purchaseFailureSpaceLines;
      case PurchaseStatus.FailureForbidden:
        return (IReadOnlyList<LocString>) this._purchaseFailureForbiddenLines;
      default:
        throw new ArgumentOutOfRangeException(nameof (status), (object) status, (string) null);
    }
  }
}
