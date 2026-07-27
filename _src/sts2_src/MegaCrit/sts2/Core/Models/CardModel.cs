// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.CardModel
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Multiplayer;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Hooks;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Nodes.Cards;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.Saves;
using MegaCrit.Sts2.Core.Saves.Runs;
using MegaCrit.Sts2.Core.TestSupport;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models;

public abstract class CardModel : AbstractModel
{
  private LocString? _titleLocString;
  private CardPoolModel? _pool;
  private Player? _owner;
  private CardEnergyCost? _energyCost;
  private int _baseReplayCount;
  private bool _starCostSet;
  private int _baseStarCost;
  private bool _wasStarCostJustUpgraded;
  private List<TemporaryCardCost> _temporaryStarCosts = new List<TemporaryCardCost>();
  private int _lastStarsSpent;
  private HashSet<CardKeyword>? _keywords;
  private HashSet<CardTag>? _tags;
  private DynamicVarSet? _dynamicVars;
  private bool _exhaustOnNextPlay;
  private bool _hasSingleTurnRetain;
  private bool _hasSingleTurnSly;
  private CardModel? _cloneOf;
  private bool _isDupe;
  private int _currentUpgradeLevel;
  private CardUpgradePreviewType _upgradePreviewType;
  private bool _isEnchantmentPreview;
  private int? _floorAddedToDeck;
  private Creature? _currentTarget;
  private int _currentPlayIndex;
  private CardModel? _deckVersion;
  private CardModel? _canonicalInstance;

  protected CardModel(
    int canonicalEnergyCost,
    CardType type,
    CardRarity rarity,
    TargetType targetType,
    bool shouldShowInCardLibrary = true)
  {
    this.CanonicalEnergyCost = canonicalEnergyCost;
    this.Type = type;
    this.Rarity = rarity;
    this.TargetType = targetType;
    this.ShouldShowInCardLibrary = shouldShowInCardLibrary;
  }

  public event Action? AfflictionChanged;

  public event Action? EnchantmentChanged;

  public event Action? EnergyCostChanged;

  public event Action? KeywordsChanged;

  public event Action? ReplayCountChanged;

  public event Action? Played;

  public event Action? Drawn;

  public event Action? StarCostChanged;

  public event Action? Upgraded;

  public event Action? Forged;

  public LocString TitleLocString
  {
    get
    {
      return this._titleLocString ?? (this._titleLocString = new LocString("cards", this.Id.Entry + ".title"));
    }
  }

  public virtual string Title
  {
    get
    {
      LocString titleLocString = this.TitleLocString;
      if (!this.IsUpgraded)
        return titleLocString.GetFormattedText();
      if (this.MaxUpgradeLevel <= 1)
        return titleLocString.GetFormattedText() + "+";
      return $"{titleLocString.GetFormattedText()}+{this.CurrentUpgradeLevel}";
    }
  }

  public LocString Description => new LocString("cards", this.Id.Entry + ".description");

  protected LocString SelectionScreenPrompt
  {
    get
    {
      LocString str = new LocString("cards", this.Id.Entry + ".selectionScreenPrompt");
      if (!str.Exists())
        throw new InvalidOperationException($"No selection screen prompt for {this.Id}.");
      this.DynamicVars.AddTo(str);
      return str;
    }
  }

  public virtual string PortraitPath
  {
    get
    {
      return ImageHelper.GetImagePath($"atlases/card_atlas.sprites/{this.Pool.Title.ToLowerInvariant()}/{this.Id.Entry.ToLowerInvariant()}.tres");
    }
  }

  public virtual string BetaPortraitPath
  {
    get
    {
      return ImageHelper.GetImagePath($"atlases/card_atlas.sprites/{this.Pool.Title.ToLowerInvariant()}/beta/{this.Id.Entry.ToLowerInvariant()}.tres");
    }
  }

  public static string MissingPortraitPath
  {
    get => ImageHelper.GetImagePath("atlases/card_atlas.sprites/beta.tres");
  }

  protected virtual string PortraitPngPath
  {
    get
    {
      return ImageHelper.GetImagePath($"packed/card_portraits/{this.Pool.Title.ToLowerInvariant()}/{this.Id.Entry.ToLowerInvariant()}.png");
    }
  }

  private string BetaPortraitPngPath
  {
    get
    {
      return ImageHelper.GetImagePath($"packed/card_portraits/{this.Pool.Title.ToLowerInvariant()}/beta/{this.Id.Entry.ToLowerInvariant()}.png");
    }
  }

  public bool HasPortrait => ResourceLoader.Exists(this.PortraitPngPath, "");

  public bool HasBetaPortrait => ResourceLoader.Exists(this.BetaPortraitPngPath, "");

  public Texture2D Portrait
  {
    get
    {
      return ResourceLoader.Load<Texture2D>(this.PortraitPath, (string) null, (ResourceLoader.CacheMode) 1L);
    }
  }

  private string FramePath
  {
    get
    {
      CardType cardType;
      switch (this.Type)
      {
        case CardType.None:
        case CardType.Status:
        case CardType.Curse:
          cardType = CardType.Skill;
          break;
        case CardType.Attack:
        case CardType.Skill:
        case CardType.Power:
        case CardType.Quest:
          cardType = this.Type;
          break;
        default:
          throw new ArgumentOutOfRangeException();
      }
      return this.Rarity != CardRarity.Ancient ? ImageHelper.GetImagePath($"atlases/ui_atlas.sprites/card/card_frame_{cardType.ToString().ToLowerInvariant()}_s.tres") : ImageHelper.GetImagePath("atlases/card_atlas.sprites/beta.tres");
    }
  }

  public Texture2D Frame
  {
    get
    {
      return ResourceLoader.Load<Texture2D>(this.FramePath, (string) null, (ResourceLoader.CacheMode) 1L);
    }
  }

  private string PortraitBorderPath
  {
    get
    {
      CardType cardType;
      switch (this.Type)
      {
        case CardType.None:
        case CardType.Status:
        case CardType.Curse:
        case CardType.Quest:
          cardType = CardType.Skill;
          break;
        case CardType.Attack:
        case CardType.Skill:
        case CardType.Power:
          cardType = this.Type;
          break;
        default:
          throw new ArgumentOutOfRangeException();
      }
      return ImageHelper.GetImagePath($"atlases/ui_atlas.sprites/card/card_portrait_border_{cardType.ToString().ToLowerInvariant()}_s.tres");
    }
  }

  private static string AncientBorderPath
  {
    get
    {
      return ImageHelper.GetImagePath("atlases/compressed_atlas.sprites/ancient_card_border.png.tres");
    }
  }

  private string AncientTextBgPath
  {
    get
    {
      if (this.Rarity != CardRarity.Ancient)
        throw new InvalidOperationException("This card is not an ancient card.");
      CardType cardType;
      switch (this.Type)
      {
        case CardType.None:
        case CardType.Status:
        case CardType.Curse:
          cardType = CardType.Skill;
          break;
        case CardType.Attack:
        case CardType.Skill:
        case CardType.Power:
        case CardType.Quest:
          cardType = this.Type;
          break;
        default:
          throw new ArgumentOutOfRangeException();
      }
      return ImageHelper.GetImagePath($"atlases/compressed_atlas.sprites/ancient_text_bg_{cardType.ToString().ToLowerInvariant()}.png.tres");
    }
  }

  public Texture2D AncientTextBg
  {
    get
    {
      return ResourceLoader.Load<Texture2D>(this.AncientTextBgPath, (string) null, (ResourceLoader.CacheMode) 1L);
    }
  }

  public Texture2D AncientBorder
  {
    get
    {
      return ResourceLoader.Load<Texture2D>(CardModel.AncientBorderPath, (string) null, (ResourceLoader.CacheMode) 1L);
    }
  }

  public Texture2D PortraitBorder
  {
    get
    {
      return ResourceLoader.Load<Texture2D>(this.PortraitBorderPath, (string) null, (ResourceLoader.CacheMode) 1L);
    }
  }

  private string EnergyIconPath => this.VisualCardPool.EnergyIconPath;

  public Texture2D EnergyIcon
  {
    get
    {
      return ResourceLoader.Load<Texture2D>(this.EnergyIconPath, (string) null, (ResourceLoader.CacheMode) 1L);
    }
  }

  protected IHoverTip EnergyHoverTip => HoverTipFactory.ForEnergy(this);

  private string BannerTexturePath
  {
    get
    {
      return this.Rarity != CardRarity.Ancient ? ImageHelper.GetImagePath("atlases/ui_atlas.sprites/card/card_banner.tres") : ImageHelper.GetImagePath("atlases/ui_atlas.sprites/card/card_banner_ancient_s.tres");
    }
  }

  public Texture2D BannerTexture
  {
    get
    {
      return ResourceLoader.Load<Texture2D>(this.BannerTexturePath, (string) null, (ResourceLoader.CacheMode) 1L);
    }
  }

  private string BannerMaterialPath
  {
    get
    {
      string bannerMaterialPath;
      switch (this.Rarity)
      {
        case CardRarity.Uncommon:
          bannerMaterialPath = "res://materials/cards/banners/card_banner_uncommon_mat.tres";
          break;
        case CardRarity.Rare:
          bannerMaterialPath = "res://materials/cards/banners/card_banner_rare_mat.tres";
          break;
        case CardRarity.Ancient:
          bannerMaterialPath = "res://materials/cards/banners/card_banner_ancient_mat.tres";
          break;
        case CardRarity.Event:
          bannerMaterialPath = "res://materials/cards/banners/card_banner_event_mat.tres";
          break;
        case CardRarity.Status:
          bannerMaterialPath = "res://materials/cards/banners/card_banner_status_mat.tres";
          break;
        case CardRarity.Curse:
          bannerMaterialPath = "res://materials/cards/banners/card_banner_curse_mat.tres";
          break;
        case CardRarity.Quest:
          bannerMaterialPath = "res://materials/cards/banners/card_banner_quest_mat.tres";
          break;
        default:
          bannerMaterialPath = "res://materials/cards/banners/card_banner_common_mat.tres";
          break;
      }
      return bannerMaterialPath;
    }
  }

  public Material BannerMaterial => PreloadManager.Cache.GetMaterial(this.BannerMaterialPath);

  public Material FrameMaterial => this.VisualCardPool.FrameMaterial;

  public virtual CardType Type { get; }

  public virtual CardRarity Rarity { get; }

  public virtual CardMultiplayerConstraint MultiplayerConstraint => CardMultiplayerConstraint.None;

  public virtual CardPoolModel Pool
  {
    get
    {
      if (this._pool != null)
        return this._pool;
      this._pool = ModelDb.AllCardPools.FirstOrDefault<CardPoolModel>((Func<CardPoolModel, bool>) (pool => pool.AllCardIds.Contains<ModelId>(this.Id)));
      if (this._pool != null)
        return this._pool;
      if (ModelDb.CardPool<MockCardPool>().AllCardIds.Contains<ModelId>(this.Id))
      {
        this._pool = (CardPoolModel) ModelDb.CardPool<MockCardPool>();
        return this._pool;
      }
      throw new InvalidProgramException($"Card {this} is not in any card pool!");
    }
  }

  public virtual CardPoolModel VisualCardPool => this.Pool;

  public Player Owner
  {
    get
    {
      this.AssertMutable();
      return this._owner;
    }
    set
    {
      this.AssertMutable();
      this._owner = this._owner == null || value == null ? value : throw new InvalidOperationException($"Card {this.Id.Entry} already has an owner.");
    }
  }

  public CardPile? Pile
  {
    get
    {
      Player owner = this._owner;
      return owner == null ? (CardPile) null : owner.Piles.FirstOrDefault<CardPile>((Func<CardPile, bool>) (p => p.Cards.Contains<CardModel>(this)));
    }
  }

  protected virtual int CanonicalEnergyCost { get; }

  protected virtual bool HasEnergyCostX => false;

  protected void MockSetEnergyCost(CardEnergyCost cost) => this._energyCost = cost;

  public CardEnergyCost EnergyCost
  {
    get
    {
      if (this._energyCost == null)
        this._energyCost = new CardEnergyCost(this, this.CanonicalEnergyCost, this.HasEnergyCostX);
      return this._energyCost;
    }
  }

  public void InvokeEnergyCostChanged()
  {
    Action energyCostChanged = this.EnergyCostChanged;
    if (energyCostChanged == null)
      return;
    energyCostChanged();
  }

  public int ResolveEnergyXValue()
  {
    if (!this.EnergyCost.CostsX)
      throw new InvalidOperationException("This card does not have an X-cost.");
    return Hook.ModifyXValue(this.CombatState, this, this.EnergyCost.CapturedXValue);
  }

  public int BaseReplayCount
  {
    get => this._baseReplayCount;
    set
    {
      this.AssertMutable();
      this._baseReplayCount = value;
      Action replayCountChanged = this.ReplayCountChanged;
      if (replayCountChanged == null)
        return;
      replayCountChanged();
    }
  }

  public int GetEnchantedReplayCount()
  {
    EnchantmentModel enchantment = this.Enchantment;
    return enchantment == null ? this.BaseReplayCount : enchantment.EnchantPlayCount(this.BaseReplayCount);
  }

  public virtual int CanonicalStarCost => -1;

  public int BaseStarCost
  {
    get
    {
      if (!this.IsMutable)
        return this.CanonicalStarCost;
      if (!this._starCostSet)
      {
        this._baseStarCost = this.CanonicalStarCost;
        this._starCostSet = true;
      }
      return this._baseStarCost;
    }
    private set
    {
      this.AssertMutable();
      if (!this.HasStarCostX)
      {
        this._baseStarCost = value;
        this._starCostSet = true;
      }
      Action starCostChanged = this.StarCostChanged;
      if (starCostChanged == null)
        return;
      starCostChanged();
    }
  }

  public bool WasStarCostJustUpgraded => this._wasStarCostJustUpgraded;

  public TemporaryCardCost? TemporaryStarCost
  {
    get => this._temporaryStarCosts.LastOrDefault<TemporaryCardCost>();
  }

  public virtual int CurrentStarCost
  {
    get
    {
      int? cost = this._temporaryStarCosts.LastOrDefault<TemporaryCardCost>()?.Cost;
      if (!cost.HasValue)
        return this.BaseStarCost;
      int? nullable = cost;
      int num = 0;
      return nullable.GetValueOrDefault() == num & nullable.HasValue && this.BaseStarCost < 0 ? this.BaseStarCost : cost.Value;
    }
  }

  public virtual bool HasStarCostX => false;

  public int LastStarsSpent
  {
    get => this._lastStarsSpent;
    set
    {
      this.AssertMutable();
      this._lastStarsSpent = value;
    }
  }

  public int ResolveStarXValue()
  {
    if (!this.HasStarCostX)
      throw new InvalidOperationException("This card does not have an X-cost.");
    return Hook.ModifyXValue(this.CombatState, this, this.LastStarsSpent);
  }

  public virtual TargetType TargetType { get; }

  public virtual IEnumerable<CardKeyword> CanonicalKeywords
  {
    get => (IEnumerable<CardKeyword>) Array.Empty<CardKeyword>();
  }

  private HashSet<CardKeyword> LocalKeywords
  {
    get
    {
      if (this._keywords != null)
        return this._keywords;
      this._keywords = new HashSet<CardKeyword>();
      this._keywords.UnionWith(this.CanonicalKeywords);
      return this._keywords;
    }
  }

  public IReadOnlySet<CardKeyword> Keywords => this.GetKeywordsWithSources(KeywordSources.All);

  public IReadOnlySet<CardKeyword> GetKeywordsWithSources(KeywordSources sources)
  {
    bool flag = sources.HasFlag((Enum) KeywordSources.Local);
    if (!sources.HasFlag((Enum) KeywordSources.Global) || this.IsCanonical || this.CombatState == null)
      return !flag ? (IReadOnlySet<CardKeyword>) ImmutableHashSet<CardKeyword>.Empty : (IReadOnlySet<CardKeyword>) this.LocalKeywords;
    HashSet<CardKeyword> cardKeywordSet1;
    if (flag)
    {
      HashSet<CardKeyword> cardKeywordSet2 = new HashSet<CardKeyword>();
      foreach (CardKeyword localKeyword in this.LocalKeywords)
        cardKeywordSet2.Add(localKeyword);
      cardKeywordSet1 = cardKeywordSet2;
    }
    else
      cardKeywordSet1 = new HashSet<CardKeyword>();
    HashSet<CardKeyword> keywords = cardKeywordSet1;
    Hook.ModifyKeywordsInCombat(this.CombatState, this, (ISet<CardKeyword>) keywords);
    return (IReadOnlySet<CardKeyword>) keywords;
  }

  public virtual IEnumerable<CardTag> Tags
  {
    get
    {
      return (IEnumerable<CardTag>) this._tags ?? (IEnumerable<CardTag>) (this._tags = this.CanonicalTags);
    }
  }

  protected virtual HashSet<CardTag> CanonicalTags => new HashSet<CardTag>();

  public DynamicVarSet DynamicVars
  {
    get
    {
      if (this._dynamicVars != null)
        return this._dynamicVars;
      this._dynamicVars = new DynamicVarSet(this.CanonicalVars);
      this._dynamicVars.InitializeWithOwner((AbstractModel) this);
      return this._dynamicVars;
    }
  }

  protected virtual IEnumerable<DynamicVar> CanonicalVars
  {
    get => (IEnumerable<DynamicVar>) Array.Empty<DynamicVar>();
  }

  public bool ExhaustOnNextPlay
  {
    get => this._exhaustOnNextPlay;
    set
    {
      this.AssertMutable();
      this._exhaustOnNextPlay = value;
    }
  }

  private bool HasSingleTurnRetain
  {
    get => this._hasSingleTurnRetain;
    set
    {
      this.AssertMutable();
      this._hasSingleTurnRetain = value;
    }
  }

  public bool ShouldRetainThisTurn
  {
    get => this.Keywords.Contains(CardKeyword.Retain) || this.HasSingleTurnRetain;
  }

  private bool HasSingleTurnSly
  {
    get => this._hasSingleTurnSly;
    set
    {
      this.AssertMutable();
      this._hasSingleTurnSly = value;
    }
  }

  public bool IsSlyThisTurn => this.Keywords.Contains(CardKeyword.Sly) || this.HasSingleTurnSly;

  public EnchantmentModel? Enchantment { get; private set; }

  public AfflictionModel? Affliction { get; private set; }

  public virtual bool CanBeGeneratedInCombat => true;

  public virtual bool CanBeGeneratedByModifiers => true;

  public virtual OrbEvokeType OrbEvokeType => OrbEvokeType.None;

  public virtual bool GainsBlock => false;

  public virtual bool IsBasicStrikeOrDefend
  {
    get
    {
      return this.Rarity == CardRarity.Basic && (this.Tags.Contains<CardTag>(CardTag.Strike) || this.Tags.Contains<CardTag>(CardTag.Defend));
    }
  }

  public CardModel? CloneOf => this._cloneOf;

  public bool IsClone => this.CloneOf != null;

  public CardModel? DupeOf => !this.IsDupe ? (CardModel) null : this.CloneOf;

  public bool IsDupe
  {
    get => this._isDupe;
    private set
    {
      this.AssertMutable();
      this._isDupe = value;
    }
  }

  public bool IsRemovable => !this.Keywords.Contains(CardKeyword.Eternal);

  public bool IsTransformable
  {
    get
    {
      if (this.IsRemovable)
        return true;
      CardPile pile = this.Pile;
      return (pile == null ? 0 : (pile.Type == PileType.Deck ? 1 : 0)) == 0;
    }
  }

  public bool IsInCombat
  {
    get
    {
      if (!this.IsMutable)
        return false;
      CardPile pile = this.Pile;
      return pile != null && pile.IsCombatPile;
    }
  }

  public int CurrentUpgradeLevel
  {
    get => this._currentUpgradeLevel;
    private set
    {
      this.AssertMutable();
      this._currentUpgradeLevel = value <= this.MaxUpgradeLevel ? value : throw new InvalidOperationException($"{this.Id} cannot be upgraded past its MaxUpgradeLevel.");
    }
  }

  public virtual int MaxUpgradeLevel => 1;

  public bool IsUpgraded => this.CurrentUpgradeLevel > 0;

  public bool IsUpgradable => this.CurrentUpgradeLevel < this.MaxUpgradeLevel;

  public CardUpgradePreviewType UpgradePreviewType
  {
    get => this._upgradePreviewType;
    set
    {
      this.AssertMutable();
      this._upgradePreviewType = value.IsPreview() || !this._upgradePreviewType.IsPreview() ? value : throw new InvalidOperationException("A card cannot go to from being upgrade preview. Consider making a new card model instead.");
    }
  }

  protected virtual bool IsPlayable => true;

  public bool ShouldShowInCardLibrary { get; }

  public bool ShouldGlowGold
  {
    get
    {
      if (this.ShouldGlowGoldInternal)
        return true;
      EnchantmentModel enchantment = this.Enchantment;
      return enchantment != null && enchantment.ShouldGlowGold;
    }
  }

  public bool ShouldGlowRed
  {
    get
    {
      if (this.ShouldGlowRedInternal)
        return true;
      EnchantmentModel enchantment = this.Enchantment;
      return enchantment != null && enchantment.ShouldGlowRed;
    }
  }

  protected virtual bool ShouldGlowGoldInternal => false;

  protected virtual bool ShouldGlowRedInternal => false;

  public bool IsEnchantmentPreview
  {
    get => this._isEnchantmentPreview;
    set
    {
      this.AssertMutable();
      this._isEnchantmentPreview = value;
    }
  }

  public virtual bool HasBuiltInOverlay => false;

  public string OverlayPath
  {
    get => SceneHelper.GetScenePath("cards/overlays/" + this.Id.Entry.ToLowerInvariant());
  }

  public Control CreateOverlay()
  {
    return PreloadManager.Cache.GetScene(this.OverlayPath).Instantiate<Control>((PackedScene.GenEditState) 0L);
  }

  public int? FloorAddedToDeck
  {
    get => this._floorAddedToDeck;
    set
    {
      this.AssertMutable();
      this._floorAddedToDeck = value;
    }
  }

  public Creature? CurrentTarget
  {
    get => this._currentTarget;
    private set
    {
      this.AssertMutable();
      this._currentTarget = value;
    }
  }

  public int CurrentPlayIndex
  {
    get => this._currentPlayIndex;
    private set
    {
      this.AssertMutable();
      this._currentPlayIndex = value;
    }
  }

  public CardModel? DeckVersion
  {
    get => this._deckVersion;
    set
    {
      this.AssertMutable();
      this._deckVersion = value;
    }
  }

  public bool HasBeenRemovedFromState { get; set; }

  protected virtual IEnumerable<IHoverTip> ExtraHoverTips
  {
    get => (IEnumerable<IHoverTip>) Array.Empty<IHoverTip>();
  }

  public IEnumerable<IHoverTip> HoverTips
  {
    get
    {
      List<IHoverTip> list = this.ExtraHoverTips.ToList<IHoverTip>();
      if (this.Enchantment != null)
        list.AddRange(this.Enchantment.HoverTips);
      if (this.Affliction != null)
        list.AddRange(this.Affliction.HoverTips);
      int enchantedReplayCount = this.GetEnchantedReplayCount();
      if (enchantedReplayCount > 0)
        list.Add(HoverTipFactory.Static(StaticHoverTip.ReplayDynamic, new DynamicVar("Times", (Decimal) enchantedReplayCount)));
      if (this.OrbEvokeType != OrbEvokeType.None)
        list.Add(HoverTipFactory.Static(StaticHoverTip.Evoke));
      if (this.GainsBlock)
        list.Add(HoverTipFactory.Static(StaticHoverTip.Block));
      foreach (CardKeyword keyword in (IEnumerable<CardKeyword>) this.Keywords)
      {
        list.Add(HoverTipFactory.FromKeyword(keyword));
        if (keyword == CardKeyword.Ethereal)
          list.Add(HoverTipFactory.FromKeyword(CardKeyword.Exhaust));
      }
      return list.Distinct<IHoverTip>();
    }
  }

  public CardModel CanonicalInstance
  {
    get => !this.IsMutable ? this : this._canonicalInstance;
    private set
    {
      this.AssertMutable();
      this._canonicalInstance = value;
    }
  }

  public IRunState? RunState => this._owner?.RunState;

  public ICombatState? CombatState
  {
    get
    {
      CardPile pile = this.Pile;
      if ((pile == null || !pile.IsCombatPile) && this.UpgradePreviewType != CardUpgradePreviewType.Combat)
        return (ICombatState) null;
      return this._owner?.Creature.CombatState;
    }
  }

  public ICardScope? CardScope
  {
    get
    {
      return (ICardScope) this.CombatState ?? (ICardScope) this._owner?.Creature.CombatState ?? (ICardScope) this.RunState;
    }
  }

  public CardModel ToMutable()
  {
    this.AssertCanonical();
    return (CardModel) this.MutableClone();
  }

  protected override void DeepCloneFields()
  {
    HashSet<CardKeyword> cardKeywordSet = new HashSet<CardKeyword>();
    foreach (CardKeyword keywordsWithSource in (IEnumerable<CardKeyword>) this.GetKeywordsWithSources(KeywordSources.Local))
      cardKeywordSet.Add(keywordsWithSource);
    this._keywords = cardKeywordSet;
    this._dynamicVars = this.DynamicVars.Clone((AbstractModel) this);
    this._energyCost = this._energyCost?.Clone(this);
    this._temporaryStarCosts = this._temporaryStarCosts.ToList<TemporaryCardCost>();
    if (this.Enchantment != null)
    {
      EnchantmentModel enchantment = (EnchantmentModel) this.Enchantment.ClonePreservingMutability();
      this.Enchantment = (EnchantmentModel) null;
      this.EnchantInternal(enchantment, (Decimal) enchantment.Amount);
    }
    if (this.Affliction == null)
      return;
    AfflictionModel affliction = (AfflictionModel) this.Affliction.ClonePreservingMutability();
    this.Affliction = (AfflictionModel) null;
    this.AfflictInternal(affliction, (Decimal) affliction.Amount);
  }

  protected override void AfterCloned()
  {
    base.AfterCloned();
    if (this._canonicalInstance == null)
      this._canonicalInstance = ModelDb.GetById<CardModel>(this.Id);
    this.CurrentTarget = (Creature) null;
    this.CurrentPlayIndex = 0;
    this.DeckVersion = (CardModel) null;
    this.HasBeenRemovedFromState = false;
    this.AfflictionChanged = (Action) null;
    this.Drawn = (Action) null;
    this.EnchantmentChanged = (Action) null;
    this.EnergyCostChanged = (Action) null;
    this.Forged = (Action) null;
    this.KeywordsChanged = (Action) null;
    this.Played = (Action) null;
    this.ReplayCountChanged = (Action) null;
    this.StarCostChanged = (Action) null;
    this.Upgraded = (Action) null;
  }

  public virtual void AfterCreated()
  {
  }

  protected virtual void AfterDeserialized()
  {
  }

  protected void NeverEverCallThisOutsideOfTests_ClearOwner()
  {
    if (TestMode.IsOff)
      throw new InvalidOperationException("You monster!");
    this._owner = (Player) null;
  }

  public void SetToFreeThisTurn()
  {
    this.EnergyCost.SetThisTurnOrUntilPlayed(0);
    this.SetStarCostThisTurn(0);
  }

  public void SetToFreeThisCombat()
  {
    this.EnergyCost.SetThisCombat(0);
    this.SetStarCostThisCombat(0);
  }

  public void SetStarCostUntilPlayed(int cost)
  {
    this.AddTemporaryStarCost(TemporaryCardCost.UntilPlayed(cost));
  }

  public void SetStarCostThisTurn(int cost)
  {
    this.AddTemporaryStarCost(TemporaryCardCost.ThisTurn(cost));
  }

  public void SetStarCostThisCombat(int cost)
  {
    this.AddTemporaryStarCost(TemporaryCardCost.ThisCombat(cost));
  }

  public int GetStarCostThisCombat()
  {
    TemporaryCardCost temporaryCardCost = this._temporaryStarCosts.FirstOrDefault<TemporaryCardCost>((Func<TemporaryCardCost, bool>) (cost => cost != null && !cost.ClearsWhenTurnEnds && !cost.ClearsWhenCardIsPlayed));
    return temporaryCardCost == null ? this.BaseStarCost : temporaryCardCost.Cost;
  }

  private void AddTemporaryStarCost(TemporaryCardCost cost)
  {
    this.AssertMutable();
    this._temporaryStarCosts.Add(cost);
    Action starCostChanged = this.StarCostChanged;
    if (starCostChanged == null)
      return;
    starCostChanged();
  }

  protected void UpgradeStarCostBy(int addend)
  {
    if (this.HasStarCostX)
      throw new InvalidOperationException($"UpgradeStarCostBy called on {this.Id.Entry} which has star cost X.");
    if (addend == 0)
      return;
    int baseStarCost = this.BaseStarCost;
    this.BaseStarCost += addend;
    this._wasStarCostJustUpgraded = true;
    if (this.BaseStarCost >= baseStarCost)
      return;
    this._temporaryStarCosts.RemoveAll((Predicate<TemporaryCardCost>) (c => c.Cost > this.BaseStarCost));
  }

  public void AddKeyword(CardKeyword keyword)
  {
    this.AssertMutable();
    this.LocalKeywords.Add(keyword);
    Action keywordsChanged = this.KeywordsChanged;
    if (keywordsChanged == null)
      return;
    keywordsChanged();
  }

  public void RemoveKeyword(CardKeyword keyword)
  {
    this.AssertMutable();
    this.LocalKeywords.Remove(keyword);
    Action keywordsChanged = this.KeywordsChanged;
    if (keywordsChanged == null)
      return;
    keywordsChanged();
  }

  public void GiveSingleTurnRetain() => this.HasSingleTurnRetain = true;

  public void GiveSingleTurnSly() => this.HasSingleTurnSly = true;

  public string GetDescriptionForPile(PileType pileType, Creature? target = null)
  {
    return this.GetDescriptionForPile(pileType, CardModel.DescriptionPreviewType.None, target);
  }

  public string GetDescriptionForUpgradePreview()
  {
    return this.GetDescriptionForPile(PileType.None, CardModel.DescriptionPreviewType.Upgrade);
  }

  private string GetDescriptionForPile(
    PileType pileType,
    CardModel.DescriptionPreviewType previewType,
    Creature? target = null)
  {
    LocString description = this.Description;
    this.DynamicVars.AddTo(description);
    this.AddExtraArgsToDescription(description);
    UpgradeDisplay upgradeDisplay = previewType != CardModel.DescriptionPreviewType.Upgrade ? (!this.IsUpgraded ? UpgradeDisplay.Normal : UpgradeDisplay.Upgraded) : UpgradeDisplay.UpgradePreview;
    description.Add((DynamicVar) new IfUpgradedVar(upgradeDisplay));
    bool variable1 = pileType == PileType.Hand || pileType == PileType.Play;
    description.Add("OnTable", variable1);
    int num1;
    if (CombatManager.Instance.IsInProgress)
    {
      CardPile pile = this.Pile;
      num1 = pile != null ? (pile.IsCombatPile ? 1 : 0) : (pileType.IsCombatPile() ? 1 : 0);
    }
    else
      num1 = 0;
    bool variable2 = num1 != 0;
    description.Add("InCombat", variable2);
    description.Add("IsTargeting", target != null);
    description.Add("TargetType", this.TargetType.ToString());
    description.Add("GainsBlock", this.GainsBlock);
    LocString locString1 = description;
    int num2;
    if (this.IsMutable)
    {
      Player owner = this.Owner;
      num2 = owner == null ? 0 : (owner.IsOstyAlive ? 1 : 0);
    }
    else
      num2 = 0;
    locString1.Add("IsOstyAlive", num2 != 0);
    string prefix = EnergyIconHelper.GetPrefix((AbstractModel) this);
    description.Add("energyPrefix", prefix);
    description.Add("singleStarIcon", "[img]res://images/packed/sprite_fonts/star_icon.png[/img]");
    foreach (KeyValuePair<string, object> variable3 in (IEnumerable<KeyValuePair<string, object>>) description.Variables)
    {
      if (variable3.Value is EnergyVar energyVar)
        energyVar.ColorPrefix = prefix;
    }
    int capacity = 1;
    List<string> stringList = new List<string>(capacity);
    CollectionsMarshal.SetCount<string>(stringList, capacity);
    CollectionsMarshal.AsSpan<string>(stringList)[0] = description.GetFormattedText();
    List<string> source = stringList;
    LocString dynamicExtraCardText1 = this.Enchantment?.DynamicExtraCardText;
    if (dynamicExtraCardText1 != null)
      source.Add($"[purple]{dynamicExtraCardText1.GetFormattedText()}[/purple]");
    LocString dynamicExtraCardText2 = this.Affliction?.DynamicExtraCardText;
    if (dynamicExtraCardText2 != null)
      source.Add($"[purple]{dynamicExtraCardText2.GetFormattedText()}[/purple]");
    IReadOnlySet<CardKeyword> keywords = this.Keywords;
    foreach (CardKeyword keyword in CardKeywordOrder.beforeDescription)
    {
      bool flag;
      switch (keyword)
      {
        case CardKeyword.Retain:
          flag = this.ShouldRetainThisTurn;
          break;
        case CardKeyword.Sly:
          flag = this.IsSlyThisTurn;
          break;
        default:
          flag = keywords.Contains(keyword);
          break;
      }
      if (flag)
        source.Insert(0, keyword.GetCardText());
    }
    int enchantedReplayCount = this.GetEnchantedReplayCount();
    if (enchantedReplayCount > 0)
    {
      LocString locString2 = new LocString("static_hover_tips", "REPLAY.extraText");
      locString2.Add("Times", (Decimal) enchantedReplayCount);
      source.Add(locString2.GetFormattedText() ?? "");
    }
    foreach (CardKeyword keyword in ((IEnumerable<CardKeyword>) CardKeywordOrder.afterDescription).Intersect<CardKeyword>((IEnumerable<CardKeyword>) keywords))
      source.Add(keyword.GetCardText());
    return string.Join<string>('\n', source.Where<string>((Func<string, bool>) (l => !string.IsNullOrEmpty(l))));
  }

  public void UpdateDynamicVarPreview(
    CardPreviewMode previewMode,
    Creature? target,
    DynamicVarSet dynamicVarSet)
  {
    if (this.RunState == null && this.CombatState == null)
      return;
    bool flag1 = this.CombatState != null;
    if (flag1)
    {
      PileType? type = this.Pile?.Type;
      bool flag2;
      if (type.HasValue)
      {
        switch (type.GetValueOrDefault())
        {
          case PileType.Hand:
          case PileType.Play:
            flag2 = true;
            goto label_7;
        }
      }
      flag2 = false;
label_7:
      flag1 = flag2 || this.UpgradePreviewType == CardUpgradePreviewType.Combat;
    }
    bool runGlobalHooks = flag1;
    foreach (DynamicVar dynamicVar in (IEnumerable<DynamicVar>) dynamicVarSet.Values.ToList<DynamicVar>())
      dynamicVar.UpdateCardPreview(this, previewMode, target, runGlobalHooks);
  }

  public void EnchantInternal(EnchantmentModel enchantment, Decimal amount)
  {
    this.AssertMutable();
    enchantment.AssertMutable();
    this.Enchantment = enchantment;
    this.Enchantment.ApplyInternal(this, amount);
    Action enchantmentChanged = this.EnchantmentChanged;
    if (enchantmentChanged == null)
      return;
    enchantmentChanged();
  }

  public void AfflictInternal(AfflictionModel affliction, Decimal amount)
  {
    this.AssertMutable();
    affliction.AssertMutable();
    this.Affliction = this.Affliction == null ? affliction : throw new InvalidOperationException($"Attempted to afflict card {this} that was already afflicted! This is not allowed");
    this.Affliction.Card = this;
    this.Affliction.Amount = (int) amount;
    Action afflictionChanged = this.AfflictionChanged;
    if (afflictionChanged == null)
      return;
    afflictionChanged();
  }

  public void ClearEnchantmentInternal()
  {
    if (this.Enchantment == null)
      return;
    this.AssertMutable();
    this.Enchantment.ClearInternal();
    this.Enchantment = (EnchantmentModel) null;
    Action enchantmentChanged = this.EnchantmentChanged;
    if (enchantmentChanged == null)
      return;
    enchantmentChanged();
  }

  public void ClearAfflictionInternal()
  {
    this.AssertMutable();
    if (this.Affliction == null)
      return;
    this.Affliction.ClearInternal();
    this.Affliction = (AfflictionModel) null;
    this.Owner.PlayerCombatState.RecalculateCardValues();
    Action afflictionChanged = this.AfflictionChanged;
    if (afflictionChanged == null)
      return;
    afflictionChanged();
  }

  protected virtual void AddExtraArgsToDescription(LocString description)
  {
  }

  public int GetStarCostWithModifiers()
  {
    if (this.HasStarCostX)
    {
      PlayerCombatState playerCombatState = this.Owner.PlayerCombatState;
      return playerCombatState == null ? 0 : playerCombatState.Stars;
    }
    CardPile pile = this.Pile;
    return pile != null && pile.IsCombatPile && this.CombatState != null ? (int) Hook.ModifyStarCost(this.CombatState, this, (Decimal) this.CurrentStarCost) : this.CurrentStarCost;
  }

  public bool CostsEnergyOrStars(bool includeGlobalModifiers)
  {
    if (includeGlobalModifiers)
    {
      if (!this.EnergyCost.CostsX && this.EnergyCost.GetWithModifiers(CostModifiers.All) > 0 || !this.HasStarCostX && this.GetStarCostWithModifiers() > 0)
        return true;
    }
    else if (this.EnergyCost.GetWithModifiers(CostModifiers.Local) > 0 || this.CurrentStarCost > 0)
      return true;
    return false;
  }

  public void RemoveFromCurrentPile(bool silent = false)
  {
    this.AssertMutable();
    this.Pile?.RemoveInternal(this, silent);
  }

  public void RemoveFromState()
  {
    this.RemoveFromCurrentPile();
    this.HasBeenRemovedFromState = true;
  }

  public void EndOfTurnCleanup()
  {
    this.ExhaustOnNextPlay = false;
    this.HasSingleTurnRetain = false;
    this.HasSingleTurnSly = false;
    if (this.EnergyCost.EndOfTurnCleanup())
    {
      Action energyCostChanged = this.EnergyCostChanged;
      if (energyCostChanged != null)
        energyCostChanged();
    }
    if (this._temporaryStarCosts.RemoveAll((Predicate<TemporaryCardCost>) (c => c.ClearsWhenTurnEnds)) <= 0)
      return;
    Action starCostChanged = this.StarCostChanged;
    if (starCostChanged == null)
      return;
    starCostChanged();
  }

  public virtual void AfterTransformedFrom()
  {
  }

  public virtual void AfterTransformedTo()
  {
  }

  public void AfterForged()
  {
    Action forged = this.Forged;
    if (forged == null)
      return;
    forged();
  }

  protected virtual Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
  {
    return Task.CompletedTask;
  }

  public virtual Task OnEnqueuePlayVfx(Creature? target) => Task.CompletedTask;

  protected virtual void OnUpgrade()
  {
  }

  public virtual bool HasTurnEndInHandEffect => false;

  protected virtual Task OnTurnEndInHand(PlayerChoiceContext choiceContext) => Task.CompletedTask;

  public async Task OnTurnEndInHandWrapper(PlayerChoiceContext choiceContext)
  {
    CardPileAddResult cardPileAddResult1 = await CardPileCmd.Add(this, PileType.Play);
    if (LocalContext.IsMe(this.Owner))
      await Cmd.CustomScaledWait(0.3f, 0.6f);
    await this.OnTurnEndInHand(choiceContext);
    if (this.Keywords.Contains(CardKeyword.Ethereal))
    {
      await CardCmd.Exhaust(choiceContext, this, true);
    }
    else
    {
      CardPileAddResult cardPileAddResult2 = await CardPileCmd.Add(this, PileType.Discard.GetPile(this.Owner));
    }
  }

  public bool CanPlayTargeting(Creature? target) => this.IsValidTarget(target) && this.CanPlay();

  public bool CanPlay() => this.CanPlay(out UnplayableReason _, out AbstractModel _);

  public bool CanPlay(out UnplayableReason reason, out AbstractModel? preventer)
  {
    reason = UnplayableReason.None;
    ICombatState combatState = this.CombatState ?? this._owner?.Creature.CombatState;
    if (combatState == null || this.Owner.PlayerCombatState == null)
    {
      preventer = (AbstractModel) null;
      return false;
    }
    if (this.Keywords.Contains(CardKeyword.Unplayable))
      reason |= UnplayableReason.HasUnplayableKeyword;
    UnplayableReason reason1;
    if (!this.Owner.PlayerCombatState.HasEnoughResourcesFor(this, out reason1))
      reason |= reason1;
    if (this.TargetType == TargetType.AnyAlly && combatState.PlayerCreatures.Count<Creature>((Func<Creature, bool>) (c => c.IsAlive)) <= 1)
      reason |= UnplayableReason.NoLivingAllies;
    if (!Hook.ShouldPlay(combatState, this, out preventer, AutoPlayType.None))
      reason |= UnplayableReason.BlockedByHook;
    if (!this.IsPlayable)
      reason |= UnplayableReason.BlockedByCardLogic;
    return reason == UnplayableReason.None;
  }

  public bool IsValidTarget(Creature? target)
  {
    if (target == null)
      return this.TargetType != TargetType.AnyEnemy && this.TargetType != TargetType.AnyAlly;
    if (!target.IsAlive)
      return false;
    if (this.TargetType == TargetType.AnyEnemy)
      return target.Side != this.Owner.Creature.Side;
    return this.TargetType == TargetType.AnyAlly && target.Side == this.Owner.Creature.Side;
  }

  public bool TryManualPlay(Creature? target)
  {
    if (!this.CanPlayTargeting(target))
      return false;
    this.EnqueueManualPlay(target);
    return true;
  }

  private void EnqueueManualPlay(Creature? target)
  {
    TaskHelper.RunSafely(this.OnEnqueuePlayVfx(target));
    RunManager.Instance.ActionQueueSynchronizer.RequestEnqueue((GameAction) new PlayCardAction(this, target));
  }

  public async Task<(int, int)> SpendResources()
  {
    int energy = this.Owner.PlayerCombatState.Energy;
    int energyToSpend = this.EnergyCost.GetAmountToSpend();
    int starsToSpend = Math.Max(0, this.GetStarCostWithModifiers());
    if (energyToSpend > energy && Hook.ShouldPayExcessEnergyCostWithStars(this.CombatState, this.Owner))
    {
      starsToSpend += (energyToSpend - energy) * 2;
      energyToSpend = energy;
    }
    await this.SpendEnergy(energyToSpend);
    await this.SpendStars(starsToSpend);
    return (energyToSpend, starsToSpend);
  }

  private async Task SpendEnergy(int amount)
  {
    if (this.EnergyCost.CostsX)
      this.EnergyCost.CapturedXValue = amount;
    if (amount > 0)
    {
      CombatManager.Instance.History.EnergySpent(this.CombatState, amount, this.Owner);
      this.Owner.PlayerCombatState.LoseEnergy((Decimal) Math.Max(0, amount));
    }
    await Hook.AfterEnergySpent(this.CombatState, this, amount);
  }

  private async Task SpendStars(int amount)
  {
    this.LastStarsSpent = amount;
    if (amount <= 0)
      return;
    this.Owner.PlayerCombatState.LoseStars((Decimal) amount);
    await Hook.AfterStarsSpent(this.Owner.Creature.CombatState, amount, this.Owner);
  }

  public async Task OnPlayWrapper(
    PlayerChoiceContext choiceContext,
    Creature? target,
    bool isAutoPlay,
    ResourceInfo resources,
    bool skipCardPileVisuals = false)
  {
    choiceContext.PushModel((AbstractModel) this);
    await CombatManager.Instance.WaitForUnpause();
    this.CurrentTarget = target;
    this.CurrentPlayIndex = 0;
    if (isAutoPlay)
    {
      CardPileAddResult cardPileAddResult = await CardPileCmd.Add(this, PileType.Play, skipVisuals: skipCardPileVisuals);
      if (!skipCardPileVisuals)
        await Cmd.CustomScaledWait(0.25f, 0.35f);
    }
    else
      await CardPileCmd.AddDuringManualCardPlay(this);
    ICombatState combatState = this.CombatState;
    CardLocation resultLocation;
    Player originalOwner;
    if (combatState == null)
    {
      combatState = (ICombatState) null;
      resultLocation = new CardLocation();
      originalOwner = (Player) null;
    }
    else
    {
      resultLocation = this.GetResultLocationForCardPlay();
      IEnumerable<AbstractModel> modifiers;
      resultLocation = Hook.ModifyCardPlayResultLocation(combatState, this, isAutoPlay, resources, resultLocation, out modifiers);
      foreach (AbstractModel abstractModel in modifiers)
        await abstractModel.AfterModifyingCardPlayResultLocation(this, resultLocation);
      int playCount = await this.GeneratePlayCount(combatState, target);
      if (this.Owner.Creature.IsDead)
      {
        combatState = (ICombatState) null;
        resultLocation = new CardLocation();
        originalOwner = (Player) null;
      }
      else
      {
        ulong playStartTime = Time.GetTicksMsec();
        CombatManager.Instance.BeginCardOrPotionEffect(this.Owner);
        object obj = (object) null;
        int num = 0;
        try
        {
          for (int i = 0; i < playCount; ++i)
          {
            if (!CombatManager.Instance.IsOverOrEnding)
            {
              this.CurrentPlayIndex = i;
              if (this.Type == CardType.Power)
                await this.PlayPowerCardFlyVfx();
              else if (i > 0)
              {
                NCard onTable = NCard.FindOnTable(this);
                if (onTable != null)
                  await onTable.AnimMultiCardPlay();
              }
              CardPlay cardPlay = new CardPlay()
              {
                Card = this,
                Player = this.Owner,
                Target = target,
                ResultPile = resultLocation.pileType,
                Resources = resources,
                IsAutoPlay = isAutoPlay,
                PlayIndex = i,
                PlayCount = playCount
              };
              await Hook.BeforeCardPlayed(combatState, cardPlay);
              CombatManager.Instance.History.CardPlayStarted(combatState, cardPlay);
              BranchingPlayerChoiceContext choiceContext1 = new BranchingPlayerChoiceContext(LocalContext.NetId.Value, GameActionType.Combat, choiceContext);
              choiceContext1.PushModel((AbstractModel) this);
              Task task = this.OnPlay((PlayerChoiceContext) choiceContext1, cardPlay);
              await choiceContext1.AssignTaskAndWaitForPauseOrCompletion(task);
              if (!this.Owner.Creature.IsDead)
              {
                this.InvokeExecutionFinished();
                if (this.Enchantment != null)
                {
                  await this.Enchantment.OnPlay(choiceContext, cardPlay);
                  if (!this.Owner.Creature.IsDead)
                    this.Enchantment.InvokeExecutionFinished();
                  else
                    goto label_37;
                }
                if (this.Affliction != null)
                {
                  AfflictionModel affliction = this.Affliction;
                  await affliction.OnPlay(choiceContext, target);
                  if (!this.Owner.Creature.IsDead)
                  {
                    affliction.InvokeExecutionFinished();
                    affliction = (AfflictionModel) null;
                  }
                  else
                    goto label_37;
                }
                CombatManager.Instance.History.CardPlayFinished(combatState, cardPlay);
                if (CombatManager.Instance.IsInProgress)
                {
                  await Hook.AfterCardPlayed(combatState, choiceContext, cardPlay);
                  if (this.Owner.Creature.IsDead)
                    goto label_37;
                }
                cardPlay = (CardPlay) null;
                continue;
              }
label_37:
              num = 1;
              break;
            }
            break;
          }
        }
        catch (object ex)
        {
          obj = ex;
        }
        await CombatManager.Instance.EndCardOrPotionEffect(this.Owner);
        object obj1 = obj;
        if (obj1 != null)
        {
          if (!(obj1 is Exception source))
            throw obj1;
          ExceptionDispatchInfo.Capture(source).Throw();
        }
        if (num == 1)
        {
          combatState = (ICombatState) null;
          resultLocation = new CardLocation();
          originalOwner = (Player) null;
        }
        else
        {
          obj = (object) null;
          if (!skipCardPileVisuals)
          {
            float num1 = (float) (Time.GetTicksMsec() - playStartTime) / 1000f;
            await Cmd.CustomScaledWait(0.15f - num1, 0.3f - num1);
          }
          originalOwner = this.Owner;
          if (originalOwner != resultLocation.player && resultLocation.pileType != PileType.None)
            await CardPileCmd.GiveToAnotherPlayer(this, resultLocation.player, resultLocation.pileType, resultLocation.position);
          CardPile pile = this.Pile;
          if ((pile != null ? (pile.Type == PileType.Play ? 1 : 0) : 0) != 0)
          {
            switch (resultLocation.pileType)
            {
              case PileType.None:
                await CardPileCmd.RemoveFromCombat(this, skipCardPileVisuals);
                break;
              case PileType.Exhaust:
                await CardCmd.Exhaust(choiceContext, this, skipVisuals: skipCardPileVisuals);
                break;
              default:
                CardPileAddResult cardPileAddResult = await CardPileCmd.Add(this, resultLocation.pileType, resultLocation.position, skipVisuals: skipCardPileVisuals);
                break;
            }
          }
          await CombatManager.Instance.CheckForEmptyHand(choiceContext, originalOwner);
          if (this.EnergyCost.AfterCardPlayedCleanup())
          {
            Action energyCostChanged = this.EnergyCostChanged;
            if (energyCostChanged != null)
              energyCostChanged();
          }
          if (this._temporaryStarCosts.RemoveAll((Predicate<TemporaryCardCost>) (c => c.ClearsWhenCardIsPlayed)) > 0)
          {
            Action starCostChanged = this.StarCostChanged;
            if (starCostChanged != null)
              starCostChanged();
          }
          this.CurrentTarget = (Creature) null;
          this.CurrentPlayIndex = 0;
          Action played = this.Played;
          if (played != null)
            played();
          choiceContext.PopModel((AbstractModel) this);
          combatState = (ICombatState) null;
          resultLocation = new CardLocation();
          originalOwner = (Player) null;
        }
      }
    }
  }

  protected async Task<int> GeneratePlayCount(ICombatState combatState, Creature? target)
  {
    int playCount = this.GetEnchantedReplayCount() + 1;
    List<AbstractModel> modifyingModels;
    playCount = Hook.ModifyCardPlayCount(combatState, this, playCount, target, out modifyingModels);
    await Hook.AfterModifyingCardPlayCount(combatState, this, (IEnumerable<AbstractModel>) modifyingModels);
    return playCount;
  }

  private async Task PlayPowerCardFlyVfx()
  {
    NCard node = NCard.FindOnTable(this);
    bool flag = false;
    if (node != null)
    {
      foreach (NCardFlyPowerVfx ncardFlyPowerVfx in ((IEnumerable) ((Node) NCombatRoom.Instance.CombatVfxContainer).GetChildren(false)).OfType<NCardFlyPowerVfx>())
      {
        if (ncardFlyPowerVfx.CardNode == node)
        {
          flag = true;
          break;
        }
      }
    }
    if (node == null | flag)
    {
      node = NCard.Create(this);
      if (node != null)
      {
        ((Node) node).CreateTween().Parallel().TweenProperty((GodotObject) node, NodePath.op_Implicit("scale"), Variant.op_Implicit(Vector2.op_Multiply(Vector2.One, 1f)), 0.10000000149011612).From(Variant.op_Implicit(Vector2.Zero)).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 7L);
        NCombatRoom instance = NCombatRoom.Instance;
        if (instance != null)
          ((Node) instance.CombatVfxContainer).AddChildSafely((Node) node);
        node.GlobalPosition = PileType.Play.GetTargetPosition(node);
        node.UpdateVisuals(PileType.Play, CardPreviewMode.Normal);
      }
      await Cmd.CustomScaledWait(0.1f, 0.8f);
    }
    if (node == null)
    {
      node = (NCard) null;
    }
    else
    {
      NCardFlyPowerVfx child = NCardFlyPowerVfx.Create(node);
      NCombatRoom instance = NCombatRoom.Instance;
      if (instance != null)
        ((Node) instance.CombatVfxContainer).AddChildSafely((Node) child);
      TaskHelper.RunSafely(child.PlayAnim());
      float duration = child.GetDuration();
      await Cmd.CustomScaledWait(duration * 0.2f, duration);
      node = (NCard) null;
    }
  }

  protected virtual CardLocation GetResultLocationForCardPlay()
  {
    if (this.IsDupe || this.Type == CardType.Power)
      return new CardLocation(this.Owner, PileType.None, CardPilePosition.Bottom);
    if (!this.ExhaustOnNextPlay && !this.Keywords.Contains(CardKeyword.Exhaust))
      return new CardLocation(this.Owner, PileType.Discard, CardPilePosition.Bottom);
    this.ExhaustOnNextPlay = false;
    return new CardLocation(this.Owner, PileType.Exhaust, CardPilePosition.Bottom);
  }

  public async Task MoveToResultPileWithoutPlaying(PlayerChoiceContext choiceContext)
  {
    CardPile pile = this.Pile;
    if ((pile != null ? (pile.Type != PileType.Play ? 1 : 0) : 1) != 0)
      return;
    if (this.IsDupe)
      await CardPileCmd.RemoveFromCombat(this);
    else if (this.ExhaustOnNextPlay || this.Keywords.Contains(CardKeyword.Exhaust))
    {
      await CardCmd.Exhaust(choiceContext, this);
    }
    else
    {
      CardPileAddResult cardPileAddResult = await CardPileCmd.Add(this, PileType.Discard);
    }
  }

  public void UpgradeInternal()
  {
    this.AssertMutable();
    ++this.CurrentUpgradeLevel;
    this.OnUpgrade();
    this.DynamicVars.RecalculateForUpgradeOrEnchant();
    Action upgraded = this.Upgraded;
    if (upgraded == null)
      return;
    upgraded();
  }

  public void FinalizeUpgradeInternal()
  {
    this.DynamicVars.FinalizeUpgrade();
    this.EnergyCost.FinalizeUpgrade();
    this._wasStarCostJustUpgraded = false;
  }

  public void DowngradeInternal()
  {
    this.AssertMutable();
    this.CurrentUpgradeLevel = 0;
    CardModel mutable = ModelDb.GetById<CardModel>(this.Id).ToMutable();
    this._dynamicVars = mutable.DynamicVars.Clone((AbstractModel) this);
    this.EnergyCost.ResetForDowngrade();
    this._baseStarCost = mutable.CanonicalStarCost;
    this._keywords = ((IEnumerable<CardKeyword>) mutable.GetKeywordsWithSources(KeywordSources.Local)).ToHashSet<CardKeyword>();
    this.AfterDowngraded();
    this.Enchantment?.ModifyCard();
    this.Affliction?.AfterApplied();
    Action upgraded = this.Upgraded;
    if (upgraded == null)
      return;
    upgraded();
  }

  protected virtual void AfterDowngraded()
  {
  }

  public void InvokeDrawn()
  {
    Action drawn = this.Drawn;
    if (drawn == null)
      return;
    drawn();
  }

  public CardModel CreateClone()
  {
    if (this.Pile != null && !this.Pile.Type.IsCombatPile())
      throw new InvalidOperationException("Cannot create a clone of a card that is not in a combat pile.");
    this.AssertMutable();
    CardModel clone = this.CardScope.CloneCard(this);
    clone._cloneOf = this;
    clone.ExhaustOnNextPlay = false;
    return clone;
  }

  public CardModel CreateCloneForPlayer(Player player)
  {
    CardModel clone = this.CreateClone();
    clone._owner = player;
    return clone;
  }

  public void GiveToAnotherPlayer(Player player) => this._owner = player;

  public CardModel CreateDupe(Player newOwner)
  {
    if (this.IsDupe)
      return this.DupeOf.CreateDupe(newOwner);
    this.AssertMutable();
    CardModel cloneForPlayer = this.CreateCloneForPlayer(newOwner);
    cloneForPlayer.IsDupe = true;
    cloneForPlayer.RemoveKeyword(CardKeyword.Exhaust);
    return cloneForPlayer;
  }

  public override bool ShouldReceiveCombatHooks
  {
    get
    {
      CardPile pile = this.Pile;
      return pile != null && pile.IsCombatPile;
    }
  }

  public SerializableCard ToSerializable()
  {
    this.AssertMutable();
    return new SerializableCard()
    {
      Id = this.Id,
      CurrentUpgradeLevel = this.CurrentUpgradeLevel,
      Props = SavedProperties.From((AbstractModel) this),
      Enchantment = this.Enchantment?.ToSerializable(),
      FloorAddedToDeck = this.FloorAddedToDeck
    };
  }

  public static CardModel FromSerializable(SerializableCard save)
  {
    CardModel mutable = SaveUtil.CardOrDeprecated(save.Id).ToMutable();
    save.Props?.Fill((AbstractModel) mutable);
    if (save.FloorAddedToDeck.HasValue)
      mutable.FloorAddedToDeck = save.FloorAddedToDeck;
    mutable.AfterDeserialized();
    if (!(mutable is DeprecatedCard))
    {
      if (save.Enchantment != null)
      {
        mutable.EnchantInternal(EnchantmentModel.FromSerializable(save.Enchantment), (Decimal) save.Enchantment.Amount);
        mutable.Enchantment.ModifyCard();
        mutable.FinalizeUpgradeInternal();
      }
      for (int index = 0; index < save.CurrentUpgradeLevel; ++index)
      {
        mutable.UpgradeInternal();
        mutable.FinalizeUpgradeInternal();
      }
    }
    return mutable;
  }

  public override int CompareTo(AbstractModel? other)
  {
    if (this == other)
      return 0;
    if (other == null)
      return 1;
    int num1 = base.CompareTo(other);
    if (num1 != 0)
      return num1;
    int num2 = this.CurrentUpgradeLevel.CompareTo(((CardModel) other).CurrentUpgradeLevel);
    return num2 != 0 ? num2 : 0;
  }

  public virtual IEnumerable<string> AllPortraitPaths
  {
    get
    {
      return (IEnumerable<string>) new \u003C\u003Ez__ReadOnlySingleElementList<string>(this.PortraitPath);
    }
  }

  public IEnumerable<string> RunAssetPaths => this.ExtraRunAssetPaths;

  protected virtual IEnumerable<string> ExtraRunAssetPaths
  {
    get => (IEnumerable<string>) Array.Empty<string>();
  }

  private enum DescriptionPreviewType
  {
    None,
    Upgrade,
  }
}
