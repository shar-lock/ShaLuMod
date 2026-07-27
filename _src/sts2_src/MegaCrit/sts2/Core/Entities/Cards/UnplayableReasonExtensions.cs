// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Entities.Cards.UnplayableReasonExtensions
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Models;
using System;

#nullable enable
namespace MegaCrit.Sts2.Core.Entities.Cards;

internal static class UnplayableReasonExtensions
{
  public static bool HasResourceCostReason(this UnplayableReason reason)
  {
    return reason.HasFlag((Enum) UnplayableReason.EnergyCostTooHigh) || reason.HasFlag((Enum) UnplayableReason.StarCostTooHigh);
  }

  public static LocString? GetPlayerDialogueLine(
    this UnplayableReason reason,
    AbstractModel? preventer = null)
  {
    if (reason.HasFlag((Enum) UnplayableReason.NoLivingAllies))
      return new LocString("combat_messages", "NO_LIVING_ALLIES");
    if (reason.HasFlag((Enum) UnplayableReason.EnergyCostTooHigh))
      return new LocString("combat_messages", "NOT_ENOUGH_ENERGY");
    if (reason.HasFlag((Enum) UnplayableReason.StarCostTooHigh))
      return new LocString("combat_messages", "NOT_ENOUGH_STARS");
    if (reason.HasFlag((Enum) UnplayableReason.BlockedByHook))
    {
      if (preventer == null)
        return new LocString("combat_messages", "UNPLAYABLE");
      LocString playerDialogueLine = new LocString("combat_messages", "BLOCKED_BY_HOOK");
      string variable = preventer is CardModel cardModel ? cardModel.Title : (preventer is RelicModel relicModel ? relicModel.Title.GetFormattedText() : (preventer is PowerModel powerModel ? powerModel.Title.GetFormattedText() : (preventer is EnchantmentModel enchantmentModel ? enchantmentModel.Title.GetFormattedText() : (preventer is AfflictionModel afflictionModel ? afflictionModel.Title.GetFormattedText() : (string) null))));
      if (variable == null)
      {
        Log.Error($"Missing case for model {preventer} in UnplayableReason.GetPlayerDialogueLine switch!");
        variable = "<UNKNOWN>";
      }
      playerDialogueLine.Add("BlockingHook", variable);
      return playerDialogueLine;
    }
    return reason.HasFlag((Enum) UnplayableReason.HasUnplayableKeyword) || reason.HasFlag((Enum) UnplayableReason.BlockedByCardLogic) ? new LocString("combat_messages", "UNPLAYABLE") : (LocString) null;
  }
}
