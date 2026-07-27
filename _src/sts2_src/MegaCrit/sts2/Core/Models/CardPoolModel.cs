// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.CardPoolModel
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Modding;
using MegaCrit.Sts2.Core.Unlocks;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable enable
namespace MegaCrit.Sts2.Core.Models;

public abstract class CardPoolModel : AbstractModel, IPoolModel
{
  private CardModel[]? _allCards;
  private HashSet<ModelId>? _allCardIds;

  public abstract string Title { get; }

  public abstract string EnergyColorName { get; }

  public abstract string CardFrameMaterialPath { get; }

  public string FrameMaterialPath
  {
    get => $"res://materials/cards/frames/{this.CardFrameMaterialPath}_mat.tres";
  }

  public Material FrameMaterial => PreloadManager.Cache.GetMaterial(this.FrameMaterialPath);

  public abstract Color DeckEntryCardColor { get; }

  public virtual Color EnergyOutlineColor => new Color("5C5440");

  public string EnergyIconPath => EnergyIconHelper.GetPath(this.EnergyColorName);

  public virtual IEnumerable<CardModel> AllCards
  {
    get
    {
      if (this._allCards == null)
      {
        this._allCards = this.GenerateAllCards();
        this._allCards = ModHelper.ConcatModelsFromMods<CardModel>((IPoolModel) this, (IEnumerable<CardModel>) this._allCards).ToArray<CardModel>();
      }
      return (IEnumerable<CardModel>) this._allCards;
    }
  }

  public IEnumerable<ModelId> AllCardIds
  {
    get
    {
      return (IEnumerable<ModelId>) this._allCardIds ?? (IEnumerable<ModelId>) (this._allCardIds = this.AllCards.Select<CardModel, ModelId>((Func<CardModel, ModelId>) (c => c.Id)).ToHashSet<ModelId>());
    }
  }

  protected abstract CardModel[] GenerateAllCards();

  protected void InvalidateCardCache()
  {
    this._allCards = (CardModel[]) null;
    this._allCardIds = (HashSet<ModelId>) null;
  }

  protected override void DeepCloneFields()
  {
    base.DeepCloneFields();
    this.InvalidateCardCache();
  }

  public IEnumerable<CardModel> GetUnlockedCards(
    UnlockState unlockState,
    CardMultiplayerConstraint multiplayerConstraint)
  {
    List<CardModel> list = this.FilterThroughEpochs(unlockState, this.AllCards).ToList<CardModel>();
    switch (multiplayerConstraint)
    {
      case CardMultiplayerConstraint.MultiplayerOnly:
        list.RemoveAll((Predicate<CardModel>) (c => c.MultiplayerConstraint == CardMultiplayerConstraint.SingleplayerOnly));
        break;
      case CardMultiplayerConstraint.SingleplayerOnly:
        list.RemoveAll((Predicate<CardModel>) (c => c.MultiplayerConstraint == CardMultiplayerConstraint.MultiplayerOnly));
        break;
    }
    return (IEnumerable<CardModel>) list;
  }

  protected virtual IEnumerable<CardModel> FilterThroughEpochs(
    UnlockState unlockState,
    IEnumerable<CardModel> cards)
  {
    return (IEnumerable<CardModel>) cards.ToList<CardModel>();
  }

  public abstract bool IsColorless { get; }

  public override bool ShouldReceiveCombatHooks => false;

  public CardPoolModel ToMutable()
  {
    this.AssertCanonical();
    return (CardPoolModel) this.MutableClone();
  }
}
