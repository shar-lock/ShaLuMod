// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.GameActions.PlayerChoiceResult
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Models;
using MegaCrit.Sts2.Core.Entities.Multiplayer;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.Saves.Runs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

#nullable enable
namespace MegaCrit.Sts2.Core.GameActions;

public class PlayerChoiceResult
{
  private List<CardModel>? _canonicalCards;
  private List<CardModel>? _combatCards;
  private List<CardModel>? _deckCards;
  private List<CardModel>? _mutableCards;
  private List<int>? _indexes;
  private ulong? _playerId;

  public PlayerChoiceType ChoiceType { get; private init; }

  public static PlayerChoiceResult FromCanonicalCard(CardModel? canonicalCard)
  {
    canonicalCard?.AssertCanonical();
    PlayerChoiceResult playerChoiceResult = new PlayerChoiceResult();
    playerChoiceResult.ChoiceType = PlayerChoiceType.CanonicalCard;
    List<CardModel> cardModelList;
    if (canonicalCard == null)
    {
      cardModelList = new List<CardModel>();
    }
    else
    {
      int capacity = 1;
      cardModelList = new List<CardModel>(capacity);
      CollectionsMarshal.SetCount<CardModel>(cardModelList, capacity);
      CollectionsMarshal.AsSpan<CardModel>(cardModelList)[0] = canonicalCard;
    }
    playerChoiceResult._canonicalCards = cardModelList;
    return playerChoiceResult;
  }

  public static PlayerChoiceResult FromMutableCombatCard(CardModel? combatCard)
  {
    combatCard?.AssertMutable();
    CardPile pile = combatCard?.Pile;
    if (pile != null && !pile.IsCombatPile)
      throw new InvalidOperationException("Card must be in a combat pile!");
    PlayerChoiceResult playerChoiceResult = new PlayerChoiceResult();
    playerChoiceResult.ChoiceType = PlayerChoiceType.CombatCard;
    List<CardModel> cardModelList;
    if (combatCard == null)
    {
      cardModelList = new List<CardModel>();
    }
    else
    {
      int capacity = 1;
      cardModelList = new List<CardModel>(capacity);
      CollectionsMarshal.SetCount<CardModel>(cardModelList, capacity);
      CollectionsMarshal.AsSpan<CardModel>(cardModelList)[0] = combatCard;
    }
    playerChoiceResult._combatCards = cardModelList;
    return playerChoiceResult;
  }

  public static PlayerChoiceResult FromMutableDeckCard(CardModel? deckCard)
  {
    deckCard?.AssertMutable();
    bool flag;
    if (deckCard != null)
    {
      CardPile pile = deckCard.Pile;
      if (pile == null || pile.Type != PileType.Deck)
      {
        flag = true;
        goto label_6;
      }
    }
    flag = false;
label_6:
    if (flag)
      throw new InvalidOperationException("Card must be in a deck!");
    PlayerChoiceResult playerChoiceResult = new PlayerChoiceResult();
    playerChoiceResult.ChoiceType = PlayerChoiceType.DeckCard;
    List<CardModel> cardModelList;
    if (deckCard == null)
    {
      cardModelList = new List<CardModel>();
    }
    else
    {
      int capacity = 1;
      cardModelList = new List<CardModel>(capacity);
      CollectionsMarshal.SetCount<CardModel>(cardModelList, capacity);
      CollectionsMarshal.AsSpan<CardModel>(cardModelList)[0] = deckCard;
    }
    playerChoiceResult._deckCards = cardModelList;
    return playerChoiceResult;
  }

  public static PlayerChoiceResult FromCanonicalCards(IEnumerable<CardModel> canonicalCards)
  {
    return new PlayerChoiceResult()
    {
      ChoiceType = PlayerChoiceType.CanonicalCard,
      _canonicalCards = canonicalCards.ToList<CardModel>()
    };
  }

  public static PlayerChoiceResult FromMutableCombatCards(IEnumerable<CardModel> combatCards)
  {
    return new PlayerChoiceResult()
    {
      ChoiceType = PlayerChoiceType.CombatCard,
      _combatCards = combatCards.ToList<CardModel>()
    };
  }

  public static PlayerChoiceResult FromMutableDeckCards(IEnumerable<CardModel> deckCards)
  {
    return new PlayerChoiceResult()
    {
      ChoiceType = PlayerChoiceType.DeckCard,
      _deckCards = deckCards.ToList<CardModel>()
    };
  }

  public static PlayerChoiceResult FromMutableCard(CardModel? mutableCard)
  {
    mutableCard?.AssertMutable();
    PlayerChoiceResult playerChoiceResult = new PlayerChoiceResult();
    playerChoiceResult.ChoiceType = PlayerChoiceType.MutableCard;
    List<CardModel> cardModelList;
    if (mutableCard == null)
    {
      cardModelList = new List<CardModel>();
    }
    else
    {
      int capacity = 1;
      cardModelList = new List<CardModel>(capacity);
      CollectionsMarshal.SetCount<CardModel>(cardModelList, capacity);
      CollectionsMarshal.AsSpan<CardModel>(cardModelList)[0] = mutableCard;
    }
    playerChoiceResult._mutableCards = cardModelList;
    return playerChoiceResult;
  }

  public static PlayerChoiceResult FromMutableCards(IEnumerable<CardModel> mutableCards)
  {
    CardModel[] array = mutableCards.ToArray<CardModel>();
    foreach (CardModel cardModel in array)
    {
      cardModel.AssertMutable();
      if (cardModel.Owner != array[0].Owner)
        throw new InvalidOperationException("All cards passed to FromMutableCards must have the same owner!");
    }
    return new PlayerChoiceResult()
    {
      ChoiceType = PlayerChoiceType.MutableCard,
      _mutableCards = ((IEnumerable<CardModel>) array).ToList<CardModel>()
    };
  }

  public static PlayerChoiceResult FromCards(
    IEnumerable<CardModel> cards,
    PlayerChoiceType choiceType)
  {
    switch (choiceType)
    {
      case PlayerChoiceType.CanonicalCard:
        return new PlayerChoiceResult()
        {
          ChoiceType = choiceType,
          _canonicalCards = cards.ToList<CardModel>()
        };
      case PlayerChoiceType.CombatCard:
        return new PlayerChoiceResult()
        {
          ChoiceType = choiceType,
          _combatCards = cards.ToList<CardModel>()
        };
      case PlayerChoiceType.DeckCard:
        return new PlayerChoiceResult()
        {
          ChoiceType = choiceType,
          _deckCards = cards.ToList<CardModel>()
        };
      case PlayerChoiceType.MutableCard:
        return new PlayerChoiceResult()
        {
          ChoiceType = choiceType,
          _mutableCards = cards.ToList<CardModel>()
        };
      default:
        throw new ArgumentOutOfRangeException(nameof (choiceType), (object) choiceType, (string) null);
    }
  }

  public static PlayerChoiceResult FromPlayerId(ulong? playerId)
  {
    return new PlayerChoiceResult()
    {
      ChoiceType = PlayerChoiceType.Player,
      _playerId = playerId
    };
  }

  public static PlayerChoiceResult FromIndex(int? index)
  {
    PlayerChoiceResult playerChoiceResult = new PlayerChoiceResult();
    playerChoiceResult.ChoiceType = PlayerChoiceType.Index;
    List<int> intList;
    if (!index.HasValue)
    {
      intList = new List<int>();
    }
    else
    {
      int capacity = 1;
      intList = new List<int>(capacity);
      CollectionsMarshal.SetCount<int>(intList, capacity);
      CollectionsMarshal.AsSpan<int>(intList)[0] = index.Value;
    }
    playerChoiceResult._indexes = intList;
    return playerChoiceResult;
  }

  public static PlayerChoiceResult FromIndexes(List<int> indexes)
  {
    return new PlayerChoiceResult()
    {
      ChoiceType = PlayerChoiceType.Index,
      _indexes = indexes
    };
  }

  public CardModel? AsCanonicalCard()
  {
    if (this.ChoiceType != PlayerChoiceType.CanonicalCard)
      throw new InvalidOperationException($"Tried to get canonical card from player choice result of type {this.ChoiceType}!");
    return this._canonicalCards.FirstOrDefault<CardModel>();
  }

  public IEnumerable<CardModel> AsCanonicalCards()
  {
    if (this.ChoiceType != PlayerChoiceType.CanonicalCard)
      throw new InvalidOperationException($"Tried to get canonical cards from player choice result of type {this.ChoiceType}!");
    return (IEnumerable<CardModel>) this._canonicalCards;
  }

  public IEnumerable<CardModel> AsCombatCards()
  {
    if (this.ChoiceType != PlayerChoiceType.CombatCard)
      throw new InvalidOperationException($"Tried to get combat cards from player choice result of type {this.ChoiceType}!");
    return (IEnumerable<CardModel>) this._combatCards;
  }

  public IEnumerable<CardModel> AsDeckCards()
  {
    if (this.ChoiceType != PlayerChoiceType.DeckCard)
      throw new InvalidOperationException($"Tried to get deck cards from player choice result of type {this.ChoiceType}!");
    return (IEnumerable<CardModel>) this._deckCards;
  }

  public CardModel? AsMutableCard()
  {
    if (this.ChoiceType != PlayerChoiceType.MutableCard)
      throw new InvalidOperationException($"Tried to get mutable cards from player choice result of type {this.ChoiceType}!");
    return this._mutableCards.FirstOrDefault<CardModel>();
  }

  public IEnumerable<CardModel> AsMutableCards()
  {
    if (this.ChoiceType != PlayerChoiceType.MutableCard)
      throw new InvalidOperationException($"Tried to get mutable cards from player choice result of type {this.ChoiceType}!");
    return (IEnumerable<CardModel>) this._mutableCards;
  }

  public IEnumerable<CardModel> AsCards(PlayerChoiceType type)
  {
    if (this.ChoiceType != type)
      throw new InvalidOperationException($"Tried to get cards of type {type} from player choice result of type {this.ChoiceType}!");
    switch (type)
    {
      case PlayerChoiceType.CanonicalCard:
        return (IEnumerable<CardModel>) this._canonicalCards;
      case PlayerChoiceType.CombatCard:
        return (IEnumerable<CardModel>) this._combatCards;
      case PlayerChoiceType.DeckCard:
        return (IEnumerable<CardModel>) this._deckCards;
      case PlayerChoiceType.MutableCard:
        return (IEnumerable<CardModel>) this._mutableCards;
      default:
        throw new ArgumentOutOfRangeException(nameof (type), (object) type, (string) null);
    }
  }

  public ulong? AsPlayerId()
  {
    if (this.ChoiceType != PlayerChoiceType.Player)
      throw new InvalidOperationException($"Tried to get player ID from player choice result of type {this.ChoiceType}!");
    return this._playerId;
  }

  public int AsIndex()
  {
    if (this.ChoiceType != PlayerChoiceType.Index)
      throw new InvalidOperationException($"Tried to get index from player choice result of type {this.ChoiceType}!");
    List<int> indexes = this._indexes;
    return indexes == null ? -1 : indexes.FirstOrDefault<int>();
  }

  public int? AsIndexOrNull()
  {
    if (this.ChoiceType != PlayerChoiceType.Index)
      throw new InvalidOperationException($"Tried to get index from player choice result of type {this.ChoiceType}!");
    List<int> indexes = this._indexes;
    return indexes == null || indexes.Count <= 0 ? new int?() : new int?(this._indexes[0]);
  }

  public List<int> AsIndexes()
  {
    if (this.ChoiceType != PlayerChoiceType.Index)
      throw new InvalidOperationException($"Tried to get indexes from player choice result of type {this.ChoiceType}!");
    return this._indexes;
  }

  public static PlayerChoiceResult FromNetData(
    Player sender,
    IPlayerCollection players,
    NetPlayerChoiceResult netData)
  {
    PlayerChoiceResult playerChoiceResult = new PlayerChoiceResult()
    {
      ChoiceType = netData.type
    };
    switch (playerChoiceResult.ChoiceType)
    {
      case PlayerChoiceType.CanonicalCard:
        playerChoiceResult._canonicalCards = netData.canonicalCards;
        break;
      case PlayerChoiceType.CombatCard:
        playerChoiceResult._combatCards = netData.combatCards.Select<NetCombatCard, CardModel>((Func<NetCombatCard, CardModel>) (c => c.ToCardModel())).ToList<CardModel>();
        break;
      case PlayerChoiceType.DeckCard:
        playerChoiceResult._deckCards = netData.deckCards.Select<NetDeckCard, CardModel>((Func<NetDeckCard, CardModel>) (c => c.ToCardModel(sender))).ToList<CardModel>();
        break;
      case PlayerChoiceType.MutableCard:
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        playerChoiceResult._mutableCards = netData.mutableCards.Select<SerializableCard, CardModel>(PlayerChoiceResult.\u003C\u003EO.\u003C0\u003E__FromSerializable ?? (PlayerChoiceResult.\u003C\u003EO.\u003C0\u003E__FromSerializable = new Func<SerializableCard, CardModel>(CardModel.FromSerializable))).ToList<CardModel>();
        if (netData.mutableCardOwner.HasValue)
        {
          using (List<CardModel>.Enumerator enumerator = playerChoiceResult._mutableCards.GetEnumerator())
          {
            while (enumerator.MoveNext())
              enumerator.Current.Owner = players.GetPlayer(netData.mutableCardOwner.Value);
            break;
          }
        }
        break;
      case PlayerChoiceType.Player:
        playerChoiceResult._playerId = netData.playerId;
        break;
      case PlayerChoiceType.Index:
        playerChoiceResult._indexes = netData.indexes;
        break;
      default:
        throw new ArgumentOutOfRangeException();
    }
    return playerChoiceResult;
  }

  public NetPlayerChoiceResult ToNetData()
  {
    NetPlayerChoiceResult netData = new NetPlayerChoiceResult();
    netData.type = this.ChoiceType;
    ref NetPlayerChoiceResult local1 = ref netData;
    List<CardModel> combatCards = this._combatCards;
    // ISSUE: reference to a compiler-generated field
    // ISSUE: reference to a compiler-generated field
    List<NetCombatCard> list1 = combatCards != null ? combatCards.Select<CardModel, NetCombatCard>(PlayerChoiceResult.\u003C\u003EO.\u003C1\u003E__FromModel ?? (PlayerChoiceResult.\u003C\u003EO.\u003C1\u003E__FromModel = new Func<CardModel, NetCombatCard>(NetCombatCard.FromModel))).ToList<NetCombatCard>() : (List<NetCombatCard>) null;
    local1.combatCards = list1;
    ref NetPlayerChoiceResult local2 = ref netData;
    List<CardModel> deckCards = this._deckCards;
    // ISSUE: reference to a compiler-generated field
    // ISSUE: reference to a compiler-generated field
    List<NetDeckCard> list2 = deckCards != null ? deckCards.Select<CardModel, NetDeckCard>(PlayerChoiceResult.\u003C\u003EO.\u003C2\u003E__FromModel ?? (PlayerChoiceResult.\u003C\u003EO.\u003C2\u003E__FromModel = new Func<CardModel, NetDeckCard>(NetDeckCard.FromModel))).ToList<NetDeckCard>() : (List<NetDeckCard>) null;
    local2.deckCards = list2;
    ref NetPlayerChoiceResult local3 = ref netData;
    List<CardModel> canonicalCards = this._canonicalCards;
    List<CardModel> list3 = canonicalCards != null ? canonicalCards.ToList<CardModel>() : (List<CardModel>) null;
    local3.canonicalCards = list3;
    ref NetPlayerChoiceResult local4 = ref netData;
    List<CardModel> mutableCards1 = this._mutableCards;
    List<SerializableCard> list4 = mutableCards1 != null ? mutableCards1.Select<CardModel, SerializableCard>((Func<CardModel, SerializableCard>) (c => c.ToSerializable())).ToList<SerializableCard>() : (List<SerializableCard>) null;
    local4.mutableCards = list4;
    ref NetPlayerChoiceResult local5 = ref netData;
    List<CardModel> mutableCards2 = this._mutableCards;
    ulong? nullable = mutableCards2 != null ? mutableCards2.FirstOrDefault<CardModel>()?.Owner.NetId : new ulong?();
    local5.mutableCardOwner = nullable;
    netData.playerId = this._playerId;
    netData.indexes = this._indexes;
    return netData;
  }

  public override string ToString()
  {
    switch (this.ChoiceType)
    {
      case PlayerChoiceType.CanonicalCard:
        return "PlayerChoiceResult canonical " + string.Join<CardModel>(",", (IEnumerable<CardModel>) (this._canonicalCards ?? new List<CardModel>()));
      case PlayerChoiceType.CombatCard:
        return "PlayerChoiceResult combat " + string.Join<CardModel>(",", (IEnumerable<CardModel>) (this._combatCards ?? new List<CardModel>()));
      case PlayerChoiceType.DeckCard:
        return "PlayerChoiceResult deck " + string.Join<CardModel>(",", (IEnumerable<CardModel>) (this._deckCards ?? new List<CardModel>()));
      case PlayerChoiceType.MutableCard:
        DefaultInterpolatedStringHandler interpolatedStringHandler = new DefaultInterpolatedStringHandler(24, 3);
        interpolatedStringHandler.AppendFormatted(nameof (PlayerChoiceResult));
        interpolatedStringHandler.AppendLiteral(" mutable cards ");
        interpolatedStringHandler.AppendFormatted(string.Join<CardModel>(",", (IEnumerable<CardModel>) (this._mutableCards ?? new List<CardModel>())));
        interpolatedStringHandler.AppendLiteral(", owner: ");
        ref DefaultInterpolatedStringHandler local = ref interpolatedStringHandler;
        List<CardModel> mutableCards = this._mutableCards;
        ulong? nullable = mutableCards != null ? mutableCards.FirstOrDefault<CardModel>()?.Owner.NetId : new ulong?();
        local.AppendFormatted<ulong?>(nullable);
        return interpolatedStringHandler.ToStringAndClear();
      case PlayerChoiceType.Player:
        return $"{nameof (PlayerChoiceResult)} player ID {this._playerId}";
      case PlayerChoiceType.Index:
        return "PlayerChoiceResult indexes " + string.Join<int>(",", (IEnumerable<int>) (this._indexes ?? new List<int>()));
      default:
        throw new ArgumentOutOfRangeException();
    }
  }
}
