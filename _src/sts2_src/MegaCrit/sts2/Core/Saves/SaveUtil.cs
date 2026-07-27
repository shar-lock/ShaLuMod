// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Saves.SaveUtil
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Acts;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.Characters;
using MegaCrit.Sts2.Core.Models.Enchantments;
using MegaCrit.Sts2.Core.Models.Encounters;
using MegaCrit.Sts2.Core.Models.Events;
using MegaCrit.Sts2.Core.Models.Modifiers;
using MegaCrit.Sts2.Core.Models.Monsters;
using MegaCrit.Sts2.Core.Models.Potions;
using MegaCrit.Sts2.Core.Models.Relics;

#nullable enable
namespace MegaCrit.Sts2.Core.Saves;

public static class SaveUtil
{
  public static EventModel EventOrDeprecated(ModelId id)
  {
    return ModelDb.GetByIdOrNull<EventModel>(id) ?? (EventModel) ModelDb.Event<DeprecatedEvent>();
  }

  public static AncientEventModel AncientEventOrDeprecated(ModelId id)
  {
    return ModelDb.GetByIdOrNull<AncientEventModel>(id) ?? (AncientEventModel) ModelDb.Event<DeprecatedAncientEvent>();
  }

  public static EncounterModel EncounterOrDeprecated(ModelId id)
  {
    return ModelDb.GetByIdOrNull<EncounterModel>(id) ?? (EncounterModel) ModelDb.Encounter<DeprecatedEncounter>();
  }

  public static CardModel CardOrDeprecated(ModelId id)
  {
    return ModelDb.GetByIdOrNull<CardModel>(id) ?? (CardModel) ModelDb.Card<DeprecatedCard>();
  }

  public static RelicModel RelicOrDeprecated(ModelId id)
  {
    return ModelDb.GetByIdOrNull<RelicModel>(id) ?? (RelicModel) ModelDb.Relic<DeprecatedRelic>();
  }

  public static PotionModel PotionOrDeprecated(ModelId id)
  {
    return ModelDb.GetByIdOrNull<PotionModel>(id) ?? (PotionModel) ModelDb.Potion<DeprecatedPotion>();
  }

  public static ModifierModel ModifierOrDeprecated(ModelId id)
  {
    return ModelDb.GetByIdOrNull<ModifierModel>(id) ?? (ModifierModel) ModelDb.Modifier<DeprecatedModifier>();
  }

  public static EnchantmentModel EnchantmentOrDeprecated(ModelId id)
  {
    return ModelDb.GetByIdOrNull<EnchantmentModel>(id) ?? (EnchantmentModel) ModelDb.Enchantment<DeprecatedEnchantment>();
  }

  public static MonsterModel MonsterOrDeprecated(ModelId id)
  {
    return ModelDb.GetByIdOrNull<MonsterModel>(id) ?? (MonsterModel) ModelDb.Monster<DeprecatedMonster>();
  }

  public static CharacterModel CharacterOrDeprecated(ModelId id)
  {
    return ModelDb.GetByIdOrNull<CharacterModel>(id) ?? (CharacterModel) ModelDb.Character<DeprecatedCharacter>();
  }

  public static ActModel ActOrDeprecated(ModelId id)
  {
    return ModelDb.GetByIdOrNull<ActModel>(id) ?? (ActModel) ModelDb.Act<DeprecatedAct>();
  }
}
