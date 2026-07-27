// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.CardSelection.CardSelectorPrefs
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models;
using System;

#nullable enable
namespace MegaCrit.Sts2.Core.CardSelection;

public struct CardSelectorPrefs
{
  private const string _cardSelectionLocFilePath = "card_selection";

  public static LocString TransformSelectionPrompt
  {
    get => new LocString("card_selection", "TO_TRANSFORM");
  }

  public static LocString ExhaustSelectionPrompt => new LocString("card_selection", "TO_EXHAUST");

  public static LocString RemoveSelectionPrompt => new LocString("card_selection", "TO_REMOVE");

  public static LocString EnchantSelectionPrompt => new LocString("card_selection", "TO_ENCHANT");

  public static LocString DiscardSelectionPrompt => new LocString("card_selection", "TO_DISCARD");

  public static LocString UpgradeSelectionPrompt => new LocString("card_selection", "TO_UPGRADE");

  public LocString Prompt { get; }

  public int MinSelect { get; }

  public int MaxSelect { get; }

  public bool RequireManualConfirmation { get; init; }

  public bool Cancelable { get; init; }

  public System.Comparison<CardModel>? Comparison { get; init; }

  public bool UnpoweredPreviews { get; init; }

  public bool PretendCardsCanBePlayed { get; init; }

  public Func<CardModel, bool>? ShouldGlowGold { get; set; }

  public CardSelectorPrefs(LocString prompt, int selectCount)
    : this(prompt, selectCount, selectCount)
  {
    this.Prompt.Add("Amount", (Decimal) selectCount);
  }

  public CardSelectorPrefs(LocString prompt, int minCount, int maxCount)
    : this()
  {
    this.Prompt = prompt;
    this.Prompt.Add("Amount", (Decimal) maxCount);
    this.Prompt.Add("MinCount", (Decimal) minCount);
    this.Prompt.Add("MaxCount", (Decimal) maxCount);
    this.MaxSelect = maxCount;
    this.MinSelect = minCount;
    this.RequireManualConfirmation = this.MinSelect >= 0 && this.MinSelect != this.MaxSelect;
  }
}
