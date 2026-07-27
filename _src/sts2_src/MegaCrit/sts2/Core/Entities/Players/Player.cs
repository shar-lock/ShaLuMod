// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Entities.Players.Player
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Potions;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.Odds;
using MegaCrit.Sts2.Core.Random;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.Saves;
using MegaCrit.Sts2.Core.Saves.Runs;
using MegaCrit.Sts2.Core.Unlocks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Entities.Players;

public class Player
{
  public const int initialMaxPotionSlotCount = 3;
  private CardPile[]? _runPiles;
  private readonly List<RelicModel> _relics = new List<RelicModel>();
  private readonly List<PotionModel?> _potionSlots = new List<PotionModel>();
  private IRunState _runState = (IRunState) NullRunState.Instance;
  private int _gold;
  private bool _canUseOrRemovePotions = true;

  public event Action<RelicModel>? RelicObtained;

  public event Action<RelicModel>? RelicRemoved;

  public event Action<int>? MaxPotionCountChanged;

  public event Action<PotionModel>? PotionProcured;

  public event Action<PotionModel>? PotionDiscarded;

  public event Action<PotionModel>? UsedPotionRemoved;

  public event Action? AddPotionFailed;

  public event Action? GoldChanged;

  public int MaxPotionCount => this._potionSlots.Count;

  public CharacterModel Character { get; }

  public Creature Creature { get; }

  public ulong NetId { get; }

  public PlayerRngSet PlayerRng { get; private set; }

  public PlayerOddsSet PlayerOdds { get; private set; }

  public RelicGrabBag RelicGrabBag { get; }

  public UnlockState UnlockState { get; }

  public IRunState RunState
  {
    get => this._runState;
    set
    {
      this._runState = this._runState is NullRunState ? value : throw new InvalidOperationException("RunState has already been set.");
    }
  }

  public bool IsActiveForHooks { get; private set; }

  public PlayerCombatState? PlayerCombatState { get; private set; }

  public ExtraPlayerFields ExtraFields { get; private set; } = new ExtraPlayerFields();

  public IReadOnlyList<RelicModel> Relics => (IReadOnlyList<RelicModel>) this._relics;

  public IReadOnlyList<PotionModel?> PotionSlots => (IReadOnlyList<PotionModel>) this._potionSlots;

  public IEnumerable<PotionModel> Potions
  {
    get
    {
      return this._potionSlots.Where<PotionModel>((Func<PotionModel, bool>) (p => p != null)).OfType<PotionModel>();
    }
  }

  public Creature? Osty => this.PlayerCombatState?.GetPet<MegaCrit.Sts2.Core.Models.Monsters.Osty>();

  public bool IsOstyAlive
  {
    get
    {
      Creature osty = this.Osty;
      return osty != null && osty.IsAlive;
    }
  }

  public bool IsOstyMissing => !this.IsOstyAlive;

  public bool HasEventPet()
  {
    return this.Relics.Any<RelicModel>((Func<RelicModel, bool>) (r => r.AddsPet)) || this.Deck.Cards.Any<CardModel>((Func<CardModel, bool>) (c => c is ByrdonisEgg));
  }

  public int Gold
  {
    get => this._gold;
    set
    {
      if (value == this.Gold)
        return;
      this._gold = value;
      Action goldChanged = this.GoldChanged;
      if (goldChanged == null)
        return;
      goldChanged();
    }
  }

  public int MaxAscensionWhenRunStarted { get; }

  public bool HasOpenPotionSlots
  {
    get => this._potionSlots.Any<PotionModel>((Func<PotionModel, bool>) (p => p == null));
  }

  public event Action? CanUseOrRemovePotionsChanged;

  public bool CanUseOrRemovePotions
  {
    get => this._canUseOrRemovePotions;
    set
    {
      this._canUseOrRemovePotions = value;
      Action removePotionsChanged = this.CanUseOrRemovePotionsChanged;
      if (removePotionsChanged == null)
        return;
      removePotionsChanged();
    }
  }

  private bool IsInventoryPopulated
  {
    get
    {
      return this.Deck.Cards.Any<CardModel>() || this.Relics.Any<RelicModel>() || this.Potions.Any<PotionModel>();
    }
  }

  public CardPile Deck { get; } = new CardPile(PileType.Deck);

  public int MaxEnergy { get; set; }

  public List<ModelId> DiscoveredCards { get; set; }

  public List<ModelId> DiscoveredRelics { get; set; }

  public List<ModelId> DiscoveredPotions { get; set; }

  public List<ModelId> DiscoveredEnemies { get; set; }

  public List<string> DiscoveredEpochs { get; set; }

  public int BaseOrbSlotCount { get; set; }

  public IEnumerable<CardPile> Piles
  {
    get
    {
      if (this._runPiles == null)
        this._runPiles = new CardPile[1]{ this.Deck };
      PlayerCombatState playerCombatState = this.PlayerCombatState;
      return ((IEnumerable<CardPile>) ((playerCombatState != null ? (object) playerCombatState.AllPiles : (object) null) ?? (object) Array.Empty<CardPile>())).Concat<CardPile>((IEnumerable<CardPile>) this._runPiles);
    }
  }

  private Player(
    CharacterModel character,
    ulong netId,
    int currentHp,
    int maxHp,
    int maxEnergy,
    int gold,
    int potionSlotCount,
    int orbSlotCount,
    RelicGrabBag sharedRelicGrabBag,
    UnlockState unlockState,
    List<ModelId>? discoveredCards = null,
    List<ModelId>? discoveredEnemies = null,
    List<string>? discoveredEpochs = null,
    List<ModelId>? discoveredPotions = null,
    List<ModelId>? discoveredRelics = null)
  {
    this.RunState = (IRunState) NullRunState.Instance;
    this.Character = character;
    this.NetId = netId;
    this.Creature = new Creature(this, currentHp, maxHp);
    this.MaxEnergy = maxEnergy;
    this.Gold = gold;
    this.SetMaxPotionCountInternal(potionSlotCount);
    this.BaseOrbSlotCount = orbSlotCount;
    this.RelicGrabBag = sharedRelicGrabBag;
    this.UnlockState = unlockState;
    this.PlayerRng = new PlayerRngSet(0UL);
    this.PlayerOdds = new PlayerOddsSet(this.PlayerRng);
    this.DiscoveredCards = discoveredCards ?? new List<ModelId>();
    this.DiscoveredEnemies = discoveredEnemies ?? new List<ModelId>();
    this.DiscoveredEpochs = discoveredEpochs ?? new List<string>();
    this.DiscoveredPotions = discoveredPotions ?? new List<ModelId>();
    this.DiscoveredRelics = discoveredRelics ?? new List<ModelId>();
    this.IsActiveForHooks = this.Creature.IsAlive;
    CharacterStats statsForCharacter = SaveManager.Instance?.Progress.GetStatsForCharacter(this.Character.Id);
    this.MaxAscensionWhenRunStarted = statsForCharacter != null ? statsForCharacter.MaxAscension : 0;
  }

  public static Player CreateForNewRun<T>(UnlockState unlockState, ulong netId) where T : CharacterModel
  {
    return Player.CreateForNewRun((CharacterModel) ModelDb.Character<T>(), unlockState, netId);
  }

  public static Player CreateForNewRun(
    CharacterModel character,
    UnlockState unlockState,
    ulong netId)
  {
    Player forNewRun = new Player(character, netId, character.StartingHp, character.StartingHp, character.MaxEnergy, character.StartingGold, 3, character.BaseOrbSlotCount, new RelicGrabBag(), unlockState);
    forNewRun.PopulateStartingInventory();
    return forNewRun;
  }

  public static Player FromSerializable(SerializablePlayer save)
  {
    Player player = new Player(ModelDb.GetById<CharacterModel>(save.CharacterId), save.NetId, save.CurrentHp, save.MaxHp, save.MaxEnergy, save.Gold, save.MaxPotionSlotCount, save.BaseOrbSlotCount, RelicGrabBag.FromSerializable(save.RelicGrabBag), UnlockState.FromSerializable(save.UnlockState), save.DiscoveredCards.ToList<ModelId>(), save.DiscoveredEnemies.ToList<ModelId>(), save.DiscoveredEpochs.ToList<string>(), save.DiscoveredPotions.ToList<ModelId>(), save.DiscoveredRelics.ToList<ModelId>())
    {
      PlayerRng = PlayerRngSet.FromSerializable(save.Rng)
    };
    player.PlayerOdds = PlayerOddsSet.FromSerializable(save.Odds, player.PlayerRng);
    player.ExtraFields = ExtraPlayerFields.FromSerializable(save.ExtraFields);
    player.LoadInventory(save);
    return player;
  }

  public void InitializeSeed(string seed)
  {
    this.PlayerRng = new PlayerRngSet(StringHelper.GetDeterministicHashCode(seed) + (ulong) this._runState.GetPlayerSlotIndex(this));
    this.PlayerOdds = new PlayerOddsSet(this.PlayerRng);
  }

  private void PopulateStartingInventory()
  {
    if (this.IsInventoryPopulated)
      throw new InvalidOperationException("Inventory is already populated.");
    if (!(this.RunState is NullRunState))
      throw new InvalidOperationException("A player's starting inventory must be populated before being added to a run.");
    this.PopulateStartingDeck();
    this.PopulateStartingRelics();
    foreach (PotionModel potion in this.Character.StartingPotions.Select<PotionModel, PotionModel>((Func<PotionModel, PotionModel>) (p => p.ToMutable())))
      this.AddPotionInternal(potion);
  }

  private void LoadInventory(SerializablePlayer save)
  {
    if (this.IsInventoryPopulated)
      throw new InvalidOperationException("Inventory is already populated.");
    if (!(this.RunState is NullRunState))
      throw new InvalidOperationException("A player's inventory must be loaded before being added to a run.");
    // ISSUE: reference to a compiler-generated field
    // ISSUE: reference to a compiler-generated field
    this.PopulateDeck(save.Deck.Select<SerializableCard, CardModel>(Player.\u003C\u003EO.\u003C0\u003E__FromSerializable ?? (Player.\u003C\u003EO.\u003C0\u003E__FromSerializable = new Func<SerializableCard, CardModel>(CardModel.FromSerializable))));
    this.LoadPotions(save.Potions);
    // ISSUE: reference to a compiler-generated field
    // ISSUE: reference to a compiler-generated field
    this.PopulateRelics(save.Relics.Select<SerializableRelic, RelicModel>(Player.\u003C\u003EO.\u003C1\u003E__FromSerializable ?? (Player.\u003C\u003EO.\u003C1\u003E__FromSerializable = new Func<SerializableRelic, RelicModel>(RelicModel.FromSerializable))));
  }

  public void PopulateRelicGrabBagIfNecessary(Rng rng)
  {
    if (this.RelicGrabBag.IsPopulated)
      return;
    this.RelicGrabBag.Populate(this, rng);
  }

  public SerializablePlayer ToSerializable()
  {
    return new SerializablePlayer()
    {
      CharacterId = this.Character.Id,
      CurrentHp = this.Creature.CurrentHp,
      MaxHp = this.Creature.MaxHp,
      MaxEnergy = this.MaxEnergy,
      MaxPotionSlotCount = this.MaxPotionCount,
      BaseOrbSlotCount = this.BaseOrbSlotCount,
      NetId = this.NetId,
      Gold = this.Gold,
      Rng = this.PlayerRng.ToSerializable(),
      Odds = this.PlayerOdds.ToSerializable(),
      RelicGrabBag = this.RelicGrabBag.ToSerializable(),
      Deck = this.Deck.Cards.Select<CardModel, SerializableCard>((Func<CardModel, SerializableCard>) (c => c.ToSerializable())).ToList<SerializableCard>(),
      Relics = this.Relics.Select<RelicModel, SerializableRelic>((Func<RelicModel, SerializableRelic>) (r => r.ToSerializable())).ToList<SerializableRelic>(),
      Potions = this.PotionSlots.Select<PotionModel, SerializablePotion>((Func<PotionModel, int, SerializablePotion>) ((p, i) => p?.ToSerializable(i))).OfType<SerializablePotion>().ToList<SerializablePotion>(),
      ExtraFields = this.ExtraFields.ToSerializable(),
      UnlockState = this.UnlockState.ToSerializable(),
      DiscoveredCards = this.DiscoveredCards.ToList<ModelId>(),
      DiscoveredEnemies = this.DiscoveredEnemies.ToList<ModelId>(),
      DiscoveredEpochs = this.DiscoveredEpochs.ToList<string>(),
      DiscoveredPotions = this.DiscoveredPotions.ToList<ModelId>(),
      DiscoveredRelics = this.DiscoveredRelics.ToList<ModelId>()
    };
  }

  public void SyncWithSerializedPlayer(SerializablePlayer player)
  {
    if ((long) player.NetId != (long) this.NetId)
      throw new InvalidOperationException($"Tried to sync player that has net ID {this.NetId} with SerializablePlayer that has net ID {player.NetId}!");
    if (player.CharacterId != this.Character.Id)
      throw new InvalidOperationException($"Character changed for player {this.NetId}! This is not allowed");
    this.Creature.SetMaxHpInternal((Decimal) player.MaxHp);
    this.Creature.SetCurrentHpInternal((Decimal) player.CurrentHp);
    this.MaxEnergy = player.MaxEnergy;
    this.Gold = player.Gold;
    this.SetMaxPotionCountInternal(player.MaxPotionSlotCount);
    this.Deck.Clear(true);
    foreach (RelicModel relic in this._relics.ToList<RelicModel>())
      this.RemoveRelicInternal(relic, true);
    foreach (PotionModel potion in this._potionSlots.ToList<PotionModel>())
    {
      if (potion != null)
        this.DiscardPotionInternal(potion, true);
    }
    this.PopulateDeck(player.Deck.Select<SerializableCard, CardModel>((Func<SerializableCard, CardModel>) (c => this.RunState.LoadCard(c, this))), true);
    // ISSUE: reference to a compiler-generated field
    // ISSUE: reference to a compiler-generated field
    this.PopulateRelics(player.Relics.Select<SerializableRelic, RelicModel>(Player.\u003C\u003EO.\u003C1\u003E__FromSerializable ?? (Player.\u003C\u003EO.\u003C1\u003E__FromSerializable = new Func<SerializableRelic, RelicModel>(RelicModel.FromSerializable))), true);
    this.LoadPotions(player.Potions, true);
    this.PlayerRng.LoadFromSerializable(player.Rng);
    this.PlayerOdds.LoadFromSerializable(player.Odds);
    this.RelicGrabBag.LoadFromSerializable(player.RelicGrabBag);
    this.DiscoveredCards = player.DiscoveredCards.ToList<ModelId>();
    this.DiscoveredEnemies = player.DiscoveredEnemies.ToList<ModelId>();
    this.DiscoveredEpochs = player.DiscoveredEpochs.ToList<string>();
    this.DiscoveredPotions = player.DiscoveredPotions.ToList<ModelId>();
    this.DiscoveredRelics = player.DiscoveredRelics.ToList<ModelId>();
    this.ExtraFields = ExtraPlayerFields.FromSerializable(player.ExtraFields);
    this.IsActiveForHooks = this.Creature.IsAlive;
  }

  public void AddRelicInternal(RelicModel relic, int index = -1, bool silent = false)
  {
    relic.AssertMutable();
    relic.Owner = this;
    if (index == -1)
      this._relics.Add(relic);
    else
      this._relics.Insert(index, relic);
    if (relic != null && !relic.IsMelted && relic.ShouldFlashOnPlayer)
      relic.Flashed += new Action<RelicModel, IEnumerable<Creature>>(this.OnRelicFlashed);
    if (silent)
      return;
    Action<RelicModel> relicObtained = this.RelicObtained;
    if (relicObtained == null)
      return;
    relicObtained(relic);
  }

  public void RemoveRelicInternal(RelicModel relic, bool silent = false)
  {
    if (!this._relics.Contains(relic))
      throw new InvalidOperationException($"Player does not have relic {relic.Id}");
    this._relics.Remove(relic);
    relic.RemoveInternal();
    if (relic.ShouldFlashOnPlayer)
      relic.Flashed -= new Action<RelicModel, IEnumerable<Creature>>(this.OnRelicFlashed);
    if (silent)
      return;
    Action<RelicModel> relicRemoved = this.RelicRemoved;
    if (relicRemoved == null)
      return;
    relicRemoved(relic);
  }

  public void MeltRelicInternal(RelicModel relic)
  {
    if (!relic.IsWax)
      throw new InvalidOperationException($"{relic.Id} is not wax.");
    if (relic.IsMelted)
      throw new InvalidOperationException($"{relic.Id} is already melted.");
    if (!this._relics.Contains(relic))
      throw new InvalidOperationException($"Player does not have relic {relic.Id}");
    if (relic.ShouldFlashOnPlayer)
      relic.Flashed -= new Action<RelicModel, IEnumerable<Creature>>(this.OnRelicFlashed);
    relic.IsMelted = true;
    relic.Status = RelicStatus.Disabled;
  }

  public T? GetRelic<T>() where T : RelicModel
  {
    return this.Relics.FirstOrDefault<RelicModel>((Func<RelicModel, bool>) (r => r is T)) as T;
  }

  public RelicModel? GetRelicById(ModelId id)
  {
    return this.Relics.FirstOrDefault<RelicModel>((Func<RelicModel, bool>) (r => r.Id == id));
  }

  public int GetPotionSlotIndex(PotionModel model) => this._potionSlots.IndexOf(model);

  public PotionModel? GetPotionAtSlotIndex(int index)
  {
    if (index < 0 || index >= this._potionSlots.Count)
      throw new IndexOutOfRangeException($"Index {index} is not a valid potion slot index! Player has {this._potionSlots.Count} potion slots");
    return this._potionSlots[index];
  }

  public void AddToMaxPotionCount(int maxPotionCountIncrease)
  {
    this.SetMaxPotionCountInternal(this._potionSlots.Count + maxPotionCountIncrease);
  }

  public void SubtractFromMaxPotionCount(int maxPotionCountDecrease)
  {
    this.SetMaxPotionCountInternal(this._potionSlots.Count - maxPotionCountDecrease);
  }

  private void SetMaxPotionCountInternal(int newMaxPotionCount)
  {
    if (newMaxPotionCount > this._potionSlots.Count)
    {
      for (int count = this._potionSlots.Count; count < newMaxPotionCount; ++count)
        this._potionSlots.Add((PotionModel) null);
      Action<int> potionCountChanged = this.MaxPotionCountChanged;
      if (potionCountChanged == null)
        return;
      potionCountChanged(this.MaxPotionCount);
    }
    else
    {
      if (newMaxPotionCount >= this._potionSlots.Count)
        return;
      for (int index1 = this._potionSlots.Count - 1; index1 >= newMaxPotionCount; --index1)
      {
        if (this._potionSlots[index1] != null)
        {
          int index2 = this._potionSlots.IndexOf((PotionModel) null);
          if (index2 < newMaxPotionCount)
            this._potionSlots[index2] = this._potionSlots[index1];
          else
            this.DiscardPotionInternal(this._potionSlots[index1]);
        }
        this._potionSlots.RemoveAt(index1);
      }
      Action<int> potionCountChanged = this.MaxPotionCountChanged;
      if (potionCountChanged == null)
        return;
      potionCountChanged(this.MaxPotionCount);
    }
  }

  public PotionProcureResult AddPotionInternal(PotionModel potion, int slotIndex = -1, bool silent = false)
  {
    potion.AssertMutable();
    PotionProcureResult potionProcureResult = new PotionProcureResult()
    {
      potion = potion
    };
    if (slotIndex < 0)
      slotIndex = this._potionSlots.IndexOf((PotionModel) null);
    if (slotIndex >= 0)
    {
      if (this._potionSlots[slotIndex] != null)
      {
        Log.Warn($"Tried to add potion {potion} at slot index {slotIndex} which is already filled with potion {this._potionSlots[slotIndex]}!");
        if (!silent)
        {
          Action addPotionFailed = this.AddPotionFailed;
          if (addPotionFailed != null)
            addPotionFailed();
        }
        potionProcureResult.success = false;
        potionProcureResult.failureReason = PotionProcureFailureReason.TooFull;
        return potionProcureResult;
      }
      potion.Owner = this;
      this._potionSlots[slotIndex] = potion;
      if (!silent)
      {
        Action<PotionModel> potionProcured = this.PotionProcured;
        if (potionProcured != null)
          potionProcured(potion);
      }
      potionProcureResult.success = true;
    }
    else
    {
      if (!silent)
      {
        Action addPotionFailed = this.AddPotionFailed;
        if (addPotionFailed != null)
          addPotionFailed();
      }
      potionProcureResult.success = false;
      potionProcureResult.failureReason = PotionProcureFailureReason.TooFull;
    }
    return potionProcureResult;
  }

  public void DiscardPotionInternal(PotionModel potion, bool silent = false)
  {
    this.RemovePotionInternal(potion);
    if (silent)
      return;
    Action<PotionModel> potionDiscarded = this.PotionDiscarded;
    if (potionDiscarded == null)
      return;
    potionDiscarded(potion);
  }

  public void RemoveUsedPotionInternal(PotionModel potion)
  {
    this.RemovePotionInternal(potion);
    Action<PotionModel> usedPotionRemoved = this.UsedPotionRemoved;
    if (usedPotionRemoved == null)
      return;
    usedPotionRemoved(potion);
  }

  private void RemovePotionInternal(PotionModel potion)
  {
    int index = this._potionSlots.IndexOf(potion);
    if (index < 0)
      throw new InvalidOperationException($"Tried to remove potion you don't have: {potion.Id}");
    this._potionSlots[index] = (PotionModel) null;
  }

  private void PopulateStartingDeck()
  {
    List<CardModel> cards = new List<CardModel>();
    foreach (CardModel cardModel in this.Character.StartingDeck)
    {
      CardModel mutable = cardModel.ToMutable();
      mutable.FloorAddedToDeck = new int?(1);
      cards.Add(mutable);
    }
    this.PopulateDeck((IEnumerable<CardModel>) cards);
  }

  private void PopulateDeck(IEnumerable<CardModel> cards, bool silent = false)
  {
    if (this.Deck.Cards.Any<CardModel>())
      throw new InvalidOperationException("Deck has already been populated.");
    foreach (CardModel card in cards)
      this.Deck.AddInternal(card, silent: silent);
  }

  private void PopulateStartingRelics()
  {
    List<RelicModel> list = this.Character.StartingRelics.Select<RelicModel, RelicModel>((Func<RelicModel, RelicModel>) (r => r.ToMutable())).ToList<RelicModel>();
    foreach (RelicModel relic in list)
    {
      relic.FloorAddedToDeck = 1;
      SaveManager.Instance.MarkRelicAsSeen(relic);
    }
    this.PopulateRelics((IEnumerable<RelicModel>) list);
  }

  private void PopulateRelics(IEnumerable<RelicModel> relics, bool silent = false)
  {
    if (this.Relics.Any<RelicModel>())
      throw new InvalidOperationException("Relics have already been populated.");
    foreach (RelicModel relic in relics)
      this.AddRelicInternal(relic, silent: silent);
  }

  private void LoadPotions(List<SerializablePotion> serializablePotions, bool silent = false)
  {
    if (this.Potions.Any<PotionModel>())
      throw new InvalidOperationException("Potions have already been populated.");
    foreach (SerializablePotion serializablePotion in serializablePotions)
      this.AddPotionInternal(PotionModel.FromSerializable(serializablePotion), serializablePotion.SlotIndex, silent);
  }

  public void ResetCombatState() => this.PlayerCombatState = new PlayerCombatState(this);

  public void PopulateCombatState(Rng rng, CombatState state)
  {
    foreach (CardModel mutableCard in this.Deck.Cards.ToList<CardModel>())
    {
      CardModel card = state.CloneCard(mutableCard);
      card.DeckVersion = mutableCard;
      this.PlayerCombatState.DrawPile.AddInternal(card);
    }
    this.PlayerCombatState.DrawPile.RandomizeOrderInternal(this, rng, state);
  }

  public async Task ReviveBeforeCombatEnd()
  {
    if (!this.Creature.IsDead)
      return;
    await CreatureCmd.Heal(this.Creature, 1M);
  }

  public void AfterCombatEnd()
  {
    this.Creature.RemoveAllPowersInternalExcept();
    this.PlayerCombatState?.AfterCombatEnd();
    this.Creature.LoseBlockInternal((Decimal) this.Creature.Block);
  }

  private void OnRelicFlashed(RelicModel relic, IEnumerable<Creature> targets)
  {
    SfxCmd.Play(relic.FlashSfx);
    foreach (Creature target in targets)
    {
      Control vfxContainer = target.GetVfxContainer();
      if (vfxContainer != null)
        ((Node) vfxContainer).AddChildSafely((Node) NRelicFlashVfx.Create(relic, target));
    }
  }

  public void OnSideSwitch()
  {
  }

  public void DeactivateHooks() => this.IsActiveForHooks = false;

  public void ActivateHooks() => this.IsActiveForHooks = true;
}
