// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.ModelDb
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Models.Acts;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Models.Characters;
using MegaCrit.Sts2.Core.Models.Encounters;
using MegaCrit.Sts2.Core.Models.Events;
using MegaCrit.Sts2.Core.Models.Exceptions;
using MegaCrit.Sts2.Core.Models.Modifiers;
using MegaCrit.Sts2.Core.Models.Orbs;
using MegaCrit.Sts2.Core.Models.PotionPools;
using MegaCrit.Sts2.Core.Models.RelicPools;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.InteropServices;

#nullable enable
namespace MegaCrit.Sts2.Core.Models;

public static class ModelDb
{
  private const int _initialCapacity = 4096 /*0x1000*/;
  private static readonly Dictionary<ModelId, AbstractModel> _contentById = new Dictionary<ModelId, AbstractModel>(4096 /*0x1000*/);
  private static Type[]? _allAbstractModelSubtypes;
  private static IEnumerable<CardModel>? _allCards;
  private static IEnumerable<CardPoolModel>? _allCardPools;
  private static IEnumerable<CardPoolModel>? _allCharacterCardPools;
  private static IEnumerable<EventModel>? _allSharedEvents;
  private static IEnumerable<EventModel>? _allEvents;
  private static IEnumerable<EncounterModel>? _allEncounters;
  private static IEnumerable<EncounterModel>? _eventEncounters;
  private static IEnumerable<PotionModel>? _allPotions;
  private static IEnumerable<PotionPoolModel>? _allPotionPools;
  private static IEnumerable<PotionPoolModel>? _allCharacterPotionPools;
  private static IEnumerable<RelicPoolModel>? _allCharacterRelicPools;
  private static IEnumerable<PotionPoolModel>? _allSharedPotionPools;
  private static IEnumerable<PowerModel>? _allPowers;
  private static IEnumerable<RelicModel>? _allRelics;
  private static List<ActModel>? _acts;
  private static List<List<ActModel>>? _actsByIndex;
  private static List<BadgeModel>? _badges;
  private static List<AchievementModel>? _achievements;

  public static Type[] AllAbstractModelSubtypes
  {
    get
    {
      if (ModelDb._allAbstractModelSubtypes != null)
        return ModelDb._allAbstractModelSubtypes;
      HashSet<Type> source = new HashSet<Type>();
      foreach (Type type in (IEnumerable<Type>) AbstractModelSubtypes.All)
        source.Add(type);
      foreach (Type subtypesInMod in ReflectionHelper.GetSubtypesInMods<AbstractModel>())
        source.Add(subtypesInMod);
      ModelDb._allAbstractModelSubtypes = source.ToArray<Type>();
      return ModelDb._allAbstractModelSubtypes;
    }
  }

  public static IEnumerable<AbstractModel> All
  {
    get => (IEnumerable<AbstractModel>) ModelDb._contentById.Values;
  }

  public static void Init(Type[]? injectedModelTypes = null)
  {
    foreach (Type type in injectedModelTypes ?? ModelDb.AllAbstractModelSubtypes)
    {
      ModelId id = ModelDb.GetId(type);
      AbstractModel instance = (AbstractModel) Activator.CreateInstance(type);
      ModelDb._contentById[id] = instance;
    }
  }

  public static void Inject([DynamicallyAccessedMembers] Type type)
  {
    if (ModelDb.Contains(type))
      return;
    ModelId id = ModelDb.GetId(type);
    AbstractModel instance = (AbstractModel) Activator.CreateInstance(type);
    ModelDb._contentById[id] = instance;
  }

  public static void Remove(Type type)
  {
    ModelId id = ModelDb.GetId(type);
    ModelDb._contentById.Remove(id);
  }

  public static void ResetForTest()
  {
    ModelDb._allAbstractModelSubtypes = (Type[]) null;
    ModelDb._contentById.Clear();
  }

  public static void InitIds()
  {
    foreach (KeyValuePair<ModelId, AbstractModel> keyValuePair in ModelDb._contentById)
      keyValuePair.Value.InitId(keyValuePair.Key);
  }

  public static void Preload()
  {
    IEnumerable<CardModel> allCards = ModelDb.AllCards;
    IEnumerable<CardPoolModel> characterCardPools = ModelDb.AllCharacterCardPools;
    IEnumerable<EventModel> allSharedEvents = ModelDb.AllSharedEvents;
    IEnumerable<EventModel> allEvents = ModelDb.AllEvents;
    IEnumerable<RelicModel> allRelics = ModelDb.AllRelics;
    IEnumerable<PotionModel> allPotions = ModelDb.AllPotions;
    IEnumerable<EncounterModel> allEncounters = ModelDb.AllEncounters;
    IReadOnlyList<AchievementModel> achievements = ModelDb.Achievements;
    foreach (CardModel allCard in ModelDb.AllCards)
    {
      CardPoolModel pool = allCard.Pool;
      IEnumerable<string> allPortraitPaths = allCard.AllPortraitPaths;
    }
    foreach (RelicModel allRelic in ModelDb.AllRelics)
    {
      string iconPath = allRelic.IconPath;
    }
    foreach (PowerModel allPower in ModelDb.AllPowers)
    {
      string iconPath = allPower.IconPath;
      string resolvedBigIconPath = allPower.ResolvedBigIconPath;
    }
  }

  public static ModelId GetId<T>() where T : AbstractModel => ModelDb.GetId(typeof (T));

  public static ModelId GetId(Type type)
  {
    return new ModelId(ModelDb.GetCategory(type), ModelDb.GetEntry(type));
  }

  public static Type GetCategoryType(Type type)
  {
    Type categoryType = type;
    while (categoryType.BaseType != typeof (AbstractModel))
      categoryType = categoryType.BaseType;
    return categoryType;
  }

  public static string GetCategory(Type type)
  {
    return ModelId.SlugifyCategory(ModelDb.GetCategoryType(type).Name);
  }

  public static string GetEntry(Type type) => StringHelper.Slugify(type.Name);

  public static T? GetByIdOrNull<T>(ModelId id) where T : AbstractModel
  {
    AbstractModel abstractModel;
    return ModelDb._contentById.TryGetValue(id, out abstractModel) ? (T) abstractModel : default (T);
  }

  public static T GetById<T>(ModelId id) where T : AbstractModel
  {
    return ModelDb.GetByIdOrNull<T>(id) ?? throw new ModelNotFoundException(id);
  }

  public static bool Contains(Type type) => ModelDb._contentById.ContainsKey(ModelDb.GetId(type));

  private static T Get<T>() where T : AbstractModel => (T) ModelDb._contentById[ModelDb.GetId<T>()];

  private static AbstractModel Get(Type type)
  {
    ModelId modelId = type.IsSubclassOf(typeof (AbstractModel)) ? ModelDb.GetId(type) : throw new InvalidOperationException();
    AbstractModel abstractModel;
    if (ModelDb._contentById.TryGetValue(modelId, out abstractModel))
      return abstractModel;
    throw new ModelNotFoundException(modelId);
  }

  public static T Affliction<T>() where T : AfflictionModel => ModelDb.Get<T>();

  public static IEnumerable<AfflictionModel> DebugAfflictions
  {
    get
    {
      return ((IEnumerable<Type>) ModelDb.AllAbstractModelSubtypes).Where<Type>((Func<Type, bool>) (t => t.IsSubclassOf(typeof (AfflictionModel)))).Select<Type, AfflictionModel>((Func<Type, AfflictionModel>) (t => (AfflictionModel) ModelDb.Get(t)));
    }
  }

  public static T Enchantment<T>() where T : EnchantmentModel => ModelDb.Get<T>();

  public static IEnumerable<EnchantmentModel> DebugEnchantments
  {
    get
    {
      return ((IEnumerable<Type>) ModelDb.AllAbstractModelSubtypes).Where<Type>((Func<Type, bool>) (t => t.IsSubclassOf(typeof (EnchantmentModel)))).Select<Type, EnchantmentModel>((Func<Type, EnchantmentModel>) (t => (EnchantmentModel) ModelDb.Get(t)));
    }
  }

  public static T Card<T>() where T : CardModel => ModelDb.Get<T>();

  public static IEnumerable<CardModel> AllCards
  {
    get
    {
      IEnumerable<CardModel> allCards = ModelDb._allCards;
      if (allCards != null)
        return allCards;
      IEnumerable<CardModel> first = ModelDb.AllCardPools.SelectMany<CardPoolModel, CardModel>((Func<CardPoolModel, IEnumerable<CardModel>>) (p => p.AllCards));
      IEnumerable<CardModel> second = ModelDb.AllCharacters.SelectMany<CharacterModel, CardModel>((Func<CharacterModel, IEnumerable<CardModel>>) (c => c.StartingDeck)).Distinct<CardModel>();
      return ModelDb._allCards = first.Concat<CardModel>(second).Distinct<CardModel>();
    }
  }

  public static T CardPool<T>() where T : CardPoolModel => ModelDb.Get<T>();

  public static IEnumerable<CardPoolModel> AllCardPools
  {
    get
    {
      return ModelDb._allCardPools ?? (ModelDb._allCardPools = ModelDb.AllCharacterCardPools.Concat<CardPoolModel>(ModelDb.AllSharedCardPools).Distinct<CardPoolModel>());
    }
  }

  public static IEnumerable<CardPoolModel> AllSharedCardPools
  {
    get
    {
      return (IEnumerable<CardPoolModel>) new \u003C\u003Ez__ReadOnlyArray<CardPoolModel>(new CardPoolModel[7]
      {
        (CardPoolModel) ModelDb.CardPool<ColorlessCardPool>(),
        (CardPoolModel) ModelDb.CardPool<CurseCardPool>(),
        (CardPoolModel) ModelDb.CardPool<DeprecatedCardPool>(),
        (CardPoolModel) ModelDb.CardPool<EventCardPool>(),
        (CardPoolModel) ModelDb.CardPool<QuestCardPool>(),
        (CardPoolModel) ModelDb.CardPool<StatusCardPool>(),
        (CardPoolModel) ModelDb.CardPool<TokenCardPool>()
      });
    }
  }

  public static IEnumerable<CardPoolModel> AllCharacterCardPools
  {
    get
    {
      IEnumerable<CardPoolModel> characterCardPools = ModelDb._allCharacterCardPools;
      if (characterCardPools != null)
        return characterCardPools;
      IEnumerable<CharacterModel> allCharacters = ModelDb.AllCharacters;
      return ModelDb._allCharacterCardPools = allCharacters.Select<CharacterModel, CardPoolModel>((Func<CharacterModel, CardPoolModel>) (c => c.CardPool));
    }
  }

  public static T Character<T>() where T : CharacterModel => ModelDb.Get<T>();

  public static IEnumerable<CharacterModel> AllCharacters
  {
    get
    {
      return (IEnumerable<CharacterModel>) new \u003C\u003Ez__ReadOnlyArray<CharacterModel>(new CharacterModel[5]
      {
        (CharacterModel) ModelDb.Character<Ironclad>(),
        (CharacterModel) ModelDb.Character<Silent>(),
        (CharacterModel) ModelDb.Character<Regent>(),
        (CharacterModel) ModelDb.Character<Necrobinder>(),
        (CharacterModel) ModelDb.Character<Defect>()
      });
    }
  }

  public static T Event<T>() where T : EventModel => ModelDb.Get<T>();

  public static IEnumerable<EventModel> AllSharedEvents
  {
    get
    {
      IEnumerable<EventModel> allSharedEvents = ModelDb._allSharedEvents;
      if (allSharedEvents != null)
        return allSharedEvents;
      EventModel[] items = new EventModel[18]
      {
        (EventModel) ModelDb.Event<BrainLeech>(),
        (EventModel) ModelDb.Event<CrystalSphere>(),
        (EventModel) ModelDb.Event<DollRoom>(),
        (EventModel) ModelDb.Event<FakeMerchant>(),
        (EventModel) ModelDb.Event<PotionCourier>(),
        (EventModel) ModelDb.Event<RanwidTheElder>(),
        (EventModel) ModelDb.Event<RelicTrader>(),
        (EventModel) ModelDb.Event<RoomFullOfCheese>(),
        (EventModel) ModelDb.Event<SelfHelpBook>(),
        (EventModel) ModelDb.Event<SlipperyBridge>(),
        (EventModel) ModelDb.Event<StoneOfAllTime>(),
        (EventModel) ModelDb.Event<Symbiote>(),
        (EventModel) ModelDb.Event<TeaMaster>(),
        (EventModel) ModelDb.Event<TheFutureOfPotions>(),
        (EventModel) ModelDb.Event<TheLegendsWereTrue>(),
        (EventModel) ModelDb.Event<ThisOrThat>(),
        (EventModel) ModelDb.Event<WarHistorianRepy>(),
        (EventModel) ModelDb.Event<WelcomeToWongos>()
      };
      return ModelDb._allSharedEvents = (IEnumerable<EventModel>) new \u003C\u003Ez__ReadOnlyArray<EventModel>(items);
    }
  }

  public static T AncientEvent<T>() where T : AncientEventModel => ModelDb.Get<T>();

  public static IEnumerable<AncientEventModel> AllAncients
  {
    get
    {
      return ModelDb.Acts.SelectMany<ActModel, AncientEventModel>((Func<ActModel, IEnumerable<AncientEventModel>>) (a => a.AllAncients)).Concat<AncientEventModel>(ModelDb.AllSharedAncients).Distinct<AncientEventModel>();
    }
  }

  public static IEnumerable<AncientEventModel> AllSharedAncients
  {
    get
    {
      return (IEnumerable<AncientEventModel>) new \u003C\u003Ez__ReadOnlySingleElementList<AncientEventModel>((AncientEventModel) ModelDb.AncientEvent<Darv>());
    }
  }

  public static IEnumerable<EventModel> AllEvents
  {
    get
    {
      IEnumerable<EventModel> allEvents = ModelDb._allEvents;
      if (allEvents != null)
        return allEvents;
      IEnumerable<ActModel> acts = ModelDb.Acts;
      return ModelDb._allEvents = acts.SelectMany<ActModel, EventModel>((Func<ActModel, IEnumerable<EventModel>>) (a => a.AllEvents)).Concat<EventModel>(ModelDb.AllSharedEvents).Distinct<EventModel>();
    }
  }

  public static T Monster<T>() where T : MonsterModel => ModelDb.Get<T>();

  public static IEnumerable<MonsterModel> Monsters
  {
    get
    {
      return ModelDb.Acts.SelectMany<ActModel, MonsterModel>((Func<ActModel, IEnumerable<MonsterModel>>) (act => act.AllMonsters)).Concat<MonsterModel>(ModelDb.EventEncounters.SelectMany<EncounterModel, MonsterModel>((Func<EncounterModel, IEnumerable<MonsterModel>>) (e => e.AllPossibleMonsters))).Distinct<MonsterModel>();
    }
  }

  public static T Encounter<T>() where T : EncounterModel => ModelDb.Get<T>();

  public static IEnumerable<EncounterModel> AllEncounters
  {
    get
    {
      IEnumerable<EncounterModel> allEncounters = ModelDb._allEncounters;
      if (allEncounters != null)
        return allEncounters;
      IEnumerable<ActModel> acts = ModelDb.Acts;
      return ModelDb._allEncounters = acts.SelectMany<ActModel, EncounterModel>((Func<ActModel, IEnumerable<EncounterModel>>) (a => a.AllEncounters)).Concat<EncounterModel>(ModelDb.EventEncounters).Distinct<EncounterModel>();
    }
  }

  public static IEnumerable<EncounterModel> EventEncounters
  {
    get
    {
      IEnumerable<EncounterModel> eventEncounters = ModelDb._eventEncounters;
      if (eventEncounters != null)
        return eventEncounters;
      EncounterModel[] items = new EncounterModel[5]
      {
        (EncounterModel) ModelDb.Encounter<FakeMerchantEventEncounter>(),
        (EncounterModel) ModelDb.Encounter<MysteriousKnightEventEncounter>(),
        (EncounterModel) ModelDb.Encounter<BattlewornDummyEventV1Encounter>(),
        (EncounterModel) ModelDb.Encounter<BattlewornDummyEventV2Encounter>(),
        (EncounterModel) ModelDb.Encounter<BattlewornDummyEventV3Encounter>()
      };
      return ModelDb._eventEncounters = (IEnumerable<EncounterModel>) new \u003C\u003Ez__ReadOnlyArray<EncounterModel>(items);
    }
  }

  public static T Potion<T>() where T : PotionModel => ModelDb.Get<T>();

  public static IEnumerable<PotionModel> AllPotions
  {
    get
    {
      IEnumerable<PotionModel> allPotions = ModelDb._allPotions;
      if (allPotions != null)
        return allPotions;
      IEnumerable<PotionModel> source = ModelDb.AllPotionPools.SelectMany<PotionPoolModel, PotionModel>((Func<PotionPoolModel, IEnumerable<PotionModel>>) (p => p.AllPotions)).Distinct<PotionModel>();
      return ModelDb._allPotions = (IEnumerable<PotionModel>) source.OrderBy<PotionModel, string>((Func<PotionModel, string>) (p => p.Id.Entry));
    }
  }

  public static T PotionPool<T>() where T : PotionPoolModel => ModelDb.Get<T>();

  public static IEnumerable<PotionPoolModel> AllPotionPools
  {
    get
    {
      return ModelDb._allPotionPools ?? (ModelDb._allPotionPools = ModelDb.AllCharacterPotionPools.Concat<PotionPoolModel>(ModelDb.AllSharedPotionPools).Distinct<PotionPoolModel>());
    }
  }

  public static IEnumerable<PotionPoolModel> AllCharacterPotionPools
  {
    get
    {
      IEnumerable<PotionPoolModel> characterPotionPools = ModelDb._allCharacterPotionPools;
      if (characterPotionPools != null)
        return characterPotionPools;
      IEnumerable<CharacterModel> allCharacters = ModelDb.AllCharacters;
      return ModelDb._allCharacterPotionPools = allCharacters.Select<CharacterModel, PotionPoolModel>((Func<CharacterModel, PotionPoolModel>) (c => c.PotionPool));
    }
  }

  public static IEnumerable<RelicPoolModel> AllCharacterRelicPools
  {
    get
    {
      IEnumerable<RelicPoolModel> characterRelicPools = ModelDb._allCharacterRelicPools;
      if (characterRelicPools != null)
        return characterRelicPools;
      IEnumerable<CharacterModel> allCharacters = ModelDb.AllCharacters;
      return ModelDb._allCharacterRelicPools = allCharacters.Select<CharacterModel, RelicPoolModel>((Func<CharacterModel, RelicPoolModel>) (c => c.RelicPool));
    }
  }

  private static IEnumerable<PotionPoolModel> AllSharedPotionPools
  {
    get
    {
      IEnumerable<PotionPoolModel> sharedPotionPools = ModelDb._allSharedPotionPools;
      if (sharedPotionPools != null)
        return sharedPotionPools;
      PotionPoolModel[] items = new PotionPoolModel[5]
      {
        (PotionPoolModel) ModelDb.PotionPool<DeprecatedPotionPool>(),
        (PotionPoolModel) ModelDb.PotionPool<EventPotionPool>(),
        (PotionPoolModel) ModelDb.PotionPool<MockPotionPool>(),
        (PotionPoolModel) ModelDb.PotionPool<SharedPotionPool>(),
        (PotionPoolModel) ModelDb.PotionPool<TokenPotionPool>()
      };
      return ModelDb._allSharedPotionPools = (IEnumerable<PotionPoolModel>) new \u003C\u003Ez__ReadOnlyArray<PotionPoolModel>(items);
    }
  }

  public static T Power<T>() where T : PowerModel => ModelDb.Get<T>();

  public static IEnumerable<PowerModel> AllPowers
  {
    get
    {
      IEnumerable<PowerModel> allPowers = ModelDb._allPowers;
      if (allPowers != null)
        return allPowers;
      IEnumerable<Type> source = ((IEnumerable<Type>) ModelDb.AllAbstractModelSubtypes).Where<Type>((Func<Type, bool>) (t => t.IsSubclassOf(typeof (PowerModel))));
      return ModelDb._allPowers = source.Select<Type, PowerModel>((Func<Type, PowerModel>) (t => (PowerModel) ModelDb.Get(t)));
    }
  }

  public static PowerModel DebugPower(Type type) => (PowerModel) ModelDb.Get(type);

  public static T Relic<T>() where T : RelicModel => ModelDb.Get<T>();

  public static IEnumerable<RelicModel> AllRelics
  {
    get
    {
      IEnumerable<RelicModel> allRelics = ModelDb._allRelics;
      if (allRelics != null)
        return allRelics;
      IEnumerable<RelicModel> source = ModelDb.AllRelicPools.SelectMany<RelicPoolModel, RelicModel>((Func<RelicPoolModel, IEnumerable<RelicModel>>) (p => p.AllRelics)).Concat<RelicModel>(ModelDb.AllCharacters.SelectMany<CharacterModel, RelicModel>((Func<CharacterModel, IEnumerable<RelicModel>>) (c => (IEnumerable<RelicModel>) c.StartingRelics))).Distinct<RelicModel>();
      return ModelDb._allRelics = (IEnumerable<RelicModel>) source.OrderBy<RelicModel, string>((Func<RelicModel, string>) (r => r.Id.Entry));
    }
  }

  public static T RelicPool<T>() where T : RelicPoolModel => ModelDb.Get<T>();

  public static IEnumerable<RelicPoolModel> AllRelicPools
  {
    get
    {
      return ModelDb.CharacterRelicPools.Concat<RelicPoolModel>(ModelDb.AllSharedRelicPools).Distinct<RelicPoolModel>();
    }
  }

  public static IEnumerable<RelicPoolModel> CharacterRelicPools
  {
    get
    {
      return ModelDb.AllCharacters.Select<CharacterModel, RelicPoolModel>((Func<CharacterModel, RelicPoolModel>) (c => c.RelicPool));
    }
  }

  private static IEnumerable<RelicPoolModel> AllSharedRelicPools
  {
    get
    {
      return (IEnumerable<RelicPoolModel>) new \u003C\u003Ez__ReadOnlyArray<RelicPoolModel>(new RelicPoolModel[4]
      {
        (RelicPoolModel) ModelDb.RelicPool<DeprecatedRelicPool>(),
        (RelicPoolModel) ModelDb.RelicPool<EventRelicPool>(),
        (RelicPoolModel) ModelDb.RelicPool<FallbackRelicPool>(),
        (RelicPoolModel) ModelDb.RelicPool<SharedRelicPool>()
      });
    }
  }

  public static T Orb<T>() where T : OrbModel => ModelDb.Get<T>();

  public static OrbModel? DebugOrb(Type type)
  {
    try
    {
      return (OrbModel) ModelDb.Get(type);
    }
    catch
    {
      return (OrbModel) null;
    }
  }

  public static IEnumerable<OrbModel> Orbs
  {
    get
    {
      return (IEnumerable<OrbModel>) new \u003C\u003Ez__ReadOnlyArray<OrbModel>(new OrbModel[4]
      {
        (OrbModel) ModelDb.Orb<LightningOrb>(),
        (OrbModel) ModelDb.Orb<FrostOrb>(),
        (OrbModel) ModelDb.Orb<DarkOrb>(),
        (OrbModel) ModelDb.Orb<PlasmaOrb>()
      });
    }
  }

  public static T Act<T>() where T : ActModel => ModelDb.Get<T>();

  public static IEnumerable<ActModel> Acts
  {
    get
    {
      if (ModelDb._acts == null)
      {
        int capacity = 4;
        List<ActModel> actModelList = new List<ActModel>(capacity);
        CollectionsMarshal.SetCount<ActModel>(actModelList, capacity);
        Span<ActModel> span = CollectionsMarshal.AsSpan<ActModel>(actModelList);
        int num1 = 0;
        span[num1] = (ActModel) ModelDb.Act<Overgrowth>();
        int num2 = num1 + 1;
        span[num2] = (ActModel) ModelDb.Act<Underdocks>();
        int num3 = num2 + 1;
        span[num3] = (ActModel) ModelDb.Act<Hive>();
        int num4 = num3 + 1;
        span[num4] = (ActModel) ModelDb.Act<Glory>();
        ModelDb._acts = actModelList;
      }
      return (IEnumerable<ActModel>) ModelDb._acts;
    }
  }

  public static IReadOnlyList<IReadOnlyList<ActModel>> ActsByIndex
  {
    get
    {
      if (ModelDb._actsByIndex != null)
        return (IReadOnlyList<IReadOnlyList<ActModel>>) ModelDb._actsByIndex;
      ModelDb._actsByIndex = new List<List<ActModel>>();
      foreach (ActModel act in ModelDb.Acts)
      {
        if (act.Index >= 0)
        {
          for (int count = ModelDb._actsByIndex.Count; count <= act.Index; ++count)
            ModelDb._actsByIndex.Add(new List<ActModel>());
          ModelDb._actsByIndex[act.Index].Add(act);
        }
      }
      return (IReadOnlyList<IReadOnlyList<ActModel>>) ModelDb._actsByIndex;
    }
  }

  public static T Singleton<T>() where T : SingletonModel => ModelDb.Get<T>();

  public static T Badge<T>() where T : BadgeModel => ModelDb.Get<T>();

  public static IReadOnlyList<BadgeModel> BadgeModels
  {
    get
    {
      if (ModelDb._badges == null)
      {
        ModelDb._badges = new List<BadgeModel>();
        foreach (Type abstractModelSubtype in ModelDb.AllAbstractModelSubtypes)
        {
          if (abstractModelSubtype.IsSubclassOf(typeof (BadgeModel)))
            ModelDb._badges.Add((BadgeModel) ModelDb.Get(abstractModelSubtype));
        }
      }
      return (IReadOnlyList<BadgeModel>) ModelDb._badges;
    }
  }

  public static T Achievement<T>() where T : AchievementModel => ModelDb.Get<T>();

  public static IReadOnlyList<AchievementModel> Achievements
  {
    get
    {
      if (ModelDb._achievements == null)
      {
        ModelDb._achievements = new List<AchievementModel>();
        foreach (Type abstractModelSubtype in ModelDb.AllAbstractModelSubtypes)
        {
          if (abstractModelSubtype.IsSubclassOf(typeof (AchievementModel)))
            ModelDb._achievements.Add((AchievementModel) ModelDb.Get(abstractModelSubtype));
        }
      }
      return (IReadOnlyList<AchievementModel>) ModelDb._achievements;
    }
  }

  public static T Modifier<T>() where T : ModifierModel => ModelDb.Get<T>();

  public static IReadOnlyList<ModifierModel> GoodModifiers
  {
    get
    {
      return (IReadOnlyList<ModifierModel>) new \u003C\u003Ez__ReadOnlyArray<ModifierModel>(new ModifierModel[9]
      {
        (ModifierModel) ModelDb.Modifier<Draft>(),
        (ModifierModel) ModelDb.Modifier<SealedDeck>(),
        (ModifierModel) ModelDb.Modifier<Hoarder>(),
        (ModifierModel) ModelDb.Modifier<Specialized>(),
        (ModifierModel) ModelDb.Modifier<Insanity>(),
        (ModifierModel) ModelDb.Modifier<AllStar>(),
        (ModifierModel) ModelDb.Modifier<Flight>(),
        (ModifierModel) ModelDb.Modifier<Vintage>(),
        (ModifierModel) ModelDb.Modifier<CharacterCards>()
      });
    }
  }

  public static IReadOnlyList<ModifierModel> BadModifiers
  {
    get
    {
      return (IReadOnlyList<ModifierModel>) new \u003C\u003Ez__ReadOnlyArray<ModifierModel>(new ModifierModel[7]
      {
        (ModifierModel) ModelDb.Modifier<DeadlyEvents>(),
        (ModifierModel) ModelDb.Modifier<CursedRun>(),
        (ModifierModel) ModelDb.Modifier<BigGameHunter>(),
        (ModifierModel) ModelDb.Modifier<Midas>(),
        (ModifierModel) ModelDb.Modifier<Murderous>(),
        (ModifierModel) ModelDb.Modifier<NightTerrors>(),
        (ModifierModel) ModelDb.Modifier<Terminal>()
      });
    }
  }

  public static IReadOnlyList<IReadOnlySet<ModifierModel>> MutuallyExclusiveModifiers
  {
    get
    {
      return (IReadOnlyList<IReadOnlySet<ModifierModel>>) new \u003C\u003Ez__ReadOnlySingleElementList<IReadOnlySet<ModifierModel>>((IReadOnlySet<ModifierModel>) new HashSet<ModifierModel>()
      {
        (ModifierModel) ModelDb.Modifier<SealedDeck>(),
        (ModifierModel) ModelDb.Modifier<Draft>(),
        (ModifierModel) ModelDb.Modifier<Insanity>()
      });
    }
  }
}
