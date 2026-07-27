// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Helpers.Models.CardCostHelper
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Models;
using System;

#nullable enable
namespace MegaCrit.Sts2.Core.Helpers.Models;

public static class CardCostHelper
{
  public static CardCostColor GetEnergyCostColor(CardModel card, ICombatState? state)
  {
    if (state == null)
      return CardCostColor.Unmodified;
    UnplayableReason reason;
    if (!card.CanPlay(out reason, out AbstractModel _) && reason.HasFlag((Enum) UnplayableReason.EnergyCostTooHigh))
      return CardCostColor.InsufficientResources;
    if (card.EnergyCost.CostsX)
      return CardCostColor.Unmodified;
    Decimal hookModifiedCost;
    if (CardCostHelper.TryModifyEnergyCostWithHooks(card, state, out hookModifiedCost))
      return CardCostHelper.GetColorForHookModifiedCost(hookModifiedCost, card.EnergyCost.GetWithModifiers(CostModifiers.None));
    return card.EnergyCost.HasLocalModifiers ? CardCostHelper.GetColorForLocalCost(card.EnergyCost.GetWithModifiers(CostModifiers.Local), card.EnergyCost.GetWithModifiers(CostModifiers.None)) : CardCostColor.Unmodified;
  }

  public static CardCostColor GetStarCostColor(CardModel card, ICombatState? state)
  {
    if (state == null)
      return CardCostColor.Unmodified;
    UnplayableReason reason;
    if (!card.CanPlay(out reason, out AbstractModel _) && reason.HasFlag((Enum) UnplayableReason.StarCostTooHigh))
      return CardCostColor.InsufficientResources;
    if (card.HasStarCostX)
      return CardCostColor.Unmodified;
    Decimal hookModifiedCost;
    if (CardCostHelper.TryModifyStarCostWithHooks(card, state, out hookModifiedCost))
      return CardCostHelper.GetColorForHookModifiedCost(hookModifiedCost, card.BaseStarCost);
    return card.TemporaryStarCost != null ? CardCostHelper.GetColorForLocalCost(card.TemporaryStarCost.Cost, card.BaseStarCost) : CardCostColor.Unmodified;
  }

  private static CardCostColor GetColorForLocalCost(int localCost, int baseCost)
  {
    if (localCost > baseCost)
      return CardCostColor.Increased;
    return localCost < baseCost ? CardCostColor.Decreased : CardCostColor.Unmodified;
  }

  private static CardCostColor GetColorForHookModifiedCost(Decimal hookModifiedCost, int baseCost)
  {
    if (hookModifiedCost > (Decimal) baseCost)
      return CardCostColor.Increased;
    return hookModifiedCost < (Decimal) baseCost ? CardCostColor.Decreased : CardCostColor.Unmodified;
  }

  private static bool TryModifyEnergyCostWithHooks(
    CardModel card,
    ICombatState state,
    out Decimal hookModifiedCost)
  {
    hookModifiedCost = (Decimal) card.EnergyCost.GetWithModifiers(CostModifiers.None);
    bool flag = false;
    foreach (AbstractModel iterateHookListener in state.IterateHookListeners())
      flag |= iterateHookListener.TryModifyEnergyCostInCombat(card, hookModifiedCost, out hookModifiedCost);
    foreach (AbstractModel iterateHookListener in state.IterateHookListeners())
      flag |= iterateHookListener.TryModifyEnergyCostInCombatLate(card, hookModifiedCost, out hookModifiedCost);
    return flag;
  }

  private static bool TryModifyStarCostWithHooks(
    CardModel card,
    ICombatState state,
    out Decimal hookModifiedCost)
  {
    hookModifiedCost = (Decimal) card.BaseStarCost;
    bool flag = false;
    foreach (AbstractModel iterateHookListener in state.IterateHookListeners())
      flag |= iterateHookListener.TryModifyStarCost(card, hookModifiedCost, out hookModifiedCost);
    return flag;
  }
}
