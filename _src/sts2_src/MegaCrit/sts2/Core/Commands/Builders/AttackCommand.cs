// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Commands.Builders.AttackCommand
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using MegaCrit.Sts2.Core.Audio.Debug;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Hooks;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Monsters;
using MegaCrit.Sts2.Core.ValueProps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Commands.Builders;

public class AttackCommand
{
  private readonly Decimal _damagePerHit;
  private readonly CalculatedDamageVar? _calculatedDamageVar;
  private int _hitCount = 1;
  private AttackCommand.SourceType _sourceType;
  private ICombatState? _combatState;
  private Creature? _singleTarget;
  private bool _spawnVfxOnEachCreature;
  private bool _spawnVfxOnCreatureCenter = true;
  private bool _doesRandomTargetingAllowDuplicates = true;
  private bool _shouldPlayAnimation = true;
  private readonly List<List<DamageResult>> _results = new List<List<DamageResult>>();
  private string? _attackerAnimName;
  private float _attackerAnimDelay;
  private Creature? _visualAttacker;
  private bool _playOnEveryHit = true;
  private string? _attackerVfx;
  private string? _attackerSfx;
  private string? _tmpAttackerSfx;
  private readonly float[] _waitBeforeHit = new float[2]
  {
    -1f,
    -1f
  };
  private readonly List<Func<Node2D?>> _customAttackerVfxNodes = new List<Func<Node2D>>();
  private readonly List<Func<Creature, Node2D?>> _customHitVfxNodes = new List<Func<Creature, Node2D>>();
  private Func<Task>? _afterAttackerAnim;
  private Func<Task>? _beforeDamage;

  public Creature? Attacker { get; private set; }

  public AbstractModel? ModelSource { get; private set; }

  public CardPlay? CardPlay { get; private set; }

  private IReadOnlyList<Creature> GetPossibleTargets()
  {
    if (this.IsSingleTargeted)
    {
      // ISSUE: object of a compiler-generated type is created
      return (IReadOnlyList<Creature>) new \u003C\u003Ez__ReadOnlySingleElementList<Creature>(this._singleTarget);
    }
    if (!this.IsMultiTargeted)
      throw new InvalidOperationException("No targets set, a Targeting method must be called before Execute");
    if (this._sourceType == AttackCommand.SourceType.Monster)
      return this._combatState.PlayerCreatures;
    return this.Attacker != null ? this._combatState.GetOpponentsOf(this.Attacker) : throw new InvalidOperationException("We require an attacker to be able to grab its opponents");
  }

  public CombatSide TargetSide { get; private set; }

  public ValueProp DamageProps { get; private set; } = ValueProp.Move;

  public bool IsSingleTargeted => this._singleTarget != null;

  public bool IsMultiTargeted => this._combatState != null;

  public bool IsRandomlyTargeted { get; private set; }

  public IEnumerable<List<DamageResult>> Results => (IEnumerable<List<DamageResult>>) this._results;

  public string? HitSfx { get; private set; }

  public string? TmpHitSfx { get; private set; }

  public string? HitVfx { get; private set; }

  public AttackCommand(Decimal damagePerHit)
  {
    this._damagePerHit = damagePerHit;
    this._calculatedDamageVar = (CalculatedDamageVar) null;
  }

  public AttackCommand(CalculatedDamageVar calculatedDamageVar)
  {
    this._damagePerHit = -1M;
    this._calculatedDamageVar = calculatedDamageVar;
  }

  public AttackCommand FromCard(CardModel card, CardPlay? cardPlay)
  {
    if (this.Attacker != null)
      throw new InvalidOperationException("Attacker has already been set.");
    if (this.ModelSource != null)
      throw new InvalidOperationException("ModelSource has already been set.");
    if (cardPlay != null && this.CardPlay != null)
      throw new InvalidOperationException("CardPlay has already been set.");
    Player owner = card.Owner;
    this.Attacker = owner.Creature;
    this._attackerAnimName = "Attack";
    this._attackerAnimDelay = owner.Character.AttackAnimDelay;
    this.ModelSource = (AbstractModel) card;
    this.CardPlay = cardPlay;
    this._sourceType = AttackCommand.SourceType.Card;
    return this;
  }

  public AttackCommand FromOsty(Creature osty, CardModel card, CardPlay? cardPlay)
  {
    this.Attacker = osty.Monster is Osty ? osty : throw new ArgumentException("Creature is not Osty");
    this.ModelSource = (AbstractModel) card;
    this.CardPlay = cardPlay;
    this._attackerAnimName = "Attack";
    this._attackerAnimDelay = 0.3f;
    this._sourceType = AttackCommand.SourceType.Card;
    return this.WithAttackerFx(sfx: "event:/sfx/characters/osty/osty_attack");
  }

  public AttackCommand FromMonster(MonsterModel monster)
  {
    this.Attacker = this.Attacker == null ? monster.Creature : throw new InvalidOperationException("Attacker has already been set.");
    this._attackerAnimName = "Attack";
    this._sourceType = AttackCommand.SourceType.Monster;
    return this.TargetingAllOpponents(monster.Creature.CombatState);
  }

  public AttackCommand Targeting(Creature target)
  {
    if (this._singleTarget != null)
      throw new InvalidOperationException("Targets already set.");
    if (this._combatState != null)
      throw new InvalidOperationException("Already set to target opponents of attacker");
    this._singleTarget = target;
    this.TargetSide = target.Side;
    return this;
  }

  public AttackCommand TargetingAllOpponents(ICombatState combatState)
  {
    if (this._singleTarget != null)
      throw new InvalidOperationException("Targets already set.");
    if (this._combatState != null)
      throw new InvalidOperationException("Already set to target opponents of attacker");
    if (this.Attacker == null)
      throw new InvalidOperationException("We require an attacker to be able to grab its opponents");
    this._combatState = combatState;
    this.TargetSide = this.Attacker.Side == CombatSide.Enemy ? CombatSide.Player : CombatSide.Enemy;
    return this;
  }

  public AttackCommand TargetingRandomOpponents(ICombatState combatState, bool allowDuplicates = true)
  {
    if (this._singleTarget != null)
      throw new InvalidOperationException("Targets already set.");
    if (this._combatState != null)
      throw new InvalidOperationException("Already set to target opponents of attacker");
    if (this.Attacker == null)
      throw new InvalidOperationException("We require an attacker to be able to grab its opponents");
    this._combatState = combatState;
    this.IsRandomlyTargeted = true;
    this._doesRandomTargetingAllowDuplicates = allowDuplicates;
    return this;
  }

  public AttackCommand Unpowered()
  {
    this.DamageProps |= ValueProp.Unpowered;
    return this;
  }

  public AttackCommand WithAttackerAnim(string? animName, float delay, Creature? visualAttacker = null)
  {
    this._attackerAnimName = this._attackerAnimName != null ? animName : throw new InvalidOperationException("WithAttackerAnim was called before FromCard/FromMonster/FromOsty, should be called after.");
    this._attackerAnimDelay = delay;
    this._visualAttacker = visualAttacker;
    return this;
  }

  public AttackCommand WithNoAttackerAnim()
  {
    this._shouldPlayAnimation = false;
    return this;
  }

  public AttackCommand AfterAttackerAnim(Func<Task> afterAttackerAnim)
  {
    this._afterAttackerAnim = afterAttackerAnim;
    return this;
  }

  public AttackCommand WithAttackerFx(string? vfx = null, string? sfx = null, string? tmpSfx = null)
  {
    this._attackerVfx = vfx;
    this._attackerSfx = sfx;
    this._tmpAttackerSfx = tmpSfx;
    return this;
  }

  public AttackCommand WithAttackerFx(Func<Node2D?> createAttackerVfx)
  {
    this._customAttackerVfxNodes.Add(createAttackerVfx);
    return this;
  }

  public AttackCommand WithWaitBeforeHit(float fastSeconds, float standardSeconds)
  {
    this._waitBeforeHit[0] = fastSeconds;
    this._waitBeforeHit[1] = standardSeconds;
    return this;
  }

  public AttackCommand WithHitFx(string? vfx = null, string? sfx = null, string? tmpSfx = null)
  {
    this.HitVfx = vfx;
    this.HitSfx = sfx;
    this.TmpHitSfx = tmpSfx;
    return this;
  }

  public AttackCommand SpawningHitVfxOnEachCreature()
  {
    this._spawnVfxOnEachCreature = true;
    return this;
  }

  public AttackCommand WithHitVfxSpawnedAtBase()
  {
    this._spawnVfxOnCreatureCenter = false;
    return this;
  }

  public AttackCommand WithHitVfxNode(Func<Creature, Node2D?> createHitVfxNode)
  {
    this._customHitVfxNodes.Add(createHitVfxNode);
    return this;
  }

  public AttackCommand OnlyPlayAnimOnce()
  {
    this._playOnEveryHit = false;
    return this;
  }

  public AttackCommand WithHitCount(int hitCount)
  {
    this._hitCount = hitCount;
    return this;
  }

  public AttackCommand BeforeDamage(Func<Task> beforeDamage)
  {
    this._beforeDamage = beforeDamage;
    return this;
  }

  public static async Task<AttackContext> CreateContextAsync(
    ICombatState combatState,
    PlayerChoiceContext choiceContext,
    CardPlay cardPlay)
  {
    return await AttackContext.CreateAsync(combatState, choiceContext, cardPlay);
  }

  public async Task<AttackCommand> Execute(PlayerChoiceContext? choiceContext)
  {
    ICombatState combatState = this.Attacker?.CombatState;
    if (this.Attacker == null)
      throw new InvalidOperationException("No attacker set.");
    if (CombatManager.Instance.IsOverOrEnding && (combatState == null || combatState.IsLiveCombat()))
      return this;
    if (combatState == null)
      throw new InvalidOperationException("No combat state even though combat is not over.");
    if (this.Attacker.IsDead)
      return this;
    if (!this.IsSingleTargeted && !this.IsMultiTargeted)
      throw new InvalidOperationException("No targets set.");
    await Hook.BeforeAttack(combatState, this);
    Decimal attackCount = Hook.ModifyAttackHitCount(combatState, this, this._hitCount);
    for (int i = 0; (Decimal) i < attackCount && !this.Attacker.IsDead; ++i)
    {
      List<Creature> validTargets = this.GetPossibleTargets().Where<Creature>((Func<Creature, bool>) (c => c.IsAlive)).ToList<Creature>();
      if (validTargets.Count != 0 || !combatState.IsLiveCombat())
      {
        if (this._playOnEveryHit || i == 0)
        {
          if (this._attackerVfx != null)
            VfxCmd.PlayOnCreatureCenter(this.Attacker, this._attackerVfx);
          foreach (Func<Node2D> customAttackerVfxNode in this._customAttackerVfxNodes)
          {
            Control vfxContainer = this.Attacker.GetVfxContainer();
            if (vfxContainer != null)
              ((Node) vfxContainer).AddChildSafely((Node) customAttackerVfxNode());
          }
          if (this._attackerSfx != null)
            SfxCmd.Play(this._attackerSfx);
          else if (this._tmpAttackerSfx != null)
            NDebugAudioManager.Instance?.Play(this._tmpAttackerSfx);
          if (this._attackerAnimName != null && this._shouldPlayAnimation)
            await CreatureCmd.TriggerAnim(this._visualAttacker ?? this.Attacker, this._attackerAnimName, this._attackerAnimDelay);
          if (this._afterAttackerAnim != null)
            await this._afterAttackerAnim();
        }
        if (validTargets.Count > 0)
        {
          if (this.HitSfx != null)
            SfxCmd.Play(this.HitSfx);
          else if (this.TmpHitSfx != null)
            NDebugAudioManager.Instance?.Play(this.TmpHitSfx);
        }
        Creature singleTarget;
        if (this.IsRandomlyTargeted)
        {
          if (!this._doesRandomTargetingAllowDuplicates)
          {
            validTargets = validTargets.Where<Creature>((Func<Creature, bool>) (c => this._results.SelectMany<List<DamageResult>, DamageResult>((Func<List<DamageResult>, IEnumerable<DamageResult>>) (r => (IEnumerable<DamageResult>) r)).All<DamageResult>((Func<DamageResult, bool>) (r => r.Receiver != c)))).ToList<Creature>();
            if (validTargets.Count == 0)
              throw new InvalidOperationException("No valid targets for attack with duplicates disallowed. If you're in a test, you probably need to add more enemies. If you're in real gameplay, something is wrong.");
          }
          singleTarget = (this.Attacker.Player ?? this.Attacker.PetOwner).RunState.Rng.CombatTargets.NextItem<Creature>((IEnumerable<Creature>) validTargets);
        }
        else
          singleTarget = validTargets.Count != 1 ? (Creature) null : validTargets[0];
        if (((IEnumerable<float>) this._waitBeforeHit).Any<float>((Func<float, bool>) (w => (double) w > 0.0)))
          await Cmd.CustomScaledWait(this._waitBeforeHit[0], this._waitBeforeHit[1]);
        foreach (Func<Creature, Node2D> customHitVfxNode in this._customHitVfxNodes)
        {
          if (singleTarget != null)
          {
            Control vfxContainer = singleTarget.GetVfxContainer();
            if (vfxContainer != null)
              ((Node) vfxContainer).AddChildSafely((Node) customHitVfxNode(singleTarget));
          }
          else
          {
            foreach (Creature creature in validTargets)
            {
              Control vfxContainer = creature.GetVfxContainer();
              if (vfxContainer != null)
                ((Node) vfxContainer).AddChildSafely((Node) customHitVfxNode(creature));
            }
          }
        }
        if (this.HitVfx != null)
        {
          if (singleTarget != null)
          {
            if (this._spawnVfxOnCreatureCenter)
              VfxCmd.PlayOnCreatureCenter(singleTarget, this.HitVfx);
            else
              VfxCmd.PlayOnCreature(singleTarget, this.HitVfx);
          }
          else if (this._spawnVfxOnEachCreature)
          {
            if (this._spawnVfxOnCreatureCenter)
              VfxCmd.PlayOnCreatureCenters((IEnumerable<Creature>) validTargets, this.HitVfx);
            else
              VfxCmd.PlayOnCreatures((IEnumerable<Creature>) validTargets, this.HitVfx);
          }
          else
            VfxCmd.PlayOnSide(this.Attacker.Side.GetOppositeSide(), this.HitVfx, combatState);
        }
        if (this._beforeDamage != null)
          await this._beforeDamage();
        Decimal amount = this._calculatedDamageVar == null ? this._damagePerHit : this._calculatedDamageVar.Calculate(singleTarget);
        PlayerChoiceContext choiceContext1 = choiceContext ?? (PlayerChoiceContext) new BlockingPlayerChoiceContext();
        List<Creature> targets;
        if (singleTarget == null)
        {
          targets = validTargets;
        }
        else
        {
          targets = new List<Creature>(1);
          targets.Add(singleTarget);
        }
        this.AddResultsInternal(await CreatureCmd.Damage(choiceContext1, (IEnumerable<Creature>) targets, amount, this.DamageProps, this.Attacker, this.ModelSource as CardModel, this.CardPlay));
        validTargets = (List<Creature>) null;
        singleTarget = (Creature) null;
      }
      else
        break;
    }
    CombatManager.Instance.History.CreatureAttacked(combatState, this.Attacker, (IReadOnlyList<DamageResult>) this._results.SelectMany<List<DamageResult>, DamageResult>((Func<List<DamageResult>, IEnumerable<DamageResult>>) (r => (IEnumerable<DamageResult>) r)).ToList<DamageResult>());
    await Hook.AfterAttack(combatState, choiceContext ?? (PlayerChoiceContext) new BlockingPlayerChoiceContext(), this);
    return this;
  }

  public void IncrementHitsInternal() => ++this._hitCount;

  public void AddResultsInternal(IEnumerable<DamageResult> results)
  {
    this._results.Add(results.ToList<DamageResult>());
  }

  private enum SourceType
  {
    None,
    Card,
    Monster,
  }
}
