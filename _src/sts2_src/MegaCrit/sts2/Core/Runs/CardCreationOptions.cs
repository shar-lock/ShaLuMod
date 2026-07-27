// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Runs.CardCreationOptions
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Random;
using MegaCrit.Sts2.Core.Rooms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

#nullable enable
namespace MegaCrit.Sts2.Core.Runs;

public record CardCreationOptions
{
  private readonly List<CardPoolModel> _cardPools = new List<CardPoolModel>();

  public IReadOnlyCollection<CardPoolModel> CardPools
  {
    get => (IReadOnlyCollection<CardPoolModel>) this._cardPools;
  }

  public Func<CardModel, bool>? CardPoolFilter { get; private set; }

  public CardCreationSource Source { get; private set; }

  public CardRarityOddsType RarityOdds { get; private set; }

  public CardCreationFlags Flags { get; private set; }

  public Rng? RngOverride { get; private set; }

  public CardCreationOptions(
    IEnumerable<CardPoolModel> cardPools,
    CardCreationSource source,
    CardRarityOddsType rarityOdds,
    Func<CardModel, bool>? cardPoolFilter = null)
  {
    this._cardPools.AddRange(cardPools);
    this.Source = source;
    this.RarityOdds = rarityOdds;
    this.CardPoolFilter = cardPoolFilter;
  }

  public static CardCreationOptions ForRoom(Player player, RoomType roomType)
  {
    CardCreationSource cardCreationSource;
    switch (roomType)
    {
      case RoomType.Monster:
      case RoomType.Elite:
      case RoomType.Boss:
        cardCreationSource = CardCreationSource.Encounter;
        break;
      case RoomType.Shop:
        cardCreationSource = CardCreationSource.Shop;
        break;
      case RoomType.Event:
        throw new InvalidOperationException("ForRoom should not be used in event rooms");
      default:
        cardCreationSource = CardCreationSource.Other;
        break;
    }
    CardCreationSource source = cardCreationSource;
    CardRarityOddsType cardRarityOddsType;
    switch (roomType)
    {
      case RoomType.Monster:
        cardRarityOddsType = CardRarityOddsType.RegularEncounter;
        break;
      case RoomType.Elite:
        cardRarityOddsType = CardRarityOddsType.EliteEncounter;
        break;
      case RoomType.Boss:
        cardRarityOddsType = CardRarityOddsType.BossEncounter;
        break;
      case RoomType.Shop:
        cardRarityOddsType = CardRarityOddsType.Shop;
        break;
      default:
        cardRarityOddsType = CardRarityOddsType.RegularEncounter;
        break;
    }
    CardRarityOddsType rarityOdds = cardRarityOddsType;
    // ISSUE: object of a compiler-generated type is created
    return new CardCreationOptions((IEnumerable<CardPoolModel>) new \u003C\u003Ez__ReadOnlySingleElementList<CardPoolModel>(player.Character.CardPool), source, rarityOdds);
  }

  public static CardCreationOptions ForNonCombatWithDefaultOdds(
    IEnumerable<CardPoolModel> cardPools,
    Func<CardModel, bool>? cardPoolFilter = null)
  {
    return new CardCreationOptions(cardPools, CardCreationSource.Other, CardRarityOddsType.RegularEncounter, cardPoolFilter).WithFlags(CardCreationFlags.NoUpgradeRoll);
  }

  public static CardCreationOptions ForNonCombatWithUniformOdds(
    IEnumerable<CardPoolModel> cardPools,
    Func<CardModel, bool>? cardPoolFilter = null)
  {
    return new CardCreationOptions(cardPools, CardCreationSource.Other, CardRarityOddsType.Uniform, cardPoolFilter).WithFlags(CardCreationFlags.NoUpgradeRoll);
  }

  public IEnumerable<CardModel> GetPossibleCards(Player player)
  {
    return this.CardPools.SelectMany<CardPoolModel, CardModel>((Func<CardPoolModel, IEnumerable<CardModel>>) (p => p.GetUnlockedCards(player.UnlockState, player.RunState.CardMultiplayerConstraint))).Where<CardModel>((Func<CardModel, bool>) (c => this.CardPoolFilter == null || this.CardPoolFilter(c)));
  }

  public CardCreationOptions WithCardPools(IEnumerable<CardPoolModel> pools)
  {
    List<CardPoolModel> list = pools.ToList<CardPoolModel>();
    this._cardPools.Clear();
    this._cardPools.AddRange((IEnumerable<CardPoolModel>) list);
    return this;
  }

  public CardCreationOptions WithFilter(Func<CardModel, bool> filter)
  {
    this.CardPoolFilter = filter;
    return this;
  }

  public CardCreationOptions WithFlags(CardCreationFlags flag)
  {
    this.Flags |= flag;
    return this;
  }

  public CardCreationOptions WithRarityOdds(CardRarityOddsType rarityOdds)
  {
    this.RarityOdds = rarityOdds;
    return this;
  }

  public CardCreationOptions WithRngOverride(Rng rng)
  {
    this.RngOverride = rng;
    return this;
  }

  public CardRarity? TryGetSingleRarityInPool()
  {
    List<CardModel> list = this.CardPools.SelectMany<CardPoolModel, CardModel>((Func<CardPoolModel, IEnumerable<CardModel>>) (c => c.AllCards)).ToList<CardModel>();
    if (this.CardPoolFilter != null)
      list = list.Where<CardModel>((Func<CardModel, bool>) (c => this.CardPoolFilter(c))).ToList<CardModel>();
    CardModel first = list.FirstOrDefault<CardModel>();
    return first != null && list.All<CardModel>((Func<CardModel, bool>) (c => c.Rarity == first.Rarity)) ? new CardRarity?(first.Rarity) : new CardRarity?();
  }

  [CompilerGenerated]
  protected virtual bool PrintMembers(StringBuilder builder)
  {
    RuntimeHelpers.EnsureSufficientExecutionStack();
    builder.Append("CardPools = ");
    builder.Append((object) this.CardPools);
    builder.Append(", CardPoolFilter = ");
    builder.Append((object) this.CardPoolFilter);
    builder.Append(", Source = ");
    builder.Append(this.Source.ToString());
    builder.Append(", RarityOdds = ");
    builder.Append(this.RarityOdds.ToString());
    builder.Append(", Flags = ");
    builder.Append(this.Flags.ToString());
    builder.Append(", RngOverride = ");
    builder.Append((object) this.RngOverride);
    return true;
  }

  [CompilerGenerated]
  public override int GetHashCode()
  {
    // ISSUE: reference to a compiler-generated field
    // ISSUE: reference to a compiler-generated field
    // ISSUE: reference to a compiler-generated field
    // ISSUE: reference to a compiler-generated field
    // ISSUE: reference to a compiler-generated field
    return (((((EqualityComparer<Type>.Default.GetHashCode(this.EqualityContract) * -1521134295 + EqualityComparer<List<CardPoolModel>>.Default.GetHashCode(this._cardPools)) * -1521134295 + EqualityComparer<Func<CardModel, bool>>.Default.GetHashCode(this.\u003CCardPoolFilter\u003Ek__BackingField)) * -1521134295 + EqualityComparer<CardCreationSource>.Default.GetHashCode(this.\u003CSource\u003Ek__BackingField)) * -1521134295 + EqualityComparer<CardRarityOddsType>.Default.GetHashCode(this.\u003CRarityOdds\u003Ek__BackingField)) * -1521134295 + EqualityComparer<CardCreationFlags>.Default.GetHashCode(this.\u003CFlags\u003Ek__BackingField)) * -1521134295 + EqualityComparer<Rng>.Default.GetHashCode(this.\u003CRngOverride\u003Ek__BackingField);
  }

  [CompilerGenerated]
  public virtual bool Equals(CardCreationOptions? other)
  {
    if ((object) this == (object) other)
      return true;
    // ISSUE: reference to a compiler-generated field
    // ISSUE: reference to a compiler-generated field
    // ISSUE: reference to a compiler-generated field
    // ISSUE: reference to a compiler-generated field
    // ISSUE: reference to a compiler-generated field
    // ISSUE: reference to a compiler-generated field
    // ISSUE: reference to a compiler-generated field
    // ISSUE: reference to a compiler-generated field
    // ISSUE: reference to a compiler-generated field
    // ISSUE: reference to a compiler-generated field
    return (object) other != null && this.EqualityContract == other.EqualityContract && EqualityComparer<List<CardPoolModel>>.Default.Equals(this._cardPools, other._cardPools) && EqualityComparer<Func<CardModel, bool>>.Default.Equals(this.\u003CCardPoolFilter\u003Ek__BackingField, other.\u003CCardPoolFilter\u003Ek__BackingField) && EqualityComparer<CardCreationSource>.Default.Equals(this.\u003CSource\u003Ek__BackingField, other.\u003CSource\u003Ek__BackingField) && EqualityComparer<CardRarityOddsType>.Default.Equals(this.\u003CRarityOdds\u003Ek__BackingField, other.\u003CRarityOdds\u003Ek__BackingField) && EqualityComparer<CardCreationFlags>.Default.Equals(this.\u003CFlags\u003Ek__BackingField, other.\u003CFlags\u003Ek__BackingField) && EqualityComparer<Rng>.Default.Equals(this.\u003CRngOverride\u003Ek__BackingField, other.\u003CRngOverride\u003Ek__BackingField);
  }

  [CompilerGenerated]
  protected CardCreationOptions(CardCreationOptions original)
  {
    this._cardPools = original._cardPools;
    // ISSUE: reference to a compiler-generated field
    // ISSUE: reference to a compiler-generated field
    this.\u003CCardPoolFilter\u003Ek__BackingField = original.\u003CCardPoolFilter\u003Ek__BackingField;
    // ISSUE: reference to a compiler-generated field
    // ISSUE: reference to a compiler-generated field
    this.\u003CSource\u003Ek__BackingField = original.\u003CSource\u003Ek__BackingField;
    // ISSUE: reference to a compiler-generated field
    // ISSUE: reference to a compiler-generated field
    this.\u003CRarityOdds\u003Ek__BackingField = original.\u003CRarityOdds\u003Ek__BackingField;
    // ISSUE: reference to a compiler-generated field
    // ISSUE: reference to a compiler-generated field
    this.\u003CFlags\u003Ek__BackingField = original.\u003CFlags\u003Ek__BackingField;
    // ISSUE: reference to a compiler-generated field
    // ISSUE: reference to a compiler-generated field
    this.\u003CRngOverride\u003Ek__BackingField = original.\u003CRngOverride\u003Ek__BackingField;
  }
}
