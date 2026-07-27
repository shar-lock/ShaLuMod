// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.EnchantmentModel
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Enchantments;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Saves;
using MegaCrit.Sts2.Core.Saves.Runs;
using MegaCrit.Sts2.Core.ValueProps;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models;

public abstract class EnchantmentModel : AbstractModel
{
  public const string locTable = "enchantments";
  private string? _iconPath;
  private CardModel? _card;
  private int _amount;
  private DynamicVarSet? _dynamicVars;
  private EnchantmentStatus _status;
  private EnchantmentModel _canonicalInstance;

  public LocString Title => new LocString("enchantments", this.Id.Entry + ".title");

  private LocString Description => new LocString("enchantments", this.Id.Entry + ".description");

  private LocString ExtraCardText
  {
    get => new LocString("enchantments", this.Id.Entry + ".extraCardText");
  }

  public virtual bool HasExtraCardText => false;

  public LocString DynamicDescription
  {
    get
    {
      LocString description = this.Description;
      description.Add("Amount", (Decimal) this.Amount);
      DynamicVarSet dynamicVarSet = this.DynamicVars.Clone((AbstractModel) this);
      dynamicVarSet.ClearPreview();
      this._card?.UpdateDynamicVarPreview(CardPreviewMode.None, (Creature) null, dynamicVarSet);
      description.Add("energyPrefix", EnergyIconHelper.GetPrefix((AbstractModel) this));
      dynamicVarSet.AddTo(description);
      return description;
    }
  }

  public LocString? DynamicExtraCardText
  {
    get
    {
      if (!this.HasExtraCardText || this.Status == EnchantmentStatus.Disabled)
        return (LocString) null;
      LocString extraCardText = this.ExtraCardText;
      extraCardText.Add("Amount", (Decimal) this.Amount);
      if (this.IsCanonical)
        extraCardText.Add("TargetType", "None");
      else
        extraCardText.Add("TargetType", this.Card.TargetType.ToString());
      this.DynamicVars.AddTo(extraCardText);
      return extraCardText;
    }
  }

  public static string MissingIconPath
  {
    get => ImageHelper.GetImagePath("enchantments/missing_enchantment.png");
  }

  public string IntendedIconPath
  {
    get => ImageHelper.GetImagePath($"enchantments/{this.Id.Entry.ToLowerInvariant()}.png");
  }

  private string BetaIconPath
  {
    get => ImageHelper.GetImagePath($"enchantments/beta/{this.Id.Entry.ToLowerInvariant()}.png");
  }

  public string IconPath
  {
    get
    {
      if (this._iconPath == null)
        this._iconPath = !ResourceLoader.Exists(this.IntendedIconPath, "") ? (!ResourceLoader.Exists(this.BetaIconPath, "") ? EnchantmentModel.MissingIconPath : this.BetaIconPath) : this.IntendedIconPath;
      return this._iconPath;
    }
  }

  public CompressedTexture2D Icon => PreloadManager.Cache.GetCompressedTexture2D(this.IconPath);

  public virtual bool ShowAmount => false;

  public virtual int DisplayAmount => this.Amount;

  public override bool PreviewOutsideOfCombat => true;

  public override bool ShouldReceiveCombatHooks
  {
    get
    {
      CardModel card = this.Card;
      return card != null && card.ShouldReceiveCombatHooks;
    }
  }

  public virtual bool ShouldStartAtBottomOfDrawPile => false;

  public CardModel Card
  {
    get
    {
      this.AssertMutable();
      return this._card;
    }
    set
    {
      this.AssertMutable();
      value.AssertMutable();
      this._card = this._card == null ? value : throw new InvalidOperationException("Enchantments cannot be moved from one card to another.");
    }
  }

  public bool HasCard => this._card != null;

  public int Amount
  {
    get => this._amount;
    set
    {
      this.AssertMutable();
      this._amount = value;
    }
  }

  [JsonPropertyName("props")]
  [JsonIgnore]
  public SavedProperties? Props { get; set; }

  public virtual bool CanEnchantCardType(CardType cardType) => true;

  public virtual bool IsStackable => false;

  public virtual bool CanEnchant(CardModel card)
  {
    bool flag;
    switch (card.Type)
    {
      case CardType.Status:
      case CardType.Curse:
      case CardType.Quest:
        flag = true;
        break;
      default:
        flag = false;
        break;
    }
    if (flag || !this.CanEnchantCardType(card.Type))
      return false;
    CardPile pile = card.Pile;
    return ((pile != null ? (pile.Type == PileType.Deck ? 1 : 0) : 0) == 0 || !card.Keywords.Contains(CardKeyword.Unplayable)) && (card.Enchantment == null || this.IsStackable && !(card.Enchantment.GetType() != this.GetType()));
  }

  public virtual Task OnPlay(PlayerChoiceContext choiceContext, CardPlay? cardPlay)
  {
    return Task.CompletedTask;
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

  protected virtual IEnumerable<DynamicVar> CanonicalVars
  {
    get => (IEnumerable<DynamicVar>) Array.Empty<DynamicVar>();
  }

  public EnchantmentStatus Status
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

  public event Action? StatusChanged;

  public virtual bool ShouldGlowGold => false;

  public virtual bool ShouldGlowRed => false;

  public EnchantmentModel CanonicalInstance
  {
    get => !this.IsMutable ? this : this._canonicalInstance;
    private set
    {
      this.AssertMutable();
      this._canonicalInstance = value;
    }
  }

  public EnchantmentModel ToMutable()
  {
    this.AssertCanonical();
    EnchantmentModel mutable = (EnchantmentModel) this.MutableClone();
    mutable.CanonicalInstance = this;
    return mutable;
  }

  protected override void DeepCloneFields()
  {
    this._card = (CardModel) null;
    this.StatusChanged = (Action) null;
    this._dynamicVars = this.DynamicVars.Clone((AbstractModel) this);
  }

  public HoverTip HoverTip
  {
    get => new HoverTip(this.Title, this.DynamicDescription, (Texture2D) this.Icon);
  }

  protected virtual IEnumerable<IHoverTip> ExtraHoverTips
  {
    get => (IEnumerable<IHoverTip>) Array.Empty<IHoverTip>();
  }

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

  public void ApplyInternal(CardModel card, Decimal amount)
  {
    if (this.Card != null)
      throw new InvalidOperationException("Can't apply an enchantment to a card when it's already been applied to a different card.");
    this.AssertMutable();
    card.AssertMutable();
    this.Amount = (int) amount;
    this.Card = card;
  }

  public void ClearInternal()
  {
    this.AssertMutable();
    this._card = (CardModel) null;
  }

  public void ModifyCard()
  {
    if (this.Card == null)
      throw new InvalidOperationException("Card must be set at this point.");
    this.OnEnchant();
    this.RecalculateValues();
    this.Card.DynamicVars.RecalculateForUpgradeOrEnchant();
  }

  public virtual void RecalculateValues()
  {
  }

  public SerializableEnchantment ToSerializable()
  {
    this.AssertMutable();
    return new SerializableEnchantment()
    {
      Id = this.Id,
      Props = SavedProperties.From((AbstractModel) this),
      Amount = this.Amount
    };
  }

  public static EnchantmentModel FromSerializable(SerializableEnchantment save)
  {
    EnchantmentModel mutable = SaveUtil.EnchantmentOrDeprecated(save.Id).ToMutable();
    save.Props?.Fill((AbstractModel) mutable);
    mutable.Amount = save.Amount;
    return mutable;
  }

  protected virtual void OnEnchant()
  {
  }

  public virtual Decimal EnchantBlockAdditive(Decimal originalBlock) => 0M;

  public virtual Decimal EnchantBlockMultiplicative(Decimal originalBlock) => 1M;

  public virtual Decimal EnchantDamageAdditive(Decimal originalDamage, ValueProp props) => 0M;

  public virtual Decimal EnchantDamageMultiplicative(Decimal originalDamage, ValueProp props) => 1M;

  public virtual int EnchantPlayCount(int originalPlayCount) => originalPlayCount;
}
