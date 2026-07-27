// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Timeline.EpochModel
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Potions;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Screens.Timeline;
using MegaCrit.Sts2.Core.Saves;
using MegaCrit.Sts2.Core.Timeline.Epochs;
using MegaCrit.Sts2.SourceGeneration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

#nullable enable
namespace MegaCrit.Sts2.Core.Timeline;

[GenerateSubtypes]
public abstract class EpochModel
{
  private static readonly List<Type> _allEpochs;
  private static List<string>? _allEpochIds;
  private static HashSet<string>? _epochIdsHashSet;
  private string? _resolvedPortraitPath;
  private static readonly Dictionary<string, Type> _epochTypeDictionary;
  private static readonly Dictionary<Type, string> _typeToIdDictionary;

  public static IReadOnlyList<Type> AllEpochs => (IReadOnlyList<Type>) EpochModel._allEpochs;

  public static IReadOnlyList<string> AllEpochIds
  {
    get
    {
      List<string> allEpochIds = EpochModel._allEpochIds;
      if (allEpochIds != null)
        return (IReadOnlyList<string>) allEpochIds;
      List<Type> allEpochs = EpochModel._allEpochs;
      Func<Type, string> selector = EpochModel.\u003C\u003EO.\u003C0\u003E__GetId ?? (EpochModel.\u003C\u003EO.\u003C0\u003E__GetId = new Func<Type, string>(EpochModel.GetId));
      return (IReadOnlyList<string>) (EpochModel._allEpochIds = allEpochs.Select<Type, string>(selector).ToList<string>());
    }
  }

  public static IReadOnlySet<string> EpochIdsHashSet
  {
    get
    {
      return (IReadOnlySet<string>) EpochModel._epochIdsHashSet ?? (IReadOnlySet<string>) (EpochModel._epochIdsHashSet = EpochModel.AllEpochIds.ToHashSet<string>());
    }
  }

  public abstract string Id { get; }

  public LocString Title => new LocString("epochs", this.Id + ".title");

  public string Description => new LocString("epochs", this.Id + ".description").GetFormattedText();

  public string? StoryTitle
  {
    get
    {
      return this.StoryId == null ? (string) null : new LocString("epochs", "STORY_" + this.StoryId.ToUpperInvariant()).GetRawText();
    }
  }

  public virtual string? StoryId => (string) null;

  public LocString UnlockInfo => new LocString("epochs", this.Id + ".unlockInfo");

  public virtual string UnlockText
  {
    get => new LocString("epochs", this.Id + ".unlockText").GetFormattedText();
  }

  public abstract EpochEra Era { get; }

  public abstract int EraPosition { get; }

  public Texture2D Portrait
  {
    get
    {
      return ResourceLoader.Load<Texture2D>(this.PackedPortraitPath, (string) null, (ResourceLoader.CacheMode) 1L);
    }
  }

  private string PackedPortraitPath
  {
    get
    {
      return ImageHelper.GetImagePath($"atlases/epoch_atlas.sprites/{this.Id.ToLowerInvariant()}.tres");
    }
  }

  public Texture2D RealPortrait
  {
    get
    {
      return ResourceLoader.Load<Texture2D>(this.ResolvedPortraitPath, (string) null, (ResourceLoader.CacheMode) 1L);
    }
  }

  public bool HasRealPortrait => ResourceLoader.Exists(this.RealPortraitPath, "");

  private string RealPortraitPath
  {
    get => ImageHelper.GetImagePath($"timeline/epoch_portraits/{this.Id.ToLowerInvariant()}.png");
  }

  private string PlaceholderPortraitPath
  {
    get
    {
      return ImageHelper.GetImagePath($"timeline/epoch_portraits/placeholder/{this.Id.ToLowerInvariant()}.png");
    }
  }

  public string ResolvedPortraitPath
  {
    get
    {
      if (this._resolvedPortraitPath != null)
        return this._resolvedPortraitPath;
      this._resolvedPortraitPath = !ResourceLoader.Exists(this.RealPortraitPath, "") ? this.PlaceholderPortraitPath : this.RealPortraitPath;
      return this._resolvedPortraitPath;
    }
  }

  public int ChapterIndex
  {
    get
    {
      if (this.StoryId == null)
        return -1;
      EpochModel[] epochs = StoryModel.Get(StringHelper.Slugify(this.StoryId)).Epochs;
      for (int index = 0; index < epochs.Length; ++index)
      {
        if (epochs[index].Id == this.Id)
          return index + 1;
      }
      return -1;
    }
  }

  static EpochModel()
  {
    int capacity = 57;
    List<Type> typeList = new List<Type>(capacity);
    CollectionsMarshal.SetCount<Type>(typeList, capacity);
    Span<Type> span = CollectionsMarshal.AsSpan<Type>(typeList);
    int num1 = 0;
    span[num1] = typeof (Act2BEpoch);
    int num2 = num1 + 1;
    span[num2] = typeof (Act3BEpoch);
    int num3 = num2 + 1;
    span[num3] = typeof (Colorless1Epoch);
    int num4 = num3 + 1;
    span[num4] = typeof (Colorless2Epoch);
    int num5 = num4 + 1;
    span[num5] = typeof (Colorless3Epoch);
    int num6 = num5 + 1;
    span[num6] = typeof (Colorless4Epoch);
    int num7 = num6 + 1;
    span[num7] = typeof (Colorless5Epoch);
    int num8 = num7 + 1;
    span[num8] = typeof (CustomAndSeedsEpoch);
    int num9 = num8 + 1;
    span[num9] = typeof (DailyRunEpoch);
    int num10 = num9 + 1;
    span[num10] = typeof (DarvEpoch);
    int num11 = num10 + 1;
    span[num11] = typeof (Defect1Epoch);
    int num12 = num11 + 1;
    span[num12] = typeof (Defect2Epoch);
    int num13 = num12 + 1;
    span[num13] = typeof (Defect3Epoch);
    int num14 = num13 + 1;
    span[num14] = typeof (Defect4Epoch);
    int num15 = num14 + 1;
    span[num15] = typeof (Defect5Epoch);
    int num16 = num15 + 1;
    span[num16] = typeof (Defect6Epoch);
    int num17 = num16 + 1;
    span[num17] = typeof (Defect7Epoch);
    int num18 = num17 + 1;
    span[num18] = typeof (Event1Epoch);
    int num19 = num18 + 1;
    span[num19] = typeof (Event2Epoch);
    int num20 = num19 + 1;
    span[num20] = typeof (Event3Epoch);
    int num21 = num20 + 1;
    span[num21] = typeof (Ironclad2Epoch);
    int num22 = num21 + 1;
    span[num22] = typeof (Ironclad3Epoch);
    int num23 = num22 + 1;
    span[num23] = typeof (Ironclad4Epoch);
    int num24 = num23 + 1;
    span[num24] = typeof (Ironclad5Epoch);
    int num25 = num24 + 1;
    span[num25] = typeof (Ironclad6Epoch);
    int num26 = num25 + 1;
    span[num26] = typeof (Ironclad7Epoch);
    int num27 = num26 + 1;
    span[num27] = typeof (Necrobinder1Epoch);
    int num28 = num27 + 1;
    span[num28] = typeof (Necrobinder2Epoch);
    int num29 = num28 + 1;
    span[num29] = typeof (Necrobinder3Epoch);
    int num30 = num29 + 1;
    span[num30] = typeof (Necrobinder4Epoch);
    int num31 = num30 + 1;
    span[num31] = typeof (Necrobinder5Epoch);
    int num32 = num31 + 1;
    span[num32] = typeof (Necrobinder6Epoch);
    int num33 = num32 + 1;
    span[num33] = typeof (Necrobinder7Epoch);
    int num34 = num33 + 1;
    span[num34] = typeof (NeowEpoch);
    int num35 = num34 + 1;
    span[num35] = typeof (OrobasEpoch);
    int num36 = num35 + 1;
    span[num36] = typeof (Potion1Epoch);
    int num37 = num36 + 1;
    span[num37] = typeof (Potion2Epoch);
    int num38 = num37 + 1;
    span[num38] = typeof (Regent1Epoch);
    int num39 = num38 + 1;
    span[num39] = typeof (Regent2Epoch);
    int num40 = num39 + 1;
    span[num40] = typeof (Regent3Epoch);
    int num41 = num40 + 1;
    span[num41] = typeof (Regent4Epoch);
    int num42 = num41 + 1;
    span[num42] = typeof (Regent5Epoch);
    int num43 = num42 + 1;
    span[num43] = typeof (Regent6Epoch);
    int num44 = num43 + 1;
    span[num44] = typeof (Regent7Epoch);
    int num45 = num44 + 1;
    span[num45] = typeof (Relic1Epoch);
    int num46 = num45 + 1;
    span[num46] = typeof (Relic2Epoch);
    int num47 = num46 + 1;
    span[num47] = typeof (Relic3Epoch);
    int num48 = num47 + 1;
    span[num48] = typeof (Relic4Epoch);
    int num49 = num48 + 1;
    span[num49] = typeof (Relic5Epoch);
    int num50 = num49 + 1;
    span[num50] = typeof (Silent1Epoch);
    int num51 = num50 + 1;
    span[num51] = typeof (Silent2Epoch);
    int num52 = num51 + 1;
    span[num52] = typeof (Silent3Epoch);
    int num53 = num52 + 1;
    span[num53] = typeof (Silent4Epoch);
    int num54 = num53 + 1;
    span[num54] = typeof (Silent5Epoch);
    int num55 = num54 + 1;
    span[num55] = typeof (Silent6Epoch);
    int num56 = num55 + 1;
    span[num56] = typeof (Silent7Epoch);
    int num57 = num56 + 1;
    span[num57] = typeof (UnderdocksEpoch);
    EpochModel._allEpochs = typeList;
    EpochModel._epochTypeDictionary = new Dictionary<string, Type>();
    EpochModel._typeToIdDictionary = new Dictionary<Type, string>();
    for (int i = 0; i < EpochModelSubtypes.Count; ++i)
    {
      Type type = EpochModelSubtypes.Get(i);
      EpochModel instance = (EpochModel) Activator.CreateInstance(type);
      EpochModel._epochTypeDictionary[instance.Id] = type;
      EpochModel._typeToIdDictionary[type] = instance.Id;
    }
  }

  public virtual EpochModel[] GetTimelineExpansion() => Array.Empty<EpochModel>();

  public virtual void QueueUnlocks()
  {
  }

  public static string GetId<T>() where T : EpochModel
  {
    return EpochModel._typeToIdDictionary[typeof (T)];
  }

  public static string GetId(Type t) => EpochModel._typeToIdDictionary[t];

  public static bool IsValid(string id) => EpochModel.EpochIdsHashSet.Contains(id);

  public static EpochModel Get(string id)
  {
    Type type;
    if (EpochModel._epochTypeDictionary.TryGetValue(id, out type))
      return (EpochModel) Activator.CreateInstance(type);
    throw new ArgumentException($"Epoch with id '{id}' does not exist.");
  }

  public static EpochModel Get<T>() where T : EpochModel => EpochModel.Get(EpochModel.GetId<T>());

  protected static void QueueTimelineExpansion(EpochModel[] epochs)
  {
    Log.Info("Queueing a Timeline expansion...");
    List<EpochSlotData> eraData = new List<EpochSlotData>();
    foreach (EpochModel epoch1 in epochs)
    {
      EpochModel epoch = epoch1;
      SerializableEpoch serializableEpoch = SaveManager.Instance.Progress.Epochs.FirstOrDefault<SerializableEpoch>((Func<SerializableEpoch, bool>) (e => e.Id == epoch.Id));
      if (serializableEpoch != null && serializableEpoch.State == EpochState.ObtainedNoSlot)
      {
        Log.Info("We have it already Yay: " + serializableEpoch.Id);
        eraData.Add(new EpochSlotData(epoch, EpochSlotState.Obtained));
      }
      else
        eraData.Add(new EpochSlotData(epoch, EpochSlotState.NotObtained));
    }
    NTimelineScreen.Instance.QueueTimelineExpansion(eraData);
    foreach (EpochModel epoch in epochs)
      SaveManager.Instance.UnlockSlot(epoch.Id);
  }

  protected string CreateCardUnlockText(List<CardModel> cards)
  {
    LocString locString = new LocString("timeline", "UNLOCK_TEXT.cards");
    cards = cards.OrderBy<CardModel, CardRarity>((Func<CardModel, CardRarity>) (c => c.Rarity)).ToList<CardModel>();
    for (int index = 0; index < 3; ++index)
      locString.Add($"Card{index + 1}", this.GetColoredCardName(cards[index]));
    return locString.GetFormattedText();
  }

  private string GetColoredCardName(CardModel card)
  {
    if (card.Rarity == CardRarity.Common)
      return card.TitleLocString.GetRawText();
    if (card.Rarity == CardRarity.Uncommon)
      return $"[blue]{card.TitleLocString.GetRawText()}[/blue]";
    return card.Rarity == CardRarity.Rare ? $"[gold]{card.TitleLocString.GetRawText()}[/gold]" : "ERROR";
  }

  protected string CreateRelicUnlockText(List<RelicModel> relics)
  {
    LocString locString = new LocString("timeline", "UNLOCK_TEXT.relics");
    relics = relics.OrderBy<RelicModel, RelicRarity>((Func<RelicModel, RelicRarity>) (r => r.Rarity)).ToList<RelicModel>();
    for (int index = 0; index < 3; ++index)
      locString.Add($"Relic{index + 1}", this.GetColoredRelicName(relics[index]));
    return locString.GetFormattedText();
  }

  private string GetColoredRelicName(RelicModel relic)
  {
    if (relic.Rarity == RelicRarity.Common)
      return relic.Title.GetRawText();
    if (relic.Rarity == RelicRarity.Uncommon)
      return $"[blue]{relic.Title.GetRawText()}[/blue]";
    return relic.Rarity == RelicRarity.Rare ? $"[gold]{relic.Title.GetRawText()}[/gold]" : "ERROR";
  }

  protected string CreatePotionUnlockText(List<PotionModel> potions)
  {
    LocString locString = new LocString("timeline", "UNLOCK_TEXT.potions");
    potions = potions.OrderBy<PotionModel, PotionRarity>((Func<PotionModel, PotionRarity>) (r => r.Rarity)).ToList<PotionModel>();
    for (int index = 0; index < 3; ++index)
      locString.Add($"Potion{index + 1}", this.GetColoredPotionName(potions[index]));
    return locString.GetFormattedText();
  }

  private string GetColoredPotionName(PotionModel potion)
  {
    if (potion.Rarity == PotionRarity.Common)
      return potion.Title.GetRawText();
    if (potion.Rarity == PotionRarity.Uncommon)
      return $"[blue]{potion.Title.GetRawText()}[/blue]";
    return potion.Rarity == PotionRarity.Rare ? $"[gold]{potion.Title.GetRawText()}[/gold]" : "ERROR";
  }
}
