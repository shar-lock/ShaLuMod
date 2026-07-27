// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.RelicModel
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Relics;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.Saves;
using MegaCrit.Sts2.Core.Saves.Runs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models;

public abstract class RelicModel : AbstractModel
{
  private static readonly StringName _isUsed = new StringName("is_used");
  private static readonly StringName _pulse = new StringName("pulse");
  private static readonly StringName _isWaxStr = new StringName("is_wax");
  protected const string _locTable = "relics";
  private string? _resolvedBigIconPath;
  private Player? _owner;
  private bool _isWax;
  private bool _isMelted;
  private DynamicVarSet? _dynamicVars;
  private int _floorAddedToDeck;
  private RelicStatus _status;
  private RelicModel? _canonicalInstance;

  public virtual LocString Title
  {
    get
    {
      LocString variable = new LocString("relics", this.Id.Entry + ".title");
      if (this.IsWax)
      {
        LocString waxRelicPrefix = ToyBox.WaxRelicPrefix;
        waxRelicPrefix.Add(nameof (Title), variable);
        variable = waxRelicPrefix;
      }
      return variable;
    }
  }

  public LocString Flavor => new LocString("relics", this.Id.Entry + ".flavor");

  protected LocString EventDescription
  {
    get => LocString.GetIfExists("relics", this.Id.Entry + ".eventDescription") ?? this.Description;
  }

  private LocString Description => new LocString("relics", this.Id.Entry + ".description");

  protected LocString SelectionScreenPrompt
  {
    get
    {
      LocString str = new LocString("relics", this.Id.Entry + ".selectionScreenPrompt");
      this.DynamicVars.AddTo(str);
      return str;
    }
  }

  public LocString DynamicEventDescription
  {
    get
    {
      LocString eventDescription = this.EventDescription;
      this.DynamicVars.AddTo(eventDescription);
      eventDescription.Add("energyPrefix", EnergyIconHelper.GetPrefix((AbstractModel) this));
      eventDescription.Add("singleStarIcon", "[img]res://images/packed/sprite_fonts/star_icon.png[/img]");
      return eventDescription;
    }
  }

  public LocString DynamicDescription
  {
    get
    {
      LocString description = this.Description;
      this.DynamicVars.AddTo(description);
      string prefix = EnergyIconHelper.GetPrefix((AbstractModel) this);
      description.Add("energyPrefix", prefix);
      description.Add("singleStarIcon", "[img]res://images/packed/sprite_fonts/star_icon.png[/img]");
      foreach (KeyValuePair<string, object> variable in (IEnumerable<KeyValuePair<string, object>>) description.Variables)
      {
        if (variable.Value is EnergyVar energyVar)
          energyVar.ColorPrefix = prefix;
      }
      return description;
    }
  }

  protected LocString? AdditionalRestSiteHealText
  {
    get
    {
      LocString ifExists = LocString.GetIfExists("relics", this.Id.Entry + ".additionalRestSiteHealText");
      if (ifExists != null)
        this.DynamicVars.AddTo(ifExists);
      return ifExists;
    }
  }

  protected virtual string IconBaseName => this.Id.Entry.ToLowerInvariant();

  public virtual string PackedIconPath
  {
    get => ImageHelper.GetImagePath($"atlases/relic_atlas.sprites/{this.IconBaseName}.tres");
  }

  protected virtual string PackedIconOutlinePath
  {
    get
    {
      return ImageHelper.GetImagePath($"atlases/relic_outline_atlas.sprites/{this.IconBaseName}.tres");
    }
  }

  protected virtual string BigIconPath
  {
    get => ImageHelper.GetImagePath($"relics/{this.IconBaseName}.png");
  }

  private string BigBetaIconPath
  {
    get => ImageHelper.GetImagePath($"relics/beta/{this.IconBaseName}.png");
  }

  private static string MissingIconPath => ImageHelper.GetImagePath("powers/missing_power.png");

  public string IconPath => this.PackedIconPath;

  public Texture2D Icon
  {
    get
    {
      return ResourceLoader.Load<Texture2D>(this.PackedIconPath, (string) null, (ResourceLoader.CacheMode) 1L);
    }
  }

  public Texture2D IconOutline
  {
    get
    {
      return ResourceLoader.Load<Texture2D>(this.PackedIconOutlinePath, (string) null, (ResourceLoader.CacheMode) 1L);
    }
  }

  public Texture2D BigIcon => PreloadManager.Cache.GetTexture2D(this.ResolvedBigIconPath);

  private string ResolvedBigIconPath
  {
    get
    {
      if (this._resolvedBigIconPath != null)
        return this._resolvedBigIconPath;
      this._resolvedBigIconPath = !ResourceLoader.Exists(this.BigIconPath, "") ? (!ResourceLoader.Exists(this.BigBetaIconPath, "") ? RelicModel.MissingIconPath : this.BigBetaIconPath) : this.BigIconPath;
      return this._resolvedBigIconPath;
    }
  }

  public abstract RelicRarity Rarity { get; }

  public RelicPoolModel Pool
  {
    get
    {
      return ModelDb.AllRelicPools.First<RelicPoolModel>((Func<RelicPoolModel, bool>) (p => p.AllRelicIds.Contains(this.Id)));
    }
  }

  public bool IsTradable
  {
    get
    {
      if (this.IsUsedUp || this.HasUponPickupEffect || this.IsMelted || this.SpawnsPets)
        return false;
      bool flag;
      switch (this.Rarity)
      {
        case RelicRarity.Starter:
        case RelicRarity.Event:
        case RelicRarity.Ancient:
          flag = true;
          break;
        default:
          flag = false;
          break;
      }
      return !flag;
    }
  }

  public virtual bool IsAllowedInShops => true;

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
      this._owner = this._owner == null || this._owner == value ? value : throw new InvalidOperationException($"Cannot move relic from {this.Id.Entry} one owner to another");
    }
  }

  public virtual bool IsUsedUp => false;

  public virtual bool HasUponPickupEffect => false;

  public virtual bool SpawnsPets => false;

  public virtual bool IsStackable => false;

  [SavedProperty(SerializationCondition.SaveIfNotTypeDefault)]
  public bool IsWax
  {
    get => this._isWax;
    set
    {
      this.AssertMutable();
      this._isWax = value;
    }
  }

  [SavedProperty(SerializationCondition.SaveIfNotTypeDefault)]
  public bool IsMelted
  {
    get => this._isMelted;
    set
    {
      this.AssertMutable();
      this._isMelted = value;
    }
  }

  public virtual bool AddsPet => false;

  public int StackCount { get; private set; } = 1;

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

  public virtual int MerchantCost
  {
    get
    {
      switch (this.Rarity)
      {
        case RelicRarity.None:
          return 1;
        case RelicRarity.Starter:
          return 999999999;
        case RelicRarity.Common:
          return 175;
        case RelicRarity.Uncommon:
          return 225;
        case RelicRarity.Rare:
          return 275;
        case RelicRarity.Shop:
          return 200;
        case RelicRarity.Event:
          return 999999999;
        case RelicRarity.Ancient:
          return 999999999;
        default:
          throw new InvalidOperationException($"Relic {this.Id} has invalid merchant rarity {this.Rarity}.");
      }
    }
  }

  public virtual bool IsAllowed(IRunState runState) => true;

  public virtual bool IsAllowedAtNeow(Player player) => this.IsAllowed(player.RunState);

  protected static bool IsBeforeAct3TreasureChest(IRunState runState)
  {
    int num = runState.Players.Count > 1 ? 38 : 41;
    return runState.TotalFloor < num;
  }

  public int FloorAddedToDeck
  {
    get => this._floorAddedToDeck;
    set
    {
      this.AssertMutable();
      this._floorAddedToDeck = value;
    }
  }

  public RelicStatus Status
  {
    get => this._status;
    set
    {
      this.AssertMutable();
      if (this._status == value)
        return;
      this._status = value;
      Action statusChanged = this.StatusChanged;
      if (statusChanged == null)
        return;
      statusChanged();
    }
  }

  public virtual bool ShowCounter => false;

  public virtual int DisplayAmount => 0;

  public void Flash()
  {
    // ISSUE: object of a compiler-generated type is created
    this.Flash((IEnumerable<Creature>) new \u003C\u003Ez__ReadOnlySingleElementList<Creature>(this.Owner.Creature));
  }

  public void Flash(IEnumerable<Creature> targets)
  {
    Action<RelicModel, IEnumerable<Creature>> flashed = this.Flashed;
    if (flashed == null)
      return;
    flashed(this, targets);
  }

  public virtual string FlashSfx => "event:/sfx/ui/relic_activate_general";

  public event Action<RelicModel, IEnumerable<Creature>>? Flashed;

  public virtual bool ShouldFlashOnPlayer => true;

  protected void InvokeDisplayAmountChanged()
  {
    Action displayAmountChanged = this.DisplayAmountChanged;
    if (displayAmountChanged == null)
      return;
    displayAmountChanged();
  }

  public event Action? DisplayAmountChanged;

  public event Action? StatusChanged;

  public void UpdateTexture(TextureRect texture)
  {
    ShaderMaterial material = (ShaderMaterial) ((CanvasItem) texture).Material;
    material.SetShaderParameter(RelicModel._isWaxStr, Variant.op_Implicit(this.IsWax ? 1 : 0));
    if (this.IsMelted)
      ((CanvasItem) texture).SelfModulate = Colors.DarkRed;
    if (!RunManager.Instance.IsInProgress || this.IsMelted)
    {
      material.SetShaderParameter(RelicModel._pulse, Variant.op_Implicit(0));
      material.SetShaderParameter(RelicModel._isUsed, Variant.op_Implicit(0));
    }
    else
    {
      switch (this.Status)
      {
        case RelicStatus.Normal:
          material.SetShaderParameter(RelicModel._pulse, Variant.op_Implicit(0));
          material.SetShaderParameter(RelicModel._isUsed, Variant.op_Implicit(0));
          break;
        case RelicStatus.Active:
          material.SetShaderParameter(RelicModel._pulse, Variant.op_Implicit(1));
          material.SetShaderParameter(RelicModel._isUsed, Variant.op_Implicit(0));
          break;
        case RelicStatus.Disabled:
          material.SetShaderParameter(RelicModel._pulse, Variant.op_Implicit(0));
          material.SetShaderParameter(RelicModel._isUsed, Variant.op_Implicit(1));
          break;
        default:
          throw new ArgumentOutOfRangeException();
      }
    }
  }

  public HoverTip HoverTip
  {
    get
    {
      LocString description = this.DynamicDescription;
      if (this.IsMelted)
      {
        description = new LocString("gameplay_ui", "RELIC_IS_MELTED");
        description.Add("description", this.DynamicDescription);
      }
      else if (this.IsUsedUp && this.IsMutable)
      {
        description = new LocString("gameplay_ui", "RELIC_USED_UP");
        description.Add("description", this.DynamicDescription);
      }
      HoverTip hoverTip = new HoverTip(this.Title, description);
      hoverTip.SetCanonicalModel((AbstractModel) this.CanonicalInstance);
      return hoverTip;
    }
  }

  protected virtual IEnumerable<IHoverTip> ExtraHoverTips
  {
    get => (IEnumerable<IHoverTip>) Array.Empty<IHoverTip>();
  }

  public IEnumerable<IHoverTip> HoverTipsExcludingRelic => this.ExtraHoverTips;

  public IEnumerable<IHoverTip> HoverTips
  {
    get
    {
      int capacity = 1;
      List<IHoverTip> hoverTipList = new List<IHoverTip>(capacity);
      CollectionsMarshal.SetCount<IHoverTip>(hoverTipList, capacity);
      CollectionsMarshal.AsSpan<IHoverTip>(hoverTipList)[0] = (IHoverTip) this.HoverTip;
      List<IHoverTip> hoverTips = hoverTipList;
      hoverTips.AddRange(this.ExtraHoverTips);
      return (IEnumerable<IHoverTip>) hoverTips;
    }
  }

  public RelicModel CanonicalInstance
  {
    get => !this.IsMutable ? this : this._canonicalInstance;
    private set
    {
      this.AssertMutable();
      this._canonicalInstance = value;
    }
  }

  public bool HasBeenRemovedFromState { get; private set; }

  public RelicModel ToMutable()
  {
    this.AssertCanonical();
    return (RelicModel) this.MutableClone();
  }

  protected override void DeepCloneFields()
  {
    base.DeepCloneFields();
    this._dynamicVars = this.DynamicVars.Clone((AbstractModel) this);
  }

  protected override void AfterCloned()
  {
    base.AfterCloned();
    if (this._canonicalInstance == null)
      this.CanonicalInstance = ModelDb.GetById<RelicModel>(this.Id);
    this.HasBeenRemovedFromState = false;
    this.Flashed = (Action<RelicModel, IEnumerable<Creature>>) null;
    this.DisplayAmountChanged = (Action) null;
    this.StatusChanged = (Action) null;
  }

  public void RemoveInternal() => this.HasBeenRemovedFromState = true;

  public void IncrementStackCount()
  {
    this.AssertMutable();
    if (!this.IsStackable)
      throw new InvalidOperationException($"Cannot increment stack count on {this.Id} because it is not a stackable relic.");
    ++this.StackCount;
  }

  public virtual Task AfterObtained() => Task.CompletedTask;

  public virtual Task AfterRemoved() => Task.CompletedTask;

  public SerializableRelic ToSerializable()
  {
    this.AssertMutable();
    return new SerializableRelic()
    {
      Id = this.Id,
      Props = SavedProperties.From((AbstractModel) this),
      FloorAddedToDeck = new int?(this.FloorAddedToDeck)
    };
  }

  public static RelicModel FromSerializable(SerializableRelic save)
  {
    RelicModel mutable = SaveUtil.RelicOrDeprecated(save.Id).ToMutable();
    save.Props?.Fill((AbstractModel) mutable);
    if (save.FloorAddedToDeck.HasValue)
      mutable.FloorAddedToDeck = save.FloorAddedToDeck.Value;
    return mutable;
  }

  protected void RelicIconChanged() => this._resolvedBigIconPath = (string) null;

  public override bool ShouldReceiveCombatHooks => true;

  protected static LocString L10NLookup(string entryName) => new LocString("relics", entryName);
}
