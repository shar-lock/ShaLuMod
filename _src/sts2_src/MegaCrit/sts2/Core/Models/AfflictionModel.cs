// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.AfflictionModel
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Runs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models;

public abstract class AfflictionModel : AbstractModel
{
  public const string locTable = "afflictions";
  private CardModel? _card;
  private int _amount;
  private AfflictionModel _canonicalInstance;

  public event Action<int, int>? AmountChanged;

  public LocString Title => new LocString("afflictions", this.Id.Entry + ".title");

  private LocString Description => new LocString("afflictions", this.Id.Entry + ".description");

  private LocString ExtraCardText => new LocString("afflictions", this.Id.Entry + ".extraCardText");

  public virtual bool HasExtraCardText => false;

  public LocString DynamicDescription
  {
    get
    {
      LocString description = this.Description;
      description.Add("Amount", (Decimal) this.Amount);
      return description;
    }
  }

  public LocString? DynamicExtraCardText
  {
    get
    {
      if (!this.HasExtraCardText)
        return (LocString) null;
      LocString extraCardText = this.ExtraCardText;
      extraCardText.Add("Amount", (Decimal) this.Amount);
      return extraCardText;
    }
  }

  public string OverlayPath
  {
    get
    {
      return SceneHelper.GetScenePath("cards/overlays/afflictions/" + this.Id.Entry.ToLowerInvariant());
    }
  }

  public Control CreateOverlay()
  {
    return PreloadManager.Cache.GetScene(this.OverlayPath).Instantiate<Control>((PackedScene.GenEditState) 0L);
  }

  public bool HasOverlay => ResourceLoader.Exists(this.OverlayPath, "");

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
      this._card = this._card == null ? value : throw new InvalidOperationException("Afflictions cannot be moved from one card to another.");
    }
  }

  public bool HasCard => this._card != null;

  public int Amount
  {
    get => this._amount;
    set
    {
      this.AssertMutable();
      if (this._amount == value)
        return;
      int amount = this._amount;
      this._amount = value;
      if (this._card != null)
        this._card.Owner.PlayerCombatState.RecalculateCardValues();
      Action<int, int> amountChanged = this.AmountChanged;
      if (amountChanged == null)
        return;
      amountChanged(amount, this._amount);
    }
  }

  public ICombatState CombatState => this.Card.CombatState;

  public virtual bool CanAfflictCardType(CardType cardType) => true;

  public virtual bool CanAfflictUnplayableCards => true;

  public virtual bool IsStackable => false;

  public virtual bool CanAfflict(CardModel card)
  {
    return this.CanAfflictCardType(card.Type) && (!card.Keywords.Contains(CardKeyword.Unplayable) || this.CanAfflictUnplayableCards) && (card.Affliction == null || this.IsStackable && !(card.Affliction.GetType() != this.GetType()));
  }

  public virtual void AfterApplied()
  {
  }

  public virtual void BeforeRemoved()
  {
  }

  public virtual Task OnPlay(PlayerChoiceContext choiceContext, Creature? target)
  {
    return Task.CompletedTask;
  }

  public AfflictionModel CanonicalInstance
  {
    get => !this.IsMutable ? this : this._canonicalInstance;
    private set
    {
      this.AssertMutable();
      this._canonicalInstance = value;
    }
  }

  public AfflictionModel ToMutable()
  {
    this.AssertCanonical();
    AfflictionModel mutable = (AfflictionModel) this.MutableClone();
    mutable.CanonicalInstance = this;
    return mutable;
  }

  protected override void AfterCloned()
  {
    base.AfterCloned();
    this.AmountChanged = (Action<int, int>) null;
    this._card = (CardModel) null;
  }

  public override bool ShouldReceiveCombatHooks => true;

  protected virtual IEnumerable<IHoverTip> ExtraHoverTips
  {
    get => (IEnumerable<IHoverTip>) Array.Empty<IHoverTip>();
  }

  public HoverTip HoverTip => new HoverTip(this, this.DynamicDescription);

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

  public IReadOnlyList<CardModel> PickRandomTargets(
    RunRngSet rngSet,
    IEnumerable<CardModel> cards,
    int count)
  {
    List<CardModel> cardModelList = cards.Where<CardModel>(new Func<CardModel, bool>(this.CanAfflict)).ToList<CardModel>().UnstableShuffle<CardModel>(rngSet.CombatCardGeneration);
    cardModelList.RemoveRange(Math.Clamp(cardModelList.Count - 1, 0, count), Math.Max(0, cardModelList.Count - count));
    return (IReadOnlyList<CardModel>) cardModelList;
  }

  public void ClearInternal()
  {
    this.BeforeRemoved();
    this._card = (CardModel) null;
  }
}
