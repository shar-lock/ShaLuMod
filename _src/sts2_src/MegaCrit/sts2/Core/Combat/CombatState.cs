// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Combat.CombatState
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.Map;
using MegaCrit.Sts2.Core.Modding;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Singleton;
using MegaCrit.Sts2.Core.Random;
using MegaCrit.Sts2.Core.Runs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Combat;

public class CombatState : ICombatState, ICardScope
{
  private readonly List<Creature> _allies = new List<Creature>();
  private readonly List<Creature> _enemies = new List<Creature>();
  private uint _nextCreatureId;
  private readonly EncounterModel? _encounter;
  private readonly List<Creature> _escapedCreatures = new List<Creature>();
  private readonly List<CardModel> _allCards = new List<CardModel>();

  public IRunState RunState { get; }

  public event Action<ICombatState>? CreaturesChanged;

  public IReadOnlyList<Creature> Allies => (IReadOnlyList<Creature>) this._allies;

  public IReadOnlyList<Creature> Enemies => (IReadOnlyList<Creature>) this._enemies;

  public IReadOnlyList<Creature> Creatures
  {
    get
    {
      return (IReadOnlyList<Creature>) this._allies.Concat<Creature>((IEnumerable<Creature>) this._enemies).ToList<Creature>();
    }
  }

  public IReadOnlyList<Creature> PlayerCreatures
  {
    get
    {
      return (IReadOnlyList<Creature>) this.Creatures.Where<Creature>((Func<Creature, bool>) (c => c.IsPlayer)).ToList<Creature>();
    }
  }

  public IReadOnlyList<Player> Players
  {
    get
    {
      return (IReadOnlyList<Player>) this.PlayerCreatures.Select<Creature, Player>((Func<Creature, Player>) (c => c.Player)).ToList<Player>();
    }
  }

  public IReadOnlyList<ModifierModel> Modifiers { get; }

  public IReadOnlyList<BadgeModel> BadgeModels { get; }

  public MultiplayerScalingModel? MultiplayerScalingModel { get; private set; }

  public int RoundNumber { get; set; }

  public CombatSide CurrentSide { get; set; }

  public EncounterModel? Encounter
  {
    get => this._encounter;
    private init
    {
      value?.AssertMutable();
      this._encounter = value;
    }
  }

  public IReadOnlyList<Creature> EscapedCreatures
  {
    get => (IReadOnlyList<Creature>) this._escapedCreatures;
  }

  public CombatState(
    EncounterModel? encounter = null,
    IRunState? runState = null,
    IReadOnlyList<ModifierModel>? modifiers = null,
    IReadOnlyList<BadgeModel>? badgeModels = null,
    MultiplayerScalingModel? multiplayerScalingModel = null)
  {
    encounter?.AssertMutable();
    this.Encounter = encounter;
    this.RoundNumber = 1;
    this.CurrentSide = CombatSide.Player;
    this.RunState = runState ?? (IRunState) NullRunState.Instance;
    this.Modifiers = (IReadOnlyList<ModifierModel>) ((object) modifiers ?? (object) Array.Empty<ModifierModel>());
    this.BadgeModels = (IReadOnlyList<BadgeModel>) ((object) badgeModels ?? (object) Array.Empty<BadgeModel>());
    this.MultiplayerScalingModel = multiplayerScalingModel;
  }

  public T CreateCard<T>(Player owner) where T : CardModel
  {
    return (T) this.CreateCard((CardModel) ModelDb.Card<T>(), owner);
  }

  public CardModel CreateCard(CardModel canonicalCard, Player owner)
  {
    CardModel mutable = canonicalCard.ToMutable();
    this.AddCard(mutable, owner);
    mutable.AfterCreated();
    return mutable;
  }

  public CardModel CloneCard(CardModel mutableCard)
  {
    CardModel card = (CardModel) mutableCard.ClonePreservingMutability();
    this.AddCard(card);
    return card;
  }

  public void AddCard(CardModel card, Player owner)
  {
    card.Owner = owner;
    this.AddCard(card);
  }

  public void RemoveCard(CardModel card)
  {
    this._allCards.Remove(card);
    card.Owner = (Player) null;
  }

  public bool ContainsCard(CardModel card) => this._allCards.Contains(card);

  public void AddPlayer(Player player)
  {
    this.AttachCreature(player.Creature);
    this.AddCreature(player.Creature);
  }

  public Creature CreateCreature(MonsterModel monster, CombatSide side, string? slot)
  {
    monster.AssertMutable();
    monster.RunRng = this.RunState.Rng;
    Creature creature = new Creature(monster, side, slot);
    List<Creature> creaturesOnSide = side == CombatSide.Player ? this._allies : this._enemies;
    if (side == CombatSide.Enemy)
    {
      creature.SetUniqueMonsterHpValue((IReadOnlyList<Creature>) creaturesOnSide, this.RunState.Rng.Niche);
      creature.ScaleMonsterHpForMultiplayer(this.Encounter, this.Players.Count, this.RunState.CurrentActIndex);
    }
    this.AttachCreature(creature);
    MonsterModel monsterModel = monster;
    long seed = (long) this.RunState.Rng.Seed;
    MapCoord? currentMapCoord1 = this.RunState.CurrentMapCoord;
    ref MapCoord? local1 = ref currentMapCoord1;
    long col = local1.HasValue ? (long) local1.GetValueOrDefault().col : 0L;
    long num = seed + col;
    MapCoord? currentMapCoord2 = this.RunState.CurrentMapCoord;
    ref MapCoord? local2 = ref currentMapCoord2;
    long row = local2.HasValue ? (long) local2.GetValueOrDefault().row : 0L;
    Rng rng = new Rng((ulong) (num + row) + (ulong) this.RunState.CurrentActIndex + (ulong) creature.CombatId.Value);
    monsterModel.Rng = rng;
    this._encounter?.OnCreatureSpawned(creature);
    return creature;
  }

  private void AttachCreature(Creature creature)
  {
    creature.CombatState = (ICombatState) this;
    creature.CombatId = new uint?(this._nextCreatureId);
    ++this._nextCreatureId;
  }

  public void CreatureEscaped(Creature creature)
  {
    this._escapedCreatures.Add(creature);
    this.RemoveCreature(creature, true);
  }

  public void RemoveCreature(Creature creature, bool unattach = true)
  {
    if (creature.CombatState == null)
      return;
    if (creature.CombatState != this)
      throw new InvalidOperationException("Creature is in a different combat.");
    if (this._enemies.Contains(creature))
      this._enemies.Remove(creature);
    else if (this._allies.Contains(creature))
      this._allies.Remove(creature);
    else
      throw new InvalidOperationException($"Removed creature '{creature}' was not found.");
    if (unattach)
      creature.CombatState = (ICombatState) null;
    Action<ICombatState> creaturesChanged = this.CreaturesChanged;
    if (creaturesChanged == null)
      return;
    creaturesChanged((ICombatState) this);
  }

  public bool ContainsCreature(Creature creature)
  {
    return this._allies.Contains(creature) || this._enemies.Contains(creature);
  }

  public bool ContainsMonster<T>() where T : MonsterModel
  {
    return this._enemies.Any<Creature>((Func<Creature, bool>) (c => c.Monster is T));
  }

  public Creature? GetCreature(uint? combatId)
  {
    return !combatId.HasValue ? (Creature) null : this.Creatures.FirstOrDefault<Creature>((Func<Creature, bool>) (c =>
    {
      uint? combatId1 = c.CombatId;
      uint? nullable = combatId;
      return (int) combatId1.GetValueOrDefault() == (int) nullable.GetValueOrDefault() & combatId1.HasValue == nullable.HasValue;
    }));
  }

  public async Task<Creature?> GetCreatureAsync(uint? combatId, double timeoutSec)
  {
    if (!combatId.HasValue)
      return (Creature) null;
    Creature creature = this.GetCreature(combatId);
    if (creature != null)
      return creature;
    uint? nullable = combatId;
    uint nextCreatureId = this._nextCreatureId;
    if (nullable.GetValueOrDefault() < nextCreatureId & nullable.HasValue)
      return (Creature) null;
    TaskCompletionSource<Creature> completionSource = new TaskCompletionSource<Creature>();
    this.CreaturesChanged += new Action<ICombatState>(OnCreaturesChanged);
    Task timeoutTask = CombatState.GodotTimerTask(timeoutSec);
    Task task = await Task.WhenAny((Task) completionSource.Task, timeoutTask);
    this.CreaturesChanged -= new Action<ICombatState>(OnCreaturesChanged);
    if (task == timeoutTask)
      throw new InvalidOperationException($"Timed out waiting for creature with target index {combatId} to spawn!");
    return await completionSource.Task;

    void OnCreaturesChanged(ICombatState _)
    {
      Creature creature = this.GetCreature(combatId);
      if (creature == null)
        return;
      completionSource.SetResult(creature);
    }
  }

  public IReadOnlyList<Creature> GetCreaturesOnSide(CombatSide side)
  {
    return side != CombatSide.Enemy ? this.Allies : this.Enemies;
  }

  public IReadOnlyList<Creature> CreaturesOnCurrentSide
  {
    get => this.GetCreaturesOnSide(this.CurrentSide);
  }

  public IReadOnlyList<Creature> GetOpponentsOf(Creature creature)
  {
    return this.GetCreaturesOnSide(creature.Side.GetOppositeSide());
  }

  public IReadOnlyList<Creature> GetTeammatesOf(Creature creature)
  {
    return this.GetCreaturesOnSide(creature.Side);
  }

  public IReadOnlyList<Creature> HittableEnemies
  {
    get
    {
      return (IReadOnlyList<Creature>) this.Enemies.Where<Creature>((Func<Creature, bool>) (e => e.IsHittable)).ToList<Creature>();
    }
  }

  public Player? GetPlayer(ulong playerId)
  {
    return this.Players.FirstOrDefault<Player>((Func<Player, bool>) (p => (long) p.NetId == (long) playerId));
  }

  public IEnumerable<AbstractModel> IterateHookListeners()
  {
    List<AbstractModel> abstractModelList = new List<AbstractModel>(this.Players.Count * 50);
    for (int index1 = 0; index1 < this._allies.Count + this._enemies.Count; ++index1)
    {
      Creature creature = index1 < this._allies.Count ? this._allies[index1] : this._enemies[index1 - this._allies.Count];
      abstractModelList.AddRange((IEnumerable<AbstractModel>) creature.Powers);
      Player player = creature.Player;
      if (player == null)
        abstractModelList.Add((AbstractModel) creature.Monster);
      else if (player.IsActiveForHooks)
      {
        IReadOnlyList<RelicModel> relics = player.Relics;
        for (int index2 = 0; index2 < relics.Count; ++index2)
        {
          if (!relics[index2].IsMelted)
            abstractModelList.Add((AbstractModel) relics[index2]);
        }
        IReadOnlyList<PotionModel> potionSlots = player.PotionSlots;
        for (int index3 = 0; index3 < potionSlots.Count; ++index3)
        {
          if (potionSlots[index3] != null)
            abstractModelList.Add((AbstractModel) potionSlots[index3]);
        }
        if (player.PlayerCombatState != null)
        {
          abstractModelList.AddRange((IEnumerable<AbstractModel>) player.PlayerCombatState.OrbQueue.Orbs);
          IReadOnlyList<CardPile> allPiles = player.PlayerCombatState.AllPiles;
          for (int index4 = 0; index4 < allPiles.Count; ++index4)
          {
            IReadOnlyList<CardModel> cards = allPiles[index4].Cards;
            for (int index5 = 0; index5 < cards.Count; ++index5)
            {
              CardModel cardModel = cards[index5];
              abstractModelList.Add((AbstractModel) cardModel);
              if (cardModel.Affliction != null)
                abstractModelList.Add((AbstractModel) cardModel.Affliction);
              if (cardModel.Enchantment != null)
                abstractModelList.Add((AbstractModel) cardModel.Enchantment);
            }
          }
        }
      }
    }
    for (int index = 0; index < this.Modifiers.Count; ++index)
      abstractModelList.Add((AbstractModel) this.Modifiers[index]);
    for (int index = 0; index < this.BadgeModels.Count; ++index)
      abstractModelList.Add((AbstractModel) this.BadgeModels[index]);
    if (this.MultiplayerScalingModel != null)
      abstractModelList.Add((AbstractModel) this.MultiplayerScalingModel);
    foreach (AbstractModel model in abstractModelList)
    {
      if (this.Contains(model))
        yield return model;
    }
    foreach (AbstractModel combatStateSubscriber in ModHelper.IterateAllCombatStateSubscribers(this))
      yield return combatStateSubscriber;
  }

  public void SortEnemiesBySlotName()
  {
    if (this.Encounter == null)
      return;
    this._enemies.Sort((Comparison<Creature>) ((a, b) => this.Encounter.Slots.IndexOf<string>(a.SlotName) - this.Encounter.Slots.IndexOf<string>(b.SlotName)));
  }

  public void SetEnemyIndex(Creature creature, int index)
  {
    if (this.Encounter.Slots.Any<string>())
      throw new InvalidOperationException("Cannot modify turn order of a combat with pre-set slots");
    if (!this.Enemies.Contains<Creature>(creature))
      throw new ArgumentException("Creature must be a valid enemy to change its turn order.");
    this._enemies.Remove(creature);
    this._enemies.Insert(Math.Min(index, this._enemies.Count - 1), creature);
  }

  private void AddCard(CardModel card)
  {
    card.AssertMutable();
    if (card.CombatState != null && card.CombatState != this)
      throw new InvalidOperationException($"Card {card.Id.Entry} combat state is set to a different combat.");
    this._allCards.Add(card);
  }

  public void AddCreature(Creature creature)
  {
    if (creature.CombatState != this)
      throw new InvalidOperationException("Creature was created for a different combat.");
    List<Creature> creatureList = creature.Side == CombatSide.Player ? this._allies : this._enemies;
    if (this.ContainsCreature(creature))
      throw new InvalidOperationException("Creature is already in this combat, but AddCreature was called on it again.");
    creatureList.Add(creature);
    Action<ICombatState> creaturesChanged = this.CreaturesChanged;
    if (creaturesChanged == null)
      return;
    creaturesChanged((ICombatState) this);
  }

  private bool Contains(AbstractModel model)
  {
    switch (model)
    {
      case PowerModel powerModel:
        int num;
        if (powerModel.Owner.CombatState != null)
        {
          Player player = powerModel.Owner.Player;
          num = player != null ? (player.IsActiveForHooks ? 1 : 0) : 1;
        }
        else
          num = 0;
        return num != 0;
      case RelicModel relicModel:
        return !relicModel.HasBeenRemovedFromState && relicModel.Owner.IsActiveForHooks;
      case PotionModel potionModel:
        return !potionModel.HasBeenRemovedFromState && potionModel.Owner.IsActiveForHooks;
      case CardModel cardModel:
        return !cardModel.HasBeenRemovedFromState && cardModel.Owner.IsActiveForHooks;
      case AfflictionModel afflictionModel:
        return afflictionModel.HasCard && !afflictionModel.Card.HasBeenRemovedFromState && afflictionModel.Card.Owner.IsActiveForHooks;
      case EnchantmentModel enchantmentModel:
        return enchantmentModel.HasCard && !enchantmentModel.Card.HasBeenRemovedFromState && enchantmentModel.Card.Owner.IsActiveForHooks;
      case OrbModel orbModel:
        return !orbModel.HasBeenRemovedFromState && orbModel.Owner.IsActiveForHooks;
      case MonsterModel monsterModel:
        return monsterModel.Creature.CombatState != null;
      case AchievementModel _:
        return true;
      case BadgeModel _:
        return true;
      case ModifierModel _:
        return true;
      case MultiplayerScalingModel _:
        return true;
      default:
        throw new ArgumentOutOfRangeException(nameof (model), (object) model, $"Invalid model type {model.GetType()} ({model})");
    }
  }

  private static async Task GodotTimerTask(double timeSec)
  {
    SceneTreeTimer timer = ((SceneTree) Engine.GetMainLoop()).CreateTimer(timeSec, true, false, false);
    Variant[] signal = await ((GodotObject) timer).ToSignal((GodotObject) timer, SceneTreeTimer.SignalName.Timeout);
  }

  public bool IsLiveCombat() => true;
}
