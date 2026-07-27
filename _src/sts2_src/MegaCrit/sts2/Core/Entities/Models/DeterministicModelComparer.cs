// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Entities.Models.DeterministicModelComparer
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.Models;
using System;
using System.Collections.Generic;

#nullable enable
namespace MegaCrit.Sts2.Core.Entities.Models;

public class DeterministicModelComparer : IComparer<AbstractModel>
{
  public static DeterministicModelComparer Instance { get; } = new DeterministicModelComparer();

  private DeterministicModelComparer()
  {
  }

  public int Compare(AbstractModel? model1, AbstractModel? model2)
  {
    if (model1 == null && model2 == null || model1 == model2)
      return 0;
    if (model1 == null)
      return -1;
    if (model2 == null)
      return 1;
    if (model1.CategorySortingId != model2.CategorySortingId)
      return model1.CategorySortingId.CompareTo(model2.CategorySortingId);
    if (model1.EntrySortingId != model2.EntrySortingId)
      return model1.EntrySortingId.CompareTo(model2.EntrySortingId);
    Creature owner1 = DeterministicModelComparer.GetOwner(model1);
    Creature owner2 = DeterministicModelComparer.GetOwner(model2);
    if (owner1 == null && owner2 == null)
      return 0;
    if (owner1 == null)
      return -1;
    if (owner2 == null)
      return 1;
    if (owner1 == owner2)
      return DeterministicModelComparer.CompareModelsWithSameOwnerAndId(model1, model2);
    if (owner1.IsPlayer && owner2.IsPlayer)
      return owner1.Player.NetId.CompareTo(owner2.Player.NetId);
    uint? combatId = owner1.CombatId;
    if (combatId.HasValue)
    {
      combatId = owner2.CombatId;
      if (combatId.HasValue)
      {
        combatId = owner1.CombatId;
        uint num1 = combatId.Value;
        ref uint local = ref num1;
        combatId = owner2.CombatId;
        int num2 = (int) combatId.Value;
        return local.CompareTo((uint) num2);
      }
    }
    combatId = owner1.CombatId;
    return combatId.HasValue ? 1 : -1;
  }

  private static Creature? GetOwner(AbstractModel model)
  {
    Creature owner;
    switch (model)
    {
      case CardModel cardModel:
        owner = cardModel.Owner.Creature;
        break;
      case RelicModel relicModel:
        owner = relicModel.Owner.Creature;
        break;
      case PotionModel potionModel:
        owner = potionModel.Owner.Creature;
        break;
      case PowerModel powerModel:
        owner = powerModel.Owner;
        break;
      case AfflictionModel afflictionModel:
        owner = afflictionModel.Card.Owner.Creature;
        break;
      case EnchantmentModel enchantmentModel:
        owner = enchantmentModel.Card.Owner.Creature;
        break;
      default:
        owner = (Creature) null;
        break;
    }
    return owner;
  }

  private static int CompareModelsWithSameOwnerAndId(AbstractModel model1, AbstractModel model2)
  {
    switch (model1)
    {
      case CardModel cardModel1 when model2 is CardModel cardModel2:
        return DeterministicModelComparer.CompareCardModelsWithSameOwnerAndId(cardModel1, cardModel2);
      case AfflictionModel afflictionModel2 when model2 is AfflictionModel afflictionModel1:
        return DeterministicModelComparer.CompareCardModelsWithSameOwnerAndId(afflictionModel2.Card, afflictionModel1.Card);
      case EnchantmentModel enchantmentModel2 when model2 is EnchantmentModel enchantmentModel1:
        return DeterministicModelComparer.CompareCardModelsWithSameOwnerAndId(enchantmentModel2.Card, enchantmentModel1.Card);
      case RelicModel relicModel2 when model2 is RelicModel relicModel1:
        return relicModel2.Owner.Relics.IndexOf<RelicModel>(relicModel2).CompareTo(relicModel1.Owner.Relics.IndexOf<RelicModel>(relicModel1));
      case PotionModel potionModel2 when model2 is PotionModel potionModel1:
        return potionModel2.Owner.PotionSlots.IndexOf<PotionModel>(potionModel2).CompareTo(potionModel1.Owner.PotionSlots.IndexOf<PotionModel>(potionModel1));
      case PowerModel powerModel2 when model2 is PowerModel powerModel1:
        return powerModel2.Owner.Powers.IndexOf<PowerModel>(powerModel2).CompareTo(powerModel1.Owner.Powers.IndexOf<PowerModel>(powerModel1));
      default:
        throw new InvalidOperationException($"Tried to compare equivalent models {model1} and {model2} but we can't map them to any know model types!");
    }
  }

  private static int CompareCardModelsWithSameOwnerAndId(CardModel cardModel1, CardModel cardModel2)
  {
    if (cardModel1.Pile == null && cardModel2.Pile == null)
      return 0;
    if (cardModel1.Pile == null)
      return 1;
    if (cardModel2.Pile == null)
      return -1;
    return cardModel1.Pile.Type != cardModel2.Pile.Type ? cardModel1.Pile.Type.CompareTo((object) cardModel2.Pile.Type) : cardModel1.Pile.Cards.IndexOf<CardModel>(cardModel1).CompareTo(cardModel2.Pile.Cards.IndexOf<CardModel>(cardModel2));
  }
}
