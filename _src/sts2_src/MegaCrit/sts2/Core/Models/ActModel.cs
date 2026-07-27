// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.ActModel
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using MegaCrit.Sts2.Core.Achievements;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Bindings.MegaSpine;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Hooks;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Map;
using MegaCrit.Sts2.Core.Models.Encounters;
using MegaCrit.Sts2.Core.Random;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.Saves;
using MegaCrit.Sts2.Core.Saves.Runs;
using MegaCrit.Sts2.Core.TestSupport;
using MegaCrit.Sts2.Core.Timeline.Epochs;
using MegaCrit.Sts2.Core.Unlocks;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable enable
namespace MegaCrit.Sts2.Core.Models;

public abstract class ActModel : AbstractModel
{
  protected RoomSet _rooms;
  private IEnumerable<EncounterModel>? _allEncounters;
  private IEnumerable<EncounterModel>? _allWeakEncounters;
  private IEnumerable<EncounterModel>? _allRegularEncounters;
  private IEnumerable<EncounterModel>? _allEliteEncounters;
  private IEnumerable<EncounterModel>? _allBossEncounters;
  private IEnumerable<MonsterModel>? _allMonsters;
  private List<AncientEventModel>? _sharedAncientSubset;
  private ActModel _canonicalInstance;

  public LocString Title => new LocString("acts", this.Id.Entry + ".title");

  protected string FilePathIdentifier => this.Id.Entry.ToLowerInvariant();

  public string RestSiteBackgroundPath
  {
    get => SceneHelper.GetScenePath($"rest_site/{this.FilePathIdentifier}_rest_site");
  }

  public Control CreateRestSiteBackground()
  {
    return PreloadManager.Cache.GetScene(this.RestSiteBackgroundPath).Instantiate<Control>((PackedScene.GenEditState) 0L);
  }

  public string MapTopBgPath
  {
    get
    {
      return ImageHelper.GetImagePath($"packed/map/map_bgs/{this.FilePathIdentifier}/map_top_{this.FilePathIdentifier}.png");
    }
  }

  public Texture2D MapTopBg
  {
    get => (Texture2D) PreloadManager.Cache.GetCompressedTexture2D(this.MapTopBgPath);
  }

  public string MapMidBgPath
  {
    get
    {
      return ImageHelper.GetImagePath($"packed/map/map_bgs/{this.FilePathIdentifier}/map_middle_{this.FilePathIdentifier}.png");
    }
  }

  public Texture2D MapMidBg
  {
    get => (Texture2D) PreloadManager.Cache.GetCompressedTexture2D(this.MapMidBgPath);
  }

  public string MapBotBgPath
  {
    get
    {
      return ImageHelper.GetImagePath($"packed/map/map_bgs/{this.FilePathIdentifier}/map_bottom_{this.FilePathIdentifier}.png");
    }
  }

  public Texture2D MapBotBg
  {
    get => (Texture2D) PreloadManager.Cache.GetCompressedTexture2D(this.MapBotBgPath);
  }

  public abstract int Index { get; }

  public abstract bool IsDefault { get; }

  public abstract Color MapTraveledColor { get; }

  public abstract Color MapUntraveledColor { get; }

  public abstract Color MapBgColor { get; }

  public IEnumerable<string> AssetPaths
  {
    get
    {
      List<string> items = new List<string>();
      items.Add(this.BackgroundScenePath);
      items.Add(this.MapBotBgPath);
      items.Add(this.MapMidBgPath);
      items.Add(this.MapTopBgPath);
      items.AddRange(this._rooms.HasAncient ? this._rooms.Ancient.MapNodeAssetPaths : (IEnumerable<string>) Array.Empty<string>());
      items.AddRange(this._rooms.Boss.MapNodeAssetPaths);
      items.AddRange(this._rooms.HasSecondBoss ? this._rooms.SecondBoss.MapNodeAssetPaths : (IEnumerable<string>) Array.Empty<string>());
      return (IEnumerable<string>) new \u003C\u003Ez__ReadOnlyList<string>(items);
    }
  }

  public abstract string[] BgMusicOptions { get; }

  public abstract string[] MusicBankPaths { get; }

  public abstract string AmbientSfx { get; }

  protected virtual int NumberOfWeakEncounters => 3;

  protected abstract int BaseNumberOfRooms { get; }

  public int GetNumberOfRooms(bool isMultiplayer)
  {
    int baseNumberOfRooms = this.BaseNumberOfRooms;
    if (isMultiplayer)
      --baseNumberOfRooms;
    return baseNumberOfRooms;
  }

  public int GetNumberOfFloors(bool isMultiplayer) => this.GetNumberOfRooms(isMultiplayer) + 2;

  public IEnumerable<EncounterModel> AllEncounters
  {
    get => this._allEncounters ?? (this._allEncounters = this.GenerateAllEncounters());
  }

  public abstract IEnumerable<EncounterModel> GenerateAllEncounters();

  public abstract bool IsUnlocked(UnlockState unlockState);

  public IEnumerable<EncounterModel> AllWeakEncounters
  {
    get
    {
      return this._allWeakEncounters ?? (this._allWeakEncounters = this.AllEncounters.Where<EncounterModel>((Func<EncounterModel, bool>) (e => e != null && e.RoomType == RoomType.Monster && e.IsWeak)));
    }
  }

  public IEnumerable<EncounterModel> AllRegularEncounters
  {
    get
    {
      return this._allRegularEncounters ?? (this._allRegularEncounters = this.AllEncounters.Where<EncounterModel>((Func<EncounterModel, bool>) (e => e != null && e.RoomType == RoomType.Monster && !e.IsWeak)));
    }
  }

  public IEnumerable<EncounterModel> AllEliteEncounters
  {
    get
    {
      return this._allEliteEncounters ?? (this._allEliteEncounters = this.AllEncounters.Where<EncounterModel>((Func<EncounterModel, bool>) (e => e.RoomType == RoomType.Elite)));
    }
  }

  public IEnumerable<EncounterModel> AllBossEncounters
  {
    get
    {
      return this._allBossEncounters ?? (this._allBossEncounters = this.AllEncounters.Where<EncounterModel>((Func<EncounterModel, bool>) (e => e.RoomType == RoomType.Boss)));
    }
  }

  public IEnumerable<MonsterModel> AllMonsters
  {
    get
    {
      return this._allMonsters ?? (this._allMonsters = this.AllEncounters.SelectMany<EncounterModel, MonsterModel>((Func<EncounterModel, IEnumerable<MonsterModel>>) (e => e.AllPossibleMonsters)).Distinct<MonsterModel>());
    }
  }

  public Achievement DefeatedAllEnemiesAchievement
  {
    get => Enum.Parse<Achievement>($"Defeat{StringExtensions.Capitalize(this.Id.Entry)}Enemies");
  }

  public virtual string ChestSpineResourcePath
  {
    get
    {
      return $"res://animations/backgrounds/treasure_room/chest_room_act_{this.FilePathIdentifier}_skel_data.tres";
    }
  }

  public abstract string ChestSpineSkinNameNormal { get; }

  public abstract string ChestSpineSkinNameStroke { get; }

  public virtual MegaSkeletonDataResource ChestSpineResource
  {
    get
    {
      return new MegaSkeletonDataResource(Variant.op_Implicit((GodotObject) PreloadManager.Cache.GetAsset<Resource>(this.ChestSpineResourcePath)));
    }
  }

  public abstract string ChestOpenSfx { get; }

  public abstract IEnumerable<EncounterModel> BossDiscoveryOrder { get; }

  public abstract IEnumerable<AncientEventModel> AllAncients { get; }

  public abstract IEnumerable<EventModel> AllEvents { get; }

  public ActModel CanonicalInstance
  {
    get => !this.IsMutable ? this : this._canonicalInstance;
    private set
    {
      this.AssertMutable();
      this._canonicalInstance = value;
    }
  }

  protected override void DeepCloneFields() => this._rooms = new RoomSet();

  public override bool ShouldReceiveCombatHooks => false;

  public abstract IEnumerable<AncientEventModel> GetUnlockedAncients(UnlockState state);

  protected string GetFullLayerPath(string layerName)
  {
    return $"res://scenes/backgrounds/{this.FilePathIdentifier}/layers/{this.FilePathIdentifier}_{layerName}.tscn";
  }

  public void SetSharedAncientSubset(List<AncientEventModel> sharedAncientSubset)
  {
    this.AssertMutable();
    this._sharedAncientSubset = new List<AncientEventModel>();
    this._sharedAncientSubset.AddRange((IEnumerable<AncientEventModel>) sharedAncientSubset);
  }

  public IEnumerable<string> GetAllBackgroundLayerPaths()
  {
    string backgroundsPath = $"res://scenes/backgrounds/{this.FilePathIdentifier}/layers";
    using (DirAccess dirAccess = DirAccess.Open(backgroundsPath))
      return dirAccess == null ? (IEnumerable<string>) Array.Empty<string>() : (IEnumerable<string>) ((IEnumerable<string>) dirAccess.GetFiles()).Where<string>((Func<string, bool>) (path => path.EndsWith(".tscn"))).Select<string, string>((Func<string, string>) (path => $"{backgroundsPath}/{path}")).ToArray<string>();
  }

  public void GenerateRooms(Rng rng, UnlockState unlockState, bool isMultiplayer = false)
  {
    this.AssertMutable();
    List<EventModel> list = this.AllEvents.Concat<EventModel>(ModelDb.AllSharedEvents).ToList<EventModel>();
    if (!unlockState.IsEpochRevealed<Event1Epoch>())
      list.RemoveAll((Predicate<EventModel>) (e => Event1Epoch.Events.Any<EventModel>((Func<EventModel, bool>) (ev => ev.Id == e.Id))));
    if (!unlockState.IsEpochRevealed<Event2Epoch>())
      list.RemoveAll((Predicate<EventModel>) (e => Event2Epoch.Events.Any<EventModel>((Func<EventModel, bool>) (ev => ev.Id == e.Id))));
    if (!unlockState.IsEpochRevealed<Event3Epoch>())
      list.RemoveAll((Predicate<EventModel>) (e => Event3Epoch.Events.Any<EventModel>((Func<EventModel, bool>) (ev => ev.Id == e.Id))));
    this._rooms.events.AddRange((IEnumerable<EventModel>) list.UnstableShuffle<EventModel>(rng));
    GrabBag<EncounterModel> grabBag1 = new GrabBag<EncounterModel>();
    for (int index = 0; index < this.NumberOfWeakEncounters; ++index)
    {
      if (!grabBag1.Any())
      {
        foreach (EncounterModel allWeakEncounter in this.AllWeakEncounters)
          grabBag1.Add(allWeakEncounter, 1.0);
      }
      ActModel.AddWithoutRepeatingTags((ICollection<EncounterModel>) this._rooms.normalEncounters, grabBag1, rng);
    }
    GrabBag<EncounterModel> grabBag2 = new GrabBag<EncounterModel>();
    for (int ofWeakEncounters = this.NumberOfWeakEncounters; ofWeakEncounters < this.GetNumberOfRooms(isMultiplayer); ++ofWeakEncounters)
    {
      if (!grabBag2.Any())
      {
        foreach (EncounterModel regularEncounter in this.AllRegularEncounters)
          grabBag2.Add(regularEncounter, 1.0);
      }
      ActModel.AddWithoutRepeatingTags((ICollection<EncounterModel>) this._rooms.normalEncounters, grabBag2, rng);
    }
    GrabBag<EncounterModel> grabBag3 = new GrabBag<EncounterModel>();
    for (int index = 0; index < 15; ++index)
    {
      if (!grabBag3.Any())
      {
        foreach (EncounterModel allEliteEncounter in this.AllEliteEncounters)
          grabBag3.Add(allEliteEncounter, 1.0);
      }
      ActModel.AddWithoutRepeatingTags((ICollection<EncounterModel>) this._rooms.eliteEncounters, grabBag3, rng);
    }
    this._rooms.Boss = rng.NextItem<EncounterModel>(this.AllBossEncounters);
    this._rooms.Ancient = rng.NextItem<AncientEventModel>(this.GetUnlockedAncients(unlockState).Concat<AncientEventModel>((IEnumerable<AncientEventModel>) (this._sharedAncientSubset ?? new List<AncientEventModel>())));
  }

  public void ValidateRoomsAfterLoad(Rng rng)
  {
    if (this._rooms.Boss is DeprecatedEncounter)
      this._rooms.Boss = rng.NextItem<EncounterModel>(this.AllBossEncounters.Where<EncounterModel>((Func<EncounterModel, bool>) (e => e.Id != this._rooms.SecondBoss?.Id)));
    if (!(this._rooms.SecondBoss is DeprecatedEncounter))
      return;
    this._rooms.SecondBoss = rng.NextItem<EncounterModel>(this.AllBossEncounters.Where<EncounterModel>((Func<EncounterModel, bool>) (e => e.Id != this._rooms.Boss.Id)));
  }

  public void ApplyDiscoveryOrderModifications(UnlockState unlockState)
  {
    foreach (EncounterModel encounter in this.BossDiscoveryOrder)
    {
      if (!unlockState.HasSeenEncounter(encounter))
      {
        this._rooms.Boss = encounter;
        break;
      }
    }
    this.ApplyActDiscoveryOrderModifications(unlockState);
  }

  protected abstract void ApplyActDiscoveryOrderModifications(UnlockState unlockState);

  private static void AddWithoutRepeatingTags(
    ICollection<EncounterModel> encounters,
    GrabBag<EncounterModel> grabBag,
    Rng rng)
  {
    EncounterModel encounterModel = grabBag.GrabAndRemove(rng, (Func<EncounterModel, bool>) (e => !e.SharesTagsWith(encounters.LastOrDefault<EncounterModel>()) && e != encounters.LastOrDefault<EncounterModel>())) ?? grabBag.GrabAndRemove(rng);
    if (encounterModel == null)
      return;
    encounters.Add(encounterModel);
  }

  public EventModel PullAncient() => (EventModel) this._rooms.Ancient;

  public EventModel PullNextEvent(RunState runState)
  {
    this._rooms.EnsureNextEventIsValid(runState);
    EventModel eventModel = Hook.ModifyNextEvent((IRunState) runState, this._rooms.NextEvent);
    runState.AddVisitedEvent(eventModel);
    return eventModel;
  }

  public EncounterModel PullNextEncounter(RoomType roomType)
  {
    switch (roomType)
    {
      case RoomType.Monster:
        return this._rooms.NextNormalEncounter;
      case RoomType.Elite:
        return this._rooms.NextEliteEncounter;
      case RoomType.Boss:
        return this._rooms.NextBossEncounter;
      default:
        throw new ArgumentOutOfRangeException(nameof (roomType), (object) roomType, (string) null);
    }
  }

  public void MarkRoomVisited(RoomType roomType) => this._rooms.MarkVisited(roomType);

  public EncounterModel BossEncounter => this._rooms.Boss;

  public EncounterModel? SecondBossEncounter => this._rooms.SecondBoss;

  public bool HasSecondBoss => this._rooms.HasSecondBoss;

  public AncientEventModel Ancient => this._rooms.Ancient;

  public string BackgroundScenePath
  {
    get
    {
      return SceneHelper.GetScenePath($"backgrounds/{this.FilePathIdentifier}/{this.FilePathIdentifier}_background");
    }
  }

  public BackgroundAssets GenerateBackgroundAssets(Rng rng)
  {
    return new BackgroundAssets(this.FilePathIdentifier, rng);
  }

  public void SetBossEncounter(EncounterModel encounter)
  {
    this.AssertMutable();
    this._rooms.Boss = encounter.RoomType == RoomType.Boss ? encounter : throw new ArgumentException("The encounter must be a boss.");
  }

  public void SetSecondBossEncounter(EncounterModel? encounter)
  {
    this.AssertMutable();
    this._rooms.SecondBoss = encounter == null || encounter.RoomType == RoomType.Boss ? encounter : throw new ArgumentException("The encounter must be a boss.");
  }

  public void RemoveEventFromSet(EventModel eventModel)
  {
    eventModel.AssertCanonical();
    this._rooms.events.Remove(eventModel);
  }

  public ActModel ToMutable()
  {
    this.AssertCanonical();
    ActModel mutable = (ActModel) this.MutableClone();
    mutable.CanonicalInstance = this;
    return mutable;
  }

  public SerializableActModel ToSave()
  {
    this.AssertMutable();
    return new SerializableActModel()
    {
      Id = this.Id,
      SerializableRooms = this._rooms.ToSave()
    };
  }

  public static ActModel FromSave(SerializableActModel save)
  {
    ActModel mutable = ModelDb.GetById<ActModel>(save.Id).ToMutable();
    mutable._rooms = RoomSet.FromSave(save.SerializableRooms);
    return mutable;
  }

  public abstract MapPointTypeCounts GetMapPointTypes(Rng mapRng);

  public ActMap CreateMap(RunState runState, bool replaceTreasureWithElites)
  {
    return (ActMap) StandardActMap.CreateFor(runState, replaceTreasureWithElites);
  }

  public static IEnumerable<ActModel> GetRandomList(
    Rng rng,
    UnlockState unlockState,
    bool isMultiplayer)
  {
    IReadOnlyList<IReadOnlyList<ActModel>> actsByIndex = ModelDb.ActsByIndex;
    List<ActModel> randomList = new List<ActModel>();
    for (int index = 0; index < actsByIndex.Count; ++index)
    {
      ActModel actModel1 = (ActModel) null;
      List<ActModel> items = new List<ActModel>();
      foreach (ActModel actModel2 in (IEnumerable<ActModel>) actsByIndex[index])
      {
        if (actModel2.IsUnlocked(unlockState))
        {
          if (!actModel2.IsDefault && !isMultiplayer && !SaveManager.Instance.Progress.DiscoveredActs.Contains(actModel2.Id) && TestMode.IsOff)
          {
            actModel1 = actModel2;
            break;
          }
          items.Add(actModel2);
        }
      }
      if (actModel1 == null)
        actModel1 = rng.NextItem<ActModel>((IEnumerable<ActModel>) items) ?? throw new InvalidOperationException($"No unlocked acts for index {index}!");
      randomList.Add(actModel1);
    }
    return (IEnumerable<ActModel>) randomList;
  }

  public static IReadOnlyList<ActModel> GetDefaultList()
  {
    IReadOnlyList<IReadOnlyList<ActModel>> actsByIndex = ModelDb.ActsByIndex;
    List<ActModel> defaultList = new List<ActModel>();
    for (int index = 0; index < actsByIndex.Count; ++index)
    {
      ActModel actModel1 = (ActModel) null;
      foreach (ActModel actModel2 in (IEnumerable<ActModel>) actsByIndex[index])
      {
        if (actModel2.IsDefault)
        {
          actModel1 = actModel2;
          break;
        }
      }
      if (actModel1 == null)
        throw new InvalidOperationException($"No default act for index {index}!");
      defaultList.Add(actModel1);
    }
    return (IReadOnlyList<ActModel>) defaultList;
  }
}
