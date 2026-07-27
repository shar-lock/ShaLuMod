// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Entities.Creatures.Creature
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Hooks;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Singleton;
using MegaCrit.Sts2.Core.MonsterMoves.Intents;
using MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Screens.Bestiary;
using MegaCrit.Sts2.Core.Platform;
using MegaCrit.Sts2.Core.Random;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.TestSupport;
using MegaCrit.Sts2.Core.ValueProps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Entities.Creatures;

public class Creature
{
  private int _block;
  private int _currentHp;
  private int _maxHp;
  private readonly List<PowerModel> _powers = new List<PowerModel>();
  private Player? _petOwner;

  public event Action<int, int>? BlockChanged;

  public event Action<int, int>? CurrentHpChanged;

  public event Action<int, int>? MaxHpChanged;

  public event Action<PowerModel>? PowerApplied;

  public event Action<PowerModel, int, bool>? PowerIncreased;

  public event Action<PowerModel, bool>? PowerDecreased;

  public event Action<PowerModel>? PowerRemoved;

  public event Action<Creature>? Died;

  public event Action<Creature>? Revived;

  public int Block
  {
    get => this._block;
    private set
    {
      if (value < 0)
        throw new ArgumentException("Block must be positive", nameof (value));
      if (this._block == value)
        return;
      int block = this._block;
      this._block = value;
      Action<int, int> blockChanged = this.BlockChanged;
      if (blockChanged == null)
        return;
      blockChanged(block, this._block);
    }
  }

  public int CurrentHp
  {
    get => this._currentHp;
    private set
    {
      if (value < 0)
        throw new ArgumentException("Current HP must be positive", nameof (value));
      if (this._currentHp == value)
        return;
      int currentHp = this._currentHp;
      this._currentHp = value;
      Action<int, int> currentHpChanged = this.CurrentHpChanged;
      if (currentHpChanged == null)
        return;
      currentHpChanged(currentHp, this._currentHp);
    }
  }

  public int MaxHp
  {
    get => this._maxHp;
    private set
    {
      if (this._maxHp == value)
        return;
      int maxHp = this._maxHp;
      this._maxHp = value;
      Action<int, int> maxHpChanged = this.MaxHpChanged;
      if (maxHpChanged == null)
        return;
      maxHpChanged(maxHp, this._maxHp);
    }
  }

  public int? MonsterMaxHpBeforeModification { get; private set; }

  public uint? CombatId { get; set; }

  public MonsterModel? Monster { get; }

  public Player? Player { get; }

  public ModelId ModelId => !this.IsPlayer ? this.Monster.Id : this.Player.Character.Id;

  public CombatSide Side { get; }

  public ICombatState? CombatState { get; set; }

  public string Name
  {
    get
    {
      if (this.IsMonster)
        return this.Monster.Title.GetFormattedText();
      return RunManager.Instance.IsSingleplayerOrFakeMultiplayer ? this.Player.Character.Title.GetFormattedText() : PlatformUtil.GetPlayerNameRaw(RunManager.Instance.NetService.Platform, this.Player.NetId);
    }
  }

  public string LogName
  {
    get
    {
      if (this.IsMonster)
        return this.Monster.Title.GetFormattedText();
      return RunManager.Instance.IsSingleplayerOrFakeMultiplayer ? this.Player.Character.Title.GetFormattedText() : "PlayerId " + this.Player.NetId.ToString();
    }
  }

  public bool IsMonster => this.Monster != null;

  public bool IsPlayer => this.Player != null;

  public HpDisplay HpDisplay { get; set; }

  public Player? PetOwner
  {
    get => this._petOwner;
    set
    {
      this._petOwner = this._petOwner == null ? value : throw new InvalidOperationException($"Pet {this} already has an owner.");
    }
  }

  public bool IsPet => this.PetOwner != null;

  public IReadOnlyList<Creature> Pets
  {
    get
    {
      return this.Player?.PlayerCombatState?.Pets ?? (IReadOnlyList<Creature>) Array.Empty<Creature>();
    }
  }

  public bool IsAlive => this.CurrentHp > 0;

  public bool IsDead => !this.IsAlive;

  public string? SlotName { get; set; }

  public IEnumerable<IHoverTip> HoverTips
  {
    get
    {
      if (!CombatManager.Instance.IsInProgress)
        return (IEnumerable<IHoverTip>) Array.Empty<IHoverTip>();
      List<IHoverTip> tips = new List<IHoverTip>();
      if (this.IsMonster)
      {
        foreach (AbstractIntent intent in (IEnumerable<AbstractIntent>) this.Monster.NextMove.Intents)
        {
          if (intent.HasIntentTip)
            tips.Add((IHoverTip) intent.GetHoverTip((IEnumerable<Creature>) this.CombatState.Allies, this));
        }
      }
      foreach (PowerModel power in this._powers)
      {
        foreach (IHoverTip hoverTip in power.HoverTips)
          tips.MegaTryAddingTip(hoverTip);
      }
      return (IEnumerable<IHoverTip>) tips;
    }
  }

  public bool IsEnemy => this.Side == CombatSide.Enemy;

  public bool IsPrimaryEnemy => this.Side == CombatSide.Enemy && !this.IsSecondaryEnemy;

  public bool IsSecondaryEnemy
  {
    get
    {
      return this.Side == CombatSide.Enemy && this.Powers.Any<PowerModel>((Func<PowerModel, bool>) (p => p.OwnerIsSecondaryEnemy));
    }
  }

  public bool IsHittable => !this.IsDead && Hook.ShouldAllowHitting(this.CombatState, this);

  public bool CanReceivePowers
  {
    get => this.CombatState != null && Hook.ShouldAllowHitting(this.CombatState, this);
  }

  public bool IsStunned => this.Monster?.NextMove.Id == "STUNNED";

  public Creature(MonsterModel monster, CombatSide side, string? slotName)
  {
    monster.AssertMutable();
    int minInitialHp = monster.MinInitialHp;
    int maxInitialHp = monster.MaxInitialHp;
    if (minInitialHp > maxInitialHp)
      throw new InvalidOperationException($"{monster.Id.Entry} has min HP {minInitialHp} greater than its max {maxInitialHp}!");
    this.Monster = monster;
    this.Monster.Creature = this;
    this.SlotName = slotName;
    this._maxHp = maxInitialHp;
    this._currentHp = maxInitialHp;
    this.Side = side;
  }

  public Creature(Player player, int currentHp, int maxHp)
  {
    this.Player = player;
    this._currentHp = currentHp;
    this._maxHp = maxHp;
    this.Side = CombatSide.Player;
  }

  public void SetUniqueMonsterHpValue(IReadOnlyList<Creature> creaturesOnSide, Rng rng)
  {
    int num1 = this.Monster != null ? this.Monster.MinInitialHp : throw new InvalidOperationException("Can't set unique monster HP value for a player.");
    int maxExclusive = this.Monster.MaxInitialHp + 1;
    HashSet<int> hashSet = Enumerable.Range(num1, maxExclusive - num1).ToHashSet<int>();
    // ISSUE: object of a compiler-generated type is created
    hashSet.ExceptWith(creaturesOnSide.Except<Creature>((IEnumerable<Creature>) new \u003C\u003Ez__ReadOnlySingleElementList<Creature>(this)).Select<Creature, int>((Func<Creature, int>) (e => e.MaxHp)));
    int num2 = hashSet.Count <= 0 ? rng.NextInt(num1, maxExclusive) : rng.NextItem<int>((IEnumerable<int>) hashSet);
    this._maxHp = num2;
    this._currentHp = num2;
    this.MonsterMaxHpBeforeModification = new int?(num2);
  }

  public void ScaleMonsterHpForMultiplayer(EncounterModel? encounter, int playerCount, int actIndex)
  {
    if (playerCount == 1)
      return;
    this.SetMaxHpInternal(Creature.ScaleHpForMultiplayer((Decimal) this.MaxHp, encounter, playerCount, actIndex));
    this.SetCurrentHpInternal((Decimal) this.MaxHp);
  }

  public NCreatureVisuals? CreateVisuals()
  {
    if (TestMode.IsOn)
      return (NCreatureVisuals) null;
    if (this.Player != null)
      return this.Player.Character.CreateVisuals();
    return this.Monster != null ? this.Monster.CreateVisuals() : throw new InvalidOperationException("Creature and Monster should never both be null.");
  }

  public async Task AfterAddedToRoom()
  {
    if (this.Side != CombatSide.Enemy)
      return;
    await this.Monster.AfterAddedToRoom();
  }

  public Decimal DamageBlockInternal(Decimal amount, ValueProp props)
  {
    Decimal num = props.HasFlag((Enum) ValueProp.Unblockable) ? 0M : Math.Min((Decimal) this.Block, amount);
    this.Block -= (int) num;
    return num;
  }

  public DamageResult LoseHpInternal(Decimal amount, ValueProp props)
  {
    bool flag = this.CurrentHp > 0 && amount >= (Decimal) this.CurrentHp;
    int currentHp = this.CurrentHp;
    int num = (int) Math.Clamp(amount, 0M, 999999999M);
    this.CurrentHp = Math.Max(this.CurrentHp - num, 0);
    return new DamageResult(this, props)
    {
      UnblockedDamage = currentHp - this.CurrentHp,
      WasTargetKilled = flag,
      OverkillDamage = flag ? Math.Max(num - currentHp, 0) : 0
    };
  }

  public void GainBlockInternal(Decimal amount)
  {
    this.Block = !(amount < 0M) ? (int) Math.Min((Decimal) this.Block + amount, 999999999M) : throw new ArgumentException("amount must be positive. Use LoseBlock for block loss.");
  }

  public void LoseBlockInternal(Decimal amount)
  {
    this.Block = !(amount < 0M) ? (int) Math.Max((Decimal) this.Block - amount, 0M) : throw new ArgumentException("amount must be positive. Use GainBlock for block gain.");
  }

  public void HealInternal(Decimal amount)
  {
    bool isDead = this.IsDead;
    this.SetCurrentHpInternal((Decimal) this.CurrentHp + amount);
    if (!isDead || this.IsDead)
      return;
    this.Player?.ActivateHooks();
    Action<Creature> revived = this.Revived;
    if (revived == null)
      return;
    revived(this);
  }

  public void SetCurrentHpInternal(Decimal amount)
  {
    this.CurrentHp = (int) Math.Min(amount, (Decimal) this.MaxHp);
  }

  public void SetMaxHpInternal(Decimal amount)
  {
    this.MaxHp = !(amount < 0M) ? Math.Min((int) amount, 999999999) : throw new ArgumentException("amount must be non-negative.");
    this.CurrentHp = Math.Min(this.CurrentHp, this.MaxHp);
  }

  public void Reset()
  {
    this.RemoveAllPowersInternalExcept();
    this.Block = 0;
  }

  public void InvokeDiedEvent()
  {
    Action<Creature> died = this.Died;
    if (died == null)
      return;
    died(this);
  }

  public void StunInternal(Func<IReadOnlyList<Creature>, Task> stunMove, string? nextMoveId)
  {
    if (this.Monster == null)
      throw new InvalidOperationException("Can't stun a player.");
    if (this.CombatState == null || this.IsDead)
      return;
    if (string.IsNullOrEmpty(nextMoveId))
      nextMoveId = this.Monster.MoveStateMachine.StateLog.Last<MonsterState>().Id;
    this.Monster.SetMoveImmediate(new MoveState("STUNNED", stunMove, new AbstractIntent[1]
    {
      (AbstractIntent) new StunIntent()
    })
    {
      FollowUpStateId = nextMoveId,
      MustPerformOnceBeforeTransitioning = true
    });
  }

  public void PrepareForNextTurn(IEnumerable<Creature> targets, bool rollNewMove = true)
  {
    Creature[] array = targets.ToArray<Creature>();
    if (rollNewMove && this.Monster.MoveStateMachine != null)
      this.Monster.RollMove((IEnumerable<Creature>) array);
    NCreature creatureNode = NCombatRoom.Instance?.GetCreatureNode(this);
    if (creatureNode == null)
      return;
    TaskHelper.RunSafely(creatureNode.RefreshIntents());
  }

  public IReadOnlyList<PowerModel> Powers => (IReadOnlyList<PowerModel>) this._powers;

  public bool HasPower<T>() where T : PowerModel
  {
    return this._powers.Any<PowerModel>((Func<PowerModel, bool>) (p => p is T));
  }

  public bool HasPower(ModelId id)
  {
    return this._powers.Any<PowerModel>((Func<PowerModel, bool>) (p => p.Id == id));
  }

  public T? GetPower<T>() where T : PowerModel
  {
    return this._powers.FirstOrDefault<PowerModel>((Func<PowerModel, bool>) (p => p is T)) as T;
  }

  public PowerModel? GetPower(ModelId id)
  {
    return this._powers.FirstOrDefault<PowerModel>((Func<PowerModel, bool>) (p => p.Id == id));
  }

  public IEnumerable<T> GetPowerInstances<T>() where T : PowerModel => this._powers.OfType<T>();

  public IEnumerable<PowerModel> GetPowerInstances(ModelId id)
  {
    return this._powers.Where<PowerModel>((Func<PowerModel, bool>) (p => p.Id == id));
  }

  public PowerModel? GetPowerById(ModelId id)
  {
    return this._powers.FirstOrDefault<PowerModel>((Func<PowerModel, bool>) (p => p.Id == id));
  }

  public int GetPowerAmount<T>() where T : PowerModel
  {
    // ISSUE: variable of a boxed type
    __Boxed<T> power = (object) this.GetPower<T>();
    return power == null ? 0 : power.Amount;
  }

  public void ApplyPowerInternal(PowerModel power)
  {
    if (power.Owner != this)
      throw new InvalidOperationException("ONLY CALL THIS FROM PowerModel.ApplyInternal!");
    if (power.InstanceType == PowerInstanceType.None && this._powers.Any<PowerModel>((Func<PowerModel, bool>) (p => p.GetType() == power.GetType())))
      throw new InvalidOperationException("Trying to add multiple instances of a non-instanced power to a creature.");
    this._powers.Add(power);
    Action<PowerModel> powerApplied = this.PowerApplied;
    if (powerApplied == null)
      return;
    powerApplied(power);
  }

  public void InvokePowerModified(PowerModel power, int change, bool silent)
  {
    if (change > 0)
    {
      Action<PowerModel, int, bool> powerIncreased = this.PowerIncreased;
      if (powerIncreased == null)
        return;
      powerIncreased(power, change, silent);
    }
    else if (power.StackType.Equals((object) PowerStackType.Counter) && power.AllowNegative && change < 0)
    {
      Action<PowerModel, int, bool> powerIncreased = this.PowerIncreased;
      if (powerIncreased == null)
        return;
      powerIncreased(power, change, silent);
    }
    else
    {
      Action<PowerModel, bool> powerDecreased = this.PowerDecreased;
      if (powerDecreased == null)
        return;
      powerDecreased(power, silent);
    }
  }

  public void RemovePowerInternal(PowerModel power)
  {
    if (power.Owner != this)
      throw new InvalidOperationException("ONLY CALL THIS FROM PowerModel.RemoveInternal!");
    this._powers.Remove(power);
    Action<PowerModel> powerRemoved = this.PowerRemoved;
    if (powerRemoved == null)
      return;
    powerRemoved(power);
  }

  public IEnumerable<PowerModel> RemoveAllPowersInternalExcept(IEnumerable<PowerModel>? except = null)
  {
    List<PowerModel> list = this._powers.Except<PowerModel>((IEnumerable<PowerModel>) ((object) except ?? (object) Array.Empty<PowerModel>())).ToList<PowerModel>();
    foreach (PowerModel powerModel in list)
      powerModel.RemoveInternal();
    return (IEnumerable<PowerModel>) list;
  }

  public IEnumerable<PowerModel> RemoveAllPowersAfterDeath()
  {
    return this.RemoveAllPowersInternalExcept(this._powers.Where<PowerModel>((Func<PowerModel, bool>) (p => !p.ShouldPowerBeRemovedAfterOwnerDeath() || !Hook.ShouldPowerBeRemovedOnDeath(p))));
  }

  public void BeforeTurnStart(CombatSide side)
  {
    foreach (PowerModel power in this._powers)
      power.AmountOnTurnStart = power.Amount;
  }

  public async Task AfterTurnStart(CombatSide side)
  {
    if (side == CombatSide.Player)
    {
      Player player = this.Player;
      if ((player != null ? (player.PlayerCombatState?.TurnNumber.GetValueOrDefault() == 1 ? 1 : 0) : 0) != 0)
        return;
    }
    await this.ClearBlock();
  }

  public void OnSideSwitch()
  {
    if (this.IsPlayer)
      this.Player.OnSideSwitch();
    else
      this.Monster.OnSideSwitch();
  }

  public async Task TakeTurn()
  {
    if (!this.IsMonster || this.Side != CombatSide.Enemy)
      throw new InvalidOperationException("Only enemy monsters can take automated turns.");
    if (this.Monster.SpawnedThisTurn)
      return;
    await this.Monster.PerformMove();
  }

  private async Task ClearBlock()
  {
    AbstractModel preventer;
    if (Hook.ShouldClearBlock(this.CombatState, this, out preventer))
      this.Block = 0;
    else
      await Hook.AfterPreventingBlockClear(this.CombatState, preventer, this);
  }

  public override string ToString() => "Creature " + this.LogName;

  public double GetHpPercentRemaining() => (double) this._currentHp / (double) this._maxHp;

  public static Decimal ScaleHpForMultiplayer(
    Decimal hp,
    EncounterModel? encounter,
    int playerCount,
    int actIndex)
  {
    return playerCount <= 1 ? hp : hp * (Decimal) playerCount * MultiplayerScalingModel.GetMultiplayerScaling(encounter, actIndex);
  }

  public NCreature? GetCreatureNode()
  {
    if (TestMode.IsOn)
      return (NCreature) null;
    NCreature creatureNode = NCombatRoom.Instance?.GetCreatureNode(this);
    if (creatureNode != null)
      return creatureNode;
    return NBestiary.Instance?.GetCreatureNode(this);
  }

  public void SetNodeVisible(bool visible)
  {
    NCreature creatureNode = this.GetCreatureNode();
    if (creatureNode == null)
      return;
    ((CanvasItem) creatureNode).Visible = visible;
  }

  public Control? GetVfxContainer()
  {
    if (TestMode.IsOn)
      return (Control) null;
    if (NCombatRoom.Instance?.GetCreatureNode(this) != null)
      return NCombatRoom.Instance.CombatVfxContainer;
    return NBestiary.Instance?.GetCreatureNode(this) != null ? NBestiary.Instance.VfxContainer : (Control) null;
  }

  public Control? GetBackVfxContainer()
  {
    if (TestMode.IsOn)
      return (Control) null;
    if (NCombatRoom.Instance?.GetCreatureNode(this) != null)
      return NCombatRoom.Instance.BackCombatVfxContainer;
    return NBestiary.Instance?.GetCreatureNode(this) != null ? NBestiary.Instance.BackVfxContainer : (Control) null;
  }
}
