// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.PowerModel
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Singleton;
using MegaCrit.Sts2.Core.Platform;
using MegaCrit.Sts2.Core.Runs;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models;

public abstract class PowerModel : AbstractModel
{
  public const string locTable = "powers";
  protected static readonly Color _normalAmountLabelColor = StsColors.cream;
  protected static readonly Color _debuffAmountLabelColor = StsColors.red;
  private string? _resolvedBigIconPath;
  private int _amount;
  private int _amountOnTurnStart;
  private bool _skipNextDurationTick;
  private Creature? _owner;
  private Creature? _applier;
  private Creature? _target;
  private DynamicVarSet? _dynamicVars;
  private object? _internalData;
  private PowerModel _canonicalInstance;

  public virtual LocString Title => new LocString("powers", this.Id.Entry + ".title");

  public virtual LocString Description => new LocString("powers", this.Id.Entry + ".description");

  public LocString SmartDescription
  {
    get
    {
      return !this.HasSmartDescription ? this.Description : new LocString("powers", this.SmartDescriptionLocKey);
    }
  }

  public bool HasSmartDescription => LocString.Exists("powers", this.SmartDescriptionLocKey);

  public LocString RemoteDescription
  {
    get
    {
      return !this.HasRemoteDescription ? this.Description : new LocString("powers", this.RemoteDescriptionLocKey);
    }
  }

  public bool HasRemoteDescription => LocString.Exists("powers", this.RemoteDescriptionLocKey);

  protected virtual string RemoteDescriptionLocKey => this.Id.Entry + ".remoteDescription";

  protected virtual string SmartDescriptionLocKey => this.Id.Entry + ".smartDescription";

  protected LocString SelectionScreenPrompt
  {
    get
    {
      LocString str = new LocString("powers", this.Id.Entry + ".selectionScreenPrompt");
      if (!str.Exists())
        throw new InvalidOperationException($"No selection screen prompt for {this.Id}.");
      this.DynamicVars.AddTo(str);
      str.Add("Amount", (Decimal) this.Amount);
      return str;
    }
  }

  public string PackedIconPath
  {
    get
    {
      return ImageHelper.GetImagePath($"atlases/power_atlas.sprites/{this.Id.Entry.ToLowerInvariant()}.tres");
    }
  }

  private string BigIconPath
  {
    get => ImageHelper.GetImagePath($"powers/{this.Id.Entry.ToLowerInvariant()}.png");
  }

  private string BigBetaIconPath
  {
    get => ImageHelper.GetImagePath($"powers/beta/{this.Id.Entry.ToLowerInvariant()}.png");
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

  public Texture2D BigIcon => PreloadManager.Cache.GetTexture2D(this.ResolvedBigIconPath);

  public string ResolvedBigIconPath
  {
    get
    {
      if (this._resolvedBigIconPath != null)
        return this._resolvedBigIconPath;
      this._resolvedBigIconPath = !ResourceLoader.Exists(this.BigIconPath, "") ? (!ResourceLoader.Exists(this.BigBetaIconPath, "") ? PowerModel.MissingIconPath : this.BigBetaIconPath) : this.BigIconPath;
      return this._resolvedBigIconPath;
    }
  }

  public abstract PowerType Type { get; }

  public virtual PowerInstanceType InstanceType => PowerInstanceType.None;

  public bool IsVisible
  {
    get
    {
      return (this.Target == null || LocalContext.IsMe(this.Target) || this.Target.IsEnemy) && this.IsVisibleInternal;
    }
  }

  protected virtual bool IsVisibleInternal => true;

  public virtual bool ShouldPlayVfx
  {
    get
    {
      Creature owner = this.Owner;
      return owner != null && owner.IsAlive && CombatManager.Instance.IsInProgress && this.IsVisible;
    }
  }

  public void StartPulsing()
  {
    Action pulsingStarted = this.PulsingStarted;
    if (pulsingStarted == null)
      return;
    pulsingStarted();
  }

  public void StopPulsing()
  {
    Action pulsingStopped = this.PulsingStopped;
    if (pulsingStopped == null)
      return;
    pulsingStopped();
  }

  public event Action? PulsingStarted;

  public event Action? PulsingStopped;

  public int Amount
  {
    get => this._amount;
    private set => this.SetAmount(value);
  }

  public int AmountOnTurnStart
  {
    get => this._amountOnTurnStart;
    set
    {
      this.AssertMutable();
      this._amountOnTurnStart = value;
    }
  }

  public virtual int DisplayAmount => this.Amount;

  public virtual Color AmountLabelColor
  {
    get
    {
      return this.GetTypeForAmount((Decimal) this.Amount) != PowerType.Debuff ? PowerModel._normalAmountLabelColor : PowerModel._debuffAmountLabelColor;
    }
  }

  protected void Flash()
  {
    Action<PowerModel> flashed = this.Flashed;
    if (flashed == null)
      return;
    flashed(this);
  }

  protected void InvokeDisplayAmountChanged()
  {
    Action displayAmountChanged = this.DisplayAmountChanged;
    if (displayAmountChanged == null)
      return;
    displayAmountChanged();
  }

  public event Action<PowerModel>? Flashed;

  public event Action? DisplayAmountChanged;

  public event Action? Removed;

  public abstract PowerStackType StackType { get; }

  public virtual bool AllowNegative => false;

  public PowerType TypeForCurrentAmount => this.GetTypeForAmount((Decimal) this.Amount);

  public PowerType GetTypeForAmount(Decimal customAmount)
  {
    if (this.StackType.Equals((object) PowerStackType.Counter) && this.AllowNegative && customAmount < 0M)
      return PowerType.Debuff;
    return !this.AllowNegative && this.Type.Equals((object) PowerType.Debuff) && customAmount < 0M ? PowerType.Buff : this.Type;
  }

  public bool ShouldRemoveDueToAmount()
  {
    if (!this.AllowNegative && this.Amount <= 0)
      return true;
    return this.AllowNegative && this.Amount == 0;
  }

  public bool SkipNextDurationTick
  {
    get => this._skipNextDurationTick;
    set
    {
      this.AssertMutable();
      this._skipNextDurationTick = value;
    }
  }

  public Creature Owner
  {
    get
    {
      this.AssertMutable();
      return this._owner;
    }
    private set
    {
      this.AssertMutable();
      this._owner = this._owner == null || this._owner == value ? value : throw new InvalidOperationException($"Cannot move power {this.Id.Entry} from one owner to another");
    }
  }

  public ICombatState CombatState => this.Owner.CombatState;

  public Creature? Applier
  {
    get => this._applier;
    set
    {
      this.AssertMutable();
      this._applier = value;
    }
  }

  public Creature? Target
  {
    get => this._target;
    set
    {
      this.AssertMutable();
      this._target = value;
    }
  }

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

  public virtual bool ShouldScaleInMultiplayer => false;

  public virtual Decimal GetScaledAmountForMultiplayer(
    ICombatState combatState,
    Creature? applier,
    Decimal amount,
    Creature target,
    CardModel? cardSource)
  {
    return amount * (Decimal) combatState.Players.Count * MultiplayerScalingModel.GetMultiplayerScaling(combatState.Encounter, combatState.RunState.CurrentActIndex);
  }

  protected virtual IEnumerable<DynamicVar> CanonicalVars
  {
    get => (IEnumerable<DynamicVar>) Array.Empty<DynamicVar>();
  }

  protected virtual object? InitInternalData() => (object) null;

  protected T GetInternalData<T>() => (T) this._internalData;

  public HoverTip DumbHoverTip => this.GetDumbHoverTip();

  public HoverTip GetDumbHoverTip(int? amountOverride = null)
  {
    LocString description = this.Description;
    this.AddDumbVariablesToDescription(description, amountOverride);
    return new HoverTip(this, description.GetFormattedText(), false);
  }

  protected virtual IEnumerable<IHoverTip> ExtraHoverTips
  {
    get => (IEnumerable<IHoverTip>) Array.Empty<IHoverTip>();
  }

  public IEnumerable<IHoverTip> HoverTips
  {
    get
    {
      List<IHoverTip> hoverTips = new List<IHoverTip>();
      if (!this.IsVisible)
        return (IEnumerable<IHoverTip>) hoverTips;
      StringBuilder stringBuilder = new StringBuilder();
      bool isSmart = this.HasSmartDescription && this.IsMutable;
      if (isSmart)
      {
        LocString locString = this.SmartDescription;
        if (this.Applier != null && !LocalContext.IsMe(this.Applier) && this.HasRemoteDescription)
          locString = this.RemoteDescription;
        locString.Add("Amount", (Decimal) this.Amount);
        locString.Add("OnPlayer", this.Owner.IsPlayer);
        locString.Add("IsMultiplayer", this.Owner.CombatState.Players.Count > 1);
        locString.Add("PlayerCount", (Decimal) this.Owner.CombatState.Players.Count);
        locString.Add("OwnerName", this.Owner.IsPlayer ? this.Owner.Player.Character.Title : this.Owner.Monster.Title);
        if (this.Applier != null)
        {
          string variable = this.Applier.Monster == null ? (this.Applier.Player == null ? "" : PlatformUtil.GetPlayerName(RunManager.Instance.NetService.Platform, this.Applier.Player.NetId)) : this.Applier.Monster.Title.GetFormattedText();
          locString.Add("ApplierName", variable);
        }
        if (this.Target != null)
        {
          if (this.Target.IsMonster)
            locString.Add("TargetName", this.Target.Monster.Title);
          else
            locString.Add("TargetName", PlatformUtil.GetPlayerName(RunManager.Instance.NetService.Platform, this.Target.Player.NetId));
        }
        this.AddDumbVariablesToDescription(locString);
        this.DynamicVars.AddTo(locString);
        stringBuilder.Append(locString.GetFormattedText());
      }
      else
      {
        LocString description = this.Description;
        this.AddDumbVariablesToDescription(description);
        stringBuilder.Append(description.GetFormattedText());
      }
      hoverTips.Add((IHoverTip) new HoverTip(this, stringBuilder.ToString(), isSmart));
      hoverTips.AddRange(this.ExtraHoverTips);
      return (IEnumerable<IHoverTip>) hoverTips;
    }
  }

  private void AddDumbVariablesToDescription(LocString description, int? amountOverride = null)
  {
    description.Add("Amount", (Decimal) (amountOverride ?? this.Amount));
    description.Add("singleStarIcon", "[img]res://images/packed/sprite_fonts/star_icon.png[/img]");
    description.Add("energyPrefix", EnergyIconHelper.GetPrefix((AbstractModel) this));
  }

  private PowerModel CanonicalInstance
  {
    get => !this.IsMutable ? this : this._canonicalInstance;
    set
    {
      this.AssertMutable();
      this._canonicalInstance = value;
    }
  }

  public void SetAmount(int amount, bool silent = false)
  {
    this.AssertMutable();
    amount = Math.Clamp(amount, -999999999, 999999999);
    int change = amount - this._amount;
    if (change == 0)
      return;
    this._amount = amount;
    Action displayAmountChanged = this.DisplayAmountChanged;
    if (displayAmountChanged != null)
      displayAmountChanged();
    this.Owner.InvokePowerModified(this, change, silent);
  }

  public PowerModel ToMutable(int initialAmount = 0)
  {
    this.AssertCanonical();
    PowerModel mutable = (PowerModel) this.MutableClone();
    mutable.CanonicalInstance = this;
    mutable.Amount = initialAmount;
    return mutable;
  }

  public void ApplyInternal(Creature owner, Decimal amount, bool silent = false)
  {
    if (amount == 0M)
      return;
    this.AssertMutable();
    this.Owner = owner;
    this.SetAmount((int) amount, silent);
    this.Owner.ApplyPowerInternal(this);
  }

  public void RemoveInternal()
  {
    this.AssertMutable();
    Action removed = this.Removed;
    if (removed != null)
      removed();
    this.Owner.RemovePowerInternal(this);
  }

  protected override void DeepCloneFields()
  {
    base.DeepCloneFields();
    this._dynamicVars = this.DynamicVars.Clone((AbstractModel) this);
    this._internalData = this.InitInternalData();
  }

  protected override void AfterCloned()
  {
    base.AfterCloned();
    this.Flashed = (Action<PowerModel>) null;
    this.DisplayAmountChanged = (Action) null;
    this.Removed = (Action) null;
    this.PulsingStarted = (Action) null;
    this.PulsingStopped = (Action) null;
    this._owner = (Creature) null;
  }

  public override bool ShouldReceiveCombatHooks => true;

  public virtual Task BeforeApplied(
    Creature target,
    Decimal amount,
    Creature? applier,
    CardModel? cardSource)
  {
    return Task.CompletedTask;
  }

  public virtual Task AfterApplied(Creature? applier, CardModel? cardSource) => Task.CompletedTask;

  public virtual Task AfterRemoved(Creature oldOwner) => Task.CompletedTask;

  public virtual bool ShouldPowerBeRemovedAfterOwnerDeath() => true;

  public virtual bool ShouldOwnerDeathTriggerFatal() => true;

  public virtual bool OwnerIsSecondaryEnemy => false;
}
