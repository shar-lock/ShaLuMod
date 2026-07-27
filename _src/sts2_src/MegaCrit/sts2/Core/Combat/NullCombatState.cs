// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Combat.NullCombatState
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Singleton;
using MegaCrit.Sts2.Core.Runs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Combat;

public class NullCombatState : ICombatState
{
  public static NullCombatState Instance { get; } = new NullCombatState();

  public IRunState RunState => (IRunState) NullRunState.Instance;

  public event Action<ICombatState>? CreaturesChanged;

  public IReadOnlyList<Creature> Allies { get; } = (IReadOnlyList<Creature>) Array.Empty<Creature>();

  public IReadOnlyList<Creature> Enemies { get; } = (IReadOnlyList<Creature>) Array.Empty<Creature>();

  public IReadOnlyList<Creature> Creatures { get; } = (IReadOnlyList<Creature>) Array.Empty<Creature>();

  public IReadOnlyList<Creature> PlayerCreatures { get; } = (IReadOnlyList<Creature>) Array.Empty<Creature>();

  public IReadOnlyList<Player> Players { get; } = (IReadOnlyList<Player>) Array.Empty<Player>();

  public IReadOnlyList<ModifierModel> Modifiers { get; } = (IReadOnlyList<ModifierModel>) Array.Empty<ModifierModel>();

  public MultiplayerScalingModel? MultiplayerScalingModel => (MultiplayerScalingModel) null;

  public int RoundNumber { get; set; } = 1;

  public CombatSide CurrentSide { get; set; }

  public EncounterModel? Encounter => (EncounterModel) null;

  public IReadOnlyList<Creature> EscapedCreatures
  {
    get => (IReadOnlyList<Creature>) Array.Empty<Creature>();
  }

  public T CreateCard<T>(Player owner) where T : CardModel => throw new NotImplementedException();

  public CardModel CreateCard(CardModel canonicalCard, Player owner)
  {
    throw new NotImplementedException();
  }

  public CardModel CloneCard(CardModel mutableCard) => throw new NotImplementedException();

  public void AddCard(CardModel card, Player owner)
  {
  }

  public void RemoveCard(CardModel card)
  {
  }

  public bool ContainsCard(CardModel card) => false;

  public void AddPlayer(Player player)
  {
  }

  public Creature CreateCreature(MonsterModel monster, CombatSide side, string? slot)
  {
    return new Creature(monster, side, slot);
  }

  public void CreatureEscaped(Creature creature)
  {
  }

  public void RemoveCreature(Creature creature, bool unattach = true)
  {
  }

  public bool ContainsCreature(Creature creature) => false;

  public bool ContainsMonster<T>() where T : MonsterModel => false;

  public Creature? GetCreature(uint? combatId) => (Creature) null;

  public Task<Creature?> GetCreatureAsync(uint? combatId, double timeoutSec)
  {
    return Task.FromResult<Creature>((Creature) null);
  }

  public IReadOnlyList<Creature> CreaturesOnCurrentSide { get; } = (IReadOnlyList<Creature>) Array.Empty<Creature>();

  public IReadOnlyList<Creature> HittableEnemies { get; } = (IReadOnlyList<Creature>) Array.Empty<Creature>();

  public IReadOnlyList<Creature> GetCreaturesOnSide(CombatSide side) => this.CreaturesOnCurrentSide;

  public IReadOnlyList<Creature> GetOpponentsOf(Creature creature)
  {
    return (IReadOnlyList<Creature>) Array.Empty<Creature>();
  }

  public IReadOnlyList<Creature> GetTeammatesOf(Creature creature)
  {
    return (IReadOnlyList<Creature>) Array.Empty<Creature>();
  }

  public Player? GetPlayer(ulong playerId) => (Player) null;

  public IEnumerable<AbstractModel> IterateHookListeners()
  {
    return (IEnumerable<AbstractModel>) Array.Empty<AbstractModel>();
  }

  public void SortEnemiesBySlotName()
  {
  }

  public void SetEnemyIndex(Creature creature, int index)
  {
  }

  public void AddCreature(Creature creature)
  {
  }

  public bool IsLiveCombat() => false;
}
