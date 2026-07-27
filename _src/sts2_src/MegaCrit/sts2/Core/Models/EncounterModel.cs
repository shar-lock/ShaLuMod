// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.EncounterModel
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Bindings.MegaSpine;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Ascension;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Encounters;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Random;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Runs;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable enable
namespace MegaCrit.Sts2.Core.Models;

public abstract class EncounterModel : AbstractModel
{
  private const string _locTable = "encounters";
  private Rng? _rng;
  private IReadOnlyList<(MonsterModel, string?)>? _monstersWithSlots;
  private List<MonsterModel>? _spawnedEnemies;
  private EncounterModel _canonicalInstance;
  private BackgroundAssets? _backgroundAssets;

  public override bool ShouldReceiveCombatHooks => false;

  protected Rng Rng => this._rng;

  public abstract RoomType RoomType { get; }

  public virtual bool IsWeak => false;

  public virtual bool ShouldGiveRewards => true;

  public virtual int MinGoldReward
  {
    get
    {
      int num;
      switch (this.RoomType)
      {
        case RoomType.Monster:
          num = 10;
          break;
        case RoomType.Elite:
          num = 35;
          break;
        case RoomType.Boss:
          num = 100;
          break;
        default:
          num = 0;
          break;
      }
      double minGoldReward = (double) num;
      if (AscensionHelper.HasAscension(AscensionLevel.Poverty))
        minGoldReward *= AscensionHelper.PovertyAscensionGoldMultiplier;
      return (int) minGoldReward;
    }
  }

  public virtual int MaxGoldReward
  {
    get
    {
      int num;
      switch (this.RoomType)
      {
        case RoomType.Monster:
          num = 20;
          break;
        case RoomType.Elite:
          num = 45;
          break;
        case RoomType.Boss:
          num = 100;
          break;
        default:
          num = 0;
          break;
      }
      double maxGoldReward = (double) num;
      if (AscensionHelper.HasAscension(AscensionLevel.Poverty))
        maxGoldReward *= AscensionHelper.PovertyAscensionGoldMultiplier;
      return (int) maxGoldReward;
    }
  }

  public LocString? CustomRewardDescription
  {
    get => LocString.GetIfExists("encounters", this.Id.Entry + ".customRewardDescription");
  }

  public virtual IEnumerable<EncounterTag> Tags
  {
    get => (IEnumerable<EncounterTag>) Array.Empty<EncounterTag>();
  }

  public bool HaveMonstersBeenGenerated => this._monstersWithSlots != null;

  public virtual float GetCameraScaling() => 1f;

  public virtual Vector2 GetCameraOffset() => Vector2.Zero;

  public string GetNextSlot(ICombatState combatState)
  {
    return Enumerable.FirstOrDefault<string>((IEnumerable<string>) this.Slots, (Func<string, bool>) (s => combatState.Enemies.All<Creature>((Func<Creature, bool>) (c => c.SlotName != s))), string.Empty);
  }

  protected abstract IReadOnlyList<(MonsterModel, string?)> GenerateMonsters();

  public void GenerateMonstersWithSlots(IRunState runState)
  {
    this.AssertMutable();
    if (this._monstersWithSlots != null)
      throw new InvalidOperationException("Monsters have already been generated for this encounter.");
    if (this._rng == null)
      this._rng = new Rng(runState.Rng.Seed + (ulong) runState.TotalFloor + StringHelper.GetDeterministicHashCode(this.Id.Entry));
    this._monstersWithSlots = this.GenerateMonsters();
    foreach ((MonsterModel, string) monstersWithSlot in (IEnumerable<(MonsterModel, string)>) this._monstersWithSlots)
      monstersWithSlot.Item1.AssertMutable();
  }

  public IReadOnlyList<(MonsterModel, string?)> MonstersWithSlots
  {
    get
    {
      this.AssertMutable();
      return this._monstersWithSlots != null ? this._monstersWithSlots : throw new InvalidOperationException("GenerateMonstersWithSlots must be called before using this property!");
    }
  }

  public IReadOnlyList<MonsterModel> SpawnedEnemies
  {
    get
    {
      this.AssertMutable();
      return (IReadOnlyList<MonsterModel>) this._spawnedEnemies ?? (IReadOnlyList<MonsterModel>) new List<MonsterModel>();
    }
  }

  public abstract IEnumerable<MonsterModel> AllPossibleMonsters { get; }

  public bool SharesTagsWith(EncounterModel? other)
  {
    return other != null && this.Tags.Intersect<EncounterTag>(other.Tags).Any<EncounterTag>();
  }

  public EncounterModel CanonicalInstance
  {
    get => !this.IsMutable ? this : this._canonicalInstance;
    private set
    {
      this.AssertMutable();
      this._canonicalInstance = value;
    }
  }

  public virtual bool HasScene => false;

  public virtual IReadOnlyList<string> Slots => (IReadOnlyList<string>) Array.Empty<string>();

  public virtual bool FullyCenterPlayers => false;

  private string ScenePath
  {
    get => SceneHelper.GetScenePath("encounters/" + this.Id.Entry.ToLowerInvariant());
  }

  protected virtual bool HasCustomBackground => false;

  public NCombatBackground CreateBackground(ActModel parentAct, Rng rng)
  {
    return NCombatBackground.Create(this.GetBackgroundAssets(parentAct, rng));
  }

  private BackgroundAssets GetBackgroundAssets(ActModel parentAct, Rng rng)
  {
    this.AssertMutable();
    if (this._backgroundAssets == null)
      this._backgroundAssets = !this.HasCustomBackground ? parentAct.GenerateBackgroundAssets(rng) : this.CreateBackgroundAssetsForCustom(rng);
    return this._backgroundAssets;
  }

  private BackgroundAssets CreateBackgroundAssetsForCustom(Rng rng)
  {
    return new BackgroundAssets(this.Id.Entry.ToLowerInvariant(), rng);
  }

  public virtual string CustomBgm => "";

  public bool HasBgm => this.CustomBgm != "";

  public virtual string AmbientSfx => "";

  public bool HasAmbientSfx => this.AmbientSfx != "";

  public Control CreateScene()
  {
    return PreloadManager.Cache.GetScene(this.ScenePath).Instantiate<Control>((PackedScene.GenEditState) 0L);
  }

  public virtual string BossNodePath
  {
    get
    {
      return $"res://animations/map/{this.Id.Entry.ToLowerInvariant()}/{this.Id.Entry.ToLowerInvariant()}_node_skel_data.tres";
    }
  }

  public virtual MegaSkeletonDataResource? BossNodeSpineResource
  {
    get
    {
      return !ResourceLoader.Exists(this.BossNodePath, "") ? (MegaSkeletonDataResource) null : new MegaSkeletonDataResource(Variant.op_Implicit((GodotObject) PreloadManager.Cache.GetAsset<Resource>(this.BossNodePath)));
    }
  }

  public LocString Title => EncounterModel.L10NLookup(this.Id.Entry + ".title");

  public EncounterModel ToMutable()
  {
    this.AssertCanonical();
    EncounterModel mutable = (EncounterModel) this.MutableClone();
    mutable.CanonicalInstance = this;
    return mutable;
  }

  public IEnumerable<string> MapNodeAssetPaths
  {
    get
    {
      if (this.BossNodeSpineResource != null)
        return (IEnumerable<string>) new \u003C\u003Ez__ReadOnlySingleElementList<string>(this.BossNodePath);
      return (IEnumerable<string>) new \u003C\u003Ez__ReadOnlyArray<string>(new string[2]
      {
        this.BossNodePath + ".png",
        this.BossNodePath + "_outline.png"
      });
    }
  }

  public IEnumerable<string> GetAssetPaths(IRunState runState)
  {
    HashSet<string> assetPaths = new HashSet<string>();
    assetPaths.UnionWith(this.GetBackgroundAssets(runState.Act, NCombatRoom.GenerateBackgroundRngForCurrentPoint(runState)).AssetPaths);
    if (this.HasScene)
      assetPaths.Add(this.ScenePath);
    assetPaths.UnionWith(this.ExtraAssetPaths);
    foreach ((MonsterModel monsterModel, string _) in (IEnumerable<(MonsterModel, string)>) this.MonstersWithSlots)
      assetPaths.UnionWith(monsterModel.AssetPaths);
    return (IEnumerable<string>) assetPaths;
  }

  public virtual IEnumerable<string> ExtraAssetPaths => (IEnumerable<string>) Array.Empty<string>();

  public void DebugRandomizeRng()
  {
    this.AssertMutable();
    this._rng = new Rng((ulong) (DateTime.UtcNow - DateTime.UnixEpoch).TotalSeconds);
  }

  public LocString GetLossMessageFor(CharacterModel character)
  {
    LocString str = EncounterModel.L10NLookup(this.Id.Entry + ".loss");
    character.AddDetailsTo(str);
    str.Add("encounter", this.Title);
    return str;
  }

  public virtual float CalculateGoldProportion(CombatState combatState)
  {
    return (float) (1.0 - (double) combatState.EscapedCreatures.Count / (double) this.SpawnedEnemies.Count);
  }

  public virtual Dictionary<string, string> SaveCustomState() => new Dictionary<string, string>();

  public virtual void LoadCustomState(Dictionary<string, string> state)
  {
  }

  private static LocString L10NLookup(string key) => new LocString("encounters", key);

  public void OnCreatureSpawned(Creature creature)
  {
    this.AssertMutable();
    if (creature.Side != CombatSide.Enemy)
      return;
    MonsterModel canonicalInstance = creature.Monster?.CanonicalInstance;
    if (canonicalInstance == null || this._spawnedEnemies != null && this._spawnedEnemies.Contains(canonicalInstance))
      return;
    if (this._spawnedEnemies == null)
      this._spawnedEnemies = new List<MonsterModel>();
    this._spawnedEnemies.Add(canonicalInstance);
  }
}
